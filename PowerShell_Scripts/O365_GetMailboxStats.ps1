param(
    [string] $SamStartingCharacters
    )

$results=@()
Get-Mailbox -Filter "{ SamAccountName -like '$SamStartingString*'}" -ResultSize unlimited| select Database,ServerName,identity, PrimarySmtpAddress, SamAccountName, ForwardingSMTPAddress, ForwardingAddress, DeliverToMailboxAndForward | % {
$stats =Get-MailboxStatistics $_.PrimarySmtpAddress |select TotalItemSize,ItemCount,StorageLimitStatus,LastLogonTime,LastLogoffTime,DisplayName
$inboxRules = Get-InboxRule -Mailbox $_.PrimarySmtpAddress | Select ForwardTo, ForwardAsAttachmentTo
$stats |Add-Member -Type NoteProperty -Name Database -Value $_.Database
$stats |Add-Member -Type NoteProperty -Name ServerName -Value $_.ServerName 
$stats |Add-Member -Type NoteProperty -Name PrimarySmtpAddress -Value $_.PrimarySmtpAddress 
$stats |Add-Member -Type NoteProperty -Name SamAccountName -Value $_.SamAccountName 
$stats | Add-Member -Type NoteProperty -Name MailboxForwardingSMTPAddress -Value $_.ForwardingSMTPAddress
$stats | Add-Member -Type NoteProperty -Name MailboxForwardingAddress -Value $_.ForwardingAddress
$stats | Add-Member -Type NoteProperty -Name RuleForwardTo -Value ([String[]] ($inboxRules | select -ExpandProperty ForwardTo))
$stats | Add-Member -Type NoteProperty -Name RuleForwardAsAttachmentTo -Value ([String[]]($inboxRules | select -ExpandProperty ForwardAsAttachmentTo))
$stats | Add-Member -Type NoteProperty -Name DeliverToMailboxAndForward -Value $_.DeliverToMailboxAndForward
$results +=($stats)
} 
$results
Clear-Variable 'results' -ErrorAction SilentlyContinue