Imports System.Threading
Imports VSFramework
Imports VSNext.Mongo.Entities
Imports MongoDB.Driver

Partial Public Class VitalSignsPlusExJournal

#Region "Create Collections of items to Monitor"

    Public Sub CreateCollections()


        Try
            CreateDominoServersCollection()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Domino Servers collection: " & ex.Message)
        End Try



    End Sub

    Private Sub CreateDominoServersCollection()

        'start with fresh data
        '***  Build the data set dynamically

        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)

        Try

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
                .Include(Function(x) x.Description) _
                .Include(Function(x) x.NotificationList) _
                .Include(Function(x) x.MemoryThreshold) _
                .Include(Function(x) x.CpuThreshold) _
                .Include(Function(x) x.ClusterReplicationDelayThreshold) _
                .Include(Function(x) x.EXJournalStartTime) _
                .Include(Function(x) x.EXJournalDuration) _
                .Include(Function(x) x.EXJournalLookbackDuration) _
                .Include(Function(x) x.EXJournalEnabled) _
                .Include(Function(x) x.CurrentNode)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()

            Dim objVSAdaptor As New VSAdaptor

            WriteAuditEntry(Now.ToString & " Created a Domino servers dataset with " & listOfServers.Count & " records found.")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateDominoServersCollection processing code. Exception: " & ex.Message)
            Exit Sub
        End Try



        Dim MyServerNames As String = ""
        Try
            WriteAuditEntry(Now.ToString & " Checking to see if any Domino servers have been deleted. ")
            If MyDominoServers.Count > 0 Then

                'Get all the names of all the servers in the data table
                For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                    WriteAuditEntry(Now.ToString & " I have " & entity.DeviceName)
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
                        If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & Dom.Name & " is not marked for deletion. ")
                    Else
                        'the server has been deleted, so delete from the collection
                        Try
                            MyDominoServers.Delete(Dom.Name)
                            If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & Dom.Name & " has been deleted by the service.")
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
        WriteAuditEntry(Now.ToString & " Reading configuration settings for Domino Servers.")

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
                    MyDominoServer.ServerType = VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription()
                    MyDominoServer.Traveler_Server = False
                    MyDominoServers.Add(MyDominoServer)
                    Dim tNow As DateTime
                    tNow = Now
                    MyDominoServer.IPAddress = ""
                    MyDominoServer.ScanCount = 0
                    MyDominoServer.PingCount = 0
                    MyDominoServer.LastPing = Now
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
                    MyDominoServer.PreviousDeliveredMail = -1
                    MyDominoServer.PreviousRoutedMail = -1
                    MyDominoServer.PreviousTransferredMail = -1
                    MyDominoServer.PreviousSMTPMessages = -1
                    MyDominoServer.PreviousMailFailures = -1

                    MyDominoServer.TransferredMail = 0
                    MyDominoServer.DeliveredMail = 0
                    MyDominoServer.RoutedMail = 0

                    'for tracking cluster stats
                    MyDominoServer.ClusterOpenRedirects_Previous_LoadBalanceSuccessful = -1
                    MyDominoServer.ClusterOpenRedirects_Previous_LoadBalanceUnSuccessful = -1
                    MyDominoServer.ClusterOpenRedirects_Previous_FailoverUnSuccessful = -1
                    MyDominoServer.ClusterOpenRedirects_Previous_FailoverSuccessful = 1
                    MyDominoServer.OffHours = False
                    MyDominoServer.AlertCondition = False
                    MyDominoServer.Status = "Not Scanned"
                    MyDominoServer.LastScan = Date.Now
                    MyDominoServer.IncrementUpCount()
                    MyDominoServer.MailboxCount = 999  'this flag tells app to check for mailbox count
                    MyDominoServer.UserCount = 0
                    MyDominoServer.Statistics_Server = ""
                    MyDominoServer.Statistics_Disk = ""


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
                    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Configuring Domino Server: " & entity.DeviceName)

                    'Try
                    '    If dr.Item("NotificationGroup") Is Nothing Then
                    '        .NotificationGroup = ""
                    '        '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                    '    Else
                    '        If Trim(dr.Item("NotificationGroup")) <> "" Then
                    '            .NotificationGroup = dr.Item("NotificationGroup")
                    '            WriteAuditEntry(Now.ToString & " " & .Name & " Notifications for this server will go to " & .NotificationGroup)
                    '        End If

                    '    End If
                    'Catch ex As Exception
                    '    .NotificationGroup = ""
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Custom notification group not set for this server, using default alert settings.")
                    'End Try


                    'Try
                    '    If entity.ClusterReplicationDelayThreshold Is Nothing Then
                    '        .ClusterRep_Threshold = 240
                    '        '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                    '    Else
                    '        .ClusterRep_Threshold = entity.ClusterReplicationDelayThreshold
                    '    End If
                    'Catch ex As Exception
                    '    .ClusterRep_Threshold = 240
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Cluster Replication Delays threshold not set, using default of 240 seconds.")
                    'End Try

                    'Try
                    '    If entity.CpuThreshold Is Nothing Then
                    '        .CPU_Threshold = 90
                    '        '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                    '    Else
                    '        .CPU_Threshold = entity.CpuThreshold * 100
                    '    End If
                    'Catch ex As Exception
                    '    .CPU_Threshold = 90
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server CPU Utilization threshold not set, using default of 90%.")
                    'End Try

                    'Try
                    '    If entity.MemoryThreshold Is Nothing Then
                    '        .Memory_Threshold = 90
                    '        '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                    '    Else
                    '        .Memory_Threshold = entity.MemoryThreshold * 100
                    '    End If
                    'Catch ex As Exception
                    '    .Memory_Threshold = 90
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server memory usage threshold not set, using default of 90%.")
                    'End Try


                    'Try
                    '    If entity.PendingMailThreshold Is Nothing Then
                    '        .PendingThreshold = 50
                    '        '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                    '    Else
                    '        .PendingThreshold = entity.PendingMailThreshold
                    '    End If
                    'Catch ex As Exception
                    '    .PendingThreshold = 50
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
                    'End Try

                    'Try
                    '    If dr.Item("DiskSpaceThreshold") Is Nothing Then
                    '        .DiskThreshold = 0.1  'Set to 10%
                    '        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server disk space threshold is the default value of 10%")
                    '    Else
                    '        .DiskThreshold = dr.Item("DiskSpaceThreshold")
                    '        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server disk space threshold is " & .DiskThreshold * 100 & "%")
                    '    End If
                    'Catch ex As Exception
                    '    .DiskThreshold = 0.1  'Set to 10%
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server disk space threshold not set, using default of 10%.")
                    'End Try

                    'If .DiskThreshold > 0.99 Then
                    '    .DiskThreshold = 0.99
                    'End If

                    Try
                        If entity.IPAddress Is Nothing Then
                            .IPAddress = ""
                        Else
                            .IPAddress = entity.IPAddress
                        End If
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server IP Address is " & .IPAddress)

                    Catch ex As Exception
                        .IPAddress = ""
                    End Try

                    Try
                        If .IPAddress.Trim = "" Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server does not have an IP or hostname defined.  I am going to try to figure it out.")
                            .IPAddress = GetDominoServerHostName(.Name)
                            WriteAuditEntry(Now.ToString & " " & .Name & " I figure the host name is " & .IPAddress)
                            If .IPAddress.Length > 4 Then
                                Dim vsObj As New VSAdaptor
                                Dim strSQL As String = "Update DominoServers SET IPAddress='" & .IPAddress & "' WHERE Name ='" & .Name & "'"
                                vsObj.ExecuteNonQueryAny("VitalSigns", "Servers", strSQL)
                            End If
                        Else
                            WriteAuditEntry(Now.ToString & " " & .Name & " host name is " & .IPAddress)
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
                        .ShowTaskStringsToSearchFor = entity.SearchString
                        If .ShowTaskStringsToSearchFor = "None" Then
                            .ShowTaskStringsToSearchFor = ""
                        End If
                    Catch ex As Exception
                        .ShowTaskStringsToSearchFor = ""
                    End Try

                    Try
                        .OffHours = BoolOffHours
                    Catch ex As Exception
                        .OffHours = False
                    End Try


                    'Try
                    '    If dr.Item("Key") Is Nothing Then

                    '    Else
                    '        .Key = dr.Item("Key")
                    '        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server 'Key' in Servers table is " & .Key)
                    '    End If
                    'Catch ex As Exception
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server 'Key' field not found. " & ex.ToString)
                    'End Try

                    'Try
                    '    If dr.Item("AdvancedMailScan") Is Nothing Then
                    '        .AdvancedMailScan = True
                    '        '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                    '    Else
                    '        .AdvancedMailScan = dr.Item("AdvancedMailScan")
                    '    End If
                    'Catch ex As Exception
                    '    .AdvancedMailScan = True
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Advanced Mail Scan setting not found, set to True.")
                    'End Try

                    'Try
                    '    If dr.Item("HeldThreshold") Is Nothing Then
                    '        .HeldThreshold = 50
                    '        '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                    '    Else
                    '        .HeldThreshold = dr.Item("HeldThreshold")
                    '    End If
                    'Catch ex As Exception
                    '    .HeldThreshold = 50
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Held Mail threshold not set, using default of 50.")
                    'End Try

                    'Try
                    '    If dr.Item("DeadThreshold") Is Nothing Then
                    '        .DeadThreshold = 50
                    '        '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                    '    Else
                    '        .DeadThreshold = dr.Item("DeadThreshold")
                    '    End If
                    'Catch ex As Exception
                    '    .DeadThreshold = 50
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                    'End Try

                    'DeadMailDeleteThreshold
                    'Try
                    '    If dr.Item("DeadMailDeleteThreshold") Is Nothing Then
                    '        .DeleteDeadThreshold = 0
                    '        '   WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail threshold not set, using default of 50.")
                    '    Else
                    '        .DeleteDeadThreshold = dr.Item("DeadMailDeleteThreshold")
                    '    End If
                    'Catch ex As Exception
                    '    .DeleteDeadThreshold = 0
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Dead Mail Auto-Delete threshold not set, using default of 0 (no auto deletion).")
                    'End Try

                    'Try
                    '    If dr.Item("FailureThreshold") Is Nothing Then
                    '        .FailureThreshold = 1
                    '    Else
                    '        .FailureThreshold = dr.Item("FailureThreshold")
                    '    End If
                    'Catch ex As Exception
                    '    .FailureThreshold = 1
                    'End Try

                    'Try
                    '    If dr.Item("Category") Is Nothing Then
                    '        .Category = "Domino"
                    '        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Category not set, using default of 'Domino'.")
                    '    ElseIf dr.Item("Category").ToString.Contains("Traveler") Then
                    '        .SecondaryRole = "Traveler"
                    '    ElseIf .ClusterMember = "" Then
                    '        .Category = dr.Item("Category")
                    '    ElseIf .ClusterMember <> "" And Not (InStr(.ClusterMember, "*ERROR*") > 0) And Not (InStr(.ClusterMember.ToUpper, "RESTRICTED") > 0) Then
                    '        .Category = "Cluster: " & .ClusterMember
                    '    End If
                    'Catch ex As Exception
                    '    .Category = "Domino"
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Category not set, using default of 'Domino'.")
                    'End Try

                    'Try
                    '    Dim myRegistry As New RegistryHandler
                    '    .MailFileScanDate = CType(myRegistry.ReadFromRegistry("MailScanDate_" & .Name), DateTime)
                    'Catch ex As Exception
                    '    .MailFileScanDate = Now.AddDays(-1)
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server MailFileScanDate not set, using default of 'Yesterday'.")
                    'End Try


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

                    'Try
                    '    If dr.Item("MailDirectory") Is Nothing Then
                    '        .MailDirectory = "mail"
                    '        .CountMailFiles = True
                    '        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server  mail directory not set, using default 'mail'.")
                    '    Else
                    '        .MailDirectory = dr.Item("MailDirectory")
                    '        .CountMailFiles = True
                    '        If .MailDirectory = "None" Then
                    '            .CountMailFiles = False
                    '            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server  mail directory will not be scanned.")
                    '        End If
                    '    End If
                    'Catch ex As Exception
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Domino server  mail directory not set, using default 'mail'.")
                    '    .MailDirectory = "mail"
                    'End Try


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

                    'Try
                    '    If dr.Item("ResponseThreshold") Is Nothing Then
                    '        .ResponseThreshold = 100
                    '    Else
                    '        .ResponseThreshold = dr.Item("ResponseThreshold")
                    '    End If
                    'Catch ex As Exception
                    '    .ResponseThreshold = 100
                    'End Try

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

                    '6/19/2015 NS added for VSPLUS-1802
                    Try
                        If entity.EXJournalEnabled Is Nothing Then
                            .EXJEnabled = False
                        Else
                            .EXJEnabled = entity.EXJournalEnabled
                        End If
                    Catch ex As Exception
                        .Location = False
                    End Try

                    Try
                        If entity.EXJournalStartTime Is Nothing Then
                            .EXJStartTime = ""
                        Else
                            .EXJStartTime = entity.EXJournalStartTime
                        End If
                    Catch ex As Exception
                        .EXJStartTime = ""
                    End Try

                    Try
                        If entity.EXJournalDuration Is Nothing Then
                            .EXJDuration = 0
                        Else
                            .EXJDuration = entity.EXJournalDuration
                        End If
                    Catch ex As Exception
                        .EXJDuration = 0
                    End Try

                    Try
                        If entity.EXJournalLookbackDuration Is Nothing Then
                            .EXJLookBackDuration = 0
                        Else
                            .EXJLookBackDuration = entity.EXJournalLookbackDuration
                        End If
                    Catch ex As Exception
                        .EXJLookBackDuration = 0
                    End Try

                    '************ Server Statistics *********************
                    'Create and add a collection of custom statistics to monitor
                    '****************************************************

                    'Dim dsDominoStatisticsSettings As New Data.DataSet
                    'Try
                    '    'Dim myCommand As New OleDb.OleDbCommand
                    '    'Dim myOleDbDataAdapter As New Data.OleDb.OleDbDataAdapter
                    '    'myCommand.CommandText = "SELECT StatName, ThresholdValue, GreaterThanOrLessThan, ConsoleCommand, TimesInARow " & _
                    '    '"FROM DominoCustomStatValues " & _
                    '    '"WHERE ServerName='" & .Name & "'"

                    '    ''WriteAuditEntry(Now.ToString & " Custom Statistics Select Command is: " & myCommand.CommandText)

                    '    'myCommand.Connection = OleDbConnectionServers
                    '    'myOleDbDataAdapter.SelectCommand = myCommand

                    '    'dsDominoStatisticsSettings.Clear()
                    '    'myOleDbDataAdapter.Fill(dsDominoStatisticsSettings, "Stats")
                    '    'myCommand.Dispose()
                    '    'myOleDbDataAdapter.Dispose()
                    '    Dim strSQL As String = "SELECT StatName, ThresholdValue, GreaterThanOrLessThan, ConsoleCommand, TimesInARow " &
                    '    "FROM DominoCustomStatValues " &
                    '    "WHERE ServerName='" & .Name & "'"
                    '    Dim objVSAdaptor As New VSAdaptor
                    '    objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsDominoStatisticsSettings, "Stats")

                    'Catch ex As Exception
                    '    WriteAuditEntry(Now.ToString & " Error Configuring Server Custom Statistics: " & ex.Message)
                    'End Try


                    'Try
                    '    Dim drTask As DataRow
                    '    If dsDominoStatisticsSettings.Tables("Stats").Rows.Count = 0 Then
                    '        If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " No custom stats defined for " & .Name)
                    '        dsDominoStatisticsSettings.Dispose()
                    '        Exit Try
                    '    End If
                    '    For Each drTask In dsDominoStatisticsSettings.Tables("Stats").Rows

                    '        Dim MyCustomStatistic As MonitoredItems.DominoCustomStatistic
                    '        'Check to see if this stat  is already configured
                    '        'If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Searching for statistic: " & drTask.Item("StatName"))
                    '        Try
                    '            MyCustomStatistic = MyDominoServer.CustomStatisticsSettings.Search(drTask.Item("StatName"))
                    '            'if not, add it to the server's collection
                    '        Catch ex As Exception
                    '            'an exception will be thrown if there are no servertaskSettings to search
                    '            'so we need to create a new blank collection that we can add to
                    '            MyCustomStatistic = Nothing
                    '            WriteAuditEntry(Now.ToString & " Exception while Searching for statistic: " & drTask.Item("StatName") & " " & ex.Message)
                    '        End Try

                    '        If MyCustomStatistic Is Nothing Then
                    '            '    WriteAuditEntry(Now.ToString & " Adding settings for istic trigger: " & MyCustomStatistic.Statistic & " " & MyCustomStatistic.ComparisonOperator & " " & MyCustomStatistic.ThresholdValue)
                    '            Try
                    '                Dim MyNewCustomStatisticSetting As New MonitoredItems.DominoCustomStatistic
                    '                'WriteAuditEntry(Now.ToString & " Adding Task: " & drTask.Item("TaskName"))
                    '                With MyNewCustomStatisticSetting
                    '                    .Statistic = drTask.Item("StatName")
                    '                    .ThresholdValue = drTask.Item("ThresholdValue")
                    '                    .ComparisonOperator = drTask.Item("GreaterThanOrLessThan")
                    '                    .RepeatThreshold = drTask.Item("TimesInARow")
                    '                    .ConsoleCommand = drTask.Item("ConsoleCommand")
                    '                End With
                    '                MyDominoServer.CustomStatisticsSettings.Add(MyNewCustomStatisticSetting)
                    '                If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Added a new Custom Statistic " & MyNewCustomStatisticSetting.Statistic & " " & MyNewCustomStatisticSetting.ComparisonOperator & " " & MyNewCustomStatisticSetting.ThresholdValue & " for " & MyDominoServer.Name)

                    '            Catch ex As Exception
                    '                WriteAuditEntry(Now.ToString & " Domino server " & .Name & " error configuring new custom statistic.  Error: " & ex.Message)
                    '            End Try

                    '        Else
                    '            Try
                    '                With MyCustomStatistic
                    '                    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Checking settings for existing Custom Statistic " & MyCustomStatistic.Statistic)
                    '                    .ThresholdValue = drTask.Item("ThresholdValue")
                    '                    .ComparisonOperator = drTask.Item("GreaterThanOrLessThan")
                    '                    .RepeatThreshold = drTask.Item("TimesInARow")
                    '                    .ConsoleCommand = drTask.Item("ConsoleCommand")
                    '                End With
                    '            Catch ex As Exception
                    '                WriteAuditEntry(Now.ToString & " Domino server " & .Name & " error configuring existing custom Statistic.  Error: " & ex.Message)
                    '            End Try

                    '        End If
                    '    Next
                    '    dsDominoStatisticsSettings.Dispose()
                    '    drTask = Nothing
                    'Catch ex As Exception
                    '    WriteAuditEntry(Now.ToString & " Error Configuring Domino custom statistics: " & ex.Message)
                    'End Try

                    'Try
                    '    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & .Name & " has " & .CustomStatisticsSettings.Count & " custom statistics defined.")

                    'Catch ex As Exception

                    'End Try

                    '***** Server Tasks
                    'Create and add the collection of server tasks that are monitored for this server
                    '******************

                    'If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " Updating server task settings-- start.")

                    'Dim myServerTaskSettings As New MonitoredItems.ServerTaskSettingCollection
                    'Dim dsDominoServerTaskSettings As New Data.DataSet
                    'Try
                    '    'Dim myCommand As New OleDb.OleDbCommand
                    '    'Dim myOleDbDataAdapter As New Data.OleDb.OleDbDataAdapter

                    '    'myCommand.CommandText = _
                    '    '" Select ServerTaskSettings.Enabled, DominoServerTasks.FreezeDetect, ServerTaskSettings.SendLoadCommand, ServerTaskSettings.SendRestartCommand, ServerTaskSettings.RestartOffHours,  ServerTaskSettings.SendExitCommand, DominoServerTasks.ConsoleString, DominoServerTasks.TaskName, DominoServerTasks.RetryCount, DominoServerTasks.MaxBusyTime,  DominoServerTasks.LoadString, ServerTaskSettings.MyID " & _
                    '    '  "FROM " & _
                    '    '  "ServerTaskSettings INNER JOIN DominoServerTasks " & _
                    '    '  "ON ServerTaskSettings.TaskID = DominoServerTasks.TaskID " & _
                    '    '  "Where ServerTaskSettings.ServerID = " & dr.Item("Key")

                    '    '' WriteAuditEntry(Now.ToString & " Join Command is: " & myCommand.CommandText)

                    '    'myCommand.Connection = OleDbConnectionServers
                    '    'myOleDbDataAdapter.SelectCommand = myCommand

                    '    'dsDominoServerTaskSettings.Clear()
                    '    'myOleDbDataAdapter.Fill(dsDominoServerTaskSettings, "Tasks")
                    '    'myCommand.Dispose()
                    '    'myOleDbDataAdapter.Dispose()
                    '    Dim strSQL As String = " Select ServerTaskSettings.Enabled, DominoServerTasks.FreezeDetect, ServerTaskSettings.SendLoadCommand, ServerTaskSettings.SendRestartCommand, ServerTaskSettings.RestartOffHours,  ServerTaskSettings.SendExitCommand, DominoServerTasks.ConsoleString, DominoServerTasks.TaskName, DominoServerTasks.RetryCount, DominoServerTasks.MaxBusyTime,  DominoServerTasks.LoadString, ServerTaskSettings.MyID " &
                    '      "FROM " &
                    '      "ServerTaskSettings INNER JOIN DominoServerTasks " &
                    '      "ON ServerTaskSettings.TaskID = DominoServerTasks.TaskID " &
                    '      "Where ServerTaskSettings.ServerID = " & dr.Item("Key")
                    '    Dim objVSAdaptor As New VSAdaptor
                    '    objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsDominoServerTaskSettings, "Tasks")

                    'Catch ex As Exception
                    '    WriteAuditEntry(Now.ToString & " First Error Configuring Server Tasks: " & ex.Message)
                    'End Try

                    'If .ServerTaskSettings Is Nothing Then
                    '    Dim myServerTaskCollection As New MonitoredItems.ServerTaskSettingCollection
                    '    .ServerTaskSettings = myServerTaskCollection
                    'Else
                    '    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " This server is configured to monitor " & .ServerTaskSettings.Count & " server tasks.")
                    '    If MyLogLevel = LogLevel.Verbose Then
                    '        For Each task As MonitoredItems.ServerTaskSetting In .ServerTaskSettings
                    '            WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " This server is configured to monitor " & task.Name)
                    '        Next
                    '    End If

                    'End If

                    'Try
                    '    Dim drTask As DataRow
                    '    For Each drTask In dsDominoServerTaskSettings.Tables("Tasks").Rows

                    '        Dim MyConfiguredDominoServerTaskSetting As MonitoredItems.ServerTaskSetting
                    '        MyConfiguredDominoServerTaskSetting = Nothing
                    '        'Check to see if this task is already configured
                    '        ' WriteAuditEntry(Now.ToString & " Searching for Task: " & drTask.Item("TaskName"))
                    '        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " Updating server task settings. Searching for Task: " & drTask.Item("TaskName"))

                    '        Try
                    '            MyConfiguredDominoServerTaskSetting = MyDominoServer.ServerTaskSettings.Search(drTask.Item("TaskName"))
                    '            'if not, add it to the server's collection
                    '            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " >> Found " & MyConfiguredDominoServerTaskSetting.Name)

                    '        Catch ex As Exception
                    '            'an exception will be thrown if there are no servertaskSettings to search
                    '            'so we need to create a new blank collection that we can add to
                    '            MyConfiguredDominoServerTaskSetting = Nothing

                    '        End Try

                    '        Try
                    '            ' If Trim(MyConfiguredDominoServerTaskSetting.Name) <> "" Then
                    '            If Not (MyConfiguredDominoServerTaskSetting) Is Nothing Then
                    '                With MyConfiguredDominoServerTaskSetting
                    '                    ' WriteAuditEntry(Now.ToString & " Checking settings for Task: " & MyConfiguredDominoServerTaskSetting.Name)
                    '                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Updating server task settings...now updating existing task: " & .Name)
                    '                    Try
                    '                        .Enabled = drTask.Item("Enabled")
                    '                    Catch ex As Exception

                    '                    End Try

                    '                    Try
                    '                        .LoadIfMissing = drTask.Item("SendLoadCommand")
                    '                        .ConsoleString = drTask.Item("ConsoleString")
                    '                        .RestartServerIfMissingASAP = drTask.Item("SendRestartCommand")
                    '                        .RestartServerIfMissingOFFHOURS = drTask.Item("RestartOffHours")
                    '                        .DisallowTask = drTask.Item("SendExitCommand")
                    '                        .LoadCommand = drTask.Item("LoadString")
                    '                        .FreezeDetection = drTask.Item("FreezeDetect")
                    '                        .FailureThreshold = drTask.Item("RetryCount")
                    '                    Catch ex As Exception
                    '                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino server " & MyDominoServer.Name & " error configuring existing task " & MyConfiguredDominoServerTaskSetting.Name & "  Error: " & ex.Message)
                    '                    End Try

                    '                End With
                    '            End If
                    '        Catch ex As Exception
                    '            WriteAuditEntry(Now.ToString & " Domino server " & .Name & " error configuring existing tasks.  Error: " & ex.Message)
                    '        End Try

                    '        Try
                    '            If MyConfiguredDominoServerTaskSetting Is Nothing Then

                    '                Dim MyNewDominoServerTaskSetting As New MonitoredItems.ServerTaskSetting
                    '                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " Updating server task settings.  Adding new task: " & drTask.Item("TaskName"))

                    '                'WriteAuditEntry(Now.ToString & " Adding Task: " & drTask.Item("TaskName"))
                    '                With MyNewDominoServerTaskSetting
                    '                    .Enabled = drTask.Item("Enabled")
                    '                    '    WriteAuditEntry(Now.ToString & " Enabled=" & .Enabled)
                    '                    .LoadIfMissing = drTask.Item("SendLoadCommand")
                    '                    '   WriteAuditEntry(Now.ToString & " SendLoadCommand=" & .LoadIfMissing)
                    '                    .Name = drTask.Item("TaskName")
                    '                    .FreezeDetection = drTask.Item("FreezeDetect")
                    '                    '  WriteAuditEntry(Now.ToString & " FreezeDetect=" & .FreezeDetection)
                    '                    .ConsoleString = drTask.Item("ConsoleString")
                    '                    ' WriteAuditEntry(Now.ToString & " ConsoleString=" & .ConsoleString)
                    '                    .RestartServerIfMissingASAP = drTask.Item("SendRestartCommand")
                    '                    .RestartServerIfMissingOFFHOURS = drTask.Item("RestartOffHours")
                    '                    'WriteAuditEntry(Now.ToString & " SendRestartCommand=" & .RestartServerIfMissing)
                    '                    .DisallowTask = drTask.Item("SendExitCommand")
                    '                    ' WriteAuditEntry(Now.ToString & " Disallow=" & .DisallowTask)
                    '                    .LoadCommand = drTask.Item("LoadString")
                    '                    .MaxRunTime = drTask.Item("MaxBusyTime")
                    '                    .FailureCount = 0
                    '                    .FailureThreshold = drTask.Item("RetryCount")
                    '                End With
                    '                MyDominoServer.ServerTaskSettings.Add(MyNewDominoServerTaskSetting)
                    '                '  WriteAuditEntry(Now.ToString & " Domino server " & .Name & " has " & MyDominoServer.ServerTaskSettings.Count & " tasks configured.")
                    '            End If
                    '        Catch ex As Exception
                    '            WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " error configuring new tasks.  Error: " & ex.Message)
                    '        End Try




                    '    Next

                    '    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " Updating server task settings-- end.")

                    '    drTask = Nothing
                    '    dsDominoServerTaskSettings.Dispose()
                    'Catch ex As Exception
                    '    WriteAuditEntry(Now.ToString & " Error Configuring Server Tasks: " & ex.Message)
                    'End Try

                End With

                MyDominoServer = Nothing

            Next


        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Domino servers error: " & exception.Message)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Domino servers error: " & ex.Message)
        End Try


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