using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebBL;
using VSWebDO;
using System.Data;

using DevExpress.Web;
using System.Runtime.InteropServices;

namespace VSWebUI
{
    public partial class AlertDefinitions_Grid : System.Web.UI.Page
    {
        //[DllImport("AlertLibrary.dll",EntryPoint="QueueAlert")]
        //public static extern void QueueAlert(string DeviceType, string DeviceName, string AlertType, string Details, string Location);
        protected void Page_Load(object sender, EventArgs e)
        {
            bool popupopen;
            Control ctrl;
            //Page.Title = "Alert Definitions";
            if (!IsPostBack)
            {              
                fillGrid();
                fillLocationComboBox();
                fillServerTypesComboBox();
                //fillEventsComboBox();
                //fillServerComboBox();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "AlertDefinitions_Grid|AlertDefGridView")
                        {
                            AlertDefGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                fillGridfromSession();
                //1/10/2013 NS added
                /*
                if (TestAlertSubmittedLabel.Text == "Test alert submitted.")
                {
                    successDiv.InnerHtml = "Test alert submitted.";
                    successDiv.Style.Add("display", "block");
                    //successDiv.Style.Value = "display: block";
                    TestAlertSubmittedLabel.Text = "";
                }
                 */
                //1/30/2014 NS added
                popupopen = CreateTestAlertPopupControl.ShowOnPageLoad;
                ctrl = GetPostBackControl(Page);
                if (popupopen && ctrl != null)
                {
                    if (ctrl.ID == "TestAlertButton")
                    {
                        //CreateTestAlert();
                    }
                }
            }
        }

        private void fillLocationComboBox()
        {
            //6/30/2014 NS added for VSPLUS-789
            try
            {
                DataTable LocationsTable = VSWebBL.SecurityBL.LocationsBL.Ins.GetAllData();
                if (LocationsTable.Rows.Count > 0)
                {
                    LocationComboBox.DataSource = LocationsTable;
                    LocationComboBox.TextField = "Location";
                    LocationComboBox.ValueField = "ID";
                    LocationComboBox.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        private void fillServerTypesComboBox()
        {
            //6/30/2014 NS added for VSPLUS-789
            try
            {
                DataTable ServerDataTable = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetAllData();
                if (ServerDataTable.Rows.Count > 0)
                {
                    DeviceTypeComboBox.DataSource = ServerDataTable;
                    DeviceTypeComboBox.TextField = "ServerType";
                    DeviceTypeComboBox.ValueField = "ID";
                    DeviceTypeComboBox.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        private void fillEventsComboBox()
        {
            string serverTypeIDs = "";
            //6/30/2014 NS added for VSPLUS-789
            try
            {
                DataTable EventsTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAllEvents(serverTypeIDs);
                if (EventsTable.Rows.Count > 0)
                {
                    EventTypeComboBox.DataSource = EventsTable;
                    EventTypeComboBox.TextField = "EventName";
                    EventTypeComboBox.ValueField = "ID";
                    EventTypeComboBox.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        private void fillServerComboBox()
        {
            //6/30/2014 NS added for VSPLUS-789
            try
            {
                DataTable ServersTable = VSWebBL.SecurityBL.ServersBL.Ins.GetAllData();
                if (ServersTable.Rows.Count > 0)
                {
                    ServerNameComboBox.DataSource = ServersTable;
                    ServerNameComboBox.TextField = "ServerName";
                    ServerNameComboBox.ValueField = "ID";
                    ServerNameComboBox.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        public void fillGrid()
        {
            DataTable AlertsTable = new DataTable();
            AlertsTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAlertNames();
            try
            {
                 Session["AlertsNameTable"] = AlertsTable;
                 AlertDefGridView.DataSource = AlertsTable;
                 AlertDefGridView.DataBind();
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
        }

        public void fillGridfromSession()
        {
            DataTable AlertsTable = new DataTable();
            if(Session["AlertsNameTable"]!=""&&Session["AlertsNameTable"]!=null)
            AlertsTable = (DataTable)Session["AlertsNameTable"];

            try
            {
                AlertDefGridView.DataSource = AlertsTable;
                AlertDefGridView.DataBind();
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }          
        
        }

        protected void AlertDefGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");


            if (e.RowType == GridViewRowType.EditForm)
            {

                //Mukund: VSPLUS-844, Page redirect on callback
                try
                {
                    if (e.GetValue("AlertKey") != " " && e.GetValue("AlertKey") != null)
                    {
                        ASPxWebControl.RedirectOnCallback("AlertDef_Edit.aspx?AlertKey=" + e.GetValue("AlertKey"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                    else
                    {
                        ASPxWebControl.RedirectOnCallback("AlertDef_Edit.aspx");
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    //throw ex;
                }
               
            }
        }

        protected void AlertDefGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }

        protected void AlertDefGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            AlertNames Alertobj = new AlertNames();
            Alertobj.AlertKey = Convert.ToInt32(e.Keys[0]);
            try
            {
                VSWebBL.ConfiguratorBL.AlertsBL.Ins.DeleteAlert(Alertobj);
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            ASPxGridView gridview = (ASPxGridView)sender;
            gridview.CancelEdit();
            e.Cancel = true;
            fillGrid();

          }

        protected void TestAlertButton_Click(object sender, EventArgs e)
        {
            //CreateTestAlertPopupControl.ShowOnPageLoad = false;
            //string script = string.Format("<script type=\"text/javascript\"> HidePopUp(); </script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "key", script, true);
            CreateTestAlertPopupControl.ShowOnPageLoad = false;
            CreateTestAlert();
        }

        protected void LocationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

			if (LocationComboBox.SelectedIndex != -1 && DeviceTypeComboBox.SelectedIndex != -1)
				populateDeviceTypeComboBox();

			
        }

        protected void DeviceTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string devicetypeids = "";
            if (DeviceTypeComboBox.SelectedIndex != -1)
            {
                devicetypeids = "'" + DeviceTypeComboBox.SelectedItem.Text.ToString() + "'";
                //6/30/2014 NS added for VSPLUS-789
                try
                {
                    DataTable EventsTable = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetAllEvents(devicetypeids);
                    if (EventsTable.Rows.Count > 0)
                    {
                        EventTypeComboBox.DataSource = EventsTable;
                        EventTypeComboBox.TextField = "EventName";
                        EventTypeComboBox.ValueField = "ID";
                        EventTypeComboBox.DataBind();
                        EventTypeComboBox.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    //6/27/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                }
            }

			if (LocationComboBox.SelectedIndex != -1 && DeviceTypeComboBox.SelectedIndex != -1)
				populateDeviceTypeComboBox();

        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            CreateTestAlertPopupControl.ShowOnPageLoad = false;
        }

        protected void SendTestAlertButton_Click(object sender, EventArgs e)
        {
            //1/10/2014 NS added
            successDiv.Style.Value = "display: none";
            LocationComboBox.SelectedIndex = -1;
            DeviceTypeComboBox.SelectedIndex = -1;
            EventTypeComboBox.SelectedIndex = -1;
            EventTypeComboBox.Enabled = false;
            ServerNameComboBox.SelectedIndex = -1;
            ServerNameComboBox.Enabled = false;
            CreateTestAlertPopupControl.ShowOnPageLoad = true;
        }

        protected void CreateTestAlert()
        {
            try
            {
                AlertLibrary.Alertdll alerts = new AlertLibrary.Alertdll();
                string DeviceType = "";
                string DeviceName = "";
                string AlertType = "";
                string Details = "This is a TEST alert.";
                string Location = "";
                int index = -1;
                if (DeviceTypeComboBox.SelectedIndex != -1)
                {
                    DeviceType = DeviceTypeComboBox.SelectedItem.Text.ToString();
                }
                if (LocationComboBox.SelectedIndex != -1)
                {
                    Location = LocationComboBox.SelectedItem.Text.ToString();
                }
                if (ServerNameComboBox.SelectedIndex != -1)
                {
                    DeviceName = ServerNameComboBox.SelectedItem.Text.ToString();
                }
                if (EventTypeComboBox.SelectedIndex != -1)
                {
                    AlertType = EventTypeComboBox.SelectedItem.Text.ToString();
                    index = AlertType.IndexOf("-");
                    if (index > -1)
                    {
                        AlertType = AlertType.Substring(index + 2, AlertType.Length - (index + 2));
                    }
                }
                alerts.QueueAlert(DeviceType, DeviceName, AlertType, Details, Location);
                successDiv.Style.Value = "display: block";
                //10/3/2014 NS modified for VSPLUS-990
                successDiv.InnerHtml = "Test alert created successfully."+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
            }
            catch (Exception ex)
            {
                errorDiv.Style.Value = "display: block";
                //10/3/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        //6/11/2013 NS added a new function that returns a control that triggered a postback (1/17/2014 NS added for VSPLUS-299)
        public static Control GetPostBackControl(Page page)
        {
            Control control = null;
            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form.AllKeys)
                {
                    Control c = page.FindControl(ctl) as Control;
                    if (c is DevExpress.Web.ASPxButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }

        protected void AlertDefGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AlertDefinitions_Grid|AlertDefGridView", AlertDefGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        
        }

		private void populateDeviceTypeComboBox()
		{
			//8/21/14 WS moved for VSPLUS-
			string location = "";
			DataTable dt = new DataTable();
			if (LocationComboBox.SelectedIndex != -1)
			{
				location = LocationComboBox.SelectedItem.Text.ToString();
				//2/5/2014 NS added
				if (DeviceTypeComboBox.SelectedItem.Text == "URL")
				{
					//6/30/2014 NS added for VSPLUS-789
					try
					{
						dt = VSWebBL.ConfiguratorBL.URLsBL.Ins.GetAllData();
						if (dt.Rows.Count > 0)
						{
							ServerNameComboBox.DataSource = dt;
							ServerNameComboBox.TextField = "Name";
							ServerNameComboBox.ValueField = "ID";
							ServerNameComboBox.DataBind();
							ServerNameComboBox.Enabled = true;
						}
					}
					catch (Exception ex)
					{
						//6/27/2014 NS added for VSPLUS-634
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					}
				}
				else
				{
					//6/30/2014 NS added for VSPLUS-789
					try
					{
						dt = VSWebBL.SecurityBL.AdminTabBL.Ins.Getserversbyloction(location);
						if (dt.Rows.Count > 0)
						{
							ServerNameComboBox.DataSource = dt;
							ServerNameComboBox.TextField = "ServerName";
							ServerNameComboBox.ValueField = "ID";
							ServerNameComboBox.DataBind();
							ServerNameComboBox.Enabled = true;
						}
					}
					catch (Exception ex)
					{
						//6/27/2014 NS added for VSPLUS-634
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					}
				}
			}
		}

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //3/2/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Configurator/AlertDef_Edit.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}