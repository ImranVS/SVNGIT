Public Class IconControl
    'copyright 2010, Alan Forbes
    'mzldad@yahoo.com
    '781-608-4060


    Inherits System.Windows.Forms.UserControl

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'UserControl1 overrides dispose to clean up the component list.
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
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IconControl))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        Me.ImageList1.Images.SetKeyName(3, "")
        Me.ImageList1.Images.SetKeyName(4, "")
        Me.ImageList1.Images.SetKeyName(5, "")
        Me.ImageList1.Images.SetKeyName(6, "")
        Me.ImageList1.Images.SetKeyName(7, "")
        Me.ImageList1.Images.SetKeyName(8, "")
        Me.ImageList1.Images.SetKeyName(9, "")
        Me.ImageList1.Images.SetKeyName(10, "")
        Me.ImageList1.Images.SetKeyName(11, "quickr.gif")
        Me.ImageList1.Images.SetKeyName(12, "exchange.jpg")
        Me.ImageList1.Images.SetKeyName(13, "share.jpg")
        Me.ImageList1.Images.SetKeyName(14, "windows.jpg")
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(1, 7)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(17, 17)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'IconControl
        '
        Me.Controls.Add(Me.PictureBox1)
        Me.Name = "IconControl"
        Me.Size = New System.Drawing.Size(20, 30)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Dim mImageCount As Integer

    Enum IconList As Integer
        NoIcon = 0
        DominoServer = 1
        Network_Device = 2
        URL = 3
        NotesMail_Probe = 4
        BlackBerry_Probe = 5
        LDAP = 6
        Mail_Service = 7
        NotesDB = 8
        BESQ = 9
        Sametime = 10
        Quickr = 11
        Exchange = 12
        Sharepoint = 13
        Windows = 14
    End Enum

    Public WriteOnly Property Icon() As Integer
        Set(ByVal Value As Integer)
            If Value >= Me.ImageList1.Images.Count Then
                Value = IconList.NoIcon
            End If
            Me.PictureBox1.Image = Me.ImageList1.Images(Value)
            '   Me.Label1.Text = Value
            Me.PictureBox1.Invalidate()
            Me.Invalidate()
        End Set
    End Property


End Class
