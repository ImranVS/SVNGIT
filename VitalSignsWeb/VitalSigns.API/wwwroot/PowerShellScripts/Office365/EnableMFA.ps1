<#
.DESCRIPTION
Enables Multi-Factor Authentication for the given user.

.SUBTYPES
[User]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$UserPrincipalName
    )
$st = New-Object -TypeName Microsoft.Online.Administration.StrongAuthenticationRequirement
$st.RelyingParty = "*"
$st.State = “Enabled”
$sta = @($st)
Set-MsolUser -UserPrincipalName UserPrincipalName -StrongAuthenticationRequirements $sta