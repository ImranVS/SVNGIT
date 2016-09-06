Imports System.Threading
Imports System.IO
Imports System.Globalization
Imports MonitoredItems.MonitoredDevice.Alert
Imports VSFramework
Imports System.Net.Sockets
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.Serialization.Json
Imports System.Runtime.Serialization

Imports MongoDB.Driver
Imports MongoDB.Bson.Serialization
Imports VSNext.Mongo.Entities

Partial Public Class VitalSignsPlusDomino


	Private DeviceTypeTranslationApple As New Dictionary(Of String, String)
	Private DeviceTypeTranslationAndroid As New Dictionary(Of String, String)
	Private OSTypeTranslationApple As New Dictionary(Of String, String)
	Private OSTypeTranslationAndroid As New Dictionary(Of String, String)
	Dim MOBILE_ALERT As String = "Mobile Users"
	Dim pm As Integer = 100
	Private DeviceIDTranslations As New System.Collections.Concurrent.ConcurrentDictionary(Of String, DeviceIDTranslationsObj)

	Public Class DeviceIDTranslationsObj

		Public Property DeviceOSType As String
		Public Property DeviceName As String
		Public Property DeviceOSTypeMin As String

		Public Sub New(ByVal DeviceName As String, ByVal DeviceOSType As String, ByVal DeviceOSTypeMin As String)
			Me.DeviceOSType = DeviceOSType
			Me.DeviceOSTypeMin = DeviceOSTypeMin
			Me.DeviceName = DeviceName
		End Sub


	End Class

	Private Sub CheckTravelerServer(ByRef myDominoServer As MonitoredItems.DominoServer)

		'  Dim MyStatValue As Double
		Dim Status As String = "OK"
        Dim Details As String = "OK"
        Dim strSQL As String = ""
        Dim DominoServerName As String = myDominoServer.Name
		If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Attempting to check Lotus Traveler health.")

		Try
			If myDominoServer.HierarchicalName = "" Then
				myDominoServer.HierarchicalName = ParseTextStatValue("Server.Name", myDominoServer.Statistics_Server)
			End If
		Catch ex As Exception

		End Try

		Dim counter As Int16 = 0
		Do While Not (InStr(myDominoServer.Statistics_Traveler, "Traveler.") > 0)
			If counter = 1 Then WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Attempting get Lotus Traveler statistics.")
			myDominoServer.Statistics_Traveler = GetStats(myDominoServer.Name, "Traveler", "")
			counter += 1
			If counter > 2 Then
				WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " I tried really hard to get Lotus Traveler statistics, but the server would not cooperate.")
				Exit Do
			End If

		Loop

		If myDominoServer.Statistics_Traveler = "" Or InStr(myDominoServer.Statistics_Traveler, "Not found") > 0 Then
			If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " No Traveler statistics available, exiting for now.")
			Exit Sub
		End If

		Dim objVSAdaptor As New VSAdaptor


		Try
            UpdateTravelerServerStatusTable(myDominoServer.Traveler_Version, myDominoServer.HTTP_Configured_Max_Sessions, myDominoServer.HTTP_Actual_Max_Sessions, Status, Details, myDominoServer.Name, myDominoServer.Traveler_Status, myDominoServer.Traveler_Details, myDominoServer.Traveler_UserCount, 1, myDominoServer.ServerObjectID)

        Catch ex As Exception

        End Try


		Try
			If Trim(myDominoServer.Traveler_Version) = "" Then
				If InStr("Traveler.Version", myDominoServer.Statistics_Traveler) > 0 Then
					myDominoServer.Traveler_Version = ParseTextStatValue("Traveler.Version", myDominoServer.Statistics_Traveler)
				Else
					myDominoServer.Traveler_Version = GetDominoTextStatistic(myDominoServer.Name, "Traveler", "Version")
				End If
				If InStr(myDominoServer.Traveler_Version, "*ERROR") Then myDominoServer.Traveler_Version = ""

				If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " This server is running Lotus Traveler version: " & myDominoServer.Traveler_Version)

			End If
		Catch ex As Exception
			myDominoServer.Traveler_Version = "n/a"
		End Try

		Try
			If myDominoServer.Traveler_Version <> "n/a" And myDominoServer.Traveler_Version <> "" Then


                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                     .Set(Function(x) x.TravelerVersion, myDominoServer.Traveler_Version)
                repository.Update(filterDef, updateDef)

			End If

		Catch ex As Exception

		End Try

		'Test for constrained state
		'  Traveler.Constrained.State = 0

        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.Name, DominoServerName)
            repository.Update(filterDef, updateDef)

            Dim Constrained As Integer = 0
            If InStr(myDominoServer.Statistics_Traveler, "Traveler.Constrained.State") Then
                Constrained = CInt(ParseNumericStatValue("Traveler.Constrained.State", myDominoServer.Statistics_Traveler))
                WriteDeviceHistoryEntry(myDominoServer.ServerType, myDominoServer.Name, Now.ToString & " Traveler.Constrained.State = " & Constrained)
                If Constrained = 0 Then
                    updateDef = updateDef.Set(Function(x) x.ResourceConstraint, "Pass")
                    myAlert.ResetAlert(myDominoServer.ServerType, myDominoServer.Name, "Traveler Resource Constraint", "The Traveler server is not operating under a detected resource constraint.")
                ElseIf Constrained = 1 Then
                    updateDef = updateDef.Set(Function(x) x.ResourceConstraint, "Fail")
                    Dim myDetails As String
                    myDetails = "Traveler is in a resource constraint state.   While the system is in constraint state, new device syncs will be denied with the 503 status code (server is busy).  Traveler will not allow new device sync or prime sync threads to start. Other threads will be allowed to complete and hopefully the constraint condition will be alleviated. "
                    myAlert.QueueAlert(myDominoServer.ServerType, myDominoServer.Name, "Traveler Resource Constraint", myDetails, myDominoServer.Location)
                End If
            End If

            repository.Update(filterDef, updateDef)
        Catch ex As Exception
            
            WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Exception when checking for Constrained State: " & ex.Message)
            Try
                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.ResourceConstraint, "Pass")
                repository.Update(filterDef, updateDef)
            Catch ex2 As Exception
                WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Exception when checking for Constrained State 2: " & ex2.Message)
            End Try
        End Try


		'Device Count
		Try
			If InStr(myDominoServer.Statistics_Traveler, "Traveler.Push.Devices.Total") Then
				myDominoServer.Traveler_DeviceCount = CInt(ParseNumericStatValue("Traveler.Push.Devices.Total", myDominoServer.Statistics_Traveler))
			Else
				Try
					WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Traveler.Push.Devices.Total was not available, so I am requesting it individually.")
					myDominoServer.Traveler_DeviceCount = CInt(GetOneStat(myDominoServer.Name, "Traveler", "Push.Devices.Total"))
				Catch ex As Exception
					WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Traveler.Push.Devices.Total still isn't available.  Sheesh.")
				End Try
			End If
			WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Traveler.Push.Devices.Total =" & myDominoServer.Traveler_DeviceCount)

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.TravelerDeviceCount, Convert.ToInt32(myDominoServer.Traveler_DeviceCount))
            repository.Update(filterDef, updateDef)

		Catch ex As Exception
			myDominoServer.Traveler_UserCount = -1
		End Try

		Try
			myDominoServer.Traveler_UserCount = ParseNumericStatValue("Traveler.Push.Users.Total", myDominoServer.Statistics_Traveler)

            If myDominoServer.Traveler_UserCount <> -1 Then
                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.TravelerUsers, Convert.ToInt32(myDominoServer.Traveler_UserCount))
                repository.Update(filterDef, updateDef)
            End If

		Catch ex As Exception

		End Try

		Try
            If myDominoServer.Traveler_Status <> "" Then
                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.TravelerStatus, myDominoServer.Traveler_Status)
                repository.Update(filterDef, updateDef)
            End If

		Catch ex As Exception

		End Try

		Try
            If myDominoServer.Traveler_Details <> "" Then
                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.TravelerDetails, myDominoServer.Traveler_Details)
                repository.Update(filterDef, updateDef)
            End If

		Catch ex As Exception

		End Try

		Try
			If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Checking HTTP health for Lotus Traveler")
			If InStr(myDominoServer.Statistics_HTTP, "Http.Workers") Then
				WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " This server has the statistic I need.")
				myDominoServer.HTTP_Configured_Max_Sessions = ParseNumericStatValue("Http.Workers", myDominoServer.Statistics_HTTP)
			Else
				WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " This server does NOT have the statistic I need.")
				myDominoServer.HTTP_Configured_Max_Sessions = GetOneStat(myDominoServer.Name, "HTTP", "Workers")
			End If
		Catch ex As Exception
			myDominoServer.HTTP_Configured_Max_Sessions = 0
		End Try

		Try
            If myDominoServer.HTTP_Configured_Max_Sessions <> 0 Then
                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-Domino")
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.HttpMaxConfiguredConnections, myDominoServer.HTTP_Configured_Max_Sessions)
                repository.Update(filterDef, updateDef)
                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " This server is configured to serve " & myDominoServer.HTTP_Configured_Max_Sessions & " simultaneous HTTP sessions.")
            End If

		Catch ex As Exception

		End Try


		Try
			If InStr(myDominoServer.Statistics_HTTP, "Http.PeakConnections") Then
				WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " This server has the statistic I need.")
				myDominoServer.HTTP_Actual_Max_Sessions = ParseNumericStatValue("Http.PeakConnections", myDominoServer.Statistics_HTTP)
			Else
				WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " This server does NOT have the statistic I need.")
				myDominoServer.HTTP_Actual_Max_Sessions = GetOneStat(myDominoServer.Name, "HTTP", "PeakConnections")
			End If
			If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " The most simultaneous HTTP sessions this server has served is " & myDominoServer.HTTP_Actual_Max_Sessions)
		Catch ex As Exception
			myDominoServer.HTTP_Actual_Max_Sessions = 0
		End Try


		Try
            If myDominoServer.HTTP_Actual_Max_Sessions <> 0 Then
                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.HttpPeakConnections, myDominoServer.HTTP_Actual_Max_Sessions)
                repository.Update(filterDef, updateDef)
                '  If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " This server is configured to serve " & myDominoServer.HTTP_Configured_Max_Sessions & " simultaneous HTTP sessions.")
            End If

		Catch ex As Exception

		End Try

		Try
			Dim myHTTPPercent As Decimal
			If myDominoServer.HTTP_Actual_Max_Sessions > 0 And myDominoServer.HTTP_Configured_Max_Sessions <> 0 Then
				myHTTPPercent = myDominoServer.HTTP_Actual_Max_Sessions / myDominoServer.HTTP_Configured_Max_Sessions
				Details = "The peak usage of HTTP sessions is " & myHTTPPercent.ToString("P") & " of the maximum worker threads available."
			Else
                Details = "Unavailable"
			End If

		Catch ex As Exception

		End Try

		Try
			With myDominoServer
				If .HTTP_Actual_Max_Sessions * 1.2 > .HTTP_Configured_Max_Sessions And .HTTP_Actual_Max_Sessions > 1 And .HTTP_Configured_Max_Sessions <> 0 Then
					.Traveler_Overall_Health = "The maximum number of actual HTTP sessions is approaching the configured limit of " & .HTTP_Configured_Max_Sessions
					.Description = "The maximum number of actual HTTP sessions is approaching the configured limit of " & .HTTP_Configured_Max_Sessions
					.Traveler_Status = "HTTP Sessions Warning"
					Status = "HTTP Sessions Warning"
                    Details = "The maximum number of actual HTTP sessions is approaching the configured server limit of " & .HTTP_Configured_Max_Sessions
                    Details = "Warning"
                    myAlert.QueueAlert(.ServerType, .Name, "Traveler Threads Warning", Details, .Location, "Traveler")
				Else
                    myAlert.ResetAlert(.ServerType, .Name, "Traveler Threads Warning", .Location, "The Traveler server has sufficient HTTP threads", "Traveler")
				End If

				If .HTTP_Actual_Max_Sessions = .HTTP_Configured_Max_Sessions And .HTTP_Actual_Max_Sessions > 1 And .HTTP_Configured_Max_Sessions <> 0 Then
					.Traveler_Overall_Health = "The maximum number of actual HTTP sessions has hit the configured limit of " & .HTTP_Configured_Max_Sessions
					.Description = "The maximum number of actual HTTP sessions has hit the configured limit of " & .HTTP_Configured_Max_Sessions & ". This can affect Traveler performance."
					.Traveler_Status = "Insufficient HTTP Sessions"
					Status = "Insufficient HTTP Sessions"
                    'Details = "The maximum number of actual HTTP sessions has hit the configured limit of " & .HTTP_Configured_Max_Sessions & ". This can adversely affect Traveler performance."
                    Details = "Insufficient"
                    myAlert.QueueAlert(.ServerType, .Name, "Traveler Insufficient Threads", Details, .Location, "Traveler")
				Else
                    myAlert.ResetAlert(.ServerType, .Name, "Traveler Insufficient Threads", .Location)
				End If
			End With
		Catch ex As Exception

		End Try

		Try
			Dim myActiveUsers As Integer
			Dim Location As Integer
			Location = InStr(myDominoServer.Traveler_Details, "of")
			myActiveUsers = CInt(Trim(Mid(myDominoServer.Traveler_Details, 1, Location - 1)))
			WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Traveler server currently has " & myActiveUsers.ToString & " active users.")

            If myActiveUsers = 0 Then

                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.TravelerDescription, "No Traveler Users are Active")
                repository.Update(filterDef, updateDef)
            End If
		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Exception calculating active users: " & ex.ToString)
		End Try


		Try
			Dim boolHTTPRunning As Boolean = False
			WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Checking to see that HTTP is running.")
			For Each Task As MonitoredItems.ServerTask In myDominoServer.ServerTasks
				' WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Found " & Task.Name & " / " & Task.Status)
				If (Task.Name = "HTTP" And Task.Status <> "Not Found") Or Task.Name = "HTTP Server" Then
					boolHTTPRunning = True
					WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " HTTP is Found ")
				End If
			Next

			If boolHTTPRunning = False Then
				Status = "Not Running"
				myDominoServer.Description = "Traveler is disabled because HTTP is not running"

                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                     .Set(Function(x) x.TravelerStatus, "Failure") _
                                                                                     .Set(Function(x) x.HttpStatus, Status) _
                                                                                     .Set(Function(x) x.HttpDetails, "n/a") _
                                                                                     .Set(Function(x) x.Description, myDominoServer.Description)
                repository.Update(filterDef, updateDef)
			End If
		Catch ex As Exception

		End Try


		Try
			Dim boolTravelerRunning As Boolean = False
			WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Checking to see that Traveler is running.")
			For Each Task As MonitoredItems.ServerTask In myDominoServer.ServerTasks
				WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Found " & Task.Name & " / " & Task.Status)
				If (InStr(Task.Name, "Traveler" Or Task.Name = "Traveler 9+") And Task.Status <> "Missing") Then
					boolTravelerRunning = True
					WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Traveler is Found ")
				End If
			Next

			If boolTravelerRunning = False Then
				WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Traveler is not running on this Traveler server.  This is generally bad. ")
				Details = "The Traveler task is not running on this server."
				myDominoServer.Description = "Traveler is not running"

                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                     .Set(Function(x) x.TravelerStatus, "Failure") _
                                                                                     .Set(Function(x) x.TravelerDetails, Details) _
                                                                                     .Set(Function(x) x.Description, myDominoServer.Description)
                repository.Update(filterDef, updateDef)
			Else
				WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Traveler is running on this Traveler server.  This is good. ")
			End If
		Catch ex As Exception

		End Try


		With myDominoServer

			Try
				If InStr(.Statistics_Traveler, "Traveler.DeviceSync.Count.200") > 0 Then
					.Traveler_Successful_DeviceSync_Count = ParseNumericStatValue("Traveler.DeviceSync.Count.200", .Statistics_Traveler)
					WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Traveler.DeviceSync.Count.200 is " & .Traveler_Successful_DeviceSync_Count)
					WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Previous value of Traveler.DeviceSync.Count.200 was " & .Traveler_Previous_Successful_DeviceSync_Count)

					'Calculate the incremental successful sync
					Dim intSuccessfulSyncCount As Integer

					intSuccessfulSyncCount = .Traveler_Successful_DeviceSync_Count - .Traveler_Previous_Successful_DeviceSync_Count
					WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " This Traveler server has successfully synced " & intSuccessfulSyncCount & " devices since the last scan at " & myDominoServer.LastScan.ToString)
					Details = intSuccessfulSyncCount & " successful device syncs since the last scan."

                    Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                    Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
                    Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                         .Set(Function(x) x.TravelerIncrementalSyncs, intSuccessfulSyncCount)
                    repository.Update(filterDef, updateDef)

                    UpdateDominoDailyStatTable(myDominoServer, "Traveler.IncrementalDeviceSyncs", intSuccessfulSyncCount)

					If .Traveler_Successful_DeviceSync_Count = .Traveler_Previous_Successful_DeviceSync_Count Then
						'The value is not increasing, perhaps Traveler is hung.  Let's see how long it has been
						Dim ScanInterval As TimeSpan
						Try
							Dim tNow As New DateTime
							tNow = Now
							ScanInterval = tNow.Subtract(.Traveler_Previous_Successful_DeviceSync_Count_Updated_Time)
							If ScanInterval.TotalMinutes > 30 Then
								.Status = "Traveler Not Syncing"
								.Traveler_Status = "Not Syncing"
                                myAlert.QueueAlert(.ServerType, .Name, "Traveler Device Sync", "The device synchronization counts for the Lotus Traveler server task haven't changed in " & ScanInterval.TotalMinutes & " minutes, and this COULD indicate a problem with Lotus Traveler. Then again, it could just indicate that nobody has been retrieving their mail remotely in a while.  Still, VitalSigns thought you should know.  When the number changes you'll get another message stating that fact.", .Location)

                                updateDef = repository.Updater.Set(Function(x) x.TravelerStatus, "Not Syncing")
                                repository.Update(filterDef, updateDef)

							End If
						Catch ex As Exception

						End Try

					Else
                        myAlert.ResetAlert(.ServerType, .Name, "Traveler Device Sync", .Location)
					End If
				Else

                    UpdateDominoDailyStatTable(myDominoServer, "Traveler.IncrementalDeviceSyncs", 0)

				End If

			Catch ex As Exception

			End Try


			'Traveler_Availability_Index
			Try
				If InStr(myDominoServer.Statistics_Traveler, "Traveler.Availability.Index.Current") Then
					'   WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & vbCrLf & myDominoServer.Statistics_Traveler)
					myDominoServer.Traveler_Availability_Index = ParseNumericStatValue("Traveler.Availability.Index.Current", myDominoServer.Statistics_Traveler)

					WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Traveler.Availability.Index.Current:  " & myDominoServer.Traveler_Availability_Index)

                    Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                    Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.TypeAndName.Equals(DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription()))
                    Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                         .Set(Function(x) x.TravelerAvailabilityIndex, Convert.ToInt32(myDominoServer.Traveler_Availability_Index))
                    repository.Update(filterDef, updateDef)

				End If

			Catch ex As Exception
				WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Exception determining Traveler.Status.State:  " & ex.ToString)
			End Try

            '10/8/2015 NS added for VSPLUS-2208
            'Traveler_Memory_Java
            Try
                If InStr(myDominoServer.Statistics_Traveler, "Traveler.Memory.Java.Current") > 0 Then
                    myDominoServer.Traveler_Memory_Java = ParseNumericStatValue("Traveler.Memory.Java.Current", myDominoServer.Statistics_Traveler)

                    WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Traveler.Memory.Java.Current:  " & myDominoServer.Traveler_Memory_Java)
                    UpdateDominoDailyStatTable(myDominoServer, "Traveler.Memory.Java.Current", myDominoServer.Traveler_Memory_Java)
                End If

            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Exception determining Traveler.Memory.Java.Current:  " & ex.ToString)
            End Try

            'Traveler_Memory_C
            Try
                If InStr(myDominoServer.Statistics_Traveler, "Traveler.Memory.C.Current") > 0 Then
                    myDominoServer.Traveler_Memory_C = ParseNumericStatValue("Traveler.Memory.C.Current", myDominoServer.Statistics_Traveler)

                    WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Traveler.Memory.C.Current:  " & myDominoServer.Traveler_Memory_C)
                    UpdateDominoDailyStatTable(myDominoServer, "Traveler.Memory.C.Current", myDominoServer.Traveler_Memory_C)
                End If

            Catch ex As Exception
                WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Exception determining Traveler.Memory.C.Current:  " & ex.ToString)
            End Try

			WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Finished monitoring Lotus Traveler")

			Exit Sub



		End With



	End Sub

	Private Sub CheckTravelerState(ByRef myDominoServer As MonitoredItems.DominoServer)
		'This sub checks whether a Traveler server is Green, Yellow, or Red
		Dim strSQL As String = ""
        Dim objVSAdaptor As New VSAdaptor
        Dim DominoServerName As String = myDominoServer.Name

		Try
			myDominoServer.Traveler_Status_Prior = myDominoServer.Traveler_Status
			myDominoServer.Traveler_Status = ""
			If InStr(myDominoServer.Statistics_Traveler, "Traveler.Status.State") Then
				'   WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & vbCrLf & myDominoServer.Statistics_Traveler)
				myDominoServer.Traveler_Status = ParseTextStatValue("Traveler.Status.State", myDominoServer.Statistics_Traveler)
				If myDominoServer.Traveler_Status.Trim = "" Then
					myDominoServer.Traveler_Status = GetOneStat(myDominoServer.Name, "Traveler", "Status.State")
				End If
				WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Traveler.Status.State:  " & myDominoServer.Traveler_Status)
			End If


		Catch ex As Exception
			WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Exception determining Traveler.Status.State:  " & ex.ToString)
		End Try

		Try
			'Will issue tell command if the status has changed AND the server name is in the comma seperated registry value
			Dim reg As New RegistryHandler()
			Dim regValue As String = reg.ReadFromRegistry("Traveler Status Send Tell Command").ToString()

			If myDominoServer.Traveler_Status_Prior <> myDominoServer.Traveler_Status And regValue.Split(",").Contains(myDominoServer.Name, StringComparer.CurrentCultureIgnoreCase) Then
				SendDominoConsoleCommands(myDominoServer.Name, "tell traveler status", "Sending tell traveler status command to refresh the stat table.")
				Thread.Sleep(1000 * 30)
				myDominoServer.Traveler_Status = GetOneStat(myDominoServer.Name, "Traveler", "status.state")

			End If
		Catch ex As Exception

		End Try

        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                 .Set(Function(x) x.TravelerStatus, myDominoServer.Traveler_Status)
            repository.Update(filterDef, updateDef)

        Catch ex As Exception

        End Try


		Try
            If myDominoServer.Traveler_Status = "Red" Then
                '5/20/2016 NS modified for VSPLUS-2874
                myDominoServer.Status = "Traveler Status: Red"
                myDominoServer.ResponseDetails = "The overall status of IBM Notes Traveler is Red"
                myDominoServer.StatusCode = "Issue"

                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                     .Set(Function(x) x.CurrentStatus, myDominoServer.Status) _
                                                                                     .Set(Function(x) x.StatusCode, myDominoServer.StatusCode) _
                                                                                     .Set(Function(x) x.Details, myDominoServer.ResponseDetails)
                repository.Update(filterDef, updateDef)
            End If

		Catch ex As Exception

		End Try


		Try
            If myDominoServer.Traveler_Status = "Yellow" Then
                '5/20/2016 NS modified for VSPLUS-2874
                myDominoServer.Status = "Traveler Status: Yellow"
                myDominoServer.ResponseDetails = "The overall status of IBM Notes Traveler is Yellow"
                myDominoServer.StatusCode = "Issue"

                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Where(Function(x) x.Name = DominoServerName And x.TypeAndName = DominoServerName & "-" & VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                     .Set(Function(x) x.CurrentStatus, myDominoServer.Status) _
                                                                                     .Set(Function(x) x.StatusCode, myDominoServer.StatusCode) _
                                                                                     .Set(Function(x) x.Details, myDominoServer.ResponseDetails)
                repository.Update(filterDef, updateDef)
            End If

		Catch ex As Exception

		End Try


	End Sub


	Private Sub UpdateTravelerHeartbeat(ByVal srv As MonitoredItems.DominoServer)

		If srv.Traveler_Server_HA = False Then Exit Sub

		Dim objVSAdaptor As New VSAdaptor
		Dim strSQL As String = ""
		Dim myHeartBeat As String = ""
		WriteDeviceHistoryEntry("All", "Traveler_Servers", Now.ToString & " Attempting to get heartbeat for HA servers")
		'Figure out the heartbeat 

		Dim myTravelerServers As New travelerServers
		Dim htmlStream = getHTMLStream("/api/traveler/servers", srv)
		WriteDeviceHistoryEntry("All", "Traveler_Servers", Now.ToString & ":traveler servers:" & htmlStream)
		If htmlStream <> "" Then
			WriteDeviceHistoryEntry("All", "Traveler_Servers", Now.ToString & ":traveler servers:Convert to Object")
			myTravelerServers = returnTravelerServersObject(htmlStream)
		End If

		'we have the servers collection
		If myTravelerServers IsNot Nothing Then

			WriteDeviceHistoryEntry("All", "Traveler_Servers", Now.ToString & ":traveler servers collection:" & srv.Name)
			For Each server As travelerServer In myTravelerServers.data
				If server.heart_beat <> "" Then
					WriteDeviceHistoryEntry("All", "Traveler_Servers", Now.ToString & ":traveler servers:" & server.domino_name)
					WriteDeviceHistoryEntry("All", "Traveler_Servers", Now.ToString & " The heart beat in EPOCH is " & server.heart_beat)
					myHeartBeat = convertEPOCHTime(server.heart_beat)
					WriteDeviceHistoryEntry("All", "Traveler_Servers", Now.ToString & ":traveler servers heartbeat in human " & myHeartBeat)
					Try
						WriteDeviceHistoryEntry("All", "Traveler_Servers", Now.ToString & ":traveler servers: Update the database for the sever:" & server.domino_name)

                        Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                        Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.TravelerHeartBeat, myHeartBeat)
                        Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.Name, server.domino_name) And repository.Filter.Eq(Function(x) x.Type, VSNext.Mongo.Entities.Enums.ServerType.Domino.ToDescription())

                        repository.Update(filterDef, updateDef)

						WriteDeviceHistoryEntry("All", "Traveler_Servers", Now.ToString & ":traveler servers : Update done.")
					Catch ex As Exception
						WriteDeviceHistoryEntry("All", "Traveler_Servers", Now.ToString & ":traveler servers: error:" & ex.Message.ToString)
					End Try

				End If
			Next
		End If
		'End If
		'Next
		'strSQL = "Update Traveler_Status SET  HeartBeat='" & myHeartBeat & "' WHERE ServerName='" & MyDominoServer.Name & "'"
		'WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " " & strSQL)
		'objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)

	End Sub

	Private Sub CycleTravelerUsers()
		WriteDeviceHistoryEntry("All", "CycleTravelerUsers", Now.ToString & " Begin cycle Traveler users.")
		'Get the Decode Values from Database
		GetDeviceTypeTranslationvalues()
		GetOSTypeTranslationvalues()
		Dim ScanCandidates As New MonitoredItems.DominoCollection
		For Each srv As MonitoredItems.DominoServer In MyDominoServers
			If srv.Enabled = True Then
				' WriteDeviceHistoryEntry("All", "CycleTravelerUsers", Now.ToString & " Processing scan candidate:  " & srv.Name)
				ScanCandidates.Add(srv)
				srv.Traveler_User_Scanning = False
			End If
		Next

		WriteDeviceHistoryEntry("All", "CycleTravelerUsers", Now.ToString & " Got scan candidates, now starting loop.  ")

		Do Until boolTimeToStop = True
			'continually loop through all the servers
			For Each srv As MonitoredItems.DominoServer In ScanCandidates
				Threading.Thread.Sleep(500)
				Try
					If srv.Traveler_Server = True And srv.ScanTravelerServer = True Then
						If srv.Traveler_User_Scanning = False Then
							srv.Traveler_User_Scanning = True
							WriteDeviceHistoryEntry("All", "CycleTravelerUsers", Now.ToString & " Creating a new thread for  " & srv.Name)
							Dim tTemp As New Thread(AddressOf CheckTravelerUsersLoop)
							ListOfTravelerUserThreads.Add(tTemp)
							tTemp.Start(srv.Name)
						End If
					End If

				Catch ex As Exception
					WriteDeviceHistoryEntry("All", "CycleTravelerUsers", Now.ToString & " Error in CycleTravelerUsers cycle with " & srv.Name & ":  " & ex.ToString)
				End Try

			Next
			' NEED TO IMPLEMENT 
			' IF there is a new or update to the configuration of domino servers Stop the thread for that server Name and span a new th

			Threading.Thread.Sleep(10000)
		Loop

	End Sub

	Private Sub GetDeviceTypeTranslationvalues()
		Dim myConnectionString As New VSFramework.XMLOperation
		Dim myAdapter As New VSFramework.VSAdaptor
		WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Get Distinct DeviceTypeTranslation List ")
		Dim sSQLServers As String = "SELECT DISTINCT DeviceType,TranslatedValue  FROM DeviceTypeTranslation where OSName='Apple'"
		Try
			DeviceTypeTranslationApple.Clear()
			Dim dtDeviceTypeList As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sSQLServers)
			WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Get Distinct DeviceTypeTranslationApple List Count: " + dtDeviceTypeList.Rows.Count.ToString)
			For Each drDeviceTypeList As DataRow In dtDeviceTypeList.Rows
				DeviceTypeTranslationApple.Add(drDeviceTypeList(0).ToString, drDeviceTypeList(1).ToString)
			Next
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Get Distinct DeviceTypeTranslationApple List: Error: " + ex.Message.ToString)
		End Try

		sSQLServers = "SELECT DISTINCT DeviceType,TranslatedValue  FROM DeviceTypeTranslation where OSName='Android'"
		Try
			DeviceTypeTranslationAndroid.Clear()
			Dim dtDeviceTypeList As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sSQLServers)
			WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Get Distinct DeviceTypeTranslationAndroid List Count: " + dtDeviceTypeList.Rows.Count.ToString)
			For Each drDeviceTypeList As DataRow In dtDeviceTypeList.Rows
				DeviceTypeTranslationAndroid.Add(drDeviceTypeList(0).ToString.ToLower, drDeviceTypeList(1).ToString)
			Next
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Get Distinct DeviceTypeTranslationAndroid List: Error: " + ex.Message.ToString)
		End Try

    End Sub

	Private Sub GetOSTypeTranslationvalues()
		Dim myConnectionString As New VSFramework.XMLOperation
		Dim myAdapter As New VSFramework.VSAdaptor
		WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Get Distinct OSTypeTranslation List ")
		Dim sSQLServers As String = "SELECT DISTINCT OSType,TranslatedValue  FROM OSTypeTranslation WHERE OSName='Apple'"
		Try
			OSTypeTranslationApple.Clear()

			Dim dtOSTypeList As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sSQLServers)
			WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Get Distinct OSTypeTranslationApple List Count: " + dtOSTypeList.Rows.Count.ToString)
			For Each drOSTypeList As DataRow In dtOSTypeList.Rows
				OSTypeTranslationApple.Add(drOSTypeList(0).ToString, drOSTypeList(1).ToString)
			Next
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Get Distinct OSTypeTranslationApple List: Error: " + ex.Message.ToString)
		End Try

		sSQLServers = "SELECT DISTINCT OSType,TranslatedValue  FROM OSTypeTranslation WHERE OSName='Android'"
		Try
			OSTypeTranslationAndroid.Clear()

			Dim dtOSTypeList As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sSQLServers)
			WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Get Distinct OSTypeTranslationAndroid List Count: " + dtOSTypeList.Rows.Count.ToString)
			For Each drOSTypeList As DataRow In dtOSTypeList.Rows
				OSTypeTranslationAndroid.Add(drOSTypeList(0).ToString, drOSTypeList(1).ToString)
			Next
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Get Distinct OSTypeTranslationAndroid List: Error: " + ex.Message.ToString)
		End Try

	End Sub
    'Private Sub CycleTravelerUsersOLD()
    '	Dim ScanCandidates As New MonitoredItems.DominoCollection
    '	For Each srv As MonitoredItems.DominoServer In MyDominoServers
    '		If srv.Enabled = True Then
    '			ScanCandidates.Add(srv)
    '		End If
    '	Next
    '	Dim OneMinute As Integer = 60000  '60,000 milliseconds
    '	Dim oWatch As New System.Diagnostics.Stopwatch

    '	Do Until boolTimeToStop = True
    '		'continually loop through all the servers
    '		For Each srv As MonitoredItems.DominoServer In ScanCandidates
    '			Threading.Thread.Sleep(500)
    '			Try
    '				If srv.Traveler_Server = True Then
    '					'Don't bother checking to see if it is in maintenance if it isn't even a traveler server
    '					If InMaintenance("Domino", srv.Name) = False Then

    '						Try
    '							oWatch.Reset()
    '							oWatch.Start()
    '							WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Starting to check Traveler users RELEASE 9 for " & srv.Name)
    '							CheckTravelerUsersUsingJSON(srv)
    '							oWatch.Stop()
    '							WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Finished Traveler users Release 9 for " & srv.Name & ". Elapsed time = " & oWatch.Elapsed.TotalMinutes & " minutes.")
    '						Catch ex As Exception
    '							WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Unhandled exception with getting Traveler Users:" & ex.ToString)
    '						End Try


    '						Select Case ScanCandidates.Count
    '							Case 1
    '								'If there is only one traveler server, scan it every 15 minutes
    '								Threading.Thread.Sleep(OneMinute * 15)
    '							Case 2
    '								'If there are two Travelers servers, wait 10 minutes between scans and alternate between them 
    '								Threading.Thread.Sleep(OneMinute * 10)
    '							Case 3
    '								Threading.Thread.Sleep(OneMinute * 5)
    '							Case Else
    '								Threading.Thread.Sleep(OneMinute * 2)
    '						End Select

    '					End If

    '				End If

    '			Catch ex As Exception
    '				WriteDeviceHistoryEntry("All", "CycleTravelerUsers", Now.ToString & " Error in CycleTravelerUsers cycle with " & srv.Name & ":  " & ex.ToString)
    '			End Try

    '		Next

    '	Loop

    'End Sub

	Private Sub CheckTravelerUsersLoop(ByVal ServerName As String)
		' Traveler Users
		WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Starting a new thread for  " & ServerName)
		Dim MyDominoServer As MonitoredItems.DominoServer
		Try
			MyDominoServer = MyDominoServers.Search(ServerName)
			WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " Starting a new thread for  " & MyDominoServer.Name)
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Exception locating server object for " & ServerName & " " & ex.ToString)
		End Try

		Try
			If MyDominoServer Is Nothing Then
				Exit Sub
			End If
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Exception locating server object for " & ServerName & " " & ex.ToString)
		End Try

		Dim objVSAdaptor As New VSAdaptor
		Dim OneMinute As Long = 1000 * 60
		Try
			Do Until boolTimeToStop = True
				WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " Begin user check for " & MyDominoServer.Name)
				Try
					'get the pagemultiplier setting
					WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " Getting the page Multiplier Setting. ")
					Dim strSQL As String = "Select svalue from dbo.settings where sname='TravelerPageMultiplier'"
					pm = objVSAdaptor.ExecuteScalarAny("VitalSigns", "Settings", strSQL)
					If pm < 10 Then
						pm = 100
					End If
					WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " Getting the page Multiplier Setting. The Value is: " + pm.ToString())
				Catch ex As Exception
					pm = 100
					WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " Error Getting the page Multiplier Setting. " + ex.Message.ToString())
				End Try
				If InMaintenance("Domino", MyDominoServer.Name) = False Then
					Try
                        CheckTravelerUsersUsingJSONMongo(MyDominoServer)
					Catch ex As Exception
						WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " Got Exception " & ex.ToString)
					End Try
				End If

				Thread.Sleep(MyDominoServer.ScanInterval * OneMinute)
			Loop
		Catch ex As Exception

		End Try


	End Sub

    'Private Sub GetMoreInfoForDevice(data As Object)
    '	Dim parameters As New MyParameters
    '	parameters = CType(data, MyParameters)
    '	Dim device As TravelerDevice
    '	Dim myMasterDevices As devices
    '	Dim myDominoServer As MonitoredItems.DominoServer

    '	'device = parameters.device
    '	'myMasterDevices = parameters.myMasterDevices
    '	'myDominoServer = parameters.myDominoServer

    '	Dim htmlStream As String = ""
    '	If device.href <> "" Then
    '		'get more info about this device
    '		Dim myDeviceMoreInfo As New devicesMoreInfo
    '		htmlStream = getHTMLStream(device.href, myDominoServer)
    '		' WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " URL for more info " & device.href & vbCrLf)
    '		If htmlStream <> "" Then
    '			'WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " I got back " & vbCrLf & htmlStream)
    '			myDeviceMoreInfo = returnDevicesMoreInfoObject(htmlStream)
    '			WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " myDeviceMoreInfo code " & myDeviceMoreInfo.code & vbCrLf)
    '			If myDeviceMoreInfo IsNot Nothing Then
    '				WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " myDeviceMoreInfo is not nothing " & myDeviceMoreInfo.message & vbCrLf)
    '				'WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " myDeviceMoreInfo is not nothing " & myDeviceMoreInfo.data.DeviceID.ToString & vbCrLf)
    '				If myDeviceMoreInfo.data IsNot Nothing Then
    '					Try
    '						' WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " myDeviceMoreInfo is not nothing " & myDeviceMoreInfo.data.DeviceID.ToString & vbCrLf)
    '						'add the more info object
    '						'  WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Adding More info object to  myMasterDevices " & vbCrLf)
    '						If myDeviceMoreInfo.data.LastSyncTime <> "" Then
    '							myDeviceMoreInfo.data.LastSyncTime = convertEPOCHTime(myDeviceMoreInfo.data.LastSyncTime)
    '						End If
    '						myMasterDevices.data.Add(myDeviceMoreInfo.data)
    '						'security  URL. we do not have to worry about it now
    '						'If deviceMoreInfo.href <> "" Then
    '						'End If
    '					Catch ex As Exception
    '						'  WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error adding device to collection #1 " & ex.ToString)
    '						'if there is an exception add the device, if not: myDeviceMoreInfo.data
    '						myMasterDevices.data.Add(device)
    '						WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Adding Less info Object since there was an error ")
    '					End Try
    '				Else
    '					WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Adding Less info Object since the moreinfo data was nothing ")
    '					'moreinfo data is empty, add the less info object 
    '					myMasterDevices.data.Add(device)

    '				End If
    '			End If
    '		End If
    '	Else
    '		Try
    '			'add the less info object if more information is not present

    '			device.LastSyncTime = convertEPOCHTime(device.LastSyncTime)
    '			myMasterDevices.data.Add(device)
    '		Catch ex As Exception
    '			WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error adding device to collection #2 " & ex.ToString)
    '		End Try

    '	End If


    'End Sub

	'Private Sub CheckTravelerUsersUsingJSON(ByRef myDominoServer As MonitoredItems.DominoServer)
	'    ' Traveler Users
	'    Dim myMasterDevices As New devices

	'    Dim myDevices As New devices
	'    Dim isFirstFetch As Boolean = True
	'    Dim htmlStream As String = ""
	'    Dim nextPage As String = ""
	'    ' we can loop thru the myDevices now. first go, isFirstFetch=true
	'    While (myDevices IsNot Nothing) AndAlso (nextPage <> "" Or isFirstFetch)
	'        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Top of while loop.")
	'        htmlStream = getHTMLStream(myDevices.next1, myDominoServer)
	'        If htmlStream <> "" Then
	'            '  WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " I got back " & vbCrLf & htmlStream)
	'            myDevices = returnDevicesObject(htmlStream)
	'        End If

	'        If myDevices IsNot Nothing Then
	'            'fill the master object with header information
	'            If isFirstFetch Then
	'                myMasterDevices.code = myDevices.code
	'                myMasterDevices.message = myDevices.message
	'                'don't think we need these three
	'                myMasterDevices.href = myDevices.href
	'                myMasterDevices.next1 = myDevices.next1
	'                myMasterDevices.totalRecords = myDevices.totalRecords
	'            End If
	'            isFirstFetch = False
	'            Dim ListOfTravelerUserDetailThreads As List(Of Threading.Thread) = New List(Of Threading.Thread)
	'            For Each device As TravelerDevice In myDevices.data
	'                ' WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Inside For loop to get more information about device.")
	'                'if more information is present

	'                Try
	'                    Dim threadGetMoreInfo As New Thread(AddressOf GetMoreInfoForDevice)
	'                    ListOfTravelerUserDetailThreads.Add(threadGetMoreInfo)
	'                    Dim parameters As MyParameters = New MyParameters
	'                    parameters.device = device
	'                    parameters.myDominoServer = myDominoServer
	'                    parameters.myMasterDevices = myMasterDevices
	'                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Calling the Detailed as a thread")
	'                    threadGetMoreInfo.Start(parameters)
	'                Catch ex As Exception
	'                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & ex.ToString)
	'                End Try
	'            Next

	'            For Each t As Thread In ListOfTravelerUserDetailThreads
	'                t.Join()
	'            Next

	'        End If
	'        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Successfully added a page of devices. Move to next set now ")
	'        Try
	'            nextPage = myDevices.next1.ToString
	'        Catch ex As Exception
	'            nextPage = ""
	'            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "  Successfully added a page of devices. Move to next set now. Error looking for next set " & ex.ToString)

	'        End Try
	'        '  WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "  Successfully added a page of devices. Move to next set now. Error looking for next set: Next page url: " & nextPage)

	'        If nextPage = "" Then
	'            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "  Successfully added a page of devices. Move to next set now. Error looking for next set: Next page url: EXIT WHILE ")
	'            Exit While
	'        End If
	'    End While

	'    'At this point we have myMasterDevices object filled with all information
	'    'we can iterate thru the myMasterDevices list and pump the values into TravelerDevice
	'    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Attempting to populate Traveler User info...")
	'    If myMasterDevices IsNot Nothing Then
	'        ' WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Attempting to update Traveler User info. myMasterDevices is not nothing")
	'        Try
	'            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Deleting previous Traveler devices for this server.")
	'            EmptyTravelerDeviceTable(myDominoServer.Name)
	'        Catch ex As Exception

	'        End Try


	'        If myMasterDevices.data IsNot Nothing Then
	'            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Attempting to insert Traveler User info. myMasterDevices.data has " & myMasterDevices.data.Capacity & " Records ")
	'            For Each myTravelerDevice As TravelerDevice In myMasterDevices.data
	'                Try

	'                    ' WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Username: " & myTravelerDevice.UserName)
	'                    ' WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Policy State: " & myTravelerDevice.policy_compliance)
	'                    Select Case myTravelerDevice.policy_compliance.ToString
	'                        Case "0"
	'                            myTravelerDevice.policy_compliance = "No Policy"
	'                        Case "1"
	'                            myTravelerDevice.policy_compliance = "Client Not Supported"
	'                        Case "2"
	'                            myTravelerDevice.policy_compliance = "Not Compliant"
	'                        Case "3"
	'                            myTravelerDevice.policy_compliance = "Compliant - Limited"
	'                        Case "4"
	'                            myTravelerDevice.policy_compliance = "Compliant"

	'                        Case "5"
	'                            myTravelerDevice.policy_compliance = "Unavailable"
	'                        Case "6"
	'                            myTravelerDevice.policy_compliance = "Pending"
	'                    End Select

	'                    ' WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Policy State: " & myTravelerDevice.Access)
	'                    Select Case myTravelerDevice.Access.ToString
	'                        Case "0"
	'                            myTravelerDevice.Access = "Allow"
	'                        Case "1"
	'                            myTravelerDevice.Access = "Deny"
	'                        Case "2"
	'                            myTravelerDevice.Access = "Prohibit"
	'                    End Select

	'                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Approval Policy: " & myTravelerDevice.ApprovalPolicy)
	'                    Select Case myTravelerDevice.ApprovalPolicy.ToString
	'                        Case "0"
	'                            myTravelerDevice.ApprovalPolicy = "Status Not Set"
	'                        Case "1"
	'                            myTravelerDevice.ApprovalPolicy = "Device Registered - Pending Approval"
	'                        Case "4"
	'                            myTravelerDevice.ApprovalPolicy = "Automatically Approved"
	'                        Case "8"
	'                            myTravelerDevice.ApprovalPolicy = "Denied"
	'                        Case "16"
	'                            myTravelerDevice.ApprovalPolicy = "Not Required"
	'                    End Select



	'                    'WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Device Provider: " & myTravelerDevice.DeviceName)
	'                    'WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Device Level: " & myTravelerDevice.Client_Build)
	'                    'WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Device Type: Before conversion: " & myTravelerDevice.OS_Type)

	'                    'VSPLUS-275 Traveler Device Graph improvement
	'                    myTravelerDevice.OS_Type = cleanDeviceType(myTravelerDevice.OS_Type)

	'                    'WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Device Type: After conversion: " & myTravelerDevice.OS_Type)
	'                    'WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Device Last Sync Time: " & myTravelerDevice.LastSyncTime)
	'                    'WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Device ID: " & myTravelerDevice.DeviceID)
	'                    'WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Device HREF: " & myTravelerDevice.href)
	'                    'WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Device Notification Type: " & myTravelerDevice.AutoSyncType)
	'                    Select Case myTravelerDevice.AutoSyncType.ToString
	'                        Case "0"
	'                            myTravelerDevice.AutoSyncType = "None"
	'                        Case "8"
	'                            myTravelerDevice.AutoSyncType = "ActiveSync"
	'                        Case "4"
	'                            myTravelerDevice.AutoSyncType = "HTTP"
	'                        Case "1"
	'                            myTravelerDevice.AutoSyncType = "SMS"
	'                        Case "2"
	'                            myTravelerDevice.AutoSyncType = "TCP"

	'                    End Select

	'                    If InStr(myTravelerDevice.Client_Build, "8.5.2") Then
	'                        myTravelerDevice.AutoSyncType = "HTTP"
	'                    End If

	'                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Approval: " & myTravelerDevice.Approval)
	'                    Select Case myTravelerDevice.Approval.ToString
	'                        Case "16"
	'                            myTravelerDevice.Approval = "Not Required"
	'                        Case "1"
	'                            myTravelerDevice.Approval = "Pending"
	'                        Case "8"
	'                            myTravelerDevice.Approval = "Denied"
	'                        Case Else
	'                            myTravelerDevice.Approval = "Approved"


	'                    End Select
	'                Catch ex As Exception
	'                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Exception before inserting/updating traveler device : " & ex.ToString)
	'                End Try

	'                Try
	'                    UpdateTravelerDeviceStatusTable(myTravelerDevice, myDominoServer.Name)
	'                Catch ex As Exception
	'                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Inserting data : " & ex.ToString)
	'                End Try

	'            Next
	'        End If
	'    End If


	'End Sub
    'Private Sub CheckTravelerUsersUsingJSONOld(ByRef myDominoServer As MonitoredItems.DominoServer)
    '    ' Traveler Users
    '    Dim myMasterDevices As New devices
    '    Dim totalDeviceCount As Integer = 0
    '    Dim myDevices As New devices
    '    Dim isFirstFetch As Boolean = True
    '    Dim htmlStream As String = ""
    '    Dim nextPage As String = ""
    '    Dim objVSAdaptor As New VSAdaptor
    '    ' we can loop thru the myDevices now. first go, isFirstFetch=true
    '    While (myDevices IsNot Nothing) AndAlso (nextPage <> "" Or isFirstFetch)
    '        Try

    '            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Top of while loop.", LogLevel.Verbose)
    '            htmlStream = getHTMLStream(myDevices.next1, myDominoServer)
    '            If htmlStream <> "" Then
    '                WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " I got back " & vbCrLf & htmlStream, LogLevel.Verbose)
    '                myDevices = returnDevicesObject(htmlStream)
    '            End If

    '            If myDevices IsNot Nothing Then
    '                'fill the master object with header information
    '                If isFirstFetch Then
    '                    myMasterDevices.code = myDevices.code
    '                    myMasterDevices.message = myDevices.message
    '                    'don't think we need these three
    '                    myMasterDevices.href = myDevices.href
    '                    myMasterDevices.next1 = myDevices.next1
    '                    myMasterDevices.totalRecords = myDevices.totalRecords
    '                End If
    '                isFirstFetch = False
    '                'set the less info object
    '                Dim myDeviceCount As Integer = 0
    '                Dim myConnection As SqlClient.SqlConnection = objVSAdaptor.StartConnectionSQL("VitalSigns")
    '                If myConnection IsNot Nothing Then
    '                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Opened the connection to process Mobile Devices. Connection Open Successful.", LogLevel.Verbose)
    '                Else
    '                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Tried to Open the connection to process Mobile Devices. Connection Open Not Successful.", LogLevel.Verbose)
    '                End If
    '                Try


    '                    For Each device As TravelerDevice In myDevices.data
    '                        Try
    '                            myDeviceCount += 1
    '                            If device.LastSyncTime <> "" Then
    '                                device.LastSyncTime = convertEPOCHTime(device.LastSyncTime)
    '                            End If
    '                            If device.href IsNot Nothing AndAlso device.href <> "" Then
    '                                device.href = IIf(myDominoServer.RequireSSL, "https://", "http://").ToString() + IIf(myDominoServer.ExternalAlias = "", myDominoServer.IPAddress, myDominoServer.ExternalAlias) & device.href.Replace("\/", "/")
    '                                WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, " device href for more details: " + device.href, LogLevel.Verbose)
    '                            End If
    '                            'myMasterDevices.data.Add(device)
    '                            Try
    '                                ' Clean the Dev
    '                                cleanDeviceType(device)
    '                                WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Aos_type: " & device.OS_Type, LogLevel.Verbose)

    '                                Select Case device.AutoSyncType.ToString
    '                                    Case "0"
    '                                        device.AutoSyncType = "None"
    '                                    Case "8"
    '                                        device.AutoSyncType = "ActiveSync"
    '                                    Case "4"
    '                                        device.AutoSyncType = "HTTP"
    '                                    Case "1"
    '                                        device.AutoSyncType = "SMS"
    '                                    Case "2"
    '                                        device.AutoSyncType = "TCP"

    '                                End Select

    '                                If InStr(device.Client_Build, "8.5.2") Then
    '                                    device.AutoSyncType = "HTTP"
    '                                End If

    '                            Catch ex As Exception
    '                                WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Exception before inserting/updating traveler device : " & ex.ToString)
    '                            End Try

    '                            Try
    '                                'UpdateTravelerDeviceStatusTable(myTravelerDevice, myDominoServer.Name, myDominoServer.TravelerHA_Pool_Name)
    '                                'Sowjany 1564 ticket
    '                                totalDeviceCount += 1
    '                                UpdateTravelerDeviceStatusTempTable(device, myDominoServer.Name, myDominoServer.TravelerHA_Pool_Name, myConnection)
    '                                WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " update done.", LogLevel.Verbose)
    '                            Catch ex As Exception
    '                                WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Inserting data : " & ex.ToString)
    '                            End Try



    '                        Catch ex As Exception
    '                            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Adding the device to the collection : " & ex.ToString)
    '                            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Adding the device to the collection : Error Object" & htmlStream)
    '                            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Adding the device to the collection : Device index in question is" & myDeviceCount.ToString())
    '                        End Try
    '                    Next ' For each Page End of loop. 
    '                Catch ex As Exception
    '                Finally
    '                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "Finally Closing the connection after adding " + totalDeviceCount.ToString() + " Mobile Devices", LogLevel.Verbose)
    '                    objVSAdaptor.StopConnectionSQL(myConnection)
    '                End Try
    '                'End If
    '            End If
    '            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Successfully added a page of devices. Move to next set now ", LogLevel.Verbose)
    '            Try
    '                nextPage = myDevices.next1.ToString
    '            Catch ex As Exception
    '                nextPage = ""
    '                WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "  Successfully added a page of devices. Move to next set now. Error looking for next set " & ex.ToString)

    '            End Try
    '            '  WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "  Successfully added a page of devices. Move to next set now. Error looking for next set: Next page url: " & nextPage)

    '            If nextPage = "" Then
    '                WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "  Successfully added a page of devices. Move to next set now. Error looking for next set: Next page url: EXIT WHILE ", LogLevel.Verbose)
    '                Exit While
    '            End If
    '        Catch ex As Exception
    '            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Parsing this stream of data : " & ex.ToString)
    '            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Adding the device to the collection : html stream in question" & htmlStream)
    '        End Try
    '    End While ' End of All Next Pages While loop.. 


    '    'Sowjanya 1564 ticket
    '    'For delete from main table, insert into main from temp and delete from temp   
    '    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Total Device Count " + totalDeviceCount.ToString(), LogLevel.Verbose)
    '    If totalDeviceCount > 0 Then CleanupTravelerDeviceStatusTable(myDominoServer.Name)

    '    'Try
    '    '	WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & ": setting the devices as active.", LogLevel.Verbose)
    '    '	SetActiveDevices()
    '    '	'Dim strUpdSQL As String = "update dbo.traveler_devices set IsActive=0"
    '    '	'objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strUpdSQL)

    '    '	'strUpdSQL = "update dbo.traveler_devices set IsActive=1 where ([Traveler_Devices].deviceid + '-' + convert(varchar,id) +'-' +convert(varchar,LastSyncTime,121)) in (select deviceid + '-' +convert(varchar,min(id))+'-'+convert(varchar,max(LastSyncTime),121) from Traveler_Devices group by DeviceID)"

    '    '	'WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Executing query: " & strUpdSQL, LogLevel.Verbose)
    '    '	'objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strUpdSQL)

    '    'Catch ex As Exception
    '    '	WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error setting the devices as active : " & ex.ToString)

    '    'End Try
    '    'we can iterate thru the myMasterDevices list and pump the values into TravelerDevice
    '    ' WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Attempting to populate Traveler User info...", LogLevel.Verbose)
    'End Sub

    Private Sub CheckTravelerUsersUsingJSON(ByRef myDominoServer As MonitoredItems.DominoServer)
        ' Traveler Users
        Dim myMasterDevices As New devices
        Dim totalDeviceCount As Integer = 0
        Dim myDevices As New devices
        Dim isFirstFetch As Boolean = True
        Dim htmlStream As String = ""
        Dim nextPage As String = ""
        Dim objVSAdaptor As New VSAdaptor
        Dim start As Long
        Dim CleaningLong As Long = 0
        Dim dtLong As Long = 0
        Dim temp As Long
        Dim maxTableSize As Integer = 0
        Dim overallTime As Long = Now.Ticks
        Dim dt As New DataTable()

        dt.Columns.Add("UserName")
        dt.Columns.Add("ClientBuild")
        dt.Columns.Add("ServerName")
        dt.Columns.Add("DeviceName")
        dt.Columns.Add("LastSyncTime", GetType(DateTime))
        dt.Columns.Add("OS_Type")
        dt.Columns.Add("OS_Type_Min")
        dt.Columns.Add("SyncType")
        dt.Columns.Add("href")
        dt.Columns.Add("LastUpdated", GetType(DateTime))
        dt.Columns.Add("HAPoolName")
        dt.Columns.Add("DeviceID")

        dt.PrimaryKey = New DataColumn() {dt.Columns("DeviceID"), dt.Columns("ServerName")}

        Dim myConnection As New SqlClient.SqlConnection()

        Try

            myConnection = objVSAdaptor.StartConnectionSQL("VitalSigns")

            Try
                Dim myRegistry As New RegistryHandler()
                maxTableSize = CType(myRegistry.ReadFromRegistry("Traveler Devices DateTable Size").ToString(), Integer)
            Catch ex As Exception
                maxTableSize = 0
            End Try

            ' we can loop thru the myDevices now. first go, isFirstFetch=true
            While (myDevices IsNot Nothing) AndAlso (nextPage <> "" Or isFirstFetch)
                Try

                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Top of while loop.", LogLevel.Verbose)
                    htmlStream = getHTMLStream(myDevices.next1, myDominoServer)
                    If htmlStream <> "" Then
                        '  WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " I got back " & vbCrLf & htmlStream)
                        myDevices = returnDevicesObject(htmlStream)
                    End If

                    If myDevices IsNot Nothing Then
                        'fill the master object with header information
                        If isFirstFetch Then
                            myMasterDevices.code = myDevices.code
                            myMasterDevices.message = myDevices.message
                            'don't think we need these three
                            myMasterDevices.href = myDevices.href
                            myMasterDevices.next1 = myDevices.next1
                            myMasterDevices.totalRecords = myDevices.totalRecords
                        End If
                        isFirstFetch = False
                        'set the less info object
                        Dim myDeviceCount As Integer = 0
                        If myConnection IsNot Nothing Then
                            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Opened the connection to process Mobile Devices. Connection Open Successful.", LogLevel.Verbose)
                        Else
                            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Tried to Open the connection to process Mobile Devices. Connection Open Not Successful.", LogLevel.Verbose)
                        End If
                        Try


                            For Each device As TravelerDevice In myDevices.data
                                Try
                                    start = Now.Ticks
                                    myDeviceCount += 1
                                    If device.LastSyncTime <> "" Then
                                        device.LastSyncTime = convertEPOCHTime(device.LastSyncTime)
                                    End If
                                    If device.href IsNot Nothing AndAlso device.href <> "" Then
                                        device.href = IIf(myDominoServer.RequireSSL, "https://", "http://").ToString() + IIf(myDominoServer.ExternalAlias = "", myDominoServer.IPAddress, myDominoServer.ExternalAlias) & device.href.Replace("\/", "/")
                                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, " device href for more details: " + device.href, LogLevel.Verbose)
                                    End If
                                    'myMasterDevices.data.Add(device)
                                    Try
                                        ' Clean the Dev
                                        cleanDeviceType(device)

                                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Aos_type: " & device.OS_Type, LogLevel.Verbose)

                                        'move this bit to sql
                                        Select Case device.AutoSyncType.ToString
                                            Case "0"
                                                device.AutoSyncType = "None"
                                            Case "8"
                                                device.AutoSyncType = "ActiveSync"
                                            Case "4"
                                                device.AutoSyncType = "HTTP"
                                            Case "1"
                                                device.AutoSyncType = "SMS"
                                            Case "2"
                                                device.AutoSyncType = "TCP"

                                        End Select

                                        If InStr(device.Client_Build, "8.5.2") Then
                                            device.AutoSyncType = "HTTP"
                                        End If

                                    Catch ex As Exception
                                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Exception before inserting/updating traveler device : " & ex.ToString)
                                    End Try

                                    Try
                                        'UpdateTravelerDeviceStatusTable(myTravelerDevice, myDominoServer.Name, myDominoServer.TravelerHA_Pool_Name)
                                        'Sowjany 1564 ticket
                                        temp = New TimeSpan(Now.Ticks - start).Milliseconds.ToString()
                                        CleaningLong += temp
                                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Time for Cleaning Device " & temp, LogUtilities.LogUtils.LogLevel.Verbose)
                                        start = Now.Ticks
                                        totalDeviceCount += 1
                                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Number " & totalDeviceCount)
                                        'adds the device to the datatable
                                        'UpdateTravelerDeviceStatusDataTable(device, myDominoServer.Name, myDominoServer.TravelerHA_Pool_Name, dt)
                                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " update done.", LogLevel.Verbose)
                                    Catch ex As Exception
                                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Inserting data : " & ex.ToString, LogUtilities.LogUtils.LogLevel.Verbose)
                                    End Try

                                    temp = New TimeSpan(Now.Ticks - start).Milliseconds.ToString()
                                    dtLong += temp
                                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Time for Adding XML Device " & temp, LogUtilities.LogUtils.LogLevel.Verbose)

                                Catch ex As Exception
                                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Adding the device to the collection : " & ex.ToString)
                                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Adding the device to the collection : Error Object" & htmlStream)
                                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Adding the device to the collection : Device index in question is" & myDeviceCount.ToString())
                                End Try

                                'adds the dt to the db if the setting is not 0
                                If Not (maxTableSize = 0) And dt.Rows.Count > maxTableSize Then
                                    AddDevicesToTempTable(myDominoServer.Name, dt, myConnection)
                                    dt.Clear()
                                End If

                            Next ' For each Page End of loop. 
                        Catch ex As Exception
                        Finally
                            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "Finally Closing the connection after adding " + totalDeviceCount.ToString() + " Mobile Devices", LogLevel.Verbose)

                        End Try
                        'End If
                    End If
                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Successfully added a page of devices. Move to next set now ", LogLevel.Verbose)
                    Try
                        nextPage = myDevices.next1.ToString
                    Catch ex As Exception
                        nextPage = ""
                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "  Successfully added a page of devices. Move to next set now. Error looking for next set " & ex.ToString)

                    End Try
                    '  WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "  Successfully added a page of devices. Move to next set now. Error looking for next set: Next page url: " & nextPage)

                    If nextPage = "" Then
                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "  Successfully added a page of devices. Move to next set now. Error looking for next set: Next page url: EXIT WHILE ", LogLevel.Verbose)
                        Exit While
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Parsing this stream of data : " & ex.ToString)
                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Adding the device to the collection : html stream in question" & htmlStream)
                End Try
            End While ' End of All Next Pages While loop.. 

            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Datatable time: " & New TimeSpan(dtLong).Milliseconds.ToString(), LogUtilities.LogUtils.LogLevel.Verbose)
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " conversion time: " & New TimeSpan(CleaningLong).Milliseconds.ToString(), LogUtilities.LogUtils.LogLevel.Verbose)
            'Sowjanya 1564 ticket
            'For delete from main table, insert into main from temp and delete from temp   
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Total Device Count " + totalDeviceCount.ToString(), LogLevel.Verbose)
            'adds the devices from the dt to the db
            AddDevicesToTempTable(myDominoServer.Name, dt, myConnection)

            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Done adding to temp table", LogUtilities.LogUtils.LogLevel.Verbose)
            'adds the entries from thetemp table to the main table
            If totalDeviceCount > 0 Then CleanupTravelerDeviceStatusTable(myDominoServer.Name, myConnection)
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Done moving all values to real table", LogUtilities.LogUtils.LogLevel.Verbose)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error in the device function CheckTravelerUsingJSON.  Error: " & ex.Message)
        Finally
            objVSAdaptor.StopConnectionSQL(myConnection)
        End Try

        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Overall Time for whole scan: " & (New TimeSpan(Now.Ticks - overallTime).TotalSeconds.ToString()), LogUtilities.LogUtils.LogLevel.Verbose)


    End Sub

    Private Sub CheckTravelerUsersUsingJSONMongo(ByRef myDominoServer As MonitoredItems.DominoServer)
        ' Traveler Users
        Dim myMasterDevices As New devices
        Dim totalDeviceCount As Integer = 0
        Dim myDevices As New devices
        Dim isFirstFetch As Boolean = True
        Dim htmlStream As String = ""
        Dim nextPage As String = ""
        Dim objVSAdaptor As New VSAdaptor
        Dim start As Long
        Dim CleaningLong As Long = 0
        Dim dtLong As Long = 0
        Dim temp As Long
        Dim maxTableSize As Integer = 0
        Dim overallTime As Long = Now.Ticks
        Dim dt As New DataTable()
        Dim StartOfScan As DateTime = Now

        dt.Columns.Add("UserName")
        dt.Columns.Add("ClientBuild")
        dt.Columns.Add("ServerName")
        dt.Columns.Add("DeviceName")
        dt.Columns.Add("LastSyncTime", GetType(DateTime))
        dt.Columns.Add("OS_Type")
        dt.Columns.Add("OS_Type_Min")
        dt.Columns.Add("SyncType")
        dt.Columns.Add("href")
        dt.Columns.Add("LastUpdated", GetType(DateTime))
        dt.Columns.Add("HAPoolName")
        dt.Columns.Add("DeviceID")

        dt.PrimaryKey = New DataColumn() {dt.Columns("DeviceID"), dt.Columns("ServerName")}

        Dim myConnection As New SqlClient.SqlConnection()

        Try


            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.MobileDevices)(connectionString)
            Dim listOfMongoDevices As New List(Of VSNext.Mongo.Entities.MobileDevices)

            Try
                'Dim myRegistry As New RegistryHandler()
                'maxTableSize = CType(myRegistry.ReadFromRegistry("Traveler Devices DateTable Size").ToString(), Integer)
                maxTableSize = 0
            Catch ex As Exception
                maxTableSize = 0
            End Try

            Try
                Dim serverName As String = myDominoServer.Name
                listOfMongoDevices = repository.Find(Function(x) x.ServerName = serverName)
            Catch ex As Exception

            End Try

            While (myDevices IsNot Nothing) AndAlso (nextPage <> "" Or isFirstFetch)
                Try

                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Top of while loop.", LogLevel.Verbose)
                    htmlStream = getHTMLStream(myDevices.next1, myDominoServer)
                    If htmlStream <> "" Then
                        '  WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " I got back " & vbCrLf & htmlStream)
                        myDevices = returnDevicesObject(htmlStream)
                    End If

                    If myDevices IsNot Nothing Then
                        'fill the master object with header information
                        If isFirstFetch Then
                            myMasterDevices.code = myDevices.code
                            myMasterDevices.message = myDevices.message
                            'don't think we need these three
                            myMasterDevices.href = myDevices.href
                            myMasterDevices.next1 = myDevices.next1
                            myMasterDevices.totalRecords = myDevices.totalRecords
                        End If
                        isFirstFetch = False
                        'set the less info object
                        Dim myDeviceCount As Integer = 0
                        If myConnection IsNot Nothing Then
                            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Opened the connection to process Mobile Devices. Connection Open Successful.", LogLevel.Verbose)
                        Else
                            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Tried to Open the connection to process Mobile Devices. Connection Open Not Successful.", LogLevel.Verbose)
                        End If
                        Try


                            For Each device As TravelerDevice In myDevices.data
                                Try
                                    start = Now.Ticks
                                    myDeviceCount += 1
                                    If device.LastSyncTime <> "" Then
                                        device.LastSyncTime = convertEPOCHTime(device.LastSyncTime)
                                    End If
                                    If device.href IsNot Nothing AndAlso device.href <> "" Then
                                        device.href = IIf(myDominoServer.RequireSSL, "https://", "http://").ToString() + IIf(myDominoServer.ExternalAlias = "", myDominoServer.IPAddress, myDominoServer.ExternalAlias) & device.href.Replace("\/", "/")
                                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, " device href for more details: " + device.href, LogLevel.Verbose)
                                    End If
                                    'myMasterDevices.data.Add(device)
                                    Try
                                        ' Clean the Dev
                                        cleanDeviceType(device)

                                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Aos_type: " & device.OS_Type, LogLevel.Verbose)

                                        'move this bit to sql
                                        Select Case device.AutoSyncType.ToString
                                            Case "0"
                                                device.AutoSyncType = "None"
                                            Case "8"
                                                device.AutoSyncType = "ActiveSync"
                                            Case "4"
                                                device.AutoSyncType = "HTTP"
                                            Case "1"
                                                device.AutoSyncType = "SMS"
                                            Case "2"
                                                device.AutoSyncType = "TCP"

                                        End Select

                                        If InStr(device.Client_Build, "8.5.2") Then
                                            device.AutoSyncType = "HTTP"
                                        End If

                                    Catch ex As Exception
                                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Exception before inserting/updating traveler device : " & ex.ToString)
                                    End Try

                                    Try
                                        'UpdateTravelerDeviceStatusTable(myTravelerDevice, myDominoServer.Name, myDominoServer.TravelerHA_Pool_Name)
                                        'Sowjany 1564 ticket
                                        temp = New TimeSpan(Now.Ticks - start).Milliseconds.ToString()
                                        CleaningLong += temp
                                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Time for Cleaning Device " & temp, LogUtilities.LogUtils.LogLevel.Verbose)
                                        start = Now.Ticks
                                        totalDeviceCount += 1
                                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Number " & totalDeviceCount)
                                        'adds the device to the Mongo Collection
                                        UpdateTravelerDeviceStatusMongoCollection(device, myDominoServer.Name, myDominoServer.TravelerHA_Pool_Name, listOfMongoDevices)
                                        'UpdateTravelerDeviceStatusDataTable(device, myDominoServer.Name, myDominoServer.TravelerHA_Pool_Name, dt)
                                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " update done.", LogLevel.Verbose)
                                    Catch ex As Exception
                                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Inserting data : " & ex.ToString, LogUtilities.LogUtils.LogLevel.Verbose)
                                    End Try

                                    temp = New TimeSpan(Now.Ticks - start).Milliseconds.ToString()
                                    dtLong += temp
                                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Time for Adding XML Device " & temp, LogUtilities.LogUtils.LogLevel.Verbose)

                                Catch ex As Exception
                                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Adding the device to the collection : " & ex.ToString)
                                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Adding the device to the collection : Error Object" & htmlStream)
                                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Adding the device to the collection : Device index in question is" & myDeviceCount.ToString())
                                End Try

                                'adds the dt to the db if the setting is not 0
                                'If Not (maxTableSize = 0) And dt.Rows.Count > maxTableSize Then
                                '    AddDevicesToTempTable(myDominoServer.Name, dt, myConnection)
                                '    dt.Clear()
                                'End If

                            Next ' For each Page End of loop. 
                        Catch ex As Exception
                        Finally
                            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "Finally Closing the connection after adding " + totalDeviceCount.ToString() + " Mobile Devices", LogLevel.Verbose)

                        End Try
                        'End If
                    End If
                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Successfully added a page of devices. Move to next set now ", LogLevel.Verbose)
                    Try
                        nextPage = myDevices.next1.ToString
                    Catch ex As Exception
                        nextPage = ""
                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "  Successfully added a page of devices. Move to next set now. Error looking for next set " & ex.ToString)

                    End Try
                    '  WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "  Successfully added a page of devices. Move to next set now. Error looking for next set: Next page url: " & nextPage)

                    If nextPage = "" Then
                        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & "  Successfully added a page of devices. Move to next set now. Error looking for next set: Next page url: EXIT WHILE ", LogLevel.Verbose)
                        Exit While
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Parsing this stream of data : " & ex.ToString)
                    WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error Adding the device to the collection : html stream in question" & htmlStream)
                End Try
            End While ' End of All Next Pages While loop.. 

            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Datatable time: " & New TimeSpan(dtLong).Milliseconds.ToString(), LogUtilities.LogUtils.LogLevel.Verbose)
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " conversion time: " & New TimeSpan(CleaningLong).Milliseconds.ToString(), LogUtilities.LogUtils.LogLevel.Verbose)
            'Sowjanya 1564 ticket
            'For delete from main table, insert into main from temp and delete from temp   
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Total Device Count " + totalDeviceCount.ToString(), LogLevel.Verbose)
            'adds the devices from the dt to the db
            'AddDevicesToTempTable(myDominoServer.Name, dt, myConnection)
            AddDevicesToCollection(myDominoServer.Name, listOfMongoDevices, repository)

            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Done adding to temp table", LogUtilities.LogUtils.LogLevel.Verbose)
            'adds the entries from thetemp table to the main table
            If totalDeviceCount > 0 Then CleanupTravelerDeviceStatusCollection(myDominoServer.Name, repository, StartOfScan)
            'CleanupTravelerDeviceStatusTable(myDominoServer.Name, myConnection)
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Done moving all values to real table", LogUtilities.LogUtils.LogLevel.Verbose)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Error in the device function CheckTravelerUsingJSON.  Error: " & ex.Message)
        Finally

        End Try

        WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Overall Time for whole scan: " & (New TimeSpan(Now.Ticks - overallTime).TotalSeconds.ToString()), LogUtilities.LogUtils.LogLevel.Verbose)

    End Sub


    Private Sub cleanDeviceType(ByRef ThisDevice As TravelerDevice)

        If DeviceIDTranslations.ContainsKey(ThisDevice.DeviceID) Then
            WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Found device in dictionary.  Will use those values", LogLevel.Verbose)
            Dim obj As DeviceIDTranslationsObj = DeviceIDTranslations(ThisDevice.DeviceID)
            ThisDevice.DeviceName = obj.DeviceName
            ThisDevice.OS_Type = obj.DeviceOSType
            ThisDevice.OS_Type_Min = obj.DeviceOSTypeMin
            Exit Sub
        End If

        Dim DeviceType As String = ""
        Dim OsType As String = ""
        Dim aDeviceType As String()
        WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Raw OS Type : " & ThisDevice.OS_Type, LogLevel.Verbose)
        'set the raw value before this gets translated below
        ThisDevice.DeviceNameRaw = ThisDevice.DeviceName
        If ThisDevice.OS_Type.Contains("/") Then

            aDeviceType = ThisDevice.OS_Type.Split("/")
            DeviceType = aDeviceType(0).ToString
            If aDeviceType.Length > 0 Then
                OsType = aDeviceType(1).ToString
            End If
            WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Raw OS Type : " & OsType, LogLevel.Verbose)
            WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Raw Device Type : " & DeviceType, LogLevel.Verbose)
            If ThisDevice.OS_Type.ToLower.Contains("apple") Then
                'split the device_type with "/" and the first part will be decoded  and put into DeviceName , the second part will be decoded and put into Os_Type
                ThisDevice.DeviceName = TranslateAppleDeviceType(DeviceType)
                If OsType <> "" Then
                    ThisDevice.OS_Type = TranslateAppleOSType(OsType)
                End If
            ElseIf ThisDevice.OS_Type.ToLower.Contains("android") Then
                ThisDevice.DeviceName = TranslateAndroidDeviceType(DeviceType)
                If OsType <> "" Then
                    ThisDevice.OS_Type = TranslateAndroidOSType(ThisDevice.OS_Type)
                End If
            Else
                'we'll still split but the first part goes into OS_Type
                'for android device types, we do not have "/" and it's usually the OS name like "Android 4.2.2". But just in case if it does..
                'do not touch the device name in case of other type of devices
                If OsType.Trim() = "" Then
                    ThisDevice.OS_Type = DeviceType
                Else
                    ThisDevice.OS_Type = OsType
                End If
            End If
        ElseIf ThisDevice.OS_Type.ToLower.Contains("android") Then
            ' DeviceName contains the actual device name of the device like SAMSUNG SGH-M919
            Dim translatedDeviceName As String = TranslateAndroidDeviceType(ThisDevice.DeviceName)

            If ThisDevice.DeviceName = translatedDeviceName Then
                translatedDeviceName = "UNKNOWN"
                ThisDevice.DeviceName = translatedDeviceName
            Else
                ThisDevice.DeviceName = translatedDeviceName
            End If
            'ThisDevice.DeviceName = TranslateAndroidDeviceType(ThisDevice.DeviceName)
            ThisDevice.OS_Type = TranslateAndroidOSType(ThisDevice.OS_Type)
        ElseIf ThisDevice.OS_Type = "8.0" Or ThisDevice.OS_Type = "8.1" Then
            ThisDevice.DeviceName = "Windows Phone"
            ThisDevice.OS_Type = "Windows"

        ElseIf ThisDevice.DeviceName.ToLower.Contains("rim") Or ThisDevice.DeviceName.ToLower.Contains("blackberry") Or ThisDevice.DeviceName.ToLower.Contains("black berry") Or ThisDevice.DeviceName.ToLower.Contains("z10") _
         Or ThisDevice.DeviceName.ToLower.Contains("z30") Or ThisDevice.DeviceName.ToLower.Contains("q10") Or ThisDevice.DeviceName.ToLower.Contains("q5") Then
            If ThisDevice.DeviceName.ToLower.Contains("z10") Then
                ThisDevice.DeviceName = "BlackBerry Z10"
            ElseIf ThisDevice.DeviceName.ToLower.Contains("q10") Then
                ThisDevice.DeviceName = "BlackBerry Q10"
            ElseIf ThisDevice.DeviceName.ToLower.Contains("z30") Then
                ThisDevice.DeviceName = "BlackBerry Z30"
            ElseIf ThisDevice.DeviceName.ToLower.Contains("q5") Then
                ThisDevice.DeviceName = "BlackBerry Q5"
            End If
            ThisDevice.OS_Type = "BlackBerry " + ThisDevice.OS_Type.ToString
        End If

        'OS_Type dec precision to 2 : 7.1.2 should be 7.1
        ThisDevice.OS_Type_Min = TrimOsType(ThisDevice.OS_Type)
        DeviceIDTranslations.TryAdd(ThisDevice.DeviceID, New DeviceIDTranslationsObj(ThisDevice.DeviceName, ThisDevice.OS_Type, ThisDevice.OS_Type_Min))

        WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Translated OS Type : " & ThisDevice.OS_Type, LogLevel.Verbose)
        WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Translated Device Type : " & ThisDevice.DeviceName, LogLevel.Verbose)
    End Sub

    Private Function TrimOsType(ByVal s As String) As String
        If s = "" Then
            Return s
        Else
            Dim aOsType() As String
            aOsType = s.Split(".")
            If aOsType.Length > 1 Then
                WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Translated OS Type : " & s.ToString, LogLevel.Verbose)
                WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Translated OS Type Length : " & s.Split(".").Length.ToString, LogLevel.Verbose)
                Dim osType As String = ""
                For i As Integer = 0 To 1
                    If i = 0 Then
                        osType = aOsType(0).ToString + "."
                    Else
                        osType += aOsType(1).ToString
                    End If
                Next
                Return osType
            Else
                Return s
            End If
        End If
    End Function
    Private Function TranslateAppleDeviceType(ByVal RawValue As String) As String
        If DeviceTypeTranslationApple.ContainsKey(RawValue) Then
            Return DeviceTypeTranslationApple.Item(RawValue)
        End If

        Return RawValue
    End Function

    Private Function TranslateAndroidDeviceType(ByVal RawValue As String) As String

        Dim dict As New Dictionary(Of String, Integer)

        'loops through every entry and checks to see if any key exists anywhere inside the raw string and if 
        'it finds one, it adds it to a dictionary of the key and the length of the key
        'It creates a collection isntead of returning that value to handle cases when there are two keys, one like 1029 and one like 1029A
        For Each name As String In DeviceTypeTranslationAndroid.Keys
            If RawValue.ToLower.Contains(name.ToLower) Then
                dict.Add(name, Len(name))
            End If
        Next

        'loops through each new entry in the dictionary and searches for the longest one to retreive the 1029A instead of 1029 (from the above example)
        Dim longest As Integer = 0
        Dim longestKey As String = ""
        For Each key As String In dict.Keys
            If dict(key) > longest Then
                longestKey = DeviceTypeTranslationAndroid(key)
                longest = dict(key)
            End If
        Next

        'returns the original value if not found, or the translated value if it is found
        If longestKey = "" Then
            Return RawValue
        End If
        Return longestKey

    End Function
    Private Function TranslateAppleOSType(ByVal RawValue As String) As String
        'strip out (OS 7), (OS 6).. etc
        If RawValue.IndexOf("(") > 0 Then
            RawValue = Left(RawValue, RawValue.IndexOf("(") - 1)
        End If
        If OSTypeTranslationApple.ContainsKey(RawValue) Then
            Return OSTypeTranslationApple.Item(RawValue)
        End If
        Return RawValue
    End Function
    Private Function TranslateAndroidOSType(ByVal RawValue As String) As String

        'checks each entry to see if a android name exists in any of them
        For Each name As String In OSTypeTranslationAndroid.Keys
            If RawValue.ToLower.Contains(name.ToLower) Then

                'if it finds a entry, it pulls the corrisponding value and checks to see if it has a version range ( indicated by a '(' ) or is a single version
                Dim valueOfKey As String = OSTypeTranslationAndroid.Item(name)
                If valueOfKey.IndexOf("(") > 0 Then
                    'removes white space to remove chance of spaces while entrering data
                    valueOfKey = valueOfKey.Replace(" ", "")
                    'if it has a range, it extracts the lower and upper boundries to the version numbers
                    Dim versionsRange As String = valueOfKey.Substring(valueOfKey.IndexOf("(") + 1, valueOfKey.IndexOf(")") - valueOfKey.IndexOf("(") - 1)
                    Dim lowerRange As Double = Double.Parse(versionsRange.Substring(0, 3))
                    Dim upperRange As Double = Double.Parse(versionsRange.Substring(4, 3))

                    'Loops through each group of 3 characters and checks if it can be parsed to a double and stores the value in version.  
                    'Then compares that value to see if it falls between the lower and upper range
                    Dim version As Double
                    For i As Integer = 0 To Len(RawValue) - 3
                        If Double.TryParse(RawValue.Substring(i, 3), version) Then
                            If version >= lowerRange And version <= upperRange Then
                                Return "Android " & version
                            End If
                        End If
                    Next
                    WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Error:  Android Version could not be located in the string '" & RawValue & "' for the key of " &
                       name & " and value of " & valueOfKey & ".  In method VitalSignsPlusDomino.TranslateAndroidOSType", LogLevel.Verbose)
                    Return valueOfKey
                End If
                Return valueOfKey
            End If
        Next
        Return RawValue
    End Function

    Private Function EnglishColors(ByVal deviceType As String) As String
        Dim strEnglish As String = deviceType
        Try
            strEnglish = deviceType.Replace("Zwart", "Black")
            strEnglish = deviceType.Replace("Wit", "White")
            strEnglish = deviceType.Replace("weiß", "White")
            strEnglish = deviceType.Replace("Vit", "White")
            strEnglish = deviceType.Replace("Svart", "White")
            strEnglish = deviceType.Replace("Sort", "Black")
            strEnglish = deviceType.Replace("Preto", "Black")
            strEnglish = deviceType.Replace("Noir", "Black")
            strEnglish = deviceType.Replace("Preto", "Black")
            strEnglish = deviceType.Replace("Unknown Google_SDK", "Android Emulator")
            strEnglish = deviceType.Replace("Nero", "Black")
            strEnglish = deviceType.Replace("Negro", "Black")
            strEnglish = deviceType.Replace("Musta", "Black")
            strEnglish = deviceType.Replace("Hvit", "White")
            strEnglish = deviceType.Replace("Hvid", "White")
            strEnglish = deviceType.Replace("Blanc", "White")
            strEnglish = deviceType.Replace("Blanco", "White")
            strEnglish = deviceType.Replace("Cierny", "Black")
            strEnglish = deviceType.Replace("Fehér", "White")
            strEnglish = deviceType.Replace("Branco", "White")
            strEnglish = deviceType.Replace("Bianco", "White")
            strEnglish = deviceType.Replace("Bilý", "Black")
            strEnglish = deviceType.Replace("Bianco", "Black")
            strEnglish = deviceType.Replace("?", "")
            Return strEnglish
        Catch ex As Exception
            Return deviceType
        End Try





    End Function

    Private Function convertEPOCHTime(ByVal epochTime As String) As String
        Dim syncTime As DateTime

        Try
            Dim Epoch As New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Device Last Sync Time in EPOCH: " & epochTime, LogLevel.Verbose)
            syncTime = Epoch.AddMilliseconds(CDbl(epochTime))
            WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Device Last Sync Time in human: " & syncTime.ToString, LogLevel.Verbose)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Traveler_Users", "Error parsing time " & ex.ToString)
            syncTime = Now
        End Try
        Return syncTime.ToLocalTime.ToString
    End Function

    Private Function returnDevicesObject(ByVal s As String) As devices
        Dim myDevices As New devices
        Dim serializer As New DataContractJsonSerializer(myDevices.[GetType]())

        Dim ms As New MemoryStream(Encoding.Unicode.GetBytes(s))
        Try
            myDevices = DirectCast(serializer.ReadObject(ms), devices)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Traveler_Users", "Error converting stream to object. String: " & s)
            WriteDeviceHistoryEntry("All", "Traveler_Users", "Error converting stream to object: " & ex.ToString)
        End Try

        ms.Close()
        ms.Dispose()
        Return myDevices
    End Function

    Private Function returnTravelerServersObject(ByVal s As String) As travelerServers
        Dim myServers As New travelerServers
        Dim serializer As New DataContractJsonSerializer(myServers.[GetType]())

        Dim ms As New MemoryStream(Encoding.Unicode.GetBytes(s))
        Try
            myServers = DirectCast(serializer.ReadObject(ms), travelerServers)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Traveler_Servers", "Error converting stream to object: " & ex.ToString)
        End Try

        ms.Close()
        ms.Dispose()
        Return myServers
    End Function

    Private Function returnDevicesMoreInfoObject(ByVal s As String) As devicesMoreInfo
        Dim myDevices As New devicesMoreInfo
        Dim serializer As New DataContractJsonSerializer(myDevices.[GetType]())

        Dim ms As New MemoryStream(Encoding.Unicode.GetBytes(s))
        Try
            myDevices = DirectCast(serializer.ReadObject(ms), devicesMoreInfo)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Traveler_Users", "Error converting stream to object: " & ex.ToString)
        End Try

        ms.Close()
        ms.Dispose()
        Return myDevices
    End Function

    Private Function getHTMLStream(ByVal URL As String, ByRef MyDominoServer As MonitoredItems.DominoServer) As String
        'This function is for Traveler 9+ servers, where the data is only available from a servlet

        WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " Checking the Traveler server's devices list via JSON availablity....", LogLevel.Verbose)

        Dim myServletURL As New MonitoredItems.URL


        WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " Checking if an external alias is defined :" + MyDominoServer.ExternalAlias)
        With myServletURL
            If URL = "" Then
                .URL = IIf(MyDominoServer.ExternalAlias = "", MyDominoServer.IPAddress, MyDominoServer.ExternalAlias) & "/api/traveler/devices?pm=" + pm.ToString()
            Else
                .URL = IIf(MyDominoServer.ExternalAlias = "", MyDominoServer.IPAddress, MyDominoServer.ExternalAlias) & URL.Replace("\/", "/")
            End If
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " I figure that the servlet location is " & myServletURL.URL)
        End With


        Dim ChilkatHTTP As New Chilkat.Http

        myServletURL.HTML = ""
        Try
            Dim success As Boolean
            success = ChilkatHTTP.UnlockComponent("MZLDADHttp_efwTynJYYR3X")
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error unlocking Chilkat component: " & ex.ToString)
            ' WriteDeviceHistoryEntry("URL", myServletURL.Name, Now.ToString & " Error unlocking component: " & ex.ToString)
        End Try
        Dim myRegistry As New RegistryHandler

        Try

            If myRegistry.ReadFromRegistry("ProxyEnabled") = "True" Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Attempting to route through proxy server.")
                ChilkatHTTP.SocksHostname = myRegistry.ReadFromRegistry("ProxyServer")
                ChilkatHTTP.SocksPort = myRegistry.ReadFromRegistry("ProxyPort")
                ChilkatHTTP.SocksUsername = myRegistry.ReadFromRegistry("ProxyUser")
                ChilkatHTTP.SocksPassword = myRegistry.ReadFromRegistry("ProxyPassword")
            End If
            myRegistry = Nothing
        Catch ex As Exception

        End Try
        'Get the passwords from the registry
        Dim MyPass As Byte()
        Dim mySecrets As New VSFramework.TripleDES

        Try
            'ChilkatHTTP.Login = myRegistry.ReadFromRegistry("Domino HTTP User")
            ChilkatHTTP.Login = MyDominoServer.HTTP_UserName
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino HTTP User is " & ChilkatHTTP.Login)
            ChilkatHTTP.Password = MyDominoServer.HTTP_Password
            WriteDeviceHistoryEntry("All", "Traveler_Users", "username: " & ChilkatHTTP.Login & "...Password:" & ChilkatHTTP.Password)
            'MyPass = myRegistry.ReadFromRegistry("Domino HTTP Password")  'sametime password as encrypted byte stream
            'If Not MyPass Is Nothing Then
            '	ChilkatHTTP.Password = mySecrets.Decrypt(MyPass) 'password in clear text, stored in memory now
            'Else
            '	ChilkatHTTP.Password = Nothing
            'End If
            'WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Domino HTTP password is " & ChilkatHTTP.Password)
        Catch ex As Exception
            MyPass = Nothing
        End Try



        Try
            Dim n As Integer
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " RequireSSL is set to :" + MyDominoServer.RequireSSL.ToString())
            Do While myServletURL.HTML = ""
                myServletURL.HTML = ChilkatHTTP.QuickGetStr(IIf(MyDominoServer.RequireSSL, "https://", "http://").ToString() + myServletURL.URL)
                WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " HTTP URL IS: " & IIf(MyDominoServer.RequireSSL, "https://", "http://").ToString() + myServletURL.URL)
                n += 1
                Thread.Sleep(500)
                If n > 15 Then Exit Do
            Loop
            ''check for <HTML> string, it might be an error of some sort
            'If myServletURL.HTML.ToLower.Contains("<html>") Then
            '	Do While myServletURL.HTML = ""
            '		myServletURL.HTML = ChilkatHTTP.QuickGetStr("https://" + myServletURL.URL)
            '		WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " HTTPS URL IS: " & myServletURL.URL)
            '		n += 1
            '		Thread.Sleep(500)
            '		If n > 15 Then Exit Do
            '	Loop
            'End If

            If (myServletURL.HTML = "") Then
                MyDominoServer.Description = "Notes Traveler servlet did not respond. "
            Else
                WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " Device Servlet response is: " & vbCrLf & myServletURL.HTML, LogLevel.Verbose)
            End If

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error performing HTTP.Get: " & ex.Message)
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error performing HTTP.Get: " & ex.Message)
        End Try

        Dim TravelerServlet As String = "OK"


        Return myServletURL.HTML
    End Function

    Private Function getHTMLStream(ByVal FullURL As String, UserName As String, Password As String) As String
        'This function is for Traveler 9+ servers, where the data is only available from a servlet

        WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Checking the Traveler server's devices list via JSON availablity....")
        Dim myServletURL As New MonitoredItems.URL
        WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Checking the Traveler server's devices list via JSON availablity....2")
        With myServletURL
            .URL = FullURL
            WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " I figure that the servlet location is " & myServletURL.URL)
        End With

        Dim ChilkatHTTP As New Chilkat.Http
        myServletURL.HTML = ""
        Try
            Dim success As Boolean
            success = ChilkatHTTP.UnlockComponent("MZLDADHttp_efwTynJYYR3X")
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", "Traveler_Users_MoreDetails", Now.ToString & " Error unlocking Chilkat component: " & ex.ToString)
            ' WriteDeviceHistoryEntry("URL", myServletURL.Name, Now.ToString & " Error unlocking component: " & ex.ToString)
        End Try
        Dim myRegistry As New RegistryHandler

        Try

            If myRegistry.ReadFromRegistry("ProxyEnabled") = "True" Then
                'WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Attempting to route through proxy server.")
                ChilkatHTTP.SocksHostname = myRegistry.ReadFromRegistry("ProxyServer")
                ChilkatHTTP.SocksPort = myRegistry.ReadFromRegistry("ProxyPort")
                ChilkatHTTP.SocksUsername = myRegistry.ReadFromRegistry("ProxyUser")
                ChilkatHTTP.SocksPassword = myRegistry.ReadFromRegistry("ProxyPassword")
            End If
            myRegistry = Nothing
        Catch ex As Exception

        End Try
        'Get the passwords from the registry
        Dim MyPass As Byte()
        Dim mySecrets As New VSFramework.TripleDES

        'Try
        '	ChilkatHTTP.Login = myRegistry.ReadFromRegistry("Domino HTTP User")
        '	'WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino HTTP User is " & ChilkatHTTP.Login)
        '	MyPass = myRegistry.ReadFromRegistry("Domino HTTP Password")  'sametime password as encrypted byte stream
        '	If Not MyPass Is Nothing Then
        '		ChilkatHTTP.Password = mySecrets.Decrypt(MyPass) 'password in clear text, stored in memory now
        '	Else
        '		ChilkatHTTP.Password = Nothing
        '	End If
        '	' WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Domino HTTP password is " & ChilkatHTTP.Password)
        'Catch ex As Exception
        '	MyPass = Nothing
        'End Try

        Try
            ChilkatHTTP.Login = UserName
            'WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino HTTP User is " & ChilkatHTTP.Login)
            ChilkatHTTP.Password = Password
        Catch ex As Exception

        End Try

        Try
            Dim n As Integer
            Do While myServletURL.HTML = ""
                myServletURL.HTML = ChilkatHTTP.QuickGetStr(myServletURL.URL)
                n += 1
                Thread.Sleep(500)
                If n > 15 Then Exit Do
            Loop

            'If (myServletURL.HTML = "") Then
            '    MyDominoServer.Description = "Notes Traveler servlet did not respond. "
            'Else
            '    WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " Device Servlet response is: " & vbCrLf & myServletURL.HTML)
            'End If

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error performing HTTP.Get: " & ex.Message)
            'WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error performing HTTP.Get: " & ex.Message)
        End Try

        Dim TravelerServlet As String = "OK"


        Return myServletURL.HTML
    End Function
    Private Sub TrackKeyDevices()
        Do Until boolTimeToStop
            Dim myConnectionString As New VSFramework.XMLOperation
            Dim myAdapter As New VSFramework.VSAdaptor
            WriteDeviceHistoryEntry("All", "Mobile_Device", Now.ToString & " Get Distinct Server Names first ", LogLevel.Verbose)
            Dim sSQLServers As String = "select TD.DeviceID,MoreDetailsURL,ServerName,SyncTimeThreshold,DeviceName,UserName   from MobileUserThreshold MUT, Traveler_Devices TD where TD.DeviceID =MUT.DeviceId and MoreDetailsURL IS NOT NULL AND IsActive=1"
            Dim dtServers As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sSQLServers)
            WriteDeviceHistoryEntry("All", "Mobile_Device", Now.ToString & " Distinct Server List Count: " & dtServers.Rows.Count & " Servers ")
            Dim MyDominoServer As MonitoredItems.DominoServer
            Dim href As String = ""
            Dim myMasterDevices As New devices
            Dim htmlStream As String = ""
            Dim Threshold_Minutes As Integer
            Dim deviceId As String = ""
            Dim objVSAdaptor As New VSAdaptor

            For Each drServers As DataRow In dtServers.Rows
                Try
                    MyDominoServer = MyDominoServers.Search(drServers("ServerName").ToString())
                    WriteDeviceHistoryEntry("All", "Mobile_Device", Now.ToString & " Searched for the server name: and the server is  " & MyDominoServer.Name, LogLevel.Verbose)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "Mobile_Device", Now.ToString & " Exception locating server object for " & drServers("ServerName").ToString() & " " & ex.ToString)
                End Try
                Try
                    If MyDominoServer Is Nothing Then
                        Exit Sub
                    End If
                Catch ex As Exception
                End Try
                href = drServers("MoreDetailsURL").ToString()
                Threshold_Minutes = Convert.ToInt32(drServers("SyncTimeThreshold").ToString())
                deviceId = drServers("DeviceID").ToString()
                If href <> "" Then
                    Dim myDeviceMoreInfo As New devicesMoreInfo

                    htmlStream = getHTMLStream(href, MyDominoServer.HTTP_UserName, MyDominoServer.HTTP_Password)
                    If htmlStream <> "" Then
                        myDeviceMoreInfo = returnDevicesMoreInfoObject(htmlStream)
                        WriteDeviceHistoryEntry("All", "Mobile_Device", Now.ToString & " myDeviceMoreInfo code " & myDeviceMoreInfo.code & vbCrLf, LogLevel.Verbose)
                        If myDeviceMoreInfo IsNot Nothing Then
                            WriteDeviceHistoryEntry("All", "Mobile_Device", Now.ToString & " myDeviceMoreInfo is not nothing " & myDeviceMoreInfo.message & vbCrLf)
                            If myDeviceMoreInfo.data IsNot Nothing Then
                                Try
                                    If myDeviceMoreInfo.data.LastSyncTime <> "" Then
                                        myDeviceMoreInfo.data.LastSyncTime = convertEPOCHTime(myDeviceMoreInfo.data.LastSyncTime)
                                        Dim timeNow, timeLastSync As Date
                                        timeNow = Date.Now
                                        timeLastSync = CDate(myDeviceMoreInfo.data.LastSyncTime)
                                        Dim Span As New TimeSpan
                                        Span = timeNow - timeLastSync
                                        'update the record

                                        Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.MobileDevices)(connectionString)
                                        Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.MobileDevices) = repository.Filter.Eq(Function(x) x.DeviceID, myDeviceMoreInfo.data.DeviceID) And repository.Filter.Eq(Function(x) x.IsActive, True)
                                        Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.MobileDevices) = repository.Updater.Set(Function(x) x.LastSyncTime, Convert.ToDateTime(myDeviceMoreInfo.data.LastSyncTime))
                                        repository.Update(filterDef, updateDef)

                                        If Span.TotalMinutes >= Threshold_Minutes Then
                                            '12/7/2015 NS modified for VSPLUS-2227
                                            If InMaintenance("Mobile Users", drServers("UserName").ToString() + "-" + deviceId) = False Then
                                                WriteDeviceHistoryEntry(MOBILE_ALERT, "All", Now.ToString & " Device with ID " & deviceId & " is overdue to sync.")
                                                myAlert.QueueAlert(MOBILE_ALERT, drServers("UserName").ToString() + "-" + deviceId, "Overdue Device Sync", drServers("DeviceName").ToString() & " for " & drServers("UserName").ToString() & " last synced at " & myDeviceMoreInfo.data.LastSyncTime.ToString(), "Mobile")
                                            End If
                                        Else
                                            '12/7/2015 NS modified for VSPLUS-2227
                                            If InMaintenance("Mobile Users", drServers("UserName").ToString() + "-" + deviceId) = False Then
                                                WriteDeviceHistoryEntry(MOBILE_ALERT, "All", Now.ToString & " Device with ID " & deviceId & " is current with syncing.")
                                                myAlert.ResetAlert(MOBILE_ALERT, drServers("UserName").ToString() + "-" + deviceId, "Overdue Device Sync", "Mobile")
                                            End If
                                        End If
                                    End If
                                Catch ex As Exception
                                    WriteDeviceHistoryEntry("All", "Mobile_Device", Now.ToString & " Error getting More device details: " + ex.Message.ToString())
                                End Try
                            End If
                        End If
                    End If

                End If
            Next
            Thread.Sleep(1000 * 60 * 3)
        Loop
    End Sub
    Private Sub getTravelerDeviceMoreDetailsWrapper()
        Do Until boolTimeToStop
            Try

                Dim myConnectionString As New VSFramework.XMLOperation
                Dim myAdapter As New VSFramework.VSAdaptor
                WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Get Distinct Server Names first ", LogLevel.Verbose)

                Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.MobileDevices)(connectionString)
                Dim Filter As FilterDefinition(Of VSNext.Mongo.Entities.MobileDevices) = _
                    (repo.Filter.Eq(Function(x) x.IsMoreDetailsFetched, False) Or _
                    repo.Filter.Exists(Function(x) x.IsMoreDetailsFetched, False)) And _
                    repo.Filter.In(Function(x) x.ServerName, MyDominoServers.Cast(Of MonitoredItems.DominoServer)().Select(Function(x) x.Name).ToList())

                Dim list As List(Of VSNext.Mongo.Entities.MobileDevices) = repo.Collection.Find(Filter) _
                                                                           .Project((New ProjectionDefinitionBuilder(Of VSNext.Mongo.Entities.MobileDevices)() _
                                                                           .Include(Function(x) x.DeviceID) _
                                                                           .Include(Function(x) x.LastSyncTime) _
                                                                           .Include(Function(x) x.Href) _
                                                                           .Include(Function(x) x.ServerName))) _
                                                                           .ToList() _
                                                                           .Select(Function(x) BsonSerializer.Deserialize(Of VSNext.Mongo.Entities.MobileDevices)(x)) _
                                                                           .ToList()



                WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Devices to process: " & list.Count() & " Records ", LogLevel.Verbose)

                Dim mycount As Int32 = 0
                Dim ListOfTravelerUserDetailThreads As List(Of Threading.Thread) = New List(Of Threading.Thread)

                For Each device As VSNext.Mongo.Entities.MobileDevices In list

                    WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " DeviceId: " & device.DeviceID.ToString(), LogLevel.Verbose)
                    mycount += 1


                    Dim getTravelerDeviceMoreDetailsThread As New Thread(AddressOf getTravelerDeviceMoreDetails)
                    ListOfTravelerUserDetailThreads.Add(getTravelerDeviceMoreDetailsThread)
                    Dim parameters As MyParameters = New MyParameters()
                    parameters.device = device
                    getTravelerDeviceMoreDetailsThread.Start(parameters)

                    If (mycount Mod 10 = 0 Or mycount = list.Count()) Then
                        WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " created all threads waiting to join ", LogLevel.Verbose)
                        For Each t As Thread In ListOfTravelerUserDetailThreads
                            t.Join()
                        Next
                        ' Empty the list of threads as we finished with the old ones already. 
                        ListOfTravelerUserDetailThreads = New List(Of Threading.Thread)
                    End If


                    ' Update all the more info data all at once. 
                    'updateTravelerDeviceCollection(moreInfoSQLs)
                    ' Wait for 5 minutes Then loop and check if prior scan was done according to what it should be. 
                    WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Updated the data sleeping for 5 mins before checking for next detail scan. ", LogLevel.Verbose)

                Next

                Try
                    WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & ": setting the devices as active.", LogLevel.Verbose)
                    SetActiveDevices()
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Error setting the devices as active : " & ex.ToString)

                End Try










                'For Each drServers As DataRow In dtServers.Rows
                '    WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Processing Server: " & drServers("ServerName").ToString(), LogLevel.Verbose)
                '    Dim SSQL As String = "SELECT MoreDetailsURL,DeviceId,ServerName FROM TRAVELER_DEVICES WHERE (IsMoreDetailsFetched IS NULL OR IsMoreDetailsFetched=0) AND ISNULL(MoreDetailsURL,'') != '' AND ISNULL(ServerName,'') ='" + drServers("ServerName").ToString() + "'"

                '    Dim dt As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), SSQL)
                '    WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Devices to process: " & dt.Rows.Count & " Records ", LogLevel.Verbose)

                '    Dim ListOfTravelerUserDetailThreads As List(Of Threading.Thread) = New List(Of Threading.Thread)
                '    Dim ListOfDetailDeviceInfoSQLs As List(Of String) = New List(Of String)
                '    Dim mycount As Int32 = 0
                '    Dim currentRow As Int32 = 0
                '    Dim totalRows As Int32 = dt.Rows.Count

                '    Dim moreInfoSQLs As List(Of System.Data.SqlClient.SqlCommand) = New List(Of System.Data.SqlClient.SqlCommand)
                '    For Each dr As DataRow In dt.Rows
                '        mycount += 1
                '        currentRow += 1
                '        Dim getTravelerDeviceMoreDetailsThread As New Thread(AddressOf getTravelerDeviceMoreDetails)
                '        ListOfTravelerUserDetailThreads.Add(getTravelerDeviceMoreDetailsThread)
                '        Dim parameters As MyParameters = New MyParameters()
                '        parameters.moreInfoSQL = moreInfoSQLs
                '        parameters.dr = dr
                '        getTravelerDeviceMoreDetailsThread.Start(parameters)
                '        If (mycount = 10 Or currentRow = totalRows) Then
                '            WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " created all threads waiting to join ", LogLevel.Verbose)
                '            For Each t As Thread In ListOfTravelerUserDetailThreads
                '                t.Join()
                '            Next
                '            ' Empty the list of threads as we finished with the old ones already. 
                '            ListOfTravelerUserDetailThreads = New List(Of Threading.Thread)
                '            mycount = 0
                '        End If
                '    Next
                '    ' Update all the more info data all at once. 
                '    'updateTravelerDeviceCollection(moreInfoSQLs)
                '    ' Wait for 5 minutes Then loop and check if prior scan was done according to what it should be. 
                '    WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Updated the data sleeping for 5 mins before checking for next detail scan. ", LogLevel.Verbose)

                'Next

            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & "Error in main loop. Error: " & ex.Message.ToString())
            End Try
            Thread.Sleep(1000 * 60 * 5)
        Loop
    End Sub

    Private Sub getTravelerDeviceMoreDetails(ByVal data As Object)
        Dim parameters As New MyParameters
        parameters = CType(data, MyParameters)
        Dim device As VSNext.Mongo.Entities.MobileDevices
        device = parameters.device


        Threading.Thread.Sleep(10000)
        Dim myMasterDevices As New devices
        Dim htmlStream As String = ""

        Dim href As String = device.Href
        WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " In getTravelerDeviceMoreDetails for each device  and Href = " & href)
        Dim ServerName As String = device.ServerName
        WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Trying to find the server name in domino collection:  " & ServerName)
        Dim MyDominoServer As MonitoredItems.DominoServer
        Try
            MyDominoServer = MyDominoServers.Search(ServerName)
            WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails" & MyDominoServer.Name, Now.ToString & " Searched for the server name: and the server is  " & MyDominoServer.Name)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Exception locating server object for " & ServerName & " " & ex.ToString)
        End Try
        Try
            If MyDominoServer Is Nothing Then
                Exit Sub
            End If
        Catch ex As Exception
        End Try

        'query 
        If href <> "" Then
            Dim myDeviceMoreInfo As New devicesMoreInfo
            'Dim getMoreInfoHTML As New Thread(AddressOf getHTMLStream)


            htmlStream = getHTMLStream(href, MyDominoServer.HTTP_UserName, MyDominoServer.HTTP_Password)
            If htmlStream <> "" Then
                myDeviceMoreInfo = returnDevicesMoreInfoObject(htmlStream)
                WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " myDeviceMoreInfo code " & myDeviceMoreInfo.code & vbCrLf, LogLevel.Verbose)
                If myDeviceMoreInfo IsNot Nothing Then
                    WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " myDeviceMoreInfo is not nothing " & myDeviceMoreInfo.message & vbCrLf, LogLevel.Verbose)
                    If myDeviceMoreInfo.data IsNot Nothing Then
                        Try
                            If myDeviceMoreInfo.data.LastSyncTime <> "" Then
                                myDeviceMoreInfo.data.LastSyncTime = convertEPOCHTime(myDeviceMoreInfo.data.LastSyncTime)
                                device.LastSyncTime = GetFixedDateTime(myDeviceMoreInfo.data.LastSyncTime)
                            End If
                            myDeviceMoreInfo.data.ServerName = ServerName
                            myMasterDevices.data.Add(myDeviceMoreInfo.data)
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Error getting More device details: " + ex.Message.ToString())
                        End Try
                    End If
                End If
            End If
        End If

        WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Collected records, ready to update the database.", LogLevel.Verbose)
        If myMasterDevices IsNot Nothing Then
            If myMasterDevices.data IsNot Nothing Then
                WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Attempting to insert Traveler User info. myMasterDevices.data has " & myMasterDevices.data.Capacity & " Records ", LogLevel.Verbose)
                For Each myTravelerDevice As TravelerDevice In myMasterDevices.data
                    Try
                        Select Case myTravelerDevice.policy_compliance.ToString
                            Case "0"
                                myTravelerDevice.policy_compliance = "No Policy"
                            Case "1"
                                myTravelerDevice.policy_compliance = "Client Not Supported"
                            Case "2"
                                myTravelerDevice.policy_compliance = "Not Compliant"
                            Case "3"
                                myTravelerDevice.policy_compliance = "Compliant - Limited"
                            Case "4"
                                myTravelerDevice.policy_compliance = "Compliant"

                            Case "5"
                                myTravelerDevice.policy_compliance = "Unavailable"
                            Case "6"
                                myTravelerDevice.policy_compliance = "Pending"
                        End Select

                        ' WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Policy State: " & myTravelerDevice.Access)
                        Select Case myTravelerDevice.Access.ToString
                            Case "0"
                                myTravelerDevice.Access = "Allow"
                            Case "1"
                                myTravelerDevice.Access = "Deny"
                            Case "2"
                                myTravelerDevice.Access = "Prohibit"
                        End Select

                        WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Approval Policy: " & myTravelerDevice.ApprovalPolicy, LogLevel.Verbose)
                        Select Case myTravelerDevice.ApprovalPolicy.ToString
                            Case "0"
                                myTravelerDevice.ApprovalPolicy = "Status Not Set"
                            Case "1"
                                myTravelerDevice.ApprovalPolicy = "Device Registered - Pending Approval"
                            Case "4"
                                myTravelerDevice.ApprovalPolicy = "Automatically Approved"
                            Case "8"
                                myTravelerDevice.ApprovalPolicy = "Denied"
                            Case "16"
                                myTravelerDevice.ApprovalPolicy = "Not Required"
                        End Select

                        cleanDeviceType(myTravelerDevice)

                        Select Case myTravelerDevice.AutoSyncType.ToString
                            Case "0"
                                myTravelerDevice.AutoSyncType = "None"
                            Case "8"
                                myTravelerDevice.AutoSyncType = "ActiveSync"
                            Case "4"
                                myTravelerDevice.AutoSyncType = "HTTP"
                            Case "1"
                                myTravelerDevice.AutoSyncType = "SMS"
                            Case "2"
                                myTravelerDevice.AutoSyncType = "TCP"

                        End Select

                        If InStr(myTravelerDevice.Client_Build, "8.5.2") Then
                            myTravelerDevice.AutoSyncType = "HTTP"
                        End If

                        WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Approval: " & myTravelerDevice.Approval, LogLevel.Verbose)
                        Select Case myTravelerDevice.Approval.ToString
                            Case "16"
                                myTravelerDevice.Approval = "Not Required"
                            Case "1"
                                myTravelerDevice.Approval = "Pending"
                            Case "8"
                                myTravelerDevice.Approval = "Denied"
                            Case Else
                                myTravelerDevice.Approval = "Approved"


                        End Select


                    Catch ex As Exception
                        WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Exception before inserting/updating traveler device : " & ex.ToString)
                    End Try

                    Try
                        addTravelerDeviceMoreInfoToCollection(myTravelerDevice)
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("All", "Traveler_Users_MoreDetails", Now.ToString & " Error Inserting data : " & ex.ToString)
                    End Try

                Next

            End If
        End If
    End Sub

    '    Private Sub CheckTravelerUsersV8(ByRef myDominoServer As MonitoredItems.DominoServer)
    '        'This function only works with Traveler versions prior to 9

    '        If myDominoServer.IsBeingScanned = True Then
    '            WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " The Traveler server is being scanned in another thread; Traveler user scanning and will be skipped for now.")
    '            Exit Sub
    '        End If

    '        '**********************************************************
    '        ' Traveler Users
    '        ' myDominoServer.IsBeingScanned = True
    '        Dim dbTraveler As Domino.NotesDatabase
    '        Dim viewTraveler As Domino.NotesView
    '        Dim docTraveler As Domino.NotesDocument
    '        Dim viewEntryTraveler As Domino.NotesViewEntry
    '        ' Dim viewEntryCollectionTraveler As Domino.NotesViewEntryCollection

    '        Dim NotesViewNavigatorTraveler As Domino.NotesViewNavigator

    '        Try
    '            dbTraveler = NotesSession.GetDatabase(myDominoServer.Name, "LotusTraveler.nsf", False)
    '            If String.IsNullOrWhiteSpace(dbTraveler.Title) Then
    '                dbTraveler.Title = "Untitled Database"
    '            End If
    '            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " The Lotus Traveler database title is " & dbTraveler.Title)
    '        Catch ex As Exception
    '            dbTraveler = Nothing
    '            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Error connecting to Lotus Traveler: " & ex.ToString)
    '            If InStr(ex.ToString, "Cannot open") > 0 Then
    '                myDominoServer.ResponseDetails += vbCrLf & "Insufficient access to LotusTraveler.nsf file.  Unable to get Traveler user information"
    '            End If
    '        End Try


    '        Try
    '            Try
    '                viewTraveler = dbTraveler.GetView("vDevice")
    '                If viewTraveler Is Nothing Then
    '                    viewTraveler = dbTraveler.GetView("Device Security")
    '                End If
    '                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " The Lotus Traveler database View name is " & viewTraveler.Name)

    '            Catch ex As Exception
    '                WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Error connecting to Devices view " & ex.ToString)
    '            End Try

    '            Try
    '                NotesViewNavigatorTraveler = viewTraveler.CreateViewNav
    '                viewEntryTraveler = NotesViewNavigatorTraveler.GetFirstDocument
    '            Catch ex As Exception
    '                WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Error connecting to Devices view, exiting sub")
    '                'myDominoServer.IsBeingScanned = False
    '                Exit Sub
    '            End Try



    '            While Not viewEntryTraveler Is Nothing
    '                docTraveler = viewEntryTraveler.Document
    '                If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Processing Traveler device for " & docTraveler.GetItemValue("Username")(0))
    '                If viewEntryTraveler.IsDocument Then
    '                    docTraveler = viewEntryTraveler.Document
    '                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Processing Traveler viewEntry for " & viewEntryTraveler.ColumnValues(0) & " with security policy " & viewEntryTraveler.ColumnValues(4))
    '                Else
    '                    GoTo NextEntry
    '                End If

    '                Dim device As New TravelerDevice
    '                With device
    '                    'Try
    '                    '    .DocID = docTraveler.UniversalID.ToString
    '                    'Catch ex As Exception

    '                    'End Try
    '                    Try
    '                        .UserName = docTraveler.GetItemValue("Username")(0)
    '                    Catch ex As Exception

    '                    End Try

    '                    Try
    '                        .wipeSupported = docTraveler.GetItemValue("wipeSupported")(0)
    '                    Catch ex As Exception

    '                    End Try
    '                    Try
    '                        .LastSyncTime = docTraveler.GetItemValue("lastSyncTime")(0).ToString
    '                    Catch ex As Exception
    '                        WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Error getting last Sync time " & ex.ToString)
    '                    End Try

    '                    If docTraveler.GetItemValue("device_conn")(0) = "1" Then
    '                        .ConnectionState = "Connected"
    '                    Else
    '                        .ConnectionState = "Disconnected"
    '                    End If
    '                    .OS_Type = docTraveler.GetItemValue("device_type")(0)
    '                    .DeviceName = docTraveler.GetItemValue("device_provider")(0)
    '                    .DeviceID = docTraveler.GetItemValue("deviceid")(0)


    '                    Try
    '                        Dim ApprovalFlag As Integer
    '                        ApprovalFlag = docTraveler.GetItemValue("approvalFlag")(0)
    '                        Select Case ApprovalFlag
    '                            Case 0
    '                                .Approval = "Not Required"
    '                            Case 16
    '                                .Approval = "Not Required"
    '                            Case 1
    '                                .Approval = "Pending"
    '                            Case 1
    '                                .Approval = "Denied"
    '                            Case Else
    '                                .Approval = "Approved"
    '                        End Select
    '                    Catch ex As Exception

    '                    End Try


    '                    Try

    '                        .Access = "Allow"
    '                        If docTraveler.GetItemValue("banned")(0) = "1" Then
    '                            .Access = "Prohibit"
    '                        End If

    '                        If docTraveler.GetItemValue("wipeRequested")(0) <> "" Then
    '                            .Access = "Deny"
    '                        End If
    '                    Catch ex As Exception

    '                    End Try

    '                    Try
    '                        'Security policy I get from the view
    '                        .ApprovalPolicy = viewEntryTraveler.ColumnValues(4)
    '                    Catch ex As Exception
    '                        .ApprovalPolicy = "Unknown"
    '                    End Try


    '                    ' @If (notificationType = "0" ; "None" ;
    '                    ' notificationType = "8" ; "ActiveSync" ;
    '                    ' notificationType = "4"  | @Begins(device_level; "8.5.2") ; "HTTP" ;
    '                    ' device_level = "" ; "" ;
    '                    ' notificationType = "1" ; "SMS" ;
    '                    ' notificationType = "2" ; "TCP" ;
    '                    '"Unknown")
    '                    Try
    '                        Select Case docTraveler.GetItemValue("notificationType")(0)
    '                            Case "0"
    '                                .AutoSyncType = "None"
    '                            Case "8"
    '                                .AutoSyncType = "ActiveSync"
    '                            Case "4"
    '                                .AutoSyncType = "HTTP"
    '                            Case "1"
    '                                .AutoSyncType = "SMS"
    '                            Case "2"
    '                                .AutoSyncType = "TCP"
    '                        End Select

    '                        If InStr(docTraveler.GetItemValue("device_level")(0), "8.5.2") > 0 Then
    '                            .AutoSyncType = "HTTP"
    '                        End If
    '                    Catch ex As Exception

    '                    End Try

    '                End With

    '                Try
    '                    If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Identified Traveler user " & device.UserName & " with device " & device.DeviceName)
    '                Catch ex As Exception
    '                    WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Error with Traveler user " & ex.ToString)
    '                End Try
    '                UpdateTravelerDeviceStatusTable(device, myDominoServer.Name, myDominoServer.TravelerHA_Pool_Name)
    'NextEntry:

    '                viewEntryTraveler = NotesViewNavigatorTraveler.GetNextDocument(viewEntryTraveler)
    '                ' docTraveler = viewTraveler.GetNextDocument(docTraveler)

    '            End While
    '        Catch ex As Exception

    '        End Try

    'cleanup:
    '        Try
    '            System.Runtime.InteropServices.Marshal.ReleaseComObject(dbTraveler)
    '            System.Runtime.InteropServices.Marshal.ReleaseComObject(viewTraveler)
    '            System.Runtime.InteropServices.Marshal.ReleaseComObject(docTraveler)
    '            System.Runtime.InteropServices.Marshal.ReleaseComObject(viewEntryTraveler)
    '            System.Runtime.InteropServices.Marshal.ReleaseComObject(NotesViewNavigatorTraveler)
    '            ' System.Runtime.InteropServices.Marshal.ReleaseComObject(s)
    '        Catch ex As Exception

    '        End Try


    '        'VitalStatus is no longer supported as of 1.2.3  -AF
    '        'Try
    '        '    '  myDominoServer.IsBeingScanned = False
    '        '    WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Attempting to update VitalStatus with Traveler User info...")
    '        '    VitalStatus_TravelerUsers()
    '        'Catch ex As Exception

    '        'End Try


    '    End Sub

    Private Sub CheckTravelerBackend(ByRef MyDominoServer As MonitoredItems.DominoServer)
        'Figure out whether or not to scan the back end for this server
        'This is only done for HA servers
        Try
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Checking Traveler back end data store... I have " & myTravelerBackends.Count & " to check.")

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Checking Traveler back end data store... I have: Exception" + ex.Message.ToString())
        End Try
        're-set it back, in case the pool gets removed after orginally set
        MyDominoServer.TravelerHA_Pool_Name = ""
        If myTravelerBackends.Count = 0 Then Exit Sub

        Dim boolContinue As Boolean = False
        Dim BE As MonitoredItems.Traveler_Backend
        For Each BackEnd As MonitoredItems.Traveler_Backend In myTravelerBackends
            If BackEnd.TestScanServer.ToUpper = MyDominoServer.Name.ToUpper Then
                BE = BackEnd
                boolContinue = True
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This server is associated with a Traveler back end.", LogLevel.Verbose)
            End If
            If BackEnd.UsedByServers.ToLower.Contains(MyDominoServer.Name.ToLower) Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This Server is associated with the Traveler HA Pool:" & BackEnd.TravelerServicePoolName, LogLevel.Verbose)
                MyDominoServer.TravelerHA_Pool_Name = BackEnd.TravelerServicePoolName
            End If
        Next

        If boolContinue = False Then
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " This server is not associated with a Traveler back end.", LogLevel.Verbose)
            Exit Sub
        End If

        Dim RowsAffected As Integer
        Dim myResult As String = "Pass"

        Try
            If InStr(BE.DataStore, "SQL") Then

                Dim mySqlConnection As New SqlClient.SqlConnection
                Dim myCommand As New SqlClient.SqlCommand
                Dim sConnectionString As String = ""

                sConnectionString = "Data Source=" & BE.ServerName & ", " & BE.Port & "; Integrated Security=" & BE.IntegratedSecurity & ";Initial Catalog=" + BE.DatabaseName.ToString() + " ;Persist Security Info=False;User ID=" & _
                 BE.UserName & ";Password=" & BE.Password & ";Min Pool Size=20;Max Pool Size=500; Connection Timeout=30;"
                'WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Connection string is " & sConnectionString)

                mySqlConnection.ConnectionString = sConnectionString
                mySqlConnection.Open()
                myCommand.Connection = mySqlConnection
                myCommand.CommandText = "SELECT COUNT (Distinct [USERNAME]) FROM [dbo].[users]"
                RowsAffected = myCommand.ExecuteScalar
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " SQL Server result is " & RowsAffected.ToString)
                If RowsAffected > 0 Then
                    myResult = "Pass"
                Else
                    '5/20/2016 NS modified for VSPLUS-2973
                    myResult = "Unknown"
                End If
                mySqlConnection.Close()

            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error connecting to Traveler back end ..." & ex.ToString)
            myResult = "Fail"
        End Try


        Try
            If InStr(BE.DataStore, "DB2") Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Checking DB2 Connection.")
                Dim sConnectionString As String = ""
                'SERVER=srvrName:1234;DATABASE=testdb;UID=userName;PWD=userPass;
                'sConnectionString = "Data Source=" & BE.ServerName & ", " & BE.Port & "; Integrated Security=" & BE.IntegratedSecurity & ";Initial Catalog=TravelerHA ;Persist Security Info=False;User ID=" & _
                '	BE.UserName & ";Password=" & BE.Password & ";Min Pool Size=20;Max Pool Size=500; Connection Timeout=30;"
                sConnectionString = "SERVER=" & BE.ServerName & ":" & BE.Port.ToString & ";DATABASE=" + BE.DatabaseName.ToString() + ";UID=" & BE.UserName & ";PWD=" & BE.Password & ";"
                'WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Connection string is " & sConnectionString)

                Dim myDB2Connection As New IBM.Data.DB2.DB2Connection
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " After Creating IBM.DB2.DB2Connection.", LogLevel.Verbose)

                myDB2Connection.ConnectionString = sConnectionString
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " after setting connection string.", LogLevel.Verbose)

                myDB2Connection.Open()
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " after open.", LogLevel.Verbose)
                Dim myCommand As New IBM.Data.DB2.DB2Command
                myCommand.Connection = myDB2Connection
                'myCommand.CommandText = "SELECT COUNT (Distinct [USERNAME]) FROM [TravelerHA].[dbo].[users]"
                'select count(*) from "dbo"."users"
                myCommand.CommandText = " select count(*) from " + Chr(34) + "USERS" + Chr(34)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " SQL:'" + myCommand.CommandText)
                RowsAffected = myCommand.ExecuteScalar
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " DB2 query result is " & RowsAffected.ToString, LogLevel.Verbose)
                If RowsAffected > 0 Then
                    myResult = "Pass"
                Else
                    '5/20/2016 NS modified for VSPLUS-2973
                    myResult = "Unknown"
                End If

                myDB2Connection.Close()
                myDB2Connection.Dispose()

            End If
        Catch ex As DllNotFoundException
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " DB2 Client Not installed: " & ex.ToString)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception querying DB2: " & ex.ToString)
        End Try

        Try
            'Update the Traveler health table for the server associated with this back end (The 'Used By Servers' field on the form)
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Updating the Traveler_Status table ", LogLevel.Verbose)

            Dim myObj As New VSAdaptor
            Dim strSQL As String = "Update Traveler_Status Set HA_Datastore_Status = '" & myResult & "' WHERE ServerName = '" & BE.TestScanServer & "'"
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Updating the Traveler_Status table with " & strSQL, LogLevel.Verbose)
            myObj.ExecuteNonQueryAny("VitalSigns", "NA", strSQL)

            Dim UsedByServersArray() As String = Split(BE.UsedByServers, ",")
            Dim serversInSql As String = ""

            'For Each currString As String In UsedByServersArray
            '    serversInSql = serversInSql & " ServerName = '" & currString & "' || "
            'Next

            'Changed 8/8/2014 because we saw at McKinsey that the || syntax did not work
            For Each currString As String In UsedByServersArray
                serversInSql = serversInSql & " ServerName = '" & currString & "' OR "
            Next

            If (serversInSql.Length > 4) Then
                serversInSql = serversInSql.Substring(0, serversInSql.Length - 4)
            End If

            strSQL = "Update Traveler_Status Set HA_Datastore_Status = '" & myResult & "' WHERE" & serversInSql
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Updating the Traveler_Status table with " & strSQL, LogLevel.Verbose)
            myObj.ExecuteNonQueryAny("VitalSigns", "NA", strSQL)
        Catch ex As Exception

        End Try


        Try
            Dim UsedByServersArray() As String = Split(BE.UsedByServers, ",")
            For Each currString As String In UsedByServersArray
                If myResult = "Fail" Then
                    myAlert.QueueAlert(MyDominoServer.ServerType, MyDominoServer.Name, "Traveler Data Store", "The Traveler Datastore  is not responding.", MyDominoServer.Location)
                Else
                    myAlert.ResetAlert(MyDominoServer.ServerType, MyDominoServer.Name, "Traveler Data Store", MyDominoServer.Location)
                End If

            Next
        Catch ex As Exception

        End Try

    End Sub

    Private Sub CheckTravelerStatusReasons(ByRef DominoServer As MonitoredItems.DominoServer)
        'This function queries the Domino server for the server tasks
        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Checking the status of Domino Traveler status reasons. ")

        Dim vsObj As New VSAdaptor
        Dim strSQL As String = "Delete FROM TravelerStatusReasons WHERE ServerName='" & DominoServer.Name & "'"
        Dim strReasons As String = ""

        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.Name, DominoServer.Name) And repository.Filter.Eq(Function(x) x.Type, DominoServer.ServerType)
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Unset(Function(x) x.TravelerStatusReasons)
            repository.Update(filterDef, updateDef)

            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Deleted previous Traveler status records.")
        Catch ex As Exception
            '10/2/2015 NS added for VSPLUS-2217
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " The following exception has occurred while executing the SQL query " & strSQL & ". " & ex.Message)
        End Try

        If DominoServer.Traveler_Status = "Green" Then
            GoTo Alerts
        End If

        Dim strServerTasks As String = ""

        Try
            'Send a remote console command to get the value of the stat
            ' If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " sending SH STAT Server.Task request to " & DominoServer.Name)
            Dim UpperLimit As Integer = 2

            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Attempting to get server task information.")
            If DominoServer.Traveler_Status.ToUpper = "YELLOW" Then
                strServerTasks = GetOneStat(DominoServer.Name, "Traveler", "Status.state.yellow")
                '  Traveler.Status.State = Yellow
            Else
                strServerTasks = GetOneStat(DominoServer.Name, "Traveler", "Status.state.red") & vbCrLf & GetOneStat(DominoServer.Name, "Traveler", "Status.state.yellow")
            End If


            If InStr(strServerTasks, "Traveler.Status") > 0 Then
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Got Traveler Status Information... first attempt")
            Else
                For n As Integer = 0 To UpperLimit
                    strServerTasks = GetOneStat(DominoServer.Name, "Traveler", "Status.*")
                    If Not (InStr(strServerTasks, "ERROR")) And InStr(strServerTasks, "Traveler.Status") > 0 Then Exit For
                    Thread.Sleep(500)
                Next
            End If

            If InStr(DominoServer.Name.ToUpper, "RPR") Then
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "The Traveler status stats are:" & vbCrLf & vbCrLf & strServerTasks & vbCrLf)
            End If
            ' WriteAuditEntry(Now.ToString & " Server TASK REQUEST returned " & strServerTasks)
            DominoServer.ShowTasks = strServerTasks.Replace(ControlChars.NewLine & ControlChars.NewLine, ControlChars.NewLine)
            ' strServerTasks = s.SendConsoleCommand(DominoServer.Name, "sh stat server.task")
            ' WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " SERVER TASKS request replied with " & vbCrLf & strServerTasks)
        Catch ex As Exception
            '10/2/2015 NS added for VSPLUS-2217
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " The following exception has occurred in CheckTravelerStatusReasons: " & ex.Message)
            WriteAuditEntry(Now.ToString & " CheckTravelerStatusReasons module Error: " & DominoServer.Name & vbCrLf & ex.Message)
            ' System.Runtime.InteropServices.Marshal.ReleaseComObject(s)
            strServerTasks = Nothing
            Exit Sub
        End Try

        If Not (InStr(strServerTasks, "=") > 0) Then
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Case #1 SERVER TASKS request for Traveler status does not contain data this time, exiting sub.")
            GoTo Alerts
        End If

        If strServerTasks = "" Then
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "Case #2 SERVER TASKS request does not contain data this time, exiting sub.")
            GoTo Alerts
        End If


        'Parse the tasks into a collection.  
        Dim myTravelerStatus As New MonitoredItems.TravelerStatusReasonsCollection

        'The statistics in strResult are in the format: 
        '  Traveler.Status.State.Yellow = Disk space for C:\Lotus\Domino\data has 7 percent free that is less than the threshold of 15. Disk space for C:\Lotus\Domino\data has 9 gigabytes free that is less than the threshold of 10.

        Dim myStart, myEnd As Integer
        Dim thisLine As String = ""
        Dim myLineEnd As Integer
        Try

            Do While InStr(strServerTasks, "=")
                Dim myStatus As New MonitoredItems.TravelerStatusReason
                Try
                    'Get the task name, such as 'Directory Indexer'
                    myStart = InStr(strServerTasks, "=") + 1

                    If Mid(strServerTasks, myStart).Contains(vbCrLf) Then
                        myEnd = InStr(myStart, strServerTasks, vbCrLf)
                    Else
                        myEnd = strServerTasks.Length + 1
                    End If

                    'This is to try to stop it from infinitly exceptioning due to "Length must be greater than 0"
                    If (myEnd < myStart) Then
                        myEnd = myStart + 1
                    End If


                    myLineEnd = InStr(myStart, strServerTasks, vbCrLf)

                    If InStr(strServerTasks, vbCrLf) Then
                        thisLine = Trim(Mid(strServerTasks, myStart, myLineEnd - myStart))
                    End If




                    'Add every server task, even repeats
                    If Mid(strServerTasks, 1, myStart).Contains(".Yellow") Or Mid(strServerTasks, 1, myStart).Contains(".Red") Then
                        If (Mid(strServerTasks, 1, myStart).Contains(".Yellow")) Then
                            myStatus.StatusDetails = "Yellow: "
                        Else
                            myStatus.StatusDetails = "Red: "
                        End If
                        myStatus.StatusDetails += Trim(Mid(strServerTasks, myStart, myEnd - myStart)).Trim()
                        myTravelerStatus.Add(myStatus)
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " GetDominoServerTasks parsed the Traveler status as " & myStatus.StatusDetails)
                    Else
                        WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Will not add the line since there is no status reason contained in it.  Line: " & Mid(strServerTasks, 1, myEnd), LogUtilities.LogUtils.LogLevel.Normal)
                    End If

                    strServerTasks = Mid(strServerTasks, myEnd)

                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing server tasks: " & ex.Message & ".  Line that threw Error: " & strServerTasks)
                    If (myEnd > 0) Then
                        strServerTasks = Mid(strServerTasks, myEnd)
                    Else
                        strServerTasks = ""
                    End If
                End Try


            Loop
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Error parsing server tasks: " & ex.Message)
        End Try




        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " Now summarizing Traveler status...")


        Try
            'WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "  Now Updating DominoServerTaskStatus table.  ")
            Dim listOfReasons As New List(Of String)()
            For Each StatusReason As MonitoredItems.TravelerStatusReason In myTravelerStatus
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, vbCrLf)
                WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & "  Traveler status reason is: " & StatusReason.StatusDetails)

                Try
                    listOfReasons.Add(StatusReason.StatusDetails)
                    strSQL = "INSERT INTO TravelerStatusReasons (ServerName, Details, LastUpdate) " _
                                          & "VALUES ('" & DominoServer.Name & "', '" & StatusReason.StatusDetails & "', '" & FixDateTime(Now) & "') "
                    strSQL = vsObj.SQL_Server_Compatible_SQLStatement(strSQL)
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, strSQL)
                    vsObj.ExecuteNonQueryAny("VitalSigns", "Status", strSQL)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, "Exception inserting into DominoServerTaskStatus: " & ex.ToString)
                End Try

                Try
                    strReasons += StatusReason.StatusDetails.ToString() & vbCrLf & vbCrLf
                Catch ex As Exception
                    '10/2/2015 NS added for VSPLUS-2217
                    WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " The following exception has occurred in CheckTravelerStatusReasons: " & ex.Message)
                End Try

            Next

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.Name, DominoServer.Name) And repository.Filter.Eq(Function(x) x.Type, DominoServer.ServerType)
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.TravelerStatusReasons, listOfReasons)
            repository.Update(filterDef, updateDef)

            'gets rid of the comma and space
            'strReasons = strReasons.Substring(0, strReasons.Length)

        Catch ex As Exception
            '10/2/2015 NS added for VSPLUS-2217
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " The following exception has occurred in CheckTravelerStatusReasons: " & ex.Message)
        End Try


