using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Configuration;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using System.Drawing;
using DevExpress.XtraCharts;

namespace VSWebUI.Dashboard
{
    public partial class DominoServerHealthPage: System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        DataTable srvroles = new DataTable();
        DataTable statuslist = new DataTable();
		DataTable dt = new DataTable();
        //10/21/2015 NS added for VSPLUS-2253
        DataTable versionlist = new DataTable();
        DataTable oslist = new DataTable();

		string Type = "";
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Session["DominoServerList"] = null;
				Session["TravelerServerList"] = null;
				Session["SametimeServerList"] = null;
				Session["DominoIssues"] = null;
				Session["MailStatus"] = null;
				dt.Columns.Add("Mail");
				DataColumn Mail = new DataColumn("Value", typeof(int));
				dt.Columns.Add(Mail);

				srvroles.Columns.Add("Role");
				DataColumn dc = new DataColumn("RoleCount", typeof(int));
				srvroles.Columns.Add(dc);

				DataColumn statuscol = new DataColumn("Status", typeof(string));
				statuslist.Columns.Add(statuscol);
				statuscol = new DataColumn("StatusCount", typeof(int));
				statuslist.Columns.Add(statuscol);

                //10/21/2015 NS added for VSPLUS-2253
                DataColumn versioncol = new DataColumn("DominoVersion", typeof(string));
                versionlist.Columns.Add(versioncol);
                versioncol = new DataColumn("DominoVersionCount", typeof(int));
                versionlist.Columns.Add(versioncol);

                DataColumn oscol = new DataColumn("OperatingSystem", typeof(string));
                oslist.Columns.Add(oscol);
                oscol = new DataColumn("OperatingSystemCount", typeof(int));
                oslist.Columns.Add(oscol);

				FillServersListGrid();
				FillTravelersListGrid();
				FillSametimeListGrid();
				FillMonitoredGrid();

				SetChartForMail(Type);

				FillDominoIssuesGrid();

                //10/23/2015 NS added for VSPLUS-2253
                FillSysInfo();

				SrvRolesWebChart.DataSource = srvroles;
				SrvRolesWebChart.Series[0].DataSource = srvroles;
				SrvRolesWebChart.Series[0].ArgumentDataMember = srvroles.Columns[0].ToString();
				SrvRolesWebChart.Series[0].ValueDataMembers.AddRange(srvroles.Columns[1].ToString());
				SrvRolesWebChart.Series[0].Visible = true;
				SrvRolesWebChart.DataBind();

				StatusWebChart.DataSource = statuslist;
				StatusWebChart.Series[0].DataSource = statuslist;
				StatusWebChart.Series[0].ArgumentDataMember = statuslist.Columns[0].ToString();
				StatusWebChart.Series[0].ValueDataMembers.AddRange(statuslist.Columns[1].ToString());
				StatusWebChart.Series[0].Visible = true;
				StatusWebChart.DataBind();

                //10/21/2015 NS added for VSPLUS-2253
                VersionWebChart.DataSource = versionlist;
                VersionWebChart.Series[0].DataSource = versionlist;
                VersionWebChart.Series[0].ArgumentDataMember = versionlist.Columns[0].ToString();
                VersionWebChart.Series[0].ValueDataMembers.AddRange(versionlist.Columns[1].ToString());
                VersionWebChart.Series[0].Visible = true;
                VersionWebChart.DataBind();

                OSWebChart.DataSource = oslist;
                OSWebChart.Series[0].DataSource = oslist;
                OSWebChart.Series[0].ArgumentDataMember = oslist.Columns[0].ToString();
                OSWebChart.Series[0].ValueDataMembers.AddRange(oslist.Columns[1].ToString());
                OSWebChart.Series[0].Visible = true;
                OSWebChart.DataBind();

