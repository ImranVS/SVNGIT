param($Identities)

$TotalWaitTimeInMinutes = 30
$results = @()
$MaxEndTime = (Get-Date).AddMinutes($TotalWaitTimeInMinutes)

Do 
{
    Start-Sleep -Seconds 30
    $results = @()
    $Identities | % { 

        $results += Get-MailboxRepairRequest -Identity $_ 

    }
    
    
}While ((($results | ? {$_.Progress -ne '100'} | Measure-Object).Count -ne 0) -and $MaxEndTime -gt $(Get-Date))

$results | % {
    $DB = (Get-MailboxDatabase $_.Identity.ToString().Substring(0, $_.Identity.ToString().IndexOf('\')))[0]
    $_  | Add-Member -MemberType NoteProperty -Name DatabaseName -Value $DB.Name
    $_  | Add-Member -MemberType NoteProperty -Name ServerName -Value $DB.Server
}

$results | Select DatabaseName, ServerName, CorruptionsDetected, ErrorCode, Corruptions, Progress, JobState 