Imports VSFramework
Imports System.Threading

Partial Public Class VitalSignsPlusCore
#Region "BlackBerry Server  Monitoring"

	Private Shared BESSelector_Mutex As New Mutex()

	Private Sub MonitorBlackBerryServers()	'This is the main sub that calls all the other ones

		Dim myBES_Server As MonitoredItems.BlackBerryServer
		Dim n As Long = 1

		Do
			'      WriteAuditEntry(Now.ToString & " Checking BlackBerry Servers.  Loop # " & n.ToString)
			Try
				If MyBlackBerryServers.Count > 0 Then
					myBES_Server = Nothing
					Try
						BESSelector_Mutex.WaitOne()
						myBES_Server = SelectBESToMonitor()
						If (Not (myBES_Server Is Nothing)) Then
							myBES_Server.LastScan = Now
							myBES_Server.IsBeingScanned = True
						End If
					Catch ex As Exception
						myBES_Server = Nothing
					Finally
						BESSelector_Mutex.ReleaseMutex()

					End Try

					If Not myBES_Server Is Nothing Then
						'  WriteAuditEntry(Now.ToString & " Selected BlackBerry Server " & myDevice.Name)

                        If InMaintenance("BES", myBES_Server.Name) = True Then
                            myBES_Server.Status = "Maintenance"
                            myBES_Server.LastScan = Now
                            myBES_Server.ResponseDetails = "This server is in a scheduled maintenance period.  Monitoring is temporarily disabled."
                            'Not in maintenance
                        Else
                            myBES_Server.AlertCondition = False
                            Try
                                WriteDeviceHistoryEntry("BES_Server", myBES_Server.Name, Now.ToString & " Monitoring BES Server " & myBES_Server.Name)
                                MonitorBESServer(myBES_Server)
                            Catch ex As Exception
                                WriteDeviceHistoryEntry("BES_Server", myBES_Server.Name, Now.ToString & " Error in MonitorBlackBerryServers sub when calling MonitorBESServer: " & ex.Message)
                            End Try

                        End If


						Try
							WriteDeviceHistoryEntry("BES_Server", myBES_Server.Name, Now.ToString & " Updating BES Server Status")
							UpdateBES_Server_Status(myBES_Server)
						Catch ex As Exception
							WriteAuditEntry(Now.ToString & " Error in MonitorBlackBerryServers sub when calling Update BES Server Status: " & ex.Message)
						End Try

						WriteAuditEntry(Now.ToString & " 5.  " & myBES_Server.Name & "'s  pending Messages : " & myBES_Server.PendingMessages)
						WriteAuditEntry(Now.ToString & " 5. " & myBES_Server.Name & "'s  pending threshold :" & myBES_Server.BES_Pending_Messages_Threshold)


						If Not myBES_Server.Status = "Not Responding" Then
							Try
								WriteDeviceHistoryEntry("BES_Server", myBES_Server.Name, Now.ToString & " Tracking BES Statistics")
								TrackBESMailStatistics(myBES_Server)
							Catch ex As Exception
								WriteAuditEntry(Now.ToString & " Error in MonitorBlackBerryServers sub when calling TrackBESMailStatistics: " & ex.Message)
							End Try
						End If

                        myBES_Server.IsBeingScanned = False
					End If
				Else
					Thread.CurrentThread.Sleep(2500)
				End If
			Catch ex As Exception
				WriteAuditEntry(Now.ToString & " Error in MonitorBlackBerryServers sub " & ex.Message)

			End Try


			If MyBlackBerryServers.Count < 24 Then
				Thread.CurrentThread.Sleep(2500)
			End If
			If MyBlackBerryServers.Count < 10 Then
				Thread.CurrentThread.Sleep(10000)
			End If

			Thread.CurrentThread.Sleep(90000)  'sleep 1 minute, 30 sec to allow SNMP replies to time out

			dtBESLastUpdate = Now	'update a global variable to show last loop time
			n += 1
		Loop

	End Sub

	Private Function SelectBESToMonitor() As MonitoredItems.BlackBerryServer
		Dim tNow As DateTime
		tNow = Now
		Dim tScheduled As DateTime

		Dim timeOne, timeTwo As DateTime

		Dim myDevice As MonitoredItems.BlackBerryServer
		Dim SelectedServer As MonitoredItems.BlackBerryServer

		Dim ServerOne As MonitoredItems.BlackBerryServer
		Dim ServerTwo As MonitoredItems.BlackBerryServer

		Dim myRegistry As New RegistryHandler

		Dim n As Integer
		Dim strSQL As String = ""
		Dim ServerType As String = "BlackBerry"
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

				For n = 0 To MyBlackBerryServers.Count - 1
					ServerOne = MyBlackBerryServers.Item(n)

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

		'   WriteAuditEntry(Now.ToString & " >>> Selecting a BlackBerry server from the " & MyBlackBerryServers.Count & " defined to monitor.")

		'Any server Not Scanned should be scanned right away.  Select the first one you encounter
		For n = 0 To MyBlackBerryServers.Count - 1
			ServerOne = MyBlackBerryServers.Item(n)

			If ServerOne.Status = "Not Scanned" And ServerOne.IsBeingScanned = False Then
				WriteAuditEntry(Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status)
				Return ServerOne
				Exit Function
			End If
		Next


		'     WriteAuditEntry(Now.ToString & " All BlackBerry Servers have been scanned at least once...Now determining which one is due.")

		Dim ScanCandidates As New MonitoredItems.BlackBerryServerCollection
		For Each srv As MonitoredItems.BlackBerryServer In MyBlackBerryServers
			'WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " " & srv.Name & " scan status: " & srv.IsBeingScanned)
			If srv.IsBeingScanned = False And srv.Enabled = True Then
				tNow = Now
				tScheduled = srv.NextScan
				If DateTime.Compare(tNow, tScheduled) > 0 Then
					ScanCandidates.Add(srv)
				End If

				' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " " & srv.Name & " is not being scanned.")
			End If
		Next
		' WriteDeviceHistoryEntry("All", "Selection", vbCrLf & vbCrLf)


		If ScanCandidates.Count = 0 Then
			' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " No servers are due to be scanned exiting sub.")
			' WriteDeviceHistoryEntry("All", "Selection", " ")
			Thread.Sleep(10000)
			Return Nothing
		Else
			' WriteDeviceHistoryEntry("All", "Selection", vbCrLf & vbCrLf)
			' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " *********** Scan Candidates *************")
			For Each srv As MonitoredItems.BlackBerryServer In ScanCandidates
				' WriteDeviceHistoryEntry("All", "Selection", Now.ToString & " " & srv.Name & " was last scanned at " & srv.LastScan & "   Scheduled Scan: " & srv.NextScan)
			Next
		End If


		'start with the first two servers
		ServerOne = ScanCandidates.Item(0)
		If ScanCandidates.Count > 1 Then ServerTwo = ScanCandidates.Item(1)

		'go through the remaining servers, see which one has the oldest (earliest) scheduled time
		If ScanCandidates.Count > 2 Then
			Try
				For n = 2 To ScanCandidates.Count - 1
					'  WriteAuditEntry(Now.ToString & " N is " & n)
					timeOne = CDate(ServerOne.NextScan)
					timeTwo = CDate(ServerTwo.NextScan)
					If DateTime.Compare(timeOne, timeTwo) < 0 Then
						'time one is earlier than time two, so keep server 1
						ServerTwo = ScanCandidates.Item(n)
					Else
						'time two is later than time one, so keep server 2
						ServerOne = ScanCandidates.Item(n)
					End If
				Next
			Catch ex As Exception
				WriteAuditEntry(Now.ToString & " >>> Error selecting a BlackBerry Servers ... " & ex.Message)
			End Try
		Else
			'There were only two server, so use those going forward
		End If

		'     WriteAuditEntry(Now.ToString & " >>> Down to two BES Servers... " & ServerOne.Name & " and " & ServerTwo.Name)

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
			'    WriteAuditEntry(Now.ToString & " >>> Down to one BlackBerry Server... " & SelectedServer.Name & " to scan at " & SelectedServer.NextScan & ". Status is " & SelectedServer.Status)
		Else
			SelectedServer = ServerOne
			tScheduled = CDate(ServerOne.NextScan)
		End If

		If MyBlackBerryServers.Count = 1 Then SelectedServer = MyBlackBerryServers.Item(0)

		tScheduled = CDate(SelectedServer.NextScan)
		If DateTime.Compare(tNow, tScheduled) < 0 Then
			If SelectedServer.Status <> "Not Scanned" Then
				'         WriteAuditEntry(Now.ToString & " No BES Servers are scheduled for monitoring, next scan after " & SelectedServer.NextScan)
				SelectedServer = Nothing
			Else
				'             WriteAuditEntry(Now.ToString & " selected BES Server: " & SelectedServer.Name & " because it has not been scanned yet.")
			End If
		Else
			'  WriteAuditEntry(Now.ToString & " selected BES Server: " & SelectedServer.Name)
		End If

		Return SelectedServer
		Exit Function

	End Function

	Private Sub MonitorBESServer(ByRef BES_Server As MonitoredItems.BlackBerryServer)
		Dim SNMPManager As New nsoftware.IPWorks.Snmpmgr("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000")

		Dim dtLastScan As DateTime = BES_Server.LastScan
		BES_Server.LastScan = Now
		BES_Server.Windows_Services = ""
		BES_Server.Status = "OK"
		BES_Server.IsUp = "True"
		BES_Server.ResponseDetails = "The BlackBerry Enterprise Server is fully operational."
		BES_Server.Description = "The BlackBerry Enterprise Server last checked at " & Date.Now.ToShortTimeString
		BES_Server.AlertCondition = False
		BES_Server.SNMP_Failure_Count = 0

		If BES_Server.Enabled = False Then
			WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " " & BES_Server.Name & " is disabled.")
			With BES_Server
				.Status = "Disabled"
				.ResponseDetails = "Monitoring is disabled for this BlackBerry."
				'.AlertCondition = False  - don't call this here or status is set to OK
			End With
			Exit Sub
		End If




		'********************* SRP Connection State


		'SRP Connection
		Try
			BES_Server.Connected_To_SRP = Get_SRP_Status(BES_Server)

			If BES_Server.Connected_To_SRP = False Then
				WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " BES Server reports that it is NOT connected to the SRP.")
				BES_Server.Description = " BES Server reports that it is NOT CONNECTED to the SRP at " & Date.Now.ToShortTimeString
				BES_Server.Status = "Not Connected"

			Else
				BES_Server.Connected_To_SRP = True
				WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " BES Server reports that it IS connected to the SRP.")
				BES_Server.Description = " BES Server reports that it is CONNECTED to the SRP at " & Date.Now.ToShortTimeString
				'Connected
			End If

		Catch ex As Exception
			' MyDevice.Connected_To_SRP = False
			BES_Server.Description = " BES Server reports that it is CONNECTED to the SRP at " & Date.Now.ToShortTimeString
		End Try

		WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " Handling this server as " & BES_Server.HAOption)
		'SRP Connection handling varies depending on the HA option selected
		Dim HAPartner As MonitoredItems.BlackBerryServer
		Dim HAPartner_Status As String

		Select Case BES_Server.HAOption
			Case "HA Server-Typically Active"
				If BES_Server.Connected_To_SRP = False Then
					BES_Server.AlertCondition = True
					BES_Server.Status = "Not Connected"
					BES_Server.IncrementDownCount()
					BES_Server.ResponseDetails = "BES Server reports that it is not connected to the Service Provider."
					WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " BES HA Server " & BES_Server.Name & " is not connected to the service provider.")
					myAlert.QueueAlert("BES", BES_Server.Name, "SRP Connection Failure", "BlackBerry Enterprise Server " & BES_Server.Name & " reports that is not connected to the RIM Network.", BES_Server.Location)
					HAPartner = MyBlackBerryServers.Search(BES_Server.HAPartner)
					If Not (HAPartner Is Nothing) Then
						HAPartner_Status = Get_SRP_Status(HAPartner)
						Select Case HAPartner_Status
							Case True
								BES_Server.ResponseDetails = "Standby mode (" & BES_Server.HAPartner & " is currently active.) "
							Case False
								BES_Server.ResponseDetails = "Warning: Not connected to SRP. HA Partner " & BES_Server.HAPartner & " is also NOT connected. "
								UpdateStatusTable("Update Status Set Status='Not Connected', StatusCode='Issue', Details = '" & BES_Server.ResponseDetails & "' WHERE TypeANDName='" & BES_Server.Name & "-BlackBerry Server'")
								Exit Sub
							Case Else
								BES_Server.ResponseDetails = "Warning: Not connected to SRP. HA Partner " & BES_Server.HAPartner & " is returning an error:  " & HAPartner_Status
						End Select
					End If
				Else
					myAlert.ResetAlert("BES", BES_Server.Name, "SRP Connection Failure", BES_Server.Location)
					BES_Server.ResponseDetails = "HA BES Server is connected to the BlackBerry Network."
					WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " BES HA Server " & BES_Server.Name & " is connected to the service provider.")

				End If
			Case "HA Server-Typically Standby"
				'SRP Connection
				If BES_Server.Connected_To_SRP = False Then
					BES_Server.AlertCondition = True
					BES_Server.Status = "Standby"
					BES_Server.IncrementUpCount()
					BES_Server.ResponseDetails = "BES Server is in standby mode."
					HAPartner = MyBlackBerryServers.Search(BES_Server.HAPartner)
					If Not (HAPartner Is Nothing) Then
						HAPartner_Status = Get_SRP_Status(HAPartner)
						Select Case HAPartner_Status   'This returns whether server is connected to the SRP or not
							Case True
								'Current server is standby, and partner is fine
								BES_Server.ResponseDetails = "Standby mode (" & BES_Server.HAPartner & " is currently active.) "
								myAlert.ResetAlert("BES", BES_Server.Name, "HA - Failover", BES_Server.Location)
							Case False
								'Current server is standby, but HA Partner is not connected.  Bad
								'  BES_Server.ResponseDetails = "Server not connected to SRP. HA Partner " & BES_Server.HAPartner & " is also NOT connected. "
								BES_Server.ResponseDetails = "Warning: Not connected to SRP. HA Partner " & BES_Server.HAPartner & " is also NOT connected. "
								BES_Server.Status = "Not Connected"
								UpdateStatusTable("Update Status Set Status='Not Connected', StatusCode='Issue', Details = '" & BES_Server.ResponseDetails & "' WHERE TypeANDName='" & BES_Server.Name & "-BlackBerry Server'")
								Exit Sub
							Case Else
								BES_Server.ResponseDetails = "Server not connected to SRP. HA Partner " & BES_Server.HAPartner & " is returning an error:  " & HAPartner_Status
						End Select

					End If

					WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " BES Server " & BES_Server.Name & " is in standby mode, and not connected to the service provider.")

				Else
					myAlert.QueueAlert("BES", BES_Server.Name, "HA - Failover", "The normally standby BlackBerry Enterprise Server " & BES_Server.Name & " reports that is connected to the RIM Network.  This likely means that the primary has failed.", BES_Server.Location)
				End If
				UpdateBES_Server_Status(BES_Server)
				'Exit Sub
			Case Else
				'Assume it is a stand alone server
				'SRP Connection
				If BES_Server.Connected_To_SRP = False Then
					BES_Server.AlertCondition = True
					BES_Server.Status = "Not Connected"
					BES_Server.IncrementDownCount()
					BES_Server.ResponseDetails = "BES Server reports that it is not connected to the Service Provider."
					WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " BES Server " & BES_Server.Name & " is not connected to the service provider.")
					myAlert.QueueAlert("BES", BES_Server.Name, "SRP Connection Failure", "BlackBerry Enterprise Server " & BES_Server.Name & " reports that is not connected to the RIM Network.", BES_Server.Location)
					Exit Sub
				Else
					myAlert.ResetAlert("BES", BES_Server.Name, "SRP Connection Failure", BES_Server.Location)
				End If
		End Select



		'****************************
		'Check the status of the BlackBerry server by querying the BES MIB

		Try
			SNMPManager.Reset()
			SNMPManager.Active = False
			SNMPManager.Config("ForceLocalPort=False")
		Catch ex As Exception

		End Try

		Try
			Dim myRegistry As New RegistryHandler
			SNMPManager.LocalPort = CType(myRegistry.ReadFromRegistry("SNMP Port"), Integer)
			myRegistry = Nothing
		Catch ex As Exception
			SNMPManager.LocalPort = 161
		End Try

		Try

			SNMPManager.RemoteHost = BES_Server.IPAddress
			SNMPManager.Community = BES_Server.SNMP_Community

            SNMPManager.Timeout = 45
			SNMPManager.Active = True
			WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " SNMP will contact " & BES_Server.Name & " on " & BES_Server.IPAddress)
			WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " SNMP will contact " & BES_Server.Name & " using the community " & BES_Server.SNMP_Community)
		Catch ex As Exception
			WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " Error creating SNMP Manager in BES Server module: " & ex.Message)
			BES_Server.Status = "SNMP Error"
			BES_Server.IsUp = False
			SNMPManager.Active = False
			SNMPManager.Dispose()
			BES_Server.LastScan = Date.Now.AddMinutes(-BES_Server.ScanInterval)
			Exit Sub
		End Try


		'************************* Pending Messages
		Try
			SNMPManager.Reset()
			SNMPManager.ObjCount = 1
			'SNMPManager.ObjId(1) = "1.3.6.1.4.1.3530.5.25.1.25.1" 'Pending messages
			SNMPManager.ObjId(1) = ".1.3.6.1.4.1.3530.6.7.10.60.2.1.2"	'Pending messages
			SNMPManager.ObjId(1) = ".1.3.6.1.4.1.3530.6.7.10.60.2.1"  'Pending messages
			SNMPManager.ObjId(1) = ".1.3.6.1.4.1.3530.6.7.10.60.2.1.2"	'Pending messages
			SNMPManager.SendGetNextRequest()
			SNMPManager.DoEvents()
		Catch ex As Exception
			' MessageBox.Show("Error getting pending messages: " & ex.ToString)
		End Try




		Try
			'Pending Messages

			Try
				BES_Server.PendingMessages = SNMPManager.ObjValue(1)
				WriteAuditEntry(Now.ToString & " BES Server " & BES_Server.Name & " has " & SNMPManager.ObjValue(1) & " Pending messages.")
			Catch ex As Exception
				' WriteDeviceHistoryEntry("BES_Server", MyDevice.Name, "Error parsing pending messages: " & ex.Message)
				BES_Server.PendingMessages = 0
			End Try


			If BES_Server.PendingMessages > 1400000000 Then
				BES_Server.PendingMessages = 0
				'Yes this is a hack.  If the server has no pending messages, sometimes GetNext returns a Unix Time stamp.
			End If

			If BES_Server.PendingMessages >= BES_Server.BES_Pending_Messages_Threshold And BES_Server.BES_Pending_Messages_Threshold > 0 Then
				BES_Server.AlertCondition = True
				'MyDevice.IncrementDownCount()
				'MyDevice.AlertType = NotResponding
				BES_Server.ResponseDetails = BES_Server.PendingMessages & " pending messages.  The alert threshold is " & BES_Server.BES_Pending_Messages_Threshold & "."
				BES_Server.Description = BES_Server.PendingMessages & " pending messages.  The alert threshold is " & BES_Server.BES_Pending_Messages_Threshold & "."

				BES_Server.Status = "Pending Messages"
				myAlert.QueueAlert("BES", BES_Server.Name, "Pending Messages", "BlackBerry Enterprise Server " & BES_Server.Name & " has " & BES_Server.PendingMessages & " pending messages.  The alert threshold is " & BES_Server.BES_Pending_Messages_Threshold & ".", BES_Server.Location)
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " Exception getting pending messages " & ex.ToString)
            'BES_Server.LastScan = Date.Now.AddMinutes(-BES_Server.ScanInterval)
		End Try





		'************************* BES License
		Try
			SNMPManager.Reset()
			'  SNMPManager.ObjValue(1) = ""
			SNMPManager.ObjCount = 1
			SNMPManager.ObjId(1) = ".1.3.6.1.4.1.3530.6.7.10.60.16.1.2.1.1"	'
			SNMPManager.SendGetRequest()
			SNMPManager.DoEvents()
			BES_Server.LicensesUsed = SNMPManager.ObjValue(1)
			Dim mySQL As String = "Update Status Set LicensesUsed = '" & BES_Server.LicensesUsed & "' WHERE TypeANDName='" & BES_Server.Name & "-BlackBerry Server'"
			UpdateStatusTable(mySQL)
		Catch ex As Exception
            SNMPManager.ObjValue(1) = ""

		End Try


		'********************* Sent Messages 
		Try
			SNMPManager.Reset()
			SNMPManager.ObjCount = 1
			If SNMPManager.ObjValue(1) = "" Then
				'  SNMPManager.ObjId(1) = "1.3.6.1.4.1.3530.5.25.1.203.1" 'Sent messages
				'  SNMPManager.ObjId(1) = "1.3.6.1.4.1.3530.6.7.10.60.4.1.2.1.2" 'Sent messages
				SNMPManager.ObjId(1) = "1.3.6.1.4.1.3530.6.7.10.60.4.1.2.1.1" 'Sent messages
				SNMPManager.SendGetNextRequest()
				SNMPManager.DoEvents()
				BES_Server.SNMP_Failure_Count = 0
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " Exception getting sent messages " & ex.ToString)
			BES_Server.SNMP_Failure_Count += 1	  ' MessageBox.Show("Error getting sent messages: " & ex.ToString)
		End Try

		Try
			If InStr(SNMPManager.ObjValue(1), "Error") > 0 Then
				Try
					SNMPManager.Reset()
					SNMPManager.ObjCount = 1
					SNMPManager.ObjId(1) = "1.3.6.1.4.1.3530.5.3.0"	'Sent messages
					SNMPManager.SendGetRequest()
					SNMPManager.DoEvents()
				Catch ex As Exception
					' MessageBox.Show("Error getting sent messages: " & ex.ToString)
				End Try
			End If
		Catch ex As Exception

		End Try

		If InStr(SNMPManager.ObjValue(1), "Error") > 0 Then
			BES_Server.BES_Total_Messages_Sent = 0
			BES_Server.SNMP_Failure_Count += 1
		Else
			Try
				BES_Server.BES_Total_Messages_Sent = SNMPManager.ObjValue(1)
			Catch ex As Exception
				WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " Error parsing total messages sent: " & ex.Message)
			End Try
		End If

		Try
			If BES_Server.BES_Total_Messages_Sent > 1400000000 Then
				BES_Server.BES_Total_Messages_Sent = 0
				'Yes this is a hack.  If the server has no pending messages, sometimes GetNext returns a Unix Time stamp.
			End If

		Catch ex As Exception

		End Try

		'********************* Received Messages 


		Try
			SNMPManager.Reset()
			SNMPManager.ObjCount = 1
			'  SNMPManager.ObjId(1) = "1.3.6.1.4.1.3530.5.25.1.204.1" 'Received messages
			'   SNMPManager.ObjId(1) = "1.3.6.1.4.1.3530.6.7.10.60.6.1.2.1.1" 'Received messages
			SNMPManager.ObjId(1) = "1.3.6.1.4.1.3530.6.7.10.60.6.1.2.1"	'Received messages

			SNMPManager.SendGetNextRequest()
			SNMPManager.DoEvents()

			If InStr(SNMPManager.ObjValue(1), "Error") > 0 Then
				Try
					SNMPManager.Reset()
					SNMPManager.ObjCount = 1
					SNMPManager.ObjId(1) = "1.3.6.1.4.1.3530.5.4.0"	'Received messages
					SNMPManager.SendGetRequest()
					SNMPManager.DoEvents()
				Catch ex As Exception
					BES_Server.SNMP_Failure_Count += 1
					' MessageBox.Show("Error getting received messages: " & ex.ToString)
				End Try
			End If

		Catch ex As Exception
			'  MessageBox.Show("Error getting received messages: " & ex.ToString)
		End Try

		Try
			If InStr(SNMPManager.ObjValue(1), "Error") > 0 Then
				BES_Server.BES_Total_Messages_Rcvd = 0
			Else
				Try
					BES_Server.BES_Total_Messages_Rcvd = SNMPManager.ObjValue(1)
				Catch ex As Exception
					WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " Error parsing total messages received: " & ex.Message)
				End Try
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " Exception getting received messages " & ex.ToString)
			BES_Server.SNMP_Failure_Count += 1
		End Try

		Try
			If BES_Server.BES_Total_Messages_Rcvd > 1400000000 Then
				BES_Server.BES_Total_Messages_Rcvd = 0
				'Yes this is a hack.  If the server has no pending messages, sometimes GetNext returns a Unix Time stamp.
			End If
		Catch ex As Exception

		End Try

		'************************* Expired Messages


		'Expired messages is no longer supported by VitalSigns, based on customer feedback
		BES_Server.BES_Total_Messages_Xpired = 0


		'************************* Filtered Messages

		SNMPManager.Reset()
		SNMPManager.ObjCount = 1
		Try
			SNMPManager.ObjId(1) = "1.3.6.1.4.1.3530.6.7.10.60.10.1.2"
			SNMPManager.SendGetNextRequest()
			SNMPManager.DoEvents()
		Catch ex As Exception
			'  MessageBox.Show("Error getting received messages: " & ex.ToString)
		End Try

		If InStr(SNMPManager.ObjValue(1), "Error") > 0 Then
			Try
				SNMPManager.ObjCount = 1
				SNMPManager.ObjId(1) = "1.3.6.1.4.1.3530.5.6.0"	'filtered messages
				SNMPManager.SendGetRequest()
				SNMPManager.DoEvents()

			Catch ex As Exception
				'  MessageBox.Show("Error getting filtered messages: " & ex.ToString)
			End Try
		End If


		'Total Filtered
		If InStr(SNMPManager.ObjValue(1), "Error") > 0 Then
			BES_Server.BES_Total_Messages_Filtered = 0
		Else
			Try
				BES_Server.BES_Total_Messages_Filtered = SNMPManager.ObjValue(1)
			Catch ex As Exception
				WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " Error parsing total messages received: " & ex.Message)
			End Try
		End If



        ''********************* Avg Per Minute 
        'Try
        '	SNMPManager.ObjCount = 1
        '	SNMPManager.ObjId(1) = "1.3.6.1.4.1.3530.5.7.0"	'Average per minute 
        '	SNMPManager.SendGetRequest()
        '	SNMPManager.DoEvents()
        '      Catch ex As Exception
        '          BES_Server.SNMP_Failure_Count += 1
        '      End Try

        'Try
        '	If SNMPManager.ObjValue(1) = "" Then
        '		SNMPManager.ObjId(1) = ".1.3.6.1.4.1.3530.6.7.10.60.12.1.2.1.1"
        '		SNMPManager.SendGetNextRequest()
        '		SNMPManager.DoEvents()
        '	End If
        'Catch ex As Exception
        '          BES_Server.SNMP_Failure_Count += 1
        'End Try
        ''
        'Try
        '	If InStr(SNMPManager.ObjValue(1), "Error") > 0 Then
        '		BES_Server.BES_Total_Mgs_Sent_PerMinute = 0
        '		'Me.txtAvg.Text = "n/a"
        '	Else
        '		Try
        '			BES_Server.BES_Total_Mgs_Sent_PerMinute = SNMPManager.ObjValue(1)
        '		Catch ex As Exception
        '			WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " Error parsing total messages sent per minute: " & ex.Message)
        '		End Try
        '	End If

        '	Try
        '		If BES_Server.BES_Total_Mgs_Sent_PerMinute > 1400000000 Then
        '			BES_Server.BES_Total_Mgs_Sent_PerMinute = 0
        '			'Yes this is a hack.  If the server has no pending messages, sometimes GetNext returns a Unix Time stamp.
        '		End If
        '	Catch ex As Exception

        '	End Try

        '	BES_Server.ResponseDetails += vbCrLf + "BES Messages/Minute: " & BES_Server.BES_Total_Mgs_Sent_PerMinute & vbCrLf & "Received: " & BES_Server.BES_Total_Messages_Rcvd & "   Total Sent: " & BES_Server.BES_Total_Messages_Sent & " "

        'Catch ex As Exception
        '	WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " Exception getting sent messages " & ex.ToString)
        'End Try



		'********************* Machine Name 
		Try
			SNMPManager.ObjCount = 1
			SNMPManager.ObjId(1) = "1.3.6.1.2.1.1.1.0"	'Machine name
			SNMPManager.SendGetRequest()
			SNMPManager.DoEvents()

			If InStr(SNMPManager.ObjValue(1), "Error") > 0 Then
				BES_Server.BES_ServerName = BES_Server.Name
				BES_Server.SNMP_Failure_Count += 1
				BES_Server.Status = "Not Responding"
				WriteAuditEntry(Now.ToString & " SNMP Error contacting " & BES_Server.Name & ".  Error is " & SNMPManager.ObjValue(1))
			Else
				BES_Server.BES_ServerName = SNMPManager.ObjValue(1)
				myAlert.ResetAlert("BES", BES_Server.Name, "Not Responding", BES_Server.Location)
				BES_Server.AlertCondition = False
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " Exception getting sent messages " & ex.ToString)
			BES_Server.SNMP_Failure_Count += 1
		End Try


		If BES_Server.SNMP_Failure_Count > 1 Then
			BES_Server.AlertCondition = True
			BES_Server.Status = "Timeout"
			BES_Server.IncrementDownCount()
			BES_Server.ResponseDetails = "BES Server is not responding to SNMP Queries."
			WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " BES Server " & BES_Server.Name & " is not responding to SNMP Queries.")
			myAlert.QueueAlert("BES", BES_Server.Name, "Timeout", "BlackBerry Enterprise Server " & BES_Server.Name & " is not responding to SNMP queries.", BES_Server.Location)
			BES_Server.LastScan = Date.Now.AddMinutes(-BES_Server.ScanInterval)
			Try
				SNMPManager.Active = False
				SNMPManager.Dispose()
			Catch ex As Exception

			End Try
			Exit Sub
		Else
			myAlert.ResetAlert("BES", BES_Server.Name, "Timeout", BES_Server.Location)
		End If



		'Now loop through all the services running on the remote server
		Try
			SNMPManager.Reset()
			SNMPManager.ObjCount = 1
			SNMPManager.ObjId(1) = "1.3.6.1.4.1.77.1.2.3"  'top OID for running services
		Catch ex As Exception

		End Try

		Dim n As Integer
		Dim val As String

		Do While SNMPManager.ErrorIndex = 0
			Try
				SNMPManager.SendGetNextRequest()
			Catch ex As nsoftware.IPWorks.IPWorksSnmpmgrException
				WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " SNMP Error querying Windows services for " & BES_Server.Name & "  Details: " & ex.Message)
				BES_Server.ResponseDetails += vbCrLf & "SNMP error while looping through Windows services " & ex.Message

				GoTo CleanUp
			Catch ex As Exception
				WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " SNMP Error querying Windows services for " & BES_Server.Name & "  Details: " & ex.Message)
				GoTo CleanUp
			End Try

			Try
				val = SNMPManager.ObjValue(1)

				If IsNumeric(SNMPManager.ObjValue(1)) Then
					Exit Do
				End If

			Catch ex As Exception

			End Try
			Try
				BES_Server.Windows_Services += SNMPManager.ObjValue(1) & vbCrLf
				n += 1
				If n = 100 Then
					Exit Do
				End If
			Catch ex As Exception

			End Try

		Loop


		WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToShortDateString & "The BES server " & BES_Server.Name & " is running the following services: " & vbCrLf & BES_Server.Windows_Services & vbCrLf)

		If BES_Server.MessagingService = True Then
			If Not (InStr(BES_Server.Windows_Services, "Messaging") > 0) And Not (InStr(BES_Server.Windows_Services, "Router") > 0) Then
				'Messaging Service is not found in the services table
				BES_Server.Status = "Messaging Service Failure"
				myAlert.QueueAlert("BES", BES_Server.Name, "Messaging Service not found", ProductName & " has scanned the Windows services table on " & BES_Server.Name & " and the BlackBerry Messaging Service was not found.", BES_Server.Location)
				BES_Server.IncrementDownCount()
				Try
					SNMPManager.Active = False
					SNMPManager.Dispose()
				Catch ex As Exception

				End Try
				Exit Sub
			Else
				myAlert.ResetAlert("BES", BES_Server.Name, "Messaging Service not found", BES_Server.Location)
			End If
		End If

		If BES_Server.ControllerService = True Then
			If Not (InStr(BES_Server.Windows_Services, "Controller") > 0) Then
				'Controller Service is not found in the services table
				BES_Server.Status = "Controller Service Failure"
				myAlert.QueueAlert("BES", BES_Server.Name, "BlackBerry Controller Service not found", ProductName & " has scanned the Windows services table on " & BES_Server.Name & " and the BlackBerry Controller Service was not found.", BES_Server.Location)
				BES_Server.IncrementDownCount()
				Try
					SNMPManager.Active = False
					SNMPManager.Dispose()
				Catch ex As Exception

				End Try
				Exit Sub
			Else
				myAlert.ResetAlert("BES", BES_Server.Name, "BlackBerry Controller Service not found", BES_Server.Location)
			End If
		End If

		If BES_Server.DispatcherService = True Then
			If Not (InStr(BES_Server.Windows_Services, "BlackBerry Dispatcher") > 0) Then
				'Dispatcher Service is not found in the services table
				BES_Server.Status = "Dispatcher Service Failure"
				myAlert.QueueAlert("BES", BES_Server.Name, "BlackBerry Dispatcher Service not found", ProductName & " has scanned the Windows services table on " & BES_Server.Name & " and the BlackBerry Dispatcher Service was not found.", BES_Server.Location)
				BES_Server.IncrementDownCount()
				Try
					SNMPManager.Active = False
					SNMPManager.Dispose()
				Catch ex As Exception

				End Try
				Exit Sub
			Else
				myAlert.ResetAlert("BES", BES_Server.Name, "BlackBerry Dispatcher Service not found", BES_Server.Location)
			End If
		End If

		If BES_Server.SynchronizationService = True And BES_Server.Connected_To_SRP = True Then
			If Not (InStr(BES_Server.Windows_Services, "BlackBerry Synchronization") > 0) Then
				'Dispatcher Service is not found in the services table
				BES_Server.Status = "Synchronization Service Failure"
				myAlert.QueueAlert("BES", BES_Server.Name, "BlackBerry Synchronization Service not found", ProductName & " has scanned the Windows services table on " & BES_Server.Name & " and the BlackBerry Dispatcher Service was not found.", BES_Server.Location)
				BES_Server.IncrementDownCount()
				Try
					SNMPManager.Active = False
					SNMPManager.Dispose()
				Catch ex As Exception

				End Try
				Exit Sub
			Else
				myAlert.ResetAlert("BES", BES_Server.Name, "BlackBerry Synchronization Service not found", BES_Server.Location)
			End If
		End If

		If BES_Server.PolicyService = True And BES_Server.Connected_To_SRP = True Then
			If Not (InStr(BES_Server.Windows_Services, "BlackBerry Policy") > 0) Then
				'Dispatcher Service is not found in the services table
				BES_Server.Status = "Policy Service Failure"
				myAlert.QueueAlert("BES", BES_Server.Name, "BlackBerry Policy Service not found", ProductName & " has scanned the Windows services table on " & BES_Server.Name & " and the BlackBerry Policy Service was not found.", BES_Server.Location)
				BES_Server.IncrementDownCount()
				Try
					SNMPManager.Active = False
					SNMPManager.Dispose()
				Catch ex As Exception

				End Try
				Exit Sub
			Else
				myAlert.ResetAlert("BES", BES_Server.Name, "BlackBerry Policy Service not found", BES_Server.Location)
			End If
		End If



		'The following services could be running on other machines

		If BES_Server.AlertService = True Then
			If BES_Server.Alert_Service_Address <> BES_Server.IPAddress Then
				Try
					SNMPManager.Reset()
					SNMPManager.RemoteHost = BES_Server.Alert_Service_Address
					SNMPManager.Community = BES_Server.SNMP_Community
					SNMPManager.Timeout = 60
				Catch ex As Exception
					WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & "Error creating SNMP Manager: " & ex.Message)
				End Try

				Try
					SNMPManager.ObjCount = 1
					SNMPManager.ObjId(1) = "1.3.6.1.4.1.77.1.2.3"
				Catch ex As Exception

				End Try
				n = 1
				val = ""
				Do While SNMPManager.ErrorIndex = 0
					Try
						SNMPManager.SendGetNextRequest()
					Catch ex As nsoftware.IPWorks.IPWorksSnmpmgrException
						'  WriteAuditEntry(Now.ToString & " SNMP Error querying Windows services for " & MyDevice.Name & "  Details: " & ex.Message)
						Exit Do
					Catch ex As Exception
						' WriteDeviceHistoryEntry("BES_Server", MyDevice.Name, Now.ToString & " SNMP Error querying Windows services for " & MyDevice.Name & "  Details: " & ex.Message)

						Exit Do
					End Try

					val = SNMPManager.ObjValue(1)

					If IsNumeric(SNMPManager.ObjValue(1)) Then
						Exit Do
					End If

					BES_Server.Windows_Services += SNMPManager.ObjValue(1) & vbCrLf
					n += 1
					If n = 70 Then
						Exit Do
					End If

				Loop

				If Not (InStr(BES_Server.Windows_Services, "BlackBerry Alert") > 0) Then
					'Alert Service is not found in the services table
					myAlert.QueueAlert("BES", BES_Server.Name, "Alert Service not found", ProductName & " has scanned the Windows services table on " & BES_Server.Name & " and the BlackBerry Alert Service was not found.", BES_Server.Location)
					BES_Server.Status = "Alert Service Failure"
					BES_Server.IncrementDownCount()
					Try
						SNMPManager.Active = False
						SNMPManager.Dispose()
					Catch ex As Exception

					End Try
					Exit Sub
				Else
					myAlert.ResetAlert("BES", BES_Server.Name, "Alert Service not found", BES_Server.Location)
				End If

			Else
				'The Alert service is running on the same machine as the BES server, so just check the existing services table

				If Not (InStr(BES_Server.Windows_Services, "BlackBerry Alert") > 0) Then
					'Alert Service is not found in the services table
					myAlert.QueueAlert("BES", BES_Server.Name, "Alert Service not found", ProductName & " has scanned the Windows services table on " & BES_Server.Name & " and the BlackBerry Alert Service was not found.", BES_Server.Location)
					BES_Server.Status = "Alert Service Failure"
					BES_Server.IncrementDownCount()
					Try
						SNMPManager.Active = False
						SNMPManager.Dispose()
					Catch ex As Exception

					End Try
					Exit Sub
				Else
					myAlert.ResetAlert("BES", BES_Server.Name, "Alert Service not found", BES_Server.Location)
				End If
			End If


		End If

		'** Attachment service

		If BES_Server.AttachmentService = True Then
			If BES_Server.Attachment_Service_Address <> BES_Server.IPAddress Then
				Try
					SNMPManager.Reset()
					SNMPManager.RemoteHost = BES_Server.Attachment_Service_Address
					SNMPManager.Community = BES_Server.SNMP_Community
					SNMPManager.Timeout = 60
				Catch ex As Exception
					WriteAuditEntry(Now.ToString & "Error creating SNMP Manager: " & ex.Message)
				End Try

				Try
					SNMPManager.ObjCount = 1
					SNMPManager.ObjId(1) = "1.3.6.1.4.1.77.1.2.3"
				Catch ex As Exception

				End Try
				n = 1
				val = ""
				Do While SNMPManager.ErrorIndex = 0
					Try
						SNMPManager.SendGetNextRequest()
					Catch ex As nsoftware.IPWorks.IPWorksSnmpmgrException
						WriteAuditEntry(Now.ToString & "SNMP Error with Attachment Service- querying remote machine's Windows services for " & BES_Server.Name & "  Details: " & ex.Message)
						Exit Do
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & "SNMP Error with Attachment Service- querying remote Windows services for " & BES_Server.Name & "  Details: " & ex.Message)
						WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & "SNMP Error with Attachment Service- querying remote Windows services for " & BES_Server.Name & "  Details: " & ex.Message)
						Exit Do
					End Try

					val = SNMPManager.ObjValue(1)

					If IsNumeric(SNMPManager.ObjValue(1)) Then
						Exit Do
					End If

					BES_Server.Windows_Services += SNMPManager.ObjValue(1) & vbCrLf
					n += 1
					If n = 70 Then
						Exit Do
					End If

				Loop

				If Not (InStr(BES_Server.Windows_Services, "BlackBerry Attachment") > 0) Then
					'Alert Service is not found in the services table
					myAlert.QueueAlert("BES", BES_Server.Name, "Attachment Service not found", ProductName & " has scanned the Windows services table on " & BES_Server.Name & " and the BlackBerry Attachment Service was not found.", BES_Server.Location)
					BES_Server.Status = "Attachment Service Failure"
					BES_Server.IncrementDownCount()
					Try
						SNMPManager.Active = False
						SNMPManager.Dispose()
					Catch ex As Exception

					End Try
					Exit Sub
				Else
					myAlert.ResetAlert("BES", BES_Server.Name, "Attachment Service not found", BES_Server.Location)
				End If

			Else
				'The Alert service is running on the same machine as the BES server, so just check the existing services table

				If Not (InStr(BES_Server.Windows_Services, "BlackBerry Attachm") > 0) Then
					'Alert Service is not found in the services table
					myAlert.QueueAlert("BES", BES_Server.Name, "Attachment Service not found", ProductName & " has scanned the Windows services table on " & BES_Server.Name & " and the BlackBerry Attachment Service was not found.", BES_Server.Location)
					BES_Server.Status = "Attachment Service Failure"
					BES_Server.IncrementDownCount()
					Try
						SNMPManager.Active = False
						SNMPManager.Dispose()
					Catch ex As Exception

					End Try
					Exit Sub
				Else
					myAlert.ResetAlert("BES", BES_Server.Name, "Attachment Service not found", BES_Server.Location)
				End If
			End If

		End If

		'** Router Service

		If BES_Server.RouterService = True Then
			If BES_Server.Router_Service_Address <> BES_Server.IPAddress Then
				Try
					SNMPManager.Reset()
					SNMPManager.RemoteHost = BES_Server.Router_Service_Address
					SNMPManager.Community = BES_Server.SNMP_Community
					SNMPManager.Timeout = 60
				Catch ex As Exception
					WriteAuditEntry(Now.ToString & "Error configuring SNMP Manager: " & ex.Message)
				End Try

				Try
					SNMPManager.ObjCount = 1
					SNMPManager.ObjId(1) = "1.3.6.1.4.1.77.1.2.3"
				Catch ex As Exception

				End Try
				n = 1
				val = ""
				Do While SNMPManager.ErrorIndex = 0
					Try
						SNMPManager.SendGetNextRequest()
					Catch ex As nsoftware.IPWorks.IPWorksSnmpmgrException
						WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & "SNMP Error with Router Service- querying remote machine's Windows services for " & BES_Server.Name & "  Details: " & ex.Message)
						Exit Do
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & "SNMP Error with Router Service- querying remote Windows services for " & BES_Server.Name & "  Details: " & ex.Message)

						Exit Do
					End Try

					val = SNMPManager.ObjValue(1)

					If IsNumeric(SNMPManager.ObjValue(1)) Then
						Exit Do
					End If

					BES_Server.Windows_Services += SNMPManager.ObjValue(1) & vbCrLf
					n += 1
					If n = 70 Then
						Exit Do
					End If

				Loop

				If Not (InStr(BES_Server.Windows_Services, "BlackBerry Router") > 0) Then
					'Alert Service is not found in the services table
					myAlert.QueueAlert("BES", BES_Server.Name, "Router Service not found", ProductName & " has scanned the Windows services table on " & BES_Server.Name & " and the BlackBerry Attachment Service was not found.", BES_Server.Location)
					BES_Server.Status = "Router Service Failure"
					BES_Server.IncrementDownCount()
					Try
						SNMPManager.Active = False
						SNMPManager.Dispose()
					Catch ex As Exception

					End Try
					Exit Sub
				Else
					myAlert.ResetAlert("BES", BES_Server.Name, "Router Service not found", BES_Server.Location)
				End If

			Else
				'The Router service is running on the same machine as the BES server, so just check the existing services table

				If Not (InStr(BES_Server.Windows_Services, "BlackBerry Router") > 0) Then
					'Alert Service is not found in the services table
					myAlert.QueueAlert("BES", BES_Server.Name, "Router Service not found", ProductName & " has scanned the Windows services table on " & BES_Server.Name & " and the BlackBerry Attachment Service was not found.", BES_Server.Location)
					BES_Server.Status = "Router Service Failure"
					BES_Server.IncrementDownCount()
					Try
						SNMPManager.Active = False
						SNMPManager.Dispose()
					Catch ex As Exception

					End Try
					Exit Sub
				Else
					myAlert.ResetAlert("BES", BES_Server.Name, "Router Service not found", BES_Server.Location)
				End If
			End If

		End If

