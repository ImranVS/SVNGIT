using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebBL;
using DevExpress.Web.ASPxGauges;
using DevExpress.Web.ASPxGauges.Base;
using DevExpress.XtraGauges.Core.Model;
using System.Data;
namespace VSWebUI
{
    public partial class WebForm12 : System.Web.UI.Page
	{
		bool checks = false;
		int ServerKey;
		string flag;
		bool flags=false;
        protected void Page_Load(object sender, EventArgs e)
		{
			//ServerKey = int.Parse(Request.QueryString["Key"]);
            //Page.Title = "Business Hours";
            // ValidateDominoPasswordSettings();
            if (!IsPostBack)
			{
				
				
				
               
				//ReadBusinessHoursDetails();
			
            }


			if (Request.QueryString["Key"] != "" && Request.QueryString["Key"] != null)
			{
				flag = "Update";
				ServerKey = int.Parse(Request.QueryString["Key"]);

				if (!IsPostBack)
				{
					
					Filldata(ServerKey);
				}
			}
			
				 else
                {
                    flag = "Insert";                
                }

        }
        private void Filldata(int ServerKey)
		{
			//ServerKey = int.Parse(Request.QueryString["Key"]);
			HoursIndicator BusinessObjects = new HoursIndicator();
			HoursIndicator ReturnObject = new HoursIndicator();
			BusinessObjects.ID = ServerKey;
			
			//ReturnDSObject = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetData(DominoServersObject);
			ReturnObject = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.GetData(BusinessObjects);
            txttimezone.Text = ReturnObject.Type.ToString();
            //4/22/2015 NS added
            if (ReturnObject.Type.ToString() == "Business Hours")
            {
                txttimezone.Enabled = false;
            }
			MonCheckBox.Checked = Convert.ToBoolean(ReturnObject.IsMonday.ToString());
			TueCheckBox.Checked = Convert.ToBoolean(ReturnObject.IsTuesday.ToString());
			WedCheckBox.Checked = Convert.ToBoolean(ReturnObject.IsWednesday.ToString());
			ThusCheckBox.Checked = Convert.ToBoolean(ReturnObject.IsThursday.ToString());
			FriCheckBox.Checked = Convert.ToBoolean(ReturnObject.IsFriday.ToString());
			SatCheckBox.Checked = Convert.ToBoolean(ReturnObject.Issaturday.ToString());
			SunCheckBox.Checked = Convert.ToBoolean(ReturnObject.Issunday.ToString());
			MaintDurationTextBox.Text = ReturnObject.Duration.ToString();
			if (ReturnObject.Starttime != null && ReturnObject.Starttime != "")
			{
				DurationTrackBar.Value = ReturnObject.Duration.ToString();
			}
			txttimezone.Text = ReturnObject.Type.ToString();
			if (ReturnObject.Starttime != null && ReturnObject.Starttime != "")
			{
				MaintStartTimeEdit.Text = ReturnObject.Starttime.ToString();
			}

			if (ReturnObject.Starttime != null && ReturnObject.Starttime != "" && ReturnObject.Starttime != null && ReturnObject.Starttime != "")
			{
				lblEndTime.Text = Convert.ToDateTime(ReturnObject.Starttime.ToString()).AddMinutes(Convert.ToDouble(ReturnObject.Duration.ToString())).ToShortTimeString();
			}
    		if (MonCheckBox.Checked ==true && TueCheckBox.Checked == true && WedCheckBox.Checked == true && ThusCheckBox.Checked == true && FriCheckBox.Checked == true && SatCheckBox.Checked == true && SunCheckBox.Checked==true)
			{
				AllCheckBox.Checked = true;
			}
            //4/23/2015 NS added
            UseDaysRadioButtonList.SelectedIndex = Convert.ToInt32(ReturnObject.UseType.ToString());
		}
        protected void OKButton_Click(object sender, EventArgs e)
		{
			InsertBusinesshoursData();
			UPdateAlertDetails();
			
			if (flags == false)
			{
				Response.Redirect("BusinessHoursGrid.aspx", false);
			}
		}
		private void UPdateAlertDetails()
		{
			if (flag == "Update")
			{
				HoursIndicator Busibject = new HoursIndicator();
				Busibject.ID = ServerKey;
				if (MaintDurationTextBox.Text != null && MaintDurationTextBox.Text != "")
				{
					Busibject.Duration = Convert.ToInt32(MaintDurationTextBox.Text);
				}
				else
				{
					Busibject.Duration = null;
				}


				Busibject.Starttime = MaintStartTimeEdit.Text;
                //6/29/2015 NS added for VSPLUS-1821
                Busibject.Issunday = SunCheckBox.Checked;
                Busibject.IsMonday = MonCheckBox.Checked;
                Busibject.IsTuesday = TueCheckBox.Checked;
                Busibject.IsWednesday = WedCheckBox.Checked;
                Busibject.IsThursday = ThusCheckBox.Checked;
                Busibject.IsFriday = FriCheckBox.Checked;
                Busibject.Issaturday = SatCheckBox.Checked;
                string day = "";
                if (Busibject.Issunday)
                {
                    day += "Sunday, ";
                }
                if (Busibject.IsMonday)
                {
                    day += "Monday, ";
                }
                if (Busibject.IsTuesday)
                {
                    day += "Tuesday, ";
                }
                if (Busibject.IsWednesday)
                {
                    day += "Wednesday, ";
                }
                if (Busibject.IsThursday)
                {
                    day += "Thursday, ";
                }
                if (Busibject.IsFriday)
                {
                    day += "Friday, ";
                }
                if (Busibject.Issaturday)
                {
                    day += "Saturday";
                }
                if (day != null && day != "")
                {
                    if (day.Substring(day.Length - 1, 1) == ",")
                    {
                        day = day.Substring(0, day.Length - 1);
                    }
                }
                Busibject.Days = day;
                bool result = false;
				result = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.UpdateAlertDetails(Busibject);
			}
		}
        protected void CancelButton_Click(object sender, EventArgs e)
        {
			Response.Redirect("~/Configurator/BusinessHoursGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
	    private HoursIndicator CollectDataForBusinessHours()

		{
			try
			{

				
				HoursIndicator BusinesshoursObject = new HoursIndicator();

				if (flag == "Update")
				{
					BusinesshoursObject.ID = ServerKey;
				}
				BusinesshoursObject.Type = txttimezone.Text;
                if (MaintDurationTextBox.Text != null && MaintDurationTextBox.Text != "")
                {
                    //BusinesshoursObject.Duration = Convert.ToInt32(DurationTrackBar.Value.ToString());
                    BusinesshoursObject.Duration = Convert.ToInt32(MaintDurationTextBox.Text);
                }
                else
                {
                    BusinesshoursObject.Duration = null;
                }


                BusinesshoursObject.Starttime = MaintStartTimeEdit.Text;
            
				BusinesshoursObject.Issunday = SunCheckBox.Checked;
				BusinesshoursObject.IsMonday = MonCheckBox.Checked;
		        BusinesshoursObject.IsTuesday = TueCheckBox.Checked;
				BusinesshoursObject.IsWednesday =WedCheckBox.Checked;
				BusinesshoursObject.IsThursday = ThusCheckBox.Checked;
				BusinesshoursObject.IsFriday = FriCheckBox.Checked;
				BusinesshoursObject.Issaturday = SatCheckBox.Checked;
                BusinesshoursObject.UseType = UseDaysRadioButtonList.SelectedIndex;
				
				return BusinesshoursObject;

            }
           catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex; 
				
			}
			finally { }
				

		}
		private void InsertBusinesshoursData()
						
		{

			if (flag == "Insert")
			{

				HoursIndicator Busibject = new HoursIndicator();

				Busibject.Type = txttimezone.Text;
				
				//UrlObj.Name = NameTextBox.Text;
				//DataTable returntable = VSWebBL.ConfiguratorBL.NetworkDevicesBL.Ins.GetIPAddress(Busibject);
				DataTable returntable = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.GetName(Busibject);
				if (AllCheckBox.Checked || SunCheckBox.Checked || MonCheckBox.Checked || TueCheckBox.Checked || WedCheckBox.Checked || ThusCheckBox.Checked || FriCheckBox.Checked || SatCheckBox.Checked)
				{
					checks = true;

				}
				
				if(checks == false)
				{
					Div1.InnerHtml = "Plase select at least one day." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					Div1.Style.Value = "display: block";
					flags = true;
				}
					
			else if(Convert.ToInt32(MaintDurationTextBox.Text)==0)	
				{
					Div1.InnerHtml = "Please enter duration greater than 0." +
							   "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					Div1.Style.Value = "display: block";
					flags = true;
				}
				else if (returntable.Rows.Count > 0)
				{
					//3/19/2014 NS modified
					//ErrorMessageLabel.Text = "This Name or IP Address is already being monitored. Please enter another IP Address or Name.";
					//ErrorMessagePopupControl.ShowOnPageLoad = true;
					//10/6/2014 NS modified for VSPLUS-990
					Div1.InnerHtml = "The Business Hours Name  already exists. Please enter another Name." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					Div1.Style.Value = "display: block";
					flags = true;
					//IPAddressTextBox.Focus();
				}
				
				
				else
				{
					object ReturnValue = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.InsertData(CollectDataForBusinessHours());
				}
				
			}
			
				
			
			if (flag == "Update")
			{
				HoursIndicator Busibject = new HoursIndicator();

				Busibject.Type = txttimezone.Text;
				bool result = false;
				Busibject.ID = ServerKey;
					DataTable returntable = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.GetName(Busibject);
					if (AllCheckBox.Checked || SunCheckBox.Checked || MonCheckBox.Checked || TueCheckBox.Checked || WedCheckBox.Checked || ThusCheckBox.Checked || FriCheckBox.Checked || SatCheckBox.Checked)
					{
						checks = true;

					}

					if (checks == false)
					{
						Div1.InnerHtml = "Plase select at least one day." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						Div1.Style.Value = "display: block";
						flags = true;
					}

					else if (Convert.ToInt32(MaintDurationTextBox.Text) == 0)
					{
						Div1.InnerHtml = "Please enter duration greater than 0." +
								   "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						Div1.Style.Value = "display: block";
						flags = true;
					}
					else if (returntable.Rows.Count > 0)
					{
						//3/19/2014 NS modified
						//ErrorMessageLabel.Text = "This Name or IP Address is already being monitored. Please enter another IP Address or Name.";
						//ErrorMessagePopupControl.ShowOnPageLoad = true;
						//10/6/2014 NS modified for VSPLUS-990
						Div1.InnerHtml = "This Business Hours Name is already existed. Please enter another Name." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						Div1.Style.Value = "display: block";
						flags = true;
						//IPAddressTextBox.Focus();
					}
					else
					{

						result = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.UpdateBusinesshours(CollectDataForBusinessHours());
						//successDiv.Style.Value = "display: none";
						
					}
			}


		}
	    protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            MsgPopupControl.ShowOnPageLoad = false;
        }
        protected void AllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (UseDaysRadioButtonList.SelectedItem.Value.ToString() == "1")
            {
                DurationTrackBar.ClientVisible = false;
            }
            else
            {
                DurationTrackBar.ClientVisible = true;
            }
            if (AllCheckBox.Checked)
            {
                MonCheckBox.Checked = true;
                TueCheckBox.Checked = true;
                WedCheckBox.Checked = true;
                ThusCheckBox.Checked = true;
                FriCheckBox.Checked = true;
                SatCheckBox.Checked = true;
                SunCheckBox.Checked = true;
            }

