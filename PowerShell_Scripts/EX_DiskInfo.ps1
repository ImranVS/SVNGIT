$Vols = @();

$Vols += Get-WmiObject win32_LogicalDisk -Filter "DriveType=3" | Foreach-Object{
 New-Object PSObject -Property @{
  SystemName=$_.SystemName
  DeviceID=$_.DeviceID
  VolumeName=$_.VolumeName
  Size = "{0:N2}" -f ($_.Size/1GB)
  FreeSpace = "{0:N2}" -f ($_.FreeSpace/1GB)
  SpaceUsed= "{0:N2}" -f (($_.Size-$_.FreeSpace)/1GB)
  PercentFree = "{0:N2}" -f ($_.FreeSpace/$_.Size)
  PercentUsed = "{0:N2}" -f (($_.Size-$_.FreeSpace)/$_.Size)
}
} 

$Vols += Get-WmiObject win32_volume | Where-object {$_.DriveLetter -eq $null -and $_.DriveType -eq 3 -and !$_.SystemVolume } | ForEach-Object{
 New-Object PSObject -Property @{
  SystemName=$_.SystemName
  DeviceID=$_.Name
  VolumeName=$_.DeviceID
  Size = "{0:N2}" -f ($_.Capacity/1GB)
  FreeSpace = "{0:N2}" -f ($_.FreeSpace/1GB)
  SpaceUsed= "{0:N2}" -f (($_.Capacity-$_.FreeSpace)/1GB)
  PercentFree = "{0:N2}" -f ($_.FreeSpace/$_.Capacity)
  PercentUsed = "{0:N2}" -f (($_.Capacity-$_.FreeSpace)/$_.Capacity)
 }
}

$Vols