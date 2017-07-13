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
Imports MongoDB.Bson

Partial Public Class VitalSignsPlusDomino

    Private Sub MonitorDominoCluster()
        Try
            '2/4/2016 NS modified for VSPLUS-2560
            Do Until boolTimeToStop = True
                'each call to Monitor Domino Cluster will monitor 1 Domino server, 1 time
                WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> Begin Loop for Domino Cluster Monitoring >>>>")
                '   dtDominoClusterLastUpdate = Now
                Dim myCluster As MonitoredItems.DominoMailCluster
                Try
                    myCluster = CType(SelectServerToMonitor(myDominoClusters), MonitoredItems.DominoMailCluster)
                Catch ex As Exception
                    '    WriteDeviceHistoryEntry("All", "Domino Cluster", Now.ToString &  " Error initializing cluster monitoring " & ex.ToString)
                    myCluster = Nothing
                End Try

                If myCluster Is Nothing Then
                    WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> No Domino clusters are due for monitoring now.  >>>>")
                    '3/2/2016 NS modified 
                    'Exit Sub
                    GoTo ThreadSleep
                Else
                    WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> Selected Domino Cluster " & myCluster.Name)
                End If

                If myCluster.Enabled = False Then
                    '       WriteDeviceHistoryEntry("All", "Domino Cluster", Now.ToString &  " >>> Selected Domino Cluster " & myCluster.Name & " is disabled.")
                    '  myCluster.LastScan = Now
                    myCluster.Status = "Disabled"
                    myCluster.ResponseDetails = "This cluster is not enabled for monitoring."
                    UpdateDominoClusterStatusTable(myCluster)
                    myCluster.LastScan = Now
                    '3/2/2016 NS modified
                    'Exit Sub
                    GoTo ThreadSleep
                End If

                If InMaintenance("Domino Cluster database", myCluster.Name) = True Then
                    '    WriteDeviceHistoryEntry("All", "Domino Cluster", Now.ToString &  " >>> Selected Domino Cluster " & myCluster.Name & " is in maintenance.")
                    myCluster.Status = "Maintenance"
                    myCluster.LastScan = Now
                    myCluster.ResponseDetails = "This cluster is in a scheduled maintenance period.  Monitoring is temporarily disabled."
                    UpdateDominoClusterStatusTable(myCluster)
                Else
                    WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> Collecting Cluster Information for " & myCluster.Name & " >>>>")
                    myCluster.AlertCondition = False
                    Dim RetryCount As Integer = 0
                    myCluster.Status = "Scanning"
                    myCluster.LastScan = Now

                    Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                    Dim filterDef As MongoDB.Driver.FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.TypeAndName, myCluster.Name + "-Domino Cluster database")
                    Dim updateDef As MongoDB.Driver.UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.CurrentStatus, myCluster.Status)
                    repository.Update(filterDef, updateDef)

                    Do While CollectClusterInformation(myCluster) = False
                        RetryCount += 1
                        Thread.CurrentThread.Sleep(5000)
                        WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> Analyzing Cluster Information for " & myCluster.Name & " >>>>")
                        If RetryCount = 10 Then
                            WriteAuditEntry(vbCrLf & Now.ToString & " ***************  Tried 10 times to analyze " & myCluster.Name & " but it didn't work out. Will try again later. ")
                            Exit Do
                        End If
                    Loop

                    Thread.CurrentThread.Sleep(1000)
                    AnalyzeClusterInformation(myCluster)
                    WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> Reporting Cluster Status Information for " & myCluster.Name & " >>>>")
                    Thread.CurrentThread.Sleep(1000)
                    '12/30 WS Commented out since Domino Clusters are no longer in the Status Table, See VSPLUS-2071
                    '3/23/2016 NS uncommented the function due to cluster information not being updated in the Status table
                    'causing the UI to display stale update dates - VSPLUS-2629
                    myCluster.LastScan = Now
                    UpdateDominoClusterStatusTable(myCluster)
                    WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> Reporting Cluster Report for " & myCluster.Name & " >>>>")
                    Thread.CurrentThread.Sleep(1000)
                    BuildClusterReport(myCluster)
                End If
