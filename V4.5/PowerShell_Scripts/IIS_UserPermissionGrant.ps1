$directory = "C:\inetpub\wwwroot\VSWeb"
$inherit = [system.security.accesscontrol.InheritanceFlags]"ContainerInherit, ObjectInherit"
$propagation = [system.security.accesscontrol.PropagationFlags]"None"
$acl = Get-Acl $directory
$accessrule = New-Object system.security.AccessControl.FileSystemAccessRule("IIS_IUSRS", "fullcontrol", $inherit, $propagation, "Allow")
$acl.AddAccessRule($accessrule)
set-acl -aclobject $acl $directory