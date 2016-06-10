
$PolicyName = "VitalSignsPolicy"
$Session = (Get-PSSession)[0]
$UserName = (Get-PSSession)[0].Runspace.ConnectionInfo.Credential.UserName

Invoke-Command -Session $Session -ScriptBlock {

New-ThrottlingPolicy -Identity {$PolicyName} -PowershellMaxConcurrency 1000 -PswsMaxConcurrency 1000 -ThrottlingPolicyScope Regular 
Set-ThrottlingPolicyAssociation -Identity {$UserName} -ThrottlingPolicy $PolicyName

}

