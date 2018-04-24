using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace VSWebUI
{
    public partial class Problems : System.Web.UI.Page
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
                FillProblemsGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "Problems|ProblemsGridView")
                        {
                            ProblemsGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillMailFilesfromSession();
            }
        }

        public void FillProblemsGrid()
        {

            DataTable probtab = VSWebBL.DashboardBL.mailFileBL.Ins.FillProblemsGrid();
            probtab.PrimaryKey = new DataColumn[] { probtab.Columns["ID"] };
            ProblemsGridView.DataSource = probtab;
            ProblemsGridView.DataBind();
            Session["ProblemsFiles"] = probtab;
            
        }
        public void FillMailFilesfromSession()
        {

            DataTable probtab = new DataTable();
            if( Session["ProblemsFiles"]!="" &&  Session["ProblemsFiles"] !=null)
             probtab =   (DataTable) Session["ProblemsFiles"] ;
            //probtab.PrimaryKey = new DataColumn[] { probtab.Columns["ID"] };
            ProblemsGridView.DataSource = probtab;
            ProblemsGridView.DataBind();
         }

        protected void ProblemsGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "OK" || e.CellValue.ToString() == "Scanning"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }

            else if (e.DataColumn.FieldName == "Status" && (e.CellValue.ToString() == "Not Responding" ||e.CellValue.ToString() == "Error" ) )
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "Not Scanned")
            {

                e.Cell.BackColor = System.Drawing.Color.FromName("#87CEEB");
                e.Cell.ForeColor = System.Drawing.Color.Black;
            }
            else if (e.DataColumn.FieldName == "Status" && e.CellValue.ToString() == "disabled")
            {
                e.Cell.BackColor = System.Drawing.Color.Gray;
                // e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Status")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                // e.DataColumn.GroupFooterCellStyle.ForeColor =
            }
        }

        protected void ProblemsGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("Problems|ProblemsGridView", ProblemsGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
      
    }
}