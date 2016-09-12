Imports System.ServiceProcess
Imports System.IO
Imports System.Text
Imports System.Threading
Imports Microsoft.Win32.Registry
Imports Microsoft.Win32
Imports System.Security.Cryptography
Imports System.Xml
Imports System.Xml.Linq
Imports VSFramework
Imports Ionic.Zip
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Runtime.Serialization.Json
Imports System.Collections.Generic
Imports System.Linq
'11/19/2015 NS added for VSPLUS-2383
Imports System.Globalization
Imports RPRWyatt.VitalSigns.Services
Imports System.Threading.Tasks


Public Class VitalSignsDailyTasks
    Inherits VSServices
    Dim objVSAdaptor As New VSAdaptor
    Dim One_Minute As Long = 1000 * 60
    Dim TimeToStop As Boolean = False

    Dim BannerText As String
    Dim ProductName As String
    Dim CompanyName As String = "JNIT Inc. dba RPR Wyatt"
    Dim EvalVersion As Boolean = False
    Dim boolHTML As Boolean = True
    Dim strLogDest, strAppPath, strAuditText, HTMLPath, StrStatisticsPath, StrSTStatisticsPath, strServersMDBPath As String
    Dim BuildNumber As Integer = 1070
    Dim sCultureString As String = "en-US"
    Dim caltureStringName As String = "CultureString"
    Dim objDateUtils As New DateUtils.DateUtils
    Dim strDateFormat As String
    Dim DominoDiskNames(100) As String
    Dim MicrosoftDiskNames(100) As String

    'Determines the verbosity of the log file
    Enum LogLevel
        Verbose = LogUtilities.LogUtils.LogLevel.Verbose
        Debug = LogUtilities.LogUtils.LogLevel.Debug
        Normal = LogUtilities.LogUtils.LogLevel.Normal
    End Enum

    'MyLogLevel is used throughout to control the volume of the log file output
    Dim MyLogLevel As LogLevel


#Region " Component Designer generated code "

    Public Sub New()
        MyBase.New()

        ' This call is required by the Component Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call

    End Sub

    'UserService overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    ' The main entry point for the process
    <MTAThread()> _
    Shared Sub Main()
        Dim ServicesToRun() As System.ServiceProcess.ServiceBase

        ' More than one NT Service may run within the same process. To add
        ' another service to this process, change the following line to
        ' create a second service object. For example,
        '
        '   ServicesToRun = New System.ServiceProcess.ServiceBase () {New Service1, New MySecondUserService}
        '
        ServicesToRun = New System.ServiceProcess.ServiceBase() {New VitalSignsDailyTasks}

        System.ServiceProcess.ServiceBase.Run(ServicesToRun)
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    ' NOTE: The following procedure is required by the Component Designer
    ' It can be modified using the Component Designer.  
    ' Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        '
        'Service1
        '
        Me.ServiceName = "Service1"

    End Sub

