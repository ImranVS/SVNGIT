Imports System.Threading
Imports System.IO
Imports VSFramework
Imports System.Data.SqlClient
Imports VSNext.Mongo.Repository
Imports VSNext.Mongo.Entities
Imports MongoDB.Driver
Partial Public Class VitalSignsPlusCore




#Region "Database Handling"

#Region "Update Statistics Database"

    Public Sub UpdateStatisticsTable(ByVal SQLUpdateStatement As String, Optional ByVal SQLInsertStatement As String = "", Optional ByVal Comment As String = "")
        WriteAuditEntry(Now.ToString + " ********************* Common Statistics UPDATE ******************** ")

        Dim RA As Integer   'Rows affected
        Dim objVSAdaptor As New VSAdaptor
        If objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "Statistics", SQLUpdateStatement) = False And SQLInsertStatement <> "" Then
            objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "Statistics", SQLInsertStatement)
        End If
        'If boolUseSQLServer = True Then
        '    If ExecuteNonQuerySQL_VSS_Statistics(SQLUpdateStatement) = False And SQLInsertStatement <> "" Then
        '        ExecuteNonQuerySQL_VSS_Statistics(SQLInsertStatement)
        '    End If

        '    'WriteAuditEntry(Now.ToString + " Using SQL Server. My connection is " & SqlConnectionVitalSigns.State.ToString)
        '    'Try
        '    '    Dim myCommand As New Data.SqlClient.SqlCommand
        '    '    myCommand.Connection = SqlConnectionVitalSigns
        '    '    myCommand.CommandText = SQLUpdateStatement
        '    '    If SqlConnectionVitalSigns.State <> ConnectionState.Open Then
        '    '        SqlConnectionVitalSigns.Open()
        '    '    End If
        '    '    If myCommand.ExecuteNonQuery = 0 And SQLInsertStatement <> "" Then
        '    '        myCommand.CommandText = SQLInsertStatement
        '    '        myCommand.ExecuteNonQuery()
        '    '    End If

        '    '    myCommand.Dispose()
        '    'Catch ex As Exception
        '    '    WriteAuditEntry(Now.ToString + " ********************* Using SQL Server exception : " & ex.ToString)
        '    '    WriteAuditEntry(Now.ToString + " ********************* SQL Update : " & SQLUpdateStatement & vbCrLf)
        '    '    WriteAuditEntry(Now.ToString + " ********************* SQL Insert : " & SQLInsertStatement & vbCrLf)
        '    'End Try

        'Else
        '    ' WriteAuditEntry(Now.ToString + " Using Access ")
        '    Dim myPath As String
        '    Dim myRegistry As New RegistryHandler
        '    'Read the registry for the location of the Config Database

        '    Try
        '        myPath = myRegistry.ReadFromRegistry("Status Path")
        '        ' WriteAuditEntry(Now.ToString & " Domino Update e database " & myPath)
        '    Catch ex As Exception
        '        WriteAuditEntry(Now.ToString & " Failed to read registry in common Status table module. Exception: " & ex.Message)
        '    End Try

        '    If myPath Is Nothing Then
        '        WriteAuditEntry(Now.ToString & " Error: Failed to read registry in common Status table module.   Cannot locate Config Database 'status.mdb'.  Configure by running" & ProductName & " client before starting the service.")
        '        '   Return False
        '        Exit Sub
        '    End If
        '    myRegistry = Nothing

        '    Dim myCommand As New OleDb.OleDbCommand
        '    Dim myConnection As New OleDb.OleDbConnection
        '    'Dim myAdapter As New OleDb.OleDbDataAdapter

        '    With myConnection
        '        .ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & myPath
        '        .Open()
        '    End With

        '    Do Until myConnection.State = ConnectionState.Open
        '        myConnection.Open()
        '    Loop
        '    '   WriteAuditEntry(Now.ToString & " The Access database Status is " & myConnection.State.ToString)
        '    '***

        '    'Save it to the Access database
        '    If myConnection.State = ConnectionState.Open Then
        '        myCommand.CommandText = SQLUpdateStatement
        '        myCommand.Connection = myConnection
        '        RA = 0
        '        RA = myCommand.ExecuteNonQuery

        '        If RA = 0 Then
        '            'server wasn't in the status table, perhaps because it was previously disabled
        '            ' WriteAuditEntry(Now.ToString & " Update Failed because it affected 0 records with " & SQLUpdateStatement)

        '            myCommand.CommandText = SQLInsertStatement
        '            myCommand.ExecuteNonQuery()
        '            ' WriteAuditEntry(Now.ToString & " Update Failed because it affected 0 records with " & SQLInsertStatement)
        '            '  OleDbDataAdapterStatus.InsertCommand.CommandText = strSQL
        '            ' OleDbDataAdapterStatus.InsertCommand.ExecuteNonQuery()
        '        Else
        '            '     WriteAuditEntry(Now.ToString & " Updated " & RA.ToString & "  record.")
        '        End If
        '    End If

        '    Try
        '        myConnection.Close()
        '        myCommand.Dispose()
        '        myConnection.Dispose()

        '    Catch ex As Exception

        '    End Try
        'End If



    End Sub


#End Region


    Private Sub UpdateBES_Server_Status(ByRef BES_Server As MonitoredItems.BlackBerryServer)
        '******************************
        'Update the status table

        Dim strSQL As String
        Dim TypeAndName As String = ""
        Dim Percent As Double = 0
        Dim myResponseTime As Double = 0

        Try
            '  WriteAuditEntry(Now.ToString & " " & myDevice.Name & "'s  pending Messages : " & myDevice.PendingMessages)
            '  WriteAuditEntry(Now.ToString & " " & myDevice.Name & "'s  pending threshold :" & myDevice.BES_Pending_Messages_Threshold)
            '  WriteDeviceHistoryEntry("BES_Server", myDevice.Name, Now.ToString & " " & myDevice.Name & "'s  pending Messages : " & myDevice.PendingMessages)
            '    WriteDeviceHistoryEntry("BES_Server", myDevice.Name, Now.ToString & " " & myDevice.Name & "'s  pending threshold :" & myDevice.BES_Pending_Messages_Threshold)
            If BES_Server.BES_Pending_Messages_Threshold <> 0 Then
                Percent = (BES_Server.PendingMessages / BES_Server.BES_Pending_Messages_Threshold) * 100
                'WriteAuditEntry(Now.ToString & " % " & Percent)
            End If
            '    WriteDeviceHistoryEntry("BES_Server", myDevice.Name, Now.ToString & " " & myDevice.Name & "'s  pending percentage :" & Percent.ToString & "%")

        Catch ex As Exception
            Percent = 0
        End Try

        BES_Server.LastScan = Now
        Dim StatusDetails As String = ""

        StatusDetails = BES_Server.ResponseDetails
        If BES_Server.LicensesUsed > 0 Then
            StatusDetails += vbCrLf & "Licenses used: " & BES_Server.LicensesUsed
        End If


        Try
            If StatusDetails.Length > 254 Then
                StatusDetails = Left(StatusDetails, 250) & "..."
            End If
        Catch ex As Exception
            StatusDetails = "Status Details exceed allowable field length."
        End Try

        If BES_Server.BES_Version.Length > 254 Then
            BES_Server.BES_Version = Left(BES_Server.BES_Version, 250) & "..."
        End If

        Try
            If InStr(StatusDetails, "'") > 0 Then
                StatusDetails = StatusDetails.Replace("'", "")
            End If

            Dim Quote As Char
            Quote = Chr(34)

            If InStr(StatusDetails, Quote) > 0 Then
                StatusDetails = StatusDetails.Replace(Quote, "~")
            End If

        Catch ex As Exception
            StatusDetails = ""
        End Try
        Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
        Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repo.Filter.Where(Function(i) i.TypeAndName.Equals(TypeAndName))
        TypeAndName = BES_Server.Name & "-BlackBerry Server"
        Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)
        Try

            With BES_Server
                strSQL = "Update Status SET Status='" & BES_Server.Status & "', Upcount=" & BES_Server.UpCount & _
                 ", Details='" & StatusDetails & _
                "', Description='" & StatusDetails & _
                "', StatusCode='" & ServerStatusCode(BES_Server.Status) & _
                "', LastUpdate='" & Now & _
                "', PendingMail='" & BES_Server.PendingMessages & _
                "', NextScan='" & BES_Server.NextScan & "' WHERE TypeANDName='" & BES_Server.Name & "-BlackBerry Server'"
            End With
            updatedef = repo.Updater _
                        .Set(Function(i) i.DeviceName, BES_Server.Name) _
                        .[Set](Function(i) i.CurrentStatus, BES_Server.Status) _
                        .[Set](Function(i) i.StatusCode, BES_Server.Status) _
                        .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                        .[Set](Function(i) i.TypeAndName, TypeAndName) _
                        .[Set](Function(i) i.Description, StatusDetails) _
                        .[Set](Function(i) i.PendingMail, Integer.Parse(BES_Server.PendingMessages)) _
                        .[Set](Function(i) i.NextScan, BES_Server.NextScan) _
                        .[Set](Function(i) i.Details, StatusDetails)
            repo.Update(filterdef, updatedef)

            '  WriteDeviceHistoryEntry("BES_Server", myDevice.Name, Now.ToString & " Short SQL statement: " & strSQL)
            ' UpdateStatusTable(strSQL)
        Catch ex As Exception
        End Try

        '                "', DeadMail='" & myDevice.BES_Total_Messages_Xpired & _

        Try
            With BES_Server
                strSQL = "Update Status SET Status='" & BES_Server.Status & "', UpPercent= '" & BES_Server.UpPercentCount & _
                "', Details='" & StatusDetails & _
                "', StatusCode='" & ServerStatusCode(BES_Server.Status) & _
                "', LastUpdate='" & Now & _
                "', PendingMail='" & BES_Server.PendingMessages & _
                "', NextScan='" & BES_Server.NextScan & _
                "', DownMinutes='" & Microsoft.VisualBasic.Strings.Format(BES_Server.DownMinutes, "F1") & _
                "', UpMinutes='" & Microsoft.VisualBasic.Strings.Format(BES_Server.UpMinutes, "F1") & _
                "', UpPercentMinutes='" & BES_Server.UpPercentMinutes & _
                "', OperatingSystem='" & Left(BES_Server.BES_ServerName, 100) & _
                "', DominoVersion='" & Left(BES_Server.BES_Version, 100) & _
                "', ResponseThreshold='" & BES_Server.ResponseThreshold & _
                "', Description='" & StatusDetails & _
                "', MyPercent='" & Percent & "' WHERE TypeANDName='" & BES_Server.Name & "-BlackBerry Server'"

                '  "', ResponseTime='" & myResponseTime & _
                updatedef = repo.Updater _
                            .Set(Function(i) i.DeviceName, BES_Server.Name) _
                            .[Set](Function(i) i.CurrentStatus, BES_Server.Status) _
                            .[Set](Function(i) i.StatusCode, BES_Server.Status) _
                            .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                            .[Set](Function(i) i.TypeAndName, TypeAndName) _
                            .[Set](Function(i) i.Description, StatusDetails) _
                            .[Set](Function(i) i.PendingMail, Integer.Parse(BES_Server.PendingMessages)) _
                            .[Set](Function(i) i.NextScan, BES_Server.NextScan) _
                            .[Set](Function(i) i.DownMinutes, Integer.Parse(Microsoft.VisualBasic.Strings.Format(BES_Server.DownMinutes, "F1"))) _
                            .[Set](Function(i) i.UpMinutes, Double.Parse(Microsoft.VisualBasic.Strings.Format(BES_Server.UpMinutes, "F1"))) _
                            .[Set](Function(i) i.UpPercentMinutes, BES_Server.UpPercentMinutes) _
                            .[Set](Function(i) i.OperatingSystem, Left(BES_Server.BES_ServerName, 100)) _
                            .[Set](Function(i) i.ResponseThreshold, Integer.Parse(BES_Server.ResponseThreshold)) _
                            .[Set](Function(i) i.MyPercent, Double.Parse(Percent)) _
                            .[Set](Function(i) i.DominoVersion, Left(BES_Server.BES_Version, 100)) _
                            .[Set](Function(i) i.StatusCode, BES_Server.Status) _
                            .[Set](Function(i) i.Details, StatusDetails)
                'repo.Update(filterdef, updatedef)
            End With
        Catch ex As Exception
            strSQL = "Update Status SET DownCount= '" & BES_Server.DownCount & _
               "', Status='" & BES_Server.Status & "', Upcount=" & BES_Server.UpCount & _
              ", UpPercent= '" & BES_Server.UpPercentCount & _
               " WHERE TypeANDName='" & BES_Server.Name & "-BlackBerry Server'"
            WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " Error with SQL statement " & ex.Message)
            updatedef = repo.Updater _
                            .[Set](Function(i) i.CurrentStatus, BES_Server.Status) _
                            .[Set](Function(i) i.StatusCode, BES_Server.Status) _
                            .[Set](Function(i) i.TypeAndName, TypeAndName) _
                            .[Set](Function(i) i.DownCount, BES_Server.DownCount) _
                            .[Set](Function(i) i.UpCount, BES_Server.UpCount) _
                            .[Set](Function(i) i.UpPercent, Integer.Parse(BES_Server.UpPercentCount))
            'repo.Update(filterdef, updatedef)
        End Try

        UpdateStatusTable(strSQL)

        Exit Sub

    End Sub

    'Private Sub UpdateBESStatisticsTable(ByVal DeviceName As String, ByVal ResponseTime As Long)
    '    ' Exit Sub
    '    WriteDeviceHistoryEntry("BES_Server", DeviceName, Now.ToString & " Updating BES statistics table")
    '    Dim strSQL As String = ""
    '    Dim MyWeekNumber As Integer
    '    MyWeekNumber = GetWeekNumber(Date.Today)
    '    Dim s As New MongoStatementsInsert(Of DailyStatistics)
    '    Dim s1 As New DailyStatistics

    '    Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
    '    repo.Insert(s1)
    '    Try
    '        strSQL = "INSERT INTO DeviceDailyStats (DeviceType, ServerName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
    '         " VALUES ('BlackBerry Server', '" & DeviceName & "', '" & Now.ToString & "', '" & "ResponseTime" & "', '" & ResponseTime & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "')"
    '        s1.StatName = "ResponseTime"
    '        s1.StatValue = ResponseTime
    '        s1.DeviceId =

    '    Catch ex As Exception
    '        '   WriteAuditEntry(Now.ToString & " BES Stats table insert failed becase: " & ex.Message)
    '        WriteAuditEntry(Now.ToString & " The failed stats table insert command was " & strSQL)
    '    End Try
    '    MyWeekNumber = Nothing


    '    'If boolUseSQLServer = True Then
    '    '    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
    '    'Else

    '    '    Dim myCommand As New OleDb.OleDbCommand
    '    '    Dim myConnection As New OleDb.OleDbConnection

    '    '    Try
    '    '        With myConnection
    '    '            .ConnectionString = Me.OleDbConnectionStatistics.ConnectionString
    '    '            .Open()
    '    '        End With

    '    '        Do Until myConnection.State = ConnectionState.Open
    '    '            myConnection.Open()
    '    '        Loop

    '    '    Catch ex As Exception
    '    '        WriteAuditEntry(Now.ToString & " Error: exception connecting to  Status table with BES Statistics info: " & ex.Message)
    '    '    End Try

    '    '    '***
    '    Dim objVSAdaptor As New VSAdaptor
    '    Try
    '        'If myConnection.State = ConnectionState.Open Then
    '        '    myCommand.CommandText = strSQL
    '        '    myCommand.Connection = myConnection
    '        '    If strSQL <> "" Then myCommand.ExecuteNonQuery()
    '        'Else
    '        '    Exit Sub
    '        'End If

    '        objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " Error updating Status table with BES Statistic info: " & ex.Message & vbCrLf & strSQL)
    '    Finally
    '        'myConnection.Close()
    '        'myConnection.Dispose()
    '        'myCommand.Dispose()

    '    End Try
    '    '  End If
    'End Sub

    Private Sub UpdateBESDailyStatTable(ByVal ServerName As String, ByVal StatName As String, ByVal StatValue As Double, ByVal DeviceId As String)

        WriteAuditEntry(Now.ToString & " Updating BES Daily Statistics table")
        ' Dim repo As New MongoStatementsInsert(Of DailyStatistics)

        Dim strSQL As String = ""
        Dim MyWeekNumber As Integer
        MyWeekNumber = GetWeekNumber(Date.Today)
        Dim objVSAdaptor As New VSAdaptor
        Try
            ' strSQL = "INSERT INTO BESDailyStats (ServerName, [Date], StatName, StatValue,  WeekNumber, MonthNumber, YearNumber, DayNumber, HourNumber)" & _
            '" VALUES ('" & ServerName & "', '" & Now.ToString & "', '" & StatName & "', '" & StatValue & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "', '" & Now.Hour & "')"
            Dim DailyStats As New DailyStatistics
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
            DailyStats.DeviceId = DeviceId
            DailyStats.DeviceType = "BES"
            DailyStats.DeviceName = ServerName
            DailyStats.StatName = StatName
            DailyStats.StatValue = StatValue
            repo.Insert(DailyStats)
            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
        Catch ex As Exception

        Finally

        End Try

        'If boolUseSQLServer = True Then
        '    Try
        '        ExecuteNonQuerySQL_VSS_Statistics(strSQL)
        '    Catch ex As Exception

        '    End Try

        'Else
        '    Dim myConnection As New Data.OleDb.OleDbConnection
        '    myConnection.ConnectionString = Me.OleDbConnectionStatistics.ConnectionString
        '    ' WriteAuditEntry(Now.ToString & " myConnection string is " & myConnection.ConnectionString)

        '    Do While myConnection.State <> ConnectionState.Open
        '        myConnection.Open()
        '    Loop

        '    '  WriteAuditEntry(Now.ToString & " myConnection state is " & myConnection.State.ToString)
        '    Dim myCommand As New OleDb.OleDbCommand
        '    myCommand.Connection = myConnection

        '    myCommand.CommandText = strSQL
        '    myCommand.ExecuteNonQuery()
        '    myConnection.Close()
        '    myConnection.Dispose()
        '    myCommand.Dispose()
        'End If

    End Sub



