Imports VSFramework
Imports System.Threading
Imports VSNext.Mongo.Repository
Imports VSNext.Mongo.Entities
Imports MongoDB.Driver

Partial Public Class VitalSignsPlusCore
    '10/6/2014 NS added for VSPLUS-1002

#Region "Cloud Monitoring"

    Private Sub MonitorCloud()  'This is the main sub that calls all the other ones

        Dim start, done As Long
        Dim elapsed As TimeSpan

        Do
            Dim myCloud As MonitoredItems.Cloud = Nothing
            start = Now.Ticks
            WriteDeviceHistoryEntry("All", "Cloud_Performance", Now.ToString & " >>> Top of Cloud monitoring loop.")
            Try
                CloudSelector_Mutex.WaitOne()
                myCloud = SelectCloudURLToMonitor()

            Catch ex As Exception
                myCloud = Nothing
            Finally
                CloudSelector_Mutex.ReleaseMutex()
            End Try

            Try
                CurrentCloud = ""
                If Not myCloud Is Nothing Then
                    myCloud.IsBeingScanned = True
                    WriteAuditEntry(Now.ToString & " Selected Cloud URL " & myCloud.Name, LogLevel.Verbose)
                    WriteDeviceHistoryEntry("All", "Cloud_Performance", Now.ToString & " Selected " & myCloud.Name & " at " & myCloud.CloudURL)
                    QueryCloudURL(myCloud)
                    myCloud.LastScan = Date.Now
                Else
                    GoTo WaitHere
                End If
            Catch ex As ThreadAbortException
                Exit Sub
            Catch ex As Exception
                WriteDeviceHistoryEntry("All", "Cloud_Performance", Now.ToString & " Cloud Monitoring error " & ex.Message, LogLevel.Normal)
            End Try

            Try
                done = Now.Ticks
                elapsed = New TimeSpan(done - start)
                WriteDeviceHistoryEntry("All", "Cloud_Performance", Now.ToString & " Finished loop to monitor " & myCloud.Name & " at " & myCloud.CloudURL)
            Catch ex As Exception

            End Try


            If elapsed.TotalMinutes > 4 Then
                start = Now.Ticks
                WriteDeviceHistoryEntry("All", "Cloud_Performance", Now.ToString & " Reloading Cloud settings.....")
                Try
                    CreateCloudCollection()
                    UpdateStatusTableWithCloudURLs()
                Catch ex As Exception

                End Try

            End If

