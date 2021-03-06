﻿Imports System.Threading
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

Partial Public Class VitalSignsPlusDomino

	Private Sub NotesMailLoop()
		Do While boolTimeToStop <> True
			Try
				If MyNotesMailProbes.Count > 0 Then
					MonitorNotesMail()
				End If
			Catch ex As Exception
				WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Exception calling MonitorNotesMail from the loop...." & ex.ToString)

			End Try
			Thread.Sleep(5000)
		Loop
	End Sub

	Private Sub MonitorNotesMail() 'This is the main sub that calls all the other ones
        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Begin loop to monitor NotesMail >>>")
		Dim MyNotesMailProbe As MonitoredItems.DominoMailProbe
        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Selecting a NotesMail Probe to monitor ***")

        Try
            MyNotesMailProbe = CType(SelectServerToMonitor(MyNotesMailProbes), MonitoredItems.DominoMailProbe)
            WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Selected " & MyNotesMailProbe.Name)
        Catch ex As Exception
            ' WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Exception selecting NotesMail Probe " & ex.ToString)
        End Try

        Try
            If Not MyNotesMailProbe Is Nothing Then

                'To do -- if source or destination is in maintenance, 

                'Mukund 10Sep14 VSPLUS-798. 
                'Commented below if condition
                'If InMaintenance("NotesMail Probe", MyNotesMailProbe.Name) = False Then
                '    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Sending NotesMail Messages.")
                '    '     MyNotesMailProbe.Status = "OK"
                '    MyNotesMailProbe.LastScan = Now
                '    SendNotesMailProbe(MyNotesMailProbe)
                'Else
                '    MyNotesMailProbe.Status = "Maintenance"
                'End If

                '------------------------------------
                'Added below lines
                Dim strSQL, strSourceServerType, strTargetServerType As String
                Dim vsAdapter As New VSAdaptor
                Dim dsNotesMailHistory As New Data.DataSet
                strSourceServerType = ""
                strTargetServerType = ""
                Try
                    strSQL = "SELECT st.ServerType FROM Servers sr, ServerTypes st where sr.ServerTypeID=st.ID and sr.ServerName='" & MyNotesMailProbe.SourceServer & "'"
                    dsNotesMailHistory.Tables.Add("NotesMailProbeHistory")
                    vsAdapter.FillDatasetAny("VitalSigns", "Servers", strSQL, dsNotesMailHistory, "NotesMailProbeHistory")
                Catch ex As Exception

                End Try

                Dim dt As DataTable = dsNotesMailHistory.Tables("NotesMailProbeHistory")
                If dt.Rows.Count > 0 Then
                    strSourceServerType = dt.Rows(0)("ServerType").ToString()
                End If

                Try
                    strSQL = "SELECT st.ServerType FROM Servers sr, ServerTypes st where sr.ServerTypeID=st.ID and sr.ServerName='" & MyNotesMailProbe.TargetServer & "'"

                    'The following line triggers an exception as the Table was already added above
                    ' dsNotesMailHistory.Tables.Add("NotesMailProbeHistory")
                    vsAdapter.FillDatasetAny("VitalSigns", "Servers", strSQL, dsNotesMailHistory, "NotesMailProbeHistory")
                    dt = dsNotesMailHistory.Tables("NotesMailProbeHistory")
                    If dt.Rows.Count > 0 Then
                        strTargetServerType = dt.Rows(0)("ServerType").ToString()
                    End If
                    dt.Dispose()
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", "Exception determining source server type: " & ex.ToString)
                End Try


                Try
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", "Checking to see if the selected NotesMail probe is in maintenance.", LogLevel.Verbose)
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", "The NotesMail probe source server is " & MyNotesMailProbe.SourceServer, LogLevel.Verbose)
                    If InMaintenance(strSourceServerType, MyNotesMailProbe.SourceServer) = True Or InMaintenance(strTargetServerType, MyNotesMailProbe.TargetServer) = True Then
                        MyNotesMailProbe.Status = "Maintenance"
                        MyNotesMailProbe.StatusCode = "Maintenance"
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", "The selected NotesMail probe is in maintenance.")
                    Else
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Sending NotesMail Messages.")
                        '     MyNotesMailProbe.Status = "OK"
                        MyNotesMailProbe.LastScan = Now
                        SendNotesMailProbe(MyNotesMailProbe)
                    End If
                    '------------------------------------
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", "Exception determining if in maintenance: " & ex.ToString)
                    SendNotesMailProbe(MyNotesMailProbe)
                End Try

                MyNotesMailProbe.LastScan = Now
            Else
                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " No NotesMail Messages due for sending***")
            End If
        Catch ex As Exception

        End Try

		Try
			MyNotesMailProbe = Nothing
		Catch ex As Exception

		End Try


		Try
			Thread.Sleep(30000)
			WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Checking Delivery of NotesMail Messages.")
			CheckNotesMailDeliveryTimes()
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error Checking Delivery of NotesMail Messages: " & ex.Message)
		End Try

		Try
			Thread.Sleep(5000)
			WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Checking for obsolete NotesMail Messages.")
			DeleteObsoleteNotesMailProbes()
		Catch ex As Exception

		End Try

		Try
			WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Updating Status Table with NotesMail Probe information.")
			UpdateNotesMailStatusTable()
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error Updating Status Table with NotesMail Probe information: " & ex.Message)
		End Try
		GC.Collect()
	End Sub

    Private Sub SendNotesMailProbe(ByRef MyNotesMailProbe As MonitoredItems.DominoMailProbe)
        'This function sends a NotesMail message to a test mailbox
        Dim strResponse, StatusDetails As String
        Dim Percent As Double = 100
        Dim strSQL As String
        Dim myKey As String

        WriteDeviceHistoryEntry("All", "NotesMail Probes", vbCrLf & vbCrLf)
        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Sending out the next available NotesMail Probe.")
        If MyNotesMailProbe Is Nothing Then Exit Sub

        Try
            If MyNotesMailProbe.Enabled = False Then
                With MyNotesMailProbe
                    .Status = "Disabled"
                    .ResponseDetails = "Monitoring is disabled for this NotesMail Probe."
                End With

                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Skipping " & MyNotesMailProbe.Name & " because it is disabled for scanning.")

                Exit Sub
            End If
            WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Probe module is sending  a message to " & MyNotesMailProbe.Name)

        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Monitor error: " & ex.Message)
        End Try


        Select Case MyNotesMailProbe.SourceServer
            Case "SMTP"
                'Send the message via an SMTP server
                Dim myRegistry As New RegistryHandler
                Dim mailman As New Chilkat.MailMan

                ' mailman.UnlockComponent("UAForbeMAIL_JtAVMAaSDDkR")
                mailman.UnlockComponent("MZLDADMAILQ_8nzv7Kxb4Rpx")
                With mailman
                    Try
                        .SmtpHost = myRegistry.ReadFromRegistry("SMTP Host")
                        '  WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Ready to send message via: " & .SmtpHost)
                        ' .Host = "smtp.comcast.net"
                        If CType(myRegistry.ReadFromRegistry("SMTP Authentication"), Boolean) = True Then
                            .SmtpUsername = myRegistry.ReadFromRegistry("SMTP Username")
                            .SmtpPassword = Trim(MySMTPPassword)
                            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " SMTP Host requires authentication. ")
                        End If
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error configuring SMTP server for sending message:  " & ex.Message)
                    End Try
                End With

                'Unique identifier for the message
                myKey = "NotesMail Probe Key=" & Now.Ticks.ToString & " (" & MyNotesMailProbe.Name & ")"

                Try
                    Dim email As New Chilkat.Email
                    Dim myBody As String
                    With MyNotesMailProbe
                        myBody = "This is a test message used to verify NotesMail delivery.  Do not delete. " & vbCrLf
                        myBody += vbCrLf & " Sent: " & Now.ToLongDateString & " at " & Now.ToShortTimeString
                        myBody += vbCrLf & ", Due no later than: " & Now.AddMinutes(.DeliveryThreshold)
                        myBody += vbCrLf & " Sent via: " & .SourceServer
                        email.Body = myBody
                        email.Subject = myKey

                        email.AddTo("Mail Probe", MyNotesMailProbe.NotesMailAddress)
                        email.From = MyNotesMailProbe.eMailAddress
                        email.ReplyTo = "Do Not Reply"
                    End With

                    ' Send mail.
                    Dim success As Boolean
                    success = mailman.SendEmail(email)
                    If success Then
                        '      WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Sent mail!")
                        'MyNotesMailProbe.Status = "Sent"
                        '  MyNotesMailProbe.LastScan = Now.ToString
                        MyNotesMailProbe.ResponseDetails = "Sent test message was via " & mailman.SmtpHost & " at " & Now.ToShortTimeString
                    Else
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error sending mail probe: " & mailman.LastErrorText)
                        MyNotesMailProbe.ResponseDetails = "Unable to send test message via " & mailman.SmtpHost & " because  " & mailman.LastErrorText
                        MyNotesMailProbe.Status = "Failed"
                        '     MyNotesMailProbe.LastScan = Now.ToString
                    End If

                    email = Nothing
                    success = False
                    myBody = Nothing

                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error sending mail " & ex.Message)
                End Try

                mailman.Dispose()
                myRegistry = Nothing

            Case Else
                '*********** Domino COM objects, must be released at end of SUB

                Dim docMail As Domino.NotesDocument
                Dim MailBox As Domino.NotesDatabase
                Dim rtItem As Domino.NotesRichTextItem
                Dim embedded As Domino.NotesEmbeddedObject
                Dim myLocalDb As Domino.NotesDatabase


                'Send message via Domino server
                '4/29/2016 NS modified for VSPLUS-2892
                Try
                    MailBox = NotesSession.GetDatabase(MyNotesMailProbe.SourceServer, "mail.box", False)
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Successfully connected to mail.box database on " & MyNotesMailProbe.SourceServer & ".", LogUtilities.LogUtils.LogLevel.Verbose)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error Connecting to mail.box database on " & MyNotesMailProbe.SourceServer & ". " & ex.Message)
                End Try

                Try
                    If MailBox Is Nothing Then
                        MailBox = NotesSession.GetDatabase(MyNotesMailProbe.SourceServer, "mail1.box", False)
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Successfully connected to mail1.box database on " & MyNotesMailProbe.SourceServer & ".", LogUtilities.LogUtils.LogLevel.Verbose)
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error Connecting to mail1.box database on " & MyNotesMailProbe.SourceServer & ". " & ex.Message)
                End Try

                Try
                    If MailBox Is Nothing Then
                        MailBox = NotesSession.GetDatabase(MyNotesMailProbe.SourceServer, "mail2.box", False)
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Successfully connected to mail2.box database on " & MyNotesMailProbe.SourceServer & ".", LogUtilities.LogUtils.LogLevel.Verbose)
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error Connecting to mail2.box database on " & MyNotesMailProbe.SourceServer & ". " & ex.Message)
                End Try

                If MailBox Is Nothing Then
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMailProbe Mail probe could not connect to mail.box file on " & MyNotesMailProbe.SourceServer)
                    MyNotesMailProbe.Status = "Failed"
                    myAlert.QueueAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Failure", "NotesMailProbe Mail probe could not connect to mail.box file on " & MyNotesMailProbe.SourceServer, MyNotesMailProbe.Location)
                    MyNotesMailProbe.ResponseDetails = " NotesMailProbe Mail probe could not connect to mail.box file on " & MyNotesMailProbe.SourceServer
                    GoTo Cleanup
                End If


                Try
                    myLocalDb = NotesSession.GetDatabase("", "AlertSend.NSF", False)
                Catch ex As Exception
                    '   WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error Connecting to Local Alerts database " & ex.Message)
                End Try

                Try
                    If myLocalDb Is Nothing Then
                        Dim dir As Domino.NotesDbDirectory
                        Dim db As Domino.NotesDatabase
                        dir = NotesSession.GetDbDirectory("")
                        db = dir.CreateDatabase("AlertSend.NSF", True)
                        db.Title = ProductName & " Service Alerts"
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(dir)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(db)
                        myLocalDb = NotesSession.GetDatabase("", "AlertSend.NSF", False)
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error Creating to Local Alerts database " & ex.Message)
                End Try


                Try
                    docMail = myLocalDb.CreateDocument
                    docMail.Save(False, False)
                    myKey = "NotesMail Probe Key=" & docMail.UniversalID & " (" & MyNotesMailProbe.Name & ")"

                    '   docMail = MailBox.CreateDocument
                    If docMail Is Nothing Then
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Warning: NotesMailProbe could not create test message on " & MyNotesMailProbe.SourceServer)
                        MyNotesMailProbe.Status = "Failed"
                        MyNotesMailProbe.ResponseDetails = "Could not create test message."
                        myAlert.QueueAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Failure", "NotesMailProbe Mail probe could create the test message on the VitalSigns monitoring station. ", MyNotesMailProbe.Location)
                        GoTo Cleanup
                    End If

                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Sending out NotesMail Probe with subject line of  " & myKey & " to " & MyNotesMailProbe.NotesMailAddress)
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Sending out NotesMail Probe with ReplyTo of  " & myKey & " to " & MyNotesMailProbe.ReplyTo)
                    Dim myBody As String
                    With MyNotesMailProbe
                        Try
                            docMail.ReplaceItemValue("SendTo", .NotesMailAddress)
                            docMail.ReplaceItemValue("Recipients", .NotesMailAddress)
                            ' docMail.ReplaceItemValue("CopyTo", "Alan Forbes")
                            docMail.ReplaceItemValue("Subject", myKey)
                            docMail.ReplaceItemValue("From", NotesSession.CommonUserName)
                            ' docMail.ReplaceItemValue("From", .eMailAddress)
                            If .ReplyTo <> "" Then
                                docMail.ReplaceItemValue("ReplyTo", .ReplyTo)
                                docMail.ReplaceItemValue("Principal", .ReplyTo)
                            End If

                        Catch ex As Exception

                        End Try

                        Try
                            myBody = "This is a test message used to verify NotesMail delivery.  Do not delete. " & vbCrLf
                            myBody += vbCrLf & "Sent: " & Now.ToLongDateString & " at " & Now.ToShortTimeString
                            myBody += vbCrLf & "Due no later than: " & Now.AddMinutes(.DeliveryThreshold)
                            myBody += vbCrLf & " Sent via: " & .SourceServer & vbCrLf & vbCrLf
                            myBody += vbCrLf & "The message key was " & myKey
                        Catch ex As Exception

                        End Try

                        Try
                            'Attach the file, if any
                            If MyNotesMailProbe.FileName <> "" Then
                                rtItem = docMail.CreateRichTextItem("Body")
                                rtItem.AppendText(myBody)
                                rtItem.AppendText("Attachments follow:" & vbCrLf)
                                embedded = rtItem.EmbedObject(Domino.EMBED_TYPE.EMBED_ATTACHMENT, "", MyNotesMailProbe.FileName)
                            Else
                                rtItem = docMail.CreateRichTextItem("Body")
                                rtItem.AppendText(myBody)
                                'docMail.ReplaceItemValue("Body", myBody)
                            End If

                        Catch ex As Exception

                        End Try

                        Try
                            docMail.ReplaceItemValue("Principal", ProductName)
                            docMail.ReplaceItemValue("PostedDate", Now.ToString)
                        Catch ex As Exception

                        End Try

                        Try
                            docMail.Save(False, False)
                            docMail.CopyToDatabase(MailBox)
                            '5/6/2016 NS uncommented out status setting below
                            MyNotesMailProbe.Status = "Sent"
                            .ResponseDetails = "Sent test message via mailbox on " & .SourceServer & " at " & Now.ToShortTimeString
                            docMail.Remove(True)
                        Catch ex As Exception
                            WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error at save/copytodatabase in SendNotesMailProbe. Error: " & ex.Message.ToString())
                            MyNotesMailProbe.Status = "Failure on Sending"
                            .ResponseDetails = "Test message could not be sent due to error: " & ex.ToString
                            myAlert.QueueAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Failure", "NotesMailProbe Mail probe could not transfer the test message to the mail.box file on " & MyNotesMailProbe.SourceServer, MyNotesMailProbe.Location)

                        End Try
                    End With


                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Monitor error creating message: " & ex.Message)
                    MyNotesMailProbe.Status = "Failed"
                    MyNotesMailProbe.ResponseDetails = "Error sending the test message " & ex.Message
                    myAlert.QueueAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Failure", "NotesMailProbe Mail probe could not create test message " & ex.ToString, MyNotesMailProbe.Location)

                Finally

                    strResponse = Nothing
                    StatusDetails = Nothing
                    Percent = Nothing
                End Try

