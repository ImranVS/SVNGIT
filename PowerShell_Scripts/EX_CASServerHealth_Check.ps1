param([string]$server)

$server = 'jnittech-exchg1.jnittech.com'
$e15casservicehealth = $null
$servicesrunning = @()
$servicesnotrunning = @()
$casservices = @(
		"IISAdmin",
		"W3Svc",
		"WinRM",
		"MSExchangeADTopology",
		"MSExchangeDiagnostics",
		"MSExchangeFrontEndTransport",
		#"MSExchangeHM",
		"MSExchangeIMAP4",
		"MSExchangePOP3",
		"MSExchangeServiceHost",
		"MSExchangeUMCR"
		)

$servicestates = @(Get-WmiObject -ComputerName $server -Class Win32_Service -ErrorAction STOP | where {$casservices -icontains $_.Name} | select name,state,startmode)

if (!($e15casservicehealth))
	{
		$servicesrunning = @($servicestates | Where {$_.StartMode -eq "Auto" -and $_.State -eq "Running"})
		$servicesnotrunning = @($servicestates | Where {$_.Startmode -eq "Auto" -and $_.State -ne "Running"})
		if ($($servicesnotrunning.Count) -gt 0)
		{
			Write-Verbose "Service health check failed"
		    Write-Verbose "Services not running:"
		    foreach ($service in $servicesnotrunning)
		    {
		        Write-Verbose "- $($service.Name)"	
		    }
			$e15casservicehealth = "Fail"	
		}
		else
		{
			Write-Verbose "Service health check passed"
			$e15casservicehealth = "Pass"
		}
	}
    
    
    $e15casservicehealth
    
