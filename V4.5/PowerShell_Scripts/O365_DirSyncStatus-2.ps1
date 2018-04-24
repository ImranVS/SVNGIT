$managementAgent = Get-WmiObject -Class MIIS_ManagementAgent -Namespace root/MicrosoftIdentityIntegrationServer

$filter = ("RunNumber='{0}'" -F $managementAgent.RunNumber().ReturnValue) 

Get-WmiObject -Class MIIS_RunHistory -Namespace root/MicrosoftIdentityIntegrationServer  |Where-Object {$_.RunNumber -Match "###"}

