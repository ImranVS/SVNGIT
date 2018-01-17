<#
.DESCRIPTION
Displays the message count in the various queues.

#>
$results = New-Object PSObject
$queue = Get-Queue
$shadow = ($queue | ? { $_.Identity -like '*Shadow*' } | Measure-Object -Sum MessageCount ).Sum
$submission = ($queue | ? { $_.Identity -like '*Submission*' } | Measure-Object -Sum MessageCount ).Sum
$unreachable = ($queue | ? { $_.Status -like '*Suspended*' -or $_.Status -like '*Retry*'} | Measure-Object -Sum MessageCount ).Sum
$results | Add-Member -Name "Shadow Queue" -Value $shadow -MemberType NoteProperty
$results | Add-Member -Name "Submission Queue" -Value $submission -MemberType NoteProperty
$results | Add-Member -Name "Unreachable Queue" -Value $unreachable -MemberType NoteProperty
$results