using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using VSWebDO;


using System.Data;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Data.SqlClient;
using System.Threading;
using System.Collections;

using DevExpress.Web.ASPxTreeList;

namespace VSWebUI.Configurator
{
    public partial class MicrosoftExchangeDAG : System.Web.UI.Page
    {
        bool isValid = true;
		bool isValidDB = true;
        protected int ServerID;
        protected int ServerTypeID;
        protected DataTable ExchangeDataTable = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            ServerTypeID = 5;
            lblServerId.Text = Request.QueryString["ID"];
            if (!IsPostBack)
            {

                 FillCredentialsComboBox();
              
                Filldata();
				fillPrmaryconnBox();
                FillSettingsdata();
                
                if (Request.QueryString["name"] != "" && Request.QueryString["name"] != null)
                {
                    NameTextBox.Text = Request.QueryString["name"].ToString();
                    LocationTextBox.Text = Request.QueryString["Loc"].ToString();
                    DescTextBox.Enabled = false;
                    NameTextBox.Enabled = false;
                    LocationTextBox.Enabled = false;
                }
				FillDatabaseGridView();
                //FillDiskGridView();
               // FillWindowsServicesGrid();
            }
            else
            {
                //Commented by Mukund 30Mar14
                // FillExchangeServerServicesGridfromSession();
               // FillDiskGridfromSession();
                FillWindowsServicesGridFromSession();
				FillDatabaseGridfromSession();
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
                string servertype = dt.Rows[0]["ServerType"].ToString();
                Session["servertype"] = servertype;
                if (dt.Rows.Count > 0)
                {
                    //ServerID =Convert.ToInt32( dt.Rows[0]["ID"].ToString());
                    //lblServerId.Text = ServerID.ToString() ;
                    //3/21/2014 NS modified
                    //lblServer.Text = ": " + dt.Rows[0]["ServerName"].ToString();
                    //11/19/2014 NS modified
                    //lblServer.Text += " - " + dt.Rows[0]["ServerName"].ToString();
                    lblServer.InnerHtml += " - " + dt.Rows[0]["ServerName"].ToString();
                    DescTextBox.Text = dt.Rows[0]["Description"].ToString();


                    if (dt.Rows[0]["Enabled"].ToString() != "" && dt.Rows[0]["Enabled"] != null)
                        EnabledCheckBox.Checked = bool.Parse(dt.Rows[0]["Enabled"].ToString());

                    RetryTextBox.Text = dt.Rows[0]["RetryInterval"].ToString();
                   // ResponseTextBox.Text = dt.Rows[0]["ResponseTime"].ToString();
                    OffscanTextBox.Text = dt.Rows[0]["OffHourInterval"].ToString();
                    //CPUThresholdTextBox.Text = dt.Rows[0]["CPU_Threshold"].ToString();
                    //MemThresholdTextBox.Text = dt.Rows[0]["MemThreshold"].ToString();
                    //AdvCPUThTrackBar.Value = dt.Rows[0]["CPU_Threshold"].ToString();
                    //CPURoundPanel.HeaderText = "CPU Utilization Alert at " + AdvCPUThTrackBar.Value + "% utilization";
                   // AdvMemoryThTrackBar.Value = dt.Rows[0]["MemThreshold"].ToString();
                   // MemRoundPanel9.HeaderText = "Memory Usage Alert at " + AdvMemoryThTrackBar.Value + "% utilization";
                    CategoryTextBox.Text = dt.Rows[0]["Category"].ToString();
                    ScanIntvlTextBox.Text = dt.Rows[0]["ScanInterval"].ToString();
                   // ServerDaysAlert.Text = dt.Rows[0]["ConsOvrThresholdBefAlert"].ToString();
                   // ConsFailuresBefAlertTextBox.Text = dt.Rows[0]["ConsFailuresBefAlert"].ToString();
                    int credentialsId = 0;
                    if (dt.Rows[0]["CredentialsId"].ToString() != null && dt.Rows[0]["CredentialsId"].ToString() != "")
                    {
                        credentialsId = Convert.ToInt32(dt.Rows[0]["CredentialsId"].ToString());
                        CredentialsComboBox.SelectedItem = CredentialsComboBox.Items.FindByValue(credentialsId);
                        //CredentialsComboBox.Value = Convert.ToInt32(dt.Rows[0]["CredentialsId"].ToString());
                        //CredentialsComboBox.SelectedItem.Value = Convert.ToInt32(dt.Rows[0]["CredentialsId"].ToString());
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
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

        }

        public void fillPrmaryconnBox()
        {
            DataTable dtprimarycombo = new DataTable();
            //string servertype = Session["servertype"].ToString();
            string servertype = "Exchange";
            dtprimarycombo = VSWebBL.SecurityBL.ServersBL.Ins.GetAllDataByType(servertype);
			PrmaryconnBox.ValueField = "ID";
			PrmaryconnBox.TextField = "TypeandLocation";
            PrmaryconnBox.DataSource = dtprimarycombo;
            PrmaryconnBox.DataBind();
			BackpcnnBox.ValueField = "ID";
			BackpcnnBox.TextField = "TypeandLocation";
            BackpcnnBox.DataSource = dtprimarycombo;
            BackpcnnBox.DataBind();
        }
        public void FillSettingsdata()
        {
            ExchangeSettings Mobj0 = new ExchangeSettings();
            Mobj0.ServerID = Convert.ToInt32(lblServerId.Text);

			DataTable dt = VSWebBL.ConfiguratorBL.DAGBL.Ins.GetAttributes(Mobj0.ServerID);

            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {


                    int Connection = 0;
					if (dt.Rows[0]["PrimaryConnection"].ToString() != null && dt.Rows[0]["PrimaryConnection"].ToString() != "")
                    {
						Connection = Convert.ToInt32(dt.Rows[0]["PrimaryConnection"].ToString());
						for (int i = 0; i < PrmaryconnBox.Items.Count; i++)
                        {
							if (PrmaryconnBox.Items[i].Value.ToString() == Connection.ToString())
								PrmaryconnBox.Items[i].Selected = true;
                        }
                    }

					if (dt.Rows[0]["BackupConnection"].ToString() != null && dt.Rows[0]["BackupConnection"].ToString() != "")
					{
						Connection = Convert.ToInt32(dt.Rows[0]["BackupConnection"].ToString());
						for (int i = 0; i < BackpcnnBox.Items.Count; i++)
						{
							if (BackpcnnBox.Items[i].Value.ToString() == Connection.ToString())
								BackpcnnBox.Items[i].Selected = true;
						}
					}
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }



        }

        //public void FillServices()
        //{
        //    try
        //    {
        //        DataTable dt = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetAllServicesByServerIdType(5,ServerID, VersionCombobox.Text);
        //        Session["Services"] = dt;
        //        ExchangeServicesGridView.DataSource = dt;
        //        ExchangeServicesGridView.DataBind();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}

        protected void FormOkButton_Click(object sender, EventArgs e)
        {
            ////5/1/2014 NS modified for VSPLUS-427
            bool proceed = true;
            ////5/12/2014 NS added for VSPLUS-615
            string errtext = "";
            int gbc = 0;
            ////5/12/2014 NS modified for VSPLUS-615
            ////if (SelCriteriaRadioButtonList.SelectedIndex == 1)
            //if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
            //{
            //    List<object> fieldValues = DiskGridView.GetSelectedFieldValues(new string[] { "DiskName", "Threshold", "ThresholdType" });
            //    if (fieldValues.Count == 0)
            //    {
            //        proceed = false;
            //        isValid = false;
            //        errtext = "You have enabled a 'Selected Disks' option on the Disk Settings tab but selected no disks " +
            //        "in the grid or some of the selected disks do not have a threshold or threshold type value. <br />" +
            //        "Please correct the disk settings in order to save your changes.";
            //    }
            //    else
            //    {
            //        foreach (object[] item in fieldValues)
            //        {
            //            if (item[1].ToString() == "" || item[2].ToString() == "")
            //            {
            //                proceed = false;
            //                isValid = false;
            //                errtext = "You have enabled a 'Selected Disks' option on the Disk Settings tab but selected no disks " +
            //                "in the grid or some of the selected disks do not have a threshold or threshold type value. <br />" +
            //                "Please correct the disk settings in order to save your changes.";
            //            }
            //        }
            //    }
            //}
            ////5/12/2014 NS added for VSPLUS-615
            //if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "3")
            //{
            //    if (GBTextBox.Text == "")
            //    {
            //        proceed = false;
            //        isValid = false;
            //        errtext = "You have enabled an 'All Disks - By GB' option on the Disk Settings tab but entered no threshold value. " +
            //            "You must enter a numeric threshold value in order to save your changes.";
            //    }
            //    else if (!int.TryParse(GBTextBox.Text, out gbc))
            //    {
            //        proceed = false;
            //        isValid = false;
            //        errtext = "You have enabled an 'All Disks - By GB' option on the Disk Settings tab but entered an invalid threshold value. " +
            //            "You must enter a numeric threshold value in order to save your changes.";
            //    }
            //}

			if (SelCriteriaDBRadioButtonList.SelectedItem.Value.ToString() == "1")
			{
				List<object> fieldValues = DatabaseGridView.GetSelectedFieldValues(new string[] { "DatabaseName", "CopyQueueThreshold", "ReplayQueueThreshold" });
				if (fieldValues.Count == 0)
				{
					proceed = false;
					isValidDB = false;
					errtext = "You have enabled a 'Selected Database' option on the Database Settings tab but selected no databases " +
					"in the grid or some of the selected databases do not have a threshold. <br />" +
					"Please correct the database settings in order to save your changes.";
				}
				else
				{
					foreach (object[] item in fieldValues)
					{
						if (item[1].ToString() == "" || item[2].ToString() == "")
						{
							proceed = false;
							isValidDB = false;
							errtext = "You have enabled a 'Selected Database' option on the Database Settings tab but selected no databases " +
							"in the grid or some of the selected databases do not have a threshold. <br />" +
							"Please correct the databases settings in order to save your changes.";
						}
					}
				}
			}

			if (SelCriteriaDBRadioButtonList.SelectedItem.Value.ToString() == "0")
			{
				if (ReplayTextBox.Text == "")
				{
					proceed = false;
					isValidDB = false;
					errtext = "You have enabled an 'All Databases' option on the Database Settings tab but entered no threshold value for Replay Queue. " +
						"You must enter a numeric threshold value in order to save your changes.";
				}
				else if (!int.TryParse(ReplayTextBox.Text, out gbc))
				{
					proceed = false;
					isValidDB = false;
					errtext = "You have enabled an 'All Database' option on the Database Settings tab but entered an invalid Replay Queue threshold value. " +
						"You must enter a numeric threshold value in order to save your changes.";
				}
				if (CopyTextBox.Text == "")
				{
					proceed = false;
					isValidDB = false;
					errtext = "You have enabled an 'All Database' option on the Database Settings tab but entered no threshold value for Copy Queue. " +
						"You must enter a numeric threshold value in order to save your changes.";
				}
				else if (!int.TryParse(CopyTextBox.Text, out gbc))
				{
					proceed = false;
					isValidDB = false;
					errtext = "You have enabled an 'All Database' option on the Database Settings tab but entered an invalid Copy Queue threshold value. " +
						"You must enter a numeric threshold value in order to save your changes.";
				}
			}

			if (PrmaryconnBox.Text == "" && EnabledCheckBox.Checked)
			{
				proceed = false;
				isValidDB = false;
				errtext = "You must have a Primary Server selected in order to proceed with enabling the server.";
			}

            if (proceed)
            {
                try
                {
                    //DataTable dt = GetDataView(DiskGridView);
                    //Save Server Attributes Tab
                    UpdateServersData();
                    updatedagsettingsdata();
					UpdateDatabaseSettings();
					Session["DAGUpdateStatus"] = NameTextBox.Text;
					Response.Redirect("~/Configurator/DAGServerGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					Context.ApplicationInstance.CompleteRequest();
				

                }
                catch (Exception ex)
                {
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }
                finally { }
            }
            else
            {
                errorDiv.Style.Value = "display: block;";
                errorDiv.InnerHtml = errtext;
            }
        }
        private void updatedagsettingsdata()
        {
            DagSettings Dobj = new DagSettings();
            try
            {
                Dobj.ServerID = Convert.ToInt32(Request.QueryString["ID"]);
				Dobj.PrimaryConnection = PrmaryconnBox.SelectedItem == null ? "" : PrmaryconnBox.SelectedItem.Value.ToString();
				Dobj.BackupConnection = BackpcnnBox.SelectedItem == null ? "" : BackpcnnBox.SelectedItem.Value.ToString();
                Object resuilt2 = VSWebBL.ConfiguratorBL.DAGBL.Ins.DagsettingsData(Dobj);
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
               // Object result1 = VSWebBL.ExchangeBAL.Ins.UpdateExchangeSettingsData(CollectDataforExchangeSettings());
                //result1 = VSWebBL.ExchangeBAL.Ins.UpdateServerRolesData(lblServerId.Text, RoleHub.Checked, RoleMailBox.Checked, RoleCAS.Checked, RoleEdge.Checked, RoleUnified.Checked);


                List<object> fieldValues = new List<object>();

                DataTable dt = GetSelectedServices();
                List<DataRow> servicesSelected = dt.AsEnumerable().ToList();

                if (servicesSelected.Count > 0)
                {
                    foreach (DataRow row in servicesSelected)
                    {
                        if (row["id"] != null || row["id"] != "")
                            fieldValues.Add(row["id"]);
                    }
                }

                Object result2 = VSWebBL.ConfiguratorBL.ServicesBL.Ins.UpdateWindowsServices(Request.QueryString["name"].ToString(), fieldValues);
              

            }
            catch (Exception ex)
            {
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message;
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        //private bool UpdateDiskSettings()
        //{
        //    bool ReturnValue = false;

        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("ServerName");
        //        dt.Columns.Add("DiskName");
        //        dt.Columns.Add("Threshold");
        //        //5/1/2014 NS added for VSPLUS-602
        //        dt.Columns.Add("ThresholdType");

        //        dt.Columns.Add("ServerID");

        //        //5/1/2014 NS modified for VSPLUS-602
        //        List<object> fieldValues = DiskGridView.GetSelectedFieldValues(new string[] { "DiskName", "Threshold", "ThresholdType" });
        //        //if (DiskGridView.VisibleRowCount == fieldValues.Count)
        //        //12/17/2013 NS modified
        //        //5/12/2014 NS modified for VSPLUS-615
        //        if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "0")
        //        //if(rdbSelAll.Checked)
        //        {
        //            DataRow row = dt.Rows.Add();
        //            row["ServerName"] = NameTextBox.Text;
        //            //5/12/2014 NS modified for VSPLUS-615
        //            //row["DiskName"] = "0";
        //            //row["Threshold"] = "0";
        //            row["DiskName"] = "AllDisks";
        //            row["Threshold"] = (Convert.ToInt32(AdvDiskSpaceThTrackBar.Value)).ToString();
        //            //5/1/2014 NS added for VSPLUS-602
        //            row["ThresholdType"] = "Percent";
        //            row["ServerID"] = lblServerId.Text;
        //        }
        //        else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
        //        //else if(rdbSelFew.Checked)
        //        {
        //            foreach (object[] item in fieldValues)
        //            {
        //                DataRow row = dt.Rows.Add();
        //                row["ServerName"] = NameTextBox.Text;
        //                row["DiskName"] = item[0].ToString();
        //                row["Threshold"] = (item[1].ToString() != "" ? item[1].ToString() : (Convert.ToInt32(AdvDiskSpaceThTrackBar.Value)).ToString());
        //                //5/1/2014 NS added for VSPLUS-602
        //                row["ThresholdType"] = item[2].ToString();
        //                row["ServerID"] = lblServerId.Text;
        //            }
        //        }
        //        else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "2")
        //        //else if (rdbNoAlerts.Checked)
        //        {

        //            DataRow row = dt.Rows.Add();
        //            row["ServerName"] = NameTextBox.Text;
        //            row["DiskName"] = "NoAlerts";
        //            row["Threshold"] = "0";
        //            //5/1/2014 NS added for VSPLUS-602
        //            row["ThresholdType"] = "Percent";
        //            row["ServerID"] = lblServerId.Text;
        //        }
        //        //5/12/2014 NS added for VSPLUS-615
        //        else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "3")
        //        {
        //            DataRow row = dt.Rows.Add();
        //            row["ServerName"] = NameTextBox.Text;
        //            row["DiskName"] = "AllDisks";
        //            row["Threshold"] = GBTextBox.Text;
        //            row["ThresholdType"] = "GB";
        //            row["ServerID"] = lblServerId.Text;
        //        }

        //        //Mukund, 14Apr14 , included if condition to avoid error deleting blank records
        //        if (dt.Rows.Count > 0)
        //            ReturnValue = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.InsertSrvDiskSettingsData(dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        //6/27/2014 NS added for VSPLUS-634
        //        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
        //        throw ex;
        //    }
        //    return ReturnValue;

        //}

        //public ExchangeSettings CollectDataforExchangeSettings()
        //{
        //    ExchangeSettings Mobj = new ExchangeSettings();
        //    try
        //    {

        //        Mobj.CASSmtp = CASSmtp.Checked;
        //        Mobj.CASPop3 = CASPop3.Checked;
        //        Mobj.CASImap = CASImap.Checked;
        //        Mobj.CASOARPC = CASOARPC.Checked;
        //        Mobj.CASOWA = CASOWA.Checked;
        //        Mobj.CASActiveSync = CASActiveSync.Checked;
        //        Mobj.CASEWS = CASEWS.Checked;
        //        Mobj.CASECP = CASECP.Checked;
        //        Mobj.CASAutoDiscovery = CASAutoDiscovery.Checked;
        //        Mobj.CASOAB = CASOAB.Checked;
        //        Mobj.SubQThreshold = (SubQThreshold.Text == "" ? 0 : Convert.ToInt32(SubQThreshold.Text));
        //        Mobj.PoisonQThreshold = (PoisonQThreshold.Text == "" ? 0 : Convert.ToInt32(PoisonQThreshold.Text));
        //        Mobj.UnReachableQThreshold = (UnReachableQThreshold.Text == "" ? 0 : Convert.ToInt32(UnReachableQThreshold.Text));
        //        Mobj.TotalQThreshold = (TotalQThreshold.Text == "" ? 0 : Convert.ToInt32(TotalQThreshold.Text));
        //        //Mobj.EdgeSubQThreshold = (EdgeSubQThreshold.Text == "" ? 0 : Convert.ToInt32(EdgeSubQThreshold.Text));
        //        //Mobj.EdgePosQThreshold = (EdgePosQThreshold.Text == "" ? 0 : Convert.ToInt32(EdgePosQThreshold.Text));
        //        //Mobj.EdgeUnReachableQThreshold = (EdgeUnReachableQThreshold.Text == "" ? 0 : Convert.ToInt32(EdgeUnReachableQThreshold.Text));
        //        //Mobj.EdgeTotalQThreshold = (EdgeTotalQThreshold.Text == "" ? 0 : Convert.ToInt32(EdgeTotalQThreshold.Text));
        //        Mobj.ServerID = (RetryTextBox.Text == "" ? 0 : Convert.ToInt32(lblServerId.Text));
        //        //Mobj.VersionNo = VersionCombobox.Text;
        //        Mobj.ACCredentialsId = (ActiveSyncCredentialsComboBox.Text == "" ? 0 : Convert.ToInt32(ActiveSyncCredentialsComboBox.Value));


        //    }
        //    catch (Exception ex)
        //    {
        //        //6/27/2014 NS added for VSPLUS-634
        //        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
        //        throw ex;
        //    }

        //    return Mobj;

        //}
        public ServerAttributes CollectDataforMSServers()
        {

            ServerAttributes Mobj = new ServerAttributes();
            try
            {
                Mobj.Enabled = EnabledCheckBox.Checked;
                //Mobj.ScanDAGHealth = ASPxCheckBoxDAG.Checked;
               // Mobj.ScanDAGHealth = ASPxCheckBoxDAG.Checked;
                Mobj.CPUThreshold = 1;// int.Parse(AdvCPUThTrackBar.Value.ToString());
                Mobj.MemThreshold = 1;// int.Parse(AdvMemoryThTrackBar.Value.ToString());
                Mobj.Category = CategoryTextBox.Text;
                Mobj.OffHourInterval = (OffscanTextBox.Text == "" ? 0 : Convert.ToInt32(OffscanTextBox.Text));
                Mobj.ScanInterval = (ScanIntvlTextBox.Text == "" ? 0 : Convert.ToInt32(ScanIntvlTextBox.Text));
              //  Mobj.ResponseTime = (ResponseTextBox.Text == "" ? 0 : Convert.ToInt32(ResponseTextBox.Text));
                Mobj.RetryInterval = (RetryTextBox.Text == "" ? 0 : Convert.ToInt32(RetryTextBox.Text));
               // Mobj.ConsFailuresBefAlert = (ConsFailuresBefAlertTextBox.Text == "" ? 0 : Convert.ToInt32(ConsFailuresBefAlertTextBox.Text));
               // Mobj.ConsOvrThresholdBefAlert = (ServerDaysAlert.Text == "" ? 0 : Convert.ToInt32(ServerDaysAlert.Text));
                Mobj.ServerId = Convert.ToInt32(lblServerId.Text);
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

       
        protected void VersionCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {

            //FillExchangeServerServicesGrid();
             UpdateTabVisibility();

        }

        //protected void RoleHub_CheckedChanged(object sender, EventArgs e)
        //{
        //    FillExchangeServerServicesGrid();

        //}

        //protected void RoleMailBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    FillExchangeServerServicesGrid();

        //}

        //protected void RoleCAS_CheckedChanged(object sender, EventArgs e)
        //{
        //    FillExchangeServerServicesGrid();

        //}

        //protected void RoleEdge_CheckedChanged(object sender, EventArgs e)
        //{
        //    FillExchangeServerServicesGrid();

        //}

     protected void ExchangeServicesGridView_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }


        protected void FormCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/DAGserverGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        //Mukund 12Jun14:  VE-4	: Implement Disk Checking - Front End
        //private void FillDiskGridView()
        //{
        //    try
        //    {

        //        DataTable DiskDataTable = new DataTable();

        //        DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetSrvRowsDiskSettings(lblServerId.Text);
        //        if (DiskDataTable.Rows.Count == 0 || (DiskDataTable.Rows.Count == 1 && DiskDataTable.Rows[0]["DiskName"].ToString() == "NoAlerts"))
        //        {
        //            //12/16/2013 NS modified - created a radio button list
        //            //5/12/2014 NS modified for VSPLUS-615
        //            //SelCriteriaRadioButtonList.SelectedIndex = 2;
        //            SelCriteriaRadioButtonList.SelectedIndex = 3;
        //            AdvDiskSpaceThTrackBar.Visible = false;
        //            DiskLabel.Visible = false;
        //            Label4.Visible = false;
        //            DiskGridView.Visible = false;
        //            DiskGridInfo.Visible = false;
        //            //rdbSelAll.Checked = false;
        //            //rdbSelFew.Checked = false;
        //            //rdbNoAlerts.Checked = true;
        //            DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetSrvDiskSettings(NameTextBox.Text, "");
        //            //12/16/2013 NS added
        //            //SelectDisksRoundPanel.Visible = false;
        //            //SelectDisksRoundPanel.Enabled = false;
        //            //1/31/2014 NS added for VSPLUS-289
        //            infoDiskDiv.Style.Value = "display: none";
        //            //5/12/2014 NS added for VSPLUS-615
        //            GBTextBox.Visible = false;
        //            GBLabel.Visible = false;
        //            GBTitle.Visible = false;
        //        }
        //        //5/12/2014 NS modified for VSPLUS-615
        //        //else if (DiskDataTable.Rows.Count == 1 && DiskDataTable.Rows[0]["DiskName"].ToString() == "0")
        //        else if (DiskDataTable.Rows.Count == 1 && DiskDataTable.Rows[0]["DiskName"].ToString() == "AllDisks" &&
        //            DiskDataTable.Rows[0]["ThresholdType"].ToString() == "Percent")
        //        {
        //            //12/16/2013 NS modified - created a radio button list
        //            SelCriteriaRadioButtonList.SelectedIndex = 0;
        //            AdvDiskSpaceThTrackBar.Visible = true;
        //            AdvDiskSpaceThTrackBar.Value = DiskDataTable.Rows[0]["Threshold"];
        //            DiskLabel.Visible = true;
        //            Label4.Visible = true;
        //            DiskGridView.Visible = false;
        //            DiskGridInfo.Visible = false;
        //            //rdbSelAll.Checked = true;
        //            //rdbSelFew.Checked = false;
        //            //rdbNoAlerts.Checked = false;


        //            //12/16/2013 NS added
        //            //SelectDisksRoundPanel.Visible = false;
        //            //SelectDisksRoundPanel.Enabled = false;
        //            //1/31/2014 NS added for VSPLUS-289
        //            infoDiskDiv.Style.Value = "display: none";
        //            //5/12/2014 NS added for VSPLUS-615
        //            GBTextBox.Visible = false;
        //            GBLabel.Visible = false;
        //            GBTitle.Visible = false;
        //            DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetSrvDiskSettings(NameTextBox.Text, "All");
        //        }
        //        //5/12/2014 NS added for VSPLUS-615
        //        else if (DiskDataTable.Rows.Count == 1 && DiskDataTable.Rows[0]["DiskName"].ToString() == "AllDisks" &&
        //          DiskDataTable.Rows[0]["ThresholdType"].ToString() == "GB")//if (DiskDataTable.Rows.Count> 0)
        //        {
        //            SelCriteriaRadioButtonList.SelectedIndex = 1;
        //            AdvDiskSpaceThTrackBar.Visible = false;
        //            DiskLabel.Visible = false;
        //            Label4.Visible = true;
        //            DiskGridView.Visible = false;
        //            DiskGridInfo.Visible = false;
        //            infoDiskDiv.Style.Value = "display: none";
        //            GBTextBox.Visible = true;
        //            GBLabel.Visible = true;
        //            GBTitle.Visible = true;
        //            if (DiskDataTable.Rows.Count > 0)
        //            {
        //                GBTextBox.Text = DiskDataTable.Rows[0]["Threshold"].ToString();
        //            }
        //            DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetSrvDiskSettings(NameTextBox.Text, "All");
        //        }
        //        else
        //        {
        //            //12/16/2013 NS modified - created a radio button list
        //            //5/12/2014 NS modified for VSPLUS-615
        //            //SelCriteriaRadioButtonList.SelectedIndex = 1;
        //            SelCriteriaRadioButtonList.SelectedIndex = 2;
        //            AdvDiskSpaceThTrackBar.Visible = false;
        //            DiskLabel.Visible = false;
        //            Label4.Visible = false;
        //            DiskGridView.Visible = true;
        //            DiskGridInfo.Visible = true;
        //            //rdbSelAll.Checked = false;
        //            //rdbSelFew.Checked = true;
        //            //rdbNoAlerts.Checked = false;
        //            DiskDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetSrvDiskSettings(NameTextBox.Text, "");
        //            //12/16/2013 NS added
        //            //SelectDisksRoundPanel.Visible = true;
        //            //SelectDisksRoundPanel.Enabled = true;
        //            //1/31/2014 NS added for VSPLUS-289
        //            infoDiskDiv.Style.Value = "display: block";
        //            //5/12/2014 NS added for VSPLUS-615
        //            GBTextBox.Visible = false;
        //            GBLabel.Visible = false;
        //            GBTitle.Visible = false;
        //        }
        //        //else
        //        //{ 

        //        //}
        //        Session["DiskDataTable"] = DiskDataTable;
        //        DiskGridView.DataSource = DiskDataTable;
        //        DiskGridView.DataBind();

        //    }
        //    catch (Exception ex)
        //    {
        //        //6/27/2014 NS added for VSPLUS-634
        //        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
        //        throw ex;
        //    }
        //    finally { }
        //}

        //private void FillDiskGridfromSession()
        //{
        //    try
        //    {

        //        DataTable ServersDataTable = new DataTable();
        //        if (Session["DiskDataTable"] != "" && Session["DiskDataTable"] != null)
        //            ServersDataTable = (DataTable)Session["DiskDataTable"];
        //        //5/2/2014 NS modified for VSPLUS-602
        //        if (ServersDataTable.Rows.Count > 0 && SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
        //        {
        //            //5/2/2014 NS added for VSPLUS-602
        //            GridViewDataColumn column1 = DiskGridView.Columns["Threshold"] as GridViewDataColumn;
        //            GridViewDataColumn column2 = DiskGridView.Columns["ThresholdType"] as GridViewDataColumn;
        //            DataTable dt = new DataTable();
        //            for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
        //            {
        //                if (DiskGridView.Selection.IsRowSelected(i))
        //                {
        //                    ASPxTextBox txtThreshold = (ASPxTextBox)DiskGridView.FindRowCellTemplateControl(i, column1, "txtFreeSpaceThresholdValue");
        //                    ASPxComboBox txtThresholdType = (ASPxComboBox)DiskGridView.FindRowCellTemplateControl(i, column2, "txtFreeSpaceThresholdType");
        //                    ServersDataTable.Rows[i]["Threshold"] = txtThreshold.Text;
        //                    ServersDataTable.Rows[i]["ThresholdType"] = txtThresholdType.SelectedItem.Text;
        //                }
        //            }
        //            DiskGridView.DataSource = ServersDataTable;
        //            DiskGridView.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorDiv.Style.Value = "display: block;";
        //        errorDiv.InnerHtml = "One of the selected row values is incorrect: threshold values must be numeric and threshold type must be set for each selected disk.";
        //        //6/27/2014 NS added for VSPLUS-634
        //        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
        //    }
        //}

        protected void SelCriteriaRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //5/12/2014 NS modified for VSPLUS-615
            //if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "0")
            //{
            //    AdvDiskSpaceThTrackBar.Visible = true;
            //    Label4.Visible = true;
            //    DiskLabel.Visible = true;
            //    DiskLabel.Text = "Current threshold: " + AdvDiskSpaceThTrackBar.Value + "% free space";
            //    DiskGridView.Visible = false;
            //    DiskGridInfo.Visible = false;
            //    for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
            //    {
            //        DiskGridView.Selection.SetSelection(i, false);
            //    }
            //    //1/31/2014 NS added for VSPLUS-289
            //    infoDiskDiv.Style.Value = "display: none";
            //    //5/12/2014 NS added for VSPLUS-615
            //    GBLabel.Visible = false;
            //    GBTextBox.Visible = false;
            //    GBTitle.Visible = false;
            //}
            //else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
            //{
            //    DiskGridView.Visible = true;
            //    DiskGridInfo.Visible = true;
            //    AdvDiskSpaceThTrackBar.Visible = false;
            //    Label4.Visible = false;
            //    DiskLabel.Visible = false;
            //    //1/31/2014 NS added for VSPLUS-289
            //    infoDiskDiv.Style.Value = "display: block";
            //    //5/12/2014 NS added for VSPLUS-615
            //    GBLabel.Visible = false;
            //    GBTextBox.Visible = false;
            //    GBTitle.Visible = false;
            //}
            //else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "2")
            //{
            //    AdvDiskSpaceThTrackBar.Visible = false;
            //    Label4.Visible = false;
            //    DiskLabel.Visible = false;
            //    DiskGridView.Visible = false;
            //    DiskGridInfo.Visible = false;
            //    for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
            //    {
            //        DiskGridView.Selection.SetSelection(i, false);
            //    }
            //    //1/31/2014 NS added for VSPLUS-289
            //    infoDiskDiv.Style.Value = "display: none";
            //    //5/12/2014 NS added for VSPLUS-615
            //    GBLabel.Visible = false;
            //    GBTextBox.Visible = false;
            //    GBTitle.Visible = false;
            //}
            ////5/12/2014 NS added for VSPLUS-615
            //else if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "3")
            //{
            //    AdvDiskSpaceThTrackBar.Visible = false;
            //    Label4.Visible = true;
            //    DiskLabel.Visible = false;
            //    DiskGridView.Visible = false;
            //    DiskGridInfo.Visible = false;
            //    for (int i = 0; i < DiskGridView.VisibleRowCount; i++)
            //    {
            //        DiskGridView.Selection.SetSelection(i, false);
            //    }
            //    infoDiskDiv.Style.Value = "display: none";
            //    GBLabel.Visible = true;
            //    GBTextBox.Visible = true;
            //    GBTitle.Visible = true;
            //}
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
            //if (isValid)
            //{
            //    //12/17/2013 NS modified
            //    //5/12/2014 NS modified for VSPLUS-615
            //    //if (SelCriteriaRadioButtonList.SelectedIndex == 1)
            //    if (SelCriteriaRadioButtonList.SelectedItem.Value.ToString() == "1")
            //    //if (rdbSelFew.Checked)
            //    {
            //        ASPxGridView gridView = (ASPxGridView)sender;
            //        for (int i = 0; i < gridView.VisibleRowCount; i++)
            //        {
            //            try
            //            {
            //                gridView.Selection.SetSelection(i, (int)gridView.GetRowValues(i, "isSelected") == 1);
            //            }
            //            catch (Exception ex)
            //            {
            //                //6/27/2014 NS added for VSPLUS-634
            //                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            //            }
            //        }
            //    }
            //}
        }
        protected void DiskGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;

            DataTable DiskDataTable = (DataTable)Session["DiskDataTable"];

            DataRow dr = GetDiskRow(DiskDataTable, e.NewValues.GetEnumerator(), e.Keys[0].ToString());//Convert.ToInt32(e.Keys[0]));



            //gridView.UpdateEdit();
            Session["DiskDataTable"] = DiskDataTable;

           // FillDiskGridfromSession();

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
           // DiskLabel.Text = "Current threshold: " + AdvDiskSpaceThTrackBar.Value + "% free space";
        }

        protected void RolesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //FillExchangeServerServicesGrid();
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
           // CPURoundPanel.HeaderText = "CPU Utilization Alert at " + AdvCPUThTrackBar.Value + "% utilization";
        }

        protected void AdvMemoryThTrackBar_PositionChanged(object sender, EventArgs e)
        {
           // MemRoundPanel9.HeaderText = "Memory Usage Alert at " + AdvMemoryThTrackBar.Value + "% utilization";
        }

        private DataTable GetSelectedServices()
        {


            DataTable dtSel = new DataTable();
            try
            {
                dtSel.Columns.Add("id");

                DataTable dt = (DataTable)Session["WindowsServices"];
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["isSelected"].ToString().ToLower() == "true")
                        {
                            DataRow dr = dtSel.NewRow();
                            dr["id"] = row["ID"];
                            dtSel.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

            return dtSel;

        }
        private void FillPrmaryconnBox()
        {
        }

        private void FillCredentialsComboBox()
        {
            DataTable CredentialsDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetCredentials();
            CredentialsComboBox.DataSource = CredentialsDataTable;
            CredentialsComboBox.TextField = "AliasName";
            CredentialsComboBox.ValueField = "ID";
            CredentialsComboBox.DataBind();

        //    ActiveSyncCredentialsComboBox.DataSource = CredentialsDataTable;
        //    ActiveSyncCredentialsComboBox.TextField = "AliasName";
        //    ActiveSyncCredentialsComboBox.ValueField = "ID";
        //    ActiveSyncCredentialsComboBox.DataBind();
        }

        //private void FillWindowsServicesGrid()
        //{
        //    try
        //    {
        //        Session["WindowsServices"] = null;

        //        DataTable dt = new DataTable();

        //        dt = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetWindowsServices(Request.QueryString["name"].ToString());

        //        DataColumn[] columns = new DataColumn[1];
        //        columns[0] = dt.Columns["ID"];
        //        dt.PrimaryKey = columns;

        //        if (dt.Rows.Count > 0)
        //        {
        //            Session["WindowsServices"] = dt;

        //        }


        //        ServicesGrid.DataSource = dt;
        //        ServicesGrid.DataBind();
        //        (ServicesGrid.Columns["Type"] as GridViewDataColumn).GroupBy();

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
        //        throw ex;
        //    }
        //}

        private void FillWindowsServicesGridFromSession()
        {
            try
            {

                DataTable dt = new DataTable();

                if (Session["WindowsServices"] != null && Session["WindowsServices"] != "")
                    dt = (DataTable)Session["WindowsServices"];


                if (dt.Rows.Count > 0)
                {
                   // ServicesGrid.DataSource = dt;
                   // ServicesGrid.DataBind();
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

        protected void checkToMonitor_Init(object sender, EventArgs e)
        {
            ASPxCheckBox chk = sender as ASPxCheckBox;
            GridViewDataItemTemplateContainer container = chk.NamingContainer as GridViewDataItemTemplateContainer;

            chk.ClientSideEvents.CheckedChanged = String.Format("function (s,e) {{ cb.PerformCallback('{0}|' + s.GetChecked()); }}", container.KeyValue);
        }

        protected void cb_callback(object sender, DevExpress.Web.CallbackEventArgs e)
        {
            String[] parameters = e.Parameter.Split('|');
            string id = parameters[0];
            bool isChecked = Convert.ToBoolean(parameters[1]);

            DataTable dt = new DataTable();

            if (Session["WindowsServices"] != null && Session["WindowsServices"] != "")
                dt = (DataTable)Session["WindowsServices"];

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows.Find(id);
                (dt.Rows.Find(id))["isSelected"] = isChecked;
                DataRow row2 = dt.Rows.Find(id);
                Session["WindowsServices"] = dt;
            }
        }

        protected void ServicesGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Result" && (e.CellValue.ToString() == "Running"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
            else if (e.DataColumn.FieldName == "Result" && (e.CellValue.ToString() == "Stopped"))
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Result")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
            }
        }



		private void FillDatabaseGridView()
		{

			//Test settigns tbl, then get the other
			try
			{

				DataTable DatabaseDataTable = new DataTable();
				DatabaseDataTable = VSWebBL.ConfiguratorBL.DAGBL.Ins.getMailDatabaseSettings(NameTextBox.Text);
				//DatabaseDataTable = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.getMailDatabaseSize(NameTextBox.Text);
				if (DatabaseDataTable.Rows.Count == 0 || (DatabaseDataTable.Rows.Count == 1 && DatabaseDataTable.Rows[0]["DatabaseName"].ToString() == "NoAlerts"))
				{
					SelCriteriaDBRadioButtonList.SelectedIndex = 2;
					DatabaseLabel.Visible = false;
					//10/15/2014 NS modified
					//Label4.Visible = false;
					Label1.Visible = false;
					DatabaseGridView.Visible = false;
					DatabaseGridInfo.Visible = false;
					infoDatabaseDiv.Style.Value = "display: none";
					ReplayTitle.Visible = false;
					ReplayTextBox.Visible = false;
					CopyTitle.Visible = false;
					CopyTextBox.Visible = false;
				}
				else if (DatabaseDataTable.Rows.Count == 1 && DatabaseDataTable.Rows[0]["DatabaseName"].ToString() == "AllDatabases")
				{
					SelCriteriaDBRadioButtonList.SelectedIndex = 0;
					DatabaseLabel.Visible = false;
					//10/15/2014 NS modified
					//Label4.Visible = true;
					Label1.Visible = true;
					DatabaseGridView.Visible = false;
					DatabaseGridInfo.Visible = false;
					infoDatabaseDiv.Style.Value = "display: none";
					ReplayTitle.Visible = true;
					ReplayTextBox.Visible = true;
					CopyTitle.Visible = true;
					CopyTextBox.Visible = true;

					ReplayTextBox.Text = DatabaseDataTable.Rows[0]["ReplayQueueThreshold"].ToString();
					CopyTextBox.Text = DatabaseDataTable.Rows[0]["CopyQueueThreshold"].ToString();
				}
				else
				{
					SelCriteriaDBRadioButtonList.SelectedIndex = 1;
					DatabaseLabel.Visible = false;
					//10/15/2014 NS modified
					//Label4.Visible = false;
					Label1.Visible = false;
					DatabaseGridView.Visible = true;
					DatabaseGridInfo.Visible = true;
					infoDatabaseDiv.Style.Value = "display: block";
					ReplayTitle.Visible = false;
					ReplayTextBox.Visible = false;
					CopyTitle.Visible = false;
					CopyTextBox.Visible = false;
				}

				//DatabaseDataTable = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.getMailDatabaseSize();
				//DatabaseDataTable = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.getMailDatabaseSizeSettings(NameTextBox.Text);
				DatabaseDataTable = VSWebBL.ConfiguratorBL.DAGBL.Ins.getMailDatabaseQueueSettings(NameTextBox.Text);
				Session["DatabaseDataTable"] = DatabaseDataTable;
				DatabaseGridView.DataSource = DatabaseDataTable;
				DatabaseGridView.DataBind();

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		private void FillDatabaseGridfromSession()
		{
			try
			{

				DataTable ServersDataTable = new DataTable();
				if (Session["DatabaseDataTable"] != "" && Session["DatabaseDataTable"] != null)
					ServersDataTable = (DataTable)Session["DatabaseDataTable"];
				//5/2/2014 NS modified for VSPLUS-602
				if (ServersDataTable.Rows.Count > 0 && SelCriteriaDBRadioButtonList.SelectedItem.Value.ToString() == "1")
				{
					//5/2/2014 NS added for VSPLUS-602
					GridViewDataColumn column1 = DatabaseGridView.Columns["CopyQueueThreshold"] as GridViewDataColumn;
					GridViewDataColumn column2 = DatabaseGridView.Columns["ReplayQueueThreshold"] as GridViewDataColumn;
					DataTable dt = new DataTable();
					for (int i = 0; i < DatabaseGridView.VisibleRowCount; i++)
					{
						if (DatabaseGridView.Selection.IsRowSelected(i))
						{
							ASPxTextBox CopyQueueThreshold = (ASPxTextBox)DatabaseGridView.FindRowCellTemplateControl(i, column1, "txtCopyQueueThreshold");
							ASPxTextBox ReplayQueueThreshold = (ASPxTextBox)DatabaseGridView.FindRowCellTemplateControl(i, column2, "txtReplayQueueThreshold");
							ServersDataTable.Rows[i]["CopyQueueThreshold"] = CopyQueueThreshold.Text;
							ServersDataTable.Rows[i]["ReplayQueueThreshold"] = ReplayQueueThreshold.Text;
							//DatabaseGridView.Selection.SetSelection(i, (int)DatabaseGridView.GetRowValues(i, "isSelected") == 1);
							ServersDataTable.Rows[i]["isSelected"] = true;
						}
					}
					DatabaseGridView.DataSource = ServersDataTable;
					DatabaseGridView.DataBind();
				}
			}
			catch (Exception ex)
			{
				/*
				errorDiv.Style.Value = "display: block;";
				errorDiv.InnerHtml = "One of the selected row values is incorrect: threshold values must be numeric and threshold type must be set for each selected disk.";
				*/

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}


		protected void SelCriteriaDBRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SelCriteriaDBRadioButtonList.SelectedItem.Value.ToString() == "0")
			{
				//10/15/2014 NS modified
				//Label4.Visible = true;
				Label1.Visible = true;
				DatabaseLabel.Visible = true;
				DatabaseGridView.Visible = false;
				DatabaseGridInfo.Visible = false;
				for (int i = 0; i < DatabaseGridView.VisibleRowCount; i++)
				{
					DatabaseGridView.Selection.SetSelection(i, false);
				}
				//1/31/2014 NS added for VSPLUS-289
				infoDatabaseDiv.Style.Value = "display: none";
				//5/12/2014 NS added for VSPLUS-615
				ReplayTitle.Visible = true;
				ReplayTextBox.Visible = true;
				CopyTitle.Visible = true;
				CopyTextBox.Visible = true;
			}
			else if (SelCriteriaDBRadioButtonList.SelectedItem.Value.ToString() == "1")
			{
				DatabaseGridView.Visible = true;
				DatabaseGridInfo.Visible = true;
				//10/15/2014 NS modified
				//Label4.Visible = false;
				Label1.Visible = false;
				DatabaseLabel.Visible = false;
				//1/31/2014 NS added for VSPLUS-289
				infoDatabaseDiv.Style.Value = "display: block";
				//5/12/2014 NS added for VSPLUS-615
				ReplayTitle.Visible = false;
				ReplayTextBox.Visible = false;
				CopyTitle.Visible = false;
				CopyTextBox.Visible = false;
			}
			else if (SelCriteriaDBRadioButtonList.SelectedItem.Value.ToString() == "2")
			{
				//10/15/2014 NS modified
				//Label4.Visible = false;
				Label1.Visible = false;
				DatabaseLabel.Visible = false;
				DatabaseGridView.Visible = false;
				DatabaseGridInfo.Visible = false;
				for (int i = 0; i < DatabaseGridView.VisibleRowCount; i++)
				{
					DatabaseGridView.Selection.SetSelection(i, false);
				}
				//1/31/2014 NS added for VSPLUS-289
				infoDatabaseDiv.Style.Value = "display: none";
				//5/12/2014 NS added for VSPLUS-615
				ReplayTitle.Visible = false;
				ReplayTextBox.Visible = false;
				CopyTitle.Visible = false;
				CopyTextBox.Visible = false;
			}
		}

		protected void DatabaseGridView_PreRender(object sender, EventArgs e)
		{
			//5/1/2014 NS added for VSPLUS-427
			if (isValidDB)
			{
				//12/17/2013 NS modified
				//5/12/2014 NS modified for VSPLUS-615
				//if (SelCriteriaDBRadioButtonList.SelectedIndex == 1)
				if (SelCriteriaDBRadioButtonList.SelectedItem.Value.ToString() == "1")
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

		protected void DatabaseGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
			ASPxGridView gridView = (ASPxGridView)sender;

			DataTable DatabaseDataTable = (DataTable)Session["DatabaseDataTable"];

			//DataRow dr = GetDiskRow(DatabaseDataTable, e.NewValues.GetEnumerator(), e.Keys[0].ToString());//Convert.ToInt32(e.Keys[0]));



			//gridView.UpdateEdit();
			Session["DatabaseDataTable"] = DatabaseDataTable;

			//FillDiskGridfromSession();

			gridView.CancelEdit();
			e.Cancel = true;
		}


		//protected void FormOkButton_Click(object sender, EventArgs e)
		//{
		//    bool proceed = true;
		//    string errtext = "";
		//    int gbc = 0;

		//    if (SelCriteriaDBRadioButtonList.SelectedItem.Value.ToString() == "1")
		//    {
		//        List<object> fieldValues = DatabaseGridView.GetSelectedFieldValues(new string[] { "DatabaseName", "DatabaseSizeThreshold", "WhiteSpaceThreshold" });
		//        if (fieldValues.Count == 0)
		//        {
		//            proceed = false;
		//            isValidDB = false;
		//            errtext = "You have enabled a 'Selected Disks' option on the Disk Settings tab but selected no disks " +
		//            "in the grid or some of the selected disks do not have a threshold or threshold type value. <br />" +
		//            "Please correct the disk settings in order to save your changes.";
		//        }
		//        else
		//        {
		//            foreach (object[] item in fieldValues)
		//            {
		//                if (item[1].ToString() == "" || item[2].ToString() == "")
		//                {
		//                    proceed = false;
		//                    isValidDB = false;
		//                    errtext = "You have enabled a 'Selected Disks' option on the Disk Settings tab but selected no disks " +
		//                    "in the grid or some of the selected disks do not have a threshold or threshold type value. <br />" +
		//                    "Please correct the disk settings in order to save your changes.";
		//                }
		//            }
		//        }
		//    }
		//    if (SelCriteriaDBRadioButtonList.SelectedItem.Value.ToString() == "0")
		//    {
		//        if (GBWhiteSpaceTextBox.Text == "")
		//        {
		//            proceed = false;
		//            isValidDB = false;
		//            errtext = "You have enabled an 'All Databases - By GB' option on the Database Settings tab but entered no threshold value for white space. " +
		//                "You must enter a numeric threshold value in order to save your changes.";
		//        }
		//        else if (!int.TryParse(GBWhiteSpaceTextBox.Text, out gbc))
		//        {
		//            proceed = false;
		//            isValidDB = false;
		//            errtext = "You have enabled an 'All Database - By GB' option on the Database Settings tab but entered an invalid white space threshold value. " +
		//                "You must enter a numeric threshold value in order to save your changes.";
		//        }
		//        if (GBDatabaseTextBox.Text == "")
		//        {
		//            proceed = false;
		//            isValidDB = false;
		//            errtext = "You have enabled an 'All Database - By GB' option on the Database Settings tab but entered no threshold value for database size. " +
		//                "You must enter a numeric threshold value in order to save your changes.";
		//        }
		//        else if (!int.TryParse(GBDatabaseTextBox.Text, out gbc))
		//        {
		//            proceed = false;
		//            isValidDB = false;
		//            errtext = "You have enabled an 'All Database - By GB' option on the Database Settings tab but entered an invalid database size threshold value. " +
		//                "You must enter a numeric threshold value in order to save your changes.";
		//        }
		//    }
		//    if (proceed)
		//    {
		//        try
		//        {
		//            //DataTable dt = GetDataView(DiskGridView);
		//            //Save Server Attributes Tab
		//            UpdateDatabaseSettings();
		//        }
		//        catch (Exception ex)
		//        {
		//            //6/27/2014 NS added for VSPLUS-634
		//            Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
		//            throw ex;
		//        }
		//        finally { }
		//    }
		//    else
		//    {
		//        //errorDiv.Style.Value = "display: block;";
		//        //errorDiv.InnerHtml = errtext;
		//    }
		//}

		private bool UpdateDatabaseSettings()
		{
			bool ReturnValue = false;

			try
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ServerName");
				dt.Columns.Add("DatabaseName");
				dt.Columns.Add("CopyQueueThreshold");
				dt.Columns.Add("ReplayQueueThreshold");

				List<object> fieldValues = DatabaseGridView.GetSelectedFieldValues(new string[] { "DatabaseName", "ServerName", "CopyQueueThreshold", "ReplayQueueThreshold" });

				//GB = 0
				//Selected = 1
				//None = 2
				if (SelCriteriaDBRadioButtonList.SelectedItem.Value.ToString() == "0")
				{
					DataRow row = dt.Rows.Add();
					row["ServerName"] = "AllDatabases";
					row["DatabaseName"] = "AllDatabases";
					row["CopyQueueThreshold"] = CopyTextBox.Text;
					row["ReplayQueueThreshold"] = ReplayTextBox.Text;
				}
				else if (SelCriteriaDBRadioButtonList.SelectedItem.Value.ToString() == "1")
				{
					foreach (object[] item in fieldValues)
					{
						DataRow row = dt.Rows.Add();
						row["DatabaseName"] = item[0].ToString();
						row["ServerName"] = item[1].ToString();
						row["CopyQueueThreshold"] = item[2].ToString();
						row["ReplayQueueThreshold"] = item[3].ToString();
					}
				}
				else if (SelCriteriaDBRadioButtonList.SelectedItem.Value.ToString() == "2")
				{

					DataRow row = dt.Rows.Add();
					row["ServerName"] = "NoAlerts";
					row["DatabaseName"] = "NoAlerts";
					row["CopyQueueThreshold"] = "0";
					row["ReplayQueueThreshold"] = "0";
				}

				if (dt.Rows.Count > 0)
					ReturnValue = VSWebBL.ConfiguratorBL.DAGBL.Ins.InsertSrvDatabaseSettingsData(dt, NameTextBox.Text);
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			return ReturnValue;

		}
    }
}