
$t = @()
$t += get-spsite | % {
$URL = $_.PrimaryUri
 $BASETEMPLATE="Pages"


    $site = Get-SPSite $URL     
    $totalItems = 0     
    foreach($web in $site.AllWebs)     
    {         
        $lists = $web.Lists         
        for ($i=0; $i -le $lists.Count – 1; $i++)          
        {             
            $list = $lists[$i]        
            if ($list.Title -eq "Pages")             
            {                 
                $totalItems = $totalItems + $list.Items.Count       
             
            }         
        }             
    }     
    $site.Dispose()     
    $obj = New-Object -TypeName PSObject -Property @{
            "URL" = $URL
            "NumOfSites" = $totalItems
            "Owner" = $_.Owner
            "SizeMB" = [Math]::round($_.usage.storage/1MB,1)
            };
    $obj

}
$t
