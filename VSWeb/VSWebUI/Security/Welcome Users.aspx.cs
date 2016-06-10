using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using DevExpress.XtraReports.UI;
using DevExpress.Web;


using VSWebDO;
using VSWebUI;
using System.IO;

namespace VSWebUI.Security
{
	public partial class Welcome : System.Web.UI.Page
    {
		string loginname;
        FileUpload uploadedFile;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString["dboard"] != null && Request.QueryString["dboard"].ToString() != "")
            {
                this.MasterPageFile = "~/DashboardSite.Master";
            }
            else
            {
                this.MasterPageFile = "~/Site1.Master";
            }
        }
		protected void Page_Load(object sender, EventArgs e)
		{
			
			if (!IsPostBack)
			{
              
				if (Session["UserFullName"] != null)
				{
					servernamelbldisp.InnerHtml = "Welcome -" + " " + Session["UserFullName"].ToString();
				}
      
				if (Session["UserLogin"] != null && Session["UserLogin"] != "")
				{
					loginname = Session["UserLogin"].ToString();
					DataTable FilepathDataTable = VSWebBL.SecurityBL.UserSecurityQuestionsBL.Ins.GetFilepathData(loginname);
					if (FilepathDataTable.Rows.Count > 0)
					{
                        //9/3/2016 sowmya removed for VSPLUS-2661
                        //string path2 = FilepathDataTable.Rows[0]["CustomBackground"].ToString();
                        //previewImage.Src = path2;
                        //bool exists = System.IO.Directory.Exists(Server.MapPath(path2));

						

						FillSecurityQuestion1ComboBox(UserSecQuestion1ComboBox);
						FillSecurityQuestion2ComboBox(UserSecQuestion2ComboBox);
						//VSPLUS-272 Go to Dashboard as default.
						
						//VSPLUS-613, Mukund 14May14, added Replace as single quote was giving issue
						UserFullNameTextBox.Value = Session["UserFullName"].ToString().Replace("&quot;", "'");
						UserLoginNameTextBox.Value = Session["UserLogin"].ToString().Replace("&quot;", "'"); ;
						// UserCurPasswordTextBox.Value = Session["UserPassword"];
						UserEmailTextBox.Value = Session["UserEmail"].ToString().Replace("&quot;", "'");
						UserSecQuestion1ComboBox.Value = Session["UserSecurityQuestion1"].ToString().Replace("&quot;", "'");
						UserSecQuestion1AnsTextBox.Value = Session["UserSecurityQuestion1Answer"].ToString().Replace("&quot;", "'");
						UserSecQuestion2ComboBox.Value = Session["UserSecurityQuestion2"].ToString().Replace("&quot;", "'");
						UserSecQuestion2AnsTextBox.Value = Session["UserSecurityQuestion2Answer"].ToString().Replace("&quot;", "'");
						
						//UserEmailTextBox.Text = Session["UserEmail"].ToString();
						UserEmailTextBox.Text = Session["UserEmail"].ToString().Replace("&quot;", "'");
						//VSPLUS-613, Mukund 14May14, disabled login name as not required in update
						UserLoginNameTextBox.Enabled = false;
						
					}
				}
			}
            Master.FindControl("NRBtn").Visible = false;
            Master.FindControl("OKBtn").Visible = false;
            Master.FindControl("IBtn").Visible = false;
            Master.FindControl("MBtn").Visible = false;

		}
		
		
        private void FillSecurityQuestion1ComboBox(ASPxComboBox LocationComboBox)
        {
            DataTable SecurityQuetionsDataTable = VSWebBL.SecurityBL.UserSecurityQuestionsBL.Ins.GetAllData();

            UserSecQuestion1ComboBox.DataSource = SecurityQuetionsDataTable;
            UserSecQuestion1ComboBox.TextField = "SecurityQuestion";
            UserSecQuestion1ComboBox.ValueField = "SecurityQuestion";
            UserSecQuestion1ComboBox.DataBind();
        }

        private void FillSecurityQuestion2ComboBox(ASPxComboBox LocationComboBox)
        {
            DataTable SecurityQuetionsDataTable = VSWebBL.SecurityBL.UserSecurityQuestionsBL.Ins.GetAllData();

            UserSecQuestion2ComboBox.DataSource = SecurityQuetionsDataTable;
            UserSecQuestion2ComboBox.TextField = "SecurityQuestion";
            UserSecQuestion2ComboBox.ValueField = "SecurityQuestion";
            UserSecQuestion2ComboBox.DataBind();
        }

        //VSPLUS-272 Go to Dashboard as default.
      
        private Users CollectDataForUsers()
        {
            try
            {

                Users UsersObject = new Users();
                //VSPLUS-613, Mukund 14May14, added Replace as single quote was giving issue
                // UsersObject.Password = UserNewPasswordTextBox.Text.Replace("'", "&quot;");
                UsersObject.FullName = UserFullNameTextBox.Text.Replace("'", "&quot;");
               
                UsersObject.CustomBackground = Session["CustomBackground"].ToString();
                UsersObject.SecurityQuestion1 = UserSecQuestion1ComboBox.Text.Replace("'", "&quot;");
                Session["UserSecurityQuestion1"] = UserSecQuestion1ComboBox.Text.Replace("'", "&quot;");
                UsersObject.SecurityQuestion1Answer = UserSecQuestion1AnsTextBox.Text.Replace("'", "&quot;");
                Session["UserSecurityQuestion1Answer"] = UserSecQuestion1AnsTextBox.Text.Replace("'", "&quot;");
				//UsersObject.Email = UserEmailTextBox.Text.Replace("'", "&quot;");
				//Session["UserEmail"] = UserEmailTextBox.Text.Replace("'", "&quot;");
                UsersObject.SecurityQuestion2 = UserSecQuestion2ComboBox.Text.Replace("'", "&quot;");
                Session["UserSecurityQuestion2"] = UserSecQuestion2ComboBox.Text.Replace("'", "&quot;");
                UsersObject.SecurityQuestion2Answer = UserSecQuestion2AnsTextBox.Text.Replace("'", "&quot;");
                Session["UserSecurityQuestion2Answer"] = UserSecQuestion2AnsTextBox.Text.Replace("'", "&quot;");
               
               
				UsersObject.Email = UserEmailTextBox.Text;
				Session["UserEmail"] = UserEmailTextBox.Text;
                UsersObject.ID = int.Parse(Session["UserID"].ToString());
                return UsersObject;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void UserInfoSaveButton_Click(object sender, EventArgs e)
        {

			var noErr = true;
			if (UserSecQuestion1ComboBox.Text == UserSecQuestion2ComboBox.Text)
			{
				Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "alert('Security questions are repeated. Please select another one.');", true);
				noErr = false;
			}
			else
			{
				noErr = true;
			}
			if (UserSecQuestion1ComboBox.SelectedIndex > -1)
			{
				if (UserSecQuestion1ComboBox.SelectedItem.Text != "")
				{
					if (UserSecQuestion1AnsTextBox.Text == "")
					{
						Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "alert('Enter Answer To First Question');", true);
						noErr = false;

					}
				}
			}
			else
			{
				if (UserSecQuestion1AnsTextBox.Text != "")
				{
					noErr = false;
					Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "alert('Please Select First Question');", true);
					

				}
				else
				{
					noErr = true;
				}
			}
			if (UserSecQuestion2ComboBox.SelectedIndex > -1)
			{
				if (UserSecQuestion2ComboBox.SelectedItem.Text != "")
				{

					if (UserSecQuestion2AnsTextBox.Text == "")
					{
						Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "alert('Enter Answer To Second Question');", true);
						noErr = false;

					}
				}
			}
			else
			{
				if (UserSecQuestion2AnsTextBox.Text != "")
				{
					noErr = false;
					Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "alert('Please Select Second Question');", true);


				}
				else
				{
					noErr = true;
				}
			}
			//if (noErr)
			//{
			//    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "alert('Enter Answer to First Question');", true);
			//}
			if(noErr)
			{
				Object ReturnValue = VSWebBL.SecurityBL.UsersBL.Ins.UpdateAccount1(CollectDataForUsers());
                //previewImage.Src = Session["CustomBackground"].ToString();
				//2/5/2013 NS modified
				//ASPxLabel1.Text = "Account Updated Successfully";
				//12/31/2013 NS modified
				//ErrorMessageLabel.Text = "Account information updated successfully.";
				//ErrorMessagePopupControl.ShowOnPageLoad = true;
				successDiv.Style.Value = "display: block";
				Object AfterLogin = new Object();
				//Sowjanya vsplus-1811
				//AfterLogin = VSWebBL.SecurityBL.UsersBL.Ins.UpdateIsFirstTimeLogin(Convert.ToInt32(Session["UserID"]));
				bool success = VSWebBL.SecurityBL.UsersBL.Ins.GetIsFirstTimeLogin(Convert.ToInt32(Session["UserID"]));
				string pwd = Session["UserPassword"].ToString();
				if (success == true && pwd.IndexOf("temp_") == 0)
				{
					Response.Redirect("~/Security/ForceChangePwd.aspx", false);
					Context.ApplicationInstance.CompleteRequest();
				}
				else
				{
					AfterLogin = VSWebBL.SecurityBL.UsersBL.Ins.UpdateIsFirstTimeLogin(Convert.ToInt32(Session["UserID"]));
					Response.Redirect("~/Configurator/Default.aspx");
					Context.ApplicationInstance.CompleteRequest();
				}

			}
			
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
			Response.Redirect("~/Login.aspx", false);
			Context.ApplicationInstance.CompleteRequest();
        }

        protected void ValidationOKButton_Click(object sender, EventArgs e)
        {
            RedirectToPage();
        }

        private void RedirectToPage()
        {
            if (Request.QueryString["dboard"] != null && Request.QueryString["dboard"].ToString() != "")
            {
                Response.Redirect("~/Dashboard/OverallHealth1.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Redirect("~/Configurator/Default.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                Context.ApplicationInstance.CompleteRequest();
            }
        }
        private bool checkIfPageExists(string startupURL)
        {
            if (Server.MapPath(startupURL) != "")
                return true;
            else
                return false;
        }
     		
		protected void UserSecQuestion1ComboBox_Validation(object sender, ValidationEventArgs e)
		{
			//if (!(e.Value is ItemClickClientSideEvents)) ;
			//    return;
			//DateTime selecteditem = (DateTime)e.Value;
			//DateTime currentDate = DateTime.Now;
			//if (selecteditem.Year != currentDate.Year || selecteditem.Month != currentDate.Month)
			//    e.IsValid = false;
		}

		
    }
}