Imports System.Data
Imports System.Data.SqlClient
Imports VSFramework
Imports System.IO
Imports System.Globalization
Imports LogUtilities


Public Class MaintenanceDll

    ''' <summary>
    ''' This function returns true if in maintenance, false if not. Current date time is checked against settings given.
    ''' </summary>
    ''' <param name="DeviceType">Type of Server</param>
    ''' <param name="DeviceName">Name of Server</param>
    ''' <returns>True/ False</returns>
    ''' <remarks></remarks>
    ''' New Version 17Sep13:Mukund D
    Public Function InMaintenance(ByVal DeviceType As String, ByVal DeviceName As String) As Boolean
        Dim objVSAdaptor As New VSAdaptor

        'WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName & "is in maintenance function.")


        Dim dsMaintWindows As New Data.DataSet
        Dim InMaintenanceWindow As Boolean = False
        Dim strSQL As String = ""

        '9/18/2013 NS modified - added a UNION clause to include URLs
        '12/7/2015 NS modified for VSPLUS-2227
		strSQL = "select m1.ID,ServerName, ServerType ,Name,StartDate,StartTime,Duration,EndDate,MaintType,MaintDaysList " &
		  "from Maintenance m1 " &
		  " inner Join " &
		  " ServerMaintenance m2 ON m1.ID=m2.MaintID " &
		 "  INNER Join " &
		  " Servers s1 ON s1.ID=m2.ServerID and s1.ServerTypeID =m2.ServerTypeID " &
		   "  INNER Join " &
		  " ServerTypes s2 ON s2.ID=s1.ServerTypeID " &
		  " where ServerName= '" & DeviceName & "' and ServerType= '" & DeviceType & "' and GETDATE() between StartDate and EndDate +' 11:59:59 PM' " &
		  " union " &
		  " select m1.ID,TheURL, ServerType ,m1.Name,StartDate,StartTime,Duration,EndDate,MaintType,MaintDaysList " &
		  " from Maintenance m1 " &
		  " inner Join " &
		  " ServerMaintenance m2 ON m1.ID=m2.MaintID " &
		  " INNER Join " &
		  " URLs s1 ON s1.ID=m2.ServerID " &
		  " INNER Join " &
		  " ServerTypes s2 ON s2.ID=m2.ServerTypeID " &
		  " where s1.Name= '" & DeviceName & "' and ServerType= '" & DeviceType & "' and m2.ServerTypeID=s2.ID and " &
		  " GETDATE() between StartDate and EndDate +' 11:59:59 PM' " &
		  " union " &
		  " select m1.ID,ServerName, ServerType ,Name,StartDate,StartTime,Duration,EndDate,MaintType,MaintDaysList " &
		  " from Maintenance m1 " &
		  " inner Join " &
		  " ServerMaintenance m2 ON m1.ID=m2.MaintID " &
		  " INNER Join " &
		  " Traveler_Devices s1 ON s1.UserName + '-' + s1.DeviceID=m2.DeviceID " &
		  " INNER Join " &
		  " ServerTypes s2 ON s2.ID=m2.ServerTypeID " &
		  " where s1.UserName + '-' + s1.DeviceID= '" & DeviceName & "' and ServerType= '" & DeviceType & "' and GETDATE() between StartDate and EndDate +' 11:59:59 PM' "


        ' WriteDeviceHistoryEntry("Maintenance", DeviceName, Now.ToString & " sql " & strSQL)

        Try
            dsMaintWindows.Tables.Add("MaintWindows")

            'Dim connectionString As String = "Data Source=174.46.239.207,443; User ID=sa;Password=vsadmin123!;Persist Security Info=True;Initial Catalog=vitalsigns;"
            'Dim Sqlcon As New SqlConnection(connectionString)
            'Dim DA As New SqlDataAdapter(strSQL, Sqlcon)
            'Dim Ds As New DataSet
            'DA.Fill(dsMaintWindows, "MaintWindows")

            objVSAdaptor.FillDatasetAny("vitalsigns", "None", strSQL, dsMaintWindows, "MaintWindows")

            'WriteDeviceHistoryEntry("Domino", DeviceName, dsMaintWindows.Tables("MaintWindows").Rows.Count)

        Catch ex As Exception
            'WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Error in maint window module dataset creation: " & ex.Message)
            WriteHistoryEntry(Now.ToString & " Server " & DeviceName & " Error in maint window module dataset creation: " & ex.Message)
            'WriteAuditEntry(Now.ToString & " Error in maint window module dataset creation: " & ex.Message)
            Return False
            Exit Function
        End Try

        Dim b2 As New DateTimeExtensions
        Dim daynum As Integer
        daynum = Weekday(Now, FirstDayOfWeek.Monday) 'CType(DayofWeek, Integer) CHECK THIS -1
        Dim MaintType As String
        Dim MaintDayList As String
        Dim StartDate, StartTime As DateTime
        Dim MyStartTime, MyEndTime As DateTime
        Dim TimeNow As DateTime = Now
        Dim Wknum, Duration, StartWknum, TodayWknum As Integer
        Wknum = b2.GetWeekOfMonth(Convert.ToDateTime(Now)) 'Format(Now.Date, "w")

        Try
            Dim dr As DataRow
            'WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName & " has " & dsMaintWindows.Tables("MaintWindows").Rows.Count & " maintenance windows.")


            For Each dr In dsMaintWindows.Tables("MaintWindows").Rows

                Try
                    MyStartTime = CType(dr.Item("StartTime"), DateTime)
                    'MyEndTime = CType(dr.Item("EndTime"), DateTime)
                    MaintType = dr.Item("MaintType").ToString()
                    MaintDayList = dr.Item("MaintDaysList").ToString()

                    StartDate = CType(dr.Item("StartDate"), DateTime)
                    StartTime = CType(dr.Item("StartTime"), DateTime)
                    Duration = CType(dr.Item("Duration"), Integer)

                    'for case 3,to get the week nos from the start date
                    StartWknum = DatePart(DateInterval.WeekOfYear, StartDate)
                    TodayWknum = DatePart(DateInterval.WeekOfYear, Now)

                    Select Case MaintType
                        Case "1"
                            MyStartTime = StartDate + StartTime.TimeOfDay
                            MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
                            If Now >= MyStartTime And Now < MyEndTime Then
                                InMaintenanceWindow = True
                                Exit For
                            End If
                        Case "2"
                            'Dim MType As Array = MaintDayList.Split(",")
                            'For Each i In MType
                            'If daynum = i Then 'MType(i) 
                            MyStartTime = Now.Date + StartTime.TimeOfDay
                            MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
                            If Now >= MyStartTime And Now < MyEndTime Then
                                InMaintenanceWindow = True
                                Exit For
                            End If
                            'End If
                            'Next


                        Case "3"
                            Dim wkType As Array = MaintDayList.Split(",")
                            For Each i In wkType
                                Dim WKday As Array = i.Split(":")
                                If (TodayWknum - StartWknum) Mod WKday(1) = 0 Then
                                    'If Wknum = WKday(1) Then
                                    If daynum = WKday(0) Then
                                        MyStartTime = Now.Date + StartTime.TimeOfDay
                                        MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
                                        If Now >= MyStartTime And Now < MyEndTime Then
                                            InMaintenanceWindow = True
                                            Exit For
                                        End If
                                    End If
                                End If
                            Next

                        Case "4"
                            Dim x, y As Integer
                            x = MaintDayList.IndexOf(",")
                            y = MaintDayList.IndexOf(":")
                            If x < 0 And y < 0 Then
                                If MaintDayList = Format(TimeNow, "dd") Then
                                    InMaintenanceWindow = True
                                    Exit For
                                End If
                            Else
                                Dim MonthType As Array = MaintDayList.Split(",")
                                For Each i In MonthType
                                    Dim mnday As Array = i.Split(":")
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

                                        If wkname = mnday(1) Then
                                            MyStartTime = Now.Date + StartTime.TimeOfDay
                                            MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
                                            If Now >= MyStartTime And Now < MyEndTime Then
                                                InMaintenanceWindow = True
                                                Exit For
                                            End If
                                        End If
                                    End If

                                Next

                            End If


                    End Select



                Catch ex As Exception
                    'WriteDeviceHistoryEntry("Domino", DeviceName, " Error calculating maintenance window start and end times. Error: " & ex.Message)
                    WriteHistoryEntry(Now.ToString & " Server " & DeviceName & " Error calculating maintenance window start and end times. Error: " & ex.Message)
                    'WriteAuditEntry(" Error calculating maintenance window start and end times. Error: " & ex.Message)
                End Try
            Next

            dr = Nothing
        Catch ex As Exception
            'WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Error in maint window module: " & ex.Message)
            WriteHistoryEntry(Now.ToString & " Server " & DeviceName & " Error in maint window module: " & ex.Message)
            'WriteAuditEntry(Now.ToString & " Error in maint window module: " & ex.Message)
        End Try

        Try
            dsMaintWindows.Dispose()
            'myPath = Nothing
            GC.Collect()
        Catch ex As Exception
            WriteHistoryEntry(Now.ToString & " Server " & DeviceName & " Error in maint window module: " & ex.Message)

        End Try

        'WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " InMaintenanceWindow:" & InMaintenanceWindow.ToString())
        WriteHistoryEntry(Now.ToString & " Server " & DeviceName & " has " & dsMaintWindows.Tables("MaintWindows").Rows.Count & " maintenance windows. and is now in Maintenance = " & InMaintenanceWindow)
        Return InMaintenanceWindow
    End Function

	Private Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String, Optional ByVal LogLevelInput As LogUtils.LogLevel = LogUtils.LogLevel.Normal)
		LogUtils.WriteDeviceHistoryEntry(DeviceType, DeviceName, strMsg, LogLevelInput)
	End Sub


	'Private Sub WriteDeviceHistoryEntry(ByVal DeviceType As String, ByVal DeviceName As String, ByVal strMsg As String)
	'    Dim DeviceLogDestination As String = ""
	'    Dim appendMode As Boolean = True
	'    '   If Left(strAppPath, 1) = "\" Then
	'    'DeviceLogDestination = strAppPath & "Data\Logfiles\" & DeviceType & "_" & DeviceName & "_Log.txt"
	'    '  Else
	'    '    DeviceLogDestination = strAppPath & "\Data\Logfiles\" & DeviceType & "_" & DeviceName & "_Log.txt"
	'    '    End If
	'    Try
	'        DeviceLogDestination = AppDomain.CurrentDomain.BaseDirectory.ToString & "\Log_Files\" & DeviceType & "_" & DeviceName & "_Log.txt"
	'        If InStr(DeviceLogDestination, "/") > 0 Then
	'            DeviceLogDestination = DeviceLogDestination.Replace("/", "_")
	'        End If
	'    Catch ex As Exception

	'    End Try

	'    Try
	'        Dim sw As New StreamWriter(DeviceLogDestination, appendMode, System.Text.Encoding.Unicode)
	'        sw.WriteLine(strMsg)
	'        sw.Close()
	'        sw = Nothing
	'    Catch ex As Exception

	'    End Try
	'    GC.Collect()
	'End Sub


	'VSPLUS-1298 Durga
	Public Function OffHours() As Boolean
		Dim strSQL As String
		Dim dt As New DataTable
		Dim ds As New DataSet
		Dim vsobj As New VSAdaptor
		Dim isoffhours As Boolean

		Try
			strSQL = "select Starttime,Duration from HoursIndicator where ID=0"

			dt.TableName = "HoursIndicator"
			ds.Tables.Add(dt)

			vsobj.FillDatasetAny("VitalSigns", "vitalsigns", strSQL, ds, "HoursIndicator")
			If dt.Rows.Count = 0 Then
				Return False
			End If
			If dt.Rows.Count > 0 Then

				Dim currenttime As DateTime = DateTime.Now
				'Dim starttime As String = dt.Rows(0)(1).ToString()
				Dim starttime As DateTime = Convert.ToDateTime(dt.Rows(0)(0).ToString())
				Dim duration As Int32 = Convert.ToInt32(dt.Rows(0)(1).ToString())
				Dim endtime As DateTime = starttime.AddMinutes(duration)
				If currenttime > starttime And currenttime < endtime Then
					isoffhours = False
				Else
					isoffhours = True
				End If
			End If

		Catch ex As Exception
			WriteHistoryEntry(Now.ToString & " Error in Business Hours module: " & ex.Message)
		End Try
		Return isoffhours
	End Function
	'VSPLUS-1298 Durga
	'Returns true if in off-hours, false otherwise
	Public Function OffHours(ByVal servername As String) As Boolean
		Dim strSQL As String
		Dim dt As New DataTable
		Dim ds As New DataSet
		Dim vsobj As New VSAdaptor
		Dim isoffhours As Boolean

		Try
			'strSQL = "select hr.Starttime,hr.Duration, hr.is" & DateTime.Now.ToString("dddd") & " DayOfWeek from [servers] sr inner join HoursIndicator hr on sr.BusinesshoursID=hr.ID where sr.ServerName='" & servername & "'"
			'6/8/15 WS modified for VSPLUS-1816
			strSQL = "exec InBusinessHoursByServer '" & servername & "'"
			dt.TableName = "HoursIndicator"
			ds.Tables.Add(dt)

			vsobj.FillDatasetAny("VitalSigns", "vitalsigns", strSQL, ds, "HoursIndicator")
			If dt.Rows.Count = 1 Then
				Dim isHours As String = dt(0)("InBusinessHours").ToString()

				If (isHours = "1") Then
					Return False
				Else
					Return True
				End If

			End If

			Return False

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


	'Private Sub WriteHistoryEntry(ByVal strMsg As String)
	'    Dim DeviceLogDestination As String = ""
	'    Dim appendMode As Boolean = True

	'    Try
	'        DeviceLogDestination = AppDomain.CurrentDomain.BaseDirectory.ToString & "\Log_Files\All_Maintenance_Log.txt"
	'        If InStr(DeviceLogDestination, "/") > 0 Then
	'            DeviceLogDestination = DeviceLogDestination.Replace("/", "_")
	'        End If
	'    Catch ex As Exception

	'    End Try

	'    Try
	'        Dim sw As New IO.StreamWriter(DeviceLogDestination, appendMode, System.Text.Encoding.Unicode)
	'        sw.WriteLine(strMsg)
	'        sw.Close()
	'        sw = Nothing
	'    Catch ex As Exception

	'    End Try
	'    GC.Collect()
	'End Sub


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