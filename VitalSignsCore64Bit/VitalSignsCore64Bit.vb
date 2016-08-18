Imports System.Management.Automation
Imports System.Management.Automation.Runspaces
Imports System.Management.Automation.Remoting
Imports VSFramework
Imports System.Threading

Imports System.ServiceProcess
Imports System.Configuration
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports MonitoredItems.MonitoredDevice.Alert
Imports System.Net
Imports System.Net.Sockets
Imports System.Xml
Imports System.Globalization
Imports System.Diagnostics
Imports System.Data
Imports System.Collections.Generic
Imports System.Linq

Imports RPRWyatt.VitalSigns.Services
Imports VSNext.Mongo.Repository
Imports VSNext.Mongo.Entities
Imports MongoDB.Driver


Partial Class VitalSignsCore
	Inherits VSServices
    Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("VitalSignsMongo").ToString()
	Dim sCultureString As String = "en-US"
	Dim connectionStringName As String = "CultureString"

	Dim ProductName As String
	Dim myAlert As New AlertLibrary.Alertdll
	Dim boolTimeToStop As Boolean = False

	'Collection of Network Devices
	Dim MyNetworkDevice As MonitoredItems.NetworkDevice
	Dim MyNetworkDevices As New MonitoredItems.NetworkDeviceCollection

	Dim dtNetworkDevicesLastUpdate As DateTime = Now


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


	Private strAppPath As String


	Dim ThreadMainNetworkDevices As New Thread(AddressOf MonitorAllNetworkDevices)


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
			'WriteAuditEntriesThread.CurrentCulture = New CultureInfo(sCultureString)
			'WriteAuditEntriesThread.Start()
		Catch ex As Exception
			EventLog.WriteEntry("VitalSigns Core Services", "Error starting Audit Thread " & ex.ToString(), EventLogEntryType.Error)
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
			'strDateFormat = objDateUtils.GetDateFormat()
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
			'myRegistry.WriteToRegistry("Service Start", Now.ToShortDateString & " " & Now.ToShortTimeString)
			'myRegistry.WriteToRegistry("VS Domino Service Start", Now.ToShortDateString & " " & Now.ToShortTimeString)
			'myRegistry.WriteToRegistry("VS Domino Build Number", BuildNumber)
			'myRegistry.WriteToRegistry("Service Build Number", BuildNumber)
			'myRegistry.WriteToRegistry("Service Reset Date", Now.ToShortDateString)
		Catch ex As Exception
			EventLog.WriteEntry("VitalSigns Core Services", ex.ToString(), EventLogEntryType.Error)
		End Try



		Try
			'WriteAuditEntry(Now.ToString + " ")
			WriteAuditEntry(Now.ToString + " ********************************* ")
			WriteAuditEntry(Now.ToString + " VitalSigns Plus Core Services.")
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
		Dim MyPass As Byte()
		Dim mySecrets As New VSFramework.TripleDES




		Try
			CreateCollections()
		Catch ex As Exception
			EventLog.WriteEntry("VitalSigns Core Services", "Error creating collections " & ex.ToString(), EventLogEntryType.Error)
		End Try



		Try
			InitializeStatusTable()
		Catch ex As Exception
		End Try
		'Sowjanya 1558 ticket
		Try
			myRegistry.WriteToRegistry("Core Service (64 bit) Start", Now.ToShortDateString & " " & Now.ToShortTimeString)


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

		'WriteAuditEntry(Environment.StackTrace.ToString(), LogLevel.Verbose)

		boolTimeToStop = True

		'Try
		'UpdateStatusTable("Update Status SET  Details='URL monitoring is not running.', Status = 'Not Scanned', StatusCode='Maintenance', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0, Description='The VitalSigns monitoring service is not monitoring URLs.' WHERE Type = 'URL' ")
		'UpdateStatusTable("Update Status SET  Details='Mail Service monitoring is not running.',  StatusCode='Maintenance', Status = 'Not Scanned', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0, Description='The VitalSigns monitoring service is not monitoring Mail Services.' WHERE Type like  'Mail%' ")

		'UpdateStatusTable("Update Status SET  Details='Sametime Server monitoring is not running.', StatusCode='Maintenance', Status = 'Not Scanned', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0 WHERE Type like 'Sametime%' ")
		'UpdateStatusTable("Update Status SET  Details='Network Device monitoring is not running.',   StatusCode='Maintenance', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0 WHERE Type like 'Network%' ")
		'UpdateStatusTable("Update Status SET  Details='The URL monitoring service is not running.', Status='Not Scanned', StatusCode='Maintenance', PendingMail=0, DeadMail=0, HeldMail=0, UserCount=0, CPU=0, MyPercent=0, ResponseTime=0, Memory=0, Description='URL monitoring is not running.' WHERE Type ='URL'")

		'Catch ex As Exception

		'End Try
		'End
		MyBase.OnStop()
	End Sub


