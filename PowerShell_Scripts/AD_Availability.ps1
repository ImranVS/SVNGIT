#$computers = Get-ADDomainController -Filter * | where {$_.Hostname -eq  "jnittech-ad.jnittech.com"}
function Test-TCPPortConnection {

[CmdletBinding()][OutputType('System.Management.Automation.PSObject')]

param(

[Parameter(Position=0,Mandatory=$true,HelpMessage="Name of the computer to test",
 ValueFromPipeline=$True,ValueFromPipelineByPropertyName=$true)]
 [Alias('CN','__SERVER','IPAddress','Server')]
 [String[]]$ComputerName,

 [Parameter(Position=1)]
 [ValidateRange(1,65535)]
 [Int[]]$Port = 3389
 )

begin {
 $TCPObject = @()
 }

 process {

 foreach ($Computer in $ComputerName){

foreach ($TCPPort in $Port){

 $Connection = New-Object Net.Sockets.TcpClient

 try {

 $Connection.Connect($Computer,$TCPPort)

 if ($Connection.Connected) {

$Response = “Open”
 $Connection.Close()
 }

 }

 catch [System.Management.Automation.MethodInvocationException]

 {
 $Response = “Closed / Filtered”
 }

 $Connection = $null

 $hash = @{

 ComputerName = $Computer
 Port = $TCPPort
 Response = $Response
 }
 $Object = New-Object PSObject -Property $hash
 $TCPObject += $Object
return $TCPObject
 }
 }
 }

end {

#$TCPObject
 }
}

$objArr = @()


 ForEach ($computer in $computers) {
$client = $Computer.Name
$objArr+=Test-tcpportconnection -Port 389 -Computername $client
}
$objArr