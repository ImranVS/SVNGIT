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

using BL;
using DO;


namespace License
{
	public partial class CompanyGrid : System.Web.UI.Page
    {
		
        DataTable UsersDataTable = null;
        static int userID = 0;
        static int pwduserID = 0;
		protected void Page_Load(object sender, EventArgs e)
		{

			//pnlAreaDtls.Style.Add("visibility", "hidden");
			if (!IsPostBack)
			{
				FillUsersGrid();

				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "CompanyGrid|UsersGridView")
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
		         UsersDataTable = new DataTable();
                DataSet UsersDataSet = new DataSet();
				UsersDataTable = BL.UsersBL.Ins.GetCompanyData();
				//if (UsersDataTable.Rows.Count > 0)
				//{
                    DataTable dtcopy = UsersDataTable.Copy();
                    dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    Session["Users"] = dtcopy;
				
                    UsersGridView.DataSource = dtcopy;
				//}
                    UsersGridView.DataBind();
				//}
				//else
				//{

				//    UsersDataTable.Columns.Add("CompanyName", typeof(String));
				//    UsersGridView.DataBind();
				//}

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
			DataRow[] matchrow = UsersDataTable.Select("CompanyName = '" + newrow.ItemArray[1] + "'");
			//string loginname = newrow.ItemArray[1].ToString();
			//DataRow[] matchrow = UsersDataTable.Select(loginname);
			if (matchrow.Length > 0)
			{
				throw new ArgumentException("Company Name Already Exists");
			}

			else
			{
				UpdateUsersData("Insert", newrow);
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
            UpdateUsersData("Update", GetRow(UsersDataTable, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0])));
			UpdateUsersData("Insert", newrow);
		    gridView.CancelEdit();
            e.Cancel = true;
            FillUsersGrid();
		 }


        private void UpdateUsersData(string Mode, DataRow UsersRow)
        {

            if (Mode == "Insert")
            {
				Object ReturnValue = BL.UsersBL.Ins.InsertCompanyName(CollectDataForUsers(Mode, UsersRow));
            }
            if (Mode == "Update")
            {
				Object ReturnValue = BL.UsersBL.Ins.UpdateCompanyData(CollectDataForUsers(Mode, UsersRow));
			          
            }
			        }

		private LicenseCompanys CollectDataForUsers(string Mode, DataRow UsersRow)
        {
            try
            {
				LicenseCompanys UsersObject = new LicenseCompanys();
                if (Mode == "Update")
                {
                    UsersObject.ID = int.Parse(UsersRow["ID"].ToString());
                   
                }

				UsersObject.CompanyName = UsersRow["CompanyName"].ToString();
                return UsersObject;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void UsersGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

            Users UserObject = new Users();
            UserObject.ID = Convert.ToInt32(e.Keys[0]);
			Object ReturnValue = BL.UsersBL.Ins.DeleteCompanyData(UserObject);
			if  (Convert.ToString((ReturnValue)) == "true")
			{
				successDiv.InnerHtml = "The Server ' + values + ' was Successfully Deleted";
			
			}
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillUsersGrid();
        }

        protected void UsersGridView_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            
        }

