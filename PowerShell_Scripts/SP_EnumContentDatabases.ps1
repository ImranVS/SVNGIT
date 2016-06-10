#enable-wsmancredssp -role client -delegatecomputer * -force
#$uname= '******';
#$Pwd='******';
#$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
#$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
#$s = new-pssession -computer "sp-app1.jnittech.com" -Authentication Credssp -Credential $cred
#Invoke-Command -Session $s -ScriptBlock {Add-PSSnapin Microsoft.SharePoint.PowerShell;}
#Invoke-Command -Session $s -ScriptBlock{
$Array=@()
[void][System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SharePoint")
$WebService = [Microsoft.SharePoint.Administration.SPWebService]::ContentService
$DBName = [Microsoft.SharePoint.Administration.SPContentDatabase].GetProperty("Name")
	$DBID = [Microsoft.SharePoint.Administration.SPContentDatabase].GetProperty("Id")  
	$DBSiteCount = [Microsoft.SharePoint.Administration.SPContentDatabase].GetProperty("CurrentSiteCount")
	$DBWarningSiteCount = [Microsoft.SharePoint.Administration.SPContentDatabase].GetProperty("WarningSiteCount")
	$DBMaximumSiteCount = [Microsoft.SharePoint.Administration.SPContentDatabase].GetProperty("MaximumSiteCount")
	$DBIsReadOnly = [Microsoft.SharePoint.Administration.SPContentDatabase].GetProperty("IsReadOnly")
	$DBServer= [Microsoft.SharePoint.Administration.SPContentDatabase].GetProperty("ServiceInstance")
	foreach($WebApplication in $WebService.WebApplications){
		$ContentDBCollection = $WebApplication.ContentDatabases
		$webAppName = $WebApplication.name		
		foreach($ContentDB in $ContentDBCollection){		
			$CurrentDBName = $DBName.GetValue($ContentDB, $null)
			$CurrentDBID = $DBID.GetValue($ContentDB)
			$CurrentDBCurrentSiteCount = $DBSiteCount.GetValue($ContentDB, $null)
			$CurrentDBDBMaximumSiteCount = $DBMaximumSiteCount.GetValue($ContentDB, $null)
			$CurrentDBDWarningSiteCount = $DBWarningSiteCount.GetValue($ContentDB, $null)
			$CurrentDBIsReadOnly = $DBIsReadOnly.GetValue($ContentDB, $null)
            $CurrentDBServer = ($DBServer.GetValue($ContentDB)).NormalizedDataSource           

$Array+=New-Object PSObject -Property @{
WebApplicationName=$webAppName;  
ContentDatabaseName=$CurrentDBName;
ContentDatabaseID=$CurrentDBID;
DatabaseSiteCount=$CurrentDBCurrentSiteCount;
MaxSiteCountThreshold=$CurrentDBDBMaximumSiteCount;
WarningSiteCountThreshold=$CurrentDBDWarningSiteCount;
IsContentDBReadOnly=$CurrentDBIsReadOnly;
WhoIsDBServer=$CurrentDBServer;
            }
            }
            }$Array
            #}