Imports System.ServiceProcess
Imports System.Threading
Imports System.IO
Imports Microsoft.Win32
Imports Microsoft.Win32.Registry
Imports Ionic.Zip
Imports System.Configuration
Imports VSFramework
Imports System.Management
Imports System.Globalization
Imports System.Data.SqlClient
Imports RPRWyatt.VitalSigns.Services
Imports System.Linq

Imports MongoDB.Driver
Imports VSNext.Mongo.Entities
Imports System.Collections.Generic

Public Class VSMaster
    Inherits VSServices

#Region " Component Designer generated code "

    Public Sub New()
        MyBase.New()

        ' This call is required by the Component Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call

    End Sub

    'UserService overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    ' The main entry point for the process
    <MTAThread()> _
    Shared Sub Main()
        Dim ServicesToRun() As System.ServiceProcess.ServiceBase

        ' More than one NT Service may run within the same process. To add
        ' another service to this process, change the following line to
        ' create a second service object. For example,
        '
        '   ServicesToRun = New System.ServiceProcess.ServiceBase () {New Service1, New MySecondUserService}
        '
        ServicesToRun = New System.ServiceProcess.ServiceBase() {New VSMaster}

        System.ServiceProcess.ServiceBase.Run(ServicesToRun)
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    ' NOTE: The following procedure is required by the Component Designer
    ' It can be modified using the Component Designer.  
    ' Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        '
        'VSMaster
        '
        Me.ServiceName = "VitalSigns Master Service"

    End Sub

