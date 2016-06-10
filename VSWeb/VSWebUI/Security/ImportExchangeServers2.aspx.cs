using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebDO;

namespace VSWebUI.Security
{
    public partial class ImportExchangeServers2 : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		private string _ServerType = "";
		private string ServerType
		{
			get
			{
				if (ViewState["_ServerType"] == null)
				{
					ViewState.Add("_ServerType", _ServerType);
				}
				return ViewState["_ServerType"].ToString();
			}
			set { ViewState.Add("_ServerType", value); }
		}

        protected void Page_Load(object sender, EventArgs e)
        {
			if (Request.QueryString["ServerType"] != null)
					ServerType = Request.QueryString["ServerType"].ToString();
			if (ServerType == "DAG")
			{
				FillExchangeServersComboBox();
				CredentialsComboBox.Visible = false;
				AdvDiskSpaceThTrackBar.Visible = false;
				AdvCPUThTrackBar.Visible = false;
				AdvMemoryThTrackBar.Visible = false;
				LblCPUTH.Visible = false;
				LBLMemTh.Visible = false;
				LblDiskTh.Visible = false;
				LblCredentials.Visible = false;
				DiskLabel.Visible = false;
				CpuLabel.Visible = false;
				MemLabel.Visible = false;
			}
			else
			{
				PrimaryExchangeServerComboBox.Visible = false;
				SecondaryExchangeServerComboBox.Visible = false;
				CopyQThresholdTetBox.Visible = false;
				ReplyQthresholdTextbox.Visible = false;
			}

            //5/2/2014 NS added for VSPLUS-589
            DataTable dtattr = new DataTable();
            string attrname = "";
            string attrval = "";
			dtattr = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.GetAllDataByServerType("Exchange", "", "ProfileName");
			 //Request.QueryString["ProfileName"].ToString()
            if (dtattr.Rows.Count > 0)
            {
                for (int i = 0; i < dtattr.Rows.Count; i++)
                {
                    attrname = dtattr.Rows[i]["AttributeName"].ToString();
                    attrval = dtattr.Rows[i]["DefaultValue"].ToString();
                    switch (attrname)
                    {
                        case "Scan Interval":
                            SrvAtrScanIntvlTextBox.Text = attrval;
                            break;
                        case "Off Hours Scan Interval":
                            SrvAtrOffScanIntvlTextBox.Text = attrval;
                            break;
                        case "Retry Interval":
                            SrvAtrRetryIntvlTextBox.Text = attrval;
                            break;
                        case "Response Time Threshold":
                            SrvAtrResponseThTextBox.Text = attrval;
                            break;
                        case "Failure Threshold":
                            SrvAtrFailBefAlertTextBox.Text = attrval;
                            break;
                      
                        case "Disk Space Threshold":
                            AdvDiskSpaceThTrackBar.Position = Convert.ToInt32(Convert.ToDecimal(attrval) * 100);
                            DiskLabel.Text = AdvDiskSpaceThTrackBar.Value.ToString() + "%";
                            break;
                        case "Memory Threshold":
                            //5/23/2014 NS modified for VSPLUS-649
                            //AdvMemoryThTrackBar.Position = Convert.ToInt32(attrval);
                            AdvMemoryThTrackBar.Position = Convert.ToInt32(Convert.ToDecimal(attrval) * 100);
                            MemLabel.Text = AdvMemoryThTrackBar.Value.ToString() + "%";
                            break;
                        case "CPU Threshold":
                            //5/23/2014 NS modified for VSPLUS-649
                            //AdvCPUThTrackBar.Position = Convert.ToInt32(attrval);
                            AdvCPUThTrackBar.Position = Convert.ToInt32(Convert.ToDecimal(attrval) * 100);
                            CpuLabel.Text = AdvCPUThTrackBar.Value.ToString() + "%";
                            break;
                    }
                }
            }
            
            
            

            //10/3/2013 NS modified to avoid server name duplication
            if (!IsPostBack)
            {
				FillCredentialsComboBox();
                DataTable dt = new DataTable();
                dt = (DataTable)Session["ImportedServers"];
                SrvCheckBoxList.DataSource = dt;
                SrvCheckBoxList.TextField = "ServerName";
                SrvCheckBoxList.ValueField = "ServerName";
                SrvCheckBoxList.DataBind();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SrvLabel.Text += dt.Rows[i]["ServerName"].ToString() + ", ";
                }
                SrvLabel.Text = SrvLabel.Text.Substring(0, SrvLabel.Text.Length - 2);
            }
        }

        protected void AdvDiskSpaceThTrackBar_ValueChanged(object sender, EventArgs e)
        {
            //DiskRoundPanel7.HeaderText = "Disk Space Alert at  " + AdvDiskSpaceThTrackBar.Value + "% free space";
            DiskLabel.Text = AdvDiskSpaceThTrackBar.Value + "%";
        }

        protected void AdvMemoryThTrackBar_PositionChanged(object sender, EventArgs e)
        {
            //MemRoundPanel9.HeaderText = "Memory Usage Alert at " + AdvMemoryThTrackBar.Value + "% utilization";
            MemLabel.Text = AdvMemoryThTrackBar.Value + "%";
        }

        protected void AdvCPUThTrackBar_ValueChanged(object sender, EventArgs e)
        {
            //CPURoundPanel.HeaderText = "CPU Utilization Alert at " + AdvCPUThTrackBar.Value + "% utilization";
            CpuLabel.Text = AdvCPUThTrackBar.Value + "%";
        }

        protected void SelectAllButton_Click(object sender, EventArgs e)
        {
            SrvCheckBoxList.SelectAll();
        }

        protected void DeselectAllButton_Click(object sender, EventArgs e)
        {
            SrvCheckBoxList.UnselectAll();
        }

        protected void AssignButton_Click(object sender, EventArgs e)
        {
            Object ReturnValue;
            Servers ServersObject;
            DataTable dtsrv = new DataTable();
            DataTable dtdisk = new DataTable();
            dtdisk.Columns.Add("ServerName");
            dtdisk.Columns.Add("DiskName");
            dtdisk.Columns.Add("Threshold");
			dtdisk.Columns.Add("ThresholdType");
            //3/4/2014 NS added for VSPLUS-431
            Object ReturnValueDisk;

			ExchangeServers ExchangeServersObjectRet = new ExchangeServers();
			ExchangeServers ExchangeServersObject = new ExchangeServers();
            if (AdvDiskSpaceThTrackBar.Value.ToString() != null)
            {
                ExchangeServersObject.DiskSpaceThreshold = float.Parse(AdvDiskSpaceThTrackBar.Value.ToString()) ;
            }
            if (AdvMemoryThTrackBar.Value.ToString() != null)
            {
                ExchangeServersObject.Memory_Threshold = float.Parse(AdvMemoryThTrackBar.Value.ToString());
            }
            if (AdvCPUThTrackBar.Value.ToString() != null)
            {
                ExchangeServersObject.CPU_Threshold = float.Parse(AdvCPUThTrackBar.Value.ToString()) ;
            }
           
            if (SrvAtrScanIntvlTextBox.Text != null && SrvAtrScanIntvlTextBox.Text != "")
            {
                ExchangeServersObject.ScanInterval = int.Parse(SrvAtrScanIntvlTextBox.Text);
            }
            if (SrvAtrOffScanIntvlTextBox.Text != null && SrvAtrOffScanIntvlTextBox.Text != "")
            {
                ExchangeServersObject.OffHoursScanInterval = int.Parse(SrvAtrOffScanIntvlTextBox.Text);
            }
            if (SrvAtrRetryIntvlTextBox.Text != null && SrvAtrRetryIntvlTextBox.Text != "")
            {
                ExchangeServersObject.RetryInterval = int.Parse(SrvAtrRetryIntvlTextBox.Text);
            }
            if (SrvAtrResponseThTextBox.Text != null && SrvAtrResponseThTextBox.Text != "")
            {
                ExchangeServersObject.ResponseThreshold = int.Parse(SrvAtrResponseThTextBox.Text);
            }
            if (SrvAtrFailBefAlertTextBox.Text != null && SrvAtrFailBefAlertTextBox.Text != "")
            {
                ExchangeServersObject.FailureThreshold = int.Parse(SrvAtrFailBefAlertTextBox.Text);
            }
			ExchangeServersObject.CredentialsID = (CredentialsComboBox.Text == "" ? 0 : Convert.ToInt32(CredentialsComboBox.Value));
			if (ServerType == "DAG")
			{
				ExchangeServersObject.DAGPrimaryServerId = (PrimaryExchangeServerComboBox.Text == "" ? 0 : Convert.ToInt32(PrimaryExchangeServerComboBox.Value));
				ExchangeServersObject.DAGBackUpServerID = (SecondaryExchangeServerComboBox.Text == "" ? 0 : Convert.ToInt32(SecondaryExchangeServerComboBox.Value));
				ExchangeServersObject.DAGCopyQTh = int.Parse(CopyQThresholdTetBox.Text);
				ExchangeServersObject.DAGResponseQTh = int.Parse(ReplyQthresholdTextbox.Text);
			}
            SrvCheckBoxList.SelectAll();
            if (SrvCheckBoxList.SelectedItems.Count > 0)
            {
                dtsrv.Columns.Add("ID");
                dtsrv.Columns.Add("ServerName");
                dtsrv.Columns.Add("IPAddress");
                dtsrv.Columns.Add("Description");
                dtsrv.Columns.Add("ServerType");
                dtsrv.Columns.Add("Location");
                dtsrv.Columns.Add("LocationID");
				dtsrv.Columns.Add("PrimaryServerId");
				dtsrv.Columns.Add("BackupServerId");
                for (int i = 0; i < SrvCheckBoxList.SelectedItems.Count; i++)
                {
                    DataRow dr = dtsrv.NewRow();
                    dr["ID"] = "";
                    dr["ServerName"] = SrvCheckBoxList.SelectedItems[i].ToString();
                    dr["IPAddress"] = "";
                    dr["Description"] = "Production";
					if (ServerType == "DAG")
					{
						dr["ServerType"] = "Database Availability Group";
						dr["PrimaryServerId"] = "Exchange";
						dr["BackupServerId"] = "Exchange";
					}
					else
						dr["ServerType"] = "Exchange";
                    dr["Location"] = "";
                    dr["LocationID"] = 0;
                    ServersObject = CollectDataForServers("Insert", dr);
                    DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetDataByName(ServersObject);
                    if (dt.Rows.Count > 0)
                    {
                        ExchangeServersObject.Key = int.Parse(dt.Rows[0]["ID"].ToString());
                    }
                    ExchangeServersObject.Category = dr["Description"].ToString();
                    ExchangeServersObject.Enabled = true;
					ExchangeServersObjectRet = VSWebBL.ConfiguratorBL.ExchangePropertiesBL.Ins.GetData(ExchangeServersObject);
					if (ServerType == "DAG")
					ReturnValue = VSWebBL.ConfiguratorBL.ExchangePropertiesBL.Ins.UpdateDAGData(ExchangeServersObject);  
					else
						ReturnValue = VSWebBL.ConfiguratorBL.ExchangePropertiesBL.Ins.UpdateData(ExchangeServersObject);  
                    //3/4/2014 NS added for VSPLUS-431
					if (ServerType != "DAG")
					{
						DataRow row = dtdisk.Rows.Add();
						row["ServerName"] = SrvCheckBoxList.SelectedItems[i].ToString();
						row["DiskName"] = "AllDisks";
						row["Threshold"] = "10";
						row["ThresholdType"] = "Percent";
					}
                }
				if (ServerType != "DAG")
					ReturnValueDisk = VSWebBL.ConfiguratorBL.ExchangePropertiesBL.Ins.InsertDiskSettingsData(dtdisk);

                Response.Redirect("~/Security/ImportExchangeServers4.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void DoneButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Security/ImportExchangeServers4.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        private Servers CollectDataForServers(string Mode, DataRow ServersRow)
        {
            try
            {
                Servers ServersObject = new Servers();
                if (Mode == "Update")
                {
                    ServersObject.ID = int.Parse(ServersRow["ID"].ToString());
                }
                ServersObject.ServerName = ServersRow["ServerName"].ToString();
                ServersObject.IPAddress = ServersRow["IPAddress"].ToString();
                ServersObject.Description = ServersRow["Description"].ToString();
                ServerTypes STypeobject = new ServerTypes();
                STypeobject.ServerType = ServersRow["ServerType"].ToString();
                ServersObject.LocationID = int.Parse(ServersRow["LocationID"].ToString());
                ServerTypes ReturnValue = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetDataForServerType(STypeobject);
                ServersObject.ServerTypeID = ReturnValue.ID;

                //DataTable ReturnValue = VSWebBL.SecurityBL.AdminTabBL.Ins.GetNavigatorByDisplayText(ServersRow["ServerType"].ToString());
                //ServersObject.ServerTypeID =int.Parse(ReturnValue.Rows[0]["ID"].ToString());

                //ServersObject.ServerTypeID = ServerTypeComboBox.Text;
                // ServersObject.LocationID = int.Parse(ServersRow["LocationID"].ToString());
                //ServersObject.ServerTypeID = int.Parse(ServersRow["ServerType"].ToString());
                //ServersObject.LocationID = int.Parse(ServersRow["Location"].ToString());
                Locations LOCobject = new Locations();
                LOCobject.Location = ServersRow["Location"].ToString();

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
		private void FillCredentialsComboBox()
		{
			DataTable CredentialsDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetCredentials();
			CredentialsComboBox.DataSource = CredentialsDataTable;
			CredentialsComboBox.TextField = "AliasName";
			CredentialsComboBox.ValueField = "ID";
			CredentialsComboBox.DataBind();
			if (CredentialsComboBox.Items.Count>0)
			CredentialsComboBox.Items[CredentialsComboBox.Items.Count-1].Selected = true;

		}
		private void FillExchangeServersComboBox()
		{
			DataTable ServersDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetExchangeServers();
			PrimaryExchangeServerComboBox.DataSource = ServersDataTable;
			PrimaryExchangeServerComboBox.TextField = "ServerName";
			PrimaryExchangeServerComboBox.ValueField = "ID";
			PrimaryExchangeServerComboBox.DataBind();

			SecondaryExchangeServerComboBox.DataSource = ServersDataTable;
			SecondaryExchangeServerComboBox.TextField = "ServerName";
			SecondaryExchangeServerComboBox.ValueField = "ID";
			SecondaryExchangeServerComboBox.DataBind();
			//if (CredentialsComboBox.Items.Count > 0)
			//    CredentialsComboBox.Items[CredentialsComboBox.Items.Count - 1].Selected = true;

		}
    }
}