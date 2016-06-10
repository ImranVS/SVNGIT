<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmTest
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Me.btnTest = New System.Windows.Forms.Button()
		Me.btnSharepoint = New System.Windows.Forms.Button()
		Me.btnActiveDirectory = New System.Windows.Forms.Button()
		Me.btnOffice365 = New System.Windows.Forms.Button()
		Me.btnWindows = New System.Windows.Forms.Button()
		Me.SuspendLayout()
		'
		'btnTest
		'
		Me.btnTest.Location = New System.Drawing.Point(12, 40)
		Me.btnTest.Name = "btnTest"
		Me.btnTest.Size = New System.Drawing.Size(282, 23)
		Me.btnTest.TabIndex = 0
		Me.btnTest.Text = "Test Exchange Service"
		Me.btnTest.UseVisualStyleBackColor = True
		'
		'btnSharepoint
		'
		Me.btnSharepoint.Location = New System.Drawing.Point(12, 69)
		Me.btnSharepoint.Name = "btnSharepoint"
		Me.btnSharepoint.Size = New System.Drawing.Size(282, 23)
		Me.btnSharepoint.TabIndex = 1
		Me.btnSharepoint.Text = "Test Sharepoint Service"
		Me.btnSharepoint.UseVisualStyleBackColor = True
		'
		'btnActiveDirectory
		'
		Me.btnActiveDirectory.Location = New System.Drawing.Point(12, 98)
		Me.btnActiveDirectory.Name = "btnActiveDirectory"
		Me.btnActiveDirectory.Size = New System.Drawing.Size(282, 23)
		Me.btnActiveDirectory.TabIndex = 2
		Me.btnActiveDirectory.Text = "Test Active Directory Service"
		Me.btnActiveDirectory.UseVisualStyleBackColor = True
		'
		'btnOffice365
		'
		Me.btnOffice365.Location = New System.Drawing.Point(12, 127)
		Me.btnOffice365.Name = "btnOffice365"
		Me.btnOffice365.Size = New System.Drawing.Size(282, 23)
		Me.btnOffice365.TabIndex = 3
		Me.btnOffice365.Text = "Test Office 365"
		Me.btnOffice365.UseVisualStyleBackColor = True
		'
		'btnWindows
		'
		Me.btnWindows.Location = New System.Drawing.Point(12, 156)
		Me.btnWindows.Name = "btnWindows"
		Me.btnWindows.Size = New System.Drawing.Size(282, 23)
		Me.btnWindows.TabIndex = 4
		Me.btnWindows.Text = "Test Windows Service"
		Me.btnWindows.UseVisualStyleBackColor = True
		'
		'FrmTest
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(329, 190)
		Me.Controls.Add(Me.btnWindows)
		Me.Controls.Add(Me.btnOffice365)
		Me.Controls.Add(Me.btnActiveDirectory)
		Me.Controls.Add(Me.btnSharepoint)
		Me.Controls.Add(Me.btnTest)
		Me.Name = "FrmTest"
		Me.Text = "Form1"
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents btnTest As System.Windows.Forms.Button
	Friend WithEvents btnSharepoint As System.Windows.Forms.Button
	Friend WithEvents btnActiveDirectory As System.Windows.Forms.Button
	Friend WithEvents btnOffice365 As System.Windows.Forms.Button
	Friend WithEvents btnWindows As System.Windows.Forms.Button

End Class
