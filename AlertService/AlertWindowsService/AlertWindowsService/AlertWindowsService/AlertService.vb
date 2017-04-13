Imports System.Threading
Imports System.ServiceProcess
Imports System.Data.SqlClient
Imports System.Data
Imports System.Net.Mail
Imports System.IO
Imports nsoftware.IPWorks
Imports Chilkat
Imports System.Configuration
'4/4/2014 NS added for VSPLUS-403
Imports System.Diagnostics
'12/1/2014 NS added for VSPLUS-946
Imports Twilio
Imports System.Globalization
Imports RPRWyatt.VitalSigns.Services
'5/20/2015 NS added for VSPLUS-1764
Imports SnmpSharpNet
'11/17/2015 NS added for VSPLUS-1562
Imports Microsoft.Win32
Imports VSNext.Mongo
Imports VSNext.Mongo.Entities
Imports VSNext.Mongo.Repository
Imports MongoDB.Driver
Imports MongoDB.Bson

Public Class AlertDefinition
    Implements IComparer
    Public Sub New(ByVal akey As String)
        _AlertKey = akey
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As _
        Object) As Integer Implements _
        System.Collections.IComparer.Compare
        Dim ax As AlertDefinition = DirectCast(x, AlertDefinition)
        Dim ay As AlertDefinition = DirectCast(y, AlertDefinition)
        Dim intx As String
        Dim inty As String

        intx = ax.AlertKey
        inty = ay.AlertKey

        Return intx.CompareTo(inty) 'The old version of the code when working with int alert keys used intx > inty comparison. When working with strings, need to use CompareTo
    End Function

    Private _AlertKey As String
    Private _AlertHistoryId As String
    Private _EventName As String
    Private _ServerType As String
    Private _ServerName As String
    Private _Details As String
    Private _SendSNMPTrap As Boolean
    Private _SendTo As String
    Private _CopyTo As String
    Private _BlindCopyTo As String
    Private _StartTime As String
    Private _Duration As Integer
    Private _Days As String
    Private _IntType As Integer
    Private _RunNow As Integer
    Private _Repeat As Boolean
    '2/17/2014 NS added for cleared alerts
    Private _SentID As String
    '4/7/2014 NS added for VSPLUS-519
    Private _EnablePersistentAlert As Boolean
    Private _NotifyOnRepeat As Boolean
    Private _EventRepeatCount As Integer
    Private _DateCreated As String
    '12/1/2014 NS added for VSPLUS-946
    Private _SMSTo As String
    '12/9/2014 NS added for VSPLUS-1229
    Private _ScriptName As String
    Private _ScriptCommand As String
    Private _ScriptLocation As String

    Public Sub New()   'constructor
        'Console.WriteLine("Object is being created")
        _Repeat = False
    End Sub
    Public Property AlertKey() As String
        Get
            Return _AlertKey
        End Get
        Set(ByVal value As String)
            _AlertKey = value
        End Set
    End Property
    Public Property AlertHistoryId() As String
        Get
            Return _AlertHistoryId
        End Get
        Set(ByVal value As String)
            _AlertHistoryId = value
        End Set
    End Property
    Public Property EventName() As String
        Get
            Return _EventName
        End Get
        Set(ByVal value As String)
            _EventName = value
        End Set
    End Property
    Public Property ServerType() As String
        Get
            Return _ServerType
        End Get
        Set(ByVal value As String)
            _ServerType = value
        End Set
    End Property
    Public Property ServerName() As String
        Get
            Return _ServerName
        End Get
        Set(ByVal value As String)
            _ServerName = value
        End Set
    End Property
    Public Property Details() As String
        Get
            Return _Details
        End Get
        Set(ByVal value As String)
            _Details = value
        End Set
    End Property
    Public Property SendSNMPTrap() As String
        Get
            Return _SendSNMPTrap
        End Get
        Set(ByVal value As String)
            _SendSNMPTrap = value
        End Set
    End Property
    Public Property SendTo() As String
        Get
            Return _SendTo
        End Get
        Set(ByVal value As String)
            _SendTo = value
        End Set
    End Property
    Public Property CopyTo() As String
        Get
            Return _CopyTo
        End Get
        Set(ByVal value As String)
            _CopyTo = value
        End Set
    End Property
    Public Property BlindCopyTo() As String
        Get
            Return _BlindCopyTo
        End Get
        Set(ByVal value As String)
            _BlindCopyTo = value
        End Set
    End Property
    Public Property StartTime() As String
        Get
            Return _StartTime
        End Get
        Set(ByVal value As String)
            _StartTime = value
        End Set
    End Property
    Public Property Duration() As Integer
        Get
            Return _Duration
        End Get
        Set(ByVal value As Integer)
            _Duration = value
        End Set
    End Property
    Public Property Days() As String
        Get
            Return _Days
        End Get
        Set(ByVal value As String)
            _Days = value
        End Set
    End Property
    Public Property IntType() As Integer
        Get
            Return _IntType
        End Get
        Set(ByVal value As Integer)
            _IntType = value
        End Set
    End Property
    Public Property RunNow() As Integer
        Get
            Return _RunNow
        End Get
        Set(ByVal value As Integer)
            _RunNow = value
        End Set
    End Property
    Public Property Repeat() As Boolean
        Get
            Return _Repeat
        End Get
        Set(ByVal value As Boolean)
            _Repeat = value
        End Set
    End Property
    Public Property SentID() As String
        Get
            Return _SentID
        End Get
        Set(ByVal value As String)
            _SentID = value
        End Set
    End Property
    Public Property EnablePersistentAlert() As Boolean
        Get
            Return _EnablePersistentAlert
        End Get
        Set(ByVal value As Boolean)
            _EnablePersistentAlert = value
        End Set
    End Property
    Public Property NotifyOnRepeat() As Boolean
        Get
            Return _NotifyOnRepeat
        End Get
        Set(ByVal value As Boolean)
            _NotifyOnRepeat = value
        End Set
    End Property
    Public Property EventRepeatCount() As Integer
        Get
            Return _EventRepeatCount
        End Get
        Set(ByVal value As Integer)
            _EventRepeatCount = value
        End Set
    End Property
    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property
    '12/1/2014 NS added for VSPLUS-946
    Public Property SMSTo() As String
        Get
            Return _SMSTo
        End Get
        Set(ByVal value As String)
            _SMSTo = value
        End Set
    End Property
    '12/9/2014 NS added for VSPLUS-1229
    Public Property ScriptName() As String
        Get
            Return _ScriptName
        End Get
        Set(ByVal value As String)
            _ScriptName = value
        End Set
    End Property
    Public Property ScriptCommand() As String
        Get
            Return _ScriptCommand
        End Get
        Set(ByVal value As String)
            _ScriptCommand = value
        End Set
    End Property
    Public Property ScriptLocation() As String
        Get
            Return _ScriptLocation
        End Get
        Set(ByVal value As String)
            _ScriptLocation = value
        End Set
    End Property
End Class

