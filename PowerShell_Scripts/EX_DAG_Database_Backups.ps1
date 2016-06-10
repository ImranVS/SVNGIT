
$DagName = 'JNITTECHDAG1'
$now = [DateTime]::Now

$mb = get-MailboxDatabase -Status | Where {$_.MasterType -eq 'DatabaseAvailabilityGroup' -and $_.MasterServerOrAvailabilityGroup -eq $DagName} | Select-Object Name,StorageGroup,
Mounted,BackupInProgress,OnlineMaintenanceInProgress,LastFullBackup,
LastIncrementalBackup,LastDifferentialBackup,LastCopyBackup


foreach($curr in $mb)
{
    if($curr.LastFullBackup -ne $null)
    {
        $curr | Add-Member NoteProperty -Name LastFullBackupDaysAgo -Value ($now - $curr.LastFullBackup).days
    }
    else
    {
        $curr | Add-Member NoteProperty -Name LastFullBackupDaysAgo -Value  'Never'
    }
    if($curr.LastIncrementalBackup -ne $null)
    {
        $curr | Add-Member NoteProperty -Name LastIncrementalBackupDaysAgo -Value ($now - $curr.LastIncrementalBackup).days
    }
    else
    {
        $curr | Add-Member NoteProperty -Name LastIncrementalBackupDaysAgo -Value 'Never'
    }
    if($curr.LastDifferentialBackup -ne $null)
    {
        $curr | Add-Member NoteProperty -Name LastDifferentialBackupDaysAgo -Value ($now - $curr.LastDifferentialBackup).days
    }
    else
    {
        $curr | Add-Member NoteProperty -Name LastDifferentialBackupDaysAgo -Value 'Never'
    }
    if($curr.LastCopyBackup -ne $null)
    {
        $curr | Add-Membe NoteProperty -Namer LastCopyBackupDaysAgo -Value ($now - $curr.LastCopyBackup).days
    }
    else
    {
        $curr | Add-Member NoteProperty -Name LastCopyBackupDaysAgo -Value 'Never'
    }
}

$mb | select * 