#End Region

    Protected Overrides Sub ServiceOnStart(ByVal args() As String)

        Try
            sCultureString = ConfigurationManager.AppSettings(caltureStringName).ToString()
        Catch ex As Exception
            sCultureString = "en-US"
        End Try

        Dim myRegistry As New RegistryHandler
        Try
            MyLogLevel = myRegistry.ReadFromRegistry("Log Level")
        Catch ex As Exception
            MyLogLevel = LogLevel.Verbose
        End Try

        ' MyLogLevel = LogLevel.Verbose

        Try
            strAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
        Catch ex As Exception
            strAppPath = "c:\"
        End Try

        Try
            'WriteAuditEntry(Now.ToString & " Querying SQL server for the current date format.", LogLevel.Normal)
            strDateFormat = objDateUtils.GetDateFormat()
            'WriteAuditEntry(Now.ToString & " The current date format is " & strDateFormat, LogLevel.Normal)
        Catch ex As Exception

        End Try

        Try
            strLogDest = strAppPath & "\Log_Files\Daily_Tasks_Log.txt"
        Catch ex As Exception

        End Try

        Try
            '   strLogDest = myRegistry.ReadFromRegistry("History Path")
            If File.Exists(strLogDest) Then
                File.Move(strLogDest, strAppPath & "\Log_Files\Daily_Tasks_Log_Bak.txt")
                File.Delete(strLogDest)
            End If
        Catch ex As Exception
            ' strLogDest = "c:\vitalsignslog.txt"
        End Try



        Try
            myRegistry.WriteToRegistry("Daily Tasks Start", Now.ToShortDateString & " " & Now.ToShortTimeString)
            myRegistry.WriteToRegistry("Daily Tasks Build", BuildNumber)
        Catch ex As Exception

        End Try

        Try
            ProductName = myRegistry.ReadFromRegistry("ProductName")
            If ProductName = "" Then
                ProductName = "VitalSigns"
            End If
        Catch ex As Exception
            ProductName = "VitalSigns"
        End Try

        Try
            '    WriteAuditEntriesThread.Start()
        Catch ex As Exception

        End Try


        WriteAuditEntry(Now.ToString & " VitalSigns Daily Tasks service is starting up.")
        WriteAuditEntry(Now.ToString & " VitalSigns Daily Tasks Build Number: " & BuildNumber)
        WriteAuditEntry(Now.ToString & " Copyright " & CompanyName & "  " & Date.Now.Year & " - All rights reserved." & vbCrLf & vbCrLf)


        Try
            DailyBackup()
        Catch ex As Exception

        End Try


        'Only continue if it is the primary node
        Dim isPrimaryNode As Boolean = True
        Dim sql As String



        Try
            If Not (System.Configuration.ConfigurationManager.AppSettings("VSNodeName") Is Nothing) Then

                Dim myConnectionString As New VSFramework.XMLOperation

                Dim NodeName As String = System.Configuration.ConfigurationManager.AppSettings("VSNodeName").ToString()
                sql = "SELECT IsPrimaryNode From Nodes WHERE Name='" & NodeName & "'"

                Dim dt As DataTable = objVSAdaptor.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sql)
                If (dt.Rows.Count > 0) Then
                    isPrimaryNode = Convert.ToBoolean(dt.Rows(0)(0).ToString())
                End If

            End If

        Catch ex As Exception
            WriteAuditEntry("Exception checking if primary node.  Error: " & ex.Message)
        End Try


        If Not isPrimaryNode Then
            WriteAuditEntry("Daily Task is finished since it is not the Primary Node....")
            Me.Stop()
            Return
        End If

        WriteAuditEntry("This is marked as Primary Node and will continue with database changes")

        Try
            WriteAuditEntry("Building a list of all unique Domino disk drives, if any. ")
            BuildDominoDriveList()
        Catch ex As Exception
            WriteAuditEntry("Exception building Domino drives list ...." & ex.ToString)
        End Try


        Try
            WriteAuditEntry("Building a list of all unique Microsoft server disk drives, if any. ")
            BuildMicrosoftDriveList()
        Catch ex As Exception
            WriteAuditEntry("Exception building Microsoft server disk drives list ...." & ex.ToString)
        End Try


        Try
            ConsolidateStatistics()

        Catch ex As Exception
            WriteAuditEntry("AAAGH...." & ex.ToString)
        End Try

        WriteAuditEntry("Consolidation is finished....")


        Try
            CleanUpObsoleteData()
        Catch ex As Exception
            WriteAuditEntry("OOPS, error cleaning up old data...." & ex.ToString)
        End Try
        Try
            'Durga VSPLUS 1874 6/26/2015
            Shrinkdb("VitalSigns")
            Shrinkdb("VSS_Statistics")
        Catch ex As Exception
            WriteAuditEntry("OOPS, error  to Shrink Databases...." & ex.ToString)
        End Try
        '6/25/2015 NS added for VSPLUS-1226
        Try
            Dim myConnectionString As New VSFramework.XMLOperation
            Dim cleanupNow As Boolean = False
            sql = "SELECT CASE WHEN DATEADD(Day,7,CONVERT(DateTime, ISNULL(svalue,DATEADD(Day,-7,GETDATE())), 120)) < GETDATE() " &
                "THEN 'true' ELSE 'false' END AS CleanupNow FROM Settings WHERE sname='CleanUpTablesDate'"

            Dim dt As DataTable = objVSAdaptor.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), sql)
            If (dt.Rows.Count > 0) Then
                cleanupNow = Convert.ToBoolean(dt.Rows(0)(0).ToString())
            End If
            If cleanupNow Then
                WriteAuditEntry(Now.ToString & " Starting weekly cleanup.")
                CleanupAnyTableWeekly()
                'Kiran Dadireddy VSPLUS-2684
                ShrinkDBLogOnWeeklyBasis()
                sql = "UPDATE Settings SET svalue=GETDATE() WHERE sname='CleanUpTablesDate'"
                objVSAdaptor.ExecuteNonQueryAny("vitalsigns", "vitalsigns", sql)
                WriteAuditEntry(Now.ToString & " Updated the Settings table CleanUpTablesDate column.")
            End If
        Catch ex As Exception
            WriteAuditEntry("Error cleaning up weekly data...." & ex.ToString)
        End Try

        Try
            WriteAuditEntry("Starting the Log Statistics")
            LogTableStatistics("Vitalsigns")
            LogTableStatistics("VSS_Statistics")

        Catch ex As Exception
            WriteAuditEntry("OOPS, error printing the statistics database" & ex.ToString)
        End Try

        Try
            WriteAuditEntry("Starting update of local tables")
            UpdateLocalTables()
        Catch ex As Exception
            WriteAuditEntry("OOPS, error updating local tables" & ex.ToString)
        End Try

        WriteAuditEntry("Daily Task is finished....")
        Me.Stop()
    End Sub

    Public Sub ConsolidateStatistics()


        Dim GoBackDays As Integer = 3
        'go back up to GoBackDays variable number of days looking for data that hasn't been summarized yet.
        Dim n As Integer
        For n = GoBackDays To 1 Step -1
            WriteAuditEntry(vbCrLf & vbCrLf & "*************************************  ---> Processing " & Today.AddDays(-n).ToString)
            '11/20/2015 NS modified for VSPLUS-2383
            ProcessSpecificDate(Today.AddDays(-n), "DATEADD(dd,-" & n.ToString() & ",GETDATE())")
        Next

        Try
            'VSPLUS-607
            CleanUpTravelerSummaryData()
        Catch ex As Exception

        End Try


        Try
            'VSPLUS-607
            WriteAuditEntry(Now.ToString & " Consolidating Traveler Stats")
            ProcessStoredProcedures("OpenTimesDelta")
            ProcessStoredProcedures("CumulativeTimesMin")
            ProcessStoredProcedures("CumulativeTimesMax")
        Catch ex As Exception
            WriteAuditEntry("  Error calling stored procedure: " & ex.ToString)
        End Try


        WriteAuditEntry(vbCrLf & vbCrLf & "***********************************************" & vbCrLf & "Finished!")


    End Sub
    '11/20/2015 NS modified for VSPLUS-2383
    Public Sub ProcessSpecificDate(ByVal SearchDate As Date, Optional ByVal SearchDateSQL As String = "")

        WriteAuditEntry(Now.ToString & " VitalSigns Daily Tasks service is consolidating statistics for " & SearchDate, LogLevel.Normal)

        Dim objVSAdaptor As New VSAdaptor
        'Update Summary Data of various Stats 
        Dim strSQL As String = ""
        Dim myDataSet As New Data.DataSet
        Dim myTable As New Data.DataTable
        myTable.TableName = "DailyTasks"
        myDataSet.Tables.Add(myTable)


        Dim AlreadyProcessed As String = ""
        ' SearchDate = FixDate(Today.AddDays(-n))

        Try
            strSQL = "Select Result FROM ConsolidationResults WHERE CONVERT (DATE, ScanDate) = '" & FixDate(SearchDate) & "' "
            WriteAuditEntry(Now.ToString & " --> " & strSQL)
            AlreadyProcessed = objVSAdaptor.ExecuteScalarAny("VSS_Statistics", "Stats", strSQL)
        Catch ex As Exception
            AlreadyProcessed = "False"
        End Try


        Try
            If AlreadyProcessed = "Success" Then
                WriteAuditEntry(Now.ToString & " " & FixDate(SearchDate) & " has already been processed", LogLevel.Normal)
                Exit Sub
            Else
                WriteAuditEntry(Now.ToString & " " & FixDate(SearchDate) & " has NOT already been processed", LogLevel.Normal)
                objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "Stats", "Insert INTO ConsolidationResults (ScanDate, Result) VALUES ('" & FixDate(SearchDate) & "', 'Success')")
            End If
        Catch ex As Exception

        End Try

        Try
            ConsolidateDominoDiskStats(SearchDate)
        Catch ex As Exception

        End Try

        Try
            ConsolidateServerDiskStats(SearchDate)
        Catch ex As Exception

        End Try

        strSQL = "SELECT SourceTableName, SourceAggregation, SourceStatName, DestinationTableName, DestinationStatName, QueryType FROM DailyTasks"
        strSQL = strSQL & " Order By SourceStatName  DESC"

        WriteAuditEntry(vbCrLf & strSQL & vbCrLf, LogLevel.Verbose)


        Try
            objVSAdaptor.FillDatasetAny("VitalSigns", "vitalsigns", strSQL, myDataSet, "DailyTasks")
        Catch ex As Exception
            WriteAuditEntry("Exception = " & ex.ToString())
            WriteAuditEntry(Now.ToString & " Error Accessing the DailyTasks Table " & ex.Message & strSQL)
        End Try

        Try
            WriteAuditEntry("I found " & myDataSet.Tables("DailyTasks").Rows.Count & " statistics to process.", LogLevel.Verbose)
        Catch ex As Exception

        End Try


        Dim myView As New Data.DataView(myDataSet.Tables("DailyTasks"))
        Dim drv As DataRowView

        Dim strSQLSelect As String = ""
        Dim srcTable, operation, srcStat, destTable, destStat, QueryType As String
        ' Dim value As Single = 0
        Dim rowCounter As Integer = 1

        For Each drv In myView

            Try

                Dim SummaryData As New Data.DataTable

                myDataSet.Tables.Add(SummaryData)
                srcTable = drv("SourceTableName")
                operation = drv("SourceAggregation")
                srcStat = drv("SourceStatName")
                destTable = drv("DestinationTableName")
                destStat = drv("DestinationStatName")
                QueryType = drv("QueryType")
                Try
                    WriteAuditEntry(vbCrLf & Now.ToString & " Processing stat #" & rowCounter & ": " & srcStat, LogLevel.Verbose)
                    rowCounter += 1
                Catch ex As Exception
                End Try

                'Followed the SQL way of providing the date range
                If (srcTable <> "" And operation <> "" And srcStat <> "" And destTable <> "" And destStat <> "" And QueryType = "1") Then
                    RunQueryType1(SearchDate, srcStat, srcTable, operation, destTable, destStat, QueryType)
                ElseIf (srcTable <> "" And operation <> "" And srcStat <> "" And destTable <> "" And destStat <> "" And QueryType = "2") Then
                    RunQueryType2(SearchDate, srcStat, srcTable, operation, destTable, destStat, QueryType)
                ElseIf (srcTable <> "" And operation <> "" And srcStat <> "" And destTable <> "" And destStat <> "" And QueryType = "3") Then
                    RunQueryType3(SearchDate, srcStat, srcTable, operation, destTable, destStat, QueryType)
                End If
            Catch ex As Exception
                WriteAuditEntry("Advancing to next row...", LogLevel.Verbose)

            End Try

        Next

        Try
            '11/20/2015 NS modified for VSPLUS-2383
            ConsolidateExchangeDatabases(SearchDate, SearchDateSQL)
        Catch ex As Exception

        End Try

        Try
            '12/23/2015 WS modified for VSPLUS-1423
            ConsolidateExchangeMailboxData(SearchDate, SearchDateSQL)
        Catch ex As Exception

        End Try

        WriteAuditEntry(vbCrLf & vbCrLf & "***********************************************" & vbCrLf & "Finished!")


    End Sub

    Public Sub RunQueryType1(ByVal SearchDate As Date, ByVal srcStat As String, ByVal srcTable As String, ByVal operation As String, ByVal destTable As String, ByVal destStat As String, ByVal QueryType As String)

        Dim drv As DataRowView
        Dim myDataSet As New Data.DataSet
        Dim myTable As New Data.DataTable
        myTable.TableName = "DailyTasks"
        myDataSet.Tables.Add(myTable)

        Dim strSQLSelect As String = ""
        'Dim srcTable, operation, srcStat, destTable, destStat, QueryType As String
        ' Dim value As Single = 0
        Dim rowCounter As Integer = 1
        '11/19/2015 NS added for VSPLUS-2383
        Dim sqlcmd As New SqlCommand
        Dim USprovider As IFormatProvider = CultureInfo.CreateSpecificCulture(sCultureString)
        Dim Europrovider As IFormatProvider = CultureInfo.CreateSpecificCulture("fr-FR")
        WriteAuditEntry(Now.ToString & " USprovider sCultureString is " & sCultureString, LogLevel.Verbose)
        Try

            'Followed the SQL way of providing the date range
            '  Console.WriteLine(" Searching for unprocessed statistics for " & SearchDate.ToShortDateString)
            If srcTable = "MicrosoftDailyStats" Then
                strSQLSelect = "SELECT ServerName, ServerTypeId, WeekNumber, MonthNumber, YearNumber, DayNumber," & operation & "(StatValue) AS value FROM " & vbCrLf
                strSQLSelect = strSQLSelect & "" & srcTable & " WHERE StatName = '" & srcStat & "' AND date >= DATEADD(day,DATEDIFF(day,0," & objVSAdaptor.DateFormat(FixDate(SearchDate)) & "),0)" & vbCrLf
                strSQLSelect = strSQLSelect & "AND date < DATEADD(day,DATEDIFF(day,0," & objVSAdaptor.DateFormat(FixDate(SearchDate.AddDays(1))) & "),0) GROUP BY ServerName, ServerTypeId, WeekNumber, MonthNumber, YearNumber, DayNumber" & vbCrLf
            Else

                strSQLSelect = "SELECT ServerName, WeekNumber, MonthNumber, YearNumber, DayNumber," & operation & "(StatValue) AS value FROM " & vbCrLf
                strSQLSelect = strSQLSelect & "" & srcTable & " WHERE StatName = '" & srcStat & "' AND date >= DATEADD(day,DATEDIFF(day,0," & objVSAdaptor.DateFormat(FixDate(SearchDate)) & "),0)" & vbCrLf
                strSQLSelect = strSQLSelect & "AND date < DATEADD(day,DATEDIFF(day,0," & objVSAdaptor.DateFormat(FixDate(SearchDate.AddDays(1))) & "),0) GROUP BY ServerName, WeekNumber, MonthNumber, YearNumber, DayNumber" & vbCrLf
                'strSQLSelect += "ORDER BY YearNumber DESC, MonthNumber DESC, DayNumber DESC"

            End If
            WriteAuditEntry(strSQLSelect, LogLevel.Verbose)

            Try
                objVSAdaptor.FillDatasetAny("VSS_Statistics", "statistics", strSQLSelect, myDataSet, "SummaryData")
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error Accessing the " & srcTable & " Table. " & ex.Message & strSQLSelect)
            End Try

            Dim SummaryView As New Data.DataView(myDataSet.Tables("SummaryData"))
            Dim summaryDrv As DataRowView

            Dim strSQLInsert As String = ""
            Dim serverName As String = ""
            Dim deviceName As String = ""
            Dim weekNumber, dayNumber, monthNumber, yearNumber As Integer
            Dim summaryValue As Single = 0
            Dim serverTypeId As String = ""
            Dim queryToLog As String = String.Empty

            Try
                If SummaryView.Count > 0 Then
                    For Each summaryDrv In SummaryView
                        Try
                            serverName = summaryDrv("ServerName")
                            weekNumber = summaryDrv("WeekNumber")
                            dayNumber = summaryDrv("DayNumber")
                            monthNumber = summaryDrv("MonthNumber")
                            yearNumber = summaryDrv("YearNumber")
                            summaryValue = summaryDrv("value")
                            'summaryDate = summaryDrv("")
                            'deviceName = summaryDrv("DeviceName")
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error getting the values " & ex.Message)
                        End Try

                        If (QueryType = "1") Then
                            '11/19/2015 NS added for VSPLUS-2383
                            'Try
                            '    strSQLInsert = "INSERT INTO " & destTable & vbCrLf
                            '    If srcTable = "MicrosoftDailyStats" Then
                            '        strSQLInsert = strSQLInsert & " VALUES ('" & serverName & "', " & summaryDrv("ServerTypeId") & ", " & objVSAdaptor.DateFormat(FixDate(SearchDate)) & ", '" & destStat & "', '" & summaryValue & "', '" & weekNumber & "', '" & monthNumber & "', '" & yearNumber & "', '" & dayNumber & "')"
                            '    Else
                            '        strSQLInsert = strSQLInsert & " VALUES ('" & serverName & "', " & objVSAdaptor.DateFormat(FixDate(SearchDate)) & ", '" & destStat & "', '" & summaryValue & "', '" & weekNumber & "', '" & monthNumber & "', '" & yearNumber & "', '" & dayNumber & "')"
                            '    End If
                            '    WriteAuditEntry(Now.ToString & " SQL INSERT statement is " & strSQLInsert, LogLevel.Verbose)
                            'Catch ex As Exception
                            '    WriteAuditEntry(Now.ToString & " Exception creating INSERT statement: " & ex.ToString)
                            'End Try

                            strSQLInsert = "INSERT INTO " & destTable & vbCrLf

                            If srcTable = "MicrosoftDailyStats" Then
                                queryToLog = strSQLInsert & String.Format(" VALUES ('{0}','{1}','{2}','{3},'{4}','{5}','{6}','{7}','{8}')", serverName, summaryDrv("ServerTypeId"), SearchDate, destStat, summaryValue, weekNumber, monthNumber, yearNumber, dayNumber)
                                strSQLInsert = strSQLInsert & " VALUES (@DeviceName,@DeviceTypeID,@Date,@StatName,@StatValue,@WeekNum,@MonthNum,@YearNum,@DayNum)"
                                sqlcmd = New SqlCommand(strSQLInsert)
                                sqlcmd.Parameters.Add(New SqlParameter("@DeviceName", SqlDbType.NVarChar, 100))
                                sqlcmd.Parameters("@DeviceName").Value = serverName
                                sqlcmd.Parameters.Add(New SqlParameter("@DeviceTypeID", SqlDbType.Int))
                                sqlcmd.Parameters("@DeviceTypeID").Value = summaryDrv("ServerTypeId")
                                sqlcmd.Parameters.Add(New SqlParameter("@Date", SqlDbType.DateTime))
                                sqlcmd.Parameters("@Date").Value = SearchDate
                                sqlcmd.Parameters.Add(New SqlParameter("@StatName", SqlDbType.NVarChar, 50))
                                sqlcmd.Parameters("@StatName").Value = destStat
                                sqlcmd.Parameters.Add(New SqlParameter("@StatValue", SqlDbType.Float))
                                sqlcmd.Parameters("@StatValue").Value = summaryValue
                                sqlcmd.Parameters.Add(New SqlParameter("@WeekNum", SqlDbType.Int))
                                sqlcmd.Parameters("@WeekNum").Value = weekNumber
                                sqlcmd.Parameters.Add(New SqlParameter("@MonthNum", SqlDbType.Int))
                                sqlcmd.Parameters("@MonthNum").Value = monthNumber
                                sqlcmd.Parameters.Add(New SqlParameter("@YearNum", SqlDbType.Int))
                                sqlcmd.Parameters("@YearNum").Value = yearNumber
                                sqlcmd.Parameters.Add(New SqlParameter("@DayNum", SqlDbType.Int))
                                sqlcmd.Parameters("@DayNum").Value = dayNumber
                            Else
                                queryToLog = strSQLInsert & String.Format(" VALUES ('{0}','{1}','{2}','{3},'{4}','{5}','{6}','{7}')", serverName, SearchDate, destStat, summaryValue, weekNumber, monthNumber, yearNumber, dayNumber)
                                strSQLInsert = strSQLInsert & " VALUES (@DeviceName,@Date,@StatName,@StatValue,@WeekNum,@MonthNum,@YearNum,@DayNum)"
                                sqlcmd = New SqlCommand(strSQLInsert)
                                sqlcmd.Parameters.Add(New SqlParameter("@DeviceName", SqlDbType.NVarChar, 100))
                                sqlcmd.Parameters("@DeviceName").Value = serverName
                                sqlcmd.Parameters.Add(New SqlParameter("@Date", SqlDbType.DateTime))
                                sqlcmd.Parameters("@Date").Value = SearchDate
                                sqlcmd.Parameters.Add(New SqlParameter("@StatName", SqlDbType.NVarChar, 50))
                                sqlcmd.Parameters("@StatName").Value = destStat
                                sqlcmd.Parameters.Add(New SqlParameter("@StatValue", SqlDbType.Float))
                                sqlcmd.Parameters("@StatValue").Value = summaryValue
                                sqlcmd.Parameters.Add(New SqlParameter("@WeekNum", SqlDbType.Int))
                                sqlcmd.Parameters("@WeekNum").Value = weekNumber
                                sqlcmd.Parameters.Add(New SqlParameter("@MonthNum", SqlDbType.Int))
                                sqlcmd.Parameters("@MonthNum").Value = monthNumber
                                sqlcmd.Parameters.Add(New SqlParameter("@YearNum", SqlDbType.Int))
                                sqlcmd.Parameters("@YearNum").Value = yearNumber
                                sqlcmd.Parameters.Add(New SqlParameter("@DayNum", SqlDbType.Int))
                                sqlcmd.Parameters("@DayNum").Value = dayNumber
                            End If
                            WriteAuditEntry(Now.ToString & " SQL command statement is " & queryToLog, LogLevel.Verbose)
                        End If
                        Try
                            '11/19/2015 NS modified for VSPLUS-2383
                            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQLInsert)
                            objVSAdaptor.ExecuteNonQuerySQLParams("VSS_Statistics", sqlcmd)
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Failed to insert Summary Data into " & destTable & " because: " & ex.Message)
                            WriteAuditEntry(Now.ToString & " The failed stats table insert command was " & strSQLInsert)
                        End Try
                    Next
                Else
                    WriteAuditEntry(Now.ToString & " This query did not produce any rows.", LogLevel.Verbose)
                End If
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Exception where I least expected it: " & ex.ToString)
            End Try

            Try
                '  myDataSet.Tables.Remove("SummaryData")
                myDataSet.Tables("SummaryData").Clear()
            Catch ex As Exception
                WriteAuditEntry("Could not Remove the table as no data in it.", LogLevel.Verbose)
            End Try

            WriteAuditEntry("Skipping this row as not all the required parameters were provided.", LogLevel.Verbose)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Exception creating SQL in DailyTasks: " & ex.ToString)
        End Try


    End Sub


    Public Sub RunQueryType2(ByVal SearchDate As Date, ByVal srcStat As String, ByVal srcTable As String, ByVal operation As String, ByVal destTable As String, ByVal destStat As String, ByVal QueryType As String)

        Dim drv As DataRowView
        Dim myDataSet As New Data.DataSet
        Dim myTable As New Data.DataTable
        myTable.TableName = "DailyTasks"
        myDataSet.Tables.Add(myTable)

        Dim strSQLSelect As String = ""
        'Dim srcTable, operation, srcStat, destTable, destStat, QueryType As String
        ' Dim value As Single = 0
        Dim rowCounter As Integer = 1
        '11/19/2015 NS added for VSPLUS-2383
        Dim sqlcmd As New SqlCommand
        Dim statvalueStr As String = ""
        Dim USprovider As IFormatProvider = CultureInfo.CreateSpecificCulture(sCultureString)
        Dim Europrovider As IFormatProvider = CultureInfo.CreateSpecificCulture("fr-FR")
        WriteAuditEntry(Now.ToString & " USprovider sCultureString is " & sCultureString, LogLevel.Verbose)
        Try
            '  Console.WriteLine(" Searching for unprocessed statistics for " & SearchDate.ToShortDateString)
            strSQLSelect = "SELECT DeviceType, DeviceName, WeekNumber, MonthNumber, YearNumber, DayNumber," & operation & "(StatValue) AS value FROM " & vbCrLf
            strSQLSelect = strSQLSelect & "" & srcTable & " WHERE StatName = '" & srcStat & "' AND date >= DATEADD(day,DATEDIFF(day,0," & objVSAdaptor.DateFormat(FixDate(SearchDate)) & "),0)" & vbCrLf
            strSQLSelect = strSQLSelect & "AND date < DATEADD(day,DATEDIFF(day,0," & objVSAdaptor.DateFormat(FixDate(SearchDate.AddDays(1))) & "),0) GROUP BY DeviceType, DeviceName, WeekNumber, MonthNumber, YearNumber, DayNumber" & vbCrLf
            'strSQLSelect += "ORDER BY YearNumber DESC, MonthNumber DESC, DayNumber DESC"
            WriteAuditEntry(strSQLSelect, LogLevel.Verbose)

            Try
                objVSAdaptor.FillDatasetAny("VSS_Statistics", "statistics", strSQLSelect, myDataSet, "SummaryData")
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error Accessing the " & srcTable & " Table. " & ex.Message & strSQLSelect)
            End Try

            Dim SummaryView As New Data.DataView(myDataSet.Tables("SummaryData"))
            Dim summaryDrv As DataRowView

            Dim strSQLInsert As String = ""
            Dim serverName As String = ""
            Dim deviceName As String = ""
            Dim deviceType As String = ""
            Dim weekNumber, dayNumber, monthNumber, yearNumber As Integer
            Dim summaryValue As Single = 0
            Dim queryToLog As String = String.Empty

            Try
                If SummaryView.Count > 0 Then
                    For Each summaryDrv In SummaryView
                        Try
                            'serverName = summaryDrv("DeviceName")
                            deviceType = summaryDrv("deviceType")
                            weekNumber = summaryDrv("WeekNumber")
                            dayNumber = summaryDrv("DayNumber")
                            monthNumber = summaryDrv("MonthNumber")
                            yearNumber = summaryDrv("YearNumber")
                            summaryValue = summaryDrv("value")
                            'summaryDate = summaryDrv("")
                            deviceName = summaryDrv("DeviceName")
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error getting the values " & ex.Message)
                        End Try

                        If (QueryType = "2") Then
                            Try
                                '11/19/2015 NS modified for VSPLUS-2383
                                strSQLInsert = "INSERT INTO " & destTable & vbCrLf
                                queryToLog = strSQLInsert & String.Format(" VALUES ('{0}','{1}','{2}','{3},'{4}','{5}','{6}','{7}',{8})", deviceType, deviceName, SearchDate, destStat, summaryValue, weekNumber, monthNumber, yearNumber, dayNumber)
                                strSQLInsert = strSQLInsert & " VALUES (@DeviceType,@DeviceName,@Date,@StatName,@StatValue,@WeekNum,@MonthNum,@YearNum,@DayNum)"
                                sqlcmd = New SqlCommand(strSQLInsert)
                                sqlcmd.Parameters.Add(New SqlParameter("@DeviceType", SqlDbType.NVarChar, 100))
                                sqlcmd.Parameters("@DeviceType").Value = deviceType
                                sqlcmd.Parameters.Add(New SqlParameter("@DeviceName", SqlDbType.NVarChar, 100))
                                sqlcmd.Parameters("@DeviceName").Value = deviceName
                                sqlcmd.Parameters.Add(New SqlParameter("@Date", SqlDbType.DateTime))
                                sqlcmd.Parameters("@Date").Value = SearchDate
                                sqlcmd.Parameters.Add(New SqlParameter("@StatName", SqlDbType.NVarChar, 50))
                                sqlcmd.Parameters("@StatName").Value = destStat
                                sqlcmd.Parameters.Add(New SqlParameter("@StatValue", SqlDbType.Float))
                                sqlcmd.Parameters("@StatValue").Value = summaryValue
                                sqlcmd.Parameters.Add(New SqlParameter("@WeekNum", SqlDbType.Int))
                                sqlcmd.Parameters("@WeekNum").Value = weekNumber
                                sqlcmd.Parameters.Add(New SqlParameter("@MonthNum", SqlDbType.Int))
                                sqlcmd.Parameters("@MonthNum").Value = monthNumber
                                sqlcmd.Parameters.Add(New SqlParameter("@YearNum", SqlDbType.Int))
                                sqlcmd.Parameters("@YearNum").Value = yearNumber
                                sqlcmd.Parameters.Add(New SqlParameter("@DayNum", SqlDbType.Int))
                                sqlcmd.Parameters("@DayNum").Value = dayNumber
                                'strSQLInsert = strSQLInsert & " VALUES ('" & deviceType & "','" & deviceName & "', " & objVSAdaptor.DateFormat(FixDate(SearchDate)) & ", '" & destStat & "', '" & summaryValue & "', '" & weekNumber & "', '" & monthNumber & "', '" & yearNumber & "', '" & dayNumber & "')"

                                WriteAuditEntry(Now.ToString & " SQL INSERT statement is " & queryToLog, LogLevel.Verbose)
                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " Exception creating INSERT statement: " & ex.ToString)
                            End Try
                        End If

                        Try
                            '11/19/2015 NS modified for VSPLUS-2383
                            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQLInsert)
                            objVSAdaptor.ExecuteNonQuerySQLParams("VSS_Statistics", sqlcmd)
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Failed to insert Summary Data into " & destTable & " because: " & ex.Message)
                            WriteAuditEntry(Now.ToString & " The failed stats table insert command was " & strSQLInsert)
                        End Try
                    Next
                Else
                    WriteAuditEntry(Now.ToString & " This query did not produce any rows.", LogLevel.Verbose)
                End If
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Exception where I least expected it: " & ex.ToString)
            End Try

            Try
                '  myDataSet.Tables.Remove("SummaryData")
                myDataSet.Tables("SummaryData").Clear()
            Catch ex As Exception
                WriteAuditEntry("Could not Remove the table as no data in it.", LogLevel.Verbose)
            End Try

            WriteAuditEntry("Skipping this row as not all the required parameters were provided.", LogLevel.Verbose)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Exception creating SQL in DailyTasks: " & ex.ToString)
        End Try


    End Sub

    Public Sub RunQueryType3(ByVal SearchDate As Date, ByVal srcStat As String, ByVal srcTable As String, ByVal operation As String, ByVal destTable As String, ByVal destStat As String, ByVal QueryType As String)

        Dim drv As DataRowView
        Dim myDataSet As New Data.DataSet
        Dim myTable As New Data.DataTable
        myTable.TableName = "DailyTasks"
        myDataSet.Tables.Add(myTable)

        Dim strSQLSelect As String = ""
        'Dim srcTable, operation, srcStat, destTable, destStat, QueryType As String
        ' Dim value As Single = 0
        Dim rowCounter As Integer = 1
        '11/19/2015 NS added for VSPLUS-2383
        Dim sqlcmd As New SqlCommand
        Dim USprovider As IFormatProvider = CultureInfo.CreateSpecificCulture(sCultureString)
        Dim Europrovider As IFormatProvider = CultureInfo.CreateSpecificCulture("fr-FR")
        WriteAuditEntry(Now.ToString & " USprovider sCultureString is " & sCultureString, LogLevel.Verbose)
        Try

            'Followed the SQL way of providing the date range
            '  Console.WriteLine(" Searching for unprocessed statistics for " & SearchDate.ToShortDateString)
            strSQLSelect = "SELECT DeviceType, ServerName, WeekNumber, MonthNumber, YearNumber, DayNumber," & operation & "(StatValue) AS value FROM " & vbCrLf
            strSQLSelect = strSQLSelect & "" & srcTable & " WHERE StatName = '" & srcStat & "' AND date >= DATEADD(day,DATEDIFF(day,0," & objVSAdaptor.DateFormat(FixDate(SearchDate)) & "),0)" & vbCrLf
            strSQLSelect = strSQLSelect & "AND date < DATEADD(day,DATEDIFF(day,0," & objVSAdaptor.DateFormat(FixDate(SearchDate.AddDays(1))) & "),0) GROUP BY DeviceType, ServerName, WeekNumber, MonthNumber, YearNumber, DayNumber" & vbCrLf
            'strSQLSelect += "ORDER BY YearNumber DESC, MonthNumber DESC, DayNumber DESC"
            WriteAuditEntry(strSQLSelect, LogLevel.Verbose)


            Try
                objVSAdaptor.FillDatasetAny("VSS_Statistics", "statistics", strSQLSelect, myDataSet, "SummaryData")
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error Accessing the " & srcTable & " Table. " & ex.Message & strSQLSelect)
            End Try

            Dim SummaryView As New Data.DataView(myDataSet.Tables("SummaryData"))
            Dim summaryDrv As DataRowView

            Dim strSQLInsert As String = ""
            Dim serverName As String = ""
            Dim deviceName As String = ""
            Dim deviceType As String = ""
            Dim weekNumber, dayNumber, monthNumber, yearNumber As Integer
            Dim summaryValue As Single = 0
            Dim queryToLog As String = String.Empty

            Try
                If SummaryView.Count > 0 Then
                    For Each summaryDrv In SummaryView
                        Try
                            deviceType = summaryDrv("deviceType")
                            serverName = summaryDrv("ServerName")
                            weekNumber = summaryDrv("WeekNumber")
                            dayNumber = summaryDrv("DayNumber")
                            monthNumber = summaryDrv("MonthNumber")
                            yearNumber = summaryDrv("YearNumber")
                            summaryValue = summaryDrv("value")
                            'summaryDate = summaryDrv("")
                            'deviceName = summaryDrv("DeviceName")
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Error getting the values " & ex.Message)
                        End Try

                        If (QueryType = "3") Then
                            '11/19/2015 NS added for VSPLUS-2383
                            Try
                                '11/19/2015 NS modified for VSPLUS-2383
                                strSQLInsert = "INSERT INTO " & destTable & vbCrLf
                                queryToLog = strSQLInsert & String.Format(" VALUES ('{0}','{1}','{2}','{3},'{4}','{5}','{6}','{7}',{8})", deviceType, deviceName, SearchDate, destStat, summaryValue, weekNumber, monthNumber, yearNumber, dayNumber)
                                strSQLInsert = strSQLInsert & " VALUES (@DeviceType,@DeviceName,@Date,@StatName,@StatValue,@WeekNum,@MonthNum,@YearNum,@DayNum)"
                                sqlcmd = New SqlCommand(strSQLInsert)
                                sqlcmd.Parameters.Add(New SqlParameter("@DeviceType", SqlDbType.NVarChar, 100))
                                sqlcmd.Parameters("@DeviceType").Value = deviceType
                                sqlcmd.Parameters.Add(New SqlParameter("@DeviceName", SqlDbType.NVarChar, 100))
                                sqlcmd.Parameters("@DeviceName").Value = serverName
                                sqlcmd.Parameters.Add(New SqlParameter("@Date", SqlDbType.DateTime))
                                sqlcmd.Parameters("@Date").Value = SearchDate
                                sqlcmd.Parameters.Add(New SqlParameter("@StatName", SqlDbType.NVarChar, 50))
                                sqlcmd.Parameters("@StatName").Value = destStat
                                sqlcmd.Parameters.Add(New SqlParameter("@StatValue", SqlDbType.Float))
                                sqlcmd.Parameters("@StatValue").Value = summaryValue
                                sqlcmd.Parameters.Add(New SqlParameter("@WeekNum", SqlDbType.Int))
                                sqlcmd.Parameters("@WeekNum").Value = weekNumber
                                sqlcmd.Parameters.Add(New SqlParameter("@MonthNum", SqlDbType.Int))
                                sqlcmd.Parameters("@MonthNum").Value = monthNumber
                                sqlcmd.Parameters.Add(New SqlParameter("@YearNum", SqlDbType.Int))
                                sqlcmd.Parameters("@YearNum").Value = yearNumber
                                sqlcmd.Parameters.Add(New SqlParameter("@DayNum", SqlDbType.Int))
                                sqlcmd.Parameters("@DayNum").Value = dayNumber
                                'strSQLInsert = strSQLInsert & " VALUES ('" & deviceType & "','" & serverName & "', " & objVSAdaptor.DateFormat(FixDate(SearchDate)) & ", '" & destStat & "', '" & summaryValue & "', '" & weekNumber & "', '" & monthNumber & "', '" & yearNumber & "', '" & dayNumber & "')"
                                WriteAuditEntry(Now.ToString & " SQL INSERT statement is " & queryToLog, LogLevel.Verbose)
                            Catch ex As Exception
                                WriteAuditEntry(Now.ToString & " Exception creating INSERT statement: " & ex.ToString)
                            End Try
                        End If
                        Try
                            '11/19/2015 NS modified for VSPLUS-2383
                            'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQLInsert)
                            objVSAdaptor.ExecuteNonQuerySQLParams("VSS_Statistics", sqlcmd)
                        Catch ex As Exception
                            WriteAuditEntry(Now.ToString & " Failed to insert Summary Data into " & destTable & " because: " & ex.Message)
                            WriteAuditEntry(Now.ToString & " The failed stats table insert command was " & strSQLInsert)
                        End Try
                    Next
                Else
                    WriteAuditEntry(Now.ToString & " This query did not produce any rows.", LogLevel.Verbose)
                End If
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Exception where I least expected it: " & ex.ToString)
            End Try

            Try
                '  myDataSet.Tables.Remove("SummaryData")
                myDataSet.Tables("SummaryData").Clear()
            Catch ex As Exception
                WriteAuditEntry("Could not Remove the table as no data in it.", LogLevel.Verbose)
            End Try

            WriteAuditEntry("Skipping this row as not all the required parameters were provided.", LogLevel.Verbose)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Exception creating SQL in DailyTasks: " & ex.ToString)
        End Try


    End Sub
	Public Sub ProcessStoredProcedures(StatName As String)
		Dim con As New SqlConnection

		Try
			Dim myAdapter As New VSFramework.XMLOperation
			con.ConnectionString = myAdapter.GetDBConnectionString("VSS_Statistics")
			con.Open()
			Dim da As SqlDataAdapter
			da = New SqlDataAdapter("PopulateTravelerSummaryStats", con)
			da.SelectCommand.CommandType = CommandType.StoredProcedure
			da.SelectCommand.Parameters.Add(New SqlParameter("@StatName", SqlDbType.VarChar, 50))
			da.SelectCommand.Parameters("@StatName").Value = StatName
			da.SelectCommand.ExecuteNonQuery()

		Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Exception in ProcessStoredProcedures: " & ex.Message)
		Finally
			If con.State = ConnectionState.Open Then
				con.Close()
				con.Dispose()
			End If
		End Try


	End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        Try
            WriteAuditEntry(Now.ToString & " Shutting down....")
            TimeToStop = True
            Dim myRegistry As New RegistryHandler
            myRegistry.WriteToRegistry("Daily Tasks End", Now.ToShortDateString & " " & Now.ToShortTimeString)

        Catch ex As Exception

        End Try
        MyBase.OnStop()
    End Sub

    Public Sub CleanUpObsoleteData()
        WriteAuditEntry("Cleaning up obsolete and processed data....")
        Dim objVSAdaptor As New VSAdaptor
        'Update Summary Data of various Stats 
        Dim strSQL As String = ""
        Dim myDataSet As New Data.DataSet
        Dim myTable As New Data.DataTable
        '11/24/2015 NS added for VSPLUS-2383
        Dim sqlcmd As New SqlCommand
        myTable.TableName = "DailyTasks"
        myDataSet.Tables.Add(myTable)

        strSQL = "SELECT distinct SourceTableName FROM DailyTasks"

        WriteAuditEntry(vbCrLf & strSQL & vbCrLf)

        Try
            objVSAdaptor.FillDatasetAny("VitalSigns", "vitalsigns", strSQL, myDataSet, "DailyTasks")
        Catch ex As Exception
            WriteAuditEntry("Exception = " & ex.ToString())
            WriteAuditEntry(Now.ToString & " Error Accessing the DailyTasks Table " & ex.Message & strSQL)
        End Try
        'Dim myView As New Data.DataView(myDataSet.Tables("DailyTasks"))
        'Dim drv As DataRowView
        Dim strSQLSelect As String = ""
        'Dim srcTable As String
        Dim sourceTables As DataTable = myDataSet.Tables("DailyTasks")
        Dim strTables As Object = String.Join(",", (From sourceTable In sourceTables.Rows Select sourceTable(0)).ToList())
        WriteAuditEntry(vbCrLf & vbCrLf & "Tables Will be impacted : " & strTables & vbCrLf)
        Dim taskStatsCleanUp As Tasks.Task = Task.Factory.StartNew(Sub() CleanUpStatsTable(strTables))
        taskStatsCleanUp.Wait()
        Dim taskVSStatsCleanUp As Tasks.Task = Task.Factory.StartNew(Sub() CleanUpVSTables())
        taskVSStatsCleanUp.Wait()

        WriteAuditEntry(vbCrLf & vbCrLf & "***********************************************" & vbCrLf & "Finished!")
    End Sub
    Private Sub CleanUpStatsTable(ByVal strTables As String)
        'This function takes a table name as a parameter then passes it to a stored procedure ("CleanUpObsoleteData") to remove data from VSS_Statistics which is no longer needed
        Dim myConnectionString As New VSFramework.XMLOperation
        Dim connectionString As String = myConnectionString.GetDBConnectionString("VSS_Statistics")
        Using connection As New System.Data.SqlClient.SqlConnection(connectionString)
            Try
                connection.Open()
                Dim command As SqlCommand = New SqlCommand("CleanUpObsoleteData", connection)
                command.CommandTimeout = 2000
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.AddWithValue("@DelimatedString", strTables)
                command.ExecuteNonQuery()
                WriteAuditEntry(vbCrLf & vbCrLf & "Success while executing the CleanUpObsoleteData stored Procedure--" & vbCrLf)
            Catch ex As Exception
				WriteAuditEntry(vbCrLf & vbCrLf & "Error while executing the CleanUpObsoleteData stored Procedure--" & ex.Message & vbCrLf)
			Finally
				If connection.State = ConnectionState.Open Then
					connection.Close()
				End If

			End Try
        End Using
    End Sub

    Private Sub CleanUpVSTables()
        'This function takes a table name as a parameter then passes it to a stored procedure ("CleanUpObsoleteData") to remove data from VitalSigns db which is no longer needed
        Dim myConnectionString As New VSFramework.XMLOperation
        Dim connectionString As String = myConnectionString.GetDBConnectionString("VitalSigns")
        Using connection As New System.Data.SqlClient.SqlConnection(connectionString)
            Try
                connection.Open()
                Dim command As SqlCommand = New SqlCommand("CleanUpData", connection)
                command.CommandTimeout = 2000
                command.CommandType = CommandType.StoredProcedure
                command.ExecuteNonQuery()
            Catch ex As Exception
				WriteAuditEntry(vbCrLf & vbCrLf & "Error while executing the CleanUpData stored Procedure--" & ex.Message & vbCrLf)
			Finally
				If connection.State = ConnectionState.Open Then
					connection.Close()
				End If
			End Try
        End Using
    End Sub

    Public Sub LogTableStatistics(ByVal dataBase As String)
        WriteAuditEntry("Print Log table Statistics... ")
        Dim objVSAdaptor As New VSAdaptor
        'Update Summary Data of various Stats 
        Dim strSQL As String = ""
        Dim myDataSet As New Data.DataSet
        Dim myTable As New Data.DataTable
        myTable.TableName = "TableData"
        myDataSet.Tables.Add(myTable)

        strSQL = "SELECT V.Name AS 'TableName' , SUM(B.rows) AS 'RowCount' FROM sys.objects V INNER JOIN sys.partitions B ON V.object_id = B.object_id WHERE V.type = 'U' GROUP BY V.schema_id, V.Name"

        WriteAuditEntry(vbCrLf & strSQL & vbCrLf)

        Try
            objVSAdaptor.FillDatasetAny(dataBase, dataBase, strSQL, myDataSet, "TableData")
        Catch ex As Exception
            WriteAuditEntry("Exception = " & ex.ToString())
            WriteAuditEntry(Now.ToString & " Error Accessing the DailyTasks Table " & ex.Message & strSQL)
        End Try

        Dim myView As New Data.DataView(myDataSet.Tables("TableData"))
        Dim drv As DataRowView

        Dim strSQLSelect As String = ""
        Dim tableName As String
        Dim rowCount As String

        WriteAuditEntry("VitalSigns Database table Statics are as below.. ")

        For Each drv In myView
            Try
                tableName = drv("TableName")
                rowCount = drv("RowCount")
                WriteAuditEntry("Table Name = " & tableName & vbTab & vbTab & " Row Count = " & rowCount)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Problem in processing Rows... " & ex.ToString)
            End Try
        Next
        WriteAuditEntry(vbCrLf & vbCrLf & "***********************************************" & vbCrLf & "Finished!")
    End Sub


    Public Sub DailyBackup()
        Dim LogFilesToBeRecreated() As String = {"History.txt", "Daily_Tasks_Log.txt"}
        If MyLogLevel = LogLevel.Verbose Then
            WriteAuditEntry(Now.ToString & " ***** Starting Daily Log File Zip Up ******* ")
            WriteAuditEntry(vbCrLf)
        End If

        'Create the backup directory
        Try
            If Not Directory.Exists(strAppPath & "\Log_Files\Backup\") Then
                Directory.CreateDirectory(strAppPath & "\Log_Files\Backup\")
            End If
        Catch ex As Exception

        End Try

        'Delete the files that are already in there so that the new ones can be zipped
        Try
            WriteAuditEntry(Now.ToString & " Deleting any leftover *.txt files from the \backup folder.")
            Dim fileArray As String()
            WriteAuditEntry(Now.ToString + " Cleaning up log files")
            fileArray = Directory.GetFiles(strAppPath & "\Log_Files\Backup\", "*.txt")

            Dim myFile As String
            For Each myFile In fileArray
                File.Delete(myFile)
                WriteAuditEntry(Now.ToString + " Deleting " & myFile)
            Next
            fileArray = Nothing

            Dim ExchangeFolders As String() = Directory.GetDirectories(strAppPath & "\Log_Files\Backup\")
            Dim myFolder As String
            For Each myFolder In ExchangeFolders
                My.Computer.FileSystem.DeleteDirectory(myFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                WriteAuditEntry(Now.ToString + " Deleting " & myFolder)
            Next
            ExchangeFolders = Nothing

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error cleaning up past log files " & ex.ToString)
        End Try

        WriteAuditEntry(vbCrLf & vbCrLf & Now.ToString & "**********************************************")

        'Copy the latest files into the backup folder to be zipped
        Try

            Dim fileArray As String()
            WriteAuditEntry(Now.ToString + " Moving the current log files to the backup folder")
            fileArray = Directory.GetFiles(strAppPath & "\Log_Files\", "*.txt")

            Dim myFile As String
            For Each myFile In fileArray
                Dim dest As String = Path.Combine(strAppPath & "\Log_Files\backup\", Path.GetFileName(myFile))
                WriteAuditEntry(Now.ToString + " Moving " & myFile & " to " & dest)
                File.Move(myFile, dest)
                If Array.IndexOf(LogFilesToBeRecreated, Path.GetFileName(myFile)) > -1 Then
                    File.Create(Path.GetFileName(myFile))
                End If

            Next
            fileArray = Nothing

            Dim ExchangeFolders As String() = Directory.GetDirectories(strAppPath & "\Log_Files\")

            Dim folder As String
            For Each folder In ExchangeFolders
                If (Path.GetFileName(folder).ToLower() = "backup") Then
                    Continue For
                End If
                Dim destFolder As String = strAppPath & "\Log_Files\backup\" & Path.GetFileName(folder) & ""
                If (My.Computer.FileSystem.DirectoryExists(destFolder) = False) Then
                    My.Computer.FileSystem.CreateDirectory(destFolder)
                End If
                WriteAuditEntry(Now.ToString + " Moving folder " & folder & " to " & destFolder)
                My.Computer.FileSystem.MoveDirectory(folder, destFolder)


            Next
            ExchangeFolders = Nothing

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error copying the files to backup: " & ex.ToString)
        End Try

        'Delete previous zip file
        Try
            Dim myzipfile As String = ""
            myzipfile = strAppPath & "\Log_Files\backup\" & Now.DayOfWeek.ToString & ".zip"
            WriteAuditEntry(Now.ToString + " Deleting prior week's zip file, if present.")
            File.Delete(myzipfile)
        Catch ex As Exception

        End Try

        'Adds files to be zipped and zips them up
        Try
            WriteAuditEntry(vbCrLf & vbCrLf & Now.ToString & "**********************************************")
            Dim myZip As New ZipFile
            Dim zipFileArray As String()
            WriteAuditEntry(Now.ToString + " Creating new zip file")
            zipFileArray = Directory.GetFiles(strAppPath & "\Log_Files\backup\", "*.txt")
            Dim myFile As String
            Try
                For Each myFile In zipFileArray
                    WriteAuditEntry(Now.ToString + " Zipping " & myFile)
                    myZip.AddFile(myFile, "")
                    ' myZip.AddFile(myFile)
                Next
            Catch ex As Exception
                WriteAuditEntry(Now.ToString + " Error zipping: " & ex.ToString)
            End Try

            Dim zipFolderArray As String()
            zipFolderArray = Directory.GetDirectories(strAppPath & "\Log_Files\backup\")
            Dim myFolder As String
            Try
                For Each myFolder In zipFolderArray
                    WriteAuditEntry(Now.ToString + " Zipping Folder " & myFolder)
                    myZip.AddDirectory(myFolder, Path.GetFileName(myFolder))
                Next
            Catch ex As Exception
                WriteAuditEntry(Now.ToString + " Error zipping folders: " & ex.ToString)
            End Try

            myZip.Save(strAppPath & "\Log_Files\backup\" & Now.DayOfWeek.ToString & ".zip")
            WriteAuditEntry(Now.ToString + " The zip file is created as " & Now.DayOfWeek.ToString & ".zip")
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating zip file: ")
        End Try

        'Delete files in the back up folder after they have been zipped
        Try
            WriteAuditEntry(Now.ToString + " Deleting the backup log files. ")
            Dim fileArray As String()
            fileArray = Directory.GetFiles(strAppPath & "\Log_Files\Backup", "*.txt")

            Dim myFile As String
            For Each myFile In fileArray
                WriteAuditEntry(Now.ToString + " Deleting " & myFile & "....")
                File.Delete(myFile)
            Next

            Dim ExchangeFolders As String() = Directory.GetDirectories(strAppPath & "\Log_Files\Backup\")
            Dim myFolder As String
            For Each myFolder In ExchangeFolders
                WriteAuditEntry(Now.ToString + " Deleting " & myFolder & "...")
                My.Computer.FileSystem.DeleteDirectory(myFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
            Next
            ExchangeFolders = Nothing

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error cleaning up the current log files " & ex.ToString)
        End Try


        '' Delete the log files from the current directory as well except the DailyBackup 
        'Try
        '	WriteAuditEntry(Now.ToString + " Deleting the backup log files. ")
        '	Dim fileArray As String()
        '	fileArray = Directory.GetFiles(strAppPath & "\Log_Files", "*.txt")

        '	Dim myFile As String
        '	For Each myFile In fileArray
        '		'If myFile.Contains("Daily_Tasks_Log.txt") Then
        '		'WriteAuditEntry(Now.ToString + " NOT Deleting " & myFile & "....")
        '		'Else
        '		WriteAuditEntry(Now.ToString + " Deleting " & myFile & "....")
        '		File.Delete(myFile)
        '		'End If
        '	Next

        '	Dim ExchangeFolders As String() = Directory.GetDirectories(strAppPath & "\Log_Files")
        '	Dim myFolder As String
        '	For Each myFolder In ExchangeFolders
        '		If (Path.GetFileName(myFolder).ToLower() = "backup") Then
        '			Continue For
        '		End If
        '		WriteAuditEntry(Now.ToString + " Deleting " & myFolder & "...")
        '		My.Computer.FileSystem.DeleteDirectory(myFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
        '	Next
        '	ExchangeFolders = Nothing
        'Catch ex As Exception
        '	WriteAuditEntry(Now.ToString & " Error cleaning up the current log files " & ex.ToString)
        'End Try


        If MyLogLevel = LogLevel.Verbose Then
            WriteAuditEntry(Now.ToString & " Finished Daily log file zip up.")
        End If
        '1537'
        LoglevelToNormal()

    End Sub

    '1537 Swathi Changing Logsettings to normal'
    Private Sub LoglevelToNormal()
        MyLogLevel = LogLevel.Normal
        Dim myRegistry As New RegistryHandler
        myRegistry.WriteToRegistry("Log Level", LogLevel.Normal)
        myRegistry.WriteToRegistry("Log Level VSAdapter", LogLevel.Normal)
    End Sub

    'Durga VSPLUS 1874 6/26/2015
	Private Sub Shrinkdb(ByVal connection As String)
		Dim myAdapter As New VSFramework.XMLOperation
		Using con As New System.Data.SqlClient.SqlConnection(myAdapter.GetDBConnectionString(connection))
			Try
				con.Open()
				Try
					Dim sql As String = "ALTER DATABASE " + connection + " SET RECOVERY SIMPLE"
					Dim command1 As New SqlCommand(sql, con)
					command1.ExecuteNonQuery()
				Catch ex As Exception

				End Try

				Dim command2 As New SqlCommand("DBCC SHRINKDATABASE(0)", con)
				'Increase the timeout
				command2.CommandTimeout = 1000
				command2.ExecuteNonQuery()

				'reset it back
				Try
					Dim sql As String = "ALTER DATABASE " + connection + " SET RECOVERY FULL"
					Dim command1 As New SqlCommand(sql, con)
					command1.ExecuteNonQuery()
				Catch ex As Exception

				End Try

			Catch ex As Exception
			Finally
				If con.State = ConnectionState.Open Then
					con.Close()
				End If
			End Try
		End Using
	End Sub

    '2/12/2016 Durga Modified for VSPLUS 2174
    Private Sub CleanUpTravelerSummaryData()
        WriteAuditEntry(Now.ToString & " Cleaning up TravelerStats for today in case the query has been run today already. ")

        Dim SearchDate As DateTime
        SearchDate = FixDate(Today.AddDays(-30))
        Dim strSQL As String = ""
        Try
            strSQL = "delete FROM [vitalsigns].[dbo].[TravelerStats] WHERE [DateUpdated] <= CAST(GETDATE() AS DATE)"
        Catch ex As Exception

        End Try

        Try
            objVSAdaptor.ExecuteNonQueryAny("vitalsigns", "servers", strSQL)

        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error executing SQL command " & ex.ToString)
        End Try

        Try
            GC.Collect()
        Catch ex As Exception
            '  Thread.Sleep(500)
        End Try

        WriteAuditEntry(Now.ToString & " Finished cleaning up TravelerStats ")

    End Sub



    '6/25/2015 NS added for VSPLUS-1226
    Private Sub CleanupAnyTableWeekly()
        Dim myDataSet As New Data.DataSet
        Dim dt As DataTable
        Dim strSQL As String = ""
        Dim strSQL2 As String = ""
        Dim strCleanup As String = ""
        Dim whereClause As String = ""
        Dim myConnectionString As New VSFramework.XMLOperation

        Try
            'DailyCleanup table processing - allows for conditional, parameterized clean up of any table
            strSQL = "SELECT ID,DBName,TableName,ISNULL(ParameterType,'') ParameterType,ISNULL(Parameter,'') Parameter,ISNULL(Condition,'') Condition,ISNULL(Value,'') Value FROM DailyCleanup"
            dt = objVSAdaptor.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), strSQL)
            If (dt.Rows.Count > 0) Then
                For n As Integer = 0 To dt.Rows.Count - 1
                    If dt.Rows(n).Item("ParameterType") = "DateTime" Then
                        whereClause = " WHERE " & dt.Rows(n).Item("Parameter") & dt.Rows(n).Item("Condition") & "CONVERT(DateTime, " & dt.Rows(n).Item("Value") & ", 120)"
                    ElseIf dt.Rows(n).Item("ParameterType") = "String" Then
                        whereClause = " WHERE " & dt.Rows(n).Item("Parameter") & dt.Rows(n).Item("Condition") & "'" & dt.Rows(n).Item("Value") & "'"
                    ElseIf dt.Rows(n).Item("ParameterType") = "Number" Then
                        whereClause = " WHERE " & dt.Rows(n).Item("Parameter") & dt.Rows(n).Item("Condition") & dt.Rows(n).Item("Value")
                    Else
                        whereClause = ""
                    End If
                    strCleanup = "DELETE FROM " & dt.Rows(n).Item("DBName") & ".dbo." & dt.Rows(n).Item("TableName") & whereClause
                    WriteAuditEntry(Now.ToString & " Cleanup SQL query " & strCleanup)
                    Try
                        objVSAdaptor.ExecuteNonQueryAny(dt.Rows(n).Item("DBName"), dt.Rows(n).Item("DBName"), strCleanup)
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error executing SQL command " & strCleanup & " - " & ex.ToString)
                    End Try
                Next
            End If
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error executing SQL command " & strSQL & " - " & ex.ToString)
        End Try

        Try
            'DailyTasks table processing - allows for the stat table cleanup where the servers/devices have been decommissioned
            strSQL = "SELECT DISTINCT SourceTablename,DestinationTableName FROM DailyTasks"
            dt = objVSAdaptor.FetchData(myConnectionString.GetDBConnectionString("VitalSigns"), strSQL)
            If (dt.Rows.Count > 0) Then
                For n As Integer = 0 To dt.Rows.Count - 1
                    strCleanup = "DELETE FROM [VSS_Statistics].dbo." & dt.Rows(n).Item("SourceTablename") & " " &
                     "WHERE ServerName NOT IN( " & _
                     "SELECT DISTINCT t1.ServerName from [VSS_Statistics].dbo." & dt.Rows(n).Item("SourceTablename") & " t1 " & _
                     "INNER JOIN dbo.DeviceInventory t2 ON t1.ServerName=t2.Name) "
                    WriteAuditEntry(Now.ToString & " Cleanup SQL query " & strCleanup)
                    Try
                        objVSAdaptor.ExecuteNonQueryAny("vitalsigns", "vitalsigns", strCleanup)
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error executing SQL command " & strCleanup & " - " & ex.ToString)
                    End Try
                    strCleanup = "DELETE FROM [VSS_Statistics].dbo." & dt.Rows(n).Item("DestinationTableName") & " " &
                     "WHERE ServerName NOT IN( " & _
                     "SELECT DISTINCT t1.ServerName from [VSS_Statistics].dbo." & dt.Rows(n).Item("DestinationTableName") & " t1 " & _
                     "INNER JOIN dbo.DeviceInventory t2 ON t1.ServerName=t2.Name) "
                    WriteAuditEntry(Now.ToString & " Cleanup SQL query " & strCleanup)
                    Try
                        objVSAdaptor.ExecuteNonQueryAny("vitalsigns", "vitalsigns", strCleanup)
                    Catch ex As Exception
                        WriteAuditEntry(Now.ToString & " Error executing SQL command " & strCleanup & " - " & ex.ToString)
                    End Try
                Next
            End If
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error executing SQL command " & strSQL & " - " & ex.ToString)
        End Try

        Try
            GC.Collect()
        Catch ex As Exception

        End Try
    End Sub

    Private Function FixDate(ByVal dt As DateTime) As String
        ' Return dt.ToUniversalTime.ToString
        Return objDateUtils.FixDate(dt, strDateFormat)
    End Function

#Region "Disk Space"

#Region "Domino Servers"

    Public Sub BuildDominoDriveList()
        WriteAuditEntry(Now.ToString & " Calculating all the unique drive names. ")
        Dim strSQL As String = "SELECT DISTINCT DiskName FROM DominoDiskSpace"
        Dim myDataSet As New Data.DataSet
        Dim myTable As New Data.DataTable
        myDataSet.Tables.Add(myTable)

        Dim myDriveCount As Integer = 0

        Try
            'myAverageResponse = myStatisticsCommand.ExecuteScalar
            objVSAdaptor.FillDatasetAny("VitalSigns", "statistics", strSQL, myDataSet, "MyTable")
            WriteAuditEntry(Now.ToString & " Filled the dataset ")
            myDriveCount = myDataSet.Tables("MyTable").Rows.Count
            WriteAuditEntry(Now.ToString & " The dataset has " & myDriveCount & " unique drive names.")
            ReDim DominoDiskNames(myDriveCount)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Daily Summary " & ex.Message & "-- The failed Average command was " & strSQL)
        End Try

        Dim n As Integer = 0
        Dim myView As New Data.DataView(myDataSet.Tables("MyTable"))
        Dim drv As DataRowView

        For Each drv In myView
            Try
                WriteAuditEntry(Now.ToString & " Found drive " & drv("DiskName"))
                DominoDiskNames(n) = drv("DiskName")
                n += 1

            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Exception creating SQL: " & ex.ToString)
            End Try

        Next

    End Sub

    'Process the disks
    Public Sub ConsolidateDominoDiskStats(ByVal SearchDate As Date)
        Dim myDiskFree As String = ""
        For n As Integer = 0 To DominoDiskNames.GetUpperBound(0)
            myDiskFree = DominoDiskNames(n) & ".Free"
            GenerateSummaryforAllDomino(SearchDate, myDiskFree, "AVG")
        Next

    End Sub


    Public Sub GenerateSummaryforAllDomino(ByVal StatDate As DateTime, ByVal SrcStatName As String, ByVal operation As String)

        'this sub finds all the ResponseTime records for a given date and averages them for the day
        'then writes the average as DailyResponseAverage stat then deletes the detail records
        '11/20/2015 NS added for VSPLUS-2383
        Dim sqlcmd As New SqlCommand
        Dim strSQL As String = ""

        Dim EndDate As DateTime = StatDate.AddDays(1)
        Dim myDataSet As New Data.DataSet
        Dim myTable As New Data.DataTable
        myDataSet.Tables.Add(myTable)

        ' SELECT DeviceType, DeviceName ,  JustDate , AVG(StatValue)  AS Average FROM 
        '(SELECT DeviceType, DeviceName, CONVERT(DATE, [Date]) AS JustDate , StatValue, StatName FROM DeviceUpTimeStats WHERE StatName = 'HourlyUpPercent'  AND Date > '29-Jun-2013' AND Date < '30-Jun-2013') as t1
        'GROUP BY DeviceType, DeviceName , JustDate  
        'ORDER BY DeviceType, DeviceName, JustDate DESC

        strSQL = "SELECT ServerName ,  JustDate , " + operation + "(StatValue)  AS Average " & vbCrLf
        strSQL += "FROM (SELECT ServerName,  CONVERT(DATE, [Date]) AS JustDate , StatValue, StatName " & vbCrLf
        strSQL += "FROM DominoDailyStats WHERE StatName = '" + SrcStatName + "' " & vbCrLf
        strSQL += " AND Date > " & objVSAdaptor.DateFormat(FixDate(StatDate)) & " AND Date < " & objVSAdaptor.DateFormat(FixDate(EndDate))

        strSQL += ") as t1  " & vbCrLf
        strSQL += "GROUP BY ServerName , JustDate " & vbCrLf
        strSQL += " ORDER BY  ServerName, JustDate DESC" & vbCrLf

        WriteAuditEntry(vbCrLf & strSQL & vbCrLf)

        Try
            'myAverageResponse = myStatisticsCommand.ExecuteScalar
            objVSAdaptor.FillDatasetAny("VSS_Statistics", "statistics", strSQL, myDataSet, "MyTable")
            'the number will come as 99.5, so we need to divide by 100 because the WriteAvailability sub multiplies by 100
            ' WriteAuditEntry(Now.ToString & " Filled the dataset ")
            '  WriteAuditEntry(Now.ToString & " The dataset has " & myDataSet.Tables("MyTable").Rows.Count)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Daily Summary " & ex.Message & "-- The failed Average command was " & strSQL)
        End Try

        Dim myView As New Data.DataView(myDataSet.Tables("MyTable"))
        Dim drv As DataRowView

        Dim strSQLInsert As String = ""
        Dim ServerName, MyNumberString As String
        Dim MyWeekNumber As Integer
        Dim myNumber As Double
        Dim QueryToLog As String
        For Each drv In myView
            Try

                ServerName = drv("ServerName")
                '   WriteAuditEntry(Now.ToString & "  Device name is " & DeviceName)
                myNumber = drv("Average")
                MyNumberString = myNumber.ToString("F2")
                '     WriteAuditEntry(Now.ToString & "  Number string is " & MyNumberString)
                MyWeekNumber = GetWeekNumber(StatDate)
                '     WriteAuditEntry(Now.ToString & " My week number is " & MyWeekNumber)
                '11/20/2015 NS modified for VSPLUS-2383
                'strSQLInsert = "INSERT INTO DominoSummaryStats (ServerName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber) VALUES " & vbCrLf
                'strSQLInsert += " ('" & ServerName & "', '" & FixDate(StatDate) & "', '" & SrcStatName & "', '" & MyNumberString & "', '" & MyWeekNumber & "', '" & StatDate.Month & "', '" & StatDate.Year & "', '" & StatDate.Day & "')"
                strSQLInsert = "INSERT INTO DominoSummaryStats (ServerName, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber) " & vbCrLf
                QueryToLog = strSQLInsert + (String.Format(" values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", ServerName, StatDate, SrcStatName, MyNumberString, MyWeekNumber, StatDate.Month, StatDate.Year, StatDate.Day))
                strSQLInsert = strSQLInsert & " VALUES (@DeviceName,@Date,@StatName,@StatValue,@WeekNum,@MonthNum,@YearNum,@DayNum)"
                sqlcmd = New SqlCommand(strSQLInsert)
                sqlcmd.Parameters.Add(New SqlParameter("@DeviceName", SqlDbType.NVarChar, 100))
                sqlcmd.Parameters("@DeviceName").Value = ServerName
                sqlcmd.Parameters.Add(New SqlParameter("@Date", SqlDbType.DateTime))
                sqlcmd.Parameters("@Date").Value = StatDate
                sqlcmd.Parameters.Add(New SqlParameter("@StatName", SqlDbType.NVarChar, 50))
                sqlcmd.Parameters("@StatName").Value = SrcStatName
                sqlcmd.Parameters.Add(New SqlParameter("@StatValue", SqlDbType.Float))
                sqlcmd.Parameters("@StatValue").Value = MyNumberString
                sqlcmd.Parameters.Add(New SqlParameter("@WeekNum", SqlDbType.Int))
                sqlcmd.Parameters("@WeekNum").Value = MyWeekNumber
                sqlcmd.Parameters.Add(New SqlParameter("@MonthNum", SqlDbType.Int))
                sqlcmd.Parameters("@MonthNum").Value = StatDate.Month
                sqlcmd.Parameters.Add(New SqlParameter("@YearNum", SqlDbType.Int))
                sqlcmd.Parameters("@YearNum").Value = StatDate.Year
                sqlcmd.Parameters.Add(New SqlParameter("@DayNum", SqlDbType.Int))
                sqlcmd.Parameters("@DayNum").Value = StatDate.Day

                WriteAuditEntry(Now.ToString & " SQL statement is " & vbCrLf & QueryToLog & vbCrLf & vbCrLf, LogLevel.Verbose)

            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Exception creating SQL: " & ex.ToString)
            End Try

            Try
                '11/20/2015 NS modified for VSPLUS-2383
                'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQLInsert)
                objVSAdaptor.ExecuteNonQuerySQLParams("VSS_Statistics", sqlcmd)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Availability Stats table insert failed because: " & ex.Message)
                WriteAuditEntry(Now.ToString & " The failed stats table insert command was " & strSQL)
            End Try
        Next

        '   strSQL = "DELETE FROM DominoDailyStats WHERE Date>" & objVSAdaptor.DateFormat(FixDate(StatDate)) & " AND Date<" & objVSAdaptor.DateFormat(FixDate(EndDate)) & " AND " & _
        '    "StatName='" & SrcStatName & "'"

        ' I want to keep more data

        strSQL = "DELETE FROM DominoDailyStats WHERE  Date<" & objVSAdaptor.DateFormat(FixDate(StatDate.AddDays(-2))) & " AND StatName='" & SrcStatName & "'"

        Try
            objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQL)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error Deleting Obsolete Daily Statistics Records " & ex.Message)
        End Try

        WriteAuditEntry(Now.ToString & " Finished processing SummaryforAllDomino for Stat" & SrcStatName & " Date = " & StatDate)
        Thread.Sleep(250)
    End Sub

    Public Sub DeleteDominoDailyStats()
        'Durga  VSPLUS 2281

        Dim strSQL As String
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim vsobj As New VSAdaptor
        Dim CleanupMonth As Integer
		Dim con As New SqlConnection
        Try
            Try
                strSQL = "select * from Settings where sname='CleanupMonth'"

                dt.TableName = "Settings"
                ds.Tables.Add(dt)
                vsobj.FillDatasetAny("VitalSigns", "vitalsigns", strSQL, ds, "Settings")

                If dt.Rows.Count > 0 Then

                    CleanupMonth = Convert.ToInt32(dt.Rows(0)("svalue").ToString())
                    WriteAuditEntry(Now.ToString & " Sucess in getting CleanupMonth from settings table. " & CleanupMonth)
                End If
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error in getting CleanupMonth from settings table. " & ex.Message)
            End Try

            Dim myAdapter As New VSFramework.XMLOperation
            con.ConnectionString = myAdapter.GetDBConnectionString("VSS_Statistics")
            con.Open()
            Dim da As SqlDataAdapter
            da = New SqlDataAdapter("DominoDailyCleanup", con)
            da.SelectCommand.CommandType = CommandType.StoredProcedure
            da.SelectCommand.Parameters.Add(New SqlParameter("@month", SqlDbType.VarChar, 50))
            da.SelectCommand.Parameters("@month").Value = CleanupMonth

            da.SelectCommand.ExecuteNonQuery()
            WriteAuditEntry(Now.ToString & " Sucess in  Deleting Obsolete Daily Statistics Records more than 6 months old ")
        Catch ex As Exception
			WriteAuditEntry(Now.ToString & " Error Deleting Obsolete Daily Statistics Records more than 6 months old " & ex.Message)
		Finally
			If con.State = ConnectionState.Open Then
				con.Close()
				con.Dispose()
			End If
		End Try

    End Sub

#End Region

#Region "Microsoft Servers"

    Public Sub BuildMicrosoftDriveList()
        WriteAuditEntry(Now.ToString & " Calculating all the unique drive names. ")
        Dim strSQL As String = "SELECT DISTINCT DiskName FROM DiskSpace"
        Dim myDataSet As New Data.DataSet
        Dim myTable As New Data.DataTable
        myDataSet.Tables.Add(myTable)

        Dim myDriveCount As Integer = 0

        Try
            'myAverageResponse = myStatisticsCommand.ExecuteScalar
            objVSAdaptor.FillDatasetAny("VitalSigns", "statistics", strSQL, myDataSet, "MyTable")
            WriteAuditEntry(Now.ToString & " Filled the dataset ")
            myDriveCount = myDataSet.Tables("MyTable").Rows.Count
            WriteAuditEntry(Now.ToString & " The dataset has " & myDriveCount & " unique drive names.")
            ReDim MicrosoftDiskNames(myDriveCount)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Daily Summary " & ex.Message & "-- The failed Average command was " & strSQL)
        End Try

        Dim n As Integer = 0
        Dim myView As New Data.DataView(myDataSet.Tables("MyTable"))
        Dim drv As DataRowView

        For Each drv In myView
            Try
                WriteAuditEntry(Now.ToString & " Found drive " & drv("DiskName"))
                MicrosoftDiskNames(n) = drv("DiskName")
                n += 1

            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Exception creating SQL: " & ex.ToString)
            End Try

        Next

    End Sub

    'Process the disks
    Public Sub ConsolidateServerDiskStats(ByVal SearchDate As Date)
        '11/20/2015 NS added for VSPLUS-2383
        Dim sqlcmd As New SqlCommand
        Dim strSQL As String = ""

        Dim myDataSet As New Data.DataSet
        Dim myTable As New Data.DataTable
        myDataSet.Tables.Add(myTable)

        strSQL = "Select ServerName, DiskName, DiskFree, ServerTypes.ID FROM [vitalsigns].[dbo].[DiskSpace] inner join [vitalsigns].[dbo].[ServerTypes] on ServerTypes.ServerType=DiskSpace.ServerType"

        WriteAuditEntry(vbCrLf & strSQL & vbCrLf)

        Try
            'myAverageResponse = myStatisticsCommand.ExecuteScalar
            objVSAdaptor.FillDatasetAny("VSS_Statistics", "vitalsigns", strSQL, myDataSet, "MyTable")
            'the number will come as 99.5, so we need to divide by 100 because the WriteAvailability sub multiplies by 100
            ' WriteAuditEntry(Now.ToString & " Filled the dataset ")
            '  WriteAuditEntry(Now.ToString & " The dataset has " & myDataSet.Tables("MyTable").Rows.Count)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Daily Disk Space Summary " & ex.Message & "-- The failed command was " & strSQL)
        End Try

        Dim myView As New Data.DataView(myDataSet.Tables("MyTable"))
        Dim drv As DataRowView

        Dim strSQLInsert As String = ""
        Dim ServerName, MyNumberString, DriveName As String
        Dim MyWeekNumber As Integer
        Dim myNumber As Double
        Dim queryToLog As String = String.Empty

        For Each drv In myView
            Try

                ServerName = drv("ServerName")
                '   WriteAuditEntry(Now.ToString & "  Device name is " & DeviceName)
                myNumber = drv("DiskFree")
                MyNumberString = myNumber.ToString("F2")
                '     WriteAuditEntry(Now.ToString & "  Number string is " & MyNumberString)
                MyWeekNumber = GetWeekNumber(Date.Now)
                '     WriteAuditEntry(Now.ToString & " My week number is " & MyWeekNumber)
                DriveName = drv("DiskName")
                '11/20/2015 NS modified for VSPLUS-2383
                'strSQLInsert = "INSERT INTO MicrosoftSummaryStats (ServerName, ServerTypeId, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber) VALUES " & vbCrLf
                'strSQLInsert += " ('" & ServerName & "', '" & drv("ID") & "', " & objVSAdaptor.DateFormat(FixDate(SearchDate)) & ", '" & "Disk." & DriveName & "', '" & MyNumberString & "', '" & MyWeekNumber & "', '" & SearchDate.Month & "', '" & SearchDate.Year & "', '" & SearchDate.Day & "')"
                strSQLInsert = "INSERT INTO MicrosoftSummaryStats (ServerName, ServerTypeId, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber) " & vbCrLf
                queryToLog = strSQLInsert & String.Format(" VALUES ('{0}','{1}','{2}','{3},'{4}','{5}','{6}','{7}',{8})", ServerName, drv("ID"), SearchDate, "Disk." & DriveName, MyNumberString, MyWeekNumber, SearchDate.Month, SearchDate.Year, SearchDate.Day)
                strSQLInsert = strSQLInsert & " VALUES (@DeviceName,@SrvTypeID,@Date,@StatName,@StatValue,@WeekNum,@MonthNum,@YearNum,@DayNum)"
                sqlcmd = New SqlCommand(strSQLInsert)
                sqlcmd.Parameters.Add(New SqlParameter("@DeviceName", SqlDbType.NVarChar, 100))
                sqlcmd.Parameters("@DeviceName").Value = ServerName
                sqlcmd.Parameters.Add(New SqlParameter("@SrvTypeID", SqlDbType.Int))
                sqlcmd.Parameters("@SrvTypeID").Value = drv("ID")
                sqlcmd.Parameters.Add(New SqlParameter("@Date", SqlDbType.DateTime))
                sqlcmd.Parameters("@Date").Value = SearchDate
                sqlcmd.Parameters.Add(New SqlParameter("@StatName", SqlDbType.NVarChar, 50))
                sqlcmd.Parameters("@StatName").Value = "Disk." & DriveName
                sqlcmd.Parameters.Add(New SqlParameter("@StatValue", SqlDbType.Float))
                sqlcmd.Parameters("@StatValue").Value = MyNumberString
                sqlcmd.Parameters.Add(New SqlParameter("@WeekNum", SqlDbType.Int))
                sqlcmd.Parameters("@WeekNum").Value = MyWeekNumber
                sqlcmd.Parameters.Add(New SqlParameter("@MonthNum", SqlDbType.Int))
                sqlcmd.Parameters("@MonthNum").Value = SearchDate.Month
                sqlcmd.Parameters.Add(New SqlParameter("@YearNum", SqlDbType.Int))
                sqlcmd.Parameters("@YearNum").Value = SearchDate.Year
                sqlcmd.Parameters.Add(New SqlParameter("@DayNum", SqlDbType.Int))
                sqlcmd.Parameters("@DayNum").Value = SearchDate.Day

                WriteAuditEntry(Now.ToString & "  SQL statements " & vbCrLf & queryToLog & vbCrLf & vbCrLf)

            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Exception creating SQL: " & ex.ToString)
            End Try

            Try
                '11/20/2015 NS modified for VSPLUS-2383
                'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQLInsert)
                objVSAdaptor.ExecuteNonQuerySQLParams("VSS_Statistics", sqlcmd)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Availability Stats table insert failed because: " & ex.Message)
                WriteAuditEntry(Now.ToString & " The failed stats table insert command was " & strSQL)
            End Try
        Next

        WriteAuditEntry(Now.ToString & " Finished processing drives for all Microsoft servers.")
        Thread.Sleep(250)
    End Sub


#End Region

#End Region

#Region "Week Number"

    Private Function GetWeekNumber(ByVal dt As DateTime) As Integer
        Dim year As Integer = dt.Year
        Dim Dec29 As New DateTime(year, 12, 29)
        Dim week1 As New DateTime
        ' Check that the date is or is after December 29.

        ' Check that the date is or is after December 29.
        If (dt >= New DateTime(year, 12, 29)) Then
            week1 = GetWeekOneDate(year + 1)
            If (dt < week1) Then week1 = GetWeekOneDate(year)
        Else
            week1 = GetWeekOneDate(year)
            If (dt < week1) Then week1 = GetWeekOneDate(--year)
        End If

        Return ((dt.Subtract(week1).Days / 7 + 1))
    End Function


    Private Function GetWeekOneDate(ByVal Year As Integer) As DateTime
        'start with the 4th of Jan for current year
        Dim MyDate As New DateTime(Year, 1, 4)
        ' Get the ISO-8601 day number for this date 1==Monday, 7==Sunday
        Dim DayNum As Integer = MyDate.DayOfWeek  '0=Sunday, 6=Sat
        If DayNum = 0 Then
            DayNum = 7
        End If

        ' Return the date of the Monday that is less than or equal to this date
        Return MyDate.AddDays(1 - DayNum)

    End Function

#End Region


#Region "Log Files"

    Private Overloads Sub WriteAuditEntry(ByVal strMsg As String, Optional ByVal LogLevel As LogLevel = LogLevel.Normal)
        MyBase.WriteAuditEntry(strMsg, "Daily_Tasks_Log.txt", LogLevel)
    End Sub

    'Private Sub WriteAuditEntry(ByVal strMsg As String, Optional ByVal LogLevel As LogLevel = LogLevel.Normal)
    '	'Only write entries tagged as verbose if the global value of log level is verbose

    '	Try

    '		Dim sw As StreamWriter

    '		Try
    '			If File.Exists(strLogDest) Then
    '				sw = New StreamWriter(strLogDest, True, System.Text.Encoding.Unicode)
    '			Else
    '				sw = New StreamWriter(strLogDest, False, System.Text.Encoding.Unicode)
    '				'  sw = File.CreateText(strLogDest)
    '			End If
    '		Catch ex As Exception

    '		End Try


    '		Try
    '			If strMsg <> "" Then
    '				sw.WriteLine(strMsg)
    '				sw.Flush()
    '				sw.Close()
    '			End If

    '		Catch ex As Exception

    '		End Try

    '		Try
    '			sw.Close()
    '		Catch ex As Exception

    '		End Try
    '		sw = Nothing

    '	Catch ex As Exception

    '	End Try


    'End Sub

#End Region


    Public Sub UpdateLocalTables()
        'Updates local tables with values on RPR's servers
        Dim timeToUpdate As String = "true1"
        Dim sql As String = "select case when DATEADD(day,7,convert(datetime, Svalue, 120)) < getdate() then 'true' else 'false' end as UpdateTables from Settings where Sname = 'LastTableUpdate'"

        Dim ds As New DataSet
        Try
            objVSAdaptor.FillDatasetAny("VitalSigns", "VitalSigns", sql, ds, "Settings")
            timeToUpdate = ds.Tables("Settings").Rows(0)(0).ToString()
            WriteAuditEntry(Now.ToString & " " & timeToUpdate)
        Catch ex As Exception
            timeToUpdate = "true"
        End Try

        If (timeToUpdate.ToLower() = "false") Then
            WriteAuditEntry(Now.ToString & " It is not time to update the local tables.")
            Return
        End If

        WriteAuditEntry(Now.ToString & " Updating local tables with new values from RPR's servers")

        Dim Web As New System.Net.WebClient()
        Dim response As String = ""
        Dim url As String
        Dim counter As Integer = 0


        url = "http://jnitinc.com/WebService/UpdateTables.php?newTable=true"
        response = Web.DownloadString(url)

        Dim root As New Root()
        Dim ms As New MemoryStream(Encoding.Unicode.GetBytes(response))
        Dim serializer As New System.Runtime.Serialization.Json.DataContractJsonSerializer(root.GetType())
        root = DirectCast(serializer.ReadObject(ms), Root)
        ms.Close()
        ms.Dispose()

        'Location Update
        If (root.Location.Length > 0) Then

            Dim dt As New DataTable()
            dt.Columns.Add("Country")
            dt.Columns.Add("State")
            counter = 0
            For Each item As Location In root.Location
                Dim row As DataRow = dt.NewRow
                row("Country") = item.Country.ToString()
                row("State") = item.State.ToString()
                dt.Rows.Add(row)
            Next

            'WriteAuditEntry(Now.ToString & " SQL FOR LOCATIONS: " & sql.Substring(0, sql.Length - 1))
            Try
                objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "", "DELETE FROM ValidLocations")
                Dim con As SqlConnection = objVSAdaptor.StartConnectionSQL("VitalSigns")

                Dim blk As New SqlBulkCopy(con)
                blk.DestinationTableName = "ValidLocations"
                blk.WriteToServer(dt)

                blk.Close()
                objVSAdaptor.StopConnectionSQL(con)


                'objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "", sql.Substring(0, sql.Length - 1))
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error executing SQL command " & ex.ToString)
            End Try

        End If


        If (root.Device.Length > 0) Then
            Dim dt As New DataTable()
            dt.Columns.Add("DeviceType")
            dt.Columns.Add("TranslatedValue")
            dt.Columns.Add("OSName")
            For Each item As Device In root.Device
                Dim row As DataRow = dt.NewRow
                row("DeviceType") = item.DeviceType.ToString()
                row("TranslatedValue") = item.TranslatedValue.ToString()
                row("OSName") = item.OSName.ToString()
                dt.Rows.Add(row)
            Next

            'WriteAuditEntry(Now.ToString & " SQL FOR DEVICES: " & sql.Substring(0, sql.Length - 1))
            Try
                objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "", "DELETE FROM DeviceTypeTranslation")

                Dim con As SqlConnection = objVSAdaptor.StartConnectionSQL("VitalSigns")

                Dim blk As New SqlBulkCopy(con)
                blk.DestinationTableName = "DeviceTypeTranslation"
                blk.WriteToServer(dt)

                blk.Close()
                objVSAdaptor.StopConnectionSQL(con)

            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error executing SQL command " & ex.ToString)
            End Try

        End If


        If (root.OS.Length > 0) Then

            Dim dt As New DataTable()
            dt.Columns.Add("OSType")
            dt.Columns.Add("TranslatedValue")
            dt.Columns.Add("OSName")
            For Each item As OS In root.OS
                Dim row As DataRow = dt.NewRow
                row("OSType") = item.OSType.ToString()
                row("TranslatedValue") = item.TranslatedValue.ToString()
                row("OSName") = item.OSName.ToString()
                dt.Rows.Add(row)
            Next
            'WriteAuditEntry(Now.ToString & " SQL FOR OS: " & sql.Substring(0, sql.Length - 1))
            Try
                objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "", "DELETE FROM OSTypeTranslation")
                Dim con As SqlConnection = objVSAdaptor.StartConnectionSQL("VitalSigns")

                Dim blk As New SqlBulkCopy(con)
                blk.DestinationTableName = "OSTypeTranslation"
                blk.WriteToServer(dt)

                blk.Close()
                objVSAdaptor.StopConnectionSQL(con)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Error executing SQL command " & ex.ToString)
            End Try
        End If



        sql = "IF NOT EXISTS(SELECT * FROM Settings WHERE Sname = 'LastTableUpdate') "
        sql += " INSERT INTO Settings (SName, SValue, SType) VALUES ('LastTableUpdate', getDate(),'System.String') "
        sql += " ELSE UPDATE Settings SET SValue=getDate() WHERE SName='LastTableUpdate'"
        Try
            objVSAdaptor.ExecuteNonQueryAny("VitalSigns", "", sql)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error executing SQL command " & ex.ToString)
        End Try


    End Sub

    Private Sub ConsolidateExchangeDatabases(ByVal curDate As Date, Optional ByVal SearchDateStr As String = "")
        '11/20/2015 NS added for VSPLUS-2383
        Dim sqlcmd As New SqlCommand
        Dim myDataSet As New DataSet()
        '11/20/2015 NS modified for VSPLUS-2383
        'Dim strSQL As String = "SELECT ServerName, StatName, AVG(StatValue) StatValue FROM MicrosoftDailyStats where StatName like 'ExDatabaseSizeMb.%' and DateAdd(day, DateDiff(day, 0, Date),0) = DateAdd(day, DateDiff(day, 0, CONVERT(Datetime, '" & curDate & "', 120)),0) GROUP BY StatName, ServerName"
        Dim strSQL As String

        strSQL = "SELECT ServerName, StatName, AVG(StatValue) StatValue FROM MicrosoftDailyStats where StatName like 'ExDatabaseSizeMb.%' " &
         "and DateAdd(day, DateDiff(day, 0, Date),0) = DateAdd(day, DateDiff(day, 0, " & SearchDateStr & "),0) " &
         "GROUP BY StatName, ServerName"

        WriteAuditEntry(vbCrLf & strSQL & vbCrLf)

        Try
            'myAverageResponse = myStatisticsCommand.ExecuteScalar
            objVSAdaptor.FillDatasetAny("VSS_Statistics", "vitalsigns", strSQL, myDataSet, "MyTable")
            'the number will come as 99.5, so we need to divide by 100 because the WriteAvailability sub multiplies by 100
            ' WriteAuditEntry(Now.ToString & " Filled the dataset ")
            '  WriteAuditEntry(Now.ToString & " The dataset has " & myDataSet.Tables("MyTable").Rows.Count)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Exchange DB collection " & ex.Message & "-- The failed command was " & strSQL)
        End Try

        Dim myView As New Data.DataView(myDataSet.Tables("MyTable"))
        Dim drv As DataRowView

        Dim strSQLInsert As String = ""
        Dim ServerName, StatName, StatValue As String
        Dim MyWeekNumber As Integer
        Dim queryToLog As String = String.Empty

        For Each drv In myView
            Try
                'ServerName, Date, StatName, StatValue,(REPLACE(StatName, 'ExDatabaseSizeMb.', '')) DBName, WeekNumber, MonthNumber, YearNumber, DayNumber
                ServerName = drv("ServerName")
                StatName = drv("StatName")
                StatValue = drv("StatValue")
                MyWeekNumber = GetWeekNumber(Date.Now)
                '11/20/2015 NS modified for VSPLUS-2383
                'strSQLInsert = "INSERT INTO MicrosoftSummaryStats (ServerName, ServerTypeId, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber) VALUES " & vbCrLf
                'strSQLInsert += " ('" & ServerName & "', '5', " & objVSAdaptor.DateFormat(FixDate(curDate)) & ", '" & StatName & "', '" & StatValue & "', '" & MyWeekNumber & "', '" & curDate.Month & "', '" & curDate.Year & "', '" & curDate.Day & "')"
                strSQLInsert = "INSERT INTO MicrosoftSummaryStats (ServerName, ServerTypeId, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber) " & vbCrLf
                queryToLog = strSQLInsert & String.Format(" VALUES ('{0}','{1}','{2}','{3},'{4}','{5}','{6}','{7}',{8})", ServerName, 5, curDate, StatName, StatValue, MyWeekNumber, curDate.Month, curDate.Year, curDate.Day)
                strSQLInsert = strSQLInsert & " VALUES (@DeviceName,@SrvTypeID,@Date,@StatName,@StatValue,@WeekNum,@MonthNum,@YearNum,@DayNum)"
                sqlcmd = New SqlCommand(strSQLInsert)
                sqlcmd.Parameters.Add(New SqlParameter("@DeviceName", SqlDbType.NVarChar, 100))
                sqlcmd.Parameters("@DeviceName").Value = ServerName
                sqlcmd.Parameters.Add(New SqlParameter("@SrvTypeID", SqlDbType.Int))
                sqlcmd.Parameters("@SrvTypeID").Value = 5
                sqlcmd.Parameters.Add(New SqlParameter("@Date", SqlDbType.DateTime))
                sqlcmd.Parameters("@Date").Value = curDate
                sqlcmd.Parameters.Add(New SqlParameter("@StatName", SqlDbType.NVarChar, 50))
                sqlcmd.Parameters("@StatName").Value = StatName
                sqlcmd.Parameters.Add(New SqlParameter("@StatValue", SqlDbType.Float))
                sqlcmd.Parameters("@StatValue").Value = StatValue
                sqlcmd.Parameters.Add(New SqlParameter("@WeekNum", SqlDbType.Int))
                sqlcmd.Parameters("@WeekNum").Value = MyWeekNumber
                sqlcmd.Parameters.Add(New SqlParameter("@MonthNum", SqlDbType.Int))
                sqlcmd.Parameters("@MonthNum").Value = curDate.Month
                sqlcmd.Parameters.Add(New SqlParameter("@YearNum", SqlDbType.Int))
                sqlcmd.Parameters("@YearNum").Value = curDate.Year
                sqlcmd.Parameters.Add(New SqlParameter("@DayNum", SqlDbType.Int))
                sqlcmd.Parameters("@DayNum").Value = curDate.Day

                WriteAuditEntry(Now.ToString & " SQL statement is " & vbCrLf & queryToLog & vbCrLf & vbCrLf)

            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Exception creating SQL: " & ex.ToString)
            End Try

            Try
                '11/20/2015 NS modified for VSPLUS-2383
                'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQLInsert)
                objVSAdaptor.ExecuteNonQuerySQLParams("VSS_Statistics", sqlcmd)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Availability Stats table insert failed because: " & ex.Message)
                WriteAuditEntry(Now.ToString & " The failed stats table insert command was " & strSQL)
            End Try
        Next




    End Sub

    Private Sub ConsolidateExchangeMailboxData(ByVal curDate As Date, Optional ByVal SearchDateStr As String = "")

        Dim sqlcmd As New SqlCommand
        Dim myDataSet As New DataSet()

        'Dim strSQL As String = "SELECT ServerName, StatName, AVG(StatValue) StatValue FROM MicrosoftDailyStats where StatName like 'ExDatabaseSizeMb.%' and DateAdd(day, DateDiff(day, 0, Date),0) = DateAdd(day, DateDiff(day, 0, CONVERT(Datetime, '" & curDate & "', 120)),0) GROUP BY StatName, ServerName"
        Dim strSQL As String

        strSQL = "SELECT ServerName, StatName, SUM(StatValue) StatValue FROM MicrosoftDailyStats where StatName like 'Mailbox.%.%.%' " &
         "and DateAdd(day, DateDiff(day, 0, Date),0) = DateAdd(day, DateDiff(day, 0, " & SearchDateStr & "),0) " &
         "GROUP BY StatName, ServerName"

        WriteAuditEntry(vbCrLf & strSQL & vbCrLf)

        Try
            'myAverageResponse = myStatisticsCommand.ExecuteScalar
            objVSAdaptor.FillDatasetAny("VSS_Statistics", "vitalsigns", strSQL, myDataSet, "MyTable")
            'the number will come as 99.5, so we need to divide by 100 because the WriteAvailability sub multiplies by 100
            ' WriteAuditEntry(Now.ToString & " Filled the dataset ")
            '  WriteAuditEntry(Now.ToString & " The dataset has " & myDataSet.Tables("MyTable").Rows.Count)
        Catch ex As Exception
            WriteAuditEntry(Now.ToString & " Error creating Exchange Mail Files collection " & ex.Message & "-- The failed command was " & strSQL)
        End Try

        Dim myView As New Data.DataView(myDataSet.Tables("MyTable"))
        Dim drv As DataRowView

        Dim strSQLInsert As String = ""
        Dim ServerName, StatName, StatValue As String
        Dim MyWeekNumber As Integer
        Dim queryToLog As String = String.Empty
        For Each drv In myView
            Try
                'ServerName, Date, StatName, StatValue,(REPLACE(StatName, 'ExDatabaseSizeMb.', '')) DBName, WeekNumber, MonthNumber, YearNumber, DayNumber
                ServerName = drv("ServerName")
                StatName = drv("StatName")
                StatValue = drv("StatValue")
                MyWeekNumber = GetWeekNumber(Date.Now)
                '11/20/2015 NS modified for VSPLUS-2383
                'strSQLInsert = "INSERT INTO MicrosoftSummaryStats (ServerName, ServerTypeId, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber) VALUES " & vbCrLf
                'strSQLInsert += " ('" & ServerName & "', '5', " & objVSAdaptor.DateFormat(FixDate(curDate)) & ", '" & StatName & "', '" & StatValue & "', '" & MyWeekNumber & "', '" & curDate.Month & "', '" & curDate.Year & "', '" & curDate.Day & "')"
                strSQLInsert = "INSERT INTO MicrosoftSummaryStats (ServerName, ServerTypeId, [Date], StatName, StatValue , WeekNumber, MonthNumber, YearNumber, DayNumber) " & vbCrLf
                queryToLog = strSQLInsert & String.Format(" VALUES ('{0}','{1}','{2}','{3},'{4}','{5}','{6}','{7}',{8})", ServerName, 5, curDate, StatName, StatValue, MyWeekNumber, curDate.Month, curDate.Year, curDate.Day)
                strSQLInsert = strSQLInsert & " VALUES (@DeviceName,@SrvTypeID,@Date,@StatName,@StatValue,@WeekNum,@MonthNum,@YearNum,@DayNum)"
                sqlcmd = New SqlCommand(strSQLInsert)
                sqlcmd.Parameters.Add(New SqlParameter("@DeviceName", SqlDbType.NVarChar, 100))
                sqlcmd.Parameters("@DeviceName").Value = ServerName
                sqlcmd.Parameters.Add(New SqlParameter("@SrvTypeID", SqlDbType.Int))
                sqlcmd.Parameters("@SrvTypeID").Value = 5
                sqlcmd.Parameters.Add(New SqlParameter("@Date", SqlDbType.DateTime))
                sqlcmd.Parameters("@Date").Value = curDate
                sqlcmd.Parameters.Add(New SqlParameter("@StatName", SqlDbType.NVarChar, 50))
                sqlcmd.Parameters("@StatName").Value = StatName
                sqlcmd.Parameters.Add(New SqlParameter("@StatValue", SqlDbType.Float))
                sqlcmd.Parameters("@StatValue").Value = StatValue
                sqlcmd.Parameters.Add(New SqlParameter("@WeekNum", SqlDbType.Int))
                sqlcmd.Parameters("@WeekNum").Value = MyWeekNumber
                sqlcmd.Parameters.Add(New SqlParameter("@MonthNum", SqlDbType.Int))
                sqlcmd.Parameters("@MonthNum").Value = curDate.Month
                sqlcmd.Parameters.Add(New SqlParameter("@YearNum", SqlDbType.Int))
                sqlcmd.Parameters("@YearNum").Value = curDate.Year
                sqlcmd.Parameters.Add(New SqlParameter("@DayNum", SqlDbType.Int))
                sqlcmd.Parameters("@DayNum").Value = curDate.Day

                WriteAuditEntry(Now.ToString & " SQL statement is " & vbCrLf & queryToLog & vbCrLf & vbCrLf)

            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Exception creating SQL: " & ex.ToString)
            End Try

            Try
                'objVSAdaptor.ExecuteNonQueryAny("VSS_Statistics", "statistics", strSQLInsert)
                objVSAdaptor.ExecuteNonQuerySQLParams("VSS_Statistics", sqlcmd)
            Catch ex As Exception
                WriteAuditEntry(Now.ToString & " Availability Stats table insert failed because: " & ex.Message)
                WriteAuditEntry(Now.ToString & " The failed stats table insert command was " & strSQL)
            End Try
        Next




    End Sub
    'Kiran Dadireddy VSPLUS-2684
    Private Sub ShrinkDBLogOnWeeklyBasis()
        WriteAuditEntry(Now.ToString & " Starting Shrinking VitalSigns Log ")
        ShrinkLog("VitalSigns")

        WriteAuditEntry(Now.ToString & " Starting Shrinking VSS_Statistics Log ")
        ShrinkLog("VSS_Statistics")
    End Sub
    'Kiran Dadireddy VSPLUS-2684
    Private Sub ShrinkLog(ByVal connection As String)
        Try

            Dim myAdapter As New VSFramework.XMLOperation
			Using con As New System.Data.SqlClient.SqlConnection(myAdapter.GetDBConnectionString(connection))
				Try
				con.Open()
				Dim command As New SqlCommand("DBCC SHRINKFILE(" + connection + "_Log,10)", con)
					command.ExecuteNonQuery()
				Catch ex As Exception
				Finally
					If con.State = ConnectionState.Open Then
						con.Close()
					End If
				End Try
			End Using

            WriteAuditEntry(Now.ToString & " Completed shrinking " + connection + " Log ")
        Catch ex As Exception

			WriteAuditEntry(Now.ToString & " Exception while Shrinking " + connection + " Log . \n Exception :" + ex.Message)
		End Try
    End Sub

End Class


Public Class OS
    Public Property OSType As String
    Public Property TranslatedValue As String
    Public Property OSName As String
End Class

Public Class Device
    Public Property DeviceType As String
    Public Property TranslatedValue As String
    Public Property OSName As String
End Class

Public Class Location
    Public Property Country As String
    Public Property State As String
End Class

Public Class Root
    Public Property OS As OS()
    Public Property Device As Device()
    Public Property Location As Location()
End Class

