
'Also use IP*Works components, see http://www.ipworks.com
' .NET Reactor  www.ezriz.com

'Written by Alan Forbes and made awesome by Joe Thumma


'Copyright 2014, All Rights Reserved JNIT Inc. and Plum Island Publishing, LLC

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

Imports RPRWyatt.VitalSigns.Services

Public Class VitalSignsPlusDomino
    Inherits VSServices







    ''' <summary>
    '''  REMOVE THIS!!!
    ''' </summary>
    ''' <remarks></remarks>
    Dim connectionString As String = "mongodb://localhost/VitalSigns"



	Dim BuildNumber As Integer = 2281
	Dim ProductName As String 'value set in start up 
	Dim CompanyName As String = "RPR Wyatt"
	Dim ListOfDominoThreads As List(Of Threading.Thread) = New List(Of Threading.Thread)
	'Dim ListOfURLThreads As List(Of Threading.Thread) = New List(Of Threading.Thread)
	Dim ListOfTravelerUserThreads As List(Of Threading.Thread) = New List(Of Threading.Thread)
	Dim sCultureString As String = "en-US"
	Dim connectionStringName As String = "CultureString"
	'Used by C API
	Dim mNotesProgDir As String	' = "c:\program files\lotus\notes"
	Dim mNotesINI As String	' = "=c:\program files\lotus\notes\notes.ini"
	Dim mUserId As String '= "C:\Program Files\lotus\notes\data\aforbesMZL.id"
	Private Shared DominoSelector_Mutex As New Mutex()
	' Private Shared NotesAPI_Mutex As New Mutex()
	' Dim Use_NotesAPI_Mutex As Boolean = False
	Dim NotesSession As New Domino.NotesSession
    '1/4/2016 NS added for VSPLUS-2434
    Dim MailStatsDict As New Dictionary(Of String, Integer)
	'Private Shared URLSelector_Mutex As New Mutex()


	Dim strDateFormat As String
	Dim objDateUtils As New DateUtils.DateUtils

	'  Dim WithEvents PingControl As New nsoftware.IPWorks.Ping("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000")
	Dim WithEvents HTTP As New nsoftware.IPWorks.Http("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000")
	'  Dim IPPort As New nsoftware.IPWorks.Ipport("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000")


	'Threads
	' Dim MasterDominoThread As New Thread(AddressOf MonitorAllThingsDomino)
	Dim CheckThreads As New Thread(AddressOf ThreadChecker)

	'Dim ThreadPingCycle As New Thread(AddressOf PingCyle)
	'Dim ThreadScanDominoCycle As New Thread(AddressOf DominoScanCycle)
	Dim ThreadTravelerUsers As New Thread(AddressOf CycleTravelerUsers)
	Dim ThreadTravelerUsersMoreDetails As New Thread(AddressOf getTravelerDeviceMoreDetailsWrapper)
	Dim ThreadTrackKeyDevices As New Thread(AddressOf TrackKeyDevices)
	Dim ThreadSetActiveDevicesLoop As New Thread(AddressOf SetActiveDevicesLoop)
	' Dim ThreadVitalStatus As New Thread(AddressOf VitalStatus_Thread)

	'  Dim WithEvents PingControl As New nsoftware.IPWorks.Ping("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000")
	Dim myAlert As New AlertLibrary.Alertdll

	Dim boolTimeToStop As Boolean = False

	' Monitor log file & other paths
	Private strLogDest As String
	Private strConsoleCommandLogDest As String
	Private strAppPath As String
	Dim strAuditText As String
	Dim strServersMDBPath As String
	'Dim myStatusURL As String

	'Store the passwords in these variables
	Dim MyDominoPassword As String
	Dim MySMTPPassword As String

	Dim BannerText As String

	'Collection of Domino servers to monitor
	Dim MyDominoServers As New MonitoredItems.DominoCollection

	Dim myKeywords As New KeywordsCollection
	Dim myTravelerBackends As New MonitoredItems.Traveler_Backend_Collection


	'Collection of Domino Mail Clusters to monitor
	Dim myDominoClusters As New MonitoredItems.DominoMailClusterCollection
	Dim myDominoCluster As New MonitoredItems.DominoMailCluster

	'Collection of Notes Databases
	Dim MyNotesDatabase As MonitoredItems.NotesDatabase
	Dim MyNotesDatabases As New MonitoredItems.NotesDatabaseCollection


	'Collection of NotesMailProbes
	Dim MyNotesMailProbe As MonitoredItems.DominoMailProbe
	Dim MyNotesMailProbes As New MonitoredItems.DominoMailProbeCollection

	'Disk space
	Dim DiskName(70) As String


	'Collection of URLs
	'Dim MyURL As MonitoredItems.URL
	'Dim MyURLs As New MonitoredItems.URLCollection
	'Dim CurrentURL As String 'Contains the name of the URL currently being queried
	'Dim URLStringFound As Boolean
	'Dim dtURLsLastUpdate As DateTime = Now

	'Thread Last Update Global Variables
	Dim dtDominoLastUpdate As DateTime = Now

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
	'  Dim ThreadURL As New Thread(AddressOf MonitorURLs)

	Dim HungThreadsKilled As Integer = 0

    Protected Overrides Sub ServiceOnStart(ByVal args() As String)

        Try
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings("VitalSignsMongo").ToString()
            WriteAuditEntry(Now.ToString + " connection string is " & connectionString)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString + " Error getting connection string. Error: " & ex.Message)
        End Try

        Try
            sCultureString = ConfigurationManager.AppSettings(connectionStringName).ToString()
        Catch ex As Exception
            sCultureString = "en-US"
        End Try


        Thread.CurrentThread.CurrentCulture = New CultureInfo(sCultureString)

        'Writes to the History.txt file
        Try
            ThreadHourlyDaily.Start()
        Catch ex As Exception

        End Try



        'Read some settings from the registry
        Dim myRegistry As New VSFramework.RegistryHandler()


        Try
            myRegistry.WriteToRegistry("Cluster Health Date", Now.ToShortDateString)
        Catch ex As Exception

        End Try

        Try
            UpdateDominoThreadKilledCounter()
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
            MyLogLevel = LogLevel.Verbose
        End Try



        Try
            myRegistry.WriteToRegistry("Service Start", Now.ToShortDateString & " " & Now.ToShortTimeString)
            myRegistry.WriteToRegistry("VS Domino Service Start", Now.ToShortDateString & " " & Now.ToShortTimeString)
            '   myRegistry.WriteToRegistry("VS Domino Build Number", BuildNumber)
            myRegistry.WriteToRegistry("Service Build Number", BuildNumber)
            'myRegistry.WriteToRegistry("Service Reset Date", Now.ToShortDateString)
        Catch ex As Exception

        End Try


        Try
            WriteAuditEntry(Now.ToString + " ")
            WriteAuditEntry(Now.ToString + " ********************************* ")
            WriteAuditEntry(Now.ToString + " VitalSigns Plus - Build Number: " & BuildNumber)
            WriteAuditEntry(Now.ToString + " The service is starting up.")
            WriteAuditEntry(Now.ToString + " ")
            WriteAuditEntry(Now.ToString + " Copyright 2006 - " & Now.Year & ", JNITH Corporation. dba RPR Wyatt and Plum Island Publishing, LLC.")
            WriteAuditEntry(Now.ToString + " All rights reserved.")
            WriteAuditEntry(Now.ToString + " Distributed worldwide by RPR Wyatt, Inc. and its partner network.")
            WriteAuditEntry(Now.ToString + " ")
            WriteAuditEntry(Now.ToString + " ")
            WriteAuditEntry(Now.ToString + " ")
            '     If Use_NotesAPI_Mutex = True Then WriteAuditEntry(Now.ToString + " Using the Notes API mutex to minimize API collisions.")

            ' Thread.CurrentThread.Sleep(4000)

        Catch ex As Exception

        End Try


        Try
            Dim myAdapter As New VSFramework.XMLOperation
            Dim MyConnectionString As String
            MyConnectionString = myAdapter.GetDBConnectionString("VitalSigns")
            'WriteAuditEntry(Now.ToString + " My connection string is " & MyConnectionString)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString + " Exception getting connection string: " & ex.ToString)
        End Try

        Try
            mNotesProgDir = myRegistry.ReadFromRegistry("Notes Program Directory")
            If mNotesProgDir = "" Then
                WriteAuditEntry(Now.ToString & " *** WARNING *** The Notes Program Directory has to be set using the Configurator, Stored Password & Options, IBM Domino Settings.")
                End
            Else
                WriteAuditEntry(Now.ToString & " The Notes Program Directory is " & mNotesProgDir)
            End If

        Catch ex As Exception

        End Try

        Try
            mNotesINI = "=" & myRegistry.ReadFromRegistry("Notes.ini")
            'Should be like "=c:\program files\lotus\notes\notes.ini"
            If mNotesINI = "=" Then
                WriteAuditEntry(Now.ToString & "  *** WARNING *** The Notes.INI location has to be set using the Configurator, Stored Password & Options, IBM Domino Settings.")
            Else
                WriteAuditEntry(Now.ToString & " The Notes.INI setting is " & mNotesINI)
            End If
        Catch ex As Exception

        End Try

        Try
            mUserId = myRegistry.ReadFromRegistry("Notes User ID")
            If mUserId = "" Then
                WriteAuditEntry(Now.ToString & "  *** WARNING ***  The Notes User ID file location has to be set using the Configurator, Stored Password & Options, IBM Domino Settings.")
                ' End
            Else
                WriteAuditEntry(Now.ToString & " The Notes User ID setting is " & mUserId)
            End If
        Catch ex As Exception

        End Try

        Try
            Dim sStartStatus As String
            InitNotes(sStartStatus)
            ' WriteAuditEntry(Now.ToString & " Notes API initialization: " & sStartStatus)
        Catch ex As Exception

        End Try

        Dim NotesSystemMessageString As String = "Incorrect Notes Password."
        Try
            MyDominoPassword = GetNotesPassword()
            NotesSession.Initialize(MyDominoPassword)
            WriteAuditEntry(Now.ToString & " Initialized NotesSession for " & NotesSession.CommonUserName)
            'CleanAlertsDB()
        Catch ex As Exception
            System.Runtime.InteropServices.Marshal.ReleaseComObject(NotesSession)
            '3/13/2015 NS added for VSPLUS-1476
            myAlert.QueueSysMessage(NotesSystemMessageString)
            WriteAuditEntry(Now.ToString & " Error Initializing NotesSession.  Many problems will follow....  " & ex.ToString)
            WriteAuditEntry(Now.ToString & " *********** ERROR ************  Error initializing a session to Query Domino server. ")
            WriteAuditEntry(Now.ToString & " Calling stopnotescl.exe then exiting in an attempt to recover.")
            WriteAuditEntry(Now.ToString & " The VitalSigns Master service should restart the monitoring service in a few moments.")
            KillNotes()
            Exit Sub
        End Try

        Try
            myAlert.ResetSysMessage(NotesSystemMessageString)
        Catch ex As Exception

        End Try

        WriteAuditEntry(Now.ToString & " Calling Off Hours check.....")

        Try
            CreateCollections()
        Catch ex As Exception

        End Try


        Try
            InitializeStatusTable()
        Catch ex As Exception

        End Try

        Try
            'UpdateStatusTable("Update Status SET  Details='The Domino monitoring service is starting up.', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0, Description='The VitalSigns monitoring service is starting up.' WHERE Status <> 'Disabled' AND Type='Domino Server'")
            'UpdateStatusTable("Update Status SET  Details='The Domino monitoring service is starting up.', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0, Description='The VitalSigns monitoring service is starting up.' WHERE Status <> 'Disabled' AND Type='Notes Database'")
            'UpdateStatusTable("Update Status SET  Details='The URL monitoring service is starting up.', Status='Not Scanned', StatusCode='Not Scanned', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0, Description='URL monitoring is not running.' WHERE Type ='URL'")
        Catch ex As Exception

        End Try


        Try
            '4/29/2014 - this was changed to a thread instead of just calling StartThreads directly so it would start quicker
            WriteAuditEntry(Now.ToString & " All configuration settings have been read.  Starting monitoring threads.")
            Dim threadStartThreads As New Thread(AddressOf StartThreads)
            'Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")
            threadStartThreads.CurrentCulture = New CultureInfo(sCultureString)

            threadStartThreads.Start()

        Catch ex As Exception

        End Try

        WriteAuditEntry(Now.ToString & " Startup process is complete. ")

    End Sub

	Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        '1/4/2016 NS added for VSPLUS-2434
        UpdateMailStats()
		boolTimeToStop = True
		Thread.Sleep(1000)
		Try
			WriteAuditEntry(Now.ToString + " The service is stopping.")
			'UpdateStatusTable("Update Status SET  Details='The Domino monitoring service is not running.', Status='Not Scanned', StatusCode='Maintenance', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0, Description='Domino monitoring is not running.' WHERE Type ='Domino'")
			'UpdateStatusTable("Update Status SET  Details='The Domino monitoring service is not running.', Status='Not Scanned', StatusCode='Maintenance', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0, Description='Domino monitoring is not running.' WHERE Type ='Notes Database'")
			'UpdateStatusTable("Update Status SET  Details='The Domino monitoring service is not running.', Status='Not Scanned', StatusCode='Maintenance', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0, Description='Domino monitoring is not running.' WHERE Type ='NotesMail Probe'")
			'UpdateStatusTable("Update Status SET  Details='The URL monitoring service is not running.', Status='Not Scanned', StatusCode='Maintenance', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0, Description='URL monitoring is not running.' WHERE Type ='URL'")
			System.Runtime.InteropServices.Marshal.ReleaseComObject(NotesSession)
		Catch ex As Exception

		End Try

		'End
		MyBase.OnStop()
	End Sub

    Dim dominoThreadCount As Integer = 0
    Dim initialDominoThreadCount As Integer = 0
    Dim AliveDominoMainThreads As System.Collections.ArrayList = New System.Collections.ArrayList()

	Protected Sub StartThreads()


		'**** Start the selected monitoring threads

     
        '3/2/2016 NS commented out
        'ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf MonitorDominoCluster))


        WriteAuditEntry(Now.ToString & " Attempting to start Domino monitoring... ")
		Dim threadMonitorDomino As New Thread(AddressOf MonitorDominoRelated)
		threadMonitorDomino.CurrentCulture = New CultureInfo(sCultureString)
        threadMonitorDomino.Start()

        '2/4/2016 NS added for VSPLUS-2560
        Try
            Dim threadMonitorCluster As New Thread(AddressOf MonitorDominoCluster)
            WriteAuditEntry(Now.ToString & " Attempting to start Domino Cluster analysis...", LogLevel.Verbose)
            threadMonitorCluster.Start()
            Thread.Sleep(250)
        Catch ex As Exception

        End Try

        '1/4/2016 NS added for VSPLUS-2434
        WriteAuditEntry(Now.ToString & " Attempting to start Mail stats SQL update ... ")
        Dim threadUpdateMailStats As New Thread(AddressOf UpdateMailStats)
        threadUpdateMailStats.CurrentCulture = New CultureInfo(sCultureString)
        threadUpdateMailStats.Start()


		WriteAuditEntry(Now.ToString & " Attempting to start NotesMail monitoring... ")
		Dim threadMonitorNotesMail As New Thread(AddressOf NotesMailLoop)
		threadMonitorNotesMail.CurrentCulture = New CultureInfo(sCultureString)
		threadMonitorNotesMail.Start()


		With ThreadTravelerUsers
			.IsBackground = True
			.Priority = ThreadPriority.Normal
			.CurrentCulture = New CultureInfo(sCultureString)
			WriteAuditEntry(Now.ToString & " Starting Traveler Users thread...")
			.Start()
		End With

		With ThreadTravelerUsersMoreDetails
			.IsBackground = True
			.Priority = ThreadPriority.Normal
			.CurrentCulture = New CultureInfo(sCultureString)
			WriteAuditEntry(Now.ToString & " Starting Traveler Users More Details thread...")
			.Start()
		End With

		With ThreadTrackKeyDevices
			.IsBackground = True
			.Priority = ThreadPriority.Normal
			.CurrentCulture = New CultureInfo(sCultureString)
			WriteAuditEntry(Now.ToString & " Starting Traveler Users Monitored Devices thread...")
			.Start()
		End With

		With ThreadSetActiveDevicesLoop
			.IsBackground = True
			.Priority = ThreadPriority.Normal
			.CurrentCulture = New CultureInfo(sCultureString)
			WriteAuditEntry(Now.ToString & " Starting Active Devices Loop Thread...")
			.Start()
		End With

        Dim boolMulti As Boolean = True
		Try
			Dim myRegistry As New VSFramework.RegistryHandler()
			If myRegistry.ReadFromRegistry("SuppressMultiThread") = True Then
				boolMulti = False
			End If
		Catch ex As Exception
			boolMulti = True
		End Try
        'ThreadPool.

		If boolMulti = False Then
			WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " >>> Suppressing multithreaded operation due to configuration setting.", LogLevel.Normal)
			Dim threadScanDomino1 As New Thread(AddressOf MonitorDomino)
			threadScanDomino1.CurrentCulture = New CultureInfo(sCultureString)
			threadScanDomino1.Start()
		Else
            ''Scan the Domino servers, creating more threads if you have more servers            
            StartMonitoringThreads()


			'Catch ex As Exception

			'End Try

		End If


		'These Threads start no matter what


		Try

			WriteAuditEntry(Now.ToString & " Now starting the Hourly Task checker...")
			ThreadHourlyDaily.IsBackground = True
			ThreadHourlyDaily.Priority = ThreadPriority.Normal
			ThreadHourlyDaily.Start()
		Catch ex As Exception

		End Try

		Try
			WriteAuditEntry(Now.ToString & " Now starting the thread checker...")
			CheckThreads.IsBackground = True
			CheckThreads.Priority = ThreadPriority.Normal
			CheckThreads.Start()
		Catch ex As Exception

		End Try
    End Sub


    Protected Sub StartMonitoringThreads()
        Dim EnabledCount As Integer = 0
        Dim ServerOne As MonitoredItems.DominoServer
        For n = 0 To MyDominoServers.Count - 1
            ServerOne = MyDominoServers.Item(n)
            '6/18/2015 NS modified for VSPLUS-1802
            If ServerOne.Enabled = True Then
                'If ServerOne.EXJEnabled = True Then
                EnabledCount += 1
                'End If
            End If
        Next n
        Dim maxThreadCount As Integer = getThreadCount("Domino")
        Dim startThreads As Integer = 0
        dominoThreadCount = EnabledCount / 3
        If dominoThreadCount <= 1 Then
            dominoThreadCount = 2
        End If

        ' 5/19/15 WS commented out.  VSPLUS 1776
        If dominoThreadCount > maxThreadCount Then
            dominoThreadCount = maxThreadCount
        End If
        'startThreads = initialDominoThreadCount
        'If initialDominoThreadCount > dominoThreadCount Then
        '    'remove the extra threads
        '    Dim j As Integer = initialDominoThreadCount - dominoThreadCount
        '    'if inital threads are 5 and current threads are 3
        '    '5-3=2: //remove 2 threads
        '    For Each th As Thread In AliveDominoMainThreads
        '        If j > 0 Then
        '            If th.IsAlive Then
        '                th.Abort()
        '            End If
        '            j -= 1
        '        End If
        '    Next
        'End If
        'initialDominoThreadCount = dominoThreadCount



        WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " There are " & EnabledCount & " enabled Domino servers to scan.", LogLevel.Normal)
        WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " I am launching " & dominoThreadCount & " threads to scan these servers.")
        ' WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " I am launching " & (intThreadCount - ListOfDominoThreads.Count) & " new threads for " & intThreadCount & " total threads to scan these servers.")

        Try
            For n = startThreads To dominoThreadCount
                ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf MonitorDomino))
                'Dim tTemp As Threading.Thread = New Threading.Thread(AddressOf MonitorDomino)
                'tTemp.Start()
                'AliveDominoMainThreads.Add(tTemp)
                'Threading.Thread.Sleep(2000)  'sleep 2 seconds then start another thread
            Next
        Catch ex As Exception
            If InStr(ex.ToString, "System.OutOfMemoryException") > 0 Then
                WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " VitalSigns is out of memory.  Attempting to exit so the Master service will restart it. ")
                Thread.Sleep(1000)
                End
            Else
                WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " Error starting MonitorDomino thread " & ex.ToString)
            End If

        End Try

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

	Public Function InMaintenanceOLD(ByVal DeviceType As String, ByVal DeviceName As String) As Boolean
		Dim InMaintenanceWindow As Boolean = False
		Dim dsMaintWindows As New Data.DataSet
		Dim myPath As String
		Dim myRegistry As New VSFramework.RegistryHandler
		'Read the registry for the location of the Config Database
		myPath = myRegistry.ReadFromRegistry("Data Path")
		'Try
		'    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Using Configuration database " & myPath)
		'Catch ex As Exception
		'    WriteAuditEntry(Now.ToString & " Failed to read registry in Maintenance window code. Exception: " & ex.Message)
		'End Try

		If myPath Is Nothing Then
			WriteAuditEntry(Now.ToString & " Error: Failed to read registry in Maintenance window code.   Cannot locate Config Database 'servers.mdb'.  Configure by running" & ProductName & " client before starting the service.")
			Return False
			Exit Function
		End If
		myRegistry = Nothing
		Dim strSQL As String
		Try
			'COMMENTED BY MUKUND 28Feb12


			myPath = Nothing


			strSQL = " Select MaintenanceWindows.MaintWindow as Name, MaintenanceSettings.StartTime, MaintenanceSettings.EndTime, MaintenanceSettings.Monday, MaintenanceSettings.Tuesday, MaintenanceSettings.Wednesday, " & _
						"MaintenanceSettings.Thursday, MaintenanceSettings.Friday, MaintenanceSettings.Saturday, MaintenanceSettings.Sunday " & _
						"FROM " & _
						"MaintenanceSettings INNER JOIN  MaintenanceWindows " & _
						"ON MaintenanceSettings.Name=MaintenanceWindows.MaintWindow WHERE MaintenanceWindows.DeviceType='" & DeviceType & "' AND MaintenanceWindows.Name='" & DeviceName & "' "
			dsMaintWindows.Clear()
			'WRITTEN BY MUKUND 28Feb12
			Dim objVSAdaptor As New VSFramework.VSAdaptor
			objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsMaintWindows, "MaintWindows")

		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Error in maint window module dataset creation: " & ex.Message)
			Return False
			Exit Function

		Finally
			'myConnection.Close()
			'myCommand.Dispose()
			'myConnection.Dispose()
			'myAdapter.Dispose()

		End Try



		Dim DayofWeek As String
		DayofWeek = Now.DayOfWeek.ToString

		Dim MyStartTime, MyEndTime As DateTime
		Dim TimeNow As DateTime = Now
		Dim boolInWindow As Boolean

		Try
			Dim dr As DataRow
			'    WriteAuditEntry(" Server " & DeviceName & " has " & dsMaintWindows.Tables("MaintWindows").Rows.Count & " maintenance windows.")
			For Each dr In dsMaintWindows.Tables("MaintWindows").Rows
				Try
					MyStartTime = CType(dr.Item("StartTime"), DateTime)
					MyEndTime = CType(dr.Item("EndTime"), DateTime)
				Catch ex As Exception
					WriteAuditEntry(" Error calculating maintenance window start and end times. Error: " & ex.Message)
				End Try


				Dim myStartResult, myEndResult As TimeSpan
				'    WriteAuditEntry(" Maintenance Window Name is " & dr.Item("Name"))
				'   WriteAuditEntry(" Maintenance window starts at " & MyStartTime.ToShortTimeString & ". It is now " & TimeNow.ToShortTimeString & ". I can send it from  " & MyStartTime.ToLongTimeString & " to " & MyEndTime.ToLongTimeString)

				myStartResult = (MyStartTime.TimeOfDay.Subtract(TimeNow.TimeOfDay))
				'  WriteAuditEntry(" My Starttime Result in minutes = " & myStartResult.TotalMinutes)

				myEndResult = (MyEndTime.TimeOfDay.Subtract(TimeNow.TimeOfDay))
				' WriteAuditEntry(" My Endtime Result in minutes = " & myEndResult.TotalMinutes)


				If myStartResult.TotalMinutes < 0 And myEndResult.TotalMinutes >= 0 Then
					boolInWindow = True
					'     WriteAuditEntry(Now.ToString & " Currently within the maintenance window " & dr.Item("Name"))
				Else
					boolInWindow = False
					'    WriteAuditEntry(Now.ToString & " Currently outside the maintenance window " & dr.Item("Name"))
				End If

				If boolInWindow = True Then
					Select Case DayofWeek
						Case "Sunday"
							If dr.Item("Sunday") = True Then
								InMaintenanceWindow = True
								Exit For
							End If
						Case "Monday"
							If dr.Item("Monday") = True Then
								InMaintenanceWindow = True
								Exit For
							End If
						Case "Tuesday"
							If dr.Item("Tuesday") = True Then
								InMaintenanceWindow = True
								Exit For
							End If
						Case "Wednesday"
							If dr.Item("Wednesday") = True Then
								InMaintenanceWindow = True
								Exit For
							End If
						Case "Thursday"
							If dr.Item("Thursday") = True Then
								InMaintenanceWindow = True
								Exit For
							End If
						Case "Friday"
							If dr.Item("Friday") = True Then
								InMaintenanceWindow = True
								Exit For
							End If
						Case "Saturday"
							If dr.Item("Saturday") = True Then
								InMaintenanceWindow = True
								Exit For
							End If
						Case "Sunday"
							If dr.Item("Sunday") = True Then
								InMaintenanceWindow = True
								Exit For
							End If

					End Select
				End If
			Next

			dr = Nothing
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Error in maint window module: " & ex.Message)
		End Try

		Try
			dsMaintWindows.Dispose()
			myPath = Nothing
			GC.Collect()
		Catch ex As Exception

		End Try

		Return InMaintenanceWindow
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

	Public Function OLDOffHours() As Boolean
		'This function returns true if called during off hours, false if called during business hours
		Dim myRegistry As New VSFramework.RegistryHandler
		Dim TwentyFourHours, InBusinessHours As Boolean
		Try
			TwentyFourHours = myRegistry.ReadFromRegistry("24 Hours")
		Catch ex As Exception
			TwentyFourHours = False
		End Try

		If TwentyFourHours = True Then
			myRegistry = Nothing
			Return False
			Exit Function
		End If

		Dim DayofWeek As String
		DayofWeek = Now.DayOfWeek.ToString

		Dim MyStartTime, MyEndTime As DateTime
		Dim TimeNow As DateTime = Now
		Dim boolInWindow As Boolean

		Try
			MyStartTime = myRegistry.ReadFromRegistry("BusinessHoursStart")
			MyEndTime = myRegistry.ReadFromRegistry("BusinessHoursEnd")
		Catch ex As Exception
			WriteAuditEntry(" Error calculating Business Hours window start and end times. Error: " & ex.Message)
		End Try


		Dim myStartResult, myEndResult As TimeSpan
		myStartResult = (MyStartTime.TimeOfDay.Subtract(TimeNow.TimeOfDay))
		myEndResult = (MyEndTime.TimeOfDay.Subtract(TimeNow.TimeOfDay))

		Try
			If myStartResult.TotalMinutes < 0 And myEndResult.TotalMinutes >= 0 Then
				InBusinessHours = True
				'     WriteAuditEntry(Now.ToString & " Currently within the business hours")
			Else
				InBusinessHours = False
				'    WriteAuditEntry(Now.ToString & " Currently outside the business hours")
			End If

			If InBusinessHours = True Then
				Select Case DayofWeek
					Case "Sunday"
						If myRegistry.ReadFromRegistry("BusinessHoursSunday") = True Then
							InBusinessHours = True
							Exit Select
						End If
					Case "Monday"
						If myRegistry.ReadFromRegistry("BusinessHoursMonday") = True Then
							InBusinessHours = True
							Exit Select
						End If
					Case "Tuesday"
						If myRegistry.ReadFromRegistry("BusinessHoursTuesday") = True Then
							InBusinessHours = True
							Exit Select
						End If
					Case "Wednesday"
						If myRegistry.ReadFromRegistry("BusinessHoursWednesday") = True Then
							InBusinessHours = True
							Exit Select
						End If
					Case "Thursday"
						If myRegistry.ReadFromRegistry("BusinessHoursThursday") = True Then
							InBusinessHours = True
							Exit Select
						End If
					Case "Friday"
						If myRegistry.ReadFromRegistry("BusinessHoursFriday") = True Then
							InBusinessHours = True
							Exit Select
						End If
					Case "Saturday"
						If myRegistry.ReadFromRegistry("BusinessHoursSaturday") = True Then
							InBusinessHours = True
							Exit Select
						End If
					Case "Sunday"
						If myRegistry.ReadFromRegistry("BusinessHoursSunday") = True Then
							InBusinessHours = True
							Exit Select
						End If
				End Select
			End If

			myRegistry = Nothing
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Error in Business Hours module: " & ex.Message)
			myRegistry = Nothing
		End Try

		Return Not (InBusinessHours)
	End Function

