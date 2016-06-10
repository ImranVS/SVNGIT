Imports VSFramework

Partial Public Class VitalSignsPlusDomino

#Region "VitalStatus  -  Notes Database Output"


    Public Sub VitalStatus_Thread()
        Dim myRegistry As New RegistryHandler

        Do
            If boolTimeToStop = True Then Exit Do
            Try
                If CType(myRegistry.ReadFromRegistry("Notes Output"), String) = "True" Then
                    VitalStatus_UpdateStatusInformation()
                    WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " >>> Finished VS output.")
                End If

            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " Exception with VS Output: " & ex.ToString)
                If InStr(ex.ToString, "System.OutOfMemoryException") > 0 Then
                    WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " VitalSigns is out of memory.  Attempting to exit so the Master service will restart it. ")
                    Threading.Thread.Sleep(1000)
                    End
                End If

            End Try
            Threading.Thread.Sleep(30000)
        Loop


    End Sub


    Public Sub VitalStatus_UpdateStatusInformation()
        Dim myRegistry As New RegistryHandler

        Try
            If myRegistry.ReadFromRegistry("Notes Output") = "False" Then
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus database output is not enabled.")
                Exit Sub
            End If
        Catch ex As Exception

        End Try

        Try
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Starting VitalStatus STATUS  output.")
        Catch ex As Exception

        End Try

        'Dim strServerName As String
        Dim myKey As String

        'All data objects
        '   Dim myConnection As New OleDb.OleDbConnection
        '   Dim myAdapter As New OleDb.OleDbDataAdapter
        '   Dim myCommand As New OleDb.OleDbCommand
        Dim drv As DataRowView
        Dim dsStatus As New Data.DataSet
        Dim dtStatus As New Data.DataTable("Status")

        Dim dsClusterStatus As New Data.DataSet
        Dim dtClusterStatus As New Data.DataTable("DominoClusterHealth")

        Dim dsTraveler_Devices As New Data.DataSet
        Dim dtTraveler_Devices As New Data.DataTable("Traveler_Devices")

        '      Dim mySQLAdapter As New Data.SqlClient.SqlDataAdapter
        '      Dim mySQLCommand As New Data.SqlClient.SqlCommand

        'All Domino objects declared here
        ' Dim Session As New Domino.NotesSession
        Dim db As Domino.NotesDatabase
        Dim doc As Domino.NotesDocument
        Dim docPrevious As Domino.NotesDocument
        Dim view As Domino.NotesView
        Dim rtitem As Domino.NotesRichTextItem
        Dim rtitem1 As Domino.NotesRichTextItem
        Dim rtitem2 As Domino.NotesRichTextItem
        Dim rtitem3 As Domino.NotesRichTextItem
        Dim rtitem4 As Domino.NotesRichTextItem
        Dim rtitem5 As Domino.NotesRichTextItem
        Dim rtitem6 As Domino.NotesRichTextItem
        Dim richStyle As Domino.NotesRichTextStyle
        Dim coll As Domino.NotesDocumentCollection
        Dim viewDiskSpace As Domino.NotesView
        Dim viewClusterHealth As Domino.NotesView
        Dim viewConfig As Domino.NotesView
        Dim viewLastUpdate As Domino.NotesView
        Dim docDiskSpace As Domino.NotesDocument


        Dim ServerName, NSFfileName As String
        Try
            ServerName = Trim(myRegistry.ReadFromRegistry("Notes Output Server"))
        Catch ex As Exception
            ServerName = "Boston"
        End Try

        Try
            NSFfileName = Trim(myRegistry.ReadFromRegistry("Notes Output Database"))
        Catch ex As Exception
            NSFfileName = "VitalStatus.NSF"
        End Try

        Try
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus is attempting to connect to the database " & NSFfileName & " on " & ServerName & ".")
        Catch ex As Exception

        End Try

        Try
            db = NotesSession.GetDatabase(ServerName, NSFfileName, False)
            WriteAuditEntry(Now.ToString & " Successfully connected to the database.")
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Could not connect to the database. " & ex.ToString)
            '  db = Session.GetDatabase(ServerName, NSFfileName, False)
        End Try

        Try
            If db Is Nothing Then
                db = NotesSession.GetDatabase(ServerName, NSFfileName, False)
                WriteAuditEntry(Now.ToString & " Successfully connected to the database.")
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Could not connect to the database. " & ex.ToString)
            '  db = Session.GetDatabase(ServerName, NSFfileName, False)
        End Try

        Try
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " The VitalStatus database is: " & db.Title)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error connecting to VitalStatus database " & ex.ToString)
            GoTo CleanUp
        End Try


        Try
            If Not db Is Nothing Then
                viewDiskSpace = db.GetView("(DiskSpaceKey)")
                viewClusterHealth = db.GetView("ClusterHealthKey")
                viewConfig = db.GetView("(Lookup)")
                viewLastUpdate = db.GetView("(LastUpdate)")
            End If
        Catch ex As Exception

        End Try

        'clean up any disks without names
        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Cleaning up any drives without a name...  ")
        Try
            docPrevious = viewDiskSpace.GetFirstDocument
            docDiskSpace = viewDiskSpace.GetNextDocument(docPrevious)

            While Not docDiskSpace Is Nothing
                ' WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Processing... " & Trim(doc.GetItemValue("ServerName")(0)) & "-" & Trim(doc.GetItemValue("DiskName")(0)))
                If Trim(docDiskSpace.GetItemValue("DiskName")(0)) = "" Then
                    WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Removing blank drive entry for ..  " & Trim(docDiskSpace.GetItemValue("ServerName")(0)))
                    docPrevious = docDiskSpace
                    docDiskSpace = viewDiskSpace.GetNextDocument(docDiskSpace)
                    docPrevious.Remove(True)
                End If
                docDiskSpace = viewDiskSpace.GetNextDocument(docDiskSpace)
            End While
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception cleaning up  drives without a name...  " & ex.ToString)
        End Try


        'clean up any duplicate drive names
        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Cleaning up any duplicate drive entries...  ")
        Try
            docPrevious = viewDiskSpace.GetFirstDocument
            docDiskSpace = viewDiskSpace.GetNextDocument(docPrevious)
            Dim CurrentKey, PreviousKey As String
            If (docDiskSpace Is Nothing) Or (docPrevious Is Nothing) Then
                Exit Try
            End If
            ' WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " I have at least two drive entries...  ")
            While Not docDiskSpace Is Nothing
                CurrentKey = Trim(docDiskSpace.GetItemValue("ServerName")(0)) & "-" & Trim(docDiskSpace.GetItemValue("DiskName")(0))
                PreviousKey = Trim(docPrevious.GetItemValue("ServerName")(0)) & "-" & Trim(docPrevious.GetItemValue("DiskName")(0))

                If PreviousKey = CurrentKey Then
                    'duplicate drive names
                    WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Removing duplicate drive entry for ..  " & CurrentKey)
                    docPrevious.Remove(True)
                End If

                docPrevious = docDiskSpace
                docDiskSpace = viewDiskSpace.GetNextDocument(docDiskSpace)
            End While
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception cleaning up duplicate drives...  " & ex.ToString)
        End Try



        If Not db Is Nothing Then
            view = db.GetView("(ByKey)")
            If Not view Is Nothing Then
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Successfully connected to the key view.")
            Else
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Could not connect to the key view.")
            End If


            Try
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Updating last update field.")
                doc = viewLastUpdate.GetFirstDocument
                If doc Is Nothing Then
                    doc = db.CreateDocument
                    With doc
                        .ReplaceItemValue("Form", "LastUpdate")
                        .ReplaceItemValue("LastUpdate", Date.Now)
                        .Save(True, False)
                    End With
                Else
                    doc.ReplaceItemValue("LastUpdate", Date.Now)
                    doc.Save(True, False)
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception updating last update field. " & ex.ToString)
            End Try

            'Sometimes there is more than one lastupdate document.  if so, delete the second one
            Try
                doc = viewLastUpdate.GetNextDocument(doc)
                If Not doc Is Nothing Then
                    doc.RemovePermanently(True)
                End If
            Catch ex As Exception

            End Try

            Try
                doc = viewLastUpdate.GetNextDocument(doc)
                If Not doc Is Nothing Then
                    doc.RemovePermanently(True)
                End If
            Catch ex As Exception

            End Try
            Dim strSQL As String = "SELECT Category, HeldMail, HeldMailThreshold, DeadMail, Description, Details, DownCount, LastUpdate, Location, MailDetails, Name, PendingMail, Status, Type, UpPercent, ResponseTime, DeadThreshold, DominoServerTasks, MyPercent, NextScan, PendingThreshold, ResponseThreshold, Upcount, UserCount, OperatingSystem, DominoVersion, Icon, UpMinutes, DownMinutes, PercentageChange, TypeANDName FROM Status"
            Dim strSQL_Traveler As String = "SELECT UserName, DeviceName, OS_Type FROM Traveler_Devices"


            Try
                dsStatus.Clear()
                dsStatus.Tables.Add(dtStatus)
                dsTraveler_Devices.Clear()
                dsTraveler_Devices.Tables.Add(dtTraveler_Devices)
            Catch ex As Exception

            End Try

            Try
                'WRITTEN BY MUKUND 28Feb12
                Dim objVSAdaptor As New VSAdaptor
                objVSAdaptor.FillDatasetAny("VitalSigns", "Status", strSQL, dsStatus, "Status")
                objVSAdaptor.FillDatasetAny("VitalSigns", "Status", strSQL_Traveler, dsTraveler_Devices, "Traveler_Devices")
                'COMMENTED BY MUKUND 28Feb12

                'If boolUseSQLServer = True Then
                '    mySQLAdapter.Fill(dsStatus, "Status")
                '    mySQLCommand.CommandText = strSQL_Traveler
                '    mySQLAdapter.Fill(dsTraveler_Devices, "Traveler_Devices")
                'Else
                '    myAdapter.Fill(dsStatus, "Status")
                '    myCommand.CommandText = strSQL_Traveler
                '    myAdapter.Fill(dsTraveler_Devices, "Traveler_Devices")
                'End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception in Notes Output module: " & ex.Message)
                If InStr(ex.ToString, "System resource exceeded") > 0 Then
                    Try
                        WriteAuditEntry(Now.ToString & " System resources have been exceeded.  Shutting down to free resources.")
                        WriteAuditEntry(Now.ToString & " The VitalSigns Master Service should detect this and restart monitoring.")
                        Threading.Thread.Sleep(1000)
                    Catch ex2 As Exception

                    Finally
                        'Terminate the program
                        End
                    End Try
                End If

                GoTo CleanUp
            End Try

            'Create a Traveler Device Graph
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Attempting to produce Traveler graph.")
            Dim myDataView As New Data.DataView(dsTraveler_Devices.Tables("Traveler_Devices"), "DeviceName <> ''", "DeviceName", DataViewRowState.CurrentRows)
            myDataView.Sort = "DeviceName DESC"
            Dim existingDevice As TravelerDeviceSummary
            Dim myTravelerDevices As New TravelerDeviceCollection


            Try
                'Build a collection of Device objects, one for each Device encountered, with a count for each
                Dim drvTravelerSummary As DataRowView
                For Each drvTravelerSummary In myDataView
                    Dim myDevice As New TravelerDeviceSummary
                    If myTravelerDevices.HasItem(drvTravelerSummary("DeviceName")) Then
                        existingDevice = myTravelerDevices.FindItem(drvTravelerSummary("DeviceName"))
                        existingDevice.Count += 1
                    Else
                        myDevice.Name = drvTravelerSummary("DeviceName")
                        myDevice.Count = 1
                        myTravelerDevices.Add(myDevice)
                    End If
                Next
                '    dsTraveler_Devices.Dispose()
                '    dtTraveler_Devices.Dispose()
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception producing Traveler device summary: " & ex.ToString)
            End Try


            Dim myTravelerCode As String = " var p = new pie();" & vbCrLf
            Try
                'Need to output 
                ' var p = new pie();
                'p.add("Jan",100);
                'p.add("Feb",200);

                Dim TravelerGadget As TravelerDeviceSummary
                For Each TravelerGadget In myTravelerDevices
                    myTravelerCode += "p.add('" & Trim(TravelerGadget.Name) & "', '" & TravelerGadget.Count & "');" & vbCrLf
                Next

                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " My Traveler code is " & vbCrLf & myTravelerCode)

            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception producing Traveler graph code: " & ex.ToString)
            End Try

            Try
                If Not (viewConfig Is Nothing) Then
                    doc = viewConfig.GetFirstDocument
                    If doc Is Nothing Then
                        doc = db.CreateDocument
                        doc.ReplaceItemValue("Form", "Lookup")
                    End If
                    doc.ReplaceItemValue("Traveler_Graph_Code", myTravelerCode)
                    doc.Save(True, False)
                End If

            Catch ex As Exception

            End Try

            'Now again for the Client OS
            myTravelerDevices.Clear()

            Try
                'Build a collection of Device objects, one for each Device encountered, with a count for each

                Dim DeviceOS As String
                Dim drvTravelerSummary As DataRowView
                For Each drvTravelerSummary In myDataView
                    Dim myDevice As New TravelerDeviceSummary
                    DeviceOS = drvTravelerSummary("OS_Type")
                    If InStr(DeviceOS, "(OS 4") > 0 Then
                        DeviceOS = "Apple OS4"
                    End If
                    If InStr(DeviceOS, "(OS 5") > 0 Then
                        DeviceOS = "Apple OS5"
                    End If
                    If InStr(DeviceOS, "(OS 3") > 0 Then
                        DeviceOS = "Apple OS3"
                    End If

                    If myTravelerDevices.HasItem(DeviceOS) Then
                        existingDevice = myTravelerDevices.FindItem(DeviceOS)
                        existingDevice.Count += 1
                    Else
                        myDevice.Name = DeviceOS
                        myDevice.Count = 1
                        myTravelerDevices.Add(myDevice)
                    End If

                Next
                dsTraveler_Devices.Dispose()
                dtTraveler_Devices.Dispose()
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception producing Traveler OS summary: " & ex.ToString)
            End Try


            myTravelerCode = " var p = new pie();" & vbCrLf
            Try
                'Need to output 
                ' var p = new pie();
                'p.add("Jan",100);
                'p.add("Feb",200);

                Dim TravelerGadget As TravelerDeviceSummary
                For Each TravelerGadget In myTravelerDevices
                    myTravelerCode += "p.add('" & Trim(TravelerGadget.Name) & "', '" & TravelerGadget.Count & "');" & vbCrLf
                Next

                '      WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " My Traveler code is " & vbCrLf & myTravelerCode)

            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception producing Traveler graph code: " & ex.ToString)
            End Try

            Try
                If Not (viewConfig Is Nothing) Then
                    doc = viewConfig.GetFirstDocument
                    If doc Is Nothing Then
                        doc = db.CreateDocument
                        doc.ReplaceItemValue("Form", "Lookup")
                    End If
                    doc.ReplaceItemValue("Traveler_OS_Code", myTravelerCode)
                    doc.Save(True, False)
                End If

            Catch ex As Exception

            End Try

            '***************** End Traveler Device Graph



            Dim lastType As String = "Foo"
            Dim myView As New Data.DataView(dtStatus)
            myView.Sort = "Type ASC"

            Try
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Notes Output module is processing " & myView.Count & " records.")
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus module error " & ex.Message & " source: " & ex.Source)
            End Try


            For Each drv In myView
                myKey = drv("TypeANDName")
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus module is processing " & myKey)
                Try
                    doc = view.GetDocumentByKey(myKey)
                    WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus found a document for " & myKey)
                Catch ex As Exception
                    doc = Nothing
                End Try

                Try
                    coll = view.GetAllDocumentsByKey(myKey)
                    If coll.Count > 1 Then
                        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus found a duplicate document for " & myKey)
                        doc = coll.GetLastDocument
                        doc.Remove(True)
                        doc = coll.GetFirstDocument
                    End If

                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus module error processing duplicate documents for " & myKey)
                    '     doc = Nothing
                End Try

                Try
                    coll = view.GetAllDocumentsByKey(myKey)
                    If coll.Count > 1 Then
                        doc = coll.GetLastDocument
                        doc.Remove(True)
                        doc = coll.GetFirstDocument
                    End If

                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus module error processing duplicate documents for " & myKey)
                    '  doc = Nothing
                End Try

                If doc Is Nothing Then
                    doc = db.CreateDocument
                    doc.ReplaceItemValue("Form", "Device")
                    doc.ReplaceItemValue("TypeANDName", myKey)
                End If


                Try

                    If drv("Type") = "Domino Server" Then
                        ' WriteAuditEntry(Now.ToString & " I am processing a Domino server.")

                        Try
                            Dim myDominoServer As MonitoredItems.DominoServer
                            myDominoServer = MyDominoServers.Search(drv("Name"))

                            richStyle = NotesSession.CreateRichTextStyle
                            richStyle.FontSize = 8
                            richStyle.NotesFont = Domino.RT_FONTS.FONT_HELV

                            Try
                                If myDominoServer.QuickrServer = True Then
                                    doc.ReplaceItemValue("QuickrServer", "True")
                                End If
                            Catch ex As Exception

                            End Try

                            Dim PercentRAMinUse As Integer = 0
                            Try

                                PercentRAMinUse = ParseNumericStatValue("Platform.Memory.RAM.PctUtil", myDominoServer.Statistics_Platform)
                            Catch ex As Exception
                                PercentRAMinUse = 0
                            End Try

                            Try
                                doc.ReplaceItemValue("Memory_Percent_In_Use", PercentRAMinUse)
                                doc.ReplaceItemValue("EXJournal", myDominoServer.EXJournal_DocCount)
                                doc.ReplaceItemValue("EXJournal1", myDominoServer.EXJournal1_DocCount)
                                doc.ReplaceItemValue("EXJournal2", myDominoServer.EXJournal2_DocCount)
                            Catch ex As Exception

                            End Try



                            '********** Lotus Traveler Information
                            Try
                                If myDominoServer.Traveler_Server = True Then
                                    doc.ReplaceItemValue("TravelerServer", "True")
                                    'Traveler.Push.Devices.Online
                                    'Traveler.Push.Users.Online
                                    If InStr(myDominoServer.Statistics_Traveler, "Traveler.Version") > 0 Then
                                        Try
                                            doc.ReplaceItemValue("TravelerVersion", ParseTextStatValue("Traveler.Version", myDominoServer.Statistics_Traveler))
                                            doc.ReplaceItemValue("TravelerBuild", ParseTextStatValue("Traveler.Version.BuildNumber", myDominoServer.Statistics_Traveler))
                                            doc.ReplaceItemValue("TravelerLastSyncTime", ParseTextStatValue("Traveler.DeviceSync.LastSyncTime ", myDominoServer.Statistics_Traveler))
                                            doc.ReplaceItemValue("TravelerLastSyncDate", ParseTextStatValue("Traveler.DeviceSync.LastSyncDate ", myDominoServer.Statistics_Traveler))
                                            doc.ReplaceItemValue("TravelerLastUserName", ParseTextStatValue("Traveler.DeviceSync.LastUserName ", myDominoServer.Statistics_Traveler))
                                        Catch ex As Exception

                                        End Try
                                    End If
                                    doc.ReplaceItemValue("TravelerPushDevicesOnline", ParseNumericStatValue("Traveler.Push.Devices.Online", myDominoServer.Statistics_Traveler))
                                    '  doc.ReplaceItemValue("TravelerTotalDeviceCount", ParseNumericStatValue("Traveler.Push.Devices.Total", myDominoServer.Statistics_Traveler))
                                    doc.ReplaceItemValue("TravelerTotalUserCount", ParseNumericStatValue("Traveler.Push.Users.Online", myDominoServer.Statistics_Traveler))
                                    doc.ReplaceItemValue("TravelerSuccessfulSync", myDominoServer.Traveler_Successful_DeviceSync_Count - myDominoServer.Traveler_Previous_Successful_DeviceSync_Count)
                                    'doc.ReplaceItemValue("TravelerUnderOneSecondPercent", myDominoServer.Traveler_Mail_DB_OpenUnderOneSecond_Percent)
                                    'doc.ReplaceItemValue("TravelerUnderTwoSecondsPercent", myDominoServer.Traveler_Mail_DB_OpenUnderTwoSeconds_Percent)
                                    'doc.ReplaceItemValue("TravelerUnderTenSecondsPercent", myDominoServer.Traveler_Mail_DB_OpenUnderTenSeconds_Percent)
                                    'doc.ReplaceItemValue("TravelerUnderThirtySeconds", myDominoServer.Traveler_Mail_DB_OpenUnderThirtySeconds_Percent)
                                    'doc.ReplaceItemValue("TravelerUnderSixtySeconds", myDominoServer.Traveler_Mail_DB_OpenUnderSixtySeconds_Percent)
                                    'doc.ReplaceItemValue("TravelerOverSixtySeconds", myDominoServer.Traveler_Mail_DB_OpenOverSixtySeconds_Percent)
                                    doc.ReplaceItemValue("Max_HTTP_Connections", ParseTextStatValue("Http.PeakConnections", myDominoServer.Statistics_HTTP))
                                    doc.ReplaceItemValue("Available_HTTP_Connections", ParseTextStatValue("Http.Workers", myDominoServer.Statistics_HTTP))
                                    Try
                                        doc.ReplaceItemValue("Required_HTTP_Connections", CInt(ParseNumericStatValue("Traveler.Push.Devices.Online", myDominoServer.Statistics_Traveler)) * 1.2)
                                    Catch ex As Exception

                                    End Try
                                    Try
                                        Call doc.RemoveItem("Statistics_Traveler")
                                    Catch ex As Exception

                                    End Try

                                    Try
                                        rtitem1 = doc.CreateRichTextItem("Statistics_Traveler")
                                        Call rtitem1.AppendStyle(richStyle)
                                        Call rtitem1.AppendText(myDominoServer.Statistics_Traveler)
                                    Catch ex As Exception
                                        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error processing Statistics_Traveler: " & ex.ToString)
                                    End Try

                                    Try
                                        Call doc.RemoveItem("TravelerMailOpenStats")
                                    Catch ex As Exception

                                    End Try

                                    Try
                                        Dim MyStats As String = ParseTravelerPerformanceStats(myDominoServer.Statistics_Traveler)
                                        ' WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " My Traveler stats are: " & vbCrLf & MyStats)
                                        rtitem1 = doc.CreateRichTextItem("TravelerMailOpenStats")
                                        Call rtitem1.AppendStyle(richStyle)
                                        Call rtitem1.AppendText(MyStats)

                                    Catch ex As Exception
                                        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error processing TravelerMailOpenStats: " & ex.ToString)
                                    End Try
                                End If

                                Try
                                    Dim myTravelerTask As MonitoredItems.ServerTask
                                    myTravelerTask = myDominoServer.ServerTasks.Search("Lotus Traveler")
                                    doc.ReplaceItemValue("TravelerStatus", myTravelerTask.Status & " - " & myTravelerTask.SecondaryStatus)
                                    myDominoServer.Traveler_Details = myTravelerTask.SecondaryStatus
                                Catch ex As Exception

                                End Try

                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception while processing Traveler information: " & ex.ToString)
                            End Try


                            '*************** Disk Space Form
                            Try
                                '   WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Processing disk space")
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Domino server " & myDominoServer.Name & " has " & myDominoServer.DiskDrives.Count & " drives. ")

                                If myDominoServer.DiskDrives.Count > 0 And Not (viewDiskSpace Is Nothing) Then
                                    'Make a copy of the drive collection, in case the server is being scanned now in the background, it modifies the colleciton
                                    Dim myDriveCollection As MonitoredItems.DominoDiskSpaceCollection
                                    myDriveCollection = myDominoServer.DiskDrives

                                    For Each drive As MonitoredItems.DominoDiskSpace In myDriveCollection
                                        '  If InStr(myDominoServer.Statistics_Disk, drive.DiskName & ".Free") And drive.DiskName <> "" Then

                                        Try
                                            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Processing  " & myDominoServer.Name & drive.DiskName)
                                            docDiskSpace = Nothing
                                            docDiskSpace = viewDiskSpace.GetDocumentByKey(myDominoServer.Name & "-" & drive.DiskName, True)
                                        Catch ex As Exception
                                            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error processing disk space document #1: " & ex.ToString)
                                        End Try

                                        If docDiskSpace Is Nothing Then
                                            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Creating a new disk space document for " & myDominoServer.Name)
                                            docDiskSpace = db.CreateDocument()
                                            docDiskSpace.ReplaceItemValue("Form", "Disk Space")
                                            docDiskSpace.ReplaceItemValue("ServerName", myDominoServer.Name)
                                            docDiskSpace.ReplaceItemValue("DiskName", drive.DiskName)
                                            docDiskSpace.ReplaceItemValue("Threshold", myDominoServer.DiskThreshold)
                                            Call docDiskSpace.Save(True, False)
                                        End If

                                        With docDiskSpace
                                            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Updating disk space document for " & myDominoServer.Name)
                                            If drive.DiskSize > 0 Then
                                                .ReplaceItemValue("DiskSize", drive.DiskSize)
                                                .ReplaceItemValue("DiskFree", drive.DiskFree)
                                                .ReplaceItemValue("DiskPercentFree", drive.PercentFree)
                                            End If

                                            .ReplaceItemValue("PercentUtil", drive.DiskPercentUtilization)
                                            .ReplaceItemValue("QueueLen", drive.DiskAverageQueueLength)
                                            .ReplaceItemValue("LastUpdated", drive.LastUpdated)
                                            .Save(True, False)
                                        End With
                                        ' End If
                                    Next

                                End If


                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error processing disk space document: " & ex.ToString)
                            End Try

                            Try
                                doc.RemoveItem("Mail.Transferred")
                            Catch ex As Exception

                            End Try

                            Try
                                If Not myDominoServer.TransferredMail <> 0 Then
                                    ' doc.AppendItemValue("Mail.Transferred", myDominoServer.TransferredMail)
                                    doc.ReplaceItemValue("Mail.Transferred", myDominoServer.TransferredMail)
                                End If

                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception while writing Mail.Transferred: " & ex.ToString)
                            End Try

                            Try
                                doc.ReplaceItemValue("Mail.Delivered", myDominoServer.DeliveredMail)
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception while writing Mail.Delivered: " & ex.ToString)
                            End Try

                            Try
                                doc.ReplaceItemValue("Mail.Routed", myDominoServer.RoutedMail)
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception while writing Mail.Routed: " & ex.ToString)
                            End Try
                            Try
                                doc.ReplaceItemValue("Mail.SMTPMailProcessed", myDominoServer.SMTPMailProcessed)
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception while writing Mail.SMTPMailProcessed: " & ex.ToString)
                            End Try
                            Try
                                doc.ReplaceItemValue("AvailabilityIndex", myDominoServer.AvailabilityIndex)
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception while writing AvailabilityIndex: " & ex.ToString)
                            End Try


                            Try
                                doc.ReplaceItemValue("PeakResponse", myDominoServer.PeakResponseTime)
                                doc.ReplaceItemValue("PeakResponseTime", myDominoServer.PeakResponseDateTime)
                                doc.ReplaceItemValue("PeakPending", myDominoServer.PeakPendingMail)
                                doc.ReplaceItemValue("PeakPendingTime", myDominoServer.PeakPendingTime)
                                doc.ReplaceItemValue("PeakDead", myDominoServer.PeakDeadMail)
                                doc.ReplaceItemValue("PeakDeadTime", myDominoServer.PeakDeadTime)
                                doc.ReplaceItemValue("AvailabilityPercentage", myDominoServer.UpPercentMinutes)
                                '  doc.ReplaceItemValue("Delivered Mail", myDominoServer.DeliveredMail)
                                If myDominoServer.MailboxCount <> 999 Then
                                    doc.ReplaceItemValue("MailboxCount", myDominoServer.MailboxCount)
                                End If

                            Catch ex As Exception

                            End Try

                            Try
                                doc.ReplaceItemValue("Server.Users.Peak", ParseNumericStatValue("Server.Users.Peak", myDominoServer.Statistics_Server))
                                doc.ReplaceItemValue("Server.Users.Peak.Time", ParseNumericStatValue("Server.Users.Peak.Time", myDominoServer.Statistics_Server))
                                doc.ReplaceItemValue("Server.Users.Active30Min", ParseNumericStatValue("Server.Users.Active30Min", myDominoServer.Statistics_Server))

                                doc.ReplaceItemValue("Server.Time.Start", ParseTextStatValue("Server.Time.Start", myDominoServer.Statistics_Server))
                            Catch ex As Exception

                            End Try


                            Try
                                doc.ReplaceItemValue("Server.Trans.PerMinute", ParseNumericStatValue("Server.Trans.PerMinute", myDominoServer.Statistics_Server))
                            Catch ex As Exception

                            End Try

                            Try
                                doc.ReplaceItemValue("Server.ElapsedTime", ParseTextStatValue("Server.ElapsedTime", myDominoServer.Statistics_Server))
                            Catch ex As Exception

                            End Try

                            Try
                                doc.ReplaceItemValue("Server.Time.Start", ParseTextStatValue("Server.Time.Start", myDominoServer.Statistics_Server))
                            Catch ex As Exception

                            End Try

                            Try
                                If InStr(myDominoServer.Statistics_Server, "Server.Administrators") > 0 Then
                                    doc.ReplaceItemValue("Server.Administrators", ParseTextStatValue("Server.Administrators", myDominoServer.Statistics_Server))
                                End If

                            Catch ex As Exception

                            End Try

                            Try
                                doc.ReplaceItemValue("HostName", myDominoServer.IPAddress)

                                If InStr(myDominoServer.OperatingSystem, "Longhorn") > 0 Then
                                    doc.ReplaceItemValue("OperatingSystem", "Windows Server 2008")
                                Else
                                    doc.ReplaceItemValue("OperatingSystem", myDominoServer.OperatingSystem)
                                End If

                            Catch ex As Exception

                            End Try

                            Try
                                If InStr(myDominoServer.Statistics_Server, "Server.Version.Notes") > 0 Then
                                    doc.ReplaceItemValue("DominoVersion", ParseTextStatValue("Server.Version.Notes", myDominoServer.Statistics_Server))
                                End If

                            Catch ex As Exception

                            End Try

                            Try
                                doc.ReplaceItemValue("HostName", myDominoServer.IPAddress)
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception while writing HostName: " & ex.ToString)
                            End Try

                            Try
                                doc.ReplaceItemValue("Server.Title", myDominoServer.Title)
                                doc.ReplaceItemValue("Server.TaskStatus", myDominoServer.TaskStatus)
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error processing Task Status: " & ex.ToString)
                            End Try


                            Try
                                doc.ReplaceItemValue("CPU_Utilization_Combined", myDominoServer.CPU_Utilization)
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error processing CPU Utilization: " & ex.ToString)
                            End Try

                            Try
                                doc.ReplaceItemValue("MailBoxPerformanceIndex", myDominoServer.MailBoxPerformanceIndex)
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error processing MailBoxPerformanceIndex: " & ex.ToString)
                            End Try

                            Try
                                doc.ReplaceItemValue("Server.Version.Architecture", ParseTextStatValue("Server.Version.Architecture", myDominoServer.Statistics_Server))

                            Catch ex As Exception

                            End Try

                            Dim myServerTask As MonitoredItems.ServerTask
                            For Each ServerTask As MonitoredItems.ServerTaskSetting In myDominoServer.ServerTaskSettings
                                'For Each ServerTask As MonitoredItems.ServerTask In myDominoServer.ServerTaskSettings
                                If ServerTask.Enabled = True Then
                                    '  WriteAuditEntry(Now.ToString & " I am processing task " & ServerTask.Name)
                                    doc.ReplaceItemValue(ServerTask.Name.Replace(" ", "_"), ServerTask.Status)
                                End If
                            Next
                            Dim myBody As String


                            If myDominoServer.ShowTasks <> "" Then

                                Try
                                    Call doc.RemoveItem("Server_Tasks")
                                    rtitem = doc.CreateRichTextItem("Server_Tasks")
                                Catch ex As Exception
                                    rtitem = doc.CreateRichTextItem("Server_Tasks")
                                End Try

                                Try
                                    'rtitem = doc.CreateRichTextItem("Server_Tasks")
                                    Call rtitem.AppendStyle(richStyle)
                                    Call rtitem.AppendText("Server Task Details as of: " & Now.ToLongDateString & " " & Now.ToLongTimeString)
                                    Call rtitem.AddNewLine()
                                    Call rtitem.AddNewLine()
                                    '  Call rtitem.AppendText(myDominoServer.ShowTasks)
                                    Call rtitem.AppendText(myDominoServer.ShowTasks.Replace(ControlChars.NewLine, ""))
                                Catch ex As Exception
                                    WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error processing Server_Tasks: " & ex.ToString)
                                End Try
                            End If

                            Try
                                Call doc.RemoveItem("Statistics_Mail")
                                ' rtitem2 = doc.GetFirstItem("Statistics_Mail")
                                'rtitem2.Remove()
                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " Error removing existing Statistics_Mail field: " & ex.ToString)
                            End Try

                            Try
                                If Not doc Is Nothing Then
                                    rtitem2 = doc.CreateRichTextItem("Statistics_Mail")
                                    Call rtitem2.AppendStyle(richStyle)
                                    Call rtitem2.AppendText("Mail Statistics as of: " & Now.ToLongDateString & " " & Now.ToLongTimeString)
                                    Call rtitem2.AddNewLine()
                                    Call rtitem2.AddNewLine()
                                    Call rtitem2.AppendText(myDominoServer.Statistics_Mail)
                                End If
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error processing Statistics_Mail: " & ex.ToString)
                            End Try

                            Try
                                Try
                                    Call doc.RemoveItem("Statistics_Memory")
                                    ' rtitem3 = doc.GetFirstItem("Statistics_Memory")
                                    'rtitem3.Remove()
                                    rtitem3 = doc.CreateRichTextItem("Statistics_Memory")
                                Catch ex As Exception
                                    rtitem3 = doc.CreateRichTextItem("Statistics_Memory")
                                End Try
                                '  rtitem3 = doc.CreateRichTextItem("Statistics_Memory")
                                Call rtitem3.AppendStyle(richStyle)
                                'Call rtitem3.AppendText("Memory Statistics as of: " & Now.ToLongDateString & " " & Now.ToLongTimeString)
                                'Call rtitem3.AddNewLine()
                                ' Call rtitem3.AddNewLine()
                                Call rtitem3.AppendText(myDominoServer.Statistics_Memory)
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error processing Statistics_Memory: " & ex.ToString)
                            End Try

                            Try
                                Try
                                    Call doc.RemoveItem("Statistics_Disk")
                                    rtitem4 = doc.GetFirstItem("Statistics_Disk")
                                    rtitem4.Remove()
                                    rtitem4 = doc.CreateRichTextItem("Statistics_Disk")
                                Catch ex As Exception
                                    rtitem4 = doc.CreateRichTextItem("Statistics_Disk")
                                End Try
                                Call rtitem4.AppendStyle(richStyle)
                                Call rtitem4.AppendText("Disk Statistics as of: " & Now.ToLongDateString & " " & Now.ToLongTimeString)
                                Call rtitem4.AddNewLine()
                                Call rtitem4.AddNewLine()
                                Call rtitem4.AppendText(myDominoServer.Statistics_Disk)
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error processing Statistics_Disk: " & ex.ToString)

                            End Try
                            Try
                                Try
                                    Call doc.RemoveItem("Statistics_Server")
                                    rtitem5 = doc.GetFirstItem("Statistics_Server")
                                    rtitem5.Remove()
                                    rtitem5 = doc.CreateRichTextItem("Statistics_Server")
                                Catch ex As Exception
                                    rtitem5 = doc.CreateRichTextItem("Statistics_Server")
                                End Try
                                Call rtitem5.AppendStyle(richStyle)
                                'Call rtitem5.AppendText("Disk Statistics as of: " & Now.ToLongDateString & " " & Now.ToLongTimeString)
                                ' Call rtitem5.AddNewLine()
                                ' Call rtitem5.AddNewLine()
                                Call rtitem5.AppendText(myDominoServer.Statistics_Server)
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error processing Statistics_Disk: " & ex.ToString)
                            End Try

                            Try

                                Try
                                    Call doc.RemoveItem("Statistics_Replica")
                                    rtitem6 = doc.GetFirstItem("Statistics_Replica")
                                    rtitem6.Remove()
                                    rtitem6 = doc.CreateRichTextItem("Statistics_Replica")
                                Catch ex As Exception
                                    rtitem6 = doc.CreateRichTextItem("Statistics_Replica")
                                End Try
                                Call rtitem6.AppendStyle(richStyle)
                                Call rtitem6.AppendText("Replication Statistics as of: " & Now.ToLongDateString & " " & Now.ToLongTimeString)
                                Call rtitem6.AddNewLine()
                                Call rtitem6.AddNewLine()
                                Call rtitem6.AppendText(myDominoServer.Statistics_Replica)
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error processing Statistics_Replica: " & ex.ToString)
                            End Try

                            Try
                                '    doc.ReplaceItemValue("Server_Tasks", myDominoServer.ShowTasks.Replace(ControlChars.Cr, ControlChars.NewLine))
                                If myDominoServer.Memory <> "" Then
                                    doc.ReplaceItemValue("Memory", myDominoServer.Memory)
                                End If

                                ' doc.ReplaceItemValue("Statistics_Mail", myDominoServer.Statistics_Mail)
                                ' doc.ReplaceItemValue("Statistics_Memory", myDominoServer.Statistics_Memory)
                                ' doc.ReplaceItemValue("Statistics_Disk", myDominoServer.Statistics_Disk)
                                'doc.ReplaceItemValue("Statistics_Server", myDominoServer.Statistics_Server)
                                'doc.ReplaceItemValue("Statistics_Replica", myDominoServer.Statistics_Replica)
                                If myDominoServer.Statistics_Platform <> "" Then
                                    doc.ReplaceItemValue("Statistics_Platform", myDominoServer.Statistics_Platform)
                                End If
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Notes Output module error outputting stats " & ex.ToString)
                            End Try


                            Dim PercentFree, MyDiskSize, MyFreeSpace As Double
                            Dim myDiskSummary As String
                            ' WriteAuditEntry(Now.ToString & " Notes Output module is processing disk space ")

                            Try
                                For n As Integer = 0 To DiskName.GetUpperBound(0)
                                    If InStr(myDominoServer.Statistics_Disk, DiskName(n) & ".Free") And DiskName(n) <> "" And (DiskName(n) & ".Free") <> ".Free" Then
                                        ' WriteAuditEntry(Now.ToString & " Server has disk " & DiskName(n))
                                        '  WriteAuditEntry(Now.ToString & " Notes Output module is processing disk " & DiskName(n))

                                        Try
                                            MyFreeSpace = ParseNumericStatValue(DiskName(n) & ".Free", myDominoServer.Statistics_Disk)
                                        Catch ex As Exception
                                            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error calculating free space " & ex.ToString)
                                        End Try

                                        Try
                                            MyDiskSize = ParseNumericStatValue(DiskName(n) & ".Size", myDominoServer.Statistics_Disk)
                                        Catch ex As Exception
                                            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error calculating disk space " & ex.ToString)
                                        End Try
                                        Try
                                            doc.ReplaceItemValue(DiskName(n) & ".Free", MyFreeSpace)
                                            doc.ReplaceItemValue(DiskName(n) & ".Size", MyDiskSize)
                                        Catch ex As Exception
                                            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error saving disk info " & ex.ToString)
                                        End Try

                                        Try
                                            PercentFree = MyFreeSpace / MyDiskSize
                                            myDiskSummary += DiskName(n) & " Free: " & MyFreeSpace.ToString("n") & ControlChars.NewLine
                                            myDiskSummary += DiskName(n) & " Size: " & MyDiskSize.ToString("n") & ControlChars.NewLine
                                            myDiskSummary += DiskName(n) & " Percent Free: " & PercentFree.ToString("p") & ControlChars.NewLine & ControlChars.NewLine
                                        Catch ex As Exception

                                        End Try
                                        '  WriteAuditEntry(Now.ToString & " Notes Output module disk summary " & vbCrLf & myDiskSummary)
                                    End If
                                Next n
                            Catch ex As Exception

                            End Try

                            '    End Try
                            myDominoServer = Nothing
                        Catch ex As Exception

                        End Try

                    End If



                    Try
                        doc.ReplaceItemValue("Name", drv("Name"))
                        doc.ReplaceItemValue("Status", drv("Status"))
                        doc.ReplaceItemValue("Type", drv("Type"))
                        doc.ReplaceItemValue("Location", drv("Location"))
                        doc.ReplaceItemValue("Category", drv("Category"))
                        doc.ReplaceItemValue("Details", drv("Details"))
                    Catch ex As Exception
                    End Try



                    Try
                        doc.ReplaceItemValue("LastUpdate", drv("LastUpdate"))
                        doc.ReplaceItemValue("NextScan", drv("NextScan"))
                        doc.ReplaceItemValue("Description", drv("Description"))
                        doc.ReplaceItemValue("PendingMail", drv("PendingMail"))
                        doc.ReplaceItemValue("DeadMail", drv("DeadMail"))
                        doc.ReplaceItemValue("HeldMail", drv("HeldMail"))
                    Catch ex As Exception

                    End Try

                    Try
                        doc.ReplaceItemValue("Upcount", drv("Upcount"))
                        doc.ReplaceItemValue("DownCount", drv("DownCount"))
                        doc.ReplaceItemValue("UpMinutes", drv("UpMinutes"))
                        doc.ReplaceItemValue("DownMinutes", drv("DownMinutes"))
                        doc.ReplaceItemValue("PercentageChange", drv("PercentageChange"))
                        doc.ReplaceItemValue("MyPercent", drv("MyPercent"))
                    Catch ex As Exception

                    End Try

                    Try

                        doc.ReplaceItemValue("ResponseTime", drv("ResponseTime"))
                        doc.ReplaceItemValue("ResponseThreshold", drv("ResponseThreshold"))
                        doc.ReplaceItemValue("Description", drv("Description"))
                        doc.ReplaceItemValue("PendingMail", drv("PendingMail"))
                        doc.ReplaceItemValue("PendingThreshold", drv("PendingThreshold"))
                        doc.ReplaceItemValue("DeadThreshold", drv("DeadThreshold"))
                        doc.ReplaceItemValue("HeldMailThreshold", drv("HeldMailThreshold"))
                        doc.ReplaceItemValue("UserCount", drv("UserCount"))
                    Catch ex As Exception

                    End Try

                    Try
                        '  doc.ReplaceItemValue("OperatingSystem", drv("OperatingSystem"))
                        '  doc.ReplaceItemValue("DominoVersion", drv("DominoVersion"))
                        doc.ReplaceItemValue("UpMinutes", drv("UpMinutes"))
                        doc.ReplaceItemValue("DownMinutes", drv("DownMinutes"))
                    Catch ex As Exception

                    End Try
                    Try
                        doc.Save(True, False)
                    Catch ex As Exception

                    End Try

                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Notes Output module error updating Notes document " & ex.Message)
                End Try

            Next

            Try
                dsStatus.Dispose()
                dtStatus.Dispose()
                myView.Dispose()
                ' WriteAuditEntry(Now.ToString & " Disposed items")

            Catch ex As Exception

            End Try
            Try
                lastType = Nothing
            Catch ex As Exception

            End Try
        End If

        '******************************* Cluster Health
        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus is processing Cluster Health ")

        If viewClusterHealth Is Nothing Then GoTo CleanUp

        Try
            dsClusterStatus.Clear()
            dsClusterStatus.Tables.Add(dtClusterStatus)

            Dim strSQL As String = "SELECT ServerName, ClusterName, SecondsOnQueue, SecondsOnQueueAvg, SecondsOnQueueMax, WorkQueueDepth, WorkQueueDepthMax, WorkQueueDepthAvg, LastUpdate, Availability, AvailabilityThreshold, Analysis FROM DominoClusterHealth WHERE ClusterName <> ''"
            Dim objVSAdaptor As New VSAdaptor
            objVSAdaptor.FillDatasetAny("VitalSigns", "Status", strSQL, dsClusterStatus, "DominoClusterHealth")

        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception in VitalStatus cluster health module: " & ex.Message)
            GoTo CleanUp
        End Try

        Dim myClusterView As New Data.DataView(dtClusterStatus)
        myClusterView.Sort = "ServerName ASC"

        Try
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus is processing " & myClusterView.Count & " cluster health records.")
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus module error " & ex.Message & " source: " & ex.Source)
        End Try


        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus is cleaning up cluster records.")
        Try
            doc = viewClusterHealth.GetFirstDocument
            While Not doc Is Nothing
                If InStr(doc.GetItemValue("ClusterName")(0), "NSPingServer") Then
                    docPrevious = doc
                    doc = viewClusterHealth.GetNextDocument(doc)
                    docPrevious.Remove(True)
                End If
                doc = viewClusterHealth.GetNextDocument(doc)
            End While
        Catch ex As Exception

        End Try

        Try
            doc = viewClusterHealth.GetFirstDocument
            While Not doc Is Nothing
                If Trim(doc.GetItemValue("ClusterNAme")(0)) = "" Then
                    docPrevious = doc
                    doc = viewClusterHealth.GetNextDocument(doc)
                    docPrevious.Remove(True)
                End If
                doc = viewClusterHealth.GetNextDocument(doc)
            End While
        Catch ex As Exception

        End Try

        Try
            doc = viewClusterHealth.GetFirstDocument
            While Not doc Is Nothing
                If InStr(doc.GetItemValue("ClusterNAme")(0), "    ") > 0 Then
                    docPrevious = doc
                    doc = viewClusterHealth.GetNextDocument(doc)
                    docPrevious.Remove(True)
                End If
                doc = viewClusterHealth.GetNextDocument(doc)
            End While
        Catch ex As Exception

        End Try

        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus is looping through current cluster records.")

        For Each drv In myClusterView
            'Clustername + "-"+ ServerName
            Try
                myKey = Trim(drv("ClusterName")) & "-" & Trim(drv("ServerName"))
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus cluster health module is processing " & myKey)
            Catch ex As Exception

            End Try

            Try
                doc = viewClusterHealth.GetDocumentByKey(myKey)
            Catch ex As Exception
                doc = Nothing
            End Try

            If doc Is Nothing Then
                myKey = Trim(drv("ServerName")) & Trim(drv("ClusterName"))
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus cluster health module is processing alternate key " & myKey)
            End If

            Try
                coll = viewClusterHealth.GetAllDocumentsByKey(myKey)
                If coll.Count > 1 Then
                    WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " There are  " & coll.Count & " documents with this key.")
                    doc = coll.GetLastDocument
                    doc.Remove(True)
                    doc = coll.GetFirstDocument
                End If

            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus module error processing duplicate Cluster Health documents for " & myKey)
                doc = Nothing
            End Try

            Try
                doc = viewClusterHealth.GetDocumentByKey(myKey)
            Catch ex As Exception
                doc = Nothing
            End Try


            If doc Is Nothing And drv("ClusterName") <> "" And Not (InStr(drv("ClusterName"), "NSPingServer")) Then
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus is creating a new cluster document.")
                doc = db.CreateDocument
                doc.ReplaceItemValue("Form", "ClusterHealth")
                doc.ReplaceItemValue("Servername", Trim(drv("ServerName")))
            End If

            Try
                If Trim(drv("ClusterName")) <> "" And Not (InStr(drv("ClusterName"), "NSPingServer")) Then
                    doc.ReplaceItemValue("ClusterName", drv("ClusterName"))
                End If

                doc.ReplaceItemValue("SecondsOnQueue", drv("SecondsOnQueue"))
                doc.ReplaceItemValue("SecondsOnQueueAvg", drv("SecondsOnQueueAvg"))
                doc.ReplaceItemValue("SecondsOnQueueMax", drv("SecondsOnQueueMax"))
                doc.ReplaceItemValue("WorkQueueDepth", drv("WorkQueueDepth"))
                doc.ReplaceItemValue("WorkQueueDepthMax", drv("WorkQueueDepthMax"))
                doc.ReplaceItemValue("WorkQueueDepthAvg", drv("WorkQueueDepthAvg"))
                doc.ReplaceItemValue("LastUpdate", drv("LastUpdate"))
                doc.ReplaceItemValue("AvailabilityIndex", drv("Availability"))
                doc.ReplaceItemValue("AvailabilityThreshold", drv("AvailabilityThreshold"))
                doc.ReplaceItemValue("Analysis", drv("Analysis"))

            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error updating Cluster Health document: " & ex.ToString)
            End Try

            Try
                doc.Save(True, False)
            Catch ex As Exception

            End Try

        Next

