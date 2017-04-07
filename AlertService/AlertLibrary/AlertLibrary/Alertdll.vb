Imports System.Threading
Imports System.IO
Imports System.Data.SqlClient
Imports System.Configuration
'Imports nsoftware.IPWorks
Imports VSFramework
Imports LogUtilities.LogUtils
Imports VSNext.Mongo
Imports MongoDB.Driver
Imports MongoDB.Bson
Imports VSNext.Mongo.Entities
Imports VSNext.Mongo.Repository

Public Class Alertdll

#Region "Declarations"
    Private strAppPath As String
    Dim strAuditText As String
    ' Private strConsoleCommandLogDest As String
    Private strLogDest As String
    Dim myAdapter As New VSFramework.XMLOperation

#End Region

    Enum LogLevel
        Verbose = LogUtilities.LogUtils.LogLevel.Verbose
        Debug = LogUtilities.LogUtils.LogLevel.Debug
        Normal = LogUtilities.LogUtils.LogLevel.Normal
    End Enum

    Dim MyLogLevel As LogLevel
#Region "Connection"
    Private Function GetDBConnection() As String
        'Return "mongodb://localhost/local"
        Dim connString As String = ""
        Try
            connString = System.Configuration.ConfigurationManager.ConnectionStrings("VitalSignsMongo").ToString()
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Alerts", Now, "Error getting connection information: " & ex.Message)
        End Try
        Return connString
    End Function
#End Region
#Region "NodeName"
    Private Function GetNodeName() As String
        'Return "mongodb://localhost/local"
        Dim nodeName As String = ""
        Try
            nodeName = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Alerts", Now, "Error getting connection information: " & ex.Message)
        End Try
        Return nodeName
    End Function
