Imports System.ServiceProcess
Imports System.IO
Imports System.Text
Imports System.Threading
Imports Microsoft.Win32.Registry
Imports Microsoft.Win32
Imports System.Security.Cryptography
Imports nsoftware.IPWorks
'Imports Chilkat.MailMan  'Used for sending SMTP messages , see http://www.chilkatsoft.com/
Imports VSFramework
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Linq

Imports RPRWyatt.VitalSigns.Services


Imports MongoDB.Bson
Imports MongoDB.Driver

Imports VSNext.Mongo.Entities
'Written by Alan Forbes
'Copyright 2013


'Ashley was getting these queries  - need to clean them up
'15/01/2014 15:17:37 My SQL statement is 
'INSERT INTO Daily (Temp,  FileNamePath, ScanDate, Q_PlaceBotCount, Q_CustomFormCount, InboxDocCount, FileName, Title, FileSize, Server, DesignTemplateName, Quota, FTIndexed, EnabledForClusterReplication, EnabledForReplication, ReplicaID, ODS, Status, DocumentCount, CurrentAccessLevel, Details, Categories, IsPrivateAddressBook, IsPublicAddressBook, IsInService, FTIndexFrequency, Folder, PercentUsed, Created, LastFixup, LastFTIndexed, LastModified, IsMailFile, PersonDocID )  VALUES ( 1, 'mail\mgroom.nsf', '15/01/2014 15:17:37', 0, 0, 0, 'mgroom.nsf', 'Mark Groom', '9', 'BirmNotx/UK/Crawco-EU', 'CrawcoUKiNotes6', '0', 0, 0, 0, '8025777B0032CAAE', 0, 'OK', 0, '', 'Connected to the database at 15:17', '', 0, 0, 0, 'None', 'mail\', '0', '00:00:00', '00:00:00', '00:00:00', '00:00:00', 0, '')
'INSERT INTO Daily (Temp,Server, ScanDate, FileName, FileSize, Title, DesignTemplateName, Quota, FTIndexed,  EnabledForClusterReplication, ReplicaID, ODS ) VALUES (1, 'BirmNoty/UK/Crawco-EU', '15-Jan-2014', 'mail\gwright.nsf', '61', 'Gary Wright', 'Crawco853Mail', 0, 0,  0, '80257AFC003A7036', '0')



Public Class VitalSignsDBHealth
	Inherits VSServices
    Dim BannerText As String
    Dim ProductName As String
    Dim CompanyName As String = "RPR Wyatt, Inc."
    Dim EvalVersion As Boolean = False
    Dim boolHTML As Boolean = True
    Dim strLogDest, strAppPath, strAuditText, HTMLPath, StrStatisticsPath, strServersMDBPath As String
    Dim ListOfDominoThreads As List(Of Threading.Thread) = New List(Of Threading.Thread)
    Dim sCultureString As String = "en-US"
    Dim connectionStringName As String = "CultureString"
    Dim One_Minute As Integer = 60000

    'How many servers do you have to scan?
    Dim ActiveServerCount As Integer = 0

    Dim strDateFormat As String
    Dim objDateUtils As New DateUtils.DateUtils

    'The limit such that if you have more than this, we are going to do the short scan only
    ' Dim MaxServerLimit As Integer = 20
    Dim FastScan As Boolean = True

    'Store the passwords in these variables
    Dim MyDominoPassword As String
    Dim BoolOffHours As Boolean
    Dim LicenseCount As Long = 10000  ' bypass license counting
    Dim BuildNumber As Integer = 1065
    Dim boolTimeToStop As Boolean = False
    Dim Selector_Mutex As New Mutex


    'Determines the verbosity of the log file
	Enum LogLevel
		Verbose = LogUtilities.LogUtils.LogLevel.Verbose
		Debug = LogUtilities.LogUtils.LogLevel.Debug
		Normal = LogUtilities.LogUtils.LogLevel.Normal
	End Enum

    'MyLogLevel is used throughout to control the volume of the log file output
    Dim MyLogLevel As LogLevel

    'Collection of Domino servers to monitor
    Dim MyDominoServers As New MonitoredItems.DominoCollection
    Dim MyDominoServer As MonitoredItems.DominoServer

    ' Dim WithEvents SNMPAgent As New nsoftware.IPWorks.Snmpagent("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000")
    Friend WithEvents OleDBConnectionServers As New System.Data.OleDb.OleDbConnection

    Dim connectionString As String = ""

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
        ServicesToRun = New System.ServiceProcess.ServiceBase() {New VitalSignsDBHealth}

        System.ServiceProcess.ServiceBase.Run(ServicesToRun)
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    ' NOTE: The following procedure is required by the Component Designer
    ' It can be modified using the Component Designer.  
    ' Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        '
        'Service1
        '
        Me.ServiceName = "VitalSigns Database Health"

    End Sub