#End Region
    ' Monitor log file & other paths
    Private strLogDest As String
    Dim connectionString As String
    Dim strAuditText, strAppPath, myPath As String
    Dim sCultureString As String = "en-US"
    Dim connectionStringName As String = "CultureString"
    Dim threadDailyTasks As New Thread(AddressOf DailyTasks)
    Public VitalSignsPlusDomino As String = "VitalSignsPlusDomino"
    Public VitalSignsPlusCore As String = "VitalSignsPlusCore"
    Public VitalSignsPlusAlerting As String = "VitalSignsAlerts"
    Public VitalSignsPlusClusterHealth As String = "VitalSigns Cluster Health"
    Public VitalSignsDailyService As String = "VitalSigns Daily Tasks"
    Public MasterServiceName As String = "VitalSigns Plus Master Service"
    Public VitalSignsPlusDBHealthService As String = "VitalSigns Database Health"
    Public EXJournalServiceName As String = "VitalSigns EXJournal Scanner"
    Public VitalSignsConsoleCommands As String = "VSConsoleCommand"
    Public VitalSignsMicrosoft As String = "VitalSignsMicrosoft"
    Public VitalSignsCore64 As String = "VitalSignsCore64"
    Dim vsobj As New VSFramework.VSAdaptor
    Dim dominoKilled As DateTime = DateTime.Now

    Dim strDateFormat As String
    Dim objDateUtils As New DateUtils.DateUtils

    ' Create a socket to be used for listening for connections.
    Dim listenSocket As New Chilkat.Socket
    ' This gets set to True when the service is shutdown.  Threads
    ' will react to this and exit/abort appropriately.
    Dim m_stopRequested As Boolean = False

    ' The number of (extra) threads still running.
    Dim m_numActiveThreads As Integer = 0


    Dim boolEXJournal As Boolean = False
    Dim boolDominoConsoleCommands As Boolean = False
    Dim boolDominoService As Boolean = False
    Dim boolMicrosoftService As Boolean = False
    Dim boolCore64Service As Boolean = False
    Dim boolIsPrimary As Boolean = False
    '  Dim myOpenFile As String
    Dim BuildNumber As Integer = 61
    Dim TimeToStop As Boolean = False
    Dim myRegistry As New RegistryHandler

    Dim SendPulseThread As New Thread(AddressOf SendPulse)
    Dim NotRespondingThread As New Thread(AddressOf MonitorNotResponding)
    '4/10/2015 NS added for VSPLUS-1397
    Dim myAlert As New AlertLibrary.Alertdll

    Dim spaceString As String = "Insufficient Disk Space - cannot start VitalSigns Plus Master Service."

    Protected Overrides Sub ServiceOnStart(ByVal args() As String)
        Try
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings("VitalSignsMongo").ToString()
            WriteAuditEntry(Now.ToString + " connection string is " & connectionString)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString + " Error getting connection string. Error: " & ex.Message)
        End Try

        Dim strNodeName As String = ""
        Try
            strNodeName = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString
        Catch ex As Exception
            strNodeName = ""
        End Try

        Try
            sCultureString = ConfigurationManager.AppSettings(connectionStringName).ToString()
        Catch ex As Exception
            sCultureString = "en-US"
        End Try
        'force the current thread culture
        Thread.CurrentThread.CurrentCulture = New CultureInfo(sCultureString)

        Try
            ReadXmlFile(strNodeName)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in ReadXML: " & ex.Message)
        End Try



        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.

        WriteAuditEntry(Now.ToString & " The VitalSigns Plus Master Service is starting up... ")

        ' Check for the percentage free space available and if free space is less than 5% so not start the service
        Dim strDiskName As String
        strDiskName = System.IO.Path.GetPathRoot(System.Reflection.Assembly.GetExecutingAssembly.Location)
        'Get directory Details, Default disk is 'Disk C'
        Dim moHD As ManagementObject
        moHD = GetHDDetails(strDiskName.Replace(":\", ""))
        'Check for free space of 1GB
        If (Math.Round(Convert.ToDouble(moHD("FreeSpace")) / 1024 / 1024)) >= 500 Then
            myAlert.ResetSysMessage(spaceString)
            WriteAuditEntry(Now.ToString & " Free Disk Space " & Math.Round(Convert.ToDouble(moHD("FreeSpace")) / 1024 / 1024).ToString & "(Mb) Total Disk Space " & Math.Round(Convert.ToDouble(moHD("Size")) / 1024 / 1024 / 1024).ToString & "(Gb)")
            'Thread.Sleep(60000)
            Try
                'This settings table value is required by the "Get Assembly Info" page
                strAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location) & "\"
                myRegistry.WriteToRegistry("Log Files Path", strAppPath & "log_files\")
            Catch ex As Exception
                strAppPath = "c:\"
            End Try

            'Try
            '	Dim success As Boolean
            '	success = listenSocket.UnlockComponent("MZLDADSocket_OACwPK2ZlEn9")
            '	Dim port As Integer
            '	Dim backLog As Integer
            '	port = 8220
            '	backLog = 5
            '	success = listenSocket.BindAndListen(port, backLog)
            '	If (success <> True) Then
            '		listenSocket.SaveLastError(strAppPath & "log_files\lastError.xml")
            '		'Exit Sub
            '	Else
            '		WriteAuditEntry(Now.ToString & " The Master Service is listening for remote connections.")
            '	End If

            '	' Begin accepting connections...
            '	'Dim t As Thread
            '	't = New Thread(AddressOf Me.AcceptConnections)
            '	'm_numActiveThreads = m_numActiveThreads + 1
            '	't.CurrentCulture = New CultureInfo(sCultureString)
            '	't.Start()
            'Catch ex As Exception

            'End Try




            myRegistry.WriteToRegistry("Master Service Start", Now.ToShortDateString & " " & Now.ToShortTimeString)
            myRegistry.WriteToRegistry("Master Service Build Number", BuildNumber)

            ' KillNotes()

            WriteAuditEntry(Now.ToString & " The Master Service is starting the monitoring services. ")

            Try
                SendPulseThread.CurrentCulture = New CultureInfo(sCultureString)
                SendPulseThread.Start()
            Catch ex As Exception

            End Try

            Try
                NotRespondingThread.CurrentCulture = New CultureInfo(sCultureString)
                NotRespondingThread.Start()
            Catch ex As Exception

            End Try

            Thread.Sleep(5000)

            Try
                UpdateInstallLocation()
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error UpdatingInstallLocation " & VitalSignsCore64 & ": " & ex.Message)
            End Try

            SetBooleansOfServicesFromSelectedFeatures()

            Try
                StartService(VitalSignsPlusAlerting)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error starting " & VitalSignsPlusAlerting & ": " & ex.Message)
            End Try

            'Try
            '    StartService(VitalSignsPlusClusterHealth)
            'Catch ex As Exception
            '    WriteAuditEntry(Now.ToString & " Error starting " & VitalSignsPlusClusterHealth & ": " & ex.Message)
            'End Try

            Try
                If boolDominoService Then
                    StartService(VitalSignsPlusDomino)
                Else
                    WriteAuditEntry(Now.ToString & " Domino feature is not selected and the service will not be started.")
                End If

            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error starting " & VitalSignsPlusDomino & ": " & ex.Message)
            End Try

            Try
                If boolMicrosoftService Then
                    StartService(VitalSignsMicrosoft)
                Else
                    WriteAuditEntry(Now.ToString & " Microsoft features are not selected and the service will not be started.")
                End If
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error starting " & VitalSignsMicrosoft & ": " & ex.Message)
            End Try

            Try
                'See if the EXJournal Service needs to be started or not
                boolDominoConsoleCommands = myRegistry.ReadFromRegistry("Enable Domino Console Commands")
                If boolDominoConsoleCommands = True Then
                    WriteAuditEntry(Now.ToString & " Domino Console Commands are enabled.")
                    StartService(VitalSignsConsoleCommands)
                Else
                    WriteAuditEntry(Now.ToString & " Domino Console Commands are not enabled.")
                End If
            Catch ex As Exception
                boolDominoConsoleCommands = False
                WriteAuditEntry(Now.ToString & " Error starting " & VitalSignsConsoleCommands & ": " & ex.Message)
            End Try


            Try
                StartService(VitalSignsPlusCore)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error starting " & VitalSignsPlusCore & ": " & ex.Message)
            End Try

            Try
                'See if the EXJournal Service needs to be started or not
                boolEXJournal = myRegistry.ReadFromRegistry("Enable ExJournal")
                If boolEXJournal = True Then
                    WriteAuditEntry(Now.ToString & " EXJournal monitoring is enabled.")
                    StartService(EXJournalServiceName)
                Else
                    WriteAuditEntry(Now.ToString & " EXJournal monitoring is not enabled.")
                End If
            Catch ex As Exception
                boolEXJournal = False
            End Try

            Try
                If boolCore64Service Then
                    StartService(VitalSignsCore64)
                Else
                    WriteAuditEntry(Now.ToString & " Core 64 features are not selected and the service will not be started.")
                End If
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error starting " & VitalSignsCore64 & ": " & ex.Message)
            End Try

            Thread.Sleep(5000)

            WriteAuditEntry(Now.ToString & " Starting the main functions....")
            Try
                threadDailyTasks.CurrentCulture = New CultureInfo(sCultureString)
                threadDailyTasks.Start()
            Catch ex As Exception

            End Try
        Else
            WriteAuditEntry(Now.ToString & " Cannot start VitalSigns Plus Master Service, as the available disk space is less than 500Mb of the total disk space... ")
            WriteAuditEntry(Now.ToString & " Free Disk Space " & Math.Round(Convert.ToDouble(moHD("FreeSpace")) / 1024 / 1024).ToString & "(Mb) Total Disk Space " & Math.Round(Convert.ToDouble(moHD("Size")) / 1024 / 1024 / 1024).ToString & "(Gb)")
            '4/10/2015 NS added for VSPLUS-1397
            myAlert.QueueSysMessage(spaceString)
        End If
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        'Try
        '    StopService("VitalSigns Output Service")
        'Catch ex As Exception
        '    WriteAuditEntry(Now.ToString & " Error stopping output service: " & ex.Message)
        'End Try

        WriteAuditEntry(Now.ToString & " The Master Service is shutting down... ")

        Try
            threadDailyTasks.Abort()
        Catch ex As Exception

        End Try

        Try
            StopService(VitalSignsPlusDomino)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping " & VitalSignsPlusDomino & ": " & ex.Message)
        End Try

        Try
            StopService(VitalSignsMicrosoft)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping Microsoft service: " & ex.Message)
        End Try

        Try
            StopService(VitalSignsPlusCore)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping " & VitalSignsPlusCore & ": " & ex.Message)
        End Try

        'Try
        '    StopService(VitalSignsPlusClusterHealth)
        'Catch ex As Exception
        '    WriteAuditEntry(Now.ToString & " Error stopping " & VitalSignsPlusClusterHealth & ": " & ex.Message)
        'End Try


        Try
            StopService(VitalSignsPlusAlerting)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping " & VitalSignsPlusAlerting & ": " & ex.Message)
        End Try

        Try
            StopService(VitalSignsDailyService)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping Daily Tasks service: " & ex.Message)
        End Try

        Try
            StopService(EXJournalServiceName)
        Catch ex As Exception
            'WriteAuditEntry(Now.ToString & " Error stopping " & VitalSignsPlusCore & ": " & ex.Message)
        End Try

        Try
            StopService(VitalSignsPlusDBHealthService)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping Database Health service: " & ex.Message)
        End Try

        Try
            StopService(VitalSignsConsoleCommands)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping Domino Console Commands service: " & ex.Message)
        End Try

        Try
            StopService(VitalSignsCore64)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping Core 64 service: " & ex.Message)
        End Try

        Try
            Thread.Sleep(5 * 1000)
            If ServiceStatus(VitalSignsPlusDomino) = "Stop Pending" Then
                WriteAuditEntry(Now.ToString & " The monitor service is stuck in StopPending mode, the process will be killed.")
                KillServiceProcess(VitalSignsPlusDomino)
                KillNotes()
            End If
        Catch ex As Exception

        End Try

        m_stopRequested = True

        ' Wait for threads to exit, but wait a max of 10 seconds..
        Dim counter As Integer = 0
        Do While (m_numActiveThreads > 0) And (counter < 100)
            counter = counter + 1
            System.Threading.Thread.Sleep(100)
        Loop
        MyBase.OnStop()
    End Sub

#Region "Stop Master Service"
    Protected Sub StopMasterService()
        WriteAuditEntry(Now.ToString & " The Master Service is shutting down... ")

        Try
            threadDailyTasks.Abort()
        Catch ex As Exception

        End Try

        Try
            StopService(VitalSignsPlusDomino)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping " & VitalSignsPlusDomino & ": " & ex.Message)
        End Try

        Try
            StopService(VitalSignsMicrosoft)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping Exchange service: " & ex.Message)
        End Try

        Try
            StopService(VitalSignsPlusCore)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping " & VitalSignsPlusCore & ": " & ex.Message)
        End Try

        'Try
        '    StopService(VitalSignsPlusClusterHealth)
        'Catch ex As Exception
        '    WriteAuditEntry(Now.ToString & " Error stopping " & VitalSignsPlusClusterHealth & ": " & ex.Message)
        'End Try


        Try
            StopService(VitalSignsPlusAlerting)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping " & VitalSignsPlusAlerting & ": " & ex.Message)
        End Try

        Try
            StopService(VitalSignsDailyService)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping Daily Tasks service: " & ex.Message)
        End Try

        Try
            StopService(EXJournalServiceName)
        Catch ex As Exception
            'WriteAuditEntry(Now.ToString & " Error stopping " & VitalSignsPlusCore & ": " & ex.Message)
        End Try

        Try
            StopService(VitalSignsPlusDBHealthService)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping Database Health service: " & ex.Message)
        End Try

        Try
            StopService(VitalSignsConsoleCommands)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error stopping Domino Console Commands service: " & ex.Message)
        End Try

        Try
            StopService(VitalSignsCore64)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & "Error stopping Core 64 service: " & ex.Message)
        End Try

        Try
            Thread.Sleep(5 * 1000)
            If ServiceStatus(VitalSignsPlusDomino) = "Stop Pending" Then
                WriteAuditEntry(Now.ToString & " The monitor service is stuck in StopPending mode, the process will be killed.")
                KillServiceProcess(VitalSignsPlusDomino)
                KillNotes()
            End If
        Catch ex As Exception

        End Try

        m_stopRequested = True

        ' Wait for threads to exit, but wait a max of 10 seconds..
        Dim counter As Integer = 0
        Do While (m_numActiveThreads > 0) And (counter < 100)
            counter = counter + 1
            System.Threading.Thread.Sleep(100)
        Loop
    End Sub
#End Region
#Region "Threads"

    Protected Sub DailyTasks()
        Dim Hour As Integer

        Dim myRegistry As New RegistryHandler
        Dim LastDate, MyDate, StatusDate As DateTime
        ' Thread.Sleep(30000)
        ' Thread.Sleep(1000 * 60 * 1)  ' wait 5 minutes before trying this

        Do 'While TimeToStop = False
            SetBooleansOfServicesFromSelectedFeatures()
            Dim strDiskName As String
            strDiskName = System.IO.Path.GetPathRoot(System.Reflection.Assembly.GetExecutingAssembly.Location)
            'Get directory Details, Default disk is 'Disk C'
            Dim moHD As ManagementObject
            moHD = GetHDDetails(strDiskName.Replace(":\", ""))
            'Check for free space of 1GB
            If ((Math.Round(Convert.ToDouble(moHD("FreeSpace")) / 1024 / 1024)) <= 500) Then
                If (Math.Round(Convert.ToDouble(moHD("FreeSpace")) / 1024 / 1024) + Convert.ToDouble(LogUtilities.LogUtils.GetLogRotationSizeMB()) > 500) Then
                    LogUtilities.LogUtils.DeleteLogRotationFolder()
                Else
                    myAlert.QueueSysMessage(spaceString)
                    WriteAuditEntry(Now.ToString & " Cannot start VitalSigns Plus Master Service, as the available disk space is less than 500Mb of the total disk space... ")
                    WriteAuditEntry(Now.ToString & " Free Disk Space " & Math.Round(Convert.ToDouble(moHD("FreeSpace")) / 1024 / 1024).ToString & "(Mb) Total Disk Space " & Math.Round(Convert.ToDouble(moHD("Size")) / 1024 / 1024 / 1024).ToString & "(Gb)")
                    'Stop All Services
                    StopMasterService()
                End If
            Else
                myAlert.ResetSysMessage(spaceString)
                WriteAuditEntry(Now.ToString & " Free Disk Space " & Math.Round(Convert.ToDouble(moHD("FreeSpace")) / 1024 / 1024).ToString & "(Mb) Total Disk Space " & Math.Round(Convert.ToDouble(moHD("Size")) / 1024 / 1024 / 1024).ToString & "(Gb)")
                '  MyDate = Now
                TimeToStop = True
                Try
                    Dim strLastDate As String = myRegistry.ReadFromRegistry("Service Reset Date")
                    'LastDate = CType(myRegistry.ReadFromRegistry("Service Reset Date"), DateTime)
                    LastDate = FixDate(strLastDate)
                Catch ex As Exception
                    LastDate = Now.AddDays(-2).ToShortDateString
                End Try

                Dim RestartTime As Boolean = False

                Try
                    ' If LastDate.ToShortDateString <> MyDate.ToShortDateString Then
                    If LastDate.Date <> FixDate(Now) Then
                        ' If LastDate.Year <> MyDate.Year And LastDate.Month <> MyDate.Month And LastDate.Day <> LastDate.Day Then
                        If (Now.DayOfWeek = DayOfWeek.Saturday Or Now.DayOfWeek = DayOfWeek.Sunday) And Now.Hour > 10 Then
                            RestartTime = True
                        End If
                        If Now.DayOfWeek <> DayOfWeek.Saturday And Now.DayOfWeek <> DayOfWeek.Sunday And Now.Hour > 7 Then
                            RestartTime = True
                        End If
                    End If
                Catch ex As Exception

                End Try


                If RestartTime = True Then
                    'If time for a daily restart, then do this
                    Try
                        WriteAuditEntry(Now.ToString & " Commencing daily restart of the monitor service.")
                        Dim strdt As String
                        Dim strDateFormat As String
                        strDateFormat = objDateUtils.GetDateFormat()
                        strdt = objDateUtils.FixDate(Date.Now, strDateFormat)
                        myRegistry.WriteToRegistry("Service Reset Date", strdt)
                        RestartTime = False
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Exception #2: " & ex.ToString)
                    End Try

                    Try
                        StopService(VitalSignsPlusDomino)
                    Catch ex As Exception

                    End Try

                    Try
                        StopService(VitalSignsMicrosoft)
                    Catch ex As Exception

                    End Try

                    Try
                        StopService(VitalSignsPlusAlerting)
                    Catch ex As Exception

                    End Try

                    Try
                        WriteAuditEntry(Now.ToString & " Shutting down EXJournal monitoring.")
                        StopService(EXJournalServiceName)
                    Catch ex As Exception

                    End Try

                    Try
                        StopService(VitalSignsPlusCore)
                    Catch ex As Exception

                    End Try

                    Try
                        StopService(VitalSignsConsoleCommands)
                    Catch ex As Exception

                    End Try

                    Try
                        If Now.DayOfWeek = DayOfWeek.Sunday Then
                            StopService(VitalSignsPlusDBHealthService)
                        End If

                    Catch ex As Exception

                    End Try

                    Try
                        StopService(VitalSignsCore64)
                    Catch ex As Exception

                    End Try
                    Thread.Sleep(5 * 1000)


                    Try
                        If ServiceStatus(VitalSignsPlusDomino) = "Stop Pending" Then
                            WriteAuditEntry(Now.ToString & " The Domino monitor service is stuck in StopPending mode, the process will be killed.")
                            KillServiceProcess(VitalSignsPlusDomino)
                            KillNotes()
                            Thread.Sleep(5 * 1000)
                        End If
                    Catch ex As Exception

                    End Try

                    'Try
                    '    If ServiceStatus(VitalSignsPlusCore) = "Stop Pending" Then
                    '        WriteAuditEntry(Now.ToString & " The Core monitor service is stuck in StopPending mode, the process will be killed.")
                    '        KillProcess("VitalSignsPlusCore")
                    '        Thread.Sleep(5 * 1000)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    Try
                        WriteAuditEntry(Now.ToString & " Calling KillNotes to clear up an Notes API issues.")
                        KillNotes()
                    Catch ex As Exception

                    End Try



                    Try
                        WriteAuditEntry(Now.ToString & " The monitor service status is " & ServiceStatus(VitalSignsPlusDomino))
                    Catch ex As Exception

                    End Try

                    'VSPlus-601
                    'Try
                    '    DailyZipUp()
                    'Catch ex As Exception

                    'End Try

                    'Clear out the StatusTable if Primary Node
                    Try
                        ClearDatabaseTables()
                    Catch ex As Exception

                    End Try


                    Try
                        If boolDominoService Then
                            WriteAuditEntry(Now.ToString & " Attempting to restart the Domino monitoring service.")
                            StartService(VitalSignsPlusDomino)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If boolMicrosoftService Then
                            WriteAuditEntry(Now.ToString & " Attempting to restart the Microsoft monitoring service.")
                            StartService(VitalSignsMicrosoft)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        WriteAuditEntry(Now.ToString & " Attempting to restart the core monitoring service.")
                        StartService(VitalSignsPlusCore)
                    Catch ex As Exception

                    End Try

                    Try
                        WriteAuditEntry(Now.ToString & " Attempting to restart the Alerting service.")
                        StartService(VitalSignsPlusAlerting)
                    Catch ex As Exception

                    End Try

                    'Try
                    '    WriteAuditEntry(Now.ToString & " Attempting to restart the cluster health service.")
                    '    StartService(VitalSignsPlusClusterHealth)
                    'Catch ex As Exception

                    'End Try

                    Try
                        If boolDominoConsoleCommands = True Then
                            StartService(VitalSignsConsoleCommands)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If boolEXJournal = True Then
                            WriteAuditEntry(Now.ToString & " Attempting to restart the EXJournal monitoring service.")
                            StartService(EXJournalServiceName)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If boolCore64Service Then
                            WriteAuditEntry(Now.ToString & " Attempting to restart the Core 64 monitoring service.")
                            StartService(VitalSignsCore64)
                        End If
                    Catch ex As Exception

                    End Try

                    Thread.Sleep(2000)

                End If


                Try
                    Dim strLastDate As String = myRegistry.ReadFromRegistry("Daily Tasks Date")
                    LastDate = FixDate(strLastDate)
                Catch ex As Exception
                    LastDate = Now.AddDays(-2).ToShortDateString
                End Try

                If LastDate.Date <> FixDate(Now) Then
                    ' *** If time for the daily tasks.
                    WriteAuditEntry(Now.ToString & " Daily Tasks are due, starting them now.")
                    Try
                        StartService(VitalSignsDailyService)
                    Catch ex As Exception

                    End Try
                    Dim strdt As String
                    Dim strDateFormat As String
                    strDateFormat = objDateUtils.GetDateFormat()
                    strdt = objDateUtils.FixDate(Date.Now, strDateFormat)
                    myRegistry.WriteToRegistry("Daily Tasks Date", strdt)
                End If

                'FIND SERVERS NOT SCANNED COUNT AND MODIFY BELOW IF CONDITION WITH 'OR'

                'VSPLUS-416,19Dec14

                Dim numOfServersNotDBScanned As Int32
                Try
                    Dim repositoryServers As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
                    Dim filterDefServers As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repositoryServers.Filter _
                                                                                                .Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription()) _
                                                                                                And
                                                                                                repositoryServers.Filter.Eq(Function(x) x.IsEnabled, True)

                    Dim listOfServers As List(Of VSNext.Mongo.Entities.Server) = repositoryServers.Find(filterDefServers).ToList()




                    Dim repositoryDatabase As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Database)(connectionString)
                    Dim filterDefDatabase As FilterDefinition(Of VSNext.Mongo.Entities.Database) = repositoryDatabase.Filter.Lte(Function(x) x.ModifiedOn, Now.AddDays(-1)) _
                                                                                                   And repositoryDatabase.Filter.In(Function(x) x.DeviceName, listOfServers.Select(Of String)(Function(x) x.DeviceName).ToList())


                    numOfServersNotDBScanned = repositoryDatabase.Find(filterDefDatabase).Count()


                Catch ex As Exception
                    numOfServersNotDBScanned = 0
                End Try

                If LastDate.Date <> FixDate(Now) Or numOfServersNotDBScanned > 0 Then
                    'VSPLUS-416 Hourly check to start service:Mukund 21Jan15
                    Try
                        Hour = CType(myRegistry.ReadFromRegistry("Core Hourly Tasks"), Integer)
                    Catch ex As Exception
                        Hour = -1
                    End Try
                    If Now.Hour <> Hour Then

                        WriteAuditEntry(Now.ToString & " Calling Hourly Tasks")
                        Try
                            If ServiceStatus(VitalSignsPlusDBHealthService) = "Stopped" Then
                                WriteAuditEntry(Now.ToString & " Service Stopped:rows:" & numOfServersNotDBScanned.ToString() & ", LastDate:" & LastDate.ToShortDateString & ", MyDate: " & MyDate.ToShortDateString)
                                StartService(VitalSignsPlusDBHealthService)
                                WriteAuditEntry(Now.ToString & " DB Health is due, starting service now.")
                            End If
                        Catch ex As Exception
                        End Try

                    End If
                End If



                '************ Do this every loop

                Dim strSQL As String
                Dim intResult As Double = 0
                Dim intTotalRowCount As Double = 0
                Dim Results As String

                If boolDominoService Then

                    Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)()

                    Try
                        Dim statusRepository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                        Dim statusFilterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = statusRepository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
                        Dim statusProjection As ProjectionDefinition(Of VSNext.Mongo.Entities.Status) = statusRepository.Project _
                                                                                                        .Include(Function(x) x.CurrentStatus) _
                                                                                                        .Include(Function(x) x.LastUpdated) _
                                                                                                        .Include(Function(x) x.NextScan) _
                                                                                                        .Include(Function(x) x.LastUpdated) _
                                                                                                        .Include(Function(x) x.DeviceType) _
                                                                                                        .Include(Function(x) x.Description)

                        listOfStatus = statusRepository.Find(statusFilterDef, statusProjection).ToList()

                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & "  Error getting all the rows in the status table" & ex.ToString)
                    End Try




                    Try
                        intResult = listOfStatus _
                            .Where(Function(x) Not {"Scanning", "Not Scanned", "Disabled", "Insufficient Licenses"}.Contains(x.CurrentStatus)) _
                            .Where(Function(x) x.DeviceType.Equals(VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())) _
                            .Select(Function(x) x.CurrentStatus) _
                            .Distinct().Count()

                        WriteAuditEntry(Now.ToString & "  There are  " & intResult & " unique status values other than Scanning -- so I think service is running fine.")
                    Catch ex As Exception
                        intResult = 5
                    End Try

                    If ServiceRunning(VitalSignsPlusDomino) = True And ServiceStatus(VitalSignsPlusDomino) <> "Start Pending" And intResult = 0 And intTotalRowCount > 0 Then
                        Try
                            ' Commenting this as we are executing the same query again. 
                            'intResult = Convert.ToDouble(vsobj.ExecuteScalarAny("VitalSigns", "Status", strSQL))
                            If intResult = 0 Then
                                '#1
                                WriteAuditEntry(Now.ToString & " VitalSignsPlusDomino Monitoring Service is stuck in 'SCANNING' mode, so I am attempting to start it. ")
                                Thread.Sleep(5000)
                                'StopService(VitalSignsPlusDomino)
                                Thread.Sleep(5000)
                                '1/5/6 WS Commented out since now handled in StopService
                                'KillProcess("VitalSignsService")
                                'KillNotes()
                            End If

                        Catch ex As Exception

                        End Try
                    End If


                    '************ Code to ensure that the latest updates are recent

                    'strSQL = "SELECT COUNT(*) N FROM STATUS WHERE Type = 'Domino' AND Status <> 'Scanning' AND Status <> 'Not Scanned' AND Status <> 'Disabled' AND "
                    'removed the condition status not scanned on 12/23/2013 because HSBC had servers stuck in scanning mode that was not detected.


                    'Gets the count of status' older then 45 minutes

                    Dim Servername As String
                    Servername = ""
                    Dim ts As New TimeSpan()
                    Try
                        Dim tempList As List(Of VSNext.Mongo.Entities.Status) = listOfStatus _
                                                                                .Where(Function(x) New TimeSpan((Now - x.LastUpdated.Value).Ticks).TotalMinutes > 45) _
                                                                                .Where(Function(x) x.DeviceType.Equals(VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())) _
                                                                                .Where(Function(x) Not {"Scanning", "Not Scanned", "Disabled", "Insufficient Licenses"}.Contains(x.CurrentStatus)) _
                                                                                .ToList()
                        intResult = tempList.Count
                        WriteAuditEntry(Now.ToString & " There are  " & tempList.Count & " Domino servers with status older than 45 minutes.")
                        Servername = String.Join(",", tempList)
                        Servername.Substring(0, Servername.Length - 1)
                        WriteAuditEntry(Now.ToString & " Domino Servers with status older than 45 minutes are  " & Servername & "")
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error printing out the servers over 45 minutes. Error: " & ex.Message)
                    End Try

                    Try

                        Dim MonitoringDelay As Int32

                        Try
                            Dim repositoryNameValue As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.NameValue)(connectionString)
                            Dim filterDefNameValue As FilterDefinition(Of VSNext.Mongo.Entities.NameValue) = repositoryNameValue.Filter.Eq(Function(x) x.Name, "MonitoringDelay")

                            MonitoringDelay = Convert.ToInt32(repositoryNameValue.Find(filterDefNameValue)(0).Value.ToString())
                        Catch ex As Exception
                            MonitoringDelay = 0
                        End Try

                        If MonitoringDelay > 0 Then

                            Dim tempList As List(Of VSNext.Mongo.Entities.Status) = listOfStatus _
                                                                                .Where(Function(x) New TimeSpan((Now - x.NextScan.Value).Ticks).TotalMinutes > MonitoringDelay) _
                                                                                .Where(Function(x) x.DeviceType.Equals(VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())) _
                                                                                .Where(Function(x) Not {"Not Scanned", "Insufficient Licenses"}.Contains(x.CurrentStatus)) _
                                                                                .ToList()

                            Try
                                For Each entity As VSNext.Mongo.Entities.Status In tempList
                                    Try
                                        Dim delayedBy As Int32 = New TimeSpan((Now - entity.NextScan.Value).Ticks).TotalMinutes
                                        'Dim SummaryData As New Data.DataTable
                                        If delayedBy >= MonitoringDelay Then
                                            myAlert.QueueAlert(entity.DeviceType, entity.DeviceName, "Monitoring Delay", entity.DeviceName & " did not scan after next scan time of " & entity.NextScan & "", "Location")
                                        Else
                                            myAlert.ResetAlert(entity.DeviceType, entity.DeviceName, "Monitoring Delay", "Location")
                                        End If

                                    Catch ex As Exception
                                        WriteAuditEntry(Now.ToString & " Exception during finding the delay for each server. Exception : " & ex.Message)
                                    End Try
                                    WriteAuditEntry(Now.ToString & " Servers which are not scanned after Next Scan time and the Server(s) is/are " & entity.DeviceName & "")
                                Next

                            Catch ex As Exception

                            End Try


                        End If


                    Catch ex As Exception

                    End Try

                    ' -------------------- END OF NEXT SCAN DID NOT REACH ON TIME. --------------------------


                    Try
                        If ServiceRunning(VitalSignsPlusDomino) = True And ServiceStatus(VitalSignsPlusDomino) <> "Start Pending" And intResult > 0 Then
                            Try
                                ' Commenting this as we are executing the same query again. 
                                ' intResult = Convert.ToDouble(vsobj.ExecuteScalarAny("VitalSigns", "Status", strSQL))
                                If intResult > 0 Then
                                    If DateTime.Now.Subtract(dominoKilled).Minutes > 15 Then
                                        '#2
                                        dominoKilled = DateTime.Now
                                        WriteAuditEntry(Now.ToString & " VitalSignsPlusDomino Monitoring Service is stuck in 'SCANNING' mode or Threading issue, so I am attempting to start it. ")
                                        Thread.Sleep(5000)
                                        'StopService(VitalSignsPlusDomino)
                                        Thread.Sleep(5000)
                                        '1/5/6 WS Commented out since now handled in StopService
                                        'KillProcess("VitalSignsService")
                                        'KillNotes()
                                    Else
                                        WriteAuditEntry(Now.ToString & " Service reset domino service " + DateTime.Now.Subtract(dominoKilled).Minutes.ToString() + " minutes ago due to threading or scanning issue.  Will wait for 15 minutes to for this again. ")
                                    End If
                                End If
                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " Exception #3 Inenr Try: " & ex.ToString)
                            End Try
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Exception #3: " & ex.ToString)
                    End Try



                    'Checks to see if any Domino Servers are 2 times past their next scan time
                    ' Example. Server is last scaned at 1:00 with a 10 mintue Scan Interval. This will check if it was scanned by 1:20 or not

                    Try

                        intResult = listOfStatus.Count()

                        If ServiceRunning(VitalSignsPlusDomino) = True And ServiceStatus(VitalSignsPlusDomino) <> "Start Pending" And intResult > 0 Then
                            Try

                                Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)()
                                Try
                                    Dim repositoryServers As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
                                    Dim filterDefServers As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repositoryServers.Filter.Where(Function(x) True)
                                    listOfServers = repositoryServers.Find(filterDefServers).ToList()
                                Catch ex As Exception

                                End Try

                                For Each statusEntity As VSNext.Mongo.Entities.Status In listOfStatus
                                    Dim serverEntity As VSNext.Mongo.Entities.Server
                                    Dim scanInterval As Int32

                                    Try
                                        Dim maint As New MaintenanceDLL.MaintenanceDll()


                                        serverEntity = listOfServers.Where(Function(x) x.DeviceName.Equals(statusEntity.DeviceName) And x.DeviceType.Equals(statusEntity.DeviceType)).ToList()(0)
                                        If maint.OffHours(serverEntity.DeviceName) Then
                                            scanInterval = serverEntity.OffHoursScanInterval
                                        Else
                                            scanInterval = serverEntity.ScanInterval
                                        End If

                                    Catch ex As Exception
                                        scanInterval = 10
                                    End Try

                                    If (Now - statusEntity.LastUpdated.Value).TotalMinutes > (scanInterval * 2) Then
                                        If DateTime.Now.Subtract(dominoKilled).Minutes > 15 Then
                                            dominoKilled = DateTime.Now
                                            '#2
                                            'WriteAuditEntry(Now.ToString & " VitalSignsPlusDomino Monitoring Service was last updated " & intResult & " minutes ago, and this is more than twice the Max Scan Interval of " & maxScanInterval & " so I am attempting to restart it. ")
                                            WriteAuditEntry(Now.ToString & " VitalSignsPlusDomino Monitoring Serviceis being restarted since the server " & statusEntity.DeviceName & " has not been scanned in 2x its interval.")
                                            Thread.Sleep(5000)
                                            StopService(VitalSignsPlusDomino)
                                            Thread.Sleep(5000)
                                            '1/5/6 WS Commented out since now handled in StopService
                                            'KillProcess("VitalSignsService")
                                            KillNotes()
                                            Exit For
                                        Else
                                            WriteAuditEntry(Now.ToString & " Service reset domino service " + DateTime.Now.Subtract(dominoKilled).Minutes.ToString() + " minutes ago due to last scan was longer then max scan interval.  Will wait for 15 minutes to for this again. #2")
                                        End If
                                    End If

                                Next

                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " Exception #3 Inner Try: " & ex.ToString)
                            End Try
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Exception #3: " & ex.ToString)
                    End Try



                    ' Starting to see if there are 25% of the servers in the Telnet Mode to restart the service to catch up doing regular scans. 
                    ' This problem has been happening @ HSBC that all the servers goes into Telnet mode and come back up after a service restart.

                    'Checks to see if 1/4 of the domino servers are in telnet

                    Dim intResultTotalTelnet As Double = 0
                    Dim telnetSpan As TimeSpan = New TimeSpan
                    Try
                        ' Remove this after testing. 

                        intResult = listOfStatus.Where(Function(x) x.DeviceType = VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription()).Count()
                        'WriteAuditEntry(Now.ToString & " There are  " & intResult & " Total Domino Servers in Monitoring")


                        intResultTotalTelnet = listOfStatus _
                            .Where(Function(x) x.DeviceType = VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription()) _
                            .Where(Function(x) x.Description.Contains("via telnet")) _
                            .Count()
                        'WriteAuditEntry(Now.ToString & " There are  " & intResultTotalTelnet & " Total Telnet Domino Servers Status")


                    Catch ex As Exception
                        intResult = 4  'as dividing by 4 I need a non 0 number
                        intResultTotalTelnet = 0
                    End Try

                    Try
                        If ServiceRunning(VitalSignsPlusDomino) = True And ServiceStatus(VitalSignsPlusDomino) <> "Start Pending" And intResultTotalTelnet > (intResult / 4) Then

                            telnetSpan = DateTime.Now.Subtract(dominoKilled)
                            ' Give at least 15 mins after Telnet Restart to see if the servers are out of telnet cycles.. 
                            If telnetSpan.Minutes > 15 Then
                                Try
                                    ' Commenting this as we are executing the same query again. 
                                    ' intResult = Convert.ToDouble(vsobj.ExecuteScalarAny("VitalSigns", "Status", strSQL))
                                    WriteAuditEntry(Now.ToString & " VitalSignsPlusDomino Monitoring Service is stuck in 'TELNET' mode or Threading issue, so I am attempting to start it. ")
                                    Dim properties As System.Net.NetworkInformation.IPGlobalProperties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties()
                                    Dim connections As System.Net.NetworkInformation.TcpConnectionInformation() = properties.GetActiveTcpConnections()
                                    Dim dict As New Collections.Generic.Dictionary(Of String, Integer)()
                                    Dim dictPortsREP As New Collections.Generic.Dictionary(Of String, Integer)()
                                    Dim dictPortsLEP As New Collections.Generic.Dictionary(Of String, Integer)()

                                    For Each connection As System.Net.NetworkInformation.TcpConnectionInformation In connections

                                        If dictPortsREP.ContainsKey(connection.RemoteEndPoint.Port) Then
                                            dictPortsREP(connection.RemoteEndPoint.Port.ToString()) += 1
                                        Else
                                            dictPortsREP.Add(connection.RemoteEndPoint.Port.ToString(), 1)
                                        End If

                                        If dictPortsLEP.ContainsKey(connection.LocalEndPoint.Port) Then
                                            dictPortsLEP(connection.LocalEndPoint.Port.ToString()) += 1
                                        Else
                                            dictPortsLEP.Add(connection.LocalEndPoint.Port.ToString(), 1)
                                        End If

                                        If dict.ContainsKey(connection.State.ToString) Then
                                            dict(connection.State.ToString()) += 1
                                        Else
                                            dict.Add(connection.State.ToString(), 1)
                                        End If

                                    Next

                                    Dim str1 As String = ""
                                    Dim str2 As String = ""
                                    Dim str3 As String = ""
                                    For Each key As String In dict.Keys
                                        str1 = str1 & key & ":" & dict(key) & ", "
                                    Next
                                    For Each key As String In dictPortsLEP.Keys
                                        str2 = str2 & "port " & key & " has " & dictPortsLEP(key) & " open connections, "
                                    Next
                                    For Each key As String In dictPortsREP.Keys
                                        str3 = str3 & "port " & key & " has " & dictPortsREP(key) & " open connections, "
                                    Next

                                    Dim s As String = DateTime.Now.ToString & " Total TCP connections: " & connections.Length & ".  Breakdown by status: " & str1.Substring(0, str1.Length - 2)
                                    Dim s2 As String = DateTime.Now.ToString & " Breakdown by Local End Point Ports: " & str2.Substring(0, str2.Length - 2)
                                    Dim s3 As String = DateTime.Now.ToString & " Breakdown by Remote End Point Ports: " & str3.Substring(0, str3.Length - 2)

                                    WriteAuditEntry(s)
                                    WriteAuditEntry(s2)
                                    WriteAuditEntry(s3)

                                    WriteAuditEntry(Now.ToString & " Total Servers Monitoring = " & intResult & " and total Telent Status = " & intResultTotalTelnet)
                                    Thread.Sleep(5000)
                                    StopService(VitalSignsPlusDomino)
                                    Thread.Sleep(5000)
                                    '1/5/6 WS Commented out since now handled in StopService
                                    'KillProcess("VitalSignsService")
                                    KillNotes()
                                    dominoKilled = DateTime.Now
                                    ' Give 15 mins after restart to check this again.  
                                Catch ex As Exception

                                End Try
                            End If
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Exception #3: " & ex.ToString)
                    End Try


                    'This bit is responsible to check to see if enough threads in the domino service were killed for a service restart
                    'checks to see if enough domino threads were kills to warrent a restart
                    Try
                        If ServiceRunning(VitalSignsPlusDomino) = True And ServiceStatus(VitalSignsPlusDomino) <> "Start Pending" Then

                            Dim repositoryNodes As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Nodes)(connectionString)
                            Dim repositoryNameValue As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.NameValue)(connectionString)

                            Dim filterDefNameValue As FilterDefinition(Of VSNext.Mongo.Entities.NameValue) = repositoryNameValue.Filter.Eq(Function(x) x.Name, "Domino Thread Killed Counter Max")
                            Dim maxVal As Integer
                            Try
                                maxVal = Convert.ToInt32(repositoryNameValue.Find(filterDefNameValue).ToList()(0).Value.ToString())
                            Catch ex As Exception
                                maxVal = 30
                            End Try
                            Dim filterDefNodes As FilterDefinition(Of VSNext.Mongo.Entities.Nodes) = repositoryNodes.Filter.Eq(Function(x) x.Name, System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString())
                            Dim currentVal As Integer
                            Try
                                currentVal = repositoryNodes.Find(filterDefNodes).ToList()(0).DominoThreadKilledCount
                            Catch ex As Exception
                                currentVal = 0
                            End Try



                            If Date.Now.Subtract(dominoKilled).Minutes > 15 And currentVal >= maxVal Then
                                Try

                                    WriteAuditEntry(Now.ToString & " VitalSignsPlusDomino Monitoring Service has aborted too many threads, so I am attempting to start it. ")
                                    Thread.Sleep(5000)
                                    StopService(VitalSignsPlusDomino)
                                    Thread.Sleep(5000)
                                    '1/5/6 WS Commented out since now handled in StopService
                                    'KillProcess("VitalSignsService")
                                    KillNotes()
                                    dominoKilled = DateTime.Now
                                    ' Give 15 mins after restart to check this again.  
                                Catch ex As Exception

                                End Try
                            End If
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Exception restarting due to killed threads in Domino service: " & ex.ToString)
                    End Try


                End If

                ' VitalSigns Alert Service
                '*** If it is not running at all, try to start it

                Try
                    If ServiceRunning(VitalSignsPlusAlerting) = False And ServiceStatus(VitalSignsPlusAlerting) <> "Start Pending" Then
                        WriteAuditEntry(Now.ToString & " " & VitalSignsPlusAlerting & " is not running, so I am attempting to start it. ")
                        Thread.Sleep(5000)
                        StartService(VitalSignsPlusAlerting)
                        Thread.Sleep(5000)
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception #4: " & ex.ToString)
                End Try




                '*** If it is stuck in Stop Pending, which sometimes happens
                Try
                    If ServiceStatus(VitalSignsPlusAlerting) = "Stop Pending" Then
                        WriteAuditEntry(Now.ToString & " The VitalSignsPlusAlerting service is stuck in StopPending mode, the process will be killed.")
                        KillServiceProcess(VitalSignsPlusAlerting)
                        ' KillNotes()
                        Thread.Sleep(5 * 1000)

                        Try
                            StartService(VitalSignsPlusAlerting)
                            WriteAuditEntry(Now.ToString & " The VitalSignsPlusAlerting service status is " & ServiceStatus(VitalSignsPlusDomino))
                        Catch ex As Exception

                        End Try
                    End If
                Catch ex As Exception

                End Try

                '*** If it is stuck in Start Pending, which sometimes happens
                Try
                    If ServiceStatus(VitalSignsPlusAlerting) = "Start Pending" Then
                        WriteAuditEntry(Now.ToString & " The VitalSignsPlusAlerting service is Start Pending so I'm going to give it some time before I kill it.")
                        Thread.Sleep(30 * 1000)
                        If ServiceStatus(VitalSignsPlusAlerting) = "Start Pending" Then

                            WriteAuditEntry(Now.ToString & " The VitalSignsPlusAlerting service is stuck in StartPending mode, the process will be killed.")
                            '  KillNotes()
                            Thread.Sleep(2 * 1000)
                            KillServiceProcess(VitalSignsPlusAlerting)
                            Thread.Sleep(5 * 1000)
                        Else
                            WriteAuditEntry(Now.ToString & " The VitalSignsPlusAlerting service started up. ")
                        End If


                        Try
                            StartService(VitalSignsPlusAlerting)
                            WriteAuditEntry(Now.ToString & " The alert service status is " & ServiceStatus(VitalSignsPlusAlerting))
                        Catch ex As Exception

                        End Try
                    End If
                Catch ex As Exception

                End Try

                ' VitalSigns Core  Service
                '*** If it is not running at all, try to start it
                Try
                    If ServiceRunning(VitalSignsPlusCore) = False And ServiceStatus(VitalSignsPlusCore) <> "Start Pending" Then
                        Try
                            WriteAuditEntry(Now.ToString & " " & VitalSignsPlusCore & " is not running, so I am attempting to start it. ")
                            Thread.Sleep(5000)
                            StartService(VitalSignsPlusCore)
                            Thread.Sleep(5000)
                        Catch ex As Exception

                        End Try
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception #5: " & ex.ToString)
                End Try


                '*** If it is stuck in Stop Pending, which sometimes happens
                Try
                    If ServiceStatus(VitalSignsPlusCore) = "Stop Pending" Then
                        WriteAuditEntry(Now.ToString & " The VitalSignsPlusCore service is stuck in StopPending mode, the process will be killed.")
                        KillServiceProcess(VitalSignsPlusCore)
                        'KillNotes()
                        Thread.Sleep(5 * 1000)

                        Try
                            StartService(VitalSignsPlusCore)
                            WriteAuditEntry(Now.ToString & " The VitalSignsPlusCore core service status is " & ServiceStatus(VitalSignsPlusDomino))
                        Catch ex As Exception

                        End Try
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception #6: " & ex.ToString)
                End Try

                '*** If it is stuck in Start Pending, which sometimes happens
                Try
                    If ServiceStatus(VitalSignsPlusCore) = "Start Pending" Then
                        WriteAuditEntry(Now.ToString & " The VitalSignsPlusCore service is Start Pending so I'm going to give it some time before I kill it.")
                        Thread.Sleep(30 * 1000)
                        If ServiceStatus(VitalSignsPlusCore) = "Start Pending" Then

                            WriteAuditEntry(Now.ToString & " The VitalSignsPlusCore service is stuck in StartPending mode, the process will be killed.")
                            ' KillNotes()
                            Thread.Sleep(2 * 1000)
                            KillServiceProcess(VitalSignsPlusCore)
                            Thread.Sleep(5 * 1000)
                        Else
                            WriteAuditEntry(Now.ToString & " The VitalSignsPlusCore service started up. ")
                        End If


                        Try
                            StartService(VitalSignsPlusCore)
                            WriteAuditEntry(Now.ToString & " The VitalSignsPlusCore service status is " & ServiceStatus(VitalSignsPlusCore))
                        Catch ex As Exception

                        End Try
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception #7: " & ex.ToString)
                End Try




                ' VitalSigns for Domino
                '*** If it is not running at all, try to start it
                Try
                    If boolDominoService Then
                        If ServiceRunning(VitalSignsPlusDomino) = False And ServiceStatus(VitalSignsPlusDomino) <> "Start Pending" Then
                            Try
                                WriteAuditEntry(Now.ToString & " VitalSigns Monitoring Service is not running, so I am attempting to start it. ")
                                Thread.Sleep(5000)
                                StartService(VitalSignsPlusDomino)
                                Thread.Sleep(5000)
                            Catch ex As Exception

                            End Try
                        End If
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception #8: " & ex.ToString)
                End Try


                '*** If it is stuck in Stop Pending, which sometimes happens
                Try
                    If ServiceStatus(VitalSignsPlusDomino) = "Stop Pending" Then
                        WriteAuditEntry(Now.ToString & " The VitalSignsPlusDomino monitor service is stuck in StopPending mode, the process will be killed.")
                        KillServiceProcess(VitalSignsPlusDomino)
                        KillNotes()
                        Thread.Sleep(5 * 1000)
                        If boolDominoService Then
                            Try
                                StartService(VitalSignsPlusDomino)
                                WriteAuditEntry(Now.ToString & " TheVitalSignsPlusDomino monitor service status is " & ServiceStatus(VitalSignsPlusDomino))
                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " Exception #9a: " & ex.ToString)
                            End Try
                        End If
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception #9: " & ex.ToString)
                End Try

                '*** If it is stuck in Start Pending, which sometimes happens
                Try
                    If ServiceStatus(VitalSignsPlusDomino) = "Start Pending" Then
                        WriteAuditEntry(Now.ToString & " The VitalSignsPlusDomino monitor service is Start Pending so I'm going to give it some time before I kill it.")
                        Thread.Sleep(90 * 1000)
                        If ServiceStatus(VitalSignsPlusDomino) = "Start Pending" Then

                            WriteAuditEntry(Now.ToString & " The VitalSignsPlusDomino monitor service is stuck in StartPending mode, the process will be killed.")

                            KillServiceProcess(VitalSignsPlusDomino)
                            Thread.Sleep(5 * 1000)
                            KillNotes()
                            Thread.Sleep(2 * 1000)
                        Else
                            WriteAuditEntry(Now.ToString & " The VitalSignsPlusDomino monitor service started up. ")
                        End If

                        If boolDominoService Then
                            Try
                                StartService(VitalSignsPlusDomino)
                                WriteAuditEntry(Now.ToString & " The VitalSignsPlusDomino monitor service status is " & ServiceStatus(VitalSignsPlusDomino))
                            Catch ex As Exception

                            End Try
                        End If
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception #10: " & ex.ToString)
                End Try


                '*** If it is running, but hung, try to detect this and fix it.
                Dim MyUserName As String = ""
                StatusDate = Now.ToShortDateString
                Try
                    StatusDate = CType(myRegistry.ReadFromRegistry("Service Status"), DateTime)
                    MyUserName = myRegistry.ReadFromRegistry("Current User Name")
                Catch ex As Exception
                    StatusDate = Now.AddDays(-2).ToShortDateString
                End Try


                Dim span As TimeSpan = StatusDate.Subtract(DateTime.Now)
                If boolDominoService Then
                    Try
                        If span.TotalMinutes > 10 Or MyUserName = "" Then
                            Thread.Sleep(1000)
                            Try
                                MyUserName = myRegistry.ReadFromRegistry("Current User Name")
                            Catch ex As Exception
                                MyUserName = ""
                            End Try

                            If span.TotalMinutes > 10 And MyUserName = "" Then
                                'The service is likely to be hung
                                Try
                                    WriteAuditEntry(Now.ToString & "  VitalSignsPlusDomino Service appears to be hung, or the Lotus COM classes are hung, so the process will be killed and restarted. ")
                                    WriteAuditEntry(Now.ToString & " VitalSignsPlusDomino hasn't updated in " & span.TotalMinutes.ToString("F1") & " minutes.")

                                Catch ex As Exception

                                End Try
                                Try
                                    StopService(VitalSignsPlusDomino)
                                    Thread.Sleep(1000 * 30)

                                Catch ex As Exception
                                    WriteAuditEntry(Now.ToString & " Error restarting monitoring service: " & ex.ToString)
                                End Try

                                Try
                                    If ServiceStatus(VitalSignsPlusDomino) = "Stop Pending" Then
                                        WriteAuditEntry(Now.ToString & " The VitalSignsPlusDomino service is stuck in StopPending mode, the process will be killed.")
                                        KillServiceProcess(VitalSignsPlusDomino)
                                        Thread.Sleep(5 * 1000)
                                        KillNotes()
                                        Thread.Sleep(2 * 1000)
                                    End If
                                Catch ex As Exception

                                End Try

                                Try
                                    StartService(VitalSignsPlusDomino)
                                    WriteAuditEntry(Now.ToString & " The VitalSignsPlusDomino service status is " & ServiceStatus(VitalSignsPlusDomino))
                                Catch ex As Exception

                                End Try
                            End If
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Exception #11: " & ex.ToString)
                    End Try
                End If



                Try
                    If boolEXJournal = True Then
                        WriteAuditEntry(Now.ToString & " The status of the EXJournal service is " & ServiceStatus(EXJournalServiceName))
                        If ServiceRunning(EXJournalServiceName) = False Then
                            StartService(EXJournalServiceName)
                        End If

                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception starting the EXJournal service: " & ex.ToString)
                End Try


                Try
                    If boolDominoConsoleCommands = True Then
                        WriteAuditEntry(Now.ToString & " The status of the Console Command service is " & ServiceStatus(VitalSignsConsoleCommands))
                        If ServiceRunning(VitalSignsConsoleCommands) = False Then
                            StartService(VitalSignsConsoleCommands)
                        End If

                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception starting the Console Command service: " & ex.ToString)
                End Try

                Try
                    If boolMicrosoftService = True Then
                        WriteAuditEntry(Now.ToString & " The status of the Microsoft service is " & ServiceStatus(VitalSignsMicrosoft))
                        If ServiceRunning(VitalSignsMicrosoft) = False Then
                            StartService(VitalSignsMicrosoft)
                        End If

                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception starting the Exchange service: " & ex.ToString)
                End Try

                Try
                    If boolCore64Service = True Then
                        WriteAuditEntry(Now.ToString & " The status of the Core 64 service is " & ServiceStatus(VitalSignsCore64))
                        If ServiceRunning(VitalSignsCore64) = False Then
                            StartService(VitalSignsCore64)
                        End If

                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception starting the Core 64 service: " & ex.ToString)
                End Try

                'Stops the Alerting and DBHealth services if not primary
                Try
                    If boolIsPrimary = False Then
                        WriteAuditEntry(Now.ToString & " The status of the Alerting service is " & ServiceStatus(VitalSignsPlusAlerting) & "...Should be stopped")
                        If ServiceRunning(VitalSignsPlusAlerting) = True Then
                            StopService(VitalSignsPlusAlerting)
                        End If
                        WriteAuditEntry(Now.ToString & " The status of the DB Health service is " & ServiceStatus(VitalSignsPlusDBHealthService) & "...Should be stopped")
                        If ServiceRunning(VitalSignsPlusDBHealthService) = True Then
                            StopService(VitalSignsPlusDBHealthService)
                        End If
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception starting the Core 64 service: " & ex.ToString)
                End Try

                SetBooleansOfServicesFromSelectedFeatures()

                Try
                    boolEXJournal = myRegistry.ReadFromRegistry("Enable ExJournal")
                Catch ex As Exception
                    boolEXJournal = False
                End Try

                Try
                    boolDominoConsoleCommands = myRegistry.ReadFromRegistry("Enable Domino Console Commands")
                Catch ex As Exception
                    boolDominoConsoleCommands = False
                End Try

                'stops the services depending on the boolean and if the service is running or not
                StopBooleanControledService(boolDominoService, VitalSignsPlusDomino)
                StopBooleanControledService(boolMicrosoftService, VitalSignsMicrosoft)
                StopBooleanControledService(boolEXJournal, EXJournalServiceName)
                StopBooleanControledService(boolDominoConsoleCommands, VitalSignsConsoleCommands)
                StopBooleanControledService(boolCore64Service, VitalSignsCore64)

                Thread.Sleep(1000 * 60)   ' wait 60 seconds


            End If
        Loop

    End Sub

    'Protected Sub WriteAuditEntries()

    '    Dim myRegistry As New RegistryHandler


    '    Try
    '        strLogDest = strAppPath & "\Log_Files\Master_Service_Log.txt"

    '    Catch ex As Exception

    '    End Try

    '    'Try
    '    '    File.Copy(strAppPath & "Data\Master_Service_Log.txt", strAppPath & "Data\backup\Master_Service_" & Now.Month.ToString & "_" & Now.Day.ToString & "_" & Now.Hour & "_" & Now.Minute & ".txt")
    '    'Catch ex As Exception

    '    'End Try

    '    'Try
    '    '    If File.Exists(strLogDest) Then
    '    '        File.Delete(strLogDest)
    '    '    End If
    '    'Catch ex As Exception
    '    '    ' strLogDest = "c:\vitalsignslog.txt"
    '    'End Try

    '    myRegistry = Nothing

    '    'Dim elapsed As TimeSpan
    '    Dim OneHour As Integer = 60 * 60 * -1  'seconds
    '    Dim dtAuditHistory As DateTime

    '    ' Dim myLastUpdate As TimeSpan

    '    Try
    '        dtAuditHistory = Now
    '        Thread.Sleep(1000)
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

    '                End If
    '            Catch ex As Exception '

    '            Finally
    '                sw.Flush()
    '                sw.Close()
    '            End Try

    '            Thread.Sleep(4250)


    '        Loop

    '    Catch ExAbort As ThreadAbortException
    '        '      sw.Write(" Shutting down, closing log")
    '        '     sw.Close()
    '        '    sw = Nothing
    '    Catch ex As Exception

    '    End Try


    'End Sub

    Protected Sub SendPulse()
        Dim NodeName As String = ""
        Dim strSQL As String


        'Gets the version of the Master Service to compare with the DB
        Dim productVersion As String = ""
        Try
            Dim versInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.BaseDirectory.ToString() & "VitalSignsMasterService.exe")
            productVersion = versInfo.ProductMajorPart.ToString() + "." + versInfo.ProductMinorPart.ToString() + "." + versInfo.ProductBuildPart.ToString()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in SendPulse while getting the Product Version.  Error: " & ex.Message.ToString())
        End Try

        'Gets the version of the VS scripts last executed
        'Dim dbVersion As String = ""
        'Try
        '    strSQL = "select Value from VS_MANAGEMENT WHERE Category='VS_VERSION'"
        '    Dim ds As New DataSet()
        '    ds.Tables.Add("VersionValue")

        '    vsobj.FillDatasetAny("VitalSigns", "VitalSigns", strSQL, ds, "VersionValue")

        '    dbVersion = ds.Tables("VersionValue").Rows(0)(0).ToString()

        'Catch ex As Exception
        '    WriteAuditEntry(Now.ToString & " Error in SendPulse while getting the VS DB Version.  Error: " & ex.Message.ToString())
        'End Try

        Dim VSWebVersion As String = ""
        Try
            Dim VersionVSWeb As FileVersionInfo = FileVersionInfo.GetVersionInfo("C:\inetpub\wwwroot\VSWeb\bin\VSWebBL.dll")
            VSWebVersion = VersionVSWeb.ProductMajorPart.ToString() + "." + VersionVSWeb.ProductMinorPart.ToString()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in SendPulse while getting the VSWeb Version.  Error: " & ex.Message.ToString())
        End Try

        'Gets the host name for the 
        Dim hostname As String = ""
        Try
            hostname = System.Net.Dns.GetHostName()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in SendPulse while getting the Host Name.  Error: " & ex.Message.ToString())
        End Try


        Dim serviceNames As New Collections.Generic.List(Of String) From {
           VitalSignsPlusDomino,
           VitalSignsPlusCore,
           VitalSignsPlusAlerting,
           VitalSignsPlusClusterHealth,
           VitalSignsDailyService,
           MasterServiceName,
           VitalSignsPlusDBHealthService,
           EXJournalServiceName,
           VitalSignsConsoleCommands,
           VitalSignsMicrosoft,
           VitalSignsCore64
          }

        Dim friendlyNames As New Collections.Generic.List(Of String) From {
         "VSService_Domino",
         "VSService_Core",
         "VSService_Alerting",
         "VSService_Cluster Health",
         "VSService_Daily Service",
         "VSService_Master Service",
         "VSService_DB Health",
         "VSService_EX Journal",
         "VSService_Console Commands",
         "VSService_Microsoft",
         "VSService_Core 64-bit"
        }

        Dim status As String
        Do
            If NodeName = "" Then
                If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") Is Nothing Then
                    WriteAuditEntry(Now.ToString & " The VSNodeName is not defined in the config file. Cannot send pulse until it is defined.")
                Else
                    NodeName = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
                End If
            End If

            If NodeName <> "" Then
                Try
                    Dim vsLic As New VitalSignsLicensing.Licensing
                    vsLic.doMasterPing(NodeName, hostname)
                    'Dim isConfiged As Integer = 0
                    'If (VSWebVersion = dbVersion) Then
                    '    isConfiged = 1
                    'End If

                    'strSQL = " IF NOT EXISTS(SELECT * FROM Nodes WHERE Name='" & NodeName & "') "
                    'strSQL += " INSERT INTO Nodes (Name, IsConfiguredPrimaryNode) VALUES ('" & NodeName & "','" & isConfiged.ToString() & "') "


                    'Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Nodes)(connectionString)
                    'Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Nodes) = repository.Filter.Eq(Function(x) x.Name, NodeName)
                    'Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Nodes) = repository.Updater.Set(Function(x) x.Pulse, Now)

                    'If (hostname <> "") Then
                    '    updateDef = updateDef.Set(Function(x) x.HostName, hostname)
                    'End If
                    ''If (productVersion <> "") Then
                    ''    updateDef = updateDef.Set(Function(x) x.Version, productVersion)
                    ''End If

                    'repository.Update(filterDef, updateDef)

                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Error in SendPulse while updating Nodes table.  Error: " & ex.Message.ToString())
                End Try
                'If (VSWebVersion = dbVersion) Then
                '    Try
                '        strSQL = "Update Nodes Set IsConfiguredPrimaryNode = 1 WHERE Name='" & NodeName & "' "
                '    Catch ex As Exception

                '    End Try
                'End If
                Dim list As New List(Of VSNext.Mongo.Entities.ServiceStatus)

                For i As Integer = 0 To serviceNames.Count - 1
                    Try

                        status = ServiceStatus(serviceNames.Item(i))
                        list.Add(New VSNext.Mongo.Entities.ServiceStatus() With {
                                 .Name = friendlyNames(i).ToString(),
                                 .State = status
                                 })
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error in SendPulse while making sql for NodeDetails table.  Error: " & ex.Message.ToString())
                    End Try

                Next

                Try

                    Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Nodes)(connectionString)
                    Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Nodes) = repository.Filter.Eq(Function(x) x.Name, NodeName)
                    Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Nodes) = repository.Updater.Set(Function(x) x.ServiceStatus, list)
                    repository.Update(filterDef, updateDef)

                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Error in SendPulse while updating service status.  Error: " & ex.Message.ToString())
                End Try



            End If
            ' do system messages thread
            'doDBSystemMessages()
            Thread.Sleep(1000 * 60)
        Loop

    End Sub
    'Private Sub doDBSystemMessages()
    '    Try
    '        Dim strSQL As String = "select ID,Details,MessageType from SystemMessagesTemp "
    '        Dim ds As New DataSet()
    '        ds.Tables.Add("SystemMessagesTemp")
    '        Dim id As String = ""
    '        vsobj.FillDatasetAny("VitalSigns", "VitalSigns", strSQL, ds, "SystemMessagesTemp")
    '        For Each dr As DataRow In ds.Tables(0).Rows
    '            If dr(2).ToString() = "1" Or dr(2).ToString().ToLower() = "true" Then
    '                myAlert.QueueSysMessage(dr(1).ToString())
    '                id = dr(0).ToString()
    '            Else
    '                myAlert.ResetSysMessage(dr(1).ToString())
    '                id = dr(0).ToString()
    '            End If
    '            'delete the message
    '            If id <> "" Then
    '                vsobj.ExecuteScalarAny("VitalSigns", "", "DELETE FROM SystemMessagesTemp WHERE ID=" + id)
    '            End If
    '        Next
    '        ds.Dispose()
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " Error in Generating System Messages.  Error: " & ex.Message.ToString())
    '    End Try
    'End Sub

    Protected Sub MonitorNotResponding()
        Dim lastID As Integer = -2  'setting to -2 so if there are no entries, the maxID will be -1 and then it will end the loop and pick up entry 0
        Dim sql As String = ""
        Dim ds As New DataSet()
        Dim TypeANDName As String
        Do
            Try
                sql = "SELECT ISNULL(MAX(ID),-1) FROM Status_History"

                ds.Tables.Add("Max ID")
                vsobj.FillDatasetAny("VitalSigns", "VitalSigns", sql, ds, "Max ID")
                lastID = Convert.ToInt32(ds.Tables("Max ID").Rows(0)(0).ToString())
                ds.Tables.Remove("Max ID")
                WriteAuditEntry(Now.ToString & " In.  ID is " & lastID)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error in MonitorNotResponding while getting the max id.  Error: " & ex.Message.ToString())
            End Try

            If (lastID <> -1) Then
                Exit Do
            End If
            Thread.Sleep(5000)
        Loop

        ds.Tables.Add("Status_History")

        Do
            Try
                ds.Tables("Status_History").Clear()

                sql = "SELECT ID, TypeANDName FROM Status_History WHERE ID > " & lastID & " and NewStatusCode = 'Not Responding' order by ID"
                vsobj.FillDatasetAny("VitalSigns", "VitalSigns", sql, ds, "Status_History")

                If ds.Tables("Status_History").Rows.Count > 0 Then

                    For Each row As DataRow In ds.Tables("Status_History").Rows
                        TypeANDName = row("TypeANDName").ToString()

                        WriteEntryForNotResponding(TypeANDName)
                    Next
                    If (ds.Tables("Status_History").Rows.Count > 0) Then
                        lastID = Convert.ToInt64(ds.Tables("Status_History").Rows(ds.Tables("Status_History").Rows.Count - 1)("ID").ToString())
                    End If
                End If

                'WriteAuditEntry(Now.ToString & " In loop.  ID is " & lastID)

            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error in MonitorNotResponding loop.  Error: " & ex.Message.ToString())
            End Try

            Thread.Sleep(30000)
        Loop

    End Sub

