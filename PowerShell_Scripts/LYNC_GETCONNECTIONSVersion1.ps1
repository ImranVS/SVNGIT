<#
.SYNOPSIS
	Returns current connection count for all front end servers in a given pool
	including a breakdown of connection by client, frontend server and users.

	It can also be used to return connection information on an individual user.

.DESCRIPTION
	This program will return a connection count for a given pool. The program
	can be edited to set a default pool. You will also be able to get
	information on an individual user by providing the users SamAccountName.
	As well as listing all the connected users to the default pool.

	NOTE: In order to gain remote access to each front end server's
	RTCLOCAL database where connection information is found,
	you need to open two local Windows firewall ports. Please see the installation
	info on the blog post for details.

.NOTES
  Version      	   		: 2.8 - See changelog at http://www.ehloworld.com/681
	Wish list						:
  Rights Required			: TBD
  Sched Task Required	: No
  Lync Version				: 2010 through CU7 and 2013 through CU3
  Author(s)    				: Pat Richard (pat@innervation.com) 	http://www.ehloworld.com @patrichard
  										: Tracy A. Cerise (tracy@uky.edu)
  										: Mahmoud Badran (v-mabadr@microsoft.com)
  Dedicated Post			: http://www.ehloworld.com/269
  Disclaimer   				: You running this script means you won't blame me if this breaks your stuff. This script is provided AS IS
												without warranty of any kind. I disclaim all implied warranties including, without limitation, any implied
												warranties of merchantability or of fitness for a particular purpose. The entire risk arising out of the use
												or performance of the sample scripts and documentation remains with you. In no event shall I be liable for
												any damages whatsoever (including, without limitation, damages for loss of business profits, business
												interruption, loss of business information, or other pecuniary loss) arising out of the use of or inability
												to use the script or documentation.
  Acknowledgements 		: http://blogs.technet.com/b/meacoex/archive/2011/07/19/list-connections-and-users-connected-to-lync-registrar-pool.aspx
  									  	This program's database connection information was originally taken from the "List Connections to Registrar Pools" submitted by Scott Stubberfield and Nick Smith from Microsoft to the Lync 2010 PowerShell blog  (http://blogs.technet.com/b/csps/) on June 10, 2010.
	Assumptions					: ExecutionPolicy of AllSigned (recommended), RemoteSigned or Unrestricted (not recommended)
  Limitations					:
  Known issues				: None yet, but I'm sure you'll find some!

.LINK
	http://www.ehloworld.com/269

.EXAMPLE
	.\Get-CsConnections.ps1

	Description
	-----------
	Returns information on all connections on all frontend servers in the
	specified pool or server.

.EXAMPLE
	.\Get-CsConnections.ps1 -PoolFqdn [pool FQDN]

	Description
	-----------
	Returns information on all connections on all frontend servers in the
	Lync Server 2010 pool FQDN given with the pool parameter.

.EXAMPLE
	.\Get-CsConnections.ps1 -PoolFqdn [pool FQDN] -Is2013

	Description
	-----------
	Returns information on all connections on all frontend servers in the
	Lync Server 2013 pool FQDN given with the pool parameter.

.EXAMPLE
	.\Get-CsConnections.ps1 -PoolFqdn [pool name]

	Description
	-----------
	Returns information on all connections on all Lync Server frontend servers
	in the pool NetBIOS name given with the pool parameter. The script will append
	the %userdnsdomain% to build the FQDN.

.EXAMPLE
	.\Get-CsConnections.ps1 -SIPAddress userid@sip.domain

	Description
	-----------
	Returns all connection information for the given user including which
	frontend server connected to, how many connections and which clients
	connected with. If the environment contains only one SIP domain, only the prefix
	is required.

.EXAMPLE
	.\Get-CsConnections.ps1 -FilePath c:\path\to\file\filename.csv

	Description
	-----------
	Returns information on all connections on all frontend servers in the
	pool and in addition writes out all the raw connection information
	into the filename specified.

.EXAMPLE
	.\Get-CsConnections.ps1 -IncludeUsers

	Description
	-----------
	Returns information on all connections on all frontend servers in the
	specified pool and in addition writes out all the users connection
	information. This does NOT include system related connections.

.EXAMPLE
	.\Get-CsConnections.ps1 -IncludeUsers -UserHighConnectionFlag 5

	Description
	-----------
	Returns information on all connections on all frontend servers in the
	specified pool and in addition writes out all the users connection
	information. This does NOT include system related connections. The
	UserHighConnectionFlag integer will color	all users with that many
	(or more) connections in red. The default is 4.

.EXAMPLE
	.\Get-CsConnections.ps1 -IncludeHighUsers -UserHighConnectionFlag 5

	Description
	-----------
	Returns information on all connections on all frontend servers in the
	specified pool and in addition writes out all the users connection
	information for users with high number of connections. This does NOT
	include system related connections. The UserHighConnectionFlag integer
	will color	all users with that many (or more) connections in red.
	The default is 4.

.EXAMPLE
	.\Get-CsConnections.ps1 -IncludeUsers -IncludeSystem

	Description
	-----------
	Returns information on all connections on all frontend servers in the
	specified pool and in addition writes out all the users connection
	information. This includes any system	related connections.

.EXAMPLE
	.\Get-CsConnections.ps1 -PoolFqdn [pool FQDN] -ClientVersion [version number]

	Description
	-----------
	Returns all connection information for only clients that contain the specified client version.
	Also lists the users connecting with that version (same as -IncludeUsers option).

.EXAMPLE
	.\Get-CsConnections.ps1 -PoolFqdn [pool FQDN] -ShowFullClient

	Description
	-----------
	Returns extended information for clients including mobile device OS and type.

.INPUTS
	None. You cannot pipe objects to this script.
#>
#Requires -Version 2.0

