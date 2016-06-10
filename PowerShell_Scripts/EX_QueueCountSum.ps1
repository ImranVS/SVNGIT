$Array=@()
$ExchangeServers = Get-ExchangeServer
foreach($ExchangeServer in $ExchangeServers)
{# Integers
[int]$ExchangeActiveConnections = 0;
[int]$ExchangeRetryQueue = 0;
[int]$ExchangeMessageQueue = 0;
[int]$ExchangeActiveQueue = 0;
[int]$HostMessageQueue = 0;
[int]$HostRetryQueue = 0;
[int]$ShadowQueue = 0;
[int]$SubmissionQueue = 0;
if ($ExchangeServer.IsHubTransportServer -eq $True)
{
	# Message Queue
	$QueueList = Get-Queue -Server $ExchangeServer.Name #| Where {$_.Status -eq "Ready" -and $_.MessageCount -gt "0"}
 
    $MessageQueue = $QueueList| Where {$_.DeliveryType -eq 'ShadowRedundancy' -and $_.MessageCount -gt "0"}

	# Sum Message Queue
	Foreach ($Queue in $MessageQueue)
	{
		[int]$ShadowQueue = [int]$ShadowQueue + [int]$Queue.MessageCount
	}

    $MessageQueue = $QueueList| Where {$_.NextHopDomain -eq 'Submission' -and $_.MessageCount -gt "0"}

	# Sum Message Queue
	Foreach ($Queue in $MessageQueue)
	{
		[int]$SubmissionQueue = [int]$SubmissionQueue + [int]$Queue.MessageCount
	}
 
	# Retry Queue
	$RetryQueue = $QueueList | Where {$_.Status -eq "Retry" -and $_.MessageCount -gt "0"}
 
	# Sum Retry Queue
	Foreach ($Queue in $RetryQueue)
	{
		[int]$HostRetryQueue = [int]$HostRetryQueue + [int]$Queue.MessageCount
	}
 
	# Exchange Queue
	#[int]$ExchangeRetryQueue = [int]$ExchangeRetryQueue + [int]$HostRetryQueue
	#[int]$ExchangeMessageQueue = [int]$ExchangeMessageQueue + [int]$HostMessageQueue
	[int]$ExchangeActiveQueue =  [int]$HostRetryQueue + [int]$ShadowQueue + [int]$SubmissionQueue


$Array+=New-Object PSObject -Property @{
RetryQueue=$HostRetryQueue
Submission=$SubmissionQueue
Shadow=$ShadowQueue
ActiveQueue=$ExchangeActiveQueue
ExchangeServer=$ExchangeServer
}
}

}
$Array