#End Region


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
		'  WriteAuditEntry(Now.ToString & " VitalSigns Database Health service is starting up #1.")
		Dim myRegistry As New RegistryHandler

		' No need of this anymore. 
		' UpdateTableDesign()

		Try
			'WriteAuditEntry(Now.ToString & " Querying SQL server for the current date format.", LogLevel.Normal)
			strDateFormat = objDateUtils.GetDateFormat()
			'WriteAuditEntry(Now.ToString & " The current date format is " & strDateFormat, LogLevel.Normal)
		Catch ex As Exception

		End Try

		Try
			strAppPath = AppDomain.CurrentDomain.BaseDirectory.ToString
			System.IO.Directory.CreateDirectory(strAppPath & "\Log_Files\Database_Health\")
			'UpdateTableDesign()
		Catch ex As Exception
			strAppPath = "c:\"
		End Try

		' First clean up old data before we start the service. 
		'CleanupDBData()

		Dim fileArray As String()
		'WriteAuditEntry(Now.ToString + " Cleaning up log files")
		fileArray = Directory.GetFiles(strAppPath & "\Log_Files\Database_Health", "*.txt")

        ' Set the global password to be used every where... 
        ' ------------------------- START OF GETTING PASSWORD AND SET TO GLOBAL VARIABLE -------------------
        Dim MyPass As Byte()
        Try

            Dim strValue As String = myRegistry.ReadFromRegistry("Password")
            Dim str1() As String
            str1 = strValue.Split(",")
            Dim bstr1(str1.Length - 1) As Byte
            For j As Integer = 0 To str1.Length - 1
                bstr1(j) = str1(j).ToString()
            Next
            MyPass = bstr1

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error decrypting the Notes password.  " & ex.ToString, LogLevel.Normal)
            MyPass = Nothing
        End Try

        Dim mySecrets As New VSFramework.TripleDES
		Try
			If Not MyPass Is Nothing Then
				MyDominoPassword = mySecrets.Decrypt(MyPass) 'password in clear text, stored in memory now
				WriteAuditEntry(Now.ToString & " DB Health module successfully decrypted the Notes password.", LogLevel.Verbose)
			End If
		Catch ex As Exception
            '  MyDominoPassword = ""
            WriteAuditEntry(Now.ToString & " Error decrypting the Notes password.  " & ex.ToString, LogLevel.Normal)
        End Try

        ' ------------------------- END OF GETTING PASSWORD ---------------------------

        Dim myFile As String
		For Each myFile In fileArray
			'WriteAuditEntry(Now.ToString + " Deleting " & myFile)

			Try
				File.Delete(myFile)
			Catch ex As Exception
				WriteAuditEntry(Now.ToString + " Error Deleting " & myFile & ": " & ex.ToString)
			End Try

		Next

		MyLogLevel = LogLevel.Normal

		Try
			MyLogLevel = myRegistry.ReadFromRegistry("Log Level")
		Catch ex As Exception
			MyLogLevel = LogLevel.Normal
		End Try


		Try
			myRegistry.WriteToRegistry("Database Health Start", Now.ToShortDateString & " " & Now.ToShortTimeString)
			myRegistry.WriteToRegistry("Database Health Build", BuildNumber)
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
			WriteAuditEntry(Now.ToString & " VitalSigns Database Health service is starting up.")
			WriteAuditEntry(Now.ToString & " VitalSigns Database Health Build Number: " & BuildNumber)
			WriteAuditEntry(Now.ToString & " Copyright " & Now.Year & ", JNIT, Inc. dba RPR Wyatt and Plum Island Publishing, LLC")
			WriteAuditEntry(Now.ToString & " All rights reserved." & vbCrLf & vbCrLf)
			WriteAuditEntry(Now.ToString & " Log level is " & MyLogLevel)
		Catch ex As Exception

		End Try


		'Try
		'    FastScan = myRegistry.ReadFromRegistry("FastDBScan")
		'Catch ex As Exception
		'    FastScan = True
		'End Try


		Try
			myRegistry.WriteToRegistry("Database Health Date", Now.ToShortDateString)
			WriteAuditEntry(Now.ToString & " FastScan option = " & FastScan & ".  To modify this value add a registry key FastDBScan set to true or false.")
		Catch ex As Exception

		End Try

		Try
			WriteAuditEntry(Now.ToString & " *** Building the collection of servers to process.")
			CreateCollections()
		Catch ex As Exception

		End Try



		Try
			WriteAuditEntry(Now.ToString & " *** Attempting to compute when the last scan date was", LogLevel.Verbose)
			IdentifyLastDBScan()
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " *** Oops, " & ex.ToString)
		End Try


		Try
			Dim StartWorking As New Thread(AddressOf StartThreads)
			StartWorking.Start()
			'StartWorking.Join()
		Catch ex As Exception

		End Try

		WriteAuditEntry(Now.ToString & " Start up procedure has finished.")

		'Me.Stop()
	End Sub

    Protected Overrides Sub OnStop()

        boolTimeToStop = True

        Try
            Dim myRegistry As New RegistryHandler
            myRegistry.WriteToRegistry("Database Health End", Now.ToShortDateString & " " & Now.ToShortTimeString)
        Catch ex As Exception

        End Try

        RemoveNullEntries()

        Dim strSQL As String
        Dim objVSAdaptor As New VSAdaptor

		'Me.Stop()
		MyBase.OnStop()

    End Sub


    'Private Sub CleanupDBData()
    '    WriteAuditEntry(Now.ToString & " Cleaning up All data older than 2 days.")
    '    Dim objVSAdaptor As New VSAdaptor
    '    Dim SearchDate As DateTime
    '    SearchDate = FixDate(Today.AddDays(-2))
    '    Dim strSQL As String = ""

    '    Try
    '        ' Delete all the earlier days data.
    '        strSQL = "DELETE FROM Daily WHERE ScanDate < " & objVSAdaptor.DateFormat(FixDate(SearchDate)) & ""
    '        objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "NSFHealth", strSQL)
    '        ' Delete all the old Temp records, if in case the earlier service did not finish its job.
    '        strSQL = "DELETE FROM Daily WHERE Temp = 1 "
    '        objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "NSFHealth", strSQL)
    '    Catch ex As Exception
    '        WriteAuditEntry(Now.ToString & " Error executing SQL command " & ex.ToString)
    '    End Try

    '    Try
    '        GC.Collect()
    '    Catch ex As Exception
    '        '  Thread.Sleep(500)
    '    End Try

    '    WriteAuditEntry(Now.ToString & " Finished up Daily Data older than 1 days. ")

    'End Sub

    'Public Sub UpdateTableDesign()
    '    Dim strSQL As String = ""
    '    strSQL = "CREATE TABLE [dbo].[ScanResults]([ID] [int] IDENTITY(1,1) NOT NULL,[ScanDate] [datetime] NULL,[ServerName] [nvarchar](250) NULL, [DatabaseCount] [int] NULL) "
    '    Dim objVSAdaptor As New VSAdaptor
    '    Try
    '        objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "NSFHealth", strSQL)
    '    Catch ex As Exception

    '    End Try

    '    Try
    '        strSQL = "ALTER TABLE Daily ADD Temp Bit Null"
    '        objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "MailFileStats", strSQL)
    '    Catch ex As Exception
    '    End Try

    'End Sub

    Public Sub StartThreads()
        Dim EnabledCount As Integer = 0
        Dim n As Integer
        Dim ServerOne As MonitoredItems.DominoServer
        For n = 0 To MyDominoServers.Count - 1
            ServerOne = MyDominoServers.Item(n)
            If ServerOne.Enabled = True And ServerOne.ScanDBHealth = True Then EnabledCount += 1
        Next n
        Dim intThreadCount As Integer = CInt(EnabledCount / 5)
        'If intThreadCount = 1 Then intThreadCount = 2
        If intThreadCount <= 1 Then intThreadCount = 2
        If intThreadCount > 5 Then intThreadCount = 5

        WriteAuditEntry(Now.ToString & " There are " & EnabledCount & " enabled Domino servers to scan for database health.")
        WriteAuditEntry(Now.ToString & " I am launching " & intThreadCount & " threads to scan these servers.")

        Try
            For n = 1 To intThreadCount
                Dim tTemp As Threading.Thread = New Threading.Thread(AddressOf MonitorNotesDatabaseHealth)
                tTemp.Start()
                ListOfDominoThreads.Add(tTemp)
                Threading.Thread.Sleep(15000)  'sleep 15 seconds then start another thread
            Next
        Catch ex As Exception
            If InStr(ex.ToString, "System.OutOfMemoryException") > 0 Then
                WriteAuditEntry(Now.ToString & " VitalSigns is out of memory.  Attempting to exit so the Master service will restart it. ")
                Thread.Sleep(1000)
                End
            Else
                WriteAuditEntry(Now.ToString & " Error starting MonitorDomino thread " & ex.ToString)
            End If

        End Try

        For Each t As System.Threading.Thread In ListOfDominoThreads
            WriteAuditEntry(Now.ToString & " Waiting for other threads to finish ")
            t.Join()
        Next

        WriteAuditEntry(Now.ToString & " Finished with all the threads")

        Dim strSQL As String
        Dim objVSAdaptor As New VSAdaptor


        RemoveNullEntries()

        Me.Stop()
        '   Dim VitalStatusThread As New Thread(AddressOf OutputVitalStatus)
        '    VitalStatusThread.Start()
    End Sub

#Region "Create Collections of items to Monitor"

    Private Function getCurrentNode() As String
        Dim NodeName As String = ""
        If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then
            NodeName = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
        End If
        Return NodeName
    End Function

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

            'Removed DominoServers.DiskSpaceThreshold, DominoServers.NotificationGroup, DominoServer.ScanServlet

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
                .Include(Function(x) x.DeadMailDeleteThreshold) _
                .Include(Function(x) x.DiskInfo) _
                .Include(Function(x) x.HeldMailThreshold) _
                .Include(Function(x) x.ScanDBHealth) _
                .Include(Function(x) x.Description) _
                .Include(Function(x) x.NotificationList) _
                .Include(Function(x) x.MemoryThreshold) _
                .Include(Function(x) x.CpuThreshold) _
                .Include(Function(x) x.ClusterReplicationDelayThreshold)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()

            WriteAuditEntry(Now.ToString & " Created a dataset with " & listOfServers.Count)
        Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateDominoServersCollection processing code. Exception: " & ex.Message)
			Exit Sub
		End Try

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
                    If Not (InStr(MyName, "RPRWyattTest")) Then
                        MyDominoServers.Add(MyDominoServer)
                    End If

                    Dim tNow As DateTime
                    tNow = Now
                    'Default values for OS and Version
                    MyDominoServer.OperatingSystem = "Unknown"
                    MyDominoServer.VersionDomino = "Unknown"
                    MyDominoServer.ResponseDetails = "This server has not yet been scanned."

                    MyDominoServer.OffHours = False
                    MyDominoServer.AlertCondition = False
                    MyDominoServer.Status = "Not Scanned"
                    MyDominoServer.LastScan = Date.Now

                    MyDominoServer.ServerType = VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription()

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
                        If entity.ScanDBHealth Is Nothing Then
                            .ScanDBHealth = False
                        Else
                            .ScanDBHealth = entity.ScanDBHealth
                        End If

                        If .ScanDBHealth = True And .Enabled = True Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server will be scanned for DB Health.")
                            ActiveServerCount += 1
                        Else
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server will not be scanned for DB Health.")
                        End If
                    Catch ex As Exception
                        .ScanDBHealth = False
                    End Try


                    .AdvancedMailScan = False



                    Try
                        If entity.Category Is Nothing Then
                            .Category = "Domino"
                            WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Category not set, using default of 'Domino'.")
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


                End With

                MyDominoServer = Nothing

            Next


        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Domino servers error: " & exception.Message)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Domino servers error: " & ex.Message)
        End Try


    End Sub


#End Region