#Region "Status Table Handling"
    'The routines here that are appended by "OLD" used the initial method of deleting all the
    'items of a given type from the status menu, then inserting the current values
    'but users did not like it, it made items seem to disappear.

    'new method does a delete of just the obsolete records

    Public Sub UpdateStatusTable(ByVal SQLUpdateStatement As String, Optional ByVal SQLInsertStatement As String = "", Optional ByVal Comment As String = "")
        '     WriteAuditEntry(Now.ToString + " ********************* Common STATUS UPDATE ******************** ")
        'If Comment <> "" Then
        '    WriteAuditEntry(Now.ToString + " *********************")
        '    WriteAuditEntry(Now.ToString + " " & Comment)
        '    WriteAuditEntry(Now.ToString + " *********************")
        'End If
        ''This routine is used to update the status table with the results and a device scan.  First it tries to 
        'do an update.  if that fails and an insert command is sent, it will try that.
        Dim RA As Integer
        Dim objVSAdaptor As New VSAdaptor
        If objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", SQLUpdateStatement) = False And SQLInsertStatement <> "" Then
            objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", SQLInsertStatement)
        End If
        'If boolUseSQLServer = True Then
        '    Try
        '        If ExecuteNonQueryUsingVitalSignsDB(SQLUpdateStatement) = 0 And SQLInsertStatement <> "" Then
        '            ExecuteNonQueryUsingVitalSignsDB(SQLInsertStatement)
        '        End If
        '    Catch ex As Exception

        '    End Try

        '    'WriteAuditEntry(Now.ToString + " Using SQL Server. My connection is " & SqlConnectionVitalSigns.State.ToString)
        '    'Try
        '    '    Dim myCommand As New Data.SqlClient.SqlCommand
        '    '    myCommand.Connection = SqlConnectionVitalSigns
        '    '    myCommand.CommandText = SQLUpdateStatement
        '    '    If SqlConnectionVitalSigns.State <> ConnectionState.Open Then
        '    '        SqlConnectionVitalSigns.Open()
        '    '    End If
        '    '    If myCommand.ExecuteNonQuery = 0 And SQLInsertStatement <> "" Then
        '    '        myCommand.CommandText = SQLInsertStatement
        '    '        myCommand.ExecuteNonQuery()
        '    '    End If

        '    '    myCommand.Dispose()
        '    'Catch ex As Exception
        '    '    WriteAuditEntry(Now.ToString + " ********************* Using SQL Server exception : " & ex.ToString)
        '    '    WriteAuditEntry(Now.ToString + " ********************* SQL Update : " & SQLUpdateStatement & vbCrLf)
        '    '    WriteAuditEntry(Now.ToString + " ********************* SQL Insert : " & SQLInsertStatement & vbCrLf)
        '    'End Try

        'Else
        '    ' WriteAuditEntry(Now.ToString + " Using Access ")
        '    Dim myPath As String = ""
        '    Dim myRegistry As New RegistryHandler
        '    'Read the registry for the location of the Config Database

        '    Try
        '        myPath = myRegistry.ReadFromRegistry("Status Path")
        '        ' WriteAuditEntry(Now.ToString & " Domino Update e database " & myPath)
        '    Catch ex As Exception
        '        WriteAuditEntry(Now.ToString & " Failed to read registry in common Status table module. Exception: " & ex.Message)
        '    End Try

        '    If myPath Is Nothing Then
        '        WriteAuditEntry(Now.ToString & " Error: Failed to read registry in common Status table module.   Cannot locate Config Database 'status.mdb'.  Configure by running" & ProductName & " client before starting the service.")
        '        '   Return False
        '        Exit Sub
        '    End If
        '    myRegistry = Nothing

        '    Dim myCommand As New OleDb.OleDbCommand
        '    Dim myConnection As New OleDb.OleDbConnection
        '    'Dim myAdapter As New OleDb.OleDbDataAdapter

        '    With myConnection
        '        .ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & myPath
        '        .Open()
        '    End With

        '    Do Until myConnection.State = ConnectionState.Open
        '        myConnection.Open()
        '    Loop
        '    '   WriteAuditEntry(Now.ToString & " The Access database Status is " & myConnection.State.ToString)
        '    '***

        '    'Save it to the Access database
        '    If myConnection.State = ConnectionState.Open Then
        '        myCommand.CommandText = SQLUpdateStatement
        '        myCommand.Connection = myConnection
        '        Try
        '            RA = 0
        '            RA = myCommand.ExecuteNonQuery
        '        Catch ex As Exception
        '            '  WriteAuditEntry(Now.ToString & " Exception processing status table: " & ex.ToString & vbCrLf & SQLUpdateStatement)
        '            RA = 0
        '        End Try


        '        If RA = 0 Then
        '            'server wasn't in the status table, perhaps because it was previously disabled
        '            '   WriteAuditEntry(Now.ToString & " Update Failed because it affected 0 records with " & SQLUpdateStatement)
        '            If SQLInsertStatement <> "" Then
        '                Try
        '                    myCommand.CommandText = SQLInsertStatement
        '                    myCommand.ExecuteNonQuery()
        '                Catch ex As Exception
        '                    '      WriteAuditEntry(Now.ToString & " Insert Statement error: " & ex.ToString)
        '                End Try

        '            End If

        '            '   WriteAuditEntry(Now.ToString & " Update Failed because it affected 0 records with " & SQLInsertStatement)
        '            '  OleDbDataAdapterStatus.InsertCommand.CommandText = strSQL
        '            ' OleDbDataAdapterStatus.InsertCommand.ExecuteNonQuery()
        '        Else
        '            '      WriteAuditEntry(Now.ToString & " Updated " & RA.ToString & "  record.")
        '        End If
        '    End If

        '    Try
        '        myConnection.Close()
        '        myCommand.Dispose()
        '        myConnection.Dispose()

        '    Catch ex As Exception

        '    End Try
        'End If

    End Sub

    Public Sub InitializeStatusTable()

        Try
            WriteAuditEntry(Now.ToString & " Inserting Mail Services into the status table.")
            If MyMailServices.Count > 0 Then
                UpdateStatusTableWithMailServices()
            End If
        Catch ex As Exception

        End Try

        Try
            WriteAuditEntry(Now.ToString & " Inserting Sametime Servers into the status table.")
            If mySametimeServers.Count > 0 Then
                UpdateStatusTableWithSametime()
            End If
        Catch ex As Exception

        End Try

        Try
            WriteAuditEntry(Now.ToString & " Inserting " & MyBlackBerryServers.Count & " monitored BES servers into the status table.")
            If MyBlackBerryServers.Count > 0 Then
                UpdateStatusTableWithBlackBerryServers()
            End If
        Catch ex As Exception

        End Try

        Try
            If MyURLs.Count > 0 Then
                UpdateStatusTableWithURLs()
            End If

        Catch ex As Exception

        End Try

        Try
            If MyClouds.Count > 0 Then
                UpdateStatusTableWithCloudURLs()
            End If

        Catch ex As Exception

        End Try


    End Sub


    Private Sub UpdateStatusTableWithBlackBerryServers()
        WriteAuditEntry(Now.ToString & " Updating status table with BlackBerry Servers.")

        'Delete from the status table any servers that have been deleted from the collection

        Dim strSQL As String
        Dim objVSAdaptor As New VSAdaptor
        'Now Update the status table with the new BB Servers
        Dim n As Integer
        Dim MyBlackBerryServer As MonitoredItems.BlackBerryServer
        Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
        Dim TypeAndName As String = ""
        Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)
        Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status)
        For n = 0 To MyBlackBerryServers.Count - 1
            Dim strSqlUpdate As String
            strSQL = ""
            MyBlackBerryServer = MyBlackBerryServers.Item(n)
            WriteAuditEntry(Now.ToString & " Updating status table with " & MyBlackBerryServer.Name)
            With MyBlackBerryServer
                If .Enabled = False Then .Status = "Disabled"
                .StatusCode = ServerStatusCode(MyBlackBerryServer.Status)
                .BES_Total_Messages_Xpired = 0
                TypeAndName = MyBlackBerryServer.Name & "-BlackBerry Server"
                strSQL = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + .Name + "-BlackBerry Server') BEGIN " & _
                 "INSERT INTO Status (StatusCode, NextScan, Category,  Description, Details, DownCount,  Location, Name,  Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime,  TypeANDName, Icon, PendingMail,  OperatingSystem, DominoVersion, UpMinutes, DownMinutes,  PendingThreshold, DeadThreshold) " & _
                 " VALUES ('" & .StatusCode & "', '" & .NextScan & "', '" & .Category & "', '" & .Description & "', '" & .ResponseDetails & "', '" & .DownCount & "', '" & .Location & "', '" & .Name & "', '" & .Status & "', 'BES', '" & .UpCount & "', '" & .UpPercentCount & "', '" & Now & "', '0', '" & .Name & "-BlackBerry Server', " & IconList.BlackBerry_Probe & ", " & .PendingMessages & ", '" & .BES_ServerName & "', '" & .BES_Version & "', " & Microsoft.VisualBasic.Strings.Format(.UpMinutes, "##,##0.#") & ", " & Microsoft.VisualBasic.Strings.Format(.DownMinutes, "##,##0.#") & ", " & .BES_Pending_Messages_Threshold & ", " & .BES_Expired_Messages_Theshold & ")" & _
                 "END"

                strSqlUpdate = "UPDATE Status SET NextScan = '" & .NextScan & "' WHERE TypeANDName = '" & .Name & "-BlackBerry Server'"
                filterdef = repo.Filter.Where(Function(i) i.TypeAndName.Equals(TypeAndName))

                updatedef = repo.Updater _
                         .Set(Function(i) i.DeviceName, .Name) _
                          .[Set](Function(i) i.CurrentStatus, .StatusCode) _
                          .[Set](Function(i) i.StatusCode, .StatusCode) _
                          .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                          .[Set](Function(i) i.TypeAndName, TypeAndName) _
                          .[Set](Function(i) i.Description, .Description) _
                          .[Set](Function(i) i.DeviceType, "BES") _
                          .[Set](Function(i) i.DownCount, Integer.Parse(.DownCount)) _
                          .[Set](Function(i) i.Location, .Location) _
                          .[Set](Function(i) i.UpCount, Integer.Parse(.UpCount)) _
                          .[Set](Function(i) i.UpPercent, Integer.Parse(.UpPercentCount)) _
                          .[Set](Function(i) i.ResponseTime, 0) _
                          .[Set](Function(i) i.Category, .Category) _
                          .[Set](Function(i) i.NextScan, .NextScan) _
                            .[Set](Function(i) i.Details, .ResponseDetails) _
                            .[Set](Function(i) i.PendingMail, Integer.Parse(.PendingMessages)) _
                            .[Set](Function(i) i.OperatingSystem, .BES_ServerName) _
                            .[Set](Function(i) i.DominoVersion, .BES_Version) _
                            .[Set](Function(i) i.PendingThreshold, Integer.Parse(.BES_Pending_Messages_Threshold)) _
                            .[Set](Function(i) i.DeadThreshold, Integer.Parse(.BES_Expired_Messages_Theshold)) _
                            .[Set](Function(i) i.UpMinutes, Integer.Parse(Microsoft.VisualBasic.Strings.Format(.UpMinutes, "##,##0.#"))) _
                            .[Set](Function(i) i.DownMinutes, Integer.Parse(Microsoft.VisualBasic.Strings.Format(.DownMinutes, "##,##0.#")))


            End With
            Try
                ' UpdateStatusTable(strSqlUpdate, strSQL, MyBlackBerryServer.Name)
                repo.Upsert(filterdef, updatedef)
            Catch ex As Exception
                If Not InStr(ex.Message, "duplicate") > 0 Then
                    WriteAuditEntry(Now.ToString & " Error Updating Status Table with BlackBerry info: " & ex.Message & vbCrLf & "Suspect SQL statement: " & strSQL)
                End If
            End Try
        Next n

        MyBlackBerryServer = Nothing
        strSQL = Nothing


    End Sub

    'Private Sub UpdateStatusTableWithDomino()
    '    'Delete from the status table any servers that have been deleted from the collection
    '    Dim myIndex As Integer
    '    Dim Dom As MonitoredItems.DominoServer
    '    Dim myServerNames As String = ""
    '    For Each Dom In MyDominoServers
    '        myServerNames += Dom.Name & vbCrLf
    '    Next

    '    Dom = Nothing
    '    ' myServerNames = Nothing
    '    myIndex = Nothing


    '    Dim dsStatusHTML As New Data.DataSet
    '    Dim Status As New Data.DataTable("Status")
    '    dsStatusHTML.Clear()
    '    dsStatusHTML.Tables.Add(Status)
    '    Dim drv As DataRowView



    '    Dim strSQL As String = "SELECT Name, Status, Type, TypeANDName FROM Status WHERE Type = 'Domino Server'"
    '    Dim objVSAdaptor As New VSAdaptor
    '    Try
    '        objVSAdaptor.FillDatasetAny("VitalSigns", "Status", strSQL, dsStatusHTML, "Status")
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " UpdateStatusTableWithDomino connection error @2 " & ex.Message)
    '    End Try
    '    'If boolUseSQLServer = True Then
    '    '    mySQLCommand.Connection = SqlConnectionVitalSigns
    '    '    mySQLCommand.CommandText = strSQL
    '    '    mySQLAdapter.SelectCommand = mySQLCommand
    '    '    mySQLAdapter.Fill(dsStatusHTML, "Status")
    '    'Else
    '    '    Try
    '    '        myCommand.CommandText = strSQL
    '    '        myCommand.Connection = myConnection
    '    '        myAdapter.SelectCommand = myCommand
    '    '        With myConnection
    '    '            .ConnectionString = Me.OleDbConnectionStatus.ConnectionString
    '    '            .Open()
    '    '        End With

    '    '        Do Until myConnection.State = ConnectionState.Open
    '    '            myConnection.Open()
    '    '        Loop
    '    '        myAdapter.Fill(dsStatusHTML, "Status")
    '    '    Catch ex As Exception
    '    '        WriteAuditEntry(Now.ToString & " UpdateStatusTableWithDomino connection error @2 " & ex.Message)
    '    '    End Try
    '    'End If

    '    Dim myView As New Data.DataView(Status)

    '    Try
    '        myView.Sort = "Type ASC"
    '        WriteAuditEntry(Now.ToString & " UpdateStatusTableWithDomino delete propogation is processing " & myView.Count & " records.")
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " UpdateStatusTableWithDomino module error " & ex.Message & " source: " & ex.Source)
    '    End Try



    '    Try
    '        For Each drv In myView
    '            Dim myName As String
    '            myName = drv("Name")
    '            If InStr(myServerNames, myName) > 0 Then
    '                'the server has not been deleted
    '                If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & myName & " is not marked for deletion. ")
    '            Else
    '                'the server has been deleted, so delete from the status table
    '                Try
    '                    strSQL = "DELETE FROM Status WHERE Type = 'Domino Server' AND Name = '" & myName & "'"
    '                    objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
    '                Catch ex As Exception
    '                    WriteAuditEntry(Now.ToString & " Error executing query updating Domino status table: " & ex.Message & vbCrLf & strSQL & vbCrLf, LogLevel.Normal)
    '                End Try


    '                If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & myName & " has been deleted from the Status table by the service.")

    '            End If
    '        Next
    '    Catch ex As Exception

    '    End Try

    '    'Insert any Domino servers that are not in the status table
    '    WriteAuditEntry(Now.ToString & " Inserting Domino servers into the status table....")
    '    Dim n As Integer
    '    Dim myDominoServer As MonitoredItems.DominoServer
    '    Dim Percent As Double
    '    For n = 0 To MyDominoServers.Count - 1
    '        Try
    '            myDominoServer = MyDominoServers.Item(n)
    '        Catch ex As Exception
    '            WriteAuditEntry(Now.ToString & " Error locating Domino servers for the Status Table " & ex.ToString)
    '        End Try


    '        With myDominoServer
    '            WriteAuditEntry(Now.ToString & " Processing server " & myDominoServer.Name & "  (" & n.ToString & ")")
    '            Try
    '                Percent = myDominoServer.PendingMail / myDominoServer.PendingThreshold * 100
    '            Catch ex As Exception
    '                Percent = 0
    '            End Try

    '            Try
    '                myDominoServer.StatusCode = ServerStatusCode(myDominoServer.Status)
    '            Catch ex As Exception

    '            End Try
    '            strSQL = "INSERT INTO Status (StatusCode, Category, DeadMail, Description, DownCount,  Location, Name, MailDetails, PendingMail, Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime, TypeANDName, Icon, OperatingSystem, DominoVersion, MyPercent, NextScan, Details, UpMinutes, DownMinutes, UpPercentMinutes, PendingThreshold, DeadThreshold) " & _
    '               " VALUES ( '" & .StatusCode & "', '" & .Category & "', '" & .DeadMail & "', '" & .Description & "', '" & .DownCount & "', '" & .Location & "', '" & .Name & "', ' ', '" & .PendingMail & "', '" & .Status & "',  " & _
    '            "'Domino Server', '" & .UpCount & "', '" & .UpPercentCount & "', '" & Now & "', '" & .ResponseTime & "' , '" & .Name & "-Domino', " & IconList.DominoServer & ", '" & .OperatingSystem & "', '" & .VersionDomino & "', " & Percent & ", '" & .NextScan & "', '" & .ResponseDetails & "', '" & Microsoft.VisualBasic.Strings.Format(myDominoServer.UpMinutes, "##,##0.#") & "', '" & Microsoft.VisualBasic.Strings.Format(myDominoServer.DownMinutes, "##,##0.#") & "', '0', " & .PendingThreshold & ", " & .DeadThreshold & ")"
    '        End With
    '        Try
    '            ' WriteAuditEntry(Now.ToString & " Processing server " & myDominoServer.Name & "  (" & n.ToString & ")") ' with " & strSQL)
    '            UpdateStatusTable(strSQL, "", "Processing " & myDominoServer.Name)
    '        Catch ex As Exception
    '            'WriteAuditEntry(Now.ToString & " Error inserting Domino servers into the Status Table " & ex.ToString)
    '        End Try

    '    Next n

    '    myDominoServer = Nothing
    '    Percent = Nothing
    '    n = Nothing

    '    'Clean up the memory when done of all unmanaged resources
    '    Try

    '        myView.Dispose()
    '        dsStatusHTML.Dispose()
    '        Status.Dispose()

    '    Catch ex As Exception

    '    End Try


    'End Sub


    Private Sub UpdateStatusTableWithSametime()
        'Delete from the status table any servers that have been deleted from the collection

        Dim strSQL As String
        Dim objVSAdaptor As New VSAdaptor

        'Insert any Sametime servers that are not in the status table
        If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " About to insert " & mySametimeServers.Count & " Sametime servers into the Status table.")

        Dim n As Integer
        ' Dim mySametimeServer As MonitoredItems.SametimeServer
        Dim Percent As Double
        Dim strSqlUpdate As String
        Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)

        For n = 0 To mySametimeServers.Count - 1
            MySametimeServer = mySametimeServers.Item(n)
            Dim myStatusCode As String = ServerStatusCode(MySametimeServer.Status)
            Dim TypeAndName As String = MySametimeServer.Name & "-" & VSNext.Mongo.Entities.Enums.ServerType.Sametime.ToDescription()
            Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repo.Filter.Where(Function(i) i.TypeAndName.Equals(TypeAndName))
            Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)
            With MySametimeServer

                'strSQL = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + .Name + "-Sametime') BEGIN " & _
                ' "INSERT INTO Status (StatusCode, Category,  Description, DownCount,  Location, Name,  Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime, TypeANDName, Icon,  MyPercent, NextScan,  UpMinutes, DownMinutes, PendingThreshold, DeadThreshold) " & _
                ' " VALUES ('" & myStatusCode & "', '" & .Category & "', '" & .Description & "', '" & .DownCount & "', '" & .Location & "', '" & .Name & "', '" & .Status & "',  " & _
                ' "'Sametime', '" & .UpCount & "', '" & .UpPercentCount & "', '" & Now & "', '" & .ResponseTime & "' , '" & .Name & "-Sametime', " & IconList.Sametime & ", '" & Percent & "', '" & .NextScan & "', '" & Microsoft.VisualBasic.Strings.Format(.UpMinutes, "F1") & "', '" & Microsoft.VisualBasic.Strings.Format(.DownMinutes, "F1") & "', " & .Chat_Sessions_Threshold & ", " & .nWay_Chat_Sessions_Threshold & ")" & _
                ' "END"

                'strSqlUpdate = "UPDATE Status SET NextScan = '" & .NextScan & "' WHERE TypeANDName = '" & .Name & "-Sametime'"
                Try
                    updatedef = repo.Updater _
                           .Set(Function(i) i.DeviceName, .Name) _
                           .[Set](Function(i) i.CurrentStatus, .Status) _
                           .[Set](Function(i) i.StatusCode, myStatusCode) _
                           .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                           .[Set](Function(i) i.TypeAndName, TypeAndName) _
                           .[Set](Function(i) i.Description, .Description) _
                           .[Set](Function(i) i.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Sametime.ToDescription()) _
                           .[Set](Function(i) i.DownCount, Integer.Parse(.DownCount)) _
                           .[Set](Function(i) i.Location, .Location) _
                           .[Set](Function(i) i.UpCount, Integer.Parse(.UpCount)) _
                           .[Set](Function(i) i.UpPercent, Double.Parse(.UpPercentCount)) _
                           .[Set](Function(i) i.ResponseTime, Integer.Parse(.ResponseTime)) _
                           .[Set](Function(i) i.NextScan, .NextScan) _
                           .[Set](Function(i) i.UpMinutes, Integer.Parse(.UpMinutes)) _
                           .[Set](Function(i) i.DownMinutes, Integer.Parse(.DownMinutes)) _
                           .[Set](Function(i) i.DeviceId, .ServerObjectID)
                    If MySametimeServer.Enabled = True Then
                        'UpdateStatusTable(strSqlUpdate, strSQL, MySametimeServer.Name)
                        repo.Upsert(filterdef, updatedef)
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception writing to mongo: " & ex.Message.ToString())
                End Try

            End With






        Next n

        n = Nothing
    End Sub


    Private Sub UpdateStatusTableWithMailServices()
        '  WriteAuditEntry(Now.ToString & " Updating status table with Mail Services.")
        'Now delete the existing  records 
        Dim objVSAdaptor As New VSAdaptor
        Dim strSQL As String = ""
        Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
        Try
            Dim n As Integer
            Dim MyMailService As MonitoredItems.MailService

            For n = 0 To MyMailServices.Count - 1
                MyMailService = MyMailServices.Item(n)
                WriteAuditEntry(Now.ToString & " Adding Mail Service " & MyMailService.Name & " to status table.")
                Dim TypeAndName As String = MyMailService.Name & "-Mail"
                Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repo.Filter.Where(Function(i) i.TypeAndName.Equals(TypeAndName))
                Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)
                With MyMailService
                    '5/5/2016 NS modified - inserting a Mail into StatusDetails fails because of a TypeANDName mismatch
                    'Changed TypeANDName -MS to -Mail
                    strSQL = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + .Name + "-Mail') BEGIN " & _
                     "INSERT INTO Status (StatusCode, Category,  Description, Details, DownCount,  Location, Name,  Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime, TypeANDName, Icon) " & _
                     " VALUES ('" & .StatusCode & "', '" & .Category & "', '" & .Description & "', ' ', '" & .DownCount & "', '" & .Location & "', '" & .Name & "', '" & .Status & "', 'Mail', '" & .UpCount & "', '" & .UpPercentCount & "', '" & Now & "', '0', '" & .Name & "-Mail', " & IconList.Mail_Service & ")" & _
                     "END"

                    updatedef = repo.Updater _
                          .Set(Function(i) i.DeviceName, .Name) _
                          .[Set](Function(i) i.CurrentStatus, .StatusCode) _
                          .[Set](Function(i) i.StatusCode, .StatusCode) _
                          .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                          .[Set](Function(i) i.TypeAndName, TypeAndName) _
                          .[Set](Function(i) i.Description, .Description) _
                          .[Set](Function(i) i.DeviceType, "Mail") _
                          .[Set](Function(i) i.DownCount, Integer.Parse(.DownCount)) _
                          .[Set](Function(i) i.Location, .Location) _
                          .[Set](Function(i) i.UpCount, Integer.Parse(.UpCount)) _
                          .[Set](Function(i) i.UpPercent, Integer.Parse(.UpPercentCount)) _
                          .[Set](Function(i) i.ResponseTime, Integer.Parse(.ResponseTime)) _
                          .[Set](Function(i) i.Category, .Category) _
                          .[Set](Function(i) i.NextScan, .NextScan)

                End With
                Try
                    'objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
                    repo.Upsert(filterdef, updatedef)

                Catch ex As Exception

                End Try
            Next n
            MyMailService = Nothing
            n = Nothing
        Catch ex As Exception
            WriteAuditEntry("Failure updating status table with  Mail Service info: " & ex.Message)
            WriteAuditEntry(Now.ToString & " Insert command was " & strSQL)
        End Try

        strSQL = Nothing

        'myConnection.Close()
        'myConnection.Dispose()
    End Sub


    'Private Sub UpdateStatusTableWithDominoClusters()
    '    WriteAuditEntry(Now.ToString & " Updating status table with Lotus Domino servers clusters.")
    '    'Delete from the status table any servers that have been deleted from the collection
    '    Dim myIndex As Integer
    '    Dim Dom As MonitoredItems.DominoMailCluster
    '    Dim myServerNames As String = ""
    '    For Each Dom In myDominoClusters
    '        myServerNames += Dom.Name & vbCrLf
    '    Next

    '    Dom = Nothing
    '    ' myServerNames = Nothing
    '    myIndex = Nothing

    '    'Connect to the data source
    '    Dim dsStatusHTML As New Data.DataSet
    '    Dim Status As New Data.DataTable("Status")
    '    dsStatusHTML.Clear()
    '    dsStatusHTML.Tables.Add(Status)
    '    Dim drv As DataRowView

    '    Dim myConnection As New OleDb.OleDbConnection
    '    Dim myAdapter As New OleDb.OleDbDataAdapter
    '    Dim myCommand As New OleDb.OleDbCommand


    '    Dim mySQLCommand As New Data.SqlClient.SqlCommand
    '    Dim mySQLAdapter As New Data.SqlClient.SqlDataAdapter


    '    Dim strSQL As String = "SELECT Name, Status, Type, TypeANDName FROM Status WHERE Type = 'Domino Cluster'"
    '    Dim objVSAdaptor As New VSAdaptor
    '    Try
    '        objVSAdaptor.FillDatasetAny("VitalSigns", "Status", strSQL, dsStatusHTML, "Status")
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " UpdateStatusTableWithDominoCluster connection error @2 " & ex.Message)
    '    End Try
    '    'If boolUseSQLServer = True Then
    '    '    mySQLCommand.Connection = SqlConnectionVitalSigns
    '    '    mySQLCommand.CommandText = strSQL
    '    '    mySQLAdapter.SelectCommand = mySQLCommand
    '    '    mySQLAdapter.Fill(dsStatusHTML, "Status")
    '    'Else
    '    '    Try
    '    '        myCommand.CommandText = strSQL
    '    '        myCommand.Connection = myConnection
    '    '        myAdapter.SelectCommand = myCommand
    '    '        With myConnection
    '    '            .ConnectionString = Me.OleDbConnectionStatus.ConnectionString
    '    '            .Open()
    '    '        End With

    '    '        Do Until myConnection.State = ConnectionState.Open
    '    '            myConnection.Open()
    '    '        Loop
    '    '        myAdapter.Fill(dsStatusHTML, "Status")
    '    '    Catch ex As Exception
    '    '        WriteAuditEntry(Now.ToString & " UpdateStatusTableWithDominoCluster connection error @2 " & ex.Message)
    '    '    End Try
    '    'End If

    '    Dim myView As New Data.DataView(Status)

    '    Try
    '        myView.Sort = "Type ASC"
    '        WriteAuditEntry(Now.ToString & " UpdateStatusTableWithDominoCluster module is processing " & myView.Count & " records.")
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " UpdateStatusTableWithDominoCluster module error " & ex.Message & " source: " & ex.Source)
    '    End Try



    '    Try
    '        For Each drv In myView
    '            Dim myName As String
    '            myName = drv("Name")
    '            If InStr(myServerNames, myName) > 0 Then
    '                'the server has not been deleted
    '                If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & myName & " is not marked for deletion. ")
    '            Else
    '                'the server has been deleted, so delete from the status table
    '                Try
    '                    strSQL = "DELETE FROM Status WHERE Type = 'Domino Cluster' AND Name = '" & myName & "'"
    '                    objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
    '                Catch ex As Exception
    '                    WriteAuditEntry(Now.ToString & " Error executing query updating Domino status table: " & ex.Message & vbCrLf & strSQL & vbCrLf, LogLevel.Normal)
    '                End Try

    '                'If boolUseSQLServer = True Then
    '                '    Try
    '                '        mySQLCommand.CommandText = strSQL
    '                '        mySQLCommand.ExecuteNonQuery()
    '                '    Catch ex As Exception
    '                '        WriteAuditEntry(Now.ToString & " Error executing query updating Domino status table: " & ex.Message & vbCrLf & strSQL & vbCrLf)
    '                '    End Try

    '                'Else
    '                '    Try
    '                '        myCommand.CommandText = strSQL
    '                '        myCommand.ExecuteNonQuery()
    '                '    Catch ex As Exception
    '                '        WriteAuditEntry(Now.ToString & " Error executing query updating Domino status table: " & ex.Message & vbCrLf & strSQL & vbCrLf)
    '                '    End Try
    '                'End If

    '                If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & myName & " has been deleted from the Status table by the service.")

    '            End If
    '        Next
    '    Catch ex As Exception

    '    End Try

    '    'Now Update the Data set with the current Domino data 

    '    Try
    '        Dim n As Integer
    '        Dim myDominoServerCluster As MonitoredItems.DominoMailCluster
    '        For n = 0 To myDominoClusters.Count - 1
    '            myDominoCluster = myDominoClusters.Item(n)

    '            With myDominoCluster
    '                strSQL = "INSERT INTO Status ( Name, Status, Type,  LastUpdate,  TypeANDName, Icon,  NextScan, Details, Category) " & _
    '    " VALUES ('" & .Name & "', '" & .Status & "',  'Domino Cluster', '" & Now & "', '" & .Name & "-Cluster', " & IconList.DominoServer & ", '" & .NextScan & "', '" & .ResponseDetails & "', '" & .Category & "')"
    '            End With
    '            Try
    '                UpdateStatusTable(strSQL, "", myDominoCluster.Name)
    '            Catch ex As Exception
    '                ' WriteAuditEntry(Now.ToString & " Error executing query updating Domino cluster status table: " & ex.Message)
    '            End Try
    '        Next n

    '        myDominoCluster = Nothing

    '        n = Nothing


    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " Error connecting to Status Table while inserting Domino Clusters:" & ex.Message)
    '        WriteAuditEntry(Now.ToString & " Insert comand was " & strSQL)
    '        '       WriteAuditEntry(Now.ToString & " Service stopped.")
    '        Thread.CurrentThread.Sleep(1000)
    '        ' End
    '    End Try

    '    mySQLCommand.Dispose()
    '    mySQLAdapter.Dispose()
    '    myCommand.Dispose()
    '    myConnection.Close()
    '    myConnection.Dispose()
    '    WriteAuditEntry(Now.ToString & " Finished updating Status table with all Domino server clusters.")

    'End Sub

    'Private Sub UpdateStatusTableWithNotesDatabases()
    '    'Delete from the status table any servers that have been deleted from the collection
    '    Dim myIndex As Integer
    '    Dim NDB As MonitoredItems.NotesDatabase
    '    Dim myNDBNames As String
    '    For Each NDB In MyNotesDatabases
    '        myNDBNames += NDB.Name & vbCrLf
    '    Next

    '    NDB = Nothing
    '    myIndex = Nothing


    '    'Connect to the data source
    '    Dim dsStatusHTML As New Data.DataSet
    '    Dim Status As New Data.DataTable("Status")
    '    dsStatusHTML.Clear()
    '    dsStatusHTML.Tables.Add(Status)
    '    Dim drv As DataRowView

    '    Dim myConnection As New OleDb.OleDbConnection
    '    Dim myAdapter As New OleDb.OleDbDataAdapter
    '    Dim myCommand As New OleDb.OleDbCommand


    '    Dim mySQLCommand As New Data.SqlClient.SqlCommand
    '    Dim mySQLAdapter As New Data.SqlClient.SqlDataAdapter


    '    Dim strSQL As String = "SELECT Name, Status, Type, TypeANDName FROM Status WHERE Type = 'Notes Database'"
    '    Dim objVSAdaptor As New VSAdaptor
    '    Try
    '        objVSAdaptor.FillDatasetAny("VitalSigns", "Status", strSQL, dsStatusHTML, "Status")
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " UpdateStatusTableWithNotesDBs connection error @2 " & ex.Message)
    '    End Try
    '    'If boolUseSQLServer = True Then
    '    '    mySQLCommand.Connection = SqlConnectionVitalSigns
    '    '    mySQLCommand.CommandText = strSQL
    '    '    mySQLAdapter.SelectCommand = mySQLCommand
    '    '    mySQLAdapter.Fill(dsStatusHTML, "Status")
    '    'Else
    '    '    Try
    '    '        myCommand.CommandText = strSQL
    '    '        myCommand.Connection = myConnection
    '    '        myAdapter.SelectCommand = myCommand
    '    '        With myConnection
    '    '            .ConnectionString = Me.OleDbConnectionStatus.ConnectionString
    '    '            .Open()
    '    '        End With

    '    '        Do Until myConnection.State = ConnectionState.Open
    '    '            myConnection.Open()
    '    '        Loop
    '    '        myAdapter.Fill(dsStatusHTML, "Status")
    '    '    Catch ex As Exception
    '    '        WriteAuditEntry(Now.ToString & " UpdateStatusTableWithNotesDBs connection error @2 " & ex.Message)
    '    '    End Try
    '    'End If

    '    Dim myView As New Data.DataView(Status)

    '    Try
    '        myView.Sort = "Type ASC"
    '        WriteAuditEntry(Now.ToString & " UpdateStatusTableWithNotesDBs module is processing " & myView.Count & " records.")
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " UpdateStatusTableWithNotesDBs module error " & ex.Message & " source: " & ex.Source)
    '    End Try



    '    Try
    '        For Each drv In myView
    '            Dim myName As String
    '            myName = drv("Name")
    '            If InStr(myNDBNames, myName) > 0 Then
    '                'the server has not been deleted
    '                If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & myName & " is not marked for deletion. ")
    '            Else
    '                'the server has been deleted, so delete from the status table
    '                Try
    '                    strSQL = "DELETE FROM Status WHERE Type = 'Notes Database' AND Name = '" & myName & "'"
    '                    objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
    '                Catch ex As Exception
    '                    WriteAuditEntry(Now.ToString & " Error executing query updating Notes Database status table: " & ex.Message & vbCrLf & strSQL & vbCrLf, LogLevel.Normal)
    '                End Try

    '                'If boolUseSQLServer = True Then
    '                '    Try
    '                '        mySQLCommand.CommandText = strSQL
    '                '        mySQLCommand.ExecuteNonQuery()
    '                '    Catch ex As Exception
    '                '        WriteAuditEntry(Now.ToString & " Error executing query updating Notes Database status table: " & ex.Message & vbCrLf & strSQL & vbCrLf)
    '                '    End Try

    '                'Else
    '                '    Try
    '                '        myCommand.CommandText = strSQL
    '                '        myCommand.ExecuteNonQuery()
    '                '    Catch ex As Exception
    '                '        WriteAuditEntry(Now.ToString & " Error executing query updating Notes Database  status table: " & ex.Message & vbCrLf & strSQL & vbCrLf)
    '                '    End Try
    '                'End If

    '                If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & myName & " has been deleted from the Status table by the service.")

    '            End If
    '        Next
    '    Catch ex As Exception

    '    End Try

    '    'Insert any Notes databases that are not in the status table

    '    Dim n As Integer

    '    Dim Percent As Double
    '    For n = 0 To MyNotesDatabases.Count - 1
    '        NDB = MyNotesDatabases.Item(n)

    '        '5/5/2016 NS modified
    '        'Changed -NDB to -Notes Database
    '        With NDB
    '            strSQL = "INSERT INTO Status (Category,  Description, Details, DownCount,  Location, Name,  Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime, TypeANDName, Icon, ResponseThreshold, NextScan) " & _
    '      " VALUES ('" & .Category & "', '" & .Description & "', '', '" & .DownCount & "', '" & .Location & "', '" & .Name & "', '" & .Status & "', 'Notes Database', '" & .UpCount & "', '" & .UpPercentCount & "', '" & .LastScan & "', " & .ResponseTime & ", '" & .Name & "-Notes Database', " & IconList.NotesDB & ", " & .ResponseThreshold & ", '" & .NextScan & "')"
    '        End With
    '        Try
    '            UpdateStatusTable(strSQL)
    '        Catch ex As Exception
    '            If Not InStr(ex.ToString, "duplicate") Then
    '                '  WriteAuditEntry(Now.ToString & " Error executing query updating Notes Databases status table: " & ex.tostring)
    '            End If
    '        End Try
    '    Next n

    '    NDB = Nothing
    '    Percent = Nothing
    '    n = Nothing

    '    'Clean up the memory when done of all unmanaged resources
    '    myCommand.Dispose()
    '    myConnection.Close()
    '    myConnection.Dispose()
    '    myView.Dispose()
    '    dsStatusHTML.Dispose()
    '    Status.Dispose()
    '    mySQLCommand.Dispose()
    '    mySQLAdapter.Dispose()
    'End Sub


    'Private Sub UpdateStatusTableWithNotesMailProbes()
    '    WriteAuditEntry(Now.ToString & " Updating status table with NotesMail Probes.")
    '    'Now delete the existing NM records 

    '    Dim strSQL As String = "DELETE FROM Status WHERE Type = 'NotesMail Probe';"
    '    'Dim myConnection As New OleDb.OleDbConnection
    '    'myConnection.ConnectionString = Me.OleDbConnectionStatus.ConnectionString
    '    Dim objVSAdaptor As New VSAdaptor
    '    Try
    '        objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " Error with Status Table while deleting obsolete NotesMail Probes:" & ex.Message)
    '    End Try

    '    'If boolUseSQLServer = True Then
    '    '    Try
    '    '        Dim myCommand As New Data.SqlClient.SqlCommand
    '    '        myCommand.Connection = SqlConnectionVitalSigns
    '    '        myCommand.CommandText = strSQL
    '    '        myCommand.ExecuteNonQuery()
    '    '        myCommand.Dispose()
    '    '    Catch ex As Exception

    '    '    End Try
    '    'Else
    '    '    myConnection.Open()
    '    '    Dim myCommand As New OleDb.OleDbCommand(strSQL, myConnection)

    '    '    Try
    '    '        myCommand.ExecuteNonQuery()
    '    '        myCommand.Dispose()
    '    '    Catch ex As Exception
    '    '        WriteAuditEntry(Now.ToString & " Error with Status Table while deleting obsolete NotesMail Probes:" & ex.Message)
    '    '    End Try
    '    'End If


    '    'Now Update the Data set with the Mail Probes
    '    Dim n As Integer
    '    Dim MyNotesMailProbe As MonitoredItems.DominoMailProbe

    '    For n = 0 To MyNotesMailProbes.Count - 1
    '        strSQL = ""
    '        MyNotesMailProbe = MyNotesMailProbes.Item(n)
    '        WriteAuditEntry(Now.ToString & " adding Mail Probe " & MyNotesMailProbe.Name)
    '        With MyNotesMailProbe
    '            strSQL = "INSERT INTO Status (Category,  Description, Details, DownCount,  Location, Name,  Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime,  TypeANDName, Icon) " & _
    '               " VALUES ('" & .Category & "', '" & .Description & "', ' ', '" & .DownCount & "', '" & .Location & "', '" & .Name & "', 'Not Scanned', 'NotesMail Probe', '" & .UpCount & "', '" & .UpPercentCount & "', '" & Now & "', '0', '" & .Name & "-NMP', " & IconList.NotesMail_Probe & ")"
    '        End With

    '        Try
    '            objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
    '            'If boolUseSQLServer = True Then
    '            '    'WriteAuditEntry(Now.ToString & " Inserting into SQL server: " & strSQL)
    '            '    Dim myCommand As New Data.SqlClient.SqlCommand
    '            '    myCommand.Connection = SqlConnectionVitalSigns
    '            '    myCommand.CommandText = strSQL
    '            '    myCommand.ExecuteNonQuery()
    '            '    myCommand.Dispose()
    '            'Else
    '            '    Dim myCommand As New OleDb.OleDbCommand
    '            '    myCommand.Connection = myConnection
    '            '    myCommand.CommandText = strSQL
    '            '    myCommand.ExecuteNonQuery()
    '            '    myCommand.Dispose()
    '            '    myConnection.Close()
    '            '    myConnection.Dispose()
    '            'End If

    '        Catch ex As Exception

    '        End Try
    '    Next n
    '    MyNotesMailProbe = Nothing
    '    n = Nothing
    '    strSQL = Nothing
    '    WriteAuditEntry(Now.ToString & " Updated Status table with NotesMail Probe information.")


    'End Sub

    Private Sub UpdateStatusTableWithWebSphere()
        'Delete from the status table any servers that have been deleted from the collection

        Dim strSQL As String
        Dim objVSAdaptor As New VSAdaptor
        Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
        'Insert any WebSphere servers that are not in the status table
        If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " About to insert " & MyWebSphereServers.Count & " WebSphere servers into the Status table.")

        Dim n As Integer
        Dim strSqlUpdate As String

        For n = 0 To MyWebSphereServers.Count - 1
            MyWebSphereServer = MyWebSphereServers.Item(n)
            Dim myStatusCode As String = ServerStatusCode(MyWebSphereServer.Status)
            Dim TypeAndName As String = MyWebSphereServer.Name & "-WebSphere"
            Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repo.Filter.Where(Function(i) i.TypeAndName.Equals(TypeAndName))
            Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)
            With MyWebSphereServer

                strSQL = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + .Name + "-WebSphere') BEGIN " & _
                  "INSERT INTO Status (StatusCode, Category,  Description, Location, Name,  Status, Type, LastUpdate, ResponseTime, TypeANDName, NextScan) " & _
                  " VALUES ('" & myStatusCode & "', '" & .Category & "', '" & .Description & "', '" & .Location & "', '" & .Name & "', '" & .Status & "',  " & _
                  "'WebSphere', '" & Now & "', '" & .ResponseTime & "' , '" & .Name & "-" & .ServerType & "', '" & .NextScan & "' )" & _
                  "END"

                strSqlUpdate = "UPDATE Status SET NextScan = '" & .NextScan & "' WHERE TypeANDName = '" & .Name & "-" & .ServerType & "'"
                updatedef = repo.Updater _
                         .Set(Function(i) i.DeviceName, .Name) _
                         .[Set](Function(i) i.CurrentStatus, .StatusCode) _
                         .[Set](Function(i) i.StatusCode, .StatusCode) _
                         .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                         .[Set](Function(i) i.TypeAndName, TypeAndName) _
                         .[Set](Function(i) i.Description, .Description) _
                         .[Set](Function(i) i.DeviceType, "WebSphere") _
                         .[Set](Function(i) i.DownCount, Integer.Parse(.DownCount)) _
                         .[Set](Function(i) i.Location, .Location) _
                         .[Set](Function(i) i.ResponseTime, Integer.Parse(.ResponseTime)) _
                         .[Set](Function(i) i.Category, .Category) _
                         .[Set](Function(i) i.NextScan, .NextScan)
            End With
            If MyWebSphereServer.Enabled = True Then
                'UpdateStatusTable(strSqlUpdate, strSQL, MyWebSphereServer.Name)
                repo.Upsert(filterdef, updatedef)
            End If

        Next n

        n = Nothing
    End Sub

    Private Sub UpdateStatusTableWithIbmConnections()
        'Delete from the status table any servers that have been deleted from the collection

        Dim strSQL As String
        Dim objVSAdaptor As New VSAdaptor
        Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
        'Insert any WebSphere servers that are not in the status table
        If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " About to insert " & MyIBMConnectServers.Count & " Ibm Connection servers into the Status table.")

        Dim n As Integer
        Dim strSqlUpdate As String

        For n = 0 To MyIBMConnectServers.Count - 1
            MyIBMConnectServer = MyIBMConnectServers.Item(n)
            Dim myStatusCode As String = ServerStatusCode(MyIBMConnectServer.Status)
            Dim TypeAndName As String = MyIBMConnectServer.Name & MyIBMConnectServer.ServerType
            Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repo.Filter.Where(Function(i) i.TypeAndName.Equals(TypeAndName))
            Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)
            With MyIBMConnectServer

                strSQL = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + .Name + "-WebSphere') BEGIN " & _
                  "INSERT INTO Status (StatusCode, Category,  Description, Location, Name,  Status, Type, LastUpdate, ResponseTime, TypeANDName, NextScan) " & _
                  " VALUES ('" & myStatusCode & "', '" & .Category & "', '" & .Description & "', '" & .Location & "', '" & .Name & "', '" & .Status & "',  " & _
                  "'" & .DeviceType & "', '" & Now & "', '" & .ResponseTime & "' , '" & .Name & "-" & .ServerType & "', '" & .NextScan & "' )" & _
                  "END"

                strSqlUpdate = "UPDATE Status SET NextScan = '" & .NextScan & "' WHERE TypeANDName = '" & .Name & "-" & .ServerType & "'"
                updatedef = repo.Updater _
                        .Set(Function(i) i.DeviceName, .Name) _
                        .[Set](Function(i) i.CurrentStatus, .StatusCode) _
                        .[Set](Function(i) i.StatusCode, .StatusCode) _
                        .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                        .[Set](Function(i) i.TypeAndName, TypeAndName) _
                        .[Set](Function(i) i.Description, .Description) _
                        .[Set](Function(i) i.DeviceType, .ServerType) _
                        .[Set](Function(i) i.Location, .Location) _
                        .[Set](Function(i) i.ResponseTime, Integer.Parse(.ResponseTime)) _
                        .[Set](Function(i) i.Category, .Category) _
                        .[Set](Function(i) i.NextScan, .NextScan)
            End With
            If MyIBMConnectServer.Enabled = True Then
                'UpdateStatusTable(strSqlUpdate, strSQL, MyIBMConnectServer.Name)
                repo.Upsert(filterdef, updatedef)
            End If

        Next n

        n = Nothing
    End Sub


    'Private Sub ClearExistingNotesMailProbeHistory()
    '    WriteAuditEntry(Now.ToString & " Clearing history table for NotesMail Probes.")
    '    'Now delete the existing Probe records 

    '    Dim strSQL As String = "DELETE FROM NotesMailProbeHistory"
    '    Dim objVSAdaptor As New VSAdaptor
    '    Try
    '        objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "servers", strSQL)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " Error with Status Table while Clearing NotesMail Probe history:" & ex.Message)
    '    End Try
    '    'Dim myConnection As New OleDb.OleDbConnection


    '    'myConnection.ConnectionString = Me.OleDbConnectionServers.ConnectionString
    '    'myConnection.Open()
    '    'Dim myCommand As New OleDb.OleDbCommand(strSQL, myConnection)

    '    'Try
    '    '    myCommand.ExecuteNonQuery()
    '    '    myCommand.Dispose()
    '    'Catch ex As Exception
    '    '    WriteAuditEntry(Now.ToString & " Error with Status Table while Clearing NotesMail Probe history:" & ex.Message)
    '    'End Try
    '    'myConnection.Close()

    '    'strSQL = Nothing

    '    'myConnection.Dispose()

    'End Sub



    'Private Sub UpdateBBQStatusTable()

    '    Dim strSQL As String = ""
    '    Dim Percent As Decimal
    '    Dim objVSAdaptor As New VSAdaptor


    '    For Each BB As MonitoredItems.BlackBerryQueue In myBlackBerryQueues
    '        Try
    '            With BB
    '                Percent = BB.PendingMail / BB.PendingThreshold

    '                Try
    '                    BB.StatusCode = ServerStatusCode(BB.Status)
    '                Catch ex As Exception
    '                    BB.StatusCode = vbNull
    '                End Try

    '                strSQL = "Update Status SET Status='" & .Status & _
    '                "', LastUpdate='" & Now & _
    '                "', PendingMail='" & .PendingMail & _
    '                "', PendingThreshold='" & .PendingThreshold & _
    '                 "', NextScan='" & .NextScan & _
    '                "', MyPercent='" & (Percent * 100) & _
    '                "', Description='" & .Description & _
    '                 "', StatusCode='" & .StatusCode & _
    '               "', Details='" & .ResponseDetails & _
    '                "', Name='" & .Name & "' " & _
    '                "  WHERE TypeANDName='" & .Name & "-BBQ' "
    '                'TypeANDName is the key
    '                ' WriteAuditEntry(Now.ToString & " " & strSQL)
    '            End With
    '            'If boolUseSQLServer = True Then
    '            '    ExecuteNonQuerySQL_VitalSigns(strSQL)
    '            'Else
    '            '    myCommand.CommandText = strSQL
    '            '    myCommand.ExecuteNonQuery()
    '            'End If
    '            objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)

    '        Catch ex As Exception
    '            WriteAuditEntry(Now.ToString & " Error while creating BlackBerry SQL statement: " & ex.Message & vbCrLf & strSQL)
    '        End Try

    '    Next

    '    strSQL = Nothing

    'End Sub



