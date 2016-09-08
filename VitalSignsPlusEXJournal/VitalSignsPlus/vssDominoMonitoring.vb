Imports System.Threading
Imports System.IO
Imports System.Globalization
Imports MonitoredItems.MonitoredDevice.Alert
Imports VSFramework
Imports System.Net.Sockets
Imports System.Collections.Generic

Imports MongoDB.Driver
Imports VSNext.Mongo.Entities
Partial Public Class VitalSignsPlusExJournal


#Region "Domino Monitoring"


    Private Sub MonitorDomino() 'This is the main sub that calls all the other modules
        dll.W32_NotesInitThread()
        Do While boolTimeToStop <> True
            Dim oWatch As New System.Diagnostics.Stopwatch

            If boolTimeToStop = True Then
                Exit Do
            End If
            ' WriteAuditEntry(Now.ToString & " >>> Begin Loop for Domino Monitoring >>>>")
            dtDominoLastUpdate = Now
            Dim myServer As MonitoredItems.DominoServer
            Try
                DominoSelector_Mutex.WaitOne()
                myServer = SelectDominoServerToMonitor()
                myServer.IsBeingScanned = True
                myServer.LastScan = Now
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
                    If myServer.Enabled = True Then
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Selected " & myServer.Name & " --- with current status of " & myServer.Status & ". Next scan due: " & myServer.NextScan)

                        myServer.LastScan = Date.Now
                        'If Trim(myServer.Status) = "OK" Or Trim(myServer.Status) = "Not Scanned" Then
                        '    myServer.Status = "Scanning"
                        'End If
                    End If
                End If

            Catch ex As Exception

            End Try

            Try
                'WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " >>> Begin Loop to monitor " & myServer.Name)
                oWatch.Start()
            Catch ex As Exception

            End Try

            Try
                If myServer.Enabled = False Then
                    myServer.LastScan = Now
                    myServer.Status = "Disabled"
                    myServer.ResponseDetails = "This server is not enabled for monitoring."
                    GoTo WaitHere
                End If
            Catch ex As Exception

            End Try

            Try
                If InMaintenance("Domino", myServer.Name) = True Then
                    'Don't check it if it is in maintenance
                Else
                    '8/7/2015 NS modified for VSPLUS-1802
                    If myServer.EXJEnabled = True Then
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Begin EXJournal check of " & myServer.Name & ".  Ping count = " & myServer.PingCount & ", Scan count = " & myServer.ScanCount & " and status of " & myServer.Status)
                        myServer.IsBeingScanned = True
                        EXJournalCheck(myServer)
                        myServer.IsBeingScanned = False
                        myServer.LastScan = Date.Now
                    End If
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Error while EXJournal scanning " & ex.ToString)

            End Try



