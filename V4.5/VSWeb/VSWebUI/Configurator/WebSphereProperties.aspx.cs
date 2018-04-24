using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using VSWebDO;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Data.SqlClient;
using System.Threading;
using System.Collections;
using DevExpress.Web.ASPxTreeList;
using VSWebBL;

namespace VSWebUI.Configurator
{
	public partial class WebSphereProperties : System.Web.UI.Page
	{
		int cellid;
		bool isValid = true;
		string Mode;
		protected int ServerID;
		protected int ServerTypeID;
		protected DataTable WebSphereDataTable = null;
		protected void Page_Load(object sender, EventArgs e)
		{
			ServerTypeID = 23;
			lblServerId.Text = Request.QueryString["ID"];
	
			int id=Convert.ToInt32( Request.QueryString["ID"]);
			if (Request.QueryString["name"] != "" && Request.QueryString["name"] != null)
			{
				Mode = "Update";
			}
			else 
			{
				Mode = "Insert";
				if (!IsPostBack)
				{

					RetryTextBox.Text = "2";
					OffscanTextBox.Text = "30";
					ScanIntvlTextBox.Text = "8";
					ResponseTextBox.Text = "2500";
					EnabledCheckBox.Checked = true;

				}
			}
			if (!IsPostBack)
			{
				FillCellTypeComboBox();
				FillCredentialsComboBox();
				FillLocationComboBox();
				
				if (Request.QueryString["name"] != "" && Request.QueryString["name"] != null)
				{

					Filldata();
					FillWebsphereData(id);
					NameTextBox.Text = Request.QueryString["name"].ToString();
					LocationComboBox.Text = Request.QueryString["Loc"].ToString();
                    CellnameComboBox.Text = Request.QueryString["CellName"].ToString();
                    NodenameComboBox.Text = Request.QueryString["NodeName"].ToString();
					DescTextBox.Enabled = false;
					NameTextBox.Enabled = false;
					LocationComboBox.Enabled = false;
					CellnameComboBox.Enabled = false;
					NodenameComboBox.Enabled = false;
				}

				FillDiskGridView();
			}
			else
			{
				
				FillDiskGridfromSession();
		
				if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() != "0")
				{
					AdvDiskSpaceThTrackBar.Visible = false;
				}
			}
		
		}

	
		public void Filldata()
		{
			DataTable dt = new DataTable();
			try
			{
				Servers ServersObject = new Servers();
				ServersObject.ServerName = Request.QueryString["name"].ToString();
				ServersObject.ServerTypeID = ServerTypeID;
				dt = VSWebBL.SecurityBL.ServersBL.Ins.GetAllDataByName(ServersObject);
				if (dt.Rows.Count > 0)
				{
					lblServer.InnerHtml += " - " + dt.Rows[0]["ServerName"].ToString();
					DescTextBox.Text = dt.Rows[0]["Description"].ToString();
					if (dt.Rows[0]["Enabled"].ToString() != "" && dt.Rows[0]["Enabled"] != null)
					EnabledCheckBox.Checked = bool.Parse(dt.Rows[0]["Enabled"].ToString());
					RetryTextBox.Text = dt.Rows[0]["RetryInterval"].ToString();
					ResponseTextBox.Text = dt.Rows[0]["ResponseTime"].ToString();
					OffscanTextBox.Text = dt.Rows[0]["OffHourInterval"].ToString();
					AdvCPUThTrackBar.Value = dt.Rows[0]["CPU_Threshold"].ToString();
					CPURoundPanel.HeaderText = "CPU Utilization Alert at " + AdvCPUThTrackBar.Value + "% utilization";
					AdvMemoryThTrackBar.Value = dt.Rows[0]["MemThreshold"].ToString();
					MemRoundPanel9.HeaderText = "Memory Usage Alert at " + AdvMemoryThTrackBar.Value + "% utilization";
					CategoryTextBox.Text = dt.Rows[0]["Category"].ToString();
					ScanIntvlTextBox.Text = dt.Rows[0]["ScanInterval"].ToString();
					ConsFailuresBefAlertTextBox.Text = dt.Rows[0]["ConsFailuresBefAlert"].ToString();
					int credentialsId = 0;
					if (dt.Rows[0]["CredentialsId"].ToString() != null && dt.Rows[0]["CredentialsId"].ToString() != "")
					{
						credentialsId = Convert.ToInt32(dt.Rows[0]["CredentialsId"].ToString());
						CredentialsComboBox.SelectedItem = CredentialsComboBox.Items.FindByValue(credentialsId);
						
						for (int i = 0; i < CredentialsComboBox.Items.Count; i++)
						{
							if (CredentialsComboBox.Items[i].Value.ToString() == credentialsId.ToString())
								CredentialsComboBox.Items[i].Selected = true;
						}
					}

				}
			}
			catch (Exception ex)
			{
			
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

		}
		public void FillWebsphereData(int id)
		{
			DataTable dt = new DataTable();
			try
			{
				WebSpherePropertie web = new WebSpherePropertie();
				web.ServerID =Convert.ToInt32( Request.QueryString["ID"]);
				//Servers ServersObject = new Servers();
				//ServersObject.ServerName = Request.QueryString["name"].ToString();
				//ServersObject.ServerTypeID = ServerTypeID;
				dt = VSWebBL.ConfiguratorBL.WebSpherepropertiesBL.Ins.GetAllDataByNames(web);
				if (dt.Rows.Count > 0)
				{
					ThreadPollTextbox.Text = dt.Rows[0]["AvgThreadPool"].ToString();
					ThreadCountTextBox.Text = dt.Rows[0]["ActiveThreadCount"].ToString();
					HeapCurrentTextBox.Text = dt.Rows[0]["CurrentHeap"].ToString();
					MaximunHeapTextBox.Text = dt.Rows[0]["MaxHeap"].ToString();
					UpTimeTextBox.Text = dt.Rows[0]["Uptime"].ToString();
					HungThradTextBox.Text = dt.Rows[0]["HungThreadCount"].ToString();
					DumpGeneratorTextBox.Text = dt.Rows[0]["DumpGenerated"].ToString();
				}
			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		private WebSpherePropertie CollectDataForWebSpherePropertie()
		{
			try
			{
				DataTable dt = VSWebBL.ConfiguratorBL.WebSpherepropertiesBL.Ins.GetDataForServerID(NameTextBox.Text);
				WebSpherePropertie WebSphereServerObject = new WebSpherePropertie();
				WebSphereServerObject.ServerID =Convert.ToInt32( dt.Rows[0]["ID"].ToString());
				WebSphereServerObject.CellID = Convert.ToInt32(CellnameComboBox.Value);
				WebSphereServerObject.NodeID = Convert.ToInt32(NodenameComboBox.Value);
				WebSphereServerObject.ServerName = NameTextBox.Text;
				
					WebSphereServerObject.AvgThreadPool =int.Parse(ThreadPollTextbox.Text==""?"0":ThreadPollTextbox.Text);
					WebSphereServerObject.ActiveThreadCount = int.Parse(ThreadCountTextBox.Text == "" ? "0" : ThreadCountTextBox.Text);
					WebSphereServerObject.CurrentHeap = (HeapCurrentTextBox.Text == "" ? "0" : HeapCurrentTextBox.Text);
					WebSphereServerObject.MaxHeap = MaximunHeapTextBox.Text == "" ? "0" : MaximunHeapTextBox.Text;
					WebSphereServerObject.Uptime = int.Parse(UpTimeTextBox.Text == "" ? "0" : UpTimeTextBox.Text);
					WebSphereServerObject.HungThreadCount = int.Parse(HungThradTextBox.Text == "" ? "0" : HungThradTextBox.Text);
					WebSphereServerObject.DumpGenerated = DumpGeneratorTextBox.Text == "" ? "0" : DumpGeneratorTextBox.Text;
				WebSphereServerObject.Enabled = EnabledCheckBox.Checked;
				//WebSphereServerObject.Name = NameTextBox.Text;
				//WebSphereServerObject.Category = CategoryTextBox.Text;
				
				//WebSphereServerObject.Description = DescTextBox.Text;
				//WebSphereServerObject.OffHoursScanInterval = int.Parse(OffScanTextBox.Text);
				//WebSphereServerObject.ScanInterval = int.Parse(ScanTextBox.Text);
				//WebSphereServerObject.RetryInterval = int.Parse(RetryTextBox.Text);
				//WebSphereServerObject.ResponseThreshold = int.Parse(RespThrTextBox.Text);
				
				Locations LOCobject = new Locations();
				ServerTypes STypeobject = new ServerTypes();
				STypeobject.ServerType = "WebSphere";
				ServerTypes ReturnValue = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetDataForServerType(STypeobject);


				Locations ReturnLocValue = VSWebBL.SecurityBL.LocationsBL.Ins.GetDataForLocation(LOCobject);
				WebSphereServerObject.LocationId = ReturnLocValue.ID;
				

				return WebSphereServerObject;
			}
			catch (Exception ex)
			{
				
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private Servers CollectDataForWebSphereServers()
		{	
			try
				{
					Servers ServersObject = new Servers();
					ServersObject.ServerName = NameTextBox.Text;
					ServersObject.IPAddress = "111.898.2435";
					ServersObject.Description = DescTextBox.Text;
					ServerTypes STypeobject = new ServerTypes();
					STypeobject.ServerType = "WebSphere"; 
					ServerTypes ReturnValue = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetDataForServerType(STypeobject);
					ServersObject.ServerTypeID = ReturnValue.ID;
					Locations LOCobject = new Locations();
					LOCobject.Location = LocationComboBox.Text;
					Locations ReturnLocValue = VSWebBL.SecurityBL.LocationsBL.Ins.GetDataForLocation(LOCobject);
					ServersObject.LocationID = ReturnLocValue.ID;

					return ServersObject;
				}
				catch (Exception ex)
				{
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}
				finally { }
			
		}
     	protected void FormOkButton_Click(object sender, EventArgs e)
		{
			
			bool proceed = true;
			
			string errtext = "";
			int gbc = 0;
			
			if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
			{
				List<object> fieldValues = DiskGridView.GetSelectedFieldValues(new string[] { "DiskName", "Threshold", "ThresholdType" });
				if (fieldValues.Count == 0)
				{
					proceed = false;
					isValid = false;
					errtext = "You have enabled a 'Selected Disks' option on the Disk Settings tab but selected no disks " +
					"in the grid or some of the selected disks do not have a threshold or threshold type value. <br />" +
					"Please correct the disk settings in order to save your changes.";
				}
				else
				{
					foreach (object[] item in fieldValues)
					{
						if (item[1].ToString() == "" || item[2].ToString() == "")
						{
							proceed = false;
							isValid = false;
							errtext = "You have enabled a 'Selected Disks' option on the Disk Settings tab but selected no disks " +
							"in the grid or some of the selected disks do not have a threshold or threshold type value. <br />" +
							"Please correct the disk settings in order to save your changes.";
						}
					}
				}
			}
		
			if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "3")
			{
				if (GBTextBox.Text == "")
				{
					proceed = false;
					isValid = false;
					errtext = "You have enabled an 'All Disks - By GB' option on the Disk Settings tab but entered no threshold value. " +
						"You must enter a numeric threshold value in order to save your changes.";
				}
				else if (!int.TryParse(GBTextBox.Text, out gbc))
				{
					proceed = false;
					isValid = false;
					errtext = "You have enabled an 'All Disks - By GB' option on the Disk Settings tab but entered an invalid threshold value. " +
						"You must enter a numeric threshold value in order to save your changes.";
				}
			}
			if (proceed)
			{
				try
				{
					if (Mode == "Update")
					{

						UpdateWebsphereServer();
						UpdateWebsphereServer();
						UpdateServersData();
					}
					if (Mode == "Insert")
					{
						InsertWebsphereServer();
						InsertWebSphereData();
						InsertAttreibuteData();
					}
					Response.Redirect("WebSphereSeverGrid.aspx");
				}
				catch (Exception ex)
				{
				
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}
				finally { }
			}
			else
			{
				errorDiv.Style.Value = "display: block;";
	
				errorDiv.InnerHtml = errtext +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			}
		}
		private void InsertAttreibuteData()
		{

			try
			{
				DataTable returntable = VSWebBL.SecurityBL.ServersBL.Ins.GetServerNameinwebsphereservers(NameTextBox.Text);
				DataTable dt1 = VSWebBL.SecurityBL.ServersBL.Ins.GetServerIdinserverattribute(Convert.ToInt32(returntable.Rows[0]["ServerID"].ToString()));
				if (dt1.Rows.Count == 0)
				{
					object ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.InsertWebspherAttreibuteData(CollectDataforMSServers());
				}

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}

		private void InsertWebsphereServer()
		{

			try
			{
				Servers ServersObject = new Servers();
				ServersObject.ServerName = NameTextBox.Text;
				DataTable returntable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetServerName(ServersObject);
				if (returntable.Rows.Count == 0)
				{
					object ReturnValue = VSWebBL.ConfiguratorBL.WebSpherepropertiesBL.Ins.InsertDataforservers(CollectDataForWebSphereServers());
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		private void InsertWebSphereData()
		{
		
					try
					{
						DataTable returntable = VSWebBL.ConfiguratorBL.WebSpherepropertiesBL.Ins.GetServerNameinwebsphereservers(NameTextBox.Text);
						if (returntable.Rows.Count == 0)
						{
							object ReturnValue = VSWebBL.ConfiguratorBL.WebSpherepropertiesBL.Ins.InsertData(CollectDataForWebSpherePropertie());
						}		
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}

		private void UpdateServersData()
		{
			try
			{
				Object result = VSWebBL.SecurityBL.ServersBL.Ins.UpdateAttributesData(CollectDataforMSServers());
				Object result1 = VSWebBL.ConfiguratorBL.WebSpherepropertiesBL.Ins.UpdateData(CollectDataforWebSphereServices());
				List<Object> fieldValues = new List<object>();	
				Object ReturnValue;
				ReturnValue = UpdateDiskSettings();

				if (ReturnValue.ToString() == "True")
				{
					Session["WebSphereUpdateStatus"] = NameTextBox.Text;
					Response.Redirect("~/Configurator/WebSphereSeverGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					Context.ApplicationInstance.CompleteRequest();
				}

			}
			catch (Exception ex)
			{
				errorDiv.InnerHtml = "The following error has occurred: " + ex.Message;
				errorDiv.Style.Value = "display: block";
				
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		private void UpdateWebsphereServer()
		{
			try
			{

				object ReturnValue = VSWebBL.ConfiguratorBL.WebSpherepropertiesBL.Ins.UpdateDataforservers(CollectDataForWebSphereServers());
			}


			catch (Exception ex)
			{
				errorDiv.InnerHtml = "The following error has occurred: " + ex.Message;
				errorDiv.Style.Value = "display: block";

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		private bool UpdateDiskSettings()
		{
			bool ReturnValue = false;

			try
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ServerName");
				dt.Columns.Add("DiskName");
				dt.Columns.Add("Threshold");
				//5/1/2014 NS added for VSPLUS-602
				dt.Columns.Add("ThresholdType");

				dt.Columns.Add("ServerID");

				//5/1/2014 NS modified for VSPLUS-602
				List<object> fieldValues = DiskGridView.GetSelectedFieldValues(new string[] { "DiskName", "Threshold", "ThresholdType" });
				//if (DiskGridView.VisibleRowCount == fieldValues.Count)
				//12/17/2013 NS modified
				//5/12/2014 NS modified for VSPLUS-615
				if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "0")
				//if(rdbSelAll.Checked)
				{
					DataRow row = dt.Rows.Add();
					row["ServerName"] = NameTextBox.Text;
					//5/12/2014 NS modified for VSPLUS-615
					//row["DiskName"] = "0";
					//row["Threshold"] = "0";
					row["DiskName"] = "AllDisks";
					row["Threshold"] = (Convert.ToInt32(AdvDiskSpaceThTrackBar.Value)).ToString();
					//5/1/2014 NS added for VSPLUS-602
					row["ThresholdType"] = "Percent";
					row["ServerID"] = lblServerId.Text;
				}
				else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
				//else if(rdbSelFew.Checked)
				{
					foreach (object[] item in fieldValues)
					{
						DataRow row = dt.Rows.Add();
						row["ServerName"] = NameTextBox.Text;
						row["DiskName"] = item[0].ToString();
						row["Threshold"] = (item[1].ToString() != "" ? item[1].ToString() : (Convert.ToInt32(AdvDiskSpaceThTrackBar.Value)).ToString());
						//5/1/2014 NS added for VSPLUS-602
						row["ThresholdType"] = item[2].ToString();
						row["ServerID"] = lblServerId.Text;
					}
				}
				else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "2")
				//else if (rdbNoAlerts.Checked)
				{

					DataRow row = dt.Rows.Add();
					row["ServerName"] = NameTextBox.Text;
					row["DiskName"] = "NoAlerts";
					row["Threshold"] = "0";
					//5/1/2014 NS added for VSPLUS-602
					row["ThresholdType"] = "Percent";
					row["ServerID"] = lblServerId.Text;
				}
				//5/12/2014 NS added for VSPLUS-615
				else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "3")
				{
					DataRow row = dt.Rows.Add();
					row["ServerName"] = NameTextBox.Text;
					row["DiskName"] = "AllDisks";
					row["Threshold"] = GBTextBox.Text;
					row["ThresholdType"] = "GB";
					row["ServerID"] = lblServerId.Text;
				}

				//Mukund, 14Apr14 , included if condition to avoid error deleting blank records
				if (dt.Rows.Count > 0)
					ReturnValue = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.InsertSrvDiskSettingsData(dt);
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			return ReturnValue;

		}


		public ServerAttributes CollectDataforMSServers()
		{

			ServerAttributes Mobj = new ServerAttributes();
			try
			{
				DataTable dt = VSWebBL.ConfiguratorBL.WebSpherepropertiesBL.Ins.GetDataForServerID(NameTextBox.Text);
				Mobj.Enabled = EnabledCheckBox.Checked;

				Mobj.CPUThreshold = 1;// int.Parse(AdvCPUThTrackBar.Value.ToString());
				Mobj.MemThreshold = 1;// int.Parse(AdvMemoryThTrackBar.Value.ToString());
			
				Mobj.Enabled = EnabledCheckBox.Checked;

				Mobj.CPUThreshold = int.Parse(AdvCPUThTrackBar.Value.ToString());
				Mobj.MemThreshold = int.Parse(AdvMemoryThTrackBar.Value.ToString());
	
				Mobj.Category = CategoryTextBox.Text;
				Mobj.OffHourInterval = (OffscanTextBox.Text == "" ? 0 : Convert.ToInt32(OffscanTextBox.Text));
				Mobj.ScanInterval = (ScanIntvlTextBox.Text == "" ? 0 : Convert.ToInt32(ScanIntvlTextBox.Text));
				Mobj.ResponseTime = (ResponseTextBox.Text == "" ? 0 : Convert.ToInt32(ResponseTextBox.Text));
				Mobj.RetryInterval = (RetryTextBox.Text == "" ? 0 : Convert.ToInt32(RetryTextBox.Text));
				Mobj.ConsFailuresBefAlert = (ConsFailuresBefAlertTextBox.Text == "" ? 0 : Convert.ToInt32(ConsFailuresBefAlertTextBox.Text));

				Mobj.ServerId = ServerID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
				Mobj.CredentialsId = (CredentialsComboBox.Text == "" ? 0 : Convert.ToInt32(CredentialsComboBox.Value));

			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

			return Mobj;

		}


		protected void FormCancelButton_Click(object sender, EventArgs e)
		{
			if (Request.QueryString["serverid"] != null)
			{
				int id = Convert.ToInt32(Request.QueryString["serverid"]);
				Response.Redirect("~/Configurator/SametimeServer.aspx?ID=" + Convert.ToInt32(Request.QueryString["serverid"]) + " ");

			}
			else
			{
				string stype = Request.QueryString["Cat"];
				//if (Request.QueryString["Cat"] == "Websphere Server")
				//{
				//    Response.Redirect("~/Configurator/WebSphereSeverGrid.aspx", false);

				//}
				Response.Redirect("~/Configurator/WebSphereSeverGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
				Context.ApplicationInstance.CompleteRequest();
			}
		}

		private void FillDiskGridView()
		{
			try
			{
				if (lblServerId.Text != null && lblServerId.Text != "")
				{
					DataTable DiskDataTable = new DataTable();

					DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetSrvRowsDiskSettings(lblServerId.Text);
					if (DiskDataTable.Rows.Count == 0 || (DiskDataTable.Rows.Count == 1 && DiskDataTable.Rows[0]["DiskName"].ToString() == "NoAlerts"))
					{
						//12/16/2013 NS modified - created a radio button list
						//5/12/2014 NS modified for VSPLUS-615
						//SelCriteriaRadioButtonList.SelectedIndex = 2;
						SelCriteriaRadioButtonList.SelectedIndex = 3;
						AdvDiskSpaceThTrackBar.Visible = false;
						DiskLabel.Visible = false;
						Label4.Visible = false;
						DiskGridView.Visible = false;
						DiskGridInfo.Visible = false;
						//rdbSelAll.Checked = false;
						//rdbSelFew.Checked = false;
						//rdbNoAlerts.Checked = true;
						DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetSrvDiskSettings(NameTextBox.Text, "");
						//12/16/2013 NS added
						//SelectDisksRoundPanel.Visible = false;
						//SelectDisksRoundPanel.Enabled = false;
						//1/31/2014 NS added for VSPLUS-289
						infoDiskDiv.Style.Value = "display: none";
						//5/12/2014 NS added for VSPLUS-615
						GBTextBox.Visible = false;
						GBLabel.Visible = false;
						GBTitle.Visible = false;
					}
					//5/12/2014 NS modified for VSPLUS-615
					//else if (DiskDataTable.Rows.Count == 1 && DiskDataTable.Rows[0]["DiskName"].ToString() == "0")
					else if (DiskDataTable.Rows.Count == 1 && DiskDataTable.Rows[0]["DiskName"].ToString() == "AllDisks" &&
						DiskDataTable.Rows[0]["ThresholdType"].ToString() == "Percent")
					{
						//12/16/2013 NS modified - created a radio button list
						SelCriteriaRadioButtonList.SelectedIndex = 0;
						AdvDiskSpaceThTrackBar.Visible = true;
						AdvDiskSpaceThTrackBar.Value = DiskDataTable.Rows[0]["Threshold"];
						DiskLabel.Visible = true;
						Label4.Visible = true;
						DiskGridView.Visible = false;
						DiskGridInfo.Visible = false;
						//rdbSelAll.Checked = true;
						//rdbSelFew.Checked = false;
						//rdbNoAlerts.Checked = false;


						//12/16/2013 NS added
						//SelectDisksRoundPanel.Visible = false;
						//SelectDisksRoundPanel.Enabled = false;
						//1/31/2014 NS added for VSPLUS-289
						infoDiskDiv.Style.Value = "display: none";
						//5/12/2014 NS added for VSPLUS-615
						GBTextBox.Visible = false;
						GBLabel.Visible = false;
						GBTitle.Visible = false;
						DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetSrvDiskSettings(NameTextBox.Text, "All");
					}
					//5/12/2014 NS added for VSPLUS-615
					else if (DiskDataTable.Rows.Count == 1 && DiskDataTable.Rows[0]["DiskName"].ToString() == "AllDisks" &&
					  DiskDataTable.Rows[0]["ThresholdType"].ToString() == "GB")//if (DiskDataTable.Rows.Count> 0)
					{
						SelCriteriaRadioButtonList.SelectedIndex = 1;
						AdvDiskSpaceThTrackBar.Visible = false;
						DiskLabel.Visible = false;
						Label4.Visible = true;
						DiskGridView.Visible = false;
						DiskGridInfo.Visible = false;
						infoDiskDiv.Style.Value = "display: none";
						GBTextBox.Visible = true;
						GBLabel.Visible = true;
						GBTitle.Visible = true;
						if (DiskDataTable.Rows.Count > 0)
						{
							GBTextBox.Text = DiskDataTable.Rows[0]["Threshold"].ToString();
						}
						DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetSrvDiskSettings(NameTextBox.Text, "All");
					}
					else
					{
						//12/16/2013 NS modified - created a radio button list
						//5/12/2014 NS modified for VSPLUS-615
						//SelCriteriaRadioButtonList.SelectedIndex = 1;
						SelCriteriaRadioButtonList.SelectedIndex = 2;
						AdvDiskSpaceThTrackBar.Visible = false;
						DiskLabel.Visible = false;
						Label4.Visible = false;
						DiskGridView.Visible = true;
						DiskGridInfo.Visible = true;
						//rdbSelAll.Checked = false;
						//rdbSelFew.Checked = true;
						//rdbNoAlerts.Checked = false;
						DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetSrvDiskSettings(NameTextBox.Text, "");
						//12/16/2013 NS added
						//SelectDisksRoundPanel.Visible = true;
						//SelectDisksRoundPanel.Enabled = true;
						//1/31/2014 NS added for VSPLUS-289
						infoDiskDiv.Style.Value = "display: block";
						//5/12/2014 NS added for VSPLUS-615
						GBTextBox.Visible = false;
						GBLabel.Visible = false;
						GBTitle.Visible = false;
					}
					//else
					//{ 

					//}
					Session["DiskDataTable"] = DiskDataTable;
					DiskGridView.DataSource = DiskDataTable;
					DiskGridView.DataBind();
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		private void FillDiskGridfromSession()
		{
			try
			{

				DataTable ServersDataTable = new DataTable();
				if (Session["DiskDataTable"] != "" && Session["DiskDataTable"] != null)
					ServersDataTable = (DataTable)Session["DiskDataTable"];
				//5/2/2014 NS modified for VSPLUS-602
				if (ServersDataTable.Rows.Count > 0 && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
				{
					//5/2/2014 NS added for VSPLUS-602
					GridViewDataColumn column1 = DiskGridView.Columns["Threshold"] as GridViewDataColumn;
					GridViewDataColumn column2 = DiskGridView.Columns["ThresholdType"] as GridViewDataColumn;
					DataTable dt = new DataTable();
					for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
					{
						if (DiskGridView.Selection.IsRowSelected(i))
						{
							ASPxTextBox txtThreshold = (ASPxTextBox)DiskGridView.FindRowCellTemplateControl(i, column1, "txtFreeSpaceThresholdValue");
							ASPxComboBox txtThresholdType = (ASPxComboBox)DiskGridView.FindRowCellTemplateControl(i, column2, "txtFreeSpaceThresholdType");
							ServersDataTable.Rows[i]["Threshold"] = txtThreshold.Text;
							ServersDataTable.Rows[i]["ThresholdType"] = txtThresholdType.SelectedItem.Text;
						}
					}
					DiskGridView.DataSource = ServersDataTable;
					DiskGridView.DataBind();
				}
			}
			catch (Exception ex)
			{
				errorDiv.Style.Value = "display: block;";
				errorDiv.InnerHtml = "One of the selected row values is incorrect: threshold values must be numeric and threshold type must be set for each selected disk.";
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		public WebSpherePropertie CollectDataforWebSphereServices()
		{
			WebSpherePropertie WebSphereobj = new WebSpherePropertie();
			int id=Convert.ToInt32( Request.QueryString["ID"]);
			try
			{
				WebSphereobj.AvgThreadPool = (ThreadPollTextbox.Text == "" ? 0 : Convert.ToInt32(ThreadPollTextbox.Text));
				WebSphereobj.ActiveThreadCount = (ThreadCountTextBox.Text == "" ? 0 : Convert.ToInt32(ThreadCountTextBox.Text));
				WebSphereobj.CurrentHeap = HeapCurrentTextBox.Text.ToString();
				WebSphereobj.MaxHeap = MaximunHeapTextBox.Text.ToString();
				WebSphereobj.Uptime = (UpTimeTextBox.Text == "" ? 0 : Convert.ToInt32(UpTimeTextBox.Text));
				WebSphereobj.HungThreadCount = (HungThradTextBox.Text == "" ? 0 : Convert.ToInt32(HungThradTextBox.Text));
				WebSphereobj.DumpGenerated = DumpGeneratorTextBox.Text.ToString();
				WebSphereobj.ServerID = id;
				WebSphereobj.CellID = Convert.ToInt32(CellnameComboBox.Value);
				WebSphereobj.Enabled = EnabledCheckBox.Checked;
				DataTable dt = VSWebBL.ConfiguratorBL.WebSpherepropertiesBL.Ins.GetNodeID(NodenameComboBox.Text);
				WebSphereobj.NodeID = Convert.ToInt32(dt.Rows[0]["NodeID"].ToString());
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			return WebSphereobj;
		}

		protected void SelCriteriaRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
		{
			//5/12/2014 NS modified for VSPLUS-615
			if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "0")
			{
				AdvDiskSpaceThTrackBar.Visible = true;
				Label4.Visible = true;
				DiskLabel.Visible = true;
				DiskLabel.Text = "Current threshold: " + AdvDiskSpaceThTrackBar.Value + "% free space";
				DiskGridView.Visible = false;
				DiskGridInfo.Visible = false;
				for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
				{
					DiskGridView.Selection.SetSelection(i, false);
				}
				//1/31/2014 NS added for VSPLUS-289
				infoDiskDiv.Style.Value = "display: none";
				//5/12/2014 NS added for VSPLUS-615
				GBLabel.Visible = false;
				GBTextBox.Visible = false;
				GBTitle.Visible = false;
			}
			else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
			{
				DiskGridView.Visible = true;
				DiskGridInfo.Visible = true;
				AdvDiskSpaceThTrackBar.Visible = false;
				Label4.Visible = false;
				DiskLabel.Visible = false;
				//1/31/2014 NS added for VSPLUS-289
				infoDiskDiv.Style.Value = "display: block";
				//5/12/2014 NS added for VSPLUS-615
				GBLabel.Visible = false;
				GBTextBox.Visible = false;
				GBTitle.Visible = false;
			}
			else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "2")
			{
				AdvDiskSpaceThTrackBar.Visible = false;
				Label4.Visible = false;
				DiskLabel.Visible = false;
				DiskGridView.Visible = false;
				DiskGridInfo.Visible = false;
				for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
				{
					DiskGridView.Selection.SetSelection(i, false);
				}
				//1/31/2014 NS added for VSPLUS-289
				infoDiskDiv.Style.Value = "display: none";
				//5/12/2014 NS added for VSPLUS-615
				GBLabel.Visible = false;
				GBTextBox.Visible = false;
				GBTitle.Visible = false;
			}
			//5/12/2014 NS added for VSPLUS-615
			else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "3")
			{
				AdvDiskSpaceThTrackBar.Visible = false;
				Label4.Visible = true;
				DiskLabel.Visible = false;
				DiskGridView.Visible = false;
				DiskGridInfo.Visible = false;
				for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
				{
					DiskGridView.Selection.SetSelection(i, false);
				}
				infoDiskDiv.Style.Value = "display: none";
				GBLabel.Visible = true;
				GBTextBox.Visible = true;
				GBTitle.Visible = true;
			}
		}

		protected void DiskGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			//VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoProperties|DiskGridView", DiskGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			//Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void DiskGridView_PreRender(object sender, EventArgs e)
		{
			//5/1/2014 NS added for VSPLUS-427
			if (isValid)
			{
				//12/17/2013 NS modified
				//5/12/2014 NS modified for VSPLUS-615
				//if (SelCriteriaRadioButtonList.SelectedIndex == 1)
				if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
				//if (rdbSelFew.Checked)
				{
					ASPxGridView gridView = (ASPxGridView)sender;
					for (int i = 0; i < gridView.VisibleRowCount; i++)
					{
						try
						{
							gridView.Selection.SetSelection(i, (int)gridView.GetRowValues(i, "isSelected") == 1);
						}
						catch (Exception ex)
						{
							//6/27/2014 NS added for VSPLUS-634
							Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						}
					}
				}
			}
		}
		protected void DiskGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
			ASPxGridView gridView = (ASPxGridView)sender;

			DataTable DiskDataTable = (DataTable)Session["DiskDataTable"];

			DataRow dr = GetDiskRow(DiskDataTable, e.NewValues.GetEnumerator(), e.Keys[0].ToString());//Convert.ToInt32(e.Keys[0]));



			//gridView.UpdateEdit();
			Session["DiskDataTable"] = DiskDataTable;

			FillDiskGridfromSession();

			gridView.CancelEdit();
			e.Cancel = true;
		}
		protected DataRow GetDiskRow(DataTable UserObject, IDictionaryEnumerator enumerator, string Keys)// int Keys)
		{
			DataTable dataTable = UserObject;
			DataRow DRRow = dataTable.NewRow();
			if (Keys == "0")
				DRRow = dataTable.NewRow();
			else
			{
				DataRow[] SelDRRow = dataTable.Select("DiskName='" + Keys + "'");
				DRRow = SelDRRow[0];
			}
			//DRRow = dataTable.Rows.Find(Keys);
			//IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
			enumerator.Reset();
			while (enumerator.MoveNext())
				DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
			return DRRow;
		}
		protected void AdvDiskSpaceThTrackBar_ValueChanged(object sender, EventArgs e)
		{
			//12/17/2013 NS modified
			//DiskRoundPanel7.HeaderText = "Disk Space Alert at  " + AdvDiskSpaceThTrackBar.Value + "% free space";
			DiskLabel.Text = "Current threshold: " + AdvDiskSpaceThTrackBar.Value + "% free space";
		}

		protected void RolesCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			//FillWindowsServerServicesGrid();
			UpdateTabVisibility();
		}

		protected void UpdateTabVisibility()
		{
			/*if (RoleHub.Checked)
			{
				ASPxPageControlWindow.TabPages[3].ClientVisible = true;
			}
			else
			{
				ASPxPageControlWindow.TabPages[3].ClientVisible = false;
			}

			if (RoleEdge.Checked)
			{
				ASPxPageControlWindow.TabPages[4].ClientVisible = true;
			}
			else
			{
				ASPxPageControlWindow.TabPages[4].ClientVisible = false;
			}

			if (RoleCAS.Checked)
			{
				ASPxPageControlWindow.TabPages[5].ClientVisible = true;
			}
			else
			{
				ASPxPageControlWindow.TabPages[5].ClientVisible = false;
			}
			 * */
		}

		protected void UpdatePanel_Unload(object sender, EventArgs e)
		{
			System.Reflection.MethodInfo methodInfo = typeof(ScriptManager).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				.Where(i => i.Name.Equals("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel")).First();
			methodInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { sender as UpdatePanel });
		}



		protected void AdvCPUThTrackBar_ValueChanged(object sender, EventArgs e)
		{
			CPURoundPanel.HeaderText = "CPU Utilization Alert at " + AdvCPUThTrackBar.Value + "% utilization";
		}

		protected void AdvMemoryThTrackBar_PositionChanged(object sender, EventArgs e)
		{
			MemRoundPanel9.HeaderText = "Memory Usage Alert at " + AdvMemoryThTrackBar.Value + "% utilization";
		}
		private void FillCredentialsComboBox()
		{
			DataTable CredentialsDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetCredentials();
			CredentialsComboBox.DataSource = CredentialsDataTable;
			CredentialsComboBox.TextField = "AliasName";
			CredentialsComboBox.ValueField = "ID";
			CredentialsComboBox.DataBind();
		}
		protected void checkToMonitor_Init(object sender, EventArgs e)
		{
			ASPxCheckBox chk = sender as ASPxCheckBox;
			GridViewDataItemTemplateContainer container = chk.NamingContainer as GridViewDataItemTemplateContainer;

			chk.ClientSideEvents.CheckedChanged = String.Format("function (s,e) {{ cb.PerformCallback('{0}|' + s.GetChecked()); }}", container.KeyValue);
		}

		private void FillCellTypeComboBox()
		{
			DataTable WebsphereCellDataTable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetSpecificCellTypes();

			CellnameComboBox.DataSource = WebsphereCellDataTable;
			CellnameComboBox.TextField = "CellName";
			CellnameComboBox.ValueField = "CellID";
			CellnameComboBox.DataBind();
		}
		private void FillNodeTypeComboBox(int cellid)

		{
			DataTable WebsphereNodeDataTable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetNodeName(cellid);
			NodenameComboBox.DataSource = WebsphereNodeDataTable;
			NodenameComboBox.TextField = "NodeName";
			NodenameComboBox.ValueField = "NodeID";
			NodenameComboBox.DataBind();
		}
		private void FillLocationComboBox()
		{
			DataTable LocationDataTable = VSWebBL.SecurityBL.LocationsBL.Ins.GetAllData();
			LocationComboBox.DataSource = LocationDataTable;
			LocationComboBox.TextField = "Location";
			LocationComboBox.ValueField = "Location";
			LocationComboBox.DataBind();
		}
		protected void CellTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (CellnameComboBox.SelectedIndex != -1)
			{
				

				if (CellnameComboBox.SelectedItem.Text != "")
				{
					cellid = Convert.ToInt32(CellnameComboBox.SelectedItem.Value);

					FillNodeTypeComboBox(cellid);
				}
			}
			
		}
        //4/5/2016 Sowmya added for VSPLUS-2923
        protected void ASPxMenu1_ItemClick(object source, MenuItemEventArgs e)
        {
            DataTable dt;
            Session["Submenu"] = "SametimeServer";
            if (Request.QueryString["ID"] != "" && Request.QueryString["ID"] != null)
            {
                dt = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByID(Convert.ToInt32(Request.QueryString["ID"])));
                if (dt.Rows.Count > 0)
                {
                    Response.Redirect("~/Dashboard/WebSphereServerDetailsPage.aspx?Name=" + dt.Rows[0]["ServerName"].ToString() + "&Type=" + dt.Rows[0]["Type"].ToString() + "&Status=" + dt.Rows[0]["Status"].ToString() + "&LastDate=" + dt.Rows[0]["LastUpdate"].ToString(), false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
        }
	}
}