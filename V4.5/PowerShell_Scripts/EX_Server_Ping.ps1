#D:\Mukund\E\RPRWyatt\VSTrunk\PowerShell_POC\PSPOC1\bin\Debug\Scripts\EX_Server_Ping.ps1 -server win-kurrr2qsdj0.jnittech.com
param(
	[Parameter(ParameterSetName='server')] [string]$server
)

Test-Connection $server -Quiet -ErrorAction Stop