#End Region
#Region "GetDiskSize"

    Public Function GetHDDetails(ByVal strDrive As String) As ManagementObject 'Get Size of Specified Disk

        'Ensure Valid Drive Letter Entered, Else, Default To C
        If strDrive = "" OrElse strDrive Is Nothing Then

            strDrive = "C"

        End If

        ' Make Use Of Win32_LogicalDisk To Obtain Hard Disk Properties
        Dim moHD As New ManagementObject("Win32_LogicalDisk.DeviceID=""" + strDrive + ":""")

        'Get Info
        moHD.[Get]()

        'Return Hard Disk Details
        Return moHD

    End Function

    'Public Function GetHDSize(ByVal strDrive As String) As Double 'Get Size of Specified Disk

    '    'Ensure Valid Drive Letter Entered, Else, Default To C
    '    If strDrive = "" OrElse strDrive Is Nothing Then

    '        strDrive = "C"

    '    End If

    '    ' Make Use Of Win32_LogicalDisk To Obtain Hard Disk Properties
    '    Dim moHD As New ManagementObject("Win32_LogicalDisk.DeviceID=""" + strDrive + ":""")

    '    'Get Info
    '    moHD.[Get]()

    '    'Get Hard Disk Size
    '    Return Convert.ToDouble(moHD("Size"))

    'End Function

    'Public Function GetHDFreeSpace(ByVal strDrive As String) As Double

    '    'Ensure Valid Drive Letter Entered, Else, Default To C
    '    If strDrive = "" OrElse strDrive Is Nothing Then

    '        strDrive = "C"

    '    End If

    '    'Make Use Of Win32_LogicalDisk To Obtain Hard Disk Properties
    '    Dim moHD As New ManagementObject("Win32_LogicalDisk.DeviceID=""" + strDrive + ":""")

    '    'Get Info
    '    moHD.[Get]()

    '    'Get Hard Disk Free Space
    '    Return Convert.ToDouble(moHD("FreeSpace"))

    'End Function
