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
    public partial class CloudHealthPage : System.Web.UI.Page
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

		string Type = "";
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Session["cloudServerList"] = null;
				
				Session["cloudIssues"] = null;
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
				
				FillServersListGrid();
				
				FillcloudIssuesGrid();

			

				StatusWebChart.DataSource = statuslist;
				StatusWebChart.Series[0].DataSource = statuslist;
				StatusWebChart.Series[0].ArgumentDataMember = statuslist.Columns[0].ToString();
				StatusWebChart.Series[0].ValueDataMembers.AddRange(statuslist.Columns[1].ToString());
				StatusWebChart.Series[0].Visible = true;
				StatusWebChart.DataBind();
				dt = (DataTable)Session["MailStatus"];
				
			
				
		
			}
			else
			{
				FillServersListGridfromSession();

                FillcloudIssuesGridFromSession();
				
			}
			string url = HttpContext.Current.Request.Url.AbsoluteUri;
			Session["BackURL"] = url;
            if (Session["UserPreferences"] != null)
            {
                DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                foreach (DataRow dr in UserPreferences.Rows)
                {
                    if (dr[1].ToString() == "CloudHealthPage|cloudHealthGridView")
                    {
                        cloudHealthGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        
                    }


                    if (dr[1].ToString() == "CloudHealthPage|cloudIssuesGrid")
                    {
                        cloudIssuesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                }
            }
           

		}
		
		public void FillServersListGrid()
		{
			string cloudList = "";
			DataTable dt = new DataTable();
			dt = VSWebBL.DashboardBL.CloudDetailsBL.Ins.GetcloudServerDetails();
			Session["cloudServerList"] = dt;
            cloudList = "0";
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
                cloudList = dt.Rows.Count.ToString();
			}
			cloudHealthGridView.DataSource = dt;
            cloudHealthGridView.DataBind();
            //DataRow rolerow = srvroles.Rows.Add();
            //rolerow["Role"] = "Domino";
            //rolerow["RoleCount"] = Convert.ToInt32(DominoList);
		}
		public void FillServersListGridfromSession()
		{
			DataTable DataServers = new DataTable();
			try
			{
				if (Session["cloudServerList"] != null && Session["cloudServerList"] != "")
				{
					DataServers = Session["cloudServerList"] as DataTable;
				}
				if (DataServers.Rows.Count > 0)
				{
					DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ID"] };

				}

                cloudHealthGridView.DataSource = DataServers;
                cloudHealthGridView.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}



        private void FillcloudIssuesGrid()
		{
			try
			{
				
				DataTable StatusTable12 = VSWebBL.DashboardBL.CloudDetailsBL.Ins.GetIssuesTasks();

				Session["cloudIssues"] = StatusTable12;
				cloudIssuesGrid.DataSource = StatusTable12;
                cloudIssuesGrid.DataBind();
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
		public void FillcloudIssuesGridFromSession()
		{
			try
			{
                if (Session["cloudIssues"] != null && Session["cloudIssues"] != "")
				{
                    dt = Session["cloudIssues"] as DataTable;
				}

                cloudIssuesGrid.DataSource = dt;
                cloudIssuesGrid.DataBind();

			}
			//    DataTable dt = Session["DominoIssues"] as DataTable;
				
			//}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
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
		}
        protected void cloudHealthGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
			
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


        protected void cloudHealthGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("CloudHealthPage|cloudHealthGridView", cloudHealthGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void cloudIssuesGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("CloudHealthPage|cloudIssuesGrid", cloudIssuesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void cloudIssuesGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{


		}
		
    }
}