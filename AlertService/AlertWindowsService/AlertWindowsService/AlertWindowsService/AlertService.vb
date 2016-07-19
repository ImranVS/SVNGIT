Imports System.Threading
Imports System.ServiceProcess
Imports System.Data.SqlClient
Imports System.Data
Imports System.Net.Mail
Imports System.IO
Imports nsoftware.IPWorks
Imports Chilkat
Imports System.Configuration
'4/4/2014 NS added for VSPLUS-403
Imports System.Diagnostics
'12/1/2014 NS added for VSPLUS-946
Imports Twilio
Imports System.Globalization
Imports RPRWyatt.VitalSigns.Services
'5/20/2015 NS added for VSPLUS-1764
Imports SnmpSharpNet
'11/17/2015 NS added for VSPLUS-1562
Imports Microsoft.Win32
Imports VSNext.Mongo
Imports VSNext.Mongo.Entities
Imports VSNext.Mongo.Repository
Imports MongoDB.Driver
Imports MongoDB.Bson

Public Class AlertDefinition
    Implements IComparer
    Public Sub New(ByVal akey As Integer)
        _AlertKey = akey
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As _
        Object) As Integer Implements _
        System.Collections.IComparer.Compare
        Dim ax As AlertDefinition = DirectCast(x, AlertDefinition)
        Dim ay As AlertDefinition = DirectCast(y, AlertDefinition)
        Dim intx As Integer
        Dim inty As Integer

        intx = ax.AlertKey
        inty = ay.AlertKey

        Return intx > inty
    End Function

    Private _AlertKey As Integer
    Private _AlertHistoryId As Integer
    Private _EventName As String
    Private _ServerType As String
    Private _ServerName As String
    Private _Details As String
    Private _SendSNMPTrap As Boolean
    Private _SendTo As String
    Private _CopyTo As String
    Private _BlindCopyTo As String
    Private _StartTime As String
    Private _Duration As Integer
    Private _Days As String
    Private _IntType As Integer
    Private _RunNow As Integer
    Private _Repeat As Boolean
    '2/17/2014 NS added for cleared alerts
    Private _SentID As Integer
    '4/7/2014 NS added for VSPLUS-519
    Private _EnablePersistentAlert As Boolean
    Private _DateCreated As String
    '12/1/2014 NS added for VSPLUS-946
    Private _SMSTo As String
    '12/9/2014 NS added for VSPLUS-1229
    Private _ScriptName As String
    Private _ScriptCommand As String
    Private _ScriptLocation As String

    Public Sub New()   'constructor
        'Console.WriteLine("Object is being created")
        _Repeat = False
    End Sub

    Public Property AlertKey() As Integer
        Get
            Return _AlertKey
        End Get
        Set(ByVal value As Integer)
            _AlertKey = value
        End Set
    End Property
    Public Property AlertHistoryId() As Integer
        Get
            Return _AlertHistoryId
        End Get
        Set(ByVal value As Integer)
            _AlertHistoryId = value
        End Set
    End Property
    Public Property EventName() As String
        Get
            Return _EventName
        End Get
        Set(ByVal value As String)
            _EventName = value
        End Set
    End Property
    Public Property ServerType() As String
        Get
            Return _ServerType
        End Get
        Set(ByVal value As String)
            _ServerType = value
        End Set
    End Property
    Public Property ServerName() As String
        Get
            Return _ServerName
        End Get
        Set(ByVal value As String)
            _ServerName = value
        End Set
    End Property
    Public Property Details() As String
        Get
            Return _Details
        End Get
        Set(ByVal value As String)
            _Details = value
        End Set
    End Property
    Public Property SendSNMPTrap() As String
        Get
            Return _SendSNMPTrap
        End Get
        Set(ByVal value As String)
            _SendSNMPTrap = value
        End Set
    End Property
    Public Property SendTo() As String
        Get
            Return _SendTo
        End Get
        Set(ByVal value As String)
            _SendTo = value
        End Set
    End Property
    Public Property CopyTo() As String
        Get
            Return _CopyTo
        End Get
        Set(ByVal value As String)
            _CopyTo = value
        End Set
    End Property
    Public Property BlindCopyTo() As String
        Get
            Return _BlindCopyTo
        End Get
        Set(ByVal value As String)
            _BlindCopyTo = value
        End Set
    End Property
    Public Property StartTime() As String
        Get
            Return _StartTime
        End Get
        Set(ByVal value As String)
            _StartTime = value
        End Set
    End Property
    Public Property Duration() As Integer
        Get
            Return _Duration
        End Get
        Set(ByVal value As Integer)
            _Duration = value
        End Set
    End Property
    Public Property Days() As String
        Get
            Return _Days
        End Get
        Set(ByVal value As String)
            _Days = value
        End Set
    End Property
    Public Property IntType() As Integer
        Get
            Return _IntType
        End Get
        Set(ByVal value As Integer)
            _IntType = value
        End Set
    End Property
    Public Property RunNow() As Integer
        Get
            Return _RunNow
        End Get
        Set(ByVal value As Integer)
            _RunNow = value
        End Set
    End Property
    Public Property Repeat() As Boolean
        Get
            Return _Repeat
        End Get
        Set(ByVal value As Boolean)
            _Repeat = value
        End Set
    End Property
    Public Property SentID() As Integer
        Get
            Return _SentID
        End Get
        Set(ByVal value As Integer)
            _SentID = value
        End Set
    End Property
    Public Property EnablePersistentAlert() As Boolean
        Get
            Return _EnablePersistentAlert
        End Get
        Set(ByVal value As Boolean)
            _EnablePersistentAlert = value
        End Set
    End Property
    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property
    '12/1/2014 NS added for VSPLUS-946
    Public Property SMSTo() As String
        Get
            Return _SMSTo
        End Get
        Set(ByVal value As String)
            _SMSTo = value
        End Set
    End Property
    '12/9/2014 NS added for VSPLUS-1229
    Public Property ScriptName() As String
        Get
            Return _ScriptName
        End Get
        Set(ByVal value As String)
            _ScriptName = value
        End Set
    End Property
    Public Property ScriptCommand() As String
        Get
            Return _ScriptCommand
        End Get
        Set(ByVal value As String)
            _ScriptCommand = value
        End Set
    End Property
    Public Property ScriptLocation() As String
        Get
            Return _ScriptLocation
        End Get
        Set(ByVal value As String)
            _ScriptLocation = value
        End Set
    End Property
End Class

