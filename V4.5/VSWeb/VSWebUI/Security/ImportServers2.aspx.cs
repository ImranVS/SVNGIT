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
    public partial class ImportServers2 : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        protected void Page_Load(object sender, EventArgs e)
        {
            //5/2/2014 NS added for VSPLUS-589
            DataTable dtattr = new DataTable();
            string attrname = "";
            string attrval = "";
            //10/14/2014 NS modified for VSPLUS-995
            if (!IsPostBack)
            {
                string profilename = Request.QueryString["Profilename"].ToString();
				dtattr = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.GetAllDataByServerType("Domino", "", profilename);
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
                            case "Cluster Replication Delays Threshold":
                                AdvClusterRepTextBox.Text = attrval;
                                break;
                            case "Pending Mail Threshold":
                                SrvAtrPendingMailThTextBox.Text = attrval;
                                break;
                            case "Dead Mail Threshold":
                                SrvAtrDeadMailThTextBox.Text = attrval;
                                break;
                            case "Held Mail Threshold":
                                SrvAtrHeldMailThTextBox.Text = attrval;
                                break;
                            case "Scan DB Health":
                                if (attrval == "1")
                                {
                                    SrvAtrDBHealthCheckBox.Checked = true;
                                }
                                break;
                            case "BES Server":
                                if (attrval == "1")
                                {
                                    AdvMonitorBESNtwrkQCheckBox.Checked = true;
                                }
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

            }
            

            //10/3/2013 NS modified to avoid server name duplication
            if (!IsPostBack)
            {
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
			//dtdisk.Columns.Add("ThresholdType");
			//dtdisk.Columns.Add("DiskInfo");
            //3/4/2014 NS added for VSPLUS-431
            Object ReturnValueDisk;

            DominoServers DominoServersObjectRet = new DominoServers();
            DominoServers DominoServersObject = new DominoServers();
            if (AdvDiskSpaceThTrackBar.Value.ToString() != null)
            {
                DominoServersObject.DiskSpaceThreshold = float.Parse(AdvDiskSpaceThTrackBar.Value.ToString()) / 100;
            }
            if (AdvMemoryThTrackBar.Value.ToString() != null)
            {
                DominoServersObject.Memory_Threshold = float.Parse(AdvMemoryThTrackBar.Value.ToString()) / 100;
            }
            if (AdvCPUThTrackBar.Value.ToString() != null)
            {
                DominoServersObject.CPU_Threshold = float.Parse(AdvCPUThTrackBar.Value.ToString()) / 100;
            }
            DominoServersObject.ScanDBHealth = SrvAtrDBHealthCheckBox.Checked;
            DominoServersObject.BES_Server = AdvMonitorBESNtwrkQCheckBox.Checked;
            if (SrvAtrPendingMailThTextBox.Text != null && SrvAtrPendingMailThTextBox.Text != "")
            {
                DominoServersObject.PendingThreshold = int.Parse(SrvAtrPendingMailThTextBox.Text);
            }
            if (SrvAtrDeadMailThTextBox.Text != null && SrvAtrDeadMailThTextBox.Text != "")
            {
                //5/15/2013 NS modified
                DominoServersObject.DeadMailDeleteThreshold = 0;
                DominoServersObject.DeadThreshold = int.Parse(SrvAtrDeadMailThTextBox.Text);
            }
            if (SrvAtrHeldMailThTextBox.Text != null && SrvAtrHeldMailThTextBox.Text != "")
            {
                DominoServersObject.HeldThreshold = int.Parse(SrvAtrHeldMailThTextBox.Text);
            }
            if (SrvAtrScanIntvlTextBox.Text != null && SrvAtrScanIntvlTextBox.Text != "")
            {
                DominoServersObject.ScanInterval = int.Parse(SrvAtrScanIntvlTextBox.Text);
            }
            if (SrvAtrOffScanIntvlTextBox.Text != null && SrvAtrOffScanIntvlTextBox.Text != "")
            {
                DominoServersObject.OffHoursScanInterval = int.Parse(SrvAtrOffScanIntvlTextBox.Text);
            }
            if (SrvAtrRetryIntvlTextBox.Text != null && SrvAtrRetryIntvlTextBox.Text != "")
            {
                DominoServersObject.RetryInterval = int.Parse(SrvAtrRetryIntvlTextBox.Text);
            }
            if (SrvAtrResponseThTextBox.Text != null && SrvAtrResponseThTextBox.Text != "")
            {
                DominoServersObject.ResponseThreshold = int.Parse(SrvAtrResponseThTextBox.Text);
            }
            if (SrvAtrFailBefAlertTextBox.Text != null && SrvAtrFailBefAlertTextBox.Text != "")
            {
                DominoServersObject.FailureThreshold = int.Parse(SrvAtrFailBefAlertTextBox.Text);
            }
            if (AdvClusterRepTextBox.Text != null && AdvClusterRepTextBox.Text != "")
            {
                DominoServersObject.Cluster_Rep_Delays_Threshold = float.Parse(AdvClusterRepTextBox.Text.ToString());
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
                for (int i = 0; i < SrvCheckBoxList.SelectedItems.Count; i++)
                {
                    DataRow dr = dtsrv.NewRow();
                    dr["ID"] = "";
                    dr["ServerName"] = SrvCheckBoxList.SelectedItems[i].ToString();
                    dr["IPAddress"] = "";
                    dr["Description"] = "Production";
                    dr["ServerType"] = "Domino";
                    dr["Location"] = "";
                    dr["LocationID"] = 0;
                    ServersObject = CollectDataForServers("Insert", dr);
                    DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetDataByName(ServersObject);
                    if (dt.Rows.Count > 0)
                    {
                        DominoServersObject.Key = int.Parse(dt.Rows[0]["ID"].ToString());
                    }
                    DominoServersObject.Category = dr["Description"].ToString();
                    DominoServersObject.Enabled = true;
                    DominoServersObjectRet = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetData(DominoServersObject);
                    //7/24/2015 NS added for VSPLUS-2013
                    DominoServersObject.ScanTravelerServer = true;
                    DominoServersObject.ScanServlet = true;
                    ReturnValue = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.UpdateData(DominoServersObject);
                    //Clearing the previous added rows
                    //(22/4/16 sowmya added for VSPLUS-2821) 
                    dtdisk.Rows.Clear();
                    //3/4/2014 NS added for VSPLUS-431
                    DataRow row = dtdisk.Rows.Add();
                    row["ServerName"] = SrvCheckBoxList.SelectedItems[i].ToString();
					//2/11/2016 Durga Added for VSPLUS 2432
					row["DiskName"] = "NoAlerts";
                    row["Threshold"] = "0";
					
                    ReturnValueDisk = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.InsertDiskSettingsData(dtdisk);
                }
                Response.Redirect("~/Security/ImportServers3.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void DoneButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Security/ImportServers3.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
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
    }
}