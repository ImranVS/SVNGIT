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
    public partial class WebForm11 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetServers();
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
                for (int i = 0; i < ASPxDataView1.Items.Count; i++)
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