#Region "Collections"

	Public Sub CreateCollections()

		Try
			CreateNetworkDevicesCollection()
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Error creating Network Devices collection: " & ex.Message)
		End Try

	End Sub

	Private Sub CreateNetworkDevicesCollection()

		'Connect to the data source
		Dim dsNetworkDevices As New Data.DataSet

		Try
			Dim strSQL As String = "SELECT nd.ID, nd.Address, nd.Category, nd.Description, nd.Enabled, nd.LastChecked, nd.LastStatus, nd.[Next Scan], nd.OffHoursScanInterval, nd.[Password], " & _
			 "nd.Port, nd.[Scanning Interval], nd.Username, nd.Location, nd.Name, nd.ResponseThreshold, nd.RetryInterval, nd.NetworkType, st.LastUpdate, st.Status, st.StatusCode, di.CurrentNodeID FROM [Network Devices] nd "

			If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then
				Dim nodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
				strSQL += "inner join DeviceInventory di on nd.ID=di.DeviceID and di.DeviceTypeId=8 " & _
				 "inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" & nodeName & "' "

			End If
			strSQL += " left outer join Status st on st.Type=(select ServerType From ServerTypes where id=8) and st.Name=nd.Name "
			strSQL += " where nd.Enabled=1"
			Dim objVSAdaptor As New VSAdaptor
			objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsNetworkDevices, "Network_Devices")
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateNetworkDevicesCollection processing code. Exception: " & ex.Message)
			'Exit Sub
		End Try

		'***

		Dim i As Integer = 0
		WriteAuditEntry(Now.ToString & "  Reading configuration settings for Network Devices.")
		'Add the network Devices to the collection

		Try
			Dim myString As String = ""
			Dim dr As DataRow
			For Each dr In dsNetworkDevices.Tables("Network_Devices").Rows()
				i += 1
				Dim MyName As String
				If dr.Item("Name") Is Nothing Then
					MyName = "NetworkDevice" & i.ToString
				Else
					MyName = dr.Item("Name")
				End If
				'See if this server is already in the collection; if so, update its settings otherwise create a new one
				MyNetworkDevice = MyNetworkDevices.Search(MyName)
				If MyNetworkDevice Is Nothing Then
					MyNetworkDevice = New MonitoredItems.NetworkDevice(MyName)
					'MyNetworkDevice.LastScan = Now
					MyNetworkDevice.NextScan = Now
					MyNetworkDevice.AlertCondition = False
					'MyNetworkDevice.Status = "Not Scanned"
					MyNetworkDevice.IncrementUpCount()
					MyNetworkDevices.Add(MyNetworkDevice)
					If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Adding new network device-- " & MyNetworkDevice.Name & " -- to the collection.")
				Else
					If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Updating settings for existing network device-- " & MyNetworkDevice.Name & ".")
				End If


				With MyNetworkDevice
					If MyLogLevel = LogLevel.Verbose Then
						WriteAuditEntry(Now.ToString & " Configuring Network Device: " & dr.Item("Name"))
						WriteAuditEntry(Now.ToString & " Status: " & MyNetworkDevice.Status)
						WriteAuditEntry(Now.ToString & " Enabled: " & MyNetworkDevice.Enabled)
						WriteAuditEntry(Now.ToString & " Next Scan: " & MyNetworkDevice.NextScan)
					End If

					Try
						.OffHours = False
					Catch ex As Exception
						.OffHours = False
					End Try

					Try
						If dr.Item("Address") Is Nothing Then
							.IPAddress = ""
						Else
							.IPAddress = dr.Item("Address")
						End If
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " Invalid IP Address")
						.IPAddress = ""
					End Try

					Try
						If dr.Item("Enabled") Is Nothing Then
							.Enabled = True
						Else
							.Enabled = dr.Item("Enabled")
						End If

					Catch ex As Exception
						.Enabled = True
					End Try

					If .Enabled = False Then
						.Status = "Disabled"
					End If

					If .Enabled = True And .Status = "Disabled" Then
						.Status = "Not Scanned"
					End If

					Try
						If dr.Item("Description") Is Nothing Then
							.Description = ""
						Else
							.Description = dr.Item("Description")
						End If
					Catch ex As Exception
						.Description = ""
					End Try

					Try
						If dr.Item("ResponseThreshold") Is Nothing Then
							.ResponseThreshold = 100
						Else
							.ResponseThreshold = dr.Item("ResponseThreshold")
						End If
					Catch ex As Exception
						.ResponseThreshold = 100
					End Try


					Try
						If dr.Item("Scanning Interval") Is Nothing Then
							.ScanInterval = 10
						Else
							.ScanInterval = dr.Item("Scanning Interval")
						End If
					Catch ex As Exception
						.ScanInterval = 10
					End Try

					Try
						If dr.Item("OffHoursScanInterval") Is Nothing Then
							.OffHoursScanInterval = 30
						Else
							.OffHoursScanInterval = dr.Item("OffHoursScanInterval")
						End If
					Catch ex As Exception
						.OffHoursScanInterval = 30
					End Try
					Try
						'   WriteAuditEntry(Now.ToString & "Adding Category")
						If dr.Item("Category") Is Nothing Then
							.Category = "Not Categorized"
						Else
							.Category = dr.Item("Category")
						End If
					Catch ex As Exception
						.Category = "Not Categorized"
					End Try

					Try
						'WriteAuditEntry(Now.ToString & "Adding Location")
						If dr.Item("Location") Is Nothing Then
							.Location = "None"
						Else
							.Location = dr.Item("Location")
						End If

					Catch ex As Exception
						.Location = "None"
					End Try

					Try
						If dr.Item("RetryInterval") Is Nothing Then
							.RetryInterval = 2
							WriteAuditEntry(Now.ToString & " " & .Name & " Network Device retry scan interval not set, using default of 10 minutes.")
						Else
							.RetryInterval = dr.Item("RetryInterval")
						End If
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " " & .Name & " Network Device retry scan interval not set, using default of 10 minutes.")
						.RetryInterval = 2
					End Try

					Try
						If dr.Item("ID") Is Nothing Then
							.ID = -1
						Else
							.ID = dr.Item("ID")
						End If
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " " & .Name & " Network Device retry scan interval not set, using default of 10 minutes.")
						.RetryInterval = 2
					End Try

					Try
						If dr.Item("NetworkType") Is Nothing Then
							.NetworkType = ""
						Else
							.NetworkType = dr.Item("NetworkType")
						End If
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " " & .Name & " Network Device NetworkType not set.")

					End Try

					Try
						If dr.Item("Username") Is Nothing Then
							.UserName = ""
						Else
							.UserName = dr.Item("Username")
						End If
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " " & .Name & " Network Device Username not set.")

					End Try

					Try
						If dr.Item("NetworkType") Is Nothing Then
							.NetworkType = ""
						Else
							.NetworkType = dr.Item("NetworkType")
						End If
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " " & .Name & " Network Device NetworkType not set.")

					End Try

					Try
						If dr.Item("Password") Is Nothing Then
							.NetworkType = ""
						Else

							Dim strEncryptedPassword As String = dr.Item("Password").ToString()
							Dim Password As String
							Dim myPass As Byte()


							Try
								Dim strValue As Object
								Dim str1() As String
								str1 = strEncryptedPassword.Split(",")
								Dim bstr1(str1.Length - 1) As Byte
								For j As Integer = 0 To str1.Length - 1
									bstr1(j) = str1(j).ToString()
								Next
								myPass = bstr1
							Catch ex As Exception

							End Try


							Dim mySecrets As New VSFramework.TripleDES
							Try
								If Not strEncryptedPassword Is Nothing Then
									Password = mySecrets.Decrypt(myPass) 'password in clear text, stored in memory now
									' If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Successfully decrypted the Notes password as " & MyDominoPassword)
									' WriteAuditEntry(Now.ToString & " HTTP password is " & Password, LogLevel.Verbose)
								Else
									Password = Nothing
								End If
							Catch ex As Exception
								Password = ""
								WriteAuditEntry(Now.ToString & " Error decrypting the Notes password.  " & ex.ToString)
							End Try
							.Password = Password

						End If
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " " & .Name & " Network Device NetworkType not set.")

					End Try


					Try
						If dr.Item("LastUpdate") Is Nothing Then
							.LastScan = Now
						Else
							.LastScan = dr.Item("LastUpdate")
						End If
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " " & .Name & " Network Device LastScan not set.")

					End Try

					Try
						If dr.Item("Status") Is Nothing Then
							.Status = "Not Scanned"
						Else
							.Status = dr.Item("Status")
						End If
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " " & .Name & " Network Device Status not set.")

					End Try

					Try
						If dr.Item("CurrentNodeID") Is Nothing Then
							.InsufficentLicenses = True
						Else
							If dr.Item("CurrentNodeID").ToString() = "-1" Then
								.InsufficentLicenses = True
							Else
								.InsufficentLicenses = False
							End If

						End If
                    Catch ex As Exception
                        '7/8/2015 NS modified for VSPLUS-1959
                        WriteAuditEntry(Now.ToString & " " & .Name & " Network Device Insufficient Licenses not set.")

					End Try

				End With

				MyNetworkDevice = Nothing
			Next
			dr = Nothing
		Catch exception As DataException
			WriteAuditEntry(Now.ToString & " Network Devices data exception" & exception.Message)
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Network Devices " & ex.ToString)
		End Try
		dsNetworkDevices.Dispose()

		InsufficentLicensesTest(MyNetworkDevices)

	End Sub

	Public Sub InsufficentLicensesTest(ByRef coll As MonitoredItems.MonitoredDevicesCollection)

		If (coll.Count = 0) Then
			Return
		End If

		Dim ServerType As String = ""
		Dim ServerTypeForTypeAndName As String = ""
		Select Case coll.GetType()
			Case GetType(MonitoredItems.NetworkDeviceCollection)
                ServerType = "Network Device"
                '5/5/2016 NS modified
                'Changed ND to Network Device
                ServerTypeForTypeAndName = "Network Device"

		End Select

        CheckForInsufficentLicenses(coll, "Network Device", "Network Device")


	End Sub

