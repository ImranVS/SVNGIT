using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebBL;
using System.Data;

namespace VSWebUI.Configurator
{
    public partial class ServerMaintenanceList : System.Web.UI.Page
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
                if (txtfromdate.Text != "" && txttodate.Text != "")
                {
                    FillMaintenance();
                }
                else
                {
                    Fillgrid();
                }
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "ServerMaintenanceList|maintenancegrid")
                        {
                            maintenancegrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                // Fillgridfromsession();
                if (txtfromdate.Text != "" && txttodate.Text != "")
                {
                    FillMaintenance();
                }
                else
                {
                    Fillgrid();
                }
            }

        }
        public void Fillgrid()
        {
            DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.servermaintenance(System.DateTime.Now, 0, 0);
            maintenancegrid.DataSource = dt;
            maintenancegrid.DataBind();
            Session["maitservers"] = dt;
        }
        public void Fillgridfromsession()
        {
            if (Session["maitservers"] != "" && Session["maitservers"] != null)
            {

                maintenancegrid.DataSource = (DataTable)Session["maitservers"];
                maintenancegrid.DataBind();

            }
        }



        protected void OKButton_Click(object sender, EventArgs e)
        {
            MsgPopupControl.ShowOnPageLoad = false;
        }

        protected void ASPxTimeEdit2_DateChanged(object sender, EventArgs e)
        {

        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            FillMaintenance();
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            txtfromdate.Text = "";
            txttodate.Text = "";
            ASPxTimeEdit1.Text = "";
            ASPxTimeEdit2.Text = "";
            Fillgridfromsession();
        }

        public void FillMaintenance()
        {
            string fromdate = txtfromdate.Text;
            string todate = txttodate.Text;
            // string fromtime = //txtfromtime.Text;ASPxTimeEdit1

            string fromtime = ASPxTimeEdit1.Text;
            //  string totime = txttotime.Text;
            string totime = ASPxTimeEdit2.Text;
            if (txtfromdate.Text != "" && txttodate.Text != "")
            {
                if (Convert.ToDateTime(fromdate) > Convert.ToDateTime(todate))
                {
                    MsgPopupControl.ShowOnPageLoad = true;
                    ErrmsgLabel.Text = "From Date value should be less than To Date.";
                }

                else
                {

                    if ((ASPxTimeEdit1.Text != null && ASPxTimeEdit2.Text != null) && (ASPxTimeEdit1.Text != "" && ASPxTimeEdit2.Text != ""))
                    {
                        DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbysearch(fromdate, todate, fromtime, totime);
                        maintenancegrid.DataSource = dt;
                        maintenancegrid.DataBind();
                    }
                }
            }


            else
            {
                if ((ASPxTimeEdit1.Text != null && ASPxTimeEdit2.Text != null) && (ASPxTimeEdit1.Text != "" && ASPxTimeEdit2.Text != ""))
                {
                    string fhour = fromtime.Substring(0, fromtime.IndexOf(":"));
                    string thour = totime.Substring(0, totime.IndexOf(":"));
                    int fhourInt = int.Parse(fhour);
                    int thourInt = int.Parse(thour);
                    string fminute = fromtime.Substring(3, 2);
                    string tminute = totime.Substring(3, 2);
                    int fminuteInt = int.Parse(fminute);
                    int tminuteInt = int.Parse(tminute);
                    if (fhourInt >= 24 || thourInt >= 24 || fminuteInt >= 60 || tminuteInt >= 60)
                    {
                        MsgPopupControl.ShowOnPageLoad = true;
                        ErrmsgLabel.Text = "Invalid hour/minute entry.";
                    }

                    else
                    {
                        DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbysearch(fromdate, todate, fromtime, totime);
                        maintenancegrid.DataSource = dt;
                        maintenancegrid.DataBind();
                    }
                }
            }



        }

        protected void maintenancegrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ServerMaintenanceList|maintenancegrid", maintenancegrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        //public void InMaintenance(string DeviceType, string DeviceName)
        //{
        //    // VSAdaptor objVSAdaptor = new VSAdaptor();
        //    DataSet dsMaintWindows = new DataSet();
        //    bool InMaintenanceWindow = false;
        //    string strSQL = "";

        //    try
        //    {
        //        DataTable dt = VSWebBL.ConfiguratorBL.MaintenanceBL.Ins.Getmaintenanceserversbysearch(fromdate, todate, fromtime, totime);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    //Dim b2 As New DateTimeExtensions
        //    int daynum;
        //    //daynum = Weekday(Now, FirstDayOfWeek.Monday) 'CType(DayofWeek, Integer) CHECK THIS -1
        //    //Dim MaintType As String
        //    //Dim MaintDayList As String
        //    //Dim StartDate, StartTime As DateTime
        //    //Dim MyStartTime, MyEndTime As DateTime
        //    //Dim TimeNow As DateTime = Now
        //    //Dim Wknum, Duration, StartWknum, TodayWknum As Integer
        //    //Wknum = b2.GetWeekOfMonth(Convert.ToDateTime(Now)) 'Format(Now.Date, "w")

        //    //Try
        //    //    Dim dr As DataRow
        //    //    'WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Server " & DeviceName & " has " & dsMaintWindows.Tables("MaintWindows").Rows.Count & " maintenance windows.")


        //    //    For Each dr In dsMaintWindows.Tables("MaintWindows").Rows

        //    //        Try
        //    //            MyStartTime = CType(dr.Item("StartTime"), DateTime)
        //    //            'MyEndTime = CType(dr.Item("EndTime"), DateTime)
        //    //            MaintType = dr.Item("MaintType").ToString()
        //    //            MaintDayList = dr.Item("MaintDaysList").ToString()

        //    //            StartDate = CType(dr.Item("StartDate"), DateTime)
        //    //            StartTime = CType(dr.Item("StartTime"), DateTime)
        //    //            Duration = CType(dr.Item("Duration"), Integer)

        //    //            'for case 3,to get the week nos from the start date
        //    //            StartWknum = DatePart(DateInterval.WeekOfYear, StartDate)
        //    //            TodayWknum = DatePart(DateInterval.WeekOfYear, Now)

        //    //            Select Case MaintType
        //    //                Case "1"
        //    //                    MyStartTime = StartDate + StartTime.TimeOfDay
        //    //                    MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
        //    //                    If Now >= MyStartTime And Now < MyEndTime Then
        //    //                        InMaintenanceWindow = True
        //    //                        Exit For
        //    //                    End If
        //    //                Case "2"
        //    //                    Dim MType As Array = MaintDayList.Split(",")
        //    //                    For Each i In MType
        //    //                        If daynum = i Then 'MType(i) 
        //    //                            MyStartTime = Now.Date + StartTime.TimeOfDay
        //    //                            MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
        //    //                            If Now >= MyStartTime And Now < MyEndTime Then
        //    //                                InMaintenanceWindow = True
        //    //                                Exit For
        //    //                            End If
        //    //                        End If
        //    //                    Next


        //    //                Case "3"
        //    //                    Dim wkType As Array = MaintDayList.Split(",")
        //    //                    For Each i In wkType
        //    //                        Dim WKday As Array = i.Split(":")
        //    //                        If (TodayWknum - StartWknum) Mod WKday(1) = 0 Then
        //    //                            'If Wknum = WKday(1) Then
        //    //                            If daynum = WKday(0) Then
        //    //                                MyStartTime = Now.Date + StartTime.TimeOfDay
        //    //                                MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
        //    //                                If Now >= MyStartTime And Now < MyEndTime Then
        //    //                                    InMaintenanceWindow = True
        //    //                                    Exit For
        //    //                                End If
        //    //                            End If
        //    //                        End If
        //    //                    Next

        //    //                Case "4"
        //    //                    Dim x, y As Integer
        //    //                    x = MaintDayList.IndexOf(",")
        //    //                    y = MaintDayList.IndexOf(":")
        //    //                    If x < 0 And y < 0 Then
        //    //                        If MaintDayList = Format(TimeNow, "dd") Then
        //    //                            InMaintenanceWindow = True
        //    //                            Exit For
        //    //                        End If
        //    //                    Else
        //    //                        Dim MonthType As Array = MaintDayList.Split(",")
        //    //                        For Each i In MonthType
        //    //                            Dim mnday As Array = i.Split(":")
        //    //                            If daynum = mnday(0) Then
        //    //                                ' ****************** 
        //    //                                Dim wkname As String = ""
        //    //                                Select Case Wknum
        //    //                                    Case "1"
        //    //                                        wkname = "First"
        //    //                                    Case "2"
        //    //                                        wkname = "Second"
        //    //                                    Case "3"
        //    //                                        wkname = "Third"
        //    //                                    Case Else
        //    //                                        wkname = "Last"
        //    //                                End Select

        //    //                                If wkname = mnday(1) Then
        //    //                                    MyStartTime = Now.Date + StartTime.TimeOfDay
        //    //                                    MyEndTime = MyStartTime.AddMinutes(dr.Item("Duration"))
        //    //                                    If Now >= MyStartTime And Now < MyEndTime Then
        //    //                                        InMaintenanceWindow = True
        //    //                                        Exit For
        //    //                                    End If
        //    //                                End If
        //    //                            End If

        //    //                        Next

        //    //                    End If


        //    //            End Select



        //    //        Catch ex As Exception
        //    //            'WriteDeviceHistoryEntry("Domino", DeviceName, " Error calculating maintenance window start and end times. Error: " & ex.Message)
        //    //            WriteHistoryEntry(Now.ToString & " Server " & DeviceName & " Error calculating maintenance window start and end times. Error: " & ex.Message)
        //    //            'WriteAuditEntry(" Error calculating maintenance window start and end times. Error: " & ex.Message)
        //    //        End Try
        //    //    Next

        //    //    dr = Nothing
        //    //Catch ex As Exception
        //    //    'WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " Error in maint window module: " & ex.Message)
        //    //    WriteHistoryEntry(Now.ToString & " Server " & DeviceName & " Error in maint window module: " & ex.Message)
        //    //    'WriteAuditEntry(Now.ToString & " Error in maint window module: " & ex.Message)
        //    //End Try

        //    //Try
        //    //    dsMaintWindows.Dispose()
        //    //    'myPath = Nothing
        //    //    GC.Collect()
        //    //Catch ex As Exception
        //    //    WriteHistoryEntry(Now.ToString & " Server " & DeviceName & " Error in maint window module: " & ex.Message)

        //    //End Try

        //    //'WriteDeviceHistoryEntry("Domino", DeviceName, Now.ToString & " InMaintenanceWindow:" & InMaintenanceWindow.ToString())
        //    //WriteHistoryEntry(Now.ToString & " Server " & DeviceName & " has " & dsMaintWindows.Tables("MaintWindows").Rows.Count & " maintenance windows. and is now in Maintenance = " & InMaintenanceWindow)
        //    //Return InMaintenanceWindow
        //}

    }
}


