$mailboxes = Get-Mailbox -WarningAction silentlycontinue | Group-Object -Property:Database | Select-Object Name,Count | Sort-Object Name
$mailboxes