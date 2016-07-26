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

#Region "Threads"

	'Protected Sub MonitorAllThingsDomino()
	'    'Dim elapsed As TimeSpan

	'    Dim FiveMinutes As Integer = 60 * 5 * -1  'seconds
	'    Dim ThreeMinutes As Integer = 60 * 3 * -1 'seconds
	'    Dim TwoMinutes As Integer = 60 * 2 * -1 'two minutes of seconds


	'    Do

	'        Dim threadMonitorDomino As New Thread(AddressOf MonitorDominoRelated)
	'        '   Dim threadMonitorDomino2 As New Thread(AddressOf MonitorDominoLoop)
	'        WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " Creating a new Lotus Domino Server monitoring thread, location #1.")

	'        Try
	'            WriteAuditEntry(Now.ToString & " Creating a new Lotus Domino Server monitoring thread.")
	'            threadMonitorDomino.Start()
	'            Try
	'                Thread.Sleep(2000) 'sleep 2 seconds to give thread a chance to start
	'            Catch ex As Exception

	'            End Try
	'            Dim myLastUpdate As TimeSpan
	'            Do
	'                Try
	'                    Try
	'                        myLastUpdate = dtDominoLastUpdate.Subtract(Now)
	'                        '   WriteAuditEntry(Now.ToString & " Domino thread last updated " & -myLastUpdate.TotalSeconds & " seconds ago.")
	'                    Catch ex2 As Exception
	'                        WriteAuditEntry(Now.ToString & " Domino thread startup error: " & ex2.Message)
	'                    End Try

	'                    Try
	'                        If myLastUpdate.TotalSeconds < FiveMinutes + TwoMinutes Then  'use < because it is negative 5 minutes
	'                            'The Domino Thread hasn't looped in 2 minutes seconds
	'                            WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " Destroying Lotus Domino Server thread because it hasn't updated in " & myLastUpdate.TotalSeconds & " seconds. ")
	'                            threadMonitorDomino.Abort()
	'                            threadMonitorDomino.Join(2500)
	'                            dtDominoLastUpdate = Now
	'                            threadMonitorDomino = Nothing
	'                            Exit Do
	'                        End If
	'                    Catch ex3 As Exception
	'                        WriteAuditEntry(Now.ToString & " Domino thread timer error: " & ex3.Message)
	'                    End Try

	'                Catch ex As ThreadAbortException
	'                    WriteAuditEntry(Now.ToString & " Destroying Lotus Domino Server thread to shut down the service.")
	'                    threadMonitorDomino.Abort()
	'                    threadMonitorDomino.Join(5000)
	'                    threadMonitorDomino = Nothing
	'                    Exit Sub
	'                Catch ex As Exception

	'                End Try

	'                Try
	'                    Thread.Sleep(15000) 'sleep 15 seconds
	'                Catch ex As Exception

	'                End Try


	'                Try
	'                    UpdateUpTimeAllDevices()
	'                Catch ex As Exception

	'                End Try

	'            Loop

	'        Catch ex As ThreadAbortException
	'            WriteAuditEntry(Now.ToString & " Destroyed Lotus Domino Server thread to shut down the service.")
	'            threadMonitorDomino.Abort()
	'            threadMonitorDomino.Join(5000)
	'            threadMonitorDomino = Nothing
	'            Exit Sub

	'        Catch ex As Exception
	'            WriteAuditEntry(Now.ToString & " Error starting Domino Thread: " & ex.Message)
	'        End Try
	'    Loop


	'End Sub

	Protected Sub CheckForDominoTableChanges()

		Dim myRegistry As New VSFramework.RegistryHandler
		Dim flags As New MonitoredItems.ServicesFlags()
		Dim NodeName As String = ""
		'********** Refresh Collection of Devices and Monitor settings

		Do Until boolTimeToStop = True
			Try
				'Update the settings anytime the registry indicates a change has been made
				If (NodeName = "") Then
					If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then
						NodeName = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
					End If
				End If
				If flags.UpdateServiceCollection(MonitoredItems.ServerTypes.Domino, NodeName) Then
					Try
						'' MonitorDominoSettings()
						WriteAuditEntry(Now.ToString & " Refreshing configuration of Domino servers")
						CreateDominoServersCollection()
						WriteAuditEntry(Now.ToString & " Refreshing Status Table for Domino")
                        UpdateStatusTableWithDomino()
                        StartMonitoringThreads()
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " Error Refreshing Domino server settings on demand: " & ex.Message)
					End Try

				End If
			Catch ex As Exception

			End Try

			Try
				If flags.UpdateServiceCollection(MonitoredItems.ServerTypes.Domino_Cluster, NodeName) Then
					Try
						WriteAuditEntry(Now.ToString & " Refreshing configuration of Domino clusters, on requst.", LogLevel.Verbose)
						CreateDominoClusterCollection()
						WriteAuditEntry(Now.ToString & " Refreshing Status Table for clusters.", LogLevel.Verbose)
						UpdateStatusTableWithDominoClusters()
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " Error Refreshing Domino cluster settings on demand: " & ex.Message)
					End Try

				End If

			Catch ex As Exception

			End Try

			Try
				If flags.UpdateServiceCollection(MonitoredItems.ServerTypes.Notes_Databases, NodeName) Then
					Try
						WriteAuditEntry(Now.ToString & " Refreshing configuration of Notes databases, on request.", LogLevel.Verbose)
						CreateNotesDatabaseCollection()
						WriteAuditEntry(Now.ToString & " Refreshing Status Table for Notes Databases, on request. ", LogLevel.Verbose)
						UpdateStatusTableWithNotesDatabases()
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " Error Refreshing Notes Database settings on demand: " & ex.Message)
					End Try
				End If
			Catch ex As Exception

			End Try

			Try
				If flags.UpdateServiceCollection(MonitoredItems.ServerTypes.Notes_Mail_Probe, NodeName) Then
					Try
						WriteAuditEntry(Now.ToString & " Refreshing configuration of NotesMail Probes, on request.", LogLevel.Verbose)
						CreateNotesMailProbeCollection()
						WriteAuditEntry(Now.ToString & " Refreshing Status Table for NotesMail Probes, on request. ", LogLevel.Verbose)
						UpdateStatusTableWithNotesMailProbes()
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " Error Refreshing NotesMail Probe settings on demand: " & ex.Message)
					End Try
				End If
			Catch ex As Exception

			End Try

			Try
				If flags.UpdateServiceCollection(MonitoredItems.ServerTypes.Traverler, NodeName) Then
					Try
						WriteAuditEntry(Now.ToString & " Refreshing configuration of Traveler Back End, on request.", LogLevel.Verbose)
						CreateTravelerBackEndCollection()
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " Error Refreshing Traveler Back End settings on demand: " & ex.Message)
					End Try
				End If
			Catch ex As Exception

			End Try

			Try
				If flags.UpdateServiceCollection(MonitoredItems.ServerTypes.Key_Words, NodeName) Then
					Try
						WriteAuditEntry(Now.ToString & " Refreshing configuration of Keywords, on request.", LogLevel.Verbose)
						CreateKeywordsCollection()
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " Error Refreshing Keywords settings on demand: " & ex.Message)
					End Try
				End If
			Catch ex As Exception

			End Try



			Thread.Sleep(10000)

		Loop


	End Sub



	Protected Sub MonitorDominoRelated()
		Dim MonitorDominoStart As DateTime
		Dim DominoElapsed As TimeSpan
        Dim myRegistry As New VSFramework.RegistryHandler

		MonitorDominoStart = Now

		WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " Starting new 'All Things Domino'  loop.")

		WriteAuditEntry(Now.ToString & " Starting new 'All Things Domino'  loop.")
		' So start the threads sequentially, instead of all at once 
		Dim n As Integer = 0

		Dim boolNotResponding As Boolean = False
		Dim EnabledCount As Integer = 0

        Dim MonitorDominoTableChanges As New Thread(AddressOf CheckForDominoTableChanges)
        MonitorDominoTableChanges.Start()

		Do Until boolTimeToStop = True
            'Don't do some things right away, it is important to get the servers scanned first 
            Dim AllScanned As Boolean = True
            Try

                For Each DominoServer As MonitoredItems.DominoServer In MyDominoServers
                    If DominoServer.Status = "Not Scanned" And DominoServer.Enabled = True Then
                        AllScanned = False
                        ' If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Cluster and Database monitoring is deferred until all Domino servers have first been scanned.")
                        Exit For
                    End If
                Next


            Catch ex As Exception

            End Try

            Try
                '********* Monitor Notes Databases
                If MyNotesDatabases.Count > 0 And AllScanned = True Then
                    WriteAuditEntry(Now.ToString & " Checking on Notes databases.", LogLevel.Verbose)
                    Dim threadMonitorNotesDB As New Thread(AddressOf MonitorNotesDatabases)
                    threadMonitorNotesDB.Start()
                    ' Call MonitorNotesDatabases()
                    Thread.Sleep(250)
                End If
                dtDominoLastUpdate = Now
            Catch ex As Exception

            End Try

            '2/4/2016 NS commented out for VSPLUS-2560
            'Try
            '    If AllScanned = True And myDominoClusters.Count > 0 Then
            '        Dim threadMonitorCluster As New Thread(AddressOf MonitorDominoCluster)
            '        WriteAuditEntry(Now.ToString & " Starting cluster analysis in a background thread.", LogLevel.Verbose)

            '        threadMonitorCluster.Start()
            '        '  MonitorDominoCluster()
            '        Thread.Sleep(250)
            '    End If

            'Catch ex As Exception

            'End Try



            'Try

            '    '********* Monitor Notes Mail

            '    If AllScanned = True And MyNotesMailProbes.Count > 0 Then  ' And n = 5 Then
            '        WriteAuditEntry(Now.ToString & " Starting NotesMail Probe monitoring.", LogLevel.Verbose)
            '        MonitorNotesMail()
            '        dtDominoLastUpdate = Now
            '        Thread.Sleep(250)
            '    End If

            'Catch ex As Exception

            'End Try

            Try

                DominoElapsed = MonitorDominoStart.Subtract(Now)

                'DateTime dt2 = new DateTime(2003,4,15);
                'TimeSpan ts = dt1.Subtract(dt2);
                'If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " ***  Domino Start = " & MonitorDominoStart.ToLongTimeString)
                'If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " ***  Now = " & Now.ToLongTimeString)

                'If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " ***  Elapsed Time TotalMinutes = " & DominoElapsed.TotalMinutes & " minutes")
                'If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " ***  Elapsed Time Minutes = " & DominoElapsed.Minutes & " minutes")
                'If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " ***  Elapsed Time seconds = " & DominoElapsed.Seconds & " seconds")
                '  If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " ***  Elapsed Time total seconds = " & DominoElapsed.TotalSeconds & " total seconds")
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error calculating runtime for Domino-related monitoring. " & ex.Message)
            End Try


            'Replaced with a separate thread to get config changes

            ''Update the settings every ten minutes, regardless
            'If DominoElapsed.TotalMinutes < -10 Then
            '    MonitorDominoStart = Now
            '    Try
            '        If MyDominoServers.Count > 0 Then
            '            WriteAuditEntry(Now.ToString & " Updating settings for Domino-related monitoring.", LogLevel.Verbose)
            '            ' MonitorDominoSettings()
            '        End If
            '    Catch ex As Exception

            '    End Try


            '    Try

            '        If MyDominoServers.Count > 0 Then
            '            WriteAuditEntry(Now.ToString & " Refreshing configuration of Domino servers", LogLevel.Verbose)
            '            CreateDominoServersCollection()
            '            WriteAuditEntry(Now.ToString & " Refreshing Status Table for Domino", LogLevel.Verbose)
            '            UpdateStatusTableWithDomino()
            '        End If

            '        If MyNotesDatabases.Count > 0 Then
            '            WriteAuditEntry(Now.ToString & " Refreshing configuration of Notes Databases", LogLevel.Verbose)
            '            CreateNotesDatabaseCollection()
            '            UpdateStatusTableWithNotesDatabases()
            '        End If


            '        'If MyConsoleCommands.Count > 0 Then
            '        '    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Refreshing configuration of scheduled console Commands")
            '        '    CreateScheduledCommandsCollection()
            '        'End IfLicenseCount

            '    Catch ex As Exception

            '    End Try

            '    '  MonitorDominoStart = Now
            'End If



            Try
                dtDominoLastUpdate = Now

                If MyDominoServers.Count < 30 And AllScanned = True Then
                    Thread.Sleep(10000)
                End If
                If MyDominoServers.Count < 20 And AllScanned = True Then
                    Thread.Sleep(10000)
                End If
                If MyDominoServers.Count < 10 And AllScanned = True Then
                    Thread.Sleep(15000)
                End If
                dtDominoLastUpdate = Now
            Catch ex As Exception

            End Try


            'Try
            '    MonitorDominoDone = Now.Ticks
            '    elapsed = New TimeSpan(MonitorDominoDone - MonitorDominoStart)
            '    If elapsed.TotalSeconds > 5 Then
            '        WriteAuditEntry(Now.ToString & " Completed one loop of Domino Monitoring in " & elapsed.TotalSeconds & " seconds.")
            '    End If
            'Catch ex As Exception

            'End Try



            Try
                '   WriteAuditEntry(Now.ToString & " Calling the garbage collector. ")
                GC.Collect()
            Catch ex As Exception

            End Try

            Try
                Thread.Sleep(1000)
            Catch ex As Exception

            End Try

        Loop

       

		Try

		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Exception processing Domino Monitoring loop: " & ex.ToString)
		End Try

	End Sub

    '1/4/2016 NS added for VSPLUS-2434
    Protected Sub UpdateMailStats()
		Do While boolTimeToStop <> True
			Try

			
			WriteAuditEntry(Now.ToString & " Will now attempt to update the Settings table in SQL.")
			If MailStatsDict.Count > 0 Then
				WriteAuditEntry(Now.ToString & " Found statistics, trying to update SQL.")
				Dim key As Dictionary(Of String, Integer).KeyCollection = MailStatsDict.Keys
				For i As Integer = 0 To key.Count - 1
					WriteAuditEntry(Now.ToString & " Updating the SQL Settings table with " & key(i).ToString() & "   " & MailStatsDict.Item(key(i)).ToString())
					WriteSettingsValue(key(i), MailStatsDict.Item(key(i)))

				Next

				'For Each key As String In MailStatsDict.Keys
				'	WriteAuditEntry(Now.ToString & " Updating the SQL Settings table with " & key & "   " & MailStatsDict(key))
				'	WriteSettingsValue(key, MailStatsDict(key))
				'Next
			Else
				WriteAuditEntry(Now.ToString & " No statistics found.")
			End If
				Thread.Sleep(300000) '5 minutes
			Catch ex As Exception
				WriteAuditEntry(Now.ToString & " Error Updating the SQL Settings table with. Exception " + ex.Message.ToString())
			End Try
		Loop
    End Sub
#End Region



	'****** Domino
#Region "Domino Log File Scanning"

	Public Class LogFileKeyword
		Public Property Keyword As String
		Public Property RepeatOnce As Boolean
        Public Property NotRequiredKeyword As String
        Public Property ScanLog As Boolean
		Public Property ScanAgentLog As Boolean
		Public Property ServerID As Integer
		Public Property ServerName As String

    End Class

	Public Class KeywordsCollection
		Inherits System.Collections.CollectionBase

		Public Sub Add(ByVal objItemToAdd As LogFileKeyword)
			Me.List.Add(objItemToAdd)
		End Sub

		Public ReadOnly Property Item(ByVal iIndex As Integer) As LogFileKeyword
			Get
				Return Me.List(iIndex)
			End Get
		End Property
	End Class
    Public Function convertQuotes(ByVal str As String) As String
        convertQuotes = str.Replace("'", "''")
    End Function

	Private Sub CheckDominoLogFile(ByRef DominoServer As MonitoredItems.DominoServer)
		'Dim servername As New Data.DataSet
		'passind server id get servername

		WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Inside CheckDominoLogFile")

		'If DominoServer.ScanLog = False Then
		'	WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " DominoServer.ScanLog false")

		'	Exit Sub
		'End If
		If myKeywords.Count = 0 Then
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Not scanning the log file because no keywords are defined.")
			Exit Sub
		Else
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Scanning the log file for keywords.")
		End If
		' WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "  ")
		' WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " ---------------  Begin Recent Log Events  -------------------")

		Dim db As Domino.NotesDatabase
		Dim view As Domino.NotesView
		Dim doc As Domino.NotesDocument
		Dim field As Domino.NotesItem
		WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Scanning the log file before database.")
		Try
			db = NotesSession.GetDatabase(DominoServer.Name, "log.nsf", False)
		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error getting log.nsf -> " & ex.ToString)
			GoTo Cleanup
		End Try

		Try
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Scanning the log file before MiscEvents.")
			view = db.GetView("MiscEvents")
		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error getting log.nsf views -> " & ex.ToString)
			GoTo Cleanup
		End Try

		Try

			If Not (view Is Nothing) Then

				If DominoServer.LastLogDocScanned = "" Then
					doc = view.GetLastDocument
					DominoServer.LastLogDocScanned = doc.UniversalID
					WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Scanning the log file  LastLogDocScanned is empty")
				Else
					'4/6/2015 NS modified
					'db.GetDocumentByUNID(DominoServer.LastLogDocScanned)
					'7/8/2015 NS modified for VSPLUS-1761
					'doc = db.GetDocumentByUNID(DominoServer.LastLogDocScanned)
					'doc = view.GetNextDocument(doc)
					WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Scanning the log file  LastLogDocScanned is not empty")
					doc = view.GetLastDocument

					While Not doc Is Nothing
						WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " After last count is nothing")
						If doc.UniversalID = DominoServer.LastLogDocScanned Then
							WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Prev doc " & doc.UniversalID, LogUtilities.LogUtils.LogLevel.Verbose)
							doc = view.GetNextDocument(doc)
							If Not doc Is Nothing Then
								WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Next doc " & doc.UniversalID, LogUtilities.LogUtils.LogLevel.Verbose)
							Else
								WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Next doc is nothing ", LogUtilities.LogUtils.LogLevel.Verbose)
							End If
							Exit While
						Else
							doc = view.GetPrevDocument(doc)
						End If
					End While
				End If

			End If
		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error getting log.nsf document-> " & ex.ToString)
			GoTo Cleanup
		End Try


		Try
			Dim i As Integer = 0
			For i = 0 To myKeywords.Count - 1
				WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Searching log file for " & myKeywords.Item(i).Keyword)
			Next
		Catch ex As Exception

		End Try

		While Not doc Is Nothing

			Try
				If Not (doc Is Nothing) Then
					field = doc.GetFirstItem("EventList")
					WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Searching log file for EventList")
				End If
				Dim i As Integer = 0
                Dim item As String
                '4/26/2016 NS added for VSPLUS-2844
                Dim excludeArr() As String
                Dim n As Integer = 0
                Dim proceed As Boolean = True

				WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Searching log file for before field values")
				For Each item In field.Values
					'  WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Event: " & item.ToString)
					For i = 0 To myKeywords.Count - 1
						Try
							WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Searching log file for mykeywordscount-1")
							WriteDeviceHistoryEntry("Domino", DominoServer.Name, UCase(myKeywords.Item(i).ServerName) & "-" & UCase(DominoServer.Name) & "-- Condition Status --" & (UCase(myKeywords.Item(i).ServerName) = UCase(DominoServer.Name)))

							WriteDeviceHistoryEntry("Domino", DominoServer.Name, UCase(item.ToString) & "-" & UCase(myKeywords.Item(i).Keyword) & "-- Condition Status --" & (InStr(UCase(item.ToString), UCase(myKeywords.Item(i).Keyword)) > 0))
							WriteDeviceHistoryEntry("Domino", DominoServer.Name, "-- Condition Status Scn log --" & (myKeywords.Item(i).ScanLog))

							'4/7/2015 NS modified for VSPLUS-1630
							'modified for VSPLUS-2300
							If myKeywords.Item(i).ScanLog = True And InStr(UCase(item.ToString), UCase(myKeywords.Item(i).Keyword)) > 0 And UCase(myKeywords.Item(i).ServerName) = UCase(DominoServer.Name) Then
                                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " all keywords searching")
                                '4/26/2016 NS modified for VSPLUS-2844
                                If InStr(myKeywords.Item(i).NotRequiredKeyword, ",") > 0 Then
                                    excludeArr = myKeywords.Item(i).NotRequiredKeyword.Split(",")
                                    For n = 0 To excludeArr.Length - 1
                                        If InStr(UCase(item.ToString), UCase(excludeArr(n))) > 0 Then
                                            proceed = False
                                            Exit For
                                        End If
                                    Next
                                Else
                                    If InStr(UCase(item.ToString), UCase(myKeywords.Item(i).NotRequiredKeyword)) > 0 And myKeywords.Item(i).NotRequiredKeyword <> "" Then
                                        proceed = False
                                    End If
                                End If
                                'If InStr(UCase(item.ToString), UCase(myKeywords.Item(i).NotRequiredKeyword)) = 0 Or myKeywords.Item(i).NotRequiredKeyword = "" Then
                                If proceed Then
                                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " The keyword or phrase -- " & myKeywords.Item(i).Keyword & " -- was found in the log file.  ")
                                    If myKeywords.Item(i).RepeatOnce = True Then
                                        '10/3/2014 NS modified for VSPLUS-981
                                        '4/6/2015 NS reverted the code back to the original line below in order to keep track of multiple keywords per server
                                        myAlert.QueueAlert("Domino", DominoServer.Name, "Log File - " & myKeywords.Item(i).Keyword, "The keyword or phrase -- " & myKeywords.Item(i).Keyword & " -- was found in the log file.  " & vbCrLf & vbCrLf & "The entry was " & item, DominoServer.Location)
                                        '12/18/2014 NS modified
                                        'myAlert.QueueAlert("Domino", DominoServer.Name, "Log File", "The keyword or phrase -- " & myKeywords.Item(i).Keyword & " -- was found in the log file.  " & vbCrLf & vbCrLf & "The entry was " & convertQuotes(item), DominoServer.Location)
                                    Else
                                        '10/3/2014 NS modified for VSPLUS-981
                                        '4/6/2015 NS reverted the code back to the original line below in order to keep track of multiple keywords per server
                                        '5/14/2015 NS modified for VSPLUS-1761 - added ticks in parenthesis to distinguish between entries when a lot of records are found
                                        myAlert.QueueAlert("Domino", DominoServer.Name, "Log File - " & myKeywords.Item(i).Keyword & " at " & Now.ToString & " (" & Now.Ticks.ToString() & ")", "The keyword or phrase -- " & myKeywords.Item(i).Keyword & " -- was found in the log file.  " & vbCrLf & vbCrLf & "The entry was " & item, DominoServer.Location)
                                        '12/18/2014 NS modified
                                        'myAlert.QueueAlert("Domino", DominoServer.Name, "Log File", "The keyword or phrase -- " & myKeywords.Item(i).Keyword & " -- was found in the log file.  " & vbCrLf & vbCrLf & "The entry was " & convertQuotes(item), DominoServer.Location)
                                    End If
                                End If
                            End If
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error in inner try block for log scanning.  Error: " & ex.Message)
                        End Try

					Next

				Next

			Catch ex As Exception
				WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error in outer try block for log scanning.  Error: " & ex.Message)
			End Try

			'  WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " ---------------  Advancing to next Log document  -------------------")
			DominoServer.LastLogDocScanned = doc.UniversalID
			doc = view.GetNextDocument(doc)


		End While

		'     WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " ---------------  End Recent Log Events  -------------------")
Cleanup:
		Try
			System.Runtime.InteropServices.Marshal.ReleaseComObject(doc)
			System.Runtime.InteropServices.Marshal.ReleaseComObject(field)
			System.Runtime.InteropServices.Marshal.ReleaseComObject(view)
			System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
		Catch ex As Exception

		End Try
	End Sub

    Private Sub CheckDominoAgentLogFile(ByRef DominoServer As MonitoredItems.DominoServer)

		'If DominoServer.ScanAgentLog = False Then
		'	WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "  AGENT log file after DominoServer.ScanAgentLog = False.")
		'	Exit Sub
		'End If
        If myKeywords.Count = 0 Then
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Not scanning the AGENT log file because no keywords are defined.")
            Exit Sub
        Else
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Scanning the Agent log file for keywords.")
        End If
        ' WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "  ")
        ' WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " ---------------  Begin Recent Log Events  -------------------")

        Dim db As Domino.NotesDatabase
        Dim view As Domino.NotesView
        Dim doc As Domino.NotesDocument
        Dim field As Domino.NotesItem

        Try
            db = NotesSession.GetDatabase(DominoServer.Name, "agentlog.nsf", False)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Does the agentlog.nsf exist on this server?  Error getting agentlog.nsf -> " & ex.ToString)
            GoTo Cleanup
        End Try

        Try
            view = db.GetView("Agent Activity")
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error getting agent log.nsf view 'agent activity' -> " & ex.ToString)
            GoTo Cleanup
        End Try

        Try
            If Not (view Is Nothing) Then
                If DominoServer.LastAgentLogDocScanned = "" Then
                    doc = view.GetLastDocument
                    DominoServer.LastAgentLogDocScanned = doc.UniversalID
                Else
                    '4/7/2015 NS modified
                    'db.GetDocumentByUNID(DominoServer.LastAgentLogDocScanned)
                    '7/8/2015 NS modified for VSPLUS-1761
                    'doc = db.GetDocumentByUNID(DominoServer.LastAgentLogDocScanned)
                    doc = view.GetLastDocument
                    While Not doc Is Nothing
                        If doc.UniversalID = DominoServer.LastAgentLogDocScanned Then
                            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Prev doc " & doc.UniversalID, LogUtilities.LogUtils.LogLevel.Verbose)
                            doc = view.GetNextDocument(doc)
                            If Not doc Is Nothing Then
                                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Next doc " & doc.UniversalID, LogUtilities.LogUtils.LogLevel.Verbose)
                            Else
                                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Next doc is nothing ", LogUtilities.LogUtils.LogLevel.Verbose)
                            End If
                            Exit While
                        Else
                            doc = view.GetPrevDocument(doc)
                        End If
                    End While
                End If

            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error looping through  agent log.nsf document-> " & ex.ToString)
            GoTo Cleanup
        End Try


        Try
            Dim i As Integer = 0
            For i = 0 To myKeywords.Count - 1
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Searching agent log file for " & myKeywords.Item(i).Keyword)
            Next
        Catch ex As Exception

		End Try


		While Not doc Is Nothing
			WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Searching agent log file after doc")
			Try
				If Not (doc Is Nothing) Then
					WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Searching agent log file after doc is nothing")
					field = doc.GetFirstItem("A$ERRMSG")
				End If
				Dim i As Integer = 0
                Dim item As String
                '4/26/2016 NS added for VSPLUS-2844
                Dim excludeArr() As String
                Dim n As Integer = 0
                Dim proceed As Boolean = True

				For Each item In field.Values
					WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "field values")
					'  WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Event: " & item.ToString)
					For i = 0 To myKeywords.Count - 1
						Try
							WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " keywords count")
							'4/7/2015 NS modified for VSPLUS-1630
							If myKeywords.Item(i).ScanAgentLog = True And InStr(UCase(item.ToString), UCase(myKeywords.Item(i).Keyword)) > 0 And UCase(myKeywords.Item(i).ServerName) = UCase(DominoServer.Name) Then
                                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " keyword found")
                                '4/26/2016 NS modified for VSPLUS-2844
                                If InStr(myKeywords.Item(i).NotRequiredKeyword, ",") > 0 Then
                                    excludeArr = myKeywords.Item(i).NotRequiredKeyword.Split(",")
                                    For n = 0 To excludeArr.Length - 1
                                        If InStr(UCase(item.ToString), UCase(excludeArr(n))) > 0 Then
                                            proceed = False
                                            Exit For
                                        End If
                                    Next
                                Else
                                    If InStr(UCase(item.ToString), UCase(myKeywords.Item(i).NotRequiredKeyword)) > 0 And myKeywords.Item(i).NotRequiredKeyword <> "" Then
                                        proceed = False
                                    End If
                                End If
                                'If InStr(UCase(item.ToString), UCase(myKeywords.Item(i).NotRequiredKeyword)) = 0 Or myKeywords.Item(i).NotRequiredKeyword = "" Then
                                If proceed Then
                                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " The keyword or phrase -- " & myKeywords.Item(i).Keyword & " -- was found in the agent log file.  ")
                                    If myKeywords.Item(i).RepeatOnce = True Then
                                        '10/3/2014 NS modified for VSPLUS-981
                                        '4/6/2015 NS reverted the code back to the original line below in order to keep track of multiple keywords per server
                                        myAlert.QueueAlert("Domino", DominoServer.Name, "Agent Log File - " & myKeywords.Item(i).Keyword, "The keyword or phrase -- " & myKeywords.Item(i).Keyword & " -- was found in the agent log file.  " & vbCrLf & vbCrLf & "The entry was " & item, DominoServer.Location)
                                        '12/18/2014 NS modified
                                        'myAlert.QueueAlert("Domino", DominoServer.Name, "Log File", "The keyword or phrase -- " & myKeywords.Item(i).Keyword & " -- was found in the agent log file.  " & vbCrLf & vbCrLf & "The entry was " & convertQuotes(item), DominoServer.Location)
                                    Else
                                        '10/3/2014 NS modified for VSPLUS-981
                                        '4/6/2015 NS reverted the code back to the original line below in order to keep track of multiple keywords per server
                                        '5/14/2015 NS modified for VSPLUS-1761 - added ticks in parenthesis to distinguish between entries when a lot of records are found
                                        myAlert.QueueAlert("Domino", DominoServer.Name, "Agent Log File - " & myKeywords.Item(i).Keyword & " at " & Now.ToString & " (" & Now.Ticks.ToString() & ")", "The keyword or phrase -- " & myKeywords.Item(i).Keyword & " -- was found in the agent log file.  " & vbCrLf & vbCrLf & "The entry was " & item, DominoServer.Location)
                                        '12/18/2014 NS modified
                                        'myAlert.QueueAlert("Domino", DominoServer.Name, "Log File", "The keyword or phrase -- " & myKeywords.Item(i).Keyword & " -- was found in the agent log file.  " & vbCrLf & vbCrLf & "The entry was " & convertQuotes(item), DominoServer.Location)
                                    End If
                                End If
                            End If
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error in inner try block for agent log scanning.  Error: " & ex.Message)
                        End Try

					Next

				Next

			Catch ex As Exception
				WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error in outer try block for agent log scanning.  Error: " & ex.Message)
			End Try
			doc = view.GetNextDocument(doc)
			'  WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " ---------------  Advancing to next Log document  -------------------")
		End While

        '     WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " ---------------  End Recent Log Events  -------------------")
Cleanup:
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(doc)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(field)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(view)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
        Catch ex As Exception

        End Try
    End Sub


#End Region


#Region "Domino Monitoring"

    Private Sub MonitorDomino()
        Dim myRegistry As New VSFramework.RegistryHandler()
        Do While boolTimeToStop <> True
            'dll.W32_NotesInitThread()
            Dim oWatch As New System.Diagnostics.Stopwatch

            ' WriteAuditEntry(Now.ToString & " >>> Begin Loop for Domino Monitoring >>>>")
            dtDominoLastUpdate = Now
            Dim myServer As MonitoredItems.DominoServer

            Try
                DominoSelector_Mutex.WaitOne()
                myServer = SelectDominoServerToMonitor()
                If Not (myServer Is Nothing) Then
                    myServer.IsBeingScanned = True
                    myServer.LastScan = Now
                    'myServer.LastPing = Now
                End If
            Catch ex As Exception
                myServer = Nothing
            Finally
                DominoSelector_Mutex.ReleaseMutex()
            End Try

            Try
                If myServer Is Nothing Then
                    'no servers due
                    GoTo WaitHere
                Else
                    WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " Selected " & myServer.Name & " --- with current status of " & myServer.Status)
                    ' myServer.LastScan = Date.Now
                    If Trim(myServer.Status) = "OK" Or Trim(myServer.Status) = "Not Scanned" Then
                        myServer.Status = "Scanning"
                    End If
                End If

            Catch ex As Exception

            End Try


            Try
                'WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " >>> Begin Loop to monitor " & myServer.Name)
                oWatch.Start()
            Catch ex As Exception

            End Try

            Dim t As Thread
            Dim threadLife As Integer

            Try
                threadLife = Convert.ToInt32(myRegistry.ReadFromRegistry("ThreadWaitTimeForDomino"))
            Catch ex As Exception
                threadLife = 10
            End Try

            WriteDeviceHistoryEntry("Domino", myServer.Name, Now.ToString & " Starting Scan.", LogLevel.Normal)
            Try
                t = New Thread(Sub() Me.MonitorDomino(myServer))
                t.CurrentCulture = Thread.CurrentThread.CurrentCulture
                t.Start()
                If t.Join(New TimeSpan(0, threadLife, 0)) Then
                    'thread finished in the threadlife time
                Else
                    WriteDeviceHistoryEntry("Domino", myServer.Name, Now.ToString & " Thread got hung and will be aborted.", LogLevel.Normal)
                    HungThreadsKilled += 1
                    WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " thread monitoing " & myServer.Name & " took longer then " & threadLife & " minutes. It will be killed. Current killed thread count: " & HungThreadsKilled)
                    t.Abort()
                    Try
                        UpdateDominoThreadKilledCounter()
                    Catch ex As Exception

                    End Try
                End If



            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", myServer.Name, Now.ToString & " Error in new thread.  Error: " & ex.Message, LogLevel.Normal)
            End Try

            WriteDeviceHistoryEntry("Domino", myServer.Name, Now.ToString & " Finishing Scan", LogLevel.Normal)
