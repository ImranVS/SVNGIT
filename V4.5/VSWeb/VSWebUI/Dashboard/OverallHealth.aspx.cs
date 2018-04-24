using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebDO;
using VSWebBL;
using System.Data;

namespace VSWebUI.Dashboard
{
    public partial class OverallHealth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            AssignStatusbox();
        }
        protected void AssignStatusbox()
        {
            DataTable dtRow1, dtRow2, dtRow3, dtRow4; DataRow[] results;       

            DataTable dtOverall = VSWebBL.DashboardBL.DashboardBL.Ins.GetAllData();
            GetAllData(dtOverall, StatusBox1,1);

            DataTable dtIndl = VSWebBL.DashboardBL.DashboardBL.Ins.GetIndlData();
            
            //Notes Database
            dtRow1 = dtIndl.Clone();
            results = dtIndl.Select("type='Notes Database'");
            foreach (DataRow dr in results) dtRow1.ImportRow(dr);
            GetAllData(dtRow1, StatusBox2,2);
            StatusBox2.Title = "Notes Database";

            //Domino Server
            dtRow1 = dtIndl.Clone();
            results = dtIndl.Select("type='Domino Server'");
            foreach (DataRow dr in results) dtRow1.ImportRow(dr);
            GetAllData(dtRow1, StatusBox3,2);
            StatusBox3.Title = "Domino Server";

            //Mail Service
            dtRow1 = dtIndl.Clone();
            results = dtIndl.Select("type='Mail Service'");
            foreach (DataRow dr in results) dtRow1.ImportRow(dr);
            GetAllData(dtRow1, StatusBox4,2);
            StatusBox4.Title = "Mail Service";

        }

        protected void GetAllData(DataTable dtOverall, StatusBox objStatusBox,int iCol )
        {
            DataTable dtRow1, dtRow2, dtRow3, dtRow4; DataRow[] results;            
            if (dtOverall.Rows.Count > 0)
            {              
                
                dtRow1 = dtOverall.Clone();
                results = dtOverall.Select("Status='Issue'");
                foreach (DataRow dr in results) dtRow1.ImportRow(dr);

                if (dtRow1.Rows.Count > 0)
                {
                    objStatusBox.Label31Text = dtRow1.Rows[0][iCol].ToString();
                }

                dtRow2 = dtOverall.Clone();
                results = dtOverall.Select("Status='Maintenance'");
                foreach (DataRow dr in results) dtRow2.ImportRow(dr);

                if (dtRow2.Rows.Count > 0)
                {
                    objStatusBox.Label41Text = dtRow2.Rows[0][iCol].ToString();
                }

                dtRow3 = dtOverall.Clone();
                results = dtOverall.Select("Status='Not Responding'");
                foreach (DataRow dr in results) dtRow3.ImportRow(dr);

                if (dtRow3.Rows.Count > 0)
                {
                    objStatusBox.Label11Text = dtRow3.Rows[0][iCol].ToString();
                }

                dtRow4 = dtOverall.Clone();
                results = dtOverall.Select("Status='OK'");
                foreach (DataRow dr in results) dtRow4.ImportRow(dr);

                if (dtRow3.Rows.Count > 0)
                {
                    objStatusBox.Label21Text = dtRow4.Rows[0][iCol].ToString();
                }
            }
        }

    }
}