param(
        [ValidateNotNullOrEmpty()]
        [string]$MailboxName
    )
$mbs = Get-Mailbox $MailboxName | Get-MailboxPermission | Where { ($_.IsInherited -eq $False) -and -not ($_.User -like "NT AUTHORITY\SELF") } 
foreach($mb in $mbs){ New-Object PSObject -Property @{ Identity=$mb.Identity; Name=(Get-User $mb.user).Id}}