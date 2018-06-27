<#
.DESCRIPTION
Blocks user access to the given UPN.

.SUBTYPES
[User]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$UserPrincipalName
    )

Set-MsolUser -UserPrincipalName $UserPrincipalName -BlockCredential $true