using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using VSWebBL;


using DevExpress.Web;


using System.Web.UI.HtmlControls;

using System.Text;
using System.Reflection;

 


namespace VSWebUI.Dashboard
{
    public partial class OverallHealthDocking : System.Web.UI.Page
    {
        //To sync with Master page refresh below 2 events are used Page_PreInit, Master_ButtonClick
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Mukund 05Nov13, Create an event handler for the master page's contentCallEvent event
            this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
        }
        private void Master_ButtonClick(object sender, EventArgs e)
        {
            //Mukund 05Nov13, This Method will be Called from Timer Click from Master page
            AssignStatusbox();
            

        }


        
        DataTable dt = new DataTable();
        // string viewby = "ServerType";           
        string str3;
        string loginname;
        protected void Page_Load(object sender, EventArgs e)
        {

          //  DominoDock.Attributes.Add("onmouseover", "this.style.BorderColor='black';");

          //  DominoDock.Attributes.Add("onmouseout", "this.style.BorderColor=this.originalcolor;");



            int index = 0;
// Iterate through the visible panels which are not owned by other panels. 
           


            if (Session["UserLogin"] == null || Session["UserLogin"] == "")
            {
                menutable.Visible = false;
            }

          

            bool exists = true;
            string pathdata = "";
            if (Session["CustomBackground"] != null)
            {
                pathdata = Session["CustomBackground"].ToString();
                exists = System.IO.Directory.Exists(Server.MapPath(pathdata));
            }

            if (!exists)
            {
                ((HtmlGenericControl)this.Page.Master.FindControl("wrapdiv")).Style["background-image"] = Page.ResolveUrl(pathdata);
                //((HtmlGenericControl)this.Page.Master.FindControl("wrapdiv")).Style["background-size"] = "cover";
            }
            else
            {
                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("wrapdiv");
                body.Attributes.Add("style", "background-color:#f8f8c0;");

            }

            //HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("wrapdiv");
            ////body.Attributes.Add("style", "background-image:url(~/Images/Reset.jpg) ;height:345px;</style>");
            ////body.Attributes.Add("style", "background-color:#FF5050;");
            //body.Attributes.Add("background-image", "url(~\\LogFiles\\Penguins.jpg)");
            //body.Attributes.Add("onunload", "Function2()");
            // Master.Attributes.Add("style", "background-image:url('" + ResolveUrl("~/Images/" + GetImage.Images) + "')");

            if (!IsPostBack)
            {

                if (Session["FilterByValue"] == null) Session["FilterByValue"] = "null";
                if (Session["ViewBy"] == null) Session["ViewBy"] = "ServerType";
                if (Session["ViewBy"].ToString() == "ServerType")
                {
                    Accordion1.SelectedIndex = 0;
                }
                else if (Session["ViewBy"].ToString() == "Location")
                {
                    Accordion1.SelectedIndex = 1;
                }
                else
                {
                    Accordion1.SelectedIndex = 2;
                }

                ASPxCheckBoxList checkbxlist = (ASPxCheckBoxList)PopupControl2.FindControl("checkBoxList1");
                if (Session["ViewBy"].ToString() != "Location")
                {
                    checkbxlist.DataSource = VSWebBL.StatusBL.StatusTBL.Ins.DistinctLocations();
                    checkbxlist.TextField = "Location";
                    checkbxlist.ValueField = "Location";
                }
                else
                {
                    checkbxlist.DataSource = VSWebBL.StatusBL.StatusTBL.Ins.DistinctServerTypes();
                    checkbxlist.TextField = "Type";
                    checkbxlist.ValueField = "Type";
                }
                checkbxlist.DataBind();

                if (Session["FilterLabel"] == null || Session["FilterLabel"].ToString() == "")
                {
                    FilterLabel.Text = "Supporting Details are grouped by Server Type";  //lnkViewByServerType.Text;
                }
                else
                {
                    FilterLabel.Text = Session["FilterLabel"].ToString();
                }

                //PopupControl2.Enabled = true;

                SetLinkVisibility();
                KeyMetrics(Session["FilterByValue"].ToString());
            }
            //25Feb2014 MD moved the Mail box out of the !IsPostback block
            if (Session["ViewBy"] == null) Session["ViewBy"] = "ServerType";
            if (Session["FilterByValue"] == null) Session["FilterByValue"] = "null";
            //KeyMetrics(Session["FilterByValue"].ToString());
            //12/12/2013 NS moved the status box calculations out of the !IsPostback block
            if (!IsPostBack)
            {
                AssignStatusbox();
            }

            
            //if (Session["Refreshtime"] != "" && Session["Refreshtime"] != null)
            //    timerupdate.Interval = Convert.ToInt32(Session["Refreshtime"]) * 1000;
            //else 
            //    timerupdate.Interval = Convert.ToInt32(Session["Refreshtime"]) * 1000;

            //lblcount.Text = System.DateTime.Now.ToString();
            // lbl.Text = System.DateTime.Now.ToString(); ;

            //HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
            //int refreshtime = Convert.ToInt32(Session["Refreshtime"]);
            //body.Attributes.Add("onload", "timedRefresh(" + refreshtime + "*1000)");
            //usertime  is in seconds 1s = 1000
            //GetCloudData();
            //GetNetworkInfraData();
            //GetSNMPData();
            //KeyMetrics(Session["FilterByValue"].ToString());
            if (Session["UserLogin"] != null && Session["UserLogin"] != "")
            {
                loginname = Session["UserLogin"].ToString();

                
                
                DataTable dtonpremises = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.Getonpremiseswithuser(loginname);
                Session["checkeddata"] = dtonpremises;
                //string cloud = dtonpremises.Rows[0]["CloudApplications"].ToString();
                bool CloudApplications = false;
                CloudApplications = Convert.ToBoolean(dtonpremises.Rows[0]["CloudApplications"]);
                bool OnPremisesApplications = Convert.ToBoolean(dtonpremises.Rows[0]["OnPremisesApplications"]);
                bool NetworkInfrastucture = Convert.ToBoolean(dtonpremises.Rows[0]["NetworkInfrastucture"]);
                bool DominoServerMetrics = Convert.ToBoolean(dtonpremises.Rows[0]["DominoServerMetrics"]);
                if (CloudApplications == false && OnPremisesApplications == false && NetworkInfrastucture == false && DominoServerMetrics == false)
                {
                    try
                    {
                        visiblealldata();
                        
                    }
                    catch
                    {
                    }
                }
                else
                {
                    GetCloudData();
                    GetNetworkInfraData();
                    if (DominoServerMetrics == true)
                    {
                        KeyMetrics(Session["FilterByValue"].ToString());
                    }
                    //if (OnPremisesApplications == true)
                    //{
                    //    AssignStatusbox();
                    //}
                }
            }
            else
            {
                visiblealldatadasbord();
                
            }


            
            

        }

