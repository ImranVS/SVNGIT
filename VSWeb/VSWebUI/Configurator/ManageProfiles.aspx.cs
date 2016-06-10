using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using VSWebDO;
using VSWebBL;
using System.Web.UI.HtmlControls;
using System.Data;
using DevExpress.Web;
using System.Collections;
using VSFramework;
using System.Drawing;
namespace VSWebUI
{
	public partial class ManageProfiles : System.Web.UI.Page
	{
		object msgprofile = "";
		DataTable ProfileNamesDataTable = null;
		static int locID = 0;
		private int _MaxUserImageSize = 0;//524288;
		private int _MaxUserImageWidth = 250;
		private int _MaxUserImageHeight = 45;
		private const string _ImageTypeInvalid = "Images are restricted to PNG.";
		private const string _ImageSizeInvalid = "Images are restricted to 512K.";
		private const string _ImageInvalidDimensions = "Images are restricted to 250 width x 45 height.";
		string savedfile;
		string DominoSession = "Domino";
		string SametimeSession = "Sametime";
		string ExchangeSession = "Exchange";
		string URLSession = "URL";
		string ActiveDirectorySession = "Active Directory";
		protected DataTable ProfilesDataTable = null;

		protected void Page_Load(object sender, EventArgs e)
		{
			pnlAreaDtls.Style.Add("visibility", "hidden");
			if (!IsPostBack)
			{

				FillprofileGrid();

			}
			if (!IsPostBack && !IsCallback)
			{
				Session["Profiles"] = null;
				Session["ProfileId"] = "0";
				Company cmpobj = new Company();
				cmpobj = VSWebBL.ConfiguratorBL.CompanyBL.Ins.GetLogo();
				Session[DominoSession] = "";
				Session[SametimeSession] = "";
				Session[ExchangeSession] = "";
				Session[URLSession] = "";
				Session[ActiveDirectorySession] = "";
				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "ServerSettingsEditor|ProfilesGridView")
						{
							ProfileGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
						if (dr[1].ToString() == "AdminTab|LocationsGridView")
						{
							ProfileGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
					}
				}
			}
			else
			{
				FillprofileGridfromSession();//profileNames

			}

		}//pro
		

