$ssa = Get-SPEnterpriseSearchServiceApplication

$cl = New-Object Microsoft.Office.Server.Search.Administration.CrawlLog $ssa

$cl.GetCrawledUrls($false,1000000,"",$false,-1,2,-1,[datetime]::minvalue,[datetime]::maxvalue) #errors