        protected void UpdatePanel_Unload(object sender, EventArgs e)
        {
            MethodInfo methodInfo = typeof(ScriptManager).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(i => i.Name.Equals("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel")).First();
            methodInfo.Invoke(ScriptManager.GetCurrent(Page),
                new object[] { sender as UpdatePanel });
        }
        protected void dockManager_ClientLayout(object sender, DevExpress.Web.ASPxClientLayoutArgs e)
        {
           
            //const string LayoutSessionKey = "1e38ba85-292e-494e-8f3e-5c8654a9dfef";
            //bool saveLayout = false;

            //if (e.LayoutMode == ClientLayoutMode.Saving)
            //    Session[LayoutSessionKey] = e.LayoutData;
            //if (e.LayoutMode == ClientLayoutMode.Loading)
            //    e.LayoutData = Session[LayoutSessionKey] as string;
           
            if (e.LayoutMode == ClientLayoutMode.Saving)
                Session["LayoutSessionKey"] = e.LayoutData;
            if (e.LayoutMode == ClientLayoutMode.Loading)
                e.LayoutData = Session["LayoutSessionKey"] as string;

            
            string s = e.LayoutData.ToString();

             s = s.Replace("'", "");
            //char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            //string text = s;
            
            string[] split = s.Split(new Char[] { ',', ']' });
            string[] getting = split;
            string[] spliting = s.Split(new char[] { '[', ',','"'});
            string[] getdock = spliting;
           
            if (s!= "")
            {
                int visible1 = Convert.ToInt32(getting[7]);
                int visible2 = Convert.ToInt32(getting[16]);
                int visible3 = Convert.ToInt32(getting[25]);
                int visible4 = Convert.ToInt32(getting[34]);
                string cloudzone1 = spliting[3];
                string premisesZone2 = spliting[12];
                string networkZone3 = spliting[21];
                string DockZone4 = spliting[30];
                string cloudzone = cloudzone1.Replace("'", "");
                string premisesZone = premisesZone2.Replace("'", "");
                string networkZone = networkZone3.Replace("'", "");
                string DockZone = DockZone4.Replace("'", "");
                
                //CloudDock.VisibleIndex = Convert.ToInt32(visible1.ToString());
                //PremisesDock.VisibleIndex = Convert.ToInt32(visible2.ToString());
                //NetworkDock.VisibleIndex = Convert.ToInt32(visible3.ToString());
                //DominoDock.VisibleIndex = Convert.ToInt32(visible4.ToString());
                int id = Convert.ToInt32(Session["UserID"]);
                 VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.savedock(visible1, visible2, visible3, visible4,id,cloudzone,premisesZone,networkZone,DockZone,s);
                // savedock();

            }
            savedock();
                    //String[] edit = s.Split(']');
            //string[] getting = edit;

        }
        protected void savedock()
        {
            DataTable dt = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetDockData(Convert.ToInt32(Session["UserID"]));
            if(dt.Rows.Count>0)
            {
            CloudDock.VisibleIndex = Convert.ToInt32(dt.Rows[0]["cloudindex"].ToString());
            PremisesDock.VisibleIndex = Convert.ToInt32(dt.Rows[0]["premisesindex"].ToString());
            NetworkDock.VisibleIndex = Convert.ToInt32(dt.Rows[0]["networkindex"].ToString());
            DominoDock.VisibleIndex = Convert.ToInt32(dt.Rows[0]["dockindex"].ToString());
            CloudDock.OwnerZoneUID = dt.Rows[0]["cloudZone"].ToString();
            PremisesDock.OwnerZoneUID = dt.Rows[0]["premisesZone"].ToString();
            NetworkDock.OwnerZoneUID = dt.Rows[0]["networkZone"].ToString();
            DominoDock.OwnerZoneUID = dt.Rows[0]["DockZone"].ToString();
            }
            //Page.ClientScript.RegisterStartupScript(this.GetType(),"Script", "MyFunction();", true);
        }
        protected void GetCloudData()
        {
            ASPxDataView2.Visible = false;
            CloudDock.Visible = false;
            //VSPLUS-1020,16Oct14, Swathi, Checking if Cloud is selected
            DataTable dt = new DataTable();
            dt = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.IsCloudSelected();
            if (dt.Rows.Count > 0)
            {
                loginname = Session["UserLogin"].ToString();
                DataTable dtonpremises = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.Getonpremiseswithuser(loginname);
                Session["checkeddata"] = dtonpremises;
                //string cloud = dtonpremises.Rows[0]["CloudApplications"].ToString();
                bool CloudApplications = false;
                CloudApplications = Convert.ToBoolean(dtonpremises.Rows[0]["CloudApplications"]);
                if (CloudApplications)
                {
                    dt = new DataTable();
                    //dt = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.GetCloudData();
                    //dt = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.GetCloudDatavisible();
                    //WS changed.  The user credentials in the configurator have nothing to do with which user can view the application, that is used only to log into the application in the 
                    //service.  Every user should have access to view every cloud application on the dashboard, so there is no need to join the status table with the users table.
                    dt = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.GetCloudStatuses();
                    Session["clouddata"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        // Image1.ImageUrl = dt.Rows[0]["Image"].ToString();
                        ASPxDataView2.DataSource = dt;
                        ASPxDataView2.DataBind();
                        ASPxDataView2.Visible = true;
                        CloudDock.Visible = true;
                    }
                }
                else
                {
                    ASPxDataView2.Visible = false;
                    CloudDock.Visible = false;

                }
            }
        }
       // ASPxDockManager dockManager = new ASPxDockManager();
        public void KeyMetrics(string ServerLoc)
        {

            DataTable dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetKeyMetrics(ServerLoc);

            foreach (DataRow row in dt.Rows)
            {
                object value = row["UserCount"];
                if (value != DBNull.Value)
                {
                    PendingLabel.Text = "Pending Mail: " + dt.Rows[0]["PendingMail"].ToString();
                    LblDead.Text = "Dead Mail: " + dt.Rows[0]["DeadMail"].ToString();
                    HeldLabel.Text = "Held Mail: " + dt.Rows[0]["HeldMail"].ToString();

                    UsersLabel.Text = dt.Rows[0]["UserCount"].ToString();

                    RespLabel.Text = dt.Rows[0]["Resp"].ToString() + " ms";
                    DwnTimeLabel.Text = dt.Rows[0]["DownMinutes"].ToString() + " minutes";
                    keymetrictable.Visible = true;
                    DominoDock.Visible = true;
                    //if (Session["ServiceStatus"] != "" && Session["ServiceStatus"] != null)
                    //{
                    //    StatusLabel.Text = Session["ServiceStatus"].ToString();
                    //}
                }
                else
                {
                    keymetrictable.Visible = false;
                    DominoDock.Visible = false;
                }

            }
        }
        public void KeyMetricsvisible(string ServerLoc)
        {

            DataTable dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetKeyMetricsvisible(ServerLoc);

            foreach (DataRow row in dt.Rows)
            {
                object value = row["UserCount"];
                if (value != DBNull.Value)
                {
                    PendingLabel.Text = "Pending Mail: " + dt.Rows[0]["PendingMail"].ToString();
                    LblDead.Text = "Dead Mail: " + dt.Rows[0]["DeadMail"].ToString();
                    HeldLabel.Text = "Held Mail: " + dt.Rows[0]["HeldMail"].ToString();

                    UsersLabel.Text = dt.Rows[0]["UserCount"].ToString();

                    RespLabel.Text = dt.Rows[0]["Resp"].ToString() + " ms";
                    DwnTimeLabel.Text = dt.Rows[0]["DownMinutes"].ToString() + " minutes";
                    keymetrictable.Visible = true;
                    DominoDock.Visible = true;
                    //if (Session["ServiceStatus"] != "" && Session["ServiceStatus"] != null)
                    //{
                    //    StatusLabel.Text = Session["ServiceStatus"].ToString();
                    //}
                }
                else
                {
                    keymetrictable.Visible = false;
                    DominoDock.Visible = false;
                }

            }
        }
        protected void GetNetworkInfraData()
        {
            ASPxDataView3.Visible = false;
            NetworkDock.Visible = false;
            DataTable dt = new DataTable();
            dt = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.IsNetworkdeviceSelected();
            if (dt.Rows.Count > 0)
            {
                loginname = Session["UserLogin"].ToString();
                DataTable dtonpremises2 = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.Getonpremiseswithuser(loginname);
                Session["checkeddata"] = dtonpremises2;
                bool NetworkInfrastucture = false;
                NetworkInfrastucture = Convert.ToBoolean(dtonpremises2.Rows[0]["NetworkInfrastucture"]);
                if (NetworkInfrastucture)
                {
                    dt = new DataTable();
                    //dt = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetCloudData();
                    dt = VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetNetworkdevicevisibleData();
                    Session["clouddata"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        // Image1.ImageUrl = dt.Rows[0]["Image"].ToString();
                        ASPxDataView3.DataSource = dt;
                        ASPxDataView3.DataBind();

                        ASPxDataView3.Visible = true;
                        NetworkDock.Visible = true;
                    }
                }
                else
                {
                    ASPxDataView3.Visible = false;
                    NetworkDock.Visible = false;
                }
            }
        }
        //protected void GetSNMPData()
        //{
        //    ASPxDataView3.Visible = false;
        //    CloudDock.Visible = false;
        //    PremisesDock.Visible = false;
        //    NetworkDock.Visible = false;
        //    DataTable dt = new DataTable();
        //    dt = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.IsSNMPSelected();
        //    if (dt.Rows.Count > 0)
        //    {
        //        dt = new DataTable();
        //        //dt = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetCloudData();
        //        dt = VSWebBL.ConfiguratorBL.SNMPDevicesBL.Ins.GetSNMPdevicedData();
        //        Session["SNMPdata"] = dt;
        //        if (dt.Rows.Count > 0)
        //        {
        //            // Image1.ImageUrl = dt.Rows[0]["Image"].ToString();
        //            ASPxDataView3.DataSource = dt;
        //            ASPxDataView3.DataBind();

