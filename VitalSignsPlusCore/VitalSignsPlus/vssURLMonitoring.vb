Imports VSFramework
Imports System.Threading
Imports VSNext.Mongo.Repository
Imports VSNext.Mongo.Entities
Imports MongoDB.Driver
Partial Public Class VitalSignsPlusCore
    Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("VitalSignsMongo").ToString()
#Region "URL Monitoring"

    Private Sub MonitorURL()  'This is the main sub that calls all the other ones

        Dim start, done As Long
        Dim elapsed As TimeSpan

        Do
            Dim myURL As MonitoredItems.URL = Nothing
            start = Now.Ticks
            ' WriteAuditEntry(Now.ToString & " %%% Begin Loop for URL Monitoring %%%", LogLevel.Verbose)
            WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " >>> Top of URL monitoring loop.")
            Try
                URLSelector_Mutex.WaitOne()
                myURL = CType(SelectServerToMonitor(MyURLs), MonitoredItems.URL)

            Catch ex As Exception
                myURL = Nothing
            Finally
                URLSelector_Mutex.ReleaseMutex()
            End Try

            Try
                CurrentURL = ""
                If Not myURL Is Nothing Then
                    myURL.IsBeingScanned = True
                    WriteAuditEntry(Now.ToString & " Selected URL " & myURL.Name, LogLevel.Verbose)
                    WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " Selected " & myURL.Name & " at " & myURL.URL)
                    'CurrentURL = myURL.Name
                    'URLStringFound = False
                    QueryURL(myURL)
                    myURL.LastScan = Date.Now
                Else
                    GoTo WaitHere
                End If
            Catch ex As ThreadAbortException
                Exit Sub
            Catch ex As Exception
                'WriteAuditEntry(Now.ToString & " URL Monitoring error " & ex.Message)
                WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " URL Monitoring error " & ex.Message, LogLevel.Normal)
            End Try

            Try
                done = Now.Ticks
                elapsed = New TimeSpan(done - start)
                WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " Finished loop to monitor " & myURL.Name & " at " & myURL.URL)
            Catch ex As Exception

            End Try


            If elapsed.TotalMinutes > 4 Then
                start = Now.Ticks
                WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " Reloading URL settings.....")
                Try
                    ' threadMonitorURLs.Suspend()
                    CreateURLCollection()
                    UpdateStatusTableWithURLs()
                Catch ex As Exception

                End Try

            End If

