Imports System.Threading
Imports System.IO
Imports VSFramework
Imports System.Data.SqlClient
Imports MongoDB.Driver
Imports VSNext.Mongo.Entities

Partial Public Class VitalSignsPlusDomino


#Region "Settings Table"

    Public Sub WriteSettingsValue(key As String, value As Object)
        Try
            Dim myAdapter As New VSFramework.XMLOperation
            myAdapter.WriteSettingsSQL(key, value)
        Catch ex As Exception

        End Try

    End Sub

    Public Function ReadSettingValue(key As String) As Object
        Try
            Dim myAdapter As New VSFramework.XMLOperation
            Return myAdapter.ReadSettingsSQL(key)
        Catch ex As Exception
            Return Nothing
        End Try

    End Function
#End Region

#Region "Database Handling"

#Region "Update Statistics Database"

    ' Not being used
    'Public Sub UpdateStatisticsTable(ByVal SQLUpdateStatement As String, Optional ByVal SQLInsertStatement As String = "", Optional ByVal Comment As String = "")
    '    WriteAuditEntry(Now.ToString + " ********************* Common Statistics UPDATE ******************** ")

    '    Dim RA As Integer   'Rows affected
    '    Dim objVSAdaptor As New VSAdaptor
    '    If objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "Statistics", SQLUpdateStatement) = False And SQLInsertStatement <> "" Then
    '        objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "Statistics", SQLInsertStatement)
    '    End If

    'End Sub


#End Region

#Region "Status Table Handling"
    'The routines here that are appended by "OLD" used the initial method of deleting all the
    'items of a given type from the status menu, then inserting the current values
    'but users did not like it, it made items seem to disappear.

    'new method does a delete of just the obsolete records

    'Public Sub UpdateStatusTable(ByVal SQLUpdateStatement As String, Optional ByVal SQLInsertStatement As String = "", Optional ByVal Comment As String = "")
    '    '     WriteAuditEntry(Now.ToString + " ********************* Common STATUS UPDATE ******************** ")
    '    'If Comment <> "" Then
    '    '    WriteAuditEntry(Now.ToString + " *********************")
    '    '    WriteAuditEntry(Now.ToString + " " & Comment)
    '    '    WriteAuditEntry(Now.ToString + " *********************")
    '    'End If
    '    ''This routine is used to update the status table with the results and a device scan.  First it tries to 
    '    'do an update.  if that fails and an insert command is sent, it will try that.
    '    Dim RA As Integer
    '    Dim objVSAdaptor As New VSAdaptor
    '    RA = objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", SQLUpdateStatement)

    '    If RA = 0 And SQLInsertStatement <> "" Then
    '        objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", SQLInsertStatement)
    '    End If

    'End Sub

    Public Sub InitializeStatusTable()

        Try
            If MyDominoServers.Count > 0 Then
                UpdateStatusTableWithDomino()
            End If
        Catch ex As Exception

        End Try

        '5/3/2016 NS uncommented for VSPLUS-2921
        Try
            If myDominoClusters.Count > 0 Then
                UpdateStatusTableWithDominoClusters()
            End If
        Catch ex As Exception

        End Try


        Try
            If MyNotesMailProbes.Count > 0 Then
                UpdateStatusTableWithNotesMailProbes()
                ClearExistingNotesMailProbeHistory()

            End If
        Catch ex As Exception

        End Try



        Try
            If MyNotesDatabases.Count > 0 Then
                UpdateStatusTableWithNotesDatabases()
            End If
        Catch ex As Exception

        End Try

        'Try
        '    If MyURLs.Count > 0 Then
        '        UpdateStatusTableWithURLs()
        '    End If

        'Catch ex As Exception

        'End Try


    End Sub

    Private Sub UpdateStatusTableWithDomino()
        'Delete from the status table any servers that have been deleted from the collection

        Dim myType As String = "'Domino'"
        Dim strSQL As String
        'Insert any Domino servers that are not in the status table
        WriteAuditEntry(Now.ToString & " Inserting Domino servers into the status table....")
        Dim n As Integer
        Dim myDominoServer As MonitoredItems.DominoServer
        Dim Percent As Double
        Dim strSqlUpdate As String
        '4/7/2016 NS added for VSPLUS-2817
        Dim sqlcmd As SqlCommand
        For n = 0 To MyDominoServers.Count - 1
            Try
                myDominoServer = MyDominoServers.Item(n)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error locating Domino servers for the Status Table " & ex.ToString)
            End Try


            With myDominoServer
                WriteAuditEntry(Now.ToString & " Processing server " & myDominoServer.Name & "  (" & n.ToString & ")")
                Try
                    Percent = myDominoServer.PendingMail / myDominoServer.PendingThreshold * 100
                Catch ex As Exception
                    Percent = 0
                End Try

                If InStr(myDominoServer.Name, "RPRWyattTest") Then
                    Percent = 0
                End If

                Try
                    myDominoServer.StatusCode = ServerStatusCode(myDominoServer.Status)
                Catch ex As Exception

                End Try

                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.TypeAndName, .Name + "-" + .ServerType)
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                     .Set(Function(x) x.StatusCode, "Maintenance") _
                                                                                     .Set(Function(x) x.Category, .Category) _
                                                                                     .Set(Function(x) x.DeadMail, .DeadMail) _
                                                                                     .Set(Function(x) x.Description, .Description) _
                                                                                     .Set(Function(x) x.DownCount, .DownCount) _
                                                                                     .Set(Function(x) x.Location, .Location) _
                                                                                     .Set(Function(x) x.Name, .Name) _
                                                                                     .Set(Function(x) x.MailDetails, "") _
                                                                                     .Set(Function(x) x.PendingMail, .PendingMail) _
                                                                                     .Set(Function(x) x.CurrentStatus, .Status) _
                                                                                     .Set(Function(x) x.Type, .ServerType) _
                                                                                     .Set(Function(x) x.UpCount, .UpCount) _
                                                                                     .Set(Function(x) x.UpPercent, .UpPercentCount) _
                                                                                     .Set(Function(x) x.ResponseTime, Convert.ToInt32(.ResponseTime)) _
                                                                                     .Set(Function(x) x.OperatingSystem, .OperatingSystem) _
                                                                                     .Set(Function(x) x.SoftwareVersion, .VersionDomino) _
                                                                                     .Set(Function(x) x.Details, .ResponseDetails) _
                                                                                     .Set(Function(x) x.PendingThreshold, .PendingThreshold) _
                                                                                     .Set(Function(x) x.DeadThreshold, .DeadThreshold) _
                                                                                     .Set(Function(x) x.UpMinutes, .UpMinutes) _
                                                                                     .Set(Function(x) x.DownMinutes, .DownMinutes) _
                                                                                     .Set(Function(x) x.UpPercentMinutes, 0) _
                                                                                     .Set(Function(x) x.NextScan, GetFixedDateTime(.NextScan))

                Try
                    repository.Upsert(filterDef, updateDef)
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Error inserting Domino servers into the Status Table " & ex.ToString)
                End Try

            End With



        Next n

        myDominoServer = Nothing
        Percent = Nothing
        n = Nothing

    End Sub


    Private Sub UpdateStatusTableWithDominoClusters()
        WriteAuditEntry(Now.ToString & " Updating status table with Lotus Domino servers clusters.")
        'Delete from the status table any servers that have been deleted from the collection
        Dim myIndex As Integer
        Dim Dom As MonitoredItems.DominoMailCluster
        Dim myServerNames As String = ""
        For Each Dom In myDominoClusters
            myServerNames += Dom.Name & vbCrLf
        Next

        Dom = Nothing
        ' myServerNames = Nothing
        myIndex = Nothing

        'Connect to the data source
        Dim dsStatusHTML As New Data.DataSet
        Dim Status As New Data.DataTable("Status")
        dsStatusHTML.Clear()
        dsStatusHTML.Tables.Add(Status)
        Dim drv As DataRowView

        Dim myConnection As New OleDb.OleDbConnection
        Dim myAdapter As New OleDb.OleDbDataAdapter
        Dim myCommand As New OleDb.OleDbCommand


        Dim mySQLCommand As New Data.SqlClient.SqlCommand
        Dim mySQLAdapter As New Data.SqlClient.SqlDataAdapter


        Dim strSQL As String = "SELECT Name, Status, Type, TypeANDName FROM Status WHERE Type = 'Domino Cluster database'"
        Dim objVSAdaptor As New VSAdaptor
        Try
            objVSAdaptor.FillDatasetAny("VitalSigns", "Status", strSQL, dsStatusHTML, "Status")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " UpdateStatusTableWithDominoCluster connection error @2 " & ex.Message)
        End Try


        Dim myView As New Data.DataView(Status)

        Try
            myView.Sort = "Type ASC"
            WriteAuditEntry(Now.ToString & " UpdateStatusTableWithDominoCluster module is processing " & myView.Count & " records.")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " UpdateStatusTableWithDominoCluster module error " & ex.Message & " source: " & ex.Source)
        End Try

        Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
        Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status)
        Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)

        Try

            WriteAuditEntry(Now.ToString & " Deleting Domino Clusters that are not in " & String.Join(",", (From x In myDominoClusters Select (x.Name.ToString())).ToList()), LogLevel.Verbose)

            filterDef = repository.Filter.Nin(Function(x) x.Name, (From x In myDominoClusters Select (x.Name.ToString())).ToList()) And _
                repository.Filter.Eq(Function(x) x.Type, "Domino Cluster database")
            repository.Delete(filterDef)

        Catch ex As Exception

        End Try

        'Now Update the Data set with the current Domino data 

        Try
            Dim n As Integer
            Dim myDominoServerCluster As MonitoredItems.DominoMailCluster
            Dim strSqlUpdate As String

            For n = 0 To myDominoClusters.Count - 1
                myDominoCluster = myDominoClusters.Item(n)
                WriteAuditEntry(Now.ToString & " Inserting " & myDominoCluster.Name)
                With myDominoCluster

                    filterDef = repository.Filter.Eq(Function(x) x.TypeAndName, .Name & "-" + .ServerType)
                    updateDef = repository.Updater _
                        .Set(Function(x) x.Name, .Name) _
                        .Set(Function(x) x.CurrentStatus, .Status) _
                        .Set(Function(x) x.Type, .ServerType) _
                        .Set(Function(x) x.LastUpdated, GetFixedDateTime(Now)) _
                        .Set(Function(x) x.NextScan, GetFixedDateTime(.NextScan)) _
                        .Set(Function(x) x.Details, .ResponseDetails) _
                        .Set(Function(x) x.Category, .Category)

                    Try
                        repository.Upsert(filterDef, updateDef)
                    Catch ex As Exception

                    End Try

                End With

            Next n

            myDominoCluster = Nothing

            n = Nothing


        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error connecting to Status Table while inserting Domino Clusters:" & ex.Message)
            WriteAuditEntry(Now.ToString & " Insert comand was " & strSQL)
            '       WriteAuditEntry(Now.ToString & " Service stopped.")
            Thread.CurrentThread.Sleep(1000)
            ' End
        End Try

        mySQLCommand.Dispose()
        mySQLAdapter.Dispose()
        myCommand.Dispose()
        myConnection.Close()
        myConnection.Dispose()
        WriteAuditEntry(Now.ToString & " Finished updating Status table with all Domino server clusters.")

    End Sub

    Private Sub UpdateStatusTableWithNotesDatabases()
        'Delete from the status table any servers that have been deleted from the collection
        'Connect to the data source

        Dim strSQL As String
        Dim objVSAdaptor As New VSAdaptor


        'Insert any Notes databases that are not in the status table

        Dim n As Integer

        Dim Percent As Double
        Dim strSqlUpdate As String
        WriteAuditEntry(Now.ToString & " Adding Notes " & MyNotesDatabases.Count & " Databases to the status table.")

        Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
        Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status)
        Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)

        For Each NotesDB As MonitoredItems.NotesDatabase In MyNotesDatabases
            With NotesDB
                'VSPLUS-939,Somaraj 16Oct14, Added StatusCode, which was missing.
                Try
                    .StatusCode = ServerStatusCode(.Status)
                Catch ex As Exception
                    .StatusCode = "OK"
                End Try

                filterDef = repository.Filter.Where(Function(x) x.TypeAndName = .Name & "-" + .ServerType)
                updateDef = repository.Updater _
                    .Set(Function(x) x.Name, .Name) _
                    .Set(Function(x) x.CurrentStatus, .Status) _
                    .Set(Function(x) x.Type, .ServerType) _
                    .Set(Function(x) x.LastUpdated, GetFixedDateTime(Now)) _
                    .Set(Function(x) x.Details, .ResponseDetails) _
                    .Set(Function(x) x.Category, .Category) _
                    .Set(Function(x) x.NextScan, GetFixedDateTime(.NextScan)) _
                    .Set(Function(x) x.Description, .Description) _
                    .Set(Function(x) x.DownCount, .DownCount) _
                    .Set(Function(x) x.Location, .Location) _
                    .Set(Function(x) x.UpCount, .UpCount) _
                    .Set(Function(x) x.UpPercent, .UpPercentCount) _
                    .Set(Function(x) x.ResponseTime, Convert.ToInt32(.ResponseTime)) _
                    .Set(Function(x) x.ResponseThreshold, Convert.ToInt32(.ResponseThreshold)) _
                    .Set(Function(x) x.StatusCode, .StatusCode)

                Try
                    repository.Upsert(filterDef, updateDef)
                Catch ex As Exception

                End Try

            End With

        Next


        Percent = Nothing
        n = Nothing

        'Clean up the memory when done of all unmanaged resources

    End Sub


    Private Sub UpdateStatusTableWithNotesMailProbes()
        WriteAuditEntry(Now.ToString & " Updating status table with NotesMail Probes.")
        'Now delete the existing NM records 

        Dim strSQL As String
        'Dim myConnection As New OleDb.OleDbConnection
        'myConnection.ConnectionString = Me.OleDbConnectionStatus.ConnectionString
        Dim objVSAdaptor As New VSAdaptor

        'Now Update the Data set with the Mail Probes
        Dim n As Integer
        Dim MyNotesMailProbe As MonitoredItems.DominoMailProbe

        Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
        Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status)
        Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)

        For n = 0 To MyNotesMailProbes.Count - 1
            strSQL = ""
            MyNotesMailProbe = MyNotesMailProbes.Item(n)
            WriteAuditEntry(Now.ToString & " adding Mail Probe " & MyNotesMailProbe.Name)
            With MyNotesMailProbe
                MyNotesMailProbe.StatusCode = ServerStatusCode(MyNotesMailProbe.Status)

                filterDef = repository.Filter.Eq(Function(x) x.TypeAndName, .Name.ToString() & "-" + .ServerType)
                updateDef = repository.Updater _
                    .Set(Function(x) x.Name, .Name) _
                    .Set(Function(x) x.CurrentStatus, .Status) _
                    .Set(Function(x) x.Type, .ServerType) _
                    .Set(Function(x) x.LastUpdated, GetFixedDateTime(Now)) _
                    .Set(Function(x) x.Details, .ResponseDetails) _
                    .Set(Function(x) x.Category, .Category) _
                    .Set(Function(x) x.NextScan, GetFixedDateTime(.NextScan)) _
                    .Set(Function(x) x.Description, .Description) _
                    .Set(Function(x) x.DownCount, .DownCount) _
                    .Set(Function(x) x.Location, .Location) _
                    .Set(Function(x) x.UpCount, .UpCount) _
                    .Set(Function(x) x.UpPercent, .UpPercentCount) _
                    .Set(Function(x) x.ResponseTime, Convert.ToInt32(.ResponseTime)) _
                    .Set(Function(x) x.ResponseThreshold, Convert.ToInt32(.ResponseThreshold)) _
                    .Set(Function(x) x.StatusCode, .StatusCode)


                Try
                    repository.Upsert(filterDef, updateDef)
                Catch ex As Exception

                End Try


            End With

        Next n
        MyNotesMailProbe = Nothing
        n = Nothing
        strSQL = Nothing
        WriteAuditEntry(Now.ToString & " Updated Status table with NotesMail Probe information.")


    End Sub

    Private Sub ClearExistingNotesMailProbeHistory()
        WriteAuditEntry(Now.ToString & " Clearing history table for NotesMail Probes.")
        'Now delete the existing Probe records 

        '2/11/2016 NS modified for VSPLUS-2585
        Dim strSQL As String = "DELETE FROM NotesMailProbeHistory WHERE SentDateTime <= DATEADD(hh,-24,GETDATE())"
        Dim objVSAdaptor As New VSAdaptor
        Try
            objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "servers", strSQL)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error with Status Table while Clearing NotesMail Probe history:" & ex.Message)
        End Try


    End Sub

    'Public Sub UpdateUpTimeAllDevices()

    '    Try
    '        If MyDominoServers.Count > 0 Then

    '            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
    '            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status)
    '            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)

    '            For Each DS As MonitoredItems.DominoServer In MyDominoServers
    '                With DS

    '                    filterDef = repository.Filter.Where(Function(x) x.TypeAndName = .Name & "-Domino")
    '                    updateDef = repository.Updater _
    '                        .Set(Function(x) x.NextScan, GetFixedDateTime(.NextScan)) _
    '                        .Set(Function(x) x.UpMinutes, .UpMinutes) _
    '                        .Set(Function(x) x.DownMinutes, .DownMinutes)

    '                    Try
    '                        repository.Upsert(filterDef, updateDef)
    '                    Catch ex As Exception

    '                    End Try

    '                End With
    '            Next
    '        End If
    '    Catch ex As Exception

    '    End Try


    'End Sub


