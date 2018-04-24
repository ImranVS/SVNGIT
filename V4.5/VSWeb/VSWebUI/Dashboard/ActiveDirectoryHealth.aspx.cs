using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using DevExpress.XtraCharts;

namespace VSWebUI.Dashboard
{
    public partial class ActiveDirectoryHealth : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{
			//Mukund 05Nov13, Create an event handler for the master page's contentCallEvent event
			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{
			//Mukund 05Nov13, This Method will be Called from Timer Click from Master page

		}
       // int Sid = 19;
       // string ServerName = "";
        static string value = null;
        DataTable dtactive = new DataTable();
        DataTable dthealth = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {

            this.ResponseWebChartControl.Width = new Unit(Convert.ToInt32(chartWidth.Value));
            GraphforResponseTime();
            
            if (!IsPostBack && !IsCallback)
            {
                FillADMembersGridView();
                

                HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("Body1");
                body.Attributes.Add("onload", "DoCallback()");
                body.Attributes.Add("onResize", "Resized()");

            }
			else
			{

				FillADMembersGridSession();

			}
		
        }
		public void FillADMembersGridSession()
		{
			if (Session["ADMembers"] != "" && Session["ADMembers"] != null)
			{
				DAGMembersGridView.DataSource = (DataTable)Session["ADMembers"];
				DAGMembersGridView.DataBind();
			}
			
		}

        public void GraphforResponseTime()
        {
            ResponseWebChartControl.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.ActiveDirectoryServerDetailsBL.Ins.GetResponseTime();

            Series series = new Series("usercount", ViewType.Bar);

            series.ArgumentDataMember = dt.Columns["Server"].ToString();
            //10/30/2013 NS added - point labels on series
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["ResponseTime"].ToString());
            ResponseWebChartControl.Series.Add(series);

            ResponseWebChartControl.Legend.Visible = false;

            ((SideBySideBarSeriesView)series.View).ColorEach = true;

            //AxisBase axisx = ((XYDiagram)deviceTypeWebChart.Diagram).AxisX;

            XYDiagram seriesXY = (XYDiagram)ResponseWebChartControl.Diagram;
            // seriesXY.AxisX.Title.Text = "Milliseconds";
            // seriesXY.AxisX.Title.Visible = true;
            ((DevExpress.XtraCharts.XYDiagram)ResponseWebChartControl.Diagram).Rotated = true;
            //11/16/2013 NS modified the AllowRotate option to false, otherwise the server names were unreadable
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisY.Title.Text = "Milliseconds";
            seriesXY.AxisY.Title.Visible = true;
            seriesXY.AxisX.Title.Text = " Servers";
            seriesXY.AxisX.Title.Visible = true;

            AxisBase axisy = ((XYDiagram)ResponseWebChartControl.Diagram).AxisY;
            //4/18/2014 NS modified for VSPLUS-312
            axisy.Range.AlwaysShowZeroLevel = true;
            
            //2/6/2013 NS modified chart height calculations based on the number of rows
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count < 4)
                {
                    ResponseWebChartControl.Height = 200;
                }
                else
                {
                    if (dt.Rows.Count >= 4 && dt.Rows.Count < 10)
                    {
                        ResponseWebChartControl.Height = ((dt.Rows.Count) * 50) + 20;
                    }
                    else
                    {
                        if (dt.Rows.Count >= 10 && dt.Rows.Count < 100)
                        {
                            ResponseWebChartControl.Height = ((dt.Rows.Count) * 40) + 20;
                        }
                        else
                        {
                            if (dt.Rows.Count >= 100)
                            {
                                ResponseWebChartControl.Height = ((dt.Rows.Count) * 20) + 20;
                            }
                        }
                    }
                }
            }
            //ResponseWebChartControl.Width = new Unit(1000);
            ResponseWebChartControl.DataSource = dt;
            ResponseWebChartControl.DataBind();

        }

     
       
        protected void DAGMembersGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName != "ServerName" && (e.CellValue.ToString() == "Pass" || e.CellValue.ToString() == "Passed"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
            //7/29/2014 NS modified
            //else if (e.DataColumn.FieldName != "ServerName" && (e.CellValue.ToString() == "Fail" || e.CellValue.ToString() == "Failed"))
            else if (e.DataColumn.FieldName != "ServerName" && ((e.CellValue.ToString()).ToUpper()).Contains("FAIL"))
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            string status = "";
            status = e.GetValue("Status").ToString();

            if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning" || e.CellValue.ToString() == "Telnet"))
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
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.FromName("#D0D0D0");
                //e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Issue")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor = System.Drawing.Color.Yellow;

            }
        }



        private void FillADMembersGridView()
        {
            try
            {
                DataTable DSTaskSettingsDataTable = new DataTable();
                DSTaskSettingsDataTable = VSWebBL.DashboardBL.ActiveDirectoryServerDetailsBL.Ins.GetactivedirectoryMembers();
                Session["ADMembers"] = DSTaskSettingsDataTable;
                DAGMembersGridView.DataSource = DSTaskSettingsDataTable;
                DAGMembersGridView.DataBind();
				
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }


		protected void DAGMembersGridView_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				if (DAGMembersGridView.Selection.Count > 0)
				{
					System.Collections.Generic.List<object> ID = DAGMembersGridView.GetSelectedFieldValues("ID");

					DataTable dt = (VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByID(Convert.ToInt32(ID[0].ToString())));

					if (dt.Rows.Count > 0)
					{
						Response.Redirect("~/Dashboard/ActiveDirectoryServerDetailsPage3.aspx?Name=" + dt.Rows[0]["ServerName"].ToString() + "&Type=" + dt.Rows[0]["Type"].ToString() + "&Status=" + dt.Rows[0]["Status"].ToString() + "&LastDate=" + dt.Rows[0]["LastUpdate"].ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						Context.ApplicationInstance.CompleteRequest();
					}

				}
			}
			catch (Exception)
			{ }
		}
        //7/9/2015 NS added for VSPLUS-1973
        protected void ResponseWebChartControl_BoundDataChanged(object sender, EventArgs e)
        {
            UI uiobj = new UI();
            uiobj.RecalibrateChartAxes(ResponseWebChartControl.Diagram);
        }
    }
}