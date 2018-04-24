Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports LogUtilities
'Updated 11-7-13
Public Class VSAdaptor


	Enum LogLevel
		Verbose = LogUtilities.LogUtils.LogLevel.Verbose
		Debug = LogUtilities.LogUtils.LogLevel.Debug
		Normal = LogUtilities.LogUtils.LogLevel.Normal
	End Enum

    Public Sub New()
		Dim s As String
	End Sub


#Region "CommonSQL"
    Public Function DateFormat(ByVal obj As Object) As Object
            Return "'" + obj + "'"
    End Function
    Public Function ExecuteNonQueryAny(ByVal SQLDBName As String, ByVal MDBName As String, ByVal SQLStatement As String) As Object
		'This function returns the number of rows affected
		Try
				ExecuteNonQueryAny = ExecuteNonQuerySQL(SQLDBName, SQLStatement)
		Catch ex As Exception
			ExecuteNonQueryAny = 0
			WriteLogEntry(DateTime.Now.ToString + " Sql Statement - " + SQLStatement + " - Exception - " + ex.ToString)
		End Try
    End Function

    Public Sub FillDatasetAny(ByVal SQLDBName As String, ByVal MDBName As String, ByVal SQLStatement As String, ByRef dataSet As DataSet, ByVal srcTable As String)
        Try

            FillDatasetSQL(SQLDBName, SQLStatement, dataSet, srcTable)

        Catch ex As Exception
            WriteLogEntry(DateTime.Now.ToString + " Sql Statement - " + SQLStatement + " - Exception - " + ex.ToString)
        End Try
    End Sub


    Public Function ExecuteScalarAny(ByVal SQLDBName As String, ByVal MDBName As String, ByVal SQLStatement As String) As Object
        Try
                Return ExecuteScalarSQL(SQLDBName, SQLStatement)
        Catch ex As Exception
            WriteLogEntry(DateTime.Now.ToString + " Sql Statement - " + SQLStatement + " - Exception - " + ex.ToString)
        End Try
    End Function

#End Region

