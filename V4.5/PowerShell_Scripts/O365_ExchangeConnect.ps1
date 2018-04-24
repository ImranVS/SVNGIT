#Enable-PSRemoting 
#set-executionpolicy unrestricted
#Set-item wsman:localhost\client\trustedhosts –Value *
clear
$uname= 'info@RPRVitalSigns.com'
$pwd= 'V1talS1gns';
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
$Session = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://outlook.office365.com/powershell-liveid/ -Credential $cred -Authentication Basic -AllowRedirection
Import-PSSession $session
Import-Module MSOnline
Connect-MsolService -Credential $cred
