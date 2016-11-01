Imports System.Threading
Imports VSFramework
Imports VSNext.Mongo.Entities
Imports MongoDB.Driver

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

        WriteAuditEntry(vbCrLf & Now.ToString & " Creating a dataset in CreateNotesDatabaseCollection." & vbCrLf)
        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)
        Try

            'Removed AboveBelow...does not appear to be used anymore


            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.NotesDatabase.ToDescription()) _
                 And repository.Filter.In(Function(x) x.CurrentNode, {getCurrentNode(), "-1"})
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.Category) _
                .Include(Function(x) x.LocationId) _
                .Include(Function(x) x.ScanInterval) _
                .Include(Function(x) x.IsEnabled) _
                .Include(Function(x) x.OffHoursScanInterval) _
                .Include(Function(x) x.ResponseTime) _
                .Include(Function(x) x.RetryInterval) _
                .Include(Function(x) x.DatabaseFileName) _
                .Include(Function(x) x.TriggerType) _
                .Include(Function(x) x.TriggerValue) _
                .Include(Function(x) x.InitiateReplication) _
                .Include(Function(x) x.ReplicationDestination) _
                .Include(Function(x) x.CurrentNode)
            listOfServers = repository.Find(filterDef, projectionDef).ToList()

            Dim repositoryStatus As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDefStatus As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.NotesDatabase.ToDescription())
            Dim projectionDefStatus As ProjectionDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Project _
                .Include(Function(x) x.StatusCode) _
                .Include(Function(x) x.CurrentStatus) _
                .Include(Function(x) x.LastUpdated) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.DeviceName)

            listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList()

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateNotesDatabaseCollection processing code. Exception: " & ex.Message)
            Exit Sub
        End Try

        WriteAuditEntry(Now.ToString & " There are " & listOfServers.Count().ToString() & " Notes Database servers found in the database", LogLevel.Normal)

        '***
        'but first delete any that are in the collection but not in the database anymore
        'Delete propagation
        Dim MyServerNames As String = ""
        If MyNotesDatabases.Count > 0 Then
            WriteAuditEntry(Now.ToString & " Checking to see if any Notes databases should be deleted. ")
            'Get all the names of all the servers in the data table
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                MyServerNames += entity.DeviceName & "  "
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
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                i += 1
                Dim MyName As String
                MyName = entity.DeviceName

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
                        If entity.Id Is Nothing Then
                            .ServerObjectID = ""
                            WriteAuditEntry(Now.ToString & " Error: No filename specified for " & .Name)
                        Else
                            .ServerObjectID = entity.Id
                        End If
                    Catch ex As Exception
                        .ServerObjectID = ""
                        WriteAuditEntry(Now.ToString & " Error: No filename specified for " & .Name)
                    End Try

                    Try
                        If entity.DatabaseFileName Is Nothing Then
                            .FileName = ""
                            WriteAuditEntry(Now.ToString & " Error: No filename specified for " & .Name)
                        Else
                            .FileName = entity.DatabaseFileName
                        End If
                    Catch ex As Exception
                        .FileName = ""
                        WriteAuditEntry(Now.ToString & " Error: No filename specified for " & .Name)
                    End Try

                    Try
                        .OffHours = OffHours(MyName)
                    Catch ex As Exception
                        .OffHours = False
                    End Try

                    Try

                        Dim repositoryLocation As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Location)(connectionString)
                        Dim filterDefLocation As FilterDefinition(Of VSNext.Mongo.Entities.Location) = repositoryLocation.Filter.Eq(Function(x) x.Id, entity.LocationId)
                        Dim locationAlias As String = repositoryLocation.Find(filterDefLocation).ToList()(0).LocationName.ToString()

                        .Location = locationAlias
                        WriteAuditEntry(Now.ToString & " The location for this Notes database is " & .Location)

                    Catch ex As Exception
                        .Location = "Unknown"
                    End Try


                    Try
                        If entity.DeviceName Is Nothing Then
                            .ServerName = ""
                            WriteAuditEntry(Now.ToString & " Error: No Server Name specified for " & .Name)
                        Else
                            .ServerName = entity.DeviceName
                        End If
                    Catch ex As Exception
                        .ServerName = ""
                        .Enabled = False
                        WriteAuditEntry(Now.ToString & " Error:  No Server Name specified for " & .Name)
                    End Try

                    'Commented out since it does not appear to be used
                    'Try
                    '    If dr.Item("AboveBelow") Is Nothing Then
                    '        .AboveBelow = "Above"
                    '        WriteAuditEntry(Now.ToString & " Note: Whether to alert above or below the threshold value not specified for " & .Name & ", 'above' assumed.")
                    '    Else
                    '        .AboveBelow = dr.Item("AboveBelow")
                    '    End If
                    'Catch ex As Exception
                    '    .AboveBelow = "Above"
                    '    'WriteAuditEntry(Now.ToString & " Error: Above or Below threshold value not specified for " & .Name & ", 'above' assumed.  Error: " & ex.ToString)
                    'End Try

                    Try
                        If entity.TriggerType Is Nothing Then
                            .TriggerType = "Database Disappearance"
                            WriteAuditEntry(Now.ToString & " Error: No trigger type specified for " & .Name)
                        Else
                            .TriggerType = entity.TriggerType
                        End If
                    Catch ex As Exception
                        .TriggerType = "Database Disappearance"
                        WriteAuditEntry(Now.ToString & " Error:  No trigger type specified for " & .Name & " because " & ex.Message)
                    End Try

                    'WriteAuditEntry(Now.ToString & " This Notes database uses trigger: " & dr.Item("TriggerType"))

                    Dim myTriggerThreshold As Long
                    Try
                        If entity.TriggerValue Is Nothing Then
                            WriteAuditEntry(Now.ToString & " Error: No trigger threshold specified for " & .Name)
                        Else
                            myTriggerThreshold = entity.TriggerValue
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
                                        If entity.ReplicationDestination Is Nothing Then
                                            .ReplicationDestination = ""
                                            WriteAuditEntry(Now.ToString & " Note: Replication Destination value not specified for " & .Name)
                                        Else
                                            .ReplicationDestination = String.Join(",", entity.ReplicationDestination)
                                            ' WriteAuditEntry(Now.ToString & " Note: Replication Destination value is " & .ReplicationDestination)
                                        End If
                                    Catch ex As Exception
                                        .ReplicationDestination = ""
                                        WriteAuditEntry(Now.ToString & " Error: Replication Destination value not specified for " & .Name & ", 'above' assumed.")
                                    End Try

                                    'InitiateReplication
                                    Try
                                        If entity.InitiateReplication Is Nothing Then
                                            .InitiateReplication = False
                                            WriteAuditEntry(Now.ToString & " Note: Initiate Replication value not specified for " & .Name)
                                        Else
                                            .InitiateReplication = entity.InitiateReplication
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
                        If entity.IsEnabled Is Nothing Then
                            .Enabled = True
                        Else
                            .Enabled = entity.IsEnabled
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
                        If entity.ResponseTime Is Nothing Then
                            .ResponseThreshold = 100
                        Else
                            .ResponseThreshold = entity.ResponseTime
                        End If
                    Catch ex As Exception
                        .ResponseThreshold = 100
                    End Try


                    Try
                        If entity.ScanInterval Is Nothing Then
                            .ScanInterval = 10
                        Else
                            .ScanInterval = entity.ScanInterval
                        End If
                    Catch ex As Exception
                        .ScanInterval = 10
                    End Try
                    WriteAuditEntry(Now.ToString & " Scan Interval is " & .ScanInterval)

                    Try
                        If entity.OffHoursScanInterval Is Nothing Then
                            .OffHoursScanInterval = 30
                        Else
                            .OffHoursScanInterval = entity.OffHoursScanInterval
                        End If
                    Catch ex As Exception
                        .OffHoursScanInterval = 30
                    End Try
                    Try
                        '   WriteAuditEntry(Now.ToString & "Adding Category")
                        If entity.Category Is Nothing Then
                            .Category = "Not Categorized"
                        Else
                            .Category = entity.Category
                        End If
                    Catch ex As Exception
                        .Category = "Not Categorized"
                    End Try



                    Try
                        If entity.RetryInterval Is Nothing Then
                            .RetryInterval = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " Notes Database retry scan interval not set, using default of 10 minutes.")
                        Else
                            .RetryInterval = entity.RetryInterval
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Notes Database retry scan interval not set, using default of 10 minutes.")
                        .RetryInterval = 2
                    End Try

                    If listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.DeviceName) And x.DeviceType.Equals(entity.DeviceType)).ToList().Count() > 0 Then

                        Dim entityStatus As VSNext.Mongo.Entities.Status = listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.DeviceName) And x.DeviceType.Equals(entity.DeviceType)).ToList()(0)


                        Try
                            If entityStatus.CurrentStatus Is Nothing Then
                                .Status = "Not Scanned"
                            Else
                                .Status = entityStatus.CurrentStatus
                            End If
                        Catch ex As Exception
                            .Status = "Not Scanned"
                        End Try

                        Try
                            If entityStatus.LastUpdated Is Nothing Then
                                .LastScan = Now
                            Else
                                .LastScan = entityStatus.LastUpdated
                            End If
                        Catch ex As Exception
                            .LastScan = Now
                        End Try

                    End If


                    Try
                        If entity.CurrentNode Is Nothing Then
                            .InsufficentLicenses = True
                        Else
                            If entity.CurrentNode.ToString() = "-1" Then
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

        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Notes Databases data exception " & exception.Message)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Notes Databases general exception " & ex.ToString)
        End Try

        InsufficentLicensesTest(MyNotesDatabases)

    End Sub


    Private Sub CreateTravelerBackEndCollection()
        myTravelerBackends.Clear()

        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)
        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.TravelerHaDatastore.ToDescription()) _
                 And repository.Filter.In(Function(x) x.CurrentNode, {getCurrentNode(), "-1"})
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.Datastore) _
                .Include(Function(x) x.PortNumber) _
                .Include(Function(x) x.Username) _
                .Include(Function(x) x.Password) _
                .Include(Function(x) x.RequireSSL) _
                .Include(Function(x) x.TestScanServer) _
                .Include(Function(x) x.UsedByServers) _
                .Include(Function(x) x.DatabaseName) _
                .Include(Function(x) x.CurrentNode)
            listOfServers = repository.Find(filterDef, projectionDef).ToList()

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateTravelerBackEndCollection processing code. Exception: " & ex.Message)
            Exit Sub
        End Try

        WriteAuditEntry(Now.ToString & " There are " & listOfServers.Count().ToString() & " Traveler servers found in the database", LogLevel.Normal)

        Try
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                Dim myTravelerBackend As New MonitoredItems.Traveler_Backend
                With myTravelerBackend
                    .TravelerServicePoolName = entity.DeviceName
                    .ServerName = entity.TravelerServiceServerName
                    .DataStore = entity.Datastore
                    .Port = entity.PortNumber
                    .UserName = entity.Username
                    .Password = entity.Password
                    .IntegratedSecurity = entity.RequireSSL
                    .TestScanServer = entity.TestScanServer
                    .UsedByServers = entity.UsedByServers
                    .DatabaseName = entity.DatabaseName
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
        Dim MyDominoServer As MonitoredItems.DominoServer

        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)

        Try

            'Removed DominoServers.DiskSpaceThreshold, DominoServers.NotificationGroup, DominoServer.ScanServlet

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription()) _
                 And repository.Filter.In(Function(x) x.CurrentNode, {getCurrentNode(), "-1"})
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.ScanInterval) _
                .Include(Function(x) x.Category) _
                .Include(Function(x) x.IsEnabled) _
                .Include(Function(x) x.RequireSSL) _
                .Include(Function(x) x.TravelerExternalAlias) _
                .Include(Function(x) x.ScanTravelerServer) _
                .Include(Function(x) x.PendingMailThreshold) _
                .Include(Function(x) x.DeadMailThreshold) _
                .Include(Function(x) x.LocationId) _
                .Include(Function(x) x.MailDirectory) _
                .Include(Function(x) x.OffHoursScanInterval) _
                .Include(Function(x) x.RetryInterval) _
                .Include(Function(x) x.ResponseTime) _
                .Include(Function(x) x.IPAddress) _
                .Include(Function(x) x.ConsecutiveFailuresBeforeAlert) _
                .Include(Function(x) x.SearchString) _
                .Include(Function(x) x.DeadMailDeleteThreshold) _
                .Include(Function(x) x.HeldMailThreshold) _
                .Include(Function(x) x.ScanDBHealth) _
                .Include(Function(x) x.CheckMailThreshold) _
                .Include(Function(x) x.ScanLog) _
                .Include(Function(x) x.ScanAgentLog) _
                .Include(Function(x) x.Description) _
                .Include(Function(x) x.MemoryThreshold) _
                .Include(Function(x) x.CpuThreshold) _
                .Include(Function(x) x.ClusterReplicationDelayThreshold) _
                .Include(Function(x) x.ServerDaysAlert) _
                .Include(Function(x) x.SendRouterRestart) _
                .Include(Function(x) x.AvailabilityIndexThreshold) _
                .Include(Function(x) x.CredentialsId) _
                .Include(Function(x) x.DominoCustomStats) _
                .Include(Function(x) x.ServerTasks) _
                .Include(Function(x) x.CurrentNode)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()

            Dim repositoryStatus As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDefStatus As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
            Dim projectionDefStatus As ProjectionDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Project _
                .Include(Function(x) x.StatusCode) _
                .Include(Function(x) x.CurrentStatus) _
                .Include(Function(x) x.LastUpdated) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.DeviceName)

            listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList()

            '"ds.ServerID As [Key], st.LastUpdate, st.Status, st.StatusCode, di.CurrentNodeID, "
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateDominoServersCollection processing code. Exception: " & ex.Message)
            Exit Sub
        End Try

        WriteAuditEntry(Now.ToString & " There are " & listOfServers.Count().ToString() & " Domino servers found in the database", LogLevel.Normal)

        Dim myDataRow As System.Data.DataRow
        Dim MyServerNames As String = ""
        Try
            WriteAuditEntry(Now.ToString & " Checking to see if any Domino servers have been deleted. ", LogLevel.Verbose)
            If MyDominoServers.Count > 0 Then

                'Get all the names of all the servers in the data table
                For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                    WriteAuditEntry(Now.ToString & " I have " & entity.DeviceName, LogLevel.Verbose)
                    MyServerNames += entity.DeviceName & "  "
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
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                i += 1
                If entity.Description Is Nothing Then
                    Description = "Lotus Domino Server"
                Else
                    Description = entity.Description
                End If

                If entity.DeviceName Is Nothing Then
                    MyName = "DominoServer" & i.ToString
                Else
                    MyName = entity.DeviceName
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
                    WriteAuditEntry(Now.ToString & " Configuring Domino Server: " & entity.DeviceName, LogLevel.Verbose)

                    Try
                        If entity.Id Is Nothing Then
                            .ServerObjectID = ""
                            WriteAuditEntry(Now.ToString & " Error: No filename specified for " & .Name)
                        Else
                            .ServerObjectID = entity.Id
                        End If
                    Catch ex As Exception
                        .ServerObjectID = ""
                        WriteAuditEntry(Now.ToString & " Error: No filename specified for " & .Name)
                    End Try

                    'Get the HTTP username and password, if any
                    Try
                        WriteAuditEntry(Now.ToString & " Getting server credentials.", LogLevel.Verbose)
                        If entity.CredentialsId Is Nothing Then
                            .HTTP_UserName = ""
                            .HTTP_Password = ""
                        Else
                            'Run a query here, then parse the results

                            Dim repositoryCredentials As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Credentials)(connectionString)
                            Dim filterDefCredentials As FilterDefinition(Of VSNext.Mongo.Entities.Credentials) = repositoryCredentials.Filter.Eq(Function(x) x.Id, entity.CredentialsId)
                            Dim entityCredentials As VSNext.Mongo.Entities.Credentials = repositoryCredentials.Find(filterDefCredentials).ToList()(0)

                            .HTTP_UserName = entityCredentials.UserId
                            WriteAuditEntry(Now.ToString & " HTTP Username is " & .HTTP_UserName, LogLevel.Verbose)

                            Dim strEncryptedPassword As String
                            Dim Password As String
                            Dim myPass As Byte()

                            strEncryptedPassword = entityCredentials.Password

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
                        If entity.ServerDaysAlert Is Nothing Then
                            .ServerDaysAlert = 0
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                        Else
                            If Trim(entity.ServerDaysAlert) <> "" Then
                                .ServerDaysAlert = entity.ServerDaysAlert
                                WriteAuditEntry(Now.ToString & " " & .Name & " ServerDaysAlert For this server Is " & .ServerDaysAlert)
                            End If

                        End If
                    Catch ex As Exception
                        .ServerDaysAlert = 0
                        WriteAuditEntry(Now.ToString & " " & .Name & " Custom notification group Not Set For this server, using default alert settings.")
                    End Try


                    'Try
                    '    If dr.Item("NotificationGroup") Is Nothing Then
                    '        .NotificationGroup = ""
                    '        '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold Not set, using default of 50.")
                    '    Else
                    '        .NotificationGroup = dr.Item("NotificationGroup")
                    '    End If
                    'Catch ex As Exception
                    '    .NotificationGroup = ""
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Custom notification group Not set for this server, using default alert settings.")
                    'End Try

                    'CheckMailThreshold
                    'This value is set to 1 if we are supposed to stop counting once the pending or held thresholds are met
                    Try
                        If entity.CheckMailThreshold Is Nothing Then
                            .MailChecking = 0
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold Not set, using default of 50.")
                        Else
                            .MailChecking = entity.CheckMailThreshold
                        End If
                    Catch ex As Exception
                        .MailChecking = 0
                        WriteAuditEntry(Now.ToString & " " & .Name & " CheckMailThreshold was set to zero in an exception.")
                    Finally
                        ' WriteAuditEntry(Now.ToString & " " & .Name & " Domino server CheckMailThreshold value Is " & .MailChecking)
                    End Try

                    Try
                        If entity.ClusterReplicationDelayThreshold Is Nothing Then
                            .ClusterRep_Threshold = 240
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold Not set, using default of 50.")
                        Else
                            .ClusterRep_Threshold = entity.ClusterReplicationDelayThreshold
                        End If
                    Catch ex As Exception
                        .ClusterRep_Threshold = 240
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Cluster Replication Delays threshold Not set, using default of 240 seconds.")
                    End Try

                    Try
                        If entity.CpuThreshold Is Nothing Then
                            .CPU_Threshold = 90
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold Not set, using default of 50.")
                        Else
                            .CPU_Threshold = entity.CpuThreshold * 100
                        End If
                    Catch ex As Exception
                        .CPU_Threshold = 90
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server CPU Utilization threshold Not set, using default of 90%.")
                    End Try

                    Try
                        If entity.MemoryThreshold Is Nothing Then
                            .Memory_Threshold = 90
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold Not set, using default of 50.")
                        Else
                            .Memory_Threshold = entity.MemoryThreshold * 100
                        End If
                    Catch ex As Exception
                        .Memory_Threshold = 90
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server memory usage threshold Not set, using default of 90%.")
                    End Try


                    Try
                        If entity.PendingMailThreshold Is Nothing Then
                            .PendingThreshold = 50
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold Not set, using default of 50.")
                        Else
                            .PendingThreshold = entity.PendingMailThreshold
                        End If
                    Catch ex As Exception
                        .PendingThreshold = 50
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold Not set, using default of 50.")
                    End Try

                    'Try
                    '    If dr.Item("DiskSpaceThreshold") Is Nothing Then
                    '        .DiskThreshold = 0.1  'Set to 10%
                    '        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server disk space threshold Is the default value of 10%", LogLevel.Verbose)
                    '    Else
                    '        .DiskThreshold = dr.Item("DiskSpaceThreshold")
                    '        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server disk space threshold Is " & .DiskThreshold * 100 & "%", LogLevel.Verbose)
                    '    End If
                    'Catch ex As Exception
                    '    .DiskThreshold = 0.1  'Set to 10%
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server disk space threshold Not set, using default of 10%.")
                    'End Try



                    Try
                        If entity.IPAddress Is Nothing Then
                            .IPAddress = ""
                        Else
                            .IPAddress = entity.IPAddress
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

                                Dim repositoryServers As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
                                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repositoryServers.Filter.Eq(Function(x) x.ObjectId, entity.ObjectId)
                                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Server) = repositoryServers.Updater.Set(Function(x) x.IPAddress, .IPAddress)
                                repositoryServers.Update(filterDef, updateDef)

                            End If
                        Else
                            WriteAuditEntry(Now.ToString & " " & .Name & " host name is " & .IPAddress, LogLevel.Verbose)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If entity.IsEnabled Is Nothing Then
                            .Enabled = True
                        Else
                            .Enabled = entity.IsEnabled
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
                        If entity.RequireSSL Is Nothing Then
                            .RequireSSL = False
                        Else
                            .RequireSSL = entity.RequireSSL
                        End If

                    Catch ex As Exception
                        .RequireSSL = False
                    End Try

                    Try
                        If entity.ScanTravelerServer Is Nothing Then
                            .ScanTravelerServer = True
                        Else
                            .ScanTravelerServer = entity.ScanTravelerServer
                        End If

                    Catch ex As Exception
                        .ScanTravelerServer = True
                    End Try

                    .ExternalAlias = entity.TravelerExternalAlias

                    Try
                        .ShowTaskStringsToSearchFor = entity.SearchString
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

                    'Try
                    '    If entity.ObjectId.ToString() Is Nothing Then
                    '    Else
                    '        .Key = entity.ObjectId.ToString()
                    '        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server 'Key' in Servers table is " & .Key, LogLevel.Verbose)
                    '    End If
                    'Catch ex As Exception
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server 'Key' field not found. " & ex.ToString)
                    'End Try

                    'AdvancedMailScan is not being used in the service and is not from SQL
                    'Try
                    '    If dr.Item("AdvancedMailScan") Is Nothing Then
                    '        .AdvancedMailScan = True
                    '        '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                    '    Else
                    '        .AdvancedMailScan = dr.Item("AdvancedMailScan")
                    '    End If
                    'Catch ex As Exception
                    '    .AdvancedMailScan = True
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Advanced Mail Scan setting not found, set to True.", LogLevel.Verbose)
                    'End Try

                    Try
                        If entity.HeldMailThreshold Is Nothing Then
                            .HeldThreshold = 50
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .HeldThreshold = entity.HeldMailThreshold
                        End If
                    Catch ex As Exception
                        .HeldThreshold = 50
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Held Mail threshold not set, using default of 50.")
                    End Try

                    Try
                        If entity.DeadMailThreshold Is Nothing Then
                            .DeadThreshold = 50
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .DeadThreshold = entity.DeadMailThreshold
                        End If
                    Catch ex As Exception
                        .DeadThreshold = 50
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                    End Try

                    'DeadMailDeleteThreshold
                    Try
                        If entity.DeadMailDeleteThreshold Is Nothing Then
                            .DeleteDeadThreshold = 0
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .DeleteDeadThreshold = entity.DeadMailDeleteThreshold
                        End If
                    Catch ex As Exception
                        .DeleteDeadThreshold = 0
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail Auto-Delete threshold not set, using default of 0 (no auto deletion).")
                    End Try

                    Try
                        If entity.ConsecutiveFailuresBeforeAlert Is Nothing Then
                            .FailureThreshold = 1
                        Else
                            .FailureThreshold = entity.ConsecutiveFailuresBeforeAlert
                        End If
                    Catch ex As Exception
                        .FailureThreshold = 1
                    End Try

                    Try
                        If entity.Category Is Nothing Then
                            .Category = "Domino"
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Category not set, using default of 'Domino'.")
                        ElseIf entity.Category.ToString.Contains("Traveler") Then
                            .SecondaryRole = "Traveler"
                        ElseIf .ClusterMember = "" Then
                            .Category = entity.Category
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


                    'Try
                    '    '  WriteAuditEntry(Now.ToString & " " & .Name & " Checking Domino server value of BES_Server for ")
                    '    If dr.Item("BES_Server") Is Nothing Then
                    '        .BES_Server = False
                    '    Else
                    '        .BES_Server = dr.Item("BES_Server")
                    '        '     WriteAuditEntry(Now.ToString & " " & .Name & " BES Server value: " & .BES_Server)
                    '    End If
                    'Catch ex As Exception
                    '    ' WriteAuditEntry(Now.ToString & " Exception ERROR: " & ex.Message)
                    '    .BES_Server = False
                    'End Try

                    'Try
                    '    If dr.Item("BES_Threshold") Is Nothing Then
                    '        .BES_Threshold = 3000
                    '    Else
                    '        .BES_Threshold = dr.Item("BES_Threshold")
                    '    End If
                    'Catch ex As Exception
                    '    .BES_Threshold = 3000
                    'End Try


                    Try
                        If entity.ScanInterval Is Nothing Then
                            .ScanInterval = 10
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server  scan interval not set, using default of 10 minutes.")
                        Else
                            .ScanInterval = entity.ScanInterval
                        End If
                    Catch ex As Exception
                        .ScanInterval = 10
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server  scan interval not set, using default of 10 minutes.")
                    End Try

                    Try
                        If entity.RetryInterval Is Nothing Then
                            .RetryInterval = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server  retry scan interval not set, using default of 10 minutes.")
                        Else
                            .RetryInterval = entity.RetryInterval
                        End If
                    Catch ex As Exception
                        .RetryInterval = 2
                    End Try

                    Try
                        If entity.MailDirectory Is Nothing Then
                            .MailDirectory = "mail"
                            .CountMailFiles = True
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server  mail directory not set, using default 'mail'.")
                        Else
                            .MailDirectory = entity.MailDirectory
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
                        If entity.OffHoursScanInterval Is Nothing Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server off hours scan interval not set, using default of 20 minutes.")
                            .OffHoursScanInterval = 20
                        Else
                            .OffHoursScanInterval = entity.OffHoursScanInterval
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server off hours scan interval not set, using default of 20 minutes.")
                        .OffHoursScanInterval = 20
                    End Try

                    Try
                        If entity.OffHoursScanInterval Is Nothing Then
                            .ResponseThreshold = 100
                        Else
                            .ResponseThreshold = entity.OffHoursScanInterval
                        End If
                    Catch ex As Exception
                        .ResponseThreshold = 100
                    End Try

                    '2/22/2016 NS added for VSPLUS-2641
                    Try
                        If entity.AvailabilityIndexThreshold Is Nothing Then
                            .AvailabilityIndexThreshold = 0
                        Else
                            .AvailabilityIndexThreshold = entity.AvailabilityIndexThreshold
                        End If
                    Catch ex As Exception
                        .AvailabilityIndexThreshold = 0
                    End Try

                    Try
                        If entity.LocationId Is Nothing Then
                            .Location = "Not Set"
                        Else

                            Dim repositoryLocation As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Location)(connectionString)
                            Dim filterDefLocation As FilterDefinition(Of VSNext.Mongo.Entities.Location) = repositoryLocation.Filter.Eq(Function(x) x.Id, entity.LocationId)
                            .Location = repositoryLocation.Find(filterDefLocation).ToList()(0).LocationName

                        End If
                    Catch ex As Exception
                        .Location = "Not Set"
                    End Try

                    Try
                        If entity.SendRouterRestart Is Nothing Then
                            .RestartRouter = False
                        Else
                            .RestartRouter = entity.SendRouterRestart
                        End If
                    Catch ex As Exception
                        .RestartRouter = False
                    End Try

                    Try
                        Dim entityStatus As VSNext.Mongo.Entities.Status
                        Try
                            entityStatus = listOfStatus.Where(Function(x) x.DeviceType.Equals(entity.DeviceType) And x.DeviceName.Equals(entity.DeviceName)).ToList()(0)
                        Catch ex As Exception
                        End Try

                        Try
                            If entityStatus.CurrentStatus Is Nothing Then
                                .Status = "Not Scanned"
                            Else
                                .Status = entityStatus.CurrentStatus
                            End If
                        Catch ex As Exception
                            .Status = "Not Scanned"
                        End Try

                        Try
                            If entityStatus.LastUpdated Is Nothing Then
                                .LastScan = Now
                            Else
                                .LastScan = entityStatus.LastUpdated
                            End If
                        Catch ex As Exception
                            .LastScan = Now
                        End Try

                    Catch ex As Exception

                    End Try

                    Try
                        If entity.ScanLog Is Nothing Then
                            .ScanLog = True
                        Else
                            .ScanLog = entity.ScanLog
                        End If
                    Catch ex As Exception
                        .ScanLog = True
                    End Try


                    Try
                        If entity.ScanAgentLog Is Nothing Then
                            .ScanAgentLog = True
                        Else
                            .ScanAgentLog = entity.ScanAgentLog
                        End If
                    Catch ex As Exception
                        .ScanAgentLog = True
                    End Try

                    '************ Server Statistics *********************
                    'Create and add a collection of custom statistics to monitor
                    '****************************************************

                    Try
                        If entity.DominoCustomStats Is Nothing OrElse entity.DominoCustomStats.Count = 0 Then
                            WriteAuditEntry(Now.ToString & " No custom stats defined for " & .Name, LogLevel.Verbose)
                            Exit Try
                        End If

                        For Each customStat As VSNext.Mongo.Entities.DominoCustomStat In entity.DominoCustomStats

                            Dim MyCustomStatistic As MonitoredItems.DominoCustomStatistic
                            'Check to see if this stat  is already configured
                            'If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Searching for statistic: " & drTask.Item("StatName"))
                            Try
                                MyCustomStatistic = MyDominoServer.CustomStatisticsSettings.Search(customStat.StatName)
                                'if not, add it to the server's collection
                            Catch ex As Exception
                                'an exception will be thrown if there are no servertaskSettings to search
                                'so we need to create a new blank collection that we can add to
                                MyCustomStatistic = Nothing
                                WriteAuditEntry(Now.ToString & " Exception while Searching for statistic: " & customStat.StatName & " " & ex.Message)
                            End Try

                            If MyCustomStatistic Is Nothing Then
                                '    WriteAuditEntry(Now.ToString & " Adding settings for istic trigger: " & MyCustomStatistic.Statistic & " " & MyCustomStatistic.ComparisonOperator & " " & MyCustomStatistic.ThresholdValue)
                                Try
                                    Dim MyNewCustomStatisticSetting As New MonitoredItems.DominoCustomStatistic
                                    'WriteAuditEntry(Now.ToString & " Adding Task: " & drTask.Item("TaskName"))
                                    With MyNewCustomStatisticSetting
                                        .Statistic = customStat.StatName
                                        .ThresholdValue = customStat.ThresholdValue
                                        .ComparisonOperator = customStat.GreaterThanOrLessThan
                                        .RepeatThreshold = customStat.TimesInARow
                                        .ConsoleCommand = customStat.ConsoleCommand

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
                                        .ThresholdValue = customStat.ThresholdValue
                                        .ComparisonOperator = customStat.GreaterThanOrLessThan
                                        .RepeatThreshold = customStat.TimesInARow
                                        .ConsoleCommand = customStat.ConsoleCommand
                                    End With
                                Catch ex As Exception
                                    WriteAuditEntry(Now.ToString & " Domino server " & .Name & " error configuring existing custom Statistic.  Error:  " & ex.Message)
                                End Try

                            End If
                        Next

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

                    Try

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
                            If entity.ServerTasks IsNot Nothing Then

                                For Each Task As VSNext.Mongo.Entities.DominoServerTask In entity.ServerTasks

                                    Dim MyConfiguredDominoServerTaskSetting As MonitoredItems.ServerTaskSetting
                                    MyConfiguredDominoServerTaskSetting = Nothing
                                    'Check to see if this task is already configured
                                    ' WriteAuditEntry(Now.ToString & " Searching for Task: " & drTask.Item("TaskName"))
                                    WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " Updating server task settings. Searching for Task: " & Task.TaskName, LogLevel.Verbose)
                                    Try
                                        MyConfiguredDominoServerTaskSetting = MyDominoServer.ServerTaskSettings.Search(Task.TaskName)
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
                                                    .Enabled = Task.Enabled
                                                Catch ex As Exception

                                                End Try

                                                Try
                                                    .LoadIfMissing = Task.SendLoadCmd
                                                    .ConsoleString = Task.ConsoleString
                                                    .RestartServerIfMissingASAP = Task.SendRestartCmd
                                                    .RestartServerIfMissingOFFHOURS = Task.SendRestartCmdOffhours
                                                    .DisallowTask = Task.SendExitCmd
                                                    .LoadCommand = Task.SendLoadCmd
                                                    .FreezeDetection = Task.FreezeDetect
                                                    .FailureThreshold = Task.RetryCount
                                                Catch ex As Exception
                                                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino server " & MyDominoServer.Name & " Error configuring existing task " & MyConfiguredDominoServerTaskSetting.Name & "  Error: " & ex.Message)
                                                End Try

                                            End With
                                        End If
                                    Catch ex As Exception
                                        WriteAuditEntry(Now.ToString & " Domino server " & .Name & " error configuring existing tasks.  Error: " & ex.Message)
                                    End Try

                                    Try
                                        If MyConfiguredDominoServerTaskSetting Is Nothing Then

                                            Dim MyNewDominoServerTaskSetting As New MonitoredItems.ServerTaskSetting
                                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " Updating server task settings.  Adding new task: " & Task.TaskName)

                                            If InStr(Task.TaskName, "Traveler") Then
                                                WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " This server is configured to monitor Traveler, so it MUST BE a Traveler server.")
                                                .Traveler_Server = True
                                                .SecondaryRole = "Traveler"
                                            End If

                                            'WriteAuditEntry(Now.ToString & " Adding Task: " & drTask.Item("TaskName"))
                                            With MyNewDominoServerTaskSetting
                                                .Enabled = Task.Enabled
                                                '    WriteAuditEntry(Now.ToString & " Enabled=" & .Enabled)
                                                .LoadIfMissing = Task.SendLoadCmd
                                                '   WriteAuditEntry(Now.ToString & " SendLoadCommand=" & .LoadIfMissing)
                                                .Name = Task.TaskName
                                                .FreezeDetection = Task.FreezeDetect
                                                '  WriteAuditEntry(Now.ToString & " FreezeDetect=" & .FreezeDetection)
                                                .ConsoleString = Task.ConsoleString
                                                ' WriteAuditEntry(Now.ToString & " ConsoleString=" & .ConsoleString)
                                                .RestartServerIfMissingASAP = Task.SendRestartCmd
                                                .RestartServerIfMissingOFFHOURS = Task.SendRestartCmdOffhours
                                                'WriteAuditEntry(Now.ToString & " SendRestartCommand=" & .RestartServerIfMissing)
                                                .DisallowTask = Task.SendExitCmd
                                                ' WriteAuditEntry(Now.ToString & " Disallow=" & .DisallowTask)
                                                .LoadCommand = Task.SendLoadCmd
                                                .MaxRunTime = Task.MaxBusyTime
                                                .FailureCount = 0
                                                .FailureThreshold = Task.RetryCount
                                            End With
                                            MyDominoServer.ServerTaskSettings.Add(MyNewDominoServerTaskSetting)
                                            '  WriteAuditEntry(Now.ToString & " Domino server " & .Name & " has " & MyDominoServer.ServerTaskSettings.Count & " tasks configured.")
                                        End If
                                    Catch ex As Exception
                                        WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " error configuring New tasks.  Error:  " & ex.Message)
                                    End Try




                                Next

                                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " Updating server task settings-- end.")

                            End If
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error Configuring Server Tasks 1: " & ex.Message)
                        End Try

                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error Configuring Server Tasks 2: " & ex.Message)
                    End Try


                    Try
                        If entity.CurrentNode Is Nothing Then
                            .InsufficentLicenses = True
                        Else
                            If entity.CurrentNode.ToString() = "-1" Then
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

        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Domino servers error: " & exception.Message)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Domino servers error: " & ex.Message)
        End Try


        InsufficentLicensesTest(MyDominoServers)

    End Sub

    Private Sub CreateNotesMailProbeCollection()
        'start with fresh data
        WriteAuditEntry(Now.ToString & " Building a collection of NotesMail Probes")
        'Connect to the data source
        Dim myPath As String
        Dim myRegistry As New RegistryHandler

        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)

        Try

            'Removed DominoServers.DiskSpaceThreshold, DominoServers.NotificationGroup, DominoServer.ScanServlet

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.NotesMailProbe.ToDescription()) _
                 And repository.Filter.In(Function(x) x.CurrentNode, {getCurrentNode(), "-1"})
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.Category) _
                .Include(Function(x) x.DeliveryThreshold) _
                .Include(Function(x) x.FileName) _
                .Include(Function(x) x.TargetDatabase) _
                .Include(Function(x) x.TargetServer) _
                .Include(Function(x) x.IsEnabled) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.SendToAddress) _
                .Include(Function(x) x.OffHoursScanInterval) _
                .Include(Function(x) x.RetryInterval) _
                .Include(Function(x) x.ScanInterval) _
                .Include(Function(x) x.SourceServer) _
                .Include(Function(x) x.SendToEchoService) _
                .Include(Function(x) x.ReplyToAddress) _
                .Include(Function(x) x.CurrentNode)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()

            Dim repositoryStatus As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDefStatus As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.NotesMailProbe.ToDescription())
            Dim projectionDefStatus As ProjectionDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Project _
                .Include(Function(x) x.StatusCode) _
                .Include(Function(x) x.CurrentStatus) _
                .Include(Function(x) x.LastUpdated) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.DeviceName)

            listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList()

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in Create NotesMail Probe processing code. Exception: " & ex.Message)
            Exit Sub
        End Try

        '***
        'but first delete any that are in the collection but not in the database anymore
        'Delete propagation
        'Add the NotesMail probes to the collection


        'Build a collection of what is in the database
        Dim MyMailProbeNames As String = ""
        If MyNotesMailProbes.Count > 0 Then
            WriteAuditEntry(Now.ToString & " Performing Delete Propagation for NotesMail Probes.")
            'Get all the names of all the servers in the data table
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                MyMailProbeNames += entity.DeviceName & "  "
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


        Try
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                WriteAuditEntry(Now.ToString & " Processing NotesMail Probe: " & entity.DeviceName)
                i += 1
                Dim MyName As String
                Try
                    MyName = entity.DeviceName
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
                        If entity.Id Is Nothing Then
                            .ServerObjectID = ""
                            WriteAuditEntry(Now.ToString & " Error: No filename specified for " & .Name)
                        Else
                            .ServerObjectID = entity.Id
                        End If
                    Catch ex As Exception
                        .ServerObjectID = ""
                        WriteAuditEntry(Now.ToString & " Error: No filename specified for " & .Name)
                    End Try

                    Try
                        If entity.ReplyToAddress Is Nothing Then
                            .ReplyTo = ""
                            WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail reply to value not set'.")
                        Else
                            .ReplyTo = entity.ReplyToAddress
                        End If
                    Catch ex As Exception
                        .Category = "NotesMail Probe"
                        WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe Category not set, using default of 'NotesMail Probe'.")
                    End Try


                    Try
                        If entity.Category Is Nothing Then
                            .Category = "NotesMail Probe"
                            WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe Category Not Set, using default of 'NotesMail Probe'.")
                        Else
                            .Category = entity.Category
                        End If
                    Catch ex As Exception
                        .Category = "NotesMail Probe"
                        WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe Category not set, using default of 'NotesMail Probe'.")
                    End Try

                    Try
                        If entity.FileName Is Nothing Then
                            .FileName = ""
                            WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe file attachment not specified.")
                        Else
                            .FileName = entity.FileName
                        End If
                    Catch ex As Exception
                        .FileName = ""
                        WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe file attachment not specified.")
                    End Try

                    Try
                        If entity.ScanInterval Is Nothing Then
                            .ScanInterval = 10
                            WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  scan interval not set, using default of 10 minutes.")
                        Else
                            .ScanInterval = entity.ScanInterval
                        End If
                    Catch ex As Exception
                        .ScanInterval = 10
                        WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe scan interval not set, using default of 10 minutes.")
                    End Try

                    Try
                        If entity.SendToAddress Is Nothing Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  NotesMailAddress not set, this probe will be disabled.")
                            .Enabled = False
                        Else
                            .NotesMailAddress = entity.SendToAddress
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  NotesMailAddress not set, this probe will be disabled.")
                        .Enabled = False
                    End Try


                    Dim myServer As Object
                    myServer = myRegistry.ReadFromRegistry("Primary Server")
                    Try
                        If entity.SourceServer Is Nothing Then
                            If myServer <> Nothing Then
                                .SourceServer = CType(myServer, String)
                                WriteAuditEntry(Now.ToString & " " & .Name & "  Probe source server not set, using default, " & .SourceServer)
                            End If
                        Else
                            .SourceServer = entity.SourceServer
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
                        If entity.TargetServer Is Nothing Then
                            .TargetServer = ""
                            .Enabled = False
                            WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe destination server not set.")
                        Else
                            .TargetServer = entity.TargetServer
                        End If
                    Catch ex As Exception
                        .TargetServer = ""
                        .Enabled = False
                        WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe TargetServer  not set. ")
                    End Try

                    Try
                        If entity.TargetDatabase Is Nothing Then
                            .TargetDatabase = ""
                            WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe target database not set.")
                        Else
                            .TargetDatabase = entity.TargetDatabase
                        End If
                    Catch ex As Exception
                        .TargetDatabase = ""
                        .Enabled = False
                        WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe TargetServer  not set. ")
                    End Try

                    Try
                        If entity.DeliveryThreshold Is Nothing Then
                            .DeliveryThreshold = 5
                            WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  DeliveryThreshold not set, using default of 5 minutes.")
                        Else
                            .DeliveryThreshold = entity.DeliveryThreshold
                        End If
                    Catch ex As Exception
                        .DeliveryThreshold = 5
                    End Try

                    Try
                        If entity.RetryInterval Is Nothing Then
                            .RetryInterval = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  retry scan interval not set, using default of 10 minutes.")
                        Else
                            .RetryInterval = entity.RetryInterval
                        End If
                    Catch ex As Exception
                        .RetryInterval = 2
                    End Try

                    Try
                        If entity.OffHoursScanInterval Is Nothing Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe off hours scan interval not set, using default of 20 minutes.")
                            .OffHoursScanInterval = 20
                        Else
                            .OffHoursScanInterval = entity.OffHoursScanInterval
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
                        If entity.IsEnabled Is Nothing Then
                            .Enabled = True
                        Else
                            .Enabled = entity.IsEnabled
                        End If
                    Catch ex As Exception
                        .Enabled = True
                    End Try

                    If .Enabled = False Then
                        .Status = "Disabled"
                    End If

                    Try
                        Dim entityStatus As VSNext.Mongo.Entities.Status
                        Try
                            entityStatus = listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.DeviceName))
                        Catch ex As Exception
                            entityStatus = New VSNext.Mongo.Entities.Status()
                        End Try

                        Try
                            If entityStatus.CurrentStatus Is Nothing Then
                                .Status = "Not Scanned"
                                WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  Status not set, using default of Not Scanned.")
                            Else
                                .Status = entityStatus.CurrentStatus
                            End If
                        Catch ex As Exception
                            .Status = "Not Scanned"
                            WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe scan interval not set, using default of Not Scanned.")
                        End Try

                        Try
                            If entityStatus.LastUpdated Is Nothing Then
                                .LastScan = Now
                                WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  scan interval not set, using default of Now.")
                            Else
                                .LastScan = entityStatus.LastUpdated
                            End If
                        Catch ex As Exception
                            .LastScan = Now
                            WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe scan interval not set, using default of Now.")
                        End Try

                    Catch ex As Exception

                    End Try


                    Try
                        If entity.CurrentNode Is Nothing Then
                            .InsufficentLicenses = True
                        Else
                            If entity.CurrentNode.ToString() = "-1" Then
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
            myRegistry = Nothing

        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " " & exception.Message)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Exception looping through database records: " & ex.Message)
        End Try



        InsufficentLicensesTest(MyNotesMailProbes)

        'free memory
        MyMailProbeNames = Nothing
        WriteAuditEntry(Now.ToString & " Done configuring NotesMail Probes")
    End Sub

    Private Sub CreateKeywordsCollection()
        'start with fresh data
        myKeywords.Clear()
        'Connect to the data source
        'VSPLUS-2300 ticket,Sowjanya
        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.DominoLogScanning.ToDescription()) _
                 And repository.Filter.In(Function(x) x.CurrentNode, {getCurrentNode(), "-1"})
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.LogFileServers) _
                .Include(Function(x) x.LogFileKeywords)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in Create Keywords Collection processing code. Exception: " & ex.Message)
            '  Exit Sub
        End Try

        'Add the keywords to the collection
        WriteAuditEntry(Now.ToString & "  Reading configuration settings for Domino log file keywords.")
        Try
            Dim dr As DataRow
            Dim i As Integer = 0
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                For Each entityKeyword As VSNext.Mongo.Entities.LogFileKeyword In entity.LogFileKeywords
                    For Each serverObjectId As String In entity.LogFileServers
                        Try


                            Dim MyKeyword As New LogFileKeyword()
                            MyKeyword.Keyword = entityKeyword.Keyword
                            MyKeyword.RepeatOnce = entityKeyword.OneAlertPerDay
                            MyKeyword.NotRequiredKeyword = entityKeyword.Exclude
                            MyKeyword.ScanLog = entityKeyword.ScanLog
                            MyKeyword.ScanAgentLog = entityKeyword.ScanAgentLog
                            'MyKeyword.ServerID = dr.Item("ServerID")
                            Dim repositoryServers As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
                            Dim filterDefServers As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repositoryServers.Filter.Eq(Function(x) x.Id, serverObjectId)
                            Dim projectionDefServers As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repositoryServers.Project.Include(Function(x) x.DeviceName)
                            MyKeyword.ServerName = repositoryServers.Find(filterDefServers, projectionDefServers).ToList()(0).DeviceName
                            'added Server Name'
                            myKeywords.Add(MyKeyword)
                            If MyKeyword.ScanAgentLog = True Then
                                WriteAuditEntry(Now.ToString & "  Watching Domino agent log file for " & MyKeyword.Keyword)
                            End If

                            If MyKeyword.ScanLog = True Then
                                WriteAuditEntry(Now.ToString & "  Watching Domino log file for " & MyKeyword.Keyword)
                            End If
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Exception creating Domino log file collection: " & ex.Message)
                        End Try
                    Next
                Next

            Next
            ' ReDim Preserve Keywords(i - 1)
            dr = Nothing
        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Data Exception creating Domino log file collection: " & exception.Message)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Random Exception creating Domino log file collection:  " & ex.Message)
        End Try

    End Sub

    Private Sub CreateDominoClusterCollection()

        'start with fresh data
        '***  Build the data set dynamically
        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfDominoServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)

        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.NotesDatabaseReplica.ToDescription()) _
                 And repository.Filter.In(Function(x) x.CurrentNode, {getCurrentNode(), "-1"})
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.DominoServerA) _
                .Include(Function(x) x.DominoServerAExcludeFolders) _
                .Include(Function(x) x.DominoServerAFileMask) _
                .Include(Function(x) x.DominoServerB) _
                .Include(Function(x) x.DominoServerBExcludeFolders) _
                .Include(Function(x) x.DominoServerBFileMask) _
                .Include(Function(x) x.DominoServerC) _
                .Include(Function(x) x.DominoServerCExcludeFolders) _
                .Include(Function(x) x.DominoServerCFileMask) _
                .Include(Function(x) x.FirstAlertThreshold) _
                .Include(Function(x) x.IsEnabled) _
                .Include(Function(x) x.Category) _
                .Include(Function(x) x.ScanInterval) _
                .Include(Function(x) x.RetryInterval) _
                .Include(Function(x) x.OffHoursScanInterval) _
                .Include(Function(x) x.CurrentNode)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()

            filterDef = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
            projectionDef = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName)
            listOfDominoServers = repository.Find(filterDef, projectionDef).ToList()

            Dim repositoryStatus As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDefStatus As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.NotesDatabase.ToDescription())
            Dim projectionDefStatus As ProjectionDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Project _
                .Include(Function(x) x.StatusCode) _
                .Include(Function(x) x.CurrentStatus) _
                .Include(Function(x) x.LastUpdated) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.DeviceName)

            listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList()

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateDominoClustersCollection processing code. Exception: " & ex.Message)
            Exit Sub
        End Try

        '***
        'but first delete any that are in the collection but not in the database anymore
        'Delete propagation

        Dim MyServerNames As String = ""

        If myDominoClusters.Count > 0 Then
            '  WriteAuditEntry(Now.ToString & " Checking to see if any Domino servers should be deleted. ")
            'Get all the names of all the servers in the data table
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                MyServerNames += entity.DeviceName & "  "
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
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                i += 1

                If entity.DeviceName Is Nothing Then
                    MyName = "Domino Cluster #" & i.ToString
                Else
                    MyName = entity.DeviceName
                End If


                'See if this server is already in the collection; if so, update its settings otherwise create a new one
                myDominoCluster = myDominoClusters.Search(MyName)
                If myDominoCluster Is Nothing Then
                    'this server is new
                    myDominoCluster = New MonitoredItems.DominoMailCluster
                    myDominoCluster.Name = MyName
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
                    myDominoCluster.ServerType = VSNext.Mongo.Entities.Enums.ServerType.NotesDatabaseReplica.ToDescription()
                Else
                    WriteAuditEntry(Now.ToString & " " & myDominoCluster.Name & " is already in the collection, updating settings.")
                End If

                With myDominoCluster
                    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Configuring Domino Cluster: " & entity.DeviceName)

                    Try
                        If entity.Id Is Nothing Then
                            .ServerObjectID = ""
                            WriteAuditEntry(Now.ToString & " Error: No filename specified for " & .Name)
                        Else
                            .ServerObjectID = entity.Id
                        End If
                    Catch ex As Exception
                        .ServerObjectID = ""
                        WriteAuditEntry(Now.ToString & " Error: No filename specified for " & .Name)
                    End Try

                    Try
                        If entity.IsEnabled Is Nothing Then
                            .Enabled = True
                        Else
                            .Enabled = entity.IsEnabled
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
                        .Category = entity.Category
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
                        If entity.DominoServerA Is Nothing Then
                            .Server_A_Name = ""
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            Try
                                .Server_A_Name = listOfDominoServers.Where(Function(x) x.ObjectId.Equals(entity.DominoServerA)).ToList()(0).DeviceName
                            Catch ex As Exception
                                .Server_A_Name = ""
                            End Try
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server A name not set.")
                    End Try

                    Try
                        If entity.DominoServerB Is Nothing Then
                            .Server_B_Name = ""
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            Try
                                .Server_B_Name = listOfDominoServers.Where(Function(x) x.ObjectId.Equals(entity.DominoServerB)).ToList()(0).DeviceName
                            Catch ex As Exception
                                .Server_B_Name = ""
                            End Try
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server B name not set.")
                    End Try

                    Try
                        If entity.DominoServerC Is Nothing Then
                            .Server_C_Name = ""
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            Try
                                .Server_C_Name = listOfDominoServers.Where(Function(x) x.ObjectId.Equals(entity.DominoServerC)).ToList()(0).DeviceName
                            Catch ex As Exception
                                .Server_C_Name = ""
                            End Try
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server C name not set.")
                    End Try

                    '4/20/2016 NS added for VSPLUS-2724
                    Try
                        If entity.DominoServerAExcludeFolders Is Nothing Then
                            .Server_A_ExcludeList = ""
                        Else
                            .Server_A_ExcludeList = entity.DominoServerAExcludeFolders
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server A folder exclusion not set.")
                    End Try

                    Try
                        If entity.DominoServerBExcludeFolders Is Nothing Then
                            .Server_B_ExcludeList = ""
                        Else
                            .Server_B_ExcludeList = entity.DominoServerBExcludeFolders
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server B folder exclusion not set.")
                    End Try

                    Try
                        If entity.DominoServerCExcludeFolders Is Nothing Then
                            .Server_C_ExcludeList = ""
                        Else
                            .Server_C_ExcludeList = entity.DominoServerCExcludeFolders
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
                        If entity.DominoServerAFileMask Is Nothing Then
                            .Server_A_Directory = "mail\"
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .Server_A_Directory = entity.DominoServerAFileMask
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server A folder not set. Using default of Mail")
                    End Try

                    Try
                        If entity.DominoServerBFileMask Is Nothing Then
                            .Server_B_Directory = "mail\"
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .Server_B_Directory = entity.DominoServerBFileMask
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster server B folder not set. Using default of Mail")
                    End Try

                    Try
                        If entity.DominoServerCFileMask Is Nothing Then
                            .Server_C_Directory = "mail\"
                            '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                        Else
                            .Server_C_Directory = entity.DominoServerCFileMask
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
                        If entity.FirstAlertThreshold Is Nothing Then
                            .FirstAlertThreshold = 10
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster first alert not set, using default of 10.")
                        Else
                            .FirstAlertThreshold = entity.FirstAlertThreshold
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
                        If entity.ScanInterval Is Nothing Then
                            .ScanInterval = 60
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster  scan interval not set, using default of 60 minutes.")
                        Else
                            .ScanInterval = entity.ScanInterval
                        End If
                    Catch ex As Exception
                        .ScanInterval = 60
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster  scan interval not set, using default of 60 minutes.")
                    End Try



                    Try
                        If entity.RetryInterval Is Nothing Then
                            .RetryInterval = 30
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino cluster  retry scan interval not set, using default of 30 minutes.")
                        Else
                            .RetryInterval = entity.RetryInterval
                        End If
                    Catch ex As Exception
                        .RetryInterval = 30
                    End Try


                    Try
                        If entity.OffHoursScanInterval Is Nothing Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server off hours scan interval not set, using default of 20 minutes.")
                            .OffHoursScanInterval = 20
                        Else
                            .OffHoursScanInterval = entity.OffHoursScanInterval
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
                        Dim entityStatus As VSNext.Mongo.Entities.Status
                        Try
                            entityStatus = listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.DeviceName)).ToList()(0)
                        Catch ex As Exception

                        End Try
                        Try
                            If entityStatus.CurrentStatus Is Nothing Then
                                .Status = "Not Scanned"
                            Else
                                .Status = entityStatus.CurrentStatus
                            End If
                        Catch ex As Exception
                            .Status = "Not Scanned"
                        End Try

                        Try
                            If entityStatus.LastUpdated Is Nothing Then
                                .LastScan = Now
                            Else
                                .LastScan = entityStatus.LastUpdated
                            End If
                        Catch ex As Exception
                            .LastScan = Now
                        End Try
                    Catch ex As Exception

                    End Try


                    Try
                        If entity.CurrentNode Is Nothing Then
                            .InsufficentLicenses = True
                        Else
                            If entity.CurrentNode.ToString() = "-1" Then
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


        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Domino servers error: " & exception.Message)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Domino servers error: " & ex.Message)
        End Try

        InsufficentLicensesTest(myDominoClusters)

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

    Private Function getCurrentNode() As String
        Dim NodeName As String = ""
        If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then
            NodeName = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
        End If
        Return NodeName
    End Function

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