ThreadSleep:
                'Increase the sleep interval
                Thread.Sleep(60000)
            Loop
        Catch ex As Exception
            'Mukund 31Oct14:VSPLUS-1130,Unhandled exception in CreateClusterReport (as per error in IMG_30102014_111430.png shared)
            WriteAuditEntry(Now.ToString & " Error in MonitorDominoCluster : " & ex.Message)
        End Try
    End Sub

    '12/12/16 WS Moved to VSServices
    'Private Function SelectDominoClusterToMonitor() As MonitoredItems.DominoMailCluster
    '    WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> Selecting a Domino Cluster for monitoring >>>>")
    '    If myDominoClusters.Count = 0 Then
    '        WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " No clusters found. Exiting SelectDominoClusterToMonitor...")
    '        Return Nothing
    '        Exit Function
    '    Else
    '        WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " Found " & myDominoClusters.Count.ToString() & " clusters", LogUtilities.LogUtils.LogLevel.Verbose)
    '    End If

    '    Dim tNow As DateTime
    '    tNow = Now
    '    Dim tScheduled As DateTime

    '    Dim timeOne, timeTwo As DateTime

    '    Dim SelectedServer As MonitoredItems.DominoMailCluster

    '    Dim ServerOne As MonitoredItems.DominoMailCluster
    '    Dim ServerTwo As MonitoredItems.DominoMailCluster

    '    Dim n As Integer

    '    'this for/next loop is for debug, disable later
    '    '  For n = 0 To myDominoClusters.Count - 1
    '    'ServerOne = myDominoClusters.Item(n)
    '    '   WriteDeviceHistoryEntry("All", "Domino Cluster", Now.ToString &  " >>> " & ServerOne.Name & " scheduled for " & ServerOne.NextScan & " and status is " & ServerOne.Status)
    '    '  Next


    '    Dim strSQL As String = ""
    '    Dim ServerType As String = "DominoCluster"
    '    Dim serverName As String = ""

    '    Try
    '        strSQL = "Select svalue from ScanSettings where sname = 'Scan" & ServerType & "ASAP'"
    '        Dim ds As New DataSet()
    '        ds.Tables.Add("ScanASAP")
    '        Dim objVSAdaptor As New VSAdaptor
    '        objVSAdaptor.FillDatasetAny("VitalSigns", "VitalSigns", strSQL, ds, "ScanASAP")

    '        For Each row As DataRow In ds.Tables("ScanASAP").Rows
    '            WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> Found clusters to scan immediately. ", LogUtilities.LogUtils.LogLevel.Verbose)
    '            Try
    '                serverName = row(0).ToString()
    '            Catch ex As Exception
    '                Continue For
    '            End Try

    '            For n = 0 To myDominoClusters.Count - 1
    '                ServerOne = myDominoClusters.Item(n)

    '                If ServerOne.Name = serverName And ServerOne.IsBeingScanned = False And ServerOne.Enabled Then
    '                    WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> " & serverName & " was marked 'Scan ASAP' so that will be scanned next.")
    '                    strSQL = "DELETE FROM ScanSettings where sname = 'Scan" & ServerType & "ASAP' and svalue='" & serverName & "'"
    '                    objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)

    '                    Return ServerOne
    '                    Exit Function

    '                End If
    '            Next
    '        Next

    '    Catch ex As Exception
    '        WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> Error executing a SQL query while selecting a Domino server... " & ex.Message)
    '    End Try



    '    'Any server Not Scanned can be scanned right away.  Select the first one you encounter
    '    For n = 0 To myDominoClusters.Count - 1
    '        Try
    '            ServerOne = myDominoClusters.Item(n)
    '            If ServerOne.Status = "Not Scanned" Or ServerOne.Status = "Master Service Stopped." Then
    '                WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status)
    '                Return ServerOne
    '                Exit Function
    '            Else
    '                WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> " & ServerOne.Name & " - " & ServerOne.Status, LogUtilities.LogUtils.LogLevel.Verbose)
    '            End If
    '        Catch ex As Exception
    '            WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> Error Selecting Domino server... " & ex.Message)
    '        End Try

    '    Next

    '    'start with the first two servers
    '    ServerOne = myDominoClusters.Item(0)
    '    If myDominoClusters.Count > 1 Then ServerTwo = myDominoClusters.Item(1)

    '    'go through the remaining servers, see which one has the oldest (earliest) scheduled time
    '    If myDominoClusters.Count > 2 Then
    '        Try
    '            For n = 2 To myDominoClusters.Count - 1
    '                '             WriteDeviceHistoryEntry("All", "Domino Cluster", Now.ToString &  " N is " & n)
    '                timeOne = CDate(ServerOne.NextScan)
    '                timeTwo = CDate(ServerTwo.NextScan)
    '                If DateTime.Compare(timeOne, timeTwo) < 0 Then
    '                    'time one is earlier than time two, so keep server 1
    '                    ServerTwo = myDominoClusters.Item(n)
    '                Else
    '                    'time two is later than time one, so keep server 2
    '                    ServerOne = myDominoClusters.Item(n)
    '                End If
    '            Next
    '        Catch ex As Exception
    '            WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> Error Selecting Domino server... " & ex.Message)
    '        End Try
    '    Else
    '        WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> There are only two servers, proceeding ... ", LogUtilities.LogUtils.LogLevel.Verbose)
    '    End If

    '    'WriteAuditEntry(Now.ToString & " >>> Down to two servers... " & ServerOne.Name & " and " & ServerTwo.Name)

    '    'Of the two remaining servers, pick the one with earliest scheduled time for next scan
    '    If Not (ServerTwo Is Nothing) Then
    '        timeOne = CDate(ServerOne.NextScan)
    '        timeTwo = CDate(ServerTwo.NextScan)
    '        WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> Time 1: " & timeOne.ToShortTimeString() & ", Time 2: " & timeTwo.ToShortTimeString(), LogUtilities.LogUtils.LogLevel.Verbose)
    '        If DateTime.Compare(timeOne, timeTwo) < 0 Then
    '            'time one is earlier than time two, so keep server 1
    '            SelectedServer = ServerOne
    '            tScheduled = CDate(ServerOne.NextScan)
    '        Else
    '            SelectedServer = ServerTwo
    '            tScheduled = CDate(ServerTwo.NextScan)
    '        End If
    '        tNow = Now
    '        WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> Down to one server... " & SelectedServer.Name & " to scan at " & SelectedServer.NextScan & ". Status is " & SelectedServer.Status, LogUtilities.LogUtils.LogLevel.Verbose)
    '    Else
    '        SelectedServer = ServerOne
    '        tScheduled = CDate(ServerOne.NextScan)
    '    End If

    '    tScheduled = CDate(SelectedServer.NextScan)
    '    WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " >>> tScheduled: " & tScheduled.ToShortTimeString(), LogUtilities.LogUtils.LogLevel.Verbose)
    '    If DateTime.Compare(tNow, tScheduled) < 0 Then
    '        If SelectedServer.Status <> "Not Scanned" Then
    '            WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " No Domino clusters scheduled for monitoring, next scan after " & SelectedServer.NextScan)
    '            SelectedServer = Nothing
    '        Else
    '            WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " selected Domino cluster: " & SelectedServer.Name & " because it has not been scanned yet.")
    '        End If
    '    Else
    '        WriteDeviceHistoryEntry("All", "Domino_Cluster", Now.ToString & " selected Domino cluster: " & SelectedServer.Name)
    '    End If

    '    'Release Memory
    '    tNow = Nothing
    '    tScheduled = Nothing
    '    n = Nothing

    '    timeOne = Nothing
    '    timeTwo = Nothing

    '    ServerOne = Nothing
    '    ServerTwo = Nothing

    '    'return selected server
    '    SelectDominoClusterToMonitor = SelectedServer
    '    'Exit Function
    '    SelectedServer = Nothing
    'End Function

    Private Function LockOrReleaseDominoServerForScan(ByRef ServerName As String, ByRef ClusterName As String, ByVal Release As Boolean) As Boolean
        Dim myDominoServer As MonitoredItems.DominoServer
        Try
            MyDominoServer = MyDominoServers.Search(ServerName)
            WriteDeviceHistoryEntry("Domino_Cluster", ClusterName, Now.ToString & " Server   " & ServerName & " is requested for Release = " & Release)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino_Cluster", ClusterName, Now.ToString & " Exception Server   " & ServerName & " is requested for Release = " & Release)
        End Try

        Try
            If MyDominoServer Is Nothing Then
                Exit Function
            End If
        Catch ex As Exception

        End Try

        If Release Then
            MyDominoServer.IsBeingScanned = False
            WriteDeviceHistoryEntry("Domino_Cluster", ClusterName, Now.ToString & " Lock is released for server   " & ServerName)
            Exit Function
        End If

        While True
            If MyDominoServer.IsBeingScanned Then
                Thread.Sleep(1000)
                Continue While
            Else
                ' Locking the Domino Server to be scanned. 
                MyDominoServer.IsBeingScanned = True
                WriteDeviceHistoryEntry("Domino_Cluster", ClusterName, Now.ToString & " Lock is Acquired for server   " & ServerName)
                Return True
                Exit While
            End If
        End While

    End Function

    Private Function CollectClusterInformation(ByRef Cluster As MonitoredItems.DominoMailCluster) As Boolean
        WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Entered module to collect cluster information.")
        CollectClusterInformation = True

        Dim mySession As New Domino.NotesSession
        Dim dbDir As Domino.NotesDbDirectory
        Dim db As Domino.NotesDatabase
        Dim collection As Domino.NotesDocumentCollection

        Dim ServerALock As Boolean = False
        Dim ServerBLock As Boolean = False
        Dim ServerCLock As Boolean = False

        '6/22/2015 NS added for VSPLUS-1475
        Dim NotesSystemMessageString As String = "Incorrect Notes Password."
        Try
            mySession.Initialize(MyDominoPassword)
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString + " Initialized a session for " & mySession.CommonUserName)
        Catch ex As Exception
            '6/22/2015 NS added for VSPLUS-1475
            System.Runtime.InteropServices.Marshal.ReleaseComObject(mySession)
            myAlert.QueueSysMessage(NotesSystemMessageString)
            WriteAuditEntry(Now.ToString & " Error Initializing NotesSession.  Many problems will follow....  " & ex.ToString)
            WriteAuditEntry(Now.ToString & " *********** ERROR ************  Error initializing a session to Query Domino server. ")
            WriteAuditEntry(Now.ToString & " Calling stopnotescl.exe then exiting in an attempt to recover.")
            WriteAuditEntry(Now.ToString & " The VitalSigns Master service should restart the monitoring service in a few moments.")
            KillNotes()
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString + ": Error creating NotesSession in CollectClusterInformation module " & ex.Message)
            GoTo ReleaseCOMObjects
        End Try

        Dim myDatabaseCollection As MonitoredItems.DominoMailClusterDatabaseCollection = Cluster.DatabaseCollection
        Dim myClusterDatabase As MonitoredItems.DominoMailClusterDatabase
        Dim ServerCount As Integer  'the number of servers in this cluster-- if all are up, then the cluster is up

        If Cluster.Server_C_Name = "" Then
            ServerCount = 2
        Else
            ServerCount = 3
        End If

        Dim ServerUpCount As Integer = 0

        'loop through all the databases on Server A
        Try
            'Get a lock before you start the scan.
            ServerALock = LockOrReleaseDominoServerForScan(Cluster.Server_A_Name, Cluster.Name, False)
            dbDir = mySession.GetDbDirectory(Cluster.Server_A_Name)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Error: Domino Cluster Monitoring routine failed to initiate NotesdbDirectory on " & Cluster.Server_A_Name & ": " & ex.Message)
            Cluster.IncrementDownCount()
            ' Cluster.Status = "Server Down"
            Cluster.ResponseDetails = "Domino server cluster member " & Cluster.Server_A_Name & " is not responding."
            GoTo ReleaseCOMObjects
        End Try

        If dbDir Is Nothing Then
            Cluster.IncrementDownCount()
            ' Cluster.Status = "Server Down"
            Cluster.ResponseDetails = "Domino server cluster member " & Cluster.Server_A_Name & " is not responding."
            GoTo ReleaseCOMObjects
        End If

        Try
            db = dbDir.GetFirstDatabase(Domino.DB_TYPES.NOTES_DATABASE)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Error: Domino Cluster Monitoring CountMailboxes routine failed to access the first database: " & ex.Message)
            Cluster.IncrementDownCount()
            '  Cluster.Status = "Server Down"
            Cluster.ResponseDetails = "Domino server cluster member " & Cluster.Server_A_Name & " is not responding."
            Cluster.LastScan = Now.AddMinutes(-30)
            GoTo ReleaseCOMObjects
        End Try

        '  WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " looping through databases for server A - " & Cluster.Server_A_Name & ", matching all databases with : '" & Cluster.Server_A_Directory & "' ")
        ServerUpCount += 1 'add one to the server up count for this cluster
        Dim dbPathlist As New Generic.List(Of String)

        Try
            If db Is Nothing Then
                GoTo ReleaseComObjects
            End If

            While Not (db Is Nothing)
                dbPathlist.Add(db.FilePath)
                WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Adding " & db.FilePath)
                db = dbDir.GetNextDatabase()
            End While

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception getting all the file paths: " & ex.ToString)
            Cluster.LastScan = Now.AddMinutes(-30)
            GoTo ReleaseComObjects
        End Try

        Try
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " I have a list of database file paths with  " & dbPathlist.Count & " databases in it.")
        Catch ex As Exception

        End Try


        Try
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " ")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " **********************************************************")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " *** Analyzing  Cluster Health for server #1: " & Cluster.Server_A_Name & ", matching all databases with : '" & Cluster.Server_A_Directory & "' ")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " **********************************************************")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " ")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " ")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " ")

        Catch ex As Exception

        End Try


        Dim filePath As String = ""
        Dim myFilePath As String = ""

        Try

            For Each filePath In dbPathlist
                Try
                    WriteAuditEntry(Now.ToString & " Attempting to open  " & Cluster.Server_A_Name & " - " & filePath, LogLevel.Verbose)
                    db = mySession.GetDatabase(Cluster.Server_A_Name, filePath)
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception getting database " & ex.ToString)
                    GoTo SkipDatabase
                End Try

                '9/10/2015 NS added for VSPLUS-2126
                Try
                    If Not db Is Nothing Then
                        If Not (db.IsOpen) Then
                            db.Open()
                        End If
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception opening database for cluster analysis (1): " & Cluster.Server_A_Name & ", " & filePath & "; " & ex.Message)
                    GoTo SkipDatabase
                End Try

                Try
                    '9/10/2015 NS modified for VSPLUS-2126
                    If Not db Is Nothing Then
                        If db.IsOpen() Then
                            myFilePath = db.FilePath.ToUpper
                        End If
                    End If
                Catch ex As Exception
                    myFilePath = ""
                End Try

                '9/10/2015 NS commented out for VSPLUS-2126
                'Try
                '	If String.IsNullOrWhiteSpace(db.Title) Then
                '		db.Title = "Untitled Database"
                '	End If
                'Catch ex As Exception
                '                WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " Error checking if db name is blank.  Error: " & ex.Message)
                'End Try
                ' WriteAuditEntry(Now.ToString & " Examining cluster mail file " & db.Title & " | " & db.FilePath)

                If InStr(myFilePath, Cluster.Server_A_Directory.ToUpper, CompareMethod.Text) Then
                    '  WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " Examining cluster mail file " & db.Title & " | " & db.FilePath & " on " & Cluster.Server_A_Name)
                    ' WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " Selected cluster mail file " & db.Title & " | " & db.FilePath & " on " & Cluster.Server_A_Name)
                    Try
                        '9/10/2015 NS modified for VSPLUS-2126
                        myClusterDatabase = Nothing
                        If Not db Is Nothing Then
                            If db.IsOpen() Then
                                myClusterDatabase = myDatabaseCollection.Search(db.ReplicaID)
                            End If
                        Else
                            myClusterDatabase = Nothing
                        End If

                    Catch ex As Exception
                        myClusterDatabase = Nothing
                    End Try

                    If myClusterDatabase Is Nothing Then
                        Dim myNewClusterDatabase As New MonitoredItems.DominoMailClusterDatabase
                        '  WriteAuditEntry(Now.ToString & " This database is not yet in the cluster, adding.")
                        Try
                            '9/10/2015 NS modified for VSPLUS-2126
                            If Not db Is Nothing Then
                                If db.IsOpen() Then
                                    myNewClusterDatabase.ReplicaID = db.ReplicaID
                                    If String.IsNullOrWhiteSpace(db.Title) Then
                                        myNewClusterDatabase.Database_Title = "Untitled Database"
                                    Else
                                        myNewClusterDatabase.Database_Title = db.Title
                                    End If
                                    myNewClusterDatabase.Database_FileName = db.FilePath
                                    myNewClusterDatabase.Server_A_Size = db.Size
                                    WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Adding database to cluster: " & db.Title & " " & db.Size & " KB", LogUtilities.LogUtils.LogLevel.Verbose)
                                End If
                            Else
                                myNewClusterDatabase.ReplicaID = "NA"
                                myNewClusterDatabase.Database_Title = "Untitled Database"
                                myNewClusterDatabase.Database_FileName = "NA"
                                myNewClusterDatabase.Server_A_Size = 0
                            End If
                            ' WriteAuditEntry(Now.ToString & "----------------> This cluster database title is: " & db.Title)
                            myNewClusterDatabase.Server_B_Comment = "Not found on " & Cluster.Server_B_Name
                            If ServerCount = 3 Then
                                myNewClusterDatabase.Server_C_Comment = "Not found on " & Cluster.Server_C_Name
                            Else
                                myNewClusterDatabase.Server_C_Comment = ""
                            End If
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception opening database summary information for cluster analysis: " & ex.Message)
                        End Try
                        '    WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " " & db.Title & " replica ID is  " & db.ReplicaID)

                        Try
                            ' WriteAuditEntry(Now.ToString & " Getting document info...")
                            '9/10/2015 NS modified for VSPLUS-2126
                            If Not db Is Nothing Then
                                If Not (db.IsOpen) Then
                                    db.Open()
                                End If
                                If db.IsOpen() Then
                                    collection = db.AllDocuments
                                    myNewClusterDatabase.Server_A_Doc_Count = collection.Count
                                End If
                            Else
                                myNewClusterDatabase.Server_A_Doc_Count = -1
                                myNewClusterDatabase.Server_A_Comment = "Database is not available."
                            End If
                            'collection = Nothing
                        Catch ex As Exception
                            myNewClusterDatabase.Server_A_Doc_Count = -1
                            myNewClusterDatabase.Server_A_Comment = ex.Message
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception opening database for cluster analysis (2): " & Cluster.Server_A_Name & ", " & filePath & "; " & ex.Message)
                        End Try

                        Try
                            '9/10/2015 NS modified for VSPLUS-2126
                            If myNewClusterDatabase.ReplicaID <> "NA" Then
                                myDatabaseCollection.Add(myNewClusterDatabase)
                            End If
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception adding database to collection for analysis: " & ex.Message)
                        End Try

                    Else
                        ' WriteAuditEntry(Now.ToString & " This database is already in the cluster, updating.")
                        Try
                            '9/10/2015 NS modified for VSPLUS-2126
                            If Not db Is Nothing Then
                                If db.IsOpen() Then
                                    myClusterDatabase.ReplicaID = db.ReplicaID
                                    myClusterDatabase.Database_Title = db.Title
                                    myClusterDatabase.Database_FileName = db.FilePath
                                    myClusterDatabase.Server_A_Size = db.Size
                                    WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Updating database in cluster: " & db.Title & " " & db.Size & " KB", LogUtilities.LogUtils.LogLevel.Verbose)
                                End If
                            Else
                                myClusterDatabase.ReplicaID = "NA"
                                myClusterDatabase.Database_Title = "Untitled Database"
                                myClusterDatabase.Database_FileName = "NA"
                                myClusterDatabase.Server_A_Size = 0
                            End If
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & "  Exception opening database summary information for cluster analysis:  " & ex.Message)
                        End Try

                        Try
                            '9/10/2015 NS modified for VSPLUS-2126
                            If Not db Is Nothing Then
                                If Not (db.IsOpen) Then
                                    db.Open()
                                End If
                                If db.IsOpen() Then
                                    collection = db.AllDocuments
                                    ' WriteAuditEntry(Now.ToString & " Document Count = " & collection.Count)
                                    myClusterDatabase.Server_A_Doc_Count = collection.Count
                                End If
                            Else
                                myClusterDatabase.Server_A_Doc_Count = -1
                            End If
                            ' collection = Nothing
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception opening database for cluster analysis (3): " & Cluster.Server_A_Name & ", " & filePath & "; " & ex.Message)
                            myClusterDatabase.Server_A_Comment = ex.Message
                            ' collection = Nothing
                        End Try

                    End If
                End If

                dtDominoLastUpdate = Now
