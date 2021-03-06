clear
#.\MailBoxReportScript.ps1 -server win-kurrr2qsdj0
#param(
#	[Parameter(ParameterSetName='server')] [string]$server
#)
param($server)
#$server = 'win-kurrr2qsdj0.jnittech.com'
#...................................
# Variables
#...................................

$now = Get-Date											#Used for timestamps
$date = $now.ToShortDateString()						#Short date format for email message subject
[array]$exchangeservers = @()							#Array for the Exchange server or servers to check
[array]$serversummary = @()								#Summary of issues found during server health checks
[int]$transportqueuehigh = 100							#Change this to set transport queue high threshold
[int]$transportqueuewarn = 80							#Change this to set transport queue warning threshold
$mapitimeout = 10
$ip = $null
[array]$serversummary = @()								#Summary of issues found during server health checks
[array]$dagsummary = @()								#Summary of issues found during DAG health checks
[array]$report = @()
[array]$dags = @()										#Array for DAG health check
[array]$dagdatabases = @()								#Array for DAG databases
[int]$replqueuewarning = 8								#Threshold to consider a replication queue unhealthy
$dagreportbody = $null


#...................................
# Error/Warning Strings
#...................................
$uptimeUnableToRetrive = "Unable to retrieve uptime."
$testServerHealthError = "Could not test service health. "
$alternativeTestForExchg2003CASOnly = "Using alternative test for Exchange 2013 CAS-only server"
$requiredServicesNotRunning = "required services not running. "
$checkHubTransportService = "Checking Hub Transport Server"
$checkQueueError = "Could not check queue. "
$checkMailBoxServer = "Checking Mailbox Server"
$checkPublicFolderDB = "Checking public folder database"
$publicFolderStatus = "Public folder database status is"
$checkMapiConnectivity = "Checking MAPI connectivity"
$mapiConnStatusIs = "MAPI connectivity status is"
$skipped = "Skipped"
$checkMailFlow = "Checking mail flow"
$mailFlowStatusIs = "Mail flow status is"
$noActiveDBs = "No active DBs"
$serverNotReachable = "Server is not reachable. "
$serverNotFoundinDns = "Server not found in DNS. "
$finshedCheckServer = "Finished checking server"

#...................................
# Functions
#...................................

#region Set of functions

#This function is used to write the log file if -Log is used
Function Write-Logfile()
{
	param( $logentry )
	$timestamp = Get-Date -DisplayHint Time
    #Implement log accordingly
	#"$timestamp $logentry" | Out-File $logfile -Append
}

#This function is used to test service health for Exchange 2013 CAS-only servers
Function Test-E15CASServiceHealth()
{
	param ( $e15cas )
	
	$e15casservicehealth = $null
	$servicesrunning = @()
	$servicesnotrunning = @()
	$casservices = @(
		"IISAdmin",
		"W3Svc",
		"WinRM",
		"MSExchangeADTopology",
		"MSExchangeDiagnostics",
		"MSExchangeFrontEndTransport",
		#"MSExchangeHM",
		"MSExchangeIMAP4",
		"MSExchangePOP3",
		"MSExchangeServiceHost",
		"MSExchangeUMCR"
		)
		
	try {
		$servicestates = @(Get-WmiObject -ComputerName $e15cas -Class Win32_Service -ErrorAction STOP | where {$casservices -icontains $_.Name} | select name,state,startmode)
	}
	catch
	{
		#log and warn
		$e15casservicehealth = "Fail"
	}	
	
	if (!($e15casservicehealth))
	{
		$servicesrunning = @($servicestates | Where {$_.StartMode -eq "Auto" -and $_.State -eq "Running"})
		$servicesnotrunning = @($servicestates | Where {$_.Startmode -eq "Auto" -and $_.State -ne "Running"})
		if ($($servicesnotrunning.Count) -gt 0)
		{
			#Write-Verbose "Service health check failed"
		    #Write-Verbose "Services not running:"
		    #foreach ($service in $servicesnotrunning)
		    #{
		    #    Write-Verbose "- $($service.Name)"	
		    #}
			$e15casservicehealth = "Fail"	
		}
		else
		{
			#Write-Verbose "Service health check passed"
			$e15casservicehealth = "Pass"
		}
	}
	return $e15casservicehealth
}