Public Class VitalSignsAlertService
    Inherits VSServices
    Dim Stopping As Boolean = False
    Dim WithEvents SNMPAgent As New nsoftware.IPWorks.Snmpagent("31504E3641414E5852464336354235353231000000000000000000000000000000000000000000004D3047595958364A00003042394E505658333642345A0000")
    Dim BuildNumber As Integer = 16
    '12/16/2014 NS commented out for VSPLUS-1267 - the variables will have to be localized
    'Dim con As New SqlConnection
    'Dim myAdapter As New VSFramework.XMLOperation
    'Dim sCultureString As String = "en-US"
    'Dim connectionStringName As String = "CultureString"
    '2/17/2014 NS moved the variable definitions from the function ProcessAlertsSendNotification to global - need to use them 
    'in the cleared alerts function ProcessAlertsClear
    Dim AlertDict As New Dictionary(Of Integer, AlertDefinition())
    Dim pair As KeyValuePair(Of Integer, AlertDefinition())
    Enum LogLevel
        Verbose = LogUtilities.LogUtils.LogLevel.Verbose
        Debug = LogUtilities.LogUtils.LogLevel.Debug
        Normal = LogUtilities.LogUtils.LogLevel.Normal
    End Enum
    '11/17/2015 NS modified for VSPLUS-1562
    Dim myRegistry As New RegistryHandler()
    
    'MyLogLevel is used throughout to control the volume of the log file output
    Dim MyLogLevel As LogLevel
    '7/20/2015 NS added for VSPLUS-1562
    Dim emergencyAlertSent As Boolean = False
    Private Function GetDBConnection() As String
        Return "mongodb://localhost/local"
    End Function

    Protected Overrides Sub ServiceOnStart(args() As String)
        '12/16/2014 NS added for VSPLUS-1267
        Dim sCultureString As String = "en-US"
        Dim connectionStringName As String = "CultureString"

        Try
            sCultureString = ConfigurationManager.AppSettings(connectionStringName).ToString()
        Catch ex As Exception
            sCultureString = "en-US"
        End Try
        '12/17/2014 NS added for VSPLUS-1267
        Thread.CurrentThread.CurrentCulture = New CultureInfo(sCultureString)

        Dim strAppPath As String

        Try
            MyLogLevel = myRegistry.ReadFromRegistry("Log Level")
        Catch ex As Exception
            '9/30/2014 NS modified - log level should be normal
            'MyLogLevel = LogLevel.Verbose
            MyLogLevel = LogLevel.Normal
        End Try

        Try
            strAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
            File.Delete(strAppPath & "\Log_Files\All_Alert_Service.txt")
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error deleting log file " & ex.ToString, LogLevel.Normal)
        End Try

        '12/16/2014 NS commented out for VSPLUS-1267 - con is localized
        'con.ConnectionString = myAdapter.GetDBConnectionString("VitalSigns")

        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        ' Log a service start message to the Application log. 
        Dim mySettings As New VSFramework.RegistryHandler
        Me.EventLog1.WriteEntry("VitalSigns Alert Service is starting up.")
        Try
            mySettings.WriteToRegistry("Alert Service Start", Now.ToString)
            mySettings.WriteToRegistry("Alert Service Build", BuildNumber)
        Catch ex As Exception

        End Try

        WriteServiceHistoryEntry(Now.ToString & " The VitalSigns Alert Service is starting up. ", LogLevel.Normal)
        WriteServiceHistoryEntry(Now.ToString & " The VitalSigns Alert Service is Copyright " & Now.Year.ToString & ", JNIT, Inc. dba RPR Wyatt and MZL Software Development, Inc. ", LogLevel.Normal)
        Dim emergencyInfoThread As New Thread(Sub()
                                                  Dim i As Integer = 0
                                                  While True
                                                      Try
                                                          GetEmergencyAlertInfo()
                                                          Thread.Sleep(New TimeSpan(1, 0, 0))
                                                      Catch ex As Exception
                                                          WriteServiceHistoryEntry(Now.ToString & " The following exception has occurred in the emergencyInfoThread: " & ex.Message, LogLevel.Normal)
                                                      End Try
                                                  End While
                                              End Sub)
        emergencyInfoThread.Start()
        ' Queue the main service function for execution in a worker thread. 
        '11/27/2013 NS modified for testing 
        '12/17/2014 NS modified for VSPLUS-1267
        'ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf ServiceWorkerThreadNew))
        Dim mainThread As New Thread(AddressOf ServiceWorkerThreadNew)
        mainThread.CurrentCulture = New CultureInfo(sCultureString)
        mainThread.Start()
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        ' Log a service stop message to the Application log. 
        Me.EventLog1.WriteEntry("The VitalSigns Alert Service is shutting down.")
        WriteServiceHistoryEntry(Now.ToString & " The VitalSigns Alert Service is shutting down. ", LogLevel.Normal)
        ' Indicate that the service is stopping and wait for the finish of  
        ' the main service function (ServiceWorkerThread). 
        Dim mySettings As New VSFramework.RegistryHandler
        mySettings.WriteToRegistry("Alert Service End", Now.ToString)
        Me.Stopping = True
		MyBase.OnStop()
    End Sub

    Private Overloads Sub WriteServiceHistoryEntry(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Verbose)
        MyBase.WriteHistoryEntry(strMsg, "All_Alert_Service.txt", LogLevelInput)
    End Sub

    '9/30/2014 NS modified - default log level should be verbose
    'Private Sub WriteServiceHistoryEntry(ByVal strMsg As String, Optional ByVal LogLevelInput As LogLevel = LogLevel.Verbose)

    '	If (LogLevelInput < MyLogLevel) Then
    '		Return
    '	End If

    '	Dim appendMode As Boolean = True

    '	' Dim myRegistry As New RegistryHandler
    '	Dim ServiceLogDestination As String = AppDomain.CurrentDomain.BaseDirectory.ToString & "\Log_Files\All_Alert_Service.txt"

    '	Try
    '		Dim sw As New StreamWriter(ServiceLogDestination, appendMode, System.Text.Encoding.Unicode)
    '		sw.WriteLine(strMsg)
    '		sw.Close()
    '		sw = Nothing
    '	Catch ex As Exception

    '	End Try
    '	GC.Collect()
    'End Sub

    Private Sub ServiceWorkerThreadNew()
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

        WriteServiceHistoryEntry(Now.ToString & " The Service Worker Thread is starting up. ", LogLevel.Normal)
        ' Periodically check if the service is stopping. 

        Do While Not Me.Stopping
            ' Perform main service function here... 
            '1/6/2014 NS added a check to see if the alerting service should be sending alerts
            Dim alertson As Boolean
            alertson = True

            Try
                '1/6/2014 NS added a check to see if the alerting service should be sending alerts
                '12/16/2014 NS modified for VSPLUS-1267
                Dim sqlstm As String = "SELECT svalue FROM Settings WHERE sname = 'AlertsOn'"
                'Dim DA2 As New SqlDataAdapter(sqlstm, con)
                'Dim DS2 As New DataSet
                'DA2.Fill(DS2, "svalue")
                'Dim dtmail As DataTable = DS2.Tables(0)
                WriteServiceHistoryEntry(Now.ToString & " ServiceWorkerThreadNew - trying to get AlertsOn from the Settings table", LogLevel.Verbose)
                Dim dtmail As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sqlstm)
                If dtmail.Rows.Count > 0 Then
                    alertson = Convert.ToBoolean(dtmail.Rows(0)("svalue").ToString())
                End If

            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " Error ServiceWorkerThread when getting the value of AlertsOn from the Settings table: " & ex.Message, LogLevel.Normal)
            End Try

            '1/6/2014 NS added - we only want to send alerts if the flag is enabled, otherwise, continue with clearing and keep checking
            If (alertson) Then
                Try
                    WriteServiceHistoryEntry(Now.ToString & " ServiceWorkerThreadNew - trying to process alerts", LogLevel.Verbose)
                    If Not myRegistry Is Nothing Then
                        WriteServiceHistoryEntry(Now.ToString & " ServiceWorkerThreadNew - got myRegistry object", LogLevel.Verbose)
                        tempObj = myRegistry.ReadFromRegistry("Alert Emergency Contacts")
                        If Not tempObj Is Nothing Then
                            emergencyContacts = tempObj
                            If emergencyContacts.Length > 0 Then
                                emergencyContacts = emergencyContacts.Substring(0, emergencyContacts.Length - 1)
                            End If
                            WriteServiceHistoryEntry(Now.ToString & " Alert Emergency Contacts: " & emergencyContacts, LogLevel.Verbose)
                        End If
                        tempObj = myRegistry.ReadFromRegistry("Alert Emergency PrimaryHostName")
                        If Not tempObj Is Nothing Then
                            PHostName = tempObj
                            WriteServiceHistoryEntry(Now.ToString & " Alert Emergency PrimaryHostName: " & PHostName, LogLevel.Verbose)
                        End If
                        tempObj = myRegistry.ReadFromRegistry("Alert Emergency primaryport")
                        If Not tempObj Is Nothing Then
                            Pport = tempObj
                            WriteServiceHistoryEntry(Now.ToString & " Alert Emergency primaryport: " & Pport, LogLevel.Verbose)
                        End If
                        tempObj = myRegistry.ReadFromRegistry("Alert Emergency primaryUserID")
                        If Not tempObj Is Nothing Then
                            PEmail = tempObj
                            WriteServiceHistoryEntry(Now.ToString & " Alert Emergency primaryUserID: " & PEmail, LogLevel.Verbose)
                        End If
                        tempObj = myRegistry.ReadFromRegistry("Alert Emergency primarypwd")
                        If Not tempObj Is Nothing Then
                            Ppwd = tempObj
                            WriteServiceHistoryEntry(Now.ToString & " Alert Emergency primarypwd: " & Ppwd, LogLevel.Verbose)
                        End If
                        tempObj = myRegistry.ReadFromRegistry("Alert Emergency primaryFrom")
                        If Not tempObj Is Nothing Then
                            PFrom = tempObj
                            WriteServiceHistoryEntry(Now.ToString & " Alert Emergency primaryFrom: " & PFrom, LogLevel.Verbose)
                        End If
                        tempObj = myRegistry.ReadFromRegistry("Alert Emergency primaryAuth")
                        If Not tempObj Is Nothing Then
                            If tempObj.ToString() <> "" Then
                                '12/3/2015 NS modified for VSPLUS-1562
                                PAuth = Convert.ToBoolean(tempObj.ToString())
                                WriteServiceHistoryEntry(Now.ToString & " Alert Emergency primaryAuth: " & PAuth, LogLevel.Verbose)
                            End If
                        End If
                        tempObj = myRegistry.ReadFromRegistry("Alert Emergency primarySSL")
                        If Not tempObj Is Nothing Then
                            If tempObj.ToString() <> "" Then
                                '12/8/2015 NS modified for VSPLUS-1562
                                PSSL = Convert.ToBoolean(tempObj.ToString())
                                WriteServiceHistoryEntry(Now.ToString & " Alert Emergency primarySSL: " & PSSL, LogLevel.Verbose)
                            End If
                        End If
                    End If
                    WriteServiceHistoryEntry(Now.ToString & " Before calling ProcessAlertsSendNotification()", LogLevel.Verbose)
                    ProcessAlertsSendNotification()
                Catch ex As Exception
                    WriteServiceHistoryEntry(Now.ToString & " Error in ServiceWorkerThreadNew: " & ex.Message, LogLevel.Normal)
                    '7/20/2015 NS added for VSPLUS-1562
                    Try
                        If Not emergencyAlertSent Then
                            If emergencyContacts <> "" Then
								SendMailNet(PHostName, Pport, PAuth, False, Ppwd, PEmail, PSSL, "The SQL server seems to be down or is not accessbile. VitalSigns will not be able to function without proper SQL access. Please contact your SQL server administrator immediately in order to ensure proper VitalSigns operation.", False, "", "", "", emergencyContacts, "", "", PFrom, True)
                                emergencyAlertSent = True
                            End If
                        End If
                    Catch ex1 As Exception
                        WriteServiceHistoryEntry(Now.ToString & " Error ServiceWorkerThreadNew when sending emergency email: " & ex1.Message, LogLevel.Normal)
                    End Try

                    Thread.Sleep(5000)  ' Wait a while and then do it again
                End Try
            End If

            Try
                'WriteServiceHistoryEntry(Now.ToString & " Calling ProcessAlertsClear")
                '2/17/2014 NS modified the function that sends cleared alerts to make sure the time frame of a send event is considered
                'ProcessAlertsClear()
                WriteServiceHistoryEntry(Now.ToString & " ServiceWorkerThreadNew - trying process clear alerts", LogLevel.Verbose)
                ProcessAlertsClearSendNotification()
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " Error ServiceWorkerThreadNew when calling ProcessAlertsClearSendNotification(): " & ex.Message, LogLevel.Normal)
            End Try

            Thread.Sleep(10000)  ' Wait a while and then do it again
        Loop


        WriteServiceHistoryEntry(Now.ToString & " The Service Worker Thread is shutting down... ", LogLevel.Normal)
        ' Signal the stopped event. 
        'Me.stoppedEvent.Set()
    End Sub

    Private Function stoppedEvent() As Object
        Throw New NotImplementedException
    End Function

    Public Sub SendMailwithChilkatorNet(ByVal SendTo As String, ByVal CC As String, ByVal BCC As String, ByVal ServerName As String,
     ByVal ServerType As String, ByVal Location As String, ByVal EventName As String, ByVal AlertType As String,
     ByVal Details As String, ByVal iscleared As String, ByVal SubjectStr As String)
        Dim boolHSBC As Boolean = False
        If InStr(ServerName.ToUpper, "HSBC") > 0 Then
            boolHSBC = True
        End If

        Dim mailman As New Chilkat.MailMan
        Try
            mailman.UnlockComponent("MZLDADMAILQ_8nzv7Kxb4Rpx")
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error unlocking Chilkat component: " & ex.ToString, LogLevel.Normal)
        End Try


        'Primary Server Credentials
        Dim PHostName As String = ""
        Dim Pport As String = ""
        Dim PEmail As String = ""
        Dim Ppwd As String = ""
        Dim PSNMP As Boolean = False
        Dim PAuth As Boolean = False
        Dim PFrom As String = ""
        '4/15/2014 NS added
        Dim PSSL As Boolean = False
        '3/31/2014 NS added for VSPLUS-489
        'Secondary Server Credentials - credentials will be reused if Chilkat send fails
        Dim PHostName2 As String = ""
        Dim Pport2 As String = ""
        Dim PEmail2 As String = ""
        Dim Ppwd2 As String = ""
        Dim PSNMP2 As Boolean = False
        Dim PAuth2 As Boolean = False
        Dim PFrom2 As String = ""
        '4/15/2014 NS added
        Dim PSSL2 As Boolean = False

        '2/28/2014 NS added for VSPLUS-326
        'Flag variable to set to False if the primary server send fails
        Dim emailSent As Boolean = True

        Try
            PHostName = getSettings("PrimaryHostName")
            Pport = getSettings("primaryport")
            PEmail = getSettings("primaryUserID")
            Ppwd = getSettings("primarypwd")
            PFrom = getSettings("primaryFrom")
            PAuth = Convert.ToBoolean(getSettings("primaryAuth").ToString())
            '4/15/2014 NS added
            PSSL = Convert.ToBoolean(getSettings("primarySSL").ToString())
            If PFrom.ToString = "" Then
                PFrom = "VS Plus"
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error getting primary settings from the Settings table:  " & ex.ToString, LogLevel.Normal)
        End Try
        '9/25/2014 NS commented out
        'WriteServiceHistoryEntry(Now.ToString & " pfrom:  " & PFrom)
        emailSent = SendMail(mailman, PHostName, Pport, PAuth, boolHSBC, Ppwd, PEmail, Details, iscleared, ServerType, ServerName,
        AlertType, SendTo, CC, BCC, PFrom, SubjectStr)

        'If an error occurred while sending email, try secondary server
        If Not emailSent Then
            WriteServiceHistoryEntry(Now.ToString & " Primary server has failed. Trying the secondary server now...", LogLevel.Normal)
            Try
                '3/31/2014 NS modified for VSPLUS-489
                PHostName2 = getSettings("SecondaryHostName")
                Pport2 = getSettings("Secondaryport")
                PEmail2 = getSettings("SecondaryUserID")
                Ppwd2 = getSettings("Secondarypwd")
                PFrom2 = getSettings("SecondaryFrom")
                PAuth2 = Convert.ToBoolean(getSettings("SecondaryAuth").ToString())
                If PFrom2.ToString = "" Then
                    PFrom2 = "VS Plus"
                End If
                '4/15/2014 NS added
                PSSL2 = Convert.ToBoolean(getSettings("SecondarySSL").ToString())
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " Error getting secondary settings from the Settings table:  " & ex.ToString, LogLevel.Normal)
            End Try

            '5/7/2015 NS modified for VSPLUS-1553
            If PHostName2 = "" Then
                WriteServiceHistoryEntry(Now.ToString & " The secondary server is not defined. The alert will not be sent.", LogLevel.Normal)
            Else
                '3/31/2014 NS modified for VSPLUS-489
                emailSent = SendMail(mailman, PHostName2, Pport2, PAuth2, boolHSBC, Ppwd2, PEmail2, Details, iscleared, ServerType, ServerName,
                   AlertType, SendTo, CC, BCC, PFrom2, SubjectStr)
                If Not emailSent Then
                    WriteServiceHistoryEntry(Now.ToString & " Secondary server has failed as well.", LogLevel.Normal)
                End If
            End If
        End If
        '3/31/2014 NS added for VSPLUS-489
        'If Chilkat fails, try sending via .Net
        If Not emailSent Then
            WriteServiceHistoryEntry(Now.ToString & " Trying to Send via .Net SMTP using Primary Server.", LogLevel.Normal)
            emailSent = SendMailNet(PHostName, Pport, PAuth, boolHSBC, Ppwd, PEmail, PSSL, Details, iscleared, ServerType, ServerName,
              AlertType, SendTo, CC, BCC, PFrom, False)
            If Not emailSent Then
                WriteServiceHistoryEntry(Now.ToString & " Primary server has failed via .Net SMTP mail send. Trying the secondary server now...", LogLevel.Normal)
                emailSent = SendMailNet(PHostName2, Pport2, PAuth2, boolHSBC, Ppwd2, PEmail2, PSSL2, Details, iscleared, ServerType, ServerName,
                 AlertType, SendTo, CC, BCC, PFrom2, False)
                If Not emailSent Then
                    WriteServiceHistoryEntry(Now.ToString & " Secondary server has failed via .Net SMTP mail send as well. No more send attempts.", LogLevel.Normal)
                End If
            End If
        End If
        'WriteServiceHistoryEntry(Now.ToString & " Reached the end of the Sendmail with Chilkat function.")
    End Sub
    '12/1/2014 NS added for VSPLUS-946
    Public Sub SendSMSwithTwilio(ByVal SMSTo As String, ByVal ServerName As String,
     ByVal ServerType As String, ByVal AlertType As String,
     ByVal Details As String, ByVal iscleared As String, ByVal PFrom As String)
        Dim SMSAccountSid As String
        Dim SMSAuthToken As String
        Dim SMSFrom As String
        Try
            SMSAccountSid = getSettings("SMSAccountSid")
            SMSAuthToken = getSettings("SMSAuthToken")
            SMSFrom = getSettings("SMSFrom")
            SendSMS(SMSAccountSid, SMSAuthToken, iscleared, ServerType, ServerName, AlertType, Details, SMSTo, SMSFrom)
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error getting SMS account settings from the Settings table:  " & ex.ToString, LogLevel.Normal)
        End Try
    End Sub
    '2/28/2014 NS added for VSPLUS-326
    Public Function SendMail(ByVal mailMan As Chilkat.MailMan, ByVal PHostName As String, ByVal Pport As String, ByVal PAuth As Boolean,
     ByVal boolHSBC As Boolean, ByVal Ppwd As String, ByVal PEmail As String,
     ByVal Details As String, ByVal iscleared As String, ByVal ServerType As String, ByVal ServerName As String,
     ByVal AlertType As String, ByVal SendTo As String, ByVal CC As String, ByVal BCC As String, ByVal PFrom As String,
     ByVal SubjectStr As String) As Boolean
        Dim success As Boolean = True
        With mailMan
            .SmtpHost = PHostName
            .SmtpPort = Pport
            If PAuth = True And boolHSBC = False Then
                .SmtpPassword = Ppwd
                .SmtpUsername = PEmail
            End If
        End With

        '1/12/2016 NS commented out - the combination of port 587 and SSL true is not always the case
        'Try
        '    If InStr(PHostName.ToUpper, "GMAIL") Then
        '        If mailMan.SmtpPort <> 587 Then
        '            WriteServiceHistoryEntry(Now.ToString & " GMail uses port 587 so I am setting it to use that port.", LogLevel.Normal)
        '            mailMan.SmtpPort = 587
        '            mailMan.SmtpSsl = True
        '        End If
        '    End If
        'Catch ex As Exception
        '    WriteServiceHistoryEntry(Now.ToString & " Error setting port in SendMail.", LogLevel.Normal)
        'End Try

        Try
            Dim email As New Chilkat.Email
            '4/21/2016 NS modified for VSPLUS-2755
            Dim localZone = TimeZone.CurrentTimeZone
            email.Body = Details & vbCrLf & vbCrLf & "Message sent: " & Now.ToLongDateString & " " & Now.ToShortTimeString & " " & localZone.StandardName
            '4/8/2015 NS modified for VSPLUS-219
            email.Subject = iscleared & SubjectStr & " for " & ServerType & " " & ServerName & " - " & AlertType
            email.AddTo(SendTo, SendTo)
            '1/30/2014 NS added for VSPLUS-315
            email.AddCC(CC, CC)
            email.AddBcc(BCC, BCC)
            email.FromAddress = PEmail
            email.ReplyTo = PEmail
            email.FromName = PFrom
            '9/25/2014 NS commented out
            'WriteServiceHistoryEntry(Now.ToString & " email.FromName:  " & email.FromName)
            ' Send mail.
            success = mailMan.SendEmail(email)
            If success Then
                WriteServiceHistoryEntry(Now.ToString & " Sent SMTP mail to " & SendTo & " re: " & ServerName & ", " & Details, LogLevel.Normal)
                WriteServiceHistoryEntry(Now.ToString & " Email subject was " & email.Subject & vbCrLf, LogLevel.Normal)
            Else
                WriteServiceHistoryEntry(Now.ToString & " Error sending mail in SendMail: " & mailMan.LastErrorText, LogLevel.Normal)
            End If
            email.Dispose()

            'success = Nothing

        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error sending mail in SendMail: " & ex.Message, LogLevel.Normal)

        End Try

        Try
            mailMan.Dispose()

            GC.Collect()
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error while disposing of the mail object in SendMail.", LogLevel.Normal)
        End Try
        Return success
    End Function
    '12/1/2014 NS added for VSPLUS-946
    Public Function SendSMS(ByVal SMSAccountSid As String, ByVal SMSAuthToken As String, ByVal iscleared As String, ByVal ServerType As String,
       ByVal ServerName As String, ByVal AlertType As String, ByVal Details As String, ByVal SMSTo As String, ByVal PFrom As String) As Boolean
        Dim success As Boolean = True
        Dim twilio As TwilioRestClient
        Dim sms As Twilio.SMSMessage
        Dim body As String
        '5/14/2015 NS modified for VSPLUS-1717
        body = iscleared & "Alert for " & ServerType & " " & ServerName & " - " & AlertType & vbCrLf & Details '& vbCrLf & "Message sent: " & Now.ToLongDateString & " " & Now.ToShortTimeString
        '12/12/2014 NS added - IMPORTANT - the max message size with Twilio is 160 characters. Attempting to send a larger SMS fails 
        'without producing an exception so there is no way of knowing the service has failed. Keep the message size to under 160!
        If body.Length > 160 Then
            body = Left(body, 159)
        End If
        Try
            If PFrom.Length = 10 Then
                PFrom = "+1" & PFrom
            End If
            WriteServiceHistoryEntry(Now.ToString & " Attempting to send SMS with the following parameters: from - " & PFrom & ", to - " & SMSTo & ", body - " & body, LogLevel.Normal)
            twilio = New TwilioRestClient(SMSAccountSid, SMSAuthToken)
            sms = twilio.SendSmsMessage(PFrom, SMSTo, body)
            If (Not sms.RestException Is vbNullString) Then
                WriteServiceHistoryEntry(Now.ToString & " REST exception... " & sms.RestException.Message)
            End If
            WriteServiceHistoryEntry(Now.ToString & " Received Sid " & sms.Sid & ", message status is " & sms.Status)
        Catch ex As Exception
            success = False
            WriteServiceHistoryEntry(Now.ToString & " Error while sending an SMS in SendSMS: " & ex.ToString, LogLevel.Normal)
        End Try
        Return success
    End Function
    '12/9/2014 NS added for VSPLUS-1229
    Public Function SendScript(ByVal ScriptName As String, ByVal ScriptCommand As String, ByVal ScriptLocation As String,
       ByVal ServerName As String, ByVal ServerType As String, ByVal AlertType As String,
       ByVal Details As String, ByVal iscleared As String)
        Dim success As Boolean = True
        Dim ServerNameParam As String = "%Name%"
        Dim ServerTypeParam As String = "%Type%"
        Dim EventTypeParam As String = "%EventType%"
        Dim AlertDetailsParam As String = "%Details%"
        Dim DTDParam As String = "%DTD%"

        Try
            WriteServiceHistoryEntry(Now.ToString & " Attempting to parametrize script " & ScriptCommand, LogLevel.Normal)
            ScriptCommand = ScriptCommand.Substring(ScriptCommand.IndexOf(" ") + 1)
            ScriptCommand = ScriptCommand.Replace(ServerNameParam, ServerName)
            ScriptCommand = ScriptCommand.Replace(ServerTypeParam, ServerType)
            ScriptCommand = ScriptCommand.Replace(EventTypeParam, AlertType)
            ScriptCommand = ScriptCommand.Replace(AlertDetailsParam, Details)
            ScriptCommand = ScriptCommand.Replace(DTDParam, Now.ToLongDateString)
            WriteServiceHistoryEntry(Now.ToString & " Attempting to execute script " & ScriptLocation & " " & ScriptCommand, LogLevel.Normal)
            Dim id As Integer = Shell(ScriptLocation & " " & ScriptCommand)
        Catch ex As Exception
            success = False
            WriteServiceHistoryEntry(Now.ToString & " In SendScript: " & ex.Message, LogLevel.Normal)
        End Try
        If success Then
            WriteServiceHistoryEntry(Now.ToString & " Sent script " & ScriptLocation & " " & ScriptCommand, LogLevel.Normal)
        End If
        Return success
    End Function

    Public Function getSettings(ByVal sname As String) As String
        '12/16/2014 NS added for VSPLUS-1267
        Dim myConnectionString As New VSFramework.XMLOperation
        Dim myAdapter As New VSFramework.VSAdaptor
        Dim str As String = ""
        Try
            Dim sqlQuery As String = "Select svalue from Settings where sname='" & sname & "'"
            '12/16/2014 NS modified for VSPLUS-1267
            'Dim SqlDA As New SqlDataAdapter(sqlQuery, con)
            'Dim ds As New DataSet
            'SqlDA.Fill(ds, "Settings")
            'Dim dt As DataTable = ds.Tables(0)
            Dim dt As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sqlQuery)
            '5/7/2015 NS modified for VSPLUS-1553
            If dt.Rows.Count > 0 Then
                str = dt.Rows(0)("svalue").ToString()
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error occurred at the time of getting value of " & sname & " from Settings Table " & ex.Message, LogLevel.Normal)
        End Try

        Return str
    End Function


    Private Sub InsertingSentMails(ByVal AlertID As Integer, ByVal SentMails As String, ByVal resent As Boolean, ByVal AlertKey As Integer)
        Dim connString As String = GetDBConnection()
        Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)
        Dim filterEventsDetected As MongoDB.Driver.FilterDefinition(Of EventsDetected)
        Dim updateEventsDetected As MongoDB.Driver.UpdateDefinition(Of EventsDetected)

        Try
            Dim vsObj As New VSFramework.VSAdaptor
            Dim str As String
            '12/17/2014 NS added
            Dim objDateUtils As New DateUtils.DateUtils
            Dim strDateFormat As String
            Dim strdt As String
            strDateFormat = objDateUtils.GetDateFormat()
            strdt = objDateUtils.FixDateTime(Date.Now, strDateFormat)

            '4/7/2014 NS modified for VSPLUS-519
            '12/17/2014 NS modified
            If resent Then
                str = "UPDATE AlertSentDetails SET AlertCreatedDateTime='" & strdt & "',AlertKey=" + AlertKey.ToString() + " WHERE AlertHistoryID=" & AlertID & " AND SentTo='" & SentMails & "' "
                'filterEventsDetected = repoEventsDetected.Filter.Where()
            Else
                str = "Insert into AlertSentDetails(SentTo,AlertCreatedDateTime,AlertHistoryID,AlertKey) values('" & SentMails & "','" & strdt & "'," & AlertID & "," & AlertKey.ToString() & ")"
            End If
            '12/16/2014 NS modified for VSPLUS-1267
            vsObj.ExecuteNonQueryAny("VitalSigns", "VitalSigns", str)
            'Dim Sqlcmd As New SqlCommand(str, con)
            'con.Open()
            'Sqlcmd.ExecuteNonQuery()
            If resent Then
                WriteServiceHistoryEntry(Now.ToString & " Updated record for " & AlertID & " , " & SentMails & " in the AlertSentDetails Table.", LogLevel.Normal)
            Else
                WriteServiceHistoryEntry(Now.ToString & " Inserted value " & AlertID & " , " & SentMails & " into  AlertSentDetails Table.", LogLevel.Normal)
            End If
            '12/16/2014 NS commented out for VSPLUS-1267
            'con.Close()

        Catch ex As ApplicationException
            WriteServiceHistoryEntry(Now.ToString & " Error occurred at the time of inserting value " & AlertID & " , " & SentMails & " into  AlertSentDetails Table " & ex.Message, LogLevel.Normal)
        End Try

    End Sub

    Private Sub UpdatingSentMails(ByVal AlertID As Integer, ByVal SentMails As String, ByVal id As String)

        Try
            Dim vsObj As New VSFramework.VSAdaptor
            '12/17/2014 NS added
            Dim objDateUtils As New DateUtils.DateUtils
            Dim strDateFormat As String
            Dim strdt As String
            strDateFormat = objDateUtils.GetDateFormat()
            strdt = objDateUtils.FixDateTime(Date.Now, strDateFormat)
            '12/17/2014 NS modified
            Dim str As String = "update AlertSentDetails set AlertClearedDateTime='" & strdt & "' where  ID=" & id
            '12/16/2014 NS modified for VSPLUS-1267
            vsObj.ExecuteNonQueryAny("VitalSigns", "VitalSigns", str)
            'Dim Sqlcmd As New SqlCommand(str, con)
            'con.Open()
            'Sqlcmd.ExecuteNonQuery()
            'con.Close()
            WriteServiceHistoryEntry(Now.ToString & " Updated the AlertSentDetails table with the current AlertClearedDateTime for " & id, LogLevel.Normal)
        Catch ex As ApplicationException
            WriteServiceHistoryEntry(Now.ToString & " Error occurred at the time of updating the row with id: " & id & ", alerthistoryid: " & AlertID & " , sentto: " & SentMails & " in the AlertSentDetails table: " & ex.Message, LogLevel.Normal)
        End Try

    End Sub
    '4/8/2015 NS added for VSPLUS-219
    Private Sub InsertSentEscalation(ByVal SentTo As String, ByVal AlertHistoryID As String, ByVal EscalationID As String)
        Try
            Dim vsObj As New VSFramework.VSAdaptor
            Dim str As String
            Dim objDateUtils As New DateUtils.DateUtils
            Dim strDateFormat As String
            Dim strdt As String
            strDateFormat = objDateUtils.GetDateFormat()
            strdt = objDateUtils.FixDateTime(Date.Now, strDateFormat)

            str = "INSERT INTO EscalationSentDetails(SentTo,EscalationCreatedDateTime,AlertHistoryID,EscalationID) " & _
             "VALUES ('" & SentTo & "','" & strdt & "'," & AlertHistoryID & "," & EscalationID & ")"
            vsObj.ExecuteNonQueryAny("VitalSigns", "VitalSigns", str)
            WriteServiceHistoryEntry(Now.ToString & " Inserted value " & AlertHistoryID & " , " & SentTo & " into EscalationSentDetails table.", LogLevel.Normal)
        Catch ex As ApplicationException
            WriteServiceHistoryEntry(Now.ToString & " Error occurred at the time of inserting value " & AlertHistoryID & " , " & SentTo & " into  EscalationSentDetails table " & ex.Message, LogLevel.Normal)
        End Try
    End Sub
    Private Sub SendSNMPTrapOLD(ByVal remoteHost As String, ByVal DeviceType As String, ByVal DeviceName As String, ByVal AlertType As String, ByVal Details As String)

        Try

            '***********************  SNMP MAIL  ****************************

            WriteServiceHistoryEntry(Now.ToString & " Sending SNMP Trap to " & remoteHost & " for " & DeviceName & " to " & AlertType, LogLevel.Normal)

            Dim trapOID As String
            SNMPAgent.Active = True
            trapOID = "1.3.6.1.4.1.26062.0.1"
            SNMPAgent.SNMPVersion = nsoftware.IPWorks.SnmpagentSNMPVersions.snmpverV2c
            SNMPAgent.ObjCount = 7


            Try
                SNMPAgent.ObjId(1) = "1.3.6.1.2.1.1.3.0"
                SNMPAgent.ObjType(1) = SnmpagentObjTypes.otTimeTicks   'string
                SNMPAgent.ObjValue(1) = SNMPAgent.SysUpTime.ToString
            Catch ex As Exception

            End Try

            Try
                SNMPAgent.ObjId(2) = "1.3.6.1.6.3.1.1.4.1.0"
                SNMPAgent.ObjType(2) = SnmpagentObjTypes.otObjectId
                SNMPAgent.ObjValue(2) = "1.3.6.1.4.1.26062.0.1"
            Catch ex As Exception

            End Try

            Try
                '** Details
                Dim bytesUTF8 As Byte() = System.Text.Encoding.UTF8.GetBytes(Details)
                SNMPAgent.ObjId(3) = "1.3.6.1.4.1.26062.0.1.1.0" 'message
                SNMPAgent.ObjType(3) = SnmpagentObjTypes.otOctetString   'string
                SNMPAgent.ObjValue(3) = Details  'message
                ' SNMPAgent.ObjValueB(3) = System.Text.Encoding.UTF8.GetBytes(Details)

            Catch ex As Exception

            End Try

            Try
                ' **** Status
                SNMPAgent.ObjId(4) = "1.3.6.1.4.1.26062.0.1.2.0"  'Status
                SNMPAgent.ObjType(4) = SnmpagentObjTypes.otOctetString
                SNMPAgent.ObjValue(4) = AlertType
            Catch ex As Exception

            End Try

            Try
                ' **** Device Type
                SNMPAgent.ObjId(5) = "1.3.6.1.4.1.26062.0.1.3.0"  'Device Type
                SNMPAgent.ObjType(5) = SnmpagentObjTypes.otOctetString
                SNMPAgent.ObjValue(5) = DeviceType
            Catch ex As Exception

            End Try

            Try
                ' **** Server Name
                SNMPAgent.ObjId(6) = "1.3.6.1.4.1.26062.0.1.4.0"  'serverName
                SNMPAgent.ObjType(6) = SnmpagentObjTypes.otOctetString
                ' SNMPAgent.ObjValueB(6) = System.Text.Encoding.Unicode.GetBytes(DeviceName)

                'SNMPAgent.ValueB(6) = System.Text.Encoding.Unicode.GetBytes("DeviceName")
                SNMPAgent.ObjValue(6) = DeviceName
            Catch ex As Exception

            End Try


            Try
                ' **** Date and Time
                SNMPAgent.ObjId(7) = "1.3.6.1.4.1.26062.0.1.5.0"  'Date Time
                SNMPAgent.ObjType(7) = SnmpagentObjTypes.otOctetString
                SNMPAgent.ObjValue(7) = Now.ToShortDateString & " at " & Now.ToShortTimeString

            Catch ex As Exception

            End Try

            Try
                SNMPAgent.SendTrap(remoteHost, "ignoredbecausecustomobjectsspecified")
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " Error sending SNMP Trap " & ex.ToString, LogLevel.Normal)
            End Try

            Try
                SNMPAgent.Active = False
            Catch ex As Exception

            End Try
        Catch ex As ApplicationException

        End Try

    End Sub
    Private Sub SendSNMPTrap(ByVal remoteHost As String, ByVal DeviceType As String, ByVal DeviceName As String, ByVal AlertType As String, ByVal Details As String)
        Dim host As String = "localhost"
        Dim community As String = "public"
        Dim agent As TrapAgent = New TrapAgent()

        Dim col As VbCollection = New VbCollection()

        Try

            '***********************  SNMP MAIL  ****************************

            WriteServiceHistoryEntry(Now.ToString & " Sending SNMP Trap to " & remoteHost & " for " & DeviceName & " to " & AlertType, LogLevel.Normal)

            Dim trapOID As String
            trapOID = "1.3.6.1.4.1.26062.0.1"
            Dim timeTickVal As UInt32 = 2324

            col.Add(New Oid("1.3.6.1.2.1.1.3.0"), New TimeTicks(timeTickVal))
            col.Add(New Oid("1.3.6.1.6.3.1.1.4.1.0"), New Oid(trapOID))
            col.Add(New Oid("1.3.6.1.4.1.26062.0.1.1.0"), New OctetString(Details))
            col.Add(New Oid("1.3.6.1.4.1.26062.0.1.2.0"), New OctetString(AlertType))
            col.Add(New Oid("1.3.6.1.4.1.26062.0.1.3.0"), New OctetString(DeviceType))
            col.Add(New Oid("1.3.6.1.4.1.26062.0.1.4.0"), New OctetString(DeviceName))
            col.Add(New Oid("1.3.6.1.4.1.26062.0.1.5.0"), New OctetString(Now.ToShortDateString & " at " & Now.ToShortTimeString))

            Try
                agent.SendV1Trap(New SnmpSharpNet.IpAddress(remoteHost), 162, "public", _
                    New Oid(trapOID), New SnmpSharpNet.IpAddress("127.0.0.1"), _
                    SnmpConstants.LinkUp, 0, 13432, col)
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " Error sending SNMP Trap " & ex.ToString, LogLevel.Normal)
            End Try

        Catch ex As ApplicationException
            WriteServiceHistoryEntry(Now.ToString & " In SendSNMPTrap: Error - " & ex.Message, LogLevel.Normal)
        End Try

    End Sub
    Private Sub ProcessAlertsSendNotification()
        Dim ADef As New AlertDefinition
        Dim AHist As New AlertDefinition
        Dim ADefOut As New AlertDefinition
        Dim keyList As New List(Of Integer)
        Dim keyArr As Integer()
        Dim ADefArr As AlertDefinition()
        Dim ADefArrOut As AlertDefinition()
        Dim AHistArr As AlertDefinition()
        Dim c As Integer
        Dim ds As New DataSet
        Dim param As New SqlParameter
        Dim retVal As Integer
        Dim da As SqlDataAdapter
        Dim sqlStr As String
        Dim con As New SqlConnection
        Dim myConnectionString As New VSFramework.XMLOperation
        Dim myAdapter As New VSFramework.VSAdaptor

        Dim connString As String = GetDBConnection()
        Dim repoEventsMaster As New Repository(Of EventsMaster)(connString)
        Dim repoNotifications As New Repository(Of Notifications)(connString)
        Dim repoServers As New Repository(Of Server)(connString)
        Dim repoBusHrs As New Repository(Of BusinessHours)(connString)
        Dim repoEventsDetected As New Repository(Of EventsDetected)(connString)

        Dim filterNotifications As MongoDB.Driver.FilterDefinition(Of Notifications)
        Dim filterEvents As MongoDB.Driver.FilterDefinition(Of EventsMaster)
        Dim filterServers As MongoDB.Driver.FilterDefinition(Of Server)
        Dim filterBusHrs As MongoDB.Driver.FilterDefinition(Of BusinessHours)
        Dim filterEventsDetected As MongoDB.Driver.FilterDefinition(Of EventsDetected)
        Dim updateEventsDetected As MongoDB.Driver.UpdateDefinition(Of EventsDetected)
        Dim eventsEntity() As EventsMaster
        Dim serversEntity() As Server
        Dim bushrsEntity() As BusinessHours
        Dim sendlist As List(Of SendList)
        Dim notificationsEntity() As Notifications
        Dim eventsCreated() As EventsDetected
        Dim notificationsSent() As NotificationsSent

        Dim dt As New DataTable
        Dim dr As DataRow
        Dim oid As ObjectId

        Try
            con.ConnectionString = myConnectionString.GetDBConnectionString("VitalSigns")
            con.Open()

            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - established SQL connection", LogLevel.Verbose)
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: could not establish SQL connection - " & ex.Message, LogLevel.Normal)
        End Try

        dt.Columns.Add("AlertKey")
        dt.Columns.Add("EventName")
        dt.Columns.Add("ServerType")
        dt.Columns.Add("ServerName")
        dt.Columns.Add("SendTo")
        dt.Columns.Add("CopyTo")
        dt.Columns.Add("BlindCopyTo")
        dt.Columns.Add("StartTime")
        dt.Columns.Add("Duration")
        dt.Columns.Add("Day")
        dt.Columns.Add("HoursIndicator")
        dt.Columns.Add("SendSNMPTrap")
        dt.Columns.Add("EnablePersistentAlert")
        dt.Columns.Add("SMSTo")
        dt.Columns.Add("ScriptName")
        dt.Columns.Add("ScriptCommand")
        dt.Columns.Add("ScriptLocation")

        filterNotifications = repoNotifications.Filter.Exists(Function(j) j.NotificationName, True)
        notificationsEntity = repoNotifications.Find(filterNotifications).ToArray()
        If notificationsEntity.Length > 0 Then
            For i As Integer = 0 To notificationsEntity.Length - 1
                Oid = notificationsEntity(i).ObjectId
                filterEvents = repoEventsMaster.Filter.And(repoEventsMaster.Filter.Exists(Function(j) j.NotificationList, True),
                        repoEventsMaster.Filter.ElemMatch(Of NotificationList)(Function(j) j.NotificationList, Function(j) j.NotificationId = Oid))
                eventsEntity = repoEventsMaster.Find(filterEvents).ToArray()

                filterServers = repoServers.Filter.And(repoServers.Filter.Exists(Function(j) j.NotificationsList, True),
                        repoServers.Filter.ElemMatch(Of NotificationsList)(Function(j) j.NotificationsList, Function(j) j.NotificationId = Oid))
                serversEntity = repoServers.Find(filterServers).ToArray()

                If eventsEntity.Length > 0 And serversEntity.Length > 0 Then
                    sendlist = notificationsEntity(i).SendList
                    For k As Integer = 0 To sendlist.Count - 1
                        For x As Integer = 0 To eventsEntity.Length - 1
                            For y As Integer = 0 To serversEntity.Length - 1
                                dr = dt.NewRow()
                                dr("AlertKey") = notificationsEntity(i).Id
                                dr("EventName") = eventsEntity(x).EventType
                                dr("ServerType") = serversEntity(y).ServerType
                                dr("ServerName") = serversEntity(y).ServerName
                                dr("CopyTo") = sendlist(k).CopyTo
                                dr("BlindCopyTo") = sendlist(k).BlindCopyTo
                                dr("SendTo") = ""
                                dr("SMSTo") = ""
                                dr("ScriptName") = ""
                                dr("ScriptCommand") = ""
                                dr("ScriptLocation") = ""
                                dr("SendSNMPTrap") = False
                                dr("EnablePersistentAlert") = sendlist(k).PersistentAlert
                                If sendlist(k).SendVia = "email" Then
                                    dr("SendTo") = sendlist(k).SendTo
                                ElseIf sendlist(k).SendVia = "sms" Then
                                    dr("SMSTo") = sendlist(k).SendTo
                                ElseIf sendlist(k).SendVia = "script" Then
                                    dr("ScriptName") = sendlist(k).SendTo
                                    dr("ScriptCommand") = sendlist(k).ScriptCommand
                                    dr("ScriptLocation") = sendlist(k).ScriptLocation
                                ElseIf sendlist(k).SendVia = "snmptrap" Then
                                    dr("SendSNMPTrap") = True
                                End If
                                dr("HoursIndicator") = sendlist(k).BusinessHoursId.ToString()
                                dr("StartTime") = ""
                                dr("Duration") = 0
                                dr("Day") = ""
                                filterBusHrs = repoBusHrs.Filter.Eq(Of ObjectId)("_id", sendlist(k).BusinessHoursId)
                                bushrsEntity = repoBusHrs.Find(filterBusHrs).ToArray()
                                If bushrsEntity.Length > 0 Then
                                    dr("StartTime") = bushrsEntity(0).StartTime
                                    dr("Duration") = bushrsEntity(0).Duration
                                    dr("Day") = String.Join(",", bushrsEntity(0).Days)
                                End If
                                dt.Rows.Add(dr)
                            Next
                        Next
                    Next
                End If
            Next
        End If

        If (dt.Rows.Count > 0) Then
            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - found records from GetAlertsForSelectedEventsServers", LogLevel.Verbose)
        Else
            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - did NOT find records from GetAlertsForSelectedEventsServers", LogLevel.Verbose)
        End If

        ReDim ADefArr(0)
        ReDim keyArr(0)
        keyArr(0) = 0
        c = 0
        '1.a. Add alert definition objects into an array
        If Not IsNothing(dt) Then
            Try
                If dt.Rows.Count > 0 Then
                    For i As Integer = 0 To dt.Rows.Count - 1
                        ADef = New AlertDefinition
                        ADef.AlertKey = dt.Rows(i)("AlertKey")
                        ADef.EventName = dt.Rows(i)("EventName")
                        ADef.ServerType = dt.Rows(i)("ServerType")
                        ADef.ServerName = dt.Rows(i)("ServerName")
                        ADef.SendTo = dt.Rows(i)("SendTo")
                        ADef.CopyTo = dt.Rows(i)("CopyTo")
                        ADef.BlindCopyTo = dt.Rows(i)("BlindCopyTo")
                        ADef.StartTime = dt.Rows(i)("StartTime")
                        ADef.Duration = dt.Rows(i)("Duration")
                        ADef.Days = dt.Rows(i)("Day")
                        ADef.IntType = dt.Rows(i)("HoursIndicator")
                        ADef.Details = ""
                        ADef.SendSNMPTrap = Convert.ToBoolean(dt.Rows(i)("SendSNMPTrap"))
                        ADef.EnablePersistentAlert = Convert.ToBoolean(dt.Rows(i)("EnablePersistentAlert"))
                        ADef.DateCreated = ""
                        ADef.SMSTo = dt.Rows(i)("SMSTo")
                        ADef.ScriptName = dt.Rows(i)("ScriptName")
                        ADef.ScriptCommand = dt.Rows(i)("ScriptCommand")
                        ADef.ScriptLocation = dt.Rows(i)("ScriptLocation")
                        ReDim Preserve keyArr(c)
                        keyArr(c) = ADef.AlertKey
                        ReDim Preserve ADefArr(c)
                        ADefArr(c) = ADef
                        c = c + 1
                    Next
                End If
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error getting a handle on the data table: " & ex.Message, LogLevel.Normal)
            End Try

        End If

        'WriteServiceHistoryEntry(Now.ToString & " After for loop.")
        'WriteServiceHistoryEntry(Now.ToString & " Alert Array: " & ADefArr.Length & " " & ADefArr(0).EventName)
        If Not IsNothing(ds) Then
            ds.Clear()
        End If

        Try
            'WriteServiceHistoryEntry(Now.ToString & " Here #4")
            If Not ADefArr(0) Is Nothing Then
                If ADefArr(0).AlertKey.ToString <> "" Then
                    c = 0
                    ReDim ADefArrOut(0)
                    Dim ADefSort As New AlertDefinition()
                    Array.Sort(ADefArr, ADefSort)
                    For i As Integer = 0 To ADefArr.Length - 1
                        'WriteServiceHistoryEntry(Now.ToString & " Alert Key: " & ADefArr(i).AlertKey)
                        If keyList.Contains(ADefArr(i).AlertKey) Then
                            'WriteServiceHistoryEntry(Now.ToString & " keyList CONTAINS key, c is " & c)
                            ReDim Preserve ADefArrOut(c)
                            ADefArrOut(c) = ADefArr(i)
                            c = c + 1
                        Else
                            'WriteServiceHistoryEntry(Now.ToString & " keyList DOES NOT contain key, c is " & c)
                            If c > 0 Then
                                If ADefArrOut(c - 1).AlertKey.ToString <> "" Then
                                    AlertDict.Add(ADefArrOut(c - 1).AlertKey, ADefArrOut)
                                    'WriteServiceHistoryEntry(Now.ToString & " added to dict " & ADefArrOut(c - 1).AlertKey)
                                End If
                            End If
                            keyList.Add(ADefArr(i).AlertKey)
                            c = 0
                            ReDim ADefArrOut(c)
                            ADefArrOut(c) = ADefArr(i)
                            c = c + 1
                        End If
                    Next
                    If ADefArrOut(c - 1).AlertKey.ToString <> "" Then
                        AlertDict.Add(ADefArrOut(c - 1).AlertKey, ADefArrOut)
                        'WriteServiceHistoryEntry(Now.ToString & " added to dict " & ADefArrOut(c - 1).AlertKey)
                    End If
                End If
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: " & ex.Message, LogLevel.Normal)
        End Try
        If Not IsNothing(ds) Then
            ds.Clear()
        End If

        filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(i) i.NotificationsSent, False),
            repoEventsDetected.Filter.Exists(Function(i) i.EventDismissed, False))
        eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToArray()
        If eventsCreated.Length > 0 Then
            dt = New DataTable
            dt.Columns.Add("ID")
            dt.Columns.Add("AlertType")
            dt.Columns.Add("DeviceType")
            dt.Columns.Add("DeviceName")
            dt.Columns.Add("Details")
            dt.Columns.Add("DateTimeOfAlert")

            For i As Integer = 0 To eventsCreated.Length - 1
                dr = dt.NewRow()
                dr("ID") = eventsCreated(i).Id
                dr("AlertType") = eventsCreated(i).EventType
                dr("DeviceType") = eventsCreated(i).DeviceType
                dr("DeviceName") = eventsCreated(i).Device
                dr("Details") = eventsCreated(i).Details
                dr("DateTimeOfAlert") = eventsCreated(i).EventDetected
                dt.Rows.Add(dr)
            Next
        End If

        '1/24/2014 NS added conditional print, otherwise the log fills out too quickly
        If (dt.Rows.Count > 0) Then
            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - did NOT find records from GetAlertHistory", LogLevel.Verbose)
        Else
            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - did NOT find records from GetAlertHistory", LogLevel.Verbose)
        End If

        c = 0
        ReDim AHistArr(0)
        Try
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    AHist = New AlertDefinition
                    ReDim Preserve AHistArr(c)
                    AHistArr(c) = AHist
                    AHist.AlertKey = dt.Rows(i)("ID")
                    AHist.EventName = dt.Rows(i)("AlertType")
                    AHist.ServerType = dt.Rows(i)("DeviceType")
                    AHist.ServerName = dt.Rows(i)("DeviceName")
                    AHist.StartTime = ""
                    AHist.Details = dt.Rows(i)("Details")
                    AHist.SendSNMPTrap = False
                    AHist.EnablePersistentAlert = False
                    AHist.DateCreated = dt.Rows(i)("DateTimeOfAlert")
                    c = c + 1
                Next
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error processing the rows from the GetAlertHistory stored procedure." & ex.Message, LogLevel.Normal)
        End Try

        ReDim ADefArrOut(0)
        c = 0
        '6. Go through the alert records in the dictionary object and try to match the records with those from the AlertHistory table.
        'Where there is a match, add the record into the alerts array which will then be used to send notifications.
        Try
            If Not IsNothing(AlertDict) And Not IsNothing(AHistArr) Then
                For Each pair In AlertDict
                    If Not IsNothing(pair) Then
                        For i As Integer = 0 To pair.Value.Length - 1
                            ADef = pair.Value(i)
                            For j As Integer = 0 To AHistArr.Length - 1
                                AHist = AHistArr(j)
                                If Not IsNothing(AHist) Then
                                    WriteServiceHistoryEntry(Now.ToString & " COMPARE: *" & AHist.EventName & "* - *" & ADef.EventName & "*, *" & AHist.ServerName & "* - *" & ADef.ServerName & "*, *" & AHist.ServerType & "* - *" & ADef.ServerType, LogLevel.Verbose)
                                    If (InStr(AHist.EventName, ADef.EventName) > 0 And ADef.ServerName = AHist.ServerName And ADef.ServerType = AHist.ServerType) Or
                                    (InStr(AHist.EventName, ADef.EventName) > 0 And ADef.ServerType = AHist.ServerType And ADef.ServerName = "") Then
                                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - found a match", LogLevel.Verbose)
                                        ADefOut = New AlertDefinition
                                        ADefOut.AlertHistoryId = AHist.AlertKey
                                        ADefOut.AlertKey = ADef.AlertKey
                                        ADefOut.EventName = AHist.EventName
                                        ADefOut.ServerType = ADef.ServerType
                                        ADefOut.ServerName = AHist.ServerName
                                        ADefOut.SendTo = ADef.SendTo
                                        ADefOut.CopyTo = ADef.CopyTo
                                        ADefOut.BlindCopyTo = ADef.BlindCopyTo
                                        ADefOut.StartTime = ADef.StartTime
                                        ADefOut.Duration = ADef.Duration
                                        ADefOut.Days = ADef.Days
                                        ADefOut.IntType = ADef.IntType
                                        ADefOut.Details = AHist.Details
                                        ADefOut.SendSNMPTrap = ADef.SendSNMPTrap
                                        ADefOut.EnablePersistentAlert = ADef.EnablePersistentAlert
                                        ADefOut.DateCreated = AHist.DateCreated
                                        ADefOut.SMSTo = ADef.SMSTo
                                        ADefOut.ScriptName = ADef.ScriptName
                                        ADefOut.ScriptCommand = ADef.ScriptCommand
                                        ADefOut.ScriptLocation = ADef.ScriptLocation
                                        ReDim Preserve ADefArrOut(c)
                                        ADefArrOut(c) = ADefOut
                                        c = c + 1
                                    End If
                                End If
                            Next
                        Next
                    End If
                Next
            End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error processing the alert distionary data set: " & ex.Message, LogLevel.Normal)
        End Try

        If Not IsNothing(ds) Then
            ds.Clear()
        End If

        '7. Process alerts array records using the IsDateInSpecificHours stored procedure to identify whether any of them
        'fall within the current time frame for sending. The stored procedure checks business hours, off hours, and specific hours
        'qualifications and returns 1 if the alert is current and 0 if it's not.
        Dim isSpecific As Boolean = False
        Dim isDayIncluded As Boolean = False
        Dim startdt As DateTime
        If Not ADefArrOut(0) Is Nothing Then
            For j As Integer = 0 To ADefArrOut.Length - 1
                ADefOut = New AlertDefinition
                ADefOut = ADefArrOut(j)
                startdt = DateTime.Parse(ADefOut.StartTime)
                If ADefOut.IntType <> 3 Then
                    isSpecific = IIf(Now.ToShortTimeString >= startdt.ToShortTimeString And Now.ToShortTimeString <= startdt.AddMinutes(ADefOut.Duration).ToShortTimeString, True, False)
                    isDayIncluded = IIf(ADefOut.Days.IndexOf(Now.DayOfWeek.ToString()) <> -1, True, False)
                    retVal = IIf(isSpecific And isDayIncluded, 1, 0)
                Else
                    retVal = 1
                End If
                ADefOut.RunNow = retVal
                ADefArrOut(j) = ADefOut
            Next
        End If

        'check if max alerts are sent today and see how may alerts per def are sent
        'WriteServiceHistoryEntry(Now.ToString & " Here #8")
        '8. Check the AlertSentDetails table and send an e-mail notification if there is no entry for each AlertHistoryID value
        Dim SendTo As String
        Dim CC As String
        Dim BCC As String
        Dim mailsent As Boolean
        Dim SMSTo As String
        Dim smssent As Boolean
        Dim ScriptName As String
        Dim ScriptCommand As String
        Dim ScriptLocation As String
        Dim scriptsent As Boolean
        Dim sSource As String
        Dim sLog As String
        Dim sEvent As String
        Dim dontNeedToSend As Boolean
        Dim resend As Boolean
        Dim persistentInterval As Integer
        Dim persistentDuration As Integer
        Dim alertDateCurrent As Date
        Dim alertDateCreated As Date
        Dim noNewRecipients As Boolean

        SendTo = ""
        CC = ""
        BCC = ""
        SMSTo = ""
        ScriptName = ""
        ScriptCommand = ""
        ScriptLocation = ""
        persistentInterval = 0
        Try
            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to get PersistentAlertInterval from Settings", LogLevel.Verbose)
            persistentInterval = Convert.ToInt32(getSettings("PersistentAlertInterval"))
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error getting Persistent Alert Interval from the Settings table:  " & ex.ToString, LogLevel.Normal)
        End Try
        persistentDuration = 0
        Try
            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to get PersistentAlertDuration from Settings", LogLevel.Verbose)
            persistentDuration = Convert.ToInt32(getSettings("PersistentAlertDuration"))
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error getting Persistent Alert Duration from the Settings table:  " & ex.ToString, LogLevel.Normal)
        End Try
        Dim maxAllowedTodayCount = 0
        'get the max allowed count here, by doing the computation
        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to GetMaxAlertsRemainingToday", LogLevel.Verbose)
        Dim TotalMaxAlertsPerDay = GetMaxAlertsRemainingToday()
        If TotalMaxAlertsPerDay >= 0 Then
            'trim the alerts collection
            If ADefArrOut.Length > TotalMaxAlertsPerDay Then
                maxAllowedTodayCount = TotalMaxAlertsPerDay - 1
            Else
                maxAllowedTodayCount = ADefArrOut.Length - 1
            End If
        Else
            maxAllowedTodayCount = ADefArrOut.Length - 1
        End If
        'get the alertkey by matching the history id with alertkey
        If Not ADefArrOut(0) Is Nothing Then
            For i = 0 To maxAllowedTodayCount
                'now check the max alerts per def here
                ADef = ADefArrOut(i)
                noNewRecipients = False
                WriteServiceHistoryEntry(Now.ToString & "ADefOut " & i.ToString() & ": " & ADef.AlertKey & ", " & ADef.AlertHistoryId & ", " & ADef.SendTo & ", " & ADef.SMSTo)
                'get max alerts remaining to be sent today for this def
                Dim MaxAlertsperDef = GetMaxAlertsRemainingPerDefToday(ADef.AlertKey)
                'if max is not reached or max alerts per def setting is not set, then send, else bail out
                If MaxAlertsperDef = -1 Or MaxAlertsperDef > 0 Then
                    dontNeedToSend = False
                    resend = False
                    Try
                        Dim dtmail As DataTable = New DataTable
                        dtmail.Columns.Add("SentTo")
                        dtmail.Columns.Add("AlertClearedDateTime")
                        dtmail.Columns.Add("AlertCreatedDateTime")
                        filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True),
                                      repoEventsDetected.Filter.Eq(Of String)(Function(j) j.Id, ADef.AlertHistoryId))
                        eventsCreated = repoEventsDetected.Find(filterEventsDetected).ToArray()
                        If eventsCreated.Length > 0 Then
                            For j As Integer = 0 To eventsCreated.Length - 1
                                notificationsSent = eventsCreated(j).NotificationsSent.ToArray()
                                For x As Integer = 0 To notificationsSent.Length - 1
                                    dr = dtmail.NewRow()
                                    dr("SentTo") = notificationsSent(x).NotificationSentTo
                                    dr("AlertClearedDateTime") = notificationsSent(x).EventDismissedSent
                                    dr("AlertCreatedDateTime") = notificationsSent(x).EventDetectedSent
                                    dtmail.Rows.Add(dr)
                                Next
                            Next
                        End If

                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to get records from events_detected ", LogLevel.Verbose)
                        If dtmail.Rows.Count > 0 Then
                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - found records in events_detected", LogLevel.Verbose)
                            '8.1. Found rows for alerts sent for the current AlertKey, now need to check if the current recipient is
                            'the same one as the record in the AlertSentDetails table
                            Dim foundRowsMail() As DataRow
                            If ADef.SendTo <> "" Then
                                foundRowsMail = dtmail.Select("SentTo like '%" & ADef.SendTo & "%'")
                            ElseIf ADef.SMSTo <> "" Then
                                foundRowsMail = dtmail.Select("SentTo like '%" & ADef.SMSTo & "%'")
                            ElseIf ADef.SendSNMPTrap <> "False" Then
                                foundRowsMail = dtmail.Select("SentTo like '%SNMP Trap%'")
                            Else
                                foundRowsMail = dtmail.Select("SentTo like '%Windows Log%'")
                            End If
                            If foundRowsMail.Length > 0 Then
                                '8.1.1 Check if persistent alerting is enabled. Notify again only if the time interval between the previous
                                'send and the current time is greater than or equal to the Settings value.
                                If ADef.SendTo <> "" Or ADef.SMSTo <> "" Or ADef.ScriptName <> "" Then
                                    If ADef.EnablePersistentAlert = True And persistentInterval > 0 Then
                                        alertDateCurrent = Convert.ToDateTime(foundRowsMail(0)("AlertCreatedDateTime").ToString())
                                        alertDateCurrent = alertDateCurrent.AddMinutes(persistentInterval)
                                        '8.1.2 If persistent alerting is unlimited, we don't care about the original alert creation date
                                        If persistentDuration = 0 Then
                                            If Now < alertDateCurrent Then
                                                dontNeedToSend = True
                                            Else
                                                resend = True
                                            End If
                                            '8.1.3 If persistent alerting is limited, we need to see if we are still within the bounds of the time limit
                                        Else
                                            alertDateCreated = Convert.ToDateTime(ADef.DateCreated)
                                            alertDateCreated = alertDateCreated.AddHours(persistentDuration)
                                            If Now > alertDateCreated Or Now < alertDateCurrent Then
                                                dontNeedToSend = True
                                            Else
                                                resend = True
                                            End If
                                        End If
                                    End If
                                    If ADef.EnablePersistentAlert = False Or dontNeedToSend = True Then
                                        noNewRecipients = True
                                        SendTo = ""
                                        CC = ""
                                        BCC = ""
                                        SMSTo = ""
                                        ScriptName = ""
                                        ScriptCommand = ""
                                        ScriptLocation = ""
                                    Else
                                        SendTo = ADef.SendTo
                                        CC = ADef.CopyTo
                                        BCC = ADef.BlindCopyTo
                                        SMSTo = ADef.SMSTo
                                        ScriptName = ADef.ScriptName
                                        ScriptCommand = ADef.ScriptCommand
                                        ScriptLocation = ADef.ScriptLocation
                                    End If
                                Else
                                    noNewRecipients = True
                                    SendTo = ""
                                    CC = ""
                                    BCC = ""
                                    SMSTo = ""
                                    ScriptName = ""
                                    ScriptCommand = ""
                                    ScriptLocation = ""
                                End If
                            Else
                                'If there are records for the current AlertKey but the recipient is different, need to assign
                                'the Send values to the current recipient
                                SendTo = ADef.SendTo
                                CC = ADef.CopyTo
                                BCC = ADef.BlindCopyTo
                                SMSTo = ADef.SMSTo
                                ScriptName = ADef.ScriptName
                                ScriptCommand = ADef.ScriptCommand
                                ScriptLocation = ADef.ScriptLocation
                            End If
                        Else
                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - found NO records in events_detected", LogLevel.Verbose)
                            Try
                                SendTo = ADef.SendTo
                                If ADef.CopyTo <> "" Then
                                    CC = ADef.CopyTo
                                End If
                                If ADef.BlindCopyTo <> "" Then
                                    BCC = ADef.BlindCopyTo
                                End If
                                SMSTo = ADef.SMSTo
                                ScriptName = ADef.ScriptName
                                ScriptCommand = ADef.ScriptCommand
                                ScriptLocation = ADef.ScriptLocation
                            Catch ex As Exception
                                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error processing SendTo/CC/BCC info: " & ex.Message, LogLevel.Normal)
                            End Try
                        End If
                    Catch ex As ApplicationException
                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error occurred at the time of getting records from AlertSentDetails " & ex.Message, LogLevel.Normal)
                    End Try
                    mailsent = False
                    smssent = False
                    scriptsent = False
                    If noNewRecipients Then
                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - found NO new recipients", LogLevel.Verbose)
                    Else
                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - found new recipients", LogLevel.Verbose)
                        Try
                            If ADef.RunNow = 1 Then
                                If SendTo <> "" Then
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - attempting to send new alert via email", LogLevel.Verbose)
                                    WriteServiceHistoryEntry(Now.ToString & " Attempting to send a new alert via e-mail:", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & "   SendTo = " & SendTo & ",   ServerName = " & ADef.ServerName & ",    AlertKey = " & ADef.AlertKey, LogLevel.Normal)
                                    If (ADef.EventName.ToString = "Services") Then
                                        Dim newString As String = ADef.Details.Substring(ADef.Details.IndexOf(" ") + 1)
                                        Dim newStrings As String() = ADef.Details.Split(New String() {" "}, StringSplitOptions.None)
                                        Dim services As String
                                        services = newStrings(0).ToString
                                        Dim substringofservices As String() = services.Split(",")
                                        ADef.Details = " " & vbCrLf & newString & vbCrLf & "The services are "
                                        If (substringofservices.Length = 1) Then
                                            ADef.Details = " " & vbCrLf & "The " & newString
                                        Else
                                            For Each substring In substringofservices
                                                ADef.Details &= vbCrLf & substring
                                            Next
                                            Dim s As String = ADef.Details
                                        End If
                                    Else
                                        ADef.Details = "" & vbCrLf & ADef.Details
                                    End If
                                    SendMailwithChilkatorNet(SendTo, CC, BCC, ADef.ServerName, ADef.ServerType, "", ADef.EventName, ADef.EventName, ADef.Details, "", "Alert")
                                    mailsent = True
                                ElseIf ADef.SendSNMPTrap <> "False" Then
                                    '***** SNMP Conditions *********
                                    Try
                                        Dim SNMPHostName As String = getSettings("SNMPHostName")
                                        If ADef.SendSNMPTrap = "True" And SNMPHostName <> "" Then
                                            SendSNMPTrap(SNMPHostName, ADef.ServerType, ADef.ServerName, ADef.EventName, ADef.Details)
                                        End If
                                        SendTo = "SNMP Trap"
                                        mailsent = True
                                    Catch ex As ApplicationException
                                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error occurred at SendSNMPTrap " & ex.Message, LogLevel.Normal)
                                    End Try
                                Else
                                    sSource = "VitalSigns Plus"
                                    sLog = "Application"
                                    sEvent = "Alert for " & ADef.ServerType & " " & ADef.ServerName & " - " & ADef.EventName & ". " & ADef.Details
                                    Try
                                        If Not EventLog.SourceExists(sSource) Then
                                            EventLog.CreateEventSource(sSource, sLog)
                                        End If

                                        EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234)
                                    Catch ex As Exception
                                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error writing to Windows log for alert ID " & ADef.AlertKey & ": " & ex.Message, LogLevel.Normal)
                                    End Try
                                    SendTo = "Windows Log"
                                    mailsent = True
                                End If
                            End If
                        Catch ex As ApplicationException
                            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error occurred at the time of sending an Alert Mail " & ex.Message, LogLevel.Normal)
                            mailsent = False
                        End Try
                        Try
                            If ADef.RunNow = 1 Then
                                If SMSTo <> "" Then
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - attempting to send new alert via SMS", LogLevel.Verbose)
                                    WriteServiceHistoryEntry(Now.ToString & " Attempting to send a new alert via SMS:", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & "   SMSTo = " & SMSTo & ",   ServerName = " & ADef.ServerName & ",    AlertKey = " & ADef.AlertKey, LogLevel.Normal)
                                    SendSMSwithTwilio(SMSTo, ADef.ServerName, ADef.ServerType, ADef.EventName, ADef.Details, "", "")
                                    smssent = True
                                End If
                            End If
                        Catch ex As ApplicationException
                            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error occurred at the time of sending an Alert SMS " & ex.Message, LogLevel.Normal)
                            smssent = False
                        End Try
                        Try
                            If ADef.RunNow = 1 Then
                                If ScriptName <> "" Then
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - attempting to send new alert via Script", LogLevel.Verbose)
                                    WriteServiceHistoryEntry(Now.ToString & " Attempting to send a new alert via Script:", LogLevel.Normal)
                                    WriteServiceHistoryEntry(Now.ToString & "   ScriptName = " & ScriptName & ",   ServerName = " & ADef.ServerName & ",    AlertKey = " & ADef.AlertKey, LogLevel.Normal)
                                    SendScript(ScriptName, ScriptCommand, ScriptLocation, ADef.ServerName, ADef.ServerType, ADef.EventName, ADef.Details, "")
                                    scriptsent = True
                                End If
                            End If
                        Catch ex As ApplicationException
                            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error occurred at the time of sending an Alert SMS " & ex.Message, LogLevel.Normal)
                            scriptsent = False
                        End Try
                    End If
                    Try
                        If mailsent = True Then
                            Dim mails As String = ""
                            mails = SendTo
                            If CC <> "" Then
                                mails = mails & "," & CC
                            End If
                            If BCC <> "" Then
                                mails = mails & "," & BCC
                            End If
                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to insert sent email information", LogLevel.Verbose)
                            'NEED TO CONVERT InsertingSentMails
                            InsertingSentMails(ADef.AlertHistoryId, mails, resend, ADef.AlertKey)
                            If (ADef.Details = "This is a TEST alert.") Or InStr(ADef.EventName, "Log File") Then
                                If (ADef.Details = "This is a TEST alert.") Then
                                    WriteServiceHistoryEntry(Now.ToString & " This is a TEST alert and will be cleared instantly", LogLevel.Normal)
                                Else
                                    WriteServiceHistoryEntry(Now.ToString & " This is a Log File alert and will be cleared instantly", LogLevel.Normal)
                                End If
                                Dim str As String = ""
                                Dim str2 As String = ""
                                Dim objDateUtils As New DateUtils.DateUtils
                                Dim strDateFormat As String
                                Dim strdt As String
                                strDateFormat = objDateUtils.GetDateFormat()
                                strdt = objDateUtils.FixDateTime(Date.Now, strDateFormat)
                                Try
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to update event_dismissed in events_detected", LogLevel.Verbose)
                                    filterEventsDetected = repoEventsDetected.Filter.Eq(Of String)(Function(j) j.Id, ADef.AlertHistoryId)
                                    updateEventsDetected = repoEventsDetected.Updater.Set(Of DateTime)(Function(j) j.EventDismissed, strdt)
                                    repoEventsDetected.Update(filterEventsDetected, updateEventsDetected)

                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to update event_dismissed_sent in events_detected.notifications_sent", LogLevel.Verbose)
                                    filterEventsDetected = repoEventsDetected.Filter.And(repoEventsDetected.Filter.Exists(Function(j) j.NotificationsSent, True),
                                                 repoEventsDetected.Filter.Eq(Of String)(Function(j) j.Id, ADef.AlertHistoryId))
                                    updateEventsDetected = repoEventsDetected.Updater.Set(Of DateTime)(Function(j) j.NotificationsSent.ElementAt(-1).EventDismissedSent, strdt)
                                    repoEventsDetected.Update(filterEventsDetected, updateEventsDetected)

                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to update sent email info", LogLevel.Verbose)
                                    UpdatingSentMails(ADef.AlertHistoryId, mails, ADef.AlertKey)
                                Catch ex As Exception
                                    WriteServiceHistoryEntry(Now.ToString & " Error occurred at the time of updating the events_detected for the Alert with ID of " & ADef.AlertHistoryId & ": " & ex.Message, LogLevel.Normal)
                                End Try
                            End If

                        End If
                    Catch ex As Exception
                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error while attempting to insert Alert Mail history for emails " & ex.ToString, LogLevel.Normal)
                    End Try
                    Try
                        If smssent = True Then
                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to insert sent SMS info", LogLevel.Verbose)
                            InsertingSentMails(ADef.AlertHistoryId, SMSTo, resend, ADef.AlertKey)
                        End If
                    Catch ex As Exception
                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error while attempting to insert Alert Mail history for SMS " & ex.ToString, LogLevel.Normal)
                    End Try
                    Try
                        If scriptsent = True Then
                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to insert sent Script info", LogLevel.Verbose)
                            InsertingSentMails(ADef.AlertHistoryId, ScriptName, resend, ADef.AlertKey)
                        End If
                    Catch ex As Exception
                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error while attempting to insert Alert Mail history for Script " & ex.ToString, LogLevel.Normal)
                    End Try
                Else
                    WriteServiceHistoryEntry(Now.ToString & " Max Alerts for this def reached: No more alerts will be sent", LogLevel.Normal)
                End If
            Next
            'ESCALATION
            Try
                Dim dt2 As New DataTable
                Dim dt3 As New DataTable
                Dim alertcreated As DateTime
                Dim esmssent As Boolean = False
                Dim eemailsent As Boolean = False
                sqlStr = "SELECT t1.[ID],t1.[AlertKey],[EscalateTo],[EscalationInterval],t1.[SMSTo],t1.[ScriptID] " &
                 "FROM EscalationDetails t1 INNER JOIN AlertDetails t2 ON t1.AlertKey=t2.AlertKey " &
                 "GROUP BY t1.ID,t1.AlertKey,EscalateTo,EscalationInterval,t1.SMSTo,t1.ScriptID " &
                 "ORDER BY EscalationInterval"
                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to get records from EscalationDetails,AlertDetails", LogLevel.Verbose)
                dt = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sqlStr)
                If dt.Rows.Count > 0 Then
                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - found records in EscalationDetails,AlertDetails", LogLevel.Verbose)
                    '1. Escalation enabled
                    For i = 0 To dt.Rows.Count - 1
                        Try
                            'NOTE: Log File alerts are EXCLUDED from escalation
                            sqlStr = "SELECT t1.ID,SentTo,AlertClearedDateTime,AlertHistoryID,AlertCreatedDateTime,t1.AlertKey, " &
                            "DeviceName, DeviceType, AlertType, Details " &
                            "FROM AlertSentDetails t1 " &
                            "INNER JOIN AlertHistory t2 " &
                            "ON t1.AlertHistoryID=t2.ID " &
                            "WHERE AlertKey = " & dt.Rows(i)("AlertKey").ToString() & " AND " &
                            "AlertType NOT LIKE '%log file%' AND " &
                            "AlertClearedDateTime Is NULL " &
                            "ORDER BY AlertCreatedDateTime DESC "
                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to get records from AlertSentDetails,AlertHistory", LogLevel.Verbose)
                            dt2 = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sqlStr)
                            If dt2.Rows.Count > 0 Then
                                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - found records in AlertSentDetails,AlertHistory", LogLevel.Verbose)
                                '5/13/2015 NS modified for VSPLUS-1758
                                '1.1 Weed out the alert types/servers that are not selected in the alert definition
                                Dim foundMatch() As DataRow
                                For n = 0 To ADefArrOut.Length - 1
                                    foundMatch = dt2.Select("DeviceName = '" & ADefArrOut(n).ServerName & "' AND DeviceType = '" &
                                        ADefArrOut(n).ServerType & "' AND AlertType = '" & ADefArrOut(n).EventName & "'")
                                    If foundMatch.Length > 0 Then
                                        '2. An alert has been sent for the alert key
                                        For j = 0 To foundMatch.Length - 1
                                            sqlStr = "SELECT DISTINCT t1.ID,t1.SentTo,EscalationCreatedDateTime,t1.AlertHistoryID, " &
                                             "t1.EscalationID,EscalationInterval " &
                                             "FROM EscalationSentDetails t1 " &
                                             "INNER JOIN AlertSentDetails t2 ON " &
                                             "t1.AlertHistoryID = t2.AlertHistoryID " &
                                             "INNER JOIN EscalationDetails t3 ON " &
                                             "t1.EscalationID = t3.ID And t3.AlertKey = t2.AlertKey " &
                                             "WHERE t1.AlertHistoryID = " & foundMatch(j)("AlertHistoryID").ToString() & " " &
                                             "AND t2.AlertKey = " & dt.Rows(i)("AlertKey").ToString() & " " &
                                             "AND EscalationInterval = " & dt.Rows(i)("EscalationInterval").ToString()
                                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to get records from EscalationSentDetails,AlertSentDetails,EscalationDetails", LogLevel.Verbose)
                                            dt3 = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sqlStr)
                                            If dt3.Rows.Count = 0 Then
                                                esmssent = False
                                                eemailsent = False
                                                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - found NO records from EscalationSentDetails,AlertSentDetails,EscalationDetails", LogLevel.Verbose)
                                                alertcreated = Convert.ToDateTime(foundMatch(j)("AlertCreatedDateTime").ToString())
                                                If (Now - alertcreated).TotalMinutes >= Convert.ToInt32(dt.Rows(i)("EscalationInterval").ToString()) Then
                                                    '3. Send escalation
                                                    If dt.Rows(i)("EscalateTo").ToString() <> "" Then
                                                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - attempting to send an escalation via email", LogLevel.Verbose)
                                                        WriteServiceHistoryEntry(Now.ToString & " Attempting to send an escalation via email:", LogLevel.Normal)
                                                        WriteServiceHistoryEntry(Now.ToString & "   EscalateTo = " & dt.Rows(i)("EscalateTo").ToString() & ",    AlertKey = " & dt.Rows(i)("AlertKey").ToString(), LogLevel.Normal)
                                                        SendMailwithChilkatorNet(dt.Rows(i)("EscalateTo").ToString(), "", "", foundMatch(j)("DeviceName").ToString(), foundMatch(j)("DeviceType").ToString(), "", foundMatch(j)("AlertType").ToString(), foundMatch(j)("AlertType").ToString(), foundMatch(j)("Details").ToString(), "", "ESCALATION")
                                                        eemailsent = True
                                                    End If
                                                    If dt.Rows(i)("SMSTo").ToString() <> "" Then
                                                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - attempting to send an escalation via SMS", LogLevel.Verbose)
                                                        WriteServiceHistoryEntry(Now.ToString & " Attempting to send an escalation via SMS:", LogLevel.Normal)
                                                        WriteServiceHistoryEntry(Now.ToString & "   SMSTo = " & dt.Rows(i)("SMSTo").ToString() & ",    AlertKey = " & dt.Rows(i)("AlertKey").ToString(), LogLevel.Normal)
                                                        SendSMSwithTwilio(dt.Rows(i)("SMSTo").ToString(), foundMatch(j)("DeviceName").ToString(), foundMatch(j)("DeviceType").ToString(), foundMatch(j)("AlertType").ToString(), foundMatch(j)("Details").ToString(), "ESCALATION ", "")
                                                        esmssent = True
                                                    End If
                                                    '4. Store escalation information
                                                    If eemailsent Then
                                                        'Update EscalationSentDetailsTable
                                                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to insert sent email escalation info", LogLevel.Verbose)
                                                        InsertSentEscalation(dt.Rows(i)("EscalateTo").ToString(), foundMatch(j)("AlertHistoryID").ToString(), dt.Rows(i)("ID").ToString())
                                                    End If
                                                    If esmssent Then
                                                        'Update EscalationSentDetailsTable
                                                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsSendNotification - trying to insert sent SMS escalation info", LogLevel.Verbose)
                                                        InsertSentEscalation(dt.Rows(i)("SMSTo").ToString(), foundMatch(j)("AlertHistoryID").ToString(), dt.Rows(i)("ID").ToString())
                                                    End If
                                                End If
                                            End If
                                        Next
                                    End If
                                Next
                            End If
                        Catch ex As Exception
                            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error while selecting last sent alert for AlertKey=" & dt.Rows(i)("AlertKey").ToString() & " " & ex.ToString, LogLevel.Normal)
                        End Try
                    Next
                End If
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsSendNotification: Error while selecting Escalation details " & ex.ToString, LogLevel.Normal)
            End Try
        End If
    End Sub
    Private Function GetAlertHistory() As DataTable
        Dim dt As DataTable

        Return dt
    End Function
    Private Function GetMaxAlertsRemainingToday()
        Dim sSQL1 As String = " SELECT COUNT(*) from AlertSentDetails where DATEPART(d,AlertCreatedDateTime)=DATEPART(d,GETDATE()) AND AlertCreatedDateTime IS NOT NULL "
        Dim sSQL2 As String = " SELECT COUNT(*) from AlertSentDetails where DATEPART(d,AlertClearedDateTime)=DATEPART(d,GETDATE()) AND AlertClearedDateTime IS NOT NULL "
        Dim sSQL3 As String = " SELECT SVALUE FROM Settings WHERE sname='TotalMaximumAlertPerDay' "
        Dim sSQL As String = sSQL1 + " UNION ALL " + sSQL2 + " UNION ALL " + sSQL3
        Dim totalRemaining As Integer = 0
        '12/16/2014 NS added for VSPLUS-1267
        Dim myConnectionString As New VSFramework.XMLOperation
        Dim myAdapter As New VSFramework.VSAdaptor

        Try

            '12/16/2014 NS modified for VSPLUS-1267
            'Dim DA2 As New SqlDataAdapter(sSQL, con)
            'Dim DS2 As New DataSet
            'DA2.Fill(DS2, "AlertSentDetails")
            'Dim dtCounts As DataTable = DS2.Tables(0)
            Dim dtCounts As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sSQL)
            Dim totalCount As Integer = 0
            Dim totalMax As Integer = 0
            If dtCounts.Rows.Count > 0 Then
                For i As Integer = 0 To dtCounts.Rows.Count - 1
                    '9/25/2014 NS commented out
                    'WriteServiceHistoryEntry(Now.ToString & " Max Alerts per day value: " & i.ToString() & "::" & dtCounts.Rows(i)(0).ToString())
                    If i = 0 Or i = 1 Then
                        totalCount += Convert.ToInt32(dtCounts.Rows(i)(0).ToString())

                    End If
                    If i = 2 Then
                        totalMax = Convert.ToInt32(dtCounts.Rows(i)(0).ToString())
                    End If
                Next
            End If
            '9/25/2014 NS commented out
            'WriteServiceHistoryEntry(Now.ToString & " Max Alerts value: total Max per day: " & totalMax.ToString())
            'WriteServiceHistoryEntry(Now.ToString & " Max Alerts value: total sent count per day: " & totalCount.ToString())
            If totalMax > 0 Then
                If totalCount > totalMax Then
                    totalRemaining = 0
                Else
                    totalRemaining = (totalMax - totalCount)
                End If
            Else
                totalRemaining = -1 ' there are no limit as the user has not configured anything
            End If
            '9/25/2014 NS commented out
            'WriteServiceHistoryEntry(Now.ToString & " Total Max Alerts value: " & totalRemaining.ToString)
        Catch ex As Exception
            totalRemaining = -1
            WriteServiceHistoryEntry(Now.ToString & " In GetMaxAlertsRemainingToday: Error while calculating max alerts per day " & ex.ToString, LogLevel.Normal)
        End Try
        '9/25/2014 NS commented out
        'WriteServiceHistoryEntry(Now.ToString & " Max Alerts value: Total Remaining per day: " & totalRemaining.ToString())
        Return totalRemaining
    End Function
    Private Function GetMaxAlertsRemainingPerDefToday(ByVal AlertKey As Integer)
        Dim sSQL1 As String = " SELECT COUNT(*) from AlertSentDetails where DATEPART(d,AlertCreatedDateTime)=DATEPART(d,GETDATE()) AND AlertCreatedDateTime IS NOT NULL AND AlertKey=" + AlertKey.ToString()
        Dim sSQL2 As String = " SELECT COUNT(*) from AlertSentDetails where DATEPART(d,AlertClearedDateTime)=DATEPART(d,GETDATE()) AND AlertClearedDateTime IS NOT NULL AND AlertKey=" + AlertKey.ToString()
        Dim sSQL3 As String = " SELECT SVALUE FROM Settings WHERE sname='TotalMaximumAlertsPerDefinition' "
        Dim sSQL As String = sSQL1 + " UNION ALL " + sSQL2 + " UNION ALL " + sSQL3
        Dim totalRemaining As Integer = 0
        '12/16/2014 NS added for VSPLUS-1267
        Dim myConnectionString As New VSFramework.XMLOperation
        Dim myAdapter As New VSFramework.VSAdaptor

        Try

            '12/16/2014 NS modified for VSPLUS-1267
            'Dim DA2 As New SqlDataAdapter(sSQL, con)
            'Dim DS2 As New DataSet
            'DA2.Fill(DS2, "AlertSentDetails")
            'Dim dtCounts As DataTable = DS2.Tables(0)
            Dim dtCounts As DataTable = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sSQL)
            Dim totalCount As Integer = 0
            Dim totalMax As Integer = 0
            If dtCounts.Rows.Count > 0 Then
                For i As Integer = 0 To dtCounts.Rows.Count - 1
                    '9/25/2014 NS commented out
                    'WriteServiceHistoryEntry(Now.ToString & " Max Alerts per def/day value: " & i.ToString() & "::" & dtCounts.Rows(i)(0).ToString())
                    If i = 0 Or i = 1 Then
                        totalCount += Convert.ToInt32(dtCounts.Rows(i)(0).ToString())

                    End If
                    If i = 2 Then
                        totalMax = Convert.ToInt32(dtCounts.Rows(i)(0).ToString())
                    End If
                Next
            End If
            '9/25/2014 NS commented out
            'WriteServiceHistoryEntry(Now.ToString & " Max Alerts value: total Max per def/day: " & totalMax.ToString())
            'WriteServiceHistoryEntry(Now.ToString & " Max Alerts value: total sent count per def/day: " & totalCount.ToString())
            If totalMax > 0 Then
                If totalCount > totalMax Then
                    totalRemaining = 0
                Else
                    totalRemaining = (totalMax - totalCount)
                End If
            Else
                totalRemaining = -1 ' there are no limit as the user has not configured anything
            End If
            '9/25/2014 NS commented out
            'WriteServiceHistoryEntry(Now.ToString & " Total Max Alerts value: " & totalRemaining.ToString)
        Catch ex As Exception
            totalRemaining = -1
            WriteServiceHistoryEntry(Now.ToString & " In GetMaxAlertsRemainingPerDefToday: Error while calculating max alerts per def/day " & ex.ToString, LogLevel.Normal)
        End Try
        '9/25/2014 NS commented out
        'WriteServiceHistoryEntry(Now.ToString & " Max Alerts value: Total Remaining per def/day: " & totalRemaining.ToString())

        Return totalRemaining
    End Function

    Private Sub ProcessAlertsClearSendNotification()
        '12/3/2013 NS updated the original function by MD - process cleared 
        Try
            Dim mailsent As Boolean = False
            Dim Details As String = ""
            Dim AlertID As Integer
            Dim SentID As Integer
            Dim ServerName As String = ""
            Dim EventName As String = ""
            Dim Location As String = ""
            Dim ServerType As String = ""
            Dim AlertType As String = ""
            Dim CC As String = ""
            Dim BCC As String = ""
            Dim SendTo As String = ""
            '12/1/2014 NS added for VSPLUS-946
            Dim SMSTo As String = ""
            Dim smssent As Boolean = False
            '12/9/2014 NS added for VSPLUS-1229
            Dim ScriptName As String = ""
            Dim ScriptCommand As String = ""
            Dim ScriptLocation As String = ""
            Dim scriptsent As Boolean = False
            Dim Historytable As DataTable
            Dim AHistArr As AlertDefinition()
            Dim ADefArrOut As AlertDefinition()
            Dim ADef As AlertDefinition
            Dim ADefOut As AlertDefinition
            Dim AHist As AlertDefinition
            Dim sqlStr As String
            Dim DA1 As New SqlDataAdapter
            Dim DS1 As New DataSet
            Dim param As New SqlParameter
            Dim retVal As Integer
            Dim da As SqlDataAdapter
            Dim dtlog As DataTable
            Dim c As Integer
            Dim actEventName As String
            Dim SDs As New DataSet
            Dim myCommand As New SqlCommand
            Dim myAdapter2 As New SqlDataAdapter
            Dim AlertDictOut As New Dictionary(Of Integer, Integer)
            Dim pairout As KeyValuePair(Of Integer, Integer)
            '4/4/2014 NS added for VSPLUS-403
            '5/14/2015 NS commented out for VSPLUS-1752
            'Dim AlertsWinLog As Boolean
            '12/16/2014 NS added for VSPLUS-1267
            Dim con As New SqlConnection
            Dim myConnectionString As New VSFramework.XMLOperation
            Dim myAdapter As New VSFramework.VSAdaptor

            con.ConnectionString = myConnectionString.GetDBConnectionString("VitalSigns")
            con.Open()
            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - established SQL connection", LogLevel.Verbose)
            '4/4/2014 NS added for VSPLUS-403
            '5/14/2015 NS commented out for VSPLUS-1752
            'AlertsWinLog = False
            'Try
            '    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - trying to get AlertsWinLog from Settings", LogLevel.Verbose)
            '    AlertsWinLog = Convert.ToBoolean(getSettings("AlertsWinLog"))
            'Catch ex As Exception
            '    WriteServiceHistoryEntry(Now.ToString & " Error getting Windows log option from the Settings table:  " & ex.ToString, LogLevel.Normal)
            'End Try

            '4/7/2014 NS modified for VSPLUS-403
            '5/14/2015 NS commented out for VSPLUS-1752
            'If AlertsWinLog = False Then
            Try
                Dim command As SqlCommand = New SqlCommand("GetClearedAlertsById", con)
                myAdapter2.SelectCommand = command
                myAdapter2.SelectCommand.CommandType = CommandType.StoredProcedure
                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - trying to get records from GetClearedAlertsById", LogLevel.Verbose)
                myAdapter2.Fill(SDs, "AlertHistory")
                Historytable = SDs.Tables(0)
                myAdapter2.SelectCommand.Parameters.Clear()
                myCommand.Dispose()

                'WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClear: The stored procedure returned " & Historytable.Rows.Count & " records.")
            Catch ex As ApplicationException
                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error  at the time of selecting records for Cleared Alerts from the AlertHistory table: " & ex.Message, LogLevel.Normal)
            End Try
            c = 0
            ReDim AHistArr(c)
            If Not Historytable Is Nothing Then
                If (Historytable.Rows.Count > 0) Then
                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - found records from GetClearedAlertsById", LogLevel.Verbose)
                    For hst As Integer = 0 To Historytable.Rows.Count - 1
                        Try
                            ADef = New AlertDefinition
                            ADef.AlertKey = Historytable.Rows(hst)("ID")
                            ADef.SentID = Historytable.Rows(hst)("sentid")
                            ADef.SendTo = Historytable.Rows(hst)("sentTo")
                            ADef.ServerName = Historytable.Rows(hst)("DeviceName").ToString()
                            ADef.ServerType = Historytable.Rows(hst)("DeviceType").ToString()
                            ADef.EventName = Historytable.Rows(hst)("AlertType").ToString()
                            '11/19/2015 NS modified for VSPLUS-2348
                            ADef.Details = Historytable.Rows(hst)("Details").ToString() & vbCrLf & vbCrLf &
                                "Alert condition was cleared at " & Historytable.Rows(hst)("DateTimeAlertCleared").ToString() &
                                vbCrLf & vbCrLf & "Detected: " & Historytable.Rows(hst)("DateTimeOfAlert").ToString()
                            ReDim Preserve AHistArr(c)
                            AHistArr(c) = ADef
                            c = c + 1
                        Catch ex As ApplicationException
                            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Exiting. For record #" + (hst + 1) + " of " + Historytable.Rows.Count + " records, error occurred at the time of assigning records to variables from AlertHistory table: " & ex.Message, LogLevel.Normal)
                        End Try
                    Next
                End If
            End If

            'WriteServiceHistoryEntry(Now.ToString & " Here #1")
            c = 0
            Try
                If Not IsNothing(AlertDict) And Not IsNothing(AHistArr) Then
                    For Each pair In AlertDict
                        If Not IsNothing(pair) Then
                            'WriteServiceHistoryEntry(Now.ToString & " " & pair.Value.Length)
                            For i As Integer = 0 To pair.Value.Length - 1
                                ADef = pair.Value(i)
                                For j As Integer = 0 To AHistArr.Length - 1
                                    AHist = AHistArr(j)
                                    If Not IsNothing(AHist) Then
                                        If (InStr(AHist.EventName, ADef.EventName) > 0 And ADef.ServerName = AHist.ServerName And ADef.ServerType = AHist.ServerType) Or
                                        (InStr(AHist.EventName, ADef.EventName) > 0 And ADef.ServerType = AHist.ServerType And ADef.ServerName = "") Then
                                            WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - found a match", LogLevel.Verbose)
                                            ADefOut = New AlertDefinition
                                            ADefOut.AlertKey = AHist.AlertKey
                                            '2/17/2014 NS added SentID value
                                            ADefOut.SentID = AHist.SentID
                                            '6/22/2015 NS modified for VSPLUS-1802
                                            ADefOut.EventName = AHist.EventName
                                            ADefOut.ServerType = ADef.ServerType
                                            ADefOut.ServerName = AHist.ServerName
                                            ADefOut.SendTo = ADef.SendTo
                                            'WriteServiceHistoryEntry(Now.ToString & ": adef  " & ADef.SendTo & ", hist " & AHist.SendTo & ", " & AHist.AlertKey, LogLevel.Normal)
                                            ADefOut.SendTo = AHist.SendTo
                                            ADefOut.CopyTo = ADef.CopyTo
                                            ADefOut.BlindCopyTo = ADef.BlindCopyTo
                                            ADefOut.StartTime = ADef.StartTime
                                            ADefOut.Duration = ADef.Duration
                                            ADefOut.Days = ADef.Days
                                            ADefOut.IntType = ADef.IntType
                                            ADefOut.Details = AHist.Details
                                            ADefOut.SendSNMPTrap = ADef.SendSNMPTrap
                                            '4/7/2014 NS added for VSPLUS-519
                                            ADefOut.EnablePersistentAlert = ADef.EnablePersistentAlert
                                            ADefOut.DateCreated = AHist.DateCreated
                                            '12/1/2014 NS added for VSPLUS-946
                                            ADefOut.SMSTo = ADef.SMSTo
                                            '12/9/2014 NS added for VSPLUS-1229
                                            ADefOut.ScriptName = ADef.ScriptName
                                            ADefOut.ScriptCommand = ADef.ScriptCommand
                                            ADefOut.ScriptLocation = ADef.ScriptLocation
                                            'WriteServiceHistoryEntry(Now.ToString & ": " & ADef.AlertKey & ", " & ADef.SendTo & ", " & ADef.SentID, LogLevel.Normal)
                                            ReDim Preserve ADefArrOut(c)
                                            ADefArrOut(c) = ADefOut
                                            c = c + 1
                                            'WriteServiceHistoryEntry(Now.ToString & " Found match (ID=" & AHist.AlertKey & "): " & ADefOut.AlertKey & " " & ADefOut.SentID & " " & ADefOut.EventName & " " & AHist.ServerName & " " & ADefOut.ServerType & " " & ADefOut.StartTime & " " & ADefOut.SendTo)
                                        End If
                                    End If
                                Next
                                'WriteServiceHistoryEntry(Now.ToString & " " & ADef.AlertKey & " " & ADef.EventName & " " & ADef.ServerName & " " & ADef.ServerType)
                            Next
                        End If
                    Next
                    '2/17/2014 NS added - clear the dictionary variable here
                    Try
                        AlertDict.Clear()
                    Catch ex As Exception
                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Could not clear AlertDict " & ex.Message, LogLevel.Normal)
                    End Try

                End If
            Catch ex As Exception
                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error processing the alert distionary data set: " & ex.Message, LogLevel.Normal)
            End Try

            'If Not IsNothing(SDs) Then
            'SDs.Clear()
            'End If
            '12/16/2014 NS commented out for VSPLUS-1267
            'con.Open()
            'WriteServiceHistoryEntry(Now.ToString & " Here #2.1")
            If Not ADefArrOut Is Nothing Then
                Try
                    da = New SqlDataAdapter("ShouldAlertGoOutNow", con)
                    da.SelectCommand.CommandType = CommandType.StoredProcedure
                Catch ex As Exception
                    WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearNotification: Error getting a handle on the ShouldAlertGoOutNow stored procedure: " & ex.Message, LogLevel.Normal)
                End Try

                Try
                    da.SelectCommand.Parameters.Add(New SqlParameter("@StartTime", SqlDbType.VarChar, 50))
                    da.SelectCommand.Parameters.Add(New SqlParameter("@Duration", SqlDbType.Int))
                    da.SelectCommand.Parameters.Add(New SqlParameter("@DaysStr", SqlDbType.VarChar, 200))
                    da.SelectCommand.Parameters.Add(New SqlParameter("@IntType", SqlDbType.Int))
                    param.Direction = ParameterDirection.ReturnValue
                    param.ParameterName = "returnValue"
                    da.SelectCommand.Parameters.Add(param)
                    For j As Integer = 0 To ADefArrOut.Length - 1
                        ADefOut = New AlertDefinition
                        ADefOut = ADefArrOut(j)
                        If ADefOut.StartTime = "" Then
                            da.SelectCommand.Parameters("@StartTime").Value = ""
                        Else
                            da.SelectCommand.Parameters("@StartTime").Value = ADefOut.StartTime
                        End If
                        da.SelectCommand.Parameters("@Duration").Value = ADefOut.Duration
                        da.SelectCommand.Parameters("@DaysStr").Value = ADefOut.Days
                        da.SelectCommand.Parameters("@IntType").Value = ADefOut.IntType
                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - trying to get records from ShouldAlertGoOutNow", LogLevel.Verbose)
                        da.SelectCommand.ExecuteNonQuery()
                        retVal = Convert.ToInt32(da.SelectCommand.Parameters("returnValue").Value.ToString())
                        ADefOut.RunNow = retVal
                        If retVal > 0 Then
                            'WriteServiceHistoryEntry(Now.ToString & " Found an alert to be sent: " & ADefOut.AlertKey & ", notify: " & ADefOut.SendTo)
                        End If
                        ADefArrOut(j) = ADefOut
                    Next
                Catch ex As Exception
                    WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error executing the ShouldAlertGoOutNow stored procedure: " & ex.Message, LogLevel.Normal)
                End Try
            End If

            If Not IsNothing(con) Then
                con.Close()
            End If

            'WriteServiceHistoryEntry(Now.ToString & " Here #3")

            If Not ADefArrOut Is Nothing Then
                For i = 0 To ADefArrOut.Length - 1
                    ADef = ADefArrOut(i)
                    mailsent = False
                    '12/1/2014 NS added for VSPLUS-946
                    smssent = False
                    '12/9/2014 NS added for VSPPLUS-1229
                    scriptsent = False
                    Try
                        AlertID = ADef.AlertKey  'Historytable.Rows(hst)("ID")
                        SentID = ADef.SentID
                        ServerName = ADef.ServerName 'Historytable.Rows(hst)("DeviceName").ToString()
                        ServerType = ADef.ServerType  'Historytable.Rows(hst)("DeviceType").ToString()
                        'Location = Historytable.Rows(hst)("Location").ToString()
                        EventName = ADef.EventName  'Historytable.Rows(hst)("AlertType").ToString()
                        AlertType = ADef.EventName
                        Details = ADef.Details '"Alert condition was cleared at " & Historytable.Rows(hst)("DateTimeAlertCleared").ToString() & vbCrLf & vbCrLf & "Detected: " & Historytable.Rows(hst)("DateTimeOfAlert").ToString()
                    Catch ex As ApplicationException
                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Exiting. For record #" + (i + 1) + " of " + ADefArrOut.Length + " records, error occurred at the time of assigning records to variables from the AlertHistory table: " & ex.Message, LogLevel.Normal)
                        Return
                    End Try

                    SendTo = ""
                    CC = ""
                    BCC = ""
                    '12/9/2014 NS added for VSPLUS-1229
                    SMSTo = ""
                    ScriptName = ""
                    ScriptCommand = ""
                    ScriptLocation = ""
                    '2/17/2014 NS added
                    Try
                        '4/16/2015 NS modified for VSPLUS-1389
                        If Not AlertDictOut.ContainsKey(SentID) Then
                            AlertDictOut.Add(SentID, SentID)
                            Try
                                SendTo = ADef.SendTo.Replace(",,", "") 'Historytable.Rows(hst)("SentTo").ToString().Replace(",,", "")
                                CC = ADef.CopyTo
                                BCC = ADef.BlindCopyTo
                                '12/9/2014 NS added for VSPLUS-1229
                                SMSTo = ADef.SMSTo
                                ScriptName = ADef.ScriptName
                                ScriptCommand = ADef.ScriptCommand
                                ScriptLocation = ADef.ScriptLocation
                            Catch ex As Exception
                                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error while reading records from datatable into Sendto: " & ex.Message, LogLevel.Normal)
                            End Try

                            '12/9/2014 NS modified for VSPLUS-1229
                            If SendTo = "" And CC = "" And BCC = "" And SMSTo = "" And ScriptName = "" Then
                                'WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClear: There are no new recipients to send alert cleared notifications.")
                                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - no new recipients to send alert cleared notification", LogLevel.Verbose)
                            Else
                                WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - found recipients to send alert cleared notification", LogLevel.Verbose)
                                Try
                                    '12/9/2014 NS modified for VSPLUS-946
                                    '6/19/2015 NS modified for VSPLUS-1764
                                    If SendTo <> "" And SendTo <> "SNMP Trap" And SendTo <> "Windows Log" Then
                                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - attempting to send alert cleared notification via email", LogLevel.Verbose)
                                        WriteServiceHistoryEntry(Now.ToString & " Attempting to send a cleared alert e-mail:", LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   SendTo = " & SendTo, LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   ServerName = " & ServerName, LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   Details = " & Details, LogLevel.Normal)
                                        '11/10/2015 NS modified for VSPLUS-2348

                                        If (AlertType.Contains("Services")) Then
                                            Details = "The Original alert was Services -" & vbCrLf & Details
                                           
                                        Else
                                            Details = "The original alert was '" & AlertType & "' - " & vbCrLf & Details
                                        End If


                                        '3/31/2014 NS renamed for VSPLUS-489
                                        SendMailwithChilkatorNet(SendTo, CC, BCC, ServerName, ServerType, "", EventName, EventName, Details, "Cleared ", "Alert")
                                        mailsent = True
                                    ElseIf SendTo = "Windows Log" Then
                                        Dim sSource As String = "VitalSigns Plus"
                                        Dim sLog As String = "Application"
                                        Dim sEvent As String = "Alert condition '" & ADef.EventName & "' for " & ADef.ServerType & ", " & ADef.ServerName & " was cleared at " & Now.ToLongTimeString & ". " & vbCrLf & ADef.Details
                                        Try
                                            If Not EventLog.SourceExists(sSource) Then
                                                EventLog.CreateEventSource(sSource, sLog)
                                            End If

                                            EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234)
                                        Catch ex As Exception
                                            WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error writing to Windows log for alert ID " & ADef.AlertKey & ": " & ex.Message, LogLevel.Normal)
                                        End Try
                                        SendTo = "Windows Log"
                                        mailsent = True
                                    End If
                                Catch ex As ApplicationException
                                    WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error occurred at the time of sending Cleared Alert Mail " & ex.Message, LogLevel.Normal)
                                    mailsent = False
                                End Try

                                Try
                                    '12/9/2014 NS added for VSPLUS-946
                                    If SMSTo <> "" Then
                                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - attempting to send alert cleared notification via SMS", LogLevel.Verbose)
                                        WriteServiceHistoryEntry(Now.ToString & " Attempting to send a cleared alert SMS:", LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   SMSTo = " & SMSTo, LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   ServerName = " & ServerName, LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   Details = " & Details, LogLevel.Normal)
                                        '11/10/2015 NS modified for VSPLUS-2348
                                        Details = "The original alert was '" & AlertType & "' - " & vbCrLf & Details
                                        SendSMSwithTwilio(SMSTo, ServerName, ServerType, EventName, Details, "Cleared ", "")
                                        smssent = True
                                    End If
                                Catch ex As ApplicationException
                                    WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error occurred at the time of sending Cleared Alert Mail " & ex.Message, LogLevel.Normal)
                                    smssent = False
                                End Try

                                Try
                                    '12/9/2014 NS added for VSPLUS-946
                                    If ScriptName <> "" Then
                                        WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - attempting to send alert cleared notification via Script", LogLevel.Verbose)
                                        WriteServiceHistoryEntry(Now.ToString & " Attempting to send a cleared alert script:", LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   Script = " & ScriptName, LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   ServerName = " & ServerName, LogLevel.Normal)
                                        WriteServiceHistoryEntry(Now.ToString & "   Details = " & Details, LogLevel.Normal)
                                        '11/10/2015 NS modified for VSPLUS-2348
                                        Details = "The original alert was '" & AlertType & "' - " & vbCrLf & Details
                                        SendScript(ScriptName, ScriptCommand, ScriptLocation, ServerName, ServerType, EventName, Details, "Cleared ")
                                        scriptsent = True
                                    End If
                                Catch ex As ApplicationException
                                    WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error occurred at the time of sending Cleared Alert Mail " & ex.Message, LogLevel.Normal)
                                    scriptsent = False
                                End Try
                                '***** SNMP Conditions *********
                                'Try
                                'WriteServiceHistoryEntry(Now.ToString & " Entering SNMP module evaluation.")
                                'Dim SNMPEnabled As String = ""
                                ''SNMPEnabled = getSettings("EnableSNMP")
                                'Dim SNMPTrap As Boolean = Historytable.Rows(hst)("SendSNMPTrap")
                                'If SNMPEnabled = "True" And SNMPTrap = True Then
                                '    'Dim SNMPHostName As String = getSettings("SNMPHostName")
                                '    'SendSNMPTrap(SNMPHostName, ServerType, ServerName, AlertType, Details)
                                'End If
                                'Catch ex As ApplicationException
                                '    'WriteServiceHistoryEntry(Now.ToString & " Error occurred at  SendSNMPTrap " & ex.Message)
                                'End Try

                            End If
                            Try
                                If mailsent = True Then
                                    Dim mails As String = ""
                                    mails = SendTo
                                    'WriteServiceHistoryEntry(Now.ToString & " Attempting to update mail history.")
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - trying to update sent mail info", LogLevel.Verbose)
                                    UpdatingSentMails(AlertID, mails, SentID)
                                    'WriteServiceHistoryEntry(Now.ToString & " Updated row for AlertID: " & AlertID)
                                End If
                            Catch ex As Exception
                                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error while attempting to update Cleared Alert mail history for E-mail: " & ex.ToString(), LogLevel.Normal)
                            End Try
                            '12/9/2014 NS added for VSPLUS-946, VSPLUS-1229
                            Try
                                If smssent = True Then
                                    Dim mails As String = ""
                                    mails = SMSTo
                                    'WriteServiceHistoryEntry(Now.ToString & " Attempting to update mail history.")
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - trying to update sent SMS info", LogLevel.Verbose)
                                    UpdatingSentMails(AlertID, mails, SentID)
                                    'WriteServiceHistoryEntry(Now.ToString & " Updated row for AlertID: " & AlertID)
                                End If
                            Catch ex As Exception
                                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error while attempting to update Cleared Alert mail history for SMS: " & ex.ToString(), LogLevel.Normal)
                            End Try
                            Try
                                If scriptsent = True Then
                                    Dim mails As String = ""
                                    mails = ScriptName
                                    'WriteServiceHistoryEntry(Now.ToString & " Attempting to update mail history.")
                                    WriteServiceHistoryEntry(Now.ToString & " ProcessAlertsClearSendNotification - trying to update sent Scipt info", LogLevel.Verbose)
                                    UpdatingSentMails(AlertID, mails, SentID)
                                    'WriteServiceHistoryEntry(Now.ToString & " Updated row for AlertID: " & AlertID)
                                End If
                            Catch ex As Exception
                                WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Error while attempting to update Cleared Alert mail history for Script: " & ex.ToString(), LogLevel.Normal)
                            End Try
                        End If
                    Catch ex As Exception
                        WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: the cleared e-mail alert has already been sent for SentID=" & SentID & ", " & ex.ToString(), LogLevel.Normal)
                    End Try
                Next
            End If
            'Else
            '         '2/17/2014 NS added - clear the dictionary variable here
            '         Try
            '             AlertDict.Clear()
            '         Catch ex As Exception
            '             WriteServiceHistoryEntry(Now.ToString & " In ProcessAlertsClearSendNotification: Could not clear AlertDict " & ex.Message, LogLevel.Normal)
            '         End Try
            'End If
        Catch ex As Exception
            WriteServiceHistoryEntry(Now.ToString & " Error in ProcessAlertsClearSendNotification: " & ex.Message, LogLevel.Normal)
        End Try
	End Sub
	Private Function SendMailNet(ByVal PHostName As String, ByVal Pport As String, ByVal PAuth As Boolean,
  ByVal boolHSBC As Boolean, ByVal Ppwd As String, ByVal PEmail As String, ByVal PSSL As Boolean,
  ByVal Details As String, ByVal iscleared As String, ByVal ServerType As String, ByVal ServerName As String,
  ByVal AlertType As String, ByVal SendTo As String, ByVal CC As String, ByVal BCC As String, ByVal PFrom As String,
  ByVal isEmergency As Boolean) As Boolean
		Return SendMailNet(PHostName, Pport, PAuth, boolHSBC, Ppwd, PEmail, PSSL, Details, iscleared, ServerType, ServerName, AlertType, SendTo, CC, BCC, PFrom, isEmergency, "")
	End Function
    '3/31/2013 NS added for VSPLUS-489
	Public Function SendMailNet(ByVal PHostName As String, ByVal Pport As String, ByVal PAuth As Boolean,
	 ByVal boolHSBC As Boolean, ByVal Ppwd As String, ByVal PEmail As String, ByVal PSSL As Boolean,
	 ByVal Details As String, ByVal iscleared As String, ByVal ServerType As String, ByVal ServerName As String,
	 ByVal AlertType As String, ByVal SendTo As String, ByVal CC As String, ByVal BCC As String, ByVal PFrom As String,
	 ByVal isEmergency As Boolean, sSubject As String) As Boolean
		Dim success As Boolean = True
		Dim subject As String

		'7/20/2015 NS modified for VSPLUS-1562
		If isEmergency Then
			subject = "VitalSigns EMERGENCY Notification - SQL Server may be down."
		Else
			subject = iscleared & "Alert for " & ServerType & " " & ServerName & " - " & AlertType
		End If
		If sSubject <> "" Then
			subject = sSubject
		End If
		Try
			Dim Smtp_Server As New SmtpClient
			Dim e_mail As New MailMessage()
			Smtp_Server.UseDefaultCredentials = False
			If PAuth = True And boolHSBC = False Then
				Smtp_Server.Credentials = New Net.NetworkCredential(PEmail, Ppwd)
			End If
			'WriteServiceHistoryEntry(Now.ToString & " Host detail. Host name =  " & PHostName & " PSSL = " & PSSL & " Pport = " & Pport & " Ppwd = " & Ppwd & vbCrLf)
			Smtp_Server.Port = Pport
			'4/15/2014 NS modified
			Smtp_Server.EnableSsl = PSSL
			Smtp_Server.Host = PHostName
			'WriteServiceHistoryEntry(Now.ToString & " Email Address = " & PEmail & " From = " & PFrom & vbCrLf)
			e_mail = New MailMessage()
			'4/15/2014 NS modified - use email and from, email must be a valid email address, from may be a name
			'e_mail.From = New MailAddress(PFrom)
			e_mail.From = New MailAddress(PEmail, PFrom)

			WriteServiceHistoryEntry(Now.ToString & " Email Address = " & PEmail & " From = " & e_mail.From.DisplayName & " SendTo = " & SendTo & " subject = " & subject & vbCrLf, LogLevel.Normal)
			If SendTo IsNot Nothing And SendTo.Trim <> "" Then
				'7/20/2015 NS modified for VSPLUS-1562
				If SendTo.IndexOf(",") > 0 Then
					Dim result As String() = SendTo.Split(New String() {","}, StringSplitOptions.None)
					If result Is Nothing Then
						e_mail.To.Add(New MailAddress(SendTo))
					Else
						For Each s As String In result
							WriteServiceHistoryEntry(Now.ToString & " SendTo = " & s & vbCrLf, LogLevel.Verbose)
							e_mail.To.Add(New MailAddress(s))
						Next
					End If
				Else
					e_mail.To.Add(New MailAddress(SendTo))
				End If
			End If
			If CC IsNot Nothing And CC.Trim <> "" Then
				e_mail.CC.Add(New MailAddress(CC))
			End If
			If BCC IsNot Nothing And BCC.Trim <> "" Then
				e_mail.Bcc.Add(New MailAddress(BCC))
			End If
			e_mail.Subject = subject
            e_mail.IsBodyHtml = False
            '4/21/2016 NS modified for VSPLUS-2755
            Dim localZone = TimeZone.CurrentTimeZone
            e_mail.Body = Details & vbCrLf & "Message sent: " & Now.ToLongDateString & " " & Now.ToShortTimeString & " " & localZone.StandardName _
                & vbCrLf & vbCrLf & "NOTE: Message sent time zone value reflects the Vital Signs server installation setting."
			Smtp_Server.Send(e_mail)
			success = True
			'WriteServiceHistoryEntry(Now.ToString & " Sent SMTP mail to " & SendTo & " re: " & ServerName & ", " & Details)
			'WriteServiceHistoryEntry(Now.ToString & " Email subject was " & subject & vbCrLf)
		Catch ex As Exception
			success = False
			WriteServiceHistoryEntry(Now.ToString & " Error sending mail in SendMailNet: " & ex.ToString, LogLevel.Normal)
		End Try

		Return success
	End Function
	Public Sub GetEmergencyAlertInfo()
		'7/17/2015 NS added for VSPLUS-1562
		Dim con As New SqlConnection
		Dim myConnectionString As New VSFramework.XMLOperation
		Dim myAdapter As New VSFramework.VSAdaptor
		Dim sqlStr As String
		Dim dt As DataTable
		Dim emergencyContacts As String = ""
		Dim PHostName As String = ""
		Dim Pport As String = ""
		Dim PEmail As String = ""
		Dim Ppwd As String = ""
		Dim PFrom As String = ""
		Dim PAuth As String = ""
		Dim PSSL As String = ""

		con.ConnectionString = myConnectionString.GetDBConnectionString("VitalSigns")
		con.Open()
		Try
			sqlStr = "SELECT Email FROM  AlertEmergencyContacts"
			WriteServiceHistoryEntry(Now.ToString & " GetEmergencyAlertInfo - trying to get records from AlertEmergencyContacts", LogLevel.Verbose)
			dt = myAdapter.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sqlStr)
			If dt.Rows.Count > 0 Then
				Try
					PHostName = getSettings("PrimaryHostName")
					Pport = getSettings("primaryport")
					PEmail = getSettings("primaryUserID")
					Ppwd = getSettings("primarypwd")
					PFrom = getSettings("primaryFrom")
					PAuth = Convert.ToBoolean(getSettings("primaryAuth").ToString())
					PSSL = Convert.ToBoolean(getSettings("primarySSL").ToString())
					If PFrom.ToString = "" Then
						PFrom = "VS Plus"
					End If
					For i As Integer = 0 To dt.Rows.Count - 1
						emergencyContacts += dt.Rows(i)("Email") + ","
					Next
					'11/17/2015 NS modified for VSPLUS-1562
					myRegistry.WriteToRegistry("Alert Emergency Contacts", emergencyContacts)
					myRegistry.WriteToRegistry("Alert Emergency PrimaryHostName", PHostName)
					myRegistry.WriteToRegistry("Alert Emergency primaryport", Pport)
					myRegistry.WriteToRegistry("Alert Emergency primaryUserID", PEmail)
					myRegistry.WriteToRegistry("Alert Emergency primarypwd", Ppwd)
					myRegistry.WriteToRegistry("Alert Emergency primaryFrom", PFrom)
					myRegistry.WriteToRegistry("Alert Emergency primaryAuth", PAuth)
					myRegistry.WriteToRegistry("Alert Emergency primarySSL", PSSL)
				Catch ex As Exception
					WriteServiceHistoryEntry(Now.ToString & " Error getting primary settings from the Settings table:  " & ex.ToString, LogLevel.Normal)
				End Try
			Else
				'11/17/2015 NS modified for VSPLUS-1562
				WriteServiceHistoryEntry(Now.ToString & " GetEmergencyAlertInfo - no records found in AlertEmergencyContacts", LogLevel.Verbose)
				myRegistry.WriteToRegistry("Alert Emergency Contacts", "")
				myRegistry.WriteToRegistry("Alert Emergency PrimaryHostName", "")
				myRegistry.WriteToRegistry("Alert Emergency primaryport", "")
				myRegistry.WriteToRegistry("Alert Emergency primaryUserID", "")
				myRegistry.WriteToRegistry("Alert Emergency primarypwd", "")
				myRegistry.WriteToRegistry("Alert Emergency primaryFrom", "")
				myRegistry.WriteToRegistry("Alert Emergency primaryAuth", "")
				myRegistry.WriteToRegistry("Alert Emergency primarySSL", "")
			End If
		Catch ex As Exception
			WriteServiceHistoryEntry(Now.ToString & " Error writing emergency contact info into registry: " & ex.ToString, LogLevel.Normal)
		End Try
		If Not IsNothing(con) Then
			con.Close()
		End If
	End Sub
