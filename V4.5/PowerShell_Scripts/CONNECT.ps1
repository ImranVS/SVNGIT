Enable-PSRemoting 
set-executionpolicy unrestricted
Set-item wsman:localhost\client\trustedhosts –Value *
clear
$Servers='mail.jnittech.com';
$uname= 'jnittech\administrator';
$Pwd='Pa$$w0rd';
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
$so = New-PSSessionOption -SkipCNCheck -SkipRevocationCheck –SkipCACheck 
$session = New-PSSession -ConfigurationName Microsoft.Exchange -Credential $cred -ConnectionUri https://mail.jnittech.com/powershell -Authentication Default -SessionOption $so
Import-PSSession $session