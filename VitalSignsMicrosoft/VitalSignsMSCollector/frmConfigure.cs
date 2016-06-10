using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

using System.IO;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;

namespace VitalSignsMSCollector
{
	public partial class frmConfigure : Form
	{
		string serviceURL = "";
		public frmConfigure()
		{
			InitializeComponent();
			cbCountry.SelectedIndexChanged += new EventHandler(cbCountry_SelectedIndexChanged);
			cbState.SelectedIndexChanged += new EventHandler(cbState_SelectedIndexChanged);
			serviceURL = ConfigurationManager.AppSettings["serviceURL"].ToString();
		}

		void cbState_SelectedIndexChanged(object sender, EventArgs e)
		{
			getCities(cbCountry.Text.ToString(), cbState.Text.ToString());
		}

		private void getCities(string country,string state)
		{
			Common c = new Common();
			string response = c.submitRequest(serviceURL +"/GetCity.php?Country=" + country + "&State=" + state, "GET", "", "", "");
			if (response != "")
			{
				response = response.Replace(":", "").Replace("{", "").Replace("}", "");
				string[] s = response.Split(new char[] { ',' });

				foreach (string s1 in s)
				{
					cbCity.Items.Add(s1.Replace("\"", "").Replace("City", "").Replace("[", "").Replace("]", ""));
				}
			}
		}
		void cbCountry_SelectedIndexChanged(object sender, EventArgs e)
		{
			getStates(cbCountry.Text.ToString());
		}
		private void getStates(string country)
		{
			cbState.Items.Clear();
			Common c = new Common();
			string response = c.submitRequest(serviceURL +"/UpdateTables.php", "GET", "", "", "");
			if (response != "")
			{
				RootObject myDevices = new RootObject();
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(myDevices.GetType());
				MemoryStream ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(response));
				try
				{
					myDevices = (RootObject)serializer.ReadObject(ms);
					if (myDevices != null && myDevices.Location != null)
					{
						System.Collections.Generic.IEnumerable<Location> l = myDevices.Location.Where(x => x.Country == country).Distinct();

						foreach (Location l1 in l)
						{
							cbState.Items.Add(l1.State);
						}
						string s = "";
						//foreach (string l in myDevices.Location.Select(x => x.Country = "United States").Distinct())
						//{
						//    cbCountry.Items.Add(l);
						//}
					}
				}
				catch
				{
				}
			}
		}
		//private void frmConfigure_Load(object sender, EventArgs e)
		//{
			//txtUserName.Text = ConfigurationManager.AppSettings["UserName"].ToString();
			//txtPassword.Text = ConfigurationManager.AppSettings["Password"].ToString();
			//txtUserName.Text = ConfigurationManager.AppSettings["logFile"].ToString();
			////txtLogFileLocation.Text = ConfigurationManager.AppSettings["logFile"].ToString();
			//cbAutoUpdate.Checked = Convert.ToBoolean(ConfigurationManager.AppSettings["autoUpdate"].ToString());
			////cbCheckForUpdates.Checked = Convert.ToBoolean(ConfigurationManager.AppSettings["checkForUpdates"].ToString());
			////txtAppUpdatePath.Text = ConfigurationManager.AppSettings["checkTimeOfDay"].ToString();
			//txtDownloadTempFolder.Text = ConfigurationManager.AppSettings["downloadFolder"].ToString();
			//txtBackupFolder.Text = ConfigurationManager.AppSettings["backupLocation"].ToString();
			//txtDatabaseBackupFolder.Text = ConfigurationManager.AppSettings["databaseBackupFolder"].ToString();

			//for (int i = 0; i < cbDatabaseFlavor.Items.Count; i++)
			//    if (cbDatabaseFlavor.Items[i].ToString().StartsWith(ConfigurationManager.AppSettings["databaseFlavor"].ToString()))
			//        cbDatabaseFlavor.SelectedIndex = i;
			//string[] runtime = ConfigurationManager.AppSettings["checkTimeOfDay"].ToString().Split(':');

