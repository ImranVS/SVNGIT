Imports System.Threading
Imports System.Net
Imports System.Net.Mail
Imports System.IO
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
Imports System
Imports System.ComponentModel
Imports System.Reflection

Public Class Servers
    Public Property DeviceId As String
    Public Property DeviceName As String
    Public Property DeviceType As String
End Class

Public Class Destinations
    Public Property DestinationId As String
    Public Property SendVia As String
    Public Property PersistantNotification As Boolean = False
    Public Property Interval As Integer? = Nothing

    'Business Hours
    Public Property StartTimeString As String
    Public Property Duration As Int16
    Public Property Days As List(Of String)

    'Email
    Public Property SendTo As String
    Public Property CopyTo As String
    Public Property BlindCopyTo As String

    'Scripts
    Public Property ScriptName As String
    Public Property ScriptLocation As String
    Public Property ScriptCommand As String

    'SMS
    Public Property PhoneNumber As String

    'SNMP Trap

    'URL
    Public Property URL As String

    'Enum
    Public Enum SendType
        <Description("E-mail")>
        Email
        <Description("SMS")>
        SMS
        <Description("Script")>
        Script
        <Description("URL")>
        URL
    End Enum


    Public Shared Function GetDescription(ByVal sendType As SendType) As String
        Dim fi As FieldInfo = sendType.GetType().GetField(sendType.ToString())
        If fi IsNot Nothing Then
            Dim attrs() As Object = fi.GetCustomAttributes(GetType(DescriptionAttribute), True)
            If attrs IsNot Nothing AndAlso attrs.Length > 0 Then
                Return CType(attrs(0), DescriptionAttribute).Description
            End If
        End If
        Return ""
    End Function

    Public Shared Function GetEnumFromDescription(ByVal description As String) As SendType
        For Each fi As FieldInfo In GetType(SendType).GetFields
            Dim attrs() As Object = fi.GetCustomAttributes(GetType(DescriptionAttribute), True)
            If attrs IsNot Nothing AndAlso attrs.Length > 0 Then
                For Each attr As DescriptionAttribute In attrs
                    If attr.Description.Equals(description) Then
                        Return CType(fi.GetValue(Nothing), SendType)
                    End If
                Next
                Return CType(attrs(0), DescriptionAttribute).Description
            End If
        Next
        Return Nothing
    End Function

End Class

Public Class Events
    Public Property EventType As String
    Public Property DeviceType As String
    Public Property NotificationOnRepeat As Boolean = False
End Class

