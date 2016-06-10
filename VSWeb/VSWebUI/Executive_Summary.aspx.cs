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

namespace VSWebUI
{
    public partial class Executive_Summary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
    }
}