#$ServerName = 'jnittech-exchg1'
$servers = Get-ExchangeServer -Identity $serverName | select Name, ServerRole, IsE14OrLater, IsExchange2007OrLater, AdminDisplayVersion
$arr = @()
foreach($serv in $servers)
{
    $version='0'
    if($serv.IsExchange2007OrLater)
    {
        if($serv.IsE14OrLater)
        {
            if($serv.AdminDisplayVersion -like 'Version 14.*')
            {
                $version='2010'
            }
            else
            {
                $version='2013'
            }
        }
        else
        {
            $version='2007'
        }
    }
    else
    {
        $version='0'
    }
    $memberObj = New-Object PSObject -Property @{
        Name=$serv.Name
        Version=$version
        Roles=$serv.ServerRole
    }
    $arr += $memberObj
}

$arr