#End Region

#Region "MS Access Tables, Columns and Settings"

    Private Sub UpdateStatusTableDesign()

        'Dim myCommand As New OleDb.OleDbCommand
        'Try
        '    myCommand.Connection = OleDbConnectionStatus
        '    OleDbConnectionStatus.Open()

        'Catch ex As Exception

        'End Try

        Dim VSObject As New VSAdaptor
        Dim strSQL As String = ""

        Try
            strSQL = "ALTER TABLE Status ADD COLUMN StatusCode Text(255)"
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'StatusCode' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        Try
            strSQL = "ALTER TABLE Status ADD COLUMN CPUThreshold Text(255)"
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'StatusCode' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try


        Try
            strSQL = "ALTER TABLE Status ADD COLUMN SecondaryRole Text(255)"
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'SecondaryRole' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try


        Try
            WriteAuditEntry(Now.ToString & " Checking design of the Traveler Status Table....")
            'strSQL = "CREATE TABLE Traveler_Status (TravelerVersion char(255), ServerName char(255), Status char(255), Details char(255), Users Integer, HTTP_MaxSessions Integer, HTTP_PeakSessions Integer, IncrementalSyncs Integer, ID COUNTER)"
            strSQL = "CREATE TABLE Traveler_Status (TravelerVersion char(255), ServerName char(255), Status char(255), Details char(255), Users Integer, HTTP_MaxSessions Integer, HTTP_PeakSessions Integer, IncrementalSyncs Integer, ID COUNTER)"
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            ' myCommand.ExecuteNonQuery()
            WriteAuditEntry(Now.ToString & "  Added the 'Traveler Status' Table to the Servers.mdb")

        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then
                '            WriteAuditEntry(Now.ToString & " Information Only: Servers.MDB already has the 'DominoDiskSpace' table.")
            Else
                '        WriteAuditEntry(Now.ToString & " Error creating Traveler Table: " & ex.Message)
            End If
        End Try

        Try
            WriteAuditEntry(Now.ToString & " Checking design of the Traveler Device Table....")
            'strSQL = "CREATE TABLE Traveler_Devices (Approval char(255), DeviceID char(255), UserName char(255), DeviceName char(255), ConnectionState char(255), LastSyncTime char(255), OS_Type char(255), Client_Build char(255), NotificationType char(255), ID COUNTER)"
            ' myCommand.ExecuteNonQuery()
            WriteAuditEntry(Now.ToString & "  Added the 'Traveler Devices' Table to the Servers.mdb")
            strSQL = "CREATE TABLE Traveler_Devices (Approval char(255), DeviceID char(255), UserName char(255), DeviceName char(255), ConnectionState char(255), LastSyncTime char(255), OS_Type char(255), Client_Build char(255), NotificationType char(255), ID COUNTER)"
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)

        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then
                '            WriteAuditEntry(Now.ToString & " Information Only: Servers.MDB already has the 'DominoDiskSpace' table.")
            Else
                '        WriteAuditEntry(Now.ToString & " Error creating Traveler Table: " & ex.Message)
            End If
        End Try

        strSQL = "ALTER TABLE Traveler_Devices ADD COLUMN Approval Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        strSQL = "ALTER TABLE Traveler_Devices ADD COLUMN ServerName Text(255)"
        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        strSQL = "ALTER TABLE Traveler_Devices ADD COLUMN DocID Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        strSQL = "ALTER TABLE Traveler_Devices ADD COLUMN device_type Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        strSQL = "ALTER TABLE Traveler_Devices ADD COLUMN Access Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        strSQL = "ALTER TABLE Traveler_Devices ADD COLUMN Security_Policy Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        strSQL = "ALTER TABLE Traveler_Devices ADD COLUMN Approval Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try
        strSQL = "ALTER TABLE Traveler_Devices ADD COLUMN wipeRequested Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try


        strSQL = "ALTER TABLE Traveler_Devices ADD COLUMN wipeOptions Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        strSQL = "ALTER TABLE Traveler_Devices ADD COLUMN wipeStatus Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        strSQL = "ALTER TABLE Traveler_Devices ADD COLUMN DeviceID Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        'SyncType
        strSQL = "ALTER TABLE Traveler_Devices ADD COLUMN SyncType Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try
        'TravelerVersion
        strSQL = "ALTER TABLE Traveler_Status ADD COLUMN TravelerVersion Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try
        'HTTP_Status
        strSQL = "ALTER TABLE Traveler_Status ADD COLUMN HTTP_Status Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        'HTTP_Details
        strSQL = "ALTER TABLE Traveler_Status ADD COLUMN HTTP_Details Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try
        'MaxConfiguredConnections
        strSQL = "ALTER TABLE Traveler_Status ADD COLUMN HTTP_MaxConfiguredConnections Integer"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        'HTTP_PeakConnections
        strSQL = "ALTER TABLE Traveler_Status ADD COLUMN HTTP_PeakConnections Integer"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try


        strSQL = "CREATE TABLE DominoDiskSpace (ServerName char(250), DiskName char(250), DiskFree DOUBLE, DiskSize DOUBLE, PercentFree DOUBLE,  PercentUtilization DOUBLE, Analysis char(250) AverageQueueLength INT, Updated DATE, ID COUNTER )"
        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        strSQL = "Delete * From DominoDiskSpace "
        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try



        strSQL = "Delete * From DominoClusterHealth "

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        strSQL = "CREATE TABLE DominoClusterHealth (ServerName char(250), ClusterName char(250),  SecondsOnQueue INT, SecondsOnQueueMax INT, SecondsOnQueueAvg DOUBLE, LastUpdate DATE, WorkQueueDepth INT, WorkQueueDepthMax INT, WorkQueueDepthAvg DOUBLE, Availability INT, AvailabilityThreshold INT, ID COUNTER )"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        strSQL = "ALTER TABLE DominoClusterHealth ADD COLUMN Analysis Text(255)"

        Try
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'Approval' column to the Status Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

        Try
            strSQL = "ALTER TABLE URLs ADD FailureThreshold INT"
            VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            WriteAuditEntry(Now.ToString & "  Added the 'FailureThreshold' column to the URLs Table.")
        Catch ex As Exception
            If InStr(ex.Message, "exists") > 0 Then

            End If
        End Try

    End Sub

    Private Function ExtractFileName(ByVal FullPath As String) As String
        Dim iPos As Integer
        ExtractFileName = FullPath
        iPos = InStrRev(FullPath, "\")
        If iPos > 0 Then
            ExtractFileName = Mid$(FullPath, iPos + 1)
        End If
    End Function

    Private Function ExtractPath(ByVal FullPath As String) As String
        Dim iPos As Integer
        ExtractPath = FullPath
        iPos = InStrRev(FullPath, "\")
        If iPos > 0 Then
            ExtractPath = Left$(FullPath, iPos - 1)
        End If
    End Function



#End Region

#End Region

#Region "Mail Services"

    Private Sub UpdateMailServicesStatisticsTable(ByVal DeviceName As String, ByVal ResponseTime As Long)
        WriteAuditEntry(Now.ToString & " Updating Mail Service statistics table")
        'Dim myConnection As New Data.OleDb.OleDbConnection
        'myConnection.ConnectionString = Me.OleDbConnectionStatistics.ConnectionString
        '' WriteAuditEntry(Now.ToString & " myConnection string is " & myConnection.ConnectionString)

        'If boolUseSQLServer = False Then
        '    Do While myConnection.State <> ConnectionState.Open
        '        myConnection.Open()
        '    Loop
        'End If

        ' WriteAuditEntry(Now.ToString & " myConnection state is " & myConnection.State.ToString)
        'Dim myCommand As New OleDb.OleDbCommand
        'myCommand.Connection = myConnection


        Dim strSQL As String
        Dim MyWeekNumber As Integer
        MyWeekNumber = GetWeekNumber(Date.Today)
        Dim objVSAdaptor As New VSAdaptor
        Try
            strSQL = "INSERT INTO DeviceDailyStats (DeviceType, ServerName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
             " VALUES ('Mail Service', '" & DeviceName & "', '" & Now.ToString & "', '" & "ResponseTime" & "', '" & ResponseTime & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "')"

            'If boolUseSQLServer = True Then
            '    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
            'Else
            '    myCommand.CommandText = strSQL
            '    myCommand.ExecuteNonQuery()
            'End If
            objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Mail Service Stats table insert failed because: " & ex.Message)
            WriteAuditEntry(Now.ToString & " The failed stats table insert command was " & strSQL)
        Finally
            strSQL = Nothing
            MyWeekNumber = Nothing
            'If boolUseSQLServer = False Then
            '    myConnection.Close()
            'End If

            'myConnection.Dispose()
            'myCommand.Dispose()
            GC.Collect()
        End Try


    End Sub

#End Region


#Region "Network Devices"
    Private Sub UpdateNDStatisticsTable(ByVal DeviceName As String, ByVal ResponseTime As Long, ByVal DeviceId As String)

        Dim objVSAdaptor As New VSAdaptor

        Dim strSQL As String
        Dim MyWeekNumber As Integer
        MyWeekNumber = GetWeekNumber(Date.Today)
        Try
            strSQL = "INSERT INTO DeviceDailyStats (DeviceType, ServerName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" &
               " VALUES ('Network Device', '" & DeviceName & "', '" & Now.ToString & "', '" & "ResponseTime" & "', '" & ResponseTime & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "')"
            'If boolUseSQLServer = True Then
            '    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
            'Else
            '    myCommand.CommandText = strSQL
            '    myCommand.ExecuteNonQuery()
            'End If
            Dim DailyStats As New DailyStatistics
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
            DailyStats.DeviceId = DeviceId
            DailyStats.DeviceType = "Network Device"
            DailyStats.DeviceName = DeviceName
            DailyStats.StatName = "ResponseTime"
            DailyStats.StatValue = ResponseTime
            repo.Insert(DailyStats)
            ' objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Network Device Stats table insert failed becase: " & ex.Message)
            WriteAuditEntry(Now.ToString & " The failed stats table insert comand was " & strSQL)
        Finally
            'myConnection.Close()

        End Try

        'myConnection.Dispose()
        'myCommand.Dispose()
    End Sub

#End Region


#Region "Sametime"

    Private Sub InsertSametimeResponseTime(ByVal ServerName As String, ByVal ResponseTime As Long, ByVal DeviceId As String)

        Dim objVSAdaptor As New VSAdaptor

        Dim strSQL As String
        Dim MyWeekNumber As Integer
        MyWeekNumber = GetWeekNumber(Date.Today)
        Try
            strSQL = "INSERT INTO SametimeDailyStats ( ServerName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber, HourNumber)" &
               " VALUES ('" & ServerName & "', '" & FixDate(Now) & " " & Now.ToShortTimeString & "', '" & "ResponseTime" & "', '" & ResponseTime & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "' , '" & Now.Hour & "')"

            Dim DailyStats As New DailyStatistics
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
            DailyStats.DeviceType = VSNext.Mongo.Entities.Enums.ServerType.Sametime.ToDescription()
            DailyStats.DeviceName = ServerName
            DailyStats.StatName = "ResponseTime"
            DailyStats.StatValue = ResponseTime
            DailyStats.DeviceId = DeviceId
            repo.Insert(DailyStats)

            WriteDeviceHistoryEntry("Sametime", ServerName, Now.ToString & "  " & strSQL)
            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Sametime Stats table insert failed because: " & ex.Message)
            WriteAuditEntry(Now.ToString & " The failed stats table insert command was " & strSQL)
        Finally

        End Try


    End Sub


    Private Sub UpdateSametimeStatusTable(ByRef MySametimeServer As MonitoredItems.SametimeServer)
        '*************************************************************
        'Update the Status Table
        '*************************************************************
        Dim strSQL As String = ""
        Dim StatusDetails As String = ""

        Try
            If MyLogLevel = LogLevel.Verbose Then
                WriteDeviceHistoryEntry("Sametime", MySametimeServer.Name, Now.ToString & "  Entered UpdateSTStatusTable for " & MySametimeServer.Name)
                '        WriteDeviceHistoryEntry("Sametime", MySametimeServer.Name, Now.ToString & " Status Details for " & MySametimeServer.Name & " are " & MySametimeServer.ResponseDetails)
            End If
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error logging StatusDetails String: " & ex.Message, LogLevel.Normal)
        End Try

        Try
            StatusDetails = "Chats: " & MySametimeServer.Chat_Sessions & vbCrLf & " n-Way Chats: " & MySametimeServer.nWay_Chat_Sessions & vbCrLf & " Users: " & MySametimeServer.Users
        Catch ex As Exception

        End Try
        Try
            If StatusDetails.Length > 254 Then
                StatusDetails = Left(MySametimeServer.ResponseDetails, 254)
            End If
        Catch ex As Exception
            StatusDetails = "Status Details exceed allowable field length.  Try again later.."
        End Try

        Try
            If InStr(StatusDetails, "'") > 0 Then
                StatusDetails = StatusDetails.Replace("'", "")
            End If

            Dim Quote As Char
            Quote = Chr(34)

            If InStr(StatusDetails, Quote) > 0 Then
                StatusDetails = StatusDetails.Replace(Quote, "~")
            End If

        Catch ex As Exception
            StatusDetails = "Chats: " & MySametimeServer.Chat_Sessions & vbCrLf & " n-Way Chats: " & MySametimeServer.nWay_Chat_Sessions & vbCrLf & " Users: " & MySametimeServer.Users
        End Try

        If MySametimeServer.Status = "Disabled" Then
            StatusDetails = "This server is not enabled for monitoring."
        End If

        If MySametimeServer.Status = "Maintenance" Then
            StatusDetails = "This Sametime server is in a scheduled maintenance period.  Monitoring is temporarily disabled."
        End If

        If MySametimeServer.Status = "Not Scanned" Then
            StatusDetails = "This Sametime server has not been scanned yet."
        End If

        '1/21/2016 NS added for VSPLUS-2542
        If MySametimeServer.Status = "Login Failure" Then
            StatusDetails = "Failed to login as a Sametime User.  Either the login service is down or the server is down."
        End If

        If MyLogLevel = LogLevel.Verbose Then
            '  WriteDeviceHistoryEntry("Sametime", MySametimeServer.Name, Now.ToString & " %% Entered UpdateSTStatusTable for " & MySametimeServer.Name)
            WriteDeviceHistoryEntry("Sametime", MySametimeServer.Name, Now.ToString & " Status Details for " & MySametimeServer.Name & " are " & StatusDetails)
        End If

        Try
            MySametimeServer.StatusCode = ServerStatusCode(MySametimeServer.Status)
        Catch ex As Exception
            MySametimeServer.StatusCode = vbNull
        End Try

        Try
            'Update the status table
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim TypeAndName As String = MySametimeServer.Name & "-Sametime"
            Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repo.Filter.Where(Function(i) i.TypeAndName.Equals(TypeAndName))
            Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)

            With MySametimeServer


                updatedef = repo.Updater _
                                    .Set(Function(i) i.DeviceName, .Name) _
                                    .[Set](Function(i) i.CurrentStatus, .Status) _
                                    .[Set](Function(i) i.StatusCode, .StatusCode) _
                                    .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                                    .[Set](Function(i) i.TypeAndName, TypeAndName) _
                                    .[Set](Function(i) i.Description, .Description) _
                                    .[Set](Function(i) i.Details, StatusDetails) _
                                    .[Set](Function(i) i.Category, .Category) _
                                    .[Set](Function(i) i.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Sametime.ToDescription()) _
                                    .[Set](Function(i) i.DownCount, Integer.Parse(.DownCount)) _
                                    .[Set](Function(i) i.Location, .Location) _
                                    .[Set](Function(i) i.UpCount, Integer.Parse(.UpCount)) _
                                    .[Set](Function(i) i.UpPercent, Double.Parse(.UpPercentCount)) _
                                    .[Set](Function(i) i.LastUpdated, Now) _
                                    .[Set](Function(i) i.ResponseTime, Integer.Parse(.ResponseTime)) _
                                    .[Set](Function(i) i.PendingMail, Integer.Parse(.Chat_Sessions)) _
                                    .[Set](Function(i) i.NextScan, .NextScan) _
                                    .[Set](Function(i) i.UpMinutes, Double.Parse(Microsoft.VisualBasic.Strings.Format(.UpMinutes, "F1"))) _
                                    .[Set](Function(i) i.DownMinutes, Double.Parse(Microsoft.VisualBasic.Strings.Format(.DownMinutes, "F1"))) _
                                    .[Set](Function(i) i.DominoVersion, "Current Chats: " & .Chat_Sessions) _
                                    .[Set](Function(i) i.OperatingSystem, "IBM Sametime server") _
                                    .[Set](Function(i) i.ResponseThreshold, Integer.Parse(.ResponseThreshold)) _
                                    .[Set](Function(i) i.DeviceId, .ServerObjectID)
            End With

            strSQL = strSQL.Replace("NaN", "0")
            strSQL = strSQL.Replace("' ", "'")

            repo.Upsert(filterdef, updatedef)

            WriteDeviceHistoryEntry("Sametime", MySametimeServer.Name, Now.ToString & " Sametime module  SQL statement: " & strSQL)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in Sametime module creating SQL statement for status table: " & ex.Message)
            WriteDeviceHistoryEntry("Sametime", MySametimeServer.Name, Now.ToString & " Error in Sametime module creating SQL statement for status table: " & ex.Message)
        End Try


        'Try
        '    '**
        '    Dim myPath As String
        '    Dim myRegistry As New RegistryHandler


        '    myRegistry = Nothing

        '    Dim objVSAdaptor As New VSAdaptor
        '    If objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL) = False And MySametimeServer.Enabled = True Then
        '        With MySametimeServer
        '            strSQL = "INSERT INTO Status (StatusCode, Category,  Description, DownCount,  Location, Name,  Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime, TypeANDName, Icon,  MyPercent, NextScan,  UpMinutes, DownMinutes, PendingThreshold, DeadThreshold) " & _
        '            " VALUES ('" & .StatusCode & "', '" & .Category & "', '" & .Description & "', '" & .DownCount & "', '" & .Location & "', '" & .Name & "', '" & .Status & "',  " & _
        '            "'Sametime', '" & .UpCount & "', '" & .UpPercentCount & "', '" & Now & "', '" & .ResponseTime & "' , '" & .Name & "-Sametime', " & IconList.Sametime & ", '" & Percent & "', '" & .NextScan & "', " & Microsoft.VisualBasic.Strings.Format(.UpMinutes, "F1") & ", " & Microsoft.VisualBasic.Strings.Format(.DownMinutes, "F1") & "', " & .Chat_Sessions_Threshold & ", " & .nWay_Chat_Sessions_Threshold & ")"
        '        End With
        '        objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
        '    End If

        'Catch ex As Exception
        '    WriteAuditEntry(Now.ToString & " Error updating Status table with Sametime server info: " & ex.Message & vbCrLf & "SQL Statement: " & strSQL, LogLevel.Normal)
        'Finally
        '    '  MyDominoServer.LastScan = Now.ToString
        '    '   If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry("Updated  Domino Status Table with " & strSQL)
        '    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry("Next scan scheduled : " & MyDominoServer.NextScan)
        'End Try

    End Sub

    Private Sub UpdateSametimeStatTable(ByVal ServerName As String, ByVal StatName As String, ByVal StatValue As Double, ByVal DeviceId As String)

        WriteDeviceHistoryEntry("Sametime", ServerName, Now.ToString & " Updating Sametime Statistics table")

        'Dim strSQL As String
        'Dim MyWeekNumber As Integer
        'MyWeekNumber = GetWeekNumber(Date.Today)
        'strSQL = "INSERT INTO SametimeDailyStats (ServerName, [Date], StatName, StatValue,  WeekNumber, MonthNumber, YearNumber, DayNumber, HourNumber)" &
        '    " VALUES ('" & ServerName & "', '" & Now.ToString & "', '" & StatName & "', '" & StatValue & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "', '" & Now.Hour & "')"

        Dim DailyStats As New DailyStatistics
        Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
        DailyStats.DeviceType = VSNext.Mongo.Entities.Enums.ServerType.Sametime.ToDescription()
        DailyStats.DeviceName = ServerName
        DailyStats.StatName = StatName
        DailyStats.StatValue = StatValue
        DailyStats.DeviceId = DeviceId
        repo.Insert(DailyStats)
        Dim objVSAdaptor As New VSAdaptor
        'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)


        'strSQL = Nothing
        'MyWeekNumber = Nothing
    End Sub

    Private Sub UpdateAdvancedSametimeStatTable(ByRef Server As MonitoredItems.SametimeServer)
        Try
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Updating Sametime Advanced Statistics table")
        Catch ex As Exception

        End Try

        'Dim strSQL As String
        'Dim MyWeekNumber As Integer
        'MyWeekNumber = GetWeekNumber(Date.Today)

        'Dim objVSAdaptor As New VSAdaptor

        For Each Stat As MonitoredItems.SametimeStatistic In Server.StatisticsCollection
            Try
                '    strSQL = "INSERT INTO SametimeDailyStats (ServerName, [Date], StatName, StatValue,  WeekNumber, MonthNumber, YearNumber, DayNumber, HourNumber)" & _
                '" VALUES ('" & Server.Name & "', '" & Now.ToString & "', '" & Stat.Name & "', '" & Stat.Value & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "', '" & Now.Hour & "')"
                Dim translatedStatName As String = ""
                Select Case Stat.Name

                    Case "ConcurrentLogins"
                        translatedStatName = "Users"
                    Case "NWs"
                        translatedStatName = "Numberofactivenwaychats"
                    Case "TotalNWChats"
                        translatedStatName = "Numberofnwaychats"
                    Case "IMs"
                        translatedStatName = "Numberofopenchatsessions"
                    Case "AvgConcurrentLogins"
                        translatedStatName = "AvgConcurrentLogins"
                    Case "TotalTwoWayChats"
                        translatedStatName = "TotalTwoWayChats"
                    Case "TotalUniqueLogins"
                        translatedStatName = "TotalUniqueLogins"
                    Case "ConcurrentLoggedInUsers"
                        translatedStatName = "ConcurrentLoggedInUsers"
                        '6/26/2015 NS added for VSPLUS-1823
                    Case "MaxConcurrentLogins"
                        translatedStatName = "MaxConcurrentLogins"
                    Case "MaxConcurrentLoggedInUsers"
                        translatedStatName = "MaxConcurrentLoggedInUsers"
                    Case "Countofallcalls"
                        translatedStatName = "Countofallcalls"
                    Case "Countofallusers"
                        translatedStatName = "Countofallusers"
                    Case "Countofall1x1calls"
                        translatedStatName = "Countofall1x1calls"
                    Case "Countofall1x1users"
                        translatedStatName = "Countofall1x1users"
                    Case "Totalcountofall1x1calls"
                        translatedStatName = "Totalcountofall1x1calls"
                    Case "Totalcountofallcalls"
                        translatedStatName = "Totalcountofallcalls"
                    Case "Totalcountofallmultiusercalls"
                        translatedStatName = "Totalcountofallmultiusercalls"
                    Case "Countofallmultiusercalls"
                        translatedStatName = "Countofallmultiusercalls"
                    Case "Countofallmultiuserusers"
                        translatedStatName = "Countofallmultiuserusers"
                    Case "Numberofactivemeetings"
                        translatedStatName = "Numberofactivemeetings"
                    Case "Currentnumberofusersinsidemeetings"
                        translatedStatName = "Currentnumberofusersinsidemeetings"

                End Select
                WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "Stat Name:" + Stat.Name)

                '5/5/2016 NS modified - if stat.Name is empty, it causes a SQL error
                If translatedStatName <> "" Then
                    'strSQL = "INSERT INTO SametimeDailyStats (ServerName, [Date], StatName, StatValue,  WeekNumber, MonthNumber, YearNumber, DayNumber, HourNumber)" & _
                    '    " VALUES ('" & Server.Name & "', '" & Now.ToString & "', '" + translatedStatName + "', '" & Stat.Value & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "', '" & Now.Hour & "')"
                    'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)

                    Dim DailyStats As New DailyStatistics
                    Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
                    DailyStats.DeviceType = VSNext.Mongo.Entities.Enums.ServerType.Sametime.ToDescription()
                    DailyStats.DeviceName = Server.Name
                    DailyStats.DeviceId = Server.ServerObjectID
                    DailyStats.StatName = translatedStatName
                    DailyStats.StatValue = Double.Parse(Stat.Value)
                    repo.Insert(DailyStats)
                End If

                '		If Stat.Name = "ConcurrentLogins" Then
                '			'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '			strSQL = "INSERT INTO SametimeDailyStats (ServerName, [Date], StatName, StatValue,  WeekNumber, MonthNumber, YearNumber, DayNumber, HourNumber)" & _
                '" VALUES ('" & Server.Name & "', '" & Now.ToString & "', 'Users', '" & Stat.Value & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "', '" & Now.Hour & "')"
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '		End If


                '		If Stat.Name = "NWs" Then
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '			strSQL = "INSERT INTO SametimeDailyStats (ServerName, [Date], StatName, StatValue,  WeekNumber, MonthNumber, YearNumber, DayNumber, HourNumber)" & _
                '" VALUES ('" & Server.Name & "', '" & Now.ToString & "', 'n-Way Chat Sessions', '" & Stat.Value & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "', '" & Now.Hour & "')"
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '		End If

                '		If Stat.Name = "IMs" Then
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '			strSQL = "INSERT INTO SametimeDailyStats (ServerName, [Date], StatName, StatValue,  WeekNumber, MonthNumber, YearNumber, DayNumber, HourNumber)" & _
                '" VALUES ('" & Server.Name & "', '" & Now.ToString & "', 'Chat Sessions', '" & Stat.Value & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "', '" & Now.Hour & "')"
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '		End If

                '		If Stat.Name = "AvgConcurrentLogins" Then
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '			strSQL = "INSERT INTO SametimeDailyStats (ServerName, [Date], StatName, StatValue,  WeekNumber, MonthNumber, YearNumber, DayNumber, HourNumber)" & _
                '" VALUES ('" & Server.Name & "', '" & Now.ToString & "', 'AvgConcurrentLogins', '" & Stat.Value & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "', '" & Now.Hour & "')"
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '		End If


                '		If Stat.Name = "TotalTwoWayChats" Then
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '			strSQL = "INSERT INTO SametimeDailyStats (ServerName, [Date], StatName, StatValue,  WeekNumber, MonthNumber, YearNumber, DayNumber, HourNumber)" & _
                '" VALUES ('" & Server.Name & "', '" & Now.ToString & "', 'TotalTwoWayChats', '" & Stat.Value & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "', '" & Now.Hour & "')"
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '		End If

                '		If Stat.Name = "TotalUniqueLogins" Then
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '			strSQL = "INSERT INTO SametimeDailyStats (ServerName, [Date], StatName, StatValue,  WeekNumber, MonthNumber, YearNumber, DayNumber, HourNumber)" & _
                '" VALUES ('" & Server.Name & "', '" & Now.ToString & "', 'TotalUniqueLogins', '" & Stat.Value & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "', '" & Now.Hour & "')"
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '		End If

                '		If Stat.Name = "ConcurrentLoggedInUsers" Then
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '			strSQL = "INSERT INTO SametimeDailyStats (ServerName, [Date], StatName, StatValue,  WeekNumber, MonthNumber, YearNumber, DayNumber, HourNumber)" & _
                '" VALUES ('" & Server.Name & "', '" & Now.ToString & "', 'ConcurrentLoggedInUsers', '" & Stat.Value & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "', '" & Now.Hour & "')"
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '		End If


                '		If Stat.Name = "MaxConcurrentLoggedInUsers" Then
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '			strSQL = "INSERT INTO SametimeDailyStats (ServerName, [Date], StatName, StatValue,  WeekNumber, MonthNumber, YearNumber, DayNumber, HourNumber)" & _
                '" VALUES ('" & Server.Name & "', '" & Now.ToString & "', 'MaxConcurrentLoggedInUsers', '" & Stat.Value & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "', '" & Now.Hour & "')"
                '			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "SametimeStats", strSQL)
                '		End If



            Catch ex As Exception
                WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Error Updating Sametime Advanced Statistics table: " & ex.ToString)
                '    WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " SQL Statement: " & strSQL)
            End Try
        Next


        'strSQL = Nothing
        'MyWeekNumber = Nothing
    End Sub

    '1/11/2016 NS added for VSPLUS-1921,VSPLUS-1823
    Private Sub UpdateSametimeSummaryTable(ByRef Server As MonitoredItems.SametimeServer)
        Dim strSQL As String
        Dim objVSAdaptor As New VSAdaptor
        Dim myConnectionString As New VSFramework.XMLOperation
        Dim dt As DataTable
        Dim translatedStatName As String = ""
        Dim MyWeekNumber As Integer
        Dim nowDate As DateTime = Now.AddDays(-1)
        MyWeekNumber = GetWeekNumber(Date.Today.AddDays(-1))

        WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Updating Sametime Summary table in VSS_Statistics")
        Try
            'strSQL = "SELECT * FROM SametimeSummaryStats WHERE DATEADD(dd,0,DATEDIFF(dd,0,Date)) < DATEADD(dd,0,DATEDIFF(dd,0,GETDATE())) AND " &
            '    "DATEADD(dd,0,DATEDIFF(dd,0,Date)) >= DATEADD(dd,-1,DATEDIFF(dd,0,GETDATE())) AND ServerName='" & Server.Name & "' "
            'WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "   SQL select: " & strSQL, LogUtilities.LogUtils.LogLevel.Verbose)
            'dt = objVSAdaptor.FetchData(myConnectionString.GetDBConnectionString("VSS_Statistics"), strSQL)
            'If dt.Rows.Count = 0 Then
            '    Try
            '        GetSametimeStatsFromLog(Server)
            '    Catch ex As Exception
            '        WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Error retrieving statistics from Sametime Log:  " & ex.ToString)
            '    End Try
            For Each Stat As MonitoredItems.SametimeStatistic In Server.StatisticsCollection
                Select Case Stat.Name
                    Case "PeakLogins", "Total2WayChats", "TotalnWayChats"
                        Try
                            'strSQL = "INSERT INTO SametimeSummaryStats (ServerName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber) " & _
                            '    "VALUES ('" & Server.Name & "', '" & Date.Today.AddDays(-1) & "', '" & Stat.Name & "', " & Stat.Value & ", " & MyWeekNumber & ", " & nowDate.Month & ", " & nowDate.Year & ", " & nowDate.Day & ")"
                            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "Status", strSQL)
                            'Dim SummaryStats As New SummaryStatistics
                            'Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.SummaryStatistics)(connectionString)
                            'SummaryStats.DeviceId = Server.ServerObjectID
                            'SummaryStats.StatName = Stat.Name
                            'SummaryStats.StatValue = Double.Parse(Stat.Value)
                            'repo.Insert(SummaryStats)
                            Dim repo1 As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.SummaryStatistics)(connectionString)
                            Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.SummaryStatistics)
                            Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.SummaryStatistics)

                            filterdef = repo1.Filter.Where(Function(i) i.CreatedOn.ToShortDateString.Equals(DateTime.UtcNow.ToShortDateString))
                            updatedef = repo1.Updater _
                            .Set(Function(i) i.DeviceId, Server.ServerObjectID) _
                            .Set(Function(i) i.DeviceName, Server.Name) _
                            .[Set](Function(i) i.StatName, Stat.Name) _
                            .[Set](Function(i) i.StatValue, Double.Parse(Stat.Value))

                            repo1.Upsert(filterdef, updatedef)
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Error inserting " & Stat.Name & " into the SametimeSummaryStats table:  " & ex.ToString)
                        End Try
                    Case Else
                        'do nothing
                End Select
            Next
            'End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Error selecting from the SametimeSummaryStats table:  " & ex.ToString)
        End Try
    End Sub
    '1/11/2016 NS added for VSPLUS-1921,VSPLUS-1823
    Private Sub GetSametimeStatsFromLog(ByRef Server As MonitoredItems.SametimeServer)
        Dim s As New Domino.NotesSession
        Dim db As Domino.NotesDatabase

        WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Attempting to get the Sametime stats from stlog.nsf")
        Try
            Dim myPassword = GetNotesPassword()
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Got password ", LogUtilities.LogUtils.LogLevel.Verbose)
            s.Initialize(myPassword)
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Initialized session ", LogUtilities.LogUtils.LogLevel.Verbose)
            db = s.GetDatabase(Server.Name, "stlog.nsf")
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Got database ", LogUtilities.LogUtils.LogLevel.Verbose)
            If Not (db.IsOpen) Then
                WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Trying to open database ", LogUtilities.LogUtils.LogLevel.Verbose)
                db.Open()
            End If
            If db.IsOpen Then
                WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Opened stlog.nsf ", LogUtilities.LogUtils.LogLevel.Verbose)
                ReadSametimeStatsFromLog(Server, db, "NotesCommunityUsersandLoginsbyDay", "PeakLogins")
                ReadSametimeStatsFromLog(Server, db, "NotesCommunityChatsandPlacesbyDay", "Total2WayChats")
                ReadSametimeStatsFromLog(Server, db, "NotesCommunityChatsandPlacesbyDay", "TotalnWayChats")
            End If
            System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(s)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Error accessing server " & Server.Name & " : " & ex.ToString)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(s)
        End Try
    End Sub
    '1/11/2016 NS added for VSPLUS-1921,VSPLUS-1823
    Private Sub ReadSametimeStatsFromLog(ByRef Server As MonitoredItems.SametimeServer, ByRef db As Domino.NotesDatabase, ByVal viewName As String, ByVal statName As String)
        Dim view As Domino.NotesView
        Dim doc As Domino.NotesDocument
        Dim stat As New MonitoredItems.SametimeStatistic

        Try
            view = db.GetView(viewName)
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Got a handle on the view " & viewName, LogUtilities.LogUtils.LogLevel.Verbose)
            doc = view.GetFirstDocument()
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Document creation date is " & CDate(doc.Created.ToString()).ToShortDateString & ", today's date is " & Date.Today)
            If CDate(doc.Created.ToString()).ToShortDateString = Date.Today.ToShortDateString Then
                stat.Name = statName
                stat.Value = Convert.ToInt32(doc.GetItemValue(statName)(0))
                Server.StatisticsCollection.Add(stat)
            End If
            System.Runtime.InteropServices.Marshal.ReleaseComObject(view)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(doc)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Error extracting stat " & statName & " from the server " & Server.Name & " : " & ex.ToString)
        End Try
    End Sub

    Public Function GetNotesPassword() As String
        'Get the passwords from the settings table
        Dim MyPass As Object
        Dim Password As String

        Try
            Dim myAdapter As New VSFramework.XMLOperation
            MyPass = myAdapter.ReadSettingsSQL("Password")
            WriteAuditEntry(Now.ToString & " Password type is " & MyPass.GetType.ToString)
            'WriteAuditEntry(Now.ToString & " Raw password is " & MyPass.ToString)
        Catch ex As Exception
            MyPass = Nothing
        End Try

        Dim mySecrets As New VSFramework.TripleDES
        Try
            If Not MyPass Is Nothing Then
                Password = mySecrets.Decrypt(MyPass) 'password in clear text, stored in memory now
                ' If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Successfully decrypted the Notes password as " & MyDominoPassword)
            Else
                Password = Nothing
            End If
        Catch ex As Exception
            Password = ""
            WriteAuditEntry(Now.ToString & " Error decrypting the Notes password.  " & ex.ToString)
        End Try

        Return Password
    End Function