Cleanup:
                Try
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(docMail)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " SendNotesMailProbe Module Error releasing Notes objects at 3: " & ex.Message)
                End Try
                Try
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(MailBox)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " SendNotesMailProbe Module Error releasing Notes objects at 4: " & ex.Message)
                End Try

                Try
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(myLocalDb)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " SendNotesMailProbe Module Error releasing Notes objects at 4: " & ex.Message)
                End Try
                Try
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(rtItem)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " SendNotesMailProbe Module Error releasing Notes objects at 5: " & ex.Message)
                End Try

                Try
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(embedded)
                Catch ex As Exception
                    '   WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" SendNotesMailProbe Module Error releasing Notes objects at 6: " & ex.Message)
                End Try

        End Select

        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Monitor  is opening the history database")
        'Record the details of this sent message in the NotesMail History table


        '     WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" NotesMail Monitor  says History table is " & OleDbConnectionServers.State.ToString)

        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.NotesMailProbeHistory)(connectionString)
            Dim entity As New VSNext.Mongo.Entities.NotesMailProbeHistory() With {
                .SentDateTime = Now.ToString(),
                .SentTo = MyNotesMailProbe.NotesMailAddress,
                .DeliveryThresholdMinutes = MyNotesMailProbe.DeliveryThreshold,
                .SubjectKey = myKey,
                .DeviceID = MyNotesMailProbe.ServerObjectID,
                .DeviceName = MyNotesMailProbe.Name,
                .Status = "Sent",
                .TargetServer = MyNotesMailProbe.TargetServer,
                .TargetDatabase = MyNotesMailProbe.TargetDatabase
            }
            repository.Insert(entity)


        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error inserting NotesMail History table with NotesMail Probe info: " & ex.Message)
        End Try

        strSQL = Nothing
        myKey = Nothing

    End Sub

    Public Sub CheckNotesMailDeliveryTimes()
		'This function checks sent NotesMail probes and updates their status
		WriteDeviceHistoryEntry("All", "NotesMail Probes", vbCrLf & vbCrLf)
		WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & "***********  Checking for NotesMail Delivery")
		WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Count of NotesMailProbe Collection: " & MyNotesMailProbes.Count)
		Dim strSubject, strSQL As String
		Dim intDeliveryThreshold As Integer
		Dim MyNotesMailProbe As MonitoredItems.DominoMailProbe
		Dim DeliveryTime As TimeSpan

		Dim tNow As DateTime
		tNow = Now
		Dim tSentDate, tDeliveryDate, tScheduled As DateTime
        Dim boolFoundMemo As Boolean

        '6/22/2015 NS added for VSPLUS-1475
        Dim NotesSystemMessageString As String = "Incorrect Notes Password."
		'Domino Objects
		Dim myLocalSession As New Domino.NotesSession
		Try
			myLocalSession.Initialize(MyDominoPassword)
        Catch ex As Exception
            '6/22/2015 NS added for VSPLUS-1475
            System.Runtime.InteropServices.Marshal.ReleaseComObject(myLocalSession)
            myAlert.QueueSysMessage(NotesSystemMessageString)
            WriteAuditEntry(Now.ToString & " Error Initializing NotesSession.  Many problems will follow....  " & ex.ToString)
            WriteAuditEntry(Now.ToString & " *********** ERROR ************  Error initializing a session to Query Domino server. ")
            WriteAuditEntry(Now.ToString & " Calling stopnotescl.exe then exiting in an attempt to recover.")
            WriteAuditEntry(Now.ToString & " The VitalSigns Master service should restart the monitoring service in a few moments.")
            KillNotes()
            Exit Sub
		End Try

		Dim boolDeleteMe As Boolean = False
		Dim docMail, docPrevious As Domino.NotesDocument
		Dim MailBox As Domino.NotesDatabase
		Dim viewInbox As Domino.NotesView
		Dim navInBox As Domino.NotesViewNavigator
		Dim entryMail As Domino.NotesViewEntry

		Dim vsAdapter As New VSAdaptor

		Dim dsNotesMailHistory As New Data.DataSet
		Dim dv As DataView

        Dim listOfEntities As New List(Of VSNext.Mongo.Entities.NotesMailProbeHistory)()

        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.NotesMailProbeHistory)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.NotesMailProbeHistory) = repository.Filter.Eq(Function(x) x.ArrivalAtMailbox, Nothing)
            listOfEntities = repository.Find(filterDef).ToList()
        Catch ex As Exception

        End Try

        Try
            listOfEntities = listOfEntities.Where(Function(x) x.Status = "Sent").ToList()

            WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Check NotesMail Delivery Times is examining " & listOfEntities.Count & " entities in List.")
            For Each entity As VSNext.Mongo.Entities.NotesMailProbeHistory In listOfEntities
                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Probe will check delivery on " & entity.DeviceName)
            Next

            If listOfEntities.Count = 0 Then
                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " ** No NotesMail Probes to check, exiting")

                Try
                    tNow = Nothing
                    tSentDate = Nothing
                    tDeliveryDate = Nothing
                    tScheduled = Nothing
                    DeliveryTime = Nothing
                    MyNotesMailProbe = Nothing
                    strSubject = Nothing
                    strSQL = Nothing
                    intDeliveryThreshold = Nothing
                    boolFoundMemo = Nothing
                    listOfEntities = Nothing
                Catch ex As Exception

                End Try

                Try
                    GoTo Cleanup
                Catch ex As Exception

                End Try
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Exception getting list of entities in CheckNotesMailDeliveryTimes. Error:" & ex.Message)
        End Try

        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.NotesMailProbeHistory)(connectionString)
            For Each entity As VSNext.Mongo.Entities.NotesMailProbeHistory In listOfEntities
                boolFoundMemo = False
                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Looping through row ")

                Try
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " searching in row " & entity.DeviceName & " with status of " & entity.Status)
                    MyNotesMailProbe = MyNotesMailProbes.Search(entity.DeviceName)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " error while searching " & ex.Message)
                End Try

                Try
                    tSentDate = entity.SentDateTime.Value
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Message was sent: " & tSentDate.ToLongTimeString, LogLevel.Verbose)
                    intDeliveryThreshold = entity.DeliveryThresholdMinutes.Value
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Message has " & intDeliveryThreshold.ToString & " minutes.", LogLevel.Verbose)
                    tScheduled = tSentDate.AddMinutes(intDeliveryThreshold)
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Message must arrive before: " & tScheduled.ToLongTimeString, LogLevel.Verbose)
                    'See if message arrived
                    MailBox = myLocalSession.GetDatabase(entity.TargetServer, entity.TargetDatabase, False)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Could not connect to: " & entity.TargetServer & ":" & entity.TargetDatabase & ". Exception : " & ex.Message.ToString())
                End Try


                If MailBox Is Nothing Then
                    'Can't connect to server, see if deadline has past
                    If DateTime.Compare(tNow, tScheduled) > 0 Then
                        'deadline has past, document has not arrived.
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Could not connect to: " & entity.TargetServer & ":" & entity.TargetDatabase)
                        MyNotesMailProbe.IncrementDownCount()
                        MyNotesMailProbe.Status = "Not Responding"
                        MyNotesMailProbe.ResponseDetails = "Cannot connect to target Notes database: " & entity.TargetServer & ":" & entity.TargetDatabase

                        entity.Status = "Not Responding"
                        entity.Details = "Unable to open target server"
                        repository.Replace(entity)

                        myAlert.QueueAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Failure", "NotesMailProbe Mail probe could not connect to target Notes database: " & entity.TargetServer & ":" & entity.TargetDatabase, MyNotesMailProbe.Location)

                        'Update statistics Table
                        UpdateNotesMailStatistics(MyNotesMailProbe, "DeliveryTime.Seconds", 0)
                    End If

                Else
                    If String.IsNullOrWhiteSpace(MailBox.Title) Then
                        MailBox.Title = "Untitled Database"
                    End If
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Successfully connected to mailbox: " & MailBox.Title)
                    viewInbox = MailBox.GetView("($Inbox)")
                    If viewInbox Is Nothing Then
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Probe module could not connect to Inbox in " & entity.TargetServer & ":" & entity.TargetDatabase)
                        '   Exit Try
                    Else
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Probe module connected to target InBox")
                        navInBox = viewInbox.CreateViewNav
                    End If

                    ' docMail = Inbox.GetFirstDocument
                    entryMail = navInBox.GetLastDocument
                    If entryMail Is Nothing Then
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Unable to connect to last document Inbox")
                        '   Exit Try
                    Else
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Connected to the last document in Inbox, searching up", LogLevel.Verbose)
                    End If
                End If

                Dim counter As Integer = 0  'This will store the total number of documents examined
                Dim DeleteCounter As Integer = 0   'This will store the total number of probes found that match the name
                Dim SearchText As String = entity.SubjectKey

                Dim oWatch As New System.Diagnostics.Stopwatch
                oWatch.Start()
                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Probe history is now searching for " & SearchText)



                While Not entryMail Is Nothing
                    'The counter should help prevent from going through huge inboxes over and over
                    counter += 1
                    If counter > 100 Then Exit While

                    boolDeleteMe = False
                    docMail = entryMail.Document
                    strSubject = docMail.GetItemValue("Subject")(0)
                    ' WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Probe history is now searching the Target Inbox... examining # " & counter.ToString & " - " & strSubject, LogLevel.Verbose)
                    If InStr(strSubject, SearchText) Then
                        'Found the document
                        boolFoundMemo = True
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Probe history found Target Document, it was delivered at: " & docMail.GetItemValue("DeliveredDate")(0))
                        tDeliveryDate = CType(docMail.GetItemValue("DeliveredDate")(0), DateTime)
                        DeliveryTime = tDeliveryDate.Subtract(tSentDate)
                        MyNotesMailProbe.ResponseTime = Math.Abs(DeliveryTime.TotalMinutes)
                        Dim MyResponse As String
                        MyResponse = " Successfully found the target document which was delivered within the target time. Elapsed time = " & DeliveryTime.TotalSeconds.ToString("F1") & " seconds."
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " " & MyNotesMailProbe.Name & " was delivered in " & MyResponse, LogLevel.Verbose)
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " " & MyNotesMailProbe.Name & " was delivered at " & tDeliveryDate.ToString, LogLevel.Verbose)
                        If DateTime.Compare(CType(docMail.GetItemValue("DeliveredDate")(0), DateTime), tScheduled) < 0 Then
                            'date is in future
                            '   WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Delivered on time")

                            myAlert.ResetAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Failure", MyNotesMailProbe.Location)

                            entity.Status = "OK"
                            entity.DeliveryTimeMinutes = MyNotesMailProbe.ResponseTime
                            entity.ArrivalAtMailbox = DateTime.Parse(docMail.GetItemValue("DeliveredDate")(0))
                            entity.Details = MyResponse

                            MyNotesMailProbe.IncrementUpCount()
                            MyNotesMailProbe.Status = "OK"
                            MyNotesMailProbe.ResponseDetails = " Found the test message in the Target database, it was delivered on time at: " & docMail.GetItemValue("DeliveredDate")(0)
                            myAlert.ResetAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Failure", MyNotesMailProbe.Location)
                            myAlert.ResetAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Late", MyNotesMailProbe.Location)
                            'Update Stats Table
                            Dim MyResponseTime As Integer
                            MyResponseTime = Math.Abs(DeliveryTime.Seconds)
                            '        WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" MyResponseTime=" & DeliveryTime.Seconds)
                            If MyResponseTime = 0 Then
                                '   WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Somehow thought it was zero....")
                                'If delivered in less than a second, set it to 1, to distinguish from failures, which get 0
                                MyResponseTime = 1
                            End If
                            UpdateNotesMailStatistics(MyNotesMailProbe, "DeliveryTime.Seconds", MyResponseTime)
                            MyResponseTime = Nothing

                        Else
                            '      WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Delivered LATE")

                            entity.Status = "Late"
                            entity.DeliveryTimeMinutes = MyNotesMailProbe.ResponseTime
                            entity.ArrivalAtMailbox = DateTime.Parse(docMail.GetItemValue("DeliveredDate")(0))
                            entity.Details = "Delivered LATE in " & MyResponse

                            MyNotesMailProbe.IncrementDownCount()
                            myAlert.QueueAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Slow", "NotesMailProbe found the test message in the Target database, but it was delivered late at: " & docMail.GetItemValue("DeliveredDate")(0), MyNotesMailProbe.Location)

                            MyNotesMailProbe.Status = "Delivered Late"
                            MyNotesMailProbe.ResponseDetails = " Found the test message in the Target database, but it was delivered late at: " & docMail.GetItemValue("DeliveredDate")(0)
                            myAlert.QueueAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Slow", MyNotesMailProbe.ResponseDetails, MyNotesMailProbe.Location)
                            'Update Stats Table
                            UpdateNotesMailStatistics(MyNotesMailProbe, "DeliveryTime.Seconds", Math.Abs(DeliveryTime.Seconds))
                        End If

                        'Update the history table
                        'myCommand.CommandText = strSQL
                        'myCommand.ExecuteNonQuery()
                        repository.Replace(entity)
                        Exit While
                    Else
                        'WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " This email is not a match.", LogLevel.Verbose)
                        'WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Checking to see if it is an old probe... if the subject contains " & row("DeviceName"), LogLevel.Verbose)
                        'WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Subject is " & strSubject, LogLevel.Verbose)
                        'If InStr(strSubject, "NotesMail Probe Key=") And InStr(strSubject, row("DeviceName")) Then
                        '    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " This message should be deleted.", LogLevel.Verbose)
                        'End If
                    End If

                    Try
                        'You can't delete a document then use it to get to the next document, so here I mark the previous document for deletion then go back and get it

                        If InStr(strSubject, "NotesMail Probe Key=") And InStr(strSubject, entity.DeviceName) Then
                            WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Deleting Obsolete NotesMail document " & strSubject)
                            '  docMail.Remove(True)
                        End If
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Exception deleting obsolete NotesMail document " & strSubject)
                    End Try

                    ' docPrevious = docMail
                    'docMail = Inbox.GetNextDocument(docMail)
                    entryMail = navInBox.GetPrevDocument(entryMail)

                End While


                oWatch.Stop()

                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & "  Searched the inbox and examined " & counter & " documents. ")
                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & "  Searched the inbox for " & oWatch.Elapsed.TotalMinutes.ToString("F1") & " minutes.")
                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & "  Searched the inbox for " & oWatch.Elapsed.TotalSeconds.ToString("F1") & " seconds.")
                'Document was not found in the target database if boolFoundMemo = False
                'see if the delivery deadline has past
                If boolFoundMemo = False Then
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Doc not found, checking deadline." & "It is now " & tNow.ToString & " and deadline was " & tScheduled.ToString)
                    If DateTime.Compare(tNow, tScheduled) > 0 Then
                        'deadline has past, document has not arrived.
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Deadline expired, mail has not arrived")
                        MyNotesMailProbe.IncrementDownCount()
                        MyNotesMailProbe.Status = "Failed"
                        MyNotesMailProbe.ResponseDetails = "The test message was not found in the target database."
                        myAlert.QueueAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Failure", MyNotesMailProbe.ResponseDetails, MyNotesMailProbe.Location)

                        entity.Status = "Failed"
                        entity.Details = "Memo not found"
                        repository.Replace(entity)

                    End If
                Else
                    'deadline not expired yet
                    '  WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Deadline has not expired. ")
                End If
            Next

        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error in CheckNotesMailDeliveryTimes module:" & ex.Message & vbCrLf & strSQL)

        End Try

