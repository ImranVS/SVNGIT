using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Security;
using System.Data;
using System.Text;
using System.Globalization;
using VSWebBL;
using VSWebDO;
namespace VSWebUI
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["Isconfigurator"] == null)
            {
                if (Session["UserFullName"] == null)
                {
                    Session["UserFullName"] = "Anonymous";
                    Session["IsDashboard"] = "true";
                    Session["UserFullName"] = "Anonymous";
                }
                Response.Redirect("~/Dashboard/OverallHealth1.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (Session["Isconfigurator"].ToString() == "False")
            {
                if (Session["UserFullName"] == null)
                {
                    Session["UserFullName"] = "Anonymous";
                    Session["IsDashboard"] = "true";
                    Session["UserFullName"] = "Anonymous";
                }
                Response.Redirect("~/Dashboard/OverallHealth1.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
            Session.Timeout = 60;
            Company cmpobj = new Company();

            cmpobj = VSWebBL.ConfiguratorBL.CompanyBL.Ins.GetLogo();
            logo.ImageUrl = cmpobj.LogoPath;//"/images/logo.png"; 

            if (!IsPostBack)
			{//1/21/2016 Durga modified for VSPLUS-2474
				ProcessPageSessions();
				CheckMasterService();

                if (Request.QueryString["hidesubmenu"] != null)
                {
                    SubMenu.Items.Clear();
                    SessionsKill();
                    SubMenu.Items.Clear();
                    Session["GroupIndex"] = null;
                    Session["ItemIndex"] = null;
                    Session["Submenu"] = null;
                    Session["MenuID"] = "";
                    SubMenu.Visible = false;
                }



                // Handle the session timeout 
                Session["Masterpage"] = "~/Site1.Master";
                
               
                string sessionExpiredUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/SessionExpired.aspx";
                StringBuilder script = new StringBuilder();
                script.Append("function expireSession(){ \n");
                script.Append(string.Format(" window.location = '{0}';\n", sessionExpiredUrl));
                script.Append("} \n");
                script.Append(string.Format("setTimeout('expireSession()', {0}); \n", this.Session.Timeout * 60000)); // Convert minutes to milliseconds 
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "expirescript", script.ToString(), true);


                if (Session["RestrictedMenus"] != null)
                {
                    if (Session["RestrictedMenus"].ToString() != "")
                    {
                        string path = HttpContext.Current.Request.Url.AbsolutePath;
                        DataTable RestrictedDataTable = (DataTable)Session["RestrictedMenus"];
                        DataRow[] RestrictedRow = RestrictedDataTable.Select("PageLink='" + path + "'");
                        if (RestrictedRow.Count() > 0)
                        {
                            SignOut();
                        }

                    }
                }

                CreateMenu();
				//Sowjanya ticket-1811
				bool success = VSWebBL.SecurityBL.UsersBL.Ins.GetIsFirstTimeLogin(Convert.ToInt32(Session["UserID"]));
				if (success == true)
				{
					
				    MainMenu.Visible = false;
					ASPxMenu1.Visible = false;					
				}
				
				
            }

            if (Session["UserFullName"] != null)
            {
                UserFullNameLabel.Text = Session["UserFullName"].ToString();
            }
      
            //Submenus Level-3 from DB
            if (Session["MenuID"] != "" && Session["MenuID"] != null)
            {
                DataTable dtL3 = (DataTable)Session["Level3Menus"];
                DataTable dtl3Filter = dtL3.Clone();
                DataRow[] drL3 = dtL3.Select("ParentId=" + Session["MenuID"]);
                foreach (DataRow row in drL3)
                {
                    dtl3Filter.ImportRow(row);
                }
                if (dtl3Filter.Rows.Count > 0)
                {
                    //5/13/2014 NS added the line below per Mukund's email
                    SubMenu.Items.Clear();
                    SubMenu.Visible = true;
                    DataView dataView = new DataView(dtl3Filter);
                    dataView.Sort = "ParentID";
                    // Build SubMenu Items
                    Dictionary<string, DevExpress.Web.MenuItem> menuItems = new Dictionary<string, DevExpress.Web.MenuItem>();
                    for (int i = 0; i < dataView.Count; i++)
                    {
                        DataRow row = dataView[i].Row;
                        DevExpress.Web.MenuItem item = CreateMenuItem(row);
                        string itemID = row["ID"].ToString();
                        string parentID = row["ParentID"].ToString();
                        SubMenu.Items.Add(item);
                        menuItems.Add(itemID, item);
                    }
                    //SubMenu.Items[0].Selected = true;
                }
            }
           
                if (Session["GroupIndex"] != null)
                {
                    MainMenu.Groups[Convert.ToInt32(Session["GroupIndex"].ToString())].Items[Convert.ToInt32(Session["ItemIndex"].ToString())].Selected = true;
                    MainMenu.Groups[Convert.ToInt32(Session["GroupIndex"].ToString())].Expanded = true;
                }

           
            //7/2/2013 NS added

            //if (Session["ViewBy"] == null) Session["ViewBy"] = "ServerType";
            //if (Session["FilterByValue"] == null) Session["FilterByValue"] = "null";

            if (Session["FilterByValue"] == null || Session["FilterByValue"].ToString() == "")
            {
                Session["FilterByValue"] = "null";
            }
            if (Session["ViewBy"] == null || Session["FilterByValue"].ToString() == "")
            {
                Session["ViewBy"] = "ServerType";
            }
            //10/3/2013 NS commented out the !IsPostBack condition since otherwise page refresh did not take place
            //when async postback was sent by the timer
            //if (!IsPostBack)
            //{
            AssignStatusbox();
            //}
            //10/3/2013 NS added timer refresh interval; set in milliseconds; refreshtime comes from the Users table and should be set in seconds
            //12/17/2013 NS uncommented out 
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
            //3/26/2015 NS added for DevEx upgrade to 14.2
			
					CreateMenu1();
			
			//WS COMMENTED FOR SAFE COMMIT
			//SetUpCircles();
        }


		//WS COMMENTED FOR SAFE COMMIT
		//public void SetUpCircles()
		//{
		//    //0  1  2  3 4  5 6 7
		//    //D DT DH MS C CC A M

		//    string str = "";
		//    bool DominoEnabled = true;
		//    bool MicrosoftEnabled = true;
		//    DataTable ConfigDt = VSWebBL.SecurityBL.MenusBL.Ins.GetSelectedFeatures();

		//    for (int i = 0; i < ConfigDt.Rows.Count; i++)
		//    {
		//        string s = ConfigDt.Rows[i]["Name"].ToString();
		//        if (ConfigDt.Rows[i]["Name"].ToString() == "Domino")
		//        {
		//            DominoEnabled = true;
		//            circleTbl.Rows[1].Cells[0].Visible = true;
		//            circleTbl.Rows[1].Cells[5].Visible = true;
		//            circleTbl.Rows[1].Cells[2].Visible = true;

		//        }
		//        if (ConfigDt.Rows[i]["Name"].ToString() == "Exchange" || ConfigDt.Rows[i]["Name"].ToString() == "Active Directory" || ConfigDt.Rows[i]["Name"].ToString() == "Sharepoint" || ConfigDt.Rows[i]["Name"].ToString() == "Windows" || ConfigDt.Rows[i]["Name"].ToString() == "Lync")
		//        {
		//            MicrosoftEnabled = true;
		//            circleTbl.Rows[1].Cells[7].Visible = true;
		//        }

		//    }

		//    circleTbl.Rows[1].Cells[1].Visible = false;
		//    circleTbl.Rows[1].Cells[2].Visible = false;
			
		//    str += setCircleAttributes(0, "VitalSignsPlusDomino", "VitalSigns for Domino");
		//    //str += setCircleAttributes(1, "VitalSigns Daily Tasks", "Daily Tasks");
		//    //str += setCircleAttributes(2, "VitalSigns Database Health", "Database Health");
		//    str += setCircleAttributes(3, "VitalSigns Plus Master Service", "Master Service");
		//    str += setCircleAttributes(4, "VitalSignsPlusCore", "Core Features");
		//    str += setCircleAttributes(5, "VSConsoleCommand", "Remote Console Commands");
		//    str += setCircleAttributes(6, "VitalSignsAlerts", "Alerting");
		//    str += setCircleAttributes(7, "VitalSignsMicrosoft", "VitalSigns for Microsoft");

			
		//    circleTbl.Attributes.Add("OnMouseOver", "tblmouseover('" + str + "')");
		//    circleTbl.Attributes.Add("OnMouseOut", "tblmouseout()");
           
		//}

		//public string setCircleAttributes(int index, string ControllerName, string FriendlyName)
		//{
		//    ServiceController VSController;

		//    string str = "";
		//    try
		//    {
		//        VSController = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == ControllerName);
		//        if (VSController == null)
		//        {
		//            str += FriendlyName + ": <font color=FF0000>Not Installed </font><br />";
		//            circleTbl.Rows[1].Cells[index].Style["background-color"] = "red";
		//            return str;
		//        }
		//        string color = VSController.Status.ToString() == "Running" ? "green" : "red";
		//        string hexColor = color == "red" ? "FF0000" : "00FF00";
		//        circleTbl.Rows[1].Cells[index].Style["background-color"] = color;
		//        //str += "VitalSigns for Domino: <p style='color:" + color + "'>" + VSDominoController.Status.ToString() + "</p><br />";
		//        str += FriendlyName + ": <font color=" + hexColor + ">" + VSController.Status.ToString() + "</font><br />";

		//    }
		//    catch (Exception ex)
		//    {
		//        circleTbl.Rows[1].Cells[0].Style["background-color"] = "red";
		//        str += FriendlyName + ": Not Found <br />";
		//    }

		//    return str;
		//}

        //Mukund 16Jul14, VSPLUS-741, VSPLUS-785 Disable/Enable Timer to update count in Header boxes
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
                if (Request.Url.AbsoluteUri.ToLower().Contains(dtPageSessions.Rows[i]["PageLink"].ToString().Replace("~", "").ToLower()) && dtPageSessions.Rows[i]["PageLink"].ToString() != "")
                {
                    if (dtPageSessions.Rows[i]["TimerEnable"].ToString() == "False")
                    {
                        timer1.Enabled = false;
                    }
                }
            }
        }


        protected void MainMenu_ItemClick(object source, NavBarItemEventArgs e)
        {
            Session["GroupIndex"] = e.Item.Group.Index;
            Session["ItemIndex"] = e.Item.Index;
            e.Item.Selected = true;
            Session["Submenu"] = "";
            Session["SubItemIndex"] = null;

            Session["Submenu"] = e.Item.Name;
            SubMenu.Items.Clear();
            SubMenu.Visible = false;
            
            Session["MenuID"] = "";

            DataTable dt = (DataTable)Session["NavigatorTree"];
            DataRow[] dr = dt.Select("RefName='" + e.Item.Name + "'");
            if (dr.Length > 0)
            {
                Session["MenuID"] = dr[0]["ID"].ToString();
                if (dr[0]["PageLink"].ToString() != "")
                {
                    Response.Redirect(dr[0]["PageLink"].ToString());
                }            
               

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
        protected void MainMenu_HeaderClick(object source, NavBarGroupCancelEventArgs e)
        {
            //5/13/2014 NS added the line below per Mukund's email
            SubMenu.Items.Clear();

            SessionsKill();
            
            SubMenu.Items.Clear();

            if (Session["GroupIndex"] != null)
                MainMenu.Groups[Convert.ToInt32(Session["GroupIndex"])].Expanded = false;
            Session["GroupIndex"] = null;
            Session["ItemIndex"] = null;
            Session["Submenu"] = null;

            if (Session["SubmenuItem"] != null)
            {
                if(SubMenu.Items.Count>0)
                SubMenu.Items[0].Selected = false;
                Session["SubmenuItem"] = null;
            }
            DataTable dt = (DataTable)Session["NavigatorTree"];
            DataRow[] dr = dt.Select("RefName='" + e.Group.Name+"'");
            if (dr.Length > 0)
            {
                if (dr[0]["PageLink"].ToString() != "")
                {
                    Session["MenuID"] = "";
                    
                    SubMenu.Visible = false;
                    Response.Redirect(dr[0]["PageLink"].ToString());
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

        public void CreateMenu()
        {
            try
            {
                if (Session["NavBar"] == null)
                {
                    if (Session["UserID"] != "" && Session["UserFullName"] != "" && Session["UserID"] != null && Session["UserFullName"] != null)
                    {
                        //ASPxNavBar nbRuntime = new ASPxNavBar();
                        NavBarGroup group;
                        NavBarItem item;
                        DataTable NavigatorTree = VSWebBL.SecurityBL.AdminTabBL.Ins.GetNavigatorByUserID(int.Parse(Session["UserID"].ToString()), "<=2", "Configurator");
                        DataTable Level3Menus = VSWebBL.SecurityBL.AdminTabBL.Ins.GetLevel3Menus("Configurator");
                        Session["NavigatorTree"] = NavigatorTree;
                        Session["Level3Menus"] = Level3Menus;
                        DataRow[] NavigatorGroupRows = NavigatorTree.Select("Level=1");

                        DataTable RestrictedNavigatorTree = VSWebBL.SecurityBL.AdminTabBL.Ins.GetRestrictedNavigatorByUserID(Session["UserFullName"].ToString(), "<=3");
                        Session["RestrictedMenus"] = RestrictedNavigatorTree;
                        if (Session["IsDashboard"] != null && Session["Isconfigurator"] != null)
                        {
                            foreach (DataRow NGRow in NavigatorGroupRows)
                            {
                                if (NGRow["RefName"].ToString() != "Dashboard" && NGRow["RefName"].ToString() != "MyAccount")
                                {

                                    if (Convert.ToBoolean(Session["Isconfigurator"].ToString()) == true)
                                    {

                                        //MainMenu0.Groups.Add(NGRow["DisplayText"], NGRow["DisplayText"].Replace("  ", string.Empty));
                                        //NavigatorTree.Rows[i]["DisplayText"],NavigatorTree.Rows[i]["DisplayText"].Replace("  ", string.Empty));
                                        DataRow[] NavigatorItemRows = NavigatorTree.Select("ParentID=" + NGRow["ID"]);
                                        // Create Group1
                                        group = new NavBarGroup(NGRow["DisplayText"].ToString(), NGRow["RefName"].ToString(), NGRow["ImageURL"].ToString());
                                        // Set up items
                                        foreach (DataRow NGItem in NavigatorItemRows)
                                        {
                                            item = new NavBarItem(NGItem["DisplayText"].ToString(), NGItem["RefName"].ToString(), NGItem["ImageURL"].ToString(), "");//NGItem["PageLink"].ToString()
											group.Items.Add(item);
                                        }
                                        MainMenu.Groups.Add(group);
                                    }
                                }

                                else
                                {
                                    if (Convert.ToBoolean(Session["IsDashboard"].ToString()) == true)
                                    {
                                        DataRow[] NavigatorItemRows = NavigatorTree.Select("ParentID=" + NGRow["ID"]);
                                        // Create Group1
                                        group = new NavBarGroup(NGRow["DisplayText"].ToString(), NGRow["RefName"].ToString(), NGRow["ImageURL"].ToString());
                                        // Set up items
                                        foreach (DataRow NGItem in NavigatorItemRows)
                                        {
                                            item = new NavBarItem(NGItem["DisplayText"].ToString(), NGItem["RefName"].ToString(), NGItem["ImageURL"].ToString(), "");//NGItem["PageLink"].ToString()
                                            group.Items.Add(item);
                                        }
                                        MainMenu.Groups.Add(group);
                                    }
                                    if (Convert.ToBoolean(Session["IsDashboard"].ToString()) != true && Convert.ToBoolean(Session["Isconfigurator"].ToString()) == true)
                                    {
                                        if (NGRow["RefName"].ToString() == "MyAccount")
                                        {
                                            DataRow[] NavigatorItemRows = NavigatorTree.Select("ParentID=" + NGRow["ID"]);
                                            // Create Group1
                                            group = new NavBarGroup(NGRow["DisplayText"].ToString(), NGRow["RefName"].ToString(), NGRow["ImageURL"].ToString());
                                            // Set up items
                                            foreach (DataRow NGItem in NavigatorItemRows)
                                            {
                                                item = new NavBarItem(NGItem["DisplayText"].ToString(), NGItem["RefName"].ToString(), NGItem["ImageURL"].ToString(), "");//NGItem["PageLink"].ToString()
                                                group.Items.Add(item);
                                            }
                                            MainMenu.Groups.Add(group);
                                        }
                                    }

                                }

                            }
                        }
                        // MainMenu.Groups[1].Expanded = true;
                        // Session["NavBar"] = nbRuntime;
                    }
                    //MainMenu = (ASPxNavBar)Session["NavBar"];
                    //Page.Controls.Add(MainMenu);
                }
                else
                {
                    Response.Redirect("~/Login.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void SessionsKill()
        {
            Session["BlackBerryServers"] = "";
            Session["MaintServers"] = "";
            Session["BlackBerryDevicePrbegrid"] = "";
            Session["DominoCluster"] = "";
            Session["DominoCustom"] = "";
            Session["NotesDB"] = "";
            Session["DominoServer"] = "";
            Session["sametime"] = "";
            Session["MailServices"] = "";
            Session["MaintServers"] = "";
            Session["NetworkDevices"] = "";
            Session["NotesDatabase"] = "";
            Session["NotesMailProbe"] = "";
            Session["URLs"] = "";
            Session["ServerVisibleDataGrid"] = "";
            Session["visible"] = "";
            Session["ServerNotVisibleDataGrid"] = "";
            Session["NotVisible"] = "";
            Session["Servers"] = "";
            Session["Users"] = "";
            Session["Locations"] = "";


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

        protected void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
               //contentCallEvent(this, EventArgs.Empty);                
            }
            catch
            {


            }

        }
        //Mukund 05Nov13, Handle page refresh Status List 
        public event EventHandler contentCallEvent;

        public void refreshStatusBoxes()
        {

            DataTable dtOverall = VSWebBL.DashboardBL.DashboardBL.Ins.GetAllData(Session["FilterByValue"].ToString(), Session["ViewBy"].ToString());
            GetAllData(dtOverall, StatusBox1, 1);

        }

        //3/26/2015 NS added for DevEx upgrade to 14.2
        public void CreateMenu1()
        {
            try
            {
                //menu.Items.Clear();
                ASPxMenu1.Items.Clear();

                DataTable NavigatorTree = VSWebBL.SecurityBL.AdminTabBL.Ins.GetNavigatorByUserID(int.Parse(Session["UserID"].ToString()), "<=3", "Configurator");
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
                        string level = row["Level"].ToString();
						//Sowjanya for ticket VSPLUS-2519
						if (item.Text.Equals("Documentation") || item.Text.Equals("Support Portal"))
						{
							item.Target = "_blank";
						}
                        if (menuItems.ContainsKey(parentID))
                            menuItems[parentID].Items.Add(item);
                        else
                        {
                            if (parentID == "") // It's Root Item
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

		//12/14/15 WS added for VSPLUS-2446
		public void CheckMasterService()
		{
			if (!VSWebBL.SecurityBL.AdminTabBL.Ins.AnyNodeAlive())
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Error while updating collection for status of Nodes.");
			}
		}
		//1/21/2016 Durga modified for VSPLUS-2474
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
    }
}