#Region "SQLFunctions"


	Public Function StartConnectionSQL(ByVal SQLDBName As String) As SqlConnection
		Dim mySqlConnection As New SqlConnection
		Try
			Dim SQLServerName As String = ""
			Dim SQLUserName As String = ""
			Dim SQLPassword As String = ""
			Dim MyPass As Byte()
			Dim SQLIntegratedSecurity As String = "False"
			Dim WorkstationName As String = Environment.MachineName.ToString
			Dim myRegistry As New RegistryHandler
			Dim mySecrets As New TripleDES


			'SQLServerName = myRegistry.ReadFromRegistry("SQL Server")
			'SQLUserName = myRegistry.ReadFromRegistry("SQL User")
			'SQLIntegratedSecurity = myRegistry.ReadFromRegistry("SQL Security")
			'MyPass = myRegistry.ReadFromRegistry("SQL Password")
			'SQLPassword = mySecrets.Decrypt(MyPass)
			Dim myOp As New XMLOperation
			With mySqlConnection
				If .State = ConnectionState.Closed Then
                    '  .ConnectionString = "Data Source=" & SQLServerName & "; Integrated Security=" & SQLIntegratedSecurity & ";Initial Catalog=" & SQLDBName & ";Persist Security Info=False;User ID=" & SQLUserName & ";Password=" & SQLPassword & "; Workstation ID=" & WorkstationName
                    .ConnectionString = "" 'myOp.GetDBConnectionString(SQLDBName)
                    .Open()
				End If
			End With


		Catch ex As Exception
			WriteLogEntry(DateTime.Now.ToString + " Failed Connection to " + SQLDBName)
		Finally

		End Try
		Return mySqlConnection
	End Function

	Public Sub StopConnectionSQL(ByVal mySqlConnection As SqlConnection)
		Try
			If mySqlConnection.State = ConnectionState.Open Then
				mySqlConnection.Close()
			End If
		Catch ex As Exception
			WriteLogEntry(DateTime.Now.ToString + " Failed to close connection")
		Finally

		End Try
	End Sub

    Private Function ExecuteNonQuerySQL(ByVal SQLDBName As String, ByVal SQLStatement As String) As Object
        Dim mySqlConnection As SqlConnection
        Dim myCommand As New SqlCommand
        Dim RowsAffected As Integer
        Try
            mySqlConnection = StartConnectionSQL(SQLDBName)
            myCommand.Connection = mySqlConnection
            myCommand.CommandText = SQL_Server_Compatible_SQLStatement(SQLStatement)
            RowsAffected = myCommand.ExecuteNonQuery
        Catch ex As Exception
            RowsAffected = 0
            WriteLogEntry(DateTime.Now.ToString + " Sql Statement - " + SQLStatement + " - Exception - " + ex.ToString)
        Finally
            myCommand.Dispose()
            StopConnectionSQL(mySqlConnection)
        End Try

        Return RowsAffected
    End Function

    Private Sub FillDatasetSQL(ByVal SQLDBName As String, ByVal SQLStatement As String, ByRef dataSet As DataSet, ByVal srcTable As String)
        Dim myCommand As New SqlCommand
        Dim myAdapter As New SqlDataAdapter
        Dim mySqlConnection As SqlConnection
        Try
            mySqlConnection = StartConnectionSQL(SQLDBName)
            myCommand.Connection = mySqlConnection
            myCommand.CommandText = SQLStatement
            myAdapter.SelectCommand = myCommand
            myAdapter.Fill(dataSet, srcTable)
        Catch ex As Exception
            WriteLogEntry(DateTime.Now.ToString + " Sql Statement - " + SQLStatement + " - Exception - " + ex.ToString)
        Finally
            myCommand.Dispose()
            StopConnectionSQL(mySqlConnection)
        End Try

    End Sub

    Private Function ExecuteScalarSQL(ByVal SQLDBName As String, ByVal SQLStatement As String) As Object
        Dim mySqlConnection As SqlConnection
        Dim intId As Integer
        Dim myCommand As New SqlCommand()
        Try
            mySqlConnection = StartConnectionSQL(SQLDBName)
            myCommand.Connection = mySqlConnection
            myCommand.CommandText = SQL_Server_Compatible_SQLStatement(SQLStatement)
            ExecuteScalarSQL = myCommand.ExecuteScalar()
            ' intId = CInt(myCommand.ExecuteScalar())

        Catch ex As Exception
            ExecuteScalarSQL = Nothing
            WriteLogEntry(DateTime.Now.ToString + " Sql Statement - " + SQLStatement + " - Exception - " + ex.ToString)
        Finally
            myCommand.Dispose()
            StopConnectionSQL(mySqlConnection)
        End Try
        Return ExecuteScalarSQL
    End Function

    Public Function FetchData(ByVal sConnectionString As String, ByVal SQLStatement As String) As DataTable
        Dim myCommand As New SqlCommand
        Dim myAdapter As New SqlDataAdapter
        Dim mySqlConnection As New SqlConnection
        Dim ds As New DataSet
        Dim dt As New DataTable
        Try
            With mySqlConnection
                If .State = ConnectionState.Closed Then
                    .ConnectionString = sConnectionString
                    .Open()
                End If
            End With

            myCommand.Connection = mySqlConnection
            myCommand.CommandText = SQLStatement
            myAdapter.SelectCommand = myCommand
            myAdapter.Fill(ds, "dt")
            dt = ds.Tables(0)
        Catch ex As Exception
            WriteLogEntry(DateTime.Now.ToString + " Sql Statement - " + SQLStatement + " - Exception - " + ex.ToString)
        Finally
            myCommand.Dispose()
            StopConnectionSQL(mySqlConnection)
        End Try
        Return dt
    End Function

    Public Sub InsertData(ByVal sConnectionString As String, ByVal SQLStatement As String)
        Dim myCommand As New SqlCommand
        Dim myAdapter As New SqlDataAdapter
        Dim mySqlConnection As New SqlConnection

        Try
            With mySqlConnection
                If .State = ConnectionState.Closed Then
                    .ConnectionString = sConnectionString
                    .Open()
                End If
            End With
            myCommand.Connection = mySqlConnection
            myCommand.CommandText = SQL_Server_Compatible_SQLStatement(SQLStatement)
            myAdapter.InsertCommand = myCommand
            myAdapter.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            WriteLogEntry(DateTime.Now.ToString + " Sql Statement - " + SQLStatement + " - Exception - " + ex.ToString)
        Finally
            myCommand.Dispose()
            StopConnectionSQL(mySqlConnection)
        End Try
    End Sub

    Public Sub UpdateData(ByVal sConnectionString As String, ByVal SQLStatement As String)
        Dim myCommand As New SqlCommand
        Dim myAdapter As New SqlDataAdapter
        Dim mySqlConnection As New SqlConnection
        Try
            With mySqlConnection
                If .State = ConnectionState.Closed Then
                    .ConnectionString = sConnectionString
                    .Open()
                End If
            End With
            myCommand.Connection = mySqlConnection
            myCommand.CommandText = SQL_Server_Compatible_SQLStatement(SQLStatement)
            myAdapter.UpdateCommand = myCommand

            myAdapter.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            WriteLogEntry(DateTime.Now.ToString + " Sql Statement - " + SQLStatement + " - Exception - " + ex.ToString)
        Finally
            myCommand.Dispose()
            StopConnectionSQL(mySqlConnection)
        End Try
    End Sub


    Public Function GetDataFromProcedure(ByVal sConnectionString As String, ByVal storedprocedure As String, ByVal paramnum As Integer, ByVal parameterName() As String, ByVal parameterValue() As String) As DataTable
        Dim mySqlConnection As New SqlConnection
        Dim myAdapter As New SqlDataAdapter
        Dim myCommand As New SqlCommand
        Dim dt As New DataTable
        Dim parameter As New SqlParameter
        Try


            With mySqlConnection
                If .State = ConnectionState.Closed Then
                    .ConnectionString = sConnectionString
                    .Open()
                End If
            End With
            myCommand.Connection = mySqlConnection
            myCommand.CommandText = storedprocedure
            myCommand.CommandType = CommandType.StoredProcedure

            For i As Integer = 0 To paramnum - 1
                myCommand.Parameters.AddWithValue("@" + parameterName(i), parameterValue(i))
            Next

            myAdapter.SelectCommand = myCommand
            Dim ds As New DataSet
            myAdapter.Fill(ds, "dt")
            dt = ds.Tables(0)
            myCommand.Dispose()


        Catch ex As Exception
        Finally
            StopConnectionSQL(mySqlConnection)
        End Try

        Return dt
    End Function

    '4/7/2015 NS added for VSPLUS-1629
    Public Function ExecuteNonQuerySQLParams(ByVal SQLDBName As String, ByVal SQLCmd As SqlCommand) As Object
        Dim mySqlConnection As SqlConnection
        Dim RowsAffected As Integer
        Try
            mySqlConnection = StartConnectionSQL(SQLDBName)
            SQLCmd.Connection = mySqlConnection
            RowsAffected = SQLCmd.ExecuteNonQuery
        Catch ex As Exception
            RowsAffected = 0
            WriteLogEntry(DateTime.Now.ToString + " Sql Statement - " + SQLCmd.CommandText + " - Exception - " + ex.ToString)
        Finally
            SQLCmd.Dispose()
            StopConnectionSQL(mySqlConnection)
        End Try

        Return RowsAffected
    End Function
