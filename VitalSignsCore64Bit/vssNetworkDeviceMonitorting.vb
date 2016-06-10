Imports VSFramework
Imports System.Linq
Imports System.Management.Automation
Imports System.Management.Automation.Runspaces
Imports System.Management.Automation.Remoting
Imports System.Collections.Generic
Imports System.Threading
Imports System.Data

Partial Class VitalSignsCore


	Private Sub MonitorNetworkDevices()	 'This is the main sub that calls all the other ones
		'   WriteAuditEntry(Now.ToString & " ^^^ Begin Loop for Network Device Monitoring ^^^")
		Try

			Dim myDevice As MonitoredItems.NetworkDevice
			Do While boolTimeToStop <> True
				Try
					If MyNetworkDevices.Count > 0 Then
						myDevice = Nothing
						myDevice = SelectDeviceToMonitor()
						If Not myDevice Is Nothing Then
							WriteAuditEntry(Now.ToString & " ^^^ End Loop for Network Device Monitoring - no devices due ^^^")
							myDevice.PreviousKeyValue = myDevice.ResponseTime
							MonitorNetworkDevice(myDevice)
							myDevice.LastScan = Date.Now
						End If
					Else
						Thread.Sleep(2500)
					End If

				Catch ex As ThreadAbortException
					WriteAuditEntry(Now.ToString & " Aborting MonitorNetworkDevices sub " & ex.Message)
					Exit Sub
				End Try
				If MyNetworkDevices.Count < 24 Then
					Thread.Sleep(2500)
				End If
				If MyNetworkDevices.Count < 10 Then
					Thread.Sleep(10000)
				End If

				dtNetworkDevicesLastUpdate = Now   'update a global variable to show last loop time
			Loop

			WriteAuditEntry(Now.ToString & " Stoppign Scanning " & boolTimeToStop.ToString())
		Catch ex As Exception

			WriteAuditEntry(Now.ToString & " Stoppign Scanning from error" & ex.Message.ToString())

		End Try


	End Sub

	Private Function SelectDeviceToMonitor() As MonitoredItems.NetworkDevice
		'    WriteAuditEntry(Now.ToString & " >>> Selecting a Network Device for monitoring >>>>")
		Dim tNow As DateTime
		tNow = Now
		Dim tScheduled As DateTime

		Dim timeOne, timeTwo As DateTime

		Dim myDevice As MonitoredItems.NetworkDevice
		Dim SelectedServer As MonitoredItems.NetworkDevice

		Dim ServerOne As MonitoredItems.NetworkDevice
		Dim ServerTwo As MonitoredItems.NetworkDevice
		Dim myRegistry As New RegistryHandler

		Dim n As Integer
		Dim strSQL As String = ""
		Dim ServerType As String = "NetworkDevice"
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

				For n = 0 To MyNetworkDevices.Count - 1
					ServerOne = MyNetworkDevices.Item(n)

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



		'Any db Not Scanned should be scanned right away.  Select the first one you encounter
		For n = 0 To MyNetworkDevices.Count - 1
			ServerOne = MyNetworkDevices.Item(n)

			If ServerOne.Status = "Not Scanned" Then
				'WriteAuditEntry(Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status)
				Return ServerOne
				Exit Function
			End If
		Next

		'start with the first two servers
		ServerOne = MyNetworkDevices.Item(0)
		If MyNetworkDevices.Count > 1 Then ServerTwo = MyNetworkDevices.Item(1)

		'go through the remaining servers, see which one has the oldest (earliest) scheduled time
		If MyNetworkDevices.Count > 2 Then
			Try
				For n = 2 To MyNetworkDevices.Count - 1
					'     WriteAuditEntry(Now.ToString & " N is " & n)
					timeOne = CDate(ServerOne.NextScan)
					timeTwo = CDate(ServerTwo.NextScan)
					If DateTime.Compare(timeOne, timeTwo) < 0 Then
						'time one is earlier than time two, so keep server 1
						ServerTwo = MyNetworkDevices.Item(n)
					Else
						'time two is later than time one, so keep server 2
						ServerOne = MyNetworkDevices.Item(n)
					End If
				Next
			Catch ex As Exception
				WriteAuditEntry(Now.ToString & " >>> Error selecting a Network Device... " & ex.Message)
			End Try
		Else
			'There were only two server, so use those going forward
		End If

		'    WriteAuditEntry(Now.ToString & " >>> Down to two network devices... " & ServerOne.Name & " and " & ServerTwo.Name)

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
			'     WriteAuditEntry(Now.ToString & " >>> Down to one Network Device... " & SelectedServer.Name & " to scan at " & SelectedServer.NextScan & ". Status is " & SelectedServer.Status)
		Else
			SelectedServer = ServerOne
			tScheduled = CDate(ServerOne.NextScan)
		End If
		tScheduled = CDate(SelectedServer.NextScan)
		If DateTime.Compare(tNow, tScheduled) < 0 Then
			If SelectedServer.Status <> "Not Scanned" Then
				'    WriteAuditEntry(Now.ToString & " No Network Devices are scheduled for monitoring, next scan after " & SelectedServer.NextScan)
				SelectedServer = Nothing
			Else
				'     WriteAuditEntry(Now.ToString & " selected Network Device: " & SelectedServer.Name & " because it has not been scanned yet.")
			End If
		Else
			'     WriteAuditEntry(Now.ToString & " selected Network Device: " & SelectedServer.Name)
		End If

		Return SelectedServer
		Exit Function

	End Function

	Private Sub MonitorNetworkDevice(ByRef MyDevice As MonitoredItems.NetworkDevice)
		'  Dim dtLastScan As DateTime = MyDevice.LastScan
		Dim strResponse, StatusDetails As String

		Try
			If MyDevice.Enabled = False Then
				WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " " & MyDevice.Name & " is disabled.")
				With MyDevice
					.Status = "Disabled"
					.ResponseDetails = "Monitoring is disabled for this Network Device."
					'.AlertCondition = False  - don't call this or status is set to OK
				End With
				'    WriteAuditEntry(Now.ToString & " " & MyDevice.Name & " is " & MyDevice.Status & ", says Luke.")
				Exit Try
			End If
			'  WriteAuditEntry(Now.ToString & " pinging " & MyDevice.Name & " at " & MyDevice.IPAddress)



			strResponse = PingNetworkDevice(MyDevice)
			' WriteDeviceHistoryEntry("Network_Device", MyDevice.Name,(Now.ToString & " " & MyDevice.Name & " response was " & strResponse)
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " " & MyDevice.Name & " details: " & MyDevice.ResponseDetails)
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " " & MyDevice.Name & " strResponse: " & strResponse)

			If strResponse <> "Timeout" And strResponse <> "Error" And MyDevice.Enabled = True Then
				MyDevice.Status = "OK"
				MyDevice.AlertCondition = False
				MyDevice.IncrementUpCount()
				myAlert.ResetAlert("Network Device", MyDevice.Name, "Not Responding", MyDevice.Location)
				Try
					MyDevice.ResponseTime = CType(strResponse, Integer)
				Catch ex As Exception
					MyDevice.ResponseTime = 0
				End Try
				MyDevice.ResponseDetails = "Response Time: " & strResponse & " ms"
				WriteToNetworkDevicesStatsTable("ResponseTime", MyDevice.ResponseTime, MyDevice)

				RunScriptsForNetworkDevice(MyDevice)


			Else
				MyDevice.Status = "Not Responding"
				MyDevice.IncrementDownCount()
				MyDevice.AlertCondition = True
				myAlert.QueueAlert("Network Device", MyDevice.Name, "Not Responding", MyDevice.ResponseDetails, MyDevice.Location)
				MyDevice.IncrementDownCount()
			End If

			If MyDevice.Status = "OK" Then
				If MyDevice.ResponseTime > MyDevice.ResponseThreshold Then
					MyDevice.Status = "Slow"
					MyDevice.AlertCondition = True
					MyDevice.ResponseDetails += " Target is " & MyDevice.ResponseThreshold & " ms."
					myAlert.QueueAlert("Network Device", MyDevice.Name, "Slow", MyDevice.ResponseDetails, MyDevice.Location)
				Else
					myAlert.ResetAlert("Network Device", MyDevice.Name, "Slow", MyDevice.Location)
				End If
			End If
			



			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " " & MyDevice.Name & " status: " & MyDevice.Status)
			' WriteAuditEntry(Now.ToString & " " & MyDevice.Name & " details: " & MyDevice.ResponseDetails)

		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " " & ex.Message)
		End Try

		Dim strSQL As String


		'Update the status table

		Dim Percent As Double = 100
		Dim myResponseTime As Double

		Try
			myResponseTime = CType(strResponse, Long)
			'WriteAuditEntry(Now.ToString & " my Response time =" & myResponseTime.ToString)
			'WriteAuditEntry(Now.ToString & " Response Threshold =" & MyDevice.ResponseThreshold)
			Percent = myResponseTime / MyDevice.ResponseThreshold * 100
			'WriteAuditEntry(Now.ToString & " % " & Percent)
		Catch ex As Exception
			myResponseTime = 500
		End Try

		Try
			MyDevice.ResponseTime = myResponseTime
		Catch ex As Exception

		End Try


		Dim PercentageChange As Double
		Try
			If MyDevice.PreviousKeyValue > 0 And MyDevice.ResponseTime > 0 Then
				PercentageChange = -(1 - MyDevice.PreviousKeyValue / MyDevice.ResponseTime)
			Else
				PercentageChange = 0
			End If

		Catch ex As Exception
			PercentageChange = 0
		End Try
		If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " Previous response time was " & MyDevice.PreviousKeyValue & " Current value is " & MyDevice.ResponseTime & "; so percent change is: " & PercentageChange)


