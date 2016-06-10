$uname= $args[0];
$pwd= $args[1];
$secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
$cred=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
Find-Module O365ServiceCommunications | Install-Module
Import-Module O365ServiceCommunications
$MySession = New-SCSession -Credential $cred

$Servicedata= Get-SCEvent -SCSession $MySession  -EventTypes Incident -PastDays 30

 $results = @()
ForEach ($data in $Servicedata) 
{ 
      $results += New-Object PSObject -Property  @{ 
        Id = $data.ID
        StartTime = $data.StartTime
        EndTime = $data.EndTime
        Status = $data.Status
        EventType = $data.EventType
        Message=  $data.Messages[0].MessageText
        ServiceName=  $data.AffectedServiceHealthStatus.ServiceName
       
         }
}
 
foreach($obj in $results)
{
    $obj
}