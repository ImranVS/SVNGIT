using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using DevExpress.Web;
using VSWebDO;

namespace VSWebUI.Configurator
{
    public partial class LyncServer : System.Web.UI.Page
    {
        bool isValid = true;
        protected int ServerTypeID;

        protected void Page_Load(object sender, EventArgs e)
        {
            ServerTypeID = 15;
            lblServerId.Text = Request.QueryString["ID"];
            if (!IsPostBack)
            {
                FillCredentialsComboBox();
                Filldata();
                if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
                {
                    NameTextBox.Text = Request.QueryString["Name"].ToString();
                    LocationTextBox.Text = Request.QueryString["Loc"].ToString();
                    DescTextBox.Enabled = false;
                    NameTextBox.Enabled = false;
                    LocationTextBox.Enabled = false;
                }
                FillDiskGridView();
            }
            else
            {
                FillDiskGridfromSession();
            }
        }

        private void FillCredentialsComboBox()
        {
            DataTable CredentialsDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetCredentials();
            CredentialsComboBox.DataSource = CredentialsDataTable;
            CredentialsComboBox.TextField = "AliasName";
            CredentialsComboBox.ValueField = "ID";
            CredentialsComboBox.DataBind();
        }

        public void Filldata()
        {
            DataTable dt = new DataTable();
            try
            {
                Servers ServersObject = new Servers();
                ServersObject.ServerName = Request.QueryString["Name"].ToString();
                ServersObject.ServerTypeID = ServerTypeID;
                dt = VSWebBL.ConfiguratorBL.LyncBL.Ins.GetAllDataByName(ServersObject);
                if (dt.Rows.Count > 0)
                {
                    //10/21/2014 NS modified for VSPLUS-934
                    //lblServer.Text += " - " + dt.Rows[0]["ServerName"].ToString();
                    lblServer.InnerHtml += " - " + dt.Rows[0]["ServerName"].ToString();
                    DescTextBox.Text = dt.Rows[0]["Description"].ToString();
                    if (dt.Rows[0]["Enabled"].ToString() != "" && dt.Rows[0]["Enabled"] != null)
                        EnabledCheckBox.Checked = bool.Parse(dt.Rows[0]["Enabled"].ToString());
                    RetryTextBox.Text = dt.Rows[0]["RetryInterval"].ToString();
                    ResponseTextBox.Text = dt.Rows[0]["ResponseThreshold"].ToString();
                    OffscanTextBox.Text = dt.Rows[0]["OffHoursScanInterval"].ToString();
                    AdvCPUThTrackBar.Value = dt.Rows[0]["CPUThreshold"].ToString();
                    CPURoundPanel.HeaderText = "CPU Utilization Alert at " + AdvCPUThTrackBar.Value + "% utilization";
                    AdvMemoryThTrackBar.Value = dt.Rows[0]["MemoryThreshold"].ToString();
                    MemRoundPanel9.HeaderText = "Memory Usage Alert at " + AdvMemoryThTrackBar.Value + "% utilization";
                    CategoryTextBox.Text = dt.Rows[0]["Category"].ToString();
                    ScanIntvlTextBox.Text = dt.Rows[0]["ScanInterval"].ToString();
                    ConsFailuresBefAlertTextBox.Text = dt.Rows[0]["FailureThreshold"].ToString();
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
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
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

        protected void AdvDiskSpaceThTrackBar_ValueChanged(object sender, EventArgs e)
        {
            //12/17/2013 NS modified
            //DiskRoundPanel7.HeaderText = "Disk Space Alert at  " + AdvDiskSpaceThTrackBar.Value + "% free space";
            DiskLabel.Text = "Current threshold: " + AdvDiskSpaceThTrackBar.Value + "% free space";
        }

        protected void AdvMemoryThTrackBar_PositionChanged(object sender, EventArgs e)
        {
            MemRoundPanel9.HeaderText = "Memory Usage Alert at " + AdvMemoryThTrackBar.Value + "% utilization";
        }

        protected void AdvCPUThTrackBar_ValueChanged(object sender, EventArgs e)
        {
            CPURoundPanel.HeaderText = "CPU Utilization Alert at " + AdvCPUThTrackBar.Value + "% utilization";
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
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "One of the selected row values is incorrect: threshold values must be numeric and threshold type must be set for each selected disk."+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
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

        private void UpdateServersData()
        {
            try
            {
                Object result = VSWebBL.ConfiguratorBL.LyncBL.Ins.UpdateData(CollectDataforLyncServers());
                List<object> fieldValues = new List<object>();

                //   List<object> fieldValues = ExchangeServicesGridView.GetSelectedFieldValues(new string[] { "id" });
                Object result2 = VSWebBL.ConfiguratorBL.ServicesBL.Ins.UpdateWindowsServices(Request.QueryString["name"].ToString(), fieldValues);
                Object ReturnValue;
                ReturnValue = UpdateDiskSettings();

                if (ReturnValue.ToString() == "True")
                {
                    Session["LyncUpdateStatus"] = NameTextBox.Text;
                    Response.Redirect("~/Configurator/LyncServersGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }

            }
            catch (Exception ex)
            {
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
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

        public LyncServers CollectDataforLyncServers()
        {
            LyncServers lyncobj = new LyncServers();
            try
            {
                lyncobj.Enabled = EnabledCheckBox.Checked;
                lyncobj.CPUThreshold = int.Parse(AdvCPUThTrackBar.Value.ToString());
                lyncobj.MemoryThreshold = int.Parse(AdvMemoryThTrackBar.Value.ToString());
                lyncobj.Category = CategoryTextBox.Text;
                lyncobj.OffHoursScanInterval = (OffscanTextBox.Text == "" ? 0 : Convert.ToInt32(OffscanTextBox.Text));
                lyncobj.ScanInterval = (ScanIntvlTextBox.Text == "" ? 0 : Convert.ToInt32(ScanIntvlTextBox.Text));
                lyncobj.ResponseThreshold = (ResponseTextBox.Text == "" ? 0 : Convert.ToInt32(ResponseTextBox.Text));
                lyncobj.RetryInterval = (RetryTextBox.Text == "" ? 0 : Convert.ToInt32(RetryTextBox.Text));
                lyncobj.FailureThreshold = (ConsFailuresBefAlertTextBox.Text == "" ? 0 : Convert.ToInt32(ConsFailuresBefAlertTextBox.Text));
                lyncobj.ServerID = Convert.ToInt32(lblServerId.Text);
                lyncobj.CredentialsID = (CredentialsComboBox.Text == "" ? 0 : Convert.ToInt32(CredentialsComboBox.Value));

            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            return lyncobj;
        }

        protected void FormCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/LyncServersGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

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
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = errtext+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
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
					Response.Redirect("~/Dashboard/LyncdetailsPage.aspx?Name=" + dt.Rows[0]["ServerName"].ToString() + "&Type=" + dt.Rows[0]["Type"].ToString() + "&LastDate=" + dt.Rows[0]["LastUpdate"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					Context.ApplicationInstance.CompleteRequest();
				}
			}


		}

    }
}