#End Region

	Private Sub KillNotes()

		Try
			WriteAuditEntry(Now.ToString & " Killing all Notes processes.")
			Dim myApp As String = strAppPath & "stopnotescl.exe /q"
			Shell(myApp, AppWinStyle.Hide, False)
		Catch ex As Exception

		End Try

	End Sub


#Region "Week Number"
    '11/5/2015 NS added for VSPLUS-2085
    Private Function GetWeekNumberNS(ByVal fromDate As DateTime) As Integer
        Dim cal As Calendar = CultureInfo.InvariantCulture.Calendar
        Dim day As Integer
        day = cal.GetDayOfWeek(fromDate)
        If (day >= DayOfWeek.Monday And day <= DayOfWeek.Wednesday) Then
            fromDate = fromDate.AddDays(3)
        End If
        Return cal.GetWeekOfYear(fromDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
    End Function

    Private Function GetWeekNumber(ByVal dtNow As DateTime) As Integer
        Return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday)
        
        'Dim year As Integer = dt.Year
        'Dim Dec29 As New DateTime(year, 12, 29)
        'Dim week1 As New DateTime
        '' Check that the date is or is after December 29.

        '' Check that the date is or is after December 29.
        'If (dt >= New DateTime(year, 12, 29)) Then
        '    week1 = GetWeekOneDate(year + 1)
        '    If (dt < week1) Then week1 = GetWeekOneDate(year)
        'Else
        '    week1 = GetWeekOneDate(year)
        '    If (dt < week1) Then week1 = GetWeekOneDate(--year)
        'End If

        'Return ((dt.Subtract(week1).Days / 7 + 1))
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
        MyBase.WriteAuditEntry(strMsg, "History.txt", LogLevelInput)
    End Sub

    Private Sub WriteConsoleCommandHistoryEntry(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
        MyBase.WriteAuditEntry(strMsg, "Console_Command_Log.txt", LogLevelInput)
    End Sub

    Private Overloads Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
        MyBase.WriteDeviceHistoryEntry(DeviceType, DeviceName, strMsg, LogLevelInput)
    End Sub



    '   Private Sub WriteAuditEntry(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
    '       If LogLevelInput >= MyLogLevel Then
    '           strAuditText += strMsg & vbCrLf
    '       End If
    '   End Sub

    'Private Sub WriteConsoleCommandHistoryEntry(ByVal strMsg As String)

    '	Try
    '		Dim appendMode As Boolean = True
    '		Dim sw As New System.IO.StreamWriter(strConsoleCommandLogDest, appendMode, System.Text.Encoding.Unicode)

    '		'Dim sw As StreamWriter
    '		'If File.Exists(strConsoleCommandLogDest) Then
    '		'    sw = New StreamWriter(strConsoleCommandLogDest, True)
    '		'Else
    '		'    sw = File.CreateText(strConsoleCommandLogDest)
    '		'End If
    '		Try
    '			sw.WriteLine(strMsg)
    '			sw.Close()

    '		Catch ex As Exception

    '		Finally
    '			sw = Nothing
    '			GC.Collect()
    '		End Try

    '	Catch ex As Exception

    '	End Try

    'End Sub


    '   Private Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
    '       Dim DeviceLogDestination As String
    '       Dim appendMode As Boolean = True
    '       'Depending on the log level, either write this out to the log or not
    '       '    If MyLogLevel = VitalSignsPlusDomino.LogLevel.Verbose Or (LogLevel = VitalSignsPlusDomino.LogLevel.Normal And MyLogLevel = VitalSignsPlusDomino.LogLevel.Normal) Then

    '       If (LogLevelInput < MyLogLevel) Then
    '           Return
    '       End If


    '       If InStr(DeviceName, "/") > 0 Then
    '           DeviceName = DeviceName.Replace("/", "_")
    '       End If


    '       Try
    '           DeviceLogDestination = AppDomain.CurrentDomain.BaseDirectory.ToString & "Log_Files\" & DeviceType & "_" & DeviceName & "_Log.txt"
    '       Catch ex As Exception

    '       End Try

    '       Try
    '           If InStr(DeviceName, "stme") Then
    '               If InStr(DeviceLogDestination, "http://") > 0 Then
    '                   DeviceLogDestination = DeviceLogDestination.Replace("http://", "")
    '               End If

    '               If InStr(DeviceLogDestination, ":8088") > 0 Then
    '                   DeviceLogDestination = DeviceLogDestination.Replace(":8088", "")
    '               End If
    '               ' WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " Computed log file name as  " & DeviceLogDestination)
    '           End If
    '       Catch ex As Exception
    '           ' WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " Exception Computing log file name as  " & ex.ToString)
    '       End Try


    '       Try
    '           Dim sw As New StreamWriter(DeviceLogDestination, appendMode, System.Text.Encoding.Unicode)
    '           sw.WriteLine(strMsg)
    '           sw.Close()
    '           sw = Nothing
    '       Catch ex As Exception

    '       End Try
    '       GC.Collect()
    ''    End If


    '   End Sub


    '   Protected Sub WriteAuditEntries()

    '       ' Dim myRegistry As New RegistryHandler
    '       Dim strLogDest As String = AppDomain.CurrentDomain.BaseDirectory.ToString & "\Log_Files\History.txt"


    '       'Try
    '       '    ' strLogDest = myRegistry.ReadFromRegistry("History Path")
    '       '    If File.Exists(strLogDest) Then
    '       '        File.Delete(strLogDest)
    '       '    End If
    '       'Catch ex As Exception
    '       '    strLogDest = "c:\vitalsignslog.txt"

    '       'End Try

    '       ' myRegistry = Nothing

    '       'Dim elapsed As TimeSpan
    '       Dim OneHour As Integer = 60 * 60 * -1  'seconds
    '       Dim dtAuditHistory As DateTime

    '       Dim myLastUpdate As TimeSpan

    '       Try
    '           dtAuditHistory = Now
    '           Thread.CurrentThread.Sleep(1000)
    '       Catch ex As Exception

    '       End Try


    '       Try

    '           Do
    '               Dim sw As StreamWriter
    '               Try
    '                   If File.Exists(strLogDest) Then
    '                       sw = New StreamWriter(strLogDest, True, System.Text.Encoding.Unicode)
    '                   Else
    '                       sw = New StreamWriter(strLogDest, False, System.Text.Encoding.Unicode)
    '                       '  sw = File.CreateText(strLogDest)
    '                   End If
    '               Catch ex As Exception

    '               End Try


    '               Try
    '                   If strAuditText <> "" Then

    '                       sw.WriteLine(strAuditText)
    '                       strAuditText = ""
    '                       sw.Flush()
    '                       sw.Close()
    '                   End If

    '               Catch ex As Exception

    '               End Try

    '               Try
    '                   sw.Close()
    '               Catch ex As Exception

    '               End Try
    '               sw = Nothing

    '               Thread.CurrentThread.Sleep(4250)

    '               Try
    '                   myLastUpdate = dtAuditHistory.Subtract(Now)
    '               Catch ExAbort As ThreadAbortException

    '               Catch ex As Exception

    '               End Try


    '           Loop

    '       Catch ExAbort As ThreadAbortException
    '           '      sw.Write(" Shutting down, closing log")
    '           '     sw.Close()
    '           '    sw = Nothing
    '       Catch ex As Exception

    '       End Try


    '   End Sub