Public Class VitalSignsAlertService
    Inherits VSServices
    Dim Stopping As Boolean = False
    Dim WithEvents SNMPAgent As New nsoftware.IPWorks.Snmpagent("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000")
    Dim BuildNumber As Integer = 16
    Dim AlertDict As New Dictionary(Of String, AlertDefinition())
    Dim pair As KeyValuePair(Of String, AlertDefinition())
    Enum LogLevel
        Verbose = LogUtilities.LogUtils.LogLevel.Verbose
        Debug = LogUtilities.LogUtils.LogLevel.Debug
        Normal = LogUtilities.LogUtils.LogLevel.Normal
    End Enum
    '11/17/2015 NS modified for VSPLUS-1562
    Dim myRegistry As New RegistryHandler()
    
    'MyLogLevel is used throughout to control the volume of the log file output
    Dim MyLogLevel As LogLevel
    '7/20/2015 NS added for VSPLUS-1562
    Dim emergencyAlertSent As Boolean = False
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
    Protected Overrides Sub ServiceOnStart(args() As String)
        '12/16/2014 NS added for VSPLUS-1267
        Dim sCultureString As String = "en-US"
        Dim connectionStringName As String = "CultureString"

        Try
            sCultureString = ConfigurationManager.AppSettings(connectionStringName).ToString()
        Catch ex As Exception
            sCultureString = "en-US"
        End Try
        '12/17/2014 NS added for VSPLUS-1267
        Thread.CurrentThread.CurrentCulture = New CultureInfo(sCultureString)

        Dim strAppPath As String

        Try
            MyLogLevel = myRegistry.ReadFromRegistry("Log Level")
        Catch ex As Exception
            '9/30/2014 NS modified - log level should be normal
            'MyLogLevel = LogLevel.Verbose
            MyLogLevel = LogLevel.Normal
        End Try

        Try
            strAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
            'Commented out file deletion so that the log keeps getting written into
            'File.Delete(strAppPath & "\Log_Files\All_Alert_Service.txt")
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error deleting log file " & ex.ToString, LogLevel.Normal)
        End Try

        '12/16/2014 NS commented out for VSPLUS-1267 - con is localized
        'con.ConnectionString = myAdapter.GetDBConnectionString("VitalSigns")

        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        ' Log a service start message to the Application log. 
        Dim mySettings As New VSFramework.RegistryHandler
        Me.EventLog1.WriteEntry("VitalSigns Alert Service is starting up.")
        Try
            mySettings.WriteToRegistry("Alert Service Start", Now.ToString)
            mySettings.WriteToRegistry("Alert Service Build", BuildNumber)
        Catch ex As Exception

        End Try

        WriteServiceHistoryEntry(Now.ToString & " The VitalSigns Alert Service is starting up. ", LogLevel.Normal)
        WriteServiceHistoryEntry(Now.ToString & " The VitalSigns Alert Service is Copyright " & Now.Year.ToString & ", JNIT, Inc. dba RPR Wyatt and MZL Software Development, Inc. ", LogLevel.Normal)
        Dim emergencyInfoThread As New Thread(Sub()
                                                  Dim i As Integer = 0
                                                  While True
                                                      Try
                                                          GetEmergencyAlertInfo()
                                                          Thread.Sleep(New TimeSpan(1, 0, 0))
                                                      Catch ex As Exception
                                                          WriteServiceHistoryEntry(Now.ToString & " The following exception has occurred in the emergencyInfoThread: " & ex.Message, LogLevel.Normal)
                                                      End Try
                                                  End While
                                              End Sub)
        emergencyInfoThread.Start()
        ' Queue the main service function for execution in a worker thread. 
        '11/27/2013 NS modified for testing 
        '12/17/2014 NS modified for VSPLUS-1267
        'ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf ServiceWorkerThreadNew))
        Dim mainThread As New Thread(AddressOf ServiceWorkerThreadNew)
        mainThread.CurrentCulture = New CultureInfo(sCultureString)
        mainThread.Start()
    End Sub
    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        ' Log a service stop message to the Application log. 
        Me.EventLog1.WriteEntry("The VitalSigns Alert Service is shutting down.")
        WriteServiceHistoryEntry(Now.ToString & " The VitalSigns Alert Service is shutting down. ", LogLevel.Normal)
        ' Indicate that the service is stopping and wait for the finish of  
        ' the main service function (ServiceWorkerThread). 
        Dim mySettings As New VSFramework.RegistryHandler
        mySettings.WriteToRegistry("Alert Service End", Now.ToString)
        Me.Stopping = True
        MyBase.OnStop()
    End Sub
    Private Overloads Sub WriteServiceHistoryEntry(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Verbose)
        MyBase.WriteHistoryEntry(strMsg, "All_Alert_Service.txt", LogLevelInput)
    End Sub
    Private Sub ServiceWorkerThreadNew()
        '12/16/2014 NS added for VSPLUS-1267
        Dim sCultureString As String = "en-US"
        Dim connectionStringName As String = "CultureString"
        Dim myConnectionString As New VSFramework.XMLOperation
        Dim myAdapter As New VSFramework.VSAdaptor
        '7/20/2015 NS added for VSPLUS-1562
        Dim emergencyContacts As String = ""
        Dim PHostName As String = ""
        Dim Pport As String = ""
        Dim PEmail As String = ""
        Dim Ppwd As String = ""
        Dim PFrom As String = ""
        Dim PAuth As Boolean = False
        Dim PSSL As Boolean = False
        Dim tempObj As Object
        Dim connString As String = GetDBConnection()
        Dim repoSettings As New Repository(Of NameValue)(connString)
        Dim filterSettings As FilterDefinition(Of NameValue)
        Dim settings() As NameValue

        WriteServiceHistoryEntry(Now.ToString & " The Service Worker Thread is starting up. ", LogLevel.Normal)
        ' Periodically check if the service is stopping. 

        Do While Not Me.Stopping
            ' Perform main service function here... 
            '1/6/2014 NS added a check to see if the alerting service should be sending alerts
            Dim alertson As Boolean = True

            Try
                filterSettings = repoSettings.Filter.Eq(Function(j) j.Name, "AlertsOn")
                settings = repoSettings.Find(filterSettings).ToArray()
                If Not IsNothing(settings) Then
                    alertson = Convert.ToBoolean(settings(0).Value.ToString())
                End If
                WriteServiceHistoryEntry(Now.ToString & " The AlertsOn value is " & alertson & ".", LogLevel.Verbose)
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " Error in ServiceWorkerThreadNew when getting the value of AlertsOn from the name_value collection: " & ex.Message, LogLevel.Normal)
            End Try

            '1/6/2014 NS added - we only want to send alerts if the flag is enabled, otherwise, continue with clearing and keep checking
            If (alertson) Then
                Try
                    WriteServiceHistoryEntry(Now.ToString & " ServiceWorkerThreadNew - trying to process alerts", LogLevel.Verbose)
                    '4/4/2017 NS commented out the emergency alert piece for VSPLUS-3539
                    'If Not myRegistry Is Nothing Then
                    '    WriteServiceHistoryEntry(Now.ToString & " ServiceWorkerThreadNew - got myRegistry object", LogLevel.Verbose)
                    '    tempObj = myRegistry.ReadFromRegistry("Alert Emergency Contacts")
                    '    If Not tempObj Is Nothing Then
                    '        emergencyContacts = tempObj
                    '        If emergencyContacts.Length > 0 Then
                    '            emergencyContacts = emergencyContacts.Substring(0, emergencyContacts.Length - 1)
                    '        End If
                    '        WriteServiceHistoryEntry(Now.ToString & " Alert Emergency Contacts: " & emergencyContacts, LogLevel.Verbose)
                    '    End If
                    '    tempObj = myRegistry.ReadFromRegistry("Alert Emergency PrimaryHostName")
                    '    If Not tempObj Is Nothing Then
                    '        PHostName = tempObj
                    '        WriteServiceHistoryEntry(Now.ToString & " Alert Emergency PrimaryHostName: " & PHostName, LogLevel.Verbose)
                    '    End If
                    '    tempObj = myRegistry.ReadFromRegistry("Alert Emergency primaryport")
                    '    If Not tempObj Is Nothing Then
                    '        Pport = tempObj
                    '        WriteServiceHistoryEntry(Now.ToString & " Alert Emergency primaryport: " & Pport, LogLevel.Verbose)
                    '    End If
                    '    tempObj = myRegistry.ReadFromRegistry("Alert Emergency primaryUserID")
                    '    If Not tempObj Is Nothing Then
                    '        PEmail = tempObj
                    '        WriteServiceHistoryEntry(Now.ToString & " Alert Emergency primaryUserID: " & PEmail, LogLevel.Verbose)
                    '    End If
                    '    tempObj = myRegistry.ReadFromRegistry("Alert Emergency primarypwd")
                    '    If Not tempObj Is Nothing Then
                    '        Ppwd = tempObj
                    '        WriteServiceHistoryEntry(Now.ToString & " Alert Emergency primarypwd: " & Ppwd, LogLevel.Verbose)
                    '    End If
                    '    tempObj = myRegistry.ReadFromRegistry("Alert Emergency primaryFrom")
                    '    If Not tempObj Is Nothing Then
                    '        PFrom = tempObj
                    '        WriteServiceHistoryEntry(Now.ToString & " Alert Emergency primaryFrom: " & PFrom, LogLevel.Verbose)
                    '    End If
                    '    tempObj = myRegistry.ReadFromRegistry("Alert Emergency primaryAuth")
                    '    If Not tempObj Is Nothing Then
                    '        If tempObj.ToString() <> "" Then
                    '            '12/3/2015 NS modified for VSPLUS-1562
                    '            PAuth = Convert.ToBoolean(tempObj.ToString())
                    '            WriteServiceHistoryEntry(Now.ToString & " Alert Emergency primaryAuth: " & PAuth, LogLevel.Verbose)
                    '        End If
                    '    End If
                    '    tempObj = myRegistry.ReadFromRegistry("Alert Emergency primarySSL")
                    '    If Not tempObj Is Nothing Then
                    '        If tempObj.ToString() <> "" Then
                    '            '12/8/2015 NS modified for VSPLUS-1562
                    '            PSSL = Convert.ToBoolean(tempObj.ToString())
                    '            WriteServiceHistoryEntry(Now.ToString & " Alert Emergency primarySSL: " & PSSL, LogLevel.Verbose)
                    '        End If
                    '    End If
                    'End If
                    WriteServiceHistoryEntry(Now.ToString & " Before calling ProcessAlertsSendNotification()", LogLevel.Verbose)
                    ProcessAlertsSendNotification()
                Catch ex As Exception
                    WriteServiceHistoryEntry(Now.ToString & " Error in ServiceWorkerThreadNew: " & ex.Message, LogLevel.Normal)
                    '7/20/2015 NS added for VSPLUS-1562
                    '4/4/2017 NS commented out the emergency alert piece
                    'Try
                    '    If Not emergencyAlertSent Then
                    '        If emergencyContacts <> "" Then
                    '            SendMailNet(PHostName, Pport, PAuth, False, Ppwd, PEmail, PSSL, "The SQL server seems to be down or is not accessbile. VitalSigns will not be able to function without proper SQL access. Please contact your SQL server administrator immediately in order to ensure proper VitalSigns operation.", False, "", "", "", emergencyContacts, "", "", PFrom, True)
                    '            emergencyAlertSent = True
                    '        End If
                    '    End If
                    'Catch ex1 As Exception
                    '    WriteServiceHistoryEntry(Now.ToString & " Error ServiceWorkerThreadNew when sending emergency email: " & ex1.Message, LogLevel.Normal)
                    'End Try

                    Thread.Sleep(5000)  ' Wait a while and then do it again
                End Try
            End If

            Try
                WriteServiceHistoryEntry(Now.ToString & " ServiceWorkerThreadNew - trying process clear alerts", LogLevel.Verbose)
                ProcessAlertsClearSendNotification()
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " Error ServiceWorkerThreadNew when calling ProcessAlertsClearSendNotification(): " & ex.Message, LogLevel.Normal)
            End Try

            Thread.Sleep(10000)  ' Wait a while and then do it again
        Loop


        WriteServiceHistoryEntry(Now.ToString & " The Service Worker Thread is shutting down... ", LogLevel.Normal)
        ' Signal the stopped event. 
        'Me.stoppedEvent.Set()
    End Sub
    Private Function stoppedEvent() As Object
        Throw New NotImplementedException
    End Function
    Private Sub ProcessAlertsSendNotification()
        Dim ADef As New AlertDefinition
        Dim AHist As New AlertDefinition
        Dim ADefOut As New AlertDefinition
        Dim keyList As New List(Of String)
        Dim keyArr As String()
        Dim ADefArr As AlertDefinition()
        Dim ADefArrOut As AlertDefinition()
        Dim AHistArr As AlertDefinition()
        Dim c As Integer

        Dim connString As String = GetDBConnection()
        Dim repoEventsMaster As New Repository(Of EventsMaster)(connString)
        Dim repoNotifications As New Repository(Of Notifications)(connString)
        Dim repoNotificationDest As New Repository(Of NotificationDestinations)(connString)
        Dim repoServers As New Repository(Of Server)(connString)
        Dim repoBusHrs As New Repository(Of BusinessHours)(connString)
        Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)

        Dim filterNotifications As FilterDefinition(Of Notifications)
        Dim filterNotificationDest As FilterDefinition(Of NotificationDestinations)
        Dim filterEvents As FilterDefinition(Of EventsMaster)
        Dim filterServers As FilterDefinition(Of Server)
        Dim filterBusHrs As FilterDefinition(Of BusinessHours)
        Dim filterEventsDetected As FilterDefinition(Of EventsDetected)
        Dim updateEventsDetected As UpdateDefinition(Of EventsDetected)

        Dim eventsEntity() As EventsMaster
        Dim serversEntity() As Server
        Dim bushrsEntity() As BusinessHours
        Dim sendlist As NotificationDestinations
        Dim notificationsEntity() As Notifications
        Dim notificationDestEntity() As NotificationDestinations
        Dim eventsCreated() As EventsDetected
        Dim eventsEscalated() As EventsDetected
        Dim notificationsSent() As NotificationsSent

        Dim alertAboutRecurrences As Boolean
        Dim numberOfRecurrences As Integer
        Dim tempVal As String = ""

        Dim dt As New DataTable
        Dim dr As DataRow
        Dim oid As String
        Dim eid As String

        alertAboutRecurrences = False
        Try
            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to get AlertAboutRecurrencesOnly from the name_value collection", LogLevel.Verbose)
            tempVal = getSettings("AlertAboutRecurrencesOnly")
            If tempVal <> "" Then
                alertAboutRecurrences = Convert.ToBoolean(tempVal)
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error getting AlertAboutRecurrencesOnly from the name_value collection:  " & ex.ToString, LogLevel.Normal)
        End Try
        numberOfRecurrences = 0
        Try
            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to get NumberOfRecurrences from the name_value collection", LogLevel.Verbose)
            tempVal = getSettings("NumberOfRecurrences")
            If tempVal <> "" Then
                numberOfRecurrences = Convert.ToInt32(tempVal)
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error getting NumberOfRecurrences from the name_value collection:  " & ex.ToString, LogLevel.Normal)
        End Try

        dt.Columns.Add("AlertKey")
        dt.Columns.Add("EventName")
        dt.Columns.Add("ServerType")
        dt.Columns.Add("ServerName")
        dt.Columns.Add("SendTo")
        dt.Columns.Add("CopyTo")
        dt.Columns.Add("BlindCopyTo")
        dt.Columns.Add("StartTime")
        dt.Columns.Add("Duration")
        dt.Columns.Add("Day")
        dt.Columns.Add("HoursIndicator")
        dt.Columns.Add("SendSNMPTrap")
        dt.Columns.Add("EnablePersistentAlert")
        dt.Columns.Add("NotifyOnRepeat")
        dt.Columns.Add("SMSTo")
        dt.Columns.Add("ScriptName")
        dt.Columns.Add("ScriptCommand")
        dt.Columns.Add("ScriptLocation")

        Try
            filterNotifications = repoNotifications.Filter.Exists(Function(j) j.NotificationName, True)
            notificationsEntity = repoNotifications.Find(filterNotifications).ToArray()
            If notificationsEntity.Length > 0 Then
                For i As Integer = 0 To notificationsEntity.Length - 1
                    oid = notificationsEntity(i).ObjectId.ToString()
                    filterEvents = repoEventsMaster.Filter.And(repoEventsMaster.Filter.Exists(Function(j) j.NotificationList, True),
                            repoEventsMaster.Filter.AnyEq(Of String)(Function(j) j.NotificationList, oid))
                    eventsEntity = repoEventsMaster.Find(filterEvents).ToArray()

                    filterServers = repoServers.Filter.And(repoServers.Filter.Exists(Function(j) j.NotificationList, True),
                            repoServers.Filter.AnyEq(Of String)(Function(j) j.NotificationList, oid))
                    serversEntity = repoServers.Find(filterServers).ToArray()

                    filterNotificationDest = repoNotificationDest.Filter.In(Of String)(Function(j) j.Id, notificationsEntity(i).SendList)
                    notificationDestEntity = repoNotificationDest.Find(filterNotificationDest).ToArray()
                    For k As Integer = 0 To notificationDestEntity.Length - 1
                        If eventsEntity.Length > 0 And serversEntity.Length > 0 Then
                            sendlist = notificationDestEntity(k)
                            'Do not include escalation documents in processing
                            If IsNothing(sendlist.Interval) Then
                                For x As Integer = 0 To eventsEntity.Length - 1
                                    For y As Integer = 0 To serversEntity.Length - 1
                                        dr = dt.NewRow()
                                        dr("AlertKey") = notificationsEntity(i).Id
                                        dr("EventName") = eventsEntity(x).EventType
                                        dr("ServerType") = serversEntity(y).DeviceType
                                        dr("ServerName") = serversEntity(y).DeviceName
                                        dr("CopyTo") = ""
                                        dr("BlindCopyTo") = ""
                                        If Not IsNothing(sendlist.CopyTo) Then
                                            dr("CopyTo") = sendlist.CopyTo
                                        End If
                                        If Not IsNothing(sendlist.BlindCopyTo) Then
                                            dr("BlindCopyTo") = sendlist.BlindCopyTo
                                        End If
                                        dr("SendTo") = ""
                                        dr("SMSTo") = ""
                                        dr("ScriptName") = ""
                                        dr("ScriptCommand") = ""
                                        dr("ScriptLocation") = ""
                                        dr("SendSNMPTrap") = False
                                        dr("EnablePersistentAlert") = False
                                        If Not IsNothing(sendlist.PersistentNotification) Then
                                            dr("EnablePersistentAlert") = sendlist.PersistentNotification
                                        End If
                                        dr("NotifyOnRepeat") = False
                                        If Not IsNothing(eventsEntity(x).NotificationOnRepeat) Then
                                            dr("NotifyOnRepeat") = eventsEntity(x).NotificationOnRepeat
                                        End If
                                        If sendlist.SendVia = "E-mail" Then
                                            dr("SendTo") = sendlist.SendTo
                                        ElseIf sendlist.SendVia = "SMS" Then
                                            dr("SMSTo") = sendlist.SendTo
                                        ElseIf sendlist.SendVia = "Script" Then
                                            dr("ScriptName") = sendlist.SendTo
                                            dr("ScriptCommand") = sendlist.ScriptCommand
                                            dr("ScriptLocation") = sendlist.ScriptLocation
                                        ElseIf sendlist.SendVia = "SNMP Trap" Then
                                            dr("SendSNMPTrap") = True
                                        End If
                                        dr("HoursIndicator") = 0 'sendlist.BId
                                        dr("StartTime") = ""
                                        dr("Duration") = 0
                                        dr("Day") = ""
                                        filterBusHrs = repoBusHrs.Filter.Eq(Of ObjectId)("_id", New ObjectId(sendlist.BusinessHoursId))
                                        bushrsEntity = repoBusHrs.Find(filterBusHrs).ToArray()
                                        If bushrsEntity.Length > 0 Then
                                            dr("StartTime") = bushrsEntity(0).StartTime
                                            dr("Duration") = bushrsEntity(0).Duration
                                            dr("Day") = String.Join(",", bushrsEntity(0).Days)
                                        End If
                                        dt.Rows.Add(dr)
                                    Next
                                Next
                            End If
                        End If
                    Next
                Next
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error getting a handle on one of the repositories or collections: " & ex.Message, LogLevel.Normal)
        End Try

        ReDim ADefArr(0)
        ReDim keyArr(0)
        keyArr(0) = 0
        c = 0
        '1.a. Add alert definition objects into an array
        If Not IsNothing(dt) Then
            Try
                If dt.Rows.Count > 0 Then
                    For i As Integer = 0 To dt.Rows.Count - 1
                        ADef = New AlertDefinition
                        ADef.AlertKey = dt.Rows(i)("AlertKey")
                        ADef.EventName = dt.Rows(i)("EventName")
                        ADef.ServerType = dt.Rows(i)("ServerType")
                        ADef.ServerName = dt.Rows(i)("ServerName")
                        ADef.SendTo = dt.Rows(i)("SendTo")
                        ADef.CopyTo = IIf(dt.Rows(i)("CopyTo") Is Nothing, "", dt.Rows(i)("CopyTo").ToString())
                        ADef.BlindCopyTo = IIf(dt.Rows(i)("BlindCopyTo") Is Nothing, "", dt.Rows(i)("BlindCopyTo").ToString())
                        ADef.StartTime = dt.Rows(i)("StartTime")
                        ADef.Duration = dt.Rows(i)("Duration")
                        ADef.Days = dt.Rows(i)("Day")
                        ADef.IntType = dt.Rows(i)("HoursIndicator")
                        ADef.Details = ""
                        ADef.SendSNMPTrap = Convert.ToBoolean(dt.Rows(i)("SendSNMPTrap"))
                        ADef.EnablePersistentAlert = Convert.ToBoolean(dt.Rows(i)("EnablePersistentAlert"))
                        ADef.NotifyOnRepeat = Convert.ToBoolean(dt.Rows(i)("NotifyOnRepeat"))
                        ADef.DateCreated = ""
                        ADef.SMSTo = dt.Rows(i)("SMSTo")
                        ADef.ScriptName = dt.Rows(i)("ScriptName")
                        ADef.ScriptCommand = dt.Rows(i)("ScriptCommand")
                        ADef.ScriptLocation = dt.Rows(i)("ScriptLocation")
                        ReDim Preserve keyArr(c)
                        keyArr(c) = ADef.AlertKey
                        ReDim Preserve ADefArr(c)
                        ADefArr(c) = ADef
                        c = c + 1
                    Next
                End If
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error adding alert definition objects into an array: " & ex.Message, LogLevel.Normal)
            End Try
        End If

        Try
            If Not ADefArr(0) Is Nothing Then
                If ADefArr(0).AlertKey.ToString <> "" Then
                    c = 0
                    ReDim ADefArrOut(0)
                    Dim ADefSort As New AlertDefinition()
                    Array.Sort(ADefArr, ADefSort)
                    For i As Integer = 0 To ADefArr.Length - 1
                        If keyList.Contains(ADefArr(i).AlertKey) Then
                            ReDim Preserve ADefArrOut(c)
                            ADefArrOut(c) = ADefArr(i)
                            c = c + 1
                        Else
                            If c > 0 Then
                                If ADefArrOut(c - 1).AlertKey.ToString <> "" Then
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification: trying to add item #" & c & ", key " & ADefArrOut(c - 1).AlertKey & ", i " & i & ", item " & ADefArr(i).AlertKey, LogLevel.Verbose)
                                    AlertDict.Add(ADefArrOut(c - 1).AlertKey, ADefArrOut)
                                End If
                            End If
                            keyList.Add(ADefArr(i).AlertKey)
                            c = 0
                            ReDim ADefArrOut(c)
                            ADefArrOut(c) = ADefArr(i)
                            c = c + 1
                        End If
                    Next
                    If ADefArrOut(c - 1).AlertKey.ToString <> "" Then
                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification: trying to add " & ADefArrOut(c - 1).AlertKey, LogLevel.Verbose)
                        AlertDict.Add(ADefArrOut(c - 1).AlertKey, ADefArrOut)
                    End If
                End If
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error building a dictionary of alerts: " & ex.Message, LogLevel.Normal)
        End Try

        Try
            filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(i) i.EventDismissed, False),
                                                             repoEventsDetected.Filter.Exists(Function(i) i.EventType, True))
            eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToArray()
            If eventsCreated.Length > 0 Then
                dt = New DataTable
                dt.Columns.Add("ID")
                dt.Columns.Add("AlertType")
                dt.Columns.Add("DeviceType")
                dt.Columns.Add("DeviceName")
                dt.Columns.Add("Details")
                dt.Columns.Add("DateTimeOfAlert")
                dt.Columns.Add("EventRepeatCount")


                For i As Integer = 0 To eventsCreated.Length - 1
                    dr = dt.NewRow()
                    dr("ID") = eventsCreated(i).Id
                    dr("AlertType") = eventsCreated(i).EventType
                    dr("DeviceType") = eventsCreated(i).DeviceType
                    dr("DeviceName") = eventsCreated(i).Device
                    dr("Details") = eventsCreated(i).Details
                    dr("DateTimeOfAlert") = eventsCreated(i).EventDetected
                    dr("EventRepeatCount") = eventsCreated(i).EventRepeatCount
                    dt.Rows.Add(dr)
                Next
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error processing detected events: " & ex.Message, LogLevel.Normal)
        End Try


        c = 0
        ReDim AHistArr(0)
        Try
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    AHist = New AlertDefinition
                    ReDim Preserve AHistArr(c)
                    AHistArr(c) = AHist
                    AHist.AlertKey = dt.Rows(i)("ID")
                    AHist.EventName = dt.Rows(i)("AlertType")
                    AHist.ServerType = dt.Rows(i)("DeviceType")
                    AHist.ServerName = dt.Rows(i)("DeviceName")
                    AHist.StartTime = ""
                    AHist.Details = dt.Rows(i)("Details")
                    AHist.SendSNMPTrap = False
                    AHist.EnablePersistentAlert = False
                    AHist.DateCreated = dt.Rows(i)("DateTimeOfAlert")
                    AHist.EventRepeatCount = dt.Rows(i)("EventRepeatCount")
                    c = c + 1
                Next
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error building the event history list: " & ex.Message, LogLevel.Normal)
        End Try

        ReDim ADefArrOut(0)
        c = 0
        '6. Go through the alert records in the dictionary object and try to match the records with those from events_detected.
        'Where there is a match, add the record into the alerts array which will then be used to send notifications.
        Try
            If Not IsNothing(AlertDict) And Not IsNothing(AHistArr) Then
                For Each pair In AlertDict
                    If Not IsNothing(pair) Then
                        For i As Integer = 0 To pair.Value.Length - 1
                            ADef = pair.Value(i)
                            For j As Integer = 0 To AHistArr.Length - 1
                                AHist = AHistArr(j)
                                If Not IsNothing(AHist) Then
                                    'Matching based on EventName, DeviceType, DeviceName or EventName and DeviceType if name is not available
                                    If (InStr(AHist.EventName, ADef.EventName) > 0 And ADef.ServerName = AHist.ServerName And ADef.ServerType = AHist.ServerType) Or
                                    (InStr(AHist.EventName, ADef.EventName) > 0 And ADef.ServerType = AHist.ServerType And ADef.ServerName = "") Then
                                        'Check whether the notification_on_repeat is set for the event and if so, whether the max number of repeats has been reached
                                        'prior to sending a notification
                                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - alertAboutRecurrences: " & alertAboutRecurrences & ", ADef.EventName: " & ADef.EventName & ", ADef.NotifyOnRepeat: " & ADef.NotifyOnRepeat & ", AHist.EventRepeatCount: " & AHist.EventRepeatCount, LogLevel.Verbose)
                                        If ADef.NotifyOnRepeat And alertAboutRecurrences And AHist.EventRepeatCount + 1 >= numberOfRecurrences Or Not ADef.NotifyOnRepeat Or Not alertAboutRecurrences Then
                                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - found a match for " & AHist.EventName & ", " & AHist.ServerName & ", " & AHist.ServerType, LogLevel.Verbose)
                                            ADefOut = New AlertDefinition
                                            ADefOut.AlertHistoryId = AHist.AlertKey
                                            ADefOut.AlertKey = ADef.AlertKey
                                            ADefOut.EventName = AHist.EventName
                                            ADefOut.ServerType = ADef.ServerType
                                            ADefOut.ServerName = AHist.ServerName
                                            ADefOut.SendTo = ADef.SendTo
                                            ADefOut.CopyTo = ADef.CopyTo
                                            ADefOut.BlindCopyTo = ADef.BlindCopyTo
                                            ADefOut.StartTime = ADef.StartTime
                                            ADefOut.Duration = ADef.Duration
                                            ADefOut.Days = ADef.Days
                                            ADefOut.IntType = ADef.IntType
                                            ADefOut.Details = AHist.Details
                                            ADefOut.SendSNMPTrap = ADef.SendSNMPTrap
                                            ADefOut.EnablePersistentAlert = ADef.EnablePersistentAlert
                                            ADefOut.DateCreated = AHist.DateCreated
                                            ADefOut.SMSTo = ADef.SMSTo
                                            ADefOut.ScriptName = ADef.ScriptName
                                            ADefOut.ScriptCommand = ADef.ScriptCommand
                                            ADefOut.ScriptLocation = ADef.ScriptLocation
                                            ReDim Preserve ADefArrOut(c)
                                            ADefArrOut(c) = ADefOut
                                            c = c + 1
                                        End If
                                    End If
                                End If
                            Next
                        Next
                    End If
                Next
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error processing the alert distionary data set: " & ex.Message, LogLevel.Normal)
        End Try

        '7. Process alert records, identify whether any of them fall within the current time frame for sending. 
        IsItTimeToSendAlert(ADefArrOut)

        'check if max alerts are sent today and see how may alerts per def are sent
        'WriteServiceHistoryEntry(Now.ToString & " Here #8")
        '8. Check the AlertSentDetails table and send an e-mail notification if there is no entry for each AlertHistoryID value
        Dim SendTo As String
        Dim CC As String
        Dim BCC As String
        Dim mailsent As Boolean
        Dim SMSTo As String
        Dim smssent As Boolean
        Dim ScriptName As String
        Dim ScriptCommand As String
        Dim ScriptLocation As String
        Dim scriptsent As Boolean
        Dim sSource As String
        Dim sLog As String
        Dim sEvent As String
        Dim dontNeedToSend As Boolean
        Dim resend As Boolean
        Dim persistentInterval As Integer
        Dim persistentDuration As Integer
        Dim alertDateCurrent As Date
        Dim alertDateCreated As Date
        Dim noNewRecipients As Boolean
        Dim nowTime As Date

        SendTo = ""
        CC = ""
        BCC = ""
        SMSTo = ""
        ScriptName = ""
        ScriptCommand = ""
        ScriptLocation = ""
        persistentInterval = 0
        Try
            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to get PersistentAlertInterval from the name_value collection", LogLevel.Verbose)
            tempVal = getSettings("AlertInterval")
            If tempVal <> "" Then
                persistentInterval = Convert.ToInt32(tempVal)
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error getting Persistent Alert Interval from the name_value collection:  " & ex.ToString, LogLevel.Normal)
        End Try
        persistentDuration = 0
        Try
            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to get PersistentAlertDuration from the name_value collection", LogLevel.Verbose)
            tempVal = getSettings("AlertDuration")
            If tempVal <> "" Then
                If IsNumeric(tempVal) Then
                    persistentDuration = Convert.ToInt32(tempVal)
                End If
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error getting Persistent Alert Duration from the name_value collection:  " & ex.ToString, LogLevel.Normal)
        End Try
        Dim maxAllowedTodayCount = 0
        'get the max allowed count here, by doing the computation
        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to GetMaxAlertsRemainingToday", LogLevel.Verbose)
        Dim TotalMaxAlertsPerDay = GetMaxAlertsRemainingToday()
        If TotalMaxAlertsPerDay >= 0 Then
            'trim the alerts collection
            If ADefArrOut.Length > TotalMaxAlertsPerDay Then
                maxAllowedTodayCount = TotalMaxAlertsPerDay - 1
            Else
                maxAllowedTodayCount = ADefArrOut.Length - 1
            End If
        Else
            maxAllowedTodayCount = ADefArrOut.Length - 1
        End If
        'get the alertkey by matching the history id with alertkey
        If Not ADefArrOut(0) Is Nothing Then
            For i = 0 To maxAllowedTodayCount
                'now check the max alerts per def here
                ADef = ADefArrOut(i)
                noNewRecipients = False
                WriteServiceHistoryEntry(Now.ToString & "ADefOut " & i.ToString() & ": " & ADef.AlertKey & ", " & ADef.AlertHistoryId & ", " & ADef.SendTo & ", " & ADef.SMSTo)
                'get max alerts remaining to be sent today for this def
                Dim MaxAlertsperDef = GetMaxAlertsRemainingToday(ADef.AlertKey)
                'if max is not reached or max alerts per def setting is not set, then send, else bail out
                If MaxAlertsperDef = -1 Or MaxAlertsperDef > 0 Then
                    dontNeedToSend = False
                    resend = False
                    Try
                        Dim dtmail As DataTable = New DataTable
                        dtmail.Columns.Add("SentTo")
                        dtmail.Columns.Add("CcdTo")
                        dtmail.Columns.Add("BccdTo")
                        dtmail.Columns.Add("AlertClearedDateTime")
                        dtmail.Columns.Add("AlertCreatedDateTime")
                        '8.1 Locate the event by event id (AlertHistoryId) in the events_detected collection where a notification has already been sent
                        filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True),
                                      repoEventsDetected.Filter.Eq(Of String)(Function(j) j.Id, ADef.AlertHistoryId))
                        eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToArray()
                        If eventsCreated.Length > 0 Then
                            For j As Integer = 0 To eventsCreated.Length - 1
                                notificationsSent = eventsCreated(j).NotificationsSent.ToArray()
                                For x As Integer = 0 To notificationsSent.Length - 1
                                    dr = dtmail.NewRow()
                                    dr("SentTo") = notificationsSent(x).NotificationSentTo
                                    dr("CcdTo") = notificationsSent(x).NotificationCcdTo
                                    dr("BccdTo") = notificationsSent(x).NotificationBccdTo
                                    dr("AlertClearedDateTime") = notificationsSent(x).EventDismissedSent
                                    dr("AlertCreatedDateTime") = notificationsSent(x).EventDetectedSent
                                    dtmail.Rows.Add(dr)
                                Next
                            Next
                        End If

                        If dtmail.Rows.Count > 0 Then
                            '8.1.1 Check if persistent alerting is enabled. Notify again only if the time interval between the previous
                            'send and the current time is greater than or equal to the name_value value.
                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification: event record has previous notifications." & SendTo, LogLevel.Verbose)
                            If ADef.SendTo <> "" Or ADef.SMSTo <> "" Or ADef.ScriptName <> "" Then
                                If ADef.EnablePersistentAlert = True And persistentInterval > 0 Then
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification: processing persistent notifications. ", LogLevel.Verbose)
                                    alertDateCurrent = Convert.ToDateTime(dtmail.Rows(dtmail.Rows.Count - 1)("AlertCreatedDateTime").ToString())
                                    alertDateCurrent = alertDateCurrent.AddMinutes(persistentInterval)
                                    nowTime = Now
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification: persistentDuration: " & persistentDuration & ", alertDateCurrent: " & alertDateCurrent & ", nowTime: " & nowTime, LogLevel.Verbose)
                                    '8.1.2 If persistent alerting is unlimited, we don't care about the original alert creation date
                                    If persistentDuration = 0 Then
                                        If nowTime < alertDateCurrent Then
                                            dontNeedToSend = True
                                        Else
                                            resend = True
                                        End If
                                        '8.1.3 If persistent alerting is limited, we need to see if we are still within the bounds of the time limit
                                    Else
                                        alertDateCreated = Convert.ToDateTime(ADef.DateCreated)
                                        alertDateCreated = alertDateCreated.AddHours(persistentDuration)
                                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification: persistentDuration: " & persistentDuration & ", alertDateCreated: " & alertDateCreated & ", nowTime: " & nowTime, LogLevel.Verbose)
                                        If nowTime > alertDateCreated Or nowTime < alertDateCurrent Then
                                            dontNeedToSend = True
                                        Else
                                            resend = True
                                        End If
                                    End If
                                End If
                                If ADef.EnablePersistentAlert = False Or dontNeedToSend = True Then
                                    noNewRecipients = True
                                    SendTo = ""
                                    CC = ""
                                    BCC = ""
                                    SMSTo = ""
                                    ScriptName = ""
                                    ScriptCommand = ""
                                    ScriptLocation = ""
                                Else
                                    SendTo = ADef.SendTo
                                    CC = ADef.CopyTo
                                    BCC = ADef.BlindCopyTo
                                    SMSTo = ADef.SMSTo
                                    ScriptName = ADef.ScriptName
                                    ScriptCommand = ADef.ScriptCommand
                                    ScriptLocation = ADef.ScriptLocation
                                End If
                            Else
                                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification: no new recipients. " & SendTo, LogLevel.Verbose)
                                noNewRecipients = True
                                SendTo = ""
                                CC = ""
                                BCC = ""
                                SMSTo = ""
                                ScriptName = ""
                                ScriptCommand = ""
                                ScriptLocation = ""
                            End If
                        Else
                            SendTo = ADef.SendTo
                            If ADef.CopyTo <> "" Then
                                CC = ADef.CopyTo
                            End If
                            If ADef.BlindCopyTo <> "" Then
                                BCC = ADef.BlindCopyTo
                            End If
                            SMSTo = ADef.SMSTo
                            ScriptName = ADef.ScriptName
                            ScriptCommand = ADef.ScriptCommand
                            ScriptLocation = ADef.ScriptLocation
                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification: event record has no previous notifications. SendTo: " & SendTo, LogLevel.Verbose)
                        End If
                    Catch ex As ApplicationException
                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error occurred at the time of getting records from events_detected " & ex.Message, LogLevel.Normal)
                    End Try
                    mailsent = False
                    smssent = False
                    scriptsent = False
                    If noNewRecipients Then
                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - found NO new recipients", LogLevel.Verbose)
                    Else
                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - found new recipients", LogLevel.Verbose)
                        Try
                            If ADef.RunNow = 1 Then
                                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification: time to process the alert", LogLevel.Verbose)
                                If SendTo <> "" Then
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - attempting to send new alert via email", LogLevel.Verbose)
                                    WriteServiceHistoryEntry(Now.ToString & " Attempting to send a new alert via e-mail:", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & "   SendTo = " & SendTo & IIf(CC = "", "", ",    CopyTo = " + CC) & IIf(BCC = "", "", ",    BlindCopyTo = " + BCC) & ",   ServerName = " & ADef.ServerName & ",    AlertKey = " & ADef.AlertKey, LogLevel.Normal)
                                    If (ADef.EventName.ToString = "Services") Then
                                        Dim newString As String = ADef.Details.Substring(ADef.Details.IndexOf(" ") + 1)
                                        Dim newStrings As String() = ADef.Details.Split(New String() {" "}, StringSplitOptions.None)
                                        Dim services As String
                                        services = newStrings(0).ToString
                                        Dim substringofservices As String() = services.Split(",")
                                        ADef.Details = " " & vbCrLf & newString & vbCrLf & "The services are "
                                        If (substringofservices.Length = 1) Then
                                            ADef.Details = " " & vbCrLf & "The " & newString
                                        Else
                                            For Each substring In substringofservices
                                                ADef.Details &= vbCrLf & substring
                                            Next
                                            Dim s As String = ADef.Details
                                        End If
                                    End If
                                    SendMailwithChilkatorNet(SendTo, CC, BCC, ADef.ServerName, ADef.ServerType, "", ADef.EventName, ADef.EventName, ADef.Details, "", "Alert")
                                    mailsent = True
                                ElseIf ADef.SendSNMPTrap <> "False" Then
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification: SNMP trap.", LogLevel.Verbose)
                                    '***** SNMP Conditions *********
                                    Try
                                        Dim SNMPHostName As String = getSettings("SNMPHostName")
                                        If ADef.SendSNMPTrap = "True" And SNMPHostName <> "" Then
                                            SendSNMPTrap(SNMPHostName, ADef.ServerType, ADef.ServerName, ADef.EventName, ADef.Details)
                                        End If
                                        SendTo = "SNMP Trap"
                                        mailsent = True
                                    Catch ex As ApplicationException
                                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error occurred at SendSNMPTrap " & ex.Message, LogLevel.Normal)
                                    End Try
                                ElseIf SendTo = "Windows Log" Then
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification: Windows Log.", LogLevel.Verbose)
                                    sSource = "VitalSigns Plus"
                                    sLog = "Application"
                                    sEvent = "Alert for " & ADef.ServerType & " " & ADef.ServerName & " - " & ADef.EventName & ". " & ADef.Details
                                    Try
                                        If Not EventLog.SourceExists(sSource) Then
                                            EventLog.CreateEventSource(sSource, sLog)
                                        End If

                                        EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234)
                                    Catch ex As Exception
                                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error writing to Windows log for alert ID " & ADef.AlertKey & ": " & ex.Message, LogLevel.Normal)
                                    End Try
                                    SendTo = "Windows Log"
                                    mailsent = True
                                End If
                            End If
                        Catch ex As ApplicationException
                            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error occurred at the time of sending an Alert Mail " & ex.Message, LogLevel.Normal)
                            mailsent = False
                        End Try
                        Try
                            If ADef.RunNow = 1 Then
                                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification: time to process the alert", LogLevel.Verbose)
                                If SMSTo <> "" Then
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - attempting to send new alert via SMS", LogLevel.Verbose)
                                    WriteServiceHistoryEntry(Now.ToString & " Attempting to send a new alert via SMS:", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & "   SMSTo = " & SMSTo & ",   ServerName = " & ADef.ServerName & ",    AlertKey = " & ADef.AlertKey, LogLevel.Normal)
                                    SendSMSwithTwilio(SMSTo, ADef.ServerName, ADef.ServerType, ADef.EventName, ADef.Details, "", "")
                                    smssent = True
                                End If
                            End If
                        Catch ex As ApplicationException
                            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error occurred at the time of sending an Alert SMS " & ex.Message, LogLevel.Normal)
                            smssent = False
                        End Try
                        Try
                            If ADef.RunNow = 1 Then
                                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification: time to process the alert", LogLevel.Verbose)
                                If ScriptName <> "" Then
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - attempting to send new alert via Script", LogLevel.Verbose)
                                    WriteServiceHistoryEntry(Now.ToString & " Attempting to send a new alert via Script:", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & "   ScriptName = " & ScriptName & ",   ServerName = " & ADef.ServerName & ",    AlertKey = " & ADef.AlertKey, LogLevel.Normal)
                                    SendScript(ScriptName, ScriptCommand, ScriptLocation, ADef.ServerName, ADef.ServerType, ADef.EventName, ADef.Details, "")
                                    scriptsent = True
                                End If
                            End If
                        Catch ex As ApplicationException
                            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error occurred at the time of sending an Alert SMS " & ex.Message, LogLevel.Normal)
                            scriptsent = False
                        End Try
                    End If
                    Try
                        If mailsent = True Then
                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to insert sent email information", LogLevel.Verbose)
                            InsertingSentMails(ADef.AlertHistoryId, SendTo, CC, BCC, resend, ADef.AlertKey)
                            If (ADef.Details = "This is a TEST alert.") Or InStr(ADef.EventName, "Log File") Then
                                If (ADef.Details = "This is a TEST alert.") Then
                                    WriteServiceHistoryEntry(Now.ToString & " This is a TEST alert and will be cleared instantly", LogLevel.Normal)
                                Else
                                    WriteServiceHistoryEntry(Now.ToString & " This is a Log File alert and will be cleared instantly", LogLevel.Normal)
                                End If
                                Dim strdt As String
                                strdt = Date.Now
                                Try
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to update event_dismissed in events_detected", LogLevel.Verbose)
                                    filterEventsDetected = repoEventsDetected.Filter.Eq(Of String)(Function(j) j.Id, ADef.AlertHistoryId)
                                    updateEventsDetected = repoEventsDetected.Updater.Set(Of DateTime)(Function(j) j.EventDismissed, strdt)
                                    repoEventsDetected.Update(filterEventsDetected, updateEventsDetected)

                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to update event_dismissed_sent in events_detected.notifications_sent", LogLevel.Verbose)
                                    UpdatingSentMails(ADef.AlertHistoryId, SendTo, CC, BCC, ADef.AlertKey)
                                Catch ex As Exception
                                    WriteServiceHistoryEntry(Now.ToString & " Error occurred at the time of updating the events_detected for the Alert with ID of " & ADef.AlertHistoryId & ": " & ex.Message, LogLevel.Normal)
                                End Try
                            End If

                        End If
                    Catch ex As Exception
                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error while attempting to insert Alert Mail history for emails " & ex.ToString, LogLevel.Normal)
                    End Try
                    Try
                        If smssent = True Then
                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to insert sent SMS info", LogLevel.Verbose)
                            InsertingSentMails(ADef.AlertHistoryId, SMSTo, "", "", resend, ADef.AlertKey)
                        End If
                    Catch ex As Exception
                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error while attempting to insert Alert Mail history for SMS " & ex.ToString, LogLevel.Normal)
                    End Try
                    Try
                        If scriptsent = True Then
                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to insert sent Script info", LogLevel.Verbose)
                            InsertingSentMails(ADef.AlertHistoryId, ScriptName, "", "", resend, ADef.AlertKey)
                        End If
                    Catch ex As Exception
                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error while attempting to insert Alert Mail history for Script " & ex.ToString, LogLevel.Normal)
                    End Try
                Else
                    WriteServiceHistoryEntry(Now.ToString & " Max Alerts for this def reached: No more alerts will be sent", LogLevel.Normal)
                End If
            Next

            'ESCALATION
            'Since escalation is Device, Device Type, and Event Type specific, filter out all user specific details from ADefArrOut
            Dim ADefArrE = New List(Of AlertDefinition)
            For n = 0 To ADefArrOut.Length - 1
                ADef = New AlertDefinition
                ADef.ServerName = ADefArrOut(n).ServerName
                ADef.ServerType = ADefArrOut(n).ServerType
                ADef.EventName = ADefArrOut(n).EventName
                If Not ADefArrE.Any(Function(j) j.ServerName = ADef.ServerName And j.ServerType = ADef.ServerType And j.EventName = ADef.EventName) Then
                    ADefArrE.Add(ADef)
                End If
            Next
            Try
                Dim alertcreated As DateTime
                Dim esmssent As Boolean = False
                Dim eemailsent As Boolean = False
                Dim notifications As List(Of String)
                'Find all Notifications that have Escalation defined. Use existence of Interval as a filter
                filterNotifications = repoNotifications.Filter.Exists(Function(j) j.Id, True)
                notificationsEntity = repoNotifications.Find(filterNotifications).ToArray()
                For i As Integer = 0 To notificationsEntity.Length - 1
                    filterNotificationDest = repoNotificationDest.Filter.And(repoNotificationDest.Filter.Exists(Function(x) x.Interval),
                                                                             repoNotificationDest.Filter.In(Of String)(Function(j) j.Id, notificationsEntity(i).SendList))
                    notificationDestEntity = repoNotificationDest.Find(filterNotificationDest).OrderBy(Function(j) j.Interval).ToArray()
                    For z As Integer = 0 To notificationDestEntity.Length - 1
                        If Not IsNothing(notificationDestEntity(z).Interval) Then
                            eid = notificationsEntity(i).ObjectId.ToString()
                            'Find all events for which notifications have gone out (no Log File events)
                            filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True),
                                                                                 repoEventsDetected.Filter.Exists(Function(j) j.EventDismissed, False),
                                                                                 repoEventsDetected.Filter.Not(repoEventsDetected.Filter.Regex(Function(j) j.EventType, New BsonRegularExpression("log file"))),
                                                                                 repoEventsDetected.Filter.ElemMatch(Function(j) j.NotificationsSent, New FilterDefinitionBuilder(Of NotificationsSent)().Eq(Function(k) k.NotificationId, eid)))
                            eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToArray()
                            'Find all events for which both notifications and escalations have gone out
                            filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True),
                                                                                 repoEventsDetected.Filter.Exists(Function(j) j.EventDismissed, False),
                                                                                 repoEventsDetected.Filter.Not(repoEventsDetected.Filter.Regex(Function(j) j.EventType, New BsonRegularExpression("log file"))),
                                                                                 repoEventsDetected.Filter.ElemMatch(Function(j) j.NotificationsSent, New FilterDefinitionBuilder(Of NotificationsSent)().Eq(Function(k) k.EscalationId, eid)),
                                                                                 repoEventsDetected.Filter.ElemMatch(Function(j) j.NotificationsSent, New FilterDefinitionBuilder(Of NotificationsSent)().Eq(Function(k) k.NotificationId, eid)))
                            eventsEscalated = repoEventsDetected.Find(filterEventsDetected).ToArray()
                            'We could not find an event that has been escalated for the current notification/escalation, so we need to continue
                            If eventsCreated.Length > 0 And eventsEscalated.Length = 0 Then
                                For Each eDef In ADefArrE
                                    For x As Integer = 0 To eventsCreated.Length - 1
                                        If eventsCreated(x).EventType = eDef.EventName And eventsCreated(x).Device = eDef.ServerName And
                                            eventsCreated(x).DeviceType = eDef.ServerType Then
                                            'Based on whether the current time has passed the interval from the time of the original event,
                                            'send an escalation
                                            alertcreated = Convert.ToDateTime(eventsCreated(x).EventDetected)
                                            If (Now - alertcreated).TotalMinutes >= Convert.ToInt32(notificationDestEntity(z).Interval) Then
                                                '3. Send escalation
                                                If notificationDestEntity(z).SendTo <> "" And notificationDestEntity(z).SendVia = "E-mail" Then
                                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - attempting to send an escalation via email", LogLevel.Verbose)
                                                    WriteServiceHistoryEntry(Now.ToString & " Attempting to send an escalation via email:", LogLevel.Normal)
                                                    WriteServiceHistoryEntry(Now.ToString & "   EscalateTo = " & notificationDestEntity(z).SendTo & ",   Event = " & eventsCreated(x).ObjectId.ToString(), LogLevel.Normal)
                                                    SendMailwithChilkatorNet(notificationDestEntity(z).SendTo, "", "", eventsCreated(x).Device, eventsCreated(x).DeviceType, "", eventsCreated(x).EventType, eventsCreated(x).EventType, eventsCreated(x).Details, "", "ESCALATION")
                                                    eemailsent = True
                                                End If
                                                If notificationDestEntity(z).SendTo <> "" And notificationDestEntity(z).SendVia = "SMS" Then
                                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - attempting to send an escalation via SMS", LogLevel.Verbose)
                                                    WriteServiceHistoryEntry(Now.ToString & " Attempting to send an escalation via SMS:", LogLevel.Normal)
                                                    WriteServiceHistoryEntry(Now.ToString & "   SMSTo = " & notificationDestEntity(z).SendTo & ",    Event = " & eventsCreated(x).ObjectId.ToString(), LogLevel.Normal)
                                                    SendSMSwithTwilio(notificationDestEntity(z).SendTo, eventsCreated(x).Device, eventsCreated(x).DeviceType, eventsCreated(x).EventType, eventsCreated(x).Details, "ESCALATION ", "")
                                                    esmssent = True
                                                End If
                                                '4. Store escalation information
                                                If eemailsent Or esmssent Then
                                                    'Update notifications_sent embedded documents in the events_detected document
                                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to insert sent email escalation info", LogLevel.Verbose)
                                                    InsertSentEscalation(eventsCreated(x), eid, eid, notificationDestEntity(z).SendTo)
                                                End If
                                            End If
                                        End If
                                    Next
                                Next
                            End If
                        End If
                    Next
                Next

                'filterNotificationDest = repoNotificationDest.Filter.Exists(Function(x) x.Interval)
                'notificationDestEntity = repoNotificationDest.Find(filterNotificationDest).ToArray()
                'For n As Integer = 0 To notificationDestEntity.Length - 1
                '    filterNotifications = repoNotifications.Filter.ElemMatch(Of String)(Function(j) j.SendList, notificationDestEntity(n).Id)
                '    notificationsEntity = repoNotifications.Find(filterNotifications).ToArray()
                '    If notificationsEntity.Length > 0 Then
                '        For i As Integer = 0 To notificationsEntity.Length - 1
                '            oid = notificationsEntity(i).ObjectId.ToString()
                '            'Get Escalation documents
                '            notifications = notificationDestEntity(n).SendTo
                '            notifications = notifications.ToList().OrderBy(Function(j) j.Interval).ToArray()
                '            For z As Integer = 0 To notifications.Length - 1
                '                If Not IsNothing(notifications(z).Interval) Then
                '                    eid = notifications(z).ObjectId.ToString()
                '                    'Find all events for which notifications have gone out (no Log File events)
                '                    filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True),
                '                                                                         repoEventsDetected.Filter.Exists(Function(j) j.EventDismissed, False),
                '                                                                         repoEventsDetected.Filter.Not(repoEventsDetected.Filter.Regex(Function(j) j.EventType, New BsonRegularExpression("log file"))),
                '                                                                         repoEventsDetected.Filter.ElemMatch(Function(j) j.NotificationsSent, New FilterDefinitionBuilder(Of NotificationsSent)().Eq(Function(k) k.NotificationId, oid)))
                '                    eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToArray()
                '                    'Find all events for which both notifications and escalations have gone out
                '                    filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True),
                '                                                                         repoEventsDetected.Filter.Exists(Function(j) j.EventDismissed, False),
                '                                                                         repoEventsDetected.Filter.Not(repoEventsDetected.Filter.Regex(Function(j) j.EventType, New BsonRegularExpression("log file"))),
                '                                                                         repoEventsDetected.Filter.ElemMatch(Function(j) j.NotificationsSent, New FilterDefinitionBuilder(Of NotificationsSent)().Eq(Function(k) k.EscalationId, eid)),
                '                                                                         repoEventsDetected.Filter.ElemMatch(Function(j) j.NotificationsSent, New FilterDefinitionBuilder(Of NotificationsSent)().Eq(Function(k) k.NotificationId, oid)))
                '                    eventsEscalated = repoEventsDetected.Find(filterEventsDetected).ToArray()
                '                    'We could not find an event that has been escalated for the current notification/escalation, so we need to continue
                '                    If eventsCreated.Length > 0 And eventsEscalated.Length = 0 Then
                '                        For Each eDef In ADefArrE
                '                            For x As Integer = 0 To eventsCreated.Length - 1
                '                                If eventsCreated(x).EventType = eDef.EventName And eventsCreated(x).Device = eDef.ServerName And
                '                                    eventsCreated(x).DeviceType = eDef.ServerType Then
                '                                    'Based on whether the current time has passed the interval from the time of the original event,
                '                                    'send an escalation
                '                                    alertcreated = Convert.ToDateTime(eventsCreated(x).EventDetected)
                '                                    If (Now - alertcreated).TotalMinutes >= Convert.ToInt32(notifications(z).Interval) Then
                '                                        '3. Send escalation
                '                                        If notifications(z).SendTo <> "" And notifications(z).SendVia = "email" Then
                '                                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - attempting to send an escalation via email", LogLevel.Verbose)
                '                                            WriteServiceHistoryEntry(Now.ToString & " Attempting to send an escalation via email:", LogLevel.Normal)
                '                                            WriteServiceHistoryEntry(Now.ToString & "   EscalateTo = " & notifications(z).SendTo & ",   Event = " & eventsCreated(x).ObjectId.ToString(), LogLevel.Normal)
                '                                            SendMailwithChilkatorNet(notifications(z).SendTo, "", "", eventsCreated(x).Device, eventsCreated(x).DeviceType, "", eventsCreated(x).EventType, eventsCreated(x).EventType, eventsCreated(x).Details, "", "ESCALATION")
                '                                            eemailsent = True
                '                                        End If
                '                                        If notifications(z).SendTo <> "" And notifications(z).SendVia = "sms" Then
                '                                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - attempting to send an escalation via SMS", LogLevel.Verbose)
                '                                            WriteServiceHistoryEntry(Now.ToString & " Attempting to send an escalation via SMS:", LogLevel.Normal)
                '                                            WriteServiceHistoryEntry(Now.ToString & "   SMSTo = " & notifications(z).SendTo & ",    Event = " & eventsCreated(x).ObjectId.ToString(), LogLevel.Normal)
                '                                            SendSMSwithTwilio(notifications(z).SendTo, eventsCreated(x).Device, eventsCreated(x).DeviceType, eventsCreated(x).EventType, eventsCreated(x).Details, "ESCALATION ", "")
                '                                            esmssent = True
                '                                        End If
                '                                        '4. Store escalation information
                '                                        If eemailsent Or esmssent Then
                '                                            'Update notifications_sent embedded documents in the events_detected document
                '                                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to insert sent email escalation info", LogLevel.Verbose)
                '                                            InsertSentEscalation(eventsCreated(x), oid, eid, notifications(z).SendTo)
                '                                        End If
                '                                    End If
                '                                End If
                '                            Next
                '                        Next
                '                    End If
                '                End If
                '            Next
                '        Next
                '    End If
                'Next

            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error while selecting Escalation details " & ex.ToString, LogLevel.Normal)
            End Try
        End If
    End Sub
    Private Sub ProcessAlertsClearSendNotification()
        Try
            Dim mailsent As Boolean = False
            Dim Details As String = ""
            Dim AlertID As String = ""
            Dim SentID As String = ""
            Dim ServerName As String = ""
            Dim EventName As String = ""
            Dim Location As String = ""
            Dim ServerType As String = ""
            Dim AlertType As String = ""
            Dim CC As String = ""
            Dim BCC As String = ""
            Dim SendTo As String = ""
            Dim SMSTo As String = ""
            Dim smssent As Boolean = False
            Dim ScriptName As String = ""
            Dim ScriptCommand As String = ""
            Dim ScriptLocation As String = ""
            Dim scriptsent As Boolean = False
            Dim Historytable As DataTable = New DataTable()
            Dim dr As DataRow
            Dim AHistArr As AlertDefinition()
            Dim ADefArrOut As AlertDefinition()
            Dim ADef As AlertDefinition
            Dim ADefOut As AlertDefinition
            Dim AHist As AlertDefinition
            Dim c As Integer
            Dim AlertDictOut As New Dictionary(Of String, String)

            Dim connString As String = GetDBConnection()
            Dim repoEventsMaster As New Repository(Of EventsMaster)(connString)
            Dim repoNotifications As New Repository(Of Notifications)(connString)
            Dim repoServers As New Repository(Of Server)(connString)
            Dim repoBusHrs As New Repository(Of BusinessHours)(connString)
            Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)

            Dim filterEventsDetected As FilterDefinition(Of EventsDetected)
            Dim eventsCreated() As EventsDetected
            Dim notificationsSent() As NotificationsSent

            Try
                Historytable.Columns.Add("ID")
                Historytable.Columns.Add("sentid")
                Historytable.Columns.Add("sentto")
                Historytable.Columns.Add("ccdto")
                Historytable.Columns.Add("bccdto")
                Historytable.Columns.Add("DeviceName")
                Historytable.Columns.Add("DeviceType")
                Historytable.Columns.Add("AlertType")
                Historytable.Columns.Add("Details")
                Historytable.Columns.Add("DateTimeAlertCleared")
                Historytable.Columns.Add("DateTimeOfAlert")

                'filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.ElemMatch(Of NotificationsSent)(Function(j) j.NotificationsSent, New FilterDefinitionBuilder(Of NotificationsSent)().Exists(Function(k) k.EventDismissedSent, False)),
                '                                                     repoEventsDetected.Filter.Exists(Function(j) j.EventDismissed, True))
                filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True),
                                                                     repoEventsDetected.Filter.Exists(Function(j) j.EventDismissed, True))
                eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToArray()
                If eventsCreated.Length > 0 Then
                    For i As Integer = 0 To eventsCreated.Length - 1
                        notificationsSent = eventsCreated(i).NotificationsSent.ToArray()
                        If notificationsSent.Length > 0 Then
                            For k As Integer = 0 To notificationsSent.Length - 1
                                If IsNothing(notificationsSent(k).EventDismissedSent) Then
                                    dr = Historytable.NewRow()
                                    dr("ID") = eventsCreated(i).ObjectId.ToString() 'notificationsSent(k).ObjectId.ToString()
                                    dr("sentid") = eventsCreated(i).ObjectId.ToString()
                                    dr("sentto") = notificationsSent(k).NotificationSentTo
                                    dr("ccdto") = ""
                                    If Not notificationsSent(k).NotificationCcdTo Is Nothing Then
                                        dr("ccdto") = notificationsSent(k).NotificationCcdTo
                                    End If
                                    dr("bccdto") = ""
                                    If Not notificationsSent(k).NotificationBccdTo Is Nothing Then
                                        dr("bccdto") = notificationsSent(k).NotificationBccdTo
                                    End If
                                    dr("DeviceName") = eventsCreated(i).Device
                                    dr("DeviceType") = eventsCreated(i).DeviceType
                                    dr("AlertType") = eventsCreated(i).EventType
                                    dr("Details") = eventsCreated(i).Details
                                    dr("DateTimeAlertCleared") = eventsCreated(i).EventDismissed
                                    dr("DateTimeOfAlert") = eventsCreated(i).EventDetected
                                    Historytable.Rows.Add(dr)
                                End If
                            Next
                        End If
                    Next
                End If
            Catch ex As ApplicationException
                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error  at the time of selecting records for Cleared Alerts from the events_detected collection: " & ex.Message, LogLevel.Normal)
            End Try
            c = 0
            ReDim AHistArr(c)
            If Not Historytable Is Nothing Then
                If (Historytable.Rows.Count > 0) Then
                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - found records from events_detected", LogLevel.Verbose)
                    For hst As Integer = 0 To Historytable.Rows.Count - 1
                        Try
                            ADef = New AlertDefinition
                            ADef.AlertKey = Historytable.Rows(hst)("ID")
                            ADef.SentID = Historytable.Rows(hst)("sentid")
                            ADef.SendTo = Historytable.Rows(hst)("sentto")
                            ADef.CopyTo = Historytable.Rows(hst)("ccdto")
                            ADef.BlindCopyTo = Historytable.Rows(hst)("bccdto")
                            ADef.ServerName = Historytable.Rows(hst)("DeviceName").ToString()
                            ADef.ServerType = Historytable.Rows(hst)("DeviceType").ToString()
                            ADef.EventName = Historytable.Rows(hst)("AlertType").ToString()
                            ADef.Details = Historytable.Rows(hst)("Details").ToString() & vbCrLf & vbCrLf &
                                "Alert condition was cleared at " & Historytable.Rows(hst)("DateTimeAlertCleared").ToString() &
                                vbCrLf & vbCrLf & "Detected: " & Historytable.Rows(hst)("DateTimeOfAlert").ToString()
                            ReDim Preserve AHistArr(c)
                            AHistArr(c) = ADef
                            c = c + 1
                        Catch ex As ApplicationException
                            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Exiting. For record #" + (hst + 1) + " of " + Historytable.Rows.Count + " records, error occurred at the time of assigning records to variables from events_detected: " & ex.Message, LogLevel.Normal)
                        End Try
                    Next
                End If
            End If

            c = 0
            Try
                If Not IsNothing(AlertDict) And Not IsNothing(AHistArr) Then
                    For Each pair In AlertDict
                        If Not IsNothing(pair) Then
                            For i As Integer = 0 To pair.Value.Length - 1
                                ADef = pair.Value(i)
                                For j As Integer = 0 To AHistArr.Length - 1
                                    AHist = AHistArr(j)
                                    If Not IsNothing(AHist) Then
                                        If (InStr(AHist.EventName, ADef.EventName) > 0 And ADef.ServerName = AHist.ServerName And ADef.ServerType = AHist.ServerType) Or
                                        (InStr(AHist.EventName, ADef.EventName) > 0 And ADef.ServerType = AHist.ServerType And ADef.ServerName = "") Then
                                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - found a match", LogLevel.Verbose)
                                            ADefOut = New AlertDefinition
                                            ADefOut.AlertKey = AHist.AlertKey
                                            ADefOut.SentID = AHist.SentID
                                            ADefOut.EventName = AHist.EventName
                                            ADefOut.ServerType = ADef.ServerType
                                            ADefOut.ServerName = AHist.ServerName
                                            ADefOut.SendTo = ADef.SendTo
                                            'ADefOut.SendTo = AHist.SendTo
                                            ADefOut.CopyTo = ADef.CopyTo
                                            ADefOut.BlindCopyTo = ADef.BlindCopyTo
                                            ADefOut.StartTime = ADef.StartTime
                                            ADefOut.Duration = ADef.Duration
                                            ADefOut.Days = ADef.Days
                                            ADefOut.IntType = ADef.IntType
                                            ADefOut.Details = AHist.Details
                                            ADefOut.SendSNMPTrap = ADef.SendSNMPTrap
                                            ADefOut.EnablePersistentAlert = ADef.EnablePersistentAlert
                                            ADefOut.DateCreated = AHist.DateCreated
                                            ADefOut.SMSTo = ADef.SMSTo
                                            ADefOut.ScriptName = ADef.ScriptName
                                            ADefOut.ScriptCommand = ADef.ScriptCommand
                                            ADefOut.ScriptLocation = ADef.ScriptLocation
                                            ReDim Preserve ADefArrOut(c)
                                            ADefArrOut(c) = ADefOut
                                            c = c + 1
                                        End If
                                    End If
                                Next
                            Next
                        End If
                    Next
                    'Clear the dictionary variable here
                    Try
                        AlertDict.Clear()
                    Catch ex As Exception
                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Could not clear AlertDict " & ex.Message, LogLevel.Normal)
                    End Try

                End If
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error processing the alert distionary data set: " & ex.Message, LogLevel.Normal)
            End Try

            If Not ADefArrOut Is Nothing Then
                IsItTimeToSendAlert(ADefArrOut)
                For i = 0 To ADefArrOut.Length - 1
                    ADef = ADefArrOut(i)
                    mailsent = False
                    smssent = False
                    scriptsent = False
                    SendTo = ""
                    CC = ""
                    BCC = ""
                    SMSTo = ""
                    ScriptName = ""
                    ScriptCommand = ""
                    ScriptLocation = ""
                    AlertID = ADef.AlertKey
                    SentID = ADef.SentID
                    ServerName = ADef.ServerName
                    ServerType = ADef.ServerType
                    EventName = ADef.EventName
                    AlertType = ADef.EventName
                    Details = ADef.Details
                    Try
                        If Not AlertDictOut.ContainsKey(SentID) Then
                            AlertDictOut.Add(SentID, SentID)
                            SendTo = ADef.SendTo
                            CC = ADef.CopyTo
                            BCC = ADef.BlindCopyTo
                            SMSTo = ADef.SMSTo
                            ScriptName = ADef.ScriptName
                            ScriptCommand = ADef.ScriptCommand
                            ScriptLocation = ADef.ScriptLocation

                            If SendTo = "" And CC = "" And BCC = "" And SMSTo = "" And ScriptName = "" Then
                                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - no new recipients to send an alert cleared notification", LogLevel.Verbose)
                            Else
                                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - found recipients to send an alert cleared notification", LogLevel.Verbose)
                                Try
                                    If SendTo <> "" And SendTo <> "SNMP Trap" And SendTo <> "Windows Log" And SMSTo = "" And ScriptName = "" Then
                                        WriteServiceHistoryEntry(Now.ToString & " Attempting to send a cleared alert e-mail:", LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   SendTo = " & SendTo & IIf(CC = "", "", ",    CopyTo = " + CC) & IIf(BCC = "", "", ",    BlindCopyTo = " + BCC), LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   ServerName = " & ServerName, LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   Details = " & Details, LogLevel.Normal)
                                        If (AlertType.Contains("Services")) Then
                                            Details = "The Original alert was Services -" & vbCrLf & Details
                                        Else
                                            Details = "The original alert was '" & AlertType & "' - " & vbCrLf & Details
                                        End If
                                        SendMailwithChilkatorNet(SendTo, CC, BCC, ServerName, ServerType, "", EventName, EventName, Details, "Cleared ", "Alert")
                                        mailsent = True
                                    ElseIf SendTo = "Windows Log" Then
                                        Dim sSource As String = "VitalSigns Plus"
                                        Dim sLog As String = "Application"
                                        Dim sEvent As String = "Alert condition '" & ADef.EventName & "' for " & ADef.ServerType & ", " & ADef.ServerName & " was cleared at " & Now.ToLongTimeString & ". " & vbCrLf & ADef.Details
                                        Try
                                            If Not EventLog.SourceExists(sSource) Then
                                                EventLog.CreateEventSource(sSource, sLog)
                                            End If

                                            EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234)
                                        Catch ex As Exception
                                            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error writing to Windows log for alert ID " & ADef.AlertKey & ": " & ex.Message, LogLevel.Normal)
                                        End Try
                                        SendTo = "Windows Log"
                                        mailsent = True
                                    End If
                                Catch ex As ApplicationException
                                    WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error occurred at the time of sending Cleared Alert Mail " & ex.Message, LogLevel.Normal)
                                    mailsent = False
                                End Try

                                Try
                                    If SMSTo <> "" Then
                                        WriteServiceHistoryEntry(Now.ToString & " Attempting to send a cleared alert SMS:", LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   SMSTo = " & SMSTo, LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   ServerName = " & ServerName, LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   Details = " & Details, LogLevel.Normal)
                                        Details = "The original alert was '" & AlertType & "' - " & vbCrLf & Details
                                        SendSMSwithTwilio(SMSTo, ServerName, ServerType, EventName, Details, "Cleared ", "")
                                        smssent = True
                                    End If
                                Catch ex As ApplicationException
                                    WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error occurred at the time of sending Cleared Alert Mail " & ex.Message, LogLevel.Normal)
                                    smssent = False
                                End Try

                                Try
                                    If ScriptName <> "" Then
                                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - attempting to send alert cleared notification via Script", LogLevel.Verbose)
                                        WriteServiceHistoryEntry(Now.ToString & " Attempting to send a cleared alert script:", LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   Script = " & ScriptName, LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   ServerName = " & ServerName, LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   Details = " & Details, LogLevel.Normal)
                                        Details = "The original alert was '" & AlertType & "' - " & vbCrLf & Details
                                        SendScript(ScriptName, ScriptCommand, ScriptLocation, ServerName, ServerType, EventName, Details, "Cleared ")
                                        scriptsent = True
                                    End If
                                Catch ex As ApplicationException
                                    WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error occurred at the time of sending Cleared Alert Mail " & ex.Message, LogLevel.Normal)
                                    scriptsent = False
                                End Try
                            End If
                            Try
                                If mailsent = True Then
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - trying to update sent mail info", LogLevel.Verbose)
                                    UpdatingSentMails(AlertID, SendTo, CC, BCC, SentID)
                                End If
                            Catch ex As Exception
                                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error while attempting to update Cleared Alert mail history for E-mail: " & ex.ToString(), LogLevel.Normal)
                            End Try
                            Try
                                If smssent = True Then
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - trying to update sent SMS info", LogLevel.Verbose)
                                    UpdatingSentMails(AlertID, SMSTo, "", "", SentID)
                                End If
                            Catch ex As Exception
                                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error while attempting to update Cleared Alert mail history for SMS: " & ex.ToString(), LogLevel.Normal)
                            End Try
                            Try
                                If scriptsent = True Then
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - trying to update sent Scipt info", LogLevel.Verbose)
                                    UpdatingSentMails(AlertID, ScriptName, "", "", SentID)
                                End If
                            Catch ex As Exception
                                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error while attempting to update Cleared Alert mail history for Script: " & ex.ToString(), LogLevel.Normal)
                            End Try
                        End If
                    Catch ex As Exception
                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: the cleared e-mail alert has already been sent for SentID=" & SentID & ", " & ex.ToString(), LogLevel.Normal)
                    End Try
                Next
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error in ProcessAlertsClearSendNotification: " & ex.Message, LogLevel.Normal)
        End Try
    End Sub
    Private Sub IsItTimeToSendAlert(ByRef ADefArrOut() As AlertDefinition)
        Dim ADefOut As AlertDefinition
        Dim isSpecific As Boolean = False
        Dim isDayIncluded As Boolean = False
        Dim startdt As DateTime
        Dim retVal As Integer = 0

        If Not ADefArrOut(0) Is Nothing Then
            For j As Integer = 0 To ADefArrOut.Length - 1
                ADefOut = New AlertDefinition
                ADefOut = ADefArrOut(j)
                startdt = DateTime.Parse(ADefOut.StartTime)
                If ADefOut.IntType <> 3 Then
                    isSpecific = IIf(Now.TimeOfDay >= startdt.TimeOfDay And Now <= startdt.AddMinutes(ADefOut.Duration), True, False)
                    isDayIncluded = IIf(ADefOut.Days.IndexOf(Now.DayOfWeek.ToString()) <> -1, True, False)
                    retVal = IIf(isSpecific And isDayIncluded, 1, 0)
                Else
                    retVal = 1
                End If
                ADefOut.RunNow = retVal
                ADefArrOut(j) = ADefOut
            Next
        End If
    End Sub
    Public Sub SendMailwithChilkatorNet(ByVal SendTo As String, ByVal CC As String, ByVal BCC As String, ByVal ServerName As String,
     ByVal ServerType As String, ByVal Location As String, ByVal EventName As String, ByVal AlertType As String,
     ByVal Details As String, ByVal iscleared As String, ByVal SubjectStr As String)
        Dim boolHSBC As Boolean = False
        If InStr(ServerName.ToUpper, "HSBC") > 0 Then
            boolHSBC = True
        End If

        Dim mailman As New Chilkat.MailMan
        Try
            mailman.UnlockComponent("MZLDADMAILQ_8nzv7Kxb4Rpx")
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error unlocking Chilkat component: " & ex.ToString, LogLevel.Normal)
        End Try


        'Primary Server Credentials
        Dim PHostName As String = ""
        Dim Pport As String = ""
        Dim PEmail As String = ""
        Dim Ppwd As String = ""
        Dim PSNMP As Boolean = False
        Dim PAuth As Boolean = False
        Dim PFrom As String = ""
        '4/15/2014 NS added
        Dim PSSL As Boolean = False
        '3/31/2014 NS added for VSPLUS-489
        'Secondary Server Credentials - credentials will be reused if Chilkat send fails
        Dim PHostName2 As String = ""
        Dim Pport2 As String = ""
        Dim PEmail2 As String = ""
        Dim Ppwd2 As String = ""
        Dim PSNMP2 As Boolean = False
        Dim PAuth2 As Boolean = False
        Dim PFrom2 As String = ""
        '4/15/2014 NS added
        Dim PSSL2 As Boolean = False

        '2/28/2014 NS added for VSPLUS-326
        'Flag variable to set to False if the primary server send fails
        Dim emailSent As Boolean = True
        Dim tempVal As String = ""

        Try
            PHostName = getSettings("PrimaryHostName")
            Pport = getSettings("PrimaryPort")
            PEmail = getSettings("PrimaryUserId")
            Ppwd = getSettings("Primarypwd")
            PFrom = getSettings("PrimaryFrom")
            tempVal = getSettings("PrimaryAuth").ToString()
            If tempVal <> "" Then
                PAuth = Convert.ToBoolean(tempVal)
            End If
            '4/15/2014 NS added
            tempVal = getSettings("PrimarySSL").ToString()
            If tempVal <> "" Then
                PSSL = Convert.ToBoolean(tempVal)
            End If
            If PFrom.ToString = "" Then
                PFrom = "VS Plus"
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error getting primary settings from the Settings table:  " & ex.ToString, LogLevel.Normal)
        End Try
        '9/25/2014 NS commented out
        'WriteServiceHistoryEntry(Now.ToString & " pfrom:  " & PFrom)
        emailSent = SendMail(mailman, PHostName, Pport, PAuth, boolHSBC, Ppwd, PEmail, Details, iscleared, ServerType, ServerName,
        AlertType, SendTo, CC, BCC, PFrom, SubjectStr)

        'If an error occurred while sending email, try secondary server
        If Not emailSent Then
            WriteServiceHistoryEntry(Now.ToString & " Primary server has failed. Trying the secondary server now...", LogLevel.Normal)
            Try
                '3/31/2014 NS modified for VSPLUS-489
                PHostName2 = getSettings("SecondaryHostName")
                Pport2 = getSettings("SecondaryPort")
                PEmail2 = getSettings("SecondaryUserId")
                Ppwd2 = getSettings("SecondaryPwd")
                PFrom2 = getSettings("SecondaryFrom")
                tempVal = getSettings("SecondaryAuth").ToString()
                If tempVal <> "" Then
                    PAuth2 = Convert.ToBoolean(tempVal)
                End If
                If PFrom2.ToString = "" Then
                    PFrom2 = "VS Plus"
                End If
                '4/15/2014 NS added
                tempVal = getSettings("SecondarySSL").ToString()
                If tempVal <> "" Then
                    PSSL2 = Convert.ToBoolean(tempVal)
                End If
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " Error getting secondary settings from the Settings table:  " & ex.ToString, LogLevel.Normal)
            End Try

            '5/7/2015 NS modified for VSPLUS-1553
            If PHostName2 = "" Then
                WriteServiceHistoryEntry(Now.ToString & " The secondary server is not defined. The alert will not be sent.", LogLevel.Normal)
            Else
                '3/31/2014 NS modified for VSPLUS-489
                emailSent = SendMail(mailman, PHostName2, Pport2, PAuth2, boolHSBC, Ppwd2, PEmail2, Details, iscleared, ServerType, ServerName,
                   AlertType, SendTo, CC, BCC, PFrom2, SubjectStr)
                If Not emailSent Then
                    WriteServiceHistoryEntry(Now.ToString & " Secondary server has failed as well.", LogLevel.Normal)
                End If
            End If
        End If
        '3/31/2014 NS added for VSPLUS-489
        'If Chilkat fails, try sending via .Net
        If Not emailSent Then
            WriteServiceHistoryEntry(Now.ToString & " Trying to Send via .Net SMTP using Primary Server.", LogLevel.Normal)
            emailSent = SendMailNet(PHostName, Pport, PAuth, boolHSBC, Ppwd, PEmail, PSSL, Details, iscleared, ServerType, ServerName,
              AlertType, SendTo, CC, BCC, PFrom, False)
            If Not emailSent Then
                WriteServiceHistoryEntry(Now.ToString & " Primary server has failed via .Net SMTP mail send. Trying the secondary server now...", LogLevel.Normal)
                emailSent = SendMailNet(PHostName2, Pport2, PAuth2, boolHSBC, Ppwd2, PEmail2, PSSL2, Details, iscleared, ServerType, ServerName,
                 AlertType, SendTo, CC, BCC, PFrom2, False)
                If Not emailSent Then
                    WriteServiceHistoryEntry(Now.ToString & " Secondary server has failed via .Net SMTP mail send as well. No more send attempts.", LogLevel.Normal)
                End If
            End If
        End If
        'WriteServiceHistoryEntry(Now.ToString & " Reached the end of the Sendmail with Chilkat function.")
    End Sub
    '12/1/2014 NS added for VSPLUS-946
    Public Sub SendSMSwithTwilio(ByVal SMSTo As String, ByVal ServerName As String,
     ByVal ServerType As String, ByVal AlertType As String,
     ByVal Details As String, ByVal iscleared As String, ByVal PFrom As String)
        Dim SMSAccountSid As String
        Dim SMSAuthToken As String
        Dim SMSFrom As String
        Try
            SMSAccountSid = getSettings("SMSAccountSid")
            SMSAuthToken = getSettings("SMSAuthToken")
            SMSFrom = getSettings("SMSFrom")
            SendSMS(SMSAccountSid, SMSAuthToken, iscleared, ServerType, ServerName, AlertType, Details, SMSTo, SMSFrom)
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error getting SMS account settings from the Settings table:  " & ex.ToString, LogLevel.Normal)
        End Try
    End Sub
    '2/28/2014 NS added for VSPLUS-326
    Public Function SendMail(ByVal mailMan As Chilkat.MailMan, ByVal PHostName As String, ByVal Pport As String, ByVal PAuth As Boolean,
     ByVal boolHSBC As Boolean, ByVal Ppwd As String, ByVal PEmail As String,
     ByVal Details As String, ByVal iscleared As String, ByVal ServerType As String, ByVal ServerName As String,
     ByVal AlertType As String, ByVal SendTo As String, ByVal CC As String, ByVal BCC As String, ByVal PFrom As String,
     ByVal SubjectStr As String) As Boolean
        Dim success As Boolean = True
        '3/2/2017 NS added for VSPLUS-3520
        'Password decryption
        Dim strEncryptedPassword As String
        Dim Password As String
        Dim myPass As Byte()
        Dim str1() As String
        Dim mySecrets As New VSFramework.TripleDES

        Try
            strEncryptedPassword = Ppwd
            str1 = strEncryptedPassword.Split(",")
            Dim bstr1(str1.Length - 1) As Byte
            For j As Integer = 0 To str1.Length - 1
                bstr1(j) = str1(j).ToString()
            Next
            myPass = bstr1
            If Not strEncryptedPassword Is Nothing Then
                Password = mySecrets.Decrypt(myPass) 'password in clear text, stored in memory now
            Else
                Password = Nothing
            End If
        Catch ex As Exception
            Password = ""
            WriteServiceHistoryEntry(Now.ToString & " Error decrypting the password in SendMail: " & ex.Message, LogLevel.Normal)
        End Try

        With mailMan
            .SmtpHost = PHostName
            .SmtpPort = Pport
            If PAuth = True And boolHSBC = False Then
                .SmtpPassword = Password
                .SmtpUsername = PEmail
            End If
        End With

        '1/12/2016 NS commented out - the combination of port 587 and SSL true is not always the case
        'Try
        '    If InStr(PHostName.ToUpper, "GMAIL") Then
        '        If mailMan.SmtpPort <> 587 Then
        '            WriteServiceHistoryEntry(Now.ToString & " GMail uses port 587 so I am setting it to use that port.", LogLevel.Normal)
        '            mailMan.SmtpPort = 587
        '            mailMan.SmtpSsl = True
        '        End If
        '    End If
        'Catch ex As Exception
        '    WriteServiceHistoryEntry(Now.ToString & " Error setting port in SendMail.", LogLevel.Normal)
        'End Try

        Try
            Dim email As New Chilkat.Email
            '4/21/2016 NS modified for VSPLUS-2755
            Dim localZone = TimeZone.CurrentTimeZone
            email.Body = Details & vbCrLf & vbCrLf & "Message sent: " & Now.ToLongDateString & " " & Now.ToShortTimeString & " " & localZone.StandardName
            '4/8/2015 NS modified for VSPLUS-219
            email.Subject = iscleared & SubjectStr & " for " & ServerType & " " & ServerName & " - " & AlertType
            email.AddTo(SendTo, SendTo)
            '1/30/2014 NS added for VSPLUS-315
            email.AddCC(CC, CC)
            email.AddBcc(BCC, BCC)
            email.FromAddress = PEmail
            email.ReplyTo = PEmail
            email.FromName = PFrom
            '9/25/2014 NS commented out
            'WriteServiceHistoryEntry(Now.ToString & " email.FromName:  " & email.FromName)
            ' Send mail.
            success = mailMan.SendEmail(email)
            If success Then
                WriteServiceHistoryEntry(Now.ToString & " Sent SMTP mail to " & SendTo & IIf(CC = "", "", ",    CopyTo = " + CC) & IIf(BCC = "", "", ",    BlindCopyTo = " + BCC) & " re: " & ServerName & ", " & Details, LogLevel.Normal)
                WriteServiceHistoryEntry(Now.ToString & " Email subject was " & email.Subject & vbCrLf, LogLevel.Normal)
            Else
                WriteServiceHistoryEntry(Now.ToString & " Error sending mail in SendMail: " & mailMan.LastErrorText, LogLevel.Normal)
            End If
            email.Dispose()

            'success = Nothing

        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error sending mail in SendMail: " & ex.Message, LogLevel.Normal)

        End Try

        Try
            mailMan.Dispose()

            GC.Collect()
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error while disposing of the mail object in SendMail.", LogLevel.Normal)
        End Try
        Return success
    End Function
    '12/1/2014 NS added for VSPLUS-946
    Public Function SendSMS(ByVal SMSAccountSid As String, ByVal SMSAuthToken As String, ByVal iscleared As String, ByVal ServerType As String,
       ByVal ServerName As String, ByVal AlertType As String, ByVal Details As String, ByVal SMSTo As String, ByVal PFrom As String) As Boolean
        Dim success As Boolean = True
        Dim twilio As TwilioRestClient
        Dim sms As Twilio.SMSMessage
        Dim body As String
        '5/14/2015 NS modified for VSPLUS-1717
        body = iscleared & "Alert for " & ServerType & " " & ServerName & " - " & AlertType & vbCrLf & Details '& vbCrLf & "Message sent: " & Now.ToLongDateString & " " & Now.ToShortTimeString
        '12/12/2014 NS added - IMPORTANT - the max message size with Twilio is 160 characters. Attempting to send a larger SMS fails 
        'without producing an exception so there is no way of knowing the service has failed. Keep the message size to under 160!
        If body.Length > 160 Then
            body = Left(body, 159)
        End If
        Try
            If PFrom.Length = 10 Then
                PFrom = "+1" & PFrom
            End If
            WriteServiceHistoryEntry(Now.ToString & " Attempting to send SMS with the following parameters: from - " & PFrom & ", to - " & SMSTo & ", body - " & body, LogLevel.Normal)
            twilio = New TwilioRestClient(SMSAccountSid, SMSAuthToken)
            sms = twilio.SendSmsMessage(PFrom, SMSTo, body)
            If (Not sms.RestException Is vbNullString) Then
                WriteServiceHistoryEntry(Now.ToString & " REST exception... " & sms.RestException.Message)
            End If
            WriteServiceHistoryEntry(Now.ToString & " Received Sid " & sms.Sid & ", message status is " & sms.Status)
        Catch ex As Exception
            success = False
            WriteServiceHistoryEntry(Now.ToString & " Error while sending an SMS in SendSMS: " & ex.ToString, LogLevel.Normal)
        End Try
        Return success
    End Function
    '12/9/2014 NS added for VSPLUS-1229
    Public Function SendScript(ByVal ScriptName As String, ByVal ScriptCommand As String, ByVal ScriptLocation As String,
       ByVal ServerName As String, ByVal ServerType As String, ByVal AlertType As String,
       ByVal Details As String, ByVal iscleared As String)
        Dim success As Boolean = True
        Dim ServerNameParam As String = "%Name%"
        Dim ServerTypeParam As String = "%Type%"
        Dim EventTypeParam As String = "%EventType%"
        Dim AlertDetailsParam As String = "%Details%"
        Dim DTDParam As String = "%DTD%"

        Try
            WriteServiceHistoryEntry(Now.ToString & " Attempting to parametrize script " & ScriptCommand, LogLevel.Normal)
            ScriptCommand = ScriptCommand.Substring(ScriptCommand.IndexOf(" ") + 1)
            ScriptCommand = ScriptCommand.Replace(ServerNameParam, ServerName)
            ScriptCommand = ScriptCommand.Replace(ServerTypeParam, ServerType)
            ScriptCommand = ScriptCommand.Replace(EventTypeParam, AlertType)
            ScriptCommand = ScriptCommand.Replace(AlertDetailsParam, Details)
            ScriptCommand = ScriptCommand.Replace(DTDParam, Now.ToShortDateString & " " & Now.ToShortTimeString)
            WriteServiceHistoryEntry(Now.ToString & " Attempting to execute script " & ScriptLocation & " " & ScriptCommand, LogLevel.Normal)
            Dim id As Integer = Shell(ScriptLocation & " " & ScriptCommand)
        Catch ex As Exception
            success = False
            WriteServiceHistoryEntry(Now.ToString & " In SendScript: " & ex.Message, LogLevel.Normal)
        End Try
        If success Then
            WriteServiceHistoryEntry(Now.ToString & " Sent script " & ScriptLocation & " " & ScriptCommand, LogLevel.Normal)
        End If
        Return success
    End Function
    Public Function getSettings(ByVal sname As String) As String
        Dim registry As New VSFramework.RegistryHandler
        Try
            Return registry.ReadFromRegistry(sname).ToString()
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Private Sub InsertingSentMails(ByVal AlertID As String, ByVal SentTo As String, ByVal CcdTo As String, ByVal BccdTo As String,
                                   ByVal resent As Boolean, ByVal AlertKey As String)
        Dim connString As String = GetDBConnection()
        Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
        Dim filterEventsDetected As FilterDefinition(Of EventsDetected)
        Dim eventsCreated() As EventsDetected
        Dim notificationsSent As List(Of NotificationsSent)

        Try
            Dim strdt As String
            strdt = Date.Now
            Dim oid As String
            oid = AlertKey
            If resent Then
                filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True),
                                                                     repoEventsDetected.Filter.Eq(Of String)(Function(j) j.Id, AlertID))
                eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToArray()
                If eventsCreated.Length > 0 Then
                    Dim notificationentity As New NotificationsSent With {.NotificationId = oid, .NotificationSentTo = SentTo, .NotificationCcdTo = CcdTo, .NotificationBccdTo = BccdTo, .EventDetectedSent = strdt}
                    eventsCreated(0).NotificationsSent.Add(notificationentity)
                    repoEventsDetected.Replace(eventsCreated(0))
                End If
            Else
                filterEventsDetected = repoEventsDetected.Filter.Eq(Of String)(Function(j) j.Id, AlertID)
                eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToArray()
                If eventsCreated.Length > 0 Then
                    Dim notificationentity As New NotificationsSent With {.NotificationId = oid, .NotificationSentTo = SentTo, .NotificationCcdTo = CcdTo, .NotificationBccdTo = BccdTo, .EventDetectedSent = strdt}
                    If eventsCreated(0).NotificationsSent Is Nothing Then
                        notificationsSent = New List(Of NotificationsSent)
                        notificationsSent.Add(notificationentity)
                        eventsCreated(0).NotificationsSent = notificationsSent
                    Else
                        eventsCreated(0).NotificationsSent.Add(notificationentity)
                    End If
                    repoEventsDetected.Replace(eventsCreated(0))
                End If
            End If
        Catch ex As ApplicationException
            WriteServiceHistoryEntry(Now.ToString & " Error occurred at the time of updating an events_detected document: " & AlertID & " , " & SentTo & "," & CcdTo & "," & BccdTo & ": " & ex.Message, LogLevel.Normal)
        End Try
    End Sub
    Private Sub UpdatingSentMails(ByVal AlertID As String, ByVal SentTo As String, ByVal CcdTo As String, ByVal BccdTo As String,
                                  ByVal id As String)
        Dim connString As String = GetDBConnection()
        Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
        Dim filterEventsDetected As FilterDefinition(Of EventsDetected)
        Dim eventsCreated() As EventsDetected
        Dim notificationsArr() As NotificationsSent

        Try
            Dim strdt As String
            strdt = Date.Now
            filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True),
                                                                 repoEventsDetected.Filter.Eq(Of String)(Function(j) j.Id, AlertID))
            eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToArray()
            If eventsCreated.Length > 0 Then
                notificationsArr = eventsCreated(0).NotificationsSent.ToArray()
                For i As Integer = 0 To notificationsArr.Length - 1
                    WriteServiceHistoryEntry(Now.ToString & " AlertID: " & AlertID & ", SentTo: " & SentTo & ", CcdTo: " & CcdTo & ", BccdTo: " & BccdTo & ", id: " & id, LogLevel.Verbose)
                    WriteServiceHistoryEntry(Now.ToString & " NotificationSentTo: " & notificationsArr(i).NotificationSentTo & ", NotificationCcdTo: " & notificationsArr(i).NotificationCcdTo & ", NotificationBccdTo: " & notificationsArr(i).NotificationBccdTo, LogLevel.Verbose)
                    notificationsArr(i).EventDismissedSent = strdt
                    eventsCreated(0).NotificationsSent(i) = notificationsArr(i)
                Next
                repoEventsDetected.Replace(eventsCreated(0))
                WriteServiceHistoryEntry(Now.ToString & " Updated the events_detected collection, the notifications_sent.event_dismissed_sent value for " & id, LogLevel.Normal)
            End If
        Catch ex As ApplicationException
            WriteServiceHistoryEntry(Now.ToString & " Error occurred at the time of updating a document with id: " & id & " , sentto: " & SentTo & "," & CcdTo & "," & BccdTo & " in the events_detected collection: " & ex.Message, LogLevel.Normal)
        End Try

    End Sub
    Private Sub InsertSentEscalation(ByRef eventCreated As EventsDetected, ByVal oid As String, ByVal eid As String, ByVal EscalateTo As String)
        Dim connString As String = GetDBConnection()
        Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
        Dim notificationsSent As List(Of NotificationsSent)
        Dim strdt As String

        Try
            strdt = Date.Now
            notificationsSent = eventCreated.NotificationsSent
            If notificationsSent.Count > 0 Then
                Dim notificationentity As New NotificationsSent With {.NotificationId = oid, .EscalationId = eid, .NotificationSentTo = EscalateTo, .EventDetectedSent = strdt}
                notificationsSent.Add(notificationentity)
                eventCreated.NotificationsSent = notificationsSent
                repoEventsDetected.Replace(eventCreated)
            End If
        Catch ex As ApplicationException
            WriteServiceHistoryEntry(Now.ToString & " Error occurred at the time of inserting value " & oid.ToString() & " , " & EscalateTo & " into  the notifications_sent embedded document in events_detected " & ex.Message, LogLevel.Normal)
        End Try
    End Sub
    Private Sub SendSNMPTrap(ByVal remoteHost As String, ByVal DeviceType As String, ByVal DeviceName As String, ByVal AlertType As String, ByVal Details As String)
        Dim host As String = "localhost"
        Dim community As String = "public"
        Dim agent As TrapAgent = New TrapAgent()

        Dim col As VbCollection = New VbCollection()

        Try

            '***********************  SNMP MAIL  ****************************

            WriteServiceHistoryEntry(Now.ToString & " Sending SNMP Trap to " & remoteHost & " for " & DeviceName & " to " & AlertType, LogLevel.Normal)

            Dim trapOID As String
            trapOID = "1.3.6.1.4.1.26062.0.1"
            Dim timeTickVal As UInt32 = 2324

            col.Add(New Oid("1.3.6.1.2.1.1.3.0"), New TimeTicks(timeTickVal))
            col.Add(New Oid("1.3.6.1.6.3.1.1.4.1.0"), New Oid(trapOID))
            col.Add(New Oid("1.3.6.1.4.1.26062.0.1.1.0"), New OctetString(Details))
            col.Add(New Oid("1.3.6.1.4.1.26062.0.1.2.0"), New OctetString(AlertType))
            col.Add(New Oid("1.3.6.1.4.1.26062.0.1.3.0"), New OctetString(DeviceType))
            col.Add(New Oid("1.3.6.1.4.1.26062.0.1.4.0"), New OctetString(DeviceName))
            col.Add(New Oid("1.3.6.1.4.1.26062.0.1.5.0"), New OctetString(Now.ToShortDateString & " at " & Now.ToShortTimeString))

            Try
                agent.SendV1Trap(New SnmpSharpNet.IpAddress(remoteHost), 162, "public",
                    New Oid(trapOID), New SnmpSharpNet.IpAddress("127.0.0.1"),
                    SnmpConstants.LinkUp, 0, 13432, col)
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " Error sending SNMP Trap " & ex.ToString, LogLevel.Normal)
            End Try

        Catch ex As ApplicationException
            WriteServiceHistoryEntry(Now.ToString & " In SendSNMPTrap: Error - " & ex.Message, LogLevel.Normal)
        End Try

    End Sub
    Private Function GetMaxAlertsRemainingToday(Optional ByVal AlertKey As String = "")
        Dim totalRemaining As Integer = 0
        Dim connString As String = GetDBConnection()
        Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
        Dim filterEventsDetected As FilterDefinition(Of EventsDetected)
        Dim eventsCreated() As EventsDetected
        Dim repoSettings As New Repository(Of NameValue)(connString)
        Dim filterSettings As FilterDefinition(Of NameValue)
        Dim settings() As NameValue
        Dim notifications() As NotificationsSent
        Dim totalCount As Integer = 0
        Dim totalMax As Integer = 0

        Try
            If AlertKey = "" Then
                filterEventsDetected = repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True)
                filterSettings = repoSettings.Filter.Eq(Function(j) j.Name, "TotalMaximumAlertsPerDay")
            Else
                filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True),
                                                                     repoEventsDetected.Filter.Eq(Function(j) j.Id, AlertKey))
                filterSettings = repoSettings.Filter.Eq(Function(j) j.Name, "TotalMaximumAlertsPerDefinition")
            End If
            eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToArray()
            settings = repoSettings.Find(filterSettings).ToArray()
            If eventsCreated.Length > 0 And settings.Length > 0 Then
                totalMax = Convert.ToInt32(settings(0).Value.ToString())
                For i As Integer = 0 To eventsCreated.Length - 1
                    notifications = eventsCreated(i).NotificationsSent.Where(Function(j) j.EventDetectedSent >= Today).ToArray()
                    totalCount += notifications.Length
                    notifications = eventsCreated(i).NotificationsSent.Where(Function(j) j.EventDismissedSent IsNot Nothing).ToArray()
                    If notifications.Length > 0 Then
                        notifications = notifications.Where(Function(j) j.EventDismissedSent >= Today).ToArray()
                        totalCount += notifications.Length
                    End If
                Next

            End If
            If totalMax > 0 Then
                If totalCount > totalMax Then
                    totalRemaining = 0
                Else
                    totalRemaining = (totalMax - totalCount)
                End If
            Else
                totalRemaining = -1 ' there is no limit 
            End If
        Catch ex As Exception
            totalRemaining = -1
            WriteServiceHistoryEntry(Now.ToString & " In GetMaxAlertsRemainingToday: Error while calculating max alerts per day " & ex.ToString, LogLevel.Normal)
        End Try
        Return totalRemaining
    End Function

    Private Function SendMailNet(ByVal PHostName As String, ByVal Pport As String, ByVal PAuth As Boolean,
  ByVal boolHSBC As Boolean, ByVal Ppwd As String, ByVal PEmail As String, ByVal PSSL As Boolean,
  ByVal Details As String, ByVal iscleared As String, ByVal ServerType As String, ByVal ServerName As String,
  ByVal AlertType As String, ByVal SendTo As String, ByVal CC As String, ByVal BCC As String, ByVal PFrom As String,
  ByVal isEmergency As Boolean) As Boolean
        Return SendMailNet(PHostName, Pport, PAuth, boolHSBC, Ppwd, PEmail, PSSL, Details, iscleared, ServerType, ServerName, AlertType, SendTo, CC, BCC, PFrom, isEmergency, "")
    End Function
    Public Function SendMailNet(ByVal PHostName As String, ByVal Pport As String, ByVal PAuth As Boolean,
     ByVal boolHSBC As Boolean, ByVal Ppwd As String, ByVal PEmail As String, ByVal PSSL As Boolean,
     ByVal Details As String, ByVal iscleared As String, ByVal ServerType As String, ByVal ServerName As String,
     ByVal AlertType As String, ByVal SendTo As String, ByVal CC As String, ByVal BCC As String, ByVal PFrom As String,
     ByVal isEmergency As Boolean, sSubject As String) As Boolean
        Dim success As Boolean = True
        Dim subject As String

        '7/20/2015 NS modified for VSPLUS-1562
        If isEmergency Then
            subject = "VitalSigns EMERGENCY Notification - SQL Server may be down."
        Else
            subject = iscleared & "Alert for " & ServerType & " " & ServerName & " - " & AlertType
        End If
        If sSubject <> "" Then
            subject = sSubject
        End If
        Try
            Dim Smtp_Server As New SmtpClient
            Dim e_mail As New MailMessage()
            Smtp_Server.UseDefaultCredentials = False
            If PAuth = True And boolHSBC = False Then
                Smtp_Server.Credentials = New Net.NetworkCredential(PEmail, Ppwd)
            End If
            'WriteServiceHistoryEntry(Now.ToString & " Host detail. Host name =  " & PHostName & " PSSL = " & PSSL & " Pport = " & Pport & " Ppwd = " & Ppwd & vbCrLf)
            Smtp_Server.Port = Pport
            '4/15/2014 NS modified
            Smtp_Server.EnableSsl = PSSL
            Smtp_Server.Host = PHostName
            'WriteServiceHistoryEntry(Now.ToString & " Email Address = " & PEmail & " From = " & PFrom & vbCrLf)
            e_mail = New MailMessage()
            '4/15/2014 NS modified - use email and from, email must be a valid email address, from may be a name
            'e_mail.From = New MailAddress(PFrom)
            e_mail.From = New MailAddress(PEmail, PFrom)

            WriteServiceHistoryEntry(Now.ToString & " Email Address = " & PEmail & " From = " & e_mail.From.DisplayName & " SendTo = " & SendTo & " subject = " & subject & vbCrLf, LogLevel.Normal)
            If SendTo IsNot Nothing And SendTo.Trim <> "" Then
                '7/20/2015 NS modified for VSPLUS-1562
                If SendTo.IndexOf(",") > 0 Then
                    Dim result As String() = SendTo.Split(New String() {","}, StringSplitOptions.None)
                    If result Is Nothing Then
                        e_mail.To.Add(New MailAddress(SendTo))
                    Else
                        For Each s As String In result
                            WriteServiceHistoryEntry(Now.ToString & " SendTo = " & s & vbCrLf, LogLevel.Verbose)
                            e_mail.To.Add(New MailAddress(s))
                        Next
                    End If
                Else
                    e_mail.To.Add(New MailAddress(SendTo))
                End If
            End If
            If CC IsNot Nothing And CC.Trim <> "" Then
                e_mail.CC.Add(New MailAddress(CC))
            End If
            If BCC IsNot Nothing And BCC.Trim <> "" Then
                e_mail.Bcc.Add(New MailAddress(BCC))
            End If
            e_mail.Subject = subject
            e_mail.IsBodyHtml = False
            '4/21/2016 NS modified for VSPLUS-2755
            Dim localZone = TimeZone.CurrentTimeZone
            e_mail.Body = Details & vbCrLf & "Message sent: " & Now.ToLongDateString & " " & Now.ToShortTimeString & " " & localZone.StandardName _
                & vbCrLf & vbCrLf & "NOTE: Message sent time zone value reflects the Vital Signs server installation setting."
            Smtp_Server.Send(e_mail)
            success = True
            'WriteServiceHistoryEntry(Now.ToString & " Sent SMTP mail to " & SendTo & " re: " & ServerName & ", " & Details)
            'WriteServiceHistoryEntry(Now.ToString & " Email subject was " & subject & vbCrLf)
        Catch ex As Exception
            success = False
            WriteServiceHistoryEntry(Now.ToString & " Error sending mail in SendMailNet: " & ex.ToString, LogLevel.Normal)
        End Try

        Return success
    End Function
    Public Sub GetEmergencyAlertInfo()
        '7/17/2015 NS added for VSPLUS-1562
        Dim dt As DataTable
        Dim emergencyContacts As String = ""
        Dim PHostName As String = ""
        Dim Pport As String = ""
        Dim PEmail As String = ""
        Dim Ppwd As String = ""
        Dim PFrom As String = ""
        Dim PAuth As String = ""
        Dim PSSL As String = ""
        Dim connString As String = GetDBConnection()
        Dim repoSettings As New Repository(Of NameValue)(connString)
        Dim filterSettings As FilterDefinition(Of NameValue)
        Dim settings() As NameValue
        Dim tempVal As String = ""

        Try
            filterSettings = repoSettings.Filter.Eq(Function(j) j.Name, "EmergencyAlertEmail")
            settings = repoSettings.Find(filterSettings).ToArray()
            If settings.Length > 0 Then
                Try
                    PHostName = getSettings("PrimaryHostName")
                    Pport = getSettings("primaryport")
                    PEmail = getSettings("primaryUserID")
                    Ppwd = getSettings("primarypwd")
                    PFrom = getSettings("primaryFrom")
                    tempVal = getSettings("primaryAuth").ToString()
                    If tempVal <> "" Then
                        PAuth = Convert.ToBoolean(tempVal)
                    End If
                    tempVal = getSettings("primarySSL").ToString()
                    If tempVal <> "" Then
                        PSSL = Convert.ToBoolean(tempVal)
                    End If
                    If PFrom.ToString = "" Then
                        PFrom = "VS Plus"
                    End If
                    For i As Integer = 0 To settings.Length - 1
                        emergencyContacts += settings(i).Value + ","
                    Next
                    '11/17/2015 NS modified for VSPLUS-1562
                    myRegistry.WriteToRegistry("Alert Emergency Contacts", emergencyContacts)
                    myRegistry.WriteToRegistry("Alert Emergency PrimaryHostName", PHostName)
                    myRegistry.WriteToRegistry("Alert Emergency primaryport", Pport)
                    myRegistry.WriteToRegistry("Alert Emergency primaryUserID", PEmail)
                    myRegistry.WriteToRegistry("Alert Emergency primarypwd", Ppwd)
                    myRegistry.WriteToRegistry("Alert Emergency primaryFrom", PFrom)
                    myRegistry.WriteToRegistry("Alert Emergency primaryAuth", PAuth)
                    myRegistry.WriteToRegistry("Alert Emergency primarySSL", PSSL)
                Catch ex As Exception
                    WriteServiceHistoryEntry(Now.ToString & " Error getting primary settings from the Settings table:  " & ex.ToString, LogLevel.Normal)
                End Try
            Else
                '11/17/2015 NS modified for VSPLUS-1562
                WriteServiceHistoryEntry(Now.ToString & " GetEmergencyAlertInfo - no records found in AlertEmergencyContacts", LogLevel.Verbose)
                myRegistry.WriteToRegistry("Alert Emergency Contacts", "")
                myRegistry.WriteToRegistry("Alert Emergency PrimaryHostName", "")
                myRegistry.WriteToRegistry("Alert Emergency primaryport", "")
                myRegistry.WriteToRegistry("Alert Emergency primaryUserID", "")
                myRegistry.WriteToRegistry("Alert Emergency primarypwd", "")
                myRegistry.WriteToRegistry("Alert Emergency primaryFrom", "")
                myRegistry.WriteToRegistry("Alert Emergency primaryAuth", "")
                myRegistry.WriteToRegistry("Alert Emergency primarySSL", "")
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error writing emergency contact info into registry: " & ex.ToString, LogLevel.Normal)
        End Try
    End Sub
End Class
Public Class RegistryHandler

    Sub WriteToRegistry(ByVal KeyName As String, ByVal KeyValue As Object)

        Dim aKey As RegistryKey

        aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\VitalSigns", True)

        If aKey Is Nothing Then
            aKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("Software\VitalSigns")
        End If

        aKey.SetValue(KeyName, KeyValue)
        aKey.Flush()

    End Sub

    Function ReadFromRegistry(ByVal KeyName As String) As Object

        Dim aKey As RegistryKey
        Try
            aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\VitalSigns")

            If aKey Is Nothing Then
                aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\VitalSigns")
            End If

            If aKey Is Nothing Then
                Return Nothing
                Exit Function
            End If
        Catch ex As Exception
            Return Nothing
            Exit Function
        End Try

        Try
            Return aKey.GetValue(KeyName)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class