[CmdletBinding(SupportsShouldProcess = $True)]
param(
	# Defines the front end pool to query for information. Cannot be used with -Server or -SIPAddress. If not specified and there is only a single front end pool, the script will automatically determine the FQDN.
	[parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	[string] $PoolFqdn,

	# Defines a specific server to query for information. Cannot be used with -PoolFqdn or -SIPAddress.
	[parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	[string] $Server,

	# Defines the SIP address for a specific user to query. Cannot be used with -PoolFqdn or -Server.
	[parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	[string] $SipAddress,

	# Defines the file path for the exported .csv file.
	[parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	[string] $FilePath,

	# Displays connected users and their connection counts. Users with a number of connections that meets or exceeds the -UserHighConnectionFlag count are colored in red. Cannot be used with -IncludeHighUsers.
	[parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	[switch] $IncludeUsers,

	# Displays connected users who meet or exceed the -UserHighConnectionFlag value. Users who are below the value are in white. Users who exceed the value are in red. Cannot be used with -IncludeUsers.
	[parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	[switch] $IncludeHighUsers,

	# Includes accounts used by the system (non-users).
	[parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	[switch] $IncludeSystem,

	# Defines the value at which users are considered to have a high number of connections. Defaults to 4. Cannot exceed the number configured for MaxEndpointsPerUser in the global RegistrarConfiguration.
	[parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	[ValidateRange(2,10)]
	[int] $UserHighConnectionFlag = 4,

	# Filter results to a specific version of the client. Helpful with -IncludeUsers to determine who's using a specific version of the client.
	[parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	[string] $ClientVersion,

	# Displays full client version information. Useful for viewing longer mobile device info.
	[parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	[switch] $ShowFullClient,

	# Queries Lync environment for total number of enabled users, and also determines percentage of total users that are currently connected.
	[parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	[switch] $ShowTotal,

	# Specifies that the server the script is connecting to is running Lync Server 2013. This parameter is not required when connecting to a pool, since the script will auto detect the version.
	[parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	[switch] $Is2013
)
#region functions

function Get-Data {
	[CmdletBinding(SupportsShouldProcess = $True)]
	param (
	 [parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	 [string] $SipAddress,

	 [parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	 [string] $Server,

	 [parameter(ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $true)]
	 [string] $ClientVersion
	)

	##############################################################################################
	# Went to using a named parameter for this function due to the
	# way Powershell does its thing with parameter passing, which
	# is NOT GOOD!  At any rate, need to call this function
	# as you would from a command line: Get-Data -sipAddr "value"
	# -server "value"
	#
	# Also, assuming a value of NULL for the SIP address of an
	# individual user, mostly to use this for finding overall
	# values, only occasionally to seek specific users.
	##############################################################################################

	if ($SipAddress) {
		[string] $whereClause = "where R.UserAtHost = '$SipAddress' "
	} else {
		if ($IncludeSystem){
			[string] $whereClause = $null
		}elseif ($ClientVersion){
			[string] $whereClause = "where upper(RE.ClientApp) like upper('%$ClientVersion%') and R.UserAtHost not like 'RtcApplication-%' "
		}else{
			[string] $whereClause = "where R.UserAtHost not like 'RtcApplication-%' "
		}
	}

	#Define SQL Connection String 
$PWD='Pa$$w0rd'
	[string] $connstring = "server=$server\rtclocal;database=rtcdyn;Trusted_Connection=true;"

	#Define SQL Command
	[object] $command = New-Object System.Data.SqlClient.SqlCommand

	if (($PoolFqdn -and (Get-CsService -PoolFqdn $PoolFqdn -Registrar).Version -eq 6) -or $Is2013){
		# SQL query for Lync Server 2013
		$command.CommandText = "Select (cast (RE.ClientApp as varchar (100))) as ClientVersion, R.UserAtHost as UserName, RA.Fqdn `
		From rtcdyn.dbo.RegistrarEndpoint RE `
		Inner Join rtcdyn.dbo.Endpoint EP on RE.EndpointId = EP.EndpointId `
		Inner Join rtc.dbo.Resource R on R.ResourceId = RE.OwnerId `
		Inner Join rtcdyn.dbo.Registrar RA on EP.RegistrarId = RA.RegistrarId `
		$whereClause `
		Order By ClientVersion, UserName"
	}else{
		# SQL query for Lync Server 2010
		$command.CommandText = "Select (cast (RE.ClientApp as varchar (100))) as ClientVersion, R.UserAtHost as UserName, FE.Fqdn `
		From rtcdyn.dbo.RegistrarEndpoint RE `
		Inner Join rtcdyn.dbo.Endpoint EP on RE.EndpointId = EP.EndpointId `
		Inner Join rtc.dbo.Resource R on R.ResourceId = RE.OwnerId `
		Inner Join rtcdyn.dbo.FrontEnd FE on EP.RegistrarId = FE.FrontEndId `
		$whereClause `
		Order By ClientVersion, UserName"
	}

	[object] $connection = New-Object System.Data.SqlClient.SqlConnection
	$connection.ConnectionString = $connstring
	$connection.Open()

	$command.Connection = $connection

	[object] $sqladapter = New-Object System.Data.SqlClient.SqlDataAdapter
	$sqladapter.SelectCommand = $command

	[object] $results = New-Object System.Data.Dataset
	$recordcount = $sqladapter.Fill($results)
	$connection.Close()
	return $Results.Tables[0]
} # end function Get-Data

function Set-ModuleStatus {
	[CmdletBinding(SupportsShouldProcess = $True)]
	param	(
		[parameter(ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true, Mandatory = $true, HelpMessage = "No module name specified!")]
		[ValidateNotNullOrEmpty()]
		[string] $name
	)
	PROCESS{
		# Executes once for each pipeline object
		# the $_ variable represents the current input object
		if (!(Get-Module -name "$name")) {
			if (Get-Module -ListAvailable | Where-Object {$_.Name -eq "$name"}) {
				Import-Module -Name "$name"
				# module was imported
				# return $true
			} else {
				# module was not available
				# return $false
			}
		} else {
			# Write-Output "$_ module already imported"
			# return $true
		}
	} # end PROCESS
} # end function Set-ModuleStatus

function Remove-ScriptVariables {
	[CmdletBinding(SupportsShouldProcess = $True)]
	param(
		[parameter(ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true, Mandatory = $true, HelpMessage = "No module name specified!")]
		[ValidateNotNullOrEmpty()]
		[string] $Path
	)

	$result = Get-Content $path |
	ForEach {
		if ( $_ -match '(\$.*?)\s*=') {
			$matches[1]  | ? { $_ -notlike '*.*' -and $_ -notmatch 'result' -and $_ -notmatch 'env:'}
		}
	}
	ForEach ($v in ($result | Sort-Object | Get-Unique)){
		# Write-Verbose "Removing" $v.replace("$","")
		Remove-Variable ($v.replace("$","")) -ErrorAction SilentlyContinue
	}
} # end function Remove-ScriptVariables

function Test-IsSigned {
<#
.SYNOPSIS

.DESCRIPTION

.NOTES
  Version							: 1.0 - See changelog at
	Wish list						: Better error trapping
  Rights Required			: Local administrator on server
  Sched Task Required	: No
  Lync Server Version	: N/A
  Author/Copyright		: © Pat Richard, Lync MVP - All Rights Reserved
  Email/Blog/Twitter	: pat@innervation.com 	http://www.ehloworld.com @patrichard
  Dedicated Blog Post	: http://www.ehloworld.com/1697
  Disclaimer   				: You running this script means you won't blame me if this breaks your stuff. This script is
  											provided AS IS without warranty of any kind. I disclaim all implied warranties including,
  											without limitation, any implied warranties of merchantability or of fitness for a particular
  											purpose. The entire risk arising out of the use or performance of the sample scripts and
  											documentation remains with you. In no event shall I be liable for any damages whatsoever
  											(including, without limitation, damages for loss of business profits, business interruption,
  											loss of business information, or other pecuniary loss) arising out of the use of or inability
  											to use the script or documentation.
  Assumptions					: ExecutionPolicy of AllSigned (recommended), RemoteSigned or Unrestricted (not recommended)
  Limitations					:
  Known issues				: None yet, but I'm sure you'll find some!
  Acknowledgements 		:

.LINK


.EXAMPLE
		PS C:\>

		Description
		-----------


.INPUTS
		None. You cannot pipe objects to this function.

.OUTPUTS
		Boolean output

#>
	[CmdletBinding(SupportsShouldProcess = $True)]
	param (
		[Parameter(ValueFromPipeline = $False, ValueFromPipelineByPropertyName = $True)]
		[ValidateNotNullOrEmpty()]
		[string] $FilePath = $MyInvocation.ScriptName
	)

	BEGIN{
		# Executes once before first item in pipeline is processed
		# the $_ variable represents the current input object
	} # end BEGIN

	PROCESS{
		# Executes once for each pipeline object
		# the $_ variable represents the current input object
		if ((Get-AuthenticodeSignature -FilePath $FilePath).Status -eq "Valid"){
			<#
			if ((Get-ExecutionPolicy) -ne "AllSigned"){
				Write-Warning "You could use an ExecutionPolicy of `"AllSigned`" when using this script. AllSigned is a higher level of security. For more information, see http://technet.microsoft.com/en-us/library/ee176961.aspx"
			}
			#>
		}
	} # end PROCESS

	END{
		# Executes once after last pipeline object is processed
		# the $_ variable represents the current input object
	} # end END
} # end function Test-IsSigned

function Test-ScriptUpdates {
<#
.SYNOPSIS
	Checks the CreationTime parameter on the script itself. If it's over 30 days, it will prompt the user & optionally take them to the changelog for that script.

.DESCRIPTION
	Checks the CreationTime parameter on the script itself. If it's over 30 days, it will prompt the user & optionally take them to the changelog for that script.

.NOTES
  Version							: 1.0 - See changelog at
	Wish list						: Better error trapping
  Rights Required			: Local administrator on server
  										: If script is not signed, ExecutionPolicy of RemoteSigned (recommended)
  											or Unrestricted (not recommended)
  										: If script is signed, ExecutionPolicy of AllSigned (recommended, RemoteSigned,
  											or Unrestricted (not recommended)
  Sched Task Required	: No
  Lync Server Version	: N/A
  Exchange Version    : N/A
  Author/Copyright		: © Pat Richard, Lync MVP - All Rights Reserved
  Email/Blog/Twitter	: pat@innervation.com 	http://www.ehloworld.com @patrichard
  Dedicated Blog Post	:
  Disclaimer   				: You running this script means you won't blame me if this breaks your stuff. This script is
  											provided AS IS without warranty of any kind. I disclaim all implied warranties including,
  											without limitation, any implied warranties of merchantability or of fitness for a particular
  											purpose. The entire risk arising out of the use or performance of the sample scripts and
  											documentation remains with you. In no event shall I be liable for any damages whatsoever
  											(including, without limitation, damages for loss of business profits, business interruption,
  											loss of business information, or other pecuniary loss) arising out of the use of or inability
  											to use the script or documentation.
  Assumptions					: ExecutionPolicy of AllSigned (recommended), RemoteSigned or Unrestricted (not recommended)
  Limitations					:
  Known issues				: None yet, but I'm sure you'll find some!
  Acknowledgements 		:

.LINK


.EXAMPLE
	PS C:\> Get-ScriptUpdates -ChangeLogURL 1234 -Age 45

	Description
	-----------
	Executes the function, specifying the article number the user will be taken to if they choose yes to go online, as well as number of days that must have elapsed.

.INPUTS
	None. You cannot pipe objects to this function.

.OUTPUTS
	Text output
#>

	[CmdletBinding(SupportsShouldProcess = $True)]
	param (
		# Specifies the article number the user will be taken to if they choose yes to go online.
		[Parameter(Position = 0, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $True)]
		[ValidateNotNullOrEmpty()]
		[int] $ChangelogUrl,

		# Specifies the number of days that must have elapsed before the prompt is displayed.
		[Parameter(Position = 1, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $True)]
		[ValidateNotNullOrEmpty()]
		[int] $Age = 90,

		[Parameter(Position = 2, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $True)]
		[ValidateNotNullOrEmpty()]
		[string] $FilePath = $MyInvocation.ScriptName
	)

	BEGIN{
		# Executes once before first item in pipeline is processed
		# the $_ variable represents the current input object
		[string]$Check4UpdatesPrompt = @"
┌──────────────────────────────────────────────────────────┐
│                Check For Updates?                        │
│            ==========================                    │
│    This script is more than $Age days old. Would you       │
│    like to check the script's website for a              │
│    newer version?                                        │
│                                                          │
└──────────────────────────────────────────────────────────┘
"@
	} # end BEGIN

	PROCESS{
		# Executes once for each pipeline object
		# the $_ variable represents the current input object
		# echo $_
		if (((Get-Date) - (Get-Item $filepath).CreationTime).TotalDays -gt $age){
			Write-Host $Check4UpdatesPrompt -ForegroundColor Green
			if ((Read-host "Go online? [y/n]") -imatch "y"){
				Start-Process "http://www.ehloworld.com/$ChangelogUrl"
			}
		}
	} # end PROCESS

	END{
		# Executes once after last pipeline object is processed
		# the $_ variable represents the current input object
	} # end END
} # end function Test-ScriptUpdates

#endregion functions

if (!(Test-IsSigned)){
	Test-ScriptUpdates -ChangelogUrl 681 -Age 90
}

Set-ModuleStatus -name Lync
if ($UserHighConnectionFlag){
	# We have to target the global policy or environments with more than one CsRegistrarConfiguration will barf
	$MaxEndPointsPerUser = (Get-CsRegistrarConfiguration -Identity Global).MaxEndPointsPerUser
	if ($UserHighConnectionFlag -gt $MaxEndPointsPerUser){
		Write-Host "MaxEndPointsPerUser in the global configuration is $MaxEndPointsPerUser. Please specify a number for UserHighConnectionFlag that does not exceed $MaxEndPointsPerUser. You specified $UserHighConnectionFlag." -ForegroundColor Red
		exit
	}
}
if ((! $server) -and (! $PoolFqdn) -and ((Get-CsService -Registrar | Measure-Object).count -eq 1)){
	Write-Verbose "Retrieving pool info"
	$PoolFqdn = (Get-CsService -Registrar).PoolFqdn
	Write-Verbose "Pool is now set to $PoolFqdn"
}
if ((! $server) -and (! $PoolFqdn)){
	$PoolFqdn = Read-Host "Enter front end pool FQDN"
	Write-Verbose "Pool is now set to $PoolFqdn"
}

#################################################################################################
########################################  Main Program  #########################################
#################################################################################################
# Here is where we pull all the front end server(s) from our topology for the designated
# pool and iterate through them to get current connections from all the servers.
#
# There are several possibilities here:
#	 1. Have collection of frontend servers
#	 2. Have a single frontend server or
#	 3. Have no servers
#  4. User specified a server instead of a pool

if ($PoolFqdn -and (! $server)){
	# set FQDN if $PoolFqdn was specified as just the netbios name
	if (!($PoolFqdn -match "\.")){
		Write-Verbose "No FQDN supplied. Building it. This may not work if the pool domain is different than this computer's domain"
		$PoolFqdn = $PoolFqdn+"."+([System.DirectoryServices.ActiveDirectory.Domain]::GetComputerDomain()).name
	}
	#Write-Host "`n`nChecking these pool servers in pool: " -ForegroundColor Cyan -NoNewLine
	#Write-Host $PoolFqdn
	$feServers = Get-CsComputer -Pool $PoolFqdn | Sort-Object identity
	ForEach ($feserver in $feservers){
		#Write-Host $($feserver.identity) -ForegroundColor Yellow
	}
}elseif ($server -and (! $PoolFqdn)){
	if (!($server -match "\.")){
		Write-Verbose "No FQDN supplied. Building it. This may not work if the server domain is different than this computer's domain"
		$server = $server+"."+([System.DirectoryServices.ActiveDirectory.Domain]::GetComputerDomain()).name
	}
	$feServers = Get-CsComputer -Identity $server -ErrorAction SilentlyContinue
	Write-Host "`n`nChecking this server: " -ForegroundColor Cyan -NoNewLine
	Write-Host $server
}

# next line added as recommended by Tristan Griffiths
$OverallRecords = @()

if ($feServers.count) {
	# Frontend pool collection, iterate through them
	for ($i = 0; $i -lt $feServers.count; $i++) {
		if ($SipAddress) {
			if ((!($SipAddress -match "@"))-and ((Get-CsSipDomain | Measure-Object).count -eq 1)){
				Write-Verbose "Incomplete SIP address supplied. Building it."
				$SipAddress = $SipAddress+"@"+(Get-CsSipDomain).identity
				Write-Verbose "SIP address corrected to $SipAddress"
			}
			Write-Verbose "Gathering info from $($feServers[$i].identity)"
			$data = Get-Data -SipAddress $SipAddress -Server $feServers[$i].identity -ClientVersion $ClientVersion
		} else {
			Write-Verbose "Gathering info from $($feServers[$i].identity)"
			$data = Get-Data -Server $feServers[$i].identity -ClientVersion $ClientVersion
		}

		# Since an individual user's connections are all homed on one server,
		# we won't have data coming back from all front-end servers in the
		# case of searching for a single user
		if ($data) {
			$OverallRecords = $OverallRecords + $data
		}
	}
} elseif ($feServers) {
	# Have a standalone server or a FE pool of only one server
	if ($SipAddress) {
		if ((!($SipAddress -match "@"))-and ((Get-CsSipDomain | Measure-Object).count -eq 1)){
			Write-Verbose "Incomplete SIP address supplied. Building it."
			$SipAddress = $SipAddress+"@"+(Get-CsSipDomain).identity
			Write-Verbose "SIP address corrected to $SipAddress"
		}
		$data = Get-Data -SipAddress $SipAddress -Server $feServers.identity -ClientVersion $ClientVersion
	} else {
		Write-Verbose "Gathering info from $($feServers.identity)"
		$data = Get-Data -Server $feServers.identity -ClientVersion $ClientVersion
	}

	# Make sure we have data to work with...
	if ($data) {
		$OverallRecords = $data
	}
}else{
	Write-Host "No servers returned!" -ForegroundColor Red
}

# Check to see if we have any data to act on
if (! $OverallRecords) {
	Write-Host "`r`nNothing returned from query!`r`n" -ForegroundColor Yellow

	# Nothing else to do
	exit
} else {
	$count = 0
	$userHash = @{}
	$clientHash = @{}
	$serverHash = @{}
	$UserList = @{}

	$OverallRecords | ForEach-object {
		# Each record has three components: Connected Client Version, User's SIP
		# address and the frontend server's FQDN. Here, we'll build a hash
		# for each of these components for each record.

		# Build hash of users

 		$UserList = $_.UserName

		if (! $userHash.ContainsKey($_.UserName)) {
			$userHash.add($_.UserName, 1)
		} else {
			$userHash.set_item($_.UserName, ($userHash.get_item($_.UserName) + 1))
		}

		# Build hash of servers
		if (! $serverHash.ContainsKey($_.fqdn)) {
			$serverHash.add($_.fqdn, 1)
		} else {
			$serverHash.set_item($_.fqdn, ($serverHash.get_item($_.fqdn) + 1))
		}

		# Build hash of clients
		# Lets get rid of the extraneous verbage from the client version names, if applicable
		# This merely gives a friendlier output, and helps prevent wordwrap
		if ($_.ClientVersion.contains('(') -and (! $ShowFullClient)) {
			# Get rid of extraneous verbage
			$clientName = $_.ClientVersion.substring(0, $_.ClientVersion.IndexOf('('))
		} else {
			# Have a client name with no extraneous verbage or $ShowFullClient switch specified
			$clientName = $_.ClientVersion
			$clientName = $clientName.replace("Microsoft ","")
			$clientName = $clientName.replace("Office ","")
			$clientName = $clientName.replace("AndroidLync","Android")
			$clientName = $clientName.replace("iPadLync","iPad")
			$clientName = $clientName.replace("iPhoneLync","iPhone")
			$clientName = $clientName.replace("WPLync","WP")
		}

		if (! $clientHash.ContainsKey($clientName)) {
			$clientHash.add($clientName, 1)
		} else {
			$clientHash.set_item($ClientName, ($clientHash.get_item($ClientName) + 1))
		}
		$count++
	}
}

#################################################################################################
####################################  Output Query Results  #####################################
#################################################################################################
# If output to file is chosen, then write out the results and a note to that effect
# then exit

if ($FilePath) {
	$OverallRecords | Export-Csv $FilePath
	Write-Host -ForegroundColor green "`r`nQuery Results written to $FilePath`r`n"
	exit
}

$arrObject = @()
$objlist = @{}


#region ClientVersions
if (! $ShowFullClient){
	#Write-Host ("`r`n`r`n{0, -26}{1, -41}{2, 11}" -f "Client Version", "Agent", "Connections") -ForegroundColor Cyan
}else{
	#Write-Host ("`r`n`r`n{0, -64}{1, 14}" -f "Agent", "Connections") -ForegroundColor cyan
}
#Write-Host "------------------------------------------------------------------------------" -ForegroundColor Cyan

ForEach ($key in $clientHash.keys | Sort-Object -Descending) {
	# Break down client version into its two component parts and print
	# them out along with their respective counts in a nice format
	$index = $key.indexof(" ")

	if ($index -eq "-1") {
		# No second part
		$first = $key
		$second = " "
	} else {
		# Client version/agent has two main parts
		$first = $key.substring(0, $index)
		$second = $key.substring($index + 1)
	}

	$value = $clientHash.$key
	if (! $ShowFullClient){
	#	"{0,-26}{1,-45}{2,7}" -f $($first.trim()), $($second.trim()), $value
	}else{
	#	"{0,-73}{1,4}" -f $($second.trim()), $value
	}
	$currObject = New-Object PSObject -Property @{
		ClientVersion=$first.trim()
		Agent=$second.trim()
		Connections=$value
		Section=1

		}
	$objlist.Add($currObject,1)
	$arrObject += $currobject
	
	
}
 #Write-Host "------------------------------------------------------------------------------" -ForegroundColor Cyan
# "{0,-41}{1,37}" -f "Client Versions Connected", $clientHash.count
#endregion ClientVersions


#region FrontEnds
# Frontend Server, Connections
#Write-Host ("`r`n`r`n{0,-41}{1,15}" -f "Front End Servers", "Connections") -ForegroundColor Cyan
#Write-Host "--------------------------------------------------------" -ForegroundColor Cyan
ForEach ($key in ($serverHash.keys | Sort-Object)) {
	$value = $serverHash.$key
	[string]$Percent = "("+"{0:P2}" -f ($value/$count)+")"
	#"{0,-40}{1,6} {2,9}" -f $($key.ToLower()), $value, $Percent
	
	$currObject = New-Object PSObject -Property @{
			FrontEndServers=$key.ToLower()
			Connections=$value
			ConnectionsPercent=$Percent
			Section=2
			}
	$objlist.add($currObject,2)
	$arrObject += $currobject
	
}
#Write-Host "--------------------------------------------------------" -ForegroundColor Cyan
#"{0,-40}{1,6}" -f "Total connections", $count

$currObject = New-Object PSObject -Property @{
			TotalConnections=$count
			Section=2
			}
$objlist.add($currObject,2)
$arrObject += $currobject

#endregion FrontEnds

#region UniqueUsers
# Unique Users, Unique Clients
$UniqueUsers = $userHash.count
#Write-Host ("`r`n`r`n{0,-41}{1,15}" -f "Total Unique Users/Clients", "Total") -ForegroundColor Cyan
#Write-Host "--------------------------------------------------------" -ForegroundColor Cyan
#"{0,-41}{1,15}" -f "Users Connected", $userHash.count
#endregion UniqueUsers

#region ShowTotal
if ($ShowTotal){
	#Write-Host "Calculating data..." -ForegroundColor Yellow -NoNewline
	[int]$TotalUsers = (Get-CsUser | Measure-Object).count
	[int]$TotalEVUsers = (Get-CsUser -filter {EnterpriseVoiceEnabled -eq $True} | Measure-Object).Count
	[string]$TotalPercent = "{0:P2}" -f ($UniqueUsers/$TotalUsers)
	#Write-Host "`b`b`b`b`b`b`b`b`b`b`b`b`b`b`b`b`b`b`b" -NoNewline
	#"{0,-41}{1,15}" -f "Lync Enabled Users (Entire organization)", $TotalUsers
	#"{0,-41}{1,15}" -f "Voice Enabled Users (Entire organization)", $TotalEVUsers
	#"{0,-41}{1,15}" -f "Percentage of Enabled Users Connected", $TotalPercent
	
	$currObject = New-Object PSObject -Property @{
			UsersConnected=$userHash.count
			LyncEnabledUsers=$TotalUsers
			VoiceEnabledUsers=$TotalEVUsers
			PercentOfEnabledUsersConnected=$TotalPercent
			ClientVersionsConnected=$clientHash.count
			Section=3
	}
	$objlist.add($currObject,3)
	$arrObject += $currobject
}
#"{0,-41}{1,15}" -f "Client Versions Connected", $clientHash.count


#Write-Host "--------------------------------------------------------" -ForegroundColor Cyan
#endregion ShowTotal

#region IncludeUsers
# Users, Connections
if ($IncludeUsers){
	#Write-Host ("`r`n`r`n{0,-45}{1,-11}" -f "Connected Users", "Connections") -ForegroundColor Cyan
	#Write-Host "--------------------------------------------------------" -ForegroundColor Cyan
	ForEach ($key in $userHash.keys | Sort-Object) {
		$value = $userHash.$key
		if ($value -ge $UserHighConnectionFlag){
 			#Write-Host ("{0,-45}{1,11}" -f $key, $value) -ForegroundColor Red
		}else{
			#"{0,-45}{1,11}" -f $key, $value
		}
		
		$currObject = New-Object PSObject -Property @{
				ConnectedUsers=$key
				Connections=$value
				Section=4
		}
		$objlist.add($currObject,4)
		$arrObject += $currobject
		#Write-Output $currobject

	}
	#Write-Host "--------------------------------------------------------`r`n" -ForegroundColor Cyan
}
#endregion IncludeUsers

#region HighUsers
# Users, Connections
if ($IncludeHighUsers){
	Write-Host ("`r`n`r`n{0,-45}{1,-11}" -f "Connected Users", "Connections") -ForegroundColor Cyan
	Write-Host "--------------------------------------------------------" -ForegroundColor Cyan
	ForEach ($key in $userHash.keys | Sort-Object) {
		$value = $userHash.$key
		if ($value -eq $UserHighConnectionFlag){
 			"{0,-45}{1,11}" -f $key, $value
		}elseif ($value -gt $UserHighConnectionFlag){
			Write-Host ("{0,-45}{1,11}" -f $key, $value) -ForegroundColor Red
		}
	}
	Write-Host "--------------------------------------------------------`r`n" -ForegroundColor Cyan
}
#endregion HighUsers

#region SipAddress
if ($SipAddress){
	Write-Host "`r`n`r`n"
	# we have to get creative here as the output from Get-CsUserPoolInfo changed from Lync 2010 to 2013
	if (((Get-Module Lync).Version).Major -eq 4){
		# Lync Server 2010
		$UserPreferredInfo = Get-CsUserPoolInfo -Identity sip:$SipAddress | Select-Object -ExpandProperty PrimaryPoolMachinesInPreferredOrder | Select-Object MachineId,Fqdn
		Write-Host ("{0,-40}" -f "Preferred Connection Order For $SipAddress") -ForegroundColor Cyan
		Write-Host "--------------------------------------------------------" -ForegroundColor Cyan
		ForEach ($server in $UserPreferredInfo){
			$machineid = $server.machineid
			$fqdn = $server.fqdn
			"{0,-40}" -f $fqdn
		}
	}elseif (((Get-Module Lync).Version).Major -eq 5){
		# Lync Server 2013
		$UserPreferredInfo = Get-CsUserPoolInfo -Identity sip:$SipAddress | Select-Object -ExpandProperty PrimaryPoolMachinesInPreferredOrder
		Write-Host ("{0,-40}" -f "Preferred Connection Order For $SipAddress") -ForegroundColor Cyan
		Write-Host "--------------------------------------------------------" -ForegroundColor Cyan
		ForEach ($server in $UserPreferredInfo){
			"{0,-40}" -f $server
		}
	}
	Write-Host "--------------------------------------------------------`n" -ForegroundColor Cyan
}
#region SipAddress




ForEach($ob in $arrObject)
{	
	Write-Output $ob
}






Write-Verbose "Query complete"

Remove-ScriptVariables($MyInvocation.MyCommand.Definition)

# SIG # Begin signature block
# MIIQBQYJKoZIhvcNAQcCoIIP9jCCD/ICAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB
# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR
# AgEAAgEAAgEAAgEAAgEAMCEwCQYFKw4DAhoFAAQU4KVyShtgn1K8Z3up6vjjaP38
# 7mWggg1KMIIGnzCCBYegAwIBAgIQCSu3nvOdPAJrDHL+XciEEzANBgkqhkiG9w0B
# AQUFADBvMQswCQYDVQQGEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYD
# VQQLExB3d3cuZGlnaWNlcnQuY29tMS4wLAYDVQQDEyVEaWdpQ2VydCBBc3N1cmVk
# IElEIENvZGUgU2lnbmluZyBDQS0xMB4XDTEzMDgyODAwMDAwMFoXDTE0MDkwNDEy
# MDAwMFowazELMAkGA1UEBhMCVVMxCzAJBgNVBAgTAk1JMRkwFwYDVQQHExBTdGVy
# bGluZyBIZWlnaHRzMRkwFwYDVQQKExBJbm5lcnZhdGlvbiwgTExDMRkwFwYDVQQD
# ExBJbm5lcnZhdGlvbiwgTExDMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKC
# AQEA8dXC+TSUZsMisaialKJLat7AnmOF0HE+U49duaBJp/N4IXhjUZM9sXRkR/X0
# KAbNauNpNrs3TLKNpO7/RUinzrzsg8IhtBVXSrK7c0HDqJ7vG181it2qQO1yB9uD
# UV8frjprnNcR1w0qW9wFqclKlkyOxgiCANVoT6qXO9IxPZT28h+tMfBCsU7sZALs
# 3kPRMCQ1ZywngVSqEQ9rBYV3jW4EgvPEOmuhtnwgVB1Alr2V1dVAtbNxw1A7fQ8w
# J6EQSNAPVJQq7304QKKgGaTXtA2TwAjO5+m9ZltD/s0wXWYs0V1r8pOJOo1r17l4
# z91OWLPP+yfxvlnm5F1IXZxCbQIDAQABo4IDOTCCAzUwHwYDVR0jBBgwFoAUe2jO
# KarAF75JeuHlP9an90WPNTIwHQYDVR0OBBYEFB9N9j4QNDPcSscWMAVDKCOcXS/G
# MA4GA1UdDwEB/wQEAwIHgDATBgNVHSUEDDAKBggrBgEFBQcDAzBzBgNVHR8EbDBq
# MDOgMaAvhi1odHRwOi8vY3JsMy5kaWdpY2VydC5jb20vYXNzdXJlZC1jcy0yMDEx
# YS5jcmwwM6AxoC+GLWh0dHA6Ly9jcmw0LmRpZ2ljZXJ0LmNvbS9hc3N1cmVkLWNz
# LTIwMTFhLmNybDCCAcQGA1UdIASCAbswggG3MIIBswYJYIZIAYb9bAMBMIIBpDA6
# BggrBgEFBQcCARYuaHR0cDovL3d3dy5kaWdpY2VydC5jb20vc3NsLWNwcy1yZXBv
# c2l0b3J5Lmh0bTCCAWQGCCsGAQUFBwICMIIBVh6CAVIAQQBuAHkAIAB1AHMAZQAg
# AG8AZgAgAHQAaABpAHMAIABDAGUAcgB0AGkAZgBpAGMAYQB0AGUAIABjAG8AbgBz
# AHQAaQB0AHUAdABlAHMAIABhAGMAYwBlAHAAdABhAG4AYwBlACAAbwBmACAAdABo
# AGUAIABEAGkAZwBpAEMAZQByAHQAIABDAFAALwBDAFAAUwAgAGEAbgBkACAAdABo
# AGUAIABSAGUAbAB5AGkAbgBnACAAUABhAHIAdAB5ACAAQQBnAHIAZQBlAG0AZQBu
# AHQAIAB3AGgAaQBjAGgAIABsAGkAbQBpAHQAIABsAGkAYQBiAGkAbABpAHQAeQAg
# AGEAbgBkACAAYQByAGUAIABpAG4AYwBvAHIAcABvAHIAYQB0AGUAZAAgAGgAZQBy
# AGUAaQBuACAAYgB5ACAAcgBlAGYAZQByAGUAbgBjAGUALjCBggYIKwYBBQUHAQEE
# djB0MCQGCCsGAQUFBzABhhhodHRwOi8vb2NzcC5kaWdpY2VydC5jb20wTAYIKwYB
# BQUHMAKGQGh0dHA6Ly9jYWNlcnRzLmRpZ2ljZXJ0LmNvbS9EaWdpQ2VydEFzc3Vy
# ZWRJRENvZGVTaWduaW5nQ0EtMS5jcnQwDAYDVR0TAQH/BAIwADANBgkqhkiG9w0B
# AQUFAAOCAQEABTcbxhPxSJP7AKrF23wXbgelEGzjpewmm0BnHO9/V+9K4UE/3i5Z
# e3bX7OvUb/55zCHT4pvZl8/i8tuvFnrWZZcjGljT3W9SIVOuKNdlItXC8dvP67Hz
# OMZvSbdI5MU6sjd3vyYKznTMHG1Mz/x+BMh6rEnGsIaY4r8r4fkNfJTyTkww8AWh
# I16Pno3dWgKhfploPODtMxczJKA15VDFhQqM3oHHMojasXo5uGb5jlGiDFkZ3mDa
# /+gLUsJUjAzF27ZIy/jRgp0eMkqlgdxGa+gZUK73Ue6aTP2LW9SL/1fLYl8QkwRs
# MWG1faGdureT3Oh0EHH6UhREB/SxLJ+xLTCCBqMwggWLoAMCAQICEA+oSQYV1wCg
# viF2/cXsbb0wDQYJKoZIhvcNAQEFBQAwZTELMAkGA1UEBhMCVVMxFTATBgNVBAoT
# DERpZ2lDZXJ0IEluYzEZMBcGA1UECxMQd3d3LmRpZ2ljZXJ0LmNvbTEkMCIGA1UE
# AxMbRGlnaUNlcnQgQXNzdXJlZCBJRCBSb290IENBMB4XDTExMDIxMTEyMDAwMFoX
# DTI2MDIxMDEyMDAwMFowbzELMAkGA1UEBhMCVVMxFTATBgNVBAoTDERpZ2lDZXJ0
# IEluYzEZMBcGA1UECxMQd3d3LmRpZ2ljZXJ0LmNvbTEuMCwGA1UEAxMlRGlnaUNl
# cnQgQXNzdXJlZCBJRCBDb2RlIFNpZ25pbmcgQ0EtMTCCASIwDQYJKoZIhvcNAQEB
# BQADggEPADCCAQoCggEBAJx8+aCPCsqJS1OaPOwZIn8My/dIRNA/Im6aT/rO38bT
# JJH/qFKT53L48UaGlMWrF/R4f8t6vpAmHHxTL+WD57tqBSjMoBcRSxgg87e98tzL
# uIZARR9P+TmY0zvrb2mkXAEusWbpprjcBt6ujWL+RCeCqQPD/uYmC5NJceU4bU7+
# gFxnd7XVb2ZklGu7iElo2NH0fiHB5sUeyeCWuAmV+UuerswxvWpaQqfEBUd9YCvZ
# oV29+1aT7xv8cvnfPjL93SosMkbaXmO80LjLTBA1/FBfrENEfP6ERFC0jCo9dAz0
# eotyS+BWtRO2Y+k/Tkkj5wYW8CWrAfgoQebH1GQ7XasCAwEAAaOCA0MwggM/MA4G
# A1UdDwEB/wQEAwIBhjATBgNVHSUEDDAKBggrBgEFBQcDAzCCAcMGA1UdIASCAbow
# ggG2MIIBsgYIYIZIAYb9bAMwggGkMDoGCCsGAQUFBwIBFi5odHRwOi8vd3d3LmRp
# Z2ljZXJ0LmNvbS9zc2wtY3BzLXJlcG9zaXRvcnkuaHRtMIIBZAYIKwYBBQUHAgIw
# ggFWHoIBUgBBAG4AeQAgAHUAcwBlACAAbwBmACAAdABoAGkAcwAgAEMAZQByAHQA
# aQBmAGkAYwBhAHQAZQAgAGMAbwBuAHMAdABpAHQAdQB0AGUAcwAgAGEAYwBjAGUA
# cAB0AGEAbgBjAGUAIABvAGYAIAB0AGgAZQAgAEQAaQBnAGkAQwBlAHIAdAAgAEMA
# UAAvAEMAUABTACAAYQBuAGQAIAB0AGgAZQAgAFIAZQBsAHkAaQBuAGcAIABQAGEA
# cgB0AHkAIABBAGcAcgBlAGUAbQBlAG4AdAAgAHcAaABpAGMAaAAgAGwAaQBtAGkA
# dAAgAGwAaQBhAGIAaQBsAGkAdAB5ACAAYQBuAGQAIABhAHIAZQAgAGkAbgBjAG8A
# cgBwAG8AcgBhAHQAZQBkACAAaABlAHIAZQBpAG4AIABiAHkAIAByAGUAZgBlAHIA
# ZQBuAGMAZQAuMBIGA1UdEwEB/wQIMAYBAf8CAQAweQYIKwYBBQUHAQEEbTBrMCQG
# CCsGAQUFBzABhhhodHRwOi8vb2NzcC5kaWdpY2VydC5jb20wQwYIKwYBBQUHMAKG
# N2h0dHA6Ly9jYWNlcnRzLmRpZ2ljZXJ0LmNvbS9EaWdpQ2VydEFzc3VyZWRJRFJv
# b3RDQS5jcnQwgYEGA1UdHwR6MHgwOqA4oDaGNGh0dHA6Ly9jcmwzLmRpZ2ljZXJ0
# LmNvbS9EaWdpQ2VydEFzc3VyZWRJRFJvb3RDQS5jcmwwOqA4oDaGNGh0dHA6Ly9j
# cmw0LmRpZ2ljZXJ0LmNvbS9EaWdpQ2VydEFzc3VyZWRJRFJvb3RDQS5jcmwwHQYD
# VR0OBBYEFHtozimqwBe+SXrh5T/Wp/dFjzUyMB8GA1UdIwQYMBaAFEXroq/0ksuC
# MS1Ri6enIZ3zbcgPMA0GCSqGSIb3DQEBBQUAA4IBAQB7ch1k/4jIOsG36eepxIe7
# 25SS15BZM/orh96oW4AlPxOPm4MbfEPE5ozfOT7DFeyw2jshJXskwXJduEeRgRNG
# +pw/alE43rQly/Cr38UoAVR5EEYk0TgPJqFhkE26vSjmP/HEqpv22jVTT8nyPdNs
# 3CPtqqBNZwnzOoA9PPs2TJDndqTd8jq/VjUvokxl6ODU2tHHyJFqLSNPNzsZlBjU
# 1ZwQPNWxHBn/j8hrm574rpyZlnjRzZxRFVtCJnJajQpKI5JA6IbeIsKTOtSbaKbf
# KX8GuTwOvZ/EhpyCR0JxMoYJmXIJeUudcWn1Qf9/OXdk8YSNvosesn1oo6WQsQz/
# MYICJTCCAiECAQEwgYMwbzELMAkGA1UEBhMCVVMxFTATBgNVBAoTDERpZ2lDZXJ0
# IEluYzEZMBcGA1UECxMQd3d3LmRpZ2ljZXJ0LmNvbTEuMCwGA1UEAxMlRGlnaUNl
# cnQgQXNzdXJlZCBJRCBDb2RlIFNpZ25pbmcgQ0EtMQIQCSu3nvOdPAJrDHL+XciE
# EzAJBgUrDgMCGgUAoHgwGAYKKwYBBAGCNwIBDDEKMAigAoAAoQKAADAZBgkqhkiG
# 9w0BCQMxDAYKKwYBBAGCNwIBBDAcBgorBgEEAYI3AgELMQ4wDAYKKwYBBAGCNwIB
# FTAjBgkqhkiG9w0BCQQxFgQUn4o2Z9JE1d7GAPy0vvL5LJ2anK0wDQYJKoZIhvcN
# AQEBBQAEggEAVisPYI+73iDqpyPT0XJzSjJsJQJTNZQ+NglASL4enFToVdPZpNqY
# WhZqsuAaJ5DYBHAtMQYiXDtXBF8iFC1iw0YMVkjDujtq54Ga2zt94CiOcJjJJS0r
# 2htNguhkAzsEC8AG8fH+ch5oxMOA7ehTWR3LvaYcfGPDvd+qM3/AF1IPcfWZlkiE
# eS3P3rFNuqOljPBIM9tLFwqpiQp/66c2r8nqR3sxkXrx6wJjvYp88sNyDK8w1L/z
# DZj4xgf4YqBOtOpCVbHPMprgJ6UvbjlfeUCBn2Ht4i4CcLLq16ZSktoiI1UsU3Ea
# fW/8MvSjfzu/aDN7rtLU72pPmpzqcdf3hw==
# SIG # End signature block