Update:
		Try
			'   WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " Status is " & MyDevice.Status)
			MyDevice.StatusCode = ServerStatusCode(MyDevice.Status)
			'    WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " Status code is " & MyDevice.StatusCode)
		Catch ex As Exception
			MyDevice.StatusCode = "OK"
		End Try

		Try
            With MyDevice
                '5/5/2016 NS modified - inserting a Network Device into StatusDetails fails because of a TypeANDName mismatch
                'Changed TypeANDName -ND to -Network Device
                strSQL = "Update Status SET DownCount= '" & MyDevice.DownCount & _
                 "', Status='" & MyDevice.Status & "', Upcount=" & MyDevice.UpCount & _
                ", UpPercent= '" & MyDevice.UpPercentCount & _
                "', Details='" & MyDevice.ResponseDetails & _
                "', LastUpdate='" & Now & _
                "', ResponseTime='" & Str(myResponseTime) & _
                "', PercentageChange='" & Str(PercentageChange) & _
                 "', StatusCode='" & .StatusCode & _
                "', NextScan='" & MyDevice.NextScan & _
                "', ResponseThreshold=" & MyDevice.ResponseThreshold & _
                ", MyPercent='" & (Percent) & _
                "', UpMinutes=" & Microsoft.VisualBasic.Strings.Format(MyDevice.UpMinutes, "F1") & _
                ", DownMinutes=" & Microsoft.VisualBasic.Strings.Format(MyDevice.DownMinutes, "F1") & _
                ", UpPercentMinutes='" & Str(MyDevice.UpPercentMinutes) & _
                "', Name='" & MyDevice.Name & "' " & _
                ", Location='" & MyDevice.Location & "' " & _
                " WHERE TypeANDName='" & MyDevice.Name & "-Network Device'"

            End With
		Catch ex As Exception
			strSQL = ""
		End Try

		Try
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, strSQL)
		Catch ex As Exception

		End Try
		UpdateStatusTableWithNetworkDevice(MyDevice)

	End Sub

	Private Function PingNetworkDevice(ByRef myDevice As MonitoredItems.NetworkDevice) As String
		'Dim myPing As New Ping
		Dim IPAddress As String = myDevice.IPAddress
		myDevice.ResponseDetails = ""
		Try
			Dim ping As New System.Net.NetworkInformation.Ping
			Dim reply As System.Net.NetworkInformation.PingReply = ping.Send(IPAddress)
			'myPing.Open()
			'Dim span As System.TimeSpan = myPing.Send(IPAddress, New TimeSpan(0, 0, 5))

			'If (span.Equals(System.TimeSpan.MaxValue)) Then
			If (reply.Status <> System.Net.NetworkInformation.IPStatus.Success) Then
				myDevice.ResponseDetails = "Request Timed Out"
				Return "Timeout"
			Else
				myDevice.ResponseDetails = "Response time: " & reply.RoundtripTime.ToString("N1") & " ms"
				Return reply.RoundtripTime.ToString("N1")
			End If
		Catch ex As Exception
			' WriteAuditEntry(Now.ToString & " Network Device Ping error: " & ex.Message)
			myDevice.ResponseDetails = ex.Message
			Return "Error"
		Finally
			'myPing.Close()
			'myPing = Nothing
		End Try
	End Function

	Private Sub RunScriptsForNetworkDevice(MyDevice As MonitoredItems.NetworkDevice)
		WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " Network Type: " & MyDevice.NetworkType)
		Try
			Select Case MyDevice.NetworkType
				Case "Juniper Junos"
					RunScriptsForJuniperJunos(MyDevice)

				Case "Juniper ScreenOS"
					'Do nothing for now

				Case "Cisco"
					RunScriptsForCisco(MyDevice)

				Case Else
					WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " No valid Network Type.  Type: " & MyDevice.NetworkType)

			End Select
		Catch ex As Exception
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " Error in RunScriptsForNetworkDevice with type " & MyNetworkDevice.NetworkType & "...Error: " & ex.Message.ToString())
		End Try



	End Sub

	Private Sub RunScriptsForCisco(MyDevice As MonitoredItems.NetworkDevice)
		Try


			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " In Cisco")

			Dim session As Runspaces.InitialSessionState = Runspaces.InitialSessionState.CreateDefault()
			Dim modules As String() = {"Posh-SSH"}
			session.ImportPSModule(modules)

			Dim powershell As PowerShell = powershell.Create()
			Dim runspace As Runspace = RunspaceFactory.CreateRunspace(session)
			Dim command As PSCommand = New PSCommand()



			Dim cmd As String = "& '" & AppDomain.CurrentDomain.BaseDirectory.ToString() & "Scripts\\Cisco_ScriptsForCisco.ps1" &
			 "' -IPAddress " & MyDevice.IPAddress & " -Username " & MyDevice.UserName & " -Password  " & MyDevice.Password & ""
			'WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, cmd)

			command.AddScript(cmd)

			Dim results As System.Collections.ObjectModel.Collection(Of PSObject) = New System.Collections.ObjectModel.Collection(Of PSObject)
			powershell.Streams.ClearStreams()
			runspace.Open()
			powershell.Runspace = runspace
			powershell.Commands = command
			results = powershell.Invoke()
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " Cisco results count: " & results.Count.ToString())
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " Cisco results error count: " & powershell.Streams.Error.Count.ToString())
			'If (powershell.Streams.Error.Count > 0) Then
			'For Each ps As ErrorRecord In powershell.Streams.Error
			'WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Error: " & ps.ToString().ToString(), LogLevel.Normal)
			'Next
			'End If
			If results.Count > 0 Then

				'WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "In:", LogLevel.Normal)

				WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "results: " & results.ToString(), LogLevel.Normal)
				'WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "results(0): " & results(0).ToString(), LogLevel.Normal)
				'WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "results(0).BaseObject: " & results(0).BaseObject.ToString(), LogLevel.Normal)

				Dim str As String = results(0).BaseObject.ToString()

				WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Output: " & str, LogLevel.Normal)

				If (str.Contains("show environment all")) Then
					DoScriptsForCiscoFor12_4(str, MyDevice)
				Else
					DoScriptsForCiscoFor12_5(str, MyDevice)
				End If




			End If


		Catch ex As Exception
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Error in RunScriptsForCisco.  Error: " & ex.Message.ToString())
		End Try









	End Sub


	Private Sub DoScriptsForCiscoFor12_4(str As String, MyDevice As MonitoredItems.NetworkDevice)

		Dim list As System.Collections.Generic.List(Of String) = str.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList()

		list.RemoveAll(Function(s) String.IsNullOrEmpty(s))

		Dim currString As String

		'currString = list.ElementAt(list.IndexOf("Power Supply:") + 1).Trim()
		'currString = currString.ToString().Substring(0, currString.ToString().Length - 1)
		'MyDevice.PowerSupplyStatus = currString

		Try
			currString = list.ElementAt(list.IndexOf("Board Temperature:")).Trim()
			currString = currString.ToString().Substring(currString.IndexOf("Board Temperature:") + "Board Temperature:".ToString() + 1)
			MyDevice.BoardTemp = currString
		Catch ex As Exception
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Error in RunScriptsForCisco for Board Temp.  Error: " & ex.Message.ToString(), LogLevel.Normal)
		End Try


		'currString = list.ElementAt(list.IndexOf("Fan status:") + 1).Trim()
		'currString = currString.ToString().Substring(0, currString.ToString().Length - 1)
		'MyDevice.FanStatus = currString
		Try
			currString = list.ElementAt(list.IndexOf(list.Where(Function(s) s.Contains("show processes memory"))(0).ToString()) + 1).ToString().Trim()
			'list.Where(Function(s) s.Contains("Total"))(0).ToString().Trim()
			If (currString.IndexOf(",") > 0) Then

				Dim Total As String = currString.Substring(currString.IndexOf(" "), currString.IndexOf(",") - currString.IndexOf(" ")).Trim()
				currString = currString.Substring(currString.IndexOf(",") + 2).Trim()
				Dim Used As String = currString.Substring(currString.IndexOf(" "), currString.IndexOf(",") - currString.IndexOf(" ")).Trim()
				Dim perc As Integer = (Convert.ToInt32(Used) / Convert.ToInt32(Total)) * 100
				MyDevice.Memory = perc

			Else
				'Processor Pool Total:   37613120 Used:   16101128 Free:   21511992
				Dim Total As String = currString.Substring(currString.IndexOf(":") + 1).Trim()
				Total = Total.Substring(0, Total.IndexOf("Used")).Trim()
				WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Total  " & Total, LogLevel.Normal)

				currString = currString.Substring(currString.IndexOf("Used:") + "Used:".Length).Trim()
				WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "curr " & currString, LogLevel.Normal)

				Dim Used As String = currString.Substring(0, currString.IndexOf("F") - 1).Trim()
				WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Used " & Used, LogLevel.Normal)

				Dim perc As Integer = (Convert.ToInt32(Used) / Convert.ToInt32(Total)) * 100
				MyDevice.Memory = perc

			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Error in RunScriptsForCisco for Memory.  Error: " & ex.Message.ToString(), LogLevel.Normal)
		End Try

		Try
			currString = list.Where(Function(s) s.StartsWith("CPU"))(0).ToString().Trim()
			currString = currString.Substring(0, currString.IndexOf("%"))
			currString = currString.Substring(currString.LastIndexOf(" ")).Trim()
			MyDevice.CPU = currString
		Catch ex As Exception
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Error in RunScriptsForCisco for CPU.  Error: " & ex.Message.ToString(), LogLevel.Normal)
		End Try

		Try
			Dim index As Integer = list.IndexOf(list.Where(Function(s) s.StartsWith("Cisco Internetwork Operating System Software"))(0))
			If (index > 0) Then
				'Or s.StartsWith("Cisco IOS Software"))(0))
				currString = list(index + 1).ToString().Trim()
				currString = currString.Substring(currString.IndexOf("Version") + "Version".Length).Trim()
				currString = currString.Substring(0, currString.IndexOf(" ") - 1)
				MyDevice.OS = currString

				index = list.IndexOf(list.Where(Function(s) s.StartsWith("System image file is"))(0))
				'currString = list.Where(Function(s) s.StartsWith("System image file is"))(0).ToString().Trim()
				currString = list(index + 1).ToString()
				currString = currString.Substring(0, currString.IndexOf("processor") - 1).ToString().Trim()
				MyDevice.Model = currString

			Else

				index = list.IndexOf(list.Where(Function(s) s.StartsWith("Cisco IOS Software"))(0))

				currString = list(index).ToString().Trim()
				currString = currString.Substring(currString.IndexOf("Version") + "Version".Length + 1).Trim()
				currString = currString.Substring(0, currString.IndexOf(",")).Trim
				'currString = currString.Substring(0, currString.IndexOf(" ") - 1)
				MyDevice.OS = currString

				WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "os: " & currString, LogLevel.Normal)

				index = list.IndexOf(list.Where(Function(s) s.StartsWith("System image file is"))(0))
				'currString = list.Where(Function(s) s.StartsWith("System image file is"))(0).ToString().Trim()
				currString = list(index).ToString()
				currString = currString.Substring(currString.IndexOf(""""))
				currString = currString.Substring(1, currString.Length - 2)
				MyDevice.Model = currString
				WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "sysimg: " & currString, LogLevel.Normal)
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Error in RunScriptsForCisco for OS and Model.  Error: " & ex.Message.ToString(), LogLevel.Normal)
		End Try

		CleanNetworkDevicesDetailsTable(MyDevice)
		WriteToNetworkDevicesDetailsTable("Cisco Router Number", MyDevice.Model.ToString(), MyDevice)
		WriteToNetworkDevicesDetailsTable("Cisco IOS Version", MyDevice.OS.ToString(), MyDevice)
		'WriteToNetworkDevicesDetailsTable("Power Supply Status", MyDevice.PowerSupplyStatus.ToString(), MyDevice)
		WriteToNetworkDevicesDetailsTable("Board Temperature", MyDevice.BoardTemp.ToString(), MyDevice)
		'WriteToNetworkDevicesDetailsTable("Fan Status", MyDevice.FanStatus.ToString(), MyDevice)
		WriteToNetworkDevicesStatsTable("Memory", MyDevice.Memory.ToString(), MyDevice)
		WriteToNetworkDevicesStatsTable("Platform.System.PctCombinedCpuUtil", MyDevice.CPU.ToString(), MyDevice)



	End Sub

	Private Sub DoScriptsForCiscoFor12_5(str As String, MyDevice As MonitoredItems.NetworkDevice)

		Dim ROM As String = ""
		Dim ProcessorID As String = ""
		Dim ChassisType As String = ""

		Dim list As System.Collections.Generic.List(Of String) = str.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList()

		list.RemoveAll(Function(s) String.IsNullOrEmpty(s))

		Dim currString As String

		Try
			currString = list.Where(Function(s) s.StartsWith("CPU"))(0).ToString().Trim()
			currString = currString.Substring(0, currString.IndexOf("%"))
			currString = currString.Substring(currString.LastIndexOf(" ")).Trim()
			MyDevice.CPU = currString
		Catch ex As Exception
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Error in RunScriptsForCisco for CPU.  Error: " & ex.Message.ToString(), LogLevel.Normal)
		End Try

		Try
			currString = list.Where(Function(s) s.StartsWith("ROM:"))(0).ToString().Trim()

			ROM = currString.Substring(currString.IndexOf(" ") + 1).Trim()

		Catch ex As Exception
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Error in RunScriptsForCisco for ROM.  Error: " & ex.Message.ToString(), LogLevel.Normal)
		End Try

		Try
			currString = list.Where(Function(s) s.StartsWith("Processor board ID"))(0).ToString().Trim()
			ProcessorID = currString.Substring(currString.LastIndexOf(" ")).Trim()
		Catch ex As Exception
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Error in RunScriptsForCisco for ProcessorID.  Error: " & ex.Message.ToString(), LogLevel.Normal)
		End Try

		Try
			currString = list.Where(Function(s) s.StartsWith("Chassis type"))(0).ToString().Trim()
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Chassis : " & currString, LogLevel.Normal)
			ChassisType = currString.Substring(currString.LastIndexOf(" ")).Trim()

		Catch ex As Exception
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Error in RunScriptsForCisco for OS and Model.  Error: " & ex.Message.ToString(), LogLevel.Normal)
		End Try

		Try

			currString = list.ElementAt(list.IndexOf(list.Where(Function(s) s.Contains("show memory free"))(0).ToString()) + 1).ToString().Trim()
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Proc 1: " & currString, LogLevel.Normal)
			currString = currString.Substring(currString.IndexOf(" ")).Trim()
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Proc 2: " & currString, LogLevel.Normal)
			currString = currString.Substring(currString.IndexOf(" ")).Trim()
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Proc 3: " & currString, LogLevel.Normal)
			Dim Total As String = currString.Substring(0, currString.IndexOf(" ")).Trim()
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Total : " & Total, LogLevel.Normal)
			currString = currString.Substring(currString.IndexOf(" ")).Trim()
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Proc 4: " & currString, LogLevel.Normal)
			Dim Used As String = currString.Substring(0, currString.IndexOf(" ")).Trim()
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "PUsedroc : " & Used, LogLevel.Normal)
			Dim perc As Integer = (Convert.ToInt64(Used) / Convert.ToInt64(Total)) * 100
			MyDevice.Memory = perc
			'head total used
			'Processor  7F03ADFDD010   2212131296   239065576   1973065720   1972716840   131
			'1723868:

		Catch ex As Exception
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Error in RunScriptsForCisco for Memory.  Error: " & ex.Message.ToString(), LogLevel.Normal)
		End Try

		'Dim Versi
		Try

			currString = list.Where(Function(s) s.StartsWith("Cisco IOS Software"))(0).ToString().Trim()
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Ver 1: " & currString, LogLevel.Normal)
			currString = currString.Substring(currString.IndexOf("Version ") + "Version ".Length).Trim()
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "ver 2: " & currString, LogLevel.Normal)
			currString = currString.Substring(0, currString.IndexOf(",")).Trim()
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "ver 3: " & currString, LogLevel.Normal)
			MyDevice.OS = currString
		Catch ex As Exception
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Error in RunScriptsForCisco for Version.  Error: " & ex.Message.ToString(), LogLevel.Normal)
		End Try

		CleanNetworkDevicesDetailsTable(MyDevice)
		WriteToNetworkDevicesDetailsTable("ROM", ROM, MyDevice)
		WriteToNetworkDevicesDetailsTable("Processor Board ID", ProcessorID, MyDevice)
		WriteToNetworkDevicesDetailsTable("Chassis Type", ChassisType, MyDevice)
		WriteToNetworkDevicesDetailsTable("ISO Version", MyDevice.OS, MyDevice)
		WriteToNetworkDevicesStatsTable("Memory", MyDevice.Memory.ToString(), MyDevice)
		WriteToNetworkDevicesStatsTable("Platform.System.PctCombinedCpuUtil", MyDevice.CPU.ToString(), MyDevice)



	End Sub


	Private Sub RunScriptsForJuniperJunos(MyDevice As MonitoredItems.NetworkDevice)
		Try


			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " In JuniperJunos", LogLevel.Normal)

			Dim session As Runspaces.InitialSessionState = Runspaces.InitialSessionState.CreateDefault()
			Dim modules As String() = {"Posh-Junos", "Posh-SSH"}
			session.ImportPSModule(modules)

			Dim powershell As PowerShell = powershell.Create()
			Dim runspace As Runspace = RunspaceFactory.CreateRunspace(session)
			Dim command As PSCommand = New PSCommand()

			Dim stri As String = "Invoke-JunosCommand -Device " & MyDevice.IPAddress & " -User " & MyDevice.UserName & " -Password " & MyDevice.Password & " -Command 'show chassis routing-engine'"
			'WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " In JuniperJunos: " & stri, LogLevel.Normal)

			command.AddScript(stri)
			Dim results As New System.Collections.ObjectModel.Collection(Of PSObject)

			runspace.Open()
			powershell.Runspace = runspace

			powershell.Commands = command

			results = powershell.Invoke()

			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " Show Chassis Routing-Engine results count: " & results.Count.ToString())

			If results.Count = 3 Then
				WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " " & results(1).ToString(), LogLevel.Normal)
				WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " " & results(1).BaseObject().ToString(), LogLevel.Normal)

				Dim str As String = results(1).BaseObject.ToString()
				'WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " Retruend String: " & str, LogLevel.Normal)


				Dim ArrayOfTitles As String() = {
				  "Temperature",
				  "Total memory",
				  "Control plane memory",
				  "Data plane memory ",
				  "CPU utilization",
				  "User",
				  "Background",
				  "Kernel",
				  "Interrupt",
				  "Idle",
				  "Model",
				  "Serial ID",
				  "Start time",
				  "Uptime",
				  "Last reboot reason",
				  "Load averages:"
				  }

				Dim ArrayOfValues(ArrayOfTitles.Length) As String

				For i As Integer = 0 To ArrayOfTitles.Length - 1
					If ArrayOfTitles(i).ToString().Contains("CPU utilization") Then
						Continue For
					End If

					If i = ArrayOfTitles.Length - 1 Then
						Dim start As Integer = str.IndexOf(ArrayOfTitles(i)) + ArrayOfTitles(i).Length
						ArrayOfValues(i) = str.Substring(start)
						Continue For
					End If
					Dim startIndex As Integer = str.IndexOf(ArrayOfTitles(i)) + ArrayOfTitles(i).Length

					Dim endIndex As Integer = str.IndexOf(ArrayOfTitles(i + 1))

					Dim shortendString As String = str.Substring(startIndex, endIndex - startIndex).Trim()

					If shortendString.Contains("percent") Then
						Dim UpToEndOfNum As String = shortendString.Substring(0, shortendString.LastIndexOf(" "))
						Dim JustTheNum As String = UpToEndOfNum.Substring(If(UpToEndOfNum.LastIndexOf(" ") < 0, 0, UpToEndOfNum.LastIndexOf(" ")))
						shortendString = JustTheNum
					End If
					ArrayOfValues(i) = shortendString



				Next

				WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " In Array Made of values", LogLevel.Normal)
				Dim dict As Dictionary(Of String, String) = New Dictionary(Of String, String)

				For i As Integer = 0 To ArrayOfTitles.Length - 1
					WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " In loop..." & ArrayOfValues(i), LogLevel.Normal)
					If ArrayOfTitles(i).ToString().Contains("CPU utilization") Then
						Continue For
					End If
					If i = ArrayOfTitles.Length - 1 Then
						Dim numberVals As String = ArrayOfValues(i).Substring(ArrayOfValues(i).IndexOf("15 minute") + "15 minute".Length).Replace("\n", "").Trim()
						Dim oneMin As String = numberVals.Substring(0, numberVals.IndexOf(" "))
						numberVals = numberVals.Substring(numberVals.IndexOf(" ")).Trim()
						Dim fiveMins As String = numberVals.Substring(0, numberVals.IndexOf(" "))
						Dim fifteenMins As String = numberVals.Substring(numberVals.IndexOf(" ")).Trim()

						dict.Add("Load Averages 1 Minute", oneMin)
						dict.Add("Load Averages 5 Minute", fiveMins)
						dict.Add("Load Averages 15 Minute", fifteenMins)
					Else
						dict.Add(ArrayOfTitles(i), ArrayOfValues(i).Replace("\n", "").Trim())
					End If

				Next

				WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, Now.ToString & " Dict Made", LogLevel.Normal)

				Dim temp As String = dict("Temperature").ToString()
				temp = temp.Substring(temp.IndexOf("/") + 1).Trim()
				temp = temp.Substring(0, temp.IndexOf(" ")).Trim().ToString()

				MyDevice.CPU = 100 - Convert.ToInt32(dict("Idle").ToString()).ToString()
				MyDevice.Memory = dict("Total memory").ToString()
				MyDevice.BoardTemp = temp
				MyDevice.Model = dict("Model").ToString()
				MyDevice.SerialID = dict("Serial ID").ToString()
				MyDevice.StartTime = dict("Start time").ToString()
				MyDevice.UpTime = dict("Uptime").ToString()
				MyDevice.LastRebootReason = dict("Last reboot reason").ToString()


				CleanNetworkDevicesDetailsTable(MyDevice)
				WriteToNetworkDevicesStatsTable("Platform.System.PctCombinedCpuUtil", MyDevice.CPU, MyDevice)
				WriteToNetworkDevicesStatsTable("Memory", MyDevice.Memory, MyDevice)

				WriteToNetworkDevicesDetailsTable("Temperature Fahrenheit", MyDevice.BoardTemp, MyDevice)
				WriteToNetworkDevicesDetailsTable("Model Number", MyDevice.Model.ToString(), MyDevice)
				WriteToNetworkDevicesDetailsTable("Serial ID", MyDevice.SerialID.ToString(), MyDevice)
				WriteToNetworkDevicesDetailsTable("Start Time", MyDevice.StartTime.ToString(), MyDevice)
				WriteToNetworkDevicesDetailsTable("Up Time", MyDevice.UpTime.ToString(), MyDevice)
				WriteToNetworkDevicesDetailsTable("Last Reboot Reason", MyDevice.LastRebootReason.ToString(), MyDevice)


			Else

				WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Unexpected number of results in RunScriptsForNetowrkDevice.  Returned " & results.Count)


			End If



		Catch ex As Exception
			WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, "Error in RunScriptsForJuniperJunos.  Error: " & ex.Message.ToString())
		End Try


	End Sub

	Private Sub WriteToNetworkDevicesDetailsTable(Name As String, Value As String, MyDevice As MonitoredItems.NetworkDevice)

		Dim dtNow As DateTime = DateTime.Now
		Dim weekNumber As Integer = GetWeekNumber(dtNow)

		Dim sqlQuery As String = "Insert into VitalSigns.dbo.NetworkDevicesDetails(NetworkID, StatName, StatValue) " &
		  " values('" & MyDevice.ID & "','" & Name & "','" & Value & "')"

		WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, sqlQuery)

		Dim adapter As New VSAdaptor
		adapter.ExecuteNonQueryAny("VitalSigns", "", sqlQuery)

	End Sub

	Private Sub CleanNetworkDevicesDetailsTable(MyDevice As MonitoredItems.NetworkDevice)

		Dim dtNow As DateTime = DateTime.Now
		Dim weekNumber As Integer = GetWeekNumber(dtNow)

		Dim sqlQuery As String = "Delete VitalSigns.dbo.NetworkDevicesDetails where NetworkId=" & MyDevice.ID & ""

		WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, sqlQuery)

		Dim adapter As New VSAdaptor
		adapter.ExecuteNonQueryAny("VitalSigns", "", sqlQuery)

	End Sub

	Private Sub WriteToNetworkDevicesStatsTable(Name As String, Value As String, MyDevice As MonitoredItems.NetworkDevice)

		Dim dtNow As DateTime = DateTime.Now
		Dim weekNumber As Integer = GetWeekNumber(dtNow)

		Dim sqlQuery As String = "Insert into VSS_Statistics.dbo.DeviceDailyStats(ServerName,DeviceType,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber) " &
		  " values('" & MyDevice.Name & "','Network Device','" & dtNow & "','" & Name & "','" & Value &
		"'," & weekNumber & ", " & dtNow.Month.ToString() & ", " & dtNow.Year.ToString() & ", " & dtNow.Day.ToString() & ")"

		WriteDeviceHistoryEntry("Network_Device", MyDevice.Name, sqlQuery)

		Dim adapter As New VSAdaptor
		adapter.ExecuteNonQueryAny("VSS_Statistics", "", sqlQuery)

	End Sub


End Class