        //            ASPxDataView3.Visible = true;
        //            CloudDock.Visible = true;
        //            PremisesDock.Visible = true;
        //            NetworkDock.Visible = true;
        //        }
        //        else
        //        {
        //            ASPxDataView3.Visible = false;
        //            CloudDock.Visible = false;
        //            NetworkDock.Visible = false;
        //        }
        //    }

        //}

        protected void AssignStatusbox()
        {
            //12/12/2013 NS added
           
            if (Session["FilterByValue"] != "" && Session["FilterByValue"] != null)
            {
                //2/12/2013 NS modified
                DataTable dtOverall = VSWebBL.DashboardBL.DashboardBL.Ins.GetAllData(Session["FilterByValue"].ToString(), Session["ViewBy"].ToString());
                //GetAllData(dtOverall, StatusBox1, 1);

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

            //CY: VS 157



            if (Session["FilterByValue"].ToString() != "null" && Session["ViewBy"].ToString() == "ServerType" && str3 != null && str3 != "")
            {
                FilterLabel.Text = "Supporting Details are grouped by Server Type" + ", filtered by Location in " + str3;
                Session["FilterLabel"] = FilterLabel.Text;
            }
            else if (Session["FilterByValue"].ToString() == "null" && Session["ViewBy"].ToString() == "ServerType")
            {
                FilterLabel.Text = "Supporting Details are grouped by Server Type";
                Session["FilterLabel"] = FilterLabel.Text;
            }
            if (Session["FilterByValue"].ToString() != "null" && Session["ViewBy"].ToString() == "Location" && str3 != null && str3 != "")
            {
                FilterLabel.Text = "Supporting Details are grouped by Location" + ", filtered by Server Type in " + str3;
                Session["FilterLabel"] = FilterLabel.Text;
            }
            else if (Session["FilterByValue"].ToString() == "null" && Session["ViewBy"].ToString() == "Location")
            {
                FilterLabel.Text = "Supporting Details are grouped by Location";
                Session["FilterLabel"] = FilterLabel.Text;
            }
            if (Session["FilterByValue"].ToString() != "null" && Session["ViewBy"].ToString() == "Category" && str3 != null && str3 != "")
            {
                FilterLabel.Text = "Supporting Details are grouped by Category" + ", filtered by Location in " + str3;
                Session["FilterLabel"] = FilterLabel.Text;
            }
            else if (Session["FilterByValue"].ToString() == "null" && Session["ViewBy"].ToString() == "Category")
            {
                FilterLabel.Text = "Supporting Details are grouped by Category";
                Session["FilterLabel"] = FilterLabel.Text;
            }
            else if (Session["FilterByValue"].ToString() != "null" && (Session["ViewBy"].ToString() != "" || Session["ViewBy"].ToString() != "null"))
            {
                string strFilter = (Session["ViewBy"].ToString() == "ServerType" || Session["ViewBy"].ToString() == "Category" ? "Location" : "Server Type");
                FilterLabel.Text = "Supporting Details are grouped by " + Session["ViewBy"].ToString() + ", filtered by " + strFilter + " in " + Session["FilterByValue"].ToString().Replace("'", "");
                Session["FilterLabel"] = FilterLabel.Text;

            }

            //ASPxDataView1.DataSource = dtTypes;
            //ASPxDataView1.DataBind();

            //StatusLabel

            //12/12/2013 NS modified
            //StatusLabel.Text = VSWebBL.DashboardBL.DashboardBL.Ins.GetProcessStatus();
            //2/25/2016 Durga Modified for VSPLUS-2634
            int ind = -1;
            int diffInMin = 0;
            List<string> tempStatus = VSWebBL.DashboardBL.DashboardBL.Ins.GetProcessStatus();


            StatusLabel.Text = string.IsNullOrEmpty(tempStatus[0]) ? "Test" : tempStatus[0];
            diffInMin = Convert.ToInt32(string.IsNullOrEmpty(tempStatus[1]) == true ? "0" : tempStatus[1]);
                if (diffInMin < 10)
                {
                    lastUpdID.Attributes["class"] = "divColorGreen";
                  
                }
                else
                {
                    if (diffInMin >= 10 && diffInMin < 30)
                    {
                        lastUpdID.Attributes["class"] = "divColorYellow";
                      
                    }
                    else
                    {
                        lastUpdID.Attributes["class"] = "divColorRed";
                      
                    }
                }

                if (StatusLabel.Text != "Stopped" && StatusLabel.Text != "" && StatusLabel.Text != "Test")
            {

                DateLabel.Text = StatusLabel.Text.Substring(0, StatusLabel.Text.IndexOf("(") - 1);
                TimeZoneLabel.Text = StatusLabel.Text.Substring(StatusLabel.Text.IndexOf("("));
                //2/7/2013 NS modified
                //StatusLabel.Text = "Running";
                StatusLabel.Text = "Last Update";
            }
            else
            {
                DateLabel.Text = "";
                TimeZoneLabel.Text = "";
            }
            if (Session["UserLogin"] != null && Session["UserLogin"] != "")
            {
                loginname = Session["UserLogin"].ToString();

                DataTable dtonpremises = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.Getonpremiseswithuser(loginname);
                Session["checkeddata"] = dtonpremises;

                bool value = false;
                value = Convert.ToBoolean(dtonpremises.Rows[0]["OnPremisesApplications"]);

                try
                {
                    if (value == true)
                    {
                        tablebutons.Visible = true;
                        ASPxDataView1.Visible = true;
                        PremisesDock.Visible = true;
                        ASPxDataView1.DataSource = dtTypes;
                        ASPxDataView1.DataBind();

                    }
                    else
                    {
                        PremisesDock.Visible = false;
                        tablebutons.Visible = false;
                        ASPxDataView1.Visible = false;
                    }
                }
                catch
                {


                }

            }
        }
        protected void AssignStatusvisiblebox()
        {
            
            if (Session["FilterByValue"] != "" && Session["FilterByValue"] != null)
            {
                //2/12/2013 NS modified
                DataTable dtOverall = VSWebBL.DashboardBL.DashboardBL.Ins.GetAllData(Session["FilterByValue"].ToString(), Session["ViewBy"].ToString());
                //GetAllData(dtOverall, StatusBox1, 1);

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

            //CY: VS 157



            if (Session["FilterByValue"].ToString() != "null" && Session["ViewBy"].ToString() == "ServerType" && str3 != null && str3 != "")
            {
                FilterLabel.Text = "Supporting Details are grouped by Server Type" + ", filtered by Location in " + str3;
                Session["FilterLabel"] = FilterLabel.Text;
            }
            else if (Session["FilterByValue"].ToString() == "null" && Session["ViewBy"].ToString() == "ServerType")
            {
                FilterLabel.Text = "Supporting Details are grouped by Server Type";
                Session["FilterLabel"] = FilterLabel.Text;
            }
            if (Session["FilterByValue"].ToString() != "null" && Session["ViewBy"].ToString() == "Location" && str3 != null && str3 != "")
            {
                FilterLabel.Text = "Supporting Details are grouped by Location" + ", filtered by Server Type in " + str3;
                Session["FilterLabel"] = FilterLabel.Text;
            }
            else if (Session["FilterByValue"].ToString() == "null" && Session["ViewBy"].ToString() == "Location")
            {
                FilterLabel.Text = "Supporting Details are grouped by Location";
                Session["FilterLabel"] = FilterLabel.Text;
            }
            if (Session["FilterByValue"].ToString() != "null" && Session["ViewBy"].ToString() == "Category" && str3 != null && str3 != "")
            {
                FilterLabel.Text = "Supporting Details are grouped by Category" + ", filtered by Location in " + str3;
                Session["FilterLabel"] = FilterLabel.Text;
            }
            else if (Session["FilterByValue"].ToString() == "null" && Session["ViewBy"].ToString() == "Category")
            {
                FilterLabel.Text = "Supporting Details are grouped by Category";
                Session["FilterLabel"] = FilterLabel.Text;
            }
            else if (Session["FilterByValue"].ToString() != "null" && (Session["ViewBy"].ToString() != "" || Session["ViewBy"].ToString() != "null"))
            {
                string strFilter = (Session["ViewBy"].ToString() == "ServerType" || Session["ViewBy"].ToString() == "Category" ? "Location" : "Server Type");
                FilterLabel.Text = "Supporting Details are grouped by " + Session["ViewBy"].ToString() + ", filtered by " + strFilter + " in " + Session["FilterByValue"].ToString().Replace("'", "");
                Session["FilterLabel"] = FilterLabel.Text;

            }

            //ASPxDataView1.DataSource = dtTypes;
            //ASPxDataView1.DataBind();

            //StatusLabel

            //12/12/2013 NS modified
            int ind = -1;
            int diffInMin = 0;
            List<string> tempStatus = VSWebBL.DashboardBL.DashboardBL.Ins.GetProcessStatus();


            StatusLabel.Text = string.IsNullOrEmpty(tempStatus[0]) ? "Test" : tempStatus[0];
            diffInMin = Convert.ToInt32(string.IsNullOrEmpty(tempStatus[1]) == true ? "0" : tempStatus[1]);
                if (diffInMin < 10)
                {
                    lastUpdID.Attributes["class"] = "divColorGreen";
                   
                }
                else
                {
                    if (diffInMin >= 10 && diffInMin < 30)
                    {
                        lastUpdID.Attributes["class"] = "divColorYellow";
                        
                    }
                    else
                    {
                        lastUpdID.Attributes["class"] = "divColorRed";
                       
                    }
                }

                if (StatusLabel.Text != "Stopped" && StatusLabel.Text != "" && StatusLabel.Text != "Test")
            {

                DateLabel.Text = StatusLabel.Text.Substring(0, StatusLabel.Text.IndexOf("(") - 1);
                TimeZoneLabel.Text = StatusLabel.Text.Substring(StatusLabel.Text.IndexOf("("));
                //2/7/2013 NS modified
                //StatusLabel.Text = "Running";
                StatusLabel.Text = "Last Update";
            }
            else
            {
                DateLabel.Text = "";
                TimeZoneLabel.Text = "";
            }
            tablebutons.Visible = true;
            ASPxDataView1.Visible = true;
            PremisesDock.Visible = true;
            ASPxDataView1.DataSource = dtTypes;
            ASPxDataView1.DataBind();

            //    }
            //    else
            //    {
            //        PremisesDock.Visible = false;
            //        tablebutons.Visible = false;
            //        ASPxDataView1.Visible = false;
            //    }
            //}
        }
        protected void visiblealldata()
        {



            dt = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.GetCloudDatavisible();
            Session["clouddata"] = dt;
            if (dt.Rows.Count > 0)
            {
                // Image1.ImageUrl = dt.Rows[0]["Image"].ToString();
                ASPxDataView2.DataSource = dt;
                ASPxDataView2.DataBind();
                ASPxDataView2.Visible = true;
                CloudDock.Visible = true;

            }

            else
            {
                ASPxDataView2.Visible = false;
                CloudDock.Visible = false;
            }


            dt = VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetNetworkdevicevisibleData();
            Session["Networkdata"] = dt;
            if (dt.Rows.Count > 0)
            {
                // Image1.ImageUrl = dt.Rows[0]["Image"].ToString();
                ASPxDataView3.DataSource = dt;
                ASPxDataView3.DataBind();

                ASPxDataView3.Visible = true;

                NetworkDock.Visible = true;
            }

            else
            {
                ASPxDataView3.Visible = false;

                NetworkDock.Visible = false;
            }
            KeyMetricsvisible(Session["FilterByValue"].ToString());
            AssignStatusvisiblebox();
        }
        protected void visiblealldatadasbord()
        {
            dt = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.GetCloudDatavisiblefordashboard();
            Session["clouddata"] = dt;
            if (dt.Rows.Count > 0)
            {
                // Image1.ImageUrl = dt.Rows[0]["Image"].ToString();
                ASPxDataView2.DataSource = dt;
                ASPxDataView2.DataBind();
                ASPxDataView2.Visible = true;
                CloudDock.Visible = true;

            }
            else
            {
                ASPxDataView2.Visible = false;
                CloudDock.Visible = false;
            }
            dt = VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetNetworkDatavisiblefordashboard();
            Session["Networkdata"] = dt;
            if (dt.Rows.Count > 0)
            {
                // Image1.ImageUrl = dt.Rows[0]["Image"].ToString();
                ASPxDataView3.DataSource = dt;
                ASPxDataView3.DataBind();

                ASPxDataView3.Visible = true;

                NetworkDock.Visible = true;
            }
            else
            {
                ASPxDataView3.Visible = false;

                NetworkDock.Visible = false;
            }
            KeyMetricsvisible(Session["FilterByValue"].ToString());
            AssignStatusvisiblebox();
        }
        protected void GetAllData(DataTable dtOverall, StatusBox objStatusBox, int iCol)
        {
            objStatusBox.Label31Text = "0";
            objStatusBox.Label21Text = "0";
            objStatusBox.Label41Text = "0";
            objStatusBox.Label11Text = "0";
            DataTable dtRow1, dtRow2, dtRow3, dtRow4; DataRow[] results;
            if (dtOverall.Rows.Count > 0)
            {

                dtRow1 = dtOverall.Clone();
                results = dtOverall.Select("StatusCode='Issue'");
                foreach (DataRow dr in results) dtRow1.ImportRow(dr);

                if (dtRow1.Rows.Count > 0)
                {
                    objStatusBox.Label31Text = dtRow1.Rows[0][iCol].ToString();
                }

                dtRow2 = dtOverall.Clone();
                results = dtOverall.Select("StatusCode='Maintenance'");
                foreach (DataRow dr in results) dtRow2.ImportRow(dr);

                if (dtRow2.Rows.Count > 0)
                {
                    objStatusBox.Label41Text = dtRow2.Rows[0][iCol].ToString();
                }

                dtRow3 = dtOverall.Clone();
                results = dtOverall.Select("StatusCode='Not Responding'");
                foreach (DataRow dr in results) dtRow3.ImportRow(dr);

                if (dtRow3.Rows.Count > 0)
                {
                    objStatusBox.Label11Text = dtRow3.Rows[0][iCol].ToString();
                }

                dtRow4 = dtOverall.Clone();
                results = dtOverall.Select("StatusCode='OK'");
                foreach (DataRow dr in results) dtRow4.ImportRow(dr);

                if (dtRow4.Rows.Count > 0)
                {
                    objStatusBox.Label21Text = dtRow4.Rows[0][iCol].ToString();
                }
            }
        }

        protected void btnSeleCtAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSeleCtAll.Text == "Select All")
                {
                    checkBoxList1.SelectAll();
                    btnSeleCtAll.Text = "UnselectAll";
                }
                else if (btnSeleCtAll.Text == "UnselectAll")
                {
                    checkBoxList1.UnselectAll();
                    btnSeleCtAll.Text = "Select All";
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;

            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSeleCtAll.Text == "Select All")
                {
                    checkBoxList1.UnselectAll();
                    btnSeleCtAll.Text = "Select All";
                }
                else if (btnSeleCtAll.Text == "UnselectAll")
                {
                    checkBoxList1.SelectAll();
                    btnSeleCtAll.Text = "UnselectAll";
                }

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;

            }
            PopupControl2.ShowOnPageLoad = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        protected void Save()
        {
            try
            {
                //12/12/2013 NS modified
                ASPxCheckBoxList checkbxlist = (ASPxCheckBoxList)PopupControl2.FindControl("checkBoxList1");

                string str1 = null;
                for (int i = 0; i < checkbxlist.SelectedValues.Count; i++)
                {
                    str1 += "'" + checkbxlist.SelectedItems[i].Text + "',";
                }
                //4/11/2014 NS commented out the line below - whoever is doing testing, please use other methods,
                //the messagebox command throws and error on 147 !!! 
                //MessageBox.Show(str1);
                if (str1 != "" && str1 != null)
                {
                    if (str1.Contains("Mail Probe"))
                        str1 += "'NotesMail Probe',";
                    string str2 = str1.Remove(str1.LastIndexOf(","));
                    Session["FilterByValue"] = str2;
                    if (Session["ViewBy"] == "ServerType")
                        Session["MyServerTypes"] = str2;
                    //=================================

                    if (Session["ViewBy"] == "Location")
                        Session["MyServerLocations"] = str2;

                    if (Session["ViewBy"] == "Category")
                        Session["MyCategory"] = str2;
                    str3 = str2.Replace("'", "");
                }
                else
                {
                    Session["FilterByValue"] = "null";
                }
                VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("FilterBy", Session["FilterByValue"].ToString(), Convert.ToInt32(Session["UserID"]));
                AssignStatusbox();

                //KeyMetrics(Session["FilterByValue"].ToString());

                Response.Redirect("OverallHealth1.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

            PopupControl2.ShowOnPageLoad = false;
        }
        protected void lnkViewByServerType_Click(object sender, EventArgs e)
        {
            Session["FilterByValue"] = "null";
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ViewBy", "ServerType", Convert.ToInt32(Session["UserID"]));
            Session["ViewBy"] = "ServerType";//lnkViewByServerType.Text;
            //VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("FilterBy", "null", Convert.ToInt32(Session["UserID"]));
            //Session["FilterByValue"] = "null";
            checkBoxList1.DataSource = VSWebBL.StatusBL.StatusTBL.Ins.DistinctLocations();
            checkBoxList1.TextField = "Location";
            checkBoxList1.ValueField = "Location";
            checkBoxList1.DataBind();
            checkBoxList1.UnselectAll();
            FilterLabel.Text = "Supporting Details are grouped by Server Type";
            Session["FilterLabel"] = FilterLabel.Text;
            PopupControl2.Enabled = true;
            AssignStatusbox();
            //KeyMetrics(Session["FilterByValue"].ToString());
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void lnkbtnFilterByLoc_Click(object sender, EventArgs e)
        {
            try
            {
                Session["FilterByValue"] = "null";
                //Session["FilterBy"] = lnkbtnFilterByLoc.CommandName;
                if (Session["FilterBy"] != null || Session["FilterBy"] != " ")
                {
                    //VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("FilterBy", lnkbtnFilterByLoc.CommandName, Convert.ToInt32(Session["UserID"]));
                    ASPxCheckBoxList checkbxlist = (ASPxCheckBoxList)PopupControl2.FindControl("checkBoxList1");
                    //  checkbxlist.DataSource = VSWebBL.SecurityBL.LocationsBL.Ins.GetAllData();
                    //checkbxlist.TextField = "Location";
                    // checkbxlist.Value = "ID";
                    checkbxlist.DataSource = VSWebBL.StatusBL.StatusTBL.Ins.DistinctLocations();
                    checkbxlist.TextField = "Location";
                    checkbxlist.ValueField = "Location";
                    checkbxlist.DataBind();
                    Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
                }
                PopupControl2.ShowOnPageLoad = true;
                ASPxPopupControl1.ShowOnPageLoad = false;
                //PopupControl2.Enabled = true;
                //ASPxPopupControl1.Enabled = false;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }


        }

        protected void lnkMyServers1_Click(object sender, EventArgs e)
        {
            // Session["FilterServerValue"] = lnkMyServers1.Text;
            string s = "";
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ViewBy", "Location", Convert.ToInt32(Session["UserID"]));
            Session["ViewBy"] = "Location";
            //Session["FilterByValue"] = Session["MyServerLocations"];
            if (Session["MyServerLocations"] != null && Session["MyServerLocations"] != "")
            {
                // VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("FilterBy", Session["MyServerLocations"].ToString(), Convert.ToInt32(Session["UserID"]));
                s = Session["MyServerLocations"].ToString().Replace("'", "");
                DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                foreach (DataRow dr in UserPreferences.Rows)
                {
                    if (dr[1].ToString() == "ViewBy")
                    {
                        Session["ViewBy"] = dr[2].ToString();
                    }
                    if (dr[1].ToString() == "FilterBy")
                    {
                        Session["FilterByValue"] = dr[2].ToString();
                    }
                }

            }
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
            FilterLabel.Text = "Supporting Details are grouped by Location " + ", filtered by Server Type in " + s;
            Session["FilterLabel"] = FilterLabel.Text;
            AssignStatusbox();
            //KeyMetrics(Session["FilterByValue"].ToString());
        }

        protected void lnkAllServers1_Click(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ViewBy", "Location", Convert.ToInt32(Session["UserID"]));
            //VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("FilterBy", "null", Convert.ToInt32(Session["UserID"]));
            Session["ViewBy"] = "Location";
            Session["FilterByValue"] = "null";
            checkBoxList1.UnselectAll();
            //FilterLabel.Text = lnkbtnViewByLoc.Text;
            FilterLabel.Text = "Supporting Details are grouped by Location";

            Session["FilterLabel"] = FilterLabel.Text;
            AssignStatusbox();
            //KeyMetrics(Session["FilterByValue"].ToString());
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void lnkMyServers2_Click(object sender, EventArgs e)
        {
            // Session["FilterServerValue"] = lnkMyServers2.Text;
            string s = "";
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ViewBy", "ServerType", Convert.ToInt32(Session["UserID"]));

            Session["ViewBy"] = "ServerType";
            //Session["FilterByValue"] = Session["MyServerTypes"];
            if (Session["MyServerTypes"] != null && Session["MyServerTypes"] != "")
            {
                //VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("FilterBy", Session["MyServerTypes"].ToString(), Convert.ToInt32(Session["UserID"]));
                s = Session["MyServerTypes"].ToString().Replace("'", "");
                DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                foreach (DataRow dr in UserPreferences.Rows)
                {
                    if (dr[1].ToString() == "ViewBy")
                    {
                        Session["ViewBy"] = dr[2].ToString();
                    }
                    if (dr[1].ToString() == "FilterBy")
                    {
                        Session["FilterByValue"] = dr[2].ToString();
                    }
                }
            }
            FilterLabel.Text = "Supporting Details are grouped by Server Type" + ", filtered by Location in " + s;
            Session["FilterLabel"] = FilterLabel.Text;
            AssignStatusbox();
            //KeyMetrics(Session["FilterByValue"].ToString());
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));

        }

        protected void lnkAllServers2_Click(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ViewBy", "ServerType", Convert.ToInt32(Session["UserID"]));
            //VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("FilterBy", "null", Convert.ToInt32(Session["UserID"]));

            Session["ViewBy"] = "ServerType";
            Session["FilterByValue"] = "null";
            checkBoxList1.UnselectAll();
            FilterLabel.Text = "Supporting Details are grouped by Server Type";
            Session["FilterLabel"] = FilterLabel.Text;
            AssignStatusbox();
            //KeyMetrics(Session["FilterByValue"].ToString());
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));

        }

        protected void lnkbtnViewByLoc_Click(object sender, EventArgs e)
        {
            Session["FilterByValue"] = "null";
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ViewBy", "Location", Convert.ToInt32(Session["UserID"]));
            //VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("FilterBy", "null", Convert.ToInt32(Session["UserID"]));

            Session["ViewBy"] = "Location";//lnkbtnViewByLoc.Text;
            //Session["FilterByValue"] = "null";
            checkBoxList1.DataSource = VSWebBL.StatusBL.StatusTBL.Ins.DistinctServerTypes();
            checkBoxList1.TextField = "Type";
            checkBoxList1.ValueField = "Type";

            checkBoxList1.DataBind();
            checkBoxList1.UnselectAll();
            FilterLabel.Text = "Supporting Details are grouped by Location";
            Session["FilterLabel"] = FilterLabel.Text;

            PopupControl2.Enabled = true;
            AssignStatusbox();
            //KeyMetrics(Session["FilterByValue"].ToString());
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void lnkbtnFilterByType_Click(object sender, EventArgs e)
        {
            try
            {
                //   Session["FilterBy"] = lnkbtnFilterByType.CommandName;
                if (Session["FilterBy"] != null || Session["FilterBy"] != "")
                {
                    //     VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("FilterBy", lnkbtnFilterByType.CommandName, Convert.ToInt32(Session["UserID"]));

                    ASPxCheckBoxList checkbxlist = (ASPxCheckBoxList)PopupControl2.FindControl("checkBoxList1");
                    // checkbxlist.DataSource = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetAllData();
                    //checkbxlist.TextField = "ServerType";
                    //checkbxlist.Value = "ID";
                    checkbxlist.DataSource = VSWebBL.StatusBL.StatusTBL.Ins.DistinctServerTypes();
                    checkbxlist.TextField = "Type";
                    checkbxlist.ValueField = "Type";

                    checkbxlist.DataBind();
                    Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
                }
                //PopupControl2.Enabled = true;
                //ASPxPopupControl1.Enabled = false;
                PopupControl2.ShowOnPageLoad = true;
                ASPxPopupControl1.ShowOnPageLoad = false;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

        }

        /// <summary>
        /// view by Category 
        /// </summary>
        /// <param name="ServerLoc"></param>

        protected void CategoryLinkButton_Click(object sender, EventArgs e)
        {
            Session["FilterByValue"] = "null";
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ViewBy", "Category", Convert.ToInt32(Session["UserID"]));
            //VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("FilterBy", "null", Convert.ToInt32(Session["UserID"]));

            Session["ViewBy"] = "Category";//lnkViewByServerType.Text;
            //Session["FilterByValue"] = "null";
            checkBoxList1.DataSource = VSWebBL.StatusBL.StatusTBL.Ins.DistinctLocations();
            checkBoxList1.TextField = "Location";
            checkBoxList1.ValueField = "Location";
            checkBoxList1.DataBind();
            checkBoxList1.UnselectAll();
            FilterLabel.Text = "Supporting Details are grouped by Category";
            Session["FilterLabel"] = FilterLabel.Text;
            PopupControl2.Enabled = true;
            AssignStatusbox();
            //KeyMetrics(Session["FilterByValue"].ToString());
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void Linkfilterbyloc_Click(object sender, EventArgs e)
        {
            try
            {
                //   Session["FilterBy"] = Linkfilterbyloc.CommandName;
                if (Session["FilterBy"] != null || Session["FilterBy"] != " ")
                {
                    //     VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("FilterBy", Linkfilterbyloc.CommandName, Convert.ToInt32(Session["UserID"]));

                    ASPxCheckBoxList checkbxlist = (ASPxCheckBoxList)PopupControl2.FindControl("checkBoxList1");
                    //  checkbxlist.DataSource = VSWebBL.SecurityBL.LocationsBL.Ins.GetAllData();
                    //checkbxlist.TextField = "Location";
                    // checkbxlist.Value = "ID";
                    checkbxlist.DataSource = VSWebBL.StatusBL.StatusTBL.Ins.DistinctLocations();
                    checkbxlist.TextField = "Location";
                    checkbxlist.ValueField = "Location";
                    checkbxlist.DataBind();
                }
                PopupControl2.ShowOnPageLoad = true;
                ASPxPopupControl1.ShowOnPageLoad = false;
                //PopupControl2.Enabled = true;
                //ASPxPopupControl1.Enabled = false;
                Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

        }
        protected void Linkmyservers_Click(object sender, EventArgs e)
        {
            string s = "";
            //VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ViewBy", "Category", Convert.ToInt32(Session["UserID"]));

            Session["ViewBy"] = "Category";
            //Session["FilterByValue"] = Session["MyCategory"] ;
            if (Session["MyCategory"] != null && Session["MyCategory"] != "")
            {
                //VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("OverallHealth1|FilterBy", Session["MyCategory"].ToString(), Convert.ToInt32(Session["UserID"]));

                s = Session["MyCategory"].ToString().Replace("'", "");
                DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                foreach (DataRow dr in UserPreferences.Rows)
                {
                    if (dr[1].ToString() == "ViewBy")
                    {
                        Session["ViewBy"] = dr[2].ToString();
                    }
                    if (dr[1].ToString() == "FilterBy")
                    {
                        Session["FilterByValue"] = dr[2].ToString();
                    }
                }
            }
            FilterLabel.Text = "Supporting Details are grouped by Server Type" + ", filtered by Location in " + s;
            Session["FilterLabel"] = FilterLabel.Text;
            AssignStatusbox();
            //KeyMetrics(Session["FilterByValue"].ToString());
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void LinkAllServers_Click(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ViewBy", "Category", Convert.ToInt32(Session["UserID"]));
            //VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("FilterBy", "null", Convert.ToInt32(Session["UserID"]));

            Session["ViewBy"] = "Category";
            Session["FilterByValue"] = "null";
            checkBoxList1.UnselectAll();
            FilterLabel.Text = "Supporting Details are grouped by Category";
            Session["FilterLabel"] = FilterLabel.Text;
            AssignStatusbox();
            //KeyMetrics(Session["FilterByValue"].ToString());
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }


        //public void KeyMetrics(string ServerLoc)
        //{

        //    DataTable dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetKeyMetrics(ServerLoc);

        //    PendingLabel.Text = "Pending Mail: " + dt.Rows[0]["PendingMail"].ToString();
        //    LblDead.Text = "Dead Mail: " + dt.Rows[0]["DeadMail"].ToString();
        //    HeldLabel.Text = "Held Mail: " + dt.Rows[0]["HeldMail"].ToString();

        //    UsersLabel.Text = dt.Rows[0]["UserCount"].ToString();

        //    RespLabel.Text = dt.Rows[0]["Resp"].ToString() + " ms";
        //    DwnTimeLabel.Text = dt.Rows[0]["DownMinutes"].ToString() + " minutes";
        //    //if (Session["ServiceStatus"] != "" && Session["ServiceStatus"] != null)
        //    //{
        //    //    StatusLabel.Text = Session["ServiceStatus"].ToString();
        //    //}


        //}

        protected void ASPxDataView1_DataBinding(object sender, EventArgs e)
        {
            // UserControl SBOX = (UserControl)ASPxDataView1.FindControl("StatusBox2");
            // ContentPlaceHolder con = (ContentPlaceHolder)Parent.Parent.FindControl("Content2");
            //UpdatePanel up = (UpdatePanel)con.FindControl("UpdatePanel2");

            // DevExpress.Web.Data.ASPxDataView.DataViewItem item=(DevExpress.Web.DataViewItem)up.FindControl("ASPxDataView1");
            //    // DevExpress.Web.DataViewItem  item = sender as  DevExpress.Web.DataViewItem; 
            //StatusBox SBOX = (StatusBox)ASPxDataView1.FindItemControl("StatusBox2",);
            // if (SBOX.Title != "")
            // {
            //     if (SBOX.Title.Length > 28)
            //     {
            //         SBOX.TitleCssClass = "title1";
            //     }
            //     else
            //     {
            //         SBOX.TitleCssClass = "title";
            //     }
            // }

        }
        protected void CStatus_DataBinding(object sender, EventArgs e)
        {
            //10/23/2014 NS modified for VSPLUS-1053
            ASPxDataView dataview = new ASPxDataView();
            dataview = ASPxDataView2;
            Label cStatus = new Label();
            cStatus = (Label)sender;
            if (dataview.Items.Count > 0)
            {
                for (int j = 0; j < dataview.Items.Count; j++)
                {
                    HtmlGenericControl divControl = new HtmlGenericControl();
                    divControl = (HtmlGenericControl)dataview.FindItemControl("statusdiv", dataview.Items[j]);
                    cStatus = (Label)dataview.FindItemControl("CStatus", dataview.Items[j]);
                    if (cStatus.Text == "OK")
                    {
                        divControl.Attributes["class"] = "OKUL";
                    }
                    else if (cStatus.Text == "Issue")
                    {
                        divControl.Attributes["class"] = "IssueUL";
                    }
                    else if (cStatus.Text == "Maintenance")
                    {
                        divControl.Attributes["class"] = "MaintenanceUL";
                    }
                    else if (cStatus.Text == "Not Scanned")
                    {
                        divControl.Attributes["class"] = "";
                        cStatus.Font.Bold = true;
                        cStatus.ForeColor = System.Drawing.Color.Black;
                    }
                    else //if (cStatus.Text == "Fail" || cStatus.Text == "Not Responding")
                    {
                        divControl.Attributes["class"] = "NotRespondingUL";
                    }
                }
            }
        }

        protected void NDStatus_DataBinding(object sender, EventArgs e)
        {
            //10/23/2014 NS modified for VSPLUS-1053
            ASPxDataView dataview = new ASPxDataView();
            dataview = ASPxDataView3;
            Label cStatus = new Label();
            cStatus = (Label)sender;
            if (dataview.Items.Count > 0)
            {
                for (int j = 0; j < dataview.Items.Count; j++)
                {
                    HtmlGenericControl divControl = new HtmlGenericControl();
                    divControl = (HtmlGenericControl)dataview.FindItemControl("statusdiv", dataview.Items[j]);
                    cStatus = (Label)dataview.FindItemControl("CStatus", dataview.Items[j]);
                    if (cStatus.Text == "OK")
                    {
                        divControl.Attributes["class"] = "OKUL";
                    }
                    else if (cStatus.Text == "Issue")
                    {
                        divControl.Attributes["class"] = "IssueUL";
                    }
                    else if (cStatus.Text == "Maintenance")
                    {
                        divControl.Attributes["class"] = "MaintenanceUL";
                    }
                    else //if (cStatus.Text == "Fail" || cStatus.Text == "Not Responding")
                    {
                        divControl.Attributes["class"] = "NotRespondingUL";
                    }
                }
            }
        }


        protected void ASPxDataView2_DataBinding(object sender, EventArgs e)
        {
            //10/23/2014 NS modified for VSPLUS-1053
            ASPxDataView dataview = new ASPxDataView();
            dataview = (ASPxDataView)sender;
            if (dataview.Items.Count > 0)
            {
                for (int j = 0; j < dataview.Items.Count; j++)
                {
                    HtmlGenericControl divControl = new HtmlGenericControl();
                    divControl = (HtmlGenericControl)dataview.FindItemControl("statusdiv", dataview.Items[j]);
                    Label cStatus = new Label();
                    cStatus = (Label)dataview.FindItemControl("CStatus", dataview.Items[j]);
                    if (cStatus.Text == "OK")
                    {
                        divControl.Attributes["class"] = "OKUL";
                    }
                    else if (cStatus.Text == "Issue")
                    {
                        divControl.Attributes["class"] = "IssueUL";
                    }
                    else if (cStatus.Text == "Maintenance")
                    {
                        divControl.Attributes["class"] = "MaintenanceUL";
                    }
                    else //if (cStatus.Text == "Fail" || cStatus.Text == "Not Responding")
                    {
                        divControl.Attributes["class"] = "NotRespondingUL";
                    }
                }
            }
        }
        public void SetLinkVisibility()
        {
            bool isadmin = false;
            if (Session["Isconfigurator"] != null && Session["Isconfigurator"] != "")
            {
                if (Session["Isconfigurator"].ToString() == "True")
                {
                    isadmin = true;
                }
            }
            if (!isadmin)
            {
                a5.HRef = "#";
            }
        }

        public string IsUserLogged()
        {
            if (Session["UserFullName"] != null)
            {
                return Session["UserFullName"].ToString();
            }
            else
            {
                return "Anonymous";
            }

        }

        public string GetLink(object status, object typeloc)
        {
            string Status = status.ToString();
            string TypeLoc = typeloc.ToString();

            if (Status == "ALL")
                return "~/Dashboard/DeviceTypeList.aspx?typeloc=" + TypeLoc;
            return "~/Dashboard/DeviceTypeList.aspx?status=" + status + "&typeloc=" + TypeLoc;
        }

        protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "Myaccountdetails")
            {
                Response.Redirect("~/Security/MyAccount.aspx?dboard=" + true);
                Context.ApplicationInstance.CompleteRequest();
            }
        }






      


    }
}