#Region "Database Health"


    Public Class Database
        Public FileName As String
        Public Title As String
        Public FileSize As Long
        Public Server As String
        Public DesignTemplateName As String
        Public Details As String
        Public Quota As Long
        Public FTIndexed As Boolean
        Public EnabledForClusterReplication As Boolean
        Public EnabledforReplication As Boolean
        Public ReplicaID As String
        Public ODS As Integer
        Public Status As String
        Public DocumentCount As Long
        Public FolderCount As Long
        Public Categories As String
        Public CurrentAccessLevel As String
        Public FTIndexFrequency As String
        Public IsPrivateAddressBook As Boolean
        Public Folder As String
        Public IsMailFile As Boolean = False
        Public InboxDocCount As Long
        Public IsInService As Boolean
        Public IsPublicAddressBook As Boolean
        Public Created As DateTime
        Public LastFixup As DateTime
        Public PercentUsed As Double
        Public LastFTIndexed As DateTime
        Public LastModified As DateTime
        Public QuickrCustomFormCount As Integer
        Public QuickrPlaceBotCount As Integer
        Public PersonDocID As String
        Public FileNamePath As String
        Public DeviceId As String
    End Class

    Private Sub MonitorNotesDatabaseHealth()


        Dim Server As MonitoredItems.DominoServer
        Do While boolTimeToStop <> True
            Selector_Mutex.WaitOne()
            Server = SelectServer()
            Selector_Mutex.ReleaseMutex()
            Try
                If Server Is Nothing Then
                    WriteAuditEntry(Now.ToString & " *****************************************************************")
                    WriteAuditEntry(Now.ToString & " No servers were selected to be analyzed.")
                    WriteAuditEntry(Now.ToString & " *****************************************************************")
                    WriteAuditEntry(vbCrLf)
                    Exit Do
                End If
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Exception stopping " & ex.ToString)
            End Try


            Try
                Server.QuickrServer = False 'Assume it is not a Quickr server
                WriteAuditEntry(Now.ToString & " *****************************************************************")
                WriteAuditEntry(Now.ToString & " Started Analyzing " & Server.Name)
                WriteAuditEntry(Now.ToString & " *****************************************************************")
                WriteAuditEntry(vbCrLf)
                Server.LastDBHealthScan = Date.Now
                AnalyzeServer(Server)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Exception @ #2 :" & ex.ToString)
            End Try

            Try
                If MyLogLevel = LogLevel.Verbose Then
                    WriteAuditEntry(Now.ToString & " ")
                    WriteAuditEntry(Now.ToString & "  ")
                    WriteAuditEntry(Now.ToString & " *****************************************************************")
                    WriteAuditEntry(Now.ToString & " Thread report")
                    WriteAuditEntry(Now.ToString & " *****************************************************************")
                    WriteAuditEntry(vbCrLf)
                End If

            Catch ex As Exception

            End Try



            Try
                Dim i As Integer = 1
                For Each t As System.Threading.Thread In ListOfDominoThreads
                    WriteAuditEntry(Now.ToString & " Server analysis thread #" & i.ToString & " state = " & t.ThreadState.ToString, LogLevel.Verbose)
                    i += 1
                Next


            Catch ex As Exception

            End Try

