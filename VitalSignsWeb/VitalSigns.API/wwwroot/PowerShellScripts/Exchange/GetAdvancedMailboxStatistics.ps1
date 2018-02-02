<#
.DESCRIPTION
Moves a mailbox to another database

.SUBTYPES
[Mailbox]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$SamAccountName
    )
	Get-MailboxStatistics -Identity $SamAccountName