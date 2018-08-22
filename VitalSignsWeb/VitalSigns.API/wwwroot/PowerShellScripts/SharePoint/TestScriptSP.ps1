$Sites=get-spsite
foreach ($site in $sites){test-spsite -Identity $site.Url}