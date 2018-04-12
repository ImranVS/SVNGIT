<#
.DESCRIPTION
Gets a list of all Sweep Rules

.SUBTYPES
[Mailbox]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$Name
    )
	Get-SweepRule -Mailbox $Name