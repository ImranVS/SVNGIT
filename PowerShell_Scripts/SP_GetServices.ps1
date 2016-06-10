#Invoke-Command -ComputerName $serverName -Credential $cred -ScriptBlock {

$Services = 'DCLauncher15','DCLoadBalancer15','OSearch15','SPAdminV4','SPSearchHostController','SPTimerV4','SPTraceV4','SPUserCodeV4', 
'SPWriterV4', 'AppFabricCachingService', 'W3SVC'

Get-Service | ? {$Services -contains $_.Name} | select Name, DisplayName, Status

#} | select Name, DisplayName, Status