End Class
'11/17/2015 NS added for VSPLUS-1562
Public Class RegistryHandler

    Sub WriteToRegistry(ByVal KeyName As String, ByVal KeyValue As Object)

        Dim aKey As RegistryKey

        aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\VitalSigns", True)

        If aKey Is Nothing Then
            aKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("Software\VitalSigns")
        End If

        aKey.SetValue(KeyName, KeyValue)
        aKey.Flush()

    End Sub

    Function ReadFromRegistry(ByVal KeyName As String) As Object

        Dim aKey As RegistryKey
        Try
            aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\VitalSigns")

            If aKey Is Nothing Then
                aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\VitalSigns")
            End If

            If aKey Is Nothing Then
                Return Nothing
                Exit Function
            End If
        Catch ex As Exception
            Return Nothing
            Exit Function
        End Try

        Try
            Return aKey.GetValue(KeyName)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class


'Private Sub ServiceWorkerThread_Md()

'    WriteServiceHistoryEntry(Now.ToString & " The Service Worker Thread is starting up. ")
'    ' Periodically check if the service is stopping. 

'    Do While Not Me.Stopping
'        ' Perform main service function here... 
'        Dim mailsent As Boolean = False
'        'Dim mails As String = ""
'        Dim Details As String = ""
'        Dim AlertID As Integer
'        Dim ServerName As String = ""
'        Dim EventName As String = ""
'        Dim Location As String = ""
'        Dim ServerType As String = ""
'        ' Dim DateTimeAlertCleared As String = ""
'        Dim AlertType As String = ""
'        Dim CC As String = ""
'        Dim BCC As String = ""
'        Dim SendTo As String = ""

