$dagname = "ex13dag"

$memberReport = @()		#DAG member server health report

$dags = @(Get-DatabaseAvailabilityGroup $dagname -Status)

foreach ($dag in $dags)
{
$dagmembers = @($dag | Select-Object -ExpandProperty Servers | Sort-Object Name)

    foreach ($dagmember in $dagmembers)
    {
	    $memberObj = New-Object PSObject
	    $memberObj | Add-Member NoteProperty -Name "Server" -Value $($dagmember)
		
	    $replicationhealth = $dagmember | Invoke-Command {Test-ReplicationHealth}
	    foreach ($healthitem in $replicationhealth)
	    {
		    $memberObj | Add-Member NoteProperty -Name $($healthitem.Check) -Value $(if($healthitem.Result -eq "Passed") { "Passed" } else {"Fail"})
	    }
	    $memberReport += $memberObj
    }
}

$memberReport
