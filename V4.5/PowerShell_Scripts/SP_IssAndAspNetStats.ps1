$IssVersion = (get-itemproperty HKLM:\SOFTWARE\Microsoft\InetStp\).SetupString
$IssStatus = (get-wmiobject Win32_Service -Filter "name='IISADMIN'").State


$AspNetVersions = ((Get-ChildItem 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP' -recurse |
Get-ItemProperty -name Version -EA 0 |
Where { $_.PSChildName -match '^(?!S)\p{L}'} |
Select Version) | %{ ($_.Version.Split('.')[0] + '.' + $_.Version.Split('.')[1])} | 
Sort-Object -Property $_ -Unique) -join ', '

New-Object PSObject -Property @{
    ISS_Version=$IssVersion
    ISS_Status=$IssStatus
    ASPNetVersions=$AspNetVersions
}
