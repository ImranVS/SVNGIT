using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using System.Drawing;
using VSWebBL;
using DevExpress.XtraCharts.Web;
using DevExpress.Web;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Xml;
using System.Text;
using System.Windows.Forms;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraPrinting;
using System.IO;
using System.Net.Mime;


namespace VSWebUI.Dashboard
{
    public partial class Sharepointdetailspage : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		// 3.5
		public bool exactmatch;

		int ServerTypeId = 4;

        protected void Page_Load(object sender, EventArgs e)
        {
            FillHealthAssessmentStatusGrid(Request.QueryString["Type"].ToString(), servernamelbl.Text);

            if (!IsPostBack && !IsCallback)
            {

                if (Request.QueryString["Type"] != "" && Request.QueryString["Type"] != null)
                {
                    lblServerType.Text = Request.QueryString["Type"].ToString() + ":  ";
                }
                if (Request.QueryString["Typ"] != "" && Request.QueryString["Typ"] != null)
                {
                    lblServerType.Text = Request.QueryString["Type"].ToString();
                }
                if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null)
                {
                    serverstatus.InnerHtml = Request.QueryString["Status"];
                    if (Request.QueryString["Status"] == "OK" || Request.QueryString["Status"] == "Scanning")
                    {
                        serverstatus.Attributes["class"] = "OK";
                    }
                    else if (Request.QueryString["Status"] == "Not Responding")
                    {
                        serverstatus.Attributes["class"] = "NotResponding";
                    }
                    else if (Request.QueryString["Status"] == "Maintenance")
                    {
                        serverstatus.Attributes["class"] = "Maintenance";
                    }
                    else
                    {
                        serverstatus.Attributes["class"] = "Issue";
                    }
                }

                if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
                {
                    servernamelbl.Text = Request.QueryString["Name"];
                    //9/19/2014 NS added for VSPLUS-934
                    servernamelbldisp.InnerHtml = lblServerType.Text + servernamelbl.Text;
                }
                if (Request.QueryString["LastDate"] != "" && Request.QueryString["LastDate"] != null)
                {
                    //10/7/2014 NS modified for VSPLUS-934
                    //Lastscanned.Text = Request.QueryString["LastDate"].ToString();
                    //Lastscanned.Text = "Last scan date: " + Request.QueryString["LastDate"].ToString();
                    Lastscanned.InnerHtml = "Last scan date: " + Request.QueryString["LastDate"].ToString();
                }
                else
                {
                    DataTable dt = VSWebBL.DashboardBL.QuickrHealthBLL.Ins.getLastScanDate(servernamelbl.Text);
                    if (dt.Rows.Count > 0)
                    {
                        //10/7/2014 NS modified for VSPLUS-934
                        //Lastscanned.Text = dt.Rows[0]["LastUpdate"].ToString();
                        //Lastscanned.Text = "Last scan date: " + dt.Rows[0]["LastUpdate"].ToString();
                        Lastscanned.InnerHtml = "Last scan date: " + Request.QueryString["LastDate"].ToString();
                    }
                    else
                    {
                        //9/19/2014 NS modified
                        //lbltext.Visible = false;
                        //Lastscanned.Visible = false;
                        Lastscanned.Style.Add("display", "none");
                    }
                }

                if (Request.QueryString["LastDate"] != "" && Request.QueryString["LastDate"] != null)
                {
                    Lastscanned.InnerHtml = "Last scan date: " + Request.QueryString["LastDate"].ToString();
                }

                else
                {
                    DataTable dt = VSWebBL.DashboardBL.QuickrHealthBLL.Ins.getLastScanDate(servernamelbl.Text);
                    if (dt.Rows.Count > 0)
                    {
                        Lastscanned.InnerHtml = "Last scan date: " + Request.QueryString["LastDate"].ToString();
                    }
                    else
                    {
                        // lbltext.Visible = false;
                        Lastscanned.Style.Add("display", "none");
                    }
                }

				DataTable dt1 = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.sharepointtab(servernamelbl.Text);
				string VMS = "Virtual Server Manufacture";
				//DataTable dt1 = new DataTable();
				lblvrtlsvr.Text = (from DataRow dr in dt1.Rows
								   where (string)dr["Sname"] == VMS
								   select (string)dr["Svalue"]).FirstOrDefault();
				string model = "Model";
				//DataTable dt1 = new DataTable();
				lblmodel.Text = (from DataRow dr in dt1.Rows
								 where (string)dr["Sname"] == model
								 select (string)dr["Svalue"]).FirstOrDefault();
				string PP = "Physical Processors";
				//DataTable dt1 = new DataTable();
				Labelprocessrs.Text = (from DataRow dr in dt1.Rows
									   where (string)dr["Sname"] == PP
									   select (string)dr["Svalue"]).FirstOrDefault();
				string LP = "Logical Processors";
				//DataTable dt1 = new DataTable();
				Labellprcessrs.Text = (from DataRow dr in dt1.Rows
									   where (string)dr["Sname"] == LP
									   select (string)dr["Svalue"]).FirstOrDefault();

				string TPM = "Total Physical Memory";
				//DataTable dt1 = new DataTable();
				Labelttlphmry.Text = (from DataRow dr in dt1.Rows
									  where (string)dr["Sname"] == TPM
									  select (string)dr["Svalue"]).FirstOrDefault();

				string TLM = "Total Logical Memory";
				//DataTable dt1 = new DataTable();
				Labelttlmry.Text = (from DataRow dr in dt1.Rows
									where (string)dr["Sname"] == TLM
									select (string)dr["Svalue"]).FirstOrDefault();

				string IIS = "IIS Version";
				//DataTable dt1 = new DataTable();
				Labeliisvrsn.Text = (from DataRow dr in dt1.Rows
									 where (string)dr["Sname"] == IIS
									 select (string)dr["Svalue"]).FirstOrDefault();
				string IISSS = "IIS Service State";
				//DataTable dt1 = new DataTable();
				Labeliissrvstate.Text = (from DataRow dr in dt1.Rows
										 where (string)dr["Sname"] == IISSS
										 select (string)dr["Svalue"]).FirstOrDefault();
				string ASP = "ASP.NET Version";
				//DataTable dt1 = new DataTable();
				lblaspntvsn.Text = (from DataRow dr in dt1.Rows
									where (string)dr["Sname"] == ASP
									select (string)dr["Svalue"]).FirstOrDefault();
				string NS = "NetworkStatus";
				//DataTable dt1 = new DataTable();
				lblstatus.Text = (from DataRow dr in dt1.Rows
								  where (string)dr["Sname"] == NS
								  select (string)dr["Svalue"]).FirstOrDefault();

				if (lblstatus.Text == "OK")
				{
					lblstatus.BackColor = System.Drawing.Color.Green;
					lblstatus.ForeColor = System.Drawing.Color.White;
					lblnwmsg.Text = "'All Metrics are OK'";
					lblnwmsg.ForeColor = System.Drawing.Color.Black;

				}
				else if (lblstatus.Text == "Not Responding")
				{
					lblstatus.BackColor = System.Drawing.Color.Red;
					lblstatus.ForeColor = System.Drawing.Color.White;
					lblnwmsg.Text = "'1 Metric is not OK'";
					lblnwmsg.ForeColor = System.Drawing.Color.Red;
				}
				else if (lblstatus.Text == "Not Scanned")
				{
					lblstatus.BackColor = System.Drawing.Color.FromName("#87CEEB");
					lblstatus.ForeColor = System.Drawing.Color.Black;
					lblnwmsg.Text = "'1 Metric is not OK'";
					lblnwmsg.ForeColor = System.Drawing.Color.Red;
				}
				else if (lblstatus.Text == "Warning")
				{
					lblstatus.BackColor = System.Drawing.Color.Yellow;
					lblstatus.ForeColor = System.Drawing.Color.Black;
					lblnwmsg.Text = "'1 Metric is not OK'";
					lblnwmsg.ForeColor = System.Drawing.Color.Red;
				}
				else if (lblstatus.Text == "Maintenance")
				{
					lblstatus.BackColor = System.Drawing.Color.LightBlue;
					lblstatus.ForeColor = System.Drawing.Color.Black;

				}
				else if (lblstatus.Text == "")
				{
					lblstatus.BackColor = System.Drawing.Color.FromName("#ffff40");
					lblstatus.ForeColor = System.Drawing.Color.FromName("#808080");
				}

				string NBR = "NetworkBytes Received";
				//DataTable dt1 = new DataTable();
				lblnwbyr.Text = (from DataRow dr in dt1.Rows
								 where (string)dr["Sname"] == NBR
								 select (string)dr["Svalue"]).FirstOrDefault();

				string NBS = "NetworkBytesSent";
				//DataTable dt1 = new DataTable();
				lblnbysnt.Text = (from DataRow dr in dt1.Rows
								  where (string)dr["Sname"] == NBS
								  select (string)dr["Svalue"]).FirstOrDefault();
				string IISts = "IISStatus";
				//DataTable dt1 = new DataTable();
				lbliistatus.Text = (from DataRow dr in dt1.Rows
									where (string)dr["Sname"] == IISts
									select (string)dr["Svalue"]).FirstOrDefault();
				if (lbliistatus.Text == "OK")
				{
					lbliistatus.BackColor = System.Drawing.Color.Green;
					lbliistatus.ForeColor = System.Drawing.Color.White;
					lbliimsg.Text = "'All Metrics are OK'";
					lbliimsg.ForeColor = System.Drawing.Color.Black;


				}
				else if (lbliistatus.Text == "Not Responding")
				{
					lbliistatus.BackColor = System.Drawing.Color.Red;
					lbliistatus.ForeColor = System.Drawing.Color.White;
					lbliimsg.Text = "'1 Metric is not OK'";
					lbliimsg.ForeColor = System.Drawing.Color.Red;
				}
				else if (lbliistatus.Text == "Not Scanned")
				{
					lbliistatus.BackColor = System.Drawing.Color.FromName("#87CEEB");
					lbliistatus.ForeColor = System.Drawing.Color.Black;
					lbliimsg.Text = "'1 Metric is not OK'";
					lbliimsg.ForeColor = System.Drawing.Color.Red;
				}
				else if (lbliistatus.Text == "Warning")
				{
					lbliistatus.BackColor = System.Drawing.Color.FromName("#FFFF00");
					lbliistatus.ForeColor = System.Drawing.Color.Black;
					lbliimsg.Text = "'1 Metric is not OK'";
					lbliimsg.ForeColor = System.Drawing.Color.Red;
				}
				else if (lbliistatus.Text == "Maintenance")
				{
					lbliistatus.BackColor = System.Drawing.Color.LightBlue;
					lbliistatus.ForeColor = System.Drawing.Color.Black;
				}
				else if (lbliistatus.Text == "")
				{
					lbliistatus.BackColor = System.Drawing.Color.FromName("#ffff40");
					lbliistatus.ForeColor = System.Drawing.Color.FromName("#808080");

				}

				string IISapp = "IISAppRequests";
				//DataTable dt1 = new DataTable();
				Labelappre.Text = (from DataRow dr in dt1.Rows
								   where (string)dr["Sname"] == IISapp
								   select (string)dr["Svalue"]).FirstOrDefault();
				string iisappre = "IISAppRequests Rejected";
				//DataTable dt1 = new DataTable();
				Labelapprerj.Text = (from DataRow dr in dt1.Rows
									 where (string)dr["Sname"] == iisappre
									 select (string)dr["Svalue"]).FirstOrDefault();
				string iiscntns = "IISCurrent Connections";
				//DataTable dt1 = new DataTable();
				Labelccntns.Text = (from DataRow dr in dt1.Rows
									where (string)dr["Sname"] == iiscntns
									select (string)dr["Svalue"]).FirstOrDefault();
				string WS = "WebStatus";
				//DataTable dt1 = new DataTable();
				lblwebsts.Text = (from DataRow dr in dt1.Rows
								  where (string)dr["Sname"] == WS
								  select (string)dr["Svalue"]).FirstOrDefault();
				if (lblwebsts.Text == "OK")
				{
					lblwebsts.BackColor = System.Drawing.Color.Green;
					lblwebsts.ForeColor = System.Drawing.Color.White;
					lblmsg.Text = "'All Metrics are OK'";
					lblmsg.ForeColor = System.Drawing.Color.Black;


				}
				else if (lblwebsts.Text == "Not Responding")
				{
					lblwebsts.BackColor = System.Drawing.Color.Red;
					lblwebsts.ForeColor = System.Drawing.Color.White;
					lblmsg.Text = "'1 Metric is not OK'";
					lblmsg.ForeColor = System.Drawing.Color.Red;
				}
				else if (lblwebsts.Text == "Not Scanned")
				{
					lblwebsts.BackColor = System.Drawing.Color.FromName("#87CEEB");
					lblwebsts.ForeColor = System.Drawing.Color.Black;
					lblmsg.Text = "'1 Metric is not OK'";
					lblmsg.ForeColor = System.Drawing.Color.Red;
				}
				else if (lblwebsts.Text == "Warning")
				{
					lblwebsts.BackColor = System.Drawing.Color.FromName("#D0D0D0");
					lblwebsts.ForeColor = System.Drawing.Color.Black;
					lblmsg.Text = "'1 Metric is not OK'";
					lblmsg.ForeColor = System.Drawing.Color.Red;
				}
				else if (lblwebsts.Text == "Maintenance")
				{
					lblwebsts.BackColor = System.Drawing.Color.LightBlue;
					lblwebsts.ForeColor = System.Drawing.Color.Black;

				}
				else if (lblwebsts.Text == "")
				{
					lblwebsts.BackColor = System.Drawing.Color.FromName("#ffff40");
					lblwebsts.ForeColor = System.Drawing.Color.FromName("#808080");

				}
				string WBR = "WebServices Bytes Received";
				//DataTable dt1 = new DataTable();
				lblwebbrevd.Text = (from DataRow dr in dt1.Rows
									where (string)dr["Sname"] == WBR
									select (string)dr["Svalue"]).FirstOrDefault();

				string WBS = "WebServices Bytes Sent";
				//DataTable dt1 = new DataTable();
				lblwebbysnt.Text = (from DataRow dr in dt1.Rows
									where (string)dr["Sname"] == WBS
									select (string)dr["Svalue"]).FirstOrDefault();
                // FillCombobox();

                //01/23/2014 MD changed it for every page load for the graphs to be populated always.
                //SetGraphForDiskSpace(servernamelbl.Text,"Disk.C");
				// VSPLUS-2197====version 3.5 soma
			
                SetGridForsharepointDisk(servernamelbl.Text);
                SetGraphForMemorysharepoint("hh", servernamelbl.Text);
                SetGraphForCPUsharepoint("hh", servernamelbl.Text);

                SetGraphForsharepointEnabledUsers("hh", servernamelbl.Text);

                //7/14/2014 NS added for VSPLUS-813
                //5/6/2015 NS modified for VSPLUS-1707
                SetGraphForDiskSpace2("'" + servernamelbl.Text + "'");
                DiskSpaceWebChartControl1.ClientVisible = true;

                DiskSpaceLync.ClientVisible = false;
                SetGraphforperformance("hh", servernamelbl.Text);

                //12/4/2013 NS commented out - the page should only load on tab click
                //2/12/2014 NS uncommented out - tab click event takes too long, we will use a link to open Web Admin in a new window instead


                // DataBase Code
                if (!IsPostBack && !IsCallback)
                {
                    // performanceASPxRadioButtonList.Items.RemoveAt(0);
                    // performanceASPxRadioButtonList.Items[0].Selected = true;

					fillcombo();
                    Fillmaintenancegrid();
                    FillAlertHistory();
                    FillOutageTab();
                    FillHealthAssessmentStatusGrid(Request.QueryString["Type"].ToString(), servernamelbl.Text);
					FillMonitoredServicesGrid();
					FillUnMonitoredServicesGrid();


                    if (Session["UserPreferences"] != null)
                    {
                        DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                        foreach (DataRow dr in UserPreferences.Rows)
                        {
                            //if (dr[1].ToString() == "DominoServerDetailsPage2|MonitoredDBGridView")
                            //{
                            //    MonitoredDBGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                            //}
                            //if (dr[1].ToString() == "DominoServerDetailsPage2|AllDBGridView")
                            //{
                            //    AllDBGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                            //}
                            //if (dr[1].ToString() == "DominoServerDetailsPage2|DominoserverTasksgrid")
                            //{
                            //    DominoserverTasksgrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                            //}
                            //if (dr[1].ToString() == "DominoServerDetailsPage2|NonmoniterdGrid")
                            //{
                            //    NonmoniterdGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                            //}
                            if (dr[1].ToString() == "LyncServerDetailsPage|maintenancegrid")
                            {
                                maintenancegrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                            }
                            if (dr[1].ToString() == "LyncServerDetailsPage|AlertGridView")
                            {
                                AlertGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                            }
                            if (dr[1].ToString() == "LyncServerDetailsPage|OutageGridView")
                            {
                                OutageGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                            }
                        }
                    }
                }
                else
                {
                    //2/6/2014 NS commented out the calls below - will move them into a tab click event and load only the ones
                    //relevant to the tab
                    //2/12/2014 NS uncommented out - the tab click event takes too long, tab loading looks like it's not happening

                    SetGridFromSession();
                    FillMonitoredServicesGrid();
                    FillUnMonitoredServicesGrid();
                    Fillmaintenancegridfromsession();
                    FillAlertHistoryfromSession();
                    FilloutagefromSession();

                    FillHealthAssessmentStatusGrid(Request.QueryString["Type"].ToString(), servernamelbl.Text);

                }
                //2/6/2013 NS added
                 UpdateButtonVisibility();
            }
        }
		protected void DateParamEdit_DateChanged(object sender, EventArgs e)
		{
			ReloadGrid();
		}
		// VSPLUS-2197====version 3.5 soma
		public void ReloadGrid()
		{
			string date;
			exactmatch = false;

			if (startDate.Value.ToString() == "")
			{
				date = DateTime.Now.ToString();
			}
			else
			{
				date = startDate.Value.ToString();
			}
			DateTime dtval = Convert.ToDateTime(date);
			DataTable dt = new DataTable();
			if (EventTypeComboBox.SelectedIndex != -1)
			{
				if (EventTypeComboBox.SelectedIndex == 0)
					dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistorydetails(servernamelbl.Text, Request.QueryString["Type"].ToString());
				else
					dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistoryByLogdetails(EventTypeComboBox.SelectedItem.Value.ToString(), servernamelbl.Text, Request.QueryString["Type"].ToString());
			}
			else
			{
				dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistorydetails(dtval.Month.ToString(), dtval.Year.ToString(), servernamelbl.Text);
			}
			EventsHistory.DataSource = dt;
			EventsHistory.DataBind();
		}
		// VSPLUS-2197====version 3.5 soma
		protected void ExportXlsButton_Click(object sender, EventArgs e)
		{

			ServerGridViewExporter.WriteXlsToResponse();
		}

		protected void ExportXlsxButton_Click(object sender, EventArgs e)
		{

			ServerGridViewExporter.WriteXlsxToResponse();
		}

		protected void ExportPdfButton_Click(object sender, EventArgs e)
		{
			ServerGridViewExporter.WritePdfToResponse();
		}
		protected void EventTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (EventTypeComboBox.SelectedIndex != -1)
			{
				DataTable dt = new DataTable();
				if (EventTypeComboBox.SelectedIndex == 0)
					dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistorydetails(servernamelbl.Text, Request.QueryString["Type"].ToString());
				else
					dt = VSWebBL.ConfiguratorBL.ELSBL.Ins.GetEventHistoryByLogdetails(EventTypeComboBox.SelectedItem.Value.ToString(), servernamelbl.Text, Request.QueryString["Type"].ToString());
				EventsHistory.DataSource = dt;
				EventsHistory.DataBind();
			}
		}
		// VSPLUS-2197====version 3.5 soma
		public void fillcombo()
		{
			EventTypeComboBox.Items.Add("All Logs", "All Logs");
			EventTypeComboBox.Items.Add("Application", "Application");
			EventTypeComboBox.Items.Add("Security", "Security");
			EventTypeComboBox.Items.Add("Setup", "Setup");
			EventTypeComboBox.Items.Add("System", "System");

		}
		// VSPLUS-2197====version 3.5 soma
		public static void WriteResponse(HttpResponse response, byte[] filearray, string type)
		{
			response.ClearContent();
			response.Buffer = true;
			response.Cache.SetCacheability(HttpCacheability.Private);
			response.ContentType = "application/pdf";
			ContentDisposition contentDisposition = new ContentDisposition();
			contentDisposition.FileName = "EventsHistory.pdf";
			contentDisposition.DispositionType = type;
			response.AddHeader("Content-Disposition", contentDisposition.ToString());
			response.BinaryWrite(filearray);
			HttpContext.Current.ApplicationInstance.CompleteRequest();
			try
			{
				response.End();
			}
			catch (System.Threading.ThreadAbortException)
			{
			}

		}
		// VSPLUS-2197====version 3.5 soma
		protected void EventSettings_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{
			if (e.Item.Name == "ExportXLSItem")
			{
				if (startDate.Value != "")
				{
					ServerGridViewExporter.FileName = "EventsHistory" + "_" + startDate.Value;
					ServerGridViewExporter.WriteXlsToResponse();
				}
				else
				{
					ServerGridViewExporter.FileName = "EventsHistory" + "_" + DateTime.Now.ToString();
					ServerGridViewExporter.WriteXlsToResponse();
				}
			}
			else if (e.Item.Name == "ExportXLSXItem")
			{
				if (startDate.Value != "")
				{
					ServerGridViewExporter.FileName = "EventsHistory" + "_" + startDate.Value;
					ServerGridViewExporter.WriteXlsxToResponse();
				}
				else
				{
					ServerGridViewExporter.FileName = "EventsHistory" + "_" + DateTime.Now.ToString();
					ServerGridViewExporter.WriteXlsxToResponse();
				}
			}
			else if (e.Item.Name == "ExportPDFItem")
			{
				if (startDate.Value != "")
				{
					ServerGridViewExporter.FileName = "EventsHistory" + "_" + startDate.Value;
					//ServerGridViewExporter.WriteXlsxToResponse();
				}
				else
				{
					ServerGridViewExporter.FileName = "EventsHistory" + "_" + DateTime.Now.ToString();
					//ServerGridViewExporter.WriteXlsxToResponse();
				}

				ServerGridViewExporter.Landscape = true;
				using (MemoryStream ms = new MemoryStream())
				{
					PrintableComponentLink pcl = new PrintableComponentLink(new PrintingSystem());
					pcl.Component = ServerGridViewExporter;
					pcl.Margins.Left = pcl.Margins.Right = 50;
					pcl.Landscape = true;
					pcl.CreateDocument(false);
					pcl.PrintingSystem.Document.AutoFitToPagesWidth = 1;
					pcl.ExportToPdf(ms);

					WriteResponse(this.Response, ms.ToArray(), System.Net.Mime.DispositionTypeNames.Attachment.ToString());
					//ServerGridViewExporter.WritePdfToResponse();
				}
			}
		}
		// VSPLUS-2197====version 3.5 soma
		protected void EventsHistoryGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("OverallServerAlerts|EventsHistory", EventsHistory.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		// VSPLUS-2197====version 3.5 soma
		protected void EventsHistory_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			if (e.DataColumn.FieldName == "EntryType" && e.CellValue.ToString() == "Error")
			{
				e.Cell.BackColor = System.Drawing.Color.Red;
				e.Cell.ForeColor = System.Drawing.Color.White;
			}
			else if (e.DataColumn.FieldName == "EntryType" && e.CellValue.ToString() == "Warning")
			{
				e.Cell.BackColor = System.Drawing.Color.Yellow;
				e.Cell.ForeColor = System.Drawing.Color.Black;
			}
		}
		// VSPLUS-2197====version 3.5 soma
        public void SetGridForsharepointDisk(string ServerName)
        {
            try
            {
                DataTable dt = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.SetGridForsharepointDisk(ServerName);
                Session["GridData"] = dt;
                DiskSpaceLync.DataSource = dt;
                DiskSpaceLync.DataBind();
                //((GridViewDataColumn)DiskHealthGrid.Columns["ServerName"]).GroupBy();
                int rowIndex = DiskSpaceLync.FindVisibleIndexByKeyValue("ID");
                //Session["rowIndex"] = rowIndex;
            }
            catch (Exception ex)
            {
                //Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        public void FillHealthAssessmentFromSession()
        {
            try
            {
                if (Session["GridData"] != null)
                {
                    DataTable dt = Session["HealthAssessment"] as DataTable;
                    HealthAssessmentGrid11.DataSource = dt;
                    HealthAssessmentGrid11.DataBind();

                }
            }
            catch (Exception ex)
            {
                //Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        private void FillHealthAssessmentStatusGrid(string type, string servername)
        {
            try
            {
                //DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetCASStatus(servernamelbl.Text);
                //3/25/2014 NS modified - we only need CAS to pass as a category
                //DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetStatusDetails(servernamelbl.Text,"CAS Status");
                string TypeAndName = servername + "-" + type;
                DataTable StatusTable12 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetHealthAssessmentStatusDetails(TypeAndName);

                Session["HealthAssessment"] = StatusTable12;
                HealthAssessmentGrid11.DataSource = StatusTable12;
                HealthAssessmentGrid11.DataBind();
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
        protected void HealthAssessmentGrid11_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            //3/25/2014 NS modified
            if (e.DataColumn.FieldName == "Result" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning" || e.CellValue.ToString() == "Telnet" ||
                e.CellValue.ToString() == "Running" || e.CellValue.ToString() == "Pass") || e.CellValue.ToString() == "Passed")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "Result" && (e.CellValue.ToString() == "Not Responding" || e.CellValue.ToString().ToUpper() == "FAIL"))
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "Not Scanned")
            {
                e.Cell.BackColor = System.Drawing.Color.Blue;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "Disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.FromName("#D0D0D0");
                //e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "ISSUE")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            }

        }
        //;;;;;;;;;;;;;;
        public void SetGridFromSession()
        {
            try
            {
                if (Session["GridData"] != null)
                {
                    DataTable dt = Session["GridData"] as DataTable;
                    DiskSpaceLync.DataSource = dt;
                    DiskSpaceLync.DataBind();
                    int rowIndex = DiskSpaceLync.FindVisibleIndexByKeyValue("ID");
                    Session["rowIndex"] = rowIndex;
                }
            }
            catch (Exception ex)
            {
                //Response.Write("Error : " + ex);
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        public void FillMonitoredServicesGrid()
        {

            try
            {
                if (VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.isResponding(servernamelbl.Text + "-SharePoint"))
                {
                    DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetWindowsServices(servernamelbl.Text);
                    Session["MonitoredServices"] = StatusTable1;
                    MonitoredServicesGrid.DataSource = StatusTable1;
                    MonitoredServicesGrid.DataBind();
                    (MonitoredServicesGrid.Columns["Type"] as GridViewDataColumn).GroupBy();
                }


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
        protected void MonitoredServicesGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
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
            //else if (e.DataColumn.FieldName == "Result" && (e.CellValue.ToString() == "Stopped"))
            //{
            //    e.Cell.BackColor = System.Drawing.Color.Red;
            //    e.Cell.ForeColor = System.Drawing.Color.White; 
            //} 
            //else if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "Not Scanned")
            //{
            //    e.Cell.BackColor = System.Drawing.Color.Blue;
            //    e.Cell.ForeColor = System.Drawing.Color.White;
            //}
            //else if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "Disabled")
            //{
            //    e.Cell.BackColor = System.Drawing.Color.FromName("#D0D0D0");
            //    //e.Cell.BackColor = System.Drawing.Color.Gray;
            //    // e.Cell.ForeColor = System.Drawing.Color.White;
            //}
            //else if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "Pass")
            //{
            //    e.Cell.BackColor = System.Drawing.Color.LightGreen;
            //    // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            //}
            //else if (e.DataColumn.FieldName == "Result" && e.CellValue.ToString() == "Fail")
            //{
            //    e.Cell.BackColor = System.Drawing.Color.Red;
            //    e.Cell.ForeColor = System.Drawing.Color.White; 
            //    // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            //}
            //else if (e.DataColumn.FieldName == "Result")
            //{
            //    e.Cell.BackColor = System.Drawing.Color.Yellow;
            //    // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            //}
        }
        public void FillUnMonitoredServicesGrid()
        {

            try
            {
                if (VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.isResponding(servernamelbl.Text + "-SharePoint"))
                {
                    DataTable StatusTable1 = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetUnmonitoredWindowsServices(servernamelbl.Text);
                    Session["UnMonitoredServices"] = StatusTable1;
                    UnMonitoredServicesGrid.DataSource = StatusTable1;
                    UnMonitoredServicesGrid.DataBind();
                }


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
        protected void MonitoredServicesGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ExchangeServerDetailsPage3|MonitoredServicesGrid", MonitoredServicesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void DiskSpaceLync_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            string thresholdType = e.GetValue("ThresholdType").ToString();
            //if (thresholdType == "")
            //    thresholdType = "Percent";
            double valThreshold;
            if (e.GetValue("Threshold").ToString() != "")
            {
                valThreshold = (double)e.GetValue("Threshold");

                //if the threshold type is "%" then divide the value by 100
                if (thresholdType == "Percent" && valThreshold > 0)
                    valThreshold = (valThreshold / 100);
            }
            else
            {
                valThreshold = 0.0;
            }
            if (e.DataColumn.FieldName == "DiskName")
            {
                if (thresholdType == "Percent")
                {
                    double valPercentFree;
                    if (e.GetValue("PercentFree").ToString() != "")
                    {
                        valPercentFree = (double)e.GetValue("PercentFree");
                    }
                    else
                    {
                        valPercentFree = 0.0;
                    }

                    if (valPercentFree > valThreshold && (valPercentFree <= 1 - (0.8 * (1 - valThreshold))))
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    else if ((valPercentFree) <= valThreshold)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
            if (e.DataColumn.FieldName == "DiskName")
            {
                if (thresholdType == "GB")
                {
                    double valGBFree;
                    if (e.GetValue("DiskFree").ToString() != "")
                    {
                        valGBFree = (double)e.GetValue("DiskFree");
                    }
                    else
                    {
                        valGBFree = 0.0;
                    }

                    //double valPercentFree = (double)e.GetValue("PercentFree");

                    //if (valGBFree > valThreshold && (valPercentFree <= 1 - (0.8 * (1 - 0))))
                    if (valGBFree > valThreshold && (valGBFree <= (valThreshold + ((20 * valThreshold) / 100))))
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    else if ((valGBFree) <= valThreshold)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
            //if (e.DataColumn.FieldName == "AverageQueueLength")
            //{
            //    double valThreshold;
            //    if (e.GetValue("Threshold").ToString() != "")
            //    {
            //        valThreshold = (double)e.GetValue("Threshold");
            //    }
            //    else
            //    {
            //        valThreshold = 0.0;
            //    }

            //    if ((valThreshold) >= 0.3 && valThreshold < 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Yellow;
            //    }
            //    else if ((valThreshold) >= 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Red;
            //    }
            //    else
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Green;
            //        e.Cell.ForeColor = System.Drawing.Color.White;
            //    }
            //}
            //if (e.DataColumn.FieldName == "PercentUtilization")
            //{
            //    double valThreshold;
            //    if (e.GetValue("Threshold").ToString() != "")
            //    {
            //        valThreshold = (double)e.GetValue("Threshold");
            //    }
            //    else
            //    {
            //        valThreshold = 0.0;
            //    }

            //    if ((valThreshold) >= 0.8 && valThreshold < 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Yellow;
            //    }
            //    else if ((valThreshold) >= 1.0)
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Red;
            //    }
            //    else
            //    {
            //        e.Cell.BackColor = System.Drawing.Color.Green;
            //        e.Cell.ForeColor = System.Drawing.Color.White;
            //    }
            //}
        }

        protected void DiskSpaceLync_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //ASPxGridView grid = sender as ASPxGridView;
            //int index = int.Parse(e.Parameters);
            //int exCount = 0;
            //for (int i = grid.SettingsPager.PageSize * grid.PageIndex; i < index; i++)
            //{
            //    if (grid.IsRowExpanded(i))
            //        exCount += 1;
            //}
            //grid.CollapseAll();
            //grid.FocusedRowIndex = index - exCount / 2;
            //grid.ExpandRow(index - exCount / 2);

        }

        protected void DiskSpaceLync_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            int index = DiskSpaceLync.FocusedRowIndex;
            if (e.VisibleIndex != index)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

            }
        }
        protected void DiskSpaceLync_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DiskHealth|DiskHealthGrid", DiskSpaceLync.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        public void FillOutageTab()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.GetOutage(servernamelbl.Text, Request.QueryString["Type"].ToString());
            Session["Outage"] = dt;
            OutageGridView.DataSource = dt;
            OutageGridView.DataBind();
        }
        public void FilloutagefromSession()
        {
            DataTable dt = new DataTable();
            dt = Session["Outage"] as DataTable;
            OutageGridView.DataSource = dt;
            OutageGridView.DataBind();
        }

        protected void ASPxMenu1_ItemClick1(object source, DevExpress.Web.MenuItemEventArgs e)
        {
            if (e.Item.Name == "BackItem")
            {
                if (Session["BackURL"] != "" && Session["BackURL"] != null)
                {
                    Response.Redirect(Session["BackURL"].ToString());
                    Session["BackURL"] = "";

                }
            }
            else if (e.Item.Name == "ScanItem")
            {
                try
                {
                    Status StatusObj = new Status();
                    StatusObj.Name = Request.QueryString["Name"];
                    StatusObj.Type = Request.QueryString["Type"];

                    bool bl = VSWebBL.StatusBL.StatusTBL.Ins.UpdateforScan(StatusObj);

                    if (Request.QueryString["Type"] == "Domino")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanDominoASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                    else if (Request.QueryString["Type"] == "Mail")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanMailServiceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                    else if (Request.QueryString["Type"] == "Network Device")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanNetworkDeviceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                    else if (Request.QueryString["Type"] == "Sametime Server")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSametimeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                    else if (Request.QueryString["Type"] == "BES")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanBlackBerryASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                    else if (Request.QueryString["Type"] == "URL")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanURLASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                    else if (Request.QueryString["Type"] == "Exchange")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanExchangeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                    else if (Request.QueryString["Type"] == "SharePoint")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSharePointASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
					else if (Request.QueryString["Type"] == "Skype for Business")
                    {
						bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("LyncASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                    }
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }
            }
            else if (e.Item.Name == "EditConfigItem")
            {
                string id = "";
                string name = "";
                Session["Submenu"] = "MSServers";
                if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
                {
                    id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
                    name = Request.QueryString["Name"].ToString();
                    DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByName1(name);
                    string Loc = dt.Rows[0]["Location"].ToString();
                    string Cat = dt.Rows[0]["ServerType"].ToString();
                    //servernamelbl.Text

                    Response.Redirect("~/Configurator/SharepointServer.aspx?ID=" + id + "&Name=" + name + "&Cat=" + dt.Rows[0]["ServerType"].ToString() + "&Loc=" + dt.Rows[0]["Location"].ToString() + "&ipaddr=" + dt.Rows[0]["ipaddress"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            else if (e.Item.Name == "SuspendItem")
            {
                ASPxPopupControl1.HeaderText = "Suspend Temporarily";
                ASPxPopupControl1.ShowOnPageLoad = true;
            }
        }
     
        public DataTable SetGraphforperformance(string paramGraph, string DeviceName)
        {
            try
            {
                performanceWebChartControl.Series.Clear();
				DataTable dt = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.SetGraphforperformance(paramGraph, DeviceName, ServerTypeId);

                Series series = null;
                series = new Series("DominoServer", ViewType.Line);
                series.Visible = true;
                series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

                ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
                seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
                performanceWebChartControl.Series.Add(series);

                // Constant Line

                // Cast the chart's diagram to the XYDiagram type, to access its axes.
                XYDiagram diagram = (XYDiagram)performanceWebChartControl.Diagram;
                //Mukund 16Jul14, VSPLUS-824- Threshold in graph is not updating
                DataTable dt1 = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.ResponseThresholdForsharepoint(DeviceName);
                if (dt1.Rows.Count > 0)
                {
                    if (int.Parse(dt1.Rows[0]["ResponseThreshold"].ToString()) > 0)
                    {
                        // Create a constant line.
                        ConstantLine constantLine1 = new ConstantLine("Constant Line 1");
                        diagram.AxisY.ConstantLines.Add(constantLine1);

                        // Define its axis value.
                        constantLine1.AxisValue = dt1.Rows[0]["ResponseThreshold"].ToString();

                        // Customize the behavior of the constant line.
                        // constantLine1.Visible = true;
                        //constantLine1.ShowInLegend = true;
                        // constantLine1.LegendText = "Some Threshold";
                        constantLine1.ShowBehind = true;

                        // Customize the constant line's title.
                        constantLine1.Title.Visible = true;
                        constantLine1.Title.Text = "Threshold:" + dt1.Rows[0]["ResponseThreshold"].ToString();
                        constantLine1.Title.TextColor = Color.Red;
                        // constantLine1.Title.Antialiasing = false;
                        //constantLine1.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);
                        constantLine1.Title.ShowBelowLine = true;
                        constantLine1.Title.Alignment = ConstantLineTitleAlignment.Far;

                        // Customize the appearance of the constant line.
                        constantLine1.Color = Color.Red;
                        constantLine1.LineStyle.DashStyle = DashStyle.Solid;
                        constantLine1.LineStyle.Thickness = 2;
                    }
                }
                ((XYDiagram)performanceWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

                XYDiagram seriesXY = (XYDiagram)performanceWebChartControl.Diagram;
                seriesXY.AxisY.Title.Text = "Response Time";
                seriesXY.AxisY.Title.Visible = true;
                seriesXY.AxisX.Title.Text = "Date/Time";
                seriesXY.AxisX.Title.Visible = true;
                seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;

                performanceWebChartControl.Legend.Visible = false;

                // ((SplineSeriesView)series.View).LineTensionPercent = 100;
                ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
                ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

                AxisBase axis = ((XYDiagram)performanceWebChartControl.Diagram).AxisX;
                //4/18/2014 NS commented out for VSPLUS-312
                //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
                axis.Range.SideMarginsEnabled = false;
                axis.GridLines.Visible = false;
                //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
                //axis.DateTimeOptions.FormatString = "MM/dd/yyyy HH:mm";
                ((LineSeriesView)series.View).Color = Color.Blue;

                AxisBase axisy = ((XYDiagram)performanceWebChartControl.Diagram).AxisY;
                axisy.Range.AlwaysShowZeroLevel = false;
                axisy.Range.SideMarginsEnabled = true;
                axisy.GridLines.Visible = true;
                performanceWebChartControl.DataSource = dt;
                performanceWebChartControl.DataBind();

                return dt;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

        }

        public void SetGraphForDiskSpace2(string serverName)
        {
            DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.SetGraphForDiskSpace(serverName, "","");
            DiskSpaceWebChartControl1.DataSource = dt;
            DiskSpaceWebChartControl1.Series[0].DataSource = dt;
            DiskSpaceWebChartControl1.Series[0].ArgumentDataMember = dt.Columns["DiskName"].ToString();
            DiskSpaceWebChartControl1.Series[0].ValueDataMembers.AddRange(dt.Columns["DiskUsed"].ToString());
            DiskSpaceWebChartControl1.Series[0].Visible = true;
            DiskSpaceWebChartControl1.Series[1].DataSource = dt;
            DiskSpaceWebChartControl1.Series[1].ArgumentDataMember = dt.Columns["DiskName"].ToString();
            DiskSpaceWebChartControl1.Series[1].ValueDataMembers.AddRange(dt.Columns["DiskFree"].ToString());
            DiskSpaceWebChartControl1.Series[1].Visible = true;
        }
      
        public void SetGraphForCPUsharepoint(string paramGraph, string serverName)
        {
            cpuWebChartControl.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.SetGraphForCPUsharepoint(paramGraph, serverName, ServerTypeId);

            Series series = null;
            series = new Series("LyncServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            cpuWebChartControl.Series.Add(series);

            ((XYDiagram)cpuWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)cpuWebChartControl.Diagram;
            seriesXY.AxisY.Title.Text = "CPU";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            cpuWebChartControl.Legend.Visible = false;

            // ((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)cpuWebChartControl.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //5/13/2014 NS commented out for VSPLUS-621
            //axis.GridSpacingAuto = false;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
            //axis.DateTimeOptions.FormatString = "dd/MM HH:mm";
            ((LineSeriesView)series.View).Color = Color.Blue;

            AxisBase axisy = ((XYDiagram)cpuWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;
            cpuWebChartControl.DataSource = dt;
            cpuWebChartControl.DataBind();

            //return dt;     
        }

        public void SetGraphForMemorysharepoint(string paramGraph, string serverName)
        {
            memWebChartControl.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.SetGraphForMemorysharepoint(paramGraph, serverName, ServerTypeId);

            Series series = null;
            series = new Series("LyncServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            memWebChartControl.Series.Add(series);

            ((XYDiagram)memWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)memWebChartControl.Diagram;
            seriesXY.AxisY.Title.Text = "Memory";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            memWebChartControl.Legend.Visible = false;

            //((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)memWebChartControl.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //5/13/2014 NS commented out for VSPLUS-621
            //axis.GridSpacingAuto = false;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;
            //axis.DateTimeOptions.FormatString = "dd/MM HH:mm";

            ((LineSeriesView)series.View).Color = Color.Blue;

            AxisBase axisy = ((XYDiagram)memWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;
            memWebChartControl.DataSource = dt;
            memWebChartControl.DataBind();
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

        protected void DominoserverTasksgrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "StatusSummary" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Scanned")
            {
                e.Cell.BackColor = System.Drawing.Color.Blue;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
            }
        }

        protected void NonmoniterdGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "StatusSummary" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "Not Scanned")
            {
                e.Cell.BackColor = System.Drawing.Color.Blue;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
            }   // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;
        }

        public void Fillmaintenancegrid()
        {
            Session["SLmaitservers"] = "";
            DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbyservername(Request.QueryString["Name"].ToString(), "", "", "", "");
            maintenancegrid.DataSource = dt;
            maintenancegrid.DataBind();
            Session["SLmaitservers"] = dt;
        }
        public void Fillmaintenancegridfromsession()
        {
            FillMaintenance();
            //if (Session["SLmaitservers"] != "" && Session["SLmaitservers"] != null)
            //{
            //    //DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.servermaintenance(System.DateTime.Now, 0, 0);
            //    maintenancegrid.DataSource = (DataTable)Session["SLmaitservers"];
            //    maintenancegrid.DataBind();
            //    //Session["maitservers"] = dt;
            //}
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            FillMaintenance();
        }
        public void FillMaintenance()
        {
            string fromdate = txtfromdate.Text;
            string todate = txttodate.Text;
            // string fromtime = //txtfromtime.Text;ASPxTimeEdit1

            string fromtime = ASPxTimeEdit1.Text;
            //  string totime = txttotime.Text;
            string totime = ASPxTimeEdit2.Text;
            if (txtfromdate.Text != "" && txttodate.Text != "")
            {
                if (Convert.ToDateTime(fromdate) > Convert.ToDateTime(todate))
                {
                    MsgPopupControl.ShowOnPageLoad = true;
                    ErrmsgLabel.Text = "From Date value should be less than To Date.";
                }

                else
                {

                    if ((ASPxTimeEdit1.Text != null && ASPxTimeEdit2.Text != null) && (ASPxTimeEdit1.Text != "" && ASPxTimeEdit2.Text != ""))
                    {
                        string fhour = fromtime.Substring(0, fromtime.IndexOf(":"));
                        string thour = totime.Substring(0, totime.IndexOf(":"));
                        int fhourInt = int.Parse(fhour);
                        int thourInt = int.Parse(thour);
                        string fminute = fromtime.Substring(3, 2);
                        string tminute = totime.Substring(3, 2);
                        int fminuteInt = int.Parse(fminute);
                        int tminuteInt = int.Parse(tminute);
                        if (fhourInt >= 24 || thourInt >= 24 || fminuteInt >= 60 || tminuteInt >= 60)
                        {
                            MsgPopupControl.ShowOnPageLoad = true;
                            ErrmsgLabel.Text = "Invalid hour/minute entry.";
                        }

                        else
                        {
                            Session["SLmaitservers"] = "";
                            DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbyservername(Request.QueryString["Name"].ToString(), fromdate, todate, fromtime, totime);
                            Session["SLmaitservers"] = dt;
                            maintenancegrid.DataSource = dt;
                            maintenancegrid.DataBind();
                        }
                    }
                }
            }
            else
            {
                if ((ASPxTimeEdit1.Text != null && ASPxTimeEdit2.Text != null) && (ASPxTimeEdit1.Text != "" && ASPxTimeEdit2.Text != ""))
                {
                    string fhour = fromtime.Substring(0, fromtime.IndexOf(":"));
                    string thour = totime.Substring(0, totime.IndexOf(":"));
                    int fhourInt = int.Parse(fhour);
                    int thourInt = int.Parse(thour);
                    string fminute = fromtime.Substring(3, 2);
                    string tminute = totime.Substring(3, 2);
                    int fminuteInt = int.Parse(fminute);
                    int tminuteInt = int.Parse(tminute);
                    if (fhourInt >= 24 || thourInt >= 24 || fminuteInt >= 60 || tminuteInt >= 60)
                    {
                        MsgPopupControl.ShowOnPageLoad = true;
                        ErrmsgLabel.Text = "Invalid hour/minute entry.";
                    }

                    else
                    {
                        Session["SLmaitservers"] = "";
                        DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbyservername(Request.QueryString["Name"].ToString(), fromdate, todate, fromtime, totime);
                        Session["SLmaitservers"] = dt;
                        maintenancegrid.DataSource = dt;
                        maintenancegrid.DataBind();
                    }
                }
                //1/4/2013 NS - when Clear button is clicked, the grid needs to return to its original state
                //which means return all records regardless of the dates/times
                else
                {
                    Session["SLmaitservers"] = "";
                    DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbyservername(Request.QueryString["Name"].ToString(), fromdate, todate, fromtime, totime);
                    Session["SLmaitservers"] = dt;
                    maintenancegrid.DataSource = dt;
                    maintenancegrid.DataBind();
                }
            }

        }
        protected void ClearButton_Click(object sender, EventArgs e)
        {
            txtfromdate.Text = "";
            txttodate.Text = "";
            ASPxTimeEdit1.Text = "";
            ASPxTimeEdit2.Text = "";
            FillMaintenance();
        }

        protected void OKButton_Click(object sender, EventArgs e)
        {

            MsgPopupControl.ShowOnPageLoad = false;

        }

        public void SetGraphForsharepointEnabledUsers(string paramGraph, string serverName)
        {
            usersWebChartControl.Series.Clear();
			DataTable dt = VSWebBL.DashboardBL.SharepointDetailsBL.Ins.SetGraphForsharepointEnabledUsers(paramGraph, serverName, ServerTypeId);

            Series series = null;
            series = new Series("DominoServer", ViewType.Line);
            series.Visible = true;
            series.ArgumentDataMember = dt.Columns["dtfrom"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["maxval"].ToString());
            usersWebChartControl.Series.Add(series);

            ((XYDiagram)usersWebChartControl.Diagram).PaneLayoutDirection = PaneLayoutDirection.Horizontal;

            XYDiagram seriesXY = (XYDiagram)usersWebChartControl.Diagram;
            seriesXY.AxisY.Title.Text = "SharePointEnabledUsers";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = "Date/Time";
            seriesXY.AxisX.Title.Visible = true;
            usersWebChartControl.Legend.Visible = false;

            //((SplineSeriesView)series.View).LineTensionPercent = 100;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Color = Color.White;

            AxisBase axis = ((XYDiagram)usersWebChartControl.Diagram).AxisX;
            //4/18/2014 NS commented out for VSPLUS-312
            //axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
            //axis.MinorCount = 15;
            //axis.GridSpacing = 0.5;
            axis.Range.SideMarginsEnabled = false;
            axis.GridLines.Visible = false;
            //axis.DateTimeOptions.Format = DateTimeFormat.Custom;

            ((LineSeriesView)series.View).Color = Color.Blue;

            //10/9/2013 NS moved the ds assignment prior to setting the grid spacing option
            usersWebChartControl.DataSource = dt;
            usersWebChartControl.DataBind();

            AxisBase axisy = ((XYDiagram)usersWebChartControl.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;
            axisy.Range.SideMarginsEnabled = true;
            axisy.GridLines.Visible = true;

            //10/8/2013 NS added
            seriesXY.AxisY.NumericOptions.Format = NumericFormat.Number;
            seriesXY.AxisY.NumericOptions.Precision = 0;
            seriesXY.AxisY.GridSpacingAuto = true;

            double min = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MinValue);
            double max = Convert.ToDouble(((XYDiagram)usersWebChartControl.Diagram).AxisY.Range.MaxValue);

            int gs = (int)((max - min) / 5);

            if (gs == 0)
            {
                gs = 1;
                seriesXY.AxisY.GridSpacingAuto = false;
                seriesXY.AxisY.GridSpacing = gs;
            }

        }

        protected void usersASPxRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGraphForsharepointEnabledUsers(usersASPxRadioButtonList.Value.ToString(), servernamelbl.Text);
        }

        public void FillAlertHistory()
        {
            DataTable AlertHistorytab = new DataTable();
            try
            {
                AlertHistorytab = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.GetAlertHistry(Request.QueryString["Name"].ToString());
                Session["AlertHistorytab"] = AlertHistorytab;
                AlertGridView.DataSource = AlertHistorytab;
                AlertGridView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }


        }

        public void FillAlertHistoryfromSession()
        {
            DataTable AlertHistorytab = new DataTable();
            try
            {

                AlertHistorytab = (DataTable)Session["AlertHistorytab"];
                AlertGridView.DataSource = AlertHistorytab;
                AlertGridView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
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

        protected void BackButton_Click(object sender, EventArgs e)
        {
            if (Session["BackURL"] != "" && Session["BackURL"] != null)
            {
                Response.Redirect(Session["BackURL"].ToString());
                Session["BackURL"] = "";

            }
        }     

        protected void OpenDBButton_Click(object sender, EventArgs e)
        {

        }

        public void UpdateButtonVisibility()
        {
            bool isadmin = false;
			if (Session["UserID"] != null)
			{
				DataTable sa = VSWebBL.SecurityBL.UsersBL.Ins.GetIsConsoleComm(Session["UserID"].ToString());
				if (sa.Rows.Count > 0)
				{
					if (sa.Rows[0]["IsConsoleComm"].ToString() == "True")
					{
						isadmin = true;
					}
				}
			}
            if (isadmin)
            {
               // CompactButton.Visible = true;
               // FixupButton.Visible = true;
              //  UpdallButton.Visible = true;
            }
            //1/2/2014 NS added
            bool isconfig = false;
			if (Session["UserID"] != null)
			{
				DataTable dsconfig = VSWebBL.SecurityBL.UsersBL.Ins.GetIsConfig(Session["UserID"].ToString());

				if (dsconfig.Rows.Count > 0)
				{
					if (dsconfig.Rows[0]["IsConfigurator"].ToString() == "True")
					{
						isconfig = true;
					}
				}
			}
            //11/3/2014 NS modified for VSPLUS-1142
            if (isconfig)
            {
                //EditInConfigButton.Visible = true;
                ASPxMenu1.Items[0].Items[2].Visible = true;
                ASPxMenu1.Items[0].Items[3].Visible = true;
            }
            else
            {
                ASPxMenu1.Items[0].Items[2].Visible = false;
                ASPxMenu1.Items[0].Items[3].Visible = false;
            }
        }   
      
        protected void ScanButton_Click(object sender, EventArgs e)
        {
            try
            {
                Status StatusObj = new Status();
                StatusObj.Name = Request.QueryString["Name"];
                StatusObj.Type = Request.QueryString["Type"];

                bool bl = VSWebBL.StatusBL.StatusTBL.Ins.UpdateforScan(StatusObj);

                if (Request.QueryString["Type"] == "Domino")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanDominoASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "Mail")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanMailServiceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "Network Device")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanNetworkDeviceASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "Sametime Server")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSametimeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "BES")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanBlackBerryASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "URL")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanURLASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "Exchange")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanExchangeASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
                else if (Request.QueryString["Type"] == "SharePoint")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("ScanSharePointASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
				else if (Request.QueryString["Type"] == "Skype for Business")
                {
					bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanvalue("LyncASAP", StatusObj.Name, VSWeb.Constants.Constants.SysString);
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void EditInConfigButton_Click(object sender, EventArgs e)
        {
            //1/2/2014 NS added
            string id = "";
            string name = "";
            Session["Submenu"] = "LyncServers";
            if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
            {
                id = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(Request.QueryString["Name"].ToString())).ToString();
                name = Request.QueryString["Name"].ToString();
                DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByName1(name);
                string Loc = dt.Rows[0]["Location"].ToString();
                string Cat = dt.Rows[0]["ServerType"].ToString();
                //servernamelbl.Text

                Response.Redirect("~/Configurator/SharepointServer.aspx?ID=" + id + "&Name=" + name + "&Cat=" + dt.Rows[0]["ServerType"].ToString() + "&Loc=" + dt.Rows[0]["Location"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void BtnApply_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> serverIDValues = new List<string>();
                List<string> servertypeIDValues = new List<string>();
                Random random = new Random((int)DateTime.Now.Ticks);
                //RandomString(5, random);
                string Name = servernamelbl.Text + "-Temp-" + DateTime.Now.ToLongDateString();
                string StartDate = DateTime.Now.ToShortDateString();
                string StartTime = DateTime.Now.ToShortTimeString();
                DateTime sdt = Convert.ToDateTime(StartDate);
                string Duration = TbDuration.Text;
                string EndDate = DateTime.Now.ToShortDateString();
                DateTime edt = Convert.ToDateTime(EndDate);
                string MaintType = "1";
                string MaintDaysList = "";
                string altime = DateTime.Now.ToShortTimeString();
                DateTime al = Convert.ToDateTime(altime);
                ASPxScheduler sh = new ASPxScheduler();
                Appointment apt = sh.Storage.CreateAppointment(AppointmentType.Pattern);
                Reminder r = apt.CreateNewReminder();

                //int min = Convert.ToInt32(MaintDurationTextBox.Text);
                int min = Convert.ToInt32(TbDuration.Text);
                r.AlertTime = al.AddMinutes(min);
                //3/24/2015 NS modified for DevExpress upgrade 14.2
                //ReminderXmlPersistenceHelper reminderHelper = new ReminderXmlPersistenceHelper(r, DateSavingType.LocalTime);
                ReminderXmlPersistenceHelper reminderHelper = new ReminderXmlPersistenceHelper(r);
                string rem = reminderHelper.ToXml().ToString();

                RecurrenceInfo reci = new RecurrenceInfo();
                reci.BeginUpdate();
                reci.AllDay = false;
                reci.Periodicity = 10;
                reci.Range = RecurrenceRange.EndByDate;
                reci.Start = sdt;
                reci.End = edt;
                reci.Duration = edt - sdt;
                reci.Type = RecurrenceType.Yearly;


                OccurrenceCalculator calc = OccurrenceCalculator.CreateInstance(reci);
                TimeInterval ttc = new TimeInterval(reci.Start, reci.End + new TimeSpan(1, 0, 0));


                var bcoll = calc.CalcOccurrences(ttc, apt);
                if (bcoll.Count != 0)
                {
                    reci.OccurrenceCount = bcoll.Count;
                }
                else
                {
                    reci.OccurrenceCount = 1;
                }
                reci.Range = RecurrenceRange.OccurrenceCount;
                reci.EndUpdate();
                string s = reci.ToXml();

                string EndDateIndicator = "";
                DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByName(servernamelbl.Text);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //VSPLUS-833:Suspend temporarily is not working correctly 
                    //19Jul14, Mukund, The below two were reversely assigned so suspend wasnt working. 
                    servertypeIDValues.Add(dt.Rows[0][2].ToString());
                    serverIDValues.Add(dt.Rows[0][0].ToString());
                }
                bool update = false;
                if (servertypeIDValues != null && servertypeIDValues.Count > 0)
                {
                    update = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.UpdateMaintenanceWindows(null, Name, StartDate, StartTime, Duration,
                          EndDate, MaintType, MaintDaysList, EndDateIndicator, serverIDValues, s, rem, 1, true, servertypeIDValues, "true", "1");
                }
                if (update == true)
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    SuccessMsg.InnerHtml = "Monitoring for " + servernamelbl.Text + " has been temporarily suspended for a duration of " + TbDuration.Text + " minutes."+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    SuccessMsg.Style.Value = "display: block";

                }
                else
                {
                    //10/3/2014 NS modified for VSPLUS-990
                    ErrorMsg.InnerHtml = "The Settings were NOT updated."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    ErrorMsg.Style.Value = "display: block";
                }
                ASPxPopupControl1.ShowOnPageLoad = false;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

       
        protected void maintenancegrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|maintenancegrid", maintenancegrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void AlertGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|AlertGridView", AlertGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void OutageGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerDetailsPage2|OutageGridView", OutageGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

    }
}