#End Region

#Region "URLs"

    Private Sub UpdateURLStatisticsTable(ByVal DeviceName As String, ByVal ResponseTime As Long, ByVal DeviceId As String)
        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("URL", DeviceName, Now.ToString & " Updating URL statistics table")
        'Dim myConnection As New Data.OleDb.OleDbConnection
        'myConnection.ConnectionString = Me.OleDbConnectionStatistics.ConnectionString
        '' WriteAuditEntry(Now.ToString & " myConnection string is " & myConnection.ConnectionString)

        'If boolUseSQLServer = False Then
        '    Do While myConnection.State <> ConnectionState.Open
        '        myConnection.Open()
        '    Loop
        'End If

        '' WriteAuditEntry(Now.ToString & " myConnection state is " & myConnection.State.ToString)
        'Dim myCommand As New OleDb.OleDbCommand
        'myCommand.Connection = myConnection


        Dim strSQL As String
        Dim MyWeekNumber As Integer
        MyWeekNumber = GetWeekNumber(Date.Today)
        Dim objVSAdaptor As New VSAdaptor
        Try
            'strSQL = "INSERT INTO DeviceDailyStats (DeviceType, ServerName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
            ' " VALUES ('URL', '" & DeviceName & "', '" & Now.ToString & "', '" & "ResponseTime" & "', '" & ResponseTime & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "')"


            'If boolUseSQLServer = True Then
            '    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
            'Else
            '    myCommand.CommandText = strSQL
            '    myCommand.ExecuteNonQuery()
            'End If
            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
            Dim DailyStats As New DailyStatistics
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
            DailyStats.DeviceType = VSNext.Mongo.Entities.Enums.ServerType.URL.ToDescription()
            DailyStats.DeviceId = DeviceId
            DailyStats.DeviceName = DeviceName
            DailyStats.StatName = "ResponseTime"
            DailyStats.StatValue = ResponseTime
            repo.Insert(DailyStats)
        Catch ex As Exception
            WriteDeviceHistoryEntry("URL", DeviceName, Now.ToString & " URL Stats table insert failed because: " & ex.Message)
            WriteDeviceHistoryEntry("URL", DeviceName, Now.ToString & " The failed stats table insert command was " & strSQL)
        Finally
            'If boolUseSQLServer = False Then
            '    myConnection.Close()
            'End If

            'myConnection.Dispose()
            'myCommand.Dispose()
            GC.Collect()
        End Try
    End Sub

    Private Sub UpdateStatusTableWithURLs()
        WriteAuditEntry(Now.ToString & " Updating status table with URLs.")
        'Now delete the existing URL records 

        Dim strSQL As String

        Dim objVSAdaptor As New VSAdaptor
        Dim n As Integer
        Dim MyURL As MonitoredItems.URL
        Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
        Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status)
        Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)
        For n = 0 To MyURLs.Count - 1
            strSQL = ""
            MyURL = MyURLs.Item(n)
            WriteAuditEntry(Now.ToString & " Configuring " & MyURL.Name)
            Try
                With MyURL
                    '5/2/2016 NS modified for VSPLUS-2887
                    strSQL = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + .Name + "-URL') BEGIN " & _
                     "INSERT INTO Status (StatusCode, Category,  Description, Details, DownCount,  Location, Name,  Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime,  TypeANDName, Icon) " & _
                     " VALUES ('Not Scanned', '" & .Category & "', 'Not Scanned', 'This URL has not yet been scanned.', '" & .DownCount & "', '" & .Location & "', '" & .Name & "', '" & .Status & "', 'URL', '" & .UpCount & "', '" & .UpPercentCount & "', '" & FixDateTime(Now) & "', '0', '" & .Name & "-URL', " & IconList.URL & ")" & _
                     "END"

                    Dim TypeAndName As String = MyURL.Name & "-" & MyURL.ServerType
                    filterdef = repo.Filter.Where(Function(i) i.TypeAndName.Equals(TypeAndName))
                    updatedef = repo.Updater _
                                              .Set(Function(i) i.DeviceName, .Name) _
                                              .[Set](Function(i) i.CurrentStatus, "Not Scanned") _
                                              .[Set](Function(i) i.StatusCode, "Not Scanned") _
                                              .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                                              .[Set](Function(i) i.Category, .Category) _
                                              .[Set](Function(i) i.TypeAndName, TypeAndName) _
                                              .[Set](Function(i) i.Description, "Not Scanned") _
                                              .[Set](Function(i) i.DeviceType, "URL") _
                                              .[Set](Function(i) i.Location, .Location) _
                                              .[Set](Function(i) i.UpCount, Integer.Parse(.UpCount)) _
                                              .[Set](Function(i) i.UpPercent, Integer.Parse(.UpPercentCount)) _
                                              .[Set](Function(i) i.LastUpdated, Now) _
                                              .[Set](Function(i) i.ResponseTime, 0) _
                                              .[Set](Function(i) i.ResponseThreshold, Integer.Parse(.ResponseThreshold)) _
                                              .[Set](Function(i) i.DeviceId, .ServerObjectID)


                End With

                ' WriteAuditEntry(Now.ToString & " " & strSQL)

                'objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
                repo.Upsert(filterdef, updatedef)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " error Updating status table with URLs. Error:" + ex.Message.ToString())
            End Try

        Next n

        n = Nothing
        strSQL = Nothing
        MyURL = Nothing



    End Sub