'        Try
'            'con = System.Configuration.ConfigurationSettings.AppSettings("vswebConnectionString")

'            '  con = New SqlConnection(ConfigurationSettings.AppSettings("ConnectionString"))
'            'COMMENT BELOW LINES
'            '================================================================================
'            '  Dim strSQL As String = "SELECT * FROM AlertDetails WHERE GETDATE() BETWEEN CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + LTRIM(RIGHT(CONVERT(VARCHAR(20), StartTime, 100), 7))) and DATEADD(MI, Duration, CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + LTRIM(RIGHT(CONVERT(VARCHAR(20), StartTime, 100), 7))))"
'            'Dim strSQL As String = "SELECT * FROM AlertDetails WHERE GETDATE() BETWEEN CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + "
'            'strSQL += " StartTime) and DATEADD(MI, Duration, CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + StartTime) )"
'            'strSQL += " or"
'            'strSQL += " GETDATE(BETWEEN)"
'            'strSQL += " DATEADD(DD,-1,CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + StartTime)) and "
'            'strSQL += " DATEADD(DD,-1,DATEADD(MI, Duration, CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + "
'            'strSQL += " StartTime)))"
'            '================================================================================


'            ''WriteServiceHistoryEntry(Now.ToString & " The VitalSigns Alert Service is executing:  " & strSQL)
'            'COMMENT BELOW LINES
'            '================================================================================
'            'Dim DA As New SqlDataAdapter(strSQL, con)
'            '================================================================================

