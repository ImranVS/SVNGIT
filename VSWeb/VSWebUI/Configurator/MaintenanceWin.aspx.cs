using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebDO;
using DevExpress.XtraScheduler;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler.Xml;
using DevExpress.Web.ASPxScheduler.Internal;
using System.Data.SqlClient;
using System.Xml;
using System.Globalization;
using DevExpress.Web.ASPxTreeList;
using System.Data.SqlTypes;
using System.Collections;


namespace VSWebUI.Configurator
{
    public partial class MaintenanceWin : System.Web.UI.Page
    {
        #region "Declarations"
        string MaintKey;
        string maintenanceName = "";
        string copy = "";
		DataTable DataServersTree;
        #endregion

        #region "Page Control Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            ServerTime();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "")
                MaintKey = Request.QueryString["ID"];
            if (Request.QueryString["Copy"] != null && Request.QueryString["Copy"] != "")
            {
                copy = Request.QueryString["Copy"];
            }
                
           // MaintSrvGridView.Styles.AlternatingRow.BackColor = System.Drawing.Color.FromName("#ADD8E6");

            if (!IsPostBack)
            {
              //MD 30Jan14
                //MaintRepeatTypeRoundPanel.Visible = false;
               
               // DataTable MaintenanceWindowDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetMaintenanceWindow("", "");
               //NameTextBox.DataSource = MaintenanceWindowDataTable;
               //NameTextBox.DataBind();
               // if (MaintKey != "")
               // {
               //     DataTable MaintenanceWindowSelDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetMaintenanceWindow(MaintKey, "");
               //     for (int i = 0; i <NameTextBox.Items.Count; i++)
               //     {
               //         if (MaintNameComboBox.Items[i].Value.ToString() == MaintenanceWindowSelDataTable.Rows[0]["Name"].ToString())
               //         {
               //            NameTextBox.Items[i].Selected = true;
               //         }
               //     }
               // }
                if (MaintKey != "" && MaintKey!=null)
                {
                    NameTextBox.Text = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetName(MaintKey);
                    if (copy != "")
                    {
                        maintenanceName = "Copy of (" + NameTextBox.Text + ")";
                    }
                }
                //MD 30Jan14
                //DataTable ServerGridDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetServers(MaintKey);
                //MaintSrvGridView.DataSource = ServerGridDataTable;
                //MaintSrvGridView.DataBind();
                PopulateRepeatFields();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "MaintenanceWin|MaintSrvGridView")
                        {
                            MaintSrvGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
               // DataTable MaintenanceWindowDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetMaintenanceWindow("", "");
               //NameTextBox.DataSource = MaintenanceWindowDataTable;
               //NameTextBox.DataBind();
               // NameTextBox.Text = "";


                //MD 30Jan14
                //DataTable ServerGridDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetServers(MaintKey);
                //MaintSrvGridView.DataSource = ServerGridDataTable;
                //MaintSrvGridView.DataBind();

            }
            if (!IsPostBack)
            {
                fillServersTreeList();
                //11/25/2015 NS added for VSPLUS-2227
                FillMobileDevicesGrid();
            }
            else
            {
                fillServersTreefromSession();
                //11/25/2015 NS added for VSPLUS-2227
                FillMobileDevicesGridfromSession();
            }
			
        }
        #endregion

