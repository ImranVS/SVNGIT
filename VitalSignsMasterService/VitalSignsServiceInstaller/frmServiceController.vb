Imports System.ServiceProcess
Imports System.Threading
Imports System.IO
Imports Microsoft.Win32
Imports Microsoft.Win32.Registry

Public Class frmServiceController
    Inherits DevComponents.DotNetBar.Office2007Form
    'Inherits System.Windows.Forms.Form
    Public MyProductName As String = "VitalSigns"
    Friend WithEvents LabelX5 As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelBuild As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelStatus As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelX3 As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelServiceStart As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelX2 As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelX1 As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelPath As DevComponents.DotNetBar.LabelX
    Friend WithEvents btnCompact As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents lblMasterBuild As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblMasterStatus As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelX11 As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelX13 As DevComponents.DotNetBar.LabelX
    Friend WithEvents GroupPanel3 As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents GroupPanel2 As DevComponents.DotNetBar.Controls.GroupPanel

    Friend WithEvents pictureBoxMaster As System.Windows.Forms.PictureBox
    Friend WithEvents LabelMaster As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelService As DevComponents.DotNetBar.LabelX
    Friend WithEvents PictureBoxService As System.Windows.Forms.PictureBox
    Friend WithEvents LabelX6 As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblMasterStart As DevComponents.DotNetBar.LabelX
    Friend WithEvents GroupPanel4 As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents LabelX7 As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelDailyStart As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelDaily As DevComponents.DotNetBar.LabelX
    Friend WithEvents PictureBoxDaily As System.Windows.Forms.PictureBox
    Friend WithEvents LabelDailyBuild As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelX12 As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelX14 As DevComponents.DotNetBar.LabelX
    Friend WithEvents LabelDailyStatus As DevComponents.DotNetBar.LabelX
    Public MonitorServiceName As String = "VitalSigns Monitoring Service"
    Public DailyServiceName As String = "VitalSigns Daily Tasks"
    Public MasterServiceName As String = "VitalSigns Master Service"
    'To Do: rename service to VitalSigns Monitoring Agent, and exe to VitalSignsService.exe


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
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Timer1 As System.Timers.Timer
    Friend WithEvents LabelStart As System.Windows.Forms.Label
    Friend WithEvents LabelStop As System.Windows.Forms.Label
    Friend WithEvents LabelBanner As System.Windows.Forms.Label
    Friend WithEvents LabelRemove As System.Windows.Forms.Label
    Friend WithEvents labelInstall As System.Windows.Forms.Label
    Friend WithEvents Installation As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents ButtonRemove As DevComponents.DotNetBar.ButtonX
    Friend WithEvents buttonInstall As DevComponents.DotNetBar.ButtonX
    Friend WithEvents DockSite2 As DevComponents.DotNetBar.DockSite
    Friend WithEvents DockSite1 As DevComponents.DotNetBar.DockSite
    Friend WithEvents DockSite3 As DevComponents.DotNetBar.DockSite
    Friend WithEvents DockSite4 As DevComponents.DotNetBar.DockSite
    Friend WithEvents DockSite5 As DevComponents.DotNetBar.DockSite
    Friend WithEvents DockSite6 As DevComponents.DotNetBar.DockSite
    Friend WithEvents DockSite7 As DevComponents.DotNetBar.DockSite
    Friend WithEvents Bar1 As DevComponents.DotNetBar.Bar
    Friend WithEvents ButtonItem1 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents DockSite8 As DevComponents.DotNetBar.DockSite
    Friend WithEvents DotNetBarManager1 As DevComponents.DotNetBar.DotNetBarManager
    Friend WithEvents ButtonItemExit As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ButtonItem3 As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ButtonItemStart As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ButtonItemStop As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ButtonItemInstall As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents ButtonItemRemove As DevComponents.DotNetBar.ButtonItem
    Friend WithEvents GroupPanel1 As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents ButtonStop As DevComponents.DotNetBar.ButtonX
    Friend WithEvents ButtonStart As DevComponents.DotNetBar.ButtonX
    Friend WithEvents Bar2 As DevComponents.DotNetBar.Bar
    Friend WithEvents StatusText As DevComponents.DotNetBar.LabelItem
    Friend WithEvents BalloonTip1 As DevComponents.DotNetBar.BalloonTip
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmServiceController))
        Me.LabelBanner = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.LabelStart = New System.Windows.Forms.Label()
        Me.LabelStop = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Timers.Timer()
        Me.LabelRemove = New System.Windows.Forms.Label()
        Me.labelInstall = New System.Windows.Forms.Label()
        Me.Installation = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.ButtonRemove = New DevComponents.DotNetBar.ButtonX()
        Me.buttonInstall = New DevComponents.DotNetBar.ButtonX()
        Me.DotNetBarManager1 = New DevComponents.DotNetBar.DotNetBarManager(Me.components)
        Me.DockSite4 = New DevComponents.DotNetBar.DockSite()
        Me.DockSite1 = New DevComponents.DotNetBar.DockSite()
        Me.DockSite2 = New DevComponents.DotNetBar.DockSite()
        Me.DockSite8 = New DevComponents.DotNetBar.DockSite()
        Me.DockSite5 = New DevComponents.DotNetBar.DockSite()
        Me.DockSite6 = New DevComponents.DotNetBar.DockSite()
        Me.DockSite7 = New DevComponents.DotNetBar.DockSite()
        Me.Bar1 = New DevComponents.DotNetBar.Bar()
        Me.ButtonItem1 = New DevComponents.DotNetBar.ButtonItem()
        Me.btnCompact = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItemExit = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItem3 = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItemStart = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItemStop = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItemInstall = New DevComponents.DotNetBar.ButtonItem()
        Me.ButtonItemRemove = New DevComponents.DotNetBar.ButtonItem()
        Me.DockSite3 = New DevComponents.DotNetBar.DockSite()
        Me.BalloonTip1 = New DevComponents.DotNetBar.BalloonTip()
        Me.GroupPanel1 = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.ButtonStop = New DevComponents.DotNetBar.ButtonX()
        Me.ButtonStart = New DevComponents.DotNetBar.ButtonX()
        Me.Bar2 = New DevComponents.DotNetBar.Bar()
        Me.StatusText = New DevComponents.DotNetBar.LabelItem()
        Me.lblMasterBuild = New DevComponents.DotNetBar.LabelX()
        Me.lblMasterStatus = New DevComponents.DotNetBar.LabelX()
        Me.LabelX11 = New DevComponents.DotNetBar.LabelX()
        Me.LabelX13 = New DevComponents.DotNetBar.LabelX()
        Me.LabelBuild = New DevComponents.DotNetBar.LabelX()
        Me.LabelStatus = New DevComponents.DotNetBar.LabelX()
        Me.LabelX3 = New DevComponents.DotNetBar.LabelX()
        Me.LabelServiceStart = New DevComponents.DotNetBar.LabelX()
        Me.LabelX2 = New DevComponents.DotNetBar.LabelX()
        Me.LabelX1 = New DevComponents.DotNetBar.LabelX()
        Me.LabelX5 = New DevComponents.DotNetBar.LabelX()
        Me.LabelPath = New DevComponents.DotNetBar.LabelX()
        Me.GroupPanel2 = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.LabelX6 = New DevComponents.DotNetBar.LabelX()
        Me.lblMasterStart = New DevComponents.DotNetBar.LabelX()
        Me.LabelMaster = New DevComponents.DotNetBar.LabelX()
        Me.pictureBoxMaster = New System.Windows.Forms.PictureBox()
        Me.GroupPanel3 = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.LabelService = New DevComponents.DotNetBar.LabelX()
        Me.PictureBoxService = New System.Windows.Forms.PictureBox()
        Me.GroupPanel4 = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.LabelX7 = New DevComponents.DotNetBar.LabelX()
        Me.LabelDailyStart = New DevComponents.DotNetBar.LabelX()
        Me.LabelDaily = New DevComponents.DotNetBar.LabelX()
        Me.PictureBoxDaily = New System.Windows.Forms.PictureBox()
        Me.LabelDailyBuild = New DevComponents.DotNetBar.LabelX()
        Me.LabelX12 = New DevComponents.DotNetBar.LabelX()
        Me.LabelX14 = New DevComponents.DotNetBar.LabelX()
        Me.LabelDailyStatus = New DevComponents.DotNetBar.LabelX()
        Me.Panel1.SuspendLayout()
        CType(Me.Timer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Installation.SuspendLayout()
        Me.DockSite7.SuspendLayout()
        CType(Me.Bar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupPanel1.SuspendLayout()
        CType(Me.Bar2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupPanel2.SuspendLayout()
        CType(Me.pictureBoxMaster, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupPanel3.SuspendLayout()
        CType(Me.PictureBoxService, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupPanel4.SuspendLayout()
        CType(Me.PictureBoxDaily, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelBanner
        '
        Me.LabelBanner.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelBanner.Location = New System.Drawing.Point(-2, 0)
        Me.LabelBanner.Name = "LabelBanner"
        Me.LabelBanner.Padding = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.LabelBanner.Size = New System.Drawing.Size(648, 55)
        Me.LabelBanner.TabIndex = 1
        Me.LabelBanner.Text = "Running the 'VitalSigns Master Service' as an automatic service is recommended.  " & _
            "All other VitalSigns services should be Manual.  "
        Me.LabelBanner.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.LightYellow
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.LabelBanner)
        Me.Panel1.Location = New System.Drawing.Point(8, 431)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(648, 59)
        Me.Panel1.TabIndex = 6
        '
        'LabelStart
        '
        Me.LabelStart.BackColor = System.Drawing.Color.Transparent
        Me.LabelStart.Location = New System.Drawing.Point(104, 3)
        Me.LabelStart.Name = "LabelStart"
        Me.LabelStart.Size = New System.Drawing.Size(291, 24)
        Me.LabelStart.TabIndex = 10
        Me.LabelStart.Text = "This button starts the VitalSigns services." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'LabelStop
        '
        Me.LabelStop.BackColor = System.Drawing.Color.Transparent
        Me.LabelStop.Location = New System.Drawing.Point(104, 38)
        Me.LabelStop.Name = "LabelStop"
        Me.LabelStop.Size = New System.Drawing.Size(291, 23)
        Me.LabelStop.TabIndex = 12
        Me.LabelStop.Text = "This button stops the VitalSigns  services."
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 5000.0R
        Me.Timer1.SynchronizingObject = Me
        '
        'LabelRemove
        '
        Me.LabelRemove.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelRemove.BackColor = System.Drawing.Color.Transparent
        Me.LabelRemove.Location = New System.Drawing.Point(104, 51)
        Me.LabelRemove.Name = "LabelRemove"
        Me.LabelRemove.Size = New System.Drawing.Size(458, 33)
        Me.LabelRemove.TabIndex = 9
        Me.LabelRemove.Text = "This button removes the VitalSigns service from the directory of Windows Services" & _
            "."
        '
        'labelInstall
        '
        Me.labelInstall.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.labelInstall.BackColor = System.Drawing.Color.Transparent
        Me.labelInstall.ForeColor = System.Drawing.SystemColors.ControlText
        Me.labelInstall.Location = New System.Drawing.Point(104, 10)
        Me.labelInstall.Name = "labelInstall"
        Me.labelInstall.Size = New System.Drawing.Size(452, 32)
        Me.labelInstall.TabIndex = 7
        Me.labelInstall.Text = "This button installs the VitalSigns service, allowing it to run automatically in " & _
            "the background"
        '
        'Installation
        '
        Me.Installation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Installation.CanvasColor = System.Drawing.SystemColors.Control
        Me.Installation.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.Installation.Controls.Add(Me.ButtonRemove)
        Me.Installation.Controls.Add(Me.buttonInstall)
        Me.Installation.Controls.Add(Me.labelInstall)
        Me.Installation.Controls.Add(Me.LabelRemove)
        Me.Installation.Location = New System.Drawing.Point(8, 31)
        Me.Installation.Name = "Installation"
        Me.Installation.Size = New System.Drawing.Size(643, 108)
        '
        '
        '
        Me.Installation.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.Installation.Style.BackColorGradientAngle = 90
        Me.Installation.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.Installation.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.Installation.Style.BorderBottomWidth = 1
        Me.Installation.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.Installation.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.Installation.Style.BorderLeftWidth = 1
        Me.Installation.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.Installation.Style.BorderRightWidth = 1
        Me.Installation.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.Installation.Style.BorderTopWidth = 1
        Me.Installation.Style.Class = ""
        Me.Installation.Style.CornerDiameter = 4
        Me.Installation.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.Installation.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.Installation.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.Installation.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.Installation.StyleMouseDown.Class = ""
        Me.Installation.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.Installation.StyleMouseOver.Class = ""
        Me.Installation.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Installation.TabIndex = 20
        Me.Installation.Text = "Install"
        '
        'ButtonRemove
        '
        Me.ButtonRemove.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ButtonRemove.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.ButtonRemove.Image = Global.ServiceInstaller.My.Resources.Resources.problem
        Me.ButtonRemove.Location = New System.Drawing.Point(11, 51)
        Me.ButtonRemove.Name = "ButtonRemove"
        Me.ButtonRemove.Size = New System.Drawing.Size(75, 23)
        Me.ButtonRemove.TabIndex = 11
        Me.ButtonRemove.Text = "&Remove"
        '
        'buttonInstall
        '
        Me.buttonInstall.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.buttonInstall.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.buttonInstall.Image = Global.ServiceInstaller.My.Resources.Resources.solution
        Me.buttonInstall.Location = New System.Drawing.Point(11, 10)
        Me.buttonInstall.Name = "buttonInstall"
        Me.buttonInstall.Size = New System.Drawing.Size(75, 23)
        Me.buttonInstall.TabIndex = 10
        Me.buttonInstall.Text = "&Install"
        '
        'DotNetBarManager1
        '
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.F1)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlC)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlA)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlV)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlX)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlZ)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlY)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Del)
        Me.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Ins)
        Me.DotNetBarManager1.BottomDockSite = Me.DockSite4
        Me.DotNetBarManager1.EnableFullSizeDock = False
        Me.DotNetBarManager1.LeftDockSite = Me.DockSite1
        Me.DotNetBarManager1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.DotNetBarManager1.ParentForm = Me
        Me.DotNetBarManager1.RightDockSite = Me.DockSite2
        Me.DotNetBarManager1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003
        Me.DotNetBarManager1.ToolbarBottomDockSite = Me.DockSite8
        Me.DotNetBarManager1.ToolbarLeftDockSite = Me.DockSite5
        Me.DotNetBarManager1.ToolbarRightDockSite = Me.DockSite6
        Me.DotNetBarManager1.ToolbarTopDockSite = Me.DockSite7
        Me.DotNetBarManager1.TopDockSite = Me.DockSite3
        '
        'DockSite4
        '
        Me.DockSite4.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.DockSite4.DocumentDockContainer = New DevComponents.DotNetBar.DocumentDockContainer()
        Me.DockSite4.Location = New System.Drawing.Point(0, 513)
        Me.DockSite4.Name = "DockSite4"
        Me.DockSite4.Size = New System.Drawing.Size(666, 0)
        Me.DockSite4.TabIndex = 24
        Me.DockSite4.TabStop = False
        '
        'DockSite1
        '
        Me.DockSite1.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite1.Dock = System.Windows.Forms.DockStyle.Left
        Me.DockSite1.DocumentDockContainer = New DevComponents.DotNetBar.DocumentDockContainer()
        Me.DockSite1.Location = New System.Drawing.Point(0, 25)
        Me.DockSite1.Name = "DockSite1"
        Me.DockSite1.Size = New System.Drawing.Size(0, 488)
        Me.DockSite1.TabIndex = 21
        Me.DockSite1.TabStop = False
        '
        'DockSite2
        '
        Me.DockSite2.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite2.Dock = System.Windows.Forms.DockStyle.Right
        Me.DockSite2.DocumentDockContainer = New DevComponents.DotNetBar.DocumentDockContainer()
        Me.DockSite2.Location = New System.Drawing.Point(666, 25)
        Me.DockSite2.Name = "DockSite2"
        Me.DockSite2.Size = New System.Drawing.Size(0, 488)
        Me.DockSite2.TabIndex = 22
        Me.DockSite2.TabStop = False
        '
        'DockSite8
        '
        Me.DockSite8.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite8.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.DockSite8.Location = New System.Drawing.Point(0, 513)
        Me.DockSite8.Name = "DockSite8"
        Me.DockSite8.Size = New System.Drawing.Size(666, 0)
        Me.DockSite8.TabIndex = 28
        Me.DockSite8.TabStop = False
        '
        'DockSite5
        '
        Me.DockSite5.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite5.Dock = System.Windows.Forms.DockStyle.Left
        Me.DockSite5.Location = New System.Drawing.Point(0, 25)
        Me.DockSite5.Name = "DockSite5"
        Me.DockSite5.Size = New System.Drawing.Size(0, 488)
        Me.DockSite5.TabIndex = 25
        Me.DockSite5.TabStop = False
        '
        'DockSite6
        '
        Me.DockSite6.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite6.Dock = System.Windows.Forms.DockStyle.Right
        Me.DockSite6.Location = New System.Drawing.Point(666, 25)
        Me.DockSite6.Name = "DockSite6"
        Me.DockSite6.Size = New System.Drawing.Size(0, 488)
        Me.DockSite6.TabIndex = 26
        Me.DockSite6.TabStop = False
        '
        'DockSite7
        '
        Me.DockSite7.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite7.Controls.Add(Me.Bar1)
        Me.DockSite7.Dock = System.Windows.Forms.DockStyle.Top
        Me.DockSite7.Location = New System.Drawing.Point(0, 0)
        Me.DockSite7.Name = "DockSite7"
        Me.DockSite7.Size = New System.Drawing.Size(666, 25)
        Me.DockSite7.TabIndex = 27
        Me.DockSite7.TabStop = False
        '
        'Bar1
        '
        Me.Bar1.AccessibleDescription = "DotNetBar Bar (Bar1)"
        Me.Bar1.AccessibleName = "DotNetBar Bar"
        Me.Bar1.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar
        Me.Bar1.DockSide = DevComponents.DotNetBar.eDockSide.Top
        Me.Bar1.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.ButtonItem1, Me.ButtonItem3})
        Me.Bar1.Location = New System.Drawing.Point(0, 0)
        Me.Bar1.MenuBar = True
        Me.Bar1.Name = "Bar1"
        Me.Bar1.Size = New System.Drawing.Size(666, 24)
        Me.Bar1.Stretch = True
        Me.Bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003
        Me.Bar1.TabIndex = 0
        Me.Bar1.TabStop = False
        Me.Bar1.Text = "Bar1"
        '
        'ButtonItem1
        '
        Me.ButtonItem1.Name = "ButtonItem1"
        Me.ButtonItem1.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.btnCompact, Me.ButtonItemExit})
        Me.ButtonItem1.Text = "&File"
        '
        'btnCompact
        '
        Me.btnCompact.Name = "btnCompact"
        Me.btnCompact.Text = "Compact Databases"
        '
        'ButtonItemExit
        '
        Me.ButtonItemExit.GlobalName = "Exit"
        Me.ButtonItemExit.Image = Global.ServiceInstaller.My.Resources.Resources.Logout
        Me.ButtonItemExit.Name = "ButtonItemExit"
        Me.ButtonItemExit.Text = "E&xit"
        Me.ButtonItemExit.Tooltip = "Exit the program"
        '
        'ButtonItem3
        '
        Me.ButtonItem3.Name = "ButtonItem3"
        Me.ButtonItem3.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.ButtonItemStart, Me.ButtonItemStop, Me.ButtonItemInstall, Me.ButtonItemRemove})
        Me.ButtonItem3.Text = "Service"
        '
        'ButtonItemStart
        '
        Me.ButtonItemStart.GlobalName = "Start"
        Me.ButtonItemStart.Image = Global.ServiceInstaller.My.Resources.Resources.Sat
        Me.ButtonItemStart.Name = "ButtonItemStart"
        Me.ButtonItemStart.Text = "Start"
        '
        'ButtonItemStop
        '
        Me.ButtonItemStop.GlobalName = "Stop"
        Me.ButtonItemStop.Image = Global.ServiceInstaller.My.Resources.Resources.unsat
        Me.ButtonItemStop.Name = "ButtonItemStop"
        Me.ButtonItemStop.Text = "Stop"
        Me.ButtonItemStop.Tooltip = "Stop the service"
        '
        'ButtonItemInstall
        '
        Me.ButtonItemInstall.Image = Global.ServiceInstaller.My.Resources.Resources.solution
        Me.ButtonItemInstall.Name = "ButtonItemInstall"
        Me.ButtonItemInstall.Text = "Install"
        '
        'ButtonItemRemove
        '
        Me.ButtonItemRemove.Image = Global.ServiceInstaller.My.Resources.Resources.problem
        Me.ButtonItemRemove.Name = "ButtonItemRemove"
        Me.ButtonItemRemove.Text = "Remove"
        '
        'DockSite3
        '
        Me.DockSite3.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.DockSite3.Dock = System.Windows.Forms.DockStyle.Top
        Me.DockSite3.DocumentDockContainer = New DevComponents.DotNetBar.DocumentDockContainer()
        Me.DockSite3.Location = New System.Drawing.Point(0, 25)
        Me.DockSite3.Name = "DockSite3"
        Me.DockSite3.Size = New System.Drawing.Size(666, 0)
        Me.DockSite3.TabIndex = 23
        Me.DockSite3.TabStop = False
        '
        'GroupPanel1
        '
        Me.GroupPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupPanel1.CanvasColor = System.Drawing.SystemColors.Control
        Me.GroupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.GroupPanel1.Controls.Add(Me.ButtonStop)
        Me.GroupPanel1.Controls.Add(Me.ButtonStart)
        Me.GroupPanel1.Controls.Add(Me.LabelStart)
        Me.GroupPanel1.Controls.Add(Me.LabelStop)
        Me.GroupPanel1.Location = New System.Drawing.Point(8, 145)
        Me.GroupPanel1.Name = "GroupPanel1"
        Me.GroupPanel1.Size = New System.Drawing.Size(643, 95)
        '
        '
        '
        Me.GroupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.GroupPanel1.Style.BackColorGradientAngle = 90
        Me.GroupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.GroupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderBottomWidth = 1
        Me.GroupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.GroupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderLeftWidth = 1
        Me.GroupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderRightWidth = 1
        Me.GroupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderTopWidth = 1
        Me.GroupPanel1.Style.Class = ""
        Me.GroupPanel1.Style.CornerDiameter = 4
        Me.GroupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.GroupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.GroupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.GroupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.GroupPanel1.StyleMouseDown.Class = ""
        Me.GroupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.GroupPanel1.StyleMouseOver.Class = ""
        Me.GroupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.GroupPanel1.TabIndex = 29
        Me.GroupPanel1.Text = "Start / Stop"
        '
        'ButtonStop
        '
        Me.ButtonStop.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ButtonStop.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.ButtonStop.Image = Global.ServiceInstaller.My.Resources.Resources.unsat
        Me.ButtonStop.Location = New System.Drawing.Point(11, 38)
        Me.ButtonStop.Name = "ButtonStop"
        Me.ButtonStop.Size = New System.Drawing.Size(75, 23)
        Me.ButtonStop.TabIndex = 14
        Me.ButtonStop.Text = "Sto&p"
        '
        'ButtonStart
        '
        Me.ButtonStart.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ButtonStart.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.ButtonStart.Image = Global.ServiceInstaller.My.Resources.Resources.Sat
        Me.ButtonStart.Location = New System.Drawing.Point(11, 3)
        Me.ButtonStart.Name = "ButtonStart"
        Me.ButtonStart.Size = New System.Drawing.Size(75, 23)
        Me.ButtonStart.TabIndex = 13
        Me.ButtonStart.Text = "&Start"
        '
        'Bar2
        '
        Me.Bar2.AccessibleDescription = "DotNetBar Bar (Bar2)"
        Me.Bar2.AccessibleName = "DotNetBar Bar"
        Me.Bar2.AccessibleRole = System.Windows.Forms.AccessibleRole.StatusBar
        Me.Bar2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Bar2.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.ResizeHandle
        Me.Bar2.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.StatusText})
        Me.Bar2.Location = New System.Drawing.Point(0, 496)
        Me.Bar2.Name = "Bar2"
        Me.Bar2.Size = New System.Drawing.Size(666, 17)
        Me.Bar2.Stretch = True
        Me.Bar2.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003
        Me.Bar2.TabIndex = 31
        Me.Bar2.TabStop = False
        Me.Bar2.Text = "Copyright 2009, MZL Software Development, Inc."
        '
        'StatusText
        '
        Me.StatusText.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusText.Name = "StatusText"
        Me.StatusText.Text = "Build 22.  Copyright 2009, MZL Software Development, Inc."
        '
        'lblMasterBuild
        '
        Me.lblMasterBuild.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblMasterBuild.BackgroundStyle.Class = ""
        Me.lblMasterBuild.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblMasterBuild.Location = New System.Drawing.Point(61, 46)
        Me.lblMasterBuild.Name = "lblMasterBuild"
        Me.lblMasterBuild.Size = New System.Drawing.Size(93, 16)
        Me.lblMasterBuild.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2000
        Me.lblMasterBuild.TabIndex = 19
        Me.lblMasterBuild.Text = "Build #"
        '
        'lblMasterStatus
        '
        Me.lblMasterStatus.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblMasterStatus.BackgroundStyle.Class = ""
        Me.lblMasterStatus.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblMasterStatus.Location = New System.Drawing.Point(60, 61)
        Me.lblMasterStatus.Name = "lblMasterStatus"
        Me.lblMasterStatus.Size = New System.Drawing.Size(93, 16)
        Me.lblMasterStatus.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2000
        Me.lblMasterStatus.TabIndex = 20
        Me.lblMasterStatus.Text = "Checking status..."
        '
        'LabelX11
        '
        Me.LabelX11.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelX11.BackgroundStyle.Class = ""
        Me.LabelX11.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelX11.Location = New System.Drawing.Point(12, 61)
        Me.LabelX11.Name = "LabelX11"
        Me.LabelX11.Size = New System.Drawing.Size(41, 16)
        Me.LabelX11.TabIndex = 24
        Me.LabelX11.Text = "Status:"
        Me.LabelX11.TextAlignment = System.Drawing.StringAlignment.Far
        '
        'LabelX13
        '
        Me.LabelX13.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelX13.BackgroundStyle.Class = ""
        Me.LabelX13.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelX13.Location = New System.Drawing.Point(20, 46)
        Me.LabelX13.Name = "LabelX13"
        Me.LabelX13.Size = New System.Drawing.Size(33, 16)
        Me.LabelX13.TabIndex = 23
        Me.LabelX13.Text = "Build:"
        Me.LabelX13.TextAlignment = System.Drawing.StringAlignment.Far
        '
        'LabelBuild
        '
        Me.LabelBuild.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelBuild.BackgroundStyle.Class = ""
        Me.LabelBuild.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelBuild.Location = New System.Drawing.Point(63, 46)
        Me.LabelBuild.Name = "LabelBuild"
        Me.LabelBuild.Size = New System.Drawing.Size(93, 16)
        Me.LabelBuild.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2000
        Me.LabelBuild.TabIndex = 10
        Me.LabelBuild.Text = "Build #"
        '
        'LabelStatus
        '
        Me.LabelStatus.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelStatus.BackgroundStyle.Class = ""
        Me.LabelStatus.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelStatus.Location = New System.Drawing.Point(63, 62)
        Me.LabelStatus.Name = "LabelStatus"
        Me.LabelStatus.Size = New System.Drawing.Size(138, 16)
        Me.LabelStatus.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2000
        Me.LabelStatus.TabIndex = 11
        Me.LabelStatus.Text = "Checking Status..."
        '
        'LabelX3
        '
        Me.LabelX3.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelX3.BackgroundStyle.Class = ""
        Me.LabelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelX3.Location = New System.Drawing.Point(19, 88)
        Me.LabelX3.Name = "LabelX3"
        Me.LabelX3.Size = New System.Drawing.Size(41, 13)
        Me.LabelX3.TabIndex = 17
        Me.LabelX3.Text = "Started:"
        Me.LabelX3.TextAlignment = System.Drawing.StringAlignment.Far
        '
        'LabelServiceStart
        '
        Me.LabelServiceStart.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelServiceStart.BackgroundStyle.Class = ""
        Me.LabelServiceStart.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelServiceStart.Location = New System.Drawing.Point(63, 86)
        Me.LabelServiceStart.Name = "LabelServiceStart"
        Me.LabelServiceStart.Size = New System.Drawing.Size(143, 16)
        Me.LabelServiceStart.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2000
        Me.LabelServiceStart.TabIndex = 12
        Me.LabelServiceStart.Text = "Start Date"
        '
        'LabelX2
        '
        Me.LabelX2.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelX2.BackgroundStyle.Class = ""
        Me.LabelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelX2.Location = New System.Drawing.Point(15, 61)
        Me.LabelX2.Name = "LabelX2"
        Me.LabelX2.Size = New System.Drawing.Size(41, 16)
        Me.LabelX2.TabIndex = 16
        Me.LabelX2.Text = "Status:"
        Me.LabelX2.TextAlignment = System.Drawing.StringAlignment.Far
        '
        'LabelX1
        '
        Me.LabelX1.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelX1.BackgroundStyle.Class = ""
        Me.LabelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelX1.Location = New System.Drawing.Point(25, 45)
        Me.LabelX1.Name = "LabelX1"
        Me.LabelX1.Size = New System.Drawing.Size(32, 16)
        Me.LabelX1.TabIndex = 15
        Me.LabelX1.Text = "Build:"
        Me.LabelX1.TextAlignment = System.Drawing.StringAlignment.Far
        '
        'LabelX5
        '
        Me.LabelX5.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelX5.BackgroundStyle.Class = ""
        Me.LabelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelX5.Location = New System.Drawing.Point(0, 246)
        Me.LabelX5.Name = "LabelX5"
        Me.LabelX5.Size = New System.Drawing.Size(98, 16)
        Me.LabelX5.TabIndex = 19
        Me.LabelX5.Text = "Application Path:"
        Me.LabelX5.TextAlignment = System.Drawing.StringAlignment.Far
        '
        'LabelPath
        '
        Me.LabelPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelPath.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelPath.BackgroundStyle.Class = ""
        Me.LabelPath.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelPath.Location = New System.Drawing.Point(12, 268)
        Me.LabelPath.Name = "LabelPath"
        Me.LabelPath.Size = New System.Drawing.Size(644, 17)
        Me.LabelPath.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2000
        Me.LabelPath.TabIndex = 14
        Me.LabelPath.Text = "Application Path:"
        '
        'GroupPanel2
        '
        Me.GroupPanel2.CanvasColor = System.Drawing.SystemColors.Control
        Me.GroupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.GroupPanel2.Controls.Add(Me.LabelX6)
        Me.GroupPanel2.Controls.Add(Me.lblMasterStart)
        Me.GroupPanel2.Controls.Add(Me.LabelMaster)
        Me.GroupPanel2.Controls.Add(Me.pictureBoxMaster)
        Me.GroupPanel2.Controls.Add(Me.lblMasterBuild)
        Me.GroupPanel2.Controls.Add(Me.LabelX13)
        Me.GroupPanel2.Controls.Add(Me.LabelX11)
        Me.GroupPanel2.Controls.Add(Me.lblMasterStatus)
        Me.GroupPanel2.Location = New System.Drawing.Point(8, 291)
        Me.GroupPanel2.Name = "GroupPanel2"
        Me.GroupPanel2.Size = New System.Drawing.Size(202, 134)
        '
        '
        '
        Me.GroupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.GroupPanel2.Style.BackColorGradientAngle = 90
        Me.GroupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.GroupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel2.Style.BorderBottomWidth = 1
        Me.GroupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.GroupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel2.Style.BorderLeftWidth = 1
        Me.GroupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel2.Style.BorderRightWidth = 1
        Me.GroupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel2.Style.BorderTopWidth = 1
        Me.GroupPanel2.Style.Class = ""
        Me.GroupPanel2.Style.CornerDiameter = 4
        Me.GroupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.GroupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.GroupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.GroupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.GroupPanel2.StyleMouseDown.Class = ""
        Me.GroupPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.GroupPanel2.StyleMouseOver.Class = ""
        Me.GroupPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.GroupPanel2.TabIndex = 34
        Me.GroupPanel2.Text = "Master Service"
        '
        'LabelX6
        '
        Me.LabelX6.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelX6.BackgroundStyle.Class = ""
        Me.LabelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelX6.Location = New System.Drawing.Point(13, 83)
        Me.LabelX6.Name = "LabelX6"
        Me.LabelX6.Size = New System.Drawing.Size(41, 16)
        Me.LabelX6.TabIndex = 30
        Me.LabelX6.Text = "Started:"
        Me.LabelX6.TextAlignment = System.Drawing.StringAlignment.Far
        '
        'lblMasterStart
        '
        Me.lblMasterStart.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lblMasterStart.BackgroundStyle.Class = ""
        Me.lblMasterStart.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblMasterStart.Location = New System.Drawing.Point(61, 83)
        Me.lblMasterStart.Name = "lblMasterStart"
        Me.lblMasterStart.Size = New System.Drawing.Size(137, 16)
        Me.lblMasterStart.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2000
        Me.lblMasterStart.TabIndex = 29
        Me.lblMasterStart.Text = "Checking status..."
        '
        'LabelMaster
        '
        Me.LabelMaster.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelMaster.BackgroundStyle.Class = ""
        Me.LabelMaster.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelMaster.Location = New System.Drawing.Point(61, 9)
        Me.LabelMaster.Name = "LabelMaster"
        Me.LabelMaster.Size = New System.Drawing.Size(138, 16)
        Me.LabelMaster.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2000
        Me.LabelMaster.TabIndex = 28
        Me.LabelMaster.Text = "Searching..."
        '
        'pictureBoxMaster
        '
        Me.pictureBoxMaster.Image = Global.ServiceInstaller.My.Resources.Resources.solution
        Me.pictureBoxMaster.Location = New System.Drawing.Point(32, 9)
        Me.pictureBoxMaster.Name = "pictureBoxMaster"
        Me.pictureBoxMaster.Size = New System.Drawing.Size(16, 15)
        Me.pictureBoxMaster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.pictureBoxMaster.TabIndex = 27
        Me.pictureBoxMaster.TabStop = False
        '
        'GroupPanel3
        '
        Me.GroupPanel3.CanvasColor = System.Drawing.SystemColors.Control
        Me.GroupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.GroupPanel3.Controls.Add(Me.LabelService)
        Me.GroupPanel3.Controls.Add(Me.PictureBoxService)
        Me.GroupPanel3.Controls.Add(Me.LabelBuild)
        Me.GroupPanel3.Controls.Add(Me.LabelX3)
        Me.GroupPanel3.Controls.Add(Me.LabelServiceStart)
        Me.GroupPanel3.Controls.Add(Me.LabelX1)
        Me.GroupPanel3.Controls.Add(Me.LabelX2)
        Me.GroupPanel3.Controls.Add(Me.LabelStatus)
        Me.GroupPanel3.Location = New System.Drawing.Point(442, 291)
        Me.GroupPanel3.Name = "GroupPanel3"
        Me.GroupPanel3.Size = New System.Drawing.Size(214, 134)
        '
        '
        '
        Me.GroupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.GroupPanel3.Style.BackColorGradientAngle = 90
        Me.GroupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.GroupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel3.Style.BorderBottomWidth = 1
        Me.GroupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.GroupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel3.Style.BorderLeftWidth = 1
        Me.GroupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel3.Style.BorderRightWidth = 1
        Me.GroupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel3.Style.BorderTopWidth = 1
        Me.GroupPanel3.Style.Class = ""
        Me.GroupPanel3.Style.CornerDiameter = 4
        Me.GroupPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.GroupPanel3.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.GroupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.GroupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.GroupPanel3.StyleMouseDown.Class = ""
        Me.GroupPanel3.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.GroupPanel3.StyleMouseOver.Class = ""
        Me.GroupPanel3.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.GroupPanel3.TabIndex = 35
        Me.GroupPanel3.Text = "Monitor Service"
        '
        'LabelService
        '
        Me.LabelService.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelService.BackgroundStyle.Class = ""
        Me.LabelService.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelService.Location = New System.Drawing.Point(62, 8)
        Me.LabelService.Name = "LabelService"
        Me.LabelService.Size = New System.Drawing.Size(138, 16)
        Me.LabelService.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2000
        Me.LabelService.TabIndex = 30
        Me.LabelService.Text = "Searching..."
        '
        'PictureBoxService
        '
        Me.PictureBoxService.Image = Global.ServiceInstaller.My.Resources.Resources.solution
        Me.PictureBoxService.Location = New System.Drawing.Point(40, 9)
        Me.PictureBoxService.Name = "PictureBoxService"
        Me.PictureBoxService.Size = New System.Drawing.Size(16, 15)
        Me.PictureBoxService.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBoxService.TabIndex = 29
        Me.PictureBoxService.TabStop = False
        '
        'GroupPanel4
        '
        Me.GroupPanel4.CanvasColor = System.Drawing.SystemColors.Control
        Me.GroupPanel4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.GroupPanel4.Controls.Add(Me.LabelX7)
        Me.GroupPanel4.Controls.Add(Me.LabelDailyStart)
        Me.GroupPanel4.Controls.Add(Me.LabelDaily)
        Me.GroupPanel4.Controls.Add(Me.PictureBoxDaily)
        Me.GroupPanel4.Controls.Add(Me.LabelDailyBuild)
        Me.GroupPanel4.Controls.Add(Me.LabelX12)
        Me.GroupPanel4.Controls.Add(Me.LabelX14)
        Me.GroupPanel4.Controls.Add(Me.LabelDailyStatus)
        Me.GroupPanel4.Location = New System.Drawing.Point(225, 291)
        Me.GroupPanel4.Name = "GroupPanel4"
        Me.GroupPanel4.Size = New System.Drawing.Size(202, 134)
        '
        '
        '
        Me.GroupPanel4.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.GroupPanel4.Style.BackColorGradientAngle = 90
        Me.GroupPanel4.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.GroupPanel4.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel4.Style.BorderBottomWidth = 1
        Me.GroupPanel4.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.GroupPanel4.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel4.Style.BorderLeftWidth = 1
        Me.GroupPanel4.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel4.Style.BorderRightWidth = 1
        Me.GroupPanel4.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel4.Style.BorderTopWidth = 1
        Me.GroupPanel4.Style.Class = ""
        Me.GroupPanel4.Style.CornerDiameter = 4
        Me.GroupPanel4.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.GroupPanel4.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.GroupPanel4.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.GroupPanel4.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.GroupPanel4.StyleMouseDown.Class = ""
        Me.GroupPanel4.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.GroupPanel4.StyleMouseOver.Class = ""
        Me.GroupPanel4.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.GroupPanel4.TabIndex = 36
        Me.GroupPanel4.Text = "Daily Tasks"
        '
        'LabelX7
        '
        Me.LabelX7.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelX7.BackgroundStyle.Class = ""
        Me.LabelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelX7.Location = New System.Drawing.Point(6, 84)
        Me.LabelX7.Name = "LabelX7"
        Me.LabelX7.Size = New System.Drawing.Size(41, 14)
        Me.LabelX7.TabIndex = 30
        Me.LabelX7.Text = "Started:"
        Me.LabelX7.TextAlignment = System.Drawing.StringAlignment.Far
        '
        'LabelDailyStart
        '
        Me.LabelDailyStart.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelDailyStart.BackgroundStyle.Class = ""
        Me.LabelDailyStart.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelDailyStart.Location = New System.Drawing.Point(53, 86)
        Me.LabelDailyStart.Name = "LabelDailyStart"
        Me.LabelDailyStart.Size = New System.Drawing.Size(138, 10)
        Me.LabelDailyStart.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2000
        Me.LabelDailyStart.TabIndex = 29
        Me.LabelDailyStart.Text = "Checking status..."
        '
        'LabelDaily
        '
        Me.LabelDaily.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelDaily.BackgroundStyle.Class = ""
        Me.LabelDaily.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelDaily.Location = New System.Drawing.Point(54, 9)
        Me.LabelDaily.Name = "LabelDaily"
        Me.LabelDaily.Size = New System.Drawing.Size(138, 16)
        Me.LabelDaily.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2000
        Me.LabelDaily.TabIndex = 28
        Me.LabelDaily.Text = "Searching..."
        '
        'PictureBoxDaily
        '
        Me.PictureBoxDaily.Image = Global.ServiceInstaller.My.Resources.Resources.solution
        Me.PictureBoxDaily.Location = New System.Drawing.Point(30, 9)
        Me.PictureBoxDaily.Name = "PictureBoxDaily"
        Me.PictureBoxDaily.Size = New System.Drawing.Size(16, 15)
        Me.PictureBoxDaily.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBoxDaily.TabIndex = 27
        Me.PictureBoxDaily.TabStop = False
        '
        'LabelDailyBuild
        '
        Me.LabelDailyBuild.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelDailyBuild.BackgroundStyle.Class = ""
        Me.LabelDailyBuild.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelDailyBuild.Location = New System.Drawing.Point(54, 47)
        Me.LabelDailyBuild.Name = "LabelDailyBuild"
        Me.LabelDailyBuild.Size = New System.Drawing.Size(93, 15)
        Me.LabelDailyBuild.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2000
        Me.LabelDailyBuild.TabIndex = 19
        Me.LabelDailyBuild.Text = "Build #"
        '
        'LabelX12
        '
        Me.LabelX12.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelX12.BackgroundStyle.Class = ""
        Me.LabelX12.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelX12.Location = New System.Drawing.Point(14, 47)
        Me.LabelX12.Name = "LabelX12"
        Me.LabelX12.Size = New System.Drawing.Size(33, 12)
        Me.LabelX12.TabIndex = 23
        Me.LabelX12.Text = "Build:"
        Me.LabelX12.TextAlignment = System.Drawing.StringAlignment.Far
        '
        'LabelX14
        '
        Me.LabelX14.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelX14.BackgroundStyle.Class = ""
        Me.LabelX14.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelX14.Location = New System.Drawing.Point(6, 64)
        Me.LabelX14.Name = "LabelX14"
        Me.LabelX14.Size = New System.Drawing.Size(41, 12)
        Me.LabelX14.TabIndex = 24
        Me.LabelX14.Text = "Status:"
        Me.LabelX14.TextAlignment = System.Drawing.StringAlignment.Far
        '
        'LabelDailyStatus
        '
        Me.LabelDailyStatus.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.LabelDailyStatus.BackgroundStyle.Class = ""
        Me.LabelDailyStatus.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelDailyStatus.Location = New System.Drawing.Point(53, 58)
        Me.LabelDailyStatus.Name = "LabelDailyStatus"
        Me.LabelDailyStatus.Size = New System.Drawing.Size(93, 23)
        Me.LabelDailyStatus.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2000
        Me.LabelDailyStatus.TabIndex = 20
        Me.LabelDailyStatus.Text = "Checking status..."
        '
        'frmServiceController
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(217, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(666, 513)
        Me.Controls.Add(Me.GroupPanel4)
        Me.Controls.Add(Me.GroupPanel3)
        Me.Controls.Add(Me.GroupPanel2)
        Me.Controls.Add(Me.LabelX5)
        Me.Controls.Add(Me.Bar2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.GroupPanel1)
        Me.Controls.Add(Me.DockSite2)
        Me.Controls.Add(Me.DockSite1)
        Me.Controls.Add(Me.Installation)
        Me.Controls.Add(Me.LabelPath)
        Me.Controls.Add(Me.DockSite3)
        Me.Controls.Add(Me.DockSite4)
        Me.Controls.Add(Me.DockSite5)
        Me.Controls.Add(Me.DockSite6)
        Me.Controls.Add(Me.DockSite7)
        Me.Controls.Add(Me.DockSite8)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmServiceController"
        Me.Text = "VitalSigns Service Installer"
        Me.Panel1.ResumeLayout(False)
        CType(Me.Timer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Installation.ResumeLayout(False)
        Me.DockSite7.ResumeLayout(False)
        CType(Me.Bar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupPanel1.ResumeLayout(False)
        CType(Me.Bar2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupPanel2.ResumeLayout(False)
        Me.GroupPanel2.PerformLayout()
        CType(Me.pictureBoxMaster, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupPanel3.ResumeLayout(False)
        Me.GroupPanel3.PerformLayout()
        CType(Me.PictureBoxService, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupPanel4.ResumeLayout(False)
        Me.GroupPanel4.PerformLayout()
        CType(Me.PictureBoxDaily, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '   ServiceStatus()
        Me.Text = MyProductName & " Service Installer"
        Me.labelInstall.Text = "This button registers the " & MyProductName & " executable as a Windows Service."
        Me.LabelRemove.Text = "This button removes the " & MyProductName & " Service from the directory of Windows Services.  It does not delete any files."
        Me.LabelStart.Text = "This button starts the " & MyProductName & " services."
        Me.LabelStop.Text = "This button stops the " & MyProductName & " services."
        Me.LabelBanner.Text = "Running the 'VitalSigns Master Service' as an automatic service is recommended.  " & _
            "All other VitalSigns services should be set as Manual. The Daily Tasks service is normally Stopped."
        If ValidateNotesPath() = False Then
            LabelBanner.Text = "Notes does not seem to be on the System Path.  To modify your Path, select your computer's System Properties, Advanced, Environment Variables, System Variables Path."
            LabelBanner.BackColor = Color.LightCoral
            LabelBanner.ForeColor = Color.White
       
        Else
            UpdateServiceStatus()
        End If

        Try
            ServiceStatus()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub UpdateServiceStatus()

        Dim myRegistry As New RegistryHandler
        Try
            Me.LabelBuild.Text = CType(myRegistry.ReadFromRegistry("Service Build Number"), String)
        Catch ex As Exception
            Me.LabelBuild.Text = "N/A"
        End Try

        Try
            Me.lblMasterBuild.Text = CType(myRegistry.ReadFromRegistry("Master Service Build Number"), String)
        Catch ex As Exception
            Me.lblMasterBuild.Text = "N/A"
        End Try

        Try
            Me.lblMasterStart.Text = CType(myRegistry.ReadFromRegistry("Master Service Start"), String)
        Catch ex As Exception
            Me.lblMasterStart.Text = "N/A"
        End Try

        Try
            Me.LabelServiceStart.Text = CType(myRegistry.ReadFromRegistry("Service Start"), String)
        Catch ex As Exception
            Me.LabelServiceStart.Text = "N/A"
        End Try

        Try
            Me.LabelPath.Text = CType(myRegistry.ReadFromRegistry("Application Path"), String)
        Catch ex As Exception
            Me.LabelPath.Text = "N/A"
        End Try

        Try
            Me.LabelDailyStart.Text = CType(myRegistry.ReadFromRegistry("Daily Tasks Start"), String)
        Catch ex As Exception
            Me.LabelPath.Text = "N/A"
        End Try
        'Daily Tasks Build
        Try
            Me.LabelDailyBuild.Text = CType(myRegistry.ReadFromRegistry("Daily Tasks Build"), String)
        Catch ex As Exception
            Me.LabelPath.Text = "N/A"
        End Try
        myRegistry = Nothing
    End Sub

    Private Function ServiceStatus() As Boolean
        Dim MasterServiceController As New ServiceController(MasterServiceName, ".")
        Dim MonitorServiceController As New ServiceController(MonitorServiceName, ".")
        Dim DailyServiceController As New ServiceController(DailyServiceName, ".")

        '   Me.ButtonCompact.Enabled = True
        Me.ButtonStart.Enabled = True
        Me.ButtonStop.Enabled = True
        Me.ButtonItemStop.Enabled = True
        Me.ButtonItemStart.Enabled = True

        '********* Master Service
        Try
            If Not (InStr(MasterServiceController.Status, "not found")) Then
                Try
                    Me.ButtonItemInstall.Enabled = False
                    Me.buttonInstall.Enabled = False
                    Me.ButtonItemRemove.Enabled = True
                    Me.pictureBoxMaster.Image = ServiceInstaller.My.Resources.Resources.tick
                    Me.LabelMaster.Text = "Installed as a service."

                Catch ex As Exception

                End Try

                Try
                    StatusText.Text = "The " & MyProductName & " monitoring service is " & MonitorServiceController.Status.ToString
                    LabelStatus.Text = MonitorServiceController.Status.ToString
                Catch ex As Exception
                    StatusText.Text = ""
                    LabelStatus.Text = "n/a"
                End Try

                Try
                    lblMasterStatus.Text = MasterServiceController.Status.ToString
                Catch ex As Exception
                    StatusText.Text = ""
                    LabelStatus.Text = "n/a"
                End Try


                Try
                    If MasterServiceController.Status = ServiceControllerStatus.Stopped Then
                        'The service is stopped
                        '     Me.ButtonCompact.Enabled = True
                        Me.ButtonStart.Enabled = True
                        Me.ButtonStop.Enabled = False
                        Me.ButtonItemStop.Enabled = False
                        Me.ButtonItemStart.Enabled = True
                    Else
                        'The service is not stopped
                        ' Me.ButtonCompact.Enabled = False
                        Me.ButtonStart.Enabled = False
                        Me.ButtonItemStart.Enabled = False
                        Me.ButtonStop.Enabled = True
                        Me.ButtonItemStop.Enabled = True

                    End If
                Catch ex As Exception
                    '      Me.ButtonCompact.Enabled = False
                End Try

            Else
                StatusText.Text = "The " & MyProductName & " service is not installed."
                Me.ButtonItemInstall.Enabled = True
                Me.buttonInstall.Enabled = True
                Me.ButtonItemRemove.Enabled = False
                Me.ButtonItemRemove.Enabled = False
                Me.pictureBoxMaster.Image = ServiceInstaller.My.Resources.Resources.red
                Me.LabelMaster.Text = "Service is not installed."
            End If
        Catch ex As Exception
            StatusText.Text = "The " & MyProductName & " service is not installed."
            Me.ButtonItemInstall.Enabled = True
            Me.buttonInstall.Enabled = True
            Me.ButtonRemove.Enabled = False
            Me.ButtonItemRemove.Enabled = False
            Me.ButtonStart.Enabled = False
            Me.ButtonStop.Enabled = False
            Me.ButtonItemStop.Enabled = False
            Me.ButtonItemStart.Enabled = False
            Me.pictureBoxMaster.Image = ServiceInstaller.My.Resources.Resources.red
            Me.LabelMaster.Text = "Service is not installed."
            StatusText.Text = "The " & MyProductName & " service is not installed."
        End Try

        '************ Daily Tasks
        Try
            If Not (InStr(DailyServiceController.Status, "not found")) Then
                Try
                    Me.PictureBoxDaily.Image = ServiceInstaller.My.Resources.Resources.tick
                    Me.LabelDaily.Text = "Installed as a service."
                Catch ex As Exception

                End Try

                Try
                    LabelDailyStatus.Text = DailyServiceController.Status.ToString
                Catch ex As Exception

                End Try

            Else
                Me.PictureBoxDaily.Image = ServiceInstaller.My.Resources.Resources.red
                Me.LabelDaily.Text = "Service is not installed."
            End If
        Catch ex As Exception
            Me.PictureBoxDaily.Image = ServiceInstaller.My.Resources.Resources.red
            Me.LabelDaily.Text = "Service is not installed."

        End Try

        'Monitor Service
        Try
            If Not (InStr(MonitorServiceController.Status, "not found")) Then
                Try
                    Me.PictureBoxService.Image = ServiceInstaller.My.Resources.Resources.tick
                    Me.LabelService.Text = "Installed as a service."
                Catch ex As Exception

                End Try

                Try
                    LabelStatus.Text = MonitorServiceController.Status.ToString
                Catch ex As Exception

                End Try

            Else
                Me.PictureBoxService.Image = ServiceInstaller.My.Resources.Resources.red
                Me.LabelService.Text = "Service is not installed."
            End If
        Catch ex As Exception
            Me.PictureBoxService.Image = ServiceInstaller.My.Resources.Resources.red
            Me.LabelService.Text = "Service is not installed."
        End Try


    End Function

    Public Function ValidateNotesPath() As Boolean

        Dim paths() As String = Split(Environment.GetEnvironmentVariable("PATH"), ";")
        ' Dim messageOutput As String

        Dim InPath As Boolean = False
        For Each pathItem As String In paths
            If InStr(UCase(pathItem), "NOTES") Then
                InPath = True
            End If
        Next

        Return InPath
    End Function

#Region "Start/ Stop Service"

    Private Sub StartServiceThread()
        Dim threadStartService As New Thread(AddressOf StartService)
        StatusText.Text = "Attempting to start the monitoring services..."
        'If MessageBox.Show("Do you want to start the monitoring service?", "Start Monitoring", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
        '    Exit Sub
        'End If
        StartService()
        Exit Sub
        If threadStartService Is Nothing Then
            threadStartService = New Thread(AddressOf StartService)
        End If
        threadStartService.IsBackground = True

        Try
            threadStartService.Start()
        Catch ex As Exception
            Me.StatusText.Text = ex.Message
        End Try
    End Sub

    Private Sub StartService()
        StatusText.Text = "Starting monitoring service..."
        ' Find service
        Dim objServiceController As New ServiceController(MasterServiceName, ".")

        ' See if the service is valid on this machine
        If (objServiceController Is Nothing) Then
            MessageBox.Show("Could Not Find " & MyProductName & " monitoring service On This Machine")
            Return
        End If

        Me.Invalidate()
        Application.DoEvents()

        ' See if it's stopped
        If (objServiceController.Status = ServiceControllerStatus.Stopped) Then
            ' Start the service
            Try
                '    objServiceController.DisplayName = ProductName & " monitors Domino, BlackBerry, and other mail services."
                objServiceController.Start()
                ' Now wait for the service to start (10 seconds)
                objServiceController.WaitForStatus(ServiceControllerStatus.Running, _
                                                   New TimeSpan(0, 0, 30))
                '      MessageBox.Show("Monitor Pro monitoring service Started: Status = " +                                 objServiceController.Status.ToString)

                StatusText.Text = "The monitoring service is " & objServiceController.Status.ToString
            Catch ex As Exception
                MessageBox.Show("Unable To start VitalSigns monitoring service" & vbCrLf & ex.Message)
            End Try
        Else
            ' Service was already running
            MessageBox.Show(MyProductName & " Service is already running.")

        End If


    End Sub

    Private Sub StopService()
        StatusText.Text = "Attempting to stop the services...."
        ' Find service
        Dim objServiceController As New ServiceController(MasterServiceName, ".")

        ' See if the service is valid on this machine
        If (objServiceController Is Nothing) Then
            MessageBox.Show("Could Not Find " & MyProductName & " Service on this machine")

            Return
        End If

        ' See if it's running
        If (objServiceController.Status = ServiceControllerStatus.Running) Then
            ' Start the service
            Try
                objServiceController.Stop()
                ' Now wait for the service to stop (10 seconds)
                objServiceController.WaitForStatus(ServiceControllerStatus.Stopped, _
                                                   New TimeSpan(0, 0, 10))

            Catch ex As Exception
                MessageBox.Show("Unable To Stop the Service")

            End Try
        Else
            'Master Service wasn't started
            Try
                Dim MonitorServiceController As New ServiceController(MonitorServiceName, ".")
                If (MonitorServiceController.Status = ServiceControllerStatus.Running) Then
                    ' stop the service
                    Try
                        MonitorServiceController.Stop()
                        ' Now wait for the service to stop (10 seconds)
                        MonitorServiceController.WaitForStatus(ServiceControllerStatus.Stopped, _
                                                        New TimeSpan(0, 0, 10))

                    Catch ex As Exception
                        MessageBox.Show("Unable To Stop the Service")

                    End Try
                End If

            Catch ex As Exception

            End Try

            'MessageBox.Show(MyProductName & " service is not running - no need to stop it")
        End If
        ServiceStatus()
    End Sub
#End Region

#Region "Install / Uninstall Service"
    Private Sub InstallService()
        Dim WindowsDir, dotNetPath, dotNet2Path, dotNet3Path As String
        dotNetPath = ""
        dotNet2Path = ""
        dotNet3Path = ""

        WindowsDir = Environment.GetEnvironmentVariable("windir")
        Dim diContents As New DirectoryInfo(WindowsDir)
        Dim myFile As FileInfo
        '  Dim MyDirs() As String
        Dim dir As DirectoryInfo
        For Each dir In diContents.GetDirectories()
            Console.WriteLine(dir.FullName)
            If InStr(dir.FullName, "Microsoft.NET") Then
                dotNetPath = dir.FullName
            End If
        Next
        If dotNetPath = "" Then
            MessageBox.Show("The Microsoft .NET framework 2.0 or higher, which is required, was not found.  Please download from Microsoft.")
            Exit Sub
        End If

        Dim diDOTNET As New DirectoryInfo(dotNetPath & "\Framework")

        Dim Mydir As DirectoryInfo = Nothing
        Dim Success As Boolean = False

        'Search for 2.0 .NET Framework
        For Each Mydir In diDOTNET.GetDirectories()
            Console.WriteLine(Mydir.FullName)
            If InStr(Mydir.FullName, "v2.") Then
                dotNet2Path = Mydir.FullName
                Exit For
            End If
        Next

        For Each myFile In Mydir.GetFiles
            Console.WriteLine(myFile.FullName)
            If InStr(myFile.FullName, "InstallUtil") Then
                Success = True
            End If
        Next

        'Search for 3.x .NET framework

        For Each Mydir In diDOTNET.GetDirectories()
            Console.WriteLine(Mydir.FullName)
            If InStr(Mydir.FullName, "v3.") Then
                'dotNetPath = Mydir.FullName
                dotNet3Path = Mydir.FullName

                Exit For
            End If
        Next


        For Each myFile In Mydir.GetFiles
            Console.WriteLine(myFile.FullName)
            If InStr(myFile.FullName, "InstallUtil") Then
                Success = True
            End If
        Next



        If Success = False Then
            MessageBox.Show("InstallUtil.exe not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim MyRegistry As New RegistryHandler
        Dim MyAppPath As String
        MyAppPath = MyRegistry.ReadFromRegistry("Application Path")
        If Not MyAppPath Is Nothing Then
            StatusText.Text = "Attempting to register the Windows Service"
            Dim MyCommand As String
            'Install batch file
            DeleteBatFile(MyAppPath & "\instsrv.bat")
            '    MyCommand = dotNet3Path & "\installutil.exe " & " mailmonitorservice.exe"
            '   WriteBatchFile(MyAppPath & "\instsrv.bat", MyCommand)
            '  WriteBatchFile(MyAppPath & "\instsrv.bat", "Pause")
            MyCommand = dotNet2Path & "\installutil.exe " & " VitalSignsService.exe"
            WriteBatchFile(MyAppPath & "\instsrv.bat", MyCommand)
            MyCommand = dotNet2Path & "\installutil.exe " & " VitalSignsMasterService.exe"
            WriteBatchFile(MyAppPath & "\instsrv.bat", MyCommand)
            ' MyCommand = dotNet2Path & "\installutil.exe " & " VitalSignsOutput.exe"
            'WriteBatchFile(MyAppPath & "\instsrv.bat", MyCommand)
            MyCommand = dotNet2Path & "\installutil.exe " & " VitalSignsDailyTasks.exe"
            WriteBatchFile(MyAppPath & "\instsrv.bat", MyCommand)
            WriteBatchFile(MyAppPath & "\instsrv.bat", "Pause")

            'Uninstall batch file
            DeleteBatFile(MyAppPath & "\uninstsrv.bat")
            MyCommand = dotNet2Path & "\installutil.exe /u " & " VitalSignsService.exe"
            WriteBatchFile(MyAppPath & "\uninstsrv.bat", MyCommand)
            MyCommand = dotNet2Path & "\installutil.exe /u " & " VitalSignsMasterService.exe"
            WriteBatchFile(MyAppPath & "\uninstsrv.bat", MyCommand)
            ' MyCommand = dotNet2Path & "\installutil.exe /u " & " VitalSignsOutput.exe"
            ' WriteBatchFile(MyAppPath & "\uninstsrv.bat", MyCommand)
            MyCommand = dotNet2Path & "\installutil.exe /u " & " VitalSignsDailyTasks.exe"
            WriteBatchFile(MyAppPath & "\uninstsrv.bat", MyCommand)

            Try
                StatusText.Text = "Beginning attempt to register the Windows Service"
                Dim myProcess As System.Diagnostics.Process = New System.Diagnostics.Process
                myProcess.StartInfo.WorkingDirectory = MyAppPath
                myProcess.StartInfo.FileName = "instsrv.bat"
                myProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
                myProcess.Start()
                StatusText.Text = "Finished attempting to register the Windows Service"
                Exit Sub
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

        Else
            MessageBox.Show("Please run the " & MyProductName & " client before installing the service.")
            End
        End If
    End Sub

    Private Sub RemoveService()
        Dim MyRegistry As New RegistryHandler
        Dim MyAppPath As String
        MyAppPath = MyRegistry.ReadFromRegistry("Application Path")
        If Not MyAppPath Is Nothing Then
            Try
                Dim myProcess As System.Diagnostics.Process = New System.Diagnostics.Process
                myProcess.StartInfo.WorkingDirectory = MyAppPath
                myProcess.StartInfo.FileName = "uninstsrv.bat"
                myProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
                myProcess.Start()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If
    End Sub
#End Region

#Region "Batch File Handling"

    Private Sub WriteBatchFile(ByVal FileName As String, ByVal strMsg As String)
        Dim sw As StreamWriter
        If File.Exists(FileName) Then
            sw = New StreamWriter(FileName, True)
        Else
            sw = File.CreateText(FileName)
        End If
        sw.WriteLine(strMsg)
        sw.Close()
    End Sub

    Private Sub DeleteBatFile(ByVal FileName As String)
        ' Dim sw As StreamWriter
        If File.Exists(FileName) Then
            File.Delete(FileName)
        End If
    End Sub


#End Region

#Region "Button Handling"

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonStart.Click
        StartServiceThread()
    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonStop.Click
        StatusText.Text = "Attempting to stop the monitoring service.."
        ' StatusText.Invalidate()
        'If MessageBox.Show("Do you want to stop the monitoring service?", "Stop Monitoring", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
        '    Exit Sub
        'End If
        StopService()

    End Sub

    Private Sub btnInstall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonInstall.Click
        buttonInstall.Enabled = False
        ButtonRemove.Enabled = True
        InstallService()
        'ShowInstallSuccess()
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRemove.Click
        buttonInstall.Enabled = True
        ButtonRemove.Enabled = False
        RemoveService()

    End Sub

#End Region

    Private Sub Timer1_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer1.Elapsed
        ServiceStatus()
        UpdateServiceStatus()
    End Sub

    Private Sub CompactDBs()
        Me.Timer1.Enabled = False
        Dim MyRegistry As New RegistryHandler
        Dim MyAppPath As String
        Dim myDatabase As New DAO.DBEngine

        MyAppPath = MyRegistry.ReadFromRegistry("Application Path")
        If Not MyAppPath Is Nothing Then
            StatusText.Text = Now.ToString & " Begin compacting databases"
            Dim success As Boolean = False
            Dim myPath As String = MyAppPath & "Data\"
            'Dim myDBName As String = dbFileName
            Dim tempDB As String ' = myPath & "temp_" & myDBName
            Dim dbList(3) As String
            dbList(0) = "status.mdb"
            dbList(1) = "statistics.mdb"
            dbList(2) = "servers.mdb"
            dbList(3) = "mailfilestats.mdb"


            For Each db As String In dbList
                StatusText.Text = Now.ToString & " Begin compacting " & db
                tempDB = myPath & "temp_" & db
                Try
                    Call myDatabase.CompactDatabase(myPath & db, tempDB)
                    ' Thread.CurrentThread.Sleep(5000)
                    success = True

                Catch ex As Exception
                    MessageBox.Show(Now.ToString & " Error compacting " & db & ": " & ex.Message)
                End Try
                Try
                    If success = True Then
                        File.Delete(myPath & db)
                        File.Copy(tempDB, myPath & db)
                        File.Delete(tempDB)
                    End If
                    success = False
                Catch ex As Exception


                End Try

            Next

        End If

        System.Runtime.InteropServices.Marshal.ReleaseComObject(myDatabase)
        StatusText.Text = Now.ToString & " Finished compacting databases"
        Me.Timer1.Enabled = True
    End Sub



#Region "Menu Handling"

    Private Sub MenuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonItemExit.Click
        End
    End Sub

    Private Sub MenuServiceStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonItemStart.Click
        StartServiceThread()
    End Sub

    Private Sub MenuServiceStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonItemStop.Click
        StopService()
    End Sub

    Private Sub MenuServiceInstall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonItemInstall.Click
        InstallService()
    End Sub

    Private Sub MenuServiceUnInstall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonItemRemove.Click
        RemoveService()
    End Sub

#End Region

    Private Sub ShowInstallSuccess()
        StatusText.Text = "Validating install...."
        ' Find service
        Dim objServiceController As New ServiceController(MasterServiceName, ".")

        ' See if the service is valid on this machine
        If (objServiceController Is Nothing) Then
            MessageBox.Show("Could Not Find " & MyProductName & " Service on this machine")
        Else
            MessageBox.Show(MyProductName & " service was installed on this machine")

        End If

        ServiceStatus()
    End Sub

    Private Sub btnCompact_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompact.Click
        CompactDBs()
    End Sub
End Class



Public Class RegistryHandler
    Sub WriteToRegistry(ByVal KeyName As String, ByVal KeyValue As Object)
        Dim aKey As RegistryKey
        aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\MZLSoft", True)
        If aKey Is Nothing Then
            aKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("Software\MZLSoft")
        End If
        aKey.SetValue(KeyName, KeyValue)
        aKey.Flush()
    End Sub

    Function ReadFromRegistry(ByVal KeyName As String) As Object
        Dim aKey As RegistryKey
        aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\MZLSoft")
        Try
            'MessageBox.Show(aKey.ToString, "Registry read attempt")
        Catch ex As Exception

        End Try

        If aKey Is Nothing Then
            aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\MZLSoft")
        End If

        If aKey Is Nothing Then
            aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("VirtualStore\MACHINE\SOFTWARE\Wow6432Node\MZLSoft")
        End If

        If aKey Is Nothing Then
            aKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("VirtualStore\MACHINE\SOFTWARE\Wow6432Node\MZLSoft")
        End If



        If aKey Is Nothing Then
            Return Nothing
            Exit Function
        End If
        Return aKey.GetValue(KeyName)
    End Function
End Class




