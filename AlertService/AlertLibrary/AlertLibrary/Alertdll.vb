Imports System.Threading
Imports System.IO
Imports System.Data.SqlClient
'Imports nsoftware.IPWorks
Imports VSFramework
Imports LogUtilities.LogUtils


Public Class Alertdll

#Region "Declarations"
    Private strAppPath As String
    Dim strAuditText As String
    ' Private strConsoleCommandLogDest As String
    Private strLogDest As String
    Dim myAdapter As New VSFramework.XMLOperation
    Dim connectionString As String = myAdapter.GetDBConnectionString("VitalSigns")
    Dim VSSconnectionString As String = myAdapter.GetDBConnectionString("VSS_Statistics")

#End Region

    Enum LogLevel
		Verbose = LogUtilities.LogUtils.LogLevel.Verbose
		Debug = LogUtilities.LogUtils.LogLevel.Debug
		Normal = LogUtilities.LogUtils.LogLevel.Normal
	End Enum

	Dim MyLogLevel As LogLevel
#Region "Alerts"
	Private Sub getAlertKey(ByVal DeviceType As String, ByVal DeviceName As String, ByVal AlertType As String, ByVal Details As String, ByVal Location As String)


	End Sub

	Public Sub QueueAlert(ByVal DeviceType As String, ByVal DeviceName As String, ByVal AlertType As String, ByVal Details As String, ByVal Location As String,
	 Optional ByVal Category As String = "")
		'VSPLUS-930,Mukund, 15Sep14 Added Category parameter
		'12/17/2014 NS commented out for VSPLUS-1267
		'Dim NowTime As String = Now.ToString
		Dim strSQL As String
		Dim Sqlcon As New SqlConnection(connectionString)
		Dim vssSqlcon As New SqlConnection(VSSconnectionString)
		Dim DA As SqlDataAdapter
		Dim Ds As DataSet
		Dim dt As DataTable
		Dim DA1 As SqlDataAdapter
		Dim Ds1 As New DataSet
		Dim dt1 As DataTable
		Dim strSQL1 As String
		Dim strSQL2 As String
		Dim sqlcmd As New SqlCommand
		'4/7/2015 NS added for VSPLUS-1629
		Dim sqlparam1 As New SqlParameter()
		Dim sqlparam2 As New SqlParameter()
		Dim sqlparam3 As New SqlParameter()
		Dim sqlparam4 As New SqlParameter()
		Dim sqlparam5 As New SqlParameter()
        Dim sqlparam6 As New SqlParameter()
		'If MyLogLevel = LogLevel.Verbose Then
		'WriteAuditEntry(NowTime & " Queuing Alert for " & DeviceType & "/" & DeviceName & " " & AlertType)
		'End If
		'10/20/2014 NS modified for VSPLUS-730
		'1. Get the value of the Settings flag AlertsRepeatOn
		Dim qalert As Boolean
		Dim DeviceList As Boolean

		Dim AlertsRepeatOn As Boolean
		Dim AlertsRepeatOccurrences As Integer
		Dim CurrentRepeatOccurrences As Integer
		'12/17/2014 NS added for VSPLUS-1267
		Dim myConnectionString As New VSFramework.XMLOperation
		Dim myAdapter As New VSFramework.VSAdaptor
		Dim objDateUtils As New DateUtils.DateUtils
		Dim strDateFormat As String
		Dim NowTime As String
		strDateFormat = objDateUtils.GetDateFormat()
		NowTime = objDateUtils.FixDateTime(Date.Now, strDateFormat)

		qalert = True
		AlertsRepeatOn = False
		DeviceList = True
		AlertsRepeatOccurrences = 0
		Try
			'12/17/2014 NS modified for VSPLUS-1267
			AlertsRepeatOn = Boolean.Parse(getSettings("AlertsRepeatOn"))
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error getting AlertsRepeatOn option from the Settings table:  " & ex.ToString)
			WriteAuditEntry(Now.ToString & " Error getting AlertsRepeatOn option from the Settings table:  " & ex.ToString)
		End Try
		'2. If the flag is set, check whether the current event type is set for repeat occurrence alert
		If AlertsRepeatOn Then
			Try
				'12/17/2014 NS modified for VSPLUS-1267
				AlertsRepeatOccurrences = Convert.ToInt32(getSettings("AlertsRepeatOccurrences"))
			Catch ex As Exception
				WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error getting AlertsRepeatOccurrences option from the Settings table:  " & ex.ToString)
				WriteAuditEntry(Now.ToString & " Error getting AlertsRepeatOccurrences option from the Settings table:  " & ex.ToString)
			End Try
			WriteDeviceHistoryEntry("All", "Alerts", NowTime & " AlertsRepeatOn flag is set. Checking if an alert is due to be queued for " & DeviceType & "/" & DeviceName & " " & AlertType)
			Try
				strSQL = "SELECT AlertOnRepeat FROM EventsMaster t1 INNER JOIN ServerTypes t2 ON t1.ServerTypeID=t2.ID " & _
				"WHERE EventName='" & AlertType & "' AND t2.ServerType='" & DeviceType & "' AND AlertOnRepeat=1"
				'12/17/2014 NS modified for VSPLUS-1267
				'DA = New SqlDataAdapter(strSQL, Sqlcon)
				'Ds = New DataSet
				'DA.Fill(Ds, "RepeatEvents")
				'dt = Ds.Tables(0)
				dt = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), strSQL)
				If dt.Rows.Count > 0 Then
					'2.1 The repeat occurrence flag is set for the event type, continue processing
					Try
						strSQL1 = "SELECT COUNT(*) TotalCount FROM AlertRepeatOccurrence WHERE DeviceType='" & DeviceType & _
						"' AND AlertType='" & AlertType & "' AND DeviceName='" & DeviceName & "' " & _
						"AND DATEDIFF(mi,DateCreated,GETDATE()) <= 60"
						'12/17/2014 NS modified for VSPLUS-1267
						'DA1 = New SqlDataAdapter(strSQL1, Sqlcon)
						'Ds1 = New DataSet
						'DA1.Fill(Ds1, "RepeatEventsCount")
						'dt1 = Ds1.Tables(0)
						dt1 = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), strSQL1)
						If dt1.Rows.Count > 0 Then
							CurrentRepeatOccurrences = Convert.ToInt32(dt1.Rows(0)("TotalCount"))
							If CurrentRepeatOccurrences + 1 >= AlertsRepeatOccurrences Then
								'2.1.1 Remove all event type entries for the alert type and device from the AlertRepeatOccurrence table, then queue the alert in step 3.
								Try
									strSQL2 = "DELETE FROM AlertRepeatOccurrence WHERE DeviceType='" & DeviceType & _
							   "' AND AlertType='" & AlertType & "' AND DeviceName='" & DeviceName & "' "
									'12/17/2014 NS modified for VSPLUS-1267
									'sqlcmd = New SqlCommand(strSQL2, Sqlcon)
									'Sqlcon.Open()
									'sqlcmd.ExecuteNonQuery()
									'Sqlcon.Close()
									myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL2)
								Catch ex As Exception
									WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error deleting rows from the AlertRepeatOccurrence table prior to alert queueing for " & AlertType & " for " & DeviceType & " for " & DeviceName & ":  " & ex.ToString)
									WriteAuditEntry(Now.ToString & " Error deleting rows from the AlertRepeatOccurrence table prior to alert queueing for " & AlertType & " for " & DeviceType & " for " & DeviceName & ":  " & ex.ToString)
								End Try
							Else
								'2.1.2 Remove records that are older than an hour from the current time
								Try
									strSQL2 = "DELETE FROM AlertRepeatOccurrence WHERE DeviceType='" & DeviceType & _
							   "' AND AlertType='" & AlertType & "' AND DeviceName='" & DeviceName & "' " & _
							   "AND DATEDIFF(mi,DateCreated,GETDATE()) > 60"
									'12/17/2014 NS modified for VSPLUS-1267
									'sqlcmd = New SqlCommand(strSQL2, Sqlcon)
									'Sqlcon.Open()
									'sqlcmd.ExecuteNonQuery()
									'Sqlcon.Close()
									myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL2)
								Catch ex As Exception
									WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error deleting rows from the AlertRepeatOccurrence table older than 1 hour for " & AlertType & " for " & DeviceType & " for " & DeviceName & ":  " & ex.ToString)
									WriteAuditEntry(Now.ToString & " Error deleting rows from the AlertRepeatOccurrence table older than 1 hour for " & AlertType & " for " & DeviceType & " for " & DeviceName & ":  " & ex.ToString)
								End Try
								'2.1.3 Insert a new record into the AlertRepeatOccurrence for the current event
								Try
									strSQL2 = "INSERT INTO AlertRepeatOccurrence(DeviceName,DeviceType,AlertType,DateCreated) " & _
							   "VALUES('" & DeviceName & "','" & DeviceType & "','" & AlertType & "','" & Now.ToString() & "')"
									'12/17/2014 NS modified for VSPLUS-1267
									'sqlcmd = New SqlCommand(strSQL2, Sqlcon)
									'Sqlcon.Open()
									'sqlcmd.ExecuteNonQuery()
									'Sqlcon.Close()
									myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL2)
								Catch ex As Exception
									WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error inserting rows into the AlertRepeatOccurrence table for " & AlertType & " for " & DeviceType & " for " & DeviceName & ":  " & ex.ToString)
									WriteAuditEntry(Now.ToString & " Error inserting rows into the AlertRepeatOccurrence table for " & AlertType & " for " & DeviceType & " for " & DeviceName & ":  " & ex.ToString)
								End Try
								'Do not queue an alert yet as the current number of occurrences hasn't reached the threshold yet
								qalert = False
							End If
						End If
					Catch ex As Exception
						WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error getting a count of rows from the AlertRepeatOccurrence table for " & AlertType & " for " & DeviceType & " for " & DeviceName & ":  " & ex.ToString)
						WriteAuditEntry(Now.ToString & " Error getting a count of rows from the AlertRepeatOccurrence table for " & AlertType & " for " & DeviceType & " for " & DeviceName & ":  " & ex.ToString)
					End Try
				End If
			Catch ex As Exception
				WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error getting AlertOnRepeat value from the EventsMaster table for " & AlertType & " for " & DeviceType & ":  " & ex.ToString)
				WriteAuditEntry(Now.ToString & " Error getting AlertOnRepeat value from the EventsMaster table for " & AlertType & " for " & DeviceType & ":  " & ex.ToString)
			End Try
		End If
		'3. If the AlertsRepeatOn flag is off/false or there are enough entries in the AlertRepeatOccurrence table for the
		'current event type, queue an alert as usual
		If qalert Then
			WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Queuing Alert for " & DeviceType & "/" & DeviceName & " " & AlertType)
			Try
                '5/6/2016 NS modified - if Location is NULL
                Dim SQLstr As String = ""
                If Location = "" Then
                    SQLstr = "select * from AlertHistory where DeviceName='" & DeviceName & "' and DeviceType ='" & DeviceType & "' and AlertType='" & AlertType & "' and (Location='" & Location & "' OR Location IS NULL) and DateTimeAlertCleared is Null"
                Else
                    SQLstr = "select * from AlertHistory where DeviceName='" & DeviceName & "' and DeviceType ='" & DeviceType & "' and AlertType='" & AlertType & "' and Location='" & Location & "' and DateTimeAlertCleared is Null"
                End If
                WriteDeviceHistoryEntry("All", "Alerts", NowTime & " SQLstr: " & SQLstr & "," & DeviceType & "/" & DeviceName & " " & AlertType, LogLevel.Verbose)
				'12/17/2014 NS modified for VSPLUS-1267
				'DA = New SqlDataAdapter(SQLstr, Sqlcon)
				'Ds = New DataSet
				'DA.Fill(Ds, "AlertHistory")
				'dt = Ds.Tables(0)
				dt = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), SQLstr)
				If dt.Rows.Count = 0 Then

					If MyLogLevel = LogLevel.Verbose Then
						WriteDeviceHistoryEntry("All", "Alerts", NowTime & " This alert is new, adding to collection")

					End If
					'4/7/2015 NS modified for VSPLUS-1629
					'strSQL = "INSERT INTO AlertHistory (DeviceName,  DeviceType, AlertType, DateTimeOfAlert,Location,Details) " & _
					'                 " VALUES ('" & DeviceName & "', '" & DeviceType & "', '" & AlertType & "', '" & NowTime & "','" & Location & "','" & Details & "')"
					sqlcmd = New SqlCommand("INSERT INTO AlertHistory (DeviceName,  DeviceType, AlertType, DateTimeOfAlert,Location,Details) " & _
						" VALUES (@DeviceName,@DeviceType,@AlertType,@NowTime,@Location,@Details)")
					sqlparam1.ParameterName = "@DeviceName"
					sqlparam1.Value = DeviceName
					sqlcmd.Parameters.Add(sqlparam1)
					sqlparam2.ParameterName = "@DeviceType"
					sqlparam2.Value = DeviceType
					sqlcmd.Parameters.Add(sqlparam2)
					sqlparam3.ParameterName = "@AlertType"
					sqlparam3.Value = AlertType
					sqlcmd.Parameters.Add(sqlparam3)
					sqlparam4.ParameterName = "@NowTime"
					sqlparam4.Value = NowTime
					sqlcmd.Parameters.Add(sqlparam4)
                    sqlparam5.ParameterName = "@Location"
                    '5/5/2016 NS modified
                    If Location = "" Then
                        sqlparam5.Value = DBNull.Value
                    Else
                        sqlparam5.Value = Location
                    End If
					sqlcmd.Parameters.Add(sqlparam5)
					sqlparam6.ParameterName = "@Details"
					sqlparam6.Value = Details
					sqlcmd.Parameters.Add(sqlparam6)

					Try
						'12/17/2014 NS modified for VSPLUS-1267
						'sqlcmd = New SqlCommand(strSQL, Sqlcon)
						'Sqlcon.Open()
						'sqlcmd.ExecuteNonQuery()
						'Sqlcon.Close()
						'4/7/2015 NS modified for VSPLUS-1629
						'myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)
						WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Query parameters: " & sqlparam1.Value & "; " & sqlparam2.Value & "; " & sqlparam3.Value & "; " & sqlparam4.Value & "; " & sqlparam5.Value & "; " & sqlparam6.Value, LogLevel.Verbose)
						myAdapter.ExecuteNonQuerySQLParams("VitalSigns", sqlcmd)
					Catch ex As Exception
						'4/6/2015 NS added
						WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error executing query: " & sqlcmd.CommandText & " - " & ex.ToString)
						WriteAuditEntry(NowTime & " Alert History Insert Error " & ex.Message)
					End Try

					'For Outages entry: Mukund 22Oct13
					If AlertType = "Not Responding" Then
						WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Outages insert started: " & DeviceType & "/" & DeviceName & " " & AlertType)
						SQLstr = "select srv.ID,srv.ServerTypeId from Servers srv where srv.ServerName='" & DeviceName &
					  "' union select srv.ID,srv.ServerTypeId from URLs srv where srv.Name='" & DeviceName &
					  "' union select [key],(select ID from ServerTypes where servertype='Mail') ServerTypeId  from MailServices where Name='" & DeviceName &
					  "' union select id,(select ID from ServerTypes where servertype='Network Device') ServerTypeId  from [Network Devices] where Name='" & DeviceName & "'"
						'WriteDeviceHistoryEntry("All", "Alerts", NowTime & " outages strSQL: " & SQLstr & "," & DeviceType & "/" & DeviceName & " " & AlertType)
						'12/17/2014 NS modified for VSPLUS-1267
						'DA1 = New SqlDataAdapter(SQLstr, Sqlcon)
						'Ds1 = New DataSet
						'DA1.Fill(Ds1, "servers")
						'dt1 = Ds1.Tables(0)
						dt1 = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), SQLstr)
						If dt1.Rows.Count > 0 Then

							strSQL = "INSERT INTO Outages(ServerID,DateTimeDown,Description,ServerTypeID) " & _
							 " VALUES ('" & dt1.Rows(0)("ID").ToString & "', '" & NowTime & "', '" & Details & "','" & dt1.Rows(0)("ServerTypeId").ToString & "')"
							WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Outages insert: " & strSQL & "," & DeviceType & "/" & DeviceName & " " & AlertType)
							Try
								'12/17/2014 NS modified for VSPLUS-1267
								'sqlcmd = New SqlCommand(strSQL, vssSqlcon)
								'vssSqlcon.Open()
								'sqlcmd.ExecuteNonQuery()
								'vssSqlcon.Close()
								myAdapter.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", strSQL)
							Catch ex As Exception
								WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Outages Insert Error " & ex.Message & "," & DeviceType & "/" & DeviceName & " " & AlertType)
								WriteAuditEntry(NowTime & " Outages Insert Error " & ex.Message)
							End Try
						End If
					End If

				Else
					'VSPLUS-694, Send alerts with updated pending/dead mail counts:: Mukund 29Sep14,
					If (AlertType = "Dead Mail" Or AlertType = "Pending Mail" Or AlertType = "Held Mail") Then
						Try
							strSQL = "Update AlertHistory set   Details='" & Details & _
							 "'  where DeviceName='" & DeviceName & "' and DeviceType ='" & DeviceType & "' and AlertType='" & AlertType & "' and Location='" & Location & "' and DateTimeAlertCleared is Null"
							'12/17/2014 NS modified for VSPLUS-1267
							'sqlcmd = New SqlCommand(strSQL, vssSqlcon)
							'vssSqlcon.Open()
							'sqlcmd.ExecuteNonQuery()
							'vssSqlcon.Close()
							myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)
						Catch ex As Exception
							WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Alert History Update Error " & ex.Message & "," & DeviceType & "/" & DeviceName & " " & AlertType)
							WriteAuditEntry(NowTime & " Alert History Update Error " & ex.Message)
						End Try
					End If
					If MyLogLevel = LogLevel.Verbose Then
						WriteAuditEntry(NowTime & " This alert has already been queued: " & DeviceType & "/" & DeviceName & " " & AlertType)
						WriteDeviceHistoryEntry("All", "Alerts", NowTime & " This alert has already been queued: " & DeviceType & "/" & DeviceName & " " & AlertType)
					End If
				End If

				'Mukund 12Sep14,On Joe's suggestion, moved outside If condition above to get data in all conditions in StatusDetail
				'Mukund 14Jul14, VSPLUS-814 - StatusDetail insert/update sql
				'VSPLUS-930,Mukund, 15Sep14 pass Category parameter

				Dim DeviceTypelist() As String = {"Mail", "NotesMail Probe", "Notes Database", "Mobile Users"}
				For Each Type As String In DeviceTypelist
					If Type = DeviceType Then
						DeviceList = False
					End If
				Next
				If DeviceList Then
					If DeviceType = "Office365" Then
						UpdateStatusDetails(Location, DeviceName, AlertType, Details, Location, Category, "Fail", NowTime)
					Else
						UpdateStatusDetails(DeviceType, DeviceName, AlertType, Details, Location, Category, "Fail", NowTime)
					End If

				End If

			Catch ex As Exception
				WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error searching alerts: " & ex.Message)
				WriteAuditEntry(NowTime & " Error searching alerts: " & ex.Message)
			End Try

			GC.Collect()
		End If
	End Sub

	'Mukund 14Jul14, VSPLUS-814 - StatusDetail insert/update sql
	Private Sub UpdateStatusDetails(ByVal DeviceType As String, ByVal DeviceName As String, ByVal AlertType As String, ByVal Details As String, ByVal Location As String,
	  ByVal Category As String, ByVal Result As String, ByVal NowTime As String)

		'4/23/15 WS Added for 2.2 Release to reduce VSAdapter log files.  Currently, Mail Services and Notes MailProbes do not have a Health Page and the Notes Database does, but all 3 of them
		'are broken due to failed insert statements



		'If (DeviceType = "Mail" Or DeviceType = "NotesMail Probe" Or DeviceType = "Notes Database") Then
		'	Return
		'End If


		Dim iRetVal As Integer = 0
		Dim strSQL As String
		Dim strSQL1 As String
		Dim Sqlcon As New SqlConnection(connectionString)
		'12/17/2014 NS added for VSPLUS-1267
		Dim myConnectionString As New VSFramework.XMLOperation
		Dim myAdapter As New VSFramework.VSAdaptor
		Dim objDateUtils As New DateUtils.DateUtils
		Dim strDateFormat As String
		strDateFormat = objDateUtils.GetDateFormat()
		NowTime = objDateUtils.FixDateTime(Date.Now, strDateFormat)

		strSQL1 = "select * from StatusDetail where TypeAndName='" & DeviceName & "-" & DeviceType & "' and TestName='" & AlertType & "'"

		'12/17/2014 NS modified for VSPLUS-1267
		'Dim DA1 As New SqlDataAdapter(strSQL1, Sqlcon)
		'Dim Ds1 As New DataSet
		'DA1.Fill(Ds1, "servers")
		'Dim dt1 As DataTable = Ds1.Tables(0)
		Dim dt1 As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), strSQL1)
        If Details.Length > 498 Then
            Details = Details.Substring(0, 498)
        End If
        'VSPLUS-930,Mukund, 15Sep14 Added Category,Details parameter
        '5/6/2016 NS modified - changed the SQL statement to use a parametrized query
        Dim sqlcmd As New SqlCommand
        Dim sqlparam1 As New SqlParameter()
        Dim sqlparam2 As New SqlParameter()
        Dim sqlparam3 As New SqlParameter()
        Dim sqlparam4 As New SqlParameter()
        Dim sqlparam5 As New SqlParameter()
        Dim sqlparam6 As New SqlParameter()
		If dt1.Rows.Count > 0 Then
            'strSQL = "Update StatusDetail set LastUpdate='" & NowTime & "', Category ='" & IIf(Category = "", DeviceType, Category) & "',Details='" & Details & "',Result='" & Result & "' " & _
            '" where TypeAndName='" & DeviceName & "-" & DeviceType & "' and TestName='" & AlertType & "'"
            sqlcmd = New SqlCommand("UPDATE StatusDetail SET LastUpdate=@LastUpdate, Category=@Category, Details=@Details, Result=@Result " & _
                                    "WHERE TypeANDName=@TypeANDName AND TestName=@TestName")
            sqlcmd.Parameters.Add(New SqlParameter("@LastUpdate", SqlDbType.DateTime))
            sqlcmd.Parameters("@LastUpdate").Value = NowTime
            sqlcmd.Parameters.Add(New SqlParameter("@Category", SqlDbType.NVarChar, 100))
            sqlcmd.Parameters("@Category").Value = IIf(Category = "", DeviceType, Category)
            sqlcmd.Parameters.Add(New SqlParameter("@Details", SqlDbType.VarChar, 500))
            sqlcmd.Parameters("@Details").Value = Details
            sqlcmd.Parameters.Add(New SqlParameter("@Result", SqlDbType.NVarChar, 100))
            sqlcmd.Parameters("@Result").Value = Result
            sqlcmd.Parameters.Add(New SqlParameter("@TypeANDName", SqlDbType.NVarChar, 255))
            sqlcmd.Parameters("@TypeANDName").Value = DeviceName & "-" & DeviceType
            sqlcmd.Parameters.Add(New SqlParameter("@TestName", SqlDbType.NVarChar, 100))
            sqlcmd.Parameters("@TestName").Value = AlertType
		Else
            'strSQL = "INSERT INTO StatusDetail (TypeAndName,Category,TestName,Result,LastUpdate,Details)" & _
            '" VALUES('" & DeviceName & "-" & DeviceType & "','" & IIf(Category = "", DeviceType, Category) & "','" & AlertType & "','" & Result & "','" & NowTime & "','" & Details & "')"
            sqlcmd = New SqlCommand("INSERT INTO StatusDetail (TypeAndName,Category,TestName,Result,LastUpdate,Details) " & _
                " VALUES (@TypeAndName,@Category,@TestName,@Result,@LastUpdate,@Details)")
            sqlparam1.ParameterName = "@TypeAndName"
            sqlparam1.Value = DeviceName & "-" & DeviceType
            sqlcmd.Parameters.Add(sqlparam1)
            sqlparam2.ParameterName = "@Category"
            sqlparam2.Value = IIf(Category = "", DeviceType, Category)
            sqlcmd.Parameters.Add(sqlparam2)
            sqlparam3.ParameterName = "@TestName"
            sqlparam3.Value = AlertType
            sqlcmd.Parameters.Add(sqlparam3)
            sqlparam4.ParameterName = "@Result"
            sqlparam4.Value = Result
            sqlcmd.Parameters.Add(sqlparam4)
            sqlparam5.ParameterName = "@LastUpdate"
            sqlparam5.Value = NowTime
            sqlcmd.Parameters.Add(sqlparam5)
            sqlparam6.ParameterName = "@Details"
            sqlparam6.Value = Details
            sqlcmd.Parameters.Add(sqlparam6)
		End If
		'12/17/2014 NS modified for VSPLUS-1267
		'Dim sqlcmd As New SqlCommand(strSQL, Sqlcon)
		'Sqlcon.Open()
		'sqlcmd.ExecuteNonQuery()
		'Sqlcon.Close()
        'myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)
        Try
            myAdapter.ExecuteNonQuerySQLParams("VitalSigns", sqlcmd)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error executing query: " & sqlcmd.CommandText & " - " & ex.ToString)
            WriteAuditEntry(NowTime & " StatusDetails Insert/Update Error " & ex.Message)
        End Try

	End Sub
	Public Function getSettings(ByVal sname As String) As String
		'12/17/2014 NS modified for VSPLUS-1267
		'Dim str As String = ""
		'Try
		'    Dim sqlQuery As String = "Select svalue from Settings where sname='" & sname & "'"
		'    Dim SqlDA As New SqlDataAdapter(sqlQuery, con)
		'    Dim ds As New DataSet

		'    SqlDA.Fill(ds, "Settings")
		'    Dim dt As DataTable = ds.Tables(0)
		'    str = dt.Rows(0)("svalue").ToString()

		'Catch ex As Exception
		'    WriteDeviceHistoryEntry("All", "Alerts", Now.ToString & " Error occurred at the time of getting value of " & sname & " from Settings Table " & ex.Message)
		'    WriteAuditEntry(Now.ToString & " Error occurred at the time of getting value of " & sname & " from Settings Table " & ex.Message)
		'End Try

		'Return str
		Dim myConnectionString As New VSFramework.XMLOperation
		Dim myAdapter As New VSFramework.VSAdaptor
		Dim str As String = ""

		Try
			Dim sqlQuery As String = "Select svalue from Settings where sname='" & sname & "'"
			Dim dt As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sqlQuery)
			If dt.Rows.Count > 0 Then
				str = dt.Rows(0)("svalue").ToString()
			End If
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "Alerts", Now.ToString & " Error occurred at the time of getting value of " & sname & " from Settings Table " & ex.Message)
		End Try

		Return str
	End Function

	Private Function GetMaxAlertsQueuedToday(ByVal con As SqlConnection) As Integer
		'12/17/2014 NS added for VSPLUS-1267
		Dim myConnectionString As New VSFramework.XMLOperation
		Dim myAdapter As New VSFramework.VSAdaptor
		Dim iRetVal As Integer = 0
		Dim dailyCountSetting As String = ""
		'12/17/2014 NS modified for VSPLUS-1267
		'Dim strSQL As String = "SELECT svalue from SETTINGS WHERE sname='DailyAlertCountSetting'"
		'Dim MyCommand As New SqlCommand(strSQL, con)
		'con.Open()
		'Using MyDataReader As IDataReader = MyCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
		'    If MyDataReader.Read Then
		'        iRetVal = Convert.ToInt32(MyDataReader(0).ToString)
		'    End If
		'End Using
		Try
			dailyCountSetting = getSettings("DailyAlertCountSetting")
			If dailyCountSetting <> "" Then
				iRetVal = Convert.ToInt32(dailyCountSetting)
			End If
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "Alerts", Now.ToString & " Error getting DailyAlertCountSetting option from the Settings table:  " & ex.ToString)
		End Try
		Return iRetVal
	End Function
	Public Sub ResetAlert(ByVal DeviceType As String, ByVal DeviceName As String, ByVal AlertType As String, ByVal Location As String,
	 Optional ByVal Details As String = "", Optional ByVal Category As String = "")
		'VSPLUS-930,Mukund, 15Sep14 Added Category, Details parameters
		'12/17/2014 NS modified for VSPLUS-1267
		Dim myConnectionString As New VSFramework.XMLOperation
		Dim myAdapter As New VSFramework.VSAdaptor
		'Dim NowTime As String = Now.ToString
		Dim objDateUtils As New DateUtils.DateUtils
		Dim strDateFormat As String
		strDateFormat = objDateUtils.GetDateFormat()
		Dim NowTime As String = objDateUtils.FixDateTime(Date.Now, strDateFormat)
		WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Received notice to reset alert for " & DeviceType & "/" & DeviceName & ": " & AlertType)

		Try
			Dim Sqlcon As New SqlConnection(connectionString)
			Dim vssSqlcon As New SqlConnection(VSSconnectionString)

			Dim SQLstr As String = "select * from AlertHistory where DeviceName='" & DeviceName & "' and DeviceType ='" & DeviceType & "' and AlertType='" & AlertType & "'  and Location='" & Location & "' and DateTimeAlertCleared is null"

			'WriteDeviceHistoryEntry("All", "Alerts", NowTime & " SQLstr: " & SQLstr & "," & DeviceType & "/" & DeviceName & " " & AlertType)
			GetMaxAlertsQueuedToday(Sqlcon)
			Dim strSQL As String
			'12/17/2014 NS modified for VSPLUS-1267
			'Dim DA As New SqlDataAdapter(SQLstr, Sqlcon)
			'Dim Ds As New DataSet
			'DA.Fill(Ds, "AlertHistory")
			'Dim dt As DataTable = Ds.Tables(0)
			Dim dt As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), SQLstr)
			If dt.Rows.Count > 0 Then

				Try

					strSQL = "Update AlertHistory SET DateTimeAlertCleared= '" & NowTime & "'" & _
					 "  WHERE DeviceName='" & DeviceName & "' AND DeviceType='" & DeviceType & "' AND AlertType='" & AlertType & "' and Location='" & Location & "' and DateTimeAlertCleared is null"

					WriteDeviceHistoryEntry("All", "Alerts", NowTime & " AlertHistory update strSQL: " & strSQL & "," & DeviceType & "/" & DeviceName & " " & AlertType)
					'12/17/2014 NS modified for VSPLUS-1267
					'Dim sqlcmd As New SqlCommand(strSQL, Sqlcon)
					'Sqlcon.Open()
					'sqlcmd.ExecuteNonQuery()
					'Sqlcon.Close()
					myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)
				Catch ex As Exception
					WriteDeviceHistoryEntry("All", "Alerts", NowTime & " AlertHistory update error: " & ex.Message & "," & DeviceType & "/" & DeviceName & " " & AlertType)
					WriteAuditEntry(NowTime & " Alert History Update Error " & ex.Message)
				End Try

				'For Outages entry: Mukund 22Oct13
				If AlertType = "Not Responding" Then
					WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Outages update started: " & DeviceType & "/" & DeviceName & " " & AlertType)
					SQLstr = "select srv.ID,srv.ServerTypeId from Servers srv where srv.ServerName='" & DeviceName &
					  "' union select srv.ID,srv.ServerTypeId from URLs srv where srv.Name='" & DeviceName &
					  "' union select [key],(select ID from ServerTypes where servertype='Mail') ServerTypeId  from MailServices where Name='" & DeviceName &
					  "' union select id,(select ID from ServerTypes where servertype='Network Device') ServerTypeId  from [Network Devices] where Name='" & DeviceName & "'"
					'12/17/2014 NS modified for VSPLUS-1267
					'WriteDeviceHistoryEntry("All", "Alerts", NowTime & " SQLstr: " & SQLstr & "," & DeviceType & "/" & DeviceName & " " & AlertType)
					'Dim DA1 As New SqlDataAdapter(SQLstr, Sqlcon)
					'Dim Ds1 As New DataSet
					'DA1.Fill(Ds1, "servers")
					'Dim dt1 As DataTable = Ds1.Tables(0)
					Dim dt1 As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), SQLstr)
					If dt1.Rows.Count > 0 Then

						strSQL = "update Outages set DateTimeUp='" & NowTime & "' where ServerID='" & dt1.Rows(0)("ID").ToString & "' and ServerTypeID='" & dt1.Rows(0)("ServerTypeId").ToString & "' and DateTimeUp is null"
						WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Outages update: " & strSQL & "," & DeviceType & "/" & DeviceName & " " & AlertType)
						Try
							'12/17/2014 NS modified for VSPLUS-1267
							'Dim sqlcmd1 As New SqlCommand(strSQL, vssSqlcon)
							'vssSqlcon.Open()
							'sqlcmd1.ExecuteNonQuery()
							'vssSqlcon.Close()
							myAdapter.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", strSQL)
						Catch ex As Exception
							WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Outages Update Error " & ex.Message & "," & DeviceType & "/" & DeviceName & " " & AlertType)
							WriteAuditEntry(NowTime & " Outages Update Error " & ex.Message)
						End Try
					End If
				End If

			Else
				WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Alert for " & DeviceType & "/" & DeviceName & ": " & AlertType & " was marked as 'Cleared'")
			End If
			'Mukund 12Sep14,On Joe's suggestion, moved outside If condition above to get data in all conditions in StatusDetail
			'Mukund 14Jul14, VSPLUS-814 - StatusDetail insert/update sql

			'VSPLUS-930,Mukund, 15Sep14 pass Category, Details parameters
			If DeviceType = "Office365" Then
				UpdateStatusDetails(Location, DeviceName, AlertType, Details, Location, Category, "Pass", NowTime)
			Else
				UpdateStatusDetails(DeviceType, DeviceName, AlertType, Details, Location, Category, "Pass", NowTime)
			End If
		Catch ex As Exception

			WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error searching alerts at the time of Update: " & ex.Message)
			WriteAuditEntry(NowTime & " Error searching alerts: " & ex.Message)
		End Try

		GC.Collect()
	End Sub

	'VSPLUS-1303 22Jan15, Mukund/Swati To clear all data from  StatusDetail table based on 1 to 3 parameters
	Public Function ClearStatusDetails(ByVal DeviceName As String, Optional ByVal DeviceType As String = "", Optional ByVal AlertType As String = "") As String
		Dim iRetVal As Integer = 0
		Dim strSQL As String = ""
		Dim strSQL1 As String = ""
		Dim Sqlcon As New SqlConnection(connectionString)
		Dim myConnectionString As New VSFramework.XMLOperation
		Dim myAdapter As New VSFramework.VSAdaptor
		Dim objDateUtils As New DateUtils.DateUtils
		Dim strDateFormat As String

		If DeviceName = "" Then
			Return "Enter Values"
		Else

			strDateFormat = objDateUtils.GetDateFormat()

			If DeviceType <> "" And DeviceName <> "" And AlertType <> "" Then
				strSQL1 = "Delete  StatusDetail where TypeAndName='" & DeviceName & "-" & DeviceType & "' and TestName='" & AlertType & "'"
			ElseIf DeviceType <> "" And DeviceName <> "" Then
				strSQL1 = "Delete  StatusDetail where TypeAndName='" & DeviceName & "-" & DeviceType & "'"
			ElseIf DeviceName <> "" Then
				strSQL1 = "Delete  StatusDetail where TypeAndName like '" & DeviceName & "-%'"
			End If

			Dim dsStatusHTML As New Data.DataSet
			Dim Status As New Data.DataTable("StatusDetail")
			Dim objVSAdaptor As New VSAdaptor
			Try
				objVSAdaptor.FillDatasetAny("VitalSigns", "StatusDetail", strSQL1, dsStatusHTML, "StatusDetail")
			Catch ex As Exception
				WriteAuditEntry(Now.ToString & " Exception in  module: " & ex.Message)
			End Try
			Return ""
		End If
	End Function
	Public Sub DeleteAlert(ByVal DeviceType As String, ByVal DeviceName As String, ByVal Location As String, Optional ByVal AlertType As String = "")
		Dim myConnectionString As New VSFramework.XMLOperation
		Dim myAdapter As New VSFramework.VSAdaptor
		Dim objDateUtils As New DateUtils.DateUtils
		Dim strDateFormat As String
		strDateFormat = objDateUtils.GetDateFormat()
		Dim NowTime As String = objDateUtils.FixDateTime(Date.Now, strDateFormat)

		WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Received notice to delete alert for " & DeviceType & "/" & DeviceName & ": " & AlertType)
		Try
			Dim Sqlcon As New SqlConnection(connectionString)
			Dim vssSqlcon As New SqlConnection(VSSconnectionString)
			Dim SQLstr As String = "DELETE FROM AlertHistory WHERE DeviceName='" & DeviceName & "' AND DeviceType ='" & DeviceType & "' AND Location='" & Location & "' AND DateTimeAlertCleared IS NULL "
			If AlertType <> "" Then
				SQLstr += "AND AlertType='" & AlertType & "'"
			End If
			myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", SQLstr)
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error deleting alerts: " & ex.Message)
			WriteAuditEntry(NowTime & " Error deleting alerts: " & ex.Message)
		End Try

		GC.Collect()
	End Sub


