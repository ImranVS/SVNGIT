Imports VitalSignsMicrosoftClasses
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports System.IO
Imports System.Threading
Imports System.Configuration
Public Class FrmTest
	Dim firstButtonClick As Boolean = True
	Dim vsClassesExchange As New ExchangeMAIN()
	Dim vsClassesSharePoint As New SharepointMAIN()
	Dim vsClassesActiveDirectory As New ActiveDirectoryMAIN()
	Dim vsClassesWindows As New WindowsMAIN()
	Dim vsClassesOffice365 As New Office365MAIN()

	Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTest.Click

		Dim MasterSharepointThread As Thread

		If (firstButtonClick) Then
			Dim utils As New LogUtilities.LogUtils()
			firstButtonClick = False
			Dim monitored As New MonitorTables(vsClassesActiveDirectory, vsClassesExchange, vsClassesSharePoint, vsClassesWindows, vsClassesOffice365)
			MasterSharepointThread = New Thread(New ThreadStart(AddressOf monitored.CheckForTableChanges))
			'Thread MasterExchangeThread = new Thread(new ThreadStart(exMain.StartProcess));
			MasterSharepointThread.IsBackground = True
			MasterSharepointThread.Name = "Table Changes Main Thread"
			MasterSharepointThread.Priority = ThreadPriority.Normal
			MasterSharepointThread.Start()
			Thread.Sleep(2000)
		End If

		MasterSharepointThread = New Thread(Sub() vsClassesExchange.StartProcess(New MicrosoftHelperObject))
		'Thread MasterExchangeThread = new Thread(new ThreadStart(exMain.StartProcess));
		MasterSharepointThread.IsBackground = True
		MasterSharepointThread.Priority = ThreadPriority.Normal
		MasterSharepointThread.Name = "Exchange Main Thread"
		MasterSharepointThread.Start()
		Thread.Sleep(2000)

		

	End Sub


	Private Sub btnSharepoint_Click(sender As System.Object, e As System.EventArgs) Handles btnSharepoint.Click

		Dim MasterSharepointThread As Thread

		If (firstButtonClick) Then
			Dim utils As New LogUtilities.LogUtils()
			firstButtonClick = False
			Dim monitored As New MonitorTables(vsClassesActiveDirectory, vsClassesExchange, vsClassesSharePoint, vsClassesWindows, vsClassesOffice365)
			MasterSharepointThread = New Thread(New ThreadStart(AddressOf monitored.CheckForTableChanges))
			'Thread MasterExchangeThread = new Thread(new ThreadStart(exMain.StartProcess));
			MasterSharepointThread.IsBackground = True
			MasterSharepointThread.Name = "Table Changes Main Thread"
			MasterSharepointThread.Priority = ThreadPriority.Normal
			MasterSharepointThread.Start()
			Thread.Sleep(2000)
		End If

		Dim vsClasses As New SharepointMAIN()
		'//vsClasses.StartProcess()
		MasterSharepointThread = New Thread(Sub() vsClasses.StartProcess(New MicrosoftHelperObject))
		'Thread MasterExchangeThread = new Thread(new ThreadStart(exMain.StartProcess));
		MasterSharepointThread.IsBackground = True
		MasterSharepointThread.Priority = ThreadPriority.Normal
		MasterSharepointThread.Name = "SP Main Thread"
		MasterSharepointThread.Start()
		Thread.Sleep(2000)

	End Sub

	Private Sub btnActiveDirectory_Click(sender As System.Object, e As System.EventArgs) Handles btnActiveDirectory.Click

		Dim MasterADThread As Thread

		If (firstButtonClick) Then
			Dim utils As New LogUtilities.LogUtils()
			firstButtonClick = False
			Dim monitored As New MonitorTables(vsClassesActiveDirectory, vsClassesExchange, vsClassesSharePoint, vsClassesWindows, vsClassesOffice365)
			MasterADThread = New Thread(New ThreadStart(AddressOf monitored.CheckForTableChanges))
			MasterADThread.IsBackground = True
			MasterADThread.Name = "Table Changes Main Thread"
			MasterADThread.Priority = ThreadPriority.Normal
			MasterADThread.Start()
			Thread.Sleep(2000)
		End If

		Dim vsClasses As New ActiveDirectoryMAIN()
		'//vsClasses.StartProcess()
		MasterADThread = New Thread(Sub() vsClasses.StartProcess(New MicrosoftHelperObject))
		'Thread MasterExchangeThread = new Thread(new ThreadStart(exMain.StartProcess));
		MasterADThread.IsBackground = True
		MasterADThread.Name = "AD Main Thread"
		MasterADThread.Priority = ThreadPriority.Normal
		MasterADThread.Start()
		Thread.Sleep(2000)

	End Sub

	Private Sub btnOffice365_Click(sender As System.Object, e As System.EventArgs) Handles btnOffice365.Click

		Dim MasterO365Thread As Thread

		If (firstButtonClick) Then
			Dim utils As New LogUtilities.LogUtils()
			firstButtonClick = False
			Dim monitored As New MonitorTables(vsClassesActiveDirectory, vsClassesExchange, vsClassesSharePoint, vsClassesWindows, vsClassesOffice365)
			MasterO365Thread = New Thread(New ThreadStart(AddressOf monitored.CheckForTableChanges))
			MasterO365Thread.IsBackground = True
			MasterO365Thread.Name = "Table Changes Main Thread"
			MasterO365Thread.Priority = ThreadPriority.Normal
			MasterO365Thread.Start()
			Thread.Sleep(2000)
		End If

		MasterO365Thread = New Thread(Sub() vsClassesOffice365.StartProcess(New MicrosoftHelperObject))
		'Thread MasterExchangeThread = new Thread(new ThreadStart(exMain.StartProcess));
		MasterO365Thread.IsBackground = True
		MasterO365Thread.Priority = ThreadPriority.Normal
		MasterO365Thread.Name = "O365 Main Thread"
		MasterO365Thread.Start()
		Thread.Sleep(2000)

		

	End Sub

	Private Sub btnWindows_Click(sender As System.Object, e As System.EventArgs) Handles btnWindows.Click

		Dim MasterWindowsThread As Thread

		If (firstButtonClick) Then
			Dim utils As New LogUtilities.LogUtils()
			firstButtonClick = False
			Dim monitored As New MonitorTables(vsClassesActiveDirectory, vsClassesExchange, vsClassesSharePoint, vsClassesWindows, vsClassesOffice365)
			MasterWindowsThread = New Thread(New ThreadStart(AddressOf monitored.CheckForTableChanges))
			MasterWindowsThread.IsBackground = True
			MasterWindowsThread.Name = "Table Changes Main Thread"
			MasterWindowsThread.Priority = ThreadPriority.Normal
			MasterWindowsThread.Start()
			Thread.Sleep(2000)
		End If

		Dim vsClasses As New WindowsMAIN()
		'//vsClasses.StartProcess()
		MasterWindowsThread = New Thread(Sub() vsClasses.StartProcess(New MicrosoftHelperObject))
		'Thread MasterExchangeThread = new Thread(new ThreadStart(exMain.StartProcess));
		MasterWindowsThread.Name = "AD Main Thread"
		MasterWindowsThread.IsBackground = True
		MasterWindowsThread.Priority = ThreadPriority.Normal
		MasterWindowsThread.Start()
		Thread.Sleep(2000)

		
	End Sub

	



	Public Class MicrosoftHelperObject

		Public Sub CheckForInsufficentLicenses(objServers As Object, ServerType As String, ServerTypeForTypeAndName As String)

			FrmTest.CheckForInsufficentLicenses(objServers, ServerType, ServerTypeForTypeAndName)
		End Sub

	End Class

	Public Shared Sub CheckForInsufficentLicenses(objServers As Object, ServerType As String, ServerTypeForTypeAndName As String)
		Dim servers As MonitoredItems.MonitoredDevicesCollection = CType(objServers, MonitoredItems.MonitoredDevicesCollection)
		Dim sql As String = ""
		Dim adapter As New VSFramework.VSAdaptor


		If servers.Count > 0 Then

			For i As Integer = 0 To i < servers.Count

				Dim server As MonitoredItems.MonitoredDevice = servers.Item(i)
				Dim t As Type = server.GetType()
				If server.InsufficentLicenses Then
					sql = "DELETE FROM Status WHERE TypeANDName='" & server.Name & "-" & ServerTypeForTypeAndName & "';" & _
					  " INSERT INTO Status (Type, Location, Category, Name, Status, Details, LastUpdate, Description, NextScan, TypeANDName, StatusCode) VALUES " & _
					  "('" & ServerType & "','" & server.Location & "','" & server.Category & "','" & server.Name & "','Insufficient Licenses','There are not enough licenses to scan this server.',getDate()," & _
					 "'There are not enough licenses to scan this server.',dateadd(day,1,getdate()),'" & server.Name & "-" & ServerTypeForTypeAndName & "','Maintenance');"
					adapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", sql)

					servers.Delete(server.Name)
					i = i - 1
				End If

			Next

            '10/3/2016 NS commented out per discussion with Wes - the insufficient licenses system message is being queued 
            'using the QueueSysMessage function elsewhere. The SysMessageForLicenses is outdated and will be removed from the Alertdll
            'Dim myAlert As New AlertLibrary.Alertdll()
            'myAlert.SysMessageForLicenses()

        End If
	End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles btnLicense.Click
        Dim vs As New VitalSignsLicensing.Licensing
        vs.refreshServerCollectionWrapper()
    End Sub

    Private Sub btnPing_Click(sender As Object, e As EventArgs) Handles btnPing.Click
        'Dim nodeName As String =System.Configuration.con
        Dim nodeName As String = ConfigurationManager.ConnectionStrings("VitalSignsMongo").ToString()
        Dim vs As New VitalSignsLicensing.Licensing
        vs.doMasterPing("VSNode2", "VSNode2", "4.2.0")
    End Sub
End Class