WaitHere:

            Try
                oWatch.Stop()
                myServer.IsBeingScanned = False
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " --------------------> End Loop to monitor " & myServer.Name & "  Elapsed Time= " & oWatch.Elapsed.TotalSeconds & " seconds. Next scan due: " & myServer.NextScan)

                myServer = Nothing
            Catch ex As Exception

            End Try

            Thread.Sleep(30000)
        Loop

        dll.W32_NotesTermThread()

    End Sub


    Private Function SelectDominoServerToMonitor() As MonitoredItems.DominoServer
        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " >>> Selecting a Domino Server for exJournal scanning >>>>", LogUtilities.LogUtils.LogLevel.Verbose)
        Dim tNow As DateTime
        tNow = Now
        Dim tScheduled As DateTime

        Dim timeOne, timeTwo As DateTime

        Dim SelectedServer As MonitoredItems.DominoServer

        Dim ServerOne As MonitoredItems.DominoServer
        Dim ServerTwo As MonitoredItems.DominoServer
        Dim myRegistry As New RegistryHandler
        Dim ScanASAP As String = ""
        Try
            ScanASAP = myRegistry.ReadFromRegistry("ScanDominoASAP")
            ' WriteAuditEntry(Now.ToString & " >>> " & ScanASAP & " was marked 'Scan ASAP'")
        Catch ex As Exception
            ScanASAP = ""
        End Try

        Dim n As Integer

        For n = 0 To MyDominoServers.Count - 1
            ServerOne = MyDominoServers.Item(n)
            '6/18/2015 NS modified for VSPLUS-1802
            'If ServerOne.Name = ScanASAP And ServerOne.IsBeingScanned = False Then
            If ServerOne.Name = ScanASAP And ServerOne.IsBeingScanned = False And ServerOne.EXJEnabled = True Then
                'This server has been marked to be scanned right away
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " >>> " & ScanASAP & " was marked 'Scan ASAP' so that will be scanned next.", LogUtilities.LogUtils.LogLevel.Verbose)
                'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " >>> Selecting " & ServerOne.Name & " because it was marked 'Scan ASAP'")
                myRegistry.WriteToRegistry("ScanDominoASAP", "n/a")
                myRegistry = Nothing
                ServerOne.IsBeingScanned = True
                Return ServerOne
                Exit Function
            End If
        Next n


        'Any server Not Scanned should be scanned right away.  Select the first one you encounter
        For n = 0 To MyDominoServers.Count - 1
            ServerOne = MyDominoServers.Item(n)
            '6/18/2015 NS modified for VSPLUS-1802
            'If ServerOne.Status = "Not Scanned" And ServerOne.Enabled = True And ServerOne.Traveler_Server = True And ServerOne.IsBeingScanned = False Then
            If ServerOne.Status = "Not Scanned" And ServerOne.Enabled = True And ServerOne.Traveler_Server = True And _
                ServerOne.IsBeingScanned = False And ServerOne.EXJEnabled = True Then
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " >>> Selecting " & ServerOne.Name & " because it is a Traveler server and the status is " & ServerOne.Status, LogUtilities.LogUtils.LogLevel.Verbose)
                ServerOne.IsBeingScanned = True
                Return ServerOne
                Exit Function
            End If
        Next


        'Any server Not Responding that is due for a scan should be scanned right away.  Select the first one you encounter
        For n = 0 To MyDominoServers.Count - 1
            ServerOne = MyDominoServers.Item(n)
            '6/18/2015 NS modified for VSPLUS-1802
            'If ServerOne.Status = "Not Responding" And ServerOne.Enabled = True And ServerOne.IsBeingScanned = False Then
            If ServerOne.Status = "Not Responding" And ServerOne.Enabled = True And ServerOne.IsBeingScanned = False And _
                ServerOne.EXJEnabled = True Then
                tScheduled = CDate(ServerOne.NextScan)
                If DateTime.Compare(tNow, tScheduled) > 0 Then
                    WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status & ".  Next scheduled scan is " & tScheduled.ToShortTimeString, LogUtilities.LogUtils.LogLevel.Verbose)
                    'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status)
                    ServerOne.IsBeingScanned = True
                    Return ServerOne
                    Exit Function
                Else
                    'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " " & ServerOne.Name & " is down, but not yet due to be re-scanned.")
                End If
            End If
        Next

        'Any server Not Scanned should be scanned right away.  Select the first one you encounter
        For n = 0 To MyDominoServers.Count - 1
            ServerOne = MyDominoServers.Item(n)
            '6/18/2015 NS modified for VSPLUS-1802
            'If ServerOne.Status = "Not Scanned" And ServerOne.Enabled = True And ServerOne.IsBeingScanned = False Then
            If ServerOne.Status = "Not Scanned" And ServerOne.Enabled = True And ServerOne.IsBeingScanned = False And _
                ServerOne.EXJEnabled = True Then
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status, LogUtilities.LogUtils.LogLevel.Verbose)
                ServerOne.IsBeingScanned = True
                Return ServerOne
                Exit Function
            End If
        Next


        Dim ScanCandidates As New MonitoredItems.DominoCollection
        For Each srv As MonitoredItems.DominoServer In MyDominoServers
            '6/18/2015 NS modified for VSPLUS-1802
            'If srv.IsBeingScanned = False And srv.Enabled = True Then
            If srv.IsBeingScanned = False And srv.Enabled = True And srv.EXJEnabled = True Then
                ScanCandidates.Add(srv)
                ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " " & srv.Name & " is not being scanned.")
            Else
                ' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " " & srv.Name & " is being scanned.")
            End If
        Next
        'WriteDeviceHistoryEntry("All", "Selection",vbCrLf & vbCrLf)
        'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " *********** Scan Candidates *************")
        For Each srv As MonitoredItems.DominoServer In ScanCandidates
            'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " " & srv.Name & " is a candidate to be scanned.  Last scan: " & srv.LastScan)
        Next

        If ScanCandidates.Count = 0 Then
            'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " All servers are already being scanned, exiting sub.")
            Return Nothing
        End If

        '*****************

        'start with the first two servers
        ServerOne = ScanCandidates.Item(0)
        ServerOne = ScanCandidates.Item(0)
        If ScanCandidates.Count > 1 Then ServerTwo = ScanCandidates.Item(1)

        Try
            'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Finding which server is the most overdue to be monitored.")
            'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Server One is  " & ServerOne.Name & ", due to be scanned " & ServerOne.NextScan)
            'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Server Two " & ServerTwo.Name & ", due to be scanned " & ServerTwo.NextScan)
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
                        'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " " & ServerOne.Name & " is more overdue ")
                        ServerTwo = ScanCandidates.Item(n)
                    Else
                        'time two is later than time one, so keep server 2
                        ServerOne = ScanCandidates.Item(n)
                        'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " " & ServerTwo.Name & " is more overdue ")
                    End If

                    'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Server One is  " & ServerOne.Name & ", due to be scanned " & ServerOne.NextScan)
                    'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Server Two " & ServerTwo.Name & ", due to be scanned " & ServerTwo.NextScan)

                Next
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " >>> Error Selecting Domino server... " & ex.Message)
            End Try
        Else
            'There were only two server, so use those going forward
        End If

        'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " >>> Down to two servers... " & ServerOne.Name & " and " & ServerTwo.Name)

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
            'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " >>> Down to one server... " & SelectedServer.Name & " to scan at " & SelectedServer.NextScan & ". Status is " & SelectedServer.Status)
        Else
            SelectedServer = ServerOne
            tScheduled = CDate(ServerOne.NextScan)
        End If

        tScheduled = CDate(SelectedServer.NextScan)
        If DateTime.Compare(tNow, tScheduled) < 0 Then
            If SelectedServer.Status <> "Not Scanned" Then
                'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " No Domino servers scheduled for monitoring, next scan after " & SelectedServer.NextScan)
                SelectedServer = Nothing
            Else
                WriteAuditEntry(Now.ToString & " selected Domino server: " & SelectedServer.Name & " because it has not been scanned yet.")
            End If
        Else
            'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " selected Domino server: " & SelectedServer.Name)
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
        SelectedServer.IsBeingScanned = True
        SelectDominoServerToMonitor = SelectedServer


        'Exit Function
        SelectedServer = Nothing
    End Function


    Private Sub EXJournalCheck(ByRef MyDominoServer As MonitoredItems.DominoServer)
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, vbCrLf & Now.ToString & " --------------------------------")
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Starting new EXJournal cycle.")
        MyDominoServer.Status = "OK"
        Dim oWatch As New System.Diagnostics.Stopwatch
        oWatch.Start()
        Dim myRegistry As New RegistryHandler
        Dim objVSAdaptor As New VSAdaptor
        '6/25/2015 NS added for VSPLUS-1802
        '10/29/2015 NS modified for VSPLUS-1802
        Dim startDT As DateTime
        Dim endDT As DateTime
        '10/29/2015 NS added for VSPLUS-1802
        Dim isBetween As Boolean
        Dim startTS As TimeSpan
        Dim endTS As TimeSpan
        'This sub coordinates the actual monitoring of a Domino server
        Dim ResponseTime As Double = 0
        dtDominoLastUpdate = Now
        Dim strSQL As String
        '11/4/2015 NS added for VSPLUS-2324
        Dim strBody As String

        Try
            WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " EXJournalCheck thread is attempting ExJournal SCAN of  " & MyDominoServer.Name & ". ")
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino thread is attempting ExJournal SCAN of  " & MyDominoServer.Name & ". This server has " & MyDominoServer.FailureCount & " consecutive failures.")
            '   If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & MyDominoServer.Name & " was scheduled for " & MyDominoServer.NextScan & " will next be scanned at " & MyDominoServer.NextScan)
        Catch ex As Exception

        End Try

        Try
            ' MyDominoServer.AlertCondition = False
            MyDominoServer.ResponseTime = 0
        Catch ex As Exception

        End Try

        Try
            If InStr(MyDominoServer.Name, "RPRWyattTest") Then
                MyDominoServer.Status = "OK"
                ResponseTime = 25
                MyDominoServer.EXJournal1_DocCount = GetRandomNumber(1, 1000)
                MyDominoServer.EXJournal2_DocCount = GetRandomNumber(1, 1000) ' CInt(Math.Ceiling(Rnd() * 1000))
                MyDominoServer.EXJournal_DocCount = GetRandomNumber(1, 1000)

                If MyDominoServer.EXJournal1_DocCount < 100 Then
                    MyDominoServer.EXJournal1_DocCount = -1
                    MyDominoServer.EXJournal2_DocCount = -1
                    MyDominoServer.EXJournal_DocCount = -1
                End If
                GoTo Update
            End If
        Catch ex As Exception

        End Try


        Try
            If ResponseTime = 0 Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Testing server response via Notes API function prior to initiating EXJournal Check.")
                ResponseTime = DominoResponseTime(MyDominoServer.Name)
                MyDominoServer.ResponseTime = ResponseTime
            End If
        Catch ex As Exception
            MyDominoServer.ResponseTime = 0
            ResponseTime = 0
        End Try


        If ResponseTime = 0 Then
            WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " The server " & MyDominoServer.Name & " did not respond so I am skipping EXJournal Check for now.")
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The server did not respond so I am skipping EXJournal Check for now.")
            ' WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Evaluating whether to test server via telnet function.")
            '  myAlert.QueueAlert("Domino", MyDominoServer.Name, "Not Responding", "The Domino server " & MyDominoServer.Name & " is not responding.  This was first detected " & Now.ToShortDateString & "  " & Now.ToShortTimeString, MyDominoServer.Location)
            Exit Sub
        Else
            ' myAlert.ResetAlert("Domino", MyDominoServer.Name, "Not Responding", MyDominoServer.Location)
        End If

        Dim db As Domino.NotesDatabase


        If ResponseTime <> 0 Then

            Try
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Attempting to open exjournal1.nsf on " & MyDominoServer.Name)
                db = NotesSession.GetDatabase(MyDominoServer.Name, "EXJournal1.nsf", False)
                If Not db Is Nothing Then
                    ' db.Open()
                    MyDominoServer.EXJournal1_DocCount = db.AllDocuments.Count
                    WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exjournal.nsf on " & MyDominoServer.Name & " has " & MyDominoServer.EXJournal1_DocCount & " documents. ")
                Else
                    MyDominoServer.EXJournal1_DocCount = -1
                End If
            Catch ex As Exception
                MyDominoServer.EXJournal1_DocCount = -1
            End Try
            WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exjournal.nsf on " & MyDominoServer.Name & " has " & MyDominoServer.EXJournal1_DocCount & " documents. ")


            Try
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Attempting to open exjournal2.nsf on " & MyDominoServer.Name)
                db = NotesSession.GetDatabase(MyDominoServer.Name, "EXJournal2.nsf", False)
                ' db.Open()
                If Not db Is Nothing Then
                    MyDominoServer.EXJournal2_DocCount = db.AllDocuments.Count
                Else
                    MyDominoServer.EXJournal2_DocCount = -1

                End If
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exjournal2.nsf on " & MyDominoServer.Name & " has " & MyDominoServer.EXJournal2_DocCount & " documents. ")
            Catch ex As Exception
                MyDominoServer.EXJournal2_DocCount = -1
            End Try

            Try
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Attempting to open exjournal3.nsf on " & MyDominoServer.Name)
                db = NotesSession.GetDatabase(MyDominoServer.Name, "EXJournal3.nsf", False)
                If Not db Is Nothing Then
                    'db.Open()
                    MyDominoServer.EXJournal_DocCount = db.AllDocuments.Count
                Else
                    MyDominoServer.EXJournal_DocCount = -1

                End If
            Catch ex As Exception
                MyDominoServer.EXJournal_DocCount = -1
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exception opening exjournal3.nsf on " & MyDominoServer.Name & ": " & ex.ToString)
            End Try

            Try
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exjournal3.nsf on " & MyDominoServer.Name & " has " & MyDominoServer.EXJournal_DocCount & " documents. ")
            Catch ex As Exception

            End Try

        End If
        '9/17/2015 NS added
        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Finished If ResponseTime <> 0 loop.", LogUtilities.LogUtils.LogLevel.Verbose)