#End Region


    Public Class TravelerDeviceSummary
        Public Name As String
        Public Count As Double
    End Class
    Public Class TravelerDeviceCollection
        Inherits System.Collections.CollectionBase

        Public ReadOnly Property Item(ByVal index As Integer) As TravelerDeviceSummary
            Get
                ' The appropriate item is retrieved from the List object and 
                ' explicitly cast to the Template type, then returned to the 
                ' caller.
                Return CType(List.Item(index), TravelerDeviceSummary)
            End Get
        End Property

        Public Function HasItem(ByVal Name As String) As Boolean
            Dim n As Integer
            Dim myTravelerDevice As TravelerDeviceSummary
            For n = 0 To Me.Count - 1
                myTravelerDevice = Item(n)
                If myTravelerDevice.Name = Name Then Return True
            Next
            Return False
        End Function

        Public Function FindItem(ByVal Name As String) As TravelerDeviceSummary
            Dim n As Integer
            Dim myTravelerDevice As TravelerDeviceSummary
            For n = 0 To Me.Count - 1
                myTravelerDevice = Item(n)
                If myTravelerDevice.Name = Name Then Return myTravelerDevice
            Next
            Return Nothing
        End Function

        Public Sub Add(ByVal aTravelerDevice As TravelerDeviceSummary)
            ' Invokes Add method of the List object to add a template.
            List.Add(aTravelerDevice)
        End Sub

    End Class


    ' Contains dll functions.
    ' These could be declared at the top of the code (outside of a class) but this seems to be the preferred way for VB.NET
    Class dll
        Declare Function InitNotes Lib "ServerStats.dll" Alias "InitNotes" (ByVal s1 As String, ByVal s2 As String, ByVal s3 As String, ByVal s4 As String) As IntPtr
        Declare Function GetServerStat Lib "ServerStats.dll" Alias "GetServerStat" (ByVal s1 As String, ByVal s2 As String, ByVal s3 As String) As IntPtr
        Declare Function ResponseTime Lib "ServerStats.dll" Alias "ResponseTime" (ByVal s1 As String) As IntPtr
        Declare Sub W32_NotesTerm Lib "nnotes" Alias "NotesTerm" ()
        Declare Sub W32_NotesInitThread Lib "nnotes" Alias "NotesInitThread" ()
        Declare Sub W32_NotesTermThread Lib "nnotes" Alias "NotesTermThread" ()
    End Class

