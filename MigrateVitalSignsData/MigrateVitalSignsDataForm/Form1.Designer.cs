namespace MigrateVitalSignsDataForm
{
    partial class Form1
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
            this.sqlHostname = new System.Windows.Forms.TextBox();
            this.sqlUsername = new System.Windows.Forms.TextBox();
            this.sqlPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.mongoPassword = new System.Windows.Forms.TextBox();
            this.mongoUsername = new System.Windows.Forms.TextBox();
            this.mongoHostname = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.mongoPort = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.sqlPort = new System.Windows.Forms.TextBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // sqlHostname
            // 
            this.sqlHostname.Location = new System.Drawing.Point(134, 50);
            this.sqlHostname.Name = "sqlHostname";
            this.sqlHostname.Size = new System.Drawing.Size(100, 20);
            this.sqlHostname.TabIndex = 0;
            // 
            // sqlUsername
            // 
            this.sqlUsername.Location = new System.Drawing.Point(134, 76);
            this.sqlUsername.Name = "sqlUsername";
            this.sqlUsername.Size = new System.Drawing.Size(100, 20);
            this.sqlUsername.TabIndex = 1;
            this.sqlUsername.Text = "vs";
            // 
            // sqlPassword
            // 
            this.sqlPassword.Location = new System.Drawing.Point(134, 102);
            this.sqlPassword.Name = "sqlPassword";
            this.sqlPassword.PasswordChar = '*';
            this.sqlPassword.Size = new System.Drawing.Size(100, 20);
            this.sqlPassword.TabIndex = 2;
            this.sqlPassword.Text = "V1talsign$";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "SQL Host Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "SQL User Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "SQL Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(268, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Mongo Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(268, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Mongo User Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(268, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Mongo Host Name";
            // 
            // mongoPassword
            // 
            this.mongoPassword.Location = new System.Drawing.Point(370, 102);
            this.mongoPassword.Name = "mongoPassword";
            this.mongoPassword.PasswordChar = '*';
            this.mongoPassword.Size = new System.Drawing.Size(100, 20);
            this.mongoPassword.TabIndex = 8;
            this.mongoPassword.Text = "V1talsign$";
            // 
            // mongoUsername
            // 
            this.mongoUsername.Location = new System.Drawing.Point(370, 76);
            this.mongoUsername.Name = "mongoUsername";
            this.mongoUsername.Size = new System.Drawing.Size(100, 20);
            this.mongoUsername.TabIndex = 7;
            this.mongoUsername.Text = "vsadmin";
            // 
            // mongoHostname
            // 
            this.mongoHostname.Location = new System.Drawing.Point(370, 50);
            this.mongoHostname.Name = "mongoHostname";
            this.mongoHostname.Size = new System.Drawing.Size(100, 20);
            this.mongoHostname.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(196, 169);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Migrate Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(270, 220);
            this.textBox7.Multiline = true;
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox7.Size = new System.Drawing.Size(253, 118);
            this.textBox7.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(268, 135);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Mongo Port";
            // 
            // mongoPort
            // 
            this.mongoPort.Location = new System.Drawing.Point(370, 128);
            this.mongoPort.Name = "mongoPort";
            this.mongoPort.Size = new System.Drawing.Size(100, 20);
            this.mongoPort.TabIndex = 16;
            this.mongoPort.Text = "27017";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(44, 131);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "SQL Port";
            // 
            // sqlPort
            // 
            this.sqlPort.Location = new System.Drawing.Point(134, 128);
            this.sqlPort.Name = "sqlPort";
            this.sqlPort.Size = new System.Drawing.Size(100, 20);
            this.sqlPort.TabIndex = 14;
            this.sqlPort.Text = "1433";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "Credentials",
            "Locations",
            "License",
            "Nodes",
            "Maintenance",
            "Settings",
            "Users",
            "Consolidation Reports",
            "Domino Server Tasks",
            "Business Hours",
            "Servers",
            "Cluster Databases",
            "Alerts",
            "Traveler Summary Stats",
            "Traveler Data Store",
            "Log File Scanning",
            "Traveler Stats",
            "Summary Stats"});
            this.checkedListBox1.Location = new System.Drawing.Point(47, 220);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(217, 124);
            this.checkedListBox1.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 368);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.mongoPort);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.sqlPort);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.mongoPassword);
            this.Controls.Add(this.mongoUsername);
            this.Controls.Add(this.mongoHostname);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sqlPassword);
            this.Controls.Add(this.sqlUsername);
            this.Controls.Add(this.sqlHostname);
            this.Name = "Form1";
            this.Text = "VitalSigns Data Migrator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sqlHostname;
        private System.Windows.Forms.TextBox sqlUsername;
        private System.Windows.Forms.TextBox sqlPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox mongoPassword;
        private System.Windows.Forms.TextBox mongoUsername;
        private System.Windows.Forms.TextBox mongoHostname;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox mongoPort;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox sqlPort;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
    }
}