#End Region

#Region "Cloud"
    Private Sub UpdateCloudURLStatisticsTable(ByVal DeviceName As String, ByVal ResponseTime As Long, ByVal DeviceId As String)
        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Cloud", DeviceName, Now.ToString & " Updating Cloud statistics table")
        'Dim myConnection As New Data.OleDb.OleDbConnection
        'myConnection.ConnectionString = Me.OleDbConnectionStatistics.ConnectionString
        '' WriteAuditEntry(Now.ToString & " myConnection string is " & myConnection.ConnectionString)

        'If boolUseSQLServer = False Then
        '    Do While myConnection.State <> ConnectionState.Open
        '        myConnection.Open()
        '    Loop
        'End If

        '' WriteAuditEntry(Now.ToString & " myConnection state is " & myConnection.State.ToString)
        'Dim myCommand As New OleDb.OleDbCommand
        'myCommand.Connection = myConnection


        Dim strSQL As String
        Dim MyWeekNumber As Integer
        MyWeekNumber = GetWeekNumber(Date.Today)
        Dim objVSAdaptor As New VSAdaptor
        Try
            strSQL = "INSERT INTO DeviceDailyStats (DeviceType, ServerName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" &
             " VALUES ('Cloud', '" & DeviceName & "', '" & Now.ToString & "', '" & "ResponseTime" & "', '" & ResponseTime & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "')"


            'If boolUseSQLServer = True Then
            '    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
            'Else
            '    myCommand.CommandText = strSQL
            '    myCommand.ExecuteNonQuery()
            'End If
            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
            Dim DailyStats As New DailyStatistics
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
            DailyStats.DeviceId = DeviceId
            DailyStats.DeviceType = "Cloud"
            DailyStats.DeviceName = DeviceName
            DailyStats.StatName = "ResponseTime"
            DailyStats.StatValue = ResponseTime
            repo.Insert(DailyStats)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Cloud", DeviceName, Now.ToString & " Cloud Stats table insert failed because: " & ex.Message)
            WriteDeviceHistoryEntry("Cloud", DeviceName, Now.ToString & " The failed stats table insert command was " & strSQL)
        Finally
            'If boolUseSQLServer = False Then
            '    myConnection.Close()
            'End If

            'myConnection.Dispose()
            'myCommand.Dispose()
            GC.Collect()
        End Try
    End Sub
    Private Sub UpdateStatusTableWithCloudURLs()
        WriteAuditEntry(Now.ToString & " Updating status table with Cloud URLs.")
        'Now delete the existing URL records 

        Dim strSQL As String
        Dim objVSAdaptor As New VSAdaptor

        'Now Update the Data set with the new URLs
        Dim n As Integer
        Dim MyCloud As MonitoredItems.Cloud

        Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
        Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status)
        Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)


        For n = 0 To MyClouds.Count - 1
            strSQL = ""
            MyCloud = MyClouds.Item(n)
            WriteAuditEntry(Now.ToString & " Configuring " & MyCloud.Name)
            With MyCloud
                strSQL = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + .Name + "-Sametime') BEGIN " & _
                 "INSERT INTO Status (StatusCode, Category,  Description, Details, DownCount,  Location, Name,  Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime,  TypeANDName, Icon) " & _
                 " VALUES ('Not Scanned', '" & .Category & "', '" & .CloudURL & "', 'This Cloud URL has not yet been scanned.', '" & .DownCount & "', 'Cloud', '" & .Name & "', '" & .Status & "', 'Cloud', '" & .UpCount & "', '" & .UpPercentCount & "', '" & FixDateTime(Now) & "', '0', '" & .Name & "-Cloud', " & IconList.URL & ")" & _
                 "END"
                Dim TypeAndName As String = .Name & "-Cloud"

                filterdef = repo.Filter.Where(Function(i) i.TypeAndName.Equals(TypeAndName))
                updatedef = repo.Updater _
                                  .Set(Function(i) i.DeviceName, .Name) _
                                  .[Set](Function(i) i.CurrentStatus, "Not Scanned") _
                                  .[Set](Function(i) i.StatusCode, "Not Scanned") _
                                  .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                                  .[Set](Function(i) i.Details, "This Cloud URL has not yet been scanned.") _
                                  .[Set](Function(i) i.Category, .Category) _
                                  .[Set](Function(i) i.TypeAndName, TypeAndName) _
                                  .[Set](Function(i) i.Description, .CloudURL) _
                                  .[Set](Function(i) i.DeviceType, "Cloud") _
                                  .[Set](Function(i) i.Location, "Cloud") _
                                  .[Set](Function(i) i.UpCount, Integer.Parse(.UpCount)) _
                                  .[Set](Function(i) i.UpPercent, Integer.Parse(.UpPercentCount)) _
                                  .[Set](Function(i) i.LastUpdated, Now) _
                                  .[Set](Function(i) i.ResponseTime, 0) _
                                  .[Set](Function(i) i.NextScan, .NextScan)

            End With

            ' WriteAuditEntry(Now.ToString & " " & strSQL)
            Try
                'objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
                repo.Upsert(filterdef, updatedef)

            Catch ex As Exception

            End Try

        Next n




        n = Nothing
        strSQL = Nothing
        MyCloud = Nothing
    End Sub