SkipDatabase:
            Next

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " General exception during cluster analysis at point A: " & ex.Message)
        End Try

        '******************************************
        'loop through all the databases on Server B
        '******************************************
        Try
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " ")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " ")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " ")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " **********************************************************")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " *** Analyzing  Cluster Health for server #2: " & Cluster.Server_B_Name & ", matching all databases with : '" & Cluster.Server_B_Directory & "' ")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " **********************************************************")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " ")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " ")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " ")

        Catch ex As Exception

        End Try

        '   WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " looping through databases for server B - " & Cluster.Server_B_Name & " directory: " & Cluster.Server_B_Directory)
        If Cluster.Server_B_Name = "" Then
            GoTo ReleaseCOMObjects
        End If

        Try
            ServerBLock = LockOrReleaseDominoServerForScan(Cluster.Server_B_Name, Cluster.Name, False)
            dbDir = mySession.GetDbDirectory(Cluster.Server_B_Name)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Error: Domino Cluster Monitoring routine failed to initiate NotesdbDirectory on " & Cluster.Server_B_Name & ": " & ex.Message)
            Cluster.IncrementDownCount()
            '  Cluster.Status = "Server Down"
            Cluster.ResponseDetails = "Domino server cluster member " & Cluster.Server_B_Name & " is not responding."

            GoTo ReleaseCOMObjects
        End Try

        If dbDir Is Nothing Then
            Cluster.IncrementDownCount()
            ' Cluster.Status = "Server Down"
            GoTo ReleaseCOMObjects
        End If

        Try
            db = dbDir.GetFirstDatabase(Domino.DB_TYPES.NOTES_DATABASE)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Error: Domino Cluster Monitoring  routine failed to access the first database: " & ex.Message)
            Cluster.IncrementDownCount()
            'Cluster.Status = "Server Down"
            Cluster.ResponseDetails = "Domino server cluster member " & Cluster.Server_B_Name & " is not responding."
            GoTo ReleaseCOMObjects
        End Try

        ServerUpCount += 1 'add one to the server up count for this cluster

        Try
            If db Is Nothing Then
                GoTo ReleaseComObjects
            End If
            'Remove all the prior items
            dbPathlist.Clear()
            While Not (db Is Nothing)
                dbPathlist.Add(db.FilePath)
                db = dbDir.GetNextDatabase()
            End While

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception getting all the file paths: " & ex.ToString)
            GoTo ReleaseComObjects
        End Try

        WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " I have a list of database file paths with  " & dbPathlist.Count & " databases in it.")

        Try
            For Each filePath In dbPathlist
                Try
                    WriteAuditEntry(Now.ToString & " Attempting to open  " & Cluster.Server_B_Name & " - " & filePath, LogLevel.Verbose)
                    db = mySession.GetDatabase(Cluster.Server_B_Name, filePath)
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception getting database " & ex.ToString)
                    GoTo SkipDatabase2
                End Try

                '9/10/2015 NS commented out for VSPLUS-2126
                'Try
                '	If String.IsNullOrWhiteSpace(db.Title) Then
                '		db.Title = "Untitled Database"
                '	End If
                'Catch ex As Exception
                '                WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " Error checking if db name is blank.  Error: " & ex.Message)
                'End Try
                '9/10/2015 NS added for VSPLUS-2126
                Try
                    If Not db Is Nothing Then
                        If Not (db.IsOpen) Then
                            db.Open()
                        End If
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception opening database for cluster analysis (1): " & Cluster.Server_B_Name & ", " & filePath & "; " & ex.Message)
                    GoTo SkipDatabase2
                End Try

                Try
                    '9/10/2015 NS modified for VSPLUS-2126
                    If Not db Is Nothing Then
                        If db.IsOpen() Then
                            myFilePath = db.FilePath.ToUpper
                        End If
                    End If
                Catch ex As Exception
                    myFilePath = ""
                End Try

                If InStr(myFilePath, Cluster.Server_B_Directory.ToUpper, CompareMethod.Text) Then
                    '     WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " Examining cluster mail file " & db.Title & " | " & db.FilePath)
                    '    WriteAuditEntry(Now.ToString & " Selected cluster mail file " & db.Title & " | " & db.FilePath)
                    Try
                        '9/10/2015 NS modified for VSPLUS-2126
                        myClusterDatabase = Nothing
                        If Not db Is Nothing Then
                            If db.IsOpen() Then
                                WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Selected cluster mail file " & db.Title & " | " & db.FilePath & " on " & Cluster.Server_B_Name, LogUtilities.LogUtils.LogLevel.Verbose)
                                WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Searching list of already found databases for replica ID: " & db.ReplicaID, LogUtilities.LogUtils.LogLevel.Verbose)
                                myClusterDatabase = myDatabaseCollection.Search(db.ReplicaID)
                            End If
                        Else
                            myClusterDatabase = Nothing
                        End If
                    Catch ex As Exception
                        myClusterDatabase = Nothing
                        WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception accessing information about the database: " & ex.Message)
                    End Try

                    If myClusterDatabase Is Nothing Then
                        WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " This database was NOT found on " & Cluster.Server_A_Name, LogUtilities.LogUtils.LogLevel.Verbose)
                        Dim myNewClusterDatabase As New MonitoredItems.DominoMailClusterDatabase
                        Try
                            If Not db Is Nothing Then
                                If db.IsOpen() Then
                                    WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Database is not nothing " & Cluster.Server_A_Name, LogUtilities.LogUtils.LogLevel.Verbose)
                                    myNewClusterDatabase.ReplicaID = db.ReplicaID
                                    If String.IsNullOrWhiteSpace(db.Title) Then
                                        myNewClusterDatabase.Database_Title = "Untitled Database"
                                    Else
                                        myNewClusterDatabase.Database_Title = db.Title
                                    End If
                                    myNewClusterDatabase.Database_FileName = db.FilePath
                                    myNewClusterDatabase.Server_B_Size = db.Size
                                End If
                            Else
                                myNewClusterDatabase.ReplicaID = "NA"
                                myNewClusterDatabase.Database_Title = "Untitled Database"
                                myNewClusterDatabase.Database_FileName = "NA"
                                myNewClusterDatabase.Server_B_Size = 0
                            End If
                            myNewClusterDatabase.Server_A_Comment = "Not found on " & Cluster.Server_A_Name
                            If ServerCount = 3 Then
                                myNewClusterDatabase.Server_C_Comment = "Not found on " & Cluster.Server_C_Name
                            Else
                                myNewClusterDatabase.Server_C_Comment = ""
                            End If
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception opening database summary information for cluster analysis: " & ex.Message)
                        End Try

                        Try
                            '9/10/2015 NS modified for VSPLUS-2126
                            If Not db Is Nothing Then
                                If Not (db.IsOpen) Then
                                    db.Open()
                                End If
                                If db.IsOpen() Then
                                    collection = db.AllDocuments
                                    ' WriteAuditEntry(Now.ToString & " Document Count = " & collection.Count)
                                    myNewClusterDatabase.Server_B_Doc_Count = collection.Count
                                End If
                            Else
                                myNewClusterDatabase.Server_B_Doc_Count = -1
                            End If
                            '     collection = Nothing
                        Catch ex As Exception
                            myNewClusterDatabase.Server_B_Doc_Count = -1
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception opening database for cluster analysis (2): " & Cluster.Server_B_Name & ", " & filePath & "; " & ex.Message)
                            '   collection = Nothing
                        End Try

                        Try
                            '9/10/2015 NS modified for VSPLUS-2126
                            If myNewClusterDatabase.ReplicaID <> "NA" Then
                                myDatabaseCollection.Add(myNewClusterDatabase)
                            End If
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception adding database to collection for analysis: " & ex.Message)
                        End Try

                    Else
                        Try
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " This database HAS already been found on " & Cluster.Server_A_Name, LogUtilities.LogUtils.LogLevel.Verbose)
                            ' myClusterDatabase.ReplicaID = db.ReplicaID
                            ' myClusterDatabase.Database_Title = db.Title
                            '   myClusterDatabase.Database_FileName = db.FilePath
                            myClusterDatabase.Server_B_Comment = ""
                            If Not db Is Nothing Then
                                If db.IsOpen() Then
                                    myClusterDatabase.Server_B_Size = db.Size
                                    WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Updating database in cluster: " & db.Title & " " & db.Size & " KB", LogUtilities.LogUtils.LogLevel.Verbose)
                                End If
                            Else
                                myClusterDatabase.Server_B_Size = 0
                            End If
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception: " & ex.Message)
                        End Try
                        Try
                            '9/10/2015 NS modified for VSPLUS-2126
                            If Not db Is Nothing Then
                                If Not (db.IsOpen) Then
                                    db.Open()
                                End If
                                If db.IsOpen() Then
                                    collection = db.AllDocuments
                                    ' WriteAuditEntry(Now.ToString & " Document Count = " & collection.Count)
                                    myClusterDatabase.Server_B_Doc_Count = collection.Count
                                End If
                            Else
                                myClusterDatabase.Server_B_Doc_Count = -1
                            End If
                            ' collection = Nothing
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception: " & ex.Message)
                            myClusterDatabase.Server_B_Comment = ex.Message
                        End Try

                    End If
                End If
                '9/10/2015 NS commented out for VSPLUS-2126
                'Try
                '	db = dbDir.GetNextDatabase
                'Catch ex As Exception
                '	WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " Error advancing to next database: " & ex.Message)
                '	If InStr(ex.Message, "Remote system no longer responding") > 1 Then
                '		WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " Server didn't respond; waiting 2 seconds and trying again.")
                '		Thread.Sleep(2000)

                '		WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " Second error when advancing to next database on Cluster B: " & ex.Message)
                '		CollectClusterInformation = False
                '		Exit For
                '	End If
                'End Try