#End Region
#Region "Alerts"
    Public Sub QueueAlert(ByVal DeviceType As String, ByVal DeviceName As String, ByVal AlertType As String, ByVal Details As String, ByVal Location As String,
     Optional ByVal Category As String = "")
        Dim qalert As Boolean
        Dim DeviceList As Boolean
        Dim AlertsRepeatOn As Boolean
        Dim AlertsRepeatOccurrences As Integer
        Dim CurrentRepeatOccurrences As Integer
        Dim objDateUtils As New DateUtils.DateUtils
        Dim strDateFormat As String
        Dim NowTime As String
        strDateFormat = objDateUtils.GetDateFormat()
        NowTime = objDateUtils.FixDateTime(Date.Now, strDateFormat)

        Dim connString As String = GetDBConnection()
        Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
        Dim repoEventsMaster As New Repository(Of EventsMaster)(connString)
        Dim repoOutages As New Repository(Of Outages)(connString)
        Dim repoServers As New Repository(Of Server)(connString)
        Dim repoServersOther As New Repository(Of ServerOther)(connString)
        Dim filterServers As FilterDefinition(Of Server)
        Dim filterServersOther As FilterDefinition(Of ServerOther)
        Dim filterEventsMaster As FilterDefinition(Of EventsMaster)
        Dim filterEventsDetected As FilterDefinition(Of EventsDetected)
        Dim updateEventsDetected As UpdateDefinition(Of EventsDetected)
        Dim eventsMasterEntity() As EventsMaster
        Dim eventsDetectedEntity() As EventsDetected
        Dim repeatEventsEntity() As EventsDetected
        Dim servers() As Server
        Dim serversother() As ServerOther
        Dim deviceId As String = ""
        Dim dt As DateTime?

        qalert = True
        AlertsRepeatOn = False
        DeviceList = True
        AlertsRepeatOccurrences = 0
        Try
            '12/17/2014 NS modified for VSPLUS-1267

            AlertsRepeatOn = Boolean.Parse(getSettings("AlertAboutRecurrencesOnly"))
            WriteDeviceHistoryEntry("All", "Alerts", NowTime & " AlertAboutRecurrencesOnly is:  " & AlertsRepeatOn)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error getting AlertAboutRecurrencesOnly option from the name_value collection:  " & ex.ToString)
            WriteAuditEntry(Now.ToString & " Error getting AlertAboutRecurrencesOnly option from the name_value collection:  " & ex.ToString)
        End Try
        '2. If the flag is set, check whether the current event type is set for repeat occurrence alert
        If AlertsRepeatOn Then
            Try
                '12/17/2014 NS modified for VSPLUS-1267
                AlertsRepeatOccurrences = Convert.ToInt32(getSettings("NumberOfRecurrences"))
                WriteDeviceHistoryEntry("All", "Alerts", NowTime & " AlertsRepeatOccurrences is:  " & AlertsRepeatOccurrences)
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error getting NumberOfRecurrences option from the name_value collection:  " & ex.ToString)
                WriteAuditEntry(Now.ToString & " Error getting NumberOfRecurrences option from the name_value collection:  " & ex.ToString)
            End Try
            WriteDeviceHistoryEntry("All", "Alerts", NowTime & " AlertAboutRecurrencesOnly flag is set. Checking if an alert is due to be queued for " & DeviceType & "/" & DeviceName & " " & AlertType)
            Try
                filterEventsMaster = repoEventsMaster.Filter.Eq(Of String)(Function(i) i.EventType, AlertType) And
                repoEventsMaster.Filter.Eq(Of String)(Function(i) i.DeviceType, DeviceType) And
                repoEventsMaster.Filter.Eq(Of Boolean)(Function(i) i.NotificationOnRepeat, True)
                eventsMasterEntity = repoEventsMaster.Find(filterEventsMaster).ToArray()
                If eventsMasterEntity.Count > 0 Then
                    'Find all events that match the Event Type, Device Type, and Device Name that have been detected within the last hour
                    'that have the NotificationOnRepeat flag set to true in the EventsMaster collection
                    dt = Now().AddSeconds(-3600)
                    filterEventsDetected = repoEventsDetected.Filter.Exists(Function(i) i.EventDismissed, False) And
                        repoEventsDetected.Filter.Eq(Of String)(Function(i) i.EventType, AlertType) And
                        repoEventsDetected.Filter.Eq(Of String)(Function(i) i.DeviceType, DeviceType) And
                        repoEventsDetected.Filter.Gte(Of DateTime?)(Function(i) i.EventDetected, dt) And
                        (repoEventsDetected.Filter.Exists(Function(i) i.NodeName, False) Or repoEventsDetected.Filter.Eq(Function(i) i.NodeName, GetNodeName()))



                    repeatEventsEntity = repoEventsDetected.Find(filterEventsDetected).ToArray()
                    If repeatEventsEntity.Count > 0 Then
                        CurrentRepeatOccurrences = repeatEventsEntity(0).EventRepeatCount
                        'If the number of event repeats has exceeded the maximum number of occurrences configured for this event type,
                        'reset the EventRepeatCount counter
                        If CurrentRepeatOccurrences + 1 >= AlertsRepeatOccurrences Then
                            updateEventsDetected = repoEventsDetected.Updater.Set(Function(i) i.EventRepeatCount, 0)
                            repoEventsDetected.Update(filterEventsDetected, updateEventsDetected)
                        Else
                            'If the number of event repeats has not exceeded the maximum number of occurrences for this event type,
                            'increment the counter EventRepeatCount by 1, then select events that have been last detected over an hour ago
                            'and reset the EventRepeatCount counter
                            updateEventsDetected = repoEventsDetected.Updater.Set(Function(i) i.EventRepeatCount, CurrentRepeatOccurrences + 1)
                            repoEventsDetected.Update(filterEventsDetected, updateEventsDetected)
                            filterEventsDetected = repoEventsDetected.Filter.Exists(Function(i) i.EventDismissed, False) And
                                repoEventsDetected.Filter.Eq(Of String)(Function(i) i.EventType, AlertType) And
                                repoEventsDetected.Filter.Eq(Of String)(Function(i) i.DeviceType, DeviceType) And
                                repoEventsDetected.Filter.Eq(Of String)(Function(i) i.Device, DeviceName) And
                                repoEventsDetected.Filter.Lt(Of DateTime)(Function(i) i.EventDetected, dt) And
                                (repoEventsDetected.Filter.Exists(Function(i) i.NodeName, False) Or repoEventsDetected.Filter.Eq(Function(i) i.NodeName, GetNodeName()))

                            updateEventsDetected = repoEventsDetected.Updater.Set(Function(i) i.EventRepeatCount, 0)
                            repoEventsDetected.Update(filterEventsDetected, updateEventsDetected)
                            'Do not queue an alert yet as the current number of occurrences hasn't reached the threshold yet
                            qalert = False
                        End If
                    End If
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error getting AlertOnRepeat value from the events_master collection for " & AlertType & " for " & DeviceType & ":  " & ex.ToString)
                WriteAuditEntry(Now.ToString & " Error getting AlertOnRepeat value from the events_master collection for " & AlertType & " for " & DeviceType & ":  " & ex.ToString)
            End Try
        End If
        '3. If the AlertsRepeatOn flag is off/false or there are documents in the EventsDetected collection that have the repeat
        'flag set to true and have reached the maximum threshold, queue an alert as usual
        If qalert Then
            WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Queuing Event for " & DeviceType & "/" & DeviceName & " " & AlertType)
            Try
                'RegEx is the equivalent of Like in SQL
                filterEventsDetected = repoEventsDetected.Filter.Exists(Function(i) i.EventDismissed, False) And
                    repoEventsDetected.Filter.Eq(Of String)(Function(i) i.Device, DeviceName) And
                    repoEventsDetected.Filter.Eq(Of String)(Function(i) i.DeviceType, DeviceType) And
                    repoEventsDetected.Filter.Eq(Of String)(Function(i) i.EventType, AlertType) And
                    (repoEventsDetected.Filter.Exists(Function(i) i.NodeName, False) Or repoEventsDetected.Filter.Eq(Function(i) i.NodeName, GetNodeName()))
                eventsDetectedEntity = repoEventsDetected.Find(filterEventsDetected).ToArray()
                If eventsDetectedEntity.Count = 0 Then
                    'Only one of the queries will return a device_id, depending on the device type
                    'Get device_id from the server collection to insert the value into events_detected
                    filterServers = repoServers.Filter.And(repoServers.Filter.Eq(Function(j) j.DeviceName, DeviceName),
                                                           repoServers.Filter.Eq(Function(j) j.DeviceType, DeviceType))
                    servers = repoServers.Find(filterServers).ToArray()
                    'Get device_id from the server_other collection to insert the value into events_detected
                    filterServersOther = repoServersOther.Filter.And(repoServersOther.Filter.Eq(Function(j) j.Name, DeviceName),
                                                           repoServersOther.Filter.Eq(Function(j) j.Type, DeviceType))
                    serversother = repoServersOther.Find(filterServersOther).ToArray()
                    If servers.Length > 0 Or serversother.Length > 0 Then
                        If servers.Length > 0 Then
                            deviceId = servers(0).Id.ToString()
                        ElseIf serversother.Length > 0 Then
                            deviceId = serversother(0).Id.ToString()
                        End If
                        Dim entity As New EventsDetected With {.DeviceId = deviceId, .Device = DeviceName, .DeviceType = DeviceType, .EventType = AlertType, .EventDetected = Now, .Details = Details}
                        If (Enums.Utility.getEnumFromDescription(Of Enums.ServerType)(DeviceType).getCrossNodeScanning()) Then
                            entity.NodeName = GetNodeName()
                        End If
                        repoEventsDetected.Insert(entity)
                        If AlertType = "Not Responding" Then
                            '6/15/2016 NS added
                            'OUTAGES
                            WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Outages collection update started: " & DeviceType & "/" & DeviceName & " " & AlertType)
                            Dim outages As New Outages With {.DeviceId = deviceId, .DeviceName = DeviceName, .DeviceType = DeviceType, .DateTimeDown = Now, .Description = Details}
                            repoOutages.Insert(outages)
                            WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Outages collection insert: " & DeviceType & "/" & DeviceName & " " & AlertType)
                        End If
                    End If
                Else
                    If (AlertType = "Dead Mail" Or AlertType = "Pending Mail" Or AlertType = "Held Mail") Then
                        Try
                            updateEventsDetected = repoEventsDetected.Updater.Set(Function(i) i.Details, Details)
                            repoEventsDetected.Update(filterEventsDetected, updateEventsDetected)
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Events Detected Error " & ex.Message & "," & DeviceType & "/" & DeviceName & " " & AlertType)

                            WriteAuditEntry(NowTime & " Events Detected Update Error " & ex.Message)
                        End Try
                    End If
                    If MyLogLevel = LogLevel.Verbose Then
                        WriteAuditEntry(NowTime & " This event has already been queued: " & DeviceType & "/" & DeviceName & " " & AlertType)
                        WriteDeviceHistoryEntry("All", "Alerts", NowTime & " This event has already been queued: " & DeviceType & "/" & DeviceName & " " & AlertType)

                    End If
                End If


                Dim DeviceTypelist() As String = {"Mail", "NotesMail Probe", "Notes Database", "Mobile Users"}

                For Each Type As String In DeviceTypelist
                    If Type = DeviceType Then
                        DeviceList = False
                    End If
                Next
                If DeviceList Then
                    UpdateStatusDetails(DeviceType, DeviceName, AlertType, Details, Location, Category, "Fail", NowTime)
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error searching events: " & ex.Message)
                WriteAuditEntry(NowTime & " Error searching events: " & ex.Message)
            End Try

            GC.Collect()
        End If
    End Sub
    'Mukund 14Jul14, VSPLUS-814 - StatusDetail insert/update sql
    Private Sub UpdateStatusDetails(ByVal DeviceType As String, ByVal DeviceName As String, ByVal AlertType As String, ByVal Details As String, ByVal Location As String,
      ByVal Category As String, ByVal Result As String, ByVal NowTime As String)
        Dim connString As String = GetDBConnection()
        Dim repoStatusDetails As New Repository(Of StatusDetails)(connString)
        Dim repoServer As New Repository(Of Server)(connString)
        Dim filterDefServer As MongoDB.Driver.FilterDefinition(Of Server)
        Dim filterDefStatusDetails As MongoDB.Driver.FilterDefinition(Of StatusDetails)
        Dim updateDefStatusDetails As MongoDB.Driver.UpdateDefinition(Of StatusDetails)
        Dim serversEntity() As Server
        Dim statusEntity() As StatusDetails
        Dim serverID As String
        Dim category1 As String
        Dim crossNodeScanning As Boolean = False

        WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Updating status_details for " & DeviceName & ", " & DeviceType, LogLevel.Verbose)
        Try
            filterDefServer = repoServer.Filter.Eq(Of String)(Function(i) i.DeviceName, DeviceName) And
                repoServer.Filter.Eq(Of String)(Function(i) i.DeviceType, DeviceType)

            serversEntity = repoServer.Find(filterDefServer).ToArray()
            If serversEntity.Count > 0 Then
                WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Found servers ", LogLevel.Verbose)
                If (Enums.Utility.getEnumFromDescription(Of Enums.ServerType)(DeviceType).getCrossNodeScanning()) Then
                    crossNodeScanning = True
                End If
                serverID = serversEntity(0).Id
                filterDefStatusDetails = repoStatusDetails.Filter.Eq(Of String)(Function(i) i.DeviceId, serverID) And
                    repoStatusDetails.Filter.Eq(Of String)(Function(i) i.Type, DeviceType) And
                    repoStatusDetails.Filter.Eq(Of String)(Function(i) i.TestName, AlertType)

                If crossNodeScanning Then
                    filterDefStatusDetails = filterDefStatusDetails And repoStatusDetails.Filter.Eq(Function(i) i.NodeName, GetNodeName())
                End If

                statusEntity = repoStatusDetails.Find(filterDefStatusDetails).ToArray()
                If statusEntity.Count > 0 Then
                    'update
                    category1 = IIf(Category = "", DeviceType, Category)
                    updateDefStatusDetails = repoStatusDetails.Updater.Set(Function(i) i.LastUpdate, Now) _
                        .Set(Function(i) i.category, category1) _
                        .Set(Function(i) i.Details, Details) _
                        .Set(Function(i) i.Result, Result) _
                        .Set(Function(i) i.TestName, AlertType)

                    If crossNodeScanning Then
                        updateDefStatusDetails = updateDefStatusDetails.Set(Function(i) i.NodeName, GetNodeName())
                    End If

                    repoStatusDetails.Update(filterDefStatusDetails, updateDefStatusDetails)
                Else
                    'insert
                    Dim entity As New StatusDetails With {
                        .DeviceId = serverID,
                        .Type = DeviceType,
                        .category = IIf(Category = "", DeviceType, Category),
                        .TestName = AlertType,
                        .Result = Result,
                        .LastUpdate = Now,
                        .Details = Details
                    }
                    If crossNodeScanning Then
                        entity.NodeName = GetNodeName()
                    End If
                    repoStatusDetails.Insert(entity)
                End If
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error executing update: " & ex.ToString)
            WriteAuditEntry(NowTime & " StatusDetails Insert/Update Error " & ex.Message)
        End Try
    End Sub
    Public Function getSettings(ByVal sname As String) As String
        Dim str As String = ""
        Dim connString As String = GetDBConnection()
        Dim repoNameValue As New Repository(Of NameValue)(connString)
        Dim filterNameValue As MongoDB.Driver.FilterDefinition(Of NameValue)
        Dim nameValueEntity() As NameValue

        Try
            filterNameValue = repoNameValue.Filter.Eq(Of String)(Function(j) j.Name, sname)
            nameValueEntity = repoNameValue.Find(filterNameValue).ToArray()
            If nameValueEntity.Length > 0 Then
                str = nameValueEntity(0).Value.ToString()
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Alerts", Now.ToString & " Error occurred at the time of getting value of " & sname & " from the name_value collection " & ex.Message)
        End Try
        Return str
    End Function
    Public Sub ResetAlert(ByVal DeviceType As String, ByVal DeviceName As String, ByVal AlertType As String, ByVal Location As String,
     Optional ByVal Details As String = "", Optional ByVal Category As String = "")
        Dim objDateUtils As New DateUtils.DateUtils
        Dim strDateFormat As String
        strDateFormat = objDateUtils.GetDateFormat()
        Dim NowTime As String = objDateUtils.FixDateTime(Date.Now, strDateFormat)
        Dim connString As String = GetDBConnection()
        Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
        Dim repoServers As New Repository(Of Server)(connString)
        Dim repoOutages As New Repository(Of Outages)(connString)
        Dim filterDefEvents As MongoDB.Driver.FilterDefinition(Of EventsDetected)
        Dim filterServers As MongoDB.Driver.FilterDefinition(Of Server)
        Dim filterDefOutages As MongoDB.Driver.FilterDefinition(Of Outages)
        Dim updateDefEvents As MongoDB.Driver.UpdateDefinition(Of EventsDetected)
        Dim updateDefOutages As MongoDB.Driver.UpdateDefinition(Of Outages)
        Dim eventsEntity() As EventsDetected
        Dim servers() As Server
        Dim deviceId As String
        WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Received notice to reset alert for " & DeviceType & "/" & DeviceName & ": " & AlertType)

        Try
            filterDefEvents = repoEventsDetected.Filter.Exists(Function(i) i.EventDismissed, False) And
                repoEventsDetected.Filter.Eq(Of String)(Function(i) i.Device, DeviceName) And
                repoEventsDetected.Filter.Eq(Of String)(Function(i) i.DeviceType, DeviceType) And
                repoEventsDetected.Filter.Eq(Of String)(Function(i) i.EventType, AlertType) And
                (repoEventsDetected.Filter.Exists(Function(i) i.NodeName, False) Or repoEventsDetected.Filter.Eq(Function(i) i.NodeName, GetNodeName()))
            eventsEntity = repoEventsDetected.Find(filterDefEvents).ToArray()
            If eventsEntity.Length > 0 Then
                Try
                    updateDefEvents = repoEventsDetected.Updater.Set(Function(i) i.EventDismissed, Now)
                    repoEventsDetected.Update(filterDefEvents, updateDefEvents)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "Alerts", NowTime & " events_detected update error: " & ex.Message & "," & DeviceType & "/" & DeviceName & " " & AlertType)
                    WriteAuditEntry(NowTime & " events_detected update Error " & ex.Message)
                End Try
                If AlertType = "Not Responding" Then
                    'OUTAGES
                    WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Outages collection update started: " & DeviceType & "/" & DeviceName & " " & AlertType)
                    filterServers = repoServers.Filter.And(repoServers.Filter.Eq(Function(j) j.DeviceName, DeviceName),
                                                           repoServers.Filter.Eq(Function(j) j.DeviceType, DeviceType))
                    servers = repoServers.Find(filterServers).ToArray()
                    If servers.Length > 0 Then
                        deviceId = servers(0).Id.ToString()
                        filterDefOutages = repoOutages.Filter.And(repoOutages.Filter.Eq(Function(j) j.DateTimeUp, Nothing),
                                                                  repoOutages.Filter.Eq(Function(j) j.DeviceName, DeviceName),
                                                                  repoOutages.Filter.Eq(Function(j) j.DeviceType, DeviceType))
                        updateDefOutages = repoOutages.Updater.Set(Function(i) i.DateTimeUp, Now)
                        repoOutages.Update(filterDefOutages, updateDefOutages)
                        WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Outages collection update: " & DeviceType & "/" & DeviceName & " " & AlertType)
                    End If
                End If
            End If
            'VSPLUS-930,Mukund, 15Sep14 pass Category, Details parameters
            UpdateStatusDetails(DeviceType, DeviceName, AlertType, Details, Location, Category, "Pass", NowTime)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error searching events at the time of Update: " & ex.Message)
            WriteAuditEntry(NowTime & " Error searching alerts: " & ex.Message)
        End Try

        GC.Collect()
    End Sub
    'VSPLUS-1303 22Jan15, Mukund/Swati To clear all data from  StatusDetail table based on 1 to 3 parameters
    Public Function ClearStatusDetails(ByVal DeviceName As String, Optional ByVal DeviceType As String = "", Optional ByVal AlertType As String = "") As String
        Dim connString As String = GetDBConnection()
        Dim repoStatusDetails As New Repository(Of StatusDetails)(connString)
        Dim repoServer As New Repository(Of Server)(connString)
        Dim filterDefServer As MongoDB.Driver.FilterDefinition(Of Server)
        Dim filterDefStatusDetails As MongoDB.Driver.FilterDefinition(Of StatusDetails)
        Dim serversEntity() As Server
        Dim serverID As String

        Try
            If DeviceName = "" Then
                Return "Enter Values"
            Else
                filterDefServer = repoServer.Filter.Eq(Of String)(Function(i) i.DeviceName, DeviceName) And
                    repoServer.Filter.Eq(Of String)(Function(i) i.DeviceType, DeviceType)
                serversEntity = repoServer.Find(filterDefServer).ToArray()
                If serversEntity.Count > 0 Then
                    serverID = serversEntity(0).Id
                    If DeviceType <> "" And DeviceName <> "" And AlertType <> "" Then
                        filterDefStatusDetails = repoStatusDetails.Filter.Eq(Of String)(Function(i) i.DeviceId, serverID) And
                            repoStatusDetails.Filter.Eq(Of String)(Function(i) i.Type, DeviceType) And
                            repoStatusDetails.Filter.Eq(Of String)(Function(i) i.TestName, AlertType)
                        repoStatusDetails.Delete(filterDefStatusDetails)
                    ElseIf DeviceType <> "" And DeviceName <> "" Then
                        filterDefStatusDetails = repoStatusDetails.Filter.Eq(Of String)(Function(i) i.DeviceId, serverID) And
                            repoStatusDetails.Filter.Eq(Of String)(Function(i) i.Type, DeviceType)
                        repoStatusDetails.Delete(filterDefStatusDetails)
                    ElseIf DeviceName <> "" Then
                        filterDefStatusDetails = repoStatusDetails.Filter.Eq(Of String)(Function(i) i.DeviceId, serverID)
                        repoStatusDetails.Delete(filterDefStatusDetails)
                    End If
                End If
            End If
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Exception in  ClearStatusDetails: " & ex.Message)
        End Try
        Return ""
    End Function
    Public Sub DeleteAlert(ByVal DeviceType As String, ByVal DeviceName As String, ByVal Location As String, Optional ByVal AlertType As String = "")
        Dim connString As String = GetDBConnection()
        Dim objDateUtils As New DateUtils.DateUtils
        Dim strDateFormat As String
        strDateFormat = objDateUtils.GetDateFormat()
        Dim NowTime As String = objDateUtils.FixDateTime(Date.Now, strDateFormat)
        Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
        Dim filterDefEvents As MongoDB.Driver.FilterDefinition(Of EventsDetected)

        WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Received notice to delete event for " & DeviceType & "/" & DeviceName & ": " & AlertType)
        Try
            filterDefEvents = repoEventsDetected.Filter.Exists(Function(i) i.EventDismissed, False) And
                repoEventsDetected.Filter.Eq(Of String)(Function(i) i.Device, DeviceName) And
                repoEventsDetected.Filter.Eq(Of String)(Function(i) i.DeviceType, DeviceType) And
                IIf(AlertType = "", repoEventsDetected.Filter.Exists(Function(i) i.EventType, False),
                    repoEventsDetected.Filter.Eq(Of String)(Function(i) i.EventType, AlertType))
            repoEventsDetected.Delete(filterDefEvents)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error deleting events: " & ex.Message)
            WriteAuditEntry(NowTime & " Error deleting events: " & ex.Message)
        End Try

        GC.Collect()
    End Sub
