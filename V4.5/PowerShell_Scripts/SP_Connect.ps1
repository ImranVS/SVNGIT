enable-wsmancredssp -role client -delegatecomputer * -force
$uname= 'jnittech\administrator';
$Pwd='Pa$$w0rd';
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
$s = new-pssession -computer "sharepoint.jnittech.com" -Authentication Credssp -Credential $cred
Enter-PSSession -Session $s
Invoke-Command -Session $s -ScriptBlock {Add-PSSnapin Microsoft.SharePoint.Powershell }