#End Region


#Region "Inits"

	Public Sub InitializeStatusTable()

		Try
			WriteAuditEntry(Now.ToString & " Inserting " & MyNetworkDevices.Count & " monitored Network Devices into the status table.")
			If MyNetworkDevices.Count > 0 Then
				UpdateStatusTableWithNetworkDevices()
			End If
		Catch ex As Exception

		End Try

	End Sub

	Protected Sub StartThreads()

		Dim myRegistry As New VSFramework.RegistryHandler()
		'**** Start the selected monitoring threads

		Try
			'Start monitoring Network Devices
			WriteAuditEntry(Now.ToString & " Starting Network Devices monitor thread...")
			ThreadMainNetworkDevices.IsBackground = True
			ThreadMainNetworkDevices.Priority = ThreadPriority.Normal
			ThreadMainNetworkDevices.CurrentCulture = New CultureInfo(sCultureString)
			ThreadMainNetworkDevices.Start()
		Catch ex As Exception

		End Try

		Try
			Dim MonitorCoreTableChanges As New Thread(AddressOf CheckForCoreTableChanges)
			MonitorCoreTableChanges.CurrentCulture = New CultureInfo(sCultureString)
			MonitorCoreTableChanges.Start()
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Error starting thread to check for table updates")
		End Try


	End Sub



	Protected Sub CheckForCoreTableChanges()


		Dim myRegistry As New VSFramework.RegistryHandler
		Dim NodeName As String = ""
		Dim flags As New MonitoredItems.ServicesFlags()
		'********** Refresh Collection of Devices and Monitor settings

		Do Until boolTimeToStop = True
			If (NodeName = "") Then
				If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then
					NodeName = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
				End If
			End If
			If NodeName <> "" Then
				Try
					'Update the settings anytime the registry indicates a change has been made
					If flags.UpdateServiceCollection(MonitoredItems.ServerTypes.Network_Devices, NodeName) Then
						WriteAuditEntry(Now.ToString & " Refreshing configuration of Network Devices")
						CreateNetworkDevicesCollection()
						WriteAuditEntry(Now.ToString & " Refreshing Status Table for Network Devices")
						UpdateStatusTableWithNetworkDevices()
					End If
				Catch ex As Exception

				End Try
			Else
				WriteAuditEntry(Now.ToString & " No Node Name is specified.  Will not do automatic updating until a name is given.")
			End If

			Thread.Sleep(2000)
		Loop
	End Sub

	Public Function ServerStatusCode(ByVal Status As String) As String

		Dim StatusCode As String = "OK"
		Try
			Select Case Status.Trim
				Case "OK"
					StatusCode = "OK"
				Case "Scanning"
					StatusCode = "OK"
				Case "Maintenance"
					StatusCode = "Maintenance"
				Case "Not Responding"
					StatusCode = "Not Responding"
				Case "Not Scanned"
					StatusCode = "Maintenance"
				Case "Disabled"
					StatusCode = vbNull
				Case "Insufficient Licenses"
					StatusCode = "Issue"
				Case "Timeout"
					StatusCode = "Not Responding"
				Case Else
					StatusCode = "Issue"
			End Select
		Catch ex As Exception
			StatusCode = vbNull
		End Try

		Return StatusCode
	End Function

	Protected Sub MonitorAllNetworkDevices()
		Dim elapsed As TimeSpan
		Dim start, done As Long
		start = Now.Ticks
		Dim myRegistry As New RegistryHandler

		Do
			WriteAuditEntry(Now.ToString & " Creating new Network Device monitoring thread.")
			Dim threadMonitorDevices As New Thread(AddressOf MonitorNetworkDevices)
			Try
				threadMonitorDevices.CurrentCulture = New CultureInfo(sCultureString)
				threadMonitorDevices.Start()
				Thread.Sleep(4000) 'sleep 4 seconds to give thread a chance to start
				Do
					Dim myLastUpdate As TimeSpan

					Try

						myLastUpdate = dtNetworkDevicesLastUpdate.Subtract(Now)
						'   WriteAuditEntry(Now.ToString & " ND thread last updated " & myLastUpdate.TotalSeconds & " seconds ago.")
						If myLastUpdate.TotalSeconds < -890 Then
							'The ND Thread hasn't looped in 90 seconds
							WriteAuditEntry(Now.ToString & " Destroying Network Devices thread because it hasn't updated in " & myLastUpdate.TotalSeconds & " seconds. ")
							threadMonitorDevices.Abort()
							threadMonitorDevices.Join(5000)
							dtNetworkDevicesLastUpdate = Now
							threadMonitorDevices = Nothing
							Exit Do
						End If

						done = Now.Ticks
						elapsed = New TimeSpan(done - start)

						If elapsed.TotalMinutes > 5 Or myRegistry.ReadFromRegistry("Network Device Update") = True Then
							start = Now.Ticks
							WriteAuditEntry(Now.ToString & " Refreshing configuration of Network Devices")
							Try
								threadMonitorDevices.Suspend()
								CreateNetworkDevicesCollection()
								UpdateStatusTableWithNetworkDevices()
								myRegistry.WriteToRegistry("Network Device Update", False)
							Catch ex As Exception

							End Try

							Try
								threadMonitorDevices.Resume()
							Catch ex As Exception

							End Try

						End If


					Catch ex As ThreadAbortException
						WriteAuditEntry(Now.ToString & " Destroying Network Devices to shut down service. ")
						threadMonitorDevices.Abort()
						threadMonitorDevices.Join(5000)
						threadMonitorDevices = Nothing
						Exit Sub
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " Network Device Thread error: " & ex.Message)
					End Try
					Thread.Sleep(29000)
				Loop
			Catch ex As Exception
				WriteAuditEntry(Now.ToString & " Network Device Thread error: " & ex.Message)
			End Try


		Loop

	End Sub


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

