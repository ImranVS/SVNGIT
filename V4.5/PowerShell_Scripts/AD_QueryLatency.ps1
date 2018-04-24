#$DCs = Get-ADDomainController -filter * | where {$_.Hostname -eq  "jnittech-ad.jnittech.com"}
$objArr = @()
foreach ($DC in $DCs) {

   
   $numberoftests = 20
   $totalmeasurement = 0

      $i = 0

      while ($i -ne $numberoftests)

      {
    $mc = (Measure-Command -Expression {Get-ADObject -LDAPFilter "(description=*test*)" -Server $($DC.Name)}).TotalSeconds
      $totalmeasurement += $mc

            $i += 1

      }
      $totalmeasurement = $totalmeasurement / $numberoftests
        
 
$objArr+=New-Object PSObject -Property @{
				seconds=$mc
				Name=$($DC.Name)
				Forest=$($DC.Forest)
}

		
      #write-host "Search took $totalmeasurement seconds on $($DC.Name) in AD Site $($DC.Forest)"
      
} # end foreach

$objArr