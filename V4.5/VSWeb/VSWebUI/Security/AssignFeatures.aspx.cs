using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web.ASPxTreeList;

namespace VSWebUI.Security
{
    public partial class AssignFeatures : System.Web.UI.Page
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
                if (Session["Features"] == null)
                {
					DataTable NavigatorTree = VSWebBL.SecurityBL.MenusBL.Ins.GetFeatures();
                    Session["Features"] = NavigatorTree;

                }
            }
            ConfiguratorMenus.DataSource = (DataTable)Session["Features"];
            ConfiguratorMenus.DataBind();

            if (!IsPostBack)
            {
                DataTable ConfigDt = VSWebBL.SecurityBL.MenusBL.Ins.GetSelectedFeatures();
                SelectTree(ConfiguratorMenus, ConfigDt);
            }
        }
        protected void SelectTree(ASPxTreeList treelist, DataTable dtSel)
        {
            TreeListNodeIterator iterator = treelist.CreateNodeIterator();
            TreeListNode node;
            for (int i = 0; i < dtSel.Rows.Count; i++)
            {
                while (true)
                {
                    node = iterator.GetNext();
                    if (node == null) break;

                    if (dtSel.Rows[i]["Name"].ToString() == node.GetValue("Name").ToString())
                    {
                        node.Selected = true;
                    }
                }
                iterator.Reset();
            }
        }
        protected void AssignMenuButton_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = GetSelectedMenu(ConfiguratorMenus);
                if (dt.Rows.Count > 0)
                {// 3/15/2016 Durga Addded for VSPLUS-2717
                    string CommonFeature = VSWebBL.SecurityBL.MenusBL.Ins.GetCommonFeature().Rows[0]["Name"].ToString();

                    dt.Rows.Add(CommonFeature);

                    bool update = VSWebBL.SecurityBL.MenusBL.Ins.InsertFeatures(dt);
                    successDiv.Style.Value = "display: block";
                    errorDiv.Style.Value = "display: none";
                }
                else
                {
                    successDiv.Style.Value = "display: none";
                    errorDiv.Style.Value = "display: block";
                    //10/6/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "Please select at least one item in order to proceed."+
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                }
            }
            catch (Exception ex)
            {

                successDiv.Style.Value = "display: none";
                errorDiv.Style.Value = "display: block";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = ex.Message+
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
            }

        }

        private DataTable GetSelectedMenu(ASPxTreeList treelist)
        {

            DataTable dtSel = new DataTable();
            try
            {
                dtSel.Columns.Add("Name");


                TreeListNodeIterator iterator = treelist.CreateNodeIterator();
                TreeListNode node;

                TreeListColumn columnId = treelist.Columns["Name"];

                while (true)
                {

                    node = iterator.GetNext();

                    if (node == null) break;
                    if (node.Selected)
                    {
                        DataRow dr = dtSel.NewRow();

                        dr["Name"] = node.GetValue("Name");
                        dtSel.Rows.Add(dr);
                    }


                }

            }
            catch (Exception ex)
            {
                //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
                //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                //    ", Error: " + ex.ToString());
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

            return dtSel;

        }

		protected void ResetMenuButton_Click(object sender, EventArgs e)
		{
			if (Request.UrlReferrer != null)
				Response.Redirect("~/Security/AssignFeatures.aspx", false);
			
		}
		protected void CancelButton_Click(object sender, EventArgs e)
		{

			Response.Redirect("~/Configurator/Default.aspx", false);
			Context.ApplicationInstance.CompleteRequest();
		}
    }
}