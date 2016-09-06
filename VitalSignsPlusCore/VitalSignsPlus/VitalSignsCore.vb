
'Also use IP*Works components, see http://www.ipworks.com
' .NET Reactor  www.ezriz.com

'Written by Alan Forbes
'64 Pleasant Street
'Rowley MA 01969

'Alan.Forbes@ServerVitalSigns.com
'mzldad@yahoo.com
'Phone: 781-608-4060
'Copyright 2012, All Rights Reserved

'The IANA has assigned the following Private Enterprise
'Number to MZL Software Development: 26062
'Use this number for sending SNMP traps

'Unlock Codes

'Chilkat Email: MZLDADMAILQ_8nzv7Kxb4Rpx
'Chilkat IMAP: MZLDADIMAPMAILQ_pVLoRztR0R2C
'Chilkat Bounce: MZLDADBOUNCE_oZEGkf5WnB8z
'Chilkat FTP2: MZLDADFTP_h5YXQ9tfmXnp
'Chilkat HTTP: MZLDADHttp_efwTynJYYR3X
'Chilkat Zip: MZLDADZIP_2nmsDacb4Ywu
'Chilkat Crypt: MZLDADCrypt_O4B5bLhFKR9v
'Chilkat RSA: MZLDADRSA_a5TXFDL6nZys
'Chilkat DSA: MZLDADDSA_7nesBOXUlLnc
'Chilkat SSH: MZLDADSSH_8vqwurX5lRxy
'Chilkat Diffie-Hellman: MZLDADDiffie_ZQm557JPTQ1K
'Chilkat Compression: MZLDADCompress_HnNPJNONNR6S
'Chilkat MHT: MZLDADMHT_ELKz2wky3Kw6
'Chilkat Mime: MZLDADSMIME_wnaeDOWt8M4o
'Chilkat HTML-to-XML: MZLDADHtmlToXml_gAmV8vUrYK3c
'Chilkat Socket: MZLDADSocket_OACwPK2ZlEn9
'Chilkat Charset: MZLDADCharset_Dma5ME8BUP8m
'Chilkat XMP: MZLDADXMP_yFT5SdmJ8Rxx
'Chilkat PFX: MZLDADPFX_tuJ2oG9K1Tvk
'Chilkat TAR: MZLDADTarArch_9JgbzEwR2RGY

Imports System.ServiceProcess
Imports System.Configuration
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports MonitoredItems.MonitoredDevice.Alert
'Imports System.ComponentModel
'Imports System.Windows.Forms
Imports System.Net
Imports System.Net.Sockets
Imports nsoftware.IPWorks
Imports System.Xml
Imports System.Globalization
Imports VSFramework
Imports System.Reflection
Imports System.Runtime.Serialization.Json
Imports System.Runtime.Serialization
Imports VitalSignsWebSphereDLL
Imports System.Linq

Imports RPRWyatt.VitalSigns.Services
Imports System.Security.Cryptography.X509Certificates


Public Class VitalSignsPlusCore
    Inherits VSServices
    Dim BuildNumber As Integer = 2275
    Dim ProductName As String 'value set in start up 
    Dim CompanyName As String = "RPR Wyatt"

    Private Shared mut As New Mutex()
    Dim sCultureString As String = "en-US"
    Dim connectionStringName As String = "CultureString"
    Dim strDateFormat As String
    Dim objDateUtils As New DateUtils.DateUtils

    'Used by Mobile Device Sync Tracking
    Dim MOBILE_ALERT As String = "Mobile Users"

    Dim BoolOffHours As Boolean = False

    'Threads
    ' Dim MasterDominoThread As New Thread(AddressOf MonitorAllThingsDomino)
    Dim CheckThreads As New Thread(AddressOf ThreadChecker)
    Dim ThreadMainMailServices As New Thread(AddressOf MonitorMailServices)
    Dim ThreadSametimeServers As New Thread(AddressOf MonitorSametimeThread)
    Dim ThreadBlackBerryServers As New Thread(AddressOf MonitorBlackBerryServers)
    Dim ThreadWebSphereServers As New Thread(AddressOf MonitorWebSphere)
    Dim ThreadIBMConnectServers As New Thread(AddressOf MonitorIBMConnect)

    '  Dim WithEvents PingControl As New nsoftware.IPWorks.Ping("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000")
    Dim WithEvents HTTP As New nsoftware.IPWorks.Http("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000")
    Dim IPPort As New nsoftware.IPWorks.Ipport("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000")
    Dim WithEvents ChilkatSametimeHTTP As New Chilkat.Http


    'Thread Last Update Global Variables
    Dim dtDominoLastUpdate As DateTime = Now
    Dim dtDominoClusterLastUpdate As DateTime = Now
    Dim dtNetworkDevicesLastUpdate As DateTime = Now
    Dim dtMailServicesLastUpdate As DateTime = Now
    Dim dtURLsLastUpdate As DateTime = Now
    Dim dtAlertsLastUpdate As DateTime = Now
    Dim dtBESLastUpdate As DateTime = Now
    Dim dtDNSLastUpdate As DateTime = Now
    Dim dtSametimeLastUpdate As DateTime = Now
    Dim dtWebSphereLastUpdate As DateTime = Now
    Dim dtIBMConnectLastUpdate As DateTime = Now

    Dim myAlert As New AlertLibrary.Alertdll
    Dim AlertOnStuckMessages As Boolean = False
    Dim boolTimeToStop As Boolean = False

    Dim boolMonitorMailServices As Boolean = True
    Dim boolMonitorNetworkDevices As Boolean = True
    Dim boolMonitorBlackBerry As Boolean = True

    ' Monitor log file & other paths
    Private strLogDest As String

    Private strAppPath As String
    Dim strAuditText As String
    Dim strServersMDBPath As String
    Dim myStatusURL As String

    'Store the passwords in these variables
    Dim MyDominoPassword As String
    Dim MySMTPPassword As String


    'Sametime Related
    Dim xmlSametime As String = ""
    'Dim SametimeUserOne As String = ""
    'Dim mySametimePassword1 As String = ""

    'Dim SametimeUserTwo As String = ""
    'Dim mySametimePassword2 As String = ""

    Dim CurrentSametime As String 'Contains the name of the Sametime server currently being queried
    Dim AdvancedSametime As Boolean = True 'Specifies whether to call the \servlet\statistics for monitoring st
    Dim boolMonitorSametime As Boolean = True

    Dim CurrentWebSphere As String 'Contains the name of the WebSphere server currently being queried
    Dim boolMonitorWebSphere As Boolean = True

    Dim CurrentIBMConnect As String 'Contains the name of the IBM Connect server currently being queried
    Dim boolMonitorIBMConnect As Boolean = True

    Dim BannerText As String

    'Collection of Domino servers to monitor
    Dim MyDominoServers As New MonitoredItems.DominoCollection
    Dim MyDominoServer As MonitoredItems.DominoServer

    'Collection of Domino Mail Clusters to monitor
    Dim myDominoClusters As New MonitoredItems.DominoMailClusterCollection
    Dim myDominoCluster As New MonitoredItems.DominoMailCluster

    'Collection of BlackBerry Servers to monitor
    Dim MyBlackBerryServers As New MonitoredItems.BlackBerryServerCollection
    Dim myBlackBerryServer As MonitoredItems.BlackBerryServer

    'Collection of BlackBerry Probes
    Dim MyBlackBerryProbe As MonitoredItems.BlackBerryMailProbe
    Dim MyBlackBerryProbes As New MonitoredItems.BlackBerryMailProbeCollection

    'Collection of BlackBerry Queues
    Dim MyBlackBerryQueue As MonitoredItems.BlackBerryQueue
    Dim myBlackBerryQueues As New MonitoredItems.BlackBerryQueueCollection

    'Collection of BlackBerry Users
    Dim myBlackBerryUser As MonitoredItems.BlackBerryUser
    Dim myBlackBerryUsers As New MonitoredItems.BlackBerryUsersCollection

    'Collection of Notes Databases
    Dim MyNotesDatabase As MonitoredItems.NotesDatabase
    Dim MyNotesDatabases As New MonitoredItems.NotesDatabaseCollection

    Dim MySametimeServer As MonitoredItems.SametimeServer
    Dim mySametimeServers As New MonitoredItems.SametimeServersCollection

    'Collection of NotesMailProbes
    Dim MyNotesMailProbe As MonitoredItems.DominoMailProbe
    Dim MyNotesMailProbes As New MonitoredItems.DominoMailProbeCollection

    'Collection of DNS Servers
    Dim MyDNS_Server As MonitoredItems.DNS_Server
    Dim MyDNS_Servers As New MonitoredItems.DNS_ServersCollection



    'Collection of Mail Services
    Dim MyMailService As MonitoredItems.MailService
    Dim MyMailServices As New MonitoredItems.MailServiceCollection

    Dim MyConsoleCommand As MonitoredItems.ScheduledServerCommand
    Dim MyConsoleCommands As New MonitoredItems.DominoConsoleCommandCollection

    'Collection of WebSphere Servers
    Dim MyWebSphereServer As MonitoredItems.WebSphere
    Dim MyWebSphereServers As New MonitoredItems.WebSphereCollection

    'Collection of IBM Connect Servers
    Dim MyIBMConnectServer As MonitoredItems.IBMConnect
    Dim MyIBMConnectServers As New MonitoredItems.IBMConnectCollection

    'URL Related
    Dim ListOfURLThreads As List(Of Threading.Thread) = New List(Of Threading.Thread)
    Private Shared URLSelector_Mutex As New Mutex()

    Dim MyURL As MonitoredItems.URL
    Dim MyURLs As New MonitoredItems.URLCollection
    Dim CurrentURL As String 'Contains the name of the URL currently being queried
    Dim URLStringFound As Boolean
    'Dim dtURLsLastUpdate As DateTime = Now

    '10/6/2014 NS added for VSPLUS-1002
    'Cloud Related
    Dim ListOfCloudThreads As List(Of Threading.Thread) = New List(Of Threading.Thread)
    Private Shared CloudSelector_Mutex As New Mutex()

    Dim MyCloud As MonitoredItems.Cloud
    Dim MyClouds As New MonitoredItems.CloudCollection
    Dim CurrentCloud As String 'Contains the name of the Cloud URL currently being queried
    Dim CloudStringFound As Boolean

    'Determines the verbosity of the log file
    Enum LogLevel
        Verbose = LogUtilities.LogUtils.LogLevel.Verbose
        Debug = LogUtilities.LogUtils.LogLevel.Debug
        Normal = LogUtilities.LogUtils.LogLevel.Normal
    End Enum

    'MyLogLevel is used throughout to control the volume of the log file output
    Dim MyLogLevel As LogLevel


    'Used for displaying the correct icon in the status grid of the client app
    Enum IconList
        NoIcon = 0
        DominoServer = 1
        Network_Device = 2
        URL = 3
        NotesMail_Probe = 4
        BlackBerry_Probe = 5
        LDAP = 6
        Mail_Service = 7
        NotesDB = 8
        BESQ = 9
        Sametime = 10
        Quickr = 11
    End Enum

    Dim ThreadHourlyDaily As New Thread(AddressOf PerformHourlyDailyTasks)

    'WS added for VSPLUS-2239
    Dim SametimeProcess As Process

    Protected Overrides Sub ServiceOnStart(ByVal args() As String)
        Try
            sCultureString = ConfigurationManager.AppSettings(connectionStringName).ToString()
        Catch ex As Exception
            sCultureString = "en-US"
        End Try

        Try
            EventLog.WriteEntry("VitalSigns Core Services", "Attempting to start. ", EventLogEntryType.Information)
        Catch ex As Exception

        End Try

        Try
            Dim success As Boolean
            success = ChilkatSametimeHTTP.UnlockComponent("MZLDADHttp_efwTynJYYR3X")
            ' WriteAuditEntry(Now.ToString + " Chilkat HTTP component is unlocked: " & ChilkatSametimeHTTP.IsUnlocked)
        Catch ex As Exception

        End Try

        Try
            strAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
        Catch ex As Exception
            strAppPath = "d:\vitalsigns\"
        End Try

        'Read some settings from the registry
        Dim myRegistry As New VSFramework.RegistryHandler()

        Try
            'WriteAuditEntry(Now.ToString & " Querying SQL server for the current date format.", LogLevel.Normal)
            strDateFormat = objDateUtils.GetDateFormat()
            'WriteAuditEntry(Now.ToString & " The current date format is " & strDateFormat, LogLevel.Normal)
        Catch ex As Exception

        End Try

        Try
            ProductName = myRegistry.ReadFromRegistry("ProductName")
            If ProductName = "" Then
                ProductName = "VitalSigns"
            End If
        Catch ex As Exception
            ProductName = "VitalSigns"
        End Try

        Try
            MyLogLevel = myRegistry.ReadFromRegistry("Log Level")
        Catch ex As Exception
            MyLogLevel = LogLevel.Normal
        End Try


        Try
            myRegistry.WriteToRegistry("Service Start", Now.ToShortDateString & " " & Now.ToShortTimeString)
            myRegistry.WriteToRegistry("VS Domino Service Start", Now.ToShortDateString & " " & Now.ToShortTimeString)
            'Sowjanya 1558 ticket
            myRegistry.WriteToRegistry("Core Features Start", Now.ToShortDateString & " " & Now.ToShortTimeString)
            myRegistry.WriteToRegistry("VS Domino Build Number", BuildNumber)
            myRegistry.WriteToRegistry("Service Build Number", BuildNumber)
            'myRegistry.WriteToRegistry("Service Reset Date", Now.ToShortDateString)

        Catch ex As Exception
            EventLog.WriteEntry("VitalSigns Core Services", ex.ToString(), EventLogEntryType.Error)
        End Try



        Try
            WriteAuditEntry(Now.ToString + " ")
            WriteAuditEntry(Now.ToString + " ********************************* ")
            WriteAuditEntry(Now.ToString + " VitalSigns Plus Core Services- Build Number: " & BuildNumber)
            WriteAuditEntry(Now.ToString + " The service is starting up.")
            WriteAuditEntry(Now.ToString + " ")
            WriteAuditEntry(Now.ToString + " Copyright 2006 - " & Now.Year & ", JNIT, Inc. dba RPR Wyatt")
            WriteAuditEntry(Now.ToString + " All rights reserved.")
            WriteAuditEntry(Now.ToString + " Distributed worldwide by RPR Wyatt and its partner network.")
            WriteAuditEntry(Now.ToString + " ")
            WriteAuditEntry(Now.ToString + " ")
            ' Thread.CurrentThread.Sleep(4000)

        Catch ex As Exception
            EventLog.WriteEntry("VitalSigns Core Services", ex.ToString(), EventLogEntryType.Error)
        End Try

        'Get the passwords from the registry
        'Dim MyPass As Byte()
        'Dim mySecrets As New VSFramework.TripleDES


        'Try
        '	SametimeUserOne = myRegistry.ReadFromRegistry("Sametime User 1")
        '	WriteAuditEntry(Now.ToString & " Sametime User One is " & SametimeUserOne)
        'MyPass = myRegistry.ReadFromRegistry("Sametime Password 1")	 'sametime password as encrypted byte stream
        '	If Not MyPass Is Nothing Then
        '		mySametimePassword1 = mySecrets.Decrypt(MyPass)	'password in clear text, stored in memory now
        '	Else
        '		mySametimePassword1 = Nothing
        '	End If
        '	' WriteAuditEntry(Now.ToString & " Sametime User One password is " & mySametimePassword1)
        'Catch ex As Exception
        '	MyPass = Nothing
        'End Try

        'Try
        '	SametimeUserTwo = myRegistry.ReadFromRegistry("Sametime User 2")
        '	WriteAuditEntry(Now.ToString & " Sametime User Two is " & SametimeUserTwo)
        '	MyPass = myRegistry.ReadFromRegistry("Sametime Password 2")	 'sametime password as encrypted byte stream
        '	If Not MyPass Is Nothing Then
        '		mySametimePassword2 = mySecrets.Decrypt(MyPass)	'password in clear text, stored in memory now
        '	Else
        '		mySametimePassword2 = Nothing
        '	End If

        'Catch ex As Exception
        '	SametimeUserOne = ""
        'End Try


        'MyPass = Nothing


        Try
            CreateCollections()
        Catch ex As Exception
            EventLog.WriteEntry("VitalSigns Core Services", "Error creating collections " & ex.ToString(), EventLogEntryType.Error)
        End Try



        Try
            InitializeStatusTable()
        Catch ex As Exception

        End Try


        Try
            WriteAuditEntry(Now.ToString & " All configuration settings have been read.  Starting monitoring threads.")
            ' Thread.CurrentThread.Sleep(2000)
            StartThreads()
        Catch ex As Exception

        End Try

        WriteAuditEntry(Now.ToString & " Start up is complete.")

    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.

        boolTimeToStop = True

        Try
            UpdateStatusTable("Update Status SET  Details='URL monitoring is not running.', Status = 'Not Scanned', StatusCode='Maintenance', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0, Description='The VitalSigns monitoring service is not monitoring URLs.' WHERE Type = 'URL' ")
            UpdateStatusTable("Update Status SET  Details='Mail Service monitoring is not running.',  StatusCode='Maintenance', Status = 'Not Scanned', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0, Description='The VitalSigns monitoring service is not monitoring Mail Services.' WHERE Type like  'Mail%' ")

            UpdateStatusTable("Update Status SET  Details='Sametime Server monitoring is not running.', StatusCode='Maintenance', Status = 'Not Scanned', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0 WHERE Type like 'Sametime%' ")
            UpdateStatusTable("Update Status SET  Details='Network Device monitoring is not running.',   StatusCode='Maintenance', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0 WHERE Type like 'Network%' ")
            UpdateStatusTable("Update Status SET  Details='The URL monitoring service is not running.', Status='Not Scanned', StatusCode='Maintenance', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0, Description='URL monitoring is not running.' WHERE Type ='URL'")

        Catch ex As Exception

        End Try
        'End
        MyBase.OnStop()
    End Sub


    Protected Sub StartThreads()

        Dim myRegistry As New VSFramework.RegistryHandler()

        'Start monitoring Sametime Servers
        If boolMonitorSametime = True And mySametimeServers.Count > 0 Then
            StartSametimeThreads()
        Else
            WriteAuditEntry(Now.ToString & " Sametime monitoring is DISABLED...")
        End If

        Try
            If boolMonitorBlackBerry = True And MyBlackBerryServers.Count > 0 Then
                StartBlackBerryThreads()
            End If
        Catch ex As Exception

        End Try


        Try
            'Start monitoring Mail Services
            If boolMonitorMailServices = True And MyMailServices.Count > 0 Then
                StartMailServicesThread()
            Else
                WriteAuditEntry(Now.ToString & " Mail Services monitoring is DISABLED...")
            End If
        Catch ex As Exception

        End Try

        Try
            Dim EnabledURLsCount As Integer = 0
            Dim URLOne As MonitoredItems.URL
            For n = 0 To MyURLs.Count - 1
                URLOne = MyURLs.Item(n)
                If URLOne.Enabled = True Then EnabledURLsCount += 1
            Next n
            Dim intThreadCount As Integer = CInt(EnabledURLsCount / 4)
            If intThreadCount <= 1 Then
                intThreadCount = 2
            End If

            WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " There are " & EnabledURLsCount & " enabled URLs to scan.")
            WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " I am launching " & intThreadCount & " threads to scan these URLs.")

            Try
                For n = 1 To intThreadCount
                    Dim tTemp As Threading.Thread = New Threading.Thread(AddressOf MonitorURL)
                    tTemp.CurrentCulture = New CultureInfo(sCultureString)
                    tTemp.Start()

                    ListOfURLThreads.Add(tTemp)
                    Threading.Thread.Sleep(2000)  'sleep 2 seconds then start another thread
                Next
            Catch ex As Exception
                If InStr(ex.ToString, "System.OutOfMemoryException") > 0 Then
                    WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " VitalSigns is out of memory.  Attempting to exit so the Master service will restart it. ")
                    Thread.Sleep(1000)
                    End
                Else
                    WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " Error starting MonitorURL thread " & ex.ToString, LogLevel.Normal)
                End If

            End Try


        Catch ex As Exception

        End Try


        Try
            Dim EnabledCloudAppsCount As Integer = 0
            Dim CloudOne As MonitoredItems.Cloud
            For n = 0 To MyClouds.Count - 1
                CloudOne = MyClouds.Item(n)
                If CloudOne.Enabled = True Then EnabledCloudAppsCount += 1
            Next n
            Dim intThreadCount As Integer = CInt(EnabledCloudAppsCount / 4)
            If intThreadCount <= 1 Then
                intThreadCount = 2
            End If

            WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " There are " & EnabledCloudAppsCount & " enabled Cloud Apps to scan.")
            WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " I am launching " & intThreadCount & " threads to scan these Cloud Apps.")

            Try
                For n = 1 To intThreadCount
                    Dim tTemp As Threading.Thread = New Threading.Thread(AddressOf MonitorCloud)
                    tTemp.CurrentCulture = New CultureInfo(sCultureString)
                    tTemp.Start()
                    ListOfCloudThreads.Add(tTemp)
                    Threading.Thread.Sleep(2000)  'sleep 2 seconds then start another thread
                Next
            Catch ex As Exception
                If InStr(ex.ToString, "System.OutOfMemoryException") > 0 Then
                    WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " VitalSigns is out of memory.  Attempting to exit so the Master service will restart it. ")
                    Thread.Sleep(1000)
                    End
                Else
                    WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " Error starting Monitor Cloud thread " & ex.ToString, LogLevel.Normal)
                End If

            End Try


        Catch ex As Exception

        End Try

        StartWebSphereThreads()

        StartIBMConnectThreads()

        Try
            Dim MonitorCoreTableChanges As New Thread(AddressOf CheckForCoreTableChanges)
            MonitorCoreTableChanges.CurrentCulture = New CultureInfo(sCultureString)
            MonitorCoreTableChanges.Start()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error starting thread to check for table updates")
        End Try



        Try
            WriteAuditEntry(Now.ToString & " Now starting the Hourly Task checker...")
            ThreadHourlyDaily.IsBackground = True
            ThreadHourlyDaily.Priority = ThreadPriority.Normal
            ThreadHourlyDaily.CurrentCulture = New CultureInfo(sCultureString)
            ThreadHourlyDaily.Start()
        Catch ex As Exception

        End Try



        'Try
        '    WriteAuditEntry(Now.ToString & " Now starting the thread checker...")
        '    CheckThreads.IsBackground = True
        '    CheckThreads.Priority = ThreadPriority.Normal
        '    CheckThreads.Start()
        'Catch ex As Exception

        'End Try
    End Sub

    Protected Sub StartSametimeThreads()
        WriteAuditEntry(Now.ToString & " Starting Sametime Servers monitoring thread...")
        ThreadSametimeServers.IsBackground = True
        ThreadSametimeServers.Priority = ThreadPriority.Normal
        ThreadSametimeServers.CurrentCulture = New CultureInfo(sCultureString)
        ThreadSametimeServers.Start()
    End Sub

    Protected Sub StartBlackBerryThreads()
        Dim myRegistry As New VSFramework.RegistryHandler()
        WriteAuditEntry(Now.ToString & " Starting BlackBerry Server monitoring...")

        Dim intThreadCount As Integer = CInt(MyBlackBerryServers.Count / 3)
        If intThreadCount <= 1 Then
            intThreadCount = 2
        End If

        Dim threadCountOverride As Integer = 0
        Try
            threadCountOverride = myRegistry.ReadFromRegistry("ThreadCount")
        Catch ex As Exception
            threadCountOverride = 0
        End Try

        If threadCountOverride > 10 Then
            intThreadCount = threadCountOverride
            WriteAuditEntry(Now.ToString & " I am launching " & intThreadCount & " BES threads because of configuration setting 'ThreadCount'.")
        End If

        'Don't launch more than 35 threads because you might run out of sockets
        If intThreadCount > 34 Then
            intThreadCount = 35
        End If

        Try
            For n = 1 To intThreadCount
                Dim tTemp As Threading.Thread = New Threading.Thread(AddressOf MonitorBlackBerryServers)
                tTemp.CurrentCulture = New CultureInfo(sCultureString)
                tTemp.Start()
                'ListOfDominoThreads.Add(tTemp)
                Threading.Thread.Sleep(2000)  'sleep 2 seconds then start another thread
            Next
        Catch ex As Exception
            If InStr(ex.ToString, "System.OutOfMemoryException") > 0 Then
                WriteAuditEntry(Now.ToString & " VitalSigns is out of memory.  Attempting to exit so the Master service will restart it. ")
                Thread.Sleep(1000)
                End
            Else
                WriteAuditEntry(Now.ToString & " Error starting MonitorBlackBerryServers thread " & ex.ToString)
            End If

        End Try
        ThreadBlackBerryServers.CurrentCulture = New CultureInfo(sCultureString)
        ThreadBlackBerryServers.Start()
    End Sub

    Protected Sub StartMailServicesThread()
        WriteAuditEntry(Now.ToString & " Starting Mail Services monitor thread...")
        ThreadMainMailServices.IsBackground = True
        ThreadMainMailServices.Priority = ThreadPriority.Normal
        ThreadMainMailServices.CurrentCulture = New CultureInfo(sCultureString)
        ThreadMainMailServices.Start()
    End Sub

    Protected Sub StartWebSphereThreads()
        If (boolMonitorWebSphere = True And MyWebSphereServers.Count > 0) Then
            If (Not ThreadWebSphereServers.IsAlive()) Then
                WriteAuditEntry(Now.ToString & " Starting WebSphere Servers monitoring thread...")
                ThreadWebSphereServers.IsBackground = True
                ThreadWebSphereServers.Priority = ThreadPriority.Normal
                ThreadWebSphereServers.CurrentCulture = New CultureInfo(sCultureString)
                ThreadWebSphereServers.Start()
            End If
        Else
            WriteAuditEntry(Now.ToString & " WebSphere monitoring is DISABLED...")
        End If
    End Sub

    Protected Sub StartIBMConnectThreads()
        If (boolMonitorIBMConnect = True And MyIBMConnectServers.Count > 0) Then
            If (Not ThreadIBMConnectServers.IsAlive()) Then
                WriteAuditEntry(Now.ToString & " Starting IBM Connect Servers monitoring thread...")
                ThreadIBMConnectServers.IsBackground = True
                ThreadIBMConnectServers.Priority = ThreadPriority.Normal
                ThreadIBMConnectServers.CurrentCulture = New CultureInfo(sCultureString)
                ThreadIBMConnectServers.Start()
            End If
        Else
            WriteAuditEntry(Now.ToString & " IBM Connect monitoring is DISABLED...")
        End If
    End Sub


#Region "Maintenance Windows and Business Hours"

    Public Function InMaintenance(ByVal DeviceType As String, ByVal DeviceName As String) As Boolean
        Dim MaintDLL As New MaintenanceDLL.MaintenanceDll
        Try
            If MaintDLL.InMaintenance(DeviceType, DeviceName) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function OffHours(ByVal ServerName As String) As Boolean
        'This function returns true if called during off hours, false if called during business hours
        Dim MaintDLL As New MaintenanceDLL.MaintenanceDll
        Try
            If MaintDLL.OffHours(ServerName) = True Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function


#End Region


#Region "Week Number"

    Private Function GetWeekNumber(ByVal dtNow As DateTime) As Integer
        Return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday)
    End Function


    Private Function GetWeekOneDate(ByVal Year As Integer) As DateTime
        'start with the 4th of Jan for current year
        Dim MyDate As New DateTime(Year, 1, 4)
        ' Get the ISO-8601 day number for this date 1==Monday, 7==Sunday
        Dim DayNum As Integer = MyDate.DayOfWeek  '0=Sunday, 6=Sat
        If DayNum = 0 Then
            DayNum = 7
        End If

        ' Return the date of the Monday that is less than or equal to this date
        Return MyDate.AddDays(1 - DayNum)

    End Function

#End Region

#Region "Log Files"



    Private Overloads Sub WriteAuditEntry(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
        MyBase.WriteAuditEntry(strMsg, "VSCore.txt", LogLevelInput)
    End Sub
    Private Overloads Sub WriteAuditEntryWebSphere(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
        MyBase.WriteAuditEntry(strMsg, "VSWebSpehere.txt", LogLevelInput)
    End Sub

    Private Overloads Sub WriteAuditEntryIBMConnect(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
        MyBase.WriteAuditEntry(strMsg, "VSIBMConnect.txt", LogLevelInput)
    End Sub


    'Private Sub WriteAuditEntry(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
    '    If LogLevelInput >= MyLogLevel Then
    '        strAuditText += strMsg & vbCrLf
    '    End If
    'End Sub

    'Private Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
    '    Dim DeviceLogDestination As String
    '    Dim appendMode As Boolean = True
    '    '   If Left(strAppPath, 1) = "\" Then
    '    'DeviceLogDestination = strAppPath & "Data\Logfiles\" & DeviceType & "_" & DeviceName & "_Log.txt"
    '    '  Else
    '    '    DeviceLogDestination = strAppPath & "\Data\Logfiles\" & DeviceType & "_" & DeviceName & "_Log.txt"
    '    '    End If

    '    If (LogLevelInput < MyLogLevel) Then
    '        Return
    '    End If

    '    Try
    '        DeviceLogDestination = AppDomain.CurrentDomain.BaseDirectory.ToString & "\Log_Files\" & DeviceType & "_" & DeviceName & "_Log.txt"
    '        If InStr(DeviceLogDestination, "/") > 0 Then
    '            DeviceLogDestination = DeviceLogDestination.Replace("/", "_")
    '        End If
    '    Catch ex As Exception

    '    End Try

    '    Try
    '        Dim sw As New StreamWriter(DeviceLogDestination, appendMode, System.Text.Encoding.Unicode)
    '        sw.WriteLine(strMsg)
    '        sw.Close()
    '        sw = Nothing
    '    Catch ex As Exception

    '    End Try
    '    GC.Collect()
    'End Sub

    'Protected Sub WriteAuditEntries()

    '    Dim strLogDest As String
    '    Try
    '        strLogDest = AppDomain.CurrentDomain.BaseDirectory.ToString & "\Log_Files\VSCore.txt"
    '    Catch ex As Exception
    '        strLogDest = "d:\vitalsigns\data\VSCore.txt"
    '    End Try


    '    'Try
    '    '    ' strLogDest = myRegistry.ReadFromRegistry("History Path")
    '    '    If File.Exists(strLogDest) Then
    '    '        File.Delete(strLogDest)
    '    '    End If
    '    'Catch ex As Exception
    '    '    strLogDest = "c:\VSCore.txt"

    '    'End Try

    '    ' myRegistry = Nothing

    '    'Dim elapsed As TimeSpan
    '    Dim OneHour As Integer = 60 * 60 * -1  'seconds
    '    Dim dtAuditHistory As DateTime

    '    Dim myLastUpdate As TimeSpan

    '    Try
    '        dtAuditHistory = Now
    '        Thread.CurrentThread.Sleep(1000)
    '    Catch ex As Exception

    '    End Try


    '    Try

    '        Do
    '            Dim sw As StreamWriter
    '            Try
    '                If File.Exists(strLogDest) Then
    '                    sw = New StreamWriter(strLogDest, True, System.Text.Encoding.Unicode)
    '                Else
    '                    sw = New StreamWriter(strLogDest, False, System.Text.Encoding.Unicode)
    '                    '  sw = File.CreateText(strLogDest)
    '                End If
    '            Catch ex As Exception

    '            End Try


    '            Try
    '                If strAuditText <> "" Then

    '                    sw.WriteLine(strAuditText)
    '                    strAuditText = ""
    '                    sw.Flush()
    '                    sw.Close()
    '                End If

    '            Catch ex As Exception

    '            End Try

    '            Try
    '                sw.Close()
    '            Catch ex As Exception

    '            End Try
    '            sw = Nothing

    '            Thread.CurrentThread.Sleep(4250)

    '            Try
    '                myLastUpdate = dtAuditHistory.Subtract(Now)
    '            Catch ExAbort As ThreadAbortException

    '            Catch ex As Exception

    '            End Try


    '        Loop

    '    Catch ExAbort As ThreadAbortException
    '        '      sw.Write(" Shutting down, closing log")
    '        '     sw.Close()
    '        '    sw = Nothing
    '    Catch ex As Exception

    '    End Try


    'End Sub
#End Region


#Region "Hourly and Daily"

    Public Sub HourlyTasks()
        If MyLogLevel = LogLevel.Verbose Then
            WriteAuditEntry(Now.ToString & " Beginning to perform Hourly Tasks.")
        End If
        Dim myRegistry As New VSFramework.RegistryHandler

        If MyLogLevel = LogLevel.Verbose Then
            WriteAuditEntry(Now.ToString & " Updated Hourly Tasks for Network Devices.")
        End If

        For Each ST As MonitoredItems.SametimeServer In mySametimeServers

            Try

                RecordCountAvailability("Sametime", ST.Name, ST.UpPercentCount * 100)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error recording count availability: " & ex.Message)
            End Try

            Try
                RecordTimeAvailability("Sametime", ST.Name, ST.UpPercentMinutes * 100)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error recording time availability: " & ex.Message)
            End Try

            Try
                RecordDownTime("Sametime", ST.Name, ST.DownMinutes)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error recording down minutes: " & ex.Message)
            End Try

            Try
                ST.ResetUpandDownCounts()

            Catch ex As Exception

            End Try
        Next


        Try
            For Each URL In MyURLs
                If URL.Enabled = True Then
                    Try
                        RecordTimeAvailability("URL", URL.Name, URL.UpPercentMinutes * 100)
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error recording time availability: " & ex.Message)
                    End Try
                End If

            Next
        Catch ex As Exception

        End Try


        Try


            myRegistry.WriteToRegistry("Core Hourly Tasks", Now.Hour)
        Catch ex As Exception

        End Try

        Try
            If MyLogLevel = LogLevel.Verbose Then
                WriteAuditEntry(Now.ToString & " Updated Hourly Tasks registry setting.")
            End If
        Catch ex As Exception

        End Try

        'Sowjanya: VSPLUS-1704 : 30-Apr-15; 01-May-15: Mukund
        Dim strNodeName As String = ""
        Try
            strNodeName = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString
        Catch ex As Exception
            strNodeName = ""
        End Try
        Try
            ReadXmlFile(strNodeName)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in ReadXML: " & ex.Message)
        End Try
        ShrinkDBLogOnHourlyBasis()
        Try
            myRegistry = Nothing
            GC.Collect()
        Catch ex As Exception

        End Try

        If MyLogLevel = LogLevel.Verbose Then
            WriteAuditEntry(Now.ToString & " Finished Hourly Tasks.")
        End If

    End Sub

    'Sowjanya: VSPLUS-1704 : 30-Apr-15; 01-May-15: Mukund
    Public Sub GetAssemblyInfo()
        'gets assembly information of Files & updates database
        Dim myAdapter As New VSFramework.VSAdaptor
        Dim dt As DataTable = New DataTable
        Dim DsSettings As New Data.DataSet
        Dim strSQL As String
        Dim Svalue As String = ""
        Dim build As String

        'Get Assembly names from the table 
        Try
            strSQL = "Select * from VS_AssemblyVersionInfo"
            myAdapter.FillDatasetAny("VitalSigns", "None", strSQL, DsSettings, "VS_AssemblyVersionInfo")
            dt = DsSettings.Tables("VS_AssemblyVersionInfo")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & ", GetAssemblyInfo:Error in fetching VS_AssemblyVersionInfo data : " & ex.Message)

        End Try

        Dim FilePath As String = ""
        Dim i As Integer = 0

        Do While (i < dt.Rows.Count)
            Try
                If (dt.Rows(i)("FileArea").ToString = "VSWebUI") Then
                    Try
                        strSQL = "Select svalue from Settings Where sname='VSWebPath'"
                        myAdapter.FillDatasetAny("VitalSigns", "None", strSQL, DsSettings, "Settings")
                        myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)
                        Svalue = DsSettings.Tables("Settings").Rows(0)("svalue").ToString()
                    Catch ex As Exception
                        Svalue = "0"
                    End Try
                    FilePath = Svalue
                ElseIf (dt.Rows(i)("FileArea").ToString = "Services") Then
                    Try
                        strSQL = "Select svalue from Settings Where sname='Log Files Path'"
                        myAdapter.FillDatasetAny("VitalSigns", "None", strSQL, DsSettings, "Settings")
                        myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)
                        Svalue = DsSettings.Tables("Settings").Rows(0)("svalue").ToString()
                    Catch ex As Exception
                        Svalue = "0"
                    End Try
                    FilePath = strSQL.ToUpper.Substring(0, strSQL.ToUpper.IndexOf("LOG_FILES"))
                End If
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & ", GetAssemblyInfo: Error in fetching Settings data : " & ex.Message)

            End Try
            If Svalue <> "0" Then
                Dim ver As Version = AssemblyName.GetAssemblyName((FilePath + dt.Rows(i)("AssemblyName").ToString)).Version
                Dim sVer As String = ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString()

                Dim fvi As FileVersionInfo = FileVersionInfo.GetVersionInfo((FilePath + dt.Rows(i)("AssemblyName").ToString))
                Dim fversion As String = fvi.ProductMajorPart.ToString() + "." + fvi.ProductMinorPart.ToString() + "." + fvi.ProductBuildPart.ToString()
                dt.Rows(i)("ProductVersion") = fversion
                build = System.IO.File.GetLastWriteTime((FilePath + dt.Rows(i)("AssemblyName").ToString))
                Try
                    strSQL = "Update VS_AssemblyVersionInfo set AssemblyVersion='" & sVer.ToString & "', ProductVersion='" & fversion & "',BuildDate='" & build & _
                    "' where  AssemblyName='" & dt.Rows(i)("AssemblyName").ToString & "'"

                    myAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & ", GetAssemblyInfo: Error in fetching File information : " & ex.Message)

                End Try

            End If
            i = (i + 1)
        Loop
    End Sub

    Protected Sub PerformHourlyDailyTasks()
        'performs operations that need to be done hourly or daily, such as summarize statistics, and look at mail files again
        Dim myRegistry As New VSFramework.RegistryHandler
        Dim LastDate, MyDate As DateTime
        Dim Hour As Integer
        Dim Day As Integer = -1

        Dim DominoServer As MonitoredItems.DominoServer

        Dim NMP As MonitoredItems.DominoMailProbe

        Dim NDB As MonitoredItems.NotesDatabase
        Dim Q As MonitoredItems.BlackBerryQueue


        Do While True
            Thread.Sleep(60 * 1000) 'Sleep a minute

            'Figure Out if We have shifted to off hours

            Try


                For Each server In MyBlackBerryServers
                    server.OffHours = OffHours(server.Name)
                Next
                For Each server In mySametimeServers
                    server.OffHours = OffHours(server.Name)
                Next

                For Each NMP In MyNotesMailProbes
                    NMP.OffHours = OffHours(NMP.Name)
                Next



            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error in Daily Tasks thread setting off hours: " & ex.Message)
            End Try
            If MyLogLevel = LogLevel.Verbose Then
                WriteAuditEntry(Now.ToString & " *********** Evaluating Hourly Tasks")
            End If
            'Figure out if hourly tasks are due
            Try
                Hour = CType(myRegistry.ReadFromRegistry("Core Hourly Tasks"), Integer)
            Catch ex As Exception
                Hour = -1
            End Try


            Try
                If Now.Hour <> Hour Then
                    WriteAuditEntry(Now.ToString & " Calling Hourly Tasks")
                    HourlyTasks()
                Else
                    WriteAuditEntry(Now.ToString & " Hourly tasks performed for this hour: " & Hour & ",")
                End If
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error in Daily Tasks thread starting hourly tasks: " & ex.Message)
            End Try

            Try

                If Now.Day <> Day Then
                    Try
                        WriteAuditEntry(Now.ToString & " Now starting the Daily Task...")
                        Dim ThreadDaily As New Thread(AddressOf DailyTasks)
                        ThreadDaily.IsBackground = True
                        ThreadDaily.Priority = ThreadPriority.Normal
                        ThreadDaily.CurrentCulture = New CultureInfo(sCultureString)
                        ThreadDaily.Start()
                    Catch ex As Exception

                    End Try

                    Day = Now.Day

                End If

            Catch ex As Exception

            End Try
            


            MyDate = Now.ToShortDateString
            Try
                LastDate = CType(myRegistry.ReadFromRegistry("Daily Tasks Date"), DateTime)
            Catch ex As Exception
                LastDate = Now.AddDays(-2).ToShortDateString
            End Try

            Try
                BannerText = myRegistry.ReadFromRegistry("BannerText")
                If BannerText = "" Then BannerText = ProductName & " Status Report"
            Catch ex As Exception
                BannerText = ProductName & " Status Report"
            End Try

            If MyLogLevel = LogLevel.Verbose Then
                WriteAuditEntry(Now.ToString & " Finished performing check for hourly tasks.")
            End If

        Loop
    End Sub

    Public Sub DailyTasks()

        Try

            Dim myRegistry As New VSFramework.RegistryHandler()
            Dim LastCoreDaily As String
            Try
                LastCoreDaily = myRegistry.ReadFromRegistry("LastCoreDaily")
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error getting last date Daily Tasks ran.", LogLevel.Normal)
            End Try

            If LastCoreDaily IsNot Nothing Then
                If LastCoreDaily <> "" Then
                    Dim lastUpdateDateTime As DateTime = DateTime.Parse(LastCoreDaily)
                    If lastUpdateDateTime.Date = Now.Date Then
                        WriteAuditEntry(Now.ToString & " Already performed Daily Tasks today.", LogLevel.Verbose)
                        Exit Sub
                    End If
                End If
            End If





            WriteAuditEntry(Now.ToString & " Beginning to perform Daily Tasks.", LogLevel.Verbose)

            For Each server As MonitoredItems.IBMConnect In MyIBMConnectServers
                WriteAuditEntry(Now.ToString & " Beginning to perform daily Tasks for IBM Connections server " & server.Name, LogLevel.Verbose)
                GetConnectionsStats(server)
            Next


            For Each server As MonitoredItems.SametimeServer In mySametimeServers
                WriteAuditEntry(Now.ToString & " Beginning to perform daily Tasks for Sametime server " & server.Name, LogLevel.Verbose)
                If server.WsScanMeetingServer Then
                    GetSametimeMeetingStatsFromDB(server)
                End If

            Next


            myRegistry.WriteToRegistry("LastCoreDaily", Now.ToString())

            WriteAuditEntry(Now.ToString & " Done with Daily Tasks.", LogLevel.Verbose)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in Daily Tasks. Error : " & ex.Message, LogLevel.Normal)
        End Try



    End Sub

#End Region

#Region "Threads"

    Protected Sub MonitorMailServices()
        '  Dim n As Long
        Dim start, done As Long
        Dim elapsed As TimeSpan
        Dim myRegistry As New RegistryHandler

        Do While boolTimeToStop <> True
            Dim threadMonitorMailServices As New Thread(AddressOf MonitorMailService)
            Try
                WriteAuditEntry(Now.ToString & " Creating new Mail Service monitoring thread.")
                threadMonitorMailServices.CurrentCulture = New CultureInfo(sCultureString)
                threadMonitorMailServices.Start()
                Thread.Sleep(1000) 'sleep 1 seconds to give thread a chance to start
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error starting Mail Services Thread: " & ex.Message)
            End Try

            Do
                Dim myLastUpdate As TimeSpan
                Try
                    myLastUpdate = dtMailServicesLastUpdate.Subtract(Now)
                    '       WriteAuditEntry(Now.ToString & " Mail Services thread last updated " & myLastUpdate.TotalSeconds & " seconds ago.")
                    If myLastUpdate.TotalSeconds > 89 Then
                        threadMonitorMailServices.Abort()
                        threadMonitorMailServices.Join(2000)
                        threadMonitorMailServices = Nothing
                        dtMailServicesLastUpdate = Now
                        WriteAuditEntry(Now.ToString & " Destroyed Mail Service thread.")
                    End If
                Catch ex As ThreadAbortException
                    WriteAuditEntry(Now.ToString & " Shutting down Mail Service thread.")
                    threadMonitorMailServices.Abort()
                    threadMonitorMailServices.Join(1000)
                    threadMonitorMailServices = Nothing
                    Exit Sub
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Error #1 in Mail Services Thread: " & ex.Message)
                End Try

                Try
                    done = Now.Ticks
                    elapsed = New TimeSpan(done - start)

                    If elapsed.TotalMinutes > 5 Then
                        start = Now.Ticks
                        Try
                            'threadMonitorMailServices.Suspend()
                            CreateMailServicesCollection()

                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error in Mail Services loop: " & ex.Message)
                        End Try
                    End If
                    Thread.Sleep(5000)
                Catch ex As ThreadAbortException
                    WriteAuditEntry(Now.ToString & " Shutting down Mail Service thread.")
                    threadMonitorMailServices.Abort()
                    threadMonitorMailServices.Join(1000)
                    threadMonitorMailServices = Nothing
                    myRegistry = Nothing
                    Exit Sub
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Error #2 in Mail Services Thread: " & ex.Message)
                End Try


            Loop

        Loop

    End Sub


    Protected Sub CheckForCoreTableChanges()


        Dim myRegistry As New VSFramework.RegistryHandler
        Dim NodeName As String = ""
        '********** Refresh Collection of Devices and Monitor settings

        Do Until boolTimeToStop = True
            Try
                'Update the settings anytime the registry indicates a change has been made
                If (NodeName = "") Then
                    If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then
                        NodeName = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
                    End If
                End If
                If NodeName <> "" Then
                    If UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.Sametime, NodeName) Then
                        Try
                            '' MonitorDominoSettings()
                            WriteAuditEntry(Now.ToString & " Refreshing configuration of Sametime servers")
                            CreateSametimeServersCollection()
                            WriteAuditEntry(Now.ToString & " Refreshing Status Table for Sametime")
                            UpdateStatusTableWithSametime()

                            'If the thread is not started, start it
                            If (boolMonitorSametime = True And mySametimeServers.Count > 0 And Not ThreadSametimeServers.IsAlive) Then
                                WriteAuditEntry(Now.ToString & " Sametime Thread was not started so will start it now.")
                                StartSametimeThreads()
                            End If
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error Refreshing Sametime server settings on demand: " & ex.Message)
                        End Try
                    End If

                    Try
                        'Update the settings anytime the registry indicates a change has been made
                        If UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.BES, NodeName) Then
                            Try
                                '' MonitorDominoSettings()
                                WriteAuditEntry(Now.ToString & " Refreshing configuration of BlackBerry Servers")
                                CreateBlackBerryServersCollection()
                                WriteAuditEntry(Now.ToString & " Refreshing Status Table for BlackBerry Servers")
                                UpdateStatusTableWithBlackBerryServers()

                                'If the thread is not started, start it
                                If (boolMonitorBlackBerry = True And MyBlackBerryServers.Count > 0 And Not ThreadBlackBerryServers.IsAlive) Then
                                    WriteAuditEntry(Now.ToString & " BlackBerry Servers Thread was not started so will start it now.")
                                    StartBlackBerryThreads()
                                End If
                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " Error Refreshing BlackBerry Servers settings on demand: " & ex.Message)
                            End Try
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        'Update the settings anytime the registry indicates a change has been made
                        If UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.Mail, NodeName) Then
                            Try
                                '' MonitorDominoSettings()
                                WriteAuditEntry(Now.ToString & " Refreshing configuration of Mail Services")
                                CreateMailServicesCollection()
                                WriteAuditEntry(Now.ToString & " Refreshing Status Table for Mail Services")
                                UpdateStatusTableWithMailServices()

                                'If the thread is not started, start it 
                                If (boolMonitorMailServices = True And MyMailServices.Count > 0 And Not ThreadMainMailServices.IsAlive) Then
                                    WriteAuditEntry(Now.ToString & " Mail Services Thread was not started so will start it now.")
                                    StartMailServicesThread()
                                End If
                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " Error Refreshing Mail Services settings on demand: " & ex.Message)
                            End Try
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        'Update the settings anytime the registry indicates a change has been made
                        If UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.URL, NodeName) Then
                            Try
                                '' MonitorDominoSettings()
                                WriteAuditEntry(Now.ToString & " Refreshing configuration of URLs")
                                CreateURLCollection()
                                WriteAuditEntry(Now.ToString & " Refreshing Status Table for URLs")
                                UpdateStatusTableWithURLs()
                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " Error Refreshing URLs settings on demand: " & ex.Message)
                            End Try
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        'Update the settings anytime the registry indicates a change has been made
                        If UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.WebSphere, NodeName) Then
                            Try
                                '' MonitorDominoSettings()
                                WriteAuditEntry(Now.ToString & " Refreshing configuration of WebSphere")
                                CreateWebSphereCollection()
                                WriteAuditEntry(Now.ToString & " Refreshing Status Table for WebSphere")
                                UpdateStatusTableWithWebSphere()
                                StartWebSphereThreads()
                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " Error Refreshing WebSphere settings on demand: " & ex.Message)
                            End Try
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        'Update the settings anytime the registry indicates a change has been made
                        If UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.Cloud, NodeName) Then
                            Try
                                '' MonitorDominoSettings()
                                WriteAuditEntry(Now.ToString & " Refreshing configuration of Cloud Applications")
                                CreateCloudCollection()
                                WriteAuditEntry(Now.ToString & " Refreshing Status Table for  Cloud Applications")
                                UpdateStatusTableWithCloudURLs()
                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " Error Refreshing Cloud settings on demand: " & ex.Message)
                            End Try
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        'Update the settings anytime the registry indicates a change has been made
                        If UpdateServiceCollection(VSNext.Mongo.Entities.Enums.ServerType.IBMConnections, NodeName) Then
                            Try
                                '' MonitorDominoSettings()
                                WriteAuditEntry(Now.ToString & " Refreshing configuration of IBM Connections")
                                CreateIBMConnectCollection()
                                WriteAuditEntry(Now.ToString & " Refreshing Status Table for IBM Connections")
                                UpdateStatusTableWithIbmConnections()
                                StartIBMConnectThreads()
                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " Error Refreshing IBM Connections settings on demand: " & ex.Message)
                            End Try
                        End If
                    Catch ex As Exception

                    End Try

                End If
            Catch ex As Exception

            End Try
            Thread.Sleep(2000)
        Loop
    End Sub


    Protected Sub MonitorSametimeThread()
        Dim elapsed As TimeSpan
        Dim start, done As Long
        start = Now.Ticks
        Dim myRegistry As New RegistryHandler

        Do While boolTimeToStop <> True
            WriteAuditEntry(Now.ToString & " Creating new Sametime monitoring thread.")
            Dim threadMonitorSametime As New Thread(AddressOf MonitorSametime)
            Try
                threadMonitorSametime.CurrentCulture = New CultureInfo(sCultureString)
                threadMonitorSametime.Start()
                Thread.Sleep(4000) 'sleep 4 seconds to give thread a chance to start
                Do
                    Dim myLastUpdate As TimeSpan
                    Try


                        myLastUpdate = Now.Subtract(dtSametimeLastUpdate)
                        '   WriteAuditEntry(Now.ToString & " ST thread last updated " & myLastUpdate.TotalSeconds & " seconds ago.")
                        If myLastUpdate.TotalSeconds >= 600 Then
                            'The ST Thread hasn't looped in 180 seconds
                            WriteAuditEntry(Now.ToString & " Destroying Sametime thread because it hasn't updated in " & myLastUpdate.TotalSeconds & " seconds. ")

                            If Not (SametimeProcess Is Nothing) And (Not (SametimeProcess Is Nothing) And Not (SametimeProcess.HasExited)) Then
                                WriteAuditEntry(Now.ToString & " Destroying Sametime java process before destroying thread. ")
                                SametimeProcess.Kill()
                            End If

                            threadMonitorSametime.Abort()
                            threadMonitorSametime.Join(5000)
                            dtSametimeLastUpdate = Now
                            threadMonitorSametime = Nothing
                            Exit Do
                        End If
                        done = Now.Ticks
                        elapsed = New TimeSpan(done - start)

                        If elapsed.TotalMinutes > 5 Then
                            start = Now.Ticks
                            WriteAuditEntry(Now.ToString & " Refreshing configuration of Sametime servers to check for changes.")
                            Try

                                CreateSametimeServersCollection()
                                UpdateStatusTableWithSametime()
                                'myRegistry.WriteToRegistry("Sametime Server Update", False)
                            Catch ex As Exception

                            End Try

                            Try

                            Catch ex As Exception

                            End Try

                        End If


                    Catch ex As ThreadAbortException
                        WriteAuditEntry(Now.ToString & " Destroying Sametime Server monitor thread to shut down service. ")
                        threadMonitorSametime.Abort()
                        threadMonitorSametime.Join(5000)
                        threadMonitorSametime = Nothing
                        Exit Sub
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Sametime Thread #1 error: " & ex.Message)
                    End Try
                    Thread.Sleep(2900)
                    '  Thread.Sleep(29000)
                Loop
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Sametime Thread #2 error: " & ex.Message)
            End Try


        Loop

    End Sub

    Protected Sub MonitorWebSpehereThread()
        Dim elapsed As TimeSpan
        Dim start, done As Long
        start = Now.Ticks
        Dim myRegistry As New RegistryHandler

        Do While boolTimeToStop <> True
            WriteAuditEntry(Now.ToString & " Creating new WebSphere monitoring thread.")
            Dim threadMonitorWebSphere As New Thread(AddressOf MonitorWebSphere)
            Try
                threadMonitorWebSphere.CurrentCulture = New CultureInfo(sCultureString)
                threadMonitorWebSphere.Start()
                Thread.Sleep(4000) 'sleep 4 seconds to give thread a chance to start
                Do
                    Dim myLastUpdate As TimeSpan

                    Try

                        myLastUpdate = dtWebSphereLastUpdate.Subtract(Now)
                        '   WriteAuditEntry(Now.ToString & " ST thread last updated " & myLastUpdate.TotalSeconds & " seconds ago.")
                        If myLastUpdate.TotalSeconds < -180 Then
                            'The ST Thread hasn't looped in 180 seconds
                            WriteAuditEntry(Now.ToString & " Destroying Sametime thread because it hasn't updated in " & myLastUpdate.TotalSeconds & " seconds. ")
                            threadMonitorWebSphere.Abort()
                            threadMonitorWebSphere.Join(5000)
                            dtWebSphereLastUpdate = Now
                            threadMonitorWebSphere = Nothing
                            Exit Do
                        End If

                        done = Now.Ticks
                        elapsed = New TimeSpan(done - start)

                        If elapsed.TotalMinutes > 5 Then
                            start = Now.Ticks
                            WriteAuditEntry(Now.ToString & " Refreshing configuration of WebSphere servers to check for changes.")
                            Try

                                CreateWebSphereCollection()
                                UpdateStatusTableWithWebSphere()
                                'myRegistry.WriteToRegistry("Sametime Server Update", False)
                            Catch ex As Exception

                            End Try

                            Try

                            Catch ex As Exception

                            End Try

                        End If


                    Catch ex As ThreadAbortException
                        WriteAuditEntry(Now.ToString & " Destroying WebSphere Server monitor thread to shut down service. ")
                        threadMonitorWebSphere.Abort()
                        threadMonitorWebSphere.Join(5000)
                        threadMonitorWebSphere = Nothing
                        Exit Sub
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " WebSphere Thread #1 error: " & ex.Message)
                    End Try
                    Thread.Sleep(2900)
                    '  Thread.Sleep(29000)
                Loop
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " WebSphere Thread #2 error: " & ex.Message)
            End Try


        Loop

    End Sub

    Protected Sub ThreadChecker()
        'watches over the threads
        Dim SleepTime As Integer
        SleepTime = 60 * 1000 * 3  '3 minutes worth of milliseconds; sleeps 3 minutes after doing its work
        Static Restart As Boolean = False

        Dim myRegistry As New VSFramework.RegistryHandler

        Do While True

            Try
                MyLogLevel = myRegistry.ReadFromRegistry("Log Level")
            Catch ex As Exception
                MyLogLevel = LogLevel.Normal
            End Try

            Try
                If MyLogLevel = LogLevel.Verbose Then
                    WriteAuditEntry(Now.ToString & "********* Thread Status Report ********")
                    '  WriteAuditEntry(Now.ToString & " Master Domino thread: " & MasterDominoThread.ThreadState.ToString)
                    WriteAuditEntry(Now.ToString & " Hourly tasks thread: " & ThreadHourlyDaily.ThreadState.ToString)
                End If
            Catch ex As Exception

            End Try


            Dim i As Integer = 1
            For Each t As Threading.Thread In ListOfURLThreads
                WriteAuditEntry(Now.ToString & " URL monitoring thread #" & i.ToString & " state = " & t.ThreadState.ToString)
                i += 1
            Next


            Try
                Thread.Sleep(SleepTime)
            Catch ex As Exception

            End Try

        Loop
        myRegistry = Nothing
    End Sub

#End Region

#Region "Device Monitoring"

#Region "Mail Services"

    Private Sub MonitorMailService()  'This is the main sub that calls all the other ones
        Try
            Dim MyMailService As MonitoredItems.MailService
            Do While boolTimeToStop <> True
                'If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " %%% Begin Loop for Mail Service Monitoring %%%")
                If MyMailServices.Count > 0 Then
                    MyMailService = SelectMailServiceToMonitor()
                    If Not MyMailService Is Nothing Then
                        QueryMailService(MyMailService)
                        MyMailService.LastScan = Date.Now
                    End If
                    Thread.Sleep(2500)
                End If

                If MyMailServices.Count = 0 Then
                    Thread.Sleep(60000)
                End If

                If MyMailServices.Count < 24 Then
                    Thread.Sleep(2500)
                End If

                If MyMailServices.Count < 10 Then
                    Thread.Sleep(10000)
                End If

                dtMailServicesLastUpdate = Now
                ' If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " %%% End Loop for Mail Service Monitoring %%%")
            Loop
        Catch ex As ThreadAbortException
            WriteAuditEntry(Now.ToString & " Mail Service thread shutting down...")
            Thread.CurrentThread.Abort()
            Exit Sub
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " %%% Mail Service Monitoring Error: " & ex.Message)
        End Try

    End Sub

    Private Function SelectMailServiceToMonitor() As MonitoredItems.MailService
        ' If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " >>> Selecting a Mail Service for monitoring >>>>")
        Dim tNow As DateTime
        tNow = Now
        Dim tScheduled As DateTime

        Dim timeOne, timeTwo As DateTime

        Dim myDevice As MonitoredItems.MailService
        Dim SelectedServer As MonitoredItems.MailService

        Dim ServerOne As MonitoredItems.MailService
        Dim ServerTwo As MonitoredItems.MailService

        Dim myRegistry As New RegistryHandler

        Dim n As Integer
        Dim strSQL As String = ""
        Dim ServerType As String = "MailService"
        Dim serverName As String = ""

        Try
            strSQL = "Select svalue from ScanSettings where sname = 'Scan" & ServerType & "ASAP'"
            Dim ds As New DataSet()
            ds.Tables.Add("ScanASAP")
            Dim objVSAdaptor As New VSAdaptor
            objVSAdaptor.FillDatasetAny("VitalSigns", "VitalSigns", strSQL, ds, "ScanASAP")

            For Each row As DataRow In ds.Tables("ScanASAP").Rows
                Try
                    serverName = row(0).ToString()
                Catch ex As Exception
                    Continue For
                End Try

                For n = 0 To MyMailServices.Count - 1
                    ServerOne = MyMailServices.Item(n)

                    If ServerOne.Name = serverName And ServerOne.IsBeingScanned = False And ServerOne.Enabled Then
                        'WriteAuditEntry(Now.ToString & " >>> " & serverName & " was marked 'Scan ASAP' so that will be scanned next.")
                        strSQL = "DELETE FROM ScanSettings where sname = 'Scan" & ServerType & "ASAP' and svalue='" & serverName & "'"
                        objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)

                        Return ServerOne
                        Exit Function

                    End If
                Next
            Next

        Catch ex As Exception

        End Try

        'Any server Not Scanned should be scanned right away.  Select the first one you encounter
        For n = 0 To MyMailServices.Count - 1
            ServerOne = MyMailServices.Item(n)
            '       WriteAuditEntry(Now.ToString & " >>>  Mail Service " & ServerOne.Name & " status is " & ServerOne.Status)
            If ServerOne.Status = "Not Scanned" Or ServerOne.Status = "Master Service Stopped." Then
                '   If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " >>> Selecting Mail Service " & ServerOne.Name & " because status is " & ServerOne.Status)
                Return ServerOne
                Exit Function
            End If
        Next

        'start with the first two servers
        ServerOne = MyMailServices.Item(0)
        If MyMailServices.Count > 1 Then ServerTwo = MyMailServices.Item(1)

        'go through the remaining servers, see which one has the oldest (earliest) scheduled time
        If MyMailServices.Count > 2 Then
            Try
                For n = 2 To MyMailServices.Count - 1
                    '    WriteAuditEntry(Now.ToString & " N is " & n)
                    timeOne = CDate(ServerOne.NextScan)
                    timeTwo = CDate(ServerTwo.NextScan)
                    If DateTime.Compare(timeOne, timeTwo) < 0 Then
                        'time one is earlier than time two, so keep server 1
                        ServerTwo = MyMailServices.Item(n)
                    Else
                        'time two is later than time one, so keep server 2
                        ServerOne = MyMailServices.Item(n)
                    End If
                Next
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " >>> Error selecting a Mail Service... " & ex.Message)
            End Try
        Else
            'There were only two server, so use those going forward
        End If

        '     WriteAuditEntry(Now.ToString & " >>> Down to two services... " & ServerOne.Name & " and " & ServerTwo.Name)

        'Of the two remaining devices, pick the one with earliest scheduled time for next scan
        If Not (ServerTwo Is Nothing) Then
            timeOne = CDate(ServerOne.NextScan)
            timeTwo = CDate(ServerTwo.NextScan)

            If DateTime.Compare(timeOne, timeTwo) < 0 Then
                'time one is earlier than time two, so keep server 1
                SelectedServer = ServerOne
                tScheduled = CDate(ServerOne.NextScan)
            Else
                SelectedServer = ServerTwo
                tScheduled = CDate(ServerTwo.NextScan)
            End If
            tNow = Now
            '     WriteAuditEntry(Now.ToString & " >>> Down to one Mail Service... " & SelectedServer.Name & " to scan at " & SelectedServer.NextScan & ". Status is " & SelectedServer.Status)
        Else
            SelectedServer = ServerOne
            tScheduled = CDate(ServerOne.NextScan)
        End If
        tScheduled = CDate(SelectedServer.NextScan)
        If DateTime.Compare(tNow, tScheduled) < 0 Then
            If SelectedServer.Status <> "Not Scanned" And SelectedServer.Status <> "Disabled" Then
                If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " No Mail Service are scheduled for monitoring, next scan after " & SelectedServer.NextScan)
                SelectedServer = Nothing
            Else
                If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " selected Mail Service: " & SelectedServer.Name & " because it has not been scanned yet.")
            End If
        Else
            If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " selected Mail Service: " & SelectedServer.Name)
        End If

        Return SelectedServer
        Exit Function



    End Function

    Private Sub QueryMailService(ByRef myMailService As MonitoredItems.MailService)
        ' Dim dtLastScan As DateTime = myMailService.LastScan
        Dim strResponse, StatusDetails As String
        Dim Percent As Double = 100
        Dim strSQL As String

        WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Begin query of " & myMailService.Name)
        myMailService.Status = ""
        Try
            If myMailService.Enabled = False Then
                WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & myMailService.Name & " is disabled")
                With myMailService
                    .Status = "Disabled"
                    .Enabled = False
                    myMailService.IncrementUpCount()
                    StatusDetails = "Monitoring is disabled for this Mail Service."
                    Percent = 0
                End With
            Else
                If InMaintenance("Mail Service", myMailService.Name) = True Then
                    myMailService.Status = "Maintenance"
                    myMailService.IncrementUpCount()

                    Percent = 0
                End If
            End If


            If myMailService.Status <> "Disabled" And myMailService.Status <> "Maintenance" Then
                Try
                    Call MailServiceResponseTime(myMailService)
                Catch ex As Exception
                    myMailService.ResponseTime = 0
                    myMailService.Status = "Error"
                    myMailService.ResponseDetails = ex.ToString
                End Try

                Select Case myMailService.ResponseTime
                    Case -1
                        myMailService.Status = "Not Responding"
                        myMailService.IncrementDownCount()
                        UpdateMailServicesStatisticsTable(myMailService.Name, 0)
                        StatusDetails = myMailService.ResponseDetails
                        myMailService.AlertCondition = True
                        myMailService.AlertType = NotResponding
                        Percent = 0
                        If myMailService.FailureCount >= myMailService.FailureThreshold Then
                            myAlert.QueueAlert("Mail", myMailService.Name, "Not Responding", "A Mail Service " & myMailService.Name & " is not responding. " & myMailService.ResponseDetails, myMailService.Location)
                        End If

                        myMailService.IsUp = False

                    Case Else
                        myMailService.Status = "OK"
                        myMailService.AlertCondition = False
                        myMailService.IncrementUpCount()
                        myMailService.IsUp = True

                        Try
                            StatusDetails = "Response Time: " & myMailService.ResponseTime & " ms"
                            UpdateMailServicesStatisticsTable(myMailService.Name, myMailService.ResponseTime)
                        Catch ex As Exception
                            StatusDetails = ex.Message
                        End Try

                        Try
                            Percent = myMailService.ResponseTime / myMailService.ResponseThreshold
                        Catch ex As Exception
                            Percent = 0
                        End Try

                        If myMailService.ResponseTime > myMailService.ResponseThreshold Then
                            myMailService.Status = "Slow"
                            myAlert.QueueAlert("Mail", myMailService.Name, "Slow", "A Mail Service " & myMailService.Name & " responded in " & myMailService.ResponseTime & " ms, but the Target is " & myMailService.ResponseThreshold & " ms.", myMailService.Location)
                        Else
                            myAlert.ResetAlert("Mail", myMailService.Name, "Slow", myMailService.Location)
                            myAlert.ResetAlert("Mail", myMailService.Name, "Not Responding", myMailService.Location)
                        End If
                End Select

            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Mail Services Monitor error: " & ex.Message)
        End Try

        If myMailService.Status = "" Then myMailService.Status = "Unknown Error"
        Dim PercentageChange As Double

        Try
            If myMailService.PreviousKeyValue > 0 And myMailService.ResponseTime > 0 Then
                PercentageChange = -(1 - myMailService.PreviousKeyValue / myMailService.ResponseTime)
            Else
                PercentageChange = 0
            End If

        Catch ex As Exception
            PercentageChange = 0
        End Try

        Try
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Description is " & myMailService.Description)
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Previous response time was " & myMailService.PreviousKeyValue & " Current value is " & myMailService.ResponseTime & "; so percent change is: " & PercentageChange)
        Catch ex As Exception

        End Try

        Try
            myMailService.StatusCode = ServerStatusCode(myMailService.Status)
        Catch ex As Exception
            myMailService.StatusCode = vbNull
        End Try


        Try
            With myMailService
                '5/5/2016 NS modified - inserting a Mail into StatusDetails fails because of a TypeANDName mismatch
                'Changed TypeANDName -MS to -Mail
                strSQL = "Update Status SET DownCount= " & .DownCount & _
                ", Status='" & .Status & "', Upcount=" & .UpCount & _
                ", UpPercent= " & .UpPercentCount & _
                ", Details='" & .ResponseDetails & _
                "', LastUpdate='" & Now & _
                "', ResponseTime='" & Str(.ResponseTime) & _
                "', NextScan='" & .NextScan & _
                "', PercentageChange='" & Str(PercentageChange) & _
                "', Location='" & "Mail Service" & _
                "', StatusCode='" & .StatusCode & _
                "', Description='" & .Description & _
                "', ResponseThreshold='" & .ResponseThreshold & _
                "', MyPercent='" & (Percent * 100) & _
                "', Name='" & .Name & "' " & _
                ", OperatingSystem='" & .ServerName & "' " & _
                ", UpPercentMinutes=" & .UpPercentMinutes & _
                ", UpMinutes=" & Str(Microsoft.VisualBasic.Strings.Format(.UpMinutes, "F1")) & _
                ", DownMinutes=" & Str(Microsoft.VisualBasic.Strings.Format(.DownMinutes, "F1")) & _
                "  WHERE TypeANDName='" & .Name & "-Mail' "
                'TypeANDName is the key

            End With
            If MyLogLevel = LogLevel.Verbose Then
                WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Mail Service SQL statement: " & vbCrLf & strSQL)
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Mail Service Monitor Error while creating SQL statement: " & ex.Message & vbCrLf & strSQL, LogLevel.Normal)
            Try
                With myMailService
                    '5/5/2016 NS modified - inserting a Mail into StatusDetails fails because of a TypeANDName mismatch
                    'Changed TypeANDName -MS to -Mail
                    strSQL = "Update Status SET DownCount= " & .DownCount & _
                       ", Status='" & .Status & "', Upcount=" & .UpCount & _
                       ", Details='" & .ResponseDetails & _
                       "', Description='" & .Description & _
                       "', LastUpdate='" & Now & _
                       "', ResponseTime='" & Str(.ResponseTime) & _
                       "', NextScan='" & .NextScan & _
                       "', ResponseThreshold='" & .ResponseThreshold & _
                       "', MyPercent='" & (Percent * 100) & _
                       "', Name='" & .Name & "' " & _
                       "  WHERE TypeANDName='" & .Name & "-Mail' "
                End With
            Catch ex3 As Exception
                WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Mail Service Monitor Error while creating SQL statement, so shorter statement was used: " & vbCrLf & strSQL)

            End Try

        End Try

        UpdateStatusTable(strSQL)


        ''**
        'Dim myPath As String
        'Dim myRegistry As New RegistryHandler
        ''Read the registry for the location of the Config Database

        'Try
        '    myPath = myRegistry.ReadFromRegistry("Status Path")
        '    'WriteAuditEntry(Now.ToString & " Mail Service module Status Table is using Configuration database " & myPath)
        'Catch ex As Exception
        '    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Failed to read registry in Mail Service module. Exception: " & ex.Message)
        'End Try

        'If myPath Is Nothing Then
        '    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Error: Failed to read registry in Mail Service module.   Cannot locate Config Database 'status.mdb'.  Configure by running" & ProductName & " client before starting the service.")
        '    '   Return False
        '    Exit Sub
        'End If
        'myRegistry = Nothing


        ''***
        ''Using SQL Server
        'If boolSQLStatus = True Then
        '    Try
        '        WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Updating the Status Table on the SQL Server")
        '        Dim myCommand As New Data.SqlClient.SqlCommand
        '        myCommand.Connection = SqlConnectionStatus
        '        myCommand.CommandText = strSQL
        '        myCommand.ExecuteNonQuery()
        '        myCommand.Dispose()
        '    Catch ex As Exception
        '        WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Error updating Mail Service status table in SQL Server: " & ex.ToString)
        '    End Try

        'Else

        '    Dim myCommand As New OleDb.OleDbCommand
        '    Dim myConnection As New OleDb.OleDbConnection

        '    With myConnection
        '        .ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & myPath
        '        .Open()
        '    End With

        '    Do Until myConnection.State = ConnectionState.Open
        '        myConnection.Open()
        '    Loop

        '    Try
        '        myCommand.CommandText = strSQL
        '        myCommand.Connection = myConnection
        '        myCommand.ExecuteNonQuery()
        '        myConnection.Close()
        '    Catch ex As Exception
        '        WriteAuditEntry(Now.ToString & " Error updating status table from Mail Service module: " & ex.Message & vbCrLf & strSQL)
        '    Finally

        '        myCommand.Dispose()
        '        myConnection.Dispose()

        '        strSQL = Nothing
        '    End Try
        'End If


        GC.Collect()
    End Sub

    Public Sub MailServiceResponseTime(ByRef myMailService As MonitoredItems.MailService)
        'This sub calculates as the number of milliseconds it takes to open a MailService 
        'sets response time at -1 if no response

        Dim start, done As Long
        Dim elapsed As TimeSpan
        start = Now.Ticks
        Dim ResponseTime As Double = 0
        Dim n As Integer
        Try
            WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Calculating " & myMailService.Category & "  response time for  " & myMailService.Name)
            myMailService.PreviousKeyValue = myMailService.ResponseTime
        Catch ex As Exception

        End Try

        Try

            With IPPort

                '      .RemoteHost = myMailService.IPAddress
                Select Case myMailService.Category
                    Case "LDAP"
                        Try

                            If MyLogLevel = LogLevel.Verbose Then
                                WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Attempting to contact LDAP for " & myMailService.Name)
                            End If

                            Try
                                .Connect(myMailService.IPAddress, 389)
                            Catch ex As Exception
                                myMailService.ResponseDetails = ex.Message
                                If MyLogLevel = LogLevel.Verbose Then
                                    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Attempt to contact " & myMailService.Name & " was unsuccessful: " & ex.Message)
                                End If
                            End Try

                            For n = 1 To 1000
                                Thread.CurrentThread.Sleep(10)
                                If .Connected = True Then
                                    '   WriteAuditEntry(Now.ToString & " Attempt to contact " & myMailService.Name & " was successful")
                                    Exit For
                                End If
                            Next

                            If .Connected = True Then
                                done = Now.Ticks
                                elapsed = New TimeSpan(done - start)

                                If MyLogLevel = LogLevel.Verbose Then
                                    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & myMailService.Name & " LDAP Service responded in " & elapsed.TotalMilliseconds & " ms.")
                                End If

                                'Try
                                '    myMailService.ResponseDetails = "Server response: " & .GetLine & vbCrLf
                                'Catch ex As Exception
                                '    myMailService.ResponseDetails = ""
                                'End Try

                                '   WriteAuditEntry(myMailService.Name & " LDAP Service responded in " & elapsed.TotalMilliseconds & " ms.")
                                Try
                                    myMailService.ResponseTime = elapsed.TotalMilliseconds
                                    myMailService.ResponseDetails = " LDAP connected in " & elapsed.TotalMilliseconds & " ms. " & vbCrLf & "Target response is " & myMailService.ResponseThreshold & " ms."

                                    myMailService.Description = " LDAP connected in " & Microsoft.VisualBasic.Strings.Format(elapsed.TotalMilliseconds, "F1") & " ms at " & Date.Now.ToShortTimeString

                                Catch ex As Exception

                                End Try

                            Else
                                ' myMailService.ResponseDetails = "Failed to connect"
                                '  WriteAuditEntry(Now.ToString & " Error in MailService module:" & ex.Message)
                                myMailService.ResponseTime = -1

                            End If
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error in LDAP MailService module:" & ex.Message)

                        Finally
                            .Disconnect()
                        End Try

                    Case "POP3"

                        Try
                            If MyLogLevel = LogLevel.Verbose Then
                                WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Attempting to contact " & myMailService.Name & " on " & myMailService.IPAddress)
                            End If

                            Try
                                .Connect(myMailService.IPAddress, 110)
                            Catch ex As Exception
                                myMailService.ResponseDetails = ex.Message
                            End Try

                            For n = 1 To 1000
                                Thread.CurrentThread.Sleep(10)
                                If .Connected = True Then
                                    '   WriteAuditEntry(Now.ToString & " Attempt to contact " & myMailService.Name & " was successful")
                                    Exit For
                                End If
                            Next

                            Dim strResponse As String = ""
                            Try
                                .SendLine("noop")
                                strResponse = .GetLine
                                myMailService.ResponseDetails = "Server response: " & strResponse & vbCrLf & vbCrLf
                            Catch ex As Exception

                            End Try

                            If .Connected = True Then
                                Try
                                    done = Now.Ticks
                                    elapsed = New TimeSpan(done - start)

                                    myMailService.ResponseTime = elapsed.TotalMilliseconds
                                    myMailService.ResponseDetails += "POP3 server connected in " & elapsed.TotalMilliseconds & " ms. " & vbCrLf & "Target response is " & myMailService.ResponseThreshold & " ms."
                                    '   myMailService.Description = "POP3 server connected in " & elapsed.TotalMilliseconds & " ms. and responded with " & strResponse
                                    myMailService.Description = " POP3 Mail service connected in " & Microsoft.VisualBasic.Strings.Format(elapsed.TotalMilliseconds, "F1") & " ms at " & Date.Now.ToShortTimeString '& vbCrLf & " with " & strResponse


                                    If MyLogLevel = LogLevel.Verbose Then
                                        WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & myMailService.ResponseDetails)
                                    End If
                                Catch ex As Exception
                                    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Error " & ex.ToString, LogLevel.Normal)
                                End Try

                            Else
                                If MyLogLevel = LogLevel.Verbose Then
                                    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Attempt to contact " & myMailService.Name & " was unsuccessful")
                                End If
                                myMailService.ResponseDetails = "Unable to connect to the POP3 server"
                                myMailService.Description = "Unable to connect to the POP3 server"

                                myMailService.ResponseTime = -1

                            End If
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error in POP3 MailService module:" & ex.Message)
                        Finally
                            .Disconnect()
                        End Try

                        If InStr(myMailService.ResponseDetails, "ERR") Then
                            myMailService.ResponseTime = -1
                            myMailService.ResponseDetails = "Failed to connect"
                        End If

                    Case "SMTP"
                        myMailService.ResponseDetails = ""
                        Dim strResponse As String = ""
                        Try
                            WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Attempting to contact " & myMailService.Name)
                            WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Using address " & myMailService.IPAddress)
                            WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Using port " & myMailService.Port)
                        Catch ex As Exception

                        End Try

                        If myMailService.Port = 0 Then
                            myMailService.Port = 25
                        End If

                        'New code using Chilkat
                        Dim Socket As New Chilkat.Socket
                        Dim success As Boolean
                        Try
                            success = Socket.UnlockComponent("MZLDADSocket_OACwPK2ZlEn9")
                            If (success <> True) Then
                                myMailService.ResponseDetails = "Failed to unlock component"
                                WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Failed to unlock Chilkat component")
                            Else
                                WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Unlocked Chilkat component")
                            End If
                        Catch ex As Exception

                        End Try


                        Try
                            Dim ssl As Boolean
                            ssl = False
                            Dim maxWaitMillisec As Long
                            maxWaitMillisec = 20000
                            success = Socket.Connect(myMailService.IPAddress, myMailService.Port, ssl, maxWaitMillisec)
                        Catch ex As Exception
                            success = False
                            WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Failed to connect #1 because " & ex.ToString)
                        End Try

                        Try

                            '  Connect to port 5555 of localhost.
                            '  The string "localhost" is for testing on a single computer.
                            '  It would typically be replaced with an IP hostname, such
                            '  as "www.chilkatsoft.com".

                            If (success <> True) Then
                                strResponse = Socket.LastErrorText
                                myMailService.ResponseTime = -1
                                myMailService.Description = "Unable to connect to the SMTP server at " & Date.Now.ToShortTimeString
                                myMailService.ResponseDetails = "Unable to connect to the SMTP server. "
                                WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Failed to connect because " & strResponse)
                                myMailService.ResponseTime = -1
                            Else
                                '  Set maximum timeouts for reading an writing (in millisec)
                                Socket.MaxReadIdleMs = 10000
                                Socket.MaxSendIdleMs = 10000
                                WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Successfully connected to " & myMailService.Name)
                                '  The server (in this example) is going to send a "Hello World!"
                                '  message.  Read it:
                                Try
                                    ' Dim receivedMsg As String
                                    Socket.SendString("EHLO")
                                    strResponse = Socket.ReceiveString()
                                    done = Now.Ticks
                                    elapsed = New TimeSpan(done - start)
                                    myMailService.ResponseTime = elapsed.TotalMilliseconds


                                    If Trim(strResponse) = "" Or InStr(strResponse.ToUpper, "NOT AVAILABLE") Then
                                        WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " No response back from the server, even though we connected. ")
                                        myMailService.ResponseDetails += " Service connected in " & elapsed.TotalMilliseconds & " ms." & " but NO RESPONSE came back from the server"
                                        myMailService.ResponseTime = -1
                                    Else
                                        WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Service answered with  " & Trim(strResponse))
                                        myMailService.ResponseDetails = "Server response: " & Trim(strResponse)
                                        myMailService.ResponseDetails += " || Connected in " & elapsed.TotalMilliseconds & " ms." & vbCrLf & "Target response is " & myMailService.ResponseThreshold & " ms."
                                        ' myMailService.Description = " SMTP server connected in " & Microsoft.VisualBasic.Strings.Format(elapsed.TotalMilliseconds, "F1") & " ms. " & vbCrLf & "Target response is " & myMailService.ResponseThreshold & " ms."
                                        myMailService.Description = " SMTP service connected in " & Microsoft.VisualBasic.Strings.Format(elapsed.TotalMilliseconds, "F1") & " ms at " & Date.Now.ToShortTimeString '& vbCrLf & " with " & strResponse
                                    End If

                                Catch ex As Exception

                                End Try


                                '  Close the connection with the server
                                '  Wait a max of 20 seconds (20000 millsec)
                                Socket.Close(20000)

                            End If

                        Catch ex As Exception

                        Finally
                            Socket.Dispose()

                        End Try



                    Case "IMAP"
                        If MyLogLevel = LogLevel.Verbose Then
                            WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Attempting to contact " & myMailService.Name)
                        End If

                        Try
                            Try
                                .Connect(myMailService.IPAddress, 143)
                            Catch ex As Exception
                                myMailService.ResponseDetails = ex.Message
                            End Try

                            For n = 1 To 1000
                                Thread.CurrentThread.Sleep(10)
                                If .Connected = True Then
                                    '   WriteAuditEntry(Now.ToString & " Attempt to contact " & myMailService.Name & " was successful")
                                    Exit For
                                End If
                            Next
                            Dim strResponse As String = ""

                            If .Connected = True Then
                                Try
                                    done = Now.Ticks
                                    elapsed = New TimeSpan(done - start)
                                    '   WriteAuditEntry(myMailService.Name & " LDAP Service responded in " & elapsed.TotalMilliseconds & " ms.")
                                    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " IMAP Service responded in " & elapsed.TotalMilliseconds & " ms.")
                                    myMailService.ResponseTime = elapsed.TotalMilliseconds
                                    myMailService.ResponseDetails = " IMAP service connected in " & elapsed.TotalMilliseconds & " ms." & vbCrLf & "Target response is " & myMailService.ResponseThreshold & " ms."
                                    myMailService.Description = " IMAP service connected in " & Microsoft.VisualBasic.Strings.Format(elapsed.TotalMilliseconds, "F1") & " ms at " & Date.Now.ToShortTimeString '& vbCrLf & " with " & strResponse

                                Catch ex As Exception

                                End Try

                                'Try
                                '    strResponse = .GetLine
                                'Catch ex As Exception
                                '    strResponse = "Failed to read a line"
                                'End Try

                                'Try
                                '    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " IMAP Service responded with " & Trim(strResponse))
                                '    myMailService.ResponseDetails += vbCrLf & "Server response: " & strResponse

                                'Catch ex As Exception
                                '    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Exception reading the line from IMAP")
                                '    myMailService.ResponseDetails = ex.Message
                                '    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Error @ IMAP 3 " & ex.ToString)
                                '    End Try

                            Else
                                'myMailService.ResponseDetails = "Failed to connect"
                                '         WriteAuditEntry(Now.ToString & " Error in MailService module:" & ex.Message)
                                myMailService.ResponseTime = -1
                                WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Failed to connect.")
                            End If
                        Catch ex As Exception

                            WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Error in IMAP MailService module:" & ex.Message)

                        Finally
                            .Disconnect()
                        End Try

                    Case Else
                        Try

                            Try
                                .Connect(myMailService.IPAddress, myMailService.Port)
                            Catch ex As Exception
                                myMailService.ResponseDetails = ex.Message
                            End Try

                            For n = 1 To 1000
                                Thread.CurrentThread.Sleep(10)
                                If .Connected = True Then
                                    '   WriteAuditEntry(Now.ToString & " Attempt to contact " & myMailService.Name & " was successful")
                                    Exit For
                                End If
                            Next

                            If .Connected = True Then
                                done = Now.Ticks
                                elapsed = New TimeSpan(done - start)

                                myMailService.ResponseTime = elapsed.TotalMilliseconds
                                myMailService.ResponseDetails = " Mail server connected in " & elapsed.TotalMilliseconds & " ms. Target response is " & myMailService.ResponseThreshold & " ms."
                                myMailService.Description = " Mail server connected in " & elapsed.TotalMilliseconds & " ms. Target response is " & myMailService.ResponseThreshold & " ms."

                            Else
                                '  myMailService.ResponseDetails = "Failed to connect"
                                myMailService.ResponseTime = -1

                            End If
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error in Mail MailService module:" & ex.Message)

                        Finally
                            .Disconnect()
                        End Try

                End Select

            End With

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in MailService module:" & ex.Message)
            Return
        End Try



    End Sub


    Public Sub MailServiceResponseTimeOLD(ByRef myMailService As MonitoredItems.MailService)
        'This sub calculates as the number of milliseconds it takes to open a MailService 
        'sets response time at -1 if no response

        Dim start, done, hits As Long
        Dim elapsed As TimeSpan
        start = Now.Ticks
        Dim ResponseTime As Double = 0
        Dim n As Integer
        myMailService.PreviousKeyValue = myMailService.ResponseTime
        Try
            Dim span As System.TimeSpan

            With IPPort

                '      .RemoteHost = myMailService.IPAddress
                Select Case myMailService.Category
                    Case "LDAP"
                        Try

                            If MyLogLevel = LogLevel.Verbose Then
                                WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Attempting to contact " & myMailService.Name)
                            End If

                            Try
                                .Connect(myMailService.IPAddress, 389)
                            Catch ex As Exception
                                myMailService.ResponseDetails = ex.Message
                                If MyLogLevel = LogLevel.Verbose Then
                                    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Attempt to contact " & myMailService.Name & " was unsuccessful: " & ex.Message)
                                End If
                            End Try

                            For n = 1 To 1000
                                Thread.CurrentThread.Sleep(10)
                                If .Connected = True Then
                                    '   WriteAuditEntry(Now.ToString & " Attempt to contact " & myMailService.Name & " was successful")
                                    Exit For
                                End If
                            Next

                            If .Connected = True Then
                                done = Now.Ticks
                                elapsed = New TimeSpan(done - start)

                                If MyLogLevel = LogLevel.Verbose Then
                                    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & myMailService.Name & " LDAP Service responded in " & elapsed.TotalMilliseconds & " ms.")
                                End If

                                'Try
                                '    myMailService.ResponseDetails = "Server response: " & .GetLine & vbCrLf
                                'Catch ex As Exception
                                '    myMailService.ResponseDetails = ""
                                'End Try

                                '   WriteAuditEntry(myMailService.Name & " LDAP Service responded in " & elapsed.TotalMilliseconds & " ms.")
                                Try
                                    myMailService.ResponseTime = elapsed.TotalMilliseconds
                                    myMailService.ResponseDetails = " LDAP connected in " & elapsed.TotalMilliseconds & " ms. " & vbCrLf & "Target response is " & myMailService.ResponseThreshold & " ms."

                                    myMailService.Description = " LDAP connected in " & Microsoft.VisualBasic.Strings.Format(elapsed.TotalMilliseconds, "F1") & " ms at " & Date.Now.ToShortTimeString

                                Catch ex As Exception

                                End Try

                            Else
                                ' myMailService.ResponseDetails = "Failed to connect"
                                '  WriteAuditEntry(Now.ToString & " Error in MailService module:" & ex.Message)
                                myMailService.ResponseTime = -1

                            End If
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error in LDAP MailService module:" & ex.Message)

                        Finally
                            .Disconnect()
                        End Try

                    Case "POP3"

                        Try

                            If MyLogLevel = LogLevel.Verbose Then
                                WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Attempting to contact " & myMailService.Name)
                            End If

                            Try
                                .Connect(myMailService.IPAddress, 110)
                            Catch ex As Exception
                                myMailService.ResponseDetails = ex.Message
                            End Try

                            For n = 1 To 1000
                                Thread.CurrentThread.Sleep(10)
                                If .Connected = True Then
                                    '   WriteAuditEntry(Now.ToString & " Attempt to contact " & myMailService.Name & " was successful")
                                    Exit For
                                End If
                            Next

                            Dim strResponse As String = ""
                            Try
                                .SendLine("noop")
                                strResponse = .GetLine
                                myMailService.ResponseDetails = "Server response: " & strResponse & vbCrLf & vbCrLf
                            Catch ex As Exception

                            End Try

                            If .Connected = True Then
                                Try
                                    done = Now.Ticks
                                    elapsed = New TimeSpan(done - start)

                                    myMailService.ResponseTime = elapsed.TotalMilliseconds
                                    myMailService.ResponseDetails += "POP3 server connected in " & elapsed.TotalMilliseconds & " ms. " & vbCrLf & "Target response is " & myMailService.ResponseThreshold & " ms."
                                    '   myMailService.Description = "POP3 server connected in " & elapsed.TotalMilliseconds & " ms. and responded with " & strResponse
                                    myMailService.Description = " POP3 Mail service connected in " & Microsoft.VisualBasic.Strings.Format(elapsed.TotalMilliseconds, "F1") & " ms at " & Date.Now.ToShortTimeString '& vbCrLf & " with " & strResponse


                                    If MyLogLevel = LogLevel.Verbose Then
                                        WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & myMailService.ResponseDetails)
                                    End If
                                Catch ex As Exception
                                    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Error " & ex.ToString, LogLevel.Normal)
                                End Try

                            Else
                                If MyLogLevel = LogLevel.Verbose Then
                                    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " Attempt to contact " & myMailService.Name & " was unsuccessful")
                                End If
                                myMailService.ResponseDetails = "Unable to connect to the POP3 server"
                                myMailService.Description = "Unable to connect to the POP3 server"

                                myMailService.ResponseTime = -1

                            End If
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error in POP3 MailService module:" & ex.Message, LogLevel.Normal)
                        Finally
                            .Disconnect()
                        End Try

                        If InStr(myMailService.ResponseDetails, "ERR") Then
                            myMailService.ResponseTime = -1
                            myMailService.ResponseDetails = "Failed to connect"
                        End If

                    Case "SMTP"


                        Try
                            myMailService.ResponseDetails = ""
                            Dim strResponse As String = ""
                            Try
                                .Connect(myMailService.IPAddress, 25)
                            Catch ex As Exception
                                myMailService.ResponseDetails = ex.ToString
                            End Try

                            For n = 1 To 1000
                                Thread.CurrentThread.Sleep(10)
                                If .Connected = True Then
                                    '   WriteAuditEntry(Now.ToString & " Attempt to contact " & myMailService.Name & " was successful")
                                    Exit For
                                End If
                            Next

                            If .Connected = True Then
                                done = Now.Ticks
                                elapsed = New TimeSpan(done - start)

                                myMailService.ResponseTime = elapsed.TotalMilliseconds
                                Try
                                    .SendLine("EHLO")
                                    strResponse = .GetLine
                                Catch ex As Exception
                                    strResponse = ex.ToString
                                End Try

                                myMailService.ResponseDetails = "Server response: " & strResponse & vbCrLf & vbCrLf
                                myMailService.ResponseDetails += " SMTP server connected in " & elapsed.TotalMilliseconds & " ms." & vbCrLf & "Target response is " & myMailService.ResponseThreshold & " ms."
                                ' myMailService.Description = " SMTP server connected in " & Microsoft.VisualBasic.Strings.Format(elapsed.TotalMilliseconds, "F1") & " ms. " & vbCrLf & "Target response is " & myMailService.ResponseThreshold & " ms."
                                myMailService.Description = " SMTP service connected in " & Microsoft.VisualBasic.Strings.Format(elapsed.TotalMilliseconds, "F1") & " ms at " & Date.Now.ToShortTimeString '& vbCrLf & " with " & strResponse

                            Else
                                ' myMailService.ResponseDetails = "Failed to connect"
                                '         WriteAuditEntry(Now.ToString & " Error in MailService module:" & ex.Message)
                                myMailService.ResponseTime = -1
                                myMailService.Description = "Unable to connect to the SMTP server at " & Date.Now.ToShortTimeString
                                myMailService.ResponseDetails = "Unable to connect to the SMTP server. "
                            End If
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error in SMTP MailService module:" & ex.Message)

                        Finally
                            .Disconnect()
                        End Try

                    Case "IMAP"
                        If MyLogLevel = LogLevel.Verbose Then
                            WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Attempting to contact " & myMailService.Name)
                        End If

                        Try
                            Try
                                .Connect(myMailService.IPAddress, 143)
                            Catch ex As Exception
                                myMailService.ResponseDetails = ex.Message
                            End Try

                            For n = 1 To 1000
                                Thread.CurrentThread.Sleep(10)
                                If .Connected = True Then
                                    '   WriteAuditEntry(Now.ToString & " Attempt to contact " & myMailService.Name & " was successful")
                                    Exit For
                                End If
                            Next
                            Dim strResponse As String = ""

                            If .Connected = True Then
                                Try
                                    done = Now.Ticks
                                    elapsed = New TimeSpan(done - start)
                                    '   WriteAuditEntry(myMailService.Name & " LDAP Service responded in " & elapsed.TotalMilliseconds & " ms.")

                                    myMailService.ResponseTime = elapsed.TotalMilliseconds
                                Catch ex As Exception

                                End Try

                                Try
                                    strResponse = .GetLine
                                    myMailService.ResponseDetails = "Server response: " & strResponse & vbCrLf & vbCrLf
                                    myMailService.ResponseDetails += " IMAP service connected in " & elapsed.TotalMilliseconds & " ms." & vbCrLf & "Target response is " & myMailService.ResponseThreshold & " ms."
                                    ' myMailService.Description = " IMAP service connected in " & elapsed.TotalMilliseconds & "Target response is " & myMailService.ResponseThreshold & " ms."

                                    myMailService.Description = " IMAP service connected in " & Microsoft.VisualBasic.Strings.Format(elapsed.TotalMilliseconds, "F1") & " ms at " & Date.Now.ToShortTimeString '& vbCrLf & " with " & strResponse

                                Catch ex As Exception
                                    myMailService.ResponseDetails = ex.Message
                                    WriteDeviceHistoryEntry("Mail_Service", myMailService.Name, Now.ToString & " " & " Error @ IMAP 3 " & ex.ToString, LogLevel.Normal)
                                End Try

                            Else
                                'myMailService.ResponseDetails = "Failed to connect"
                                '         WriteAuditEntry(Now.ToString & " Error in MailService module:" & ex.Message)
                                myMailService.ResponseTime = -1

                            End If
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error in IMAP MailService module:" & ex.Message)

                        Finally
                            .Disconnect()
                        End Try

                    Case Else
                        Try

                            Try
                                .Connect(myMailService.IPAddress, myMailService.Port)
                            Catch ex As Exception
                                myMailService.ResponseDetails = ex.Message
                            End Try

                            For n = 1 To 1000
                                Thread.CurrentThread.Sleep(10)
                                If .Connected = True Then
                                    '   WriteAuditEntry(Now.ToString & " Attempt to contact " & myMailService.Name & " was successful")
                                    Exit For
                                End If
                            Next

                            If .Connected = True Then
                                done = Now.Ticks
                                elapsed = New TimeSpan(done - start)

                                myMailService.ResponseTime = elapsed.TotalMilliseconds
                                myMailService.ResponseDetails = " Mail server connected in " & elapsed.TotalMilliseconds & " ms. Target response is " & myMailService.ResponseThreshold & " ms."
                                myMailService.Description = " Mail server connected in " & elapsed.TotalMilliseconds & " ms. Target response is " & myMailService.ResponseThreshold & " ms."

                            Else
                                '  myMailService.ResponseDetails = "Failed to connect"
                                myMailService.ResponseTime = -1

                            End If
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error in Mail MailService module:" & ex.Message)

                        Finally
                            .Disconnect()
                        End Try

                End Select

            End With

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in MailService module:" & ex.Message)
            Return
        End Try



    End Sub


#End Region


#Region "Network Devices"




#End Region

#Region "Websphere Monitoring"

    Private Sub MonitorWebSphere()

        Dim WsDll As New VitalSignsWebSphereDLL.VitalSignsWebSphereDLL()
        'Dim WsDllForObjs As VitalSignsWebSphereDLL.VitalSignsWebSphereDLL

        Do While boolTimeToStop <> True

            Try
                Dim myServer As MonitoredItems.WebSphere
                myServer = SelectWebSphereServerToMonitor()

                If myServer Is Nothing Then
                    '    WriteAuditEntry(Now.ToString & " >>> No ST servers are due for monitoring now.  >>>>")
                    CurrentWebSphere = ""
                    GoTo CleanUp
                Else
                    '   WriteAuditEntry(Now.ToString & " >>> Selected " & myServer.Name)
                End If
                WriteAuditEntryWebSphere(Now.ToString & " Scanning " & myServer.Name)
                myServer.Status = "OK"
                myServer.ResponseDetails = "This server passed all tests"

                Dim Cells As VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Cells_ServerStats
                Try
                    Cells = WsDll.getServerStats(myServer)
                Catch ex As Exception
                    WriteAuditEntryWebSphere(Now.ToString & " Exception thrown while getting stats: " & ex.Message.ToString())
                    myServer.ResponseDetails = "The server never sent a response back.  Check the status of the server."
                    myServer.Status = "Issue"
                    myServer.StatusCode = "Issue"

                    Cells = New VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Cells_ServerStats
                    Cells.Connectionstatus = "NOT CONNECTED"
                End Try

                If (Cells.Connectionstatus <> "CONNECTED") Then
                    WriteDeviceHistoryEntry("WebSphere", myServer.Name, Now.ToString & " Could not connect. Name: " & myServer.ServerName)
                    WriteAuditEntryWebSphere(Now.ToString & " Could not conenct to " & myServer.Name & ".")
                    myServer.Status = "Not Responding"
                    myServer.StatusCode = "Not Responding"
                    myServer.ResponseDetails = "Could not connect to the server"
                    SendWebSphereNotRespondingAlert(myServer, False)

                Else
                    WriteDeviceHistoryEntry("WebSphere", myServer.Name, Now.ToString & " Connected. Name: " & myServer.ServerName)
                    SendWebSphereNotRespondingAlert(myServer, True)

                    Dim server As VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Server_ServerStats = Cells.Cell.Nodes.Node.Servers.Server

                    'go through all the stats and save it to the server object
                    Try
                        CopyValuesToCollectionServers(myServer, server)
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("WebSphere", myServer.Name, Now.ToString & " Exception copying values. Exception: " & ex.Message.ToString())
                    End Try


                    'Send Alerts
                    SendWebSphereAlerts(myServer)

                End If

                myServer.LastScan = Date.Now

                UpdateWebSphereDetailsTable(myServer)
                UpdateWebSphereStatusTable(myServer)


                myServer.IsBeingScanned = False
                WriteAuditEntryWebSphere(Now.ToString & " Done Scanning " & myServer.Name)
CleanUp:

            Catch ThreadEx As ThreadAbortException
                WriteAuditEntry(Now.ToString & " Aborting WebSphere monitoring sub " & ThreadEx.Message)
                Exit Sub
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Exception WebSphere monitoring sub " & ex.Message)
            End Try

            If MyWebSphereServers.Count < 24 Then
                Thread.Sleep(2500)
            ElseIf MyWebSphereServers.Count < 10 Then
                Thread.Sleep(10000)
            End If

            dtWebSphereLastUpdate = Now

        Loop

    End Sub

    Private Sub SendWebSphereNotRespondingAlert(ByRef server As MonitoredItems.WebSphere, ByVal resetAlert As Boolean)

        With server

            If (resetAlert) Then
                myAlert.ResetAlert(.ServerType, .Name, "Not Responding", .Location, "The server is responding.", "WebSphere")
            Else
                myAlert.QueueAlert(.ServerType, .Name, "Not Responding", .Location, "The server is not responding", "WebSphere")
            End If

        End With

    End Sub

    Private Sub CopyValuesToCollectionServers(ByRef CollectionServerBeingCopiedTo As MonitoredItems.WebSphere, ByRef XMLServerCopiedFrom As VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Server_ServerStats)

        Dim stats As VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.stats = XMLServerCopiedFrom.Stats

        WriteAuditEntryWebSphere(Now.ToString & " into CopyValuesToCollectionServers" & "")
        If Not (stats.ActiveCount Is Nothing) Then
            CollectionServerBeingCopiedTo.ActiveThreadCount = Convert.ToInt32(stats.ActiveCount.Current)
        End If

        If Not (stats.ClearedThreadHangCount Is Nothing) Then
            CollectionServerBeingCopiedTo.ClearedThreadHangCount = Convert.ToInt32(stats.ClearedThreadHangCount.Count)
        End If

        If Not (stats.ConcurrentHungThreadCount Is Nothing) Then
            CollectionServerBeingCopiedTo.ConcurrentHungThreadCount = Convert.ToInt32(stats.ConcurrentHungThreadCount.Current)
        End If

        If Not (stats.DeclaredThreadHungCount Is Nothing) Then
            CollectionServerBeingCopiedTo.DeclaredThreadHungCount = Convert.ToInt32(stats.DeclaredThreadHungCount.Count)
        End If

        If Not (stats.FreeMemory Is Nothing) Then
            CollectionServerBeingCopiedTo.Memory_Free = Convert.ToInt32(stats.FreeMemory.Count)
        End If

        If Not (stats.HeapSize Is Nothing) Then
            CollectionServerBeingCopiedTo.CurrentHeap = Convert.ToInt32(stats.HeapSize.Current) / 1024  'to convert kb to mb
        End If

        If Not (stats.HeapSize Is Nothing) Then
            CollectionServerBeingCopiedTo.HeapSizeinitial = Convert.ToInt32(stats.HeapSize.initial) / 1024  'to convert kb to mb
            '' ''WriteAuditEntryWebSphere(Now.ToString & " InitialHeap " & CollectionServerBeingCopiedTo.InitialHeap)
        End If
        If Not (stats.HeapSize Is Nothing) Then
            CollectionServerBeingCopiedTo.HeapSizemaximum = Convert.ToInt32(stats.HeapSize.maximum) / 1024  'to convert kb to mb
            ''WriteAuditEntryWebSphere(Now.ToString & " MaximumHeap " & CollectionServerBeingCopiedTo.maximum)
        End If
        If Not (stats.PoolSize Is Nothing) Then
            CollectionServerBeingCopiedTo.AverageThreadPool = Convert.ToInt32(stats.PoolSize.Current)
        End If

        If Not (stats.ProcessCpuUsage Is Nothing) Then
            CollectionServerBeingCopiedTo.CPU_Utilization = Convert.ToInt32(stats.ProcessCpuUsage.Count)
        End If

        If Not (stats.UpTime Is Nothing) Then
            'In seconds, convert to days
            'sec * 1 min / 60 sec * 1 hour / 60 sec * 1 day / 24 horus = sec / 60 / 60 / 24
            CollectionServerBeingCopiedTo.UpTime = Math.Round(Convert.ToInt32(stats.UpTime.Count), 1)
        End If

        If Not (stats.UsedMemory Is Nothing) Then
            CollectionServerBeingCopiedTo.Memory_Used = Math.Round(Convert.ToInt32(stats.UsedMemory.Count) / 1024, 0)    'to convert kb to mb
        End If

        If Not (stats.Process Is Nothing Or stats.Process.ID Is Nothing) Then
            CollectionServerBeingCopiedTo.ProcessId = Convert.ToInt32(stats.Process.ID)
        End If

        If Not (stats.ResponseTime Is Nothing Or stats.ResponseTime.Value Is Nothing) Then
            CollectionServerBeingCopiedTo.ResponseTime = Convert.ToInt32(stats.ResponseTime.Value)
        End If

    End Sub

    Private Sub SendWebSphereAlert(ByRef server As MonitoredItems.WebSphere, ByVal actualValue As Double, ByVal thresholdValue As Double, ByVal statName As String)
        Try


            With server

                Dim resetAlert As Boolean = actualValue < thresholdValue
                Dim msg As String = "The " & statName & " has a current value of " & actualValue & " and a threshold of " & thresholdValue & "."

                If (resetAlert) Then
                    myAlert.ResetAlert(.ServerType, .Name, statName, .Location, msg, "WebSphere")
                Else
                    myAlert.QueueAlert(.ServerType, .Name, statName, .Location, msg, "WebSphere")
                    server.Status = "Issue"
                    server.ResponseDetails = msg
                End If
            End With
        Catch ex As Exception
            WriteDeviceHistoryEntry("WebSphere", statName, Now.ToString & " the " & statName & " alert faield to process.  Exception: " & ex.Message.ToString())
        End Try
    End Sub

    Private Sub SendWebSphereAlerts(ByRef server As MonitoredItems.WebSphere)

        Dim msg As String
        Dim resetAlert As Boolean
        Dim actualVal As Double
        Dim thresholdVal As Double
        Dim statName As String

        With server
            'WriteAuditEntryWebSphere(Now.ToString & " into SendWebSphereAlerts" & "")
            actualVal = .ActiveThreadCount
            thresholdVal = .ActiveThreadCountThreshold
            statName = "Active Thread Count"
            'SendWebSphereAlert(server, resetAlert, actualVal, thresholdVal, statName)
            InsertIntoWebSphereDailyStats(server.Name, statName.Replace(" ", ""), actualVal, "")

            actualVal = .ClearedThreadHangCount
            thresholdVal = .ClearedThreadHangCountThreshold
            statName = "Cleared Hung Thread Count"
            'SendWebSphereAlert(server, resetAlert, actualVal, thresholdVal, statName)
            InsertIntoWebSphereDailyStats(server.Name, statName.Replace(" ", ""), actualVal, "")

            actualVal = .ConcurrentHungThreadCount
            thresholdVal = 1
            statName = "Current Hung Thread Count"
            SendWebSphereAlert(server, actualVal, thresholdVal, statName)
            InsertIntoWebSphereDailyStats(server.Name, statName.Replace(" ", ""), actualVal, "")

            actualVal = .DeclaredThreadHungCount
            thresholdVal = .DeclaredThreadHungCountThreshold
            statName = "Declared Hung Thread Count"
            'SendWebSphereAlert(server, resetAlert, actualVal, thresholdVal, statName)
            InsertIntoWebSphereDailyStats(server.Name, statName.Replace(" ", ""), actualVal, "")

            actualVal = Math.Round((.Memory_Used / (.Memory_Free + .Memory_Used)) * 100, 0)
            thresholdVal = .Memory_Threshold
            statName = "Memory"
            'SendWebSphereAlert(server, resetAlert, actualVal, thresholdVal, statName)
            InsertIntoWebSphereDailyStats(server.Name, statName.Replace(" ", ""), actualVal, "")

            actualVal = .Memory_Used
            statName = "Memory Used"
            'SendWebSphereAlert(server, resetAlert, actualVal, thresholdVal, statName)
            InsertIntoWebSphereDailyStats(server.Name, statName.Replace(" ", ""), actualVal, "")

            actualVal = .Memory_Free
            thresholdVal = .Memory_Threshold
            statName = "Memory Free"
            'SendWebSphereAlert(server, resetAlert, actualVal, thresholdVal, statName)
            InsertIntoWebSphereDailyStats(server.Name, statName.Replace(" ", ""), actualVal, "")

            actualVal = .CurrentHeap
            thresholdVal = .CurrentHeapThreshold
            statName = "Current Heap Size"
            'SendWebSphereAlert(server, resetAlert, actualVal, thresholdVal, statName)
            InsertIntoWebSphereDailyStats(server.Name, statName.Replace(" ", ""), actualVal, "")


            actualVal = .HeapSizeinitial
            thresholdVal = .HeapSizeinitial
            statName = "Initial Heap"
            'SendWebSphereAlert(server, resetAlert, actualVal, thresholdVal, statName)
            InsertIntoWebSphereDailyStats(server.Name, statName.Replace(" ", ""), actualVal, "")
            WriteAuditEntryWebSphere(Now.ToString & " InitialHeap " & actualVal)


            actualVal = .HeapSizemaximum
            thresholdVal = .HeapSizemaximum
            statName = "Maximum Heap"
            'SendWebSphereAlert(server, resetAlert, actualVal, thresholdVal, statName)
            InsertIntoWebSphereDailyStats(server.Name, statName.Replace(" ", ""), actualVal, "")
            WriteAuditEntryWebSphere(Now.ToString & " MaximumHeap " & actualVal)



            actualVal = .AverageThreadPool
            thresholdVal = .AverageThreadPoolThreshold
            statName = "Average Pool Size"
            'SendWebSphereAlert(server, resetAlert, actualVal, thresholdVal, statName)
            InsertIntoWebSphereDailyStats(server.Name, statName.Replace(" ", ""), actualVal, "")

            actualVal = .UpTime
            thresholdVal = .UpTimeThreshold
            statName = "Up Time"
            'SendWebSphereAlert(server, resetAlert, actualVal, thresholdVal, statName)
            InsertIntoWebSphereDailyStats(server.Name, statName.Replace(" ", ""), actualVal, "")

            actualVal = .CPU_Utilization
            thresholdVal = .CPU_Threshold
            statName = "Process CPU Usage"
            'SendWebSphereAlert(server, resetAlert, actualVal, thresholdVal, statName)
            InsertIntoWebSphereDailyStats(server.Name, statName.Replace(" ", ""), actualVal, "")

            actualVal = .ResponseTime
            thresholdVal = .ResponseThreshold
            statName = "ResponseTime"
            'SendWebSphereAlert(server, resetAlert, actualVal, thresholdVal, statName)
            InsertIntoWebSphereDailyStats(server.Name, statName.Replace(" ", ""), actualVal, "")
        End With

    End Sub

    Private Function CreateListOfCells() As List(Of VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties)
        Dim list As New List(Of VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties)
        Dim cellNames As New List(Of String)
        Dim tempCell As VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties
        For Each server As MonitoredItems.WebSphere In MyWebSphereServers
            Dim cellName As String = server.CellName
            If Not cellNames.Contains(cellName) Then
                tempCell = New VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties
                tempCell.Name = server.CellName
                tempCell.HostName = server.CellHostName
                tempCell.ConnectionType = server.ConnectionType
                tempCell.ID = server.CellID
                tempCell.Password = server.Password
                tempCell.Port = server.Port
                tempCell.Realm = server.Realm
                tempCell.UserName = server.UserName

                list.Add(tempCell)
                cellNames.Add(cellName)
            End If
        Next
        Return list
    End Function

    Private Function SelectWebSphereServerToMonitor() As MonitoredItems.WebSphere
        'WriteAuditEntry(Now.ToString & " >>> Selecting a Domino Server for monitoring >>>>")
        Dim tNow As DateTime
        tNow = Now
        Dim tScheduled As DateTime

        Dim timeOne, timeTwo As DateTime

        Dim SelectedServer As MonitoredItems.WebSphere

        Dim ServerOne As MonitoredItems.WebSphere
        Dim ServerTwo As MonitoredItems.WebSphere

        Dim myRegistry As New RegistryHandler

        Dim n As Integer
        Dim strSQL As String = ""
        Dim ServerType As String = "WebSphere"
        Dim serverName As String = ""

        Try
            strSQL = "Select svalue from ScanSettings where sname = 'Scan" & ServerType & "ASAP'"
            Dim ds As New DataSet()
            ds.Tables.Add("ScanASAP")
            Dim objVSAdaptor As New VSAdaptor
            objVSAdaptor.FillDatasetAny("VitalSigns", "VitalSigns", strSQL, ds, "ScanASAP")

            For Each row As DataRow In ds.Tables("ScanASAP").Rows
                Try
                    serverName = row(0).ToString()
                Catch ex As Exception
                    Continue For
                End Try

                For n = 0 To MyWebSphereServers.Count - 1
                    ServerOne = MyWebSphereServers.Item(n)

                    If ServerOne.Name = serverName And ServerOne.IsBeingScanned = False And ServerOne.Enabled Then
                        'WriteAuditEntry(Now.ToString & " >>> " & serverName & " was marked 'Scan ASAP' so that will be scanned next.")
                        strSQL = "DELETE FROM ScanSettings where sname = 'Scan" & ServerType & "ASAP' and svalue='" & serverName & "'"
                        objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)

                        Return ServerOne
                        Exit Function

                    End If
                Next
            Next

        Catch ex As Exception

        End Try

        'Any server Not Scanned should be scanned right away.  Select the first one you encounter
        For n = 0 To MyWebSphereServers.Count - 1
            ServerOne = MyWebSphereServers.Item(n)

            If ServerOne.Status = "Not Scanned" Or ServerOne.Status = "Master Service Stopped." Then
                '        WriteAuditEntry(Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status)
                Return ServerOne
                Exit Function
            End If
        Next

        'start with the first two servers
        ServerOne = MyWebSphereServers.Item(0)
        If MyWebSphereServers.Count > 1 Then ServerTwo = MyWebSphereServers.Item(1)

        'go through the remaining servers, see which one has the oldest (earliest) scheduled time
        If MyWebSphereServers.Count > 2 Then
            Try
                For n = 2 To MyWebSphereServers.Count - 1
                    '           WriteAuditEntry(Now.ToString & " N is " & n)
                    timeOne = CDate(ServerOne.NextScan)
                    timeTwo = CDate(ServerTwo.NextScan)
                    If DateTime.Compare(timeOne, timeTwo) < 0 Then
                        'time one is earlier than time two, so keep server 1
                        ServerTwo = MyWebSphereServers.Item(n)
                    Else
                        'time two is later than time one, so keep server 2
                        ServerOne = MyWebSphereServers.Item(n)
                    End If
                Next
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " >>> Error Selecting WebSphere server... " & ex.Message)
            End Try
        Else
            'There were only two server, so use those going forward
        End If

        'WriteAuditEntry(Now.ToString & " >>> Down to two servers... " & ServerOne.Name & " and " & ServerTwo.Name)

        'Of the two remaining servers, pick the one with earliest scheduled time for next scan
        If Not (ServerTwo Is Nothing) Then
            timeOne = CDate(ServerOne.NextScan)
            timeTwo = CDate(ServerTwo.NextScan)

            If DateTime.Compare(timeOne, timeTwo) < 0 Then
                'time one is earlier than time two, so keep server 1
                SelectedServer = ServerOne
                tScheduled = CDate(ServerOne.NextScan)
            Else
                SelectedServer = ServerTwo
                tScheduled = CDate(ServerTwo.NextScan)
            End If
            tNow = Now
            '   WriteAuditEntry(Now.ToString & " >>> Down to one server... " & SelectedServer.Name & " to scan at " & SelectedServer.NextScan & ". Status is " & SelectedServer.Status)
        Else
            SelectedServer = ServerOne
            tScheduled = CDate(ServerOne.NextScan)
        End If

        tScheduled = CDate(SelectedServer.NextScan)
        If DateTime.Compare(tNow, tScheduled) < 0 Then
            If SelectedServer.Status <> "Not Scanned" Then
                '  WriteAuditEntry(Now.ToString & " No Domino servers scheduled for monitoring, next scan after " & SelectedServer.NextScan)
                SelectedServer = Nothing
            Else
                WriteAuditEntry(Now.ToString & " selected WebSphere server: " & SelectedServer.Name & " because it has not been scanned yet.")
            End If
        Else
            WriteAuditEntry(Now.ToString & " selected WebSphere server: " & SelectedServer.Name)
        End If

        'Release Memory
        tNow = Nothing
        tScheduled = Nothing
        n = Nothing

        timeOne = Nothing
        timeTwo = Nothing

        ServerOne = Nothing
        ServerTwo = Nothing

        'return selectedserver
        SelectWebSphereServerToMonitor = SelectedServer
        'Exit Function
        SelectedServer = Nothing
    End Function

    'Private Sub ScanWebSphereServer(Server As MonitoredItems.WebSphere)

    '	Dim WebSphereDLL As New VitalSignsWebSphereDLL.VitalSignsWebSphereDLL()


    '	Dim Cells As VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Cells_ServerStats = WebSphereDLL.getServerStats(Server)

    '	Server.ResponseDetails = "Ok"

    '	'go through all the stats and save it to the server object
    '	WriteAuditEntryWebSphere(Now.ToString & " Copying Values")
    '	CopyValuesToCollectionServers(Server, Cells.Cell.Nodes.Node.Servers.Server)

    '	'Send Alerts
    '	WriteAuditEntryWebSphere(Now.ToString & " Sending Alerts")
    '	SendWebSphereAlerts(Server)

    '	Server.AlertCondition = False

    '	Server.LastScan = Date.Now

    '	WriteAuditEntryWebSphere(Now.ToString & " Updating Status")
    '	UpdateWebSphereStatusTable(Server)

    '	'Server.Status = Status

    'End Sub

    Public Function GetAppServerListByCell(CellName As String, HostName As String, RealmName As String, Port As Integer, UserID As String, Password As String) As String
        'Should return XML in the format
        '<cell name="DmgrCell">
        '  <nodes>
        '    <node name="AppNode01" hostName="hostserver.nss.net">
        '      <servers>
        '         <server>BC_M1</server>
        '         <server>nodeagent</server>
        '         <server>testServer1</server>
        '         <server>server1</server>
        '      </servers>
        '   </node>
        '     <node name="hostserver2Node03" hostName="hostserver2.nss.net">
        '       <servers>
        '          <server>nodeagent</server>
        '          <server>BC_M2</server>
        '          <server>demoServer</server>
        '       </servers>
        '    </node>
        '  </nodes>
        '</cell>

        Dim strResults As String = ""
        Dim ProcessProperties As New ProcessStartInfo
        ProcessProperties.FileName = "java"
        ' ProcessProperties.Arguments = "-cp com.ibm.ws.admin.client_7.0.0.jar;xalan.jar;xercesImpl.jar;xml-apis.jar;serializer.jar;. ServerList “hostname” “RealmName” “Port#” “Uid” “Passwd”  'command line arguments
        dtSametimeLastUpdate = Now
        Dim Arguments As String = "-cp com.ibm.ws.admin.client_7.0.0.jar;xalan.jar;xercesImpl.jar;xml-apis.jar;serializer.jar;. ServerList *hostname* *RealmName* *Port#* *Uid* *Passwd* "
        Arguments = Arguments.Replace("*", Chr(34))   ' Replace the * with a "
        Arguments = Arguments.Replace("ServerList", CellName)
        Arguments = Arguments.Replace("RealmName", RealmName)
        Arguments = Arguments.Replace("Port#", Port)
        Arguments = Arguments.Replace("Uid", UserID)
        Arguments = Arguments.Replace("Passwd", Password)


        ProcessProperties.Arguments = Arguments
        ProcessProperties.WindowStyle = ProcessWindowStyle.Hidden

        Arguments = Arguments.Replace(Password, "********")

        WriteDeviceHistoryEntry("WebSphere", CellName, Now.ToString & "  Arguments to Java are " & Arguments)
        ProcessProperties.RedirectStandardOutput = True
        ProcessProperties.UseShellExecute = False
        ProcessProperties.WorkingDirectory = strAppPath


        Try
            Dim myProcess As Process = Process.Start(ProcessProperties)
            WriteDeviceHistoryEntry("WebSphere", CellName, Now.ToString & " Calling Process.Start")
            strResults = myProcess.StandardOutput.ReadToEnd()

            If Not myProcess.HasExited Then
                myProcess.Kill()
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("WebSphere", CellName, Now.ToString & " Error calling Process.start: " & ex.ToString)
            strResults = "Error"
        End Try

        Return strResults

    End Function
#End Region

#Region "IBM Connect Monitoring"


    'Ensure the error catching is working as intended (has not been fully tested)
    'alert types have to be added to SQL to be monitored or not
    'check the TestResponding and TestLogon functions (has not been fully tested)


    Public Shared Function ValidateRemoteCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function

    Private Sub MonitorIBMConnect()

        Dim WsDll As New VitalSignsWebSphereDLL.VitalSignsWebSphereDLL()
        'Dim WsDllForObjs As VitalSignsWebSphereDLL.VitalSignsWebSphereDLL
        ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate
        Dim adapter As New VSAdaptor()
        Do While boolTimeToStop <> True

            Try
                Dim myServer As MonitoredItems.IBMConnect
                myServer = SelectIBMConnectServerToMonitor()

                If myServer Is Nothing Then
                    CurrentIBMConnect = ""
                    GoTo CleanUp
                Else
                    '   WriteAuditEntry(Now.ToString & " >>> Selected " & myServer.Name)
                End If
                WriteAuditEntryIBMConnect(Now.ToString & " Scanning " & myServer.Name)
                myServer.Status = "OK"
                myServer.StatusCode = "OK"
                myServer.ResponseDetails = "This instance passed all tests"

                If TestIBMConnectResponding(myServer) = True Then

                    TestIMBConnectLogon(myServer)

                    If (myServer.TestCreateActivity) Then
                        TestCreateActivity(myServer)
                    Else
                        adapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", "DELETE FROM StatusDetail WHERE TypeANDName = '" & myServer.Name & "-IBM Connections' and TestName = 'Create Activity'")
                    End If

                    If (myServer.TestCreateBlog) Then
                        TestCreateBlog(myServer)
                    Else
                        adapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", "DELETE FROM StatusDetail WHERE TypeANDName = '" & myServer.Name & "-IBM Connections' and TestName = 'Create Blog'")
                    End If

                    If (myServer.TestCreateBookmarks) Then
                        TestCreateBookmarks(myServer)
                    Else
                        adapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", "DELETE FROM StatusDetail WHERE TypeANDName = '" & myServer.Name & "-IBM Connections' and TestName = 'Create Bookmark'")
                    End If

                    If (myServer.TestCreateCommunities) Then
                        TestCreateCommunities(myServer)
                    Else
                        adapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", "DELETE FROM StatusDetail WHERE TypeANDName = '" & myServer.Name & "-IBM Connections' and TestName = 'Create Community'")
                    End If

                    If (myServer.TestCreateFiles) Then
                        TestCreateFiles(myServer)
                    Else
                        adapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", "DELETE FROM StatusDetail WHERE TypeANDName = '" & myServer.Name & "-IBM Connections' and TestName = 'Create File'")
                    End If

                    If (myServer.TestCreateWikis) Then
                        TestCreateWikis(myServer)
                    Else
                        adapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", "DELETE FROM StatusDetail WHERE TypeANDName = '" & myServer.Name & "-IBM Connections' and TestName = 'Create Wiki'")
                    End If

                    If (myServer.TestSearchProfiles) Then
                        TestSearchProfiles(myServer)
                    Else
                        adapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", "DELETE FROM StatusDetail WHERE TypeANDName = '" & myServer.Name & "-IBM Connections' and TestName = 'Search Profiles'")
                    End If



                    'GetConnectionsStats(myServer)

                Else

                    myServer.Status = "Not Responding"
                    myServer.ResponseDetails = "This instance could not connect"

                End If

                myServer.LastScan = Date.Now

                UpdateIBMConenctStatusTable(myServer)


                myServer.IsBeingScanned = False
                WriteAuditEntryWebSphere(Now.ToString & " Done Scanning " & myServer.Name)
CleanUp:

            Catch ThreadEx As ThreadAbortException
                WriteAuditEntry(Now.ToString & " Aborting IBM Connect monitoring sub " & ThreadEx.Message)
                Exit Sub
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Exception IBM Connect monitoring sub " & ex.Message)
            End Try

            If MyIBMConnectServers.Count < 24 Then
                Thread.Sleep(2500)
            ElseIf MyIBMConnectServers.Count < 10 Then
                Thread.Sleep(10000)
            End If

            dtIBMConnectLastUpdate = Now

        Loop

    End Sub

    Private Sub SendIBMConnectNotRespondingAlert(ByRef server As MonitoredItems.IBMConnect, ByVal resetAlert As Boolean)

        With server

            If (resetAlert) Then
                myAlert.ResetAlert(.ServerType, .Name, "Not Responding", .Location, "The server is responding.", "IBMConnect")
            Else
                myAlert.QueueAlert(.ServerType, .Name, "Not Responding", "The server is not responding", .Location, "IBMConnect")
            End If

        End With

    End Sub

    Private Function SelectIBMConnectServerToMonitor() As MonitoredItems.IBMConnect
        'WriteAuditEntry(Now.ToString & " >>> Selecting a Domino Server for monitoring >>>>")
        Dim tNow As DateTime
        tNow = Now
        Dim tScheduled As DateTime

        Dim timeOne, timeTwo As DateTime

        Dim SelectedServer As MonitoredItems.IBMConnect

        Dim ServerOne As MonitoredItems.IBMConnect
        Dim ServerTwo As MonitoredItems.IBMConnect

        Dim myRegistry As New RegistryHandler

        Dim n As Integer
        Dim strSQL As String = ""
        Dim ServerType As String = "IBMConnections"
        Dim serverName As String = ""

        Try
            strSQL = "Select svalue from ScanSettings where sname = 'Scan" & ServerType & "ASAP'"
            Dim ds As New DataSet()
            ds.Tables.Add("ScanASAP")
            Dim objVSAdaptor As New VSAdaptor
            objVSAdaptor.FillDatasetAny("VitalSigns", "VitalSigns", strSQL, ds, "ScanASAP")

            For Each row As DataRow In ds.Tables("ScanASAP").Rows
                Try
                    serverName = row(0).ToString()
                Catch ex As Exception
                    Continue For
                End Try

                For n = 0 To MyIBMConnectServers.Count - 1
                    ServerOne = MyIBMConnectServers.Item(n)

                    If ServerOne.Name = serverName And ServerOne.IsBeingScanned = False And ServerOne.Enabled Then
                        'WriteAuditEntry(Now.ToString & " >>> " & serverName & " was marked 'Scan ASAP' so that will be scanned next.")
                        strSQL = "DELETE FROM ScanSettings where sname = 'Scan" & ServerType & "ASAP' and svalue='" & serverName & "'"
                        objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)

                        Return ServerOne
                        Exit Function

                    End If
                Next
            Next

        Catch ex As Exception

        End Try

        'Any server Not Scanned should be scanned right away.  Select the first one you encounter
        For n = 0 To MyIBMConnectServers.Count - 1
            ServerOne = MyIBMConnectServers.Item(n)

            If ServerOne.Status = "Not Scanned" Or ServerOne.Status = "Master Service Stopped." Then
                '        WriteAuditEntry(Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status)
                Return ServerOne
                Exit Function
            End If
        Next

        'start with the first two servers
        ServerOne = MyIBMConnectServers.Item(0)
        If MyIBMConnectServers.Count > 1 Then ServerTwo = MyIBMConnectServers.Item(1)

        'go through the remaining servers, see which one has the oldest (earliest) scheduled time
        If MyIBMConnectServers.Count > 2 Then
            Try
                For n = 2 To MyIBMConnectServers.Count - 1
                    '           WriteAuditEntry(Now.ToString & " N is " & n)
                    timeOne = CDate(ServerOne.NextScan)
                    timeTwo = CDate(ServerTwo.NextScan)
                    If DateTime.Compare(timeOne, timeTwo) < 0 Then
                        'time one is earlier than time two, so keep server 1
                        ServerTwo = MyIBMConnectServers.Item(n)
                    Else
                        'time two is later than time one, so keep server 2
                        ServerOne = MyIBMConnectServers.Item(n)
                    End If
                Next
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " >>> Error Selecting IBM Connect server... " & ex.Message)
            End Try
        Else
            'There were only two server, so use those going forward
        End If

        'WriteAuditEntry(Now.ToString & " >>> Down to two servers... " & ServerOne.Name & " and " & ServerTwo.Name)

        'Of the two remaining servers, pick the one with earliest scheduled time for next scan
        If Not (ServerTwo Is Nothing) Then
            timeOne = CDate(ServerOne.NextScan)
            timeTwo = CDate(ServerTwo.NextScan)

            If DateTime.Compare(timeOne, timeTwo) < 0 Then
                'time one is earlier than time two, so keep server 1
                SelectedServer = ServerOne
                tScheduled = CDate(ServerOne.NextScan)
            Else
                SelectedServer = ServerTwo
                tScheduled = CDate(ServerTwo.NextScan)
            End If
            tNow = Now
            '   WriteAuditEntry(Now.ToString & " >>> Down to one server... " & SelectedServer.Name & " to scan at " & SelectedServer.NextScan & ". Status is " & SelectedServer.Status)
        Else
            SelectedServer = ServerOne
            tScheduled = CDate(ServerOne.NextScan)
        End If

        tScheduled = CDate(SelectedServer.NextScan)
        If DateTime.Compare(tNow, tScheduled) < 0 Then
            If SelectedServer.Status <> "Not Scanned" Then
                '  WriteAuditEntry(Now.ToString & " No Domino servers scheduled for monitoring, next scan after " & SelectedServer.NextScan)
                SelectedServer = Nothing
            Else
                WriteAuditEntry(Now.ToString & " selected IBM Connect server: " & SelectedServer.Name & " because it has not been scanned yet.")
            End If
        Else
            WriteAuditEntry(Now.ToString & " selected IBM Connect server: " & SelectedServer.Name)
        End If

        'Release Memory
        tNow = Nothing
        tScheduled = Nothing
        n = Nothing

        timeOne = Nothing
        timeTwo = Nothing

        ServerOne = Nothing
        ServerTwo = Nothing

        'return selectedserver
        SelectIBMConnectServerToMonitor = SelectedServer
        'Exit Function
        SelectedServer = Nothing
    End Function

    Public Function Base64Encode(ByVal str As String)
        Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(str)
        Return System.Convert.ToBase64String(bytes)
    End Function

    Public Function GetEncodedUsernamePassword(ByVal user As String, ByVal pass As String)
        Return Base64Encode(user & ":" & pass)
    End Function

    Public Sub TestCreateActivity(ByRef myServer As MonitoredItems.IBMConnect)
        Try

            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " In TestCreateActivity", LogUtilities.LogUtils.LogLevel.Normal)
            Dim AlertType As String = "Create Activity"
            Dim URL As String = myServer.IPAddress
            Dim Username As String = myServer.UserName
            Dim Password As String = myServer.Password
            Dim TestThreshold As Int32 = myServer.CreateActivityThreshold

            Dim alertReset As Boolean

            Dim ActivityName As String = "VitalSigns Test Activity"
            Dim activityURL As String = URL + "/activities/service/atom2/activities"
            Dim activityBody As String = "<?xml version=""1.0"" encoding=""utf-8""?><entry xmlns=""http://www.w3.org/2005/Atom""><category scheme=""http://www.ibm.com/xmlns/prod/sn/type"" term=""activity"" label=""Activity""/><title type=""text"">" & ActivityName & "</title><content type=""html"">This is an activity</content></entry>"

            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Will first try deleting all undeleted VitalSigns Activities", LogUtilities.LogUtils.LogLevel.Normal)
            CleanAllVitalSignActivities(myServer)
            'activities/service/atom2/activities?title=VitalSigns%20Test%20Activity


            Dim httpWR As HttpWebRequest = WebRequest.Create(activityURL)
            httpWR.Timeout = 60000
            httpWR.Method = "POST"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/atom+xml"
            httpWR.Accept = "*/*"
            httpWR.Headers.Add("Authorization", "Basic " & GetEncodedUsernamePassword(Username, Password))

            Dim byteArr As Byte() = System.Text.Encoding.ASCII.GetBytes(activityBody.ToString())
            httpWR.ContentLength = byteArr.Length
            Dim dataStream As Stream = httpWR.GetRequestStream()
            dataStream.Write(byteArr, 0, byteArr.Length)
            dataStream.Close()
            Dim cookieContainer As New CookieContainer()
            httpWR.CookieContainer = cookieContainer


            Dim webResponse As HttpWebResponse

            Try
                Dim startTime As DateTime = DateTime.Now
                webResponse = httpWR.GetResponse()
                Dim endTime As DateTime = DateTime.Now
                Dim span As TimeSpan = endTime - startTime
                Dim createTime As Double = Math.Round(span.TotalMilliseconds, 1)

                If (webResponse.StatusCode = HttpStatusCode.Created) Then
                    'Created Correctly...do things
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Created activity in " & createTime & " ms", LogUtilities.LogUtils.LogLevel.Normal)
                    alertReset = True
                    InsertIntoIBMConnectionsDailyStats(myServer.ServerName, "Create.Activity.TimeMs", createTime.ToString())
                Else
                    'Created wrongly...do things
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Activity failed to create. It took " & createTime & " ms and produced a status code of " & webResponse.StatusCode & " and description of " & webResponse.StatusDescription & ".", LogUtilities.LogUtils.LogLevel.Normal)
                    alertReset = False

                    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The activity was not created. It produced a status code of " & webResponse.StatusCode & " and a description of " & webResponse.StatusDescription & ".", myServer.Location)

                    If myServer.StatusCode = "OK" Then
                        myServer.StatusCode = "Issue"
                        myServer.Status = "Issue"
                        myServer.ResponseDetails = "The activity was not created. It produced a status code of " & webResponse.StatusCode & " and a description of " & webResponse.StatusDescription & "."
                    End If
                End If

                If (webResponse.StatusCode = HttpStatusCode.Created) Then

                    Dim actString As String = webResponse.ResponseUri.AbsolutePath
                    Dim actDS As Stream = webResponse.GetResponseStream()
                    Dim actReader As StreamReader = New StreamReader(actDS)
                    Dim resposne As String = actReader.ReadToEnd()

                    Dim k As Int32 = resposne.LastIndexOf("<link rel=""edit""")
                    k = resposne.IndexOf("href=""", k)
                    Dim m As Int32 = 0
                    If Not (k = 0) Then
                        m = resposne.IndexOf(""" />", k + 7)
                    End If
                    Dim deleteString As String = resposne.Substring(k + 6, m - (k + 6))

                    httpWR = WebRequest.Create(deleteString)
                    httpWR.Timeout = 6000000
                    httpWR.Method = "DELETE"
                    httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
                    httpWR.ContentType = "application/atom+xml"
                    httpWR.Accept = "*/*"
                    'httpWR.Headers.Add("Authorization", "Basic d3N0YW51bGlzOldzMTMxNTU3MDIh")

                    httpWR.CookieContainer = cookieContainer

                    Dim webResponse2 As HttpWebResponse

                    Try
                        webResponse2 = httpWR.GetResponse()

                        If (webResponse2.StatusCode = HttpStatusCode.NoContent) Then
                            'It deleted...do nothing
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Deleted activity.", LogUtilities.LogUtils.LogLevel.Normal)
                            'If TestThreshold > createTime Then
                            '    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The activity was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)
                            'Else
                            '    myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The activity was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                            'End If

                        Else
                            'It failed to delete...send alert
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Failed to delete activity.", LogUtilities.LogUtils.LogLevel.Normal)
                            'myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The activity was successfully created in " & createTime & " ms.")

                            If myServer.StatusCode = "OK" Then
                                'myServer.StatusCode = "Issue"
                                'myServer.ResponseDetails = "The activity was created but failed to be deleted. It produced a status code of " & webResponse2.StatusCode & " and a description of " & webResponse2.StatusDescription & "."
                            End If
                        End If

                    Catch ex As Exception
                        WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to delete activity due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                        'myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The activity was successfully created in " & createTime & " ms but failed to delete.")

                        'If TestThreshold > createTime Then
                        '    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The activity was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)
                        'Else
                        '    myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The activity was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                        'End If

                        If myServer.StatusCode = "OK" Then
                            'myServer.StatusCode = "Issue"
                            'myServer.ResponseDetails = "The activity was created but failed to be deleted. It produced a status code of " & webResponse2.StatusCode & " and a description of " & webResponse2.StatusDescription & "."
                        End If
                    Finally
                        If webResponse2 IsNot Nothing Then
                            webResponse2.Close()
                        End If

                        If TestThreshold < createTime Then
                            myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The activity was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)

                            If myServer.StatusCode = "OK" Then
                                myServer.StatusCode = "Issue"
                                myServer.Status = "Issue"
                                myServer.ResponseDetails = "The activity was successfully created in " & createTime & " ms but has a threshold of " & TestThreshold & " ms."
                            End If

                        Else
                            myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The activity was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                        End If

                    End Try
                End If

            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to create activity due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The activity was not created.", myServer.Location)

                If myServer.StatusCode = "OK" Then
                    myServer.StatusCode = "Issue"
                    myServer.Status = "Issue"
                    myServer.ResponseDetails = "The activity was not created."
                End If
            Finally
                If webResponse IsNot Nothing Then
                    webResponse.Close()
                End If
            End Try
        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error in TestCreateActivity. Error: " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try
    End Sub

    Public Sub TestCreateBlog(ByRef myServer As MonitoredItems.IBMConnect)
        Try
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " In TestCreateBlog", LogUtilities.LogUtils.LogLevel.Normal)
            Dim AlertType As String = "Create Blog"
            Dim alertReset As Boolean

            Dim URL As String = myServer.IPAddress
            Dim Username As String = myServer.UserName
            Dim Password As String = myServer.Password
            Dim TestThreshold As Int32 = myServer.CreateBlogThreshold

            Dim Name As String = "VitalSigns Test Blog"
            URL = URL + "/blogs/homepage/api/blogs"
            Dim Body As String = "<?xml version=""1.0"" encoding=""UTF-8""?><entry xmlns:snx=""http://www.ibm.com/xmlns/prod/sn"" xmlns=""http://www.w3.org/2005/Atom""><title type=""text"">" & Name & "</title><category term=""watchit""/><snx:handle>watchit</snx:handle></entry>"""

            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Will first try deleting all undeleted VitalSigns Blog", LogUtilities.LogUtils.LogLevel.Normal)
            CleanAllVitalSignBlogs(myServer)

            Dim httpWR As HttpWebRequest = WebRequest.Create(URL)
            httpWR.Timeout = 60000
            httpWR.Method = "POST"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/atom+xml"
            httpWR.Accept = "*/*"
            httpWR.Headers.Add("Authorization", "Basic " & GetEncodedUsernamePassword(Username, Password))

            Dim byteArr As Byte() = System.Text.Encoding.ASCII.GetBytes(Body.ToString())
            httpWR.ContentLength = byteArr.Length
            Dim dataStream As Stream = httpWR.GetRequestStream()
            dataStream.Write(byteArr, 0, byteArr.Length)
            dataStream.Close()
            Dim cookieContainer As New CookieContainer()
            httpWR.CookieContainer = cookieContainer

            Dim webResponse As HttpWebResponse

            Try
                Dim startTime As DateTime = DateTime.Now
                webResponse = httpWR.GetResponse()
                Dim endTime As DateTime = DateTime.Now
                Dim span As TimeSpan = endTime - startTime
                Dim createTime As Double = Math.Round(span.TotalMilliseconds, 1)

                If (webResponse.StatusCode = HttpStatusCode.Created) Then
                    'Created Correctly...do things
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Created blog in " & createTime & " ms.", LogUtilities.LogUtils.LogLevel.Normal)
                    alertReset = True
                    InsertIntoIBMConnectionsDailyStats(myServer.ServerName, "Create.Blog.TimeMs", createTime.ToString())
                Else
                    'Created wrongly...do things
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Blog failed to create. It took " & createTime & " ms and produced a status code of " & webResponse.StatusCode & " and description of " & webResponse.StatusDescription & ".", LogUtilities.LogUtils.LogLevel.Normal)
                    alertReset = False

                    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The blog was not created. It produced a status code of " & webResponse.StatusCode & " and a description of " & webResponse.StatusDescription & ".", myServer.Location)

                    If myServer.StatusCode = "OK" Then
                        myServer.StatusCode = "Issue"
                        myServer.Status = "Issue"
                        myServer.ResponseDetails = "The blog was not created. It produced a status code of " & webResponse.StatusCode & " and a description of " & webResponse.StatusDescription & "."
                    End If
                End If

                If (webResponse.StatusCode = HttpStatusCode.Created) Then

                    Dim actString As String = webResponse.ResponseUri.AbsolutePath
                    Dim actDS As Stream = webResponse.GetResponseStream()
                    Dim actReader As StreamReader = New StreamReader(actDS)
                    Dim resposne As String = actReader.ReadToEnd()

                    Dim xmlDoc As New Xml.XmlDocument()
                    xmlDoc.LoadXml(resposne)

                    Dim deleteString As String = ""

                    For Each xmlElement As Xml.XmlElement In xmlDoc.Item("entry")
                        If xmlElement.Name = "link" Then
                            If xmlElement.Attributes("href").Value.ToString().Contains("/blogs/homepage/api/blogs") Then
                                deleteString = xmlElement.Attributes("href").Value.ToString()
                                Exit For
                            End If
                        End If
                    Next


                    httpWR = WebRequest.Create(deleteString)
                    httpWR.Timeout = 6000000
                    httpWR.Method = "DELETE"
                    httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
                    httpWR.ContentType = "application/atom+xml"
                    httpWR.Accept = "*/*"
                    'httpWR.Headers.Add("Authorization", "Basic d3N0YW51bGlzOldzMTMxNTU3MDIh")

                    httpWR.CookieContainer = cookieContainer

                    Dim webResponse2 As HttpWebResponse

                    Try
                        webResponse2 = httpWR.GetResponse()

                        If (webResponse2.StatusCode = HttpStatusCode.NoContent) Then
                            'It deleted...do nothing
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Deleted blog.", LogUtilities.LogUtils.LogLevel.Normal)

                            'If TestThreshold > createTime Then
                            '    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The activity was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)
                            'Else
                            '    myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The activity was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                            'End If
                        Else
                            'It failed to delete...send alert
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Failed to delete blog.", LogUtilities.LogUtils.LogLevel.Normal)
                            'myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The blog was successfully created in " & createTime & " ms.")

                            If myServer.StatusCode = "OK" Then
                                'myServer.StatusCode = "Issue"
                                'myServer.ResponseDetails = "The blog was created but failed to be deleted. It produced a status code of " & webResponse2.StatusCode & " and a description of " & webResponse2.StatusDescription & "."
                            End If
                        End If

                    Catch ex As Exception
                        WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to delete blog due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                        'myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The blog was successfully created in " & createTime & " ms but failed to delete.")

                        'If TestThreshold > createTime Then
                        '    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The activity was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)
                        'Else
                        '    myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The activity was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                        'End If

                        If myServer.StatusCode = "OK" Then
                            'myServer.StatusCode = "Issue"
                            'myServer.ResponseDetails = "The blog was created but failed to be deleted."
                        End If
                    Finally
                        If webResponse2 IsNot Nothing Then
                            webResponse2.Close()
                        End If

                        If TestThreshold < createTime Then
                            myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The activity was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)

                            If myServer.StatusCode = "OK" Then
                                myServer.StatusCode = "Issue"
                                myServer.Status = "Issue"
                                myServer.ResponseDetails = "The blog was successfully created in " & createTime & " ms but has a threshold of " & TestThreshold & " ms."
                            End If

                        Else
                            myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The activity was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                        End If

                    End Try

                End If

            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to create blog due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The blog was not created.", myServer.Location)

                If myServer.StatusCode = "OK" Then
                    myServer.StatusCode = "Issue"
                    myServer.Status = "Issue"
                    myServer.ResponseDetails = "The blog was not created."
                End If
            Finally
                If webResponse IsNot Nothing Then
                    webResponse.Close()
                End If
            End Try
        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error in TestCreateBlog. Error: " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try

    End Sub

    Public Sub TestCreateBookmarks(ByRef myServer As MonitoredItems.IBMConnect)
        Try
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " In TestCreateBookmarks", LogUtilities.LogUtils.LogLevel.Normal)
            Dim AlertType As String = "Create Bookmark"
            Dim alertReset As Boolean

            Dim URL As String = myServer.IPAddress
            Dim Username As String = myServer.UserName
            Dim Password As String = myServer.Password
            Dim TestThreshold As Int32 = myServer.CreateBookmarkThreshold

            Dim Name As String = "http://www.google.com"
            URL = URL + "/dogear/api/app"
            Dim Body As String = "<?xml version=""1.0"" encoding=""utf-8""?>\r\n<entry xmlns=""http://www.w3.org/2005/Atom"">\r\n<author><name>Author</name></author>\r\n<title>VitalSigns Test BookMark</title>\r\n<content type=""html""><![CDATA[VitalSigns Creating Bookmark]]></content>\r\n<category scheme=""http://www.ibm.com/xmlns/prod/sn/type""  term=""bookmark"" />\r\n<category term=""lotus"" />\r\n<category term=""connections"" />\r\n<category term=""VitalSigns Testing"" />\r\n<link href=""" & Name & """ />\r\n" + "</entry>\r\n"
            Dim t As String = "<?xml version=""1.0"" encoding=""utf-8""?><entry xmlns=""http://www.w3.org/2005/Atom""><author><name>Author</name></author><title>VitalSigns Test BookMark Title</title><content type=""html""><![CDATA[VitalSigns Creating Bookmark]]></content><category scheme=""http://www.ibm.com/xmlns/prod/sn/type""  term=""bookmark"" /><category term=""lotus"" /><category term=""connections"" /><category term=""VitalSigns Testing"" /><link href=""" & Name & """ />" + "</entry>"

            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Will first try deleting all undeleted VitalSigns Bookmarks", LogUtilities.LogUtils.LogLevel.Normal)
            CleanAllVitalSignBookmarks(myServer)

            Dim httpWR As HttpWebRequest = WebRequest.Create(URL)
            httpWR.Timeout = 60000
            httpWR.Method = "POST"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/atom+xml"
            httpWR.Accept = "*/*"
            httpWR.Headers.Add("Authorization", "Basic " & GetEncodedUsernamePassword(Username, Password))

            Dim byteArr As Byte() = System.Text.Encoding.ASCII.GetBytes(t.ToString())
            httpWR.ContentLength = byteArr.Length
            Dim dataStream As Stream = httpWR.GetRequestStream()
            dataStream.Write(byteArr, 0, byteArr.Length)
            dataStream.Close()
            Dim cookieContainer As New CookieContainer()
            httpWR.CookieContainer = cookieContainer

            Dim webResposne As HttpWebResponse

            Try
                Dim startTime As DateTime = DateTime.Now
                webResposne = httpWR.GetResponse()
                Dim endTime As DateTime = DateTime.Now
                Dim span As TimeSpan = endTime - startTime
                Dim createTime As Double = Math.Round(span.TotalMilliseconds, 1)

                If (webResposne.StatusCode = HttpStatusCode.Created) Then
                    'Created Correctly...do things
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Created bookmark in " & createTime & " ms.", LogUtilities.LogUtils.LogLevel.Normal)
                    alertReset = True
                    InsertIntoIBMConnectionsDailyStats(myServer.ServerName, "Create.Bookmark.TimeMs", createTime.ToString())
                Else
                    'Created wrongly...do things
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Bookmark failed to create. It took " & createTime & " ms and produced a status code of " & webResposne.StatusCode & " and description of " & webResposne.StatusDescription & ".", LogUtilities.LogUtils.LogLevel.Normal)
                    alertReset = False

                    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The bookmark was not created. It produced a status code of " & webResposne.StatusCode & " and a description of " & webResposne.StatusDescription & ".", myServer.Location)

                    If myServer.StatusCode = "OK" Then
                        myServer.StatusCode = "Issue"
                        myServer.Status = "Issue"
                        myServer.ResponseDetails = "The bookmark was not created. It produced a status code of " & webResposne.StatusCode & " and a description of " & webResposne.StatusDescription & "."
                    End If
                End If

                If (webResposne.StatusCode = HttpStatusCode.Created) Then

                    Dim deleteString As String = URL & "?url=" & Name

                    httpWR = WebRequest.Create(deleteString)
                    httpWR.Timeout = 6000000
                    httpWR.Method = "DELETE"
                    httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
                    httpWR.ContentType = "application/atom+xml"
                    httpWR.Accept = "*/*"
                    'httpWR.Headers.Add("Authorization", "Basic d3N0YW51bGlzOldzMTMxNTU3MDIh")

                    httpWR.CookieContainer = cookieContainer

                    Dim webResponse2 As HttpWebResponse

                    Try
                        webResponse2 = httpWR.GetResponse()

                        If (webResponse2.StatusCode = HttpStatusCode.NoContent) Then
                            'It deleted...do nothing
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Deleted bookmark.", LogUtilities.LogUtils.LogLevel.Normal)
                            'If TestThreshold > createTime Then
                            '    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The activity was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)
                            'Else
                            '    myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The activity was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                            'End If
                        Else
                            'It failed to delete...send alert
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Failed to delete bookmark.", LogUtilities.LogUtils.LogLevel.Normal)
                            'myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The bookmark was successfully created in " & createTime & " ms but failed to delete.")

                            If myServer.StatusCode = "OK" Then
                                'myServer.StatusCode = "Issue"
                                'myServer.ResponseDetails = "The bookmark was created but failed to be deleted. It produced a status code of " & webResposne.StatusCode & " and a description of " & webResposne.StatusDescription & "."
                            End If
                        End If

                    Catch ex As Exception
                        WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to delete bookmark due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                        'myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The bookmark was successfully created in " & createTime & " ms but failed to delete.")

                        If myServer.StatusCode = "OK" Then
                            'myServer.StatusCode = "Issue"
                            'myServer.ResponseDetails = "The bookmark was created but failed to be deleted."
                        End If

                    Finally
                        If webResponse2 IsNot Nothing Then
                            webResponse2.Close()
                        End If

                        If TestThreshold < createTime Then
                            myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The activity was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)

                            If myServer.StatusCode = "OK" Then
                                myServer.StatusCode = "Issue"
                                myServer.Status = "Issue"
                                myServer.ResponseDetails = "The bookmark was successfully created in " & createTime & " ms but has a threshold of " & TestThreshold & " ms."
                            End If

                        Else
                            myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The activity was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                        End If

                    End Try

                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to create bookmark due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The bookmark was not created.", myServer.Location)

                If myServer.StatusCode = "OK" Then
                    myServer.StatusCode = "Issue"
                    myServer.Status = "Issue"
                    myServer.ResponseDetails = "The bookmark failed to be created."
                End If

            Finally
                If webResposne IsNot Nothing Then
                    webResposne.Close()
                End If
            End Try
        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error in TestCreateBookmark. Error: " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try


    End Sub

    Public Sub TestCreateCommunities(ByRef myServer As MonitoredItems.IBMConnect)
        Try
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " In TestCreateCommunities", LogUtilities.LogUtils.LogLevel.Normal)
            Dim AlertType As String = "Create Community"
            Dim alertReset As Boolean

            Dim URLBase As String = myServer.IPAddress
            Dim Username As String = myServer.UserName
            Dim Password As String = myServer.Password
            Dim TestThreshold As Int32 = myServer.CreateCommunitiesThreshold

            Dim Name As String = "VitalSigns Test Community"
            Dim URL As String = URLBase + "/communities/service/atom/communities/my"
            Dim Body As String = "<?xml version=""1.0"" encoding=""utf-8""?><entry xmlns=""http://www.w3.org/2005/Atom"" xmlns:app=""http://www.w3.org/2007/app""  xmlns:snx=""http://www.ibm.com/xmlns/prod/sn""><title type=""text"">" + Name + "</title><content type=""html"">Test Community</content><category term=""community"" scheme=""http://www.ibm.com/xmlns/prod/sn/type""></category><snx:communityType>public</snx:communityType></entry>"

            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Will first try deleting all undeleted VitalSigns Communities", LogUtilities.LogUtils.LogLevel.Normal)
            CleanAllVitalSignCommunities(myServer)

            Dim httpWR As HttpWebRequest = WebRequest.Create(URL)
            httpWR.Timeout = 60000
            httpWR.Method = "POST"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/atom+xml"
            httpWR.Accept = "*/*"
            httpWR.Headers.Add("Authorization", "Basic " & GetEncodedUsernamePassword(Username, Password))

            Dim byteArr As Byte() = System.Text.Encoding.ASCII.GetBytes(Body.ToString())
            httpWR.ContentLength = byteArr.Length
            Dim dataStream As Stream = httpWR.GetRequestStream()
            dataStream.Write(byteArr, 0, byteArr.Length)
            dataStream.Close()
            Dim cookieContainer As New CookieContainer()
            httpWR.CookieContainer = cookieContainer

            Dim webResposne As HttpWebResponse

            Try
                Dim startTime As DateTime = DateTime.Now
                webResposne = httpWR.GetResponse()
                Dim endTime As DateTime = DateTime.Now
                Dim span As TimeSpan = endTime - startTime
                Dim createTime As Double = Math.Round(span.TotalMilliseconds, 1)

                If (webResposne.StatusCode = HttpStatusCode.Created) Then
                    'Created Correctly...do things
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Created community in " & createTime & " ms.", LogUtilities.LogUtils.LogLevel.Normal)
                    alertReset = True
                    InsertIntoIBMConnectionsDailyStats(myServer.ServerName, "Create.Community.TimeMs", createTime.ToString())
                Else
                    'Created wrongly...do things
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Community failed to create. It took " & createTime & " ms and produced a status code of " & webResposne.StatusCode & " and description of " & webResposne.StatusDescription & ".", LogUtilities.LogUtils.LogLevel.Normal)
                    alertReset = False

                    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The community was not created. It produced a status code of " & webResposne.StatusCode & " and a description of " & webResposne.StatusDescription & ".", myServer.Location)

                    If myServer.StatusCode = "OK" Then
                        myServer.StatusCode = "Issue"
                        myServer.Status = "Issue"
                        myServer.ResponseDetails = "The community was not created. It produced a status code of " & webResposne.StatusCode & " and a description of " & webResposne.StatusDescription & "."
                    End If
                End If


                If (webResposne.StatusCode = HttpStatusCode.Created) Then

                    Dim headers As System.Net.WebHeaderCollection
                    headers = webResposne.Headers


                    Dim deleteString As String = headers.Get("Location")

                    httpWR = WebRequest.Create(deleteString)
                    httpWR.Timeout = 6000000
                    httpWR.Method = "DELETE"
                    httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
                    httpWR.ContentType = "application/atom+xml"
                    httpWR.Accept = "*/*"
                    'httpWR.Headers.Add("Authorization", "Basic d3N0YW51bGlzOldzMTMxNTU3MDIh")

                    httpWR.CookieContainer = cookieContainer

                    Dim webResponse2 As HttpWebResponse

                    Try
                        webResponse2 = httpWR.GetResponse()

                        If (webResponse2.StatusCode = HttpStatusCode.NoContent) Then
                            'It deleted...do nothing
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Deleted community.", LogUtilities.LogUtils.LogLevel.Normal)

                            'If TestThreshold > createTime Then
                            '    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The activity was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)
                            'Else
                            '    myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The activity was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                            'End If
                        Else
                            'It failed to delete...send alert
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Failed to delete community.", LogUtilities.LogUtils.LogLevel.Normal)
                            'myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The community was successfully created in " & createTime & " ms but failed to delete.")

                            If myServer.StatusCode = "OK" Then
                                'myServer.StatusCode = "Issue"
                                'myServer.ResponseDetails = "The community was successfully created in " & createTime & " ms but failed to delete."
                            End If
                        End If

                    Catch ex As Exception
                        WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to delete community due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                        'myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The community was successfully created in " & createTime & " ms but failed to delete.")

                        If myServer.StatusCode = "OK" Then
                            'myServer.StatusCode = "Issue"
                            'myServer.ResponseDetails = "The community was successfully created in " & createTime & " ms but failed to delete."
                        End If

                    Finally
                        If webResponse2 IsNot Nothing Then
                            webResponse2.Close()
                        End If

                        If TestThreshold < createTime Then
                            myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The activity was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)

                            If myServer.StatusCode = "OK" Then
                                myServer.StatusCode = "Issue"
                                myServer.Status = "Issue"
                                myServer.ResponseDetails = "The community was successfully created in " & createTime & " ms but has a threshold of " & TestThreshold & " ms."
                            End If

                        Else
                            myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The activity was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                        End If

                    End Try

                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to create community due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The community was not created.", myServer.Location)
                If myServer.StatusCode = "OK" Then
                    myServer.StatusCode = "Issue"
                    myServer.Status = "Issue"
                    myServer.ResponseDetails = "The community was not created."
                End If
            Finally
                If webResposne IsNot Nothing Then
                    webResposne.Close()
                End If
            End Try
        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error in TestCreateCommunities. Error: " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try

    End Sub

    Public Sub TestCreateFiles(ByRef myServer As MonitoredItems.IBMConnect)
        Try
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " In TestCreateFiles", LogUtilities.LogUtils.LogLevel.Normal)
            Dim AlertType As String = "Create File"
            Dim alertReset As Boolean

            Dim URL As String = myServer.IPAddress
            Dim Username As String = myServer.UserName
            Dim Password As String = myServer.Password
            Dim TestThreshold As Int32 = myServer.CreateFilesThreshold

            Dim Name As String = "VitalSignsTestFiles.txt"
            Dim GetURL As String = URL + "/files/basic/api/nonce"
            URL = URL + "/files/basic/api/myuserlibrary/feed"

            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Will first try deleting all undeleted VitalSigns Files", LogUtilities.LogUtils.LogLevel.Normal)
            CleanAllVitalSignFiles(myServer)

            Dim httpWR As HttpWebRequest = WebRequest.Create(GetURL)
            httpWR.Timeout = 60000
            httpWR.Method = "Get"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/atom+xml"
            httpWR.Accept = "*/*"
            httpWR.Headers.Add("Authorization", "Basic " & GetEncodedUsernamePassword(Username, Password))

            Dim cookieContainer As New CookieContainer()
            httpWR.CookieContainer = cookieContainer

            Dim webResponse As HttpWebResponse
            Dim response As String = ""

            Try
                webResponse = httpWR.GetResponse()
                Dim ds As Stream = webResponse.GetResponseStream()
                Dim reader As StreamReader = New StreamReader(ds)
                response = reader.ReadToEnd()
            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Exception getting Nonce. " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
            Finally
                If webResponse IsNot Nothing Then
                    webResponse.Close()
                End If
            End Try

            httpWR = WebRequest.Create(URL)
            httpWR.Timeout = 6000000
            httpWR.Method = "POST"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "text/plain"
            httpWR.Accept = "*/*"
            httpWR.CookieContainer = cookieContainer
            httpWR.Headers.Add("X-Update-Nonce", response)
            httpWR.Headers.Add("SLUG", Name)

            Try
                Dim startTime As DateTime = DateTime.Now
                webResponse = httpWR.GetResponse()
                Dim endTime As DateTime = DateTime.Now
                Dim span As TimeSpan = endTime - startTime
                Dim createTime As Double = Math.Round(span.TotalMilliseconds, 1)

                If (webResponse.StatusCode = HttpStatusCode.Created) Then
                    'Created Correctly...do things
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Created file in " & createTime & " ms.", LogUtilities.LogUtils.LogLevel.Normal)
                    alertReset = True
                    InsertIntoIBMConnectionsDailyStats(myServer.ServerName, "Create.File.TimeMs", createTime.ToString())
                Else
                    'Created wrongly...do things
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " File failed to create. It took " & createTime & " ms and produced a status code of " & webResponse.StatusCode & " and description of " & webResponse.StatusDescription & ".", LogUtilities.LogUtils.LogLevel.Normal)
                    alertReset = False

                    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The file was not created. It produced a status code of " & webResponse.StatusCode & " and a description of " & webResponse.StatusDescription & ".", myServer.Location)
                    If myServer.StatusCode = "OK" Then
                        myServer.StatusCode = "Issue"
                        myServer.Status = "Issue"
                        myServer.ResponseDetails = "The file was not created. It produced a status code of " & webResponse.StatusCode & " and a description of " & webResponse.StatusDescription & "."
                    End If
                End If

                If (webResponse.StatusCode = HttpStatusCode.Created) Then

                    Dim headers As System.Net.WebHeaderCollection
                    headers = webResponse.Headers


                    Dim deleteString As String = headers.Get("Location")

                    httpWR = WebRequest.Create(deleteString)
                    httpWR.Timeout = 6000000
                    httpWR.Method = "DELETE"
                    httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
                    httpWR.ContentType = "application/atom+xml"
                    httpWR.Accept = "*/*"
                    'httpWR.Headers.Add("Authorization", "Basic d3N0YW51bGlzOldzMTMxNTU3MDIh")

                    httpWR.CookieContainer = cookieContainer
                    Dim webResponse2 As HttpWebResponse

                    Try
                        webResponse2 = httpWR.GetResponse()

                        If (webResponse2.StatusCode = HttpStatusCode.NoContent) Then
                            'It deleted...do nothing
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Deleted file.", LogUtilities.LogUtils.LogLevel.Normal)

                            'If TestThreshold > createTime Then
                            '    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The activity was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)
                            'Else
                            '    myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The activity was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                            'End If
                        Else
                            'It failed to delete...send alert
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Failed to delete file.", LogUtilities.LogUtils.LogLevel.Normal)
                            'myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The file was successfully created in " & createTime & " ms but failed to delete.")

                            If myServer.StatusCode = "OK" Then
                                'myServer.StatusCode = "Issue"
                                'myServer.ResponseDetails = "The file was successfully created in " & createTime & " ms but failed to delete."
                            End If
                        End If

                    Catch ex As Exception
                        WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to delete file due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                        'myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The file was successfully created in " & createTime & " ms but failed to delete.")

                        If myServer.StatusCode = "OK" Then
                            'myServer.StatusCode = "Issue"
                            'myServer.ResponseDetails = "The file was successfully created in " & createTime & " ms but failed to delete."
                        End If
                    Finally
                        If webResponse2 IsNot Nothing Then
                            webResponse2.Close()
                        End If

                        If TestThreshold < createTime Then
                            myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The activity was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)

                            If myServer.StatusCode = "OK" Then
                                myServer.StatusCode = "Issue"
                                myServer.Status = "Issue"
                                myServer.ResponseDetails = "The file was successfully created in " & createTime & " ms but has a threshold of " & TestThreshold & " ms."
                            End If

                        Else
                            myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The activity was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                        End If

                    End Try

                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to create file due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The file was not created.", myServer.Location)

                If myServer.StatusCode = "OK" Then
                    myServer.StatusCode = "Issue"
                    myServer.Status = "Issue"
                    myServer.ResponseDetails = "The file was not created."
                End If
            Finally
                If webResponse IsNot Nothing Then
                    webResponse.Close()
                End If
            End Try
        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error in TestCreateFiles. Error: " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try

    End Sub

    Public Sub TestCreateforums(ByRef myServer As MonitoredItems.IBMConnect)
        Try
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " In TestCreateForums", LogUtilities.LogUtils.LogLevel.Normal)
            Dim AlertType As String = "Create forum"
            Dim alertReset As Boolean

            Dim URLBase As String = myServer.IPAddress
            Dim Username As String = myServer.UserName
            Dim Password As String = myServer.Password
            Dim TestThreshold As Int32 = myServer.CreateForumsThreshold

            Dim Name As String = "VitalSigns Test Community"
            Dim URL As String = URLBase + "/communities/service/atom/communities/my"
            Dim Body As String = "<?xml version=""1.0"" encoding=""utf-8""?><entry xmlns=""http://www.w3.org/2005/Atom"" xmlns:app=""http://www.w3.org/2007/app""  xmlns:snx=""http://www.ibm.com/xmlns/prod/sn""><title type=""text"">" + Name + "</title><content type=""html"">Test Community</content><category term=""community"" scheme=""http://www.ibm.com/xmlns/prod/sn/type""></category><snx:communityType>public</snx:communityType></entry>"
            Dim ForumsURL As String = URLBase & "/communities/service/atom/community/forum/topics"
            Dim ForumsBody As String = "<?xml version=""1.0"" encoding=""utf-8""?><entry xmlns=""http://www.w3.org/2005/Atom""><title type=""text"">VitalSigns Test Forum</title><content type=""html"">VitalSigns Test Forum</content><category scheme=""http://www.ibm.com/xmlns/prod/sn/type"" term=""forum-topic""></category></entry>"

            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Will first try deleting all undeleted VitalSigns Forums", LogUtilities.LogUtils.LogLevel.Normal)
            CleanAllVitalSignCommunities(myServer)

            Dim httpWR As HttpWebRequest = WebRequest.Create(URL)
            httpWR.Timeout = 60000
            httpWR.Method = "POST"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/atom+xml"
            httpWR.Accept = "*/*"
            httpWR.Headers.Add("Authorization", "Basic " & GetEncodedUsernamePassword(Username, Password))

            Dim byteArr As Byte() = System.Text.Encoding.ASCII.GetBytes(Body.ToString())
            httpWR.ContentLength = byteArr.Length
            Dim dataStream As Stream = httpWR.GetRequestStream()
            dataStream.Write(byteArr, 0, byteArr.Length)
            dataStream.Close()
            Dim cookieContainer As New CookieContainer()
            httpWR.CookieContainer = cookieContainer

            Dim webResponse As HttpWebResponse
            Try
                webResponse = httpWR.GetResponse()

                If (webResponse.StatusCode = HttpStatusCode.Created) Then
                    'Created Correctly...do nothing
                Else
                    'Created wrongly...do things
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Commuinity for the forum failed to create. It produced a status code of " & webResponse.StatusCode & " and description of " & webResponse.StatusDescription & ".", LogUtilities.LogUtils.LogLevel.Normal)

                    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The community for the forum was not created. It produced a status code of " & webResponse.StatusCode & " and a description of " & webResponse.StatusDescription & ".", myServer.Location)

                    If myServer.StatusCode = "OK" Then
                        myServer.StatusCode = "Issue"
                        myServer.Status = "Issue"
                        myServer.ResponseDetails = "The community for the forum was not created. It produced a status code of " & webResponse.StatusCode & " and a description of " & webResponse.StatusDescription & "."
                    End If
                End If

                If (webResponse.StatusCode = HttpStatusCode.Created) Then

                    Dim headers As System.Net.WebHeaderCollection
                    headers = webResponse.Headers

                    Dim LocationsHeader As String = headers.Get("Location")
                    webResponse.Close()
                    ForumsURL = ForumsURL & LocationsHeader.Substring(LocationsHeader.IndexOf("?"))

                    httpWR = WebRequest.Create(ForumsURL)
                    httpWR.Timeout = 60000
                    httpWR.Method = "POST"
                    httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
                    httpWR.ContentType = "application/atom+xml"
                    httpWR.Accept = "*/*"
                    httpWR.Headers.Add("Authorization", "Basic " & GetEncodedUsernamePassword(Username, Password))
                    httpWR.CookieContainer = cookieContainer

                    byteArr = System.Text.Encoding.ASCII.GetBytes(ForumsBody.ToString())
                    httpWR.ContentLength = byteArr.Length
                    dataStream = httpWR.GetRequestStream()
                    dataStream.Write(byteArr, 0, byteArr.Length)
                    dataStream.Close()

                    Dim httpWebResponse As HttpWebResponse

                    Try
                        Dim startTime As DateTime = DateTime.Now
                        httpWebResponse = httpWR.GetResponse()
                        Dim endTime As DateTime = DateTime.Now
                        Dim span As TimeSpan = endTime - startTime
                        Dim createTime As Double = Math.Round(span.TotalMilliseconds, 1)

                        If (webResponse.StatusCode = HttpStatusCode.OK) Then
                            'Created Correctly...do things
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Created forum in " & createTime & " ms.", LogUtilities.LogUtils.LogLevel.Normal)
                            alertReset = True
                            InsertIntoIBMConnectionsDailyStats(myServer.ServerName, "Create.Forum.TimeMs", createTime.ToString())
                        Else
                            'Created wrongly...do things
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Forum failed to create. It took " & createTime & " ms and produced a status code of " & webResponse.StatusCode & " and description of " & webResponse.StatusDescription & ".", LogUtilities.LogUtils.LogLevel.Normal)
                            alertReset = False

                            myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The forum was not created. It produced a status code of " & webResponse.StatusCode & " and a description of " & webResponse.StatusDescription & ".", myServer.Location)

                            If myServer.StatusCode = "OK" Then
                                myServer.StatusCode = "Issue"
                                myServer.Status = "Issue"
                                myServer.ResponseDetails = "The forum was not created. It produced a status code of " & webResponse.StatusCode & " and a description of " & webResponse.StatusDescription & "."
                            End If
                        End If

                        httpWR = WebRequest.Create(LocationsHeader)
                        httpWR.Timeout = 60000
                        httpWR.Method = "DELETE"
                        httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
                        httpWR.ContentType = "application/atom+xml"
                        httpWR.Accept = "*/*"
                        'httpWR.Headers.Add("Authorization", "Basic d3N0YW51bGlzOldzMTMxNTU3MDIh")

                        Dim webResponse2 As HttpWebResponse

                        Try
                            webResponse2 = httpWR.GetResponse()

                            If (webResponse2.StatusCode = HttpStatusCode.OK) Then
                                'It deleted...do nothing
                                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Deleted comunity.", LogUtilities.LogUtils.LogLevel.Normal)

                                'If TestThreshold > createTime Then
                                '    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The forum was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)
                                'Else
                                '    myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The forum was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                                'End If
                            Else
                                'It failed to delete...send alert
                                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Failed to delete comunity.", LogUtilities.LogUtils.LogLevel.Normal)
                                'myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The forum was successfully created in " & createTime & " ms but failed to delete the comunity.")

                                If myServer.StatusCode = "OK" Then
                                    'myServer.StatusCode = "Issue"
                                    'myServer.ResponseDetails = "The forum was successfully created in " & createTime & " ms but failed to delete the comunity."
                                End If
                            End If

                        Catch ex As Exception
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to delete comunity due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                            'myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The forum was successfully created in " & createTime & " ms but failed to delete the comunity.")

                            If myServer.StatusCode = "OK" Then
                                'myServer.StatusCode = "Issue"
                                'myServer.ResponseDetails = "The forum was successfully created in " & createTime & " ms but failed to delete the comunity."
                            End If
                        Finally
                            If webResponse2 IsNot Nothing Then
                                webResponse2.Close()
                            End If

                            If TestThreshold < createTime Then
                                myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The forum was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)

                                If myServer.StatusCode = "OK" Then
                                    myServer.StatusCode = "Issue"
                                    myServer.Status = "Issue"
                                    myServer.ResponseDetails = "The forum was successfully created in " & createTime & " ms but has a threshold of " & TestThreshold & " ms."
                                End If

                            Else
                                myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The forum was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                            End If

                        End Try
                    Catch ex As Exception
                        WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to create forum due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                        myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The forum was not created.", myServer.Location)

                        If myServer.StatusCode = "OK" Then
                            myServer.StatusCode = "Issue"
                            myServer.Status = "Issue"
                            myServer.ResponseDetails = "The forum was not created."
                        End If
                    Finally
                        If httpWebResponse IsNot Nothing Then
                            httpWebResponse.Close()
                        End If
                    End Try
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to create comunity for forum due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The forum was not created.", myServer.Location)

                If myServer.StatusCode = "OK" Then
                    myServer.StatusCode = "Issue"
                    myServer.Status = "Issue"
                    myServer.ResponseDetails = "The forum was not created."
                End If
            Finally
                If webResponse IsNot Nothing Then
                    webResponse.Close()
                End If
            End Try
        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error in TestCreateForums. Error: " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try


    End Sub

    Public Sub TestSearchProfiles(ByRef myServer As MonitoredItems.IBMConnect)
        Try
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " In TestSearchProfiles", LogUtilities.LogUtils.LogLevel.Normal)
            Dim AlertType As String = "Search Profiles"
            Dim alertReset As Boolean
            Dim TestThreshold As Int64 = myServer.SearchProfilesThreshold

            Dim URL As String = myServer.IPAddress
            Dim Username As String = myServer.UserName
            Dim Password As String = myServer.Password

            Dim Name As String = "VitalSigns Test Profiles"
            URL = URL + "/profiles/atom/search.do?name=A"

            Dim httpWR As HttpWebRequest = WebRequest.Create(URL)
            httpWR.Timeout = 60000
            httpWR.Method = "GET"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.Accept = "*/*"
            httpWR.Headers.Add("Authorization", "Basic " & GetEncodedUsernamePassword(Username, Password))
            Dim cookieContainer As New CookieContainer()
            httpWR.CookieContainer = cookieContainer

            Dim webResponse As HttpWebResponse



            Try
                Dim startTime As DateTime = DateTime.Now
                webResponse = httpWR.GetResponse()
                Dim endTime As DateTime = DateTime.Now
                Dim span As TimeSpan = endTime - startTime
                Dim createTime As Double = Math.Round(span.TotalMilliseconds, 1)


                If (webResponse.StatusCode = HttpStatusCode.OK) Then
                    'It deleted...do nothing
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Searched profiles.", LogUtilities.LogUtils.LogLevel.Normal)
                    If TestThreshold < createTime Then
                        myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The search on the profiles was successfully done in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)

                        If myServer.StatusCode = "OK" Then
                            myServer.StatusCode = "Issue"
                            myServer.Status = "Issue"
                            myServer.ResponseDetails = "The profiles were successfully searched in " & createTime & " ms but has a threshold of " & TestThreshold & " ms."
                        End If

                    Else
                        myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The search on the profiles was successfully done in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                    End If
                    InsertIntoIBMConnectionsDailyStats(myServer.ServerName, "Search.Profile.TimeMs", createTime.ToString())
                Else
                    'It failed to delete...send alert
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Failed to delete file.", LogUtilities.LogUtils.LogLevel.Normal)
                    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The search on the profiles was not done. It produced a status code of " & webResponse.StatusCode & " and a description of " & webResponse.StatusDescription & ".")

                    If myServer.StatusCode = "OK" Then
                        myServer.StatusCode = "Issue"
                        myServer.Status = "Issue"
                        myServer.ResponseDetails = "The search on the profiles was not done. It produced a status code of " & webResponse.StatusCode & " and a description of " & webResponse.StatusDescription & "."
                    End If
                End If

            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Profile Search failed due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The search on the profiles was not done.", myServer.Location)

                If myServer.StatusCode = "OK" Then
                    myServer.StatusCode = "Issue"
                    myServer.Status = "Issue"
                    myServer.ResponseDetails = "The search on the profiles was not done."
                End If
            Finally
                If webResponse IsNot Nothing Then
                    webResponse.Close()
                End If
            End Try
        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error in TestSearchProfiles. Error: " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try

    End Sub

    Public Sub TestCreateWikis(ByRef myServer As MonitoredItems.IBMConnect)
        Try
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " In TestCreateWikis", LogUtilities.LogUtils.LogLevel.Normal)
            Dim AlertType As String = "Create Wiki"
            Dim alertReset As Boolean
            Dim TestThreshold As String = myServer.CreateWikisThreshold

            Dim URL As String = myServer.IPAddress
            Dim Username As String = myServer.UserName
            Dim Password As String = myServer.Password

            Dim Name As String = "VitalSigns Test Wiki"
            URL = URL + "/wikis/basic/api/wikis/feed"
            Dim Body As String = "<?xml version=""1.0"" encoding=""UTF-8""?><entry xmlns=""http://www.w3.org/2005/Atom""><category scheme=""http://www.ibm.com/xmlns/prod/sn/type"" term=""wiki"" label=""VitalSigns wiki""/><title type=""text"">" + Name + "</title></entry>"

            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Will first try deleting all undeleted VitalSigns Wikis", LogUtilities.LogUtils.LogLevel.Normal)
            CleanAllVitalSignWikis(myServer)

            Dim httpWR As HttpWebRequest = WebRequest.Create(URL)
            httpWR.Timeout = 60000
            httpWR.Method = "POST"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/atom+xml"
            httpWR.Accept = "*/*"
            httpWR.Headers.Add("Authorization", "Basic " & GetEncodedUsernamePassword(Username, Password))

            Dim byteArr As Byte() = System.Text.Encoding.ASCII.GetBytes(Body.ToString())
            httpWR.ContentLength = byteArr.Length
            Dim dataStream As Stream = httpWR.GetRequestStream()
            dataStream.Write(byteArr, 0, byteArr.Length)
            dataStream.Close()
            Dim cookieContainer As New CookieContainer()
            httpWR.CookieContainer = cookieContainer

            Dim webResponse As HttpWebResponse

            Try
                Dim startTime As DateTime = DateTime.Now
                webResponse = httpWR.GetResponse()
                Dim endTime As DateTime = DateTime.Now
                Dim span As TimeSpan = endTime - startTime
                Dim createTime As Double = Math.Round(span.TotalMilliseconds, 1)

                If (webResponse.StatusCode = HttpStatusCode.Created) Then
                    'Created Correctly...do things
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Created wiki in " & createTime & " ms.", LogUtilities.LogUtils.LogLevel.Normal)
                    alertReset = True
                    InsertIntoIBMConnectionsDailyStats(myServer.ServerName, "Create.Wiki.TimeMs", createTime.ToString())
                Else
                    'Created wrongly...do things
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Wiki failed to create. It took " & createTime & " ms and produced a status code of " & webResponse.StatusCode & " and description of " & webResponse.StatusDescription & ".", LogUtilities.LogUtils.LogLevel.Normal)
                    alertReset = False

                    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The wiki was not created. It produced a status code of " & webResponse.StatusCode & " and a description of " & webResponse.StatusDescription & ".", myServer.Location)

                    If myServer.StatusCode = "OK" Then
                        myServer.StatusCode = "Issue"
                        myServer.Status = "Issue"
                        myServer.ResponseDetails = "The wiki was not created. It produced a status code of " & webResponse.StatusCode & " and a description of " & webResponse.StatusDescription & "."
                    End If
                End If

                If (webResponse.StatusCode = HttpStatusCode.Created) Then

                    Dim actString As String = webResponse.ResponseUri.AbsolutePath
                    Dim actDS As Stream = webResponse.GetResponseStream()
                    Dim actReader As StreamReader = New StreamReader(actDS)
                    Dim resposne As String = actReader.ReadToEnd()



                    Dim xmlDoc As New Xml.XmlDocument()
                    xmlDoc.LoadXml(resposne)

                    Dim deleteString As String = ""

                    For Each xmlElement As Xml.XmlElement In xmlDoc.Item("entry")
                        If xmlElement.Name = "link" Then
                            If xmlElement.Attributes("rel").Value.ToString() = "edit" Then
                                deleteString = xmlElement.Attributes("href").Value.ToString()
                                Exit For
                            End If
                        End If
                    Next


                    Dim headers As System.Net.WebHeaderCollection
                    headers = webResponse.Headers




                    httpWR = WebRequest.Create(deleteString)
                    httpWR.Timeout = 6000000
                    httpWR.Method = "DELETE"
                    httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
                    httpWR.ContentType = "application/atom+xml"
                    httpWR.Accept = "*/*"
                    'httpWR.Headers.Add("Authorization", "Basic d3N0YW51bGlzOldzMTMxNTU3MDIh")

                    httpWR.CookieContainer = cookieContainer

                    Dim webResponse2 As HttpWebResponse

                    Try
                        webResponse2 = httpWR.GetResponse()

                        If (webResponse2.StatusCode = HttpStatusCode.OK) Then
                            'It deleted...do nothing
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Deleted wiki.", LogUtilities.LogUtils.LogLevel.Normal)
                            'If TestThreshold > span.Milliseconds Then
                            '    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The wiki was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)
                            'Else
                            '    myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The wiki was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                            'End If
                        Else
                            'It failed to delete...send alert
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Failed to delete wiki.", LogUtilities.LogUtils.LogLevel.Normal)
                            'myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The wiki was successfully created in " & createTime & " ms but failed to delete.")

                            If myServer.StatusCode = "OK" Then
                                'myServer.StatusCode = "Issue"
                                'myServer.ResponseDetails = "The wiki was successfully created in " & createTime & " ms but failed to delete."
                            End If
                        End If

                    Catch ex As Exception
                        WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to delete wiki due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                        'myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The wiki was successfully created in " & createTime & " ms but failed to delete.")

                        If myServer.StatusCode = "OK" Then
                            'myServer.StatusCode = "Issue"
                            'myServer.ResponseDetails = "The wiki was successfully created in " & createTime & " ms but failed to delete."
                        End If
                    Finally
                        If webResponse2 IsNot Nothing Then
                            webResponse2.Close()
                        End If

                        If TestThreshold < createTime Then
                            myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The wiki was successfully created in " & createTime & " ms with a threshold value of " & TestThreshold & " ms.", myServer.Location)

                            If myServer.StatusCode = "OK" Then
                                myServer.StatusCode = "Issue"
                                myServer.Status = "Issue"
                                myServer.ResponseDetails = "The wiki was successfully created in " & createTime & " ms but has a threshold of " & TestThreshold & " ms."
                            End If

                        Else
                            myAlert.ResetAlert(myServer.DeviceType, myServer.Name, AlertType, myServer.Location, "The wiki was successfully created in " & createTime & " ms but has a threshold value of " & TestThreshold & " ms.")
                        End If
                    End Try

                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error!! Failed to create wiki due to " & ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                myAlert.QueueAlert(myServer.DeviceType, myServer.Name, AlertType, "The wiki was not created.", myServer.Location)

                If myServer.StatusCode = "OK" Then
                    myServer.StatusCode = "Issue"
                    myServer.Status = "Issue"
                    myServer.ResponseDetails = "The wiki was not created."
                End If
            Finally
                If webResponse IsNot Nothing Then
                    webResponse.Close()
                End If
            End Try
        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error in TestCreateFiles. Error: " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try


    End Sub




    Public Sub CleanAllVitalSignActivities(ByRef myServer As MonitoredItems.IBMConnect)


        Try
            Dim URL As String = myServer.IPAddress
            Dim Username As String = myServer.UserName
            Dim Password As String = myServer.Password


            Dim ActivityName As String = "VitalSigns Test Activity"
            Dim activityURL As String = URL + "/activities/service/atom2/activities?title=" & ActivityName.Replace(" ", "%20") & "&ps=100"


            Dim httpWR As HttpWebRequest = WebRequest.Create(activityURL)
            httpWR.Timeout = 60000
            httpWR.Method = "GET"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/atom+xml"
            httpWR.Accept = "*/*"
            httpWR.Headers.Add("Authorization", "Basic " & GetEncodedUsernamePassword(Username, Password))
            Dim cookieContainer As New CookieContainer()
            httpWR.CookieContainer = cookieContainer

            Dim webResponse As HttpWebResponse

            Try

                webResponse = httpWR.GetResponse()

                If (webResponse.StatusCode = HttpStatusCode.OK) Then
                    'Got Correctly...do things

                    Dim actString As String = webResponse.ResponseUri.AbsolutePath
                    Dim actDS As Stream = webResponse.GetResponseStream()
                    Dim actReader As StreamReader = New StreamReader(actDS)
                    Dim resposne As String = actReader.ReadToEnd()

                    Dim xmlDoc As New Xml.XmlDocument()
                    xmlDoc.LoadXml(resposne)

                    Dim deleteString As String = ""

                    For Each xmlElement As Xml.XmlElement In xmlDoc.GetElementsByTagName("id")
                        If xmlElement.InnerText.Contains("ps=100") Then
                            Continue For
                        End If


                        Dim ID As String = xmlElement.InnerText.Substring(xmlElement.InnerText.LastIndexOf(":") + 1)


                        Dim deleteURL As String = URL + "/activities/service/atom2/activitynode?activityNodeUuid=" + ID

                        Try
                            Dim httpWR2 As HttpWebRequest = WebRequest.Create(deleteURL)
                            httpWR2.Timeout = 60000
                            httpWR2.Method = "DELETE"
                            httpWR2.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
                            httpWR2.ContentType = "application/atom+xml"
                            httpWR2.Accept = "*/*"
                            httpWR2.CookieContainer = cookieContainer

                            Dim webResponse2 As HttpWebResponse

                            Try
                                webResponse2 = httpWR2.GetResponse()
                                If webResponse2.StatusCode <> HttpStatusCode.NoContent Then
                                    'Not deleted
                                End If

                            Catch ex As Exception

                            Finally
                                If webResponse2 IsNot Nothing Then
                                    webResponse2.Close()
                                End If

                            End Try

                        Catch ex As Exception

                        End Try

                    Next

                End If

            Catch ex As Exception

            Finally
                If webResponse IsNot Nothing Then
                    webResponse.Close()
                End If
            End Try

        Catch ex As Exception

        End Try


    End Sub

    Public Sub CleanAllVitalSignBlogs(ByRef myServer As MonitoredItems.IBMConnect)


        Try
            Dim URL As String = myServer.IPAddress
            Dim Username As String = myServer.UserName
            Dim Password As String = myServer.Password


            Dim BlogName As String = "VitalSigns Test Blog"
            Dim BlogURL As String = URL + "/blogs/homepage/feed/blogs/atom?search=" & BlogName.Replace(" ", "%20") & "&ps=100&lang=en_us"


            Dim httpWR As HttpWebRequest = WebRequest.Create(BlogURL)
            httpWR.Timeout = 60000
            httpWR.Method = "GET"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/atom+xml"
            httpWR.Accept = "*/*"
            httpWR.Headers.Add("Authorization", "Basic " & GetEncodedUsernamePassword(Username, Password))
            Dim cookieContainer As New CookieContainer()
            httpWR.CookieContainer = cookieContainer

            Dim webResponse As HttpWebResponse

            Try

                webResponse = httpWR.GetResponse()

                If (webResponse.StatusCode = HttpStatusCode.OK) Then
                    'Got Correctly...do things

                    Dim actString As String = webResponse.ResponseUri.AbsolutePath
                    Dim actDS As Stream = webResponse.GetResponseStream()
                    Dim actReader As StreamReader = New StreamReader(actDS)
                    Dim resposne As String = actReader.ReadToEnd()

                    Dim xmlDoc As New Xml.XmlDocument()
                    xmlDoc.LoadXml(resposne)


                    Dim ID As String = ""
                    For Each xmlEntry As Xml.XmlElement In xmlDoc.GetElementsByTagName("entry")

                        If xmlEntry.GetElementsByTagName("title")(0).InnerText.ToString() = BlogName Then
                            ID = xmlEntry.GetElementsByTagName("id")(0).InnerText.ToString()
                            ID = ID.Substring(ID.IndexOf("blog-") + "blog-".Length)

                            Dim deleteURL As String = URL + "/blogs/homepage/api/blogs/" + ID

                            Try
                                Dim httpWR2 As HttpWebRequest = WebRequest.Create(deleteURL)
                                httpWR2.Timeout = 60000
                                httpWR2.Method = "DELETE"
                                httpWR2.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
                                httpWR2.ContentType = "application/atom+xml"
                                httpWR2.Accept = "*/*"
                                httpWR2.CookieContainer = cookieContainer

                                Dim webResponse2 As HttpWebResponse

                                Try
                                    webResponse2 = httpWR2.GetResponse()
                                    If webResponse2.StatusCode <> HttpStatusCode.NoContent Then
                                        'Not deleted
                                    End If

                                Catch ex As Exception

                                Finally
                                    If webResponse2 IsNot Nothing Then
                                        webResponse2.Close()
                                    End If

                                End Try

                            Catch ex As Exception

                            End Try

                        End If

                    Next

                End If

            Catch ex As Exception

            Finally
                If webResponse IsNot Nothing Then
                    webResponse.Close()
                End If
            End Try

        Catch ex As Exception

        End Try


    End Sub

    Public Sub CleanAllVitalSignBookmarks(ByRef myServer As MonitoredItems.IBMConnect)


        Try

            Dim URL As String = myServer.IPAddress
            Dim Username As String = myServer.UserName
            Dim Password As String = myServer.Password

            Dim Name As String = "http://www.DummyURLForVitalSigns.com"
            URL = URL + "/dogear/api/app"

            Dim deleteString As String = URL & "?url=" & Name

            Dim httpWR As HttpWebRequest = WebRequest.Create(deleteString)
            httpWR.Timeout = 6000000
            httpWR.Method = "DELETE"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/atom+xml"
            httpWR.Accept = "*/*"
            httpWR.Headers.Add("Authorization", "Basic d3N0YW51bGlzOldzMTMxNTU3MDIh")
            Dim cookieContainer As New CookieContainer()
            httpWR.CookieContainer = cookieContainer
            Dim httpWP As HttpWebResponse

            Try
                httpWP = httpWR.GetResponse()

                If (httpWP.StatusCode = HttpStatusCode.NoContent) Then
                    'It deleted...do things
                Else
                    'It failed to delete...do things
                End If

            Catch ex As Exception

            Finally
                If httpWP IsNot Nothing Then
                    httpWP.Close()
                End If
            End Try



        Catch ex As Exception

        End Try


    End Sub

    Public Sub CleanAllVitalSignCommunities(ByRef myServer As MonitoredItems.IBMConnect)

        Try
            Dim URL As String = myServer.IPAddress
            Dim Username As String = myServer.UserName
            Dim Password As String = myServer.Password


            Dim Name As String = "VitalSigns Test Community"
            Dim CommunityURL As String = URL + "/communities/service/atom/communities/all?search=" & Name.Replace(" ", "%20") & "&ps=100&lang=en_us"


            Dim httpWR As HttpWebRequest = WebRequest.Create(CommunityURL)
            httpWR.Timeout = 60000
            httpWR.Method = "GET"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/atom+xml"
            httpWR.Accept = "*/*"
            httpWR.Headers.Add("Authorization", "Basic " & GetEncodedUsernamePassword(Username, Password))
            Dim cookieContainer As New CookieContainer()
            httpWR.CookieContainer = cookieContainer

            Dim webResponse As HttpWebResponse

            Try

                webResponse = httpWR.GetResponse()

                If (webResponse.StatusCode = HttpStatusCode.OK) Then
                    'Got Correctly...do things

                    Dim actString As String = webResponse.ResponseUri.AbsolutePath
                    Dim actDS As Stream = webResponse.GetResponseStream()
                    Dim actReader As StreamReader = New StreamReader(actDS)
                    Dim resposne As String = actReader.ReadToEnd()

                    Dim xmlDoc As New Xml.XmlDocument()
                    xmlDoc.LoadXml(resposne)


                    Dim ID As String = ""
                    For Each xmlEntry As Xml.XmlElement In xmlDoc.GetElementsByTagName("entry")

                        If xmlEntry.GetElementsByTagName("title")(0).InnerText.ToString() = Name Then

                            For Each xmlLink As Xml.XmlElement In xmlEntry.ChildNodes()
                                If xmlLink.Name = "link" Then
                                    If xmlLink.Attributes("rel").Value.ToString() = "edit" Then
                                        Dim deleteURL As String = xmlLink.Attributes("href").Value.ToString()

                                        Try
                                            Dim httpWR2 As HttpWebRequest = WebRequest.Create(deleteURL)
                                            httpWR2.Timeout = 60000
                                            httpWR2.Method = "DELETE"
                                            httpWR2.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
                                            httpWR2.ContentType = "application/atom+xml"
                                            httpWR2.Accept = "*/*"
                                            httpWR2.CookieContainer = cookieContainer

                                            Dim webResponse2 As HttpWebResponse

                                            Try
                                                webResponse2 = httpWR2.GetResponse()
                                                If webResponse2.StatusCode <> HttpStatusCode.OK Then
                                                    'deleted
                                                End If

                                            Catch ex As Exception

                                            Finally
                                                If webResponse2 IsNot Nothing Then
                                                    webResponse2.Close()
                                                End If

                                            End Try

                                        Catch ex As Exception

                                        End Try


                                    End If
                                End If

                            Next

                        End If

                    Next

                End If

            Catch ex As Exception

            Finally
                If webResponse IsNot Nothing Then
                    webResponse.Close()
                End If
            End Try

        Catch ex As Exception

        End Try

    End Sub

    Public Sub CleanAllVitalSignFiles(ByRef myServer As MonitoredItems.IBMConnect)

        Try

            Dim URL As String = myServer.IPAddress
            Dim Username As String = myServer.UserName
            Dim Password As String = myServer.Password

            Dim Name As String = "VitalSignsTestFiles.txt"
            URL = URL + "/files/basic/api/myuserlibrary/feed?search=" & Name & "&ps=100"

            '  https://connections-as.jnittech.com:9444/communities/service/atom/community/instance?communityUuid=ae774096-1d25-4840-8cd4-855807dc5f69

            Dim httpWR As HttpWebRequest = WebRequest.Create(URL)
            httpWR.Timeout = 60000
            httpWR.Method = "GET"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/atom+xml"
            httpWR.Accept = "*/*"
            httpWR.Headers.Add("Authorization", "Basic " & GetEncodedUsernamePassword(Username, Password))
            Dim cookieContainer As New CookieContainer()
            httpWR.CookieContainer = cookieContainer

            Dim webResponse As HttpWebResponse

            Try

                webResponse = httpWR.GetResponse()

                If (webResponse.StatusCode = HttpStatusCode.OK) Then
                    'Got Correctly...do things

                    Dim actString As String = webResponse.ResponseUri.AbsolutePath
                    Dim actDS As Stream = webResponse.GetResponseStream()
                    Dim actReader As StreamReader = New StreamReader(actDS)
                    Dim resposne As String = actReader.ReadToEnd()

                    Dim xmlDoc As New Xml.XmlDocument()
                    xmlDoc.LoadXml(resposne)


                    Dim ID As String = ""
                    For Each xmlEntry As Xml.XmlElement In xmlDoc.GetElementsByTagName("entry")

                        If xmlEntry.GetElementsByTagName("td:label")(0).InnerText.ToString() = Name Then

                            For Each xmlLink As Xml.XmlElement In xmlEntry.ChildNodes()
                                If xmlLink.Name = "link" Then
                                    If xmlLink.Attributes("rel").Value.ToString() = "edit" Then
                                        Dim deleteURL As String = xmlLink.Attributes("href").Value.ToString()

                                        Try
                                            Dim httpWR2 As HttpWebRequest = WebRequest.Create(deleteURL)
                                            httpWR2.Timeout = 60000
                                            httpWR2.Method = "DELETE"
                                            httpWR2.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
                                            httpWR2.ContentType = "application/atom+xml"
                                            httpWR2.Accept = "*/*"
                                            httpWR2.CookieContainer = cookieContainer

                                            Dim webResponse2 As HttpWebResponse

                                            Try
                                                webResponse2 = httpWR2.GetResponse()
                                                If webResponse2.StatusCode <> HttpStatusCode.NoContent Then
                                                    'deleted
                                                End If

                                            Catch ex As Exception

                                            Finally
                                                If webResponse2 IsNot Nothing Then
                                                    webResponse2.Close()
                                                End If

                                            End Try

                                        Catch ex As Exception

                                        End Try


                                    End If
                                End If

                            Next

                            'ID = xmlEntry.GetElementsByTagName("id")(0).InnerText.ToString()
                            'ID = ID.Substring(ID.IndexOf("blog-") + "blog-".Length)

                            'Dim deleteURL As String = URL + "/blogs/homepage/api/blogs/" + ID



                        End If

                    Next

                End If

            Catch ex As Exception

            Finally
                If webResponse IsNot Nothing Then
                    webResponse.Close()
                End If
            End Try

        Catch ex As Exception

        End Try

    End Sub

    Public Sub CleanAllVitalSignWikis(ByRef myServer As MonitoredItems.IBMConnect)

        Try
            Dim URL As String = myServer.IPAddress
            Dim Username As String = myServer.UserName
            Dim Password As String = myServer.Password


            Dim Name As String = "VitalSigns Test Wiki"
            URL = URL + "/wikis/basic/api/wikis/feed?ps=500"

            '  https://connections-as.jnittech.com:9444/communities/service/atom/community/instance?communityUuid=ae774096-1d25-4840-8cd4-855807dc5f69

            Dim httpWR As HttpWebRequest = WebRequest.Create(URL)
            httpWR.Timeout = 60000
            httpWR.Method = "GET"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/atom+xml"
            httpWR.Accept = "*/*"
            httpWR.Headers.Add("Authorization", "Basic " & GetEncodedUsernamePassword(Username, Password))
            Dim cookieContainer As New CookieContainer()
            httpWR.CookieContainer = cookieContainer

            Dim webResponse As HttpWebResponse

            Try

                webResponse = httpWR.GetResponse()

                If (webResponse.StatusCode = HttpStatusCode.OK) Then
                    'Got Correctly...do things

                    Dim actString As String = webResponse.ResponseUri.AbsolutePath
                    Dim actDS As Stream = webResponse.GetResponseStream()
                    Dim actReader As StreamReader = New StreamReader(actDS)
                    Dim resposne As String = actReader.ReadToEnd()

                    Dim xmlDoc As New Xml.XmlDocument()
                    xmlDoc.LoadXml(resposne)


                    Dim ID As String = ""
                    For Each xmlEntry As Xml.XmlElement In xmlDoc.GetElementsByTagName("entry")

                        If xmlEntry.GetElementsByTagName("td:label")(0).InnerText.ToString() = Name Then

                            For Each xmlLink As Xml.XmlElement In xmlEntry.ChildNodes()
                                If xmlLink.Name = "link" Then
                                    If xmlLink.Attributes("rel").Value.ToString() = "edit" Then
                                        Dim deleteURL As String = xmlLink.Attributes("href").Value.ToString()

                                        Try
                                            Dim httpWR2 As HttpWebRequest = WebRequest.Create(deleteURL)
                                            httpWR2.Timeout = 60000
                                            httpWR2.Method = "DELETE"
                                            httpWR2.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
                                            httpWR2.ContentType = "application/atom+xml"
                                            httpWR2.Accept = "*/*"
                                            httpWR2.CookieContainer = cookieContainer

                                            Dim webResponse2 As HttpWebResponse

                                            Try
                                                webResponse2 = httpWR2.GetResponse()
                                                If webResponse2.StatusCode <> HttpStatusCode.NoContent Then
                                                    'deleted
                                                End If

                                            Catch ex As Exception

                                            Finally
                                                If webResponse2 IsNot Nothing Then
                                                    webResponse2.Close()
                                                End If

                                            End Try

                                        Catch ex As Exception

                                        End Try


                                    End If
                                End If

                            Next

                            'ID = xmlEntry.GetElementsByTagName("id")(0).InnerText.ToString()
                            'ID = ID.Substring(ID.IndexOf("blog-") + "blog-".Length)

                            'Dim deleteURL As String = URL + "/blogs/homepage/api/blogs/" + ID



                        End If

                    Next

                End If

            Catch ex As Exception

            Finally
                If webResponse IsNot Nothing Then
                    webResponse.Close()
                End If
            End Try

        Catch ex As Exception

        End Try

    End Sub



    Public Function TestIBMConnectResponding(ByRef myServer As MonitoredItems.IBMConnect) As Boolean

        Dim responseFromServer As String
        Dim isResponding = False
        Try

            ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

            Dim URL As String = myServer.IPAddress & "/homepage/j_security_check"
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "URL: " & URL, LogUtilities.LogUtils.LogLevel.Normal)
            Dim httpWR As HttpWebRequest = WebRequest.Create(URL)
            httpWR.Timeout = 6000000
            httpWR.Method = "POST"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/x-www-form-urlencoded"
            'httpWR.CookieContainer
            Dim s As String = "service.name=&secure=&fragment=&j_username=" & myServer.UserName & "&j_password=" & Uri.EscapeUriString(myServer.Password)

            Dim byteArr As Byte() = System.Text.Encoding.ASCII.GetBytes(s.ToString())
            httpWR.ContentLength = byteArr.Length
            Dim dataStr As Stream = httpWR.GetRequestStream()
            dataStr.Write(byteArr, 0, byteArr.Length)
            dataStr.Close()

            'httpWR.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
            Dim webresp As HttpWebResponse
            Try
                webresp = httpWR.GetResponse()
                isResponding = True
            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Failed to connect in TestResponding. " & ex.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                isResponding = False

            Finally

                If webresp IsNot Nothing Then
                    webresp.Close()
                End If

            End Try



        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Failed to connect in TestResponding 2. " & ex.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
        End Try
        Return isResponding
    End Function

    Public Function TestIMBConnectLogon(ByRef myServer As MonitoredItems.IBMConnect)

        Dim responseFromServer As String
        Dim loggedIn = False
        Try

            ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

            Dim URL As String = myServer.IPAddress & "/homepage/j_security_check"

            Dim httpWR As HttpWebRequest = WebRequest.Create(URL)
            httpWR.Timeout = 6000000
            httpWR.Method = "POST"
            httpWR.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0"
            httpWR.ContentType = "application/x-www-form-urlencoded"
            'httpWR.CookieContainer
            Dim s As String = "service.name=&secure=&fragment=&j_username=" & myServer.UserName & "&j_password=" & Uri.EscapeUriString(myServer.Password)

            Dim byteArr As Byte() = System.Text.Encoding.ASCII.GetBytes(s.ToString())
            httpWR.ContentLength = byteArr.Length
            Dim dataStr As Stream = httpWR.GetRequestStream()
            dataStr.Write(byteArr, 0, byteArr.Length)
            dataStr.Close()

            httpWR.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
            Dim webresp As HttpWebResponse
            Try
                Dim before As DateTime = DateTime.Now
                webresp = httpWR.GetResponse()
                Dim span As TimeSpan = New TimeSpan(Now.Ticks - before.Ticks)
                Dim createTime As Double = Math.Round(span.TotalMilliseconds, 1)

                Dim queryString As String = webresp.ResponseUri.Query

                If queryString.Contains("error=true") Then
                    'failed to log in
                    myAlert.QueueAlert(myServer.DeviceType, myServer.Name, "Login Test", "The account failed to log in.", myServer.Location)
                    myServer.ResponseTime = 0

                    If myServer.StatusCode = "OK" Then
                        myServer.StatusCode = "Issue"
                        myServer.Status = "Issue"
                        myServer.ResponseDetails = "The Login Test for this server failed."
                    End If
                Else
                    'log in succedded
                    loggedIn = True
                    myAlert.ResetAlert(myServer.DeviceType, myServer.Name, "Login Test", myServer.Location, "The account logged in after " & createTime & " ms.")
                    InsertIntoIBMConnectionsDailyStats(myServer.Name, "ResponseTime", createTime)
                    myServer.ResponseTime = createTime

                End If
            Catch ex As Exception

                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error getting Logon Test. Error: " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)

            Finally

                If webresp IsNot Nothing Then
                    webresp.Close()
                End If

            End Try



        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & " Error in TestIBMConnectLogon. Error: " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try

    End Function




    Public Sub GetConnectionsStats(ByRef myServer As MonitoredItems.IBMConnect)
        'Activities https://connections-as.jnittech.com:9444/activities/service/html/servermetrics
        'Blogs https://connections-as.jnittech.com:9444/blogs/roller-ui/servermetrics.do?lang=en_us
        'Bookmarks https://connections-as.jnittech.com:9444/dogear/toolbox/servermetrics
        'Communities https://connections-as.jnittech.com:9444/communities/service/html/servermetrics
        'Files https://connections-as.jnittech.com:9444/files/app/statistics
        'Homepage https://connections-as.jnittech.com:9444/homepage/web/servermetrics
        'Profiles https://connections-as.jnittech.com:9444/profiles/html/servermetrics.do
        'Wikis https://connections-as.jnittech.com:9444/wikis/home/statistics

        Try

            Randomize()

            ClearConnectionObjectTables(myServer)

            'This should be first
            GetProfileStats(myServer)

            'These can go whenever
            GetActivityStats(myServer)
            GetBlogStats(myServer)
            GetBookmarksStats(myServer)
            GetCommunityStats(myServer)
            GetFileStats(myServer)
            GetWikiStats(myServer)
            GetForumStats(myServer)


            'This should be last
            GetHomepageStats(myServer)

        Catch ex As Exception

        End Try

    End Sub

    Public Sub ClearConnectionObjectTables(ByRef myServer As MonitoredItems.IBMConnect)
        Dim sql As String = "delete from IbmConnectionsObjects where ServerId = '" & myServer.ID & "';"
        sql += "delete from IbmConnectionsUsers where ServerId = '" & myServer.ID & "';"

        Try
            Dim objVSAdaptor As New VSFramework.VSAdaptor()
            objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "VitalSigns", sql)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub GetActivityStats(ByRef myServer As MonitoredItems.IBMConnect)

        Dim sql As String = "SELECT COALESCE(SUM(CASE WHEN LASTLOGIN > CURRENT_TIMESTAMP - 1 MONTH THEN 1 ELSE 0 END), 0) LOGINS_LAST_MONTH,COALESCE( SUM(CASE WHEN LASTLOGIN > CURRENT_TIMESTAMP - 7 DAYS THEN 1 ELSE 0 END),0) LOGINS_LAST_WEEK, COALESCE(SUM(CASE WHEN LASTLOGIN > CURRENT_TIMESTAMP - 1 DAY THEN 1 ELSE 0 END),0) LOGINS_LAST_DAY FROM ACTIVITIES.OA_MEMBERPROFILE;" & _
         "SELECT COUNT(*) NUM_OF_ACTIVITIES FROM ACTIVITIES.OA_NODE WHERE ISDELETED = 0 AND NODETYPE = 'application/activity';" & _
         "SELECT COUNT(distinct ACTIVITIES.OA_NODEMEMBER.MEMBERID) NUM_OF_USERS_FOLLOWING_ACTIVITY FROM ACTIVITIES.OA_NODEMEMBER INNER JOIN ACTIVITIES.OA_MEMBERPROFILE ON ACTIVITIES.OA_NODEMEMBER.MEMBERID = ACTIVITIES.OA_MEMBERPROFILE.MEMBERID;" & _
         "SELECT COUNT(distinct ACTIVITIES.OA_NODE.CREATEDBY) NUM_OF_ACTIVITY_OWNERS FROM ACTIVITIES.OA_NODE WHERE ISDELETED = 0 AND NODETYPE = 'application/activity';" & _
         "SELECT COUNT(*) NUM_OF_ACTIVITIES_CREATED_YESTERDAY FROM ACTIVITIES.OA_NODE WHERE ISDELETED = 0 AND NODETYPE = 'application/activity' AND DATE(CREATED) = CURRENT_DATE - 1 DAY;" & _
         "SELECT COUNT(*) NUM_OF_ACTIVITIES_FOLLOWED_YESTERDAY FROM ACTIVITIES.OA_NODEMEMBER WHERE DATE(CREATED) = CURRENT_DATE - 1 DAY;" & _
         "SELECT node.Name, 'Activity' as Type, node.CREATED, node.LASTMOD, mp.EXID, node.ACTIVITYUUID FROM ACTIVITIES.OA_NODE node INNER JOIN ACTIVITIES.OA_MEMBERPROFILE mp ON mp.MEMBERID = node.CREATEDBY WHERE node.ISDELETED = 0;" & _
         "SELECT NODEUUID, NAME FROM ACTIVITIES.OA_TAG;" & _
         "SELECT nm.NODEUUID, mp.EXID FROM ACTIVITIES.OA_NODEMEMBER nm INNER JOIN ACTIVITIES.OA_MEMBERPROFILE mp ON mp.MEMBERID = nm.MEMBERID;"


        Dim Category As String = "Activity"

        Try
            Dim con As IBM.Data.DB2.DB2Connection
            Dim ds As New DataSet
            Try

                con = New IBM.Data.DB2.DB2Connection("Database=OPNACT;UserID=" & myServer.DBUserName & ";Password=" & myServer.DBPassword & ";Server=" & myServer.DBHostName & ":" & myServer.DBPort & "")
                con.Open()

                Dim cmd As New IBM.Data.DB2.DB2Command(Sql, con)
                Dim adapter As New IBM.Data.DB2.DB2DataAdapter(cmd)
                adapter.Fill(ds)



            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error getting Activity Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            Finally
                Try
                    If con.IsOpen Then
                        con.Close()
                    End If
                Catch ex As Exception

                End Try

            End Try

            'sql = "INSERT INTO IbmConnectionsActivityStats (ServerId, StatName, StatValue, Category, DateTime) VALUES "
            Dim dict As New Dictionary(Of String, String)

            Try
                Dim cols As String = ""
                Dim i As Integer = 0
                For Each column As DataColumn In ds.Tables(0).Columns
                    cols += column.ColumnName + ","
                    i += 1
                Next


                dict.Add("ACTIVITY_LOGINS_LAST_MONTH", ds.Tables(0).Rows(0)("LOGINS_LAST_MONTH"))
                dict.Add("ACTIVITY_LOGINS_LAST_WEEK", ds.Tables(0).Rows(0)("LOGINS_LAST_WEEK"))
                dict.Add("ACTIVITY_LOGINS_LAST_DAY", ds.Tables(0).Rows(0)("LOGINS_LAST_DAY"))

                dict.Add("NUM_OF_ACTIVITIES_ACTIVITIES", ds.Tables(1).Rows(0)("NUM_OF_ACTIVITIES"))

                dict.Add("NUM_OF_USERS_FOLLOWING_ACTIVITY", ds.Tables(2).Rows(0)("NUM_OF_USERS_FOLLOWING_ACTIVITY"))

                dict.Add("NUM_OF_ACTIVITY_OWNERS", ds.Tables(3).Rows(0)("NUM_OF_ACTIVITY_OWNERS"))

                dict.Add("NUM_OF_ACTIVITIES_ACTIVITIES_CREATED_YESTERDAY", ds.Tables(4).Rows(0)("NUM_OF_ACTIVITIES_CREATED_YESTERDAY"))

                dict.Add("NUM_OF_ACTIVITIES_ACTIVITIES_FOLLOWED_YESTERDAY", ds.Tables(5).Rows(0)("NUM_OF_ACTIVITIES_FOLLOWED_YESTERDAY"))

                Sql = "INSERT INTO IbmConnectionsSummaryStats (ServerName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber) VALUES "

                Dim sqlCols As String = ""
                Dim sqlVals As String = ""
                For Each key In dict.Keys
                    Dim Name As String = key
                    Dim Val As String = dict(key)

                    If String.Equals(myServer.IPAddress, "https://connections-as.jnittech.com:9444", StringComparison.CurrentCultureIgnoreCase) Then
                        If key = "NUM_OF_ACTIVITIES_ACTIVITIES_CREATED_YESTERDAY" Or key = "NUM_OF_ACTIVITIES_ACTIVITIES_FOLLOWED_YESTERDAY" Then
                            Val = Int(20 * Rnd()) + 1
                        End If
                    End If

                    Sql += "('" & myServer.Name & "', GetDate(), '" & Name.ToUpper() & "', '" & Val & "', '" & GetWeekNumber(Now) & "', '" & Now.Month.ToString() & "', '" & Now.Year.ToString() & "', '" & Now.Day.ToString() & "'),"


                Next

                Dim adapter As New VSAdaptor()
                adapter.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", Sql.Substring(0, Sql.Length - 1))



                'Handling for other tables
                Dim dt As DataTable = ds.Tables(6)


                Using sqlConn As SqlClient.SqlConnection = adapter.StartConnectionSQL("VitalSigns")

                    Dim cmd As New SqlClient.SqlCommand()
                    cmd.Connection = sqlConn
                    cmd.CommandText = "DELETE FROM IbmConnectionsObjects WHERE ServerID = @ServerId AND Type = 'Activity'"
                    cmd.Parameters.AddWithValue("@ServerId", myServer.ID)

                    'cmd.ExecuteNonQuery()

                    For Each row As DataRow In dt.Rows()


                        cmd = New SqlClient.SqlCommand()
                        cmd.Connection = sqlConn
                        cmd.CommandText = "INSERT INTO IbmConnectionsObjects (Name, Type, DateCreated, DateLastModified, ServerID, OwnerId, GUID) VALUES " & _
                            "(@Name, 'Activity', @Created, @Modified, @ServerId, (SELECT ID FROM IbmConnectionsUsers WHERE GUID = @GUID AND ServerID = @ServerId), @ActGUID)"
                        cmd.Parameters.AddWithValue("@Name", row("NAME").ToString())
                        cmd.Parameters.AddWithValue("@Created", row("CREATED").ToString())
                        cmd.Parameters.AddWithValue("@Modified", row("LASTMOD").ToString())
                        cmd.Parameters.AddWithValue("@ServerId", myServer.ID)
                        cmd.Parameters.AddWithValue("@GUID", row("EXID").ToString())
                        cmd.Parameters.AddWithValue("@ActGUID", row("ACTIVITYUUID").ToString())

                        cmd.ExecuteNonQuery()

                        For Each tagRow As DataRow In ds.Tables(7).Select("NODEUUID = '" & row("ACTIVITYUUID").ToString() & "'")



                            cmd = New SqlClient.SqlCommand()
                            cmd.Connection = sqlConn
                            cmd.CommandText = "IF NOT EXISTS ( SELECT 1 FROM IbmConnectionsTags WHERE Tag = @TagName) BEGIN INSERT INTO IbmConnectionsTags (Tag) VALUES (@TagName) END"
                            cmd.Parameters.AddWithValue("@TagName", tagRow("NAME").ToString())

                            cmd.ExecuteNonQuery()

                            cmd = New SqlClient.SqlCommand()
                            cmd.Connection = sqlConn
                            cmd.CommandText = "INSERT INTO IbmConnectionsObjectTags (ObjectId, TagId) VALUES ((SELECT TOP 1 ID FROM IbmConnectionsObjects WHERE GUID=@GUID AND" & _
                            " Type = 'Activity' AND ServerID = @ServerId ORDER BY ID DESC), (SELECT TOP 1 ID FROM IbmConnectionsTags WHERE Tag = @TagName))"
                            cmd.Parameters.AddWithValue("@TagName", tagRow("NAME").ToString())
                            cmd.Parameters.AddWithValue("@GUID", row("ACTIVITYUUID").ToString())
                            cmd.Parameters.AddWithValue("@ServerId", myServer.ID)

                            cmd.ExecuteNonQuery()
                        Next



                        For Each userRow As DataRow In ds.Tables(8).Select("NODEUUID = '" & row("ACTIVITYUUID").ToString() & "'")
                            cmd = New SqlClient.SqlCommand()
                            cmd.Connection = sqlConn
                            cmd.CommandText = "INSERT INTO IbmConnectionsObjectUsers (ObjectId, UserId) VALUES((SELECT TOP 1 ID FROM IbmConnectionsObjects WHERE GUID = @NodeGUID AND " & _
                                "Type = 'Activity' AND ServerID = @ServerId ORDER BY ID DESC), (SELECT TOP 1 ID FROM IbmConnectionsUsers WHERE GUID = @GUID AND ServerID = @ServerId));"
                            cmd.Parameters.AddWithValue("@NodeGUID", row("ACTIVITYUUID").ToString())
                            cmd.Parameters.AddWithValue("@GUID", userRow("EXID").ToString())
                            cmd.Parameters.AddWithValue("@ServerId", myServer.ID)

                            cmd.ExecuteNonQuery()
                        Next

                    Next




                End Using



            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error parsing Activity Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            End Try



        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error in GetActivityStats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try

    End Sub

    Public Sub GetBlogStats(ByRef myServer As MonitoredItems.IBMConnect)

        Dim sql As String = "select count(*) Num_Of_Published_Blogs from BLOGS.WEBSITE;" & _
         "select count(distinct WEBSITEID) Num_Of_Blogs_More_Than_One_Author from (select USERID, WEBSITEID FROM BLOGS.WEBLOGENTRY GROUP BY WEBSITEID,USERID) group by WEBSITEID having count(*) > 1;" & _
         "select COALESCE(SUM(CASE WHEN LASTLOGIN > CURRENT_TIMESTAMP - 1 MONTH THEN 1 ELSE 0 END),0) LOGINS_LAST_MONTH, COALESCE(SUM(CASE WHEN LASTLOGIN > CURRENT_TIMESTAMP - 7 DAYS THEN 1 ELSE 0 END),0) LOGINS_LAST_WEEK, COALESCE(SUM(CASE WHEN LASTLOGIN > CURRENT_TIMESTAMP - 1 DAY THEN 1 ELSE 0 END),0) LOGINS_LAST_DAY from BLOGS.ROLLERUSER;" & _
         "select count(distinct USERID) Num_Of_Bloggers FROM BLOGS.WEBLOGENTRY;" & _
         "select count(*) Num_Of_Comments FROM BLOGS.ROLLER_COMMENT;" & _
         "select count(distinct NAME) Num_Of_Distinct_Blog_Tags FROM BLOGS.ROLLER_WEBSITETAG;" & _
         "SELECT COUNT(DISTINCT NAME) Num_Of_Distinct_Entry_Tags FROM BLOGS.ROLLER_WEBLOGENTRYTAG;" & _
         "SELECT COUNT(*) Num_Of_Notifications_Sent FROM BLOGS.ROLLER_NOTIFICATION ;" & _
         "SELECT COUNT(*) Num_Of_Active_Blogs FROM (SELECT distinct WEBSITEID FROM BLOGS.WEBLOGENTRY WHERE (UPDATETIME > (CURRENT_TIMESTAMP - 3 MONTH)) GROUP BY WEBSITEID HAVING COUNT(*) >= 10);" & _
         "SELECT COALESCE(SUM(CASE WHEN DATECREATED > CURRENT_TIMESTAMP - 1 MONTH THEN 1 ELSE 0 END),0) Blogs_Created_LAST_MONTH, COALESCE(SUM(CASE WHEN DATECREATED > CURRENT_TIMESTAMP - 7 DAYS THEN 1 ELSE 0 END),0) Blogs_Created_LAST_WEEK, COALESCE(SUM(CASE WHEN DATECREATED > CURRENT_TIMESTAMP - 1 DAY THEN 1 ELSE 0 END),0) Blogs_Created_LAST_DAY FROM BLOGS.WEBSITE WHERE DATECREATED > CURRENT_TIMESTAMP - 1 MONTH;" & _
         "SELECT COALESCE(SUM(CASE WHEN PUBTIME > CURRENT_TIMESTAMP - 1 MONTH THEN 1 ELSE 0 END),0) Entry_Created_LAST_MONTH, COALESCE(SUM(CASE WHEN PUBTIME > CURRENT_TIMESTAMP - 7 DAYS THEN 1 ELSE 0 END),0) Entry_Created_LAST_WEEK, COALESCE(SUM(CASE WHEN PUBTIME > CURRENT_TIMESTAMP - 1 DAY THEN 1 ELSE 0 END),0) Entry_Created_LAST_DAY FROM BLOGS.WEBLOGENTRY WHERE PUBTIME > CURRENT_TIMESTAMP - 1 MONTH;" & _
         "SELECT COALESCE(SUM(CASE WHEN POSTTIME > CURRENT_TIMESTAMP - 1 MONTH THEN 1 ELSE 0 END),0) Comment_Created_LAST_MONTH, COALESCE(SUM(CASE WHEN POSTTIME > CURRENT_TIMESTAMP - 7 DAYS THEN 1 ELSE 0 END),0) Comment_Created_LAST_WEEK, COALESCE(SUM(CASE WHEN POSTTIME > CURRENT_TIMESTAMP - 1 DAY THEN 1 ELSE 0 END),0) Comment_Created_LAST_DAY FROM BLOGS.ROLLER_COMMENT WHERE POSTTIME > CURRENT_TIMESTAMP - 1 MONTH;" & _
         "select Name, count(*) Num_Of_Blog_Tags FROM BLOGS.ROLLER_WEBSITETAG group by Name order by count(*) desc fetch first 20 rows only;" & _
         "SELECT Name, count(*) Num_Of_Entry_Tags FROM BLOGS.ROLLER_WEBLOGENTRYTAG group by Name order by count(*) desc fetch first 20 rows only;" & _
         "SELECT COUNT(*) NUM_OF_BLOGS_CREATED_YESTERDAY FROM BLOGS.WEBSITE WHERE DATE(DATECREATED) = CURRENT_DATE - 1 DAY;" & _
         "SELECT COUNT(*) NUM_OF_ENTRIES_CREATED_YESTERDAY FROM BLOGS.WEBLOGENTRY WHERE DATE(PUBTIME) = CURRENT_DATE - 1 DAY;" & _
         "SELECT COUNT(*) NUM_OF_COMMENTS_CREATED_YESTERDAY FROM BLOGS.ROLLER_COMMENT WHERE DATE(POSTTIME) = CURRENT_DATE - 1 DAY;" & _
         "SELECT COUNT(*) NUM_OF_NOTIFICATIONS_CREATED_YESTERDAY FROM BLOGS.ROLLER_NOTIFICATION WHERE DATE(TIME) = CURRENT_DATE - 1 DAY;" & _
         "SELECT blog.ID, blog.NAME, 'Blog' as TYPE, blog.DATECREATED, blog.LASTMODIFIED, users.EXTID FROM BLOGS.WEBSITE blog INNER JOIN BLOGS.ROLLERUSER users ON blog.USERID = users.ID;" & _
         "SELECT NAME, WEBSITEID FROM BLOGS.ROLLER_WEBSITETAG;" & _
         "SELECT entry.ID, entry.Title, 'Blog Entry' as TYPE, entry.PUBTIME, entry.UPDATETIME, users.EXTID, entry.WEBSITEID FROM BLOGS.WEBLOGENTRY entry INNER JOIN BLOGS.ROLLERUSER users ON entry.USERID = users.ID;"


        Dim Category As String = "Blog"

        Try
            Dim con As IBM.Data.DB2.DB2Connection
            Dim ds As New DataSet
            Try

                con = New IBM.Data.DB2.DB2Connection("Database=BLOGS;UserID=" & myServer.DBUserName & ";Password=" & myServer.DBPassword & ";Server=" & myServer.DBHostName & ":" & myServer.DBPort & "")
                con.Open()

                Dim cmd As New IBM.Data.DB2.DB2Command(sql, con)
                Dim adapter As New IBM.Data.DB2.DB2DataAdapter(cmd)
                adapter.Fill(ds)



            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error getting Blog Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            Finally
                Try
                    If con.IsOpen Then
                        con.Close()
                    End If
                Catch ex As Exception

                End Try

            End Try

            Dim dict As New Dictionary(Of String, String)

            Try
                Dim adapter As New VSAdaptor()

                dict.Add("Num_Of_Published_Blogs", ds.Tables(0).Rows(0)("Num_Of_Published_Blogs"))

                dict.Add("Num_Of_Blogs_More_Than_One_Author", ds.Tables(1).Rows(0)("Num_Of_Blogs_More_Than_One_Author"))

                dict.Add("BLOG_LOGINS_LAST_MONTH", ds.Tables(2).Rows(0)("LOGINS_LAST_MONTH"))
                dict.Add("BLOG_LOGINS_LAST_WEEK", ds.Tables(2).Rows(0)("LOGINS_LAST_WEEK"))
                dict.Add("BLOG_LOGINS_LAST_DAY", ds.Tables(2).Rows(0)("LOGINS_LAST_DAY"))

                dict.Add("Num_Of_Bloggers", ds.Tables(3).Rows(0)("Num_Of_Bloggers"))

                dict.Add("Num_Of_Comments", ds.Tables(4).Rows(0)("Num_Of_Comments"))

                dict.Add("Num_Of_Distinct_Blog_Tags", ds.Tables(5).Rows(0)("Num_Of_Distinct_Blog_Tags"))

                dict.Add("Num_Of_Distinct_Entry_Tags", ds.Tables(6).Rows(0)("Num_Of_Distinct_Entry_Tags"))

                dict.Add("Num_Of_Notifications_Sent", ds.Tables(7).Rows(0)("Num_Of_Notifications_Sent"))

                dict.Add("Num_Of_Active_Blogs", ds.Tables(8).Rows(0)("Num_Of_Active_Blogs"))

                dict.Add("Blogs_Created_LAST_MONTH", ds.Tables(9).Rows(0)("Blogs_Created_LAST_MONTH"))
                dict.Add("Blogs_Created_LAST_WEEK", ds.Tables(9).Rows(0)("Blogs_Created_LAST_WEEK"))
                dict.Add("Blogs_Created_LAST_DAY", ds.Tables(9).Rows(0)("Blogs_Created_LAST_DAY"))

                dict.Add("Entry_Created_LAST_MONTH", ds.Tables(10).Rows(0)("Entry_Created_LAST_MONTH"))
                dict.Add("Entry_Created_LAST_WEEK", ds.Tables(10).Rows(0)("Entry_Created_LAST_WEEK"))
                dict.Add("Entry_Created_LAST_DAY", ds.Tables(10).Rows(0)("Entry_Created_LAST_DAY"))

                dict.Add("Comment_Created_LAST_MONTH", ds.Tables(11).Rows(0)("Comment_Created_LAST_MONTH"))
                dict.Add("Comment_Created_LAST_WEEK", ds.Tables(11).Rows(0)("Comment_Created_LAST_WEEK"))
                dict.Add("Comment_Created_LAST_DAY", ds.Tables(11).Rows(0)("Comment_Created_LAST_DAY"))

                Try
                    Dim counter As Integer = 1
                    If (ds.Tables(12).Rows.Count > 0) Then

                        sql = "DELETE FROM IbmConnectionsTopStats WHERE Type = 'NumOfBlogTagUses'; INSERT INTO IbmConnectionsTopStats (ServerId, ServerName, Ranking, Name, UsageCount, Type, DateTime) VALUES "
                        For Each row As DataRow In ds.Tables(12).Rows
                            sql += "('" & myServer.ID & "', '" & myServer.Name & "', '" & counter & "', '" & row("Name").ToString() & "', '" & row("Num_Of_Blog_Tags").ToString() & "', 'NumOfBlogTagUses', GetDate()),"
                            counter += 1
                        Next
                        adapter.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", sql.Substring(0, sql.Length - 1))
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error parsing Blog Stats 1. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
                End Try


                Try
                    Dim counter As Integer = 1
                    If (ds.Tables(13).Rows.Count > 0) Then

                        sql = "DELETE FROM IbmConnectionsTopStats WHERE Type = 'NumOfEntryTagUses'; INSERT INTO IbmConnectionsTopStats (ServerId, ServerName, Ranking, Name, UsageCount, Type, DateTime) VALUES "
                        For Each row As DataRow In ds.Tables(13).Rows
                            sql += "('" & myServer.ID & "', '" & myServer.Name & "', '" & counter & "', '" & row("Name").ToString() & "', '" & row("Num_Of_Entry_Tags").ToString() & "', 'NumOfEntryTagUses', GetDate()),"
                            counter += 1
                        Next
                        adapter.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", sql.Substring(0, sql.Length - 1))
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error parsing Blog Stats 2. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
                End Try


                dict.Add("NUM_OF_BLOGS_BLOGS_CREATED_YESTERDAY", ds.Tables(14).Rows(0)("NUM_OF_BLOGS_CREATED_YESTERDAY"))

                dict.Add("NUM_OF_BLOGS_ENTRIES_CREATED_YESTERDAY", ds.Tables(15).Rows(0)("NUM_OF_ENTRIES_CREATED_YESTERDAY"))

                dict.Add("NUM_OF_BLOGS_COMMENTS_CREATED_YESTERDAY", ds.Tables(16).Rows(0)("NUM_OF_COMMENTS_CREATED_YESTERDAY"))

                dict.Add("NUM_OF_BLOGS_NOTIFICATIONS_CREATED_YESTERDAY", ds.Tables(17).Rows(0)("NUM_OF_NOTIFICATIONS_CREATED_YESTERDAY"))

                sql = "INSERT INTO IbmConnectionsSummaryStats (ServerName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber) VALUES "

                Dim sqlCols As String = ""
                Dim sqlVals As String = ""
                For Each key In dict.Keys
                    Dim Name As String = key
                    Dim Val As String = dict(key)

                    If String.Equals(myServer.IPAddress, "https://connections-as.jnittech.com:9444", StringComparison.CurrentCultureIgnoreCase) Then
                        If key = "NUM_OF_BLOGS_BLOGS_CREATED_YESTERDAY" Or key = "NUM_OF_BLOGS_ENTRIES_CREATED_YESTERDAY" Or key = "NUM_OF_BLOGS_COMMENTS_CREATED_YESTERDAY" Or key = "NUM_OF_BLOGS_NOTIFICATIONS_CREATED_YESTERDAY" Then
                            Val = Int(20 * Rnd()) + 1
                        End If
                    End If

                    sql += "('" & myServer.Name & "', GetDate(), '" & Name.ToUpper() & "', '" & Val & "', '" & GetWeekNumber(Now) & "', '" & Now.Month.ToString() & "', '" & Now.Year.ToString() & "', '" & Now.Day.ToString() & "'),"

                Next


                adapter.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", sql.Substring(0, sql.Length - 1))



                Using sqlConn As SqlClient.SqlConnection = adapter.StartConnectionSQL("VitalSigns")

                    Dim cmd As New SqlClient.SqlCommand()
                    cmd.Connection = sqlConn
                    cmd.CommandText = "DELETE FROM IbmConnectionsObjects WHERE ServerID = @ServerId AND Type = 'Activity'"
                    cmd.Parameters.AddWithValue("@ServerId", myServer.ID)

                    'cmd.ExecuteNonQuery()

                    For Each row As DataRow In ds.Tables(18).Rows()


                        cmd = New SqlClient.SqlCommand()
                        cmd.Connection = sqlConn
                        cmd.CommandText = "INSERT INTO IbmConnectionsObjects (Name, Type, DateCreated, DateLastModified, ServerID, OwnerId, GUID) VALUES " & _
                            "(@Name, 'Blog', @Created, @Modified, @ServerId, (SELECT ID FROM IbmConnectionsUsers WHERE GUID = @GUID AND ServerID = @ServerId), @BlogGUID)"
                        cmd.Parameters.AddWithValue("@Name", row("NAME").ToString())
                        cmd.Parameters.AddWithValue("@Created", row("DATECREATED").ToString())
                        cmd.Parameters.AddWithValue("@Modified", row("LASTMODIFIED").ToString())
                        cmd.Parameters.AddWithValue("@ServerId", myServer.ID)
                        cmd.Parameters.AddWithValue("@GUID", row("EXTID").ToString())
                        cmd.Parameters.AddWithValue("@BlogGUID", row("ID").ToString())
                        cmd.ExecuteNonQuery()

                        For Each tagRow As DataRow In ds.Tables(19).Select("WEBSITEID = '" & row("ID").ToString() & "'")



                            cmd = New SqlClient.SqlCommand()
                            cmd.Connection = sqlConn
                            cmd.CommandText = "IF NOT EXISTS ( SELECT 1 FROM IbmConnectionsTags WHERE Tag = @TagName) BEGIN INSERT INTO IbmConnectionsTags (Tag) VALUES (@TagName) END"
                            cmd.Parameters.AddWithValue("@TagName", tagRow("NAME").ToString())
                            cmd.ExecuteNonQuery()



                            cmd = New SqlClient.SqlCommand()
                            cmd.Connection = sqlConn
                            cmd.CommandText = "INSERT INTO IbmConnectionsObjectTags (ObjectId, TagId) VALUES ((SELECT TOP 1 ID FROM IbmConnectionsObjects WHERE GUID=@GUID AND" & _
                            " Type = 'Blog' AND ServerID = @ServerId ORDER BY ID DESC), (SELECT TOP 1 ID FROM IbmConnectionsTags WHERE Tag = @TagName))"
                            cmd.Parameters.AddWithValue("@TagName", tagRow("NAME").ToString())
                            cmd.Parameters.AddWithValue("@GUID", tagRow("WEBSITEID").ToString())
                            cmd.Parameters.AddWithValue("@ServerId", myServer.ID)

                            cmd.ExecuteNonQuery()

                        Next


                    Next


                    'SELECT entry.ID, entry.Title, 'Blog Entry' as TYPE, entry.PUBTIME, entry.UPDATETIME, users.EXTID, entry.WEBSITEID FROM BLOGS.WEBLOGENTRY entry INNER JOIN BLOGS.ROLLERUSER users ON entry.USERID = users.ID;
                    For Each row As DataRow In ds.Tables(20).Rows()

                        cmd = New SqlClient.SqlCommand()
                        cmd.Connection = sqlConn
                        cmd.CommandText = "INSERT INTO IbmConnectionsObjects (Name, Type, DateCreated, DateLastModified, ServerID, OwnerId, GUID, ParentObjectID) VALUES " & _
                            "(@Name, @Type, @Created, @Modified, @ServerId, (SELECT ID FROM IbmConnectionsUsers WHERE GUID = @GUID AND ServerID = @ServerId), @EntryGUID, (SELECT ID FROM IbmConnectionsObjects WHERE GUID = @BlogGUID AND ServerID = @ServerId))"
                        cmd.Parameters.AddWithValue("@Name", row("TITLE").ToString())
                        cmd.Parameters.AddWithValue("@Created", row("PUBTIME").ToString())
                        cmd.Parameters.AddWithValue("@Modified", row("UPDATETIME").ToString())
                        cmd.Parameters.AddWithValue("@ServerId", myServer.ID)
                        cmd.Parameters.AddWithValue("@GUID", row("EXTID").ToString())
                        cmd.Parameters.AddWithValue("@EntryGUID", row("ID").ToString())
                        cmd.Parameters.AddWithValue("@BlogGUID", row("WEBSITEID").ToString())
                        cmd.Parameters.AddWithValue("@Type", "Blog Entry")
                        cmd.ExecuteNonQuery()

                    Next

                End Using



            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error parsing Blog Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            End Try




        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error in GetBlogStats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try

    End Sub

    Public Sub GetCommunityStats(ByRef myServer As MonitoredItems.IBMConnect)

        Dim sql As String = "SELECT COMMUNITY_TYPE, COUNT(*) Num_Of_Communities FROM SNCOMM.COMMUNITY WHERE DELETE_STATE = 0 GROUP BY COMMUNITY_TYPE;" & _
         "SELECT COALESCE(SUM(CASE WHEN LASTLOGIN > CURRENT_TIMESTAMP - 1 MONTH THEN 1 ELSE 0 END),0) COMMUNITY_LOGIN_LAST_MONTH, COALESCE(SUM(CASE WHEN LASTLOGIN > CURRENT_TIMESTAMP - 7 DAYS THEN 1 ELSE 0 END),0) COMMUNITY_LOGIN_LAST_WEEK, COALESCE(SUM(CASE WHEN LASTLOGIN > CURRENT_TIMESTAMP - 1 DAY THEN 1 ELSE 0 END),0) COMMUNITY_LOGIN_LAST_DAY FROM SNCOMM.MEMBERPROFILE WHERE LASTLOGIN > CURRENT_TIMESTAMP - 1 MONTH;" & _
         "SELECT COUNT(*) NUM_OF_COMMUNITIES_MADE_IN_LAST_MONTH FROM SNCOMM.COMMUNITY WHERE CREATED > CURRENT_TIMESTAMP - 1 MONTH AND DELETE_STATE = 0;" & _
         "SELECT COUNT(*) TAG_COUNT, COUNT(DISTINCT NAME) DISTINCT_TAG_COUNT FROM SNCOMM.TAG;" & _
         "SELECT COUNT(*) TOP_TAG_COUNT, NAME FROM SNCOMM.TAG GROUP BY NAME ORDER BY TOP_TAG_COUNT DESC FETCH FIRST 20 ROWS ONLY;" & _
         "SELECT COUNT(*) NUM_OF_COMMUNITIES_CREATED_YESTERDAY FROM SNCOMM.COMMUNITY WHERE DATE(CREATED) = CURRENT_DATE - 1 DAY AND DELETE_STATE = 0;" & _
         "select c.COMMUNITY_UUID, c.NAME, 'Community' as Type, c.CREATED, c.LASTMOD, c.TAGS_LIST, mp.DIRECTORY_UUID, c.COMMUNITY_TYPE FROM SNCOMM.COMMUNITY c INNER JOIN SNCOMM.MEMBERPROFILE mp ON mp.MEMBER_UUID = c.CREATED_BY WHERE c.DELETE_STATE = 0;" & _
         "Select mp.Display, mp.DIRECTORY_UUID, c.Name, (CASE WHEN m.Role = 1 THEN 'Owner' ELSE 'Member' END) MemberType from sncomm.community c inner join sncomm.member m on c.community_uuid = m.community_uuid and c.delete_state = 0 inner join sncomm.memberprofile mp on mp.member_uuid = m.member_uuid;" & _
         "SELECT r.REF_UUID, r.COMMUNITY_UUID, r.NAME, 'Bookmark' as Type,  r.CREATED, r.LASTMOD, mp.DIRECTORY_UUID FROM SNCOMM.REF r INNER JOIN SNCOMM.MEMBERPROFILE mp ON mp.MEMBER_UUID = r.CREATED_BY;"




        Dim Category As String = "Community"

        Try
            Dim con As IBM.Data.DB2.DB2Connection
            Dim ds As New DataSet
            Try

                con = New IBM.Data.DB2.DB2Connection("Database=SNCOMM;UserID=" & myServer.DBUserName & ";Password=" & myServer.DBPassword & ";Server=" & myServer.DBHostName & ":" & myServer.DBPort & "")
                con.Open()

                Dim cmd As New IBM.Data.DB2.DB2Command(sql, con)
                Dim adapter As New IBM.Data.DB2.DB2DataAdapter(cmd)
                adapter.Fill(ds)



            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error getting Community Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            Finally
                Try
                    If con.IsOpen Then
                        con.Close()
                    End If
                Catch ex As Exception

                End Try

            End Try

            sql = "INSERT INTO TABLENAME (ServerId, StatName, StatValue, Category, DateTime) VALUES "

            Dim dict As New Dictionary(Of String, String)
            Try
                Dim counter As Integer = 0
                Dim adapter As New VSAdaptor()
                For Each row As DataRow In ds.Tables(0).Rows
                    If {"private", "public", "publicInviteOnly"}.Contains(ds.Tables(0).Rows(counter)("COMMUNITY_TYPE")) Then
                        dict.Add("COMMUNITY_TYPE_" & ds.Tables(0).Rows(counter)("COMMUNITY_TYPE"), ds.Tables(0).Rows(counter)("Num_Of_Communities"))
                    Else
                        'Add a print statement here
                    End If
                    counter += 1
                Next

                dict.Add("COMMUNITY_LOGIN_LAST_MONTH", ds.Tables(1).Rows(0)("COMMUNITY_LOGIN_LAST_MONTH"))
                dict.Add("COMMUNITY_LOGIN_LAST_WEEK", ds.Tables(1).Rows(0)("COMMUNITY_LOGIN_LAST_WEEK"))
                dict.Add("COMMUNITY_LOGIN_LAST_DAY", ds.Tables(1).Rows(0)("COMMUNITY_LOGIN_LAST_DAY"))

                dict.Add("NUM_OF_COMMUNITIES_MADE_IN_LAST_MONTH", ds.Tables(2).Rows(0)("NUM_OF_COMMUNITIES_MADE_IN_LAST_MONTH"))

                dict.Add("COMMUNITY_TAG_COUNT", ds.Tables(3).Rows(0)("TAG_COUNT"))
                dict.Add("DISTINCT_COMMUNITY_TAG_COUNT", ds.Tables(3).Rows(0)("DISTINCT_TAG_COUNT"))


                counter = 0
                If (ds.Tables(4).Rows.Count > 0) Then
                    sql = "DELETE FROM IbmConnectionsTopStats WHERE Type = 'NumOfCommunityTagUses';INSERT INTO IbmConnectionsTopStats (ServerId, ServerName, Ranking, Name, UsageCount, Type, DateTime) VALUES "
                    For Each row As DataRow In ds.Tables(4).Rows
                        sql += "('" & myServer.ID & "', '" & myServer.Name & "', '" & counter & "', '" & row("Name").ToString() & "', '" & row("TOP_TAG_COUNT").ToString() & "', 'NumOfCommunityTagUses', GetDate()),"
                        counter += 1
                    Next
                    adapter.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", sql.Substring(0, sql.Length - 1))
                End If

                dict.Add("NUM_OF_COMMUNITIES_COMMUNITIES_CREATED_YESTERDAY", ds.Tables(5).Rows(0)("NUM_OF_COMMUNITIES_CREATED_YESTERDAY"))


                sql = "INSERT INTO IbmConnectionsSummaryStats (ServerName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber) VALUES "

                Dim sqlCols As String = ""
                Dim sqlVals As String = ""
                For Each key In dict.Keys
                    Dim Name As String = key
                    Dim Val As String = dict(key)

                    If String.Equals(myServer.IPAddress, "https://connections-as.jnittech.com:9444", StringComparison.CurrentCultureIgnoreCase) Then
                        If key = "NUM_OF_COMMUNITIES_COMMUNITIES_CREATED_YESTERDAY" Then
                            Val = Int(20 * Rnd()) + 1
                        End If
                    End If

                    sql += "('" & myServer.Name & "', GetDate(), '" & Name.ToUpper() & "', '" & Val & "', '" & GetWeekNumber(Now) & "', '" & Now.Month.ToString() & "', '" & Now.Year.ToString() & "', '" & Now.Day.ToString() & "'),"

                Next


                adapter.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", sql.Substring(0, sql.Length - 1))



                'Handling for other tables
                Dim dt As DataTable = ds.Tables(6)


                Using sqlConn As SqlClient.SqlConnection = adapter.StartConnectionSQL("VitalSigns")

                    Dim cmd As New SqlClient.SqlCommand()
                    cmd.Connection = sqlConn
                    cmd.CommandText = "DELETE FROM IbmConnectionsObjects WHERE ServerID = @ServerId AND Type = 'Community'"
                    cmd.Parameters.AddWithValue("@ServerId", myServer.ID)

                    'cmd.ExecuteNonQuery()

                    For Each row As DataRow In dt.Rows()


                        cmd = New SqlClient.SqlCommand()
                        cmd.Connection = sqlConn
                        cmd.CommandText = "INSERT INTO IbmConnectionsObjects (Name, Type, DateCreated, DateLastModified, ServerID, OwnerId, GUID) VALUES " & _
                            "(@Name, 'Community', @Created, @Modified, @ServerId, (SELECT ID FROM IbmConnectionsUsers WHERE GUID = @GUID AND ServerID = @ServerId), @ObjGUID)"
                        cmd.Parameters.AddWithValue("@Name", row("NAME").ToString())
                        cmd.Parameters.AddWithValue("@Created", row("CREATED").ToString())
                        cmd.Parameters.AddWithValue("@Modified", row("LASTMOD").ToString())
                        cmd.Parameters.AddWithValue("@ServerId", myServer.ID)
                        cmd.Parameters.AddWithValue("@GUID", row("DIRECTORY_UUID").ToString())
                        cmd.Parameters.AddWithValue("@ObjGUID", row("COMMUNITY_UUID").ToString())
                        cmd.ExecuteNonQuery()


                        cmd = New SqlClient.SqlCommand()
                        cmd.Connection = sqlConn
                        cmd.CommandText = "INSERT INTO IbmConnectionsCommunity (ObjectID, CommunityType) VALUES " & _
                            "((SELECT ID FROM IbmConnectionsObjects WHERE GUID = @ObjGUID AND ServerID = @ServerId), @CommunityType)"
                        cmd.Parameters.AddWithValue("@ServerId", myServer.ID)
                        cmd.Parameters.AddWithValue("@ObjGUID", row("COMMUNITY_UUID").ToString())
                        cmd.Parameters.AddWithValue("@CommunityType", row("COMMUNITY_TYPE").ToString())
                        cmd.ExecuteNonQuery()


                        For Each tag As String In row("TAGS_LIST").ToString().Split(",")

                            cmd = New SqlClient.SqlCommand()
                            cmd.Connection = sqlConn
                            cmd.CommandText = "IF NOT EXISTS ( SELECT 1 FROM IbmConnectionsTags WHERE Tag = @TagName) BEGIN INSERT INTO IbmConnectionsTags (Tag) VALUES (@TagName) END"
                            cmd.Parameters.AddWithValue("@TagName", tag)

                            cmd.ExecuteNonQuery()

                            cmd = New SqlClient.SqlCommand()
                            cmd.Connection = sqlConn
                            cmd.CommandText = "INSERT INTO IbmConnectionsObjectTags (ObjectId, TagId) VALUES ((SELECT TOP 1 ID FROM IbmConnectionsObjects WHERE GUID = @CommGUID AND" & _
                            " Type = 'Community' AND ServerID = @ServerId), (SELECT TOP 1 ID FROM IbmConnectionsTags WHERE Tag = @TagName))"
                            cmd.Parameters.AddWithValue("@TagName", tag)
                            cmd.Parameters.AddWithValue("@CommGUID", row("COMMUNITY_UUID").ToString())
                            cmd.Parameters.AddWithValue("@ServerId", myServer.ID)

                            cmd.ExecuteNonQuery()
                        Next

                        For Each bookmarkRow As DataRow In ds.Tables(8).Select("COMMUNITY_UUID = '" & row("COMMUNITY_UUID").ToString() & "'")
                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Doing bookmarks for " & row("COMMUNITY_UUID").ToString(), LogUtilities.LogUtils.LogLevel.Normal)
                            cmd = New SqlClient.SqlCommand()
                            cmd.Connection = sqlConn
                            cmd.CommandText = "INSERT INTO IbmConnectionsObjects (Name, Type, DateCreated, DateLastModified, ServerID, OwnerId, ParentObjectID,GUID) VALUES " & _
                                "(@Name, 'Bookmark', @Created, @Modified, @ServerId, (SELECT ID FROM IbmConnectionsUsers WHERE GUID = @GUID AND ServerID = @ServerId), (SELECT TOP 1 ID FROM IbmConnectionsObjects WHERE " & _
                                "GUID = @CommGUID AND ServerID = @ServerId ORDER BY ID DESC), @BookmarkGUID)"
                            cmd.Parameters.AddWithValue("@Name", bookmarkRow("NAME").ToString())
                            cmd.Parameters.AddWithValue("@Created", bookmarkRow("CREATED").ToString())
                            cmd.Parameters.AddWithValue("@Modified", bookmarkRow("LASTMOD").ToString())
                            cmd.Parameters.AddWithValue("@ServerId", myServer.ID)
                            cmd.Parameters.AddWithValue("@GUID", bookmarkRow("DIRECTORY_UUID").ToString())
                            cmd.Parameters.AddWithValue("@CommGUID", row("COMMUNITY_UUID").ToString())
                            cmd.Parameters.AddWithValue("@BookmarkGUID", bookmarkRow("REF_UUID").ToString())

                            Dim s As String = "INSERT INTO IbmConnectionsObjects (Name, Type, DateCreated, DateLastModified, ServerID, OwnerId, ParentObjectID,GUID) VALUES " & _
                                "(@Name, 'Bookmark', @Created, @Modified, @ServerId, (SELECT ID FROM IbmConnectionsUsers WHERE GUID = @GUID AND ServerID = @ServerId), (SELECT TOP 1 ID FROM IbmConnectionsObjects WHERE " & _
                                "GUID = @CommGUID AND ServerID = @ServerId ORDER BY ID DESC), @BookmarkGUID)".Replace("@Name", bookmarkRow("NAME").ToString()).Replace("@Created", bookmarkRow("CREATED").ToString()).Replace("@Modified", bookmarkRow("LASTMOD").ToString()).Replace("@ServerId", myServer.ID).Replace("@GUID", bookmarkRow("DIRECTORY_UUID").ToString()).Replace("@CommGUID", row("COMMUNITY_UUID").ToString()).Replace("@BookmarkGUID", bookmarkRow("REF_UUID").ToString())

                            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Doing bookmarks for " & row("COMMUNITY_UUID").ToString() & ". sql = " & s, LogUtilities.LogUtils.LogLevel.Normal)

                            cmd.ExecuteNonQuery()

                        Next



                    Next
                End Using


                dt = ds.Tables(7)


                Using sqlConn As SqlClient.SqlConnection = adapter.StartConnectionSQL("VitalSigns")

                    For Each row As DataRow In dt.Rows()
                        Dim cmd As New SqlClient.SqlCommand()
                        cmd.Connection = sqlConn
                        cmd.CommandText = "INSERT INTO IbmConnectionsObjectUsers (ObjectId, UserId) VALUES((SELECT TOP 1 ID FROM IbmConnectionsObjects WHERE Name = @CommName AND " & _
                            "Type = 'Community' AND ServerID = @ServerId), (SELECT TOP 1 ID FROM IbmConnectionsUsers WHERE GUID = @GUID AND ServerID = @ServerId));"
                        cmd.Parameters.AddWithValue("@CommName", row("NAME").ToString())
                        cmd.Parameters.AddWithValue("@GUID", row("DIRECTORY_UUID").ToString())
                        cmd.Parameters.AddWithValue("@ServerId", myServer.ID)

                        cmd.ExecuteNonQuery()
                    Next

                End Using

            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error parsing Community Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            End Try






        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error in GetCommunityStats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try

    End Sub

    Public Sub GetFileStats(ByRef myServer As MonitoredItems.IBMConnect)

        Dim sql As String = "SELECT COALESCE(SUM(CASE WHEN LAST_VISIT > CURRENT_TIMESTAMP - 1 MONTH THEN 1 ELSE 0 END),0) LOGIN_LAST_MONTH, COALESCE(SUM(CASE WHEN LAST_VISIT > CURRENT_TIMESTAMP - 7 DAYS THEN 1 ELSE 0 END),0) LOGIN_LAST_WEEK, COALESCE(SUM(CASE WHEN LAST_VISIT > CURRENT_TIMESTAMP - 1 DAY THEN 1 ELSE 0 END),0) LOGIN_LAST_DAY FROM FILES.USER WHERE LAST_VISIT > CURRENT_TIMESTAMP - 1 MONTH;" & _
         "SELECT COUNT(*) TOTAL_NUM_OF_FILES, COALESCE(SUM(CASE WHEN FILE_SIZE >= 1024 AND FILE_SIZE < 1024*1024 THEN 1 ELSE 0 END),0) FILES_OVER_KB, COALESCE(SUM(CASE WHEN FILE_SIZE >= 1024*1024 AND FILE_SIZE < 1024*1024*1024 THEN 1 ELSE 0 END),0) FILES_OVER_MB, COALESCE(SUM(CASE WHEN FILE_SIZE >= 1024*1024*1024 THEN 1 ELSE 0 END),0) FILES_OVER_GB FROM FILES.MEDIA;" & _
         "select COUNT(*) NUM_OF_USERS_WITH_NO_FILES FROM FILES.USER WHERE ID NOT IN (SELECT OWNER_USER_ID FROM FILES.MEDIA);" & _
         "SELECT FILES.USER.NAME, SUM(FILES.MEDIA.FILE_SIZE) FILE_SIZE FROM FILES.MEDIA INNER JOIN FILES.USER ON FILES.USER.ID = FILES.MEDIA.OWNER_USER_ID GROUP BY FILES.USER.NAME;" & _
         "SELECT COUNT(*) NUM_OF_FILES_IN_TRASH FROM FILES.R_MEDIA;" & _
         "SELECT COALESCE(SUM(CASE WHEN LAST_UPDATE > CURRENT_TIMESTAMP - 1 MONTH THEN 1 ELSE 0 END),0) NUM_OF_FILES_UPDATED_LAST_MONTH, COALESCE(SUM(CASE WHEN LAST_UPDATE > CURRENT_TIMESTAMP - 7 DAYS THEN 1 ELSE 0 END),0) NUM_OF_FILES_UPDATED_LAST_WEEK, COALESCE(SUM(CASE WHEN LAST_UPDATE > CURRENT_TIMESTAMP - 1 DAY THEN 1 ELSE 0 END),0) NUM_OF_FILES_UPDATED_LAST_DAY FROM FILES.MEDIA WHERE LAST_UPDATE > CURRENT_TIMESTAMP - 1 MONTH;" & _
         "SELECT CASE VISIBILITY_COMPUTED WHEN '1' THEN 'Everyone' WHEN '2' THEN 'OnlyMe' WHEN '3' THEN 'Shared' END SHAREDWITH , COUNT(*) COUNT  FROM FILES.MEDIA GROUP BY VISIBILITY_COMPUTED;" & _
         "SELECT COUNT(*) NUM_OF_FILES_WITH_A_REVISION FROM (SELECT MEDIA_ID FROM FILES.MEDIA_REVISION GROUP BY MEDIA_ID HAVING COUNT(*) > 1);" & _
         "SELECT COUNT(DISTINCT MEDIA_ID) FILES_WITH_COMMENTS FROM FILES.MEDIA_COMMENT;" & _
         "SELECT COUNT(DISTINCT MEDIA_ID) FILES_WITH_SHARES FROM FILES.MEDIA_SHARE;" & _
         "SELECT COALESCE(SUM(CASE WHEN DOWNLOADED_AT > CURRENT_TIMESTAMP - 1 MONTH THEN 1 ELSE 0 END),0) DOWNLOADS_LAST_MONTH, COALESCE(SUM(CASE WHEN DOWNLOADED_AT > CURRENT_TIMESTAMP - 7 DAYS THEN 1 ELSE 0 END),0) DOWNLOADS_LAST_WEEK, COALESCE(SUM(CASE WHEN DOWNLOADED_AT > CURRENT_TIMESTAMP - 1 DAY THEN 1 ELSE 0 END),0) DOWNLOADS_LAST_DAY FROM FILES.MEDIA_DOWNLOAD WHERE DOWNLOADED_AT > CURRENT_TIMESTAMP - 1 MONTH;" & _
         "SELECT COUNT(*) TOTAL_NUM_OF_TAGS FROM FILES.MEDIA_TO_TAG;" & _
         "SELECT COUNT(*) NUM_OF_FILES_CREATED_YESTERDAY FROM FILES.MEDIA WHERE DATE(CREATE_DATE) = CURRENT_DATE - 1 DAY;" & _
         "SELECT COUNT(*) NUM_OF_FILES_UPDATED_YESTERDAY FROM FILES.MEDIA WHERE DATE(LAST_UPDATE) = CURRENT_DATE - 1 DAY;" & _
         "SELECT COUNT(*) NUM_OF_FILES_DOWNLOADED_YESTERDAY FROM FILES.MEDIA_DOWNLOAD WHERE DATE(DOWNLOADED_AT) = CURRENT_DATE - 1 DAY;" & _
         "SELECT COUNT(*) NUM_OF_FILES_REVISIONED_YESTERDAY FROM FILES.MEDIA_REVISION WHERE DATE(LAST_UPDATE) = CURRENT_DATE - 1 DAY;"


        Dim Category As String = "File"

        Try
            Dim con As IBM.Data.DB2.DB2Connection
            Dim ds As New DataSet
            Try

                con = New IBM.Data.DB2.DB2Connection("Database=FILES;UserID=" & myServer.DBUserName & ";Password=" & myServer.DBPassword & ";Server=" & myServer.DBHostName & ":" & myServer.DBPort & "")
                con.Open()

                Dim cmd As New IBM.Data.DB2.DB2Command(sql, con)
                Dim adapter As New IBM.Data.DB2.DB2DataAdapter(cmd)
                adapter.Fill(ds)



            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error getting Files Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            Finally
                Try
                    If con.IsOpen Then
                        con.Close()
                    End If
                Catch ex As Exception

                End Try

            End Try


            Dim dict As New Dictionary(Of String, String)
            Try
                Dim adapter As New VSAdaptor()

                dict.Add("FILE_LOGIN_LAST_MONTH", ds.Tables(0).Rows(0)("LOGIN_LAST_MONTH"))
                dict.Add("FILE_LOGIN_LAST_WEEK", ds.Tables(0).Rows(0)("LOGIN_LAST_WEEK"))
                dict.Add("FILE_LOGIN_LAST_DAY", ds.Tables(0).Rows(0)("LOGIN_LAST_DAY"))

                dict.Add("TOTAL_NUM_OF_FILES", ds.Tables(1).Rows(0)("TOTAL_NUM_OF_FILES"))
                dict.Add("FILES_OVER_KB", ds.Tables(1).Rows(0)("FILES_OVER_KB"))
                dict.Add("FILES_OVER_MB", ds.Tables(1).Rows(0)("FILES_OVER_MB"))
                dict.Add("FILES_OVER_GB", ds.Tables(1).Rows(0)("FILES_OVER_GB"))

                dict.Add("NUM_OF_USERS_WITH_NO_FILES", ds.Tables(2).Rows(0)("NUM_OF_USERS_WITH_NO_FILES"))



                dict.Add("NUM_OF_FILES_IN_TRASH", ds.Tables(4).Rows(0)("NUM_OF_FILES_IN_TRASH"))

                dict.Add("NUM_OF_FILES_UPDATED_LAST_MONTH", ds.Tables(5).Rows(0)("NUM_OF_FILES_UPDATED_LAST_MONTH"))
                dict.Add("NUM_OF_FILES_UPDATED_LAST_WEEK", ds.Tables(5).Rows(0)("NUM_OF_FILES_UPDATED_LAST_WEEK"))
                dict.Add("NUM_OF_FILES_UPDATED_LAST_DAY", ds.Tables(5).Rows(0)("NUM_OF_FILES_UPDATED_LAST_DAY"))

                Dim counter As Integer = 0
                For Each row As DataRow In ds.Tables(6).Rows
                    If {"Everyone", "OnlyMe", "Shared"}.Contains(ds.Tables(6).Rows(counter)("SHAREDWITH")) Then
                        dict.Add("FILES_PERMISSIONS_FOR_" & ds.Tables(6).Rows(counter)("SHAREDWITH"), ds.Tables(6).Rows(counter)("COUNT"))
                    Else
                        'Add a print statement here
                    End If
                    counter += 1
                Next

                dict.Add("NUM_OF_FILES_WITH_A_REVISION", ds.Tables(7).Rows(0)("NUM_OF_FILES_WITH_A_REVISION"))

                dict.Add("FILES_WITH_COMMENTS", ds.Tables(8).Rows(0)("FILES_WITH_COMMENTS"))

                dict.Add("FILES_WITH_SHARES", ds.Tables(9).Rows(0)("FILES_WITH_SHARES"))

                dict.Add("FILE_DOWNLOADS_LAST_MONTH", ds.Tables(10).Rows(0)("DOWNLOADS_LAST_MONTH"))
                dict.Add("FILE_DOWNLOADS_LAST_WEEK", ds.Tables(10).Rows(0)("DOWNLOADS_LAST_WEEK"))
                dict.Add("FILE_DOWNLOADS_LAST_DAY", ds.Tables(10).Rows(0)("DOWNLOADS_LAST_DAY"))

                dict.Add("FILE_TOTAL_NUM_OF_TAGS", ds.Tables(11).Rows(0)("TOTAL_NUM_OF_TAGS"))

                dict.Add("NUM_OF_FILES_FILES_CREATED_YESTERDAY", ds.Tables(12).Rows(0)("NUM_OF_FILES_CREATED_YESTERDAY"))

                dict.Add("NUM_OF_FILES_FILES_UPDATED_YESTERDAY", ds.Tables(13).Rows(0)("NUM_OF_FILES_UPDATED_YESTERDAY"))

                dict.Add("NUM_OF_FILES_FILES_DOWNLOADED_YESTERDAY", ds.Tables(14).Rows(0)("NUM_OF_FILES_DOWNLOADED_YESTERDAY"))

                dict.Add("NUM_OF_FILES_FILES_REVISIONED_YESTERDAY", ds.Tables(15).Rows(0)("NUM_OF_FILES_REVISIONED_YESTERDAY"))


                sql = "INSERT INTO IbmConnectionsSummaryStats (ServerName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber) VALUES "

                Dim sqlCols As String = ""
                Dim sqlVals As String = ""
                For Each key In dict.Keys
                    Dim Name As String = key
                    Dim Val As String = dict(key)

                    If String.Equals(myServer.IPAddress, "https://connections-as.jnittech.com:9444", StringComparison.CurrentCultureIgnoreCase) Then
                        If key = "NUM_OF_FILES_FILES_CREATED_YESTERDAY" Or key = "NUM_OF_FILES_FILES_UPDATED_YESTERDAY" Or key = "NUM_OF_FILES_FILES_DOWNLOADED_YESTERDAY" Or key = "NUM_OF_FILES_FILES_REVISIONED_YESTERDAY" Then
                            Val = Int(20 * Rnd()) + 1
                        End If
                    End If

                    sql += "('" & myServer.Name & "', GetDate(), '" & Name.ToUpper() & "', '" & Val & "', '" & GetWeekNumber(Now) & "', '" & Now.Month.ToString() & "', '" & Now.Year.ToString() & "', '" & Now.Day.ToString() & "'),"

                Next


                adapter.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", sql.Substring(0, sql.Length - 1))



            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error parsing Files Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            End Try



        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error in GetFilesStats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try

    End Sub

    Public Sub GetBookmarksStats(ByRef myServer As MonitoredItems.IBMConnect)

        Dim sql As String = "SELECT COUNT(*) NUM_OF_BOOKMARKS_BOOKMARKS FROM DOGEAR.LINK WHERE DELETED = 0;" & _
            "SELECT COUNT(*) NUM_OF_BOOKMARKS_BOOKMARKS_CREATED_YESTERDAY FROM DOGEAR.LINK WHERE DELETED = 0 AND DATE(DATE) = CURRENT_DATE - 1 DAY;" & _
            "SELECT COUNT(DISTINCT URL) NUM_OF_DISTINCT_BOOKMARK_URLS FROM DOGEAR.URL WHERE LINKCOUNT > 0;" & _
            "SELECT url.LINK_ID, url.TITLE, 'Bookmark' as TYPE, url.DATE, url.MODIFIED, users.MEMBER_ID FROM DOGEAR.LINK url INNER JOIN DOGEAR.PERSON users ON users.PERSON_ID = url.PERSON WHERE url.DELETED = 0;" & _
            "SELECT TAG, LINK_ID FROM DOGEAR.TAGS WHERE LINK_ID IN ( SELECT LINK_ID FROM DOGEAR.LINK WHERE DELETED = 0 );"

        Dim Category As String = "Bookmark"

        Try
            Dim con As IBM.Data.DB2.DB2Connection
            Dim ds As New DataSet
            Try

                con = New IBM.Data.DB2.DB2Connection("Database=DOGEAR;UserID=" & myServer.DBUserName & ";Password=" & myServer.DBPassword & ";Server=" & myServer.DBHostName & ":" & myServer.DBPort & "")
                con.Open()

                Dim cmd As New IBM.Data.DB2.DB2Command(sql, con)
                Dim adapter As New IBM.Data.DB2.DB2DataAdapter(cmd)
                adapter.Fill(ds)



            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error getting bookmark Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            Finally
                Try
                    If con.IsOpen Then
                        con.Close()
                    End If
                Catch ex As Exception

                End Try

            End Try


            Dim dict As New Dictionary(Of String, String)
            Try
                Dim adapter As New VSAdaptor()

                dict.Add("NUM_OF_BOOKMARKS_BOOKMARKS", ds.Tables(0).Rows(0)("NUM_OF_BOOKMARKS_BOOKMARKS"))

                dict.Add("NUM_OF_BOOKMARKS_BOOKMARKS_CREATED_YESTERDAY", ds.Tables(1).Rows(0)("NUM_OF_BOOKMARKS_BOOKMARKS_CREATED_YESTERDAY"))

                dict.Add("NUM_OF_DISTINCT_BOOKMARK_URLS", ds.Tables(2).Rows(0)("NUM_OF_DISTINCT_BOOKMARK_URLS"))

                sql = "INSERT INTO IbmConnectionsSummaryStats (ServerName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber) VALUES "

                Dim sqlCols As String = ""
                Dim sqlVals As String = ""
                For Each key In dict.Keys
                    Dim Name As String = key
                    Dim Val As String = dict(key)

                    If String.Equals(myServer.IPAddress, "https://connections-as.jnittech.com:9444", StringComparison.CurrentCultureIgnoreCase) Then
                        If key = "NUM_OF_BOOKMARKS_BOOKMARKS_CREATED_YESTERDAY" Then
                            Val = Int(20 * Rnd()) + 1
                        End If
                    End If

                    sql += "('" & myServer.Name & "', GetDate(), '" & Name.ToUpper() & "', '" & Val & "', '" & GetWeekNumber(Now) & "', '" & Now.Month.ToString() & "', '" & Now.Year.ToString() & "', '" & Now.Day.ToString() & "'),"

                Next


                adapter.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", sql.Substring(0, sql.Length - 1))


                Using sqlConn As SqlClient.SqlConnection = adapter.StartConnectionSQL("VitalSigns")

                    Dim cmd As New SqlClient.SqlCommand()

                    For Each row As DataRow In ds.Tables(3).Rows()


                        cmd = New SqlClient.SqlCommand()
                        cmd.Connection = sqlConn
                        cmd.CommandText = "INSERT INTO IbmConnectionsObjects (Name, Type, DateCreated, DateLastModified, ServerID, OwnerId, GUID) VALUES " & _
                            "(@Name, @Type, @Created, @Modified, @ServerId, (SELECT ID FROM IbmConnectionsUsers WHERE GUID = @GUID AND ServerID = @ServerId), @BookmarkGUID)"
                        cmd.Parameters.AddWithValue("@Name", row("TITLE").ToString())
                        cmd.Parameters.AddWithValue("@Created", row("DATE").ToString())
                        cmd.Parameters.AddWithValue("@Modified", row("MODIFIED").ToString())
                        cmd.Parameters.AddWithValue("@ServerId", myServer.ID)
                        cmd.Parameters.AddWithValue("@GUID", row("MEMBER_ID").ToString())
                        cmd.Parameters.AddWithValue("@BookmarkGUID", row("LINK_ID").ToString())
                        cmd.Parameters.AddWithValue("@Type", "Bookmark")
                        cmd.ExecuteNonQuery()

                    Next

                    For Each tagRow As DataRow In ds.Tables(4).Rows()

                        cmd = New SqlClient.SqlCommand()
                        cmd.Connection = sqlConn
                        cmd.CommandText = "IF NOT EXISTS ( SELECT 1 FROM IbmConnectionsTags WHERE Tag = @TagName) BEGIN INSERT INTO IbmConnectionsTags (Tag) VALUES (@TagName) END"
                        cmd.Parameters.AddWithValue("@TagName", tagRow("TAG").ToString())
                        cmd.ExecuteNonQuery()

                        cmd = New SqlClient.SqlCommand()
                        cmd.Connection = sqlConn
                        cmd.CommandText = "INSERT INTO IbmConnectionsObjectTags (ObjectId, TagId) VALUES ((SELECT TOP 1 ID FROM IbmConnectionsObjects WHERE GUID=@GUID AND" & _
                        " Type = 'Bookmark' AND ServerID = @ServerId ORDER BY ID DESC), (SELECT TOP 1 ID FROM IbmConnectionsTags WHERE Tag = @TagName))"
                        cmd.Parameters.AddWithValue("@TagName", tagRow("TAG").ToString())
                        cmd.Parameters.AddWithValue("@GUID", tagRow("LINK_ID").ToString())
                        cmd.Parameters.AddWithValue("@ServerId", myServer.ID)
                        cmd.ExecuteNonQuery()

                    Next

                End Using


            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error parsing Bookmark Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            End Try



        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error in GetBookmarksStats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try

    End Sub

    Public Sub GetForumStats(ByRef myServer As MonitoredItems.IBMConnect)

        Dim sql As String = "SELECT COUNT(*) NUM_OF_FORUMS_FORUMS FROM FORUM.DF_NODE WHERE NODETYPE = 'application/forum' AND NODEALIAS <> 'community' AND STATE = 0;" & _
            "SELECT COUNT(*) NUM_OF_FORUMS_TOPICS FROM FORUM.DF_NODE WHERE NODETYPE = 'forum/topic' AND NODEALIAS <> 'community' AND STATE = 0;" & _
            "SELECT COUNT(*) NUM_OF_FORUMS_REPLIES FROM FORUM.DF_NODE WHERE NODETYPE = 'forum/reply' AND NODEALIAS <> 'community' AND STATE = 0;" & _
            "SELECT COUNT(*) NUM_OF_FORUMS_FORUMS_CREATED_YESTERDAY FROM FORUM.DF_NODE WHERE NODETYPE = 'application/forum' AND NODEALIAS <> 'community' AND STATE = 0 AND DATE(CREATED) = CURRENT_DATE - 1 DAY;" & _
            "SELECT COUNT(*) NUM_OF_FORUMS_TOPICS_CREATED_YESTERDAY FROM FORUM.DF_NODE WHERE NODETYPE = 'forum/topic' AND NODEALIAS <> 'community' AND STATE = 0 AND DATE(CREATED) = CURRENT_DATE - 1 DAY;" & _
            "SELECT COUNT(*) NUM_OF_FORUMS_REPLIES_CREATED_YESTERDAY FROM FORUM.DF_NODE WHERE NODETYPE = 'forum/reply' AND NODEALIAS <> 'community' AND STATE = 0 AND DATE(CREATED) = CURRENT_DATE - 1 DAY;"




        Dim Category As String = "File"

        Try
            Dim con As IBM.Data.DB2.DB2Connection
            Dim ds As New DataSet
            Try

                con = New IBM.Data.DB2.DB2Connection("Database=FORUM;UserID=" & myServer.DBUserName & ";Password=" & myServer.DBPassword & ";Server=" & myServer.DBHostName & ":" & myServer.DBPort & "")
                con.Open()

                Dim cmd As New IBM.Data.DB2.DB2Command(sql, con)
                Dim adapter As New IBM.Data.DB2.DB2DataAdapter(cmd)
                adapter.Fill(ds)



            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error getting Forum Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            Finally
                Try
                    If con.IsOpen Then
                        con.Close()
                    End If
                Catch ex As Exception

                End Try

            End Try


            Dim dict As New Dictionary(Of String, String)
            Try
                Dim adapter As New VSAdaptor()

                dict.Add("NUM_OF_FORUMS_FORUMS", ds.Tables(0).Rows(0)("NUM_OF_FORUMS_FORUMS"))

                dict.Add("NUM_OF_FORUMS_TOPICS", ds.Tables(1).Rows(0)("NUM_OF_FORUMS_TOPICS"))

                dict.Add("NUM_OF_FORUMS_REPLIES", ds.Tables(2).Rows(0)("NUM_OF_FORUMS_REPLIES"))

                dict.Add("NUM_OF_FORUMS_FORUMS_CREATED_YESTERDAY", ds.Tables(3).Rows(0)("NUM_OF_FORUMS_FORUMS_CREATED_YESTERDAY"))

                dict.Add("NUM_OF_FORUMS_TOPICS_CREATED_YESTERDAY", ds.Tables(4).Rows(0)("NUM_OF_FORUMS_TOPICS_CREATED_YESTERDAY"))

                dict.Add("NUM_OF_FORUMS_REPLIES_CREATED_YESTERDAY", ds.Tables(5).Rows(0)("NUM_OF_FORUMS_REPLIES_CREATED_YESTERDAY"))



                sql = "INSERT INTO IbmConnectionsSummaryStats (ServerName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber) VALUES "

                Dim sqlCols As String = ""
                Dim sqlVals As String = ""
                For Each key In dict.Keys
                    Dim Name As String = key
                    Dim Val As String = dict(key)

                    If String.Equals(myServer.IPAddress, "https://connections-as.jnittech.com:9444", StringComparison.CurrentCultureIgnoreCase) Then

                        If key = "NUM_OF_FORUMS_FORUMS_CREATED_YESTERDAY" Or key = "NUM_OF_FORUMS_TOPICS_CREATED_YESTERDAY" Or key = "NUM_OF_FORUMS_REPLIES_CREATED_YESTERDAY" Then
                            Val = Int(20 * Rnd()) + 1
                        End If
                    End If

                    sql += "('" & myServer.Name & "', GetDate(), '" & Name.ToUpper() & "', '" & Val & "', '" & GetWeekNumber(Now) & "', '" & Now.Month.ToString() & "', '" & Now.Year.ToString() & "', '" & Now.Day.ToString() & "'),"

                Next


                adapter.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", sql.Substring(0, sql.Length - 1))



            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error parsing Forum Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            End Try



        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error in GetForumStats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try

    End Sub

    Public Sub GetWikiStats(ByRef myServer As MonitoredItems.IBMConnect)

        Dim sql As String = "SELECT COUNT(*) NUM_OF_WIKIS_WIKIS FROM WIKIS.LIBRARY;" & _
            "SELECT COUNT(*) NUM_OF_WIKIS_WIKIS_CREATED_YESTERDAY FROM WIKIS.LIBRARY WHERE DATE(CREATE_DATE) = CURRENT_DATE - 1 DAY;" & _
            "SELECT COUNT(*) NUM_OF_WIKIS_PAGES FROM WIKIS.MEDIA;" & _
            "SELECT COUNT(*) NUM_OF_WIKIS_PAGES_CREATED_YESTERDAY FROM WIKIS.MEDIA WHERE DATE(CREATE_DATE) = CURRENT_DATE - 1 DAY;" & _
            "SELECT COUNT(*) NUM_OF_WIKIS_REVISIONS FROM WIKIS.MEDIA_REVISION;" & _
            "SELECT COUNT(*) NUM_OF_WIKIS_REVISIONS_EDITED_YESTERDAY FROM WIKIS.MEDIA_REVISION WHERE DATE(CREATE_DATE) = CURRENT_DATE - 1 DAY;" & _
            "SELECT HEX(wiki.ID) as ID, wiki.LABEL, 'Wiki' as Type, wiki.CREATE_DATE, wiki.LAST_UPDATE, user.DIRECTORY_ID FROM WIKIS.LIBRARY wiki INNER JOIN WIKIS.USER user ON wiki.OWNER_USER_ID = user.ID;" & _
            "SELECT HEX(lib.LIBRARY_ID) as LIBRARY_ID, tag.TAG FROM WIKIS.LIBRARY_TO_TAG lib INNER JOIN WIKIS.TAG tag ON tag.ID = lib.TAG_ID;" & _
            "SELECT HEX(media.ID) as ID, media.LABEL, 'Wiki Entry' as TYPE, media.CREATE_DATE, media.LAST_UPDATE, user.DIRECTORY_ID, HEX(media.LIBRARY_ID) as LIBRARY_ID FROM WIKIS.MEDIA media INNER JOIN WIKIS.USER user ON user.ID = media.OWNER_USER_ID;"


        Dim Category As String = "WIKIS"

        Try
            Dim con As IBM.Data.DB2.DB2Connection
            Dim ds As New DataSet
            Try

                con = New IBM.Data.DB2.DB2Connection("Database=WIKIS;UserID=" & myServer.DBUserName & ";Password=" & myServer.DBPassword & ";Server=" & myServer.DBHostName & ":" & myServer.DBPort & "")
                con.Open()

                Dim cmd As New IBM.Data.DB2.DB2Command(sql, con)
                Dim adapter As New IBM.Data.DB2.DB2DataAdapter(cmd)
                adapter.Fill(ds)



            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error getting Wiki Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            Finally
                Try
                    If con.IsOpen Then
                        con.Close()
                    End If
                Catch ex As Exception

                End Try

            End Try


            Dim dict As New Dictionary(Of String, String)
            Try
                Dim adapter As New VSAdaptor()

                dict.Add("NUM_OF_WIKIS_WIKIS", ds.Tables(0).Rows(0)("NUM_OF_WIKIS_WIKIS"))

                dict.Add("NUM_OF_WIKIS_WIKIS_CREATED_YESTERDAY", ds.Tables(1).Rows(0)("NUM_OF_WIKIS_WIKIS_CREATED_YESTERDAY"))

                dict.Add("NUM_OF_WIKIS_PAGES", ds.Tables(2).Rows(0)("NUM_OF_WIKIS_PAGES"))

                dict.Add("NUM_OF_WIKIS_PAGES_CREATED_YESTERDAY", ds.Tables(3).Rows(0)("NUM_OF_WIKIS_PAGES_CREATED_YESTERDAY"))

                dict.Add("NUM_OF_WIKIS_REVISIONS", ds.Tables(4).Rows(0)("NUM_OF_WIKIS_REVISIONS"))

                dict.Add("NUM_OF_WIKIS_REVISIONS_EDITED_YESTERDAY", ds.Tables(5).Rows(0)("NUM_OF_WIKIS_REVISIONS_EDITED_YESTERDAY"))


                sql = "INSERT INTO IbmConnectionsSummaryStats (ServerName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber) VALUES "

                Dim sqlCols As String = ""
                Dim sqlVals As String = ""
                For Each key In dict.Keys
                    Dim Name As String = key
                    Dim Val As String = dict(key)

                    If String.Equals(myServer.IPAddress, "https://connections-as.jnittech.com:9444", StringComparison.CurrentCultureIgnoreCase) Then
                        If key = "NUM_OF_WIKIS_WIKIS_CREATED_YESTERDAY" Or key = "NUM_OF_WIKIS_PAGES_CREATED_YESTERDAY" Or key = "NUM_OF_WIKIS_REVISIONS_EDITED_YESTERDAY" Then
                            Val = Int(20 * Rnd()) + 1
                        End If
                    End If

                    sql += "('" & myServer.Name & "', GetDate(), '" & Name.ToUpper() & "', '" & Val & "', '" & GetWeekNumber(Now) & "', '" & Now.Month.ToString() & "', '" & Now.Year.ToString() & "', '" & Now.Day.ToString() & "'),"

                Next


                adapter.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", sql.Substring(0, sql.Length - 1))


                Using sqlConn As SqlClient.SqlConnection = adapter.StartConnectionSQL("VitalSigns")

                    Dim cmd As New SqlClient.SqlCommand()

                    For Each row As DataRow In ds.Tables(6).Rows()

                        cmd = New SqlClient.SqlCommand()
                        cmd.Connection = sqlConn
                        cmd.CommandText = "INSERT INTO IbmConnectionsObjects (Name, Type, DateCreated, DateLastModified, ServerID, OwnerId, GUID) VALUES " & _
                            "(@Name, @Type, @Created, @Modified, @ServerId, (SELECT ID FROM IbmConnectionsUsers WHERE GUID = @UserGUID AND ServerID = @ServerId), @ObjectGUID)"
                        cmd.Parameters.AddWithValue("@Name", row("LABEL").ToString())
                        cmd.Parameters.AddWithValue("@Created", row("CREATE_DATE").ToString())
                        cmd.Parameters.AddWithValue("@Modified", row("LAST_UPDATE").ToString())
                        cmd.Parameters.AddWithValue("@ServerId", myServer.ID)
                        cmd.Parameters.AddWithValue("@UserGUID", row("DIRECTORY_ID").ToString())
                        cmd.Parameters.AddWithValue("@ObjectGUID", HexToGUID(row("ID").ToString()))
                        cmd.Parameters.AddWithValue("@Type", "Wiki")
                        cmd.ExecuteNonQuery()

                    Next

                    For Each tagRow As DataRow In ds.Tables(7).Rows()

                        cmd = New SqlClient.SqlCommand()
                        cmd.Connection = sqlConn
                        cmd.CommandText = "IF NOT EXISTS ( SELECT 1 FROM IbmConnectionsTags WHERE Tag = @TagName) BEGIN INSERT INTO IbmConnectionsTags (Tag) VALUES (@TagName) END"
                        cmd.Parameters.AddWithValue("@TagName", tagRow("TAG").ToString())
                        cmd.ExecuteNonQuery()

                        cmd = New SqlClient.SqlCommand()
                        cmd.Connection = sqlConn
                        cmd.CommandText = "INSERT INTO IbmConnectionsObjectTags (ObjectId, TagId) VALUES ((SELECT TOP 1 ID FROM IbmConnectionsObjects WHERE GUID=@ObjectGUID AND" & _
                        " Type = @Type AND ServerID = @ServerId ORDER BY ID DESC), (SELECT TOP 1 ID FROM IbmConnectionsTags WHERE Tag = @TagName))"
                        cmd.Parameters.AddWithValue("@TagName", tagRow("TAG").ToString())
                        cmd.Parameters.AddWithValue("@ObjectGUID", HexToGUID(tagRow("LIBRARY_ID").ToString()))
                        cmd.Parameters.AddWithValue("@ServerId", myServer.ID)
                        cmd.Parameters.AddWithValue("@Type", "Wiki")
                        cmd.ExecuteNonQuery()

                    Next

                    For Each row As DataRow In ds.Tables(8).Rows()

                        cmd = New SqlClient.SqlCommand()
                        cmd.Connection = sqlConn
                        cmd.CommandText = "INSERT INTO IbmConnectionsObjects (Name, Type, DateCreated, DateLastModified, ServerID, OwnerId, GUID, ParentObjectID) VALUES " & _
                            "(@Name, @Type, @Created, @Modified, @ServerId, (SELECT ID FROM IbmConnectionsUsers WHERE GUID = @GUID AND ServerID = @ServerId), @EntryGUID, (SELECT ID FROM IbmConnectionsObjects WHERE GUID = @WikiGUID AND ServerID = @ServerId))"
                        cmd.Parameters.AddWithValue("@Name", row("LABEL").ToString())
                        cmd.Parameters.AddWithValue("@Created", row("CREATE_DATE").ToString())
                        cmd.Parameters.AddWithValue("@Modified", row("LAST_UPDATE").ToString())
                        cmd.Parameters.AddWithValue("@ServerId", myServer.ID)
                        cmd.Parameters.AddWithValue("@GUID", row("DIRECTORY_ID").ToString())
                        cmd.Parameters.AddWithValue("@EntryGUID", HexToGUID(row("ID").ToString()))
                        cmd.Parameters.AddWithValue("@WikiGUID", HexToGUID(row("LIBRARY_ID").ToString()))
                        cmd.Parameters.AddWithValue("@Type", "Wiki Entry")
                        cmd.ExecuteNonQuery()

                    Next

                End Using



            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error parsing Wiki Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            End Try



        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error in GetWikiStats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try

    End Sub

    Public Sub GetProfileStats(ByRef myServer As MonitoredItems.IBMConnect)

        Dim sql As String = "SELECT COUNT(*) NUM_OF_PROFILES_WITH_NO_PICTURE FROM EMPINST.EMPLOYEE WHERE PROF_KEY NOT IN (SELECT PROF_KEY FROM EMPINST.PHOTO);" & _
            "SELECT COUNT(*) NUM_OF_PROFILES_NOT_RECENTLY_UPDATED FROM EMPINST.EMPLOYEE WHERE PROF_LAST_UPDATE > CURRENT_DATE - 90 DAYS;" & _
            "SELECT COUNT(*) NUM_OF_PROFILES_WITH_NO_PRONUNCIATION FROM EMPINST.EMPLOYEE WHERE PROF_KEY NOT IN (SELECT PROF_KEY FROM EMPINST.PRONUNCIATION); " & _
            "SELECT COUNT(*) NUM_OF_PROFILES_MANAGERS FROM EMPINST.EMPLOYEE WHERE PROF_UID IN (SELECT PROF_MANAGER_UID FROM EMPINST.EMPLOYEE);" & _
            "SELECT COUNT(*) NUM_OF_PROFILES_WITH_NO_MANAGER FROM EMPINST.EMPLOYEE WHERE PROF_MANAGER_UID IS NOT NULL;" & _
            "SELECT COUNT(DISTINCT PROF_UID) NUM_OF_PROFILES_WITH_NO_JOB_HIERARCHY FROM (SELECT PROF_UID FROM EMPINST.EMPLOYEE WHERE PROF_MANAGER_UID IS NULL AND PROF_UID NOT IN (SELECT PROF_MANAGER_UID FROM EMPINST.EMPLOYEE WHERE PROF_MANAGER_UID IS NOT NULL));" & _
            "SELECT COUNT(DISTINCT PROF_UID) NUM_OF_PROFILES_WITH_JOB_HIERARCHY FROM (SELECT PROF_UID FROM EMPINST.EMPLOYEE WHERE PROF_MANAGER_UID IS NOT NULL UNION SELECT PROF_UID FROM EMPINST.EMPLOYEE WHERE PROF_UID IN (SELECT PROF_MANAGER_UID FROM EMPINST.EMPLOYEE));" & _
            "SELECT COUNT(*) NUM_OF_PROFILES_WITH_NO_JOB_TITLE FROM EMPINST.EMPLOYEE WHERE PROF_JOB_RESPONSIBILITIES IS NULL;" & _
            "SELECT AVG(DAYS(CURRENT_DATE) - DAYS(PROF_LAST_UPDATE)) PROFILES_AVERAGE_DAYS_SINCE_EDIT FROM EMPINST.EMPLOYEE;" & _
            "SELECT COUNT(*) NUM_OF_PROFILES_EDITED_YESTERDAY FROM EMPINST.EMPLOYEE WHERE DATE(PROF_LAST_UPDATE) = CURRENT_DATE - 1 DAY;" & _
            "SELECT COUNT(*) NUM_OF_PROFILES_PROFILES FROM EMPINST.EMPLOYEE;" & _
            "SELECT COUNT(*) NUM_OF_PROFILES_CREATED_YESTERDAY FROM EMPINST.EMP_ROLE_MAP E1 INNER JOIN EMPINST.EMPLOYEE E2 ON E1.PROF_KEY = E2.PROF_KEY WHERE DATE(E1.CREATED) = CURRENT_DATE - 1 DAY;" & _
            "SELECT PROF_GUID, PROF_DISPLAY_NAME FROM EMPINST.EMPLOYEE;"



        Dim Category As String = "Profiles"

        Try
            Dim con As IBM.Data.DB2.DB2Connection
            Dim ds As New DataSet
            Try

                con = New IBM.Data.DB2.DB2Connection("Database=PEOPLEDB;UserID=" & myServer.DBUserName & ";Password=" & myServer.DBPassword & ";Server=" & myServer.DBHostName & ":" & myServer.DBPort & "")
                con.Open()

                Dim cmd As New IBM.Data.DB2.DB2Command(sql, con)
                Dim adapter As New IBM.Data.DB2.DB2DataAdapter(cmd)
                adapter.Fill(ds)



            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error getting Profile Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            Finally
                Try
                    If con.IsOpen Then
                        con.Close()
                    End If
                Catch ex As Exception

                End Try

            End Try


            Dim dict As New Dictionary(Of String, String)
            Try
                Dim adapter As New VSAdaptor()

                dict.Add("NUM_OF_PROFILES_WITH_NO_PICTURE", ds.Tables(0).Rows(0)("NUM_OF_PROFILES_WITH_NO_PICTURE"))

                dict.Add("NUM_OF_PROFILES_NOT_RECENTLY_UPDATED", ds.Tables(1).Rows(0)("NUM_OF_PROFILES_NOT_RECENTLY_UPDATED"))

                dict.Add("NUM_OF_PROFILES_WITH_NO_PRONUNCIATION", ds.Tables(2).Rows(0)("NUM_OF_PROFILES_WITH_NO_PRONUNCIATION"))

                dict.Add("NUM_OF_PROFILES_MANAGERS", ds.Tables(3).Rows(0)("NUM_OF_PROFILES_MANAGERS"))

                dict.Add("NUM_OF_PROFILES_WITH_NO_MANAGER", ds.Tables(4).Rows(0)("NUM_OF_PROFILES_WITH_NO_MANAGER"))

                dict.Add("NUM_OF_PROFILES_WITH_NO_JOB_HIERARCHY", ds.Tables(5).Rows(0)("NUM_OF_PROFILES_WITH_NO_JOB_HIERARCHY"))

                dict.Add("NUM_OF_PROFILES_WITH_JOB_HIERARCHY", ds.Tables(6).Rows(0)("NUM_OF_PROFILES_WITH_JOB_HIERARCHY"))

                dict.Add("NUM_OF_PROFILES_WITH_NO_JOB_TITLE", ds.Tables(7).Rows(0)("NUM_OF_PROFILES_WITH_NO_JOB_TITLE"))

                dict.Add("PROFILES_AVERAGE_DAYS_SINCE_EDIT", ds.Tables(8).Rows(0)("PROFILES_AVERAGE_DAYS_SINCE_EDIT"))

                dict.Add("NUM_OF_PROFILES_EDITED_YESTERDAY", ds.Tables(9).Rows(0)("NUM_OF_PROFILES_EDITED_YESTERDAY"))

                dict.Add("NUM_OF_PROFILES_PROFILES", ds.Tables(10).Rows(0)("NUM_OF_PROFILES_PROFILES"))

                dict.Add("NUM_OF_PROFILES_CREATED_YESTERDAY", ds.Tables(11).Rows(0)("NUM_OF_PROFILES_CREATED_YESTERDAY"))

                sql = "INSERT INTO IbmConnectionsSummaryStats (ServerName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber) VALUES "

                Dim sqlCols As String = ""
                Dim sqlVals As String = ""
                For Each key In dict.Keys
                    Dim Name As String = key
                    Dim Val As String = dict(key)

                    If String.Equals(myServer.IPAddress, "https://connections-as.jnittech.com:9444", StringComparison.CurrentCultureIgnoreCase) Then
                        If key = "NUM_OF_POFILES_EDITED_YESTERDAY" Or key = "NUM_OF_PROFILES_CREATED_YESTERDAY" Then
                            Val = Int(20 * Rnd()) + 1
                        End If
                    End If

                    sql += "('" & myServer.Name & "', GetDate(), '" & Name.ToUpper() & "', '" & Val & "', '" & GetWeekNumber(Now) & "', '" & Now.Month.ToString() & "', '" & Now.Year.ToString() & "', '" & Now.Day.ToString() & "'),"

                Next


                adapter.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", sql.Substring(0, sql.Length - 1))




                'Other Tables
                'PROF_UID, PROF_DISPLAY_NAME
                sql = ""

                Using sqlConn As SqlClient.SqlConnection = adapter.StartConnectionSQL("VitalSigns")
                    For Each row As DataRow In ds.Tables(12).Rows()
                        sql += "IF NOT EXISTS ( SELECT 1 FROM IbmConnectionsUsers WHERE GUID = @ProfGuid AND ServerId = @ServerId) BEGIN " & _
                            "INSERT IbmConnectionsUsers (GUID, DisplayName, ServerID) VALUES (@ProfGuid, @ProfDisplayName, @ServerId) END;"

                        Dim sqlCmd As New SqlClient.SqlCommand()
                        sqlCmd.Connection = sqlConn
                        sqlCmd.CommandText = sql
                        sqlCmd.Parameters.AddWithValue("@ProfDisplayName", row("PROF_DISPLAY_NAME").ToString())
                        sqlCmd.Parameters.AddWithValue("@ProfGuid", row("PROF_GUID").ToString())
                        sqlCmd.Parameters.AddWithValue("@ServerId", myServer.ID)

                        sqlCmd.ExecuteNonQuery()


                    Next
                End Using

            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error parsing Profile Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            End Try



        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error in GetProfileStats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try

    End Sub

    Public Sub GetHomepageStats(ByRef myServer As MonitoredItems.IBMConnect)

        Dim sqlDs As New DataSet()
        Dim sql As String = ""

        Try
            Dim objVSAdaptor As New VSFramework.VSAdaptor()
            objVSAdaptor.FillDatasetAny("VitalSigns", "table", "SELECT GUID, Type FROM IbmConnectionsObjects", sqlDs, "table")
        Catch ex As Exception

        End Try

        Try
            Dim linq = From row In sqlDs.Tables("table").Rows.Cast(Of DataRow)()
                       Where row.Field(Of String)("Type") = "Community"
                       Select row.Field(Of String)("GUID")

            sql += "SELECT READER_ID, SOURCE, CONTAINER_ID, ITEM_ID FROM HOMEPAGE.NR_COMMUNITIES_VIEW WHERE READER_ID IN ( '" & String.Join("','", linq) & "') AND READER_ID != CONTAINER_ID;"

            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Homepage Stats DB@ sql : " & sql, LogUtilities.LogUtils.LogLevel.Normal)

        Catch ex As Exception

        End Try






        Dim Category As String = "Homepage"

        Try
            Dim con As IBM.Data.DB2.DB2Connection
            Dim ds As New DataSet
            Try

                con = New IBM.Data.DB2.DB2Connection("Database=HOMEPAGE;UserID=" & myServer.DBUserName & ";Password=" & myServer.DBPassword & ";Server=" & myServer.DBHostName & ":" & myServer.DBPort & "")
                con.Open()

                Dim cmd As New IBM.Data.DB2.DB2Command(sql, con)
                Dim adapter As New IBM.Data.DB2.DB2DataAdapter(cmd)
                adapter.Fill(ds)



            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error getting Homepage Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            Finally
                Try
                    If con.IsOpen Then
                        con.Close()
                    End If
                Catch ex As Exception

                End Try

            End Try


            Dim dict As New Dictionary(Of String, String)
            Try
                Dim adapter As New VSAdaptor()

                sql = ""

                Using sqlConn As SqlClient.SqlConnection = adapter.StartConnectionSQL("VitalSigns")
                    For Each row As DataRow In ds.Tables(0).Rows()
                        Dim type As String = ""
                        If row("SOURCE").ToString() = "dogear" Then
                            Continue For
                        ElseIf row("SOURCE").ToString() = "forums" Then
                            type = "Forum"
                        ElseIf row("SOURCE").ToString() = "activities" Then
                            type = "Activity"
                        ElseIf row("SOURCE").ToString() = "communities" Then
                            type = "Community"
                        ElseIf row("SOURCE").ToString() = "blogs" Then
                            'This ensure you get the blog and not a blog post. Blogs and Blog Posts will be connected under blogs function
                            If row("CONTAINER_ID").ToString() <> row("ITEM_ID").ToString() Then
                                Continue For
                            End If

                            type = "Blog"

                        ElseIf row("SOURCE").ToString() = "wikis" Then
                            'This ensure you get the blog and not a blog post. Blogs and Blog Posts will be connected under blogs function
                            If row("CONTAINER_ID").ToString() <> row("ITEM_ID").ToString() Then
                                Continue For
                            End If

                            type = "Wiki"
                        End If


                        sql += "UPDATE IbmConnectionsObjects SET ParentObjectID = ( SELECT TOP 1 ID FROM IbmConnectionsObjects WHERE GUID = @ParentGUID AND ServerID = @ServerId AND Type = 'Community') WHERE GUID = @ChildGuid AND ServerID = @ServerId AND Type = @Type;"
                        sql += "UPDATE IbmConnectionsObjects SET OwnerID = (SELECT OwnerID FROM IbmConnectionsObjects WHERE GUID = @ParentGUID AND ServerID = @ServerId AND Type = 'Community') WHERE GUID = @ChildGuid AND ServerID = @ServerID AND Type = @Type AND OwnerID IS NULL;"
                        Dim sqlCmd As New SqlClient.SqlCommand()
                        sqlCmd.Connection = sqlConn
                        sqlCmd.CommandText = sql
                        sqlCmd.Parameters.AddWithValue("@ParentGUID", row("READER_ID").ToString())
                        sqlCmd.Parameters.AddWithValue("@ChildGuid", row("CONTAINER_ID").ToString())
                        sqlCmd.Parameters.AddWithValue("@Type", type)
                        sqlCmd.Parameters.AddWithValue("@ServerId", myServer.ID)
                        sqlCmd.ExecuteNonQuery()


                    Next
                End Using

            Catch ex As Exception
                WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error parsing Homepage Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            End Try



        Catch ex As Exception
            WriteDeviceHistoryEntry(myServer.DeviceType, myServer.Name, Now.ToString & "Error in GetHomepageStats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try

    End Sub

    Public Function HexToGUID(ByRef hex As String) As String
        Return hex.Insert(20, "-").Insert(16, "-").Insert(12, "-").Insert(8, "-")
    End Function

    Public Function BinaryToText(ByRef data As Byte()) As String
        Return Encoding.UTF8.GetString(data)
    End Function

    Public Function BinaryToText2(ByRef data As Byte()) As String
        Dim decoder As Decoder = Encoding.GetEncoding(37).GetDecoder()
        Dim c(decoder.GetCharCount(data, 0, data.Length)) As Char
        decoder.GetChars(data, 0, data.Length, c, 0)
        Return New String(c)
    End Function


#End Region


#Region "Sametime Monitoring"

    Private Sub MonitorSametime() 'This is the main sub that calls all the other modules
        'each call to Monitor Sametime will monitor 1 ST  server, 1 time
        ' WriteAuditEntry(Now.ToString & " >>> Begin Loop for ST Monitoring >>>>")
        'dtSametimeLastUpdate = Now

        Do While boolTimeToStop <> True
            Dim exitLoop As Boolean = False
            Try
                Dim myServer As MonitoredItems.SametimeServer
                myServer = SelectSametimeServerToMonitor()

                If myServer Is Nothing Then
                    '    WriteAuditEntry(Now.ToString & " >>> No ST servers are due for monitoring now.  >>>>")
                    CurrentSametime = ""
                    'Exit Sub
                    exitLoop = True
                Else
                    '   WriteAuditEntry(Now.ToString & " >>> Selected " & myServer.Name)
                End If

                If myServer.Enabled = False And exitLoop = False Then
                    ' myServer.IncrementUpCount()
                    myServer.Status = "Disabled"
                    myServer.LastScan = Date.Now
                    myServer.ResponseDetails = "This ST server is not enabled for monitoring."
                    UpdateSametimeStatusTable(myServer)
                    'Exit Sub
                    exitLoop = True
                End If
                If exitLoop = False Then

                    If InMaintenance("Sametime", myServer.Name) = True Then
                        myServer.Status = "Maintenance"
                        ' myServer.IncrementUpCount()
                        myServer.ResponseDetails = "This ST server is in a scheduled maintenance period.  Monitoring is temporarily disabled."
                        UpdateSametimeStatusTable(myServer)
                    Else
                        '  myServer.Status = "Scanning"
                        '  myServer.ResponseDetails = "Information will be available when the server is scanned."
                        '   myServer.Description = "New scan cycle started at " & Date.Now.ToShortTimeString
                        '   UpdateSametimeStatusTable(myServer)
                        myServer.AlertCondition = False
                        '  myServer.LastScan = Now
                        xmlSametime = ""
                        CurrentSametime = myServer.Name
                        myServer.LastScan = Date.Now

                        Try
                            WriteDeviceHistoryEntry("Sametime", myServer.Name, Now.ToString & " Querying the server as an end user...")
                            TestSametimeServerAsUser(myServer)
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Sametime", myServer.Name, Now.ToString & " Exception querying as end user: " & ex.ToString)
                        End Try

                        Try

                            GetSametimeXMLStats(myServer)
                            GetSametimeConfStats(myServer)
                            Thread.Sleep(1000)
                            UpdateAdvancedSametimeStatTable(myServer)
                            Thread.Sleep(1000)
                            '   CheckSametimeRunningServices(myServer)
                            '1/11/2016 NS added for VSPLUS-1921,VSPLUS-1823
                            UpdateSametimeSummaryTable(myServer)
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("Sametime", myServer.Name, " Error querying server: " & ex.ToString)
                        End Try


                        UpdateSametimeStatusTable(myServer)
                    End If
                End If
            Catch ThreadEx As ThreadAbortException
                WriteAuditEntry(Now.ToString & " Aborting Sametime monitoring sub " & ThreadEx.Message)
                Exit Sub
            Catch ex As Exception

            End Try

            If mySametimeServers.Count < 24 Then
                Thread.Sleep(2500)
            ElseIf mySametimeServers.Count < 10 Then
                Thread.CurrentThread.Sleep(10000)
            End If

            dtSametimeLastUpdate = Now

        Loop

    End Sub

    Private Function SelectSametimeServerToMonitor() As MonitoredItems.SametimeServer
        'WriteAuditEntry(Now.ToString & " >>> Selecting a Domino Server for monitoring >>>>")
        Dim tNow As DateTime
        tNow = Now
        Dim tScheduled As DateTime

        Dim timeOne, timeTwo As DateTime

        Dim SelectedServer As MonitoredItems.SametimeServer

        Dim ServerOne As MonitoredItems.SametimeServer
        Dim ServerTwo As MonitoredItems.SametimeServer

        Dim myRegistry As New RegistryHandler

        Dim n As Integer
        Dim strSQL As String = ""
        Dim ServerType As String = "Sametime"
        Dim serverName As String = ""

        Try
            strSQL = "Select svalue from ScanSettings where sname = 'Scan" & ServerType & "ASAP'"
            Dim ds As New DataSet()
            ds.Tables.Add("ScanASAP")
            Dim objVSAdaptor As New VSAdaptor
            objVSAdaptor.FillDatasetAny("VitalSigns", "VitalSigns", strSQL, ds, "ScanASAP")

            For Each row As DataRow In ds.Tables("ScanASAP").Rows
                Try
                    serverName = row(0).ToString()
                Catch ex As Exception
                    Continue For
                End Try

                For n = 0 To mySametimeServers.Count - 1
                    ServerOne = mySametimeServers.Item(n)

                    If ServerOne.Name = serverName And ServerOne.IsBeingScanned = False And ServerOne.Enabled Then
                        'WriteAuditEntry(Now.ToString & " >>> " & serverName & " was marked 'Scan ASAP' so that will be scanned next.")
                        strSQL = "DELETE FROM ScanSettings where sname = 'Scan" & ServerType & "ASAP' and svalue='" & serverName & "'"
                        objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)

                        Return ServerOne
                        Exit Function

                    End If
                Next
            Next

        Catch ex As Exception

        End Try

        'Any server Not Scanned should be scanned right away.  Select the first one you encounter
        For n = 0 To mySametimeServers.Count - 1
            ServerOne = mySametimeServers.Item(n)

            If ServerOne.Status = "Not Scanned" Or ServerOne.Status = "Master Service Stopped." Then
                '        WriteAuditEntry(Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status)
                Return ServerOne
                Exit Function
            End If
        Next

        'start with the first two servers
        ServerOne = mySametimeServers.Item(0)
        If mySametimeServers.Count > 1 Then ServerTwo = mySametimeServers.Item(1)

        'go through the remaining servers, see which one has the oldest (earliest) scheduled time
        If mySametimeServers.Count > 2 Then
            Try
                For n = 2 To mySametimeServers.Count - 1
                    '           WriteAuditEntry(Now.ToString & " N is " & n)
                    timeOne = CDate(ServerOne.NextScan)
                    timeTwo = CDate(ServerTwo.NextScan)
                    If DateTime.Compare(timeOne, timeTwo) < 0 Then
                        'time one is earlier than time two, so keep server 1
                        ServerTwo = mySametimeServers.Item(n)
                    Else
                        'time two is later than time one, so keep server 2
                        ServerOne = mySametimeServers.Item(n)
                    End If
                Next
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " >>> Error Selecting Sametime server... " & ex.Message)
            End Try
        Else
            'There were only two server, so use those going forward
        End If

        'WriteAuditEntry(Now.ToString & " >>> Down to two servers... " & ServerOne.Name & " and " & ServerTwo.Name)

        'Of the two remaining servers, pick the one with earliest scheduled time for next scan
        If Not (ServerTwo Is Nothing) Then
            timeOne = CDate(ServerOne.NextScan)
            timeTwo = CDate(ServerTwo.NextScan)

            If DateTime.Compare(timeOne, timeTwo) < 0 Then
                'time one is earlier than time two, so keep server 1
                SelectedServer = ServerOne
                tScheduled = CDate(ServerOne.NextScan)
            Else
                SelectedServer = ServerTwo
                tScheduled = CDate(ServerTwo.NextScan)
            End If
            tNow = Now
            '   WriteAuditEntry(Now.ToString & " >>> Down to one server... " & SelectedServer.Name & " to scan at " & SelectedServer.NextScan & ". Status is " & SelectedServer.Status)
        Else
            SelectedServer = ServerOne
            tScheduled = CDate(ServerOne.NextScan)
        End If

        tScheduled = CDate(SelectedServer.NextScan)
        If DateTime.Compare(tNow, tScheduled) < 0 Then
            If SelectedServer.Status <> "Not Scanned" Then
                '  WriteAuditEntry(Now.ToString & " No Domino servers scheduled for monitoring, next scan after " & SelectedServer.NextScan)
                SelectedServer = Nothing
            Else
                WriteAuditEntry(Now.ToString & " selected Sametime server: " & SelectedServer.Name & " because it has not been scanned yet.")
            End If
        Else
            WriteAuditEntry(Now.ToString & " selected Sametime server: " & SelectedServer.Name)
        End If

        'Release Memory
        tNow = Nothing
        tScheduled = Nothing
        n = Nothing

        timeOne = Nothing
        timeTwo = Nothing

        ServerOne = Nothing
        ServerTwo = Nothing

        'return selectedserver
        SelectSametimeServerToMonitor = SelectedServer
        'Exit Function
        SelectedServer = Nothing
    End Function


    Private Sub QuerySametimeServer(ByRef Server As MonitoredItems.SametimeServer)
        Dim Result As String
        Server.Status = "OK"
        Try
            Result = PingSametimeServer(Server.IPAddress, "SQ")
            ParseData(Result, Server)
            '   WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " >>> Sametime server results for SQ: " & vbCrLf & Result & vbCrLf & "Users: " & Server.Users)
            Server.IncrementUpCount()
        Catch ex As Exception
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " >>> Error parsing SQ " & ex.Message)
            Server.IncrementDownCount()
        End Try

        Try
            Result = PingSametimeServer(Server.IPAddress, "IM")
            ParseData(Result, Server)
            '   WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " >>> Sametime server results for IM: " & vbCrLf & Result & vbCrLf & "Chats: " & Server.Chat_Sessions)

        Catch ex As Exception
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " >>> Error parsing IM " & ex.Message)
        End Try

        Try
            Result = PingSametimeServer(Server.IPAddress, "NW")
            ParseData(Result, Server)
            '      WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " >>> Sametime server results for NW: " & vbCrLf & Result & vbCrLf & "NWay: " & Server.nWay_Chat_Sessions)

        Catch ex As Exception
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " >>> Error parsing NW " & ex.Message)
        End Try


        Try
            Result = PingSametimeServer(Server.IPAddress, "PL")
            ParseData(Result, Server)
            ' WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " >>> Sametime server results for PL: " & vbCrLf & Result & vbCrLf & "Places: " & Server.Places)

        Catch ex As Exception
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " >>> Error parsing PL " & ex.Message)
        End Try

        Try
            Server.Description = "The Sametime server currently has " & Server.Users & " active users. "
        Catch ex As Exception

        End Try

        'AF 1/20/2015 -  We are no longer doing alerts on these thresholds.

        'If Server.Users > Server.Users_Threshold And Server.Users_Threshold <> 0 Then
        '    Server.Status = "User Sessions"
        '    Server.ResponseDetails = "The Sametime server currently has " & Server.Users & " users.  The alert threshold is " & Server.Users_Threshold & " users."
        '    myAlert.QueueAlert("Sametime", Server.Name, "User", "The Sametime server currently has " & Server.Users & " users.  The alert threshold is " & Server.Users_Threshold & " users.", Server.Location)
        'Else
        '    myAlert.ResetAlert("Sametime", Server.Name, "User", Server.Location)
        'End If

        'If Server.Chat_Sessions > Server.Chat_Sessions_Threshold And Server.Chat_Sessions_Threshold <> 0 Then
        '    Server.Status = "Chat Sessions"
        '    Server.ResponseDetails = "The Sametime server currently has " & Server.Chat_Sessions & " active chat sessions.  The alert threshold is " & Server.Chat_Sessions_Threshold & " chat sessions."
        '    myAlert.QueueAlert("Sametime", Server.Name, "Chat", "The Sametime server currently has " & Server.Chat_Sessions & " active chat sessions.  The alert threshold is " & Server.Chat_Sessions_Threshold & " chat sessions.", Server.Location)
        'Else
        '    myAlert.ResetAlert("Sametime", Server.Name, "Chat", Server.Location)
        'End If

        'If Server.nWay_Chat_Sessions > Server.nWay_Chat_Sessions_Threshold And Server.nWay_Chat_Sessions_Threshold <> 0 Then
        '    Server.Status = "n-Way Chat"
        '    Server.ResponseDetails = "The Sametime server currently has " & Server.nWay_Chat_Sessions & " active n-Way chat sessions.  The alert threshold is " & Server.nWay_Chat_Sessions_Threshold & " chat sessions."
        '    myAlert.QueueAlert("Sametime", Server.Name, "n-Way Chat", "The Sametime server currently has " & Server.nWay_Chat_Sessions & " active n-Way chat sessions.  The alert threshold is " & Server.nWay_Chat_Sessions_Threshold & " chat sessions.", Server.Location)
        'Else
        '    myAlert.ResetAlert("Sametime", Server.Name, "NChat", Server.Location)
        'End If


        'If Server.Places > Server.Places_Threshold And Server.Places_Threshold <> 0 Then
        '    Server.Status = "Places"
        '    Server.ResponseDetails = "The Sametime server currently has " & Server.Places & " active Places.  The alert threshold is " & Server.Places_Threshold & " Places."
        '    myAlert.QueueAlert("Sametime", Server.Name, "Places", "The Sametime server currently has " & Server.Places & " active Places.  The alert threshold is " & Server.Places_Threshold & " Places.", Server.Location)
        'Else
        '    myAlert.ResetAlert("Sametime", Server.Name, "Places", Server.Location)
        'End If

        'Update Statistics
        Try
            UpdateSametimeStatTable(Server.Name, "Users", Server.Users)
        Catch ex As Exception

        End Try

        Try
            UpdateSametimeStatTable(Server.Name, "Chat Sessions", Server.Chat_Sessions)
        Catch ex As Exception

        End Try

        Try
            UpdateSametimeStatTable(Server.Name, "n-Way Chat Sessions", Server.nWay_Chat_Sessions)
        Catch ex As Exception

        End Try

        Try
            UpdateSametimeStatTable(Server.Name, "Places", Server.Places)
        Catch ex As Exception

        End Try


        Try
            If Server.StatisticsCollection.Count > 0 Then
                For Each stat As MonitoredItems.SametimeStatistic In Server.StatisticsCollection
                    Try
                        '5/5/2016 NS modified - if stat.Name is empty, it causes a SQL error
                        If CType(stat.Value, Double) And stat.Name <> "" Then
                            UpdateSametimeStatTable(Server.Name, stat.Name, stat.Value)
                        End If
                    Catch ex As Exception

                    End Try

                Next
            End If
        Catch ex As Exception

        End Try


    End Sub

    Public Function PingSametimeServer(ByVal ServerHostName As String, ByVal Code As String) As String
        Dim Result As String
        Dim MyHTTP As New nsoftware.IPWorks.Http("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000")
        Dim PostCode As String
        PostCode = "op_code=" & Code
        With MyHTTP
            .ContentType = "application/x-www-form-urlencoded"
            .PostData = PostCode
            Try
                .Post(ServerHostName & "/cgi-bin/stadminAct.exe")
                Result = .TransferredData
            Catch ex As Exception
                Result = ""
            End Try
        End With

        Dim myReturn As String = Chr(13) & Chr(10)
        Dim newResult As String
        WriteAuditEntry("Result from PingSametimeServer:  " & Result, LogLevel.Normal)
        newResult = Result.Replace(myReturn, "~")
        MyHTTP.Dispose()
        Return newResult
    End Function

    Public Sub ParseData(ByVal ServerResult As String, ByRef Server As MonitoredItems.SametimeServer)

        If ServerResult = "" Then Exit Sub
        Dim Status As String
        Dim Start, TheEnd As Integer
        Dim Offset As Integer
        If InStr(ServerResult, "Status: ") > 0 Then
            Offset = "Status: ".Length
            Start = InStr(ServerResult, "Status: ")
            TheEnd = InStr(Start, ServerResult, "~")
            Status = Mid(ServerResult, Start + Offset, TheEnd - (Start + Offset))
            Server.Status = Trim(Status)
            If Server.Status = "ok" Then Server.Status = "OK"
        End If

        If InStr(ServerResult, "UsersNum=") > 0 Then
            Offset = "UsersNum=".Length
            Start = InStr(ServerResult, "UsersNum=")
            TheEnd = InStr(Start, ServerResult, "~")
            Status = Mid(ServerResult, Start + Offset, TheEnd - (Start + Offset))
            Server.Users = Trim(Status)
        End If

        If InStr(ServerResult, "usersNum=") > 0 Then
            Offset = "usersNum=".Length
            Start = InStr(ServerResult, "usersNum=")
            TheEnd = InStr(Start, ServerResult, "~")
            Status = Mid(ServerResult, Start + Offset, TheEnd - (Start + Offset))
            Server.Users = Trim(Status)
        End If

        'If InStr(ServerResult, "UserLoginsNum=") > 0 Then
        '    Offset = "UserLoginsNum=".Length
        '    Start = InStr(ServerResult, "UserLoginsNum=")
        '    TheEnd = InStr(Start, ServerResult, "~")
        '    Status = Mid(ServerResult, Start + Offset, TheEnd - (Start + Offset))
        '    Me.txtUserLogins.Text = Trim(Status)
        'End If

        'IMs=0~Status: ok~
        If InStr(ServerResult, "IMs=") > 0 Then
            Offset = "IMs=".Length
            Start = InStr(ServerResult, "IMs=")
            TheEnd = InStr(Start, ServerResult, "~")
            Status = Mid(ServerResult, Start + Offset, TheEnd - (Start + Offset))
            Server.Chat_Sessions = Trim(Status)
        End If

        'NWs=0~Status: ok~
        If InStr(ServerResult, "NWs=") > 0 Then
            Offset = "NWs=".Length
            Start = InStr(ServerResult, "NWs=")
            TheEnd = InStr(Start, ServerResult, "~")
            Status = Mid(ServerResult, Start + Offset, TheEnd - (Start + Offset))
            Server.nWay_Chat_Sessions = Trim(Status)
        End If

        If InStr(ServerResult, "Places=") > 0 Then
            Offset = "Places=".Length
            Start = InStr(ServerResult, "Places=")
            TheEnd = InStr(Start, ServerResult, "~")
            Status = Mid(ServerResult, Start + Offset, TheEnd - (Start + Offset))
            Server.Places = Trim(Status)
        End If
    End Sub

    Public Sub GetSametimeXMLStats(ByRef Server As MonitoredItems.SametimeServer)
        If Server.CollectExtendedChat = True Then

            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Sending request for XML statistics to " & Server.IPAddress & "/servlet/statistics")
            'URL syntax is http://azphxweb2.rprwyatt.com/servlet/statistics
            Dim myURL As String = ""
            If Server.SSL = True Then
                myURL = "https://" & Server.IPAddress & ":" + Server.ExtendedChatPort + "/servlet/statistics"
            Else
                myURL = "http://" & Server.IPAddress & ":" + Server.ExtendedChatPort + "/servlet/statistics"
            End If

            ''HSBC code

            'If Server.SSL = True And InStr(Server.Name, "HBUS/HSBC" > 0) Then
            '    myURL = "https://" & Server.IPAddress & ":8088/servlet/statistics"

            'End If

            If InStr(Server.Name, "HBUS/HSBC") Then
                myURL = "http://" & Server.IPAddress & ":" + Server.ExtendedChatPort + "/servlet/statistics"
            End If

            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Querying Sametime using  " & myURL)
            Dim HTML As String = ""

            With ChilkatSametimeHTTP
                .Login = Server.UserId1
                .Password = Server.Password1
                HTML = .QuickGetStr(myURL)
            End With

            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Username  is " & Server.UserId1)
            ' WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Password is " & mySametimePassword1)
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Server response is: " & vbCrLf & HTML, LogLevel.Verbose)

            ParseSametimeXMLStatistics(HTML)
        End If

        ' SametimeHTTP.Get("http://" & Server.IPAddress & "/servlet/statistics")
    End Sub

    Private Sub ParseSametimeXMLStatistics(xmlString As String)
        If xmlString = "" Then Exit Sub

        Dim output As StringBuilder = New StringBuilder()
        Dim StatName As String

        Dim m_xmld As XmlDocument
        Dim m_nodelist As XmlNodeList
        Dim m_node As XmlNode
        Dim child_node As XmlNode
        Dim mySametimeServer As MonitoredItems.SametimeServer
        Try
            mySametimeServer = mySametimeServers.SearchByName(CurrentSametime)
        Catch ex As Exception
            Exit Sub
        End Try



        If mySametimeServer Is Nothing Then Exit Sub
        WriteDeviceHistoryEntry("Sametime", mySametimeServer.Name, Now.ToString & "  Parsing Sametime XML statistics....")
        ' WriteDeviceHistoryEntry("Sametime", mySametimeServer.Name, vbCrLf & xmlSametime)
        mySametimeServer.StatisticsCollection.Clear()


        Try
            'Create the XML Document

            m_xmld = New XmlDocument()
            'Load the Xml file
            m_xmld.Load(New StringReader(xmlString))
            'Get the list of name nodes 

            m_nodelist = m_xmld.SelectNodes("/SametimeStatistics/Platform")
            'Loop through the nodes

            For Each m_node In m_nodelist
                Try
                    If m_node.HasChildNodes Then
                        For Each child_node In m_node
                            Dim myStat As New MonitoredItems.SametimeStatistic
                            myStat.Name = Trim(child_node.Name)
                            myStat.Value = child_node.InnerText
                            ' TextBoxX1.Text += child_node.Name & ": " & child_node.InnerText & vbCrLf
                            mySametimeServer.StatisticsCollection.Add(myStat)
                            WriteDeviceHistoryEntry("Sametime", mySametimeServer.Name, Now.ToString & "  Parsing Sametime XML statistics...." & child_node.Name & ": " & child_node.InnerText & vbCrLf)
                        Next
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Sametime", mySametimeServer.Name, Now.ToString & "  Error Parsing Sametime XML statistics at #1: " & ex.ToString)
                End Try
            Next

            m_nodelist = m_xmld.SelectNodes("/SametimeStatistics/Statistic")
            'Loop through the nodes

            For Each m_node In m_nodelist
                'Get the Gender Attribute Value
                Try
                    Dim myStat As New MonitoredItems.SametimeStatistic
                    myStat.Name = m_node.Attributes.GetNamedItem("name").Value
                    myStat.Value = m_node.ChildNodes.Item(0).InnerText
                    mySametimeServer.StatisticsCollection.Add(myStat)
                    WriteDeviceHistoryEntry("Sametime", mySametimeServer.Name, Now.ToString & "  Parsing Sametime XML statistics...." & myStat.Name & ": " & myStat.Value)

                    Select Case myStat.Name
                        Case "UsersNum"
                            mySametimeServer.Users = myStat.Value
                            WriteDeviceHistoryEntry("Sametime", mySametimeServer.Name, Now.ToString & "  Parsing Sametime XML statistics....My user count is: " & myStat.Value)
                        Case "PlacesNum"
                            mySametimeServer.Places = myStat.Value
                        Case "IMs"
                            mySametimeServer.Chat_Sessions = myStat.Value
                        Case "NWs"
                            mySametimeServer.nWay_Chat_Sessions = myStat.Value
                    End Select
                Catch ex As Exception
                    StatName = "nothing"
                    WriteDeviceHistoryEntry("Sametime", mySametimeServer.Name, Now.ToString & "  Error Parsing Sametime XML statistics at #2: " & ex.ToString)
                End Try
            Next
        Catch errorVariable As Exception
            'Error trapping
            '       Console.Write(errorVariable.ToString())
        End Try

        WriteDeviceHistoryEntry("Sametime", mySametimeServer.Name, Now.ToString & "  Parsing Sametime enumerated processes....")
        'EnumeratedProcesses
        m_nodelist = m_xmld.SelectNodes("/SametimeStatistics/EnumeratedProcesses")
        'Loop through the nodes
        Try
            mySametimeServer.RunningProcesses.Clear()
        Catch ex As Exception

        End Try

        For Each m_node In m_nodelist
            Try
                If m_node.HasChildNodes Then
                    For Each child_node In m_node
                        Try
                            WriteDeviceHistoryEntry("Sametime", mySametimeServer.Name, Now.ToString & " Process: " & child_node.InnerText)
                            Dim myProcess As New MonitoredItems.SametimeRunningProcess
                            myProcess.Name = child_node.InnerText
                            mySametimeServer.RunningProcesses.Add(myProcess)
                        Catch ex As Exception

                        End Try

                    Next
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("Sametime", mySametimeServer.Name, Now.ToString & "  Error Parsing Sametime XML statistics at #3: " & ex.ToString)
            End Try
        Next
        'Clear it out for next time so we don't continually parse the same information
        xmlSametime = ""
        WriteDeviceHistoryEntry("Sametime", mySametimeServer.Name, Now.ToString & "  Finished parsing Sametime information...")
    End Sub


    Private Sub CheckSametimeRunningServices(ByRef Server As MonitoredItems.SametimeServer)
        WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Checking the health of Sametime subsystems.")

        Dim RunningService As MonitoredItems.SametimeRunningProcess
        For Each Service As MonitoredItems.SametimeMonitoredProcess In Server.MonitoredProcesses
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Checking the status of " & Service.Name)

            Try
                RunningService = Nothing
                RunningService = Server.RunningProcesses.Search(Service.Name & ".exe".ToUpper)
                WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Found " & RunningService.Name)
            Catch ex As Exception

            End Try

            Try
                If RunningService Is Nothing Then
                    'The service is not running on the ST server
                    Server.Status = "Missing Service"
                    WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " " & Service.Name & " not found.")
                    Server.Description = "The service " & Service.Name & " is not running."
                    Server.ResponseDetails = "The service " & Service.Name & " is not running."
                    myAlert.QueueAlert("Sametime", Server.Name, Service.Name & " Service", "The service " & Service.Name & " is not running.", Server.Location)
                Else
                    'the service is running
                    myAlert.ResetAlert("Sametime", Server.Name, Service.Name & " Service", Server.Location)
                    WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Service " & Service.Name & " is found.")
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Exception: " & ex.ToString)
            End Try

        Next
    End Sub

    Public Sub GetSametimeMeetingStats(ByRef Server As MonitoredItems.SametimeServer)
        If Server.Platform = "websphere" And Server.WsScanMeetingServer = True Then

            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Sending request for JSON statistics to " & Server.WsMeetingHost)
            'URL syntax is http://azphxweb2.rprwyatt.com/servlet/statistics
            Dim myURL As String = ""
            'http://sametime.jnittech.com:9083/ConferenceFocus/monitoring/MonitoringRestServlet?method=ConferenceIdList

            If Server.WsMeetingRequireSSL = True Then
                myURL = "https://" & Server.WsMeetingHost & ":" & Server.WsMeetingPort & "/ConferenceFocus/monitoring/MonitoringRestServlet?method=ConferenceIdList"
            Else
                myURL = "http://" & Server.WsMeetingHost & ":" & Server.WsMeetingPort & "/ConferenceFocus/monitoring/MonitoringRestServlet?method=ConferenceIdList"
            End If


            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Querying Sametime using  " & myURL)
            Dim HTML As String = ""

            With ChilkatSametimeHTTP
                '.Login = SametimeUserOne
                '.Password = mySametimePassword1
                HTML = .QuickGetStr(myURL)
            End With
            Dim myDevices As New ConferenceIdList
            If HTML <> "" Then
                myDevices = returnConfIdObject(Server, HTML)
                WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  conf id is: " & myDevices.ConferenceIdList(0).ToString, LogLevel.Normal)
            End If
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Username  is " & Server.UserId1)
            ' WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Password is " & mySametimePassword1)
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Server response is: " & vbCrLf & HTML, LogLevel.Normal)
        End If

        'ParseSametimeXMLStatistics(HTML)

        ' SametimeHTTP.Get("http://" & Server.IPAddress & "/servlet/statistics")
    End Sub

    Public Sub GetSametimeMeetingStatsFromDB(ByRef Server As MonitoredItems.SametimeServer)
        Dim sql As String = "SELECT COUNT(*) NUM_OF_TOTAL_MEETING_ROOMS FROM MTG.ROOM;" & _
            "SELECT COUNT(DISTINCT ROOM_ID) NUM_OF_ROOMS_ACTIVE_YESTERDAY FROM MTG.ROOM_USAGE WHERE DATE(ROOM_ACTIVE) = CURRENT_DATE - 1 DAY;" & _
            "SELECT COUNT(DISTINCT USER_ID) NUM_OF_USERS_ACTIVE_YESTERDAY FROM MTG.USER_USAGE WHERE IS_ANONYMOUS = 0 AND DATE(JOIN_TIME) = CURRENT_DATE - 1 DAY;" & _
            "SELECT COUNT(*) NUM_OF_MEETINGS_YESTERDAY FROM MTG.ROOM_USAGE WHERE DATE(ROOM_ACTIVE) = CURRENT_DATE - 1 DAY;" & _
            "SELECT COUNT(DISTINCT USER_ID) NUM_OF_USERS_ACTIVE_WITHIN_A_WEEK FROM MTG.USER_USAGE WHERE IS_ANONYMOUS = 0 AND DATE(JOIN_TIME) >= CURRENT_DATE - 7 DAYS;" & _
            "SELECT COUNT(DISTINCT ROOM_ID) NUM_OF_ROOMS_ACTIVE_WITHIN_A_WEEK FROM MTG.ROOM_USAGE WHERE DATE(ROOM_ACTIVE) >= CURRENT_DATE - 7 DAYS;"


        Try
            Dim con As IBM.Data.DB2.DB2Connection
            Dim ds As New DataSet
            Try

                con = New IBM.Data.DB2.DB2Connection("Database=STMS;UserID=" & Server.DBUserName & ";Password=" & Server.DBPassword & ";Server=" & Server.WsMeetingHost & ":" & Server.WsMeetingPort & "")
                con.Open()

                Dim cmd As New IBM.Data.DB2.DB2Command(sql, con)
                Dim adapter As New IBM.Data.DB2.DB2DataAdapter(cmd)
                adapter.Fill(ds)



            Catch ex As Exception
                WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "Error getting Meeting Server Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            Finally
                Try
                    If con.IsOpen Then
                        con.Close()
                    End If
                Catch ex As Exception

                End Try

            End Try


            Dim SummaryDict As New Dictionary(Of String, String)
            Dim DailyDict As New Dictionary(Of String, String)
            Try
                Dim adapter As New VSAdaptor()

                SummaryDict.Add("TotalNumOfMeetingRooms", ds.Tables(0).Rows(0)("NUM_OF_TOTAL_MEETING_ROOMS"))

                SummaryDict.Add("MeetingRoomsActiveYesterday", ds.Tables(1).Rows(0)("NUM_OF_ROOMS_ACTIVE_YESTERDAY"))

                SummaryDict.Add("MeetingUsersActiveYesterday", ds.Tables(2).Rows(0)("NUM_OF_USERS_ACTIVE_YESTERDAY"))

                SummaryDict.Add("MeetingsYesterday", ds.Tables(3).Rows(0)("NUM_OF_MEETINGS_YESTERDAY"))

                SummaryDict.Add("ActiveUserCount", ds.Tables(4).Rows(0)("NUM_OF_USERS_ACTIVE_WITHIN_A_WEEK"))

                SummaryDict.Add("ActiveMeetingRoomCount", ds.Tables(5).Rows(0)("NUM_OF_ROOMS_ACTIVE_WITHIN_A_WEEK"))


                sql = "INSERT INTO SametimeSummaryStats (ServerName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber) VALUES "

                Dim sqlCols As String = ""
                Dim sqlVals As String = ""
                For Each key In SummaryDict.Keys
                    Dim Name As String = key
                    Dim Val As String = SummaryDict(key)

                    If String.Equals(Server.IPAddress, "sametime.jnittech.com", StringComparison.CurrentCultureIgnoreCase) Then
                        If key = "MeetingRoomsActiveYesterday" Or key = "MeetingUsersActiveYesterday" Or key = "MeetingsYesterday" Then
                            Val = Int(20 * Rnd()) + 1
                        End If
                    End If

                    sql += "('" & Server.Name & "', GetDate(), '" & Name & "', '" & Val & "', '" & GetWeekNumber(Now) & "', '" & Now.Month.ToString() & "', '" & Now.Year.ToString() & "', '" & Now.Day.ToString() & "'),"

                Next


                adapter.ExecuteNonQueryAny("VSS_Statistics", "VSS_Statistics", sql.Substring(0, sql.Length - 1))



            Catch ex As Exception
                WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "Error parsing Files Stats. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
            End Try



        Catch ex As Exception
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "Error in GetSametimeMeetingStatsFromDB. Error : " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
        End Try
    End Sub


    Public Sub GetSametimeConfStats(ByRef Server As MonitoredItems.SametimeServer)
        WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Media Server: Platform" & Server.Platform)
        WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Media Server: WsScanMediaServer" & Server.WsScanMediaServer)
        If Server.Platform = "Websphere" And Server.WsScanMediaServer = True Then

            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Sending request for JSON statistics to " & Server.WsMeetingHost)
            'URL syntax is http://azphxweb2.rprwyatt.com/servlet/statistics
            Dim myURL As String = ""
            'http://sametime.jnittech.com:9083/ConferenceFocus/monitoring/MonitoringRestServlet?method=ConferenceIdList

            If Server.WsMediaRequireSSL = True Then
                myURL = "https://" & Server.WsMediaHost & ":" & Server.WsMediaPort & "/ConferenceFocus/monitoring/MonitoringRestServlet"
            Else
                myURL = "http://" & Server.WsMediaHost & ":" & Server.WsMediaPort & "/ConferenceFocus/monitoring/MonitoringRestServlet"
            End If


            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Querying Sametime using  " & myURL)
            Dim HTML As String = ""

            With ChilkatSametimeHTTP
                '.Login = SametimeUserOne
                '.Password = mySametimePassword1
                HTML = .QuickGetStr(myURL + "?method=ConferenceIdList")
            End With
            Dim myDevices As New ConferenceIdList
            Dim myConfData As ConferenceData
            Dim myAcrossConfData As New AcrossConferenceData
            If HTML <> "" Then
                myDevices = returnConfIdObject(Server, HTML)

                Dim iNoOfActiveConf As Integer = 0
                Dim iTotalUsersInConference As Integer = 0
                For Each s As String In myDevices.ConferenceIdList
                    If s <> "" Then
                        iNoOfActiveConf += 1
                        WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  conf id is: " & s, LogLevel.Normal)
                        HTML = ChilkatSametimeHTTP.QuickGetStr(myURL + "?method=ConferenceData&conferenceId=" + s)
                        WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  conf Data JSON is: " & HTML, LogLevel.Normal)

                        If HTML <> "" Then
                            myConfData = returnConfDataObject(Server, HTML)
                            If myConfData IsNot Nothing Then
                                WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  My conf Data is: Start Time: " & myConfData.StartTime(0).ToString(), LogLevel.Normal)
                                WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  My conf Data is: Current Users: " & myConfData.CurrentUsers(0).ToString(), LogLevel.Normal)
                                iTotalUsersInConference += Convert.ToInt32(myConfData.CurrentUsers(0).ToString())
                            End If
                        End If


                    End If


                Next
                HTML = ChilkatSametimeHTTP.QuickGetStr(myURL + "?method=AcrossConferenceData")
                If HTML <> "" Then
                    myAcrossConfData = returnAcrossConfDataObject(Server, HTML)
                    If myAcrossConfData IsNot Nothing Then
                        WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  My conf Data is:Countofall1x1calls: " & myAcrossConfData.P2PActiveCalls(0).ToString(), LogLevel.Normal)
                        WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  My conf Data is: Countofallmultiusercalls: " & myAcrossConfData.MCUActiveCalls(0).ToString(), LogLevel.Normal)

                        Server.StatisticsCollection.Add(New MonitoredItems.SametimeStatistic With {.Name = "Countofallcalls", .Value = myAcrossConfData.AllActiveCalls(0).ToString()})
                        Server.StatisticsCollection.Add(New MonitoredItems.SametimeStatistic With {.Name = "Countofallusers", .Value = myAcrossConfData.AllActiveUsers(0).ToString()})

                        Server.StatisticsCollection.Add(New MonitoredItems.SametimeStatistic With {.Name = "Countofall1x1calls", .Value = myAcrossConfData.P2PActiveCalls(0).ToString()})
                        Server.StatisticsCollection.Add(New MonitoredItems.SametimeStatistic With {.Name = "Countofall1x1users", .Value = myAcrossConfData.P2PActiveUsers(0).ToString()})
                        Server.StatisticsCollection.Add(New MonitoredItems.SametimeStatistic With {.Name = "Totalcountofall1x1calls", .Value = myAcrossConfData.P2PActiveCalls(0).ToString()})
                        Server.StatisticsCollection.Add(New MonitoredItems.SametimeStatistic With {.Name = "Totalcountofallcalls", .Value = myAcrossConfData.AllActiveCalls(0).ToString()})
                        Server.StatisticsCollection.Add(New MonitoredItems.SametimeStatistic With {.Name = "Totalcountofallmultiusercalls", .Value = myAcrossConfData.MCUActiveCalls(0).ToString()})

                        Server.StatisticsCollection.Add(New MonitoredItems.SametimeStatistic With {.Name = "Countofallmultiusercalls", .Value = myAcrossConfData.MCUActiveCalls(0).ToString()})
                        Server.StatisticsCollection.Add(New MonitoredItems.SametimeStatistic With {.Name = "Countofallmultiuserusers", .Value = myAcrossConfData.P2PActiveUsers(0).ToString()})
                    End If
                End If
                Server.StatisticsCollection.Add(New MonitoredItems.SametimeStatistic With {.Name = "Numberofactivemeetings", .Value = iNoOfActiveConf.ToString()})
                Server.StatisticsCollection.Add(New MonitoredItems.SametimeStatistic With {.Name = "Currentnumberofusersinsidemeetings", .Value = iTotalUsersInConference.ToString()})


            End If
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Username  is " & Server.UserId1)
            ' WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Password is " & mySametimePassword1)
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Server response is: " & vbCrLf & HTML, LogLevel.Normal)
        End If

        'ParseSametimeXMLStatistics(HTML)

        ' SametimeHTTP.Get("http://" & Server.IPAddress & "/servlet/statistics")
    End Sub
    Private Function returnConfIdObject(Server As MonitoredItems.SametimeServer, ByVal s As String) As ConferenceIdList
        Dim myDevices As New ConferenceIdList
        Dim serializer As New DataContractJsonSerializer(myDevices.[GetType]())

        Dim ms As New MemoryStream(Encoding.Unicode.GetBytes(s))
        Try
            myDevices = DirectCast(serializer.ReadObject(ms), ConferenceIdList)
        Catch ex As Exception
            'WriteDeviceHistoryEntry("All", "Traveler_Users", "Error converting stream to object: " & ex.ToString)
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "Error converting stream to object: " & vbCrLf & s, LogLevel.Normal)
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "Error converting stream to object: " & ex.ToString, LogLevel.Normal)
        End Try

        ms.Close()
        ms.Dispose()
        Return myDevices
    End Function
    Private Function returnConfDataObject(Server As MonitoredItems.SametimeServer, ByVal s As String) As ConferenceData
        Dim myDevices As New ConferenceData
        Dim serializer As New DataContractJsonSerializer(myDevices.[GetType]())

        Dim ms As New MemoryStream(Encoding.Unicode.GetBytes(s))
        Try
            myDevices = DirectCast(serializer.ReadObject(ms), ConferenceData)
        Catch ex As Exception
            'WriteDeviceHistoryEntry("All", "Traveler_Users", "Error converting stream to object: " & ex.ToString)
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "returnConfDataObject: Error converting stream to object: " & vbCrLf & s, LogLevel.Normal)
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "returnConfDataObject: Error converting stream to object: " & ex.ToString, LogLevel.Normal)
        End Try

        ms.Close()
        ms.Dispose()
        Return myDevices
    End Function
    Private Function returnAcrossConfDataObject(Server As MonitoredItems.SametimeServer, ByVal s As String) As AcrossConferenceData
        Dim myDevices As New AcrossConferenceData
        Dim serializer As New DataContractJsonSerializer(myDevices.[GetType]())

        Dim ms As New MemoryStream(Encoding.Unicode.GetBytes(s))
        Try
            myDevices = DirectCast(serializer.ReadObject(ms), AcrossConferenceData)
        Catch ex As Exception
            'WriteDeviceHistoryEntry("All", "Traveler_Users", "Error converting stream to object: " & ex.ToString)
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "returnAcrossConfDataObject: Error converting stream to object: " & vbCrLf & s, LogLevel.Normal)
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "returnAcrossConfDataObject: Error converting stream to object: " & ex.ToString, LogLevel.Normal)
        End Try

        ms.Close()
        ms.Dispose()
        Return myDevices
    End Function
#Region "Testing Sametime Server as a User"


    Public Sub TestSametimeServerAsUser(ByRef Server As MonitoredItems.SametimeServer)
        ' If Server.Enabled = False Then Exit Sub

        Dim strResults As String = ""
        Dim ProcessProperties As New ProcessStartInfo
        ProcessProperties.FileName = "java"
        ' ProcessProperties.Arguments = "-classpath .;SameTime.jar;STComm.jar com.rpr.sametime.SametimeTest azphxweb1.rprwyatt.com ""One Sametime/RPRWyatt""  rprwyatt123 azphxweb1.rprwyatt.com ""Two Sametime/RPRWyatt"" rprwyatt123" 'command line arguments
        dtSametimeLastUpdate = Now
        Dim Arguments As String = "-classpath .;SameTime.jar;STComm.jar com.rpr.sametime.SametimeTest ""STServer"" ""STUser1""  ""STPW1"" STServer ""STUser2"" ""STPW2""" 'command line arguments
        Arguments = Arguments.Replace("STServer", Server.IPAddress)
        Arguments = Arguments.Replace("STUser1", Server.UserId1)
        Arguments = Arguments.Replace("STUser2", Server.UserId2)
        Arguments = Arguments.Replace("STPW1", Server.Password1)
        Arguments = Arguments.Replace("STPW2", Server.Password2)

        'two sametime  users should be configured
        If Trim(Server.UserId1) = "" Or Trim(Server.UserId2) = "" Then
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Sametime users are not configured, so I cannot continue. You need to configure two users to continue.")
            Exit Sub
        End If

        'We also need two sametime passwords?
        If Trim(Server.Password1) = "" Or Trim(Server.Password2) = "" Then
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Sametime passwords are not configured, so I cannot continue. You need to configure two users with passwords to continue.")
            Exit Sub
        End If

        ProcessProperties.Arguments = Arguments
        ProcessProperties.WindowStyle = ProcessWindowStyle.Hidden

        Arguments = Arguments.Replace(Server.Password1, "********")
        Arguments = Arguments.Replace(Server.Password2, "********")
        WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & "  Arguments to Java are " & Arguments)
        ProcessProperties.RedirectStandardOutput = True
        ProcessProperties.UseShellExecute = False
        ProcessProperties.WorkingDirectory = strAppPath


        Try
            SametimeProcess = Process.Start(ProcessProperties)
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Calling Process.Start")
            strResults = SametimeProcess.StandardOutput.ReadToEnd()
            ' WriteDeviceHistoryEntry("Sametime", Server.Name, strResults)
            'WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Calling Process.Kill")
            ' If myProcess IsNot Nothing Then myProcess.Kill()
            If Not SametimeProcess.HasExited Then
                SametimeProcess.Kill()
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Error calling Process.start: " & ex.ToString)
            Server.Status = "Not Responding"
            Server.Description = Left(ex.ToString, 250) & "..."
        End Try

        dtSametimeLastUpdate = Now


        Try
            If strResults <> "" Then
                Server.Description = "All tests (login, user resolve, status change, and IM) passed successfully"
                If Server.TestChatSimulation Then
                    Server.Description = "All tests (login, user resolve, status change, and IM) passed successfully"
                Else
                    Server.Description = "All tests (login, user resolve and status change) passed successfully"
                End If
                Server.Status = "OK"
                Dim dt As DataTable = parseXML(strResults)
                If dt.Rows.Count >= 1 Then Server.Time_Login = dt.Rows(0)(1).ToString()
                If dt.Rows.Count >= 2 Then Server.Time_Resolve = dt.Rows(1)(1).ToString()

                If dt.Rows.Count >= 1 Then Server.Test_Login = dt.Rows(0)(2).ToString()
                If dt.Rows.Count >= 2 Then Server.Test_Resolve = dt.Rows(1)(2).ToString()
                If dt.Rows.Count >= 3 Then Server.Test_StatusChange = dt.Rows(2)(2).ToString()
                If Server.TestChatSimulation Then
                    If dt.Rows.Count >= 4 Then Server.Test_IM = dt.Rows(3)(2).ToString()
                End If
                If dt.Rows.Count >= 5 Then Server.Test_Awareness = dt.Rows(4)(2).ToString()
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Error parsing results data table : " & ex.ToString)
        End Try
        Try
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Login time was " & Server.Time_Login)
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Resolve time was " & Server.Time_Resolve)
            If Server.TestChatSimulation Then
                WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Test IM " & Server.Test_IM)
            End If
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Test Awareness " & Server.Test_Awareness)
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Test Status Change " & Server.Test_StatusChange)
            'WriteDeviceHistoryEntry("Sametime", Server.Name, strResults)
        Catch ex As Exception

        End Try

        Try
            If Server.Time_Login <> 0 Then
                Server.ResponseTime = Server.Time_Login
                InsertSametimeResponseTime(Server.Name, Server.ResponseTime)
            ElseIf Server.Time_Resolve <> 0 Then
                Server.ResponseTime = Server.Time_Resolve
                InsertSametimeResponseTime(Server.Name, Server.ResponseTime)
            End If

        Catch ex As Exception

        End Try

        Try
            If Server.Test_Login = "OK" Then
                myAlert.ResetAlert("Sametime", Server.Name, "Login", Server.Location)
                Server.IsUp = True
                Server.IncrementUpCount()
            Else
                Server.IncrementDownCount()
                If Server.FailureCount >= Server.FailureThreshold Then
                    'Mukund 22Jan15: VSPLUS-1303 If login test fails for Sametime, other rows in health assement are not updated
                    '----------------------------
                    Dim strReturn As String = ""
                    strReturn = myAlert.ClearStatusDetails(Server.Name, "Sametime")
                    If strReturn <> "" Then
                        WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Error: ClearStatusDetails requires atleast server name")
                    End If
                    '----------------------------
                    myAlert.QueueAlert("Sametime", Server.Name, "Login", "Failed to login as a Sametime User.  Either the login service is down or the server is down.", Server.Location)
                End If
                Server.Status = "Login Failure"
                Server.Description = "Failed to login as a Sametime User.  Either the login service is down or the server is down."

                Server.IsUp = False
                Exit Sub
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Error evaluating Login test: " & ex.ToString)
        End Try

        Try
            If Server.Test_Resolve = "OK" Then
                myAlert.ResetAlert("Sametime", Server.Name, "Resolve", Server.Location)
            Else
                If Server.FailureCount > Server.FailureThreshold Then
                    myAlert.QueueAlert("Sametime", Server.Name, "Resolve", "Failed to resolve a known Sametime User.  Either the resolve service is down or the directory has an issue.", Server.Location)
                End If

                Server.Status = "Resolve Failure"
                Server.Description = "Failed to resolve a Sametime User. Users logging in will have problems with their buddy list."
                Server.IncrementDownCount()
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Error evaluating Resolve test: " & ex.ToString)
        End Try

        Try
            If Server.Test_StatusChange = "OK" Then
                myAlert.ResetAlert("Sametime", Server.Name, "Status Change", Server.Location)
            Else
                If Server.FailureCount > Server.FailureThreshold Then
                    myAlert.QueueAlert("Sametime", Server.Name, "Status Change", "Failed to track the status change of a known Sametime User. ", Server.Location)
                End If
                Server.Status = "Status Change Failure"
                Server.Description = "Failed status change test for Sametime User. Some logged in users may have an out-of-date availability status."
                Server.IncrementDownCount()
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Error evaluating Status Change test: " & ex.ToString)
        End Try

        If Server.TestChatSimulation Then

            Try
                If Server.Test_IM = "OK" Then
                    myAlert.ResetAlert("Sametime", Server.Name, "IM", Server.Location)
                Else
                    If Server.FailureCount > Server.FailureThreshold Then
                        myAlert.QueueAlert("Sametime", Server.Name, "IM", "Failed to successfully instant message with another Sametime User. ", Server.Location)
                    End If
                    Server.Status = "IM Failure"
                    Server.Description = "Failed instant message test-- a critical function."
                    Server.IncrementDownCount()
                End If
            Catch ex As Exception
                WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Error evaluating Status Change test: " & ex.ToString)
            End Try
        End If

        Try
            If Server.Test_Awareness = "OK" Then
                myAlert.ResetAlert("Sametime", Server.Name, "Awareness", Server.Location)
            Else
                If Server.FailureCount > Server.FailureThreshold Then
                    myAlert.QueueAlert("Sametime", Server.Name, "Awareness", "Failed awareness test. ", Server.Location)
                End If

                Server.Description = "Failed awareness test for Sametime User. Some logged in users may appear offline."
                Server.Status = "Awareness Failure"
                Server.IncrementDownCount()
            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Sametime", Server.Name, Now.ToString & " Error evaluating Awareness test: " & ex.ToString)
        End Try

        With Server
            If .Test_Login = "OK" And .Test_Awareness = "OK" And .Test_IM = "OK" And .Test_Resolve = "OK" And .Test_StatusChange = "OK" Then
                Server.IncrementUpCount()
            End If
        End With

    End Sub


    'Mukund 11Nov13, parsing out the Sametime monitoring results
    Public Function parseXML(ByVal sInput As String) As DataTable
        Dim sOutput As String = ""
        Dim newDD As DataTable = New DataTable()
        Try
            Dim ds As New DataSet()

            'Read XML from Textbox
            Dim reader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(New System.IO.StringReader(sInput))
            reader.Read()

            'Read XML into Datatable
            ds.ReadXml(reader, System.Data.XmlReadMode.Auto)
            Dim dd As DataTable = ds.Tables(0)

            'Get all distinct Names from Datatable
            Dim distinctDT As DataTable = dd.DefaultView.ToTable(True, "name")

            'Clone structure for output Table
            Dim newDS As DataSet = ds.Clone()
            newDD = newDS.Tables(0)

            'Loop for distinct names
            For i As Integer = 0 To distinctDT.Rows.Count - 1
                Dim sname As String = distinctDT.Rows(i)("name")
                Dim sReason As String = ""
                Dim iReason As Integer = 0
                Dim sStatus As String = "OK"

                'Find Status<> OK and take that status
                Dim statdd() As DataRow = dd.Select("name='" & sname & "' and Status<>'OK'")
                If statdd.Length > 0 Then
                    sStatus = statdd(0).Item("Status")
                End If

                'For Login, calculate the Time taken and get average from the output. 
                'Note the time is taken from input after the string ' Time = ', as in :: ...Time = 2175
                If sname = "Login" Then
                    Dim LoginTimedd() As DataRow = dd.Select("name='Login'")
                    For j As Integer = 0 To LoginTimedd.Length - 1
                        If LoginTimedd(j)("Reason").ToString().IndexOf("Time =") >= 0 Then
                            iReason += LoginTimedd(j)("Reason").ToString().Substring(LoginTimedd(j)("Reason").ToString().IndexOf("Time =") + 6)
                        End If
                    Next
                    If LoginTimedd.Length > 0 Then
                        iReason = iReason / LoginTimedd.Length
                    End If
                    sReason = iReason.ToString()
                End If

                'For Resolve, calculate the Time taken and get sum from the output.
                ' Note the time is taken from input between the strings 'was' and 'milliseconds', as in :: ...was 211 milliseconds
                If sname = "Resolve" Then
                    Dim ResolveTimedd() As DataRow = dd.Select("name='Resolve'")
                    For j As Integer = 0 To ResolveTimedd.Length - 1
                        If ResolveTimedd(j)("Reason").ToString().IndexOf("was") >= 0 Then
                            iReason += ResolveTimedd(j)("Reason").ToString().Substring(ResolveTimedd(j)("Reason").ToString().IndexOf("was") + 4, ResolveTimedd(j)("Reason").ToString().IndexOf("milliseconds") - ResolveTimedd(j)("Reason").ToString().IndexOf("was") - 5)
                        End If
                    Next
                    sReason = iReason.ToString()
                End If
                newDD.Rows.Add(sname, sReason, sStatus)
            Next

        Catch ex As Exception
            'sOutput = ex.Message
        End Try
        Return newDD
    End Function
#End Region

#End Region


#End Region

End Class
