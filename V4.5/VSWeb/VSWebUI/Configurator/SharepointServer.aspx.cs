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

namespace VSWebUI.Configurator
{
    public partial class SharepointServer : System.Web.UI.Page
    {
        bool isValid = true;
        protected int ServerId;
        protected int ServerID;
        protected int ServerTypeID;
        protected DataTable ExchangeDataTable = null;
        VSFramework.TripleDES encryptkey = new VSFramework.TripleDES();
        protected void Page_Load(object sender, EventArgs e)
        {
            ServerTypeID = 5;
            lblServerId.Text = Request.QueryString["ID"];
            if (!IsPostBack)
            {
                //CredentialsComboBox.SelectedItem.Text = "Sharepoint";
                FillCredentialsComboBox();
                Filldata();

                if (Request.QueryString["name"] != "" && Request.QueryString["name"] != null)
                {
                    NameTextBox.Text = Request.QueryString["name"].ToString();
                    LocationTextBox.Text = Request.QueryString["Loc"].ToString();
                    DescTextBox.Enabled = false;
                    NameTextBox.Enabled = false;
                    LocationTextBox.Enabled = false;
                }
                FillDiskGridView();
				FillWindowsServicesGrid();
                FillSiteCollectionsTestOptions();
                //5/2/2016 Sowjanya modified for VSPLUS-2914
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "SharepointServer|ServicesGrid")
                        {
                            ServicesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }

                    }
                }
            }
            else
            {
                //Commented by Mukund 30Mar14
                // FillExchangeServerServicesGridfromSession();
                FillDiskGridfromSession();
				FillWindowsServicesGridFromSession();

            }
            // ((GridViewDataColumn)ExchangeServicesGridView.Columns["Role"]).GroupBy();
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
                    ResponseTextBox.Text = dt.Rows[0]["ResponseTime"].ToString();
                    OffscanTextBox.Text = dt.Rows[0]["OffHourInterval"].ToString();
                    //CPUThresholdTextBox.Text = dt.Rows[0]["CPU_Threshold"].ToString();
                    //MemThresholdTextBox.Text = dt.Rows[0]["MemThreshold"].ToString();
                    AdvCPUThTrackBar.Value = dt.Rows[0]["CPU_Threshold"].ToString();
                    CPURoundPanel.HeaderText = "CPU Utilization Alert at " + AdvCPUThTrackBar.Value + "% utilization";
                    AdvMemoryThTrackBar.Value = dt.Rows[0]["MemThreshold"].ToString();
                    MemRoundPanel9.HeaderText = "Memory Usage Alert at " + AdvMemoryThTrackBar.Value + "% utilization";
                    CategoryTextBox.Text = dt.Rows[0]["Category"].ToString();
                    ScanIntvlTextBox.Text = dt.Rows[0]["ScanInterval"].ToString();
                    // ServerDaysAlert.Text = dt.Rows[0]["ConsOvrThresholdBefAlert"].ToString();
                    ConsFailuresBefAlertTextBox.Text = dt.Rows[0]["ConsFailuresBefAlert"].ToString();
                    int credentialsId = 0;
                    if (dt.Rows[0]["CredentialsId"].ToString() != null && dt.Rows[0]["CredentialsId"].ToString() != "")
                    {
                        credentialsId = Convert.ToInt32(dt.Rows[0]["CredentialsId"].ToString());
                        CredentialsComboBox.SelectedItem = CredentialsComboBox.Items.FindByValue(credentialsId);
                        //CredentialsComboBox.Value = Convert.ToInt32(dt.Rows[0]["CredentialsId"].ToString());
                        ////CredentialsComboBox.SelectedItem.Value = Convert.ToInt32(dt.Rows[0]["CredentialsId"].ToString());
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
            //5/1/2014 NS modified for VSPLUS-427
            bool proceed = true;
            //5/12/2014 NS added for VSPLUS-615
            string errtext = "";
            int gbc = 0;
            //5/12/2014 NS modified for VSPLUS-615
            //if (SelCriteriaRadioButtonList.SelectedIndex == 1)
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
            //5/12/2014 NS added for VSPLUS-615
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
                    //DataTable dt = GetDataView(DiskGridView);
                    //Save Server Attributes Tab
                    UpdateServersData();
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
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = errtext +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
            }
            databasesettings();
            frontsettings();
        }

        private void UpdateServersData()
        {
            try
            {
                Object result = VSWebBL.SecurityBL.ServersBL.Ins.UpdateAttributesData(CollectDataforMSServers());



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


                //   List<object> fieldValues = ExchangeServicesGridView.GetSelectedFieldValues(new string[] { "id" });
                Object result2 = VSWebBL.ConfiguratorBL.ServicesBL.Ins.UpdateWindowsServices(Request.QueryString["name"].ToString(), fieldValues);
                Object ReturnValue;
                ReturnValue = UpdateDiskSettings();

                if (ReturnValue.ToString() == "True")
                {
                    Session["ExchangeUpdateStatus"] = NameTextBox.Text;
                    Response.Redirect("~/Configurator/SharePointGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }

                result = VSWebBL.SharePointSettingsBL.Ins.UpdateSharePointServerSettings(GetSiteCollectionsTestOptions());

            }
            catch (Exception ex)
            {
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
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
                //<<<<<<< .mine
                Mobj.Enabled = EnabledCheckBox.Checked;

                Mobj.CPUThreshold = 1;// int.Parse(AdvCPUThTrackBar.Value.ToString());
                Mobj.MemThreshold = 1;// int.Parse(AdvMemoryThTrackBar.Value.ToString());
                //=======
                Mobj.Enabled = EnabledCheckBox.Checked;

                Mobj.CPUThreshold = int.Parse(AdvCPUThTrackBar.Value.ToString());
                Mobj.MemThreshold = int.Parse(AdvMemoryThTrackBar.Value.ToString());
                //>>>>>>> .r2872
                Mobj.Category = CategoryTextBox.Text;
                Mobj.OffHourInterval = (OffscanTextBox.Text == "" ? 0 : Convert.ToInt32(OffscanTextBox.Text));
                Mobj.ScanInterval = (ScanIntvlTextBox.Text == "" ? 0 : Convert.ToInt32(ScanIntvlTextBox.Text));
                Mobj.ResponseTime = (ResponseTextBox.Text == "" ? 0 : Convert.ToInt32(ResponseTextBox.Text));
                Mobj.RetryInterval = (RetryTextBox.Text == "" ? 0 : Convert.ToInt32(RetryTextBox.Text));
                Mobj.ConsFailuresBefAlert = (ConsFailuresBefAlertTextBox.Text == "" ? 0 : Convert.ToInt32(ConsFailuresBefAlertTextBox.Text));

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
            Response.Redirect("~/Configurator/SharePointGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        //Mukund 12Jun14:  VE-4	: Implement Disk Checking - Front End
        private void FillDiskGridView()
        {
            try
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
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "One of the selected row values is incorrect: threshold values must be numeric and threshold type must be set for each selected disk." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
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


            //ActiveSyncCredentialsComboBox.DataSource = CredentialsDataTable;
            //ActiveSyncCredentialsComboBox.TextField = "AliasName";
            //ActiveSyncCredentialsComboBox.ValueField = "ID";
            //ActiveSyncCredentialsComboBox.DataBind();
        }


        public void databasesettings()
        {
            bool returnvalue;
            string Notupdate = "";

            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "NetworkInterfaceBytes", dbnetworkbytes.Text);
                if (returnvalue == false)
                {
                    Notupdate += "NetworkInterfaceBytes";
                }

            }


            catch (Exception ex)
            {
                
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "NetworkInterfacePackets", dbnetworkpkts.Text);
                if (returnvalue == false)
                {
                    Notupdate += "NetworkInterfacePackets";
                }
            }
            catch (Exception ex)
            {
                
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "RedirectorServerSessionsHung ", redirectorssh.Text);
                if (returnvalue == false)
                {
                    Notupdate += "RedirectorServerSessionsHung ";
                }
            }
            catch (Exception ex)
            {
               
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            
            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "SQLServerBuffercachehitratio", sqlbm.Text);
                if (returnvalue == false)
                {
                    Notupdate += "SQLServerBuffercachehitratio";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "SQLServerDatabasesTransactions", sqldt.Text);
                if (returnvalue == false)
                {
                    Notupdate += "SQLServerDatabasesTransactions";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "SQLServerDatabasesDataFilesSize", sqldatafile.Text);
                if (returnvalue == false)
                {
                    Notupdate += "SQLServerDatabasesDataFilesSize";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }


            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "SQLServerDatabasesLogFiles", sqllogfile.Text);
                if (returnvalue == false)
                {
                    Notupdate += "SQLServerDatabasesLogFiles";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "SQLServerGeneralStatisticsUserConnections", sqlgstat.Text);
                if (returnvalue == false)
                {
                    Notupdate += "SQLServerGeneralStatisticsUserConnections";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }


            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "SQLServerLocksNoDeadlocks", sqldeadlock.Text);
                if (returnvalue == false)
                {
                    Notupdate += "SQLServerLocksNoDeadlocks";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }




            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "SQLServerTransactionsFreeSpacetempdb", sqltemp.Text);
                if (returnvalue == false)
                {
                    Notupdate += "SQLServerTransactionsFreeSpacetempdb";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            if (Notupdate == "" || Notupdate == null)
            {

                errorDiv.Style.Value = "display: none";
                //successDiv.Style.Value = "display: block";
            }
        
            else
            {
               
                //successDiv.Style.Value = "display: none";
                
                errorDiv.InnerHtml = "The following fields were not updated: " + Notupdate +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }
        }
                
        public void frontsettings()
        {
            bool returnvalue;
            string Notupdate = "";

                                                                                   

            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "ASPNETRequestExecutionTime", reqetime.Text);
                if (returnvalue == false)
                {
                    Notupdate += "ASPNETRequestExecutionTime";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }




            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "ASPNETRequestsRejected", reqreject.Text);
                if (returnvalue == false)
                {
                    Notupdate += "ASPNETRequestsRejected";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }


            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "ASPNETRequestsQueued", reqqueued.Text);
                if (returnvalue == false)
                {
                    Notupdate += "ASPNETRequestsQueued";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }



            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "ASPNETRequestWaitTime", reqwaittime.Text);
                if (returnvalue == false)
                {
                    Notupdate += "ASPNETRequestWaitTime";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }



            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "ASPNETApplicationsRequests", appreq.Text);
                if (returnvalue == false)
                {
                    Notupdate += "ASPNETApplicationsRequests";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }

            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "PagingFilePercentUsage", pagingfile.Text);
                if (returnvalue == false)
                {
                    Notupdate += "PagingFilePercentUsage";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }



            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "RedirectorServerSessionsHung", redirectorsession.Text);
                if (returnvalue == false)
                {
                    Notupdate += "RedirectorServerSessionsHung";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }


            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "ServerWorkItemShortages", servershortages.Text);
                if (returnvalue == false)
                {
                    Notupdate += "ServerWorkItemShortages";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }


            try
            {
                returnvalue = VSWebBL.SharePointSettingsBL.Ins.UpdateSvalue(Convert.ToInt16(lblServerId.Text), "SystemContextSwitches", systemcontextswitch.Text);
                if (returnvalue == false)
                {
                    Notupdate += "SystemContextSwitches";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
            
            
            if (Notupdate == "" || Notupdate == null)
            {
               
                errorDiv.Style.Value = "display: none";
                //successDiv.Style.Value = "display: block";
            }
            else
            {
               
                //successDiv.Style.Value = "display: none";
                
                errorDiv.InnerHtml = "The following fields were not updated: " + Notupdate +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }

        }

		protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{


			//if (e.Item.Name == "ServerDetailsPage")
			//{
			//    Response.Redirect("~/DashBoard/DominoServerDetailsPage2.aspx", false);
			//    Context.ApplicationInstance.CompleteRequest();
			//}

			DataTable dt;
			Session["Submenu"] = "LotusDomino";
			//if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
			//{
			//    id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
			//    Response.Redirect("~/Dashboard/DominoServerDetailsPage2.aspx?Key=" + id, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			//    Context.ApplicationInstance.CompleteRequest();
			//}
			if (Request.QueryString["ID"] != "" && Request.QueryString["ID"] != null)
			{
				dt = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByID(Convert.ToInt32(Request.QueryString["ID"])));
				if (dt.Rows.Count > 0)
				{
					Response.Redirect("~/Dashboard/SharePointdetailsPage.aspx?Name=" + dt.Rows[0]["ServerName"].ToString() + "&Type=" + dt.Rows[0]["Type"].ToString() + "&LastDate=" + dt.Rows[0]["LastUpdate"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					Context.ApplicationInstance.CompleteRequest();
				}
			}


		}


		//SERVICES

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

		private void FillWindowsServicesGrid()
		{
			try
			{
				Session["WindowsServices"] = null;

				DataTable dt = new DataTable();

				dt = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetWindowsServicesForSP(Request.QueryString["name"].ToString());

				DataColumn[] columns = new DataColumn[1];
				columns[0] = dt.Columns["ID"];
				dt.PrimaryKey = columns;

				if (dt.Rows.Count > 0)
				{
					Session["WindowsServices"] = dt;

				}


				ServicesGrid.DataSource = dt;
				ServicesGrid.DataBind();
				(ServicesGrid.Columns["Type"] as GridViewDataColumn).GroupBy();
				//11/19/2014 NS added
				ServicesGrid.SortBy(ServicesGrid.Columns["Type"] as GridViewDataColumn, DevExpress.Data.ColumnSortOrder.Descending);

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}

		private void FillWindowsServicesGridFromSession()
		{
			try
			{

				DataTable dt = new DataTable();

				if (Session["WindowsServices"] != null && Session["WindowsServices"] != "")
					dt = (DataTable)Session["WindowsServices"];


				if (dt.Rows.Count > 0)
				{
					ServicesGrid.DataSource = dt;
					ServicesGrid.DataBind();
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
        //5/2/2016 Sowjanya modified for VSPLUS-2914
        protected void ServicesGrid_PageSizeChanged(object sender, EventArgs e)
        {
             VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("SharepointServer|ServicesGrid", ServicesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void btn_clickcopyprofile(object sender, EventArgs e)
        {

            CopyProfilePopupControl.ShowOnPageLoad = true;
            UserID.Visible = true;
            OKCopy.Visible = true;
            Cancel.Visible = true;
            Password.Visible = true;


        }
        protected void OKCopy_Click(object sender, EventArgs e)
        {
            bool check = false;
            Credentials Csibject = new Credentials();

            Csibject.AliasName = AliasName.Text;

           
            DataTable returntable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetAliasName(Csibject);
            string rawpass = Password.Text;
            byte[] encryptedpass = encryptkey.Encrypt(rawpass);
            string encryptedpasasstring = string.Join(", ", encryptedpass.Select(s => s.ToString()).ToArray());


            if (returntable.Rows.Count > 0)
            {

                Div3.InnerHtml = "This Alias already exists. Enter another one." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                Div3.Style.Value = "display: block";

            }
            else
            {
                check = VSWebBL.SecurityBL.webspehereImportBL.Ins.Insertpwd(AliasName.Text, UserID.Text, encryptedpasasstring, 4);
                CopyProfilePopupControl.ShowOnPageLoad = false;
                FillCredentialsComboBox();
            }

        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            
            CopyProfilePopupControl.ShowOnPageLoad = false;
        }

        protected SharePointSettings GetSiteCollectionsTestOptions()
        {
            SharePointSettings sps = new SharePointSettings()
            {
                ConflictingContentTypes = CheckBoxConflictingContentTypes.Checked,
                CustomizedFiles = ComboBoxCustomizedFiles.Checked,
                MissingGalleries = ComboBoxMissingGalleries.Checked,
                MissingParentContentTypes = ComboBoxMissingParentContentTypes.Checked,
                MissingSiteTemplates = ComboBoxMissingSiteTemplates.Checked,
                UnsupportedLanguagePackReferences = ComboBoxUnsupportedLanguagePackReferences.Checked,
                UnsupportedMUIReferences = ComboBoxUnsupportedMUIReferences.Checked,
                ServerId = Convert.ToInt32(lblServerId.Text)
            };

            return sps;
        }

        protected void FillSiteCollectionsTestOptions()
        {
            DataTable dt = VSWebBL.SharePointSettingsBL.Ins.GetSharePointSiteCollectionSettings(Convert.ToInt32(lblServerId.Text));

            if (dt.Rows.Count > 0)
            {
                CheckBoxConflictingContentTypes.Checked = Convert.ToBoolean(dt.Rows[0]["ConflictingContentType"]);
                ComboBoxCustomizedFiles.Checked = Convert.ToBoolean(dt.Rows[0]["CustomizedFiles"]);
                ComboBoxMissingGalleries.Checked = Convert.ToBoolean(dt.Rows[0]["MissingGalleries"]);
                ComboBoxMissingParentContentTypes.Checked = Convert.ToBoolean(dt.Rows[0]["MissingParentContentTypes"]);
                ComboBoxMissingSiteTemplates.Checked = Convert.ToBoolean(dt.Rows[0]["MissingSiteTemplates"]);
                ComboBoxUnsupportedLanguagePackReferences.Checked = Convert.ToBoolean(dt.Rows[0]["UnsupportedLanguagePack"]);
                ComboBoxUnsupportedMUIReferences.Checked = Convert.ToBoolean(dt.Rows[0]["UnsupportedMUI"]);
            }
            else
            {
                CheckBoxConflictingContentTypes.Checked = true;
                ComboBoxCustomizedFiles.Checked = true;
                ComboBoxMissingGalleries.Checked = true;
                ComboBoxMissingParentContentTypes.Checked = true;
                ComboBoxMissingSiteTemplates.Checked = true;
                ComboBoxUnsupportedLanguagePackReferences.Checked = true;
                ComboBoxUnsupportedMUIReferences.Checked = true;

            }

        }

    }
}