#End Region

#Region "MS Access Tables, Columns and Settings"


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


    Public Function GetConsecutiveTelnetValue() As Double

        Dim strSQL As String
        Dim intResult As Double = 0
        Dim objVSAdaptor As New VSAdaptor

        'str1SQL = "Select COUNT (Distinct  Status) from Status WHERE Type = 'Domino' AND Status <> 'Scanning'"
        strSQL = "SELECT svalue AS N FROM dbo.settings where sname = 'ConsecutiveTelnet' "

        Try
            intResult = Convert.ToDouble(objVSAdaptor.ExecuteScalarAny("VitalSigns", "Status", strSQL))
            WriteAuditEntry(Now.ToString & "  Consecutive Telnet Value = " & intResult)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & "  Exception getting Consecutive Telnet Value ")
            intResult = 8
        End Try
        Return intResult

    End Function
    Public Function getThreadCount(ServerType As String) As Integer
        'Dim db As New CommonDB()
        Dim numOfThreads As Integer = 35
        Dim Svalue As String = ""
        Dim sql As String = (Convert.ToString("SELECT SValue FROM Settings WHERE SName = 'ThreadLimit") & ServerType) + "'"
        Dim myAdapter As New VSAdaptor
        Dim dt As DataTable = New DataTable
        Dim DsSettings As New Data.DataSet


        dt.TableName = "Settings"
        DsSettings.Tables.Add(dt)
        myAdapter.FillDatasetAny("VitalSigns", "vitalsigns", sql, DsSettings, "Settings")

        If dt.Rows.Count > 0 Then
            Svalue = DsSettings.Tables("Settings").Rows(0)("svalue").ToString()
            numOfThreads = Convert.ToInt32(dt.Rows(0)(0).ToString())

        End If

        Return numOfThreads
    End Function

    Public Function ServerStatusCode(ByVal Status As String) As String

        Dim StatusCode As String = "OK"
        Try
            Select Case Status
                Case "OK"
                    StatusCode = "OK"
                Case "Scanning"
                    StatusCode = "OK"
                Case "Maintenance"
                    StatusCode = "Maintenance"
                Case "Not Responding"
                    StatusCode = "Not Responding"
                Case "Not Scanned"
                    StatusCode = "Maintenance" ' was OK
                Case "Disabled"
                    StatusCode = vbNull
                Case "Insufficient Licenses"
                    StatusCode = "Issue"
                    '5/20/2016 NS modified for VSPLUS-2874
                Case "Traveler Status: Red"
                    StatusCode = "Issue"
                    '5/20/2016 NS modified for VSPLUS-2874
                Case "Traveler Status: Yellow"
                    StatusCode = "Issue"
                Case Else
                    StatusCode = "Issue"
            End Select
        Catch ex As Exception
            StatusCode = vbNull
        End Try

        Return StatusCode
    End Function


