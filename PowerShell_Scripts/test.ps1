#.D:\Mukund\E\RPRWyatt\VSTrunk\PowerShell_POC\PSPOC1\bin\Debug\Scripts\test.ps1 -computer "mail.jnittech.com" -uname "jnittech\administrator" -Pwd "Jnit@Tices1"
param( $computer, $uname, $Pwd
    
)

clear
$so = New-PSSessionOption -SkipCNCheck -SkipRevocationCheck -SkipCACheck
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
Invoke-Command -ComputerName $computer -SessionOption $so -Credential $cred {

Get-WmiObject win32_operatingsystem  | Foreach-Object{
 New-Object PSObject -Property @{
  SystemName=$_.CSName
  TotalMemoryMB="{0:N2}" -f ($_.TotalVisibleMemorySize/1KB)
 FreeMemoryMB = "{0:N2}" -f ($_.FreePhysicalmemory/1KB)
  PercentMemoryFree = "{0:N2}" -f (($_.FreePhysicalmemory/$_.TotalVisibleMemorySize)*100)
}
} | Format-List

};