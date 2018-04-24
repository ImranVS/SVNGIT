using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using DevExpress.Web;
using System.Collections;
using VSWebBL;
using VSWebDO;

namespace VSWebUI.Configurator
{
    public partial class TravelerDataStore : System.Web.UI.Page
    {
        DataTable TravelerDT;
		 int ID;
        string travelerDTVar = "TravelerDSGrid";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "TravelerDataStore|TravelerHAGrid")
                        {
                            TravelerHAGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                fillGridFromViewState();
            }
        }

        #region "LoadDataFunctions"
        public void fillGrid()
        {
            try
            {
                TravelerDT = VSWebBL.ConfiguratorBL.TravelerBL.Ins.GetTravelerHADataStore();
                if (TravelerDT.Rows.Count > 0)
                {
                    TravelerDT.PrimaryKey = new DataColumn[] { TravelerDT.Columns["ID"] };
                }
                //ViewState.Add(travelerDTVar, TravelerDT);
                Session[travelerDTVar] = TravelerDT;
                TravelerHAGrid.DataSource = TravelerDT;
                TravelerHAGrid.DataBind();
            }
            catch (Exception ex)
            {
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }
        public void fillGridFromViewState()
        {
            try
            {
                //if (ViewState[travelerDTVar] != null)
                //{
                //    TravelerDT = (DataTable)ViewState[travelerDTVar];
                //}
                if (Session[travelerDTVar] != null)
                {
                    TravelerDT = (DataTable)Session[travelerDTVar];
                }
                TravelerHAGrid.DataSource = TravelerDT;
                TravelerHAGrid.DataBind();  
            }
            catch (Exception ex)
            {
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }
        private void UpdateData(string mode, DataRow travelerrow)
        {
            try
            {
                if (mode == "Insert")
                {
                    Object ReturnValue = VSWebBL.ConfiguratorBL.TravelerBL.Ins.InsertTravelerDataStoreData(CollectData(mode, travelerrow));
                }
                if (mode == "Update")
                {

                    Object ReturnValue = VSWebBL.ConfiguratorBL.TravelerBL.Ins.UpdateTravelerDataStoreData(CollectData(mode, travelerrow));
                }
            }
            catch (Exception ex)
            {
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }
        private TravelerDS CollectData(string mode, DataRow travelerrow)
        {
            TravelerDS TravelerObject = new TravelerDS();
            ASPxComboBox txtDataStore = (ASPxComboBox)TravelerHAGrid.FindEditFormTemplateControl("DataStoreComboBox");
            ASPxComboBox txtIntegratedSecurity = (ASPxComboBox)TravelerHAGrid.FindEditFormTemplateControl("IntegratedSecurityComboBox");
            ASPxComboBox txtTestScan = (ASPxComboBox)TravelerHAGrid.FindEditFormTemplateControl("TestScanComboBox");
            try
            {
                if (mode == "Update")
                {
                    TravelerObject.ID = int.Parse(Convert.ToString(travelerrow["ID"]));
                }
                TravelerObject.TravelerPoolName = travelerrow["TravelerServicePoolName"].ToString();
                TravelerObject.ServerName = travelerrow["ServerName"].ToString();
                for (int i = 0; i < txtDataStore.Items.Count; i++)
                {
                    if (txtDataStore.Items[i].Value.ToString() == travelerrow["DataStore"].ToString())
                    {
                        txtDataStore.SelectedIndex = i;
                        break;
                    }
                }
                TravelerObject.DataStore = txtDataStore.SelectedItem.Value.ToString();
                if (travelerrow["Port"].ToString() == "-1")
                {
                    TravelerObject.Port = "NULL";
                }
                else
                {
                    TravelerObject.Port = travelerrow["Port"].ToString();
                }
                TravelerObject.Password = travelerrow["Password"].ToString();
                TravelerObject.UserName = travelerrow["UserName"].ToString();
                for (int i = 0; i < txtIntegratedSecurity.Items.Count; i++)
                {
                    if (txtIntegratedSecurity.Items[i].Text == travelerrow["IntegratedSecurity"].ToString())
                    {
                        txtIntegratedSecurity.SelectedIndex = i;
                        break;
                    }
                }
                if (txtIntegratedSecurity.SelectedItem.Text == "SQL Server")
                {
                    TravelerObject.IntegratedSecurity = "False";
                }
                else
                {
                    TravelerObject.IntegratedSecurity = "True";
                }
                for (int i = 0; i < txtTestScan.Items.Count; i++)
                {
                    if (txtTestScan.Items[i].Value.ToString() == travelerrow["TestScanServer"].ToString())
                    {
                        txtTestScan.SelectedIndex = i;
                        break;
                    }
                }

                if (txtTestScan.SelectedItem != null)
                {
                    TravelerObject.TestScan = txtTestScan.SelectedItem.Value.ToString();
                }
                else
                {
                    TravelerObject.TestScan = "";
                }

                TravelerObject.UsedByServers = travelerrow["UsedByServers"].ToString();
                //7/18/2014 NS added for VSPLUS-806
                TravelerObject.DatabaseName = travelerrow["DatabaseName"].ToString();
            }
            catch (Exception ex)
            {
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            return TravelerObject;
        }
        protected DataRow GetRow(DataTable TravelerDT, IDictionaryEnumerator enumerator, int Keys)
        {
            DataTable dataTable = TravelerDT;
            DataRow DRRow = dataTable.NewRow();
            try
            {
                if (Keys == 0)
                    DRRow = dataTable.NewRow();
                else
                    DRRow = dataTable.Rows.Find(Keys);
                enumerator.Reset();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Value == null)
                    {
                        if (enumerator.Key.ToString() == "Port")
                        {
                            DRRow[enumerator.Key.ToString()] = -1;
                        }
                        else
                        {
                            DRRow[enumerator.Key.ToString()] = "";
                        }
                    }
                    else
                    {
                        DRRow[enumerator.Key.ToString()] = enumerator.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            return DRRow;
        }
        #endregion
        protected void TravelerHAGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
				
				//PasswordLabel.Vissible = false;
                //if (ViewState[travelerDTVar] != null)
                //{
                //    TravelerDT = (DataTable)ViewState[travelerDTVar];
                //}
                if (Session[travelerDTVar] != null)
                {
                    TravelerDT = (DataTable)Session[travelerDTVar];
                }
                ASPxGridView gridView = (ASPxGridView)sender;
                //gridView.DoRowValidation();
                DataRow row =  GetRow(TravelerDT, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0]));
                if (!(row["UsedByServers"] == null || row["UsedByServers"].ToString() == ""))
                {
                    UpdateData("Update", row);
                }
                else
                {
                    
                    e.Cancel = true;
                    fillGrid();
                    throw new System.InvalidOperationException("Please select at least one server to use these credentials");
                }
                gridView.CancelEdit();
                e.Cancel = true;
                fillGrid();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Please select at least one server to use these credentials")
                    throw ex;
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void TravelerHAGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                TravelerDT = (DataTable)Session[travelerDTVar];
                ASPxGridView gridView = (ASPxGridView)sender;
                DataRow row = GetRow(TravelerDT, e.NewValues.GetEnumerator(), 0);
                if (!(row["UsedByServers"] == null || row["UsedByServers"].ToString() == ""))
                {
                    UpdateData("Insert", row);
                }
                else
                {
                    e.Cancel = true;
                    fillGrid();
                    throw new System.InvalidOperationException("Please select at least one server to use these credentials");
                }
                gridView.CancelEdit();
                e.Cancel = true;
                fillGrid();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Please select at least one server to use these credentials")
                    throw ex;
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void TravelerHAGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            //1/24/2014 NS added "manual" adding of the rows to the combobox in order to avoid the error:
            //Databinding methods such as Eval(), XPath(), and Bind() can only be used in the context of a databound control
            try
            {
                if (e.RowType == GridViewRowType.EditForm)
                {
                    ASPxComboBox txtTestScan = (ASPxComboBox)TravelerHAGrid.FindEditFormTemplateControl("TestScanComboBox");
                    txtTestScan.TextField = "TestScanServer";
                    txtTestScan.ValueField = "TestScanServer";
                    txtTestScan.DataSource = VSWebBL.ConfiguratorBL.TravelerBL.Ins.GetTravelerTestWhenScan();
                    //txtTestScan.DataBind();
                    ASPxTextBox txtTravelerPoolName = (ASPxTextBox)TravelerHAGrid.FindEditFormTemplateControl("TravelerPoolNameTextBox");
                    ASPxTextBox txtServerName = (ASPxTextBox)TravelerHAGrid.FindEditFormTemplateControl("ServerNameTextBox");
                    ASPxComboBox txtDataStore = (ASPxComboBox)TravelerHAGrid.FindEditFormTemplateControl("DataStoreComboBox");
                    ASPxTextBox txtPort = (ASPxTextBox)TravelerHAGrid.FindEditFormTemplateControl("PortTextBox");
                    ASPxTextBox txtUserName = (ASPxTextBox)TravelerHAGrid.FindEditFormTemplateControl("UserNameTextBox");
                    ASPxTextBox txtPwd = (ASPxTextBox)TravelerHAGrid.FindEditFormTemplateControl("PasswordTextBox");
                    ASPxComboBox txtIntegratedSecurity = (ASPxComboBox)TravelerHAGrid.FindEditFormTemplateControl("IntegratedSecurityComboBox");
                    ASPxDropDownEdit txtUsedBySrv = (ASPxDropDownEdit)TravelerHAGrid.FindEditFormTemplateControl("UsedByServersComboBox");
					ASPxLabel pwdlb = (ASPxLabel)TravelerHAGrid.FindEditFormTemplateControl("PasswordLabel");
					txtPwd.Visible = true;
					pwdlb.Visible = true;
                    if (!((ASPxGridView)(sender)).IsNewRowEditing)
					{
						txtPwd.Visible = false;
						pwdlb.Visible = false;

						//PasswordLabel.Visible = false;
                        txtTravelerPoolName.Value = e.GetValue("TravelerServicePoolName");
                        txtServerName.Value = e.GetValue("ServerName");
                        for (int i = 0; i < txtDataStore.Items.Count; i++)
                        {
                            if (txtDataStore.Items[i].Value.ToString() == e.GetValue("DataStore").ToString())
                            {
                                txtDataStore.SelectedIndex = i;
                                break;
                            }
                        }
                        txtPort.Value = e.GetValue("Port");
                        txtUserName.Value = e.GetValue("UserName");
                        txtPwd.Value = e.GetValue("Password");
                        int IntegratedSecurity = -1;
                        if (e.GetValue("IntegratedSecurity").ToString() == "Yes")
                        {
                            IntegratedSecurity = 1;
                        }
                        else
                        {
                            IntegratedSecurity = 0;
                        }
                        for (int i = 0; i < txtIntegratedSecurity.Items.Count; i++)
                        {
                            if (txtIntegratedSecurity.Items[i].Value.ToString() == IntegratedSecurity.ToString())
                            {
                                txtIntegratedSecurity.SelectedIndex = i;
                                break;
                            }
                        }
                        txtTestScan.Value = e.GetValue("TestScanServer");
                        txtUsedBySrv.Value = e.GetValue("UsedByServers");
                    }
                }
            }
            catch (Exception ex)
            {
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void TravelerHAGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            TravelerDS TravelerObject = new TravelerDS();
            TravelerObject.ID = Convert.ToInt32(e.Keys[0]);
            Object ReturnValue = VSWebBL.ConfiguratorBL.TravelerBL.Ins.DeleteTravelerDataStoreData(TravelerObject);
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            fillGrid();
        }

        protected void TravelerHAGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("TravelerDataStore|TravelerHAGrid", TravelerHAGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void TravelerHAGrid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
           /* if (e.Column.FieldName == "ServerType")
            {
                ASPxComboBox ServerTypeComboBox = e.Editor as ASPxComboBox;
                FillServerTypeComboBox(ServerTypeComboBox);
                ServerTypeComboBox.Callback += new CallbackEventHandlerBase(ServerTypeComboBox_OnCallback);


                ASPxListBox txtTestScan = (ASPxListBox)TravelerHAGrid.FindEditFormTemplateControl("TestScanComboBox");
                txtTestScan.TextField = "TestScanServer";
                txtTestScan.ValueField = "TestScanServer";
                txtTestScan.DataSource = VSWebBL.ConfiguratorBL.TravelerBL.Ins.GetTravelerTestWhenScan();



            }
            * */
        }

        protected void UsedByServersComboBox_OnDataBinding(object sender, EventArgs e)
        {

        }

        protected void UsedByServersListoBox_OnDataBinding(object sender, EventArgs e)
        {


            ASPxListBox lb = (ASPxListBox)sender;
            DataTable dt = VSWebBL.ConfiguratorBL.TravelerBL.Ins.GetTravelerTestWhenScan();


            lb.DataSource = dt;
            lb.TextField = "TestScanServer";
            lb.ValueField = "TestScanServer";
            lb.DataBind();

        }
		protected void btnrp_Click(object sender, ImageClickEventArgs e)
		{

			ImageButton btn = (ImageButton)sender;
			ID = Convert.ToInt32(btn.CommandArgument);
			int id = ID;
			TravelerDS TravelerObject = new TravelerDS();

			if (id != null)
				TravelerObject.ID = id;

			bool s = true;
			if (s == true)
			{

				Response.Redirect("~/Configurator/TravelerRegisterPassword.aspx?id=" + id.ToString(), false);
				Context.ApplicationInstance.CompleteRequest();

			}




		}
    }
}