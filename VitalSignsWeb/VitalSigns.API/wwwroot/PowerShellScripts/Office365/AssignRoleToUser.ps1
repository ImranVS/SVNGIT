<#
.DESCRIPTION
Adds a role to a given user.

.SUBTYPES
[User]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$UserPrincipalName,
        [ValidateNotNullOrEmpty()]
        [string]$RoleName
    )

try {
    Add-MsolRoleMember -RoleMemberEmailAddress $UserPrincipalName -RoleName $RoleName
} catch {
    Write-Output $_
    Write-Output "There was an error. Please ensure the UPN and RoleName are valid. Possible Roles are:"
    Get-MsolRole | Select Name
}