Public Class AlertDefinition
    Implements IComparer
    Public Sub New(ByVal akey As String)
        '_AlertKey = akey
    End Sub
    Public Sub New()

    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As _
        Object) As Integer Implements _
        System.Collections.IComparer.Compare
        Dim ax As AlertDefinition = DirectCast(x, AlertDefinition)
        Dim ay As AlertDefinition = DirectCast(y, AlertDefinition)
        Dim intx As String
        Dim inty As String

        'intx = ax.AlertKey
        'inty = ay.AlertKey

        Return intx.CompareTo(inty) 'The old version of the code when working with int alert keys used intx > inty comparison. When working with strings, need to use CompareTo
    End Function

    Public Property NotificationId As String
    Public Property NotificationName As String
    Public Property Servers As New List(Of Servers)
    Public Property Events As New List(Of Events)
    Public Property Destinations As New List(Of Destinations)
    Public Property currentSendCount


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

                    WriteServiceHistoryEntry(Now.ToString & " Before calling ProcessAlertsSendNotification()", LogLevel.Verbose)
                    ProcessAlertsSendNotification()
                Catch ex As Exception
                    WriteServiceHistoryEntry(Now.ToString & " Error in ServiceWorkerThreadNew: " & ex.Message, LogLevel.Normal)

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
        Dim counter As Int32 = 0
        Dim connString As String = GetDBConnection()
        Dim repoEventsMaster As New Repository(Of EventsMaster)(connString)
        Dim repoNotifications As New Repository(Of Notifications)(connString)
        Dim repoNotificationDest As New Repository(Of NotificationDestinations)(connString)
        Dim repoServers As New Repository(Of Server)(connString)
        Dim repoServersOther As New Repository(Of ServerOther)(connString)
        Dim repoBusHrs As New Repository(Of BusinessHours)(connString)
        Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
        Dim repoScripts As New Repository(Of Scripts)(connString)
        Dim repoAlertURLs As New Repository(Of Alert_URLs)(connString)

        Dim filterNotifications As FilterDefinition(Of Notifications)
        Dim filterNotificationDest As FilterDefinition(Of NotificationDestinations)
        Dim filterEvents As FilterDefinition(Of EventsMaster)
        Dim filterServers As FilterDefinition(Of Server)
        Dim filterServersOther As FilterDefinition(Of ServerOther)
        Dim filterBusHrs As FilterDefinition(Of BusinessHours)
        Dim filterEventsDetected As FilterDefinition(Of EventsDetected)
        Dim filterScripts As FilterDefinition(Of Scripts)
        Dim filterURLs As FilterDefinition(Of Alert_URLs)

        Dim eventsList As List(Of EventsMaster)
        Dim serversList As List(Of Server)
        Dim bushrsList As List(Of BusinessHours)
        Dim notificationsList As List(Of Notifications)
        Dim notificationDestList As List(Of NotificationDestinations)
        Dim scriptsList As List(Of Scripts)
        Dim URLsList As List(Of Alert_URLs)

        Dim alertDefinitions As New List(Of AlertDefinition)

        Dim alertAboutRecurrences As Boolean
        Dim numberOfRecurrences As Integer
        Dim persistentInterval As Integer
        Dim persistentDuration As Integer

        Try
            Dim registry As New VSFramework.RegistryHandler()
            alertAboutRecurrences = False
            Try
                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to get AlertAboutRecurrencesOnly from the name_value collection", LogLevel.Verbose)
                alertAboutRecurrences = Convert.ToBoolean(registry.ReadFromRegistry("AlertAboutRecurrencesOnly"))
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " Error getting AlertAboutRecurrencesOnly from the name_value collection:  " & ex.ToString, LogLevel.Normal)
            End Try

            numberOfRecurrences = 0
            Try
                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to get NumberOfRecurrences from the name_value collection", LogLevel.Verbose)
                numberOfRecurrences = Convert.ToInt32(registry.ReadFromRegistry("NumberOfRecurrences"))
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " Error getting NumberOfRecurrences from the name_value collection:  " & ex.ToString, LogLevel.Normal)
            End Try

            persistentInterval = 0
            Try
                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to get PersistentAlertInterval from the name_value collection", LogLevel.Verbose)
                persistentInterval = Convert.ToInt32(registry.ReadFromRegistry("AlertInterval"))
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " Error getting Persistent Alert Interval from the name_value collection:  " & ex.ToString, LogLevel.Normal)
            End Try

            persistentDuration = 0
            Try
                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to get PersistentAlertDuration from the name_value collection", LogLevel.Verbose)
                persistentDuration = Convert.ToInt32(registry.ReadFromRegistry("AlertDuration"))
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " Error getting Persistent Alert Duration from the name_value collection:  " & ex.ToString, LogLevel.Normal)
            End Try
        Catch ex As Exception

        End Try


        'Make an array of objects for every configured 
        Try
            'Gets all the data from the DB
            filterNotifications = repoNotifications.Filter.Exists(Function(j) j.NotificationName, True)
            notificationsList = repoNotifications.Find(filterNotifications).ToList()

            filterEvents = repoEventsMaster.Filter.Exists(Function(j) j.NotificationList, True)
            eventsList = repoEventsMaster.Find(filterEvents).ToList()

            filterServers = repoServers.Filter.Exists(Function(j) j.NotificationList, True)
            serversList = repoServers.Find(filterServers).ToList()

            filterServersOther = repoServersOther.Filter.Exists(Function(j) j.NotificationList, True)
            serversList = repoServersOther.Find(filterServersOther).ToArray().Select(
                Function(x) New Server() With {
                .Id = x.Id,
                .DeviceName = x.Name,
                .DeviceType = x.Type
                }
            ).Concat(serversList).ToList()

            filterNotificationDest = repoNotificationDest.Filter.Exists(Function(x) x.Id, True)
            notificationDestList = repoNotificationDest.Find(filterNotificationDest).ToList()

            filterBusHrs = repoBusHrs.Filter.Exists(Function(x) x.Id, True)
            bushrsList = repoBusHrs.Find(filterBusHrs).ToList()

            filterScripts = repoScripts.Filter.Exists(Function(j) j.Id, True)
            scriptsList = repoScripts.Find(filterScripts).ToList()

            filterURLs = repoAlertURLs.Filter.Exists(Function(k) k.Id, True)
            URLsList = repoAlertURLs.Find(filterURLs).ToList()

            'Loops through each Notification and constructs a list of AlertDefinitions
            For Each notification As Notifications In notificationsList
                Try
                    'Makes a new AlertDefinition.
                    Dim currAlertDef As New AlertDefinition()
                    currAlertDef.NotificationId = notification.Id
                    currAlertDef.NotificationName = notification.NotificationName

                    currAlertDef.Servers = serversList.
                        Where(Function(x) x.NotificationList IsNot Nothing AndAlso x.NotificationList.Contains(notification.Id)).
                        Select(Function(x) New Servers() With {.DeviceName = x.DeviceName, .DeviceType = x.DeviceType, .DeviceId = x.Id}).
                        ToList()

                    currAlertDef.Events = eventsList.
                        Where(Function(x) x.NotificationList IsNot Nothing AndAlso x.NotificationList.Contains(notification.Id)).
                        Select(Function(x) New Events() With {.DeviceType = x.DeviceType, .EventType = x.EventType, .NotificationOnRepeat = x.NotificationOnRepeat}).
                        ToList()

                    'Loops through all the destinations for the definition and makes a new AlertService.Destinations() for it
                    For Each destination As NotificationDestinations In notificationDestList.Where(Function(x) notification.SendList.Contains(x.Id))
                        Try
                            Dim currDestination As New Destinations()
                            currDestination.DestinationId = destination.Id
                            currDestination.SendVia = destination.SendVia
                            currDestination.PersistantNotification = destination.PersistentNotification.HasValue AndAlso destination.PersistentNotification.Value = True
                            currDestination.Interval = destination.Interval

                            Dim currBusinessHours As List(Of BusinessHours) = bushrsList.Where(Function(x) x.Id = destination.BusinessHoursId).ToList()
                            If currBusinessHours.Count > 0 Then
                                currDestination.StartTimeString = currBusinessHours(0).StartTime
                                currDestination.Duration = currBusinessHours(0).Duration
                                currDestination.Days = currBusinessHours(0).Days.ToList()
                            End If

                            Select Case currDestination.SendVia
                                Case Destinations.SendType.Email.ToDescription()
                                    currDestination.SendTo = destination.SendTo
                                    currDestination.CopyTo = destination.CopyTo
                                    currDestination.BlindCopyTo = destination.BlindCopyTo

                                Case Destinations.SendType.SMS.ToDescription()
                                    currDestination.PhoneNumber = destination.SendTo

                                Case Destinations.SendType.Script.ToDescription()
                                    Dim currentScript As List(Of Scripts) = scriptsList.Where(Function(x) x.ScriptName = destination.SendTo).ToList()
                                    If currentScript.Count > 0 Then
                                        currDestination.ScriptName = destination.SendTo
                                        currDestination.ScriptLocation = currentScript(0).ScriptLocation
                                        currDestination.ScriptCommand = currentScript(0).ScriptCommand
                                    End If

                                     Case Destinations.SendType.URL.ToDescription()
                                      Dim currentURL As List(Of Alert_URLs) = URLsList.Where(Function(x) x.Name = destination.SendTo).ToList()
                                      If currentURL.Count > 0 Then
                                    currDestination.URL = currentURL.First().Url
                                     End If
                            End Select

                            currAlertDef.Destinations.Add(currDestination)
                        Catch ex As Exception
                            WriteServiceHistoryEntry(Now.ToString & " Error processing destination from DB. Notification ID: " & notification.Id & ". Destination ID: " + destination.Id & ". Error: " & ex.ToString, LogLevel.Normal)
                        End Try

                    Next
                    alertDefinitions.Add(currAlertDef)
                Catch ex As Exception
                    WriteServiceHistoryEntry(Now.ToString & " Error processing notification from DB. Notification ID: " & notification.Id & ". Error: " & ex.ToString, LogLevel.Normal)
                End Try
            Next
            WriteServiceHistoryEntry(Now.ToString & " There are " & alertDefinitions.Count() & " in memory.", LogLevel.Verbose)

            'Gets a list of all open issues
            filterEventsDetected = repoEventsDetected.Filter.Exists(Function(i) i.EventDismissed, False)
            Dim openEventsList As List(Of EventsDetected) = repoEventsDetected.Find(filterEventsDetected).ToList()


            Dim bulkOps As New List(Of WriteModel(Of VSNext.Mongo.Entities.EventsDetected))


            Dim remainingAlertsToday = GetMaxAlertsRemainingToday()
            WriteServiceHistoryEntry(Now.ToString & " There are " & openEventsList.Count() & " open events and a remaining alerts count of " & remainingAlertsToday & ".", LogLevel.Normal)
            'Loops through each open event
            For Each currEvent As EventsDetected In openEventsList
                'Loops through the AlertDefinitions which are subscribed to the events, which is determined by checking if a set exists where the device type and event type match
                'Compares the device type AND device ID for the very very slim chance there is a document in both server and server_other collections with the same id. Type will make sure it will be correct
                For Each currAlertDefinition As AlertDefinition In alertDefinitions.Where(Function(x) _
                        (x.Servers IsNot Nothing AndAlso x.Servers.Exists(Function(y) y.DeviceType = currEvent.DeviceType And y.DeviceId = currEvent.DeviceId)) And
                        (x.Events IsNot Nothing AndAlso x.Events.Exists(Function(y) y.DeviceType = currEvent.DeviceType And currEvent.EventType.Contains(y.EventType)))
                    )

                    'Checks if it can send via recurrence rules
                    Dim recurrencePassed As Boolean = False

                    Dim currEventFromDefinition As Events = currAlertDefinition.Events.
                            Where(Function(x) x.DeviceType = currEvent.DeviceType And currEvent.EventType.Contains(x.EventType)).First()

                    If alertAboutRecurrences AndAlso currEventFromDefinition.NotificationOnRepeat AndAlso currEvent.EventRepeatCount + 1 >= numberOfRecurrences Then
                        recurrencePassed = True
                    ElseIf alertAboutRecurrences AndAlso currEventFromDefinition.NotificationOnRepeat = False Then
                        recurrencePassed = True
                    ElseIf alertAboutRecurrences = False Then
                        recurrencePassed = True
                    End If

                    'If it should not send due to recurrence settings, continue
                    If recurrencePassed = False Then
                        WriteServiceHistoryEntry(Now.ToString & " Recurrence failed.  Stopping", LogLevel.Normal)
                        Exit For
                    End If

                    'Checks to see which destinations it has yet to send to
                    Dim sentDestinationIds As New List(Of String)
                    If currEvent.NotificationsSent IsNot Nothing Then
                        sentDestinationIds = currEvent.NotificationsSent.Where(Function(x) x.NotificationId = currAlertDefinition.NotificationId).Select(Function(x) x.NotificationDestinationId).ToList()
                    End If

                    Dim destinationsToSend As New List(Of Destinations)
                    If currAlertDefinition.Destinations IsNot Nothing Then
                        destinationsToSend = currAlertDefinition.Destinations.Where(Function(x) Not sentDestinationIds.Contains(x.DestinationId)).ToList()
                    End If

                    'If it is within persistent duration, check each of the sent destinations and see if it should resend
                    If currEvent.EventDetected.GetValueOrDefault().AddHours(persistentDuration) > Now Then
                        Dim notificationDestinationPersistantIds As New List(Of String)
                        If currEvent.NotificationsSent IsNot Nothing Then
                            'gets a list of max dates for each destiantion where the notification id matches
                            Dim maxDestinationsBasedOffDate = currEvent.NotificationsSent.
                                Where(Function(x) x.NotificationId = currAlertDefinition.NotificationId).
                                GroupBy(Function(x) x.NotificationDestinationId).
                                Select(Function(x) New With {.NotificationDestinationId = x.Key, .MaxDate = x.Max(Function(y) y.EventDetectedSent)}).
                                ToList()
                            'gets a list of destination ids where it is time to send due to persistant when it matches a case above
                            notificationDestinationPersistantIds = currEvent.NotificationsSent.
                                Where(Function(x) x.NotificationId = currAlertDefinition.NotificationId AndAlso maxDestinationsBasedOffDate.Exists(Function(y) y.NotificationDestinationId = x.NotificationDestinationId And y.MaxDate = x.EventDetectedSent)).
                                Where(Function(x) x.EventDetectedSent.GetValueOrDefault().AddMinutes(persistentInterval) < Now).
                                Select(Function(x) x.NotificationDestinationId).ToList()
                        End If

                        destinationsToSend.AddRange(currAlertDefinition.Destinations.Where(Function(x) notificationDestinationPersistantIds.Contains(x.DestinationId) And x.PersistantNotification = True))
                        destinationsToSend = destinationsToSend.Distinct().ToList()
                    End If

                    'Handles escalation
                    If destinationsToSend.Exists(Function(x) x.Interval IsNot Nothing) Then
                        destinationsToSend.RemoveAll(Function(x) x.Interval IsNot Nothing AndAlso currEvent.EventDetected.GetValueOrDefault().AddMinutes(x.Interval) > Now)
                    End If


                    'Loop through each destination. Determine if its time to send (business hours) and if allowed to send (max # of alerts per day/definition), then process it
                    For Each currDestination As Destinations In destinationsToSend

                        'Checks to see if the service has hit its daily limit. If so, stop
                        If remainingAlertsToday = 0 Then
                            'Queue System Message
                            WriteServiceHistoryEntry(Now.ToString & " No more alerts today.", LogLevel.Normal)
                            Exit For
                        End If

                        'Checks to see if the Definition has hit its daily limit. If so, stop
                        If GetMaxAlertsRemainingToday(currAlertDefinition.NotificationId) = 0 Then
                            'Queue System Messages
                            WriteServiceHistoryEntry(Now.ToString & " No more alerts for this def today.", LogLevel.Normal)
                            Exit For
                        End If

                        Dim timeToSend = IsItTimeToSendAlert(currDestination)
                        If Not timeToSend Then
                            WriteServiceHistoryEntry(Now.ToString & " Not time to send.", LogLevel.Normal)
                            Continue For
                        End If


                        'If all is finally good and can send, process the alert
                        Dim alertSent As Boolean = False
                        Try
                            Select Case currDestination.SendVia
                                Case Destinations.SendType.Email.ToDescription()
                                    WriteServiceHistoryEntry(Now.ToString & " Processing alert " & currEvent.Id & ". " & currEvent.DeviceType & " - " & currEvent.EventType & ".", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & " Processing alert definition " & currAlertDefinition.NotificationId & ".", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & " Processing alert destination " & currDestination.DestinationId & ".", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & " SENT EMAIL. /" & currDestination.SendTo & "/" & "/" & currDestination.CopyTo & "/" & currDestination.BlindCopyTo & "", LogLevel.Normal)
                                        alertSent = SendMailwithChilkatorNet(currDestination.SendTo, currDestination.CopyTo, currDestination.BlindCopyTo, currEvent.Device,
                                        currEvent.DeviceType, "", currEvent.EventType, currEvent.EventType, currEvent.Details, "", "Alert")
                                    'alertSent = True
                                Case Destinations.SendType.SMS.ToDescription()
                                    WriteServiceHistoryEntry(Now.ToString & " Processing alert " & currEvent.Id & ". " & currEvent.DeviceType & " - " & currEvent.EventType & ".", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & " Processing alert definition " & currAlertDefinition.NotificationId & ".", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & " Processing alert destination " & currDestination.DestinationId & ".", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & " SENT SMS. /" & currDestination.PhoneNumber & "", LogLevel.Normal)
                                        alertSent = SendSMSwithTwilio(currDestination.PhoneNumber, currEvent.Device, currEvent.DeviceType, currEvent.EventType, currEvent.Details, "", "")
                                    'alertSent = True
                                Case Destinations.SendType.Script.ToDescription()
                                    WriteServiceHistoryEntry(Now.ToString & " Processing alert " & currEvent.Id & ". " & currEvent.DeviceType & " - " & currEvent.EventType & ".", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & " Processing alert definition " & currAlertDefinition.NotificationId & ".", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & " Processing alert destination " & currDestination.DestinationId & ".", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & " SENT SCRIPT. /" & currDestination.ScriptName & "/" & "/" & currDestination.ScriptCommand & "/" & currDestination.ScriptLocation & "", LogLevel.Normal)
                                     alertSent = SendScript(currDestination.ScriptName, currDestination.ScriptCommand, currDestination.ScriptLocation, currEvent.Device,
                                    currEvent.DeviceType, currEvent.EventType, currEvent.Details, "")
                                    Case Destinations.SendType.URL.ToDescription()
                                        WriteServiceHistoryEntry(Now.ToString & " Processing alert " & currEvent.Id & ". " & currEvent.DeviceType & " - " & currEvent.EventType & ".", LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & " Processing alert definition " & currAlertDefinition.NotificationId & ".", LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & " Processing alert destination " & currDestination.DestinationId & ".", LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & " Calling URL. /" & currDestination.URL, LogLevel.Normal)
                                       alertSent = PostURL(currDestination.URL, currEvent.Device, currEvent.DeviceType, currEvent.EventType, currEvent.Details, False)

                            End Select
                        Catch ex As Exception
                            alertSent = False
                        End Try

                        'If alert was successfully sent, make a update def and add it to a bulk update
                        If alertSent Then
                            remainingAlertsToday -= 1

                            Dim notificationSentEntity As New NotificationsSent()
                            notificationSentEntity.EventDetectedSent = Now
                            notificationSentEntity.NotificationDestinationId = currDestination.DestinationId
                            notificationSentEntity.NotificationId = currAlertDefinition.NotificationId

                            Select Case currDestination.SendVia
                                Case Destinations.SendType.Email.ToDescription()
                                    notificationSentEntity.NotificationBccdTo = currDestination.BlindCopyTo
                                    notificationSentEntity.NotificationCcdTo = currDestination.CopyTo
                                    notificationSentEntity.NotificationSentTo = currDestination.SendTo
                                Case Destinations.SendType.SMS.ToDescription()
                                    notificationSentEntity.NotificationSentTo = currDestination.PhoneNumber
                                Case Destinations.SendType.Script.ToDescription()
                                    notificationSentEntity.NotificationSentTo = currDestination.ScriptName
                                    notificationSentEntity.ScriptCommand = currDestination.ScriptCommand
                                    notificationSentEntity.ScriptLocation = currDestination.ScriptLocation
                                Case Destinations.SendType.URL.ToDescription()
                                    notificationSentEntity.NotificationSentTo = currDestination.URL
                            End Select

                            'Handles alerts that will be cleared instantly with no cleared alert
                            If (currEvent.Details = "This is a TEST alert.") Or InStr(currEvent.EventType, "Log File") Or InStr(currEvent.EventType, "Dead Mail Deletion") Then
                                notificationSentEntity.EventDismissedSent = Now

                                Dim filterDefForEventDismissed As FilterDefinition(Of EventsDetected) = repoEventsDetected.Filter.Eq(Function(x) x.Id, currEvent.Id)
                                Dim updateDefForEventDismissed As UpdateDefinition(Of EventsDetected) = repoEventsDetected.Updater.Set(Function(x) x.EventDismissed, Now)
                                bulkOps.Add(New MongoDB.Driver.UpdateOneModel(Of VSNext.Mongo.Entities.EventsDetected)(filterDefForEventDismissed, updateDefForEventDismissed))

                            End If

                            Dim filterDefForUpdate As FilterDefinition(Of EventsDetected) = repoEventsDetected.Filter.Eq(Function(x) x.Id, currEvent.Id)
                            Dim updateDefForUpdate As UpdateDefinition(Of EventsDetected) = repoEventsDetected.Updater.Push(Function(x) x.NotificationsSent, notificationSentEntity)

                            bulkOps.Add(New MongoDB.Driver.UpdateOneModel(Of VSNext.Mongo.Entities.EventsDetected)(filterDefForUpdate, updateDefForUpdate))
                        Else
                            WriteServiceHistoryEntry(Now.ToString & " Failed to send notification", LogLevel.Normal)
                        End If



                    Next

                Next
            Next
            If (bulkOps.Count > 0) Then
                repoEventsDetected.BulkInsert(bulkOps)
            End If


        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error processing notification in ProcessAlertsSendNotifictaion. Error: " & ex.ToString, LogLevel.Normal)
        End Try


    End Sub

    Private Sub ProcessAlertsClearSendNotification()
        Try

            Dim counter As Int32 = 0
            Dim connString As String = GetDBConnection()
            Dim repoEventsMaster As New Repository(Of EventsMaster)(connString)
            Dim repoNotifications As New Repository(Of Notifications)(connString)
            Dim repoNotificationDest As New Repository(Of NotificationDestinations)(connString)
            Dim repoServers As New Repository(Of Server)(connString)
            Dim repoServersOther As New Repository(Of ServerOther)(connString)
            Dim repoBusHrs As New Repository(Of BusinessHours)(connString)
            Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)

            Dim filterNotificationDest As FilterDefinition(Of NotificationDestinations)
            Dim filterEventsDetected As FilterDefinition(Of EventsDetected)
            Dim filterBusHrs As FilterDefinition(Of BusinessHours)
            Dim updateEventsDetected As UpdateDefinition(Of EventsDetected)

            Dim notificationDestList As List(Of NotificationDestinations)
            Dim eventsCreated As List(Of EventsDetected)
            Dim alertDefinitions As New List(Of AlertDefinition)
            Dim businessHours As List(Of BusinessHours)

            Dim notificationsSentFilterBuilder As New FilterDefinitionBuilder(Of NotificationsSent)()


            Dim Details As String = ""

            'gets a list of all closed events with a event dismissed notification not yet sent and all the destinations defined
            filterEventsDetected = repoEventsDetected.Filter.Exists(Function(x) x.NotificationsSent, True) And
                repoEventsDetected.Filter.Exists(Function(x) x.EventDismissed, True) And
                repoEventsDetected.Filter.ElemMatch(Function(x) x.NotificationsSent, New FilterDefinitionBuilder(Of NotificationsSent)().Exists(Function(y) y.EventDismissedSent, False))
            eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToList()

            filterNotificationDest = repoNotificationDest.Filter.Exists(Function(x) x.Id)
            notificationDestList = repoNotificationDest.Find(filterNotificationDest).ToList()

            filterBusHrs = repoBusHrs.Filter.Exists(Function(x) x.Id)
            businessHours = repoBusHrs.Find(filterBusHrs).ToList()

            Dim bulkOps As New List(Of WriteModel(Of VSNext.Mongo.Entities.EventsDetected))

            'loops through each event found and then loops through all notifications not yet sent out
            For Each currEvent As EventsDetected In eventsCreated
                'If (currEvent.Id = "5a2eaabb721f5b5cec082895") Then
                'currEvent.Id = "5a2eaabb721f5b5cec082895"
                'End If
                Dim undismissedNotificationsSent As List(Of NotificationsSent) = currEvent.NotificationsSent.Where(Function(x) x.EventDismissedSent Is Nothing).ToList()
                Dim notificationsSentToClear As New List(Of NotificationsSent)

                undismissedNotificationsSent.
                    GroupBy(Function(x) New With {Key x.NotificationId, Key x.NotificationDestinationId}).
                    Select(Function(x) New With {.NotificationDestinationId = x.Key.NotificationDestinationId, .NotificationId = x.Key.NotificationId, .Id = x.Min(Function(y) y.Id)}).
                    ToList().
                    ForEach(Sub(x)
                                notificationsSentToClear.Add(undismissedNotificationsSent.First(Function(y) x.NotificationDestinationId = y.NotificationDestinationId And x.NotificationId = y.NotificationId And x.Id = y.Id))
                            End Sub)
                For Each currSendNotification As NotificationsSent In notificationsSentToClear
                    'Looks for the destination assigned ot the sent notifcation
                    If notificationDestList.Exists(Function(x) x.Id = currSendNotification.NotificationDestinationId) Then
                        Dim currNotificationDest As NotificationDestinations = notificationDestList.Where(Function(x) x.Id = currSendNotification.NotificationDestinationId).First()

                        'checks to see if its time to send the notification
                        Dim businessHourList As List(Of BusinessHours) = businessHours.Where(Function(x) x.Id = currNotificationDest.BusinessHoursId).ToList()
                        If businessHourList.Count() > 0 Then
                            Dim businessHour As BusinessHours = businessHourList.First()
                        Dim timeToSend = IsItTimeToSendAlert(New Destinations() With {.Days = businessHour.Days.ToList(), .StartTimeString = businessHour.StartTime, .Duration = businessHour.Duration})
                        If Not timeToSend Then
                            Continue For
                        End If
                        Else
                            WriteServiceHistoryEntry(Now.ToString & " Could not find a business definition for this notification. This can be due to escalation, removal of the definition or another issue. Notification Destination: " & currNotificationDest.Id, LogLevel.Normal)
                        End If

                        'attempts to send the notification
                        Details = currEvent.Details
                        Dim clearedAlertSent As Boolean = False
                        Try
                            Select Case currNotificationDest.SendVia
                                Case Destinations.SendType.Email.ToDescription()
                                    If (currEvent.EventType.Contains("Services")) Then
                                        Details = "The Original alert was Services -" & vbCrLf & Details
                                    Else
                                        Details = "The original alert was '" & currEvent.EventType & "' - " & vbCrLf & Details
                                    End If
                                    WriteServiceHistoryEntry(Now.ToString & " SENT EMAIL. /" & currNotificationDest.SendTo & "/" & "/" & currNotificationDest.CopyTo & "/" & currNotificationDest.BlindCopyTo & "", LogLevel.Normal)
                                    clearedAlertSent = SendMailwithChilkatorNet(currNotificationDest.SendTo, currNotificationDest.CopyTo, currNotificationDest.BlindCopyTo, currEvent.Device,
                                                             currEvent.DeviceType, "", currEvent.EventType, currEvent.EventType, Details, "Cleared ", "Alert")
                                    'clearedAlertSent = True

                                Case Destinations.SendType.SMS.ToDescription()
                                    Details = "The original alert was '" & currEvent.EventType & "' - " & vbCrLf & Details
                                    WriteServiceHistoryEntry(Now.ToString & " SENT SMS. /" & currNotificationDest.SendTo & "", LogLevel.Normal)
                                    clearedAlertSent = SendSMSwithTwilio(currNotificationDest.SendTo, currEvent.Device, currEvent.DeviceType, currEvent.EventType, Details, "Cleared ", "")
                                    'clearedAlertSent = True
                                Case Destinations.SendType.Script.ToDescription()
                                    Details = "The original alert was '" & currEvent.EventType & "' - " & vbCrLf & Details
                                    WriteServiceHistoryEntry(Now.ToString & " SENT SCRIPT. /" & currNotificationDest.SendTo & "/" & "/" & currNotificationDest.ScriptCommand & "/" & currNotificationDest.ScriptLocation & "", LogLevel.Normal)
                                    clearedAlertSent = SendScript(currNotificationDest.SendTo, currNotificationDest.ScriptCommand, currNotificationDest.ScriptLocation, currEvent.Device,
                                                                  currEvent.DeviceType, currEvent.EventType, Details, "Cleared ")
                                    'clearedAlertSent = True
                            End Select
                        Catch ex As Exception
                            clearedAlertSent = False
                        End Try

                        'makes a mongo update statement if notification successfully sent BE SURE TO UPDATE PERSISTANT IN MEMORY AND DB
                        If clearedAlertSent Then
                            filterEventsDetected = repoEventsDetected.Filter.Eq(Function(x) x.Id, currEvent.Id) And
                                    repoEventsDetected.Filter.ElemMatch(Function(x) x.NotificationsSent, notificationsSentFilterBuilder.Eq(Function(y) y.NotificationDestinationId, currSendNotification.NotificationDestinationId) And notificationsSentFilterBuilder.Eq(Function(y) y.NotificationId, currSendNotification.NotificationId) And notificationsSentFilterBuilder.Exists(Function(y) y.EventDismissedSent, False))
                            updateEventsDetected = repoEventsDetected.Updater.Set(Function(x) x.NotificationsSent(-1).EventDismissedSent, Now)
                            For i As Integer = 0 To currEvent.NotificationsSent.Where(Function(x) x.NotificationId = currSendNotification.NotificationId And x.NotificationDestinationId = currSendNotification.NotificationDestinationId).Count() - 1
                                bulkOps.Add(New MongoDB.Driver.UpdateOneModel(Of VSNext.Mongo.Entities.EventsDetected)(filterEventsDetected, updateEventsDetected))
                            Next

                        End If

                    End If
                Next
            Next
            If (bulkOps.Count > 0) Then
                repoEventsDetected.BulkInsert(bulkOps)
            End If


        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error in ProcessAlertsClearSendNotification: " & ex.ToString(), LogLevel.Normal)
        End Try
    End Sub

    Private Function IsItTimeToSendAlert(ByRef alertDestination As Destinations) As Boolean
        Try
            If alertDestination.Interval.HasValue Then
                Return True
            End If

            Dim isSpecific As Boolean = False
            Dim isDayIncluded As Boolean = False
            Dim startdt As DateTime
            Dim retVal As Integer = 0

            startdt = DateTime.Parse(alertDestination.StartTimeString)
            Dim enddt As DateTime = startdt.AddMinutes(alertDestination.Duration)
            If startdt.Day = enddt.Day Then
                'do nothing
            Else
                'different days, goes over midnight
                If Now.Hour > startdt.Hour Or (Now.Hour = startdt.Hour AndAlso Now.Minute > startdt.Minute) Then
                    'if on 1st day
                    'do it normally
                Else
                    startdt = startdt.AddDays(-1)
                    enddt = enddt.AddDays(-1)
                End If
            End If
            isDayIncluded = IIf(alertDestination.Days.IndexOf(startdt.DayOfWeek.ToString()) <> -1, True, False)
            If startdt < Now And enddt > Now And isDayIncluded Then
                Return True
            End If
            Return False
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error in IsItTimeToSendAlert: " & ex.ToString, LogLevel.Normal)
            Return True
        End Try


    End Function

    Public Function SendMailwithChilkatorNet(ByVal SendTo As String, ByVal CC As String, ByVal BCC As String, ByVal ServerName As String,
     ByVal ServerType As String, ByVal Location As String, ByVal EventName As String, ByVal AlertType As String,
     ByVal Details As String, ByVal iscleared As String, ByVal SubjectStr As String) As Boolean
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
        Return emailSent
    End Function
    '12/1/2014 NS added for VSPLUS-946
    Public Function SendSMSwithTwilio(ByVal SMSTo As String, ByVal ServerName As String,
     ByVal ServerType As String, ByVal AlertType As String,
     ByVal Details As String, ByVal iscleared As String, ByVal PFrom As String) As Boolean
        Dim SMSAccountSid As String
        Dim SMSAuthToken As String
        Dim SMSFrom As String
        Try
            SMSAccountSid = getSettings("SMSAccountSid")
            SMSAuthToken = getSettings("SMSAuthToken")
            SMSFrom = getSettings("SMSFrom")
            Return SendSMS(SMSAccountSid, SMSAuthToken, iscleared, ServerType, ServerName, AlertType, Details, SMSTo, SMSFrom)
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error getting SMS account settings from the Settings table:  " & ex.ToString, LogLevel.Normal)
        End Try
    End Function
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
            Password = ""
            If (Ppwd <> "") Then
                strEncryptedPassword = Ppwd
                str1 = strEncryptedPassword.Split(",")
                Dim bstr1(str1.Length - 1) As Byte
                For j As Integer = 0 To str1.Length - 1
                    bstr1(j) = str1(j).ToString()
                Next
                myPass = bstr1
                If Not strEncryptedPassword Is Nothing Then
                    Password = mySecrets.Decrypt(myPass) 'password in clear text, stored in memory now
                End If
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


    Public Function PostURL(ByVal URL As String,
       ByVal ServerName As String, ByVal ServerType As String, ByVal AlertType As String,
       ByVal Details As String, ByVal iscleared As String) As Boolean

        Dim success As Boolean = True
        Dim ServerNameParam As String = "%Name%"
        Dim ServerTypeParam As String = "%Type%"
        Dim EventTypeParam As String = "%EventType%"
        Dim AlertDetailsParam As String = "%Details%"
        Dim DTDParam As String = "%DTD%"

        'DTD is date time detected 

        Try
            WriteServiceHistoryEntry(Now.ToString & " Attempting to parametrize URL " & URL, LogLevel.Normal)
            URL = URL.Substring(URL.IndexOf(" ") + 1)
            URL = URL.Replace(ServerNameParam, ServerName)
            URL = URL.Replace(ServerTypeParam, ServerType)
            URL = URL.Replace(EventTypeParam, AlertType)
            URL = URL.Replace(AlertDetailsParam, Details)
            URL = URL.Replace(DTDParam, Now.ToShortDateString & " " & Now.ToShortTimeString)
            WriteServiceHistoryEntry(Now.ToString & " Converted parameters.  Now attempting to post to " & URL, LogLevel.Normal)

        Catch ex As Exception

            WriteServiceHistoryEntry(Now.ToString & " Exception In PostURL: " & ex.Message, LogLevel.Normal)
        End Try

        Try
            WriteServiceHistoryEntry(Now.ToString & " Attempting to create a web request. " & URL, LogLevel.Verbose)

            ' Create a request using the URL  
            Dim request As WebRequest = WebRequest.Create(URL)

            ' If required by the server, set the credentials.  
            ' request.Credentials = CredentialCache.DefaultCredentials
            ' Get the response.  
            WriteServiceHistoryEntry(Now.ToString & " Attempting to create a web response. " & URL, LogLevel.Verbose)

            Dim response As WebResponse = request.GetResponse()
            ' Display the status.  
            WriteServiceHistoryEntry(Now.ToString & " URL status is " & (CType(response, HttpWebResponse).StatusDescription))
            ' Get the stream containing content returned by the server.  
            WriteServiceHistoryEntry(Now.ToString & " Attempting to to get a web response stream.", LogLevel.Verbose)

            Dim dataStream As Stream = response.GetResponseStream()
            ' Open the stream using a StreamReader for easy access.  
            Dim reader As New StreamReader(dataStream)
            ' Read the content.  
            Dim responseFromServer As String = reader.ReadToEnd()
            ' Display the content.  
            WriteServiceHistoryEntry(Now.ToString & " URL response " & responseFromServer, LogLevel.Verbose)
            ' Clean up the streams and the response.  
            reader.Close()
            response.Close()
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Exception posting to URL: " & ex.ToString)
        End Try

        If success Then
            WriteServiceHistoryEntry(Now.ToString & " Posted to " & URL, LogLevel.Normal)
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
    'Private Sub InsertingSentMails(ByVal AlertID As String, ByVal SentTo As String, ByVal CcdTo As String, ByVal BccdTo As String,
    '                               ByVal resent As Boolean, ByVal AlertKey As String, Optional ByVal ScriptCommand As String = "",
    '                               Optional ByVal ScriptLocation As String = "")
    '    Dim connString As String = GetDBConnection()
    '    Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
    '    Dim filterEventsDetected As FilterDefinition(Of EventsDetected)
    '    Dim eventsCreated() As EventsDetected
    '    Dim notificationsSent As List(Of NotificationsSent)
    '    Dim strdt As String
    '    Dim oid As String
    '    Dim nid As New ObjectId
    '    Dim notificationentity As New NotificationsSent

    '    Try
    '        WriteServiceHistoryEntry(Now.ToString & " Updating sent event for " & SentTo & IIf(ScriptLocation <> "", ", " & ScriptLocation & ", " & ScriptCommand, ""), LogLevel.Verbose)
    '        strdt = Date.Now
    '        oid = AlertKey
    '        nid = ObjectId.GenerateNewId()
    '        If resent Then
    '            filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True),
    '                                                                 repoEventsDetected.Filter.Eq(Of String)(Function(j) j.Id, AlertID))
    '            eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToArray()
    '            If eventsCreated.Length > 0 Then
    '                If ScriptLocation = "" Or ScriptCommand = "" Then
    '                    notificationentity = New NotificationsSent With {.Id = nid.ToString(), .NotificationId = oid, .NotificationSentTo = SentTo, .NotificationCcdTo = CcdTo, .NotificationBccdTo = BccdTo, .EventDetectedSent = strdt}
    '                Else
    '                    notificationentity = New NotificationsSent With {.Id = nid.ToString(), .NotificationId = oid, .NotificationSentTo = SentTo, .NotificationCcdTo = CcdTo, .NotificationBccdTo = BccdTo, .EventDetectedSent = strdt, .ScriptCommand = ScriptCommand, .ScriptLocation = ScriptLocation}
    '                End If
    '                eventsCreated(0).NotificationsSent.Add(notificationentity)
    '                repoEventsDetected.Replace(eventsCreated(0))
    '            End If
    '        Else
    '            filterEventsDetected = repoEventsDetected.Filter.Eq(Of String)(Function(j) j.Id, AlertID)
    '            eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToArray()
    '            If eventsCreated.Length > 0 Then
    '                If ScriptLocation = "" Or ScriptCommand = "" Then
    '                    notificationentity = New NotificationsSent With {.Id = nid.ToString(), .NotificationId = oid, .NotificationSentTo = SentTo, .NotificationCcdTo = CcdTo, .NotificationBccdTo = BccdTo, .EventDetectedSent = strdt}
    '                Else
    '                    notificationentity = New NotificationsSent With {.Id = nid.ToString(), .NotificationId = oid, .NotificationSentTo = SentTo, .NotificationCcdTo = CcdTo, .NotificationBccdTo = BccdTo, .EventDetectedSent = strdt, .ScriptCommand = ScriptCommand, .ScriptLocation = ScriptLocation}
    '                End If
    '                If eventsCreated(0).NotificationsSent Is Nothing Then
    '                    notificationsSent = New List(Of NotificationsSent)
    '                    notificationsSent.Add(notificationentity)
    '                    eventsCreated(0).NotificationsSent = notificationsSent
    '                Else
    '                    eventsCreated(0).NotificationsSent.Add(notificationentity)
    '                End If
    '                repoEventsDetected.Replace(eventsCreated(0))
    '            End If
    '        End If
    '    Catch ex As ApplicationException
    '        WriteServiceHistoryEntry(Now.ToString & " Error occurred at the time of updating an events_detected document: " & AlertID & " , " & SentTo & "," & CcdTo & "," & BccdTo & ": " & ex.Message, LogLevel.Normal)
    '    End Try
    'End Sub
    'Private Sub UpdatingSentMails(ByVal AlertID As String, ByVal SentTo As String, ByVal CcdTo As String, ByVal BccdTo As String,
    '                              ByVal id As String)
    '    Dim connString As String = GetDBConnection()
    '    Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
    '    Dim filterEventsDetected As FilterDefinition(Of EventsDetected)
    '    Dim eventsCreated() As EventsDetected
    '    Dim notificationsArr() As NotificationsSent

    '    Try
    '        Dim strdt As String
    '        strdt = Date.Now
    '        filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True),
    '                                                             repoEventsDetected.Filter.Eq(Of String)(Function(j) j.Id, AlertID))
    '        eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToArray()
    '        If eventsCreated.Length > 0 Then
    '            notificationsArr = eventsCreated(0).NotificationsSent.ToArray()
    '            For i As Integer = 0 To notificationsArr.Length - 1
    '                WriteServiceHistoryEntry(Now.ToString & " AlertID: " & AlertID & ", SentTo: " & SentTo & ", CcdTo: " & CcdTo & ", BccdTo: " & BccdTo & ", id: " & id, LogLevel.Verbose)
    '                WriteServiceHistoryEntry(Now.ToString & " NotificationSentTo: " & notificationsArr(i).NotificationSentTo & ", NotificationCcdTo: " & notificationsArr(i).NotificationCcdTo & ", NotificationBccdTo: " & notificationsArr(i).NotificationBccdTo, LogLevel.Verbose)
    '                notificationsArr(i).EventDismissedSent = strdt
    '                eventsCreated(0).NotificationsSent(i) = notificationsArr(i)
    '            Next
    '            repoEventsDetected.Replace(eventsCreated(0))
    '            WriteServiceHistoryEntry(Now.ToString & " Updated the events_detected collection, the notifications_sent.event_dismissed_sent value for " & id, LogLevel.Normal)
    '        End If
    '    Catch ex As ApplicationException
    '        WriteServiceHistoryEntry(Now.ToString & " Error occurred at the time of updating a document with id: " & id & " , sentto: " & SentTo & "," & CcdTo & "," & BccdTo & " in the events_detected collection: " & ex.Message, LogLevel.Normal)
    '    End Try

    'End Sub
    'Private Sub InsertSentEscalation(ByRef eventCreated As EventsDetected, ByVal oid As String, ByVal eid As String, ByVal EscalateTo As String)
    '    Dim connString As String = GetDBConnection()
    '    Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
    '    Dim notificationsSent As List(Of NotificationsSent)
    '    Dim strdt As String

    '    Try
    '        strdt = Date.Now
    '        notificationsSent = eventCreated.NotificationsSent
    '        If notificationsSent.Count > 0 Then
    '            Dim notificationentity As New NotificationsSent With {.NotificationId = oid, .EscalationId = eid, .NotificationSentTo = EscalateTo, .EventDetectedSent = strdt}
    '            notificationsSent.Add(notificationentity)
    '            eventCreated.NotificationsSent = notificationsSent
    '            repoEventsDetected.Replace(eventCreated)
    '        End If
    '    Catch ex As ApplicationException
    '        WriteServiceHistoryEntry(Now.ToString & " Error occurred at the time of inserting value " & oid.ToString() & " , " & EscalateTo & " into  the notifications_sent embedded document in events_detected " & ex.Message, LogLevel.Normal)
    '    End Try
    'End Sub
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
            Try
                If getSettings("EnableAlertLimits").ToString() = "False" Then
                    Return -1
                End If
            Catch ex As Exception

            End Try

            If AlertKey = "" Then
                filterEventsDetected = repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True)
                filterSettings = repoSettings.Filter.Eq(Function(j) j.Name, "TotalMaximumAlertsPerDay")
            Else
                filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True),
                                                                     repoEventsDetected.Filter.ElemMatch(Function(j) j.NotificationsSent, New FilterDefinitionBuilder(Of NotificationsSent)().Eq(Function(x) x.NotificationId, AlertKey)))
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
