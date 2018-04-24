Imports System.Threading
Imports System.IO
Imports VSFramework
Partial Public Class VitalSignsPlusExJournal


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
        RA = objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", SQLUpdateStatement)

        If RA = 0 And SQLInsertStatement <> "" Then
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
                    StatusCode = "OK"
                Case "Disabled"
                    StatusCode = vbNull
                Case "Insufficient Licenses"
                    StatusCode = "Issue"
                Case Else
                    StatusCode = "Issue"
            End Select
        Catch ex As Exception
            StatusCode = vbNull
        End Try

        Return StatusCode
    End Function



    Private Function FixDate(ByVal DateValue As DateTime) As String
        'Takes a date object and puts it into a standard, culture-independent format
        'returns DD-JAN-YYYY
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
        FixDate = aDay.ToUpper & "-" & aMonth & "-" & aYear
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