#Region "Domino-related Database Updating"

    Private Sub cleanUpClusterDetailedTable(ByVal ClusterName As String)
        Dim strSQL As String = ""
        Dim objVSAdaptor As New VSAdaptor
        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.ClusterDatabaseDetails)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.ClusterDatabaseDetails)

            filterDef = repository.Filter.Where(Function(x) x.ClusterName = ClusterName)
            repository.Delete(filterDef)


        Catch ex As Exception

        Finally

        End Try
        strSQL = Nothing
    End Sub

    '9/30/2015 NS modified for VSPLUS-2150
    Private Sub UpdateClusterDataTable(ByVal ClusterName As String, ByVal DatabaseTitle As String, ByVal DatabaseName As String,
            ByVal SACount As Int32, ByVal SBCount As Int32, ByVal SCCount As Int32, ByVal SASize As Long, ByVal SBSize As Long,
            ByVal SCSize As Long, ByVal Desc As String, ByVal lastScanned As DateTime, ByVal ReplicaID As String)
        Dim strSQL As String = ""
        Dim objVSAdaptor As New VSAdaptor
        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.ClusterDatabaseDetails)(connectionString)

            Dim entity As New VSNext.Mongo.Entities.ClusterDatabaseDetails With {
                .ClusterName = ClusterName,
                .DatabaseName = DatabaseName,
                .DatabaseSizeA = SASize,
                .DatabaseSizeB = SBSize,
                .DatabaseSizeC = SCSize,
                .DatabaseTitle = DatabaseTitle,
                .Description = Desc,
                .DocumentCountA = SACount,
                .DocumentCountB = SBCount,
                .DocumentCountC = SCCount,
                .ReplicaID = ReplicaID
            }

            repository.Insert(entity)

        Catch ex As Exception
            Debug.WriteLine(Now.ToString & " update ClusterDatabaseDetails table insert failed because: " & ex.Message)
            Debug.WriteLine(Now.ToString & " The failed cluster table insert command was " & strSQL)
            WriteAuditEntry("Exception updating ClusterDatabaseDetails: " & ex.Message)
        Finally
            'myConnection.Close()
            'myConnection.Dispose()
            'myCommand.Dispose()
        End Try
        strSQL = Nothing
    End Sub


    Private Sub UpdateDominoStatusTable(ByRef MyDominoServer As MonitoredItems.DominoServer)
        '*************************************************************
        'Update the Status Table
        '*************************************************************
        Dim strSQL As String
        Dim strSQL_Short As String
        Dim StatusDetails As String = ""

        Try
            If MyLogLevel = LogLevel.Verbose Then
                '   WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " %% Entered UpdateDominoStatusTable for " & MyDominoServer.Name)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Status Details for " & MyDominoServer.Name & " are " & MyDominoServer.ResponseDetails)
            End If
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error logging StatusDetails String: " & ex.Message)
        End Try

        Try
            '   WriteAuditEntry(Now.ToString & " Response Details: " & MyDominoServer.ResponseDetails)
            If MyDominoServer.ResponseDetails <> "" Or MyDominoServer.ResponseDetails <> " " Then
                StatusDetails = MyDominoServer.ResponseDetails
            Else
                StatusDetails = "Pending: " & MyDominoServer.PendingMail & vbCrLf & " Dead: " & MyDominoServer.DeadMail & vbCrLf & " Users: " & MyDominoServer.UserCount
            End If
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating StatusDetails String: " & ex.Message)
        End Try

        Try
            If MyDominoServer.Memory <> "" Then
                StatusDetails += vbCrLf & "Memory: " & MyDominoServer.Memory & " "
            End If
        Catch ex As Exception

        End Try

        Try
            If MyDominoServer.TaskStatus = "" Then MyDominoServer.TaskStatus = "Not configured to monitor server tasks on this server."
        Catch ex As Exception
        End Try

        Try
            If MyDominoServer.TaskStatus > 254 Then
                MyDominoServer.TaskStatus = Left(MyDominoServer.TaskStatus, 250) & "..."
            End If
        Catch ex As Exception
            ' StatusDetails = "Task status exceed allowable field length."
        End Try


        Try
            If StatusDetails.Length > 254 Then
                StatusDetails = Left(StatusDetails, 250) & "..."
            End If
        Catch ex As Exception
            StatusDetails = "Status Details exceed allowable field length."
        End Try

        Try
            If InStr(StatusDetails, "'") > 0 Then
                StatusDetails = StatusDetails.Replace("'", "")
            End If

            Dim Quote As Char
            Quote = Chr(34)

            If InStr(StatusDetails, Quote) > 0 Then
                StatusDetails = StatusDetails.Replace(Quote, " ")
            End If

        Catch ex As Exception
            StatusDetails = "Pending: " & MyDominoServer.PendingMail & vbCrLf & " Dead: " & MyDominoServer.DeadMail & vbCrLf & " Users: " & MyDominoServer.UserCount
        End Try

        If MyDominoServer.Status = "Disabled" Then
            StatusDetails = "This server is not enabled for monitoring."
        End If

        If MyDominoServer.Status = "Maintenance" Then
            StatusDetails = "This server is in a scheduled maintenance period.  Monitoring is temporarily disabled."
        End If

        If MyDominoServer.Status = "Not Scanned" Then
            StatusDetails = "This server has not been scanned yet."
        End If


        'Update the status table
        'WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Calculating the value for the Percent Meter")
        'WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This server's pending mail is: " & MyDominoServer.PendingMail)
        'WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This server's pending mail threshold is: " & MyDominoServer.PendingThreshold)
        'WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " About to divide pending mail by pending threshold")

        Dim Percent As Double
        Try
            Percent = MyDominoServer.PendingMail / MyDominoServer.PendingThreshold * 100
        Catch ex As Exception
            Percent = 0
        End Try
        '  WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This server's percent meter value (no formatting) is " & Percent.ToString)
        '  WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This server's percent meter value (fixed 2) is " & Percent.ToString("F2"))

        '     Dim MyCI As New CultureInfo("en-US", False)
        Dim strPercent As String
        strPercent = Percent.ToString("F2")
        '  WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This server's percent meter value (US format) is " & Percent.ToString(myCI))

        Dim PercentageChange As Double
        Try
            If MyDominoServer.PreviousKeyValue > 0 And MyDominoServer.ResponseTime > 0 Then
                PercentageChange = -(1 - MyDominoServer.PreviousKeyValue / MyDominoServer.ResponseTime)
            Else
                PercentageChange = 0
            End If

        Catch ex As Exception
            PercentageChange = 0
        End Try
        '  If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Previous response time was " & MyDominoServer.PreviousKeyValue & " Current value is " & MyDominoServer.ResponseTime & "; so percent change is: " & PercentageChange)

        Dim myUpPercent As Double
        Try
            myUpPercent = CType(MyDominoServer.UpPercentMinutes, Double)
        Catch ex As Exception
            myUpPercent = 1
        End Try

        ' ", UpPercentMinutes=" & myUpPercent & _

        Try
            If InStr(MyDominoServer.VersionDomino, "Access to this server") > 0 Then
                MyDominoServer.VersionDomino = "Unknown"
            End If
        Catch ex As Exception

        End Try

        Dim myIcon As Integer
        Try
            If MyDominoServer.QuickrServer = True Then
                myIcon = IconList.Quickr
            Else
                myIcon = IconList.DominoServer
            End If
        Catch ex As Exception

        End Try

        Dim DominoVersion As String = ""
        Try
            DominoVersion = Left(ParseTextStatValue("Server.Version.Notes", MyDominoServer.Statistics_Server), 100)
        Catch ex As Exception
            DominoVersion = " "
        End Try


        Try
            MyDominoServer.StatusCode = ServerStatusCode(MyDominoServer.Status)
        Catch ex As Exception
            MyDominoServer.StatusCode = vbNull
        End Try

        Try
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Parsing out Last Scan of " & MyDominoServer.LastScan)
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & "   LastUpdate=" & FixDateTime(MyDominoServer.LastScan))
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception parsing out Last Scan of " & ex.ToString)
        End Try

        Try
            With MyDominoServer

                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.TypeAndName, .Name & "-" & .ServerType)
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                     .Set(Function(x) x.DeadMail, .DeadMail) _
                                                                                     .Set(Function(x) x.DownCount, .DownCount) _
                                                                                     .Set(Function(x) x.PendingMail, .PendingMail) _
                                                                                     .Set(Function(x) x.CurrentStatus, .Status) _
                                                                                     .Set(Function(x) x.UpCount, .UpCount) _
                                                                                     .Set(Function(x) x.UpPercent, .UpPercentCount) _
                                                                                     .Set(Function(x) x.StatusCode, .StatusCode) _
                                                                                     .Set(Function(x) x.Details, StatusDetails) _
                                                                                     .Set(Function(x) x.SecondaryRole, .SecondaryRole) _
                                                                                     .Set(Function(x) x.Description, Left(.Description, 254)) _
                                                                                     .Set(Function(x) x.DominoServerTasksStatus, .TaskStatus) _
                                                                                     .Set(Function(x) x.LastUpdated, GetFixedDateTime(.LastScan)) _
                                                                                     .Set(Function(x) x.Category, .Category) _
                                                                                     .Set(Function(x) x.ResponseTime, Convert.ToInt32(.ResponseTime)) _
                                                                                     .Set(Function(x) x.PendingThreshold, .PendingThreshold) _
                                                                                     .Set(Function(x) x.DeadThreshold, .DeadThreshold) _
                                                                                     .Set(Function(x) x.HeldMail, .HeldMail) _
                                                                                     .Set(Function(x) x.HeldThreshold, .HeldThreshold) _
                                                                                     .Set(Function(x) x.ResponseThreshold, Convert.ToInt32(.ResponseThreshold)) _
                                                                                     .Set(Function(x) x.UserCount, .UserCount) _
                                                                                     .Set(Function(x) x.CPU, .CPU_Utilization) _
                                                                                     .Set(Function(x) x.CPUthreshold, (.CPU_Threshold / 100)) _
                                                                                     .Set(Function(x) x.Memory, (.MemoryPercentUsed / 100)) _
                                                                                     .Set(Function(x) x.NextScan, GetFixedDateTime(.NextScan)) _
                                                                                     .Set(Function(x) x.UpPercentMinutes, myUpPercent) _
                                                                                     .Set(Function(x) x.MyPercent, Double.Parse(strPercent)) _
                                                                                     .Set(Function(x) x.PercentageChange, PercentageChange) _
                                                                                     .Set(Function(x) x.UpMinutes, .UpMinutes) _
                                                                                     .Set(Function(x) x.DownMinutes, .DownMinutes)


                repository.Upsert(filterDef, updateDef)


            End With
        Catch ex As Exception
            '  WriteAuditEntry(Now.ToString & " Error in Domino module creating SQL statement for " & MyDominoServer.Name & " while updating status table: " & ex.Message)
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error in Domino module creating SQL statement for status table: " & ex.Message)
        End Try


    End Sub

    Private Sub UpdateDominoStats(ByRef DominoServer As MonitoredItems.DominoServer)
        'This sub pulls selected statistics from the in-memory Server instance and writes it to the Access Stat table
        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Updating Daily Statistics Table for disk.")
        'Disk Stats

        Try
            For Each Disk As MonitoredItems.DominoDiskSpace In DominoServer.DiskDrives
                If InStr(DominoServer.Statistics_Disk, Disk.DiskName & ".Free") And Disk.DiskName <> "" Then
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Updating " & Disk.DiskName)
                    UpdateDominoDailyStatTable(DominoServer, Disk.DiskName & ".Free", ParseNumericStatValue(Disk.DiskName & ".Free", DominoServer.Statistics_Disk))
                End If
            Next
        Catch ex As Exception

        End Try


        'Memory
        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Updating Daily Statistics Table for Memory.")

        Try
            If InStr(DominoServer.Statistics_Memory, "Mem.Free") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Mem.Free", ParseNumericStatValue("Mem.Free", DominoServer.Statistics_Memory))
            ElseIf InStr(DominoServer.Statistics_All, "Mem.Free") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Mem.Free", ParseNumericStatValue("Mem.Free", DominoServer.Statistics_All))

            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Mem. statistics: " & ex.Message)

        End Try

        'Platform
        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Updating Daily Statistics Table for Platform.")

        Try
            'If InStr(DominoServer.Statistics_All, "Platform.System.PctCombinedCpuUtil") > 0 Then
            '    UpdateDominoDailyStatTable(DominoServer.Name, "Platform.System.PctCombinedCpuUtil", ParseNumericStatValue("Platform.System.PctCombinedCpuUtil", DominoServer.Statistics_All))
            'ElseIf InStr(DominoServer.Statistics_Platform, "Platform.System.PctCombinedCpuUtil") > 0 Then
            '    UpdateDominoDailyStatTable(DominoServer.Name, "Platform.System.PctCombinedCpuUtil", ParseNumericStatValue("Platform.System.PctCombinedCpuUtil", DominoServer.Statistics_Platform))
            'End If

            If InStr(DominoServer.Statistics_All, "Platform.Memory.KBFree") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Platform.Memory.KBFree", ParseNumericStatValue("Platform.Memory.KBFree", DominoServer.Statistics_All))
            ElseIf InStr(DominoServer.Statistics_Platform, "Platform.System.PctCombinedCpuUtil") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Platform.Memory.KBFree", ParseNumericStatValue("Platform.Memory.KBFree", DominoServer.Statistics_Platform))
            End If

            If InStr(DominoServer.Statistics_All, "Platform.Memory.RAM.AvailMBytes") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Platform.Memory.RAM.AvailMBytes", ParseNumericStatValue("Platform.Memory.RAM.AvailMBytes", DominoServer.Statistics_All))
            ElseIf InStr(DominoServer.Statistics_Platform, "Platform.Memory.RAM.AvailMBytes") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Platform.Memory.RAM.AvailMBytes", ParseNumericStatValue("Platform.Memory.RAM.AvailMBytes", DominoServer.Statistics_Platform))
            End If

            If InStr(DominoServer.Statistics_All, "Platform.Memory.RAM.TotalMBytes") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Platform.Memory.RAM.TotalMBytes", ParseNumericStatValue("Platform.Memory.RAM.TotalMBytes", DominoServer.Statistics_All))
            ElseIf InStr(DominoServer.Statistics_Platform, "Platform.Memory.RAM.TotalMBytes") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Platform.Memory.RAM.TotalMBytes", ParseNumericStatValue("Platform.Memory.RAM.TotalMBytes", DominoServer.Statistics_Platform))
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Platform statistics: " & ex.Message)
        End Try

        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Updating Daily Statistics Table for Replication.")

        'Replication
        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Failed") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Replica.Failed", ParseNumericStatValue("Replica.Failed", DominoServer.Statistics_Replica))
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Replication statistics: " & ex.Message)

        End Try

        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Updating Daily Statistics Table for Cluster.")

        'Cluster Replication
        Try
            If DominoServer.ClusterMember <> "" Then
                DominoClusterStats(DominoServer)
            End If
        Catch ex As Exception

        End Try


        'Load and availability
        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Updating Daily Statistics Table for server load and availability.")
        Try
            If InStr(DominoServer.Statistics_Server, "Server.Users") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Server.Users", ParseNumericStatValue("Server.Users", DominoServer.Statistics_Server))
            End If

            If InStr(DominoServer.Statistics_Server, "Server.Trans.PerMinute") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Server.Trans.PerMinute", ParseNumericStatValue("Server.Trans.PerMinute", DominoServer.Statistics_Server))
            End If

            If InStr(DominoServer.Statistics_Server, "Server.AvailabilityIndex") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Server.AvailabilityIndex", ParseNumericStatValue("Server.AvailabilityIndex", DominoServer.Statistics_Server))
            End If



        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Server statistics: " & ex.Message)
        End Try


        Try
            If InStr(DominoServer.Statistics_Mail, "Mail.TotalPending") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Mail.TotalPending", ParseNumericStatValue("Mail.TotalPending", DominoServer.Statistics_Mail))
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Mail.TotalPending statistics: " & ex.Message)
        End Try

        Try

            If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.1.PctUtil") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Platform.LogicalDisk.1.PctUtil", ParseNumericStatValue("Platform.LogicalDisk.1.PctUtil", DominoServer.Statistics_Platform))
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
        End Try

        Try

            If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.1.AvgQueueLen") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Platform.LogicalDisk.1.AvgQueueLen", ParseNumericStatValue("Platform.LogicalDisk.1.AvgQueueLen", DominoServer.Statistics_Platform))
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
        End Try

        Try

            If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.2.AvgQueueLen") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Platform.LogicalDisk.2.AvgQueueLen", ParseNumericStatValue("Platform.LogicalDisk.2.AvgQueueLen", DominoServer.Statistics_Platform))
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
        End Try


        Try

            If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.2.PctUtil") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Platform.LogicalDisk.2.PctUtil", ParseNumericStatValue("Platform.LogicalDisk.2.PctUtil", DominoServer.Statistics_Platform))
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
        End Try

        Try

            If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.3.PctUtil") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Platform.LogicalDisk.3.PctUtil", ParseNumericStatValue("Platform.LogicalDisk.3.PctUtil", DominoServer.Statistics_Platform))
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
        End Try


        Try

            If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.3.AvgQueueLen") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Platform.LogicalDisk.3.AvgQueueLen", ParseNumericStatValue("Platform.LogicalDisk.3.AvgQueueLen", DominoServer.Statistics_Platform))
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
        End Try

        Try

            If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.4.PctUtil") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Platform.LogicalDisk.4.PctUtil", ParseNumericStatValue("Platform.LogicalDisk.4.PctUtil", DominoServer.Statistics_Platform))
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
        End Try


        Try

            If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.4.AvgQueueLen") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Platform.LogicalDisk.4.AvgQueueLen", ParseNumericStatValue("Platform.LogicalDisk.4.AvgQueueLen", DominoServer.Statistics_Platform))
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
        End Try


        Try

            If InStr(DominoServer.Statistics_Mail, "Mail.Mailbox.AccessConflicts") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Mail.Mailbox.AccessConflicts", ParseNumericStatValue("Mail.Mailbox.AccessConflicts", DominoServer.Statistics_Mail))
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
        End Try


        Try

            If InStr(DominoServer.Statistics_Mail, "Mail.Mailbox.Accesses") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Mail.Mailbox.Accesses", ParseNumericStatValue("Mail.Mailbox.Accesses", DominoServer.Statistics_Mail))
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
        End Try

        Try
            If InStr(DominoServer.Statistics_HTTP, "Http.CurrentConnections") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Http.CurrentConnections", ParseNumericStatValue("Http.CurrentConnections", DominoServer.Statistics_HTTP))
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
        End Try

        Try
            If InStr(DominoServer.Statistics_HTTP, "Http.Worker.Total.QuickPlace.Requests") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Http.Worker.Total.QuickPlace.Requests", ParseNumericStatValue("Http.Worker.Total.QuickPlace.Requests", DominoServer.Statistics_HTTP))
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
        End Try


    End Sub

    Private Sub UpdateDominoResponseTimeTable(ByRef MyDominoServer As MonitoredItems.DominoServer, ByVal ResponseTime As Double)

        Dim strSQL As String = ""
        Dim MyWeekNumber As Integer
        '10/14/2015 NS modified for VSPLUS-2085
        Dim nowDate As DateTime = Now
        MyWeekNumber = GetWeekNumber(nowDate)

        Try

            UpdateDominoDailyStatTable(MyDominoServer, "ResponseTime", ResponseTime)

        Catch ex As Exception
            Debug.WriteLine(Now.ToString & " update Domino Response Stats table insert failed because: " & ex.Message)
            Debug.WriteLine(Now.ToString & " The failed stats table insert command was " & strSQL)
        Finally
            'myConnection.Close()
            'myConnection.Dispose()
            'myCommand.Dispose()
        End Try
        strSQL = Nothing
        MyWeekNumber = Nothing
        nowDate = Nothing

    End Sub

    Private Sub DominoClusterStats(ByRef DominoServer As MonitoredItems.DominoServer)
        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.WorkQueueDepth") > 0 Then
                Dim myDepth As Integer = ParseNumericStatValue("Replica.Cluster.WorkQueueDepth", DominoServer.Statistics_Replica)
                DominoServer.ReplicaClusterWorkQueueDepth = myDepth
                UpdateDominoDailyStatTable(DominoServer, "Replica.Cluster.WorkQueueDepth", myDepth)
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Replica.Cluster.WorkQueueDepth is " & myDepth.ToString)
                'Else
                '    Dim myDepth As Integer = CInt(OLDDominoNumericStatistic(DominoServer.Name, "Replica.Cluster.WorkQueueDepth"))
                '    DominoServer.ReplicaClusterSecondsOnQueue = myDepth
                '    UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.WorkQueueDepth", myDepth)
                '    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Replica.Cluster.WorkQueueDepth is " & myDepth.ToString)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Replica.Cluster.WorkQueueDepth: " & ex.Message)
        End Try

        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Updating Daily Statistics Table for Replica.Cluster.WorkQueueDepth.Avg.")
        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.WorkQueueDepth.Avg") > 0 Then
                Dim myDepth As Integer = ParseNumericStatValue("Replica.Cluster.WorkQueueDepth.Avg", DominoServer.Statistics_Replica)
                DominoServer.ReplicaClusterWorkQueueDepthAvg = myDepth
                UpdateDominoDailyStatTable(DominoServer, "Replica.Cluster.WorkQueueDepth.Avg", myDepth)
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Replica.Cluster.WorkQueueDepth.Avg is " & myDepth.ToString)
                'Else
                '    Dim myDepth As Integer = CInt(OLDDominoNumericStatistic(DominoServer.Name, "Replica.Cluster.WorkQueueDepth.Avg"))
                '    DominoServer.ReplicaClusterSecondsOnQueue = myDepth
                '    UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.WorkQueueDepth.Avg", myDepth)
                '    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Replica.Cluster.WorkQueueDepth.Avg is " & myDepth.ToString)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Replica.Cluster.WorkQueueDepthAvg: " & ex.Message)
        End Try

        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.WorkQueueDepth.Max") > 0 Then
                Dim myDepth As Integer = ParseNumericStatValue("Replica.Cluster.WorkQueueDepth.Max", DominoServer.Statistics_Replica)
                DominoServer.ReplicaClusterWorkQueueDepthMax = myDepth
                UpdateDominoDailyStatTable(DominoServer, "Replica.Cluster.WorkQueueDepth.Max", myDepth)
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Replica.Cluster.WorkQueueDepth.Max is " & myDepth.ToString)
                'Else
                '    Dim myDepth As Integer = CInt(OLDDominoNumericStatistic(DominoServer.Name, "Replica.Cluster.WorkQueueDepth.Max"))
                '    DominoServer.ReplicaClusterSecondsOnQueue = myDepth
                '    UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.WorkQueueDepth.Max", myDepth)
                '    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Replica.Cluster.WorkQueueDepth.Max is " & myDepth.ToString)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Replica.Cluster.WorkQueueDepth.Max: " & ex.Message)
        End Try
        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Updating Daily Statistics Table for Replica.Cluster.SecondsOnQueue.")

        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.SecondsOnQueue") > 0 Then
                Dim myDepth As Integer = ParseNumericStatValue("Replica.Cluster.SecondsOnQueue", DominoServer.Statistics_Replica)
                DominoServer.ReplicaClusterSecondsOnQueue = myDepth
                UpdateDominoDailyStatTable(DominoServer, "Replica.Cluster.SecondsOnQueue", myDepth)
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Replica.Cluster.SecondsOnQueue is " & myDepth.ToString)
                'Else
                '    Dim myDepth As Integer = CInt(OLDDominoNumericStatistic(DominoServer.Name, "Replica.Cluster.SecondsOnQueue"))
                '    DominoServer.ReplicaClusterSecondsOnQueue = myDepth
                '    UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.SecondsOnQueue", myDepth)
                '    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Replica.Cluster.SecondsOnQueue is " & myDepth.ToString)
            End If
        Catch ex As Exception

            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Replica.Cluster.SecondsOnQueue: " & ex.Message)
        End Try

        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.SecondsOnQueue.Avg") > 0 Then
                Dim myDepth As Integer = ParseNumericStatValue("Replica.Cluster.SecondsOnQueue.Avg", DominoServer.Statistics_Replica)

                DominoServer.ReplicaClusterSecondsOnQueueAvg = myDepth
                UpdateDominoDailyStatTable(DominoServer, "Replica.Cluster.SecondsOnQueue.Avg", myDepth)
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Replica.Cluster.SecondsOnQueueAvg is " & myDepth.ToString)
                'Else
                '    Dim myDepth As Integer = CInt(OLDDominoNumericStatistic(DominoServer.Name, "Replica.Cluster.SecondsOnQueue.Avg"))

                '    DominoServer.ReplicaClusterSecondsOnQueueAvg = myDepth
                '    UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.SecondsOnQueue.Avg", myDepth)
                '    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Replica.Cluster.SecondsOnQueueAvg is " & myDepth.ToString)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Replica.Cluster.SecondsOnQueue.Avg: " & ex.Message)
        End Try

        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.SecondsOnQueue.Max") > 0 Then
                Dim myDepth As Integer = ParseNumericStatValue("Replica.Cluster.SecondsOnQueue.Max", DominoServer.Statistics_Replica)
                DominoServer.ReplicaClusterSecondsOnQueueMax = myDepth
                UpdateDominoDailyStatTable(DominoServer, "Replica.Cluster.SecondsOnQueue.Max", myDepth)
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Replica.Cluster.SecondsOnQueue.Max is " & myDepth.ToString)
                'Else
                '    Dim myDepth As Integer = CInt(OLDDominoNumericStatistic(DominoServer.Name, "Replica.Cluster.SecondsOnQueue.Max"))
                '    DominoServer.ReplicaClusterSecondsOnQueue = myDepth
                '    UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.SecondsOnQueue.Max", myDepth)
                '    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Replica.Cluster.SecondsOnQueue.Max is " & myDepth.ToString)
            End If
        Catch ex As Exception

        End Try

        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.Failed") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Replica.Cluster.Failed", ParseNumericStatValue("Replica.Cluster.Failed", DominoServer.Statistics_Replica))
            End If
        Catch ex As Exception

        End Try

        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.RetryWaiting") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Replica.Cluster.RetryWaiting", ParseNumericStatValue("Replica.Cluster.RetryWaiting", DominoServer.Statistics_Replica))
            End If
        Catch ex As Exception

        End Try

        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.Files.Local") > 0 Then
                UpdateDominoDailyStatTable(DominoServer, "Replica.Cluster.Files.Local", ParseNumericStatValue("Replica.Cluster.Files.Local", DominoServer.Statistics_Replica))
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Cluster Replication statistics: " & ex.Message)
        End Try

    End Sub

    Private Sub UpdateDominoDailyStatTable(ByRef MyDominoServer As MonitoredItems.DominoServer, ByVal StatName As String, ByVal StatValue As Double)

        If StatValue = -999 Then Exit Sub

        '**

        Dim strSQL As String = ""
        Dim MyWeekNumber As Integer
        '10/14/2015 NS modified for VSPLUS-2085
        Dim nowDate As DateTime = Now
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Current date/time: " & nowDate, LogUtilities.LogUtils.LogLevel.Verbose)
        MyWeekNumber = GetWeekNumber(nowDate)
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Week Number: " & MyWeekNumber, LogUtilities.LogUtils.LogLevel.Verbose)

        Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
        Dim entity As New VSNext.Mongo.Entities.DailyStatistics() With {
            .DeviceId = MyDominoServer.ServerObjectID,
            .StatName = StatName,
            .StatValue = StatValue
        }
        repository.Insert(entity)

        Try
            GC.Collect()
        Catch ex As Exception

        End Try
    End Sub

    'Private Sub UpdateDominoDailyMailFileStatTable(ByVal ScanDate As DateTime, ByVal MailServer As String, ByVal FileName As String, ByVal FileTitle As String, ByVal FileSize As Double, ByVal TemplateName As String, ByVal Quota As Double, ByVal FTIndexed As Boolean, ByVal OutOfOfficeAgentEnabled As Boolean, ByVal EnabledForClusterReplication As Boolean, ByVal ReplicaID As String, ByVal ODS As Double)
    '    Dim strSQL As String
    '    If TemplateName = "" Then
    '        TemplateName = "None"
    '    End If
    '    If InStr(FileTitle, "'") Then
    '        FileTitle = FileName
    '    End If

    '    WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " Entered UpdateDominoFileMailFileStatTable with " & ScanDate & ",  " & FileTitle)

    '    Dim objVSAdaptor As New VSAdaptor
    '    Try

    '        Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.MailFiles)(connectionString)
    '        Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.MailFiles) = repository.Filter.Where(Function(x) x.ServerName = MailServer & x.ReplicaID = ReplicaID)
    '        Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.MailFiles) = repository.Updater _
    '                                                                                .Set(Function(x) x.FileSize, FileSize) _
    '                                                                                .Set(Function(x) x.FileTitle, FileTitle) _
    '                                                                                .Set(Function(x) x.MailTemplate, TemplateName) _
    '                                                                                .Set(Function(x) x.Quota, Quota) _
    '                                                                                .Set(Function(x) x.FileName, FileName) _
    '                                                                                .Set(Function(x) x.FTIndexed, FTIndexed) _
    '                                                                                .Set(Function(x) x.OutOfOfficeAgentEnabled, OutOfOfficeAgentEnabled) _
    '                                                                                .Set(Function(x) x.EnabledForClusterReplication, EnabledForClusterReplication) _
    '                                                                                .Set(Function(x) x.ScanDate, GetFixedDateTime(ScanDate))
    '        repository.Upsert(filterDef, updateDef)

    '    Catch ex As Exception
    '        WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " Error UpdateDominoFileMailFileStatTable. Error " & ex.Message.ToString())
    '    End Try

    '    Try
    '        GC.Collect()
    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Private Sub UpdateDominoDailyMailFileSummaryTable(ByVal ScanDate As DateTime, ByVal MailServer As String, ByVal FileCount As Long, ByVal TotalFileSize As Long)
    '    Dim strSQL As String
    '    '**
    '    Dim myPath As String
    '    Dim myRegistry As New RegistryHandler
    '    'Read the registry for the location of the Config Database


    '    Dim myConnection As New Data.OleDb.OleDbConnection
    '    Try
    '        myPath = myRegistry.ReadFromRegistry("Application Path")
    '        ' debug.writeline(Now.ToString & " Domino Update e database " & myPath)
    '    Catch ex As Exception
    '        Debug.WriteLine(Now.ToString & " Failed to read registry in Domino Response Table module. Exception: " & ex.Message)
    '    End Try

    '    If myPath Is Nothing Then
    '        Debug.WriteLine(Now.ToString & " Error: Failed to read registry in Domino Status module.   Cannot locate Config Database 'status.mdb'.  Configure by running" & ProductName & " client before starting the service.")
    '        '   Return False
    '        Exit Sub
    '    End If
    '    myRegistry = Nothing


    '    'Delete any existing data for today, if any
    '    Dim objVSAdaptor As New VSAdaptor
    '    Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.MailFilesSummary)(connectionString)

    '    Try
    '        Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.MailFilesSummary) = repository.Filter.Where(Function(x) x.ScanDate = GetFixedDateTime(ScanDate) And x.ServerName = MailServer)
    '        repository.Delete(filterDef)
    '    Catch ex As Exception
    '        Debug.WriteLine(Now.ToString & " Count Mail Files delete command failed because: " & ex.Message)
    '    End Try

    '    'Insert the new data for today
    '    Try
    '        'somarju

    '        repository.Insert(New VSNext.Mongo.Entities.MailFilesSummary() With {
    '                          .MailFileCount = FileCount,
    '                          .MailFileSize = TotalFileSize,
    '                          .ScanDate = GetFixedDateTime(ScanDate),
    '                          .ServerName = MailServer})

    '    Catch ex As Exception

    '    End Try

    '    Try
    '        strSQL = Nothing
    '        'myConnection.Close()
    '        'myConnection.Dispose()
    '        'myCommand.Dispose()
    '    Catch ex As Exception

    '    End Try

    '    Try
    '        GC.Collect()
    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Private Sub UpdateTraveler9DeviceStatusTable(ByRef device As deviceDetails, ByVal ServerName As String)

    '    WriteDeviceHistoryEntry("All", "Traveler_Users", vbCrLf & "************* INSERTING DATA for Traveler Devices")
    '    '  WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Username: " & device.username)
    '    '   WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Device Provider: " & device.device_provider)
    '    WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Device Level: " & device.device_level)
    '    ' WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Device Type: " & device.device_type)

    '    Dim syncTime As DateTime

    '    Try
    '        Dim x As New DateTime
    '        WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Device Last Sync Time in EPOCH: " & device.last_sync_time)
    '        x = DateTime.Parse("January 1 1970 12:00:00 am")
    '        syncTime = x.AddMilliseconds(CLng(device.last_sync_time))
    '        WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Device Last Sync Time in human: " & syncTime.ToString)
    '    Catch ex As Exception
    '        WriteDeviceHistoryEntry("All", "Traveler_Users", "Error parsing time " & ex.ToString)
    '        syncTime = Now
    '    End Try


    '    If device.device_type.Trim = "" Then Exit Sub

    '    Dim strSQL As String

    '    With device
    '        strSQL = "Update Traveler_Devices SET UserName= '" & .username & _
    '        "', DeviceName= '" & .device_provider & _
    '        "', OS_Type='" & .device_type & _
    '        "', DeviceID='" & .deviceid & _
    '        "', SyncType='" & .notification_type & _
    '        "', LastSyncTime='" & syncTime.ToString & _
    '        "', Client_Build='" & .device_level & _
    '        "', ServerName='" & ServerName & _
    '        "'  WHERE DocID='" & .deviceid & "'"
    '    End With

    '    strSQL = strSQL.Replace("NaN", "1")

    '    WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " SQL Statement is " & strSQL)

    '    Dim strSQLInsert As String

    '    With device
    '        strSQLInsert = "INSERT INTO Traveler_Devices ( DeviceID, Client_Build, ServerName, UserName, DeviceName,  LastSyncTime, OS_Type, DocID, SyncType) " & _
    '             " VALUES ('" & .deviceid & "','" & .device_level & "','" & ServerName & "','" & .username & "','" & .device_provider & "', '" & syncTime.ToString & "', '" & .device_type & "', '" & .deviceid & "', '" & .notification_type & "')"
    '    End With

    '    strSQLInsert = strSQLInsert.Replace("NaN", "1")

    '    WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " SQL Insert Statement is " & strSQLInsert)
    '    Dim objVSAdaptor As New VSAdaptor
    '    Try
    '        If objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL) = 0 Then
    '            objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQLInsert)
    '        End If
    '    Catch ex As Exception
    '        WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & "  Error updating Traveler Device table: " & ex.ToString)
    '    End Try



    '    Try
    '        Call GC.Collect()
    '    Catch ex As Exception

    '    End Try


    'End Sub

    'Private Sub EmptyTravelerDeviceTable(ByVal ServerName As String)
    '    Dim strSQL As String
    '    Try
    '        strSQL = "Delete FROM Traveler_Devices WHERE ServerName='" & ServerName & "'"
    '        Dim objVSAdaptor As New VSAdaptor
    '        objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)

    '        Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.MobileDevices)(connectionString)
    '        Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.MobileDevices) = repository.Filter.Where(Function(x) x.ServerName = ServerName)
    '        repository.Delete(filterDef)
    '    Catch ex As Exception

    '    End Try

    'End Sub

    'Private Sub updateTravelerDeviceCollection(ByVal MoreInfoSQLs As List(Of SqlCommand))
    '    For Each t As SqlCommand In MoreInfoSQLs
    '        Dim objVSAdaptor As New VSAdaptor
    '        Try
    '            objVSAdaptor.ExecuteNonQuerySQLParams("VitalSigns", t)
    '            'WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Updated the data using the SQL = " & t)
    '        Catch ex As Exception
    '            WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & "  Error updating Traveler Device table: " & ex.ToString)
    '        End Try
    '    Next

    'End Sub
    Private Sub addTravelerDeviceMoreInfoToCollection(ByRef Device As TravelerDevice)
        If Device.DeviceName.Trim = "" Then Exit Sub

        'Device2 is made so Device can be used in lamda expressions
        Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.MobileDevices)(connectionString)
        Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.MobileDevices) = repository.Filter.Eq(Function(x) x.DeviceID, Device.DeviceID.ToString()) _
                                                                                    And repository.Filter.Eq(Function(x) x.ServerName, Device.ServerName.ToString())
        Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.MobileDevices) = repository.Updater _
                                                                                    .Set(Function(x) x.UserName, Device.UserName.ToString()) _
                                                                                    .Set(Function(x) x.DeviceName, Device.DeviceName.ToString()) _
                                                                                    .Set(Function(x) x.OSType, Device.OS_Type.ToString()) _
                                                                                    .Set(Function(x) x.ClientBuild, Device.Client_Build.ToString()) _
                                                                                    .Set(Function(x) x.Access, Device.UserName.ToString()) _
                                                                                    .Set(Function(x) x.WipeSupported, Device.wipeSupported.ToString()) _
                                                                                    .Set(Function(x) x.SyncType, Device.AutoSyncType.ToString()) _
                                                                                    .Set(Function(x) x.SecurityPolicy, Device.ApprovalPolicy.ToString()) _
                                                                                    .Set(Function(x) x.Approval, Device.Approval.ToString()) _
                                                                                    .Set(Function(x) x.LastSyncTime, GetFixedDateTime(Device.LastSyncTime)) _
                                                                                    .Set(Function(x) x.IsMoreDetailsFetched, True)


        Try
            repository.Update(filterDef, updateDef)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", "TravelerMoreDetails", Now.ToString & "  Error updating Traveler Device More info for DeviceId: " & Device.DeviceID.ToString() & ". Exception:" & ex.ToString)
        End Try
        'If objVSAdaptor.ExecuteNonQuerySQLParams("VitalSigns", returnTDSQLCommand(sqlcmd, Device, ServerName, HAPoolName)) = 0 Then
    End Sub

    Private Sub UpdateTravelerDeviceStatusTable(ByVal Device As TravelerDevice, ByVal ServerName As String, HAPoolName As String)
        '*************************************************************
        'Update the Traveler Table
        '*************************************************************

        'The following line prevents empty entries from being created
        If Device.DeviceName.Trim = "" Then Exit Sub

        Try
            'This function puts all the colors into English so the graphs group better
            'VSPlus-532
            If Not (InStr(Device.DeviceName, "White")) And Not (InStr(Device.DeviceName, "Black")) Then
                Device.DeviceName = EnglishColors(Device.DeviceName)
            End If
        Catch ex As Exception

        End Try

        Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.MobileDevices)(connectionString)
        Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.MobileDevices) = repository.Filter.Where(Function(x) x.DeviceID = Device.DeviceID And x.ServerName = ServerName)
        Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.MobileDevices) = repository.Updater _
                                                                                    .Set(Function(x) x.UserName, Device.UserName) _
                                                                                    .Set(Function(x) x.DeviceName, Device.DeviceName) _
                                                                                    .Set(Function(x) x.ConnectionState, Device.ConnectionState) _
                                                                                    .Set(Function(x) x.OSType, Device.OS_Type) _
                                                                                    .Set(Function(x) x.OSTypeMin, Device.OS_Type_Min) _
                                                                                    .Set(Function(x) x.ClientBuild, Device.Client_Build) _
                                                                                    .Set(Function(x) x.SyncType, Device.AutoSyncType) _
                                                                                    .Set(Function(x) x.LastSyncTime, GetFixedDateTime(Device.LastSyncTime)) _
                                                                                    .Set(Function(x) x.MoreDetailsUrl, Device.href) _
                                                                                    .Set(Function(x) x.ModifiedOn, GetFixedDateTime(Now)) _
                                                                                    .Set(Function(x) x.HAPool, HAPoolName)


        Try
            repository.Upsert(filterDef, updateDef)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & "  Error updating Traveler Device table: " & ex.ToString)
        End Try

        Try
            Call GC.Collect()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub UpdateTravelerDeviceStatusTempTable(ByVal Device As TravelerDevice, ByVal ServerName As String, HAPoolName As String, myConnection As SqlClient.SqlConnection)
        '*************************************************************
        'Update the TravelerTemp Table
        '*************************************************************

        'The following line prevents empty entries from being created
        If Device.DeviceName.Trim = "" Then Exit Sub

        Try
            'This function puts all the colors into English so the graphs group better
            'VSPlus-532
            If Not (InStr(Device.DeviceName, "White")) And Not (InStr(Device.DeviceName, "Black")) Then
                Device.DeviceName = EnglishColors(Device.DeviceName)
            End If
        Catch ex As Exception

        End Try

        Dim strSQL As String = ""
        Dim sqlcmd As New SqlCommand("Update Traveler_Devices_temp SET UserName=@UserName" & _
        ", DeviceName=@DeviceName" & _
        ", OS_Type=@OS_Type" & _
        ", OS_Type_Min=@OS_Type_Min" & _
        ", Client_Build=@Client_Build" & _
        ", SyncType=@SyncType" & _
        ", LastSyncTime=@LastSyncTime" & _
        ", MoreDetailsURL=@MoreDetailsURL" & _
        ", LastUpdated=@LastUpdated" & _
        ", HAPoolName=@HAPoolName" & _
        "  WHERE DeviceID=@DeviceID AND ServerName=@SN")

        Try
            sqlcmd.Parameters.Add(New SqlParameter("@Client_Build", SqlDbType.NVarChar, 510))
            sqlcmd.Parameters("@Client_Build").Value = Device.Client_Build.ToString()

            sqlcmd.Parameters.Add(New SqlParameter("@DeviceID", SqlDbType.NVarChar, 300))
            sqlcmd.Parameters("@DeviceID").Value = Device.DeviceID.ToString()

            sqlcmd.Parameters.Add(New SqlParameter("@SN", SqlDbType.NVarChar, 510))
            sqlcmd.Parameters("@SN").Value = ServerName.ToString()

            sqlcmd.Parameters.Add(New SqlParameter("@UserName", SqlDbType.NVarChar, 510))
            sqlcmd.Parameters("@UserName").Value = Device.UserName.ToString()

            sqlcmd.Parameters.Add(New SqlParameter("@DeviceName", SqlDbType.NVarChar, 510))
            sqlcmd.Parameters("@DeviceName").Value = Device.DeviceName.ToString()

            sqlcmd.Parameters.Add(New SqlParameter("@LastSyncTime", SqlDbType.DateTime))
            sqlcmd.Parameters("@LastSyncTime").Value = FixDateTime(Device.LastSyncTime)

            sqlcmd.Parameters.Add(New SqlParameter("@OS_Type", SqlDbType.NVarChar, 510))
            sqlcmd.Parameters("@OS_Type").Value = Device.OS_Type.ToString()

            sqlcmd.Parameters.Add(New SqlParameter("@OS_Type_Min", SqlDbType.NVarChar, 510))
            sqlcmd.Parameters("@OS_Type_Min").Value = Device.OS_Type_Min.ToString()

            sqlcmd.Parameters.Add(New SqlParameter("@SyncType", SqlDbType.NVarChar, 510))
            sqlcmd.Parameters("@SyncType").Value = Device.AutoSyncType.ToString()

            sqlcmd.Parameters.Add(New SqlParameter("@MoreDetailsURL", SqlDbType.NVarChar, 1000))
            sqlcmd.Parameters("@MoreDetailsURL").Value = Device.href.ToString()

            sqlcmd.Parameters.Add(New SqlParameter("@LastUpdated", SqlDbType.DateTime))
            sqlcmd.Parameters("@LastUpdated").Value = FixDateTime(Now)

            sqlcmd.Parameters.Add(New SqlParameter("@HAPoolName", SqlDbType.VarChar, 150))
            If HAPoolName = "" Then
                sqlcmd.Parameters("@HAPoolName").Value = ""
            Else
                sqlcmd.Parameters("@HAPoolName").Value = HAPoolName.ToString()
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Exception creating device parameters" & ex.Message.ToString())
        End Try
        Dim strSQLInsert As String = ""
        Try
            'strSQLInsert = strSQLInsert.Replace("NaN", "1")
            Dim objVSAdaptor As New VSAdaptor
            'If objVSAdaptor.ExecuteNonQuerySQLParams("VitalSigns", returnTDSQLCommand(sqlcmd, Device, ServerName, HAPoolName)) = 0 Then
            If objVSAdaptor.ExecuteNonQuerySQLParams(sqlcmd, myConnection) = 0 Then
                WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " user not found with deviceid.Inserting new record: device id:" & Device.DeviceID)
                sqlcmd = New SqlCommand("INSERT INTO Traveler_Devices_temp (Client_Build, DeviceID, ServerName, UserName, DeviceName, LastSyncTime, OS_Type,  OS_Type_Min, SyncType, MoreDetailsURL,LastUpdated,HAPoolName) " & _
                " VALUES (@Client_Build,@DeviceID,@SN,@UserName,@DeviceName,@LastSyncTime,@OS_Type,@OS_Type_Min,@SyncType,@MoreDetailsURL,@LastUpdated,@HAPoolName)")

                sqlcmd.Parameters.Add(New SqlParameter("@Client_Build", SqlDbType.NVarChar, 510))
                sqlcmd.Parameters("@Client_Build").Value = Device.Client_Build.ToString()

                sqlcmd.Parameters.Add(New SqlParameter("@DeviceID", SqlDbType.NVarChar, 300))
                sqlcmd.Parameters("@DeviceID").Value = Device.DeviceID.ToString()

                sqlcmd.Parameters.Add(New SqlParameter("@SN", SqlDbType.NVarChar, 510))
                sqlcmd.Parameters("@SN").Value = ServerName.ToString()

                sqlcmd.Parameters.Add(New SqlParameter("@UserName", SqlDbType.NVarChar, 510))
                sqlcmd.Parameters("@UserName").Value = Device.UserName.ToString()

                sqlcmd.Parameters.Add(New SqlParameter("@DeviceName", SqlDbType.NVarChar, 510))
                sqlcmd.Parameters("@DeviceName").Value = Device.DeviceName.ToString()

                sqlcmd.Parameters.Add(New SqlParameter("@LastSyncTime", SqlDbType.DateTime))
                sqlcmd.Parameters("@LastSyncTime").Value = FixDateTime(Device.LastSyncTime)

                sqlcmd.Parameters.Add(New SqlParameter("@OS_Type", SqlDbType.NVarChar, 510))
                sqlcmd.Parameters("@OS_Type").Value = Device.OS_Type.ToString()

                sqlcmd.Parameters.Add(New SqlParameter("@OS_Type_Min", SqlDbType.NVarChar, 510))
                sqlcmd.Parameters("@OS_Type_Min").Value = Device.OS_Type_Min.ToString()

                sqlcmd.Parameters.Add(New SqlParameter("@SyncType", SqlDbType.NVarChar, 510))
                sqlcmd.Parameters("@SyncType").Value = Device.AutoSyncType.ToString()

                sqlcmd.Parameters.Add(New SqlParameter("@MoreDetailsURL", SqlDbType.NVarChar, 1000))
                sqlcmd.Parameters("@MoreDetailsURL").Value = Device.href.ToString()

                sqlcmd.Parameters.Add(New SqlParameter("@LastUpdated", SqlDbType.DateTime))
                sqlcmd.Parameters("@LastUpdated").Value = FixDateTime(Now)

                sqlcmd.Parameters.Add(New SqlParameter("@HAPoolName", SqlDbType.VarChar, 150))
                If HAPoolName = "" Then
                    sqlcmd.Parameters("@HAPoolName").Value = ""
                Else
                    sqlcmd.Parameters("@HAPoolName").Value = HAPoolName.ToString()
                End If

                'WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " user not found with deviceid.Inserting new record: update SQL:" & strSQL)
                'objVSAdaptor.ExecuteNonQuerySQLParams("VitalSigns", returnTDSQLCommand(sqlcmd, Device, ServerName, HAPoolName))
                objVSAdaptor.ExecuteNonQuerySQLParams(sqlcmd, myConnection)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & "  Error updating Traveler Device Temp table: " & ex.ToString)
        End Try

        Try
            Call GC.Collect()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub UpdateTravelerDeviceStatusMongoCollection(ByVal Device As TravelerDevice, ByVal ServerName As String, HAPoolName As String, ByRef list As List(Of VSNext.Mongo.Entities.MobileDevices))
        '*************************************************************
        'Update the TravelerTemp Table
        '*************************************************************

        'The following line prevents empty entries from being created
        If Device.DeviceName.Trim = "" Then Exit Sub

        Try
            'This function puts all the colors into English so the graphs group better
            'VSPlus-532
            If Not (InStr(Device.DeviceName, "White")) And Not (InStr(Device.DeviceName, "Black")) Then
                Device.DeviceName = EnglishColors(Device.DeviceName)
            End If
        Catch ex As Exception

        End Try

        Try
            Dim entity As VSNext.Mongo.Entities.MobileDevices = List.Find(Function(x) x.DeviceID = Device.DeviceID)

            If entity IsNot Nothing Then

                entity.ClientBuild = Device.Client_Build
                entity.ServerName = ServerName
                entity.UserName = Device.UserName
                entity.DeviceName = Device.DeviceName
                entity.LastSyncTime = (Device.LastSyncTime)
                entity.OSType = Device.OS_Type
                entity.OSTypeMin = Device.OS_Type_Min
                entity.SyncType = Device.AutoSyncType
                entity.Href = Device.href
                entity.LastUpdated = (Now)
                entity.HAPool = HAPoolName

            Else

                List.Add(New VSNext.Mongo.Entities.MobileDevices With {
                    .Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                    .DeviceID = Device.DeviceID,
                    .ClientBuild = Device.Client_Build,
                    .ServerName = ServerName,
                    .UserName = Device.UserName,
                    .DeviceName = Device.DeviceName,
                    .LastSyncTime = (Device.LastSyncTime),
                    .OSType = Device.OS_Type,
                    .OSTypeMin = Device.OS_Type_Min,
                    .SyncType = Device.AutoSyncType,
                    .Href = Device.href,
                    .LastUpdated = (Now),
                    .HAPool = HAPoolName
                })

            End If



        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & ServerName, ex.Message, LogLevel.Normal)
        End Try

    End Sub

    Private Sub AddDevicesToTempTable(ByVal ServerName As String, ByRef dt As DataTable, myConnection As SqlClient.SqlConnection)
        '*************************************************************
        ' delete from main table, insert into main from temp and delete from temp
        '*************************************************************
        Try
            For Each row As DataRow In dt.Rows()
                If String.IsNullOrWhiteSpace(row("LastSyncTime").ToString()) Then
                    row("LastSyncTime") = Nothing
                End If
                If String.IsNullOrWhiteSpace(row("LastUpdated").ToString()) Then
                    row("LastUpdated") = Nothing
                End If
            Next

            WriteDeviceHistoryEntry("All", "Traveler_Users_" & ServerName, Now.ToString & " Rows: " & dt.Rows.Count.ToString(), LogUtilities.LogUtils.LogLevel.Verbose)


            Dim n As Long = Now.Ticks

            Dim cmd As New SqlCommand("UpdateTravelerTempTableTVP", myConnection)
            cmd.Parameters.AddWithValue("@tbl", dt)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 60 * 8
            Dim k As Integer = cmd.ExecuteNonQuery()

            n = Now.Ticks - n
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & ServerName, Now.ToString & " Time: " & (New TimeSpan(n).TotalSeconds.ToString()), LogUtilities.LogUtils.LogLevel.Verbose)

        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & ServerName, Now.ToString & " Error in AddDevicesToTempTable: " & ex.Message)
        End Try

    End Sub

    Private Sub AddDevicesToCollection(ByVal ServerName As String, ByRef list As List(Of VSNext.Mongo.Entities.MobileDevices), ByRef repository As VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.MobileDevices))
        '*************************************************************
        ' delete from main table, insert into main from temp and delete from temp
        '*************************************************************
        Try


            WriteDeviceHistoryEntry("All", "Traveler_Users_" & ServerName, Now.ToString & " Devices: " & list.Count.ToString(), LogUtilities.LogUtils.LogLevel.Verbose)


            Dim n As Long = Now.Ticks

            repository.Replace(list, New UpdateOptions() With {.IsUpsert = True})

            n = Now.Ticks - n

            WriteDeviceHistoryEntry("All", "Traveler_Users_" & ServerName, Now.ToString & " Time: " & (New TimeSpan(n).TotalSeconds.ToString()), LogUtilities.LogUtils.LogLevel.Verbose)

        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & ServerName, Now.ToString & " Error in AddDevicesToTempTable: " & ex.Message)
        End Try

    End Sub



    Private Sub CleanupTravelerDeviceStatusTable(ByVal ServerName As String)
        '*************************************************************
        ' delete from main table, insert into main from temp and delete from temp
        '*************************************************************
        Dim strSQL As String
        Try
            'The if exists clause makes sure that we only delete from the primary table if we have new data we can insert in its place
            strSQL = "if exists(select * from Traveler_Devices_temp WHERE ServerName='" & ServerName + "') Delete from Traveler_Devices  WHERE  ServerName='" & ServerName + "'"

            Dim objVSAdaptor As New VSAdaptor
            objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
        Catch ex As Exception
        End Try

        Dim strSQLInsert As String
        Try
            strSQLInsert = "INSERT INTO Traveler_Devices (Client_Build, DeviceID, ServerName, UserName, DeviceName, LastSyncTime, OS_Type,  OS_Type_Min, SyncType, MoreDetailsURL,LastUpdated,HAPoolName,ConnectionState,Access,wipeSupported,Security_Policy,Approval,IsMoreDetailsFetched, IsActive) " & _
              "select Client_Build, DeviceID, ServerName, UserName, DeviceName, LastSyncTime, OS_Type,  OS_Type_Min, SyncType, MoreDetailsURL,LastUpdated,HAPoolName,ConnectionState,Access,wipeSupported,Security_Policy,Approval,IsMoreDetailsFetched,IsActive from Traveler_Devices_temp  WHERE ServerName='" & ServerName + "'"

            Dim objVSAdaptor As New VSAdaptor
            objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQLInsert)
        Catch ex As Exception

        End Try



        'Dim strSQL1 As String
        'Try
        '	strSQL1 = "Delete from Traveler_Devices_temp  WHERE ServerName='" & ServerName + "'"
        '	Dim objVSAdaptor As New VSAdaptor
        '	objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL1)
        'Catch ex As Exception

        'End Try



    End Sub

    Private Sub CleanupTravelerDeviceStatusTable(ByVal ServerName As String, myConnection As SqlClient.SqlConnection)
        '*************************************************************
        ' delete from main table, insert into main from temp and delete from temp
        '*************************************************************

        Try
            Dim n As Long = Now.Ticks

            Dim cmd As New SqlCommand("MoveTravelerTempTable", myConnection)
            cmd.Parameters.AddWithValue("@ServerName", ServerName)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 60 * 5
            Dim k As Integer = cmd.ExecuteNonQuery()

            n = Now.Ticks - n
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & ServerName, Now.ToString & " Time: " & (New TimeSpan(n).TotalSeconds.ToString()), LogUtilities.LogUtils.LogLevel.Verbose)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & ServerName, Now.ToString & " Error in CleanupTravelerDeviceStatusTable: " & ex.Message)
        End Try

    End Sub

    Private Sub CleanupTravelerDeviceStatusCollection(ByVal ServerName As String, ByRef repository As VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.MobileDevices), ByRef StartOfScan As DateTime)
        '*************************************************************
        ' delete from main table, insert into main from temp and delete from temp
        '*************************************************************

        Try
            Dim n As Long = Now.Ticks

            repository.Delete(repository.Filter.Lt(Function(x) x.LastUpdated, StartOfScan))

            n = Now.Ticks - n
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & ServerName, Now.ToString & " Time: " & (New TimeSpan(n).TotalSeconds.ToString()), LogUtilities.LogUtils.LogLevel.Verbose)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & ServerName, Now.ToString & " Error in CleanupTravelerDeviceStatusTable: " & ex.Message)
        End Try

    End Sub


    Public Sub SetActiveDevices()


        Try
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.MobileDevices)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.MobileDevices)
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.MobileDevices)


            filterDef = repo.Filter.In(Function(x) x.ServerName, MyDominoServers.Cast(Of MonitoredItems.DominoServer)().Select(Function(x) x.Name))
            Dim list As List(Of VSNext.Mongo.Entities.MobileDevices) = repo.Collection.Find(filterDef) _
                    .Project((New ProjectionDefinitionBuilder(Of VSNext.Mongo.Entities.MobileDevices)() _
                    .Include(Function(x) x.DeviceID) _
                    .Include(Function(x) x.LastSyncTime) _
                    .Include(Function(x) x.Id) _
                    .Include(Function(x) x.ServerName))) _
                    .ToList() _
                    .Select(Function(x) MongoDB.Bson.Serialization.BsonSerializer.Deserialize(Of VSNext.Mongo.Entities.MobileDevices)(x)) _
                    .ToList()

            repo.Collection.Aggregate.SortBy(Function(x) x.CreatedOn).First()
            Dim activeList As List(Of VSNext.Mongo.Entities.MobileDevices) = list _
                    .GroupBy(Function(x) x.ServerName) _
                    .Select(Function(x) x.Aggregate((Function(max, cur) IIf(max Is Nothing Or cur.LastSyncTime.Value > max.LastSyncTime.Value, cur, max))))




            filterDef = repo.Filter.In(Function(x) x.Id, activeList.Select(Function(y) y.Id.ToString()).ToList()) 
            updateDef = repo.Updater.Set(Function(x) x.IsActive, True)

            repo.Update(filterDef, updateDef)



        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Exception in Setting ActiveDevices: " & ex.Message)
        End Try

    End Sub
    Private Sub UpdateTravelerServerStatusTable(ByVal TravelerVersion As String, ByVal HTTP_MaxConfiguredSessions As Integer, ByVal HTTP_PeakSessions As Integer, ByVal HTTP_Status As String, ByVal HTTP_Details As String, ByVal ServerName As String, ByVal Status As String, ByVal Details As String, ByVal Users As Integer, ByVal IncrementalSyncs As Integer)
        '*************************************************************
        'Update the Status Table
        '*************************************************************
        Dim strSQL As String
        WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Updating traveler status table.")

        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.TypeAndName.Equals(ServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription()))
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                 .Set(Function(x) x.TravelerStatus, Status) _
                                                                                 .Set(Function(x) x.TravelerDetails, Details) _
                                                                                 .Set(Function(x) x.TravelerUsers, Users) _
                                                                                 .Set(Function(x) x.TravelerVersion, TravelerVersion) _
                                                                                 .Set(Function(x) x.HttpPeakConnections, HTTP_PeakSessions) _
                                                                                 .Set(Function(x) x.HttpMaxConfiguredConnections, HTTP_MaxConfiguredSessions) _
                                                                                 .Set(Function(x) x.HttpStatus, HTTP_Status) _
                                                                                 .Set(Function(x) x.HttpDetails, HTTP_Details)

            repository.Upsert(filterDef, updateDef)



        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & "  Error updating Traveler Status: " & ex.ToString)
        End Try

    End Sub

    Private Sub UpdateTravelerDeviceMoreDetails(ByRef Device As TravelerDevice)
        '*************************************************************
        'Update the Traveler Table
        '*************************************************************

        'The following line prevents empty entries from being created
        If Device.DeviceName.Trim = "" Then Exit Sub

        Dim strSQL As String
        With Device
            strSQL = "Update Traveler_Devices SET UserName= '" & .UserName & _
            "', DeviceName= '" & .DeviceName & _
            "', ConnectionState='" & .ConnectionState & _
            "', OS_Type='" & .OS_Type & _
            "', OS_Type_Min='" & .OS_Type_Min & _
            "', Client_Build='" & .Client_Build & _
            "', Access='" & .Access & _
            "', wipeSupported='" & .wipeSupported & _
            "', SyncType='" & .AutoSyncType & _
            "', Security_Policy='" & .ApprovalPolicy & _
            "', Approval='" & .Approval & _
            "', LastSyncTime='" & .LastSyncTime & _
            "', IsMoreDetailsFetched=1" & _
            ", LastUpdated='" + FixDateTime(Now) & _
            "'  WHERE DeviceID='" & .DeviceID & "'"
        End With

        strSQL = strSQL.Replace("NaN", "1")
        WriteDeviceHistoryEntry("Domino", "TravelerMoreDetails", Now.ToString & "  SQL to update more details: " & strSQL)
        Dim objVSAdaptor As New VSAdaptor
        Try
            objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", "TravelerMoreDetails", Now.ToString & "  Error updating Traveler Device table: " & ex.ToString)
        End Try

        Try
            Call GC.Collect()
        Catch ex As Exception

        End Try

    End Sub

    '10/9/2015 NS added for VSPLUS-2252
    Private Sub UpdateDominoServerDetailsTable(ByRef DominoServer As MonitoredItems.DominoServer)

        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Entered UpdateDominoServerDetailsTable")

        Dim objVSAdaptor As New VSAdaptor
        Try


            Dim DominoServer2 As MonitoredItems.DominoServer = DominoServer
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.TypeAndName, DominoServer2.Name.ToString() + "-" + DominoServer2.ServerType)
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                    .Set(Function(x) x.ElapsedDays, DominoServer2.ElapsedTime / 60 / 60 / 24) _
                                                                                    .Set(Function(x) x.VersionArchitecture, DominoServer2.VersionArchitecture) _
                                                                                    .Set(Function(x) x.CpuCount, DominoServer2.CPUCount)
            repository.Update(filterDef, updateDef)

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error occurred in UpdateDominoServerDetailsTable: " & ex.Message)
        End Try

        Try
            GC.Collect()
        Catch ex As Exception

        End Try
    End Sub