#endregion
#...................................
# Script
#...................................

try
{
    [bool]$NoDAG = $true
	$exchangeservers = Get-ExchangeServer $server -ErrorAction STOP
    
    #If List of Servers were given
    #$tmpservers = @(Get-Content $serverlist -ErrorAction STOP)
	#$exchangeservers = @($tmpservers | Get-ExchangeServer)
    #If there is a ignore server list also implement by looping in through the list
    
}
catch
{
	# Error Log
	EXIT
}
        

$ip = @([System.Net.Dns]::GetHostByName($server).AddressList | Select-Object IPAddressToString -ExpandProperty IPAddressToString)

foreach ($server in $exchangeservers)
{
    $server = $server.OriginatingServer
	#Find out some details about the server
	try
	{
		$serverinfo = Get-ExchangeServer $server -ErrorAction Stop
	}
	catch
	{
		#Log and warning
		$serverinfo = $null
	}
    
    if ($serverinfo -eq $null )
	{
		#Server is not an Exchange server
		#log
	}
	elseif ( $serverinfo.IsEdgeServer )
	{
		#log
	}
	else
	{    
    	#Server is an Exchange server, continue the health check

		#Custom object properties
        $serverObj = New-Object PSObject
        $serverObj | Add-Member -MemberType NoteProperty -Name "DNS" -Value $null
        $serverObj | Add-Member NoteProperty -Name "Ping" -Value $null
        $serverObj | Add-Member NoteProperty -Name "Uptime (hrs)" -Value $null
        $serverObj | Add-Member NoteProperty -Name "Version" -Value $null
        $serverObj | Add-Member NoteProperty -Name "Roles" -Value $null
		$serverObj | Add-Member NoteProperty -Name "Client Access Server Role Services" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "Hub Transport Server Role Services" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "Mailbox Server Role Services" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "Unified Messaging Server Role Services" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "Transport Queue" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "Queue Length" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "PF DBs Mounted" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "MB DBs Mounted" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "Mail Flow Test" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "MAPI Test" -Value "n/a"
        
        #Check server name resolves in DNS
        if ( $ip -ne $null )
        {
            $serverObj | Add-Member NoteProperty -Name "DNS" -Value "Pass" -Force
          
            #Is server online "Ping Check: " 
            $ping = $null
            try
            {
        	   $ping = Test-Connection $server -Quiet -ErrorAction Stop
            }
            catch
            {
            }
          
          switch ($ping)
          {
            $true {
             $serverObj | Add-Member NoteProperty -Name "Ping" -Value "Pass" -Force
            }
            default {
             $serverObj | Add-Member NoteProperty -Name "Ping" -Value "Fail" -Force
            }
          }
          
          #Uptime check, even if ping fails
          [int]$uptime = $null
          $laststart = $null
          
          try 
          {
        	$laststart = [System.Management.ManagementDateTimeconverter]::ToDateTime((Get-WmiObject -Class Win32_OperatingSystem -computername $server -ErrorAction Stop).LastBootUpTime)
          }
          catch
          {
          }
          
          #Uptime hrs
          if ($laststart -eq $null)
           {
        	# log or do something as for formating
             [string]$uptime = $uptimeUnableToRetrive 
           }
          else
           {
        	 [int]$timespan = (New-TimeSpan $laststart $now).TotalHours
        	 [int]$uptime = "{0:N0}" -f $timespan     
           }
          $serverObj | Add-Member NoteProperty -Name "Uptime (hrs)" -Value $uptime -Force
          
          if ($ping -or ($uptime -ne $uptimeUnableToRetrive))
           {
                #Determine the friendly version number
				$ExVer = $serverinfo.AdminDisplayVersion            
             
        		if ($ExVer -like "Version 6.*")
				{
					$version = "Exchange 2003"
				}
				
				if ($ExVer -like "Version 8.*")
				{
					$version = "Exchange 2007"
				}
				
				if ($ExVer -like "Version 14.*")
				{
					$version = "Exchange 2010"
				}
				
				if ($ExVer -like "Version 15.*")
				{
					$version = "Exchange 2013"
				}
                
                $serverObj | Add-Member NoteProperty -Name "Version" -Value $version -Force
                
                if ($version -eq "Exchange 2003")
				{
					# Log.
				}
                #START - Exchange 2013/2010/2007 Health Checks
				if ($version -ne "Exchange 2003")
				{
					Write-Host "Roles:" $serverinfo.ServerRole
                    $serverObj | Add-Member NoteProperty -Name "Roles" -Value $serverinfo.ServerRole -Force
					
					$IsEdge = $serverinfo.IsEdgeServer		
					$IsHub = $serverinfo.IsHubTransportServer
					$IsCAS = $serverinfo.IsClientAccessServer
					$IsMB = $serverinfo.IsMailboxServer  
                    #START - General Server Health Check
					#Skipping Edge Transports for the general health check, as firewalls usually get
					#in the way. If you want to include them, remove this If.
					if ($IsEdge -ne $true)
					{
                        $servicehealth = @()
						$e15casservicehealth = @()
						try {
							$servicehealth = @(Test-ServiceHealth $server -ErrorAction Stop)
						}
						catch {
							#Workaround for Test-ServiceHealth problem with CAS-only Exchange 2013 servers
							#More info: http://exchangeserverpro.com/exchange-2013-test-servicehealth-error/
							if ($_.Exception.Message -like "*There are no Microsoft Exchange 2007 server roles installed*")
							{
								if ($Log) {Write-Logfile $alternativeTestForExchg2003CASOnly}
								$e15casservicehealth = Test-E15CASServiceHealth($server)
							}
							else
							{
								$serversummary += "$server - $testServerHealthError"
								if ($Log) {Write-Logfile $_.Exception}
	                            $serverObj | Add-Member NoteProperty -Name "Client Access Server Role Services" -Value $testServerHealthError -Force
			                    $serverObj | Add-Member NoteProperty -Name "Hub Transport Server Role Services" -Value $testServerHealthError -Force
			                    $serverObj | Add-Member NoteProperty -Name "Mailbox Server Role Services" -Value $testServerHealthError -Force
			                    $serverObj | Add-Member NoteProperty -Name "Unified Messaging Server Role Services" -Value $testServerHealthError -Force
							}
						}                        	
						if ($servicehealth)
						{
							foreach($s in $servicehealth)
							{
								$roleName = $s.Role
								Write-Host $roleName "Services: " -NoNewline;
															
								switch ($s.RequiredServicesRunning)
								{
									$true {
										$svchealth = "Pass"
										}
									$false {
										$svchealth = "Fail"
										$serversummary += "$server - $rolename $requiredServicesNotRunning"
										}
                                    default {
										$svchealth = "Warn"
										$serversummary += "$server - $rolename $requiredServicesNotRunning"
										}
								}

								switch ($s.Role)
								{
									"Client Access Server Role" { $serverinfoservices = "Client Access Server Role Services" }
									"Hub Transport Server Role" { $serverinfoservices = "Hub Transport Server Role Services" }
									"Mailbox Server Role" { $serverinfoservices = "Mailbox Server Role Services" }
									"Unified Messaging Server Role" { $serverinfoservices = "Unified Messaging Server Role Services" }
								}
								if ($Log) {Write-Logfile "$serverinfoservices status is $svchealth"}	
								$serverObj | Add-Member NoteProperty -Name $serverinfoservices -Value $svchealth -Force
							}
						}
                        
						if ($e15casservicehealth)
						{
							$serverinfoservices = "Client Access Server Role Services"
							if ($Log) {Write-Logfile "$serverinfoservices status is $e15casservicehealth"}
							$serverObj | Add-Member NoteProperty -Name $serverinfoservices -Value $e15casservicehealth -Force
						}
                    }
                    #END - General Server Health Check

					#START - Hub Transport Server Check
					if ($IsHub)
					{
						$q = $null
						if ($Log) {Write-Logfile $checkHubTransportService}
						Write-Host "Total Queue: " -NoNewline; 
						try {
							$q = Get-Queue -server $server -ErrorAction Stop
						}
						catch {
							$serversummary += "$server - $checkQueueError"
							Write-Warning $_.Exception.Message
							if ($Log) {Write-Logfile $checkQueueError}
							if ($Log) {Write-Logfile $_.Exception.Message}
						}
						
						if ($q)
						{
							$qcount = $q | Measure-Object MessageCount -Sum
							[int]$qlength = $qcount.sum
							$serverObj | Add-Member NoteProperty -Name "Queue Length" -Value $qlength -Force
							if ($Log) {Write-Logfile "Queue length is $qlength"}
							if ($qlength -le $transportqueuewarn)
							{
								$serverObj | Add-Member NoteProperty -Name "Transport Queue" -Value "Pass" -Force
							}
							elseif ($qlength -gt $transportqueuewarn -and $qlength -lt $transportqueuehigh)
							{
								$serverObj | Add-Member NoteProperty -Name "Transport Queue" -Value "Warn" -Force
							}
							else
							{
								$serverObj | Add-Member NoteProperty -Name "Transport Queue" -Value "Fail" -Force
							}
						}
						else
						{
							$serverObj | Add-Member NoteProperty -Name "Transport Queue" -Value "Unknown" -Force
						}
					}
					#END - Hub Transport Server Check
                    
                    #START - Mailbox Server Check
					if ($IsMB)
					{
                        if ($Log) {Write-Logfile $checkMailBoxServer}
						
						#Get the PF and MB databases
						[array]$pfdbs = @(Get-PublicFolderDatabase -server $server -status -WarningAction SilentlyContinue)
						[array]$mbdbs = @(Get-MailboxDatabase -server $server -status | Where {$_.Recovery -ne $true})
                        
                        if ($version -ne "Exchange 2007")
                        {
						    [array]$activedbs = @(Get-MailboxDatabase -server $server -status | Where {$_.MountedOnServer -eq ($serverinfo.fqdn)})
                        }
                        else
                        {
                            [array]$activedbs = $mbdbs
                        }
                        
                        #START - Database Mount Check
                        
                        #Check public folder databases
						if ($pfdbs.count -gt 0)
						{
							if ($Log) {Write-Logfile $checkPublicFolderDB}
							Write-Host "Public Folder databases mounted: " -NoNewline;
							[string]$pfdbstatus = "Pass"
							[array]$alertdbs = @()
							foreach ($db in $pfdbs)
							{
								if (($db.mounted) -ne $true)
								{
									$pfdbstatus = "Fail"
									$alertdbs += $db.name
								}
							}

							$serverObj | Add-Member NoteProperty -Name "PF DBs Mounted" -Value $pfdbstatus -Force
							if ($Log) {Write-Logfile "$publicFolderStatus $pfdbstatus"}
						}
                        #END - Database Mount Check
						
						#START - MAPI Connectivity Test
						if ($activedbs.count -gt 0 -or $pfdbs.count -gt 0 -or $version -eq "Exchange 2007")
						{
							[string]$mbdbstatus = "Pass"
							[array]$alertdbs = @()
							if ($Log) {Write-Logfile $checkMapiConnectivity}
							Write-Host "MAPI connectivity: " -NoNewline;
							foreach ($db in $mbdbs)
							{
								$mapistatus = Test-MapiConnectivity -Database $db.Identity -PerConnectionTimeout $mapitimeout
                                if ($mapistatus.Result.Value -eq $null)
                                {
                                    $mapiresult = $mapistatus.Result
                                }
                                else
                                {
                                    $mapiresult = $mapistatus.Result.Value
                                }
                                if (($mapiresult) -ne "Success")
								{
									$mbdbstatus = "Fail"
									$alertdbs += $db.name
								}
							}

							$serverObj | Add-Member NoteProperty -Name "MAPI Test" -Value $mbdbstatus -Force
							if ($Log) {Write-Logfile "$mapiConnStatusIs $mbdbstatus"}
						}
						#END - MAPI Connectivity Test
                        
                        #START - Mail Flow Test
						if ($version -eq "Exchange 2013")
						{
							#Skip mail flow tests for now due to bug
							if ($Log) {Write-Logfile $checkMailFlow}
							Write-Host "Mail flow test: Skipped"
							$serverObj | Add-Member NoteProperty -Name "Mail Flow Test" -Value $skipped -Force
							if ($Log) {Write-Logfile $skipped}
						}
						elseif ($activedbs.count -gt 0 -or ($version -eq "Exchange 2007" -and $mbdbs.count -gt 0))
						{
							$flow = $null
							$testmailflowresult = $null
							
							if ($Log) {Write-Logfile $checkMailFlow}
							Write-Host "Mail flow test: " -NoNewline;
							try
							{
								$flow = Test-Mailflow $server -ErrorAction Stop
							}
							catch
							{
								$testmailflowresult = $_.Exception.Message
								if ($Log) {Write-Logfile $_.Exception.Message}
							}
							
							if ($flow)
							{
								$testmailflowresult = $flow.testmailflowresult
								if ($Log) {Write-Logfile "$mailFlowStatusIs $testmailflowresult"}
							}

							if ($testmailflowresult -eq "Success")
							{
								$serverObj | Add-Member NoteProperty -Name "Mail Flow Test" -Value "Pass" -Force
							}
							else
							{
								$serversummary += "$server - $string11"
								$serverObj | Add-Member NoteProperty -Name "Mail Flow Test" -Value "Fail" -Force
							}
						}
						else
						{
							Write-Host "Mail flow test: No active mailbox databases"
							$serverObj | Add-Member NoteProperty -Name "Mail Flow Test" -Value $noActiveDBs -Force
							if ($Log) {Write-Logfile $noActiveDBs}
						}
						#END - Mail Flow Test
                    } 
                    #END - Mailbox Server Check                                 
                }
                #END - Exchange 2013/2010/2007 Health Checks
				if ($Log) {Write-Logfile "$finshedCheckServer $server"}
           }
           else
			{
				#Server is not reachable and uptime could not be retrieved
				if ($Log) {Write-Logfile $serverNotReachable}
				$serversummary += "$server - $serverNotReachable"
				$serverObj | Add-Member NoteProperty -Name "Ping" -Value "Fail" -Force
				if ($Log) {Write-Logfile "$finshedCheckServer $server"}
			}
        }
        else
		{
			if ($Log) {Write-Logfile $serverNotFoundinDns}
			$serversummary += "$server - $serverNotFoundinDns"
			$serverObj | Add-Member NoteProperty -Name "DNS" -Value "Fail" -Force
			if ($Log) {Write-Logfile "$finshedCheckServer $server"}
			$report = $report + $serverObj
		}
    }
}

