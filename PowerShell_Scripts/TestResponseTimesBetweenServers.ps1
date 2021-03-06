param($Servers, $TestDuration)
$serverResults = @();


    $cmd = {
        param($sourceServer, $destServer, $TestDuration, $serverResults) 
        $start = [DateTime]::Now

        $ping = New-Object System.Net.NetworkInformation.ping


        $PingResults = @()

        while([datetime]::now -le $start.AddMinutes($TestDuration)){ 
    
            $outping = $ping.send($destServer.Name)
            if($outping.Status -ne "Success")
            {

                $PingResults = $PingResults + 100

            } else{

                $PingResults = $PingResults + $outping.RoundtripTime

            }

            Start-Sleep 1

        } 
        #Write-Host $($PingResults | Measure-Object -Average).Average
        $results =  $($PingResults | Measure-Object -Average).Average

        return New-Object PSObject -Property @{
            From=$sourceServer.Name
            FromID=$sourceServer.ID
            To=$destServer.Name
            ToID=$destServer.ID
            AvgTime=[math]::Round($results,1)
        }
    }



foreach($sourceServer in $Servers)
{
    foreach($destServer in $Servers)
    {
        $t = Invoke-Command -ComputerName $sourceServer.Name -ScriptBlock $cmd -Credential $sourceServer.Creds -ArgumentList $destServer, $sourceServer, $TestDuration, $serverResults -AsJob -JobName "$($sourceServer.Name) to $($destServer.Name)"
    }
}


Start-Sleep -Seconds $($($TestDuration * ($60)))

While (Get-Job -State Running)
{
    Start-Sleep $(10)
}

foreach($sourceServer in $Servers)
{
    foreach($destServer in $Servers)
    {
       Receive-Job -Name "$($sourceServer.Name) to $($destServer.Name)"
       Remove-Job -Name "$($sourceServer.Name) to $($destServer.Name)"
    }
}

$serverResults

     