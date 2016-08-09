Imports System.Threading
Imports VSFramework
Imports VSNext.Mongo.Entities

Partial Public Class VitalSignsPlusDomino

#Region "Create Collections of items to Monitor"

    Public Sub CreateCollections()

        Try
            CreateDominoServersCollection()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Domino Servers collection: " & ex.Message, LogLevel.Debug)
        End Try

        Try
            CreateTravelerBackEndCollection()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Traveler backend collection: " & ex.Message)
        End Try

        Try
			CreateKeywordsCollection()
        Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Error creating Domino Servers keywords collection: " & ex.Message)
        End Try

        Try
            CreateDominoClusterCollection()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Domino cluster collection: " & ex.Message)
        End Try

        'Try
        '    CreateURLCollection()

        'Catch ex As Exception
        '    WriteAuditEntry(Now.ToString & " Error creating URL collection: " & ex.Message)
        'End Try

        Try
            CreateNotesMailProbeCollection()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating NotesMail Probe collection: " & ex.Message)
        End Try


        Try
            CreateNotesDatabaseCollection()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Notes Database collection: " & ex.Message)
        End Try

    End Sub

    Private Sub CreateNotesDatabaseCollection()
        'start with fresh data

        'Connect to the data source
        Dim dsNotesDatabases As New Data.DataSet

        WriteAuditEntry(vbCrLf & Now.ToString & " Creating a dataset in CreateNotesDatabaseCollection." & vbCrLf)
        Try

            Dim strSQL As String
            'Dim strSQL As String = " SELECT Category, Enabled, FileName, ID, Name, OffHoursScanInterval, ResponseThreshold, RetryInterval, ScanInterval, ServerName, TriggerType, TriggerValue, AboveBelow, ReplicationDestination, InitiateReplication FROM NotesDatabases"

			strSQL = " Select ds.ID, srv.ServerName AS ServerName, ds.Name, ds.Category, loc.Location, ScanInterval, Enabled, OffHoursScanInterval, ds.ResponseThreshold, RetryInterval, FileName, TriggerType, "
			strSQL += " TriggerValue, AboveBelow, ReplicationDestination, InitiateReplication, st.LastUpdate, st.Status, st.StatusCode, di.CurrentNodeID "
			strSQL += " FROM "
			strSQL += " dbo.NotesDatabases ds "
			strSQL += " inner join dbo.Servers srv on ds.ServerId=srv.ID "
			strSQL += " inner join dbo.Locations Loc  on srv.LocationId=Loc.Id "
			If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then

				Dim NodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()

				strSQL += " inner join DeviceInventory di on ds.ID=di.DeviceID and di.DeviceTypeId=9  "
				strSQL += " inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" & NodeName & "' "
			End If

			strSQL += " left outer join Status st on st.Type=(select ServerType From ServerTypes Where ID=9) and st.Name=ds.Name  "

			strSQL += " WHERE ds.Enabled = 1 "





			WriteAuditEntry(Now.ToString & " SQL statement is: " & vbCrLf & strSQL & vbCrLf)
			Dim objVSAdaptor As New VSAdaptor
			objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsNotesDatabases, "NotesDatabases")

		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateNotesDatabaseCollection processing code. Exception: " & ex.Message)
			Exit Sub
		End Try


        '***
        'but first delete any that are in the collection but not in the database anymore
        'Delete propagation
        Dim myDataRow As System.Data.DataRow
        Dim MyServerNames As String = ""
        If MyNotesDatabases.Count > 0 Then
            WriteAuditEntry(Now.ToString & " Checking to see if any Notes databases should be deleted. ")
            'Get all the names of all the servers in the data table
            For Each myDataRow In dsNotesDatabases.Tables("NotesDatabases").Rows()
                MyServerNames += myDataRow.Item("Name") & "  "
            Next
        End If

        Dim NDB As MonitoredItems.NotesDatabase
        Dim myIndex As Integer

        If MyNotesDatabases.Count > 0 Then
            For myIndex = MyNotesDatabases.Count - 1 To 0 Step -1
                NDB = MyNotesDatabases.Item(myIndex)
                Try
                    WriteAuditEntry(Now.ToString & " Checking to see if Notes database " & NDB.Name & " should be deleted...")
                    If InStr(MyServerNames, NDB.Name) > 0 Then
                        'the server has not been deleted
                        WriteAuditEntry(Now.ToString & " " & NDB.Name & " is not marked for deletion. ")
                    Else
                        'the server has been deleted, so delete from the collection
                        Try
                            MyNotesDatabases.Delete(NDB.Name)
                            WriteAuditEntry(Now.ToString & " " & NDB.Name & " has been deleted by the service.")
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " " & NDB.Name & " was not deleted by the service because " & ex.Message)
                        End Try
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception Notes Databases Deletion Loop on " & NDB.Name & ".  The error was " & ex.Message)
                End Try
            Next
        End If

        MyServerNames = Nothing
        NDB = Nothing
        myIndex = Nothing

        '*** End delete propagation

        'Add / Update Notes databases

        Dim i As Integer = 0
		WriteAuditEntry(Now.ToString & "  Reading configuration settings for Notes Databases.", LogLevel.Verbose)
        'Add the network Devices to the collection

        Try
            Dim myString As String = ""
            Dim dr As DataRow
            For Each dr In dsNotesDatabases.Tables("NotesDatabases").Rows
                i += 1
                Dim MyName As String
                MyName = dr.Item("Name")

                If InStr(MyName, "'") > 0 Then
                    MyName = MyName.Replace("'", "")
                End If

                Dim Quote As Char
                Quote = Chr(34)

                If InStr(MyName, Quote) > 0 Then
                    MyName = MyName.Replace(Quote, "~")
                End If

                If MyName Is Nothing Then
                    MyName = "Notes Database" & i.ToString
                End If

                'See if this server is already in the collection; if so, update its settings otherwise create a new one
                MyNotesDatabase = MyNotesDatabases.Search(MyName)
                If MyNotesDatabase Is Nothing Then
                    MyNotesDatabase = New MonitoredItems.NotesDatabase
                    MyNotesDatabase.Name = MyName
                    MyNotesDatabase.LastScan = Now
                    MyNotesDatabase.NextScan = Now
                    MyNotesDatabase.IncrementUpCount()
                    MyNotesDatabase.AlertCondition = False
                    MyNotesDatabase.Status = "Not Scanned"
                    MyNotesDatabases.Add(MyNotesDatabase)
                    WriteAuditEntry(Now.ToString & " Adding new Notes Database -- " & MyNotesDatabase.Name & " -- to the collection.", LogLevel.Verbose)
                Else
                    WriteAuditEntry(Now.ToString & " Updating settings for existing Notes Database-- " & MyNotesDatabase.Name & ".", LogLevel.Verbose)
                End If

                With MyNotesDatabase

                    Try
                        If dr.Item("FileName") Is Nothing Then
                            .FileName = ""
                            WriteAuditEntry(Now.ToString & " Error: No filename specified for " & .Name)
                        Else
                            .FileName = dr.Item("FileName")
                        End If
                    Catch ex As Exception
                        .FileName = ""
                        WriteAuditEntry(Now.ToString & " Error:  No filename specified for " & .Name)
                    End Try

                    Try
						.OffHours = OffHours(MyName)
                    Catch ex As Exception
                        .OffHours = False
                    End Try

                    Try
                        .Location = dr.Item("Location")
                        WriteAuditEntry(Now.ToString & " The location for this Notes database is " & .Location)
                    Catch ex As Exception
                        .Location = "Unknown"
                    End Try


                    Try
                        If dr.Item("ServerName") Is Nothing Then
                            .ServerName = ""
                            WriteAuditEntry(Now.ToString & " Error: No Server Name specified for " & .Name)
                        Else
                            .ServerName = dr.Item("ServerName")
                        End If
                    Catch ex As Exception
                        .ServerName = ""
                        .Enabled = False
                        WriteAuditEntry(Now.ToString & " Error:  No Server Name specified for " & .Name)
                    End Try


                    Try
                        If dr.Item("AboveBelow") Is Nothing Then
                            .AboveBelow = "Above"
                            WriteAuditEntry(Now.ToString & " Note: Whether to alert above or below the threshold value not specified for " & .Name & ", 'above' assumed.")
                        Else
                            .AboveBelow = dr.Item("AboveBelow")
                        End If
                    Catch ex As Exception
                        .AboveBelow = "Above"
                        'WriteAuditEntry(Now.ToString & " Error: Above or Below threshold value not specified for " & .Name & ", 'above' assumed.  Error: " & ex.ToString)
                    End Try

                    Try
                        If dr.Item("TriggerType") Is Nothing Then
                            .TriggerType = "Database Disappearance"
                            WriteAuditEntry(Now.ToString & " Error: No trigger type specified for " & .Name)
                        Else
                            .TriggerType = dr.Item("TriggerType")
                        End If
                    Catch ex As Exception
                        .TriggerType = "Database Disappearance"
                        WriteAuditEntry(Now.ToString & " Error:  No trigger type specified for " & .Name & " because " & ex.Message)
                    End Try

                    'WriteAuditEntry(Now.ToString & " This Notes database uses trigger: " & dr.Item("TriggerType"))

                    Dim myTriggerThreshold As Long
                    Try
                        If dr.Item("TriggerValue") Is Nothing Then
                            WriteAuditEntry(Now.ToString & " Error: No trigger threshold specified for " & .Name)
                        Else
                            myTriggerThreshold = dr.Item("TriggerValue")
                            WriteAuditEntry(Now.ToString & " This Notes database uses trigger: " & .TriggerType)
                            Select Case .TriggerType
                                Case "Database Size"
                                    .DatabaseSizeTrigger = myTriggerThreshold
                                    .Description = "Compares the size of Notes database " & .FileName & " with threshold of " & .DatabaseSizeTrigger & " MB"
                                Case "Document Count"
                                    .DocumentCountTrigger = myTriggerThreshold
                                    .Description = "Checks to see if more than " & .DocumentCountTrigger & " documents are in the database."
                                Case "Database Response Time"
                                    .ResponseThreshold = myTriggerThreshold
                                    .Description = "Opens the database " & .FileName & " default view and compares response time to threshold of " & .ResponseThreshold & " ms."
                                Case "Database Disappearance"
                                    .Description = "Verifies that the database exists."
                                Case "Refresh All Views"
                                    .Description = "Periodically refreshes all the view indexes."
                                Case "Replication"
                                    .ResponseThreshold = 0
                                    .Description = "Verifies that this database replicates properly."

                                    'Read the keys of the servers it is supposed to replicate with

                                    Try
                                        If dr.Item("ReplicationDestination") Is Nothing Then
                                            .ReplicationDestination = ""
                                            WriteAuditEntry(Now.ToString & " Note: Replication Destination value not specified for " & .Name)
                                        Else
                                            .ReplicationDestination = dr.Item("ReplicationDestination")
                                            ' WriteAuditEntry(Now.ToString & " Note: Replication Destination value is " & .ReplicationDestination)
                                        End If
                                    Catch ex As Exception
                                        .ReplicationDestination = ""
                                        WriteAuditEntry(Now.ToString & " Error: Replication Destination value not specified for " & .Name & ", 'above' assumed.")
                                    End Try

                                    'InitiateReplication
                                    Try
                                        If dr.Item("InitiateReplication") Is Nothing Then
                                            .InitiateReplication = False
                                            WriteAuditEntry(Now.ToString & " Note: Initiate Replication value not specified for " & .Name)
                                        Else
                                            .InitiateReplication = dr.Item("InitiateReplication")
                                            WriteAuditEntry(Now.ToString & " Note: Replication Destination value is " & .ReplicationDestination)
                                        End If
                                    Catch ex As Exception
                                        .InitiateReplication = False
                                        WriteAuditEntry(Now.ToString & " Error: Initiate Replication value not specified for " & .Name & ", 'False' assumed.")
                                    End Try

                                    'Build a collection of servers
                                    Dim myCollection As New MonitoredItems.DominoCollection
                                    Dim myServerKeys As String
                                    myServerKeys = .ReplicationDestination
                                    'Explode the keys into an array, and use each key to locate the appropriate server
                                    Try

                                        Dim x As Integer
                                        Dim words() As String

                                        ' Split the string at the space characters.
                                        words = Split(.ReplicationDestination)
                                        Dim DominoServer As MonitoredItems.DominoServer
                                        Dim tempStr As String
                                        For x = 0 To UBound(words)
                                            ' Eliminate punctuation
                                            tempStr = Replace(words(x), ".", "")
                                            tempStr = Replace(tempStr, ",", "")
                                            If Len(tempStr) > 0 Then
                                                Try
                                                    DominoServer = MyDominoServers.Search(tempStr)
                                                    If Not DominoServer Is Nothing And DominoServer.Name <> .ServerName Then
                                                        myCollection.Add(DominoServer)
                                                        WriteAuditEntry(Now.ToString & " Adding " & DominoServer.Name & " as a server replication target for " & .Name)
                                                    End If
                                                Catch ex As Exception
                                                    WriteAuditEntry(Now.ToString & " Error adding server to collection: tempStr is " & tempStr & "  " & ex.ToString)
                                                End Try

                                            End If
                                        Next
                                        Try
                                            .ReplicationServers = myCollection
                                        Catch ex As Exception
                                            WriteAuditEntry(Now.ToString & " Error setting collection as replication destination: " & ex.ToString)
                                        End Try

                                        myCollection = Nothing
                                    Catch ex As Exception
                                        WriteAuditEntry(Now.ToString & " Error:  Could not set create destination servers for Notes database:  " & .Name & " because " & ex.Message)
                                    End Try


                                Case Else
                                    WriteAuditEntry(Now.ToString & " Error:  No matching trigger category specified for " & .Name)
                            End Select
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error:  Could not set trigger threshold for Notes database:  " & .Name & " because " & ex.Message)
                    End Try

                    myTriggerThreshold = Nothing

                    Try
                        If dr.Item("Enabled") Is Nothing Then
                            .Enabled = True
                        Else
                            .Enabled = dr.Item("Enabled")
                        End If

                    Catch ex As Exception
                        .Enabled = True
                    End Try

                    If .Enabled = False Then
                        .Status = "Disabled"
                    End If

                    If .Enabled = True And .Status = "Disabled" Then
                        .Status = "Not Scanned"
                    End If

					Try
						If dr.Item("ResponseThreshold") Is Nothing Then
							.ResponseThreshold = 100
						Else
							.ResponseThreshold = dr.Item("ResponseThreshold")
						End If
					Catch ex As Exception
						.ResponseThreshold = 100
					End Try


                    Try
                        If dr.Item("ScanInterval") Is Nothing Then
                            .ScanInterval = 10
                        Else
                            .ScanInterval = dr.Item("ScanInterval")
                        End If
                    Catch ex As Exception
                        .ScanInterval = 10
                    End Try
                    WriteAuditEntry(Now.ToString & " Scan Interval is " & .ScanInterval)

                    Try
                        If dr.Item("OffHoursScanInterval") Is Nothing Then
                            .OffHoursScanInterval = 30
                        Else
                            .OffHoursScanInterval = dr.Item("OffHoursScanInterval")
                        End If
                    Catch ex As Exception
                        .OffHoursScanInterval = 30
                    End Try
                    Try
                        '   WriteAuditEntry(Now.ToString & "Adding Category")
                        If dr.Item("Category") Is Nothing Then
                            .Category = "Not Categorized"
                        Else
                            .Category = dr.Item("Category")
                        End If
                    Catch ex As Exception
                        .Category = "Not Categorized"
                    End Try



                    Try
                        If dr.Item("RetryInterval") Is Nothing Then
                            .RetryInterval = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " Notes Database retry scan interval not set, using default of 10 minutes.")
                        Else
                            .RetryInterval = dr.Item("RetryInterval")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Notes Database retry scan interval not set, using default of 10 minutes.")
                        .RetryInterval = 2
					End Try

					Try
						If dr.Item("Status") Is Nothing Then
							.Status = "Not Scanned"
						Else
							.Status = dr.Item("Status")
						End If
					Catch ex As Exception
						.Status = "Not Scanned"
					End Try

					Try
						If dr.Item("LastUpdate") Is Nothing Then
							.LastScan = Now
						Else
							.LastScan = dr.Item("LastUpdate")
						End If
					Catch ex As Exception
						.LastScan = Now
					End Try

					Try
						If dr.Item("CurrentNodeID") Is Nothing Then
							.InsufficentLicenses = True
						Else
							If dr.Item("CurrentNodeID").ToString() = "-1" Then
								.InsufficentLicenses = True
							Else
								.InsufficentLicenses = False
							End If

						End If
                    Catch ex As Exception
                        '7/8/2015 NS modified for VSPLUS-1959
                        WriteAuditEntry(Now.ToString & " " & .Name & " Notes Databases insufficient licenses not set.")

					End Try

                End With
                MyNotesDatabase = Nothing
            Next
            dr = Nothing

        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Notes Databases data exception " & exception.Message)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Notes Databases general exception " & ex.ToString)
		End Try

		InsufficentLicensesTest(MyNotesDatabases)

        dsNotesDatabases.Dispose()
    End Sub


    Private Sub CreateTravelerBackEndCollection()
        Dim dsTravelerDataStore As New Data.DataSet
        myTravelerBackends.Clear()

        Try
            dsTravelerDataStore.Tables.Add("Traveler_Backends")
            Dim strSQL As String

			strSQL = "SELECT  TravelerServicePoolName, ServerName ,DataStore, Port, UserName, Password, IntegratedSecurity, TestScanServer, UsedByServers,DatabaseName  "
			strSQL += " FROM vitalsigns.dbo.Traveler_HA_Datastore ds "

            WriteAuditEntry(vbCrLf & Now.ToString & " My Traveler Backend data store SQL statement is " & strSQL)
            Dim objVSAdaptor As New VSAdaptor
            objVSAdaptor.FillDatasetAny("VitalSigns", "Servers", strSQL, dsTravelerDataStore, "Traveler_Backends")

            WriteAuditEntry(Now.ToString & " Created a Domino servers dataset with " & dsTravelerDataStore.Tables(0).Rows.Count & " records found.")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateTravelerBackEndCollection processing code. Exception: " & ex.Message)
            Exit Sub
        End Try

        Dim dr As DataRow

        Try
            For Each dr In dsTravelerDataStore.Tables("Traveler_Backends").Rows
                Dim myTravelerBackend As New MonitoredItems.Traveler_Backend
                With myTravelerBackend
                    .TravelerServicePoolName = dr.Item("TravelerServicePoolName")
                    .ServerName = dr.Item("ServerName")
                    .DataStore = dr.Item("DataStore")
                    .Port = dr.Item("Port")
                    .UserName = dr.Item("UserName")
                    .Password = dr.Item("Password")
                    .IntegratedSecurity = dr.Item("IntegratedSecurity")
                    .TestScanServer = dr.Item("TestScanServer")
                    .UsedByServers = dr.Item("UsedByServers")
                    .DatabaseName = dr.Item("DatabaseName")
                    WriteAuditEntry(Now.ToString & " Adding " & .TravelerServicePoolName)
                End With

                myTravelerBackends.Add(myTravelerBackend)
            Next
        Catch ex As Exception

        End Try


    End Sub

    Private Sub CreateDominoServersCollection()

        'start with fresh data
        '***  Build the data set dynamically
        Dim dsDominoServers As New Data.DataSet
        Dim MyDominoServer As MonitoredItems.DominoServer

        Try
            dsDominoServers.Tables.Add("DominoServers")
            Dim strSQL As String

            '2/22/2016 NS modified for VSPLUS-2641
			strSQL = " SELECT srv.ID, ServerName AS Name, [Scan Interval], ds.Category, Enabled, RequireSSL,ExternalAlias,ScanTravelerServer,ds.PendingThreshold, ds.DeadThreshold, LOC.Location, Loc.ID as LocationID, ServerType, "
			strSQL += " MailDirectory, OffHoursScanInterval, RetryInterval, ds.ServerID AS [Key], ds.ResponseThreshold, srv.IPAddress, BES_Server, BES_Threshold, FailureThreshold,  "
            strSQL += " SearchString, DeadMailDeleteThreshold, DiskSpaceThreshold, HeldThreshold, ScanDBHealth, CheckMailThreshold, scanlog, scanagentlog, srv.Description, NotificationGroup, Memory_Threshold, "
            strSQL += " CPU_Threshold, Cluster_Rep_Delays_Threshold, ServerDaysAlert, AliasName, SendRouterRestart, st.LastUpdate, st.Status, st.StatusCode, ds.ScanServlet, di.CurrentNodeID, ds.AvailabilityIndexThreshold  "
			strSQL += " FROM dbo.DominoServers ds  "
			strSQL += " LEFT OUTER JOIN  Credentials c on ds.CredentialsID=c.ID "
			strSQL += " inner join Servers srv on srv.ID = ds.ServerID "
			strSQL += " inner join ServerTypes srvt on srvt.id=srv.ServerTypeId "
			strSQL += " inner join Locations Loc on srv.LocationId = Loc.ID "
			If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then
				Dim NodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
				strSQL += " inner join DeviceInventory di on srv.ID=di.DeviceID and di.DeviceTypeID=srv.ServerTypeId "
				strSQL += " inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" & NodeName & "'   "
			End If
			strSQL += " left outer join Status st on st.Type=(select ServerType From ServerTypes where Id=srv.ServerTypeId) and st.Name=srv.ServerName   "
			strSQL += " WHERE(ds.Enabled = 1) "

			WriteAuditEntry(vbCrLf & Now.ToString & " My Domino servers SQL statement is " & strSQL)
			Dim objVSAdaptor As New VSAdaptor
			objVSAdaptor.FillDatasetAny("VitalSigns", "Servers", strSQL, dsDominoServers, "DominoServers")

			WriteAuditEntry(Now.ToString & " Created a Domino servers dataset with " & dsDominoServers.Tables(0).Rows.Count & " records found.")
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateDominoServersCollection processing code. Exception: " & ex.Message)
			Exit Sub
		End Try



        Dim myDataRow As System.Data.DataRow
        Dim MyServerNames As String = ""
        Try
            WriteAuditEntry(Now.ToString & " Checking to see if any Domino servers have been deleted. ", LogLevel.Verbose)
            If MyDominoServers.Count > 0 Then

                'Get all the names of all the servers in the data table
                For Each myDataRow In dsDominoServers.Tables("DominoServers").Rows()
                    WriteAuditEntry(Now.ToString & " I have " & myDataRow.Item("Name"), LogLevel.Verbose)
                    MyServerNames += myDataRow.Item("Name") & "  "
                Next
            End If
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Exception at #1 CreateDominoServersCollection processing code. Exception: " & ex.Message)
        End Try
        '***
        'but first delete any that are in the collection but not in the database anymore
        'Delete propagation


        Dim Dom As MonitoredItems.DominoServer
        Dim myIndex As Integer

        If MyDominoServers.Count > 0 Then
            For myIndex = MyDominoServers.Count - 1 To 0 Step -1
                Dom = MyDominoServers.Item(myIndex)
                Try
                    '    WriteAuditEntry(Now.ToString & " Checking to see if Domino server " & Dom.Name & " should be deleted...")
                    If InStr(MyServerNames, Dom.Name) > 0 Then
                        'the server has not been deleted
                        WriteAuditEntry(Now.ToString & " " & Dom.Name & " is not marked for deletion. ", LogLevel.Verbose)
                    Else
                        'the server has been deleted, so delete from the collection
                        Try
                            MyDominoServers.Delete(Dom.Name)
                            WriteAuditEntry(Now.ToString & " " & Dom.Name & " has been deleted by the service.", LogLevel.Verbose)
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " " & Dom.Name & " was not deleted by the service because " & ex.Message)
                        End Try
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception Domino Servers Deletion Loop on " & Dom.Name & ".  The error was " & ex.Message)
                End Try
            Next
        End If

        Dom = Nothing
        MyServerNames = Nothing
        myIndex = Nothing


        'Now Add the Domino servers to the collection, if required
        Dim i As Integer = 0
        Dim MyName As String
        Dim Description As String
        WriteAuditEntry(Now.ToString & " Reading configuration settings for Domino Servers.", LogLevel.Verbose)

        Try
            Dim dr As DataRow
            For Each dr In dsDominoServers.Tables("DominoServers").Rows
                i += 1
                If dr.Item("Description") Is Nothing Then
                    Description = "Lotus Domino Server"
                Else
                    Description = dr.Item("Description")
                End If

                If dr.Item("Name") Is Nothing Then
                    MyName = "DominoServer" & i.ToString
                Else
                    MyName = dr.Item("Name")
                End If



                'See if this server is already in the collection; if so, update its settings otherwise create a new one
                MyDominoServer = MyDominoServers.Search(MyName)
                If MyDominoServer Is Nothing Then
                    'this server is new
                    MyDominoServer = New MonitoredItems.DominoServer(MyName, Description)
                    MyDominoServer.Traveler_Server = False
                    MyDominoServers.Add(MyDominoServer)
                    Dim tNow As DateTime
                    tNow = Now
                    MyDominoServer.IPAddress = ""
                    MyDominoServer.ScanCount = 0
                    MyDominoServer.PingCount = 0
                    ' MyDominoServer.LastPing = Now
                    MyDominoServer.SecondaryRole = ""
                    'Default values for OS and Version
                    MyDominoServer.OperatingSystem = "Unknown"
                    MyDominoServer.VersionDomino = "Unknown"
                    MyDominoServer.ResponseDetails = "This server has not yet been scanned."
                    MyDominoServer.EXJournal1_DocCount = 0
                    MyDominoServer.EXJournal2_DocCount = 0
                    MyDominoServer.EXJournal_DocCount = 0
                    MyDominoServer.Traveler_Server = False
                    'Default Values for tracking mail stats
                    ' MyDominoServer.PreviousDeliveredMail = -1
                    ' MyDominoServer.PreviousRoutedMail = -1
                    ' MyDominoServer.PreviousTransferredMail = -1
                    ' MyDominoServer.PreviousSMTPMessages = -1
                    '  MyDominoServer.PreviousMailFailures = -1

                    'For tracking Domino.Command values
                    MyDominoServer.priorDominoOpenDocument = -1
                    MyDominoServer.priorDominoOpenDatabase = -1
                    MyDominoServer.priorDominoOpenView = -1
                    MyDominoServer.priorDominoDeleteDocument = -1
                    MyDominoServer.priorDominoCreateDocument = -1
                    MyDominoServer.priorDominoTotal = -1

                    '   MyDominoServer.TransferredMail = 0
                    '   MyDominoServer.DeliveredMail = 0
                    '   MyDominoServer.RoutedMail = 0

                    'for tracking cluster stats
                    MyDominoServer.ClusterOpenRedirects_Previous_LoadBalanceSuccessful = -1
                    MyDominoServer.ClusterOpenRedirects_Previous_LoadBalanceUnSuccessful = -1
                    MyDominoServer.ClusterOpenRedirects_Previous_FailoverUnSuccessful = -1
                    MyDominoServer.ClusterOpenRedirects_Previous_FailoverSuccessful = 1
					MyDominoServer.OffHours = OffHours(MyName)
					MyDominoServer.AlertCondition = False
					'MyDominoServer.Status = "Not Scanned"
					'MyDominoServer.LastScan = Date.Now
                    MyDominoServer.IncrementUpCount()
                    MyDominoServer.MailboxCount = 999  'this flag tells app to check for mailbox count
                    MyDominoServer.UserCount = 0
                    MyDominoServer.Statistics_Server = ""
                    MyDominoServer.Statistics_Disk = ""
                    MyDominoServer.ServerType = VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription()

                    Try
                        Dim myDiskCollection As New MonitoredItems.DominoDiskSpaceCollection
                        MyDominoServer.DiskDrives = myDiskCollection
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error adding new empty drive collection to " & MyDominoServer.Name)
                    End Try


                    Try
                        Dim myCustomStatCollection As New MonitoredItems.DominoStatisticsCollection
                        MyDominoServer.CustomStatisticsSettings = myCustomStatCollection

                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error adding new empty stats collection to " & MyDominoServer.Name)
                    End Try
                Else
                    '      WriteAuditEntry(Now.ToString & " " & MyDominoServer.Name & " is already in the collection, updating settings.")
                End If

                With MyDominoServer
                    WriteAuditEntry(Now.ToString & " Configuring Domino Server: " & dr.Item("Name"), LogLevel.Verbose)

                    'Get the HTTP username and password, if any
                    Try
                        WriteAuditEntry(Now.ToString & " Getting server credentials.", LogLevel.Verbose)
                        If dr.Item("AliasName") Is Nothing Then
                            .HTTP_UserName = ""
                            .HTTP_Password = ""
                        Else
                            'Run a query here, then parse the results
                            Dim strSQL As String = "select UserID from dbo.Credentials where AliasName = '" & dr.Item("AliasName") & "' "
                            Dim objVSAdaptor As New VSAdaptor

                            .HTTP_UserName = objVSAdaptor.ExecuteScalarAny("VitalSigns", "Servers", strSQL)
                            WriteAuditEntry(Now.ToString & " HTTP Username is " & .HTTP_UserName, LogLevel.Verbose)

                            strSQL = "select  Password from dbo.Credentials where AliasName = '" & dr.Item("AliasName") & "' "
                            'Get the passwords from the settings table
                            Dim strEncryptedPassword As String
                            Dim Password As String
                            Dim myPass As Byte()

                            Try
                                strEncryptedPassword = objVSAdaptor.ExecuteScalarAny("VitalSigns", "Servers", strSQL)
                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " Exception retrieving encrypted HTTP password  " & ex.ToString, LogLevel.Verbose)
                                strEncryptedPassword = Nothing
                            End Try

                            Try
                                Dim strValue As Object
                                Dim str1() As String
                                str1 = strEncryptedPassword.Split(",")
                                Dim bstr1(str1.Length - 1) As Byte
                                For j As Integer = 0 To str1.Length - 1
                                    bstr1(j) = str1(j).ToString()
                                Next
                                myPass = bstr1
                            Catch ex As Exception

                            End Try


                            Dim mySecrets As New VSFramework.TripleDES
                            Try
                                If Not strEncryptedPassword Is Nothing Then
                                    Password = mySecrets.Decrypt(myPass) 'password in clear text, stored in memory now
                                    ' If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Successfully decrypted the Notes password as " & MyDominoPassword)
                                    ' WriteAuditEntry(Now.ToString & " HTTP password is " & Password, LogLevel.Verbose)
                                Else
                                    Password = Nothing
                                End If
                            Catch ex As Exception
                                Password = ""
                                WriteAuditEntry(Now.ToString & " Error decrypting the Notes password.  " & ex.ToString)
                            End Try
                            .HTTP_Password = Password
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("ServerDaysAlert") Is Nothing Then
                            .ServerDaysAlert = 0
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                        Else
                            If Trim(dr.Item("ServerDaysAlert")) <> "" Then
                                .ServerDaysAlert = dr.Item("ServerDaysAlert")
                                WriteAuditEntry(Now.ToString & " " & .Name & " ServerDaysAlert for this server is " & .ServerDaysAlert)
                            End If

                        End If
                    Catch ex As Exception
                        .ServerDaysAlert = 0
                        WriteAuditEntry(Now.ToString & " " & .Name & " Custom notification group not set for this server, using default alert settings.")
                    End Try


                    Try
                        If dr.Item("NotificationGroup") Is Nothing Then
                            .NotificationGroup = ""
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                        Else
                            .NotificationGroup = dr.Item("NotificationGroup")
                        End If
                    Catch ex As Exception
                        .NotificationGroup = ""
                        WriteAuditEntry(Now.ToString & " " & .Name & " Custom notification group not set for this server, using default alert settings.")
                    End Try

                    'CheckMailThreshold
                    'This value is set to 1 if we are supposed to stop counting once the pending or held thresholds are met
                    Try
                        If dr.Item("CheckMailThreshold") Is Nothing Then
                            .MailChecking = 0
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                        Else
                            .MailChecking = dr.Item("CheckMailThreshold")
                        End If
                    Catch ex As Exception
                        .MailChecking = 0
                        WriteAuditEntry(Now.ToString & " " & .Name & " CheckMailThreshold was set to zero in an exception.")
                    Finally
                        ' WriteAuditEntry(Now.ToString & " " & .Name & " Domino server CheckMailThreshold value is " & .MailChecking)
                    End Try

                    Try
                        If dr.Item("Cluster_Rep_Delays_Threshold") Is Nothing Then
                            .ClusterRep_Threshold = 240
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                        Else
                            .ClusterRep_Threshold = dr.Item("Cluster_Rep_Delays_Threshold")
                        End If
                    Catch ex As Exception
                        .ClusterRep_Threshold = 240
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Cluster Replication Delays threshold not set, using default of 240 seconds.")
                    End Try

                    Try
                        If dr.Item("CPU_Threshold") Is Nothing Then
                            .CPU_Threshold = 90
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                        Else
                            .CPU_Threshold = dr.Item("CPU_Threshold") * 100
                        End If
                    Catch ex As Exception
                        .CPU_Threshold = 90
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server CPU Utilization threshold not set, using default of 90%.")
                    End Try

                    Try
                        If dr.Item("Memory_Threshold") Is Nothing Then
                            .Memory_Threshold = 90
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                        Else
                            .Memory_Threshold = dr.Item("Memory_Threshold") * 100
                        End If
                    Catch ex As Exception
                        .Memory_Threshold = 90
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server memory usage threshold not set, using default of 90%.")
                    End Try


                    Try
                        If dr.Item("PendingThreshold") Is Nothing Then
                            .PendingThreshold = 50
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                        Else
                            .PendingThreshold = dr.Item("PendingThreshold")
                        End If
                    Catch ex As Exception
                        .PendingThreshold = 50
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                    End Try

                    Try
                        If dr.Item("DiskSpaceThreshold") Is Nothing Then
                            .DiskThreshold = 0.1  'Set to 10%
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server disk space threshold is the default value of 10%", LogLevel.Verbose)
                        Else
                            .DiskThreshold = dr.Item("DiskSpaceThreshold")
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server disk space threshold is " & .DiskThreshold * 100 & "%", LogLevel.Verbose)
                        End If
                    Catch ex As Exception
                        .DiskThreshold = 0.1  'Set to 10%
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server disk space threshold not set, using default of 10%.")
                    End Try



                    Try
                        If dr.Item("IPAddress") Is Nothing Then
                            .IPAddress = ""
                        Else
                            .IPAddress = dr.Item("IPAddress")
                        End If
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server IP Address is " & .IPAddress, LogLevel.Verbose)

                    Catch ex As Exception
                        .IPAddress = ""
                    End Try

                    Try
                        If .IPAddress.Trim = "" Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server does not have an IP or hostname defined.  I am going to try to figure it out.", LogLevel.Verbose)
                            .IPAddress = GetDominoServerHostName(.Name)
                            WriteAuditEntry(Now.ToString & " " & .Name & " I figure the host name is " & .IPAddress, LogLevel.Verbose)
                            If .IPAddress.Length > 4 Then
                                Dim vsObj As New VSAdaptor
                                Dim strSQL As String = "Update Servers SET IPAddress='" & .IPAddress & "' WHERE ServerName ='" & .Name & "' AND ServerTypeID =1"
                                vsObj.ExecuteNonQueryAny("VitalSigns", "Servers", strSQL)
                            End If
                        Else
                            WriteAuditEntry(Now.ToString & " " & .Name & " host name is " & .IPAddress, LogLevel.Verbose)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("Enabled") Is Nothing Then
                            .Enabled = True
                        Else
                            .Enabled = dr.Item("Enabled")
                        End If

                    Catch ex As Exception
                        .Enabled = True
                    End Try

                    If .Enabled = False Then
                        .Status = "Disabled"
                    End If

                    If .Enabled = True And .Status = "Disabled" Then
                        .Status = "Not Scanned"
                    End If

                    Try
                        If dr.Item("RequireSSL") Is Nothing Then
                            .RequireSSL = False
                        Else
                            .RequireSSL = Convert.ToBoolean(dr.Item("RequireSSL"))
                        End If

                    Catch ex As Exception
                        .RequireSSL = False
                    End Try

					Try
						If dr.Item("ScanTravelerServer") Is Nothing Then
							.ScanTravelerServer = True
						Else
							.ScanTravelerServer = Convert.ToBoolean(dr.Item("ScanTravelerServer"))
						End If

					Catch ex As Exception
						.ScanTravelerServer = True
					End Try

                    .ExternalAlias = dr.Item("ExternalAlias").ToString()

					Try
						.ShowTaskStringsToSearchFor = dr.Item("SearchString")
						If .ShowTaskStringsToSearchFor = "None" Then
							.ShowTaskStringsToSearchFor = ""
						End If
					Catch ex As Exception
						.ShowTaskStringsToSearchFor = ""
					End Try

                    Try
						.OffHours = OffHours(MyName)
                    Catch ex As Exception
                        .OffHours = False
                    End Try

                    Try
                        If dr.Item("Key") Is Nothing Then
                        Else
                            .Key = dr.Item("Key")
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server 'Key' in Servers table is " & .Key, LogLevel.Verbose)
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server 'Key' field not found. " & ex.ToString)
                    End Try

                    Try
                        If dr.Item("AdvancedMailScan") Is Nothing Then
                            .AdvancedMailScan = True
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .AdvancedMailScan = dr.Item("AdvancedMailScan")
                        End If
                    Catch ex As Exception
                        .AdvancedMailScan = True
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Advanced Mail Scan setting not found, set to True.", LogLevel.Verbose)
                    End Try

                    Try
                        If dr.Item("HeldThreshold") Is Nothing Then
                            .HeldThreshold = 50
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .HeldThreshold = dr.Item("HeldThreshold")
                        End If
                    Catch ex As Exception
                        .HeldThreshold = 50
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Held Mail threshold not set, using default of 50.")
                    End Try

                    Try
                        If dr.Item("DeadThreshold") Is Nothing Then
                            .DeadThreshold = 50
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .DeadThreshold = dr.Item("DeadThreshold")
                        End If
                    Catch ex As Exception
                        .DeadThreshold = 50
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                    End Try

                    'DeadMailDeleteThreshold
                    Try
                        If dr.Item("DeadMailDeleteThreshold") Is Nothing Then
                            .DeleteDeadThreshold = 0
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .DeleteDeadThreshold = dr.Item("DeadMailDeleteThreshold")
                        End If
                    Catch ex As Exception
                        .DeleteDeadThreshold = 0
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail Auto-Delete threshold not set, using default of 0 (no auto deletion).")
                    End Try

                    Try
                        If dr.Item("FailureThreshold") Is Nothing Then
                            .FailureThreshold = 1
                        Else
                            .FailureThreshold = dr.Item("FailureThreshold")
                        End If
                    Catch ex As Exception
                        .FailureThreshold = 1
                    End Try

                    Try
                        If dr.Item("Category") Is Nothing Then
                            .Category = "Domino"
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Category not set, using default of 'Domino'.")
                        ElseIf dr.Item("Category").ToString.Contains("Traveler") Then
                            .SecondaryRole = "Traveler"
                        ElseIf .ClusterMember = "" Then
                            .Category = dr.Item("Category")
                        ElseIf .ClusterMember <> "" And Not (InStr(.ClusterMember, "*ERROR*") > 0) And Not (InStr(.ClusterMember.ToUpper, "RESTRICTED") > 0) Then
                            .Category = "Cluster: " & .ClusterMember
                        End If
                    Catch ex As Exception
                        .Category = "Domino"
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Category not set, using default of 'Domino'.")
                    End Try

                    Try
                        Dim myRegistry As New RegistryHandler
                        .MailFileScanDate = CType(myRegistry.ReadFromRegistry("MailScanDate_" & .Name), DateTime)
                    Catch ex As Exception
                        .MailFileScanDate = Now.AddDays(-1)
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server MailFileScanDate not set, using default of 'Yesterday'.", LogLevel.Verbose)
                    End Try


                    Try
                        '  WriteAuditEntry(Now.ToString & " " & .Name & " Checking Domino server value of BES_Server for ")
                        If dr.Item("BES_Server") Is Nothing Then
                            .BES_Server = False
                        Else
                            .BES_Server = dr.Item("BES_Server")
                            '     WriteAuditEntry(Now.ToString & " " & .Name & " BES Server value: " & .BES_Server)
                        End If
                    Catch ex As Exception
                        ' WriteAuditEntry(Now.ToString & " Exception ERROR: " & ex.Message)
                        .BES_Server = False
                    End Try

                    Try
                        If dr.Item("BES_Threshold") Is Nothing Then
                            .BES_Threshold = 3000
                        Else
                            .BES_Threshold = dr.Item("BES_Threshold")
                        End If
                    Catch ex As Exception
                        .BES_Threshold = 3000
                    End Try


                    Try
                        If dr.Item("Scan Interval") Is Nothing Then
                            .ScanInterval = 10
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server  scan interval not set, using default of 10 minutes.")
                        Else
                            .ScanInterval = dr.Item("Scan Interval")
                        End If
                    Catch ex As Exception
                        .ScanInterval = 10
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server  scan interval not set, using default of 10 minutes.")
                    End Try

                    Try
                        If dr.Item("RetryInterval") Is Nothing Then
                            .RetryInterval = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server  retry scan interval not set, using default of 10 minutes.")
                        Else
                            .RetryInterval = dr.Item("RetryInterval")
                        End If
                    Catch ex As Exception
                        .RetryInterval = 2
                    End Try

                    Try
                        If dr.Item("MailDirectory") Is Nothing Then
                            .MailDirectory = "mail"
                            .CountMailFiles = True
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server  mail directory not set, using default 'mail'.")
                        Else
                            .MailDirectory = dr.Item("MailDirectory")
                            .CountMailFiles = True
                            If .MailDirectory = "None" Then
                                .CountMailFiles = False
                                WriteAuditEntry(Now.ToString & " " & .Name & " Domino server  mail directory will not be scanned.")
                            End If
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server  mail directory not set, using default 'mail'.")
                        .MailDirectory = "mail"
                    End Try


                    Try
                        If dr.Item("OffHoursScanInterval") Is Nothing Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server off hours scan interval not set, using default of 20 minutes.")
                            .OffHoursScanInterval = 20
                        Else
                            .OffHoursScanInterval = dr.Item("OffHoursScanInterval")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server off hours scan interval not set, using default of 20 minutes.")
                        .OffHoursScanInterval = 20
                    End Try

                    Try
                        If dr.Item("ResponseThreshold") Is Nothing Then
                            .ResponseThreshold = 100
                        Else
                            .ResponseThreshold = dr.Item("ResponseThreshold")
                        End If
                    Catch ex As Exception
                        .ResponseThreshold = 100
                    End Try

                    '2/22/2016 NS added for VSPLUS-2641
                    Try
                        If dr.Item("AvailabilityIndexThreshold") Is Nothing Then
                            .AvailabilityIndexThreshold = 0
                        Else
                            .AvailabilityIndexThreshold = dr.Item("AvailabilityIndexThreshold")
                        End If
                    Catch ex As Exception
                        .AvailabilityIndexThreshold = 0
                    End Try

                    Try
                        If dr.Item("Location") Is Nothing Then
                            .Location = "Not Set"
                        Else
                            .Location = dr.Item("Location")
                        End If
                    Catch ex As Exception
                        .Location = "Not Set"
                    End Try

                    Try
                        If dr.Item("SendRouterRestart") Is Nothing Then
                            .RestartRouter = False
                        Else
                            .RestartRouter = Convert.ToBoolean(dr.Item("SendRouterRestart"))
                        End If
                    Catch ex As Exception
                        .RestartRouter = False
                    End Try

					Try
						If dr.Item("Status") Is Nothing Then
							.Status = "Not Scanned"
						Else
							.Status = Convert.ToBoolean(dr.Item("Status"))
						End If
					Catch ex As Exception
						.Status = "Not Scanned"
					End Try

					Try
						If dr.Item("LastUpdate") Is Nothing Then
							.LastScan = Now
						Else
							.LastScan = dr.Item("LastUpdate")
						End If
					Catch ex As Exception
						.LastScan = Now
					End Try

					Try
						If dr.Item("ScanServlet") Is Nothing Then
							.scanServlet = True
						Else
							.scanServlet = dr.Item("ScanServlet")
						End If
					Catch ex As Exception
						.scanServlet = True
                    End Try

                    Try
                        If dr.Item("ScanLog") Is Nothing Then
                            .ScanLog = True
                        Else
                            .ScanLog = dr.Item("ScanLog")
                        End If
                    Catch ex As Exception
                        .ScanLog = True
                    End Try


                    Try
                        If dr.Item("ScanAgentLog") Is Nothing Then
                            .ScanAgentLog = True
                        Else
                            .ScanAgentLog = dr.Item("ScanAgentLog")
                        End If
                    Catch ex As Exception
                        .ScanAgentLog = True
                    End Try

                    '************ Server Statistics *********************
                    'Create and add a collection of custom statistics to monitor
                    '****************************************************

                    Dim dsDominoStatisticsSettings As New Data.DataSet
                    Try
                        'Dim myCommand As New OleDb.OleDbCommand
                        'Dim myOleDbDataAdapter As New Data.OleDb.OleDbDataAdapter
                        'myCommand.CommandText = "SELECT StatName, ThresholdValue, GreaterThanOrLessThan, ConsoleCommand, TimesInARow " & _
                        '"FROM DominoCustomStatValues " & _
                        '"WHERE ServerName='" & .Name & "'"

                        ''WriteAuditEntry(Now.ToString & " Custom Statistics Select Command is: " & myCommand.CommandText)

                        'myCommand.Connection = OleDbConnectionServers
                        'myOleDbDataAdapter.SelectCommand = myCommand

                        'dsDominoStatisticsSettings.Clear()
                        'myOleDbDataAdapter.Fill(dsDominoStatisticsSettings, "Stats")
                        'myCommand.Dispose()
                        'myOleDbDataAdapter.Dispose()
                        Dim strSQL As String = "SELECT StatName, ThresholdValue, GreaterThanOrLessThan, ConsoleCommand, TimesInARow " & _
                        "FROM DominoCustomStatValues " & _
                        "WHERE ServerName='" & .Name & "'"
                        Dim objVSAdaptor As New VSAdaptor
                        objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsDominoStatisticsSettings, "Stats")

                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error Configuring Server Custom Statistics: " & ex.Message)
                    End Try


                    Try
                        Dim drTask As DataRow
                        If dsDominoStatisticsSettings.Tables("Stats").Rows.Count = 0 Then
                            WriteAuditEntry(Now.ToString & " No custom stats defined for " & .Name, LogLevel.Verbose)
                            dsDominoStatisticsSettings.Dispose()
                            Exit Try
                        End If
                        For Each drTask In dsDominoStatisticsSettings.Tables("Stats").Rows

                            Dim MyCustomStatistic As MonitoredItems.DominoCustomStatistic
                            'Check to see if this stat  is already configured
                            'If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Searching for statistic: " & drTask.Item("StatName"))
                            Try
                                MyCustomStatistic = MyDominoServer.CustomStatisticsSettings.Search(drTask.Item("StatName"))
                                'if not, add it to the server's collection
                            Catch ex As Exception
                                'an exception will be thrown if there are no servertaskSettings to search
                                'so we need to create a new blank collection that we can add to
                                MyCustomStatistic = Nothing
                                WriteAuditEntry(Now.ToString & " Exception while Searching for statistic: " & drTask.Item("StatName") & " " & ex.Message)
                            End Try

                            If MyCustomStatistic Is Nothing Then
                                '    WriteAuditEntry(Now.ToString & " Adding settings for istic trigger: " & MyCustomStatistic.Statistic & " " & MyCustomStatistic.ComparisonOperator & " " & MyCustomStatistic.ThresholdValue)
                                Try
                                    Dim MyNewCustomStatisticSetting As New MonitoredItems.DominoCustomStatistic
                                    'WriteAuditEntry(Now.ToString & " Adding Task: " & drTask.Item("TaskName"))
                                    With MyNewCustomStatisticSetting
                                        .Statistic = drTask.Item("StatName")
                                        .ThresholdValue = drTask.Item("ThresholdValue")
                                        .ComparisonOperator = drTask.Item("GreaterThanOrLessThan")
                                        .RepeatThreshold = drTask.Item("TimesInARow")
                                        .ConsoleCommand = drTask.Item("ConsoleCommand")
                                    End With
                                    MyDominoServer.CustomStatisticsSettings.Add(MyNewCustomStatisticSetting)
                                    WriteAuditEntry(Now.ToString & " Added a new Custom Statistic " & MyNewCustomStatisticSetting.Statistic & " " & MyNewCustomStatisticSetting.ComparisonOperator & " " & MyNewCustomStatisticSetting.ThresholdValue & " for " & MyDominoServer.Name, LogLevel.Verbose)

                                Catch ex As Exception
                                    WriteAuditEntry(Now.ToString & " Domino server " & .Name & " error configuring new custom statistic.  Error: " & ex.Message)
                                End Try

                            Else
                                Try
                                    With MyCustomStatistic
                                        WriteAuditEntry(Now.ToString & " Checking settings for existing Custom Statistic " & MyCustomStatistic.Statistic, LogLevel.Verbose)
                                        .ThresholdValue = drTask.Item("ThresholdValue")
                                        .ComparisonOperator = drTask.Item("GreaterThanOrLessThan")
                                        .RepeatThreshold = drTask.Item("TimesInARow")
                                        .ConsoleCommand = drTask.Item("ConsoleCommand")
                                    End With
                                Catch ex As Exception
                                    WriteAuditEntry(Now.ToString & " Domino server " & .Name & " error configuring existing custom Statistic.  Error: " & ex.Message)
                                End Try

                            End If
                        Next
                        dsDominoStatisticsSettings.Dispose()
                        drTask = Nothing
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error Configuring Domino custom statistics: " & ex.Message)
                    End Try

                    Try
                        WriteAuditEntry(Now.ToString & " " & .Name & " has " & .CustomStatisticsSettings.Count & " custom statistics defined.")
                    Catch ex As Exception

                    End Try

                    '***** Server Tasks
                    'Create and add the collection of server tasks that are monitored for this server
                    '******************

                    WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " Updating server task settings-- start.", LogLevel.Verbose)

                    Dim myServerTaskSettings As New MonitoredItems.ServerTaskSettingCollection
                    Dim dsDominoServerTaskSettings As New Data.DataSet
                    Try

                        Dim strSQL As String = " Select ServerTaskSettings.Enabled, DominoServerTasks.FreezeDetect, ServerTaskSettings.SendLoadCommand, ServerTaskSettings.SendRestartCommand, ServerTaskSettings.RestartOffHours,  ServerTaskSettings.SendExitCommand, DominoServerTasks.ConsoleString, DominoServerTasks.TaskName, DominoServerTasks.RetryCount, DominoServerTasks.MaxBusyTime,  DominoServerTasks.LoadString, ServerTaskSettings.MyID " & _
                          "FROM " & _
                          "ServerTaskSettings INNER JOIN DominoServerTasks " & _
                          "ON ServerTaskSettings.TaskID = DominoServerTasks.TaskID " & _
                          "Where ServerTaskSettings.ServerID = " & dr.Item("Key")
                        Dim objVSAdaptor As New VSAdaptor
                        objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsDominoServerTaskSettings, "Tasks")

                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " First Error Configuring Server Tasks: " & ex.Message)
                    End Try

                    If .ServerTaskSettings Is Nothing Then
                        Dim myServerTaskCollection As New MonitoredItems.ServerTaskSettingCollection
                        .ServerTaskSettings = myServerTaskCollection
                    Else
                        WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " This server is configured to monitor " & .ServerTaskSettings.Count & " server tasks.", LogLevel.Verbose)

                        For Each task As MonitoredItems.ServerTaskSetting In .ServerTaskSettings
                            WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " This server is configured to monitor " & task.Name, LogLevel.Verbose)
                        Next

                    End If

                    Try
                        Dim drTask As DataRow
                        For Each drTask In dsDominoServerTaskSettings.Tables("Tasks").Rows

                            Dim MyConfiguredDominoServerTaskSetting As MonitoredItems.ServerTaskSetting
                            MyConfiguredDominoServerTaskSetting = Nothing
                            'Check to see if this task is already configured
                            ' WriteAuditEntry(Now.ToString & " Searching for Task: " & drTask.Item("TaskName"))
                            WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " Updating server task settings. Searching for Task: " & drTask.Item("TaskName"), LogLevel.Verbose)
                            Try
                                MyConfiguredDominoServerTaskSetting = MyDominoServer.ServerTaskSettings.Search(drTask.Item("TaskName"))
                                'if not, add it to the server's collection
                                WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " >> Found " & MyConfiguredDominoServerTaskSetting.Name, LogLevel.Verbose)

                            Catch ex As Exception
                                'an exception will be thrown if there are no servertaskSettings to search
                                'so we need to create a new blank collection that we can add to
                                MyConfiguredDominoServerTaskSetting = Nothing

                            End Try

                            Try
                                ' If Trim(MyConfiguredDominoServerTaskSetting.Name) <> "" Then
                                If Not (MyConfiguredDominoServerTaskSetting) Is Nothing Then
                                    With MyConfiguredDominoServerTaskSetting
                                        ' WriteAuditEntry(Now.ToString & " Checking settings for Task: " & MyConfiguredDominoServerTaskSetting.Name)
                                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Updating server task settings...now updating existing task: " & .Name, LogLevel.Verbose)
                                        Try
                                            .Enabled = drTask.Item("Enabled")
                                        Catch ex As Exception

                                        End Try

                                        Try
                                            .LoadIfMissing = drTask.Item("SendLoadCommand")
                                            .ConsoleString = drTask.Item("ConsoleString")
                                            .RestartServerIfMissingASAP = drTask.Item("SendRestartCommand")
                                            .RestartServerIfMissingOFFHOURS = drTask.Item("RestartOffHours")
                                            .DisallowTask = drTask.Item("SendExitCommand")
                                            .LoadCommand = drTask.Item("LoadString")
                                            .FreezeDetection = drTask.Item("FreezeDetect")
                                            .FailureThreshold = drTask.Item("RetryCount")
                                        Catch ex As Exception
                                            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino server " & MyDominoServer.Name & " error configuring existing task " & MyConfiguredDominoServerTaskSetting.Name & "  Error: " & ex.Message)
                                        End Try

                                    End With
                                End If
                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " Domino server " & .Name & " error configuring existing tasks.  Error: " & ex.Message)
                            End Try

                            Try
                                If MyConfiguredDominoServerTaskSetting Is Nothing Then

                                    Dim MyNewDominoServerTaskSetting As New MonitoredItems.ServerTaskSetting
                                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " Updating server task settings.  Adding new task: " & drTask.Item("TaskName"))

                                    If InStr(drTask.Item("TaskName"), "Traveler") Then
                                        WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " This server is configured to monitor Traveler, so it MUST BE a Traveler server.")
                                        .Traveler_Server = True
                                        .SecondaryRole = "Traveler"
                                    End If

                                    'WriteAuditEntry(Now.ToString & " Adding Task: " & drTask.Item("TaskName"))
                                    With MyNewDominoServerTaskSetting
                                        .Enabled = drTask.Item("Enabled")
                                        '    WriteAuditEntry(Now.ToString & " Enabled=" & .Enabled)
                                        .LoadIfMissing = drTask.Item("SendLoadCommand")
                                        '   WriteAuditEntry(Now.ToString & " SendLoadCommand=" & .LoadIfMissing)
                                        .Name = drTask.Item("TaskName")
                                        .FreezeDetection = drTask.Item("FreezeDetect")
                                        '  WriteAuditEntry(Now.ToString & " FreezeDetect=" & .FreezeDetection)
                                        .ConsoleString = drTask.Item("ConsoleString")
                                        ' WriteAuditEntry(Now.ToString & " ConsoleString=" & .ConsoleString)
                                        .RestartServerIfMissingASAP = drTask.Item("SendRestartCommand")
                                        .RestartServerIfMissingOFFHOURS = drTask.Item("RestartOffHours")
                                        'WriteAuditEntry(Now.ToString & " SendRestartCommand=" & .RestartServerIfMissing)
                                        .DisallowTask = drTask.Item("SendExitCommand")
                                        ' WriteAuditEntry(Now.ToString & " Disallow=" & .DisallowTask)
                                        .LoadCommand = drTask.Item("LoadString")
                                        .MaxRunTime = drTask.Item("MaxBusyTime")
                                        .FailureCount = 0
                                        .FailureThreshold = drTask.Item("RetryCount")
                                    End With
                                    MyDominoServer.ServerTaskSettings.Add(MyNewDominoServerTaskSetting)
                                    '  WriteAuditEntry(Now.ToString & " Domino server " & .Name & " has " & MyDominoServer.ServerTaskSettings.Count & " tasks configured.")
                                End If
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " error configuring new tasks.  Error: " & ex.Message)
                            End Try




                        Next

                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " Updating server task settings-- end.")

                        drTask = Nothing
                        dsDominoServerTaskSettings.Dispose()
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error Configuring Server Tasks: " & ex.Message)
                    End Try

					Try
						If dr.Item("CurrentNodeID") Is Nothing Then
							.InsufficentLicenses = True
						Else
							If dr.Item("CurrentNodeID").ToString() = "-1" Then
								.InsufficentLicenses = True
							Else
								.InsufficentLicenses = False
							End If

						End If
                    Catch ex As Exception
                        '7/8/2015 NS modified for VSPLUS-1959
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino Servers insufficient licenses not set.")

					End Try

                End With

                MyDominoServer = Nothing

            Next

            dr = Nothing

        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Domino servers error: " & exception.Message)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Domino servers error: " & ex.Message)
		End Try


		InsufficentLicensesTest(MyDominoServers)

		dsDominoServers.Dispose()

	End Sub

    Private Sub CreateNotesMailProbeCollection()
        'start with fresh data
        WriteAuditEntry(Now.ToString & " Building a collection of NotesMail Probes")
        'Connect to the data source
        Dim dsNotesMailSettings As New Data.DataSet
        Dim myPath As String
        Dim myRegistry As New RegistryHandler

        Try
			Dim strSQL As String

			strSQL = "SELECT NotesMailProbe.Category, DeliveryThreshold, DestinationDatabase, ServerName DestinationServer, Enabled, NotesMailProbe.Name, NotesMailAddress, "
			strSQL += " OffHoursScanInterval, RetryInterval, ScanInterval, SourceServer, EchoService, ReplyTo, Filename, st.LastUpdate, st.Status, st.StatusCode, di.CurrentNodeID "
			strSQL += " FROM NotesMailProbe "
			strSQL += " inner join Servers on DestinationServerID=ID AND Enabled=1  "
			If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then

				Dim NodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
				strSQL += " inner join DeviceInventory di on NotesMailProbe.Name=di.Name and di.DeviceTypeId=13   "
				strSQL += " inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" & NodeName & "' "
			End If

			strSQL += " left outer join Status st on st.Type=(select ServerType From ServerTypes where Id=13) and st.Name=NotesMailProbe.Name "

			WriteAuditEntry(Now.ToString & " NotesMail Probes query is: " & strSQL)
			Dim objVSAdaptor As New VSAdaptor
			objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsNotesMailSettings, "NotesMailProbe")

		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Failed to create dataset in Create NotesMail Probe processing code. Exception: " & ex.Message)
			Exit Sub
		End Try

        '***
        'but first delete any that are in the collection but not in the database anymore
        'Delete propagation
        'Add the NotesMail probes to the collection


        'Build a collection of what is in the database
        Dim myDataRow As System.Data.DataRow
        Dim MyMailProbeNames As String = ""
        If MyNotesMailProbes.Count > 0 Then
            WriteAuditEntry(Now.ToString & " Performing Delete Propagation for NotesMail Probes.")
            'Get all the names of all the servers in the data table
            For Each myDataRow In dsNotesMailSettings.Tables("NotesMailProbe").Rows()
                MyMailProbeNames += myDataRow.Item("Name") & "  "
            Next
        End If

        'Then loop through the collection and see if it is in the string you just created above
        Dim NMP As MonitoredItems.DominoMailProbe
        Dim myIndex As Integer

        Try
            If MyNotesMailProbes.Count > 0 Then
                For myIndex = MyNotesMailProbes.Count - 1 To 0 Step -1
                    NMP = MyNotesMailProbes.Item(myIndex)
                    Try
                        WriteAuditEntry(Now.ToString & " Checking to see if NotesMail Probe " & NMP.Name & " should be deleted...")
                        If InStr(MyMailProbeNames, NMP.Name) > 0 Then
                            'the server has not been deleted
                            WriteAuditEntry(Now.ToString & " " & NMP.Name & " is not marked for deletion. ")
                        Else
                            'the server has been deleted, so delete from the collection
                            Try
                                MyNotesMailProbes.Delete(NMP.Name)
                                WriteAuditEntry(Now.ToString & " " & NMP.Name & " has been deleted by the service.")
                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " " & NMP.Name & " was not deleted by the service because " & ex.Message)
                            End Try
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Exception NotesMail Probe Deletion Loop on " & NMP.Name & ".  The error was " & ex.Message)
                    End Try
                Next
            End If
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Exception in NotesMail Probes #1 " & ex.ToString)
        End Try



        'Add the NotesMail probes to the collection
		WriteAuditEntry(Now.ToString & " Reading configuration settings for NotesMail Probes.")
		Dim i As Integer = 0

		With dsNotesMailSettings
			Try
				Dim dr As DataRow
				For Each dr In dsNotesMailSettings.Tables("NotesMailProbe").Rows
					WriteAuditEntry(Now.ToString & " Processing NotesMail Probe: " & dr.Item("Name"))
					i += 1
					Dim MyName As String
					Try
						MyName = dr.Item("Name")
					Catch ex As Exception
						MyName = "NotesMail Probe" & i.ToString
					End Try


					MyNotesMailProbe = MyNotesMailProbes.Search(MyName)
					If MyNotesMailProbe Is Nothing Then
						'this is a new one
						MyNotesMailProbe = New MonitoredItems.DominoMailProbe(MyName)
						MyNotesMailProbe.IncrementUpCount()
						WriteAuditEntry(Now.ToString & " Adding NotesMail Probe: " & MyName)
						MyNotesMailProbes.Add(MyNotesMailProbe)
						With MyNotesMailProbe
							.OffHours = False
							.AlertCondition = False
							.Status = "Not Scanned"
							.LastScan = Now.AddMinutes(-15)
							.Description = "Sends NotesMail to target address and verifies delivery."
							.ResponseDetails = ""
							.ResponseTime = 0
                            .ServerType = VSNext.Mongo.Entities.Enums.ServerType.NotesMailProbe.ToDescription()
						End With
					End If


					With MyNotesMailProbe
						WriteAuditEntry(Now.ToString & " Updating NotesMail Probe: " & MyName, LogLevel.Verbose)

						Try
							If dr.Item("ReplyTo") Is Nothing Then
								.ReplyTo = ""
								WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail reply to value not set'.")
							Else
								.ReplyTo = dr.Item("ReplyTo")
							End If
						Catch ex As Exception
							.Category = "NotesMail Probe"
							WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe Category not set, using default of 'NotesMail Probe'.")
						End Try


						Try
							If dr.Item("Category") Is Nothing Then
								.Category = "NotesMail Probe"
								WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe Category not set, using default of 'NotesMail Probe'.")
							Else
								.Category = dr.Item("Category")
							End If
						Catch ex As Exception
							.Category = "NotesMail Probe"
							WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe Category not set, using default of 'NotesMail Probe'.")
						End Try

						Try
							If dr.Item("Filename") Is Nothing Then
								.FileName = ""
								WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe file attachment not specified.")
							Else
								.FileName = dr.Item("Filename")
							End If
						Catch ex As Exception
							.FileName = ""
							WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe file attachment not specified.")
						End Try

						Try
							If dr.Item("ScanInterval") Is Nothing Then
								.ScanInterval = 10
								WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  scan interval not set, using default of 10 minutes.")
							Else
								.ScanInterval = dr.Item("ScanInterval")
							End If
						Catch ex As Exception
							.ScanInterval = 10
							WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe scan interval not set, using default of 10 minutes.")
						End Try

						Try
							If dr.Item("NotesMailAddress") Is Nothing Then
								WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  NotesMailAddress not set, this probe will be disabled.")
								.Enabled = False
							Else
								.NotesMailAddress = dr.Item("NotesMailAddress")
							End If
						Catch ex As Exception
							WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  NotesMailAddress not set, this probe will be disabled.")
							.Enabled = False
						End Try


						Dim myServer As Object
						myServer = myRegistry.ReadFromRegistry("Primary Server")
						Try
							If dr.Item("SourceServer") Is Nothing Then
								If myServer <> Nothing Then
									.SourceServer = CType(myServer, String)
									WriteAuditEntry(Now.ToString & " " & .Name & "  Probe source server not set, using default, " & .SourceServer)
								End If
							Else
								.SourceServer = dr.Item("SourceServer")
							End If
						Catch ex As Exception
							If myServer <> Nothing Then
								.SourceServer = CType(myServer, String)
								WriteAuditEntry(Now.ToString & " " & .Name & "  Probe source server not set, using default, " & .SourceServer)
							End If
						End Try

                        Try
                            '1/21/2016 NS modified for VSPLUS-2332
                            '.OffHours = OffHours(MyName)
                            .OffHours = OffHours(.SourceServer)
                        Catch ex As Exception
                            .OffHours = False
                        End Try

						myServer = Nothing

						Try
							If dr.Item("DestinationServer") Is Nothing Then
								.TargetServer = ""
								.Enabled = False
								WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe destination server not set.")
							Else
								.TargetServer = dr.Item("DestinationServer")
							End If
						Catch ex As Exception
							.TargetServer = ""
							.Enabled = False
							WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe TargetServer  not set. ")
						End Try

						Try
							If dr.Item("DestinationDatabase") Is Nothing Then
								.TargetDatabase = ""
								WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe target database not set.")
							Else
								.TargetDatabase = dr.Item("DestinationDatabase")
							End If
						Catch ex As Exception
							.TargetDatabase = ""
							.Enabled = False
							WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe TargetServer  not set. ")
						End Try

						Try
							If dr.Item("DeliveryThreshold") Is Nothing Then
								.DeliveryThreshold = 5
								WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  DeliveryThreshold not set, using default of 5 minutes.")
							Else
								.DeliveryThreshold = dr.Item("DeliveryThreshold")
							End If
						Catch ex As Exception
							.DeliveryThreshold = 5
						End Try

						Try
							If dr.Item("RetryInterval") Is Nothing Then
								.RetryInterval = 2
								WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  retry scan interval not set, using default of 10 minutes.")
							Else
								.RetryInterval = dr.Item("RetryInterval")
							End If
						Catch ex As Exception
							.RetryInterval = 2
						End Try

						Try
							If dr.Item("OffHoursScanInterval") Is Nothing Then
								WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe off hours scan interval not set, using default of 20 minutes.")
								.OffHoursScanInterval = 20
							Else
								.OffHoursScanInterval = dr.Item("OffHoursScanInterval")
							End If
						Catch ex As Exception
							WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe off hours scan interval not set, using default of 20 minutes.")
							.OffHoursScanInterval = 20
						End Try

						'Try
						'    If dr.Item("Location") Is Nothing Then
						'        .Location = "From " & .SourceServer & " to " & .TargetServer
						'    Else
						'        .Location = dr.Item("Location")
						'    End If
						'Catch ex As Exception
						'    .Location = "From " & .SourceServer & " to " & .TargetServer
                        'End Try


                        Try
                            Dim myDominoServer As MonitoredItems.DominoServer
                            myDominoServer = MyDominoServers.Search(.SourceServer)
                            .Location = myDominoServer.Location
                        Catch ex As Exception
                            .Location = "NotesMail Probe"
                        End Try

						Try
							If dr.Item("Enabled") Is Nothing Then
								.Enabled = True
							Else
								.Enabled = dr.Item("Enabled")
							End If
						Catch ex As Exception
							.Enabled = True
						End Try

						If .Enabled = False Then
							.Status = "Disabled"
						End If

						Try
							If dr.Item("Status") Is Nothing Then
								.Status = "Not Scanned"
								WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  Status not set, using default of Not Scanned.")
							Else
								.Status = dr.Item("Status")
							End If
						Catch ex As Exception
							.Status = "Not Scanned"
							WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe scan interval not set, using default of Not Scanned.")
						End Try

						Try
							If dr.Item("LastUpdate") Is Nothing Then
								.LastScan = Now
								WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  scan interval not set, using default of Now.")
							Else
								.LastScan = dr.Item("LastUpdate")
							End If
						Catch ex As Exception
							.LastScan = Now
							WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe scan interval not set, using default of Now.")
						End Try

						Try
							If dr.Item("CurrentNodeID") Is Nothing Then
								.InsufficentLicenses = True
							Else
								If dr.Item("CurrentNodeID").ToString() = "-1" Then
									.InsufficentLicenses = True
								Else
									.InsufficentLicenses = False
								End If

							End If
                        Catch ex As Exception
                            '7/8/2015 NS modified for VSPLUS-1959
                            WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probes insufficient licenses not set.")

						End Try

					End With

					MyNotesMailProbe = Nothing
				Next
				dr = Nothing
				myRegistry = Nothing

			Catch exception As DataException
				WriteAuditEntry(Now.ToString & " " & exception.Message)
			Catch ex As Exception
				WriteAuditEntry(Now.ToString & " Exception looping through database records: " & ex.Message)
			End Try
		End With

		InsufficentLicensesTest(MyNotesMailProbes)

        'free memory
        dsNotesMailSettings.Dispose()
        myDataRow = Nothing
        MyMailProbeNames = Nothing
        WriteAuditEntry(Now.ToString & " Done configuring NotesMail Probes")
    End Sub

    Private Sub CreateKeywordsCollection()
        'start with fresh data
        myKeywords.Clear()
        'Connect to the data source
        Dim dsKeywords As New Data.DataSet
		'VSPLUS-2300 ticket,Sowjanya
        Try
			Dim strSQL As String = "select Keyword,ServerID,ServerName, RepeatOnce,NotRequiredKeyword,log,AgentLog FROM Logfile inner join [DominoEventLogServers] on logfile.DominoEventLogId=[DominoEventLogServers].DominoEventLogId  inner join Servers on servers.id=[DominoEventLogServers].serverid "

			'changed the query for getting serverid'
            Dim objVSAdaptor As New VSAdaptor
            objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsKeywords, "Keywords")

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in Create Keywords Collection processing code. Exception: " & ex.Message)
            '  Exit Sub
        End Try

        'Add the keywords to the collection
        WriteAuditEntry(Now.ToString & "  Reading configuration settings for Domino log file keywords.")
        Try
            Dim dr As DataRow
            Dim i As Integer = 0
            For Each dr In dsKeywords.Tables("Keywords").Rows
                Dim MyKeyword As New LogFileKeyword
                MyKeyword.Keyword = dr.Item("Keyword")
                MyKeyword.RepeatOnce = dr.Item("RepeatOnce")
                MyKeyword.NotRequiredKeyword = dr.Item("NotRequiredKeyword")
                MyKeyword.ScanLog = dr.Item("Log")
				MyKeyword.ScanAgentLog = dr.Item("AgentLog")
				'MyKeyword.ServerID = dr.Item("ServerID")
				MyKeyword.ServerName = dr.Item("ServerName")
				'added Server Name'
                myKeywords.Add(MyKeyword)
                If MyKeyword.ScanAgentLog = True Then
                    WriteAuditEntry(Now.ToString & "  Watching Domino agent log file for " & MyKeyword.Keyword)
                End If

                If MyKeyword.ScanLog = True Then
                    WriteAuditEntry(Now.ToString & "  Watching Domino log file for " & MyKeyword.Keyword)
                End If
            Next
            ' ReDim Preserve Keywords(i - 1)
            dr = Nothing
        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Data Exception creating Domino log file collection: " & exception.Message)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Random Exception creating Domino log file collection:  " & ex.Message)
        End Try

        'Free the memory
        dsKeywords.Dispose()

    End Sub

    Private Sub CreateDominoClusterCollection()

        'start with fresh data
        '***  Build the data set dynamically
        Dim dsDominoClusters As New Data.DataSet

        Try
            '4/20/2016 NS modified for VSPLUS-2724 
            Dim strSelect As String = ""
            Dim strJoins As String = ""
            Dim strSQL As String = ""
            strSelect = "select d.ID myID,*,s.ID ClustID, s.ServerName A,s2.ServerName B, s3.ServerName C,st.LastUpdate, st.Status, st.StatusCode "

            strJoins = " from DominoCluster d "
            strJoins += " left join Servers s on ServerID_A=s.ID "
            strJoins += "inner join Servers s2 on ServerID_B=s2.ID left join Servers s3 on ServerID_C=s3.ID AND d.Enabled=1 "

			If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then
				Dim NodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
                strSelect += ", di.CurrentNodeID "
                strJoins += "	inner join DeviceInventory di on d.ID=di.DeviceID and di.DeviceTypeId=(select id from ServerTypes where ServerType = 'Domino Cluster')"
                strJoins += " inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" & NodeName & "'"
			End If

            strJoins += " left outer join Status st on st.Type='Domino Cluster database' and st.Name=d.Name"

            strJoins += " where(Enabled = 1)"

            strSQL = strSelect + strJoins
            WriteAuditEntry(Now.ToString & " SQL query for cluster: " & strSQL, LogLevel.Verbose)
			Dim objVSAdaptor As New VSAdaptor
			objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsDominoClusters, "DominoClusters")

			WriteAuditEntry(Now.ToString & " Created a dataset with " & dsDominoClusters.Tables("DominoClusters").Rows.Count & " Domino clusters defined.")
		Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateDominoClustersCollection processing code. Exception: " & ex.Message)
            Exit Sub
        End Try

        '***
        'but first delete any that are in the collection but not in the database anymore
        'Delete propagation

        Dim myDataRow As System.Data.DataRow
        Dim MyServerNames As String = ""

        If myDominoClusters.Count > 0 Then
            '  WriteAuditEntry(Now.ToString & " Checking to see if any Domino servers should be deleted. ")
            'Get all the names of all the servers in the data table
            For Each myDataRow In dsDominoClusters.Tables("DominoClusters").Rows()
                MyServerNames += myDataRow.Item("Name") & "  "
            Next
        End If

        Dim Dom As MonitoredItems.DominoMailCluster
        Dim myIndex As Integer

        If myDominoClusters.Count > 0 Then
            For myIndex = myDominoClusters.Count - 1 To 0 Step -1
                Dom = myDominoClusters.Item(myIndex)
                Try
                    '    WriteAuditEntry(Now.ToString & " Checking to see if Domino server " & Dom.Name & " should be deleted...")
                    If InStr(MyServerNames, Dom.Name) > 0 Then
                        'the server has not been deleted
                        If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Domino Cluster " & Dom.Name & " is not marked for deletion. ")
                    Else
                        'the server has been deleted, so delete from the collection
                        Try
							myDominoClusters.Delete(Dom.Name)
                            If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Domino Cluster " & Dom.Name & " has been deleted by the service.")
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " " & Dom.Name & " was not deleted by the service because " & ex.Message)
                        End Try
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception Domino Cluster Deletion Loop on " & Dom.Name & ".  The error was " & ex.Message)
                End Try
            Next
        End If

        Dom = Nothing
        MyServerNames = Nothing
        myIndex = Nothing


        'Now Add the Domino clusters to the collection, if required
        Dim i As Integer = 0
        Dim MyName As String
        Dim Description As String = ""
        WriteAuditEntry(Now.ToString & " Reading configuration settings for Domino Clusters.")

        Try
            Dim dr As DataRow
            For Each dr In dsDominoClusters.Tables("DominoClusters").Rows
                i += 1

                If dr.Item("Name") Is Nothing Then
                    MyName = "Domino Cluster #" & i.ToString
                Else
                    MyName = dr.Item("Name")
                End If

                Dim ClusterId As String = ""
                ClusterId = dr.Item("myID")

                'See if this server is already in the collection; if so, update its settings otherwise create a new one
                myDominoCluster = myDominoClusters.Search(MyName)
                If myDominoCluster Is Nothing Then
                    'this server is new
                    myDominoCluster = New MonitoredItems.DominoMailCluster
                    myDominoCluster.Name = MyName
                    myDominoCluster.ClusterID = ClusterId
                    myDominoCluster.Status = "Not Scanned"
                    myDominoClusters.Add(myDominoCluster)
                    myDominoCluster.IncrementUpCount()

                    myDominoCluster.ResponseDetails = ""

                    Try
                        Dim myClusterDatabaseCollection As New MonitoredItems.DominoMailClusterDatabaseCollection
                        myDominoCluster.DatabaseCollection = myClusterDatabaseCollection

                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error adding new empty database collection to " & myDominoCluster.Name)
                    End Try
                    myDominoCluster.ServerType = VSNext.Mongo.Entities.Enums.ServerType.DominoCluster.ToDescription()
                Else
                    WriteAuditEntry(Now.ToString & " " & myDominoCluster.Name & " is already in the collection, updating settings.")
                End If

                With myDominoCluster
                    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Configuring Domino Cluster: " & dr.Item("Name"))

                    Try
                        If dr.Item("Enabled") Is Nothing Then
                            .Enabled = True
                        Else
                            .Enabled = dr.Item("Enabled")
                        End If

                    Catch ex As Exception
                        .Enabled = True
                    End Try

                    If .Enabled = False Then
                        .Status = "Disabled"
                    End If

                    If .Enabled = True And .Status = "Disabled" Then
                        .Status = "Not Scanned"
                    End If

					Try
						'Category
						.Category = dr.Item("Category")
					Catch ex As Exception
						.Category = "Mail"
						WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster category exception : " & ex.Message)

					End Try

                    WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster category is : " & .Category)

                    Try
						.OffHours = OffHours(MyName)
                    Catch ex As Exception
                        .OffHours = False
                    End Try

                    Try
                        If dr.Item("A") Is Nothing Then
                            .Server_A_Name = ""
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .Server_A_Name = dr.Item("A")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server A name not set.")
                    End Try

                    Try
                        If dr.Item("B") Is Nothing Then
                            .Server_B_Name = ""
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .Server_B_Name = dr.Item("B")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server B name not set.")
                    End Try

                    Try
                        If dr.Item("C") Is Nothing Then
                            .Server_C_Name = ""
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .Server_C_Name = dr.Item("C")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server C name not set.")
                    End Try

                    '4/20/2016 NS added for VSPLUS-2724
                    Try
                        If dr.Item("Server_A_ExcludeList") Is Nothing Then
                            .Server_A_ExcludeList = ""
                        Else
                            .Server_A_ExcludeList = dr.Item("Server_A_ExcludeList")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server A folder exclusion not set.")
                    End Try

                    Try
                        If dr.Item("Server_B_ExcludeList") Is Nothing Then
                            .Server_B_ExcludeList = ""
                        Else
                            .Server_B_ExcludeList = dr.Item("Server_B_ExcludeList")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server B folder exclusion not set.")
                    End Try

                    Try
                        If dr.Item("Server_C_ExcludeList") Is Nothing Then
                            .Server_C_ExcludeList = ""
                        Else
                            .Server_C_ExcludeList = dr.Item("Server_C_ExcludeList")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server C folder exclusion not set.")
                    End Try

                    Try
                        If .Server_C_Name = "None" Then
                            .Server_C_Name = ""
                            .Server_C_Directory = ""
                            .Server_C_ExcludeList = ""
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("Server_A_Directory") Is Nothing Then
                            .Server_A_Directory = "mail\"
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .Server_A_Directory = dr.Item("Server_A_Directory")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server A folder not set. Using default of Mail")
                    End Try

                    Try
                        If dr.Item("Server_B_Directory") Is Nothing Then
                            .Server_B_Directory = "mail\"
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .Server_B_Directory = dr.Item("Server_B_Directory")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server B folder not set. Using default of Mail")
                    End Try

                    Try
                        If dr.Item("Server_C_Directory") Is Nothing Then
                            .Server_C_Directory = "mail\"
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .Server_C_Directory = dr.Item("Server_C_Directory")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server C folder not set. Using default of Mail")
                    End Try

                    ' This feature is removed
                    'Try
                    '    If dr.Item("Missing_Replica_Alert") Is Nothing Then
                    '        .Missing_Replica_Alert = False
                    '        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster missing alert not set, using default of 'False'.")
                    '    Else
                    '        .Missing_Replica_Alert = dr.Item("Missing_Replica_Alert")
                    '    End If
                    'Catch ex As Exception
                    '    .Missing_Replica_Alert = False
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Category not set, using default of 'Domino'.")
                    'End Try



                    Try
                        If dr.Item("First_Alert_Threshold") Is Nothing Then
                            .FirstAlertThreshold = 10
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster first alert not set, using default of 10.")
                        Else
                            .FirstAlertThreshold = dr.Item("First_Alert_Threshold")
                        End If
                    Catch ex As Exception
                        .FirstAlertThreshold = 10
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server first alert threshold not set, using default of '10%'.")
                    End Try

                    Try
                        .FirstAlertThreshold = .FirstAlertThreshold / 100
                    Catch ex As Exception
                        .FirstAlertThreshold = 0.1
                    End Try



                    Try
                        If dr.Item("ScanInterval") Is Nothing Then
                            .ScanInterval = 60
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster  scan interval not set, using default of 60 minutes.")
                        Else
                            .ScanInterval = dr.Item("ScanInterval")
                        End If
                    Catch ex As Exception
                        .ScanInterval = 60
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster  scan interval not set, using default of 60 minutes.")
                    End Try



                    Try
                        If dr.Item("RetryInterval") Is Nothing Then
                            .RetryInterval = 30
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster  retry scan interval not set, using default of 30 minutes.")
                        Else
                            .RetryInterval = dr.Item("RetryInterval")
                        End If
                    Catch ex As Exception
                        .RetryInterval = 30
                    End Try


                    Try
                        If dr.Item("OffHoursScanInterval") Is Nothing Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server off hours scan interval not set, using default of 20 minutes.")
                            .OffHoursScanInterval = 20
                        Else
                            .OffHoursScanInterval = dr.Item("OffHoursScanInterval")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server off hours scan interval not set, using default of 20 minutes.")
                        .OffHoursScanInterval = 20
                    End Try


                    Try
                        If .ScanInterval < 120 Then
                            .ScanInterval = 120
                        End If

                        If .OffHoursScanInterval < 240 Then
                            .OffHoursScanInterval = 240
                        End If

                        If .RetryInterval < 30 Then
                            .RetryInterval = 30
                        End If
                    Catch ex As Exception

					End Try

					Try
						If dr.Item("Status") Is Nothing Then
							.Status = "Not Scanned"
						Else
							.Status = dr.Item("Status")
						End If
					Catch ex As Exception
						.Status = "Not Scanned"
					End Try

					Try
						If dr.Item("LastUpdate") Is Nothing Then
							.LastScan = Now
						Else
							.LastScan = dr.Item("LastUpdate")
						End If
					Catch ex As Exception
						.LastScan = Now
					End Try

					Try
						If dr.Item("CurrentNodeID") Is Nothing Then
							.InsufficentLicenses = True
						Else
							If dr.Item("CurrentNodeID").ToString() = "-1" Then
								.InsufficentLicenses = True
							Else
								.InsufficentLicenses = False
							End If

						End If
                    Catch ex As Exception
                        '7/8/2015 NS modified for VSPLUS-1959
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino Cluster insufficient licenses not set.")

					End Try

                End With

                myDominoCluster = Nothing
            Next

            dr = Nothing

        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Domino servers error: " & exception.Message)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Domino servers error: " & ex.Message)
        End Try

		InsufficentLicensesTest(myDominoClusters)

        dsDominoClusters.Dispose()


    End Sub


	Private Sub InsufficentLicensesTest(ByRef coll As System.Collections.CollectionBase)

		If (coll.Count = 0) Then
			Return
		End If

		Dim ServerType As String = ""
		Dim ServerTypeForTypeAndName As String = ""
		Select Case coll.GetType()

			Case GetType(MonitoredItems.NotesDatabaseCollection)
                ServerType = "Notes Database"
                '5/5/2016 NS modified
                'Changed from NDB to Notes Database
                ServerTypeForTypeAndName = "Notes Database"

			Case GetType(MonitoredItems.DominoCollection)
				ServerType = "Domino"
				ServerTypeForTypeAndName = "Domino"

			Case GetType(MonitoredItems.DominoMailProbeCollection)
                ServerType = "NotesMail Probe"
                '5/5/2016 NS modified
                'Changed from NMP to NotesMail Probe
                ServerTypeForTypeAndName = "NotesMail Probe"

			Case GetType(MonitoredItems.DominoMailClusterCollection)
                ServerType = "Domino Cluster database"
                '5/5/2016 NS modified
                ServerTypeForTypeAndName = "Domino Cluster database"

		End Select
		CheckForInsufficentLicenses(coll, ServerType, ServerTypeForTypeAndName)

	End Sub