WaitHere:

            Thread.CurrentThread.Sleep(5000)
            If MyURLs.Count < 24 Then
                Thread.CurrentThread.Sleep(2500)
            End If
            If MyURLs.Count < 10 Then
                Thread.CurrentThread.Sleep(10000)
            End If

            Try
                If Not myURL Is Nothing Then
                    myURL.IsBeingScanned = False
                End If

            Catch ex As Exception

            End Try
        Loop

    End Sub

    Private Sub QueryURL(ByRef myURL As MonitoredItems.URL)
        '   Dim dtLastScan As DateTime = myURL.LastScan

        Dim StatusDetails As String = ""
        Dim Percent As Double = 100
        Dim strSQL As String
        myURL.Status = "OK"

        Try
            If myURL.Enabled = False Then
                With myURL
                    .Status = "Disabled"
                    .Enabled = False
                    StatusDetails = "Monitoring is disabled for this URL."
                    Percent = 0
                End With
                GoTo Update
                Exit Try
            End If

        Catch ex As Exception

        End Try

        Try
            Dim bIsInMaintenance As Boolean = InMaintenance("URL", myURL.Name)
            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " Checking to see if " & myURL.Name & " is in maintenance.", LogLevel.Verbose)
            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " Maintenance DLL says " & bIsInMaintenance.ToString)
            If bIsInMaintenance Then
                myURL.Status = "Maintenance"
                StatusDetails = "This URL is in a scheduled maintenance period.  Monitoring is temporarily disabled."
                'myURL.Description = "This URL is in a scheduled maintenance period.  Monitoring is temporarily disabled."
                Percent = 0
                GoTo Update
                Exit Try
            End If
        Catch ex As Exception

        End Try

        WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " Checking " & myURL.Name & " at " & myURL.URL, LogLevel.Normal)
        WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " http location " & InStr(myURL.URL.ToUpper, "HTTP").ToString() & " at " & myURL.URL)

        ' if not protocol is defined, append "http://"
        If InStr(myURL.URL.ToUpper, "HTTP") <> 1 And InStr(myURL.URL.ToUpper, "FTP") <> 1 Then
            myURL.URL = "http://" + myURL.URL
            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " append http:// to the URL " & " at " & myURL.URL)
        End If
        Try
            '' myURL.Status = "OK"
            ''myURL.ResponseDetails = ""
            'If InStr(myURL.URL.ToUpper, "HTTPS://") Then
            '    URLResponseTimeSSL(myURL)
            'Else
            Call URLResponseTimeChilkat(myURL)
            '      Call URLResponseTime(myURL)
            'End If
        Catch ex As Exception
            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " Exception calling response time module: " & ex.ToString)
            WriteAuditEntry(Now.ToString & " %%% EXCEPTION IN URL RESPONSE TIME  %%%" & ex.ToString)
        End Try

        Try
            If myURL.ResponseTime > myURL.ResponseThreshold Then
                WriteDeviceHistoryEntry("URL", myURL.Name, " The URL was slow, but we're going to check again to make sure.", LogLevel.Verbose)
                'Users don't like false alerts about slow, so we're going to try again to be sure
                Call URLResponseTimeChilkat(myURL)
            End If

        Catch ex As Exception

        End Try

        Try
            If myURL.ResponseTime > myURL.ResponseThreshold Then
                WriteDeviceHistoryEntry("URL", myURL.Name, " The URL was slow AGAIN, so we're going to make one final check.", LogLevel.Verbose)
                'Users don't like false alerts about slow, so we're going to try again to be sure
                Call URLResponseTimeChilkat(myURL)
            End If

        Catch ex As Exception

        End Try


        Try

            If myURL.ResponseTime > 0 Then

                myURL.Status = "OK"
                myURL.AlertCondition = False
                myURL.IncrementUpCount()
                If myURL.ResponseTime > myURL.ResponseThreshold Then
                    StatusDetails = "Response Time: " & myURL.ResponseTime & " ms, Target Time: " & myURL.ResponseThreshold & " ms"
                Else
                    StatusDetails = "Response Time: " & myURL.ResponseTime & " ms, Target Time: " & myURL.ResponseThreshold & " ms"
                End If
                myAlert.ResetAlert("URL", myURL.Name, "Not Responding", myURL.Location)
                myURL.Description = "URL responded in " & myURL.ResponseTime & " ms."
                UpdateURLStatisticsTable(myURL.Name, myURL.ResponseTime, myURL.ServerObjectID)

                Try
                    Percent = myURL.ResponseTime / myURL.ResponseThreshold
                Catch ex As Exception
                    WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " Error calculating response time percentage: " & ex.Message)
                End Try

                Try
                    Dim myContent As String = URLContentCheck(myURL)
                    Select Case myContent
                        Case "Domino Exception"
                            StatusDetails = "The URL responded but contains a Domino exception"
                            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " The URL is responding but has a Domino exception in it", LogLevel.Normal)
                            myURL.Status = "Issue"
                        Case "String Not Found"
                            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " : String not Found. The myURL.AlertStringFound is:" + myURL.AlertStringFound.ToString(), LogLevel.Normal)
                            If myURL.AlertStringFound = "0" Or myURL.AlertStringFound = "" Then
                                StatusDetails = "The URL responded but the expected text (" & myURL.SearchString & ") was not found."
                                myURL.Status = "Issue"
                            Else
                                StatusDetails = "The URL responded but the expected text (" & myURL.SearchString & ") was not found."
                            End If

                        Case "String Found"
                            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " : String Found. The myURL.AlertStringFound is:", LogLevel.Normal)
                            If myURL.AlertStringFound = "0" Or myURL.AlertStringFound = "" Then
                                StatusDetails = "The expected text was found.  The URL responded in " & myURL.ResponseTime & " ms"
                                WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " : String Found. The expected text was found. So no issue.", LogLevel.Normal)
                            Else
                                StatusDetails = "The expected text was found.  The URL responded in " & myURL.ResponseTime & " ms"
                                WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " : String Found. The expected text was found. So it is an issue.", LogLevel.Normal)
                                myURL.Status = "Issue"
                            End If
                    End Select
                Catch ex As Exception
                    WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " Exception in URL content check module: " & ex.Message, LogUtilities.LogUtils.LogLevel.Normal)
                End Try

                Try
                    'MUKUND 15NOV13: copied below 2 lines from else condition to outside. As the status is OK, reset the 'Error'/'Not Responding'
                    'The 'Slow' status reseting remains in else condition          
                    If myURL.ResponseTime > myURL.ResponseThreshold Then
                        myURL.Status = "Slow"
                        StatusDetails = "Response Time: " & myURL.ResponseTime & " ms. Target is " & myURL.ResponseThreshold & " ms."
                        myURL.Description = "Response Time: " & myURL.ResponseTime & " ms. Target is " & myURL.ResponseThreshold & " ms."
                        myAlert.QueueAlert("URL", myURL.Name, "Slow", myURL.Name & " at " & myURL.URL & " responded in " & myURL.ResponseTime & " ms, but the Target is " & myURL.ResponseThreshold & " ms.", myURL.Location)
                    Else
                        myAlert.ResetAlert("URL", myURL.Name, "Slow", myURL.Location)
                        'myAlert.ResetAlert("URL", myURL.Name, "Error", myURL.Location)
                        'myAlert.ResetAlert("URL", myURL.Name, "Not Responding", myURL.Location)
                    End If
                Catch ex As Exception
                    WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " Error comparing URL response time to threshold: " & ex.Message)
                End Try

            End If

            If myURL.ResponseTime = 0 Then
                myURL.IncrementDownCount()
                UpdateURLStatisticsTable(myURL.Name, 0, myURL.ServerObjectID)
                If myURL.ResponseDetails <> "" Then
                    StatusDetails = myURL.ResponseDetails
                Else
                    StatusDetails = "The URL is not responding. Either the monitoring station has lost connectivity or the URL is down."
                    myURL.Description = "The URL is not responding. Either the monitoring station has lost connectivity or the URL is down."
                End If

                myURL.Status = "Not Responding"
                myURL.StatusCode = "Not Responding"
                myURL.AlertCondition = True
                WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " " & StatusDetails, LogLevel.Normal)
                myAlert.QueueAlert("URL", myURL.Name, "Not Responding", Now.ToString & " " & StatusDetails, myURL.Location)

                Percent = 0
            End If

        Catch ex As Exception
            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " URL Monitor error: " & ex.Message)
        End Try

        Dim PercentageChange As Double  'calculate the change in response time from prior scan

        Try
            If myURL.PreviousKeyValue > 0 And myURL.ResponseTime > 0 Then
                PercentageChange = -(1 - myURL.PreviousKeyValue / myURL.ResponseTime)
            Else
                PercentageChange = 0
            End If

        Catch ex As Exception
            PercentageChange = 0
        End Try
        If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " Previous response time was " & myURL.PreviousKeyValue & " Current value is " & myURL.ResponseTime & "; so percent change is: " & PercentageChange)