Alerts:
        Try
            If DominoServer.Traveler_Status = "Red" Then
                '5/20/2016 NS modified for VSPLUS-2874
                myAlert.QueueAlert(DominoServer.ServerType, DominoServer.Name, "Traveler Status: Red", "The overall status of IBM Notes Traveler on " & DominoServer.Name & " is " & DominoServer.Traveler_Status & " due to: " & vbCrLf & strReasons, DominoServer.Location)
            Else
                'In some cases Traveler_Status was blank, and this was resetting the alert even though the server was still Red  - AF
                If Trim(DominoServer.Traveler_Status) <> "" Then
                    Dim strDetails As String
                    strDetails = "The overall status of IBM Notes Traveler is " & DominoServer.Traveler_Status
                    '5/20/2016 NS modified for VSPLUS-2874
                    myAlert.ResetAlert(DominoServer.ServerType, DominoServer.Name, "Traveler Status: Red", DominoServer.Location, strDetails, "Traveler")
                End If

            End If

        Catch ex As Exception
            '10/2/2015 NS added for VSPLUS-2217
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " The following exception has occurred in CheckTravelerStatusReasons while queueing/resetting an alert: " & ex.Message)
        End Try


        Try
            If DominoServer.Traveler_Status = "Yellow" Then
                '5/20/2016 NS modified for VSPLUS-2874
                myAlert.QueueAlert(DominoServer.ServerType, DominoServer.Name, "Traveler Status: Yellow", "The overall status of IBM Notes Traveler on " & DominoServer.Name & " is " & DominoServer.Traveler_Status & " due to: " & vbCrLf & strReasons, DominoServer.Location)
            Else
                Dim strDetails As String
                If DominoServer.Traveler_Status = "Green" Then
                    strDetails = "The overall status of IBM Notes Traveler is Green"
                    '5/20/2016 NS modified for VSPLUS-2874
                    myAlert.ResetAlert(DominoServer.ServerType, DominoServer.Name, "Traveler Status: Yellow", DominoServer.Location, strDetails, "Traveler")
                End If

            End If

        Catch ex As Exception
            '10/2/2015 NS added for VSPLUS-2217
            WriteDeviceHistoryEntry("Domino", DominoServer.Name, Now.ToString & " The following exception has occurred in CheckTravelerStatusReasons while queueing/resetting an alert: " & ex.Message)
        End Try

    End Sub


    Private Sub CheckTravelerMailServerAccess(ByRef myDominoServer As MonitoredItems.DominoServer)
        WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Checking the Traveler server's access to mail servers....")
        If Not (InStr(myDominoServer.Statistics_Traveler, "Traveler.DCA.DB_OPEN") > 0) Then
            WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " The Traveler server's stats are not available. ")
            Exit Sub
        End If

        Dim parseStr As String
        Dim tStrInd As Integer
        Dim rStrInd As Integer
        Dim sStrInd As Integer
        Dim timeVal As Long
        Dim rangeVal As String
        Dim srvVal As String
        Dim travelerSrvVal As String
        Dim linesArr As String()
        Dim statsDict As New Dictionary(Of String, Long)

        Const travelerIDStr As String = "Traveler.DCA.DB_OPEN.Time.Histogram"
        Const travelerIDStrWithJava As String = "Traveler.DCA.DB_OPEN.Time.Java.Histogram"
        Const equalStr As String = "="
        Const dotStr As String = "."
        Const histStr As String = "Histogram."

        parseStr = myDominoServer.Statistics_Traveler
        linesArr = parseStr.Split(vbCrLf)
        '   WriteDeviceHistoryEntry("Domino", myDominoServer.Name, vbCrLf & vbCrLf)
        Dim x As Integer
        For x = LBound(linesArr) To UBound(linesArr)
            '   WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " " & linesArr(x))
            If linesArr(x).IndexOf(travelerIDStr) > -1 Or linesArr(x).IndexOf(travelerIDStrWithJava) > -1 Then
                tStrInd = linesArr(x).LastIndexOf(equalStr)
                rStrInd = linesArr(x).LastIndexOf(dotStr)
                sStrInd = linesArr(x).LastIndexOf(histStr)
                If tStrInd > -1 Then
                    timeVal = linesArr(x).Substring(tStrInd + equalStr.Length, linesArr(x).Length - tStrInd - equalStr.Length)
                    rangeVal = linesArr(x).Substring(rStrInd + dotStr.Length, tStrInd - rStrInd - dotStr.Length)
                    srvVal = linesArr(x).Substring(sStrInd + histStr.Length, rStrInd - sStrInd - histStr.Length)
                    'WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " The mail server name - srvVal - is " & srvVal)
                    '3/7/14 AF added to optimize SQL query later
                    srvVal = DominoServerAbbreviatedName(srvVal)
                    ' WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " The transformed mail server name - srvVal - is " & srvVal)
                    ' statsDict.Add(srvVal + "_" + rangeVal, timeVal)
                    '9/1/2015 NS modified for VSPLUS-2096
                    If srvVal <> "" Then
                        statsDict.Add(DominoServerAbbreviatedName(srvVal) + "_" + rangeVal, timeVal)
                    End If
                End If
            End If
        Next
        WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " The Traveler server's hierarchical name is " & myDominoServer.HierarchicalName)

        ' travelerSrvVal = myDominoServer.HierarchicalName
        travelerSrvVal = myDominoServer.Name
        Call UpdateTravelerTable(travelerSrvVal, statsDict, myDominoServer)
    End Sub

    Public Function DominoServerAbbreviatedName(ByVal Name As String) As String
        Dim AbbreviatedName As String
        AbbreviatedName = Name.Replace("CN=", "")
        AbbreviatedName = AbbreviatedName.Replace("O=", "")
        AbbreviatedName = AbbreviatedName.Replace("OU=", "")
        Return AbbreviatedName
    End Function

    Public Sub UpdateTravelerTable(ByVal travelerSrvVal As String, ByVal statsDict As Dictionary(Of String, Long), ByRef myDominoServer As MonitoredItems.DominoServer)

        ' WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Attempting to update Traveler Table.")
        Dim uStrInd As Integer
        Dim pair As KeyValuePair(Of String, Long)
        Dim timeVal As Long
        Dim timeValPrev As Long
        Dim deltaVal As Long
        Dim srvVal As String
        Dim rangeVal As String
        Dim found As Boolean
        Dim datetimeVal As Date
        Const underscoreStr As String = "_"
        Dim dtNow As DateTime = Now

        datetimeVal = Nothing
        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.TravelerStats)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.TravelerStats)
            Dim entityList As List(Of VSNext.Mongo.Entities.TravelerStats)
            Dim entity As VSNext.Mongo.Entities.TravelerStats

            For Each pair In statsDict
                uStrInd = pair.Key.LastIndexOf(underscoreStr)
                srvVal = pair.Key.Substring(0, uStrInd)
                rangeVal = pair.Key.Substring(uStrInd + 1, pair.Key.Length - uStrInd - 1)
                timeVal = pair.Value
                WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " The current value of the open times stat is " & timeVal)
                If datetimeVal = Nothing Then
                    filterDef = repository.Filter.Eq(Function(x) x.TravelerServerName, travelerSrvVal) _
                        And repository.Filter.Eq(Function(x) x.MailServerName, srvVal) _
                        And repository.Filter.Eq(Function(x) x.Interval, rangeVal)
                    entityList = repository.Find(filterDef, Function(x) x.DateUpdated, 0, 1, True).ToList()
                Else
                    filterDef = repository.Filter.Eq(Function(x) x.TravelerServerName, travelerSrvVal) _
                        And repository.Filter.Eq(Function(x) x.MailServerName, srvVal) _
                        And repository.Filter.Eq(Function(x) x.Interval, rangeVal)
                    entityList = repository.Find(filterDef).ToList()
                End If

                found = False
                If entityList.Count <> 0 Then
                    WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Top of read loop.")
                    entity = entityList(0)
                    Try
                        timeValPrev = entity.OpenTimes
                        WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Previous actual open value is " & timeValPrev)
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Exception calculating timeValPrev: " & ex.ToString)
                    End Try

                    Try
                        datetimeVal = entity.DateUpdated
                        WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Previous date value is " & datetimeVal)
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Exception calculating datetimeVal: " & ex.ToString)
                    End Try

                    Try
                        '6/24/2015 NS modified for VSPLUS-1898
                        If (timeVal < timeValPrev) Then
                            deltaVal = timeVal - timeValPrev
                            WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " The delta value is " & deltaVal)
                            deltaVal = timeVal
                            WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " The recorded delta value is " & deltaVal & " due to a possible server restart.")
                        Else
                            deltaVal = timeVal - timeValPrev
                            WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " The delta value is " & deltaVal)
                        End If
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Exception calculating deltaVal: " & ex.ToString)
                    End Try


                    found = True
                    WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Bottom of read loop.")
                End If

                entity = New VSNext.Mongo.Entities.TravelerStats() With {
                    .TravelerServerName = travelerSrvVal,
                    .MailServerName = srvVal,
                    .Interval = rangeVal,
                    .Delta = deltaVal,
                    .OpenTimes = timeVal,
                    .DateUpdated = dtNow
                }
                repository.Insert(entity)
                WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Attempting to update Traveler Table with new entity")
            Next

        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Error while inserting record on table..." & ex.Message)
        Finally

        End Try
    End Sub

    Private Sub CheckTravelerHA(ByRef myDominoServer As MonitoredItems.DominoServer)
        'Now figure out if the server is in an High Availability Pool or not


        Dim myServletURL As New MonitoredItems.URL
        With myServletURL
            .URL = myDominoServer.IPAddress & "/servlet/traveler"
            WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " I figure that the Traveler servlet is " & myServletURL.URL)
        End With

        Dim ChilkatHTTP As New Chilkat.Http
        myServletURL.HTML = ""
        Try
            Dim success As Boolean
            success = ChilkatHTTP.UnlockComponent("MZLDADHttp_efwTynJYYR3X")
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Error unlocking Chilkat component: " & ex.ToString)
            ' WriteDeviceHistoryEntry("URL", myServletURL.Name, Now.ToString & " Error unlocking component: " & ex.ToString)
        End Try
        Dim myRegistry As New RegistryHandler

        Try

            If myRegistry.ReadFromRegistry("ProxyEnabled") = "True" Then
                WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Attempting to route through proxy server.")
                ChilkatHTTP.SocksHostname = myRegistry.ReadFromRegistry("ProxyServer")
                ChilkatHTTP.SocksPort = myRegistry.ReadFromRegistry("ProxyPort")
                ChilkatHTTP.SocksUsername = myRegistry.ReadFromRegistry("ProxyUser")
                ChilkatHTTP.SocksPassword = myRegistry.ReadFromRegistry("ProxyPassword")
            End If
            myRegistry = Nothing
        Catch ex As Exception

        End Try
        ''Get the passwords from the registry
        'Dim MyPass As Byte()
        'Dim mySecrets As New VSFramework.TripleDES

        Dim TravelerServlet As String = "OK"
        Dim strSQL As String
        Dim objVSAdaptor As New VSAdaptor

        Try
            ' ChilkatHTTP.Login = myRegistry.ReadFromRegistry("Domino HTTP User")
            ChilkatHTTP.Login = myDominoServer.HTTP_UserName
            WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Domino HTTP User is " & ChilkatHTTP.Login)
            ChilkatHTTP.Password = myDominoServer.HTTP_Password

            'MyPass = myRegistry.ReadFromRegistry("Domino HTTP Password")  'Domino password as encrypted byte stream
            'If Not MyPass Is Nothing Then
            '	ChilkatHTTP.Password = mySecrets.Decrypt(MyPass) 'password in clear text, stored in memory now
            'Else
            '	ChilkatHTTP.Password = Nothing
            'End If
            ' WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino HTTP password is " & ChilkatHTTP.Password)
        Catch ex As Exception
            'MyPass = Nothing
        End Try

        WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Checking the Traveler server's HA status....")

        Try
            If myDominoServer.Traveler_Server_HA = True Then
                'No need to perform this over and over
                WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " This Traveler server is HA")
                Exit Sub
            End If
        Catch ex As Exception

        End Try

        myServletURL.HTML = ""
        myServletURL.URL = myDominoServer.IPAddress & "/lotustraveler.nsf/xServers.xsp"
        WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Querying " & myServletURL.URL)
        Try
            Dim n As Integer
            strSQL = ""
            Do While myServletURL.HTML = ""
                myServletURL.HTML = ChilkatHTTP.QuickGetStr(myServletURL.URL)
                n += 1
                Thread.Sleep(500)
                If n > 5 Then Exit Do
            Loop

            If (myServletURL.HTML = "") Then
                'This server is not an HA server if this page comes back blank
                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.Name, myDominoServer.Name) _
                                                                                     And repository.Filter.Eq(Function(x) x.Type, myDominoServer.ServerType)
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.TravelerHA, False)
                repository.Update(filterDef, updateDef)

            Else
                If InStr(myServletURL.HTML, "Domino Name") > 0 Or InStr(myServletURL.HTML, "Nome Domino") > 0 Then
                    ' This server is an HA server
                    myDominoServer.Traveler_Server_HA = True
                    Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                    Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.Name, myDominoServer.Name) _
                                                                                         And repository.Filter.Eq(Function(x) x.Type, myDominoServer.ServerType)
                    Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater.Set(Function(x) x.TravelerHA, True)
                    repository.Update(filterDef, updateDef)
                End If
            End If

        Catch ex As Exception

            WriteDeviceHistoryEntry("Domino", myDominoServer.Name, Now.ToString & " Error performing HTTP.Get while determining HA status: " & ex.Message)
        End Try
    End Sub

    Private Sub CheckTravelerServlet(ByRef MyDominoServer As MonitoredItems.DominoServer)
        WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Checking the Traveler server's servlet availablity....")

        If Not MyDominoServer.ScanTravelerServer Then
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Stopping scan on servlet due to configuration of the server.")
        End If

        Dim myServletURL As New MonitoredItems.URL


        With myServletURL
            'Modified for VSPLUS-1933  AF
            'If the user has specified an external alias, then you should use that; if not use the hostname/IP Address
            If MyDominoServer.ExternalAlias.Trim <> "" Then
                .URL = MyDominoServer.ExternalAlias & "/servlet/traveler"
            Else
                .URL = MyDominoServer.IPAddress & "/servlet/traveler"
            End If

            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " I figure that the Traveler servlet is " & myServletURL.URL)
        End With

        Dim ChilkatHTTP As New Chilkat.Http
        myServletURL.HTML = ""
        Try
            Dim success As Boolean
            success = ChilkatHTTP.UnlockComponent("MZLDADHttp_efwTynJYYR3X")
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error unlocking Chilkat component: " & ex.ToString)
            ' WriteDeviceHistoryEntry("URL", myServletURL.Name, Now.ToString & " Error unlocking component: " & ex.ToString)
        End Try
        Dim myRegistry As New RegistryHandler

        Try

            If myRegistry.ReadFromRegistry("ProxyEnabled") = "True" Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Attempting to route through proxy server.")
                ChilkatHTTP.SocksHostname = myRegistry.ReadFromRegistry("ProxyServer")
                ChilkatHTTP.SocksPort = myRegistry.ReadFromRegistry("ProxyPort")
                ChilkatHTTP.SocksUsername = myRegistry.ReadFromRegistry("ProxyUser")
                ChilkatHTTP.SocksPassword = myRegistry.ReadFromRegistry("ProxyPassword")
            End If
            myRegistry = Nothing
        Catch ex As Exception

        End Try
        ''Get the passwords from the registry
        'Dim MyPass As Byte()
        'Dim mySecrets As New VSFramework.TripleDES

        Dim TravelerServlet As String = "OK"
        Dim strSQL As String
        Dim objVSAdaptor As New VSAdaptor

        Try
            ' ChilkatHTTP.Login = myRegistry.ReadFromRegistry("Domino HTTP User")
            ChilkatHTTP.Login = MyDominoServer.HTTP_UserName
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino HTTP User is " & ChilkatHTTP.Login)
            ChilkatHTTP.Password = MyDominoServer.HTTP_Password

            'MyPass = myRegistry.ReadFromRegistry("Domino HTTP Password")  'Domino password as encrypted byte stream
            'If Not MyPass Is Nothing Then
            '	ChilkatHTTP.Password = mySecrets.Decrypt(MyPass) 'password in clear text, stored in memory now
            'Else
            '	ChilkatHTTP.Password = Nothing
            'End If
            ' WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Domino HTTP password is " & ChilkatHTTP.Password)
        Catch ex As Exception
            'MyPass = Nothing
        End Try

        Try
            If ChilkatHTTP.Login.ToUpper = "TRUE" Or ChilkatHTTP.Login.Trim = "" Then
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The Traveler servlet cannot be checked because the username and password are not configured.  See Configurator, Stored Passwords & Options, Server Credentials.")
                Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.Name, MyDominoServer.Name) _
                                                                                     And repository.Filter.Eq(Function(x) x.Type, MyDominoServer.ServerType)
                Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                     .Set(Function(x) x.TravelerDetails, "The Traveler servlet cannot be checked because the username and password are not configured.  See Configurator, Stored Passwords & Options, Server Credentials.") _
                                                                                     .Set(Function(x) x.TravelerServlet, "Not Checked")
                repository.Update(filterDef, updateDef)
                Exit Sub
            End If
        Catch ex As Exception

        End Try

        Try
            Dim n As Integer
            Do While myServletURL.HTML = ""
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " RequireSSL is set to :" + MyDominoServer.RequireSSL.ToString())
                myServletURL.HTML = ChilkatHTTP.QuickGetStr(IIf(MyDominoServer.RequireSSL, "https://", "http://").ToString() + myServletURL.URL)

                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " HTTP URL IS: " & IIf(MyDominoServer.RequireSSL, "https://", "http://").ToString() + myServletURL.URL)
                n += 1
                Thread.Sleep(500)
                If n > 15 Then Exit Do
            Loop

            If (myServletURL.HTML = "") Then
                MyDominoServer.Description = "Notes Traveler servlet did not respond. "
            Else
                ' WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Servlet response is: " & vbCrLf & myServletURL.HTML)
            End If

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error performing HTTP.Get: " & ex.Message)
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error performing HTTP.Get: " & ex.Message)
            myServletURL.ResponseDetails = ex.Message
        End Try



        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.Name, MyDominoServer.Name) _
                                                                                And repository.Filter.Eq(Function(x) x.Type, MyDominoServer.ServerType)
            Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)

            If InStr(myServletURL.HTML, "Traveler server is available") Or InStr(myServletURL.HTML, "Traveler está disponível") Or InStr(myServletURL.HTML, "ist verfügbar") Or InStr(myServletURL.HTML, "är tillgänglig") Then
                'DRS 10/19/2014 check if JSON is returned
                myAlert.ResetAlert(MyDominoServer.ServerType, MyDominoServer.Name, "Traveler Servlet", MyDominoServer.Location)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The Traveler Servlet is functioning. ")  ', LogLevel.Verbose)

                updateDef = repository.Updater _
                    .Set(Function(x) x.TravelerServlet, "TravelerServlet")

                If MyDominoServer.Traveler_Server_HA = False Then
                    updateDef = updateDef.Set(Function(x) x.TravelerHeartBeat, Now.ToString())
                End If

                repository.Update(filterDef, updateDef)

                Dim n As Integer = 0
                Dim myDevicesData As String = ""
                Do While myDevicesData = ""
                    WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & "Traveler Server Ok, but checking to see if JSON data is returned")
                    myDevicesData = ChilkatHTTP.QuickGetStr(IIf(MyDominoServer.RequireSSL, "https://", "http://").ToString() + MyDominoServer.IPAddress & "/api/traveler/devices?pm=10")

                    'WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " HTTP URL IS: " & IIf(MyDominoServer.RequireSSL, "https://", "http://").ToString() + myServletURL.URL)
                    n += 1
                    Thread.Sleep(500)
                    If n > 15 Then Exit Do
                Loop
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & "My Devices Data:" + myDevicesData)
                Dim sMyDevicesStatus As String = "FAIL"
                If myDevicesData <> "" Then
                    'set the new Devices flag to PASS
                    sMyDevicesStatus = "Pass"
                Else
                    'set the devices flaf to FAIL
                    sMyDevicesStatus = "Fail"
                End If

                updateDef = repository.Updater.Set(Function(x) x.TravelerDevicesAPIStatus, sMyDevicesStatus)
                repository.Update(filterDef, updateDef)

            ElseIf InStr(myServletURL.HTML, "Traveler server is not available") Then

                updateDef = repository.Updater _
                    .Set(Function(x) x.TravelerServlet, TravelerServlet) _
                    .Set(Function(x) x.Details, "IBM Notes Traveler server is not available.") _
                    .Set(Function(x) x.TravelerStatus, "Fail") _
                    .Set(Function(x) x.TravelerDevicesAPIStatus, "Fail") _
                    .Unset(Function(x) x.TravelerStatusReasons)
                repository.Update(filterDef, updateDef)

            Else
                MyDominoServer.Traveler_Status = "Failure"
                myAlert.QueueAlert(MyDominoServer.ServerType, MyDominoServer.Name, "Traveler Servlet", "The Traveler Servlet is not responding.", MyDominoServer.Location)
                WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " The Traveler Servlet is not responding. ", LogLevel.Verbose)
                TravelerServlet = "Not Responding"

                updateDef = repository.Updater _
                    .Set(Function(x) x.TravelerServlet, TravelerServlet) _
                    .Set(Function(x) x.Details, "The Traveler Servlet is not responding.") _
                    .Set(Function(x) x.TravelerStatus, "Fail") _
                    .Set(Function(x) x.TravelerDevicesAPIStatus, "Fail") _
                    .Unset(Function(x) x.TravelerStatusReasons)
                repository.Update(filterDef, updateDef)

            End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Exception updating Traveler servlet: " & ex.ToString)
        End Try


    End Sub

    Private Function getHTMLStreamFromFile(ByVal URL As String, ByRef MyDominoServer As MonitoredItems.DominoServer) As String
        'This function is for Traveler 9+ servers, where the data is only available from a servlet

        WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " Checking the Traveler server's devices list via JSON availablity....", LogLevel.Verbose)

        Dim myServletURL As New MonitoredItems.URL


        WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " Checking if an external alias is defined :" + MyDominoServer.ExternalAlias)
        With myServletURL
            If URL = "" Then
                .URL = IIf(MyDominoServer.ExternalAlias = "", MyDominoServer.IPAddress, MyDominoServer.ExternalAlias) & "/api/traveler/devices?pm=" + pm.ToString()
            Else
                .URL = IIf(MyDominoServer.ExternalAlias = "", MyDominoServer.IPAddress, MyDominoServer.ExternalAlias) & URL.Replace("\/", "/")
            End If
            WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " I figure that the servlet location is " & myServletURL.URL)
        End With



        Try
            Dim n As Integer
            Dim filePath As String = "C:\Program Files (x86)\VitalSignsPlus\TestJSON\" & MyDominoServer.Name.Replace("/", "_") & "_JSON_File.txt"
            Do While myServletURL.HTML = ""
                Try
                    myServletURL.HTML = My.Computer.FileSystem.ReadAllText(filePath)
                Catch ex As Exception

                End Try

                WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " HTTP URL IS: " & IIf(MyDominoServer.RequireSSL, "https://", "http://").ToString() + myServletURL.URL)
                n += 1
                Thread.Sleep(500)
                If n > 15 Then Exit Do
            Loop

            If (myServletURL.HTML = "") Then
                MyDominoServer.Description = "Notes Traveler servlet did not respond. "
            Else
                WriteDeviceHistoryEntry("All", "Traveler_Users_" & MyDominoServer.Name, Now.ToString & " Device Servlet response is: " & vbCrLf & myServletURL.HTML, LogLevel.Verbose)
            End If

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error performing HTTP.Get: " & ex.Message)
            WriteDeviceHistoryEntry("Domino", MyDominoServer.Name, Now.ToString & " Error performing HTTP.Get: " & ex.Message)
        End Try

        Dim TravelerServlet As String = "OK"


        Return myServletURL.HTML
    End Function

    Private Sub SetActiveDevicesLoop()

        Do Until boolTimeToStop

            Try
                WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & ": setting the devices as active.", LogLevel.Verbose)
                SetActiveDevices()
                'Dim strUpdSQL As String = "update dbo.traveler_devices set IsActive=0"
                'objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strUpdSQL)

                'strUpdSQL = "update dbo.traveler_devices set IsActive=1 where ([Traveler_Devices].deviceid + '-' + convert(varchar,id) +'-' +convert(varchar,LastSyncTime,121)) in (select deviceid + '-' +convert(varchar,min(id))+'-'+convert(varchar,max(LastSyncTime),121) from Traveler_Devices group by DeviceID)"

                'WriteDeviceHistoryEntry("All", "Traveler_Users_" & myDominoServer.Name, Now.ToString & " Executing query: " & strUpdSQL, LogLevel.Verbose)
                'objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", strUpdSQL)

            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "Traveler_Users", Now.ToString & " Error setting the devices as active : " & ex.ToString)

            End Try

            Thread.Sleep(60 * 1000 * 1)

        Loop



    End Sub


    <DataContract()> _
    Public Class MyParameters
        <DataMember(Name:="moreInfoSQL")>
        Public Property moreInfoSQL As List(Of System.Data.SqlClient.SqlCommand) = New List(Of System.Data.SqlClient.SqlCommand)
        <DataMember(Name:="dr")>
        Public Property dr As DataRow
        <DataMember(Name:="device")>
        Public Property device As VSNext.Mongo.Entities.MobileDevices
    End Class

    <DataContract()> _
    Public Class TravelerDevice
        <DataMember(Name:="username")> _
        Public Property UserName As String = ""
        <DataMember(Name:="device_provider")> _
        Public Property DeviceName As String = ""

        Public Property ConnectionState As String = ""
        <DataMember(Name:="notification_type")> Public Property AutoSyncType As String = ""
        <DataMember(Name:="device_type")> Public Property OS_Type As String = ""
        <DataMember(Name:="device_level")> Public Property Client_Build As String = ""
        <DataMember(Name:="last_sync_time")> Public Property LastSyncTime As String = ""

        <DataMember(Name:="deviceid")> Public Property DeviceID As String = ""

        <DataMember(Name:="approval_date")> Public Property Approval As String = ""

        <DataMember(Name:="approval_policy")> Public Property ApprovalPolicy As String = ""
        <DataMember(Name:="policy_compliance")> Public Property policy_compliance As String = ""
        <DataMember(Name:="requested_wipe_options")> Public Property WipeOptions As String = ""
        <DataMember(Name:="wipe_request_status")> Public Property WipeStatus As String = ""
        <DataMember(Name:="wipe_types_supported")> Public Property wipeSupported As String = ""

        Public Property ActionDate As String = ""
        <DataMember(Name:="access_status")> Public Property Access As String = ""
        <DataMember(Name:="href")> Public Property href As String = ""


        <DataMember(Name:="approver_ids")> _
        Public Property approver_ids As String = ""

        <DataMember(Name:="sms_address")> _
        Public Property sms_address As String = ""
        <DataMember(Name:="wipe_action_date")> _
        Public Property wipe_action_date As String = ""
        Public Property ServerName As String = ""
        Public Property OS_Type_Min As String = ""
        Public Property DeviceNameRaw As String = ""
    End Class
    <DataContract()> _
    Public Class devices
        <DataMember(Name:="code")> _
        Public Property code As Integer
        <DataMember(Name:="data")> _
        Public Property data As New List(Of TravelerDevice)
        <DataMember(Name:="href")> _
        Public Property href As String
        <DataMember(Name:="message")> _
        Public Property message As String
        'next is a VB key word
        <DataMember(Name:="next")> _
        Public Property next1 As String
        <DataMember(Name:="totalRecords")> _
        Public Property totalRecords As String
    End Class
    <DataContract()> _
    Public Class devicesMoreInfo
        <DataMember(Name:="code")> _
        Public Property code As Integer
        <DataMember(Name:="data")> _
        Public Property data As TravelerDevice
        <DataMember(Name:="href")> _
        Public Property href As String
        <DataMember(Name:="message")> _
        Public Property message As String
        'next is a VB key word
        <DataMember(Name:="next")> _
        Public Property next1 As String
        <DataMember(Name:="totalRecords")> _
        Public Property totalRecords As String
    End Class
    <DataContract()> _
    Public Class travelerServers
        <DataMember(Name:="code")> _
        Public Property code As Integer
        <DataMember(Name:="data")> _
        Public Property data As New List(Of travelerServer)
        <DataMember(Name:="href")> _
        Public Property href As String
        <DataMember(Name:="message")> _
        Public Property message As String
        'next is a VB key word
        <DataMember(Name:="next")> _
        Public Property next1 As String
        <DataMember(Name:="totalRecords")> _
        Public Property totalRecords As String
    End Class
    <DataContract()> _
    Public Class travelerServer
        <DataMember(Name:="availability_index")> _
        Public Property availability_index As String
        <DataMember(Name:="domino_name")> _
        Public Property domino_name As String
        <DataMember(Name:="heart_beat")> _
        Public Property heart_beat As String
        <DataMember(Name:="hostname")> _
        Public Property hostname As String
        'next is a VB key word
        <DataMember(Name:="ip_address")> _
        Public Property ip_address As String
        <DataMember(Name:="number_of_users")> _
        Public Property number_of_users As String
        <DataMember(Name:="port")> _
        Public Property port As Integer
        <DataMember(Name:="server_status")> _
        Public Property server_status As String
    End Class
End Class
