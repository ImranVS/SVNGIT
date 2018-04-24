using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DevExpress.XtraPrinting;
using System.Net.Mime;
using System.Data;
using DevExpress.Web;
using DevExpress.XtraGrid;
using System.Drawing;
using DevExpress.XtraCharts;
using System.Drawing;
using VSWebBL;
using DevExpress.XtraCharts.Web;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Xml;
using System.Text;
using System.Windows.Forms;
using DevExpress.Web.ASPxScheduler;

namespace VSWebUI.Dashboard
{
	public partial class LatencyTest : System.Web.UI.Page
	{
		int NetworkLatencyID = 0;
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		DataTable stdata;

		string NetworkLatencyId = "";
	
		
		protected void Page_Load(object sender, EventArgs e)
		{
			
			//Request.QueryString["
			if (!IsPostBack)
			{
				initComboBox();
				fillgrid();


			}
			if (TestNameComboBox.SelectedIndex > -1)
			{
				ASPxMenu1.Visible = true;
			}
			else
			{
				ASPxMenu1.Visible = false;
			}

		}

		public void initComboBox()
		{
			DataTable dt = new DataTable();
			dt = VSWebBL.DashboardBL.LatencyTestBL.Ins.getNetworkLatencyTestNames();
			TestNameComboBox.DataSource = dt;
			TestNameComboBox.TextField = "TestName";
			TestNameComboBox.ValueField = "NetworkLatencyId";
			TestNameComboBox.DataBind();
			if (Request.QueryString["ID"] != null)
			TestNameComboBox.SelectedItem = TestNameComboBox.Items.FindByValue(Request.QueryString["ID"].ToString());
			//TestNameComboBox.SelectedIndex.
		}


		public void fillgrid()
		{
			if (Request.QueryString["ID"] == null)
				return;
			//DataTable stdata = VSWebBL.DashboardBL.LatencyTestBL.Ins.getNetworkLatencyHeatMap(TestNameComboBox.SelectedItem.Value.ToString());
			DataTable stdata = VSWebBL.DashboardBL.LatencyTestBL.Ins.getNetworkLatencyHeatMap(Request.QueryString["ID"].ToString());
			Session["sessiondata"] = stdata;
			LatencyHeatGridView.DataSource = stdata;
			LatencyHeatGridView.DataBind();
			

			for (int i = 0; i < LatencyHeatGridView.Columns.Count; i++)
			{
                if (LatencyHeatGridView.Columns[i].ToString().Contains("Latency Red Threshold"))
				{
					LatencyHeatGridView.Columns[i].Visible = false;
				}
                else if (LatencyHeatGridView.Columns[i].ToString().Contains("Latency Yellow Threshold"))
				{
					LatencyHeatGridView.Columns[i].Visible = false;
				}
			}
		}
	

        protected void LatencyHeatGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
			
            if (e.DataColumn.FieldName != "From Server")
            {
                ASPxGridView request = sender as ASPxGridView;
                string sourceserver = request.GetRowValues(e.VisibleIndex, "From Server").ToString();
                //e.Cell.Attributes.Add("onclick", "onCellClick('" + sourceserver + "', '" + e.DataColumn.FieldName + "')");
                string destval = e.CellValue.ToString();
                string YellowTh = request.GetRowValues(e.VisibleIndex, "LatencyYellowThreshold").ToString();
                string RedTh = request.GetRowValues(e.VisibleIndex, "LatencyRedThreshold").ToString();

                e.Cell.HorizontalAlign = HorizontalAlign.Center;
                if (destval == "--")
                {
                    e.Cell.BackColor = Color.Gray;
                    e.Cell.ForeColor = Color.White;
                }
                else
                {
                    if (YellowTh.ToString() != "")
                    {
                        if (double.Parse(destval) < double.Parse(YellowTh))
                        {
                            e.Cell.BackColor = Color.Green;
                            e.Cell.ForeColor = Color.White;
                        }
                        if (RedTh.ToString() != "")
                        {
                            if (double.Parse(destval) >= double.Parse(YellowTh) && double.Parse(destval) < double.Parse(RedTh))
                            {
                                e.Cell.BackColor = Color.Yellow;
                                e.Cell.ForeColor = Color.Black;
                            }
                        }

                    }
                    if (RedTh.ToString() != "")
                    {
                        if (double.Parse(destval) >= double.Parse(RedTh))
                        {
                            e.Cell.BackColor = Color.Red;
                            e.Cell.ForeColor = Color.White;
                        }
                    }
                }
            }
        }

		protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{
			//if (Session["NetworkLatencyID"] == null || Session["NetworkLatencyID"] == "")
			//{
			//    Response.Redirect("~/Configurator/NetworkLatencyServers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			//    Context.ApplicationInstance.CompleteRequest();
			//}
			//else
			//{
			//    Response.Redirect("~/Configurator/NetworkLatencyTestServers.aspx?Key=" + Convert.ToInt32(Session["NetworkLatencyID"].ToString()), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			//    Context.ApplicationInstance.CompleteRequest();
			//}



			Response.Redirect("~/Configurator/NetworkLatencyTestServers.aspx?Key=" + Convert.ToInt32(TestNameComboBox.Value.ToString()), false);
				Context.ApplicationInstance.CompleteRequest();
			
			


		}

		protected void TestnameCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
			ASPxMenu1.Visible = true;
			//NetworkLatencyId = TestNameComboBox.SelectedItem.Value.ToString();
			//fillgrid();
			Session["NetworkLatencyID"] = Convert.ToInt32(TestNameComboBox.Value.ToString());
			Response.Redirect("~/Dashboard/LatencyTest.aspx?ID=" + TestNameComboBox.SelectedItem.Value.ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
        }

		
	}
}