CleanUp:

        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(view)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(doc)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(docPrevious)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(richStyle)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(rtitem)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(rtitem1)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(rtitem2)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(rtitem3)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(rtitem4)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(rtitem5)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(rtitem6)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(coll)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(viewClusterHealth)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(viewDiskSpace)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(viewLastUpdate)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(viewConfig)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(docDiskSpace)
           
            '   System.Runtime.InteropServices.Marshal.ReleaseComObject(Session)
        Catch ex As Exception

        End Try

        Try
            dsClusterStatus.Dispose()
            dtClusterStatus.Dispose()
            viewClusterHealth.Dispose()
        Catch ex As Exception

        End Try

        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus module finished updating " & NSFfileName & " on " & ServerName)
        GC.Collect()
    End Sub

    Public Sub VitalStatus_UpdateWithDominoStats(ByRef Session As Domino.NotesSession, ByRef DominoServer As MonitoredItems.DominoServer)
        Dim myRegistry As New RegistryHandler
        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Putting performance statistics into VitalStatus database.")
        Dim doc As Domino.NotesDocument
        Dim db As Domino.NotesDatabase
        Dim view As Domino.NotesView

        Dim ServerName, NSFfileName As String
        Try
            ServerName = myRegistry.ReadFromRegistry("Notes Output Server")
        Catch ex As Exception
            ServerName = "Boston"
        End Try

        Try
            NSFfileName = myRegistry.ReadFromRegistry("Notes Output Database")
        Catch ex As Exception
            NSFfileName = "VitalStatus.NSF"
        End Try
        '  WriteAuditEntry(Now.ToString & " Attempting to connect to the database " & NSFfileName & " on " & ServerName)

        Try
            db = Session.GetDatabase(ServerName, NSFfileName, False)
            '       WriteAuditEntry(Now.ToString & " -------------Successfully connected to the database.")
        Catch ex As Exception

            ' System.Runtime.InteropServices.Marshal.ReleaseComObject(Session)
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Could not connect to the database.")
            GoTo CleanUp
        End Try

        '      WriteAuditEntry(Now.ToString & " --------- Creating Stats doc in database -----------")
        Try
            doc = db.CreateDocument
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Could not create Notes document to store statistics.")
            GoTo CleanUp
        End Try

        Try
            With doc
                .ReplaceItemValue("Form", "Statistic")
                .ReplaceItemValue("Type", "Domino Server")
                .ReplaceItemValue("Name", DominoServer.Name)
                .ReplaceItemValue("Response_Time", DominoServer.ResponseTime)
                .ReplaceItemValue("Response_Threshold", DominoServer.ResponseThreshold)
                .ReplaceItemValue("Users", DominoServer.UserCount)
                .ReplaceItemValue("Mail.Routed", DominoServer.RoutedMail)
                .ReplaceItemValue("Downtime_Minutes", DominoServer.DownMinutes)
                .ReplaceItemValue("Mail.Delivered", DominoServer.DeliveredMail)
                .ReplaceItemValue("Mail.Transferred", DominoServer.TransferredMail)
                .ReplaceItemValue("Mail.Pending", DominoServer.PendingMail)
                .ReplaceItemValue("Pending_Threshold", DominoServer.PendingThreshold)
                .ReplaceItemValue("AvailabilityIndex", DominoServer.AvailabilityIndex)
                .Save(True, False)
            End With
        Catch ex As Exception

        End Try

