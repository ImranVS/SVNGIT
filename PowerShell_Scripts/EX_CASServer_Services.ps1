param([string]$server)

$server = 'mail.jnittech.com'
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
		"MSExchangeHM",
		"MSExchangeIMAP4",
		"MSExchangePOP3",
		"MSExchangeServiceHost",
		"MSExchangeUMCR"
		)

$servicestates = @(Get-WmiObject -ComputerName $server -Class Win32_Service -ErrorAction STOP | where {$casservices -icontains $_.Name} | select name,state,startmode)

$servicestates
    
