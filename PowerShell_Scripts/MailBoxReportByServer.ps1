#.\MailBoxReportScript.ps1 -all
#.\MailBoxReport.ps1 -server win-kurrr2qsdj0

#param(
#	[Parameter(ParameterSetName='server')] [string]$server
#)

$server = 'win-kurrr2qsdj0.jnittech.com'
#...................................
# Variables
#...................................

$report = @()

#...................................
# Script
#...................................

$mailboxes = @(Get-Mailbox -server $server -resultsize unlimited -IgnoreDefaultScope)


$mailboxcount = $mailboxes.count
$i = 0

#Loop through mailbox list and find the aged mailboxes
foreach ($mb in $mailboxes)
{
    $stats = $mb | Get-MailboxStatistics | Select-Object TotalDeletedItemSize,ItemCount,LastLogonTime, `
                     @{name="TotalItemSize"; expression={[math]::Round( `
                     ($_.TotalItemSize.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)}}, `
                     LastLoggedOnUserAccount
    
	#Create a custom PS object to aggregate the data we're interested in
	
	$userObj = New-Object PSObject
	$userObj | Add-Member NoteProperty -Name "DisplayName" -Value $mb.DisplayName
	$userObj | Add-Member NoteProperty -Name "LastMailboxLogon" -Value $stats.LastLogonTime
	$userObj | Add-Member NoteProperty -Name "LastLogonBy" -Value $stats.LastLoggedOnUserAccount
	$userObj | Add-Member NoteProperty -Name "ItemSize" -Value $stats.TotalItemSize
	$userObj | Add-Member NoteProperty -Name "DeletedItemSize" -Value $stats.TotalDeletedItemSize
	$userObj | Add-Member NoteProperty -Name "Items" -Value $stats.ItemCount
	$userObj | Add-Member NoteProperty -Name "Type" -Value $mb.RecipientTypeDetails
	$userObj | Add-Member NoteProperty -Name "Server" -Value $mb.ServerName
	$userObj | Add-Member NoteProperty -Name "Database" -Value $mb.Database
    
    #Add the object to the report
	$report = $report += $userObj
}

$report #| Format-Table