#Region " C API stuff"

    ' Get Statistics from Domino Server.
    ' If sStat$ is blank then all stats are returned for the specified Facility
    'Function GetStats(ByVal sServer$, ByVal sFacility$, ByVal sStat$) As String
    '    ' Wait until it is safe to enter.

    '        ' WriteAuditEntry(Now.ToString & " Attempting to get stats using C API")
    '        Dim sStartStatus As String
    '        GetStats = "*ERROR* Unknown error"                                       ' assume failure

    '        Try
    '        If InitNotes(sStartStatus) Then
    '            If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
    '            ' if Notes initialized ok then
    '            Dim ptr As IntPtr = dll.GetServerStat(sServer$, sFacility$, sStat$)     ' Get stats from server
    '            GetStats = FormatResults(Marshal.PtrToStringUni(ptr))                                   ' Get string returned from dll and marshall into .NET memory space.
    '            Marshal.FreeBSTR(ptr)
    '            If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
    '            ' Free memory allocated in dll
    '        Else
    '            GetStats = "*ERROR*" + sStartStatus
    '            WriteAuditEntry(Now.ToString & " ***** IMPORTANT " & ProductName & " is having problems accessing the Notes.ID file.  Please check Preferences, Domino Settings, Notes ID File.    ******* IMPORTANT")
    '        End If

    '        If InStr(GetStats, "*ERROR*") Then
    '            WriteAuditEntry(Now.ToString & " ***** ERROR GETTING STATS with ERROR Message = " & GetStats, LogLevel.Verbose)
    '        End If
    '    Catch e As Exception
    '        GetStats = "*ERROR*" + e.ToString()                         ' return error string to caller
    '    Finally
    '        dll.W32_NotesTerm() ' always terminate Notes session
    '    End Try


    '    Try
    '        If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
    '    Catch ex As Exception

    '    End Try

    'End Function


    Function GetStats(ByVal sServer$, ByVal sFacility$, ByVal sStat$) As String
        ' Wait until it is safe to enter.

        ' WriteAuditEntry(Now.ToString & " Attempting to get stats using C API")

        GetStats = "*ERROR* Unknown error"                                       ' assume failure

        Try
            Dim ptr As IntPtr = dll.GetServerStat(sServer$, sFacility$, sStat$)     ' Get stats from server
            GetStats = FormatResults(Marshal.PtrToStringUni(ptr))                                   ' Get string returned from dll and marshall into .NET memory space.
            Marshal.FreeBSTR(ptr)
            If InStr(GetStats, "*ERROR*") Then
                WriteAuditEntry(Now.ToString & " ***** ERROR GETTING STATS with ERROR Message = " & GetStats, LogLevel.Verbose)
            End If
        Catch e As Exception
            GetStats = "*ERROR*" + e.ToString()                         ' return error string to caller

        End Try
    End Function

    ' Start Notes runtime environment
    Function InitNotes(ByRef sStartStatus As String) As Boolean

        InitNotes = False               ' assume failure

        ' Initialize Notes and get status
        ' The InitNotes function calls the C API function NotesInitExtended.
        ' This function can optionally take two parameters, ProgDir and Notes.ini
        ' Whether the function works without these values depends on:
        ' A) which dir the program is running in
        ' B) whether or not the Notes program file directory is in the Windows PATH environment variable
        '
        'WriteAuditEntry(Now.ToString & " INI Notes is using Prog dir in " & mNotesProgDir)
        'WriteAuditEntry(Now.ToString & " INI Notes is using User ID in " & mUserId)
        'WriteAuditEntry(Now.ToString & " INI Notes is using INI in " & mNotesINI)

        Try
            Dim ptrStart As IntPtr = dll.InitNotes(mNotesProgDir, mNotesINI, mUserId, MyDominoPassword)    ' Call dll routine to init Notes environment
            sStartStatus = Marshal.PtrToStringUni(ptrStart) ' Get string returned from dll and marshall into .NET memory space.
            '  WriteAuditEntry(Now.ToString & " C API says " & sStartStatus)
            Marshal.FreeBSTR(ptrStart)                                  ' Free memory allocated in dll
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Exception initializing Notes for C API " & ex.ToString)
        End Try

        If (sStartStatus = "*SUCCESS*") Then                        ' If Notes started ok then 
            InitNotes = True
            ' WriteAuditEntry(Now.ToString & " C API was Initialized")
            ' set return value to True
        End If
        ' WriteAuditEntry(Now.ToString & " C API status is " & InitNotes)



    End Function

    ' Get latency from Domino server
    Function GetResponseTime(ByVal sServer$) As String
        If Trim(sServer$) = "" Then
            GetResponseTime = 0
            Exit Function
        End If


        Dim sStartStatus As String
        GetResponseTime = "*ERROR* Unknown error"                        ' assume failure

        Try
            Dim ptr As IntPtr = dll.ResponseTime(sServer$)          ' get server latency
            GetResponseTime = Marshal.PtrToStringUni(ptr)           ' Get string returned from dll and marshall into .NET memory space.
            Marshal.FreeBSTR(ptr)                                   ' Free memory allocated in dll      
        Catch e As Exception
            GetResponseTime = "*ERROR*" + e.ToString()      ' return error string to caller
            WriteDeviceHistoryEntry("Domino", sServer$, Now.ToString & " Error getting response time:  " & e.ToString())

        End Try

    End Function

    'This function was commented out as a backup, new function does not call InitNotes

    ' Get latency from Domino server
    'Function GetResponseTime(ByVal sServer$) As String
    '    If Trim(sServer$) = "" Then
    '        GetResponseTime = 0
    '        Exit Function
    '    End If


    '    Dim sStartStatus As String
    '    GetResponseTime = "*ERROR* Unknown error"                        ' assume failure

    '    Try
    '        WriteDeviceHistoryEntry("Domino", sServer$, Now.ToString & " Attempting to initialize Notes. ", LogLevel.Verbose)
    '        If InitNotes(sStartStatus) Then                             ' if Notes initialized ok then
    '            WriteDeviceHistoryEntry("Domino", sServer$, Now.ToString & " Notes initialized. ")
    '            'If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.WaitOne()
    '            Dim ptr As IntPtr = dll.ResponseTime(sServer$)          ' get server latency
    '            GetResponseTime = Marshal.PtrToStringUni(ptr)           ' Get string returned from dll and marshall into .NET memory space.
    '            Marshal.FreeBSTR(ptr)                                   ' Free memory allocated in dll
    '            'If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()

    '        Else                                                        ' Otherwise
    '            GetResponseTime = "*ERROR*" + sStartStatus                       ' return error
    '        End If

    '    Catch e As Exception
    '        GetResponseTime = "*ERROR*" + e.ToString()      ' return error string to caller
    '        WriteDeviceHistoryEntry("Domino", sServer$, Now.ToString & " Error getting response time:  " & e.ToString())
    '    Finally
    '        dll.W32_NotesTerm()                         ' always terminate Notes session
    '    End Try

    '    Try
    '        If Use_NotesAPI_Mutex = True Then NotesAPI_Mutex.ReleaseMutex()
    '    Catch ex As Exception

    '    End Try

    'End Function


    Private Function FormatResults(ByVal pOrigResults As String) As String
        Try
            ' Replace tab chars with "="
            Dim sFormattedResults$ = pOrigResults.Replace(ControlChars.Tab, "=")
            ' Replace Linefeed char with Newline
            sFormattedResults$ = sFormattedResults$.Replace(ControlChars.Lf, ControlChars.NewLine)
            ' Display formatted results in the dialog
            Return sFormattedResults
        Catch ex As Exception
            Return ""
        End Try

    End Function

