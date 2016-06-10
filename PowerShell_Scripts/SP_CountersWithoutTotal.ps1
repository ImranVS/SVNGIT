param($arrOfCounters)

$t =
(

Get-Counter -Counter $arrOfCounters
).CounterSamples 

for($i=0; $i -lt $t.length; $i++)
{
    $t[$i].Path = $t[$i].Path -replace '[(].*[)]','(*)'
}

$temp = $t | Group-Object Path | %{
    New-Object psobject -Property @{
        Path=$_.Name
        CookedValue = ($_.Group | Measure-Object CookedValue -Sum).Sum
    }
}

$temp 