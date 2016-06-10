[array]$ComputerName = @()
Get-ExchangeServer | Where-Object { $_.ServerRole -ilike "*Mailbox*" } | % { $ComputerName += $_.name }
    [array]$newFolderPath = @()
    foreach($srv in $ComputerName)
    {
        foreach($f in $FolderPath)
        {
            Get-PublicFolder $f | ForEach-Object { if ($newFolderPath -inotcontains $_.Identity) { $newFolderPath += $_.Identity } }
        }
    }
    $FolderPath = $newFolderPath

# Get statistics for all public folders on all selected servers
# This is significantly faster than trying to get folders one by one by name
[array]$publicFolderList = @()
[array]$nameList = @()
foreach($server in $ComputerName)
{ 
    $pfOnServer = $null
    $pfOnServer = Get-PublicFolderStatistics -ResultSize Unlimited -ErrorAction SilentlyContinue
    $pfOnServer.FolderPath
    if ($FolderPath.Count -gt 0)
    {
        $pfOnServer = $pfOnServer | Where-Object { $FolderPath -icontains "\$($_.FolderPath)" }
    }
    if ($pfOnServer -eq $null) { continue }
    $publicFolderList += New-Object PSObject -Property @{"ComputerName" = $server; "PublicFolderStats" = $pfOnServer}
    $pfOnServer | Foreach-Object { if ($nameList -inotcontains $_.FolderPath) { $nameList += $_.FolderPath } }
}

$nameList = [array]$nameList | Sort-Object
[array]$ResultMatrix = @()
foreach($folder in $nameList)
{ 
    $resultItem = @{}
    $maxBytes = 0
    $maxSize = $null
    $maxItems = 0
    foreach($pfServer in $publicFolderList)
    { 
        $pfData = $pfServer.PublicFolderStats | Where-Object { $_.FolderPath -eq $folder }
        $pfData |fl
        if ($pfData -eq $null) { Write-Verbose "Skipping $pfServer.CompuerName for $folder"; continue }
        if (-not $resultItem.ContainsKey("FolderPath"))
        {
            $resultItem.Add("FolderPath", "\$($pfData.FolderPath)")
        }
        if (-not $resultItem.ContainsKey("Name"))
        {
            $resultItem.Add("Name", $pfData.Name)
        }
        if ($resultItem.Data -eq $null)
        {
            $resultItem.Data = @()
        }
        $currentItems = $pfData.ItemCount
        $currentSize = $pfData.TotalItemSize
        #$currentSize = '511.1 KB (523,336 bytes)'
        $currentSize = $currentSize.Substring($currentSize.IndexOf("(") + 1)
        $currentSize
        $currentSize = $currentSize.Substring(0, $currentSize.IndexOf(" "))
      
        if ($currentItems -gt 0)
        {
            $maxItems = $currentItems
        }
        if ( -not $currentSize.Contains("(0 bytes)"))
        {
            $maxSize = $currentSize
            $maxBytes = $currentSize
        }
        $resultItem.Data += New-Object PSObject -Property @{"ComputerName" = $pfServer.ComputerName;"TotalItemSize" = $currentSize; "ItemCount" = $currentItems}
    }
    $resultItem.Add("TotalItemSize", $maxSize)
    $resultItem.Add("TotalBytes", $maxBytes)
    $resultItem.Add("ItemCount", $maxItems)
    $replCheck = $true
    foreach($dataRecord in $resultItem.Data)
    {
        if ($maxItems -eq 0)
        {
            $progress = 100
        } else {
            $progress = ([Math]::Round($dataRecord.ItemCount / $maxItems * 100, 0))
        }
        if ($progress -lt 100)
        {
            $replCheck = $false
        }
        $dataRecord | Add-Member -MemberType NoteProperty -Name "Progress" -Value $progress
    }
    $resultItem.Add("ReplicationComplete", $replCheck)
    $ResultMatrix += New-Object PSObject -Property $resultItem
    if (-not $AsHTML)
    {
        New-Object PSObject -Property $resultItem
        
    }
}


    $html = @"
<html>
<style>
body
{
font-family:Arial,sans-serif;
font-size:8pt;
}
table
{
border-collapse:collapse;
font-size:8pt;
font-family:Arial,sans-serif;
border-collapse:collapse;
min-width:400px;
}
table,th, td
{
border: 1px solid black;
}
th
{
text-align:center;
font-size:18;
font-weight:bold;
}
</style>
<body>
<font size="1" face="Arial,sans-serif">
<h1 align="center">Exchange Public Folder Replication Report</h1>
<h4 align="center">Generated $([DateTime]::Now)</h3>

