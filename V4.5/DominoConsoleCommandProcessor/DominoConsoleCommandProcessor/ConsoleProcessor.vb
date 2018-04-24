Imports System.Threading
Imports System.IO
Imports RPRWyatt.VitalSigns.Services

Imports MongoDB.Driver

Public Class ConsoleProcessor
	Inherits VSServices
    Dim connectionString As String
	Dim ProcessCommandsThread As New Thread(AddressOf ProcessCommands)
	Dim strAuditText, strLogDest As String
	Dim BuildNumber As Integer = 11
	'10/1/2014 NS added for VSPLUS-956
	Enum LogLevel
		Verbose = LogUtilities.LogUtils.LogLevel.Verbose
		Debug = LogUtilities.LogUtils.LogLevel.Debug
		Normal = LogUtilities.LogUtils.LogLevel.Normal
	End Enum
	Dim myRegistry As New VSFramework.RegistryHandler()

	'MyLogLevel is used throughout to control the volume of the log file output
	Dim MyLogLevel As LogLevel

	Protected Overrides Sub ServiceOnStart(ByVal args() As String)
		' Add code here to start your service. This method should set things
		' in motion so your service can do its work.
        '10/1/2014 NS added for VSPLUS-956

        Try
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings("VitalSignsMongo").ToString()
            WriteHistoryEntry(Now.ToString + " connection string is " & connectionString)
        Catch ex As Exception
            WriteHistoryEntry(Now.ToString + " Error getting connection string. Error: " & ex.Message)
        End Try

		Try
			MyLogLevel = myRegistry.ReadFromRegistry("Log Level")
		Catch ex As Exception
			'9/30/2014 NS modified - log level should be normal
			'MyLogLevel = LogLevel.Verbose
			MyLogLevel = LogLevel.Normal
		End Try

		strLogDest = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\Log_Files\"
		WriteHistoryEntry(Now.ToString & " The VitalSigns Domino Console Command Processor is starting up.", LogLevel.Normal)

		'10/1/2014 NS commented out the code below for VSPLUS-956
		'Try
		'Dim vsAdapter As New VSFramework.VSAdaptor
		'Dim strSQL As String = "Alter TABLE DominoConsoleCommands Add Result varchar(250)"
		'vsAdapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)
		'Catch ex As Exception
		'End Try

		ProcessCommandsThread.Start()
		Dim mySettings As New VSFramework.RegistryHandler
		mySettings.WriteToRegistry("Console Commands Start", Now.ToString)
		mySettings.WriteToRegistry("Console Commands Build", BuildNumber)
		'Sowjanya 1558 ticket
		Try
			myRegistry.WriteToRegistry("Console Commands Start", Now.ToShortDateString & " " & Now.ToShortTimeString)
			myRegistry.WriteToRegistry("Console Commands Build", BuildNumber)
		Catch ex As Exception

		End Try

	End Sub

	Protected Overrides Sub OnStop()
		' Add code here to perform any tear-down necessary to stop your service.
		WriteHistoryEntry(Now.ToString & " The VitalSigns Domino Console Command Processor is shutting down.", LogLevel.Normal)
		Dim mySettings As New VSFramework.RegistryHandler
		mySettings.WriteToRegistry("Console Commands End", Now.ToString)
		ProcessCommandsThread.Abort()
	End Sub

	Private Sub ProcessCommands()
		WriteHistoryEntry(Now.ToString & " The VitalSigns Domino Console Command Processor is beginning to process commands.", LogLevel.Normal)
		Dim MyDominoPassword As String
		Dim s As New Domino.NotesSession
		'Get the passwords from the registry
		Dim myRegistry As New VSFramework.RegistryHandler
		Dim MyPass As Byte()
		Try
			MyPass = myRegistry.ReadFromRegistry("Password")  'Domino password as encrypted byte stream
		Catch ex As Exception
			MyPass = Nothing
		End Try

		Dim mySecrets As New VSFramework.TripleDES
		Try
			If Not MyPass Is Nothing Then
				MyDominoPassword = mySecrets.Decrypt(MyPass) 'password in clear text, stored in memory now
				WriteHistoryEntry(Now.ToString & " The VitalSigns Domino Console Command Processor successfully decrypted the Notes password.")
			End If
		Catch ex As Exception
			WriteHistoryEntry(Now.ToString + ": Error retrieving Notes password  " & ex.Message, LogLevel.Normal)
			'MyDominoPassword = Nothing
		End Try


		Try
			s.Initialize(MyDominoPassword)
			WriteHistoryEntry(Now.ToString & " Initialized a session for  " & s.CommonUserName)

		Catch ex As Exception
			WriteHistoryEntry(Now.ToString + ": Error initializing NotesSession " & ex.Message, LogLevel.Normal)
			GoTo CleanUp
		End Try

        MyDominoPassword = ""
        Dim strResult As String = ""

		Try
            Do
                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.ConsoleCommands)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.ConsoleCommands) = repository.Filter.Exists(Function(x) x.DateTimeProcessed, False)
                WriteHistoryEntry(Now.ToString & " Before find")
                Dim listOfCommands As List(Of VSNext.Mongo.Entities.ConsoleCommands) = repository.Find(filterDef).ToList()
                WriteHistoryEntry(Now.ToString & " Before sort.")
                listOfCommands.Sort(Function(x, y) y.DateTimeSubmitted.Value.CompareTo(x.DateTimeSubmitted.Value))

                WriteHistoryEntry(Now.ToString & " I found " & listOfCommands.Count & " commands waiting to be sent.")
                If listOfCommands.Count > 0 Then

                    For Each command As VSNext.Mongo.Entities.ConsoleCommands In listOfCommands
                        Try
                            If IsAuthorized(command.Submitter) = True Then
                                WriteHistoryEntry(Now.ToString & " Sending '" & command.Command & "' to " & command.DeviceName & ", as requested by " & command.Submitter, LogLevel.Normal)
                                strResult = s.SendConsoleCommand(command.DeviceName, command.Command)
                                WriteHistoryEntry(Now.ToString & " The result from SendConsoleCommand is " & strResult)

                                If InStr(strResult, "You are not authorized") > 0 Then
                                    command.Result = "Not Authorized"
                                    command.DateTimeProcessed = DateTime.Now
                                Else
                                    command.Result = "Command Sent"
                                    command.DateTimeProcessed = DateTime.Now
                                End If

                            Else
                                command.Result = "Not Authorized"
                                command.DateTimeProcessed = DateTime.Now
                                WriteHistoryEntry(Now.ToString & " NOT Sending '" & command.Command & "' to " & command.DeviceName & ", as requested by " & command.Submitter & ".  This user is not authorized to send console commands via VitalSigns.", LogLevel.Normal)
                            End If

                        Catch ex As Exception
                            WriteHistoryEntry(Now.ToString & " Exception with SendConsoleCommand - " & ex.ToString, LogLevel.Normal)
                            If InStr(ex.ToString.ToLower, "not authorized") > 0 Then
                                command.Result = s.CommonUserName & " is not authorized to use the remote console on this server"
                                command.DateTimeProcessed = DateTime.Now
                            End If
                        End Try

                        repository.Replace(command)

                    Next

                End If
                'WriteHistoryEntry(Now.ToString & " Sleeping for 15 seconds.")
                Thread.Sleep(30000)
            Loop
        Catch ex As Exception
            WriteHistoryEntry(Now.ToString & " Error processing commands. Error: " & ex.Message)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(s)
		End Try

