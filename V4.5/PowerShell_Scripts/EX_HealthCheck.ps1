$resultObjArr = @()
$testObj = @()
$wc = New-Object System.Net.WebClient


$vdirs = @(
    "owa",
    "ecp",
    "oab",
    "rpc",
    "ews",
    "mapi",
    "Microsoft-Server-ActiveSync",
    "Autodiscover"
    )


function Test-CASURL() {

    param($obj)
    

    $testurl = "$($obj.URL)/healthcheck.htm"

    [System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
    try {
        $src = $wc.DownloadString($testurl)
    }
    catch
    {
        # Suppress exceptions
    }

    if ($src -like "200 OK*")
    {
        $result="OK"
    }
    else
    {
        $result="Error"
    }

    [System.Net.ServicePointManager]::ServerCertificateValidationCallback = $null


    $obj.Result = $result
    $obj.URL = $testurl
    


}


function Get-ExternalInternalURLS($UrlCmdlet, $TestServer, [ref]$testObj) {

    
    $Urls = @(& $UrlCmdlet -Server $CAS -AdPropertiesOnly | Select InternalURL,ExternalURL)
    foreach ($url in $Urls)
    {
        if ($($testObj | Where-Object { $_.URL -eq $url.InternalURL.AbsoluteUri -and $_.TestType -eq 'Internal URL'}).count -eq 0 -and 
            ($url.InternalURL.AbsoluteUri -ne $null))
        {
            
            $testObj.value += New-Object PSObject -Property @{
                TestType="Internal URL"
                URL = $url.InternalURL.AbsoluteUri
                Server = $TestServer
                Result = "N/A"
            }
            #'Adding 0 ' + $url.InternalURL.AbsoluteUri
        }
        if ($($testObj | Where-Object { $_.URL -eq $url.ExternalURL.AbsoluteUri -and $_.TestType -eq 'External URL'}).count -eq 0 -and 
            ($url.ExternalURL.AbsoluteUri -ne $null))
        {
            
            $testObj.value += New-Object PSObject -Property @{
                TestType="External URL"
                URL = $url.ExternalUrl.AbsoluteUri
                Server = $TestServer
                Result = "N/A"
            }
            #'Adding 1 ' + $url.ExternalUrl.AbsoluteUri
        }
    }
}

$ClientAccessServers = @(Get-ExchangeServer | Where {$_.IsClientAccessServer -and $_.AdminDisplayVersion -like "Version 15.*"})

$sites = @($ClientAccessServers | Group-Object -Property:Site | Select Name)

foreach ($site in $sites)
{
    $SiteName = ($Site.Name).Split("/")[-1]

    $CASinSite = @($ClientAccessServers | Where {$_.Site -eq $site.Name})

    foreach ($CAS in $CASinSite.Name)
    {
       
        Get-ExternalInternalURLS -UrlCmdlet "Get-OWAVirtualDirectory" -TestServer $CAS -testObj ([ref]$testObj)
        Get-ExternalInternalURLS -UrlCmdlet "Get-ECPVirtualDirectory" -TestServer $CAS -testObj ([ref]$testObj)
        Get-ExternalInternalURLS -UrlCmdlet "Get-OABVirtualDirectory" -TestServer $CAS -testObj ([ref]$testObj)
        Get-ExternalInternalURLS -UrlCmdlet "Get-WebServicesVirtualDirectory" -TestServer $CAS -testObj ([ref]$testObj)
        Get-ExternalInternalURLS -UrlCmdlet "Get-MAPIVirtualDirectory" -TestServer $CAS -testObj ([ref]$testObj)
        Get-ExternalInternalURLS -UrlCmdlet "Get-ActiveSyncVirtualDirectory" -TestServer $CAS -testObj ([ref]$testObj)

        #Write-Host "Getting RPC Urls"

        $OA = Get-OutlookAnywhere -Server $CAS -AdPropertiesOnly | Select InternalHostName,ExternalHostName
        [string]$OAInternalHostName = $OA.InternalHostName
        [string]$OAExternalHostName = $OA.ExternalHostName

        [string]$OAInternalUrl = "https://$OAInternalHostName/rpc"
        [string]$OAExternalUrl = "https://$OAExternalHostName/rpc"

        if ($($testObj | Where-Object { $_.URL -eq $OAInternalUrl -and $_.TestType -eq 'Internal URL'}).count -eq 0 -and 
            ($OAInternalHostName -ne $null))
        {
            $testObj += New-Object PSObject -Property @{
                TestType="Internal URL"
                URL = $OAInternalUrl
                Server = $CAS
                Result = "N/A"
            }
            #'Adding 2 ' + $OAInternalUrl
        }
        if ($($testObj | Where-Object { $_.URL -eq $OAExternalUrl -and $_.TestType -eq 'External URL'}).count -eq 0 -and 
            ($OAExternalHostName -ne $null) -and ($OAExternalHostName -ne ""))
        {
            $testObj += New-Object PSObject -Property @{
                TestType="External URL"
                URL = $OAExternalUrl
                Server = $CAS
                Result = "N/A"
            }
            #'Adding 3 ' + $OAExternalUrl
        }
    

        #Write-Host "Getting AutoDiscover Urls"

        $CASServer = Get-ClientAccessServer $CAS
        [string]$AutodiscoverSCP = (Get-ClientAccessServer $CAS).AutoDiscoverServiceInternalUri
        $CASAutodiscoverUrl = $AutodiscoverSCP.Replace("/Autodiscover.xml","")
        
        if ($($testObj | Where-Object { $_.URL -eq $CASAutodiscoverUrl -and $_.TestType -eq 'Internal URL'}).count -eq 0) 
        {
            #'Adding 4 ' + $CASAutodiscoverUrl
            $testObj += New-Object PSObject -Property @{
                TestType="Internal URL"
                URL = $CASAutodiscoverUrl
                Server = $CAS
                Result = "N/A"
            }
        }
    }
    

    $testType = "Per-Server Health Check"
    $ServerFQDNs = @($CASinSite.FQDN)

    foreach ($ServerFQDN in $ServerFQDNs)
    {
        
        #Write-Host "Server: $ServerFQDN"

        foreach ($vdir in $vdirs)
        {
            $src = $null
            $testurl = "https://$ServerFQDN/$vdir"

            if($($testObj | Where-Object { $_.URL -eq $testurl }).count -eq 0)
            {
                $testObj += New-Object PSObject -Property @{
                    TestType="Pre-Server Check"
                    URL = $testurl
                    Server = ($CASinSite | Where-Object {$_.FQDN -eq $ServerFQDN} | select Name).Name
                    Result = "N/A"
                }
            }
              
        }
    }

    foreach($obj in $testObj)
    {
        Test-CASURL $obj
    }
    
}

$testObj