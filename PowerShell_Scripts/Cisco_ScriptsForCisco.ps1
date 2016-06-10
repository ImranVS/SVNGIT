param([string] $IPAddress, [string] $UserName, [string] $Password)


$secpasswd = ConvertTo-SecureString $Password -AsPlainText -Force
$creds = New-Object System.Management.Automation.PSCredential ($UserName, $secpasswd)

#Build SSH session
$session = New-SSHSession -ComputerName $IPAddress -Credential $creds -AcceptKey $true
#Build open stream for use in cisco devices
$Stream = $session.Session.CreateShellStream("test", 0, 0, 0, 0, 1024)
$Writer = New-Object System.IO.StreamWriter($Stream)
$Buffer = New-Object System.Byte[] 1024 
$Encoding = New-Object System.Text.AsciiEncoding

$Commands = @("show environment all", "show memory free", 's', "show processes cpu", "s", "show version")

#Now start issuing the commands
ForEach ($Command in $Commands)
{   $Writer.WriteLine($Command) 
    $Writer.Flush()
    Start-Sleep -Milliseconds 100
}


Start-Sleep -Milliseconds (1000 * 4)
$Result = ""
#Save all the results
While($Stream.DataAvailable) 
{   $Read = $Stream.Read($Buffer, 0, 1024) 
    $Result += ($Encoding.GetString($Buffer, 0, $Read))
}

if($Result.Contains('Invalid input detected at'))
{

    $Writer = New-Object System.IO.StreamWriter($Stream)
    $Buffer = New-Object System.Byte[] 1024 
    $Encoding = New-Object System.Text.AsciiEncoding

    $Commands = @("show processes cpu | include utilization", 
    "show version | include gigabit|physical|Processor|ROM|Cisco IOS Software",
    'show platform | include Chassis',
    'show memory free | include Processor')

    #Now start issuing the commands
    ForEach ($Command in $Commands)
    {   $Writer.WriteLine($Command) 
        $Writer.Flush()
        Start-Sleep -Milliseconds 100
    }


    Start-Sleep -Milliseconds (1000 * 5)
    $Result = ""
    #Save all the results
    While($Stream.DataAvailable) 
    {   $Read = $Stream.Read($Buffer, 0, 1024) 
        $Result += ($Encoding.GetString($Buffer, 0, $Read))
    }

}



$Result

Get-SSHSession | Remove-SSHSession
