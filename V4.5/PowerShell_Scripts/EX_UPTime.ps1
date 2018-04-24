$objlist = @()
$Servers = Get-ExchangeServer

foreach($Server in $Servers)

{

$wmi=Get-WmiObject -class Win32_OperatingSystem -ComputerName $Server

$LBTime=$wmi.ConvertToDateTime($wmi.Lastbootuptime)

[TimeSpan]$uptime=New-TimeSpan $LBTime $(get-date)



$curr = New-Object PSObject -Property @{
		Server=$Server.Name;
        	UpDays=$Uptime.Days
		}
$objlist += ($curr)
        

}

ForEach($ob in $objlist)
{	
	Write-Output $ob
}