        #region "Procedures/Functions"
        private void ResetRepeatOptions(string selValue)
        {
            MaintRepeatRadioButtonList.Value = selValue;
            string selText = MaintRepeatRadioButtonList.SelectedItem.Text;
            switch (selValue)
            {
                //One Time
                case "1":
                    //MaintRepeatTypeRoundPanel.Visible = false;
                    MaintRepeatDailyLabel.Visible = false;
                    MaintRepeatEveryLabel.Visible = false;
                    MaintRepeatWeeksTextBox.Visible = false;
                    MaintRepeatWeeksLabel.Visible = false;
                    ASPxCheckBoxSelectAll.Visible = false;
                    //11/24/2015 NS added for VSPLUS-2227
                    SelectAllBtn.Visible = false;
                    ClearAllBtn.Visible = false;
                    MaintRepeatCheckBoxList.Visible = false;
                    MaintRepeatCheckBoxList.UnselectAll();
                    MaintRepeatMonthlyLabel.Visible = false;
                    MaintRepeatMonthlyComboBox.Visible = false;
                    MaintRepeatDayLabel.Visible = false;
                    MaintRepeatDayTextBox.Visible = false;
                    //2/17/2014 NS added
                    infoDivOnce.Style.Value = "display: block";
                    infoDivDaily.Style.Value = "display: none";
                    infoDivWeekly.Style.Value = "display: none";
                    infoDivMonthly.Style.Value = "display: none";
                    //9/9/2014 NS added for VSPLUS-911
                    lblEndDate.Visible = true;
                    ASPxLabelEndDate.Visible = true;
                    MaintEndDateEdit.Visible = false;
                    lblDurationType.Visible = false;
                    RadioButtonListEndDate.Visible = false;
                    lblEndDate.ClientVisible = true;
                    ASPxLabelEndDate.ClientVisible = true;
                    MaintEndDateEdit.ClientVisible = false;
                    lblDurationType.ClientVisible = false;
                    RadioButtonListEndDate.ClientVisible = false;
                    if (lblEndDate.Text != "")
                    {
                        MaintEndDateEdit.Text = lblEndDate.Text;
                    }
                    break;
                //Daily
                case "2":
                    //if (!MaintRepeatTypeRoundPanel.Visible)
                    //{
                    //    MaintRepeatTypeRoundPanel.Visible = true;
                    //}
                    MaintRepeatDailyLabel.Visible = false;
                    MaintRepeatEveryLabel.Visible = false;
                    MaintRepeatWeeksTextBox.Visible = false;
                    MaintRepeatWeeksLabel.Visible = false;
                    //2/27/2014 NS modified for VSPLUS-409
                    //ASPxCheckBoxSelectAll.Visible = true;
                    //MaintRepeatCheckBoxList.Visible = true;
                    //MaintRepeatCheckBoxList.UnselectAll();
                    ASPxCheckBoxSelectAll.Visible = false;
                    //11/24/2015 NS added for VSPLUS-2227
                    SelectAllBtn.Visible = false;
                    ClearAllBtn.Visible = false;
                    MaintRepeatCheckBoxList.Visible = false;
                    MaintRepeatCheckBoxList.SelectAll();

                    MaintRepeatMonthlyLabel.Visible = false;
                    MaintRepeatMonthlyComboBox.Visible = false;
                    MaintRepeatDayLabel.Visible = false;
                    MaintRepeatDayTextBox.Visible = false;
                    //2/17/2014 NS added
                    infoDivOnce.Style.Value = "display: none";
                    infoDivDaily.Style.Value = "display: block";
                    infoDivWeekly.Style.Value = "display: none";
                    infoDivMonthly.Style.Value = "display: none";
                    //MaintRepeatCheckBoxList.SelectAll();
                    //9/9/2014 NS added for VSPLUS-911
                    lblEndDate.Visible = false;
                    lblDurationType.Visible = true;
                    RadioButtonListEndDate.Visible = true;
                    lblEndDate.ClientVisible = false;
                    if (RadioButtonListEndDate.SelectedItem.Value.ToString() == "1")
                    {
                        ASPxLabelEndDate.Visible = true;
                        ASPxLabelEndDate.ClientVisible = true;
                        MaintEndDateEdit.Visible = true;
                        MaintEndDateEdit.ClientVisible = true;
                    }
                    else
                    {
                        ASPxLabelEndDate.Visible = false;
                        ASPxLabelEndDate.ClientVisible = false;
                        MaintEndDateEdit.Visible = false;
                        MaintEndDateEdit.ClientVisible = false;
                    }
                    lblDurationType.ClientVisible = true;
                    RadioButtonListEndDate.ClientVisible = true;
                    break;
                //Weekly
                case "3":
                    //if (!MaintRepeatTypeRoundPanel.Visible)
                    //{
                    //    MaintRepeatTypeRoundPanel.Visible = true;
                    //}
                    MaintRepeatDailyLabel.Visible = false;
                    MaintRepeatEveryLabel.Visible = true;
                    MaintRepeatWeeksTextBox.Visible = true;
                    MaintRepeatWeeksTextBox.Text = "1";
                    MaintRepeatWeeksLabel.Visible = true;
                    //ASPxCheckBoxSelectAll.Visible = true;
                    //8/6/2014 NS added
                    //ASPxCheckBoxSelectAll.Checked = false;
                    //11/24/2015 NS added for VSPLUS-2227
                    SelectAllBtn.Visible = true;
                    ClearAllBtn.Visible = true;
                    MaintRepeatCheckBoxList.Visible = true;
                    MaintRepeatCheckBoxList.UnselectAll();
                    MaintRepeatMonthlyLabel.Visible = false;
                    MaintRepeatMonthlyComboBox.Visible = false;
                    MaintRepeatDayLabel.Visible = false;
                    MaintRepeatDayTextBox.Visible = false;
                    //2/17/2014 NS added
                    infoDivOnce.Style.Value = "display: none";
                    infoDivDaily.Style.Value = "display: none";
                    infoDivWeekly.Style.Value = "display: block";
                    infoDivMonthly.Style.Value = "display: none";
                    //9/9/2014 NS added for VSPLUS-911
                    lblEndDate.Visible = false;
                    lblDurationType.Visible = true;
                    RadioButtonListEndDate.Visible = true;
                    lblEndDate.ClientVisible = false;
                    if (RadioButtonListEndDate.SelectedItem.Value.ToString() == "1")
                    {
                        ASPxLabelEndDate.Visible = true;
                        ASPxLabelEndDate.ClientVisible = true;
                        MaintEndDateEdit.Visible = true;
                        MaintEndDateEdit.ClientVisible = true;
                    }
                    else
                    {
                        ASPxLabelEndDate.Visible = false;
                        ASPxLabelEndDate.ClientVisible = false;
                        MaintEndDateEdit.Visible = false;
                        MaintEndDateEdit.ClientVisible = false;
                    }
                    lblDurationType.ClientVisible = true;
                    RadioButtonListEndDate.ClientVisible = true;
                    break;
                //Monthly
                case "4":
                    //if (!MaintRepeatTypeRoundPanel.Visible)
                    //{
                    //    MaintRepeatTypeRoundPanel.Visible = true;
                    //}
                    MaintRepeatDailyLabel.Visible = false;
                    MaintRepeatEveryLabel.Visible = false;
                    MaintRepeatWeeksTextBox.Visible = false;
                    MaintRepeatWeeksLabel.Visible = false;
                    //ASPxCheckBoxSelectAll.Visible = true;
                    //8/6/2014 NS added
                    //ASPxCheckBoxSelectAll.Checked = false;
                    //11/24/2015 NS added for VSPLUS-2227
                    SelectAllBtn.Visible = true;
                    ClearAllBtn.Visible = true;
                    MaintRepeatCheckBoxList.Visible = true;
                    MaintRepeatCheckBoxList.UnselectAll();
                    MaintRepeatMonthlyLabel.Visible = true;
                    MaintRepeatMonthlyComboBox.Visible = true;
                    MaintRepeatMonthlyComboBox.SelectedIndex = -1;
                    MaintRepeatDayLabel.Visible = false;
                    MaintRepeatDayTextBox.Visible = false;
                    //2/17/2014 NS added
                    infoDivOnce.Style.Value = "display: none";
                    infoDivDaily.Style.Value = "display: none";
                    infoDivWeekly.Style.Value = "display: none";
                    infoDivMonthly.Style.Value = "display: block";
                    //9/9/2014 NS added for VSPLUS-911
                    lblEndDate.Visible = false;
                    lblDurationType.Visible = true;
                    RadioButtonListEndDate.Visible = true;
                    lblEndDate.ClientVisible = false;
                    if (RadioButtonListEndDate.SelectedItem.Value.ToString() == "1")
                    {
                        ASPxLabelEndDate.Visible = true;
                        ASPxLabelEndDate.ClientVisible = true;
                        MaintEndDateEdit.Visible = true;
                        MaintEndDateEdit.ClientVisible = true;
                    }
                    else
                    {
                        ASPxLabelEndDate.Visible = false;
                        ASPxLabelEndDate.ClientVisible = false;
                        MaintEndDateEdit.Visible = false;
                        MaintEndDateEdit.ClientVisible = false;
                    }
                    lblDurationType.ClientVisible = true;
                    RadioButtonListEndDate.ClientVisible = true;
                    break;
                default:
                    break;
            }
            //MaintRepeatTypeRoundPanel.HeaderText = "Repeat " + selText;
        }
        private void PopulateRepeatFields()
        {
            //string WinName = "";
            //for (int i = 0; i <NameTextBox.Items.Count; i++)
            //{
            //    if (MaintNameComboBox.Items[i].Selected)
            //    {
            //        WinName =NameTextBox.Items[i].Value.ToString();
            //    }
            //}
            //9/9/2014 NS modified for VSPLUS-911
            if (NameTextBox.Text == "")
            {
                lblEndDate.Visible = true;
                lblEndDate.ClientVisible = true;
                MaintEndDateEdit.ClientVisible = false;
                lblDurationType.ClientVisible = false;
                RadioButtonListEndDate.ClientVisible = false;
                MaintStartDateEdit.Value = DateTime.Now;
                //9/16/2014 NS added for VSPLUS-911
                DateTime endDateTime = DateTime.Parse(MaintStartDateEdit.Text + " " + MaintStartTimeEdit.Text);
                if (MaintDurationTextBox.Text == "")
                {
                    MaintDurationTextBox.Text = "0";
                }
                endDateTime = endDateTime.AddMinutes(Convert.ToDouble(MaintDurationTextBox.Text));
                lblEndDate.Text = endDateTime.ToShortDateString();
                MaintEndDateEdit.Value = lblEndDate.Text;
                lblEndTime.Text = MaintStartTimeEdit.Text;
            }
            else if (NameTextBox.Text!= "")
            {
                DataTable MaintenanceDataTable = new DataTable();
                string maintID = "";
                string sqlDT = "";
                DateTime dt;
                //Get values from the Maintenance table
                MaintenanceDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetMaintenanceWindow(MaintKey, NameTextBox.Text);
                if (MaintenanceDataTable.Rows.Count > 0)
                {
                    if (copy != "")
                    {
                        NameTextBox.Text = maintenanceName;
                    }
                    maintID = MaintenanceDataTable.Rows[0]["ID"].ToString();
                    sqlDT = MaintenanceDataTable.Rows[0]["StartDate"].ToString();
                    dt = DateTime.Parse(sqlDT);
                    MaintStartDateEdit.Date = dt;
                    sqlDT = MaintenanceDataTable.Rows[0]["StartTime"].ToString();
                    dt = DateTime.Parse(sqlDT);
                    MaintStartTimeEdit.DateTime = dt;
                    sqlDT = MaintenanceDataTable.Rows[0]["EndDate"].ToString();
                    dt = DateTime.Parse(sqlDT);
                    MaintEndDateEdit.Date = dt;
                    
                    MaintDurationTextBox.Value = MaintenanceDataTable.Rows[0]["Duration"].ToString();
                    lblDuration.Text = DurationHoursMins(MaintenanceDataTable.Rows[0]["Duration"].ToString());// DurationHoursMins(MaintenanceDataTable.Rows[0]["Duration"].ToString());
                    DurationTrackBar.Value = MaintenanceDataTable.Rows[0]["Duration"].ToString();
                    //lblEndTime.Text = Convert.ToDateTime(MaintStartTimeEdit.Value).AddMinutes(Convert.ToDouble(MaintDurationTextBox.Value)).ToShortTimeString();
                    lblEndTime.Text = Convert.ToDateTime(MaintStartTimeEdit.Value).AddMinutes(Convert.ToDouble(DurationTrackBar.Value)).ToShortTimeString();
                    //Need to add fields for Repeat panels based on the column values of MaintType and MaintDaysList
                    string maintType = MaintenanceDataTable.Rows[0]["MaintType"].ToString();
                    string RepeatDaysListTxt = "";
                    string[] RepeatDaysList;

                    RadioButtonListEndDate.Value = (MaintenanceDataTable.Rows[0]["Type"].ToString() == "" ? "1" : MaintenanceDataTable.Rows[0]["Type"].ToString());
                    DurationSelect(false);

                    MaintRepeatRadioButtonList.Value = maintType;
                    ResetRepeatOptions(MaintRepeatRadioButtonList.Value.ToString());
                    //9/9/2014 NS added for VSPLUS-911
                    lblEndDate.ClientVisible = false;
                    DateTime endDateTime = DateTime.Parse(MaintStartDateEdit.Text + " " + MaintStartTimeEdit.Text);
                    endDateTime = endDateTime.AddMinutes(Convert.ToDouble(MaintDurationTextBox.Text));
                    lblEndDate.Text = endDateTime.ToShortDateString();
                    switch (maintType)
                    {
                        //One Time
                        case "1":
                            //2/17/2014 NS added
                            infoDivOnce.Style.Value = "display: block";
                            infoDivDaily.Style.Value = "display: none";
                            infoDivWeekly.Style.Value = "display: none";
                            infoDivMonthly.Style.Value = "display: none";
                            //9/9/2014 NS added for VSPLUS-911
                            lblEndDate.ClientVisible = true;
                            lblEndDate.Visible = true;
                            MaintEndDateEdit.ClientVisible = false;
                            lblDurationType.ClientVisible = false;
                            RadioButtonListEndDate.ClientVisible = false;
                            break;
                        //Daily
                        case "2":
                            //2/17/2014 NS added
                            infoDivOnce.Style.Value = "display: none";
                            infoDivDaily.Style.Value = "display: block";
                            infoDivWeekly.Style.Value = "display: none";
                            infoDivMonthly.Style.Value = "display: none";
                            RepeatDaysListTxt = MaintenanceDataTable.Rows[0]["MaintDaysList"].ToString();
                            RepeatDaysList = RepeatDaysListTxt.Split(',');
                            for (int i = 0; i < RepeatDaysList.Length; i++)
                            {
                                if (MaintRepeatCheckBoxList.Items.FindByValue(RepeatDaysList[i]) != null)
                                {
                                    MaintRepeatCheckBoxList.Items.FindByValue(RepeatDaysList[i]).Selected = true;
                                }
                            }
                            break;
                        //Weekly
                        case "3":
                            //2/17/2014 NS added
                            infoDivOnce.Style.Value = "display: none";
                            infoDivDaily.Style.Value = "display: none";
                            infoDivWeekly.Style.Value = "display: block";
                            infoDivMonthly.Style.Value = "display: none";
                            string[] RepeatDaysListWeekTxt;
                            RepeatDaysListTxt = MaintenanceDataTable.Rows[0]["MaintDaysList"].ToString();
                            RepeatDaysList = RepeatDaysListTxt.Split(',');
                            for (int i = 0; i < RepeatDaysList.Length; i++)
                            {
                                RepeatDaysListWeekTxt = RepeatDaysList[i].Split(':');
                                if (MaintRepeatCheckBoxList.Items.FindByValue(RepeatDaysListWeekTxt[0]) != null)
                                {
                                    MaintRepeatCheckBoxList.Items.FindByValue(RepeatDaysListWeekTxt[0]).Selected = true;
                                }
                                MaintRepeatWeeksTextBox.Value = RepeatDaysListWeekTxt[1];
                            }
                            break;
                        //Monthly
                        case "4":
                            //2/17/2014 NS added
                            infoDivOnce.Style.Value = "display: none";
                            infoDivDaily.Style.Value = "display: none";
                            infoDivWeekly.Style.Value = "display: none";
                            infoDivMonthly.Style.Value = "display: block";
                            //ASPxCheckBoxSelectAll.Visible = true;
                            //11/24/2015 NS added for VSPLUS-2227
                            SelectAllBtn.Visible = true;
                            ClearAllBtn.Visible = true;
                            string[] RepeatDaysListMonthTxt;
                            RepeatDaysListTxt = MaintenanceDataTable.Rows[0]["MaintDaysList"].ToString();
                            RepeatDaysList = RepeatDaysListTxt.Split(',');
                            for (int i = 0; i < RepeatDaysList.Length; i++)
                            {
                                RepeatDaysListMonthTxt = RepeatDaysList[i].Split(':');
                                if (RepeatDaysListMonthTxt.Length > 1)
                                {
                                    if (MaintRepeatCheckBoxList.Items.FindByValue(RepeatDaysListMonthTxt[0]) != null)
                                    {
                                        MaintRepeatCheckBoxList.Visible = true;
                                        MaintRepeatDayLabel.Visible = false;
                                        MaintRepeatDayTextBox.Visible = false;
                                        MaintRepeatCheckBoxList.Items.FindByValue(RepeatDaysListMonthTxt[0]).Selected = true;
                                    }
                                    if (MaintRepeatMonthlyComboBox.Items.FindByText(RepeatDaysListMonthTxt[1]) != null)
                                    {
                                        MaintRepeatMonthlyComboBox.Items.FindByText(RepeatDaysListMonthTxt[1]).Selected = true;
                                    }
                                }
                                else
                                {
                                    //Specific day
                                    if (MaintRepeatMonthlyComboBox.Items.FindByValue("5") != null)
                                    {
                                        MaintRepeatMonthlyComboBox.Items.FindByValue("5").Selected = true;
                                        MaintRepeatCheckBoxList.Visible = false;
                                        MaintRepeatDayLabel.Visible = true;
                                        MaintRepeatDayTextBox.Visible = true;
                                        MaintRepeatDayTextBox.Value = RepeatDaysListMonthTxt[0];
                                        //2/17/2014 NS added
                                        ASPxCheckBoxSelectAll.Visible = false;
                                        //11/24/2015 NS added for VSPLUS-2227
                                        SelectAllBtn.Visible = false;
                                        ClearAllBtn.Visible = false;
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    if (MaintRepeatCheckBoxList.SelectedItems.Count == 7)
                    {
                        ASPxCheckBoxSelectAll.Checked = true;
                    }
                }
                //MD 31Jan14
                ////Get values from the ServerMaintenance table
                //DataTable ServerMaintenanceDataTable = new DataTable();
                //ServerMaintenanceDataTable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetServerMaintenance(maintID);
                //MaintSrvGridView.Selection.UnselectAll();
                //if (ServerMaintenanceDataTable.Rows.Count > 0)
                //{
                //    for (int i = 0; i < ServerMaintenanceDataTable.Rows.Count; i++)
                //    {
                //        string key = ServerMaintenanceDataTable.Rows[i]["ServerID"].ToString();
                //        string key2 = ServerMaintenanceDataTable.Rows[i]["ServerTypeID"].ToString();
                //        for (int j = 0; j < MaintSrvGridView.VisibleRowCount; j++)
                //        {
                //            if (MaintSrvGridView.GetRowValues(j, "ID").ToString() == key && MaintSrvGridView.GetRowValues(j, "ServerTypeID").ToString() == key2)
                //            {
                //                MaintSrvGridView.Selection.SelectRow(j);
                //            }
                //        }
                //    }
                //}
            }
        }

        public void ServerTime()
        {
            const string dataFmt = "{0,-30}{1}";
            const string timeFmt = "{0,-30}{1:yyyy-MM-dd HH:mm}";
         
            // Get the local time zone and the current local time and year.
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            int currentYear = currentDate.Year;
            string strlocalZone="";
            if (localZone.IsDaylightSavingTime(currentDate))
            {
                strlocalZone = localZone.StandardName;
            }
            else
            {
                strlocalZone = localZone.DaylightName;
            }
            MaitenanceRoundPanel.HeaderText = "Server:: " + strlocalZone + ": " + currentDate.ToString();

            // Display the names for standard time and daylight saving  
            // time for the local time zone.
            //Console.WriteLine(dataFmt, "Standard time name:",
            //    localZone.StandardName);
            //Console.WriteLine(dataFmt, "Daylight saving time name:",
            //    localZone.DaylightName);

            // Display the current date and time and show if they occur  
            // in daylight saving time.
            //Console.WriteLine("\n" + timeFmt, "Current date and time:",
            //    currentDate);
            //Console.WriteLine(dataFmt, "Daylight saving time?",
            //    localZone.IsDaylightSavingTime(currentDate));

            // Get the current Coordinated Universal Time (UTC) and UTC  
            // offset.
            //DateTime currentUTC =
            //    localZone.ToUniversalTime(currentDate);
            //TimeSpan currentOffset =
            //    localZone.GetUtcOffset(currentDate);

            //Console.WriteLine(timeFmt, "Coordinated Universal Time:",
            //    currentUTC);
            //Console.WriteLine(dataFmt, "UTC offset:", currentOffset);

            // Get the DaylightTime object for the current year.
            //DaylightTime daylight =
            //    localZone.GetDaylightChanges(currentYear);

            // Display the daylight saving time range for the current year.
           // Console.WriteLine(
           //     "\nDaylight saving time for year {0}:", currentYear);
           // Console.WriteLine("{0:yyyy-MM-dd HH:mm} to " +
           //     "{1:yyyy-MM-dd HH:mm}, delta: {2}",
            //    daylight.Start, daylight.End, daylight.Delta);
        }
        //11/30/2015 NS added for VSPLUS-2227
        private void FillMobileDevicesGrid()
        {
            try
            {
                DataTable dt1 = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.GetKeyUserDevices("");
                KeyUsersGridView.DataSource = dt1;
                Session["KeyUsersGridView"] = dt1;
                KeyUsersGridView.DataBind();

                string id = "";
                DataTable dt2 = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetKeyMobileUsers(MaintKey);

                if (dt2.Rows.Count > 0)
				{
					if (MaintKey != null && MaintKey != "")
					{
                        DataRow[] dttemp = dt2.Select("MaintID=" + MaintKey);
                        if (dttemp.Any())
                        {
                            DataTable dtSel = dttemp.CopyToDataTable();
                            if (dtSel.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtSel.Rows.Count; i++)
                                {
                                    for (int j = i; j < KeyUsersGridView.VisibleRowCount; j++)
                                    {
                                        id = KeyUsersGridView.GetRowValues(j, KeyUsersGridView.KeyFieldName).ToString();
                                        if (dtSel.Rows[i]["ID"].ToString() == id.ToString())
                                        {
                                            KeyUsersGridView.Selection.SelectRow(j);
                                        }
                                    }
                                }
                            }
                        }
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
        private void FillMobileDevicesGridfromSession()
        {
            try
            {
                DataTable dt = new DataTable();
                if ((Session["KeyUsersGridView"] != null))
                {
                    dt = (DataTable)Session["KeyUsersGridView"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                    if (dt.Rows.Count > 0)
                    {
                        KeyUsersGridView.DataSource = dt;
                        KeyUsersGridView.DataBind();
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
        #endregion

        protected void MaintRepeatRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selItemValue = MaintRepeatRadioButtonList.SelectedItem.Value.ToString();
            ResetRepeatOptions(selItemValue);
        }

        protected void MaintRepeatMonthlyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selItemVal = "";
            for (int i = 0; i < MaintRepeatMonthlyComboBox.Items.Count; i++)
            {
                if (MaintRepeatMonthlyComboBox.Items[i].Selected)
                {
                    selItemVal = MaintRepeatMonthlyComboBox.Items[i].Value.ToString();
                    
                }
            }
            if (selItemVal == "5")
            {
                MaintRepeatCheckBoxList.Visible = false;
                MaintRepeatDayLabel.Visible = true;
                MaintRepeatDayTextBox.Visible = true;
                //2/17/2014 NS added
                ASPxCheckBoxSelectAll.Visible = false;
                //11/24/2015 NS added for VSPLUS-2227
                SelectAllBtn.Visible = false;
                ClearAllBtn.Visible = false;
                infoDivOnce.Style.Value = "display: none";
                infoDivDaily.Style.Value = "display: none";
                infoDivWeekly.Style.Value = "display: none";
                infoDivMonthly.Style.Value = "display: block";
            }
            else
            {
                MaintRepeatCheckBoxList.Visible = true;
                MaintRepeatDayLabel.Visible = false;
                MaintRepeatDayTextBox.Visible = false;
                //2/17/2014 NS added
                //ASPxCheckBoxSelectAll.Visible = true;
                //11/24/2015 NS added for VSPLUS-2227
                SelectAllBtn.Visible = true;
                ClearAllBtn.Visible = true;
                infoDivOnce.Style.Value = "display: none";
                infoDivDaily.Style.Value = "display: none";
                infoDivWeekly.Style.Value = "display: none";
                infoDivMonthly.Style.Value = "display: block";
            }
        }

        //protected voidNameTextBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    PopulateRepeatFields();
        //}

        protected void MaintAddButton_Click(object sender, EventArgs e)
        {
            //8/6/2014 NS added
            bool maintOK = true;
            bool isnumeric = true;
            string fromdate = MaintStartDateEdit.Text;
            //9/16/2014 NS added for VSPLUS-911
            if (MaintRepeatRadioButtonList.SelectedItem.Value.ToString() == "1")
            {
                DateTime endDateTime = DateTime.Parse(MaintStartDateEdit.Text + " " + MaintStartTimeEdit.Text);
                endDateTime = endDateTime.AddMinutes(Convert.ToDouble(MaintDurationTextBox.Text));
                lblEndDate.Text = endDateTime.ToShortDateString();
                MaintEndDateEdit.Text = lblEndDate.Text;
            }
            string todate = MaintEndDateEdit.Text;
            if (MaintStartDateEdit.Text != "" && MaintEndDateEdit.Text != "")
            {
                if (Convert.ToDateTime(fromdate) > Convert.ToDateTime(todate))
                {
                    //11/13/2015 NS modified
                    /*
                    MaintenancePopupControl.ShowOnPageLoad = true;
                    MaintenancePopupControl.HeaderText = "Validation Error";
                    MsgLabel.Text = "Start Date value should be less than End Date. Please correct the dates before hitting Submit.";
                     */
                    errorDiv.Style.Value = "display: block";
                    errorDiv.InnerHtml = "Start Date value should be less than End Date. Please correct the dates before hitting Submit." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                }
                else if (NameTextBox.Text.Contains("Copy") || NameTextBox.Text.Contains("(") || NameTextBox.Text.Contains(")"))
                {
                    //11/13/2015 NS modified
                    /*
                    MaintenancePopupControl.ShowOnPageLoad = true;
                    MsgLabel.Text = "Invalid window maintenance name. Name should not contain 'Copy of,(,)'.";
                     */
                    errorDiv.Style.Value = "display: block";
                    errorDiv.InnerHtml = "Invalid window maintenance name. Name should not contain 'Copy of,(,)'." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                }
                else
                {
                    try
                    {
                        DataTable nametable = new DataTable();
                        string N = NameTextBox.Text;
                        if (N != "" && N != null)
                            nametable = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.nametable(N, MaintKey);

                        if (nametable.Rows.Count > 0)
                        {
                            //11/13/2015 NS modified
                            /*
                            MaintenancePopupControl.ShowOnPageLoad = true;
                            MsgLabel.Text = "The window maintenance name you entered already exists. Please enter another name.";
                            */
                            errorDiv.Style.Value = "display: block";
                            errorDiv.InnerHtml = "The window maintenance name you entered already exists. Please enter another name." +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        }

                        else
                        {
                            //string WinName = "";
                            List<string> serverIDValues = new List<string>();
                            //9/17/2013 NS added ServerTypeID in order to insert URL records into the ServerMaintenance table
                            List<string> servertypeIDValues = new List<string>();
                            //12/7/2015 NS added for VSPLUS-2227
                            List<string> deviceIDValues = new List<string>();
                            //for (int i = 0; i <NameTextBox.Items.Count; i++)
                            //{
                            //    if (MaintNameComboBox.Items[i].Selected)
                            //    {
                            //        WinName =NameTextBox.Items[i].Value.ToString();
                            //    }
                            //}
                            //if (WinName == "")
                            //{
                            //    WinName =NameTextBox.Value.ToString();
                            //}
                            DateUtils.DateUtils objDateUtils = new DateUtils.DateUtils();
                            string strDateFormat = objDateUtils.GetDateFormat();
                            string dtFormat = (strDateFormat == "dmy" ? "d/M/yyyy" : (strDateFormat == "mdy" ? "M/d/yyyy" : "yyyy/M/d"));
                            string Durationtype = RadioButtonListEndDate.SelectedItem.Value.ToString();
                            string Name = NameTextBox.Text;
                            //string StartDate = MaintStartDateEdit.Value.ToString();
                            string StartDate = objDateUtils.FixDate(Convert.ToDateTime(MaintStartDateEdit.Value), strDateFormat);
                            string StartTime = MaintStartTimeEdit.Text.ToString();
                            DateTime sdt = DateTime.ParseExact(StartDate, dtFormat, null); //Convert.ToDateTime(StartDate);
                            //string Duration = MaintDurationTextBox.Value.ToString();
                            string Duration = DurationTrackBar.Value.ToString();
                            //string EndDate = MaintEndDateEdit.Value.ToString();
                            string EndDate = objDateUtils.FixDate(Convert.ToDateTime(MaintEndDateEdit.Value), strDateFormat);
                            DateTime edt = DateTime.ParseExact(EndDate, dtFormat, null); //Convert.ToDateTime(EndDate);
                            string MaintType = MaintRepeatRadioButtonList.SelectedItem.Value.ToString();
                            string MaintDaysList = "";
                            string altime = Convert.ToDateTime(MaintStartTimeEdit.Text).ToShortTimeString();
                            DateTime al = Convert.ToDateTime(altime);
                            ASPxScheduler sh = new ASPxScheduler();
                            Appointment apt = sh.Storage.CreateAppointment(AppointmentType.Pattern);
                            Reminder r = apt.CreateNewReminder();

                            //int min = Convert.ToInt32(MaintDurationTextBox.Text);
                            int min = Convert.ToInt32(DurationTrackBar.Value);
                            r.AlertTime = al.AddMinutes(min);
                            //3/24/2015 NS modified for DevExpress upgrade 14.2
                            //ReminderXmlPersistenceHelper reminderHelper = new ReminderXmlPersistenceHelper(r, DateSavingType.LocalTime);
                            ReminderXmlPersistenceHelper reminderHelper = new ReminderXmlPersistenceHelper(r);
                            string rem = reminderHelper.ToXml().ToString();
                            RecurrenceInfo reci = new RecurrenceInfo();
                            reci.BeginUpdate();
                            reci.AllDay = false;

                            if (MaintRepeatDayTextBox.Text != "")
                            {

                                reci.Periodicity = Convert.ToInt32(MaintRepeatDayTextBox.Text);
                            }
                            else
                            {

                                reci.Periodicity = 10;
                            }
                            reci.Range = RecurrenceRange.EndByDate;
                            reci.Start = sdt;
                            reci.End = edt;
                            reci.Duration = edt - sdt;
                            if (MaintRepeatRadioButtonList.SelectedItem.Value == "1")
                            {
                                reci.Type = RecurrenceType.Yearly;
                                //Mukund 07Aug14, VSPLUS-874 Maintenance window redirects to the Error page when switching from Monthly to Daily repeat option
                                MaintRepeatMonthlyComboBox.SelectedItem = null;
                                MaintRepeatCheckBoxList.SelectedItem = null;
 
                            }
                            else if (MaintRepeatRadioButtonList.SelectedItem.Value == "2")
                            {
                                reci.Type = RecurrenceType.Daily;
                                //Mukund 07Aug14, VSPLUS-874 Maintenance window redirects to the Error page when switching from Monthly to Daily repeat option
                                MaintRepeatMonthlyComboBox.SelectedItem = null;
                                MaintRepeatCheckBoxList.SelectedItem = null;
                            }
                            else if (MaintRepeatRadioButtonList.SelectedItem.Value == "3")
                            {
                                reci.Type = RecurrenceType.Weekly;
                                //Mukund 07Aug14, VSPLUS-874 Maintenance window redirects to the Error page when switching from Monthly to Daily repeat option
                                MaintRepeatMonthlyComboBox.SelectedItem = null;
                                
                                for (int i = 0; i < MaintRepeatCheckBoxList.SelectedItems.Count; i++)
                                {
                                    if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "1")
                                    {

                                        reci.WeekDays = WeekDays.Monday;
                                    }
                                    else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "2")
                                    {
                                        //reci.WeekOfMonth = WeekOfMonth.First;
                                        reci.WeekDays = WeekDays.Tuesday;
                                    }
                                    else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "3")
                                    {
                                        // reci.WeekOfMonth = WeekOfMonth.First;
                                        reci.WeekDays = WeekDays.Wednesday;
                                    }
                                    else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "4")
                                    {
                                        reci.WeekOfMonth = WeekOfMonth.First;
                                        reci.WeekDays = WeekDays.Thursday;
                                    }
                                    else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "5")
                                    {
                                        //reci.WeekOfMonth = WeekOfMonth.First;
                                        reci.WeekDays = WeekDays.Friday;
                                    }
                                    else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "6")
                                    {
                                        // reci.WeekOfMonth = WeekOfMonth.First;
                                        reci.WeekDays = WeekDays.Saturday;
                                    }
                                    else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "7")
                                    {
                                        //reci.WeekOfMonth = WeekOfMonth.First;
                                        reci.WeekDays = WeekDays.Sunday;
                                    }
                                }
                                //12/12/2013 NS added to avoid YSD
                                if (MaintRepeatWeeksTextBox.Text == null || MaintRepeatWeeksTextBox.Text == "" || Convert.ToInt32(MaintRepeatWeeksTextBox.Text) < 1)
                                {
                                    MaintRepeatWeeksTextBox.Text = "1";
                                }
                            }
                            else if (MaintRepeatRadioButtonList.SelectedItem.Value == "4")
                            {
                                reci.Type = RecurrenceType.Monthly;
                                //Mukund 07Aug14, VSPLUS-874 Maintenance window redirects to the Error page when switching from Monthly to Daily repeat option
                                //8/18/2014 NS commented out the line below - setting the value to null does not allow
                                //any selection of days to stick thus completely disabling users from saving a window
                                //MaintRepeatCheckBoxList.SelectedItem = null;
                            }
                            if (MaintRepeatMonthlyComboBox.SelectedItem != null)
                            {
                                if (MaintRepeatMonthlyComboBox.SelectedItem.Value == "1")
                                {
                                    for (int i = 0; i < MaintRepeatCheckBoxList.SelectedItems.Count; i++)
                                    {
                                        if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "1")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Monday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "2")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Tuesday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "3")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Wednesday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "4")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Thursday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "5")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Friday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "6")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Saturday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "7")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Sunday;
                                        }
                                    }

                                }
                                else if (MaintRepeatMonthlyComboBox.SelectedItem.Value == "2")
                                {
                                    for (int i = 0; i < MaintRepeatCheckBoxList.SelectedItems.Count; i++)
                                    {
                                        if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "1")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Monday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "2")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Tuesday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "3")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Wednesday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "4")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Thursday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "5")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Friday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "6")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Saturday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "7")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Sunday;
                                        }
                                    }
                                }
                                else if (MaintRepeatMonthlyComboBox.SelectedItem.Value == "3")
                                {
                                    for (int i = 0; i < MaintRepeatCheckBoxList.SelectedItems.Count; i++)
                                    {
                                        if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "1")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Monday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "2")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Tuesday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "3")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Wednesday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "4")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Thursday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "5")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Friday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "6")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Saturday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "7")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Sunday;
                                        }
                                    }

                                }
                                else if (MaintRepeatMonthlyComboBox.SelectedItem.Value == "4")
                                {
                                    for (int i = 0; i < MaintRepeatCheckBoxList.SelectedItems.Count; i++)
                                    {
                                        if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "1")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Monday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "2")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Tuesday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "3")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Wednesday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "4")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Thursday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "5")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Friday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "6")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Saturday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "7")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Sunday;
                                        }
                                    }

                                }
                                else if (MaintRepeatMonthlyComboBox.SelectedItem.Value == "5")
                                {
                                    for (int i = 0; i < MaintRepeatCheckBoxList.SelectedItems.Count; i++)
                                    {
                                        if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "1")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Monday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "2")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Tuesday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "3")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Wednesday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "4")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Thursday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "5")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Friday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "6")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Saturday;
                                        }
                                        else if (MaintRepeatCheckBoxList.SelectedItems[i].Value == "7")
                                        {
                                            reci.WeekOfMonth = WeekOfMonth.First;
                                            reci.WeekDays = WeekDays.Sunday;
                                        }
                                    }

                                }
                            }

                            OccurrenceCalculator calc = OccurrenceCalculator.CreateInstance(reci);
                            TimeInterval ttc = new TimeInterval(reci.Start, reci.End + new TimeSpan(1, 0, 0));


                            var bcoll = calc.CalcOccurrences(ttc, apt);
                            if (bcoll.Count != 0)
                            {
                                reci.OccurrenceCount = bcoll.Count;
                            }
                            else
                            {
                                reci.OccurrenceCount = 1;
                            }
                            reci.Range = RecurrenceRange.OccurrenceCount;
                            reci.EndUpdate();
                            string s = reci.ToXml();
                            //Build a list of maintenance days based on the Repeat round panels values
                            switch (MaintType)
                            {
                                //One Time
                                case "1":
                                    break;
                                //Daily
                                case "2":
                                    for (int i = 0; i < MaintRepeatCheckBoxList.Items.Count; i++)
                                    {
                                        if (MaintRepeatCheckBoxList.Items[i].Selected)
                                        {
                                            if (MaintDaysList == "")
                                            {
                                                MaintDaysList += MaintRepeatCheckBoxList.Items[i].Value.ToString();
                                            }
                                            else
                                            {
                                                MaintDaysList += "," + MaintRepeatCheckBoxList.Items[i].Value.ToString();
                                            }
                                        }
                                    }
                                    break;
                                //Weekly
                                case "3":
                                    //8/6/2014 NS modified
                                    string MaintRepeatWeeks = MaintRepeatWeeksTextBox.Text;
                                    isnumeric = true;
                                    int weekno = 0;
                                    try
                                    {
                                        weekno = int.Parse(MaintRepeatWeeksTextBox.Text);
                                    }
                                    catch
                                    {
                                        isnumeric = false;
                                    }
                                    if (MaintRepeatWeeksTextBox.Text == "" || !isnumeric || weekno == 0)
                                    {
                                        maintOK = false;
                                        MsgLabel.Text = "You must enter a numeric value into the 'Every ... weeks on' field when scheduling a weely maintenance window.";
                                    }
                                    if (MaintRepeatCheckBoxList.SelectedItems.Count > 0)
                                    {
                                        for (int i = 0; i < MaintRepeatCheckBoxList.Items.Count; i++)
                                        {
                                            if (MaintRepeatCheckBoxList.Items[i].Selected)
                                            {
                                                if (MaintDaysList == "")
                                                {
                                                    MaintDaysList += MaintRepeatCheckBoxList.Items[i].Value.ToString() + ":" + MaintRepeatWeeks;
                                                }
                                                else
                                                {
                                                    MaintDaysList += "," + MaintRepeatCheckBoxList.Items[i].Value.ToString() + ":" + MaintRepeatWeeks;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        maintOK = false;
                                        MsgLabel.Text = "You must select at least one day of the week when scheduling weekly maintenance windows. Please make your selection on the 'Repeat' panel.";
                                    }
                                    break;
                                //Monthly
                                case "4":
                                    string MaintRepeatMonthlyVal = "";
                                    string MaintRepeatMonthlyTxt = "";
                                    if (MaintRepeatMonthlyComboBox.SelectedIndex != -1)
                                    {
                                        MaintRepeatMonthlyVal = MaintRepeatMonthlyComboBox.SelectedItem.Value.ToString();
                                        MaintRepeatMonthlyTxt = MaintRepeatMonthlyComboBox.SelectedItem.Text;
                                        if (MaintRepeatMonthlyVal != "5")
                                        {
                                            //8/6/2014 NS modified
                                            if (MaintRepeatCheckBoxList.SelectedItems.Count > 0)
                                            {
                                                for (int i = 0; i < MaintRepeatCheckBoxList.Items.Count; i++)
                                                {
                                                    if (MaintRepeatCheckBoxList.Items[i].Selected)
                                                    {
                                                        if (MaintDaysList == "")
                                                        {
                                                            MaintDaysList += MaintRepeatCheckBoxList.Items[i].Value.ToString() + ":" + MaintRepeatMonthlyTxt;
                                                        }
                                                        else
                                                        {
                                                            MaintDaysList += "," + MaintRepeatCheckBoxList.Items[i].Value.ToString() + ":" + MaintRepeatMonthlyTxt;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                maintOK = false;
                                                MsgLabel.Text = "You must select at least one day of the week when scheduling monthly maintenance windows. Please make your selection on the 'Repeat' panel.";
                                            }
                                        }
                                        //Monthly on specific day of the month
                                        else
                                        {
                                            MaintDaysList = MaintRepeatDayTextBox.Text;
                                            //8/6/2014 NS added
                                            isnumeric = true;
                                            int dayofmonth = 0;
                                            try
                                            {
                                                dayofmonth = int.Parse(MaintRepeatDayTextBox.Text);
                                            }
                                            catch
                                            {
                                                isnumeric = false;
                                            }
                                            if (MaintRepeatDayTextBox.Text == "" || !isnumeric || dayofmonth == 0 || dayofmonth > 28)
                                            {
                                                maintOK = false;
                                                MsgLabel.Text = "You must enter a numeric value into the 'Day of the month' field when scheduling a monthly maintenance window on a specific day. Please note that not all months have the 29th, 30th, or 31st so enter the value accordingly.";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        maintOK = false;
                                        MsgLabel.Text = "You must select an option from the 'Repeat every month on' drop down list when scheduling monthly maintenance windows. Please make your selection on the 'Repeat' panel.";
                                    }
                                    break;
                                default:
                                    break;
                            }
                            string EndDateIndicator = "";
                            //Set EndDateIndicator value to 0 or 1 based on other selections
                            //9/17/2013 NS added ServerTypeIDs in order to be able to insert into the ServerMaintenance table
                            //List<Object> selectItemsServerTypeID = MaintSrvGridView.GetSelectedFieldValues("ServerTypeID");

                            //MD 30Jan14

                            //foreach (object selectItemServerTypeID in selectItemsServerTypeID)
                            //{
                            //    servertypeIDValues.Add(selectItemServerTypeID.ToString());
                            //}
                            //List<Object> selectItemsServerID = MaintSrvGridView.GetSelectedFieldValues("ID");
                            //foreach (object selectItemServerID in selectItemsServerID)
                            //{
                            //    serverIDValues.Add(selectItemServerID.ToString());
                            //}

                            DataTable dt = GetSelectedServers(0);
                            //11/30/2015 NS modified for VSPLUS-2227
                            /*
							if (dt.Rows.Count == 0)
							{
								maintOK = false;
								MsgLabel.Text = "Please select at least one server for Maintenance.";
							}
                             */
							
                            List<string> selectItemsServerTypeID = dt.AsEnumerable().Select(x => x[3].ToString()).ToList();
                            foreach (object selectItemServerTypeID in selectItemsServerTypeID)
                            {
                                servertypeIDValues.Add(selectItemServerTypeID.ToString());
                            }
                            List<string> selectItemsServerID = dt.AsEnumerable().Select(x => x[1].ToString()).ToList();
                            foreach (object selectItemServerID in selectItemsServerID)
                            {
                                serverIDValues.Add(selectItemServerID.ToString());
                            }
                            List<string> selectItemsDeviceID = dt.AsEnumerable().Select(x => x[1].ToString()).ToList();
                            foreach (object selectItemServerID in selectItemsServerID)
                            {
                                deviceIDValues.Add("NULL");
                            }

                            //11/30/2015 NS added for VSPLUS-2227
                            DataTable dt2 = GetSelectedKeyUsers(0);
                            if (dt2.Rows.Count == 0 && dt.Rows.Count == 0)
                            {
                                maintOK = false;
                                MsgLabel.Text = "Please select at least one server or one key user in order to save this maintenance window.";
                            }
                            if (dt2.Rows.Count > 0)
                            {
                                selectItemsServerTypeID = dt2.AsEnumerable().Select(x => x[3].ToString()).ToList();
                                foreach (object selectItemServerTypeID in selectItemsServerTypeID)
                                {
                                    servertypeIDValues.Add(selectItemServerTypeID.ToString());
                                }
                                selectItemsServerID = dt2.AsEnumerable().Select(x => x[1].ToString()).ToList();
                                foreach (object selectItemServerID in selectItemsServerID)
                                {
                                    serverIDValues.Add(selectItemServerID.ToString());
                                }
                                selectItemsDeviceID = dt2.AsEnumerable().Select(x => x[2].ToString()).ToList();
                                foreach (object selectItemDeviceID in selectItemsDeviceID)
                                {
                                    deviceIDValues.Add(selectItemDeviceID.ToString());
                                }
                            }
                            
                            //8/6/2014 NS modified
                            bool update = false;
                            if (maintOK == true){
                                //12/7/2015 NS modified for VSPLUS-2227
                               update = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.UpdateMaintenanceWindows(MaintKey, Name, StartDate, StartTime, Duration,
                                  EndDate, MaintType, MaintDaysList, EndDateIndicator, serverIDValues, s, rem, 1, true, servertypeIDValues, copy,
                                  Durationtype, deviceIDValues);
                            }
                            else if (maintOK == false)
                            {
                                //11/13/2015 NS modified
                                /*
                                MaintenancePopupControl.ShowOnPageLoad = true;
                                MaintenancePopupControl.HeaderText = "Validation Error";
                                 */
                                errorDiv.Style.Value = "display: block";
                                errorDiv.InnerHtml = MsgLabel.Text +
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            }

                            if (update == true)
                            {
                                ReturnURL();

                            }
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        //6/27/2014 NS added for VSPLUS-634
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        throw ex;
                    }
                }
            }
            
        }

        private DataTable GetSelectedServers(int AlertKey)
        {

            DataTable dtSel = new DataTable();
            try
            {
                dtSel.Columns.Add("AlertKey");
                dtSel.Columns.Add("ServerID");
                dtSel.Columns.Add("LocationID");
                //11/26/2013 NS added
                dtSel.Columns.Add("ServerTypeID");

                //string selValues = "";
                TreeListNodeIterator iterator = ServersTreeList.CreateNodeIterator();
                TreeListNode node;

                TreeListColumn columnActid = ServersTreeList.Columns["actid"];
                TreeListColumn columnSrvId = ServersTreeList.Columns["LocId"];
                TreeListColumn columnTbl = ServersTreeList.Columns["tbl"];
                //11/26/2013 NS added
                TreeListColumn columnSrvTypeId = ServersTreeList.Columns["srvtypeid"];
                while (true)
                {
                    node = iterator.GetNext();

                    if (node == null) break;
                    if (node.Level == 1 && node.ParentNode.Selected)
                    {
                        //COMMENTED BY MUKUND FOR VSPLUS-605
                        //// root node selected ie All Servers selected
                        //DataRow dr = dtSel.NewRow();
                        //dr["AlertKey"] = AlertKey;
                        //dr["ServerID"] = 0;// node.GetValue("actid");
                        //dr["LocationID"] = 0;//node.GetValue("LocId");
                        ////11/26/2013 NS added
                        //dr["ServerTypeID"] = 0;//node.GetValue("srvtypeid");
                        //dtSel.Rows.Add(dr);
                        // break; 
                    }
                    else if (node.Level == 1 && node.ParentNode.Selected == false && node.Selected)
                    {
                        //COMMENTED BY MUKUND FOR VSPLUS-605
                        //// level 1 node selected ie One Location selected and all Servers under it

                        //DataRow dr = dtSel.NewRow();
                        //dr["AlertKey"] = AlertKey;
                        //dr["ServerID"] = 0;//node.GetValue("actid");
                        //dr["LocationID"] = ((System.Data.DataRowView)(node.DataItem)).Row.ItemArray[3];// node.GetValue("LocId");
                        ////11/26/2013 NS added
                        //dr["ServerTypeID"] = 0;//node.GetValue("srvtypeid");
                        //dtSel.Rows.Add(dr);
                    }
                    else if (node.Level == 2) //&& node.ParentNode.Selected == false //COMMENTED BY MUKUND FOR VSPLUS-605
                    {

                        if (node.Selected)
                        {
                            DataRow dr = dtSel.NewRow();
                            dr["AlertKey"] = AlertKey;
                            dr["ServerID"] = node.GetValue("actid");
                            dr["LocationID"] = node.GetValue("LocId");
                            //11/26/2013 NS added
                            dr["ServerTypeID"] = node.GetValue("srvtypeid");
                            dtSel.Rows.Add(dr);
                        }
                    }


                }


            }
            catch (Exception ex)
            {
                //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
                //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                //    ", Error: " + ex.ToString());
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }

            return dtSel;

        }
        //11/30/2015 NS added for VSPLUS-2227
        private DataTable GetSelectedKeyUsers(int AlertKey)
        {
            DataTable dtSel = new DataTable();
            System.Collections.Generic.List<object> devID;
            System.Collections.Generic.List<object> uName;
            try
            {
                if (KeyUsersGridView.Selection.Count > 0)
                {
                    devID = KeyUsersGridView.GetSelectedFieldValues("DeviceID");
                    uName = KeyUsersGridView.GetSelectedFieldValues("UserName");
                    dtSel.Columns.Add("AlertKey");
                    dtSel.Columns.Add("ServerID");
                    dtSel.Columns.Add("DeviceID");
                    dtSel.Columns.Add("ServerTypeID");
                    for (int i = 0; i < devID.Count; i++)
                    {
                        DataRow dr = dtSel.NewRow();
                        dr["AlertKey"] = AlertKey;
                        dr["ServerID"] = 0;
                        dr["DeviceID"] = uName[i] + "-" + devID[i];
                        dr["ServerTypeID"] = 11;
                        dtSel.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            return dtSel;
        }

        protected void MaintResetButton_Click(object sender, EventArgs e)
        {
			if (Request.UrlReferrer != null)


				//	Response.Redirect(Request.UrlReferrer, false);
			//	Response.Redirect(Request.UrlReferrer);


			Response.Redirect(Request.RawUrl); 
        }

        protected void MaintCancelButton_Click(object sender, EventArgs e)
        {
			
            ReturnURL();
			Session["MaintenanceWinList"] = "";
           
        }

        public void ReturnURL()
        {
            if (Session["ReturnUrl"]!=""&&Session["ReturnUrl"]!=null)
            {
                string URL = Session["ReturnUrl"].ToString();
                Session["ReturnUrl"] = "";
                Response.Redirect(URL);

            }
            else
            {
				Session["MaintenanceWinList"] = NameTextBox.Text;
                Response.Redirect("MaintenanceWinList.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
        
        }

        protected void OKButton_Click(object sender, EventArgs e)
        {
            MaintenancePopupControl.ShowOnPageLoad = false;
        }

        protected void MaintSrvGridView_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "__Key")
                e.Value = e.GetListSourceFieldValue("ID").ToString() + "_" + e.GetListSourceFieldValue("ServerTypeID").ToString();
        }

        protected void DurationTrackBar_PositionChanged(object sender, EventArgs e)
        {
            //lblEndTime.Text =Convert.ToString(
            //    Convert.ToDateTime(MaintStartTimeEdit.Value) + Convert.ToDateTime(DurationTrackBar.Value)) ;
            MaintDurationTextBox.Value = DurationTrackBar.Value.ToString();
            //9/3/2015 NS modified for VSPLUS-2114
            if (MaintStartTimeEdit.Value != null)
            {
                lblEndTime.Text = Convert.ToDateTime(MaintStartTimeEdit.Value.ToString()).AddMinutes(Convert.ToDouble(DurationTrackBar.Value)).ToString("h:mm tt");
            }
            lblDuration.Text = DurationHoursMins(DurationTrackBar.Value.ToString());// DurationHoursMins(MaintenanceDataTable.Rows[0]["Duration"].ToString());
            //9/16/2014 NS added for VSPLUS-911
            //9/3/2015 NS modified for VSPLUS-2114
            DateTime endDateTime;
            if (MaintStartTimeEdit.Value != null)
            {
                endDateTime = DateTime.Parse(MaintStartDateEdit.Text + " " + MaintStartTimeEdit.Text);
            }
            else
            {
                endDateTime = DateTime.Parse(MaintStartDateEdit.Text + " " + "12:00 AM");
            }
            endDateTime = endDateTime.AddMinutes(Convert.ToDouble(MaintDurationTextBox.Value));
            if (MaintRepeatRadioButtonList.SelectedItem.Value.ToString() == "1")
            {
                lblEndDate.Text = endDateTime.ToShortDateString();
                MaintEndDateEdit.Text = lblEndDate.Text;
            }
        }

      

        protected void MaintStartTimeEdit_ValueChanged(object sender, EventArgs e)
        {
            lblEndTime.Text = Convert.ToDateTime(MaintStartTimeEdit.Value.ToString()).AddMinutes(Convert.ToDouble(DurationTrackBar.Value)).ToString("h:mm tt");
            lblDuration.Text = DurationHoursMins(DurationTrackBar.Value.ToString());

        }

        public void fillServersTreeList()
        { 
            try
            {
                if (Session["DataServers"] == null)
                {
					DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
					List<string> lstResult = (from table in DataServersTree.AsEnumerable()
											  where table.Field<string>("ServerType") != null
											  select table.Field<string>("tbl")).ToList();

					var servers = from n in DataServersTree.AsEnumerable()
								  where n.Field<string>("tbl").Contains("Servers")
								  select n;
					var Locations = from n in DataServersTree.AsEnumerable()
									where n.Field<string>("tbl").Contains("Locations")
									select n;
					DataTable filteredDT = null;
					DataTable Serversdt = null;
					DataTable Locationsdt = null;
					DataTable sum = new DataTable();


					if ((servers != null && servers.Count() > 0) && (Locations != null && Locations.Count() > 0))
					{
						Serversdt = servers.CopyToDataTable();
						Locationsdt = Locations.CopyToDataTable();
						sum = Locationsdt.Copy();
						sum.Merge(Serversdt);
						var customers = sum.AsEnumerable();
						var orders = Locationsdt.AsEnumerable();
						var LocationIdNotNull = from n in sum.AsEnumerable()
												where !n.IsNull("LocId")
												select n;
						filteredDT = LocationIdNotNull.CopyToDataTable();
						DataTable filterbylocation = (from n in Locationsdt.AsEnumerable()
													  join prod in filteredDT.AsEnumerable() on n.Field<int>("id") equals prod.Field<int>("LocId")
													  select n).Distinct().CopyToDataTable();
						DataTable Filteredservers = new DataTable();
						Filteredservers = filterbylocation.Copy();
						Filteredservers.Merge(filteredDT);
						Session["DataServers"] = Filteredservers;

					}
                }
				
				
                ServersTreeList.DataSource = (DataTable)Session["DataServers"];
                ServersTreeList.DataBind();
                //int AlertKey = 0;
                DataTable dtSer = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.GetServers(MaintKey);//GetSelectedServers(int.Parse(MaintKey));
                if (MaintKey != null && MaintKey != "")
                {
                    DataTable dtSel = dtSer.Select("MaintID=" + MaintKey).CopyToDataTable();
                    if (dtSel.Rows.Count > 0)
                    {
                        TreeListNodeIterator iterator = ServersTreeList.CreateNodeIterator();
                        TreeListNode node;
                        for (int i = 0; i < dtSel.Rows.Count; i++)
                        {
                            if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == 0 && Convert.ToInt32(dtSel.Rows[i]["LocationID"]) == 0)
                            {
                                //select all
                                while (true)
                                {
                                    node = iterator.GetNext();
                                    if (node == null) break;
                                    node.Selected = true;
                                }
                            }
                            else if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == 0 && (Convert.ToInt32(dtSel.Rows[i]["LocationID"]) != 0))
                            {
                                //parent selected
                                while (true)
                                {
                                    node = iterator.GetNext();
                                    if (node == null) break;
                                    if ((Convert.ToInt32(dtSel.Rows[i]["LocationID"]) == Convert.ToInt32(node.GetValue("actid"))) && node.GetValue("tbl").ToString() == "Locations")
                                    {
                                        node.Selected = true;
                                    }
                                    else if (node.GetValue("LocId").ToString() != "")
                                    {
                                        if ((Convert.ToInt32(dtSel.Rows[i]["LocationID"]) == Convert.ToInt32(node.GetValue("LocId"))) && node.GetValue("tbl").ToString() != "Locations")
                                        {
                                            node.Selected = true;
                                        }
                                    }
                                }
                            }
                            else if (Convert.ToInt32(dtSel.Rows[i]["ServerID"]) != 0 && (Convert.ToInt32(dtSel.Rows[i]["LocationID"]) != 0))
                            {
                                //specific selected
                                while (true)
                                {

                                    node = iterator.GetNext();
                                    if (node == null) break;
                                    //11/25/2013 NS modified - selection is loaded incorrectly when servers and URLs are selected
                                    if ((Convert.ToInt32(dtSel.Rows[i]["ServerID"]) == Convert.ToInt32(node.GetValue("actid"))) &&
                                        node.GetValue("tbl").ToString() != "Locations")
                                    {
                                        if (node.GetValue("LocId") != null)
                                        {
                                            if ((Convert.ToInt32(dtSel.Rows[i]["LocationID"]) == Convert.ToInt32(node.GetValue("LocId"))))
                                            {
                                                if (Convert.ToInt32(dtSel.Rows[i]["ServerTypeId"]) == Convert.ToInt32(node.GetValue("srvtypeid")))
                                                {
                                                    node.Selected = true;
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                            iterator.Reset();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
                //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                //    ", Error: " + ex.ToString());
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        public void fillServersTreefromSession()
        {
            DataTable DataServers = new DataTable();
            try
            {
                if (Session["DataServers"] != "" && Session["DataServers"] != null)
                    DataServers = Session["DataServers"] as DataTable;
                if (DataServers.Rows.Count > 0)
                {
                    DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["ID"] };
                }
                ServersTreeList.DataSource = DataServers;
                ServersTreeList.DataBind();

            }
            catch (Exception ex)
            {
                //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
                //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                //    ", Error: " + ex.ToString());
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }

        protected void CollapseAllSrvButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (CollapseAllSrvButton.Text == "Collapse All")
                {
                    ServersTreeList.CollapseAll();
                    CollapseAllSrvButton.Image.Url = "~/images/icons/add.png";
                    CollapseAllSrvButton.Text = "Expand All";
                }
                else
                {
                    ServersTreeList.ExpandAll();
                    CollapseAllSrvButton.Image.Url = "~/images/icons/forbidden.png";
                    CollapseAllSrvButton.Text = "Collapse All";
                }
            }
            catch (Exception ex)
            {
                //Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
                //    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                //    ", Error: " + ex.ToString());
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }
        protected void RadioButtonListEndDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MaintRepeatCheckBoxList.Items.cou
            //9/15/2014 NS modified for VSPLUS-911
            DurationSelect(true);
        }
        protected void DurationSelect(bool clickevent)
        {
            if (RadioButtonListEndDate.SelectedIndex == 1)
            {
                DateTime dt = DateTime.Today.AddYears(10);
                ASPxLabelEndDate.Visible = false;
                MaintEndDateEdit.Visible = false;
                //dt.AddYears(-1);
                MaintEndDateEdit.Text = dt.ToShortDateString();
            }
            else
            {
                //9/15/2014 NS modified for VSPLUS-911
                //MaintEndDateEdit.Text = DateTime.Today.ToShortDateString();
                if (clickevent || MaintEndDateEdit.Text == "")
                {
                    MaintEndDateEdit.Text = DateTime.Today.ToShortDateString();
                }
                ASPxLabelEndDate.Visible = true;
                MaintEndDateEdit.Visible = true;
            }
            //9/9/2014 NS added for VSPLUS-911
            if (RadioButtonListEndDate.SelectedItem.Value.ToString() == "2")
            {
                ASPxLabelEndDate.Visible = false;
                ASPxLabelEndDate.ClientVisible = false;
                MaintEndDateEdit.Visible = false;
                MaintEndDateEdit.ClientVisible = false;
                lblEndDate.Visible = false;
                lblEndDate.ClientVisible = false;
            }
            else
            {
                ASPxLabelEndDate.Visible = true;
                ASPxLabelEndDate.ClientVisible = true;
                MaintEndDateEdit.Visible = true;
                MaintEndDateEdit.ClientVisible = true;
                lblEndDate.Visible = false;
                lblEndDate.ClientVisible = false;
            }
        }
        //protected void SelectAllCheckBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    //MaintRepeatCheckBoxList.Items[1].se
        //    if (SelectAllCheckBox.Checked)
        //    {
        //        MaintRepeatCheckBoxList.SelectAll();
        //    }
        //    else
        //    {
        //        MaintRepeatCheckBoxList.UnselectAll();
        //    }
        //}
        //protected void MaintDurationTextBox_TextChanged(object sender, EventArgs e)
        //{
        //    DurationTrackBar.Value = MaintDurationTextBox.Text;
        //    DurationTrackBar.Position = Convert.ToInt32(MaintDurationTextBox.Text);
        //}
        protected string DurationHoursMins(string period)
        {
            int time = 0;
            time = Convert.ToInt32(period);
            //ArrayList duration = new ArrayList();
            string duration = "";
            //duration.Add(time / 60);
            //duration.Add(time % 60);
            if ((time / 60) > 0)
            {
                if (time % 60 > 0)
                {
                    duration = (time / 60) + " hour(s)  " + (time % 60) + " minutes";
                }
                else
                {
                    duration = (time / 60) + " hour(s)";
                }
            }
            else
            {
                duration = (time % 60) + " minutes";
            } 
            return duration;
        }

        protected void MaintSrvGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MaintenanceWin|MaintSrvGridView", MaintSrvGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void DataBound(object sender, EventArgs e)
        {
            SetItemCount();

        }
        void SetItemCount()
        {


            int itemCount = ServersTreeList.Nodes.OfType<TreeListNode>().Select(x => x.ChildNodes.Count).Sum();      // int itemCount = (int)ServersTreeList.GetSummaryValue()

            ServersTreeList.SettingsPager.Summary.Text = "Page {0} of {1} (" + itemCount + " items)";
        }
    }
}