<#
.DESCRIPTION
Moves a mailbox to another database

.SUBTYPES
[Mailbox]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$Name
    )
	Get-MailboxStatistics -Identity $Name