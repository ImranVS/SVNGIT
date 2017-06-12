Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports VSFramework
Imports System.IO
Imports System.Globalization
Imports LogUtilities
Imports VSNext.Mongo
Imports VSNext.Mongo.Entities
Imports VSNext.Mongo.Repository
Imports MongoDB.Driver
Imports System.Linq



Public Class MaintenanceDll
    Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("VitalSignsMongo").ToString()
    ''' <summary>
    ''' This function returns true if in maintenance, false if not. Current date time is checked against settings given.
    ''' </summary>
    ''' <param name="DeviceType">Type of Server</param>
    ''' <param name="DeviceName">Name of Server</param>
    ''' <returns>True/ False</returns>
    ''' <remarks></remarks>
    ''' New Version 17Sep13:Mukund D
    Public Function InMaintenance(ByVal DeviceType As String, ByVal DeviceName As String) As Boolean
        Dim InMaintenanceWindow As Boolean = False
        Dim connString As String = GetDBConnection()
        Dim repoServers As New Repository(Of Server)(connString)
        Dim filterServers As FilterDefinition(Of Server)
        Dim serversEntity() As Server
        Dim repoMaint As New Repository(Of Maintenance)(connString)
        Dim filterMaint As FilterDefinition(Of Maintenance)
        Dim maintEntity As New List(Of Maintenance)
        Dim dt As DateTime
        Dim dt1 As DataTable
        Dim dr As DataRow

        dt = DateTime.Now
        Dim dtUTC As DateTime = DateTime.SpecifyKind(dt, DateTimeKind.Utc)
        Try
            dt1 = New DataTable
            dt1.Columns.Add("ID")
            dt1.Columns.Add("ServerName")
            dt1.Columns.Add("ServerType")
            dt1.Columns.Add("Name")
            dt1.Columns.Add("StartDate")
            dt1.Columns.Add("StartTime")
            dt1.Columns.Add("Duration")
            dt1.Columns.Add("EndDate")
            dt1.Columns.Add("MaintType")
            dt1.Columns.Add("MaintDaysList")
            filterMaint = repoMaint.Filter.And(repoMaint.Filter.Lte(Function(j) j.StartDate, dt.Date),
                                            repoMaint.Filter.Gte(Function(j) j.EndDate, dt.Date))

            Dim t = repoMaint.Find(filterMaint).ToList()
            For Each entry As Maintenance In t
                If entry.StartDate.TimeOfDay.CompareTo(New TimeSpan(0)) <> 0 Then
                    If maintEntity.Where(Function(x) x.Id = entry.Id).Count = 0 Then
                        maintEntity.Add(entry)
                    End If
                End If
            Next
            maintEntity = repoMaint.Find(filterMaint).ToList()

            filterMaint = repoMaint.Filter.And(repoMaint.Filter.Lte(Function(j) j.StartDate, dtUTC.Date),
                                            repoMaint.Filter.Gte(Function(j) j.EndDate, dtUTC.Date))

            t = repoMaint.Find(filterMaint).ToList()
            For Each entry As Maintenance In t
                entry.StartDate = DateTime.SpecifyKind(entry.StartDate.ToUniversalTime, DateTimeKind.Local)
                entry.EndDate = DateTime.SpecifyKind(entry.EndDate.ToUniversalTime, DateTimeKind.Local)
                If entry.StartDate.TimeOfDay.CompareTo(New TimeSpan(0)) = 0 Then
                    If maintEntity.Where(Function(x) x.Id = entry.Id).Count = 0 Then
                        maintEntity.Add(entry)
                    End If
                End If
            Next

            If maintEntity.Count > 0 Then
                filterServers = repoServers.Filter.And(repoServers.Filter.Eq(Of String)(Function(j) j.DeviceName, DeviceName),
                                                   repoServers.Filter.Eq(Of String)(Function(j) j.DeviceType, DeviceType),
                                                   repoServers.Filter.Exists(Function(j) j.MaintenanceWindows, True))
                serversEntity = repoServers.Find(filterServers).ToArray()
                If serversEntity.Length > 0 Then
                    For x As Integer = 0 To maintEntity.Count - 1
                        For i As Integer = 0 To serversEntity.Length - 1
                            For j As Integer = 0 To serversEntity(i).MaintenanceWindows.Count - 1
                                If serversEntity(i).MaintenanceWindows(j) = maintEntity(x).Id Then
                                    dr = dt1.NewRow()
                                    dr("ID") = serversEntity(i).MaintenanceWindows(j)
                                    dr("ServerName") = serversEntity(i).DeviceName
                                    dr("ServerType") = serversEntity(i).DeviceType
                                    dr("Name") = maintEntity(x).Name
                                    dr("StartDate") = maintEntity(x).StartDate
                                    dr("StartTime") = maintEntity(x).StartTime
                                    dr("Duration") = maintEntity(x).Duration
                                    dr("EndDate") = maintEntity(x).EndDate
                                    dr("MaintType") = maintEntity(x).MaintainType
                                    dr("MaintDaysList") = maintEntity(x).MaintenanceDaysList
                                    dt1.Rows.Add(dr)
                                End If
                            Next
                        Next
                    Next
                Else
                    'If no server records have maintenance windows defined, exit function with False
                    Return False
                    Exit Function
                End If
            Else
                'If no maintenance records found, exit function with False
                Return False
                Exit Function
            End If
        Catch ex As Exception
            WriteHistoryEntry(Now.ToString & " Server " & DeviceName & " Error in maint window module dataset creation: " & ex.Message)
            Return False
            Exit Function
        End Try

        Dim b2 As New DateTimeExtensions
        Dim daynum As Integer
        Dim MaintType As String
        Dim MaintDayList As String
        Dim StartDate, StartTime, EndDate As DateTime
        Dim MyStartTime, MyEndTime As DateTime
        Dim MinTime, MaxTime As TimeSpan
        Dim TimeNow As DateTime = Now
        Dim Wknum, Duration, StartWknum, TodayWknum As Integer
        Dim dayOfWeekT As Integer

        Wknum = b2.GetWeekOfMonth(Convert.ToDateTime(Now)) 'Format(Now.Date, "w")

        Try
            WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName & " has " & dt1.Rows.Count & " maintenance windows.")
            For Each dr In dt1.Rows

                Try
                    MyStartTime = CType(dr.Item("StartTime"), DateTime)
                    MaintType = dr.Item("MaintType").ToString()
                    MaintDayList = dr.Item("MaintDaysList").ToString()
                    StartDate = CType(dr.Item("StartDate"), DateTime)
                    StartTime = CType(dr.Item("StartTime"), DateTime)
                    MinTime = StartTime.TimeOfDay
                    Duration = CType(dr.Item("Duration"), Integer)
                    EndDate = CType(dr.Item("EndDate"), DateTime)
                    'for case 3,to get the week nos from the start date
                    StartWknum = DatePart(DateInterval.WeekOfYear, StartDate)
                    TodayWknum = DatePart(DateInterval.WeekOfYear, Now)
                    Select Case MaintType
                        Case "1"
                            'One time
                            MyStartTime = StartDate + StartTime.TimeOfDay
                            MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
                            If Now >= MyStartTime And Now < MyEndTime Then
                                InMaintenanceWindow = True
                                Exit For
                            End If
                        Case "2"
                            '7/30/2016 NS modified for VSPLUS-3127
                            'Daily
                            'First, check whether today falls within the date interval
                            If Today >= StartDate.Date And Today <= EndDate.Date Then
                                MyStartTime = Now.Date + StartTime.TimeOfDay
                                MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
                                MaxTime = MyEndTime.TimeOfDay
                                'If the time interval spans midnight (end time is less than the start time),
                                'subtract a day from MyStartTime
                                'Example 1: MyStartTime = 7/30/2016 4:00 PM, MyEndTime = 7/31/2016 2:00 AM, 
                                'MinTime = 4:00 PM, MaxTime = 2:00 AM, Now = 7/30/2016 3:00 PM
                                'Example 2: MyStartTime = 7/30/2016 4:00 PM, MyEndTime = 7/31/2016 2:00 AM, 
                                'MinTime = 4:00 PM, MaxTime = 2:00 AM, Now = 7/30/2016 6:00 PM
                                If MaxTime < MinTime And Now < MyStartTime Then
                                    MyStartTime = MyStartTime.AddDays(-1)
                                End If
                                'Example 1: MyStartTime = 7/29/2016 4:00 PM
                                'Example 2: MyStartTime = 7/30/2016 4:00 PM
                                'Re-calculate end date/time based on duration
                                MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
                                'Example 1: MyEndTime = 7/30/2016 2:00 AM
                                'Example 2: MyEndTime = 7/31/2016 2:00 AM
                                WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName & ";  MyStartTime - " & MyStartTime.ToString() & "; MyEndTime - " & MyEndTime.ToString() & "; Now - " & Now.ToString())
                                'Example 1: Is 7/30/2016 3:00 PM within interval 7/29/2016 4:00 PM and 7/30/2016 2:00 AM - server is NOT in maintenance
                                'Example 2: Is 7/30/2016 6:00 PM within interval 7/30/2016 4:00 PM and 7/31/2016 2:00 AM - server is in maintenance
                                If Now >= MyStartTime And Now < MyEndTime Then
                                    WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName & " is in maintenance.")
                                    InMaintenanceWindow = True
                                    Exit For
                                Else
                                    WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName & " is NOT in maintenance.")
                                End If
                            End If
                        Case "3"
                            'Weekly
                            Dim wkType As Array = MaintDayList.Split(",")
                            '8/2/2016 NS modified for VSPLUS-3144
                            'First, check whether today falls within the date interval
                            If Today >= StartDate.Date And Today <= EndDate.Date Then
                                MyStartTime = Now.Date + StartTime.TimeOfDay
                                MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
                                MaxTime = MyEndTime.TimeOfDay
                                If MaxTime < MinTime And Now < MyStartTime Then
                                    MyStartTime = MyStartTime.AddDays(-1)
                                End If
                                MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
                                TodayWknum = DatePart(DateInterval.WeekOfYear, MyStartTime)
                                For Each i In wkType
                                    Dim WKday As Array = i.Split(":")
                                    If (TodayWknum - StartWknum) Mod WKday(1) = 0 Then
                                        'If Wknum = WKday(1) Then
                                        dayOfWeekT = IIf(MyStartTime.DayOfWeek = 0, 7, MyStartTime.DayOfWeek)
                                        WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName &
                                                                "; MyStartTime - " & MyStartTime.ToString() &
                                                                "; MyEndTime - " & MyEndTime.ToString() &
                                                                "; Now - " & Now.ToString() &
                                                                "; StartWkNum - " & StartWknum.ToString() &
                                                                "; TodayWknum - " & TodayWknum.ToString() &
                                                                "; WKday(1) - " & WKday(1).ToString() &
                                                                "; WKday(0) - " & WKday(0).ToString() &
                                                                "; dayOfWeekT - " & dayOfWeekT)
                                        If dayOfWeekT = WKday(0) Then
                                            If Now >= MyStartTime And Now < MyEndTime Then
                                                WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName & " is in maintenance.")
                                                InMaintenanceWindow = True
                                                Exit For
                                            Else
                                                WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName & " is NOT in maintenance.")
                                            End If
                                        Else
                                            WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName & " is NOT in maintenance.")
                                        End If
                                    End If
                                Next
                            End If
                        Case "4"
                            'Monthly
                            Dim x, y As Integer
                            'First, check whether today falls within the date interval
                            If Today >= StartDate.Date And Today <= EndDate.Date Then
                                x = MaintDayList.IndexOf(",")
                                y = MaintDayList.IndexOf(":")
                                MyStartTime = Now.Date + StartTime.TimeOfDay
                                MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
                                MaxTime = MyEndTime.TimeOfDay
                                If MaxTime < MinTime And Now < MyStartTime Then
                                    MyStartTime = MyStartTime.AddDays(-1)
                                End If
                                MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
                                'Maintenance window is set for a specific day of the month, 1-31
                                If x < 0 And y < 0 Then
                                    If MaintDayList = Format(MyStartTime, "dd") And Now >= MyStartTime And Now < MyEndTime Then
                                        WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName & " is in maintenance.")
                                        InMaintenanceWindow = True
                                        Exit For
                                    Else
                                        WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName & " is NOT in maintenance.")
                                    End If
                                    'Maintenance window is set for a specific week of the month, First, Second, Third, Last
                                Else
                                    daynum = Weekday(MyStartTime, FirstDayOfWeek.Monday) 'CType(DayofWeek, Integer) CHECK THIS -1
                                    Wknum = b2.GetWeekOfMonth(Convert.ToDateTime(MyStartTime)) 'Format(Now.Date, "w")
                                    Dim MonthType As Array = MaintDayList.Split(",")
                                    For Each i In MonthType
                                        Dim mnday As Array = i.Split(":")
                                        WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName &
                                                                "; daynum - " & daynum.ToString() &
                                                                "; mnday(0) - " & mnday(0).ToString())
                                        If daynum = mnday(0) Then
                                            ' ****************** 
                                            Dim wkname As String = ""
                                            Select Case Wknum
                                                Case "1"
                                                    wkname = "First"
                                                Case "2"
                                                    wkname = "Second"
                                                Case "3"
                                                    wkname = "Third"
                                                Case Else
                                                    wkname = "Last"
                                            End Select
                                            WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName &
                                                                "; wkname - " & wkname.ToString() &
                                                                "; mnday(1) - " & mnday(1).ToString())
                                            If wkname = mnday(1) Then
                                                WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName &
                                                                "; MyStartTime - " & MyStartTime.ToString() &
                                                                "; MyEndTime - " & MyEndTime.ToString() &
                                                                "; Now - " & Now.ToString())
                                                If Now >= MyStartTime And Now < MyEndTime Then
                                                    WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName & " is in maintenance.")
                                                    InMaintenanceWindow = True
                                                    Exit For
                                                Else
                                                    WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName & " is NOT in maintenance.")
                                                End If
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                    End Select
                Catch ex As Exception
                    WriteHistoryEntry(Now.ToString & " Server " & DeviceName & " Error calculating maintenance window start and end times. Error: " & ex.Message)
                End Try
            Next
            dr = Nothing
        Catch ex As Exception
            WriteHistoryEntry(Now.ToString & " Server " & DeviceName & " Error in maint window module: " & ex.Message)
        End Try

        Try
            GC.Collect()
        Catch ex As Exception
            WriteHistoryEntry(Now.ToString & " Server " & DeviceName & " Error in maint window module: " & ex.Message)

        End Try

        WriteHistoryEntry(Now.ToString & " Server " & DeviceName & " has " & dt1.Rows.Count & " maintenance windows. It is now in Maintenance = " & InMaintenanceWindow)
        Return InMaintenanceWindow
    End Function

	Private Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String, Optional ByVal LogLevelInput As LogUtils.LogLevel = LogUtils.LogLevel.Normal)
		LogUtils.WriteDeviceHistoryEntry(DeviceType, DeviceName, strMsg, LogLevelInput)
	End Sub

    'Public Function OffHours(ByVal servername As String) As Boolean
    '	Dim strSQL As String
    '	Dim dt As New DataTable
    '	Dim ds As New DataSet
    '	Dim vsobj As New VSAdaptor
    '	Dim isoffhours As Boolean

    '	Try
    '		'strSQL = "select hr.Starttime,hr.Duration, hr.is" & DateTime.Now.ToString("dddd") & " DayOfWeek from [servers] sr inner join HoursIndicator hr on sr.BusinesshoursID=hr.ID where sr.ServerName='" & servername & "'"
    '		'6/8/15 WS modified for VSPLUS-1816
    '		strSQL = "exec InBusinessHoursByServer '" & servername & "'"
    '		dt.TableName = "HoursIndicator"
    '		ds.Tables.Add(dt)

    '		vsobj.FillDatasetAny("VitalSigns", "vitalsigns", strSQL, ds, "HoursIndicator")
    '		If dt.Rows.Count = 1 Then
    '			Dim isHours As String = dt(0)("InBusinessHours").ToString()

    '			If (isHours = "1") Then
    '				Return False
    '			Else
    '				Return True
    '			End If

    '		End If

    '		Return False

    '	Catch ex As Exception
    '		WriteHistoryEntry(Now.ToString & " Error in Business Hours module: " & ex.Message)

    '	End Try
    '	Return isoffhours
    '   End Function

    Public Function OffHours(ByVal servername As String) As Boolean
        Dim strSQL As String
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim vsobj As New VSAdaptor
        Dim isoffhours As Boolean

        Dim businessId As String = ""
        Dim filterdefBusinessHours As MongoDB.Driver.FilterDefinition(Of VSNext.Mongo.Entities.BusinessHours)
        Dim currentBH As VSNext.Mongo.Entities.BusinessHours
        Dim businessHours As List(Of VSNext.Mongo.Entities.BusinessHours)

        Try
            Dim repo As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.Server)(connectionString)
            Dim repoBusinessHours As New VSNext.Mongo.Repository.Repository(Of VSNext.Mongo.Entities.BusinessHours)(connectionString)
            Dim filterdef As MongoDB.Driver.FilterDefinition(Of VSNext.Mongo.Entities.Server) = repo.Filter.Where(Function(i) i.DeviceName.Equals(servername))
            Dim projectDef As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repo.Project.Include(Function(i) i.BusinessHoursId)
            Dim projectDefBusinessHours As ProjectionDefinition(Of VSNext.Mongo.Entities.Server) = repo.Project.Include(Function(i) i.BusinessHoursId)
            Dim server As VSNext.Mongo.Entities.Server = repo.Find(filterdef, projectDef).FirstOrDefault()
            If Not server Is Nothing Then
                businessId = server.BusinessHoursId
                filterdefBusinessHours = repoBusinessHours.Filter.Where(Function(i) i.Id.Equals(server.BusinessHoursId))
            End If

            businessHours = repoBusinessHours.All().ToList()
            If Not businessHours Is Nothing Then
                For Each BH As VSNext.Mongo.Entities.BusinessHours In businessHours
                    If BH.ObjectId.ToString = businessId Then
                        currentBH = BH
                        Exit For
                    End If
                Next
            End If

            Dim bCurrentDay As Boolean = False
            Dim bcurrntTime As Boolean = False
            If currentBH IsNot Nothing Then
                For Each s As String In currentBH.Days
                    If s = Now.DayOfWeek.ToString() Then
                        bCurrentDay = True
                    End If
                Next
                ' Dim startTime As String() = currentBH.StartTime.Split(":")
                Dim dtStart As DateTime = Now.ToShortDateString + " " + currentBH.StartTime
                Dim duration As Integer = currentBH.Duration
                Dim days As String() = currentBH.Days
                Dim dtFuture As DateTime = dtStart.AddMinutes(duration)

                If dtFuture.Ticks < dtStart.Ticks Then
                    'EndTime is time for next day  
                    If (Now.Ticks >= dtStart.Ticks And Now.Ticks >= dtFuture.Ticks) Or _
                        (Now.Ticks <= dtStart.Ticks And Now.Ticks <= dtFuture.Ticks) Then
                        Console.WriteLine("Time is within range.")
                        bcurrntTime = True
                    Else
                        Console.WriteLine("Time is outside of range.")
                        bcurrntTime = False
                    End If
                Else
                    If Now.Ticks >= dtStart.Ticks And Now.Ticks <= dtFuture.Ticks Then
                        Console.WriteLine("Time is within range.")
                        bcurrntTime = True
                    Else
                        Console.WriteLine("Time is outside of range.")
                        bcurrntTime = False
                    End If
                End If
            Else
                ' if a server is not associated with a business hour record, we will treat it as working hour
                isoffhours = False
            End If

            If bCurrentDay And bcurrntTime Then
                isoffhours = False
            Else
                isoffhours = True
            End If
        Catch ex As Exception
            WriteHistoryEntry(Now.ToString & " Error in Business Hours module: " & ex.Message)

        End Try
        Return isoffhours
    End Function

    ''' <summary>
    ''' Fetches values from Settings table
    ''' </summary>
    ''' <param name="Sname">Name of the Setting</param>
    ''' <returns>Setting value</returns>
    ''' <remarks></remarks>
    Public Function Settings(ByVal Sname As String) As String
        Dim DsSettings As New Data.DataSet
        Dim objVSAdaptor As New VSAdaptor
        Dim Svalue As String = ""
        Dim strSQL As String
        Try
            strSQL = "Select svalue from Settings Where sname='" & Sname & "'"
            objVSAdaptor.FillDatasetAny("VitalSigns", "None", strSQL, DsSettings, "Settings")
            Svalue = DsSettings.Tables("Settings").Rows(0)("svalue").ToString()
        Catch ex As ApplicationException
        End Try
        Return Svalue
    End Function

    Public Function isTodayBusinessDay(ByVal dayName As String) As Boolean
        Dim InBusinessHours As Boolean = False
        Try
            If Convert.ToBoolean(Convert.ToInt32(Settings(dayName))) = True Then
                InBusinessHours = True
            Else
                InBusinessHours = False
            End If
        Catch ex As Exception
            InBusinessHours = False
            WriteHistoryEntry(Now.ToString & "  Exception in isTodayBusiness Day " & ex.Message)
        End Try

        Return InBusinessHours
    End Function

	Private Sub WriteHistoryEntry(ByVal strMsg As String, Optional ByVal LogLevelInput As LogUtils.LogLevel = LogUtils.LogLevel.Normal)
		LogUtils.WriteHistoryEntry(strMsg, "All_Maintenance_Log.txt", LogLevelInput)
	End Sub

    Private Function GetDBConnection() As String
        'Return "mongodb://localhost/local"
        Dim connString As String = ""
        Try
            connString = System.Configuration.ConfigurationManager.ConnectionStrings("VitalSignsMongo").ToString()
        Catch ex As Exception
            WriteDeviceHistoryEntry("All", "Alerts", Now, "Error getting connection information: " & ex.Message)
        End Try
        Return connString
    End Function
End Class

Public Class DateTimeExtensions
    Dim _gc As New GregorianCalendar
    Public Function GetWeekOfMonth(ByVal time As DateTime) As Integer
        Dim first As DateTime = New DateTime(time.Year, time.Month, 1)
        Return GetWeekOfYear(time) - GetWeekOfYear(first) + 1
    End Function

    Public Function GetWeekOfYear(ByVal time As DateTime) As Integer
        Return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday)
    End Function

End Class