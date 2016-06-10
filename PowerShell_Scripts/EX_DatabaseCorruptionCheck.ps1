$Identities = @()
$Failed = @()
$Remaining = @()
$results = @()

Get-MailboxDatabase | % { $Identities += (New-MailboxRepairRequest -Database $_.Name -CorruptionType SearchFolder,AggregateCounts,ProvisionedFolder,FolderView -DetectOnly).Identity }
$IdentitiesORIGINAL = $Identities
While ($Identities.Count -gt 0)
{
    $Identities | % { 

    $results = Get-MailboxRepairRequest -Identity $_ 
    $results | ? {$_.Progress -eq '100' -and $_.JobState -ne 'Succeeded'} | % {

        $DBName = (Get-MailboxDatabase $_.Identity.ToString().Substring(0, $_.Identity.ToString().IndexOf('\')))[0].Name
        
        $_  | Add-Member -MemberType NoteProperty -Name DatabaseName -Value $DBName
        $Failed += $_
    }
     
    $results | ? {$_.Progress -ne '100'} | % {$Remaining += $_.Identity} 

    }
    $Identities = $Remaining
    $Remaining = @()
    
    Start-Sleep -Seconds 60
}

$Failed | fl
