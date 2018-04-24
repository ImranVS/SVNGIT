
$VM = "VM-PS-DEPLOY"
$VM_IP="192.168.1.221"
$VM_GW="192.168.1.1"
$VM_MASK="255.255.255.0"
$VM_DNS="192.168.1.25","172.30.102.50","172.16.100.21"
$VMuname= 'VSINSTALL';
            $VMPwd='P@ssworD';
            $VMsecpasswd = ConvertTo-SecureString $VMPwd -AsPlainText –Force;
            $VMcredentials=New-object -typename System.Management.Automation.PSCredential -argumentlist $VMuname,$VMsecpasswd;

$vmhost = "esx-rpr2.jnittech.com"
$folderName="USDOP_VITALSIGNS"
$name=$VM
$datastore="USDOPDS01"
$template="TMPL-VS-ALLFE"            

#Importing PowerCLi SnapIn
$vmsnapin = Get-PSSnapin VMware.VimAutomation.Core -ErrorAction SilentlyContinue
				$Error.Clear()
				if ($vmsnapin -eq $null){
					Add-PSSnapin VMware.VimAutomation.Core 
					if ($error.Count -ne 0){
						(Get-Date -Format ("[yyyy-MM-dd HH:mm:ss] ")) + "Error: Loading PowerCLI" | out-file $log -Append
						exit
					}
				}
$uname= 'administrator@vsphere.local';
            $Pwd='NjJn1t!@345';
            $secpasswd = ConvertTo-SecureString $Pwd -AsPlainText –Force;
            $credentials=New-object -typename System.Management.Automation.PSCredential -argumentlist $uname,$secpasswd;
			Connect-VIServer -Server ussvcvmwvc01.jnittech.com -Credential $credentials -ErrorAction SilentlyContinue | Out-Null

$vmObj =foreach($VirtualM in $VM){get-VM $VirtualM} 
 
foreach($active in $vmObj){
if($active.PowerState -eq "PoweredOn"){
Stop-VM -VM $active -Confirm:$false  | Out-Null} 
}
Start-Sleep -Seconds 7
 
foreach($delete in $vmObj){
Remove-VM -VM $delete -DeleteFromDisk -Confirm:$false  | Out-Null}


# location
$folder = Get-Folder -Name $folderName

$now = Get-Date

Write-Host "Starting deploy script: $now"



$now = Get-Date
Write-Host "Starting deployment process: $now"

if (!$datastore) {
	Write-Host "No datastore could be found. VM cannot be deployed! Aborting."
	return
}



Write-Host "Cloning VM ..."
try {
$vm = New-VM -Name $name `
	-VMHost $vmhost `
	-Template $template `
	-Datastore $datastore `
	-Location $folder `
	-ErrorAction:Stop
}
catch
{
        Write-Host "An error ocurred during template clone operation:"
 
        # output all exception information
        $_ | fl
 
        Write-Host "Cleaning up ..."
 
        # clean up and exit
        $exists = Get-VM -Name $name -ErrorAction SilentlyContinue
        If ($Exists){
                Remove-VM -VM $exists -DeletePermanently
        }
        return
}
Start-VM -VM $VM -Confirm:$false

sleep 240


$Computername =Get-VM -name $VM
$ComputerIp= $Computername.guest.IPAddress|where {([IPAddress]$_).AddressFamily -eq [System.Net.Sockets.AddressFamily]::InterNetwork}

#NOW to Change STATIC IP of VM
#THE ACTUALL STATIC PROCESS

$scriptblock1={
param($VM,$VM_IP,$VM_MASK,$VM_GW,$VM_DNS)
$gethostname=Get-WMIObject Win32_ComputerSystem;
write-host This is the Current Host Name:
echo $gethostname
write-host This is the NEW Hostname that VM will Change to
echo $VM.name
write-host Changing Hostname of VM
$gethostname.Rename($VM.Name);
Write-Host Changing Network Adapter from DHCP To Static with Provided IP
$NICs=Get-WMIObject Win32_NetworkAdapterConfiguration -computername .| where{$_.IPEnabled -eq $true -and $_.DHCPEnabled -eq $true} 
Foreach($NIC in $NICs) { 

$NIC.EnableStatic($VM_IP, $VM_MASK); 
$NIC.SetGateways($VM_GW); 
$NIC.SetDNSServerSearchOrder($VM_DNS); 
$NIC.SetDynamicDNSRegistration(“FALSE”);
    } 
Write-host Hostname Change Requires Restart of VM, Restarting NOW!
restart-computer
}
  
Invoke-Command -ComputerName $ComputerIp -ScriptBlock $scriptBlock1 -Credential $VMcredentials -ArgumentList $VM,$VM_IP,$VM_MASK,$VM_GW,$VM_DNS -InDisconnectedSession
