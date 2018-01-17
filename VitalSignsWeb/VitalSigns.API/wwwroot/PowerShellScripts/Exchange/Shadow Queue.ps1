<#
.DESCRIPTION
Displays the messages in the shadow queue.

#>
Get-Queue | ? { $_.Status -like '*Suspended*' -or $_.Status -like '*Retry*'}