WaitHere:

            Try
                oWatch.Stop()
                myServer.IsBeingScanned = False
                If myServer.Enabled = True Then
                    WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " --------> End Loop to monitor " & myServer.Name & "  Elapsed Time= " & oWatch.Elapsed.TotalSeconds & " seconds.  Ending status: " & myServer.Status)
                End If
                myServer = Nothing
            Catch ex As Exception

            End Try
            'dll.W32_NotesTermThread()

            If boolTimeToStop = True Then Exit Do
            Thread.Sleep(10000)
        Loop

        WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " --------------------> MonitorDomino thread instance terminated in response to shutdown command. ")

    End Sub

    Private Sub MonitorDomino(ByRef myServer As MonitoredItems.DominoServer)
        dll.W32_NotesInitThread()
        '3/28/2016 NS added for VSPLUS-2669
        'The default value of a server's time zone is set to 1000. We check it at the beginning to only set it once every cycle
        If myServer.ServerTimeTZ = 1000 Then
            Dim s As New Domino.NotesSession
            Dim consolestr As String
            Dim myPassword = GetNotesPassword()
            Try
                s.Initialize(myPassword)
                consolestr = s.SendConsoleCommand(myServer.Name, "show time")
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s)
                GetServerTimeZone(consolestr, myServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", myServer.Name, Now.ToString & " Error accessing server time : " & ex.ToString)
            End Try
        End If
        
        Try
            If InStr(myServer.Name, "RPRWyattTest") Then
                WriteDeviceHistoryEntry("Domino", myServer.Name, Now.ToString & " Simulating a scan.  It's all good.", LogLevel.Verbose)
                myServer.Status = "OK"
                myServer.StatusCode = "OK"
                myServer.PendingThreshold = 100
                myServer.DeadThreshold = 100
                myServer.HeldThreshold = 100
                myServer.PendingThreshold = 100
                myServer.IsUp = True
                myServer.Description = "This server is a test account for RPR Wyatt, not a real server."
                myServer.ResponseDetails = "Last scanned at " & Date.Now.ToShortTimeString
                myServer.UserCount = CInt(Math.Ceiling(Rnd() * 50))
                myServer.PendingMail = CInt(Math.Ceiling(Rnd() * 25))
                myServer.DeadMail = CInt(Math.Ceiling(Rnd() * 100))
                myServer.ResponseTime = CInt(Math.Ceiling(Rnd() * 1000))
                Dim myStatus As Integer = CInt(Math.Ceiling(Rnd() * 100))
                Select Case myStatus
                    Case 1 To 3
                        myServer.Status = "Not Responding"
                        myServer.StatusCode = "Not Responding"
                        myServer.IsUp = False
                        myServer.ResponseTime = 0
                    Case 3 To 10
                        myServer.Status = "Low Disk Space"
                        myServer.StatusCode = "Issue"
                        myServer.ResponseDetails = "Successfully connected to the server at " & Date.Now.ToShortTimeString & " Drive C is running out of space."
                    Case 11 - 15
                        myServer.Status = "Slow"
                        myServer.ResponseDetails = "Successfully connected to the server at " & Date.Now.ToShortTimeString
                        myServer.StatusCode = "Issue"
                    Case 16 - 20
                        myServer.Status = "Maintenance"
                        myServer.ResponseDetails = "The server is in a scheduled maintenance period and will not be scanned."
                        myServer.StatusCode = "Maintenance"
                    Case Else
                        myServer.Status = "OK"
                        myServer.ResponseDetails = "Successfully connected to the server at " & Date.Now.ToShortTimeString
                        myServer.Description = "Due to be next scanned at " & myServer.NextScan.ToShortTimeString
                        myServer.IsUp = True

                End Select
                myServer.IsBeingScanned = False
                UpdateDominoStatusTable(myServer)
                Exit Sub
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", myServer.Name, Now.ToString & " Exception with fake server: " & ex.ToString)
        End Try


        Try
            If myServer.Enabled = False Then
                myServer.LastScan = Now
                myServer.Status = "Disabled"
                myServer.ResponseDetails = "This server is not enabled for monitoring."
                Try
                    UpdateDominoStatusTable(myServer)
                Catch ex As Exception

                End Try

                Exit Sub
            End If
        Catch ex As Exception

        End Try

        Dim bIsInMaintenance As Boolean = InMaintenance("Domino", myServer.Name)
        If bIsInMaintenance Then
            Try
                '3/13/2015 NS added for VSPLUS-1422
                myAlert.DeleteAlert("Domino", myServer.Name, myServer.Location)
                myServer.Status = "Maintenance"
                myServer.LastScan = Now
                myServer.ResponseDetails = "This server is in a scheduled maintenance period.  Monitoring is temporarily disabled."
                myServer.PendingMail = 0
                myServer.DeadMail = 0
                myServer.HeldMail = 0
                myServer.UserCount = 0
                myServer.CPU_Utilization = 0
                myServer.IsBeingScanned = False
                UpdateDominoStatusTable(myServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", myServer.Name, Now.ToString & " Error handling Domino server in maintenance period: " & ex.Message)
            End Try
        End If

        If bIsInMaintenance Then
            myServer.IsBeingScanned = False
            Exit Sub
        End If


        Try
            myServer.Memory = ""
            myServer.TaskStatus = ""
            myServer.PreviousKeyValue = myServer.ResponseTime  'remember the last response time
            dtDominoLastUpdate = Now
        Catch ex As Exception

        End Try

        'Try
        '    WriteDeviceHistoryEntry("Domino", myServer.Name, Now.ToString & " Attempting to update settings prior to a scan.")
        '    UpdateDominoServerSettings(myServer)
        'Catch ex As Exception
        '    WriteDeviceHistoryEntry("Domino", myServer.Name, Now.ToString & " Error updating scan settings: " & ex.Message)
        'End Try

        'Figure out whether to ping or scan
        Dim Ping As Boolean = True

        Try
            If myServer.PingCount > 2 Or myServer.PingCount = 0 Then
                Ping = False
                myServer.PingCount = 1
            End If

            If (myServer.Status = "Not Responding" Or myServer.Status = "Network Issue") Then
                Ping = True
            End If
            'If the server is set to scan from the editor screen, then do a fast scan than ping scan or any other scan. 
            If (myServer.scanASAP = True) Then
                WriteDeviceHistoryEntry("Domino", myServer.Name, Now.ToString & "Since Scan Now Button is pressed doing Fast Scan ")
                Ping = False
                myServer.scanASAP = False
            End If
        Catch ex As Exception
            Ping = True
        End Try

        'Never just ping Traveler servers, they are too important
        If myServer.Traveler_Server = True Then
            Ping = False
        End If

        'If a server has server tasks to monitor, don't do a ping but a full scan
        If myServer.ServerTaskSettings.Count > 0 Then
            Ping = False
        End If

        Try
            If Ping = True Then
                WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " Begin just ping cycle scan of " & myServer.Name & ".  Ping count = " & myServer.PingCount & ", Scan count = " & myServer.ScanCount & " and status of " & myServer.Status)
                JustPingDominoServer(myServer)
            Else
                WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " Begin fast scan of " & myServer.Name & ".  Ping count = " & myServer.PingCount & ", Scan count = " & myServer.ScanCount)
                QueryDominoServer(myServer)
                'Reset Ping back to 1 so the next scan will be a ping
                myServer.PingCount = 1
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " Exception at scan type decision: " & ex.ToString)
        End Try


        Try
            If (myServer.Status = "Not Responding" Or myServer.Status = "Network Issue") Then
                myServer.ScanCount = 1
            End If
        Catch ex As Exception

        End Try

        Try
            myServer.IsBeingScanned = False
        Catch ex As Exception

        End Try

        Try
            If myServer.RestartRouter = True Then
                Try
                    If myServer.PendingMail > 1.5 * myServer.PendingThreshold Then
                        SendDominoConsoleCommands(myServer.Name, "tell router restart", "Sending tell Router restart command because Pending mail is more than 1.5 times of pending threshold")
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", myServer.Name, Now.ToString & ex.Message & " Error while sending Console Command " & ex.Source)
                End Try
            End If

        Catch ex As Exception

        End Try

        dll.W32_NotesTermThread()
    End Sub


    Private Function SelectDominoServerToMonitor() As MonitoredItems.DominoServer
        ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " >>> Selecting a Domino Server for monitoring >>>>")
        Dim tNow As DateTime
        tNow = Now
        Dim tScheduled As DateTime

        Dim timeOne, timeTwo As DateTime

        Dim SelectedServer As MonitoredItems.DominoServer

        Dim ServerOne As MonitoredItems.DominoServer
        Dim ServerTwo As MonitoredItems.DominoServer
        Dim myRegistry As New RegistryHandler

        Dim strSQL As String = ""
        Dim ServerType As String = "Domino"
        Dim serverName As String = ""
        Dim n As Integer

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

                For n = 0 To MyDominoServers.Count - 1
                    ServerOne = MyDominoServers.Item(n)

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
        For n = 0 To MyDominoServers.Count - 1
            ServerOne = MyDominoServers.Item(n)
            If ServerOne.Status = "Not Scanned" And ServerOne.Enabled = True And ServerOne.Traveler_Server = True And ServerOne.IsBeingScanned = False Then
                WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " >>> Selecting " & ServerOne.Name & " because it is a Traveler server and the status is " & ServerOne.Status)
                ' ServerOne.IsBeingScanned = True
                Return ServerOne
                Exit Function
            End If
        Next


        'Any server Not Responding that is due for a scan should be scanned right away.  Select the first one you encounter
        For n = 0 To MyDominoServers.Count - 1
            ServerOne = MyDominoServers.Item(n)
            If ServerOne.Status = "Not Responding" And ServerOne.Enabled = True And ServerOne.IsBeingScanned = False Then
                tScheduled = CDate(ServerOne.NextScan)
                If DateTime.Compare(tNow, tScheduled) > 0 Then
                    WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status & ".  Next scheduled scan is " & tScheduled.ToShortTimeString)
                    ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status)
                    'ServerOne.IsBeingScanned = True
                    Return ServerOne
                    Exit Function
                Else
                    ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " " & ServerOne.Name & " is down, but not yet due to be re-scanned until " & ServerOne.NextScan)
                End If
            End If
        Next

        'Any server Not Scanned should be scanned right away.  Select the first one you encounter
        For n = 0 To MyDominoServers.Count - 1
            ServerOne = MyDominoServers.Item(n)
            If ServerOne.Status = "Not Scanned" And ServerOne.Enabled = True And ServerOne.IsBeingScanned = False Then
                WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status)
                ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status)
                ' ServerOne.IsBeingScanned = True
                Return ServerOne
                Exit Function
            End If
        Next


        Dim ScanCandidates As New MonitoredItems.DominoCollection
        For Each srv As MonitoredItems.DominoServer In MyDominoServers
            'WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " " & srv.Name & " scan status: " & srv.IsBeingScanned)
            If srv.IsBeingScanned = False And srv.Enabled = True Then
                tNow = Now
                tScheduled = srv.NextScan
                If DateTime.Compare(tNow, tScheduled) > 0 Then
                    ScanCandidates.Add(srv)
                End If

                ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " " & srv.Name & " is not being scanned.")
            End If
        Next
        ' WriteDeviceHistoryEntry("All", "Selection", vbCrLf & vbCrLf)


        If ScanCandidates.Count = 0 Then
            ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " No servers are due to be scanned exiting sub.")
            ' WriteDeviceHistoryEntry("All", "Selection", " ")
            Thread.Sleep(10000)
            Return Nothing
        Else
            ' WriteDeviceHistoryEntry("All", "Selection", vbCrLf & vbCrLf)
            ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " *********** Scan Candidates *************")
            For Each srv As MonitoredItems.DominoServer In ScanCandidates
                ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " " & srv.Name & " was last scanned at " & srv.LastScan & "   Scheduled Scan: " & srv.NextScan)
            Next
        End If

        '*****************

        'start with the first two servers
        ServerOne = ScanCandidates.Item(0)
        ServerTwo = ScanCandidates.Item(0)
        If ScanCandidates.Count > 1 Then ServerTwo = ScanCandidates.Item(1)

        Try
            ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " Finding which server is the most overdue to be monitored.")
            ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " Server One is  " & ServerOne.Name & ", due to be scanned " & ServerOne.NextScan)
            ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " Server Two " & ServerTwo.Name & ", due to be scanned " & ServerTwo.NextScan)
        Catch ex As Exception
        End Try


        'go through the remaining servers, see which one has the oldest (earliest) scheduled time
        If ScanCandidates.Count > 2 Then
            Try
                For n = 2 To ScanCandidates.Count - 1
                    '           WriteAuditEntry(Now.ToString & " N is " & n)
                    timeOne = CDate(ServerOne.NextScan)
                    timeTwo = CDate(ServerTwo.NextScan)
                    If DateTime.Compare(timeOne, timeTwo) < 0 Then
                        'time one is earlier than time two, so keep server 1
                        WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " " & ServerOne.Name & " is more overdue ")
                        ServerTwo = ScanCandidates.Item(n)
                    Else
                        'time two is later than time one, so keep server 2
                        ServerOne = ScanCandidates.Item(n)
                        WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " " & ServerTwo.Name & " is more overdue ")
                    End If

                    ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " Server One is  " & ServerOne.Name & ", due to be scanned " & ServerOne.NextScan)
                    ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " Server Two " & ServerTwo.Name & ", due to be scanned " & ServerTwo.NextScan)

                Next
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " >>> Error Selecting Domino server... " & ex.Message)
            End Try
        Else
            'There were only two server, so use those going forward
        End If

        ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " >>> Down to two servers... " & ServerOne.Name & " and " & ServerTwo.Name)

        'Of the two remaining servers, pick the one with earliest scheduled time for next scan
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
            ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " >>> Down to one server... " & SelectedServer.Name & " due to be scanned at " & SelectedServer.NextScan & ". Status is " & SelectedServer.Status)
        Else
            SelectedServer = ServerOne
            tScheduled = CDate(ServerOne.NextScan)
        End If

        tScheduled = CDate(SelectedServer.NextScan)
        If DateTime.Compare(tNow, tScheduled) < 0 Then
            If SelectedServer.Status <> "Not Scanned" Then
                ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " No Domino servers scheduled for monitoring, next scan after " & SelectedServer.NextScan)
                SelectedServer = Nothing
            Else
                ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " Selected Domino server: " & SelectedServer.Name & " because it has not been scanned yet.")
            End If
        Else
            ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " Selected Domino server: " & SelectedServer.Name)
            Dim mySpan As TimeSpan = tNow - tScheduled
            ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " " & mySpan.TotalSeconds & " seconds after schedule.")
        End If

        '**************

        'Release Memory
        tNow = Nothing
        tScheduled = Nothing
        n = Nothing

        timeOne = Nothing
        timeTwo = Nothing

        ServerOne = Nothing
        ServerTwo = Nothing

        'return selected server
        ' SelectedServer.IsBeingScanned = True
        SelectDominoServerToMonitor = SelectedServer


        'Exit Function
        SelectedServer = Nothing
    End Function

    Private Sub QueryDominoServer(ByRef MyDominoServer As MonitoredItems.DominoServer)
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, vbCrLf & Now.ToString & " -------------------------------------------------")
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Starting new QueryDominoServer cycle.")
        Dim oWatch As New System.Diagnostics.Stopwatch
        oWatch.Start()
        'MyDominoServer.LastPing = Now

        Try
            If Trim(MyDominoServer.Status) = "Not Responding" Then
                MyDominoServer.ResponseDetails = "Not Responding - New fast scan cycle started at " & Date.Now.ToShortTimeString
                MyDominoServer.Description = "Not Responding - New fast scan cycle started at " & Date.Now.ToShortTimeString
            ElseIf Trim(MyDominoServer.Status) = "Telnet" Then
                MyDominoServer.Description = "PING scan cycle started at " & Date.Now.ToShortTimeString

            Else
                MyDominoServer.ResponseDetails = "FAST scan cycle started at " & Date.Now.ToShortTimeString
                MyDominoServer.Description = "FAST scan cycle started at " & Date.Now.ToShortTimeString
            End If

            If Trim(MyDominoServer.Status) = "OK" Then
                MyDominoServer.Status = "Scanning"
            End If

            If MyDominoServer.Status = "Not Scanned" Then
                MyDominoServer.Status = "Scanning"
            End If

            UpdateDominoStatusTable(MyDominoServer)

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #1: " & ex.ToString)
        End Try

        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " QueryDominoServer cycle at A")


        'This sub coordinates the actual monitoring of a Domino server
        Dim ResponseTime As Double = 0
        Dim myUsers As Long
        Dim myMemory As String
        Dim MyStatValue As Double
        dtDominoLastUpdate = Now

        Try
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino thread is attempting to contact " & MyDominoServer.Name & ". This server has " & MyDominoServer.FailureCount & " consecutive failures.")
        Catch ex As Exception

        End Try

        Try
            MyDominoServer.Status = "OK"
            MyDominoServer.AlertCondition = False
            MyDominoServer.ResponseTime = 0
        Catch ex As Exception

        End Try


        '******************************************************

        Try
            dtDominoLastUpdate = Now
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino thread is attempting FAST SCAN of  " & MyDominoServer.Name & ". This server has " & MyDominoServer.FailureCount & " consecutive failures.")
            ' If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & MyDominoServer.Name & " was scheduled for " & MyDominoServer.LastScan & " will next be scanned at " & MyDominoServer.NextScan)
        Catch ex As Exception

        End Try



        Try
            If ResponseTime = 0 Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " QueryDominoServer is testing server response time via Notes API function.")
                ResponseTime = DominoResponseTime(MyDominoServer.Name)
                MyDominoServer.ResponseTime = ResponseTime
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The server responded in " & ResponseTime.ToString & " ms.")
            End If
        Catch ex As Exception
            MyDominoServer.ResponseTime = 0
            ResponseTime = 0
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #2: " & ex.ToString)
        End Try

        Try
            If ResponseTime = 0 Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server did not respond via Notes API function.  Now testing server response time via stats call.")
                ResponseTime = DominoResponseTimeByStatCall(MyDominoServer.Name)
                MyDominoServer.ResponseTime = ResponseTime
            End If
        Catch ex As Exception
            ResponseTime = 0
        End Try


        Try
            If ResponseTime > 0 Then
                MyDominoServer.ConsecutiveTelnetCount = 0
                myAlert.ResetAlert("Domino", MyDominoServer.Name, "Telnet", MyDominoServer.Location, "The server is responding to Notes client requests")
            End If

        Catch ex As Exception

        End Try

        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " I am here...")

        Dim TelnetSuccess As Boolean = False
        Try
            If ResponseTime = 0 Then
                ' WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Evaluating whether to test server via telnet function.")
                If MyDominoServer.IPAddress <> "" Then
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Testing server response time via telnet function.")
                    TelnetSuccess = TelnetDomino(MyDominoServer)
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Telnet response was " & TelnetSuccess.ToString)
                Else
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server does not have an IP address I can use for the telnet function.")
                End If

                If TelnetSuccess = True Then
                    ' MyDominoServer.ResponseTime = 15
                    MyDominoServer.Description = "Connected to the server via telnet on port 1352 at " & Now.ToShortTimeString
                    ' MyDominoServer.Status = "Telnet"
                    MyDominoServer.ResponseDetails = "Connected to the server via telnet on port 1352 at " & Now.ToShortTimeString
                    myAlert.ResetAlert("Domino", MyDominoServer.Name, "Not Responding", MyDominoServer.Location)
                    MyDominoServer.IsUp = True
                    MyDominoServer.LastScan = Now
                    MyDominoServer.IncrementUpCount()  '2
                    MyDominoServer.ConsecutiveTelnetCount += 1
                    If MyDominoServer.ConsecutiveTelnetCount > GetConsecutiveTelnetValue() Then
                        MyDominoServer.Status = "Telnet"
                        MyDominoServer.ResponseDetails = "The server is answering, but only to telnet on port 1352"
                        myAlert.QueueAlert("Domino", MyDominoServer.Name, "Telnet", "The server is answering, but only to telnet on port 1352.  This could indicate a problem.", MyDominoServer.Location)
                        '1/26/2016 NS modified for VSPLUS-2209
                    Else
                        MyDominoServer.Status = "OK"
                    End If
                    UpdateDominoStatusTable(MyDominoServer)
                    'Put the response time in the statistics.mdb 'DeviceDailyStat' table
                    UpdateDominoResponseTimeTable(MyDominoServer, MyDominoServer.ResponseTime)

                    Exit Sub
                End If
            End If


        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception using Telnet function: " & ex.ToString)
        End Try


        Try
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " QueryDominoServer cycle at C")
            If ResponseTime = 0 Then
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server did not respond on the first attempt.  It is probably down....")
            End If
        Catch ex As Exception

        End Try

        Try
            If ResponseTime <> 0 Then
                '1/21/2016 NS modified for VSPLUS-2534
                MyDominoServer.Description = "Successfully connected to the server via fast scan at " & Date.Now.ToShortTimeString & ". "
                MyDominoServer.ResponseDetails = "Successfully connected to the server via fast scan at " & Date.Now.ToShortTimeString & ". "
                MyDominoServer.Status = "OK"
                MyDominoServer.LastScan = Now
                'Server IS responding
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server responded in " & CType(ResponseTime, String) & " ms.")
                MyDominoServer.IncrementUpCount()  '1
                MyDominoServer.ResponseTime = ResponseTime
                '1/21/2016 NS modified for VSPLUS-2534
                MyDominoServer.Description = "Successfully connected to the server via fast scan at " & Now.ToShortTimeString & ". "
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " QueryDominoServer cycle at C(a)")
                myAlert.ResetAlert("Domino", MyDominoServer.Name, "Not Responding", MyDominoServer.Location, "The server is responding to Notes client requests")
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " QueryDominoServer cycle at C(b)")
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server up time is " & Microsoft.VisualBasic.Strings.Format(MyDominoServer.UpMinutes, "F1") & " minutes.")
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server down time is " & Microsoft.VisualBasic.Strings.Format(MyDominoServer.DownMinutes, "F1") & " minutes.")
                UpdateDominoStatusTable(MyDominoServer)
                MyDominoServer.IsUp = True

            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #3: " & ex.ToString)
        End Try


        If ResponseTime <> 0 Then
            '******************************
            '  Performance Targets
            '******************************
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " QueryDominoServer cycle at D")
            Try
                'Check if response is within Target
                With MyDominoServer
                    Try
                        If .ResponseTime <= .ResponseThreshold Then
                            'On Time
                            .IncrementOnTargetCount()
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino server " & MyDominoServer.Name & " is within performance target.  Server responded in " & MyDominoServer.ResponseTime & " ms, and target is " & MyDominoServer.ResponseThreshold & " ms.")

                            If .OffHours = False Then
                                .IncrementBusinessHoursOnTargetCount()
                            End If
                        Else
                            'Slow
                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino server " & MyDominoServer.Name & " is slow.  Server responded in " & MyDominoServer.ResponseTime & " ms but target is " & MyDominoServer.ResponseThreshold & " ms.")
                            .Status = "Slow"
                            .ResponseDetails = "Server responded in " & MyDominoServer.ResponseTime & " ms but target is " & MyDominoServer.ResponseThreshold & " ms."
                            .Description = "Server responded in " & MyDominoServer.ResponseTime & " ms but target is " & MyDominoServer.ResponseThreshold & " ms."

                            .IncrementOffTargetCount()
                            If .OffHours = False Then
                                .IncrementBusinessHoursOffTargetCount()
                            End If
                            '%%%%%%%%%%%%%
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & MyDominoServer.Name & " is slow, alert status is " & MyDominoServer.AlertCondition)

                        End If
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception checking Domino Performance Targets: " & ex.Message)
                    End Try
                End With

                Try
                    'Put the response time in the statistics.mdb 'DeviceDailyStat' table
                    UpdateDominoResponseTimeTable(MyDominoServer, ResponseTime)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error updating Domino Response Table " & ex.Message)

                End Try

            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error in Performance Tracking  block: " & ex.Message)

            End Try
        End If


        If ResponseTime <> 0 Then
            'OS and Version

            Try
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " QueryDominoServer cycle at Getting Statistics.. ")
                GetAllDominoStatistics(MyDominoServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #8: " & ex.ToString)
            End Try


            Try
                ServerElapsedTime(MyDominoServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Unhandled exception in ServerElapsedTime function: " & ex.ToString)
            End Try

            Try
                If MyDominoServer.Statistics_Memory <> "" Then
                    ' If MyDominoServer.UpCount > 1 Then
                    CheckDominoMemory(MyDominoServer)
                End If
                dtDominoLastUpdate = Now
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error checking memory:  " & ex.Message)
            End Try


            'Users
            Try
                If MyDominoServer.Statistics_Server <> "" Then
                    MyDominoServer.UserCount = ParseNumericStatValue("Server.Users", MyDominoServer.Statistics_Server)
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This server has " & MyDominoServer.UserCount & " users currently active. ")
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception calculating user count" & ex.ToString)
            End Try

            Try
                If MyDominoServer.OperatingSystem.Trim = "Unknown" Or MyDominoServer.OperatingSystem.Trim = "" Then
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & MyDominoServer.Name & " retrieving Operating System information.")

                    If InStr(MyDominoServer.Statistics_Server, "Version.OS") Then
                        MyDominoServer.OperatingSystem = ParseTextStatValue("Server.Version.OS", MyDominoServer.Statistics_Server)   'Server.Version.OS
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & MyDominoServer.Name & " is running on " & MyDominoServer.OperatingSystem)
                        Exit Try
                    End If

                    If InStr(MyDominoServer.OperatingSystem, "NSFGetServerStats") > 0 Then
                        MyDominoServer.OperatingSystem = "Unknown"
                    End If

                    If MyDominoServer.OperatingSystem = "" Or InStr(MyDominoServer.OperatingSystem, "*ERROR*" > 0) Then
                        MyDominoServer.OperatingSystem = "Unknown"
                    End If

                    If InStr(MyDominoServer.OperatingSystem, "failed on server") > 0 Then
                        MyDominoServer.OperatingSystem = "Unknown"
                    End If

                End If
                '      MyDominoServer.OperatingSystem = DominoTextStatistic(MyDominoServer.Name, "Server", "Version.OS")

                '  WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & MyDominoServer.Name & " is running on " & MyDominoServer.OperatingSystem)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error getting server OS :  " & ex.Message)
                '  WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server Stats: " & vbCrLf & MyDominoServer.Statistics_Server)
            End Try


            Try

                If MyDominoServer.VersionDomino = "Unknown" Or MyDominoServer.VersionDomino.Trim = "" Then

                    If InStr(MyDominoServer.Statistics_Server, "Version.Notes") Then
                        MyDominoServer.VersionDomino = ParseTextStatValue("Server.Version.Notes", MyDominoServer.Statistics_Server)
                    End If

                    ' MyDominoServer.VersionDomino = DominoTextStatistic(MyDominoServer.Name, "Server", "Version.Notes")
                End If
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & MyDominoServer.Name & " is running Domino " & MyDominoServer.VersionDomino)


                If InStr(MyDominoServer.VersionDomino, "*ERROR*") > 0 Then
                    MyDominoServer.VersionDomino = "Unknown"
                End If

                If InStr(MyDominoServer.VersionDomino, "failed on server") > 0 Then
                    MyDominoServer.VersionDomino = "Unknown"
                End If

                If MyDominoServer.VersionDomino = "" Then
                    MyDominoServer.VersionDomino = "Unknown"
                End If

            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error getting Domino version :  " & ex.Message)
            End Try


            If MyDominoServer.VersionDomino <> "Unknown" Then
                Dim strSQL As String = ""

                Try

                    With MyDominoServer
                        strSQL = "Update Status SET DominoVersion='" & .VersionDomino & "', OperatingSystem='" & Left(.OperatingSystem, 100) & "'" & _
                                 "  WHERE TypeANDName='" & .Name & "-Domino'"
                        Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                        Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.TypeAndName, .Name & "-Domino")
                        Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                             .Set(Function(x) x.SoftwareVersion, .VersionDomino) _
                                                                                             .Set(Function(x) x.OperatingSystem, Left(.OperatingSystem, 100))
                        repository.Update(filterDef, updateDef)
                    End With

                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error updating Domino software and OS version :  " & ex.Message)
                End Try
            End If
        End If

        If ResponseTime <> 0 Then
            Try
                CheckDominoServerTasks(MyDominoServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #10A: " & ex.ToString)
            End Try
        End If

        If ResponseTime <> 0 Then
            Try
                'Check Dead and Pending Mail
                Call GetDeadandPendingMailWrapper(MyDominoServer)

            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error getting dead and pending mail:  " & ex.Message)
            End Try

            Try
                Call CompareMailThresholds(MyDominoServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error comparing mail thresholds  " & ex.Message)
            End Try
        End If



        Try
              If ResponseTime <> 0 Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " About to query disk space")
                CheckDominoDiskSpace(MyDominoServer)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #10: " & ex.ToString)
        End Try

        'Availability / AvailabilityIndex
        Try
            If MyDominoServer.Statistics_Server <> "" Then
                MyDominoServer.AvailabilityIndex = ParseNumericStatValue("Server.AvailabilityIndex", MyDominoServer.Statistics_Server)
                If MyDominoServer.AvailabilityIndex = Nothing Then
                    MyDominoServer.AvailabilityIndex = GetDominoNumericStatistic(MyDominoServer.Name, "Server", "AvailabilityIndex")
                End If
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & MyDominoServer.Name & " Availability Index is " & MyDominoServer.AvailabilityIndex)
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error getting server availability index:  " & ex.Message)
        End Try

        Try
            If MyDominoServer.Statistics_Server <> "" Then
                MyDominoServer.AvailabilityThreshold = ParseNumericStatValue("Server.AvailabilityThreshold", MyDominoServer.Statistics_Server)
                If MyDominoServer.AvailabilityThreshold = Nothing Then
                    MyDominoServer.AvailabilityThreshold = GetDominoNumericStatistic(MyDominoServer.Name, "Server", "AvailabilityThreshold")
                End If
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & MyDominoServer.Name & " Availability Threshold is " & MyDominoServer.AvailabilityThreshold)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error getting server availability threshold:  " & ex.Message)
        End Try

        Try
            If MyDominoServer.Statistics_Server <> "" Then
                MyDominoServer.VersionArchitecture = ParseTextStatValue("Server.Version.Architecture", MyDominoServer.Statistics_Server)
                If MyDominoServer.VersionArchitecture = Nothing Then
                    MyDominoServer.VersionArchitecture = GetDominoTextStatistic(MyDominoServer.Name, "Server", "Version.Architecture")
                End If
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & MyDominoServer.Name & " Version.Architecture is " & MyDominoServer.VersionArchitecture)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error getting server elapsed time:  " & ex.Message)
        End Try

        Try
            If MyDominoServer.Statistics_Server <> "" Then
                MyDominoServer.CPUCount = ParseNumericStatValue("Server.CPU.Count", MyDominoServer.Statistics_Server)
                If MyDominoServer.CPUCount = Nothing Then
                    MyDominoServer.CPUCount = GetDominoNumericStatistic(MyDominoServer.Name, "Server", "CPU.Count")
                End If
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & MyDominoServer.Name & " CPU Count is " & MyDominoServer.CPUCount)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error getting server elapsed time:  " & ex.Message)
        End Try

        Try
            If MyDominoServer.Statistics_Server <> "" Then
                If MyDominoServer.ElapsedTime = Nothing Then
                    MyDominoServer.ElapsedTime = 0
                End If
                If MyDominoServer.VersionArchitecture = Nothing Then
                    MyDominoServer.VersionArchitecture = ""
                End If
                If MyDominoServer.CPUCount = Nothing Then
                    MyDominoServer.CPUCount = 0
                End If
                UpdateDominoServerDetailsTable(MyDominoServer)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error trying to update DominoServerDetails:  " & ex.Message)
        End Try

        Try
            If ResponseTime <> 0 Then
                CheckDomino_DominoStats(MyDominoServer)
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Unhandled Exception in QueryDominoServer thread at CheckDomino_DominoStats: " & ex.ToString)
        End Try


        If ResponseTime <> 0 Then
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " QueryDominoServer cycle at I")

            'CPU Utilization
            Try
                If MyDominoServer.Statistics_Platform <> "" Then
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " QueryDominoServer is about to check CPU")
                    Dim myCPU As Double
                    Dim myCPUString As String
                    myCPUString = ParseTextStatValue("Platform.System.PctCombinedCpuUtil", MyDominoServer.Statistics_Platform)
                    '   myCPU = ParseNumericStatValue("Platform.System.PctCombinedCpuUtil", MyDominoServer.Statistics_Platform)

                    Try
                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Parsed out the CPU as " & myCPUString)
                    Catch exCPU As Exception
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error parsing out the CPU:  " & exCPU.ToString)
                    End Try

                    Try
                        myCPUString = myCPUString.Replace(",", ".")
                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Changed the CPU to " & myCPUString)

                    Catch ex As Exception

                    End Try
                    '**************  New code added 4/2/2011 to parse cpu percent in Europe
                    Dim USprovider As IFormatProvider = CultureInfo.CreateSpecificCulture(sCultureString)
                    Dim Europrovider As IFormatProvider = CultureInfo.CreateSpecificCulture("fr-FR")

                    Try
                        Double.TryParse(myCPUString, NumberStyles.Any Or NumberStyles.AllowDecimalPoint, USprovider, myCPU)
                    Catch ex As Exception
                        myCPU = Nothing
                    End Try

                    Try
                        If myCPU = Nothing Or myCPU > 100 Or myCPU = 0 Then
                            Double.TryParse(myCPUString, NumberStyles.Any Or NumberStyles.AllowDecimalPoint, Europrovider, myCPU)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        '  myCPU = CType(myCPUString, Double)
                        MyDominoServer.CPU_Utilization = myCPU / 100
                    Catch ex As Exception
                        MyDominoServer.CPU_Utilization = 0
                    End Try

                    Try
                        UpdateDominoDailyStatTable(MyDominoServer, "Platform.System.PctCombinedCpuUtil", myCPU)
                    Catch ex As Exception

                    End Try

                    Try
                        If myCPU > MyDominoServer.CPU_Threshold And MyDominoServer.CPU_Threshold > 0 Then
                            MyDominoServer.Status = "Insufficient CPU"
                            MyDominoServer.ResponseDetails += "The CPU utilization on this server exceeds the alert threshold of " & MyDominoServer.CPU_Threshold & "%. " & vbCrLf
                            MyDominoServer.Description = "The CPU utilization on this server exceeds the alert threshold of " & MyDominoServer.CPU_Threshold & "%. "
                            myAlert.QueueAlert("Domino", MyDominoServer.Name, "CPU", "The CPU utilization on this server exceeds the alert threshold of " & MyDominoServer.CPU_Threshold & "%.", MyDominoServer.Location)
                        Else
                            myAlert.ResetAlert("Domino", MyDominoServer.Name, "CPU", MyDominoServer.Location, "The CPU is at " & MyDominoServer.CPU_Utilization & "%")
                        End If
                    Catch ex As Exception

                    End Try

                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error in CPU module:  " & ex.ToString)
            End Try

        End If

        If ResponseTime <> 0 Then
            'log file

            Try
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & "CheckDominoLogFile  ")
                CheckDominoLogFile(MyDominoServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error checking log file:  " & ex.Message)
            End Try

            Try
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & "CheckDominoAgentLogFile  ")
                CheckDominoAgentLogFile(MyDominoServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error checking agent log file:  " & ex.Message)
            End Try
        End If


        'If InStr(MyDominoServer.Name, "Eureka8/Servers/Roto") Then
        '    MyDominoServer.Traveler_Server = False
        'End If

        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " MyDominoServer.ScanTravelerServer:  " & MyDominoServer.ScanTravelerServer.ToString())
        If ResponseTime <> 0 And (MyDominoServer.Traveler_Server = True And MyDominoServer.ScanTravelerServer = True) Then
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " QueryDominoServer is checking Traveler")

            '*********** Lotus Traveler Monitoring


            Try
                'Find out if the server is RED or Yellow or GREEN
                CheckTravelerState(MyDominoServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Unhandled Error checking Lotus Traveler State:  " & ex.Message)
            End Try

            Try
                'If the server is RED or Yellow you'll have the reasons.
                CheckTravelerStatusReasons(MyDominoServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Unhandled Error checking Lotus Traveler Status Reasons:  " & ex.Message)
            End Try


            Try
                CheckTravelerServer(MyDominoServer)
                UpdateDominoStatusTable(MyDominoServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error in Lotus Traveler module:  " & ex.Message)
            End Try



            Try
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Before calling UpdateTravelerHeartbeat")
                UpdateTravelerHeartbeat(MyDominoServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Unhandled Error checking Lotus Traveler heartbeat:  " & ex.Message)
            End Try

            Try
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Before calling CheckTravelerMailServerAccess")
                CheckTravelerMailServerAccess(MyDominoServer)
            Catch ex As Exception

            End Try

            Try
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Before calling CheckTravelerBackend")
                CheckTravelerBackend(MyDominoServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Unhandled Error checking Notes Traveler data store:  " & ex.Message)
            End Try

            Try
                CheckTravelerHA(MyDominoServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Unhandled Error checking Lotus Traveler servlet:  " & ex.Message)
            End Try

            Try
                CheckTravelerServlet(MyDominoServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Unhandled Error checking Lotus Traveler servlet:  " & ex.Message)
            End Try


        End If


        If ResponseTime <> 0 Then
            'Cluster stats
            Try
                If MyDominoServer.ClusterMember = "" Or InStr(MyDominoServer.ClusterMember, "ERROR") > 0 Then
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " QueryDominoServer is about to check cluster stats...")
                    WriteAuditEntry(Now.ToString & " Checking to see if " & MyDominoServer.Name & " is a member of cluster.", LogLevel.Verbose)
                    MyDominoServer.ClusterMember = ParseTextStatValue("Cluster.Name", MyDominoServer.Statistics_Server)
                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.Today & " " & MyDominoServer.Name & " value of Cluster.Member is " & MyDominoServer.ClusterMember)
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #10: " & ex.ToString)
            End Try


            Try
                MyDominoServer.Statistics_Replica = ""
                MyDominoServer.Statistics_Replica = GetStats(MyDominoServer.Name, "replica", "")
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #11: " & ex.ToString)
                MyDominoServer.Statistics_Replica = ""
            End Try

            Try
                If MyDominoServer.Statistics_Replica <> "" Then
                    CheckClusterMemberAvailability(MyDominoServer)
                    UpdateClusterHealth(MyDominoServer)
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #12: " & ex.ToString)
            End Try

        End If



        Try
            If ResponseTime <> 0 Then
                TrackDominoMailStatistics(MyDominoServer)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #13: " & ex.ToString)
        End Try



        Try
            If ResponseTime <> 0 Then
                UpdateDominoStats(MyDominoServer)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #13: " & ex.ToString)
        End Try

        If ResponseTime <> 0 Then
            'custom stats		
            Try
                If MyDominoServer.CustomStatisticsSettings.Count > 0 Then
                    CheckDominoCustomStatistics(MyDominoServer)
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at CheckDominoCustomStatistics: " & ex.ToString)
            End Try

        End If

        '2/22/2016 NS added for VSPLUS-2641
        If ResponseTime <> 0 Then
            Try
                CheckDominoAvailabilityIndex(MyDominoServer)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at CheckDominoAvailabilityIndexThreshold: " & ex.ToString)
            End Try
        End If



        'Begin code if Server is DOWN
        If MyDominoServer.ResponseTime = 0 Then
            Try
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Warning: " & MyDominoServer.Name & " is down.  Consecutive Failure count=" & MyDominoServer.FailureCount)
                MyDominoServer.Status = "Not Responding"
                MyDominoServer.LastScan = Now
                MyDominoServer.ResponseDetails = "Notes error: The server is not responding. The server may be down or you may be experiencing network problems."
                MyDominoServer.Description = "Notes error: The server is not responding. The server may be down or you may be experiencing network problems."

            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #14: " & ex.ToString)
            End Try
        End If



        If MyDominoServer.ResponseTime = 0 Then
            Try
                MyDominoServer.IncrementDownCount()
                MyDominoServer.AlertCondition = True
                '  If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Consecutive Failure count=" & MyDominoServer.FailureCount)
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server up time is " & Microsoft.VisualBasic.Strings.Format(MyDominoServer.UpMinutes, "F1") & " minutes.")
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server down time is " & Microsoft.VisualBasic.Strings.Format(MyDominoServer.DownMinutes, "F1") & " minutes.")

            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #14: " & ex.ToString)
            End Try

            MyDominoServer.IsUp = False
            Try
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Consecutive Failure count=" & MyDominoServer.FailureCount)
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Failure Threshold =" & MyDominoServer.FailureThreshold)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #15: " & ex.ToString)
            End Try
        End If

        If MyDominoServer.ResponseTime = 0 Then
            Try
                If MyDominoServer.FailureCount >= MyDominoServer.FailureThreshold Then
                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Alert: " & MyDominoServer.Name & " is down.")
                    'Server has failed before, so time to send out some alerts
                    ' myAlert.QueueAlert("Domino Server", MyDominoServer.Name, "Not Responding", "The Domino server " & MyDominoServer.Name & " is not responding.  This was first detected " & Now.ToShortDateString & "  " & Now.ToShortTimeString)
                    '3/4/2016 NS modified for VSPLUS-2682
                    myAlert.QueueAlert("Domino", MyDominoServer.Name, "Not Responding", "The server " & MyDominoServer.Name & " is not responding.  " & MyDominoServer.Description & vbCrLf & vbCrLf & " This was first detected " & Now.ToShortDateString & "  " & Now.ToShortTimeString, MyDominoServer.Location)

                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #16: " & ex.ToString)
            End Try


        End If

        Try
            UpdateDominoStatusTable(MyDominoServer)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #17: " & ex.ToString)
        End Try



        Try
            If MyDominoServer.Statistics_Mail <> "" And MyDominoServer.Status <> "Not Responding" And MyDominoServer.Status <> "Network Issue" Then
                UpdateMailHealth(MyDominoServer)
            End If

        Catch ex As Exception

        End Try

        Try
            dtDominoLastUpdate = Now
            oWatch.Stop()
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " QueryDominoServer has finished a cycle.  " & "  Elapsed Time= " & oWatch.Elapsed.TotalSeconds & " seconds. " & vbCrLf & vbCrLf)

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #19: " & ex.ToString)
        End Try


        Try
            ResponseTime = Nothing
            myUsers = Nothing
            myMemory = Nothing
            MyStatValue = Nothing
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception in QueryDominoServer thread at #18: " & ex.ToString)
        End Try


        Try
            GC.Collect()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub JustPingDominoServer(ByRef MyDominoServer As MonitoredItems.DominoServer)
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, vbCrLf & Now.ToString & " --------------------------------")
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Starting new JustPing cycle.")
        Dim previousStatus As String = MyDominoServer.Status


        ' MyDominoServer.LastPing = Now
        MyDominoServer.PingCount += 1
        Dim oWatch As New System.Diagnostics.Stopwatch
        oWatch.Start()

        Try
            If Trim(MyDominoServer.Status) = "Not Responding" Or Trim(MyDominoServer.Status) = "Network Issue" Then
                MyDominoServer.ResponseDetails = "Not Responding - New scan started at " & Date.Now.ToShortTimeString
                '	MyDominoServer.Description = "Not Responding - New scan started at " & Date.Now.ToShortTimeString
            ElseIf Trim(MyDominoServer.Status) = "Telnet" Then
                MyDominoServer.ResponseDetails = "PING scan cycle started at " & Date.Now.ToShortTimeString
            Else
                MyDominoServer.ResponseDetails = "PING scan cycle started at " & Date.Now.ToShortTimeString
                'MyDominoServer.Description = "PING scan cycle started at " & Date.Now.ToShortTimeString
            End If

            If MyDominoServer.Status = "Not Scanned" Then
                MyDominoServer.Status = "Scanning"
            End If

            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " #1 The server status is " & MyDominoServer.Status, LogLevel.Verbose)


            UpdateDominoStatusTable(MyDominoServer)
            ' MyDominoServer.Description = "Successfully connected to the server via ping scan at " & Date.Now.ToShortTimeString
            MyDominoServer.ResponseDetails = "Successfully connected via Notes Client ping scan at " & Date.Now.ToShortTimeString
            'MyDominoServer.Description = "The server answered a Notes client ping at " & Now.ToShortTimeString
        Catch ex As Exception

        End Try

        Dim ResponseTime As Double = 0
        Dim myUsers As Long

        Dim myMemory As String
        Dim MyStatValue As Double
        dtDominoLastUpdate = Now

        Try
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino thread is attempting PING SCAN of  " & MyDominoServer.Name & ". This server has " & MyDominoServer.FailureCount & " consecutive failures.", LogLevel.Verbose)
            '   If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & MyDominoServer.Name & " was scheduled for " & MyDominoServer.NextScan & " will next be scanned at " & MyDominoServer.NextScan)
        Catch ex As Exception

        End Try

        Try
            ' MyDominoServer.AlertCondition = False
            MyDominoServer.ResponseTime = 0
        Catch ex As Exception

        End Try


        Try
            If ResponseTime = 0 Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Testing server response time via Notes API function.")
                ResponseTime = DominoResponseTime(MyDominoServer.Name)
                MyDominoServer.ResponseTime = ResponseTime
            End If
        Catch ex As Exception
            MyDominoServer.ResponseTime = 0
            ResponseTime = 0
        End Try

        If ResponseTime > 0 And Trim(MyDominoServer.Status) = "Telnet" Then
            MyDominoServer.Status = "OK"
        End If

        Try
            If ResponseTime = 0 Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server did not respond via Notes API function.  Now testing server response time via stats call.")
                ResponseTime = DominoResponseTimeByStatCall(MyDominoServer.Name)
                MyDominoServer.ResponseTime = ResponseTime
            End If
        Catch ex As Exception
            ResponseTime = 0
        End Try


        'Try
        '	If ResponseTime = 0 Then
        '		WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server did not respond via Notes API function or Server Stats call.  Now testing server response time via COM.")
        '		ResponseTime = DominoResponseTimeAlternate(NotesSession, MyDominoServer.Name)
        '		MyDominoServer.ResponseTime = ResponseTime
        '	End If
        'Catch ex As Exception
        '	ResponseTime = 0
        'End Try

        Try
            ' Added the check to reset and reset the alert only if the prior Telnet value was > 0 to avoid un-necessary call to do a reset alert. 
            If ResponseTime > 0 And MyDominoServer.ConsecutiveTelnetCount > 0 Then
                MyDominoServer.ConsecutiveTelnetCount = 0
                myAlert.ResetAlert("Domino", MyDominoServer.Name, "Telnet", MyDominoServer.Location)
            End If

        Catch ex As Exception

        End Try

        Dim TelnetSuccess As Boolean = False
        Try
            If ResponseTime = 0 Then
                ' WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Evaluating whether to test server via telnet function.")
                If MyDominoServer.IPAddress <> "" Then
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Testing server response time via telnet function to " & MyDominoServer.IPAddress)
                    TelnetSuccess = TelnetDomino(MyDominoServer)
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Telnet response was " & TelnetSuccess.ToString)
                Else
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server does not have an IP address I can use for the telnet function.")
                End If

                If TelnetSuccess = True Then
                    'MyDominoServer.ResponseTime = 15
                    '	MyDominoServer.Description = "Connected at " & Now.ToShortTimeString
                    'MyDominoServer.Status = "OK"
                    MyDominoServer.ResponseDetails = "Connected to the server via telnet on port 1352 at " & Now.ToShortTimeString
                    myAlert.ResetAlert("Domino", MyDominoServer.Name, "Not Responding", MyDominoServer.Location, "Connected to the server via telnet on port 1352")
                    MyDominoServer.IsUp = True
                    ' MyDominoServer.LastScan = Now
                    MyDominoServer.IncrementUpCount()
                    MyDominoServer.ConsecutiveTelnetCount += 1
                    If MyDominoServer.ConsecutiveTelnetCount > GetConsecutiveTelnetValue() Then
                        MyDominoServer.Status = "Telnet"
                        MyDominoServer.ResponseDetails = "The server is answering, but only to telnet on port 1352"
                        myAlert.QueueAlert("Domino", MyDominoServer.Name, "Telnet", "The server is answering, but only to telnet on port 1352.  This could indicate a problem.", MyDominoServer.Location)
                        '1/26/2016 NS modified for VSPLUS-2209
                    Else
                        MyDominoServer.Status = "OK"
                    End If
                    UpdateDominoStatusTable(MyDominoServer)
                    'Put the response time in the statistics.mdb 'DeviceDailyStat' table
                    UpdateDominoResponseTimeTable(MyDominoServer, MyDominoServer.ResponseTime)
                    Exit Sub
                End If
            End If


        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception using Telnet function: " & ex.ToString)
        End Try


        Try
            If ResponseTime = 0 Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server did not respond on the first attempt.  It may be down....", LogLevel.Verbose)
            End If
        Catch ex As Exception

        End Try

        If ResponseTime <> 0 Then
            Try
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Prior status was " & previousStatus, LogLevel.Verbose)
                'MyDominoServer.LastScan = Now
                If Trim(previousStatus) = "Scanning" Then
                    MyDominoServer.Status = "OK"
                End If
                If Trim(previousStatus) = "Not Responding" Then
                    MyDominoServer.Status = "OK"
                End If

                If Trim(previousStatus) = "Not Scanned" Then
                    MyDominoServer.Status = "OK"
                End If

                If Trim(previousStatus) = "Network Issue" Then
                    MyDominoServer.Status = "OK"
                End If

                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Updated status is " & MyDominoServer.Status, LogLevel.Verbose)

                'Server IS responding
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server responded in " & CType(ResponseTime, String) & " ms.", LogLevel.Verbose)
                MyDominoServer.IncrementUpCount()
                ' MyDominoServer.Status = "OK"
                MyDominoServer.ResponseTime = ResponseTime
                MyDominoServer.Description = "Successfully connected to the server via Just Ping scan at " & Now.ToShortTimeString
                ' JT 2/16 Added a condition to reset only if the Prior Statu was Not Responding to minimize the un-necessary call. 
                If Trim(previousStatus) = "Not Responding" Then
                    myAlert.ResetAlert("Domino", MyDominoServer.Name, "Not Responding", MyDominoServer.Location)
                End If



                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server up time is " & Microsoft.VisualBasic.Strings.Format(MyDominoServer.UpMinutes, "F1") & " minutes.", LogLevel.Verbose)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server down time is " & Microsoft.VisualBasic.Strings.Format(MyDominoServer.DownMinutes, "F1") & " minutes.", LogLevel.Verbose)
                UpdateDominoStatusTable(MyDominoServer)
                MyDominoServer.IsUp = True
            Catch ex As Exception

            End Try
        End If



        If ResponseTime <> 0 Then
            '******************************
            '  Performance Targets
            '******************************

            Try
                'Check if response is within Target
                With MyDominoServer
                    Try
                        If .ResponseTime <= .ResponseThreshold Then
                            'On Time
                            .IncrementOnTargetCount()
                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino server " & MyDominoServer.Name & " is within performance target.  Server responded in " & MyDominoServer.ResponseTime & " ms, and target is " & MyDominoServer.ResponseThreshold & " ms.", LogLevel.Verbose)

                            If .OffHours = False Then
                                .IncrementBusinessHoursOnTargetCount()
                            End If
                        Else
                            'Slow
                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino server " & MyDominoServer.Name & " is slow.  Server responded in " & MyDominoServer.ResponseTime & " ms but target is " & MyDominoServer.ResponseThreshold & " ms.")
                            .Status = "Slow"
                            .ResponseDetails = "Server responded in " & MyDominoServer.ResponseTime & " ms but target is " & MyDominoServer.ResponseThreshold & " ms."
                            .Description = "Server responded in " & MyDominoServer.ResponseTime & " ms but target is " & MyDominoServer.ResponseThreshold & " ms."

                            .IncrementOffTargetCount()
                            If .OffHours = False Then
                                .IncrementBusinessHoursOffTargetCount()
                            End If
                            '%%%%%%%%%%%%%
                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & MyDominoServer.Name & " is slow, alert status is " & MyDominoServer.AlertCondition, LogLevel.Verbose)

                        End If
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception checking Domino Performance Targets: " & ex.Message)
                    End Try
                End With

                Try
                    'Put the response time in the statistics.mdb 'DeviceDailyStat' table
                    UpdateDominoResponseTimeTable(MyDominoServer, ResponseTime)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error updating Domino Response Table " & ex.Message)

                End Try

            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error in Performance Tracking  block: " & ex.Message)

            End Try
        End If

        'Begin code if Server is DOWN
        If MyDominoServer.ResponseTime = 0 Then
            Try
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Warning: According to the JustPingDominoServer module, " & MyDominoServer.Name & " is down.  Consecutive Failure count=" & MyDominoServer.FailureCount, LogLevel.Verbose)
                MyDominoServer.Status = "Not Responding"
                MyDominoServer.LastScan = Now
                ' MyDominoServer.LastPing = Now

                MyDominoServer.ResponseDetails = "Notes error: The server is not responding. The server may be down or you may be experiencing network problems."
                MyDominoServer.Description = "Pinged at " & Now.ToShortTimeString & ".  The server is not responding. The server may be down or you may be experiencing network problems."

            Catch ex As Exception

            End Try
        End If

        If MyDominoServer.ResponseTime = 0 Then
            Try
                MyDominoServer.IncrementDownCount()
                MyDominoServer.AlertCondition = True
                '  If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Consecutive Failure count=" & MyDominoServer.FailureCount)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server up time is " & Microsoft.VisualBasic.Strings.Format(MyDominoServer.UpMinutes, "F1") & " minutes.", LogLevel.Verbose)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server down time is " & Microsoft.VisualBasic.Strings.Format(MyDominoServer.DownMinutes, "F1") & " minutes.", LogLevel.Verbose)

            Catch ex As Exception

            End Try

            MyDominoServer.IsUp = False
            Try
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Consecutive Failure count=" & MyDominoServer.FailureCount, LogLevel.Verbose)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Failure Threshold =" & MyDominoServer.FailureThreshold, LogLevel.Verbose)
            Catch ex As Exception

            End Try
        End If

        If MyDominoServer.ResponseTime = 0 Then

            Try
                If MyDominoServer.FailureCount >= MyDominoServer.FailureThreshold Then
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Alert: " & MyDominoServer.Name & " is down.", LogLevel.Verbose)
                    'Server has failed before, so time to send out some alerts
                    '3/4/2016 NS modified for VSPLUS-2682
                    myAlert.QueueAlert("Domino", MyDominoServer.Name, "Not Responding", "The server " & MyDominoServer.Name & " is not responding.  " & MyDominoServer.Description & vbCrLf & vbCrLf & " This was first detected " & Now.ToShortDateString & "  " & Now.ToShortTimeString, MyDominoServer.Location)

                End If
            Catch ex As Exception

            End Try

        End If

        Try
            UpdateDominoStatusTable(MyDominoServer)
        Catch ex As Exception

        End Try

        Try
            ResponseTime = Nothing
            myUsers = Nothing
            myMemory = Nothing
            MyStatValue = Nothing
        Catch ex As Exception

        End Try

        Try
            dtDominoLastUpdate = Now
        Catch ex As Exception

        End Try

        Try
            GC.Collect()
        Catch ex As Exception

        End Try

        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " JustPing module has finished a cycle.  " & "  Elapsed Time= " & oWatch.Elapsed.TotalSeconds & " seconds. " & vbCrLf & vbCrLf)

    End Sub

    Private Sub ServerElapsedTime(ByRef MyDominoServer As MonitoredItems.DominoServer)
        'Server Running Days alert
        'This first function calculates the number of days and compares it with the reboot threshold, if any
        Try
            'exit the try if the threshold is not set
            '3/22/2016 NS commented out for VSPLUS-2736 - the condition was preventing ElapsedDays from being collected
            'If MyDominoServer.ServerDaysAlert < 2 Then Exit Try '1 or zero are not valid values
            Dim ElapsedDays As Integer
            Dim myElapsedString As String
            If InStr(MyDominoServer.Statistics_Server, "Elapsed") > 0 Then
                myElapsedString = ParseTextStatValue("Server.ElapsedTime", MyDominoServer.Statistics_Server)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The server is reporting an elapsed time of: " & myElapsedString, LogUtilities.LogUtils.LogLevel.Verbose)
                Dim dIndex = myElapsedString.IndexOf(" ")
                ElapsedDays = myElapsedString.Substring(0, dIndex + 1)
                If MyDominoServer.ServerDaysAlert > 0 Then
                    If (ElapsedDays > MyDominoServer.ServerDaysAlert) Then
                        myAlert.QueueAlert("Domino", MyDominoServer.Name, "Reboot Overdue", "Server has been running for " & ElapsedDays & " Days and is longer than the threshold of " & MyDominoServer.ServerDaysAlert & " Days", MyDominoServer.Location, "Policy")
                    Else
                        myAlert.ResetAlert("Domino", MyDominoServer.Name, "Reboot Overdue", MyDominoServer.Location, "Server has been running for " & ElapsedDays, "Policy")
                    End If
                End If
                Dim objVSAdaptor As New VSAdaptor
                Dim strSQL_Short As String = ""
                With MyDominoServer
                    strSQL_Short = "Update Status SET ElapsedDays = " & ElapsedDays & " WHERE TypeANDName='" & .Name & "-Domino'"
                End With
                objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL_Short)
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception getting elapsed time" & ex.ToString, LogLevel.Verbose)
        End Try

        '10/9/2015 NS added for VSPLUS-2252
        'This second function calculates in more detail, also including hours and minutes, for display on the Health Assessment Tab
        Dim elapsedTime As String
        Try
            If MyDominoServer.Statistics_Server <> "" Then
                elapsedTime = ParseTextStatValue("Server.ElapsedTime", MyDominoServer.Statistics_Server)
                MyDominoServer.ElapsedTime = ParseTotalSecondsFromStat(elapsedTime)
                If MyDominoServer.ElapsedTime = Nothing Then
                    elapsedTime = GetDominoTextStatistic(MyDominoServer.Name, "Server", "ElapsedTime")
                    MyDominoServer.ElapsedTime = ParseTotalSecondsFromStat(elapsedTime)
                End If
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & MyDominoServer.Name & " Elapsed Time is " & MyDominoServer.ElapsedTime)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error getting server elapsed time:  " & ex.Message)
        End Try
    End Sub

    Private Function TelnetDomino(ByRef Server As MonitoredItems.DominoServer) As Boolean
        Dim oWatch As New System.Diagnostics.Stopwatch
        oWatch.Start()

        Dim remoteIPAddress As Net.IPAddress
        Dim ep As Net.IPEndPoint
        Dim tnSocket As Net.Sockets.Socket
        ' Get the IP Address and the Port and create an IPEndpoint (ep)
        WriteDeviceHistoryEntry("Domino", Server.Name, Now.ToString & " The server's IP is " & Server.IPAddress)

        Try
            remoteIPAddress = Net.IPAddress.Parse(Server.IPAddress.Trim)
        Catch ex As FormatException
            Try
                Dim host As Net.IPHostEntry
                Dim hostname As String
                hostname = Server.IPAddress.Trim
                host = Net.Dns.GetHostEntry(hostname)
                For Each ip As Net.IPAddress In host.AddressList
                    If ip.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork Then
                        remoteIPAddress = Net.IPAddress.Parse(ip.ToString)
                    End If
                Next
            Catch ex2 As Exception

            End Try

        End Try


        Try
            WriteDeviceHistoryEntry("Domino", Server.Name, Now.ToString & " The server's IP was parsed to  " & remoteIPAddress.ToString)
            ep = New Net.IPEndPoint(remoteIPAddress, 1352)

            ' Set the socket up (type etc)
            tnSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

        Catch ex As Exception

        End Try

        ' Convert the ASCII command into bytes, adding a line termination on (vbCrLf)
        ' Dim SendBytes As [Byte]() = Text.Encoding.ASCII.GetBytes(Command() & vbCrLf)

        ' Create a byte array for receiving bytes from the telnet socket
        Dim RecvBytes(255) As [Byte]

        Try
            ' Connect
            tnSocket.Connect(ep)
        Catch oEX As SocketException
            ' error
            ' You will need to do error cleanup here e.g killing the socket
            ' and exiting the procedure.
            remoteIPAddress = Nothing
            ep = Nothing
            tnSocket = Nothing
            Return False
        End Try

        ' If we get to here then all seems good (we are connected)
        Try
            ' Wait a few seconds (5) (depending on connection) telnet can be slow.
            Thread.Sleep(5000)

            ' Double check we are connected
            If tnSocket.Connected Then
                ' Send the command
                WriteDeviceHistoryEntry("Domino", Server.Name, Now.ToString & " Connected on port 1352 to " & remoteIPAddress.ToString)
                Try
                    Server.ResponseTime = oWatch.Elapsed.TotalMilliseconds
                    WriteDeviceHistoryEntry("Domino", Server.Name, Now.ToString & " Telnet response time was  " & oWatch.Elapsed.TotalMilliseconds & " ms")
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", Server.Name, Now.ToString & " Error calculating Telnet response time:  " & ex.ToString)
                End Try
                ' Disconnect
                tnSocket.Disconnect(False)
                Return True
            Else
                WriteDeviceHistoryEntry("Domino", Server.Name, Now.ToString & " Unable to connect on port 1352 to " & Server.IPAddress)
            End If
        Catch oEX As Exception
            ' Error cleanup etc needed
        End Try

        ' Cleanup
        remoteIPAddress = Nothing
        ep = Nothing
        tnSocket = Nothing
        oWatch.Stop()



    End Function


    Private Function DominoResponseTime(ByVal Server As String) As Double
        'This Function returns response time in milliseconds
        WriteDeviceHistoryEntry("Domino", Server, Now.ToString & " Attempting to contact Domino server: " & Server & " to measure response time.")
        dtDominoLastUpdate = Now
        Dim ReturnValue As Double = 0
        Dim strResponseTime As String = ""
        Dim n As Integer
        Dim myName As String = ""
        Dim retryTime As Double

        Dim UpperBound As Integer = 3

        Try
            If InStr(Server, "HSBC") > 0 Then
                UpperBound = 5
            End If
        Catch ex As Exception
            UpperBound = 15
        End Try

        ' this is a customization for TSYS
        Try
            If InStr(Server, "TotalSystem") > 0 Then
                UpperBound = 5
            End If
        Catch ex As Exception
            UpperBound = 15
        End Try

        'This is a customization for Interface

        Try
            If Server = "Bangalore/InterfaceEurope/FC/Interface" Then
                UpperBound = 10
            End If
        Catch ex As Exception
            UpperBound = 50
        End Try

        Try
            If Server = "Bangkok/InterfaceAsiaPacific/FC/Interface" Then
                UpperBound = 10
            End If
        Catch ex As Exception
            UpperBound = 50
        End Try

        Try
            If Server = "HongKong/InterfaceEurope/FC/Interface" Then
                UpperBound = 10
            End If
        Catch ex As Exception
            UpperBound = 50
        End Try

        'Joe - forcing to try only for 5 times to limit the # of sockets, as the max is 4000. 
        UpperBound = 5
        Dim oWatch As New System.Diagnostics.Stopwatch
        oWatch.Start()
        For n = 1 To UpperBound

            Try
                '   If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
                strResponseTime = GetResponseTime(Server)
                ReturnValue = Math.Round(CDbl(strResponseTime))
            Catch ex As Exception
                ReturnValue = 0
                ' WriteDeviceHistoryEntry("Domino", Server, Now.ToString & " Exception from C API " & ex.ToString)
            Finally
                '    If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
            End Try

            If ReturnValue > 0 Then
                Return ReturnValue
            End If

            retryTime = 3500

            Thread.Sleep(retryTime)
            WriteDeviceHistoryEntry("Domino", Server, Now.ToString & " No response from server, attempt #" & n.ToString & ". Waiting " & retryTime & " ms then trying again.")
        Next
        oWatch.Stop()
        n = n - 1
        WriteDeviceHistoryEntry("Domino", Server, Now.ToString & " No response from server, after " & n.ToString & " attempts in " & oWatch.Elapsed.Seconds & " seconds.")
        Return ReturnValue
    End Function

    Function DominoResponseTimeByStatCall(ByVal server As String) As Double
        Dim oWatch As New System.Diagnostics.Stopwatch
        Dim ReturnValue As Double = 0
        WriteDeviceHistoryEntry("Domino", server, Now.ToString & " Attempting to get server response time via stat call.")
        oWatch.Start()
        Dim strResult As String = ""
        Try
            ' If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
            strResult = GetStats(server, "Disk", "")
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", server, Now.ToString & " Error getting server response time via stat call: " & ex.ToString)
        Finally
            ' If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
        End Try

        If InStr(strResult, "Disk.") Then
            oWatch.Stop()
            WriteDeviceHistoryEntry("Domino", server, Now.ToString & " Response time is " & oWatch.Elapsed.TotalMilliseconds & " ms")
            ReturnValue = oWatch.Elapsed.TotalMilliseconds
        Else
            ReturnValue = 0
        End If

        Return ReturnValue
    End Function


    Function DominoResponseTimeAlternate(ByRef s As Domino.NotesSession, ByVal Server As String) As Double
        Dim oWatch As New System.Diagnostics.Stopwatch
        Dim dbDir As Domino.NotesDbDirectory
        Dim db As Domino.NotesDatabase

        Dim UpperBound As Integer = 2
        Dim ReturnValue As Double = 0
        Dim retryTime As Double
        Dim n As Integer
        oWatch.Start()

        ' this is a hack for TSYS

        Try
            If InStr(Server, "TotalSystem") > 0 Then
                UpperBound = 5
            End If
        Catch ex As Exception
            UpperBound = 5
        End Try

        For n = 1 To UpperBound
            Try
                dbDir = s.GetDbDirectory(Server)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", Server, Now.ToString & " Exception attempting to connect to dbDir via COM " & ex.ToString)
            End Try

            Try
                If Not dbDir Is Nothing Then
                    db = dbDir.GetFirstDatabase(Domino.DB_TYPES.NOTES_DATABASE)
                    Try
                        If String.IsNullOrWhiteSpace(db.Title) Then
                            db.Title = "Untitled Database"
                        End If
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", Server, Now.ToString & " Error checking if db name is blank.  Error: " & ex.Message)
                    End Try
                    WriteDeviceHistoryEntry("Domino", Server, Now.ToString & " Successfully connected to " & db.Title & " to test for server response time. ")
                    oWatch.Stop()
                    WriteDeviceHistoryEntry("Domino", Server, Now.ToString & " Response time is " & oWatch.Elapsed.TotalMilliseconds & " ms")
                    ReturnValue = oWatch.Elapsed.TotalMilliseconds
                    Exit For
                End If
            Catch ex As Exception
                '  WriteDeviceHistoryEntry("Domino", Server, Now.ToString & " Exception attempting to connect to server via COM " & ex.ToString)
            End Try

            retryTime = 125
            Thread.Sleep(retryTime)
            WriteDeviceHistoryEntry("Domino", Server, Now.ToString & " No response from server using alternate method, attempt #" & n.ToString & ". Waiting " & retryTime & " ms then trying again.")
            ReturnValue = 0
        Next

        If ReturnValue > 0 And ReturnValue < 1 Then
            ReturnValue = 1
        End If


        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(dbDir)

        Catch ex As Exception

        End Try


        Return ReturnValue

    End Function


    Private Sub CheckDominoServerTasks(ByRef DominoServer As MonitoredItems.DominoServer)

        'This function queries the Domino server for the server tasks
        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Checking the status of Domino server tasks. ")
        'if no tasks are configured for monitoring on this server, exit the sub immediately
        If DominoServer.ServerTaskSettings.Count = 0 And DominoServer.Traveler_Server = False Then
            DominoServer.TaskStatus = "No server tasks are being monitored for this server."
            'Continue anyway because we want to list the running tasks on the Dashboard
        End If

        '3/28/2016 NS added for VSPLUS-2669
        Dim timevar As Object
        Dim s As New Domino.NotesSession
        Dim db As Domino.NotesDatabase
        Dim doc As Domino.NotesDocument
        Dim notesDateTime As Domino.NotesDateTime
        Dim tempstr As String
        Dim myPassword = GetNotesPassword()
        Try
            s.Initialize(myPassword)
            db = s.GetDatabase(DominoServer.Name, "log.nsf")
            doc = db.CreateDocument
            timevar = doc.Created
            notesDateTime = s.CreateDateTime(doc.Created.ToString())
            notesDateTime.ConvertToZone(DominoServer.ServerTimeTZ, DominoServer.ServerTimeDST)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(doc)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(s)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error converting server time in CheckDominoServerTasks: " & ex.ToString)
        End Try
        

        DominoServer.TaskStatus = ""
        Dim TaskStatus As String = ""
        Dim strServerTasks As String = ""
        Dim strSHOWTASKS As String

        Try
            'Send a remote console command to get the value of the stat
            ' If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " sending SH STAT Server.Task request to " & DominoServer.Name)
            Dim UpperLimit As Integer = 2

            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Attempting to get server task information.")
            strServerTasks = GetOneStat(DominoServer.Name, "Server", "Task")

            If InStr(strServerTasks, "Server.Task") > 0 Then
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Got server task information... first attempt")
            Else
                For n As Integer = 0 To UpperLimit
                    strServerTasks = GetOneStat(DominoServer.Name, "Server", "Task")
                    If Not (InStr(strServerTasks, "ERROR")) And InStr(strServerTasks, "Server.Task") > 0 Then Exit For
                    Thread.Sleep(500)
                Next
            End If

            'If InStr(DominoServer.Name.ToUpper, "RPR") Then
            '    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "The server tasks are:" & vbCrLf & vbCrLf & strServerTasks & vbCrLf)
            'End If
            ' WriteAuditEntry(Now.ToString & " Server TASK REQUEST returned " & strServerTasks)
            DominoServer.ShowTasks = strServerTasks.Replace(ControlChars.NewLine & ControlChars.NewLine, ControlChars.NewLine)
            ' strServerTasks = s.SendConsoleCommand(DominoServer.Name, "sh stat server.task")
            ' WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " SERVER TASKS request replied with " & vbCrLf & strServerTasks)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " GetDominoServerTasks module Error: " & DominoServer.Name & vbCrLf & ex.Message)
            ' System.Runtime.InteropServices.Marshal.ReleaseComObject(s)
            strServerTasks = Nothing
            strSHOWTASKS = Nothing
            Exit Sub
        End Try

        If Not (InStr(strServerTasks, "=") > 0) Then
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Case #1 SERVER TASKS request does not contain data this time, exiting sub.")
            Exit Sub
        End If

        If DominoServer.Traveler_Server = True Then
            DominoServer.Traveler_Status = "Not Found"
        End If

        If strServerTasks = "" Then
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "Case #2 SERVER TASKS request does not contain data this time, exiting sub.")
            Exit Sub
        End If

        If Not (InStr(strServerTasks, "=") > 0) Then
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Case #3 SERVER TASKS request does not contain data this time, exiting sub.")
            Exit Sub
        End If


        dtDominoLastUpdate = Now
        'Parse the tasks into a collection.  
        Dim MyServerTasks As New MonitoredItems.ServerTasksCollection

        'The tasks in strResult are in the format: 
        ' Server.Task = Directory Indexer: Idle: [08/09/2005 16:38:07 EDT]
        ' Server.Task = SMTP Server: Control task: [08/09/2005 16:38:07 EDT]

        'but some also have a secondary status (Searching for mail to transfer)
        'Server.Task=Router: Transfer: Searching for mail to transfer: [03/11/2011 08:08:20 MST]


        Dim myStart, myEnd As Integer
        Dim thisLine As String = ""
        Dim myLineEnd As Integer
        Try
            Do While InStr(strServerTasks, "=")
                Dim myTask As New MonitoredItems.ServerTask
                myTask.IsMonitored = False
                myTask.StatusSummary = "OK"
                Try
                    'Get the task name, such as 'Directory Indexer'
                    myStart = InStr(strServerTasks, "=") + 1
                    myEnd = InStr(myStart, strServerTasks, ":")
                    myLineEnd = InStr(myStart, strServerTasks, vbCrLf)
                    If InStr(strServerTasks, vbCrLf) Then
                        thisLine = Trim(Mid(strServerTasks, myStart, myLineEnd - myStart))
                    End If

                    myTask.Name = Trim(Mid(strServerTasks, myStart, myEnd - myStart))
                    strServerTasks = Mid(strServerTasks, myEnd)
                    '   WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " GetDominoServerTasks parsed the task name as " & myTask.Name)

                    If InStr(myTask.Name, "Traveler") Then
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " This server is running Traveler, so it MUST BE a Traveler server.")
                        DominoServer.Traveler_Server = True
                        If DominoServer.SecondaryRole.Contains("Traveler") = False Then
                            If DominoServer.SecondaryRole.Trim = "" Then
                                DominoServer.SecondaryRole = "Traveler"
                            Else
                                DominoServer.SecondaryRole += "; Traveler"
                            End If
                        End If
                    End If



                    'Get the task status
                    Try
                        myStart = InStr(thisLine, ":") + 1
                        If InStr(Mid(thisLine, 1, 75), "TCP Port") Then
                            ' Debug.Print(Mid(strServerTasks, 1, 50))
                            myEnd = InStr(myStart + 2, thisLine, "[") - 2
                        Else
                            myEnd = InStr(myStart + 1, thisLine, ":")
                        End If
                        '   myEnd = InStr(myStart + 2, strServerTasks, ":")
                        If myEnd > 0 Then
                            myTask.Status = Trim(Mid(thisLine, myStart, myEnd - myStart))
                        Else
                            myTask.Status = "No Status Provided"
                        End If

                        If InStr(myTask.Status, "Server.Task=") Then
                            'Skip this one
                            '  Debug.Print("Skipping because my status is " & MyStatus)
                        Else
                            If myEnd > 0 Then
                                thisLine = Mid(thisLine, myEnd)
                            End If
                        End If

                        '  WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " GetDominoServerTasks parsed the primary status of " & myTask.Name & " as " & myTask.Status)
                        ' WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " The string I have left to parse is " & vbCrLf & strServerTasks)
                        If InStr(myTask.Status, "Server.Task=") Then
                            myTask.Status = "OK"
                        End If
                    Catch ex As Exception

                    End Try


                    'Server.Task=Lotus Traveler: Running: 4 of 4 users active.  Completed 1,718 device and 1,618 prime syncs.: [03/11/2011 08:32:50 MST]
                    Try
                        'Get the secondary task status if it is available, but not all server tasks have this
                        myStart = InStr(thisLine, ":") + 1
                        myEnd = InStr(myStart + 2, thisLine, ":")
                        If myEnd > 0 Then
                            myTask.SecondaryStatus = Trim(Mid(thisLine, myStart, myEnd - myStart))
                        Else
                            myTask.SecondaryStatus = ""
                        End If

                        '  strServerTasks = Mid(strServerTasks, myEnd)
                        ' WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " GetDominoServerTasks parsed the secondary status of " & myTask.Name & " as " & myTask.SecondaryStatus)
                        If InStr(myTask.SecondaryStatus, "[") Or InStr(myTask.SecondaryStatus.ToUpper, "SERVER.TASK") Then
                            myTask.SecondaryStatus = ""
                        End If
                    Catch ex As Exception
                        myTask.SecondaryStatus = ""
                    End Try

                    If InStr(myTask.Name, "Traveler") Then

                        'VSPLUS-1938 strip out funny characters from secondary task status for Traveler  - AF 7/2/2015
                        If InStr(myTask.SecondaryStatus, "'") Then
                            myTask.SecondaryStatus = myTask.SecondaryStatus.Replace("'", "")
                        End If

                        If InStr(myTask.SecondaryStatus, "ÿ") Then
                            myTask.SecondaryStatus = myTask.SecondaryStatus.Replace("ÿ", "")
                        End If

                        DominoServer.Traveler_Details = myTask.SecondaryStatus
                        DominoServer.Traveler_Status = "Running"
                        DominoServer.Traveler_Server = True
                        If DominoServer.SecondaryRole.Contains("Traveler") = False Then
                            If DominoServer.SecondaryRole.Trim = "" Then
                                DominoServer.SecondaryRole = "Traveler"
                            Else
                                DominoServer.SecondaryRole += "; Traveler"
                            End If
                        End If
                    End If

                    'BlackBerry Server
                    If InStr(myTask.Name, "DBES") Then

                        DominoServer.BES_Server = True
                        If DominoServer.SecondaryRole.Contains("BES") = False Then
                            If DominoServer.SecondaryRole.Trim = "" Then
                                DominoServer.SecondaryRole = "BES"
                            Else
                                DominoServer.SecondaryRole += "; BES"
                            End If
                        End If
                    End If

                    'Sametime Server
                    If myTask.Name.Contains("Sametime Server") = True Then
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Setting secondary role to 'Sametime'")
                        If DominoServer.SecondaryRole.Contains("Sametime") = False Then
                            If DominoServer.SecondaryRole.Trim = "" Then
                                DominoServer.SecondaryRole = "Sametime"
                            Else
                                DominoServer.SecondaryRole += "; Sametime"
                            End If
                        End If
                    End If
                    ' WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " The line I am working with is " & thisLine)
                    'Get the date
                    myStart = InStr(thisLine, "[") + 1
                    myEnd = InStr(myStart + 2, thisLine, "]")
                    '       WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " The date starts with " & myStart & " and ends with " & myEnd)
                    Dim myDateString As String

                    Try
                        Dim SpaceLocation As Integer
                        Try

                            myDateString = Trim(Mid(thisLine, myStart, myEnd - myStart))

                        Catch ex As Exception
                            myDateString = "No date provided"
                        End Try
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " I have isolated the date as  " & myDateString)
                        If myDateString <> "No date provided" Then
                            Dim myTimeOnly As String
                            Dim myDateOnly As String
                            'format is 08/11/2005 14:02:17 AM EDT
                            SpaceLocation = myDateString.IndexOf(" ")
                            myTimeOnly = Mid(myDateString, SpaceLocation + 2, 8)
                            myDateOnly = Mid(myDateString, 1, SpaceLocation)
                            'WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " My date string for " & myTask.Name & ", " & myTask.TaskName & " is " & myDateString & ", my time only is " & myTimeOnly)
                            Try
                                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Attempting to convert " & myDateOnly & " " & myTimeOnly)
                                'myTask.LastUpdated = (CDate(Now.Date & " " & myTimeOnly)).ToUniversalTime
                                myTask.LastUpdated = CDate(myDateOnly & " " & myTimeOnly)
                                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Dated converted as " & myTask.LastUpdated.ToLongDateString & " " & myTask.LastUpdated.ToShortTimeString)
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Exception: " & ex.Message)
                                myTask.LastUpdated = Now
                            End Try
                            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Final date is " & myTask.LastUpdated)

                        End If


                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " GetDominoServerTasks module Error parsing the date: " & ex.Message)
                        myTask.LastUpdated = ""
                    End Try

                    'lop off everything you've processed already
                    '    strServerTasks = Mid(strServerTasks, myEnd)

                    'Add every server task, even repeats
                    MyServerTasks.Add(myTask)

                    'If MyServerTasks.Search(myTask.Name) Is Nothing Then
                    '    'Only track the first instance of a task
                    '    MyServerTasks.Add(myTask)
                    '    '  WriteAuditEntry(Now.ToString & " Adding " & myTask.Name & " to list of current server tasks")
                    'End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing server tasks: " & ex.Message)
                End Try


            Loop
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing server tasks: " & ex.Message)
        End Try


        'Add the collection of what is currently running to the server
        Try
            DominoServer.ServerTasks = Nothing
            DominoServer.ServerTasks = MyServerTasks
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Found " & DominoServer.ServerTasks.Count & " running server tasks on Domino server " & DominoServer.Name)
        Catch ex As Exception

        End Try

        Dim strMessage As String

        'Compare what you got with what you're supposed to have
        If DominoServer.ServerTaskSettings.Count > 0 Then
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.ServerTaskSettings.Count & " server tasks are configured for monitoring:")
            For Each ConfiguredTask As MonitoredItems.ServerTaskSetting In DominoServer.ServerTaskSettings
                If ConfiguredTask.DisallowTask = True Then
                    strMessage = " is prohibited from running on this server."
                Else
                    strMessage = " is supposed to be running on this server."
                End If
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & ConfiguredTask.Name & strMessage)
            Next
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, " ")
        End If

        Try
            For Each ConfiguredTask As MonitoredItems.ServerTaskSetting In DominoServer.ServerTaskSettings

                'for each task that is supposed to be running, check to see if it is
                Dim SearchTask As MonitoredItems.ServerTask
                Dim SearchString As String
                SearchString = ConfiguredTask.ConsoleString
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Checking the status of server task: " & SearchString)


                Try
                    '2/24/2016 NS modified for VSPLUS-2380
                    If InStr(ConfiguredTask.ConsoleString.ToLower, "traveler") > 0 Then
                        SearchTask = DominoServer.ServerTasks.Search("Traveler")
                    Else
                        SearchTask = DominoServer.ServerTasks.Search(ConfiguredTask.ConsoleString)
                    End If
                    If SearchTask Is Nothing Then
                        SearchTask = DominoServer.ServerTasks.Search(ConfiguredTask.Name)
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error locating server task: " & SearchString)
                End Try


                If SearchTask Is Nothing Then
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & SearchString & " is not loaded.")
                    'HSBC Specific Code
                    If InStr(DominoServer.Name.ToUpper, "HSBC") > 0 Or InStr(DominoServer.Name.ToUpper, "RPR") > 0 Then
                        If InStr(ConfiguredTask.Name, "Admin") > 0 Then
                            If Now.Hour > 5 Or Now.Hour < 21 Then
                                Dim ServerTask As New MonitoredItems.ServerTask
                                ServerTask.SecondaryStatus = "Not monitored from 5 AM - 9 PM"
                                ServerTask.IsMonitored = True
                                ServerTask.LastUpdated = Now
                                ServerTask.Status = "Off Duty"
                                ServerTask.Name = "Admin Process"
                                '  ServerTask.StatusSummary = "Status Summary - OFF DUTY"
                                MyServerTasks.Add(ServerTask)

                                GoTo SkipTask
                            End If
                        End If
                    End If

                    'End HSBC Specific code

                Else
                    '  WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & SearchString & " is loaded on the server. ")
                End If

                If Not (SearchTask Is Nothing) Then
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " I found the " & SearchString & " task.  I am marking it as monitored. ")
                    SearchTask.IsMonitored = True
                    SearchTask.StatusSummary = "OK"
                End If


                If SearchTask Is Nothing And ConfiguredTask.DisallowTask <> True And ConfiguredTask.Enabled Then
                    'the task is supposed to be running, but isn't
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Configured Task " & ConfiguredTask.Name & " was not found.")
                    DominoServer.Status = ConfiguredTask.Name & " Task Warning"
                    'DominoServer.Description = ConfiguredTask.Name & " Task Warning"
                    'WriteAuditEntry(Now.ToString & " Server Status: " & DominoServer.Status)
                    ConfiguredTask.FailureCount += 1
                    ConfiguredTask.Status = "Missing"

                    Try
                        Dim ServerTask As MonitoredItems.ServerTask
                        Dim newServerTask As New MonitoredItems.ServerTask
                        ServerTask = MyServerTasks.Search(ConfiguredTask.Name)
                        If ServerTask Is Nothing Then
                            ServerTask = newServerTask
                        End If
                        ServerTask.Name = ConfiguredTask.Name
                        ServerTask.StatusSummary = "Missing"
                        ServerTask.IsMonitored = True
                        ServerTask.LastUpdated = Now
                        ServerTask.Status = "Not Found"
                        ServerTask.SecondaryStatus = ""

                        If MyServerTasks.Search(ConfiguredTask.Name) Is Nothing Then
                            'Only track the first instance of a task
                            MyServerTasks.Add(ServerTask)
                            WriteAuditEntry(Now.ToString & " Adding the missing " & ServerTask.Name & " to list of current server tasks")
                        End If

                        'DominoServer.ServerTasks.Add(ServerTask)

                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Exception with missing Configured Task " & ex.ToString)

                    End Try

                    Try
                        '  WriteAuditEntry(Now.ToString & " Failure Count = " & ConfiguredTask.FailureCount)
                        If ConfiguredTask.FailureCount = 1 Then
                            If TaskStatus = "" Then
                                TaskStatus = "Task " & ConfiguredTask.Name & " was not found."
                            Else
                                TaskStatus += vbCrLf & "Task " & ConfiguredTask.Name & " was not found."
                            End If
                        Else
                            If TaskStatus = "" Then
                                TaskStatus = "Task " & ConfiguredTask.Name & " was not found " & ConfiguredTask.FailureCount & " times in a row."
                            Else
                                TaskStatus += vbCrLf & "Task " & ConfiguredTask.Name & " was not found " & ConfiguredTask.FailureCount & " times in a row."
                            End If
                        End If

                        DominoServer.AlertCondition = True
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & ex.Message & "  " & ex.Source)
                    End Try

                    If ConfiguredTask.LoadIfMissing = True And ConfiguredTask.Enabled = True And ConfiguredTask.FailureCount >= ConfiguredTask.FailureThreshold Then
                        'need to get the load command
                        DominoServer.Status = ConfiguredTask.Name & " Task not found - Load Attempted"
                        If TaskStatus = "" Then
                            TaskStatus = ConfiguredTask.Name & " was not found, so the command " & ConfiguredTask.LoadCommand & " was sent."
                        Else
                            TaskStatus += vbCrLf & ConfiguredTask.Name & " was not found, so the command " & ConfiguredTask.LoadCommand & " was sent."
                        End If

                        Try
                            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Sending Load command because the " & ConfiguredTask.Name & " task was not found")
                            SendDominoConsoleCommands(DominoServer.Name, ConfiguredTask.LoadCommand, "Sending Load command because the task was not found")
                            myAlert.QueueAlert("Domino", DominoServer.Name, "Server Task: " & ConfiguredTask.Name, "The task " & ConfiguredTask.Name & " was not found on " & DominoServer.Name & ". A load command was sent to the server.", DominoServer.Location)
                            DominoServer.AlertCondition = True
                        Catch ex As Exception

                        End Try

                        If ConfiguredTask.RestartServerIfMissingASAP = True And ConfiguredTask.FailureCount >= (ConfiguredTask.FailureThreshold + 1) Then
                            Try
                                myAlert.QueueAlert("Domino", DominoServer.Name, "Server Task: " & ConfiguredTask.Name, "The task " & ConfiguredTask.Name & " was not found on " & DominoServer.Name & ". A load command was sent to the server, but it failed.  A restart server command has been sent.", DominoServer.Location)
                                DominoServer.Status = ConfiguredTask.Name & " Task Failure - Server Restarted"
                                If ConfiguredTask.FailureCount = 1 Then
                                    If TaskStatus = "" Then
                                        TaskStatus = "Task " & ConfiguredTask.Name & " not found. A server restart command was sent ASAP."
                                    Else
                                        TaskStatus += vbCrLf & "Task " & ConfiguredTask.Name & " not found. A server restart command was sent ASAP."

                                    End If
                                Else
                                    If TaskStatus = "" Then
                                        TaskStatus = "Task " & ConfiguredTask.Name & " not found " & ConfiguredTask.FailureCount & " times. A server restart command was sent ASAP."
                                    Else
                                        TaskStatus += vbCrLf & "Task " & ConfiguredTask.Name & " not found " & ConfiguredTask.FailureCount & " times. A server restart command was sent ASAP."
                                    End If
                                End If
                                SendDominoConsoleCommands(DominoServer.Name, "restart server", "Sending Restart server command to the server because the task is not found")
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & ex.Message & " Error restarting server " & ex.Source)
                            End Try

                        End If

                        If ConfiguredTask.RestartServerIfMissingOFFHOURS = True And ConfiguredTask.FailureCount >= (ConfiguredTask.FailureThreshold + 1) And DominoServer.OffHours = True Then
                            Try
                                myAlert.QueueAlert("Domino", DominoServer.Name, ConfiguredTask.Name, "The task " & ConfiguredTask.Name & " was not found on " & DominoServer.Name & ". A load command was sent to the server, but it failed.  A restart server command has been sent.", DominoServer.Location)
                                DominoServer.Status = ConfiguredTask.Name & " Task Failure - Server Restarted"
                                If ConfiguredTask.FailureCount = 1 Then
                                    If TaskStatus = "" Then
                                        TaskStatus = "Task " & ConfiguredTask.Name & " not found. A server restart command was sent, OFF HOURS."
                                    Else
                                        TaskStatus = vbCrLf & "Task " & ConfiguredTask.Name & " not found. A server restart command was sent, OFF HOURS."

                                    End If
                                Else
                                    If TaskStatus = "" Then
                                        TaskStatus = "Task " & ConfiguredTask.Name & " not found " & ConfiguredTask.FailureCount & " times. A server restart command was sent, OFF HOURS."
                                    Else
                                        TaskStatus += vbCrLf & "Task " & ConfiguredTask.Name & " not found " & ConfiguredTask.FailureCount & " times. A server restart command was sent, OFF HOURS."
                                    End If
                                End If
                                SendDominoConsoleCommands(DominoServer.Name, "restart server", "Sending restart server command because the task is not found for several times")
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & ex.Message & " Error restarting server " & ex.Source)
                            End Try
                            DominoServer.Description = "Task " & ConfiguredTask.Name & " not found. A server restart command was sent, OFF HOURS."
                        End If


                    End If

                    If ConfiguredTask.Enabled = True And ConfiguredTask.LoadIfMissing = False And ConfiguredTask.FailureCount = ConfiguredTask.FailureThreshold Then
                        DominoServer.Status = ConfiguredTask.Name & " Not Found"
                        If TaskStatus = "" Then
                            TaskStatus = ConfiguredTask.Name & " Not Found"
                        Else
                            TaskStatus += vbCrLf & ConfiguredTask.Name & " Not Found"
                        End If

                        myAlert.QueueAlert("Domino", DominoServer.Name, "Server Task: " & ConfiguredTask.Name, "The task " & ConfiguredTask.Name & " was not found on " & DominoServer.Name & ". No action was taken", DominoServer.Location)
                        DominoServer.AlertCondition = True
                    End If


                ElseIf Not (SearchTask Is Nothing) Then

                    'the task is running
                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & ConfiguredTask.Name & " is running on " & DominoServer.Name)
                    ConfiguredTask.Status = "OK"
                    SearchTask.StatusSummary = "OK"


                    If ConfiguredTask.Enabled = True And ConfiguredTask.DisallowTask = True Then
                        'Task is not supposed to be running
                        ConfiguredTask.Status = "Unauthorized"
                        Try
                            ' WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & ConfiguredTask.Name & " is not supposed to be running. ")

                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & ConfiguredTask.Name & " is not supposed to be running on " & DominoServer.Name & ".  Sending Command: " & "Tell " & Mid(ConfiguredTask.LoadCommand, 3) & " Exit")
                            SearchTask.StatusSummary = "Unauthorized"
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error killing task: " & ex.Message)
                        End Try

                        Try
                            SendDominoConsoleCommands(DominoServer.Name, "Tell " & Mid(ConfiguredTask.LoadCommand, 3) & " Exit", "Sending exit command to the server because the task is not allowed")
                        Catch ex As Exception

                        End Try
                        Try
                            DominoServer.AlertCondition = True
                            DominoServer.Status = "Unauthorized task: " & ConfiguredTask.Name & " Found"

                            If TaskStatus = "" Then
                                TaskStatus = "Unauthorized task: " & ConfiguredTask.Name & " Found"
                            Else
                                TaskStatus += vbCrLf & "Unauthorized task: " & ConfiguredTask.Name & " Found"
                            End If

                        Catch ex As Exception

                        End Try
                        '     myAlert.QueueAlert("Domino", DominoServer.Name, "Server Task: " & ConfiguredTask.Name, "The task " & ConfiguredTask.Name & " was found on " & DominoServer.Name & ". The task was told to exit.", DominoServer.Location)

                        myAlert.QueueAlert("Domino", DominoServer.Name, "Server Task: " & ConfiguredTask.Name, "The task " & ConfiguredTask.Name & " was found on " & DominoServer.Name & ". The task was told to exit.", DominoServer.Location)
                    End If


                    '12/10/2015 NS re-added for VSPLUS-2369
                    'HUNG Task detection
                    Dim RunningTime As TimeSpan
                    Try
                        Dim tNow As New DateTime
                        Try
                            If Not notesDateTime Is Nothing Then
                                tempstr = (notesDateTime.ZoneTime).Substring(0, (notesDateTime.ZoneTime).LastIndexOf(" "))
                                tNow = CDate(tempstr)
                                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Using server time of " & tNow.ToString, LogUtilities.LogUtils.LogLevel.Verbose)
                            Else
                                tNow = Now
                            End If
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(notesDateTime)
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Exception calculating server time : " & ex.Message)
                            tNow = Now
                        End Try


                        Dim tRunningSince As DateTime = SearchTask.LastUpdated
                        RunningTime = tNow.Subtract(tRunningSince)
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & SearchTask.Name & " on " & DominoServer.Name & " was updated at " & tRunningSince & ". " & RunningTime.TotalMinutes & " minutes ago. ")
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error calculating task run time " & ex.Message)
                    End Try
                    'WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & SearchTask.Name & " " & RunningTime.TotalMinutes & " " & ConfiguredTask.MaxRunTime & " " & ConfiguredTask.FreezeDetection & " " & ConfiguredTask.Enabled)
                    If RunningTime.TotalMinutes > ConfiguredTask.MaxRunTime And ConfiguredTask.FreezeDetection = True And ConfiguredTask.Enabled = True Then
                        'The task has been running too long, it is probably hung
                        ConfiguredTask.FailureCount += 1
                        ConfiguredTask.Status = "Hung"
                        Try
                            SearchTask.Status = "Hung"
                            SearchTask.StatusSummary = "Hung"
                        Catch ex As Exception

                        End Try
                        '4/7/2016 NS modified for VSPLUS-2734
                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & ConfiguredTask.Name & " on " & DominoServer.Name & " appears to be hung.  The last update was " & Math.Round(RunningTime.TotalMinutes, MidpointRounding.AwayFromZero).ToString & " minutes ago.")
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " ConfiguredTask.LoadIfMissing - " & ConfiguredTask.LoadIfMissing & ",  ConfiguredTask.FailureCount - " & ConfiguredTask.FailureCount & ", ConfiguredTask.FailureThreshold - " & ConfiguredTask.FailureThreshold)
                        If ConfiguredTask.LoadIfMissing = False And ConfiguredTask.FailureCount >= ConfiguredTask.FailureThreshold Then
                            DominoServer.Status = ConfiguredTask.Name & " Hung"

                            If TaskStatus = "" Then
                                TaskStatus = "The server task '" & ConfiguredTask.Name & "' is not responding. The last update was " & Math.Round(RunningTime.TotalMinutes, MidpointRounding.AwayFromZero).ToString & " minutes ago."
                            Else
                                TaskStatus += vbCrLf & "The server task '" & ConfiguredTask.Name & "' is not responding. The last update was " & Math.Round(RunningTime.TotalMinutes, MidpointRounding.AwayFromZero).ToString & " minutes ago."
                            End If
                            DominoServer.Description = "The task " & ConfiguredTask.Name & " appears to be hung"
                            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Attempting to send an alert for " & ConfiguredTask.Name & " on " & DominoServer.Name & " - hung.")
                            myAlert.QueueAlert("Domino", DominoServer.Name, "Server Task: " & ConfiguredTask.Name, "The task " & ConfiguredTask.Name & " appears to be hung on " & DominoServer.Name & ". The last update was " & Math.Round(RunningTime.TotalMinutes, MidpointRounding.AwayFromZero).ToString & " minutes ago.  No action was taken", DominoServer.Location)
                        End If

                        If ConfiguredTask.LoadIfMissing = True And ConfiguredTask.FailureCount = ConfiguredTask.FailureThreshold And ConfiguredTask.RestartServerIfMissingASAP = True Then
                            DominoServer.Status = ConfiguredTask.Name & " Hung"
                            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Attempting to send an alert for " & ConfiguredTask.Name & " on " & DominoServer.Name & " - hung.")
                            myAlert.QueueAlert("Domino", DominoServer.Name, "Server Task: " & ConfiguredTask.Name, "The task " & ConfiguredTask.Name & " was hung on " & DominoServer.Name & ". The server was restarted.", DominoServer.Location)
                            SendDominoConsoleCommand(DominoServer.Name, "restart server")
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & ConfiguredTask.Name & " on " & DominoServer.Name & " was hung.  Server restarted.")
                            If TaskStatus = "" Then
                                TaskStatus = "The server task '" & ConfiguredTask.Name & "' was hung.  The server was restarted."
                            Else
                                TaskStatus += vbCrLf & "The server task '" & ConfiguredTask.Name & "' was hung.  The server was restarted ASAP."
                            End If
                            DominoServer.Description = "The server task '" & ConfiguredTask.Name & "' was hung.  The server was restarted ASAP."
                        End If

                        If ConfiguredTask.LoadIfMissing = True And ConfiguredTask.FailureCount = ConfiguredTask.FailureThreshold And ConfiguredTask.RestartServerIfMissingOFFHOURS = True And OffHours(DominoServer.Name) Then
                            DominoServer.Status = ConfiguredTask.Name & " Hung"
                            DominoServer.Description = "The task " & ConfiguredTask.Name & " was hung on " & DominoServer.Name & ". It is now off hours, so the server was restarted."
                            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Attempting to send an alert for " & ConfiguredTask.Name & " on " & DominoServer.Name & " - hung.")
                            myAlert.QueueAlert("Domino", DominoServer.Name, "Server Task: " & ConfiguredTask.Name, "The task " & ConfiguredTask.Name & " was hung on " & DominoServer.Name & ". It is now off hours, so the server was restarted.", DominoServer.Location)
                            SendDominoConsoleCommand(DominoServer.Name, "restart server")
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & ConfiguredTask.Name & " on " & DominoServer.Name & " was hung.  Server restarted.")
                            If TaskStatus = "" Then
                                TaskStatus = "The server task '" & ConfiguredTask.Name & "' was hung.  The server was restarted."
                            Else
                                TaskStatus += vbCrLf & "The server task '" & ConfiguredTask.Name & "' was hung.  The server was restarted (Off Hours)."
                            End If

                        End If

                    End If

                    '12/11/2015 NS modified for VSPLUS-2369
                    Try
                        If ConfiguredTask.Enabled = True And ConfiguredTask.DisallowTask = False And (ConfiguredTask.FreezeDetection = False Or ConfiguredTask.FreezeDetection = True And ConfiguredTask.Status <> "Hung") Then
                            'it is supposed to be running
                            'If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Name: " & ConfiguredTask.Name)
                            ' If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Location: " & DominoServer.Location)
                            myAlert.ResetAlert("Domino", DominoServer.Name, "Server Task: " & ConfiguredTask.Name, DominoServer.Location, "Task is running", "Server Tasks")
                            ConfiguredTask.FailureCount = 0
                        End If
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error calculating whether task is OK " & ex.Message)
                    End Try

                Else
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Server task is not supposed to be running, and it is not running.")
                    'Not supposed to be running and not running so do nothing
                    Try
                        ' myAlert.ResetAlert("Server Task", DominoServer.Name, ConfiguredTask.Name, DominoServer.Location)
                        myAlert.ResetAlert("Domino", DominoServer.Name, "Server Task: " & ConfiguredTask.Name, DominoServer.Location)

                    Catch ex As Exception

                    End Try
                    Try
                        ConfiguredTask.Status = "OK"
                        SearchTask.StatusSummary = "OK"
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error setting status of task " & ex.Message)
                    End Try
                End If