###################### Should be modified once the DAGS are available ###############

#Check if -Server or -Serverlist parameter was used, and skip if it was
if (!($NoDAG))
{

	#Get all DAGs
	$tmpdags = @(Get-DatabaseAvailabilityGroup -Status)

	#Remove DAGs in ignorelist
	foreach ($tmpdag in $tmpdags)
	{
		if (!($ignorelist -icontains $tmpdag.name))
		{
			$dags += $tmpdag
		}
	}
}

#if ($($dags.count) -gt 0)
#{
	#foreach ($dag in $dags)
	#{
		$dagdbcopyReport = @()		#Database copy health report
		$dagciReport = @()			#Content Index health report
		$dagmemberReport = @()		#DAG member server health report
		$dagdatabaseSummary = @()	#Database health summary report
		$dagdatabases = @()			#Array of databases in the DAG
        
        #Get all databases in the DAG
		$tmpdatabases = @(Get-MailboxDatabase -Status | Sort-Object Name)
        $dagdatabases = $tmpdatabases
        foreach ($database in $dagdatabases)
		{
            #Custom object for Database
			$objectHash = @{
				"Database" = $database.Identity
				"Mounted on" = "Unknown"
				"Preference" = $null
				"Total Copies" = $null
				"Healthy Copies" = $null
				"Unhealthy Copies" = $null
				"Healthy Queues" = $null
				"Unhealthy Queues" = $null
				"Lagged Queues" = $null
				"Healthy Indexes" = $null
				"Unhealthy Indexes" = $null
				}
			$databaseObj = New-Object PSObject -Property $objectHash
            $dbcopystatus = @($database | Get-MailboxDatabaseCopyStatus)
            
            foreach ($dbcopy in $dbcopystatus)
			{
				#Custom object for DB copy
				$objectHash = @{
					"Database Copy" = $dbcopy.Identity
					"Database Name" = $dbcopy.DatabaseName
					"Mailbox Server" = $null
					"Activation Preference" = $null
					"Status" = $null
					"Copy Queue" = $null
					"Replay Queue" = $null
					"Replay Lagged" = $null
					"Truncation Lagged" = $null
					"Content Index" = $null
					}
				$dbcopyObj = New-Object PSObject -Property $objectHash
                
                $mailboxserver = $dbcopy.MailboxServer
                
                $pref = ($database | Select-Object -ExpandProperty ActivationPreference | Where-Object {$_.Key -eq $mailboxserver}).Value
                
                $copystatus = $dbcopy.Status
                
                [int]$copyqueuelength = $dbcopy.CopyQueueLength
                
                [int]$replayqueuelength = $dbcopy.ReplayQueueLength
                
                $contentindexstate = $dbcopy.ContentIndexState
                
                #Checking whether this is a replay lagged copy
				$replaylagcopies = @($database | Select -ExpandProperty ReplayLagTimes | Where-Object {$_.Value -gt 0})
				if ($($replaylagcopies.count) -gt 0)
	            {
	                [bool]$replaylag = $false
	                foreach ($replaylagcopy in $replaylagcopies)
				    {
					    if ($replaylagcopy.Key -eq $mailboxserver)
					    {
						    $tmpstring = "$database is replay lagged on $mailboxserver"
							if ($Log) {Write-Logfile $tmpstring}
						    [bool]$replaylag = $true
					    }
				    }
	            }
	            else
				{
				   [bool]$replaylag = $false
				}
                
                $tmpstring = "Replay lag is $replaylag"
				if ($Log) {Write-Logfile $tmpstring}
                
                #Checking for truncation lagged copies
				$truncationlagcopies = @($database | Select -ExpandProperty TruncationLagTimes | Where-Object {$_.Value -gt 0})
				if ($($truncationlagcopies.count) -gt 0)
	            {
	                [bool]$truncatelag = $false
	                foreach ($truncationlagcopy in $truncationlagcopies)
				    {
					    if ($truncationlagcopy.Key -eq $mailboxserver)
					    {
						    $tmpstring = "$database is truncate lagged on $mailboxserver"
							if ($Log) {Write-Logfile $tmpstring}							
							[bool]$truncatelag = $true
					    }
				    }
	            }
	            else
				{
				   [bool]$truncatelag = $false
				}
	            $tmpstring = "Truncation lag is $truncatelag"
				if ($Log) {Write-Logfile $tmpstring}	
                
                				$dbcopyObj | Add-Member NoteProperty -Name "Mailbox Server" -Value $mailboxserver -Force
				$dbcopyObj | Add-Member NoteProperty -Name "Activation Preference" -Value $pref -Force
				$dbcopyObj | Add-Member NoteProperty -Name "Status" -Value $copystatus -Force
				$dbcopyObj | Add-Member NoteProperty -Name "Copy Queue" -Value $copyqueuelength -Force
				$dbcopyObj | Add-Member NoteProperty -Name "Replay Queue" -Value $replayqueuelength -Force
				$dbcopyObj | Add-Member NoteProperty -Name "Replay Lagged" -Value $replaylag -Force
				$dbcopyObj | Add-Member NoteProperty -Name "Truncation Lagged" -Value $truncatelag -Force
				$dbcopyObj | Add-Member NoteProperty -Name "Content Index" -Value $contentindexstate -Force
				
				$dagdbcopyReport += $dbcopyObj
            }
            
            $copies = @($dagdbcopyReport | Where-Object { ($_."Database Name" -eq $database) })
		
			$mountedOn = ($copies | Where-Object { ($_.Status -eq "Mounted") })."Mailbox Server"
			if ($mountedOn)
			{
				$databaseObj | Add-Member NoteProperty -Name "Mounted on" -Value $mountedOn -Force
			}
		
			$activationPref = ($copies | Where-Object { ($_.Status -eq "Mounted") })."Activation Preference"
			$databaseObj | Add-Member NoteProperty -Name "Preference" -Value $activationPref -Force

			$totalcopies = $copies.count
			$databaseObj | Add-Member NoteProperty -Name "Total Copies" -Value $totalcopies -Force
		
			$healthycopies = @($copies | Where-Object { (($_.Status -eq "Mounted") -or ($_.Status -eq "Healthy")) }).Count
			$databaseObj | Add-Member NoteProperty -Name "Healthy Copies" -Value $healthycopies -Force
			
			$unhealthycopies = @($copies | Where-Object { (($_.Status -ne "Mounted") -and ($_.Status -ne "Healthy")) }).Count
			$databaseObj | Add-Member NoteProperty -Name "Unhealthy Copies" -Value $unhealthycopies -Force

			$healthyqueues  = @($copies | Where-Object { (($_."Copy Queue" -lt $replqueuewarning) -and (($_."Replay Queue" -lt $replqueuewarning)) -and ($_."Replay Lagged" -eq $false)) }).Count
	        $databaseObj | Add-Member NoteProperty -Name "Healthy Queues" -Value $healthyqueues -Force

			$unhealthyqueues = @($copies | Where-Object { (($_."Copy Queue" -ge $replqueuewarning) -or (($_."Replay Queue" -ge $replqueuewarning) -and ($_."Replay Lagged" -eq $false))) }).Count
			$databaseObj | Add-Member NoteProperty -Name "Unhealthy Queues" -Value $unhealthyqueues -Force

			$laggedqueues = @($copies | Where-Object { ($_."Replay Lagged" -eq $true) -or ($_."Truncation Lagged" -eq $true) }).Count
			$databaseObj | Add-Member NoteProperty -Name "Lagged Queues" -Value $laggedqueues -Force

			$healthyindexes = @($copies | Where-Object { ($_."Content Index" -eq "Healthy") }).Count
			$databaseObj | Add-Member NoteProperty -Name "Healthy Indexes" -Value $healthyindexes -Force
			
			$unhealthyindexes = @($copies | Where-Object { ($_."Content Index" -ne "Healthy") }).Count
			$databaseObj | Add-Member NoteProperty -Name "Unhealthy Indexes" -Value $unhealthyindexes -Force
			
			$dagdatabaseSummary += $databaseObj
        }
    #}
#}
### End the Exchange Server health checks
$serverObj
$dagdbcopyReport
$dagdatabaseSummary
cd ..