			//for (int i = 0; i < cbCountry.Items.Count; i++)
			//    if (cbCountry.Items[i].ToString().StartsWith(runtime[0].ToString().Trim()))
			//        cbCountry.SelectedIndex = i;
			//for (int i = 0; i < cbMins.Items.Count; i++)
			//    if (cbMins.Items[i].ToString().StartsWith(runtime[1].ToString().Trim()))
			//        cbMins.SelectedIndex = i;

			//for (int i = 0; i < cbDatabaseFlavor.Items.Count; i++)
			//    if (cbDatabaseFlavor.Items[i].ToString().StartsWith(ConfigurationManager.AppSettings["databaseFlavor"].ToString()))
			//        cbDatabaseFlavor.SelectedIndex = i;
			////txtLicenseKey.Text=ConfigurationManager.AppSettings["licenseKey"].ToString();
			////txtCurrentVersion.Text = ConfigurationManager.AppSettings["currentVersion"].ToString();
			//txtUserName.Text = ConfigurationManager.AppSettings["appUpdatePath"].ToString();
			////getLicenseAndVersionNumbers(txtAppUpdatePath.Text.ToString());
			//if (txtUserName.Text == "")
			//    return;
			//if (!Directory.Exists(txtUserName.Text))
			//{
			//    MessageBox.Show("Invalid Application Path!");
			//    return;
			//}
			//getApplicationInfo();
			////cbDatabaseFlavor.SelectedValue =ConfigurationManager.AppSettings["databaseFlavor"].ToString();

			//timeChanged = false;

			////database
			////txtServer.Text =ConfigurationManager.AppSettings["server"].ToString();
			////    txtDatabase.Text=ConfigurationManager.AppSettings["database"].ToString();
			////    txtUserId.Text =ConfigurationManager.AppSettings["userid"].ToString();
			////    txtPassword.Text = ConfigurationManager.AppSettings["password"].ToString();
		//}

