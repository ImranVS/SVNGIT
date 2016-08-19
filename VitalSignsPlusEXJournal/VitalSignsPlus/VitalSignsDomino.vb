
'Also use IP*Works components, see http://www.ipworks.com
' .NET Reactor  www.ezriz.com

'Written by Alan Forbes
'64 Pleasant Street
'Rowley MA 01969

'Alan.Forbes@ServerVitalSigns.com
'mzldad@yahoo.com
'Phone: 781-608-4060
'Copyright 2013, All Rights Reserved

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
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Configuration
Imports System.Threading
Imports MonitoredItems.MonitoredDevice.Alert
'Imports System.ComponentModel
'Imports System.Windows.Forms
Imports System.Net
Imports System.Net.Sockets
Imports System.Xml
Imports System.Globalization
Imports VSFramework
Imports RPRWyatt.VitalSigns.Services


Public Class VitalSignsPlusExJournal
    Inherits VSServices

    Dim connectionString As String

	Dim BuildNumber As Integer = 2280
	Dim ProductName As String 'value set in start up 
	Dim CompanyName As String = "JNIT, Inc.  dba RPR Wyatt"
	Dim ListOfDominoThreads As List(Of Threading.Thread) = New List(Of Threading.Thread)
	Dim myThreshold As Integer = 1000
	Dim sCultureString As String = "en-US"
	Dim connectionStringName As String = "CultureString"
	'Used by C API
	Dim mNotesProgDir As String	' = "c:\program files\lotus\notes"
	Dim mNotesINI As String	' = "=c:\program files\lotus\notes\notes.ini"
	Dim mUserId As String '= "C:\Program Files\lotus\notes\data\aforbesMZL.id"
	Private Shared DominoSelector_Mutex As New Mutex()

	Dim NotesSession As New Domino.NotesSession

	'  Dim WithEvents PingControl As New nsoftware.IPWorks.Ping("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000")
	Dim myAlert As New AlertLibrary.Alertdll
	Dim boolTimeToStop As Boolean = False

	' Monitor log file & other paths
	Private strLogDest As String
	Private strAppPath As String
	Dim strAuditText As String

	'Store the passwords in these variables
	Dim MyDominoPassword As String

	Dim BannerText As String

	'Collection of Domino servers to monitor
	Dim MyDominoServers As New MonitoredItems.DominoCollection
	Dim MyDominoServer As MonitoredItems.DominoServer


	Dim BoolOffHours As Boolean


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

        Try
            strAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
        Catch ex As Exception
            strAppPath = "c:\"
        End Try


        'Read some settings from the registry
        Dim myRegistry As New VSFramework.RegistryHandler()

        SetMyThresholdValue()

        Try
            MyLogLevel = myRegistry.ReadFromRegistry("Log Level")
        Catch ex As Exception
            MyLogLevel = LogLevel.Verbose
        End Try



        Try
            WriteAuditEntry(Now.ToString + " ")
            WriteAuditEntry(Now.ToString + " ********************************* ")
            WriteAuditEntry(Now.ToString + " VitalSigns Plus EXJournal Scanner - Build Number: " & BuildNumber)
            WriteAuditEntry(Now.ToString + " The service is starting up.")
            WriteAuditEntry(Now.ToString + " ")
            WriteAuditEntry(Now.ToString + " Copyright 2006 - " & Now.Year & ", JNITH, Inc. and Plum Island Publishing, LLC.")
            WriteAuditEntry(Now.ToString + " All rights reserved.")
            WriteAuditEntry(Now.ToString + " Distributed worldwide by RPR Wyatt and its partner network.")
            WriteAuditEntry(Now.ToString + " ")
            WriteAuditEntry(Now.ToString + " ")
            ' Thread.CurrentThread.Sleep(4000)

        Catch ex As Exception

        End Try


        Try
            Dim myAdapter As New VSFramework.XMLOperation
            Dim MyConnectionString As String
            MyConnectionString = myAdapter.GetDBConnectionString("VitalSigns")
            WriteAuditEntry(Now.ToString + " My connection string is " & MyConnectionString)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString + " Exception getting connection string: " & ex.ToString)
        End Try

        '5/5/2016 NS commented out the code below since it is outdated and produces an error in SQL
        'Try
        '	Dim VSObject As New VSFramework.VSAdaptor
        '	Dim strSQL As String = "ALTER TABLE Status ADD ExJournalDate DateTime"
        '	VSObject.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
        'Catch ex As Exception

        'End Try

        Try
            mNotesProgDir = myRegistry.ReadFromRegistry("Notes Program Directory")
            If mNotesProgDir = "" Then
                WriteAuditEntry(Now.ToString & " *** WARNING *** The Notes Program Directory has to be set in File, Preferences.")
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
                WriteAuditEntry(Now.ToString & "  *** WARNING *** The Notes.INI location has to be set in File, Preferences.")
            Else
                WriteAuditEntry(Now.ToString & " The Notes.INI setting is " & mNotesINI)
            End If
        Catch ex As Exception

        End Try

        Try
            mUserId = myRegistry.ReadFromRegistry("Notes User ID")
            If mUserId = "" Then
                WriteAuditEntry(Now.ToString & "  *** WARNING ***  The Notes User ID file location has to be set in File, Preferences.")
                ' End
            Else
                WriteAuditEntry(Now.ToString & " The Notes User ID setting is " & mUserId)
            End If
        Catch ex As Exception

        End Try

        'Get the passwords from the registry
        Dim MyPass As Object

        Try
            ' MyPass = myRegistry.ReadFromRegistry("Password")  'Domino password as encrypted byte stream
            Dim myAdapter As New VSFramework.XMLOperation
            MyPass = myAdapter.ReadSettingsSQL("Password")

        Catch ex As Exception
            MyPass = Nothing
        End Try

        Dim mySecrets As New VSFramework.TripleDES
        Try
            If Not MyPass Is Nothing Then
                MyDominoPassword = mySecrets.Decrypt(MyPass) 'password in clear text, stored in memory now
                '  If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Successfully decrypted the Notes password as " & MyDominoPassword)

            Else
                MyDominoPassword = Nothing
            End If
        Catch ex As Exception
            MyDominoPassword = ""
            If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Error decrypting the Notes password.  " & ex.ToString)
        End Try

        Try
            NotesSession.Initialize(MyDominoPassword)
            WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Initialized NotesSession for " & NotesSession.CommonUserName)
        Catch ex As Exception
            System.Runtime.InteropServices.Marshal.ReleaseComObject(NotesSession)
            If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Error Initializing NotesSession.  Many problems will follow....  " & ex.ToString)
            WriteAuditEntry(Now.ToString & " *********** ERROR ************  Error initializing a session to Query Domino server. ")
            '  Exit Sub
        End Try


        Try
            CreateDominoServersCollection()
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
            WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " All server settings have been read.  Starting EXJournal monitoring.")
            ' Thread.CurrentThread.Sleep(2000)
            StartThreads()
        Catch ex As Exception

        End Try



    End Sub

	Protected Overrides Sub OnStop()
		' Add code here to perform any tear-down necessary to stop your service.
		boolTimeToStop = True
		Try
			System.Runtime.InteropServices.Marshal.ReleaseComObject(NotesSession)
		Catch ex As Exception

		End Try
		MyBase.OnStop()
		'End
	End Sub


	Protected Sub ThreadChecker()
		'watches over the threads
		Dim SleepTime As Integer
		SleepTime = 60 * 1000 * 1  '1 minutes worth of milliseconds; sleeps 3 minutes after doing its work
		Static Restart As Boolean = False

		Dim myRegistry As New VSFramework.RegistryHandler

		Do While True


			Try
				If MyLogLevel = LogLevel.Verbose Then
					WriteDeviceHistoryEntry("All", "ExJournal_Threads", Now.ToString & "********* Thread Status Report ********")
					' WriteAuditEntry(Now.ToString & " URL Monitoring:  " & threadMonitorURLs.ThreadState.ToString)
					Dim i As Integer = 1
					For Each t As Threading.Thread In ListOfDominoThreads
						WriteDeviceHistoryEntry("All", "ExJournal_Threads", Now.ToString & " EXJournal monitoring thread #" & i.ToString & " state = " & t.ThreadState.ToString)
						i += 1
					Next

					i = 1

					WriteDeviceHistoryEntry("All", "ExJournal_Threads", vbCrLf)
				End If
			Catch ex As Exception

			End Try



			Try
				Thread.Sleep(SleepTime)
			Catch ex As Exception

			End Try

		Loop

	End Sub


	Protected Sub StartThreads()

		Dim myThreads As Threading.Thread = New Threading.Thread(AddressOf ThreadChecker)
		myThreads.Start()

		StartMonitoringThreads()

		Try
			Dim MonitorCoreTableChanges As New Thread(AddressOf CheckForCoreTableChanges)
			MonitorCoreTableChanges.CurrentCulture = New CultureInfo(sCultureString)
			MonitorCoreTableChanges.Start()
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Error starting thread to check for table updates")
		End Try

	End Sub
    Dim dominoThreadCount As Integer = 0
    Dim initialDominoThreadCount As Integer = 0
    Dim AliveDominoMainThreads As System.Collections.ArrayList = New System.Collections.ArrayList()
    Protected Sub StartMonitoringThreads()
        Dim EnabledCount As Integer = 0
        Dim ServerOne As MonitoredItems.DominoServer
        For n = 0 To MyDominoServers.Count - 1
            ServerOne = MyDominoServers.Item(n)
            '6/18/2015 NS modified for VSPLUS-1802
            If ServerOne.Enabled = True Then
                If ServerOne.EXJEnabled = True Then
                    EnabledCount += 1
                End If
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


        WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " There are " & EnabledCount & " enabled Domino servers with enabled EXJournal scan option available to scan.")
        ' WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " I am launching " & (intThreadCount - ListOfDominoThreads.Count) & " new threads for " & intThreadCount & " total threads to scan these servers.")

        Try
            For n = startThreads To dominoThreadCount
                Dim tTemp As Threading.Thread = New Threading.Thread(AddressOf MonitorDomino)
                tTemp.Start()
                AliveDominoMainThreads.Add(tTemp)
                Threading.Thread.Sleep(2000)  'sleep 2 seconds then start another thread
            Next
        Catch ex As Exception
            If InStr(ex.ToString, "System.OutOfMemoryException") > 0 Then
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " VitalSigns is out of memory.  Attempting to exit so the Master service will restart it. ")
                Thread.Sleep(1000)
                End
            Else
                WriteDeviceHistoryEntry("All", "ExJournal", Now.ToString & " Error starting MonitorDomino thread " & ex.ToString)
            End If

        End Try

    End Sub
    Public Function getThreadCount(ServerType As String) As Integer

        Dim myRegistry As New VSFramework.XMLOperation()
        Dim numOfThreads As Integer = 35
        Try
            numOfThreads = Convert.ToInt32(myRegistry.ReadSettingsSQL("ThreadLimit" & ServerType))
        Catch ex As Exception
            numOfThreads = 35
        End Try

        Return numOfThreads

    End Function
	Protected Sub SetMyThresholdValue()
		Try
			Dim myRegistry As New VSFramework.RegistryHandler()
			myThreshold = myRegistry.ReadFromRegistry("ExJournal Threshold")
		Catch ex As Exception
			'WS commented so it wont reset value if unreachable the 2nd time.  Default is already 1000
			'myThreshold = 1000
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

    Private Function GetWeekNumber(ByVal dt As DateTime) As Integer
        Dim year As Integer = dt.Year
        Dim Dec29 As New DateTime(year, 12, 29)
        Dim week1 As New DateTime
        ' Check that the date is or is after December 29.

        ' Check that the date is or is after December 29.
        If (dt >= New DateTime(year, 12, 29)) Then
            week1 = GetWeekOneDate(year + 1)
            If (dt < week1) Then week1 = GetWeekOneDate(year)
        Else
            week1 = GetWeekOneDate(year)
            If (dt < week1) Then week1 = GetWeekOneDate(--year)
        End If

        Return ((dt.Subtract(week1).Days / 7 + 1))
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



    Private Overloads Sub WriteAuditEntry(ByVal strMsg As String, Optional ByVal logLevel As LogLevel = LogLevel.Normal)
        MyBase.WriteAuditEntry(strMsg, "Ex_Journal_Log.txt", logLevel)
    End Sub

    Private Overloads Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String, Optional ByVal logLevel As LogLevel = LogLevel.Normal)
        MyBase.WriteDeviceHistoryEntry(DeviceType, DeviceName, strMsg, logLevel)
    End Sub


    'Private Sub WriteAuditEntry(ByVal strMsg As String)
    '    strAuditText += strMsg & vbCrLf
    'End Sub


    'Private Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String)
    '    Dim DeviceLogDestination As String
    '    Dim appendMode As Boolean = True
    '    '   If Left(strAppPath, 1) = "\" Then
    '    'DeviceLogDestination = strAppPath & "Data\Logfiles\" & DeviceType & "_" & DeviceName & "_Log.txt"
    '    '  Else
    '    '    DeviceLogDestination = strAppPath & "\Data\Logfiles\" & DeviceType & "_" & DeviceName & "_Log.txt"
    '    '    End If
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