				dt = (DataTable)Session["MailStatus"];
				MailWebChart.DataSource = dt;
				MailWebChart.Series[0].DataSource = dt;
				MailWebChart.Series[0].ArgumentDataMember = dt.Columns[1].ToString();
				MailWebChart.Series[0].ValueDataMembers.AddRange(dt.Columns[2].ToString());
				MailWebChart.Series[0].Visible = true;
				MailWebChart.DataBind();
				
		
			}
			else
			{
				FillServersListGridfromSession();
				FillTravelerListGridfromSession();
				FillSametimeListGridfromSession();
				FillDominoIssuesGridFromSession();
                //10/23/2015 NS added for VSPLUS-2253
                FillSysInfoFromSession();
			}
			string url = HttpContext.Current.Request.Url.AbsoluteUri;
			Session["BackURL"] = url;
            if (Session["UserPreferences"] != null)
            {
                DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                foreach (DataRow dr in UserPreferences.Rows)
                {
                    if (dr[1].ToString() == "DominoServerHealthPage|DominoHealthGridView")
                    {
                        DominoHealthGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                    if (dr[1].ToString() == "DominoServerHealthPage|TravelerGridView")
                    {
                        TravelerGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                    if (dr[1].ToString() == "DominoServerHealthPage|SameTimeGridView")
                    {
                        SameTimeGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                    if (dr[1].ToString() == "DominoServerHealthPage|DominoIssuesGrid")
                    {
                        DominoIssuesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                }
            }
           

		}
		
		public void FillServersListGrid()
		{
			string DominoList = "";
			DataTable dt = new DataTable();
			dt = VSWebBL.DashboardBL.DominoServerHealthBLL.Ins.GetDominoServerDetails();
			Session["DominoServerList"] = dt;
			DominoList = "0";
			DataRow[] foundRows;
			DataRow statusrow;
			statusrow = statuslist.NewRow();
            //10/21/2015 NS added for VSPLUS-2253
            DataRow versionrow;
            versionrow = versionlist.NewRow();
            DataRow osrow;
            osrow = oslist.NewRow();

			if (dt != null && dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (statuslist.Rows.Count > 0)
					{
						foundRows = statuslist.Select("Status = '" + dt.Rows[i]["Status"].ToString() + "'");
						if (foundRows.Length == 0)
						{
							statusrow = statuslist.Rows.Add();
							statusrow["Status"] = dt.Rows[i]["Status"].ToString();
							statusrow["StatusCount"] = 1;
							statusrow = statuslist.NewRow();
						}
						else
						{
							statusrow = foundRows[0];
							statusrow["StatusCount"] = Convert.ToInt32(statusrow["StatusCount"].ToString()) + 1;
						}
					}
					else
					{
						statusrow = statuslist.Rows.Add();
						statusrow["Status"] = dt.Rows[i]["Status"].ToString();
						statusrow["StatusCount"] = 1;
						statusrow = statuslist.NewRow();
					}
                    //10/21/2015 NS added for VSPLUS-2253
                    if (versionlist.Rows.Count > 0)
                    {
                        foundRows = versionlist.Select("DominoVersion = '" + dt.Rows[i]["DominoVersion"].ToString() + "'");
                        if (foundRows.Length == 0)
                        {
                            versionrow = versionlist.Rows.Add();
                            versionrow["DominoVersion"] = dt.Rows[i]["DominoVersion"].ToString();
                            versionrow["DominoVersionCount"] = 1;
                            versionrow = versionlist.NewRow();
                        }
                        else
                        {
                            versionrow = foundRows[0];
                            versionrow["DominoVersionCount"] = Convert.ToInt32(versionrow["DominoVersionCount"].ToString()) + 1;
                        }
                    }
                    else
                    {
                        versionrow = versionlist.Rows.Add();
                        versionrow["DominoVersion"] = dt.Rows[i]["DominoVersion"].ToString();
                        versionrow["DominoVersionCount"] = 1;
                        versionrow = versionlist.NewRow();
                    }
                    if (oslist.Rows.Count > 0)
                    {
                        foundRows = oslist.Select("OperatingSystem = '" + dt.Rows[i]["OperatingSystem"].ToString() + "'");
                        if (foundRows.Length == 0)
                        {
                            osrow = oslist.Rows.Add();
                            osrow["OperatingSystem"] = dt.Rows[i]["OperatingSystem"].ToString();
                            osrow["OperatingSystemCount"] = 1;
                            osrow = oslist.NewRow();
                        }
                        else
                        {
                            osrow = foundRows[0];
                            osrow["OperatingSystemCount"] = Convert.ToInt32(osrow["OperatingSystemCount"].ToString()) + 1;
                        }
                    }
                    else
                    {
                        osrow = oslist.Rows.Add();
                        osrow["OperatingSystem"] = dt.Rows[i]["OperatingSystem"].ToString();
                        osrow["OperatingSystemCount"] = 1;
                        osrow = oslist.NewRow();
                    }
				}
				DominoList = dt.Rows.Count.ToString();
			}
			DominoHealthGridView.DataSource = dt;
			DominoHealthGridView.DataBind();
			DataRow rolerow = srvroles.Rows.Add();
			rolerow["Role"] = "Domino";
			rolerow["RoleCount"] = Convert.ToInt32(DominoList);
		}
		public void FillServersListGridfromSession()
		{
			DataTable DataServers = new DataTable();
			try
			{
				if (Session["DominoServerList"] != null && Session["DominoServerList"] != "")
				{
					DataServers = Session["DominoServerList"] as DataTable;
				}
				if (DataServers.Rows.Count > 0)
				{
					DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ID"] };

				}

				DominoHealthGridView.DataSource = DataServers;
				DominoHealthGridView.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		public void FillTravelersListGrid()
		{
			string TravelersList = "";
			DataTable dt = new DataTable();
			dt = VSWebBL.DashboardBL.DominoServerHealthBLL.Ins.GetTravelerServerDetails();
			Session["TravelerServerList"] = dt;
			TravelersList = "0";
			DataRow[] foundRows;
			DataRow statusrow;
			statusrow = statuslist.NewRow();
			if (dt != null && dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (statuslist.Rows.Count > 0)
					{
						foundRows = statuslist.Select("Status = '" + dt.Rows[i]["Status"].ToString() + "'");
						if (foundRows.Length == 0)
						{
							statusrow = statuslist.Rows.Add();
							statusrow["Status"] = dt.Rows[i]["Status"].ToString();
							statusrow["StatusCount"] = 1;
							statusrow = statuslist.NewRow();
						}
						else
						{
							statusrow = foundRows[0];
							statusrow["StatusCount"] = Convert.ToInt32(statusrow["StatusCount"].ToString()) + 1;
						}
					}
					else
					{
						statusrow = statuslist.Rows.Add();
						statusrow["Status"] = dt.Rows[i]["Status"].ToString();
						statusrow["StatusCount"] = 1;
						statusrow = statuslist.NewRow();
					}
				}
				TravelersList = dt.Rows.Count.ToString();
			}
			TravelerGridView.DataSource = dt;
			TravelerGridView.DataBind();
			DataRow rolerow = srvroles.Rows.Add();
			rolerow["Role"] = "Traveler";
			rolerow["RoleCount"] = Convert.ToInt32(TravelersList);
		}
		public void FillTravelerListGridfromSession()
		{
			DataTable DataServers = new DataTable();
			try
			{
				if (Session["TravelerServerList"] != null && Session["TravelerServerList"] != "")
				{
					DataServers = Session["TravelerServerList"] as DataTable;
				}
				if (DataServers.Rows.Count > 0)
				{
					DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ID"] };

				}

				TravelerGridView.DataSource = DataServers;
				TravelerGridView.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		public void FillSametimeListGrid()
		{
			string SametimeList = "";
			DataTable dt = new DataTable();
			dt = VSWebBL.DashboardBL.DominoServerHealthBLL.Ins.GetSametimeServerDetails();
			Session["SametimeServerList"] = dt;
			SametimeList = "0";
			DataRow[] foundRows;
			DataRow statusrow;
			statusrow = statuslist.NewRow();
			if (dt != null && dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (statuslist.Rows.Count > 0)
					{
						foundRows = statuslist.Select("Status = '" + dt.Rows[i]["Status"].ToString() + "'");
						if (foundRows.Length == 0)
						{
							statusrow = statuslist.Rows.Add();
							statusrow["Status"] = dt.Rows[i]["Status"].ToString();
							statusrow["StatusCount"] = 1;
							statusrow = statuslist.NewRow();
						}
						else
						{
							statusrow = foundRows[0];
							statusrow["StatusCount"] = Convert.ToInt32(statusrow["StatusCount"].ToString()) + 1;
						}
					}
					else
					{
						statusrow = statuslist.Rows.Add();
						statusrow["Status"] = dt.Rows[i]["Status"].ToString();
						statusrow["StatusCount"] = 1;
						statusrow = statuslist.NewRow();
					}
				}
				SametimeList = dt.Rows.Count.ToString();
			}
			SameTimeGridView.DataSource = dt;
			SameTimeGridView.DataBind();
			DataRow rolerow = srvroles.Rows.Add();
			rolerow["Role"] = "Sametime";
			rolerow["RoleCount"] = Convert.ToInt32(SametimeList);
		}
		public void FillSametimeListGridfromSession()
		{
			DataTable DataServers = new DataTable();
			try
			{
				if (Session["SametimeServerList"] != null && Session["SametimeServerList"] != "")
				{
					DataServers = Session["SametimeServerList"] as DataTable;
				}
				if (DataServers.Rows.Count > 0)
				{
					DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ID"] };

				}

				SameTimeGridView.DataSource = DataServers;
				SameTimeGridView.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		private void FillDominoIssuesGrid()
		{
			try
			{
				
				DataTable StatusTable12 = VSWebBL.DashboardBL.DominoServerHealthBLL.Ins.GetIssuesTasks();

				Session["DominoIssues"] = StatusTable12;
				DominoIssuesGrid.DataSource = StatusTable12;
				DominoIssuesGrid.DataBind();
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
		public void FillDominoIssuesGridFromSession()
		{
			try
			{
				if (Session["DominoIssues"] != null && Session["DominoIssues"] != "")
				{
					dt = Session["DominoIssues"] as DataTable;
				}
				
				DominoIssuesGrid.DataSource = dt;
				DominoIssuesGrid.DataBind();

			}
			//    DataTable dt = Session["DominoIssues"] as DataTable;
				
			//}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}
		public void SetChartForMail(string serverName)
		{
			string MailList = "";

			DataTable dt = new DataTable();
			dt = VSWebBL.DashboardBL.DominoServerHealthBLL.Ins.GetMailChart(Type);
			Session["MailStatus"] = dt;
			MailList = "0";
			if (dt != null && dt.Rows.Count > 0)
			{
				MailList = dt.Rows.Count.ToString();
			}
			MailWebChart.DataSource = dt;
			MailWebChart.DataBind();

		}
		public void FillMonitoredGrid()
		{

			try
			{
				DataTable StatusTable = VSWebBL.DashboardBL.DominoServerHealthBLL.Ins.GetMoniteredTasks(servernamelbl.Text);

				if (StatusTable.Rows.Count <= 0)
				{

				}
				Session["MoniterdeServerTasks"] = StatusTable;
				MaintanceTasksgrid.DataSource = StatusTable;
				MaintanceTasksgrid.DataBind();


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

		protected void StatusWebChart_CustomDrawSeriesPoint(object sender, DevExpress.XtraCharts.CustomDrawSeriesPointEventArgs e)
		{
			Pie3DDrawOptions drawOptions = e.SeriesDrawOptions as Pie3DDrawOptions;
			Pie3DDrawOptions legendOptions = e.LegendDrawOptions as Pie3DDrawOptions;
			drawOptions.FillStyle.FillMode = FillMode3D.Solid;
			legendOptions.FillStyle.FillMode = FillMode3D.Solid;
			if (e.SeriesPoint.Argument == "OK")
			{
				drawOptions.Color = System.Drawing.Color.FromArgb(0, 128, 0);
				legendOptions.Color = System.Drawing.Color.FromArgb(0, 128, 0);
			}
			else if (e.SeriesPoint.Argument == "Issue")
			{
				drawOptions.Color = System.Drawing.Color.FromArgb(242, 242, 0);
				legendOptions.Color = System.Drawing.Color.FromArgb(242, 242, 0);
			}
			else if (e.SeriesPoint.Argument == "Not Responding")
			{
				drawOptions.Color = System.Drawing.Color.FromArgb(253, 0, 0);
				legendOptions.Color = System.Drawing.Color.FromArgb(253, 0, 0);
			}
			else if (e.SeriesPoint.Argument == "Maintenance")
			{
				drawOptions.Color = System.Drawing.Color.FromArgb(80, 80, 80);
				legendOptions.Color = System.Drawing.Color.FromArgb(80, 80, 80);
			}
            else if (e.SeriesPoint.Argument == "Scanning")
            {
                drawOptions.Color = System.Drawing.Color.LightBlue;
                legendOptions.Color = System.Drawing.Color.LightBlue;
            }
		}
		protected void DominoHealthGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
			
		{
            if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Not Responding")
            {
                //e.DataColumn.CellStyle.CssClass = "GridCss3";
                e.Cell.CssClass = "GridCss3";
            }

            //6/10/2013 NS modified - added Telnet status
            if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "OK" || e.GetValue("Status").ToString() == "Scanning")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
                //e.DataColumn.CellStyle.CssClass = "GridCss3";
            }
            else if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Not Scanned")
            {

                e.Cell.BackColor = System.Drawing.Color.FromName("#87CEEB");
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            //3/1/2013 NS modified the value of text below - lower case disabled did not match the returned status with capital D
            else if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.FromName("#C8C8C8");
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Maintenance")
            {
                e.Cell.BackColor = System.Drawing.Color.LightBlue;

            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            }

		}
		protected void SameTimeGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{

            if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Not Responding")
            {
                //e.DataColumn.CellStyle.CssClass = "GridCss3";
                e.Cell.CssClass = "GridCss3";
            }

            //6/10/2013 NS modified - added Telnet status
            if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "OK" || e.GetValue("Status").ToString() == "Scanning")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
                //e.DataColumn.CellStyle.CssClass = "GridCss3";
            }
            else if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Not Scanned")
            {

                e.Cell.BackColor = System.Drawing.Color.FromName("#87CEEB");
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            //3/1/2013 NS modified the value of text below - lower case disabled did not match the returned status with capital D
            else if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.FromName("#C8C8C8");
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Maintenance")
            {
                e.Cell.BackColor = System.Drawing.Color.LightBlue;

            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            }

		}
		protected void TravelerGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Not Responding")
            {
                //e.DataColumn.CellStyle.CssClass = "GridCss3";
                e.Cell.CssClass = "GridCss3";
            }

            //6/10/2013 NS modified - added Telnet status
            if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "OK" || e.GetValue("Status").ToString() == "Scanning")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
                //e.DataColumn.CellStyle.CssClass = "GridCss3";
            }
            else if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Not Scanned")
            {

                e.Cell.BackColor = System.Drawing.Color.FromName("#87CEEB");
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            //3/1/2013 NS modified the value of text below - lower case disabled did not match the returned status with capital D
            else if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.FromName("#C8C8C8");
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && e.GetValue("Status").ToString() == "Maintenance")
            {
                e.Cell.BackColor = System.Drawing.Color.LightBlue;

            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            }
		}
		protected void MaintanceTasksgrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{

            if (e.DataColumn.FieldName == "StatusSummary" && e.GetValue("StatusSummary").ToString() == "Not Responding")
            {
                //e.DataColumn.CellStyle.CssClass = "GridCss3";
                e.Cell.CssClass = "GridCss3";
            }

            //6/10/2013 NS modified - added Telnet status
            if (e.DataColumn.FieldName == "StatusSummary" && e.GetValue("StatusSummary").ToString() == "OK" || e.GetValue("StatusSummary").ToString() == "Scanning")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "StatusSummary" && e.GetValue("StatusSummary").ToString() == "Not Responding")
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
                //e.DataColumn.CellStyle.CssClass = "GridCss3";
            }
            else if (e.DataColumn.FieldName == "StatusSummary" && e.GetValue("StatusSummary").ToString() == "Not Scanned")
            {

                e.Cell.BackColor = System.Drawing.Color.FromName("#87CEEB");
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            //3/1/2013 NS modified the value of text below - lower case disabled did not match the returned status with capital D
            else if (e.DataColumn.FieldName == "StatusSummary" && e.GetValue("StatusSummary").ToString() == "Disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.FromName("#C8C8C8");
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "StatusSummary" && e.GetValue("StatusSummary").ToString() == "Maintenance")
            {
                e.Cell.BackColor = System.Drawing.Color.LightBlue;

            }
            else if (e.DataColumn.FieldName == "StatusSummary")
            {
                //e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            }

		}
        protected void DominoHealthGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerHealthPage|DominoHealthGridView", DominoHealthGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void TravelerGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerHealthPage|TravelerGridView", TravelerGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void SameTimeGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerHealthPage|SameTimeGridView", SameTimeGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void DominoIssuesGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerHealthPage|DominoIssuesGrid", DominoIssuesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
		protected void DominoIssues_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{


		}
        //10/23/2015 NS added for VSPLUS-2253
        private void FillSysInfo()
        {
            try
            {
                DataTable dt = VSWebBL.DashboardBL.DominoServerDetailsBL.Ins.GetServerDetailsData("");
                Session["SysInfo"] = dt;
                SysInfoGrid.DataSource = dt;
                SysInfoGrid.DataBind();
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

        private void FillSysInfoFromSession()
        {
            DataTable dt = new DataTable();
            try
            {
                if (Session["SysInfo"] != null && Session["SysInfo"] != "")
                {
                    dt = Session["SysInfo"] as DataTable;
                }
                if (dt.Rows.Count > 0)
                {
                    dt.PrimaryKey = new DataColumn[] { dt.Columns["ID"] };
                }
                SysInfoGrid.DataSource = dt;
                SysInfoGrid.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void SysInfoGrid_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DominoServerHealthPage|SysInfoGrid", SysInfoGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
		
    }
}