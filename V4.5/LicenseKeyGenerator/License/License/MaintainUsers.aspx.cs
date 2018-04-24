using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using DevExpress.Web;
using System.Configuration;
using VSFramework;
using BL;
using DO;


namespace License
{
    public partial class MaintainUsers : System.Web.UI.Page
    {
		
        DataTable UsersDataTable = null;
        static int userID = 0;
        static int pwduserID = 0;
		protected void Page_Load(object sender, EventArgs e)
		{

			pnlAreaDtls.Style.Add("visibility", "hidden");
			if (!IsPostBack)
			{
				FillUsersGrid();

				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "MaintainUsers|UsersGridView")
						{
							UsersGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
					}
				}

				
			}
			else
			{

				FillUsersGridfromSession();

			}
		}
        private void FillUsersGrid()
        {
            try
			{
				//bool isadmin = false;
				//DataTable sa = BL.UsersBL.Ins.GetIsAdmin(Session["UserID"].ToString());
				//if (sa.Rows.Count > 0)
				//{
				//    if (sa.Rows[0]["SuperAdmin"].ToString() == "Y")
				//    {
				//        isadmin = true;
				//    }
				//}
				//if (isadmin)
				//{
				//    UsersGridView.Columns["IsConsoleComm"].Visible = true;
				//    UsersGridView.Columns["IsConsoleComm"].ShowInCustomizationForm = true;
				//}
				//else
				//{
				//    UsersGridView.Columns[0].Visible = false;
				//    UsersGridView.Columns[1].Visible = false;
				//    UsersGridView.Columns[2].Visible = false;
				//}
                UsersDataTable = new DataTable();
                DataSet UsersDataSet = new DataSet();
                UsersDataTable = BL.UsersBL.Ins.GetAllData();
                if (UsersDataTable.Rows.Count > 0)
                {
                    DataTable dtcopy = UsersDataTable.Copy();
                    dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    Session["Users"] = dtcopy;
                    UsersGridView.DataSource = dtcopy;
                    UsersGridView.DataBind();
                }
                else
                {

                    UsersGridView.DataSource = UsersDataTable;
					UsersDataTable.Columns.Add("FullName", typeof(String));
					UsersDataTable.Columns.Add("Email", typeof(String));
					UsersDataTable.Columns.Add("Status", typeof(String));
					UsersDataTable.Columns.Add("UserType", typeof(String));
					UsersDataTable.Columns.Add("LoginName", typeof(String));

					//Session{Newname"]=UsersDataTable.rows[0].columns["LoginName"].text;
                    UsersGridView.DataBind();
                }

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        private void FillUsersGridfromSession()
        {
            try
            {

                UsersDataTable = new DataTable();
                if (Session["Users"] != null && Session["Users"] != "")
                    UsersDataTable = (DataTable)Session["Users"];
                if (UsersDataTable.Rows.Count > 0)
                {
                    UsersGridView.DataSource = UsersDataTable;
                    UsersGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected DataRow GetRow(DataTable UserObject, IDictionaryEnumerator enumerator, int Keys)
        {
            DataTable dataTable = UserObject;
            DataRow DRRow = dataTable.NewRow();
            if (Keys == 0)
                DRRow = dataTable.NewRow();
            else
                DRRow = dataTable.Rows.Find(Keys);
            //IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
            return DRRow;
        }

        protected void UsersGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            string userpwd = "";
            UsersDataTable = (DataTable)Session["Users"];
            ASPxGridView gridView = (ASPxGridView)sender;
			DataRow newrow = GetRow(UsersDataTable, e.NewValues.GetEnumerator(), 0);
			DataRow[] matchrow = UsersDataTable.Select("LoginName = '" + newrow.ItemArray[1] + "'");
			//string loginname = newrow.ItemArray[1].ToString();
			//DataRow[] matchrow = UsersDataTable.Select(loginname);
			if (matchrow.Length > 0)
			{
				throw new ArgumentException("LoginName Already Exists");
			}

			DataRow[] matchrow1 = UsersDataTable.Select("Email = '" + newrow.ItemArray[4] + "'");
			if (matchrow1.Length > 0)
			{
				throw new ArgumentException("Email Already Exists");
			}

			else
			{
				userpwd = CreateRandomPassword(6);
				Session["randompwd"] = userpwd;


				//string errString = Sendmailforinserting(newrow, userpwd);

				//if (errString == "")
				//{
				  UpdateUsersData("Insert", newrow, userpwd);
				//}
				//else
				//{
				//    throw new System.InvalidOperationException("Error: " + errString + ". Check SMTP settings at Alert-> Alert Settings and then retry to create user.");
				//}

				//Update Grid after inserting new row, refresh grid as in page load
				gridView.CancelEdit();
				e.Cancel = true;
				FillUsersGrid();
			}
        }

        protected void UsersGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
			string userpwd = "";
            UsersDataTable = (DataTable)Session["Users"];
            ASPxGridView gridView = (ASPxGridView)sender;
			DataRow newrow = GetRow(UsersDataTable, e.NewValues.GetEnumerator(), 0);

            //Update row in DB



            UpdateUsersData("Update", GetRow(UsersDataTable, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0])), "");


            // Sendmail(GetRow(UsersDataTable, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0])));

			

			//if (errString == "")
			//{
			    UpdateUsersData("Insert", newrow, userpwd);
			//}
			//else
			//{
			//    throw new System.InvalidOperationException("Error: " + errString + ". Check SMTP settings at Alert-> Alert Settings and then retry to create user.");
			//}

            //Update Grid after inserting new row, refresh grid as in page load
            gridView.CancelEdit();
            e.Cancel = true;
            FillUsersGrid();
			//string errString = Sendmail(newrow, userpwd);

        }


        private static string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }



		//public string Sendmail(DataRow UsersRow, string userpwd)
		//{
		//    try
		//    {
		//        //pass Generate Code
		//        //11/21/2013 NS modified
		//        string toEmailAddress;
		//        string subject;
		//        string mailbody;
		//        string[] mailparams = new string[5];
		//        Settings[] settingsObject = new Settings[5];
		//        Settings[] rtsettingsObject = new Settings[5];
		//        for (int i = 0; i < 5; ++i)
		//        {
		//            settingsObject[i] = new Settings();
		//        }
		//        for (int i = 0; i < 5; ++i)
		//        {
		//            rtsettingsObject[i] = new Settings();
		//        }
		//        settingsObject[0].sname = "PrimaryHostName";
		//        settingsObject[1].sname = "primaryUserID";
		//        settingsObject[2].sname = "primarypwd";
		//        settingsObject[3].sname = "primaryport";
		//        //12/4/2013 NS added SSL setting
		//        settingsObject[4].sname = "primarySSL";
		//        mailparams[0] = "smtp.gmail.com";
		//        mailparams[1] = ConfigurationSettings.AppSettings["AdminMailID"]; //"web.vitalsigns@gmail.com";
		//        mailparams[2] = ConfigurationSettings.AppSettings["Password"];        //"vitalsigns2012";
		//        mailparams[3] = "587";
		//        //12/4/2013 NS added SSL setting
		//        mailparams[4] = "true";
		//        for (int i = 0; i < 5; i++)
		//        {
		//            try
		//            {
		//                rtsettingsObject[i] = VSWebBL.SettingBL.SettingsBL.Ins.GetData(settingsObject[i]);
		//                if (rtsettingsObject[i].svalue == "" || rtsettingsObject[i].svalue == null)
		//                {
		//                    //
		//                }
		//                else
		//                {
		//                    mailparams[i] = rtsettingsObject[i].svalue;
		//                }
		//            }
		//            catch (Exception ex)
		//            {
		//                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
		//                throw ex;
		//            }
		//        }

		//        toEmailAddress = UsersRow["Email"].ToString();
		//        subject = "Your VitalSigns Account Information Update";

		//        if (UsersRow["Password"].ToString() != "" && UsersRow["Password"].ToString() != null)
		//        {
		//            //11/20/2014 NS modified for VSPLUS-1190
		//            //mailbody = "Your VitalSigns Account Details: \nLoginName:" + UsersRow["LoginName"].ToString() + "\nPassword:" + UsersRow["Password"].ToString() + "";
		//            mailbody = "Welcome to VitalSigns. Your account details are as follows: \n\rUser name:" + UsersRow["LoginName"].ToString() + "\nPassword:" + UsersRow["Password"].ToString() + "";
		//        }
		//        else 
		//        {
		//            //11/20/2014 NS modified for VSPLUS-1190
		//            //mailbody = "Your VitalSigns Account Details: \nLoginName: " + UsersRow["LoginName"].ToString() + "\nPassword :" + Session["randompwd"].ToString() + "";
		//            mailbody = "Welocme to VitalSigns.You account details updated as follows:\nLoginName: " + UsersRow["LoginName"].ToString() +  "";
		//        }

		
		//        //5/8/2014 NS modified for VSPLUS-587
		//        //mailbody += " \nURL To Dashboard: " + HttpContext.Current.Request.Url.AbsoluteUri.ToString().Replace("Security/MaintainUsers.aspx", "Dashboard/OverallHealth1.aspx");
		//        mailbody += " \nURL To Dashboard: " + HttpContext.Current.Request.Url.AbsoluteUri.ToString().Replace("Security/MaintainUsers.aspx", "Login.aspx");

		//        var client = new System.Net.Mail.SmtpClient(mailparams[0], Convert.ToInt32(mailparams[3]))
		//        {
		//            Credentials = new System.Net.NetworkCredential(mailparams[1], mailparams[2]),
		//            EnableSsl = Convert.ToBoolean(mailparams[4])
		//        };
		//        client.Send(mailparams[1], toEmailAddress, subject, mailbody);
		//        return "";

		//    }
		//    catch (Exception ex)
		//    {
		//        //The SMTP server requires a secure connection or the client was not authenticated. The server response was: 5.7.0 Must issue a STARTTLS command first. d12sm11699332qac.37 - gsmtp";
		//        return ex.Message;

		//    }
		//}
		//public string Sendmailforinserting(DataRow UsersRow, string userpwd)
		//{
		//    try
		//    {
		//        //pass Generate Code
		//        //11/21/2013 NS modified
		//        string toEmailAddress;
		//        string subject;
		//        string mailbody;
		//        string[] mailparams = new string[5];
		//        Settings[] settingsObject = new Settings[5];
		//        Settings[] rtsettingsObject = new Settings[5];
		//        for (int i = 0; i < 5; ++i)
		//        {
		//            settingsObject[i] = new Settings();
		//        }
		//        for (int i = 0; i < 5; ++i)
		//        {
		//            rtsettingsObject[i] = new Settings();
		//        }
		//        settingsObject[0].sname = "PrimaryHostName";
		//        settingsObject[1].sname = "primaryUserID";
		//        settingsObject[2].sname = "primarypwd";
		//        settingsObject[3].sname = "primaryport";
		//        //12/4/2013 NS added SSL setting
		//        settingsObject[4].sname = "primarySSL";
		//        mailparams[0] = "smtp.gmail.com";
		//        mailparams[1] = ConfigurationSettings.AppSettings["AdminMailID"]; //"web.vitalsigns@gmail.com";
		//        mailparams[2] = ConfigurationSettings.AppSettings["Password"];        //"vitalsigns2012";
		//        mailparams[3] = "587";
		//        //12/4/2013 NS added SSL setting
		//        mailparams[4] = "true";
		//        for (int i = 0; i < 5; i++)
		//        {
		//            try
		//            {
		//                rtsettingsObject[i] = VSWebBL.SettingBL.SettingsBL.Ins.GetData(settingsObject[i]);
		//                if (rtsettingsObject[i].svalue == "" || rtsettingsObject[i].svalue == null)
		//                {
		//                    //
		//                }
		//                else
		//                {
		//                    mailparams[i] = rtsettingsObject[i].svalue;
		//                }
		//            }
		//            catch (Exception ex)
		//            {
		//                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
		//                throw ex;
		//            }
		//        }

		//        toEmailAddress = UsersRow["Email"].ToString();
		//        subject = "Your VitalSigns Account Information Update";

		//        if (UsersRow["Password"].ToString() != "" && UsersRow["Password"].ToString() != null)
		//        {
		//            //11/20/2014 NS modified for VSPLUS-1190
		//            //mailbody = "Your VitalSigns Account Details: \nLoginName:" + UsersRow["LoginName"].ToString() + "\nPassword:" + UsersRow["Password"].ToString() + "";
		//            mailbody = "Welcome to VitalSigns. Your account details are as follows: \n\rUser name:" + UsersRow["LoginName"].ToString() + "\nPassword:" + UsersRow["Password"].ToString() + "";
		//        }
		//        else
		//        {
		//            //11/20/2014 NS modified for VSPLUS-1190
		//            //mailbody = "Your VitalSigns Account Details: \nLoginName: " + UsersRow["LoginName"].ToString() + "\nPassword :" + Session["randompwd"].ToString() + "";
		//            mailbody = "Welcome to VitalSigns. Your account details are as follows: \n\rUser name: " + UsersRow["LoginName"].ToString() + "\nPassword :" + Session["randompwd"].ToString() + "";
		//        }


		//        //5/8/2014 NS modified for VSPLUS-587
		//        //mailbody += " \nURL To Dashboard: " + HttpContext.Current.Request.Url.AbsoluteUri.ToString().Replace("Security/MaintainUsers.aspx", "Dashboard/OverallHealth1.aspx");
		//        mailbody += " \nURL To Dashboard: " + HttpContext.Current.Request.Url.AbsoluteUri.ToString().Replace("Security/MaintainUsers.aspx", "Login.aspx");

		//        var client = new System.Net.Mail.SmtpClient(mailparams[0], Convert.ToInt32(mailparams[3]))
		//        {
		//            Credentials = new System.Net.NetworkCredential(mailparams[1], mailparams[2]),
		//            EnableSsl = Convert.ToBoolean(mailparams[4])
		//        };
		//        client.Send(mailparams[1], toEmailAddress, subject, mailbody);
		//        return "";

		//    }
		//    catch (Exception ex)
		//    {
		//        //The SMTP server requires a secure connection or the client was not authenticated. The server response was: 5.7.0 Must issue a STARTTLS command first. d12sm11699332qac.37 - gsmtp";
		//        return ex.Message;

		//    }
		//}

        private void UpdateUsersData(string Mode, DataRow UsersRow, string userpwd)
        {

            if (Mode == "Insert")
            {
                Object ReturnValue = BL.UsersBL.Ins.CreateAccount(CollectDataForUsers(Mode, UsersRow, userpwd));
            }
            if (Mode == "Update")
            {
                Object ReturnValue = BL.UsersBL.Ins.UpdateData(CollectDataForUsers(Mode, UsersRow, userpwd));
			          

            }
        }

        private Users CollectDataForUsers(string Mode, DataRow UsersRow, string userpwd)
        {
            try
            {
                Users UsersObject = new Users();
                if (Mode == "Update")
                {
                    UsersObject.ID = int.Parse(UsersRow["ID"].ToString());
                    UsersObject.Password = UsersRow["Password"].ToString();
                }

                UsersObject.LoginName = UsersRow["LoginName"].ToString();
                if (Mode == "Insert")
                {
                    UsersObject.Password = userpwd;// CreateRandomPassword(6);
                    Session["randompwd"] = UsersObject.Password;
                }
				//UsersObject.CloudApplications = true;
				//UsersObject.DominoServerMetrics = true;
				//UsersObject.NetworkInfrastucture = true;
				//UsersObject.OnPremisesApplications = true;

                UsersObject.FullName = UsersRow["FullName"].ToString();
                UsersObject.Email = UsersRow["Email"].ToString();
                UsersObject.Status = UsersRow["Status"].ToString();
				UsersObject.UserType = UsersRow["UserType"].ToString();
				//if (UsersObject.SuperAdmin == "Y")
				//{
				//    UsersObject.IsConfigurator = true;
				//    UsersObject.Isconsolecomm = true;
				//}
				//else
				//{
				//    UsersObject.IsConfigurator = Convert.ToBoolean(UsersRow["Isconfigurator"].ToString());
                   
				//    if (UsersRow["IsConsoleComm"].ToString() == "" || UsersRow["IsConsoleComm"] == null)
				//    {
				//        UsersRow["IsConsoleComm"] = "false";
				//    }
				//    UsersObject.Isconsolecomm = Convert.ToBoolean(UsersRow["IsConsoleComm"].ToString());
                    
				//}
				////UsersObject.Isdashboard = Convert.ToBoolean(UsersRow["IsDashboard"].ToString());
				//UsersObject.Isdashboard = true;
                return UsersObject;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }




        //private Users CollectDataForUsers(string Mode, DataRow UsersRow, string userpwd)
        //{
        //    try
        //    {
        //        Users UsersObject = new Users();
        //        if (Mode == "Update")
        //        {
        //            UsersObject.ID = int.Parse(UsersRow["ID"].ToString());
        //            UsersObject.Password = UsersRow["Password"].ToString();
        //        }

        //        UsersObject.LoginName = UsersRow["LoginName"].ToString();
        //        if (Mode == "Insert")
        //        {
        //            UsersObject.Password = userpwd;// CreateRandomPassword(6);
        //            Session["randompwd"] = UsersObject.Password;
        //        }
        //        UsersObject.FullName = UsersRow["FullName"].ToString();
        //        UsersObject.Email = UsersRow["Email"].ToString();
        //        UsersObject.Status = UsersRow["Status"].ToString();
        //        UsersObject.SuperAdmin = UsersRow["SuperAdmin"].ToString();
        //        UsersObject.IsConfigurator = Convert.ToBoolean(UsersRow["Isconfigurator"].ToString());
        //        //UsersObject.Isdashboard = Convert.ToBoolean(UsersRow["IsDashboard"].ToString());
        //        UsersObject.Isdashboard = true;
        //        if (UsersRow["IsConsoleComm"].ToString() == "" || UsersRow["IsConsoleComm"] == null)
        //        {
        //            UsersRow["IsConsoleComm"] = "false";
        //        }
        //        UsersObject.Isconsolecomm = Convert.ToBoolean(UsersRow["IsConsoleComm"].ToString());
        //        return UsersObject;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
        //        throw ex;
        //    }
        //    finally { }
        //}

        protected void UsersGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

            Users UserObject = new Users();
            UserObject.ID = Convert.ToInt32(e.Keys[0]);
            // UsersDataTable=(DataTable)Session["Users"];
            //DataRow row=GetRow(UsersDataTable, e.Values.GetEnumerator(), Convert.ToInt32(e.Keys[0]));
            //UsersGridView.SettingsText.ConfirmDelete = "Are you sure you want to delete  " + row["LoginName"] + " ?"; 
            //Delete row from DB
            Object ReturnValue = BL.UsersBL.Ins.DeleteData(UserObject);
			if  (Convert.ToString((ReturnValue)) == "true")
			{
				successDiv.InnerHtml = "The Server ' + values + ' was Successfully Deleted";
				////successDiv.Style.Value = "display: block";

			}
            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillUsersGrid();
        }

        protected void UsersGridView_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            //GridViewDataItemTemplateContainer container;
            //if (e.ButtonID != "Reset")

            //{ return; }
            //else
            //{
            //   // Label ID = (Label)container.FindControl("AID");
            //}

            //UsersGridView.GetSelectedFieldValues(UsersGridView.KeyFieldName);
            //string pwd = CreateRandomPassword(6);

            //Users Userobj = new Users();
            //Userobj.Password = pwd;
            //if (UsersRow["ID"].ToString() != null && UsersRow["ID"].ToString() != "")
            //    Userobj.ID = int.Parse(UsersRow["ID"].ToString());
            //VSWebBL.SecurityBL.UsersBL.Ins.UpdateAccntPassword(Userobj);




        }

        protected void UsersGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            // int ID = int.Parse(e.KeyValue.ToString);
			e.Editor.Enabled = true;
        }

        protected void UsersGridView_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            int index = e.VisibleIndex;
            int ID = Convert.ToInt32(UsersGridView.KeyFieldName);


        }

        //  protected void UsersGridView_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        //  {

        //      //string[] Copyfields=new string[]{"LoginName","FullName","Email","Status","Active","SuperAdmin"};



        //  //    if (e.ButtonID != "Reset")

        //  //    { return; }
        //  //    else
        //  //    {
        //  //        int index = e.VisibleIndex;
        //  //        int ID = Convert.ToInt32(UsersGridView.KeyFieldName);


        //  //            Users UserObject = new Users();
        //  //        UserObject.ID = Convert.ToInt32(e.Keys[0]);

        //  //    }
        //}

        protected void btn_click0(object sender, EventArgs e)
        {

            bttnOK.Visible = false;
            btnpwdOK.Visible = true;
            ImageButton btn = (ImageButton)sender;
            Users usersObj = new Users();
            usersObj.ID = Convert.ToInt32(btn.CommandArgument);
            pwduserID = Convert.ToInt32(btn.CommandArgument);
            string username = btn.AlternateText;
            pnlAreaDtls.Style.Add("visibility", "visible");
            divheader.InnerHtml = "Reset Password";
            divmsg.InnerHtml = "Are you sure you want to reset password for the user " + username + "?";

        }

        public string Resetrp(GridViewDataItemTemplateContainer container)
        {
            Label ID = (Label)container.FindControl("lblid");
            string pwd = CreateRandomPassword(6);

            Users Userobj = new Users();
            Userobj.Password = pwd;
            if (ID.Text != null && ID.Text != "")
                Userobj.ID = int.Parse(ID.Text);
            bool s = BL.UsersBL.Ins.UpdateAccntPassword(Userobj);
            if (s == true)
            {
                return "Password Updated";
            }
            else
            {
                return "";
            }
        }

        protected void bttnDelete_Click(object sender, EventArgs e)
        {
            //12/12/2013 NS modified
            //tdmsg.InnerHtml = "";
            btnpwdOK.Visible = false;
            bttnOK.Visible = true;
            ImageButton bttnDel = (ImageButton)sender;
            Users usersObj = new Users();
            usersObj.ID = Convert.ToInt32(bttnDel.CommandArgument);
            userID = Convert.ToInt32(bttnDel.CommandArgument);
            //string username = bttnDel.AlternateText;
            string username = "";
            DataTable usersdt = (DataTable)Session["Users"];
            DataRow[] userrow = usersdt.Select("ID=" + bttnDel.CommandArgument.ToString());
            if (userrow[0] != null)
            {
                username = userrow[0]["FullName"].ToString();
            }
            pnlAreaDtls.Style.Add("visibility", "visible");
            divheader.InnerHtml = "Delete User";
            divmsg.InnerHtml = "Are you sure you want to delete the user " + username + "?";
        }

        protected void bttnOK_Click(object sender, EventArgs e)
        {
            Users usersObj = new Users();
            usersObj.ID = userID;
            Object returnValue = BL.UsersBL.Ins.DeleteData(usersObj);
            if (Convert.ToBoolean(returnValue) == false)
            {
                NavigatorPopupControl.ShowOnPageLoad = true;
                MsgLabel.Text = "This user cannot be deleted, other dependencies exist.";
            }
            else
            {
                FillUsersGrid();
            }
        }
        protected void btnpwdOK_Click(object sender, EventArgs e)
        { }
        protected void btn_click(object sender, EventArgs e)
        {
            // ImageButton btn = (ImageButton)sender;
            //if (btn.CommandName == "RP")
            //{
            ImageButton btn = (ImageButton)sender;

            pwduserID = Convert.ToInt32(btn.CommandArgument);
            int id = pwduserID;  //Convert.ToInt32(btn.CommandArgument.ToString());
            string pwd = CreateRandomPassword(6);

            Users Userobj = new Users();
            Userobj.Password = pwd;
            if (id != null)
                Userobj.ID = id;
            ////// bool s = VSWebBL.SecurityBL.UsersBL.Ins.UpdateAccntPassword(Userobj);
            bool s = true;
            if (s == true)
            {
                //int id1 = Convert.ToInt32(btn.CommandArgument.ToString());
                //DataTable dt = VSWebBL.SecurityBL.UsersBL.Ins.GetUserbyID(Userobj);
                //DataRow dr = dt.Rows[0];
                Response.Redirect("~/Security/MailChangePwd.aspx?id=" + id.ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();

                ////Sendmail(dr);
                //////12/12/2013 NS modified
                //////tdmsg.InnerHtml = "Password Updated Successfully";
                ////successDiv.InnerHtml = "Password reset successfully.";
                ////successDiv.Style.Value = "display: block";
            }
            else
            {
                //12/12/2013 NS modified
                //tdmsg.InnerHtml = "Password Not Updated";
                //10/7/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "Password was not updated." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }
            //}
        }

        protected void bttnCancel_Click(object sender, EventArgs e)
        {
            FillUsersGrid();
        }

        protected void subbttnOK_Click(object sender, EventArgs e)
        {
            NavigatorPopupControl.ShowOnPageLoad = false;
        }

        protected void UsersGridView_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["SuperAdmin"] = "N";
            e.NewValues["Status"] = "Active";
        }

		//protected void UsersGridView_PageSizeChanged(object sender, EventArgs e)
		//{
		//    //ProfilesGridView.PageIndex;
		//    VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MaintainUsers|UsersGridView", UsersGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
		//    Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		//}

        //7/9/2014 NS added for VSPLUS-710
        protected void UsersGridView_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            if (grid.IsEditing)
            {
                grid.Settings.ShowFilterRow = false;
            }
        }

		protected void UsersGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			BL.UserPreferencesBL.Ins.UpdateUserPreferences("MaintainUsers|UsersGridView", UsersGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = BL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}

    }
}