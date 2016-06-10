[string[]] $Identities = @()

Get-MailboxDatabase | % { $Identities += (New-MailboxRepairRequest -Database $_.Name -CorruptionType SearchFolder,AggregateCounts,ProvisionedFolder,FolderView -DetectOnly).Identity }
$Identities