Cleanup:
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(view)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(doc)
            ' System.Runtime.InteropServices.Marshal.ReleaseComObject(Session)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub VitalStatus_Cleanup()
        If Date.Now.DayOfWeek <> DayOfWeek.Sunday Then
            ' WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Normally, I wouldn't do this except on Sunday but I am cleaning up now.")
            Exit Sub
        End If


        Dim myRegistry As New RegistryHandler

        Try
            If myRegistry.ReadFromRegistry("Notes Output") = "False" Then
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus database output is not enabled.")
                Exit Sub
            End If
        Catch ex As Exception

        End Try

        Try
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Starting VitalStatus database output.")
        Catch ex As Exception

        End Try

        'Dim strServerName As String
        Dim myKey As String

        'All Domino objects declared here
        '  Dim Session As New Domino.NotesSession
        Dim db As Domino.NotesDatabase
        Dim doc As Domino.NotesDocument

        Dim viewDiskSpace As Domino.NotesView
        Dim viewClusterHealth As Domino.NotesView
        Dim viewByKey As Domino.NotesView
        Dim viewDeviceID As Domino.NotesView


        Dim ServerName, NSFfileName As String
        Try
            ServerName = Trim(myRegistry.ReadFromRegistry("Notes Output Server"))
        Catch ex As Exception
            ServerName = "Boston"
        End Try

        Try
            NSFfileName = Trim(myRegistry.ReadFromRegistry("Notes Output Database"))
        Catch ex As Exception
            NSFfileName = "VitalStatus.NSF"
        End Try

        Try
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus is attempting to connect to the database " & NSFfileName & " on " & ServerName & ".")
        Catch ex As Exception

        End Try

        Try
            db = NotesSession.GetDatabase(ServerName, NSFfileName, False)
            WriteAuditEntry(Now.ToString & " Successfully connected to the database.")
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Could not connect to the database. " & ex.ToString)
            '  db = Session.GetDatabase(ServerName, NSFfileName, False)
        End Try

        Try
            If db Is Nothing Then
                db = NotesSession.GetDatabase(ServerName, NSFfileName, False)
                WriteAuditEntry(Now.ToString & " Successfully connected to the database.")
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Could not connect to the database. " & ex.ToString)
            '  db = Session.GetDatabase(ServerName, NSFfileName, False)
        End Try

        Try
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " The VitalStatus database is: " & db.Title)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error connecting to VitalStatus database " & ex.ToString)
            GoTo CleanUp
        End Try


        Try
            If Not db Is Nothing Then
                viewDiskSpace = db.GetView("(DiskSpaceKey)")
                viewClusterHealth = db.GetView("ClusterHealthKey")
                viewByKey = db.GetView("(ByKey)")
                viewDeviceID = db.GetView("(DeviceID)")
            End If
        Catch ex As Exception

        End Try

        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Cleaning up all disk drives documents...  ")
        Try
            Dim coll As Domino.NotesDocumentCollection
            doc = viewDiskSpace.GetFirstDocument

            While Not doc Is Nothing
                coll.AddDocument(doc)
                doc = viewDiskSpace.GetNextDocument(doc)
            End While
            coll.RemoveAll(True)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(coll)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception cleaning up  drives...  " & ex.ToString)
        End Try

        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Cleaning up all cluster documents...  ")
        Try
            Dim coll As Domino.NotesDocumentCollection
            doc = viewClusterHealth.GetFirstDocument

            While Not doc Is Nothing
                coll.AddDocument(doc)
                doc = viewClusterHealth.GetNextDocument(doc)
            End While
            coll.RemoveAll(True)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(coll)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception cleaning up  drives...  " & ex.ToString)
        End Try

        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Cleaning up all Traveler Device documents...  ")
        Try
            Dim coll As Domino.NotesDocumentCollection
            doc = viewDeviceID.GetFirstDocument

            While Not doc Is Nothing
                coll.AddDocument(doc)
                doc = viewDeviceID.GetNextDocument(doc)
            End While
            coll.RemoveAll(True)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(coll)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception cleaning up  drives...  " & ex.ToString)
        End Try



