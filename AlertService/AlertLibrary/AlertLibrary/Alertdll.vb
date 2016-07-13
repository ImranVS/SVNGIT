﻿Imports System.Threading
Imports System.IO
Imports System.Data.SqlClient
Imports System.Configuration
'Imports nsoftware.IPWorks
Imports VSFramework
Imports LogUtilities.LogUtils
Imports VSNext.Mongo
Imports VSNext.Mongo.Entities
Imports VSNext.Mongo.Repository



Public Class Alertdll

#Region "Declarations"
    Private strAppPath As String
    Dim strAuditText As String
    ' Private strConsoleCommandLogDest As String
    Private strLogDest As String
    Dim myAdapter As New VSFramework.XMLOperation
    Dim connectionString As String = myAdapter.GetDBConnectionString("VitalSigns")
    Dim VSSconnectionString As String = myAdapter.GetDBConnectionString("VSS_Statistics")

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
        Dim filterEventsMaster As MongoDB.Driver.FilterDefinition(Of EventsMaster)
        Dim filterEventsDetected As MongoDB.Driver.FilterDefinition(Of EventsDetected)
        Dim updateEventsDetected As MongoDB.Driver.UpdateDefinition(Of EventsDetected)
        Dim eventsMasterEntity() As EventsMaster
        Dim eventsDetectedEntity() As EventsDetected
        Dim repeatEventsEntity() As EventsDetected

        qalert = True
        AlertsRepeatOn = False
        DeviceList = True
        AlertsRepeatOccurrences = 0
        Try
            '12/17/2014 NS modified for VSPLUS-1267
            AlertsRepeatOn = Boolean.Parse(getSettings("AlertsRepeatOn"))
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error getting AlertsRepeatOn option from the Settings table:  " & ex.ToString)
            WriteAuditEntry(Now.ToString & " Error getting AlertsRepeatOn option from the Settings table:  " & ex.ToString)
        End Try
        '2. If the flag is set, check whether the current event type is set for repeat occurrence alert
        If AlertsRepeatOn Then
            Try
                '12/17/2014 NS modified for VSPLUS-1267
                AlertsRepeatOccurrences = Convert.ToInt32(getSettings("AlertsRepeatOccurrences"))
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error getting AlertsRepeatOccurrences option from the Settings table:  " & ex.ToString)
                WriteAuditEntry(Now.ToString & " Error getting AlertsRepeatOccurrences option from the Settings table:  " & ex.ToString)
            End Try
            WriteDeviceHistoryEntry("All", "Alerts", NowTime & " AlertsRepeatOn flag is set. Checking if an alert is due to be queued for " & DeviceType & "/" & DeviceName & " " & AlertType)
            Try
                filterEventsMaster = repoEventsMaster.Filter.Eq(Of String)(Function(i) i.EventType, AlertType) And
                repoEventsMaster.Filter.Eq(Of String)(Function(i) i.DeviceType, DeviceType) And
                repoEventsMaster.Filter.Eq(Of Boolean)(Function(i) i.NotificationOnRepeat, True)
                eventsMasterEntity = repoEventsMaster.Find(filterEventsMaster).ToArray()
                If eventsMasterEntity.Count > 0 Then
                    'Find all events that match the Event Type, Device Type, and Device Name that have been detected within the last hour
                    'that have the NotificationOnRepeat flag set to true in the EventsMaster collection
                    filterEventsDetected = repoEventsDetected.Filter.Exists(Function(i) i.EventDismissed, False) And
                        repoEventsDetected.Filter.Eq(Of String)(Function(i) i.EventType, AlertType) And
                        repoEventsDetected.Filter.Eq(Of String)(Function(i) i.DeviceType, DeviceType) And
                        repoEventsDetected.Filter.Gte(Of DateTime)(Function(i) i.EventDetected, Now().AddSeconds(-3600))
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
                                repoEventsDetected.Filter.Lt(Of DateTime)(Function(i) i.EventDetected, Now().AddSeconds(-3600))
                            updateEventsDetected = repoEventsDetected.Updater.Set(Function(i) i.EventRepeatCount, 0)
                            repoEventsDetected.Update(filterEventsDetected, updateEventsDetected)
                            'Do not queue an alert yet as the current number of occurrences hasn't reached the threshold yet
                            qalert = False
                        End If
                    End If
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error getting AlertOnRepeat value from the EventsMaster table for " & AlertType & " for " & DeviceType & ":  " & ex.ToString)
                WriteAuditEntry(Now.ToString & " Error getting AlertOnRepeat value from the EventsMaster table for " & AlertType & " for " & DeviceType & ":  " & ex.ToString)
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
                    repoEventsDetected.Filter.Eq(Of String)(Function(i) i.EventType, AlertType)
                eventsDetectedEntity = repoEventsDetected.Find(filterEventsDetected).ToArray()
                If eventsDetectedEntity.Count = 0 Then
                    Dim entity As New EventsDetected With {.Device = DeviceName, .DeviceType = DeviceType, .EventType = AlertType, .EventDetected = Now, .Details = Details}
                    repoEventsDetected.Insert(entity)
                    If AlertType = "Not Responding" Then
                        '6/15/2016 NS added
                        'OUTAGES
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
                    If DeviceType = "Office365" Then
                        UpdateStatusDetails(Location, DeviceName, AlertType, Details, Location, Category, "Fail", NowTime)
                    Else
                        UpdateStatusDetails(DeviceType, DeviceName, AlertType, Details, Location, Category, "Fail", NowTime)
                    End If
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

        WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Updating status_details for " & DeviceName & ", " & DeviceType, LogLevel.Verbose)
        Try
            filterDefServer = repoServer.Filter.Eq(Of String)(Function(i) i.ServerName, DeviceName) And
                repoServer.Filter.Eq(Of String)(Function(i) i.ServerType, DeviceType)
            serversEntity = repoServer.Find(filterDefServer).ToArray()
            If serversEntity.Count > 0 Then
                WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Found servers ", LogLevel.Verbose)
                serverID = serversEntity(0).Id
                filterDefStatusDetails = repoStatusDetails.Filter.Eq(Of String)(Function(i) i.DeviceId, serverID) And
                repoStatusDetails.Filter.Eq(Of String)(Function(i) i.Type, DeviceType) And
                repoStatusDetails.Filter.Eq(Of String)(Function(i) i.TestName, AlertType)
                statusEntity = repoStatusDetails.Find(filterDefStatusDetails).ToArray()
                If statusEntity.Count > 0 Then
                    'update
                    updateDefStatusDetails = repoStatusDetails.Updater.Set(Function(i) i.LastUpdate, Now)
                    repoStatusDetails.Update(filterDefStatusDetails, updateDefStatusDetails)
                    category1 = IIf(Category = "", DeviceType, Category)
                    updateDefStatusDetails = repoStatusDetails.Updater.Set(Function(i) i.category, category1)
                    repoStatusDetails.Update(filterDefStatusDetails, updateDefStatusDetails)
                    updateDefStatusDetails = repoStatusDetails.Updater.Set(Function(i) i.Details, Details)
                    repoStatusDetails.Update(filterDefStatusDetails, updateDefStatusDetails)
                    updateDefStatusDetails = repoStatusDetails.Updater.Set(Function(i) i.Result, Result)
                    repoStatusDetails.Update(filterDefStatusDetails, updateDefStatusDetails)
                    updateDefStatusDetails = repoStatusDetails.Updater.Set(Function(i) i.TestName, AlertType)
                    repoStatusDetails.Update(filterDefStatusDetails, updateDefStatusDetails)
                Else
                    'insert
                    Dim entity As New StatusDetails With {.DeviceId = serverID, .Type = DeviceType, .category = IIf(Category = "", DeviceType, Category), .TestName = AlertType, .Result = Result, .LastUpdate = Now, .Details = Details}
                    repoStatusDetails.Insert(entity)
                End If
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Error executing update: " & ex.ToString)
            WriteAuditEntry(NowTime & " StatusDetails Insert/Update Error " & ex.Message)
        End Try
    End Sub
    Public Function getSettings(ByVal sname As String) As String
        '12/17/2014 NS modified for VSPLUS-1267
        Dim myConnectionString As New VSFramework.XMLOperation
        Dim myAdapter As New VSFramework.VSAdaptor
        Dim str As String = ""

        Try
            Dim sqlQuery As String = "Select svalue from Settings where sname='" & sname & "'"
            Dim dt As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sqlQuery)
            If dt.Rows.Count > 0 Then
                str = dt.Rows(0)("svalue").ToString()
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Alerts", Now.ToString & " Error occurred at the time of getting value of " & sname & " from Settings Table " & ex.Message)
        End Try

        Return str
    End Function
    Private Function GetMaxAlertsQueuedToday(ByVal con As SqlConnection) As Integer
        '12/17/2014 NS added for VSPLUS-1267
        Dim myConnectionString As New VSFramework.XMLOperation
        Dim myAdapter As New VSFramework.VSAdaptor
        Dim iRetVal As Integer = 0
        Dim dailyCountSetting As String = ""
        '12/17/2014 NS modified for VSPLUS-1267
        Try
            dailyCountSetting = getSettings("DailyAlertCountSetting")
            If dailyCountSetting <> "" Then
                iRetVal = Convert.ToInt32(dailyCountSetting)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Alerts", Now.ToString & " Error getting DailyAlertCountSetting option from the Settings table:  " & ex.ToString)
        End Try
        Return iRetVal
    End Function
    Public Sub ResetAlert(ByVal DeviceType As String, ByVal DeviceName As String, ByVal AlertType As String, ByVal Location As String,
     Optional ByVal Details As String = "", Optional ByVal Category As String = "")
        Dim objDateUtils As New DateUtils.DateUtils
        Dim strDateFormat As String
        strDateFormat = objDateUtils.GetDateFormat()
        Dim NowTime As String = objDateUtils.FixDateTime(Date.Now, strDateFormat)
        Dim connString As String = GetDBConnection()
        Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
        Dim filterDefEvents As MongoDB.Driver.FilterDefinition(Of EventsDetected)
        Dim updateDefEvents As MongoDB.Driver.UpdateDefinition(Of EventsDetected)
        Dim eventsEntity() As EventsDetected
        WriteDeviceHistoryEntry("All", "Alerts", NowTime & " Received notice to reset alert for " & DeviceType & "/" & DeviceName & ": " & AlertType)

        Try
            filterDefEvents = repoEventsDetected.Filter.Exists(Function(i) i.EventDismissed, False) And
                repoEventsDetected.Filter.Eq(Of String)(Function(i) i.Device, DeviceName) And
                repoEventsDetected.Filter.Eq(Of String)(Function(i) i.DeviceType, DeviceType) And
                repoEventsDetected.Filter.Eq(Of String)(Function(i) i.EventType, AlertType)
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
                End If
            End If
            'VSPLUS-930,Mukund, 15Sep14 pass Category, Details parameters
            If DeviceType = "Office365" Then
                UpdateStatusDetails(Location, DeviceName, AlertType, Details, Location, Category, "Pass", NowTime)
            Else
                UpdateStatusDetails(DeviceType, DeviceName, AlertType, Details, Location, Category, "Pass", NowTime)
            End If
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
                filterDefServer = repoServer.Filter.Eq(Of String)(Function(i) i.ServerName, DeviceName) And
                    repoServer.Filter.Eq(Of String)(Function(i) i.ServerType, DeviceType)
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
    Public Sub SysMessageForLicenses()
        'HA related design hasn't been discussed yet; leaving the function as is for now
        Dim myConnectionString As New VSFramework.XMLOperation
        Dim myAdapter As New VSFramework.VSAdaptor
        '7/8/2015 NS modified verbiage for VSPLUS-1959
        Dim message As String = "There is an insufficient number of licenses for your servers."
        Try

            'Dim sql As String = "SELECT Count(*) FROM DeviceInventory WHERE CurrentNodeID=-1"
            Dim sql As String = "select COUNT(*) from DeviceInventory,Nodes  where CurrentNodeId =-1 and nodes.Alive =1"
            Dim dt As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sql)

            If (dt.Rows.Count > 0 And dt.Rows(0)(0).ToString() = "0") Then
                ResetSysMessage(message)
            Else
                QueueSysMessage(message)

            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "SysMessages", Now.ToShortTimeString & " System Message for License: " & ex.Message)
            WriteAuditEntry(Now.ToShortTimeString & " System Message for License: " & ex.Message)
        End Try
    End Sub
#End Region




End Class
