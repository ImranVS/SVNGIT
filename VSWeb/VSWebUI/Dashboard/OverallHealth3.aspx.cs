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
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
using VSWebDO;

namespace VSWebUI.Dashboard
{
	public partial class OverallHealth3 : System.Web.UI.Page
	{
        public int maxmin = 0;
        public int minmin = 0;

        //To sync with Master page refresh below 2 events are used Page_PreInit, Master_ButtonClick
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Mukund 05Nov13, Create an event handler for the master page's contentCallEvent event
            this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
        }
        private void Master_ButtonClick(object sender, EventArgs e)
        {
            //Mukund 05Nov13, This Method will be Called from Timer Click from Master page

            if(!IsPostBack)
            {
            
            AssignStatusbox();
            }
        }

		DataTable dt = new DataTable();
		// string viewby = "ServerType";           
		string str3;
		string loginname;
		protected void Page_Load(object sender, EventArgs e)
		{
            GetStatus();
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
                if (Session["FilterLabel"] == null || Session["FilterLabel"].ToString() == "")
                {
                    //FilterLabel.Text = "Supporting Details are grouped by Server Type";  //lnkViewByServerType.Text;
                }
                else
                {
                    //FilterLabel.Text = Session["FilterLabel"].ToString();
                }

                //PopupControl2.Enabled = true;
                //2/11/2016 NS commented out
                //SetLinkVisibility();
                //KeyMetrics(Session["FilterByValue"].ToString());
            }
            FillMobileDevicesGrid();
			
