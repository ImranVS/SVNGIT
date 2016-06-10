using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Diagnostics;

using System.Xml;
using System.Text;
using System.Data;
using DevExpress.Web;


namespace VSWebUI.Configurator
{
    public partial class GetAssemblyInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //ReadXmlFile(Server.MapPath("~/Assemblies.xml"));
            getAssemblyVersionInfo();
            getDatabaseVersionInfo();
            //2/11/2016 NS added for VSPLUS-2594
            getBuildInfo();
            if (!IsPostBack)
            {
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "GetAssemblyInfo|AssemblyGridView")
                        {
                            AssemblyGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }

        }

        protected void ReadXmlFile(string xmlFile)
        {
            try
            {

                DataTable newt = new DataTable();
                newt.ReadXml(xmlFile);
                newt.Columns.Add("BuildDate");

                string FilePath = "";
                for (int i = 0; i < newt.Rows.Count; i++)
                {
                    try
                    {
                        if (newt.Rows[i]["FileArea"].ToString() == "VSWebUI")
                        {
                            FilePath = Server.MapPath("~/bin/");
                        }
                        else if (newt.Rows[i]["FileArea"].ToString() == "Services")
                        {
                            string Adv = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Log Files Path");

                            FilePath = Adv.ToUpper().Substring(0, Adv.ToUpper().IndexOf("LOG_FILES"));
                        }
                        //Assembly assembly = Assembly.LoadFrom(FilePath + newt.Rows[i]["AssemblyName"].ToString());
                        //Version ver = assembly.GetName().Version;   
                        //FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

                        //AssemblyName assembly = AssemblyName.GetAssemblyName(FilePath + newt.Rows[i]["AssemblyName"].ToString());
                        Version ver = AssemblyName.GetAssemblyName(FilePath + newt.Rows[i]["AssemblyName"].ToString()).Version;
                        newt.Rows[i]["AssemblyVersion"] = ver.ToString();
                        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(FilePath + newt.Rows[i]["AssemblyName"].ToString());
                        string fversion = fvi.ProductVersion;
                        newt.Rows[i]["ProductVersion"] = fversion;
                        newt.Rows[i]["BuildDate"] = System.IO.File.GetLastWriteTime(FilePath + newt.Rows[i]["AssemblyName"].ToString());

                    }
                    catch (Exception ex)
                    {
                        //6/27/2014 NS added for VSPLUS-634
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    }
                }
                AssemblyGridView.DataSource = newt;
                AssemblyGridView.DataBind();
                //1/9/2014 NS added styles to the grid
                AssemblyGridView.Styles.Cell.CssClass = "GridCss";
                AssemblyGridView.Styles.Header.CssClass = "GridCssHeader";
                AssemblyGridView.Columns[3].Visible = false;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void AssemblyGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("GetAssemblyInfo|AssemblyGridView", AssemblyGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        private void getAssemblyVersionInfo()
        {
            DataTable AssemblyVersionInfo = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.getAssemblyVersionInfo();
            AssemblyGridView.DataSource = AssemblyVersionInfo;
            AssemblyGridView.DataBind();
            AssemblyGridView.Styles.Cell.CssClass = "GridCss";
            AssemblyGridView.Styles.Header.CssClass = "GridCssHeader";
            AssemblyGridView.KeyFieldName = "NodeName";
		
            //grid.SettingsBehavior.AllowFocusedRow = true;
            AssemblyGridView.SettingsBehavior.AllowDragDrop =true;
			((GridViewDataColumn)AssemblyGridView.Columns[0]).GroupBy();
		
            AssemblyGridView.Styles.InlineEditCell.CssClass = "GridCssHeader";
            
        
        }
        private void getDatabaseVersionInfo()
        {
            DataTable DBVersionInfo = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetDatabaseVersionInfo();
            DatabaseGridView.DataSource = DBVersionInfo;
            DatabaseGridView.DataBind();
            //1/9/2014 NS added styles to the grid
            DatabaseGridView.Styles.Cell.CssClass = "GridCss";
            DatabaseGridView.Styles.Header.CssClass = "GridCssHeader";
        }

        //2/11/2016 NS added for VSPLUS-2594
        private void getBuildInfo()
        {
            string vsVersion = "VSx.x_SPRT_xx_RCxx";
            DataTable DBVersionInfo = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetDatabaseVersionInfo("VS_BUILD");
            if (DBVersionInfo.Rows.Count > 0)
            {
                vsVersion = DBVersionInfo.Rows[0]["VALUE"].ToString();
            }
            lblVersion.InnerText = "Build: " + vsVersion;
        }
    }
}