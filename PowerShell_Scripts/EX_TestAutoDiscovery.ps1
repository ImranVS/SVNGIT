param(
    [string] $ServerName,
    [string] $MailboxIdentity,
    [PSCredential] $MailboxCreds
    )
Test-OutlookWebServices -ClientAccessServer $ServerName -Identity $MailboxIdentity -MailboxCredential $MailboxCreds | ? { $_.Scenario -eq "AutoDiscoverOutlookProvider"}