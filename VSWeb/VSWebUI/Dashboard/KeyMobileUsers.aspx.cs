using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;

namespace VSWebUI.Dashboard
{
    public partial class KeyMobileUsers : System.Web.UI.Page
    {
        public int maxmin = 0;
        public int minmin = 0;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
        }

        private void Master_ButtonClick(object sender, EventArgs e)
        {
            FillMobileDevicesGrid();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillMobileDevicesGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "KeyMobileDevicesGrid|KeyMobileDevicesGridView")
                        {
                            KeyMobileDevicesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                FillMobileDevicesGridfromSession();
            }
        }

        private void FillMobileDevicesGrid()
        {
            try
            {
                DataTable dt1 = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.GetKeyUserDevices("");
                if (dt1.Rows.Count > 0)
                {
                    maxmin = Convert.ToInt32(dt1.Rows[0]["LastSyncMin"].ToString());
                    minmin = Convert.ToInt32(dt1.Rows[dt1.Rows.Count-1]["LastSyncMin"].ToString());
                }
                KeyMobileDevicesGrid.DataSource = dt1;
                Session["KeyMobileDevicesGrid"] = dt1;
                KeyMobileDevicesGrid.DataBind();
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillMobileDevicesGridfromSession()
        {
            try
            {
                DataTable dt = new DataTable();
                if ((Session["KeyMobileDevicesGrid"] != null))
                {
                    dt = (DataTable)Session["KeyMobileDevicesGrid"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                    if (dt.Rows.Count > 0)
                    {
                        KeyMobileDevicesGrid.DataSource = dt;
                        KeyMobileDevicesGrid.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void SyncWebChart_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            WebChartControl chartControl = (WebChartControl)sender;
            
            DevExpress.Web.GridViewDataItemTemplateContainer gridc = (DevExpress.Web.GridViewDataItemTemplateContainer)chartControl.Parent;
            string deviceid = DataBinder.Eval(gridc.DataItem, "DeviceID").ToString();

            dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.GetKeyUserDevices(deviceid);
            chartControl.DataSource = dt;
            chartControl.Series["MinSinceSync"].DataSource = dt;
            chartControl.Series["MinSinceSync"].ArgumentDataMember = dt.Columns["DeviceID"].ToString();
            chartControl.Series["MinSinceSync"].ValueDataMembers.AddRange(dt.Columns["LastSyncMin"].ToString());
            chartControl.Series["MinSinceSync"].Visible = true;
            XYDiagram seriesXY = (XYDiagram)chartControl.Diagram;
            seriesXY.AxisY.Range.MinValue = minmin;
            seriesXY.AxisY.Range.MaxValue = maxmin;
            chartControl.DataBind();
        }

        protected void KeyMobileDevicesGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            string status = e.GetValue("Status").ToString();
            if (e.DataColumn.FieldName == "Status")
            {
                if (status == "Overdue")
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
            }
        }
    }
}