'            'ADD BELOW LINES
'            '================================================================================
'            Dim DA As New SqlDataAdapter("GetAlertDetailsAll", con)
'            DA.SelectCommand.CommandType = CommandType.StoredProcedure
'            '================================================================================
'            Dim ds As New DataSet
'            DA.Fill(ds, "alert")
'            Dim dt As DataTable = ds.Tables(0)
'            WriteServiceHistoryEntry(Now.ToString & " " & dt.Rows.Count & " Potential Alert recipient (AlertDetails) rows were found.")
'            If dt.Rows.Count > 0 Then
'                For i As Integer = 0 To dt.Rows.Count - 1
'                    'COMMENT BELOW LINES
'                    '================================================================================
'                    'Dim days As String = dt.Rows(i)("Day").ToString()
'                    'Dim daylist As Array = days.Split(",")
'                    'For x As Integer = 0 To daylist.Length - 1
'                    '    If DateTime.Now.DayOfWeek.ToString() = Trim(daylist(x)) Then
'                    '================================================================================
'                    Dim Historytable As DataTable
'                    Try
'                        Dim SqlQuery As String = " select * from AlertHistory " &
'                         "  where" &
'                         " DateTimeAlertCleared Is null and (( " &
'                         " DeviceName not in (select srv.ServerName	 from Servers srv, AlertServers asrv where srv.ID=asrv.ServerID and asrv.AlertKey=" & dt.Rows(i)("AlertKey").ToString() & ")" &
'                         " and Location not in  " &
'                         " (select loc.Location from Locations loc, AlertLocations aloc where loc.ID=aloc.LocationID and aloc.AlertKey=" & dt.Rows(i)("AlertKey").ToString() & ")" &
'                         " and" &
'                          " DeviceType not in" &
'                         " (select srvt.ServerType from ServerTypes srvt, AlertDeviceTypes asrvt where srvt.ID=asrvt.ServerTypeID and " &
'                           "  asrvt.AlertKey = " & dt.Rows(i)("AlertKey").ToString() & "" &
'                         " and asrvt.ServerTypeID  not in " &
'                         " (select evt.ServerTypeID from EventsMaster evt, AlertEvents aevt where evt.ID=aevt.EventID and aevt.AlertKey=" & dt.Rows(i)("AlertKey").ToString() & " )" &
'                         " )" &
'                         " )" &
'                         " or" &
'                         " (" &
'                         " DeviceType in " &
'                         " (select srvt.ServerType from ServerTypes srvt, AlertDeviceTypes asrvt where " &
'                         " srvt.ID = asrvt.ServerTypeID And asrvt.AlertKey = " & dt.Rows(i)("AlertKey").ToString() & "" &
'                         " and asrvt.ServerTypeID in " &
'                         " (select evt.ServerTypeID from EventsMaster evt, AlertEvents aevt where evt.ID=aevt.EventID and aevt.AlertKey=" & dt.Rows(i)("AlertKey").ToString() & " )) " &
'                         " and" &
'                         " AlertType not in " &
'                         " (select evt.EventName from EventsMaster evt, AlertEvents aevt where evt.ID=aevt.EventID and aevt.AlertKey=" & dt.Rows(i)("AlertKey").ToString() & " )" &
'                         " ))" &
'                         " order by id"
'                        'WriteServiceHistoryEntry(Now.ToString & " Running the following command:" & vbCrLf & SqlQuery)
'                        Dim SDA As New SqlDataAdapter(SqlQuery, con)
'                        Dim SDs As New DataSet
'                        SDA.Fill(SDs, "AlertHistory")
'                        Historytable = SDs.Tables(0)
'                        ' WriteServiceHistoryEntry(Now.ToString & " The command returned " & Historytable.Rows.Count & " records.")
'                    Catch ex As ApplicationException
'                        WriteServiceHistoryEntry(Now.ToString & " Error  at the time of selecting records from Alert History Table: " & ex.Message)
'                    End Try

