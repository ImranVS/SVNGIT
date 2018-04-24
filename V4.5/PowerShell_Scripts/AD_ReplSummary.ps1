#$uname= '************'
#$Pwd='************';
#$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
#$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
#$so = New-PSSessionOption -SkipCNCheck -SkipRevocationCheck –SkipCACheck 
#$cmd={cmd.exe /c repadmin /replsummary /bysrc %computername% }
#Invoke-Command -ComputerName *************** -Credential $cred -ScriptBlock $cmd
cmd.exe /c repadmin /replsummary /bysrc %computername%