</font><h2>Overall Summary</h2>
<table border="0" cellpadding="3">
<tr style="background-color:#B0B0B0"><th colspan="2">Public Folder Environment Summary</th></tr>
<tr><td>Servers Selected for this Report</td><td>$($ComputerName -join ", ")</td></tr>
<tr><td>Servers Selected with Public Folders Present</td><td>$(
$serverList = @()
$publicFolderList | ForEach-Object { $serverList += $_.ComputerName }
$serverList -join ", "
)</td></tr>
<tr><td>Number of Public Folders</td><td>$($TotalCount = $ResultMatrix.Count; $TotalCount)</td></tr>
<tr><td>Total Size of Public Folders</td><td>$(
$totalSize = $null
$ResultMatrix | Foreach-Object { $totalSize += $_.TotalItemSize }
$totalSize
)</td></tr>
<tr><td>Average Folder Size</td><td>$($totalSize / $TotalCount)</td></tr>
<tr><td>Total Number of Items in Public Folders</td><td>$(
$totalItemCount = $null
$ResultMatrix | Foreach-Object { $totalItemCount += $_.ItemCount }
$totalItemCount
)</td></tr>
<tr><td>Average Folder Item Count</td><td>$([Math]::Round($totalItemCount / $TotalCount, 0))</td></tr>
</table>
<br />
<table border="0" cellpadding="3">
<tr style="background-color:#B0B0B0"><th colspan="4">Folders with Incomplete Replication</th></tr>
<tr style="background-color:#E9E9E9;font-weight:bold"><td>Folder Path</td><td>Item Count</td><td>Size</td><td>Servers with Replication Incomplete</td></tr>
$(
[array]$incompleteItems = $ResultMatrix | Where-Object { $_.ReplicationComplete -eq $false }
if (-not $incompleteItems.Count -gt 0)
{
    "<tr><td colspan='4'>There are no public folders with incomplete replication.</td></tr>"
} else {
    foreach($result in $incompleteItems)
    {
        "<tr><td>$($result.FolderPath)</td><td>$($result.ItemCount)</td><td>$($result.TotalItemSize)</td><td>$(($result.Data | Where-Object { $_.Progress -lt 100 }).ComputerName -join ", ")</td></tr>`r`n"
    }
}
)
</table>
<br />
<table border="0" cellpadding="3">
<tr style="background-color:#B0B0B0"><th colspan="3">Largest Public Folders</th></tr>
<tr style="background-color:#E9E9E9;font-weight:bold"><td>Folder Path</td><td>Item Count</td><td>Size</td></tr>
$(
[array]$largestItems = $ResultMatrix | Sort-Object TotalItemSize -Descending | Select-Object -First 10
if (-not $largestItems.Count -gt 0)
{
    "<tr><td colspan='3'>There are no public folders in this report.</td></tr>"
} else {
    foreach($sizeResult in $largestItems)
    {
        "<tr><td>$($sizeResult.FolderPath)</td><td>$($sizeResult.ItemCount)</td><td>$($sizeResult.TotalItemSize)</td></tr>`r`n"
    }
}
)
</table>

</font><h2>Public Folder Replication Results</h2>
<table border="0" cellpadding="3">
<tr style="background-color:#B0B0B0"><th colspan="$($publicFolderList.Count + 1)">Public Folder Replication Information</th></tr>
<tr style="background-color:#E9E9E9;font-weight:bold"><td>Folder Path</td>
$(
foreach($rServer in $publicFolderList)
{
    "<td>$($rServer.ComputerName)</td>"
}
)
</tr>
$(
if (-not $ResultMatrix.Count -gt 0)
{
    "<tr><td colspan='$($publicFolderList.Count + 1)'>There are no public folders in this report.</td></tr>"
}
foreach($rItem in $ResultMatrix)
{
    "<tr><td>$($rItem.FolderPath)</td>"
    foreach($rServer in $publicFolderList)
    {
        $(
        $rDataItem = $rItem.Data | Where-Object { $_.ComputerName -eq $rServer.ComputerName }
        if ($rDataItem -eq $null)
        {
            "<td>N/A</td>"
        } else {
            if ($rDataItem.Progress -ne 100)
            {
                $color = "#FC2222"
            } else {
                $color = "#A9FFB5"
            }
            "<td style='background-color:$($color)'><div title='$($rDataItem.TotalItemSize) of $($rItem.TotalItemSize) and $($rDataItem.ItemCount) of $($rItem.ItemCount) items.'>$($rDataItem.Progress)%</div></td>"
        }
        )
    }
    "</tr>"
}
)
</table>
</body>
</html>
"@
$html | Out-File C:\Users\DJNiViS\Desktop\PubLicFolderRepl.html
