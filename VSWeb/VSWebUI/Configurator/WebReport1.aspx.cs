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
using VSWebBL;
using VSWebDO;

namespace VSWebUI.Dashboard
{
    public partial class WebReport1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserFullName"] != null)
            {
                //  UserNameLabel.Text = Session["UserFullName"].ToString();
            }
			AssignStatusbox();
            if (Session["IsDashboard"] != "" && Session["IsDashboard"] != null)
            {
                if (Convert.ToBoolean(Session["IsDashboard"].ToString()) == true)
                {
                    Session.Timeout = 600;
                    // UserNameLabel.Text = Session["UserFullName"].ToString();

                    if (!IsPostBack)
                    {
                        //2/13/2013 NS added
                        ViewState["PreviousPage"] = Request.UrlReferrer;
                        // Handle the session timeout 
                        string sessionExpiredUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/SessionExpired.aspx";
                        StringBuilder script = new StringBuilder();
                        script.Append("function expireSession(){ \n");
                        script.Append(string.Format(" window.location = '{0}';\n", sessionExpiredUrl));
                        script.Append("} \n");
                        script.Append(string.Format("setTimeout('expireSession()', {0}); \n", this.Session.Timeout * 60000)); // Convert minutes to milliseconds 
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "expirescript", script.ToString(), true);
                    }
                    if (Session["Isconfigurator"] != null && Session["Isconfigurator"] != "")
                    {
                        if (Convert.ToBoolean(Session["Isconfigurator"].ToString()) == true)
                        {
                            //ASPxMenu1.Items[6].Visible = true;

                            //  dashboard.Visible = true;
                        }
                    }
                    Company cmpobj = new Company();



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
                Response.Redirect("~/SessionExpired.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
            CreateMenu1();
            fraHtml.Attributes.Add("src", Request.QueryString["url"]);
            fraHtml.Attributes.Add("target", "_top");
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
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            //6/12/2013 NS moved the redirect line below from the try/catch block. Having the call within the block was throwing an exception
            //and therefore was not logging out a user correctly.
            Response.Redirect("~/Login.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
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
			DataTable dtRow1, dtRow2, dtRow3, dtRow4; DataRow[] results;
			//CY: VS 157
			if (dtOverall.Rows.Count > 0)
			{
				//if (Session["RestrictedServers"] != "" && Session["RestrictedServers"] != null)
				//{
				//    List<int> ServerID = new List<int>();
				//    List<int> LocationID = new List<int>();
				//    DataTable resServers = (DataTable)Session["RestrictedServers"];
				//    foreach (DataRow resser in resServers.Rows)
				//    {
				//        foreach (DataRow dominorow in dtOverall.Rows)
				//        {

				//            if (resser["serverid"].ToString() == dominorow["ID"].ToString())
				//            {
				//                ServerID.Add(dtOverall.Rows.IndexOf(dominorow));
				//            }
				//        }

				//    }
				//    foreach (int Id in ServerID)
				//    {
				//        dtOverall.Rows[Id].Delete();
				//    }
				//    dtOverall.AcceptChanges();
				//}
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

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Dashboard/DeviceTypeList.aspx?server=" + SearchTextBox.Text, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        public void CreateMenu1()
        {
            try
            {
                //3/26/2015 NS modified for DevEx upgrade to 14.2
                //menu.Items.Clear();
                ASPxMenu2.Items.Clear();
                DataTable NavigatorTree = new DataTable();
                if (Session["UserID"] != null && Session["UserID"] != "")
                {
                    NavigatorTree = VSWebBL.SecurityBL.AdminTabBL.Ins.GetNavigatorByUserID(int.Parse(Session["UserID"].ToString()), "<=2", "Dashboard");

                }
                else
                {
                    NavigatorTree = VSWebBL.SecurityBL.AdminTabBL.Ins.GetNavigatorForDashboardOnly();
                }

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
                        {
                            //3/29/2015 NS added for VSPLUS-1610
                            item.Name = row["DisplayText"].ToString();
                            menuItems[parentID].Items.Add(item);
                        }
                        else
                        {
                            if (parentID == "") // It's Root Item
                                //3/26/2015 NS modified for DevEx upgrade to 14.2
                                //menu.Items.Add(item);
                                //3/29/2015 NS added for VSPLUS-1610
                                item.Name = row["DisplayText"].ToString();
                            ASPxMenu2.Items.Add(item);
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
    }
}