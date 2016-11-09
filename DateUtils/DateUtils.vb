Imports VSFramework
Imports System.Data.SqlClient

Public Class DateUtils
#Region "Member Variables"
    Dim mlangid As Integer
    Dim mdateformat As String
    Dim mName As String
#End Region

#Region "Properties"
    Public Property langid() As Integer
        Get
            Return mlangid
        End Get
        Set(ByVal value As Integer)
            mlangid = value
        End Set
    End Property
    Public Property dateformat() As String
        Get
            Return mdateformat
        End Get
        Set(ByVal value As String)
            mdateformat = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal MyName As String)
            mName = MyName
        End Set
    End Property
#End Region

#Region "Functions/Procedures"
    ''' <summary>
    ''' Returns MS-SQL default language date format
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDateFormat() As String
        Dim strdateformat As String = "mdy"
        Return strdateformat
        Dim myAdapter As New VSFramework.XMLOperation

        'CONNECTION STRING TO BE TAKEN FROM VSFRAMEWORK
        Dim connectionString As String = myAdapter.GetDBConnectionString("VitalSigns")
        '"Data Source=SERVER_NAME; Integrated Security=true;Initial Catalog=vitalsigns;"

        Dim strSQL As String = "select * from master..syslanguages where langid = (select value from master..sysconfigures where comment = 'default language')"
        Dim Sqlcon As New SqlConnection(connectionString)
        Dim DA As New SqlDataAdapter(strSQL, Sqlcon)
        Dim Ds As New DataSet
        DA.Fill(Ds, "SysLanguages")
        Dim dt As DataTable = Ds.Tables(0)
        If dt.Rows.Count > 0 Then
            strdateformat = dt.Rows.Item(0).Item("dateformat").ToString()
        End If
        Return strdateformat
    End Function

    ''' <summary>
    ''' Returns Short Date format
    ''' </summary>
    ''' <param name="pDate"></param>
    ''' <param name="pdateformat"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FixDate(ByVal pDate As DateTime, ByVal pdateformat As String) As String
        Dim retDt As String = ""
        If pdateformat = "dmy" Then
            retDt = String.Format("{0:d/M/yyyy}", pDate)
        ElseIf pdateformat = "mdy" Then
            retDt = String.Format("{0:M/d/yyyy}", pDate)
        ElseIf pdateformat = "ymd" Then
            retDt = String.Format("{0:yyyy/M/d}", pDate)
        End If
        retDt = retDt.Replace("mei", "may")
        Return retDt
    End Function


    ''' <summary>
    ''' Returns full date and time format
    ''' </summary>
    ''' <param name="pDate"></param>
    ''' <param name="pdateformat"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FixDateTime(ByVal pDate As DateTime, ByVal pdateformat As String) As String
        Dim retDt As String = ""
        If pdateformat = "dmy" Then
            retDt = String.Format("{0:d/M/yyyy HH:mm:ss}", pDate)
        ElseIf pdateformat = "mdy" Then
            retDt = String.Format("{0:M/d/yyyy HH:mm:ss}", pDate)
        ElseIf pdateformat = "ymd" Then
            retDt = String.Format("{0:yyyy/M/d HH:mm:ss}", pDate)
        End If
        Return retDt
    End Function
#End Region

End Class
