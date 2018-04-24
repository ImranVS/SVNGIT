<#
.DESCRIPTION
Gets the Exchange Mailbox Object and prints all the attributes. The Name of the mailbox is required.

.SUBTYPES
[Mailbox]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$SamAccountName
    )
	Get-Mailbox $SamAccountName