CleanUp:

		Try
			SNMPManager.Active = False
			SNMPManager.Dispose()
		Catch ex As Exception

		End Try
		BES_Server.IncrementUpCount()
	End Sub


	Private Sub TrackBESMailStatistics(ByRef BES_Server As MonitoredItems.BlackBerryServer)
		'This sub calculates the daily volume of mail traffic

		WriteAuditEntry(Now.ToString & " " & BES_Server.Name & ": previous value of BES_Total_Messages_Sent " & BES_Server.BES_Total_Messages_Sent)

		Dim Total_Recvd_CurrentStatValue, Total_Sent_CurrentStatValue, Total_Expired_CurrentStatValue, Total_Filtered_CurrentStatValue As Long
		Dim ReceivedMail, SentMail, ExpiredMail, FilteredMessages As Long
		'Xpired, Sent, Received, Filtered

		'Filtered
		Try
			If BES_Server.BES_Total_Messages_Filtered <> 0 Then
				Total_Filtered_CurrentStatValue = BES_Server.BES_Total_Messages_Filtered

				If BES_Server.PreviousMessagesFiltered <> -1 Then
					FilteredMessages = Total_Recvd_CurrentStatValue - BES_Server.PreviousMessagesReceived
					WriteAuditEntry(Now.ToString & " " & BES_Server.Name & " current value of calculated filtered mail is " & FilteredMessages)

					If FilteredMessages < 0 Then
						FilteredMessages = 0
					End If
					BES_Server.PreviousMessagesFiltered = Total_Filtered_CurrentStatValue
				Else
					BES_Server.PreviousMessagesFiltered = Total_Filtered_CurrentStatValue
					FilteredMessages = 0
				End If

			End If
			UpdateBESDailyStatTable(BES_Server.Name, "BES_Messages_Filtered", FilteredMessages)

		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " " & BES_Server.Name & ": Error calculating BES_Messages_Filtered " & ex.Message)
		End Try



		'Expired
		'Try
		'    If BES_Server.BES_Total_Messages_Xpired <> 0 Then
		'        Total_Expired_CurrentStatValue = BES_Server.BES_Total_Messages_Rcvd

		'        If BES_Server.PreviousMessagesReceived <> -1 Then
		'            ExpiredMail = Total_Recvd_CurrentStatValue - BES_Server.PreviousMessagesReceived
		'            WriteAuditEntry(Now.ToString & " " & BES_Server.Name & " current value of calculated expired mail is " & ExpiredMail)

		'            If ExpiredMail < 0 Then
		'                ExpiredMail = 0
		'            End If
		'            BES_Server.PreviousMessagesExpired = Total_Expired_CurrentStatValue
		'        Else
		'            BES_Server.PreviousMessagesExpired = Total_Expired_CurrentStatValue
		'            ExpiredMail = 0
		'        End If

		'    End If
		'    UpdateBESDailyStatTable(BES_Server.Name, "BES_Messages_Expired", ExpiredMail)

		'Catch ex As Exception
		'    WriteAuditEntry(Now.ToString & " " & BES_Server.Name & ": Error calculating BES_Messages_Xpired " & ex.Message)
		'End Try


		'Received
		Try
			If BES_Server.BES_Total_Messages_Rcvd <> 0 Then
				Total_Recvd_CurrentStatValue = BES_Server.BES_Total_Messages_Rcvd

				If BES_Server.PreviousMessagesReceived <> -1 Then
					ReceivedMail = Total_Recvd_CurrentStatValue - BES_Server.PreviousMessagesReceived
					WriteAuditEntry(Now.ToString & " " & BES_Server.Name & " current value of calculated received mail is " & ReceivedMail)

					If ReceivedMail < 0 Then
						ReceivedMail = 0
					End If
					BES_Server.PreviousMessagesReceived = Total_Recvd_CurrentStatValue
				Else
					BES_Server.PreviousMessagesReceived = Total_Recvd_CurrentStatValue
					ReceivedMail = 0
				End If

			End If
			UpdateBESDailyStatTable(BES_Server.Name, "BES_Messages_Received", ReceivedMail)

		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " " & BES_Server.Name & ": Error calculating BES_Total_Messages_Rcvd " & ex.Message)
		End Try

		'Sent
		Try
			If BES_Server.BES_Total_Messages_Sent <> 0 Then
				Total_Sent_CurrentStatValue = BES_Server.BES_Total_Messages_Sent

				If BES_Server.PreviousMessagesSent <> -1 Then
					SentMail = Total_Sent_CurrentStatValue - BES_Server.PreviousMessagesSent
					WriteAuditEntry(Now.ToString & " " & BES_Server.Name & " current value of calculated sent mail is " & SentMail)

					If SentMail < 0 Then
						SentMail = 0
					End If
					BES_Server.PreviousMessagesSent = Total_Sent_CurrentStatValue
				Else
					BES_Server.PreviousMessagesSent = Total_Sent_CurrentStatValue
					SentMail = 0
				End If

			End If
			UpdateBESDailyStatTable(BES_Server.Name, "BES_Messages_Sent", SentMail)

		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " " & BES_Server.Name & ": Error calculating BES_Messages_Sent " & ex.Message)
		End Try

	End Sub

	Private Function Get_SRP_Status(ByRef BES_Server As MonitoredItems.BlackBerryServer) As String
		'This returns whether server is connected to the SRP or not.  It can also return an error message
		Dim SNMPManager As New nsoftware.IPWorks.Snmpmgr("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000")
		Dim returnValue As String = ""
		'****************************
		'Check the status of the BlackBerry server by querying the BES MIB

		Try
			SNMPManager.Reset()
			SNMPManager.Active = False
			SNMPManager.Config("ForceLocalPort=False")
		Catch ex As Exception

		End Try

		Try
			Dim myRegistry As New RegistryHandler
			SNMPManager.LocalPort = CType(myRegistry.ReadFromRegistry("SNMP Port"), Integer)
			myRegistry = Nothing
		Catch ex As Exception
			SNMPManager.LocalPort = 161
		End Try

		Try

			SNMPManager.RemoteHost = BES_Server.IPAddress
			SNMPManager.Community = BES_Server.SNMP_Community

			SNMPManager.Timeout = 90
			SNMPManager.Active = True
			WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " SNMP will contact " & BES_Server.Name & " on " & BES_Server.IPAddress)
			WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " SNMP will contact " & BES_Server.Name & " using the community " & BES_Server.SNMP_Community)
		Catch ex As Exception
			WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " Error creating SNMP Manager in BES Server module: " & ex.Message)
			BES_Server.Status = "SNMP Error"
			BES_Server.IsUp = False
			SNMPManager.Active = False
			SNMPManager.Dispose()
			Return "ERROR: " & ex.ToString
			Exit Function
		End Try


		'********************* SRP Connection State

		Try
			SNMPManager.Reset()
			SNMPManager.ObjCount = 1
		Catch ex As Exception
			' MessageBox.Show("Error getting connection state: " & ex.ToString)
		End Try


		Try
			SNMPManager.ObjId(1) = ".1.3.6.1.4.1.3530.6.7.35.120.15.1.2.1.15.1"
			'this is the value used in the vs client rather than the longer one above
			SNMPManager.ObjId(1) = ".1.3.6.1.4.1.3530.6.7.15.120.15.1.2"
			SNMPManager.ObjId(1) = ".1.3.6.1.4.1.3530.6.7.15.120.15"
			SNMPManager.ObjId(1) = ".1.3.6.1.4.1.3530.6.7.15.120.15.1.2.1.5"
			SNMPManager.SendGetNextRequest()
			SNMPManager.DoEvents()
		Catch ex As Exception
			'*************  Assume that it is connected to avoid false alerts
			BES_Server.Connected_To_SRP = True
		End Try

        'SRP Connection
        Try
            If SNMPManager.ObjValue(1) <> 1 Then
                SNMPManager.ObjId(1) = ".1.3.6.1.4.1.3530.6.7.35.120.15.1.2.1.15.1"
                SNMPManager.SendGetNextRequest()
                SNMPManager.DoEvents()
            End If
        Catch ex As Exception

        End Try


        'SRP Connection
        Try
            If SNMPManager.ObjValue(1) <> 1 Then
                SNMPManager.ObjId(1) = ".1.3.6.1.4.1.3530.6.7.15.120.15.1.2"
                SNMPManager.SendGetNextRequest()
                SNMPManager.DoEvents()
            End If
        Catch ex As Exception

        End Try


		Dim mySQL As String = ""
		Try

			If SNMPManager.ObjValue(1) <> 1 Then
				BES_Server.Connected_To_SRP = False
				mySQL = "Update Status Set SRPConnectionn = 'Not Connected' WHERE TypeANDName='" & BES_Server.Name & "-BlackBerry Server'"
				WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " BES Server reports that it is NOT connected to the SRP.")
				WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, mySQL)
			Else
				BES_Server.Connected_To_SRP = True
				WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, Now.ToString & " BES Server reports that it IS connected to the SRP.")
				BES_Server.Description = " BES Server reports that it is CONNECTED to the SRP at " & Date.Now.ToShortTimeString
				mySQL = "Update Status Set SRPConnectionn = 'Connected' WHERE TypeANDName='" & BES_Server.Name & "-BlackBerry Server'"
				WriteDeviceHistoryEntry("BES_Server", BES_Server.Name, mySQL)
				'Connected
			End If
			UpdateStatusTable(mySQL)
		Catch ex As Exception
			' MyDevice.Connected_To_SRP = False
			BES_Server.Description = " BES Server reports that it is CONNECTED to the SRP at " & Date.Now.ToShortTimeString
		End Try

		Try
			SNMPManager.Active = False
			SNMPManager.Dispose()
		Catch ex As Exception

		End Try

		Return BES_Server.Connected_To_SRP
	End Function

#End Region

End Class
