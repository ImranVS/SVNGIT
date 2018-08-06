$uname= 'jnittech\vsinstall';
$Pwd='Admin123!';
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
$s = new-pssession -computer "USSVCADDC02.jnittech.com" -Credential $cred
Invoke-Command -session $s -script { Import-Module ActiveDirectory }
Import-PSSession -session $s -module ActiveDirectory 