Cleanup:


		Try
            listOfEntities = Nothing
		Catch ex As Exception

		End Try

		Try
			If MailBox IsNot Nothing Then
				System.Runtime.InteropServices.Marshal.ReleaseComObject(MailBox)
			End If

		Catch ex As Exception
			'WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error in releasing Mailbox in CheckNotesMailDeliveryTimes module:" & ex.Message)
		End Try

		Try
			If viewInbox IsNot Nothing Then
				System.Runtime.InteropServices.Marshal.ReleaseComObject(viewInbox)
			End If

		Catch ex As Exception
			' WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error in releasing inBox in CheckNotesMailDeliveryTimes module:" & ex.Message)
		End Try


		Try
			If navInBox IsNot Nothing Then
				System.Runtime.InteropServices.Marshal.ReleaseComObject(navInBox)
			End If

		Catch ex As Exception
			' WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error in releasing inBox in CheckNotesMailDeliveryTimes module:" & ex.Message)
		End Try

		Try
			If entryMail IsNot Nothing Then
				System.Runtime.InteropServices.Marshal.ReleaseComObject(entryMail)
			End If

		Catch ex As Exception
			' WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error in releasing inBox in CheckNotesMailDeliveryTimes module:" & ex.Message)
		End Try


		Try
			If docMail IsNot Nothing Then
				System.Runtime.InteropServices.Marshal.ReleaseComObject(docMail)
			End If

			If docPrevious IsNot Nothing Then
				System.Runtime.InteropServices.Marshal.ReleaseComObject(docPrevious)
			End If

		Catch ex As Exception
			'    WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Error in releasing docMail in CheckNotesMailDeliveryTimes module:" & ex.Message)
		End Try

		Try
			If myLocalSession IsNot Nothing Then
				System.Runtime.InteropServices.Marshal.ReleaseComObject(myLocalSession)
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error in releasing mySession in CheckNotesMailDeliveryTimes module:" & ex.Message)

		End Try


	End Sub


    Public Sub CheckNotesMailDeliveryTimesUsingIMAP()
        'This function checks sent NotesMail probes and updates their status
        WriteDeviceHistoryEntry("All", "NotesMail Probes", vbCrLf & vbCrLf)
        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & "***********  Checking for NotesMail Delivery")
        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Count of NotesMailProbe Collection: " & MyNotesMailProbes.Count)
        Dim strSubject, strSQL As String
        Dim intDeliveryThreshold As Integer
        Dim MyNotesMailProbe As MonitoredItems.DominoMailProbe
        Dim DeliveryTime As TimeSpan

        Dim tNow As DateTime
        tNow = Now
        Dim tSentDate, tDeliveryDate, tScheduled As DateTime
        Dim boolFoundMemo As Boolean


        Dim boolDeleteMe As Boolean = False
        Dim listOfEntities As New List(Of VSNext.Mongo.Entities.NotesMailProbeHistory)()

        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.NotesMailProbeHistory)(connectionString)
            Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.NotesMailProbeHistory) = repository.Filter.Eq(Function(x) x.ArrivalAtMailbox, Nothing)
            listOfEntities = repository.Find(filterDef).ToList()
        Catch ex As Exception

        End Try

        Try
            listOfEntities = listOfEntities.Where(Function(x) x.Status = "Sent").ToList()

            WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Check NotesMail Delivery Times is examining " & listOfEntities.Count & " entities in List.")
            For Each entity As VSNext.Mongo.Entities.NotesMailProbeHistory In listOfEntities
                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Probe will check delivery on " & entity.DeviceName)
            Next

            If listOfEntities.Count = 0 Then
                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " ** No NotesMail Probes to check, exiting")

                Try
                    tNow = Nothing
                    tSentDate = Nothing
                    tDeliveryDate = Nothing
                    tScheduled = Nothing
                    DeliveryTime = Nothing
                    MyNotesMailProbe = Nothing
                    strSubject = Nothing
                    strSQL = Nothing
                    intDeliveryThreshold = Nothing
                    boolFoundMemo = Nothing
                    listOfEntities = Nothing
                Catch ex As Exception

                End Try

                Try
                    GoTo Cleanup
                Catch ex As Exception

                End Try
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Exception getting list of entities in CheckNotesMailDeliveryTimes. Error:" & ex.Message)
        End Try

        Try
            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.NotesMailProbeHistory)(connectionString)
            For Each entity As VSNext.Mongo.Entities.NotesMailProbeHistory In listOfEntities
                boolFoundMemo = False
                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Looping through row ")

                Try
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " searching in row " & entity.DeviceName & " with status of " & entity.Status)
                    MyNotesMailProbe = MyNotesMailProbes.Search(entity.DeviceName)
                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " error while searching " & ex.Message)
                End Try

                Try
                    tSentDate = entity.SentDateTime.Value
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Message was sent: " & tSentDate.ToLongTimeString, LogLevel.Verbose)
                    intDeliveryThreshold = entity.DeliveryThresholdMinutes.Value
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Message has " & intDeliveryThreshold.ToString & " minutes.", LogLevel.Verbose)
                    tScheduled = tSentDate.AddMinutes(intDeliveryThreshold)
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Message must arrive before: " & tScheduled.ToLongTimeString, LogLevel.Verbose)

                Catch ex As Exception
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Could not connect to: " & entity.TargetServer & ":" & entity.TargetDatabase & ". Exception : " & ex.Message.ToString())
                End Try


                '*********** New code for IMAP
                Dim imap As New Chilkat.Imap

                '  Anything unlocks the component and begins a fully-functional 30-day trial.
                Dim success As Boolean = imap.UnlockComponent("MZLDADIMAPMAILQ_pVLoRztR0R2C")

                If (success <> True) Then
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Could not unlock Chilkat component: " & imap.LastErrorText)
                    Exit Sub
                End If


                '  Connect to an IMAP server.
                '  Use TLS
                imap.Ssl = True
                imap.Port = 993
                success = imap.Connect(entity.TargetServer)
                If (success <> True) Then
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Could not connect to target server " & entity.TargetServer & " " & imap.LastErrorText)

                    Exit Sub
                End If


                '  Login
                success = imap.Login("admin@chilkatsoft.com", "myPassword")
                If (success <> True) Then
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Could not log into IMAP server: " & imap.LastErrorText)
                    Exit Sub
                End If


                '  Select an IMAP mailbox
                success = imap.SelectMailbox("Inbox")
                If (success <> True) Then
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Could not connect to the Inbox: " & imap.LastErrorText)

                    Exit Sub
                End If



                Dim messageSet As Chilkat.MessageSet
                '  We can choose to fetch UIDs or sequence numbers.
                Dim fetchUids As Boolean = True

                '  Here are examples of different search criteria:

                '  Return all messages.
                Dim allMsgs As String = "ALL"

                '  Search for already-answered emails.
                Dim answered As String = "ANSWERED"

                '  Search for messages on a specific date.
                '  The date string is DD-Month-YYYY where Month is
                '  Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, or Dec.
                Dim onDate As String = "SENTON 05-Mar-2007"

                '  Search for messages between two dates.  SENTBEFORE
                '  finds emails sent before a date, and SENTSINCE finds
                '  email sent on or after a date.  The "AND" operation
                '  is implied by joining criteria, separated by spaces.
                Dim betweenDates As String = "SENTSINCE 01-Mar-2007 SENTBEFORE 05-Mar-2007"

                '  Another example of AND: find all unanswered emails
                '  sent after 04-Mar-2007 with "Problem" in the subject:
                Dim complexSearch1 As String = "UNANSWERED SENTSINCE 04-Mar-2007 Subject ""Problem"""

                '  Find messages with a specific string in the body:
                Dim bodySearch As String = "BODY ""problem solved"""

                '  Using OR.  The syntax is OR <criteria1> <criteria2>.
                '  The "OR" comes first, followed by each criteria.
                '  For example, to match all emails with "Help" or "Question" in the subject.
                '  You'll notice that literal strings may be quoted or unquoted.
                '  If a literal contains SPACE characters, quote it:
                Dim orSearch As String = "OR SUBJECT Help SUBJECT Question"

                '  ----------------------------------------------
                '  Strings are case-insensitive when searching....
                '  ----------------------------------------------

                '  Find all emails sent from yahoo.com addresses:
                Dim fromSearch As String = "FROM yahoo.com"
                '  Find all emails sent from anyone with "John" in their name:
                Dim johnSearch As String = "FROM John"

                '  Find emails with the RECENT flag set:
                Dim recentSearch As String = "RECENT"

                '  Find emails that don't have the recent flag set:
                Dim notRecentSearch As String = "NOT RECENT"
                '  This is synonymous with "OLD":
                Dim oldSearch As String = "OLD"

                '  Find all emails marked for deletion:
                Dim markedForDeleteSearch As String = "DELETED"

                '  Find all emails having a specified header field with a value
                '  containing a substring:
                Dim headerSearch As String = "HEADER DomainKey-Signature paypal.com"

                '  Find any emails having a specific header field.  If the
                '  2nd argument to the "HEADER" criteria is an empty string,
                '  any email having the header field is returned regardless
                '  of the header field's content.
                '  Find any emails with a DomainKey-Signature field:
                Dim headerExistsSearch As String = "HEADER DomainKey-Signature """""

                '  Find NEW emails: these are emails that have the RECENT flag
                '  set, but not the SEEN flag:
                Dim newSearch As String = "NEW"

                '  Find emails larger than a certain number of bytes:
                Dim sizeLargerSearch As String = "LARGER 500000"

                '  Find emails marked as seen or not already seen:
                Dim seenSearch As String = "SEEN"
                Dim notSeenSearch As String = "NOT SEEN"

                '  Find emails having a given substring in the TO header field:
                Dim toSearch As String = "TO support@chilkatsoft.com"
                '  A more long-winded way to do the same thing:
                Dim toSearch2 As String = "HEADER TO support@chilkatsoft.com"

                '  Find emails smaller than a size in bytes:
                Dim smallerSearch As String = "SMALLER 30000"

                '  Find emails that have a substring anywhere in the header
                '  or body:
                Dim fullSubstringSearch As String = "TEXT ""Zip Component"""


                '  Pass any of the above strings here to test a search:
                messageSet = imap.Search(orSearch, fetchUids)
                If (messageSet Is Nothing) Then
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " No messages found: " & imap.LastErrorText)

                    Exit Sub
                End If


                '  Fetch the email headers into a bundle object:
                Dim bundle As Chilkat.EmailBundle
                bundle = imap.FetchHeaders(messageSet)
                If (bundle Is Nothing) Then

                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error bundling email headers: " & imap.LastErrorText)

                    Exit Sub
                End If

                Dim foundEmail As Chilkat.Email
                '  Display the Subject and From of each email.
                Dim i As Integer
                For i = 0 To bundle.MessageCount - 1
                    Dim email As Chilkat.Email
                    email = bundle.GetEmail(i)

                    '  Console.WriteLine(email.GetHeaderField("Date"))
                    If entity.SubjectKey = email.Subject Then
                        'message was found
                        foundEmail = email
                        boolFoundMemo = True
                        Exit For
                    End If

                Next

                If boolFoundMemo Then

                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Probe history found Target Document, it was delivered at: " & foundEmail.GetHeaderField("Date"))
                    tDeliveryDate = CType(foundEmail.GetHeaderField("Date"), DateTime)
                    DeliveryTime = tDeliveryDate.Subtract(tSentDate)
                    MyNotesMailProbe.ResponseTime = Math.Abs(DeliveryTime.TotalMinutes)
                    Dim MyResponse As String
                    MyResponse = " Successfully found the target document which was delivered within the target time. Elapsed time = " & DeliveryTime.TotalSeconds.ToString("F1") & " seconds."
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " " & MyNotesMailProbe.Name & " was delivered in " & MyResponse, LogLevel.Verbose)
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " " & MyNotesMailProbe.Name & " was delivered at " & tDeliveryDate.ToString, LogLevel.Verbose)
                    '  If DateTime.Compare(CType(docMail.GetItemValue("DeliveredDate")(0), DateTime), tScheduled) < 0 Then
                    'date is in future
                    '   WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Delivered on time")

                    myAlert.ResetAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Failure", MyNotesMailProbe.Location)

                        entity.Status = "OK"
                        entity.DeliveryTimeMinutes = MyNotesMailProbe.ResponseTime
                        entity.ArrivalAtMailbox = tDeliveryDate

                        entity.Details = MyResponse

                        MyNotesMailProbe.IncrementUpCount()
                        MyNotesMailProbe.Status = "OK"
                    '       MyNotesMailProbe.ResponseDetails = " Found the test message in the Target database, it was delivered on time at: " & docMail.GetItemValue("DeliveredDate")(0)
                    myAlert.ResetAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Failure", MyNotesMailProbe.Location)
                        myAlert.ResetAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Late", MyNotesMailProbe.Location)
                        'Update Stats Table
                        Dim MyResponseTime As Integer
                        MyResponseTime = Math.Abs(DeliveryTime.Seconds)
                        '        WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" MyResponseTime=" & DeliveryTime.Seconds)
                        If MyResponseTime = 0 Then
                            '   WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Somehow thought it was zero....")
                            'If delivered in less than a second, set it to 1, to distinguish from failures, which get 0
                            MyResponseTime = 1
                        End If
                        UpdateNotesMailStatistics(MyNotesMailProbe, "DeliveryTime.Seconds", MyResponseTime)
                        MyResponseTime = Nothing

                    Else
                        '      WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Delivered LATE")

                        entity.Status = "Late"
                    entity.DeliveryTimeMinutes = MyNotesMailProbe.ResponseTime
                    '  entity.ArrivalAtMailbox = DateTime.Parse(docMail.GetItemValue("DeliveredDate")(0))
                    '   entity.Details = "Delivered LATE in " & MyResponse

                    MyNotesMailProbe.IncrementDownCount()
                    '    myAlert.'QueueAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Slow", "NotesMailProbe found the test message in the Target database, but it was delivered late at: " & ' docMail.GetItemValue("DeliveredDate")(0), MyNotesMailProbe.Location)

                    MyNotesMailProbe.Status = "Delivered Late"
                    MyNotesMailProbe.ResponseDetails = " Found the test message in the Target database, but it was delivered late at: " '& docMail.GetItemValue("DeliveredDate")(0)
                    myAlert.QueueAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Slow", MyNotesMailProbe.ResponseDetails, MyNotesMailProbe.Location)
                    'Update Stats Table
                    UpdateNotesMailStatistics(MyNotesMailProbe, "DeliveryTime.Seconds", Math.Abs(DeliveryTime.Seconds))
                End If



                '  Disconnect from the IMAP server.
                success = imap.Disconnect()




                '**** end new




                Dim counter As Integer = 0  'This will store the total number of documents examined
                Dim DeleteCounter As Integer = 0   'This will store the total number of probes found that match the name
                Dim SearchText As String = entity.SubjectKey

                Dim oWatch As New System.Diagnostics.Stopwatch
                oWatch.Start()
                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Probe history is now searching for " & SearchText)



                ' While Not entryMail Is Nothing
                While True
                        'The counter should help prevent from going through huge inboxes over and over
                        counter += 1
                    If counter > 100 Then Exit While

                    boolDeleteMe = False
                    '    docMail = entryMail.Document
                    '    strSubject = docMail.GetItemValue("Subject")(0)
                    ' WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Probe history is now searching the Target Inbox... examining # " & counter.ToString & " - " & strSubject, LogLevel.Verbose)
                    If InStr(strSubject, SearchText) Then
                        'Found the document
                        boolFoundMemo = True
                        ' WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Probe history found Target Document, it was delivered at: " & docMail.GetItemValue("DeliveredDate")(0))
                        ' tDeliveryDate = CType(docMail.GetItemValue("DeliveredDate")(0), DateTime)
                        DeliveryTime = tDeliveryDate.Subtract(tSentDate)
                        MyNotesMailProbe.ResponseTime = Math.Abs(DeliveryTime.TotalMinutes)
                        Dim MyResponse As String
                        MyResponse = " Successfully found the target document which was delivered within the target time. Elapsed time = " & DeliveryTime.TotalSeconds.ToString("F1") & " seconds."
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " " & MyNotesMailProbe.Name & " was delivered in " & MyResponse, LogLevel.Verbose)
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " " & MyNotesMailProbe.Name & " was delivered at " & tDeliveryDate.ToString, LogLevel.Verbose)
                        ' If DateTime.Compare(CType(docMail.GetItemValue("DeliveredDate")(0), DateTime), tScheduled) < 0 Then
                        If True Then

                                'date is in future
                                '   WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Delivered on time")

                                myAlert.ResetAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Failure", MyNotesMailProbe.Location)

                            entity.Status = "OK"
                            entity.DeliveryTimeMinutes = MyNotesMailProbe.ResponseTime
                            '   entity.ArrivalAtMailbox = DateTime.Parse(docMail.GetItemValue("DeliveredDate")(0))
                            entity.Details = MyResponse

                            MyNotesMailProbe.IncrementUpCount()
                            MyNotesMailProbe.Status = "OK"
                            'MyNotesMailProbe.ResponseDetails = " Found the test message in the Target database, it was delivered on time at: " & docMail.GetItemValue("DeliveredDate")(0)
                            myAlert.ResetAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Failure", MyNotesMailProbe.Location)
                            myAlert.ResetAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Late", MyNotesMailProbe.Location)
                            'Update Stats Table
                            Dim MyResponseTime As Integer
                            MyResponseTime = Math.Abs(DeliveryTime.Seconds)
                            '        WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" MyResponseTime=" & DeliveryTime.Seconds)
                            If MyResponseTime = 0 Then
                                '   WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Somehow thought it was zero....")
                                'If delivered in less than a second, set it to 1, to distinguish from failures, which get 0
                                MyResponseTime = 1
                            End If
                            UpdateNotesMailStatistics(MyNotesMailProbe, "DeliveryTime.Seconds", MyResponseTime)
                            MyResponseTime = Nothing

                        Else
                            '      WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Delivered LATE")

                            entity.Status = "Late"
                            entity.DeliveryTimeMinutes = MyNotesMailProbe.ResponseTime
                            '  entity.ArrivalAtMailbox = DateTime.Parse(docMail.GetItemValue("DeliveredDate")(0))
                            entity.Details = "Delivered LATE in " & MyResponse

                            MyNotesMailProbe.IncrementDownCount()
                            '    myAlert.QueueAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Slow", "NotesMailProbe found the test message in the Target database, but it was delivered late at: " & docMail.GetItemValue("DeliveredDate")(0), MyNotesMailProbe.Location)

                            MyNotesMailProbe.Status = "Delivered Late"
                            '       MyNotesMailProbe.ResponseDetails = " Found the test message in the Target database, but it was delivered late at: " & docMail.GetItemValue("DeliveredDate")(0)
                            myAlert.QueueAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Slow", MyNotesMailProbe.ResponseDetails, MyNotesMailProbe.Location)
                            'Update Stats Table
                            UpdateNotesMailStatistics(MyNotesMailProbe, "DeliveryTime.Seconds", Math.Abs(DeliveryTime.Seconds))
                        End If

                        'Update the history table
                        'myCommand.CommandText = strSQL
                        'myCommand.ExecuteNonQuery()
                        repository.Replace(entity)
                        Exit While
                    Else
                        'WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " This email is not a match.", LogLevel.Verbose)
                        'WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Checking to see if it is an old probe... if the subject contains " & row("DeviceName"), LogLevel.Verbose)
                        'WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Subject is " & strSubject, LogLevel.Verbose)
                        'If InStr(strSubject, "NotesMail Probe Key=") And InStr(strSubject, row("DeviceName")) Then
                        '    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " This message should be deleted.", LogLevel.Verbose)
                        'End If
                    End If

                    Try
                        'You can't delete a document then use it to get to the next document, so here I mark the previous document for deletion then go back and get it

                        If InStr(strSubject, "NotesMail Probe Key=") And InStr(strSubject, entity.DeviceName) Then
                            WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Deleting Obsolete NotesMail document " & strSubject)
                            '  docMail.Remove(True)
                        End If
                    Catch ex As Exception
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Exception deleting obsolete NotesMail document " & strSubject)
                    End Try

                    ' docPrevious = docMail
                    'docMail = Inbox.GetNextDocument(docMail)


                End While


                oWatch.Stop()

                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & "  Searched the inbox and examined " & counter & " documents. ")
                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & "  Searched the inbox for " & oWatch.Elapsed.TotalMinutes.ToString("F1") & " minutes.")
                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & "  Searched the inbox for " & oWatch.Elapsed.TotalSeconds.ToString("F1") & " seconds.")
                'Document was not found in the target database if boolFoundMemo = False
                'see if the delivery deadline has past
                If boolFoundMemo = False Then
                    WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Doc not found, checking deadline." & "It is now " & tNow.ToString & " and deadline was " & tScheduled.ToString)
                    If DateTime.Compare(tNow, tScheduled) > 0 Then
                        'deadline has past, document has not arrived.
                        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Deadline expired, mail has not arrived")
                        MyNotesMailProbe.IncrementDownCount()
                        MyNotesMailProbe.Status = "Failed"
                        MyNotesMailProbe.ResponseDetails = "The test message was not found in the target database."
                        myAlert.QueueAlert(MyNotesMailProbe.ServerType, MyNotesMailProbe.Name, "Failure", MyNotesMailProbe.ResponseDetails, MyNotesMailProbe.Location)

                        entity.Status = "Failed"
                        entity.Details = "Memo not found"
                        repository.Replace(entity)

                    End If
                Else
                    'deadline not expired yet
                    '  WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Deadline has not expired. ")
                End If
            Next

        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error in CheckNotesMailDeliveryTimes module:" & ex.Message & vbCrLf & strSQL)

        End Try

