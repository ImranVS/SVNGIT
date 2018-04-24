Imports System.Runtime.InteropServices

Public Class Form1
    Inherits System.Windows.Forms.Form
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents txtServer As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Statistic As System.Windows.Forms.Label
    Friend WithEvents btnGetAstat As System.Windows.Forms.Button
    Friend WithEvents btnGetAllStats As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtFacility As System.Windows.Forms.TextBox
    Friend WithEvents txtStatistic As System.Windows.Forms.TextBox
    Friend WithEvents txtResults As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnGetAstat = New System.Windows.Forms.Button
        Me.txtServer = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Statistic = New System.Windows.Forms.Label
        Me.txtFacility = New System.Windows.Forms.TextBox
        Me.txtStatistic = New System.Windows.Forms.TextBox
        Me.btnGetAllStats = New System.Windows.Forms.Button
        Me.txtResults = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnGetAstat
        '
        Me.btnGetAstat.Location = New System.Drawing.Point(286, 93)
        Me.btnGetAstat.Name = "btnGetAstat"
        Me.btnGetAstat.Size = New System.Drawing.Size(230, 23)
        Me.btnGetAstat.TabIndex = 0
        Me.btnGetAstat.Text = "Get Individual Statistic"
        '
        'txtServer
        '
        Me.txtServer.Location = New System.Drawing.Point(93, 21)
        Me.txtServer.Name = "txtServer"
        Me.txtServer.Size = New System.Drawing.Size(169, 20)
        Me.txtServer.TabIndex = 1
        Me.txtServer.Text = "azphxdom3/RPRWyatt"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(26, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 14)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Server:"
        '
        'Statistic
        '
        Me.Statistic.Location = New System.Drawing.Point(27, 59)
        Me.Statistic.Name = "Statistic"
        Me.Statistic.Size = New System.Drawing.Size(56, 14)
        Me.Statistic.TabIndex = 4
        Me.Statistic.Text = "Facility"
        '
        'txtFacility
        '
        Me.txtFacility.Location = New System.Drawing.Point(94, 55)
        Me.txtFacility.Name = "txtFacility"
        Me.txtFacility.Size = New System.Drawing.Size(170, 20)
        Me.txtFacility.TabIndex = 3
        Me.txtFacility.Text = "Server.Version.OS"
        '
        'txtStatistic
        '
        Me.txtStatistic.Location = New System.Drawing.Point(91, 93)
        Me.txtStatistic.Name = "txtStatistic"
        Me.txtStatistic.Size = New System.Drawing.Size(174, 20)
        Me.txtStatistic.TabIndex = 5
        Me.txtStatistic.Text = ""
        '
        'btnGetAllStats
        '
        Me.btnGetAllStats.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGetAllStats.Location = New System.Drawing.Point(283, 52)
        Me.btnGetAllStats.Name = "btnGetAllStats"
        Me.btnGetAllStats.Size = New System.Drawing.Size(235, 23)
        Me.btnGetAllStats.TabIndex = 6
        Me.btnGetAllStats.Text = "Get All Statistics"
        '
        'txtResults
        '
        Me.txtResults.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtResults.Location = New System.Drawing.Point(18, 175)
        Me.txtResults.Multiline = True
        Me.txtResults.Name = "txtResults"
        Me.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtResults.Size = New System.Drawing.Size(763, 226)
        Me.txtResults.TabIndex = 7
        Me.txtResults.Text = ""
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(27, 97)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 14)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Statistic"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(28, 147)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 14)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Results"
        '
        'Button1
        '
        Me.Button1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(285, 20)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(235, 23)
        Me.Button1.TabIndex = 10
        Me.Button1.Text = "Get Server Response Time"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(791, 412)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtResults)
        Me.Controls.Add(Me.btnGetAllStats)
        Me.Controls.Add(Me.txtStatistic)
        Me.Controls.Add(Me.Statistic)
        Me.Controls.Add(Me.txtFacility)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtServer)
        Me.Controls.Add(Me.btnGetAstat)
        Me.Name = "Form1"
        Me.Text = "Notes API Example"
        Me.ResumeLayout(False)

    End Sub