'                    If Not Historytable Is Nothing Then
'                        If (Historytable.Rows.Count > 0) Then
'                            For hst As Integer = 0 To Historytable.Rows.Count - 1

'                                AlertID = Historytable.Rows(hst)("ID")
'                                ServerName = Historytable.Rows(hst)("DeviceName").ToString()
'                                ServerType = Historytable.Rows(hst)("DeviceType").ToString()
'                                Location = Historytable.Rows(hst)("Location").ToString()
'                                'EventName = Historytable.Rows(hst)("EventType").ToString()
'                                AlertType = Historytable.Rows(hst)("AlertType").ToString()
'                                Details = Historytable.Rows(hst)("Details").ToString()
'                                Try
'                                    Dim sqlstm As String = "Select * from AlertSentDetails where AlertHistoryID= " & AlertID & ""
'                                    Dim DA1 As New SqlDataAdapter(sqlstm, con)
'                                    Dim DS1 As New DataSet
'                                    DA1.Fill(DS1, "AlertSentDetails")
'                                    Dim dtmail As DataTable = DS1.Tables(0)
'                                    If dtmail.Rows.Count > 0 Then
'                                        SendTo = dt.Rows(i)("SendTo").ToString()
'                                        Dim foundRows() As DataRow = dtmail.Select("SentTo like '%" & SendTo & "%'")
'                                        If foundRows.Length > 0 Then
'                                            SendTo = ""
'                                            CC = ""
'                                            BCC = ""
'                                        End If
'                                    Else

