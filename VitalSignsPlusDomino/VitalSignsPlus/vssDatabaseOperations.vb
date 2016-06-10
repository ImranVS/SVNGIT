Imports System.Threading
Imports System.IO
Imports VSFramework
Imports System.Data.SqlClient
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

    Public Sub UpdateStatisticsTable(ByVal SQLUpdateStatement As String, Optional ByVal SQLInsertStatement As String = "", Optional ByVal Comment As String = "")
        WriteAuditEntry(Now.ToString + " ********************* Common Statistics UPDATE ******************** ")

        Dim RA As Integer   'Rows affected
        Dim objVSAdaptor As New VSAdaptor
        If objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "Statistics", SQLUpdateStatement) = False And SQLInsertStatement <> "" Then
            objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "Statistics", SQLInsertStatement)
        End If
    
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

    End Sub

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

				strSQL = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + .Name + "-Domino') BEGIN "
				strSQL += "INSERT INTO Status (StatusCode, Category, DeadMail, Description, DownCount,  Location, Name, MailDetails, PendingMail, Status, Type, Upcount, UpPercent,  ResponseTime, TypeANDName, Icon, OperatingSystem, DominoVersion, MyPercent,  Details, UpMinutes, DownMinutes, UpPercentMinutes, PendingThreshold, DeadThreshold) "
				strSQL += " VALUES ( 'Maintenance', '" & .Category & "', '" & .DeadMail & "', '" & .Description & "', '" & .DownCount & "', '" & .Location & "', '" & .Name & "', ' ', '" & .PendingMail & "', '" & .Status & "',  "
				strSQL += "'Domino', '" & .UpCount & "', '" & .UpPercentCount & "', '" & .ResponseTime & "' , '" & .Name & "-Domino', " & IconList.DominoServer & ", '" & .OperatingSystem & "', '" & .VersionDomino & "', " & Percent & ", '" & .ResponseDetails & "', '" & Microsoft.VisualBasic.Strings.Format(myDominoServer.UpMinutes, "##,##0.#") & "', '" & Microsoft.VisualBasic.Strings.Format(myDominoServer.DownMinutes, "##,##0.#") & "', '0', " & .PendingThreshold & ", " & .DeadThreshold & ")"
				strSQL += "END"

                '4/7/2016 NS modified for VSPLUS-2817
                strSqlUpdate = "UPDATE Status SET NextScan = '" & .NextScan & "' WHERE TypeANDName = '" & .Name & "-Domino'"
                sqlcmd = New SqlCommand("UPDATE Status SET NextScan=@NextScan WHERE TypeANDName=@Name")

                sqlcmd.Parameters.Add(New SqlParameter("@NextScan", SqlDbType.DateTime))
                sqlcmd.Parameters("@NextScan").Value = .NextScan

                sqlcmd.Parameters.Add(New SqlParameter("@Name", SqlDbType.NVarChar, 255))
                sqlcmd.Parameters("@Name").Value = .Name & "-Domino"

			End With

            Try
                Dim objVSAdaptor As New VSAdaptor
                WriteAuditEntry(vbCrLf & strSQL & vbCrLf & "Is Server Enabled  " & myDominoServer.Enabled.ToString() & vbCrLf, LogLevel.Verbose)
                If myDominoServer.Enabled = True Or InStr(myDominoServer.Name, "RPRWyattTest") Then
                    WriteAuditEntry(vbCrLf & strSQL & vbCrLf & "Update Statement: " & strSqlUpdate & vbCrLf, LogLevel.Verbose)
                    'UpdateStatusTable(strSqlUpdate, strSQL, "Processing " & myDominoServer.Name)
                    objVSAdaptor.ExecuteNonQuerySQLParams("VitalSigns", sqlcmd)
                End If

            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error inserting Domino servers into the Status Table " & ex.ToString)
            End Try

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
		'If boolUseSQLServer = True Then
		'    mySQLCommand.Connection = SqlConnectionVitalSigns
		'    mySQLCommand.CommandText = strSQL
		'    mySQLAdapter.SelectCommand = mySQLCommand
		'    mySQLAdapter.Fill(dsStatusHTML, "Status")
		'Else
		'    Try
		'        myCommand.CommandText = strSQL
		'        myCommand.Connection = myConnection
		'        myAdapter.SelectCommand = myCommand
		'        With myConnection
		'            .ConnectionString = Me.OleDbConnectionStatus.ConnectionString
		'            .Open()
		'        End With

		'        Do Until myConnection.State = ConnectionState.Open
		'            myConnection.Open()
		'        Loop
		'        myAdapter.Fill(dsStatusHTML, "Status")
		'    Catch ex As Exception
		'        WriteAuditEntry(Now.ToString & " UpdateStatusTableWithDominoCluster connection error @2 " & ex.Message)
		'    End Try
		'End If

		Dim myView As New Data.DataView(Status)

		Try
			myView.Sort = "Type ASC"
			WriteAuditEntry(Now.ToString & " UpdateStatusTableWithDominoCluster module is processing " & myView.Count & " records.")
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " UpdateStatusTableWithDominoCluster module error " & ex.Message & " source: " & ex.Source)
		End Try



		Try
			For Each drv In myView
				Dim myName As String
				myName = drv("Name")
				If InStr(myServerNames, myName) > 0 Then
					'the server has not been deleted
					WriteAuditEntry(Now.ToString & " " & myName & " is not marked for deletion. ", LogLevel.Verbose)
				Else
					'the server has been deleted, so delete from the status table
					Try
                        strSQL = "DELETE FROM Status WHERE Type = 'Domino Cluster database' AND Name = '" & myName & "'"
						objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " Error executing query updating Domino status table: " & ex.Message & vbCrLf & strSQL & vbCrLf)
					End Try

					'If boolUseSQLServer = True Then
					'    Try
					'        mySQLCommand.CommandText = strSQL
					'        mySQLCommand.ExecuteNonQuery()
					'    Catch ex As Exception
					'        WriteAuditEntry(Now.ToString & " Error executing query updating Domino status table: " & ex.Message & vbCrLf & strSQL & vbCrLf)
					'    End Try

					'Else
					'    Try
					'        myCommand.CommandText = strSQL
					'        myCommand.ExecuteNonQuery()
					'    Catch ex As Exception
					'        WriteAuditEntry(Now.ToString & " Error executing query updating Domino status table: " & ex.Message & vbCrLf & strSQL & vbCrLf)
					'    End Try
					'End If

					WriteAuditEntry(Now.ToString & " " & myName & " has been deleted from the Status table by the service.", LogLevel.Verbose)

				End If
			Next
		Catch ex As Exception

		End Try

		'Now Update the Data set with the current Domino data 

		Try
			Dim n As Integer
			Dim myDominoServerCluster As MonitoredItems.DominoMailCluster
			Dim strSqlUpdate As String

			For n = 0 To myDominoClusters.Count - 1
				myDominoCluster = myDominoClusters.Item(n)

                With myDominoCluster
                    '5/5/2016 NS modified - inserting a NotesMail Probe into StatusDetails fails because of a TypeANDName mismatch
                    'Changed TypeANDName -Cluster to -Domino Cluster database
                    strSQL = "INSERT INTO Status ( Name, Status, Type,  LastUpdate,  TypeANDName, Icon,  NextScan, Details, Category) " & _
                 " VALUES ('" & .Name & "', '" & .Status & "',  'Domino Cluster database', '" & FixDateTime(Now) & "', '" & .Name & "-Domino Cluster database', " & IconList.DominoServer & ", '" & FixDateTime(.NextScan) & "', '" & .ResponseDetails & "', '" & .Category & "')"

                    strSqlUpdate = "UPDATE Status SET NextScan = '" & .NextScan & "' WHERE TypeANDName = '" & .Name & "-Domino Cluster database'"

                End With
				Try
					UpdateStatusTable(strSqlUpdate, strSQL, myDominoCluster.Name)
				Catch ex As Exception
					'WriteAuditEntry(Now.ToString & " Error executing query updating Domino cluster status table: " & ex.Message)
				End Try
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

		For Each NotesDB As MonitoredItems.NotesDatabase In MyNotesDatabases
			With NotesDB
				'VSPLUS-939,Somaraj 16Oct14, Added StatusCode, which was missing.
				Try
					.StatusCode = ServerStatusCode(.Status)
				Catch ex As Exception
					.StatusCode = "OK"
				End Try
                '5/5/2016 NS modified
                'Changed -NDB to -Notes Database
                strSQL = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + .Name + "-Notes Database') BEGIN "
				strSQL += "INSERT INTO Status (Category,  Description, Details, DownCount,  Location, Name,  Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime, TypeANDName, Icon, ResponseThreshold, NextScan, StatusCode) "
                strSQL += " VALUES ('" & .Category & "', '" & .Description & "', '', '" & .DownCount & "', '" & .Location & "', '" & .Name & "', '" & .Status & "', 'Notes Database', '" & .UpCount & "', '" & .UpPercentCount & "', '" & FixDateTime(.LastScan) & "', " & .ResponseTime & ", '" & .Name & "-Notes Database', " & IconList.NotesDB & ", " & .ResponseThreshold & ", '" & .NextScan & "', '" & .StatusCode & "')"
				strSQL += "END"

				WriteAuditEntry(Now.ToString & " " & strSQL)

                strSqlUpdate = "UPDATE Status SET NextScan = '" & .NextScan & "' WHERE TypeANDName = '" & .Name & "-Notes Database'"
				WriteAuditEntry(Now.ToString & " " & strSQL)
			End With
			Try
				UpdateStatusTable(strSqlUpdate, strSQL, NotesDB.Name)
			Catch ex As Exception
				If Not InStr(ex.ToString, "duplicate") Then
					'  WriteAuditEntry(Now.ToString & " Error executing query updating Notes Databases status table: " & ex.tostring)
				End If
			End Try
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

		For n = 0 To MyNotesMailProbes.Count - 1
			strSQL = ""
			MyNotesMailProbe = MyNotesMailProbes.Item(n)
			WriteAuditEntry(Now.ToString & " adding Mail Probe " & MyNotesMailProbe.Name)
			With MyNotesMailProbe
				MyNotesMailProbe.StatusCode = ServerStatusCode(MyNotesMailProbe.Status)
				'strSQL = "INSERT INTO Status (Category,  Description, Details, DownCount,  Location, Name,  Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime,  TypeANDName, Icon, StatusCode) " & _
				'   " VALUES ('" & .Category & "', '" & .Description & "', '" & .ResponseDetails & "', '" & .DownCount & "', 'Mail Probe', '" & .Name & "', ' " & .Status & "', 'NotesMail Probe', '" & .UpCount & "', '" & .UpPercentCount & "', '" & FixDateTime(Now) & "', '" & .ResponseTime & "', '" & .Name & "-NMP', " & IconList.NotesMail_Probe & ", '" & ServerStatusCode(MyNotesMailProbe.Status) & "')"
                '5/5/2016 NS modified - inserting a NotesMail Probe into StatusDetails fails because of a TypeANDName mismatch
                'Changed TypeANDName -NMP to -NotesMail Probe; corrected a typo with an extra leading white space in front of status
                strSQL = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + .Name + "-NotesMail Probe') BEGIN "
				strSQL += "INSERT INTO Status (Category,  Description, Details, DownCount,  Location, Name,  Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime,  TypeANDName, Icon, StatusCode) "
                strSQL += " VALUES ('" & .Category & "', '" & .Description & "', '" & .ResponseDetails & "', '" & .DownCount & "', 'Mail Probe', '" & .Name & "', '" & .Status & "', 'NotesMail Probe', '" & .UpCount & "', '" & .UpPercentCount & "', '" & FixDateTime(Now) & "', '" & .ResponseTime & "', '" & .Name & "-NotesMail Probe', " & IconList.NotesMail_Probe & ", '" & ServerStatusCode(MyNotesMailProbe.Status) & "')"
				strSQL += "END ELSE BEGIN "
                strSQL += " UPDATE Status SET Category='" & .Category & "', Description='" & .Description & "', Details='" & .ResponseDetails & "', DownCount='" & .DownCount & "', Location='Mail Probe', Name='" & .Name & "', Status='" & .Status & "', Type='NotesMail Probe', Upcount='" & .UpCount & "', UpPercent='" & .UpPercentCount & "',LastUpdate='" & FixDateTime(Now) & "', ResponseTime='" & .ResponseTime & "', Icon='" & IconList.NotesMail_Probe & "', StatusCode='" & ServerStatusCode(MyNotesMailProbe.Status) & "' WHERE TypeANDName='" & .Name & "-NotesMail Probe'"
				strSQL += "END"

			End With

			Try
				'WriteAuditEntry(Now.ToString & " Sql statement: " & strSQL)
				objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
				'If boolUseSQLServer = True Then
				'    'WriteAuditEntry(Now.ToString & " Inserting into SQL server: " & strSQL)
				'    Dim myCommand As New Data.SqlClient.SqlCommand
				'    myCommand.Connection = SqlConnectionVitalSigns
				'    myCommand.CommandText = strSQL
				'    myCommand.ExecuteNonQuery()
				'    myCommand.Dispose()
				'Else
				'    Dim myCommand As New OleDb.OleDbCommand
				'    myCommand.Connection = myConnection
				'    myCommand.CommandText = strSQL
				'    myCommand.ExecuteNonQuery()
				'    myCommand.Dispose()
				'    myConnection.Close()
				'    myConnection.Dispose()
				'End If

			Catch ex As Exception

			End Try
		Next n
		MyNotesMailProbe = Nothing
		n = Nothing
		strSQL = Nothing
		WriteAuditEntry(Now.ToString & " Updated Status table with NotesMail Probe information.")


	End Sub

	'Not Used
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

	Public Sub UpdateUpTimeAllDevices()
		Dim strSQL As String = ""

		Try
			If MyDominoServers.Count > 0 Then
				For Each DS As MonitoredItems.DominoServer In MyDominoServers
					With DS
						With DS
							strSQL = "Update Status SET NextScan='" & FixDateTime(.NextScan) & _
							"', UpMinutes=" & Microsoft.VisualBasic.Strings.Format(.UpMinutes, "F1") & _
							", DownMinutes=" & Microsoft.VisualBasic.Strings.Format(.DownMinutes, "F1") & _
							"  WHERE TypeANDName='" & .Name & "-Domino'"
						End With


					End With
					UpdateStatusTable(strSQL)
				Next
			End If
		Catch ex As Exception

		End Try


	End Sub


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

	Private Sub cleanUpClusterDetailedTable(ByVal ClusterId As String)
		Dim strSQL As String = ""
		Dim objVSAdaptor As New VSAdaptor
		Try
			Dim dsDominoClusters As New Data.DataSet
			strSQL = "Delete from ClusterDatabaseDetails where ClusterID = " & ClusterId
			objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "ClusterDatabaseDetails", strSQL)

		Catch ex As Exception

		Finally

		End Try
		strSQL = Nothing
	End Sub

    '9/30/2015 NS modified for VSPLUS-2150
    Private Sub UpdateClusterDataTable(ByVal ClusterId As String, ByVal DatabaseTitle As String, ByVal DatabaseName As String,
            ByVal SACount As Int32, ByVal SBCount As Int32, ByVal SCCount As Int32, ByVal SASize As Long, ByVal SBSize As Long,
            ByVal SCSize As Long, ByVal Desc As String, ByVal lastScanned As DateTime, ByVal ReplicaID As String)
        Dim strSQL As String = ""
        Dim objVSAdaptor As New VSAdaptor
        Try
            Dim dsDominoClusters As New Data.DataSet
            'strSQL = "Select * from ClusterDatabaseDetails where ClusterID = " & ClusterId
            'If (objVSAdaptor.ExecuteScalarAny("VitalSigns", "ClusterDatabaseDetails", strSQL) = vbNull) Then
            '9/30/2015 NS modified for VSPLUS-2150
            '5/6/2016 NS modified
            Dim sqlcmd As New SqlCommand
            Dim sqlparam1 As New SqlParameter()
            Dim sqlparam2 As New SqlParameter()
            Dim sqlparam3 As New SqlParameter()
            Dim sqlparam4 As New SqlParameter()
            Dim sqlparam5 As New SqlParameter()
            Dim sqlparam6 As New SqlParameter()
            Dim sqlparam7 As New SqlParameter()
            Dim sqlparam8 As New SqlParameter()
            Dim sqlparam9 As New SqlParameter()
            Dim sqlparam10 As New SqlParameter()
            Dim sqlparam11 As New SqlParameter()
            Dim sqlparam12 As New SqlParameter()
            'strSQL = "INSERT INTO ClusterDatabaseDetails (ClusterID, DatabaseTitle, DatabaseName, DocCountA, DocCountB, DocCountC , DBSizeA, DBSizeB, DBSizeC, [Description],LastScanned,ReplicaID)" & _
            '  " VALUES (" & ClusterId & ", '" & DatabaseTitle & "', '" & DatabaseName & "', " & SACount & ", " & SBCount & ", " &
            '  SCCount & ", " & SASize & "," & SBSize & ", " & SCSize & ", '" & Desc & "','" & lastScanned & "','" & ReplicaID & "')"
            'objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "ClusterDatabaseDetails", strSQL)

            sqlcmd = New SqlCommand("INSERT INTO ClusterDatabaseDetails (ClusterID, DatabaseTitle, DatabaseName, DocCountA, DocCountB, DocCountC , DBSizeA, DBSizeB, DBSizeC, [Description],LastScanned,ReplicaID) " & _
                " VALUES (@ClusterID,@DatabaseTitle,@DatabaseName,@DocCountA,@DocCountB,@DocCountC,@DBSizeA,@DBSizeB,@DBSizeC,@Description,@LastScanned,@ReplicaID)")
            sqlparam1.ParameterName = "@ClusterID"
            sqlparam1.Value = ClusterId
            sqlcmd.Parameters.Add(sqlparam1)
            sqlparam2.ParameterName = "@DatabaseTitle"
            sqlparam2.Value = DatabaseTitle
            sqlcmd.Parameters.Add(sqlparam2)
            sqlparam3.ParameterName = "@DatabaseName"
            sqlparam3.Value = DatabaseName
            sqlcmd.Parameters.Add(sqlparam3)
            sqlparam4.ParameterName = "@DocCountA"
            sqlparam4.Value = SACount
            sqlcmd.Parameters.Add(sqlparam4)
            sqlparam5.ParameterName = "@DocCountB"
            sqlparam5.Value = SBCount
            sqlcmd.Parameters.Add(sqlparam5)
            sqlparam6.ParameterName = "@DocCountC"
            sqlparam6.Value = SCCount
            sqlcmd.Parameters.Add(sqlparam6)
            sqlparam7.ParameterName = "@DBSizeA"
            sqlparam7.Value = SASize
            sqlcmd.Parameters.Add(sqlparam7)
            sqlparam8.ParameterName = "@DBSizeB"
            sqlparam8.Value = SBSize
            sqlcmd.Parameters.Add(sqlparam8)
            sqlparam9.ParameterName = "@DBSizeC"
            sqlparam9.Value = SCSize
            sqlcmd.Parameters.Add(sqlparam9)
            sqlparam10.ParameterName = "@Description"
            sqlparam10.Value = Desc
            sqlcmd.Parameters.Add(sqlparam10)
            sqlparam11.ParameterName = "@LastScanned"
            sqlparam11.Value = lastScanned
            sqlcmd.Parameters.Add(sqlparam11)
            sqlparam12.ParameterName = "@ReplicaID"
            sqlparam12.Value = ReplicaID
            sqlcmd.Parameters.Add(sqlparam12)
            WriteAuditEntry(" Query parameters: " & sqlparam1.Value & "; " & sqlparam2.Value & "; " & sqlparam3.Value & "; " & sqlparam4.Value & _
                            "; " & sqlparam5.Value & "; " & sqlparam6.Value & "; " & sqlparam7.Value & "; " & sqlparam8.Value & "; " & _
                            sqlparam9.Value & "; " & sqlparam10.Value & "; " & sqlparam11.Value & "; " & sqlparam12.Value, LogLevel.Verbose)
            objVSAdaptor.ExecuteNonQuerySQLParams("VitalSigns", sqlcmd)
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
				strSQL = "Update Status SET DeadMail= " & .DeadMail & _
				", DownCount= " & .DownCount & _
				", PendingMail=" & .PendingMail & _
				", Icon=" & myIcon & _
				", Status='" & .Status & _
				"', Upcount=" & .UpCount & _
				", UpPercent= '" & .UpPercentCount.ToString("F2") & _
				"', StatusCode='" & .StatusCode & _
				"', Details='" & StatusDetails & _
				"', SecondaryRole='" & .SecondaryRole & _
				"', Description ='" & Left(.Description, 254) & _
				"', DominoServerTasks ='" & .TaskStatus & _
				"', LastUpdate='" & FixDateTime(.LastScan) & _
				"', Category='" & .Category & _
				"', ResponseTime='" & .ResponseTime & _
				"', PendingThreshold=" & .PendingThreshold & _
				", DeadThreshold=" & .DeadThreshold & _
				", HeldMail=" & .HeldMail & _
				" , HeldMailThreshold=" & .HeldThreshold & _
				", ResponseThreshold=" & .ResponseThreshold & _
				", UserCount=" & .UserCount & _
				", CPU='" & .CPU_Utilization.ToString("F2") & _
				"', CPUThreshold='" & (.CPU_Threshold / 100).ToString("F2") & _
				 "', Memory='" & (.MemoryPercentUsed / 100).ToString & _
				"', NextScan='" & FixDateTime(.NextScan) & _
				 "', UpPercentMinutes='" & myUpPercent.ToString("F2") & _
				"', MyPercent='" & strPercent & _
				"', PercentageChange='" & PercentageChange.ToString("F2") & _
				"', UpMinutes='" & Microsoft.VisualBasic.Strings.Format(.UpMinutes, "##0.#") & _
				"', DownMinutes='" & Microsoft.VisualBasic.Strings.Format(.DownMinutes, "##0.#") & _
				"'  WHERE TypeANDName='" & .Name & "-Domino'"
			End With
		Catch ex As Exception
			'  WriteAuditEntry(Now.ToString & " Error in Domino module creating SQL statement for " & MyDominoServer.Name & " while updating status table: " & ex.Message)
			WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error in Domino module creating SQL statement for status table: " & ex.Message)
		End Try


		Try
			With MyDominoServer
				strSQL_Short = "Update Status SET Status='" & .Status & "', StatusCode='" & .StatusCode & _
					  "', Details='" & StatusDetails & "', Description='" & Left(.Description, 254) & "', LastUpdate='" & FixDate(.LastScan) & " " & .LastScan.ToShortTimeString & "', NextScan='" & FixDate(.NextScan) & " " & .NextScan.ToShortTimeString & _
					  "'  WHERE TypeANDName='" & .Name & "-Domino'"
			End With
			WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & strSQL_Short)
		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error in Domino module creating shorter SQL statement for status table: " & ex.Message)
		End Try

		Dim objVSAdaptor As New VSAdaptor
		Try
			objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL_Short)
		Catch ex As Exception

		End Try

		strSQL = strSQL.Replace("NaN", "1")
		strSQL = strSQL.Replace("n. def.", "1")

		Dim strSQLInsert As String

		Try
			With MyDominoServer
				strSQLInsert = "INSERT INTO Status (StatusCode, Category, DeadMail, Description, Details, DownCount,  Location, Name, MailDetails, PendingMail, Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime, TypeANDName, Icon) " & _
				   " VALUES ( '" & .StatusCode & "','" & .Category & "', " & .DeadMail & ", '" & .Description & "', '" & StatusDetails & "', " & .DownCount & ", '" & .Location & "', '" & .Name & "', 'Mail Details', " & .PendingMail & ", '" & .Status & "',  " & _
				"'Domino', " & .UpCount & ", " & .UpPercentCount & ", '" & FixDateTime(Now) & "', '0' , '" & .Name & "-Domino', " & IconList.DominoServer & ")"
			End With

			strSQLInsert = strSQLInsert.Replace("NaN", "1")
			Try
				WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Attempting to update status with " & strSQL)
			Catch ex As Exception

			End Try


			Try
				If objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL) = 0 Then
					WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " No records updated, attempting an INSERT instead.")
					objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQLInsert)
				End If
			Catch ex As Exception
				WriteAuditEntry(Now.ToString & " Error writing to Domino Status using SQL Server" & ex.ToString)
			End Try

		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Error updating Status table with Domino server info: " & ex.Message & vbCrLf & "SQL Statement: " & strSQL)

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
					UpdateDominoDailyStatTable(DominoServer.Name, Disk.DiskName & ".Free", ParseNumericStatValue(Disk.DiskName & ".Free", DominoServer.Statistics_Disk))
				End If
			Next
		Catch ex As Exception

		End Try


		'Memory
		WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Updating Daily Statistics Table for Memory.")

		Try
			If InStr(DominoServer.Statistics_Memory, "Mem.Free") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Mem.Free", ParseNumericStatValue("Mem.Free", DominoServer.Statistics_Memory))
			ElseIf InStr(DominoServer.Statistics_All, "Mem.Free") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Mem.Free", ParseNumericStatValue("Mem.Free", DominoServer.Statistics_All))

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
				UpdateDominoDailyStatTable(DominoServer.Name, "Platform.Memory.KBFree", ParseNumericStatValue("Platform.Memory.KBFree", DominoServer.Statistics_All))
			ElseIf InStr(DominoServer.Statistics_Platform, "Platform.System.PctCombinedCpuUtil") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Platform.Memory.KBFree", ParseNumericStatValue("Platform.Memory.KBFree", DominoServer.Statistics_Platform))
			End If

			If InStr(DominoServer.Statistics_All, "Platform.Memory.RAM.AvailMBytes") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Platform.Memory.RAM.AvailMBytes", ParseNumericStatValue("Platform.Memory.RAM.AvailMBytes", DominoServer.Statistics_All))
			ElseIf InStr(DominoServer.Statistics_Platform, "Platform.Memory.RAM.AvailMBytes") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Platform.Memory.RAM.AvailMBytes", ParseNumericStatValue("Platform.Memory.RAM.AvailMBytes", DominoServer.Statistics_Platform))
			End If

			If InStr(DominoServer.Statistics_All, "Platform.Memory.RAM.TotalMBytes") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Platform.Memory.RAM.TotalMBytes", ParseNumericStatValue("Platform.Memory.RAM.TotalMBytes", DominoServer.Statistics_All))
			ElseIf InStr(DominoServer.Statistics_Platform, "Platform.Memory.RAM.TotalMBytes") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Platform.Memory.RAM.TotalMBytes", ParseNumericStatValue("Platform.Memory.RAM.TotalMBytes", DominoServer.Statistics_Platform))
			End If
		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Platform statistics: " & ex.Message)
		End Try

		WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Updating Daily Statistics Table for Replication.")

		'Replication
		Try
			If InStr(DominoServer.Statistics_Replica, "Replica.Failed") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Failed", ParseNumericStatValue("Replica.Failed", DominoServer.Statistics_Replica))
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
				UpdateDominoDailyStatTable(DominoServer.Name, "Server.Users", ParseNumericStatValue("Server.Users", DominoServer.Statistics_Server))
			End If

			If InStr(DominoServer.Statistics_Server, "Server.Trans.PerMinute") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Server.Trans.PerMinute", ParseNumericStatValue("Server.Trans.PerMinute", DominoServer.Statistics_Server))
			End If

			If InStr(DominoServer.Statistics_Server, "Server.AvailabilityIndex") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Server.AvailabilityIndex", ParseNumericStatValue("Server.AvailabilityIndex", DominoServer.Statistics_Server))
			End If



		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Server statistics: " & ex.Message)
		End Try


		Try
			If InStr(DominoServer.Statistics_Mail, "Mail.TotalPending") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Mail.TotalPending", ParseNumericStatValue("Mail.TotalPending", DominoServer.Statistics_Mail))
			End If
		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Mail.TotalPending statistics: " & ex.Message)
		End Try

		Try

			If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.1.PctUtil") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Platform.LogicalDisk.1.PctUtil", ParseNumericStatValue("Platform.LogicalDisk.1.PctUtil", DominoServer.Statistics_Platform))
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
		End Try

		Try

			If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.1.AvgQueueLen") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Platform.LogicalDisk.1.AvgQueueLen", ParseNumericStatValue("Platform.LogicalDisk.1.AvgQueueLen", DominoServer.Statistics_Platform))
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
		End Try

		Try

			If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.2.AvgQueueLen") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Platform.LogicalDisk.2.AvgQueueLen", ParseNumericStatValue("Platform.LogicalDisk.2.AvgQueueLen", DominoServer.Statistics_Platform))
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
		End Try


		Try

			If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.2.PctUtil") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Platform.LogicalDisk.2.PctUtil", ParseNumericStatValue("Platform.LogicalDisk.2.PctUtil", DominoServer.Statistics_Platform))
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
		End Try

		Try

			If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.3.PctUtil") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Platform.LogicalDisk.3.PctUtil", ParseNumericStatValue("Platform.LogicalDisk.3.PctUtil", DominoServer.Statistics_Platform))
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
		End Try


		Try

			If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.3.AvgQueueLen") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Platform.LogicalDisk.3.AvgQueueLen", ParseNumericStatValue("Platform.LogicalDisk.3.AvgQueueLen", DominoServer.Statistics_Platform))
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
		End Try

		Try

			If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.4.PctUtil") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Platform.LogicalDisk.4.PctUtil", ParseNumericStatValue("Platform.LogicalDisk.4.PctUtil", DominoServer.Statistics_Platform))
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
		End Try


		Try

			If InStr(DominoServer.Statistics_Platform, "Platform.LogicalDisk.4.AvgQueueLen") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Platform.LogicalDisk.4.AvgQueueLen", ParseNumericStatValue("Platform.LogicalDisk.4.AvgQueueLen", DominoServer.Statistics_Platform))
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
		End Try


		Try

			If InStr(DominoServer.Statistics_Mail, "Mail.Mailbox.AccessConflicts") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Mail.Mailbox.AccessConflicts", ParseNumericStatValue("Mail.Mailbox.AccessConflicts", DominoServer.Statistics_Mail))
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
		End Try


		Try

			If InStr(DominoServer.Statistics_Mail, "Mail.Mailbox.Accesses") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Mail.Mailbox.Accesses", ParseNumericStatValue("Mail.Mailbox.Accesses", DominoServer.Statistics_Mail))
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
		End Try

		Try
			If InStr(DominoServer.Statistics_HTTP, "Http.CurrentConnections") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Http.CurrentConnections", ParseNumericStatValue("Http.CurrentConnections", DominoServer.Statistics_HTTP))
			End If
		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
		End Try

		Try
			If InStr(DominoServer.Statistics_HTTP, "Http.Worker.Total.QuickPlace.Requests") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Http.Worker.Total.QuickPlace.Requests", ParseNumericStatValue("Http.Worker.Total.QuickPlace.Requests", DominoServer.Statistics_HTTP))
			End If
		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Database statistics: " & ex.Message)
		End Try


	End Sub

	Private Sub UpdateDominoResponseTimeTable(ByVal DeviceName As String, ByVal ResponseTime As Double)

		Dim strSQL As String = ""
        Dim MyWeekNumber As Integer
        '10/14/2015 NS modified for VSPLUS-2085
        Dim nowDate As DateTime = Now
        MyWeekNumber = GetWeekNumber(nowDate)

		Try
            strSQL = "INSERT INTO DeviceDailyStats (DeviceType, ServerName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
             " VALUES ('Domino', '" & DeviceName & "', '" & FixDateTime(nowDate) & "', '" & "ResponseTime" & "', '" & ResponseTime & "', '" & MyWeekNumber & "', '" & nowDate.Month & "', '" & nowDate.Year & "', '" & nowDate.Day & "')"
			Dim objVSAdaptor As New VSAdaptor
			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
			'myCommand.CommandText = strSQL
			'myCommand.ExecuteNonQuery()

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
				UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.WorkQueueDepth", myDepth)
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
				UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.WorkQueueDepth.Avg", myDepth)
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
				UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.WorkQueueDepth.Max", myDepth)
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
				UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.SecondsOnQueue", myDepth)
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
				UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.SecondsOnQueue.Avg", myDepth)
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
				UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.SecondsOnQueue.Max", myDepth)
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
				UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.Failed", ParseNumericStatValue("Replica.Cluster.Failed", DominoServer.Statistics_Replica))
			End If
		Catch ex As Exception

		End Try

		Try
			If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.RetryWaiting") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.RetryWaiting", ParseNumericStatValue("Replica.Cluster.RetryWaiting", DominoServer.Statistics_Replica))
			End If
		Catch ex As Exception

		End Try

		Try
			If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.Files.Local") > 0 Then
				UpdateDominoDailyStatTable(DominoServer.Name, "Replica.Cluster.Files.Local", ParseNumericStatValue("Replica.Cluster.Files.Local", DominoServer.Statistics_Replica))
			End If
		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Cluster Replication statistics: " & ex.Message)
		End Try

	End Sub

	Private Sub UpdateDominoDailyStatTable(ByVal ServerName As String, ByVal StatName As String, ByVal StatValue As Double)

		If StatValue = -999 Then Exit Sub

		'**

		Dim strSQL As String = ""
        Dim MyWeekNumber As Integer
        '10/14/2015 NS modified for VSPLUS-2085
        Dim nowDate As DateTime = Now
        WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Current date/time: " & nowDate, LogUtilities.LogUtils.LogLevel.Verbose)
        MyWeekNumber = GetWeekNumber(nowDate)
        WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Week Number: " & MyWeekNumber, LogUtilities.LogUtils.LogLevel.Verbose)

        strSQL = "INSERT INTO DominoDailyStats (ServerName, [Date], StatName, StatValue,  WeekNumber, MonthNumber, YearNumber, DayNumber, HourNumber)" & _
         " VALUES ('" & ServerName & "', '" & FixDateTime(nowDate) & "', '" & StatName & "', '" & StatValue & "', '" & MyWeekNumber & "', '" & nowDate.Month & "', '" & nowDate.Year & "', '" & nowDate.Day & "', '" & nowDate.Hour & "')"


		Dim objVSAdaptor As New VSAdaptor
		Try
			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "Statistics", strSQL)
		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Domino Daily Stats table insert failed because: " & ex.Message)
			WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " The failed stats table insert command was " & strSQL)
		End Try

		strSQL = Nothing
        MyWeekNumber = Nothing
        nowDate = Nothing

		Try
			GC.Collect()
		Catch ex As Exception

		End Try
	End Sub

	Private Sub UpdateDominoDailyMailFileStatTable(ByVal ScanDate As DateTime, ByVal MailServer As String, ByVal FileName As String, ByVal FileTitle As String, ByVal FileSize As Double, ByVal TemplateName As String, ByVal Quota As Double, ByVal FTIndexed As Boolean, ByVal OutOfOfficeAgentEnabled As Boolean, ByVal EnabledForClusterReplication As Boolean, ByVal ReplicaID As String, ByVal ODS As Double)
		Dim strSQL As String
		If TemplateName = "" Then
			TemplateName = "None"
		End If
		If InStr(FileTitle, "'") Then
			FileTitle = FileName
		End If

		WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " Entered UpdateDominoFileMailFileStatTable with " & ScanDate & ",  " & FileTitle)

		strSQL = "SELECT COUNT(*) FROM Daily Where FileName = '" & FileName & "' AND MailServer= '" & MailServer & "' "

		Dim objVSAdaptor As New VSAdaptor
		Try
			Dim myResult As Integer
			myResult = objVSAdaptor.ExecuteScalarAny("VSS_Statistics", "mailfilestats", strSQL)
			If Not (myResult = Nothing) Then
				'got some, better update
				WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " My result was " & myResult)
				Try
					'changed by somaraj Fixdate as Fixdatetime
					strSQL = "Update (Daily) Set ScanDate = '" & FixDateTime(ScanDate) & "', FileSize='" & FileSize & "', FileTitle='" & FileTitle & "', MailTemplate='" & TemplateName & "', "
					strSQL += " Quota='" & Quota & "', FileName='" & FileName & "',  FTIndexed=" & FTIndexed & ", OutOfOfficeAgentEnabled=" & OutOfOfficeAgentEnabled & ", "
					strSQL += " EnabledForClusterReplication=" & EnabledForClusterReplication & ", "
					strSQL += " ODS =" & ODS & ""
					strSQL += " WHERE MailServer='" & MailServer & "' AND ReplicaID='" & ReplicaID & "'"
					objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "NSFHealth", strSQL)

				Catch ex2 As Exception
					WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " Error Updating Mail Stats: " & ex2.Message & vbCrLf & strSQL)
				End Try
			Else
				'no existing rows so instead insert

				Try
					'changed by somaraju fixdate as fixdatetime
					strSQL = "INSERT INTO Daily (MailServer, ScanDate, FileName, FileSize, FileTitle, MailTemplate, Quota, FTIndexed, OutOfOfficeAgentEnabled, EnabledForClusterReplication, ReplicaID, ODS )" & _
					 " VALUES ('" & MailServer & "', '" & FixDateTime(ScanDate) & "', '" & FileName & "', '" & FileSize & "', '" & FileTitle & "', '" & TemplateName & "', " & Quota & ", " & FTIndexed & ", " & OutOfOfficeAgentEnabled & ", " & EnabledForClusterReplication & ", '" & ReplicaID & "', '" & ODS & "')"
					objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "NSFHealth", strSQL)
				Catch ex2 As Exception
					WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " Error Inserting Mail Stats: " & ex2.Message & vbCrLf & strSQL)
				End Try

			End If
		Catch ex As Exception

		End Try
		'If boolUseSQLServer = True Then
		'    strSQL = "SELECT COUNT(*) FROM Daily Where FileName = '" & FileName & "' AND MailServer= '" & MailServer & "' "
		'    Dim myResult As Integer
		'    myResult = ExecuteScalarUSingVSS_Statisics(strSQL)
		'    If Not (myResult = Nothing) Then
		'        'got some, better update
		'        WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " My result was " & myResult)
		'        Try
		'            strSQL = "Update (Daily) Set ScanDate = '" & FixDate(ScanDate) & "', FileSize='" & FileSize & "', FileTitle='" & FileTitle & "', MailTemplate='" & TemplateName & "', "
		'            strSQL += " Quota='" & Quota & "', FileName='" & FileName & "',  FTIndexed=" & FTIndexed & ", OutOfOfficeAgentEnabled=" & OutOfOfficeAgentEnabled & ", "
		'            strSQL += " EnabledForClusterReplication=" & EnabledForClusterReplication & ", "
		'            strSQL += " ODS =" & ODS & ""
		'            strSQL += " WHERE MailServer='" & MailServer & "' AND ReplicaID='" & ReplicaID & "'"
		'            ExecuteNonQuerySQL_VSS_Statistics(strSQL)

		'        Catch ex2 As Exception
		'            WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " Error Updating Mail Stats: " & ex2.Message & vbCrLf & strSQL)
		'        End Try
		'    Else
		'        'no existing rows so instead insert

		'        Try
		'            strSQL = "INSERT INTO Daily (MailServer, ScanDate, FileName, FileSize, FileTitle, MailTemplate, Quota, FTIndexed, OutOfOfficeAgentEnabled, EnabledForClusterReplication, ReplicaID, ODS )" & _
		'                         " VALUES ('" & MailServer & "', '" & FixDate(ScanDate) & "', '" & FileName & "', '" & FileSize & "', '" & FileTitle & "', '" & TemplateName & "', " & Quota & ", " & FTIndexed & ", " & OutOfOfficeAgentEnabled & ", " & EnabledForClusterReplication & ", '" & ReplicaID & "', '" & ODS & "')"
		'          ExecuteNonQuerySQL_VSS_Statistics(strSQL)
		'        Catch ex2 As Exception
		'            WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " Error Inserting Mail Stats: " & ex2.Message & vbCrLf & strSQL)
		'        End Try

		'    End If
		'    ExecuteNonQueryUsingVSS_Statistics(strSQL)
		'Else
		'    '**
		'    Dim myPath As String
		'    Dim myRegistry As New RegistryHandler
		'    'Read the registry for the location of the Config Database


		'    Dim myConnection As New Data.OleDb.OleDbConnection
		'    Try
		'        myPath = myRegistry.ReadFromRegistry("Application Path")
		'        ' debug.writeline(Now.ToString & " Domino Update e database " & myPath)
		'    Catch ex As Exception

		'    End Try


		'    If myPath Is Nothing Then
		'        myRegistry = Nothing
		'        myConnection.Close()
		'        myConnection.Dispose()
		'        Exit Sub
		'    End If
		'    myRegistry = Nothing

		'    With myConnection
		'        .ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & myPath & "data\mailfilestats.mdb"
		'        .Open()
		'    End With

		'    Do Until myConnection.State = ConnectionState.Open
		'        myConnection.Open()
		'    Loop


		'    ' debug.writeline(Now.ToString & " myConnection state is " & myConnection.State.ToString)
		'    Dim myCommand As New OleDb.OleDbCommand
		'    myCommand.Connection = myConnection


		'    'figure out if the row is there already; if so, update it.  If not insert a new row
		'    Dim myResult As Object
		'    myCommand.CommandText = "SELECT COUNT(*) FROM Daily Where FileName = '" & FileName & "' AND MailServer= '" & MailServer & "' "
		'    ' WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " Attempting Scalar query : " & myCommand.CommandText)

		'    Try
		'        myResult = myCommand.ExecuteScalar

		'        If Not (myResult = Nothing) Then
		'            'got some, better update
		'            WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " My result was " & myResult)
		'            Try
		'                strSQL = "Update (Daily) Set ScanDate = '" & FixDate(ScanDate) & "', FileSize='" & FileSize & "', FileTitle='" & FileTitle & "', MailTemplate='" & TemplateName & "', "
		'                strSQL += " Quota='" & Quota & "', FileName='" & FileName & "',  FTIndexed=" & FTIndexed & ", OutOfOfficeAgentEnabled=" & OutOfOfficeAgentEnabled & ", "
		'                strSQL += " EnabledForClusterReplication=" & EnabledForClusterReplication & ", "
		'                strSQL += " ODS =" & ODS & ""
		'                strSQL += " WHERE MailServer='" & MailServer & "' AND ReplicaID='" & ReplicaID & "'"

		'                myCommand.CommandText = strSQL
		'                myCommand.ExecuteNonQuery()
		'                '  WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " Updating with : " & strSQL)

		'            Catch ex2 As Exception
		'                WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " Error Updating Mail Stats: " & ex2.Message & vbCrLf & strSQL)
		'            End Try
		'        Else

		'            'no existing rows so instead insert

		'            Try
		'                strSQL = "INSERT INTO Daily (MailServer, ScanDate, FileName, FileSize, FileTitle, MailTemplate, Quota, FTIndexed, OutOfOfficeAgentEnabled, EnabledForClusterReplication, ReplicaID, ODS )" & _
		'                             " VALUES ('" & MailServer & "', '" & FixDate(ScanDate) & "', '" & FileName & "', '" & FileSize & "', '" & FileTitle & "', '" & TemplateName & "', " & Quota & ", " & FTIndexed & ", " & OutOfOfficeAgentEnabled & ", " & EnabledForClusterReplication & ", '" & ReplicaID & "', '" & ODS & "')"
		'                myCommand.CommandText = strSQL
		'                myCommand.ExecuteNonQuery()
		'                '    WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " Inserting with  : " & strSQL)

		'            Catch ex2 As Exception
		'                WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " Error Inserting Mail Stats: " & ex2.Message & vbCrLf & strSQL)
		'            End Try

		'        End If
		'    Catch ex As Exception
		'        WriteDeviceHistoryEntry("Domino", MailServer, Now.ToString & " Error executing scalar query: " & ex.Message & vbCrLf & strSQL)
		'    End Try


		'    Try
		'        myConnection.Close()
		'        myConnection.Dispose()
		'        myCommand.Dispose()
		'    Catch ex As Exception

		'    End Try
		'End If




		Try
			GC.Collect()
		Catch ex As Exception

		End Try
	End Sub

	Private Sub UpdateDominoDailyMailFileSummaryTable(ByVal ScanDate As DateTime, ByVal MailServer As String, ByVal FileCount As Long, ByVal TotalFileSize As Long)
		Dim strSQL As String
		'**
		Dim myPath As String
		Dim myRegistry As New RegistryHandler
		'Read the registry for the location of the Config Database


		Dim myConnection As New Data.OleDb.OleDbConnection
		Try
			myPath = myRegistry.ReadFromRegistry("Application Path")
			' debug.writeline(Now.ToString & " Domino Update e database " & myPath)
		Catch ex As Exception
			Debug.WriteLine(Now.ToString & " Failed to read registry in Domino Response Table module. Exception: " & ex.Message)
		End Try

		If myPath Is Nothing Then
			Debug.WriteLine(Now.ToString & " Error: Failed to read registry in Domino Status module.   Cannot locate Config Database 'status.mdb'.  Configure by running" & ProductName & " client before starting the service.")
			'   Return False
			Exit Sub
		End If
		myRegistry = Nothing


		'Delete any existing data for today, if any
		Dim objVSAdaptor As New VSAdaptor
		Try
			strSQL = "DELETE FROM DailySummary WHERE ScanDate=#" & FixDate(ScanDate) & "# AND MailServer='" & MailServer & "'"
			'myCommand.CommandText = strSQL
			'myCommand.ExecuteNonQuery()
			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "mailfilestats", strSQL)
		Catch ex As Exception
			Debug.WriteLine(Now.ToString & " Count Mail Files delete command failed because: " & ex.Message)
			Debug.WriteLine(Now.ToString & " The failed Count Mail Files delete  command was " & strSQL)
		End Try

		'Insert the new data for today
		Try
			'somarju
			strSQL = "INSERT INTO DailySummary (MailServer, ScanDate, MailFileCount, MailFileSize )" & _
			 " VALUES ('" & MailServer & "', '" & FixDateTime(ScanDate) & "', '" & FileCount & "', '" & TotalFileSize & "')"

			'myCommand.CommandText = strSQL
			'myCommand.ExecuteNonQuery()
			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "NSFHealth", strSQL)


		Catch ex As Exception

		End Try

		Try
			strSQL = Nothing
			'myConnection.Close()
			'myConnection.Dispose()
			'myCommand.Dispose()
		Catch ex As Exception

		End Try

		Try
			GC.Collect()
		Catch ex As Exception

		End Try
	End Sub

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

	Private Sub EmptyTravelerDeviceTable(ByVal ServerName As String)
		Dim strSQL As String
		Try
			strSQL = "Delete FROM Traveler_Devices WHERE ServerName='" & ServerName & "'"
			Dim objVSAdaptor As New VSAdaptor
			objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
		Catch ex As Exception

		End Try

	End Sub

	Private Sub updateTravelerDeviceCollection(ByVal MoreInfoSQLs As List(Of SqlCommand))
		For Each t As SqlCommand In MoreInfoSQLs
			Dim objVSAdaptor As New VSAdaptor
			Try
				objVSAdaptor.ExecuteNonQuerySQLParams("VitalSigns", t)
				'WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Updated the data using the SQL = " & t)
			Catch ex As Exception
				WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & "  Error updating Traveler Device table: " & ex.ToString)
			End Try
		Next

	End Sub
	Private Sub addTravelerDeviceMoreInfoToCollection(ByRef travelerDeviceSQLs As List(Of SqlCommand), ByRef Device As TravelerDevice)
		If Device.DeviceName.Trim = "" Then Exit Sub

		Dim sqlcmd As New SqlCommand("Update Traveler_Devices_Temp SET UserName= @UserName" & _
  ", DeviceName= @DeviceName" & _
  ", OS_Type=@OS_Type" & _
  ", Client_Build=@Client_Build" & _
  ", Access=@Access" & _
  ", wipeSupported=@wipeSupported" & _
  ", SyncType=@SyncType" & _
  ", Security_Policy=@ApprovalPolicy" & _
  ", Approval=@Approval" & _
  ", LastSyncTime=@LastSyncTime" & _
  ", IsMoreDetailsFetched=1" & _
  ", LastUpdated=@LastUpdated" & _
  "  WHERE DeviceID=@DeviceID AND ServerName=@SN")

		sqlcmd.Parameters.Add(New SqlParameter("@Client_Build", SqlDbType.NVarChar, 510))
		sqlcmd.Parameters("@Client_Build").Value = Device.Client_Build.ToString()

		sqlcmd.Parameters.Add(New SqlParameter("@DeviceID", SqlDbType.NVarChar, 300))
		sqlcmd.Parameters("@DeviceID").Value = Device.DeviceID.ToString()

		sqlcmd.Parameters.Add(New SqlParameter("@SN", SqlDbType.NVarChar, 510))
		sqlcmd.Parameters("@SN").Value = Device.ServerName.ToString()

		sqlcmd.Parameters.Add(New SqlParameter("@UserName", SqlDbType.NVarChar, 510))
		sqlcmd.Parameters("@UserName").Value = Device.UserName.ToString()

		sqlcmd.Parameters.Add(New SqlParameter("@DeviceName", SqlDbType.NVarChar, 510))
		sqlcmd.Parameters("@DeviceName").Value = Device.DeviceName.ToString()

		sqlcmd.Parameters.Add(New SqlParameter("@LastSyncTime", SqlDbType.DateTime))
		sqlcmd.Parameters("@LastSyncTime").Value = FixDateTime(Device.LastSyncTime)

		sqlcmd.Parameters.Add(New SqlParameter("@OS_Type", SqlDbType.NVarChar, 510))
		sqlcmd.Parameters("@OS_Type").Value = Device.OS_Type.ToString()

		sqlcmd.Parameters.Add(New SqlParameter("@wipeSupported", SqlDbType.NVarChar, 510))
		sqlcmd.Parameters("@wipeSupported").Value = Device.wipeSupported.ToString()

		sqlcmd.Parameters.Add(New SqlParameter("@SyncType", SqlDbType.NVarChar, 510))
		sqlcmd.Parameters("@SyncType").Value = Device.AutoSyncType.ToString()

		sqlcmd.Parameters.Add(New SqlParameter("@Access", SqlDbType.NVarChar, 510))
		sqlcmd.Parameters("@Access").Value = Device.Access.ToString()

		sqlcmd.Parameters.Add(New SqlParameter("@ApprovalPolicy", SqlDbType.NVarChar, 510))
		sqlcmd.Parameters("@ApprovalPolicy").Value = Device.ApprovalPolicy.ToString()

		sqlcmd.Parameters.Add(New SqlParameter("@Approval", SqlDbType.NVarChar, 510))
		sqlcmd.Parameters("@Approval").Value = Device.Approval.ToString()

		sqlcmd.Parameters.Add(New SqlParameter("@LastUpdated", SqlDbType.DateTime))
		sqlcmd.Parameters("@LastUpdated").Value = FixDateTime(Now)

		
		'strSQL = strSQL.Replace("NaN", "1")
		'WriteDeviceHistoryEntry("Domino", "TravelerMoreDetails", Now.ToString & "  SQL to update more details: " & strSQL)
		'travelerDeviceSQLs.Add(sqlcmd)
		Dim objVSAdaptor As New VSAdaptor
		Try
			objVSAdaptor.ExecuteNonQuerySQLParams("VitalSigns", sqlcmd)

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

		Dim strSQL As String
		' WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Updating traveler device table.")
		With Device
			strSQL = "Update Traveler_Devices SET UserName= '" & .UserName & _
			"', DeviceName= '" & .DeviceName & _
			"', ConnectionState='" & .ConnectionState & _
			"', OS_Type='" & .OS_Type & _
			"', OS_Type_Min='" & .OS_Type_Min & _
			"', Client_Build='" & .Client_Build & _
			"', SyncType='" & .AutoSyncType & _
			"', LastSyncTime='" & FixDateTime(.LastSyncTime) & _
			"', MoreDetailsURL='" & .href & _
			"', LastUpdated='" + FixDateTime(Now) & _
			"', HAPoolName='" + HAPoolName & _
			"'  WHERE DeviceID='" & .DeviceID & "' AND ServerName='" & ServerName + "'"
		End With

		strSQL = strSQL.Replace("NaN", "1")
		Dim strSQLInsert As String

		With Device
			strSQLInsert = "INSERT INTO Traveler_Devices (Client_Build, DeviceID, ServerName, UserName, DeviceName, LastSyncTime, OS_Type,  OS_Type_Min, SyncType, MoreDetailsURL,LastUpdated,HAPoolName) " & _
			 " VALUES ('" & .Client_Build & "', '" & .DeviceID & "', '" & ServerName & "', '" & .UserName & "', '" & .DeviceName & "', '" & FixDateTime(.LastSyncTime) & "', '" & .OS_Type & "', '" & .OS_Type_Min & "','" & .AutoSyncType & "','" & .href + "','" & FixDateTime(Now) + "','" + HAPoolName + "')"
		End With

		strSQLInsert = strSQLInsert.Replace("NaN", "1")
		Dim objVSAdaptor As New VSAdaptor
		Try
			If objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL) = 0 Then
				WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " user not found with deviceid.Inserting new record: device id:" & Device.DeviceID)
				WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " user not found with deviceid.Inserting new record: update SQL:" & strSQL)
				objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQLInsert)
			End If
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
	Private Sub UpdateTravelerDeviceStatusDataTable(ByVal Device As TravelerDevice, ByVal ServerName As String, HAPoolName As String, ByRef dt As DataTable)
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
			Dim row As DataRow = dt.NewRow()

			row("DeviceID") = Device.DeviceID.ToString().Trim()
			row("ClientBuild") = Device.Client_Build.ToString().Trim()
			row("ServerName") = ServerName.ToString().Trim()
			row("UserName") = Device.UserName.ToString().Trim()
			row("DeviceName") = Device.DeviceName.ToString().Trim()
			row("LastSyncTime") = FixDateTime(Device.LastSyncTime)
			row("OS_Type") = Device.OS_Type.ToString().Trim()
			row("OS_Type_Min") = Device.OS_Type_Min.ToString().Trim()
			row("SyncType") = Device.AutoSyncType.ToString().Trim()
			row("href") = Device.href.ToString().Trim()
			row("LastUpdated") = FixDateTime(Now)

			If HAPoolName = "" Then
				row("HAPoolName") = ""
			Else
				row("HAPoolName") = HAPoolName.ToString().Trim()
			End If

			dt.Rows.Add(row)

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

	Public Sub SetActiveDevices()
		Dim con As New SqlConnection

		Try
			Dim myAdapter As New VSFramework.XMLOperation
			con.ConnectionString = myAdapter.GetDBConnectionString("Vitalsigns")
			con.Open()
			Dim da As SqlDataAdapter
			da = New SqlDataAdapter("PR_SetActiveDevices", con)
			da.SelectCommand.CommandType = CommandType.StoredProcedure
			da.SelectCommand.CommandTimeout = 60 * 1000 * 5
			da.SelectCommand.ExecuteNonQuery()

		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Exception in Setting ActiveDevices: " & ex.Message)
		Finally
			con.Close()
		End Try

	End Sub
    Private Sub UpdateTravelerServerStatusTable(ByVal TravelerVersion As String, ByVal HTTP_MaxConfiguredSessions As Integer, ByVal HTTP_PeakSessions As Integer, ByVal HTTP_Status As String, ByVal HTTP_Details As String, ByVal ServerName As String, ByVal Status As String, ByVal Details As String, ByVal Users As Integer, ByVal IncrementalSyncs As Integer)
		'*************************************************************
		'Update the Status Table
		'*************************************************************
		Dim strSQL As String
		WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Updating traveler status table.")


		strSQL = "Update Traveler_Status SET Status= '" & Status & _
		"', Details= '" & Details & _
		"', Users='" & Users & _
		"', TravelerVersion='" & TravelerVersion & _
		"', HTTP_PeakConnections='" & HTTP_PeakSessions & _
		"', HTTP_MaxConfiguredConnections='" & HTTP_MaxConfiguredSessions & _
		"', HTTP_Status='" & HTTP_Status & _
		"', HTTP_Details='" & HTTP_Details & _
		"'  WHERE ServerName='" & ServerName & "'"


		Dim strSQLInsert As String

		strSQLInsert = "INSERT INTO Traveler_Status (TravelerServlet, HTTP_PeakConnections, HTTP_MaxConfiguredConnections, ServerName, Status, Details, Users, IncrementalSyncs, HTTP_Status, HTTP_Details, DominoServerId) " & _
		 " VALUES ('Not Checked', " & HTTP_PeakSessions & ", " & HTTP_MaxConfiguredSessions & ", '" & ServerName & "', '" & Status & "', '" & Details & "', '" & Users & "', '" & IncrementalSyncs & "', '" & HTTP_Status & "', '" & HTTP_Details & "',(SELECT ID FROM Servers WHERE ServerName='" & ServerName & "' AND ServerTypeId=1))"


		strSQLInsert = strSQLInsert.Replace("NaN", "1")

		WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Attempting to update status with " & strSQL)
		'UpdateStatusTable(strSQL, strSQLInsert)

		Dim objVSAdaptor As New VSAdaptor
		Try
			If objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL) = 0 Then
				WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Traveler status was not updated, attempting an INSERT instead.")
				WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " SQL Statements for Traveler Status are " & vbCrLf & strSQL & vbCrLf & strSQLInsert)
				objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQLInsert)
			End If
		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & "  Error updating Traveler Status table: " & ex.ToString)
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
        Dim strSQL As String
        
        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Entered UpdateDominoServerDetailsTable")

        strSQL = "SELECT COUNT(*) FROM DominoServerDetails WHERE ServerID = " & DominoServer.Key & " "

        Dim objVSAdaptor As New VSAdaptor
        Try
            Dim myResult As Integer
            myResult = objVSAdaptor.ExecuteScalarAny("vitalsigns", "vitalsigns", strSQL)
            If Not (myResult = Nothing) Then
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " My result was " & myResult)
                Try
                    strSQL = "UPDATE DominoServerDetails SET ElapsedTimeSeconds = " & DominoServer.ElapsedTime & ", "
                    strSQL += " VersionArchitecture='" & DominoServer.VersionArchitecture & "', CPUCount=" & DominoServer.CPUCount & " "
                    strSQL += " WHERE ServerID=" & DominoServer.Key & " "
                    objVSAdaptor.ExecuteNonQueryAny("vitalsigns", "vitalsigns", strSQL)
                Catch ex2 As Exception
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error Updating DominoServerDetails: " & ex2.Message & vbCrLf & strSQL)
                End Try
            Else
                Try
                    strSQL = "INSERT INTO DominoServerDetails (ServerID,ElapsedTimeSeconds,VersionArchitecture,CPUCount)" & _
                        " VALUES (" & DominoServer.Key & ", " & DominoServer.ElapsedTime & ", '" & DominoServer.VersionArchitecture & "', " & DominoServer.CPUCount & ")"
                    objVSAdaptor.ExecuteNonQueryAny("vitalsigns", "vitalsigns", strSQL)
                Catch ex2 As Exception
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error Inserting into DominoServerDetails: " & ex2.Message & vbCrLf & strSQL)
                End Try

            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error occurred in UpdateDominoServerDetailsTable: " & ex.Message)
        End Try
 
        Try
            GC.Collect()
        Catch ex As Exception

        End Try
    End Sub
