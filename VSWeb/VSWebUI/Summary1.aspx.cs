using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
namespace VSWebUI
{
    public partial class Summary1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetServers();
        }
        protected void AssignStatusbox()
        {
            DataTable dtOverall = VSWebBL.DashboardBL.DashboardBL.Ins.GetAllData("null", "Location");
            GetAllData(dtOverall, StatusBox1, 1);
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
        protected void GetServers()
        {
            try
            {
                DataTable dt = VSWebBL.DashboardBL.DashboardBL.Ins.GetServerType("");
                ASPxDataView1.DataSource = dt;
                ASPxDataView1.DataBind();
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ASPxDataView1_DataBound(object sender, EventArgs e)
        {
            if (ASPxDataView1.Items.Count > 0)
            {
                DataTable dt = VSWebBL.DashboardBL.DashboardBL.Ins.GetServerType("");
                for(int i=0;i<ASPxDataView1.Items.Count ;i++)
                {
                   // ASPxLabel lbl = (ASPxLabel)ASPxDataView1.FindItemControl("ASPxLabel1", ASPxDataView1.Items[i]);
                ASPxGridView grid = (ASPxGridView)ASPxDataView1.FindItemControl("ASPxGridView1", ASPxDataView1.Items[i]);
                grid.DataSource = VSWebBL.DashboardBL.DashboardBL.Ins.GetStatusGrid(dt.Rows[i]["Type"].ToString());
                grid.DataBind();

                    
                }
            }
        }

        protected void ASPxGridView1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            string status = e.GetValue("Status").ToString();
            if (e.DataColumn.FieldName == "")//|| e.DataColumn.Caption="")
            {

                if (status == "OK" || status == "Scanning")
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                }
                else if (status == "Not Responding")
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                }
                else if (status == "Not Scanned")
                {
                    e.Cell.BackColor = System.Drawing.Color.FromName("#87CEEB");
                }
                else if (status == "disabled")
                {
                    e.Cell.BackColor = System.Drawing.Color.Gray;
                }
                else if (status == "Maintenance")
                {
                    e.Cell.BackColor = System.Drawing.Color.LightBlue;
                }
                else if (e.DataColumn.FieldName == "")
                {
                    e.Cell.BackColor = System.Drawing.Color.Yellow;


                }

            }

        }
    }
}