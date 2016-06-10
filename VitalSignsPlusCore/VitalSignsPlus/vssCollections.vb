Imports System.Threading
Imports VSFramework

Partial Public Class VitalSignsPlusCore


#Region "Create Collections of items to Monitor"

    Public Sub CreateCollections()

        Try
            CreateBlackBerryServersCollection()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating BES Servers collection: " & ex.Message)
        End Try

        Try
            CreateMailServicesCollection()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Mail Services collection: " & ex.Message)
        End Try

        Try
            CreateSametimeServersCollection()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Sametime Servers collection: " & ex.Message)
        End Try

        Try
            CreateURLCollection()

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating URL collection: " & ex.Message)
        End Try

        '10/7/2014 NS added for VSPLUS-1002
        Try
            CreateCloudCollection()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Cloud URL collection: " & ex.Message)
		End Try

		Try
			CreateWebSphereCollection()
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Error creating WebSphere collection: " & ex.Message)
		End Try

		Try
			CreateIBMConnectCollection()
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Error creating IBM Connect collection: " & ex.Message)
		End Try


    End Sub


    Private Sub CreateBlackBerryServersCollection()

        'Connect to the data source
        Dim dsBlackBerryServers As New Data.DataSet

        Try

            Dim strSQL As String 
            'strSQL = " SELECT  srv.ServerName AS Name, srv.IPAddress, ScanInterval, Category, Enabled, IPAddress, Location, Loc.ID as LocationID,  OffHoursScanInterval, RetryInterval, SNMPCommunity, Messaging, Controller, Dispatcher, Synchronization, Policy, MDS, Attachment, Alert, Router, AlertIP, RouterIP, AttachmentIP, OtherServices, MDSConnection, BESVersion, MDSServices, TimeZoneAdjustment, USDateFormat, PendingThreshold, ExpiredThreshold, HAOption, srv.ServerName as HAPartner "
            'strSQL += " from dbo.BlackBerryServers  ds, dbo.Servers srv, dbo.ServerTypes srvt, dbo.Locations Loc"
            'strSQL += " where "
            'strSQL += " ds.ServerID = srv.ID And srv.ServerTypeID = srvt.id And srv.LocationID = Loc.ID AND ds.Enabled = 1"


			strSQL = " SELECT  srv.ServerName AS Name, srv.IPAddress, ScanInterval, ds.Category, Enabled, srv.IPAddress, Loc.Location,  Loc.ID as LocationID,  OffHoursScanInterval, RetryInterval, "
			strSQL += " SNMPCommunity, Messaging, Controller, Dispatcher,  Synchronization, Policy, MDS, Attachment, Alert, Router, AlertIP, RouterIP, AttachmentIP, OtherServices,  MDSConnection, "
			strSQL += " BESVersion, MDSServices, TimeZoneAdjustment, USDateFormat, ds.PendingThreshold, ExpiredThreshold,  HAOption, srv1.ServerName as HAPartner, srv.ID, st.LastUpdate, st.Status, "
			strSQL += " st.StatusCode, di.CurrentNodeID"
			strSQL += " from dbo.BlackBerryServers  ds "
			strSQL += " inner join dbo.Servers srv on srv.ID=ds.ServerID "
			strSQL += " left outer join  dbo.Servers srv1 on ds.HAPartner=srv1.ID"
			strSQL += " left outer join dbo.ServerTypes srvt on srvt.id=srv.ServerTypeID"
			strSQL += " left outer join dbo.Locations Loc on Loc.ID=srv.LocationID"

			If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then
				Dim NodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
				strSQL += " inner join DeviceInventory di on srv.ID=di.DeviceID and di.DeviceTypeId=srv.ServerTypeId  "
				strSQL += " inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" & NodeName & "'  "
			End If

			strSQL += " left outer join Status st on st.Name=srv.ServerName "
			strSQL += " and "
			strSQL += " st.Type=(select ServerType From ServerTypes where srv.ServerTypeId=ID)"

			strSQL += " where(ds.ServerID = srv.ID And srv.ServerTypeID = srvt.id And srv.LocationID = Loc.ID And ds.Enabled = 1)"

			WriteAuditEntry(Now.ToString & " SQL Statement is " & vbCrLf & strSQL)

			Dim objVSAdaptor As New VSAdaptor
			objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsBlackBerryServers, "BlackBerryServers")

		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateBlackBerryServersCollection processing code. Exception: " & ex.Message)
			Exit Sub
		End Try

        '***
        WriteAuditEntry(Now.ToString & " #1")

        Dim i As Integer = 0
		Try
			WriteAuditEntry(Now.ToString & "  Reading configuration settings for " & dsBlackBerryServers.Tables("BlackBerryServers").Rows.Count & " BlackBerry Servers.")
		Catch ex As Exception

		End Try

        'Add the BES servers to the collection
        Dim dr As DataRow

        'but first delete any that are in the collection but not in the database anymore
        'Delete propagation

        Dim MyServerNames As String = ""
        If MyBlackBerryServers.Count > 0 Then
            If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Checking to see if any BES servers should be deleted. ")
            'Get all the names of all the servers in the data table
            For Each dr In dsBlackBerryServers.Tables("BlackBerryServers").Rows()
                MyServerNames += dr.Item("Name") & "  "
            Next
        End If

        Dim BES As MonitoredItems.BlackBerryServer
        Dim myIndex As Integer

        If MyBlackBerryServers.Count > 0 Then
			For myIndex = MyBlackBerryServers.Count - 1 To 0 Step -1
				BES = MyBlackBerryServers.Item(myIndex)
				Try
					If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Checking to see if BlackBerry server " & BES.Name & " should be deleted...")
					If InStr(MyServerNames, BES.Name) > 0 Then
						'the server has not been deleted
						If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & BES.Name & " is not marked for deletion. ")
					Else
						'the server has been deleted, so delete from the collection
						Try
							MyBlackBerryServers.Delete(BES.Name)
							If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " " & BES.Name & " has been deleted by the service.")
						Catch ex As Exception
							WriteAuditEntry(Now.ToString & " " & BES.Name & " was not deleted by the service because " & ex.Message, LogLevel.Normal)
						End Try
					End If
				Catch ex As Exception
					WriteAuditEntry(Now.ToString & " Exception BES Deletion Loop on " & BES.Name & ".  The error was " & ex.Message, LogLevel.Normal)
				End Try
			Next
        End If

        '***
        WriteAuditEntry(Now.ToString & " #2")

        Try
			For Each dr In dsBlackBerryServers.Tables("BlackBerryServers").Rows()
				WriteAuditEntry(Now.ToString & " Processing " & dr.Item("Name"))
				i += 1
				Dim MyName As String
				If dr.Item("Name") Is Nothing Then
					MyName = "BlackBerry Server " & i.ToString
				Else
					MyName = dr.Item("Name")
				End If
				'See if this server is already in the collection; if so, update its settings otherwise create a new one
				myBlackBerryServer = MyBlackBerryServers.Search(MyName)
				If myBlackBerryServer Is Nothing Then
					myBlackBerryServer = New MonitoredItems.BlackBerryServer
					'myBlackBerryServer.LastScan = Now
					myBlackBerryServer.Name = MyName
					myBlackBerryServer.NextScan = Now
					myBlackBerryServer.IncrementUpCount()
					myBlackBerryServer.AlertCondition = False
					myBlackBerryServer.BES_Version = "Unknown"
					'myBlackBerryServer.Status = "Not Scanned"
					myBlackBerryServer.ResponseDetails = "This BES Server has not been scanned."
					myBlackBerryServer.BES_Version = "BlackBerry Enterprise Server for Domino"
					myBlackBerryServer.BES_ServerName = MyName
					'default values indicate that the server has not been scanned
					myBlackBerryServer.PreviousMessagesSent = -1
					myBlackBerryServer.PreviousMessagesReceived = -1
					myBlackBerryServer.PreviousMessagesFiltered = -1
					myBlackBerryServer.PreviousMessagesExpired = -1
					myBlackBerryServer.BES_Total_Mgs_Sent_PerMinute = -1
					Try
						If dr.Item("BESVersion") Is Nothing Then
							myBlackBerryServer.BES_Version = "Unknown"
						Else
							myBlackBerryServer.BES_Version = dr.Item("BESVersion")
						End If

					Catch ex As Exception
						myBlackBerryServer.BES_Version = "Unknown"
					End Try

					MyBlackBerryServers.Add(myBlackBerryServer)
					WriteAuditEntry(Now.ToString & " Adding new BlackBerry Server -- " & myBlackBerryServer.Name & " -- to the collection.")
				Else
					WriteAuditEntry(Now.ToString & " Updating settings for existing BlackBerry Server -- " & myBlackBerryServer.Name & ".")
				End If


				With myBlackBerryServer
					If MyLogLevel = LogLevel.Verbose Then
						WriteAuditEntry(Now.ToString & " Configuring BlackBerryServer Device: " & dr.Item("Name"))
						WriteAuditEntry(Now.ToString & " Status: " & myBlackBerryServer.Status)
						WriteAuditEntry(Now.ToString & " Enabled: " & myBlackBerryServer.Enabled)
						WriteAuditEntry(Now.ToString & " Next Scan: " & myBlackBerryServer.NextScan)
						WriteAuditEntry(Now.ToString & " Pending Messages: " & myBlackBerryServer.PendingMessages)
						WriteAuditEntry(Now.ToString & " Pending Threshold: " & myBlackBerryServer.BES_Pending_Messages_Threshold)
					End If

					Try
						.OffHours = BoolOffHours
					Catch ex As Exception
						.OffHours = False
					End Try

					Try
						If dr.Item("TimeZoneAdjustment") Is Nothing Then
							.TimeZoneAdjustment = 0
						Else
							.TimeZoneAdjustment = dr.Item("TimeZoneAdjustment")
						End If
					Catch ex As Exception
						.TimeZoneAdjustment = 0
					End Try

					Try
						If dr.Item("HAOption") Is Nothing Then
							.HAOption = "Stand Alone Server"
							'  WriteAuditEntry(Now.ToString & " " & .Name & " Domino server Pending Mail threshold not set, using default of 50.")
						Else
							.HAOption = dr.Item("HAOption")
							WriteAuditEntry(Now.ToString & " " & .Name & " server is  " & .HAOption)
						End If
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " " & .Name & "  Exception setting HA option " & ex.ToString, LogLevel.Normal)
					End Try

					Try
						If dr.Item("HAPartner") Is Nothing Then
							.HAPartner = ""
							WriteAuditEntry(Now.ToString & " BES server " & .Name & " does not have an HA partner.")
						Else
							.HAPartner = dr.Item("HAPartner")
							WriteAuditEntry(Now.ToString & " BES server " & .Name & " HA partner is  " & .HAPartner)
						End If
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " " & .Name & "  Exception setting HA partner " & ex.ToString, LogLevel.Normal)
					End Try


					Try
						If dr.Item("USDateFormat") Is Nothing Then
							.USDateFormat = True
						Else
							.USDateFormat = dr.Item("USDateFormat")
						End If
					Catch ex As Exception
						.USDateFormat = True
					End Try

					Try
						If dr.Item("ExpiredThreshold") Is Nothing Then
							.BES_Expired_Messages_Theshold = 10000
						Else
							.BES_Expired_Messages_Theshold = dr.Item("ExpiredThreshold")
						End If
					Catch ex As Exception
						.BES_Expired_Messages_Theshold = 10000
					End Try
					WriteAuditEntry(Now.ToString & "  BlackBerry Server " & .Name & " expired threshold is " & .BES_Expired_Messages_Theshold, LogLevel.Normal)



					Try
						If dr.Item("PendingThreshold") Is Nothing Then
							.BES_Pending_Messages_Threshold = 10000
						Else
							.BES_Pending_Messages_Threshold = dr.Item("PendingThreshold")
						End If
					Catch ex As Exception
						.BES_Pending_Messages_Threshold = 10000
					End Try
					WriteAuditEntry(Now.ToString & "  BlackBerry Server " & .Name & " pending threshold is " & .BES_Pending_Messages_Threshold, LogLevel.Normal)


					Try
						If dr.Item("IPAddress") Is Nothing Then
							.IPAddress = ""
						Else
							.IPAddress = dr.Item("IPAddress")
						End If
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " Invalid IP Address", LogLevel.Normal)
						.IPAddress = ""
					End Try



					'BES 4.1.x services
					Try
						If dr.Item("MDSServices") Is Nothing Then
							.MDS_Services_BES41 = True
						Else
							.MDS_Services_BES41 = dr.Item("MDSServices")
						End If

					Catch ex As Exception
						.MDS_Services_BES41 = True
					End Try

					Try
						If dr.Item("MDSConnection") Is Nothing Then
							.MDS_Connection_Service_BES41 = True
						Else
							.MDS_Connection_Service_BES41 = dr.Item("MDSConnection")
						End If

					Catch ex As Exception
						.MDS_Connection_Service_BES41 = True
					End Try

					'End BES 4.1.x service

					Try
						If dr.Item("Enabled") Is Nothing Then
							.Enabled = True
						Else
							.Enabled = dr.Item("Enabled")
						End If

						If .Enabled = False Then
							.Status = "Disabled"
						End If

						If .Enabled = True And .Status = "Disabled" Then
							.Status = "Not Scanned"
						End If
					Catch ex As Exception
						.Enabled = True
					End Try

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
						If dr.Item("ScanInterval") Is Nothing Then
							.ScanInterval = 10
						Else
							.ScanInterval = dr.Item("ScanInterval")
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
						If dr.Item("SNMPCommunity") Is Nothing Then
							.SNMP_Community = "public"
						Else
							.SNMP_Community = dr.Item("SNMPCommunity")
						End If

					Catch ex As Exception
						.SNMP_Community = "public"
					End Try


					'Configure the services
					Try
						If dr.Item("Controller") Is Nothing Then
							.ControllerService = True
						Else
							.ControllerService = dr.Item("Controller")
						End If

					Catch ex As Exception
						.ControllerService = True
					End Try

					Try
						If dr.Item("Messaging") Is Nothing Then
							.MessagingService = True
						Else
							.MessagingService = dr.Item("Messaging")
						End If

					Catch ex As Exception
						.MessagingService = True
					End Try


					Try
						If dr.Item("Dispatcher") Is Nothing Then
							.DispatcherService = True
						Else
							.DispatcherService = dr.Item("Dispatcher")
						End If

					Catch ex As Exception
						.DispatcherService = True
					End Try

					Try
						If dr.Item("Synchronization") Is Nothing Then
							.SynchronizationService = True
						Else
							.SynchronizationService = dr.Item("Synchronization")
						End If

					Catch ex As Exception
						.SynchronizationService = True
					End Try

					Try
						If dr.Item("Policy") Is Nothing Then
							.PolicyService = True
						Else
							.PolicyService = dr.Item("Policy")
						End If

					Catch ex As Exception
						.PolicyService = True
					End Try

					Try
						If dr.Item("MDS") Is Nothing Then
							.MobileDataService_BES40 = True
						Else
							.MobileDataService_BES40 = dr.Item("MDS")
						End If

					Catch ex As Exception
						.MobileDataService_BES40 = True
					End Try

					Try
						If dr.Item("Attachment") Is Nothing Then
							.AttachmentService = True
						Else
							.AttachmentService = dr.Item("Attachment")
						End If

					Catch ex As Exception
						.AttachmentService = True
					End Try

					Try
						If dr.Item("Alert") Is Nothing Then
							.AlertService = True
						Else
							.AlertService = dr.Item("Alert")
						End If

					Catch ex As Exception
						.AlertService = True
					End Try


					Try
						If dr.Item("Router") Is Nothing Then
							.RouterService = True
						Else
							.RouterService = dr.Item("Router")
						End If

					Catch ex As Exception
						.RouterService = True
					End Try

					Try
						If dr.Item("AlertIP") Is Nothing Then
							.Alert_Service_Address = .IPAddress
						Else
							.Attachment_Service_Address = dr.Item("AlertIP")
						End If

					Catch ex As Exception
						.Alert_Service_Address = .IPAddress
					End Try

					Try
						If dr.Item("RouterIP") Is Nothing Then
							.Router_Service_Address = .IPAddress
						Else
							.Router_Service_Address = dr.Item("RouterIP")
						End If

					Catch ex As Exception
						.Router_Service_Address = .IPAddress
					End Try


					Try
						If dr.Item("Location") Is Nothing Then
							' .Location = .IPAddress
						Else
							.Location = dr.Item("Location")
						End If

					Catch ex As Exception

					End Try

					Try
						If dr.Item("AttachmentIP") Is Nothing Then
							.Attachment_Service_Address = .IPAddress
						Else
							.Attachment_Service_Address = dr.Item("AttachmentIP")
						End If

					Catch ex As Exception
						.Attachment_Service_Address = .IPAddress
					End Try

					Try
						If dr.Item("Status") Is Nothing Then
							.Status = "Not Scanned"
						Else
							.Status = dr.Item("Status")
						End If
					Catch ex As Exception
						.Status = "Not Scanned"
					End Try

					Try
						If dr.Item("LastUpdate") Is Nothing Then
							.LastScan = Now
						Else
							.LastScan = dr.Item("LastUpdate")
						End If
					Catch ex As Exception
						.LastScan = Now
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
                        WriteAuditEntry(Now.ToString & " " & .Name & " BlackBerry Servers insufficient licenses not set.")

					End Try

				End With

				myBlackBerryServer = Nothing
			Next
			dr = Nothing
     
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " BlackBerry Servers exception " & ex.ToString)
		End Try

		InsufficentLicensesTest(MyBlackBerryServers)

        dsBlackBerryServers.Dispose()

        WriteAuditEntry(Now.ToString & " Finished configuring " & MyBlackBerryServers.Count & " BES servers.")
    End Sub

    Private Sub CreateMailServicesCollection()
        WriteAuditEntry(Now.ToString & "  Reading configuration settings for Mail Services.")
        'start with fresh data

        'Connect to the data source
        Dim dsMailServices As New Data.DataSet
        Dim strSQL As String
        Try
            'Changed the query for VSPLUS-564
            'ds.Address as IPAddress was just IPAddress

			'strSQL = "select srv.ServerName, Name, Location, ScanInterval, Category, Enabled, ds.Address as IPAddress,  ServerType,  OffHoursScanInterval, RetryInterval, ResponseThreshold, FailureThreshold, Port from  "
			'strSQL += " dbo.MailServices ds, dbo.Servers srv, dbo.ServerTypes srvt,  dbo.Locations Loc "
			'strSQL += "  where(ds.ServerID = srv.ID And srv.ServerTypeID = srvt.id And srv.LocationID = Loc.ID And Enabled = 1)"

			strSQL = " select ds.Name, Loc.Location, ScanInterval, ds.Category, Enabled, ds.Address as IPAddress,  ServerType,  OffHoursScanInterval, RetryInterval, ds.ResponseThreshold, FailureThreshold, Port,  "
			strSQL += " st.LastUpdate, st.Status, st.StatusCode, di.CurrentNodeID "
			strSQL += " from dbo.MailServices ds  "
			strSQL += " inner join dbo.ServerTypes srvt on ds.ServerTypeId=srvt.ID "
			strSQL += " inner join dbo.Locations Loc on Loc.ID=ds.LocationID "

			If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then
				Dim nodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
				strSQL += " inner join DeviceInventory di on ds.[Key]=di.DeviceID and di.DeviceTypeId=(select ID from ServerTypes where ServerType='Mail') " & _
				  "inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" & nodeName & "' "

			End If

			strSQL += " left outer join Status st on st.Type='Mail' and st.Name=ds.Name "
			strSQL += " where Enabled = 1"

			'  strSQL = " select srv.ServerName, ScanInterval, Category, Enabled, IPAddress, Location, ServerType, OffHoursScanInterval, RetryInterval, ResponseThreshold, FailureThreshold, Port "
			'    strSQL += "from  dbo.MailServices ds, dbo.Servers srv, dbo.ServerTypes srvt,  dbo.Locations(Loc) "
			'    strSQL += "where ds.ServerID = srv.ID And srv.ServerTypeID = srvt.id And srv.LocationID = Loc.ID"
			WriteAuditEntry(Now.ToString & " The SQL Statement is: " & vbCrLf & strSQL)
			Dim objVSAdaptor As New VSAdaptor
			objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsMailServices, "MailServices")
		Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateMailServicesCollection processing code. Exception: " & ex.Message)
            Exit Sub
        End Try

        If dsMailServices.Tables("MailServices").Rows.Count = 0 Then
            WriteAuditEntry(Now.ToString & " There are no mail services defined. ")
            Exit Sub
        End If

		'Add the Mail Services  to the collection

        Dim dr As DataRow
        Dim i As Integer = 0
        For Each dr In dsMailServices.Tables("MailServices").Rows
            MyMailService = Nothing
            i += 1
            Dim MyName As String
            If dr.Item("Name") Is Nothing Then
                MyName = "Mail Service" & i.ToString
            Else
                MyName = dr.Item("Name")
            End If
            MyMailService = MyMailServices.Search(MyName)
            If MyMailService Is Nothing Then
                MyMailService = New MonitoredItems.MailService
                MyMailService.Name = MyName
                MyMailService.Description = MyName
                'Default Values
                MyMailService.IncrementUpCount()
                MyMailService.OffHours = False
                MyMailService.AlertCondition = False  'calling this sets status to OK
                MyMailService.Status = "Not Scanned"
                MyMailService.LastScan = Now
				MyMailService.NextScan = Now
				MyMailService.StatusCode = "Maintenance"
                MyMailServices.Add(MyMailService)
                If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Adding new Mail Service--" & MyMailService.Name & "-- to the collection.")
            Else
                If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Updating existing Mail Service--" & MyMailService.Name & ".")
            End If

            With MyMailService
                Try
                    If dr.Item("IPAddress") Is Nothing Then
                        .IPAddress = ""
                        '.Location = "Unknown"
                    Else
                        .IPAddress = dr.Item("IPAddress")
                        '.Location = dr.Item("IPAddress")
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Mail Service specified invalid IP Address")
                    .IPAddress = "Unknown"
                End Try

                Try
                    .OffHours = BoolOffHours
                Catch ex As Exception
                    .OffHours = False
                End Try

                Try
                    If dr.Item("Location") Is Nothing Then
                        .Location = dr.Item("ServerName")
                    Else
                        .Location = dr.Item("Location")
                    End If
                Catch ex As Exception

                End Try

                Try
                    If Not (dr.Item("ServerName")) Is Nothing Then
                        .ServerName = dr.Item("ServerName")
                    End If
                Catch ex As Exception
                    .ServerName = "None"
                End Try

                Try
                    If dr.Item("Enabled") Is Nothing Then
                        .Enabled = False
                    Else
                        .Enabled = dr.Item("Enabled")
                    End If
                Catch ex As Exception
                    .Enabled = False
                End Try

                If .Enabled = False Then
                    .Status = "Disabled"
                End If

                If .Enabled = True And .Status = "Disabled" Then
                    .Status = "Not Scanned"
                End If

				Try
					If dr.Item("Description") Is Nothing Then
						.Description = " "
					Else
						.Description = dr.Item("Description")
					End If
				Catch ex As Exception
					.Description = " "
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
                    If dr.Item("FailureThreshold") Is Nothing Then
                        .ResponseThreshold = 100
                    Else
                        .FailureThreshold = dr.Item("FailureThreshold")
                    End If
                Catch ex As Exception
                    .FailureThreshold = 2
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
                    If dr.Item("Category") Is Nothing Then
                        .Category = "Not Categorized"
                    Else
                        .Category = dr.Item("Category")
                    End If
                Catch ex As Exception
                    .Category = "Not Categorized"
                End Try

                Try
                    If dr.Item("RetryInterval") Is Nothing Then
                        .RetryInterval = 2
                        WriteAuditEntry(Now.ToString & " " & .Name & " Mail Service retry scan interval not set, using default of 10 minutes.")
                    Else
                        .RetryInterval = dr.Item("RetryInterval")
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " " & .Name & " Mail Service retry scan interval not set, using default of 10 minutes.")
                    .RetryInterval = 2
				End Try

				Try
					If dr.Item("Status") Is Nothing Then
						.Status = "Not Scanned"
					Else
						.Status = dr.Item("Status")
					End If
				Catch ex As Exception
					.Status = " "
				End Try

				Try
					If dr.Item("StatusCode") Is Nothing Then
						.StatusCode = "Maintenance"
					Else
						.StatusCode = dr.Item("StatusCode")
					End If
				Catch ex As Exception
					.StatusCode = " "
				End Try

				Try
					If dr.Item("LastUpdate") Is Nothing Then
						.LastScan = Now
					Else
						.LastScan = dr.Item("LastUpdate")
					End If
				Catch ex As Exception
					.LastScan = Now
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
                    WriteAuditEntry(Now.ToString & " " & .Name & " Mail Server insufficient licenses not set.")

				End Try

            End With
        Next
        dr = Nothing

		InsufficentLicensesTest(MyMailServices)

        'Free memory
        dsMailServices.Dispose()
        MyMailService = Nothing

    End Sub

    Private Sub CreateSametimeServersCollection()
        'start with fresh data
        'Connect to the data source
        Dim dsSametime As New Data.DataSet
		Dim mySecrets As New VSFramework.TripleDES
        Try


			Dim strSQL As String ' = "SELECT Name, Description, Category, UserThreshold, ChatThreshold, NChatThreshold, PlacesThreshold, Enabled, ScanInterval, OffHoursScanInterval, Location, ID, RetryInterval, ResponseThreshold, IPAddress, nserver, stcommlaunch, stcommunity, stconfigurationapp, stplaces, stmux, stusers, stonlinedir, stdirectory, stlogger, stlinks, stprivacy, stsecurity, stpresencemgr, stservicemanager, stpresencesubmgr, steventserver, stpolicy, stconfigurationbridge, stadminsrv, stuserstorage, stchatlogging, stpolling, stresolve, SSL, stpresencecompatmgr FROM SametimeServers"

			strSQL = " select srv.ServerName as Name, ScanInterval, ds.Category, Enabled, IPAddress, Loc.Location, ServerType, OffHoursScanInterval, RetryInterval, UserThreshold, ChatThreshold,"
			strSQL += " NChatThreshold, PlacesThreshold, ds.ResponseThreshold, nserver, stcommlaunch, stcommunity, stconfigurationapp, stplaces, stmux, stusers, stonlinedir, stdirectory, "
			strSQL += " stlogger stlinks, stprivacy, stsecurity, stpresencemgr, stpresencesubmgr, steventserver, stpolicy, stconfigurationbridge, stadminsrv, stuserstorage, stchatlogging,"
            strSQL += " stpolling, stpresencecompatmgr, SSL, stservicemanager, stresolve, stconference, ST.LastUpdate, ST.Status, ST.StatusCode, di.CurrentNodeID,Platform,WsScanMeetingServer,"
            strSQL += " WsMeetingHost,WsMeetingRequireSSL,WsMeetingPort,WsScanMediaServer,WsMediaHost,WsMediaRequireSSL,WsMediaPort,ChatUser1CredentialsId,ChatUser2CredentialsId,"
            strSQL += " STExtendedStatsPort,STScanExtendedStats,Cred1.UserId UserId1 ,Cred1.Password Password1 ,Cred2.UserId UserId2 ,Cred2.Password Password2, TestChatSimulation,"
            strSQL += " db2hostname, db2port, Cred3.UserId db2username, Cred3.Password db2password"


			strSQL += "  from dbo.SametimeServers ds"
			strSQL += " inner join dbo.Servers srv on ds.ServerID=srv.ID "
			strSQL += " inner join dbo.ServerTypes srvt on srvt.ID=srv.ServerTypeID"
			strSQL += " inner join dbo.Locations Loc on Loc.ID=srv.LocationID"
			strSQL += " left outer join dbo.Credentials Cred1 on Cred1.ID=ds.ChatUser1CredentialsId"
			strSQL += " left outer join dbo.Credentials Cred2 on Cred2.ID=ds.ChatUser2CredentialsId"
            strSQL += " left outer join dbo.Credentials Cred3 on Cred3.ID=ds.CredentialID"

			If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then

				Dim NodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
				strSQL += "  inner join DeviceInventory di on srv.ID=di.DeviceID and di.DeviceTypeId=srv.ServerTypeId  "
				strSQL += " inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" & NodeName & "'  "
			End If
			strSQL += " left outer join Status st on st.Type=srvt.ServerType and st.Name=srv.ServerName  "
			strSQL += " where(ds.ServerID = srv.ID And srv.ServerTypeID = srvt.id And srv.LocationID = Loc.ID And srv.ServerTypeID = 3 And ds.Enabled = 1)"


			Dim objVSAdaptor As New VSAdaptor
			objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsSametime, "SametimeServers")
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateSametimeServersCollection processing code. Exception: " & ex.Message)
			Exit Sub
		End Try



        '***
        'but first delete any that are in the collection but not in the database anymore
        'Delete propogation
        Dim myDataRow As System.Data.DataRow
        Dim MyServerNames As String

        If mySametimeServers.Count > 0 Then
            WriteAuditEntry(Now.ToString & " Checking to see if any ST databases should be deleted. ")
            'Get all the names of all the servers in the data table
            For Each myDataRow In dsSametime.Tables("SametimeServers").Rows()
                MyServerNames += myDataRow.Item("Name") & "  "
            Next
        End If

        Dim ST As MonitoredItems.SametimeServer
        Dim myIndex As Integer

        If mySametimeServers.Count > 0 Then
            For myIndex = mySametimeServers.Count - 1 To 0 Step -1
                ST = mySametimeServers.Item(myIndex)
                Try
                    WriteAuditEntry(Now.ToString & " Checking to see if ST Server " & MySametimeServer.Name & " should be deleted...")
                    If InStr(MyServerNames, ST.Name) > 0 Then
                        'the server has not been deleted
                        WriteAuditEntry(Now.ToString & " " & ST.Name & " is not marked for deletion. ")
                    Else
                        'the server has been deleted, so delete from the collection
                        Try
							mySametimeServers.Delete(ST.Name)
                            WriteAuditEntry(Now.ToString & " " & ST.Name & " has been deleted by the service.")
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " " & ST.Name & " was not deleted by the service because " & ex.Message)
                        End Try
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " Exception ST servers Deletion Loop on " & ST.Name & ".  The error was " & ex.Message)
                End Try
            Next
        End If

        MyServerNames = Nothing
        ST = Nothing
        myIndex = Nothing

        '*** End delete propogation

        Dim i As Integer = 0
		'Add the Sametime servers to the collection
        Try
            If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & "  Reading configuration settings for " & dsSametime.Tables("SametimeServers").Rows.Count & " Sametime Servers.")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error with sametime table " & ex.Message, LogLevel.Normal)
        End Try
        Try
            Dim myString As String = ""
            Dim dr As DataRow
            For Each dr In dsSametime.Tables("SametimeServers").Rows
                i += 1
                Dim MyName As String
                If dr.Item("Name") Is Nothing Then
                    MyName = "Sametime Server #" & i.ToString
                Else
                    MyName = dr.Item("Name")
                End If
                'See if this server is already in the collection; if so, update its settings otherwise create a new one
                MySametimeServer = mySametimeServers.SearchByName(MyName)
                If MySametimeServer Is Nothing Then
                    Try
                        MySametimeServer = New MonitoredItems.SametimeServer
                        MySametimeServer.Name = MyName
                        MySametimeServer.LastScan = Now
                        MySametimeServer.NextScan = Now
                        MySametimeServer.AlertCondition = False
                        MySametimeServer.Status = "Not Scanned"
                        MySametimeServer.IncrementUpCount()
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error adding new empty stats collection to " & MySametimeServer.Name)
                    End Try
                    mySametimeServers.Add(MySametimeServer)
                    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Adding new Sametime server -- " & MySametimeServer.Name & " -- to the collection.")
                Else
                    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Updating settings for existing Sametime server-- " & MySametimeServer.Name & ".")
                End If

                With MySametimeServer

                    'Sametime Specific
                    Try
                        If dr.Item("ChatThreshold") Is Nothing Then
                            .Chat_Sessions_Threshold = 999
                            WriteAuditEntry(Now.ToString & " Error: No chat threshold specified for ST server " & .Name)
                        Else
                            .Chat_Sessions_Threshold = dr.Item("ChatThreshold")
                        End If
                    Catch ex As Exception
                        .Chat_Sessions_Threshold = 999
                        WriteAuditEntry(Now.ToString & " Error:  No chat threshold specified  for ST server " & .Name)
                    End Try

                    Try
                        If dr.Item("NChatThreshold") Is Nothing Then
                            .nWay_Chat_Sessions_Threshold = 999
                            WriteAuditEntry(Now.ToString & " Error: No n-way chat threshold specified for ST server " & .Name)
                        Else
                            .nWay_Chat_Sessions_Threshold = dr.Item("NChatThreshold")
                        End If
                    Catch ex As Exception
                        .nWay_Chat_Sessions_Threshold = 999
                        WriteAuditEntry(Now.ToString & " Error:  No n-way chat threshold specified  for ST server " & .Name)
                    End Try

                    Try
                        If dr.Item("PlacesThreshold") Is Nothing Then
                            .Places_Threshold = 999
                            WriteAuditEntry(Now.ToString & " Error: No Places threshold specified for ST server " & .Name)
                        Else
                            .Places_Threshold = dr.Item("PlacesThreshold")
                        End If
                    Catch ex As Exception
                        .Places_Threshold = 999
                        WriteAuditEntry(Now.ToString & " Error:  No Places threshold specified  for ST server " & .Name)
                    End Try

                    Try
                        If dr.Item("UserThreshold") Is Nothing Then
                            .Users_Threshold = 999
                            WriteAuditEntry(Now.ToString & " Error: No Users threshold specified for ST server " & .Name)
                        Else
                            .Users_Threshold = dr.Item("UserThreshold")
                        End If
                    Catch ex As Exception
                        .Users_Threshold = 999
                        WriteAuditEntry(Now.ToString & " Error:  No Users threshold specified  for ST server " & .Name)
                    End Try

                    Try
                        If dr.Item("SSL") = 1 Then
                            .SSL = True
                        Else
                            .SSL = False
                        End If
                    Catch ex As Exception
                        .SSL = False
                    End Try
                   
                    'Try
                    '    If dr.Item("SSL") = True Then
                    '        .SSL = True
                    '    Else
                    '        .SSL = False
                    '    End If
                    'Catch ex As Exception
                    '    .SSL = False
                    'End Try

                   

                    'BEGIN Running Processes to monitor *****************
                    Dim myMonitoredServices As New MonitoredItems.SametimeMonitoredProcessCollection

                    Try
                        If dr.Item("nserver") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "nserver"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stcommlaunch") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stcommlaunch"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stcommunity") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stcommunity"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try


                    Try
                        If dr.Item("stconfigurationapp") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stconfigurationapp"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stplaces") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stplaces"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stmux") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stmux"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stusers") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stusers"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stonlinedir") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stonlinedir"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stdirectory") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stdirectory"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stlogger") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stlogger"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try


                    Try
                        If dr.Item("stlinks") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stlinks"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try


                    Try
                        If dr.Item("stprivacy") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stprivacy"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stsecurity") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stsecurity"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stpresencemgr") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stpresencemgr"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stservicemanager") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stservicemanager"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stpresencesubmgr") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stpresencesubmgr"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try


                    Try
                        If dr.Item("steventserver") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "steventserver"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stpolicy") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stpolicy"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stconfigurationbridge") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stconfigurationbridge"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try


                    Try
                        If dr.Item("stadminsrv") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stadminsrv"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stuserstorage") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stuserstorage"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try
                    Try
                        If dr.Item("stchatlogging") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stchatlogging"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stpolling") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stpolling"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If dr.Item("stresolve") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stresolve"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try


                    Try
                        If dr.Item("stpresencecompatmgr") = True Then
                            Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                            myMonitoredService.Name = "stpresencecompatmgr"
                            myMonitoredServices.Add(myMonitoredService)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If myMonitoredServices.Count > 0 Then
                            MySametimeServer.MonitoredProcesses = myMonitoredServices
                        End If

                    Catch ex As Exception

                    End Try

                    'END Running Processes to monitor *****************

                    Try
                        WriteAuditEntry(Now.ToString & " The following processes will be monitored for this Sametime server: ")
                        For Each Process As MonitoredItems.SametimeMonitoredProcess In MySametimeServer.MonitoredProcesses
                            WriteAuditEntry(Now.ToString & " " & Process.Name)
                        Next
                    Catch ex As Exception

                    End Try


                    'Standard attributes

                    Try
                        If dr.Item("Location") Is Nothing Then
                            .Location = "Not Set"
                        Else
                            .Location = dr.Item("Location")
                        End If
                    Catch ex As Exception
                        .Location = "Not Set"
                    End Try

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
                        .OffHours = BoolOffHours
                    Catch ex As Exception
                        .OffHours = False
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
                        If dr.Item("IPAddress") Is Nothing Then
                            .IPAddress = ""
                        Else
                            .IPAddress = dr.Item("IPAddress")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Invalid IP Address")
                        .IPAddress = ""
                        .Status = "Invalid IP Address"
                        .Enabled = True
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
						If dr.Item("ScanInterval") Is Nothing Then
							.ScanInterval = 10
						Else
							.ScanInterval = dr.Item("ScanInterval")
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
                        If dr.Item("RetryInterval") Is Nothing Then
                            .RetryInterval = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " Notes Database retry scan interval not set, using default of 10 minutes.")
                        Else
                            .RetryInterval = dr.Item("RetryInterval")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Notes Database retry scan interval not set, using default of 10 minutes.")
                        .RetryInterval = 2
					End Try

					Try
						If dr.Item("Status") Is Nothing Then
							.Status = "Not Scanned"
						Else
							.Status = dr.Item("Status")
						End If
					Catch ex As Exception
						.Status = "Not Scanned"
					End Try

					Try
						If dr.Item("LastUpdate") Is Nothing Then
							.LastScan = Now
						Else
							.LastScan = dr.Item("LastUpdate")
						End If
					Catch ex As Exception
						.LastScan = Now
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
                        WriteAuditEntry(Now.ToString & " " & .Name & " Sametime insufficient licenses not set.")

                    End Try

                    Try
                        If dr.Item("WsMeetingHost") Is Nothing Then
                            .WsMeetingHost = ""
                        Else
                            .WsMeetingHost = dr.Item("WsMeetingHost")
                        End If
                    Catch ex As Exception
                        .WsMeetingHost = ""
                    End Try

					Try
						If dr.Item("WsScanMeetingServer") Is Nothing Then
							.WsScanMeetingServer = False
						Else
							.WsScanMeetingServer = dr.Item("WsScanMeetingServer")
						End If
					Catch ex As Exception
						.WsScanMeetingServer = False
					End Try

					Try
						If dr.Item("WsMeetingRequireSSL") Is Nothing Then
							.WsMeetingRequireSSL = False
						Else
							.WsMeetingRequireSSL = dr.Item("WsMeetingRequireSSL")
						End If
					Catch ex As Exception
						.WsMeetingRequireSSL = False
					End Try

					Try
						.WsMeetingPort = dr.Item("WsMeetingPort").ToString()
					Catch ex As Exception
						.WsMeetingPort = "80"
					End Try


					Try
						If dr.Item("WsScanMediaServer") Is Nothing Then
							.WsScanMediaServer = False
						Else
							.WsScanMediaServer = dr.Item("WsScanMediaServer")
						End If
					Catch ex As Exception
						.WsScanMediaServer = False
					End Try

					Try
						If dr.Item("WsMediaRequireSSL") Is Nothing Then
							.WsMediaRequireSSL = False
						Else
							.WsMediaRequireSSL = dr.Item("WsMediaRequireSSL")
						End If
					Catch ex As Exception
						.WsMediaRequireSSL = False
					End Try

					Try
						.WsMediaPort = dr.Item("WsMediaPort").ToString()
					Catch ex As Exception
						.WsMediaPort = "80"
					End Try

					.WsMediaHost = dr.Item("WsMediaHost").ToString()

					.Platform = dr.Item("Platform").ToString()
					Dim myRegistry As New VSFramework.RegistryHandler()
					Dim userOne As String = "", userTwo As String = "", pass1 As Byte(), pass2 As Byte()
					Dim strEncryptedPassword As String = ""

					Try
						userOne = dr.Item("UserId1").ToString()
						WriteAuditEntry(Now.ToString & " Sametime User One is " & userOne)
						WriteAuditEntry(Now.ToString & " Sametime User One pwd is " & dr.Item("Password1").ToString())
						strEncryptedPassword = dr.Item("Password1").ToString()	 'sametime password as encrypted byte stream
						Try
							Dim str1() As String
							str1 = strEncryptedPassword.Split(",")
							Dim bstr1(str1.Length - 1) As Byte
							For j As Integer = 0 To str1.Length - 1
								bstr1(j) = str1(j).ToString()
							Next
							pass1 = bstr1
						Catch ex As Exception

						End Try

						If Not pass1 Is Nothing Then
							.Password1 = mySecrets.Decrypt(pass1)  'password in clear text, stored in memory now
						Else
							.Password1 = ""
						End If
						.UserId1 = userOne
						WriteAuditEntry(Now.ToString & " Sametime User One decrypt pwd is " & dr.Item("Password1").ToString())
					Catch ex As Exception
						.UserId1 = ""
						WriteAuditEntry(Now.ToString & " Cannot Set user id1 " + ex.ToString(), LogLevel.Normal)
					End Try

					Try
						userTwo = dr.Item("UserId2").ToString()
						WriteAuditEntry(Now.ToString & " Sametime User two is " & userTwo)
						WriteAuditEntry(Now.ToString & " Sametime User One is " & dr.Item("Password2").ToString())
						strEncryptedPassword = dr.Item("Password2")	 'sametime password as encrypted byte stream
						Try
							Dim str1() As String
							str1 = strEncryptedPassword.Split(",")
							Dim bstr1(str1.Length - 1) As Byte
							For j As Integer = 0 To str1.Length - 1
								bstr1(j) = str1(j).ToString()
							Next
							pass2 = bstr1
						Catch ex As Exception

						End Try

						If Not pass2 Is Nothing Then
							.Password2 = mySecrets.Decrypt(pass2)  'password in clear text, stored in memory now
						Else
							.Password2 = ""
						End If
						.UserId2 = userTwo
					Catch ex As Exception
						.UserId2 = ""
						WriteAuditEntry(Now.ToString & " Cannot Set user id2 ", LogLevel.Normal)
					End Try

					Try
						.ExtendedChatPort = dr.Item("STExtendedStatsPort").ToString()
					Catch ex As Exception
						.ExtendedChatPort = "80"
					End Try



					Try
						If dr.Item("STScanExtendedStats") Is Nothing Then
							.CollectExtendedChat = False
						Else
							.CollectExtendedChat = dr.Item("STScanExtendedStats")
						End If
					Catch ex As Exception
						.CollectExtendedChat = False
					End Try

					Try
						If dr.Item("TestChatSimulation") Is Nothing Then
							.TestChatSimulation = False
						Else
							.TestChatSimulation = dr.Item("TestChatSimulation")
						End If
					Catch ex As Exception
						.TestChatSimulation = False
                    End Try

                    Try
                        If dr.Item("db2hostname") Is Nothing Then
                            .DBHostName = ""
                        Else
                            .DBHostName = dr.Item("db2hostname")
                        End If
                    Catch ex As Exception
                        .DBHostName = ""
                    End Try

                    Try
                        If dr.Item("db2port") Is Nothing Then
                            .DBPort = 0
                        Else
                            .DBPort = dr.Item("db2port")
                        End If
                    Catch ex As Exception
                        .DBPort = 0
                    End Try

                    Try
                        If dr.Item("db2username") Is Nothing Then
                            .DBUserName = ""
                        Else
                            .DBUserName = dr.Item("db2username")
                        End If
                    Catch ex As Exception
                        .DBUserName = ""
                    End Try

                    Try
                        strEncryptedPassword = dr.Item("db2password")  'sametime password as encrypted byte stream
                        Try
                            Dim str1() As String
                            str1 = strEncryptedPassword.Split(",")
                            Dim bstr1(str1.Length - 1) As Byte
                            For j As Integer = 0 To str1.Length - 1
                                bstr1(j) = str1(j).ToString()
                            Next
                            pass2 = bstr1
                        Catch ex As Exception

                        End Try

                        If Not pass2 Is Nothing Then
                            .DBPassword = mySecrets.Decrypt(pass2)  'password in clear text, stored in memory now
                        Else
                            .DBPassword = ""
                        End If
                        .UserId2 = userTwo
                    Catch ex As Exception

                    End Try

                  

                End With
                MySametimeServer = Nothing
            Next
            dr = Nothing

        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Sametime Servers data exception " & exception.Message)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Sametime Servers general exception " & ex.ToString)
        End Try

		InsufficentLicensesTest(mySametimeServers)

        dsSametime.Dispose()
    End Sub

    Private Sub CreateURLCollection()
        'start with fresh data

        'Connect to the data source
        Dim dsURLs As New Data.DataSet
        '5/2/2016 NS modified
        Dim strQuery As String = ""
        Dim strJoins As String = ""
        Dim strSQL As String = ""
        Try
			dsURLs.Tables.Add("URLs")
            '5/2/2016 NS modified for VSPLUS-2887
            strQuery = "SELECT URLs.Category, FailureThreshold, UserName, PW, Enabled, LastChecked, LastStatus, URLs.Name, URLs.NextScan, OffHoursScanInterval, URLs.ResponseThreshold, RetryInterval,  SearchString,AlertStringFound, "
            strQuery += " ScanInterval, TheURL, st.LastUpdate, st.Status, st.StatusCode, l.Location Location "
            strJoins = " FROM URLs "
            strJoins += " INNER JOIN Locations l ON l.ID=LocationID "

			If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then
                Dim NodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
                strQuery += ", di.CurrentNodeID "
                strJoins += " inner join DeviceInventory di on URLs.ID=di.DeviceID and di.DeviceTypeId=(select ID from ServerTypes where ServerType='URL')  "
                strJoins += " inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" & NodeName & "' "
			End If

            strJoins += " left outer join Status st on st.Type='URL' and st.Name=URLs.Name"
            strJoins += " WHERE(Enabled = 1)"
            strSQL = strQuery + strJoins

			WriteAuditEntry(vbCrLf & Now.ToString & " My URL collection SQL statement is " & strSQL)
			Dim objVSAdaptor As New VSAdaptor
			objVSAdaptor.FillDatasetAny("VitalSigns", "Servers", strSQL, dsURLs, "URLs")

			WriteAuditEntry(Now.ToString & " Created a URL dataset with " & dsURLs.Tables(0).Rows.Count & " records found.")
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Failed to create dataset in URL processing code. Exception: " & ex.Message)
			'  Exit Sub
		End Try

        '***


        'Add the URLs  to the collection
        WriteAuditEntry(Now.ToString & "  Reading configuration settings for URLs.")
        Try
            Dim dr As DataRow
            Dim i As Integer = 0
            For Each dr In dsURLs.Tables("URLs").Rows
                i += 1
                Dim MyName As String
                If dr.Item("Name") Is Nothing Then
                    MyName = "URL" & i.ToString
                Else
                    MyName = dr.Item("Name")
                End If
                MyURL = MyURLs.Search(MyName)
                If MyURL Is Nothing Then
                    MyURL = New MonitoredItems.URL
                    MyURL.Name = MyName
                    MyURL.Description = MyName
                    'Default Values
                    MyURL.OffHours = False
                    MyURL.AlertCondition = False  'calling this sets status to OK
                    MyURL.Status = "Not Scanned"
                    MyURL.IncrementUpCount()
                    MyURL.LastScan = Now
                    MyURL.NextScan = Now
                    MyURL.SearchString = ""
                    '5/2/2016 NS modified for VSPLUS-2887
                    MyURL.Location = dr.Item("Location")
                    MyURLs.Add(MyURL)

                    WriteAuditEntry(Now.ToString & " Adding new URL--" & MyURL.Name & "-- to the collection.", LogLevel.Verbose)
                Else
                    WriteAuditEntry(Now.ToString & " Updating existing URL--" & MyURL.Name & ".", LogLevel.Verbose)
                End If

                With MyURL

                    WriteAuditEntry(Now.ToString & " Configuring URL: " & dr.Item("Name"), LogLevel.Verbose)
                    Try
                        If dr.Item("Enabled") Is Nothing Then
                            .Enabled = True
                        Else
                            .Enabled = dr.Item("Enabled")
                        End If

                    Catch ex As Exception
                        .Enabled = True
                    End Try

                    Try
                        .OffHours = BoolOffHours
                    Catch ex As Exception
                        .OffHours = False
                    End Try

                    If .Enabled = False Then
                        .Status = "Disabled"
                    End If

                    If .Enabled = True And .Status = "Disabled" Then
                        .Status = "Not Scanned"
                    End If

					Try
						If dr.Item("Category") Is Nothing Then
							.Category = "Internet"
							WriteAuditEntry(Now.ToString & " " & .Name & " URL Category not set, using default of 'Domino'.")
						Else
							.Category = dr.Item("Category")
						End If
					Catch ex As Exception
						.Category = "Internet"
						WriteAuditEntry(Now.ToString & " " & .Name & " URL Category not set, using default of 'NotesMail Probe'.")
					End Try

                    Try
                        If dr.Item("FailureThreshold") Is Nothing Then
                            .FailureThreshold = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " URL failure threshold not set, using default of '2'.")
                        Else
                            .FailureThreshold = dr.Item("FailureThreshold")
                        End If
                    Catch ex As Exception
                        .FailureThreshold = 2
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL failure threshold not set, using default of '2'.")
                    End Try


                    Try
                        If dr.Item("ScanInterval") Is Nothing Then
                            .ScanInterval = 10
                            WriteAuditEntry(Now.ToString & " " & .Name & " URL  scan interval not set, using default of 10 minutes.")
                        Else
                            .ScanInterval = dr.Item("ScanInterval")
                        End If
                    Catch ex As Exception
                        .ScanInterval = 10
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL  scan interval not set, using default of 10 minutes.")
                    End Try


                    Try
                        If dr.Item("ResponseThreshold") Is Nothing Then
                            .ResponseThreshold = 5
                            WriteAuditEntry(Now.ToString & " " & .Name & " URL threshold not set, using default of 5 minutes.")
                        Else
                            .ResponseThreshold = dr.Item("ResponseThreshold")
                        End If
                    Catch ex As Exception
                        .ResponseThreshold = 5
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL threshold not set, using default of 5 minutes.")

                    End Try

                    Try
                        If dr.Item("SearchString") Is Nothing Then
                            .SearchString = ""
                        Else
                            .SearchString = dr.Item("SearchString")
                        End If
                    Catch ex As Exception
                        .SearchString = ""
                    End Try
                    WriteAuditEntry(Now.ToString & " " & .Name & " URL search string is " & .SearchString)

                    Try
                        If dr.Item("RetryInterval") Is Nothing Then
                            .RetryInterval = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " URL retry scan interval not set, using default of 2 minutes.")
                        Else
                            .RetryInterval = dr.Item("RetryInterval")
                        End If
                    Catch ex As Exception
                        .RetryInterval = 2
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL retry scan interval not set, using default of 2 minutes.")
                    End Try

                    Try
                        If dr.Item("OffHoursScanInterval") Is Nothing Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " URL off hours scan interval not set, using default of 20 minutes.")
                            .OffHoursScanInterval = 20
                        Else
                            .OffHoursScanInterval = dr.Item("OffHoursScanInterval")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL off hours scan interval not set, using default of 20 minutes.")
                        .OffHoursScanInterval = 20
                    End Try

                    Try
                        If dr.Item("TheURL") Is Nothing Then
                            .URL = "Not Set"
                        Else
                            .URL = dr.Item("TheURL")
                        End If
                    Catch ex As Exception
                        .IPAddress = "Not Set"
                        .Enabled = False
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL has no URL. ")

                    End Try

                    Try
                        If dr.Item("UserName") Is Nothing Or dr.Item("UserName").trim = "" Then
                            .UserName = ""
                            WriteAuditEntry(Now.ToString & " " & .Name & " URL does not require authentication. ")
                        Else
                            .UserName = dr.Item("UserName")
                            WriteAuditEntry(Now.ToString & " " & .Name & " This URL does require authentication, and will be logged in as " & .UserName)
                        End If
                    Catch ex As Exception
                        .UserName = ""
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL does not require authentication. ")
                        '  WriteAuditEntry(Now.ToString & " " & .Name & " URL authentication exception " & ex.ToString)
                    End Try

                    Try
                        If dr.Item("PW") Is Nothing Then
                            .Password = ""
                        Else
                            .Password = dr.Item("PW")
                        End If
                    Catch ex As Exception
                        .Password = ""
					End Try

					Try
						If dr.Item("Status") Is Nothing Then
							.Status = "Not Scanned"
						Else
							.Status = dr.Item("Status")
						End If
					Catch ex As Exception
						.Status = "Not Scanned"
					End Try

					Try
						If dr.Item("LastUpdate") Is Nothing Then
							.LastScan = Now
						Else
							.LastScan = dr.Item("LastUpdate")
						End If
					Catch ex As Exception
						.LastScan = Now
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
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL insufficient licenses not set.")

					End Try

					Try
						If dr.Item("AlertStringFound") Is Nothing Then
							.AlertStringFound = "0"
						Else
							.AlertStringFound = dr.Item("AlertStringFound").ToString()
						End If
					Catch ex As Exception
						.AlertStringFound = "0"
					End Try

                    '5/2/2016 NS modified for VSPLUS-2887
                    Try
                        If dr.Item("Location") Is Nothing Then
                            .Location = "URL"
                        Else
                            .Location = dr.Item("Location")
                        End If
                    Catch ex As Exception
                        .Location = "URL"
                    End Try
                End With

                MyURL = Nothing

            Next
            dr = Nothing
        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Data Exception creating URL collection: " & exception.Message)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Random Exception creating URL collection:  " & ex.Message)
        End Try

		InsufficentLicensesTest(MyURLs)

        'Free the memory
        dsURLs.Dispose()

    End Sub

    '10/6/2014 NS added for VSPLUS-1002
    Private Sub CreateCloudCollection()
        Dim dsCloudURLs As New Data.DataSet


        Try
            dsCloudURLs.Tables.Add("CloudURLs")
			Dim strSQL As String = "SELECT cd.Category, FailureThreshold, UserName, PW, Enabled, LastChecked, LastStatus, cd.Name, cd.NextScan, OffHoursScanInterval, cd.ResponseThreshold, RetryInterval,  "
			strSQL += " SearchString, ScanInterval, Url, st.LastUpdate, st.Status, st.StatusCode, di.CurrentNodeID"
			strSQL += " FROM CloudDetails cd  "

			If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then
				Dim NodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
				strSQL += " inner join DeviceInventory di on cd.ID=di.DeviceID and di.DeviceTypeId=(select ID from ServerTypes where ServerType='Cloud')  "
				strSQL += " inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" & NodeName & "'  "
			End If

			strSQL += " left outer join Status st on st.Type='Cloud' and st.Name=cd.Name  "
			strSQL += " WHERE Enabled=1"

            WriteAuditEntry(vbCrLf & Now.ToString & " My Cloud URL collection SQL statement is " & strSQL)
            Dim objVSAdaptor As New VSAdaptor
            objVSAdaptor.FillDatasetAny("VitalSigns", "Servers", strSQL, dsCloudURLs, "CloudURLs")

            WriteAuditEntry(Now.ToString & " Created a Cloud URL dataset with " & dsCloudURLs.Tables(0).Rows.Count & " records found.")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in Cloud URL processing code. Exception: " & ex.Message)
            '  Exit Sub
        End Try

        '***

        'Add the URLs  to the collection
        WriteAuditEntry(Now.ToString & "  Reading configuration settings for Cloud URLs.")
        Try
            Dim dr As DataRow
            Dim i As Integer = 0
            For Each dr In dsCloudURLs.Tables("CloudURLs").Rows
                i += 1
                Dim MyName As String
                If dr.Item("Name") Is Nothing Then
                    MyName = "CloudURL" & i.ToString
                Else
                    MyName = dr.Item("Name")
                End If
                MyCloud = MyClouds.Search(MyName)
                If MyCloud Is Nothing Then
                    MyCloud = New MonitoredItems.Cloud
                    MyCloud.Name = MyName
                    MyCloud.Description = MyName
                    'Default Values
                    MyCloud.OffHours = False
                    MyCloud.AlertCondition = False  'calling this sets status to OK
                    MyCloud.Status = "Not Scanned"
                    MyCloud.IncrementUpCount()
                    MyCloud.LastScan = Now
                    MyCloud.NextScan = Now
                    MyCloud.SearchString = ""
                    MyCloud.Location = "Cloud"
                    MyClouds.Add(MyCloud)
                    WriteAuditEntry(Now.ToString & " Adding new Cloud URL--" & MyCloud.Name & "-- to the collection.", LogLevel.Verbose)
                Else
                    WriteAuditEntry(Now.ToString & " Updating existing Cloud URL--" & MyCloud.Name & ".", LogLevel.Verbose)
                End If

                With MyCloud

                    WriteAuditEntry(Now.ToString & " Configuring Cloud URL: " & dr.Item("Name"), LogLevel.Verbose)
                    Try
                        If dr.Item("Enabled") Is Nothing Then
                            .Enabled = True
                        Else
                            .Enabled = dr.Item("Enabled")
                        End If

                    Catch ex As Exception
                        .Enabled = True
                    End Try

                    Try
                        .OffHours = BoolOffHours
                    Catch ex As Exception
                        .OffHours = False
                    End Try

                    If .Enabled = False Then
                        .Status = "Disabled"
                    End If

                    If .Enabled = True And .Status = "Disabled" Then
                        .Status = "Not Scanned"
                    End If

					Try
						If dr.Item("Category") Is Nothing Then
							.Category = "Internet"
							WriteAuditEntry(Now.ToString & " " & .Name & " Cloud Category not set, using default of 'Domino'.")
						Else
							.Category = dr.Item("Category")
						End If
					Catch ex As Exception
						.Category = "Internet"
						WriteAuditEntry(Now.ToString & " " & .Name & " Cloud Category not set, using default of 'NotesMail Probe'.")
					End Try

                    Try
                        If dr.Item("FailureThreshold") Is Nothing Then
                            .FailureThreshold = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " Cloud failure threshold not set, using default of '2'.")
                        Else
                            .FailureThreshold = dr.Item("FailureThreshold")
                        End If
                    Catch ex As Exception
                        .FailureThreshold = 2
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud failure threshold not set, using default of '2'.")
                    End Try


                    Try
                        If dr.Item("ScanInterval") Is Nothing Then
                            .ScanInterval = 10
                            WriteAuditEntry(Now.ToString & " " & .Name & " Cloud scan interval not set, using default of 10 minutes.")
                        Else
                            .ScanInterval = dr.Item("ScanInterval")
                        End If
                    Catch ex As Exception
                        .ScanInterval = 10
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud scan interval not set, using default of 10 minutes.")
                    End Try


                    Try
                        If dr.Item("ResponseThreshold") Is Nothing Then
                            .ResponseThreshold = 5
                            WriteAuditEntry(Now.ToString & " " & .Name & " Cloud threshold not set, using default of 5 minutes.")
                        Else
                            .ResponseThreshold = dr.Item("ResponseThreshold")
                        End If
                    Catch ex As Exception
                        .ResponseThreshold = 5
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud threshold not set, using default of 5 minutes.")

                    End Try

                    Try
                        If dr.Item("SearchString") Is Nothing Then
                            .SearchString = ""
                        Else
                            .SearchString = dr.Item("SearchString")
                        End If
                    Catch ex As Exception
                        .SearchString = ""
                    End Try
                    WriteAuditEntry(Now.ToString & " " & .Name & " Cloud search string is " & .SearchString)

                    Try
                        If dr.Item("RetryInterval") Is Nothing Then
                            .RetryInterval = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " Cloud retry scan interval not set, using default of 2 minutes.")
                        Else
                            .RetryInterval = dr.Item("RetryInterval")
                        End If
                    Catch ex As Exception
                        .RetryInterval = 2
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud retry scan interval not set, using default of 2 minutes.")
                    End Try

                    Try
                        If dr.Item("OffHoursScanInterval") Is Nothing Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " Cloud off hours scan interval not set, using default of 20 minutes.")
                            .OffHoursScanInterval = 20
                        Else
                            .OffHoursScanInterval = dr.Item("OffHoursScanInterval")
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud off hours scan interval not set, using default of 20 minutes.")
                        .OffHoursScanInterval = 20
                    End Try

                    Try
                        If dr.Item("Url") Is Nothing Then
                            .CloudURL = "Not Set"
                        Else
                            .CloudURL = dr.Item("Url")
                        End If
                    Catch ex As Exception
                        .IPAddress = "Not Set"
                        .Enabled = False
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud has no URL. ")

                    End Try

                    Try
                        If dr.Item("UserName") Is Nothing Or dr.Item("UserName").trim = "" Then
                            .UserName = ""
                            WriteAuditEntry(Now.ToString & " " & .Name & " Cloud does not require authentication. ")
                        Else
                            .UserName = dr.Item("UserName")
                            WriteAuditEntry(Now.ToString & " " & .Name & " This Cloud URL does require authentication, and will be logged in as " & .UserName)
                        End If
                    Catch ex As Exception
                        .UserName = ""
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud URL does not require authentication. ")
                        '  WriteAuditEntry(Now.ToString & " " & .Name & " URL authentication exception " & ex.ToString)
                    End Try

                    Try
                        If dr.Item("PW") Is Nothing Then
                            .Password = ""
                        Else
                            .Password = dr.Item("PW")
                        End If
                    Catch ex As Exception
                        .Password = ""
					End Try

					Try
						If dr.Item("Status") Is Nothing Then
							.Status = "Not Scanned"
						Else
							.Status = dr.Item("Status")
						End If
					Catch ex As Exception
						.Status = "Not Scanned"
					End Try

					Try
						If dr.Item("LastUpdate") Is Nothing Then
							.LastScan = Now
						Else
							.LastScan = dr.Item("LastUpdate")
						End If
					Catch ex As Exception
						.LastScan = Now
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
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud insufficient licenses not set.")

					End Try

				End With

				MyURL = Nothing

			Next
            dr = Nothing
        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Data Exception creating Cloud collection: " & exception.Message)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Random Exception creating Cloud collection:  " & ex.Message)
        End Try

		InsufficentLicensesTest(MyClouds)

        'Free the memory
        dsCloudURLs.Dispose()
	End Sub

	Private Sub CreateWebSphereCollection()
		'start with fresh data
		'Connect to the data source
		Dim dsWebSphere As New Data.DataSet
		Dim mySecrets As New VSFramework.TripleDES
		Try


			Dim strSQL As String ' = "SELECT Name, Description, Category, UserThreshold, ChatThreshold, NChatThreshold, PlacesThreshold, Enabled, ScanInterval, OffHoursScanInterval, Location, ID, RetryInterval, ResponseThreshold, IPAddress, nserver, stcommlaunch, stcommunity, stconfigurationapp, stplaces, stmux, stusers, stonlinedir, stdirectory, stlogger, stlinks, stprivacy, stsecurity, stpresencemgr, stservicemanager, stpresencesubmgr, steventserver, stpolicy, stconfigurationbridge, stadminsrv, stuserstorage, stchatlogging, stpolling, stresolve, SSL, stpresencecompatmgr FROM SametimeServers"

			strSQL = " select wss.ServerName, wss.ServerID, wss.NodeID, wss.CellID, wss.Enabled, wss.Hostname, " & _
			"wsn.NodeName, wsc.CellName, wsc.HostName as CellHostName, wsc.ConnectionType, wsc.PortNo, wsc.GlobalSecurity, wsc.SametimeId, " & _
			"wsc.Realm, srvt.ServerType, Loc.Location, creds.UserId, creds.Password, ST.LastUpdate, ST.Status, " & _
			"ST.StatusCode, di.CurrentNodeID, sa.ScanInterval, sa.RetryInterval, sa.OffHourInterval, sa.Category, " & _
			"sa.CPU_Threshold, sa.MemThreshold, sa.ResponseTime, sa.ConsFailuresBefAlert, sa.ConsOvrThresholdBefAlert, " & _
			"srv.Description, srvt.ID as ServerTypeID "

			'strSQL += ",wss.AvgThreadPool, wss.ActiveThreadCount, wss.CurrentHeap, wss.MaxHeap, wss.UpTime, wss.HungThreadCount, wss.DumpGenerated "
			strSQL += "from WebSphereServer wss " & _
			 "inner join WebSphereNode wsn on wss.NodeID=wsn.NodeID " & _
			 "inner join WebSphereCell wsc on wss.CellID=wsc.CellID " & _
			 "inner join Servers srv on wss.ServerID=srv.ID " & _
			 "inner join ServerTypes srvt on srvt.ID=srv.ServerTypeID " & _
			 "inner join Locations Loc on Loc.ID=srv.LocationID " & _
			 "left outer join Credentials creds on creds.ID=wsc.CredentialsID " & _
			 "inner join ServerAttributes sa on sa.ServerID=wss.ServerID "

			If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then

				Dim NodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
				strSQL += "  inner join DeviceInventory di on srv.ID=di.DeviceID and di.DeviceTypeId=srv.ServerTypeId  "
				strSQL += " inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" & NodeName & "'  "

			End If

			strSQL += "left outer join Status st on st.Type=srvt.ServerType and st.Name=srv.ServerName   "

			WriteAuditEntry(Now.ToString & " SQL Statement is " & vbCrLf & strSQL)

			Dim objVSAdaptor As New VSAdaptor
			objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsWebSphere, "WebSphere")
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateWebSphereCollection processing code. Exception: " & ex.Message)
			Exit Sub
		End Try



		'***
		'but first delete any that are in the collection but not in the database anymore
		'Delete propogation
		Dim myDataRow As System.Data.DataRow
		Dim MyServerNames As String = ""

		If MyWebSphereServers.Count > 0 Then
			WriteAuditEntry(Now.ToString & " Checking to see if any WebSphere Servers should be deleted. ")
			'Get all the names of all the servers in the data table
			For Each myDataRow In dsWebSphere.Tables("WebSphere").Rows()
				MyServerNames += myDataRow.Item("ServerName") & "  "
			Next
		End If

		Dim server As MonitoredItems.WebSphere
		Dim myIndex As Integer

		If MyWebSphereServers.Count > 0 Then
			For myIndex = MyWebSphereServers.Count - 1 To 0 Step -1
				server = MyWebSphereServers.Item(myIndex)
				Try
					WriteAuditEntry(Now.ToString & " Checking to see if WebSphere Server " & server.Name & " should be deleted...")
					If InStr(MyServerNames, server.Name) > 0 Then
						'the server has not been deleted
						WriteAuditEntry(Now.ToString & " " & server.Name & " is not marked for deletion. ")
					Else
						'the server has been deleted, so delete from the collection
						Try
							MyWebSphereServers.Delete(server.Name)
							WriteAuditEntry(Now.ToString & " " & server.Name & " has been deleted by the service.")
						Catch ex As Exception
							WriteAuditEntry(Now.ToString & " " & server.Name & " was not deleted by the service because " & ex.Message)
						End Try
					End If
				Catch ex As Exception
					WriteAuditEntry(Now.ToString & " Exception WebSphere servers Deletion Loop on " & server.Name & ".  The error was " & ex.Message)
				End Try
			Next
		End If

		MyServerNames = Nothing
		server = Nothing
		myIndex = Nothing

		'*** End delete propogation

		Dim i As Integer = 0
		'Add the Sametime servers to the collection
		Try
			If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & "  Reading configuration settings for " & dsWebSphere.Tables("WebSphere").Rows.Count & " WebSphere Servers.")
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Error with WebSphere table " & ex.Message, LogLevel.Normal)
		End Try
		Try
			Dim myString As String = ""
			Dim dr As DataRow
			For Each dr In dsWebSphere.Tables("WebSphere").Rows
				i += 1
				Dim MyName As String
				If dr.Item("ServerName") Is Nothing Then
					MyName = "WebSphere #" & i.ToString
				Else
					MyName = dr.Item("ServerName")
				End If
				'See if this server is already in the collection; if so, update its settings otherwise create a new one
				MyWebSphereServer = MyWebSphereServers.SearchByName(MyName)
				If MyWebSphereServer Is Nothing Then
					Try
						MyWebSphereServer = New MonitoredItems.WebSphere
						MyWebSphereServer.Name = MyName
						MyWebSphereServer.LastScan = Now
						MyWebSphereServer.NextScan = Now
						MyWebSphereServer.AlertCondition = False
						MyWebSphereServer.Status = "Not Scanned"
						MyWebSphereServer.IncrementUpCount()
						MyWebSphereServer.ServerType = "WebSphere"
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " Error adding new empty stats collection to " & MyWebSphereServer.Name)
					End Try
					MyWebSphereServers.Add(MyWebSphereServer)
					If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Adding new WebSphere server -- " & MyWebSphereServer.Name & " -- to the collection.")
				Else
					If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Updating settings for existing WebSphere server-- " & MyWebSphereServer.Name & ".")
				End If

				With MyWebSphereServer

					'Standard attributes

					Try
						If dr.Item("Location") Is Nothing Then
							.Location = "Not Set"
						Else
							.Location = dr.Item("Location")
						End If
					Catch ex As Exception
						.Location = "Not Set"
					End Try

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
						.OffHours = BoolOffHours
					Catch ex As Exception
						.OffHours = False
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

					'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
					'Try
					'	If dr.Item("IPAddress") Is Nothing Then
					'		.IPAddress = ""
					'	Else
					'		.IPAddress = dr.Item("IPAddress")
					'	End If
					'Catch ex As Exception
					'	WriteAuditEntry(Now.ToString & " Invalid IP Address")
					'	.IPAddress = ""
					'	.Status = "Invalid IP Address"
					'	.Enabled = True
					'End Try
					'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

					Try
						If dr.Item("ResponseTime") Is Nothing Then
							.ResponseThreshold = 100
						Else
							.ResponseThreshold = dr.Item("ResponseTime")
						End If
					Catch ex As Exception
						.ResponseThreshold = 100
					End Try


					Try
						If dr.Item("ScanInterval") Is Nothing Then
							.ScanInterval = 10
						Else
							.ScanInterval = dr.Item("ScanInterval")
						End If
					Catch ex As Exception
						.ScanInterval = 10
					End Try

					Try
						If dr.Item("OffHourInterval") Is Nothing Then
							.OffHoursScanInterval = 30
						Else
							.OffHoursScanInterval = dr.Item("OffHourInterval")
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
						If dr.Item("RetryInterval") Is Nothing Then
							.RetryInterval = 2
							WriteAuditEntry(Now.ToString & " " & .Name & " Notes Database retry scan interval not set, using default of 10 minutes.")
						Else
							.RetryInterval = dr.Item("RetryInterval")
						End If
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " " & .Name & " Notes Database retry scan interval not set, using default of 10 minutes.")
						.RetryInterval = 2
					End Try

					Try
						If dr.Item("Status") Is Nothing Then
							.Status = "Not Scanned"
						Else
							.Status = dr.Item("Status")
						End If
					Catch ex As Exception
						.Status = "Not Scanned"
					End Try

					Try
						If dr.Item("StatusCode") Is Nothing Then
							.StatusCode = "Maintenance"
						Else
							.StatusCode = dr.Item("StatusCode")
						End If
					Catch ex As Exception
						.StatusCode = "Maintenance"
					End Try

					Try
						If dr.Item("LastUpdate") Is Nothing Then
							.LastScan = Now
						Else
							.LastScan = dr.Item("LastUpdate")
						End If
					Catch ex As Exception
						.LastScan = Now
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
						WriteAuditEntry(Now.ToString & " " & .Name & " WebSphere insufficent licenses not set.")

					End Try

					Try
						If dr.Item("ServerID") Is Nothing Then
							.ID = -1
						Else
							.ID = dr.Item("ServerID")
						End If
					Catch ex As Exception
						.ID = -1
					End Try

					Try
						If dr.Item("NodeID") Is Nothing Then
							.NodeID = -1
						Else
							.NodeID = dr.Item("NodeID")
						End If
					Catch ex As Exception
						.NodeID = -1
					End Try

					Try
						If dr.Item("CellID") Is Nothing Then
							.CellID = -1
						Else
							.CellID = dr.Item("CellID")
						End If
					Catch ex As Exception
						.CellID = -1
					End Try

					Try
						If dr.Item("Hostname") Is Nothing Then
							.HostName = ""
						Else
							.HostName = dr.Item("Hostname")
						End If
					Catch ex As Exception
						.HostName = ""
					End Try

					Try
						If dr.Item("NodeName") Is Nothing Then
							.NodeName = ""
						Else
							.NodeName = dr.Item("NodeName")
						End If
					Catch ex As Exception
						.NodeName = ""
					End Try

					Try
						If dr.Item("CellName") Is Nothing Then
							.CellName = ""
						Else
							.CellName = dr.Item("CellName")
						End If
					Catch ex As Exception
						.CellName = ""
					End Try

					Try
						If dr.Item("CellHostName") Is Nothing Then
							.CellHostName = "RMI"
						Else
							.CellHostName = dr.Item("CellHostName")
						End If
					Catch ex As Exception
						.CellHostName = "RMI"
					End Try

					Try
						If dr.Item("ConnectionType") Is Nothing Then
							.ConnectionType = "RMI"
						Else
							.ConnectionType = dr.Item("ConnectionType")
						End If
					Catch ex As Exception
						.ConnectionType = "RMI"
					End Try

					Try
						If dr.Item("PortNo") Is Nothing Then
							.Port = 1099
						Else
							.Port = dr.Item("PortNo")
						End If
					Catch ex As Exception
						.Port = 1099
					End Try

					Try
						If dr.Item("GlobalSecurity") Is Nothing Then
							.GlobalSecurity = False
						Else
							.GlobalSecurity = dr.Item("GlobalSecurity")
						End If
					Catch ex As Exception
						.GlobalSecurity = False
					End Try

					Try
						If dr.Item("SametimeId") Is Nothing Then
							.SametimeID = -1
						Else
							.SametimeID = dr.Item("SametimeId")
						End If
					Catch ex As Exception
						.SametimeID = -1
					End Try

					Try
						If dr.Item("Realm") Is Nothing Then
							.Realm = ""
						Else
							.Realm = dr.Item("Realm")
						End If
					Catch ex As Exception
						.Realm = ""
					End Try

					Try
						If dr.Item("ServerType") Is Nothing Then
							.ServerType = "WebSphere"
						Else
							.ServerType = dr.Item("ServerType")
						End If
					Catch ex As Exception
						.ServerType = "WebSphere"
					End Try

					Try
						If dr.Item("UserId") Is Nothing Then
							.UserName = ""
						Else
							.UserName = dr.Item("UserId")
						End If
					Catch ex As Exception
						.UserName = ""
					End Try

					Try
						If dr.Item("Password") Is Nothing Then
							.Password = ""
						Else
							Dim myPass As Byte()
							Dim Password As String = ""
							Try
								Dim str1() As String
								str1 = dr.Item("Password").ToString().Split(",")
								Dim bstr1(str1.Length - 1) As Byte
								For j As Integer = 0 To str1.Length - 1
									bstr1(j) = str1(j).ToString()
								Next
								myPass = bstr1

								Password = mySecrets.Decrypt(myPass) 'password in clear text, stored in memory now

							Catch ex As Exception
								Password = ""
								WriteAuditEntry(Now.ToString & " Error decrypting the Notes password.  " & ex.ToString)
							End Try

							.Password = Password
						End If
					Catch ex As Exception
						.Password = ""
					End Try

					'Try
					'	If dr.Item("CPU_Threshold") Is Nothing Then
					'		.CPU_Threshold = 0
					'	Else
					'		.CPU_Threshold = dr.Item("CPU_Threshold")
					'	End If
					'Catch ex As Exception
					'	.CPU_Threshold = 0
					'End Try

					'Try
					'	If dr.Item("MemThreshold") Is Nothing Then
					'		.Memory_Threshold = 0
					'	Else
					'		.Memory_Threshold = dr.Item("MemThreshold")
					'	End If
					'Catch ex As Exception
					'	.Memory_Threshold = 0
					'End Try

					Try
						If dr.Item("ServerTypeID") Is Nothing Then
							.SametimeID = 22
						Else
							.SametimeID = dr.Item("ServerTypeID")
						End If
					Catch ex As Exception
						.SametimeID = 22
					End Try

					Try
						If dr.Item("ConsFailuresBefAlert") Is Nothing Then
							.FailureThreshold = 3
						Else
							.FailureThreshold = dr.Item("ConsFailuresBefAlert")
						End If
					Catch ex As Exception
						.FailureThreshold = 3
					End Try

					Try
						If dr.Item("ConsOvrThresholdBefAlert") Is Nothing Then
							.ServerDaysAlert = 14
						Else
							.ServerDaysAlert = dr.Item("ConsOvrThresholdBefAlert")
						End If
					Catch ex As Exception
						.ServerDaysAlert = 14
					End Try

					Try
						.ServerName = .Name.Substring(0, .Name.IndexOf("[") - 1).Trim()
					Catch ex As Exception

					End Try

					'Try
					'	If dr.Item("AvgThreadPool") Is Nothing Then
					'		.AverageThreadPoolThreshold = 0
					'	Else
					'		.AverageThreadPoolThreshold = dr.Item("AvgThreadPool")
					'	End If
					'Catch ex As Exception
					'	.AverageThreadPoolThreshold = 0
					'End Try

					'Try
					'	If dr.Item("ActiveThreadCount") Is Nothing Then
					'		.ActiveThreadCountThreshold = 0
					'	Else
					'		.ActiveThreadCountThreshold = dr.Item("ActiveThreadCount")
					'	End If
					'Catch ex As Exception
					'	.ActiveThreadCountThreshold = 0
					'End Try

					'Try
					'	If dr.Item("CurrentHeap") Is Nothing Then
					'		.CurrentHeapThreshold = 0
					'	Else
					'		.CurrentHeapThreshold = dr.Item("CurrentHeap")
					'	End If
					'Catch ex As Exception
					'	.CurrentHeapThreshold = 0
					'End Try

					'Try
					'	If dr.Item("MaxHeap") Is Nothing Then
					'		.MaxHeapThreshold = 0
					'	Else
					'		.MaxHeapThreshold = dr.Item("MaxHeap")
					'	End If
					'Catch ex As Exception
					'	.MaxHeapThreshold = 0
					'End Try

					'Try
					'	If dr.Item("UpTime") Is Nothing Then
					'		.UpTimeThreshold = 0
					'	Else
					'		.UpTimeThreshold = dr.Item("UpTime")
					'	End If
					'Catch ex As Exception
					'	.UpTimeThreshold = 0
					'End Try

					'Try
					'	If dr.Item("HungThreadCount") Is Nothing Then
					'		.HungThreadCountThreshold = 0
					'	Else
					'		.HungThreadCountThreshold = dr.Item("HungThreadCount")
					'	End If
					'Catch ex As Exception
					'	.HungThreadCountThreshold = 0
					'End Try

					'Try
					'	If dr.Item("DumpGenerated") Is Nothing Then
					'		.DumpGeneratedThreshold = 0
					'	Else
					'		.DumpGeneratedThreshold = dr.Item("DumpGenerated")
					'	End If
					'Catch ex As Exception
					'	.DumpGeneratedThreshold = 0
					'End Try

				End With
				MyWebSphereServer = Nothing
			Next
			dr = Nothing

		Catch exception As DataException
			WriteAuditEntry(Now.ToString & " WebSphere Servers data exception " & exception.Message)

		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " WebSphere Servers general exception " & ex.ToString)
		End Try

		InsufficentLicensesTest(MyWebSphereServers)

		dsWebSphere.Dispose()
	End Sub

	Private Sub CreateIBMConnectCollection()
		'start with fresh data
		'Connect to the data source
		Dim dsIBMConnect As New Data.DataSet
		Dim mySecrets As New VSFramework.TripleDES
		Try


			Dim strSQL As String ' = "SELECT Name, Description, Category, UserThreshold, ChatThreshold, NChatThreshold, PlacesThreshold, Enabled, ScanInterval, OffHoursScanInterval, Location, ID, RetryInterval, ResponseThreshold, IPAddress, nserver, stcommlaunch, stcommunity, stconfigurationapp, stplaces, stmux, stusers, stonlinedir, stdirectory, stlogger, stlinks, stprivacy, stsecurity, stpresencemgr, stservicemanager, stpresencesubmgr, steventserver, stpolicy, stconfigurationbridge, stadminsrv, stuserstorage, stchatlogging, stpolling, stresolve, SSL, stpresencecompatmgr FROM SametimeServers"

            strSQL = " select srv.ServerName, srv.ID ServerID, ibmsrv.Enabled, " & _
            "" & _
            "srvt.ServerType, Loc.Location, creds.UserId, creds.Password, ST.LastUpdate, ST.Status, " & _
            "ST.StatusCode, di.CurrentNodeID, ibmsrv.ScanInterval, ibmsrv.RetryInterval, ibmsrv.OffHoursScanInterval, ibmsrv.Category, " & _
            "ibmsrv.ResponseThreshold, ibmsrv.FailureThreshold, " & _
            "srv.Description, srvt.ID as ServerTypeID, srv.IPAddress, ibmsrv.DBName, ibmsrv.DBPort, ibmsrv.DBHostName, credsDB.UserID DBUsername, credsDB.Password DBPassword "

			'strSQL += ",wss.AvgThreadPool, wss.ActiveThreadCount, wss.CurrentHeap, wss.MaxHeap, wss.UpTime, wss.HungThreadCount, wss.DumpGenerated "
            strSQL += "from IBMConnectionsServers ibmsrv " & _
             "inner join Servers srv on srv.ID = ibmsrv.ServerID " & _
             "inner join ServerTypes srvt on srvt.ID=srv.ServerTypeID and srv.ServerTypeID = '27' " & _
             "inner join Locations Loc on Loc.ID=srv.LocationID " & _
             "left outer join Credentials creds on creds.ID=ibmsrv.CredentialId  " & _
             "left outer join Credentials credsDB on credsDB.ID = ibmsrv.DBCredentialsId "



			If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then

				Dim NodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
				strSQL += "  inner join DeviceInventory di on srv.ID=di.DeviceID and di.DeviceTypeId=srv.ServerTypeId  "
				strSQL += " inner join Nodes on (Nodes.ID=di.CurrentNodeId or di.CurrentNodeId=-1) and Nodes.Name='" & NodeName & "'  "

			End If

			strSQL += "left outer join Status st on st.Type=srvt.ServerType and st.Name=srv.ServerName   "

			WriteAuditEntry(Now.ToString & " SQL Statement is " & vbCrLf & strSQL)

			Dim objVSAdaptor As New VSAdaptor
			objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSQL, dsIBMConnect, "Servers")
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateIBMConnectCollection processing code. Exception: " & ex.Message)
			Exit Sub
		End Try



		'***
		'but first delete any that are in the collection but not in the database anymore
		'Delete propogation
		Dim myDataRow As System.Data.DataRow
		Dim MyServerNames As String = ""

		If MyIBMConnectServers.Count > 0 Then
			WriteAuditEntry(Now.ToString & " Checking to see if any IBM Connect Servers should be deleted. ")
			'Get all the names of all the servers in the data table
			For Each myDataRow In dsIBMConnect.Tables("Servers").Rows()
				MyServerNames += myDataRow.Item("ServerName") & "  "
			Next
		End If

		Dim server As MonitoredItems.IBMConnect
		Dim myIndex As Integer

		If MyIBMConnectServers.Count > 0 Then
			For myIndex = MyIBMConnectServers.Count - 1 To 0 Step -1
				server = MyIBMConnectServers.Item(myIndex)
				Try
					WriteAuditEntry(Now.ToString & " Checking to see if IBM Connect Server " & server.Name & " should be deleted...")
					If InStr(MyServerNames, server.Name) > 0 Then
						'the server has not been deleted
						WriteAuditEntry(Now.ToString & " " & server.Name & " is not marked for deletion. ")
					Else
						'the server has been deleted, so delete from the collection
						Try
							MyIBMConnectServers.Delete(server.Name)
							WriteAuditEntry(Now.ToString & " " & server.Name & " has been deleted by the service.")
						Catch ex As Exception
							WriteAuditEntry(Now.ToString & " " & server.Name & " was not deleted by the service because " & ex.Message)
						End Try
					End If
				Catch ex As Exception
					WriteAuditEntry(Now.ToString & " Exception IBM Connection servers Deletion Loop on " & server.Name & ".  The error was " & ex.Message)
				End Try
			Next
		End If

		MyServerNames = Nothing
		server = Nothing
		myIndex = Nothing

		'*** End delete propogation

		Dim i As Integer = 0
		'Add the Sametime servers to the collection
		Try
			If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & "  Reading configuration settings for " & dsIBMConnect.Tables("Servers").Rows.Count & " IBM Connect Servers.")
		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Error with IBM Connect table " & ex.Message, LogLevel.Normal)
		End Try
		Try
			Dim myString As String = ""
			Dim dr As DataRow
			For Each dr In dsIBMConnect.Tables("Servers").Rows
				i += 1
				Dim MyName As String
				If dr.Item("ServerName") Is Nothing Then
					MyName = "IBM Connect #" & i.ToString
				Else
					MyName = dr.Item("ServerName")
				End If
				'See if this server is already in the collection; if so, update its settings otherwise create a new one
				MyIBMConnectServer = MyIBMConnectServers.SearchByName(MyName)
				If MyIBMConnectServer Is Nothing Then
					Try
						MyIBMConnectServer = New MonitoredItems.IBMConnect
						MyIBMConnectServer.Name = MyName
						MyIBMConnectServer.LastScan = Now
						MyIBMConnectServer.NextScan = Now
						MyIBMConnectServer.AlertCondition = False
						MyIBMConnectServer.Status = "Not Scanned"
						MyIBMConnectServer.IncrementUpCount()
						MyIBMConnectServer.ServerType = MyIBMConnectServer.DeviceType
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " Error adding new empty stats collection to " & MyIBMConnectServer.Name)
					End Try
					MyIBMConnectServers.Add(MyIBMConnectServer)
					If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Adding new IBM Connect server -- " & MyIBMConnectServer.Name & " -- to the collection.")
				Else
					If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Updating settings for existing IBM Connect server-- " & MyIBMConnectServer.Name & ".")
				End If

				With MyIBMConnectServer

					'Standard attributes

					Try
						If dr.Item("Location") Is Nothing Then
							.Location = "Not Set"
						Else
							.Location = dr.Item("Location")
						End If
					Catch ex As Exception
						.Location = "Not Set"
					End Try

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
						.OffHours = BoolOffHours
					Catch ex As Exception
						.OffHours = False
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
                        If dr.Item("ResponseThreshold") Is Nothing Then
                            .ResponseThreshold = 100
                        Else
                            .ResponseThreshold = dr.Item("ResponseThreshold")
                        End If
					Catch ex As Exception
						.ResponseThreshold = 100
					End Try


					Try
						If dr.Item("ScanInterval") Is Nothing Then
							.ScanInterval = 10
						Else
							.ScanInterval = dr.Item("ScanInterval")
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
						If dr.Item("RetryInterval") Is Nothing Then
							.RetryInterval = 2
							WriteAuditEntry(Now.ToString & " " & .Name & " IBM Connect retry scan interval not set, using default of 10 minutes.")
						Else
							.RetryInterval = dr.Item("RetryInterval")
						End If
					Catch ex As Exception
						WriteAuditEntry(Now.ToString & " " & .Name & " IBM Connect retry scan interval not set, using default of 10 minutes.")
						.RetryInterval = 2
					End Try

					Try
						If dr.Item("Status") Is Nothing Then
							.Status = "Not Scanned"
						Else
							.Status = dr.Item("Status")
						End If
					Catch ex As Exception
						.Status = "Not Scanned"
					End Try

					Try
						If dr.Item("StatusCode") Is Nothing Then
							.StatusCode = "Maintenance"
						Else
							.StatusCode = dr.Item("StatusCode")
						End If
					Catch ex As Exception
						.StatusCode = "Maintenance"
					End Try

					Try
						If dr.Item("LastUpdate") Is Nothing Then
							.LastScan = Now
						Else
							.LastScan = dr.Item("LastUpdate")
						End If
					Catch ex As Exception
						.LastScan = Now
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
						WriteAuditEntry(Now.ToString & " " & .Name & " IBM Connect insufficent licenses not set.")

					End Try

					Try
						If dr.Item("ServerID") Is Nothing Then
							.ID = -1
						Else
							.ID = dr.Item("ServerID")
						End If
					Catch ex As Exception
						.ID = -1
					End Try



					If (False) Then


						Try
							If dr.Item("NodeID") Is Nothing Then
								.NodeID = -1
							Else
								.NodeID = dr.Item("NodeID")
							End If
						Catch ex As Exception
							.NodeID = -1
						End Try

						Try
							If dr.Item("CellID") Is Nothing Then
								.CellID = -1
							Else
								.CellID = dr.Item("CellID")
							End If
						Catch ex As Exception
							.CellID = -1
						End Try

						Try
							If dr.Item("Hostname") Is Nothing Then
								.HostName = ""
							Else
								.HostName = dr.Item("Hostname")
							End If
						Catch ex As Exception
							.HostName = ""
						End Try

						Try
							If dr.Item("NodeName") Is Nothing Then
								.NodeName = ""
							Else
								.NodeName = dr.Item("NodeName")
							End If
						Catch ex As Exception
							.NodeName = ""
						End Try

						Try
							If dr.Item("CellName") Is Nothing Then
								.CellName = ""
							Else
								.CellName = dr.Item("CellName")
							End If
						Catch ex As Exception
							.CellName = ""
						End Try

						Try
							If dr.Item("CellHostName") Is Nothing Then
								.CellHostName = "RMI"
							Else
								.CellHostName = dr.Item("CellHostName")
							End If
						Catch ex As Exception
							.CellHostName = "RMI"
						End Try

						Try
							If dr.Item("ConnectionType") Is Nothing Then
								.ConnectionType = "RMI"
							Else
								.ConnectionType = dr.Item("ConnectionType")
							End If
						Catch ex As Exception
							.ConnectionType = "RMI"
						End Try

						Try
							If dr.Item("PortNo") Is Nothing Then
								.Port = 1099
							Else
								.Port = dr.Item("PortNo")
							End If
						Catch ex As Exception
							.Port = 1099
						End Try

						Try
							If dr.Item("GlobalSecurity") Is Nothing Then
								.GlobalSecurity = False
							Else
								.GlobalSecurity = dr.Item("GlobalSecurity")
							End If
						Catch ex As Exception
							.GlobalSecurity = False
						End Try

						Try
							If dr.Item("SametimeId") Is Nothing Then
								.SametimeID = -1
							Else
								.SametimeID = dr.Item("SametimeId")
							End If
						Catch ex As Exception
							.SametimeID = -1
						End Try

						Try
							If dr.Item("Realm") Is Nothing Then
								.Realm = ""
							Else
								.Realm = dr.Item("Realm")
							End If
						Catch ex As Exception
							.Realm = ""
						End Try

						Try
							If dr.Item("ServerType") Is Nothing Then
								.ServerType = "IBM Connect"
							Else
								.ServerType = dr.Item("ServerType")
							End If
						Catch ex As Exception
							.ServerType = "WebSphere"
						End Try
					End If
					Try
						If dr.Item("UserId") Is Nothing Then
							.UserName = ""
						Else
							.UserName = dr.Item("UserId")
						End If
					Catch ex As Exception
						.UserName = ""
					End Try

					Try
						If dr.Item("Password") Is Nothing Then
							.Password = ""
						Else
							Dim myPass As Byte()
							Dim Password As String = ""
							Try
								Dim str1() As String
								str1 = dr.Item("Password").ToString().Split(",")
								Dim bstr1(str1.Length - 1) As Byte
								For j As Integer = 0 To str1.Length - 1
									bstr1(j) = str1(j).ToString()
								Next
								myPass = bstr1

								Password = mySecrets.Decrypt(myPass) 'password in clear text, stored in memory now

							Catch ex As Exception
								Password = ""
								WriteAuditEntry(Now.ToString & " Error decrypting the Notes password.  " & ex.ToString)
							End Try

							.Password = Password
						End If
					Catch ex As Exception
						.Password = ""
					End Try

					Try
						If dr.Item("ServerTypeID") Is Nothing Then
							.SametimeID = 27
						Else
							.SametimeID = dr.Item("ServerTypeID")
						End If
					Catch ex As Exception
						.SametimeID = 27
					End Try

					Try
                        If dr.Item("FailureThreshold") Is Nothing Then
                            .FailureThreshold = 3
                        Else
                            .FailureThreshold = dr.Item("FailureThreshold")
                        End If
					Catch ex As Exception
						.FailureThreshold = 3
					End Try

					Try
						If dr.Item("ConsOvrThresholdBefAlert") Is Nothing Then
							.ServerDaysAlert = 14
						Else
							.ServerDaysAlert = dr.Item("ConsOvrThresholdBefAlert")
						End If
					Catch ex As Exception
						.ServerDaysAlert = 14
					End Try

					Try
						.ServerName = .Name
					Catch ex As Exception

					End Try

					Try
						If dr.Item("IPAddress") Is Nothing Then
							.IPAddress = ""
						Else
							.IPAddress = dr.Item("IPAddress")
						End If
					Catch ex As Exception
						.IPAddress = ""
					End Try

					Try
						If dr.Item("DBHostName") Is Nothing Then
							.DBHostName = ""
						Else
							.DBHostName = dr.Item("DBHostName")
						End If
					Catch ex As Exception
						.DBHostName = ""
                    End Try

					Try
						If dr.Item("DBPort") Is Nothing Then
							.DBPort = ""
						Else
							.DBPort = dr.Item("DBPort")
						End If
					Catch ex As Exception
						.DBPort = ""
					End Try

					Try
						If dr.Item("DBUsername") Is Nothing Then
							.DBUserName = ""
						Else
							.DBUserName = dr.Item("DBUsername")
						End If
					Catch ex As Exception
						.DBUserName = ""
					End Try

					Try
						If dr.Item("DBPassword") Is Nothing Then
							.DBPassword = ""
						Else
							Dim myPass As Byte()
							Dim Password As String = ""
							Try
								Dim str1() As String
								str1 = dr.Item("DBPassword").ToString().Split(",")
								Dim bstr1(str1.Length - 1) As Byte
								For j As Integer = 0 To str1.Length - 1
									bstr1(j) = str1(j).ToString()
								Next
								myPass = bstr1

								Password = mySecrets.Decrypt(myPass) 'password in clear text, stored in memory now

							Catch ex As Exception
								Password = ""
								WriteAuditEntry(Now.ToString & " Error decrypting the DB password.  " & ex.ToString)
							End Try

							.DBPassword = Password
						End If
					Catch ex As Exception
						.DBPassword = ""
					End Try






                    Try

                        'Set them all to false first since no row is generated if it is disabled
                        .TestCreateActivity = False
                        .TestCreateBlog = False
                        .TestCreateBookmarks = False
                        .TestCreateCommunities = False
                        .TestCreateFiles = False
                        .TestCreateForums = False
                        .TestCreateWikis = False
                        .TestSearchProfiles = False

                        Dim strSql As String = "Select tm.Tests, ibm.ResponseThreshold, ibm.EnableSimulationTests from IBMConnectionsTests ibm inner join TestsMaster tm on ibm.Id = tm.Id where ibm.ServerId = '" & .ID & "'"
                        Dim objVSAdaptor As New VSAdaptor
                        Dim dataset As New DataSet()
                        objVSAdaptor.FillDatasetAny("VitalSigns", "servers", strSql, dataset, "Servers")
                        If dataset.Tables.Count > 0 Then
                            For Each row As DataRow In dataset.Tables(0).Rows
                                Select Case row("Tests").ToString()

                                    Case "Create Activity"
                                        .CreateActivityThreshold = row("ResponseThreshold").ToString()
                                        .TestCreateActivity = IIf(row("EnableSimulationTests").ToString() = "True", True, False)

                                    Case "Create Blog"
                                        .CreateBlogThreshold = row("ResponseThreshold").ToString()
                                        .TestCreateBlog = IIf(row("EnableSimulationTests").ToString() = "True", True, False)

                                    Case "Create Bookmark"
                                        .CreateBookmarkThreshold = row("ResponseThreshold").ToString()
                                        .TestCreateBookmarks = IIf(row("EnableSimulationTests").ToString() = "True", True, False)

                                    Case "Create Community"
                                        .CreateCommunitiesThreshold = row("ResponseThreshold").ToString()
                                        .TestCreateCommunities = IIf(row("EnableSimulationTests").ToString() = "True", True, False)

                                    Case "Create File"
                                        .CreateFilesThreshold = row("ResponseThreshold").ToString()
                                        .TestCreateFiles = IIf(row("EnableSimulationTests").ToString() = "True", True, False)

                                    Case "Create Wiki"
                                        .CreateWikisThreshold = row("ResponseThreshold").ToString()
                                        .TestCreateWikis = IIf(row("EnableSimulationTests").ToString() = "True", True, False)

                                    Case "Search Profile"
                                        .SearchProfilesThreshold = row("ResponseThreshold").ToString()
                                        .TestSearchProfiles = IIf(row("EnableSimulationTests").ToString() = "True", True, False)

                                End Select
                            Next
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " IBM Connect Servers exception for tests " & ex.Message)
                    End Try

                End With


                MyIBMConnectServer = Nothing
            Next
			dr = Nothing

		Catch exception As DataException
			WriteAuditEntry(Now.ToString & " IBM Connect Servers data exception " & exception.Message)

		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " IBM Connect Servers general exception " & ex.ToString)
		End Try

		InsufficentLicensesTest(MyIBMConnectServers)

		dsIBMConnect.Dispose()
	End Sub

	Private Sub InsufficentLicensesTest(ByRef coll As System.Collections.CollectionBase)

		If (coll.Count = 0) Then
			Return
		End If

		Dim ServerType As String = ""
		Dim ServerTypeForTypeAndName As String = ""

		Select Case coll.GetType()

			Case GetType(MonitoredItems.BlackBerryServerCollection)
				ServerType = "BES"
				ServerTypeForTypeAndName = "BlackBerry Server"

			Case GetType(MonitoredItems.MailServiceCollection)
				ServerType = "Mail"
				ServerTypeForTypeAndName = "MS"

			Case GetType(MonitoredItems.SametimeServersCollection)
				ServerType = "Sametime"
				ServerTypeForTypeAndName = "Sametime"

			Case GetType(MonitoredItems.URLCollection)
				ServerType = "URL"
				ServerTypeForTypeAndName = "URL"

			Case GetType(MonitoredItems.CloudCollection)

				ServerType = "Cloud"
				ServerTypeForTypeAndName = "Cloud"

			Case GetType(MonitoredItems.WebSphereCollection)
				ServerType = CType(coll(0), MonitoredItems.WebSphere).ServerType
                ServerTypeForTypeAndName = CType(coll(0), MonitoredItems.WebSphere).ServerType

            Case GetType(MonitoredItems.IBMConnectCollection)
                ServerType = CType(coll(0), MonitoredItems.IBMConnect).ServerType
                ServerTypeForTypeAndName = CType(coll(0), MonitoredItems.IBMConnect).ServerType

        End Select

		CheckForInsufficentLicenses(coll, ServerType, ServerTypeForTypeAndName)

	End Sub

