<#
.DESCRIPTION
Increases the Mailbox Quota sizes by 10%

.SUBTYPES
[Mailbox]
#>
param(
        [ValidateNotNullOrEmpty()]
        [string]$SamAccountName
    )
$Unlimited = "Unlimited";

$mb = Get-Mailbox $SamAccountName

#$mbBefore = get-mailbox wesley
#Set-Mailbox -Identity $SamAccountName

#$previous | Get-Member

$IssueWarningQuotaBefore = $mb.IssueWarningQuota
$ProhibitSendQuotaBefore = $mb.ProhibitSendQuota
$ProhibitSendReceiveQuotaBefore = $mb.ProhibitSendReceiveQuota

$IssueWarningQuotaAfter = ''
$ProhibitSendQuotaAfter = ''
$ProhibitSendReceiveQuotaAfter = ''

if($IssueWarningQuotaBefore -ne $Unlimited) {
    #"Before: " + $("{0:f2}" -f ($mb.IssueWarningQuota.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2) + " MB"
    $IssueWarningQuotaBefore = [double] $("{0:f2}" -f ($mb.IssueWarningQuota.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)
    $IssueWarningQuotaAfter = "$($IssueWarningQuotaBefore * 1.1)MB"
    $IssueWarningQuotaBefore = "$($IssueWarningQuotaBefore)MB"
} else { 
    $IssueWarningQuotaAfter = $Unlimited
}

if($ProhibitSendQuotaBefore -ne $Unlimited) {
    #"Before: " + $("{0:f2}" -f ($mb.ProhibitSendQuota.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2) + " MB"
    $ProhibitSendQuotaBefore = [double] $("{0:f2}" -f ($mb.ProhibitSendQuota.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)
    $ProhibitSendQuotaAfter = "$($ProhibitSendQuotaBefore * 1.1)MB"
    $ProhibitSendQuotaBefore = "$($ProhibitSendQuotaBefore)MB"
} else { 
    $ProhibitSendQuotaAfter = $Unlimited
}
    
if($ProhibitSendReceiveQuotaBefore -ne $Unlimited) {
    #"Before: " + $("{0:f2}" -f ($mb.ProhibitSendReceiveQuota.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2) + " MB"
    $ProhibitSendReceiveQuotaBefore = [double] $("{0:f2}" -f ($mb.ProhibitSendReceiveQuota.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)
    $ProhibitSendReceiveQuotaAfter = "$($ProhibitSendReceiveQuotaBefore * 1.1)MB"
    $ProhibitSendReceiveQuotaBefore = "$($ProhibitSendReceiveQuotaBefore)MB"
} else { 
    $ProhibitSendReceiveQuotaAfter = $Unlimited
}

set-mailbox -Identity $SamAccountName -UseDatabaseQuotaDefaults $false -ProhibitSendReceiveQuota $ProhibitSendReceiveQuotaAfter -ProhibitSendQuota $ProhibitSendQuotaAfter -IssueWarningQuota $IssueWarningQuotaAfter

$obj = [ordered] @{}
$obj.IssueWarningQuotaBefore = $IssueWarningQuotaBefore;
$obj.IssueWarningQuotaAfter = $IssueWarningQuotaAfter;
$obj.ProhibitSendQuotaBefore = $ProhibitSendQuotaBefore;
$obj.ProhibitSendQuotaAfter = $ProhibitSendQuotaAfter;
$obj.ProhibitSendReceiveQuotaBefore = $ProhibitSendReceiveQuotaBefore;
$obj.ProhibitSendReceiveQuotaAfter = $ProhibitSendReceiveQuotaAfter;

$psobj = New-Object PSObject -Property $obj
$psobj


