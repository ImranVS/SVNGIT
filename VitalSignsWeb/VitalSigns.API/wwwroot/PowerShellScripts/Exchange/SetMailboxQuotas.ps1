<#
.DESCRIPTION
Sets each quota on a mailbox. Specify enter Unlimited for no quota or a value in MBs for each box

.SUBTYPES
[Mailbox]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$SamAccountName,
        [ValidateNotNullOrEmpty()]
        [string]$IssueWarningQuota,
        [ValidateNotNullOrEmpty()]
        [string]$ProhibitSendQuota,
        [ValidateNotNullOrEmpty()]
        [string]$ProhibitSendReceiveQuota
    )

$Unlimited = "Unlimited";
$tempTryParse = 0;
if(-not (($Unlimited -ieq $IssueWarningQuota -or [double]::TryParse($IssueWarningQuota, [ref] $tempTryParse)) -and ($Unlimited -ieq $ProhibitSendQuota -or [double]::TryParse($ProhibitSendQuota, [ref] $tempTryParse)) -and ($Unlimited -ieq $ProhibitSendReceiveQuota -or [double]::TryParse($ProhibitSendReceiveQuota, [ref] $tempTryParse)))) {
    throw("One or more of the quotas is not a valid number or the string Unlimited. Please try again")
    return;
}

if([double]::TryParse($IssueWarningQuota, [ref] $tempTryParse)) {
    $IssueWarningQuota = "$([double] $IssueWarningQuota)MB";
}
if([double]::TryParse($ProhibitSendQuota, [ref] $tempTryParse)) {
    $ProhibitSendQuota = "$([double] $ProhibitSendQuota)MB";
}
if([double]::TryParse($ProhibitSendReceiveQuota, [ref] $tempTryParse)) {
    $ProhibitSendReceiveQuota = "$([double] $ProhibitSendReceiveQuota)MB";
}

set-mailbox -Identity $SamAccountName -UseDatabaseQuotaDefaults $false -ProhibitSendReceiveQuota $ProhibitSendReceiveQuota -ProhibitSendQuota $ProhibitSendQuota -IssueWarningQuota $IssueWarningQuota

$obj = [ordered] @{}
$obj.Message = "Values are updated."
$obj.IssueWarningQuota = $IssueWarningQuota;
$obj.ProhibitSendQuota = $ProhibitSendQuota;
$obj.ProhibitSendReceiveQuota = $ProhibitSendReceiveQuota;

$psobj = New-Object PSObject -Property $obj
$psobj


