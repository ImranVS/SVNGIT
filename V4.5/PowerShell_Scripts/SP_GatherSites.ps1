function RecurseSiteAndDoSomething() {
    param([Microsoft.SharePoint.SPWeb]$SiteIdentity)

    #Write-Output "Site: $($SiteIdentity.Url)"
    
    if($SiteIdentity.Webs.Count -gt 0)
    {
        foreach($subWeb in $SiteIdentity.Webs)
        {
            RecurseSiteAndDoSomething -SiteIdentity $subWeb + ","
        }

    }
    
    $returnArr+= $($SiteIdentity.Url)
    return $returnArr
}

$contentWebAppServices = (Get-SPFarm).services | ? {$_.typename -eq "Microsoft SharePoint Foundation Web Application"}
$returnArr = @()
$objArr = @()

foreach($webApp in $contentWebAppServices.WebApplications)
{
    #Write-Output "Web Application: $($webApp.name)"
    foreach($siteColl in $webApp.Sites)
    {
        #Write-Output "Site Collection: $($siteColl.Url)"
        $returnObj = RecurseSiteAndDoSomething -SiteIdentity $($siteColl.RootWeb)
        

        #$returnObj

        
        foreach($obj in $returnObj)
        {
            $objArr+=New-Object PSObject -Property @{
    				WebApp=$($webApp.name)
    				SiteColl=$($siteColl.Url)
    				Site=$obj
                    }
        }
    }
} 

$objArr