            else
            {
                MonCheckBox.Checked = false;
                TueCheckBox.Checked = false;
                WedCheckBox.Checked = false;
                ThusCheckBox.Checked = false;
                FriCheckBox.Checked = false;
                SatCheckBox.Checked = false;
                SunCheckBox.Checked = false;
            
            }
        }



		protected void MaintStartTimeEdit_ValueChanged(object sender, EventArgs e)
		{
			if (MaintStartTimeEdit.Value!= null && MaintDurationTextBox.Text != "")
			{
				lblEndTime.Text = Convert.ToDateTime(MaintStartTimeEdit.Value.ToString()).AddMinutes(Convert.ToDouble(MaintDurationTextBox.Text)).ToString("h:mm tt");
				lblEndTime.Visible = true;
				lblDuration.Text = DurationHoursMins(DurationTrackBar.Value.ToString());
			}
		}
		protected void DurationTrackBar_PositionChanged(object sender, EventArgs e)
		{
			
			//MaintDurationTextBox.Value = DurationTrackBar.Value.ToString();
			MaintDurationTextBox.Text = DurationTrackBar.Value.ToString();
			if (MaintStartTimeEdit.Value != null)
			{
				lblEndTime.Text = Convert.ToDateTime(MaintStartTimeEdit.Value.ToString()).AddMinutes(Convert.ToDouble(DurationTrackBar.Value)).ToString("h:mm tt");
			}
			lblDuration.Text = DurationHoursMins(DurationTrackBar.Value.ToString());// DurationHoursMins(MaintenanceDataTable.Rows[0]["Duration"].ToString());
			
			
		}
		
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
                    duration = " minutes=" + (time / 60) + " hour(s)  " + (time % 60) + " minutes";
				}
				else
				{
					duration = " minutes=" + (time / 60) + " hour(s)";
				}
			}
			else
			{
				duration = (time % 60) + " minutes";
			}
			return duration;
		}

        /*
        protected void UseDaysRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UseDaysRadioButtonList.SelectedItem.Value.ToString() == "1")
            {
                MaintStartTimeEdit.Text = "12:00 AM";
                MaintDurationTextBox.Text = (60 * 24).ToString();
            }
            DisplayDaysOnly();
        }

        private void DisplayDaysOnly()
        {
            if (UseDaysRadioButtonList.SelectedItem.Value.ToString() == "1")
            {
                ASPxLabel7.ClientVisible = false;
                MaintStartTimeEdit.ClientVisible = false;
                infoDiv.Style.Value = "display: none";
                ASPxLabel8.ClientVisible = false;
                MaintDurationTextBox.ClientVisible = false;
                lblDuration.ClientVisible = false;
                DurationTrackBar.ClientVisible = false;
                ASPxLabel10.ClientVisible = false;
                lblEndTime.ClientVisible = false;
            }
            else
            {
                ASPxLabel7.ClientVisible = true;
                MaintStartTimeEdit.ClientVisible = true;
                infoDiv.Style.Value = "display: block";
                ASPxLabel8.ClientVisible = true;
                MaintDurationTextBox.ClientVisible = true;
                lblDuration.ClientVisible = true;
                DurationTrackBar.ClientVisible = true;
                ASPxLabel10.ClientVisible = true;
                lblEndTime.ClientVisible = true;
            }
        }
         */
    }
}

