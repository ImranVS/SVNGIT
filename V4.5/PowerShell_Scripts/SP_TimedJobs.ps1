Get-SPTimerJob | ? { $_.IsDisabled -eq $False } | select HistoryEntries, Schedule, Farm | ForEach-Object {
    
    $obj = $_.HistoryEntries | Select-Object -First 1 -Property JobDefinitionTitle, WebApplicationName, ServerName, Status, StartTime, EndTime, DatabaseName, ErrorMessage
    $obj | Add-Member -MemberType NoteProperty -Name Schedule -Value $_.Schedule
    $obj | Add-Member -MemberType NoteProperty -Name Farm -Value $_.Farm.Name
    $obj

} | Sort-Object -Property JobDefinitionTitle