#End Region

#Region "Start/ Stop Service"

    Private Sub StartService(ByVal ServiceName As String)

        If ServiceName = VitalSignsPlusAlerting Or ServiceName = VitalSignsPlusDBHealthService Then
            Dim sql As String
            Dim isPriamry As Boolean = True
            Try

                If Not (System.Configuration.ConfigurationManager.AppSettings("VSNodeName") Is Nothing) Then

                    'Dim myConnectionString As New VSFramework.XMLOperation

                    Dim NodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
                    'sql = "SELECT IsPrimaryNode From Nodes WHERE Name='" & NodeName & "'"

                    'Dim dt As DataTable = vsobj.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sql)
                    'If (dt.Rows.Count > 0) Then
                    '    isPriamry = Convert.ToBoolean(dt.Rows(0)(0).ToString())
                    'End If
                    Dim repoLiveNodes As New VSNext.Mongo.Repository.Repository(Of Nodes)(connectionString)
                    Dim nodesListAlive As List(Of Nodes) = repoLiveNodes.Find(Function(i) i.Name = NodeName).ToList()
                    For Each n As Nodes In nodesListAlive
                        If n.IsPrimary Then
                            isPriamry = True
                        End If
                    Next
                End If

            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error in StartService checking if Priamry Node.  Error: " & ex.Message)
            End Try

            If Not isPriamry Then
                Return
            End If

        End If
        ' Find service
        WriteAuditEntry(Now.ToString & " Starting the " & ServiceName & " service.")
        Dim objServiceController As New ServiceController(ServiceName, ".")
        Try
            ' See if the service is valid on this machine
            If (objServiceController Is Nothing) Then
                WriteAuditEntry(Now.ToString & " Ooops!  Big problem-- I could not find the " & ServiceName & "  service on this machine, so I couldn't start it.")
                Return
            End If
        Catch ex As Exception

        End Try


        Try
            If ServiceStatus(ServiceName) = "Stop Pending" Then
                WriteAuditEntry(Now.ToString & " The  service is stuck in StopPending mode, the process will be killed.")
                KillServiceProcess(ServiceName)
                Thread.Sleep(5 * 1000)
            End If
        Catch ex As Exception

        End Try

        Try
            ' See if it's stopped
            Dim bServiceStartStatus As Boolean = False
            If (objServiceController.Status = ServiceControllerStatus.Stopped) Then
                ' Start the service
                Try
                    bServiceStartStatus = True
                    '    objServiceController.DisplayName = ProductName & " monitors Domino, BlackBerry, and other mail services."
                    objServiceController.Start()
                    ' Now wait for the service to start (10 seconds)
                    objServiceController.WaitForStatus(ServiceControllerStatus.Running, New TimeSpan(0, 0, 45))

                Catch ex As Exception
                    bServiceStartStatus = False
                    WriteAuditEntry(Now.ToString & " Unable To start " & ServiceName & "  service because " & vbCrLf & ex.Message)
                End Try
                'send system message if the service cannot be started
                Try
                    If ServiceName <> "VitalSigns Daily Tasks" Then
                        If bServiceStartStatus Then
                            Dim msg As String = "Could not start the service: " + ServiceName
                            myAlert.ResetSysMessage(msg)
                        Else
                            Dim msg As String = "Could not start the service: " + ServiceName
                            myAlert.QueueSysMessage(msg)
                            'VSPLUS-2800. comment out the emergency alerts part
                            'sendEmergencyAlert(ServiceName)
                        End If
                    End If

                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Error trying to send emergency alert for service: " & ServiceName & " Exception:" + ex.Message.ToString())
                End Try
            Else
                ' Service was already running
                WriteAuditEntry(Now.ToString & " No need to start " & ServiceName & " as it was already running.")
            End If
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Exception at #6: " & ex.ToString)
        End Try

        WriteAuditEntry(Now.ToString & " Done starting the " & ServiceName & " service.")

    End Sub

    Private Sub sendEmergencyAlert(serviceName As String)
        Dim myAlertService As New VitalSignsAlertService.VitalSignsAlertService
        myAlertService.GetEmergencyAlertInfo()
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

        ' Periodically check if the service is stopping. 

        ' Perform main service function here... 
        '1/6/2014 NS added a check to see if the alerting service should be sending alerts
        Dim alertson As Boolean
        alertson = True

        Try
            Dim myRegistry As New VitalSignsAlertService.RegistryHandler()
            '1/6/2014 NS added a check to see if the alerting service should be sending alerts
            '12/16/2014 NS modified for VSPLUS-1267
            Dim sqlstm As String = myRegistry.ReadFromRegistry("AlertsOn")
            'Dim DA2 As New SqlDataAdapter(sqlstm, con)
            'Dim DS2 As New DataSet
            'DA2.Fill(DS2, "svalue")
            'Dim dtmail As DataTable = DS2.Tables(0)
            Dim dtmail As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sqlstm)
            If dtmail.Rows.Count > 0 Then
                alertson = Convert.ToBoolean(dtmail.Rows(0)("svalue").ToString())
            End If

        Catch ex As Exception
        End Try
        WriteAuditEntry(Now.ToString & " Sending emergency alert for service:" & serviceName, LogUtilities.LogUtils.LogLevel.Normal)
        If (alertson) Then
            Try
                '1/6/2014 NS added - we only want to send alerts if the flag is enabled, otherwise, continue with clearing and keep checking
                Dim myRegistry As New VitalSignsAlertService.RegistryHandler()
                If Not myRegistry Is Nothing Then
                    tempObj = myRegistry.ReadFromRegistry("Alert Emergency Contacts")
                    If Not tempObj Is Nothing Then
                        emergencyContacts = tempObj
                        If emergencyContacts.Length > 0 Then
                            emergencyContacts = emergencyContacts.Substring(0, emergencyContacts.Length - 1)
                        End If
                    End If

                    tempObj = myRegistry.ReadFromRegistry("Alert Emergency PrimaryHostName")
                    If Not tempObj Is Nothing Then
                        PHostName = tempObj
                    End If
                    tempObj = myRegistry.ReadFromRegistry("Alert Emergency primaryport")
                    If Not tempObj Is Nothing Then
                        Pport = tempObj
                    End If
                    tempObj = myRegistry.ReadFromRegistry("Alert Emergency primaryUserID")
                    If Not tempObj Is Nothing Then
                        PEmail = tempObj
                    End If
                    tempObj = myRegistry.ReadFromRegistry("Alert Emergency primarypwd")
                    If Not tempObj Is Nothing Then
                        Ppwd = tempObj
                    End If
                    tempObj = myRegistry.ReadFromRegistry("Alert Emergency primaryFrom")
                    If Not tempObj Is Nothing Then
                        PFrom = tempObj
                    End If
                    tempObj = myRegistry.ReadFromRegistry("Alert Emergency primaryAuth")
                    If Not tempObj Is Nothing Then
                        If tempObj.ToString() <> "" Then
                            '12/3/2015 NS modified for VSPLUS-1562
                            PAuth = Convert.ToBoolean(tempObj.ToString())
                        End If
                    End If
                    tempObj = myRegistry.ReadFromRegistry("Alert Emergency primarySSL")
                    If Not tempObj Is Nothing Then
                        If tempObj.ToString() <> "" Then
                            '12/8/2015 NS modified for VSPLUS-1562
                            PSSL = Convert.ToBoolean(tempObj.ToString())
                        End If
                    End If
                End If
                Dim sSubject As String = "Service : " + serviceName + " Cannot be started."
                WriteAuditEntry(Now.ToString & " Trying to send an email:" & sSubject, LogUtilities.LogUtils.LogLevel.Normal)
                If emergencyContacts <> "" Then
                    myAlertService.SendMailNet(PHostName, Pport, PAuth, False, Ppwd, PEmail, PSSL, "The Service: " + serviceName + " cannot be started in a timely fashion. Please contact the Vital Signs system administrator and rectify the issue immediately.", False, "", "", "", emergencyContacts, "", "", PFrom, True, sSubject)
                End If

            Catch ex As Exception
                WriteAuditEntry(Now.ToString & "Exception: Trying to send an email: exception:" + ex.Message.ToString(), LogUtilities.LogUtils.LogLevel.Normal)
            End Try
        End If

    End Sub
    Private Sub StopService(ByVal ServiceName As String)
        ' Find service
        Dim objServiceController As New ServiceController(ServiceName, ".")
        WriteAuditEntry(Now.ToString & " Stopping the " & ServiceName & " service.")

        ' See if the service is valid on this machine
        If (objServiceController Is Nothing) Then
            WriteAuditEntry(Now.ToString & " Ooops!  Big problem-- I could not find the " & ServiceName & "  service on this machine, so I couldn't stop it.")
            Return
        End If

        ' See if it's running
        If (objServiceController.Status = ServiceControllerStatus.Running) Then
            ' Start the service
            Try
                objServiceController.Stop()
                ' Now wait for the service to stop (10 seconds)
                objServiceController.WaitForStatus(ServiceControllerStatus.Stopped, New TimeSpan(0, 0, 30))

                '1/5/16 WS Added for 2275
            Catch ex As ServiceProcess.TimeoutException
                'Timeout occured, will now end process
                KillServiceProcess(ServiceName)
                If ServiceName = VitalSignsPlusDomino Or ServiceName = VitalSignsPlusDBHealthService Or ServiceName = VitalSignsConsoleCommands Then
                    KillNotes()
                End If
            Catch ex As Exception

            End Try
        ElseIf (objServiceController.Status = ServiceControllerStatus.Stopped) Then
            ' Service wasn't started
        Else
            '1/5/16 WS Added for 2275
            ' Service is in a hung state
            KillServiceProcess(ServiceName)
        End If

    End Sub

