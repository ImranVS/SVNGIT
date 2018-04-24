using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Web.UI.HtmlControls;

using DevExpress.Web;
using DevExpress.XtraCharts;
using System.IO;
using DevExpress.XtraPrinting;
using System.Net.Mime;


namespace VSWebUI.Dashboard
{
	public partial class MobileUsers : System.Web.UI.Page
	{
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		DataRow myRow = null;
		string Duration;
		protected void Page_Load(object sender, EventArgs e)
		{
			//this.deviceTypeWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
			//this.OSTypeWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
			if (!IsPostBack)
			{
				//11/7/2014 NS added
				TravelerTrackBar.Visible = false;
				trackLabel.Visible = false;


				//2/7/2013 NS added
				//2/5/2014 NS commented out the line below for now (1.2.3) until properly tested 
				//UpdateMenuVisibility();

				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "TravelerUsersDevicesOS|UsersGrid")
						{
							UsersGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
					}
				}
			}
			if (!IsPostBack && !IsCallback)
			{
				//10/7/2013 NS moved the code to the end
				HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
				body.Attributes.Add("onload", "DoCallback()");
				body.Attributes.Add("onResize", "Resized()");

				fillDeviceTypeServer();
				fillOSTypeCombo();
				fillSyncTypeCombo();
				UsersGrid.FocusedRowIndex = -1;
				SetGrid(0, 0, 0, 2);
				SetGraphForDeviceType(SortRadioButtonList1.SelectedItem.Value.ToString(), ServerComboBox.Text);
				SetGraphForDeviceCount();
				TravelerTrackBar.Enabled = false;
				TravelerTrackBar.Value = 15;
				SetGraphForDevice_OSType(SortRadioButtonList2.SelectedItem.Value.ToString(), OSComboBox.Text);
				SetBarGraphForOSType(SortRadioButtonList2.SelectedItem.Value.ToString(), OSComboBox.Text);
				SetGraphForSyncType(SyncTypeComboBox.Text, SyncTypeSubComboBox.Text);
            }
			else
			{
				FillUserGridFromSession();

				// SetGrid(0, 0, 0, 0);
				//2/6/2013 NS moved the call below from the If block above so that the graph is set every time,
				//otherwise, when a radio button changes values on the Users tab, the chart is emptied
				SetGraphForDeviceType(SortRadioButtonList1.SelectedItem.Value.ToString(), ServerComboBox.Text);
				SetBarGraphForOSType(SortRadioButtonList2.SelectedItem.Value.ToString(), OSComboBox.Text);

			}
            //10/8/2015 NS added for VSPLUS-2242
            bool isCurrent = false;
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.GetMaxLastUpdated();
            if (dt.Rows.Count > 0)
            {
                isCurrent = Convert.ToBoolean(dt.Rows[0]["IsCurrent"].ToString());
            }
            if (!isCurrent)
            {
                lastSyncDiv.Style.Value = "display: block";
                LastSyncLabel.Text = "This information appears to be out of date";
            }
            else
            {
                lastSyncDiv.Style.Value = "display: none";
            }

		}

		public DataTable SetGrid(int lastmin, int agomin, int moreDevices, int keyUsers)
		{
			DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGrid(lastmin, agomin, moreDevices, keyUsers);
			UsersGrid.DataSource = dt;
			UsersGrid.Columns["ServerName"].Visible = true;
			if (keyUsers == 2)
				UsersGrid.Columns["ServerName"].Visible = false;
			UsersGrid.DataBind();
			if (moreDevices > 0)
			{
				//UsersGrid.ClearSort();
				UsersGrid.GroupSummarySortInfo.Clear();

				((GridViewDataColumn)UsersGrid.Columns["UserName"]).GroupBy();
				//UsersGrid.GroupBy(UsersGrid.Columns["UserName"], 0);
				ASPxGroupSummarySortInfo sortInfo = new ASPxGroupSummarySortInfo();
				sortInfo.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
				sortInfo.SummaryItem = UsersGrid.GroupSummary["UserName", DevExpress.Data.SummaryItemType.Count];
				sortInfo.GroupColumn = "UserName";

				UsersGrid.GroupSummarySortInfo.AddRange(sortInfo);
			}
			else
				UsersGrid.ClearSort();
			dt.Columns.Add("Monitor", typeof(String));
			for (int i = 0; i < dt.Rows.Count; i++)
			{

				if (dt.Rows[i]["Monitoring"].ToString() != "")
				{
					dt.Rows[i]["Monitor"] = "Yes";
				}
				else
				{
					dt.Rows[i]["Monitor"] = "No";
				}
				
			}
			Session["Fillgid"] = dt;

			return dt;
		}

		public void FillUserGridFromSession()
		{
            //10/8/2015 NS modified
            if (Session["Fillgid"] != "" && Session["Fillgid"] != null)
            {
                UsersGrid.DataSource = (DataTable)Session["Fillgid"];
                UsersGrid.DataBind();
            }
            /*
			if (travelerButtonList.Items[3].Selected == true)
			{
				SetGrid(0, Convert.ToInt32(TravelerTrackBar.Value), 1, 0);

			}
			else
			{
				if (Session["Fillgid"] != "" && Session["Fillgid"] != null)
				{
					UsersGrid.DataSource = (DataTable)Session["Fillgid"];
					UsersGrid.DataBind();
				}
			}
             */
		}

		protected void UsersGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{
			int index = UsersGrid.FocusedRowIndex;
			if (e.VisibleIndex != index)
			{
				e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';"); //'#C0C0C0'
				e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
			}
		}

		protected void TravelerusersPopupMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{
			if (e.Item.Name == "DenyAccess")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					CancelButton.Visible = false;
					// CancelButton.Text = "Cancel";
					Session["MenuItem"] = "Deny";
					DenyAccess();
				}
				else
				{
					msgPopupControl.HeaderText = "Deny Access";
					msgPopupControl.ShowOnPageLoad = true;
					msgLabel.Text = "Please select a device in the grid.";
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "WipeDevice")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					CancelButton.Text = "Cancel";
					Session["MenuItem"] = "wipe";
					//  WipeDevice Form Code 
					WipeDevice();
				}
				else
				{
					msgPopupControl.HeaderText = "Wipe Device";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "ClearWipeRequest")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					// CancelButton.Text = "Cancel";
					CancelButton.Visible = false;
					Session["MenuItem"] = "Clearwipe";
					ClearWipe();
				}
				else
				{
					msgPopupControl.HeaderText = "Clear Wipe Request";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "ChangeApproval-Deny")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					CancelButton.Visible = false;
					//CancelButton.Text = "Cancel";
					Session["MenuItem"] = "changeDeny";
					changeDeny();
				}
				else
				{
					msgPopupControl.HeaderText = "Change Approval - Deny";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "ChangeApproval-Approve")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					CancelButton.Visible = false;
					//CancelButton.Text = "Cancel";
					Session["MenuItem"] = "ChangeApprove";
					ChangeApprove();
				}
				else
				{
					msgPopupControl.HeaderText = "Change Approval - Approve";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "LogLevel-DisableFinest")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					CancelButton.Visible = false;
					// CancelButton.Text = "Cancel";
					Session["MenuItem"] = "LogDisable";
					LogDisable();
				}
				else
				{
					msgPopupControl.HeaderText = "Log Level - Disable Finest";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "LogLevel-EnableFinest")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					CancelButton.Visible = false;
					//CancelButton.Text = "Cancel";
					Session["MenuItem"] = "LogEnable";
					LogEnable();
				}
				else
				{
					msgPopupControl.HeaderText = "Log Level - Enable Finest";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "LogLevel-CreateDumpFile")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					CancelButton.Visible = false;
					// CancelButton.Text = "Cancel";
					Session["MenuItem"] = "LogCreate";
					LogCreate();
				}
				else
				{
					msgPopupControl.HeaderText = "Log Level - Create Dump File";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
		}

		public void DenyAccess()
		{
			string TellCommand = "";
			string myUserName = "";
			string myDeviceName = "";
			string myServerName = "";

			DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
			try
			{
				myUserName = myRow["UserName"].ToString();
				Session["myUserName"] = myUserName;
				myServerName = myRow["ServerName"].ToString();
				Session["myServerName"] = myServerName;
				myDeviceName = myRow["DeviceID"].ToString();
				Session["myDeviceName"] = myDeviceName;
				TellCommand = "Tell Traveler Security flagsAdd lock " + myDeviceName + " " + myUserName;
				Session["TellCommand"] = TellCommand;
			}
			catch (Exception ex)
			{
				myUserName = "";
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			msgPopupControl.ShowOnPageLoad = true;
			msgLabel.Text = "Are you sure you want to deny access to " + myUserName + " on device " + myDeviceName + "?";
		}

		public void WipeDevice()
		{
			string myServerName = "";
			string wipeSupported = "";
			string myUserName = "";
			string myDeviceName = "";
			string myDeviceID = "";

			HardCheckBox.Enabled = true;
			TravelerAppCheckBox.Enabled = true;
			StorageCadrCheckBox.Enabled = true;
			DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
			try
			{
				wipeSupported = myRow["wipeSupported"].ToString();
				myServerName = myRow["ServerName"].ToString();
				myUserName = myRow["UserName"].ToString();
				// Session["myUserName"] = myUserName;
				//  myServerName = myRow["ServerName"].ToString();
				Session["myServerName"] = myServerName;
				myDeviceName = myRow["DeviceName"].ToString();
				// Session["myDeviceName"] = myDeviceName;
				myDeviceID = myRow["DeviceID"].ToString();

				UserNameLabel.Text = myUserName;
				DeviceNameLabel.Text = myDeviceName;
				DeviceIDLabel.Text = myDeviceID;
			}
			catch (Exception ex)
			{
				wipeSupported = "1";
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			switch (wipeSupported)
			{
				case "1":
					break;
				case "2":
					HardCheckBox.Enabled = false;
					break;
				case "3":
					HardCheckBox.Enabled = true;
					TravelerAppCheckBox.Enabled = false;
					StorageCadrCheckBox.Enabled = false;
					break;
				case "4":
					HardCheckBox.Enabled = false;
					TravelerAppCheckBox.Enabled = true;
					StorageCadrCheckBox.Enabled = false;
					break;
				case "5":
					HardCheckBox.Enabled = true;
					TravelerAppCheckBox.Enabled = true;
					StorageCadrCheckBox.Enabled = false;
					break;
				default:
					HardCheckBox.Enabled = true;
					TravelerAppCheckBox.Enabled = true;
					StorageCadrCheckBox.Enabled = true;
					break;
			}
			WipePopupControl.ShowOnPageLoad = true;
		}

		public void ClearWipe()
		{
			string TellCommand = "";
			string myUserName = "";
			string myDeviceName = "";
			string myServerName = "";
			string myDeviceTitle = "";

			DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
			try
			{
				myUserName = myRow["UserName"].ToString();
				Session["myUserName"] = myUserName;
				myServerName = myRow["ServerName"].ToString();
				Session["myServerName"] = myServerName;
				myDeviceName = myRow["DeviceID"].ToString();
				//  Session["myDeviceName"] = myDeviceName;
				myDeviceTitle = myRow["DeviceID"].ToString();
				// Session["myDeviceTitle"] = myDeviceName;
				TellCommand = "Tell Traveler Security flagsRemove all " + myDeviceName + " " + myUserName;
				Session["TellCommand"] = TellCommand;
			}
			catch (Exception ex)
			{
				myUserName = "";
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			msgPopupControl.ShowOnPageLoad = true;
			msgLabel.Text = "Would you like to try to restore access for " + myUserName + " on device " + myDeviceTitle + "?" + "\n" + "Note that this will ONLY work if the device has not already been wiped.";
		}

		public void changeDeny()
		{
			string TellCommand = "";
			string myUserName = "";
			string myDeviceName = "";
			string myServerName = "";

			DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
			try
			{
				myUserName = myRow["UserName"].ToString();
				// Session["myUserName"] = myUserName;
				myServerName = myRow["ServerName"].ToString();
				// Session["myServerName"] = myServerName;
				myDeviceName = myRow["DeviceID"].ToString();
				// Session["myDeviceName"] = myDeviceName;
				TellCommand = "Tell Traveler Security approval deny " + myDeviceName + " " + myUserName;
				// Session["TellCommand"] = TellCommand;
				VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);
			}
			catch (Exception ex)
			{
				myUserName = "";
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}

		public void ChangeApprove()
		{

			string TellCommand = "";
			string myUserName = "";
			string myDeviceName = "";
			string myServerName = "";

			DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
			try
			{
				myUserName = myRow["UserName"].ToString();
				// Session["myUserName"] = myUserName;
				myServerName = myRow["ServerName"].ToString();
				// Session["myServerName"] = myServerName;
				myDeviceName = myRow["DeviceID"].ToString();
				// Session["myDeviceName"] = myDeviceName;
				TellCommand = "Tell Traveler Security approval approve " + myDeviceName + " " + myUserName;
				// Session["TellCommand"] = TellCommand;
				VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);
			}
			catch (Exception ex)
			{
				myUserName = "";
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}

		public void LogDisable()
		{
			string TellCommand = "";
			string myUserName = "";
			string myDeviceName = "";
			string myServerName = "";

			DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
			try
			{
				myUserName = myRow["UserName"].ToString();
				Session["myUserName"] = myUserName;
				myServerName = myRow["ServerName"].ToString();
				Session["myServerName"] = myServerName;
				myDeviceName = myRow["DeviceID"].ToString();
				// Session["myDeviceName"] = myDeviceName;
				TellCommand = "Tell Traveler AddUser Finest " + myUserName;
				Session["TellCommand"] = TellCommand;
			}
			catch (Exception ex)
			{
				myUserName = "";
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			msgPopupControl.ShowOnPageLoad = true;
			msgLabel.Text = "Would you like to disable finest logging level for " + myUserName + "?";
		}

		public void LogEnable()
		{
			string TellCommand = "";
			string myUserName = "";
			string myDeviceName = "";
			string myServerName = "";

			DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
			try
			{
				myUserName = myRow["UserName"].ToString();
				Session["myUserName"] = myUserName;
				myServerName = myRow["ServerName"].ToString();
				Session["myServerName"] = myServerName;
				myDeviceName = myRow["DeviceID"].ToString();
				// Session["myDeviceName"] = myDeviceName;
				TellCommand = "Tell Traveler Log RemoveUser  " + myUserName;
				Session["TellCommand"] = TellCommand;
			}
			catch (Exception ex)
			{
				myUserName = "";
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			msgPopupControl.ShowOnPageLoad = true;
			msgLabel.Text = "Would you like to enable finest logging level for " + myUserName + "?";
		}

		public void LogCreate()
		{
			string TellCommand = "";
			string myUserName = "";
			string myDeviceName = "";
			string myServerName = "";

			DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
			try
			{
				myUserName = myRow["UserName"].ToString();
				Session["myUserName"] = myUserName;
				myServerName = myRow["ServerName"].ToString();
				Session["myServerName"] = myServerName;
				myDeviceName = myRow["DeviceID"].ToString();
				Session["myDeviceName"] = myDeviceName;
				TellCommand = "Tell Traveler Dump  " + myUserName;
				Session["TellCommand"] = TellCommand;
			}
			catch (Exception ex)
			{
				myUserName = "";
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			msgPopupControl.ShowOnPageLoad = true;
			msgLabel.Text = "Would you like to create a dump file for " + myUserName + "?";
		}

		protected void YesButton_Click(object sender, EventArgs e)
		{
			// StatusText.Text = "Denying access to " & myUserName
			if (Session["MenuItem"] == "Deny")
			{
				if (Session["TellCommand"] != "" && Session["myServerName"] != "" && Session["myUserName"] != "")
				{
					msgLabel.Text = Session["TellCommand"].ToString() + "," + Session["myServerName"].ToString();
					VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), Session["TellCommand"].ToString(), Session["myUserName"].ToString());

				}
				YesButton.Visible = false;
				NOButton.Visible = false;
				CancelButton.Visible = true;
				CancelButton.Text = "OK";
				Session["myUserName"] = "";
				Session["myServerName"] = "";
				Session["myDeviceName"] = "";
				Session["TellCommand"] = "";
			}
			if (Session["MenuItem"] == "Clearwipe")
			{
				if (Session["myUserName"] != "" && Session["myUserName"] != null)
				{
					msgLabel.Text = "Restoring access for " + Session["myUserName"].ToString() + ".";
					if (Session["myServerName"] != "" && Session["TellCommand"] != "")
						VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), Session["TellCommand"].ToString(), Session["myUserName"].ToString());
				}
				YesButton.Visible = false;
				NOButton.Visible = false;
				CancelButton.Visible = true;
				CancelButton.Text = "OK";
				Session["myUserName"] = "";
				Session["myServerName"] = "";
				Session["myDeviceName"] = "";
				Session["TellCommand"] = "";
			}
			if (Session["MenuItem"] == "LogDisable" || Session["MenuItem"] == "LogEnable")
			{
				if (Session["myUserName"] != "" && Session["myUserName"] != null)
				{
					msgLabel.Text = "Updating Log Level for " + Session["myUserName"].ToString() + ".";
					if (Session["myServerName"] != "" && Session["TellCommand"] != "")
						VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), Session["TellCommand"].ToString(), Session["myUserName"].ToString());
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
				Session["myUserName"] = "";
				Session["myServerName"] = "";
				Session["myDeviceName"] = "";
				Session["TellCommand"] = "";
			}
			if (Session["MenuItem"] == "LogCreate")
			{
				if (Session["myServerName"] != "" && Session["myServerName"] != null)
					msgLabel.Text = "Created a text file on the server in the \\data\\ibm_technical_support\traveler\\logs\\dumps\\" + Session["myUserName"].ToString() + ".timedate.log file.";
				VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), Session["TellCommand"].ToString(), Session["myUserName"].ToString());
				YesButton.Visible = false;
				NOButton.Visible = false;
				CancelButton.Visible = true;
				CancelButton.Text = "OK";
				Session["myUserName"] = "";
				Session["myServerName"] = "";
				Session["myDeviceName"] = "";
				Session["TellCommand"] = "";
			}
		}

		protected void NOButton_Click(object sender, EventArgs e)
		{
			if (Session["myUserName"] != "" && Session["myUserName"] != null)
				msgLabel.Text = "No changes made to the user account " + Session["myUserName"].ToString() + ".";
			YesButton.Visible = false;
			NOButton.Visible = false;
			CancelButton.Visible = true;
			CancelButton.Text = "OK";
			Session["myUserName"] = "";
			Session["myServerName"] = "";
			Session["myDeviceName"] = "";
			Session["TellCommand"] = "";
		}

		protected void CancelButton_Click(object sender, EventArgs e)
		{
			msgPopupControl.ShowOnPageLoad = false;
		}

		protected void WipeCancelButton_Click(object sender, EventArgs e)
		{
			WipePopupControl.ShowOnPageLoad = false;
		}

		protected void WipeButton_Click(object sender, EventArgs e)
		{
			string tellCommand = "";
			string flags = "";
			if (HardCheckBox.Checked == false && TravelerAppCheckBox.Checked == false && StorageCadrCheckBox.Checked == false)
			{
				msgPopupControl.ShowOnPageLoad = true;
				msgLabel.Text = "Please select a wipe option:";
				YesButton.Visible = false;
				NOButton.Visible = false;
				CancelButton.Visible = true;
				CancelButton.Text = "OK";
			}
			if (TravelerAppCheckBox.Checked == true && HardCheckBox.Checked == false)
			{
				flags = "wipeApps ";
			}
			if (TravelerAppCheckBox.Checked == false && HardCheckBox.Checked == true && StorageCadrCheckBox.Checked == false)
			{
				flags = "wipeDevice ";
			}
			if (TravelerAppCheckBox.Checked == true && HardCheckBox.Checked == false && StorageCadrCheckBox.Checked == true)
			{
				flags = "wipeApps/wipeStorageCard";
			}
			if (TravelerAppCheckBox.Checked == true && HardCheckBox.Checked == true)
			{
				flags = "wipeDevice/wipeApps";
			}
			if (StorageCadrCheckBox.Checked == true && HardCheckBox.Checked == true)
			{
				flags = "wipeDevice/wipeStorageCard";
			}
			if (TravelerAppCheckBox.Checked == false && HardCheckBox.Checked == false && StorageCadrCheckBox.Checked == true)
			{
				flags = "wipeStorageCard ";
			}
			if (TravelerAppCheckBox.Checked == true && HardCheckBox.Checked == true && StorageCadrCheckBox.Checked == true)
			{
				flags = "wipeDevice/wipeApps/wipeStorageCard";
			}
			tellCommand += flags + DeviceIDLabel.Text + " " + UserNameLabel.Text;
			if (Session["myServerName"] != "" && Session["myServerName"] != null)
				VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), tellCommand, UserNameLabel.Text);
			WipePopupControl.ShowOnPageLoad = false;
			Session["myServerName"] = "";
		}

		protected void TravelerTrackBar_PositionChanged(object sender, EventArgs e)
		{
            //10/8/2015 NS modified
            if (ASPxNavBar1.SelectedItem.Name == "item2" && ASPxNavBar1.SelectedItem.Group.Name == "Devices")
            {
                ASPxNavBar1.SelectedItem.Text = "Devices synchronized in the last " + TravelerTrackBar.Value + " minutes";
                SetGrid(Convert.ToInt32(TravelerTrackBar.Value), 0, 0, 0);
            }
            else if (ASPxNavBar1.SelectedItem.Name == "item3" && ASPxNavBar1.SelectedItem.Group.Name == "Devices")
            {
                ASPxNavBar1.SelectedItem.Text = "Devices NOT synchronized in the last " + TravelerTrackBar.Value + " minutes";
                SetGrid(0, Convert.ToInt32(TravelerTrackBar.Value), 0, 0);
            }
            else if (ASPxNavBar1.SelectedItem.Name == "item5" && ASPxNavBar1.SelectedItem.Group.Name == "Devices")
            {
                ASPxNavBar1.SelectedItem.Text = "Devices NOT synchronized in the last " + TravelerTrackBar.Value + " days";
                SetGrid(0, (Convert.ToInt32(TravelerTrackBar.Value) * 1440), 0, 0);
            }
            /*
			if (travelerButtonList.Items[1].Selected == true)
			{
				travelerButtonList.Items[1].Text = "Show Devices that HAVE synchronized within the last " + TravelerTrackBar.Value + " minutes";
				travelerButtonList.Items[2].Text = "Show Devices that have NOT synchronized within the last " + TravelerTrackBar.Value + " minutes";
				//11/7/2014 NS commented out
				//trackLabel.Text = TravelerTrackBar.Value + " minutes";
				SetGrid(Convert.ToInt32(TravelerTrackBar.Value), 0, 0, 0);
			}
			if (travelerButtonList.Items[2].Selected == true)
			{
				travelerButtonList.Items[1].Text = "Show Devices that HAVE synchronized within the last " + TravelerTrackBar.Value + " minutes";
				travelerButtonList.Items[2].Text = "Show Devices that have NOT synchronized within the last " + TravelerTrackBar.Value + " minutes";
				//11/7/2014 NS commented out
				//trackLabel.Text = TravelerTrackBar.Value + " minutes";
				SetGrid(0, Convert.ToInt32(TravelerTrackBar.Value), 0, 0);
			}

			if (travelerButtonList.Items[6].Selected == true)
			{
				travelerButtonList.Items[6].Text = "Show Devices that HAVE NOT synchronized within the last " + TravelerTrackBar.Value + " Days";
				//11/7/2014 NS commented out
				//trackLabel.Text = TravelerTrackBar.Value + " minutes";
				SetGrid(0, (Convert.ToInt32(TravelerTrackBar.Value) * 1440), 0, 0);
			}
            */
		}

		protected void UserDetailsMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{
			if (e.Item.Name == "DenyAccess")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					CancelButton.Visible = false;
					// CancelButton.Text = "Cancel";
					Session["MenuItem"] = "Deny";
					DenyAccess();
				}
				else
				{
					msgPopupControl.HeaderText = "Deny Access";
					msgPopupControl.ShowOnPageLoad = true;
					msgLabel.Text = "Please select a device in the grid.";
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "WipeDevice")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					CancelButton.Text = "Cancel";
					Session["MenuItem"] = "wipe";
					//  WipeDevice Form Code 
					WipeDevice();
				}
				else
				{
					msgPopupControl.HeaderText = "Wipe Device";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "ClearWipeRequest")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					// CancelButton.Text = "Cancel";
					CancelButton.Visible = false;
					Session["MenuItem"] = "Clearwipe";
					ClearWipe();
				}
				else
				{
					msgPopupControl.HeaderText = "Clear Wipe Request";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "ChangeApproval-Deny")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					CancelButton.Visible = false;
					//CancelButton.Text = "Cancel";
					Session["MenuItem"] = "changeDeny";
					changeDeny();
				}
				else
				{
					msgPopupControl.HeaderText = "Change Approval - Deny";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "ChangeApproval-Approve")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					CancelButton.Visible = false;
					//CancelButton.Text = "Cancel";
					Session["MenuItem"] = "ChangeApprove";
					ChangeApprove();
				}
				else
				{
					msgPopupControl.HeaderText = "Change Approval - Approve";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "LogLevel-DisableFinest")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					CancelButton.Visible = false;
					// CancelButton.Text = "Cancel";
					Session["MenuItem"] = "LogDisable";
					LogDisable();
				}
				else
				{
					msgPopupControl.HeaderText = "Log Level - Disable Finest";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "LogLevel-EnableFinest")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					CancelButton.Visible = false;
					//CancelButton.Text = "Cancel";
					Session["MenuItem"] = "LogEnable";
					LogEnable();
				}
				else
				{
					msgPopupControl.HeaderText = "Log Level - Enable Finest";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			if (e.Item.Name == "LogLevel-CreateDumpFile")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					YesButton.Visible = true;
					NOButton.Visible = true;
					CancelButton.Visible = false;
					// CancelButton.Text = "Cancel";
					Session["MenuItem"] = "LogCreate";
					LogCreate();
				}
				else
				{
					msgPopupControl.HeaderText = "Log Level - Create Dump File";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
		}

		protected void UsersGrid_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("TravelerUsersDevicesOS|UsersGrid", UsersGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}

		public DataTable SetGraphForDeviceType(string SortField, string servername)
		{

			deviceTypeWebChart.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForDeviceType(SortField, servername);

			Series series = new Series("DeviceType", ViewType.Bar);

			series.ArgumentDataMember = dt.Columns["DeviceName"].ToString();
			//10/30/2013 NS added - point labels on series
			series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["No_of_Users"].ToString());
			deviceTypeWebChart.Series.Add(series);

			deviceTypeWebChart.Legend.Visible = false;

			((SideBySideBarSeriesView)series.View).ColorEach = true;

			//AxisBase axisx = ((XYDiagram)deviceTypeWebChart.Diagram).AxisX;

			XYDiagram seriesXY = (XYDiagram)deviceTypeWebChart.Diagram;
			// seriesXY.AxisX.Title.Text = "Milliseconds";
			// seriesXY.AxisX.Title.Visible = true;
			((DevExpress.XtraCharts.XYDiagram)deviceTypeWebChart.Diagram).Rotated = true;
			//11/16/2013 NS modified the AllowRotate option to false, otherwise the server names were unreadable
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
			seriesXY.AxisY.Title.Text = "Users";
			seriesXY.AxisY.Title.Visible = true;

			AxisBase axisy = ((XYDiagram)deviceTypeWebChart.Diagram).AxisY;
			//4/18/2014 NS modified for VSPLUS-312
			axisy.Range.AlwaysShowZeroLevel = true;
			double min = Convert.ToDouble(((XYDiagram)deviceTypeWebChart.Diagram).AxisY.Range.MinValue);
			double max = Convert.ToDouble(((XYDiagram)deviceTypeWebChart.Diagram).AxisY.Range.MaxValue);

			int gs = (int)((max - min) / 5);

			if (gs == 0)
			{
				gs = 1;
				seriesXY.AxisY.GridSpacingAuto = false;
				seriesXY.AxisY.GridSpacing = gs;
			}
			else
			{
				seriesXY.AxisY.GridSpacingAuto = true;
			}

			//2/6/2013 NS modified chart height calculations based on the number of rows
			if (dt.Rows.Count > 0)
			{
				if (dt.Rows.Count < 4)
				{
					deviceTypeWebChart.Height = 200;
				}
				else
				{
					if (dt.Rows.Count >= 4 && dt.Rows.Count < 10)
					{
						deviceTypeWebChart.Height = ((dt.Rows.Count) * 50) + 20;
					}
					else
					{
						if (dt.Rows.Count >= 10 && dt.Rows.Count < 100)
						{
							deviceTypeWebChart.Height = ((dt.Rows.Count) * 40) + 20;
						}
						else
						{
							if (dt.Rows.Count >= 100)
							{
								deviceTypeWebChart.Height = ((dt.Rows.Count) * 20) + 20;
							}
						}
					}
				}
			}
			//ResponseWebChartControl.Width = new Unit(1000);
			deviceTypeWebChart.DataSource = dt;
			deviceTypeWebChart.DataBind();
			ChartTitle title = new ChartTitle();
			if (servername == "All")
			{
				title.Text = "Mobile Devices for All Servers";
			}
			else
			{
				title.Text = "Mobile Devices for " + servername;
			}
			deviceTypeWebChart.Titles.Clear();
			deviceTypeWebChart.Titles.Add(title);
			return dt;
		}
		protected void SortRadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetGraphForDeviceType(SortRadioButtonList1.SelectedItem.Value.ToString(), ServerComboBox.Text);
		}
		protected void SortRadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetBarGraphForOSType(SortRadioButtonList2.SelectedItem.Value.ToString(), OSComboBox.Text);
		}
		public DataTable SetGraphForDevice_OSType(string SortField, string servername)
		{
			OSTypeWebChartControl.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForDevice_OSType(SortField, servername, "0");
			Series series = new Series("OSType", ViewType.Pie);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				series.Points.Add(new SeriesPoint(dt.Rows[i]["OS_Type_Min"], dt.Rows[i]["No_of_Users"]));
			}
			OSTypeWebChartControl.Series.Add(series);
			series.Label.PointOptions.PointView = PointView.Argument;
			series.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
			//series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.General;
			// series.Label.PointOptions.ValueNumericOptions.Precision = 0;

			OSTypeWebChartControl.Legend.Visible = false;
			ChartTitle title = new ChartTitle();
			if (servername == "All")
			{
				title.Text = "Mobile Devices OS for All Servers";
			}
			else
			{
				title.Text = "Mobile Devices OS for " + servername;
			}
			OSTypeWebChartControl.Titles.Clear();
			OSTypeWebChartControl.Titles.Add(title);

			OSTypeWebChartControl.DataSource = dt;
			OSTypeWebChartControl.DataBind();
			return dt;
		}

		public DataTable SetBarGraphForOSType(string SortField, string servername)
		{

			OSBarChart1.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForDevice_OSType(SortField, servername, "1");

			Series series = new Series("usercount", ViewType.Bar);

			series.ArgumentDataMember = dt.Columns["OS_Type_Min"].ToString();
			//10/30/2013 NS added - point labels on series
			series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

			ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
			seriesValueDataMembers.AddRange(dt.Columns["No_of_Users"].ToString());
			OSBarChart1.Series.Add(series);

			OSBarChart1.Legend.Visible = false;

			((SideBySideBarSeriesView)series.View).ColorEach = true;

			//AxisBase axisx = ((XYDiagram)deviceTypeWebChart.Diagram).AxisX;

			XYDiagram seriesXY = (XYDiagram)OSBarChart1.Diagram;
			// seriesXY.AxisX.Title.Text = "Milliseconds";
			// seriesXY.AxisX.Title.Visible = true;
			((DevExpress.XtraCharts.XYDiagram)OSBarChart1.Diagram).Rotated = true;
			//11/16/2013 NS modified the AllowRotate option to false, otherwise the server names were unreadable
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
			seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
			seriesXY.AxisY.Title.Text = "Users";
			seriesXY.AxisY.Title.Visible = true;

			AxisBase axisy = ((XYDiagram)OSBarChart1.Diagram).AxisY;
			//4/18/2014 NS modified for VSPLUS-312
			axisy.Range.AlwaysShowZeroLevel = true;
			double min = Convert.ToDouble(((XYDiagram)OSBarChart1.Diagram).AxisY.Range.MinValue);
			double max = Convert.ToDouble(((XYDiagram)OSBarChart1.Diagram).AxisY.Range.MaxValue);

			int gs = (int)((max - min) / 5);

			if (gs == 0)
			{
				gs = 1;
				seriesXY.AxisY.GridSpacingAuto = false;
				seriesXY.AxisY.GridSpacing = gs;
			}
			else
			{
				seriesXY.AxisY.GridSpacingAuto = true;
			}

			//2/6/2013 NS modified chart height calculations based on the number of rows
			if (dt.Rows.Count > 0)
			{
				if (dt.Rows.Count < 4)
				{
					OSBarChart1.Height = 200;
				}
				else
				{
					if (dt.Rows.Count >= 4 && dt.Rows.Count < 10)
					{
						OSBarChart1.Height = ((dt.Rows.Count) * 50) + 20;
					}
					else
					{
						if (dt.Rows.Count >= 10 && dt.Rows.Count < 100)
						{
							OSBarChart1.Height = ((dt.Rows.Count) * 40) + 20;
						}
						else
						{
							if (dt.Rows.Count >= 100)
							{
								OSBarChart1.Height = ((dt.Rows.Count) * 20) + 20;
							}
						}
					}
				}
			}
			//ResponseWebChartControl.Width = new Unit(1000);
			OSBarChart1.DataSource = dt;
			OSBarChart1.DataBind();
			ChartTitle title = new ChartTitle();
			if (servername == "All")
			{
				title.Text = "Mobile Devices OS for All Servers";
			}
			else
			{
				title.Text = "Mobile Devices OS for " + servername;
			}
			OSBarChart1.Titles.Clear();
			OSBarChart1.Titles.Add(title);
			return dt;
		}

		public void fillDeviceTypeServer()
		{
			DataTable dt = new DataTable();
			dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGrid3();
			DataRow dr = dt.NewRow();
			dr[0] = "All";
			dt.Rows.InsertAt(dr, 0);
			dt = dt.DefaultView.ToTable(true, "Name");
			ServerComboBox.DataSource = dt;
			ServerComboBox.TextField = "Name";
			ServerComboBox.ValueField = "Name";
			ServerComboBox.DataBind();
			ServerComboBox.SelectedIndex = 0;
		}

		protected void ServerComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetGraphForDeviceType(SortRadioButtonList1.SelectedItem.Value.ToString(), ServerComboBox.Text);
		}

		protected void deviceTypeWebChart_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			this.deviceTypeWebChart.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}

		protected void OSBarChart1_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			this.OSBarChart1.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}


		public void fillOSTypeCombo()
		{
			DataTable dt = new DataTable();
			dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGrid3();
			DataRow dr = dt.NewRow();
			dr[0] = "All";
			dt.Rows.InsertAt(dr, 0);
			dt = dt.DefaultView.ToTable(true, "Name");
			OSComboBox.DataSource = dt;
			OSComboBox.TextField = "Name";
			OSComboBox.ValueField = "Name";
			OSComboBox.DataBind();
			OSComboBox.SelectedIndex = 0;
		}

		protected void OSComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetGraphForDevice_OSType(SortRadioButtonList2.SelectedItem.Value.ToString(), OSComboBox.Text);
			SetBarGraphForOSType(SortRadioButtonList2.SelectedItem.Value.ToString(), OSComboBox.Text);
		}

		protected void OSTypeWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			this.OSTypeWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}

		public string SetIcon(GridViewDataItemTemplateContainer Container)
		{
			bool imgset = false;
			System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)Container.FindControl("IconImage");
			Label lbl = (Label)Container.FindControl("lblIcon");
			string lblOS = lbl.Text;
			CultureInfo culture = new CultureInfo("");

            //6/21/16 WS modified for VSPLUS-2716
            if (culture.CompareInfo.IndexOf(lblOS, "Outlook", CompareOptions.IgnoreCase) >= 0)
            {
                img.ImageUrl = "~/images/icons/winphone.png";
                imgset = true;
            }

			//8/16/2013 NS modified
			//if (lblOS.Contains("Android") == true)
			if (culture.CompareInfo.IndexOf(lblOS, "Android", CompareOptions.IgnoreCase) >= 0 && !imgset)
			{
				img.ImageUrl = "~/images/icons/android-icon.png";
				imgset = true;
			}

			//if (lblOS.Contains("Apple") == true)
			//5/6/2014 NS modified for VSPLUS-588
            //10/9/2015 NS modified for VSPLUS-2242
			if ((culture.CompareInfo.IndexOf(lblOS, "Apple", CompareOptions.IgnoreCase) >= 0 ||
				culture.CompareInfo.IndexOf(lblOS, "iOS", CompareOptions.IgnoreCase) >= 0 ||
                culture.CompareInfo.IndexOf(lblOS, "iPad", CompareOptions.IgnoreCase) >= 0 ||
                culture.CompareInfo.IndexOf(lblOS, "iPhone", CompareOptions.IgnoreCase) >= 0) && !imgset)
			{
				img.ImageUrl = "~/images/icons/os_icon_mac.png";
				imgset = true;
			}

			//8/15/2013 NS added
			//if (lblOS.Contains("RIM") == true)
            if (culture.CompareInfo.IndexOf(lblOS, "RIM", CompareOptions.IgnoreCase) >= 0 && !imgset)
			{
				img.ImageUrl = "~/images/icons/rim.png";
				imgset = true;
			}
			//if (lblOS.Contains("Win") == true)
            if (culture.CompareInfo.IndexOf(lblOS, "Win", CompareOptions.IgnoreCase) >= 0 && !imgset)
			{
				img.ImageUrl = "~/images/icons/winphone.png";
				imgset = true;
			}

			if (!imgset)
			{
				img.ImageUrl = "~/images/icons/phone.png";
				imgset = true;
			}
			return "";
		}

		protected void UsersGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{

			if (e.DataColumn.FieldName == "Security_Policy" && e.CellValue.ToString() == "No policy")
			{
				e.Cell.BackColor = System.Drawing.Color.Yellow;
				e.Cell.ForeColor = System.Drawing.Color.Black;
			}
			else if (e.DataColumn.FieldName == "Security_Policy" && e.CellValue.ToString() == "Compliant")
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
				e.Cell.ForeColor = System.Drawing.Color.Black;
			}
			else if (e.DataColumn.FieldName == "Security_Policy" && e.CellValue.ToString() == "Not Compliant")
			{
				e.Cell.BackColor = System.Drawing.Color.Red;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "Security_Policy" && e.CellValue.ToString() != "" && e.CellValue.ToString() != "No policy" && e.CellValue.ToString() != "Not Compliant")
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
				e.Cell.ForeColor = System.Drawing.Color.Black;
			}
			else if (e.DataColumn.FieldName == "Security_Policy" && e.CellValue.ToString() == "")
			{
				e.Cell.BackColor = System.Drawing.Color.Empty;
				e.Cell.ForeColor = System.Drawing.Color.Empty;
			}
			if (e.DataColumn.FieldName == "Access" && e.CellValue.ToString() == "")
			{
				e.Cell.BackColor = System.Drawing.Color.Empty;
				e.Cell.ForeColor = System.Drawing.Color.Empty;
			}
			else if (e.DataColumn.FieldName == "Access" && e.CellValue.ToString() == "Allow")
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
				e.Cell.ForeColor = System.Drawing.Color.Black;
			}
			else if (e.DataColumn.FieldName == "Access" && e.CellValue.ToString() != "Allow" && e.CellValue.ToString() != "")
			{
				e.Cell.BackColor = System.Drawing.Color.Red;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}

			if (e.DataColumn.FieldName == "UserName")
			{
				string Uname = e.CellValue.ToString();
				Uname = Uname.Replace("CN=", " ");
				Uname = Uname.Replace("O=", " ");
				Uname = Uname.Replace("OU=", " ");
				e.Cell.Text = Uname;
			}
			if (e.DataColumn.FieldName == "Monitoring" && (e.CellValue.ToString() == ""))
			{
				e.Cell.Width = 0;
			}
           
            if (ASPxNavBar1.SelectedItem.Name != "item1" && ASPxNavBar1.SelectedItem.Group.Name == "Devices")
            {
                //10/9/2015 NS added for VSPLUS-2242
                DateTime dt = DateTime.Today;
                DateTime dtnow = DateTime.Now;
                DateTime dtold = dtnow.AddHours(-4);
                if (e.DataColumn.FieldName == "LastUpdated")
                {
                    if ((DateTime)e.CellValue > dtold)
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGreen;
                        e.Cell.ForeColor = System.Drawing.Color.Black;
                    }
                    else if ((DateTime)e.CellValue <= dtold && (DateTime)e.CellValue >= dt)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                        e.Cell.ForeColor = System.Drawing.Color.Black;
                    }
                    else if ((DateTime)e.CellValue < dt)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
		}

		protected void ExportPdfButton_Click(object sender, EventArgs e)
		{
			UsersGridViewExporter.WritePdfToResponse();
		}

		protected void ExportXlsButton_Click(object sender, EventArgs e)
		{
			UsersGridViewExporter.WriteXlsToResponse();
		}

		protected void ExportXlsxButton_Click(object sender, EventArgs e)
		{
			UsersGridViewExporter.WriteXlsxToResponse();
		}
		public void fillSyncTypeCombo()
		{
			SyncTypeComboBox.Items.Add("All Devices", 0);
			SyncTypeComboBox.Items.Add("Key Users", 1);
			//SyncTypeComboBox.Items.Add("By Network", 2);
			//SyncTypeComboBox.Items.Add("By Device Type", 3);
			SyncTypeComboBox.Items.Add("By OS Type", 4);


			SyncTypeComboBox.SelectedIndex = 0;
			SyncTypeSubComboBox.Enabled = false;
		}
		protected void SyncTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			fillDeviceTypeList();
			SetGraphForSyncType(SyncTypeComboBox.Text, SyncTypeSubComboBox.Text);
		}

		protected void SyncTypeWebChartControl_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
		{
			this.SyncTypeWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
		}

		public DataTable SetGraphForSyncType(string syncType, string syncSubType)
		{
			SyncTypeWebChartControl.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForSyncType(syncType, syncSubType);
			Series series = new Series("OSType", ViewType.Pie);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				series.Points.Add(new SeriesPoint(dt.Rows[i]["Duration"], dt.Rows[i]["No_of_Users"]));
			}
			SyncTypeWebChartControl.Series.Add(series);
			series.Label.PointOptions.PointView = PointView.Argument;
			series.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
			//series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.General;
			// series.Label.PointOptions.ValueNumericOptions.Precision = 0;

			SyncTypeWebChartControl.Legend.Visible = false;
			ChartTitle title = new ChartTitle();
			if (SyncTypeSubComboBox.Enabled)
				title.Text = syncType + " - " + syncSubType;
			else
				title.Text = syncType;
			SyncTypeWebChartControl.Titles.Clear();
			SyncTypeWebChartControl.Titles.Add(title);

			SyncTypeWebChartControl.DataSource = dt;
			SyncTypeWebChartControl.DataBind();
			return dt;
		}

		public void fillDeviceTypeList()
		{
			SyncTypeSubComboBox.Items.Clear();
			SyncTypeSubComboBox.Enabled = true;
			string selectedText = SyncTypeComboBox.Text;
			if (selectedText == "By Device Type")
			{
				SyncTypeSubComboBox.Items.Add("All Device Types", 0);
				SyncTypeSubComboBox.Items.Add("Apple Tablets", 1);
				SyncTypeSubComboBox.Items.Add("Apple Phones", 2);
				SyncTypeSubComboBox.Items.Add("Samsung", 3);
				SyncTypeSubComboBox.Items.Add("All Other Devices", 4);
				SyncTypeSubComboBox.SelectedIndex = 0;
			}
			else if (selectedText == "By OS Type")
			{
				SyncTypeSubComboBox.Items.Add("All OS", 0);
				SyncTypeSubComboBox.Items.Add("iOS", 1);
				SyncTypeSubComboBox.Items.Add("Android", 2);
				SyncTypeSubComboBox.Items.Add("Win", 3);
				SyncTypeSubComboBox.Items.Add("All Other OS", 4);
				SyncTypeSubComboBox.SelectedIndex = 0;
			}
			else
			{
				SyncTypeSubComboBox.Items.Clear();
				SyncTypeSubComboBox.Enabled = false;
			}


		}


		protected void SyncTypeSubComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetGraphForSyncType(SyncTypeComboBox.Text, SyncTypeSubComboBox.Text);
		}
		public DataTable SetGraphForDeviceCount()
		{
			DeviceCountChart.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForDeviceCount();
			Series series = new Series("DeviceCount", ViewType.Pie);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				series.Points.Add(new SeriesPoint(dt.Rows[i]["Description"], dt.Rows[i]["UserCount"]));
			}
			DeviceCountChart.Series.Add(series);
			series.Label.PointOptions.PointView = PointView.Argument;
			series.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
			//series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.General;
			// series.Label.PointOptions.ValueNumericOptions.Precision = 0;

			DeviceCountChart.Legend.Visible = false;
			ChartTitle title = new ChartTitle();

			title.Text = "Device Count";

			DeviceCountChart.Titles.Clear();
			DeviceCountChart.Titles.Add(title);

			DeviceCountChart.DataSource = dt;
			DeviceCountChart.DataBind();
			return dt;
		}

		protected void StatusListMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{

		}

		protected void StatusListPopupMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{

			if (e.Item.Name == "Suspend")
			{
				if (UsersGrid.FocusedRowIndex > -1)
				{
					myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
					Session["MyDeviceId"] = myRow["DeviceID"];
					TbDuration.Text = myRow["SyncTimeThreshold"].ToString();
					SuspendPopupControl.HeaderText = "Sync Duration";
					//ASPxLabel9.Text = myRow["Name"].ToString();
					SuspendPopupControl.ShowOnPageLoad = true;

				}

			}

		}

		protected void BtnApply_Click(object sender, EventArgs e)
		{
			DataTable Mobile = new DataTable();

			//Mobile = VSWebBL.ConfiguratorBL.MobileUsersBL.Ins.GetDeviceID(Session["MyDeviceId"].ToString());

			//if(Mobile.Rows.Count > 0)
			//   {

			object ReturnValue = VSWebBL.ConfiguratorBL.MobileUsersBL.Ins.UpdateDataforDashboard(Session["MyDeviceId"].ToString(), Convert.ToInt32(TbDuration.Text));
			//  }

			//else
			//   {
			//     object ReturnValue = VSWebBL.ConfiguratorBL.MobileUsersBL.Ins.InsertData(Session["MyDeviceId"].ToString(), Convert.ToInt32(TbDuration.Text));

			//   }

			//FillMobileDevicesGrid();
			SuspendPopupControl.ShowOnPageLoad = false;
			SetGrid(0, 0, 0, 0);
		}

		protected string GetImage(object val)
		{
			if (val.ToString() == "")
			{
				return "visibility:hidden;";
			}

			return "visibility:visible;";

		}

		//protected void MobileUsersGrid_HtmalDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		//{

		//}
		//protected void MobileUsersGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		//{
		//    //int index = UsersGrid.FocusedRowIndex;
		//    //if (e.VisibleIndex != index)
		//    //{
		//    //    //e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';"); //'#C0C0C0'
		//    //    //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
		//    //}
		//    //SetGrid(0, 0, 0, 0);
		//}

		//protected void StopMonitoring(object sender, CommandEventArgs e)
		//{


		//    if (UsersGrid.FocusedRowIndex > -1)
		//    {
		//        myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
		//        String deviceId = myRow["DeviceID"].ToString();

		//        object ReturnValue = VSWebBL.ConfiguratorBL.MobileUsersBL.Ins.DeleteThresholdData(deviceId);

		//    }

		//}



		protected void ibtnMonitoring_click(object sender, EventArgs e)
		{
			ImageButton btn = (ImageButton)sender;

			string deviceId = btn.CommandArgument;
			object ReturnValue = VSWebBL.ConfiguratorBL.MobileUsersBL.Ins.DeleteThresholdData(deviceId);
			SetGrid(0, 0, 0, 0);
			//string URL = "MaintenanceWin.aspx?ID=" + Id.ToString() + "&Copy=True";
			//Response.Redirect("MaintenanceWin.aspx?ID=" + Id.ToString() + "&Copy=True", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			//Context.ApplicationInstance.CompleteRequest();
			// ASPxWebControl.RedirectOnCallback(URL);

		}

		protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{
			if (e.Item.Name == "ExportXLSItem")
			{
				UsersGridViewExporter.WriteXlsToResponse();
			}
			else if (e.Item.Name == "ExportXLSXItem")
			{
				UsersGridViewExporter.WriteXlsxToResponse();
			}
			else if (e.Item.Name == "ExportPDFItem")
			{
				UsersGridViewExporter.Landscape = true;
				using (MemoryStream ms = new MemoryStream())
				{
					PrintableComponentLink pcl = new PrintableComponentLink(new PrintingSystem());
					pcl.Component = UsersGridViewExporter;
					pcl.Margins.Left = pcl.Margins.Right = 50;
					pcl.Landscape = true;
					pcl.CreateDocument(false);
					pcl.PrintingSystem.Document.AutoFitToPagesWidth = 1;
					pcl.ExportToPdf(ms);
					WriteResponse(this.Response, ms.ToArray(), System.Net.Mime.DispositionTypeNames.Attachment.ToString());
					//ServerGridViewExporter.WritePdfToResponse();
				}
			}
		}
		//11/7/2014 NS added - the function below will format the table to fit on one page width wise
		public static void WriteResponse(HttpResponse response, byte[] filearray, string type)
		{
			response.ClearContent();
			response.Buffer = true;
			response.Cache.SetCacheability(HttpCacheability.Private);
			response.ContentType = "application/pdf";
			ContentDisposition contentDisposition = new ContentDisposition();
			contentDisposition.FileName = "MobileUsers.pdf";
			contentDisposition.DispositionType = type;
			response.AddHeader("Content-Disposition", contentDisposition.ToString());
			response.BinaryWrite(filearray);
			HttpContext.Current.ApplicationInstance.CompleteRequest();
			try
			{
				response.End();
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
		}

        protected void ASPxNavBar1_ItemClick(object source, NavBarItemEventArgs e)
        {
            if (e.Item.Name == "item1" && e.Item.Group.Name == "Users")
            {
                gridDiv.Style.Value = "display: block";
                devicesCountDiv.Style.Value = "display: none";
                devicesSrvDiv.Style.Value = "display: none";
                devicesOSDiv.Style.Value = "display: none";
                devicesSyncTimesDiv.Style.Value = "display: none";
                SetGrid(0, Convert.ToInt32(TravelerTrackBar.Value), 1, 0);
                //TravelerTrackBar.Visible = false;
                ASPxSpinEdit1.Visible = false;
                trackLabel.Visible = false;
                infoDivInactive.Style.Value = "display: none";
                infoDivPersistent.Style.Value = "display: none";
            }
            else if (e.Item.Name == "item2" && e.Item.Group.Name == "Users")
            {
                gridDiv.Style.Value = "display: block";
                devicesCountDiv.Style.Value = "display: none";
                devicesSrvDiv.Style.Value = "display: none";
                devicesOSDiv.Style.Value = "display: none";
                devicesSyncTimesDiv.Style.Value = "display: none";
                SetGrid(0, 0, 0, 1);
                //TravelerTrackBar.Visible = false;
                ASPxSpinEdit1.Visible = false;
                trackLabel.Visible = false;
                infoDivInactive.Style.Value = "display: none";
                infoDivPersistent.Style.Value = "display: none";
            }
            else if (e.Item.Name == "item1" && e.Item.Group.Name == "Devices")
            {
                gridDiv.Style.Value = "display: block";
                devicesCountDiv.Style.Value = "display: none";
                devicesSrvDiv.Style.Value = "display: none";
                devicesOSDiv.Style.Value = "display: none";
                devicesSyncTimesDiv.Style.Value = "display: none";
                SetGrid(0, 0, 0, 0);
                //TravelerTrackBar.Enabled = false;
                //TravelerTrackBar.Visible = false;
                //TravelerTrackBar.Value = 15;
                ASPxSpinEdit1.Visible = false;
                ASPxSpinEdit1.Number = 15;
                trackLabel.Visible = false;
                infoDivInactive.Style.Value = "display: block";
                infoDivPersistent.Style.Value = "display: none";
            }
            else if (e.Item.Name == "item2" && e.Item.Group.Name == "Devices")
            {
                gridDiv.Style.Value = "display: block";
                devicesCountDiv.Style.Value = "display: none";
                devicesSrvDiv.Style.Value = "display: none";
                devicesOSDiv.Style.Value = "display: none";
                devicesSyncTimesDiv.Style.Value = "display: none";
                SetGrid(Convert.ToInt32(ASPxSpinEdit1.Number), 0, 0, 0);
                trackLabel.Text = "Number of minutes:";
                ASPxSpinEdit1.Visible = true;
                ASPxSpinEdit1.MinValue = 1;
                ASPxSpinEdit1.MaxValue = 120;
                ASPxSpinEdit1.Number = 15;
                ASPxSpinEdit1.Increment = 5;
                //TravelerTrackBar.Visible = true;
                //TravelerTrackBar.Enabled = true;
                trackLabel.Visible = true;
                infoDivInactive.Style.Value = "display: none";
                infoDivPersistent.Style.Value = "display: block";
            }
            else if (e.Item.Name == "item3" && e.Item.Group.Name == "Devices")
            {
                gridDiv.Style.Value = "display: block";
                devicesCountDiv.Style.Value = "display: none";
                devicesSrvDiv.Style.Value = "display: none";
                devicesOSDiv.Style.Value = "display: none";
                devicesSyncTimesDiv.Style.Value = "display: none";
                SetGrid(0, Convert.ToInt32(ASPxSpinEdit1.Number), 0, 0);
                trackLabel.Text = "Number of minutes:";
                ASPxSpinEdit1.Visible = true;
                ASPxSpinEdit1.MinValue = 1;
                ASPxSpinEdit1.MaxValue = 120;
                ASPxSpinEdit1.Number = 15;
                ASPxSpinEdit1.Increment = 5;
                //TravelerTrackBar.Visible = true;
                //TravelerTrackBar.Enabled = true;
                trackLabel.Visible = true;
                infoDivInactive.Style.Value = "display: none";
                infoDivPersistent.Style.Value = "display: block";
            }
            else if (e.Item.Name == "item4" && e.Item.Group.Name == "Devices")
            {
                gridDiv.Style.Value = "display: block";
                devicesCountDiv.Style.Value = "display: none";
                devicesSrvDiv.Style.Value = "display: none";
                devicesOSDiv.Style.Value = "display: none";
                devicesSyncTimesDiv.Style.Value = "display: none";
                SetGrid(0, 0, 0, 2);
                //TravelerTrackBar.Visible = false;
                ASPxSpinEdit1.Visible = false;
                trackLabel.Visible = false;
                infoDivInactive.Style.Value = "display: none";
                infoDivPersistent.Style.Value = "display: block";
            }
            else if (e.Item.Name == "item5" && e.Item.Group.Name == "Devices")
            {
                gridDiv.Style.Value = "display: block";
                devicesCountDiv.Style.Value = "display: none";
                devicesSrvDiv.Style.Value = "display: none";
                devicesOSDiv.Style.Value = "display: none";
                devicesSyncTimesDiv.Style.Value = "display: none";
                SetGrid(0, 1440, 0, 0);
                trackLabel.Text = "Number of days:";
                ASPxSpinEdit1.Visible = true;
                ASPxSpinEdit1.MinValue = 1;
                ASPxSpinEdit1.MaxValue = 45;
                ASPxSpinEdit1.Number = 1;
                ASPxSpinEdit1.Increment = 1;
                /*
                TravelerTrackBar.MinValue = 1;
                TravelerTrackBar.MaxValue = 45;
                TravelerTrackBar.LargeTickStartValue = 1;
                TravelerTrackBar.Position = 1;
                TravelerTrackBar.PositionStart = 1;
                TravelerTrackBar.Step = 1;
                TravelerTrackBar.Visible = true;
                TravelerTrackBar.Enabled = true;
                */
                trackLabel.Visible = true;
                infoDivInactive.Style.Value = "display: none";
                infoDivPersistent.Style.Value = "display: block";
            }
            else if (e.Item.Name == "item6" && e.Item.Group.Name == "Devices")
            {
                devicesSrvDiv.Style.Value = "display: block";
                gridDiv.Style.Value = "display: none";
                devicesOSDiv.Style.Value = "display: none";
                devicesSyncTimesDiv.Style.Value = "display: none";
                devicesCountDiv.Style.Value = "display: none";
                infoDivInactive.Style.Value = "display: none";
                infoDivPersistent.Style.Value = "display: none";
            }
            else if (e.Item.Name == "item7" && e.Item.Group.Name == "Devices")
            {
                devicesOSDiv.Style.Value = "display: block";
                devicesSrvDiv.Style.Value = "display: none";
                gridDiv.Style.Value = "display: none";
                devicesSyncTimesDiv.Style.Value = "display: none";
                devicesCountDiv.Style.Value = "display: none";
                infoDivInactive.Style.Value = "display: none";
                infoDivPersistent.Style.Value = "display: none";
            }
            else if (e.Item.Name == "item8" && e.Item.Group.Name == "Devices")
            {
                devicesSyncTimesDiv.Style.Value = "display: block";
                devicesSrvDiv.Style.Value = "display: none";
                gridDiv.Style.Value = "display: none";
                devicesOSDiv.Style.Value = "display: none";
                devicesCountDiv.Style.Value = "display: none";
                infoDivInactive.Style.Value = "display: none";
                infoDivPersistent.Style.Value = "display: none";
            }
            else if (e.Item.Name == "item9" && e.Item.Group.Name == "Devices")
            {
                devicesCountDiv.Style.Value = "display: block";
                devicesSrvDiv.Style.Value = "display: none";
                gridDiv.Style.Value = "display: none";
                devicesOSDiv.Style.Value = "display: none";
                devicesSyncTimesDiv.Style.Value = "display: none";
                infoDivInactive.Style.Value = "display: none";
                infoDivPersistent.Style.Value = "display: none";
            }
        }

        protected void ASPxSpinEdit1_NumberChanged(object sender, EventArgs e)
        {
            if (ASPxNavBar1.SelectedItem.Name == "item2" && ASPxNavBar1.SelectedItem.Group.Name == "Devices")
            {
                ASPxNavBar1.SelectedItem.Text = "Devices synchronized in the last " + ASPxSpinEdit1.Number + " minutes";
                SetGrid(Convert.ToInt32(ASPxSpinEdit1.Number), 0, 0, 0);
            }
            else if (ASPxNavBar1.SelectedItem.Name == "item3" && ASPxNavBar1.SelectedItem.Group.Name == "Devices")
            {
                ASPxNavBar1.SelectedItem.Text = "Devices NOT synchronized in the last " + ASPxSpinEdit1.Number + " minutes";
                SetGrid(0, Convert.ToInt32(ASPxSpinEdit1.Number), 0, 0);
            }
            else if (ASPxNavBar1.SelectedItem.Name == "item5" && ASPxNavBar1.SelectedItem.Group.Name == "Devices")
            {
                ASPxNavBar1.SelectedItem.Text = "Devices NOT synchronized in the last " + ASPxSpinEdit1.Number + " days";
                SetGrid(0, (Convert.ToInt32(ASPxSpinEdit1.Number) * 1440), 0, 0);
            }
        }

		//1/21/2016 Sowjanya added for  VSPLUS-2067 ticket
		protected void UsersGrid_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
		{
			if (e.Column.FieldName == "OS_Type")
			{
				if (e.OldGroupIndex == -1)
					hdnResetTable.Value = "0";
				else if (e.OldGroupIndex == 0)
					hdnResetTable.Value = "1";
			}
		}
		protected void UsersGrid_CustomGroupDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
		{	
			if (e.Column.FieldName == "OS_Type")
			{
				if(hdnResetTable.Value == "0")
				
				{
					if (string.IsNullOrEmpty(e.DisplayText))
					{						
						e.DisplayText = "Client Build Information Not Available";
					}
					
				}
				
			}
	
		}
	}
}