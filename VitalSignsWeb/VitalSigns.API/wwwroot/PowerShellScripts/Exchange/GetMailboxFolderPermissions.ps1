<#
.DESCRIPTION
Gets all the users which have access for each folder inside the given mailbox

.SUBTYPES
[Mailbox]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$SamAccountName
    )

$results = @()
$results = Get-MailboxFolderPermission -Identity $SamAccountName
Get-MailboxFolderStatistics -Identity $SamAccountName | % { $results += Get-MailboxFolderPermission $($_.Identity.replace('\', ':\')) -ErrorAction silentlycontinue}

$a = "<style>"
$a = $a + "TABLE{border-width: .5px;border-style: solid;border-color: black;border-collapse: collapse;}"
$a = $a + "TH{border-width: .5px;padding: .5rem;border-style: solid;border-color: gray; font-weight : bold;}"
$a = $a + "TD{border-width: .5px;padding: .5rem;border-style: solid;border-color: gray; font-weight : normal;}"
$a = $a + "</style>"

$results | Select FolderName, User, AccessRights  | ConvertTo-Html -Head $a