#End Region




    Private Function GetDominoServerHostName(ServerName As String) As String
        WriteDeviceHistoryEntry("Domino", ServerName, Now.ToShortTimeString & " Attempting to determine the ip or hostname")

        'All Domino objects

        Dim docServer As Domino.NotesDocument
        Dim ServersView As Domino.NotesView
        Dim NAB As Domino.NotesDatabase
        Dim DominoServerName As Domino.NotesName
        Dim DirectoryServer As String = ""
        Dim myRegistry As New RegistryHandler
        Dim HostName As String = ""

        Try
            DirectoryServer = NotesSession.GetEnvironmentString("MailServer", True)
            WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Querying  'Names.nsf' on " & DirectoryServer)

        Catch ex As Exception

        End Try

        Try
            NAB = NotesSession.GetDatabase(DirectoryServer, "Names.nsf", False)
            If NAB Is Nothing Then
                Try
                    DirectoryServer = myRegistry.ReadFromRegistry("Primary Server")
                    WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Querying  'Names.nsf' on " & DirectoryServer)
                    NAB = NotesSession.GetDatabase(DirectoryServer, "names.nsf", False)
                Catch ex As Exception
                End Try
            End If


            If NAB Is Nothing Then
                WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Finally, querying  'Names.nsf' on " & ServerName)
                NAB = NotesSession.GetDatabase(ServerName, "names.nsf", False)
            End If



            If NAB Is Nothing Then
                ' MessageBox.Show("Error connecting to 'LocalNames.nsf' ", "Error Retrieving Server Names", MessageBoxButtons.OK, MessageBoxIcon.Error)
                GoTo CleanUp
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", ServerName, "Error connecting to 'Names.nsf' on " & ServerName & vbCrLf & ex.Message)

        End Try

        Try
            ServersView = NAB.GetView("Servers")
            If ServersView Is Nothing Then
                ServersView = NAB.GetView("($Servers)")
            End If


            If ServersView Is Nothing Then
                GoTo CleanUp
            End If
            docServer = ServersView.GetFirstDocument
            WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Connected to the servers view, now searching for server entry.")
            Dim CurrentServer As String
            While Not docServer Is Nothing
                Try
                    CurrentServer = ""
                    CurrentServer = docServer.GetItemValue("ServerName")(0)
                    ' WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Examining " & CurrentServer)
                    DominoServerName = NotesSession.CreateName(docServer.GetItemValue("ServerName")(0))
                    CurrentServer = DominoServerName.Abbreviated
                    '  WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Calculated abbreviated name as " & CurrentServer)
                Catch ex As Exception
                    CurrentServer = ""
                    WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Exception determining server name " & ex.ToString)
                End Try



                Try
                    If InStr(CurrentServer.ToUpper, ServerName.ToUpper) > 0 Then
                        HostName = ""
                        HostName = docServer.GetItemValue("SMTPFullHostDomain")(0)
                        '1/22/2016 NS added for VSPLUS-2068
                        If HostName = "" Then
                            HostName = docServer.GetItemValue("NetAddresses")(0)
                        End If
                        Exit While
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Exception comparing server name " & ex.ToString)
                End Try


                docServer = ServersView.GetNextDocument(docServer)
            End While

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Exception trying to figure out IP or hostname: " & ex.ToString)
        End Try


CleanUp:

        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(docServer)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(ServersView)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(NAB)
            ' System.Runtime.InteropServices.Marshal.ReleaseComObject(s)
        Catch ex As Exception

        End Try

        Return HostName
    End Function

End Class