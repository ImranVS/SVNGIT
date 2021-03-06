param([string] $path, [string] $owners, [string] $webapp, [string] $pwd)
#$path = “C:\Users\Alan\Desktop\TestUploads”;
#$owners = 'jnittech\administrator'
#$webapp = 'http://sharepoint.jnittech.com:100'
#$pwd='Pa$$w0rd';

$TestSite = '/sites/VSTestSiteCollection'
$DocumentLibName = 'My Documents'
$siteURL = $webapp + $TestSite

$s = Get-PSSession

Invoke-Command $s -ArgumentList $siteURL, $Owners, $webApp -ScriptBlock {
param($siteURL, $owner, $webApp)
$ReturnObject = New-Object PSObject -Property @{
SiteCreation = $false
FileUpload = $false
}
#Make the site collection

$managedPath = New-SPManagedPath -RelativeURL "sites/VSTestWes" -WebApplication $webApp -Explicit
$owner = $Owner
$secondOwner = $Owner
$template = “STS#1"
$description = “This is a sample site that was built using PowerShell.”
$newSite = New-SPSite $siteURL -OwnerAlias $owner -SecondaryOwnerAlias $secondOwner -name “PowerShell for SharePoint” -Template $template -Description $description

if($newSite -eq $null)
{
    $ReturnObject.SiteCreation = $false
}
else
{
    $ReturnObject.SiteCreation = $true
}
}

Start-Sleep -s 1

Invoke-Command $s -ArgumentList $DocumentLibName -ScriptBlock {
param($DocumentLibName)
#Make the Document Library

$spWeb = Get-SPWeb -Identity $siteURL
$listTemplate = [Microsoft.SharePoint.SPListTemplateType]::DocumentLibrary
$listAdding = $spWeb.Lists.Add($DocumentLibName,"VS Test Library",$listTemplate)


}


Start-Sleep -s 1


$destination = $siteURL + '/' + $DocumentLibName;
$domain = $owners.Substring(0,$owners.IndexOf('\'))
$uname = $owners.Substring($owners.IndexOf('\') + 1)
$webclient = new-object System.Net.WebClient
$webclient.Credentials = new-object System.Net.NetworkCredential($uname, $pwd, $domain)
Get-ChildItem $path | ForEach-Object {$webclient.UploadFile($destination + “/” + $_.Name, “PUT”, $_.FullName)}


Start-Sleep -s 1


Invoke-Command $s -ArgumentList $DocumentLibName -ScriptBlock {
param($DocumentLibName)

$File = $spWeb.Lists[$DocumentLibName].Items | ?{$_.Name -eq 'Wes.txt'}
if($File -eq $null)
{
    $ReturnObject.FileUpload = $false
}
else
{
    $ReturnObject.FileUpload = $true
}
}

Invoke-Command $s -ScriptBlock {
$ReturnObject

Remove-SPSite -Identity $siteURL -GradualDelete -confirm:$false
Remove-SPManagedPath -Identity "sites/VSTestWes" -WebApplication $webApp -confirm:$false

}