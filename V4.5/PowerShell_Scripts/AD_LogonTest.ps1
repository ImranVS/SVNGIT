$UserName = '#user'
$Password = '#pwd'
$Domain = '#domain'

Add-Type -AssemblyName System.DirectoryServices.AccountManagement
$ct = [System.DirectoryServices.AccountManagement.ContextType]::Domain
$pc = New-Object System.DirectoryServices.AccountManagement.PrincipalContext $ct,$Domain
$pc.ValidateCredentials($UserName,$Password)
