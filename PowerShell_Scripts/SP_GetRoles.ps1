
$servers = @()
$servers += Get-SPServer | select Name, Role, Farm 
$servers | foreach-object { 
 $_.Farm = $($_.Farm.Farm.Name)
 }


Start-SPAssignment -Global    $farm = Get-SPFarm    foreach ($server in $farm.Servers)    {        foreach ($svc in $server.ServiceInstances)        {            if($svc -is [Microsoft.SharePoint.Administration.SPDatabaseServiceInstance])            {
                $svr = ''
                $role="Database"                $s  = [Microsoft.SharePoint.Administration.SPDatabaseServiceInstance]$svc;
                                if([System.string]::IsNullOrEmpty($s.Instance))                {                    $svr = $svc.DisplayName                }                else                {                     $svr = $svc.DispalyName + "\" + $s.Instance                }

                $servers  | where {$_.Name -eq $svr} | foreach-object { $_.Role = $role}

                            }        }    }Stop-SPAssignment -Global

$servers | foreach-object {$_.Name = [System.Net.Dns]::GetHostByName($_.Name).HostName}

$servers