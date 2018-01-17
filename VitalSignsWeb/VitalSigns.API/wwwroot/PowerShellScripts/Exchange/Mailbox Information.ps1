<#
.DESCRIPTION
Gets the Exchange Mailbox Object and prints all the attributes. The Name of the mailbox is required.

#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$Name
    )
	Get-Mailbox $Name