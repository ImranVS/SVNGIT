$AllMailboxes = @()
$Mailboxes = Get-Mailbox -ResultSize Unlimited -WarningAction SilentlyContinue | Select DisplayName, Database, IssueWarningQuota, ProhibitSendQuota, ProhibitSendReceiveQuota, Alias, PrimarySmtpAddress, SAMAccountName, ExchangeGuid, RecipientType, RecipientTypeDetails
$Users = Get-User -WarningAction SilentlyContinue | select SAMAccountName, Company, Department

$MailboxStatistics = @()
ForEach ($DAGServer in (Get-DatabaseAvailabilityGroup).Servers) {
	ForEach ($MailboxStats in (Get-MailboxStatistics -Server $DAGServer | Where {$_.DisconnectDate -eq $Null})) {
		$NewMBXStatsDTRow = "" |Select  TotalitemSize,ItemCount,LastLogonTime,LastLogoffTime,MailboxGUID, StorageLimitStatus, ServerName
        $NewMBXStatsDTRow.TotalitemSize = $MailboxStats.TotalItemSize
		$NewMBXStatsDTRow.ItemCount = $MailboxStats.ItemCount
        $NewMBXStatsDTRow.StorageLimitStatus = $MailboxStats.StorageLimitStatus
        $NewMBXStatsDTRow.LastLogonTime = $MailboxStats.LastLogonTime
        $NewMBXStatsDTRow.LastLogoffTime = $MailboxStats.LastLogoffTime
        $NewMBXStatsDTRow.ServerName = $MailboxStats.ServerName
        $NewMBXStatsDTRow.MailboxGUID = $MailboxStats.MailboxGuid.ToString()
		$MailboxStatistics += $NewMBXStatsDTRow
	}
}


foreach ($Mailbox in $Mailboxes){ 
    $MailboxStats = "" |Select  DisplayName,Database,IssueWarningQuota,ProhibitSendQuota,ProhibitSendReceiveQuota,TotalItemSize,ItemCount,StorageLimitStatus,ServerName, SAMAccountName, PrimarySmtpAddress,Company, Department, Folders,LastLogonTime, RecipientType, RecipientTypeDetails
    $Stats = ($MailboxStatistics | ? {$_.MailboxGUID -eq $Mailbox.ExchangeGuid})[0]
    $User = ($Users | ? {$_.SAMAccountName -eq $Mailbox.SAMAccountName})[0]
    $MailboxStats.DisplayName = $Mailbox.DisplayName
    $MailboxStats.Database = $Mailbox.Database
    $MailboxStats.IssueWarningQuota = $("{0:f2}" -f ($Mailbox.IssueWarningQuota.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)
    $MailboxStats.ProhibitSendQuota =$("{0:f2}" -f ($Mailbox.ProhibitSendQuota.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)
    $MailboxStats.ProhibitSendReceiveQuota =$("{0:f2}" -f ($Mailbox.ProhibitSendReceiveQuota.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)
    $MailboxStats.TotalItemSize = $("{0:f2}" -f ($Stats.TotalItemSize.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)
    $MailboxStats.ItemCount = $Stats.ItemCount
    $MailboxStats.StorageLimitStatus = $Stats.StorageLimitStatus
    $MailboxStats.ServerName = $stats.ServerName
    $MailboxStats.LastLogonTime = $stats.LastLogonTime
    $MailboxStats.RecipientType = $Mailbox.RecipientType
    $MailboxStats.RecipientTypeDetails = $Mailbox.RecipientTypeDetails

    $folders = Get-MailboxFolderStatistics $Mailbox.Alias | select Name, ItemsInFolder, DeletedItemsInFolder, FolderSize, ItemsInFolderAndSubFolders, FolderAndSubFolderSize
    $MailboxStats.Folders = $folders

    $MailboxStats.SAMAccountName = $Mailbox.SAMAccountName
    $MailboxStats.PrimarySmtpAddress = $Mailbox.PrimarySmtpAddress
    $MailboxStats.Company = $User.Company
    $MailboxStats.Department = $User.Department
    
    $AllMailboxes += $MailboxStats
}

ForEach($ob in $AllMailboxes)
{	
	Write-Output $ob
}

