using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data;

namespace VSWebUI.Dashboard
{
    public partial class DBsByTemplate : System.Web.UI.Page
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
                FillByTemplateGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "DBsByTemplate|ByTemplateGridView")
                        {
                            ByTemplateGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillByTemplateGridFromSession();
            }
        }

        public void FillByTemplateGrid()
        {
            DataTable dt = VSWebBL.DashboardBL.mailFileBL.Ins.FillDBByTemplateGrid();
            dt.PrimaryKey = new DataColumn[] { dt.Columns["ID"] };
            ByTemplateGridView.DataSource = dt;
            ByTemplateGridView.DataBind();
            ((GridViewDataColumn)ByTemplateGridView.Columns["DesignTemplateName"]).GroupBy();
            Session["DBsByTemplate"] = dt;
        }

        private void FillByTemplateGridFromSession()
        {
            try
            {
                DataTable dt = new DataTable();
                if (Session["DBsByTemplate"] != "" && Session["DBsByTemplate"] != null)
                    dt = (DataTable)Session["DBsByTemplate"];
                if (dt.Rows.Count > 0)
                {
                    ByTemplateGridView.DataSource = dt;
                    ByTemplateGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void btnCollapse_Click(object sender, EventArgs e)
        {
            if (btnCollapse.Text == "Collapse All Rows")
            {
                ByTemplateGridView.CollapseAll();
                btnCollapse.Image.Url = "~/images/icons/add.png";
                btnCollapse.Text = "Expand All Rows";
            }
            else
            {
                ByTemplateGridView.ExpandAll();
                btnCollapse.Image.Url = "~/images/icons/forbidden.png";
                btnCollapse.Text = "Collapse All Rows";

            }
        }

        protected void btnExpand_Click(object sender, EventArgs e)
        {

        }

        protected void ByTemplateGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DBsByTemplate|ByTemplateGridView", ByTemplateGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}