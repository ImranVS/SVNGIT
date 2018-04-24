namespace VitalSignsMSCollector
{
	partial class frmConfigure
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.lblAppUpdatePath = new System.Windows.Forms.Label();
			this.lblCurrentVersion = new System.Windows.Forms.Label();
			this.lblLicenseKey = new System.Windows.Forms.Label();
			this.cbCountry = new System.Windows.Forms.ComboBox();
			this.cbState = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cbCity = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cbScanInterval = new System.Windows.Forms.ComboBox();
			this.lblScanInterval = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.txtServiceURL = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.txtPingURL = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.cbPingTest = new System.Windows.Forms.CheckBox();
			this.cbLoginTest = new System.Windows.Forms.CheckBox();
			this.cbSPOTest = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// txtUserName
			// 
			this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtUserName.Location = new System.Drawing.Point(164, 51);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(266, 20);
			this.txtUserName.TabIndex = 16;
			// 
			// txtPassword
			// 
			this.txtPassword.AcceptsReturn = true;
			this.txtPassword.AcceptsTab = true;
			this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPassword.Location = new System.Drawing.Point(164, 82);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(266, 20);
			this.txtPassword.TabIndex = 14;
			// 
			// lblAppUpdatePath
			// 
			this.lblAppUpdatePath.AutoSize = true;
			this.lblAppUpdatePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblAppUpdatePath.Location = new System.Drawing.Point(58, 58);
			this.lblAppUpdatePath.Name = "lblAppUpdatePath";
			this.lblAppUpdatePath.Size = new System.Drawing.Size(96, 13);
			this.lblAppUpdatePath.TabIndex = 13;
			this.lblAppUpdatePath.Text = "Office 365 User";
			// 
			// lblCurrentVersion
			// 
			this.lblCurrentVersion.AutoSize = true;
			this.lblCurrentVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCurrentVersion.Location = new System.Drawing.Point(104, 171);
			this.lblCurrentVersion.Name = "lblCurrentVersion";
			this.lblCurrentVersion.Size = new System.Drawing.Size(50, 13);
			this.lblCurrentVersion.TabIndex = 12;
			this.lblCurrentVersion.Text = "Country";
			// 
			// lblLicenseKey
			// 
			this.lblLicenseKey.AutoSize = true;
			this.lblLicenseKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLicenseKey.Location = new System.Drawing.Point(93, 89);
			this.lblLicenseKey.Name = "lblLicenseKey";
			this.lblLicenseKey.Size = new System.Drawing.Size(61, 13);
			this.lblLicenseKey.TabIndex = 11;
			this.lblLicenseKey.Text = "Password";
			// 
			// cbCountry
			// 
			this.cbCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbCountry.FormattingEnabled = true;
			this.cbCountry.Location = new System.Drawing.Point(164, 163);
			this.cbCountry.Name = "cbCountry";
			this.cbCountry.Size = new System.Drawing.Size(266, 21);
			this.cbCountry.TabIndex = 26;
			// 
			// cbState
			// 
			this.cbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbState.FormattingEnabled = true;
			this.cbState.Location = new System.Drawing.Point(164, 195);
			this.cbState.Name = "cbState";
			this.cbState.Size = new System.Drawing.Size(266, 21);
			this.cbState.TabIndex = 28;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(117, 203);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(37, 13);
			this.label1.TabIndex = 27;
			this.label1.Text = "State";
			// 
			// cbCity
			// 
			this.cbCity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbCity.FormattingEnabled = true;
			this.cbCity.Location = new System.Drawing.Point(164, 227);
			this.cbCity.Name = "cbCity";
			this.cbCity.Size = new System.Drawing.Size(266, 21);
			this.cbCity.TabIndex = 30;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(126, 235);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(28, 13);
			this.label2.TabIndex = 29;
			this.label2.Text = "City";
			// 
			// cbScanInterval
			// 
			this.cbScanInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbScanInterval.FormattingEnabled = true;
			this.cbScanInterval.Items.AddRange(new object[] {
            "8",
            "10",
            "20",
            "30",
            "60",
            "90",
            "120"});
			this.cbScanInterval.Location = new System.Drawing.Point(164, 480);
			this.cbScanInterval.Name = "cbScanInterval";
			this.cbScanInterval.Size = new System.Drawing.Size(70, 21);
			this.cbScanInterval.TabIndex = 32;
			// 
			// lblScanInterval
			// 
			this.lblScanInterval.AutoSize = true;
			this.lblScanInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblScanInterval.Location = new System.Drawing.Point(71, 488);
			this.lblScanInterval.Name = "lblScanInterval";
			this.lblScanInterval.Size = new System.Drawing.Size(83, 13);
			this.lblScanInterval.TabIndex = 31;
			this.lblScanInterval.Text = "Scan Interval";
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(303, 507);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(116, 23);
			this.button1.TabIndex = 33;
			this.button1.Text = "Update";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button2.Location = new System.Drawing.Point(164, 507);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(116, 23);
			this.button2.TabIndex = 34;
			this.button2.Text = "Exit";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(27, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(387, 17);
			this.label3.TabIndex = 35;
			this.label3.Text = "Configure your Office 365 Account information here:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(27, 127);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(244, 17);
			this.label4.TabIndex = 36;
			this.label4.Text = "Configure your current Location:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(45, 410);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(113, 17);
			this.label5.TabIndex = 37;
			this.label5.Text = "Scan Settings:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(251, 488);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(66, 13);
			this.label6.TabIndex = 38;
			this.label6.Text = "In Minites.";
			// 
			// button3
			// 
			this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button3.ForeColor = System.Drawing.Color.Red;
			this.button3.Location = new System.Drawing.Point(164, 537);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(255, 23);
			this.button3.TabIndex = 39;
			this.button3.Text = "Exit Application";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(27, 279);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(131, 17);
			this.label7.TabIndex = 40;
			this.label7.Text = "Service Settings:";
			// 
			// txtServiceURL
			// 
			this.txtServiceURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtServiceURL.Location = new System.Drawing.Point(164, 310);
			this.txtServiceURL.Name = "txtServiceURL";
			this.txtServiceURL.Size = new System.Drawing.Size(266, 20);
			this.txtServiceURL.TabIndex = 42;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(75, 317);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(79, 13);
			this.label8.TabIndex = 41;
			this.label8.Text = "Service URL";
			// 
			// txtPingURL
			// 
			this.txtPingURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPingURL.Location = new System.Drawing.Point(164, 336);
			this.txtPingURL.Name = "txtPingURL";
			this.txtPingURL.Size = new System.Drawing.Size(266, 20);
			this.txtPingURL.TabIndex = 44;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(93, 343);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(61, 13);
			this.label9.TabIndex = 43;
			this.label9.Text = "Ping URL";
			// 
			// textBox1
			// 
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox1.Location = new System.Drawing.Point(164, 362);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(266, 20);
			this.textBox1.TabIndex = 46;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(81, 369);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(73, 13);
			this.label10.TabIndex = 45;
			this.label10.Text = "Health URL";
			// 
			// cbPingTest
			// 
			this.cbPingTest.AutoSize = true;
			this.cbPingTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbPingTest.Location = new System.Drawing.Point(172, 426);
			this.cbPingTest.Name = "cbPingTest";
			this.cbPingTest.Size = new System.Drawing.Size(80, 17);
			this.cbPingTest.TabIndex = 47;
			this.cbPingTest.Text = "Ping Test";
			this.cbPingTest.UseVisualStyleBackColor = true;
			// 
			// cbLoginTest
			// 
			this.cbLoginTest.AutoSize = true;
			this.cbLoginTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbLoginTest.Location = new System.Drawing.Point(172, 449);
			this.cbLoginTest.Name = "cbLoginTest";
			this.cbLoginTest.Size = new System.Drawing.Size(86, 17);
			this.cbLoginTest.TabIndex = 48;
			this.cbLoginTest.Text = "Login Test";
			this.cbLoginTest.UseVisualStyleBackColor = true;
			// 
			// cbSPOTest
			// 
			this.cbSPOTest.AutoSize = true;
			this.cbSPOTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbSPOTest.Location = new System.Drawing.Point(279, 426);
			this.cbSPOTest.Name = "cbSPOTest";
			this.cbSPOTest.Size = new System.Drawing.Size(80, 17);
			this.cbSPOTest.TabIndex = 49;
			this.cbSPOTest.Text = "SPO Test";
			this.cbSPOTest.UseVisualStyleBackColor = true;
			// 
			// frmConfigure
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(623, 579);
			this.Controls.Add(this.cbSPOTest);
			this.Controls.Add(this.cbLoginTest);
			this.Controls.Add(this.cbPingTest);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.txtPingURL);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.txtServiceURL);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.cbScanInterval);
			this.Controls.Add(this.lblScanInterval);
			this.Controls.Add(this.cbCity);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cbState);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbCountry);
			this.Controls.Add(this.txtUserName);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.lblAppUpdatePath);
			this.Controls.Add(this.lblCurrentVersion);
			this.Controls.Add(this.lblLicenseKey);
			this.Name = "frmConfigure";
			this.Text = "Configure";
			this.Load += new System.EventHandler(this.frmConfigure_Load_1);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label lblAppUpdatePath;
		private System.Windows.Forms.Label lblCurrentVersion;
		private System.Windows.Forms.Label lblLicenseKey;
		private System.Windows.Forms.ComboBox cbCountry;
		private System.Windows.Forms.ComboBox cbState;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbCity;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbScanInterval;
		private System.Windows.Forms.Label lblScanInterval;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtServiceURL;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtPingURL;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.CheckBox cbPingTest;
		private System.Windows.Forms.CheckBox cbLoginTest;
		private System.Windows.Forms.CheckBox cbSPOTest;
	}
}