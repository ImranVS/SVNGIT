using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace VSWebUI.Configurator
{
    public partial class ScheduledReports_Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int rID = -1;
            if (!IsPostBack)
            {
                if (Request.QueryString["ID"] != "" && Request.QueryString["ID"] != null)
                {
                    rID = int.Parse(Request.QueryString["ID"]);
                }
                else
                {
                    RptFrequencyCheckBoxList.ClientVisible = false;
                    RptDayTextBox.ClientVisible = false;
                }
                FillSchedRptList();
                FillRpt(rID);
                SetVisibility();
            }
        }

        public void FillSchedRptList()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = VSWebBL.DashboardBL.ReportBL.Ins.GetRptsMaySchedule();
                RptListComboBox.DataSource = dt;
                RptListComboBox.TextField = "Name";
                RptListComboBox.ValueField = "ID";
                RptListComboBox.DataBind();
            }
            catch (Exception ex)
            {
                //5/15/2014 NS modified for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                //WriteServiceHistoryEntry(DateTime.Now.ToString() + "Exception - " + ex);
                throw ex;
            }
        }

        protected void RptFrequencyRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RptFrequencyRadioButtonList.SelectedItem.Value.ToString() == "1")
            {
                RptFrequencyCheckBoxList.ClientVisible = true;
                RptDayLabel.ClientVisible = false;
                RptDayTextBox.ClientVisible = false;
            }
            else if (RptFrequencyRadioButtonList.SelectedItem.Value.ToString() == "2")
            {
                RptFrequencyCheckBoxList.ClientVisible = false;
                RptDayLabel.ClientVisible = true;
                RptDayTextBox.ClientVisible = true;
            }
            else
            {
                RptFrequencyCheckBoxList.ClientVisible = false;
                RptDayLabel.ClientVisible = false;
                RptDayTextBox.ClientVisible = false;
            }
        }

        protected void RptApplyButton_Click(object sender, EventArgs e)
        {
            string doNotProceed = "";
            string days = "";
            int specificday = 0;
            DataTable dt = new DataTable();
            int rID = -1;
            int reportID = 0;
            if (Request.QueryString["ID"] != "" && Request.QueryString["ID"] != null)
            {
                rID = int.Parse(Request.QueryString["ID"]);
            }
            dt = VSWebBL.ConfiguratorBL.ReportsBL.Ins.GetReports(rID);
            try
            {
                if (RptListComboBox.SelectedItem.Text != "")
                {
                    reportID = Convert.ToInt32(RptListComboBox.SelectedItem.Value);
                }
                if (RptFrequencyRadioButtonList.SelectedItem.Value.ToString() == "1")
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (RptFrequencyCheckBoxList.Items[i].Selected)
                        {
                            if (RptFrequencyCheckBoxList.SelectedItems.Count == 1)
                            {
                                days += RptFrequencyCheckBoxList.SelectedItem.Text;
                            }
                            else
                            {
                                days += RptFrequencyCheckBoxList.Items[i].Text + ",";
                            }
                        }
                    }
                }
                else if (RptFrequencyRadioButtonList.SelectedItem.Value.ToString() == "2")
                {
                    specificday = Convert.ToInt32(RptDayTextBox.Text);

                }
                if (RptFrequencyCheckBoxList.SelectedItems.Count > 1)
                {
                    if (days != "")
                    {
                        days = days.Remove(days.Length - 1);
                    }
                }
                if (RptFrequencyRadioButtonList.SelectedItem.Value.ToString() == "1" && days == "")
                {
                    doNotProceed = "you must select at least one day when Weekly frequency is set.";
                }
                if (doNotProceed == "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        //Update
                        ScheduledReports sr = new ScheduledReports(int.Parse(dt.Rows[0]["ID"].ToString()), reportID,
                            RptFrequencyRadioButtonList.SelectedItem.Text, days, specificday, RptSendToTextBox.Text,
                            RptCopyToTextBox.Text, RptBlindCopyToTextBox.Text, RptSubjectTextBox.Text, RptBodyMemo.Text,
                            RptFileFormatComboBox.SelectedItem.Text);
                        bool updated = VSWebBL.ConfiguratorBL.ReportsBL.Ins.UpdateData(sr);
                    }
                    else
                    {
                        //Insert
                        ScheduledReports sr = new ScheduledReports(0, reportID,
                            RptFrequencyRadioButtonList.SelectedItem.Text, days, specificday, RptSendToTextBox.Text,
                            RptCopyToTextBox.Text, RptBlindCopyToTextBox.Text, RptSubjectTextBox.Text, RptBodyMemo.Text,
                            RptFileFormatComboBox.SelectedItem.Text);
                        bool inserted = VSWebBL.ConfiguratorBL.ReportsBL.Ins.InsertData(sr);
                    }
                    errorDiv.Style.Value = "display: none";
                    Session["SchedRptUpdateStatus"] = RptListComboBox.SelectedItem.Text;
                    Response.Redirect("Reports.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    errorDiv.Style.Value = "display: block";
                    errorDiv.InnerHtml = RptListComboBox.SelectedItem.Text.ToString() + " scheduling has failed: " + doNotProceed +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                }
            }
            catch (Exception ex)
            {
                errorDiv.Style.Value = "display: block";
                errorDiv.InnerHtml = RptListComboBox.SelectedItem.Text.ToString() + " report scheduling has failed: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void WriteServiceHistoryEntry(string strMsg)
        {
            bool appendMode = true;

            string ServiceLogDestination = Page.MapPath("~/LogFiles/WebLogs.txt");
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

        public void FillRpt(int rID)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = VSWebBL.ConfiguratorBL.ReportsBL.Ins.GetReports(rID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < RptListComboBox.Items.Count; i++)
                    {
                        if (RptListComboBox.Items[i].Text == dt.Rows[0]["Name"].ToString())
                        {
                            RptListComboBox.Items[i].Selected = true;
                        }
                    }
                    RptSubjectTextBox.Text = dt.Rows[0]["Title"].ToString();
                    RptBodyMemo.Text = dt.Rows[0]["Body"].ToString();
                    for (int i = 0; i < RptFrequencyRadioButtonList.Items.Count; i++)
                    {
                        if (RptFrequencyRadioButtonList.Items[i].Text == dt.Rows[0]["Frequency"].ToString())
                        {
                            RptFrequencyRadioButtonList.Items[i].Selected = true;
                        }
                    }
                    RptDayTextBox.Text = dt.Rows[0]["SpecificDay"].ToString();
                    for (int i = 0; i < RptFileFormatComboBox.Items.Count; i++)
                    {
                        if (RptFileFormatComboBox.Items[i].Text == dt.Rows[0]["FileFormat"].ToString())
                        {
                            RptFileFormatComboBox.Items[i].Selected = true;
                        }
                    }
                    RptSendToTextBox.Text = dt.Rows[0]["SendTo"].ToString();
                    RptCopyToTextBox.Text = dt.Rows[0]["CopyTo"].ToString();
                    RptBlindCopyToTextBox.Text = dt.Rows[0]["BlindCopyTo"].ToString();
                    String chararr = ",";
                    String[] daysarr = Regex.Split(dt.Rows[0]["Days"].ToString(),chararr);
                    for (int i = 0; i < daysarr.Length; i++)
                    {
                        for (int j=0;j<RptFrequencyCheckBoxList.Items.Count;j++){
                            if (daysarr[i].ToString() == RptFrequencyCheckBoxList.Items[j].Text)
                            {
                                RptFrequencyCheckBoxList.Items[j].Selected = true;
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        private void SetVisibility()
        {
            if (RptFrequencyRadioButtonList.SelectedItem.Text == "Daily")
            {
                RptFrequencyCheckBoxList.ClientVisible = false;
                RptDayTextBox.ClientVisible = false;
                RptDayLabel.ClientVisible = false;
            }
            else if (RptFrequencyRadioButtonList.SelectedItem.Text == "Weekly")
            {
                RptFrequencyCheckBoxList.ClientVisible = true;
                RptDayTextBox.ClientVisible = false;
                RptDayLabel.ClientVisible = false;
            }
            else if (RptFrequencyRadioButtonList.SelectedItem.Text == "Monthly")
            {
                RptFrequencyCheckBoxList.ClientVisible = false;
                RptDayTextBox.ClientVisible = true;
                RptDayLabel.ClientVisible = true;
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Reports.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}