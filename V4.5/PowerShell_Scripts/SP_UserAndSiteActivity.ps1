#Invoke-Command -Session $s -ScriptBlock {

    Import-Module WebAdministration
    
    Get-WebConfigurationProperty "/system.applicationHost/sites/siteDefaults" -name logfile.directory| ForEach-Object {
    
            $firstTime = $true
    
            $date = Get-Date
            $endDate = [DateTime]$(($date.Ticks) - $($date.Ticks % $(New-TimeSpan -Hours 1).ticks))
            $startDate = [DateTime]$($endDate.Ticks - $(New-TimeSpan -days 70).Ticks) 
    
            Get-ChildItem $([System.Environment]::ExpandEnvironmentVariables($_.Value)) -Recurse |  ? {$_.Directory -ne $null -and $_.LastWriteTime -gt $startDate} | ForEach-Object {
    
                # Location of IIS LogFile
                $File = $_.Directory.ToString() + "\" + $_.Name
    
    
                # Get-Content gets the file, pipe to Where-Object and skip the first 3 lines.
                $Log = Get-Content $File | where {$_ -notLike "#[D,S-V]*" -and $_.ToString().Trim() -ne ''} 
    
    
                if($firstTime)
                {
                    # Replace unwanted text in the line containing the columns.
                    $Columns = (($Log[0].TrimEnd()) -replace "#Fields: ", "" -replace "-","" -replace "\(","" -replace "\)","").Split(" ")
                    # Count available Columns, used later
                    $Count = $Columns.Length
    
                    # Create an instance of a System.Data.DataTable
                    $IISLog = New-Object System.Data.DataTable "IISLog"

                    # Loop through each Column, create a new column through Data.DataColumn and add it to the DataTable
                    foreach ($Column in $Columns) {
                       $NewColumn = New-Object System.Data.DataColumn $Column, ([string])
                       $IISLog.Columns.Add($NewColumn)
                    }
                    $firstTime = $false
                }
    
    
                # Get all Rows that i Want to retrieve, if i wanted rows containing POST instead, replace Microsoft-Server-ActiveSync with POST
                $Rows = $Log | where {$_ -like "*GET*"}
    
                # Loop Through each Row and add the Rows.
                foreach ($Row in $Rows) {
                   $Row = $Row.Split(" ")
                   $AddRow = $IISLog.newrow()
                   for($i=0;$i -lt $Count; $i++) {
                     $ColumnName = $Columns[$i]
                     $AddRow.$ColumnName = $Row[$i]
                   }
                   $IISLog.Rows.Add($AddRow)
                 }
                 
            }
    
            # Now I've got the IISLog entries containing Microsoft-Server-ActiveSync stored in the $IISLog Variable
            # Call Variable to display content.
    
            $ignoredTypes = 'ico', 'js', 'axd', 'png', 'css', 'gif', 'ashx', 'jpg'
    
            [regex] $a_regex = ‘(?i)‘ + (($ignoredTypes |foreach {".*\.$_"}) –join “|”) + ‘’
            
    
            $obj = New-Object -TypeName PSObject -Property @{
            "path" = @()
            "users" = $null
            };
    
        #$IISLog
            #$a_regex.ToString()
            #$paths = $IISLog | select csuristem | ? {$_.csuristem -notmatch $a_regex} | Group-Object -Property csuristem | select Name,Count
            #$sites = Get-SPSite | select ServerRelativeUrl | Sort-Object -Property @{Expression={$_.ServerRelativeURL.length}; Ascending=$false}
            #$paths
    
            #for($i = 0; $i -lt $sites.count; $i++) {
            #    $obj.path+= New-Object -TypeName PSObject -Property @{
            #        "Name"=$sites[$i].ServerRelativeUrl.ToString()
            #        "Count"=0
            #    };
            #}
    
            #for($i = 0; $i -lt $paths.count; $i++) {
                #$paths[$i]
            #    for($j = 0; $j -lt $sites.count; $j++) {
    
                    #$sites[$j]
    
    
            #        if($paths[$i].Name.ToString()  -like $sites[$j].ServerRelativeUrl.ToString() + "*") {
                        #Write-Host "here"
            #            $obj.path[$j].Count += $paths[$i].Count
            #            break
            #        }
    
            #    }
                
            #}
            
            if($IISLog)
            {
                $obj.path = $IISLog | ? {$_.csuristem -notmatch $a_regex -and [datetime]($_.date + ' ' + $_.time) -gt $startDate -and [datetime]($_.date + ' ' + $_.time) -lt $endDate} | select csuristem |  Group-Object -Property csuristem | select Name,Count
                $obj.users = $IISLog | ? {$_.csuristem -notmatch $a_regex -and [datetime]($_.date + ' ' + $_.time) -gt $startDate -and [datetime]($_.date + ' ' + $_.time) -lt $endDate} | Group-Object -Property csusername | select Name, Count 
    
                #$obj.path = $IISLog | select csuristem |  Group-Object -Property csuristem | select Name,Count
    
    
                #$obj.users = $IISLog | Group-Object -Property csusername | select Name, Count
    
                #$obj.path
                #$obj.users
                $obj.path| select Name,count
                $obj.users| select @{Name="DisplaName";expression={$_.Name}}, @{Name="Displacount";expression={$_.Count}}| ft
               
            }
            
    }
#} #| Select Name, Count | ft -AutoSize
,$theArray | foreach{Write-Host $_}
