
$Session = (Get-PSSession)[0]
$UserName = (Get-PSSession)[0].Runspace.ConnectionInfo.Credential.UserName

Invoke-Command -Session $Session -ScriptBlock {

winrm set winrm/config/winrs '@{MaxShellsPerUser="100"}'
winrm set winrm/config/winrs '@{MaxConcurrentUsers="100"}'
Set-ExecutionPolicy -ExecutionPolicy Unrestricted

}