WaitHere:

            Thread.CurrentThread.Sleep(5000)
            If MyClouds.Count < 24 Then
                Thread.CurrentThread.Sleep(2500)
            End If
            If MyClouds.Count < 10 Then
                Thread.CurrentThread.Sleep(10000)
            End If

            Try
                If Not myCloud Is Nothing Then
                    myCloud.IsBeingScanned = False
                End If

            Catch ex As Exception

            End Try
        Loop

    End Sub

    Private Function SelectCloudURLToMonitor() As MonitoredItems.Cloud
        Dim tNow As DateTime
        tNow = Now
        Dim tScheduled As DateTime

        Dim timeOne, timeTwo As DateTime

        Dim SelectedServer As MonitoredItems.Cloud

        Dim ServerOne As MonitoredItems.Cloud
        Dim ServerTwo As MonitoredItems.Cloud
        Dim myRegistry As New RegistryHandler

		Dim n As Integer
		Dim strSQL As String = ""
		Dim ServerType As String = "Cloud"
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

				For n = 0 To MyClouds.Count - 1
					ServerOne = MyClouds.Item(n)

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

        


        'Any server Not Responding that is due for a scan should be scanned right away.  Select the first one you encounter
        For n = 0 To MyClouds.Count - 1
            ServerOne = MyClouds.Item(n)
            If ServerOne.Status = "Not Responding" And ServerOne.Enabled = True And ServerOne.IsBeingScanned = False Then
                tScheduled = CDate(ServerOne.NextScan)
                If DateTime.Compare(tNow, tScheduled) > 0 Then
                    WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status & ".  Next scheduled scan is " & tScheduled.ToShortTimeString)
                    Return ServerOne
                    Exit Function
                Else

                End If
            End If
        Next

        'Any server Not Scanned should be scanned right away.  Select the first one you encounter
        For n = 0 To MyClouds.Count - 1
            ServerOne = MyClouds.Item(n)
            If ServerOne.Status = "Not Scanned" Or ServerOne.Status = "Master Service Stopped." And ServerOne.Enabled = True And ServerOne.IsBeingScanned = False Then
                Return ServerOne
                Exit Function
            End If
        Next


        Dim ScanCandidates As New MonitoredItems.CloudCollection
        For Each srv As MonitoredItems.Cloud In MyClouds
            If srv.IsBeingScanned = False And srv.Enabled = True Then
                ScanCandidates.Add(srv)
            Else
            End If
        Next

        For Each srv As MonitoredItems.Cloud In ScanCandidates
            ' WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " " & srv.Name & " is a candidate to be scanned.  Last scan: " & srv.LastScan)
        Next

        If ScanCandidates.Count = 0 Then
            ' WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " All servers are already being scanned, exiting sub.")
            Return Nothing
        End If

        '*****************

        'start with the first two servers
        ServerOne = ScanCandidates.Item(0)
        ServerOne = ScanCandidates.Item(0)
        If ScanCandidates.Count > 1 Then ServerTwo = ScanCandidates.Item(1)

        Try
            'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Finding which server is the most overdue to be monitored.")
            'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Server One is  " & ServerOne.Name & ", due to be scanned " & ServerOne.NextScan)
            'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Server Two " & ServerTwo.Name & ", due to be scanned " & ServerTwo.NextScan)
        Catch ex As Exception
        End Try


        'go through the remaining servers, see which one has the oldest (earliest) scheduled time
        If ScanCandidates.Count > 2 Then
            Try
                For n = 2 To ScanCandidates.Count - 1
                    timeOne = CDate(ServerOne.NextScan)
                    timeTwo = CDate(ServerTwo.NextScan)
                    If DateTime.Compare(timeOne, timeTwo) < 0 Then
                        'time one is earlier than time two, so keep server 1
                        'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " " & ServerOne.Name & " is more overdue ")
                        ServerTwo = ScanCandidates.Item(n)
                    Else
                        'time two is later than time one, so keep server 2
                        ServerOne = ScanCandidates.Item(n)
                        'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " " & ServerTwo.Name & " is more overdue ")
                    End If

                    'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Server One is  " & ServerOne.Name & ", due to be scanned " & ServerOne.NextScan)
                    'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Server Two " & ServerTwo.Name & ", due to be scanned " & ServerTwo.NextScan)

                Next
            Catch ex As Exception
                ' WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " >>> Error Selecting URL server... " & ex.Message)
            End Try
        Else
            'There were only two server, so use those going forward
        End If

        'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " >>> Down to two servers... " & ServerOne.Name & " and " & ServerTwo.Name)

        'Of the two remaining servers, pick the one with earliest scheduled time for next scan
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
            'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " >>> Down to one server... " & SelectedServer.Name & " to scan at " & SelectedServer.NextScan & ". Status is " & SelectedServer.Status)
        Else
            SelectedServer = ServerOne
            tScheduled = CDate(ServerOne.NextScan)
        End If

        tScheduled = CDate(SelectedServer.NextScan)
        If DateTime.Compare(tNow, tScheduled) < 0 Then
            If SelectedServer.Status <> "Not Scanned" Then
                'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " No Domino servers scheduled for monitoring, next scan after " & SelectedServer.NextScan)
                SelectedServer = Nothing
            Else
                ' WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " selected URL server: " & SelectedServer.Name & " because it has not been scanned yet.")
            End If
        Else
            'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " selected Domino server: " & SelectedServer.Name)
        End If

        '**************

        'Release Memory
        tNow = Nothing
        tScheduled = Nothing
        n = Nothing

        timeOne = Nothing
        timeTwo = Nothing

        ServerOne = Nothing
        ServerTwo = Nothing

        'return selected server
        ' SelectedServer.IsBeingScanned = True
        SelectCloudURLToMonitor = SelectedServer


        'Exit Function
        SelectedServer = Nothing
    End Function

    Private Sub QueryCloudURL(ByRef myCloud As MonitoredItems.Cloud)
        Dim StatusDetails As String
        Dim Percent As Double = 100
        Dim strSQL As String
        myCloud.Status = "OK"

        Try
            If myCloud.Enabled = False Then
                With myCloud
                    .Status = "Disabled"
                    .Enabled = False
                    StatusDetails = "Monitoring is disabled for this Cloud URL."
                    Percent = 0
                End With
                GoTo Update
                Exit Try
            End If

        Catch ex As Exception

        End Try

        Try
            Dim bIsInMaintenance As Boolean = InMaintenance("Cloud", myCloud.Name)
            WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Checking to see if " & myCloud.Name & " is in maintenance.", LogLevel.Verbose)
            WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Maintenance DLL says " & bIsInMaintenance.ToString)
            If bIsInMaintenance Then
                myCloud.Status = "Maintenance"
                StatusDetails = "This Cloud URL is in a scheduled maintenance period.  Monitoring is temporarily disabled."
                Percent = 0
                GoTo Update
                Exit Try
            End If
        Catch ex As Exception

        End Try

        WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Checking " & myCloud.Name & " at " & myCloud.CloudURL, LogLevel.Normal)

        Try
            '' myURL.Status = "OK"
            ''myURL.ResponseDetails = ""
            'If InStr(myURL.URL.ToUpper, "HTTPS://") Then
            '    URLResponseTimeSSL(myURL)
            'Else
            Call CloudURLResponseTimeChilkat(myCloud)
            '      Call URLResponseTime(myURL)
            'End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Exception calling response time module: " & ex.ToString)
            WriteAuditEntry(Now.ToString & " %%% EXCEPTION IN CLOUD URL RESPONSE TIME  %%%" & ex.ToString)
        End Try

        Try
            If myCloud.ResponseTime > myCloud.ResponseThreshold Then
                WriteDeviceHistoryEntry("Cloud", myCloud.Name, " The Cloud URL was slow, but we're going to check again to make sure.", LogLevel.Verbose)
                'Users don't like false alerts about slow, so we're going to try again to be sure
                Call CloudURLResponseTimeChilkat(myCloud)
            End If

        Catch ex As Exception

        End Try

        Try
            If myCloud.ResponseTime > myCloud.ResponseThreshold Then
                WriteDeviceHistoryEntry("Cloud", myCloud.Name, " The Cloud URL was slow AGAIN, so we're going to make one final check.", LogLevel.Verbose)
                'Users don't like false alerts about slow, so we're going to try again to be sure
                Call CloudURLResponseTimeChilkat(myCloud)
            End If

        Catch ex As Exception

        End Try


        Try

            If myCloud.ResponseTime > 0 Then

                myCloud.Status = "OK"
                myCloud.AlertCondition = False
                myCloud.IncrementUpCount()
                If myCloud.ResponseTime > myCloud.ResponseThreshold Then
                    StatusDetails = "Response Time: " & myCloud.ResponseTime & " ms, Target Time: " & myCloud.ResponseThreshold & " ms"
                Else
                    StatusDetails = "Response Time: " & myCloud.ResponseTime & " ms, Target Time: " & myCloud.ResponseThreshold & " ms"
                End If
                myAlert.ResetAlert("Cloud", myCloud.Name, "Not Responding", myCloud.Location)
                myCloud.Description = "Cloud URL responded in " & myCloud.ResponseTime & " ms."
                UpdateCloudURLStatisticsTable(myCloud.Name, myCloud.ResponseTime, myCloud.ServerObjectID)

                Try
                    Percent = myCloud.ResponseTime / myCloud.ResponseThreshold
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Error calculating response time percentage: " & ex.Message)
                End Try

                Try
                    Dim myContent As String = CloudURLContentCheck(myCloud)
                    Select Case myContent
                        Case "Exception"
                            StatusDetails = "The Cloud URL responded but contains an exception."
                            myCloud.Status = "Issue"
                        Case "String Not Found"
                            StatusDetails = "The Cloud URL responded but the search string was not found."
                            myCloud.Status = "Issue"
                        Case "String Found"
                            StatusDetails = "The expected text was found.   The Cloud URL responded in " & myCloud.ResponseTime & " ms"

                    End Select
                Catch ex As Exception

                End Try

                Try
                    'MUKUND 15NOV13: copied below 2 lines from else condition to outside. As the status is OK, reset the 'Error'/'Not Responding'
                    'The 'Slow' status reseting remains in else condition          
                    If myCloud.ResponseTime > myCloud.ResponseThreshold Then
                        myCloud.Status = "Slow"
                        StatusDetails = "Response Time: " & myCloud.ResponseTime & " ms. Target is " & myCloud.ResponseThreshold & " ms."
                        myCloud.Description = "Response Time: " & myCloud.ResponseTime & " ms. Target is " & myCloud.ResponseThreshold & " ms."
                        myAlert.QueueAlert("Cloud", myCloud.Name, "Slow", myCloud.Name & " at " & myCloud.CloudURL & " responded in " & myCloud.ResponseTime & " ms, but the Target is " & myCloud.ResponseThreshold & " ms.", myCloud.Location)
                    Else
                        myAlert.ResetAlert("Cloud", myCloud.Name, "Slow", myCloud.Location)
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Error comparing Cloud URL response time to threshold: " & ex.Message)
                End Try

            End If

            If myCloud.ResponseTime = 0 Then
                myCloud.IncrementDownCount()
                UpdateURLStatisticsTable(myCloud.Name, 0, myCloud.ServerObjectID)
                If myCloud.ResponseDetails <> "" Then
                    StatusDetails = myCloud.ResponseDetails
                Else
                    StatusDetails = "The Cloud URL is not responding. Either the monitoring station has lost connectivity or the Cloud URL is down."
                    myCloud.Description = "The Cloud URL is not responding. Either the monitoring station has lost connectivity or the Cloud URL is down."
                End If

                myCloud.AlertCondition = True

                Percent = 0
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " URL Monitor error: " & ex.Message)
        End Try

        Dim PercentageChange As Double  'calculate the change in response time from prior scan

        Try
            If myCloud.PreviousKeyValue > 0 And myCloud.ResponseTime > 0 Then
                PercentageChange = -(1 - myCloud.PreviousKeyValue / myCloud.ResponseTime)
            Else
                PercentageChange = 0
            End If

        Catch ex As Exception
            PercentageChange = 0
        End Try
        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Previous response time was " & myCloud.PreviousKeyValue & " Current value is " & myCloud.ResponseTime & "; so percent change is: " & PercentageChange)

