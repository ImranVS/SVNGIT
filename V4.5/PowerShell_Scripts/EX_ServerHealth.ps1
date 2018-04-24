#.\D:\Mukund\E\RPRWyatt\VSTrunk\PowerShell_POC\PSPOC1\bin\Debug\Scripts\EX_ServerHealth.ps1 -server mail.jnittech.com
#param($server)
#$server = 'win-kurrr2qsdj0.jnittech.com'
#...................................
# Variables
#...................................

$now = Get-Date											#Used for timestamps
$date = $now.ToShortDateString()						#Short date format for email message subject
[array]$exchangeservers = @()							#Array for the Exchange server or servers to check
[int]$transportqueuehigh = 100							#Change this to set TransportQueue high threshold
[int]$transportqueuewarn = 80							#Change this to set TransportQueue warning threshold
$mapitimeout = 10
$ip = $null
[System.Boolean]$Log = [System.Boolean]$false

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

$exchangeservers = Get-ExchangeServer $server -ErrorAction STOP

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
        #Custom object properties
        $serverObj = New-Object PSObject
        $serverObj | Add-Member -MemberType NoteProperty -Name "DNS" -Value $null
        $serverObj | Add-Member NoteProperty -Name "Ping" -Value $null
        $serverObj | Add-Member NoteProperty -Name "Uptime" -Value $null
        $serverObj | Add-Member NoteProperty -Name "Version" -Value $null
        $serverObj | Add-Member NoteProperty -Name "Roles" -Value $null
		$serverObj | Add-Member NoteProperty -Name "ClientAccessServerRoleServices" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "HubTransportServerRoleServices" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "MailboxServerRoleServices" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "UnifiedMessagingServerRoleServices" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "TransportQueue" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "QueueLength" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "PFDBsMounted" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "MBDBsMounted" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "MailFlowTest" -Value "n/a"
		$serverObj | Add-Member NoteProperty -Name "MAPITest" -Value "n/a"

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
								if ($Log) {Write-Logfile $_.Exception}
	                            $serverObj | Add-Member NoteProperty -Name "ClientAccessServerRoleServices" -Value $testServerHealthError -Force
			                    $serverObj | Add-Member NoteProperty -Name "HubTransportServerRoleServices" -Value $testServerHealthError -Force
			                    $serverObj | Add-Member NoteProperty -Name "MailboxServerRoleServices" -Value $testServerHealthError -Force
			                    $serverObj | Add-Member NoteProperty -Name "UnifiedMessagingServerRoleServices" -Value $testServerHealthError -Force
							}
						}                        	
						if ($servicehealth)
						{
							foreach($s in $servicehealth)
							{
								$roleName = $s.Role
															
								switch ($s.RequiredServicesRunning)
								{
									$true {
										$svchealth = "Pass"
										}
									$false {
										$svchealth = "Fail"
										}
                                    default {
										$svchealth = "Warn"
										}
								}

								switch ($s.Role)
								{
									"Client Access Server Role" { $serverinfoservices = "ClientAccessServerRoleServices" }
									"Hub Transport Server Role" { $serverinfoservices = "HubTransportServerRoleServices" }
									"Mailbox Server Role" { $serverinfoservices = "MailboxServerRoleServices" }
									"Unified Messaging Server Role" { $serverinfoservices = "UnifiedMessagingServerRoleServices" }
								}
								if ($Log) {Write-Logfile "$serverinfoservices status is $svchealth"}	
								$serverObj | Add-Member NoteProperty -Name $serverinfoservices -Value $svchealth -Force
							}
						}
                        
						if ($e15casservicehealth)
						{
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
						try {
							$q = Get-Queue -server $server -ErrorAction Stop
						}
						catch {
							Write-Warning $_.Exception.Message
							if ($Log) {Write-Logfile $checkQueueError}
							if ($Log) {Write-Logfile $_.Exception.Message}
						}
						
						if ($q)
						{
							$qcount = $q | Measure-Object MessageCount -Sum
							[int]$qlength = $qcount.sum
							$serverObj | Add-Member NoteProperty -Name "QueueLength" -Value $qlength -Force
							if ($Log) {Write-Logfile "QueueLength is $qlength"}
							if ($qlength -le $transportqueuewarn)
							{
								$serverObj | Add-Member NoteProperty -Name "TransportQueue" -Value "Pass" -Force
							}
							elseif ($qlength -gt $transportqueuewarn -and $qlength -lt $transportqueuehigh)
							{
								$serverObj | Add-Member NoteProperty -Name "TransportQueue" -Value "Warn" -Force
							}
							else
							{
								$serverObj | Add-Member NoteProperty -Name "TransportQueue" -Value "Fail" -Force
							}
						}
						else
						{
							$serverObj | Add-Member NoteProperty -Name "TransportQueue" -Value "Unknown" -Force
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

							$serverObj | Add-Member NoteProperty -Name "PFDBsMounted" -Value $pfdbstatus -Force
							if ($Log) {Write-Logfile "$publicFolderStatus $pfdbstatus"}
						}
                        #END - Database Mount Check
						
						#START - MAPI Connectivity Test
						if ($activedbs.count -gt 0 -or $pfdbs.count -gt 0 -or $version -eq "Exchange 2007")
						{
							[string]$mbdbstatus = "Pass"
							[array]$alertdbs = @()
							if ($Log) {Write-Logfile $checkMapiConnectivity}
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

							$serverObj | Add-Member NoteProperty -Name "MAPITest" -Value $mbdbstatus -Force
							if ($Log) {Write-Logfile "$mapiConnStatusIs $mbdbstatus"}
						}
						#END - MAPI Connectivity Test
                        
                        #START - MailFlowTest
						if ($version -eq "Exchange 2013")
						{
							#Skip MailFlowTests for now due to bug
							if ($Log) {Write-Logfile $checkMailFlow}
							$serverObj | Add-Member NoteProperty -Name "MailFlowTest" -Value $skipped -Force
							if ($Log) {Write-Logfile $skipped}
						}
						elseif ($activedbs.count -gt 0 -or ($version -eq "Exchange 2007" -and $mbdbs.count -gt 0))
						{
							$flow = $null
							$testmailflowresult = $null
							
							if ($Log) {Write-Logfile $checkMailFlow}
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
								$serverObj | Add-Member NoteProperty -Name "MailFlowTest" -Value "Pass" -Force
							}
							else
							{
								$serverObj | Add-Member NoteProperty -Name "MailFlowTest" -Value "Fail" -Force
							}
						}
						else
						{
							$serverObj | Add-Member NoteProperty -Name "MailFlowTest" -Value $noActiveDBs -Force
							if ($Log) {Write-Logfile $noActiveDBs}
						}
						#END - MailFlowTest
                    } 
                    #END - Mailbox Server Check                                 
                }
                #END - Exchange 2013/2010/2007 Health Checks
                    
                    }
        }
    }
}

$serverObj