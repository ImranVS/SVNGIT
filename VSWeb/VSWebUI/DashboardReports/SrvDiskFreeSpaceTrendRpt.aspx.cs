using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Globalization;
using System.IO;

namespace VSWebUI.DashboardReports
{
    public partial class SrvDiskFreeSpaceTrendRpt : System.Web.UI.Page
    {
        string selectedType = "";
        protected void dde_Init(object sender, EventArgs e)
        {
            dde.DropDownWindowTemplate = new DateTemplate();
        }

        public class DateTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {

                container.Controls.Add(new LiteralControl("<table  class='tab'"));
                container.Controls.Add(new LiteralControl("<tr>"));

                container.Controls.Add(new LiteralControl("<td class='cell' align='left'>"));
                ASPxButton btPrev = new ASPxButton();
                btPrev.ID = "btPrev";
                container.Controls.Add(btPrev);
                btPrev.EnableDefaultAppearance = false;
                btPrev.CssClass = "buttonMonth";
                btPrev.Width = 10;
                btPrev.ClientSideEvents.Click = "OnPrevClick";
                btPrev.AutoPostBack = false;
                btPrev.Text = "<";
                container.Controls.Add(new LiteralControl("</td>"));

                container.Controls.Add(new LiteralControl("<td class='cell' style='text-align: center'>"));
                ASPxLabel label = new ASPxLabel();
                label.ID = "YearLabel";
                container.Controls.Add(label);
                label.Text = DateTime.Now.Year.ToString();
                label.ClientInstanceName = "lblYear";
                container.Controls.Add(new LiteralControl("</td>"));

                container.Controls.Add(new LiteralControl("<td class='cell' align='right'>"));
                ASPxButton btNext = new ASPxButton();
                btNext.ID = "btNext";
                container.Controls.Add(btNext);
                btNext.AutoPostBack = false;
                btNext.EnableDefaultAppearance = false;
                btNext.CssClass = "buttonMonth";
                btNext.Width = 10;
                btNext.Text = ">";
                btNext.ClientSideEvents.Click = "OnNextClick";
                container.Controls.Add(new LiteralControl("</td>"));

                container.Controls.Add(new LiteralControl("</tr>"));
                container.Controls.Add(new LiteralControl("</table>"));

                container.Controls.Add(new LiteralControl("<table  class='tab'>"));
                int k = 1;
                for (int i = 0; i < 3; i++)
                {
                    container.Controls.Add(new LiteralControl("<tr>"));
                    for (int j = 0; j < 4; j++)
                    {
                        container.Controls.Add(new LiteralControl("<td class='cell'>"));
                        ASPxButton button = new ASPxButton();
                        button.ID = "btn#" + k;
                        container.Controls.Add(button);
                        button.AutoPostBack = false;
                        button.Width = 50;
                        button.EnableDefaultAppearance = false;
                        button.CssClass = "buttonMonth";
                        button.FocusRectBorder.BorderWidth = 1;
                        button.ClientSideEvents.Click = "OnClick";
                        button.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(k);
                        container.Controls.Add(new LiteralControl("</td>"));
                        k++;
                    }
                    container.Controls.Add(new LiteralControl("</tr>"));
                }
                container.Controls.Add(new LiteralControl("</table>"));

                /*
                container.Controls.Add(new LiteralControl("<table  class='tab'>"));
                ASPxButton btOk = new ASPxButton();
                btOk.ID = "BtOk";
                container.Controls.Add(btOk);
                btOk.AutoPostBack = false;
                btOk.Text = "OK";
                btOk.Width = Unit.Percentage(98);
                btOk.ClientSideEvents.Click = "OnOkClick";
                container.Controls.Add(new LiteralControl("</table>"));
                */
            }
        }
        //1/27/2016 NS added for VSPLUS-2533
        private static SrvDiskFreeSpaceTrendRpt _self = new SrvDiskFreeSpaceTrendRpt();
        public static SrvDiskFreeSpaceTrendRpt Ins
        {
            get
            {
                return _self;
            }
        }
        DashboardReports.SrvDiskFreeSpaceTrendXtraRpt report;
        public DashboardReports.SrvDiskFreeSpaceTrendXtraRpt GetRpt()
        {
            string date;
            string srvName = "";
            string srvType = "";
            date = DateTime.Now.ToString();
            DateTime dt = Convert.ToDateTime(date);
            dt.AddDays(-1);
            try
            {
                report = new DashboardReports.SrvDiskFreeSpaceTrendXtraRpt();
                report.Parameters["DateM"].Value = dt.Month;
                report.Parameters["DateY"].Value = dt.Year;
                report.Parameters["ServerName"].Value = srvName;
                report.Parameters["ServerType"].Value = srvType;
                report.SetReport(report);
            }
            catch (Exception ex)
            {
                WriteServiceHistoryEntry(DateTime.Now.ToString() + " The following error has occurred in GetRpt: " + ex.Message);
            }
            return report;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Report();
            if (!IsPostBack)
            {
                fillcombo("");
                fillservertypelist();
            }
            else
            {
                fillcombo(selectedType);
            }
        }