NextServer:
            Thread.Sleep(10000)
        Loop

        Try
            GC.Collect()
        Catch ex As Exception

        End Try

        WriteAuditEntry(Now.ToString & " The Database Health Service is shutting down.")
        Try
            Dim myRegistry As New RegistryHandler
            myRegistry.WriteToRegistry("Database Health End", Now.ToShortDateString & " " & Now.ToShortTimeString)
        Catch ex As Exception

        End Try
        'Me.Stop()

    End Sub

    Private Function SelectServer() As MonitoredItems.DominoServer
        Dim objVSAdaptor As New VSAdaptor
        Dim intDBCount, intRecordCount As Integer
        Dim strSQL As String = ""
        WriteAuditEntry(Now.ToString & " Selecting the next server to scan.... ", LogLevel.Verbose)

        Dim n As Integer

        For Each server As MonitoredItems.DominoServer In MyDominoServers

            If server.Enabled = False Or server.ScanDBHealth = False Or server.IsBeingScanned = True Then
                Continue For
            End If

            For n = 45 To 0 Step -1
                Dim mydate As DateTime = Date.Today.AddDays(-n)

                'WriteAuditEntry(Now.ToString & " " & server.Name & " was last scanned " & server.LastDBHealthScan.Date)

                If server.LastDBHealthScan.Date = mydate.Date And server.LastDBHealthScan < Now.AddHours(-3) Then
                    server.LastDBHealthScan = Now  'prevents it from being selected again today in the later selection
                    server.IsBeingScanned = True
                    Return server
                End If

            Next
        Next


        For Each server As MonitoredItems.DominoServer In MyDominoServers
            If server.Enabled = False Or server.ScanDBHealth = False Or server.IsBeingScanned = True Then
                Continue For
            End If

            Try

                Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repo.Filter.Eq(Function(x) x.DeviceName, server.Name) And repo.Filter.Eq(Function(x) x.DeviceType, "Domino")
                Dim entity As VSNext.Mongo.Entities.Server = repo.Find(filterDef).ToList()(0)

                intDBCount = entity.DatabaseCount


                'strSQL = "Select DatabaseCount FROM ScanResults WHERE ServerName ='" & server.Name & "'"
                'intDBCount = objVSAdaptor.ExecuteScalarAny("VSS_Statistics", "NSFHealth", strSQL)
                WriteAuditEntry(Now.ToString & " The previous run of DB Health found " & intDBCount & " databases on " & server.Name, LogLevel.Verbose)
            Catch ex As Exception
                intDBCount = 0
            End Try

            Try

                Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Database)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Database) = repo.Filter.Eq(Function(x) x.DeviceName, server.Name)
                intRecordCount = repo.Collection.Count(filterDef)

                'strSQL = "Select COUNT(ID) FROM DAILY WHERE Server ='" & server.Name & "'"
                'intRecordCount = objVSAdaptor.ExecuteScalarAny("VSS_Statistics", "NSFHealth", strSQL)
                WriteAuditEntry(Now.ToString & " There are " & intRecordCount & " records for " & server.Name & " in the Daily table.", LogLevel.Verbose)
            Catch ex As Exception
                intRecordCount = 0
            End Try

            Dim myPercent As Double
            myPercent = intRecordCount / intDBCount
            '  WriteAuditEntry(Now.ToString & " There are " & myPercent.ToString("P1") & " of the records found.")

            If myPercent > 0.95 And server.Enabled = True Then
                server.IsBeingScanned = False
            End If

            If myPercent < 0.9 And server.Enabled = True And server.IsBeingScanned = False Then
                server.IsBeingScanned = True
                server.LastDBHealthScan = Date.Now
                WriteAuditEntry(Now.ToString & "  " & server.Name & " did not complete the last scan cycle, so I am scanning it again.", LogLevel.Verbose)
                Return server
            End If

        Next



        Return Nothing
    End Function

    Private Sub AnalyzeServer(ByRef Server As MonitoredItems.DominoServer)
        WriteAuditEntry(Now.ToString & " Started collecting information for " & Server.Name, LogLevel.Verbose)
        'Try
        '    CleanMailFileDetails(Server.Name)
        'Catch ex As Exception
        '    ' WriteDeviceHistoryEntry("Database_Health", Server.Name, Now.ToString &" Exception cleaning mail files for " & Server.Name)
        '    WriteDeviceHistoryEntry("Database_Health", Server.Name, Now.ToString & " Exception cleaning mail files: " & ex.ToString)
        'End Try

        Dim objVSAdaptor As New VSAdaptor
        Dim watchOverall As New System.Diagnostics.Stopwatch
        watchOverall.Start()

        If InStr(Server.Name, "VFCorp") Then FastScan = True
        If InStr(Server.Name, "TSYS") Then FastScan = True

       
        ' Creating a local Notes Session to make sure if this is not a bottleneck. 
        Dim LocalNotesSession As New Domino.NotesSession
        Dim db As Domino.NotesDatabase
        Dim dbDir As Domino.NotesDbDirectory

        If Not MyDominoPassword Is Nothing Then
            If MyLogLevel = LogLevel.Verbose Then
                WriteAuditEntry(Now.ToString & " Notes password is registered.  Attempting to initialize a session to process database health.")
            End If
            Try
                LocalNotesSession.Initialize(MyDominoPassword)
                WriteAuditEntry(Now.ToString & " Initialized Notes session for " & LocalNotesSession.CommonUserName)
            Catch ex As Exception
                ' If MyLogLevel = LogLevel.Verbose Then
                WriteAuditEntry(Now.ToString & " Error initializing a session to process database health: " & ex.Message)
            End Try
        End If


        dbDir = LocalNotesSession.GetDbDirectory(Server.Name)

        Dim dbPathlist1 As New Generic.List(Of String)
        Dim dbPathlist2 As New Generic.List(Of String)
        Dim dbPathlist3 As New Generic.List(Of String)
        Dim dbPathlist4 As New Generic.List(Of String)


        Dim intCounter As Integer = 1
        Dim intTotalCount As Integer = 0
        Dim strSQL As String = ""
        Dim boolIsMailFile As Boolean = False

        Try
            db = dbDir.GetFirstDatabase(Domino.DB_TYPES.NOTES_DATABASE)
            If db Is Nothing Then
                Exit Try
            End If

            dbPathlist1.Add(Server.Name)
            dbPathlist2.Add(Server.Name)
            dbPathlist3.Add(Server.Name)
            dbPathlist4.Add(Server.Name)

            dbPathlist1.Add("Thread 1")
            dbPathlist2.Add("Thread 2")
            dbPathlist3.Add("Thread 3")
            dbPathlist4.Add("Thread 4")

            dbPathlist1.Add(Server.ServerObjectID)
            dbPathlist2.Add(Server.ServerObjectID)
            dbPathlist3.Add(Server.ServerObjectID)
            dbPathlist4.Add(Server.ServerObjectID)

            While Not (db Is Nothing)
                WriteDeviceHistoryEntry("Database_Health", Server.Name, Now.ToString & " Processing " & db.Title)
                intTotalCount += 1
                Select Case intCounter
                    Case 1
                        dbPathlist1.Add(db.FilePath)
                    Case 2
                        dbPathlist2.Add(db.FilePath)
                    Case 3
                        dbPathlist3.Add(db.FilePath)
                    Case 4
                        dbPathlist4.Add(db.FilePath)
                End Select

                intCounter += 1
                If intCounter = 5 Then intCounter = 1
                db = dbDir.GetNextDatabase()
            End While



        Catch ex As Exception
            WriteDeviceHistoryEntry("Database_Health", Server.Name, Now.ToString & " Exception getting all the file paths: " & ex.ToString)
            WriteDeviceHistoryEntry("Database_Health", Server.Name, Now.ToString & " " & Server.Name & " is not responding right now.  Database Health service will move on to the next server.")
        Finally
            If db IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
            End If

            If dbDir IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(dbDir)
            End If
            If LocalNotesSession IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(LocalNotesSession)
            End If
        End Try

        Try
            WriteDeviceHistoryEntry("Database_Health", Server.Name, Now.ToString & " I have a list of database file paths with  " & intTotalCount & " databases in it.")
            If intTotalCount = 0 Then Exit Sub
        Catch ex As Exception

        End Try


        Try
            'Dim intResult As Integer
            ' ScanResults (ID COUNTER, ServerName char(250), DatabaseCount INT, ScanCount INT, ScanDate DATE) "
            'strSQL = "Update ScanResults Set DatabaseCount = " & intTotalCount & ", ScanDate = '" & Date.Now & "' WHERE ServerName='" & Server.Name & "'"
            'intResult = objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "NSFHealth", strSQL)
            '
            'If intResult = 0 Then
            '    strSQL = "Insert into ScanResults (ServerName, DatabaseCount, ScanDate) VALUES ('" & Server.Name & "', " & intTotalCount & ", '" & Date.Now.Date & "')"
            '    objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "NSFHealth", strSQL)
            'End If


            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repo.Filter.Eq(Function(x) x.DeviceName, Server.Name) And repo.Filter.Eq(Function(x) x.DeviceType, Server.ServerType)
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Server) = repo.Updater.Set(Function(x) x.DatabaseCount, intTotalCount)
            repo.Update(filterDef, updateDef)


        Catch ex As Exception
            WriteDeviceHistoryEntry("Database_Health", Server.Name, Now.ToString & " Error assigning the Database Count to the server. Error: " & ex.Message)
        End Try

        Try
            Dim intResult As Integer
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Database)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Database) =
                repo.Filter.Eq(Function(x) x.DeviceName, Server.Name) And
                repo.Filter.Eq(Function(x) x.Temp, True)

            repo.Delete(filterDef)

        Catch ex As Exception
            WriteDeviceHistoryEntry("Database_Health", Server.Name, Now.ToString & " Exception deleting data " & ex.ToString)
        End Try

        Dim t1 As New Thread(AddressOf CollectDatabaseHealthWrapper)
        Dim t2 As New Thread(AddressOf CollectDatabaseHealthWrapper)
        Dim t3 As New Thread(AddressOf CollectDatabaseHealthWrapper)
        Dim t4 As New Thread(AddressOf CollectDatabaseHealthWrapper)

        Try
            WriteDeviceHistoryEntry("Database_Health", Server.Name, Now.ToString & " Starting thread one with " & dbPathlist1.Count - 2 & " databases in it.", LogLevel.Verbose)

            t1.Start(dbPathlist1)
            Thread.Sleep(1000)

            WriteDeviceHistoryEntry("Database_Health", Server.Name, Now.ToString & " Starting thread two with " & dbPathlist2.Count - 2 & " databases in it.", LogLevel.Verbose)

            t2.Start(dbPathlist2)
            Thread.Sleep(1000)

            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", Server.Name, Now.ToString & " Starting thread three with " & dbPathlist3.Count - 2 & " databases in it.")

            t3.Start(dbPathlist3)
            Thread.Sleep(15000)

            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", Server.Name, Now.ToString & " Starting thread four with " & dbPathlist4.Count - 2 & " databases in it.")

            t4.Start(dbPathlist4)
        Catch ex As Exception

        End Try

        Try
            t1.Join()
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", Server.Name, " Thread #1 has completed. ")
            t2.Join()
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", Server.Name, " Thread #2 has completed. ")
            t3.Join()
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", Server.Name, " Thread #3 has completed. ")
            t4.Join()
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", Server.Name, " Thread #4 has completed. ")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error joining the four threads for " & Server.Name & ": " & ex.ToString)
        End Try
        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", Server.Name, Now.ToString & " All four threads have completed.")

        Try
            'Convert the temp records into permanent records for NSFHealth  
            WriteDeviceHistoryEntry("Database_Health", Server.Name, Now.ToString & " Deleting the prior records for " & Server.Name)

            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Database)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Database) =
                repo.Filter.Eq(Function(x) x.DeviceName, Server.Name) And
                repo.Filter.Eq(Function(x) x.Temp, False)

            repo.Delete(filterDef)

            'UPDATE table_name SET column1=value1,column2=value2,... WHERE some_column=some_value;

            filterDef = repo.Filter.Eq(Function(x) x.DeviceName, Server.Name) And
                repo.Filter.Eq(Function(x) x.Temp, True)
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Database) = repo.Updater.Set(Function(x) x.Temp, False)

            repo.Update(filterDef, updateDef)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error transferring records for " & Server.Name & ": " & ex.ToString)
        End Try

        Try
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Database)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Database) = _
                repo.Filter.Eq(Function(x) x.FileName, "") Or _
                repo.Filter.Exists(Function(x) x.FileName, False)

            repo.Delete(filterDef)

        Catch ex As Exception

            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", Server.Name, " Exception in Analyze Server when removing empty and null FileNames. Exception : " & ex.Message.ToString())

        End Try

    End Sub

    Private Sub CollectDatabaseHealthWrapper(dbPathlist As Generic.List(Of String))

        Dim LocalNotesSession As New Domino.NotesSession
        Dim db As Domino.NotesDatabase
        Dim v As Domino.NotesView
        Dim nRepl As Domino.NotesReplication
        Dim coll As Domino.NotesDocumentCollection

        Try
            ' Creating a local Notes Session to make sure if this is not a bottleneck. 
            If Not MyDominoPassword Is Nothing Then
                If MyLogLevel = LogLevel.Verbose Then
                    WriteAuditEntry(Now.ToString & " Notes password is registered.  Attempting to initialize a session to process database health.")
                End If
                Try
                    LocalNotesSession.Initialize(MyDominoPassword)
                    WriteAuditEntry(Now.ToString & " Initialized Notes session for " & LocalNotesSession.CommonUserName)
                    'MyDominoPassword = ""
                Catch ex As Exception
                    ' If MyLogLevel = LogLevel.Verbose Then
                    WriteAuditEntry(Now.ToString & " Error initializing a session to process database health: " & ex.Message)
                End Try
            End If
            CollectDatabaseHealth(dbPathlist, LocalNotesSession, db, v, nRepl, coll)
        Catch ex As Exception

        Finally
            If coll IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(coll)
            End If

            If nRepl IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(nRepl)
            End If
            If v IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(v)
            End If

            If LocalNotesSession IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(LocalNotesSession)
            End If
        End Try



    End Sub



    Private Sub CollectDatabaseHealth(dbPathlist As Generic.List(Of String), ByRef LocalNotesSession As Domino.NotesSession, ByRef db As Domino.NotesDatabase, ByRef v As Domino.NotesView, ByRef nRepl As Domino.NotesReplication, ByRef coll As Domino.NotesDocumentCollection)

        Dim ServerName As String = dbPathlist.Item(0)
        Dim ThreadName As String = dbPathlist.Item(1)
        Dim DeviceId As String = dbPathlist.Item(2)


        'Remove the first three items from the list
        dbPathlist.RemoveAt(0)
        dbPathlist.RemoveAt(0)
        dbPathlist.RemoveAt(0)

        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Started collecting information for " & ServerName)
        Dim watchOverall As New System.Diagnostics.Stopwatch
        watchOverall.Start()

        Dim strDBTitle As String = ""

        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " ")
        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " **********************************************************")
        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " *** Analyzing Notes Database Health for server: " & ServerName & " from " & ThreadName)
        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " **********************************************************")
        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " ")
        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This collection has " & dbPathlist.Count & " databases to analyze.")
        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " ")
        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " DeviceId : " & DeviceId)

        'reset the counter
        Dim dbCounter As Integer = 0

        Dim oWatch As New System.Diagnostics.Stopwatch
        oWatch.Start()

        Dim FilePath As String
        For Each FilePath In dbPathlist
            oWatch.Reset()
            oWatch.Start()
            If boolTimeToStop = True Then
                Exit Sub
            End If

            dbCounter += 1

            Try
                WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " ")
                WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Attempting to inventory  " & ServerName & " - " & FilePath & " -- database # " & dbCounter.ToString)
                db = LocalNotesSession.GetDatabase(ServerName, FilePath)
            Catch ex As Exception
                WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception getting database " & ex.ToString)
                Dim ndb As New Database
                Dim strTemp, strSQL As String
                With ndb
                    .FileNamePath = FilePath
                    .FileName = FilePath
                    .Status = "No Access"
                    .Title = "Insufficient Access"
                    .Details = "Could not open the database"
                    .Server = ServerName
					.CurrentAccessLevel = "No Access"
                    'changed fixdate as fixdatetime by somaraj

                End With

                Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Database)(connectionString)
                Dim entity As New VSNext.Mongo.Entities.Database() With {
                    .Temp = True,
                    .Status = "Exception",
                    .FileNamePath = ndb.FileNamePath,
                    .ScanDateTime = GetFixedDateTime(DateTime.Now),
                    .FileName = ndb.FileName,
                    .Title = ndb.Title,
                    .DeviceName = ndb.Server,
                    .CurrentAccessLevel = ndb.CurrentAccessLevel,
                    .Details = ndb.Details,
                    .Folder = ndb.Folder,
                    .DeviceId = DeviceId
                }

                repo.Insert(entity)

                WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Skipping detailed Notes Database Health for " & FilePath & " because it cannot be opened-- database # " & dbCounter.ToString)

                GoTo SkipDatabase
            End Try

            If db Is Nothing Then
                WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Skipping Notes Database Health for " & FilePath & " because it cannot be opened-- database # " & dbCounter.ToString)
                GoTo SkipDatabase
            End If


            Thread.Sleep(500)
            Dim myNotesDatabase As New Database
            With myNotesDatabase

                Try
                    If Not db Is Nothing Then
                        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Analyzing Notes Database Health for " & db.Title & "-- database # " & dbCounter.ToString & ".  Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))
                    Else
                        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Skipping Notes Database Health for " & FilePath & " because it cannot be opened-- database # " & dbCounter.ToString)
                        GoTo SkipDatabase
                    End If

                Catch ex As Exception
                    GoTo SkipDatabase
                End Try


                Try
                    .DesignTemplateName = ""
                    .DesignTemplateName = Trim(db.DesignTemplateName)
                Catch ex As Exception
                    .DesignTemplateName = ""
                End Try

                Try
                    .InboxDocCount = 0
                    .Status = "OK"
                    .Server = db.Server
                    .Categories = db.Categories
                    .ReplicaID = db.ReplicaID
                    .IsMailFile = False  'Assume it is not a mail file
                    .PersonDocID = ""
                    .FileName = db.FileName
                    'strDBTitle = db.Title
                    'strDBTitle = strDBTitle.Replace("(", "")
                    'strDBTitle = strDBTitle.Replace(")", "")
                    'strDBTitle = strDBTitle.Replace("'", "")
                    .Title = db.Title
                    If Trim(strDBTitle) = "" Then
                        strDBTitle = "Untitled Database"
                    End If

                    If InStr(.Title, "'") Then
                        .Title = .Title.Replace("'", "")
                    End If
                    ' .Title = strDBTitle.Substring(0, 50)

                    If InStr(.FileNamePath, "'") Then
                        .FileNamePath = .FileNamePath.Replace("'", "")
                    End If

                    If InStr(.FileName, "'") Then
                        .FileName = .FileName.Replace("'", "")
                    End If

                    .DeviceId = DeviceId

                Catch ex As Exception

                End Try

                Try
                    If InStr(strDBTitle.ToUpper, "PLACE CATALOG") Or InStr(strDBTitle.ToUpper, "PLACE STATISTICS") Or InStr(.DesignTemplateName.ToUpper, "PLACE CAT") Then
                        Dim Server As MonitoredItems.DominoServer
                        Server = MyDominoServers.Search(ServerName)
                        Server.QuickrServer = True
                    End If
                Catch ex As Exception

                End Try

                Try
                    Dim sFilePath As String = db.FilePath
                    .FileNamePath = sFilePath.ToLower
                    Dim sFolder As String = sFilePath.Replace(db.FileName, "")
                    .Folder = sFolder
                Catch ex As Exception
                    .Folder = db.FilePath
                End Try

                Try
                    .FileSize = db.Size / 1024 / 1024  'size is in bytes
                Catch ex As Exception

                End Try

                Try
                    .Quota = db.SizeQuota / 1024   'size quota is in Kilobytes
                Catch ex As Exception
                    .Quota = 0
                End Try

                Try
                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " The database path is " & db.FilePath)
                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " The database is " & .FileSize.ToString & " MB with a quota of " & .Quota.ToString & " MB ")
                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))
                Catch ex As Exception

                End Try

                Try
                    .ReplicaID = db.ReplicaID
                    .FTIndexFrequency = "None"
                    .Details = "Connected to the database at " & Date.Now.ToShortTimeString
                Catch ex As Exception

                End Try

                Try
                    If InStr(db.Title.ToUpper, "MAIL") Then
                        .IsMailFile = True
                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is being marked as a mail database because the database title is " & db.Title)
                    End If
                Catch ex As Exception

                End Try


                Try
                    '7/28/2016 NS modified for VSPLUS-3141
                    If InStr(db.FilePath.ToUpper, "MAIL") Then
                        .IsMailFile = True
                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is being marked as a mail database because the file name is " & db.FileName)
                    End If
                Catch ex As Exception

                End Try

                '7/28/2016 NS added for VSPLUS-3141
                Try
                    If InStr(.DesignTemplateName.ToUpper, "MBE open NTF 8.5 VC") > 0 And Trim(.DesignTemplateName) <> "" Then
                        .IsMailFile = True
                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is being marked as a mail database because the template name is " & db.DesignTemplateName)
                    End If
                Catch ex As Exception
                    '   If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString &" This database is being marked as a mail database because the template name is " & db.DesignTemplateName)
                End Try

                '7/28/2016 NS added for VSPLUS-3141
                Try
                    If InStr(.DesignTemplateName.ToUpper, "OpenNTFDWA7a") > 0 And Trim(.DesignTemplateName) <> "" Then
                        .IsMailFile = True
                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is being marked as a mail database because the template name is " & db.DesignTemplateName)
                    End If
                Catch ex As Exception
                    '   If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString &" This database is being marked as a mail database because the template name is " & db.DesignTemplateName)
                End Try

                Try
                    If InStr(.DesignTemplateName.ToUpper, "MAIL") > 0 And Trim(.DesignTemplateName) <> "" Then
                        .IsMailFile = True
                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is being marked as a mail database because the template name is " & db.DesignTemplateName)
                    End If
                Catch ex As Exception
                    '   If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString &" This database is being marked as a mail database because the template name is " & db.DesignTemplateName)
                End Try

                Try
                    If InStr(.DesignTemplateName.ToUpper, "MCKMAIL") > 0 And Trim(.DesignTemplateName) <> "" Then
                        .IsMailFile = True
                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is being marked as a mail database because the template name is " & db.DesignTemplateName)
                    End If
                Catch ex As Exception
                    '   If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString &" This database is being marked as a mail database because the template name is " & db.DesignTemplateName)
                End Try

                If FastScan = False Then
                    '*********************
                    'This section is skipped for sites with large server counts
                    Try
                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Attempting to open the database.  The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))
                        If Not (db.IsOpen) Then
                            db.Open()
                            WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " The database is open.  The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))
                        End If
                    Catch ex As Exception
                        If InStr(ex.ToString.ToUpper, "CORRUPT") Then
                            myNotesDatabase.Status = "Corrupt"
                            Dim myName As String = myNotesDatabase.ReplicaID
                            Dim myDetails As String = "The database titled " & myNotesDatabase.Title & " on " & myNotesDatabase.Server & " located at " & myNotesDatabase.FileNamePath & " seems to be corrupt.  Please try to open this database to determine if any action is required."
                            With myNotesDatabase
                                ' QueueAlert(myName, "Corrupt", myDetails, .Server, .Title, .FileNamePath, db)
                            End With
                        Else
                            .Status = "Cannot Open"
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " ********** QUEUEING ALERT *********** The database titled " & myNotesDatabase.Title & " on " & myNotesDatabase.Server & " located at " & myNotesDatabase.FileNamePath & " cannot be opened.  Please try to open this database to determine if any action is required. ")
                            Dim myName As String = myNotesDatabase.ReplicaID
                            Dim myDetails As String = "The database titled " & myNotesDatabase.Title & " on " & myNotesDatabase.Server & " located at " & myNotesDatabase.FileNamePath & " cannot be opened.  Please try to open this database to determine if any action is required. "
                            With myNotesDatabase
                                '      QueueAlert(myName, "Cannot Open", myDetails, .Server, .Title, .FileNamePath, db)
                            End With
                        End If
                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception opening database " & ex.ToString)
                        .Details = ex.Message
                    End Try

                    Try
                        If db.IsOpen Then
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Successfully opened the database.")

                            .Details = "Successfully opened database at " & Date.Now.ToShortTimeString
                            Try
                                .FTIndexed = db.IsFTIndexed
                                .EnabledForClusterReplication = db.IsClusterReplication
                                .ODS = db.FileFormat
                                .IsInService = db.IsInService
                                .IsMailFile = False
                            Catch ex As Exception
                                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception querying database #1 " & ex.ToString)
                                If InStr(ex.ToString, "full text") Then
                                    myNotesDatabase.Status = "Full Text Error"
                                    myNotesDatabase.Status = "The full text index needs to be rebuilt"
                                    Dim myName As String = myNotesDatabase.ReplicaID
                                    Dim myDetails As String = "The database titled " & myNotesDatabase.Title & " on " & myNotesDatabase.Server & " located at " & myNotesDatabase.FileNamePath & " has an error with the full text index.  The full text index needs to be rebuilt"
                                    With myNotesDatabase
                                        ' QueueAlert(myName, "Full Text", myDetails, .Server, .Title, .FileNamePath, db)
                                    End With
                                Else
                                    myNotesDatabase.Status = "Error"
                                    Dim myName As String = myNotesDatabase.ReplicaID
                                    Dim myDetails As String = "The database titled " & myNotesDatabase.Title & " on " & myNotesDatabase.Server & " located at " & myNotesDatabase.FileNamePath & " returned the following error: " & ex.ToString
                                    With myNotesDatabase
                                        'QueueAlert(myName, "Error", myDetails, .Server, .Title, .FileNamePath, db)
                                    End With
                                End If


                            End Try

                            Try
                                ' Dim notesDateTime As Domino.NotesDateTime
                                Dim timevar As Object
                                timevar = db.Created
                                Dim myTime As New DateTime
                                Try
                                    If Not timevar Is Nothing Then
                                        myTime = Date.Parse(timevar.ToString)
                                    Else
                                        myTime = Now
                                    End If
                                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database was created " & Trim(timevar.ToString))
                                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))
                                Catch ex As Exception
                                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception calculating db.created " & ex.ToString)
                                    myTime = Now

                                End Try
                                .Created = myTime

                                timevar = db.LastFixup
                                Try
                                    If Not timevar Is Nothing Then
                                        myTime = Date.Parse(timevar.ToString)
                                    Else
                                        myTime = Now
                                    End If
                                Catch ex As Exception
                                    myTime = Now
                                End Try

                                .LastFixup = myTime

                                If .FTIndexed = True Then
                                    timevar = db.LastFTIndexed
                                    Try
                                        If Not timevar Is Nothing Then
                                            myTime = Date.Parse(timevar.ToString)
                                        Else
                                            myTime = Now
                                        End If
                                    Catch ex As Exception
                                        myTime = Now
                                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception calculating db.lastFTIndexed " & ex.ToString)
                                    End Try
                                    .LastFTIndexed = myTime
                                Else
                                    .LastFTIndexed = Date.Now
                                End If

                                timevar = db.LastModified
                                Try
                                    If Not timevar Is Nothing Then
                                        myTime = Date.Parse(timevar.ToString)
                                    Else
                                        myTime = Now
                                    End If
                                Catch ex As Exception
                                    myTime = Now
                                End Try

                                .LastModified = myTime

                            Catch ex As Exception
                                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception calculating dates " & ex.ToString)

                            End Try

                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Calculated the dates.  The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))

                            Try
                                'Find out if replication is enabled

                                nRepl = db.ReplicationInfo
                                .EnabledforReplication = Not (nRepl.Disabled)
                            Catch ex As Exception
                                .EnabledforReplication = True
                                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception querying replication info: " & ex.ToString)

                            End Try

                            Try
                                .IsPrivateAddressBook = db.IsPrivateAddressBook
                                .IsPublicAddressBook = db.IsPublicAddressBook
                                .PercentUsed = db.PercentUsed
                            Catch ex As Exception
                                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception querying database #2 " & ex.ToString)

                            End Try

                            Try
                                If .FTIndexed = True Then
                                    Dim myInt As Integer
                                    myInt = db.FTIndexFrequency
                                    Select Case myInt
                                        Case 1
                                            .FTIndexFrequency = "Daily"
                                        Case 3
                                            .FTIndexFrequency = "Hourly"
                                        Case 4
                                            .FTIndexFrequency = "Immediate"
                                        Case 2
                                            .FTIndexFrequency = "Scheduled"
                                    End Select
                                End If
                            Catch ex As Exception
                                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception querying database #3 " & ex.ToString)

                            End Try

                            Try
                                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Attempting to count the documents.  The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))
                                coll = db.AllDocuments
                                .DocumentCount = coll.Count
                                coll = Nothing
                                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Counted the documents.  The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))

                            Catch ex As Exception
                                .DocumentCount = 0
                                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception querying database #4 " & ex.ToString)
                            End Try

                            Try
                                Dim myAccess As Int16
                                myAccess = db.CurrentAccessLevel
                                Select Case myAccess
                                    Case 0
                                        .CurrentAccessLevel = "No Access"
                                        .Status = "No Access"
                                    Case 1
                                        .CurrentAccessLevel = "Depositor"
                                        .Status = "No Access"
                                    Case 2
                                        .CurrentAccessLevel = "Reader"
                                    Case 3
                                        .CurrentAccessLevel = "Author"
                                    Case 4
                                        .CurrentAccessLevel = "Editor"
                                    Case 5
                                        .CurrentAccessLevel = "Designer"
                                    Case 6
                                        .CurrentAccessLevel = "Manager"
                                    Case Else
                                        .CurrentAccessLevel = "Unknown"
                                End Select
                            Catch ex As Exception
                                .CurrentAccessLevel = ex.ToString
                                WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception querying database ACL: " & ex.ToString)
                            End Try

                            Dim myViewCount As Integer = 0


                            If Not db Is Nothing And db.CurrentAccessLevel > 1 Then
                                Try
                                    If .Details = "" Then
                                        '.Details = "Refreshed " & myViewCount.ToString & " views at " & Date.Now.ToShortTimeString
                                        .Details = "Database opened successfully at " & Date.Now.ToShortTimeString
                                    End If

                                Catch ex As Exception
                                    '   WriteDeviceHistoryEntry("Notes_Database", myNotesDatabase.Name, Now.ToString & " Exception looping through views: " & ex.ToString)
                                    myNotesDatabase.Status = "Error"

                                    If InStr(ex.ToString.ToUpper, "CORRUPT") Then
                                        myNotesDatabase.Status = "Corrupt"
                                    End If
                                End Try

                            End If

                            .Details = Strings.Replace(.Details, "'", "")
                            .Details = Strings.Replace(.Details, "(0x80040FA5)", "")
                            .Details = Strings.Replace(.Details, "(0x80040FDC)", "")
                            .Details = Strings.Replace(.Details, "System.Runtime.InteropServices.COMException ", "")

                        Else
                            .CurrentAccessLevel = "No Access"
                        End If

                    Catch ex As Exception
                        If InStr(ex.ToString.ToUpper, "CORRUPT") Then
                            myNotesDatabase.Status = "Corrupt"
                        End If
                        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception opening database " & ex.ToString)
                        .Details = ex.ToString
                    End Try
                    'End Fast Scan Option
                End If


                Try
                    If .IsMailFile = True Then

                        If InStr(db.Title.ToUpper, "ARCHIVE") Then
                            .IsMailFile = False
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is NOT being marked as a mail database because it has 'Archive' in the title.")
                        ElseIf InStr(db.Server.ToUpper, "HSBC") And Not (InStr(db.FileName.ToUpper, "MAIL\")) Then
                            'Customization recommended by Rob Burns
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is NOT being marked as a mail database because all HSBC mail files are in the mail\ folder.")
                            .IsMailFile = False
                        ElseIf InStr(db.Title.ToUpper, "MAILTRACKER") Then
                            .IsMailFile = False
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is NOT being marked as a mail database because it has 'MailTracker' in the title.")
                        ElseIf InStr(db.Title.ToUpper, "JOURNAL") Then
                            .IsMailFile = False
                        ElseIf InStr(db.FileName.ToUpper, "ROAMING") Then
                            .IsMailFile = False
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is NOT being marked as a mail database because it has 'Roaming' in the path.")
                        ElseIf InStr(db.FileName.ToUpper, "MTDATA") Then
                            .IsMailFile = False
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is NOT being marked as a mail database because it has 'MTData' in the path.")
                        ElseIf InStr(db.FileName.ToUpper, "NAB") Then
                            .IsMailFile = False
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is NOT being marked as a mail database because it has 'NAB' in the path.")
                        ElseIf InStr(db.Title.ToUpper, "JUMP") Then
                            .IsMailFile = False
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is NOT being marked as a mail database because it has 'JUMP' in the title.")
                            '3/1/2016 NS modified for VSPLUS-2645
                        ElseIf InStr(db.Title.ToUpper, "NAB ") Or InStr(db.Title.ToUpper, " NAB ") Or InStr(db.Title.ToUpper, " NAB") Then
                            .IsMailFile = False
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is NOT being marked as a mail database because it has 'NAB' in the title.")
                        ElseIf InStr(db.Title.ToUpper, "ADDRESS BOOK") Then
                            .IsMailFile = False
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is NOT being marked as a mail database because it has 'Address Book' in the title.")
                        ElseIf InStr(db.Title.ToUpper, "REDIRECT") Then
                            .IsMailFile = False
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is NOT being marked as a mail database because it has 'REDIRECT' in the title.")
                        ElseIf InStr(db.FileName.ToUpper, "A_") Then
                            .IsMailFile = False
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This database is NOT being marked as a mail database because the filename begins with 'a_' which usually indicates an archive database.")
                        End If
                    End If

                Catch ex As Exception
                    WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception evaluating whether db is a mail file: " & ex.ToString)
                End Try

                Dim boolCrawford As Boolean = False

                '****************************
                ' Customization for Crawford to capture folder count and doc count
                If (InStr(ServerName, "/Crawco-") > 0 Or InStr(ServerName, "azphxdom1")) And .IsMailFile = True Then
                    'do this for Crawford servers and selected RPR servers
                    boolCrawford = True
                    Try
                        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Attempting to open the database to find out the folder count.  The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))
                        If Not (db.IsOpen) Then
                            db.Open()
                            WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " The database is open.  The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))
                        End If
                    Catch ex As Exception
                        If InStr(ex.ToString.ToUpper, "CORRUPT") Then
                            myNotesDatabase.Status = "Corrupt"
                            Dim myName As String = myNotesDatabase.ReplicaID
                            Dim myDetails As String = "The database titled " & myNotesDatabase.Title & " on " & myNotesDatabase.Server & " located at " & myNotesDatabase.FileNamePath & " seems to be corrupt.  Please try to open this database to determine if any action is required."
                            With myNotesDatabase
                                ' QueueAlert(myName, "Corrupt", myDetails, .Server, .Title, .FileNamePath, db)
                            End With
                        Else
                            .Status = "Cannot Open"
                        End If
                        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception opening database " & ex.ToString)
                        .Details = ex.Message
                    End Try
                    Try
                        If db.IsOpen Then
                            WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Successfully opened the database.")
                            .Details = "Successfully opened database at " & Date.Now.ToShortTimeString
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Attempting to count the documents.  The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))
                        coll = db.AllDocuments
                        .DocumentCount = coll.Count
                        coll = Nothing
                        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Counted the documents.  The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))

                    Catch ex As Exception
                        .DocumentCount = 0
                        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception querying database #4 " & ex.ToString)
                    End Try

                    'End special Crawford handling
                End If


                Try
                    If FastScan = False Or boolCrawford = True Then

                        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Attempting to count the folders.  The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))

                        Try
                            If .IsMailFile = True Then
                                WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Attempting to check the views.  The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))

                                .FolderCount = 0
                                For Each v In db.Views
                                    ' NEED TO REMOVE THIS COMMENT BELOW, AS ITS ONLY FOR TESTING - JOE THUMMA
                                    'WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Attempting to check the view" & v.Name)
                                    If v.IsFolder Then
                                        .FolderCount += 1
                                    End If

                                    If v.Name = "($Inbox)" Then
                                        .InboxDocCount = v.EntryCount
                                        WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Inbox has " & .InboxDocCount.ToString & " documents.", LogLevel.Verbose)
                                    End If
                                    'WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Next ")
                                Next
                                WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " This mail file has " & .FolderCount & " folders.")
                                WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Finished checking views.  The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1"))

                            End If
                        Catch ex As Exception
                            If InStr(ex.ToString, "Index is not to be generated") Then
                                'The problem is a private on first view error, not an error
                            ElseIf InStr(ex.ToString, "not authorized") Then
                                myNotesDatabase.Status = "Access"
                                .Details = "Not authorized error refreshing " & v.Name
                            Else
                                If InStr(ex.ToString.ToUpper, "CORRUPT") Then
                                    myNotesDatabase.Status = "Corrupt"
                                End If
                                .Details = "Error refreshing " & v.Name & " " & ex.ToString

                            End If
                        End Try
                    End If



                Catch ex As Exception
                    WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception evaluating whether db is a mail file: " & ex.ToString)
                End Try

                Try
                    If .IsMailFile = True Then
                        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Entering this database into the Mail File Stat table ")
                        UpdateDominoDailyMailFileStatTable(Date.Now, db.Server, .FileNamePath, .Title, .FileSize, .DesignTemplateName, .Quota, .FTIndexed, False, .EnabledForClusterReplication, .ReplicaID, .ODS, .DeviceId)
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception entering data into the Mail File Stat table: " & ex.ToString)
                End Try
            End With

            Try
                InsertIntoDatabaseHealthTable(myNotesDatabase)
            Catch ex As Exception
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Exception entering data into the NSF Health table: " & ex.ToString)
            End Try

            'Get the next database on the server
            WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " Finished a loop.   The Elapsed time: " & oWatch.Elapsed.TotalSeconds.ToString("F1") & " seconds.")

            WriteDeviceHistoryEntry("Database_Health", ServerName & "_" & ThreadName, Now.ToString & " " & (dbCounter / dbPathlist.Count).ToString("0.0%") & " complete")

