# .\EX_DAGHealth_CopySummary.ps1 -replqueuewarning 8
param([int]$replqueuewarning)

[array]$dagsummary = @()	
[array]$dags = @()		
[array]$dagdatabases = @()								#Array for DAG databases
#[int]$replqueuewarning = 8								#Threshold to consider a replication queue unhealthy
$dags = @(Get-DatabaseAvailabilityGroup -Status)

foreach ($dag in $dags)
{
  $dbcopyReport = @()		#Database copy health report 
  $dagdatabases = @(Get-MailboxDatabase -Status | Where-Object {$_.MasterServerOrAvailabilityGroup -eq $dag.Name} | Sort-Object Name)

   foreach ($database in $dagdatabases)
	{
       #Custom object for Database
	   $objectHash = @{
			"Database" = $database.Identity
			"Mountedon" = "Unknown"
			"Preference" = $null
			"TotalCopies" = $null
			"HealthyCopies" = $null
			"UnhealthyCopies" = $null
			"HealthyQueues" = $null
			"UnhealthyQueues" = $null
			"LaggedQueues" = $null
			"HealthyIndexes" = $null
			"UnhealthyIndexes" = $null
			}

       $databaseObj = New-Object PSObject -Property $objectHash
       $dbcopystatus = @($database | Get-MailboxDatabaseCopyStatus)
	   
        foreach ($dbcopy in $dbcopystatus)
		{
           #Custom object for DB copy
			$objectHash = @{
			"DatabaseCopy" = $dbcopy.Identity
			"DatabaseName" = $dbcopy.DatabaseName
			"MailboxServer" = $dbcopy.MailboxServer
			"ActivationPreference" = $null
			"Status" = $dbcopy.Status
			"CopyQueueLength" = $dbcopy.CopyQueueLength
			"ReplayQueueLength" = $dbcopy.ReplayQueueLength
			"ReplayLagged" = $null
			"TruncationLagged" = $null
			"ContentIndex" = $dbcopy.ContentIndexState
            "ActiveCopy" =  $dbcopy.ActiveCopy                      
            }

            $dbcopyObj = New-Object PSObject -Property $objectHash

            $mailboxserver = $dbcopy.MailboxServer

            $pref = ($database | Select-Object -ExpandProperty ActivationPreference | Where-Object {$_.Key -eq $mailboxserver}).Value

            #Checking whether this is a replay lagged copy
			$replaylagcopies = @($database | Select -ExpandProperty ReplayLagTimes | Where-Object {$_.Value -gt 0})
			if ($($replaylagcopies.count) -gt 0)
            {
                [bool]$replaylag = $false
                foreach ($replaylagcopy in $replaylagcopies)
			    {
				    if ($replaylagcopy.Key -eq $mailboxserver)
				    {
					    [bool]$replaylag = $true
				    }
			    }
            }
            else
			{
			   [bool]$replaylag = $false
			}


            #Checking for truncation lagged copies
			$truncationlagcopies = @($database | Select -ExpandProperty TruncationLagTimes | Where-Object {$_.Value -gt 0})
			if ($($truncationlagcopies.count) -gt 0)
	        {
	            [bool]$truncatelag = $false
	            foreach ($truncationlagcopy in $truncationlagcopies)
				{
					if ($truncationlagcopy.Key -eq $mailboxserver)
					{							
						[bool]$truncatelag = $true
					}
				}
	        }
	        else
			{
				[bool]$truncatelag = $false
			}

            $dbcopyObj | Add-Member NoteProperty -Name "ActivationPreference" -Value $pref -Force
            $dbcopyObj | Add-Member NoteProperty -Name "ReplayLagged" -Value $replaylag -Force
			$dbcopyObj | Add-Member NoteProperty -Name "TruncationLagged" -Value $truncatelag -Force

         $dbcopyReport += $dbcopyObj

        }	

        $copies = @($dbcopyReport | Where-Object { ($_."DatabaseName" -eq $database) })
		
		$mountedOn = ($copies | Where-Object { ($_.Status -eq "Mounted") })."MailboxServer"
		if ($mountedOn)
		{
			$databaseObj | Add-Member NoteProperty -Name "Mountedon" -Value $mountedOn -Force
		}
		
		$activationPref = ($copies | Where-Object { ($_.Status -eq "Mounted") })."ActivationPreference"
		$databaseObj | Add-Member NoteProperty -Name "Preference" -Value $activationPref -Force

		$totalcopies = $copies.count
		$databaseObj | Add-Member NoteProperty -Name "TotalCopies" -Value $totalcopies -Force
		
		$healthycopies = @($copies | Where-Object { (($_.Status -eq "Mounted") -or ($_.Status -eq "Healthy")) }).Count
		$databaseObj | Add-Member NoteProperty -Name "HealthyCopies" -Value $healthycopies -Force
			
		$unhealthycopies = @($copies | Where-Object { (($_.Status -ne "Mounted") -and ($_.Status -ne "Healthy")) }).Count
		$databaseObj | Add-Member NoteProperty -Name "UnhealthyCopies" -Value $unhealthycopies -Force

		$healthyqueues  = @($copies | Where-Object { (($_."CopyQueueLength" -lt $replqueuewarning) -and (($_."ReplayQueueLength" -lt $replqueuewarning)) -and ($_."ReplayLagged" -eq $false)) }).Count
	    $databaseObj | Add-Member NoteProperty -Name "HealthyQueues" -Value $healthyqueues -Force

		$unhealthyqueues = @($copies | Where-Object { (($_."CopyQueueLength" -ge $replqueuewarning) -or (($_."ReplayQueueLength" -ge $replqueuewarning) -and ($_."ReplayLagged" -eq $false))) }).Count
		$databaseObj | Add-Member NoteProperty -Name "UnhealthyQueues" -Value $unhealthyqueues -Force

		$laggedqueues = @($copies | Where-Object { ($_."ReplayLagged" -eq $true) -or ($_."TruncationLagged" -eq $true) }).Count
		$databaseObj | Add-Member NoteProperty -Name "LaggedQueues" -Value $laggedqueues -Force

		$healthyindexes = @($copies | Where-Object { ($_."ContentIndex" -eq "Healthy") }).Count
		$databaseObj | Add-Member NoteProperty -Name "HealthyIndexes" -Value $healthyindexes -Force
			
		$unhealthyindexes = @($copies | Where-Object { ($_."ContentIndex" -ne "Healthy") }).Count
		$databaseObj | Add-Member NoteProperty -Name "UnhealthyIndexes" -Value $unhealthyindexes -Force
			
		$dagsummary += $databaseObj

    }
   
}
$dagsummary

#(Get-DatabaseAvailabilityGroup) | ForEach {$_.Servers | ForEach {Get-MailboxDatabaseCopyStatus -Server $_}}