Imports System.Threading
Imports VSFramework
Imports VSNext.Mongo.Entities
Imports MongoDB.Bson
Imports MongoDB.Driver

Partial Public Class VitalSignsPlusCore


#Region "Create Collections of items to Monitor"

    Public Sub CreateCollections()

        Try
            'CreateBlackBerryServersCollection()
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

        Try
            CreateIBMFileNetCollection()
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating IBM FileNet collection: " & ex.Message)
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
                            .LastScan = Now.AddMinutes(-30)
                        Else
							.LastScan = dr.Item("LastUpdate")
						End If
					Catch ex As Exception
                        .LastScan = Now.AddMinutes(-30)
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
    Private Function getCurrentNode() As String
        Dim NodeName As String = ""
        If System.Configuration.ConfigurationManager.AppSettings("VSNodeName") <> Nothing Then
            NodeName = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
        End If
        Return NodeName
    End Function
    Private Sub CreateMailServicesCollection()
        WriteAuditEntry(Now.ToString & "  Reading configuration settings for Mail Services.")
        'start with fresh data

        'Connect to the data source
        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)

        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Mail.ToDescription())
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.LocationId) _
                .Include(Function(x) x.ScanInterval) _
                .Include(Function(x) x.Category) _
                .Include(Function(x) x.IsEnabled) _
                .Include(Function(x) x.IPAddress) _
                .Include(Function(x) x.OffHoursScanInterval) _
                .Include(Function(x) x.RetryInterval) _
                .Include(Function(x) x.ResponseTime) _
                .Include(Function(x) x.ConsecutiveFailuresBeforeAlert) _
                .Include(Function(x) x.PortNumber) _
                .Include(Function(x) x.CurrentNode)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()

            Dim repositoryStatus As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDefStatus As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Mail.ToDescription())
            Dim projectionDefStatus As ProjectionDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Project _
                .Include(Function(x) x.StatusCode) _
                .Include(Function(x) x.CurrentStatus) _
                .Include(Function(x) x.LastUpdated) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.DeviceName)

            listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList()

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateMailServicesCollection processing code. Exception: " & ex.Message)
            Exit Sub
        End Try

        If listOfServers.Count = 0 Then
            WriteAuditEntry(Now.ToString & " There are no mail services defined. ")
            Exit Sub
        End If

        'Add the Mail Services  to the collection

        Dim i As Integer = 0
        For Each entity As VSNext.Mongo.Entities.Server In listOfServers
            MyMailService = Nothing
            i += 1
            Dim MyName As String
            If entity.DeviceName Is Nothing Then
                MyName = "Mail Service" & i.ToString
            Else
                MyName = entity.DeviceName
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
                MyMailService.LastScan = Now.AddMinutes(-30)
                MyMailService.NextScan = Now
                MyMailService.StatusCode = "Maintenance"
                MyMailServices.Add(MyMailService)
                If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Adding new Mail Service--" & MyMailService.Name & "-- to the collection.")
            Else
                If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Updating existing Mail Service--" & MyMailService.Name & ".")
            End If

            With MyMailService

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
                    If entity.IPAddress Is Nothing Then
                        .IPAddress = ""
                        '.Location = "Unknown"
                    Else
                        .IPAddress = entity.IPAddress
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
                    If entity.LocationId Is Nothing Then
                        .Location = "Not Set"
                    Else

                        Dim repositoryLocation As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Location)(connectionString)
                        Dim filterDefLocation As FilterDefinition(Of VSNext.Mongo.Entities.Location) = repositoryLocation.Filter.Eq(Function(x) x.Id, entity.LocationId)
                        .Location = repositoryLocation.Find(filterDefLocation).ToList()(0).LocationName

                    End If
                Catch ex As Exception
                    .Location = "Not Set"
                End Try

                'This part is not being used any more, bu used elsewhere in product
                Try
                    'If Not (dr.Item("ServerName")) Is Nothing Then
                    '.ServerName = dr.Item("ServerName")
                    ' End If
                    .ServerName = "None"
                Catch ex As Exception
                    .ServerName = "None"
                End Try

                Try
                    If entity.IsEnabled Is Nothing Then
                        .Enabled = False
                    Else
                        .Enabled = entity.IsEnabled
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
                    If entity.Description Is Nothing Then
                        .Description = " "
                    Else
                        .Description = entity.Description
                    End If
                Catch ex As Exception
                    .Description = " "
                End Try

                Try
                    If entity.ResponseTime Is Nothing Then
                        .ResponseThreshold = 100
                    Else
                        .ResponseThreshold = entity.ResponseTime
                    End If
                Catch ex As Exception
                    .ResponseThreshold = 100
                End Try

                Try
                    If entity.ConsecutiveFailuresBeforeAlert Is Nothing Then
                        .ResponseThreshold = 100
                    Else
                        .FailureThreshold = entity.ConsecutiveFailuresBeforeAlert
                    End If
                Catch ex As Exception
                    .FailureThreshold = 2
                End Try
                Try
                    If entity.ScanInterval Is Nothing Then
                        .ScanInterval = 10
                    Else
                        .ScanInterval = entity.ScanInterval
                    End If
                Catch ex As Exception
                    .ScanInterval = 10
                End Try

                Try
                    If entity.OffHoursScanInterval Is Nothing Then
                        .OffHoursScanInterval = 30
                    Else
                        .OffHoursScanInterval = entity.OffHoursScanInterval
                    End If
                Catch ex As Exception
                    .OffHoursScanInterval = 30
                End Try
                Try
                    If entity.Category Is Nothing Then
                        .Category = "Not Categorized"
                    Else
                        .Category = entity.Category
                    End If
                Catch ex As Exception
                    .Category = "Not Categorized"
                End Try

                Try
                    If entity.RetryInterval Is Nothing Then
                        .RetryInterval = 2
                        WriteAuditEntry(Now.ToString & " " & .Name & " Mail Service retry scan interval not set, using default of 10 minutes.")
                    Else
                        .RetryInterval = entity.RetryInterval
                    End If
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " " & .Name & " Mail Service retry scan interval not set, using default of 10 minutes.")
                    .RetryInterval = 2
                End Try

                Try
                    Dim entityStatus As VSNext.Mongo.Entities.Status
                    Try
                        entityStatus = listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.DeviceName)).ToList()(0)
                    Catch ex As Exception
                    End Try

                    Try
                        If entityStatus.CurrentStatus Is Nothing Then
                            .Status = "Not Scanned"
                        Else
                            .Status = entityStatus.CurrentStatus
                        End If
                    Catch ex As Exception
                        .Status = "Not Scanned"
                    End Try

                    Try
                        If entityStatus.LastUpdated Is Nothing Then
                            .LastScan = Now.AddMinutes(-30)
                        Else
                            .LastScan = entityStatus.LastUpdated
                        End If
                    Catch ex As Exception
                        .LastScan = Now.AddMinutes(-30)
                    End Try

                Catch ex As Exception

                End Try


                Try
                    If entity.CurrentNode Is Nothing Then
                        .InsufficentLicenses = True
                    Else
                        If entity.CurrentNode.ToString() <> getCurrentNode() Then
                            .InsufficentLicenses = True
                        Else
                            .InsufficentLicenses = False
                        End If
                    End If
                    .CurrentNode = entity.CurrentNode
                Catch ex As Exception
                    WriteAuditEntry(Now.ToString & " " & .Name & " Mail Servers insufficient licenses not set.")
                End Try

            End With
        Next

        InsufficentLicensesTest(MyMailServices)

        'Free memory
        MyMailService = Nothing

    End Sub

    Private Sub CreateSametimeServersCollection()
        'start with fresh data
        'Connect to the data source
        Dim mySecrets As New VSFramework.TripleDES

        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)

        Try

            'removed UserThreshold, ChatThreshold, NChatThreshold, PlacesThreshold

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Sametime.ToDescription())
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.RequireSSL) _
                .Include(Function(x) x.LocationId) _
                .Include(Function(x) x.Description) _
                .Include(Function(x) x.IsEnabled) _
                .Include(Function(x) x.IPAddress) _
                .Include(Function(x) x.ResponseTime) _
                .Include(Function(x) x.OffHoursScanInterval) _
                .Include(Function(x) x.ScanInterval) _
                .Include(Function(x) x.Category) _
                .Include(Function(x) x.RetryInterval) _
                .Include(Function(x) x.MeetingHostName) _
                .Include(Function(x) x.CollectMeetingStatistics) _
                .Include(Function(x) x.MeetingRequireSSL) _
                .Include(Function(x) x.MeetingPort) _
                .Include(Function(x) x.ConferenceRequireSSL) _
                .Include(Function(x) x.ConferencePort) _
                .Include(Function(x) x.ConferenceHostName) _
                .Include(Function(x) x.Platform) _
                .Include(Function(x) x.User1CredentialsId) _
                .Include(Function(x) x.User2CredentialsId) _
                .Include(Function(x) x.ExtendedStatisticsPort) _
                .Include(Function(x) x.CollectExtendedStatistics) _
                .Include(Function(x) x.TestChatSimulation) _
                .Include(Function(x) x.DatabaseSettingsHostName) _
                .Include(Function(x) x.DatabaseSettingsPort) _
                .Include(Function(x) x.DatabaseSettingsCredentialsId) _
                .Include(Function(x) x.DominoServerName) _
                 .Include(Function(x) x.LastStatsProcessedDate) _
                .Include(Function(x) x.CurrentNode)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()

            Dim repositoryStatus As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDefStatus As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Sametime.ToDescription())
            Dim projectionDefStatus As ProjectionDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Project _
                .Include(Function(x) x.StatusCode) _
                .Include(Function(x) x.CurrentStatus) _
                .Include(Function(x) x.LastUpdated) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.DeviceName)

            listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList()

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateSametimeServersCollection processing code. Exception: " & ex.Message)
            Exit Sub
        End Try



        '***
        'but first delete any that are in the collection but not in the database anymore
        'Delete propogation
        Dim MyServerNames As String

        If mySametimeServers.Count > 0 Then
            WriteAuditEntry(Now.ToString & " Checking to see if any ST databases should be deleted. ")
            'Get all the names of all the servers in the data table
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                MyServerNames += entity.DeviceName & "  "
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
            If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & "  Reading configuration settings for " & listOfServers.Count.ToString() & " Sametime Servers.")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error with sametime table " & ex.Message, LogLevel.Normal)
        End Try
        Try
            Dim myString As String = ""
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                i += 1
                Dim MyName As String
                If entity.DeviceName Is Nothing Then
                    MyName = "Sametime Server #" & i.ToString
                Else
                    MyName = entity.DeviceName
                End If
                'See if this server is already in the collection; if so, update its settings otherwise create a new one
                MySametimeServer = mySametimeServers.SearchByName(MyName)
                If MySametimeServer Is Nothing Then
                    Try
                        MySametimeServer = New MonitoredItems.SametimeServer
                        MySametimeServer.Name = MyName
                        MySametimeServer.LastScan = Now.AddMinutes(-30)
                        MySametimeServer.NextScan = Now
                        MySametimeServer.AlertCondition = False
                        MySametimeServer.Status = "Not Scanned"
                        MySametimeServer.IncrementUpCount()
                        MySametimeServer.ServerType = VSNext.Mongo.Entities.Enums.ServerType.Sametime.ToDescription()
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error adding new empty stats collection to " & MySametimeServer.Name)
                    End Try
                    mySametimeServers.Add(MySametimeServer)
                    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Adding new Sametime server -- " & MySametimeServer.Name & " -- to the collection.")
                Else
                    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Updating settings for existing Sametime server-- " & MySametimeServer.Name & ".")
                End If

                With MySametimeServer

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

                    'Sametime Specific
                    Try
                        If entity.RequireSSL Then
                            .SSL = True
                        Else
                            .SSL = False
                        End If
                    Catch ex As Exception
                        .SSL = False
                    End Try

                    'BEGIN Running Processes to monitor *****************
                    'Dim myMonitoredServices As New MonitoredItems.SametimeMonitoredProcessCollection

                    'Try
                    '    If dr.Item("nserver") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "nserver"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stcommlaunch") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stcommlaunch"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stcommunity") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stcommunity"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try


                    'Try
                    '    If dr.Item("stconfigurationapp") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stconfigurationapp"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stplaces") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stplaces"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stmux") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stmux"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stusers") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stusers"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stonlinedir") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stonlinedir"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stdirectory") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stdirectory"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stlogger") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stlogger"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try


                    'Try
                    '    If dr.Item("stlinks") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stlinks"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try


                    'Try
                    '    If dr.Item("stprivacy") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stprivacy"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stsecurity") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stsecurity"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stpresencemgr") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stpresencemgr"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stservicemanager") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stservicemanager"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stpresencesubmgr") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stpresencesubmgr"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try


                    'Try
                    '    If dr.Item("steventserver") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "steventserver"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stpolicy") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stpolicy"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stconfigurationbridge") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stconfigurationbridge"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try


                    'Try
                    '    If dr.Item("stadminsrv") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stadminsrv"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stuserstorage") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stuserstorage"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try
                    'Try
                    '    If dr.Item("stchatlogging") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stchatlogging"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stpolling") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stpolling"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If dr.Item("stresolve") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stresolve"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try


                    'Try
                    '    If dr.Item("stpresencecompatmgr") = True Then
                    '        Dim myMonitoredService As New MonitoredItems.SametimeMonitoredProcess
                    '        myMonitoredService.Name = "stpresencecompatmgr"
                    '        myMonitoredServices.Add(myMonitoredService)
                    '    End If
                    'Catch ex As Exception

                    'End Try

                    'Try
                    '    If myMonitoredServices.Count > 0 Then
                    '        MySametimeServer.MonitoredProcesses = myMonitoredServices
                    '    End If

                    'Catch ex As Exception

                    'End Try

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
                        If entity.LocationId Is Nothing Then
                            .Location = "Not Set"
                        Else

                            Dim repositoryLocation As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Location)(connectionString)
                            Dim filterDefLocation As FilterDefinition(Of VSNext.Mongo.Entities.Location) = repositoryLocation.Filter.Eq(Function(x) x.Id, entity.LocationId)
                            .Location = repositoryLocation.Find(filterDefLocation).ToList()(0).LocationName

                        End If
                    Catch ex As Exception
                        .Location = "Not Set"
                    End Try

                    Try
                        If entity.Description Is Nothing Then
                            .Description = ""
                        Else
                            .Description = entity.Description
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

                    If .Enabled = True And .Status = "Disabled" Then
                        .Status = "Not Scanned"
                    End If

                    Try
                        If entity.IPAddress Is Nothing Then
                            .IPAddress = ""
                        Else
                            .IPAddress = entity.IPAddress
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Invalid IP Address")
                        .IPAddress = ""
                        .Status = "Invalid IP Address"
                        .Enabled = True
                    End Try

                    Try
                        If entity.ResponseTime Is Nothing Then
                            .ResponseThreshold = 100
                        Else
                            .ResponseThreshold = entity.ResponseTime
                        End If
                    Catch ex As Exception
                        .ResponseThreshold = 100
                    End Try

                    Try
                        If entity.DominoServerName Is Nothing Then
                            .DominoServerName = ""
                        Else
                            .DominoServerName = entity.DominoServerName
                        End If
                    Catch ex As Exception
                        .DominoServerName = ""
                    End Try

                    Try
                        If entity.LastStatsProcessedDate Is Nothing Then
                            .LastStatsProcessedDate = Date.MinValue
                        Else
                            .LastStatsProcessedDate = DateTime.Parse(entity.LastStatsProcessedDate)
                        End If
                    Catch ex As Exception
                        .LastStatsProcessedDate = Date.MinValue
                    End Try

                    Try
                        If entity.ScanInterval Is Nothing Then
                            .ScanInterval = 10
                        Else
                            .ScanInterval = entity.ScanInterval
                        End If
                    Catch ex As Exception
                        .ScanInterval = 10
                    End Try

                    Try
                        If entity.OffHoursScanInterval Is Nothing Then
                            .OffHoursScanInterval = 30
                        Else
                            .OffHoursScanInterval = entity.OffHoursScanInterval
                        End If
                    Catch ex As Exception
                        .OffHoursScanInterval = 30
                    End Try
                    Try
                        '   WriteAuditEntry(Now.ToString & "Adding Category")
                        If entity.Category Is Nothing Then
                            .Category = "Not Categorized"
                        Else
                            .Category = entity.Category
                        End If
                    Catch ex As Exception
                        .Category = "Not Categorized"
                    End Try



                    Try
                        If entity.RetryInterval Is Nothing Then
                            .RetryInterval = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " Notes Database retry scan interval not set, using default of 10 minutes.")
                        Else
                            .RetryInterval = entity.RetryInterval
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Notes Database retry scan interval not set, using default of 10 minutes.")
                        .RetryInterval = 2
                    End Try

                    Try
                        Dim entityStatus As VSNext.Mongo.Entities.Status
                        Try
                            entityStatus = listOfStatus.Where(Function(x) x.DeviceType.Equals(entity.DeviceType) And x.DeviceName.Equals(entity.DeviceName)).ToList()(0)
                        Catch ex As Exception
                        End Try

                        Try
                            If entityStatus.CurrentStatus Is Nothing Then
                                .Status = "Not Scanned"
                            Else
                                .Status = entityStatus.CurrentStatus
                            End If
                        Catch ex As Exception
                            .Status = "Not Scanned"
                        End Try

                        Try
                            If entityStatus.LastUpdated Is Nothing Then
                                .LastScan = Now.AddMinutes(-30)
                            Else
                                .LastScan = entityStatus.LastUpdated
                            End If
                        Catch ex As Exception
                            .LastScan = Now.AddMinutes(-30)
                        End Try

                    Catch ex As Exception

                    End Try

                    'Try
                    '    If dr.Item("CurrentNodeID") Is Nothing Then
                    '        .InsufficentLicenses = True
                    '    Else
                    '        If dr.Item("CurrentNodeID").ToString() = "-1" Then
                    '            .InsufficentLicenses = True
                    '        Else
                    '            .InsufficentLicenses = False
                    '        End If

                    '    End If
                    'Catch ex As Exception
                    '    '7/8/2015 NS modified for VSPLUS-1959
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " Sametime insufficient licenses not set.")

                    'End Try

                    Try
                        If entity.MeetingHostName Is Nothing Then
                            .WsMeetingHost = ""
                        Else
                            .WsMeetingHost = entity.MeetingHostName
                        End If
                    Catch ex As Exception
                        .WsMeetingHost = ""
                    End Try

                    Try
                        If entity.CollectMeetingStatistics Is Nothing Then
                            .WsScanMeetingServer = False
                        Else
                            .WsScanMeetingServer = entity.CollectMeetingStatistics
                        End If
                    Catch ex As Exception
                        .WsScanMeetingServer = False
                    End Try

                    Try
                        If entity.MeetingRequireSSL Is Nothing Then
                            .WsMeetingRequireSSL = False
                        Else
                            .WsMeetingRequireSSL = entity.MeetingRequireSSL
                        End If
                    Catch ex As Exception
                        .WsMeetingRequireSSL = False
                    End Try

                    Try
                        .WsMeetingPort = entity.MeetingPort
                    Catch ex As Exception
                        .WsMeetingPort = "80"
                    End Try


                    Try
                        If entity.CollectConferenceStatistics Is Nothing Then
                            .WsScanMediaServer = False
                        Else
                            .WsScanMediaServer = entity.CollectConferenceStatistics
                        End If
                    Catch ex As Exception
                        .WsScanMediaServer = False
                    End Try

                    Try
                        If entity.ConferenceRequireSSL Is Nothing Then
                            .WsMediaRequireSSL = False
                        Else
                            .WsMediaRequireSSL = entity.ConferenceRequireSSL
                        End If
                    Catch ex As Exception
                        .WsMediaRequireSSL = False
                    End Try

                    Try
                        .WsMediaPort = entity.ConferencePort
                    Catch ex As Exception
                        .WsMediaPort = "80"
                    End Try

                    .WsMediaHost = entity.ConferenceHostName

                    .Platform = entity.Platform
                    Dim myRegistry As New VSFramework.RegistryHandler()
                    Dim userOne As String = "", userTwo As String = "", pass1 As Byte(), pass2 As Byte()
                    Dim strEncryptedPassword As String = ""

                    Try

                        Dim repositoryCredentials As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Credentials)(connectionString)
                        Dim filterDefCredentials As FilterDefinition(Of VSNext.Mongo.Entities.Credentials) = repositoryCredentials.Filter.Eq(Function(x) x.Id, entity.User1CredentialsId)
                        Dim entityCredentials As VSNext.Mongo.Entities.Credentials = repositoryCredentials.Find(filterDefCredentials).ToList()(0)

                        userOne = entityCredentials.UserId
                        WriteAuditEntry(Now.ToString & " Sametime User One is " & userOne)
                        WriteAuditEntry(Now.ToString & " Sametime User One pwd is " & entityCredentials.Password)
                        strEncryptedPassword = entityCredentials.Password   'sametime password as encrypted byte stream
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
                        WriteAuditEntry(Now.ToString & " Sametime User One decrypt pwd is " & entity.Password)
                    Catch ex As Exception
                        .UserId1 = ""
                        WriteAuditEntry(Now.ToString & " Cannot Set user id1 " + ex.ToString(), LogLevel.Normal)
                    End Try

                    Try

                        Dim repositoryCredentials As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Credentials)(connectionString)
                        Dim filterDefCredentials As FilterDefinition(Of VSNext.Mongo.Entities.Credentials) = repositoryCredentials.Filter.Eq(Function(x) x.Id, entity.User2CredentialsId)
                        Dim entityCredentials As VSNext.Mongo.Entities.Credentials = repositoryCredentials.Find(filterDefCredentials).ToList()(0)

                        userTwo = entityCredentials.UserId
                        WriteAuditEntry(Now.ToString & " Sametime User two is " & userTwo)
                        WriteAuditEntry(Now.ToString & " Sametime User One is " & entity.Password)
                        strEncryptedPassword = entityCredentials.Password  'sametime password as encrypted byte stream
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
                        .ExtendedChatPort = entity.ExtendedStatisticsPort
                    Catch ex As Exception
                        .ExtendedChatPort = "80"
                    End Try



                    Try
                        If entity.CollectExtendedStatistics Is Nothing Then
                            .CollectExtendedChat = False
                        Else
                            .CollectExtendedChat = entity.CollectExtendedStatistics
                        End If
                    Catch ex As Exception
                        .CollectExtendedChat = False
                    End Try

                    Try
                        If entity.TestChatSimulation Is Nothing Then
                            .TestChatSimulation = False
                        Else
                            .TestChatSimulation = entity.TestChatSimulation
                        End If
                    Catch ex As Exception
                        .TestChatSimulation = False
                    End Try

                    Try
                        If entity.DatabaseSettingsHostName Is Nothing Then
                            .DBHostName = ""
                        Else
                            .DBHostName = entity.DatabaseSettingsHostName
                        End If
                    Catch ex As Exception
                        .DBHostName = ""
                    End Try

                    Try
                        If entity.DatabaseSettingsPort Is Nothing Then
                            .DBPort = 0
                        Else
                            .DBPort = entity.DatabaseSettingsPort
                        End If
                    Catch ex As Exception
                        .DBPort = 0
                    End Try

                    Try

                        Dim repositoryCredentials As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Credentials)(connectionString)
                        Dim filterDefCredentials As FilterDefinition(Of VSNext.Mongo.Entities.Credentials) = repositoryCredentials.Filter.Eq(Function(x) x.Id, entity.DatabaseSettingsCredentialsId)
                        Dim entityCredentials As VSNext.Mongo.Entities.Credentials = repositoryCredentials.Find(filterDefCredentials).ToList()(0)

                        Dim user As String = entity.Username
                        WriteAuditEntry(Now.ToString & " Sametime User two is " & user)
                        WriteAuditEntry(Now.ToString & " Sametime User One is " & entity.Password)
                        strEncryptedPassword = entity.Password  'sametime password as encrypted byte stream
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
                        .DBUserName = user
                    Catch ex As Exception
                        .DBUserName = ""
                        WriteAuditEntry(Now.ToString & " Cannot Set user db2 password ", LogLevel.Normal)
                    End Try

                    Try
                        If entity.CurrentNode Is Nothing Then
                            .InsufficentLicenses = True
                        Else
                            If entity.CurrentNode.ToString() <> getCurrentNode() Then
                                .InsufficentLicenses = True
                            Else
                                .InsufficentLicenses = False
                            End If
                        End If
                        .CurrentNode = entity.CurrentNode
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Sametime Servers insufficient licenses not set.")
                    End Try

                End With
                MySametimeServer = Nothing
            Next

        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Sametime Servers data exception " & exception.Message)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Sametime Servers general exception " & ex.ToString)
        End Try

        InsufficentLicensesTest(mySametimeServers)

    End Sub

    Private Sub CreateURLCollection()
        'start with fresh data

        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)

        Try

            'removed LastChecked, LastStatus, NextScan

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.URL.ToDescription())
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.Category) _
                .Include(Function(x) x.ConsecutiveFailuresBeforeAlert) _
                .Include(Function(x) x.Username) _
                .Include(Function(x) x.Password) _
                .Include(Function(x) x.IsEnabled) _
                .Include(Function(x) x.OffHoursScanInterval) _
                .Include(Function(x) x.ResponseTime) _
                .Include(Function(x) x.RetryInterval) _
                .Include(Function(x) x.SearchString) _
                .Include(Function(x) x.AlertIfStringFound) _
                .Include(Function(x) x.ScanInterval) _
                .Include(Function(x) x.IPAddress) _
                .Include(Function(x) x.LocationId) _
                .Include(Function(x) x.PortNumber) _
                .Include(Function(x) x.CurrentNode)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()

            Dim repositoryStatus As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDefStatus As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.URL.ToDescription())
            Dim projectionDefStatus As ProjectionDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Project _
                .Include(Function(x) x.StatusCode) _
                .Include(Function(x) x.CurrentStatus) _
                .Include(Function(x) x.LastUpdated) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.DeviceName)

            listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList()

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in URL processing code. Exception: " & ex.Message)
            '  Exit Sub
        End Try

        '***


        'Add the URLs  to the collection
        WriteAuditEntry(Now.ToString & "  Reading configuration settings for URLs.")
        Try
            Dim i As Integer = 0
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                i += 1
                Dim MyName As String
                If entity.DeviceName Is Nothing Then
                    MyName = "URL" & i.ToString
                Else
                    MyName = entity.DeviceName
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
                    MyURL.LastScan = Now.AddMinutes(-30)
                    MyURL.NextScan = Now
                    MyURL.SearchString = ""
                    '5/2/2016 NS modified for VSPLUS-2887
                    'MyURL.Location = dr.Item("Location")
                    MyURL.ServerType = VSNext.Mongo.Entities.Enums.ServerType.URL.ToDescription()
                    MyURLs.Add(MyURL)

                    WriteAuditEntry(Now.ToString & " Adding new URL--" & MyURL.Name & "-- to the collection.", LogLevel.Verbose)
                Else
                    WriteAuditEntry(Now.ToString & " Updating existing URL--" & MyURL.Name & ".", LogLevel.Verbose)
                End If

                With MyURL

                    WriteAuditEntry(Now.ToString & " Configuring URL: " & entity.DeviceName, LogLevel.Verbose)

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
                        If entity.Category Is Nothing Then
                            .Category = "Internet"
                            WriteAuditEntry(Now.ToString & " " & .Name & " URL Category not set, using default of 'Domino'.")
                        Else
                            .Category = entity.Category
                        End If
                    Catch ex As Exception
                        .Category = "Internet"
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL Category not set, using default of 'NotesMail Probe'.")
                    End Try

                    Try
                        If entity.ConsecutiveFailuresBeforeAlert Is Nothing Then
                            .FailureThreshold = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " URL failure threshold not set, using default of '2'.")
                        Else
                            .FailureThreshold = entity.ConsecutiveFailuresBeforeAlert
                        End If
                    Catch ex As Exception
                        .FailureThreshold = 2
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL failure threshold not set, using default of '2'.")
                    End Try


                    Try
                        If entity.ScanInterval Is Nothing Then
                            .ScanInterval = 10
                            WriteAuditEntry(Now.ToString & " " & .Name & " URL  scan interval not set, using default of 10 minutes.")
                        Else
                            .ScanInterval = entity.ScanInterval
                        End If
                    Catch ex As Exception
                        .ScanInterval = 10
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL  scan interval not set, using default of 10 minutes.")
                    End Try


                    Try
                        If entity.ResponseTime Is Nothing Then
                            .ResponseThreshold = 5
                            WriteAuditEntry(Now.ToString & " " & .Name & " URL threshold not set, using default of 5 minutes.")
                        Else
                            .ResponseThreshold = entity.ResponseTime
                        End If
                    Catch ex As Exception
                        .ResponseThreshold = 5
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL threshold not set, using default of 5 minutes.")

                    End Try

                    Try
                        If entity.SearchString Is Nothing Then
                            .SearchString = ""
                        Else
                            .SearchString = entity.SearchString
                        End If
                    Catch ex As Exception
                        .SearchString = ""
                    End Try
                    WriteAuditEntry(Now.ToString & " " & .Name & " URL search string is " & .SearchString)

                    Try
                        If entity.RetryInterval Is Nothing Then
                            .RetryInterval = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " URL retry scan interval not set, using default of 2 minutes.")
                        Else
                            .RetryInterval = entity.RetryInterval
                        End If
                    Catch ex As Exception
                        .RetryInterval = 2
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL retry scan interval not set, using default of 2 minutes.")
                    End Try

                    Try
                        If entity.OffHoursScanInterval Is Nothing Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " URL off hours scan interval not set, using default of 20 minutes.")
                            .OffHoursScanInterval = 20
                        Else
                            .OffHoursScanInterval = entity.OffHoursScanInterval
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL off hours scan interval not set, using default of 20 minutes.")
                        .OffHoursScanInterval = 20
                    End Try

                    Try
                        If entity.IPAddress Is Nothing Then
                            .URL = "Not Set"
                        Else
                            .URL = entity.IPAddress
                        End If
                    Catch ex As Exception
                        .IPAddress = "Not Set"
                        .Enabled = False
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL has no URL. ")

                    End Try

                    Try
                        If entity.Username Is Nothing Or entity.Username.Trim = "" Then
                            .UserName = ""
                            WriteAuditEntry(Now.ToString & " " & .Name & " URL does not require authentication. ")
                        Else
                            .UserName = entity.Username
                            WriteAuditEntry(Now.ToString & " " & .Name & " This URL does require authentication, and will be logged in as " & .UserName)
                        End If
                    Catch ex As Exception
                        .UserName = ""
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL does not require authentication. ")
                        '  WriteAuditEntry(Now.ToString & " " & .Name & " URL authentication exception " & ex.ToString)
                    End Try

                    Try
                        Dim mySecrets As New VSFramework.TripleDES()
                        If entity.Password Is Nothing Then
                            .Password = ""
                        Else
                            .Password = mySecrets.Decrypt(entity.Password)
                        End If
                    Catch ex As Exception
                        .Password = ""
                    End Try

                    Try
                        Dim entityStatus As VSNext.Mongo.Entities.Status
                        Try
                            entityStatus = listOfStatus.Where(Function(x) x.DeviceType.Equals(entity.DeviceType) And x.DeviceName.Equals(entity.DeviceName)).ToList()(0)
                        Catch ex As Exception
                        End Try

                        Try
                            If entityStatus.CurrentStatus Is Nothing Then
                                .Status = "Not Scanned"
                            Else
                                .Status = entityStatus.CurrentStatus
                            End If
                        Catch ex As Exception
                            .Status = "Not Scanned"
                        End Try

                        Try
                            If entityStatus.LastUpdated Is Nothing Then
                                .LastScan = Now.AddMinutes(-30)
                            Else
                                .LastScan = entityStatus.LastUpdated
                            End If
                        Catch ex As Exception
                            .LastScan = Now.AddMinutes(-30)
                        End Try

                    Catch ex As Exception
                        .Status = "Not Scanned"
                        .LastScan = Now.AddMinutes(-30)
                    End Try

                    'Try
                    '    If dr.Item("CurrentNodeID") Is Nothing Then
                    '        .InsufficentLicenses = True
                    '    Else
                    '        If dr.Item("CurrentNodeID").ToString() = "-1" Then
                    '            .InsufficentLicenses = True
                    '        Else
                    '            .InsufficentLicenses = False
                    '        End If

                    '    End If
                    'Catch ex As Exception
                    '    '7/8/2015 NS modified for VSPLUS-1959
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " URL insufficient licenses not set.")
                    '
                    'End Try

                    Try
                        If entity.AlertIfStringFound Is Nothing Then
                            .AlertStringFound = "0"
                        Else
                            .AlertStringFound = IIf(entity.AlertIfStringFound, "1", "0")
                        End If
                    Catch ex As Exception
                        .AlertStringFound = "0"
                    End Try

                    '5/2/2016 NS modified for VSPLUS-2887
                    Try

                        Dim repositoryLocation As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Location)(connectionString)
                        Dim filterDefLocation As FilterDefinition(Of VSNext.Mongo.Entities.Location) = repositoryLocation.Filter.Eq(Function(x) x.Id, entity.LocationId)
                        Dim locationAlias As String = repositoryLocation.Find(filterDefLocation).ToList()(0).LocationName.ToString()

                        .Location = locationAlias
                        WriteAuditEntry(Now.ToString & " The location for this Notes database is " & .Location)

                    Catch ex As Exception
                        .Location = "URL"
                    End Try

                    Try
                        If entity.CurrentNode Is Nothing Then
                            .InsufficentLicenses = True
                        Else
                            If entity.CurrentNode.ToString() <> getCurrentNode() Then
                                .InsufficentLicenses = True
                            Else
                                .InsufficentLicenses = False
                            End If
                        End If
                        .CurrentNode = entity.CurrentNode
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " URL Servers insufficient licenses not set.")
                    End Try

                End With

                MyURL = Nothing

            Next
        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Data Exception creating URL collection: " & exception.Message)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Random Exception creating URL collection:  " & ex.Message)
        End Try

        InsufficentLicensesTest(MyURLs)

        'Free the memory

    End Sub

    '10/6/2014 NS added for VSPLUS-1002
    Private Sub CreateCloudCollection()


        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)

        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Cloud.ToDescription())
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.Category) _
                .Include(Function(x) x.ConsecutiveFailuresBeforeAlert) _
                .Include(Function(x) x.Username) _
                .Include(Function(x) x.Password) _
                .Include(Function(x) x.IsEnabled) _
                .Include(Function(x) x.OffHoursScanInterval) _
                .Include(Function(x) x.RetryInterval) _
                .Include(Function(x) x.ResponseTime) _
                .Include(Function(x) x.SearchString) _
                .Include(Function(x) x.ScanInterval) _
                .Include(Function(x) x.IPAddress) _
                .Include(Function(x) x.CurrentNode)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()

            Dim repositoryStatus As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDefStatus As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.Cloud.ToDescription())
            Dim projectionDefStatus As ProjectionDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Project _
                .Include(Function(x) x.StatusCode) _
                .Include(Function(x) x.CurrentStatus) _
                .Include(Function(x) x.LastUpdated) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.DeviceName)

            listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList()

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in Cloud URL processing code. Exception: " & ex.Message)
            '  Exit Sub
        End Try

        '***

        'Add the URLs  to the collection
        WriteAuditEntry(Now.ToString & "  Reading configuration settings for Cloud URLs.")
        Try
            Dim i As Integer = 0
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                i += 1
                Dim MyName As String
                If entity.DeviceName Is Nothing Then
                    MyName = "CloudURL" & i.ToString
                Else
                    MyName = entity.DeviceName
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
                    MyCloud.LastScan = Now.AddMinutes(-30)
                    MyCloud.NextScan = Now
                    MyCloud.SearchString = ""
                    MyCloud.Location = "Cloud"
                    MyClouds.Add(MyCloud)
                    WriteAuditEntry(Now.ToString & " Adding new Cloud URL--" & MyCloud.Name & "-- to the collection.", LogLevel.Verbose)
                Else
                    WriteAuditEntry(Now.ToString & " Updating existing Cloud URL--" & MyCloud.Name & ".", LogLevel.Verbose)
                End If

                With MyCloud

                    WriteAuditEntry(Now.ToString & " Configuring Cloud URL: " & entity.DeviceName, LogLevel.Verbose)

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
                        If entity.Category Is Nothing Then
                            .Category = "Internet"
                            WriteAuditEntry(Now.ToString & " " & .Name & " Cloud Category not set, using default of 'Domino'.")
                        Else
                            .Category = entity.Category
                        End If
                    Catch ex As Exception
                        .Category = "Internet"
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud Category not set, using default of 'NotesMail Probe'.")
                    End Try

                    Try
                        If entity.ConsecutiveFailuresBeforeAlert Is Nothing Then
                            .FailureThreshold = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " Cloud failure threshold not set, using default of '2'.")
                        Else
                            .FailureThreshold = entity.ConsecutiveFailuresBeforeAlert
                        End If
                    Catch ex As Exception
                        .FailureThreshold = 2
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud failure threshold not set, using default of '2'.")
                    End Try


                    Try
                        If entity.ScanInterval Is Nothing Then
                            .ScanInterval = 10
                            WriteAuditEntry(Now.ToString & " " & .Name & " Cloud scan interval not set, using default of 10 minutes.")
                        Else
                            .ScanInterval = entity.ScanInterval
                        End If
                    Catch ex As Exception
                        .ScanInterval = 10
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud scan interval not set, using default of 10 minutes.")
                    End Try


                    Try
                        If entity.ResponseTime Is Nothing Then
                            .ResponseThreshold = 5
                            WriteAuditEntry(Now.ToString & " " & .Name & " Cloud threshold not set, using default of 5 minutes.")
                        Else
                            .ResponseThreshold = entity.ResponseTime
                        End If
                    Catch ex As Exception
                        .ResponseThreshold = 5
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud threshold not set, using default of 5 minutes.")

                    End Try

                    Try
                        If entity.SearchString Is Nothing Then
                            .SearchString = ""
                        Else
                            .SearchString = entity.SearchString
                        End If
                    Catch ex As Exception
                        .SearchString = ""
                    End Try
                    WriteAuditEntry(Now.ToString & " " & .Name & " Cloud search string is " & .SearchString)

                    Try
                        If entity.RetryInterval Is Nothing Then
                            .RetryInterval = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " Cloud retry scan interval not set, using default of 2 minutes.")
                        Else
                            .RetryInterval = entity.RetryInterval
                        End If
                    Catch ex As Exception
                        .RetryInterval = 2
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud retry scan interval not set, using default of 2 minutes.")
                    End Try

                    Try
                        If entity.OffHoursScanInterval Is Nothing Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " Cloud off hours scan interval not set, using default of 20 minutes.")
                            .OffHoursScanInterval = 20
                        Else
                            .OffHoursScanInterval = entity.OffHoursScanInterval
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud off hours scan interval not set, using default of 20 minutes.")
                        .OffHoursScanInterval = 20
                    End Try

                    Try
                        If entity.IPAddress Is Nothing Then
                            .CloudURL = "Not Set"
                        Else
                            .CloudURL = entity.IPAddress
                        End If
                    Catch ex As Exception
                        .IPAddress = "Not Set"
                        .Enabled = False
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud has no URL. ")

                    End Try

                    Try
                        If entity.Username Is Nothing Or entity.Username.Trim = "" Then
                            .UserName = ""
                            WriteAuditEntry(Now.ToString & " " & .Name & " Cloud does not require authentication. ")
                        Else
                            .UserName = entity.Username
                            WriteAuditEntry(Now.ToString & " " & .Name & " This Cloud URL does require authentication, and will be logged in as " & .UserName)
                        End If
                    Catch ex As Exception
                        .UserName = ""
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud URL does not require authentication. ")
                        '  WriteAuditEntry(Now.ToString & " " & .Name & " URL authentication exception " & ex.ToString)
                    End Try

                    Try
                        Dim mySecrets As New VSFramework.TripleDES()
                        If entity.Password Is Nothing Then
                            .Password = ""
                        Else
                            .Password = mySecrets.Decrypt(entity.Password)
                        End If
                    Catch ex As Exception
                        .Password = ""
                    End Try

                    If listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.DeviceName) And x.DeviceType.Equals(entity.DeviceType)).ToList().Count() > 0 Then

                        Dim entityStatus As VSNext.Mongo.Entities.Status = listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.DeviceName) And x.DeviceType.Equals(entity.DeviceType)).ToList()(0)


                        Try
                            If entityStatus.CurrentStatus Is Nothing Then
                                .Status = "Not Scanned"
                            Else
                                .Status = entityStatus.CurrentStatus
                            End If
                        Catch ex As Exception
                            .Status = "Not Scanned"
                        End Try

                        Try
                            If entityStatus.LastUpdated Is Nothing Then
                                .LastScan = Now.AddMinutes(-30)
                            Else
                                .LastScan = entityStatus.LastUpdated
                            End If
                        Catch ex As Exception
                            .LastScan = Now.AddMinutes(-30)
                        End Try

                    End If

                    Try
                        If entity.CurrentNode Is Nothing Then
                            .InsufficentLicenses = True
                        Else
                            If entity.CurrentNode.ToString() <> getCurrentNode() Then
                                .InsufficentLicenses = True
                            Else
                                .InsufficentLicenses = False
                            End If
                        End If
                        .CurrentNode = entity.CurrentNode
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Cloud Servers insufficient licenses not set.")
                    End Try


                End With

                MyURL = Nothing

            Next
        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " Data Exception creating Cloud collection: " & exception.Message)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Random Exception creating Cloud collection:  " & ex.Message)
        End Try

        InsufficentLicensesTest(MyClouds)

        'Free the memory
    End Sub

    Private Sub CreateWebSphereCollection()
        'start with fresh data
        'Connect to the data source
        Dim dsWebSphere As New Data.DataSet
        Dim mySecrets As New VSFramework.TripleDES
        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfNodes As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfCells As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)

        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.WebSphere.ToDescription())
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.NodeId) _
                .Include(Function(x) x.CellId) _
                .Include(Function(x) x.IsEnabled) _
                .Include(Function(x) x.LocationId) _
                .Include(Function(x) x.ScanInterval) _
                .Include(Function(x) x.OffHoursScanInterval) _
                .Include(Function(x) x.RetryInterval) _
                .Include(Function(x) x.Category) _
                .Include(Function(x) x.CpuThreshold) _
                .Include(Function(x) x.MemoryThreshold) _
                .Include(Function(x) x.ResponseTime) _
                .Include(Function(x) x.ConsecutiveFailuresBeforeAlert) _
                .Include(Function(x) x.ConsecutiveOverThresholdBeforeAlert) _
                .Include(Function(x) x.Description) _
                .Include(Function(x) x.CurrentNode) _
                .Include(Function(x) x.ActiveThreadCount) _
                .Include(Function(x) x.HungThreadCount) _
                .Include(Function(x) x.MemoryUsed) _
                .Include(Function(x) x.HeapCurrent) _
                .Include(Function(x) x.AverageThreadPool) _
                .Include(Function(x) x.UpTime) _
                .Include(Function(x) x.ProcessCPU)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()


            filterDef = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.WebSphereNode.ToDescription()) ' And repository.Filter.In(Function(x) x.CurrentNode, {getCurrentNode(), "-1"})
            projectionDef = repository.Project _