Update:

        '9/17/2015 NS added
        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Trying to compare document counts now.", LogUtilities.LogUtils.LogLevel.Verbose)
        If ResponseTime <> 0 Then
            Dim mySum As Long
            Try
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Before getting the total.", LogUtilities.LogUtils.LogLevel.Verbose)
                mySum = MyDominoServer.EXJournal2_DocCount + MyDominoServer.EXJournal1_DocCount + MyDominoServer.EXJournal_DocCount
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " " & MyDominoServer.Name & " has " & mySum & " EXJournal documents.")
            Catch ex As Exception
                mySum = 0
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Getting the total FAILED.", LogUtilities.LogUtils.LogLevel.Verbose)
            End Try

            'Update EXJournal counts

            Try

                If mySum >= 0 Then
                    WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " mySum >= 0 condition is true.", LogUtilities.LogUtils.LogLevel.Verbose)
                    With MyDominoServer

                        Try
                            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.Type, VSNext.Mongo.Entities.Enums.ServerType.Domino.ToString()) _
                                                                                                 And repository.Filter.Eq(Function(x) x.Name, .Name)
                            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                                 .Set(Function(x) x.Exjournal, Convert.ToInt32(.EXJournal_DocCount)) _
                                                                                                 .Set(Function(x) x.Exjournal1, Convert.ToInt32(.EXJournal1_DocCount)) _
                                                                                                 .Set(Function(x) x.Exjournal2, Convert.ToInt32(.EXJournal2_DocCount)) _
                                                                                                 .Set(Function(x) x.ExjournalDate, Now)

                            repository.Update(filterDef, updateDef)

                        Catch ex As Exception
                            WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exception updating EXJournal Status for EXJournal Stats. Exception : " & ex.Message)
                        End Try


                    End With

                    '4/25/2016 NS added for VSPLUS-2806
                    'Inserting EXJournal document count total into the DominoDailyStats table
                    WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Updating DominoDailyStats with the EXJournal document total = " & mySum.ToString())

                    Try
                        Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
                        Dim entity As New VSNext.Mongo.Entities.DailyStatistics() With {
                            .DeviceId = MyDominoServer.ServerObjectID,
                            .DeviceType = VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription(),
                            .DeviceName = MyDominoServer.Name,
                            .StatName = "EXJournal.DocCount.Total",
                            .StatValue = mySum.ToString()
                        }
                        repository.Insert(entity)
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exception inserting EXJournal daily stat for EXJournal.DocCount.Total. Exception : " & ex.Message)
                    End Try

                    '6/19/2015 NS added for VSPLUS-1802
                    '8/7/2015 NS modified for VSPLUS-1802
                    WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Before getting the look back parameters.", LogUtilities.LogUtils.LogLevel.Verbose)
                    WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Look back parameters are: EXJDuration - " & MyDominoServer.EXJDuration &
                        ", EXJLookBackDuration - " & MyDominoServer.EXJLookBackDuration & ", EXJStartTime - " & MyDominoServer.EXJStartTime, LogUtilities.LogUtils.LogLevel.Verbose)
                    If MyDominoServer.EXJDuration <> 0 And MyDominoServer.EXJLookBackDuration <> 0 And MyDominoServer.EXJStartTime <> "" Then
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Got the look back parameters.", LogUtilities.LogUtils.LogLevel.Verbose)
                        Call UpdateEXJournalStatsTable(MyDominoServer)
                        startDT = Convert.ToDateTime(MyDominoServer.EXJStartTime)
                        endDT = startDT.AddMinutes(MyDominoServer.EXJDuration)
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Start: " & startDT & ", End: " & endDT)
                        '10/29/2015 NS modified for VSPLUS-1802
                        startDT = DateTime.ParseExact(MyDominoServer.EXJStartTime, "h:mm tt", CultureInfo.InvariantCulture)
                        startTS = startDT.TimeOfDay
                        endDT = DateTime.ParseExact(endDT.TimeOfDay.ToString(), "HH:mm:ss", CultureInfo.InvariantCulture)
                        endTS = endDT.TimeOfDay
                        isBetween = TimeBetween(DateTime.Now, startTS, endTS)
                        If isBetween Then
                            WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Comparing document counts for the look back period ")
                            Call CompareEXJournalDocCounts(MyDominoServer)
                        Else
                            WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " The current time is not within the look back interval. ", LogUtilities.LogUtils.LogLevel.Verbose)
                        End If
                    Else
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " One of the look back parameters is 0 or empty. ", LogUtilities.LogUtils.LogLevel.Verbose)
                    End If
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " The following exception has occurred: " & ex.Message)
            End Try

            'MyDominoServer.ResponseDetails += vbCrLf & vbCrLf & "The sum of the documents in the EXJournal databases is " & mySum

            Try
                If mySum > myThreshold Then
                    MyDominoServer.Status = "EXJournal"
                    MyDominoServer.Description = "The sum of the documents in the EXJournal databases is " & mySum

                    '11/4/2015 NS modified for VSPLUS-2324
                    strBody = "The sum of the EXJournal databases exceeds the alert threshold of " & myThreshold & "." & vbCrLf & vbCrLf
                    If MyDominoServer.EXJournal_DocCount > -1 Then
                        '11/4/2015 NS modified for VSPLUS-2325
                        strBody += "The database 'EXJournal3.nsf' currently has " & MyDominoServer.EXJournal_DocCount & " documents in it." & vbCrLf
                    End If
                    If MyDominoServer.EXJournal1_DocCount > -1 Then
                        strBody += "The database 'EXJournal1.nsf' currently has " & MyDominoServer.EXJournal1_DocCount & " documents in it." & vbCrLf
                    End If
                    If MyDominoServer.EXJournal2_DocCount > -1 Then
                        strBody += "The database 'EXJournal2.nsf' currently has " & MyDominoServer.EXJournal2_DocCount & " documents in it."
                    End If
                    myAlert.QueueAlert("Domino", MyDominoServer.Name, "EXJournal", strBody, MyDominoServer.Location)
                    Try
                        Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                        Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.Type, VSNext.Mongo.Entities.Enums.ServerType.Domino.ToString()) _
                                                                                             And repository.Filter.Eq(Function(x) x.Name, MyDominoServer.Name)
                        Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                             .Set(Function(x) x.Description, MyDominoServer.Description)

                        repository.Update(filterDef, updateDef)

                    Catch ex As Exception
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exception updating ExJournal Status Description. Exception : " & ex.Message)
                    End Try

                Else
                    If mySum >= 0 Then

                        Try
                            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.Type, VSNext.Mongo.Entities.Enums.ServerType.Domino.ToString()) _
                                                                                                 And repository.Filter.Eq(Function(x) x.Name, MyDominoServer.Name)
                            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                                 .Set(Function(x) x.Description, "EXJournal databases are below threshold.")

                            repository.Update(filterDef, updateDef)

                        Catch ex As Exception
                            WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exception updating ExJournal Status Description. Exception : " & ex.Message)
                        End Try

                    End If
                    myAlert.ResetAlert("Domino", MyDominoServer.Name, "EXJournal", MyDominoServer.Location)
                End If
            Catch ex As Exception

            End Try


        End If


        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
        Catch ex As Exception

        End Try

    End Sub
    '10/29/2015 NS added for VSPLUS-1802
    Public Function TimeBetween(ByRef datetime As DateTime, ByRef tstart As TimeSpan, ByRef tend As TimeSpan) As Boolean
        Dim now As TimeSpan
        now = datetime.TimeOfDay
        'see if start comes before end
        If (tstart < tend) Then
            Return tstart <= now And now <= tend
        End If
        'start is after end, so do the inverse comparison
        Return Not (tend < now And now < tstart)
    End Function
    '6/19/2015 NS added for VSPLUS-1802
    Public Sub UpdateEXJournalStatsTable(ByRef myDominoServer As MonitoredItems.DominoServer)
        Dim cn As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim countVal As Int32
        Dim countValPrev As Int32
        Dim deltaVal As Int32 = 555 ' default value - the first time the service runs, there will be no 0 delta
        Dim found As Boolean
        Dim sqlReader As SqlClient.SqlDataReader
        Dim datetimeVal As Date
        Dim dbArr(2) As String
        Dim dtNow As DateTime = Now

        dbArr(0) = "EXJournal1.nsf"
        dbArr(1) = "EXJournal2.nsf"
        dbArr(2) = "EXJournal3.nsf"

        datetimeVal = Nothing
        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.ExJournalStats)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.ExJournalStats)
            Dim entity As VSNext.Mongo.Entities.ExJournalStats
            For i As Integer = 0 To UBound(dbArr)
                If i = 0 Then
                    countVal = myDominoServer.EXJournal1_DocCount
                ElseIf i = 1 Then
                    countVal = myDominoServer.EXJournal2_DocCount
                ElseIf i = 2 Then
                    countVal = myDominoServer.EXJournal_DocCount
                End If

                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " The current value of the doc count for " & dbArr(i) & " on " & myDominoServer.Name & " is " & countVal)

                Try

                    filterDef = repository.Filter.Eq(Function(x) x.ExJournalDatabase, dbArr(i)) _
                        And repository.Filter.Eq(Function(x) x.DeviceName, myDominoServer.Name)

                    entity = repository.Find(filterDef, Function(x) x.ModifiedOn, 0, 1, True)(0)



                    WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Top of read loop.", LogUtilities.LogUtils.LogLevel.Verbose)
                    Try
                        countValPrev = entity.DocumentCount
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Previous doc count value for " & dbArr(i) & " on " & myDominoServer.Name & " is " & countValPrev)
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exception calculating countValPrev: " & ex.ToString)
                    End Try

                    Try
                        datetimeVal = entity.ModifiedOn
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Previous date value for " & dbArr(i) & " on " & myDominoServer.Name & " is " & datetimeVal)
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exception calculating datetimeVal: " & ex.ToString)
                    End Try

                    Try
                        deltaVal = countVal - countValPrev
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " The delta value for " & dbArr(i) & " is " & deltaVal)
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exception calculating deltaVal: " & ex.ToString)
                    End Try

                    found = True
                    WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Bottom of read loop.", LogUtilities.LogUtils.LogLevel.Verbose)


                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " UpdateEXJournalStatsTable has an exception in getting the entity. Exception : " & ex.Message)
                End Try

                Try
                    entity = New VSNext.Mongo.Entities.ExJournalStats()
                    entity.ExJournalDatabase = dbArr(i)
                    entity.Delta = deltaVal
                    entity.DocumentCount = countVal
                    entity.DeviceName = myDominoServer.Name
                    repository.Insert(entity)

                    '10/19/2015 NS added - reset delta

                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " UpdateEXJournalStatsTable has an exception in inserting the entity. Exception : " & ex.Message)
                End Try
                deltaVal = 555
            Next

        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Error while inserting a record in the table..." & ex.Message)
        Finally
            cn.Close()
        End Try
    End Sub
    '6/22/2015 NS added for VSPLUS-1802
    Public Sub CompareEXJournalDocCounts(ByRef myDominoServer As MonitoredItems.DominoServer)
        Dim cn As New SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim countVal As Int32
        Dim countValPrev As Int32
        '9/23/2015 NS added for VSPLUS-2196
        Dim countValTotal As Int32
        Dim countValPrevTotal As Int32
        Dim lookbackPeriod As Int32
        Dim sqlReader As SqlClient.SqlDataReader
        Dim dbArr(2) As String
        '2/11/2016 NS added for VSPLUS-2598
        Dim foundRows As Boolean

        dbArr(0) = "EXJournal1.nsf"
        dbArr(1) = "EXJournal2.nsf"
        dbArr(2) = "EXJournal3.nsf"

        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.ExJournalStats)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.ExJournalStats)
            Dim entity As VSNext.Mongo.Entities.ExJournalStats
            '9/23/2015 NS added for VSPLUS-2196
            countValTotal = 0
            countValPrevTotal = 0
            For i As Integer = 0 To UBound(dbArr)
                '2/11/2016 NS added for VSPLUS-2598

                
                foundRows = False
                lookbackPeriod = myDominoServer.EXJLookBackDuration

                Try
                    filterDef = repository.Filter.Eq(Function(x) x.ExJournalDatabase, dbArr(i)) _
                        And repository.Filter.Eq(Function(x) x.DeviceName, myDominoServer.Name)

                    entity = repository.Find(filterDef, Function(x) x.ModifiedOn, 0, 1, True)(0)

                    Try
                        countVal = entity.DocumentCount
                        '9/23/2015 NS added for VSPLUS-2196
                        If (countVal > -1) Then
                            countValTotal += countVal
                        End If
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Current doc count value for " & dbArr(i) & " on " & myDominoServer.Name & " is " & countVal)
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exception calculating countVal in CompareEXJournalDocCounts: " & ex.ToString)
                    End Try

                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " CompareEXJournalDocCounts has an exception in getting the entity. Exception : " & ex.Message)
                End Try

                Try

                    filterDef = repository.Filter.Eq(Function(x) x.ExJournalDatabase, dbArr(i)) _
                        And repository.Filter.Eq(Function(x) x.DeviceName, myDominoServer.Name)

                    entity = repository.Find(filterDef, Function(x) x.ModifiedOn, 0, 1, True)(0)

                    filterDef = filterDef And repository.Filter.Lte(Function(x) x.ModifiedOn, entity.ModifiedOn.Value.AddMinutes(-1 * lookbackPeriod))

                    Dim listOfEntities As List(Of VSNext.Mongo.Entities.ExJournalStats) = repository.Find(filterDef)

                    For Each entity In listOfEntities
                        foundRows = True
                        Try
                            countValPrev = entity.DocumentCount
                            '9/23/2015 NS added
                            If (countValPrev > -1) Then
                                countValPrevTotal += countValPrev
                            End If
                            WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Previous doc count value for " & dbArr(i) & " on " & myDominoServer.Name & " is " & countValPrev)
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exception calculating countValPrev in CompareEXJournalDocCounts: " & ex.ToString)
                        End Try
                    Next

                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Exception looping in CompareEXJournalDocCounts: " & ex.ToString)
                End Try

            Next
            WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Count totals - current: " & countValTotal & ", previous: " & countValPrevTotal & " for " & myDominoServer.Name)
            '9/23/2015 NS modified for VSPLUS-2196
            If countValTotal = countValPrevTotal Then
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Attempting to send a doc count notification for " & myDominoServer.Name)
                '10/8/2015 NS modified the subject line to make it more obvious
                myAlert.QueueAlert("Domino", myDominoServer.Name, "EXJournal - document count unchanged", "The number of documents in ALL EXJournal databases on " & myDominoServer.Name & " has remained the same for the look back period of " & lookbackPeriod & " minutes." & vbCrLf & vbCrLf & "The EXJournal service may be experiencing an issue.", myDominoServer.Location)
            Else
                '2/11/2016 NS modified for VSPLUS-2598
                If foundRows = True Then
                    '10/8/2015 NS modified the subject line to make it more obvious
                    myAlert.ResetAlert("Domino", myDominoServer.Name, "EXJournal - document count unchanged", myDominoServer.Location)
                Else
                    WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " No rows found to compare the current document count.", LogUtilities.LogUtils.LogLevel.Verbose)
                End If
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " The following error has occurred: " & ex.Message)
        Finally
            cn.Close()
        End Try
    End Sub
    Function DominoResponseTime(ByVal Server As String) As Double
        'This Function returns response time in milliseconds
        WriteDeviceHistoryEntry("Domino", Server, Now.ToString & " Attempting to contact Domino server: " & Server & " to measure response time.")
        dtDominoLastUpdate = Now
        Dim ReturnValue As Double = 0
        Dim strResponseTime As String = ""
        Dim n As Integer
        Dim myName As String = ""
        Dim retryTime As Double

        Dim UpperBound As Integer = 3

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

        Dim oWatch As New System.Diagnostics.Stopwatch
        oWatch.Start()
        For n = 1 To UpperBound

            Try
                strResponseTime = GetResponseTime(Server)
                ReturnValue = Math.Round(CDbl(strResponseTime))
            Catch ex As Exception
                ReturnValue = 0
                ' WriteDeviceHistoryEntry("Domino", Server, Now.ToString & " Exception from C API " & ex.ToString)
            End Try

            If ReturnValue > 0 Then
                Return ReturnValue
            End If

            retryTime = 500

            Thread.Sleep(retryTime)
            WriteDeviceHistoryEntry("Domino", Server, Now.ToString & " No response from server, attempt #" & n.ToString & ". Waiting " & retryTime & " ms then trying again.")
        Next
        oWatch.Stop()
        n = n - 1
        WriteDeviceHistoryEntry("Domino", Server, Now.ToString & " No response from server, after " & n.ToString & " attempts in " & oWatch.Elapsed.Seconds & " seconds.")
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


    Function DominoResponseTimeByStatCall(ByVal server As String) As Double
        Dim oWatch As New System.Diagnostics.Stopwatch
        Dim ReturnValue As Double = 0
        WriteDeviceHistoryEntry("Domino", server, Now.ToString & " Attempting to get server response time via stat call.")
        oWatch.Start()
        Dim strResult As String = ""
        strResult = GetStats(server, "Disk", "")
        If InStr(strResult, "Disk.") Then
            oWatch.Stop()
            WriteDeviceHistoryEntry("Domino", server, Now.ToString & " Response time is " & oWatch.Elapsed.TotalMilliseconds & " ms")
            ReturnValue = oWatch.Elapsed.TotalMilliseconds
        Else
            ReturnValue = 0
        End If

        Return ReturnValue
    End Function


#End Region



    Dim objRandom As New System.Random(CType(System.DateTime.Now.Ticks Mod System.Int32.MaxValue, Integer))

    Public Function GetRandomNumber(Optional ByVal Low As Integer = 1, Optional ByVal High As Integer = 100) As Integer
        ' Returns a random number,
        ' between the optional Low and High parameters
        Return objRandom.Next(Low, High + 1)
    End Function


End Class