SkipDatabase2:
            Next
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " General exception during cluster analysis at point B: " & ex.Message)
        End Try

        If ServerCount = ServerUpCount Then
            Cluster.IncrementUpCount()
            Cluster.Status = "OK"
        End If

        '**********************************************************
        'loop through all the databases on Server C, if defined
        '***********************************************************

        If Cluster.Server_C_Name = "" Then
            GoTo ReleaseCOMObjects
        End If

        Try
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " ")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " **********************************************************")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " *** Analyzing  Cluster Health for server #3: " & Cluster.Server_C_Name & ", matching all databases with : '" & Cluster.Server_C_Directory & "' ")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " **********************************************************")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " ")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " ")
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " ")

        Catch ex As Exception

        End Try


        Try
            ServerCLock = LockOrReleaseDominoServerForScan(Cluster.Server_C_Name, Cluster.Name, False)
            dbDir = mySession.GetDbDirectory(Cluster.Server_C_Name)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Error: Domino Cluster Monitoring routine failed to initiate NotesdbDirectory on " & Cluster.Server_C_Name & ": " & ex.Message)
            Cluster.IncrementDownCount()
            ' Cluster.Status = "Server Down"
            Cluster.ResponseDetails = "Domino server cluster member " & Cluster.Server_C_Name & " is not responding."
            GoTo ReleaseCOMObjects
        End Try

        Try
            If dbDir Is Nothing Then
                Cluster.IncrementDownCount()
                '  Cluster.Status = "Server Down"
                GoTo ReleaseCOMObjects
            End If
        Catch ex As Exception

        End Try


        Try
            db = dbDir.GetFirstDatabase(Domino.DB_TYPES.NOTES_DATABASE)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Error: Domino Cluster Monitoring CountMailboxes routine failed to access the first database: " & ex.Message)
            '   Cluster.Status = "Server Down"
            Cluster.ResponseDetails = "Domino server cluster member " & Cluster.Server_C_Name & " is not responding."
            GoTo ReleaseCOMObjects
        End Try

        ServerUpCount += 1 'add one to the server up count for this cluster
        Try
            If db Is Nothing Then
                GoTo ReleaseComObjects
            End If
            'Start with fresh data only for this server
            dbPathlist.Clear()
            While Not (db Is Nothing)
                dbPathlist.Add(db.FilePath)
                db = dbDir.GetNextDatabase()
            End While

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception getting all the file paths: " & ex.ToString)
            GoTo ReleaseComObjects
        End Try

        WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " I have a list of database file paths with  " & dbPathlist.Count & " databases in it.")

        Try
            For Each filePath In dbPathlist
                Try
                    WriteAuditEntry(Now.ToString & " Attempting to open  " & Cluster.Server_C_Name & " - " & filePath, LogLevel.Verbose)
                    db = mySession.GetDatabase(Cluster.Server_C_Name, filePath)
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception getting database " & ex.ToString)
                    GoTo SkipDatabase3
                End Try

                '9/10/2015 NS commented out for VSPLUS-2126
                'Try
                '	If String.IsNullOrWhiteSpace(db.Title) Then
                '		db.Title = "Untitled Database"
                '	End If
                'Catch ex As Exception
                '                WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " Error checking if db name is blank.  Error: " & ex.Message)
                'End Try

                '9/10/2015 NS added for VSPLUS-2126
                Try
                    If Not db Is Nothing Then
                        If Not (db.IsOpen) Then
                            db.Open()
                        End If
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception opening database for cluster analysis (1): " & Cluster.Server_C_Name & ", " & filePath & "; " & ex.Message)
                    GoTo SkipDatabase3
                End Try

                Try
                    '9/10/2015 NS modified for VSPLUS-2126
                    If Not db Is Nothing Then
                        If db.IsOpen() Then
                            myFilePath = db.FilePath.ToUpper
                        End If
                    End If
                Catch ex As Exception
                    myFilePath = ""
                End Try
                '    WriteAuditEntry(Now.ToString & " Examining cluster mail file " & db.Title & " | " & db.FilePath)

                If InStr(myFilePath, Cluster.Server_C_Directory.ToUpper, CompareMethod.Text) Then
                    ' WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " Examining cluster mail file " & db.Title & " | " & db.FilePath)
                    '   myClusterDatabase = myDatabaseCollection.Search(db.ReplicaID)
                    '9/10/2015 NS modified for VSPLUS-2126
                    myClusterDatabase = Nothing
                    If Not db Is Nothing Then
                        If db.IsOpen() Then
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Selected cluster mail file " & db.Title & " | " & db.FilePath & " on " & Cluster.Server_C_Name, LogUtilities.LogUtils.LogLevel.Verbose)
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Searching the database list for any database with replica ID of " & db.ReplicaID, LogUtilities.LogUtils.LogLevel.Verbose)
                            myClusterDatabase = myDatabaseCollection.Search(db.ReplicaID)
                        End If
                    Else
                        myClusterDatabase = Nothing
                    End If
                    If myClusterDatabase Is Nothing Then
                        WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " This database has NOT already been found in the cluster", LogUtilities.LogUtils.LogLevel.Verbose)
                        Dim myNewClusterDatabase As New MonitoredItems.DominoMailClusterDatabase
                        Try
                            If Not db Is Nothing Then
                                If db.IsOpen() Then
                                    myNewClusterDatabase.ReplicaID = db.ReplicaID
                                    If String.IsNullOrWhiteSpace(db.Title) Then
                                        myNewClusterDatabase.Database_Title = "Untitled Database"
                                    Else
                                        myNewClusterDatabase.Database_Title = db.Title
                                    End If
                                    myNewClusterDatabase.Database_FileName = db.FilePath
                                    myNewClusterDatabase.Server_C_Size = db.Size
                                    WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Adding database to cluster: " & db.Title & " " & db.Size & " KB", LogUtilities.LogUtils.LogLevel.Verbose)
                                End If
                            Else
                                myNewClusterDatabase.ReplicaID = "NA"
                                myNewClusterDatabase.Database_Title = "Untitled Database"
                                myNewClusterDatabase.Database_FileName = "NA"
                                myNewClusterDatabase.Server_C_Size = 0
                            End If
                            myNewClusterDatabase.Server_B_Comment = "Not found on " & Cluster.Server_B_Name
                            myNewClusterDatabase.Server_A_Comment = "Not found on " & Cluster.Server_A_Name
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception opening database summary information for cluster analysis:  " & ex.Message)
                        End Try
                        Try
                            '9/10/2015 NS modified for VSPLUS-2126
                            If Not db Is Nothing Then
                                If Not (db.IsOpen) Then
                                    db.Open()
                                End If
                                If db.IsOpen() Then
                                    collection = db.AllDocuments
                                    ' WriteAuditEntry(Now.ToString & " Document Count = " & collection.Count)
                                    myNewClusterDatabase.Server_C_Doc_Count = collection.Count
                                End If
                                collection = Nothing
                            Else
                                myNewClusterDatabase.Server_C_Doc_Count = -1
                            End If
                        Catch ex As Exception
                            myNewClusterDatabase.Server_C_Doc_Count = -1
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception opening database information for cluster analysis: : " & ex.Message)
                        End Try

                        Try
                            myDatabaseCollection.Add(myNewClusterDatabase)
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Error adding a database to the collection: " & ex.Message)
                        End Try

                    Else
                        Try
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " This database has already been found in the cluster.", LogUtilities.LogUtils.LogLevel.Verbose)
                            ' myClusterDatabase.ReplicaID = db.ReplicaID
                            ' myClusterDatabase.Database_Title = db.Title
                            'myClusterDatabase.Database_FileName = db.FilePath
                            myClusterDatabase.Server_C_Comment = ""
                            '9/10/2015 NS modified for VSPLUS-2126
                            If Not db Is Nothing Then
                                If db.IsOpen() Then
                                    myClusterDatabase.Server_C_Size = db.Size
                                    WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Updating database in cluster: " & db.Title & " " & db.Size & " KB", LogUtilities.LogUtils.LogLevel.Verbose)
                                End If
                            End If
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception opening database summary information for cluster analysis: " & ex.Message)
                        End Try
                        Try
                            '9/10/2015 NS modified for VSPLUS-2126
                            If Not db Is Nothing Then
                                If Not (db.IsOpen) Then
                                    db.Open()
                                End If
                                If db.IsOpen() Then
                                    collection = db.AllDocuments
                                    ' WriteAuditEntry(Now.ToString & " Document Count = " & collection.Count)
                                    myClusterDatabase.Server_C_Doc_Count = collection.Count
                                End If
                                collection = Nothing
                            Else
                                myClusterDatabase.Server_C_Doc_Count = -1
                            End If
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Exception opening database count information for cluster analysis: : " & ex.Message)
                            myClusterDatabase.Server_C_Doc_Count = -1
                        End Try

                    End If
                End If

                '9/10/2015 NS commented out for VSPLUS-2126
                'Try
                '	db = dbDir.GetNextDatabase
                'Catch ex As Exception
                '	WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " Error advancing to next database: " & ex.Message)
                '	If InStr(ex.Message, "Remote system no longer responding") > 1 Then
                '		WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " Server didn't respond; waiting 2 seconds and trying again.")
                '		Thread.Sleep(2000)
                '		Try
                '			WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " Testing state of dbDir: " & dbDir.ToString)
                '		Catch ex3 As Exception
                '			WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " The exception testing dbdir is : " & ex3.ToString)
                '		End Try

                '		WriteDeviceHistoryEntry("Domino Cluster", Cluster.Name, Now.ToString & " Second error when advancing to next database on Cluster C: " & ex.Message)
                '		CollectClusterInformation = False
                '		Exit For

                '	End If
                'End Try
