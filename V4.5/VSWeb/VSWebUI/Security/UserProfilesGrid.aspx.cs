using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebBL;
using VSWebDO;
using System.Data;

using DevExpress.Web;
using System.Runtime.InteropServices;

namespace VSWebUI
{
    public partial class UserProfilesGrid : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        //[DllImport("UserProfileLibrary.dll",EntryPoint="QueueUserProfile")]
        //public static extern void QueueUserProfile(string DeviceType, string DeviceName, string UserProfileType, string Details, string Location);
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "UserProfile Definitions";
            if (!IsPostBack)
            {              
                fillGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "UserProfilesGrid|UserProfileView")
                        {
                            UserProfileView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                fillGridfromSession();
            }
        }

        public void fillGrid()
        {
            DataTable UserProfilesTable = new DataTable();
            UserProfilesTable = VSWebBL.SecurityBL.UserProfileMasterBL.Ins.GetAllData();
            try
            {
                 Session["UserProfilesNameTable"] = UserProfilesTable;
                 UserProfileView.DataSource = UserProfilesTable;
                 UserProfileView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        public void fillGridfromSession()
        {
            DataTable UserProfilesTable = new DataTable();
            if(Session["UserProfilesNameTable"]!=""&&Session["UserProfilesNameTable"]!=null)
            UserProfilesTable = (DataTable)Session["UserProfilesNameTable"];

            try
            {
                UserProfileView.DataSource = UserProfilesTable;
                UserProfileView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }          
        
        }

        protected void UserProfileView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + "this.style.backgroundColor='#C0C0C0';");

            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");


            if (e.RowType == GridViewRowType.EditForm)
            {
              

                if (e.GetValue("ID") != " " && e.GetValue("ID") != null)
                {
                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
                        ASPxWebControl.RedirectOnCallback("UserProfiles.aspx?ProfileId=" + e.GetValue("ID"));
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                    catch (Exception ex)
                    {
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        //throw ex;
                    }
                   
                }
                else
                {
                    //Mukund: VSPLUS-844, Page redirect on callback
                    try
                    {
                        ASPxWebControl.RedirectOnCallback("UserProfiles.aspx");
                        Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    }
                    catch (Exception ex)
                    {
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                        //throw ex;
                    }
                   
                }
            }
        }

        protected void UserProfileView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }

        protected void UserProfileView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            UserProfileMaster UserProfileobj = new UserProfileMaster();
            UserProfileobj.ID = Convert.ToInt32(e.Keys[0]);
            VSWebBL.SecurityBL.UserProfileMasterBL.Ins.DeleteData(UserProfileobj);
            ASPxGridView gridview = (ASPxGridView)sender;
            gridview.CancelEdit();
            e.Cancel = true;
            fillGrid();

          }
       
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            //CreateTestUserProfilePopupControl.ShowOnPageLoad = false;
        }

        protected void UserProfileView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("UserProfilesGrid|UserProfileView", UserProfileView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

    }
}