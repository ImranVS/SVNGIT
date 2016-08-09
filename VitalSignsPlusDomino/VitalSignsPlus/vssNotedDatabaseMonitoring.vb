Imports System.Threading
Imports System.IO
Imports System.Globalization
Imports MonitoredItems.MonitoredDevice.Alert
Imports VSFramework
Imports System.Net.Sockets
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.Serialization.Json
Imports System.Runtime.Serialization
Imports MongoDB.Driver
Partial Public Class VitalSignsPlusDomino

#Region "Notes Database Monitoring"

	Private Sub MonitorNotesDatabases()	'This is the main sub that calls all the other ones
		Dim MyNotesDatabase As MonitoredItems.NotesDatabase
		WriteAuditEntry(Now.ToString & " Selecting a Notes database to monitor.", LogLevel.Verbose)
		MyNotesDatabase = SelectNotesDBToMonitor()
		If MyNotesDatabase Is Nothing Then
			Exit Sub
		End If

		Try
			Dim Server As MonitoredItems.DominoServer
			Server = MyDominoServers.Search(MyNotesDatabase.ServerName)
			If Server.IsBeingScanned = True Then
				WriteAuditEntry(Now.ToString & " This server is being scanned, Notes DB scanning is deferred.", LogLevel.Verbose)
				Exit Sub
			End If

		Catch ex As Exception

		End Try

		'If MyNotesDatabase.Scanning = True Then
		'    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " I am already scanning this database, so I'm not going to start another scan right now.")
		'    MyNotesDatabase = Nothing
		'    Exit Sub
		'End If

		Try
			MyNotesDatabase.Status = "Scanning"
			'  MyNotesDatabase.Scanning = True
			MyNotesDatabase.ResponseDetails = "New scan cycle started at " & Date.Now.ToShortTimeString
			If MyNotesDatabase.TriggerType = "Refresh All Views" Then
				MyNotesDatabase.Description = "New cycle to refresh all views started at " & Date.Now.ToShortTimeString
			End If
			UpdateStatusTableNotesDB(MyNotesDatabase, 0)
		Catch ex As Exception

		End Try

		Try
			MyNotesDatabase.LastScan = Now
			'     If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Selecting a Notes database to monitor.")
			MonitorNotesDatabase(MyNotesDatabase)
			'  MyNotesDatabase.Scanning = False
			MyNotesDatabase = Nothing
		Catch ex As Exception

		End Try


		GC.Collect()
	End Sub

	Private Function SelectNotesDBToMonitor() As MonitoredItems.NotesDatabase
		If MyNotesDatabases.Count = 0 Then
			Return Nothing
			Exit Function
		End If

		Dim tNow As DateTime
		tNow = Now
		Dim tScheduled As DateTime

		Dim timeOne, timeTwo As DateTime

		Dim myDevice As MonitoredItems.NotesDatabase
		Dim SelectedServer As MonitoredItems.NotesDatabase

		Dim ServerOne As MonitoredItems.NotesDatabase
		Dim ServerTwo As MonitoredItems.NotesDatabase

		Dim n As Integer
		Dim myRegistry As New RegistryHandler

		Dim strSQL As String = ""
		Dim ServerType As String = "NotesDatabase"
		Dim serverName As String = ""

		Try
			strSQL = "Select svalue from ScanSettings where sname = 'Scan" & ServerType & "ASAP'"
			Dim ds As New DataSet()
			ds.Tables.Add("ScanASAP")
			Dim objVSAdaptor As New VSAdaptor
			objVSAdaptor.FillDatasetAny("VitalSigns", "VitalSigns", strSQL, ds, "ScanASAP")

			For Each row As DataRow In ds.Tables("ScanASAP").Rows
				Try
					serverName = row(0).ToString()
				Catch ex As Exception
					Continue For
				End Try

				For n = 0 To MyNotesDatabases.Count - 1
					ServerOne = MyNotesDatabases.Item(n)

					If ServerOne.Name = serverName And ServerOne.IsBeingScanned = False And ServerOne.Enabled Then
						WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " >>> " & serverName & " was marked 'Scan ASAP' so that will be scanned next.")
						strSQL = "DELETE FROM ScanSettings where sname = 'Scan" & ServerType & "ASAP' and svalue='" & serverName & "'"
						objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)

						Return ServerOne
						Exit Function

					End If
				Next
			Next

		Catch ex As Exception

		End Try





		'Any server Not Scanned should be scanned right away.  Select the first one you encounter
		For n = 0 To MyNotesDatabases.Count - 1
			ServerOne = MyNotesDatabases.Item(n)
			If ServerOne.Status = "Not Scanned" Then
				'If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status)
				Return ServerOne
				Exit Function
			End If
		Next

		'start with the first two servers
		ServerOne = MyNotesDatabases.Item(0)
		If MyNotesDatabases.Count > 1 Then ServerTwo = MyNotesDatabases.Item(1)

		'go through the remaining servers, see which one has the oldest (earliest) scheduled time
		If MyNotesDatabases.Count > 2 Then
			Try
				For n = 2 To MyNotesDatabases.Count - 1
					'     WriteAuditEntry(Now.ToString & " N is " & n)
					timeOne = CDate(ServerOne.NextScan)
					timeTwo = CDate(ServerTwo.NextScan)
					If DateTime.Compare(timeOne, timeTwo) < 0 Then
						'time one is earlier than time two, so keep server 1
						ServerTwo = MyNotesDatabases.Item(n)
					Else
						'time two is later than time one, so keep server 2
						ServerOne = MyNotesDatabases.Item(n)
					End If
				Next
			Catch ex As Exception
				WriteAuditEntry(Now.ToString & " >>> Error selecting a Notes Database... " & ex.Message)
			End Try
		Else
			'There were only two dbs, so use those going forward
		End If


		'Of the two remaining devices, pick the one with earliest scheduled time for next scan
		If Not (ServerTwo Is Nothing) Then
			timeOne = CDate(ServerOne.NextScan)
			timeTwo = CDate(ServerTwo.NextScan)

			If DateTime.Compare(timeOne, timeTwo) < 0 Then
				'time one is earlier than time two, so keep server 1
				SelectedServer = ServerOne
				tScheduled = CDate(ServerOne.NextScan)
			Else
				SelectedServer = ServerTwo
				tScheduled = CDate(ServerTwo.NextScan)
			End If
			tNow = Now
			'     WriteAuditEntry(Now.ToString & " >>> Down to one Notes Database... " & SelectedServer.Name & " to scan at " & SelectedServer.NextScan & ". Status is " & SelectedServer.Status)
		Else
			SelectedServer = ServerOne
			tScheduled = CDate(ServerOne.NextScan)
		End If
		tScheduled = CDate(SelectedServer.NextScan)
		If DateTime.Compare(tNow, tScheduled) < 0 Then
			If SelectedServer.Status <> "Not Scanned" Then
				WriteAuditEntry(Now.ToString & " No Notes DB are scheduled for monitoring, next scan after " & SelectedServer.NextScan, LogLevel.Verbose)
				SelectedServer = Nothing
			Else
				WriteAuditEntry(Now.ToString & " selected Notes DB: " & SelectedServer.Name & " because it has not been scanned yet.", LogLevel.Verbose)
			End If
		Else
			WriteAuditEntry(Now.ToString & " selected Notes DB: " & SelectedServer.Name, LogLevel.Verbose)
		End If

		'Release Memory
		tNow = Nothing
		tScheduled = Nothing
		n = Nothing

		timeOne = Nothing
		timeTwo = Nothing

		myDevice = Nothing
		ServerOne = Nothing
		ServerTwo = Nothing


		Return SelectedServer
		Exit Function

	End Function

	Private Sub MonitorNotesDatabase(ByRef MyNotesDatabase As MonitoredItems.NotesDatabase)
		'this sub checks on Notes databases and compares against threshold of size, count, or performance
		'  Dim s As New Domino.NotesSession

		'  Dim s As New Domino.NotesSession
		'Try
		'    s.Initialize(MyDominoPassword)
		'Catch ex As Exception
		'    System.Runtime.InteropServices.Marshal.ReleaseComObject(s)
		'    Exit Sub
		'End Try


		Dim strResponse, StatusDetails As String
		Dim Percent As Double = 100
		Dim myResponseTime As Integer
		WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Begin Notes Database Scan for " & MyNotesDatabase.Name & ", which is triggered on: " & MyNotesDatabase.TriggerType)
		'WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Current Scan Interval: " & MyNotesDatabase.ScanInterval)
		'WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Next scheduled scan " & MyNotesDatabase.NextScan)


		'Domino Objects
		Dim db As Domino.NotesDatabase
		Dim doc As Domino.NotesDocument
		Dim docTarget As Domino.NotesDocument
		Dim view As Domino.NotesView
		Dim Collection As Domino.NotesDocumentCollection
		Dim myDate As Domino.IDateTime
		Dim v As Domino.NotesView

		Try
			If MyNotesDatabase.Enabled = False Then
				WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " " & MyNotesDatabase.Name & " is disabled.")
				With MyNotesDatabase
					.Status = "Disabled"
					.ResponseDetails = "Monitoring is disabled for this Notes Database."
					'.AlertCondition = False  - don't call this or status is set to OK
				End With
				' WriteAuditEntry(Now.ToString & " " & MyNotesDatabase.Name & " is " & MyNotesDatabase.Status & ", says Luke.")
				db = Nothing
				Exit Try
			End If

			'Don't monitor if the Domino server is in a maintenance window
			Dim bIsInMaintenance As Boolean = InMaintenance("Domino", MyNotesDatabase.ServerName)
			If bIsInMaintenance Then
				WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " " & MyNotesDatabase.Name & " is in maintenance.")
				With MyNotesDatabase
					.Status = "Maintenance"
					.ResponseDetails = "Monitoring is disabled for this Notes Database, because the server is in a maintenance window."
					Percent = 0
					.ResponseThreshold = 0
				End With
				' WriteAuditEntry(Now.ToString & " " & MyNotesDatabase.Name & " is " & MyNotesDatabase.Status & ", says Luke.")
				'db = Nothing
				Exit Try
			End If

			If MyNotesDatabase.Enabled = True And bIsInMaintenance = False Then
				Select Case MyNotesDatabase.TriggerType
					Case "Database Disappearance"
						Try
							db = NotesSession.GetDatabase(MyNotesDatabase.ServerName, MyNotesDatabase.FileName, False)
						Catch ex As Exception
							MyNotesDatabase.AlertCondition = True
							MyNotesDatabase.Status = "Not Found"
							MyNotesDatabase.ResponseDetails = ex.Message
							MyNotesDatabase.IncrementDownCount()
							'  MyNotesDatabase.LastScan = Now
                            myAlert.QueueAlert(MyNotesDatabase.ServerType, MyNotesDatabase.Name, "Database Disappearance", MyNotesDatabase.ResponseDetails, MyNotesDatabase.Location)
						End Try

						If db Is Nothing Then
							MyNotesDatabase.AlertCondition = True
							MyNotesDatabase.Status = "Not Found"
							MyNotesDatabase.ResponseDetails = MyNotesDatabase.FileName & " is not found on " & MyNotesDatabase.ServerName & "."
							MyNotesDatabase.IncrementDownCount()
							'   MyNotesDatabase.LastScan = Now
                            myAlert.QueueAlert(MyNotesDatabase.ServerType, MyNotesDatabase.Name, "Database Disappearance", MyNotesDatabase.ResponseDetails, MyNotesDatabase.Location)
						Else
							MyNotesDatabase.AlertCondition = False
							MyNotesDatabase.Status = "OK"
							MyNotesDatabase.ResponseDetails = MyNotesDatabase.FileName & " exists on " & MyNotesDatabase.ServerName & "."
							MyNotesDatabase.IncrementUpCount()

                            myAlert.ResetAlert(MyNotesDatabase.ServerType, MyNotesDatabase.Name, "Database Disappearance", MyNotesDatabase.Location)
						End If

					Case "Replication"
						WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, vbCrLf & Now.ToString & " " & MyNotesDatabase.Name & " is going to be monitored for replication.")
						' Dim span As System.TimeSpan
						MyNotesDatabase.Status = "OK"
						Dim DestinationServer As MonitoredItems.DominoServer
						db = NotesSession.GetDatabase(MyNotesDatabase.ServerName, MyNotesDatabase.FileName, False)

						If db Is Nothing Then
							MyNotesDatabase.AlertCondition = True
							MyNotesDatabase.Status = "Failed"
							MyNotesDatabase.ResponseDetails = "Source database is not responding, so replication cannot be tested."
							MyNotesDatabase.IncrementDownCount()
							'   MyNotesDatabase.LastScan = Now
							Exit Try
						End If
						MyNotesDatabase.ResponseDetails = ""
						If MyNotesDatabase.DocumentID <> "" Then
							WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Searching for document " & MyNotesDatabase.DocumentID)							   'A replication cycle has already been sent, so see if it worked.
							MyNotesDatabase.Description = "Verifies that the database replicates with "
							For Each DestinationServer In MyNotesDatabase.ReplicationServers
								MyNotesDatabase.Description += DestinationServer.Name & "  "
							Next

							For Each DestinationServer In MyNotesDatabase.ReplicationServers
								WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Searching for database on " & DestinationServer.Name)
								'A replication cycle has already been sent, so see if it worked.

								Try
									db = NotesSession.GetDatabase(DestinationServer.Name, MyNotesDatabase.FileName, False)
								Catch ex As Exception
									MyNotesDatabase.Status = "Database not found on " & DestinationServer.Name
								End Try

								If db Is Nothing Then
									MyNotesDatabase.Status = "Failed"
									MyNotesDatabase.ResponseDetails = "Database not found on " & DestinationServer.Name
									Exit For
								Else
									Try
										If String.IsNullOrWhiteSpace(db.Title) Then
											db.Title = "Untitled Database"
										End If
									Catch ex As Exception
                                        WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error checking if db name is blank.  Error: " & ex.Message)
									End Try
									WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Found the target database " & db.Title & " on " & DestinationServer.Name)
								End If

								Try
									WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Attempting to locate document " & MyNotesDatabase.DocumentID & " on " & db.Title & " on " & DestinationServer.Name)
									docTarget = db.GetDocumentByUNID(MyNotesDatabase.DocumentID)
								Catch ex As Exception
									WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error searching for document " & ex.ToString)
								End Try

								If docTarget Is Nothing Then
									WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Replication failed with " & DestinationServer.Name)
									' MyNotesDatabase.Status = "Failed"
									MyNotesDatabase.ResponseDetails += "Replication failed with " & DestinationServer.Name & vbCrLf
								Else
									WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Replication succeeded with " & DestinationServer.Name)
									'MyNotesDatabase.Status = "OK"
									MyNotesDatabase.ResponseDetails += "Replication succeeded with " & DestinationServer.Name & vbCrLf
									' docTarget.Remove(True)
								End If
							Next
						End If

						If InStr(MyNotesDatabase.ResponseDetails, "failed") > 0 Or MyNotesDatabase.Status = "Failed" Then
							MyNotesDatabase.Status = "Failed"
						Else
							MyNotesDatabase.Status = "OK"
						End If

						'Start a new cycle 
						'First delete the document used in the previous cycle
						Try
							If MyNotesDatabase.DocumentID <> "" Then
								db = NotesSession.GetDatabase(MyNotesDatabase.ServerName, MyNotesDatabase.FileName, False)
								docTarget = db.GetDocumentByUNID(MyNotesDatabase.DocumentID)
								If Not docTarget Is Nothing Then
									docTarget.Remove(True)
									WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Located the previous target document in the source and deleted it.")
								End If
							End If

						Catch ex As Exception
							WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error deleting prior document: " & ex.ToString)
						End Try

						Try
							'If there are some documents left over from a past scan that weren't cleaned up
							If db.IsFTIndexed Then
								Collection = db.FTSearch("Select Form = '" & ProductName & "' & CreatedBy='" & ProductName & "'", 10)
								WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Found " & Collection.Count & " documents left behind from previous replication scans to clean up.")
								If Collection.Count > 0 Then
									Collection.RemoveAll(True)
								End If
							Else
								WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " INFORMATION: " & ProductName & " cannot search the database for prior test documents to clean up because it is not full-text indexed.")
								WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " INFORMATION: Some test documents may remain in the database if the service is terminated in the middle of a test cycle.")
							End If

						Catch ex As Exception
							WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error searching for prior test documents to remove: " & ex.ToString)

						End Try

						Try
							doc = db.CreateDocument
							doc.AppendItemValue("Form", ProductName)
							doc.AppendItemValue("Subject", "Replication Test -- it is OK to delete this document")
							doc.AppendItemValue("From", ProductName)
							doc.AppendItemValue("CreatedBy", ProductName)
							doc.Save(True, False)
							MyNotesDatabase.DocumentID = doc.UniversalID
							WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " DocID of new target document is " & MyNotesDatabase.DocumentID)
							doc = db.GetDocumentByUNID(MyNotesDatabase.DocumentID)
							If Not doc Is Nothing Then
								WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Located the targed document in the source using GetDocumentByUNID.")
							End If
						Catch ex As Exception

						End Try

						'Initiate replication, if so directed
						If MyNotesDatabase.InitiateReplication = True Then
							Try
								For Each DestinationServer In MyNotesDatabase.ReplicationServers
									WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Sending command 'Pull " & MyNotesDatabase.ServerName & " names.nsf to " & DestinationServer.Name)
									SendConsoleCommand(MyNotesDatabase.ServerName, "Pull " & MyNotesDatabase.ServerName & " names.nsf")
								Next
							Catch ex As Exception

							End Try
						End If

						'MyNotesDatabase.Status = "Sent"
					Case "Document Count"
						Try
							Dim span As System.TimeSpan
							db = NotesSession.GetDatabase(MyNotesDatabase.ServerName, MyNotesDatabase.FileName, False)
							If db Is Nothing Then
								MyNotesDatabase.AlertCondition = True
								MyNotesDatabase.AlertType = NotResponding
								MyNotesDatabase.Status = "Not Responding"
								MyNotesDatabase.ResponseDetails = MyNotesDatabase.ServerName & " is not responding."
								MyNotesDatabase.IncrementDownCount()

								Exit Try
							End If
							MyNotesDatabase.IncrementUpCount()
							MyNotesDatabase.DocumentCount = db.AllDocuments.Count
							If MyNotesDatabase.DocumentCount > MyNotesDatabase.DocumentCountTrigger Then
								With MyNotesDatabase
									.AlertCondition = True
									.Status = "Too Many Documents"
									.ResponseDetails = "Notes database has  " & MyNotesDatabase.DocumentCount & " documents. "
                                    myAlert.QueueAlert(MyNotesDatabase.ServerType, MyNotesDatabase.Name, "Too Many Documents", "The Notes database " & MyNotesDatabase.Name & " on " & MyNotesDatabase.ServerName & " (" & MyNotesDatabase.FileName & ") has  " & MyNotesDatabase.DocumentCount & " documents. Alert threshold is " & MyNotesDatabase.DocumentCountTrigger, MyNotesDatabase.Location)
								End With
							Else
								With MyNotesDatabase
									.AlertCondition = False
									.Status = "OK"
                                    myAlert.ResetAlert(MyNotesDatabase.ServerType, MyNotesDatabase.Name, "Too Many Documents", MyNotesDatabase.Location)
									.ResponseDetails = "Notes database has  " & MyNotesDatabase.DocumentCount & " documents. "
								End With
							End If
						Catch ex As Exception
							WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error checking Notes Database count  " & ex.Message)
						Finally
							'   WriteAuditEntry(Now.ToString & " Domino count  has finished.")
						End Try

						Try
							MyNotesDatabase.ResponseTime = MyNotesDatabase.DocumentCount
							MyNotesDatabase.ResponseThreshold = MyNotesDatabase.DocumentCountTrigger
						Catch ex As Exception
							WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error setting NDB doc count values " & ex.Message)

						End Try


						Try
							Percent = MyNotesDatabase.DocumentCount / MyNotesDatabase.DocumentCountTrigger * 100
							If MyNotesDatabase.AlertType = NotResponding Then
								Percent = 0
							End If
						Catch ex As Exception
							Percent = 0
						End Try

					Case "Database Size"
						Try
							Dim span As System.TimeSpan
							db = NotesSession.GetDatabase(MyNotesDatabase.ServerName, MyNotesDatabase.FileName, False)
							If db Is Nothing Then
								MyNotesDatabase.AlertCondition = True
								MyNotesDatabase.AlertType = NotResponding
								MyNotesDatabase.Status = "Not Responding"
								MyNotesDatabase.ResponseDetails = MyNotesDatabase.ServerName & " is not responding."
								MyNotesDatabase.IncrementDownCount()
								' MyNotesDatabase.LastScan = Now
								Exit Try
							End If
							MyNotesDatabase.IncrementUpCount()
							MyNotesDatabase.DatabaseSize = db.Size	'Returns the size in bytes, trigger is specified in MB

							WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Database size is " & db.Size)
							WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Database size trigger is " & MyNotesDatabase.DatabaseSizeTrigger * 1024 * 1024)

							If db.Size > (MyNotesDatabase.DatabaseSizeTrigger * 1024 * 1024) Then
								WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " " & db.Size & " is greater than " & MyNotesDatabase.DatabaseSizeTrigger * 1024 * 1024)
								With MyNotesDatabase
									.AlertCondition = True
									.Status = "Over Size"
									.ResponseDetails = "Notes database is " & (.DatabaseSize / 1024 / 1024) & " MB"
                                    myAlert.QueueAlert(MyNotesDatabase.ServerType, MyNotesDatabase.Name, "Database Size", "The Notes database " & MyNotesDatabase.Name & " on " & MyNotesDatabase.ServerName & " (" & MyNotesDatabase.FileName & ") has has exceed the size theshold. The Notes database is " & (.DatabaseSize / 1024 / 1024) & " MB", MyNotesDatabase.Location)

								End With
							Else
								With MyNotesDatabase
									.AlertCondition = False
									.Status = "OK"
                                    myAlert.ResetAlert(MyNotesDatabase.ServerType, MyNotesDatabase.Name, "Database Size", MyNotesDatabase.Location)
									.ResponseDetails = "Notes database is " & (.DatabaseSize / 1024 / 1024) & " MB"
								End With
							End If
						Catch ex As Exception
							WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error checking Notes Database Size " & ex.Message)
						Finally
							db = Nothing
							'   WriteAuditEntry(Now.ToString & " Notes Database size checking has finished.")
						End Try

						Try
							MyNotesDatabase.ResponseTime = MyNotesDatabase.DatabaseSize / 1024 / 1024
							MyNotesDatabase.ResponseThreshold = MyNotesDatabase.DatabaseSizeTrigger
						Catch ex As Exception
							WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error setting NDB size values " & ex.Message)
						End Try

						Try

							Percent = (MyNotesDatabase.DatabaseSize / (MyNotesDatabase.DatabaseSizeTrigger * 1024 * 1024)) * 100
							If MyNotesDatabase.AlertType = NotResponding Then
								Percent = 0
							End If
						Catch ex As Exception
							Percent = 0
						End Try


					Case "Database Response Time"
						Dim start, done, hits As Long
						Dim elapsed As TimeSpan
						Dim span As System.TimeSpan
						MyNotesDatabase.PreviousKeyValue = MyNotesDatabase.ResponseTime
						start = Now.Ticks

						Try
							Try
								db = NotesSession.GetDatabase(MyNotesDatabase.ServerName, MyNotesDatabase.FileName, False)

								If db Is Nothing Then
									MyNotesDatabase.AlertCondition = True
									MyNotesDatabase.AlertType = NotResponding
									MyNotesDatabase.Status = "Not Responding"
									MyNotesDatabase.ResponseDetails = MyNotesDatabase.ServerName & " is not responding."
									MyNotesDatabase.IncrementDownCount()
									' Exit Try
								Else
									Try
										If String.IsNullOrWhiteSpace(db.Title) Then
											db.Title = "Untitled Database"
										End If
									Catch ex As Exception
                                        WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error checking if db name is blank.  Error: " & ex.Message)
									End Try
									WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " database is " & db.Title)
									MyNotesDatabase.IncrementUpCount()
								End If
							Catch ex As Exception
								WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Exception connecting to database: " & ex.ToString)
								MyNotesDatabase.AlertCondition = True
								MyNotesDatabase.AlertType = NotResponding
								MyNotesDatabase.Status = "Not Responding"
								MyNotesDatabase.ResponseDetails = MyNotesDatabase.ServerName & " is not responding."
								MyNotesDatabase.IncrementDownCount()
								'   MyNotesDatabase.LastScan = Now
							End Try

							WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " about to loop through views in  " & db.Title)
							'loop through the views until you find the default view

							If Not db Is Nothing Then
								Try
									For Each v In db.Views
										'    WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " View name is: " & v.Name)
										If v.IsDefaultView Then
											WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " " & v.Name & " is the default view.  ")
											view = v
											Exit For
										End If
									Next

								Catch ex As Exception
									WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Exception looping through views: " & ex.ToString)
									view = Nothing
								End Try


								Try
									If view Is Nothing Then
										Dim dc As Domino.NotesDocumentCollection
										dc = db.AllDocuments
										WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " No default view so counting documents instead.  This db has " & dc.Count & " documents.")
										System.Runtime.InteropServices.Marshal.ReleaseComObject(dc)
										Percent = 0
										'Exit Try
									Else
										view.GetFirstDocument()
									End If
								Catch ex As Exception
									WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error attempting to refresh view. " & ex.ToString)
								End Try

								Try
									done = Now.Ticks
									elapsed = New TimeSpan(done - start)
									MyNotesDatabase.ResponseTime = elapsed.TotalMilliseconds
									WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " My response time was " & MyNotesDatabase.ResponseTime.ToString)

									If elapsed.TotalMilliseconds = 0 Then
										MyNotesDatabase.ResponseTime = 1
									End If
								Catch ex As Exception
									MyNotesDatabase.ResponseTime = 1
								End Try

								If MyNotesDatabase.ResponseTime < MyNotesDatabase.ResponseThreshold Then
									MyNotesDatabase.Status = "OK"
                                    myAlert.ResetAlert(MyNotesDatabase.ServerType, MyNotesDatabase.Name, "Slow", MyNotesDatabase.Location)
									MyNotesDatabase.AlertCondition = False
									MyNotesDatabase.IncrementUpCount()

									MyNotesDatabase.ResponseDetails = "Notes database responded in " & MyNotesDatabase.ResponseTime & " ms, and goal is " & MyNotesDatabase.ResponseThreshold
                                    myAlert.ResetAlert(MyNotesDatabase.ServerType, MyNotesDatabase.Name, "Response Time", MyNotesDatabase.Location)
								Else
									MyNotesDatabase.Status = "Slow"
									MyNotesDatabase.ResponseDetails = "Notes database responded in " & MyNotesDatabase.ResponseTime & " ms, but goal is " & MyNotesDatabase.ResponseThreshold
                                    myAlert.QueueAlert(MyNotesDatabase.ServerType, MyNotesDatabase.Name, "Response Time", "The Notes database " & MyNotesDatabase.Name & " on " & MyNotesDatabase.ServerName & " (" & MyNotesDatabase.FileName & ") is slow.  " & MyNotesDatabase.ResponseDetails, MyNotesDatabase.Location)
									MyNotesDatabase.IncrementUpCount()
								End If
							End If

						Catch ex As Exception
							'Server not responding
							WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Notes Database response time module error: " & ex.Message)

						Finally

							' db = Nothing
							'  WriteAuditEntry(Now.ToString & " Database Response Time has finished.")
						End Try
                        UpdateNDBStatisticsTable(MyNotesDatabase.ServerObjectID, MyNotesDatabase.ResponseTime)

					Case "Refresh All Views"
						Dim start, done, hits As Long
						Dim elapsed As TimeSpan
						Dim span As System.TimeSpan
						MyNotesDatabase.PreviousKeyValue = MyNotesDatabase.ResponseTime
						start = Now.Ticks

						Try
							db = NotesSession.GetDatabase(MyNotesDatabase.ServerName, MyNotesDatabase.FileName, False)

							If db Is Nothing Then
								MyNotesDatabase.AlertCondition = True
								MyNotesDatabase.AlertType = NotResponding
								MyNotesDatabase.Status = "Not Responding"
								MyNotesDatabase.ResponseDetails = MyNotesDatabase.ServerName & " is not responding."
								MyNotesDatabase.IncrementDownCount()
								' Exit Try
							Else
								Try
									If String.IsNullOrWhiteSpace(db.Title) Then
										db.Title = "Untitled Database"
									End If
								Catch ex As Exception
                                    WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error checking if db name is blank.  Error: " & ex.Message)
								End Try
								WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " database is " & db.Title)
								MyNotesDatabase.IncrementUpCount()
							End If
						Catch ex As Exception
							WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Exception connecting to database: " & ex.ToString)
							MyNotesDatabase.AlertCondition = True
							MyNotesDatabase.AlertType = NotResponding
							MyNotesDatabase.Status = "Not Responding"
							MyNotesDatabase.ResponseDetails = MyNotesDatabase.ServerName & " is not responding."
							MyNotesDatabase.IncrementDownCount()
							'   MyNotesDatabase.LastScan = Now
						End Try

						WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " about to loop through views in  " & db.Title)
						'loop through the views until you find the default view
						MyNotesDatabase.Status = "OK"
						Dim myViewCount As Integer = 0
						If Not db Is Nothing Then
							Try
								For Each v In db.Views
									Try
										myViewCount += 1
										WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Refreshing view: " & v.Name)
										v.Refresh()
                                        myAlert.ResetAlert(MyNotesDatabase.ServerType, MyNotesDatabase.Name, "View " & v.Name, MyNotesDatabase.Location)

										'    WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " View name is: " & v.Name)
										If v.IsDefaultView Then
											WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " " & v.Name & " is the default view.  ")
											view = v
											'Exit For
										End If
									Catch ex As Exception
										WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Exception looping through views: " & ex.ToString)
										'  MyNotesDatabase.Status = "Error"
										'    MyNotesDatabase.ResponseDetails = ex.ToString
										'   MyNotesDatabase.Description = "Error refreshing " & v.Name & " " & ex.ToString
										'   myAlert.QueueAlert("Notes Database", MyNotesDatabase.Name, "View " & v.Name, "Error refreshing " & v.Name & " " & ex.ToString)

									End Try

								Next

								MyNotesDatabase.Description = "Refreshed " & myViewCount.ToString & " views at " & Date.Now.ToShortTimeString

							Catch ex As Exception
								WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Exception looping through views: " & ex.ToString)
								view = Nothing
								MyNotesDatabase.Status = "Error"
								MyNotesDatabase.ResponseDetails = ex.ToString
								MyNotesDatabase.Description = "Error refreshing " & v.Name & " " & ex.ToString
							End Try

						End If


						Try
							done = Now.Ticks
							elapsed = New TimeSpan(done - start)
							MyNotesDatabase.ResponseTime = elapsed.TotalMilliseconds
							WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Refreshing all views took " & elapsed.TotalSeconds.ToString & " seconds.")

							If elapsed.TotalMilliseconds = 0 Then
								MyNotesDatabase.ResponseTime = 1
							End If
							MyNotesDatabase.Description = "Refreshed " & myViewCount.ToString & " views in " & elapsed.TotalSeconds.ToString("F1") & " seconds at " & Date.Now.ToShortTimeString  '& " on " & Date.Now.ToShortDateString
							MyNotesDatabase.ResponseDetails = "Refreshed " & myViewCount.ToString & " views in " & elapsed.TotalSeconds.ToString("F1") & " seconds. "
							MyNotesDatabase.ResponseThreshold = MyNotesDatabase.ResponseTime * 100
						Catch ex As Exception
							MyNotesDatabase.ResponseTime = 1
						End Try
                        UpdateNDBStatisticsTable(MyNotesDatabase.ServerObjectID, MyNotesDatabase.ResponseTime)
				End Select
			End If



		Catch ex As Exception
			WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error monitoring Notes Database " & ex.Message)
		End Try


		Try
			Percent = MyNotesDatabase.ResponseTime / MyNotesDatabase.ResponseThreshold * 100
			If MyNotesDatabase.AlertType = NotResponding Then
				Percent = 0
			End If
		Catch ex As Exception
			Percent = 0
		End Try
		If Percent < 1 Then
			Percent = 1	 ' 1%
		End If

		If InMaintenance("Domino", MyNotesDatabase.ServerName) = True Then
			Percent = 0
		End If

		Try
			UpdateStatusTableNotesDB(MyNotesDatabase, Percent)
		Catch ex As Exception

		End Try


		'Release Domino Objects
		Try
			System.Runtime.InteropServices.Marshal.ReleaseComObject(doc)
		Catch ex As Exception

		End Try
		Try
			System.Runtime.InteropServices.Marshal.ReleaseComObject(docTarget)
		Catch ex As Exception

		End Try
		Try
			System.Runtime.InteropServices.Marshal.ReleaseComObject(view)
		Catch ex As Exception

		End Try

		Try
			System.Runtime.InteropServices.Marshal.ReleaseComObject(v)
		Catch ex As Exception

		End Try

		Try
			System.Runtime.InteropServices.Marshal.ReleaseComObject(Collection)
		Catch ex As Exception

		End Try
		Try
			System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
		Catch ex As Exception

		End Try



		Try
			GC.Collect()
		Catch ex As Exception

		End Try

	End Sub

    Private Sub UpdateNDBStatisticsTable(ByVal DeviceId As String, ByVal ResponseTime As Long)
        WriteAuditEntry(Now.ToString & " Updating Notes Database statistics table")
        'COMMENTED BY MUKUND 28Feb12
        'Dim myConnection As New Data.OleDb.OleDbConnection
        'myConnection.ConnectionString = Me.OleDbConnectionStatistics.ConnectionString
        '' WriteAuditEntry(Now.ToString & " myConnection string is " & myConnection.ConnectionString)

        'Do While myConnection.State <> ConnectionState.Open
        '    myConnection.Open()
        'Loop

        ''   WriteAuditEntry(Now.ToString & " myConnection state is " & myConnection.State.ToString)
        'Dim myCommand As New OleDb.OleDbCommand
        'myCommand.Connection = myConnection

        'WRITTEN BY MUKUND 28Feb12

        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
            Dim entity As New VSNext.Mongo.Entities.DailyStatistics() With {
                .DeviceId = DeviceId,
                .StatName = "ResponseTime",
                .StatValue = ResponseTime}

            repository.Insert(entity)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Notes Database Stats table insert failed becase: " & ex.Message)
        Finally
            'myConnection.Close()
            'myConnection.Dispose()
            'myCommand.Dispose()
        End Try

        GC.Collect()

    End Sub

	Private Sub UpdateStatusTableNotesDB(ByRef MyNotesDatabase As MonitoredItems.NotesDatabase, ByVal Percent As Double)
		Dim strSQL As String

		'Update the status table
		If MyNotesDatabase.Enabled = False Then
			Percent = 0
			MyNotesDatabase.ResponseThreshold = 0
		End If

		If MyNotesDatabase.AlertType = NotResponding Or MyNotesDatabase.TriggerType = "Database Disappearance" Then
			Percent = 0
		End If

		Dim PercentageChange As Double
		Try
			If MyNotesDatabase.PreviousKeyValue > 0 And MyNotesDatabase.ResponseTime > 0 Then
				PercentageChange = -(1 - MyNotesDatabase.PreviousKeyValue / MyNotesDatabase.ResponseTime)
			Else
				PercentageChange = 0
			End If

			If MyNotesDatabase.TriggerType <> "Database Response Time" Then
				PercentageChange = 0
			End If

		Catch ex As Exception
			PercentageChange = 0
		End Try

		If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Previous response time was " & MyNotesDatabase.PreviousKeyValue & " Current value is " & MyNotesDatabase.ResponseTime & "; so percent change is: " & PercentageChange)

		With MyNotesDatabase
            .StatusCode = ServerStatusCode(.Status)
            '5/5/2016 NS modified
            'Changed -NDB to -Notes Database

            Dim MyNotesDatabase2 = MyNotesDatabase

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.TypeAndName = MyNotesDatabase2.Name & "-" & MyNotesDatabase2.ServerType)
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                 .Set(Function(x) x.DownCount, .DownCount) _
                                                                                 .Set(Function(x) x.CurrentStatus, .Status) _
                                                                                 .Set(Function(x) x.UpCount, .UpCount) _
                                                                                 .Set(Function(x) x.UpPercent, .UpPercentCount) _
                                                                                 .Set(Function(x) x.Details, .ResponseDetails) _
                                                                                 .Set(Function(x) x.LastUpdated, GetFixedDateTime(Now)) _
                                                                                 .Set(Function(x) x.StatusCode, .StatusCode) _
                                                                                 .Set(Function(x) x.SoftwareVersion, .ServerName) _
                                                                                 .Set(Function(x) x.DominoServerTasksStatus, .FileName) _
                                                                                 .Set(Function(x) x.ResponseTime, Convert.ToInt32(.ResponseTime)) _
                                                                                 .Set(Function(x) x.NextScan, .NextScan) _
                                                                                 .Set(Function(x) x.ResponseThreshold, Convert.ToInt32(.ResponseThreshold)) _
                                                                                 .Set(Function(x) x.Description, .Description) _
                                                                                 .Set(Function(x) x.MyPercent, Percent) _
                                                                                 .Set(Function(x) x.Name, .Name) _
                                                                                 .Set(Function(x) x.Location, .Location)

            repository.Update(filterDef, updateDef)

		End With

		' WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & "*****  Updating NotesDatabase with " & strSQL)
		'Save it to the Access database

		'WRITTEN BY MUKUND 28Feb12
		Dim objVSAdaptor As New VSAdaptor
		objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
		'COMMENTED BY MUKUND 28Feb12

		'Using SQL Server
		'If boolUseSQLServer = True Then
		'    Try
		'        WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Updating the Status Table on the SQL Server")
		'        Dim myCommand As New Data.SqlClient.SqlCommand
		'        myCommand.Connection = SqlConnectionVitalSigns
		'        myCommand.CommandText = strSQL
		'        myCommand.ExecuteNonQuery()
		'        myCommand.Dispose()
		'    Catch ex As Exception
		'        WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error updating Notes DB status table in SQL Server: " & ex.ToString)
		'    End Try

		'Else
		'    Dim myCommand As New OleDb.OleDbCommand
		'    Dim myConnection As New OleDb.OleDbConnection
		'    Try
		'        With myConnection
		'            .ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & myPath
		'            .Open()
		'        End With

		'        Do Until myConnection.State = ConnectionState.Open
		'            myConnection.Open()
		'        Loop

		'    Catch ex As Exception
		'        WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error: exception connecting to  Status table with Notes Database info: " & ex.Message)
		'    End Try

		'    '***


		'    Try
		'        If myConnection.State = ConnectionState.Open Then
		'            myCommand.CommandText = strSQL
		'            myCommand.Connection = myConnection
		'            myCommand.ExecuteNonQuery()
		'        End If
		'    Catch ex As Exception
		'        WriteDeviceHistoryEntry("Notes_Database", MyNotesDatabase.Name, Now.ToString & " Error updating Status table with Notes Database info: " & ex.Message & vbCrLf & strSQL)
		'    Finally
		'        strSQL = Nothing
		'        myConnection.Close()
		'        myCommand.Dispose()
		'        myConnection.Dispose()
		'        '  MyNotesDatabase.LastScan = Now.ToString
		'        ' WriteAuditEntry(Now.ToString & " Next scan for " & MyNotesDatabase.Name & " scheduled : " & MyNotesDatabase.NextScan)
		'        ' Me.OleDbConnectionStatus.Close()
		'    End Try

		'End If

	End Sub
#End Region

End Class
