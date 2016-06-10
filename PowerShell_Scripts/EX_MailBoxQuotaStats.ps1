$AllMailboxes = @()
$Mailboxes = Get-Mailbox -ResultSize Unlimited | Select DisplayName, Database, IssueWarningQuota, ProhibitSendQuota, ProhibitSendReceiveQuota, Alias
foreach ($Mailbox in $Mailboxes){   
    $MailboxStats = "" |Select  DisplayName,Database,IssueWarningQuota,ProhibitSendQuota,ProhibitSendReceiveQuota,TotalItemSize,ItemCount,StorageLimitStatus,ServerName
    $Stats = Get-MailboxStatistics -Identity $Mailbox.Alias
    $MailboxStats.DisplayName = $Mailbox.DisplayName
    $MailboxStats.Database = $Mailbox.Database
    $MailboxStats.IssueWarningQuota = $("{0:f2}" -f ($Mailbox.IssueWarningQuota.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)
    $MailboxStats.ProhibitSendQuota =$("{0:f2}" -f ($Mailbox.ProhibitSendQuota.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)
    $MailboxStats.ProhibitSendReceiveQuota =$("{0:f2}" -f ($Mailbox.ProhibitSendReceiveQuota.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)
    $MailboxStats.TotalItemSize = $("{0:f2}" -f ($Stats.TotalItemSize.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)
    $MailboxStats.ItemCount = $Stats.ItemCount
    $MailboxStats.StorageLimitStatus = $Stats.StorageLimitStatus
    $MailboxStats.ServerName = $stats.ServerName
    $AllMailboxes += $MailboxStats
}

#$AllMailboxes| Sort-Object "itemCount" -Descending | ft -AutoSize


ForEach($ob in $AllMailboxes)
{	
	Write-Output $ob
}

