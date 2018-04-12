<#
.DESCRIPTION
Gets a list of all Mailbox Rules

.SUBTYPES
[Mailbox]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$Name
    )
	Get-InboxRule -Mailbox $Name