SkipDatabase3:
            Next

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " General exception during cluster analysis at point C: " & ex.Message)
        End Try

        If ServerCount = ServerUpCount Then
            Cluster.IncrementUpCount()
            Cluster.Status = "OK"
        End If

        If CollectClusterInformation = False Then
            Cluster.Status = "Incomplete Analysis"
        End If

ReleaseCOMObjects:
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
        Catch ex As Exception

        End Try

        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(dbDir)
        Catch ex As Exception

        End Try

        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(collection)
        Catch ex As Exception

        End Try

        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(mySession)
        Catch ex As Exception

        End Try

        Try
            If ServerALock Then
                LockOrReleaseDominoServerForScan(Cluster.Server_A_Name, Cluster.Name, True)
            End If
        Catch ex As Exception
        End Try
        Try
            If ServerBLock Then
                LockOrReleaseDominoServerForScan(Cluster.Server_B_Name, Cluster.Name, True)
            End If
        Catch ex As Exception

        End Try
        Try
            If ServerCLock Then
                LockOrReleaseDominoServerForScan(Cluster.Server_C_Name, Cluster.Name, True)
            End If
        Catch ex As Exception

        End Try

    End Function

    Private Sub AnalyzeClusterInformation(ByRef Cluster As MonitoredItems.DominoMailCluster)
        If Cluster.Status = "Server Down" Then Exit Sub
        '5/6/2016 NS added
        Dim AlertStringMR As String = ""
        Dim AlertStringCR As String = ""
        '4/20/2016 NS - the code to exclude a database from scanning 
        Dim myDatabaseCollection As MonitoredItems.DominoMailClusterDatabaseCollection = Cluster.DatabaseCollection
        WriteAuditEntry(Now.ToString & " ------- Cluster Analysis Begin for " & Cluster.Name & " ----------")

        'start off assuming all the databases are ok
        Cluster.TotalDatabasesInError = 0

        'Find databases that only reside on one server

        Dim ServerCount As Integer  'the number of servers in this cluster-- if all are up, then the cluster is up
        If Cluster.Server_C_Name = "" Then
            ServerCount = 2
        Else
            ServerCount = 3
        End If
        ' WriteAuditEntry(Now.ToString & " ------- Cluster Analysis server count ----------" & ServerCount)

        '4/20/2016 NS added for VSPLUS-2724
        Dim proceed As Boolean
        For Each db As MonitoredItems.DominoMailClusterDatabase In myDatabaseCollection
            proceed = ProceedWithDBProcessing(db, Cluster)

            Dim ReplicaCount As Integer = 0
            If db.Server_A_Size <> 0 Then
                ReplicaCount += 1
            End If

            If db.Server_B_Size <> 0 Then
                ReplicaCount += 1
            End If

            If db.Server_C_Size <> 0 Then
                ReplicaCount += 1
            End If

            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Replica count for " & db.Database_FileName & " is " & ReplicaCount.ToString())
            If ReplicaCount = 1 Then
                'If a database does not qualify for any of the exclusion lists, send an alert
                If proceed Then
                    'Database in the cluster has no replicas, this is a problem
                    '5/5/2016 NS modified - single quotes are not handled by SQL if used as a part of the value, only as literal string separators
                    AlertStringMR = "The database titled " & db.Database_Title & " exists in the " & Cluster.Name & " cluster on "
                    If db.Server_A_Size > 0 Then
                        AlertStringMR += Cluster.Server_A_Name
                    End If
                    If db.Server_B_Size > 0 Then
                        AlertStringMR += Cluster.Server_B_Name
                    End If
                    If db.Server_C_Size > 0 Then
                        AlertStringMR += Cluster.Server_C_Name
                    End If

                    AlertStringMR += " as " & db.Database_FileName & " but it does not appear on the other servers "
                    AlertStringMR += "defined in the cluster.  If the primary server fails, this user will not fail over properly."
                    Cluster.TotalDatabasesInError += 1
                    '5/6/2016 NS modified - the reference should be to the Cluster name, not the Database title
                    'myAlert.QueueAlert("Domino Cluster database", db.Database_Title, "Missing Replica", AlertString, Cluster.Location)
                    myAlert.QueueAlert(Cluster.ServerType, Cluster.Name, "Missing Replica", AlertStringMR, Cluster.Location)
                    '5/5/2016 NS added for VSPLUS-2921
                    Cluster.Status = "Issue"
                End If
            End If

            If ReplicaCount < ServerCount Then
                'come back and figure this out
            End If

            'Find databases where doc count is off by threshold#1 or more
            Dim myPercent As Double = 0
            Dim AlertCondition As Boolean = False
            Dim SmallDocCount As Double = 0
            Dim BigDocCount As Double = 0

            'VSPLUS-859, Mukund 18Sep14 
            If Cluster.Server_C_Name = "" Then
                'without Server C
                'Find least count
                SmallDocCount = IIf(db.Server_A_Doc_Count < db.Server_B_Doc_Count, db.Server_A_Doc_Count, db.Server_B_Doc_Count)
                'Find highest count
                BigDocCount = IIf(db.Server_A_Doc_Count > db.Server_B_Doc_Count, db.Server_A_Doc_Count, db.Server_B_Doc_Count)
            Else
                'with Server C
                'Find least count
                SmallDocCount = IIf(db.Server_A_Doc_Count < db.Server_B_Doc_Count, IIf(db.Server_A_Doc_Count < db.Server_C_Doc_Count, db.Server_A_Doc_Count, db.Server_C_Doc_Count), IIf(db.Server_B_Doc_Count < db.Server_C_Doc_Count, db.Server_B_Doc_Count, db.Server_C_Doc_Count))
                'Find highest count
                BigDocCount = IIf(db.Server_A_Doc_Count > db.Server_B_Doc_Count, IIf(db.Server_A_Doc_Count > db.Server_C_Doc_Count, db.Server_A_Doc_Count, db.Server_C_Doc_Count), IIf(db.Server_B_Doc_Count > db.Server_C_Doc_Count, db.Server_B_Doc_Count, db.Server_C_Doc_Count))
            End If

            'to avoid division by 0. If 0 then myPercent too is 0
            If BigDocCount > 0 Then
                myPercent = 100 - (SmallDocCount * 100 / BigDocCount)
            End If

            'Check if myPercent is greater or equal to threshold. If yes, it is to Alert.
            If (myPercent >= Cluster.FirstAlertThreshold) Then
                AlertCondition = True
            End If

            If AlertCondition = True And SmallDocCount <> -1 And Trim(db.Database_Title) <> "" Then
                'If a database does not qualify for any of the exclusion lists, send an alert
                If proceed Then
                    Cluster.TotalDatabasesInError += 1
                    '5/5/2016 NS modified - single quotes are not handled by SQL if used as a part of the value, only as literal string separators
                    AlertStringCR = "The database titled " & db.Database_Title & " " & " in the " & Cluster.Name & " cluster on "
                    If db.Server_A_Size > 0 Then
                        AlertStringCR += Cluster.Server_A_Name
                    ElseIf db.Server_B_Size > 0 Then
                        AlertStringCR += Cluster.Server_B_Name
                    ElseIf db.Server_C_Size > 0 Then
                        AlertStringCR += Cluster.Server_C_Name
                    End If

                    AlertStringCR += " as " & db.Database_FileName & " appears to have an issue with cluster replication. " & vbCrLf & vbCrLf
                    AlertStringCR += Cluster.Server_A_Name & " document count: " & db.Server_A_Doc_Count & vbCrLf
                    AlertStringCR += Cluster.Server_B_Name & " document count: " & db.Server_B_Doc_Count
                    If Cluster.Server_C_Name <> "" Then
                        AlertStringCR += vbCrLf & Cluster.Server_C_Name & " document count: " & db.Server_C_Doc_Count
                    End If
                    '5/6/2016 NS modified - the reference should be to the Cluster name, not the Database title
                    'myAlert.QueueAlert("Domino Cluster database", db.Database_Title, "Cluster Replication", AlertStringCR, Cluster.Location)
                    myAlert.QueueAlert(Cluster.ServerType, Cluster.Name, "Cluster Replication", AlertStringCR, Cluster.Location)
                    '5/5/2016 NS added for VSPLUS-2921
                    Cluster.Status = "Issue"
                End If
            End If
        Next

        Cluster.ResponseDetails = Cluster.TotalDatabases & " databases in cluster analyzed. " & Cluster.TotalDatabasesInError & " databases with potential replication issues."
        Cluster.ResponseDetails = Cluster.TotalDatabases & " databases in cluster analyzed. Go to Cluster Health, Cluster Database Reports for details. "

    End Sub

    Private Sub BuildClusterReport(ByRef Cluster As MonitoredItems.DominoMailCluster)
        Try

            'This function needs to be re-written to output to a new database table, rather than an HTML file

            If Cluster.Status = "Server Down" Then Exit Sub
            Dim myDatabaseCollection As MonitoredItems.DominoMailClusterDatabaseCollection = Cluster.DatabaseCollection
            ' Remove Cluster details for the Cluster ID. 
            If myDatabaseCollection.Count > 0 Then
                cleanUpClusterDetailedTable(Cluster.ServerObjectID)
            End If
            '4/20/2016 NS added for VSPLUS-2724
            Dim proceed As Boolean
            For Each db As MonitoredItems.DominoMailClusterDatabase In myDatabaseCollection
                proceed = ProceedWithDBProcessing(db, Cluster)
                Dim myComment As String = ""
                If db.Server_A_Comment <> "" Or db.Server_B_Comment <> "" Or db.Server_C_Comment <> "" Then
                    If db.Server_A_Comment <> "" Then
                        myComment = Trim(db.Server_A_Comment)
                    End If
                    If myComment = "" Then
                        If db.Server_B_Comment <> "" Then
                            myComment = Trim(db.Server_B_Comment)
                        End If
                    Else
                        If db.Server_B_Comment <> "" Then
                            myComment += " " & db.Server_B_Comment
                        End If
                    End If

                    If myComment = "" Then
                        If db.Server_C_Comment <> "" Then
                            myComment = db.Server_C_Comment
                        End If
                    Else
                        If db.Server_C_Comment <> "" Then
                            myComment += " " & db.Server_C_Comment
                        End If
                    End If
                End If

                Dim dbtitle As String = db.Database_Title.Replace("'", "''")
                WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Title = ------- " & dbtitle & " ----------")
                If proceed Then
                    '9/30/2015 NS modified for VSPLUS-2150
                    UpdateClusterDataTable(Cluster.Name, dbtitle, db.Database_FileName, db.Server_A_Doc_Count, db.Server_B_Doc_Count,
                                           db.Server_C_Doc_Count, db.Server_A_Size / 1024 / 1024, db.Server_B_Size / 1024 / 1024,
                                           db.Server_C_Size / 1024 / 1024, myComment, DateTime.Now(), db.ReplicaID, Cluster.ServerObjectID)
                Else
                    WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " The database " & db.Database_Title & " (" & db.Database_FileName & ") qualifies for one of the exclusion criteria. It will not be reported.")
                End If

            Next
        Catch ex As Exception
            'Mukund 31Oct14:VSPLUS-1130,Unhandled exception in CreateClusterReport (as per error in IMG_30102014_111430.png shared)
            WriteAuditEntry(Now.ToString & " Error in BuildClusterReport : " & ex.Message)
        End Try
    End Sub
    '4/21/2016 NS added for VSPLUS-2724
    Private Function ProceedWithDBProcessing(ByRef db As MonitoredItems.DominoMailClusterDatabase, ByRef Cluster As MonitoredItems.DominoMailCluster) As Boolean
        Dim dbFolder As String = ""
        Dim folders As String()
        Dim proceed As Boolean = True

        If db.Database_FileName.LastIndexOf("\") > 0 Then
            dbFolder = Left(db.Database_FileName, db.Database_FileName.LastIndexOf("\"))
        End If
        If dbFolder <> "" Then
            WriteDeviceHistoryEntry("Domino_Cluster", Cluster.Name, Now.ToString & " Title = " & db.Database_Title & ", Folder = " & dbFolder, LogUtilities.LogUtils.LogLevel.Verbose)
            'Compare the database folder structure to each value in the exclusion list. 
            'If there is a match, flag the database as do not proceed.
            If Cluster.Server_A_ExcludeList <> "" Then
                folders = Cluster.Server_A_ExcludeList.Split(New Char() {","c})
                For Each folder In folders
                    If InStr(LCase(dbFolder), LCase(folder)) Then
                        proceed = False
                        Exit For
                    End If
                Next
            End If
            If Cluster.Server_B_ExcludeList <> "" Then
                folders = Cluster.Server_B_ExcludeList.Split(New Char() {","c})
                For Each folder In folders
                    If InStr(LCase(dbFolder), LCase(folder)) Then
                        proceed = False
                        Exit For
                    End If
                Next
            End If
            If Cluster.Server_C_ExcludeList <> "" Then
                folders = Cluster.Server_C_ExcludeList.Split(New Char() {","c})
                For Each folder In folders
                    If InStr(LCase(dbFolder), LCase(folder)) Then
                        proceed = False
                        Exit For
                    End If
                Next
            End If
        End If
        Return proceed
    End Function

    Private Sub UpdateDominoClusterStatusTable(ByRef MyDominoCluster As MonitoredItems.DominoMailCluster)

        '*************************************************************
        'Update the Status Collection with Cluster Information
        '*************************************************************
        Dim strSQL As String
        Dim StatusDetails As String

        Try
            If MyLogLevel = LogLevel.Verbose Then
                WriteDeviceHistoryEntry("Domino_Cluster", MyDominoCluster.Name, Now.ToString & " %% Entered UpdateDominoStatusTable for " & MyDominoCluster.Name)
                WriteDeviceHistoryEntry("Domino_Cluster", MyDominoCluster.Name, Now.ToString & " Status Details for " & MyDominoCluster.Name & " are " & MyDominoCluster.ResponseDetails)
            End If
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error logging StatusDetails String: " & ex.Message)
        End Try

        Try
            '   WriteAuditEntry(Now.ToString & " Response Details: " & MyDominoServer.ResponseDetails)
            If MyDominoCluster.ResponseDetails <> "" Or MyDominoCluster.ResponseDetails <> " " Then
                StatusDetails = MyDominoCluster.ResponseDetails
            Else
                StatusDetails = "No databases in this cluster are affected by problems with Cluster Replication"
                '  StatusDetails = "Pending: " & MyDominoServer.PendingMail & vbCrLf & " Dead: " & MyDominoServer.DeadMail & vbCrLf & " Users: " & MyDominoCluster.UserCount
            End If
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating StatusDetails String: " & ex.Message)
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
            StatusDetails = "No databases in this cluster are affected by problems with Cluster Replication"
        End Try

        If MyDominoCluster.Status = "Disabled" Then
            StatusDetails = "This cluster is not enabled for monitoring."
        End If

        If MyDominoCluster.Status = "Maintenance" Then
            StatusDetails = "This server cluster is in a scheduled maintenance period.  Monitoring is temporarily disabled."
        End If

        If MyDominoCluster.Status = "Not Scanned" Then
            StatusDetails = "This cluster has not been scanned yet."
        End If

        Try
            'Update the status table

            Dim Percent As Double
            Try
                Percent = MyDominoCluster.TotalDatabasesInError / MyDominoCluster.TotalDatabases
            Catch ex As Exception
                Percent = 0
            End Try
            WriteAuditEntry(Now.ToString & " Domino Cluster " & MyDominoCluster.Name & " percent of databases in error  =" & (Percent * 100).ToString & "%", LogLevel.Verbose)
            Dim myLocation As String = ""

            With MyDominoCluster
                '3/28/2016 NS modified for VSPLUS-2629
                'Changed Now to LastScan for the LastUpdate value
                '5/3/2016 NS modified for VSPLUS-2921


                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As MongoDB.Driver.FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.TypeAndName, .Name & "-" & .ServerType)
                Dim updateDef As MongoDB.Driver.UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                                    .Set(Function(x) x.DeviceName, .Name) _
                                                                                                    .Set(Function(x) x.CurrentStatus, .Status) _
                                                                                                    .Set(Function(x) x.DeviceType, .ServerType) _
                                                                                                    .Set(Function(x) x.LastUpdated, GetFixedDateTime(Now)) _
                                                                                                    .Set(Function(x) x.NextScan, GetFixedDateTime(.NextScan)) _
                                                                                                    .Set(Function(x) x.Details, .ResponseDetails) _
                                                                                                    .Set(Function(x) x.Category, .Category) _
                                                                                                    .Set(Function(x) x.DownCount, .DownCount) _
                                                                                                    .Set(Function(x) x.UpCount, .UpCount) _
                                                                                                    .Set(Function(x) x.UpPercent, .UpPercentCount) _
                                                                                                    .Set(Function(x) x.Description, myLocation) _
                                                                                                    .Set(Function(x) x.UserCount, Convert.ToInt32(.TotalDatabasesInError))
                '.Set(Function(x) x.DeadMail, Convert.ToInt32(.TotalDatabasesInError))
                repository.Upsert(filterDef, updateDef)


            End With
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in Domino cluster module creating SQL statement for status table: " & ex.Message)
            WriteDeviceHistoryEntry("Domino_Cluster", MyDominoCluster.Name, Now.ToString & " Error in Domino Cluster module creating SQL statement for status table: " & ex.Message)
        Finally
            WriteAuditEntry("Next scan scheduled : " & MyDominoCluster.NextScan, LogLevel.Verbose)
        End Try

        Try
            GC.Collect()
        Catch ex As Exception

        End Try

    End Sub


End Class
