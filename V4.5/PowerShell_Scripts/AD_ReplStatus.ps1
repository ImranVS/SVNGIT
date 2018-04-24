$uname= 'jnittech\administrator';
$Pwd='Pa$$w0rd';
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
$so = New-PSSessionOption -SkipCNCheck -SkipRevocationCheck –SkipCACheck 
$cmd={cmd.exe /c repadmin /showrepl}
Invoke-Command -ComputerName ad-dcin1.jnittech.com -Credential $cred -ScriptBlock $cmd