#Region "Log Files"


	Private Sub WriteAuditEntry(ByVal strMsg As String, Optional ByVal logLevel As LogLevel = LogLevel.Normal)
		LogUtilities.LogUtils.WriteAuditEntry(strMsg, "Alertdll.txt", logLevel)
	End Sub

	Private Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String, Optional ByVal logLevel As LogLevel = LogLevel.Normal)
		LogUtilities.LogUtils.WriteDeviceHistoryEntry(DeviceType, DeviceName, strMsg, logLevel)
	End Sub

	'Private Sub WriteAuditEntry(ByVal strMsg As String)
	'strAuditText += strMsg & vbCrLf
	'End Sub


	'Private Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String)
	'	Dim DeviceLogDestination As String
	'	Dim appendMode As Boolean = True

	'	DeviceLogDestination = AppDomain.CurrentDomain.BaseDirectory.ToString & "\Log_Files\" & DeviceType & "_" & DeviceName & "_Log.txt"
	'	' DeviceLogDestination = "D:\Logfiles\" & DeviceType & "_" & DeviceName & "_Log.txt"

	'	Try
	'		Dim sw As New StreamWriter(DeviceLogDestination, appendMode, System.Text.Encoding.Unicode)
	'		sw.WriteLine(strMsg)
	'		sw.Close()
	'		sw = Nothing
	'	Catch ex As Exception

	'	End Try
	'	GC.Collect()
	'End Sub


