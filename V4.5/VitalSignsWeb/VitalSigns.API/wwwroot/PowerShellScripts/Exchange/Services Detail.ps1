<#
.DESCRIPTION
Displays run status all Exchange-related services.

#>
$results = @()
Test-ServiceHealth | % { 
    $_.ServicesRunning | % { 
        $results += New-Object PSObject -Property @{ Name= $_; Status="Running"}
    }
    $_.ServicesNotRunning | % { 
        $results += New-Object PSObject -Property @{ Name= $_; Status="Stopped"}
    }
}
$results | sort Name -Unique