        public void Report()
        {
            string selectedServer = "";
            /*
            if (this.ServerListFilterComboBox.SelectedIndex >= 0)
            {
                selectedServer = this.ServerListFilterComboBox.SelectedItem.Value.ToString();
            }
             */
            if (ServerListFilterListBox.SelectedItems.Count > 0)
            {
                selectedServer = "";
                for (int i = 0; i < ServerListFilterListBox.SelectedItems.Count; i++)
                {
                    selectedServer += "'" + ServerListFilterListBox.SelectedItems[i].Text + "'" + ",";
                }
                try
                {
                    selectedServer = selectedServer.Substring(0, selectedServer.Length - 1);
                }
                catch
                {
                    selectedServer = "";     // throw ex; 
                }
                finally { }
            }
            //2/4/2015 NS added for VSPLUS-1370
            if (this.ServerTypeFilterListBox.SelectedItems.Count > 0)
            {
                selectedType = "";
                for (int i = 0; i < this.ServerTypeFilterListBox.SelectedItems.Count; i++)
                {
                    selectedType += "'" + this.ServerTypeFilterListBox.SelectedItems[i].Text + "'" + ",";
                }
                try
                {
                    selectedType = selectedType.Substring(0, selectedType.Length - 1);
                }
                catch
                {
                    selectedType = "";     // throw ex; 
                }
                finally { }
            }
            string date;
            //10/23/2013 NS modified - added jQuery month/year control
            /*
            if (this.DateParamEdit.Text == "")
            {
                date = DateTime.Now.ToString();
                this.DateParamEdit.Date = Convert.ToDateTime(date);
            }
            else
            {
                date = this.DateParamEdit.Value.ToString();
            }
             */
            /*
            if (dde.Text == "")
            {
                date = DateTime.Now.ToString();
            }
            else
            {
                //date = startDate.Value.ToString();
                date = dde.Text;
            }
             */
            if (startDate.Value.ToString() == "")
            {
                date = DateTime.Now.ToString();
            }
            else
            {
                date = startDate.Value.ToString();
            }
            DateTime dt = Convert.ToDateTime(date);
            DashboardReports.SrvDiskFreeSpaceTrendXtraRpt report = new DashboardReports.SrvDiskFreeSpaceTrendXtraRpt();
            report.Parameters["DateM"].Value = dt.Month;
            report.Parameters["DateY"].Value = dt.Year; 
            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            string strMonthName = mfi.GetMonthName(dt.Month).ToString() + ", " + dt.Year.ToString();
            report.Parameters["MonthYear"].Value = strMonthName;
            report.Parameters["ServerName"].Value = selectedServer;
            //2/4/2015 NS added for VSPLUS-1370
            report.Parameters["ServerType"].Value = selectedType;
            this.ReportViewer1.Report = report;
            this.ReportViewer1.DataBind();
        }

        public void fillcombo(string sType)
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.SrvDiskFreeSpaceTrendRptBL(sType);
            /*
            ServerListFilterComboBox.DataSource = dt;
            ServerListFilterComboBox.TextField = "ServerDiskName";
            ServerListFilterComboBox.ValueField = "ServerDiskName";
            ServerListFilterComboBox.DataBind();
             */
            ServerListFilterListBox.DataSource = dt;
            ServerListFilterListBox.TextField = "ServerDiskName";
            ServerListFilterListBox.ValueField = "ServerDiskName";
            ServerListFilterListBox.DataBind();
        }

        public void fillservertypelist()
        {
            DataTable dt = new DataTable();
            dt = VSWebBL.ReportsBL.ReportsBL.Ins.SrvDiskFreeSpaceServerTypes();
            ServerTypeFilterListBox.DataSource = dt;
            ServerTypeFilterListBox.TextField = "ServerType";
            ServerTypeFilterListBox.ValueField = "ServerType";
            ServerTypeFilterListBox.DataBind();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString["M"] == "C" && Request.QueryString["M"].ToString() != "")
            {
                this.MasterPageFile = "~/Reports.Master";

            }
            else
            {
                this.MasterPageFile = "~/Reports.Master";

            }
        }
        protected void ReptBtn_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Configurator/Reports.aspx?M=" + Request.QueryString["M"], false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Report();
        }

        protected void ServerListResetButton_Click(object sender, EventArgs e)
        {
            this.ServerListFilterListBox.UnselectAll();
            //2/4/2015 NS added for VSPLUS-1370
            this.ServerTypeFilterListBox.UnselectAll();
            fillcombo("");
            Report();
        }
        //1/27/2016 Ns added for VSPLUS-2533
        protected void WriteServiceHistoryEntry(string strMsg)
        {
            bool appendMode = true;
            string ServiceLogDestination = AppDomain.CurrentDomain.BaseDirectory.ToString() + "/log_files/ScheduledReports_Service.txt";
            try
            {
                StreamWriter sw = new StreamWriter(ServiceLogDestination, appendMode, System.Text.Encoding.Unicode);
                sw.WriteLine(strMsg);
                sw.Close();
                sw = null;
            }
            catch
            {
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}