#End Region

    '********************************************************
    '*** change these value to match those on your system ***
    '********************************************************
    Dim mNotesProgDir As String = "C:\Program Files (x86)\lotus\notes"
    Dim mNotesINI As String = "=C:\Program Files (x86)\lotus\notes\notes.ini"             ' *** Must start with "=" ***
    Dim mUserId As String = "C:\Program Files (x86)\lotus\notes\data\aforbesrpr.id"
    Dim mPassword As String = "vitalsigns"
    '********************************************************

    ' Get latency from Domino server
    Function GetResponseTime(ByVal sServer$) As String
        Dim sStartStatus As String
        GetResponseTime = "*ERROR* Unknown error"                        ' assume failure

        Try
            If InitNotes(sStartStatus) Then                             ' if Notes initialised ok then
                Dim ptr As IntPtr = dll.ResponseTime(sServer$)          ' get server latency
                GetResponseTime = Marshal.PtrToStringUni(ptr)                    ' Get string returned from dll and marshall into .NET memory space.
                Marshal.FreeBSTR(ptr)                                       ' Free memeory allocated in dll
            Else                                                        ' Otherwise
                GetResponseTime = "*ERROR*" + sStartStatus                       ' return error
            End If

        Catch e As Exception
            GetResponseTime = "*ERROR*" + e.ToString()      ' return error string to caller
        Finally
            dll.W32_NotesTerm()                         ' always terminate Notes session
        End Try

    End Function


    ' Get Statistics from Domino Server.
    ' If sStat$ is blank then all stats are returned for the specified Facility
    Function GetStats(ByVal sServer$, ByVal sFacility$, ByVal sStat$) As String
        Dim sStartStatus As String
        GetStats = "*ERROR*Unknown error"                                       ' assume failure

        Try
            If InitNotes(sStartStatus) Then                                         ' if Notes initialised ok then
                Dim ptr As IntPtr = dll.GetServerStat(sServer$, sFacility$, sStat$)     ' Get stats from server
                GetStats = Marshal.PtrToStringUni(ptr)                                  ' Get string returned from dll and marshall into .NET memory space.
                Marshal.FreeBSTR(ptr)                                                   ' Free memeory allocated in dll
            Else
                GetStats = "*ERROR*" + sStartStatus
            End If

        Catch e As Exception
            GetStats = "*ERROR*" + e.ToString()                         ' return error string to caller
        Finally
            dll.W32_NotesTerm()                                         ' always terminate Notes session
        End Try

    End Function

    ' Start Notes runtime environment
    Function InitNotes(ByRef sStartStatus As String) As Boolean
        InitNotes = False               ' assume failure

        ' Initialise Notes and get status
        ' The InitNotes function calls the C API function NotesInitExtended.
        ' This function can optionally take two parameters, ProgDir and Notes.ini
        ' Whether the function works without these values depends on:
        ' A) which dir the program is running in
        ' B) whether or not the Notes program file directory is in the Windows PATH environment variable
        '
        Dim ptrStart As IntPtr = dll.InitNotes(mNotesProgDir, mNotesINI, mUserId, mPassword)    ' Call dll routine to init Notes environment
        sStartStatus = Marshal.PtrToStringUni(ptrStart)                           ' Get string returned from dll and marshall into .NET memory space.
        Marshal.FreeBSTR(ptrStart)                                                              ' Free memory allocated in dll

        If (sStartStatus = "*SUCCESS*") Then                                                    ' If Notes started ok then 
            InitNotes = True                                                                        ' set return value to True
        End If

    End Function

    ' Get specific stat for a facility
    Private Sub GetOneStat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetAstat.Click
        Dim ret$, stat$
        ret$ = GetStats(txtServer.Text, txtFacility.Text, txtStatistic.Text)
        FormatResults(ret$)
    End Sub

    ' Get all stats for a facility
    Private Sub GetAllStats_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetAllStats.Click
        Dim ret$, stat$
        ret$ = GetStats(txtServer.Text, txtFacility.Text, "")
        FormatResults(ret$)
    End Sub

    Private Sub GetResponseTime_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        txtResults.Text = GetResponseTime(txtServer.Text)
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Set default values
        txtServer.Text = "AZPhxDom1"
        txtFacility.Text = "Server"
        txtStatistic.Text = "Name"

    End Sub

    ' Format the returned results string for display on form
    ' Each result string return from domino seperates label and value with a tab
    '  and ends the line with a linefeed char.
    Private Sub FormatResults(ByVal pOrigResults As String)

        ' Replace tab chars with "="
        Dim sFormattedResults$ = pOrigResults.Replace(ControlChars.Tab, "=")

        ' Replace Linefeed char with Newline
        sFormattedResults$ = sFormattedResults$.Replace(ControlChars.Lf, ControlChars.NewLine)

        ' Display formatted results in the dialog
        txtResults.Text = sFormattedResults

    End Sub

End Class

' Contains dll functions.
' These could be declared at the top of the code (outside of a class) but this seems to be the preferred way for VB.NET
Class dll
    Declare Function InitNotes Lib "ServerStats.dll" Alias "InitNotes" (ByVal s1 As String, ByVal s2 As String, ByVal s3 As String, ByVal s4 As String) As IntPtr
    Declare Function GetServerStat Lib "ServerStats.dll" Alias "GetServerStat" (ByVal s1 As String, ByVal s2 As String, ByVal s3 As String) As IntPtr
    Declare Function ResponseTime Lib "ServerStats.dll" Alias "ResponseTime" (ByVal s1 As String) As IntPtr
    Declare Sub W32_NotesTerm Lib "nnotes" Alias "NotesTerm" ()
End Class