'                                        Try
'                                            SendTo = dt.Rows(i)("SendTo").ToString()
'                                            '                         'WriteServiceHistoryEntry(Now.ToString & " Sending message to " & SendTo)
'                                            If dt.Rows(i)("CopyTo").ToString() <> "" Then
'                                                CC = dt.Rows(i)("CopyTo").ToString()
'                                            End If
'                                            If dt.Rows(i)("BlindCopyTo").ToString() <> "" Then
'                                                BCC = dt.Rows(i)("BlindCopyTo").ToString()
'                                            End If

'                                        Catch ex As Exception
'                                            '                       'WriteServiceHistoryEntry(Now.ToString & " Error  at the time of read records from datatable into CC/BCC/sendto: " & ex.Message)
'                                        End Try
'                                    End If


'                                Catch ex As ApplicationException
'                                    '            'WriteServiceHistoryEntry(Now.ToString & " Error occurred at the time of getting records from  AlertSentDetails " & ex.Message)
'                                End Try

'                                If SendTo = "" And CC = "" And BCC = "" Then
'                                    '            'WriteServiceHistoryEntry(Now.ToString & " There is no new recipient list/ already mails sent to that mailIDs")
'                                Else
'                                    Try
'                                        ''WriteServiceHistoryEntry(Now.ToString & " Calling sendmail function.")
'                                        ''WriteServiceHistoryEntry(Now.ToString & " SendTo=" & SendTo)
'                                        ''WriteServiceHistoryEntry(Now.ToString & " CC=" & CC)
'                                        ''WriteServiceHistoryEntry(Now.ToString & " BCC=" & BCC)
'                                        ''WriteServiceHistoryEntry(Now.ToString & " ServerName=" & ServerName)
'                                        ''WriteServiceHistoryEntry(Now.ToString & " ServerType=" & ServerType)
'                                        ''WriteServiceHistoryEntry(Now.ToString & " Location=" & Location)
'                                        ''WriteServiceHistoryEntry(Now.ToString & " EventName=" & EventName)
'                                        ''WriteServiceHistoryEntry(Now.ToString & " AlertType=" & AlertType)
'                                        ''WriteServiceHistoryEntry(Now.ToString & " Details=" & Details)

