Clear
$uname= 'alan@jnittech.com';
$Pwd='Vitalsigns123!';
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
Test-OutlookWebServices -ClientAccessServer JNITTECH-EXCHG1   -Identity:alan@jnittech.com
