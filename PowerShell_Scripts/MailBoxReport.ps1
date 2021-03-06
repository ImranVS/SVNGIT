#.\MailBoxReportScript.ps1 -all
#.\MailBoxReport.ps1 -server win-kurrr2qsdj0

#param(
	#[Parameter(ParameterSetName='database')] [string]$database,
	#[Parameter(ParameterSetName='file')] [string]$file,
	#[Parameter(ParameterSetName='server')] [string]$server,
	#[Parameter(ParameterSetName='mailbox')] [string]$mailbox,
	#[Parameter(ParameterSetName='all')] [switch]$all
	#[string]$filename
#)
#$server = 'win-kurrr2qsdj0.jnittech.com'
param($server)
#...................................
# Variables
#...................................

$report = @()

#...................................
# Script
#...................................

#Add dependencies
#Import-Module ActiveDirectory

#Get the mailbox list

Write-Host -ForegroundColor White "Collecting mailbox list"

if($all) { $mailboxes = @(Get-Mailbox -resultsize unlimited -IgnoreDefaultScope) }

if($server) { $mailboxes = @(Get-Mailbox -server $server -resultsize unlimited -IgnoreDefaultScope) }

if($database){ $mailboxes = @(Get-Mailbox -database $database -resultsize unlimited -IgnoreDefaultScope) }

if($file) {	$mailboxes = @(Get-Content $file | Get-Mailbox -resultsize unlimited) }

$mailboxcount = $mailboxes.count
$i = 0

#Loop through mailbox list and find the aged mailboxes
foreach ($mb in $mailboxes)
{
    $stats = $mb | Get-MailboxStatistics | Select-Object TotalDeletedItemSize,ItemCount,LastLogonTime, `
                     @{name="TotalItemSize"; expression={[math]::Round( `
                     ($_.TotalItemSize.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)}}, `
                     LastLoggedOnUserAccount
           
    #This is an aged mailbox, so we want some extra details about the account
	
	 #$user = Get-User $mb
	#$aduser = Get-ADUser $mb.samaccountname -Properties Enabled,AccountExpirationDate
    
	#Create a custom PS object to aggregate the data we're interested in
	
	$userObj = New-Object PSObject
	$userObj | Add-Member NoteProperty -Name "DisplayName" -Value $mb.DisplayName
	#$userObj | Add-Member NoteProperty -Name "Title" -Value $user.Title
	#$userObj | Add-Member NoteProperty -Name "Department" -Value $user.Department
	#$userObj | Add-Member NoteProperty -Name "Office" -Value $user.Office
	#$userObj | Add-Member NoteProperty -Name "Enabled" -Value $aduser.Enabled
	#$userObj | Add-Member NoteProperty -Name "Expires" -Value $aduser.AccountExpirationDate
	$userObj | Add-Member NoteProperty -Name "LastMailboxLogon" -Value $stats.LastLogonTime
	$userObj | Add-Member NoteProperty -Name "LastLogonBy" -Value $stats.LastLoggedOnUserAccount
	$userObj | Add-Member NoteProperty -Name "ItemSize" -Value $stats.TotalItemSize#.Value.ToMB()
	$userObj | Add-Member NoteProperty -Name "DeletedItemSize" -Value $stats.TotalDeletedItemSize#.Value.ToMB()
	$userObj | Add-Member NoteProperty -Name "Items" -Value $stats.ItemCount
	$userObj | Add-Member NoteProperty -Name "Type" -Value $mb.RecipientTypeDetails
	$userObj | Add-Member NoteProperty -Name "Server" -Value $mb.ServerName
	$userObj | Add-Member NoteProperty -Name "Database" -Value $mb.Database
    
    #Add the object to the report
	$report = $report += $userObj
}

$report |  Format-List