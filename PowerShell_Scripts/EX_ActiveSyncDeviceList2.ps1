$EASDevices = "" 
$AllEASDevices = @() 
 

$EASDevices = ""| select 'User','PrimarySMTPAddress','DeviceType', 'DevicePolicyApplied', 'DeviceModel','DeviceOS', 'DeviceID', 'LastSyncAttemptTime','LastSuccessSync', 'DeviceAccessState', 'DeviceActiveSyncVersion', 'Status', 'DeviceMobileOperator', 'DeviceFriendlyName', 'DeviceUserAgent', 'DeviceOSLanguage'

$EasMailboxes = Get-CASMailbox -Filter {HasActivesyncDevicePartnership -eq $true}

foreach ($EASUser in $EasMailboxes) { 

$EASDevices.user = $EASUser.Name 

$EASDevices.PrimarySMTPAddress = $EASUser.PrimarySMTPAddress.tostring() 

foreach ($EASUserDevices in Get-ActiveSyncDevice -Mailbox $EasUser.Name -WarningAction SilentlyContinue -ErrorAction SilentlyContinue) { 
$EASDeviceStatistics = $EASUserDevices | Get-ActiveSyncDeviceStatistics -WarningAction SilentlyContinue -ErrorAction SilentlyContinue
$EASDevices.devicetype = $EASUserDevices.devicetype 
$EASDevices.devicemodel = $EASUserDevices.devicemodel 
$EASDevices.deviceos = $EASUserDevices.deviceos 
$EASDevices.deviceid = $EASUserDevices.deviceid
$EASDevices.DeviceActiveSyncVersion = $EASUserDevices.DeviceActiveSyncVersion
$EASDevices.DeviceFriendlyName = $EASDeviceStatistics.DeviceFriendlyName
$EASDevices.DeviceUserAgent = $EASDeviceStatistics.DeviceUserAgent
$EASDevices.status = $EASDeviceStatistics.status 
$EASDevices.DeviceMobileOperator = $EASUserDevices.DeviceMobileOperator 
$EASDevices.DeviceOSLanguage = $EASUserDevices.DeviceOSLanguage 
$EASDevices.DeviceAccessState = $EASDeviceStatistics.DeviceAccessState 
$EASDevices.lastsyncattempttime = $EASDeviceStatistics.lastsyncattempttime 
$EASDevices.lastsuccesssync = $EASDeviceStatistics.lastsuccesssync 
$EASDevices.DevicePolicyApplied = $EASDeviceStatistics.DevicePolicyApplied 


$AllEASDevices += $EASDevices | select user,primarysmtpaddress,DevicePolicyApplied, DeviceAccessState,devicetype,devicemodel,deviceos,lastsyncattempttime,lastsuccesssync,status,DeviceMobileOperator, deviceid, DeviceActiveSyncVersion, DeviceFriendlyName, DeviceUserAgent, DeviceOSLanguage

} 

} 

$AllEASDevices = $AllEASDevices | ? {$_.User -ne ''}| sort user 
$AllEASDevices