#End Region

#Region "WebSphere"

    Private Sub UpdateWebSphereStatusTable(ByRef MyWebSphereServer As MonitoredItems.WebSphere)
        '*************************************************************
        'Update the Status Table
        '*************************************************************
        Dim strSQL As String = ""
        Dim StatusDetails As String = ""

        Try
            If MyLogLevel = LogLevel.Verbose Then
                WriteDeviceHistoryEntry("WebSphere", MyWebSphereServer.Name, Now.ToString & "  Entered UpdateWebSphereStatusTable for " & MyWebSphereServer.Name)

            End If
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error logging StatusDetails String: " & ex.Message, LogLevel.Normal)
        End Try

        Try
            StatusDetails = MyWebSphereServer.ResponseDetails & " at " & Now.ToString("t")
        Catch ex As Exception

        End Try
        Try
            If StatusDetails.Length > 254 Then
                StatusDetails = Left(MyWebSphereServer.ResponseDetails, 254)
            End If
        Catch ex As Exception
            StatusDetails = "Status Details exceed allowable field length.  Try again later.."
        End Try

        Try
            If InStr(StatusDetails, "'") > 0 Then
                StatusDetails = StatusDetails.Replace("'", "")
            End If

            Dim Quote As Char
            Quote = Chr(34)

            If InStr(StatusDetails, Quote) > 0 Then
                StatusDetails = StatusDetails.Replace(Quote, "~")
            End If

        Catch ex As Exception
            StatusDetails = ""
        End Try

        If MyWebSphereServer.Status = "Disabled" Then
            StatusDetails = "This server is not enabled for monitoring."
        End If

        If MyWebSphereServer.Status = "Maintenance" Then
            StatusDetails = "This WebSphere server is in a scheduled maintenance period.  Monitoring is temporarily disabled."
        End If

        If MyWebSphereServer.Status = "Not Scanned" Then
            StatusDetails = "This WebSphere server has not been scanned yet."
        End If

        If MyLogLevel = LogLevel.Verbose Then
            WriteDeviceHistoryEntry("WebSphere", MyWebSphereServer.Name, Now.ToString & " Status Details for " & MyWebSphereServer.Name & " are " & StatusDetails)
        End If


        Try
            MyWebSphereServer.StatusCode = ServerStatusCode(MyWebSphereServer.Status)
        Catch ex As Exception
            MyWebSphereServer.StatusCode = vbNull
        End Try

        Try
            'Update the status table
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status)
            Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)
            With MyWebSphereServer
                'strSQL = "Update Status SET " & _
                '" Status='" & .Status & "'" & _
                '", Details='" & StatusDetails & _
                ' "', Description='" & .Description & _
                ' "', StatusCode='" & .StatusCode & _
                '"', LastUpdate='" & Now & _
                '"', Category='" & .Category & _
                '"', ResponseTime='" & Str(.ResponseTime) & _
                '"', ResponseThreshold='" & .ResponseThreshold & _
                '"', NextScan='" & .NextScan & _
                '"', Name='" & .Name & _
                '"', CPU='" & .CPU_Utilization & _
                '"', Memory='" & .Memory_Utilization & _
                '"' WHERE TypeANDName='" & .Name & "-" & .ServerType & "'"

                Dim TypeAndName As String = .Name & "-" & .ServerType

                filterdef = repo.Filter.Where(Function(i) i.TypeAndName.Equals(TypeAndName))
                updatedef = repo.Updater _
                                  .Set(Function(i) i.DeviceName, .Name) _
                                  .[Set](Function(i) i.CurrentStatus, .Status) _
                                  .[Set](Function(i) i.StatusCode, .Status) _
                                  .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                                  .[Set](Function(i) i.Details, StatusDetails) _
                                  .[Set](Function(i) i.Category, .Category) _
                                  .[Set](Function(i) i.TypeAndName, TypeAndName) _
                                  .[Set](Function(i) i.Description, .Description) _
                                  .[Set](Function(i) i.DeviceType, .ServerType) _
                                  .[Set](Function(i) i.LastUpdated, Now) _
                                  .[Set](Function(i) i.ResponseTime, Integer.Parse(.ResponseTime)) _
                                  .[Set](Function(i) i.ResponseThreshold, Integer.Parse(.ResponseThreshold)) _
                                  .[Set](Function(i) i.NextScan, .NextScan) _
                                  .[Set](Function(i) i.DeviceId, .ServerObjectID) _
                                  .[Set](Function(i) i.ProcessId, .ProcessId) _
                                  .[Set](Function(i) i.UpMinutes, New TimeSpan(0, 0, .UpTime).TotalMinutes)

            End With
            repo.Upsert(filterdef, updatedef)
            'strSQL = strSQL.Replace("NaN", "0")
            'strSQL = strSQL.Replace("' ", "'")



            WriteDeviceHistoryEntry("WebSphere", MyWebSphereServer.Name, Now.ToString & " WebSphere module  SQL statement: " & strSQL)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in WebSphere module creating SQL statement for status table: " & ex.Message)
            WriteDeviceHistoryEntry("WebSphere", MyWebSphereServer.Name, Now.ToString & " Error in WebSphere module creating SQL statement for status table: " & ex.Message)
        End Try


        'Try
        '    '**
        '    Dim myPath As String
        '    Dim myRegistry As New RegistryHandler


        '    myRegistry = Nothing

        '    Dim objVSAdaptor As New VSAdaptor
        '    If objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL) = False And MyWebSphereServer.Enabled = True Then
        '        Dim strSQLUpdate As String = strSQL
        '        With MyWebSphereServer
        '            strSQL = "INSERT INTO Status (StatusCode, Category,  Description, Location, Name,  Status, Type, LastUpdate, ResponseTime, TypeANDName, NextScan, Details, ResponseThreshold) " & _
        '            " VALUES ('" & .StatusCode & "', '" & .Category & "', '" & .Description & "', '" & .Location & "', '" & .Name & "', '" & .Status & "',  " & _
        '            "'" & .ServerType & "', '" & Now & "', '" & .ResponseTime & "' , '" & .Name & "-" & .ServerType & "', '" & .NextScan & "', '" & StatusDetails & "', '" & .ResponseThreshold & "' )"
        '        End With
        '        WriteAuditEntryWebSphere(Now.ToString & " Status Insert: " & strSQL)
        '        objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
        '        objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQLUpdate)
        '    End If

        'Catch ex As Exception
        '    WriteAuditEntry(Now.ToString & " Error updating Status table with WebSphere server info: " & ex.Message & vbCrLf & "SQL Statement: " & strSQL, LogLevel.Normal)
        'Finally
        '    '  MyDominoServer.LastScan = Now.ToString
        '    '   If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry("Updated  Domino Status Table with " & strSQL)
        '    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry("Next scan scheduled : " & MyWebSphereServer.NextScan)
        'End Try

        Try
            updateWebSphereNodeStatus(MyWebSphereServer)
            updateWebSphereCellStatus(MyWebSphereServer)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub updateWebSphereNodeStatus(ByRef MyWebSphereServer As MonitoredItems.WebSphere)
        'This function will get the status of all the servers inside a node and pick the "worest" one and assign the status of the node to that
        'Status order goes Not Responding > Issue > OK > Maintenance
        Dim strSQL As String = ""
        Dim objVSAdaptor As New VSAdaptor

        Try
            'Gets a list of all the servers inside the node
            Dim repositoryServers As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDefServers As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repositoryServers.Filter.Eq(Function(x) x.NodeId, MyWebSphereServer.NodeID) _
                                                                                        And repositoryServers.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.WebSphere.ToDescription())
            Dim listOfServers As List(Of VSNext.Mongo.Entities.Server) = repositoryServers.Find(filterDefServers).ToList()

            'Gets a list of all the status entitys of the servers in the node
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.In(Function(x) x.DeviceName, listOfServers.Select(Function(x) x.DeviceName).ToList()) _
                                                                                 And repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.WebSphere.ToDescription())

            Dim listOfStatus As List(Of VSNext.Mongo.Entities.Status) = repository.Find(filterDef).ToList()

            'Finds the worst status
            Dim worstStatus As String = ""
            Dim listOfDistinctStatuses As List(Of String) = listOfStatus.Select(Function(x) x.StatusCode).ToList().Distinct().ToList()

            If listOfDistinctStatuses.Contains("Not Responding") Then
                worstStatus = "Not Responding"
            ElseIf listOfDistinctStatuses.Contains("Issue") Then
                worstStatus = "Issue"
            ElseIf listOfDistinctStatuses.Contains("OK") Then
                worstStatus = "OK"
            Else
                worstStatus = "Maintenance"
            End If

            'updates the node's status document
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                 .Set(Function(x) x.StatusCode, worstStatus) _
                                                                                 .Set(Function(x) x.CurrentStatus, worstStatus) _
                                                                                 .Set(Function(x) x.JvmCount, listOfServers.Count) _
                                                                                 .Set(Function(x) x.JvmMonitoredCount, listOfServers.Where(Function(x) x.IsEnabled).Count)

            filterDef = repository.Filter.Eq(Function(x) x.DeviceName, MyWebSphereServer.NodeName) _
                And repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.WebSphereNode.ToDescription()) _
                And repository.Filter.Eq(Function(x) x.DeviceId, MyWebSphereServer.NodeID)

            repository.Upsert(filterDef, updateDef)

        Catch ex As Exception
            WriteDeviceHistoryEntry("WebSphere", MyWebSphereServer.Name, Now.ToString & " Error in WebSphereNode module creating statement for status table: " & ex.Message)
        End Try

        'WS Commented out for VSPLUS-2596
        'Try
        '	With MyWebSphereServer
        '		strSQL = "Select count(*) from Status where TypeANDName = '" & .NodeName & "-" & .ServerType & "'"
        '		Dim ds As New DataSet()
        '		ds.Tables.Add("table")
        '		objVSAdaptor.FillDatasetAny("VitalSigns", "VitalSigns", strSQL, ds, "table")
        '		Dim s As String = ds.Tables("table").Rows(0)(0).ToString()

        '		If ds.Tables("table").Rows(0)(0) = False Then
        '			strSQL = "INSERT INTO Status (Name, TypeANDName, StatusCode, Category,  Description, Location, Status, Type, LastUpdate, Details) VALUES " & _
        '			 "('" & .NodeName & "', '" & .NodeName & "-" & .ServerType & "', (select Status from WebSphereNode where NodeID=" & .NodeID & "), 'WebSphere Node', 'WebSphere Node', '" & .Location & "', (select Status from WebSphereNode where NodeID=" & .NodeID & "), " & _
        '			 "'" & .ServerType & "', '" & Now.ToString() & "', 'WebSphere Node')"

        '		Else

        '			strSQL = "UPDATE Status SET Name='" & .NodeName & "', StatusCode=(select Status from WebSphereNode where NodeID=" & .NodeID & "), " & _
        '			  "Category='WebSphere Node',  Description='WebSphere Node', Location='" & .Location & "', Status=(select Status from WebSphereNode where NodeID=" & .NodeID & "), Type='" & .ServerType & "', LastUpdate='" & Now.ToString() & "', Details='WebSphere Node' " & _
        '			  " where TypeANDName = '" & .NodeName & "-" & .ServerType & "'"

        '		End If

        '		objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)
        '	End With

        'Catch ex As Exception

        'End Try



    End Sub

    Private Sub updateWebSphereCellStatus(ByRef MyWebSphereServer As MonitoredItems.WebSphere)
        'This function will get the status of all the Nodes inside a Cell and pick the "worest" one and assign the status of the Cell to that
        'Status order goes Not Responding > Issue > OK > Maintenance
        Dim strSQL As String = ""
        Dim objVSAdaptor As New VSAdaptor

        Try
            'Gets a list of all the nodes inside the cell
            Dim repositoryServers As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDefServers As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repositoryServers.Filter.Eq(Function(x) x.CellId, MyWebSphereServer.CellID) _
                                                                                        And repositoryServers.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.WebSphereNode.ToDescription())
            Dim listOfServers As List(Of VSNext.Mongo.Entities.Server) = repositoryServers.Find(filterDefServers).ToList()

            'Gets a list of all the status docuemnts of the nodes in the cell
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.In(Function(x) x.DeviceName, listOfServers.Select(Function(x) x.DeviceName).ToList()) _
                                                                                 And repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.WebSphereNode.ToDescription())

            Dim listOfStatus As List(Of VSNext.Mongo.Entities.Status) = repository.Find(filterDef).ToList()

            'Finds the worst status
            Dim worstStatus As String = ""
            Dim listOfDistinctStatuses As List(Of String) = listOfStatus.Select(Function(x) x.StatusCode).ToList().Distinct().ToList()

            If listOfDistinctStatuses.Contains("Not Responding") Then
                worstStatus = "Not Responding"
            ElseIf listOfDistinctStatuses.Contains("Issue") Then
                worstStatus = "Issue"
            ElseIf listOfDistinctStatuses.Contains("OK") Then
                worstStatus = "OK"
            Else
                worstStatus = "Maintenance"
            End If

            'updates the cell status
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                 .Set(Function(x) x.StatusCode, worstStatus) _
                                                                                 .Set(Function(x) x.CurrentStatus, worstStatus) _
                                                                                 .Set(Function(x) x.JvmCount, listOfStatus.Sum(Function(x) x.JvmCount)) _
                                                                                 .Set(Function(x) x.JvmMonitoredCount, listOfStatus.Sum(Function(x) x.JvmMonitoredCount))

            filterDef = repository.Filter.Eq(Function(x) x.DeviceName, MyWebSphereServer.CellName) _
                And repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.WebSphereCell.ToDescription()) _
                And repository.Filter.Eq(Function(x) x.DeviceId, MyWebSphereServer.CellID)

            repository.Upsert(filterDef, updateDef)


        Catch ex As Exception
            WriteDeviceHistoryEntry("WebSphere", MyWebSphereServer.Name, Now.ToString & " Error in WebSphereCell module creating statement for status table: " & ex.Message)
        End Try

        'WS Commented out for VSPLUS-2596
        'Try
        '	With MyWebSphereServer
        '		strSQL = "Select count(*) from Status where TypeANDName = '" & .CellName & "-" & .ServerType & "'"

        '		Dim ds As New DataSet()
        '		ds.Tables.Add("table")
        '		objVSAdaptor.FillDatasetAny("VitalSigns", "VitalSigns", strSQL, ds, "table")
        '		Dim s As String = ds.Tables("table").Rows(0)(0).ToString()

        '		If ds.Tables("table").Rows(0)(0) = False Then
        '			strSQL = "INSERT INTO Status (Name, TypeANDName, StatusCode, Category,  Description, Location, Status, Type, LastUpdate, Details) VALUES " & _
        '			 "('" & .CellName & "', '" & .CellName & "-" & .ServerType & "', (select Status from WebSphereCellStats where CellID=" & .CellID & "), 'WebSphere Cell', 'WebSphere Cell', '" & .Location & "', (select Status from WebSphereCellStats where CellID=" & .CellID & "), " & _
        '			 "'" & .ServerType & "', '" & Now.ToString() & "', 'WebSphere Cell')"

        '		Else

        '			strSQL = "UPDATE Status SET Name='" & .CellName & "', StatusCode=(select Status from WebSphereCellStats where CellID=" & .CellID & "), " & _
        '			 "Category='WebSphere Cell',  Description='WebSphere Cell', Location='" & .Location & "', Status=(select Status from WebSphereCellStats where CellID=" & .CellID & "), Type='" & .ServerType & "', LastUpdate='" & Now.ToString() & "', Details='WebSphere Cell' " & _
        '			 " where TypeANDName = '" & .CellName & "-" & .ServerType & "'"

        '		End If

        '			objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)
        '	End With

        'Catch ex As Exception

        'End Try



    End Sub

    Public Sub InsertIntoWebSphereDailyStats(ByVal ServerName As String, ByVal StatName As String, ByVal StatValue As String, ByVal Details As String, DeviceId As String)
        'Dim dtNow As DateTime = Now

        'If StatValue IsNot Nothing Then

        '    Dim strSql As String = "Insert into VSS_Statistics.dbo.WebSphereDailyStats(ServerName,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) " & _
        '     " values('" & ServerName & "','" & dtNow & "','" & StatName & "','" & StatValue & "'," & GetWeekNumber(dtNow) & ", " & dtNow.Month.ToString() & _
        '     ", " & dtNow.Year.ToString() & ", " & dtNow.Day.ToString() & ", " & dtNow.Hour.ToString() & ",'')"

        '    Dim objVSAdaptor As New VSAdaptor

        '    Try
        '        objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", strSql)
        '    Catch ex As Exception

        '    End Try

        'Else
        '    WriteDeviceHistoryEntry("WebSphere", StatName, Now.ToString & " the following stat is empty: " & StatName)
        'End If



        Try
            'strSQL = "INSERT INTO DeviceDailyStats (DeviceType, ServerName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
            '   " VALUES ('Network Device', '" & DeviceName & "', '" & Now.ToString & "', '" & "ResponseTime" & "', '" & ResponseTime & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "')"
            'If boolUseSQLServer = True Then
            '    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
            'Else
            '    myCommand.CommandText = strSQL
            '    myCommand.ExecuteNonQuery()
            'End If

            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
            Dim DailyStats As New DailyStatistics
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
            DailyStats.DeviceType = VSNext.Mongo.Entities.Enums.ServerType.WebSphere.ToDescription()
            DailyStats.DeviceName = ServerName
            DailyStats.DeviceId = DeviceId
            DailyStats.StatName = StatName
            DailyStats.StatValue = StatValue

            repo.Insert(DailyStats)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & ex.Message)
            'WriteAuditEntry(Now.ToString & " The failed stats table insert comand was " & strSQL)
        Finally
            'myConnection.Close()

        End Try

        'myConnection.Dispose()
        'myCommand.Dispose()

    End Sub

    Private Sub UpdateWebSphereDetailsTable(ByRef MyWebSphereServer As MonitoredItems.WebSphere)
        '*************************************************************
        'Update the Status Table
        '*************************************************************
        Dim strSQL As String = ""

        Try
            'Update the status table

            With MyWebSphereServer
                strSQL = "Update WebSphereServerDetails SET " &
                " ProcessID='" & .ProcessId &
                 "', UpTimeSeconds='" & .UpTime &
                 "', AverageThreadCount='" & .AverageThreadPool &
                 "', ActiveThreadCount='" & .ActiveThreadCount &
                 "', CurrentHeapSize='" & .CurrentHeap &
                 "', HungThreadCount='" & .HungThreadCount &
                 "', ProcessCpu='" & .CPU_Utilization &
                 "', ProcessMemoryMb='" & .Memory_Used &
                "' WHERE ServerID='" & .ID & "'"
            End With

            WriteDeviceHistoryEntry("WebSphere", MyWebSphereServer.Name, Now.ToString & " WebSphere module  SQL statement: " & strSQL)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in WebSphere module creating SQL statement for WebSphereServerDetails: " & ex.Message)
            WriteDeviceHistoryEntry("WebSphere", MyWebSphereServer.Name, Now.ToString & " Error in WebSphere module creating SQL statement for status table: " & ex.Message)
        End Try


        Try

            Dim myRegistry As New RegistryHandler


            myRegistry = Nothing

            Dim objVSAdaptor As New VSAdaptor
            If objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL) = False And MyWebSphereServer.Enabled = True Then
                With MyWebSphereServer
                    strSQL = "INSERT INTO WebSphereServerDetails (ServerID, ProcessID, UpTimeSeconds, AverageThreadCount, ActiveThreadCount, " &
                        "CurrentHeapSize, HungThreadCount, ProcessCpu, ProcessMemoryMb) " &
                    " VALUES ('" & .ID & "', '" & .ProcessId & "', '" & .UpTime & "', '" & .AverageThreadPool & "', '" & .ActiveThreadCount & "', " &
                    " '" & .CurrentHeap & "', '" & .HungThreadCount & "', '" & .CPU_Utilization & "', '" & .Memory_Used & "')"
                End With
                WriteAuditEntryWebSphere(Now.ToString & " WebSphereServerDetails Insert: " & strSQL)
                objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
            End If

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error updating WebSphereServerDetails with WebSphere server info: " & ex.Message & vbCrLf & "SQL Statement: " & strSQL, LogLevel.Normal)

        End Try

    End Sub