SkipDatabase:

        Next


        Try
            WriteAuditEntry(Now.ToString & " Finished gathering information for " & ServerName & " Total time was " & watchOverall.Elapsed.TotalHours.ToString("f1") & " hours, " & watchOverall.Elapsed.Minutes.ToString("f1"))

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Finished gathering information for " & ServerName)
        End Try




    End Sub

    Private Sub InsertIntoDatabaseHealthTable(ByVal myNotesDatabase As Database)
        Dim myDetails As String = myNotesDatabase.Details
        If myNotesDatabase.Details.Trim = "" Then
            Exit Sub
        End If

        If myNotesDatabase.Status.Trim = "" Then
            Exit Sub
        End If


        Try
            If myDetails.Length > 255 Then
                myDetails = myDetails.Substring(0, 254)
            End If
        Catch ex As Exception

        End Try

        Dim strSQL, strTemp As String

        Dim objVSAdaptor As New VSAdaptor

        Try

            Try

                Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Database)(connectionString)
                Dim entity As New VSNext.Mongo.Entities.Database() With {
                    .Temp = True,
                    .Status = myNotesDatabase.Status,
                    .FileNamePath = myNotesDatabase.FileNamePath,
                    .ScanDateTime = GetFixedDateTime(DateTime.Now),
                    .FileName = myNotesDatabase.FileName,
                    .Title = myNotesDatabase.Title,
                    .DeviceName = myNotesDatabase.Server,
                    .Details = myDetails,
                    .Folder = myNotesDatabase.Folder,
                    .FolderCount = myNotesDatabase.FolderCount,
                    .QPlaceBotCount = myNotesDatabase.QuickrPlaceBotCount,
                    .QCustomFormCount = myNotesDatabase.QuickrCustomFormCount,
                    .InboxDocCount = myNotesDatabase.InboxDocCount,
                    .FileSize = myNotesDatabase.FileSize,
                    .DesignTemplateName = myNotesDatabase.DesignTemplateName,
                    .Quota = myNotesDatabase.Quota,
                    .FTIndexed = myNotesDatabase.FTIndexed,
                    .EnabledForClusterReplication = myNotesDatabase.EnabledForClusterReplication,
                    .EnabledForReplication = myNotesDatabase.EnabledforReplication,
                    .ReplicaId = myNotesDatabase.ReplicaID,
                    .ODS = myNotesDatabase.ODS,
                    .DocumentCount = myNotesDatabase.DocumentCount,
                    .CurrentAccessLevel = myNotesDatabase.CurrentAccessLevel,
                    .Categories = myNotesDatabase.Categories,
                    .IsPrivateAddressBook = myNotesDatabase.IsPrivateAddressBook,
                    .IsPublicAddressBook = myNotesDatabase.IsPublicAddressBook,
                    .IsInService = myNotesDatabase.IsInService,
                    .FTIndexFrequency = myNotesDatabase.FTIndexFrequency,
                    .PercentUsed = myNotesDatabase.PercentUsed,
                    .Created = myNotesDatabase.Created,
                    .LastFixup = myNotesDatabase.LastFixup,
                    .LastFTIndexed = myNotesDatabase.LastFTIndexed,
                    .LastModified = myNotesDatabase.LastModified,
                    .IsMailFile = myNotesDatabase.IsMailFile,
                    .PersonDocId = myNotesDatabase.PersonDocID,
                    .DeviceId = myNotesDatabase.DeviceId
                    }

                repo.Insert(entity)


                Dim RA As Integer   'Rows affected
                RA = objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "NSFHealth", strSQL)

                ' WriteAuditEntry(Now.ToString & " " & RA.ToString & " rows affected. ")
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error writing to Domino Status using SQL Server" & ex.ToString)
            End Try


        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Update Database Health table insert failed because: " & ex.Message)
            WriteAuditEntry(Now.ToString & " my SQL statement is " & vbCrLf & strSQL & vbCrLf)
        Finally

        End Try
        strSQL = Nothing


    End Sub


    'Private Sub EmptyDatabaseHealthTable(ByVal ServerName As String)

    '    Dim vsAdapter As New VSAdaptor
    '    Dim strSQL As String
    '    Try
    '        strSQL = "Delete * FROM Daily WHERE Server='" & ServerName & "'"
    '        vsAdapter.ExecuteNonQueryAny("VSS_Statistics", "NSFHealth", strSQL)
    '    Catch ex As Exception

    '    End Try

    'End Sub


