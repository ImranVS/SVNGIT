using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Drawing;

namespace VSWebUI.Dashboard
{
    public partial class CPUMemoryHealth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "CPUMemGridData|CPUMemGrid")
                        {
                            CPUMemGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            SetGrid();
        }

        public void SetGrid()
        {
            try
            {
                DataTable dt = VSWebBL.DashboardBL.KeyMetricsBL.Ins.GetCPUMemoryHealth();
                Session["CPUMemGridData"] = dt;
                CPUMemGrid.DataSource = dt;
                CPUMemGrid.DataBind();
                ((GridViewDataColumn)CPUMemGrid.Columns["servertype"]).GroupBy();
                //int rowIndex = CPUMemGrid.FindVisibleIndexByKeyValue("ID");
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void CPUMemGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            int memVal = 0;
            int memTVal = 0;
            int cpuVal = 0;
            int cpuTVal = 0;
            if (e.DataColumn.FieldName == "memdisp")
            {
                memVal = Convert.ToInt32(e.GetValue("memory"));
                memTVal = Convert.ToInt32(e.GetValue("Memory_Threshold"));
                e.Cell.ForeColor = (memVal >= 0 && memVal < (memTVal * 0.4) ? Color.Green : (memVal >= (memTVal * 0.4) && memVal < memTVal ? Color.Orange : Color.Red));
            }
            if (e.DataColumn.FieldName == "cpudisp")
            {
                cpuVal = Convert.ToInt32(e.GetValue("cpu"));
                cpuTVal = Convert.ToInt32(e.GetValue("CPU_Threshold"));
                e.Cell.ForeColor = (cpuVal >= 0 && cpuVal < (cpuTVal * 0.4) ? Color.Green : (cpuVal >= (cpuTVal * 0.4) && cpuVal < cpuTVal ? Color.Orange : Color.Red));
            }
        }

        protected void CPUMemGrid_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("CPUMemGridData|CPUMemGrid", CPUMemGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }
}