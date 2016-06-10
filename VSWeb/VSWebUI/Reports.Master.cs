using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Text;
using System.Globalization;
using VSWebDO;
using DevExpress.Web;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

namespace VSWebUI
{
    public partial class Reports : System.Web.UI.MasterPage
    {
        bool message;
        public void ProcessPageSessions()
        {
            DataTable dtPageSessions;
            if (Session["dtPageSessions"] == null)
            {
                dtPageSessions = VSWebBL.DashboardBL.DashboardBL.Ins.GetPageSessions();
                Session["dtPageSessions"] = dtPageSessions;
            }
            else
            {
                dtPageSessions = (DataTable)Session["dtPageSessions"];
            }

            for (int i = 0; i < dtPageSessions.Rows.Count; i++)
            {
                if (Request.Url.AbsoluteUri.ToLower().Contains(dtPageSessions.Rows[i]["PageLink"].ToString().Replace("~", "").ToLower()) && dtPageSessions.Rows[i]["PageLink"].ToString() != "")
                {
                    VSWebUI.UI.Ins.KillSessionsList(dtPageSessions.Rows[i]["PageLink"].ToString().Replace("~", ""), dtPageSessions.Rows[i]["SessionNames"].ToString());
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Masterpage"] = "~/Reports.Master";


            if (!IsPostBack)
            {
                ProcessPageSessions();

            }

            bool isDashBoardOnlyAccess = false;

            if ((Request.Url.AbsoluteUri.ToLower().Contains("overallhealth1.aspx") || Request.Url.AbsoluteUri.ToLower().Contains("devicetypelist.aspx") || Request.Url.AbsoluteUri.ToLower().Contains("summarylandscape.aspx") || Request.Url.AbsoluteUri.ToLower().Contains("maildeliverystatus.aspx")))
            {
                //if the session variable is not found or the session variable is empty, either the user is ciming in as dashboardonly mode or the session expired. Either cases, we'll change the mode to DashBoardOnlyMode
                if (Session["UserFullName"] == null || Session["UserFullName"].ToString() == "" || Session["UserFullName"].ToString() == "Anonymous")
                {
                    Session["IsDashboard"] = "true";
                    Session["UserFullName"] = "Anonymous";
                    isDashBoardOnlyAccess = true;
                    for (int i = 0; i < ASPxMenu1.Items.Count; i++)
                        ASPxMenu1.Items[i].Visible = false;
                    ASPxMenu1.Items[0].Visible = true;
                    ASPxMenu1.Items[ASPxMenu1.Items.Count - 1].Visible = true;

                    ASPxMenu1.Visible = true;
                    nameAndLogoutButton.Visible = false;

                }
                else
                {
                    CreateMenu1();
                }

            }
            else
            {
                CreateMenu1();
            }


            if (Session["FilterByValue"] == null || Session["FilterByValue"].ToString() == "")
            {
                Session["FilterByValue"] = "null";
            }
            if (Session["ViewBy"] == null || Session["FilterByValue"].ToString() == "")
            {
                Session["ViewBy"] = "ServerType";
            }


            if (Session["UserFullName"].ToString() != "Anonymous" || Session["SummaryEXJournal"] != null || isDashBoardOnlyAccess == true)
            {
                UserFullNameLabel.Text = Session["UserFullName"].ToString();
            }
            else
            {
                Response.Redirect("~/login.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
            if (Session["IsDashboard"] != null && Session["IsDashboard"].ToString() != "")
            {
                if (Convert.ToBoolean(Session["IsDashboard"].ToString()) == true)
                {
                    Session.Timeout = 600;
                    // UserNameLabel.Text = Session["UserFullName"].ToString();
                    //9/5/2013 NS commented out the !IsPostBack condition since otherwise page refresh did not take place
                    //when async postback was sent by the timer
                    //if (!IsPostBack)
                    //{
                    // Handle the session timeout 
                    string sessionExpiredUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/SessionExpired.aspx";
                    StringBuilder script = new StringBuilder();
                    script.Append("function expireSession(){ \n");
                    script.Append(string.Format(" window.location = '{0}';\n", sessionExpiredUrl));
                    script.Append("} \n");
                    script.Append(string.Format("setTimeout('expireSession()', {0}); \n", this.Session.Timeout * 60000)); // Convert minutes to milliseconds 
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "expirescript", script.ToString(), true);
                    //}
                    if (Session["Isconfigurator"] != null && Session["Isconfigurator"].ToString() != "")
                    {
                        if (Convert.ToBoolean(Session["Isconfigurator"].ToString()) == true)
                        {
                            ASPxMenu1.Items[6].Visible = true;

                            //  dashboard.Visible = true;
                        }
                    }

                    //Mukund added code 11-Oct-13, to make ExJournal,visible or not
                    string ExJournalEnabled = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Enable ExJournal");
                    if (ExJournalEnabled == "true")
                    {
                        ASPxMenu1.Items[0].Items[2].Visible = true;
                    }
                    else
                    {
                        ASPxMenu1.Items[0].Items[2].Visible = false;
                    }

                    Company cmpobj = new Company();
                    //1/6/2014 NS added
                    cmpobj = VSWebBL.ConfiguratorBL.CompanyBL.Ins.GetLogo();
                    //1/15/2014 NS modified
                    //logo.ImageUrl = cmpobj.LogoPath;//"/images/logo.png"; 
                    logo.Src = cmpobj.LogoPath;//"/images/logo.png"; 

                }
            }
            else if (Session["Isconfigurator"] != null && Session["Isconfigurator"] != "")
            {
                if (Convert.ToBoolean(Session["Isconfigurator"].ToString()) == true)
                {
                    Response.Redirect("~/Configurator/Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    Response.Redirect("~/login.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            else
            {
                //MD 30Dec2013
                if (Session["SummaryEXJournal"] != null)
                {
                    //Response.Redirect("~/Dashboard/SummaryEXJournal.aspx");
                    ASPxMenu1.Visible = false;
                    StatusBox1.Visible = false;
                    SearchTextBox.Visible = false;
                    ASPxButton1.Visible = false;
                    Session["SummaryEXJournal"] = null;
                }
                else
                {
                    Response.Redirect("~/SessionExpired.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
            }

            //2/8/2013 NS added
            //9/5/2013 NS commented out the !IsPostBack condition since otherwise page refresh did not take place
            //when async postback was sent by the timer
            //if (!IsPostBack)
            //{
            AssignStatusbox();
            //}
            //9/4/2013 NS added timer refresh interval; set in milliseconds; refreshtime comes from the Users table and should be set in seconds
            //Session["Refreshtime"] = "";
            if (Session["Refreshtime"] != "" && Session["Refreshtime"] != null)
            {
                timer1.Interval = Convert.ToInt32(Session["Refreshtime"]) * 1000;
            }

            //Mukund 16Jul14, VSPLUS-741, VSPLUS-785 Disable/Enable Timer to update count in Header boxes
            //Check MenuItems table for new fields SessionNames & TimerEnable
            DisableTimer();

            //10/27/2014 NS added for VSPLUS-1039
            if (CultureInfo.CurrentCulture.Name.Contains("zh-"))
            {
                fontLink.Href = "http://fonts.useso.com/css?family=Francois One";
            }
            else
            {
                fontLink.Href = "http://fonts.googleapis.com/css?family=Francois One";
            }
            PopulateReportMenu();


            Page.LoadComplete += new EventHandler(Page_LoadComplete);
            //2/25/2016 Durga Modified for VSPLUS-2634
            //2/11/2016 NS added for VSPLUS-2531
            int ind = -1;
            int diffInMin = 0;
            List<string> tempStatus = VSWebBL.DashboardBL.DashboardBL.Ins.GetProcessStatus();
            //3/1/2016 NS modified - when nothing was being returned, a red button showed up next to the menu
            //ASPxButton2.Text = (string.IsNullOrEmpty(tempStatus[0]) ? "Test" : "Updated: " + tempStatus[0].Replace("on ", ""));
            if (string.IsNullOrEmpty(tempStatus[0]))
            {
                ASPxButton2.Style.Value = "display: none";
            }
            else
            {
                ASPxButton2.Text = (string.IsNullOrEmpty(tempStatus[0]) ? "Test" : "Updated: " + tempStatus[0].Replace("on ", ""));
            }
            diffInMin = Convert.ToInt32(string.IsNullOrEmpty(tempStatus[1]) == true ? "0" : tempStatus[1]);
                if (diffInMin < 10)
                {
                    ASPxButton2.CssClass = "greenButton";
                   
                }
                else
                {
                    if (diffInMin >= 10 && diffInMin < 30)
                    {
                        ASPxButton2.CssClass = "yellowButton";
                    }
                    else
                    {
                        ASPxButton2.CssClass = "redButton";
                    }
                }
           
           

        }
   
        protected void LogoutLinkButton_Click(object sender, EventArgs e)
        {
            SignOut();
        }

        protected void SignOut()
        {
            try
            {
                string message = "<script language=JavaScript>window.history.forward(1);</script>";

                if (!Page.ClientScript.IsStartupScriptRegistered("clientscript"))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "clientscript", message, true);
                }

                FormsAuthentication.SignOut();
                Session.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //6/12/2013 NS moved the redirect line below from the try/catch block. Having the call within the block was throwing an exception
            //and therefore was not logging out a user correctly.
            Response.Redirect("~/Login.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            //Mukund 05Nov13, Handle page refresh Status List 
            try
            {
                //contentCallEvent(this, EventArgs.Empty);
            }
            catch
            {


            }

        }

        protected void AssignStatusbox()
        {
            if (Session["FilterByValue"] != "" && Session["FilterByValue"] != null)
            {
                DataTable dtOverall = VSWebBL.DashboardBL.DashboardBL.Ins.GetAllData(Session["FilterByValue"].ToString(), Session["ViewBy"].ToString());
                GetAllData(dtOverall, StatusBox1, 1);
            }
            DataTable dtTypes = new DataTable();

            if (Session["ViewBy"] == "" || Session["ViewBy"] == null)
            {
                Session["ViewBy"] = "Location";
            }
            if (Session["FilterByValue"] == "" || Session["FilterByValue"] == null)
            {
                Session["FilterByValue"] = "null";
            }
            if (Session["ViewBy"].ToString() == "ServerType")
            {
                dtTypes = VSWebBL.DashboardBL.DashboardBL.Ins.GetStatusbyType(Session["FilterByValue"].ToString());
            }
            if (Session["ViewBy"].ToString() == "Location")
            {
                dtTypes = VSWebBL.DashboardBL.DashboardBL.Ins.GetStatusbyLocation(Session["FilterByValue"].ToString());
            }

            //============Category

            if (Session["ViewBy"].ToString() == "Category")
            {
                dtTypes = VSWebBL.DashboardBL.DashboardBL.Ins.GetStatusbyCategory(Session["FilterByValue"].ToString());
            }


            //ASPxDataView1.DataSource = dtTypes;
            //ASPxDataView1.DataBind();

            //StatusLabel
        }

        protected void GetAllData(DataTable dtOverall, StatusBoxHeader objStatusBox, int iCol)
        {
            objStatusBox.Label31Text = "0";
            objStatusBox.Label21Text = "0";
            objStatusBox.Label41Text = "0";
            objStatusBox.Label11Text = "0";
            IBtn.Value = "0";
            MBtn.Value = "0";
            NRBtn.Value = "0";
            OKBtn.Value = "0";
            string calloutimg = "<img class=\"callout\" src=\"../images/callout.gif\" />";
            ILabel.InnerHtml = "0 server(s) have issues";
            MLabel.InnerHtml = "0 server(s) are in maintenance";
            NRLabel.InnerHtml = "0 server(s) are not responding";
            OKLabel.InnerHtml = "0 server(s) have no issues";
            DataTable dtRow1, dtRow2, dtRow3, dtRow4; DataRow[] results;
            if (dtOverall.Rows.Count > 0)
            {

                dtRow1 = dtOverall.Clone();
                results = dtOverall.Select("StatusCode='Issue'");
                foreach (DataRow dr in results) dtRow1.ImportRow(dr);

                if (dtRow1.Rows.Count > 0)
                {
                    objStatusBox.Label31Text = dtRow1.Rows[0][iCol].ToString();
                    IBtn.Value = dtRow1.Rows[0][iCol].ToString();
                    ILabel.InnerHtml = dtRow1.Rows[0][iCol].ToString() + " server(s) have issues";
                }

                dtRow2 = dtOverall.Clone();
                results = dtOverall.Select("StatusCode='Maintenance'");
                foreach (DataRow dr in results) dtRow2.ImportRow(dr);

                if (dtRow2.Rows.Count > 0)
                {
                    objStatusBox.Label41Text = dtRow2.Rows[0][iCol].ToString();
                    MBtn.Value = dtRow2.Rows[0][iCol].ToString();
                    MLabel.InnerHtml = dtRow2.Rows[0][iCol].ToString() + " server(s) are in maintenance";
                }

                dtRow3 = dtOverall.Clone();
                results = dtOverall.Select("StatusCode='Not Responding'");
                foreach (DataRow dr in results) dtRow3.ImportRow(dr);

                if (dtRow3.Rows.Count > 0)
                {
                    objStatusBox.Label11Text = dtRow3.Rows[0][iCol].ToString();
                    NRBtn.Value = dtRow3.Rows[0][iCol].ToString();
                    NRLabel.InnerHtml = dtRow3.Rows[0][iCol].ToString() + " server(s) are not responding";
                }

                dtRow4 = dtOverall.Clone();
                results = dtOverall.Select("StatusCode='OK'");
                foreach (DataRow dr in results) dtRow4.ImportRow(dr);

                if (dtRow4.Rows.Count > 0)
                {
                    objStatusBox.Label21Text = dtRow4.Rows[0][iCol].ToString();
                    OKBtn.Value = dtRow4.Rows[0][iCol].ToString();
                    OKLabel.InnerHtml = dtRow4.Rows[0][iCol].ToString() + " server(s) have no issues";
                }
            }
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
			if (!string.IsNullOrEmpty(SearchTextBox.Value.Trim()))
            Response.Redirect("/Dashboard/DeviceTypeList.aspx?server=" + SearchTextBox.Value, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        public void CreateMenu1()
        {
            try
            {
                //3/26/2015 NS modified for DevEx upgrade to 14.2
                //menu.Items.Clear();
                ASPxMenu1.Items.Clear();

                DataTable NavigatorTree = VSWebBL.SecurityBL.AdminTabBL.Ins.GetNavigatorByUserID(int.Parse(Session["UserID"].ToString()), "<=2", "Dashboard");
                DataView dataView = new DataView(NavigatorTree);
                dataView.Sort = "ParentID";

                // Build Menu Items
                Dictionary<string, DevExpress.Web.MenuItem> menuItems =
                    new Dictionary<string, DevExpress.Web.MenuItem>();

                for (int i = 0; i < dataView.Count; i++)
                {
                    DataRow row = dataView[i].Row;
                    if (row["DisplayText"].ToString() == "Configurator" && Session["Isconfigurator"] == null)
                    {
                        //do nothing, to stop adding configurator menu
                    }
                    else if (row["DisplayText"].ToString() == "Configurator" && Session["Isconfigurator"].ToString() == "False")
                    {
                        //do nothing, to stop adding configurator menu
                    }
                    else
                    {
                        DevExpress.Web.MenuItem item = CreateMenuItem(row);
                        string itemID = row["ID"].ToString();
                        string parentID = row["ParentID"].ToString();

                        if (menuItems.ContainsKey(parentID))
                            menuItems[parentID].Items.Add(item);
                        else
                        {
                            if (parentID == "") // It's Root Item
                                //3/26/2015 NS modified for DevEx upgrade to 14.2
                                //menu.Items.Add(item);
                                ASPxMenu1.Items.Add(item);
                        }
                        menuItems.Add(itemID, item);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
        }

        private DevExpress.Web.MenuItem CreateMenuItem(DataRow row)
        {
            DevExpress.Web.MenuItem ret = new DevExpress.Web.MenuItem();
            ret.Text = row["DisplayText"].ToString();
            ret.NavigateUrl = row["PageLink"].ToString();
            ret.BeginGroup = true;
            ret.Image.Url = row["ImageUrl"].ToString();
            return ret;
        }

        public void DisableTimer()
        {
            DataTable dtPageSessions;
            if (Session["dtPageSessions"] == null)
            {
                dtPageSessions = VSWebBL.DashboardBL.DashboardBL.Ins.GetPageSessions();
                Session["dtPageSessions"] = dtPageSessions;
            }
            else
            {
                dtPageSessions = (DataTable)Session["dtPageSessions"];
            }

            for (int i = 0; i < dtPageSessions.Rows.Count; i++)
            {
                //7/27/2015 NS modified
                if (Request.Url.AbsoluteUri.ToLower().Contains(dtPageSessions.Rows[i]["PageLink"].ToString().Replace("~", "").ToLower()) && dtPageSessions.Rows[i]["PageLink"].ToString() != "" ||
                    Request.Url.AbsoluteUri.ToLower().Contains("dashboardreports") || Request.Url.AbsoluteUri.ToLower().Contains("configuratorreports"))
                {
                    if (dtPageSessions.Rows[i]["TimerEnable"].ToString() == "False")
                    {
                        timer1.Enabled = false;
                    }
                }
            }
        }

        public void PopulateReportMenu()
        {
            DataTable ReportTable = new DataTable();
            DataColumn[] keys = new DataColumn[1];
            DataColumn column;
            NavBarGroup group;
            NavBarItem item;
            try
            {
                //8/31/2015 NS added for VSPLUS-2111
                ReportsMenu.Groups.Clear();
                if (Request.QueryString["M"] != null)
                {
                    if (Request.QueryString["M"].ToString() == "C")
                    {
                        ReportTable = VSWebBL.DashboardBL.ReportBL.Ins.GetAllData(true, Request.QueryString["M"].ToString());
                    }
                    else
                    {
                        ReportTable = VSWebBL.DashboardBL.ReportBL.Ins.GetAllData(false, Request.QueryString["M"].ToString());
                    }
                    column = ReportTable.Columns["ID"];
                    keys[0] = column;
                    ReportTable.PrimaryKey = keys;
                    Session["ReportMenuItems"] = ReportTable;
                    DataRow[] NavigatorGroupRows = (DataRow[])((ReportTable.DefaultView.ToTable(true, "Category")).Select());
                    foreach (DataRow NGRow in NavigatorGroupRows)
                    {
                        DataRow[] NavigatorItemRows = ReportTable.Select("Category='" + NGRow["Category"] + "'");
                        // Create Group1
                        group = new NavBarGroup(NGRow[0].ToString(), NGRow[0].ToString());
                        // Set up items
                        foreach (DataRow NGItem in NavigatorItemRows)
                        {
                            //item = new NavBarItem(NGItem["Name"].ToString(), NGItem["Name"].ToString(), "", NGItem["PageURL"].ToString());
                            item = new NavBarItem();
                            item.Text = NGItem["Name"].ToString();
                            item.Name = NGItem["Name"].ToString();
                            item.NavigateUrl = (NGItem["PageURL"].ToString()).Replace("..", "~");
                            group.Items.Add(item);
                        }
                        ReportsMenu.Groups.Add(group);
                    }
                }
            }
            catch (Exception ex)
            {
                //5/15/2014 NS modified for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                foreach (NavBarGroup group in ReportsMenu.Groups)
                    group.Expanded = false;
                


            }
            
        }

        protected void SearchRptButton_Click(object sender, EventArgs e)
        {


            DataTable ReportTable = new DataTable();
            NavBarGroup group;
            NavBarItem item;

            var selected = new List<NavBarGroup>();
            if (Session["ReportMenuItems"] != null)
            {
                foreach (NavBarGroup g in ReportsMenu.Groups)
                {
                    selected.Add(g);
                }
                foreach (NavBarGroup gs in selected)
                {
                    ReportsMenu.Groups.Remove(gs);
                }
                ReportTable = (DataTable)Session["ReportMenuItems"];
                DataRow[] NavigatorGroupRows = (DataRow[])((ReportTable.DefaultView.ToTable(true, "Category")).Select());
                foreach (DataRow NGRow in NavigatorGroupRows)
                {
                    if (SearchRptTextBox.Text.Contains("servers") == true)
                    {

                        // SearchRptTextBox.Text.Replace("servers", "server");
                        string s = SearchRptTextBox.Text;
                        string servalue = s.Replace("servers", "server");
                        SearchRptTextBox.Text = servalue.ToString();

                    }
                    else
                    {
                        string servalue = SearchRptTextBox.Text;

                    }


                    DataRow[] NavigatorItemRows = ReportTable.Select("Category='" + NGRow["Category"] + "' AND Name like'%" + SearchRptTextBox.Text + "%' ");
                    // Create Group1


                    group = new NavBarGroup(NGRow[0].ToString(), NGRow[0].ToString());
                    // Set up items
                    foreach (DataRow NGItem in NavigatorItemRows)
                    {
                        //item = new NavBarItem(NGItem["Name"].ToString(), NGItem["Name"].ToString(), "", NGItem["PageURL"].ToString());
                        item = new NavBarItem();
                        item.Text = NGItem["Name"].ToString();
                        item.Name = NGItem["Name"].ToString();
                        item.NavigateUrl = (NGItem["PageURL"].ToString()).Replace("..", "~");
                        group.Items.Add(item);
                    }
                    if (group.Items.Count > 0)
                    {
                        ReportsMenu.Groups.Add(group);
                        lblerror.Visible = false;

                      


                    }
                    else if (group.Items.Count == 0)
                    {
                        if (ReportsMenu.Items.Count == 0)
                        {
                            lblerror.Visible = true;
                        }
                    }
                   

                }
            }

        }


        protected void ClearRptButton_Click(object sender, EventArgs e)
        {
            SearchRptTextBox.Text = "";
            PopulateReportMenu();
            foreach (NavBarGroup group in ReportsMenu.Groups)
                group.Expanded = false;
            lblerror.Visible = false;
        }

        protected void SchedRptButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/Reports.aspx?M=d", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void menu_Init(object sender, EventArgs e)
        {
            DevExpress.Web.ASPxMenu menu = (DevExpress.Web.ASPxMenu)sender;
            try
            {
                //3/26/2015 NS modified for DevEx upgrade to 14.2
                //menu.Items.Clear();
                menu.Items.Clear();
                DataTable NavigatorTree = VSWebBL.SecurityBL.AdminTabBL.Ins.GetNavigatorByUserID(int.Parse(Session["UserID"].ToString()), "<=2", "Dashboard");
                DataView dataView = new DataView(NavigatorTree);
                //dataView.Sort = "ParentID";

                // Build Menu Items
                Dictionary<string, DevExpress.Web.MenuItem> menuItems =
                    new Dictionary<string, DevExpress.Web.MenuItem>();

                for (int i = 0; i < dataView.Count; i++)
                {
                    DataRow row = dataView[i].Row;
                    if (row["DisplayText"].ToString() == "Configurator" && Session["Isconfigurator"] == null)
                    {
                        //do nothing, to stop adding configurator menu
                    }
                    else if (row["DisplayText"].ToString() == "Configurator" && Session["Isconfigurator"].ToString() == "False")
                    {
                        //do nothing, to stop adding configurator menu
                    }
                    else
                    {
                        DevExpress.Web.MenuItem item = CreateMenuItem(row);
                        string itemID = row["ID"].ToString();
                        string parentID = row["ParentID"].ToString();

                        if (menuItems.ContainsKey(parentID))
                            menuItems[parentID].Items.Add(item);
                        else
                        {
                            if (parentID == "") // It's Root Item
                                //3/26/2015 NS modified for DevEx upgrade to 14.2
                                //menu.Items.Add(item);
                                menu.Items.Add(item);
                        }
                        menuItems.Add(itemID, item);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw;
            }
        }

        protected void ASPxMenu2_ItemClick(object source, MenuItemEventArgs e)
        {
            if (e.Item.Name == "HelpItem")
            {
                getAssemblyVersionInfo();
                getBuildInfo();
                ASPxPopupControl1.ShowOnPageLoad = true;
            }
        }

        private void getAssemblyVersionInfo()
        {
            DataTable AssemblyVersionInfo = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.getAssemblyVersionInfo();
            AssemblyGridView.DataSource = AssemblyVersionInfo;
            AssemblyGridView.DataBind();
            AssemblyGridView.Styles.Cell.CssClass = "GridCss";
            AssemblyGridView.Styles.Header.CssClass = "GridCssHeader";
            AssemblyGridView.KeyFieldName = "NodeName";

            //grid.SettingsBehavior.AllowFocusedRow = true;
            AssemblyGridView.SettingsBehavior.AllowDragDrop = true;
            ((GridViewDataColumn)AssemblyGridView.Columns[0]).GroupBy();

            AssemblyGridView.Styles.InlineEditCell.CssClass = "GridCssHeader";


        }

        private void getBuildInfo()
        {
            string vsVersion = "VSx.x_SPRT_xx_RCxx";
            DataTable DBVersionInfo = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetDatabaseVersionInfo("VS_BUILD");
            if (DBVersionInfo.Rows.Count > 0)
            {
                vsVersion = DBVersionInfo.Rows[0]["VALUE"].ToString();
            }
            lblVersion.InnerText = "Build: " + vsVersion;
        }

        protected void ASPxButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/ServiceController.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}