#End Region

#Region "Mail File Count"

    'Private Sub CleanMailFileDetails(ServerName As String)
    '    Dim myRegistry As New RegistryHandler
    '    Dim myPath As String = ""
    '    Dim vsObj As New VSAdaptor

    '    Dim StrSQL As String = ""
    '    Try
    '        StrSQL = "DELETE FROM Daily WHERE  MailServer='" & ServerName & "'"
    '        vsObj.ExecuteNonQueryAny("VSS_Statistics", "MailFileStats", StrSQL)
    '    Catch ex As Exception

    '    End Try


    '    'Try
    '    '    StrSQL = "DELETE FROM Daily_Temp WHERE  Server='" & ServerName & "'"
    '    '    vsObj.ExecuteNonQueryAny("VSS_Statistics", "NSFHealth", StrSQL)
    '    'Catch ex As Exception

    '    'End Try

    'End Sub


    Private Sub UpdateDominoDailyMailFileStatTable(ByVal ScanDate As DateTime, ByVal MailServer As String, ByVal FileName As String, ByVal FileTitle As String, ByVal FileSize As Double, ByVal TemplateName As String, ByVal Quota As Double, ByVal FTIndexed As Boolean, ByVal OutOfOfficeAgentEnabled As Boolean, ByVal EnabledForClusterReplication As Boolean, ByVal ReplicaID As String, ByVal ODS As Double, ByVal DeviceId As String)
        Dim strSQL As String = ""
        If TemplateName = "" Then
            TemplateName = "None"
        End If
        If InStr(FileTitle, "'") Then
            FileTitle = FileName
        End If

        Dim vsObj As New VSAdaptor

        '11/10 - changed temp to true

        Try
            'change by somaraj fixdate as fixdatetime

            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Database)(connectionString)
            Dim entity As New VSNext.Mongo.Entities.Database() With {
                .Temp = True,
                .FileName = FileName,
                .Title = FileTitle,
                .DeviceName = MailServer,
                .FileSize = FileSize,
                .DesignTemplateName = TemplateName,
                .Quota = Quota,
                .FTIndexed = FTIndexed,
                .ReplicaId = ReplicaID,
                .ODS = ODS,
                .ScanDateTime = GetFixedDateTime(ScanDate),
                .DeviceId = DeviceId
                }

            repo.Insert(entity)


        Catch ex2 As Exception
            WriteAuditEntry(Now.ToString & " Error Inserting Mail Stats: " & ex2.Message & vbCrLf & strSQL)

        End Try



    End Sub


    '   Private Function FixDate(ByVal dt As DateTime) As String
    '       ' Return dt.ToUniversalTime.ToString
    '       Return objDateUtils.FixDate(dt, strDateFormat)
    'End Function
    Private Function FixDateTime(ByVal dt As DateTime) As String 'change by somaraj
		' Return dt.ToUniversalTime.ToString
		Return objDateUtils.FixDateTime(dt, strDateFormat)
    End Function

    Private Function GetFixedDateTime(ByVal dt As DateTime) As DateTime
        ' Return dt.ToUniversalTime.ToString
        Return DateTime.Parse(FixDateTime(dt))
    End Function
    'Private Function FixTime(ByVal TimeString As String) As String
    '    'incoming format is 08/11/2005 14:02:17 
    '    'Outgoing format is time only "14:33:17"
    '    Dim NewTime As String

    '    Dim mystart As Integer
    '    mystart = TimeString.IndexOf(" ")
    '    NewTime = Right(TimeString, mystart)
    '    ' WriteAuditEntry(Now.ToString & " FIXTIME ends with with " & NewTime)

    '    Return NewTime


    'End Function

