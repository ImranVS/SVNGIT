<#
.DESCRIPTION
Runs a health check on the sites and their content
#>
$Sites=get-spsite
foreach ($site in $sites){test-spsite -Identity $site.Url}