#uncomment for testign in powershell ise
#$databases = Get-MailboxDatabase -IncludePreExchange2013 -status | sort name

Function New-Array {,$args}
$Report = new-array


foreach ($db in $databases){
 $name = $db.Name 
 $Size = $("{0:f2}" -f ($db.DatabaseSize.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)
 $whiteSpace = $("{0:f2}" -f ($db.AvailableNewMailboxSpace.ToString().Split("(")[1].Split(" ")[0].Replace(",","")/1MB),2)
 $mailboxes = Get-MailboxStatistics -Database "$db"
 $mbxs = $mailboxes | Where {$_.DisconnectDate -eq $null} | Measure-Object
 #number of disconnected mailboxes 
 $mbxs_disc = $mailboxes | Where {$_.DisconnectDate -ne $null} | Measure-Object
 #$mbcount = [int]$mbxs + [int]$mbxs_disc

 $tmp = New-Object System.Object
 $tmp | Add-Member -type NoteProperty -name ServerName -value $db.ServerName
 $tmp | Add-Member -type NoteProperty -name DataBaseName -value $name
 $tmp | Add-Member -type NoteProperty -name SizeMB -value $size
 $tmp | Add-Member -type NoteProperty -name WhiteSpaceMB -value $whitespace
 $tmp | Add-Member -type NoteProperty -name MBXs -value $mbxs.count
 $tmp | Add-Member -type NoteProperty -name MBXsdisc -value $mbxs_disc.count
 $tmp | Add-Member -type NoteProperty -name Mbcount -value ($mbxs_disc.count + $mbxs.count)
 $tmp | Add-Member -type NoteProperty -Name DagName -Value ($db.MailboxProvisioningAttributes.Attributes | ? {$_.Key -eq "DagName" }).Value

 $Report += $tmp
}

$Report