#End Region



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
    Function GetStats(ByVal sServer$, ByVal sFacility$, ByVal sStat$) As String
        ' Wait until it is safe to enter.
        ' Release Mutex.

        ' WriteAuditEntry(Now.ToString & " Attempting to get stats using C API")
        Dim sStartStatus As String
        GetStats = "*ERROR* Unknown error"                                       ' assume failure

        Try
            If InitNotes(sStartStatus) Then                                         ' if Notes initialized ok then
                Dim ptr As IntPtr = dll.GetServerStat(sServer$, sFacility$, sStat$)     ' Get stats from server
                GetStats = FormatResults(Marshal.PtrToStringUni(ptr))                                   ' Get string returned from dll and marshall into .NET memory space.
                Marshal.FreeBSTR(ptr)                                                   ' Free memory allocated in dll
            Else
                GetStats = "*ERROR*" + sStartStatus
                WriteAuditEntry(Now.ToString & " ***** IMPORTANT " & ProductName & " is having problems accessing the Notes.ID file.  Please check Preferences, Domino Settings, Notes ID File.    ******* IMPORTANT")
            End If

        Catch e As Exception
            GetStats = "*ERROR*" + e.ToString()                         ' return error string to caller
        Finally
            dll.W32_NotesTerm() ' always terminate Notes session
            '  WriteAuditEntry(Now.ToString & "C API returned " & GetStats)

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
            WriteDeviceHistoryEntry("Domino", sServer$, Now.ToString & " Attempting to initialize Notes. ")
            If InitNotes(sStartStatus) Then                             ' if Notes initialized ok then
                WriteDeviceHistoryEntry("Domino", sServer$, Now.ToString & " Notes initialized. ")
                Dim ptr As IntPtr = dll.ResponseTime(sServer$)          ' get server latency
                GetResponseTime = Marshal.PtrToStringUni(ptr)           ' Get string returned from dll and marshall into .NET memory space.
                Marshal.FreeBSTR(ptr)                                   ' Free memory allocated in dll
            Else                                                        ' Otherwise
                GetResponseTime = "*ERROR*" + sStartStatus                       ' return error
            End If

        Catch e As Exception
            GetResponseTime = "*ERROR*" + e.ToString()      ' return error string to caller
            WriteDeviceHistoryEntry("Domino", sServer$, Now.ToString & " Error getting response time:  " & e.ToString())
        Finally
            dll.W32_NotesTerm()                         ' always terminate Notes session
        End Try

    End Function


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

    Protected Sub CheckForCoreTableChanges()


        Dim myRegistry As New VSFramework.RegistryHandler
        Dim flags As New MonitoredItems.ServicesFlags()
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
                    If flags.UpdateServiceCollection(MonitoredItems.ServerTypes.ExJournal, NodeName) Then
                        Try
                            '' MonitorDominoSettings()
                            WriteAuditEntry(Now.ToString & " Refreshing configuration of ExJournal servers")
                            CreateDominoServersCollection()

                            StartMonitoringThreads()

                            SetMyThresholdValue()

                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error Refreshing Sametime server settings on demand: " & ex.Message)
                        End Try
                    End If

                End If
            Catch ex As Exception

            End Try
            Thread.Sleep(2000)
        Loop
    End Sub



End Class
