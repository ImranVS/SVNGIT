<#
.DESCRIPTION
Gets a list of all the workflows in the envirement
#>
get-spsite | % { $_.AllWebs | % { $_.Lists | % { $_.WorkFlowAssociations | ? {$_.Name -notlike "*Previous Version*" }}}}