		private void frmConfigure_Load_1(object sender, EventArgs e)
		{
			
		}
		public void loadMe()
		{
			getCountries();
			string country=ConfigurationManager.AppSettings["Country"].ToString();
			string state=ConfigurationManager.AppSettings["State"].ToString();
			string city = ConfigurationManager.AppSettings["City"].ToString();

			int i = cbCountry.Items.IndexOf(country);
			cbCountry.SelectedIndex = i;
			getStates(cbCountry.Text.ToString());
			int j = cbState.Items.IndexOf(state);
			cbState.SelectedIndex = j;
			getCities(cbCountry.Text.ToString(), cbState.Text.ToString());
			int k = cbCity.Items.IndexOf(city);
			cbCity.SelectedIndex = k;
			txtUserName.Text = ConfigurationManager.AppSettings["UserName"].ToString();
			txtPassword.Text = ConfigurationManager.AppSettings["Password"].ToString();
			txtServiceURL.Text  = ConfigurationManager.AppSettings["serviceURL"].ToString();
			txtPingURL.Text = ConfigurationManager.AppSettings["PingURL"].ToString();

			cbPingTest.Checked = Convert.ToBoolean(ConfigurationManager.AppSettings["PingTest"].ToString());
			cbLoginTest.Checked = Convert.ToBoolean(ConfigurationManager.AppSettings["ServerLoginTest"].ToString());
			cbSPOTest.Checked = Convert.ToBoolean(ConfigurationManager.AppSettings["SPOLoginTest"].ToString());


			txtPingURL.Text = ConfigurationManager.AppSettings["ServerLoginTest"].ToString();
			txtPingURL.Text = ConfigurationManager.AppSettings["SPOLoginTest"].ToString();

			int l = cbScanInterval.Items.IndexOf(ConfigurationManager.AppSettings["ScanIntervalMins"].ToString());
			cbScanInterval.SelectedIndex = l;
		}
		private void getCountries()
		{
			Common c= new Common();
			string response = c.submitRequest(serviceURL +"/UpdateTables.php", "GET", "", "", "");
			if (response != "")
			{
				RootObject myDevices = new RootObject();
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(myDevices.GetType());
				MemoryStream ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(response));
				try
				{
					myDevices = (RootObject)serializer.ReadObject(ms);
					if (myDevices != null && myDevices.Location != null)
					{
						foreach( string l in myDevices.Location.Select(x => x.Country).Distinct())
						{
							cbCountry.Items.Add(l);
						}

						//if (myDevices.NameSpaceType == "Federated")
						//{
						//    myServer.ADFSMode = true;
						//    response = submitRequest(myDevices.AuthURL, "GET", "", "", "");
						//    if (response =="-1")
						//    {
						//        myServer.ADFSRedirectTest = false;
						//        Common.makeAlert(false , myServer, commonEnums.AlertType.ADFS, ref AllTestsList, "ADFS Redirect Failed", myServer.Category);
						//    }
						//    else
						//    {
						//        myServer.ADFSRedirectTest = true;
						//        Common.makeAlert(true, myServer, commonEnums.AlertType.ADFS, ref AllTestsList, "", myServer.Category);
						//    }
						//}
						//else
						//{
						//    myServer.ADFSMode = false;
						//    //Common.makeAlert(true, myServer, commonEnums.AlertType.ADFS, ref AllTestsList, "", myServer.Category);
						//}
					}
						
				}
				catch (Exception ex)
				{
					string s = ex.Message.ToString();
				}
			}
		}

		public void appSettingsUpdate(string strKey, string strValue)
		{
			Configuration objConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
			try
			{
				if ((objAppsettings != null))
				{
					if (objAppsettings.Settings[strKey].Value != strValue)
					{
						try
						{
							objAppsettings.Settings[strKey].Value = strValue;
							objConfig.Save(ConfigurationSaveMode.Modified);
							ConfigurationManager.RefreshSection("appSettings");
						}
						catch (Exception ex)
						{
							throw ex;
						}
					}
				}
			}
			catch (System.Security.SecurityException ex2)
			{
				throw ex2;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			appSettingsUpdate("UserName", txtUserName.Text.ToString());
			appSettingsUpdate("Password", txtPassword.Text.ToString());
			appSettingsUpdate("Country", cbCountry.Text.ToString());
			appSettingsUpdate("State", cbState.Text.ToString());
			appSettingsUpdate("City", cbCity.Text.ToString());
			appSettingsUpdate("ScanIntervalMins", cbScanInterval.Text.ToString());
			appSettingsUpdate("serviceURL", txtServiceURL.Text.ToString());
			appSettingsUpdate("PingURL", txtPingURL.Text.ToString());
			appSettingsUpdate("PingTest", cbPingTest.Checked.ToString());
			appSettingsUpdate("ServerLoginTest", cbLoginTest.Checked.ToString());
			appSettingsUpdate("SPOLoginTest", cbSPOTest.Checked.ToString());

			MessageBox.Show("Please exit the application and re-start the application for the changes to take affect", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			DialogResult dlgR = MessageBox.Show("Are you sure, you want to exit the Application?", "Exit Application", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
			if (dlgR == System.Windows.Forms.DialogResult.OK)
			{
				Application.Exit();

			}

		}

	}
	public class locations
	{
		public string Country;
	}
	public class O
	{
		public string OSType { get; set; }
		public string TranslatedValue { get; set; }
		public string OSName { get; set; }
	}

	public class Device
	{
		public string DeviceType { get; set; }
		public string TranslatedValue { get; set; }
		public string OSName { get; set; }
	}

	public class Location
	{
		public string Country { get; set; }
		public string State { get; set; }
	}

	public class RootObject
	{
		public List<O> OS { get; set; }
		public List<Device> Device { get; set; }
		public List<Location> Location { get; set; }
	}
	public class Cities
	{
		public string City { get; set; }
	}
}
