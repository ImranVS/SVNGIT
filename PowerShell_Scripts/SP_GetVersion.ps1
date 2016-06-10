$verNum = (Get-PSSnapin microsoft.sharepoint.powershell).Version
$year = ''
if($verNum.Major -eq 14)
{
    $year = 2010
}
elseif($vernum.Major -eq 15)
{
    $year = 2013
}

New-Object PSObject -Property @{ 
DisplayName = "$year" 
Build = "$($verNum.Major).$($verNum.Minor).$($verNum.Build).$($verNum.Revision)" 
}