Update:
        myCloud.LastScan = Now
        Dim strPercent As String
        Percent = Percent * 100
        strPercent = Percent.ToString
        Dim strPercentFormatted As String
        If InStr(strPercent, ",") Then
            strPercentFormatted = strPercent.Replace(",", ".")
            strPercentFormatted = strPercentFormatted.Replace(" ", "")
            strPercent = strPercentFormatted
        End If

        Try
            myCloud.StatusCode = ServerStatusCode(myCloud.Status)

        Catch ex As Exception
            myCloud.StatusCode = vbNull
        Finally
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " My status code is " & myCloud.StatusCode)
        End Try

        'Try
        '    strSQL = "Update Status SET DownCount= " & myCloud.DownCount & _
        '                    ", Status='" & myCloud.Status & "', Upcount=" & myCloud.UpCount & _
        '                    ", LastUpdate='" & FixDateTime(Now) & _
        '                    "', StatusCode='" & myCloud.StatusCode & _
        '                    "', Location='Cloud', ResponseTime=" & Str(myCloud.ResponseTime) & _
        '                    ", NextScan='" & FixDateTime(myCloud.NextScan) & _
        '                    "'  WHERE TypeANDName='" & myCloud.Name & "-Cloud' "
        '    WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " " & strSQL)
        '    UpdateStatusTable(strSQL)
        'Catch ex2 As Exception
        '    WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Cloud URL Monitor Error while creating second try Cloud URL SQL statement: " & ex2.Message & vbCrLf)
        'End Try

		Dim strSQLInsert As String = ""

        Try

            With myCloud
                '            strSQL = "Update Status SET DownCount= " & myCloud.DownCount & _
                '            ", Status='" & myCloud.Status & "', Upcount=" & myCloud.UpCount & _
                '            ", UpPercent= " & myCloud.UpPercentCount & _
                '            ", LastUpdate='" & FixDateTime(Now) & _
                '            "', ResponseTime='" & Str(myCloud.ResponseTime) & _
                '            "', StatusCode='" & .StatusCode & _
                '            "', NextScan='" & FixDateTime(myCloud.NextScan) & _
                '            "', PercentageChange='" & Str(PercentageChange) & _
                '            "', ResponseThreshold='" & myCloud.ResponseThreshold & _
                '            "', MyPercent='" & strPercent & _
                '            "', Name='" & myCloud.Name & "' " & _
                '            ", Details='" & StatusDetails & "' " & _
                '            ", MailDetails='" & myCloud.CloudURL & "' " & _
                '            ", UpPercentMinutes='" & myCloud.UpPercentMinutes & _
                '            "', UpMinutes='" & Microsoft.VisualBasic.Strings.Format(myCloud.UpMinutes, "F1") & _
                '            "', DownMinutes='" & Microsoft.VisualBasic.Strings.Format(myCloud.DownMinutes, "F1") & _
                '            "'  WHERE TypeANDName='" & myCloud.Name & "-Cloud' "
                '            'TypeANDName is the key


                'strSQLInsert = "INSERT INTO Status (StatusCode, Category,  Description, Details, DownCount,  Location, Name,  Status, Type, Upcount, UpPercent, LastUpdate, ResponseTime,  TypeANDName, Icon, " & _
                '  "NextScan, PercentageChange, ResponseThreshold, MyPercent, MailDetails, UpPercentMinutes, UpMinutes, DownMinutes)  " & _
                '   " VALUES ('" & .StatusCode & "', '" & .Category & "', '" & .CloudURL & "', '" & StatusDetails & "', '" & .DownCount & "', 'Cloud', '" & .Name & "', '" & .Status & "', 'Cloud', '" & .UpCount & "'," & _
                '   "'" & .UpPercentCount & "', '" & FixDateTime(Now) & "', '" & Str(.ResponseTime) & "', '" & .Name & "-Cloud', " & IconList.URL & ", '" & FixDateTime(myCloud.NextScan) & "', '" & Str(PercentageChange) & "', " & _
                '   "'" & myCloud.ResponseThreshold & "', '" & strPercent & "', '" & myCloud.CloudURL & "', '" & myCloud.UpPercentMinutes & "', '" & Microsoft.VisualBasic.Strings.Format(myCloud.UpMinutes, "F1") & "', " & _
                '   "'" & Microsoft.VisualBasic.Strings.Format(myCloud.DownMinutes, "F1") & "')"

                Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
                Dim TypeAndName As String = myCloud.Name & "-Cloud"
                Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repo.Filter.Where(Function(i) i.TypeAndName = TypeAndName)
                Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status)
                updatedef = repo.Updater _
                                          .Set(Function(i) i.Name, .Name.ToString()) _
                                          .[Set](Function(i) i.CurrentStatus, .Status) _
                                          .[Set](Function(i) i.StatusCode, .Status) _
                                          .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                                           .[Set](Function(i) i.Details, StatusDetails) _
                                          .[Set](Function(i) i.Category, .Category) _
                                          .[Set](Function(i) i.TypeAndName, TypeAndName) _
                                          .[Set](Function(i) i.Description, .Description) _
                                          .[Set](Function(i) i.Type, "Cloud") _
                                          .[Set](Function(i) i.DownCount, Integer.Parse(.DownCount)) _
                                          .[Set](Function(i) i.Location, "Cloud") _
                                          .[Set](Function(i) i.UpCount, Integer.Parse(.UpCount)) _
                                          .[Set](Function(i) i.UpPercent, Integer.Parse(.UpPercentCount)) _
                                          .[Set](Function(i) i.ResponseTime, Integer.Parse(.ResponseTime)) _
                                          .[Set](Function(i) i.MyPercent, Double.Parse(strPercent)) _
                                          .[Set](Function(i) i.NextScan, .NextScan) _
                                          .[Set](Function(i) i.UpMinutes, Integer.Parse(Microsoft.VisualBasic.Strings.Format(.UpMinutes, "F1"))) _
                                          .[Set](Function(i) i.DownMinutes, Integer.Parse(Microsoft.VisualBasic.Strings.Format(.DownMinutes, "F1"))) _
                                          .[Set](Function(i) i.MailDetails, TypeAndName) _
                                          .[Set](Function(i) i.PercentageChange, PercentageChange) _
                                         .[Set](Function(i) i.ResponseThreshold, Integer.Parse(.ResponseThreshold))
                repo.Upsert(filterdef, updatedef)

			End With

            WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " " & strSQL)
            'UpdateStatusTable(strSQL, SQLInsertStatement:=strSQLInsert)
        Catch ex As Exception
            WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Cloud URL Monitor Error while creating Cloud URL SQL statement: " & ex.Message & vbCrLf & strSQL)
        End Try

    End Sub

    Public Sub CloudURLResponseTimeChilkat(ByRef myCloud As MonitoredItems.Cloud)
        'This function returns as the number of milliseconds it takes to open a URL and retrieve its content
        'returns 0 in case of exception
        'returns -1 if it cannot connect
        Try
            '  WriteAuditEntry(Now.ToString & " Querying " & myURL.URL & " for response time using Chilkat component.")
            WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Querying " & myCloud.CloudURL & " for response time using Chilkat component.")
        Catch ex As Exception

        End Try

        Try
            myCloud.PreviousKeyValue = myCloud.ResponseTime   'remember what the time was last time, so you can compare
        Catch ex As Exception

        End Try

        Dim start, done, hits As Long
        Dim elapsed As TimeSpan
        start = Now.Ticks
        Dim ResponseTime As Double = 0
        Dim ChilkatHTTP As New Chilkat.Http
        myCloud.HTML = ""
        Try
            Dim success As Boolean
            success = ChilkatHTTP.UnlockComponent("MZLDADHttp_efwTynJYYR3X")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error unlocking Chilkat component: " & ex.ToString)
            WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Error unlocking component: " & ex.ToString)
        End Try

        Try
            Dim myRegistry As New RegistryHandler
            If myRegistry.ReadFromRegistry("ProxyEnabled") = "True" Then
                WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Attempting to route through proxy server.")
                ChilkatHTTP.SocksHostname = myRegistry.ReadFromRegistry("ProxyServer")
                ChilkatHTTP.SocksPort = myRegistry.ReadFromRegistry("ProxyPort")
                ChilkatHTTP.SocksUsername = myRegistry.ReadFromRegistry("ProxyUser")
                ChilkatHTTP.SocksPassword = myRegistry.ReadFromRegistry("ProxyPassword")
            End If
            myRegistry = Nothing
        Catch ex As Exception

        End Try


        Try

            If myCloud.UserName <> "" Then
                WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " This Cloud URL requires a username and password.")
                ChilkatHTTP.Login = myCloud.UserName
                ChilkatHTTP.Password = myCloud.Password
            End If

            Dim n As Integer
            Do While myCloud.HTML = ""
                myCloud.HTML = ChilkatHTTP.QuickGetStr(myCloud.CloudURL)
                n += 1
                Thread.Sleep(500)
                If n > 15 Then Exit Do
            Loop

            If (myCloud.HTML = "") Then
                myCloud.ResponseDetails = "Cloud URL did not respond. "
                myCloud.Status = "Not Responding"
                myCloud.Description = "The Cloud URL has timed out without responding. "
                myCloud.ResponseTime = 0
                Exit Sub
            End If

            done = Now.Ticks
            elapsed = New TimeSpan(done - start)
            myCloud.ResponseTime = elapsed.TotalMilliseconds
            WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Cloud URL responded in " & elapsed.TotalMilliseconds & " ms.")
            '  WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " URL returned the following HTML: " & vbCrLf & HTML)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error performing HTTP.Get: " & ex.Message)
            WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Error performing HTTP.Get: " & ex.Message)
            myCloud.ResponseDetails = ex.Message
            myCloud.Description = ex.Message
            myCloud.Status = "Error"
            myCloud.ResponseTime = 0
        End Try



    End Sub

    Public Function CloudURLContentCheck(ByRef myCloud As MonitoredItems.Cloud) As String
        'Returns 

        Dim returnValue As String = ""

        WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " Checking the Cloud URL content.")
		If InStr(myCloud.HTML.ToLower(), "exception") And InStr(myCloud.CloudURL.ToLower(), ".nsf") Then
			WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " This Cloud URL has an Exception error in the response. That's bad.  Here's what we got:" & vbCrLf & vbCrLf & myCloud.HTML, )
			myAlert.QueueAlert("Cloud", myCloud.Name, "Exception", "This Cloud URL " & myCloud.Name & " at " & myCloud.CloudURL & " is returning an Exception.  Here's what we got:" & vbCrLf & vbCrLf & myCloud.HTML, myCloud.Location)
			myCloud.Status = "Exception"
			myCloud.ResponseDetails = "This Cloud URL has an Exception."
			returnValue = "Exception"
		Else
			myAlert.ResetAlert("Cloud", myCloud.Name, "Exception", myCloud.Location)
			WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " This Cloud URL does not have an Exception error in the response. ", LogLevel.Verbose)
		End If


        If myCloud.SearchString = "" Then
            returnValue = "N/A"
        Else
            WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " This response will be checked for the specified search string of " & myCloud.SearchString)
            If InStr(myCloud.HTML, myCloud.SearchString) Then
                'myURL.ResponseDetails = myURL.ResponseDetails & " The search string " & myURL.SearchString & " was found."
                WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " This response contains the correct text.")
                CloudStringFound = True
                returnValue = "String Found"
                myAlert.ResetAlert("Cloud", myCloud.Name, "Search String", myCloud.Location)

            Else
                WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " The string we were looking for was not found!", LogLevel.Verbose)
                WriteDeviceHistoryEntry("Cloud", myCloud.Name, Now.ToString & " The response we got was  " & vbCrLf & myCloud.HTML, LogLevel.Verbose)
                CloudStringFound = False
                returnValue = "String Not Found"
                myAlert.QueueAlert("Cloud", myCloud.Name, "Search String", "This Cloud URL " & myCloud.Name & " at " & myCloud.CloudURL & " does not contain the string we were searching for.  Here's what we got instead:" & vbCrLf & vbCrLf & myCloud.HTML, myCloud.Location)

            End If
        End If

        Return returnValue

    End Function
#End Region

End Class
