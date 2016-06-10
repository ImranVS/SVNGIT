$uname= 'jnittech\administrator';
$Pwd='Pa$$w0rd';
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
$SESSION=New-PSSession -ComputerName ex13-1.jnittech.com -Credential $cred
Invoke-Command -Session $SESSION{
Import-Module -Name "C:\Windows\Temp\ExchangeSetup\Microsoft.Exchange.WebServices.dll" 
New-Variable -Name "MailboxUser" -Value "tarun" -Scope Script 
New-Variable -Name "MailboxPWD" -Value "ASdfg12#$%" -Scope Script 
New-Variable -Name "MailboxDomain" -Value "jnittech" -Scope Script 
New-Variable -Name "SenderAddress" -Value "tarun@jnittech.com" -Scope Script 
New-Variable -Name "RecipientAddress" -Value "echo@telcomplus.net" -Scope Script 
New-Variable -Name "WarningTime" -Value 30 -Scope Script 
New-Variable -Name "CriticalTime" -Value 60 -Scope Script 
New-Variable -Name "EWSUrl" -Value "https://mail.jnittech.com/EWS/Exchange.asmx" -Scope Script 
$ExchangeVersion = "Exchange2013" 
Function SendMail() 
{ 
    $RandId = GenerateRandomID 
    $Mail = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage($EWS) 
    $Mail.Subject = "Test-MailFlow-EWS - $RandId" 
    $Mail.Body = "This is a Monitoring-Message send from Test-MailFlow-EWS.ps1<br><br>Timestamp: " + (Get-Date) 
    $Mail.ToRecipients.Add($RecipientAddress) 
    $Mail.From = $SenderAddress 
    $Mail.Send.Invoke() 
    Return $RandId 
} 
 
#Creates our RandomID which will be used to identify the correct message 
Function GenerateRandomID() 
{ 
    $MsgID = $Null 
    $Rand = New-Object System.Random 
    1..21 | %{ $MsgID = $MsgID + [char]$Rand.Next(65,90) + [char]$Rand.Next(48,57) + [char]$Rand.Next(97,122) } 
 
    Return $MsgID 
 
} 
 
Function GetItems($EWS) 
{ 
    #Opens the Inbox and define to get 42 Items 
    $Inbox = New-Object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Inbox, $SenderAddress) 
    $ItemView = New-Object Microsoft.Exchange.WebServices.Data.itemView(42) 
 
    #Finally really get the Data 
    $Data = $EWS.FindItems($Inbox, $ItemView) 
     
    Return $Data 
} 
 
# 
# Start of the Script 
# 
 
$EWS = New-Object Microsoft.Exchange.WebServices.Data.ExchangeService([Microsoft.Exchange.WebServices.Data.ExchangeVersion]::$ExchangeVersion) 
 
# Set the Credentials 
$EWS.Credentials = new-object Microsoft.Exchange.WebServices.Data.WebCredentials($MailboxUser, $MailboxPWD, $MailboxDomain) 
 
 
# Set EWS URL 
$EWS.Url = New-Object Uri($EWSUrl) 
 
#Create StopWatch to measure the time 
$sw = New-Object Diagnostics.Stopwatch 
 
 
# Send E-Mail 
$RandID = (SendMail)[1] 
$sw.Start() 
 
$MailFlow = $False 
 
while($MailFlow -eq $False) 
{ 
    $Data = GetItems($EWS) 
 
    ForEach($I in $Data) 
    { 
        If($I.Subject -like "*$RandID") 
        { 
            $MailFlow = $True 
            $sw.Stop() 
            $I.Delete("HardDelete") 
        } 
    } 
 
    If($sw.Elapsed.TotalSeconds -ge $CriticalTime) 
    { 
        $sw.Stop() 
        break 
    } 
     
    Start-Sleep -Milliseconds 500 
 
} 
 
If($MailFlow) 
{ 
    $TimeTaken = $sw.Elapsed.TotalSeconds 
         
    If($TimeTaken -le $WarningTime) 
    { 
        $Color = "Green" 
        $Message = "Mail received within $TimeTaken Seconds. Everything looks OK" 
        $RetVal = 0 
    }ElseIf( ($TimeTaken -gt $WarningTime) -and ($TimeTaken -le $CriticalTime) ) 
    { 
        $Color = "Yellow" 
        $Message = "Mail received within $TimeTaken Seconds. Took longer than expected" 
        $RetVal = 1 
    }Else{ 
        $Color = "Magenta" 
        $Message = "Mail received within $TimeTaken Seconds. It's quite long, but at least we got something." 
        $RetVal = 2 
    } 
 
         
}Else{ 
        $Color = "Red" 
        $Message = "Didn't received the Mail. Something is wrong!" 
        $RetVal = 3 
} 
 


    "{0}|'email roundtrip'={1:N2}s;{2};{3}" -f $Message, $TimeTaken, $WarningTime, $CriticalTime  

 
exit $RetVal
}