#End Region


#Region "Log Files"


	Private Overloads Sub WriteAuditEntry(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
		MyBase.WriteAuditEntry(strMsg, "Database_Health_Log.txt", LogLevelInput)
	End Sub

	Private Overloads Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
		MyBase.WriteAuditEntry(strMsg, "Database_Health\" & DeviceType.Replace("/", "_").Replace("\", "_") & "_" & DeviceName.Replace("/", "_").Replace("\", "_") & "_Log.txt", LogLevelInput)
	End Sub


	'Private Sub WriteAuditEntry(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
	'    If LogLevelInput >= MyLogLevel Then
	'        strAuditText += strMsg & vbCrLf
	'    End If

	'End Sub

	'Private Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
	'    Dim DeviceLogDestination As String = ""
	'    Dim appendMode As Boolean = True

	'    If (LogLevelInput < MyLogLevel) Then
	'        Return
	'    End If


	'    Try
	'        DeviceLogDestination = strAppPath & "\Log_Files\Database_Health\" & DeviceType & "_" & DeviceName & "_Log.txt"
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

	'    '  Dim myRegistry As New RegistryHandler

	'    Try
	'        strLogDest = AppDomain.CurrentDomain.BaseDirectory.ToString & "\Log_Files\Database_Health_Log.txt"

	'    Catch ex As Exception

	'    End Try

	'    Try
	'        '   strLogDest = myRegistry.ReadFromRegistry("History Path")
	'        If File.Exists(strLogDest) Then
	'            File.Delete(strLogDest)
	'        End If
	'    Catch ex As Exception
	'        ' strLogDest = "c:\vitalsignslog.txt"
	'    End Try

	'    '
	'    'Dim elapsed As TimeSpan
	'    Dim OneHour As Integer = 60 * 60 * -1  'seconds
	'    Dim dtAuditHistory As DateTime

	'    Dim myLastUpdate As TimeSpan

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
	'            Catch ExAbort As ThreadAbortException
	'            End Try


	'            Try
	'                If strAuditText <> "" Then
	'                    sw.WriteLine(strAuditText)
	'                    strAuditText = ""
	'                    sw.Flush()
	'                    sw.Close()
	'                End If

	'            Catch ex As Exception
	'            Catch ExAbort As ThreadAbortException
	'            End Try

	'            Try
	'                sw.Close()
	'            Catch ex As Exception

	'            End Try
	'            sw = Nothing

	'            Thread.Sleep(4250)

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



    Public Sub IdentifyLastDBScan()

        Dim strSQL As String
        Dim vsObj As New VSAdaptor
        Dim myString As String = ""

        For Each Server As MonitoredItems.DominoServer In MyDominoServers
            WriteAuditEntry(Now.ToString & " *** Examining " & Server.Name, LogLevel.Verbose)

            Try
                If Server.Enabled = True And Server.ScanDBHealth = True Then
                    Server.IsBeingScanned = False
                    Server.LastDBHealthScan = Date.Today.AddDays(-7)

                    Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Database)(connectionString)
                    Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Database) = repo.Filter.Eq(Function(x) x.DeviceName, Server.Name)

                    Dim list As List(Of VSNext.Mongo.Entities.Database) = repo.Find(filterDef)


                    'strSQL = "Select MAX(ScanDate) AS 'MyDate' From Daily WHERE Server='" & Server.Name & "'"
                    'myString = vsObj.ExecuteScalarAny("VSS_Statistics", "NSFHealth", strSQL)
                    '  WriteAuditEntry(Now.ToString & " *** Server " & Server.Name & " database query returned the string: " & myString)
                    Try
                        'Server.LastDBHealthScan =
                        list.Sort(Function(x, y) x.ScanDateTime.Value.CompareTo(y.ScanDateTime.Value))
                        Server.LastDBHealthScan = list(0).ScanDateTime
                        'Server.LastDBHealthScan = CDate(myString)
                    Catch ex As Exception
                        Server.LastDBHealthScan = Date.Today.AddDays(-7)
                    End Try

                    ' WriteAuditEntry(Now.ToString & " *** Server " & Server.Name & " was last scanned " & Server.LastDBHealthScan.ToLongDateString)
                End If
            Catch ex As Exception
                Server.LastDBHealthScan = Date.Today.AddDays(-7)
                '   WriteAuditEntry(Now.ToString & " *** Error calculating last scan for Server " & Server.Name & ".  Error: " & ex.ToString)
                WriteAuditEntry(Now.ToString & " *** No data for Server " & Server.Name & " set to default value of last week-- " & Server.LastDBHealthScan)
            End Try
        Next

        strSQL = Nothing
        vsObj = Nothing

    End Sub

    Public Sub RemoveNullEntries()

        Try
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Database)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Database) = _
                repo.Filter.Exists(Function(x) x.FileName, False) And _
                repo.Filter.Exists(Function(x) x.Status, False)
            repo.Delete(filterDef)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error in RemoveNullEntries while deleteing entries. Error: " & ex.Message.ToString())
        End Try

    End Sub



End Class