#End Region


End Class
'http://sametime.jnittech.com:9083/ConferenceFocus/monitoring/MonitoringRestServlet?method=ConferenceData&conferenceId=TCSPI_ID=[68]com.ibm.mediaserver.telephony.conferencing.service.ConferenceService/1001
Public Class ConferenceData
	Public Property CN As String()
	Public Property MediaFlagString As String()
	Public Property TypeDescription As String()
	Public Property ModeratorUri As String()
	Public Property MaximumUsers As String()
	Public Property ElapsedTime As String()
	Public Property McuURI As String()
	Public Property ProxyStateName As String()
	Public Property CurrentUsers As String()
	Public Property StartTime As String()
	Public Property ExpectedUsers As String()
	Public Property ServiceId As String()
	Public Property ConferenceId As String()
	Public Property Title As String()
End Class

Public Class AcrossConferenceData
	Public Property P2PActiveCalls As String()
	Public Property AllActiveUsers As String()
	Public Property MCUActiveUsers As String()
	Public Property ExternallyInvokedConferences As String()
	Public Property TelephonyActivities As String()
	Public Property MCUCallsStarted As String()
	Public Property PSActiveCalls As String()
	Public Property AllCallsStarted As String()
	Public Property AllActiveCalls As String()
	Public Property PSCallsStarted As String()
	Public Property MCUActiveCalls As String()
	Public Property P2PActiveUsers As String()
	Public Property P2PCallsStarted As String()
	Public Property PSActiveUsers As String()
End Class
'http://sametime.jnittech.com:9083/ConferenceFocus/monitoring/MonitoringRestServlet?method=ConferenceIdList

Public Class ConferenceIdList
	Public Property ConferenceIdList As String()
End Class