#Region "StatusTableStuff"

	Private Sub UpdateNDStatisticsTable(ByVal DeviceName As String, ByVal ResponseTime As Long)

		Dim objVSAdaptor As New VSAdaptor

		Dim strSQL As String
		Dim MyWeekNumber As Integer
		MyWeekNumber = GetWeekNumber(Date.Today)
		Try
            'strSQL = "INSERT INTO DeviceDailyStats (DeviceType, ServerName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber)" & _
            '   " VALUES ('Network Device', '" & DeviceName & "', '" & Now.ToString & "', '" & "ResponseTime" & "', '" & ResponseTime & "', '" & MyWeekNumber & "', '" & Now.Month & "', '" & Now.Year & "', '" & Now.Day & "')"
			'If boolUseSQLServer = True Then
			'    ExecuteNonQuerySQL_VSS_Statistics(strSQL)
			'Else
			'    myCommand.CommandText = strSQL
			'    myCommand.ExecuteNonQuery()
			'End If

            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
            Dim DailyStats As New DailyStatistics
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
            DailyStats.DeviceType = VSNext.Mongo.Entities.Enums.ServerType.URL.ToDescription()
            DailyStats.ServerName = DeviceName
            DailyStats.StatName = "ResponseTime"
            DailyStats.StatValue = ResponseTime
            repo.Insert(DailyStats)
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Network Device Stats table insert failed becase: " & ex.Message)
			WriteAuditEntry(Now.ToString & " The failed stats table insert comand was " & strSQL)
		Finally
			'myConnection.Close()

		End Try

		'myConnection.Dispose()
		'myCommand.Dispose()
	End Sub

	Public Sub UpdateStatusTable(ByVal SQLUpdateStatement As String, Optional ByVal SQLInsertStatement As String = "", Optional ByVal Comment As String = "")
		'     WriteAuditEntry(Now.ToString + " ********************* Common STATUS UPDATE ******************** ")
		'If Comment <> "" Then
		'    WriteAuditEntry(Now.ToString + " *********************")
		'    WriteAuditEntry(Now.ToString + " " & Comment)
		'    WriteAuditEntry(Now.ToString + " *********************")
		'End If
		''This routine is used to update the status table with the results and a device scan.  First it tries to 
		'do an update.  if that fails and an insert command is sent, it will try that.
		Dim RA As Integer
		Dim objVSAdaptor As New VSAdaptor
		If objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", SQLUpdateStatement) = False And SQLInsertStatement <> "" Then
			objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "Status", SQLInsertStatement)
		End If
		'If boolUseSQLServer = True Then
		'    Try
		'        If ExecuteNonQueryUsingVitalSignsDB(SQLUpdateStatement) = 0 And SQLInsertStatement <> "" Then
		'            ExecuteNonQueryUsingVitalSignsDB(SQLInsertStatement)
		'        End If
		'    Catch ex As Exception

		'    End Try

		'    'WriteAuditEntry(Now.ToString + " Using SQL Server. My connection is " & SqlConnectionVitalSigns.State.ToString)
		'    'Try
		'    '    Dim myCommand As New Data.SqlClient.SqlCommand
		'    '    myCommand.Connection = SqlConnectionVitalSigns
		'    '    myCommand.CommandText = SQLUpdateStatement
		'    '    If SqlConnectionVitalSigns.State <> ConnectionState.Open Then
		'    '        SqlConnectionVitalSigns.Open()
		'    '    End If
		'    '    If myCommand.ExecuteNonQuery = 0 And SQLInsertStatement <> "" Then
		'    '        myCommand.CommandText = SQLInsertStatement
		'    '        myCommand.ExecuteNonQuery()
		'    '    End If

		'    '    myCommand.Dispose()
		'    'Catch ex As Exception
		'    '    WriteAuditEntry(Now.ToString + " ********************* Using SQL Server exception : " & ex.ToString)
		'    '    WriteAuditEntry(Now.ToString + " ********************* SQL Update : " & SQLUpdateStatement & vbCrLf)
		'    '    WriteAuditEntry(Now.ToString + " ********************* SQL Insert : " & SQLInsertStatement & vbCrLf)
		'    'End Try

		'Else
		'    ' WriteAuditEntry(Now.ToString + " Using Access ")
		'    Dim myPath As String = ""
		'    Dim myRegistry As New RegistryHandler
		'    'Read the registry for the location of the Config Database

		'    Try
		'        myPath = myRegistry.ReadFromRegistry("Status Path")
		'        ' WriteAuditEntry(Now.ToString & " Domino Update e database " & myPath)
		'    Catch ex As Exception
		'        WriteAuditEntry(Now.ToString & " Failed to read registry in common Status table module. Exception: " & ex.Message)
		'    End Try

		'    If myPath Is Nothing Then
		'        WriteAuditEntry(Now.ToString & " Error: Failed to read registry in common Status table module.   Cannot locate Config Database 'status.mdb'.  Configure by running" & ProductName & " client before starting the service.")
		'        '   Return False
		'        Exit Sub
		'    End If
		'    myRegistry = Nothing

		'    Dim myCommand As New OleDb.OleDbCommand
		'    Dim myConnection As New OleDb.OleDbConnection
		'    'Dim myAdapter As New OleDb.OleDbDataAdapter

		'    With myConnection
		'        .ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & myPath
		'        .Open()
		'    End With

		'    Do Until myConnection.State = ConnectionState.Open
		'        myConnection.Open()
		'    Loop
		'    '   WriteAuditEntry(Now.ToString & " The Access database Status is " & myConnection.State.ToString)
		'    '***

		'    'Save it to the Access database
		'    If myConnection.State = ConnectionState.Open Then
		'        myCommand.CommandText = SQLUpdateStatement
		'        myCommand.Connection = myConnection
		'        Try
		'            RA = 0
		'            RA = myCommand.ExecuteNonQuery
		'        Catch ex As Exception
		'            '  WriteAuditEntry(Now.ToString & " Exception processing status table: " & ex.ToString & vbCrLf & SQLUpdateStatement)
		'            RA = 0
		'        End Try


		'        If RA = 0 Then
		'            'server wasn't in the status table, perhaps because it was previously disabled
		'            '   WriteAuditEntry(Now.ToString & " Update Failed because it affected 0 records with " & SQLUpdateStatement)
		'            If SQLInsertStatement <> "" Then
		'                Try
		'                    myCommand.CommandText = SQLInsertStatement
		'                    myCommand.ExecuteNonQuery()
		'                Catch ex As Exception
		'                    '      WriteAuditEntry(Now.ToString & " Insert Statement error: " & ex.ToString)
		'                End Try

		'            End If

		'            '   WriteAuditEntry(Now.ToString & " Update Failed because it affected 0 records with " & SQLInsertStatement)
		'            '  OleDbDataAdapterStatus.InsertCommand.CommandText = strSQL
		'            ' OleDbDataAdapterStatus.InsertCommand.ExecuteNonQuery()
		'        Else
		'            '      WriteAuditEntry(Now.ToString & " Updated " & RA.ToString & "  record.")
		'        End If
		'    End If

		'    Try
		'        myConnection.Close()
		'        myCommand.Dispose()
		'        myConnection.Dispose()

		'    Catch ex As Exception

		'    End Try
		'End If

	End Sub

	Private Sub UpdateStatusTableWithNetworkDevices()

		Dim strSQL As String
		Dim objVSAdaptor As New VSAdaptor

        Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
        Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status)
        Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)
		strSQL = ""
		Try
			Dim n As Integer
			Dim myStatus As String = ""
			Dim MyNetworkDevice As MonitoredItems.MonitoredDevice
			For n = 0 To MyNetworkDevices.Count - 1
				MyNetworkDevice = MyNetworkDevices.Item(n)
				'   WriteAuditEntry(Now.ToString & " Adding " & MyNetworkDevice.Name & " to status table.")
                myStatus = ServerStatusCode(MyNetworkDevice.Status)
                Try

                    With MyNetworkDevice
                        '5/5/2016 NS modified - inserting a Network Device into StatusDetails fails because of a TypeANDName mismatch
                        'Changed TypeANDName -ND to -Network Device
                        'strSQL = "IF NOT EXISTS(SELECT * FROM Status WHERE TypeANDName = '" + .Name + "-Network Device') BEGIN " & _
                        ' "INSERT INTO Status(StatusCode, Category,  Description, Details, DownCount,  Location, Name,  Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime, TypeANDName, Icon) VALUES " & _
                        ' "('" & myStatus & "', '" & .Category & "', '" & .Description & "', ' ', '" & .DownCount & "', '" & .Location & "', '" & .Name & "', '" & .Status & "', 'Network Device', '" & .UpCount & "', '" & .UpPercentCount & "', '" & Now & "', '0', '" & .Name & "-Network Device', " & IconList.Network_Device & ")" & _
                        ' "END"
                        Dim TypeAndName As String = .Name & "-" & .ServerType
                        filterdef = repo.Filter.Where(Function(i) i.TypeAndName.Equals(TypeAndName))
                        updatedef = repo.Updater _
                                                  .Set(Function(i) i.Name, .Name) _
                                                  .[Set](Function(i) i.CurrentStatus, .Status) _
                                                  .[Set](Function(i) i.StatusCode, myStatus) _
                                                  .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                                                  .[Set](Function(i) i.Category, .Category) _
                                                  .[Set](Function(i) i.TypeAndName, TypeAndName) _
                                                  .[Set](Function(i) i.Description, .Description) _
                                                  .[Set](Function(i) i.Type, .ServerType) _
                                                  .[Set](Function(i) i.Location, .Location) _
                                                  .[Set](Function(i) i.UpCount, Integer.Parse(.UpCount)) _
                                                  .[Set](Function(i) i.UpPercent, Integer.Parse(.UpPercentCount)) _
                                                  .[Set](Function(i) i.LastUpdated, Now) _
                                                  .[Set](Function(i) i.ResponseTime, 0)
                    End With

                    repo.Upsert(filterdef, updatedef)
                Catch ex As Exception

                End Try
            Next n
			n = Nothing
			MyNetworkDevice = Nothing
		Catch ex As Exception
			WriteAuditEntry("Failure updating status table with Network Device info: " & ex.Message)
			WriteAuditEntry(Now.ToString & " Insert command was " & strSQL)
			WriteAuditEntry(Now.ToString & " Service stopped.")
		Finally
			strSQL = Nothing
			'myConnection.Close()
		End Try
		'myConnection.Dispose()

	End Sub

	Private Sub UpdateStatusTableWithNetworkDevice(ByRef myDevice As MonitoredItems.NetworkDevice)

		Dim strSQLUpdate As String = ""
		Dim strSQLInsert As String = ""

		Dim objVSAdaptor As New VSAdaptor
        Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
        Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status)
        Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)
		Try

			Dim PercentageChange As Double
			Dim Percent As Double
			Try
				If myDevice.PreviousKeyValue > 0 And myDevice.ResponseTime > 0 Then
					PercentageChange = -(1 - myDevice.PreviousKeyValue / myDevice.ResponseTime)
				Else
					PercentageChange = 0
				End If

			Catch ex As Exception
				PercentageChange = 0
			End Try

			Try
				Percent = myDevice.ResponseTime / myDevice.ResponseThreshold * 100
			Catch ex As Exception
				Percent = 0
			End Try

			Try
                With myDevice
                    '5/5/2016 NS modified - inserting a Network Device into StatusDetails fails because of a TypeANDName mismatch
                    'Changed TypeANDName -ND to -Network Device
                    'strSQLUpdate = "Update Status SET DownCount= '" & myDevice.DownCount & _
                    '   "', Status='" & myDevice.Status & "', Upcount=" & myDevice.UpCount & _
                    '  ", UpPercent= '" & myDevice.UpPercentCount & _
                    '  "', Details='" & myDevice.ResponseDetails & _
                    '  "', LastUpdate='" & Now & _
                    '  "', ResponseTime='" & Str(myDevice.ResponseTime) & _
                    '  "', PercentageChange='" & Str(PercentageChange) & _
                    ' "', StatusCode='" & .StatusCode & _
                    '  "', NextScan='" & myDevice.NextScan & _
                    '  "', ResponseThreshold=" & myDevice.ResponseThreshold & _
                    '  ", MyPercent='" & (Percent) & _
                    '  "', UpMinutes=" & Microsoft.VisualBasic.Strings.Format(myDevice.UpMinutes, "F1") & _
                    '  ", DownMinutes=" & Microsoft.VisualBasic.Strings.Format(myDevice.DownMinutes, "F1") & _
                    '  ", UpPercentMinutes='" & Str(myDevice.UpPercentMinutes) & _
                    '  "', Name='" & myDevice.Name & "' " & _
                    '  ", Location='" & myDevice.Location & "' " & _
                    '  " WHERE TypeANDName='" & myDevice.Name & "-Network Device'"
                    Dim TypeAndName As String = .Name & "-" & .ServerType
                    filterdef = repo.Filter.Where(Function(i) i.TypeAndName.Equals(TypeAndName))
                    updatedef = repo.Updater _
                    .Set(Function(i) i.Name, .Name) _
                    .[Set](Function(i) i.CurrentStatus, .Status) _
                    .[Set](Function(i) i.StatusCode, .StatusCode) _
                    .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                    .[Set](Function(i) i.Category, .Category) _
                    .[Set](Function(i) i.TypeAndName, TypeAndName) _
                    .[Set](Function(i) i.Description, .Description) _
                    .[Set](Function(i) i.Type, .ServerType) _
                    .[Set](Function(i) i.Location, .Location) _
                    .[Set](Function(i) i.UpCount, Integer.Parse(.UpCount)) _
                    .[Set](Function(i) i.UpPercent, Integer.Parse(.UpPercentCount)) _
                    .[Set](Function(i) i.LastUpdated, Now) _
                    .[Set](Function(i) i.MyPercent, Percent) _
                    .[Set](Function(i) i.UpMinutes, Double.Parse(Microsoft.VisualBasic.Strings.Format(myDevice.UpMinutes, "F1"))) _
                    .[Set](Function(i) i.DownMinutes, Double.Parse(Microsoft.VisualBasic.Strings.Format(myDevice.DownMinutes, "F1"))) _
                    .[Set](Function(i) i.UpPercentMinutes, myDevice.UpPercentMinutes) _
                    .[Set](Function(i) i.ResponseTime, 0) _
                    .[Set](Function(i) i.ResponseThreshold, Convert.ToInt32(.ResponseThreshold))


                End With
                repo.Upsert(filterdef, updatedef)
			Catch ex As Exception
				strSQLUpdate = ""
			End Try

            'Try
            '             With myDevice
            '                 '5/5/2016 NS modified - inserting a Network Device into StatusDetails fails because of a TypeANDName mismatch
            '                 'Changed TypeANDName -ND to -Network Device
            '                 strSQLInsert = "INSERT INTO Status(StatusCode, Category,  Description, Details, DownCount,  Location, Name,  Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime, TypeANDName, Icon, " & _
            '                  "PercentageChange, NextScan, ResponseThreshold, MyPercent, UpMinutes, DownMinutes, UpPercentMinutes) VALUES " & _
            '                   "('" & .Status & "', '" & .Category & "', '" & .Description & "', '" & .ResponseDetails & "', '" & .DownCount & "', '" & .Location & "', '" & .Name & "', '" & .Status & "', " & _
            '                   "'Network Device', '" & .UpCount & "', '" & .UpPercentCount & "', '" & Now & "', '" & Str(.ResponseTime) & "', '" & .Name & "-Network Device', " & IconList.Network_Device & ", " & _
            '                   "'" & Str(PercentageChange) & "', '" & .NextScan & "', '" & .ResponseThreshold & "', '" & Percent & "', " & Microsoft.VisualBasic.Strings.Format(myDevice.UpMinutes, "F1") & ", " & _
            '                   "" & Microsoft.VisualBasic.Strings.Format(myDevice.DownMinutes, "F1") & ", '" & Str(.UpPercentMinutes) & "')"

            '             End With
            'Catch ex As Exception
            '	strSQLUpdate = ""
            'End Try

            'UpdateStatusTable(strSQLUpdate, SQLInsertStatement:=strSQLInsert)
		Catch ex As Exception
			WriteAuditEntry("Failure updating status table with Network Device info: " & ex.Message)
			WriteAuditEntry(Now.ToString & " Update command was " & strSQLUpdate)
			WriteAuditEntry(Now.ToString & " Insert command was " & strSQLUpdate)
			WriteAuditEntry(Now.ToString & " Service stopped.")
		Finally
			strSQLInsert = Nothing
			strSQLUpdate = Nothing
			'myConnection.Close()
		End Try
		'myConnection.Dispose()

	End Sub


