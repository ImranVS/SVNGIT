<#
.DESCRIPTION
Disables a Sweep Rule based on the Sweep Rule Identity, which can be obtained from the PowerScript GetSweepRules.ps1. If no errors, the script has executed successfully.

.SUBTYPES
[Mailbox]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$SweepRuleIdentity
    )
	Disable-SweepRule -Identity $SweepRuleIdentity
    Write-Host "If no error is displayed then the script executed successfully"