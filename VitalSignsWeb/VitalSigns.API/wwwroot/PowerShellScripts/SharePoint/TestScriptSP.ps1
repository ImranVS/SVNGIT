﻿param(
        [ValidateNotNullOrEmpty()]
        [string]$NameSp,
		[ValidateNotNullOrEmpty()]
        [string]$NameSp1,
		[ValidateNotNullOrEmpty()]
        [string]$NameSp2,
		[ValidateNotNullOrEmpty()]
        [string]$NameSp3
    )
	Get-Mailbox $NameSp