#End Region

#Region "Hourly and Daily"

    Public Sub HourlyTasks()

        WriteAuditEntry(Now.ToString & " Beginning to perform Hourly Tasks.")

        Dim myRegistry As New VSFramework.RegistryHandler

        Try

            WriteAuditEntry(Now.ToString & " Beginning to perform Hourly Tasks for Domino Servers.", LogLevel.Verbose)
            Dim Server As MonitoredItems.DominoServer
            For Each Server In MyDominoServers
                If Server.Enabled = True Then
                    WriteAuditEntry(Now.ToString & " Domino server " & Server.Name & " availability percentage is " & Server.UpPercentCount, LogLevel.Verbose)
                    '   WriteAuditEntry(Now.ToString & " Domino server " & Server.Name & " availability percentage is " & Server.UpPercentCount)
                    Try

                        'RecordCountAvailability("Domino", Server.Name, Server.UpPercentCount * 100)
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error recording count availability: " & ex.Message)
                    End Try

                    Try
                        'RecordTimeAvailability("Domino", Server.Name, Server.UpPercentMinutes * 100)
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error recording time availability: " & ex.Message)
                    End Try

                    Try
                        'RecordDownTime("Domino", Server.Name, Server.DownMinutes)
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error recording down minutes: " & ex.Message)
                    End Try

                    Try
                        'RecordOnTargetAvailability("Domino", Server.Name, Server.OnTargetPercent * 100)
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error recording hourly uptime: " & ex.Message)
                    End Try

                    Try
                        If Server.BusinessHoursOnTargetPercent <> 0 Then
                            'RecordBusinessHoursOnTargetAvailability("Domino", Server.Name, Server.BusinessHoursOnTargetPercent * 100)
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error recording OnTarget hourly uptime: " & ex.Message)
                    End Try


                    Try
                        Server.ResetUpandDownCounts()
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error resetting up and down counts: " & ex.Message)
                    End Try

                End If
            Next
            Server = Nothing

            WriteAuditEntry(Now.ToString & " Updated Hourly Tasks for Domino servers.")

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error processing Domino hourly tasks: " & ex.Message)
        End Try

        'Try
        '    For Each URL In MyURLs
        '        If URL.Enabled = True Then
        '            Try
        '                RecordTimeAvailability("URL", URL.Name, URL.UpPercentMinutes * 100)
        '            Catch ex As Exception
        '                WriteAuditEntry(Now.ToString & " Error recording time availability: " & ex.Message)
        '            End Try
        '        End If

        '    Next
        'Catch ex As Exception

        'End Try

        Try
            If CType(myRegistry.ReadFromRegistry("MonitorNotesMail"), Boolean) = True Then
                Dim NMProbe As MonitoredItems.DominoMailProbe
                For Each NMProbe In MyNotesMailProbes
                    If NMProbe.Enabled = True Then
                        '   WriteAuditEntry(Now.ToString & " BlackBerry " & BB.Name & " availability percentage is " & BB.UpPercentCount)
                        'RecordCountAvailability("NotesMail Probe", NMProbe.Name, NMProbe.UpPercentCount * 100)
                        '     RecordTimeAvailability("NotesMail Probe", NMProbe.Name, NMProbe.UpPercentMinutes * 100)
                        'RecordDownTime("NotesMail Probe", NMProbe.Name, NMProbe.DownMinutes)
                        NMProbe.ResetUpandDownCounts()
                    End If
                Next
                NMProbe = Nothing
            End If
        Catch ex As Exception

        End Try

        WriteAuditEntry(Now.ToString & " Updated Hourly Tasks for NotesMail probes.", LogLevel.Verbose)

        Try
            If CType(myRegistry.ReadFromRegistry("MonitorNotesDatabases"), Boolean) = True Then
                Dim NDB As MonitoredItems.NotesDatabase
                For Each NDB In MyNotesDatabases
                    If NDB.Enabled = True And NDB.TriggerType = "Database Response Time" Then
                        '   WriteAuditEntry(Now.ToString & " Notes Database " & NDB.Name & " availability percentage is " & NDB.UpPercentCount)
                        'RecordCountAvailability("Notes Database", NDB.Name, NDB.UpPercentCount * 100)
                        '  RecordTimeAvailability("Notes Database", NDB.Name, NDB.UpPercentMinutes * 100)
                        'RecordDownTime("Notes Database", NDB.Name, NDB.DownMinutes)
                        NDB.ResetUpandDownCounts()
                    End If
                Next
                NDB = Nothing
            End If
        Catch ex As Exception

        End Try


        Try
            'write it when you are done
            myRegistry.WriteToRegistry("Hourly Tasks", Now.Hour)
        Catch ex As Exception

        End Try

        Try
            WriteAuditEntry(Now.ToString & " Updated Hourly Tasks registry setting.", LogLevel.Verbose)
        Catch ex As Exception

        End Try

        Try
            myRegistry = Nothing
            GC.Collect()
        Catch ex As Exception

        End Try

        WriteAuditEntry(Now.ToString & " Finished Hourly Tasks.")

    End Sub


    Protected Sub PerformHourlyDailyTasks()
        'performs operations that need to be done hourly or daily, such as summarize statistics, and look at mail files again
        Dim myRegistry As New VSFramework.RegistryHandler
        Dim LastDate, MyDate As DateTime
        Dim Hour As Integer

        Dim DominoServer As MonitoredItems.DominoServer

        Dim NMP As MonitoredItems.DominoMailProbe

        Dim NDB As MonitoredItems.NotesDatabase
        Dim Q As MonitoredItems.BlackBerryQueue


        Do While True
            Thread.Sleep(60 * 1000) 'Sleep a minute

            'Figure Out if We have shifted to off hours

            Try

                For Each NDB In MyNotesDatabases
                    NDB.OffHours = OffHours(NDB.Name)
                Next
                For Each DominoServer In MyDominoServers
                    DominoServer.OffHours = OffHours(DominoServer.Name)
                Next

                For Each NMP In MyNotesMailProbes
                    '1/22/2016 NS modified for VSPLUS-2332
                    'NMP.OffHours = OffHours(NMP.Name)
                    NMP.OffHours = OffHours(NMP.SourceServer)
                Next


            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error in Daily Tasks thread setting off hours: " & ex.Message)
            End Try
            WriteAuditEntry(Now.ToString & " *********** Evaluating Hourly Tasks", LogLevel.Verbose)

            'Figure out if hourly tasks are due
            Try
                Hour = CType(myRegistry.ReadFromRegistry("Hourly Tasks"), Integer)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Exception determining the most recent hourly tasks performed: " & ex.ToString)
                Hour = -1
            End Try


            Try
                If Now.Hour <> Hour Then
                    WriteAuditEntry(Now.ToString & " Calling Hourly Tasks")
                    HourlyTasks()
                Else
                    WriteAuditEntry(Now.ToString & " The current hour is " & Hour & ". Hourly tasks were performed for this hour: " & Hour, LogLevel.Verbose)
                End If
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error in Daily Tasks thread starting hourly tasks: " & ex.Message)
            End Try



        Loop
    End Sub

