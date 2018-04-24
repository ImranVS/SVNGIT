
#param(
	#[Parameter(ParameterSetName='server')] [string]$server
#)

#$server |
# %{
      $RPC = Get-Counter "\MSExchange RpcClientAccess\User Count" 
      $OWA = Get-Counter "\MSExchange OWA\Current Unique Users" 
      New-Object PSObject -Property @{
        "RPC Client Access" = $RPC.CounterSamples[0].CookedValue
        "Outlook Web App" = $OWA.CounterSamples[0].CookedValue
      }
  #    }

#$server = 'win-kurrr2qsdj0.jnittech.com'

#Get-Counter “\MSExchange RpcClientAccess\User Count”  
#Get-Counter “\MSExchange OWA\Current Unique Users” 

#Get-Counter -computername (get-content computers.txt)  '\MSExchange RpcClientAccess\User Count'  
#Get-Counter  -ComputerName $server  -Counter “\MSExchange OWA\Current Unique Users” 

