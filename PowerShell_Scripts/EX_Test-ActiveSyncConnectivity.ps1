Clear
$uname= 'alan@jnittech.com';
$Pwd='Vitalsigns123!';
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;

Test-ActiveSyncConnectivity -ClientAccessServer JNITTECH-EXCHG1  -TrustAnySSLCertificate  -MailboxCredential $cred | select Scenario, Result