#End Region


    Private Function ServiceRunning(ByVal MyServiceName As String) As Boolean

        If MyServiceName = "" Then MyServiceName = "VitalSigns Monitoring Service"

        Try
            Dim objServiceController As New ServiceController(MyServiceName, ".")
            ' See if the service is valid on this machine
            Try
                WriteAuditEntry(Now.ToString & " The status of " & MyServiceName & " is " & objServiceController.Status.ToString)
            Catch ex As Exception

            End Try
            If (objServiceController Is Nothing) Then
                Return False
            End If
            If objServiceController.Status = ServiceControllerStatus.Running Then
                Return True
            ElseIf objServiceController.Status = ServiceControllerStatus.StartPending Then
                Return False
            ElseIf objServiceController.Status = ServiceControllerStatus.Stopped Then
                Return False
            ElseIf objServiceController.Status = ServiceControllerStatus.StopPending Then
                Return False
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try


    End Function

    Private Function ServiceStatus(ByVal MyServiceName As String) As String

        If MyServiceName = "" Then MyServiceName = "VitalSigns Monitoring Service"

        Try
            Dim objServiceController As New ServiceController(MyServiceName, ".")
            ' See if the service is valid on this machine
            If (objServiceController Is Nothing) Then
                Return "Not Found"

            End If

            If objServiceController.Status = ServiceControllerStatus.Running Then
                Return "Running"
            ElseIf objServiceController.Status = ServiceControllerStatus.StartPending Then
                Return "Start Pending"

            ElseIf objServiceController.Status = ServiceControllerStatus.Stopped Then
                Return "Stopped"
            ElseIf objServiceController.Status = ServiceControllerStatus.StopPending Then
                Return "Stop Pending"
            Else
                Return "Stopped"
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub KillNotes()
        Try
            WriteAuditEntry(Now.ToString & " Killing SUService.")
            KillProcess("SUService")
        Catch ex As Exception

        End Try
        Thread.Sleep(2000)
        Try
            WriteAuditEntry(Now.ToString & " Killing all Notes processes.")
            Dim myApp As String = strAppPath & "stopnotescl.exe /q"
            Shell(myApp, AppWinStyle.Hide, False)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub KillProcess(ByVal ProcessName As String)

        For Each p As System.Diagnostics.Process In System.Diagnostics.Process.GetProcessesByName(ProcessName)
            Try
                p.Kill()
            Catch ex As Exception

            End Try

        Next
    End Sub
    Private Sub KillProcessAndChildren(pid As Integer)
        Dim searcher As New ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid.ToString())
        Dim moc As ManagementObjectCollection = searcher.[Get]()
        For Each mo As ManagementObject In moc
            KillProcessAndChildren(Convert.ToInt32(mo("ProcessID")))
        Next
        Try
            Dim proc As Process = Process.GetProcessById(pid)
            proc.Kill()
            ' Process already exited.
        Catch generatedExceptionName As ArgumentException
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error Killing the Process and it's children.  Error: " & ex.Message)

        End Try
    End Sub
    Private Sub KillServiceProcess(ByVal ServiceName As String)
        '1/5/16 WS Added for 2275

        Dim query As String = "SELECT ProcessId FROM Win32_Service WHERE Name='" & ServiceName & "'"

        Dim searcher As New ManagementObjectSearcher(query)

        For Each obj As ManagementObject In searcher.Get()

            Dim ProcessID As Integer = obj("ProcessId")
            Dim process As Process = Nothing

            Try
                process = process.GetProcessById(ProcessID)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Could not find process for " & ServiceName)
            End Try

            If Not (process Is Nothing) And process.ProcessName <> "Idle" And process.ProcessName <> "svchost" Then
                WriteAuditEntry(Now.ToString & " Killing process " & process.ProcessName)
                'KillProcess(process.ProcessName)
                KillProcessAndChildren(process.Id)
            End If


        Next


    End Sub