Cleanup:


        Try
            listOfEntities = Nothing
        Catch ex As Exception

        End Try




    End Sub


    Public Sub DeleteObsoleteNotesMailProbes()
		'This function checks sent NotesMail probes and updates their status
		WriteDeviceHistoryEntry("All", "NotesMail Probes", vbCrLf & vbCrLf)
		WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " ***********  Checking for Obsolete NotesMail probes")
		Dim strSubject, strSQL As String

		Dim DeliveryTime As TimeSpan

        '6/22/2015 NS added for VSPLUS-1475
        Dim NotesSystemMessageString As String = "Incorrect Notes Password."
		'Domino Objects
		Dim myLocalSession As New Domino.NotesSession
		Try
			myLocalSession.Initialize(MyDominoPassword)
		Catch ex As Exception
            '6/22/2015 NS added for VSPLUS-1475
            System.Runtime.InteropServices.Marshal.ReleaseComObject(myLocalSession)
            myAlert.QueueSysMessage(NotesSystemMessageString)
            WriteAuditEntry(Now.ToString & " Error Initializing NotesSession.  Many problems will follow....  " & ex.ToString)
            WriteAuditEntry(Now.ToString & " *********** ERROR ************  Error initializing a session to Query Domino server. ")
            WriteAuditEntry(Now.ToString & " Calling stopnotescl.exe then exiting in an attempt to recover.")
            WriteAuditEntry(Now.ToString & " The VitalSigns Master service should restart the monitoring service in a few moments.")
            KillNotes()
            Exit Sub
		End Try

		Dim boolDeleteMe As Boolean = False
		Dim docMail, docPrevious As Domino.NotesDocument
		Dim MailBox As Domino.NotesDatabase
		Dim viewInbox As Domino.NotesView
		Dim navInBox As Domino.NotesViewNavigator
		Dim entryMail As Domino.NotesViewEntry

		Dim vsAdapter As New VSAdaptor



		Try
			Dim objVSAdaptor As New VSAdaptor
			'Loop through the NotesMail probes with status of 'Sent'
			For Each MailProbe As MonitoredItems.DominoMailProbe In MyNotesMailProbes

				strSQL = ""
				MailBox = myLocalSession.GetDatabase(MailProbe.TargetServer, MailProbe.TargetDatabase, False)


				If Not (MailBox Is Nothing) Then
					If String.IsNullOrWhiteSpace(MailBox.Title) Then
						MailBox.Title = "Untitled Database"
					End If
					WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Successfully connected to mailbox: " & MailBox.Title)
					viewInbox = MailBox.GetView("($Inbox)")
					WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Probe module connected to target InBox")
					navInBox = viewInbox.CreateViewNav

					' docMail = Inbox.GetFirstDocument
					entryMail = navInBox.GetLastDocument
					If entryMail Is Nothing Then
						WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Unable to connect to last document Inbox")
						'   Exit Try
					Else
						WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Connected to the last document in Inbox, searching up", LogLevel.Verbose)
					End If
				End If

				Dim counter As Integer = 0	'This will store the total number of documents examined
				Dim DeleteCounter As Integer = 0   'This will store the total number of probes found that match the name
				Dim SearchText As String = MailProbe.Name
				Dim tDeliveryDate As Date

				Dim oWatch As New System.Diagnostics.Stopwatch
				oWatch.Start()
				WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Probe history is now searching for " & SearchText)

				While Not entryMail Is Nothing
					'The counter should help prevent from going through huge inboxes over and over
					counter += 1
					If counter > 100 Then Exit While

					boolDeleteMe = False
					docMail = entryMail.Document
					strSubject = docMail.GetItemValue("Subject")(0)

					entryMail = navInBox.GetPrevDocument(entryMail)

					Try
						'You can't delete a document then use it to get to the next document, so I advance first then go back and get it

						If InStr(strSubject, "NotesMail Probe Key=") And InStr(strSubject, MailProbe.Name) Then
							'Only delete messages that are older than the scan interval
							tDeliveryDate = CType(docMail.GetItemValue("DeliveredDate")(0), DateTime)
							DeliveryTime = tDeliveryDate.Subtract(Now)
							' WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Found Obsolete NotesMail document " & strSubject & ", delivered " & DeliveryTime.TotalMinutes.ToString("F1") & " ago. ")

							If Math.Abs(DeliveryTime.TotalMinutes) > 10 Then
								WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Deleting Obsolete NotesMail document " & strSubject & ", delivered " & DeliveryTime.TotalMinutes.ToString("F1") & " ago. ", LogLevel.Verbose)
								docMail.Remove(True)
							End If

						End If
					Catch ex As Exception
						WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Exception deleting obsolete NotesMail document " & strSubject)
					End Try

				End While

				oWatch.Stop()

				WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & "  Searched the inbox and examined " & counter & " documents. ", LogLevel.Verbose)
				WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & "  Searched the inbox for " & oWatch.Elapsed.TotalMinutes.ToString("F1") & " minutes.", LogLevel.Verbose)
				WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & "  Searched the inbox for " & oWatch.Elapsed.TotalSeconds.ToString("F1") & " seconds.", LogLevel.Verbose)

			Next


		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error in DeleteObsoleteMailProbes module:" & ex.Message & vbCrLf & strSQL)
		End Try

