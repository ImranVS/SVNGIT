
$results = @()

$users = Get-Mailbox -WarningAction SilentlyContinue | select Name, EmailAddresses

foreach($server in Get-TransportServer)
{
    $date = Get-Date
    $endDate = [DateTime]$(($date.Ticks) - $($date.Ticks % $(New-TimeSpan -Hours 1).ticks))

    $startDate = [DateTime]$($endDate.Ticks - $(New-TimeSpan -Hours 1).Ticks)
  
    $LogFiles  = Get-MessageTrackingLog -Server $server.Name -ResultSize ([Int]::MaxValue) -Start $startDate -End $endDate
    
    if($LogFiles -ne $null)
    {

        $SentAndReceived = $LogFiles | ? {$_.EventID -eq 'RECEIVE' -or $_.EventID -eq 'SEND'} 

        if($SentAndReceived -ne $null)
        {

            $SentMessages = $SentAndReceived | ? {$_.EventID -eq 'SEND'}

            $SentCount = 0

            if($SentMessages -ne $null)
            {
                $SentCount = 1 

                if($SentMessages.Length -ne $null) 
                { 
                    $SentCount = $SentMessages.Length 
                } 
            }




            $ReceivedMessages = $SentAndReceived | ? {$_.EventID -eq 'RECEIVE'}
        
            $ReceivedCount = 0

            if($ReceivedMessages -ne $null)
            {
                $ReceivedCount = 1 

                if($ReceivedMessages.Length -ne $null) 
                { 
                    $ReceivedCount = $ReceivedMessages.Length 
                } 

            }

            $SentSize = 0

            if($SentMessages -ne $null)
            {
                $SentSize = [Math]::Round(($SentMessages | Measure-Object -Property TotalBytes -Sum).Sum / 1MB,2)
            }
            
            $ReceivedSize = 0

            if($ReceivedMessages -ne $null)
            {
                $ReceivedSize = [Math]::Round(($ReceivedMessages | Measure-Object -Property TotalBytes -Sum).Sum / 1MB,2)
            }
        
        }

        
        $DeliverMessages = $LogFiles | ? {$_.EventID -eq 'DELIVER'}

        $DeliverCount = 0
        if($DeliverMessages -ne $null)
        {
            $DeliverCount = 1
            if($DeliverMessages.Length -ne $null)
            {
                $DeliverCount = $DeliverMessages.Length
            }
        }


        $FailMessages = $LogFiles | ? {$_.EventID -eq 'FAIL'}

        $FailCount = 0
        if($FailMessages -ne $null)
        {
            $FailCount = 1
            if($FailMessages.Length -ne $null)
            {
                $FailCount = $FailMessages.Length
            }
        }

	    $SucessRate = 1

        if($DeliverCount + $FailCount -eq 0)
        {
            $SucessRate = 1
        }
        else
        {
            $SuccessRate = [Math]::Round(($DeliverCount) / ($DeliverCount + $FailCount),2)           
        }

        $results += New-Object PSObject -Property @{ 
        Server = $server.Name
        StartTime = $startDate
        EndDate = $endDate
        MailDeliverySuccessRate = $SuccessRate
        ReceivedSizeMB = $ReceivedSize
        ReceivedCount = $ReceivedCount
        SentSizeMB = $SentSize
        SentCount = $SentCount
        DeliverCount = $DeliverCount
        FailCount = $FailCount
        Type="Server"
        }


        #For Users
        #$receivedTemp = $LogFiles | ? {$_.EventID -eq 'DELIVER' -and $_.Directionality -eq 'Originating'} | group-object -Property Sender | % {
        #    New-Object psobject -Property @{
        #        Name = $_.Name
        #        Count = ($_.Group.Count)
        #        SizeMB = (($_.Group | Measure-Object -Property TotalBytes -Sum).Sum) / 1MB
        #    }
        #}


        #$received = $LogFiles | ? {$_.EventID -eq 'DELIVER' -and $_.Directionality -eq 'Originating'} | group-object -Property Sender | select Count, Name | Sort-Object -Property Name
        #$sent = $LogFiles | ? {$_.EventID -eq 'RECEIVE' -and $_.Source -eq 'STOREDRIVER'}| select-object -ExpandProperty Recipients | group-object | select Count, Name | Sort-Object -Property Name


        $received = $LogFiles | ? {$_.EventID -eq 'DELIVER'}  | select-object * -ExpandProperty Recipients | group-object | % {
            New-Object psobject -Property @{
                Name = $_.Name
                Count = ($_.Group.Count)
                SizeMB = (($_.Group | Measure-Object -Property TotalBytes -Sum).Sum) / 1MB
            }
        }


        $sent = $LogFiles | ? {$_.EventID -eq 'RECEIVE' -and $_.Source -eq 'STOREDRIVER'} | group-object Sender  | % {
            New-Object psobject -Property @{
                Name = $_.Name
                Count = ($_.Group.Count)
                SizeMB = (($_.Group | Measure-Object -Property TotalBytes -Sum).Sum) / 1MB
            }
        }

       

        $ReturnObj = $users | ForEach-Object {
            $currName = $_.Name;
            $currEmails = $_.EmailAddresses | foreach{ $_.Substring($_.IndexOf(':') + 1)}
                
            $sentNum = $($sent | ? {$_.Name -in $currEmails}).Count
            $recNum = $($received | ? {$_.Name -in $currEmails}).Count
            $sentMB = $($sent | ? {$_.Name -in $currEmails}).SizeMB 
            $recMB = $($received | ? {$_.Name -in $currEmails}).SizeMB

            $sentMB = [math]::Round($sentMB, 2)
            $recMB = [math]::Round($recMB, 2)
            
            $currObj = $results | ? {$_.Type -eq "User" -and $_.Name -eq $currName}
            

            if($sentNum + $recNum -ne 0)
            {
                if($currObj -eq $null)
                {
                    $results +=New-Object -TypeName PSObject -Property @{
                                Name = $currName
                                Sent = $sentNum
                                Received = $recNum
                                SentSizeMB = $sentMB
                                RecSizeMB = $recMB
                                Type = "User"
                                }
                }
                else
                {
                    $currObj.Sent += $sentNum
                    $currObj.Received += $recNum
                    $currObj.SentSizeMB += $sentNum
                    $currObj.RecSizeMB += $recNum
                }
                
            }

        }
        




    }
    else
    {
        $results += New-Object PSObject -Property @{ 
        Server = $server.Name
        StartTime = $startDate
        EndDate = $endDate
        MailDeliverySuccessRate = 0
        ReceivedSizeMB = 0
        ReceivedCount = 0
        SentSizeMB = 0
        SentCount = 0
        DeliverCount = 0
        FailCount = 0
        Type="Server"
        }
    }
    

} 

foreach($obj in $results)
{
    $obj
}
