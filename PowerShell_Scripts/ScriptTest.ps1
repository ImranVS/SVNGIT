Get-WmiObject win32_operatingsystem  | Foreach-Object{
 New-Object PSObject -Property @{
  SystemName=$_.CSName
  TotalMemoryMB="{0:N2}" -f ($_.TotalVisibleMemorySize/1KB)
 FreeMemoryMB = "{0:N2}" -f ($_.FreePhysicalmemory/1KB)
  PercentMemoryFree = "{0:N2}" -f (($_.FreePhysicalmemory/$_.TotalVisibleMemorySize)*100)
}
}