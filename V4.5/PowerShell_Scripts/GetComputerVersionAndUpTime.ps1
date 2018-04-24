$wmi=Get-WmiObject -class Win32_OperatingSystem

$LBTime=$wmi.ConvertToDateTime($wmi.Lastbootuptime)

[TimeSpan]$uptime=New-TimeSpan $LBTime $(get-date)



New-Object PSObject -Property @{
        UpDays=$Uptime.Days;
        ComputerVersion=$wmi.Caption;
	}