Cleanup:



		Try
			If MailBox IsNot Nothing Then
				System.Runtime.InteropServices.Marshal.ReleaseComObject(MailBox)
			End If

		Catch ex As Exception
			'WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error in releasing Mailbox in CheckNotesMailDeliveryTimes module:" & ex.Message)
		End Try

		Try
			If viewInbox IsNot Nothing Then
				System.Runtime.InteropServices.Marshal.ReleaseComObject(viewInbox)
			End If

		Catch ex As Exception
			' WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error in releasing inBox in CheckNotesMailDeliveryTimes module:" & ex.Message)
		End Try


		Try
			If navInBox IsNot Nothing Then
				System.Runtime.InteropServices.Marshal.ReleaseComObject(navInBox)
			End If

		Catch ex As Exception
			' WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error in releasing inBox in CheckNotesMailDeliveryTimes module:" & ex.Message)
		End Try

		Try
			If entryMail IsNot Nothing Then
				System.Runtime.InteropServices.Marshal.ReleaseComObject(entryMail)
			End If

		Catch ex As Exception
			' WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error in releasing inBox in CheckNotesMailDeliveryTimes module:" & ex.Message)
		End Try


		Try
			If docMail IsNot Nothing Then
				System.Runtime.InteropServices.Marshal.ReleaseComObject(docMail)
			End If

			If docPrevious IsNot Nothing Then
				System.Runtime.InteropServices.Marshal.ReleaseComObject(docPrevious)
			End If

		Catch ex As Exception
			'    WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Error in releasing docMail in CheckNotesMailDeliveryTimes module:" & ex.Message)
		End Try

		Try
			If myLocalSession IsNot Nothing Then
				System.Runtime.InteropServices.Marshal.ReleaseComObject(myLocalSession)
			End If

		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error in releasing mySession in CheckNotesMailDeliveryTimes module:" & ex.Message)

		End Try


	End Sub

	Private Sub UpdateNotesMailStatusTable()

		Dim strSQL As String
		Dim Percent As Decimal
        Dim statusDetails As String
        Dim objVSAdaptor As New VSAdaptor
        For Each NotesMailProbe As MonitoredItems.DominoMailProbe In MyNotesMailProbes
            Try
                With NotesMailProbe
                    Select Case .Status
                        Case "Sent"
                            .StatusCode = "OK"
                        Case "Slow"
                            .StatusCode = "Issue"
                        Case "Delivered Late"
                            .StatusCode = "Issue"
                        Case "Failed"
                            .StatusCode = "Not Responding"
                        Case "Maintenance"
                            .StatusCode = "Maintenance"
                            .ResponseDetails = "Either the source or destination server is in maintenance."
                        Case "Not Scanned"
                            .StatusCode = "Maintenance"
                        Case Else
                            .StatusCode = "OK"
                    End Select
                    Percent = NotesMailProbe.ResponseTime / NotesMailProbe.DeliveryThreshold
                    '    WriteDeviceHistoryEntry("All", "NotesMail Probes", now.tostring &" Response Time: " & NotesMailProbe.ResponseTime & " /  Delivery Threshold: " & NotesMailProbe.DeliveryThreshold & " = Percent of Threshold = " & Percent)
                    '5/5/2016 NS modified - inserting a NotesMail Probe into StatusDetail fails because of a TypeANDName mismatch
                    'Changed TypeANDName -NMP to -NotesMail Probe
                    'TypeANDName is the key

                    Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                    Dim filterDef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repository.Filter.Eq(Function(x) x.TypeAndName, .Name & "-" & .ServerType)
                    Dim updateDef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repository.Updater _
                                                                                         .Set(Function(x) x.DownCount, .DownCount) _
                                                                                         .Set(Function(x) x.CurrentStatus, .Status) _
                                                                                         .Set(Function(x) x.UpPercent, .UpPercentCount) _
                                                                                         .Set(Function(x) x.LastUpdated, GetFixedDateTime(.LastScan)) _
                                                                                         .Set(Function(x) x.ResponseTime, Convert.ToInt32(.ResponseTime)) _
                                                                                         .Set(Function(x) x.Details, .ResponseDetails) _
                                                                                         .Set(Function(x) x.NextScan, .NextScan) _
                                                                                         .Set(Function(x) x.StatusCode, .StatusCode) _
                                                                                         .Set(Function(x) x.ResponseThreshold, Convert.ToInt32(.ResponseThreshold)) _
                                                                                         .Set(Function(x) x.MyPercent, Percent * 100) _
                                                                                         .Set(Function(x) x.Location, "Mail Probe") _
                                                                                         .Set(Function(x) x.DeviceId, .ServerObjectID) _
                                                                                         .Set(Function(x) x.DeviceName, .Name) _
                                                                                         .Set(Function(x) x.DeviceType, .ServerType) _
                                                                                         .Set(Function(x) x.Category, .Category) _
                                                                                         .Set(Function(x) x.Description, .Description) _
                                                                                         .Set(Function(x) x.DownCount, .DownCount) _
                                                                                         .Set(Function(x) x.Location, .Location) _
                                                                                         .Set(Function(x) x.UpCount, .UpCount)
                    repository.Upsert(filterDef, updateDef)

                End With
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error while executing NotesMail Probe update statement: " & ex.Message)
            End Try


        Next

        Try
			'myConnection.Close()
			'myCommand.Dispose()
			'myConnection.Dispose()
			strSQL = Nothing
			Percent = Nothing
			statusDetails = Nothing
		Catch ex As Exception
			WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Error cleaning memory when updating status table with NotesMail Probe info: " & ex.Message & vbCrLf & strSQL)
		End Try
		GC.Collect()
	End Sub

    Private Sub UpdateNotesMailStatistics(ByRef myServer As MonitoredItems.DominoMailProbe, ByVal StatName As String, ByVal StatValue As Double)
        WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " Updating NotesMail statistics table")

        Dim objVSAdaptor As New VSAdaptor

        Dim strSQL As String
        Dim MyWeekNumber As Integer
        MyWeekNumber = GetWeekNumber(Date.Today)
        Try

            Dim repository As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.DailyStatistics)(connectionString)
            Dim entity As New VSNext.Mongo.Entities.DailyStatistics With {
                .DeviceId = myServer.ServerObjectID,
                .DeviceName = myServer.Name,
                .DeviceType = myServer.ServerType,
                .StatName = StatName,
                .StatValue = StatValue
            }

            repository.Insert(entity)
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "NotesMail Probes", Now.ToString & " NotesMail Stats table insert failed becase: " & ex.Message)
        Finally
            'myConnection.Close()
            'myConnection.Dispose()
            'myCommand.Dispose()
            strSQL = Nothing
            MyWeekNumber = Nothing
        End Try
        GC.Collect()

    End Sub

End Class