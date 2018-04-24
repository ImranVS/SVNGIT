$uname= 'jnittech\administrator';
$Pwd='Pa$$w0rd';
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
$s = new-pssession -computer "jnittech-ad.jnittech.com" -Credential $cred
Invoke-Command -session $s -script { Import-Module ActiveDirectory }
Import-PSSession -session $s -module ActiveDirectory 