#End Region

#Region "IBMConnect"

    Private Sub UpdateIBMConnectionStatusTable(ByRef myServer As MonitoredItems.IBMConnect)
        '*************************************************************
        'Update the Status Table
        '*************************************************************
        Dim strSQL As String = ""
        Dim StatusDetails As String = ""
        Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
        Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status)
        Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)
        Try
            If MyLogLevel = LogLevel.Verbose Then
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "  Entered UpdateIBMConenctStatusTable for " & myServer.Name)

            End If
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error logging StatusDetails String: " & ex.Message, LogLevel.Normal)
        End Try

        Try
            If (myServer.ResponseDetails.Trim().EndsWith(".")) Then
                myServer.ResponseDetails = myServer.ResponseDetails.Trim().Substring(0, myServer.ResponseDetails.Trim().Length - 1)
            End If
        Catch ex As Exception

        End Try

        Try
            StatusDetails = myServer.ResponseDetails & " at " & Now.ToString("t")
        Catch ex As Exception

        End Try
        Try
            If StatusDetails.Length > 254 Then
                StatusDetails = Left(myServer.ResponseDetails, 254)
            End If
        Catch ex As Exception
            StatusDetails = "Status Details exceed allowable field length.  Try again later.."
        End Try

        Try
            If InStr(StatusDetails, "'") > 0 Then
                StatusDetails = StatusDetails.Replace("'", "")
            End If

            Dim Quote As Char
            Quote = Chr(34)

            If InStr(StatusDetails, Quote) > 0 Then
                StatusDetails = StatusDetails.Replace(Quote, "~")
            End If

        Catch ex As Exception
            StatusDetails = ""
        End Try

        If myServer.Status = "Disabled" Then
            StatusDetails = "This instance is not enabled for monitoring."
        End If

        If myServer.Status = "Maintenance" Then
            StatusDetails = "This instance is in a scheduled maintenance period.  Monitoring is temporarily disabled."
        End If

        If myServer.Status = "Not Scanned" Then
            StatusDetails = "This instance has not been scanned yet."
        End If

        If MyLogLevel = LogLevel.Verbose Then
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Status Details for " & myServer.Name & " are " & StatusDetails)
        End If


        Try
            myServer.StatusCode = ServerStatusCode(myServer.Status)
        Catch ex As Exception
            myServer.StatusCode = vbNull
        End Try

        Try
            'Update the status table

            With myServer
                'strSQL = "Update Status SET " & _
                '" Status='" & .Status & "'" & _
                '", Details='" & StatusDetails & _
                ' "', Description='" & .Description & _
                ' "', StatusCode='" & .StatusCode & _
                '"', LastUpdate='" & Now & _
                '"', Category='" & .Category & _
                '"', ResponseTime='" & Str(.ResponseTime) & _
                '"', ResponseThreshold='" & .ResponseThreshold & _
                '"', NextScan='" & .NextScan & _
                '"', Name='" & .Name & _
                '"', CPU='" & .CPU_Utilization & _
                '"', Memory='" & .Memory_Utilization & _
                '"' WHERE TypeANDName='" & .Name & "-" & .ServerType & "'"

                Dim TypeAndName As String = .Name & "-" & .ServerType

                filterdef = repo.Filter.Where(Function(i) i.TypeAndName.Equals(TypeAndName))
                updatedef = repo.Updater _
                                  .Set(Function(i) i.DeviceName, .Name) _
                                  .[Set](Function(i) i.CurrentStatus, .Status) _
                                  .[Set](Function(i) i.StatusCode, .Status) _
                                  .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                                  .[Set](Function(i) i.Details, StatusDetails) _
                                  .[Set](Function(i) i.Category, .Category) _
                                  .[Set](Function(i) i.TypeAndName, TypeAndName) _
                                  .[Set](Function(i) i.Description, .Description) _
                                  .[Set](Function(i) i.DeviceType, .ServerType) _
                                  .[Set](Function(i) i.ResponseTime, Integer.Parse(.ResponseTime)) _
                                  .[Set](Function(i) i.ResponseThreshold, Integer.Parse(.ResponseThreshold)) _
                                  .[Set](Function(i) i.CPU, Integer.Parse(.CPU_Utilization)) _
                                  .[Set](Function(i) i.Memory, Integer.Parse(.Memory_Utilization)) _
                                  .[Set](Function(i) i.NextScan, .NextScan) _
                                  .[Set](Function(i) i.DeviceId, .ServerObjectID)
            End With
            repo.Upsert(filterdef, updatedef)
            'strSQL = strSQL.Replace("NaN", "0")
            'strSQL = strSQL.Replace("' ", "'")

            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " IBM Connect module  SQL statement: " & strSQL)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in IBM Connect module creating SQL statement for status table: " & ex.Message)
            WriteDeviceHistoryEntry(myServer.DeviceType, MyWebSphereServer.Name, Now.ToString & " Error in IBM Connect module creating SQL statement for status table: " & ex.Message)
        End Try


        'Try
        '**
        'Dim myPath As String
        'Dim myRegistry As New RegistryHandler


        'myRegistry = Nothing

        'Dim objVSAdaptor As New VSAdaptor
        'If objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL) = False And myServer.Enabled = True Then
        '    Dim strSQLUpdate As String = strSQL
        '    With myServer
        '        strSQL = "INSERT INTO Status (StatusCode, Category,  Description, Location, Name,  Status, Type, LastUpdate, ResponseTime, TypeANDName, NextScan, Details, ResponseThreshold) " & _
        '        " VALUES ('" & .StatusCode & "', '" & .Category & "', '" & .Description & "', '" & .Location & "', '" & .Name & "', '" & .Status & "',  " & _
        '        "'" & .ServerType & "', '" & Now & "', '" & .ResponseTime & "' , '" & .Name & "-" & .ServerType & "', '" & .NextScan & "', '" & StatusDetails & "', '" & .ResponseThreshold & "' )"
        '    End With
        '    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Status Insert: " & strSQL)
        '    objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
        '    objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQLUpdate)
        'End If

        'Catch ex As Exception
        '    WriteAuditEntry(Now.ToString & " Error updating Status table with IBM Connect server info: " & ex.Message & vbCrLf & "SQL Statement: " & strSQL, LogLevel.Normal)
        'Finally
        '    '  MyDominoServer.LastScan = Now.ToString
        '    '   If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry("Updated  Domino Status Table with " & strSQL)
        '    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry("Next scan scheduled : " & myServer.NextScan)
        'End Try

    End Sub

    Public Sub InsertIntoIBMConnectionsDailyStats(ByVal ServerName As String, ByVal StatName As String, ByVal StatValue As String, ByVal DeviceId As String)
        Dim dtNow As DateTime = Now

        If StatValue IsNot Nothing Then

            '         Dim strSql As String = "Insert into VSS_Statistics.dbo.IBMConnectionsDailyStats(ServerName,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber) " & _
            '          " values('" & ServerName & "','" & dtNow & "','" & StatName & "','" & StatValue & "'," & GetWeekNumber(dtNow) & ", " & dtNow.Month.ToString() & _
            '          ", " & dtNow.Year.ToString() & ", " & dtNow.Day.ToString() & ", " & dtNow.Hour.ToString() & ")"

            'Dim objVSAdaptor As New VSAdaptor

            Try
                Dim DailyStats As New DailyStatistics
                Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
                DailyStats.DeviceId = DeviceId
                DailyStats.DeviceType = VSNext.Mongo.Entities.Enums.ServerType.IBMConnections.ToDescription()
                DailyStats.DeviceName = ServerName
                DailyStats.StatName = StatName
                DailyStats.StatValue = StatValue
                repo.Insert(DailyStats)

                'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", strSql)
            Catch ex As Exception

            End Try

        Else
            WriteDeviceHistoryEntry("WebSphere", StatName, Now.ToString & " the following stat is empty: " & StatName)
        End If

    End Sub


