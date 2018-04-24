Clear-Variable 'results' -ErrorAction SilentlyContinue
 
#Write-Host "Collecting Recipients..." 
 
#Collect all recipients from Office 365 
$results = @()

$users = Get-Mailbox -WarningAction SilentlyContinue | select Name, EmailAddresses
$Recipients = Get-Recipient * -ResultSize Unlimited | select PrimarySMTPAddress 

$MailTraffic = @{} 
foreach($Recipient in $Recipients) 
{ 
    $MailTraffic[$Recipient.PrimarySMTPAddress.ToLower()] = @{} 
} 
$Recipients = $null 
 
#Collect Message Tracking Logs (These are broken into "pages" in Office 365 so we need to collect them all with a loop) 
$Messages = $null 
#$Page = 1 
#do 
#{ 
    #Write-Host "Collecting Message Tracking - Page $Page..." 
    $CurrMessages = Get-MessageTrace -StartDate ([DateTime]::Today.AddDays(-1)) -EndDate ([DateTime]::Today)| Select Received,SenderAddress,RecipientAddress,Size 
   # $Page++ 
    $Messages += $CurrMessages 
#} 
#until ($CurrMessages -eq $null) 
 
#Remove-PSSession $session 
 
#Write-Host "Crunching Results..." 
 
#Read each message tracking entry and add it to a hash table 
foreach($Message in $Messages) 
{ 
    if ($Message.SenderAddress -ne $null) 
    { 
        if ($MailTraffic.ContainsKey($Message.SenderAddress)) 
        { 
            $MessageDate = Get-Date -Date $Message.Received -Format yyyy-MM-dd 
             
            if ($MailTraffic[$Message.SenderAddress].ContainsKey($MessageDate)) 
            { 
                $MailTraffic[$Message.SenderAddress][$MessageDate]['Outbound']++ 
                $MailTraffic[$Message.SenderAddress][$MessageDate]['OutboundSize'] += $Message.Size
            } 
            else 
            { 
                $MailTraffic[$Message.SenderAddress][$MessageDate] = @{} 
                $MailTraffic[$Message.SenderAddress][$MessageDate]['Outbound'] = 1 
                $MailTraffic[$Message.SenderAddress][$MessageDate]['Inbound'] = 0 
				$MailTraffic[$Message.SenderAddress][$MessageDate]['InboundSize'] = 0
				$MailTraffic[$Message.SenderAddress][$MessageDate]['OutboundSize'] += $Message.Size
            } 
             
        } 
    } 
     
    if ($Message.RecipientAddress -ne $null) 
    { 
        if ($MailTraffic.ContainsKey($Message.RecipientAddress)) 
        { 
            $MessageDate = Get-Date -Date $Message.Received -Format yyyy-MM-dd 
             
            if ($MailTraffic[$Message.RecipientAddress].ContainsKey($MessageDate)) 
            { 
                $MailTraffic[$Message.RecipientAddress][$MessageDate]['Inbound']++ 
				$MailTraffic[$Message.RecipientAddress][$MessageDate]['InboundSize'] += $Message.Size
            } 
            else 
            { 
                $MailTraffic[$Message.RecipientAddress][$MessageDate] = @{} 
                $MailTraffic[$Message.RecipientAddress][$MessageDate]['Inbound'] = 1 
                $MailTraffic[$Message.RecipientAddress][$MessageDate]['Outbound'] = 0 
				$MailTraffic[$Message.RecipientAddress][$MessageDate]['OutboundSize'] = 0
				$MailTraffic[$Message.RecipientAddress][$MessageDate]['InboundSize'] += $Message.Size

			} 
        }     
    } 
} 


ForEach ($Recipient in $MailTraffic.keys) 
{ 
    $RecipientName = $Recipient 
    
    foreach($Date in $MailTraffic[$RecipientName].keys) 
    {
  $results += New-Object PSObject -Property @{ 
        ReceivedDate = $Date
        RecipientName = $RecipientName
        Inbound = $MailTraffic[$RecipientName][$Date].Inbound
        Outbound = $MailTraffic[$RecipientName][$Date].Outbound
        InboundSize = $MailTraffic[$RecipientName][$Date].InboundSize
        OutboundSize = $MailTraffic[$RecipientName][$Date].OutboundSize
      
        }

         
    } 
 
}
 
foreach($obj in $results)
{
    $obj
}
Clear-Variable 'results' -ErrorAction SilentlyContinue