#End Region
#Region "Log Files"
    Private Sub WriteAuditEntry(ByVal strMsg As String, Optional ByVal logLevel As LogLevel = LogLevel.Normal)
        LogUtilities.LogUtils.WriteAuditEntry(strMsg, "Alertdll.txt", logLevel)
    End Sub

    Private Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String, Optional ByVal logLevel As LogLevel = LogLevel.Normal)
        LogUtilities.LogUtils.WriteDeviceHistoryEntry(DeviceType, DeviceName, strMsg, logLevel)
    End Sub
#End Region
#Region "System Messages"
    Public Sub QueueSysMessage(ByVal Details As String)
        Dim myConnectionString As New VSFramework.XMLOperation
        Dim myAdapter As New VSFramework.VSAdaptor
        Dim objDateUtils As New DateUtils.DateUtils
        Dim strDateFormat As String
        Dim NowTime As String
        Dim connString As String = GetDBConnection()
        Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
        Dim filterDefEvents As MongoDB.Driver.FilterDefinition(Of EventsDetected)
        Dim sysMessageEntity() As EventsDetected
        Try

            strDateFormat = objDateUtils.GetDateFormat()
            NowTime = objDateUtils.FixDateTime(Date.Now, strDateFormat)

            WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " Queueing System Message - " & Details)
            filterDefEvents = repoEventsDetected.Filter.Exists(Function(i) i.EventDismissed, False) And
                repoEventsDetected.Filter.Eq(Of String)(Function(i) i.Details, Details) And
                repoEventsDetected.Filter.Eq(Of Boolean)(Function(i) i.IsSystemMessage, True)
            sysMessageEntity = repoEventsDetected.Find(filterDefEvents).ToArray()
            If sysMessageEntity.Count = 0 Then
                WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " This message is new, adding to collection", LogLevel.Verbose)
                Dim entity As New EventsDetected With {.Details = Details, .EventDetected = Now, .IsSystemMessage = True}
                repoEventsDetected.Insert(entity)
                'User system messages
                'This code will need to be re-written with MongoDB once the user collection is defined
                'strSQL = "INSERT INTO SystemMessages(Details,DateCreated) VALUES('" & Details & "','" & NowTime & "')"
                'Try
                '    myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)
                '    Dim SQLstr2 As String = "IF NOT EXISTS(SELECT * FROM UserSystemMessages WHERE SysMsgID IN (SELECT MAX(ID) FROM SystemMessages)) " &
                '     "BEGIN " &
                '     "INSERT INTO UserSystemMessages (SysMsgID,UserID) " &
                '     "SELECT t1.ID SysMsgID,t2.ID UserID FROM SystemMessages t1, Users t2 " &
                '     "WHERE t1.DateCleared IS NULL AND t1.ID=(SELECT MAX(ID) FROM SystemMessages) " &
                '     "END"
                '    Try
                '        myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", SQLstr2)
                '    Catch ex As Exception
                '        WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " User System Messages Insert Error " & ex.Message)
                '        WriteAuditEntry(NowTime & " User System Messages Insert Error " & ex.Message)
                '    End Try
                'Catch ex As Exception
                '    WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " System Messages Insert Error " & ex.Message)
                '    WriteAuditEntry(NowTime & " System Messages Insert Error " & ex.Message)
                'End Try
            Else
                WriteAuditEntry(NowTime & " This message has already been queued: " & Details, LogLevel.Verbose)
                WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " This message has already been queued: " & Details, LogLevel.Verbose)
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
        Dim connString As String = GetDBConnection()
        Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
        Dim filterDefEvents As MongoDB.Driver.FilterDefinition(Of EventsDetected)
        Dim updateDefEvents As MongoDB.Driver.UpdateDefinition(Of EventsDetected)
        Dim eventsEntity() As EventsDetected

        Try

            strDateFormat = objDateUtils.GetDateFormat()
            Dim NowTime As String = objDateUtils.FixDateTime(Date.Now, strDateFormat)
            WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " Received notice to reset system message for " & Details)

            filterDefEvents = repoEventsDetected.Filter.Exists(Function(i) i.EventDismissed, False) And
                repoEventsDetected.Filter.Eq(Of String)(Function(i) i.Details, Details) And
                repoEventsDetected.Filter.Eq(Of Boolean)(Function(i) i.IsSystemMessage, True)
            eventsEntity = repoEventsDetected.Find(filterDefEvents).ToArray()
            If eventsEntity.Count > 0 Then
                updateDefEvents = repoEventsDetected.Updater.Set(Function(i) i.EventDismissed, Now)
                repoEventsDetected.Update(filterDefEvents, updateDefEvents)
                'User system messages
                'This code will need to be re-written with MongoDB once the user collection is defined
                'Try
                '    Dim SQLstr2 As String = "DELETE  t1 FROM UserSystemMessages t1 INNER JOIN SystemMessages t2 ON t2.ID=t1.SysMsgID WHERE t2.Details='" & Details & "' "
                '    myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", SQLstr2)
                'Catch ex As Exception
                '    WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " UserSystemMessages delete error: " & ex.Message & "," & Details)
                '    WriteAuditEntry(NowTime & " User System Messages Delete Error " & ex.Message)
                'End Try
                WriteDeviceHistoryEntry("All", "SysMessages", NowTime & " System Message for " & Details & " was marked as 'Cleared'")
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "SysMessages", Now.ToShortTimeString() & " Error searching system messages at the time of Update: " & ex.Message)
            WriteAuditEntry(Now.ToShortTimeString() & " Error searching system messages: " & ex.Message)
        End Try

        GC.Collect()
    End Sub

#End Region




End Class
