Enable-PSRemoting 
set-executionpolicy unrestricted
Set-item wsman:localhost\client\trustedhosts –Value *
clear
$uname= 'vsinstall@jnittech.com';
$Pwd='Admin123!';
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
$so = New-PSSessionOption -SkipCNCheck -SkipRevocationCheck –SkipCACheck 
$session = New-PSSession -ConfigurationName Microsoft.Exchange -Credential $cred -ConnectionUri https://EX13-2.jnittech.com/powershell -Authentication Default -SessionOption $so
Import-PSSession $session -AllowClobber