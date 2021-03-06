#.D:\Mukund\E\RPRWyatt\VSTrunk\PowerShell_POC\PSPOC1\bin\Debug\Scripts\EX_DiskInfo0.ps1 -computer "mail.jnittech.com" -uname "jnittech\administrator" -Pwd "Jnit@Tices1"
param( $computer, $uname, $Pwd
    
)

clear
$so = New-PSSessionOption -SkipCNCheck -SkipRevocationCheck -SkipCACheck
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
Invoke-Command -ComputerName $computer -SessionOption $so -Credential $cred {
Get-WmiObject Win32_LogicalDisk -Filter "DriveType=3" | Foreach-Object{
 New-Object PSObject -Property @{
  SystemName=$_.SystemName
  DeviceID=$_.DeviceID
  VolumeName=$_.VolumeName
  "Size(GB)" = "{0:N2}" -f ($_.Size/1GB)
  "FreeSpace(GB)" = "{0:N2}" -f ($_.FreeSpace/1GB)
  "SpaceUsed(GB)" = "{0:N2}" -f (($_.Size-$_.FreeSpace)/1GB)
  PercentFree = "{0:N2}" -f ($_.FreeSpace/$_.Size)
  PercentUsed = "{0:N2}" -f (($_.Size-$_.FreeSpace)/$_.Size)
}
} | Format-List
};