		private void FillprofileGrid()
		{
			try
			{

				ProfileNamesDataTable = new DataTable();
				DataSet ProfileNameDataSet = new DataSet();
				ProfileNamesDataTable = VSWebBL.SecurityBL.ProfilesNamesBL.Ins.GetAllData();
				if (ProfileNamesDataTable.Rows.Count > 0)
				{
					DataTable dtcopy = ProfileNamesDataTable.Copy();
					dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
					Session["ProfileName"] = dtcopy;
					ProfileGridView.DataSource = ProfileNamesDataTable;
					ProfileGridView.DataBind();
				}
				else
				{
					ProfileGridView.DataSource = ProfileNamesDataTable;
					ProfileGridView.DataBind();
					Session["ProfileName"] = ProfileNamesDataTable;
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}//profile name

		private void FillprofileGridfromSession()
		{
			try
			{

				ProfileNamesDataTable = new DataTable();
				if (Session["ProfileName"] != "" && Session["ProfileName"] != null)
					ProfileNamesDataTable = (DataTable)Session["ProfileName"];
				if (ProfileNamesDataTable.Rows.Count > 0)
				{
					ProfileGridView.DataSource = ProfileNamesDataTable;
					ProfileGridView.DataBind();
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}//profile name

		protected DataRow GetRow(DataTable LocObject, IDictionaryEnumerator enumerator, int Keys)
		{
			//4/24/2013 NS added a fix for cases when there are no entries in the Locations table yet
			DataTable dataTable = new DataTable();
			if (LocObject != null)
			{
				dataTable = LocObject;
			}
			else
			{
				dataTable.Columns.Add("ProfileName");
			}
			DataRow DRRow = null;
			if (Keys == 0)

				DRRow = dataTable.NewRow();

			else
				DRRow = dataTable.Rows.Find(Keys);
			//IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
			enumerator.Reset();
			while (enumerator.MoveNext())
				DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
			return DRRow;
		}//pro

		protected void ProfileGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{
			
			if (Session["ProfileName"] != null && Session["ProfileName"] != "")
			{
				ProfileNamesDataTable = (DataTable)Session["ProfileName"];
				
			}
			else
			{
				ProfileNamesDataTable = null;

			}
			ASPxGridView gridView = (ASPxGridView)sender;
			UpdateProfilesData("Insert", GetRow(ProfileNamesDataTable, e.NewValues.GetEnumerator(), 0));
			gridView.CancelEdit();
			e.Cancel = true;
			FillprofileGrid();

		}//pro

		private void UpdateProfilesData(string Mode, DataRow LocationsRow)
		{
			if (Mode == "Insert")
			{
				Object ReturnValue = VSWebBL.SecurityBL.ProfilesNamesBL.Ins.InsertData1(CollectDataForLocations(Mode, LocationsRow));
				if (ReturnValue == "")
				{
					msgprofile = "Profile Already Exists";
					throw new ArgumentException("Profile Already Exists");
				}
			}
			if (Mode == "Update")
			{
				Object ReturnValue = VSWebBL.SecurityBL.ProfilesNamesBL.Ins.UpdateData1(CollectDataForLocations(Mode, LocationsRow));

			}
		}//pro

		private ProfileNames CollectDataForLocations(string Mode, DataRow LocationsRow)
		{
			try
			{
				ProfileNames ProfileNamesObject = new ProfileNames();
				ProfileNamesObject.ProfileName = LocationsRow["ProfileName"].ToString();
				if (Mode == "Update")
					ProfileNamesObject.ID = int.Parse(LocationsRow["ID"].ToString());

				return ProfileNamesObject;
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}//pro

		protected void ProfileGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
			ProfileNamesDataTable = (DataTable)Session["ProfileName"];
			ASPxGridView gridView = (ASPxGridView)sender;

			
			UpdateProfilesData("Update", GetRow(ProfileNamesDataTable, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0])));
			
			gridView.CancelEdit();
			e.Cancel = true;
			FillprofileGrid();
			
		}//pro

		protected void ProfileGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
			ProfileNames ProfileNamesObject = new ProfileNames();
			ProfileNamesObject.ID = Convert.ToInt32(e.Keys[0]);
			Object ReturnValue = VSWebBL.SecurityBL.ProfilesNamesBL.Ins.DeleteData1(ProfileNamesObject);
			Object profilenameobj = VSWebBL.SecurityBL.ServersBL.Ins.UpdateServersProfileName(ProfileNamesObject);

			ASPxGridView gridView = (ASPxGridView)sender;
			gridView.CancelEdit();
			e.Cancel = true;
			FillprofileGrid();
		}//pro
		protected void btn_Clickdelete(object sender, EventArgs e)
		{
			ImageButton bttnDel = (ImageButton)sender;
			ProfileNames ProfileNamesObject = new ProfileNames();
			ProfileNamesObject.ID = Convert.ToInt32(bttnDel.CommandArgument);
			locID = Convert.ToInt32(bttnDel.CommandArgument);
			string ProfileName = bttnDel.AlternateText;
			pnlAreaDtls.Style.Add("visibility", "visible");
			divmsg.InnerHtml = "Are you sure you want to delete the server " + ProfileName + "?";

		}//pro
		protected void btn_Clickeditserver(object sender, EventArgs e)
		{
			int ID;

			ImageButton btn = (ImageButton)sender;
			ID = Convert.ToInt32(btn.CommandArgument);
			int id = ID;
			ProfileNames ProfileNamesObject = new ProfileNames();

			if (id != null)
				ProfileNamesObject.ID = id;

			bool s = true;
			if (s == true)
			{

				Response.Redirect("~/Configurator/EditProfiles.aspx?id=" + id.ToString(), false);
				Context.ApplicationInstance.CompleteRequest();

			}

		}//pro

		protected void btn_OkClick(object sender, EventArgs e)
		{
			ProfileNames ProfileNamesObject = new ProfileNames();
			ProfileNamesObject.ID = locID;
			Object returnValue = VSWebBL.SecurityBL.ProfilesNamesBL.Ins.DeleteData1(ProfileNamesObject);
			FillprofileGrid();
		}//pro

		protected void btn_CancelClick(object sender, EventArgs e)
		{
			FillprofileGrid();
		}//pro



		protected void ProfileGridView_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AdminTab|LocationsGridView", ProfileGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}//pro
		
		protected void btn_clickcopyprofile(object sender, EventArgs e)
		{

			CopyProfilePopupControl.ShowOnPageLoad = true;
			CopyProfileTextBox.Visible = true;
			OKCopy.Visible = true;
			KeyOKSave.Visible = false;
			
		}//popup
		protected void OKCopy_Click(object sender, EventArgs e)
		{
			bool check = false;

			check = VSWebBL.SecurityBL.ProfilesNamesBL.Ins.GetDataCopy(CopyProfileTextBox.Text);

			FillprofileGrid();
			Response.Redirect("~/Configurator/ManageProfiles.aspx");
		}//popup
		protected void KeyOKSave_Click(object sender, EventArgs e)
		{
			CopyProfilePopupControl.ShowOnPageLoad = false;
		}//popup

		protected void ProfileGridView_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
		{

			if (e.VisibleIndex == -1) return;

			if (e.ButtonID == "deleteButton")
			{
				ASPxGridView grid = sender as ASPxGridView;
				string profileName = grid.GetRowValues(e.VisibleIndex, "ProfileName").ToString();
				if (profileName != "Default")
					 e.Visible = DevExpress.Utils.DefaultBoolean.True; 
				else
					e.Visible = DevExpress.Utils.DefaultBoolean.False;
			}
			
		}

	}
}