#End Region

#Region "AccessFunctions"

	Public Function ExecuteNonQuerySQLParams(ByVal SQLCmd As SqlCommand, mySqlConnection As SqlConnection) As Object
		Dim RowsAffected As Integer
		Try
			SQLCmd.Connection = mySqlConnection
			RowsAffected = SQLCmd.ExecuteNonQuery
		Catch ex As Exception
			RowsAffected = 0
			WriteLogEntry(DateTime.Now.ToString + " Sql Statement - " + SQLCmd.CommandText + " - Exception - " + ex.ToString)
		Finally
			SQLCmd.Dispose()
		End Try

		Return RowsAffected
	End Function
#End Region

#Region "SQL Manipulation"

    Function SQL_Server_Compatible_SQLStatement(ByVal SQLStatement As String) As String
        'This function takes a SQL statement written for Access and returns one that will work in SQL server
        'transforms True to 1 and False to 0
        SQLStatement = SQLStatement.Replace("True", "1")
        SQLStatement = SQLStatement.Replace("False", "0")
        SQLStatement = SQLStatement.Replace("Delete * From ", "Delete From ")
        ' SQLStatement = SQLStatement.Replace("#", "'")
        SQLStatement = SQLStatement.Replace("yesno", "bit")
        SQL_Server_Compatible_SQLStatement = SQLStatement
    End Function
    Function ToBit(ByVal value As Boolean) As Integer
        If value = True Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Function ToBit(ByVal Value As String) As Integer
        If Value.ToUpper = "TRUE" Then
            Return 1
        Else
            Return 0
        End If
    End Function


#End Region

#Region "Write Logs"

    Private Function GetAppPath() As String
        Dim path As String
        path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
        Return path
    End Function

	Public Sub WriteLogEntry(ByVal msg As String, Optional ByVal logLevel As LogLevel = LogLevel.Verbose)
		LogUtilities.LogUtils.WriteAuditEntryForVSAdapter(msg, "VSAdapter_log.txt", logLevel)
	End Sub

	'Public Sub WriteLogEntry(ByVal msg As String)
	'	Dim ThisLogLevel As LogLevel
	'	Dim file As System.IO.StreamWriter
	'	Dim LogPath As String

	'	Try
	'		Dim myRegistry As New RegistryHandler
	'		ThisLogLevel = CType(myRegistry.ReadFromRegistry("Log Level VSAdapter"), LogLevel)
	'	Catch ex As Exception
	'		ThisLogLevel = LogLevel.Verbose
	'	End Try


	'	If ThisLogLevel = LogLevel.Verbose Then
	'		Try
	'			LogPath = GetAppPath() + "\Log_Files\"
	'		Catch ex As Exception
	'			LogPath = "c:\"
	'		End Try

	'		LogPath = LogPath + "VSAdapter_Log.txt"

	'		Try
	'			file = My.Computer.FileSystem.OpenTextFileWriter(LogPath, True)
	'			file.WriteLine(msg)
	'			file.Close()
	'		Catch ex As Exception

	'		End Try
	'	End If



	'End Sub

#End Region



End Class