#End Region

    Public Function GetNotesPassword() As String
        'Get the passwords from the settings table
        Dim MyPass As Object
        Dim Password As String

        Try
            Dim myAdapter As New VSFramework.XMLOperation
            MyPass = myAdapter.ReadSettingsSQL("Password")
            WriteAuditEntry(Now.ToString & " Password type is " & MyPass.GetType.ToString)
            'WriteAuditEntry(Now.ToString & " Raw password is " & MyPass.ToString)
        Catch ex As Exception
            MyPass = Nothing
        End Try

        Dim mySecrets As New VSFramework.TripleDES
        Try
            If Not MyPass Is Nothing Then
                Password = mySecrets.Decrypt(MyPass) 'password in clear text, stored in memory now
                ' If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Successfully decrypted the Notes password as " & MyDominoPassword)
            Else
                Password = Nothing
            End If
        Catch ex As Exception
            Password = ""
            WriteAuditEntry(Now.ToString & " Error decrypting the Notes password.  " & ex.ToString)
        End Try

        Return Password
    End Function

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
                WriteAuditEntry(Now.ToString & "********* Thread Status Report ********")
                WriteAuditEntry(Now.ToString & " Hourly tasks thread: " & ThreadHourlyDaily.ThreadState.ToString)
                ' WriteAuditEntry(Now.ToString & " URL Monitoring:  " & threadMonitorURLs.ThreadState.ToString)
                Dim i As Integer = 1
                For Each t As Threading.Thread In ListOfDominoThreads
                    WriteAuditEntry(Now.ToString & " Domino monitoring thread #" & i.ToString & " state = " & t.ThreadState.ToString)
                    i += 1
                Next

                'i = 1
                'For Each t As Threading.Thread In ListOfURLThreads
                'WriteAuditEntry(Now.ToString & " URL monitoring thread #" & i.ToString & " state = " & t.ThreadState.ToString)
                'i += 1
                'Next
                WriteAuditEntry(Now.ToString & " Traveler Users thread: " & ThreadTravelerUsers.ThreadState.ToString)
                '   WriteAuditEntry(Now.ToString & " VitalStatus thread: " & ThreadVitalStatus.ThreadState.ToString)
                WriteAuditEntry(Now.ToString & vbCrLf)
            Catch ex As Exception

            End Try



            Try
                Thread.Sleep(SleepTime)
            Catch ex As Exception

            End Try

            Try
                myRegistry.WriteToRegistry("Service Status", Now.ToString)
            Catch ex As Exception

            End Try

        Loop
        myRegistry = Nothing
    End Sub


End Class
