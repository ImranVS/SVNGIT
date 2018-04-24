<#
.DESCRIPTION
Enables an Inbox Rule based on the Inbox Rule Identity, which can be obtained from the PowerScript GetInboxRules.ps1. If no errors, the script has executed successfully.

.SUBTYPES
[Mailbox]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$InboxRuleIdentity
    )
	Enable-InboxRule -Identity $InboxRuleIdentity
    Write-Host "If no error is displayed then the script executed successfully"