SkipTask:
            Next
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error looping through monitored tasks: " & ex.Message)
        End Try

        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Now summarizing server task status...")

        Try
            Dim n, myCounter, myErrorCount As Integer
            myCounter = 0
            Dim task As MonitoredItems.ServerTaskSetting
            For n = 0 To DominoServer.ServerTaskSettings.Count - 1
                task = DominoServer.ServerTaskSettings.Item(n)
                Try
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Server Task: " & task.Name & " status is  " & task.Status)
                    If task.Status = "OK" Then
                        myCounter += 1
                    Else
                        myErrorCount += 1
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Exception looping through tasks: " & ex.Message)
                End Try
            Next

            If myErrorCount > 1 Then
                DominoServer.Status = "Server Tasks Warning"

                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.Name, DominoServer.Name) And _
                    repository.Filter.Eq(Function(x) x.Type, "Domino")
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                     .Set(Function(x) x.CurrentStatus, "Server Tasks Warning") _
                                                                                     .Set(Function(x) x.StatusCode, "Issue")


            End If


            If myCounter = DominoServer.ServerTaskSettings.Count Then
                Try
                    TaskStatus = "All (" & myCounter.ToString & ") monitored server tasks are OK."
                Catch ex As Exception

                End Try
            Else
                Try
                    Dim myString As String = ""
                    myString = myCounter.ToString & " of " & DominoServer.ServerTaskSettings.Count & " monitored server tasks are running." & vbCrLf & vbCrLf & TaskStatus
                    TaskStatus = myString
                Catch ex As Exception

                End Try
            End If

            n = Nothing
            myCounter = Nothing
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error looping through task status " & ex.Message)
        End Try

        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "  Server Task status is: " & TaskStatus & vbCrLf)

        'DominoServer.ResponseDetails += vbCrLf & TaskStatus
        '   WriteAuditEntry(Now.ToString & " ********* AF Says server Task status is: " & vbCrLf & TaskStatus & vbCrLf)
        DominoServer.TaskStatus = TaskStatus
        strServerTasks = Nothing
        strSHOWTASKS = Nothing


        Try
            Dim ServerTask As MonitoredItems.ServerTask
            Dim ConfiguredTask As MonitoredItems.ServerTaskSetting
            Dim n As Integer
            For n = 0 To DominoServer.ServerTaskSettings.Count - 1
                ConfiguredTask = DominoServer.ServerTaskSettings.Item(n)
                ServerTask = Nothing
                Try
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Server Task: " & ConfiguredTask.Name & " status is  " & ConfiguredTask.Status)
                    If ConfiguredTask.Status = "OK" Then
                        'Don't do anything
                    Else
                        ServerTask = DominoServer.ServerTasks.Search(ConfiguredTask.Name)
                        If Not ServerTask Is Nothing Then
                            ServerTask.StatusSummary = ConfiguredTask.Status
                            '12/11/2015 NS modified for VSPLUS-2369
                            If ServerTask.IsMonitored = "" Then
                                ServerTask.IsMonitored = True
                            End If
                            If ServerTask.Status = "" Then
                                ServerTask.Status = ConfiguredTask.Status
                            End If
                        Else
                            ServerTask = New MonitoredItems.ServerTask
                            ServerTask.Name = ConfiguredTask.Name
                            ServerTask.StatusSummary = ConfiguredTask.Status
                            '12/11/2015 NS modified for VSPLUS-2369
                            ServerTask.LastUpdated = Now
                            If ServerTask.IsMonitored = "" Then
                                ServerTask.IsMonitored = True
                            End If
                            If ServerTask.Status = "" Then
                                ServerTask.Status = ConfiguredTask.Status
                            End If
                            ServerTask.SecondaryStatus = ""
                            DominoServer.ServerTasks.Add(ServerTask)
                        End If
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Exception looping through configured tasks: " & ex.Message)
                End Try
            Next
        Catch ex As Exception

        End Try

        Try
            UpdateDominoStatusTable(DominoServer)
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Finished checking Server Tasks.  The task status is " & TaskStatus)
        Catch ex As Exception

        End Try
        Try
            'WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "  Now Updating DominoServerTaskStatus table.  ")
            Dim listOfEntities As New List(Of VSNext.Mongo.Entities.DominoServerTask)()

            For Each ServerTask As MonitoredItems.ServerTask In MyServerTasks
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, vbCrLf)
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "  Server Task name is: " & ServerTask.Name)
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "  Server Task status summary is: " & ServerTask.StatusSummary)
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "  Server Task status is: " & ServerTask.Status)
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "  Server Task secondary is: " & ServerTask.SecondaryStatus)
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "  Server Task is monitored: " & ServerTask.IsMonitored)
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "  Server Task last updated: " & ServerTask.LastUpdated)

                Try
                    listOfEntities.Add(New VSNext.Mongo.Entities.DominoServerTask With {
                                    .LastUpdated = GetFixedDateTime(ServerTask.LastUpdated),
                                    .Monitored = IIf(Boolean.TryParse(ServerTask.IsMonitored, New Boolean), Boolean.Parse(ServerTask.IsMonitored), False),
                                    .PrimaryStatus = ServerTask.Status,
                                    .StatusSummary = ServerTask.StatusSummary,
                                    .TaskName = ServerTask.TaskName,
                                    .SecondaryStatus = ServerTask.SecondaryStatus
                                })

                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, "Exception adding to DominoServerTaskStatus List: " & ex.ToString)
                End Try



            Next

            Try
                Dim DominoServer2 As MonitoredItems.DominoServer = DominoServer
                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.TypeAndName.Equals(DominoServer2.Name & "-Domino"))
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                     .Set(Function(x) x.DominoServerTasks, listOfEntities)
                repository.Update(filterDef, updateDef)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, "Exception inserting DominoServerTask List: " & ex.ToString)
            End Try

        Catch ex As Exception

        End Try



    End Sub

    Private Sub CheckDomino_DominoStats(ByRef MyDominoServer As MonitoredItems.DominoServer)
        'Domino servers maintain a set of 'Domino' statistics that store http user usage settings
        'Hence the two Dominos in the module name
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Checking Domino.Command statistics.")
        '  WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, vbCrLf & MyDominoServer.Statistics_Domino)

        Try
            If Not (MyDominoServer.Statistics_Domino Is Nothing) And Not (InStr(MyDominoServer.Statistics_Domino.ToUpper, "Domino.Command".ToUpper) > 0) Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " No Domino.Command statistics found.")
                Exit Sub
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino Statistics in not set.  Will now exit Sub.")
            Exit Sub
        End Try

        Try
            Dim myStat, myIncrement As Integer
            myStat = ParseNumericStatValue("Domino.Command.OpenDocument", MyDominoServer.Statistics_Domino)

            If MyDominoServer.priorDominoOpenDocument = -1 Then
                myIncrement = 1
            Else
                myIncrement = myStat - MyDominoServer.priorDominoOpenDocument
            End If

            If myStat < MyDominoServer.priorDominoOpenDocument And MyDominoServer.priorDominoOpenDocument <> -1 Then
                'the server restarted
                myIncrement = myStat
            End If

            If myStat <> Nothing Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.OpenDocument value is " & myStat)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.OpenDocument increment value from last scan is " & myIncrement)
                UpdateDominoDailyStatTable(MyDominoServer, "Domino.Command.OpenDocument", myIncrement)
                MyDominoServer.priorDominoOpenDocument = myStat
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.OpenDocument could not be determined: " & ex.ToString)
        End Try

        Try
            Dim myStat, myIncrement As Integer
            myStat = ParseNumericStatValue("Domino.Command.CreateDocument", MyDominoServer.Statistics_Domino)

            If MyDominoServer.priorDominoCreateDocument = -1 Then
                myIncrement = 1
            Else
                myIncrement = myStat - MyDominoServer.priorDominoCreateDocument
            End If

            If myStat < MyDominoServer.priorDominoCreateDocument And MyDominoServer.priorDominoCreateDocument <> -1 Then
                'the server restarted
                myIncrement = myStat
            End If

            If myStat <> Nothing Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.CreateDocument value is " & myStat)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.CreateDocument increment value from last scan is " & myIncrement)
                UpdateDominoDailyStatTable(MyDominoServer, "Domino.Command.CreateDocument", myIncrement)
                MyDominoServer.priorDominoCreateDocument = myStat
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.CreateDocument could not be determined: " & ex.ToString)
        End Try


        Try
            Dim myStat, myIncrement As Integer
            myStat = ParseNumericStatValue("Domino.Command.DeleteDocument", MyDominoServer.Statistics_Domino)

            If MyDominoServer.priorDominoDeleteDocument = -1 Then
                myIncrement = 1
            Else
                myIncrement = myStat - MyDominoServer.priorDominoDeleteDocument
            End If

            If myStat < MyDominoServer.priorDominoDeleteDocument And MyDominoServer.priorDominoDeleteDocument <> -1 Then
                'the server restarted
                myIncrement = myStat
            End If

            If myStat <> Nothing Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.DeleteDocument value is " & myStat)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.DeleteDocument increment value from last scan is " & myIncrement)
                UpdateDominoDailyStatTable(MyDominoServer, "Domino.Command.DeleteDocument", myIncrement)
                MyDominoServer.priorDominoDeleteDocument = myStat
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.DeleteDocument could not be determined: " & ex.ToString)
        End Try


        Try
            Dim myStat, myIncrement As Integer
            myStat = ParseNumericStatValue("Domino.Command.OpenDatabase", MyDominoServer.Statistics_Domino)

            If MyDominoServer.priorDominoOpenDatabase = -1 Then
                myIncrement = 1
            Else
                myIncrement = myStat - MyDominoServer.priorDominoOpenDatabase
            End If

            If myStat < MyDominoServer.priorDominoOpenDatabase And MyDominoServer.priorDominoOpenDatabase <> -1 Then
                'the server restarted
                myIncrement = myStat
            End If

            If myStat <> Nothing Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.OpenDatabase value is " & myStat)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.OpenDatabase increment value from last scan is " & myIncrement)
                UpdateDominoDailyStatTable(MyDominoServer, "Domino.Command.OpenDatabase", myIncrement)
                MyDominoServer.priorDominoOpenDatabase = myStat
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.OpenDatabase could not be determined: " & ex.ToString)
        End Try


        Try
            Dim myStat, myIncrement As Integer
            myStat = ParseNumericStatValue("Domino.Command.OpenView", MyDominoServer.Statistics_Domino)

            If MyDominoServer.priorDominoOpenView = -1 Then
                myIncrement = 1
            Else
                myIncrement = myStat - MyDominoServer.priorDominoOpenView
            End If


            If myStat < MyDominoServer.priorDominoOpenView And MyDominoServer.priorDominoOpenView <> -1 Then
                'the server restarted
                myIncrement = myStat
            End If

            If myStat <> Nothing Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.OpenView value is " & myStat)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.OpenView increment value from last scan is " & myIncrement)
                UpdateDominoDailyStatTable(MyDominoServer, "Domino.Command.OpenView", myIncrement)
                MyDominoServer.priorDominoOpenView = myStat
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.OpenView could not be determined: " & ex.ToString)
        End Try


        Try
            Dim myStat, myIncrement As Integer
            myStat = ParseNumericStatValue("Domino.Command.Total", MyDominoServer.Statistics_Domino)

            If MyDominoServer.priorDominoTotal = -1 Then
                myIncrement = 1
            Else
                myIncrement = myStat - MyDominoServer.priorDominoTotal
            End If

            If myStat < MyDominoServer.priorDominoTotal And MyDominoServer.priorDominoTotal <> -1 Then
                'the server restarted
                myIncrement = myStat
            End If

            If myStat <> Nothing Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.Total value is " & myStat)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.Total increment value from last scan is " & myIncrement)
                UpdateDominoDailyStatTable(MyDominoServer, "Domino.Command.Total", myIncrement)
                MyDominoServer.priorDominoTotal = myStat
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino.Command.Total could not be determined: " & ex.ToString)
        End Try


    End Sub

    Private Sub CheckDominoMemory(ByRef MyDominoServer As MonitoredItems.DominoServer)
        'this subroutine makes sure the Domino server has enough RAM
        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino Monitoring is checking memory for " & MyDominoServer.Name)


        Dim PercentRAMinUse As Double

        'Useful memory platform statistics
        ' Platform.Memory.RAM.AvailMBytes = 2,978
        ' Platform.Memory.RAM.PctUtil = 27   <-- the amount of memory in use
        ' Platform.Memory.RAM.TotalMBytes = 4096

        PercentRAMinUse = 999

        Try
            If MyDominoServer.Memory_Threshold < 10 Then
                Exit Sub
            End If
        Catch ex As Exception

        End Try

        Dim USprovider As IFormatProvider = Globalization.CultureInfo.CreateSpecificCulture("en-us")
        Dim Europrovider As IFormatProvider = Globalization.CultureInfo.CreateSpecificCulture("fr-FR")



        If InStr(MyDominoServer.Statistics_Platform, "Platform.Memory.RAM.PctUtil") Then
            Try
                PercentRAMinUse = ParseNumericStatValue("Platform.Memory.RAM.PctUtil", MyDominoServer.Statistics_Platform)

                Try
                    MyDominoServer.MemoryPercentUsed = Decimal.Parse(PercentRAMinUse, System.Globalization.NumberStyles.Float, USprovider)
                Catch ex As Exception

                End Try

                If MyDominoServer.MemoryPercentUsed > 100 Then
                    Try
                        MyDominoServer.MemoryPercentUsed = Decimal.Parse(PercentRAMinUse, System.Globalization.NumberStyles.Float, Europrovider)
                    Catch ex As Exception

                    End Try
                End If

                If MyDominoServer.MemoryPercentUsed > 100 Then
                    MyDominoServer.MemoryPercentUsed = 0
                End If

                ' MyDominoServer.MemoryPercentUsed = Convert.ToDouble(PercentRAMinUse)
                If MyLogLevel = LogLevel.Verbose Then
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & PercentRAMinUse & "% of memory is in use.")
                End If
            Catch ex As Exception
                PercentRAMinUse = 999
                MyDominoServer.MemoryPercentUsed = 0
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception converting memory in use.  The server returned " & PercentRAMinUse)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception converting memory in use.  The exception was " & ex.ToString)
            End Try
        Else
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Platform.Memory.RAM.PctUtil was not found, so free memory cannot be determined.")
        End If

        Try
            If PercentRAMinUse <> 999 Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Updating DominoDailyStat table with Mem.PercentUsed as " & PercentRAMinUse & "% of memory used.")
                UpdateDominoDailyStatTable(MyDominoServer, "Mem.PercentUsed", PercentRAMinUse)

                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Updating DominoDailyStat table with Mem.PercentAvailable as " & (100 - PercentRAMinUse) & "% of memory used.")
                UpdateDominoDailyStatTable(MyDominoServer, "Mem.PercentAvailable", (100 - PercentRAMinUse))

                If PercentRAMinUse > MyDominoServer.Memory_Threshold And InStr(MyDominoServer.OperatingSystem.ToUpper, "WINDOWS") > 0 Then
                    'Unix servers are supposed to use all available memory.
                    MyDominoServer.Status = "Low Memory: " & PercentRAMinUse & "% used."
                    If MyDominoServer.Memory_Threshold <> 0 Then
                        myAlert.QueueAlert("Domino", MyDominoServer.Name, "Memory", "The server is reporting low memory.  The statistic Platform.Memory.RAM.PctUtil reports that " & PercentRAMinUse & "% of memory is currently in use.  Your alert threshold for this server is " & MyDominoServer.Memory_Threshold & "%. Consistent low memory often leads to a server crash.", MyDominoServer.Location)
                    End If
                Else
                    ' myAlert.ResetAlert("Domino", MyDominoServer.Name, "Memory", MyDominoServer.Location)
                    myAlert.ResetAlert("Domino", MyDominoServer.Name, "Memory", MyDominoServer.Location, "Server reports memory at " & PercentRAMinUse & "% used.")
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub UpdateDominoThreadKilledCounter()
        Try
            Dim strSql As String = "Update NodeDetails set Value = '" & HungThreadsKilled & "' where NodeId = (Select ID from Nodes where Name='" & System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString() & "') and Name='Domino Thread Killed Counter'"
            Dim objVSAdaptor As New VSAdaptor
            If objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSql) = False Then
                strSql = "INSERT INTO NodeDetails (NodeId, Name, Value) VALUES ((Select ID from Nodes where Name='" & System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString() & "'), 'Domino Thread Killed Counter', '" & HungThreadsKilled & "')"
                objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSql)
            End If

        Catch ex As Exception

        End Try
    End Sub

    '12/30/2015 NS added for VSPLUS-2669
    Private Sub GetServerTimeZone(ByVal consoleStr As String, ByRef server As MonitoredItems.DominoServer)
        Dim tempstr As String = ""
        Dim dstStr As Boolean = False
        Dim tzInt As Integer = 0
        Try
            tempstr = consoleStr.Substring(consoleStr.IndexOf("GMT"), consoleStr.IndexOf("DST") - consoleStr.IndexOf("GMT"))
            tempstr = tempstr.Substring(3, tempstr.IndexOf(":") - 3)
            tzInt = Convert.ToInt32(tempstr)
            tempstr = consoleStr.Substring(consoleStr.IndexOf("DST"), consoleStr.Length - consoleStr.IndexOf("DST") - 1)
            If Not InStr(tempstr, "Not") Then
                dstStr = True
            End If
            '3/28/2016 NS modified - the number must be multiplied by -1 to convert it to the Domino format
            '7:00	MST		MDT	Mountain Standard Time
            '-7:00	ZE7		ZE7	7 hours east of GMT
            'So the value returned by the server GMT-7 should really be converted to 7 to match what Domino supports
            server.ServerTimeTZ = tzInt * (-1)
            server.ServerTimeDST = dstStr
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " Exception in GetServerTimeZone : " & ex.Message)
        End Try
    End Sub

    '2/22/2016 NS added for VSPLUS-2641
    Private Sub CheckDominoAvailabilityIndex(ByRef MyDominoServer As MonitoredItems.DominoServer)
        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino Monitoring is checking availability index threshold for " & MyDominoServer.Name)

        If MyDominoServer.AvailabilityIndex < MyDominoServer.AvailabilityIndexThreshold Then
            '3/1/2016 NS modified for VSPLUS-2641
            myAlert.QueueAlert("Domino", MyDominoServer.Name, "Availability Index", "The server is reporting its availability index below the set threshold. The statistic Server.AvailabilityIndex reports that " & MyDominoServer.AvailabilityIndex & "% of the server's resources are available. This alert is triggered whenever less than " & MyDominoServer.AvailabilityIndexThreshold & "% of the server's resources are available.", MyDominoServer.Location)
        Else
            myAlert.ResetAlert("Domino", MyDominoServer.Name, "Availability Index", MyDominoServer.Location, "Server reports availability index at " & MyDominoServer.AvailabilityIndex & "%.")
        End If
    End Sub
