Get-MailboxDatabase -IncludePreExchange2013 -status |select name,activationpreference,mountedonserver |ft -AutoSize