'                                        SendMailwithChilkat(SendTo, CC, BCC, ServerName, ServerType, Location, EventName, AlertType, Details)
'                                        mailsent = True
'                                    Catch ex As ApplicationException
'                                        WriteServiceHistoryEntry(Now.ToString & " Error occurred at the time of sending Mail " & ex.Message)
'                                        mailsent = False
'                                    End Try

'                                    '***** SNMP Conditions *********
'                                    Try
'                                        'WriteServiceHistoryEntry(Now.ToString & " Entering SNMP module evaluation.")
'                                        Dim SNMPEnabled As String = ""
'                                        'SNMPEnabled = getSettings("EnableSNMP")
'                                        Dim SNMPTrap As Boolean = dt.Rows(i)("SendSNMPTrap")
'                                        If SNMPEnabled = "True" And SNMPTrap = True Then
'                                            'Dim SNMPHostName As String = getSettings("SNMPHostName")
'                                            'SendSNMPTrap(SNMPHostName, ServerType, ServerName, AlertType, Details)
'                                        End If
'                                    Catch ex As ApplicationException
'                                        'WriteServiceHistoryEntry(Now.ToString & " Error occurred at  SendSNMPTrap " & ex.Message)
'                                    End Try

'                                End If
'                                Try
'                                    If mailsent = True Then
'                                        Dim mails As String = ""

'                                        mails = SendTo & "," & CC & "," & BCC
'                                        WriteServiceHistoryEntry(Now.ToString & " Attempting to insert mail history..")
'                                        InsertingSentMails(AlertID, mails)

'                                    End If
'                                Catch ex As Exception
'                                    WriteServiceHistoryEntry(Now.ToString & " Exception while attempting to insert mail history.." & ex.ToString)
'                                End Try

'                            Next
'                        End If
'                    Else
'                        'The History Table IS nothing
'                        WriteServiceHistoryEntry(Now.ToString & " The historytable is nothing so I don't have anything to do.")
'                    End If
'                    'COMMENT BELOW LINES
'                    '================================================================================
'                    '    End If
'                    'Next
'                    '================================================================================
'                Next
'            Else
'                'No potential alert recipients were found

'                WriteServiceHistoryEntry(Now.ToString & " There are no alert recipients so I don't have anything to do.")
'            End If



'            Thread.Sleep(5000)  ' Wait a while and then do it again
'        Catch ex As ApplicationException
'            WriteServiceHistoryEntry(Now.ToString & " Error #565 at the time of Selecting records from AlertDefinitions_View1: " & ex.Message)
'        End Try
'        ' WriteServiceHistoryEntry(Now.ToString & " At the bottom of the loop.")
'    Loop


'    WriteServiceHistoryEntry(Now.ToString & " The Service Worker Thread is shutting down... ")
'    ' Signal the stopped event. 
'    'Me.stoppedEvent.Set()
'End Sub
