﻿Imports System.Threading
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

        Try
            CreateSametimeServersCollection()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Sametime Servers collection: " & ex.Message)
        End Try
    End Sub


    Private Sub CreateSametimeServersCollection()
        'start with fresh data
        'Connect to the data source
        Dim mySecrets As New VSFramework.TripleDES

        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)

        Try

            'removed UserThreshold, ChatThreshold, NChatThreshold, PlacesThreshold

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Sametime.ToDescription())
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.RequireSSL) _
                .Include(Function(x) x.LocationId) _
                .Include(Function(x) x.Description) _
                .Include(Function(x) x.IsEnabled) _
                .Include(Function(x) x.IPAddress) _
                .Include(Function(x) x.ResponseTime) _
                .Include(Function(x) x.OffHoursScanInterval) _
                .Include(Function(x) x.ScanInterval) _
                .Include(Function(x) x.Category) _
                .Include(Function(x) x.RetryInterval) _
                .Include(Function(x) x.MeetingHostName) _
                .Include(Function(x) x.CollectMeetingStatistics) _
                .Include(Function(x) x.MeetingRequireSSL) _
                .Include(Function(x) x.MeetingPort) _
                .Include(Function(x) x.ConferenceRequireSSL) _
                .Include(Function(x) x.ConferencePort) _
                .Include(Function(x) x.ConferenceHostName) _
                .Include(Function(x) x.Platform) _
                .Include(Function(x) x.User1CredentialsId) _
                .Include(Function(x) x.User2CredentialsId) _
                .Include(Function(x) x.ExtendedStatisticsPort) _
                .Include(Function(x) x.CollectExtendedStatistics) _
                .Include(Function(x) x.TestChatSimulation) _
                .Include(Function(x) x.DatabaseSettingsHostName) _
                .Include(Function(x) x.DatabaseSettingsPort) _
                .Include(Function(x) x.DatabaseSettingsCredentialsId) _
                .Include(Function(x) x.DominoServerName) _
                 .Include(Function(x) x.LastStatsProcessedDate) _
                .Include(Function(x) x.CurrentNode)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()

            Dim repositoryStatus As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDefStatus As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Sametime.ToDescription())
            Dim projectionDefStatus As ProjectionDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Project _
                .Include(Function(x) x.StatusCode) _
                .Include(Function(x) x.CurrentStatus) _
                .Include(Function(x) x.LastUpdated) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.DeviceName)

            listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList()

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateSametimeServersCollection processing code. Exception: " & ex.Message)
            Exit Sub
        End Try



        '***
        'but first delete any that are in the collection but not in the database anymore
        'Delete propogation
        Dim MyServerNames As String

        If mySametimeServers.Count > 0 Then
            WriteAuditEntry(Now.ToString & " Checking to see if any ST databases should be deleted. ")
            'Get all the names of all the servers in the data table
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                MyServerNames += entity.DeviceName & "  "
            Next
        End If

        Dim ST As MonitoredItems.SametimeServer
        Dim myIndex As Integer

        If mySametimeServers.Count > 0 Then
            For myIndex = mySametimeServers.Count - 1 To 0 Step -1
                ST = mySametimeServers.Item(myIndex)
                Try
                    WriteAuditEntry(Now.ToString & " Checking to see if ST Server " & MySametimeServer.Name & " should be deleted...")
                    If InStr(MyServerNames, ST.Name) > 0 Then
                        'the server has not been deleted
                        WriteAuditEntry(Now.ToString & " " & ST.Name & " is not marked for deletion. ")
                    Else
                        'the server has been deleted, so delete from the collection
                        Try
                            mySametimeServers.Delete(ST.Name)
                            WriteAuditEntry(Now.ToString & " " & ST.Name & " has been deleted by the service.")
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " " & ST.Name & " was not deleted by the service because " & ex.Message)
                        End Try
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception ST servers Deletion Loop on " & ST.Name & ".  The error was " & ex.Message)
                End Try
            Next
        End If

        MyServerNames = Nothing
        ST = Nothing
        myIndex = Nothing

        '*** End delete propogation

        Dim i As Integer = 0
        'Add the Sametime servers to the collection
        Try
            If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & "  Reading configuration settings for " & listOfServers.Count.ToString() & " Sametime Servers.")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error with sametime table " & ex.Message, LogLevel.Normal)
        End Try
        Try
            Dim myString As String = ""
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                i += 1
                Dim MyName As String
                If entity.DeviceName Is Nothing Then
                    MyName = "Sametime Server #" & i.ToString
                Else
                    MyName = entity.DeviceName
                End If
                'See if this server is already in the collection; if so, update its settings otherwise create a new one
                MySametimeServer = mySametimeServers.SearchByName(MyName)
                If MySametimeServer Is Nothing Then
                    Try
                        MySametimeServer = New MonitoredItems.SametimeServer
                        MySametimeServer.Name = MyName
                        MySametimeServer.LastScan = Now.AddMinutes(-30)
                        MySametimeServer.NextScan = Now
                        MySametimeServer.AlertCondition = False
                        MySametimeServer.Status = "Not Scanned"
                        MySametimeServer.IncrementUpCount()
                        MySametimeServer.ServerType = VSNext.Mongo.Entities.Enums.ServerType.Sametime.ToDescription()
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error adding new empty stats collection to " & MySametimeServer.Name)
                    End Try
                    mySametimeServers.Add(MySametimeServer)
                    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Adding new Sametime server -- " & MySametimeServer.Name & " -- to the collection.")
                Else
                    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Updating settings for existing Sametime server-- " & MySametimeServer.Name & ".")
                End If

                With MySametimeServer

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

                    'Sametime Specific
                    Try
                        If entity.RequireSSL Then
                            .SSL = True
                        Else
                            .SSL = False
                        End If
                    Catch ex As Exception
                        .SSL = False
                    End Try

                    'BEGIN Running Processes to monitor *****************
                    'Dim myMonitoredServices As New MonitoredItems.SametimeMonitoredProcessCollection

                    'Try
                    '    If dr.Item("nserver") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "nserver"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stcommlaunch") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stcommlaunch"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stcommunity") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stcommunity"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try


                    'Try
                    '    If dr.Item("stconfigurationapp") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stconfigurationapp"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stplaces") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stplaces"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stmux") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stmux"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stusers") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stusers"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stonlinedir") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stonlinedir"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stdirectory") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stdirectory"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stlogger") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stlogger"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try


                    'Try
                    '    If dr.Item("stlinks") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stlinks"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try


                    'Try
                    '    If dr.Item("stprivacy") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stprivacy"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stsecurity") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stsecurity"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stpresencemgr") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stpresencemgr"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stservicemanager") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stservicemanager"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stpresencesubmgr") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stpresencesubmgr"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try


                    'Try
                    '    If dr.Item("steventserver") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "steventserver"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stpolicy") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stpolicy"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stconfigurationbridge") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stconfigurationbridge"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try


                    'Try
                    '    If dr.Item("stadminsrv") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stadminsrv"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stuserstorage") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stuserstorage"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try
                    'Try
                    '    If dr.Item("stchatlogging") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stchatlogging"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stpolling") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stpolling"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stresolve") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stresolve"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try


                    'Try
                    '    If dr.Item("stpresencecompatmgr") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stpresencecompatmgr"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If myMonitoredServices.Count > 0 Then
                    '        MySametimeServer.MonitoredProcesses = myMonitoredServices
                    '    End If

                    'Catch ex As Exception

                    'End Try

                    'END Running Processes to monitor *****************
                    'Try
                    '    WriteAuditEntry(Now.ToString & " The following processes will be monitored for this Sametime server: ")
                    '    For Each Process As MonitoredItems.SametimeMonitoredProcess In MySametimeServer.MonitoredProcesses
                    '        WriteAuditEntry(Now.ToString & " " & Process.Name)
                    '    Next
                    'Catch ex As Exception

                    'End Try


                    'Standard attributes

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
                        If entity.Description Is Nothing Then
                            .Description = ""
                        Else
                            .Description = entity.Description
                        End If
                    Catch ex As Exception
                        .Description = ""
                    End Try

                    Try
                        .OffHours = False
                    Catch ex As Exception
                        .OffHours = False
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
                        If entity.IPAddress Is Nothing Then
                            .IPAddress = ""
                        Else
                            .IPAddress = entity.IPAddress
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Invalid IP Address")
                        .IPAddress = ""
                        .Status = "Invalid IP Address"
                        .Enabled = True
                    End Try

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
                        If entity.DominoServerName Is Nothing Then
                            .DominoServerName = ""
                        Else
                            .DominoServerName = entity.DominoServerName
                        End If
                    Catch ex As Exception
                        .DominoServerName = ""
                    End Try

                    Try
                        If entity.LastStatsProcessedDate Is Nothing Then
                            .LastStatsProcessedDate = Date.MinValue
                        Else
                            .LastStatsProcessedDate = DateTime.Parse(entity.LastStatsProcessedDate)
                        End If
                    Catch ex As Exception
                        .LastStatsProcessedDate = Date.MinValue
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
                                .LastScan = Now.AddMinutes(-30)
                            Else
                                .LastScan = entityStatus.LastUpdated
                            End If
                        Catch ex As Exception
                            .LastScan = Now.AddMinutes(-30)
                        End Try

                    Catch ex As Exception

                    End Try

                    'Try
                    '    If dr.Item("CurrentNodeID") Is Nothing Then
                    '        .InsufficentLicenses = True
                    '    Else
                    '        If dr.Item("CurrentNodeID").ToString() = "-1" Then
                    '            .InsufficentLicenses = True
                    '        Else
                    '            .InsufficentLicenses = False
                    '        End If

                    '    End If
                    'Catch ex As Exception
                    '    '7/8/2015 NS modified for VSPLUS-1959
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Sametime insufficient licenses not set.")

                    'End Try

                    Try
                        If entity.MeetingHostName Is Nothing Then
                            .WsMeetingHost = ""
                        Else
                            .WsMeetingHost = entity.MeetingHostName
                        End If
                    Catch ex As Exception
                        .WsMeetingHost = ""
                    End Try

                    Try
                        If entity.CollectMeetingStatistics Is Nothing Then
                            .WsScanMeetingServer = False
                        Else
                            .WsScanMeetingServer = entity.CollectMeetingStatistics
                        End If
                    Catch ex As Exception
                        .WsScanMeetingServer = False
                    End Try

                    Try
                        If entity.MeetingRequireSSL Is Nothing Then
                            .WsMeetingRequireSSL = False
                        Else
                            .WsMeetingRequireSSL = entity.MeetingRequireSSL
                        End If
                    Catch ex As Exception
                        .WsMeetingRequireSSL = False
                    End Try

                    Try
                        .WsMeetingPort = entity.MeetingPort
                    Catch ex As Exception
                        .WsMeetingPort = "80"
                    End Try


                    Try
                        If entity.CollectConferenceStatistics Is Nothing Then
                            .WsScanMediaServer = False
                        Else
                            .WsScanMediaServer = entity.CollectConferenceStatistics
                        End If
                    Catch ex As Exception
                        .WsScanMediaServer = False
                    End Try

                    Try
                        If entity.ConferenceRequireSSL Is Nothing Then
                            .WsMediaRequireSSL = False
                        Else
                            .WsMediaRequireSSL = entity.ConferenceRequireSSL
                        End If
                    Catch ex As Exception
                        .WsMediaRequireSSL = False
                    End Try

                    Try
                        .WsMediaPort = entity.ConferencePort
                    Catch ex As Exception
                        .WsMediaPort = "80"
                    End Try

                    .WsMediaHost = entity.ConferenceHostName

                    .Platform = entity.Platform
                    Dim myRegistry As New VSFramework.RegistryHandler()
                    Dim userOne As String = "", userTwo As String = "", pass1 As Byte(), pass2 As Byte()
                    Dim strEncryptedPassword As String = ""

                    Try

                        Dim repositoryCredentials As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Credentials)(connectionString)
                        Dim filterDefCredentials As FilterDefinition(Of VSNext.Mongo.Entities.Credentials) = repositoryCredentials.Filter.Eq(Function(x) x.Id, entity.User1CredentialsId)
                        Dim entityCredentials As VSNext.Mongo.Entities.Credentials = repositoryCredentials.Find(filterDefCredentials).ToList()(0)

                        userOne = entityCredentials.UserId
                        WriteAuditEntry(Now.ToString & " Sametime User One is " & userOne)
                        WriteAuditEntry(Now.ToString & " Sametime User One pwd is " & entityCredentials.Password)
                        strEncryptedPassword = entityCredentials.Password   'sametime password as encrypted byte stream
                        Try
                            Dim str1() As String
                            str1 = strEncryptedPassword.Split(",")
                            Dim bstr1(str1.Length - 1) As Byte
                            For j As Integer = 0 To str1.Length - 1
                                bstr1(j) = str1(j).ToString()
                            Next
                            pass1 = bstr1
                        Catch ex As Exception

                        End Try

                        If Not pass1 Is Nothing Then
                            .Password1 = mySecrets.Decrypt(pass1)  'password in clear text, stored in memory now
                        Else
                            .Password1 = ""
                        End If
                        .UserId1 = userOne
                        WriteAuditEntry(Now.ToString & " Sametime User One decrypt pwd is " & entity.Password)
                    Catch ex As Exception
                        .UserId1 = ""
                        WriteAuditEntry(Now.ToString & " Cannot Set user id1 " + ex.ToString(), LogLevel.Normal)
                    End Try

                    Try

                        Dim repositoryCredentials As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Credentials)(connectionString)
                        Dim filterDefCredentials As FilterDefinition(Of VSNext.Mongo.Entities.Credentials) = repositoryCredentials.Filter.Eq(Function(x) x.Id, entity.User2CredentialsId)
                        Dim entityCredentials As VSNext.Mongo.Entities.Credentials = repositoryCredentials.Find(filterDefCredentials).ToList()(0)

                        userTwo = entityCredentials.UserId
                        WriteAuditEntry(Now.ToString & " Sametime User two is " & userTwo)
                        WriteAuditEntry(Now.ToString & " Sametime User One is " & entity.Password)
                        strEncryptedPassword = entityCredentials.Password  'sametime password as encrypted byte stream
                        Try
                            Dim str1() As String
                            str1 = strEncryptedPassword.Split(",")
                            Dim bstr1(str1.Length - 1) As Byte
                            For j As Integer = 0 To str1.Length - 1
                                bstr1(j) = str1(j).ToString()
                            Next
                            pass2 = bstr1
                        Catch ex As Exception

                        End Try

                        If Not pass2 Is Nothing Then
                            .Password2 = mySecrets.Decrypt(pass2)  'password in clear text, stored in memory now
                        Else
                            .Password2 = ""
                        End If
                        .UserId2 = userTwo
                    Catch ex As Exception
                        .UserId2 = ""
                        WriteAuditEntry(Now.ToString & " Cannot Set user id2 ", LogLevel.Normal)
                    End Try

                    Try
                        .ExtendedChatPort = entity.ExtendedStatisticsPort
                    Catch ex As Exception
                        .ExtendedChatPort = "80"
                    End Try



                    Try
                        If entity.CollectExtendedStatistics Is Nothing Then
                            .CollectExtendedChat = False
                        Else
                            .CollectExtendedChat = entity.CollectExtendedStatistics
                        End If
                    Catch ex As Exception
                        .CollectExtendedChat = False
                    End Try

                    Try
                        If entity.TestChatSimulation Is Nothing Then
                            .TestChatSimulation = False
                        Else
                            .TestChatSimulation = entity.TestChatSimulation
                        End If
                    Catch ex As Exception
                        .TestChatSimulation = False
                    End Try

                    Try
                        If entity.DatabaseSettingsHostName Is Nothing Then
                            .DBHostName = ""
                        Else
                            .DBHostName = entity.DatabaseSettingsHostName
                        End If
                    Catch ex As Exception
                        .DBHostName = ""
                    End Try

                    Try
                        If entity.DatabaseSettingsPort Is Nothing Then
                            .DBPort = 0
                        Else
                            .DBPort = entity.DatabaseSettingsPort
                        End If
                    Catch ex As Exception
                        .DBPort = 0
                    End Try

                    Try

                        Dim repositoryCredentials As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Credentials)(connectionString)
                        Dim filterDefCredentials As FilterDefinition(Of VSNext.Mongo.Entities.Credentials) = repositoryCredentials.Filter.Eq(Function(x) x.Id, entity.DatabaseSettingsCredentialsId)
                        Dim entityCredentials As VSNext.Mongo.Entities.Credentials = repositoryCredentials.Find(filterDefCredentials).ToList()(0)

                        Dim user As String = entity.Username
                        WriteAuditEntry(Now.ToString & " Sametime User two is " & user)
                        WriteAuditEntry(Now.ToString & " Sametime User One is " & entity.Password)
                        strEncryptedPassword = entity.Password  'sametime password as encrypted byte stream
                        Try
                            Dim str1() As String
                            str1 = strEncryptedPassword.Split(",")
                            Dim bstr1(str1.Length - 1) As Byte
                            For j As Integer = 0 To str1.Length - 1
                                bstr1(j) = str1(j).ToString()
                            Next
                            pass2 = bstr1
                        Catch ex As Exception

                        End Try

                        If Not pass2 Is Nothing Then
                            .DBPassword = mySecrets.Decrypt(pass2)  'password in clear text, stored in memory now
                        Else
                            .DBPassword = ""
                        End If
                        .DBUserName = user
                    Catch ex As Exception
                        .DBUserName = ""
                        WriteAuditEntry(Now.ToString & " Cannot Set user db2 password ", LogLevel.Normal)
                    End Try

                    Try
                        If entity.CurrentNode Is Nothing Then
                            .InsufficentLicenses = True
                        Else
                            If entity.CurrentNode.ToString() = getCurrentNode() Then
                                .InsufficentLicenses = True
                            Else
                                .InsufficentLicenses = False
                            End If
                        End If
                        .CurrentNode = entity.CurrentNode
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Sametime Servers insufficient licenses not set.")
                    End Try

                End With
                MySametimeServer = Nothing
            Next

        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Sametime Servers data exception " & exception.Message)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Sametime Servers general exception " & ex.ToString)
        End Try

        InsufficentLicensesTest(mySametimeServers)

    End Sub


    Private Sub CreateNotesDatabaseCollection()
        'start with fresh data

        'Connect to the data source

        WriteAuditEntry(vbCrLf & Now.ToString & " Creating a dataset in CreateNotesDatabaseCollection." & vbCrLf)
        Dim listOfNotesDatabases As New List(Of VSNext.Mongo.Entities.ServerOther)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)
        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.ServerOther)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.ServerOther) = repository.Filter.Eq(Function(x) x.Type, VSNext.Mongo.Entities.Enums.ServerType.NotesDatabase.ToDescription())
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.ServerOther) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.Name) _
                .Include(Function(x) x.Type) _
                .Include(Function(x) x.Category) _
                .Include(Function(x) x.DominoServerName) _
                .Include(Function(x) x.ScanInterval) _
                .Include(Function(x) x.IsEnabled) _
                .Include(Function(x) x.OffHoursScanInterval) _
                .Include(Function(x) x.RetryInterval) _
                .Include(Function(x) x.DatabaseFileName) _
                .Include(Function(x) x.TriggerType) _
                .Include(Function(x) x.TriggerValue) _
                .Include(Function(x) x.InitiateReplication) _
                .Include(Function(x) x.ReplicationDestination) _
                .Include(Function(x) x.CurrentNode)
            listOfNotesDatabases = repository.Find(filterDef, projectionDef).ToList()

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

        WriteAuditEntry(Now.ToString & " There are " & listOfNotesDatabases.Count().ToString() & " Notes Database servers found in the database", LogLevel.Normal)

        '***
        'but first delete any that are in the collection but not in the database anymore
        'Delete propagation
        Dim MyServerNames As String = ""
        If MyNotesDatabases.Count > 0 Then
            WriteAuditEntry(Now.ToString & " Checking to see if any Notes databases should be deleted. ")
            'Get all the names of all the servers in the data table
            For Each entity As VSNext.Mongo.Entities.ServerOther In listOfNotesDatabases
                MyServerNames += entity.Name & "  "
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
            For Each entity As VSNext.Mongo.Entities.ServerOther In listOfNotesDatabases
                i += 1
                Dim MyName As String
                MyName = entity.Name

                'If InStr(MyName, "'") > 0 Then
                '    MyName = MyName.Replace("'", "")
                'End If

                'Dim Quote As Char
                'Quote = Chr(34)

                'If InStr(MyName, Quote) > 0 Then
                '    MyName = MyName.Replace(Quote, "~")
                'End If

                If MyName Is Nothing Then
                    MyName = "Notes Database" & i.ToString
                End If

                'See if this server is already in the collection; if so, update its settings otherwise create a new one
                MyNotesDatabase = MyNotesDatabases.Search(MyName)
                If MyNotesDatabase Is Nothing Then
                    MyNotesDatabase = New MonitoredItems.NotesDatabase
                    MyNotesDatabase.Name = MyName
                    MyNotesDatabase.LastScan = Now.AddMinutes(-30)
                    MyNotesDatabase.NextScan = Now
                    MyNotesDatabase.IncrementUpCount()
                    MyNotesDatabase.AlertCondition = False
                    MyNotesDatabase.Status = "Not Scanned"
                    MyNotesDatabase.ServerType = VSNext.Mongo.Entities.Enums.ServerType.NotesDatabase.ToDescription()
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
                        If entity.DominoServerName Is Nothing Then
                            .ServerName = ""
                            WriteAuditEntry(Now.ToString & " Error: No Server Name specified for " & .Name)
                        Else
                            .ServerName = entity.DominoServerName
                        End If
                    Catch ex As Exception
                        .ServerName = ""
                        .Enabled = False
                        WriteAuditEntry(Now.ToString & " Error:  No Server Name specified for " & .Name)
                    End Try

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

                    If listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.Name) And x.DeviceType.Equals(entity.Type)).ToList().Count() > 0 Then

                        Dim entityStatus As VSNext.Mongo.Entities.Status = listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.Name) And x.DeviceType.Equals(entity.Type)).ToList()(0)

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
                                .LastScan = Now.AddMinutes(-30)
                            Else
                                .LastScan = entityStatus.LastUpdated
                            End If
                        Catch ex As Exception
                            .LastScan = Now.AddMinutes(-30)
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
                        .CurrentNode = entity.CurrentNode
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
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.TravelerHaDatastore.ToDescription())
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
        Dim listOfCustomStats As New List(Of VSNext.Mongo.Entities.ServerOther)
        Dim listOfDominoServerTasks As New List(Of VSNext.Mongo.Entities.DominoServerTasks)

        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
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
                .Include(Function(x) x.ServerTasks) _
                .Include(Function(x) x.CurrentNode)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()

            'Make a collection of the possible status entries
            Dim repositoryStatus As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDefStatus As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
            Dim projectionDefStatus As ProjectionDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Project _
                .Include(Function(x) x.StatusCode) _
                .Include(Function(x) x.CurrentStatus) _
                .Include(Function(x) x.LastUpdated) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.Details)

            listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList()

            'Make a collection of all the Domino custom stats
            Dim repositoryServerOther As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.ServerOther)(connectionString)
            Dim filterDefServerOther As FilterDefinition(Of VSNext.Mongo.Entities.ServerOther) = repositoryServerOther.Filter.Eq(Function(x) x.Type, VSNext.Mongo.Entities.Enums.ServerType.DominoCustomStatistic.ToDescription())
            Dim projectionDefServerOther As ProjectionDefinition(Of VSNext.Mongo.Entities.ServerOther) = repositoryServerOther.Project _
                .Include(Function(x) x.StatName) _
                .Include(Function(x) x.ThresholdValue) _
                .Include(Function(x) x.GreaterThanOrLessThan) _
                .Include(Function(x) x.TimesInARow) _
                .Include(Function(x) x.DominoServers) _
                .Include(Function(x) x.ConsoleCommand)

            listOfCustomStats = repositoryServerOther.Find(filterDefServerOther, projectionDefServerOther).ToList()

            Dim repositoryDominoServerTasks As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DominoServerTasks)(connectionString)
            listOfDominoServerTasks = repositoryDominoServerTasks.Find(Function(x) True).ToList()

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateDominoServersCollection processing code. Exception: " & ex.Message)
            Exit Sub
        End Try

        WriteAuditEntry(Now.ToString & " There are " & listOfServers.Count().ToString() & " Domino servers found in the database", LogLevel.Normal)

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
                            .CPU_Threshold = 0.9
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold Not set, using default of 50.")
                        Else
                            .CPU_Threshold = entity.CpuThreshold
                        End If
                    Catch ex As Exception
                        .CPU_Threshold = 0.9
                        WriteAuditEntry(Now.ToString & " " & .Name & " Domino server CPU Utilization threshold Not set, using default of 90%.")
                    End Try

                    Try
                        If entity.MemoryThreshold Is Nothing Then
                            .Memory_Threshold = 0.9
                            '  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold Not set, using default of 50.")
                        Else
                            .Memory_Threshold = entity.MemoryThreshold
                        End If
                    Catch ex As Exception
                        .Memory_Threshold = 0.9
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
                        If entity.ResponseTime Is Nothing Then
                            .ResponseThreshold = 100
                        Else
                            .ResponseThreshold = entity.ResponseTime
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
                            If entityStatus.StatusCode Is Nothing Then
                                .Status = "Maintenance"
                            Else
                                .Status = entityStatus.StatusCode
                            End If
                        Catch ex As Exception
                            .Status = "Maintenance"
                        End Try

                        Try
                            If entityStatus.LastUpdated Is Nothing Then
                                .LastScan = Now.AddMinutes(30)
                            Else
                                .LastScan = entityStatus.LastUpdated
                            End If
                        Catch ex As Exception
                            .LastScan = Now.AddMinutes(30)
                        End Try

                        Try
                            If entityStatus.Details Is Nothing Then

                            Else
                                .ResponseDetails = entityStatus.Details
                            End If
                        Catch ex As Exception

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

                        'Create a list of only those custom stats which are applicable to this server
                        Dim listCurrentCustomStats As List(Of VSNext.Mongo.Entities.ServerOther) = listOfCustomStats.Where(Function(x) x.DominoServers IsNot Nothing).Where(Function(x) x.DominoServers.Contains(entity.Id)).ToList()


                        For Each customStat As VSNext.Mongo.Entities.ServerOther In listCurrentCustomStats

                            Dim MyCustomStatistic As MonitoredItems.DominoCustomStatistic
                            'Check to see if this stat  is already configured
                            ' WriteAuditEntry(Now.ToString & " Found custom statistic: " & customStat.StatName)
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
                                        .ComparisonOperator = IIf(customStat.TypeOfStatistic = "String", customStat.EqualOrNotEqual, customStat.GreaterThanOrLessThan)
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
                                        .ComparisonOperator = IIf(customStat.TypeOfStatistic = "String", customStat.EqualOrNotEqual, customStat.GreaterThanOrLessThan)
                                        .RepeatThreshold = customStat.TimesInARow
                                        .ConsoleCommand = customStat.ConsoleCommand
                                    End With
                                Catch ex As Exception
                                    WriteAuditEntry(Now.ToString & " Domino server " & .Name & " error configuring existing custom Statistic.  Error:  " & ex.Message)
                                End Try

                            End If
                        Next

                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error Configuring Domino custom statistics: " & ex.Message, LogLevel.Verbose)
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
                                        If Task.Monitored = False Then
                                            WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " Task is disabled. Will remove it from collection.", LogLevel.Verbose)
                                            If Not (MyConfiguredDominoServerTaskSetting Is Nothing) Then
                                                For i = 0 To MyDominoServer.ServerTaskSettings.Count - 1
                                                    If MyDominoServer.ServerTaskSettings(i) = MyConfiguredDominoServerTaskSetting Then
                                                        WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " Found entry at index " & i, LogLevel.Verbose)
                                                        MyDominoServer.ServerTaskSettings.RemoveAt(i)
                                                        Exit For
                                                    End If
                                                    If i = MyDominoServer.ServerTaskSettings.Count - 1 Then
                                                        WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " No entry found.", LogLevel.Verbose)
                                                    End If
                                                Next
                                            Else
                                                WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " No entry found so nothing to remove.", LogLevel.Verbose)
                                            End If
                                        Else

                                            Try
                                                ' If Trim(MyConfiguredDominoServerTaskSetting.Name) <> "" Then
                                                If Not (MyConfiguredDominoServerTaskSetting) Is Nothing Then
                                                    With MyConfiguredDominoServerTaskSetting
                                                        ' WriteAuditEntry(Now.ToString & " Checking settings for Task: " & MyConfiguredDominoServerTaskSetting.Name)
                                                        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Updating server task settings...now updating existing task: " & .Name, LogLevel.Verbose)

                                                        Dim dominoServerTask As VSNext.Mongo.Entities.DominoServerTasks
                                                        Try
                                                            dominoServerTask = listOfDominoServerTasks.Where(Function(x) x.Id = Task.TaskId)(0)
                                                        Catch ex As Exception
                                                            dominoServerTask = New DominoServerTasks()
                                                        End Try

                                                        Try
                                                            .Enabled = If(Task.Monitored, False)
                                                        Catch ex As Exception

                                                        End Try

                                                        Try
                                                            .LoadIfMissing = If(Task.SendLoadCmd, False)
                                                            .ConsoleString = dominoServerTask.ConsoleString
                                                            .RestartServerIfMissingASAP = If(Task.SendRestartCmd, False)
                                                            .RestartServerIfMissingOFFHOURS = If(Task.SendRestartCmdOffhours, False)
                                                            .DisallowTask = If(Task.SendExitCmd, False)
                                                            .LoadCommand = dominoServerTask.LoadString
                                                            .FreezeDetection = dominoServerTask.FreezeDetect
                                                            .FailureThreshold = dominoServerTask.RetryCount
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
                                                    'If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " Updating server task settings.  Adding new task: " & Task.TaskName)

                                                    If InStr(Task.TaskName, "Traveler") Then
                                                        WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " This server is configured to monitor Traveler, so it MUST BE a Traveler server.")
                                                        .Traveler_Server = True
                                                        .SecondaryRole = "Traveler"
                                                    End If

                                                    Dim dominoServerTask As VSNext.Mongo.Entities.DominoServerTasks
                                                    Try
                                                        dominoServerTask = listOfDominoServerTasks.Where(Function(x) x.Id = Task.TaskId)(0)
                                                    Catch ex As Exception
                                                        dominoServerTask = New DominoServerTasks()
                                                    End Try

                                                    'WriteAuditEntry(Now.ToString & " Adding Task: " & drTask.Item("TaskName"))
                                                    With MyNewDominoServerTaskSetting
                                                        .Enabled = If(Task.Monitored, False)
                                                        '    WriteAuditEntry(Now.ToString & " Enabled=" & .Enabled)
                                                        .LoadIfMissing = If(Task.SendLoadCmd, False)
                                                        '   WriteAuditEntry(Now.ToString & " SendLoadCommand=" & .LoadIfMissing)
                                                        .Name = Task.TaskName
                                                        .FreezeDetection = dominoServerTask.FreezeDetect
                                                        '  WriteAuditEntry(Now.ToString & " FreezeDetect=" & .FreezeDetection)
                                                        .ConsoleString = dominoServerTask.ConsoleString
                                                        ' WriteAuditEntry(Now.ToString & " ConsoleString=" & .ConsoleString)
                                                        .RestartServerIfMissingASAP = If(Task.SendRestartCmd, False)
                                                        .RestartServerIfMissingOFFHOURS = If(Task.SendRestartCmdOffhours, False)
                                                        'WriteAuditEntry(Now.ToString & " SendRestartCommand=" & .RestartServerIfMissing)
                                                        .DisallowTask = If(Task.SendExitCmd, False)
                                                        ' WriteAuditEntry(Now.ToString & " Disallow=" & .DisallowTask)
                                                        .LoadCommand = dominoServerTask.LoadString
                                                        .MaxRunTime = dominoServerTask.MaxBusyTime
                                                        .FailureCount = 0
                                                        .FailureThreshold = dominoServerTask.RetryCount
                                                    End With
                                                    MyDominoServer.ServerTaskSettings.Add(MyNewDominoServerTaskSetting)
                                                    '  WriteAuditEntry(Now.ToString & " Domino server " & .Name & " has " & MyDominoServer.ServerTaskSettings.Count & " tasks configured.")
                                                End If
                                            Catch ex As Exception
                                                WriteDeviceHistoryEntry("Domino", .Name, Now.ToString & " error configuring New tasks.  Error:  " & ex.Message)
                                            End Try

                                        End If
                                    Catch ex As Exception

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
                            If entity.CurrentNode.ToString() = getCurrentNode() Then
                                .InsufficentLicenses = True
                            Else
                                .InsufficentLicenses = False
                            End If

                        End If
                        .CurrentNode = entity.CurrentNode
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
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.NotesMailProbe.ToDescription())
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
                .Include(Function(x) x.CurrentNode) _
                .Include(Function(x) x.ImapHostName) _
                .Include(Function(x) x.CredentialsId)

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
                                .LastScan = Now.AddMinutes(-30)
                                WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe  scan interval not set, using default of Now.")
                            Else
                                .LastScan = entityStatus.LastUpdated
                            End If
                        Catch ex As Exception
                            .LastScan = Now.AddMinutes(-30)
                            WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probe scan interval not set, using default of Now.")
                        End Try

                    Catch ex As Exception

                    End Try


                    Try
                        If entity.CurrentNode Is Nothing Then
                            .InsufficentLicenses = True
                        Else
                            If entity.CurrentNode.ToString() = getCurrentNode() Then
                                .InsufficentLicenses = True
                            Else
                                .InsufficentLicenses = False
                            End If

                        End If
                        .CurrentNode = entity.CurrentNode
                    Catch ex As Exception
                        '7/8/2015 NS modified for VSPLUS-1959
                        WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probes insufficient licenses not set.")

                    End Try

                    Try
                        If entity.ImapHostName Is Nothing Then
                            .ImapHostName = ""
                        Else
                            .ImapHostName = entity.ImapHostName
                        End If
                    Catch ex As Exception
                        '7/8/2015 NS modified for VSPLUS-1959
                        WriteAuditEntry(Now.ToString & " " & .Name & " NotesMail Probes ImapHostName not set.")

                    End Try

                    Try
                        WriteAuditEntry(Now.ToString & " Getting server credentials.", LogLevel.Verbose)
                        If entity.CredentialsId Is Nothing Then
                            .ImapUserName = ""
                            .ImapPassword = ""
                        Else
                            'Run a query here, then parse the results

                            Dim repositoryCredentials As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Credentials)(connectionString)
                            Dim filterDefCredentials As FilterDefinition(Of VSNext.Mongo.Entities.Credentials) = repositoryCredentials.Filter.Eq(Function(x) x.Id, entity.CredentialsId)
                            Dim entityCredentials As VSNext.Mongo.Entities.Credentials = repositoryCredentials.Find(filterDefCredentials).ToList()(0)

                            .ImapUserName = entityCredentials.UserId
                            WriteAuditEntry(Now.ToString & " IMAP Username is " & .ImapUserName, LogLevel.Verbose)

                            Dim mySecrets As New VSFramework.TripleDES
                            Try
                                .ImapPassword = mySecrets.Decrypt(entityCredentials.Password)
                            Catch ex As Exception
                                .ImapPassword = ""
                                WriteAuditEntry(Now.ToString & " Error decrypting the password.  " & ex.ToString)
                            End Try
                        End If
                    Catch ex As Exception

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
        Dim listOfServers As New List(Of VSNext.Mongo.Entities.ServerOther)
        Dim listOfDominoServers As New List(Of VSNext.Mongo.Entities.Server)
        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.ServerOther)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.ServerOther) = repository.Filter.Eq(Function(x) x.Type, VSNext.Mongo.Entities.Enums.ServerType.DominoLogScanning.ToDescription())
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.ServerOther) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.Name) _
                .Include(Function(x) x.Type) _
                .Include(Function(x) x.LogFileServers) _
                .Include(Function(x) x.LogFileKeywords)
            listOfServers = repository.Find(filterDef, projectionDef).ToList()

            Dim repositoryServers As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDefServers As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repositoryServers.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
            Dim projectionDefServers As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repositoryServers.Project.Include(Function(x) x.DeviceName).Include(Function(x) x.Id)
            listOfDominoServers = repositoryServers.Find(filterDefServers, projectionDefServers).ToList()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in Create Keywords Collection processing code. Exception: " & ex.Message)
            '  Exit Sub
        End Try

        'Add the keywords to the collection
        WriteAuditEntry(Now.ToString & "  Reading configuration settings for Domino log file keywords.")
        Try

            For Each entity As VSNext.Mongo.Entities.ServerOther In listOfServers
                WriteAuditEntry(Now.ToString & "  Processing keyword: " & entity.Name)

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

                            MyKeyword.ServerName = listOfDominoServers.Where(Function(x) x.Id = serverObjectId).ToList()(0).DeviceName
                            'added Server Name'
                            myKeywords.Add(MyKeyword)
                            If MyKeyword.ScanAgentLog = True Then
                                WriteAuditEntry(Now.ToString & "  Watching Domino agent log file for " & MyKeyword.Keyword, LogLevel.Verbose)
                            End If

                            If MyKeyword.ScanLog = True Then
                                WriteAuditEntry(Now.ToString & "  Watching Domino log file for " & MyKeyword.Keyword, LogLevel.Verbose)
                            End If
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Exception creating Domino log file collection: " & ex.Message)
                        End Try
                    Next
                Next

            Next
            ' ReDim Preserve Keywords(i - 1)

        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Data Exception creating Domino log file collection: " & exception.Message)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Random Exception creating Domino log file collection:  " & ex.Message)
        End Try

    End Sub

    Private Sub CreateDominoClusterCollection()

        'start with fresh data
        '***  Build the data set dynamically
        Dim listOfDatabaseReplicas As New List(Of VSNext.Mongo.Entities.ServerOther)
        Dim listOfDominoServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)

        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.ServerOther)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.ServerOther) = repository.Filter.Eq(Function(x) x.Type, VSNext.Mongo.Entities.Enums.ServerType.NotesDatabaseReplica.ToDescription()) _
                 And repository.Filter.In(Function(x) x.CurrentNode, {getCurrentNode(), "-1"})
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.ServerOther) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.Name) _
                .Include(Function(x) x.Type) _
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

            listOfDatabaseReplicas = repository.Find(filterDef, projectionDef).ToList()


            Dim repositoryServers As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDefServers As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repositoryServers.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
            Dim projectionDefServers As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repositoryServers.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName)
            listOfDominoServers = repositoryServers.Find(filterDefServers, projectionDefServers).ToList()

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
            For Each entity As VSNext.Mongo.Entities.ServerOther In listOfDatabaseReplicas
                MyServerNames += entity.Name & "  "
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
            For Each entity As VSNext.Mongo.Entities.ServerOther In listOfDatabaseReplicas
                i += 1

                If entity.Name Is Nothing Then
                    MyName = "Domino Cluster #" & i.ToString
                Else
                    MyName = entity.Name
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
                    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Configuring Domino Cluster: " & entity.Name)

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
                                .Server_A_Name = listOfDominoServers.Where(Function(x) x.DeviceName.Equals(entity.DominoServerA)).ToList()(0).DeviceName
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
                                .Server_B_Name = listOfDominoServers.Where(Function(x) x.DeviceName.Equals(entity.DominoServerB)).ToList()(0).DeviceName
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
                                .Server_C_Name = listOfDominoServers.Where(Function(x) x.DeviceName.Equals(entity.DominoServerC)).ToList()(0).DeviceName
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
                        .RetryInterval = .ScanInterval
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
                            entityStatus = listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.Name)).ToList()(0)
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
                                .LastScan = Now.AddMinutes(-30)
                            Else
                                .LastScan = entityStatus.LastUpdated
                            End If
                        Catch ex As Exception
                            .LastScan = Now.AddMinutes(-30)
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
                    DirectoryServer = MyDominoServers.Item(0).Name
                    WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Querying  'Names.nsf' on " & DirectoryServer)
                    NAB = NotesSession.GetDatabase(DirectoryServer, "names.nsf", False)
                Catch ex As Exception
                    HostName = "Foo"
                    GoTo CleanUp
                End Try
            End If

            Try
                If NAB Is Nothing Then
                    DirectoryServer = MyDominoServers.Item(1).Name
                    WriteDeviceHistoryEntry("Domino", ServerName, Now.ToString & " Finally, querying  'Names.nsf' on " & DirectoryServer)
                    NAB = NotesSession.GetDatabase(DirectoryServer, "names.nsf", False)
                End If
            Catch ex As Exception
                HostName = "Foo"
                GoTo CleanUp
            End Try



            If NAB Is Nothing Then
                ' MessageBox.Show("Error connecting to 'LocalNames.nsf' ", "Error Retrieving Server Names", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
                HostName = "Foo"
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
            HostName = "Foo"
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