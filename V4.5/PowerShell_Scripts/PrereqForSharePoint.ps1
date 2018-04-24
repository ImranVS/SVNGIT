
$Session = (Get-PSSession)[0]
$UserName = (Get-PSSession)[0].Runspace.ConnectionInfo.Credential.UserName
$Creds = (Get-PSSession)[0].Runspace.ConnectionInfo.Credential

Invoke-Command -Session $Session -ScriptBlock {

Set-ExecutionPolicy -ExecutionPolicy Unrestricted
Enable-WSManCredSP -Role Server
winrm set winrm/config/winrs ‘@{MaxShellsPerUser=”100”}’
winrm set winrm/config/winrs ‘@{MaxMemoryPerShellMB=”600”}’

Add-PSSnapin Microsoft.SharePoint.PowerShell

Get-SPContentDatabase | Add-SPShellAdmin -UserName $UserName

}