        protected void UsersGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
           e.Editor.Enabled = true;
        }

        protected void UsersGridView_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            int index = e.VisibleIndex;
            int ID = Convert.ToInt32(UsersGridView.KeyFieldName);


        }

       

		//protected void btn_click0(object sender, EventArgs e)
		//{

		//    bttnOK.Visible = false;
		//    btnpwdOK.Visible = true;
		//    ImageButton btn = (ImageButton)sender;
		//    Users usersObj = new Users();
		//    usersObj.ID = Convert.ToInt32(btn.CommandArgument);
		//    pwduserID = Convert.ToInt32(btn.CommandArgument);
		//    string username = btn.AlternateText;
		//    pnlAreaDtls.Style.Add("visibility", "visible");
		//    divheader.InnerHtml = "Reset Password";
		//    divmsg.InnerHtml = "Are you sure you want to reset password for the user " + username + "?";

		//}

       

		//protected void bttnDelete_Click(object sender, EventArgs e)
		//{
		//    //12/12/2013 NS modified
		//     //tdmsg.InnerHtml = "";
		//     btnpwdOK.Visible = false;
		//      bttnOK.Visible = true;
		//    ImageButton bttnDel = (ImageButton)sender;
		//    Users usersObj = new Users();
		//    usersObj.ID = Convert.ToInt32(bttnDel.CommandArgument);
		//    userID = Convert.ToInt32(bttnDel.CommandArgument);
		//    //string username = bttnDel.AlternateText;
		//    string username = "";
		//    DataTable usersdt = (DataTable)Session["Users"];
		//    DataRow[] userrow = usersdt.Select("ID=" + bttnDel.CommandArgument.ToString());
		//    if (userrow[0] != null)
		//    {
		//        username = userrow[0]["FullName"].ToString();
		//    }
		//    pnlAreaDtls.Style.Add("visibility", "visible");
		//    divheader.InnerHtml = "Delete User";
		//    divmsg.InnerHtml = "Are you sure you want to delete the user " + username + "?";
		//}

		//protected void bttnOK_Click(object sender, EventArgs e)
		//{
		//    Users usersObj = new Users();
		//    usersObj.ID = userID;
		//    Object returnValue = BL.UsersBL.Ins.DeleteData(usersObj);
		//    if (Convert.ToBoolean(returnValue) == false)
		//    {
		//        NavigatorPopupControl.ShowOnPageLoad = true;
		//        MsgLabel.Text = "This user cannot be deleted, other dependencies exist.";
		//    }
		//    else
		//    {
		//        FillUsersGrid();
		//    }
		//}
		//protected void btnpwdOK_Click(object sender, EventArgs e)
		//{ }
		//protected void btn_click(object sender, EventArgs e)
		//{
		//    // ImageButton btn = (ImageButton)sender;
		//    //if (btn.CommandName == "RP")
		//    //{
		//    ImageButton btn = (ImageButton)sender;

		//    pwduserID = Convert.ToInt32(btn.CommandArgument);
		//    int id = pwduserID;  //Convert.ToInt32(btn.CommandArgument.ToString());
		//     Users Userobj = new Users();
		//       if (id != null)
		//        Userobj.ID = id;
		//    ////// bool s = VSWebBL.SecurityBL.UsersBL.Ins.UpdateAccntPassword(Userobj);
		//    bool s = true;
		//    if (s == true)
		//    {
		//        //int id1 = Convert.ToInt32(btn.CommandArgument.ToString());
		//        //DataTable dt = VSWebBL.SecurityBL.UsersBL.Ins.GetUserbyID(Userobj);
		//        //DataRow dr = dt.Rows[0];
		//        Response.Redirect("~/Security/MailChangePwd.aspx?id=" + id.ToString(), false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
		//        Context.ApplicationInstance.CompleteRequest();

		//        ////Sendmail(dr);
		//        //////12/12/2013 NS modified
		//        //////tdmsg.InnerHtml = "Password Updated Successfully";
		//        ////successDiv.InnerHtml = "Password reset successfully.";
		//        ////successDiv.Style.Value = "display: block";
		//    }
		//    else
		//    {
		//        //12/12/2013 NS modified
		//        //tdmsg.InnerHtml = "Password Not Updated";
		//        //10/7/2014 NS modified for VSPLUS-990
		//        errorDiv.InnerHtml = "Password was not updated." +
		//            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
		//        errorDiv.Style.Value = "display: block";
		//    }
		//    //}
		//}

		//protected void bttnCancel_Click(object sender, EventArgs e)
		//{
		//    FillUsersGrid();
		//}

		//protected void subbttnOK_Click(object sender, EventArgs e)
		//{
		//    NavigatorPopupControl.ShowOnPageLoad = false;
		//}

        protected void UsersGridView_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["SuperAdmin"] = "N";
            e.NewValues["Status"] = "Active";
        }

		
        protected void UsersGridView_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            if (grid.IsEditing)
            {
                grid.Settings.ShowFilterRow = false;
            }
        }

		protected void CompanyGrid_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			BL.UserPreferencesBL.Ins.UpdateUserPreferences("CompanyGrid|UsersGridView", UsersGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = BL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}


    }
}