CleanUp:
		System.Runtime.InteropServices.Marshal.ReleaseComObject(s)

	End Sub


#Region "Log Files"

    Private Overloads Sub WriteHistoryEntry(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
        MyBase.WriteHistoryEntry(strMsg, "All_DominoConsoleCommands_Log.txt", LogLevelInput)
    End Sub

	'Private Sub WriteHistoryEntry(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Verbose)
	'	Dim DeviceLogDestination As String
	'	Dim appendMode As Boolean = True

	'	'10/1/2014 NS added for VSPLUS-956
	'	If (LogLevelInput < MyLogLevel) Then
	'		Return
	'	End If

	'	Try
	'		DeviceLogDestination = strLogDest & "All_DominoConsoleCommands_Log.txt"
	'		If InStr(DeviceLogDestination, "/") > 0 Then
	'			DeviceLogDestination = DeviceLogDestination.Replace("/", "_")
	'		End If
	'	Catch ex As Exception

	'	End Try

	'	Try
	'		Dim sw As New StreamWriter(DeviceLogDestination, appendMode, System.Text.Encoding.Unicode)
	'		sw.WriteLine(strMsg)
	'		sw.Close()
	'		sw = Nothing
	'	Catch ex As Exception

	'	End Try
	'	GC.Collect()
	'End Sub

#End Region



	Private Function IsAuthorized(Submitter As String) As Boolean

        'Check to see if the user is authorized to submit remote console commands via VitalSigns
        Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Users)(connectionString)
        Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Users) = (repository.Filter.Eq(Function(x) x.Email, Submitter) Or repository.Filter.Eq(Function(x) x.FullName, Submitter)) _
            And repository.Filter.AnyEq(Function(x) x.Roles, "RemoteConsole")
        Dim list As List(Of VSNext.Mongo.Entities.Users) = repository.Find(filterDef).ToList()

        If list.Count > 0 Or Submitter = "VitalSigns Domino Service" Then
            Return True
        End If

        Return False

        '      Dim vsAdapter As New VSFramework.VSAdaptor
        'Dim strSQL As String = "SELECT FullName FROM Users WHERE IsConsoleComm = 1"
        'Dim Result As Boolean = False
        'Dim dsConsoleUsers As New Data.DataSet
        'dsConsoleUsers.Tables.Add("Users")

        'dsConsoleUsers.Clear()
        'vsAdapter.FillDatasetAny("VitalSigns", "servers", strSQL, dsConsoleUsers, "Users")
        'WriteHistoryEntry(Now.ToString & " I found " & dsConsoleUsers.Tables("Users").Rows.Count & " authorized console users.")
        'If dsConsoleUsers.Tables("Users").Rows.Count > 0 Then
        '	Dim myView As New Data.DataView(dsConsoleUsers.Tables("Users"))
        '	myView.Sort = "FullName ASC"
        '	Dim drv As DataRowView
        '	For Each drv In myView
        '		If InStr(drv("FullName"), Submitter) Or drv("FullName") = Submitter Then
        '			Result = True
        '		End If
        '	Next
        'End If


        'If Submitter = "VitalSigns Domino Service" Then
        '	Result = True
        'End If

        'Return Result

    End Function





End Class
