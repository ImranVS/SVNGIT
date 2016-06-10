$uname= $args[0];
$pwd= $args[1];
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;

Import-Module MSOnline
Connect-MsolService -Credential $cred
$MySession = New-SCSession -Credential $cred

$Servicedata= Get-MsolUser -All |Where {$_.IsLicensed -eq $true }| Select-Object  @{Name="MDM";Expression={$_.Licenses[0].ServiceStatus[0].ProvisioningStatus}}, @{Name="Yammer";Expression={$_.Licenses[0].ServiceStatus[1].ProvisioningStatus}}, @{Name="AD RMS";Expression={$_.Licenses[0].ServiceStatus[2].ProvisioningStatus}}, @{Name="OfficePro";Expression={$_.Licenses[0].ServiceStatus[3].ProvisioningStatus}}, @{Name="Skype";Expression={$_.Licenses[0].ServiceStatus[4].ProvisioningStatus}}, @{Name="OfficeWeb";Expression={$_.Licenses[0].ServiceStatus[5].ProvisioningStatus}}, @{Name="SharePoint";Expression={$_.Licenses[0].ServiceStatus[6].ProvisioningStatus}}, @{Name="Exchange";Expression={$_.Licenses[0].ServiceStatus[7].ProvisioningStatus}},DisplayName, Licenses
		

 $results = @()
ForEach ($data in $Servicedata) 
{ 
      $results += New-Object PSObject -Property  @{ 
       
       Licenses = $data.Licenses[0].AccountSkuId
       MDM = $data.MDM
       Yammer= $data.Yammer
       ADRMS= $data.'AD RMS'
       OfficePro= $data.OfficePro
       Skype= $data.Skype
       OfficeWeb= $data.OfficeWeb
       SharePoint= $data.SharePoint
       Exchange= $data.Exchange
        DisplayName = $data.DisplayName
         }
}
 
foreach($obj in $results)
{
    $obj
}