#Region "Disk Space Related"

    Private Sub CheckDominoDiskSpace(ByRef MyDominoServer As MonitoredItems.DominoServer)
        'this subroutine makes sure the Domino server has enough Disk space
        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Checking disk space.")


        If MyDominoServer.Name = "germanland/RPRWyatt" Then
            MyDominoServer.Statistics_Disk = "Disk.C.Free=30ÿ750ÿ699ÿ520" & vbCrLf & "Disk.C.Size=64ÿ316ÿ502ÿ016" & vbCrLf & "Disk.C.Type=NTFS"
        End If


        If MyDominoServer.Name = "Apollo/RPRWyatt" Then
            MyDominoServer.Statistics_Disk = "Disk.C.Free=1.0995084000E+12" & vbCrLf & "Disk.C.Size=2.0995084780E+12" & vbCrLf & "Disk.C.Type=NTFS"
        End If


        'If this is an AIX box, don't check the disk space

        'If InStr(MyDominoServer.OperatingSystem, "AIX") Then
        '    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Skipping disk space checking for AIX servers")
        '    Exit Sub
        'End If

        'First, figure out which disks the user wants to monitor

        Dim strSQL As String = "SELECT ServerName, DiskName, Threshold, ThresholdType FROM DominoDiskSettings WHERE ServerName ='" & MyDominoServer.Name & "' "
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Disk SQL= " & strSQL)
        Dim dbAdapter As New VSAdaptor
        Dim dsDrives As New DataSet
        dsDrives.Tables.Add("Drives")
        dbAdapter.FillDatasetAny("VitalSigns", "vs", strSQL, dsDrives, "Drives")

        Dim dtDataTable As DataTable
        dtDataTable = dsDrives.Tables(0)
        Dim boolAllDrives As Boolean = False
        Dim AllDrivesThresholdType As String = "Percent"
        Dim boolNoAlerts As Boolean = False

        Try
            If dtDataTable.Rows.Count = 0 Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This server is configured to monitor all servers at the same threshold value.")
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The threshold value is " & dtDataTable.Rows(0).Item("Threshold") & " " & dtDataTable.Rows(0).Item("ThresholdType"))
                AllDrivesThresholdType = dtDataTable.Rows(0).Item("ThresholdType")
                boolAllDrives = True
            End If
        Catch ex As Exception

        End Try

        Try
            For Each row As DataRow In dtDataTable.Rows
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Set to monitor " & row.Item("DiskName") & " for " & row.Item("Threshold") & " " & row.Item("ThresholdType"))
                If row.Item("DiskName") = "AllDisks" Then
                    boolAllDrives = True
                    MyDominoServer.DiskThreshold = row.Item("Threshold")
                    AllDrivesThresholdType = dtDataTable.Rows(0).Item("ThresholdType")
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This server is configured to monitor all servers at the same threshold value: " & MyDominoServer.DiskThreshold & " " & AllDrivesThresholdType)
                End If
                If row.Item("DiskName") = "NoAlerts" Then
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This server is configured to NOT monitor disk drives.")
                    boolNoAlerts = True
                End If
            Next row
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception looping through drive configuration: " & ex.ToString)
        End Try


        Dim myDiskDrive As MonitoredItems.DominoDiskSpace
        Dim myDiskNames As String = ""
        Dim strDiskSize As String = ""


        If MyDominoServer.Statistics_Platform <> "" And Not (InStr(MyDominoServer.OperatingSystem, "AIX")) Then
            For n As Integer = 1 To 150

                Try
                    If InStr(MyDominoServer.Statistics_Platform, "Platform.LogicalDisk." & n.ToString & ".AssignedName") Then
                        Try
                            myDiskNames = "Disk." & ParseTextStatValue("Platform.LogicalDisk." & n.ToString & ".AssignedName", MyDominoServer.Statistics_Platform)
                            ' myDiskNames = myDiskNames.Replace("Disk.", "")
                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Platform.LogicalDisk." & n.ToString & " name is " & myDiskNames)

                        Catch ex As Exception
                            myDiskNames = "n/a"
                        End Try

                        If (InStr(myDiskNames, "opt/patrol")) Or (InStr(myDiskNames, "domlog")) Then
                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Skipping this drive... ")
                            GoTo skipdrive
                        End If

                        Try
                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Searching for the drive in the collection.")
                            myDiskDrive = MyDominoServer.DiskDrives.Search(myDiskNames)
                        Catch ex As Exception
                            myDiskDrive = Nothing
                        End Try

                        If myDiskDrive Is Nothing And ParseNumericStatValue("Platform.LogicalDisk.TotalNumofDisks", MyDominoServer.Statistics_Platform) = 1 Then
                            If MyDominoServer.DiskDrives.Count = 1 Then
                                myDiskDrive = MyDominoServer.DiskDrives.Item(0)
                            End If
                        End If

                        If myDiskDrive Is Nothing And Not (InStr(myDiskNames, "opt/patrol")) And Not (InStr(myDiskNames, "domlog")) Then
                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Adding the drive " & myDiskNames & " to the collection to add platform stats.")
                            myDiskDrive = New MonitoredItems.DominoDiskSpace
                            myDiskDrive.DiskName = myDiskNames
                            MyDominoServer.DiskDrives.Add(myDiskDrive)
                        Else
                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Updating existing drive in the collection to add platform stats.")
                            myDiskDrive.LastUpdated = Now
                        End If
                    End If
                Catch ex As Exception

                End Try

                Dim myQueueLen As Double = 0
                Try

                    If InStr(MyDominoServer.Statistics_Platform, "Platform.LogicalDisk." & n.ToString & ".AvgQue") Then

                        If InStr(MyDominoServer.OperatingSystem, "Windows") > 0 Then
                            myQueueLen = ParseNumericStatValue("Platform.LogicalDisk." & n.ToString & ".AvgQueueLen", MyDominoServer.Statistics_Platform)
                        Else
                            'Linux has Platform.LogicalDisk.1.AvgQueLen  instead
                            myQueueLen = ParseNumericStatValue("Platform.LogicalDisk." & n.ToString & ".AvgQueLen", MyDominoServer.Statistics_Platform)
                        End If

                        Try
                            myDiskDrive.DiskAverageQueueLength = myQueueLen
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error setting myDiskDrive.DiskAverageQueueLength: " & ex.ToString)
                            myQueueLen = 0
                        End Try

                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Platform.LogicalDisk." & n.ToString & ".AvgQueueLen is " & myQueueLen & ". Under 2 is good.")

                    Else
                        '   WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Platform.LogicalDisk." & n.ToString & ".AvgQueueLen was not found.")
                        '  WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & vbCrLf & MyDominoServer.Statistics_Platform & vbCrLf)
                    End If
                Catch ex As Exception
                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error checking Platform.LogicalDisk." & n.ToString & ".AvgQueueLen" & "  " & ex.ToString)
                End Try

                Try
                    If InStr(MyDominoServer.Statistics_Platform, "Platform.LogicalDisk." & n.ToString & ".PctUtil") Then
                        '   ResetAlert("Domino Server", MyDominoServer.Name, "Platform.LogicalDisk." & n.ToString & ".PctUtil")
                        Dim myPctUtil As Double
                        myPctUtil = ParseNumericStatValue("Platform.LogicalDisk." & n.ToString & ".PctUtil", MyDominoServer.Statistics_Platform)
                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Platform.LogicalDisk." & n.ToString & ".PctUtil is " & myPctUtil & ". Under 80 is good.")

                        If myPctUtil > 100 Then
                            myPctUtil = 0
                            '       QueueAlert("Domino Server", MyDominoServer.Name, "Platform.LogicalDisk." & n.ToString & ".AvgQueLen", " Platform.LogicalDisk." & n.ToString & ".PctUtil is " & myPctUtil & ".  This number should not consistently exceed 2 or performance problems will follow.  Consider upgrading this drive.")
                        End If


                        Try
                            myDiskNames = "Disk." & ParseTextStatValue("Platform.LogicalDisk." & n.ToString & ".AssignedName", MyDominoServer.Statistics_Platform)
                        Catch ex As Exception

                        End Try

                        Try
                            myDiskDrive = MyDominoServer.DiskDrives.Search(myDiskNames)
                        Catch ex As Exception
                            myDiskDrive = Nothing
                        End Try

                        If myDiskDrive Is Nothing And ParseNumericStatValue("Platform.LogicalDisk.TotalNumofDisks", MyDominoServer.Statistics_Platform) = 1 Then
                            If MyDominoServer.DiskDrives.Count = 1 Then
                                myDiskDrive = MyDominoServer.DiskDrives.Item(0)
                            End If
                        End If


                        If myDiskDrive Is Nothing Then
                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Adding a new drive to the collection.")
                            myDiskDrive = New MonitoredItems.DominoDiskSpace
                            myDiskDrive.DiskName = myDiskNames
                            MyDominoServer.DiskDrives.Add(myDiskDrive)
                        Else
                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Updating existing drive in the collection.")
                        End If

                        Try
                            myDiskDrive.DiskPercentUtilization = myPctUtil
                        Catch ex As Exception
                            myDiskDrive.DiskPercentUtilization = 0
                        End Try

                    End If
                Catch ex As Exception
                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error checking Platform.LogicalDisk." & n.ToString & ".PctUtil" & "  " & ex.ToString)
                End Try