#Region "Log Files"


    Public Overloads Sub WriteAuditEntry(ByVal strMsg As String, Optional ByVal logLevel As LogUtilities.LogUtils.LogLevel = LogUtilities.LogUtils.LogLevel.Normal)
        MyBase.WriteAuditEntry(strMsg, "Master_Service_Log.txt", logLevel)
    End Sub

    Public Overloads Sub WriteEntryForNotResponding(ByVal TypeANDName As String)
        MyBase.WriteEntryForNotResponding(TypeANDName)
    End Sub

    'Public Sub WriteAuditEntry(ByVal strMsg As String)
    '    strAuditText += strMsg & vbCrLf
    'End Sub



#End Region


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

    Public Sub SetBooleansOfServicesFromSelectedFeatures()
        'pulls the set of features that are enabled to be monitored
        Try
            Dim strSQL As String = "SELECT f.Name FROM Features f INNER JOIN SelectedFeatures AS sf ON sf.FeatureID=f.ID"
            Dim dt As New DataTable
            Dim ds As New DataSet
            dt.TableName = "SelectedFeatures"
            ds.Tables.Add(dt)

            Try
                vsobj.FillDatasetAny("VitalSigns", "vitalsigns", strSQL, ds, "SelectedFeatures")
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error loading selected features. Error: " & ex.ToString)
            End Try

            Dim SelectedFeaturesDataView As New DataView(ds.Tables("SelectedFeatures"))
            SelectedFeaturesDataView.Sort = "Name"

            If SelectedFeaturesDataView.Find("Domino") <> -1 Then
                boolDominoService = True
            Else
                boolDominoService = False
            End If

			If SelectedFeaturesDataView.Find("Exchange") <> -1 Or SelectedFeaturesDataView.Find("Skype for Business") <> -1 Or SelectedFeaturesDataView.Find("Active Directory") <> -1 Or SelectedFeaturesDataView.Find("SharePoint") <> -1 Or SelectedFeaturesDataView.Find("Windows") <> -1 Or SelectedFeaturesDataView.Find("Office 365") <> -1 Then
                boolMicrosoftService = True
            Else
                boolMicrosoftService = False
            End If

            If SelectedFeaturesDataView.Find("Network Devices") <> -1 Then
                boolCore64Service = True
            Else
                boolCore64Service = False
            End If

            Try

                If Not (System.Configuration.ConfigurationManager.AppSettings("VSNodeName") Is Nothing) Then

                    'Dim myConnectionString As New VSFramework.XMLOperation

                    Dim NodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
                    'strSQL = "SELECT IsPrimaryNode From Nodes WHERE Name='" & NodeName & "'"

                    'dt = vsobj.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), strSQL)
                    'If (dt.Rows.Count > 0) Then
                    '    boolIsPrimary = Convert.ToBoolean(dt.Rows(0)(0).ToString())
                    'End If
                    Dim repoLiveNodes As New VSNext.Mongo.Repository.Repository(Of Nodes)(connectionString)
                    Dim nodesListAlive As List(Of Nodes) = repoLiveNodes.Find(Function(i) i.Name = NodeName).ToList()
                    For Each n As Nodes In nodesListAlive
                        If n.IsPrimary Then
                            boolIsPrimary = True
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error in SetBooleansOfServices checking if Priamry Node.  Error: " & ex.Message)
            End Try

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error setting booleans for services to monitor.  Error: " & ex.ToString)
        End Try

    End Sub

    Public Sub StopBooleanControledService(ByVal Monitoring As Boolean, ByVal ServiceName As String)

        'stops boolean powered services.  Monitoring is the global boolean for a given service and the service name is the global service string name
        If Monitoring <> True And ServiceStatus(ServiceName) = "Running" Then
            Try
                WriteAuditEntry(Now.ToString & " The service " & ServiceName & " is no longer needed and will stop.")
                StopService(ServiceName)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error when trying to stop " & ServiceName & ".  Error: " & ex.ToString)
            End Try
        End If
    End Sub


    Private Function FixDate(ByVal dt As DateTime) As String
        ' Return dt.ToUniversalTime.ToString
        'WriteAuditEntry(Now.ToString & " FixDateTime, strDateFormat: " & strDateFormat)

        'Mukund 28May14: Required to get current date format of the SQL Server 
        Try
            'WriteAuditEntry(Now.ToString & " Querying SQL server for the current date format.", LogLevel.Normal)
            strDateFormat = objDateUtils.GetDateFormat()
            'WriteAuditEntry(Now.ToString & " The current date format is " & strDateFormat, LogLevel.Normal)
        Catch ex As Exception

        End Try


        'Mukund 28May14: FixDate called to convert date to current date format of the SQL Server, using strDateFormat
        Dim strdt As String
        strdt = objDateUtils.FixDate(dt, strDateFormat)

        'WriteAuditEntry(Now.ToString & " FixDateTime, ret dt " & strDateFormat & ":" & strdt)
        Return strdt

    End Function

    Private Sub ClearDatabaseTables()
        Dim sql As String
        Dim isPriamry As Boolean = False
        Try
            If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") Then
                Dim NodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
                'sql = "SELECT IsPrimaryNode From Nodes WHERE Name='" & NodeName & "'"
                'isPriamry = Convert.ToBoolean(vsobj.ExecuteNonQueryAny("VitalSigns", "VitalSigns", sql).ToString())

                Dim repoLiveNodes As New VSNext.Mongo.Repository.Repository(Of Nodes)(connectionString)
                Dim nodesListAlive As List(Of Nodes) = repoLiveNodes.Find(Function(i) i.Name = NodeName).ToList()
                For Each n As Nodes In nodesListAlive
                    If n.IsPrimary Then
                        isPriamry = True
                    End If
                Next

            End If

            If isPriamry Then
                'sql = "DELETE FROM Status"
                'vsobj.ExecuteNonQueryAny("VitalSigns", "VitalSigns", sql).ToString()
                Dim DailyStats As New VSNext.Mongo.Entities.Status
                Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim statusList As List(Of Status) = repo.All()
                For Each s As Status In statusList
                    Dim filterdef As MongoDB.Driver.FilterDefinition(Of VSNext.Mongo.Entities.Status) = repo.Filter.Where(Function(i) i.DeviceName.Equals(s.DeviceName))
                    repo.Delete(filterdef)
                Next
                
                Dim NotesMailProbeHistory As New VSNext.Mongo.Entities.NotesMailProbeHistory
                Dim repo1 As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.NotesMailProbeHistory)(connectionString)
                Dim NotesMailProbeHistoryList As List(Of NotesMailProbeHistory) = repo1.All()
                For Each s As NotesMailProbeHistory In NotesMailProbeHistoryList
                    Dim filterdef As MongoDB.Driver.FilterDefinition(Of VSNext.Mongo.Entities.NotesMailProbeHistory) = repo1.Filter.Where(Function(i) i.DeviceName.Equals(s.DeviceName))
                    repo1.Delete(filterdef)
                Next

                'sql = "DELETE FROM NotesMailProbeHistory"
                'vsobj.ExecuteNonQueryAny("VitalSigns", "VitalSigns", sql).ToString()

                Dim TravelerStats As New VSNext.Mongo.Entities.TravelerStats
                Dim repo2 As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.TravelerStats)(connectionString)
                Dim TravelerStatsList As List(Of TravelerStats) = repo2.All()
                For Each s As TravelerStats In TravelerStatsList
                    Dim filterdef As MongoDB.Driver.FilterDefinition(Of VSNext.Mongo.Entities.TravelerStats) = repo2.Filter.Where(Function(i) i.TravelerServerName.Equals(s.TravelerServerName))
                    repo2.Delete(filterdef)
                Next

                'sql = "DELETE FROM Traveler_Status"
                'vsobj.ExecuteNonQueryAny("VitalSigns", "VitalSigns", sql).ToString()
            End If

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in ClearDatabaseTables.  Error: " & ex.Message)
        End Try
    End Sub

    Private Sub UpdateInstallLocation()

        Try
            Dim Path As String = myRegistry.ReadFromVitalSignsComputerRegistry("InstallPath").ToString()
            If (Not (Path Is Nothing)) And Path <> "" Then
                myRegistry.WriteToRegistry("InstallLocation", Path)
            Else
                WriteAuditEntry(Now.ToString & " The install path came back as nothing.  Will not update the path.")
            End If

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in UpdateInstallLocation.  Error: " & ex.Message)
        End Try

    End Sub

End Class

