[array]$dagdatabases = @()								#Array for DAG databases
$dags = @(Get-DatabaseAvailabilityGroup $dagname -Status)

foreach ($dag in $dags)
{
  $dbcopyReport = @()		#Database copy health report 
  $dagdatabases = @(Get-MailboxDatabase -Status | Where-Object {$_.MasterServerOrAvailabilityGroup -eq $dag.Name} | Sort-Object Name)

   foreach ($database in $dagdatabases)
	{
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
			"Content Index" = $dbcopy.ContentIndexState
            "Active Copy" =  $dbcopy.ActiveCopy                      
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
    }
    $dbcopyReport
}


#(Get-DatabaseAvailabilityGroup) | ForEach {$_.Servers | ForEach {Get-MailboxDatabaseCopyStatus -Server $_}}