Update:
        myURL.LastScan = Now
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
            myURL.StatusCode = ServerStatusCode(myURL.Status)

        Catch ex As Exception
            myURL.StatusCode = vbNull
        Finally
            If MyLogLevel = LogLevel.Verbose Then WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " My status code is " & myURL.StatusCode)
        End Try

        'Try
        '    strSQL = "Update Status SET DownCount= " & myURL.DownCount & _
        '                    ", Status='" & myURL.Status & "', Upcount=" & myURL.UpCount & _
        '                    ", LastUpdate='" & FixDateTime(Now) & _
        '                    "', StatusCode='" & myURL.StatusCode & _
        '                    "', Location='URL', ResponseTime=" & Str(myURL.ResponseTime) & _
        '                    ", NextScan='" & FixDateTime(myURL.NextScan) & _
        '                    "'  WHERE TypeANDName='" & myURL.Name & "-URL' "
        '    WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " " & strSQL)
        '    UpdateStatusTable(strSQL)
        'Catch ex2 As Exception
        '    WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " URL Monitor Error while creating second try URL SQL statement: " & ex2.Message & vbCrLf)
        'End Try

        Try


            'With myURL
            '    '5/2/2016 NS modified for VSPLUS-2887
            '    strSQL = "Update Status SET DownCount= " & myURL.DownCount & _
            '    ", Status='" & myURL.Status & "', Upcount=" & myURL.UpCount & _
            '    ", UpPercent= " & myURL.UpPercentCount & _
            '    ", LastUpdate='" & FixDateTime(Now) & _
            '    "', StatusCode='" & .StatusCode & _
            '    "', Location='" & .Location & "', ResponseTime=" & Str(.ResponseTime) & _
            '    ", NextScan='" & FixDateTime(myURL.NextScan) & _
            '    "', PercentageChange='" & Str(PercentageChange) & _
            '    "', ResponseThreshold='" & myURL.ResponseThreshold & _
            '    "', MyPercent='" & strPercent & _
            '    "', Name='" & myURL.Name & "' " & _
            '    ", Details='" & StatusDetails & "' " & _
            '    ", Description='" & StatusDetails & "' " & _
            '    ", MailDetails='" & myURL.URL & "' " & _
            '    ", UpPercentMinutes='" & myURL.UpPercentMinutes & _
            '    "', UpMinutes='" & Microsoft.VisualBasic.Strings.Format(myURL.UpMinutes, "F1") & _
            '    "', DownMinutes='" & Microsoft.VisualBasic.Strings.Format(myURL.DownMinutes, "F1") & _
            '    "'  WHERE TypeANDName='" & myURL.Name & "-URL' "
            '    'TypeANDName is the key

            'End With

            Dim Typeandname As String = myURL.Name & "-URL"
            'Dim mongoStatement As New MongoStatementsUpsert(Of VSNext.Mongo.Entities.Status)()
            ' mongoStatement.filterDef = mongoStatement.repo.Filter.Where(Function(i) i.TypeAndName = Typeandname)
            ' mongoStatement.updateDef = mongoStatement.repo.Updater.[Set](Function(i) i.Name, myURL.Name).[Set](Function(i) i.CurrentStatus, myURL.Status).[Set](Function(i) i.StatusCode, myURL.StatusCode).[Set](Function(i) i.LastUpdated, DateTime.Now).[Set](Function(i) i.Location, myURL.Location).[Set](Function(i) i.Category, myURL.Category).[Set](Function(i) i.TypeAndName, myURL.Name & "-URL' ").[Set](Function(i) i.Description, "Microsoft").[Set](Function(i) i.ResponseTime, Integer.Parse(myURL.ResponseTime.ToString())).[Set](Function(i) i.ResponseThreshold, Integer.Parse(myURL.ResponseThreshold.ToString())).[Set](Function(i) i.Details, StatusDetails)



            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Status)(connectionString)
            Dim filterdef As FilterDefinition(Of VSNext.Mongo.Entities.Status) = repo.Filter.Eq(Function(i) i.TypeAndName, Typeandname)
            Dim updatedef As UpdateDefinition(Of VSNext.Mongo.Entities.Status) = repo.Updater _
                                                                                 .Set(Function(i) i.DeviceName, myURL.Name) _
                                                                                 .[Set](Function(i) i.CurrentStatus, myURL.Status) _
                                                                                 .[Set](Function(i) i.StatusCode, myURL.StatusCode) _
                                                                                 .[Set](Function(i) i.LastUpdated, DateTime.Now) _
                                                                                 .[Set](Function(i) i.Location, myURL.Location) _
                                                                                 .[Set](Function(i) i.Category, myURL.Category) _
                                                                                 .[Set](Function(i) i.Description, StatusDetails) _
                                                                                 .[Set](Function(i) i.ResponseTime, Integer.Parse(myURL.ResponseTime.ToString())) _
                                                                                 .[Set](Function(i) i.ResponseThreshold, Integer.Parse(myURL.ResponseThreshold.ToString())) _
                                                                                 .[Set](Function(i) i.Details, StatusDetails) _
                                                                                 .[Set](Function(i) i.DeviceType, myURL.ServerType) _
                                                                                 .[Set](Function(i) i.UpCount, Integer.Parse(myURL.UpCount)) _
                                                                                 .[Set](Function(i) i.UpPercent, Double.Parse(myURL.UpPercentCount)) _
                                                                                 .[Set](Function(i) i.DeviceId, myURL.ServerObjectID)
            repo.Upsert(filterdef, updatedef)

            'UpdateStatusTable(strSQL)
        Catch ex As Exception
            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " URL Monitor Error while creating URL SQL statement: " & ex.Message & vbCrLf & strSQL)
        End Try




    End Sub

    '12/12/16WS Moved to VSServices
    'Private Function SelectURLToMonitor() As MonitoredItems.URL
    '    ' WriteDeviceHistoryEntry("All", "URL_Performance", " Selecting a URL to monitor.")
    '    Dim tNow As DateTime
    '    tNow = Now
    '    Dim tScheduled As DateTime

    '    Dim timeOne, timeTwo As DateTime

    '    Dim SelectedServer As MonitoredItems.URL

    '    Dim ServerOne As MonitoredItems.URL
    '    Dim ServerTwo As MonitoredItems.URL
    '    Dim myRegistry As New RegistryHandler

    '    Dim n As Integer
    '    Dim strSQL As String = ""
    '    Dim ServerType As String = "URL"
    '    Dim serverName As String = ""

    '    Try
    '        strSQL = "Select svalue from ScanSettings where sname = 'Scan" & ServerType & "ASAP'"
    '        Dim ds As New DataSet()
    '        ds.Tables.Add("ScanASAP")
    '        Dim objVSAdaptor As New VSAdaptor
    '        objVSAdaptor.FillDatasetAny("VitalSigns", "VitalSigns", strSQL, ds, "ScanASAP")

    '        For Each row As DataRow In ds.Tables("ScanASAP").Rows
    '            Try
    '                serverName = row(0).ToString()
    '            Catch ex As Exception
    '                Continue For
    '            End Try

    '            For n = 0 To MyURLs.Count - 1
    '                ServerOne = MyURLs.Item(n)

    '                If ServerOne.Name = serverName And ServerOne.IsBeingScanned = False And ServerOne.Enabled Then
    '                    'WriteAuditEntry(Now.ToString & " >>> " & serverName & " was marked 'Scan ASAP' so that will be scanned next.")
    '                    strSQL = "DELETE FROM ScanSettings where sname = 'Scan" & ServerType & "ASAP' and svalue='" & serverName & "'"
    '                    objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "VitalSigns", strSQL)

    '                    Return ServerOne
    '                    Exit Function

    '                End If
    '            Next
    '        Next

    '    Catch ex As Exception

    '    End Try



    '    'Any server Not Responding that is due for a scan should be scanned right away.  Select the first one you encounter
    '    For n = 0 To MyURLs.Count - 1
    '        ServerOne = MyURLs.Item(n)
    '        If ServerOne.Status = "Not Responding" And ServerOne.Enabled = True And ServerOne.IsBeingScanned = False Then
    '            tScheduled = CDate(ServerOne.NextScan)
    '            If DateTime.Compare(tNow, tScheduled) > 0 Then
    '                WriteDeviceHistoryEntry("All", "Performance", Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status & ".  Next scheduled scan is " & tScheduled.ToShortTimeString)
    '                'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status)
    '                'ServerOne.IsBeingScanned = True
    '                Return ServerOne
    '                Exit Function
    '            Else
    '                'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " " & ServerOne.Name & " is down, but not yet due to be re-scanned.")
    '            End If
    '        End If
    '    Next

    '    'Any server Not Scanned should be scanned right away.  Select the first one you encounter
    '    For n = 0 To MyURLs.Count - 1
    '        ServerOne = MyURLs.Item(n)
    '        If ServerOne.Status = "Not Scanned" Or ServerOne.Status = "Master Service Stopped." And ServerOne.Enabled = True And ServerOne.IsBeingScanned = False Then
    '            ' WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " >>> Selecting " & ServerOne.Name & " because status is " & ServerOne.Status)
    '            ' ServerOne.IsBeingScanned = True
    '            Return ServerOne
    '            Exit Function
    '        End If
    '    Next


    '    Dim ScanCandidates As New MonitoredItems.URLCollection
    '    For Each srv As MonitoredItems.URL In MyURLs
    '        If srv.IsBeingScanned = False And srv.Enabled = True Then
    '            ScanCandidates.Add(srv)
    '            ' WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " " & srv.Name & " is not being scanned.")
    '        Else
    '            ' WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " " & srv.Name & " is being scanned.")
    '        End If
    '    Next
    '    ' WriteDeviceHistoryEntry("All", "URL_Performance", vbCrLf & vbCrLf)
    '    ' WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " *********** Scan Candidates *************")
    '    For Each srv As MonitoredItems.URL In ScanCandidates
    '        ' WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " " & srv.Name & " is a candidate to be scanned.  Last scan: " & srv.LastScan)
    '    Next

    '    If ScanCandidates.Count = 0 Then
    '        ' WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " All servers are already being scanned, exiting sub.")
    '        Return Nothing
    '    End If

    '    '*****************

    '    'start with the first two servers
    '    ServerOne = ScanCandidates.Item(0)
    '    ServerOne = ScanCandidates.Item(0)
    '    If ScanCandidates.Count > 1 Then ServerTwo = ScanCandidates.Item(1)

    '    Try
    '        'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Finding which server is the most overdue to be monitored.")
    '        'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Server One is  " & ServerOne.Name & ", due to be scanned " & ServerOne.NextScan)
    '        'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Server Two " & ServerTwo.Name & ", due to be scanned " & ServerTwo.NextScan)
    '    Catch ex As Exception
    '    End Try


    '    'go through the remaining servers, see which one has the oldest (earliest) scheduled time
    '    If ScanCandidates.Count > 2 Then
    '        Try
    '            For n = 2 To ScanCandidates.Count - 1
    '                '           WriteAuditEntry(Now.ToString & " N is " & n)
    '                timeOne = CDate(ServerOne.NextScan)
    '                timeTwo = CDate(ServerTwo.NextScan)
    '                If DateTime.Compare(timeOne, timeTwo) < 0 Then
    '                    'time one is earlier than time two, so keep server 1
    '                    'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " " & ServerOne.Name & " is more overdue ")
    '                    ServerTwo = ScanCandidates.Item(n)
    '                Else
    '                    'time two is later than time one, so keep server 2
    '                    ServerOne = ScanCandidates.Item(n)
    '                    'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " " & ServerTwo.Name & " is more overdue ")
    '                End If

    '                'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Server One is  " & ServerOne.Name & ", due to be scanned " & ServerOne.NextScan)
    '                'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " Server Two " & ServerTwo.Name & ", due to be scanned " & ServerTwo.NextScan)

    '            Next
    '        Catch ex As Exception
    '            ' WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " >>> Error Selecting URL server... " & ex.Message)
    '        End Try
    '    Else
    '        'There were only two server, so use those going forward
    '    End If

    '    'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " >>> Down to two servers... " & ServerOne.Name & " and " & ServerTwo.Name)

    '    'Of the two remaining servers, pick the one with earliest scheduled time for next scan
    '    If Not (ServerTwo Is Nothing) Then
    '        timeOne = CDate(ServerOne.NextScan)
    '        timeTwo = CDate(ServerTwo.NextScan)

    '        If DateTime.Compare(timeOne, timeTwo) < 0 Then
    '            'time one is earlier than time two, so keep server 1
    '            SelectedServer = ServerOne
    '            tScheduled = CDate(ServerOne.NextScan)
    '        Else
    '            SelectedServer = ServerTwo
    '            tScheduled = CDate(ServerTwo.NextScan)
    '        End If
    '        tNow = Now
    '        'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " >>> Down to one server... " & SelectedServer.Name & " to scan at " & SelectedServer.NextScan & ". Status is " & SelectedServer.Status)
    '    Else
    '        SelectedServer = ServerOne
    '        tScheduled = CDate(ServerOne.NextScan)
    '    End If

    '    tScheduled = CDate(SelectedServer.NextScan)
    '    If DateTime.Compare(tNow, tScheduled) < 0 Then
    '        If SelectedServer.Status <> "Not Scanned" Then
    '            'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " No Domino servers scheduled for monitoring, next scan after " & SelectedServer.NextScan)
    '            SelectedServer = Nothing
    '        Else
    '            ' WriteDeviceHistoryEntry("All", "URL_Performance", Now.ToString & " selected URL server: " & SelectedServer.Name & " because it has not been scanned yet.")
    '        End If
    '    Else
    '        'WriteDeviceHistoryEntry("All", "Selection",Now.ToString & " selected Domino server: " & SelectedServer.Name)
    '    End If

    '    '**************

    '    'Release Memory
    '    tNow = Nothing
    '    tScheduled = Nothing
    '    n = Nothing

    '    timeOne = Nothing
    '    timeTwo = Nothing

    '    ServerOne = Nothing
    '    ServerTwo = Nothing

    '    'return selected server
    '    ' SelectedServer.IsBeingScanned = True
    '    SelectURLToMonitor = SelectedServer


    '    'Exit Function
    '    SelectedServer = Nothing
    'End Function


    Public Sub URLResponseTimeChilkat(ByRef myURL As MonitoredItems.URL)
        'This function returns as the number of milliseconds it takes to open a URL and retrieve its content
        'returns 0 in case of exception
        'returns -1 if it cannot connect
        Try
            '  WriteAuditEntry(Now.ToString & " Querying " & myURL.URL & " for response time using Chilkat component.")
            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " Querying " & myURL.URL & " for response time using Chilkat component.")
        Catch ex As Exception

        End Try

        Try
            myURL.PreviousKeyValue = myURL.ResponseTime   'remember what the time was last time, so you can compare
        Catch ex As Exception

        End Try

        Dim start, done, hits As Long
        Dim elapsed As TimeSpan
        start = Now.Ticks
        Dim ResponseTime As Double = 0
        Dim ChilkatHTTP As New Chilkat.Http
        myURL.HTML = ""
        ChilkatHTTP.FollowRedirects = True
        ChilkatHTTP.SaveCookies = True
        ChilkatHTTP.SendCookies = True
        ChilkatHTTP.CookieDir = "memory"
        Try
            Dim success As Boolean
            success = ChilkatHTTP.UnlockComponent("MZLDADHttp_efwTynJYYR3X")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error unlocking Chilkat component: " & ex.ToString)
            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " Error unlocking component: " & ex.ToString)
        End Try

        Try
            Dim myRegistry As New RegistryHandler
            If myRegistry.ReadFromRegistry("ProxyEnabled") = "True" Then
                WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " Attempting to route through proxy server.")
                ChilkatHTTP.SocksHostname = myRegistry.ReadFromRegistry("ProxyServer")
                ChilkatHTTP.SocksPort = myRegistry.ReadFromRegistry("ProxyPort")
                ChilkatHTTP.SocksUsername = myRegistry.ReadFromRegistry("ProxyUser")
                ChilkatHTTP.SocksPassword = myRegistry.ReadFromRegistry("ProxyPassword")
            End If
            myRegistry = Nothing
        Catch ex As Exception

        End Try


        Try
            'Dim h As System.Net.HttpWebRequest
            'h = CType(System.Net.WebRequest.Create(myURL.URL), System.Net.HttpWebRequest)
            If myURL.UserName <> "" Then
                'Dim c As New System.Net.NetworkCredential
                'c.UserName = myURL.UserName
                'c.Password = myURL.Password
                WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " This URL requires a username and password.")
                ChilkatHTTP.Login = myURL.UserName
                ChilkatHTTP.Password = myURL.Password
                ' h.Credentials = c
            End If

            Dim n As Integer = 0
            'Dim r As System.Net.HttpWebResponse
            Do While myURL.HTML IsNot Nothing AndAlso myURL.HTML = ""
                'myURL.HTML = ChilkatHTTP.QuickGetStr(myURL.URL)
                Try
                    WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " before getting chilkat response ")
                    myURL.HTML = ChilkatHTTP.QuickGetStr(myURL.URL)
                    WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " after getting chilkat response code: " + ChilkatHTTP.LastStatus.ToString())
                    If myURL.HTML IsNot Nothing Then
                        WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " after getting chilkat response " + Left(myURL.HTML.ToString(), 100))
                    Else
                        WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " after getting chilkat response. HTML is NOTHING")
                    End If
                    If myURL.HTML Is Nothing Then
                        myURL.HTML = ""
                    End If
                    'WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " before getting other response ")
                    'r = h.GetResponse
                    'WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " after getting response ")
                Catch ex As Exception
                    WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " exception getting response " + ex.Message.ToString())
                    myURL.ResponseDetails = ex.Message.ToString
                    myURL.Status = "Not Responding"
                    myURL.Description = ex.Message.ToString
                    myURL.ResponseTime = 0
                    'Exit Sub

                End Try
                'Dim s As New System.IO.StreamReader(r.GetResponseStream())
                'myURL.HTML = s.ReadToEnd()
                n += 1
                Thread.Sleep(500)
                If n > 5 Then Exit Do
            Loop
            If (myURL.HTML = "") Then
                If (ChilkatHTTP.LastStatus <> 200) Then
                    myURL.ResponseDetails = "Page returned with no data."
                    myURL.Status = "Not Responding"
                    myURL.Description = "Page returned with no data."
                    myURL.ResponseTime = 0
                    Exit Sub
                End If
            End If

            If (ChilkatHTTP.LastStatus > 399 And ChilkatHTTP.LastStatus < 499) Then

                myURL.ResponseDetails = "URL returned with error code:" + ChilkatHTTP.LastStatus.ToString() + "."
                myURL.Status = "OK"
                myURL.Description = "URL returned with error code:" + ChilkatHTTP.LastStatus.ToString() + "."
                done = Now.Ticks
                elapsed = New TimeSpan(done - start)
                myURL.ResponseTime = elapsed.TotalMilliseconds
                Exit Sub
            End If
            'should cover all the 400's and 500's errors
            If (ChilkatHTTP.LastStatus > 500 And ChilkatHTTP.LastStatus < 600) Then

                myURL.ResponseDetails = "URL returned with error code:" + ChilkatHTTP.LastStatus.ToString() + "."
                myURL.Status = "Not Responding"
                myURL.Description = "URL returned with error code:" + ChilkatHTTP.LastStatus.ToString() + "."
                myURL.ResponseTime = 0
                Exit Sub
            End If
            'If (r IsNot Nothing AndAlso (r.StatusCode > 399 And r.StatusCode < 600)) Then

            '    myURL.ResponseDetails = "URL returned with error code:" + r.StatusCode.ToString() + " (" + r.StatusDescription + ")."
            '    myURL.Status = "Not Responding"
            '    myURL.Description = "URL returned with error code:" + r.StatusCode.ToString() + " (" + r.StatusDescription + ")."
            '    myURL.ResponseTime = 0
            '    Exit Sub
            'End If

            done = Now.Ticks
            elapsed = New TimeSpan(done - start)
            myURL.ResponseTime = elapsed.TotalMilliseconds
            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " URL responded in " & elapsed.TotalMilliseconds & " ms.")
            '  WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " URL returned the following HTML: " & vbCrLf & HTML)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error performing HTTP.Get: " & ex.Message)
            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " Error performing HTTP.Get: " & ex.Message)
            myURL.ResponseDetails = ex.Message.Replace("URl", "URL")
            myURL.Description = ex.Message.Replace("URl", "URL")
            myURL.Status = "Error"
            myURL.ResponseTime = 0
        End Try



    End Sub

    Public Function URLContentCheck(ByRef myURL As MonitoredItems.URL) As String
        'Returns 

        Dim returnValue As String = ""

        WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " Checking the URL content.")
        If InStr(myURL.HTML, "Notes Exception") Then
            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " This URL has a Notes Exception error in the response. That's bad.  Here's what we got:" & vbCrLf & vbCrLf & myURL.HTML, )
            myAlert.QueueAlert("URL", myURL.Name, "Domino Exception", "This URL " & myURL.Name & " at " & myURL.URL & " is returning a Lotus Notes Exception.  Here's what we got:" & vbCrLf & vbCrLf & myURL.HTML, myURL.Location)
            myURL.Status = "Domino Exception"
            myURL.ResponseDetails = "This URL has a Notes Exception."
            returnValue = "Domino Exception"
        Else
            myAlert.ResetAlert("URL", myURL.Name, "Domino Exception", myURL.Location)
            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " This URL does not have a Lotus Notes Exception error in the response. ", LogLevel.Verbose)
        End If


        If myURL.SearchString = "" Then
            returnValue = "N/A"
        Else
            WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " This response will be checked for the specified search string of " & myURL.SearchString)
            If InStr(myURL.HTML, myURL.SearchString) Then
                'myURL.ResponseDetails = myURL.ResponseDetails & " The search string " & myURL.SearchString & " was found."
                WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " This response contains the correct text.")
                URLStringFound = True
                returnValue = "String Found"
                If myURL.AlertStringFound = "0" Or myURL.AlertStringFound = "" Then
                    myAlert.ResetAlert("URL", myURL.Name, "Search String", myURL.Location)
                Else
                    myAlert.QueueAlert("URL", myURL.Name, "Search String", "This URL " & myURL.Name & " at " & myURL.URL & " contains the string (" & myURL.SearchString & ") we were searching for. ", myURL.Location)
                End If

            Else
                WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " The string we were looking for was not found!", LogLevel.Verbose)
                ' WriteDeviceHistoryEntry("URL", myURL.Name, Now.ToString & " The response we got was  " & vbCrLf & myURL.HTML, LogLevel.Verbose)
                URLStringFound = False
                returnValue = "String Not Found"
                If myURL.AlertStringFound = "0" Or myURL.AlertStringFound = "" Then
                    myAlert.QueueAlert("URL", myURL.Name, "Search String", "This URL " & myURL.Name & " at " & myURL.URL & " does not contain the string (" & myURL.SearchString & ") we were searching for. ", myURL.Location)
                Else
                    myAlert.ResetAlert("URL", myURL.Name, "Search String", myURL.Location)
                End If

            End If
        End If

        Return returnValue

    End Function

#End Region

End Class
