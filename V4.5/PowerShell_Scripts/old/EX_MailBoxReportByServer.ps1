#.\MailBoxReportByServer.ps1 -server win-kurrr2qsdj0

param(
	[Parameter(ParameterSetName='server')] [string]$server
)

$report = @()
#$server = 'win-kurrr2qsdj0.jnittech.com'

$sortedStats = Get-Mailbox -Server $server -ResultSize Unlimited | Get-MailboxStatistics | Sort-Object TotalItemSize -Descending| Select-Object DisplayName, RecipientTypeDetails, ServerName, Database, TotalDeletedItemSize,ItemCount,LastLogonTime, `
                     @{name="TotalItemSize"; expression={[math]::Round( `
                     ($_.TotalItemSize.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)}}, `
                     LastLoggedOnUserAccount


foreach ($stats in $sortedStats) {
                 	$userObj = New-Object PSObject
	$userObj | Add-Member NoteProperty -Name "DisplayName" -Value $stats.DisplayName
	$userObj | Add-Member NoteProperty -Name "LastMailboxLogon" -Value $stats.LastLogonTime
	$userObj | Add-Member NoteProperty -Name "LastLogonBy" -Value $stats.LastLoggedOnUserAccount
	$userObj | Add-Member NoteProperty -Name "ItemSize" -Value $stats.TotalItemSize
	$userObj | Add-Member NoteProperty -Name "DeletedItemSize" -Value $stats.TotalDeletedItemSize
	$userObj | Add-Member NoteProperty -Name "Items" -Value $stats.ItemCount
	$userObj | Add-Member NoteProperty -Name "Server" -Value $stats.ServerName
	$userObj | Add-Member NoteProperty -Name "Database" -Value $stats.Database
    
    #Add the object to the report
	$report = $report += $userObj

}

$report | Format-Table