CleanUp:

        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(doc)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(viewClusterHealth)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(viewDiskSpace)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(viewDeviceID)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(viewByKey)
         
        Catch ex As Exception

        End Try


        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus module finished emptying " & NSFfileName & " on " & ServerName)
        GC.Collect()
    End Sub

    Public Sub VitalStatus_TravelerUsers()
        Dim myRegistry As New RegistryHandler

        Try
            If myRegistry.ReadFromRegistry("Notes Output") = "False" Then
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus database output is not enabled.")
                Exit Sub
            End If
        Catch ex As Exception

        End Try

        Try
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Starting VitalStatus Traveler User output.")
        Catch ex As Exception

        End Try

        'Dim strServerName As String
        Dim myKey As String

        'All data objects
        Dim drv As DataRowView
        Dim dsTraveler_Devices As New Data.DataSet
        Dim dtTraveler_Devices As New Data.DataTable("Traveler_Devices")

        'All Domino objects declared here
        ' Dim Session As New Domino.NotesSession
        Dim db As Domino.NotesDatabase
        Dim doc As Domino.NotesDocument
        Dim docPrevious As Domino.NotesDocument

        Dim coll As Domino.NotesDocumentCollection
        Dim viewDeviceID As Domino.NotesView


        Dim ServerName, NSFfileName As String
        Try
            ServerName = Trim(myRegistry.ReadFromRegistry("Notes Output Server"))
        Catch ex As Exception
            ServerName = "Boston"
        End Try

        Try
            NSFfileName = Trim(myRegistry.ReadFromRegistry("Notes Output Database"))
        Catch ex As Exception
            NSFfileName = "VitalStatus.NSF"
        End Try

        Try
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus is attempting to connect to the database " & NSFfileName & " on " & ServerName & ".")
        Catch ex As Exception

        End Try

        Try
            db = NotesSession.GetDatabase(ServerName, NSFfileName, False)
            WriteAuditEntry(Now.ToString & " Successfully connected to the database.")
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Could not connect to the database. " & ex.ToString)
            '  db = Session.GetDatabase(ServerName, NSFfileName, False)
        End Try

        Try
            If db Is Nothing Then
                db = NotesSession.GetDatabase(ServerName, NSFfileName, False)
                WriteAuditEntry(Now.ToString & " Successfully connected to the database.")
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Could not connect to the database. " & ex.ToString)
        End Try

        Try
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " The VitalStatus database is: " & db.Title)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error connecting to VitalStatus database " & ex.ToString)
            GoTo CleanUp
        End Try


        Try
            If Not db Is Nothing Then
                viewDeviceID = db.GetView("(DeviceID)")
            End If
            If Not viewDeviceID Is Nothing Then
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Successfully connected to the DeviceID view.")
            Else
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Could not connect to the DeviceID view.")
            End If
        Catch ex As Exception

        End Try


        Dim strSQL_Traveler As String = "SELECT UserName, DeviceName, OS_Type FROM Traveler_Devices"
        strSQL_Traveler = "SELECT UserName, DeviceName, wipeSupported, ConnectionState, LastSyncTime, OS_Type, NotificationType, Client_Build, wipeRequested, wipeOptions, wipeStatus, Access, Security_Policy, SyncType, device_type, DocID, ServerName, Approval, DeviceID FROM Traveler_Devices"

        Try
            dsTraveler_Devices.Clear()
            dsTraveler_Devices.Tables.Add(dtTraveler_Devices)
        Catch ex As Exception

        End Try

        Try
            Dim objVSAdaptor As New VSAdaptor
            objVSAdaptor.FillDatasetAny("VitalSigns", "Status", strSQL_Traveler, dsTraveler_Devices, "Traveler_Devices")
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Exception in Notes Output module: " & ex.Message)
            If InStr(ex.ToString, "System resource exceeded") > 0 Then
                Try
                    WriteAuditEntry(Now.ToString & " System resources have been exceeded.  Shutting down to free resources.")
                    WriteAuditEntry(Now.ToString & " The VitalSigns Master Service should detect this and restart monitoring.")
                    Threading.Thread.Sleep(1000)
                Catch ex2 As Exception

                Finally
                    'Terminate the program
                    End
                End Try
            End If

            GoTo CleanUp
        End Try

        Try
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " I have to process " & dsTraveler_Devices.Tables("Traveler_Devices").Rows.Count & "  Traveler device documents.")
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error with dataset " & ex.ToString)
        End Try

        'Update Traveler Devices
        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Attempting to produce Traveler device documents.")
        Dim myDataView As New Data.DataView(dsTraveler_Devices.Tables("Traveler_Devices"), "DeviceName <> ''", "DeviceName", DataViewRowState.CurrentRows)
        myDataView.Sort = "DeviceName DESC"

        For Each drv In myDataView
            myKey = drv("DeviceID")

            Try
                coll = viewDeviceID.GetAllDocumentsByKey(myKey)
                If coll.Count > 1 Then
                    WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus found a duplicate device document for " & myKey)
                    doc = coll.GetLastDocument
                    doc.Remove(True)
                    doc = coll.GetFirstDocument
                End If

            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus module error processing duplicate documents for " & myKey)
                '     doc = Nothing
            End Try

            Try
                coll = viewDeviceID.GetAllDocumentsByKey(myKey)
                If coll.Count > 1 Then
                    doc = coll.GetLastDocument
                    doc.Remove(True)
                    doc = coll.GetFirstDocument
                End If

            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus module error processing duplicate documents for " & myKey)
            End Try

            Try
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus is processing device for " & drv("UserName") & " with ID " & myKey)
            Catch ex As Exception

            End Try

            Try
                doc = viewDeviceID.GetDocumentByKey(myKey)
            Catch ex As Exception
                doc = Nothing
            End Try


            If doc Is Nothing Then
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus is creating a device document for " & myKey)
                doc = db.CreateDocument
                doc.ReplaceItemValue("Form", "TravelerDevice")
                doc.ReplaceItemValue("DeviceID", myKey)
                doc.Save(True, False)
            Else
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus found a device document for " & myKey)
            End If


            Try
                doc.ReplaceItemValue("UserName", Trim(drv("UserName")))
                doc.ReplaceItemValue("DeviceName", Trim(drv("DeviceName")))
                doc.ReplaceItemValue("device_type", Trim(drv("OS_Type")))
                doc.ReplaceItemValue("TravelerServer", Trim(drv("ServerName")))
                doc.ReplaceItemValue("wipeSupported", Trim(drv("wipeSupported")))
                doc.ReplaceItemValue("ConnectionState", Trim(drv("ConnectionState")))
                doc.ReplaceItemValue("LastSyncTime", Trim(drv("LastSyncTime")))
                doc.ReplaceItemValue("OS_Type", Trim(drv("OS_Type")))

            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error #1 populating device document with fields for " & myKey)
            End Try

            Try
                If drv("NotificationType") <> "" Then
                    doc.ReplaceItemValue("NotificationType", Trim(drv("NotificationType")))
                End If
            Catch ex As Exception
                '  WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error #2 populating device document with fields for " & myKey & " " & ex.ToString)
            End Try

            Try
                If drv("Client_Build") <> "" Then
                    doc.ReplaceItemValue("Client_Build", Trim(drv("Client_Build")))
                End If
            Catch ex As Exception
                '    WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error #2 populating device document with fields for " & myKey & " " & ex.ToString)
            End Try

            Try
                If drv("wipeRequested") <> "" Then
                    doc.ReplaceItemValue("wipeRequested", Trim(drv("wipeRequested")))
                End If

            Catch ex As Exception
                '   WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error #2 populating device document with fields for " & myKey & " " & ex.ToString)
            End Try

            Try
                If drv("Access") <> "" Then
                    doc.ReplaceItemValue("Access", Trim(drv("Access")))
                End If
            Catch ex As Exception

            End Try

            Try
                doc.ReplaceItemValue("device_type", Trim(drv("OS_Type")))
            Catch ex As Exception

            End Try

            Try
                doc.ReplaceItemValue("device_provider", Trim(drv("DeviceName")))
            Catch ex As Exception

            End Try

            Try
                doc.ReplaceItemValue("wipeOptions", Trim(drv("wipeOptions")))
            Catch ex As Exception

            End Try

            Try
                doc.ReplaceItemValue("wipeStatus", Trim(drv("wipeStatus")))
            Catch ex As Exception

            End Try
            Try
                doc.ReplaceItemValue("Security_Policy", Trim(drv("Security_Policy")))
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error #3 populating device document with fields for " & myKey & " " & ex.ToString)
            End Try

            Try
                doc.ReplaceItemValue("DocID", Trim(drv("DocID")))
                doc.ReplaceItemValue("Approval", Trim(drv("Approval")))
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " Error #4  populating device document with fields for " & myKey & " " & ex.ToString)
            End Try

            Try
                doc.Save(True, False)
            Catch ex As Exception

            End Try

        Next



CleanUp:

        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(doc)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(docPrevious)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(coll)
            '     System.Runtime.InteropServices.Marshal.ReleaseComObject(Session)
        Catch ex As Exception

        End Try

        WriteDeviceHistoryEntry("All", "VitalStatus", Now.ToString & " VitalStatus module finished updating " & NSFfileName & " on " & ServerName)
        GC.Collect()
    End Sub


#End Region

End Class