#End Region


#Region "Logs"


	Private Overloads Sub WriteAuditEntry(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
		MyBase.WriteAuditEntry(strMsg, "VSNetworkDevices.txt", LogLevelInput)
	End Sub


	Private Overloads Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
		MyBase.WriteDeviceHistoryEntry(DeviceType, DeviceName, strMsg, LogLevelInput)
	End Sub



	'Private Sub WriteAuditEntry(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
	'	Dim strAuditText As String = ""
	'	If LogLevelInput >= MyLogLevel Then
	'		strAuditText += strMsg & vbCrLf
	'	End If

	'	Dim strLogDest As String
	'	Try
	'		strLogDest = AppDomain.CurrentDomain.BaseDirectory.ToString & "\Log_Files\VSNetworkDevices.txt"
	'	Catch ex As Exception
	'		strLogDest = "d:\vitalsigns\data\VSNetworkDevices.txt"
	'	End Try


	'	'Try
	'	'    ' strLogDest = myRegistry.ReadFromRegistry("History Path")
	'	'    If File.Exists(strLogDest) Then
	'	'        File.Delete(strLogDest)
	'	'    End If
	'	'Catch ex As Exception
	'	'    strLogDest = "c:\VSCore.txt"

	'	'End Try

	'	' myRegistry = Nothing

	'	'Dim elapsed As TimeSpan
	'	Dim OneHour As Integer = 60 * 60 * -1  'seconds
	'	Dim dtAuditHistory As DateTime

	'	Dim myLastUpdate As TimeSpan

	'	Try
	'		dtAuditHistory = Now
	'		Thread.CurrentThread.Sleep(1000)
	'	Catch ex As Exception

	'	End Try


	'	Try


	'		Dim sw As StreamWriter
	'		Try
	'			If File.Exists(strLogDest) Then
	'				sw = New StreamWriter(strLogDest, True, System.Text.Encoding.Unicode)
	'			Else
	'				sw = New StreamWriter(strLogDest, False, System.Text.Encoding.Unicode)
	'				'  sw = File.CreateText(strLogDest)
	'			End If
	'		Catch ex As Exception

	'		End Try


	'		Try
	'			If strAuditText <> "" Then

	'				sw.WriteLine(strAuditText)
	'				strAuditText = ""
	'				sw.Flush()
	'				sw.Close()
	'			End If

	'		Catch ex As Exception

	'		End Try

	'		Try
	'			sw.Close()
	'		Catch ex As Exception

	'		End Try
	'		sw = Nothing

	'		Thread.CurrentThread.Sleep(4250)

	'		Try
	'			myLastUpdate = dtAuditHistory.Subtract(Now)
	'		Catch ExAbort As ThreadAbortException

	'		Catch ex As Exception

	'		End Try




	'	Catch ExAbort As ThreadAbortException
	'		'      sw.Write(" Shutting down, closing log")
	'		'     sw.Close()
	'		'    sw = Nothing
	'	Catch ex As Exception

	'	End Try


	'End Sub


	'Private Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Normal)
	'	Dim DeviceLogDestination As String
	'	Dim appendMode As Boolean = True
	'	'   If Left(strAppPath, 1) = "\" Then
	'	'DeviceLogDestination = strAppPath & "Data\Logfiles\" & DeviceType & "_" & DeviceName & "_Log.txt"
	'	'  Else
	'	'    DeviceLogDestination = strAppPath & "\Data\Logfiles\" & DeviceType & "_" & DeviceName & "_Log.txt"
	'	'    End If

	'	If (LogLevelInput < MyLogLevel) Then
	'		Return
	'	End If

	'	Try
	'		DeviceLogDestination = AppDomain.CurrentDomain.BaseDirectory.ToString & "\Log_Files\" & DeviceType & "_" & DeviceName & "_Log.txt"
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


End Class