skipdrive:

            Next n
        Else
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Platform statistics are not currently available for this server.")
        End If



        If MyDominoServer.Statistics_Disk = "" Then
            Exit Sub
        End If

        Dim PercentFree, MyDiskSize, MyFreeSpace As Double
        ' Dim myCommand As New OleDb.OleDbCommand
        ' Dim mySQLCommand As New Data.SqlClient.SqlCommand

        Dim myPath As String = ""



        '**********************
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " ----------------------------------------------------------------")
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Creating a new array of logical drive names using Disk Statistics")
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, vbCrLf)
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, MyDominoServer.Statistics_Disk)

        Dim DiskNames(70) As String
        '  Dim myDriveCount As Integer = 0
        Try
            Dim tempArray() As String
            tempArray = MyDominoServer.Statistics_Disk.Split(vbCrLf)
            Dim counter, myInt As Integer
            'Disk space


            ReDim DiskNames(tempArray.GetUpperBound(0))

            'Loop through the array and send the contents of the array to debug window.
            For counter = 0 To tempArray.GetUpperBound(0)
                myInt = InStr(tempArray(counter).ToUpper, ".FREE")
                If myInt > 0 Then
                    DiskNames(counter) = Trim(tempArray(counter).Remove(myInt - 1))
                    '   Debug.Print(tempArray(counter).Remove(myInt - 1))
                    DiskNames(counter) = DiskNames(counter).Replace("\n", "")
                    DiskNames(counter) = DiskNames(counter).Replace("\r", "")
                    DiskNames(counter) = DiskNames(counter).Replace(vbCrLf, "")
                    DiskNames(counter) = DiskNames(counter).Replace(vbCr, "")
                    DiskNames(counter) = DiskNames(counter).Replace(vbLf, "")
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Found a drive named " & Trim(DiskNames(counter)))
                    ' myDriveCount += 1
                    ' Debug.Print(tempArray(counter))
                End If
            Next
            ' ReDim Preserve DiskNames(myDriveCount)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error parsing drive names " & ex.ToString)
        End Try

        '**********************
        Dim boolAIX As Boolean = False
        Dim boolHSBC As Boolean = False

        If InStr(MyDominoServer.OperatingSystem, "AIX") Then
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This is an AIX box that will get special drive handling, if the client is HSBC.")
            boolAIX = True
        End If

        If InStr(MyDominoServer.Name.ToUpper, "HSBC") Then
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This is an HSBC server.")
            boolHSBC = True
        Else
            ' WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The client is not HSBC, so all disks will be processed.")
        End If



        For n As Integer = 0 To DiskNames.GetUpperBound(0) '32
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Analyzing free space on " & DiskNames(n))

            If InStr(MyDominoServer.Statistics_Disk, DiskNames(n) & ".Free") And DiskNames(n) <> "" Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " **********************")


                If boolAIX = True And boolHSBC = True Then
                    'This is an HSBC AIX box.  They only care about the data drive, which will have the server name in it.
                    Dim strServerName As String = ""
                    Try
                        '   WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The full server name is " & MyDominoServer.Name)
                        strServerName = Mid(MyDominoServer.Name, 1, InStr(MyDominoServer.Name, "/") - 1)
                        '    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " I figure that the data drive will have the word " & strServerName & " in its name.")
                    Catch ex As Exception
                        strServerName = MyDominoServer.Name
                    End Try

                    '   WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Testing to see if " & DiskNames(n).ToUpper & " is the data drive for " & strServerName.ToUpper)

                    If (InStr(DiskNames(n).ToUpper, strServerName.ToUpper) > 0) Then
                        '      WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This is the data drive. ")
                    Else
                        '           WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Skipping " & DiskNames(n) & " because this is an HSBC server and this is not the data drive.")
                        GoTo skipdrive2
                    End If


                End If

                Try
                    '************ FREE SPACE

                    strDiskSize = ParseTextStatValue(DiskNames(n) & ".Free", MyDominoServer.Statistics_Disk)
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The server reports disk FREE space of " & strDiskSize & " on " & DiskNames(n))

                    Try
                        'This function does NOT work if the number is given in exponential format, such as 1.0995084780E+12
                        If Not InStr(strDiskSize, "E") Then
                            MyDiskSize = ConvertNumberString(strDiskSize, MyDominoServer.Name)
                        End If
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error converting the disk size string " & ex.ToString)
                    End Try


                    Try
                        'This function DOES work for numbers in exponential format, such as 1.0995084780E+12
                        If InStr(strDiskSize, "E") Then
                            MyDiskSize = ConvertNumberInExponentialFormat(strDiskSize, MyDominoServer.Name)
                        End If
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error converting the disk size string " & ex.ToString)
                    End Try


                    Try
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " I converted the number string to this easy-to-convert string: " & MyDiskSize)
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception displaying the disk size string " & ex.ToString)
                    End Try

                    Try
                        MyFreeSpace = Convert.ToDouble(MyDiskSize)
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The converted free space is " & MyFreeSpace)
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error converting the free space " & ex.ToString)
                        GoTo skipdrive2
                    End Try

                    MyFreeSpace = MyFreeSpace / 1024 / 1024 / 1024
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Free space (in GB) is  " & MyFreeSpace.ToString("F1") & " GB")
                    ' myDiskDrive.DiskFreeInGB = MyFreeSpace

                Catch ex As Exception
                    GoTo skipdrive2
                End Try
                Try


                    '************ DISK SIZE
                    strDiskSize = ParseTextStatValue(DiskNames(n) & ".Size", MyDominoServer.Statistics_Disk)
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The server reports disk SIZE of " & strDiskSize)

                    Try
                        'This function does NOT work if the number is given in exponential format, such as 1.0995084780E+12
                        If Not InStr(strDiskSize, "E") Then
                            MyDiskSize = ConvertNumberString(strDiskSize, MyDominoServer.Name)
                        End If
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error converting the disk size string " & ex.ToString)
                    End Try


                    Try
                        'This function DOES work for numbers in exponential format, such as 1.0995084780E+12
                        If InStr(strDiskSize, "E") Then
                            MyDiskSize = ConvertNumberInExponentialFormat(strDiskSize, MyDominoServer.Name)
                        End If
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error converting the disk size string " & ex.ToString)
                    End Try

                    Try
                        'If you got a recognizable number from above, it should convert to a double here
                        MyDiskSize = Convert.ToDouble(MyDiskSize)
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The converted disk size is " & MyDiskSize)
                    Catch ex As Exception
                        GoTo skipdrive2
                    End Try


                    MyDiskSize = MyDiskSize / 1024 / 1024 / 1024  'convert to GB
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The disk size (in GB) is   " & MyDiskSize.ToString("F1") & " GB")

                Catch ex As Exception

                End Try

                Try
                    PercentFree = MyFreeSpace / MyDiskSize
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Dividing  " & MyFreeSpace.ToString("F1") & " GB by " & MyDiskSize.ToString("F1") & " GB gives free space percentage of " & PercentFree.ToString("P1"))
                Catch ex As Exception
                    PercentFree = 100
                End Try

                Try

                    Try
                        myDiskDrive = MyDominoServer.DiskDrives.Search(DiskNames(n))
                    Catch ex As Exception
                        myDiskDrive = Nothing
                    End Try

                    If (InStr(DiskNames(n), "opt/patrol")) Or (InStr(DiskNames(n), "domlog")) Then
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Skipping this drive... ")
                        GoTo skipdrive2
                    End If

                    Try
                        'Some basic validation to see if the numbers are plausible
                        If MyFreeSpace > MyDiskSize Then
                            GoTo skipdrive2
                        End If

                        If PercentFree > 100 Then
                            GoTo skipdrive2
                        End If

                        If DiskNames(n) = "" Then
                            GoTo skipdrive2
                        End If
                    Catch ex As Exception

                    End Try

                    If myDiskDrive Is Nothing Then
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Adding a new drive to the collection.")
                        myDiskDrive = New MonitoredItems.DominoDiskSpace
                        myDiskDrive.DiskName = DiskNames(n)
                        MyDominoServer.DiskDrives.Add(myDiskDrive)
                    Else
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Updating existing drive in the collection.")
                    End If

                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error creating Disk space collection " & ex.ToString)
                End Try



                Try
                    myDiskDrive.DiskSize = MyDiskSize
                    myDiskDrive.DiskFree = MyFreeSpace
                    myDiskDrive.PercentFree = PercentFree
                    myDiskDrive.DiskFreeInGB = MyFreeSpace
                    myDiskDrive.DiskSizeInGB = MyDiskSize
                    myDiskDrive.LastUpdated = Now
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error updating Disk space item " & ex.ToString)
                End Try

                'Check to see if this drive has a specific threshold set for it
                Try
                    For Each row As DataRow In dtDataTable.Rows
                        Dim myThreshold As Integer = row.Item("Threshold")
                        If myDiskDrive.DiskName = row.Item("DiskName") And boolNoAlerts = False And boolAllDrives = False And row.Item("Threshold") <> 0 Then
                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Set to monitor " & row.Item("DiskName") & " with a threshold of " & row.Item("Threshold") & " " & row.Item("ThresholdType"))
                            Select Case row.Item("ThresholdType")
                                Case "Percent"
                                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This drive has " & PercentFree * 100 & " % free space.")
                                    If PercentFree * 100 < row.Item("Threshold") Then
                                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This drive is in an alert condition. ")
                                        MyDominoServer.Status = "Low Disk Space"
                                        If InStr(MyDominoServer.Name, "MutalOMA") And Trim(DiskNames(n)) = "Disk.E" Then
                                            'QueueAlert("Domino Server", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The Domino server " & MyDominoServer.Name & " has " & Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#%") & " available space on drive " & DiskNames(n) & ". Note: This is the transaction logging drive. The threshold is " & MyDominoServer.DiskThreshold * 100 & "%")
                                            '3/4/2016 NS modified for VSPLUS-2682
                                            myAlert.QueueAlert("Domino", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The server " & MyDominoServer.Name & " has " & Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#") & "% available space on drive " & DiskNames(n) & ".  Note: This is the transaction logging drive.  The threshold is " & row.Item("Threshold") & "%", MyDominoServer.Location)

                                        Else
                                            ' QueueAlert("Domino Server", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The Domino server " & MyDominoServer.Name & " has " & Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#%") & " available space on drive " & DiskNames(n) & ". The threshold is " & MyDominoServer.DiskThreshold * 100 & "%")
                                            '3/4/2016 NS modified for VSPLUS-2682
                                            myAlert.QueueAlert("Domino", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The server " & MyDominoServer.Name & " has " & Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#") & "% available space on drive " & DiskNames(n) & ". The threshold is " & row.Item("Threshold") & "%", MyDominoServer.Location)
                                            ' MyDominoServer.ResponseDetails += " - " & Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#") & "% free space on " & DiskNames(n) & ". Threshold is " & Microsoft.VisualBasic.Strings.Format(row.Item("Threshold"), "##0.#" & "%")
                                            MyDominoServer.ResponseDetails += " | " & Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#") & "% free space on " & DiskNames(n) & ". Threshold is " & row.Item("Threshold").ToString & "%"

                                        End If
                                    Else
                                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This drive is not in an alert condition. ")
                                        myAlert.ResetAlert("Domino", MyDominoServer.Name, "Disk Space " & DiskNames(n), MyDominoServer.Location)
                                    End If
                                Case "GB"
                                    Try
                                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This drive has " & myDiskDrive.DiskFreeInGB & " GB free space.")
                                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This drive has a threshold of " & row.Item("Threshold") & " GB free space.")


                                        If myDiskDrive.DiskFreeInGB < row.Item("Threshold") Then
                                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This drive is in an alert condition. ")
                                            MyDominoServer.Status = "Low Disk Space"
                                            If InStr(MyDominoServer.Name, "MutalOMA") And Trim(DiskNames(n)) = "Disk.E" Then
                                                'QueueAlert("Domino Server", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The Domino server " & MyDominoServer.Name & " has " & Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#%") & " available space on drive " & DiskNames(n) & ". Note: This is the transaction logging drive. The threshold is " & MyDominoServer.DiskThreshold * 100 & "%")
                                                '3/4/2016 NS modified for VSPLUS-2682
                                                myAlert.QueueAlert("Domino", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The server " & MyDominoServer.Name & " has " & myDiskDrive.DiskFreeInGB.ToString("F2") & " GB available space on drive " & DiskNames(n) & ".  Note: This is the transaction logging drive.  The threshold is " & row.Item("Threshold") & " GB", MyDominoServer.Location)
                                                MyDominoServer.ResponseDetails += " | " & myDiskDrive.DiskFreeInGB.ToString("F2") & " GB free space on " & DiskNames(n) & ". Threshold is " & myThreshold.ToString("F2") & "%"

                                            Else
                                                ' QueueAlert("Domino Server", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The Domino server " & MyDominoServer.Name & " has " & Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#%") & " available space on drive " & DiskNames(n) & ". The threshold is " & MyDominoServer.DiskThreshold * 100 & "%")
                                                '3/4/2016 NS modified for VSPLUS-2682
                                                myAlert.QueueAlert("Domino", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The server " & MyDominoServer.Name & " has " & myDiskDrive.DiskFreeInGB.ToString("F2") & " GB available space on drive " & DiskNames(n) & ". The threshold is " & row.Item("Threshold") & " GB", MyDominoServer.Location)
                                                MyDominoServer.ResponseDetails += " | " & myDiskDrive.DiskFreeInGB.ToString("F2") & " GB free space on " & DiskNames(n) & ". Threshold is " & myThreshold.ToString("F2") & " GB"

                                            End If
                                        Else
                                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This drive is not in an alert condition. ")
                                            myAlert.ResetAlert("Domino", MyDominoServer.Name, "Disk Space " & DiskNames(n), MyDominoServer.Location, "This drive has " & myDiskDrive.DiskFreeInGB & " GB free space.")
                                        End If
                                    Catch ex As Exception
                                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception with disk space at #5: " & ex.ToString)
                                    End Try


                                Case Else

                            End Select

                        End If

                    Next row
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception looping through drive configuration: " & ex.ToString)
                End Try


                'Handle the case when all drives are monitored for the same threshold.
                If boolAllDrives = True And boolNoAlerts = False Then
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " All drives are evaluated on the same threshold.")
                    Select Case AllDrivesThresholdType
                        Case "Percent"
                            Try
                                If PercentFree * 100 < MyDominoServer.DiskThreshold And MyDominoServer.DiskThreshold <> 0 And Trim(DiskNames(n)) <> "" Then
                                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " My threshold is " & MyDominoServer.DiskThreshold)
                                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " My percent free is " & PercentFree)
                                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " My disk name is " & Trim(DiskNames(n)))
                                    If InStr(MyDominoServer.Name, "MutalOMA") And Trim(DiskNames(n)) = "Disk.E" Then
                                        'QueueAlert("Domino Server", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The Domino server " & MyDominoServer.Name & " has " & Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#%") & " available space on drive " & DiskNames(n) & ". Note: This is the transaction logging drive. The threshold is " & MyDominoServer.DiskThreshold * 100 & "%")
                                        '3/4/2016 NS modified for VSPLUS-2682
                                        myAlert.QueueAlert("Domino", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The server " & MyDominoServer.Name & " has " & Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#%") & " available space on drive " & DiskNames(n) & ".  Note: This is the transaction logging drive.  The threshold is " & MyDominoServer.DiskThreshold & "%", MyDominoServer.Location)

                                    Else
                                        ' QueueAlert("Domino Server", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The Domino server " & MyDominoServer.Name & " has " & Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#%") & " available space on drive " & DiskNames(n) & ". The threshold is " & MyDominoServer.DiskThreshold * 100 & "%")
                                        '3/4/2016 NS modified for VSPLUS-2682
                                        myAlert.QueueAlert("Domino", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The server " & MyDominoServer.Name & " has " & Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#%") & " available space on drive " & DiskNames(n) & ". The threshold is " & MyDominoServer.DiskThreshold & "%", MyDominoServer.Location)

                                    End If
                                    MyDominoServer.Status = "Low Disk Space"

                                    MyDominoServer.ResponseDetails += " | " & (PercentFree * 100).ToString("F1") & "% free space on " & DiskNames(n) & ". Threshold is " & MyDominoServer.DiskThreshold.ToString("F0") & "%"
                                    ' MyDominoServer.Description = Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#") & "% free space on " & DiskNames(n) & ". Threshold is " & Microsoft.VisualBasic.Strings.Format(MyDominoServer.DiskThreshold, "##0.#") & "% at " & Now.ToShortTimeString
                                    UpdateDominoStatusTable(MyDominoServer)
                                Else
                                    'ResetAlert("Domino Server", MyDominoServer.Name, "Disk Space " & DiskNames(n))
                                    myAlert.ResetAlert("Domino", MyDominoServer.Name, "Disk Space " & DiskNames(n), MyDominoServer.Location, (PercentFree * 100).ToString("F1") & "% free space on " & DiskNames(n))
                                End If


                            Catch ex As Exception
                                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error calculating disk space " & ex.ToString)

                            End Try
                        Case "GB"
                            Try
                                If myDiskDrive.DiskFreeInGB < MyDominoServer.DiskThreshold And MyDominoServer.DiskThreshold <> 0 And Trim(DiskNames(n)) <> "" Then

                                    If InStr(MyDominoServer.Name, "MutalOMA") And Trim(DiskNames(n)) = "Disk.E" Then
                                        'QueueAlert("Domino Server", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The Domino server " & MyDominoServer.Name & " has " & Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#%") & " available space on drive " & DiskNames(n) & ". Note: This is the transaction logging drive. The threshold is " & MyDominoServer.DiskThreshold * 100 & "%")
                                        '3/4/2016 NS modified for VSPLUS-2682
                                        myAlert.QueueAlert("Domino", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The server " & MyDominoServer.Name & " has " & myDiskDrive.DiskFreeInGB.ToString("F2") & " GB available space on drive " & DiskNames(n) & ".  Note: This is the transaction logging drive.  The threshold is " & MyDominoServer.DiskThreshold & " GB", MyDominoServer.Location)

                                    Else
                                        ' QueueAlert("Domino Server", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The Domino server " & MyDominoServer.Name & " has " & Microsoft.VisualBasic.Strings.Format(PercentFree, "##0.#%") & " available space on drive " & DiskNames(n) & ". The threshold is " & MyDominoServer.DiskThreshold * 100 & "%")
                                        '3/4/2016 NS modified for VSPLUS-2682
                                        myAlert.QueueAlert("Domino", MyDominoServer.Name, "Disk Space " & DiskNames(n), "The server " & MyDominoServer.Name & " has " & myDiskDrive.DiskFreeInGB.ToString("F2") & " GB available space on drive " & DiskNames(n) & ". The threshold is " & MyDominoServer.DiskThreshold & " GB", MyDominoServer.Location)

                                    End If
                                    MyDominoServer.Status = "Low Disk Space"

                                    MyDominoServer.ResponseDetails += " | " & myDiskDrive.DiskFreeInGB.ToString("F2") & " GB free space on " & DiskNames(n) & ". Threshold is " & MyDominoServer.DiskThreshold & "GB"
                                    'MyDominoServer.Description = myDiskDrive.DiskFreeInGB & " GB free space on " & DiskNames(n) & ". Threshold is " & myDiskDrive.DiskFreeInGB.ToString("F2") & " at " & Now.ToShortTimeString
                                    UpdateDominoStatusTable(MyDominoServer)
                                Else
                                    'ResetAlert("Domino Server", MyDominoServer.Name, "Disk Space " & DiskNames(n))
                                    myAlert.ResetAlert("Domino", MyDominoServer.Name, "Disk Space " & DiskNames(n), MyDominoServer.Location, myDiskDrive.DiskFreeInGB.ToString("F2") & " GB free")
                                End If


                            Catch ex As Exception
                                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error calculating disk space " & ex.ToString)

                            End Try
                    End Select


                End If



            Else
                ' WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Server does not have a disk named " & DiskNames(n))
            End If

skipdrive2:
        Next


        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " ********* Disk space summary   **************")
        Dim listOfDisks As New List(Of VSNext.Mongo.Entities.Disk)()
        For Each myDiskDrive In MyDominoServer.DiskDrives

            If MyLogLevel = LogLevel.Verbose And Trim(myDiskDrive.DiskName) <> "" Then
                Try
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & myDiskDrive.DiskName & ".Free is " & myDiskDrive.DiskFree)
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & myDiskDrive.DiskName & ".Size is " & myDiskDrive.DiskSize)
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The disk threshold type is " & myDiskDrive.ThresholdType)
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & myDiskDrive.DiskName & " Percent Free is " & Microsoft.VisualBasic.Strings.Format(myDiskDrive.PercentFree, "##0.#%"))
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & myDiskDrive.DiskName & " percent utilization is " & myDiskDrive.DiskPercentUtilization)
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & myDiskDrive.DiskName & " Average Queue length " & myDiskDrive.DiskAverageQueueLength)
                    ' WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & DiskNames(n) & " Percent Free is " & PercentFree * 100 & "%")
                Catch ex As Exception

                End Try

                If myDiskDrive.PercentFree < MyDominoServer.DiskThreshold And MyDominoServer.DiskThreshold <> 0 Then
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Drive " & myDiskDrive.DiskName & " is running out of space.")
                Else
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Drive " & myDiskDrive.DiskName & " has plenty of disk space.")
                End If
            End If

            'Write the results to the database
            strSQL = ""
            '4/7/2016 NS modified for VSPLUS-2790
            Dim strUpdate As String = ""
            Dim strInsert As String = ""
            Dim objVSAdaptor As New VSAdaptor
            Try
                If myDiskDrive.DiskSize <> 0 And myDiskDrive.DiskName.Trim <> "" Then
                    listOfDisks.Add(New VSNext.Mongo.Entities.Disk With {
                                    .DiskFree = myDiskDrive.DiskFree,
                                    .DiskName = myDiskDrive.DiskName,
                                    .DiskSize = myDiskDrive.DiskSize,
                                    .PercentFree = myDiskDrive.PercentFree,
                                    .AverageQueueLength = myDiskDrive.DiskAverageQueueLength,
                                    .DiskThreshold = MyDominoServer.DiskThreshold
                                    })

                Else

                    listOfDisks.Add(New VSNext.Mongo.Entities.Disk With {
                                    .DiskName = myDiskDrive.DiskName,
                                    .PercentFree = myDiskDrive.PercentFree,
                                    .AverageQueueLength = myDiskDrive.DiskAverageQueueLength
                                    })
                End If

                ' WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " My Disk Space SQL UPDATE statement is : " & vbCrLf & strSQL)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error in Domino disk space creating SQL statement for status table: " & ex.Message & vbCrLf & strSQL)
            End Try
        Next

        Try
            Dim MyDominoServer2 As MonitoredItems.DominoServer = MyDominoServer2
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.TypeAndName = MyDominoServer2.Name & "-Domino")
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.Disks, listOfDisks)
            repository.Update(filterDef, updateDef)
        Catch ex As Exception

        End Try

        If MyLogLevel = LogLevel.Verbose Then
            'WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " My disk statistics are: " & vbCrLf & MyDominoServer.Statistics_Disk & vbCrLf)
        End If

    End Sub

    Private Function ConvertNumberString(strDiskSize As String, ServerName As String) As String
        'This function takes a string representing disk space number, such as 1,0995084780E+12 and returns it as a number without exponential notation
        WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " received " & strDiskSize & " for parsing.", LogLevel.Verbose)

        If InStr(strDiskSize, "'") Then
            WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Parsing out the ' character", LogLevel.Verbose)
            strDiskSize = strDiskSize.Replace("'", "")
        End If

        If InStr(strDiskSize, ".") Then
            WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Parsing out the . character", LogLevel.Verbose)
            strDiskSize = strDiskSize.Replace(".", "")
        End If

        If InStr(strDiskSize, " ") Then
            WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Parsing out the extra spaces", LogLevel.Verbose)
            strDiskSize = strDiskSize.Replace("'", "")
        End If

        If InStr(strDiskSize, "ÿ") Then
            strDiskSize = strDiskSize.Replace("ÿ", "")
        End If

        WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Returning " & strDiskSize & " for parsing.", LogLevel.Verbose)
        Return strDiskSize

    End Function

    Private Function ConvertNumberInExponentialFormat(strDiskSize As String, ServerName As String) As String
        'This function takes a string representing disk space number, such as 1,0995084780E+12 and returns it as a number without exponential notation
        WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " received " & strDiskSize & " for parsing.", LogLevel.Verbose)
        Dim USprovider As IFormatProvider = Globalization.CultureInfo.CreateSpecificCulture("en-us")
        Dim Europrovider As IFormatProvider = Globalization.CultureInfo.CreateSpecificCulture("fr-FR")


        Dim myNumber As Decimal

        Try
            myNumber = Decimal.Parse(strDiskSize, System.Globalization.NumberStyles.Float)
        Catch ex As FormatException
            Try
                myNumber = Decimal.Parse(strDiskSize, System.Globalization.NumberStyles.Float, Europrovider)
            Catch ex2 As Exception
                myNumber = strDiskSize
            End Try
        Catch ex As Exception
            myNumber = strDiskSize
        End Try



        WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Returning " & strDiskSize & " for parsing.", LogLevel.Verbose)
        Return myNumber.ToString

    End Function


#End Region

