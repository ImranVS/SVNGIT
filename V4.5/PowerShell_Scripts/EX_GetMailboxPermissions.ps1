param(
        [ValidateNotNullOrEmpty()]
        [string]$MailboxName
    )
$mbs = Get-Mailbox $MailboxName | Get-MailboxPermission | Where { ($_.IsInherited -eq $False)}
$mbs | ? {($_.User -like "NT AUTHORITY\SELF") } | % {$_.User = $_.Identity}
foreach($mb in $mbs){ 
    $obj = New-Object PSObject -Property @{ Identity=$mb.Identity; User=(Get-User $mb.user).Id} 
    if(-not $obj.User) { $obj.User = (Get-Group $mb.User).Id}
    $obj
}