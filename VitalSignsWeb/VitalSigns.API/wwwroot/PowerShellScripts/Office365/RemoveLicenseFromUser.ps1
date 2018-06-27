<#
.DESCRIPTION
Removes a license from a user.

.SUBTYPES
[User]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$UserPrincipalName,
        [ValidateNotNullOrEmpty()]
        [string]$LicenseSkuId
    )

try {
    Set-MsolUserLicense -UserPrincipalName $UserPrincipalName -RemoveLicenses $LicenseSkuId
} catch {
    Write-Output $_
    Write-Output "There was an error. Please ensure the UPN and License are valid. Possible Licenses are:"
    Get-MsolAccountSku | select AccountSkuId
}