#Region "Domino Mail Related"


    Private Sub GetDeadandPendingMailWrapper(ByRef MyDominoServer As MonitoredItems.DominoServer)
        Dim start, done As Long
        Dim elapsed As TimeSpan
        Dim span As System.TimeSpan
        start = Now.Ticks

        'Domino Objects
        Dim LocalNotesSession As New Domino.NotesSession
        Dim db As Domino.NotesDatabase
        Dim docMail As Domino.NotesDocument
        Dim NotesView As Domino.NotesView

        '6/22/2015 NS added for VSPLUS-1475
        Dim NotesSystemMessageString As String = "Incorrect Notes Password."
        Try
            Dim myPassword = GetNotesPassword()
            LocalNotesSession.Initialize(myPassword)
            myPassword = ""
            GetDeadandPendingMail(MyDominoServer, LocalNotesSession, db, docMail, NotesView)
        Catch ex As Exception
            '6/22/2015 NS added for VSPLUS-1475
            System.Runtime.InteropServices.Marshal.ReleaseComObject(LocalNotesSession)
            myAlert.QueueSysMessage(NotesSystemMessageString)
            WriteAuditEntry(Now.ToString & " Error Initializing NotesSession.  Many problems will follow....  " & ex.ToString)
            WriteAuditEntry(Now.ToString & " *********** ERROR ************  Error initializing a session to Query Domino server. ")
            WriteAuditEntry(Now.ToString & " Calling stopnotescl.exe then exiting in an attempt to recover.")
            WriteAuditEntry(Now.ToString & " The VitalSigns Master service should restart the monitoring service in a few moments.")
            KillNotes()
            Exit Sub
        End Try


        Try
            If Not (IsNothing(db)) Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
            End If
        Catch ex As Exception

        End Try


        Try
            If Not (IsNothing(NotesView)) Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(NotesView)
            End If
        Catch ex As Exception

        End Try

        Try
            If Not (IsNothing(db)) Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
            End If
        Catch ex As Exception

        End Try

        Try
            If Not (IsNothing(LocalNotesSession)) Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(LocalNotesSession)
            End If
        Catch ex As Exception

        End Try

        done = Now.Ticks
        elapsed = New TimeSpan(done - start)


        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString + ": Time to calculate dead/pending/held mail = " & elapsed.TotalMilliseconds & " ms")
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString + ": Time to calculate dead/pending/held mail = " & elapsed.TotalSeconds & " seconds")
    End Sub

    Private Sub GetDeadandPendingMail(ByRef MyDominoServer As MonitoredItems.DominoServer, ByRef Session As Domino.NotesSession, ByRef db As Domino.NotesDatabase, ByRef docMail As Domino.NotesDocument, ByRef NotesView As Domino.NotesView)

        Dim boolMailError As Boolean = False
        ' boolMailError will track if there was an error getting mail values.
        ' if so, don't clear any existing mail alerts

        Try
            If InStr(MyDominoServer.Statistics_Server, "Server.MailBoxes") > 0 Then
                MyDominoServer.MailboxCount = ParseNumericStatValue("Server.MailBoxes", MyDominoServer.Statistics_Server)
            End If
        Catch ex As Exception

        End Try


        dtDominoLastUpdate = Now
        Dim MailBoxCount As Integer
        Try
            MailBoxCount = MyDominoServer.MailboxCount
        Catch ex As Exception
            MailBoxCount = 1
        End Try

        If MailBoxCount = 0 Then MailBoxCount = 1

        If MailBoxCount = 999 Then
            'Mailbox count is not set yet
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString + ": Error checking mail.box files.  Count is still set to startup value of 999.")
            Exit Sub
        End If

        'Set this value to true if you exit the loop to count the messages
        Dim boolStoppedCounting As Boolean = False

        'The values we calculate
        Dim DeadCount As Integer = 0
        Dim PendingCount As Integer = 0
        Dim HeldCount As Integer = 0

        'The values reported by the server stat table
        Dim ServerDeadCount As Integer = 0
        Dim ServerPendingCount As Integer = 0
        Dim ServerHeldCount As Integer = 0
        Dim myAccessLevel As String = ""


        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & MyDominoServer.Name & " has " & MailBoxCount & " mailbox(s)")

        Dim myRoutingState As String
        Dim Counter As Integer
        Dim MailboxName As String

        For Counter = 1 To MailBoxCount
            If Counter = 1 And MailBoxCount = 1 Then
                MailboxName = "mail.box"
            Else
                MailboxName = "mail" & Counter.ToString & ".box"
            End If

            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Attempting to open " & MailboxName & "  on " & MyDominoServer.Name)


            Try
                db = NotesSession.GetDatabase(MyDominoServer.Name, MailboxName, False)
                If String.IsNullOrWhiteSpace(db.Title) Then
                    db.Title = "Untitled Database"
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error checking if db name is blank.  Error: " & ex.Message)
            End Try

            dtDominoLastUpdate = Now
            Try
                If db.IsOpen Then
                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " opened Mailbox named " & db.Title & " on " & MyDominoServer.Name)
                    NotesView = db.GetView("Mail")
                    ' If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " opened Mailbox 'inbox' named " & NotesView.Name)
                    Try
                        Select Case db.QueryAccess(NotesSession.UserName)
                            Case Domino.ACLLEVEL.ACLLEVEL_NOACCESS
                                myAccessLevel = "No Access"
                            Case Domino.ACLLEVEL.ACLLEVEL_DEPOSITOR
                                myAccessLevel = "Depositor"
                            Case Domino.ACLLEVEL.ACLLEVEL_READER
                                myAccessLevel = "Reader"
                            Case Domino.ACLLEVEL.ACLLEVEL_AUTHOR
                                myAccessLevel = "Author"
                            Case Domino.ACLLEVEL.ACLLEVEL_EDITOR
                                myAccessLevel = "Editor"
                            Case Domino.ACLLEVEL.ACLLEVEL_DESIGNER
                                myAccessLevel = "Designer"
                            Case Domino.ACLLEVEL.ACLLEVEL_MANAGER
                                myAccessLevel = "Manager"
                            Case Else
                                myAccessLevel = "Undefined"
                        End Select
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & NotesSession.CommonUserName & " has " & myAccessLevel & " access to " & db.FileName)
                    Catch ex As Exception
                        myAccessLevel = "Error"
                    End Try

                    If myAccessLevel = "Depositor" Or myAccessLevel = "No Access" Then

                        MyDominoServer.ResponseDetails = "Insufficient access to " & MailboxName & " file.  You have " & myAccessLevel & " but at least Reader required."
                        MyDominoServer.Description = "Insufficient access to " & MailboxName & " file.  You have " & myAccessLevel & " but at least Reader required."

                        If InStr(MyDominoServer.Statistics_Mail.ToUpper, "MAIL.WAITING") > 0 Then
                            Try
                                MyDominoServer.PendingMail = ParseNumericStatValue("Mail.Waiting", MyDominoServer.Statistics_Mail)
                                MyDominoServer.DeadMail = ParseNumericStatValue("Mail.Dead", MyDominoServer.Statistics_Mail)
                                MyDominoServer.HeldMail = ParseNumericStatValue("Mail.Hold", MyDominoServer.Statistics_Mail)
                            Catch ex2 As Exception
                            End Try
                        Else
                            MyDominoServer.PendingMail = GetDominoNumericStatistic(MyDominoServer.Name, "Mail", "Waiting")
                            MyDominoServer.HeldMail = GetDominoNumericStatistic(MyDominoServer.Name, "Mail", "Hold")
                            MyDominoServer.DeadMail = GetDominoNumericStatistic(MyDominoServer.Name, "Mail", "Dead")
                        End If

                    End If

                    Try
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Now examining the messages.... ")

                        docMail = NotesView.GetFirstDocument
                        While Not (docMail Is Nothing)
                            'Thread.Sleep(25)
                            Try
                                myRoutingState = ""
                                If docMail.HasItem("RoutingState") Then
                                    myRoutingState = docMail.GetItemValue("RoutingState")(0)  'read the value of the RoutingState field
                                    If myRoutingState = "DEAD" Then
                                        DeadCount += 1
                                    ElseIf myRoutingState = "HOLD" Then
                                        HeldCount += 1
                                    Else
                                        PendingCount += 1
                                    End If
                                Else
                                    PendingCount += 1
                                End If
                                If MyDominoServer.MailChecking = 1 Then
                                    If PendingCount > MyDominoServer.PendingThreshold Or HeldCount > MyDominoServer.HeldThreshold Then
                                        boolStoppedCounting = True
                                        Exit While
                                    End If

                                End If
                                docMail = NotesView.GetNextDocument(docMail)

                            Catch ex As Exception
                                'Exception reading the Routing State 
                                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception processing " & MailboxName & ":  " & ex.ToString)
                            End Try

                        End While
                    Catch ex2 As Exception
                        ' Error to see if there is not documents at all. 
                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Has No document in " & MailboxName & ":  " & ex2.ToString)
                    End Try

                Else
                    'Cannot Open the Database so log it and move on.
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Could not Open the database " & MailboxName & ":  ")
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Failed Processing Mail box count for  " & MailboxName & ": " & ex.ToString)
            End Try
            'move on to the next mailbox file, if any
        Next

        Try
            ' Here we ask the server what it thinks the values for dead, pending, and held are
            If InStr(MyDominoServer.Statistics_Mail.ToUpper, "MAIL.WAITING") > 0 Then
                Try
                    ServerPendingCount = ParseNumericStatValue("Mail.Waiting", MyDominoServer.Statistics_Mail)
                    ServerDeadCount = ParseNumericStatValue("Mail.Dead", MyDominoServer.Statistics_Mail)
                    ServerHeldCount = ParseNumericStatValue("Mail.Hold", MyDominoServer.Statistics_Mail)
                Catch ex2 As Exception
                    boolMailError = True
                End Try
            Else
                Try
                    ServerPendingCount = GetDominoNumericStatistic(MyDominoServer.Name, "Mail", "Waiting")
                    ServerHeldCount = GetDominoNumericStatistic(MyDominoServer.Name, "Mail", "Hold")
                    ServerDeadCount = GetDominoNumericStatistic(MyDominoServer.Name, "Mail", "Dead")
                Catch ex2 As Exception
                    boolMailError = True
                End Try
            End If

        Catch ex2 As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Failed to get Mailbox Counts : " & ex2.ToString)
        Finally
            'If we have stopped counting AND if the server reports higher numbers than our counts, then take the server numbers
            'If we have not stopped counting then we will use our own calculations over what the server reports
            If boolStoppedCounting = True Then
                If PendingCount < ServerPendingCount Then MyDominoServer.PendingMail = ServerPendingCount Else MyDominoServer.PendingMail = PendingCount
                If HeldCount < ServerHeldCount Then MyDominoServer.HeldMail = ServerHeldCount Else MyDominoServer.HeldMail = HeldCount
                If DeadCount < ServerDeadCount Then MyDominoServer.DeadMail = ServerDeadCount Else MyDominoServer.DeadMail = DeadCount
            Else
                MyDominoServer.PendingMail = PendingCount
                MyDominoServer.HeldMail = HeldCount
                MyDominoServer.DeadMail = DeadCount
            End If

        End Try

        Try
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Done analyzing mail boxes... ")
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Pending mail: " & MyDominoServer.PendingMail)
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Dead Mail: " & MyDominoServer.DeadMail)
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Held Mail: " & MyDominoServer.HeldMail)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CompareMailThresholds(ByRef MyDominoServer As MonitoredItems.DominoServer)

        Try
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Comparing Mail Values with Thresholds...")
        Catch ex As Exception

        End Try

        Try
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Prior Pending mail: " & MyDominoServer.PriorPendingMail)
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Pending mail: " & MyDominoServer.PendingMail)
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Dead Mail: " & MyDominoServer.DeadMail & " Dead mail threshold: " & MyDominoServer.DeadThreshold)
        Catch ex As Exception

        End Try


        '** Dead  Messages
        Try
            If MyDominoServer.DeadMail >= MyDominoServer.DeadThreshold And MyDominoServer.DeadMail > 0 And MyDominoServer.DeadThreshold > 0 Then
                MyDominoServer.Status = "Dead Mail Alert"
                MyDominoServer.AlertType = DeadMail
                '3/1/2016 NS modified for VSPLUS-2682
                myAlert.QueueAlert("Domino", MyDominoServer.Name, "Dead Mail", "The server " & MyDominoServer.Name & " has " & MyDominoServer.DeadMail & " dead messages.", MyDominoServer.Location)
                ' myAlert.QueueAlert("Domino", MyDominoServer.Name, "Dead Mail", "The Domino server " & MyDominoServer.Name & " has " & MyDominoServer.DeadMail & " dead messages.", MyDominoServer.Location)
            Else
                '3/1/2016 NS modified for VSPLUS-2682
                myAlert.ResetAlert("Domino", MyDominoServer.Name, "Dead Mail", MyDominoServer.Location, "The server has " & MyDominoServer.DeadMail & " dead messages.")
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " No problem with dead mail, Dead Mail= " & MyDominoServer.DeadMail & " Dead mail threshold= " & MyDominoServer.DeadThreshold)
            End If
        Catch ex As Exception
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception calculating dead mail alert: " & ex.ToString)
        End Try

        Try
            If MyDominoServer.DeleteDeadThreshold > 0 And MyDominoServer.DeadMail > 0 Then
                'only delete dead mail if set to a number greater than zero
                If MyDominoServer.DeadMail >= MyDominoServer.DeleteDeadThreshold Then
                    dtDominoLastUpdate = Now
                    Dim intMessagesDeleted As Integer = 0
                    Try
                        intMessagesDeleted = DeleteDeadMail(MyDominoServer)
                        If intMessagesDeleted > 0 Then
                            MyDominoServer.Status = "OK"
                            MyDominoServer.DeadMail = 0
                        End If

                    Catch ex As Exception

                    End Try
                    '3/4/2016 NS modified for VSPLUS-2682
                    myAlert.QueueAlert("Domino", MyDominoServer.Name, "Dead Mail Deletion", "The server " & MyDominoServer.Name & " had dead mail of " & MyDominoServer.DeadMail & " messages, which exceeded the automatic deletion threshold.  VitalSigns attempted to automatically delete the dead mail on this server. " & intMessagesDeleted & " messages were deleted.", MyDominoServer.Location)
                End If
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error deleting dead mail:  " & ex.Message)
        End Try

        '** Held Messages
        Try
            If MyDominoServer.HeldMail >= MyDominoServer.HeldThreshold And MyDominoServer.HeldThreshold > 0 Then
                MyDominoServer.Status = "Held Mail Alert"
                MyDominoServer.AlertType = HeldMail
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " No problem with Held mail, Held Mail= " & MyDominoServer.HeldMail & " Held mail threshold= " & MyDominoServer.HeldThreshold)
                Dim strmsg As String = ""
                If MyDominoServer.MailChecking = 1 Then
                    '3/4/2016 NS modified for VSPLUS-2682
                    strmsg = "The server " & MyDominoServer.Name & " has at least " & MyDominoServer.HeldMail.ToString & " held messages.  This server is configured to stop counting when the threshold is exceeded."
                Else
                    '3/4/2016 NS modified for VSPLUS-2682
                    strmsg = "The server " & MyDominoServer.Name & " has " & MyDominoServer.HeldMail.ToString & " held messages."
                End If

                myAlert.QueueAlert("Domino", MyDominoServer.Name, "Held Mail", strmsg, MyDominoServer.Location)
            Else
                '3/1/2016 NS modified for VSPLUS-2682
                myAlert.ResetAlert("Domino", MyDominoServer.Name, "Held Mail", MyDominoServer.Location, "The server has " & MyDominoServer.HeldMail & " held messages.")
            End If
        Catch ex As Exception
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception calculating Held mail alert: " & ex.ToString)
        End Try

        'Pending Mail Alerts get queued from this one place
        Try
            If MyDominoServer.PendingMail >= MyDominoServer.PendingThreshold And MyDominoServer.PendingThreshold > 0 Then
                MyDominoServer.Status = "Pending Mail Alert"
                MyDominoServer.AlertCondition = True
                MyDominoServer.AlertType = MonitoredItems.MonitoredDevice.Alert.PendingMail
                MyDominoServer.Description = "Successfully connected to the server at " & Date.Now.ToShortTimeString & ", but pending mail is over the threshold."
                ' MyDominoServer.Description = "Successfully connected to the server at " & Now.ToShortTimeString

                Dim strmsg As String = ""
                If MyDominoServer.MailChecking = 1 Then
                    '3/4/2016 NS modified for VSPLUS-2682
                    strmsg = "The server " & MyDominoServer.Name & " has at least " & MyDominoServer.PendingMail.ToString & " pending messages.  This server is configured to stop counting when the threshold is exceeded."
                Else
                    '3/4/2016 NS modified for VSPLUS-2682
                    strmsg = "The server " & MyDominoServer.Name & " has " & MyDominoServer.PendingMail.ToString & " pending messages."
                End If
                myAlert.QueueAlert("Domino", MyDominoServer.Name, "Pending Mail", strmsg, MyDominoServer.Location)

                'myAlert.QueueAlert("Domino", MyDominoServer.Name, "Pending Mail", "The Domino server " & MyDominoServer.Name & " has " & MyDominoServer.PendingMail & " pending messages.", MyDominoServer.Location)
                If MyDominoServer.DeadMail >= MyDominoServer.DeadThreshold Then
                    'both dead and pending are over
                    MyDominoServer.Status = "Pending and Dead Mail Alert"
                End If

            Else
                myAlert.ResetAlert("Domino", MyDominoServer.Name, "Pending Mail", MyDominoServer.Location, "Server has " & MyDominoServer.PendingMail & " pending messages")
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Function DeleteDeadMail(ByRef server As MonitoredItems.DominoServer) As Integer
        Dim DeadCount As Integer = 0
        Dim PendingCount As Integer = 0

        Dim db As Domino.NotesDatabase
        Dim docMail As Domino.NotesDocument
        Dim PreviousDocMail As Domino.NotesDocument
        Dim NotesView As Domino.NotesView

        Try

            Dim myRoutingState As String
            '   WriteAuditEntry("Entered dead and pending as " & Session.UserName)
            Dim MailBoxCount As Integer
            MailBoxCount = server.MailboxCount

            ' WriteAuditEntry(server.Name & " has " & MailBoxCount & " mailbox(s)")
            Select Case MailBoxCount
                Case 1  '1 mailbox, open mail.box
                    WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " Attempting to clean up dead mail in mail.box on " & server.Name)


                    Try
                        db = NotesSession.GetDatabase(server.Name, "mail.box", False)
                        If db.IsOpen Then
                            Try
                                If String.IsNullOrWhiteSpace(db.Title) Then
                                    db.Title = "Untitled Database"
                                End If
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " Error checking if db name is blank.  Error: " & ex.Message)
                            End Try
                            WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " opened Mailbox named " & db.Title & " on " & server.Name)
                            NotesView = db.GetView("Mail")
                            WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " opened Mailbox 'inbox' named " & NotesView.Name)
                            docMail = NotesView.GetFirstDocument
                            Try
                                While Not (docMail Is Nothing)
                                    myRoutingState = ""
                                    If docMail.HasItem("RoutingState") Then
                                        myRoutingState = docMail.GetItemValue("RoutingState")(0)

                                        If myRoutingState = "DEAD" Or myRoutingState = "HOLD" Then
                                            PreviousDocMail = docMail
                                            DeadCount += 1
                                        Else
                                            PreviousDocMail = Nothing
                                        End If
                                    End If
                                    docMail = NotesView.GetNextDocument(docMail)
                                    If Not PreviousDocMail Is Nothing Then
                                        Try
                                            Call PreviousDocMail.Remove(True)
                                        Catch ex As Exception
                                            WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " Error deleting dead mail message: " & ex.Message)
                                        End Try

                                    End If

                                End While

                            Catch ex As Exception
                                WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " Error deleting dead mail messages on " & db.Title & " on " & server.Name & ". Error is " & ex.Message)
                                '   server.Status = "Insufficient Access"
                                '   server.ResponseDetails = "Insufficient access to mail.box file.  Editor, Can Delete required to delete dead mail."
                            Finally
                                WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " Deleted " & DeadCount & " Dead Mail messages from " & server.Name & ".")
                                NotesView = Nothing
                                docMail = Nothing
                                PreviousDocMail = Nothing
                                db = Nothing
                                server.DeadMail = 0
                            End Try

                        Else
                            WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " ERROR: Attempted to open mail.box" & server.Name & " failed.")
                            server.Status = "Insufficient Access"
                            server.ResponseDetails = "Insufficient access to mail.box file.  Editor, Can Delete required to delete dead mail."
                        End If
                    Catch ex As Exception
                        Try
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(docMail)
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(PreviousDocMail)
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(NotesView)
                        Catch ex2 As Exception

                        End Try
                    End Try


                Case Else  'Two or more mail boxes

                    Dim Counter As Integer
                    Dim MailboxName As String
                    WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " Attempting to clean Dead Mail messages from multiple mailboxes on " & server.Name)
                    For Counter = 1 To MailBoxCount
                        MailboxName = "mail" & Counter.ToString & ".box"
                        WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " Attempting to clean dead messages in " & MailboxName & "  on " & server.Name)
                        db = NotesSession.GetDatabase(server.Name, MailboxName, False)
                        'Reset counters for each mailbox
                        DeadCount = 0
                        Try
                            If db.IsOpen Then

                                Try
                                    If String.IsNullOrWhiteSpace(db.Title) Then
                                        db.Title = "Untitled Database"
                                    End If
                                Catch ex As Exception
                                    WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " Error checking if db name is blank.  Error: " & ex.Message)
                                End Try

                                WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " opened Mailbox named " & db.Title & " on " & server.Name)
                                NotesView = db.GetView("Mail")
                                WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " opened Mailbox 'Inbox' named " & NotesView.Name)
                                Try
                                    docMail = NotesView.GetFirstDocument
                                Catch ex As Exception

                                End Try

                                Try
                                    While Not (docMail Is Nothing)
                                        myRoutingState = ""
                                        If docMail.HasItem("RoutingState") Then
                                            myRoutingState = docMail.GetItemValue("RoutingState")(0)

                                            If myRoutingState = "DEAD" Or myRoutingState = "HOLD" Then
                                                PreviousDocMail = docMail
                                                DeadCount += 1
                                            Else
                                                PreviousDocMail = Nothing
                                            End If
                                        End If
                                        docMail = NotesView.GetNextDocument(docMail)
                                        If Not PreviousDocMail Is Nothing Then
                                            Try
                                                Call PreviousDocMail.Remove(True)
                                            Catch ex As Exception
                                                WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " Error deleting dead mail message: " & ex.Message)
                                            End Try

                                        End If

                                    End While

                                Catch ex As Exception
                                    WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " Error deleting dead mail messages on " & db.Title & " on " & server.Name & ". Error is " & ex.Message)
                                    'server.Status = "Insufficient Access"
                                    '          server.ResponseDetails = "Insufficient access to mail.box file.  Editor, Can Delete required to delete dead mail."
                                Finally
                                    WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " Deleted " & DeadCount & " Dead Mail messages from " & server.Name & ".")
                                    server.DeadMail = 0
                                End Try
                            Else
                                'could not open the database

                                WriteAuditEntry("Attempt to clean up dead messages on server: " & server.Name & " failed.")
                            End If
                        Catch ex As Exception
                            ' server.Status = "Insufficient Access"
                            '    server.ResponseDetails = "Insufficient access to mail.box file.  Editor, Can Delete required to delete dead mail."
                        End Try

                    Next

            End Select
            WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " Finished cleaning up Dead Mail from mailboxes...")
        Catch ex As Exception
            WriteAuditEntry(server.Name & " error: " * ex.Message)
        End Try

        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(docMail)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(PreviousDocMail)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(NotesView)
        Catch ex As Exception

        End Try

        WriteDeviceHistoryEntry("Domino", server.Name, Now.ToString & " leaving Clean Up Dead Mail routine...")

        Return DeadCount
    End Function

    Private Sub TrackDominoMailStatistics(ByRef DominoServer As MonitoredItems.DominoServer)
        'This sub calculates the daily volume of mail traffic
        dtDominoLastUpdate = Now
        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "*---- Tracking values of mail-related statistics ----*")


        Dim RoutedMailCurrentStatValue, DeliveredCurrentStatValue, TransferredMailCurrentStatValue, SMTPMailCurrentStatValue, MailFailuresCurrentStatValue As Long
        Dim RoutedMail, DeliveredMail, TransferredMail, SMTPMessages, MailFailures As Long

        Try
            If InStr(DominoServer.Statistics_Mail.ToUpper, "Mail.Delivered".ToUpper) > 0 Then
                Try
                    DeliveredCurrentStatValue = ParseNumericStatValue("Mail.Delivered", DominoServer.Statistics_Mail)
                Catch ex As Exception
                    DeliveredCurrentStatValue = 0
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Mail.Delivered from stat table. Error was " & ex.Message)
                End Try
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Current value of Mail.Delivered stat is " & DeliveredCurrentStatValue)

                Try
                    DominoServer.PreviousDeliveredMail = ReadSettingValue(DominoServer.Name & "-PreviousDeliveredMail")
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Previous value of Mail.Delivered " & DominoServer.PreviousDeliveredMail)

                Catch ex As Exception
                    DominoServer.PreviousDeliveredMail = 0
                End Try

                Try
                    DeliveredMail = DeliveredCurrentStatValue - DominoServer.PreviousDeliveredMail
                    WriteAuditEntry(Now.ToString & " " & DominoServer.Name & " current value of calculated delivered mail is " & DeliveredMail)
                    If DeliveredMail < 0 Then
                        DeliveredMail = 0
                    End If
                Catch ex As Exception
                    DeliveredMail = 0
                End Try


                Try
                    '1/4/2016 NS modified for VSPLUS-2434
                    'WriteSettingsValue(DominoServer.Name & "-PreviousDeliveredMail", DeliveredCurrentStatValue)
                    If MailStatsDict.ContainsKey(DominoServer.Name & "-PreviousDeliveredMail") Then
                        MailStatsDict(DominoServer.Name & "-PreviousDeliveredMail") = DeliveredCurrentStatValue
                    Else
                        MailStatsDict.Add(DominoServer.Name & "-PreviousDeliveredMail", DeliveredCurrentStatValue)
                    End If
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Incremental Delivered Mail value is " & DeliveredMail)
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Value of Previous delivered mail is " & DominoServer.PreviousDeliveredMail)
                    UpdateDominoDailyStatTable(DominoServer, "Mail.Delivered", DeliveredMail)
                Catch ex As Exception

                End Try

            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error calculating Mail.Delivered " & ex.Message)
        End Try



        Try
            If InStr(DominoServer.Statistics_Mail.ToUpper, "SMTP.MessagesProcessed".ToUpper) > 0 Then
                SMTPMailCurrentStatValue = ParseNumericStatValue("SMTP.MessagesProcessed", DominoServer.Statistics_Mail)
                Try
                    DominoServer.PreviousSMTPMessages = ReadSettingValue(DominoServer.Name & "-PreviousSMTPMessages")
                Catch ex As Exception
                    DominoServer.PreviousSMTPMessages = 0
                End Try

                SMTPMessages = SMTPMailCurrentStatValue - DominoServer.PreviousSMTPMessages
                If SMTPMessages < 0 Then
                    SMTPMessages = 0
                End If
                DominoServer.PreviousSMTPMessages = SMTPMailCurrentStatValue
                UpdateDominoDailyStatTable(DominoServer, "SMTP.MessagesProcessed", SMTPMessages)
                '1/4/2016 NS modified for VSPLUS-2434
                'WriteSettingsValue(DominoServer.Name & "-PreviousSMTPMessages", SMTPMailCurrentStatValue)
                If MailStatsDict.ContainsKey(DominoServer.Name & "-PreviousSMTPMessages") Then
                    MailStatsDict(DominoServer.Name & "-PreviousSMTPMessages") = SMTPMailCurrentStatValue
                Else
                    MailStatsDict.Add(DominoServer.Name & "-PreviousSMTPMessages", SMTPMailCurrentStatValue)
                End If
            Else
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " SMTP.MessagesProcessed is not found in this server's stat table.")
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & ": Error calculating SMTP Messages " & ex.Message)
        End Try

        Try
            If InStr(DominoServer.Statistics_Mail, "Mail.TransferFailures") > 0 Then
                MailFailuresCurrentStatValue = ParseNumericStatValue("Mail.TransferFailures", DominoServer.Statistics_Mail)
                Try
                    DominoServer.PreviousMailFailures = ReadSettingValue(DominoServer.Name & "-PreviousMailFailures")
                Catch ex As Exception
                    DominoServer.PreviousMailFailures = 0
                End Try

                MailFailures = MailFailuresCurrentStatValue - DominoServer.PreviousMailFailures
                If MailFailures < 0 Then
                    MailFailures = 0
                End If
                DominoServer.PreviousMailFailures = MailFailuresCurrentStatValue
                '   WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & ": Mail.TransferFailures value is " & DominoServer.MailTransferFailures)
                UpdateDominoDailyStatTable(DominoServer, "Mail.TransferFailures", MailFailures)
                '1/4/2016 NS modified for VSPLUS-2434
                'WriteSettingsValue(DominoServer.Name & "-PreviousMailFailures", MailFailuresCurrentStatValue)
                If MailStatsDict.ContainsKey(DominoServer.Name & "-PreviousMailFailures") Then
                    MailStatsDict(DominoServer.Name & "-PreviousMailFailures") = MailFailuresCurrentStatValue
                Else
                    MailStatsDict.Add(DominoServer.Name & "-PreviousMailFailures", MailFailuresCurrentStatValue)
                End If
            Else
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Mail.TransferFailures is not found in this server's stat table.")
            End If

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " " & DominoServer.Name & ": Error calculating Mail.TransferFailures " & ex.Message)
        End Try


        Try
            If InStr(DominoServer.Statistics_Mail, "Mail.TotalRouted") > 0 Then

                Try
                    RoutedMailCurrentStatValue = ParseNumericStatValue("Mail.TotalRouted", DominoServer.Statistics_Mail)
                Catch ex As Exception
                    RoutedMailCurrentStatValue = 0
                End Try

                Try
                    DominoServer.PreviousRoutedMail = ReadSettingValue(DominoServer.Name & "-PreviousRoutedMail")
                Catch ex As Exception
                    DominoServer.PreviousRoutedMail = 0
                End Try


                Try
                    RoutedMail = RoutedMailCurrentStatValue - DominoServer.PreviousRoutedMail
                    If RoutedMail < 0 Then
                        RoutedMail = 1
                    End If
                Catch ex As Exception
                    RoutedMail = 0
                End Try

                UpdateDominoDailyStatTable(DominoServer, "Mail.TotalRouted", RoutedMail)
                '1/4/2016 NS modified for VSPLUS-2434
                'WriteSettingsValue(DominoServer.Name & "-PreviousRoutedMail", RoutedMailCurrentStatValue)
                If MailStatsDict.ContainsKey(DominoServer.Name & "-PreviousRoutedMail") Then
                    MailStatsDict(DominoServer.Name & "-PreviousRoutedMail") = RoutedMailCurrentStatValue
                Else
                    MailStatsDict.Add(DominoServer.Name & "-PreviousRoutedMail", RoutedMailCurrentStatValue)
                End If
            End If


        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " " & DominoServer.Name & ": Error calculating Mail.TotalRouted " & ex.Message)
        End Try


        Try
            If InStr(DominoServer.Statistics_Mail.ToUpper, "Mail.Transferred".ToUpper) > 0 Then
                Try
                    TransferredMailCurrentStatValue = ParseNumericStatValue("Mail.Transferred", DominoServer.Statistics_Mail)
                Catch ex As Exception
                    TransferredMailCurrentStatValue = 0
                End Try

                Try
                    DominoServer.PreviousTransferredMail = ReadSettingValue(DominoServer.Name & "-PreviousTransferredMail")
                Catch ex As Exception
                    DominoServer.PreviousTransferredMail = 0
                End Try

                Try
                    TransferredMail = TransferredMailCurrentStatValue - DominoServer.PreviousTransferredMail
                Catch ex As Exception
                    TransferredMail = 0
                End Try

                If TransferredMail < 0 Then
                    TransferredMail = 0
                End If
                DominoServer.PreviousTransferredMail = TransferredMailCurrentStatValue
                '1/4/2016 NS modified for VSPLUS-2434
                'WriteSettingsValue(DominoServer.Name & "-PreviousTransferredMail", TransferredMailCurrentStatValue)
                If MailStatsDict.ContainsKey(DominoServer.Name & "-PreviousTransferredMail") Then
                    MailStatsDict(DominoServer.Name & "-PreviousTransferredMail") = TransferredMailCurrentStatValue
                Else
                    MailStatsDict.Add(DominoServer.Name & "-PreviousTransferredMail", TransferredMailCurrentStatValue)
                End If
            Else
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Mail.Transferred is not found in this server's stat table.")

            End If
            ' WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & ": Incremental Transferred Mail value is " & DominoServer.TransferredMail)
            UpdateDominoDailyStatTable(DominoServer, "Mail.Transferred", TransferredMail)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " " & DominoServer.Name & ": Error calculating Mail.Transferred " & ex.Message)
        End Try


        Try
            Dim AvgMailDeliver As Integer
            If InStr(DominoServer.Statistics_Mail.ToUpper, "Mail.AverageDeliverTime".ToUpper) > 0 Then
                Try
                    AvgMailDeliver = ParseNumericStatValue("Mail.AverageDeliverTime", DominoServer.Statistics_Mail)
                    UpdateDominoDailyStatTable(DominoServer, "Mail.AverageDeliverTime", AvgMailDeliver)
                Catch ex As Exception
                    AvgMailDeliver = 0
                End Try

            End If

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " " & DominoServer.Name & ": Error calculating Mail.AverageDeliverTime " & ex.Message)
        End Try

        Try
            Dim myNumber As Integer
            If InStr(DominoServer.Statistics_Mail.ToUpper, "Mail.AverageSizeDelivered".ToUpper) > 0 Then
                Try
                    myNumber = ParseNumericStatValue("Mail.AverageSizeDelivered", DominoServer.Statistics_Mail)
                    'This is in KBytes so convert to MBytes
                    'myNumber = myNumber / 1024
                    UpdateDominoDailyStatTable(DominoServer, "Mail.AverageSizeDelivered", myNumber)
                Catch ex As Exception
                    myNumber = 0
                End Try

            End If

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " " & DominoServer.Name & ": Error calculating Mail.AverageSizeDelivered " & ex.Message)
        End Try

        Try
            Dim myNumber As Integer
            If InStr(DominoServer.Statistics_Mail.ToUpper, "Mail.AverageServerHops".ToUpper) > 0 Then
                Try
                    myNumber = ParseNumericStatValue("Mail.AverageServerHops", DominoServer.Statistics_Mail)
                    UpdateDominoDailyStatTable(DominoServer, "Mail.AverageServerHops", myNumber)
                Catch ex As Exception
                    myNumber = 0
                End Try

            End If

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " " & DominoServer.Name & ": Error calculating Mail.AverageServerHops " & ex.Message)
        End Try

    End Sub

    'Not being used
    'Private Function CountMailFiles(ByRef s As Domino.NotesSession, ByRef MyDominoServer As MonitoredItems.DominoServer) As Integer
    '    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Entered Mail File Count for  " & MyDominoServer.Name)
    '    'Don't do this right away, it is too slow

    '    Try
    '        Dim AllScanned As Boolean = True
    '        For Each Server As MonitoredItems.DominoServer In MyDominoServers
    '            If Server.Status = "Not Scanned" Then
    '                AllScanned = False
    '                WriteAuditEntry(Now.ToString & " Mail File Analysis is deferred until all Domino servers have been scanned.", LogLevel.Verbose)
    '                Exit Function
    '            End If
    '        Next

    '    Catch ex As Exception

    '    End Try

    '    If MyDominoServer.Status = "Maintenance" Then
    '        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exiting Mail File Count because the server is in a maintenance window.")
    '        Return 0
    '        Exit Function
    '    End If

    '    Dim myRegistry As New RegistryHandler

    '    'This subroutine counts, details, and summarizes the number of files in the \mail directory
    '    If MyDominoServer.Enabled = False Then
    '        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exiting Mail File Count because the server is not enabled.")
    '        myRegistry.WriteToRegistry("MailFileScanDate_" & MyDominoServer.Name, Now.ToShortDateString)
    '        Return 0
    '        Exit Function
    '    End If

    '    If MyDominoServer.CountMailFiles = False Or MyDominoServer.MailDirectory = "None" Then
    '        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exiting Mail File Count because mail file directory is " & MyDominoServer.MailDirectory & " or count mail files=" & MyDominoServer.CountMailFiles)
    '        myRegistry.WriteToRegistry("MailFileScanDate_" & MyDominoServer.Name, Now.ToShortDateString)
    '        Return 0
    '        Exit Function
    '    End If

    '    '  Dim s As New Domino.NotesSession
    '    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Searching " & MyDominoServer.Name & " for mail files in the " & MyDominoServer.MailDirectory & " directory.")
    '    Dim MyCount As Integer
    '    Dim MyTotalSize As Long
    '    Dim dbDir As Domino.NotesDbDirectory
    '    Dim db As Domino.NotesDatabase
    '    Dim a As Domino.NotesAgent
    '    Dim myCommand As New OleDb.OleDbCommand
    '    Dim myConnection As New OleDb.OleDbConnection
    '    'Dim myAdapter As New OleDb.OleDbDataAdapter
    '    Dim myPath As String

    '    'Read the registry for the location of the mailfilestats Database

    '    Try
    '        myPath = myRegistry.ReadFromRegistry("Application Path")
    '        ' WriteAuditEntry(Now.ToString & " Domino Update e database " & myPath)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " Failed to read registry in Domino count mail files module. Exception: " & ex.Message)
    '    End Try

    '    If myPath Is Nothing Then
    '        WriteAuditEntry(Now.ToString & " Error: Failed to read registry in Domino count maile files module.   Cannot locate Config Database 'mailfilestats.mdb'.  Configure by running" & ProductName & " client before starting the service.")
    '        '   Return False
    '        Exit Function
    '    End If
    '    '   myRegistry = Nothing

    '    With myConnection
    '        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Opening mailfilestats on " & myPath & "data\mailfilestats.mdb")
    '        .ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & myPath & "data\mailfilestats.mdb"
    '        .Open()
    '    End With

    '    Do Until myConnection.State = ConnectionState.Open
    '        myConnection.Open()
    '    Loop

    '    'myCommand.Connection = myConnection
    '    '***

    '    'Delete any existing data that is older than a week, as only the current data matters
    '    Dim StrSQL As String
    '    Try
    '        StrSQL = "DELETE FROM Daily WHERE ScanDate<=#" & FixDate(Today.AddDays(-7)) & "# AND MailServer='" & MyDominoServer.Name & "'"
    '        'myCommand.CommandText = StrSQL
    '        'myCommand.ExecuteNonQuery()

    '        'WRITTEN BY MUKUND 28Feb12
    '        Dim objVSAdaptor As New VSAdaptor
    '        objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "NSFHealth", StrSQL)
    '    Catch ex As Exception
    '        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Count Mail Files delete command failed because: " & ex.Message)
    '        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The failed Count Mail Files delete  command was " & StrSQL)
    '    End Try

    '    'myConnection.Close()
    '    'myConnection.Dispose()
    '    'myCommand.Dispose()

    '    Try
    '        dbDir = s.GetDbDirectory(MyDominoServer.Name)
    '    Catch ex As Exception
    '        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error: Domino Monitoring CountMailboxes routine failed to initiate NotesdbDirectory: " & ex.Message)
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(dbDir)
    '        Exit Function
    '    End Try

    '    If dbDir Is Nothing Then
    '        MyCount = Nothing
    '        MyTotalSize = Nothing
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(dbDir)
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(a)
    '        Exit Function
    '    End If

    '    Try
    '        db = dbDir.GetFirstDatabase(Domino.DB_TYPES.NOTES_DATABASE)
    '    Catch ex As Exception
    '        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error: Domino Monitoring CountMailboxes routine failed to access the first database: " & ex.Message)
    '        MyCount = Nothing
    '        MyTotalSize = Nothing
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(dbDir)
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(a)

    '        Exit Function
    '    End Try


    '    Dim FT As Boolean = False
    '    Dim OOO As Boolean = False
    '    Dim ClusterRep As Boolean = False
    '    Dim ReplicaID As String
    '    Dim ODS As Double
    '    myRegistry.WriteToRegistry("MailFileScanDate_" & MyDominoServer.Name, Now.ToShortDateString)

    '    Try
    '        While Not (db Is Nothing)
    '            If InStr(db.FilePath.ToUpper, MyDominoServer.MailDirectory.ToUpper, CompareMethod.Text) Then
    '                Try
    '                    If String.IsNullOrWhiteSpace(db.Title) Then
    '                        db.Title = "Untitled Database"
    '                    End If
    '                Catch ex As Exception
    '                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error checking if db name is blank.  Error: " & ex.Message)
    '                End Try


    '                MyCount += 1
    '                MyTotalSize += (db.Size / 1024 / 1024)
    '                ' WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Examining mail file " & db.Server & " | " & db.FilePath)

    '                FT = False
    '                OOO = False
    '                ClusterRep = False
    '                ReplicaID = ""
    '                ODS = 0

    '                Dim Quota As Long
    '                Try
    '                    Quota = db.SizeQuota / 1024
    '                Catch ex As Exception
    '                    Quota = 0
    '                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error calculating Quota for " & db.Server & " | " & db.FilePath & " Error: " & ex.Message)
    '                End Try

    '                Try
    '                    ReplicaID = db.ReplicaID
    '                Catch ex As Exception
    '                    ReplicaID = ""
    '                End Try

    '                If MyDominoServer.AdvancedMailScan = True Then
    '                    Try
    '                        If Not (db.IsOpen) Then
    '                            db.Open()
    '                        End If
    '                    Catch ex As Exception
    '                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error Opening Database: " & db.Server & " | " & db.FilePath & " Error: " & ex.Message)
    '                    End Try

    '                    Try
    '                        FT = db.IsFTIndexed
    '                    Catch ex As Exception
    '                        FT = False
    '                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error determining if database " & db.Server & " | " & db.FilePath & " is FTIndexed: " & ex.Message)
    '                    End Try


    '                    Try
    '                        ODS = db.FileFormat
    '                    Catch ex As Exception
    '                        ODS = 0
    '                    End Try

    '                    Try
    '                        a = db.GetAgent("OutOfOffice")
    '                        If Not a Is Nothing Then
    '                            ' WriteAuditEntry(Now.ToString & " Examining agent " & a.Name & " Enabled: " & a.IsEnabled)
    '                            If InStr(a.Name, "OutOfOffice") Then
    '                                OOO = a.IsEnabled
    '                            End If
    '                        End If
    '                    Catch ex As Exception
    '                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error calculating status of OOO Agent for " & db.Server & " | " & db.FilePath & " Error: " & ex.Message)
    '                        OOO = False
    '                    End Try

    '                    Try
    '                        '  WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Cluster Replication for " & db.Server & " | " & db.FilePath & " is " & db.IsClusterReplication)
    '                        ClusterRep = db.IsClusterReplication
    '                    Catch ex As Exception
    '                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error calculating status of Enabled for Cluster Replication for " & db.Server & " | " & db.FilePath & " Error: " & ex.Message)
    '                        ClusterRep = False
    '                    End Try

    '                End If

    '                Dim mySize As Double
    '                Try
    '                    mySize = db.Size / 1024 / 1024  'Convert to MB
    '                Catch ex As Exception
    '                    mySize = 0
    '                End Try

    '                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Selected for Mail file count: " & db.Title & " (" & db.FilePath & " ) Size: " & db.Size / 1024 & " Template: " & db.DesignTemplateName & " Quota: " & db.SizeQuota & " Rep ID: " & ReplicaID)
    '                '  WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Selected " & db.Title & " " & db.DesignTemplateName & " Q: " & Quota & " FT: " & FT & " OOO: " & OOO & " CR: " & ClusterRep & " ID: " & ReplicaID & " size " & mySize)

    '                Try
    '                    UpdateDominoDailyMailFileStatTable(Date.Now, MyDominoServer.Name, db.FilePath, db.Title, mySize, db.DesignTemplateName, Quota, FT, OOO, ClusterRep, ReplicaID, ODS)
    '                Catch ex As Exception
    '                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error calling stat update: " & ex.Message)
    '                End Try

    '            Else
    '                ' WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Rejected for Mail file count: " & db.Title & " (" & db.FilePath & ")")
    '            End If
    '            db = dbDir.GetNextDatabase
    '            dtDominoLastUpdate = Now
    '        End While

    '        Try
    '            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & "  Domino Monitoring finished counting Mail files on " & MyDominoServer.Name & " Found " & MyCount & " consuming " & MyTotalSize & " MB")
    '            myRegistry.WriteToRegistry("MailFileScanDate_" & MyDominoServer.Name, Now.ToShortDateString)
    '        Catch ex As Exception

    '        End Try

    '    Catch ex As Exception
    '        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error: Domino Monitoring CountMailFiles routine failed while looping through databases: " & ex.Message)
    '        MyCount = Nothing
    '        MyTotalSize = Nothing
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(dbDir)
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(a)

    '        myRegistry.WriteToRegistry("MailFileScanDate_" & MyDominoServer.Name, Now.AddDays(-1).ToShortDateString)
    '        Return 0
    '        Exit Function
    '    Finally
    '        myRegistry = Nothing
    '    End Try


    '    Try
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(dbDir)
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(a)
    '    Catch ex As Exception

    '    End Try

    '    Try
    '        MyDominoServer.MailFileCount = MyCount
    '        UpdateDominoDailyMailFileSummaryTable(Date.Now, MyDominoServer.Name, MyCount, MyTotalSize)
    '    Catch ex As Exception

    '    End Try

    '    Return MyCount
    'End Function

    'Private Function GetDominoMailDotBoxCount(ByVal ServerName As String) As Integer
    '    Dim MailBoxCount As Integer
    '    Try
    '        MailBoxCount = GetDominoNumericStatistic(ServerName, "Server", "mailboxes")   'Figure out how many mailboxes
    '        If MailBoxCount = 0 Then MailBoxCount = 1
    '        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Mailbox count returned: " & MailBoxCount)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " ERROR: Domino monitor module error determining mailbox count for " & ServerName & " Error: " & ex.Message)
    '        MailBoxCount = 999
    '    End Try

    '    Return MailBoxCount
    'End Function

    Private Sub UpdateMailHealth(ByRef MyDominoServer As MonitoredItems.DominoServer)
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString + ": Calculating overall mail health statistics")
        Dim strSQL As String = ""

        Dim Domino_Domain As String = ""
        Try
            Domino_Domain = ParseTextStatValue("Mail.Domain", MyDominoServer.Statistics_Mail)
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString + ": This server is in the " & Domino_Domain & " domain.")
        Catch ex As Exception
            Domino_Domain = ""
        End Try

        Dim Mailbox_PerformanceIndex As Double = MyDominoServer.MailBoxPerformanceIndex
        Dim Mail_Pending As Integer = MyDominoServer.PendingMail
        Dim Mail_Dead As Integer = MyDominoServer.DeadMail
        Dim PendingThreshold As Integer = MyDominoServer.PendingThreshold
        Dim DeadThreshold As Integer = MyDominoServer.DeadThreshold


        Dim Mail_Waiting As Double = 0
        Try
            Mail_Waiting = ParseNumericStatValue("Mail.Waiting", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_Waiting = 0
        End Try

        Dim Mail_MaximiumSizeDelivered As Double = 0
        Try
            Mail_MaximiumSizeDelivered = ParseNumericStatValue("Mail.MaximumSizeDelivered", MyDominoServer.Statistics_Mail)
            'WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString + " Mail.MaximumSizeDelivered in KB is " & Mail_MaximiumSizeDelivered)
            Mail_MaximiumSizeDelivered = Mail_MaximiumSizeDelivered / 1024 ' convert to MB
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString + " Mail.MaximumSizeDelivered in MB is " & Mail_MaximiumSizeDelivered)
        Catch ex As Exception
            Mail_MaximiumSizeDelivered = 0
        End Try

        Dim Mail_AverageDeliveryTime As Double = 0
        Try
            Mail_AverageDeliveryTime = ParseNumericStatValue("Mail.AverageDeliverTime", MyDominoServer.Statistics_Mail)
            ' Mail_AverageDeliveryTime = Mail_AverageDeliveryTime
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString + " Mail.AverageDeliverTime  is " & Mail_AverageDeliveryTime)
        Catch ex As Exception
            Mail_AverageDeliveryTime = 0
        End Try

        Dim Mail_AverageSizeDelivered As Double = 0
        Try
            Mail_AverageSizeDelivered = ParseNumericStatValue("Mail.AverageSizeDelivered", MyDominoServer.Statistics_Mail)
            Mail_AverageSizeDelivered = Mail_AverageSizeDelivered / 1024 ' convert to MB
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString + " Mail.AverageSizeDelivered is " & Mail_AverageSizeDelivered)
        Catch ex As Exception
            Mail_AverageSizeDelivered = 0
        End Try

        Dim Mail_AverageServerHops As Double = 0
        Try
            Mail_AverageServerHops = ParseNumericStatValue("Mail.AverageServerHops", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_AverageServerHops = 0
        End Try


        Dim Mail_Transferred As Double = 0
        Try
            Mail_Transferred = ParseNumericStatValue("Mail.Transferred", MyDominoServer.Statistics_Mail)
            If Mail_Transferred = -999 Then
                Mail_Transferred = 0
            End If
        Catch ex As Exception
            Mail_Transferred = 0
        End Try



        Dim Mail_Delivered As Double = 0
        Try
            Mail_Delivered = ParseNumericStatValue("Mail.Delivered", MyDominoServer.Statistics_Mail)
            If Mail_Delivered = -999 Then
                Mail_Delivered = 0
            End If

        Catch ex As Exception
            Mail_Delivered = 0
        End Try


        Dim Mail_Routed As Double = 0
        Try
            Mail_Routed = ParseNumericStatValue("Mail.TotalRouted", MyDominoServer.Statistics_Mail)

            If Mail_Routed = -999 Then
                Mail_Routed = 0
            End If
        Catch ex As Exception
            Mail_Routed = 0
        End Try



        Dim Mail_DeliveredSize_100KB_to_1MB As Double = 0
        Try
            Mail_DeliveredSize_100KB_to_1MB = ParseNumericStatValue("Mail.DeliveredSize.100KB_to_1MB", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_DeliveredSize_100KB_to_1MB = 0
        End Try

        Dim Mail_DeliveredSize_10KB_to_100KB As Double = 0
        Try
            Mail_DeliveredSize_10KB_to_100KB = ParseNumericStatValue("Mail.DeliveredSize.10KB_to_100K", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_DeliveredSize_10KB_to_100KB = 0
        End Try

        Dim Mail_DeliveredSize_10MB_to_100MB As Double = 0
        Try
            Mail_DeliveredSize_10MB_to_100MB = ParseNumericStatValue("Mail.DeliveredSize.10MB_to_100MB", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_DeliveredSize_10MB_to_100MB = 0
        End Try

        Dim Mail_DeliveredSize_1KB_to_10KB As Double = 0
        Try
            Mail_DeliveredSize_1KB_to_10KB = ParseNumericStatValue("Mail.DeliveredSize.1KB_to_10KB", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_DeliveredSize_1KB_to_10KB = 0
        End Try

        Dim Mail_DeliveredSize_1MB_to_10MB As Double = 0
        Try
            Mail_DeliveredSize_1MB_to_10MB = ParseNumericStatValue("Mail.DeliveredSize.1MB_to_10MB", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_DeliveredSize_1MB_to_10MB = 0
        End Try

        Dim Mail_DeliveredSize_Under_1KB As Double = 0
        Try
            Mail_DeliveredSize_Under_1KB = ParseNumericStatValue("Mail.DeliveredSize.Under_1KB", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_DeliveredSize_Under_1KB = 0
        End Try

        Dim Mail_PeakMessageDeliveryTime As String = "n/a"
        Try
            Mail_PeakMessageDeliveryTime = ParseTextStatValue("Mail.PeakMessageDeliveryTime", MyDominoServer.Statistics_Mail)
            If Mail_PeakMessageDeliveryTime = "-999" Then
                Mail_PeakMessageDeliveryTime = "n/a"
            End If
        Catch ex As Exception
            Mail_PeakMessageDeliveryTime = "n/a"
        End Try

        Dim Mail_PeakMessagesDelivered As Double = 0
        Try
            Mail_PeakMessagesDelivered = ParseNumericStatValue("Mail.PeakMessagesDelivered", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_PeakMessagesDelivered = 0
        End Try

        Dim Mail_PeakMessagesTransferred As Double = 0
        Try
            Mail_PeakMessagesTransferred = ParseNumericStatValue("Mail.PeakMessagesTransferred", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_PeakMessagesTransferred = 0
        End Try


        Dim Mail_Transferred_NRPC As Double = 0
        Try
            Mail_Transferred_NRPC = ParseNumericStatValue("Mail.Transferred.NRPC", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_Transferred_NRPC = 0
        End Try

        Dim Mail_Transferred_SMTP As Double = 0
        Try
            Mail_Transferred_SMTP = ParseNumericStatValue("Mail.Transferred.SMTP", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_Transferred_SMTP = 0
        End Try

        Dim Mail_TransferThreads_Active As Double = 0
        Try
            Mail_TransferThreads_Active = ParseNumericStatValue("Mail.TransferThreads.Active", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_TransferThreads_Active = 0
        End Try

        Dim Mail_WaitingForDeliveryRetry As Double = 0
        Try
            Mail_WaitingForDeliveryRetry = ParseNumericStatValue("Mail.WaitingForDeliveryRetry", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_WaitingForDeliveryRetry = 0
        End Try


        Dim Mail_WaitingForDNS As Double = 0
        Try
            Mail_WaitingForDNS = ParseNumericStatValue("Mail.WaitingForDNS", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_WaitingForDNS = 0
        End Try

        Dim Mail_WaitingRecipients As Double = 0
        Try
            Mail_WaitingRecipients = ParseNumericStatValue("Mail.WaitingRecipients", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_WaitingRecipients = 0
        End Try

        Dim Mail_WaitingForDIR As Double = 0
        Try
            Mail_WaitingForDIR = ParseNumericStatValue("Mail.WaitingForDIR", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_WaitingForDIR = 0
        End Try

        Dim Mail_PeakMessageTransferredTime As String = "n/a"
        Try
            Mail_PeakMessageTransferredTime = ParseTextStatValue("Mail.PeakMessageTransferredTime", MyDominoServer.Statistics_Mail)
            If Mail_PeakMessageTransferredTime = "-999" Then
                Mail_PeakMessageTransferredTime = "n/a"
            End If
        Catch ex As Exception
            Mail_PeakMessageTransferredTime = "n/a"
        End Try

        Dim Mail_RecallFailures As Double = 0
        Try
            Mail_RecallFailures = ParseNumericStatValue("Mail.RecallFailures", MyDominoServer.Statistics_Mail)
        Catch ex As Exception
            Mail_RecallFailures = 0
        End Try

        Dim MailHeld As Integer = 0
        Try
            MailHeld = MyDominoServer.HeldMail
        Catch ex As Exception
            MailHeld = 0
        End Try


        Dim heldThreshold As Integer = 50
        Try
            heldThreshold = MyDominoServer.HeldThreshold
        Catch ex As Exception
            heldThreshold = 50
        End Try

        Dim sqlStatement As New System.Text.StringBuilder
        With MyDominoServer
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.TypeAndName = .Name & "-Domino")
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                 .Set(Function(x) x.Domain, Domino_Domain) _
                                                                                 .Set(Function(x) x.MailboxCount, MyDominoServer.MailboxCount) _
                                                                                 .Set(Function(x) x.MailboxPerformanceIndex, IIf(Mailbox_PerformanceIndex = 0, Convert.ToInt32(Mailbox_PerformanceIndex), MongoDB.Bson.BsonNull.Value)) _
                                                                                 .Set(Function(x) x.MailPending, .PendingMail) _
                                                                                 .Set(Function(x) x.PendingThreshold, .PendingThreshold) _
                                                                                 .Set(Function(x) x.MailDead, .DeadMail) _
                                                                                 .Set(Function(x) x.MailHeld, .HeldMail) _
                                                                                 .Set(Function(x) x.HeldMailThreshold, heldThreshold) _
                                                                                 .Set(Function(x) x.DeadThreshold, .DeadThreshold) _
                                                                                 .Set(Function(x) x.MailWaiting, Convert.ToInt32(Mail_Waiting)) _
                                                                                 .Set(Function(x) x.MailAverageSizeDelivered, IIf(Mail_AverageSizeDelivered = 0, Convert.ToInt32(Mail_AverageSizeDelivered), MongoDB.Bson.BsonNull.Value)) _
                                                                                 .Set(Function(x) x.MailAverageDeliveryTime, IIf(Mail_AverageDeliveryTime = 0, Convert.ToInt32(Mail_AverageDeliveryTime), MongoDB.Bson.BsonNull.Value)) _
                                                                                 .Set(Function(x) x.MailPeakMessageDeliveryTime, IIf(Mail_PeakMessageDeliveryTime <> "n/a" And Mail_PeakMessageDeliveryTime <> "", Convert.ToInt32(Mail_PeakMessageDeliveryTime), MongoDB.Bson.BsonNull.Value)) _
                                                                                 .Set(Function(x) x.MailPeakMessageTransferredTime, IIf(Mail_PeakMessageTransferredTime <> "n/a" And Mail_PeakMessageTransferredTime <> "", Convert.ToInt32(Mail_PeakMessageTransferredTime), MongoDB.Bson.BsonNull.Value)) _
                                                                                 .Set(Function(x) x.MailMaximiumSizeDelivered, IIf(Mail_MaximiumSizeDelivered = 0, Convert.ToInt32(Mail_MaximiumSizeDelivered), MongoDB.Bson.BsonNull.Value)) _
                                                                                 .Set(Function(x) x.MailPeakMessagesDelivered, IIf(Mail_PeakMessagesDelivered = 0, Convert.ToInt32(Mail_PeakMessagesDelivered), MongoDB.Bson.BsonNull.Value)) _
                                                                                 .Set(Function(x) x.MailAverageServerHops, IIf(Mail_AverageServerHops = 0, Convert.ToInt32(Mail_AverageServerHops), MongoDB.Bson.BsonNull.Value)) _
                                                                                 .Set(Function(x) x.MailTransferred, IIf(Mail_Transferred = 0, Convert.ToInt32(Mail_Transferred), MongoDB.Bson.BsonNull.Value)) _
                                                                                 .Set(Function(x) x.MailDelivered, IIf(Mail_Delivered = 0, Convert.ToInt32(Mail_Delivered), MongoDB.Bson.BsonNull.Value)) _
                                                                                 .Set(Function(x) x.MailRouted, IIf(Mail_Routed = 0, Convert.ToInt32(Mail_Routed), MongoDB.Bson.BsonNull.Value)) _
                                                                                 .Set(Function(x) x.MailWaitingForDeliveryRetry, Convert.ToInt32(Mail_WaitingForDeliveryRetry)) _
                                                                                 .Set(Function(x) x.MailWaitingForDNS, Convert.ToInt32(Mail_WaitingForDNS)) _
                                                                                 .Set(Function(x) x.MailWaitingForDIR, Convert.ToInt32(Mail_WaitingForDIR)) _
                                                                                 .Set(Function(x) x.MailWaitingRecipients, Convert.ToInt32(Mail_WaitingRecipients)) _
                                                                                 .Set(Function(x) x.MailTransferredNRPC, Convert.ToInt32(Mail_Transferred_NRPC)) _
                                                                                 .Set(Function(x) x.MailTransferredSMTP, Convert.ToInt32(Mail_Transferred_SMTP)) _
                                                                                 .Set(Function(x) x.MailRecallFailures, Convert.ToInt32(Mail_RecallFailures)) _
                                                                                 .Set(Function(x) x.MailTransferThreadsActive, Convert.ToInt32(Mail_TransferThreads_Active)) _
                                                                                 .Set(Function(x) x.MailPeakMessagesTransferred, Convert.ToInt32(Mail_PeakMessagesTransferred)) _
                                                                                 .Set(Function(x) x.MailDeliveredSize_100KB_to_1MB, Convert.ToInt32(Mail_DeliveredSize_100KB_to_1MB)) _
                                                                                 .Set(Function(x) x.MailDeliveredSize_10KB_to_100KB, Convert.ToInt32(Mail_DeliveredSize_10KB_to_100KB)) _
                                                                                 .Set(Function(x) x.MailDeliveredSize_Under_1KB, Convert.ToInt32(Mail_DeliveredSize_Under_1KB)) _
                                                                                 .Set(Function(x) x.MailDeliveredSize_1KB_to_10KB, Convert.ToInt32(Mail_DeliveredSize_1KB_to_10KB)) _
                                                                                 .Set(Function(x) x.MailDeliveredSize_1MB_to_10MB, Convert.ToInt32(Mail_DeliveredSize_1MB_to_10MB)) _
                                                                                 .Set(Function(x) x.MailDeliveredSize_10MB_to_100MB, Convert.ToInt32(Mail_DeliveredSize_10MB_to_100MB))

            Try
                repository.Upsert(filterDef, updateDef)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString + " Error updating Mail Health. Error: " & ex.Message)
            End Try


        End With

        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString + " Finished updating mail health ")

    End Sub
#End Region

#Region "Domino Cluster Replicator Health Related"
    ' *******************************  Domino Cluster Health

    'This is not being used
    'Private Sub TrackDominoClusterStatistics(ByRef DominoServer As MonitoredItems.DominoServer)

    '    'Sucessful Failover Open Redirects 
    '    Dim ClusterOpenRedirectsFailoverSuccessful, CurrentStatValueOfClusterOpenRedirectsFailoverSuccessful As Long

    '    Try
    '        If InStr(DominoServer.Statistics_Server, "Server.Cluster.OpenRedirects.Failover.Successful") > 0 Then
    '            CurrentStatValueOfClusterOpenRedirectsFailoverSuccessful = ParseNumericStatValue("Server.Cluster.OpenRedirects.Failover.Successful", DominoServer.Statistics_Server)
    '            '  WriteAuditEntry(Now.ToString & " " & DominoServer.Name & " current value of Mail.Delivered stat is " & DeliveredCurrentStatValue)
    '            ' WriteAuditEntry(Now.ToString & " " & DominoServer.Name & " current value of DeliveredMailPrevious is " & DeliveredMailPrevious)
    '            If DominoServer.ClusterOpenRedirects_Previous_FailoverSuccessful <> -1 Then
    '                ClusterOpenRedirectsFailoverSuccessful = CurrentStatValueOfClusterOpenRedirectsFailoverSuccessful - DominoServer.ClusterOpenRedirects_Previous_FailoverSuccessful
    '                ' WriteAuditEntry(Now.ToString & " " & DominoServer.Name & " current value of calculated delivered ClusterOpenRedirectsFailoverSuccessful is " & ClusterOpenRedirectsFailoverSuccessful)

    '                If ClusterOpenRedirectsFailoverSuccessful < 0 Then
    '                    ClusterOpenRedirectsFailoverSuccessful = 0
    '                End If
    '                DominoServer.ClusterOpenRedirects_Previous_FailoverSuccessful = CurrentStatValueOfClusterOpenRedirectsFailoverSuccessful
    '            Else
    '                DominoServer.ClusterOpenRedirects_Previous_FailoverSuccessful = CurrentStatValueOfClusterOpenRedirectsFailoverSuccessful
    '                ClusterOpenRedirectsFailoverSuccessful = 0
    '            End If

    '        End If
    '        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & ": Incremental ClusterOpenRedirectsFailoverSuccessful value is " & ClusterOpenRedirectsFailoverSuccessful)
    '        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & ": Value of Previous ClusterOpenRedirectsFailoverSuccessful is " & DominoServer.ClusterOpenRedirects_Previous_FailoverSuccessful)

    '        UpdateDominoDailyStatTable(DominoServer, "Server.Cluster.OpenRedirects.Failover.Successful", ClusterOpenRedirectsFailoverSuccessful)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " " & DominoServer.Name & ": Error calculating ClusterOpenRedirectsFailoverSuccessful " & ex.Message)
    '    End Try

    '    'Unsucessful Failover Open Redirects 
    '    Dim ClusterOpenRedirectsFailoverUnSuccessful, CurrentStatValueOfClusterOpenRedirectsFailoverUnSuccessful As Long

    '    Try
    '        If InStr(DominoServer.Statistics_Server, "Server.Cluster.OpenRedirects.Failover.Unsuccessful") > 0 Then
    '            CurrentStatValueOfClusterOpenRedirectsFailoverSuccessful = ParseNumericStatValue("Server.Cluster.OpenRedirects.Failover.Unsuccessful", DominoServer.Statistics_Server)

    '            If DominoServer.ClusterOpenRedirects_Previous_FailoverUnSuccessful <> -1 Then
    '                ClusterOpenRedirectsFailoverUnSuccessful = CurrentStatValueOfClusterOpenRedirectsFailoverUnSuccessful - DominoServer.ClusterOpenRedirects_Previous_FailoverUnSuccessful
    '                '   WriteAuditEntry(Now.ToString & " " & DominoServer.Name & " current value of calculated delivered ClusterOpenRedirectsFailoverSuccessful is " & ClusterOpenRedirectsFailoverSuccessful)

    '                If ClusterOpenRedirectsFailoverUnSuccessful < 0 Then
    '                    ClusterOpenRedirectsFailoverUnSuccessful = 0
    '                End If
    '                DominoServer.ClusterOpenRedirects_Previous_FailoverUnSuccessful = CurrentStatValueOfClusterOpenRedirectsFailoverUnSuccessful
    '            Else
    '                DominoServer.ClusterOpenRedirects_Previous_FailoverUnSuccessful = CurrentStatValueOfClusterOpenRedirectsFailoverUnSuccessful
    '                ClusterOpenRedirectsFailoverUnSuccessful = 0
    '            End If

    '        End If
    '        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & ": Incremental ClusterOpenRedirectsFailoverUnSuccessful value is " & ClusterOpenRedirectsFailoverUnSuccessful)
    '        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & ": Value of Previous ClusterOpenRedirectsFailoverUnSuccessful is " & DominoServer.ClusterOpenRedirects_Previous_FailoverUnSuccessful)

    '        UpdateDominoDailyStatTable(DominoServer, "Server.Cluster.OpenRedirects.Failover.Unsuccessful", ClusterOpenRedirectsFailoverUnSuccessful)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " " & DominoServer.Name & ": Error calculating ClusterOpenRedirectsFailoverUnSuccessful " & ex.Message)
    '    End Try

    '    'Unsucessful Load Balance Open Redirects 
    '    Dim ClusterOpenRedirectsLoadBalanceUnSuccessful, CurrentStatValueOfClusterOpenRedirectsLoadBalanceUnSuccessful As Long

    '    Try
    '        If InStr(DominoServer.Statistics_Server, "Server.Cluster.OpenRedirects.LoadBalance.Unsuccessful") > 0 Then
    '            CurrentStatValueOfClusterOpenRedirectsLoadBalanceUnSuccessful = ParseNumericStatValue("Server.Cluster.OpenRedirects.LoadBalance.Unsuccessful", DominoServer.Statistics_Server)

    '            If DominoServer.ClusterOpenRedirects_Previous_LoadBalanceUnSuccessful <> -1 Then
    '                ClusterOpenRedirectsLoadBalanceUnSuccessful = CurrentStatValueOfClusterOpenRedirectsLoadBalanceUnSuccessful - DominoServer.ClusterOpenRedirects_Previous_LoadBalanceUnSuccessful
    '                '   WriteAuditEntry(Now.ToString & " " & DominoServer.Name & " current value of calculated delivered ClusterOpenRedirectsLoadBalanceUnSuccessful is " & ClusterOpenRedirectsLoadBalanceUnSuccessful)

    '                If ClusterOpenRedirectsLoadBalanceUnSuccessful < 0 Then
    '                    ClusterOpenRedirectsLoadBalanceUnSuccessful = 0
    '                End If
    '                DominoServer.ClusterOpenRedirects_Previous_LoadBalanceUnSuccessful = CurrentStatValueOfClusterOpenRedirectsLoadBalanceUnSuccessful
    '            Else
    '                DominoServer.ClusterOpenRedirects_Previous_LoadBalanceUnSuccessful = CurrentStatValueOfClusterOpenRedirectsLoadBalanceUnSuccessful
    '                ClusterOpenRedirectsLoadBalanceUnSuccessful = 0
    '            End If

    '        End If
    '        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & ": Incremental ClusterOpenRedirectsFailoverUnSuccessful value is " & ClusterOpenRedirectsFailoverUnSuccessful)
    '        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & ": Value of Previous ClusterOpenRedirectsFailoverUnSuccessful is " & DominoServer.ClusterOpenRedirects_Previous_FailoverUnSuccessful)

    '        UpdateDominoDailyStatTable(DominoServer, "Server.Cluster.OpenRedirects.LoadBalance.Unsuccessful", ClusterOpenRedirectsLoadBalanceUnSuccessful)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " " & DominoServer.Name & ": Error calculating ClusterOpenRedirectsLoadBalanceUnSuccessful " & ex.Message)
    '    End Try


    '    'Sucessful Load Balance Open Redirects 
    '    Dim ClusterOpenRedirectsLoadBalanceSuccessful, CurrentStatValueOfClusterOpenRedirectsLoadBalanceSuccessful As Long

    '    Try
    '        If InStr(DominoServer.Statistics_Server, "Server.Cluster.OpenRedirects.LoadBalance.Successful") > 0 Then
    '            CurrentStatValueOfClusterOpenRedirectsLoadBalanceSuccessful = ParseNumericStatValue("Server.Cluster.OpenRedirects.LoadBalance.Successful", DominoServer.Statistics_Server)

    '            If DominoServer.ClusterOpenRedirects_Previous_LoadBalanceSuccessful <> -1 Then
    '                ClusterOpenRedirectsLoadBalanceSuccessful = CurrentStatValueOfClusterOpenRedirectsLoadBalanceSuccessful - DominoServer.ClusterOpenRedirects_Previous_LoadBalanceSuccessful
    '                '     WriteAuditEntry(Now.ToString & " " & DominoServer.Name & " current value of calculated delivered ClusterOpenRedirectsLoadBalanceSuccessful is " & ClusterOpenRedirectsLoadBalanceSuccessful)

    '                If ClusterOpenRedirectsLoadBalanceSuccessful < 0 Then
    '                    ClusterOpenRedirectsLoadBalanceSuccessful = 0
    '                End If
    '                DominoServer.ClusterOpenRedirects_Previous_LoadBalanceSuccessful = CurrentStatValueOfClusterOpenRedirectsLoadBalanceSuccessful
    '            Else
    '                DominoServer.ClusterOpenRedirects_Previous_LoadBalanceSuccessful = CurrentStatValueOfClusterOpenRedirectsLoadBalanceSuccessful
    '                ClusterOpenRedirectsLoadBalanceSuccessful = 0
    '            End If

    '        End If
    '        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & ": Incremental ClusterOpenRedirectsFailoverUnSuccessful value is " & ClusterOpenRedirectsFailoverUnSuccessful)
    '        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & ": Value of Previous ClusterOpenRedirectsFailoverUnSuccessful is " & DominoServer.ClusterOpenRedirects_Previous_FailoverUnSuccessful)

    '        UpdateDominoDailyStatTable(DominoServer, "Server.Cluster.OpenRedirects.LoadBalance.Successful", ClusterOpenRedirectsLoadBalanceSuccessful)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " " & DominoServer.Name & ": Error calculating ClusterOpenRedirectsLoadBalanceSuccessful " & ex.Message)
    '    End Try

    'End Sub

    Private Sub CheckClusterMemberAvailability(ByRef DominoServer As MonitoredItems.DominoServer)
        Try
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Checking Cluster Health and Availability for " & DominoServer.Name)
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & " Availability Threshold is " & DominoServer.AvailabilityThreshold)
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & " Availability Index is " & DominoServer.AvailabilityIndex)
        Catch ex As Exception

        End Try

        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.WorkQueueDepth") > 0 Then
                Dim myDepth As Integer = ParseNumericStatValue("Replica.Cluster.WorkQueueDepth", DominoServer.Statistics_Replica)
                DominoServer.ReplicaClusterWorkQueueDepth = myDepth
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Replica.Cluster.WorkQueueDepth: " & ex.Message)
        End Try

        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Updating Daily Statistics Table for Replica.Cluster.WorkQueueDepth.Avg.")
        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.WorkQueueDepth.Avg") > 0 Then
                DominoServer.ReplicaClusterWorkQueueDepthAvg = ParseNumericStatValue("Replica.Cluster.WorkQueueDepth.Avg", DominoServer.Statistics_Replica)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Replica.Cluster.WorkQueueDepthAvg: " & ex.Message)
        End Try

        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.WorkQueueDepth.Max") > 0 Then
                Dim myDepth As Integer = ParseNumericStatValue("Replica.Cluster.WorkQueueDepth.Max", DominoServer.Statistics_Replica)
                DominoServer.ReplicaClusterWorkQueueDepthMax = myDepth
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Replica.Cluster.WorkQueueDepth.Max: " & ex.Message)
        End Try
        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Updating Daily Statistics Table for Replica.Cluster.SecondsOnQueue.")

        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.SecondsOnQueue") > 0 Then
                Dim myDepth As Integer = ParseNumericStatValue("Replica.Cluster.SecondsOnQueue", DominoServer.Statistics_Replica)
                DominoServer.ReplicaClusterSecondsOnQueue = myDepth
            End If
        Catch ex As Exception

            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Replica.Cluster.SecondsOnQueue: " & ex.Message)
        End Try

        Try
            'change the server status if the secondsonqueuue is bad
            If DominoServer.ReplicaClusterSecondsOnQueue > DominoServer.ClusterRep_Threshold Then
                DominoServer.Status = "Cluster Replication Delays"
                DominoServer.ResponseDetails = "The cluster replication threshold is " & DominoServer.ClusterRep_Threshold & " and the current value is " & DominoServer.ReplicaClusterSecondsOnQueue
                myAlert.QueueAlert("Domino", DominoServer.Name, "Cluster Replicator Delay", DominoServer.ResponseDetails, DominoServer.Location)
            Else
                myAlert.ResetAlert("Domino", DominoServer.Name, "Cluster Replicator Delay", DominoServer.Location)
            End If
        Catch ex As Exception

        End Try

        Try
            If DominoServer.ReplicaClusterSecondsOnQueue > DominoServer.ClusterRep_Threshold And DominoServer.ClusterRep_Threshold > 10 Then
                Try
                    If DominoServer.ReplicaClusterSecondsOnQueue > DominoServer.Load_ClusterRep_Threshold Then
                        '  DominoServer.Status = "Load Cluster Replication Delays"
                        'DominoServer.ResponseDetails = "The cluster replicator was falling behind so VitalSigns loaded another instance by issuing a remote console command."
                        SendDominoConsoleCommands(DominoServer.Name, "load clrepl", "Sending a console command since the Cluster Replicator Delay is higher than the load cluster replication threshold")
                        'Increase the threshold for this instance so you don't keep adding instances
                        DominoServer.Load_ClusterRep_Threshold += 10
                    End If
                Catch ex As Exception

                End Try

            End If
        Catch ex As Exception

        End Try
        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.SecondsOnQueue.Avg") > 0 Then
                Dim myDepth As Integer = ParseNumericStatValue("Replica.Cluster.SecondsOnQueue.Avg", DominoServer.Statistics_Replica)
                DominoServer.ReplicaClusterSecondsOnQueueAvg = myDepth
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing Replica.Cluster.SecondsOnQueue.Avg: " & ex.Message)
        End Try

        Try
            If InStr(DominoServer.Statistics_Replica, "Replica.Cluster.SecondsOnQueue.Max") > 0 Then
                Dim myDepth As Integer = ParseNumericStatValue("Replica.Cluster.SecondsOnQueue.Max", DominoServer.Statistics_Replica)
                DominoServer.ReplicaClusterSecondsOnQueueMax = myDepth
            End If
        Catch ex As Exception

        End Try


        Try
            If DominoServer.AvailabilityIndex = Nothing Then
                WriteAuditEntry(Now.ToString & " Error checking Cluster Availability for " & DominoServer.Name & ". The Availability Index is not available.  Ironic, isn't it?", LogLevel.Verbose)
                Exit Sub
            End If

        Catch ex As Exception

        End Try

        Try
            If DominoServer.AvailabilityThreshold = Nothing And DominoServer.AvailabilityThreshold <> 0 Then
                WriteAuditEntry(Now.ToString & " Error checking Cluster Availability for " & DominoServer.Name & ". The Availability Threshold is not available.  Ironic, isn't it?", LogLevel.Verbose)
                Exit Sub
            End If
        Catch ex As Exception

        End Try

        Try
            If DominoServer.AvailabilityIndex <= DominoServer.AvailabilityThreshold Then
                'This server is officially "Busy" and will reject database.open requests
                DominoServer.Status = "Load Balancing"
                DominoServer.ResponseDetails = "The server's availability index, " & DominoServer.AvailabilityIndex & ", equals or exceeds the availability threshold of " & DominoServer.AvailabilityThreshold & ".  Database open requests to this server will be redirected to another server for load balancing."
                DominoServer.Description = "The server's availability index, " & DominoServer.AvailabilityIndex & ", equals or exceeds the availability threshold of " & DominoServer.AvailabilityThreshold & ".  Database open requests to this server will be redirected to another server for load balancing."

                UpdateDominoStatusTable(DominoServer)
                myAlert.QueueAlert("Domino", DominoServer.Name, "Failover", DominoServer.ResponseDetails, DominoServer.Location)
            Else
                myAlert.ResetAlert("Domino", DominoServer.Name, "Failover", DominoServer.Location)  'no need to be busy and overloaded
            End If
        Catch ex As Exception

        End Try


        Try
            dtDominoLastUpdate = Now
        Catch ex As Exception

        End Try
    End Sub

    Private Sub UpdateClusterHealth(ByRef DominoServer As MonitoredItems.DominoServer)
        'Write the results to the database
        Try
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Updating Domino Cluster Health ")
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " This cluster's availability index is " & DominoServer.AvailabilityIndex)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error Updating Domino Cluster Health: " & ex.ToString)
        End Try

        Dim Analysis As String = ""
        Try
            If DominoServer.AvailabilityIndex <= DominoServer.AvailabilityThreshold Then
                Analysis = "Redirecting Users"
            End If

            If (DominoServer.AvailabilityIndex * 0.9) <= DominoServer.AvailabilityThreshold Then
                Analysis = "Approaching Threshold"
            End If

            If DominoServer.AvailabilityIndex >= DominoServer.AvailabilityThreshold And DominoServer.AvailabilityThreshold <> 0 Then
                Analysis = "OK"
            End If

            If DominoServer.AvailabilityThreshold = 0 Then
                Analysis = "Not Configured for Load Balancing"
            End If

            If DominoServer.AvailabilityThreshold = 100 Then
                Analysis = "Set as BUSY, will only be used if all other servers are down"
            End If

        Catch ex As Exception
            Analysis = ""
        End Try

        Dim myClusterName As String = DominoServer.ClusterMember
        Try
            If InStr(myClusterName, "NSPingServer") > 0 Or InStr(myClusterName, "ERROR") > 0 Then
                myClusterName = ""
            End If
        Catch ex As Exception
            myClusterName = ""
        End Try

        If myClusterName = "" Then Exit Sub

        Dim strSQL As String = ""
        Try
            With DominoServer
                Dim DominoServer2 As MonitoredItems.DominoServer = DominoServer
                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.TypeAndName.Equals(DominoServer2.Name & "-Domino"))
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                     .Set(Function(x) x.ClusterName, myClusterName) _
                                                                                     .Set(Function(x) x.ClusterSecondsOnQueue, .ReplicaClusterSecondsOnQueue) _
                                                                                     .Set(Function(x) x.ClusterSecondsOnQueueMax, .ReplicaClusterSecondsOnQueueMax) _
                                                                                     .Set(Function(x) x.ClusterSecondsOnQueueAverage, .ReplicaClusterSecondsOnQueueAvg) _
                                                                                     .Set(Function(x) x.ClusterWorkQueueDepth, .ReplicaClusterWorkQueueDepth) _
                                                                                     .Set(Function(x) x.ClusterWorkQueueDepthMax, .ReplicaClusterWorkQueueDepthMax) _
                                                                                     .Set(Function(x) x.ClusterWorkQueueDepthAverage, .ReplicaClusterWorkQueueDepthAvg) _
                                                                                     .Set(Function(x) x.ClusterAvailability, .AvailabilityIndex) _
                                                                                     .Set(Function(x) x.ClusterAvailabilityThreshold, .AvailabilityThreshold) _
                                                                                     .Set(Function(x) x.ClusterAnalysis, Analysis)

                Try
                    repository.Upsert(filterDef, updateDef)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error updating the DominoClusterHealth stats:  " & ex.ToString)
                End Try

            End With

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error in Domino Cluster Health creating SQL statement for status table: " & ex.Message & vbCrLf & strSQL)
        End Try

        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Finished Updating Domino Cluster Health ")
    End Sub

#End Region

#Region "Domino Console Commands"


    Private Sub SendDominoConsoleCommand(ByVal ServerName As String, ByVal Command As String)
        'This function returns a string for any Domino statistic that returns text, such as Disk.C.Type returns NTFS

        Try
            WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " sending " & Command & " to " & ServerName)
            NotesSession.SendConsoleCommand(ServerName, Command)

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Error sending " & Command & " to " & ServerName & ": " & ex.ToString)
        End Try


    End Sub

    Private Sub SendDominoConsoleCommands(ByVal ServerName As String, ByVal Command As String, ByVal Comments As String)
        'This function submits a record in the DominoConsoleCommands table to be processed by the Console Service
        Dim StrSQL As String
        Dim vsAdapter As New VSFramework.VSAdaptor
        Try
            'WriteDeviceHistoryEntry("Domino", ServerName, "Step3 - region DCC")
            WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " sending " & Command & " to " & ServerName)
            'NotesSession.SendConsoleCommand(ServerName, Command)

            StrSQL = "DECLARE @currDate DATETIME; " & vbCrLf
            StrSQL += "SET @currDate = GETDATE(); " & vbCrLf
            StrSQL += "Insert INTO DominoConsoleCommands (ServerName, Command, Submitter, DateTimeSubmitted, DateTimeProcessed, Result, Comments) Values ('" + ServerName + "', '" + Command + "', 'VitalSigns Domino Service', @currDate, NULL, 'Command Request sent', '" + Comments + " '); "
            vsAdapter.ExecuteNonQueryAny("VitalSigns", "DominoConsoleCommands", StrSQL)
            'WriteDeviceHistoryEntry("Domino", ServerName, StrSQL)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Error sending " & Command & " to " & ServerName & ": " & ex.ToString)
        End Try

    End Sub

#End Region

#Region "Domino Statistic Handling"


    Sub GetAllDominoStatistics(ByRef DominoServerName As MonitoredItems.DominoServer)
        'This function queries the Domino server for all statistics and saves it to the in-memory Server instance
        Dim strResult As String
        Dim n As Integer

        Dim oWatch As New System.Diagnostics.Stopwatch

        Try
            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Sending statistics request.")


            Try
                oWatch.Start()
                DominoServerName.Statistics_Mail = ""
                WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Attempting to get mail stats...")
                ' If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
                strResult = GetStats(DominoServerName.Name, "MAIL", "")
                ' 'If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()

                If InStr(strResult, "Mail.Dead") > 0 Then
                    DominoServerName.Statistics_Mail = strResult
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Successfully retrieved  MAIL stats... first attempt")
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_Mail, LogLevel.Verbose)
                    ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got MAIL stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                    'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", Memory, Success, " & oWatch.ElapsedMilliseconds & ", 1, " & Now.ToString, LogLevel.Verbose)
                End If

            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Error getting MAIL stats..." & ex.ToString)
                DominoServerName.Statistics_Mail = ""
            Finally

            End Try


            Try
                If DominoServerName.Statistics_Mail <> "" And InStr(DominoServerName.Statistics_Mail, "Mail.Domain") Then
                    DominoServerName.RemoteConsoleAccess = True
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " " & DominoServerName.Name & " has mail statistics.")
                    '   WriteAuditEntry(vbCrLf & DominoServerName.Name & " Statistics" & vbCrLf & strResult & vbCrLf)
                Else
                    '    DominoServerName.RemoteConsoleAccess = False
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " " & DominoServerName.Name & " Does NOT have any mail statistics.")
                End If
            Catch ex As Exception

            End Try


            oWatch.Restart()
            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Attempting to get SERVER stats... ")
            strResult = ""
            n = 1

            Try
                ' DominoServerName.Statistics_Server = ""
                '  If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
                strResult = GetStats(DominoServerName.Name, "Server", "")
                If InStr(strResult, "Server.Users") > 0 Then
                    DominoServerName.Statistics_Server = strResult
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Successfully retrieved  SERVER stats... first attempt")
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_Server, LogLevel.Verbose)
                    ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got Server stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                    'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", Server, Success, " & oWatch.ElapsedMilliseconds & ", 1, " & Now.ToString, LogLevel.Verbose)
                End If

                If Not (InStr(strResult, "Server.Users") > 0) Then

                    Do While Not (InStr(strResult, "Server.Users") > 0)
                        strResult = GetStats(DominoServerName.Name, "Server", "")
                        If InStr(strResult, "Server.Users") Then
                            DominoServerName.Statistics_Server = strResult
                            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Successfully retrieved  SERVER stats...subsequent attempt ")
                            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_Server, LogLevel.Verbose)
                            ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got Server stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                            'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", Server, Success, " & oWatch.ElapsedMilliseconds & ", " & n & ", " & Now.ToString, LogLevel.Verbose)
                            Exit Do
                        End If

                        n += 1
                        If n > 3 Then
                            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Unable to retrieve SERVER stats despite multiple attempts. ")
                            ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Failed to get SERVER stats after " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                            'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", Server, Failure, " & oWatch.ElapsedMilliseconds, LogLevel.Verbose)
                            Exit Do
                        End If
                    Loop
                End If

            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Error getting SERVER stats..." & ex.ToString)
                DominoServerName.Statistics_Server = ""
            Finally
                'If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
            End Try

            oWatch.Restart()
            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Attempting to get MEM stats...")
            strResult = ""
            Try
                '  If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
                ' DominoServerName.Statistics_Memory = ""
                strResult = GetStats(DominoServerName.Name, "Mem", "")
                If InStr(strResult, "Mem.Availability") > 0 Then
                    DominoServerName.Statistics_Memory = strResult
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Successfully retrieved  MEMORY stats... first attempt")
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_Memory, LogLevel.Verbose)
                    ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got Memory stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                    'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", Memory, Success, " & oWatch.ElapsedMilliseconds & ", 1" & n, LogLevel.Verbose)
                End If

                n = 1

                If Not (InStr(strResult, "Mem.Free") > 0) Then
                    '  If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
                    Do While Not (InStr(strResult, "Mem.Free") > 0)
                        strResult = GetStats(DominoServerName.Name, "Mem", "")
                        If InStr(strResult, "Mem.Free") Then
                            DominoServerName.Statistics_Memory = strResult
                            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Successfully retrieved  MEMORY stats... subsequent attempt ")
                            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_Memory, LogLevel.Verbose)
                            ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got Memory stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                            'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", Memory, Success, " & oWatch.ElapsedMilliseconds & ", " & n & ", " & Now.ToString, LogLevel.Verbose)
                            Exit Do
                        End If

                        n += 1
                        If n > 3 Then
                            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Unable to retrieve MEM stats despite multiple attempts. ")
                            ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Failed to get MEM stats after " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                            'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", Memory, Failure, " & oWatch.ElapsedMilliseconds, LogLevel.Verbose)
                            Exit Do
                        End If
                    Loop
                End If

            Catch ex As Exception
                DominoServerName.Statistics_Memory = ""
            Finally
                'If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
            End Try

            '1/6/2015 NS modified for VSPLUS-1302
            'If InStr(DominoServerName.ShowTasks, "DOMINO") Then
            oWatch.Restart()
            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Attempting to get DOMINO stats...")
            strResult = ""
            Try
                '  If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
                strResult = GetStats(DominoServerName.Name, "Domino", "")
                If InStr(strResult, "Domino.Threads") > 0 Then
                    DominoServerName.Statistics_Domino = strResult
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Successfully retrieved  DOMINO stats... first attempt")
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_Domino, LogLevel.Verbose)
                    ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got DOMINO stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                    'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", Domino, Success, " & oWatch.ElapsedMilliseconds & ", 1, " & Now.ToString, LogLevel.Verbose)
                End If
            Catch ex As Exception
                DominoServerName.Statistics_Domino = ""
            Finally
                'If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
            End Try

            n = 1
            If Not (InStr(strResult, "Domino.Threads") > 0) Then
                Try
                    '  If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
                    Do While Not (InStr(strResult, "Domino.Threads") > 0)
                        strResult = GetStats(DominoServerName.Name, "Domino", "")
                        If InStr(strResult, "Domino.Threads") Then
                            DominoServerName.Statistics_Domino = strResult
                            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Successfully retrieved  DOMINO stats... subsequent attempt ")
                            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_Domino, LogLevel.Verbose)
                            ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got DOMINO stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                            'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", Domino, Success, " & oWatch.ElapsedMilliseconds & ", " & n & ", " & Now.ToString, LogLevel.Verbose)
                            Exit Do
                        End If

                        n += 1
                        If n > 3 Then
                            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Unable to retrieve DOMINO  stats despite multiple attempts. ")
                            ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Failed to get DOMINO stats after " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                            'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", DOMINO, Failure, " & oWatch.ElapsedMilliseconds, LogLevel.Verbose)
                            Exit Do
                        End If
                    Loop
                Catch ex As Exception
                Finally
                    'If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
                End Try

            End If

            oWatch.Restart()
            '1/7/2015 NS modified for VSPLUS-1302
            'Else
            ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Not requesting DOMINO stats because HTTP is not running.", LogLevel.Verbose)
            'End If



            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Attempting to get DISK stats...")
            strResult = ""
            Try
                '  If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
                ' DominoServerName.Statistics_Disk = ""
                strResult = GetStats(DominoServerName.Name, "Disk", "")
                If InStr(strResult, "Disk.Fixed") > 0 Then
                    DominoServerName.Statistics_Disk = strResult
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Successfully retrieved  DISK stats... first attempt")
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_Disk, LogLevel.Verbose)
                    ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got DISK stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                    'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", DISK, Success, " & oWatch.ElapsedMilliseconds & ", 1, " & Now.ToString, LogLevel.Verbose)
                End If

                n = 1
                Do While Not (InStr(strResult, "Disk.Fixed") > 0)
                    strResult = GetStats(DominoServerName.Name, "Disk", "")
                    If InStr(strResult, "Disk.Fixed") Then
                        DominoServerName.Statistics_Disk = strResult
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Successfully retrieved  DISK stats... subsequent attempt")
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_Disk, LogLevel.Verbose)
                        ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got DISK stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                        'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", DISK, Success, " & oWatch.ElapsedMilliseconds & ", " & n & ", " & Now.ToString, LogLevel.Verbose)
                        Exit Do
                    End If

                    n += 1
                    If n > 3 Then
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Unable to retrieve DISK stats despite multiple attempts. ")
                        ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Failed to get DISK stats after " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                        'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", DISK, Failure, " & oWatch.ElapsedMilliseconds, LogLevel.Verbose)
                        Exit Do
                    End If
                Loop
            Catch ex As Exception
                DominoServerName.Statistics_Disk = ""
            Finally
                'If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
            End Try


            '7/21/2014 I commented out this entire section because Quickr is dead

            'WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Attempting to get Quickr stats...")
            'strResult = ""
            'Try
            '    '  If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
            '    strResult = GetStats(DominoServerName.Name, "QuickPlace", "")
            '    ' WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & strResult)
            '    If InStr(strResult, "QuickPlace") > 0 Then
            '        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " This IS a Quickr server.")
            '        DominoServerName.QuickrServer = True


            '        If DominoServerName.QuickrServer = True And (DominoServerName.SecondaryRole.Contains("Quickr") = False) Then
            '            If DominoServerName.SecondaryRole = "" Then
            '                DominoServerName.SecondaryRole = "Quickr"
            '            Else
            '                DominoServerName.SecondaryRole += "; Quickr"
            '            End If
            '        End If

            '    Else
            '        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " This is NOT a Quickr server.")
            '    End If
            'Catch ex As Exception
            '    DominoServerName.QuickrServer = False
            'Finally
            '    'If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
            'End Try

            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Attempting to get PLATFORM stats...")
            strResult = ""
            n = 1

            Try
                '  If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
                DominoServerName.Statistics_Platform = ""
                strResult = GetStats(DominoServerName.Name, "PLATFORM", "")
                If InStr(strResult, "Platform.") > 0 Then
                    DominoServerName.Statistics_Platform = strResult
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Successfully retrieved  PLATFORM stats... first attempt")
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_Platform, LogLevel.Verbose)
                    ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got Platform stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                    'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", Platform, Success, " & oWatch.ElapsedMilliseconds & ", " & n & ", " & Now.ToString, LogLevel.Verbose)
                End If


                Do While Not (InStr(strResult, "Platform.") > 0)
                    strResult = GetStats(DominoServerName.Name, "Platform.", "")
                    If InStr(strResult, "Platform.") Then
                        DominoServerName.Statistics_Disk = strResult
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Successfully retrieved  PLATFORM stats... subsequent attempt")
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_Platform, LogLevel.Verbose)
                        ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got Platform stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                        'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", Platform, Success, " & oWatch.ElapsedMilliseconds & ", " & n & ", " & Now.ToString, LogLevel.Verbose)
                        Exit Do
                    End If

                    n += 1
                    If n > 2 Then
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Unable to retrieve Platform stats despite multiple attempts. ")
                        ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Failed to get Platform stats after " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                        'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", Platform, Failure, " & oWatch.ElapsedMilliseconds, LogLevel.Verbose)
                        Exit Do
                    End If
                Loop

            Catch ex As Exception
                DominoServerName.Statistics_Platform = "Error getting Platform statistics.  Please confirm that platform statistics are enabled for this server."
            Finally
                'If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
            End Try

            '1/6/2015 NS modified for VSPLUS-1302
            'If InStr(DominoServerName.ShowTasks, "HTTP") Then
            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Attempting to get HTTP stats...")
            strResult = ""
            Try
                '  If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
                DominoServerName.Statistics_HTTP = ""
                strResult = GetStats(DominoServerName.Name, "HTTP", "")
                n = 1

                If InStr(strResult.ToUpper, "HTTP.") > 0 Then
                    DominoServerName.Statistics_HTTP = strResult
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Successfully retrieved HTTP stats... first attempt")
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_HTTP, LogLevel.Verbose)
                    ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got HTTP stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                    'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", HTTP, Success, " & oWatch.ElapsedMilliseconds & ", " & n & ", " & Now.ToString, LogLevel.Verbose)
                End If


                Do While Not (InStr(strResult.ToUpper, "HTTP.") > 0)
                    strResult = GetStats(DominoServerName.Name, "HTTP", "")
                    If InStr(strResult, "http") Then
                        DominoServerName.Statistics_HTTP = strResult
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Successfully retrieved HTTP stats... subsequent attempt")
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_HTTP, LogLevel.Verbose)
                        ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got HTTP stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                        'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", HTTP, Success, " & oWatch.ElapsedMilliseconds & ", " & n & ", " & Now.ToString, LogLevel.Verbose)
                        Exit Do
                    End If

                    If InStr(strResult, "not found in server") > 0 Then
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Server says HTTP not found in server statistics table... it would seem that this server does not run HTTP.")
                    End If

                    n += 1
                    If n > 3 Then
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Unable to retrieve HTTP stats despite multiple attempts. ")
                        ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Failed to get HTTP stats after " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                        'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", HTTP, Failure, " & oWatch.ElapsedMilliseconds, LogLevel.Verbose)
                        Exit Do
                    End If
                Loop

                If InStr(strResult, "Http.Workers") > 0 Then
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " This server runs HTTP.")
                Else
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " This does not run HTTP, or HTTP statistics were not available.")
                End If
                '  WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & strResult)

            Catch ex As Exception
                DominoServerName.Statistics_HTTP = ""

            End Try
            '1/7/2015 NS modified for VSPLUS-1302
            'Else
            'WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Skipping HTTP stats because the server does not seem to be running HTTP...", LogLevel.Verbose)
            'End If


            If DominoServerName.Traveler_Server <> False Then
                WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Attempting to get TRAVELER stats...")
                strResult = ""

                Try
                    '  If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
                    DominoServerName.Statistics_Traveler = ""
                    strResult = GetStats(DominoServerName.Name, "Traveler", "")
                    n = 1

                    If InStr(strResult, "Traveler.Version") > 0 Then
                        DominoServerName.Traveler_Server = True
                        DominoServerName.Statistics_Traveler = strResult
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Got TRAVELER statistics... Obviously, this IS a Traveler server.")
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_Traveler, LogLevel.Verbose)
                        ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got TRAVELER stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                        'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", TRAVELER, Success, " & oWatch.ElapsedMilliseconds & ", " & n & ", " & Now.ToString, LogLevel.Verbose)
                    End If


                    If (DominoServerName.Traveler_Server <> False And Not (InStr(strResult, "Traveler.Version") > 0)) Then

                        Do
                            ' WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Attempt #" & n.ToString & " to get Traveler statistics...")
                            strResult = GetStats(DominoServerName.Name, "Traveler", "")
                            If InStr(strResult, "Traveler.Version") Then
                                DominoServerName.Statistics_Traveler = strResult
                                ' WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Retrieved the following Traveler stats: " & vbCrLf & strResult)
                                WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_Traveler, LogLevel.Verbose)
                                ' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got TRAVELER stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                                'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", TRAVELER, Success, " & oWatch.ElapsedMilliseconds & ", " & n & ", " & Now.ToString, LogLevel.Verbose)
                                Exit Do
                            End If

                            If InStr(strResult, "not found in server") > 0 Then
                                DominoServerName.Traveler_Server = False
                                WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Server says TRAVELER not found in server statistics table... it would seem that this is NOT a Traveler server.")
                                Exit Do
                            End If

                            n += 1
                            If n > 3 Then Exit Do
                        Loop While Not (InStr(DominoServerName.Statistics_Traveler, "Traveler.Version"))

                    End If

                    If InStr(DominoServerName.Statistics_Traveler, "Traveler.CPU") Then
                        DominoServerName.Traveler_Server = True
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " This IS a Traveler server.")
                    Else
                        'WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " This IS NOT a Traveler server.")
                    End If

                Catch ex As Exception
                    DominoServerName.Statistics_Traveler = ""
                    ' DominoServerName.Traveler_Server = False
                Finally
                    'If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
                End Try
            End If

            If DominoServerName.Traveler_Server = True And (DominoServerName.SecondaryRole.Contains("Traveler") = False) Then
                If DominoServerName.SecondaryRole = "" Then
                    DominoServerName.SecondaryRole = "Traveler"
                Else
                    DominoServerName.SecondaryRole += "; Traveler"
                End If
            End If

            If InStr(DominoServerName.ShowTasks, "Replicator") Then
                Try
                    WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Attempting to get REPLICA stats...")
                    strResult = ""
                    '  If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
                    strResult = GetStats(DominoServerName.Name, "replica", "")
                    n = 1

                    If Not InStr(strResult, "Replica.") > 0 Then
                        Do While Not (InStr(strResult.ToUpper, "Replica.") > 0)
                            strResult = GetStats(DominoServerName.Name, "replica", "")
                            If InStr(strResult, "Replica.") Then
                                DominoServerName.Statistics_Replica = strResult
                                WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Successfully retrieved REPLICA stats... subsequent attempt")
                                '' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got REPLICA stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                                'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", REPLICA, Success, " & oWatch.ElapsedMilliseconds & ", " & n & ", " & Now.ToString, LogLevel.Verbose)
                                Exit Do
                            End If
                            n += 1
                            If n > 3 Then
                                WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Unable to retrieve REPLICA stats despite multiple attempts. ")
                                '' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Failed to get REPLICA stats after " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                                'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", REPLICA, Failure, " & oWatch.ElapsedMilliseconds, LogLevel.Verbose)
                                Exit Do
                            End If
                        Loop
                    Else
                        DominoServerName.Statistics_Replica = ""
                        DominoServerName.Statistics_Replica = strResult
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Retrieved REPLICA stats using API function.")
                        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, DominoServerName.Statistics_Replica, LogLevel.Verbose)
                        '' WriteDeviceHistoryEntry("Notes API", DominoServerName.Name, Now.ToString & " Got REPLICA stats in " & oWatch.ElapsedMilliseconds & " ms", LogLevel.Verbose)
                        ' 'WriteDeviceHistoryEntry("All", "Notes API", DominoServerName.Name & ", REPLICA, Success, " & oWatch.ElapsedMilliseconds & ", " & n & ", " & Now.ToString, LogLevel.Verbose)
                    End If
                Catch ex As Exception
                    DominoServerName.Statistics_Replica = ""
                Finally
                    'If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
                End Try
            Else
                WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Skipping REPLICA stats as the server is not running the replicator")
            End If




        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " An Error was detected in the GetAllDominoStatistics module when querying stats for " & DominoServerName.Name & ": " & ex.Message)
            DominoServerName.ResponseDetails = ex.Message
            DominoServerName.Statistics_Mail = ""

            If InStr(ex.Message, "Network operation") > 0 Then
                DominoServerName.RemoteConsoleAccess = True
                WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Network Error while gathering statistics: " & ex.Message)
            ElseIf InStr(ex.Message, "not authorized") > 0 Then
                DominoServerName.RemoteConsoleAccess = False
                WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Not Authorized Error gathering statistics: " & ex.Message)
            Else
                WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Exception Error when gathering statistics: " & ex.Message)
            End If

        End Try

        Try
            DominoServerName.Statistics_All = DominoServerName.Statistics_Mail & vbCrLf & DominoServerName.Statistics_Database & vbCrLf & DominoServerName.Statistics_Disk & vbCrLf & DominoServerName.Statistics_Memory & vbCrLf & DominoServerName.Statistics_Platform & vbCrLf & DominoServerName.Statistics_Replica & vbCrLf & DominoServerName.Statistics_Server
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Error creating all stats: " & ex.ToString)

        End Try

        WriteDeviceHistoryEntry("Domino", DominoServerName.Name, Now.ToString & " Completed getting all the stats.")


    End Sub



    Sub CheckDominoCustomStatistics(ByRef DominoServer As MonitoredItems.DominoServer)
        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Checking custom statistics for " & DominoServer.Name & ".")
        If DominoServer.CustomStatisticsSettings.Count = 0 Then
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & " has no custom statistics defined.")
            Exit Sub
        End If
        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Server has " & DominoServer.CustomStatisticsSettings.Count & " custom statistics.")

        For Each Stat As MonitoredItems.DominoCustomStatistic In DominoServer.CustomStatisticsSettings
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Searching statistics collection for the value of " & Stat.Statistic & ". ")
            Try
                Try
                    Stat.Value = ParseNumericStatValue(Stat.Statistic, DominoServer.Statistics_All)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " #1 Error checking Domino Custom Statistics for " & DominoServer.Name & ":  " & ex.Message)
                    Stat.Value = -999
                End Try

                Try
                    If Stat.Value <> -999 Then
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Found " & Stat.Statistic & " with value of  " & Stat.Value & ". ")
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " #2 Error checking Domino Custom Statistics for " & DominoServer.Name & ":  " & ex.Message)
                    Stat.Value = -999
                End Try

                Try

                    If Stat.Value = -999 Then
                        '    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & Stat.Statistic.ToUpper & " not found in collection, which is  " & vbCrLf & DominoServer.Statistics_All.ToUpper & ". ")
                        If InStr(Stat.Statistic.ToUpper, "Mail.".ToUpper) Then
                            Stat.Value = ParseNumericStatValue(Stat.Statistic, DominoServer.Statistics_Mail)
                        End If
                        If InStr(Stat.Statistic.ToUpper, "Server.".ToUpper) Then
                            Stat.Value = ParseNumericStatValue(Stat.Statistic, DominoServer.Statistics_Server)
                        End If
                        If InStr(Stat.Statistic.ToUpper, "Disk.".ToUpper) Then
                            Stat.Value = ParseNumericStatValue(Stat.Statistic, DominoServer.Statistics_Disk)
                        End If
                        If InStr(Stat.Statistic.ToUpper, "Mem.".ToUpper) Then
                            Stat.Value = ParseNumericStatValue(Stat.Statistic, DominoServer.Statistics_Memory)
                        End If
                        If InStr(Stat.Statistic.ToUpper, "Replica.".ToUpper) Then
                            Stat.Value = ParseNumericStatValue(Stat.Statistic, DominoServer.Statistics_Replica)
                        End If
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " #3 Error checking Domino Custom Statistics for " & DominoServer.Name & ":  " & ex.Message)
                    Stat.Value = -999
                End Try

                Try
                    If Stat.Value = -999 Then
                        Dim Facility As String
                        Dim StatName As String

                        Dim intStartLocation As Integer  'location of start of target stat
                        intStartLocation = InStr(Stat.Statistic, ".")

                        Facility = Mid(Stat.Statistic, 1, intStartLocation - 1)
                        StatName = Mid(Stat.Statistic, intStartLocation + 1)
                        ' WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Parsed out Facility as " & Facility & " and Stat as " & StatName)

                        Stat.Value = GetDominoNumericStatistic(DominoServer.Name, Facility, StatName)
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " #4 Error checking Domino Custom Statistics for " & DominoServer.Name & ":  " & ex.Message)
                    Stat.Value = -999
                End Try

                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & " " & Stat.Statistic & " " & Stat.ComparisonOperator & " " & Stat.ThresholdValue & ".  Current Value is " & Stat.Value & ".")

                If Not Stat.Value = -999 Then
                    If Stat.AlertCondition = True Then
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & " " & Stat.Statistic & " is in an Alert condition.")
                        DominoServer.AlertCondition = True
                        DominoServer.Status = Stat.Statistic & " outside target range"
                        DominoServer.ResponseDetails += vbCrLf & Stat.Statistic & " " & Stat.ComparisonOperator & " " & Stat.ThresholdValue & ".  Current Value is " & Stat.Value & "." & vbCrLf
                        DominoServer.Description = Stat.Statistic & " " & Stat.ComparisonOperator & " " & Stat.ThresholdValue & ".  Current Value is " & Stat.Value & "."
                        If Stat.ConsoleCommand <> "" Then
                            SendConsoleCommand(DominoServer.Name, Stat.ConsoleCommand)
                        End If
                        myAlert.QueueAlert("Domino", DominoServer.Name, "Domino Statistic: " & Stat.Statistic, "A custom statistic has passed its threshold: " & Stat.Statistic & " " & Stat.ComparisonOperator & " " & Stat.ThresholdValue & ".  Current Value is " & Stat.Value & ".", DominoServer.Location)
                    Else
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " " & DominoServer.Name & " " & Stat.Statistic & " is NOT in an Alert condition.")
                        myAlert.ResetAlert("Domino", DominoServer.Name, "Domino Statistic: " & Stat.Statistic, DominoServer.Location)
                    End If
                End If
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " General Error checking Domino Custom Statistics " & ex.Message)
            End Try
        Next
    End Sub

    Function GetDominoNumericStatistic(ByVal ServerName As String, ByVal FacilityName As String, ByVal StatName As String) As Double
        'This function returns a Long  for any Domino statistic that returns a number, such as server.users
        Dim intEqualLocation As Integer
        Dim intReturnLocation As Integer
        Dim strResult As String = ""
        Dim ReturnValue As Double = 0
        Dim ConnectAttempts As Integer = 100
        Dim Counter As Integer

        Try
            'Send a remote console command to get the value of the stat
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Requesting value of " & StatName)
            ' strResult = s.SendConsoleCommand(ServerName, "sh stat " & StatName)
            Do While strResult = ""
                strResult = GetOneStat(ServerName, FacilityName, StatName)
                Counter += 1
                If Counter > ConnectAttempts Then Exit Do
            Loop

            ' If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Unformatted server response is " & strResult)

            intEqualLocation = InStr(strResult, "=")
            intReturnLocation = InStr(strResult, vbCrLf)
            Dim intStart, intLength As Integer
            intStart = intEqualLocation + 1
            intLength = intReturnLocation - intEqualLocation - 1
            If intLength > 0 Then
                strResult = Mid(strResult, intStart, intLength)
            Else
                strResult = Mid(strResult, intStart, 1)
            End If

            intEqualLocation = Nothing
            intReturnLocation = Nothing
            intStart = Nothing

            strResult = Trim(strResult)
            '  If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " " & ServerName & " server response: " & strResult)

            Try
                ReturnValue = CType(strResult, Long)
            Catch
                Try
                    Dim myVal As String
                    myVal = strResult.Replace(".", ",")
                    ReturnValue = CType(myVal, Long)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " An error was detected in the Domino Numeric Statistic Module when querying " & ServerName & ": " & ex.Message)
                    ReturnValue = Nothing
                End Try
            End Try

            'ReturnValue = CType(strResult, Long)
        Catch ex As Exception
        Finally
            strResult = Nothing
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " DominoNumericStatistic has finished.")
        End Try

        If InStr(strResult, "NSPingServer failed") Then
            ReturnValue = Nothing
        End If

        Return ReturnValue
    End Function


    Function GetDominoTextStatistic(ByVal ServerName As String, ByVal FacilityName As String, ByVal StatName As String) As String
        'This function returns a string for any Domino statistic that returns text, such as Disk.C.Type returns NTFS
        '  Dim s As New Domino.NotesSession
        Dim intEqualLocation As Integer
        Dim intReturnLocation As Integer
        Dim strResult As String
        Dim ReturnValue As String = ""

        Try
            'Send a remote console command to get the value of the stat
            WriteAuditEntry(Now.ToString & " sending request for " & StatName & " to " & ServerName, LogLevel.Verbose)
            ' strResult = s.SendConsoleCommand(ServerName, "sh stat " & StatName)
            strResult = GetOneStat(ServerName, FacilityName, StatName)

            intEqualLocation = InStr(strResult, "=")
            intReturnLocation = InStr(strResult, vbCrLf)
            Dim intStart, intLength As Integer
            intStart = intEqualLocation + 1
            intLength = intReturnLocation - intEqualLocation - 1
            If intLength > 0 Then
                strResult = Mid(strResult, intStart, intLength)
            Else
                strResult = Mid(strResult, intStart, 1)
            End If
            strResult = Trim(strResult)
            '  If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & ServerName & " server response: " & strResult)

            'writeAuditEntry(Now.ToString & " DominoTextStatistic thread has finished.")
            If Not (InStr(strResult, "not found") > 0) Then
                ReturnValue = strResult
            Else
                ReturnValue = ""
            End If

        Catch ex As Exception
            WriteAuditEntry("Domino Text Statistic Module Error: " & ServerName & vbCrLf & ex.Message & vbCrLf & ex.Source)
            ReturnValue = ""
        Finally
            strResult = Nothing
            WriteAuditEntry(Now.ToString & " DominoNumericStatistic has finished.", LogLevel.Verbose)
        End Try


        Return ReturnValue
    End Function

    Private Function ParseNumericStatValue(ByVal DesiredStatistic As String, ByVal StatisticCollection As String) As Double
        'This statistic returns the numeric value of the desired statistic from the collection of all statistics
        'that is returned by a Domino server when the console command 'show stat' is typed: 

        'Disk.C.Free = 199,281,758,208
        'Disk.C.Size = 250,994,384,896
        'Disk.C.Type = NTFS
        'Disk.Fixed = 2

        'If Europe, value is
        'Disk.C.Free = 199.281.758.208

        '  WriteAuditEntry(Now.ToString & " ParseNumeric Stat is looking for " & DesiredStatistic)
        '  WriteAuditEntry(Now.ToString & " ParseNumeric Stat is searching in " & StatisticCollection)

        Dim intEqualLocation As Integer  'location of equal sign
        Dim intReturnLocation As Integer 'location of carriage return

        Dim ReturnValue As Double = -1
        Dim intStartLocation As Integer  'location of start of target stat

        intStartLocation = InStr(StatisticCollection.ToUpper, Trim(DesiredStatistic.ToUpper))
        If intStartLocation = 0 Then
            '   If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & DesiredStatistic & " was not found...look yourself: " & vbCrLf & StatisticCollection)
            Return 0
            Exit Function
        Else
            ' If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & DesiredStatistic & " was found at " & intStartLocation)
        End If

        intEqualLocation = InStr(intStartLocation, StatisticCollection, "=")
        intReturnLocation = InStr(intEqualLocation, StatisticCollection, vbCrLf)

        Dim intStart, intLength As Integer  'where to start and stop looking for the stat
        intStart = intEqualLocation + 1
        intLength = intReturnLocation - intEqualLocation - 1
        If intLength > 0 Then
            StatisticCollection = Mid(StatisticCollection, intStart, intLength)
        Else
            StatisticCollection = Mid(StatisticCollection, intStart, 1)
        End If

        StatisticCollection = Trim(StatisticCollection)
        ' If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & DesiredStatistic & " is now reduced to " & StatisticCollection)

        'Sometimes the numbers come in like this!
        ' Disk.C.Free = 5'075'542'016
        ' Disk.C.Size = 12'872'527'872

        Try
            If InStr(StatisticCollection, "'") Then
                StatisticCollection = StatisticCollection.Replace("'", "")
            End If

            If InStr(StatisticCollection, "ÿ") Then
                StatisticCollection = StatisticCollection.Replace("ÿ", "")
            End If

            If InStr(StatisticCollection, ",") Then
                StatisticCollection = StatisticCollection.Replace(",", "")
            End If
        Catch ex As Exception

        End Try


        'New code
        Dim myNum As Double
        Dim USprovider As IFormatProvider = CultureInfo.CreateSpecificCulture(sCultureString)
        Dim Europrovider As IFormatProvider = CultureInfo.CreateSpecificCulture("fr-FR")

        Try
            Double.TryParse(StatisticCollection.Trim, NumberStyles.Any Or NumberStyles.AllowDecimalPoint, USprovider, ReturnValue)
            If ReturnValue = 0 Then
                Double.TryParse(StatisticCollection.Trim, NumberStyles.Any Or NumberStyles.AllowDecimalPoint, Europrovider, ReturnValue)
            End If
        Catch ex As Exception
            ReturnValue = Nothing
        End Try

        If ReturnValue = Nothing Then
            ReturnValue = Double.Parse(StatisticCollection, System.Globalization.NumberStyles.Float)
        End If




        'old code
        'Try
        '    ' ReturnValue = CType(StatisticCollection, Double)
        '    ReturnValue = Double.Parse(StatisticCollection, System.Globalization.NumberStyles.Float)
        'Catch
        '    Try
        '        Dim myVal As String
        '        myVal = StatisticCollection.Replace(".", ",")
        '        ReturnValue = CType(myVal, Double)
        '    Catch ex As Exception
        '        '   WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " An error was detected in the Domino Numeric Statistic Module when querying " & ServerName & ": " & ex.Message)
        '        ReturnValue = Nothing

        '    End Try
        'End Try

        ' If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " ParseNumericStatValue is returning " & ReturnValue.ToString & " for " & DesiredStatistic)
        Return ReturnValue
    End Function

    Private Function ParseTextStatValue(ByVal DesiredStatistic As String, ByVal StatisticCollection As String) As String
        'This statistic returns the text value of the desired statistic from the collection of all statistics
        'that is returned by a Domino server when the console command 'show stat' is typed: 

        'Disk.C.Free = 199,281,758,208
        'Disk.C.Size = 250,994,384,896
        'Disk.C.Type = NTFS
        'Disk.Fixed = 2

        Dim intEqualLocation As Integer  'location of equal sign
        Dim intReturnLocation As Integer 'location of carriage return

        Dim ReturnValue As String = ""
        Dim intStartLocation As Integer  'location of start of target stat

        intStartLocation = InStr(StatisticCollection.ToUpper, DesiredStatistic.ToUpper)
        If intStartLocation = 0 Then
            'WriteAuditEntry(Now.ToString & " " & DesiredStatistic & " was not found. ", LogLevel.Verbose)
            Return ""
            Exit Function
        End If

        intEqualLocation = InStr(intStartLocation, StatisticCollection, "=")
        intReturnLocation = InStr(intEqualLocation, StatisticCollection, vbCrLf)

        Dim intStart, intLength As Integer  'where to start and stop looking for the stat
        intStart = intEqualLocation + 1
        intLength = intReturnLocation - intEqualLocation - 1
        If intLength > 0 Then
            StatisticCollection = Mid(StatisticCollection, intStart, intLength)
        Else
            StatisticCollection = Mid(StatisticCollection, intStart, 1)
        End If

        StatisticCollection = Trim(StatisticCollection)

        If InStr(StatisticCollection, "*ERROR*NSPingServer") > 0 Then
            StatisticCollection = ""
        End If
        Try
            ReturnValue = StatisticCollection
        Catch
            ReturnValue = Nothing
        End Try
        ' If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " ParseTextStatValue is returning " & ReturnValue.ToString & " for " & DesiredStatistic)

        Return ReturnValue
    End Function


    ' Get specific stat for a facility
    Private Function GetOneStat(ByVal ServerName As String, ByVal StatisticCategory As String, ByVal SpecificStat As String) As String
        Dim Stat As String
        Dim FormattedStat As String
        Try
            'If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
            Stat = GetStats(ServerName, StatisticCategory, SpecificStat)
            FormattedStat = FormatResults(Stat)
        Catch ex As Exception
            FormattedStat = ""
        Finally
            '   If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
        End Try

        Return FormattedStat
    End Function


    Function ParseTravelerPerformanceStats(ByVal StatisticCollection As String) As String
        Dim myResult As String = ""
        Dim LeftoverStats As String = StatisticCollection

        Do While InStr(StatisticCollection, "Traveler.DCA.DB_OPEN.Time.Histogram.") > 0
            Try

                Dim DesiredStatistic As String = "Traveler.DCA.DB_OPEN.Time.Histogram."
                Dim intEqualLocation As Integer  'location of equal sign
                Dim intReturnLocation As Integer 'location of carriage return

                Dim ReturnValue As String = ""
                Dim intStartLocation As Integer  'location of start of target stat

                intStartLocation = InStr(StatisticCollection.ToUpper, DesiredStatistic.ToUpper)
                Try
                    intEqualLocation = InStr(intStartLocation, StatisticCollection, "=")
                    intReturnLocation = InStr(intEqualLocation, StatisticCollection, vbCrLf)
                Catch ex As Exception

                End Try

                Dim intStart, intLength As Integer  'where to start and stop looking for the stat
                intStart = intStartLocation
                intLength = intReturnLocation - intStartLocation
                LeftoverStats = Mid(StatisticCollection, intReturnLocation)
                If intLength > 0 Then
                    StatisticCollection = Mid(StatisticCollection, intStart, intLength)
                Else
                    StatisticCollection = Mid(StatisticCollection, intStart, 1)
                End If
                Dim myStatValue As String

                myStatValue = Trim(StatisticCollection)
                '  txtParsed.Text = txtParsed.Text & myStatValue & vbCrLf
                myResult = myResult & myStatValue & vbCrLf
                ' txtAllStats.Text = LeftoverStats
                StatisticCollection = LeftoverStats
            Catch ex As Exception

            End Try
        Loop
        Return myResult
    End Function

    Private Function ParseTotalSecondsFromStat(ByVal stattext As String) As Long
        Dim daysstr As String
        Dim timestr As String
        Dim daysts As TimeSpan
        Dim timets As TimeSpan
        Dim totalsec As Long = 0

        If InStr(stattext, "days") > 0 Then
            daysstr = stattext.Substring(0, stattext.IndexOf("days") - 1).Trim()
            timestr = stattext.Substring(stattext.IndexOf("days") + 5, stattext.Length - (stattext.IndexOf("days") + 5)).Trim()
            daysts = New TimeSpan(Convert.ToInt32(daysstr), 0, 0, 0)
            timets = TimeSpan.Parse(timestr)
            totalsec = Convert.ToInt32(daysts.TotalSeconds) + Convert.ToInt32(timets.TotalSeconds)
        Else
            timestr = stattext.Substring(0, stattext.Length - 1).Trim()
            timets = TimeSpan.Parse(timestr)
            totalsec = Convert.ToInt32(timets.TotalSeconds)
        End If
        Return totalsec
    End Function
