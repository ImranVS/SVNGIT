<#
.DESCRIPTION
Disables Multi-Factor Authentication for the given user.

.SUBTYPES
[User]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$UserPrincipalName
    )
    $Sta = @()
Set-MsolUser -UserPrincipalName $UserPrincipalName -StrongAuthenticationRequirements $Sta 