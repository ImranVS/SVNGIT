using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using System.Drawing;
using DevExpress.Web;

namespace VSWebUI
{
    public partial class TravelerUsersDevices : System.Web.UI.Page
    {
        string value;
          
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !IsCallback)
            {
                UsersGrid.FocusedRowIndex = -1;
                SetGrid(0, 0);
                SetGraphForDeviceType("");
            }
            else
            {
                FillUserGridFromSession();
                if (Session["value"] != "" && Session["value"] != null)
                {
                    SetGraphForDeviceType(Session["value"].ToString());
                    Session["value"] = "";
                }
                else
                {
                    if (travelerButtonList.Items[0].Selected == true)
                    SetGraphForDeviceType("");
                   // if (travelerButtonList.Items[1].Selected == true)

                }
            }
        }
        public DataTable SetGrid(int lastmin,int agomin)
        {
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGrid(lastmin, agomin);
            UsersGrid.DataSource = dt;

            UsersGrid.DataBind();

            Session["Fillgid"] = dt;

            return dt;
        }

        public void FillUserGridFromSession()
        {
            if (Session["Fillgid"] != "" && Session["Fillgid"] != null)
            {
                UsersGrid.DataSource = (DataTable)Session["Fillgid"];
                UsersGrid.DataBind();
            }
        }
        public DataTable SetGraphForDeviceType(string servername)
        {
            deviceTypeWebChart.Series.Clear();
            DataTable dt = VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SetGraphForDeviceType(servername);


            Series series = new Series("DeviceType", ViewType.Bar);

            series.ArgumentDataMember = dt.Columns["DeviceName"].ToString();

            ValueDataMemberCollection seriesValueDataMembers = (ValueDataMemberCollection)series.ValueDataMembers;
            seriesValueDataMembers.AddRange(dt.Columns["No_of_Users"].ToString());
            deviceTypeWebChart.Series.Add(series);

            deviceTypeWebChart.Legend.Visible = false;

            ((SideBySideBarSeriesView)series.View).ColorEach = true;

            XYDiagram seriesXY = (XYDiagram)deviceTypeWebChart.Diagram;
            seriesXY.AxisX.Title.Text = "Device Type";
            seriesXY.AxisX.Title.Visible = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            seriesXY.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            seriesXY.AxisY.Title.Text = "Users";
            seriesXY.AxisY.Title.Visible = true;

            AxisBase axisy = ((XYDiagram)deviceTypeWebChart.Diagram).AxisY;
            axisy.Range.AlwaysShowZeroLevel = false;

            deviceTypeWebChart.DataSource = dt;
            deviceTypeWebChart.DataBind();

            return dt;
        }

        protected void UsersGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            int index = UsersGrid.FocusedRowIndex;
            if (e.VisibleIndex != index)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            }
        }

        protected void travelerButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (travelerButtonList.Items[0].Selected == true)
            {
                minutesagoTextBox.Visible = false;
                MinutesagoLabel.Visible = false;
                lastminutesTextBox.Visible = false;
                LastminutesLabel.Visible = false;
                SubButton.Visible = false;
                SetGrid(0, 0);
                minutesagoTextBox.Text = "0";
                lastminutesTextBox.Text = "0";

            }
            else if (travelerButtonList.Items[1].Selected == true)
            {
                minutesagoTextBox.Text = "0";
                lastminutesTextBox.Text = "0";
                lastminutesTextBox.Visible = true;
                LastminutesLabel.Visible = true;
                minutesagoTextBox.Visible = false;
                MinutesagoLabel.Visible = false;
                SubButton.Visible = true;
            }
            else if(travelerButtonList.Items[2].Selected==true)
            {
                minutesagoTextBox.Text = "0";
                lastminutesTextBox.Text = "0";
                minutesagoTextBox.Visible = true;
                MinutesagoLabel.Visible = true;
                lastminutesTextBox.Visible = false;
                LastminutesLabel.Visible = false;
                SubButton.Visible = true;
            }

        }

        protected void TravelerusersPopupMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
        {
           
            if (e.Item.Name == "DenyAccess")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "Deny";
                    DenyAccess();
                }
                else
                {
                    msgPopupControl.ShowOnPageLoad = true;
                    msgLabel.Text = "Please Select Device in Grid";
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Text = "OK";
                }
            }
            if (e.Item.Name == "WipeDevice")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Text = "Cancel";
                        Session["MenuItem"] = "wipe";

                    //  WipeDevice Form Code 

                        string myServerName = "";
                        string wipeSupported = "";
                        string myUserName = "";
                        string myDeviceName = "";
                        string myDeviceID = "";

                        HardCheckBox.Enabled = true;
                        TravelerAppCheckBox.Enabled = true;
                        StorageCadrCheckBox.Enabled = true;
                        DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
                        try
                        {
                            wipeSupported = myRow["wipeSupported"].ToString();
                            myServerName = myRow["ServerName"].ToString();
                            myUserName = myRow["UserName"].ToString();
                           // Session["myUserName"] = myUserName;
                          //  myServerName = myRow["ServerName"].ToString();
                            Session["myServerName"] = myServerName;
                            myDeviceName = myRow["DeviceName"].ToString();
                           // Session["myDeviceName"] = myDeviceName;
                            myDeviceID = myRow["DeviceID"].ToString();

                            UserNameLabel.Text = myUserName;
                            DeviceNameLabel.Text = myDeviceName;
                            DeviceIDLabel.Text = myDeviceID;

                        }
                        catch (Exception)
                        {
                            wipeSupported = "1";
                        }
                        switch (wipeSupported)
                        {
                            case "1":
                                break;
                            case "2":
                                HardCheckBox.Enabled = false;
                                break;
                            case "3":
                                 HardCheckBox.Enabled = true;
                                 TravelerAppCheckBox.Enabled = false;
                                 StorageCadrCheckBox.Enabled = false;
                                  break;
                            case "4":
                                HardCheckBox.Enabled = false;
                                TravelerAppCheckBox.Enabled = true;
                                StorageCadrCheckBox.Enabled = false;
                                break;
                            case "5":
                                  HardCheckBox.Enabled = true;
                                TravelerAppCheckBox.Enabled = true;
                                StorageCadrCheckBox.Enabled = false;
                                break;
                            default:
                                    HardCheckBox.Enabled = true;
                                    TravelerAppCheckBox.Enabled = true;
                                    StorageCadrCheckBox.Enabled = true;
                                    break;
                        }

                        WipePopupControl.ShowOnPageLoad = true;
                }
                else
                {
                    msgLabel.Text = "Please Select Device in Grid";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Text = "OK";
                }

               
            }
            if (e.Item.Name == "ClearWipeRequest")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Text = "Cancel";
                  
                        Session["MenuItem"] = "Clearwipe";
                        ClearWipe();
                }
                else
                {
                    msgLabel.Text = "Please Select Device in Grid";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Text = "OK";
                }
               
            }
            if (e.Item.Name == "ChangeApproval-Deny")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "changeDeny";
                    changeDeny();
                }
                else
                {
                    msgLabel.Text = "Please Select Device in Grid";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Text = "OK";
                }
               

            }
            if (e.Item.Name == "ChangeApproval-Approve")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "ChangeApprove";
                    ChangeApprove();
                }
                else
                {
                    msgLabel.Text = "Please Select Device in Grid";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Text = "OK";
                }

               
            }
            if (e.Item.Name == "LogLevel-DisableFinest")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "LogDisable";
                    LogDisable();
                }
                else
                {
                    msgLabel.Text = "Please Select Device in Grid";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Text = "OK";
                }
             
            }
            if (e.Item.Name == "LogLevel-EnableFinest")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "LogEnable";
                    LogEnable();
                }
                else
                {
                    msgLabel.Text = "Please Select Device in Grid";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Text = "OK";
                }
             
            }
            if (e.Item.Name == "LogLevel-CreateDumpFile")
            {
                if (UsersGrid.FocusedRowIndex > -1)
                {
                    YesButton.Visible = true;
                    NOButton.Visible = true;
                    CancelButton.Text = "Cancel";
                    Session["MenuItem"] = "LogCreate";
                    LogCreate();
                }
                else
                {
                    msgLabel.Text = "Please Select Device in Grid";
                    msgPopupControl.ShowOnPageLoad = true;
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Text = "OK";
                }
             
            }
            SetGraphForDeviceType(value);
        }

        protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (!IsPostBack)
            {
                SetGraphForDeviceType("");
            }
            else
            {
                int index = UsersGrid.FocusedRowIndex;
                if (index > -1)
                {
                    value = UsersGrid.GetRowValues(index, "DeviceName").ToString();
                    Session["value"] = value;
                    SetGraphForDeviceType(value);
                }
            
            }
        }

        protected void SubButton_Click(object sender, EventArgs e)
        {
            UsersGrid.FocusedRowIndex = -1;
            if(travelerButtonList.Items[1].Selected==true)
            {
                 SetGrid(Convert.ToInt32(lastminutesTextBox.Text),0);
                 UsersGrid.FocusedRowIndex = -1;
            }

            if (travelerButtonList.Items[2].Selected == true)
            {
                SetGrid(0, Convert.ToInt32(minutesagoTextBox.Text));
                UsersGrid.FocusedRowIndex = -1;
            }
            //SetGraphForDeviceType(value);
        }

        public void DenyAccess()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";   
     
       DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
       try 
	       {      
		  
            myUserName = myRow["UserName"].ToString();
            Session["myUserName"] = myUserName;
            myServerName = myRow["ServerName"].ToString();
            Session["myServerName"] = myServerName;
            myDeviceName =  myRow["DeviceID"].ToString();
            Session["myDeviceName"] = myDeviceName;
            TellCommand = "Tell Traveler Security flagsAdd lock " + myDeviceName + " " + myUserName;
            Session["TellCommand"] = TellCommand;
	            }
	catch (Exception)
	{
		 myUserName = "";		
	}
    
    msgPopupControl.ShowOnPageLoad=true;
    msgLabel.Text="Are you sure you want to deny access to " + myUserName + " on device " + myDeviceName +"?";                      

        }
        public void ClearWipe()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";
          string  myDeviceTitle = "";
            DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
              try 
	       {      
		  
             myUserName = myRow["UserName"].ToString();
            Session["myUserName"] = myUserName;
            myServerName = myRow["ServerName"].ToString();
           Session["myServerName"] = myServerName;
            myDeviceName =  myRow["DeviceID"].ToString();
          //  Session["myDeviceName"] = myDeviceName;
            myDeviceTitle = myRow["DeviceID"].ToString();
           // Session["myDeviceTitle"] = myDeviceName;
            TellCommand = "Tell Traveler Security flagsRemove all " + myDeviceName + " " + myUserName;
            Session["TellCommand"] = TellCommand;
           }
              catch (Exception)
              {
                  myUserName = "";
              }
              msgPopupControl.ShowOnPageLoad = true;
              msgLabel.Text = "Try to restore access to " + myUserName + " on device " + myDeviceTitle + "?" + "\n" + "Note that this will ONLY work if the device has not already been wiped.";
        }
        public void changeDeny()
        {

            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

            DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
            try
            {

                myUserName = myRow["UserName"].ToString();
               // Session["myUserName"] = myUserName;
                myServerName = myRow["ServerName"].ToString();
               // Session["myServerName"] = myServerName;
                myDeviceName = myRow["DeviceID"].ToString();
               // Session["myDeviceName"] = myDeviceName;
                TellCommand = "Tell Traveler Security approval deny " + myDeviceName + " " + myUserName;
               // Session["TellCommand"] = TellCommand;
                VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);
            }
            catch (Exception)
            {
                myUserName = "";
            }
                     
                      
        }
        public void ChangeApprove() 
        {

            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

            DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
            try
            {

                myUserName = myRow["UserName"].ToString();
               // Session["myUserName"] = myUserName;
                myServerName = myRow["ServerName"].ToString();
               // Session["myServerName"] = myServerName;
                myDeviceName = myRow["DeviceID"].ToString();
               // Session["myDeviceName"] = myDeviceName;
                TellCommand = "Tell Traveler Security approval approve " + myDeviceName + " " + myUserName;
               // Session["TellCommand"] = TellCommand;
                VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(myServerName, TellCommand, myUserName);
            }
            catch (Exception)
            {
                myUserName = "";
            }
                     
                      
        }
        public void LogDisable()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

            DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
            try
            {

                myUserName = myRow["UserName"].ToString();
                Session["myUserName"] = myUserName;
                myServerName = myRow["ServerName"].ToString();
                Session["myServerName"] = myServerName;
                myDeviceName = myRow["DeviceID"].ToString();
                // Session["myDeviceName"] = myDeviceName;
                TellCommand = "Tell Traveler AddUser Finest " + myUserName;
                 Session["TellCommand"] = TellCommand;
            }
            catch (Exception)
            {
                myUserName = "";
            }

            msgPopupControl.ShowOnPageLoad = true;
            msgLabel.Text = "Enable finest logging level for " + myUserName + "?";


        }

        public void LogEnable()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";

            DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
            try
            {

                myUserName = myRow["UserName"].ToString();
                Session["myUserName"] = myUserName;
                myServerName = myRow["ServerName"].ToString();
                 Session["myServerName"] = myServerName;
                myDeviceName = myRow["DeviceID"].ToString();
                // Session["myDeviceName"] = myDeviceName;
                TellCommand = "Tell Traveler Log RemoveUser  " + myUserName;
                 Session["TellCommand"] = TellCommand;
            }
            catch (Exception)
            {
                myUserName = "";
            }

            msgPopupControl.ShowOnPageLoad = true;
            msgLabel.Text = "Disable finest logging level for  " + myUserName + "?";


        }

        public void LogCreate()
        {
            string TellCommand = "";
            string myUserName = "";
            string myDeviceName = "";
            string myServerName = "";   
     
       DataRow myRow = UsersGrid.GetDataRow(UsersGrid.FocusedRowIndex);
       try 
	       {      
		  
            myUserName = myRow["UserName"].ToString();
            Session["myUserName"] = myUserName;
            myServerName = myRow["ServerName"].ToString();
            Session["myServerName"] = myServerName;
            myDeviceName =  myRow["DeviceID"].ToString();
            Session["myDeviceName"] = myDeviceName;
            TellCommand = "Tell Traveler Dump  " + myUserName;
            Session["TellCommand"] = TellCommand;
	            }
	catch (Exception)
	{
		 myUserName = "";		
	}
    
    msgPopupControl.ShowOnPageLoad=true;
    msgLabel.Text = "Create a dump file " + myUserName + "?"; 
                      

        }
        protected void YesButton_Click(object sender, EventArgs e)
        {
            // StatusText.Text = "Denying access to " & myUserName
            if ( Session["MenuItem"] =="Deny")
            {
                if (Session["TellCommand"] != "" && Session["myServerName"] != "" && Session["myUserName"] != "")
                {
                    msgLabel.Text = Session["TellCommand"].ToString() + "," + Session["myServerName"].ToString();
                    VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), Session["TellCommand"].ToString(), Session["myUserName"].ToString());

                } YesButton.Visible = false;
                NOButton.Visible = false;
                CancelButton.Text = "OK";
                Session["myUserName"] ="";
                     Session["myServerName"]="";
                          Session["myDeviceName"]="";
                          Session["TellCommand"] = "";


            }
            if (Session["MenuItem"] == "Clearwipe")
            {
                if (Session["myUserName"] != "" && Session["myUserName"] != null)
                {
                    msgLabel.Text = "Restoring access to" + Session["myUserName"].ToString();
                    if (Session["myServerName"] != "" && Session["TellCommand"] != "")
                        VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), Session["TellCommand"].ToString(), Session["myUserName"].ToString());
                }
                YesButton.Visible = false;
                NOButton.Visible = false;
                CancelButton.Text = "OK";
                Session["myUserName"] = "";
                Session["myServerName"] = "";
                Session["myDeviceName"] = "";
                Session["TellCommand"] = "";

            }
            if (Session["MenuItem"] == "LogDisable" || Session["MenuItem"] == "LogEnable")
            {
                if (Session["myUserName"] != "" && Session["myUserName"] != null)
                {
                    msgLabel.Text = "Updating Log Level for " + Session["myUserName"].ToString();
                    if(Session["myServerName"]!="" && Session["TellCommand"]!="")
                    VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), Session["TellCommand"].ToString(), Session["myUserName"].ToString());
                    YesButton.Visible = false;
                    NOButton.Visible = false;
                    CancelButton.Text = "OK";
                }
                Session["myUserName"] = "";
                Session["myServerName"] = "";
                Session["myDeviceName"] = "";
                Session["TellCommand"] = "";

            }
            if (Session["MenuItem"] == "LogCreate")
            {
                if (Session["myServerName"] != "" && Session["myServerName"] != null)
                    msgLabel.Text = "Creates a text file on the server such as \\data\\ibm_technical_support\traveler\\logs\\dumps\\" + Session["myUserName"].ToString() + ".timedate.log file";
                VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), Session["TellCommand"].ToString(), Session["myUserName"].ToString());
                YesButton.Visible = false;
                NOButton.Visible = false;
                CancelButton.Text = "OK";
                Session["myUserName"] = "";
                Session["myServerName"] = "";
                Session["myDeviceName"] = "";
                Session["TellCommand"] = "";
                
            }
           

        }

        protected void NOButton_Click(object sender, EventArgs e)
        {          
                if (Session["myUserName"] != "" && Session["myUserName"] != null)
                    msgLabel.Text = "No changes made to " + Session["myUserName"].ToString();
                YesButton.Visible = false;
                NOButton.Visible = false;
                CancelButton.Text = "OK";
                   Session["myUserName"] ="";
                     Session["myServerName"]="";
                          Session["myDeviceName"]="";
                          Session["TellCommand"] = "";                      
                                  
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            msgPopupControl.ShowOnPageLoad = false;
        }

        protected void WipeCancelButton_Click(object sender, EventArgs e)
        {
            WipePopupControl.ShowOnPageLoad = false;
        }

        protected void WipeButton_Click(object sender, EventArgs e)
        {
            string tellCommand = "";
            string flags = "";
            if (HardCheckBox.Checked == false && TravelerAppCheckBox.Checked == false && StorageCadrCheckBox.Checked == false)
            {
                msgPopupControl.ShowOnPageLoad = true;
                msgLabel.Text = "Please select a wipe option";
                YesButton.Visible = false;
                NOButton.Visible = false;
                CancelButton.Text = "OK";
            }
            if (TravelerAppCheckBox.Checked == true && HardCheckBox.Checked == false)
            {
                flags = "wipeApps ";
            }
            if (TravelerAppCheckBox.Checked == false && HardCheckBox.Checked == true && StorageCadrCheckBox.Checked == false)
            {
                flags = "wipeDevice ";
            }
            if (TravelerAppCheckBox.Checked == true && HardCheckBox.Checked == false && StorageCadrCheckBox.Checked == true)
            {
                flags = "wipeApps/wipeStorageCard";
            }
            if (TravelerAppCheckBox.Checked == true && HardCheckBox.Checked == true)
            {
                flags = "wipeDevice/wipeApps"; 
            }
            if (StorageCadrCheckBox.Checked == true && HardCheckBox.Checked == true)
            {
                flags = "wipeDevice/wipeStorageCard";
            }
            if (TravelerAppCheckBox.Checked == false && HardCheckBox.Checked == false && StorageCadrCheckBox.Checked == true)
            {
                flags = "wipeStorageCard ";
            }
            if (TravelerAppCheckBox.Checked == true && HardCheckBox.Checked == true && StorageCadrCheckBox.Checked == true)
            {
                flags = "wipeDevice/wipeApps/wipeStorageCard";
            }
            tellCommand += flags + DeviceIDLabel.Text + " " + UserNameLabel.Text;
            if(Session["myServerName"]!=""&& Session["myServerName"]!=null)
            VSWebBL.DashboardBL.LotusTravelerHealthBLL.Ins.SENDTravelerConsoleCommand(Session["myServerName"].ToString(), tellCommand, UserNameLabel.Text);
            WipePopupControl.ShowOnPageLoad = false;
            Session["myServerName"] = "";
        }
    }
}