#End Region

#End Region


#Region "BlackBerry Queue Monitoring"

    'This function is not being used
    'Private Function CheckBESMessageQueue(ByRef s As Domino.NotesSession, ByVal BESDominoServer As MonitoredItems.BlackBerryQueue) As Long
    '    'This function searches the results of "sh ta" and returns the value of BES Pending Mail
    '    ' Expected String format:
    '    ' DBES Mail Agent   BES pending count 5873, sent 206, queued 5667
    '    '  Server.Task = DBES Mail Agent: BES pending count 0, sent 0, queued 0: [04/12/2011 18:04:41 MST]

    '    '  Dim s As New Domino.NotesSession
    '    Dim intDBESLocation As Integer
    '    Dim intCommaLocation As Integer
    '    Dim strResult As String
    '    Dim ReturnValue As String = ""
    '    'Try
    '    '   ' s.Initialize(MyDominoPassword)
    '    'Catch ex As Exception
    '    '   ' System.Runtime.InteropServices.Marshal.ReleaseComObject(s)
    '    '    WriteAuditEntry(Now.ToString + ": Error creating NotesSession in CheckBESMessageQueue module " & ex.Message)
    '    '    Exit Function
    '    'End Try


    '    Try
    '        'Send a remote console command to get the value of the stat
    '        strResult = s.SendConsoleCommand(BESDominoServer.Name, "sh ta")
    '        ' strResult = " DBES Mail Agent   BES pending count 350, sent 206, queued 5667"
    '        ' WriteAuditEntry(Now.ToString & " Response is : " & strResult)
    '        BESDominoServer.IncrementUpCount()
    '    Catch ex As Exception
    '        BESDominoServer.IncrementDownCount()
    '        WriteAuditEntry(Now.ToString & " " & ex.Message)
    '    End Try

    '    'Try
    '    '    System.Runtime.InteropServices.Marshal.ReleaseComObject(s)
    '    'Catch ex As Exception
    '    '    WriteAuditEntry(Now.ToString & " CheckBESMessageQueue Module Error releasing NotesSession: " & ex.Message)
    '    'End Try



    '    Try
    '        intDBESLocation = InStr(strResult, "BES pending count ")
    '        If intDBESLocation > 0 Then
    '            intCommaLocation = InStr(intDBESLocation + 18, strResult, ",")
    '            Dim intStart, intLength As Integer
    '            intStart = intDBESLocation + 18  '18 characters in 'BES pending count '
    '            intLength = intCommaLocation - intDBESLocation - 18
    '            If intLength > 0 Then
    '                strResult = Mid(strResult, intStart, intLength)
    '            Else
    '                strResult = Mid(strResult, intStart, 1)
    '            End If
    '            strResult = Trim(strResult)
    '            WriteAuditEntry(Now.ToString & " " & BESDominoServer.Name & " BES Pending Messages: " & strResult)

    '            WriteAuditEntry(Now.ToString & " CheckBESMessageQueue thread has finished.")
    '            ReturnValue = strResult
    '        Else
    '            ReturnValue = -999  'This means the search string was not found
    '        End If

    '        strResult = Nothing
    '    Catch ex As Exception
    '        WriteAuditEntry("CheckBESMessageQueue Module Error: " & BESDominoServer.Name & vbCrLf & ex.Message & vbCrLf & ex.Source)
    '        Return Nothing

    '    End Try

    '    Return ReturnValue

    'End Function

#End Region


#Region "Scheduled Domino Server Console Commands"


    Private Function SendConsoleCommand(ByVal Servername As String, ByVal ConsoleCommand As String) As String
        'This subroutine sends the specified console command to the specified server

        ' Dim s As New Domino.NotesSession
        Dim strResult As String

        '

        Try
            'Send a remote console command to get the value of the stat
            strResult = NotesSession.SendConsoleCommand(Servername, ConsoleCommand)
            WriteConsoleCommandHistoryEntry(Now.ToString & " Sent Command '" & ConsoleCommand & "' to " & Servername)
        Catch ex As Exception
            WriteConsoleCommandHistoryEntry(Now.ToString & " Error sending Command '" & ConsoleCommand & "' to " & Servername & ". Error was: " & ex.Message)
            WriteAuditEntry(Now.ToString & " Scheduled Console Command Module Error: " & Servername & vbCrLf & ex.Message)
        Finally

            WriteAuditEntry(Now.ToString & " Scheduled Console Command Module has finished. ")
        End Try

        Return strResult
    End Function

#End Region




End Class