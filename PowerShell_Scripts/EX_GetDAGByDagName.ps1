param(
    [Parameter(ParameterSetName='dagname')] [string]$dagname
)

Get-DatabaseAvailabilityGroup $dagname