            //2/4/2016 NS added for VSPLUS-2570
            CheckSelectedFeatures();
            //25Feb2014 MD moved the Mail box out of the !IsPostback block
			if (Session["ViewBy"] == null) Session["ViewBy"] = "ServerType";
			if (Session["FilterByValue"] == null) Session["FilterByValue"] = "null";
			//KeyMetrics(Session["FilterByValue"].ToString());
			//12/12/2013 NS moved the status box calculations out of the !IsPostback block
            if (!IsPostBack)
            {
                AssignStatusbox();
                //2/4/2016 NS added for VSPLUS-2570
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "MailDeliveryStatus|EXJournalGridView")
                        {
                            EXJournalGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        else if (dr[1].ToString() == "ExMailDeliveryStatus|ExchangeMailGridView")
                        {
                            ExchangeMailGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            //if (Session["Refreshtime"] != "" && Session["Refreshtime"] != null)
            //    timerupdate.Interval = Convert.ToInt32(Session["Refreshtime"]) * 1000;
            //else 
            //    timerupdate.Interval = Convert.ToInt32(Session["Refreshtime"]) * 1000;

			//else
			//{
			//    timerupdate.Interval = 1000;
			//}
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
					else
					{
						//keymetrictable.Visible = false;
						//servernamelbldisp.Visible = false;
                        //11/4/2015 NS added for VSPLUS-1271
                        DominoDock.ShowOnPageLoad = false;
						DominoMetrics.Visible = false;
						
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
		protected void GetCloudData()
		{
			ASPxDataView2.Visible = false;
			//divCloud.Visible = false;
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
					
					if (dt.Rows.Count > 0)
					{
						// Image1.ImageUrl = dt.Rows[0]["Image"].ToString();
						ASPxDataView2.DataSource = dt;
						ASPxDataView2.DataBind();
						ASPxDataView2.Visible = true;
                        //11/4/2015 NS modified for VSPLUS-1271
						//divCloud.Visible = true;
						CloudApp.Visible = true;
                        CloudDock.ShowOnPageLoad = true;
					

					}
				}
				else
				{
					ASPxDataView2.Visible = false;
                    //11/4/2015 NS modified for VSPLUS-1271
					//divCloud.Visible = false;
					CloudDock.ShowOnPageLoad = false;
					CloudApp.Visible = false;

				}
			}
		}

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

					UsersLabel.Text = "User Sessions: " + dt.Rows[0]["UserCount"].ToString();

					RespLabel.Text = "Avg Response Time: " + dt.Rows[0]["Resp"].ToString() + " ms";
					DwnTimeLabel.Text = "Down Time: " + dt.Rows[0]["DownMinutes"].ToString() + " minutes";
				//	keymetrictable.Visible = true;
					//servernamelbldisp.Visible = true;
					//if (Session["ServiceStatus"] != "" && Session["ServiceStatus"] != null)
					//{
					//    StatusLabel.Text = Session["ServiceStatus"].ToString();
					//}
                    //11/4/2015 NS added for VSPLUS-1271

					DominoMetrics.Visible = true;
                    DominoDock.ShowOnPageLoad = true;
				}
				else
				{
			//		keymetrictable.Visible = false;
					//servernamelbldisp.Visible = false;
                    //11/4/2015 NS added for VSPLUS-1271
					DominoMetrics.Visible = false;	
                    DominoDock.ShowOnPageLoad = false;
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

                    //1/26/2016 NS modified for VSPLUS-2531
                    UsersLabel.Text = "User Sessions: " + dt.Rows[0]["UserCount"].ToString();

                    RespLabel.Text = "Avg Response Time: " + dt.Rows[0]["Resp"].ToString() + " ms";
                    DwnTimeLabel.Text = "Down Time: " + dt.Rows[0]["DownMinutes"].ToString() + " minutes";
					DominoMetrics.Visible = true;

					//servernamelbldisp.Visible = true;
					//if (Session["ServiceStatus"] != "" && Session["ServiceStatus"] != null)
					//{
					//    StatusLabel.Text = Session["ServiceStatus"].ToString();
					//}
                    //11/4/2015 NS added for VSPLUS-1271
                    DominoDock.ShowOnPageLoad = true;
					
				}
				else
				{
					DominoMetrics.Visible = false;
					//servernamelbldisp.Visible = false;
                    //11/4/2015 NS added for VSPLUS-1271
                    DominoDock.ShowOnPageLoad = false;
				}

			}
		}
		protected void GetNetworkInfraData()
		{
			ASPxDataView3.Visible = false;
           //divnetwrkInf.Visible = false;
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
					
                    if (dt.Rows.Count > 0)
                    {
                        // Image1.ImageUrl = dt.Rows[0]["Image"].ToString();
                        ASPxDataView3.DataSource = dt;
                        ASPxDataView3.DataBind();

                        ASPxDataView3.Visible = true;
                        //11/4/2015 NS modified for VSPLUS-1271
                        //divnetwrkInf.Visible = true;
                        NetworkDock.ShowOnPageLoad = true;
						NetworkInfra.Visible = true;

                    }
                    else
                    {
                        ASPxDataView3.Visible = false;
                        NetworkDock.ShowOnPageLoad = false;
						NetworkInfra.Visible = false;
					}
				}
				else
				{
					ASPxDataView3.Visible = false;
                    //11/4/2015 NS modified for VSPLUS-1271
                    //divnetwrkInf.Visible = false;
                    NetworkDock.ShowOnPageLoad = false;
					NetworkInfra.Visible = false;


				}
			}
		}
		//protected void GetSNMPData()
		//{
		//    ASPxDataView3.Visible = false;
		//    divCloud.Visible = false;
		//    divOnPrem.Visible = false;
		//    divnetwrkInf.Visible = false;
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
		//            divCloud.Visible = true;
		//            divOnPrem.Visible = true;
		//            divnetwrkInf.Visible = true;
		//        }
		//        else
		//        {
		//            ASPxDataView3.Visible = false;
		//            divCloud.Visible = false;
		//            divnetwrkInf.Visible = false;
		//        }
		//    }

		//}

		protected void AssignStatusbox()
		{
			//12/12/2013 NS added
			string tempStatus = "";
			int ind = -1;
			int diffInMin = 0;
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
            if (Session["FilterByValue"] == " " || Session["FilterByValue"] == "" || Session["FilterByValue"] == null)
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
				//FilterLabel.Text = "Supporting Details are grouped by Server Type" + ", filtered by Location in " + str3;
				//Session["FilterLabel"] = FilterLabel.Text;
			}
			else if (Session["FilterByValue"].ToString() == "null" && Session["ViewBy"].ToString() == "ServerType")
			{
				//FilterLabel.Text = "Supporting Details are grouped by Server Type.";
				//Session["FilterLabel"] = FilterLabel.Text;
			}
			if (Session["FilterByValue"].ToString() != "null" && Session["ViewBy"].ToString() == "Location" && str3 != null && str3 != "")
			{
				//FilterLabel.Text = "Supporting Details are grouped by Location" + ", filtered by Server Type in " + str3;
				//Session["FilterLabel"] = FilterLabel.Text;
			}
			else if (Session["FilterByValue"].ToString() == "null" && Session["ViewBy"].ToString() == "Location")
			{
				//FilterLabel.Text = "Supporting Details are grouped by Location.";
				//Session["FilterLabel"] = FilterLabel.Text;
			}
			if (Session["FilterByValue"].ToString() != "null" && Session["ViewBy"].ToString() == "Category" && str3 != null && str3 != "")
			{
				//FilterLabel.Text = "Supporting Details are grouped by Category" + ", filtered by Location in " + str3;
				//Session["FilterLabel"] = FilterLabel.Text;
			}
			else if (Session["FilterByValue"].ToString() == "null" && Session["ViewBy"].ToString() == "Category")
			{
				//FilterLabel.Text = "Supporting Details are grouped by Category.";
				//Session["FilterLabel"] = FilterLabel.Text;
			}
			else if (Session["FilterByValue"].ToString() != "null" && (Session["ViewBy"].ToString() != "" || Session["ViewBy"].ToString() != "null"))
			{
				string strFilter = (Session["ViewBy"].ToString() == "ServerType" || Session["ViewBy"].ToString() == "Category" ? "Location" : "Server Type");
				//FilterLabel.Text = "Supporting Details are grouped by " + Session["ViewBy"].ToString() + ", filtered by " + strFilter + " in " + Session["FilterByValue"].ToString().Replace("'", "");
				//Session["FilterLabel"] = FilterLabel.Text;

			}
			if (Session["UserLogin"] != null && Session["UserLogin"] != "")
			{
				loginname = Session["UserLogin"].ToString();

				DataTable dtonpremises = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.Getonpremiseswithuser(loginname);
				Session["checkeddata"] = dtonpremises;

				bool value = false;
				value = Convert.ToBoolean(dtonpremises.Rows[0]["OnPremisesApplications"]);

				bool CloudApplications = false;
				CloudApplications = Convert.ToBoolean(dtonpremises.Rows[0]["CloudApplications"]);
				bool OnPremisesApplications = Convert.ToBoolean(dtonpremises.Rows[0]["OnPremisesApplications"]);
				bool NetworkInfrastucture = Convert.ToBoolean(dtonpremises.Rows[0]["NetworkInfrastucture"]);
				bool DominoServerMetrics = Convert.ToBoolean(dtonpremises.Rows[0]["DominoServerMetrics"]);

				
				try
				{
					if ((CloudApplications == false && OnPremisesApplications == false && NetworkInfrastucture == false && DominoServerMetrics == false) || value == true)
					{
						//11/4/2015 NS modified for VSPLUS-1271
						//divOnPrem.Visible = true;
                        PremisesDock.ShowOnPageLoad = true;
						OnPremises.Visible = true;
                 		
					}
					else
					{
                        //11/4/2015 NS modified for VSPLUS-1271
						//divOnPrem.Visible = false;
                        PremisesDock.ShowOnPageLoad = false;
						OnPremises.Visible = false;

					}
				}
				catch
				{


				}

			}
		}

		protected void AssignStatusvisiblebox()
		{
			//12/12/2013 NS added
			string tempStatus = "";
			int ind = -1;
			int diffInMin = 0;
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
				//FilterLabel.Text = "Supporting Details are grouped by Server Type" + ", filtered by Location in " + str3;
				//Session["FilterLabel"] = FilterLabel.Text;
			}
			else if (Session["FilterByValue"].ToString() == "null" && Session["ViewBy"].ToString() == "ServerType")
			{
				//FilterLabel.Text = "Supporting Details are grouped by Server Type";
				//Session["FilterLabel"] = FilterLabel.Text;
			}
			if (Session["FilterByValue"].ToString() != "null" && Session["ViewBy"].ToString() == "Location" && str3 != null && str3 != "")
			{
				//FilterLabel.Text = "Supporting Details are grouped by Location" + ", filtered by Server Type in " + str3;
				//Session["FilterLabel"] = FilterLabel.Text;
			}
			else if (Session["FilterByValue"].ToString() == "null" && Session["ViewBy"].ToString() == "Location")
			{
				//FilterLabel.Text = "Supporting Details are grouped by Location";
				//Session["FilterLabel"] = FilterLabel.Text;
			}
			if (Session["FilterByValue"].ToString() != "null" && Session["ViewBy"].ToString() == "Category" && str3 != null && str3 != "")
			{
				//FilterLabel.Text = "Supporting Details are grouped by Category" + ", filtered by Location in " + str3;
				//Session["FilterLabel"] = FilterLabel.Text;
			}
			else if (Session["FilterByValue"].ToString() == "null" && Session["ViewBy"].ToString() == "Category")
			{
				//FilterLabel.Text = "Supporting Details are grouped by Category";
				//Session["FilterLabel"] = FilterLabel.Text;
			}
			else if (Session["FilterByValue"].ToString() != "null" && (Session["ViewBy"].ToString() != "" || Session["ViewBy"].ToString() != "null"))
			{
				string strFilter = (Session["ViewBy"].ToString() == "ServerType" || Session["ViewBy"].ToString() == "Category" ? "Location" : "Server Type");
				//FilterLabel.Text = "Supporting Details are grouped by " + Session["ViewBy"].ToString() + ", filtered by " + strFilter + " in " + Session["FilterByValue"].ToString().Replace("'", "");
				//Session["FilterLabel"] = FilterLabel.Text;

			}

			//11/4/2015 NS modified for VSPLUS-1271
			//divOnPrem.Visible = true;
            PremisesDock.ShowOnPageLoad = true;
			OnPremises.Visible = true;
		}
		protected void visiblealldata()
		{
			dt = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.GetCloudDatavisible();
			
			if (dt.Rows.Count > 0)
			{
				// Image1.ImageUrl = dt.Rows[0]["Image"].ToString();
				ASPxDataView2.DataSource = dt;
				ASPxDataView2.DataBind();
				ASPxDataView2.Visible = true;
				CloudDock.ShowOnPageLoad = true;

				CloudApp.Visible = true;

			}

			else
			{
				ASPxDataView2.Visible = false;
				CloudDock.ShowOnPageLoad = false;
				CloudApp.Visible = false;
			}


			dt = VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetNetworkdevicevisibleData();
			
			if (dt.Rows.Count > 0)
			{
				// Image1.ImageUrl = dt.Rows[0]["Image"].ToString();
				ASPxDataView3.DataSource = dt;
				ASPxDataView3.DataBind();

				ASPxDataView3.Visible = true;
				NetworkDock.ShowOnPageLoad = true;

				NetworkInfra.Visible = true;
			}

			else
			{
				ASPxDataView3.Visible = false;
				NetworkDock.ShowOnPageLoad = false;
				NetworkInfra.Visible = false;
			}
			KeyMetricsvisible(Session["FilterByValue"].ToString());
			AssignStatusvisiblebox();
		}
		protected void visiblealldatadasbord()
		{
			dt = VSWebBL.ConfiguratorBL.CloudApplicationsServerBL.Ins.GetCloudDatavisiblefordashboard();

			if (dt.Rows.Count > 0)
			{
				// Image1.ImageUrl = dt.Rows[0]["Image"].ToString();
				ASPxDataView2.DataSource = dt;
				ASPxDataView2.DataBind();
				ASPxDataView2.Visible = true;
				CloudApp.Visible = true;

			}
			else
			{
				ASPxDataView2.Visible = false;
				CloudApp.Visible = false;
			}
			dt = VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetNetworkDatavisiblefordashboard();

			if (dt.Rows.Count > 0)
			{
				// Image1.ImageUrl = dt.Rows[0]["Image"].ToString();
				ASPxDataView3.DataSource = dt;
				ASPxDataView3.DataBind();

				ASPxDataView3.Visible = true;

				NetworkInfra.Visible = true;
			}
			else
			{
				ASPxDataView3.Visible = false;

				NetworkInfra.Visible = false;
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

		/// <summary>
		/// view by Category 
		/// </summary>
		/// <param name="ServerLoc"></param>


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

        protected void UpdatePanel_Unload(object sender, EventArgs e)
        {
            System.Reflection.MethodInfo methodInfo = typeof(ScriptManager).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Where(i => i.Name.Equals("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel")).First();
            methodInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { sender as UpdatePanel });
        }

        protected void SyncWebChart_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            WebChartControl chartControl = (WebChartControl)sender;

            DevExpress.Web.GridViewDataItemTemplateContainer gridc = (DevExpress.Web.GridViewDataItemTemplateContainer)chartControl.Parent;
            string deviceid = DataBinder.Eval(gridc.DataItem, "DeviceID").ToString();

            dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.GetKeyUserDevices(deviceid);
            chartControl.DataSource = dt;
            chartControl.Series["MinSinceSync"].DataSource = dt;
            chartControl.Series["MinSinceSync"].ArgumentDataMember = dt.Columns["DeviceID"].ToString();
            chartControl.Series["MinSinceSync"].ValueDataMembers.AddRange(dt.Columns["LastSyncMin"].ToString());
            chartControl.Series["MinSinceSync"].Visible = true;
            XYDiagram seriesXY = (XYDiagram)chartControl.Diagram;
            seriesXY.AxisY.Range.MinValue = minmin;
            seriesXY.AxisY.Range.MaxValue = maxmin;
            chartControl.DataBind();
        }

        protected void KeyMobileDevicesGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            string status = e.GetValue("Status").ToString();
            if (e.DataColumn.FieldName == "Status")
            {
                if (status == "Overdue")
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        private void FillMobileDevicesGrid()
        {
            try
            {
                DataTable dt1 = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.GetKeyUserDevices("");
                if (dt1.Rows.Count > 0)
                {
                    maxmin = Convert.ToInt32(dt1.Rows[0]["LastSyncMin"].ToString());
                    minmin = Convert.ToInt32(dt1.Rows[dt1.Rows.Count - 1]["LastSyncMin"].ToString());
                    KeyUserDevicesDock.ShowOnPageLoad = true;
                }
                else
                {
                    KeyUserDevicesDock.ShowOnPageLoad = false;
                }
                KeyMobileDevicesGrid.DataSource = dt1;
                Session["KeyMobileDevicesGrid"] = dt1;
                KeyMobileDevicesGrid.DataBind();
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillMobileDevicesGridfromSession()
        {
            try
            {
                DataTable dt = new DataTable();
                if ((Session["KeyMobileDevicesGrid"] != null))
                {
                    dt = (DataTable)Session["KeyMobileDevicesGrid"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                    if (dt.Rows.Count > 0)
                    {
                        maxmin = Convert.ToInt32(dt.Rows[0]["LastSyncMin"].ToString());
                        minmin = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["LastSyncMin"].ToString());
                        KeyMobileDevicesGrid.DataSource = dt;
                        KeyMobileDevicesGrid.DataBind();
                    }
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

        protected void ASPxDockManager1_ClientLayout(object sender, ASPxClientLayoutArgs e)
        {
            if (e.LayoutMode == ClientLayoutMode.Saving)
                Session["DockingLayout"] = e.LayoutData;
            if (e.LayoutMode == ClientLayoutMode.Loading)
                e.LayoutData = Session["DockingLayout"] as string;
			//2/11/2016 Durga Added for VSPLUS 2595
			string s = e.LayoutData.ToString();

			s = s.Replace("'", "");
			
			string[] split = s.Split(new Char[] { ',', ']' });
			string[] getting = split;
			string[] spliting = s.Split(new char[] { '[', ',', '"' });
			string[] getdock = spliting;

			if (s != "")
			{
				//2/11/2016 Durga Added for VSPLUS 2595
				Users Usersobj=new Users();
				Usersobj.KeyUserDevicesIndex = Convert.ToInt32(getting[7]);

				Usersobj.premisesindex = Convert.ToInt32(getting[16]);

				Usersobj.StatusIndex = Convert.ToInt32(getting[25]);

				Usersobj.cloudindex = Convert.ToInt32(getting[34]);

				Usersobj.networkindex = Convert.ToInt32(getting[43]);

				Usersobj.TravelerIndex = Convert.ToInt32(getting[52]);


				Usersobj.dockindex = Convert.ToInt32(getting[61]);
			
				Usersobj.KeyUserDevicesZone = spliting[3].Replace("'", "");
				Usersobj.premisesZone = spliting[12].Replace("'", "");
				Usersobj.StatusZone = spliting[21].Replace("'", "");
				Usersobj.cloudZone = spliting[30].Replace("'", "");
				Usersobj.networkZone = spliting[39].Replace("'", "");
				Usersobj.TravelerZone = spliting[48].Replace("'", "");
				Usersobj.DockZone = spliting[57].Replace("'", "");
				
				int id = Convert.ToInt32(Session["UserID"]);
				VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.SaveOveralldock(Usersobj, s, id);
				

			}
			savedock();
        }
		//2/11/2016 Durga Added for VSPLUS 2595
		protected void savedock()
		{
			DataTable dt = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetDockData(Convert.ToInt32(Session["UserID"]));
			if (dt.Rows.Count > 0)
			{
				CloudDock.VisibleIndex = Convert.ToInt32(dt.Rows[0]["cloudindex"].ToString());
				PremisesDock.VisibleIndex = Convert.ToInt32(dt.Rows[0]["premisesindex"].ToString());
				NetworkDock.VisibleIndex = Convert.ToInt32(dt.Rows[0]["networkindex"].ToString());
				DominoDock.VisibleIndex = Convert.ToInt32(dt.Rows[0]["dockindex"].ToString());
				MailStatusDock.VisibleIndex = Convert.ToInt32(dt.Rows[0]["StatusIndex"].ToString());
				KeyUserDevicesDock.VisibleIndex = Convert.ToInt32(dt.Rows[0]["KeyUserDevicesIndex"].ToString());
				TravelerDock.VisibleIndex = Convert.ToInt32(dt.Rows[0]["TravelerIndex"].ToString());
				PremisesDock.OwnerZoneUID = dt.Rows[0]["premisesZone"].ToString();
				NetworkDock.OwnerZoneUID = dt.Rows[0]["networkZone"].ToString();
				DominoDock.OwnerZoneUID = dt.Rows[0]["DockZone"].ToString();
				MailStatusDock.OwnerZoneUID = dt.Rows[0]["StatusZone"].ToString();
				KeyUserDevicesDock.OwnerZoneUID = dt.Rows[0]["KeyUserDevicesZone"].ToString();
				TravelerDock.OwnerZoneUID = dt.Rows[0]["TravelerZone"].ToString();
				//2/11/2016 Durga Added for VSPLUS 2595
				CloudDock.OwnerZoneUID = dt.Rows[0]["cloudZone"].ToString();
			}
			//Page.ClientScript.RegisterStartupScript(this.GetType(),"Script", "MyFunction();", true);
		}
		private void FillTravelerGrid()
		{
			DataTable dt = new DataTable();
			try
			{
				dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGrid();

				//Combines all the reasoning into one long string seperated by new lines
				for (int i = 0; i < dt.Rows.Count - 1; i++)
				{
					if (dt.Rows[i]["Name"].ToString() == dt.Rows[i + 1]["Name"].ToString())
					{
						dt.Rows[i]["Reasons"] += "<br /><br />" + dt.Rows[i + 1]["Reasons"].ToString();
						dt.Rows.Remove(dt.Rows[i + 1]);
						i--;
					}
				}
				Session["GridData"] = dt;
				grid.DataSource = Session["GridData"];

				DataTable dt2 = new DataTable();

				dt2 = (DataTable)Session["GridData"];
				for (int i = 0; i < dt2.Rows.Count; i++)
				{
					if (dt2.Rows[i]["Status"].ToString() == "Yellow" || dt2.Rows[i]["Status"].ToString() == "Red")
					{
						if (dt2.Rows[i]["Reasons"].ToString() == "" || dt2.Rows[i]["Reasons"].ToString() == null)
						{
							dt2.Rows[i]["Reasons"] = "Status detail is available for Traveler 9.0.1.4 and higher";
						}
					}
					else if (dt2.Rows[i]["Status"].ToString() == "Fail")
					{
						dt2.Rows[i]["Reasons"] = dt2.Rows[i]["Details"].ToString();
					}
					else
					{
						if (dt2.Rows[i]["Reasons"].ToString() == "" || dt2.Rows[i]["Reasons"].ToString() == null)
						{
							dt2.Rows[i]["Reasons"] = "No Traveler-specific issues detected";
						}
					}
				}


				grid.DataBind();

			}
			catch (Exception ex)
			{
				Response.Write("Error :" + ex);
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

		}
	

		protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Running")
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
				e.Cell.ForeColor = System.Drawing.Color.Black;
			}
			else if (e.DataColumn.FieldName == "Status")
			{
				
				if (e.CellValue.ToString() == "Green")
				{
					e.Cell.BackColor = System.Drawing.Color.LightGreen;
					e.Cell.ForeColor = System.Drawing.Color.Black;
				}
				else
				{
					if (e.CellValue.ToString() == "Yellow")
					{
						e.Cell.BackColor = System.Drawing.Color.Yellow;
						e.Cell.ForeColor = System.Drawing.Color.Black;
					}
					else
					{
						if (e.CellValue.ToString() == "Red")
						{
							e.Cell.BackColor = System.Drawing.Color.Red;
							e.Cell.ForeColor = System.Drawing.Color.White;
						}
						else if (e.CellValue.ToString() == "")
						{
							e.Cell.BackColor = System.Drawing.Color.Empty;
							e.Cell.ForeColor = System.Drawing.Color.Empty;
						}
                        //6/9/2016 NS added for VSPLUS-2973
                        else if (e.CellValue.ToString().ToUpper() == "FAIL")
                        {
                            e.Cell.BackColor = System.Drawing.Color.Red;
                            e.Cell.ForeColor = System.Drawing.Color.White;
                        }
					}
				}
			}

			if (e.DataColumn.FieldName == "HTTP_Status" && e.CellValue.ToString().Trim() == "OK")
			{
				e.Cell.BackColor = System.Drawing.Color.LightGreen;
				e.Cell.ForeColor = System.Drawing.Color.Black;
			}
			else if (e.DataColumn.FieldName == "HTTP_Status")
			{
				e.Cell.BackColor = System.Drawing.Color.Red;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "HTTP_Status" && e.CellValue.ToString() == "")
			{
				e.Cell.BackColor = System.Drawing.Color.Empty;
				e.Cell.ForeColor = System.Drawing.Color.Empty;
			}

			if (e.DataColumn.FieldName == "TravelerServlet")
			{
				if (e.CellValue.ToString() == "OK")
				{
					e.Cell.BackColor = System.Drawing.Color.LightGreen;
					e.Cell.ForeColor = System.Drawing.Color.Black;
				}
                //5/20/2016 NS modified for VSPLUS-2973
                else if (e.CellValue.ToString() == "Not Responding")
				{
					e.Cell.BackColor = System.Drawing.Color.Red;
					e.Cell.ForeColor = System.Drawing.Color.White;
				}
				else if (e.DataColumn.FieldName == "TravelerServlet" && e.CellValue.ToString() == "")
				{
					e.Cell.BackColor = System.Drawing.Color.Empty;
					e.Cell.ForeColor = System.Drawing.Color.Black;
				}
			}
		}

		protected void hfNameLabel_Load(object sender, EventArgs e)
		{
			Label StatusLabel = (Label)sender;

			if (StatusLabel.Text == "Red")
				StatusLabel.ForeColor = System.Drawing.Color.White;
			else
				StatusLabel.ForeColor = System.Drawing.Color.Black;
		}

        protected void GetStatus()
        {
            double widthval = 0;
            DataRow[] foundRows;
            System.Web.UI.HtmlControls.HtmlGenericControl ahref = new System.Web.UI.HtmlControls.HtmlGenericControl("a");
            System.Web.UI.HtmlControls.HtmlGenericControl dynDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            System.Web.UI.HtmlControls.HtmlGenericControl dynDivAll = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            System.Web.UI.HtmlControls.HtmlGenericControl dynDivHeader = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            try
            {
                DataTable dt = VSWebBL.DashboardBL.DashboardBL.Ins.GetStatusCountByType();
                DataTable dt2 = VSWebBL.DashboardBL.DashboardBL.Ins.GetStatusCountTotalByType();
                if (dt.Rows.Count > 0)
                {
                    string currtype = dt.Rows[0]["Type"].ToString();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (currtype != dt.Rows[i]["Type"].ToString() || i == 0)
                        {
                            currtype = dt.Rows[i]["Type"].ToString();
                            dynDivHeader = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                            dynDivHeader.ID = dt.Rows[i]["Type"].ToString().Trim() + "Div";
                            dynDivHeader.Attributes["class"] = "subheaderN";
                            dynDivHeader.Attributes["runat"] = "server";
                            //2/11/2016 NS modified for VSPLUS-2568
                            if (currtype == "Office365")
                            {
                                dynDivHeader.InnerHtml = "Office 365";
                            }
                            else
                            {
                                dynDivHeader.InnerHtml = currtype;
                            }
                            ahref = new System.Web.UI.HtmlControls.HtmlGenericControl("a");
                            ahref.Attributes["href"] = "DeviceTypeList.aspx?typeloc=" + dt.Rows[i]["Type"].ToString().Trim();
                            ahref.Controls.Add(dynDivHeader);
                            ASPxPanel1.Controls.Add(ahref);
                            dynDivAll = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                            dynDivAll.ID = dt.Rows[i]["Type"].ToString().Trim() + "progressDiv";
                            dynDivAll.Attributes["class"] = "progress";
                            ASPxPanel1.Controls.Add(dynDivAll);
                        }
                        dynDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                        dynDiv.ID = dt.Rows[i]["Type"].ToString().Trim() + "Div" + (i + 1).ToString();
                        ahref = new System.Web.UI.HtmlControls.HtmlGenericControl("a");
                        switch (dt.Rows[i]["OrderNum"].ToString())
                        {
                            case "1":
                                dynDiv.Attributes["class"] = "progress-bar progress-bar-success";
                                ahref.Attributes["href"] = "DeviceTypeList.aspx?status=OK&typeloc=" + dt.Rows[i]["Type"].ToString().Trim();
                                //dynDiv.InnerHtml = "OK";
                                dynDiv.InnerHtml = dt.Rows[i]["StatusCount"].ToString();
                                break;
                            case "2":
                                dynDiv.Attributes["class"] = "progress-bar progress-bar-warning";
                                ahref.Attributes["href"] = "DeviceTypeList.aspx?status=Issue&typeloc=" + dt.Rows[i]["Type"].ToString().Trim();
                                //dynDiv.InnerHtml = "Issue";
                                dynDiv.InnerHtml = dt.Rows[i]["StatusCount"].ToString();
                                break;
                            case "3":
                                dynDiv.Attributes["class"] = "progress-bar progress-bar-danger";
                                ahref.Attributes["href"] = "DeviceTypeList.aspx?status=Not%20Responding&typeloc=" + dt.Rows[i]["Type"].ToString().Trim();
                                //dynDiv.InnerHtml = "Down";
                                dynDiv.InnerHtml = dt.Rows[i]["StatusCount"].ToString();
                                break;
                            case "4":
                                dynDiv.Attributes["class"] = "progress-bar progress-bar-info";
                                ahref.Attributes["href"] = "DeviceTypeList.aspx?status=Maintenance&typeloc=" + dt.Rows[i]["Type"].ToString().Trim();
                                //dynDiv.InnerHtml = "Maintenance";
                                dynDiv.InnerHtml = dt.Rows[i]["StatusCount"].ToString();
                                break;
                        }
                        foundRows = dt2.Select("Type='" + currtype + "'");
                        if (foundRows[0] != null)
                        {
                            widthval = (Math.Round(Convert.ToDouble(dt.Rows[i]["StatusCount"].ToString()) / Convert.ToDouble(foundRows[0]["TotalStatusCount"].ToString()), 2)) * 100;
                        }
                        dynDiv.Style.Add(HtmlTextWriterStyle.Width, widthval.ToString() + "%");
                        ahref.Controls.Add(dynDiv);
                        dynDivAll.Controls.Add(ahref);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> Name = grid.GetSelectedFieldValues("Name");
                System.Collections.Generic.List<object> Status = grid.GetSelectedFieldValues("Status");
                System.Collections.Generic.List<object> HeartBeat = grid.GetSelectedFieldValues("HeartBeat");
                if (Name.Count > 0)
                {
                    //DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoServerDetailsPage2.aspx?Name=" + Name[0].ToString() + "&Type=Traveler" + "&Status=" + Status[0].ToString() + "&LastDate=" + HeartBeat[0].ToString() + "");
                    Response.Redirect("~/Dashboard/DominoServerDetailsPage2.aspx?Name=" + Name[0].ToString() + "&Type=Traveler" + "&Status=" + Status[0].ToString() + "&LastDate=" + HeartBeat[0].ToString() + "", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
        }

        protected void grid_FocusedRowChanged(object sender, EventArgs e)
        {
            if (grid.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> Name = grid.GetSelectedFieldValues("Name");
                System.Collections.Generic.List<object> Status = grid.GetSelectedFieldValues("Status");
                System.Collections.Generic.List<object> HeartBeat = grid.GetSelectedFieldValues("HeartBeat");
                if (Name.Count > 0)
                {
                    //DevExpress.Web.ASPxWebControl.RedirectOnCallback("DominoServerDetailsPage2.aspx?Name=" + Name[0].ToString() + "&Type=Traveler" + "&Status=" + Status[0].ToString() + "&LastDate=" + HeartBeat[0].ToString() + "");
                    Response.Redirect("~/Dashboard/DominoServerDetailsPage2.aspx?Name=" + Name[0].ToString() + "&Type=Traveler" + "&Status=" + Status[0].ToString() + "&LastDate=" + HeartBeat[0].ToString() + "", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
        }
        //2/4/2016 NS added for VSPLUS-2570
        public void FillGrid()
        {
            try
            {
                DataTable StatusTable = VSWebBL.DashboardBL.MailHealthBL.Ins.GetMailDelivData("Domino");
                EXJournalGridView.DataSource = StatusTable;
                EXJournalGridView.DataBind();
                Session["StatusTable"] = StatusTable;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally
            {
            }
        }
        //2/4/2016 NS added for VSPLUS-2570
        public void FillQueueGrid()
        {
            try
            {
                DataTable StatusTable = VSWebBL.DashboardBL.MailHealthBL.Ins.GetMailDelivData("Exchange");
                ExchangeMailGridView.DataSource = StatusTable;
                ExchangeMailGridView.DataBind();
                Session["ExStatusTable"] = StatusTable;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally
            {
            }
        }

        protected void EXJournalGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            ASPxGridView gv = sender as ASPxGridView;
            Label hfStatus = gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "hfNameLabel1") as Label;


            if (e.DataColumn.FieldName == "Status" && (hfStatus.Text.ToString() == "OK" || hfStatus.Text.ToString() == "Scanning" || hfStatus.Text.ToString() == "Telnet"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }

            else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Not Scanned")
            {

                e.Cell.BackColor = System.Drawing.Color.FromName("#87CEEB");
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.FromName("#D0D0D0");
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Maintenance")
            {
                e.Cell.BackColor = System.Drawing.Color.LightBlue;
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
        }

        protected void EXJournalGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
        }

        protected void EXJournalGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MailDeliveryStatus|EXJournalGridView", EXJournalGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void ExchangeMailGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            ASPxGridView gv = sender as ASPxGridView;
            Label hfStatus = gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "hfExNameLabel") as Label;


            if (e.DataColumn.FieldName == "Status" && (hfStatus.Text.ToString() == "OK" || hfStatus.Text.ToString() == "Scanning" || hfStatus.Text.ToString() == "Telnet"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }

            else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Not Scanned")
            {

                e.Cell.BackColor = System.Drawing.Color.FromName("#87CEEB");
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.FromName("#D0D0D0");
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            else if (e.DataColumn.FieldName == "Status" && hfStatus.Text.ToString() == "Maintenance")
            {
                e.Cell.BackColor = System.Drawing.Color.LightBlue;
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
        }

        protected void ExchangeMailGridView_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExMailDeliveryStatus|ExchangeMailGridView", ExchangeMailGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        private void CheckSelectedFeatures()
        {
            DataTable dt = new DataTable();
            bool flag1 = false;
            bool flag2 = false;
            dt = VSWebBL.SecurityBL.MenusBL.Ins.GetSelectedFeatures();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Name"].ToString() == "Domino")
                    {
                        flag1 = true;
                    }
                    else if (dt.Rows[i]["Name"].ToString() == "Exchange")
                    {
                        flag2 = true;
                    }
                }
            }
            if (!flag1 && !flag2)
            {
                MailStatusDock.ShowOnPageLoad = false;
                TravelerDock.ShowOnPageLoad = false;
            }
            else
            {
                if (!flag1)
                {
                    dominoheaderDiv.Style.Value = "display: none";
                    spacerDiv.Style.Value = "display: none";
                    EXJournalGridView.Visible = false;
                    TravelerDock.ShowOnPageLoad = false;
                }
                else
                {
                    spacerDiv.Style.Value = "display: block";
                    FillGrid();
                    FillTravelerGrid();
                }
                if (!flag2)
                {
                    exchangeheaderDiv.Style.Value = "display: none";
                    spacerDiv.Style.Value = "display: none";
                    ExchangeMailGridView.Visible = false;
                }
                else
                {
                    FillQueueGrid();
                }
            }
        }

	}
}