#End Region


    'WS Commented out due to discusion with Alan

    'Private Sub RecordCountAvailability(ByVal DeviceType As String, ByVal DeviceName As String, ByVal UpPercent As Double)
    '    'This sub records the overall availability based on up and down counts
    '    WriteAuditEntry(Now.ToString & " Writing hourly Count Availability stats for " & DeviceName)

    '    Dim strSQL As String
    '    Dim MyWeekNumber As Integer
    '    Dim StatDate As DateTime = Now

    '    MyWeekNumber = GetWeekNumber(StatDate)
    '    Dim objVSAdaptor As New VSAdaptor
    '    Try
    '        strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
    '         " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyUpPercent', '" & UpPercent & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
    '        '  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)
    '        'If boolUseSQLServer = True Then
    '        '    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
    '        'Else
    '        '    OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
    '        '    OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
    '        'End If


    '        objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " RecordCountAvailability insert failed because: " & ex.Message)
    '        WriteAuditEntry(Now.ToString & " The failed RecordCountAvailability insert command was " & strSQL)
    '    End Try

    '    strSQL = Nothing
    '    MyWeekNumber = Nothing
    '    StatDate = Nothing

    'End Sub

    'Private Sub RecordOnTargetAvailability(ByVal DeviceType As String, ByVal DeviceName As String, ByVal OnTargetPercent As Double)
    '    WriteAuditEntry(Now.ToString & " Writing hourly On-Target Availability stats for " & DeviceName)
    '    'This sub records the percentage of times the device responded within its target response time
    '    Dim strSQL As String = ""
    '    Dim MyWeekNumber As Integer
    '    Dim StatDate As DateTime = Now

    '    MyWeekNumber = GetWeekNumber(StatDate)
    '    Dim objVSAdaptor As New VSAdaptor
    '    Try
    '        strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
    '         " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyOnTargetPercent', '" & OnTargetPercent & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
    '        '  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)

    '        'If boolUseSQLServer = True Then
    '        '    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
    '        'Else
    '        '    OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
    '        '    OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
    '        'End If
    '        objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " Record On Target Availability insert failed becase: " & ex.Message)
    '        WriteAuditEntry(Now.ToString & " The failed Record On Target Availability insert comand was " & strSQL)
    '    End Try
    '    strSQL = Nothing
    '    MyWeekNumber = Nothing
    '    StatDate = Nothing

    'End Sub

    'Private Sub RecordBusinessHoursOnTargetAvailability(ByVal DeviceType As String, ByVal DeviceName As String, ByVal OnTargetPercent As Double)
    '    WriteAuditEntry(Now.ToString & " Writing hourly Business Hours On-Target Availability stats for " & DeviceName)
    '    'This sub records the percentage of times the device responded within its target response time during business Hours only
    '    Dim strSQL As String
    '    Dim MyWeekNumber As Integer
    '    Dim StatDate As DateTime = Now

    '    MyWeekNumber = GetWeekNumber(StatDate)
    '    Dim objVSAdaptor As New VSAdaptor
    '    Try
    '        strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
    '         " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyBusinessHoursOnTargetPercent', '" & OnTargetPercent & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
    '        '  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)

    '        'If boolUseSQLServer = True Then
    '        '    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
    '        'Else
    '        '    OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
    '        '    OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
    '        'End If
    '        objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " Record On Target Availability insert failed becase: " & ex.Message)
    '        WriteAuditEntry(Now.ToString & " The failed Record On Target Availability insert comand was " & strSQL)
    '    End Try
    '    strSQL = Nothing
    '    MyWeekNumber = Nothing
    '    StatDate = Nothing


    'End Sub

    'Private Sub RecordTimeAvailability(ByVal DeviceType As String, ByVal DeviceName As String, ByVal UpPercent As Double)
    '    'This sub records the overall availability based on up and down counts
    '    WriteAuditEntry(Now.ToString & " Writing hourly Time Availability stats for " & DeviceName)

    '    Dim strSQL As String
    '    Dim MyWeekNumber As Integer
    '    Dim StatDate As DateTime = Now

    '    MyWeekNumber = GetWeekNumber(StatDate)
    '    Dim objVSAdaptor As New VSAdaptor
    '    Try
    '        strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
    '         " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyUpTimePercent', '" & UpPercent & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
    '        '  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)
    '        'OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
    '        'OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
    '        objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " RecordTimeAvailability insert failed because: " & ex.Message)
    '        WriteAuditEntry(Now.ToString & " The failed RecordTimeAvailability insert command was " & strSQL)
    '    End Try

    '    strSQL = Nothing
    '    MyWeekNumber = Nothing
    '    StatDate = Nothing

    'End Sub

    'Private Sub RecordDownTime(ByVal DeviceType As String, ByVal DeviceName As String, ByVal DownMinutes As Double)
    '    'This sub records the overall availability based on up and down counts
    '    WriteAuditEntry(Now.ToString & " Writing hourly Downtime  stats for " & DeviceName)
    '    If DownMinutes > 60 Then
    '        DownMinutes = 60
    '    End If

    '    Dim strSQL As String
    '    Dim MyWeekNumber As Integer
    '    Dim StatDate As DateTime = Now

    '    MyWeekNumber = GetWeekNumber(StatDate)
    '    Dim objVSAdaptor As New VSAdaptor
    '    Try
    '        strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
    '         " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyDownTimeMinutes', '" & DownMinutes & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
    '        '  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)

    '        'If boolUseSQLServer = True Then
    '        '    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
    '        'Else
    '        '    OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
    '        '    OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
    '        'End If
    '        objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " RecordTimeAvailability insert failed because: " & ex.Message)
    '        WriteAuditEntry(Now.ToString & " The failed RecordTimeAvailability insert command was " & strSQL)
    '    End Try

    '    strSQL = Nothing
    '    MyWeekNumber = Nothing
    '    StatDate = Nothing

    'End Sub

    Private Function FixDateTime(ByVal dt As DateTime) As String
        ' Return dt.ToUniversalTime.ToString
        Dim strDate As String
        strDate = FixDate(dt) & " " & dt.ToShortTimeString
        ' strDate = FixDate_OLD(dt) & " " & dt.ToShortTimeString

        Return strDate

        ' Return objDateUtils.FixDateTime(dt, strDateFormat)


    End Function
    Private Function GetFixedDateTime(ByVal dt As DateTime) As DateTime
        Return Convert.ToDateTime(FixDateTime(dt))
    End Function


    Private Function FixDate(ByVal dt As DateTime) As String
        ' Return dt.ToUniversalTime.ToString
        'WriteAuditEntry(Now.ToString & " FixDateTime, strDateFormat: " & strDateFormat)
        'If strDateFormat = "dmy" Then
        '    WriteAuditEntry(Now.ToString & " FixDateTime, ret dt dmy:" & String.Format("{0:d/M/yyyy}", dt))
        'ElseIf strDateFormat = "mdy" Then
        '    WriteAuditEntry(Now.ToString & " FixDateTime, ret dt mdy:" & String.Format("{0:M/d/yyyy}", dt))
        'ElseIf strDateFormat = "ymd" Then
        '    WriteAuditEntry(Now.ToString & " FixDateTime, ret dt ymd:" & String.Format("{0:yyyy/M/d}", dt))
        'End If
        'Mukund 28May14: Required to get current date format of the SQL Server 
        Try
            'WriteAuditEntry(Now.ToString & " Querying SQL server for the current date format.", LogLevel.Normal)
            strDateFormat = objDateUtils.GetDateFormat()
            'WriteAuditEntry(Now.ToString & " The current date format is " & strDateFormat, LogLevel.Normal)
        Catch ex As Exception

        End Try


        'Mukund 28May14: FixDate called to convert date to current date format of the SQL Server, using strDateFormat
        Dim strdt As String
        strdt = objDateUtils.FixDate(dt, strDateFormat)

        'WriteAuditEntry(Now.ToString & " FixDateTime, ret dt " & strDateFormat & ":" & strdt)
        Return strdt

    End Function

    Private Function FixDate_OLD(ByVal DateValue As DateTime) As String
        'Takes a date object and puts it into a standard, culture-independent format
        'returns DD-JAN-YYYY
        '08-MAY-2013

        Dim aDay As String
        Dim aMonth As String
        Dim aYear As String

        aDay = DateValue.Day
        aMonth = MonthName(Month(DateValue), True)

        'Finnish

        If InStr(aMonth, "tammi") Then
            aMonth = "Jan"
        End If

        If InStr(aMonth, "helmi") Then
            aMonth = "Feb"
        End If

        If InStr(aMonth, "maali") Then
            aMonth = "Mar"
        End If

        If InStr(aMonth, "huht") Then
            aMonth = "Apr"
        End If

        If InStr(aMonth, "touko") Then
            aMonth = "May"
        End If

        If InStr(aMonth, "kesä") Then
            aMonth = "Jun"
        End If

        If InStr(aMonth, "heinä") Then
            aMonth = "Jul"
        End If

        If InStr(aMonth, "elo") Then
            aMonth = "Aug"
        End If

        If InStr(aMonth, "syys") Then
            aMonth = "Sep"
        End If

        If InStr(aMonth, "loka") Then
            aMonth = "Oct"
        End If

        If InStr(aMonth, "marras") Then
            aMonth = "Nov"
        End If


        If InStr(aMonth, "joulu") Then
            aMonth = "Dec"
        End If

        'German
        If aMonth = "Mrz" Then
            aMonth = "Mar"
        End If

        If aMonth = "Mai" Then
            aMonth = "MAY"
        End If
        If aMonth = "Okt" Then
            aMonth = "Oct"
        End If
        If aMonth = "Dez" Then
            aMonth = "Dec"
        End If
        aYear = DateValue.Year
        FixDate_OLD = aDay.ToUpper & "-" & aMonth & "-" & aYear
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


End Class
