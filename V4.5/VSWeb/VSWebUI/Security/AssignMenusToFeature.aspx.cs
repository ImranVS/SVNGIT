using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;


namespace VSWebUI.Security
{
    public partial class AssignMenusToFeature : System.Web.UI.Page
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
            try
            {
                if (!IsPostBack)
                {
                    FillFeatureComboBox();
                 }

                //Navigator
                if (Session["DataConfigurator"] == null)
                {
                    DataTable NavigatorTree = VSWebBL.SecurityBL.MenusBL.Ins.MenuTree("Configurator");
                    Session["DataConfigurator"] = NavigatorTree;
                    NavigatorTree = VSWebBL.SecurityBL.MenusBL.Ins.MenuTree("Dashboard");
                    Session["DataDashboard"] = NavigatorTree;
                }
             
                ConfiguratorMenus.DataSource = (DataTable)Session["DataConfigurator"];
                ConfiguratorMenus.DataBind();

                DashboardMenus.DataSource = (DataTable)Session["DataDashboard"];
                DashboardMenus.DataBind();

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        private void FillFeatureComboBox()
        {
            DataTable FeaturesDataTable = VSWebBL.SecurityBL.MenusBL.Ins.GetFeatures();
            FeaturesComboBox.DataSource = FeaturesDataTable;
            FeaturesComboBox.TextField = "Name";
            FeaturesComboBox.ValueField = "Name";
            FeaturesComboBox.DataBind();
        }
        protected void FeaturesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selValue;
            selValue = FeaturesComboBox.SelectedItem.Text;
            DataTable ConfigDt = VSWebBL.SecurityBL.MenusBL.Ins.SelectedMenuTree(selValue, "Configurator");
            DataTable DashBrdDT= VSWebBL.SecurityBL.MenusBL.Ins.SelectedMenuTree(selValue, "Dashboard");
            ConfiguratorMenus.UnselectAll();
            DashboardMenus.UnselectAll();
            SelectTree(ConfiguratorMenus, ConfigDt);
            SelectTree(DashboardMenus, DashBrdDT);

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
             
                    if ((Convert.ToInt32(dtSel.Rows[i]["ID"]) == Convert.ToInt32(node.GetValue("ID"))))
                    {
                        //5/8/2014 NS modified
                        if (node.ParentNode.Key != "" || !node.HasChildren)
                        {
                            node.Selected = true;
                        }
                    }

                }
                iterator.Reset();
            }
        }

        protected void AssignMenuButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (FeaturesComboBox.Text != "")
                {
                    DataTable dt = GetSelectedMenu(ConfiguratorMenus);
                    DataTable dt1 = GetSelectedMenu(DashboardMenus);
                    if (dt.Rows.Count > 0 || dt1.Rows.Count > 0)
                    {
                        bool update = VSWebBL.SecurityBL.MenusBL.Ins.DeleteFeatureMenus(FeaturesComboBox.Text);
                        if (dt.Rows.Count > 0)
                        {
                            update = VSWebBL.SecurityBL.MenusBL.Ins.InsertFeatureMenus(FeaturesComboBox.Text, dt);
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            update = VSWebBL.SecurityBL.MenusBL.Ins.InsertFeatureMenus(FeaturesComboBox.Text, dt1);
                        }
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
                else
                {
                    successDiv.Style.Value = "display: none";
                    errorDiv.Style.Value = "display: block";
                    //10/6/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "Please select Feature in order to proceed."+
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
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        private DataTable GetSelectedMenu(ASPxTreeList treelist)
        {

            DataTable dtSel = new DataTable();
            //5/8/2014 NS added
            DataColumn[] keys = new DataColumn[1];
            DataColumn column;
            try
            {
                dtSel.Columns.Add("ID");
                //5/8/2014 NS added
                column = dtSel.Columns["ID"];
                keys[0] = column;
                dtSel.PrimaryKey = keys;

                TreeListNodeIterator iterator = treelist.CreateNodeIterator();
                TreeListNode node;
                //5/8/2014 NS added
                TreeListNode parent;
                DataRow founddr;

                TreeListColumn columnId = treelist.Columns["ID"];
             
                while (true)
                {

                    node = iterator.GetNext();

                    if (node == null) break;
                        if (node.Selected)
                        {
                            DataRow dr = dtSel.NewRow();

                            dr["ID"] = node.GetValue("ID");
                            dtSel.Rows.Add(dr);
                            //5/8/2014 NS added
                            parent = node.ParentNode;
                            if (parent != null)
                            {
                                if (parent.Key.ToString() != "")
                                {
                                    founddr = dtSel.Rows.Find(parent.GetValue("ID"));
                                    if (founddr == null)
                                    {
                                        dr = dtSel.NewRow();
                                        dr["ID"] = parent.GetValue("ID");
                                        dtSel.Rows.Add(dr);
                                    }
                                }
                            }
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

        

    }
}