#End Region


	Public Function ServerStatusCode(ByVal Status As String) As String

		Dim StatusCode As String = "OK"
		Try
			Select Case Status.Trim
				Case "OK"
					StatusCode = "OK"
				Case "Scanning"
					StatusCode = "OK"
				Case "Maintenance"
					StatusCode = "Maintenance"
				Case "Not Responding"
					StatusCode = "Not Responding"
				Case "Not Scanned"
					StatusCode = "Maintenance"
				Case "Disabled"
					StatusCode = vbNull
				Case "Insufficient Licenses"
					StatusCode = "Issue"
				Case "Timeout"
					StatusCode = "Not Responding"
				Case Else
					StatusCode = "Issue"
			End Select
		Catch ex As Exception
			StatusCode = vbNull
		End Try

		Return StatusCode
	End Function

    Private Sub RecordCountAvailability(ByVal DeviceType As String, ByVal DeviceName As String, ByVal UpPercent As Double, ByVal DeviceId As String)
		'This sub records the overall availability based on up and down counts
		WriteAuditEntry(Now.ToString & " Writing hourly Count Availability stats for " & DeviceName)

		Dim strSQL As String
		Dim MyWeekNumber As Integer
		Dim StatDate As DateTime = Now

		MyWeekNumber = GetWeekNumber(StatDate)
		Dim objVSAdaptor As New VSAdaptor
		Try
            'strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
            ' " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyUpPercent', '" & UpPercent & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
			'  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)
			'If boolUseSQLServer = True Then
			'    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
			'Else
			'    OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
			'    OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
			'End If

            Dim DailyStats As New DailyStatistics
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
            DailyStats.DeviceId = DeviceId
            DailyStats.DeviceType = DeviceType
            DailyStats.DeviceName = DeviceName
            DailyStats.StatName = "HourlyUpPercent"
            DailyStats.StatValue = UpPercent
            repo.Insert(DailyStats)

            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " RecordCountAvailability insert failed becase: " & ex.Message)
			WriteAuditEntry(Now.ToString & " The failed RecordCountAvailability insert comand was " & strSQL)
		End Try

		strSQL = Nothing
		MyWeekNumber = Nothing
		StatDate = Nothing

	End Sub

    Private Sub RecordOnTargetAvailability(ByVal DeviceType As String, ByVal DeviceName As String, ByVal OnTargetPercent As Double, ByVal DeviceId As String)
		WriteAuditEntry(Now.ToString & " Writing hourly On-Target Availability stats for " & DeviceName)
		'This sub records the percentage of times the device responded within its target response time
		Dim strSQL As String = ""
		Dim MyWeekNumber As Integer
		Dim StatDate As DateTime = Now

		MyWeekNumber = GetWeekNumber(StatDate)
		Dim objVSAdaptor As New VSAdaptor
		Try
            'strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
            ' " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyOnTargetPercent', '" & OnTargetPercent & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
			'  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)

			'If boolUseSQLServer = True Then
			'    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
			'Else
			'    OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
			'    OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
			'End If
            objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
            Dim DailyStats As New DailyStatistics
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
            DailyStats.DeviceId = DeviceId
            DailyStats.DeviceType = DeviceType
            DailyStats.DeviceName = DeviceName
            DailyStats.StatName = "HourlyOnTargetPercent"
            DailyStats.StatValue = OnTargetPercent
            repo.Insert(DailyStats)
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Record On Target Availability insert failed becase: " & ex.Message)
			WriteAuditEntry(Now.ToString & " The failed Record On Target Availability insert comand was " & strSQL)
		End Try
		strSQL = Nothing
		MyWeekNumber = Nothing
		StatDate = Nothing

	End Sub

    Private Sub RecordBusinessHoursOnTargetAvailability(ByVal DeviceType As String, ByVal DeviceName As String, ByVal OnTargetPercent As Double, ByVal DeviceId As String)
		WriteAuditEntry(Now.ToString & " Writing hourly Business Hours On-Target Availability stats for " & DeviceName)
		'This sub records the percentage of times the device responded within its target response time during business Hours only
		Dim strSQL As String
		Dim MyWeekNumber As Integer
		Dim StatDate As DateTime = Now

		MyWeekNumber = GetWeekNumber(StatDate)
		Dim objVSAdaptor As New VSAdaptor
		Try
            'strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
            ' " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyBusinessHoursOnTargetPercent', '" & OnTargetPercent & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
			'  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)

			'If boolUseSQLServer = True Then
			'    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
			'Else
			'    OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
			'    OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
			'End If
            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
            Dim DailyStats As New DailyStatistics
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
            DailyStats.DeviceId = DeviceId
            DailyStats.DeviceType = DeviceType
            DailyStats.DeviceName = DeviceName
            DailyStats.StatName = "HourlyBusinessHoursOnTargetPercent"
            DailyStats.StatValue = OnTargetPercent
            repo.Insert(DailyStats)
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Record On Target Availability insert failed becase: " & ex.Message)
			WriteAuditEntry(Now.ToString & " The failed Record On Target Availability insert comand was " & strSQL)
		End Try
		strSQL = Nothing
		MyWeekNumber = Nothing
		StatDate = Nothing


	End Sub

    Private Sub RecordTimeAvailability(ByVal DeviceType As String, ByVal DeviceName As String, ByVal UpPercent As Double, ByVal DeviceId As String)
		'This sub records the overall availability based on up and down counts
		WriteAuditEntry(Now.ToString & " Writing hourly Time Availability stats for " & DeviceName)

		Dim strSQL As String
		Dim MyWeekNumber As Integer
		Dim StatDate As DateTime = Now

		MyWeekNumber = GetWeekNumber(StatDate)
		Dim objVSAdaptor As New VSAdaptor
		Try
            'strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
            ' " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyUpTimePercent', '" & UpPercent & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
            ''  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)
            ''OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
            ''OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
            Dim DailyStats As New DailyStatistics
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
            DailyStats.DeviceId = DeviceId
            DailyStats.DeviceType = DeviceType
            DailyStats.DeviceName = DeviceName
            DailyStats.StatName = "HourlyUpTimePercent"
            DailyStats.StatValue = UpPercent
            repo.Insert(DailyStats)
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " RecordTimeAvailability insert failed because: " & ex.Message)
			WriteAuditEntry(Now.ToString & " The failed RecordTimeAvailability insert command was " & strSQL)
		End Try

		strSQL = Nothing
		MyWeekNumber = Nothing
		StatDate = Nothing

	End Sub

    Private Sub RecordDownTime(ByVal DeviceType As String, ByVal DeviceName As String, ByVal DownMinutes As Double, ByVal DeviceId As String)
		'This sub records the overall availability based on up and down counts
		WriteAuditEntry(Now.ToString & " Writing hourly Downtime  stats for " & DeviceName)
		If DownMinutes > 60 Then
			DownMinutes = 60
		End If

		Dim strSQL As String
		Dim MyWeekNumber As Integer
		Dim StatDate As DateTime = Now

		MyWeekNumber = GetWeekNumber(StatDate)
		Dim objVSAdaptor As New VSAdaptor
		Try
            'strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
            ' " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyDownTimeMinutes', '" & DownMinutes & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
            ''  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)

            ''If boolUseSQLServer = True Then
            ''    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
            ''Else
            ''    OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
            ''    OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
            ''End If
            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
            Dim DailyStats As New DailyStatistics
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
            DailyStats.DeviceId = DeviceId
            DailyStats.DeviceType = DeviceType
            DailyStats.DeviceName = DeviceName
            DailyStats.StatName = "HourlyDownTimeMinutes"
            DailyStats.StatValue = DownMinutes
            repo.Insert(DailyStats)
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " RecordTimeAvailability insert failed because: " & ex.Message)
			WriteAuditEntry(Now.ToString & " The failed RecordTimeAvailability insert command was " & strSQL)
		End Try

		strSQL = Nothing
		MyWeekNumber = Nothing
		StatDate = Nothing

	End Sub


	Private Function FixDate(ByVal dt As DateTime) As String
		' Return dt.ToUniversalTime.ToString
		'Mukund 28May14: FixDate called to convert date to current date format of the SQL Server, using strDateFormat
		Dim strdt As String
		strdt = objDateUtils.FixDate(dt, strDateFormat)

		'WriteAuditEntry(Now.ToString & " FixDateTime, ret dt " & strDateFormat & ":" & strdt)
		Return strdt

	End Function

	Private Function FixDateTime(ByVal dt As DateTime) As String
		' Return dt.ToUniversalTime.ToString
		Dim strDate As String
		strDate = FixDate(dt) & " " & dt.ToShortTimeString
		' strDate = FixDate_OLD(dt) & " " & dt.ToShortTimeString

		Return strDate

		' Return objDateUtils.FixDateTime(dt, strDateFormat)


	End Function

	Private Function FixTime(ByVal TimeString As String) As String
		'incoming format is 08/11/2005 14:02:17 
		'Outgoing format is time only "14:33:17"
		Dim NewTime As String

		Dim mystart As Integer
		mystart = TimeString.IndexOf(" ")
		NewTime = Right(TimeString, mystart)
		' WriteAuditEntry(Now.ToString & " FIXTIME ends with with " & NewTime)

		Return NewTime


	End Function

    'Kiran Dadireddy VSPLUS-2684
    Public Sub ShrinkDBLogOnHourlyBasis()
        WriteAuditEntry(Now.ToString & " Starting Shrinking VitalSigns Log ")
        ShrinkLog("VitalSigns")

        WriteAuditEntry(Now.ToString & " Starting Shrinking VSS_Statistics Log ")
        ShrinkLog("VSS_Statistics")
    End Sub
    'Kiran Dadireddy VSPLUS-2684
    Private Sub ShrinkLog(ByVal connection As String)
        Return
        'Try

        '    Dim myAdapter As New VSFramework.XMLOperation
        '    Using con As New System.Data.SqlClient.SqlConnection(myAdapter.GetDBConnectionString(connection))
        '        con.Open()
        '        Dim command As New SqlCommand("DBCC SHRINKFILE(" + connection + "_Log,10)", con)
        '        command.ExecuteNonQuery()
        '    End Using

        '    WriteAuditEntry(Now.ToString & " Completed shrinking " + connection + " Log ")
        'Catch ex As Exception

        '    WriteAuditEntry(Now.ToString & " Exception while Shrinking " + connection + " Log . \n Exception :" + ex.Message)
        'End Try
    End Sub

End Class
