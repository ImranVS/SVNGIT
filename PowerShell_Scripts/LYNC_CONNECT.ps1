clear
$Servers='LYNC.jnittech.com';
$uname='JNITTECH\administrator';
$Pwd='Pa$$w0rd';
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
$SessionOptions = New-PSSessionOption –SkipCACheck –SkipCNCheck –SkipRevocationCheck
$session=New-PSSession -SessionOption $SessionOptions -Credential $cred -ConnectionUri https://LYNC.jnittech.com/OcsPowershell -Authentication default 
Import-PSSession $session