#End Region

#End Region
#Region "System Messages"
	Public Sub QueueSysMessage(ByVal Details As String)
		Dim strSQL As String
		Dim dt As DataTable
		Dim Sqlcon As New SqlConnection(connectionString)
		Dim myConnectionString As New VSFramework.XMLOperation
		Dim myAdapter As New VSFramework.VSAdaptor
		Dim objDateUtils As New DateUtils.DateUtils
		Dim strDateFormat As String
		Dim NowTime As String
		Try

			strDateFormat = objDateUtils.GetDateFormat()
			NowTime = objDateUtils.FixDateTime(Date.Now, strDateFormat)

			WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " Queueing System Message - " & Details)
			Dim SQLstr As String = "SELECT * FROM SystemMessages WHERE Details='" & Details & "' AND DateCleared IS NULL"
			dt = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), SQLstr)
			If dt.Rows.Count = 0 Then
				If MyLogLevel = LogLevel.Verbose Then
					WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " This message is new, adding to collection")
				End If
				strSQL = "INSERT INTO SystemMessages(Details,DateCreated) VALUES('" & Details & "','" & NowTime & "')"
				Try
					myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)
					Dim SQLstr2 As String = "IF NOT EXISTS(SELECT * FROM UserSystemMessages WHERE SysMsgID IN (SELECT MAX(ID) FROM SystemMessages)) " &
					 "BEGIN " &
					 "INSERT INTO UserSystemMessages (SysMsgID,UserID) " &
					 "SELECT t1.ID SysMsgID,t2.ID UserID FROM SystemMessages t1, Users t2 " &
					 "WHERE t1.DateCleared IS NULL AND t1.ID=(SELECT MAX(ID) FROM SystemMessages) " &
					 "END"
					Try
						myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", SQLstr2)
					Catch ex As Exception
						WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " User System Messages Insert Error " & ex.Message)
						WriteAuditEntry(NowTime & " User System Messages Insert Error " & ex.Message)
					End Try
				Catch ex As Exception
					WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " System Messages Insert Error " & ex.Message)
					WriteAuditEntry(NowTime & " System Messages Insert Error " & ex.Message)
				End Try
			Else
				If MyLogLevel = LogLevel.Verbose Then
					WriteAuditEntry(NowTime & " This message has already been queued: " & Details)
					WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " This message has already been queued: " & Details)
				End If
			End If
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "SysMessages", Now.ToShortTimeString & " Error searching system messages: " & ex.Message)
			WriteAuditEntry(Now.ToShortTimeString & " Error searching system messages: " & ex.Message)
		End Try

		GC.Collect()
	End Sub

	Public Sub ResetSysMessage(ByVal Details As String)

		Dim myConnectionString As New VSFramework.XMLOperation
		Dim myAdapter As New VSFramework.VSAdaptor
		Dim objDateUtils As New DateUtils.DateUtils
		Dim strDateFormat As String
		Try

			strDateFormat = objDateUtils.GetDateFormat()
			Dim NowTime As String = objDateUtils.FixDateTime(Date.Now, strDateFormat)
			WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " Received notice to reset system message for " & Details)

			Dim Sqlcon As New SqlConnection(connectionString)
			Dim vssSqlcon As New SqlConnection(VSSconnectionString)
			Dim SQLstr As String = "SELECT * FROM SystemMessages WHERE Details='" & Details & "' AND DateCleared IS NULL"
			Dim strSQL As String
			'4/22/2015 NS modified the SQL query below
			Dim SQLstr2 As String = "DELETE  t1 FROM UserSystemMessages t1 INNER JOIN SystemMessages t2 ON t2.ID=t1.SysMsgID WHERE t2.Details='" & Details & "' "
			Dim dt As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), SQLstr)
			If dt.Rows.Count > 0 Then
				Try
					strSQL = "UPDATE SystemMessages SET DateCleared='" & NowTime & "' WHERE Details='" & Details & "' AND DateCleared IS NULL"
					WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " SystemMessages update strSQL: " & strSQL)
					myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)
					Try
						myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", SQLstr2)
					Catch ex As Exception
						WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " UserSystemMessages delete error: " & ex.Message & "," & Details)
						WriteAuditEntry(NowTime & " User System Messages Delete Error " & ex.Message)
					End Try
				Catch ex As Exception
					WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " SystemMessages update error: " & ex.Message & "," & Details)
					WriteAuditEntry(NowTime & " System Messages Update Error " & ex.Message)
				End Try
			Else
				WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " System Message for " & Details & " was marked as 'Cleared'")
			End If
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "SysMessages", Now.ToShortTimeString() & " Error searching system messages at the time of Update: " & ex.Message)
			WriteAuditEntry(Now.ToShortTimeString() & " Error searching system messages: " & ex.Message)
		End Try

		GC.Collect()
	End Sub

	Public Sub SysMessageForLicenses()
		Dim myConnectionString As New VSFramework.XMLOperation
		Dim myAdapter As New VSFramework.VSAdaptor
		'7/8/2015 NS modified verbiage for VSPLUS-1959
		Dim message As String = "There is an insufficient number of licenses for your servers."
		Try

			'Dim sql As String = "SELECT Count(*) FROM DeviceInventory WHERE CurrentNodeID=-1"
			Dim sql As String = "select COUNT(*) from DeviceInventory,Nodes  where CurrentNodeId =-1 and nodes.Alive =1"
			Dim dt As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sql)

			If (dt.Rows.Count > 0 And dt.Rows(0)(0).ToString() = "0") Then
				ResetSysMessage(message)
			Else
				QueueSysMessage(message)

			End If
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "SysMessages", Now.ToShortTimeString & " System Message for License: " & ex.Message)
			WriteAuditEntry(Now.ToShortTimeString & " System Message for License: " & ex.Message)
		End Try
	End Sub
#End Region




End Class
