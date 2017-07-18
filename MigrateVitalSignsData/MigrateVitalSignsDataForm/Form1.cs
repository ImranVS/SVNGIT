using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigrateVitalSignsDataForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public Timer timer;
        public Boolean inProcess = false;
        public System.IO.StringWriter sw;

        protected void timer_Ticked(object sender, EventArgs e)
        {
            if (!inProcess)
            {
                timer.Enabled = false;
                button1.Enabled = true;
            }
            textBox7.Text = sw.ToString();
            textBox7.SelectionStart = textBox7.Text.Length;
            textBox7.ScrollToCaret();

        }
         
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);
            
            string sqlString = "Data Source=" + sqlHostname.Text + "," + sqlPort.Text + ";Initial Catalog=VitalSigns; User ID=" + sqlUsername.Text + ";Password=" + sqlPassword.Text + ";Persist Security Info=True;";
            string mongoString = "mongodb://" + mongoUsername.Text + ":" + mongoPassword.Text + "@" + mongoHostname.Text + ":" + mongoPort.Text + "/vitalsigns_wes_test";
            List<String> selectedItems = checkedListBox1.CheckedItems.Cast<String>().ToList();
            System.Threading.Thread thread = new System.Threading.Thread(() => {
                inProcess = true;
                MigrateVitalSignsData.Program.Main(new string[] { sqlString, mongoString}, selectedItems);
                Console.WriteLine("Finished.");
                inProcess = false;
            });
            thread.IsBackground = true;
            thread.Start();

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer_Ticked;
            timer.Enabled = true;

            //String s = sw.ToString();
            //System.Threading.Thread.Sleep(10000);


            //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.FileName = @"powershell.exe";
            //startInfo.Arguments = @"""C:\VitalSigns\VSInstallScripts\BuildDeploymentScripts\BuildAutomation\AutoBuildScript.ps1 -Version " + buildName + " -IsTrunk " + IsTrunk + " -isDailyBuild " + IsDailyBuild + " -DoIfBuildExists " + OverwriteBuild + "\"";
            ////startInfo.Arguments = @"""C:\TestScript.ps1""";
            //startInfo.RedirectStandardOutput = true;
            //startInfo.RedirectStandardError = true;
            //startInfo.UseShellExecute = false;
            //startInfo.CreateNoWindow = true;
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            //process.StartInfo = startInfo;
            //process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(outputCollection_DataAdded);
            //process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(outputCollection_ErrorDataAdded);
            //process.Exited += new EventHandler(process_Exited);
            //process.StartInfo.Domain = "WIN-C6HBC6EODR2";
            //process.StartInfo.UserName = "Administrator";
            //string password = "Admin123!";
            //System.Security.SecureString ssPwd = new System.Security.SecureString();
            //for (int x = 0; x < password.Length; x++)
            //{
            //    ssPwd.AppendChar(password[x]);
            //}
            //process.StartInfo.Password = ssPwd;
            ////process.Start();

            //Session["OutputFromPS"] += "\n" + @"""C:\VitalSigns\VSInstallScripts\BuildDeploymentScripts\BuildAutomation\AutoBuildScript.ps1 -Version " + buildName + " -IsTrunk " + IsTrunk + " -isDailyBuild " + IsDailyBuild + " -DoIfBuildExists " + OverwriteBuild + "\"";

            //process.BeginErrorReadLine();
            //process.BeginOutputReadLine();
            ////System.Diagnostics.Process.
            //Session["process"] = process;
        }
    }
}