.Include(Function(x) x.Id) _
.Include(Function(x) x.DeviceName) _
.Include(Function(x) x.IPAddress)

            listOfNodes = repository.Find(filterDef, projectionDef).ToList()


            filterDef = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.WebSphereCell.ToDescription()) ' And repository.Filter.In(Function(x) x.CurrentNode, {getCurrentNode(), "-1"})
            projectionDef = repository.Project _
.Include(Function(x) x.Id) _
.Include(Function(x) x.DeviceName) _
.Include(Function(x) x.CellHostName) _
.Include(Function(x) x.ConnectionType) _
.Include(Function(x) x.PortNumber) _
.Include(Function(x) x.Realm) _
.Include(Function(x) x.CredentialsId)

            listOfCells = repository.Find(filterDef, projectionDef).ToList()

            Dim repositoryStatus As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDefStatus As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.WebSphere.ToDescription())
            Dim projectionDefStatus As ProjectionDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Project _
.Include(Function(x) x.StatusCode) _
.Include(Function(x) x.CurrentStatus) _
.Include(Function(x) x.LastUpdated) _
.Include(Function(x) x.DeviceType) _
.Include(Function(x) x.DeviceName)

            listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList()

            'WSC: IPAddress, CellName, ConnectionType, PortNumber, Realm, CredentialID
            'WSN: DeviceName

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
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                i += 1
                Dim MyName As String
                If entity.DeviceName Is Nothing Then
                    MyName = "WebSphere #" & i.ToString
                Else
                    MyName = entity.DeviceName
                End If
                'See if this server is already in the collection; if so, update its settings otherwise create a new one
                MyWebSphereServer = MyWebSphereServers.SearchByName(MyName)
                If MyWebSphereServer Is Nothing Then
                    Try
                        MyWebSphereServer = New MonitoredItems.WebSphere
                        MyWebSphereServer.Name = MyName
                        MyWebSphereServer.LastScan = Now.AddMinutes(-30)
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

                        Dim repositoryLocation As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Location)(connectionString)
                        Dim filterDefLocation As FilterDefinition(Of VSNext.Mongo.Entities.Location) = repositoryLocation.Filter.Eq(Function(x) x.Id, entity.LocationId)
                        Dim locationAlias As String = repositoryLocation.Find(filterDefLocation).ToList()(0).LocationName.ToString()

                        .Location = locationAlias
                        WriteAuditEntry(Now.ToString & " The location for this Notes database is " & .Location)

                    Catch ex As Exception
                        .Location = "Not Set"
                    End Try

                    Try
                        If entity.Description Is Nothing Then
                            .Description = ""
                        Else
                            .Description = entity.Description
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
                        If entity.ResponseTime Is Nothing Then
                            .ResponseThreshold = 100
                        Else
                            .ResponseThreshold = entity.ResponseTime
                        End If
                    Catch ex As Exception
                        .ResponseThreshold = 100
                    End Try


                    Try
                        If entity.ScanInterval Is Nothing Then
                            .ScanInterval = 10
                        Else
                            .ScanInterval = entity.ScanInterval
                        End If
                    Catch ex As Exception
                        .ScanInterval = 10
                    End Try

                    Try
                        If entity.OffHoursScanInterval Is Nothing Then
                            .OffHoursScanInterval = 30
                        Else
                            .OffHoursScanInterval = entity.OffHoursScanInterval
                        End If
                    Catch ex As Exception
                        .OffHoursScanInterval = 30
                    End Try
                    Try
                        '   WriteAuditEntry(Now.ToString & "Adding Category")
                        If entity.Category Is Nothing Then
                            .Category = "Not Categorized"
                        Else
                            .Category = entity.Category
                        End If
                    Catch ex As Exception
                        .Category = "Not Categorized"
                    End Try



                    Try
                        If entity.RetryInterval Is Nothing Then
                            .RetryInterval = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " Notes Database retry scan interval not set, using default of 10 minutes.")
                        Else
                            .RetryInterval = entity.RetryInterval
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Notes Database retry scan interval not set, using default of 10 minutes.")
                        .RetryInterval = 2
                    End Try

                    If listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.DeviceName) And x.DeviceType.Equals(entity.DeviceType)).ToList().Count() > 0 Then

                        Dim entityStatus As VSNext.Mongo.Entities.Status = listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.DeviceName) And x.DeviceType.Equals(entity.DeviceType)).ToList()(0)


                        Try
                            If entityStatus.CurrentStatus Is Nothing Then
                                .Status = "Not Scanned"
                            Else
                                .Status = entityStatus.CurrentStatus
                            End If
                        Catch ex As Exception
                            .Status = "Not Scanned"
                        End Try

                        Try
                            If entityStatus.LastUpdated Is Nothing Then
                                .LastScan = Now.AddMinutes(-30)
                            Else
                                .LastScan = entityStatus.LastUpdated
                            End If
                        Catch ex As Exception
                            .LastScan = Now.AddMinutes(-30)
                        End Try

                    End If

                    'Try
                    '    If dr.Item("CurrentNodeID") Is Nothing Then
                    '        .InsufficentLicenses = True
                    '    Else
                    '        If dr.Item("CurrentNodeID").ToString() = "-1" Then
                    '            .InsufficentLicenses = True
                    '        Else
                    '            .InsufficentLicenses = False
                    '        End If

                    '    End If
                    'Catch ex As Exception
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " WebSphere insufficent licenses not set.")

                    'End Try

                    Try
                        Dim node As VSNext.Mongo.Entities.Server

                        Try
                            .NodeID = entity.NodeId
                        Catch ex As Exception

                        End Try

                        Try
                            WriteAuditEntry(Now.ToString & " NodeList : " & listOfNodes.ToList().Count(), LogLevel.Normal)
                            WriteAuditEntry(Now.ToString & " NodeList after filter: " & listOfNodes.Where(Function(x) x.Id.Equals(entity.NodeId)).ToList().Count(), LogLevel.Normal)
                            WriteAuditEntry(Now.ToString & " NodeName : " & listOfNodes.Where(Function(x) x.Id.Equals(entity.NodeId)).ToList()(0).NodeName, LogLevel.Normal)
                            node = listOfNodes.Where(Function(x) x.Id.Equals(entity.NodeId)).ToList()(0)
                        Catch ex As Exception

                        End Try

                        Try
                            .NodeName = node.DeviceName
                        Catch ex As Exception
                            .NodeName = ""
                        End Try

                        Try
                            .HostName = node.IPAddress
                        Catch ex As Exception
                            .HostName = ""
                        End Try

                    Catch ex As Exception

                    End Try

                    Try
                        Dim cell As VSNext.Mongo.Entities.Server

                        Try
                            .CellID = entity.CellId
                        Catch ex As Exception

                        End Try

                        Try
                            cell = listOfCells.Where(Function(x) x.Id.Equals(entity.CellId)).ToList()(0)
                        Catch ex As Exception

                        End Try

                        Try
                            .CellName = cell.DeviceName
                        Catch ex As Exception
                            .CellName = ""
                        End Try

                        Try
                            .CellHostName = cell.CellHostName
                        Catch ex As Exception
                            .CellHostName = ""
                        End Try

                        Try
                            .ConnectionType = cell.ConnectionType
                        Catch ex As Exception
                            .ConnectionType = ""
                        End Try

                        Try
                            .Port = cell.PortNumber
                        Catch ex As Exception
                            .Port = ""
                        End Try

                        Try
                            .Realm = cell.Realm
                        Catch ex As Exception
                            .Realm = ""
                        End Try

                        Try
                            WriteAuditEntry(Now.ToString & " Getting server credentials.", LogLevel.Verbose)
                            If cell.CredentialsId Is Nothing Then
                                .UserName = ""
                                .Password = ""
                            Else
                                'Run a query here, then parse the results

                                Dim repositoryCredentials As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Credentials)(connectionString)
                                Dim filterDefCredentials As FilterDefinition(Of VSNext.Mongo.Entities.Credentials) = repositoryCredentials.Filter.Eq(Function(x) x.Id, cell.CredentialsId)
                                Dim entityCredentials As VSNext.Mongo.Entities.Credentials = repositoryCredentials.Find(filterDefCredentials).ToList()(0)

                                .UserName = entityCredentials.UserId
                                WriteAuditEntry(Now.ToString & " Username is " & .UserName, LogLevel.Verbose)

                                Dim strEncryptedPassword As String
                                Dim Password As String
                                Dim myPass As Byte()

                                strEncryptedPassword = entityCredentials.Password

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
                                    WriteAuditEntry(Now.ToString & " Error decrypting the password.  " & ex.ToString)
                                End Try
                                .Password = Password
                            End If
                        Catch ex As Exception

                        End Try

                    Catch ex As Exception

                    End Try



                    Try
                        If entity.ConsecutiveFailuresBeforeAlert Is Nothing Then
                            .FailureThreshold = 2
                        Else
                            .FailureThreshold = entity.ConsecutiveFailuresBeforeAlert
                        End If
                    Catch ex As Exception
                        .FailureThreshold = 2
                    End Try

                    Try
                        If entity.ConsecutiveOverThresholdBeforeAlert Is Nothing Then
                            .ServerDaysAlert = 14
                        Else
                            .ServerDaysAlert = entity.ConsecutiveOverThresholdBeforeAlert
                        End If
                    Catch ex As Exception
                        .ServerDaysAlert = 14
                    End Try

                    Try
                        .ServerName = .Name.Substring(0, .Name.IndexOf("[") - 1).Trim()
                    Catch ex As Exception

                    End Try


                    Try
                        If entity.CurrentNode Is Nothing Then
                            .InsufficentLicenses = True
                        Else
                            If entity.CurrentNode.ToString() <> getCurrentNode() Then
                                .InsufficentLicenses = True
                            Else
                                .InsufficentLicenses = False
                            End If
                        End If
                        .CurrentNode = entity.CurrentNode
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Connections Servers insufficient licenses not set.")
                    End Try

                    Try
                        If entity.HeapCurrent Is Nothing Then
                            .CurrentHeapThreshold = 500
                        Else
                            .CurrentHeapThreshold = entity.HeapCurrent
                        End If
                    Catch ex As Exception
                        .CurrentHeapThreshold = 500
                    End Try

                    Try
                        If entity.AverageThreadPool Is Nothing Then
                            .AverageThreadPoolThreshold = 30
                        Else
                            .AverageThreadPoolThreshold = entity.AverageThreadPool
                        End If
                    Catch ex As Exception
                        .AverageThreadPoolThreshold = 30
                    End Try

                    Try
                        If entity.UpTime Is Nothing Then
                            .UpTimeThreshold = 45
                        Else
                            .UpTimeThreshold = entity.UpTime
                        End If
                    Catch ex As Exception
                        .UpTimeThreshold = 45
                    End Try

                    Try
                        If entity.ProcessCPU Is Nothing Then
                            .CPU_Threshold = 75
                        Else
                            .CPU_Threshold = entity.ProcessCPU
                        End If
                    Catch ex As Exception
                        .CPU_Threshold = 75
                    End Try

                    Try
                        If entity.ActiveThreadCount Is Nothing Then
                            .ActiveThreadCountThreshold = 3
                        Else
                            .ActiveThreadCountThreshold = entity.ActiveThreadCount
                        End If
                    Catch ex As Exception
                        .ActiveThreadCountThreshold = 3
                    End Try

                    Try
                        If entity.HungThreadCount Is Nothing Then
                            .HungThreadCountThreshold = 10
                        Else
                            .HungThreadCountThreshold = entity.HungThreadCount
                        End If
                    Catch ex As Exception
                        .HungThreadCountThreshold = 10
                    End Try

                    Try
                        If entity.MemoryUsed Is Nothing Then
                            .Memory_Threshold = 2048
                        Else
                            .Memory_Threshold = entity.MemoryUsed
                        End If
                    Catch ex As Exception
                        .Memory_Threshold = 2048
                    End Try


                End With
                MyWebSphereServer = Nothing
            Next

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
        Dim mySecrets As New VSFramework.TripleDES
        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)

        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.IBMConnections.ToDescription())
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.IsEnabled) _
                .Include(Function(x) x.LocationId) _
                .Include(Function(x) x.CredentialsId) _
                .Include(Function(x) x.CurrentNode) _
                .Include(Function(x) x.ScanInterval) _
                .Include(Function(x) x.RetryInterval) _
                .Include(Function(x) x.OffHoursScanInterval) _
                .Include(Function(x) x.Category) _
                .Include(Function(x) x.ResponseTime) _
                .Include(Function(x) x.ConsecutiveFailuresBeforeAlert) _
                .Include(Function(x) x.Description) _
                .Include(Function(x) x.IPAddress) _
                .Include(Function(x) x.DatabaseSettingsHostName) _
                .Include(Function(x) x.DatabaseSettingsCredentialsId) _
                .Include(Function(x) x.DatabaseSettingsPort) _
                .Include(Function(x) x.SimulationTests) _
                .Include(Function(x) x.ConnectionsCommunityUuid) _
                .Include(Function(x) x.ConnectionsTestUrl)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()

            Dim repositoryStatus As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterDefStatus As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.IBMConnections.ToDescription())
            Dim projectionDefStatus As ProjectionDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Project _
                .Include(Function(x) x.StatusCode) _
                .Include(Function(x) x.CurrentStatus) _
                .Include(Function(x) x.LastUpdated) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.DeviceName)

            listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList()

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create dataset in CreateIBMConnectCollection processing code. Exception: " & ex.Message)
            Exit Sub
        End Try



        '***
        'but first delete any that are in the collection but not in the database anymore
        'Delete propogation
        Dim MyServerNames As String = ""

        If MyIBMConnectServers.Count > 0 Then
            WriteAuditEntry(Now.ToString & " Checking to see if any IBM Connect Servers should be deleted. ")
            'Get all the names of all the servers in the data table
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                MyServerNames += entity.DeviceName & "  "
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
            If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & "  Reading configuration settings for " & listOfServers.Count & " IBM Connect Servers.")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error with IBM Connect table " & ex.Message, LogLevel.Normal)
        End Try
        Try
            Dim myString As String = ""
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                i += 1
                Dim MyName As String
                If entity.DeviceName Is Nothing Then
                    MyName = "IBM Connect #" & i.ToString
                Else
                    MyName = entity.DeviceName
                End If
                'See if this server is already in the collection; if so, update its settings otherwise create a new one
                MyIBMConnectServer = MyIBMConnectServers.SearchByName(MyName)
                If MyIBMConnectServer Is Nothing Then
                    Try
                        MyIBMConnectServer = New MonitoredItems.IBMConnect
                        MyIBMConnectServer.Name = MyName
                        MyIBMConnectServer.LastScan = Now.AddMinutes(-30)
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

                        Dim repositoryLocation As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Location)(connectionString)
                        Dim filterDefLocation As FilterDefinition(Of VSNext.Mongo.Entities.Location) = repositoryLocation.Filter.Eq(Function(x) x.Id, entity.LocationId)
                        Dim locationAlias As String = repositoryLocation.Find(filterDefLocation).ToList()(0).LocationName.ToString()

                        .Location = locationAlias

                    Catch ex As Exception
                        .Location = "Not Set"
                    End Try

                    Try
                        If entity.Description Is Nothing Then
                            .Description = ""
                        Else
                            .Description = entity.Description
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

                    If .Enabled = True And .Status = "Disabled" Then
                        .Status = "Not Scanned"
                    End If

                    Try
                        If entity.ResponseTime Is Nothing Then
                            .ResponseThreshold = 100
                        Else
                            .ResponseThreshold = entity.ResponseTime
                        End If
                    Catch ex As Exception
                        .ResponseThreshold = 100
                    End Try


                    Try
                        If entity.ScanInterval Is Nothing Then
                            .ScanInterval = 10
                        Else
                            .ScanInterval = entity.ScanInterval
                        End If
                    Catch ex As Exception
                        .ScanInterval = 10
                    End Try

                    Try
                        If entity.OffHoursScanInterval Is Nothing Then
                            .OffHoursScanInterval = 30
                        Else
                            .OffHoursScanInterval = entity.OffHoursScanInterval
                        End If
                    Catch ex As Exception
                        .OffHoursScanInterval = 30
                    End Try
                    Try
                        '   WriteAuditEntry(Now.ToString & "Adding Category")
                        If entity.Category Is Nothing Then
                            .Category = "Not Categorized"
                        Else
                            .Category = entity.Category
                        End If
                    Catch ex As Exception
                        .Category = "Not Categorized"
                    End Try



                    Try
                        If entity.RetryInterval Is Nothing Then
                            .RetryInterval = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " IBM Connect retry scan interval not set, using default of 10 minutes.")
                        Else
                            .RetryInterval = entity.RetryInterval
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " IBM Connect retry scan interval not set, using default of 10 minutes.")
                        .RetryInterval = 2
                    End Try

                    If listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.DeviceName) And x.DeviceType.Equals(entity.DeviceType)).ToList().Count() > 0 Then

                        Dim entityStatus As VSNext.Mongo.Entities.Status = listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.DeviceName) And x.DeviceType.Equals(entity.DeviceType)).ToList()(0)


                        Try
                            If entityStatus.CurrentStatus Is Nothing Then
                                .Status = "Not Scanned"
                            Else
                                .Status = entityStatus.CurrentStatus
                            End If
                        Catch ex As Exception
                            .Status = "Not Scanned"
                        End Try

                        Try
                            If entityStatus.LastUpdated Is Nothing Then
                                .LastScan = Now.AddMinutes(-30)
                            Else
                                .LastScan = entityStatus.LastUpdated
                            End If
                        Catch ex As Exception
                            .LastScan = Now.AddMinutes(-30)
                        End Try

                    End If

                    'Try
                    '    If dr.Item("CurrentNodeID") Is Nothing Then
                    '        .InsufficentLicenses = True
                    '    Else
                    '        If dr.Item("CurrentNodeID").ToString() = "-1" Then
                    '            .InsufficentLicenses = True
                    '        Else
                    '            .InsufficentLicenses = False
                    '        End If

                    '    End If
                    'Catch ex As Exception
                    '    WriteAuditEntry(Now.ToString & " " & .Name & " IBM Connect insufficent licenses not set.")

                    'End Try

                    If (False) Then


                        'Try
                        '    If dr.Item("NodeID") Is Nothing Then
                        '        .NodeID = -1
                        '    Else
                        '        .NodeID = dr.Item("NodeID")
                        '    End If
                        'Catch ex As Exception
                        '    .NodeID = -1
                        'End Try

                        'Try
                        '    If dr.Item("CellID") Is Nothing Then
                        '        .CellID = -1
                        '    Else
                        '        .CellID = dr.Item("CellID")
                        '    End If
                        'Catch ex As Exception
                        '    .CellID = -1
                        'End Try

                        'Try
                        '    If dr.Item("Hostname") Is Nothing Then
                        '        .HostName = ""
                        '    Else
                        '        .HostName = dr.Item("Hostname")
                        '    End If
                        'Catch ex As Exception
                        '    .HostName = ""
                        'End Try

                        'Try
                        '    If dr.Item("NodeName") Is Nothing Then
                        '        .NodeName = ""
                        '    Else
                        '        .NodeName = dr.Item("NodeName")
                        '    End If
                        'Catch ex As Exception
                        '    .NodeName = ""
                        'End Try

                        'Try
                        '    If dr.Item("CellName") Is Nothing Then
                        '        .CellName = ""
                        '    Else
                        '        .CellName = dr.Item("CellName")
                        '    End If
                        'Catch ex As Exception
                        '    .CellName = ""
                        'End Try

                        'Try
                        '    If dr.Item("CellHostName") Is Nothing Then
                        '        .CellHostName = "RMI"
                        '    Else
                        '        .CellHostName = dr.Item("CellHostName")
                        '    End If
                        'Catch ex As Exception
                        '    .CellHostName = "RMI"
                        'End Try

                        'Try
                        '    If dr.Item("ConnectionType") Is Nothing Then
                        '        .ConnectionType = "RMI"
                        '    Else
                        '        .ConnectionType = dr.Item("ConnectionType")
                        '    End If
                        'Catch ex As Exception
                        '    .ConnectionType = "RMI"
                        'End Try

                        'Try
                        '    If dr.Item("PortNo") Is Nothing Then
                        '        .Port = 1099
                        '    Else
                        '        .Port = dr.Item("PortNo")
                        '    End If
                        'Catch ex As Exception
                        '    .Port = 1099
                        'End Try

                        'Try
                        '    If dr.Item("GlobalSecurity") Is Nothing Then
                        '        .GlobalSecurity = False
                        '    Else
                        '        .GlobalSecurity = dr.Item("GlobalSecurity")
                        '    End If
                        'Catch ex As Exception
                        '    .GlobalSecurity = False
                        'End Try

                        'Try
                        '    If dr.Item("SametimeId") Is Nothing Then
                        '        .SametimeID = -1
                        '    Else
                        '        .SametimeID = dr.Item("SametimeId")
                        '    End If
                        'Catch ex As Exception
                        '    .SametimeID = -1
                        'End Try

                        'Try
                        '    If dr.Item("Realm") Is Nothing Then
                        '        .Realm = ""
                        '    Else
                        '        .Realm = dr.Item("Realm")
                        '    End If
                        'Catch ex As Exception
                        '    .Realm = ""
                        'End Try

                        'Try
                        '    If dr.Item("ServerType") Is Nothing Then
                        '        .ServerType = "IBM Connect"
                        '    Else
                        '        .ServerType = dr.Item("ServerType")
                        '    End If
                        'Catch ex As Exception
                        '    .ServerType = "WebSphere"
                        'End Try
                    End If


                    Try
                        WriteAuditEntry(Now.ToString & " Getting server credentials.", LogLevel.Verbose)
                        If entity.CredentialsId Is Nothing Then
                            .UserName = ""
                            .Password = ""
                        Else
                            'Run a query here, then parse the results

                            Dim repositoryCredentials As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Credentials)(connectionString)
                            Dim filterDefCredentials As FilterDefinition(Of VSNext.Mongo.Entities.Credentials) = repositoryCredentials.Filter.Eq(Function(x) x.Id, entity.CredentialsId)
                            Dim entityCredentials As VSNext.Mongo.Entities.Credentials = repositoryCredentials.Find(filterDefCredentials).ToList()(0)

                            .UserName = entityCredentials.UserId
                            WriteAuditEntry(Now.ToString & " HTTP Username is " & .UserName, LogLevel.Verbose)

                            Dim strEncryptedPassword As String
                            Dim Password As String
                            Dim myPass As Byte()

                            strEncryptedPassword = entityCredentials.Password

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

                    End Try

                    Try
                        If entity.ConsecutiveFailuresBeforeAlert Is Nothing Then
                            .FailureThreshold = 3
                        Else
                            .FailureThreshold = entity.ConsecutiveFailuresBeforeAlert
                        End If
                    Catch ex As Exception
                        .FailureThreshold = 3
                    End Try

                    Try
                        If entity.ConsecutiveOverThresholdBeforeAlert Is Nothing Then
                            .ServerDaysAlert = 14
                        Else
                            .ServerDaysAlert = entity.ConsecutiveOverThresholdBeforeAlert
                        End If
                    Catch ex As Exception
                        .ServerDaysAlert = 14
                    End Try

                    Try
                        .ServerName = .Name
                    Catch ex As Exception

                    End Try

                    Try
                        If entity.IPAddress Is Nothing Then
                            .IPAddress = ""
                        Else
                            .IPAddress = entity.IPAddress
                        End If
                    Catch ex As Exception
                        .IPAddress = ""
                    End Try

                    Try
                        If entity.DatabaseSettingsHostName Is Nothing Then
                            .DBHostName = ""
                        Else
                            .DBHostName = entity.DatabaseSettingsHostName
                        End If
                    Catch ex As Exception
                        .DBHostName = ""
                    End Try

                    Try
                        If entity.DatabaseSettingsPort Is Nothing Then
                            .DBPort = ""
                        Else
                            .DBPort = entity.DatabaseSettingsPort
                        End If
                    Catch ex As Exception
                        .DBPort = ""
                    End Try

                    Try
                        If entity.ConnectionsCommunityUuid Is Nothing Then
                            .CommunityUUID = ""
                        Else
                            .CommunityUUID = entity.ConnectionsCommunityUuid
                        End If
                    Catch ex As Exception
                        .CommunityUUID = ""
                    End Try

                    Try
                        If entity.ConnectionsTestUrl Is Nothing Then
                            .TestUrl = ""
                        Else
                            .TestUrl = entity.ConnectionsTestUrl
                        End If
                    Catch ex As Exception
                        .TestUrl = ""
                    End Try

                    Try
                        WriteAuditEntry(Now.ToString & " Getting server credentials.", LogLevel.Verbose)
                        If entity.DatabaseSettingsCredentialsId Is Nothing Then
                            .DBUserName = ""
                            .DBPassword = ""
                        Else
                            'Run a query here, then parse the results

                            Dim repositoryCredentials As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Credentials)(connectionString)
                            Dim filterDefCredentials As FilterDefinition(Of VSNext.Mongo.Entities.Credentials) = repositoryCredentials.Filter.Eq(Function(x) x.Id, entity.DatabaseSettingsCredentialsId)
                            Dim entityCredentials As VSNext.Mongo.Entities.Credentials = repositoryCredentials.Find(filterDefCredentials).ToList()(0)

                            .DBUserName = entityCredentials.UserId
                            WriteAuditEntry(Now.ToString & " HTTP Username is " & .DBUserName, LogLevel.Verbose)

                            Dim strEncryptedPassword As String
                            Dim Password As String
                            Dim myPass As Byte()

                            strEncryptedPassword = entityCredentials.Password

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
                            .DBPassword = Password
                        End If
                    Catch ex As Exception

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

                        For Each test As VSNext.Mongo.Entities.NameValuePair In entity.SimulationTests
                            Select Case test.Name

                                Case "Create Activity Threshold"
                                    .CreateActivityThreshold = test.Value
                                    .TestCreateActivity = True

                                Case "Create Blog Threshold"
                                    .CreateBlogThreshold = test.Value
                                    .TestCreateBlog = True
                                Case "Create Bookmark Threshold"
                                    .CreateBookmarkThreshold = test.Value
                                    .TestCreateBookmarks = True

                                Case "Create Community Threshold"
                                    .CreateCommunitiesThreshold = test.Value
                                    .TestCreateCommunities = True

                                Case "Create File Threshold"
                                    .CreateFilesThreshold = test.Value
                                    .TestCreateFiles = True

                                Case "Create Wiki Threshold"
                                    .CreateWikisThreshold = test.Value
                                    .TestCreateWikis = True

                                Case "Search Profile Threshold"
                                    .SearchProfilesThreshold = test.Value
                                    .SearchProfilesThreshold = True

                            End Select
                        Next

                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " IBM Connect Servers exception for tests " & ex.Message)
                    End Try

                    Try
                        If entity.CurrentNode Is Nothing Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " Connections Server marked to insufficent licenses due to it not being set.", LogLevel.Verbose)
                            .InsufficentLicenses = True
                        Else
                            If entity.CurrentNode.ToString() <> getCurrentNode() Then
                                WriteAuditEntry(Now.ToString & " " & .Name & " Connections Server marked to insufficent licenses due to it being -1.", LogLevel.Verbose)
                                .InsufficentLicenses = True
                            Else
                                .InsufficentLicenses = False
                            End If
                        End If
                        .CurrentNode = entity.CurrentNode
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " Connections Servers insufficient licenses not set.")
                    End Try

                End With


                MyIBMConnectServer = Nothing
            Next

        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " IBM Connect Servers data exception " & exception.Message)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " IBM Connect Servers general exception " & ex.ToString)
        End Try

        InsufficentLicensesTest(MyIBMConnectServers)

    End Sub


    Private Sub CreateIBMFileNetCollection()
        'start with fresh data
        'Connect to the data source
        Dim mySecrets As New VSFramework.TripleDES
        Dim listOfServers As New List(Of VSNext.Mongo.Entities.Server)
        Dim listOfStatus As New List(Of VSNext.Mongo.Entities.Status)

        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Server) = repository.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.IBMFileNet.ToDescription())
            Dim projectionDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repository.Project _
                .Include(Function(x) x.Id) _
                .Include(Function(x) x.DeviceName) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.IsEnabled) _
                .Include(Function(x) x.LocationId) _
                .Include(Function(x) x.CredentialsId) _
                .Include(Function(x) x.CurrentNode) _
                .Include(Function(x) x.ScanInterval) _
                .Include(Function(x) x.RetryInterval) _
                .Include(Function(x) x.OffHoursScanInterval) _
                .Include(Function(x) x.Category) _
                .Include(Function(x) x.ResponseTime) _
                .Include(Function(x) x.Description) _
                .Include(Function(x) x.IPAddress)

            listOfServers = repository.Find(filterDef, projectionDef).ToList()
            WriteAuditEntry(Now.ToString & " Created ListOfServers dataset in CreateIBMFileNetCollection", LogLevel.Verbose)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create ListOfServers dataset in CreateIBMFileNetCollection processing code. Exception: " & ex.Message)
        End Try

        Try
                Dim repositoryStatus As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim filterDefStatus As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Filter.Eq(Function(x) x.DeviceType, VSNext.Mongo.Entities.Enums.ServerType.IBMFileNet.ToDescription())
                Dim projectionDefStatus As ProjectionDefinition(Of VSNext.Mongo.Entities.Status) = repositoryStatus.Project _
                .Include(Function(x) x.StatusCode) _
                .Include(Function(x) x.CurrentStatus) _
                .Include(Function(x) x.LastUpdated) _
                .Include(Function(x) x.DeviceType) _
                .Include(Function(x) x.DeviceName)

                listOfStatus = repositoryStatus.Find(filterDefStatus, projectionDefStatus).ToList()
            WriteAuditEntry(Now.ToString & " Created ListOfStatus dataset in CreateIBMFileNetCollection", LogLevel.Verbose)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Failed to create ListOfStatus dataset in CreateIBMFileNetCollection processing code. Exception: " & ex.Message)
            ' Exit Sub
        End Try

        Try
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                WriteAuditEntry(Now.ToString & " Server: " & entity.DeviceName, LogLevel.Verbose)
            Next
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Exception loping through server list code. Exception: " & ex.Message)
            ' Exit Sub
        End Try

        '***
        'but first delete any that are in the collection but not in the database anymore
        'Delete propogation

        Try
            Dim MyServerNames As String = ""

            If myIBMFileNetServers.Count > 0 Then
                WriteAuditEntry(Now.ToString & " Checking to see if any IBM FileNet Servers should be deleted. ")
                'Get all the names of all the servers in the data table
                For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                    MyServerNames += entity.DeviceName & "  "
                    WriteAuditEntry(Now.ToString & " Checking on " & entity.DeviceName,  LogLevel.Verbose)
                Next
            End If

            Dim server As MonitoredItems.IBMFileNet
            Dim myIndex As Integer

            If myIBMFileNetServers.Count > 0 Then
                For myIndex = myIBMFileNetServers.Count - 1 To 0 Step -1
                    server = myIBMFileNetServers.Item(myIndex)
                    Try
                        WriteAuditEntry(Now.ToString & " Checking to see if IBM Connect Server " & server.Name & " should be deleted...")
                        If InStr(MyServerNames, server.Name) > 0 Then
                            'the server has not been deleted
                            WriteAuditEntry(Now.ToString & " " & server.Name & " is not marked for deletion. ")
                        Else
                            'the server has been deleted, so delete from the collection
                            Try
                                myIBMFileNetServers.Delete(server.Name)
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
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error with IBM FileNet delete propogation: " & ex.Message, LogLevel.Normal)
        End Try


        Dim i As Integer = 0
        'Add the FileNet servers to the collection
        Try
            WriteAuditEntry(Now.ToString & "  Reading configuration settings for " & listOfServers.Count & " IBM FileNet Servers.")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error with IBM FileNet table " & ex.Message, LogLevel.Normal)
        End Try

        Try
            Dim myString As String = ""
            For Each entity As VSNext.Mongo.Entities.Server In listOfServers
                i += 1
                Dim MyName As String
                If entity.DeviceName Is Nothing Then
                    MyName = "IBM FileNet #" & i.ToString
                Else
                    MyName = entity.DeviceName
                End If
                WriteAuditEntry(Now.ToString & "  Reading configuration settings for " & MyName)
                'See if this server is already in the collection; if so, update its settings otherwise create a new one
                Try
                    MyIBMFileNetServer = myIBMFileNetServers.SearchByName(MyName)
                Catch ex As Exception
                    MyIBMFileNetServer = Nothing
                End Try

                If MyIBMFileNetServer Is Nothing Then
                    Try
                        WriteAuditEntry(Now.ToString & " Found a new IBM FileNet server to monitor:  " & MyName, LogLevel.Normal)
                        MyIBMFileNetServer = New MonitoredItems.IBMFileNet
                        MyIBMFileNetServer.Name = MyName
                        MyIBMFileNetServer.LastScan = Now.AddMinutes(-30)
                        MyIBMFileNetServer.NextScan = Now
                        MyIBMFileNetServer.AlertCondition = False
                        MyIBMFileNetServer.Status = "Not Scanned"
                        MyIBMFileNetServer.IncrementUpCount()
                        MyIBMFileNetServer.ServerType = MyIBMFileNetServer.DeviceType
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error adding new empty stats collection to " & MyIBMFileNetServer.Name)
                    End Try
                    MyIBMFileNetServers.Add(MyIBMFileNetServer)
                    WriteAuditEntry(Now.ToString & " Adding new IBM FileNet server -- " & MyIBMFileNetServer.Name & " -- to the collection.")
                Else
                    WriteAuditEntry(Now.ToString & " Updating settings for existing IBM FileNet server-- " & MyIBMFileNetServer.Name & ".")
                End If

                With MyIBMFileNetServer
                    If MyLogLevel = LogLevel.Verbose Then WriteAuditEntry(Now.ToString & " Configuring settings for IBM FileNet server-- " & MyIBMFileNetServer.Name & ".", LogLevel.Verbose)

                    'Standard attributes
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

                        Dim repositoryLocation As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Location)(connectionString)
                        Dim filterDefLocation As FilterDefinition(Of VSNext.Mongo.Entities.Location) = repositoryLocation.Filter.Eq(Function(x) x.Id, entity.LocationId)
                        Dim locationAlias As String = repositoryLocation.Find(filterDefLocation).ToList()(0).LocationName.ToString()

                        .Location = locationAlias

                    Catch ex As Exception
                        .Location = "Not Set"
                    End Try

                    Try
                        If entity.Description Is Nothing Then
                            .Description = ""
                        Else
                            .Description = entity.Description
                        End If
                    Catch ex As Exception
                        .Description = ""
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

                    Try
                        If .Enabled = False Then
                            .Status = "Disabled"
                        End If

                        If .Enabled = True And .Status = "Disabled" Then
                            .Status = "Not Scanned"
                        End If
                    Catch ex As Exception

                    End Try


                    Try
                        If entity.ResponseTime Is Nothing Then
                            .ResponseThreshold = 100
                        Else
                            .ResponseThreshold = entity.ResponseTime
                        End If
                    Catch ex As Exception
                        .ResponseThreshold = 100
                    End Try


                    Try
                        If entity.ScanInterval Is Nothing Then
                            .ScanInterval = 10
                        Else
                            .ScanInterval = entity.ScanInterval
                        End If
                    Catch ex As Exception
                        .ScanInterval = 10
                    End Try

                    Try
                        If entity.OffHoursScanInterval Is Nothing Then
                            .OffHoursScanInterval = 30
                        Else
                            .OffHoursScanInterval = entity.OffHoursScanInterval
                        End If
                    Catch ex As Exception
                        .OffHoursScanInterval = 30
                    End Try
                    Try
                        '   WriteAuditEntry(Now.ToString & "Adding Category")
                        If entity.Category Is Nothing Then
                            .Category = "Not Categorized"
                        Else
                            .Category = entity.Category
                        End If
                    Catch ex As Exception
                        .Category = "Not Categorized"
                    End Try



                    Try
                        If entity.RetryInterval Is Nothing Then
                            .RetryInterval = 2
                            WriteAuditEntry(Now.ToString & " " & .Name & " IBM FileNet retry scan interval not set, using default of 10 minutes.")
                        Else
                            .RetryInterval = entity.RetryInterval
                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " IBM FileNet retry scan interval not set, using default of 10 minutes.")
                        .RetryInterval = 2
                    End Try

                    Try
                        If listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.DeviceName) And x.DeviceType.Equals(entity.DeviceType)).ToList().Count() > 0 Then

                            Dim entityStatus As VSNext.Mongo.Entities.Status = listOfStatus.Where(Function(x) x.DeviceName.Equals(entity.DeviceName) And x.DeviceType.Equals(entity.DeviceType)).ToList()(0)

                            Try
                                If entityStatus.CurrentStatus Is Nothing Then
                                    .Status = "Not Scanned"
                                Else
                                    .Status = entityStatus.CurrentStatus
                                End If
                            Catch ex As Exception
                                .Status = "Not Scanned"
                            End Try

                            Try
                                If entityStatus.LastUpdated Is Nothing Then
                                    .LastScan = Now.AddMinutes(-30)
                                Else
                                    .LastScan = entityStatus.LastUpdated
                                End If
                            Catch ex As Exception
                                .LastScan = Now.AddMinutes(-30)
                            End Try

                        End If
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Exception with IBM FileNet scan status:  " & ex.ToString, LogLevel.Verbose)

                    End Try




                    Try
                        WriteAuditEntry(Now.ToString & " Getting server credentials.", LogLevel.Verbose)
                        If entity.CredentialsId Is Nothing Then
                            .UserName = ""
                            .Password = ""
                        Else
                            'Run a query here, then parse the results

                            Dim repositoryCredentials As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Credentials)(connectionString)
                            Dim filterDefCredentials As FilterDefinition(Of VSNext.Mongo.Entities.Credentials) = repositoryCredentials.Filter.Eq(Function(x) x.Id, entity.CredentialsId)
                            Dim entityCredentials As VSNext.Mongo.Entities.Credentials = repositoryCredentials.Find(filterDefCredentials).ToList()(0)

                            .UserName = entityCredentials.UserId
                            WriteAuditEntry(Now.ToString & " Username is " & .UserName, LogLevel.Verbose)

                            Dim strEncryptedPassword As String
                            Dim Password As String
                            Dim myPass As Byte()

                            strEncryptedPassword = entityCredentials.Password

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
                                WriteAuditEntry(Now.ToString & " Error decrypting the FileNet password.  " & ex.ToString)
                            End Try
                            .Password = Password
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If entity.ConsecutiveFailuresBeforeAlert Is Nothing Then
                            .FailureThreshold = 3
                        Else
                            .FailureThreshold = entity.ConsecutiveFailuresBeforeAlert
                        End If
                    Catch ex As Exception
                        .FailureThreshold = 3
                    End Try

                    Try
                        If entity.ConsecutiveOverThresholdBeforeAlert Is Nothing Then
                            .ServerDaysAlert = 14
                        Else
                            .ServerDaysAlert = entity.ConsecutiveOverThresholdBeforeAlert
                        End If
                    Catch ex As Exception
                        .ServerDaysAlert = 14
                    End Try

                    Try
                        .ServerName = .Name
                    Catch ex As Exception

                    End Try

                    Try
                        If entity.IPAddress Is Nothing Then
                            .IPAddress = ""
                        Else
                            .IPAddress = entity.IPAddress
                        End If
                    Catch ex As Exception
                        .IPAddress = ""
                    End Try



                    Try
                        If entity.CurrentNode Is Nothing Then
                            WriteAuditEntry(Now.ToString & " " & .Name & " FileNet Server marked to insufficent licenses due to it not being set.", LogLevel.Verbose)
                            .InsufficentLicenses = True
                        Else
                            If entity.CurrentNode.ToString() <> getCurrentNode() Then
                                WriteAuditEntry(Now.ToString & " " & .Name & " FileNet Server marked to insufficent licenses due to it being -1.", LogLevel.Verbose)
                                .InsufficentLicenses = True
                            Else
                                .InsufficentLicenses = False
                            End If
                        End If
                        .CurrentNode = entity.CurrentNode
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " " & .Name & " FileNet Servers insufficient licenses not set.")
                    End Try

                End With


                MyIBMFileNetServer = Nothing
            Next

        Catch exception As DataException
            WriteAuditEntry(Now.ToString & " IBM FileNet Servers data exception " & exception.Message)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " IBM FileNet Servers general exception " & ex.ToString)
        End Try

        InsufficentLicensesTest(MyIBMFileNetServers)

    End Sub


    Private Sub InsufficentLicensesTest(ByRef coll As System.Collections.CollectionBase)
        'Return
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

