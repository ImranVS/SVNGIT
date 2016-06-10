using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSFramework;
using VSWebBL;
using Domino;

namespace VSWebUI
{
    public partial class WebForm4 : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                FillGrid();
                FillALLGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "MonitoredDB|MonitoredDBGridView")
                        {
                            MonitoredDBGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "MonitoredDB|AllDBGridView")
                        {
                            AllDBGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillGridfromSession();
                FillALLGridfromSession();
            }
            UpdateButtonVisibility();
        }

        public void FillGrid()
        {
            DataTable Statustab = new DataTable();
            try
            {
                Statustab = VSWebBL.DashboardBL.DatabaseHealthBL.Ins.GetData1("");

                if (Statustab.Rows.Count <= 0)
                {
                  
                }
                Session["Statustab"] = Statustab;
                MonitoredDBGridView.DataSource = Statustab;
                MonitoredDBGridView.DataBind();


            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            
        }

        public void FillGridfromSession()
        {
            DataTable dt = new DataTable();
            if(Session["Statustab"]!="" && Session["Statustab"]!=null)
            dt = Session["Statustab"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                MonitoredDBGridView.DataSource = dt;
                MonitoredDBGridView.DataBind();
            }
        }

        protected void MonitoredDBGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {

            if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Scanned")
            {
                e.Cell.BackColor = System.Drawing.Color.FromName("#87CEEB");
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            }
        }

        protected void MonitoredDBGridView_SelectionChanged(object sender, EventArgs e)
        {
            
            if (MonitoredDBGridView.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> Type = MonitoredDBGridView.GetSelectedFieldValues("Name");
                System.Collections.Generic.List<object> c = MonitoredDBGridView.GetSelectedFieldValues( "Category");
                System.Collections.Generic.List<object> ServerType = MonitoredDBGridView.GetSelectedFieldValues("Type");
                System.Collections.Generic.List<object> LastDate = MonitoredDBGridView.GetSelectedFieldValues("LastUpdate");
                System.Collections.Generic.List<object> Status = MonitoredDBGridView.GetSelectedFieldValues("Status");
                if (Type.Count > 0 && c.Count > 0)
                {
                  
                    string Name = Type[0].ToString();
                    string RT = c[0].ToString();
                    string SType = ServerType[0].ToString();
                    string LastUpdate = LastDate[0].ToString();
                    string SStatus = Status[0].ToString();
                    if(RT=="Database Response Time")                        
                 
                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("DefaultDetailsPage.aspx?Name=" + Name + "&Type=" + SType + "&Status=" + SStatus + "&LastDate=" + LastUpdate + "");
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    catch (Exception ex)
                    {
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        //throw ex;
                    }
                }


            }
        }

        //protected void AllDBGridView_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (AllDBGridView.Selection.Count > 0)
        //    {
        //        System.Collections.Generic.List<object> Type = AllDBGridView.GetSelectedFieldValues("Server");

        //        if (Type.Count > 0)
        //        {
        //            string Name = Type[0].ToString();
        //            Session["Type"] = Type[0];
        //            DevExpress.Web.ASPxWebControl.RedirectOnCallback("Performance.aspx?Name=" + Server + "");
        //            //Response.Redirect("DeviceChart.aspx");
        //        }


        //    }
        //}

        public void FillALLGrid()
        {
            DataTable Dailytab = new DataTable();
            try
            {
                Dailytab = VSWebBL.DashboardBL.DatabaseHealthBL.Ins.GetAllData("");

                if (Dailytab.Rows.Count > 0)
                {

                }
                Session["Dailytab"] = Dailytab;
                AllDBGridView.DataSource = Dailytab;
                AllDBGridView.DataBind();
                AllDBGridView.FocusedRowIndex = -1;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

        }

        public void FillALLGridfromSession()
        {
            DataTable dt = new DataTable();
            if (Session["Dailytab"] != "" && Session["Dailytab"] != null)
                dt = Session["Dailytab"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                AllDBGridView.DataSource = dt;
                AllDBGridView.DataBind();

            }
        }

        protected void AllDBGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {

            if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Scanned")
            {
                e.Cell.BackColor = System.Drawing.Color.Blue;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            }
        }

        protected void OpenButton_Click(object sender, EventArgs e)
        {
            if (AllDBGridView.FocusedRowIndex > -1)
            {
                DataTable dt = new DataTable();
                int index;
                if (AllDBGridView.FocusedRowIndex > -1)
                {
                    index = AllDBGridView.FocusedRowIndex;
                }
                else
                {
                    index = 0;
                }
                string HostName="Nohost";
                string Server = AllDBGridView.GetRowValues(index, "Server").ToString();
                DataTable servertab = VSWebBL.DashboardBL.DatabaseHealthBL.Ins.GetIPfromServers(Server);
                if(servertab.Rows.Count>0)
                    HostName = servertab.Rows[0]["IPAddress"].ToString();
                string filepath = AllDBGridView.GetRowValues(index, "Folder").ToString();
                string fileName = AllDBGridView.GetRowValues(index, "FileName").ToString();
                filepath = filepath.Replace("'\'", "'/'");
                if (filepath == "")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Script", "PopupCenter('http://" + HostName + "/" + fileName + "?OpenDatabase');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Script", "PopupCenter('http://" + HostName + "/" + filepath + "/" + fileName + "?OpenDatabase');", true);
                }
                //if (filepath == "")
                //{
                //    Response.Write("<script>window.open('http://" + HostName + "/" + fileName + "?OpenDatabase','Open Database','width=500px height=50px left=(screen.width/2)-(500/2) top=(screen.height/2)-(50/2)',1)</script>");
                //}
                //else
                //{
                //    Response.Write("<script>window.open('http://" + HostName + "/" + filepath + "/" + fileName + "?OpenDatabase','Open Database','width=500px height=50px',1)</script>");
                //}
            }
            else
            {
                msglbl.Text = "Please select a database in the All Databases grid.";
                msgPopupControl.ShowOnPageLoad = true;
            }
        }

        protected void CompactButton_Click(object sender, EventArgs e)
        {
            popuptextBox.Text = "";
            DBPopupControl.HeaderText = "Compact Options";
            ASPxLabel1.Text = "Please enter the desired COMPACT options.";
            ASPxLabel2.Text = "Use -c for corrupt databases and -B for in place compaction with file size reduction.";
            ASPxLabel3.Text = "Google 'IBM domino compact switches' for the full list of options.";
            ASPxLabel4.Visible = false;
            DBPopupControl.ShowOnPageLoad = true;
            Session["ActBtn"] = "CompactButton";
        }

        protected void FixupButton_Click(object sender, EventArgs e)
        {
            popuptextBox.Text = "";
            DBPopupControl.HeaderText = "Fixup Options";
            ASPxLabel1.Text = "Please enter the desired FIXUP options.";
            ASPxLabel2.Text = "For example, use -Q to check more quickly but less thoroughly.";
            ASPxLabel3.Text = "Google 'IBM fixup compact switches' for the full list of options.";
            ASPxLabel4.Visible = true;
            ASPxLabel4.Text = "Use -V to prevent Fixup from running on views. This option reduces the time it takes Fixup to run.";
            DBPopupControl.ShowOnPageLoad = true;
            Session["ActBtn"] = "Fixup";
        }

        protected void UpdallButton_Click(object sender, EventArgs e)
        {
            popuptextBox.Text = "";
            DBPopupControl.HeaderText = "Updall Options";
            ASPxLabel1.Text = "Please enter the desired UPDALL options.";
            ASPxLabel2.Text = "For example, use -R to rebuild all used views (resource intensive).";
            ASPxLabel3.Text = "Use -X to rebuild the full text index.";
            ASPxLabel4.Visible = false;
            DBPopupControl.ShowOnPageLoad = true;
            Session["ActBtn"] = "Updall";
        }

        protected void OKButton_Click(object sender, EventArgs e)
        {
            if (Session["ActBtn"] != "" && Session["ActBtn"] != null)
            {
                if (AllDBGridView.FocusedRowIndex > -1)
                {
                    if (Session["ActBtn"] == "CompactButton")
                    {

                        //            NotesSession  session =new NotesSession();
                        //// NotesUIWorkspace workspace = New NotesUIWorkspace();
                        //    // NotesUIDocument  uidoc= new NotesUIDocument();
                        ////Set uidoc = workspace.CurrentDocument
                        //        NotesDocument doc =new NotesDocument();
                        //    //doc = uidoc.Document
                        //// Variant StatValue;
                        //    string dbPath;
                        //if(doc.FolderReferences.Folder(0) != "" )
                        //{

                        //    dbPath = doc.FolderReferences.Folder(0) &doc.FolderReferences.Filename(0);
                        //}

                        //else
                        //                {
                        //    dbPath = doc.FolderReferences.Filename(0);
                        //}
                        ////serverName$ =doc.Server(0);
                        //   
                        //// Config$ ="lo compact "+ dbPath + " " + Options;
                        ////consoleReturn$ = session.SendConsoleCommand(serverName$, Config$)
                        ////Print  "Sent the command " &Config$  & ".  Note that this will only work if YOU have the appropriate remote console and admin rights."
                        string Server = "";
                        string Config = "";
                        string Folder = "";
                        string FileName = "";
                        DataRow myRow = AllDBGridView.GetDataRow(AllDBGridView.FocusedRowIndex);
                        if (AllDBGridView.FocusedRowIndex > -1)
                        {
                            Server = myRow["Server"].ToString();
                            Folder = myRow["Folder"].ToString();
                            FileName = myRow["Filename"].ToString();
                        }
                        string Options;
                        Options = popuptextBox.Text;
                        Config = "lo compact " + Folder + "/" + FileName + "" + Options;
                        bool returnval = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Server, Config, Session["UserFullName"].ToString());
                        if (returnval == true)
                        {
                            msglbl.Text = "Sent the following command - " + Config + ". Note that this will only work if you have the appropriate remote console and admin rights.";
                            msgPopupControl.ShowOnPageLoad = true;
                        }

                    }

                    if (Session["ActBtn"] == "Fixup")
                    {
                        string Server = "";
                        string Config = "";
                        string Folder = "";
                        string FileName = "";
                        DataRow myRow = AllDBGridView.GetDataRow(AllDBGridView.FocusedRowIndex);
                        if (AllDBGridView.FocusedRowIndex > -1)
                        {
                            Server = myRow["Server"].ToString();
                            Folder = myRow["Folder"].ToString();
                            FileName = myRow["Filename"].ToString();
                        }
                        string Options;
                        Options = popuptextBox.Text;
                        Config = "Load fixupdbpath options " + Folder + "/" + FileName + "" + Options;
                        bool returnval = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Server, Config, Session["UserFullName"].ToString());
                        if (returnval == true)
                        {
                            msglbl.Text = "Sent the following command - " + Config + ". Note that this will only work if you have the appropriate remote console and admin rights.";
                            msgPopupControl.ShowOnPageLoad = true;
                        }
                        // Config ="Load fixupdbpath options "+ dbPath + " " + Options;


                    }
                    if (Session["ActBtn"] == "Updall")
                    {
                        string Server = "";
                        string Config = "";
                        string Folder = "";
                        string FileName = "";
                        DataRow myRow = AllDBGridView.GetDataRow(AllDBGridView.FocusedRowIndex);
                        if (AllDBGridView.FocusedRowIndex > -1)
                        {
                            Server = myRow["Server"].ToString();
                            Folder = myRow["Folder"].ToString();
                            FileName = myRow["Filename"].ToString();
                        }
                        string Options;
                        Options = popuptextBox.Text;
                        Config = "Load updalldbpath options " + Folder + "/" + FileName + "" + Options;
                        bool returnval = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Server, Config, Session["UserFullName"].ToString());
                        if (returnval == true)
                        {
                            msglbl.Text = "Sent the following command - " + Config + ". Note that this will only work if you have the appropriate remote console and admin rights.";
                            msgPopupControl.ShowOnPageLoad = true;
                        }


                        // Config$ ="Load updalldbpath options"+ dbPath + " " + Options;

                    }
                    DBPopupControl.ShowOnPageLoad = false;
                  
                }

                else
                {
                    msglbl.Text = "Please select a server from the grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                    DBPopupControl.ShowOnPageLoad = false;
                }
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            DBPopupControl.ShowOnPageLoad = false;
        }

        protected void msgbtn_Click(object sender, EventArgs e)
        {
            msgPopupControl.ShowOnPageLoad = false;
        }

        public void UpdateButtonVisibility()
        {
            bool isadmin = false;
            DataTable sa = VSWebBL.SecurityBL.UsersBL.Ins.GetIsConsoleComm(Session["UserID"].ToString());
            if (sa.Rows.Count > 0)
            {
                if (sa.Rows[0]["IsConsoleComm"].ToString() == "True")
                {
                    isadmin = true;
                }
            }
            if (isadmin)
            {
                //11/10/2014 NS modified
                //CompactButton.Visible = true;
                //FixupButton.Visible = true;
                //UpdallButton.Visible = true;
                ASPxMenu1.Visible = true;
            }
        }

     

        protected void MonitoredDBGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MonitoredDB|MonitoredDBGridView", MonitoredDBGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void AllDBGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MonitoredDB|AllDBGridView", AllDBGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "OpenDBItem")
            {
                if (AllDBGridView.FocusedRowIndex > -1)
                {
                    DataTable dt = new DataTable();
                    int index;
                    if (AllDBGridView.FocusedRowIndex > -1)
                    {
                        index = AllDBGridView.FocusedRowIndex;
                    }
                    else
                    {
                        index = 0;
                    }
                    string HostName = "Nohost";
                    string Server = AllDBGridView.GetRowValues(index, "Server").ToString();
                    DataTable servertab = VSWebBL.DashboardBL.DatabaseHealthBL.Ins.GetIPfromServers(Server);
                    if (servertab.Rows.Count > 0)
                        HostName = servertab.Rows[0]["IPAddress"].ToString();
                    string filepath = AllDBGridView.GetRowValues(index, "Folder").ToString();
                    string fileName = AllDBGridView.GetRowValues(index, "FileName").ToString();
                    filepath = filepath.Replace("'\'", "'/'");
                    if (filepath == "")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Script", "PopupCenter('http://" + HostName + "/" + fileName + "?OpenDatabase');", true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Script", "PopupCenter('http://" + HostName + "/" + filepath + "/" + fileName + "?OpenDatabase');", true);
                    }
                    //if (filepath == "")
                    //{
                    //    Response.Write("<script>window.open('http://" + HostName + "/" + fileName + "?OpenDatabase','Open Database','width=500px height=50px left=(screen.width/2)-(500/2) top=(screen.height/2)-(50/2)',1)</script>");
                    //}
                    //else
                    //{
                    //    Response.Write("<script>window.open('http://" + HostName + "/" + filepath + "/" + fileName + "?OpenDatabase','Open Database','width=500px height=50px',1)</script>");
                    //}
                }
                else
                {
                    msglbl.Text = "Please select a database in the All Databases grid.";
                    msgPopupControl.ShowOnPageLoad = true;
                }
            }
            else if (e.Item.Name == "CompactItem")
            {
                popuptextBox.Text = "";
                DBPopupControl.HeaderText = "Compact Options";
                ASPxLabel1.Text = "Please enter the desired COMPACT options.";
                ASPxLabel2.Text = "Use -c for corrupt databases and -B for in place compaction with file size reduction.";
                ASPxLabel3.Text = "Google 'IBM domino compact switches' for the full list of options.";
                ASPxLabel4.Visible = false;
                DBPopupControl.ShowOnPageLoad = true;
                Session["ActBtn"] = "CompactButton";
            }
            else if (e.Item.Name == "FixupItem")
            {
                popuptextBox.Text = "";
                DBPopupControl.HeaderText = "Fixup Options";
                ASPxLabel1.Text = "Please enter the desired FIXUP options.";
                ASPxLabel2.Text = "For example, use -Q to check more quickly but less thoroughly.";
                ASPxLabel3.Text = "Google 'IBM fixup compact switches' for the full list of options.";
                ASPxLabel4.Visible = true;
                ASPxLabel4.Text = "Use -V to prevent Fixup from running on views. This option reduces the time it takes Fixup to run.";
                DBPopupControl.ShowOnPageLoad = true;
                Session["ActBtn"] = "Fixup";
            }
            else if (e.Item.Name == "UpdallItem")
            {
                popuptextBox.Text = "";
                DBPopupControl.HeaderText = "Updall Options";
                ASPxLabel1.Text = "Please enter the desired UPDALL options.";
                ASPxLabel2.Text = "For example, use -R to rebuild all used views (resource intensive).";
                ASPxLabel3.Text = "Use -X to rebuild the full text index.";
                ASPxLabel4.Visible = false;
                DBPopupControl.ShowOnPageLoad = true;
                Session["ActBtn"] = "Updall";
            }
        }
    }
}