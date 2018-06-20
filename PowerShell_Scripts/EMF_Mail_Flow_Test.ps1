param(
        $serverArray,
        $timeout
    )
function ConnectToExchange($server) {
    $secpasswd = ConvertTo-SecureString $server.Password -AsPlainText –Force;
    $cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $server.Username,$secpasswd;
    $so = New-PSSessionOption -SkipCNCheck -SkipRevocationCheck –SkipCACheck 
    $session = New-PSSession -ConfigurationName Microsoft.Exchange -Credential $cred -ConnectionUri "$($server.Uri)/powershell" -Authentication $server.Authentication -SessionOption $so
    Import-PSSession $session | Out-Null
    return $session
}

$allList = @()
foreach($source in $serverArray) {
    $session = ConnectToExchange -server $source
    foreach($destination in $serverArray) {
        if($source -eq $destination) { 
            continue;
        }
        Write-Host "$($source.Uri) to $($destination.Uri)"
        $result = try { Test-Mailflow $source.ServerName -TargetMailboxServer $destination.ServerName -ExecutionTimeout $timeout -ErrorAction SilentlyContinue -warningaction stop |Foreach-Object{New-Object PSObject -Property @{
                        SourceServer=$source.ServerName
                        TargetServer=$destination.ServerName
                        TestMailflowResult=$_.TestMailflowResult
                        MessageLatencyTime=$_.MessageLatencyTime
                    }}
                    } catch { 
                        New-Object PSObject -Property @{
                            SourceServer=$source.ServerName
                            TargetServer=$destination.ServerName
                            TestMailflowResult=$_
                            MessageLatencyTime=$null
                    }}
        $allList += $result
    }
    Remove-PSSession $session
}

$allList