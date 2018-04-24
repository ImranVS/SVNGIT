clear

$EASDevices = "" 
$AllEASDevices = @() 


$EASDevices = ""| select 'User','PrimarySMTPAddress','DeviceType','DeviceModel','DeviceOS', 'DeviceID', 'LastSyncAttemptTime','LastSuccessSync', 'DeviceActiveSyncVersion', 'Status', 'DeviceMobileOperator', 'DeviceFriendlyName', 'DeviceUserAgent', 'DeviceOSLanguage'

$EasMailboxes = Get-Mailbox -ResultSize unlimited 

foreach ($EASUser in $EasMailboxes) { 

$EASDevices.user = $EASUser.displayname 

$EASDevices.PrimarySMTPAddress = $EASUser.PrimarySMTPAddress.tostring() 

foreach ($EASUserDevices in Get-ActiveSyncDevice -Mailbox $EasUser.alias) { 

$EASDeviceStatistics = $EASUserDevices | Get-ActiveSyncDeviceStatistics 
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
$EASDevices.lastsyncattempttime = $EASDeviceStatistics.lastsyncattempttime 
$EASDevices.lastsuccesssync = $EASDeviceStatistics.lastsuccesssync 


$AllEASDevices += $EASDevices | select user,primarysmtpaddress,devicetype,devicemodel,deviceos,lastsyncattempttime,lastsuccesssync,status,DeviceMobileOperator, deviceid, DeviceActiveSyncVersion, DeviceFriendlyName, DeviceUserAgent, DeviceOSLanguage

} 

} 

$AllEASDevices = $AllEASDevices | sort user 
$AllEASDevices 

