$results = @()
$results += Get-User -ResultSize Unlimited | select Id, DistinguishedName, UserPrincipalName, Name, IsValid, DisplayName, SamAccountName, @{Name='Type'; expression={'User'}}
$results += Get-Group -ResultSize Unlimited | select Id, DistinguishedName, Name, IsValid, Members, DisplayName, SamAccountName, @{Name='Type'; expression={'Group'}}
$results