#End Region



	Private Sub RecordCountAvailability(ByVal DeviceType As String, ByVal DeviceName As String, ByVal UpPercent As Double)
		'This sub records the overall availability based on up and down counts
		WriteAuditEntry(Now.ToString & " Writing hourly Count Availability stats for " & DeviceName)

		Dim strSQL As String
		Dim MyWeekNumber As Integer
		Dim StatDate As DateTime = Now

		MyWeekNumber = GetWeekNumber(StatDate)
		Dim objVSAdaptor As New VSAdaptor
		Try
			strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
			 " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyUpPercent', '" & UpPercent & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
			'  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)
			'If boolUseSQLServer = True Then
			'    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
			'Else
			'    OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
			'    OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
			'End If


			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " RecordCountAvailability insert failed because: " & ex.Message)
			WriteAuditEntry(Now.ToString & " The failed RecordCountAvailability insert command was " & strSQL)
		End Try

		strSQL = Nothing
		MyWeekNumber = Nothing
		StatDate = Nothing

	End Sub

	Private Sub RecordOnTargetAvailability(ByVal DeviceType As String, ByVal DeviceName As String, ByVal OnTargetPercent As Double)
		WriteAuditEntry(Now.ToString & " Writing hourly On-Target Availability stats for " & DeviceName)
		'This sub records the percentage of times the device responded within its target response time
		Dim strSQL As String = ""
		Dim MyWeekNumber As Integer
		Dim StatDate As DateTime = Now

		MyWeekNumber = GetWeekNumber(StatDate)
		Dim objVSAdaptor As New VSAdaptor
		Try
			strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
			 " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyOnTargetPercent', '" & OnTargetPercent & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
			'  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)

			'If boolUseSQLServer = True Then
			'    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
			'Else
			'    OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
			'    OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
			'End If
			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Record On Target Availability insert failed becase: " & ex.Message)
			WriteAuditEntry(Now.ToString & " The failed Record On Target Availability insert comand was " & strSQL)
		End Try
		strSQL = Nothing
		MyWeekNumber = Nothing
		StatDate = Nothing

	End Sub

	Private Sub RecordBusinessHoursOnTargetAvailability(ByVal DeviceType As String, ByVal DeviceName As String, ByVal OnTargetPercent As Double)
		WriteAuditEntry(Now.ToString & " Writing hourly Business Hours On-Target Availability stats for " & DeviceName)
		'This sub records the percentage of times the device responded within its target response time during business Hours only
		Dim strSQL As String
		Dim MyWeekNumber As Integer
		Dim StatDate As DateTime = Now

		MyWeekNumber = GetWeekNumber(StatDate)
		Dim objVSAdaptor As New VSAdaptor
		Try
			strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
			 " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyBusinessHoursOnTargetPercent', '" & OnTargetPercent & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
			'  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)

			'If boolUseSQLServer = True Then
			'    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
			'Else
			'    OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
			'    OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
			'End If
			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Record On Target Availability insert failed becase: " & ex.Message)
			WriteAuditEntry(Now.ToString & " The failed Record On Target Availability insert comand was " & strSQL)
		End Try
		strSQL = Nothing
		MyWeekNumber = Nothing
		StatDate = Nothing


	End Sub

	Private Sub RecordTimeAvailability(ByVal DeviceType As String, ByVal DeviceName As String, ByVal UpPercent As Double)
		'This sub records the overall availability based on up and down counts
		WriteAuditEntry(Now.ToString & " Writing hourly Time Availability stats for " & DeviceName)

		Dim strSQL As String
		Dim MyWeekNumber As Integer
		Dim StatDate As DateTime = Now

		MyWeekNumber = GetWeekNumber(StatDate)
		Dim objVSAdaptor As New VSAdaptor
		Try
			strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
			 " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyUpTimePercent', '" & UpPercent & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
			'  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)
			'OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
			'OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " RecordTimeAvailability insert failed because: " & ex.Message)
			WriteAuditEntry(Now.ToString & " The failed RecordTimeAvailability insert command was " & strSQL)
		End Try

		strSQL = Nothing
		MyWeekNumber = Nothing
		StatDate = Nothing

	End Sub

	Private Sub RecordDownTime(ByVal DeviceType As String, ByVal DeviceName As String, ByVal DownMinutes As Double)
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
			strSQL = "INSERT INTO DeviceUpTimeStats (DeviceType, DeviceName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
			 " VALUES ('" & DeviceType & "', '" & DeviceName & "', '" & StatDate.ToString & "', 'HourlyDownTimeMinutes', '" & DownMinutes & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
			'  WriteAuditEntry(Now.ToString & " Updating hourly stats with : " & strSQL)

			'If boolUseSQLServer = True Then
			'    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
			'Else
			'    OleDbDataAdapterDeviceDailyStats.InsertCommand.CommandText = strSQL
			'    OleDbDataAdapterDeviceDailyStats.InsertCommand.ExecuteNonQuery()
			'End If
			objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " RecordTimeAvailability insert failed because: " & ex.Message)
			WriteAuditEntry(Now.ToString & " The failed RecordTimeAvailability insert command was " & strSQL)
		End Try

		strSQL = Nothing
		MyWeekNumber = Nothing
		StatDate = Nothing

	End Sub

	Private Function FixDateTime(ByVal dt As DateTime) As String
		' Return dt.ToUniversalTime.ToString
		Dim strDate As String
		strDate = FixDate(dt) & " " & dt.ToShortTimeString
		' strDate = FixDate_OLD(dt) & " " & dt.ToShortTimeString

		Return strDate

		' Return objDateUtils.FixDateTime(dt, strDateFormat)


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
