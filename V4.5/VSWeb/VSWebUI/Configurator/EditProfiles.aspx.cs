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
	public partial class EditProfiles : System.Web.UI.Page
	{
		int ID;
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
		string ServerType;
		string roletype;
		string ProfileName;
		bool checkedvalue;
		bool isValid = true;
		
		string ActiveDirectorySession = "Active Directory";
		protected DataTable ProfilesDataTable = null;
		
		
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.QueryString["id"] == null)
			{
				Response.Redirect("~/Configurator/ManageProfiles.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
				Context.ApplicationInstance.CompleteRequest();
			}
			else
			{
				ProfileNames ProfileObject = new ProfileNames();
				ID = Convert.ToInt32(Request.QueryString["id"]);
				ProfileObject.ID = ID;
				//DataTable dt = VSWebBL.ConfiguratorBL.TravelerBL.Ins.GetValuebyID(TravelerObject);
				DataTable dt = VSWebBL.SecurityBL.ProfilesNamesBL.Ins.GetValuebyID(ProfileObject);
				string name = dt.Rows[0]["ProfileName"].ToString();
                //4/15/2015 NS modified
				//reglabel.Text = " Profiles for" + " -  " + name;
                reglabel.InnerHtml = "Profiles for " + name;
                lblprofilename.Text = name;
			}
			//pnlAreaDtls.Style.Add("visibility", "hidden");
			//if (!IsPostBack)
			//{

			//    FillProfilesGrid(ServerType,roletype,lblprofilename.Text);

			//}
			if (!IsPostBack && !IsCallback)
			{
				Session["Profiles"] = null;
				Session["ProfileId"] = "0";
				//ProfileTextBox.Enabled = true;
				FillServerTypeComboBox(ID);
			}
			else
			{
				
				FillProfilesGridfromSession();
			}


		}

		private void FillServerTypeComboBox(int id)
		{
			DataTable ServerDataTable = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetAllDatawithprofileid(id);
			ServerTypeComboBox.DataSource = ServerDataTable;
			ServerTypeComboBox.TextField = "ServerType";
			ServerTypeComboBox.ValueField = "ServerType";
			ServerTypeComboBox.DataBind();
		}
		
		private void FillProfilesGridfromSession()
		{
			try
			{
				ProfilesDataTable = new DataTable();
				if (Session["Profiles"] != null && Session["Profiles"] != "")
				{
					ProfilesDataTable = (DataTable)Session["Profiles"];
				}
				if (ProfilesDataTable.Rows.Count > 0)
				{
					GridViewDataColumn column2 = ProfilesGridView.Columns["DefaultValue"] as GridViewDataColumn;
					DataTable dt = new DataTable();
					int startIndex = ProfilesGridView.PageIndex * ProfilesGridView.SettingsPager.PageSize;
					int endIndex = Math.Min(ProfilesGridView.VisibleRowCount, startIndex + ProfilesGridView.SettingsPager.PageSize);
					for (int i = startIndex; i < endIndex; i++)
					{
						if (ProfilesGridView.Selection.IsRowSelected(i))
						{
							ASPxTextBox txtValue = (ASPxTextBox)ProfilesGridView.FindRowCellTemplateControl(i, column2, "txtDefaultValue");
							ProfilesDataTable.Rows[i]["DefaultValue"] = txtValue.Text;

							checkedvalue = Convert.ToBoolean(ProfilesDataTable.Rows[i]["isSelected"] = "true");
						}
						else
						{
							ProfilesDataTable.Rows[i]["isSelected"] = "false";
							checkedvalue = Convert.ToBoolean(ProfilesDataTable.Rows[i]["isSelected"] = "false");
						}

					}
					
					ProfilesGridView.DataSource = ProfilesDataTable;
					ProfilesGridView.DataBind();
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		protected void ServerTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ServerTypeComboBox.SelectedIndex != -1)
			{
				Session["DataServers"] = null;
				Session["Profiles"] = null;

				if (ServerTypeComboBox.SelectedItem.Text != "")
				{
					// CollapseAllButton.Visible = true;
					// ApplyServersButton.Visible = true;
					//ExchangeRolesComboBox.SelectedIndex = -1;
					//ExchangeRolesComboBox.Visible = false;
					//ExchangeRolesLabel.Visible = false;
					//ExchangeRolesComboBox.Attributes.Add("visibility", "hidden");
					//ExchangeRolesLabel.Attributes.Add("visibility", "hidden"); 
					ApplyServersButton.Visible = true;
					canclbuttn.Visible = true;
					FillProfilesGrid(ServerTypeComboBox.SelectedItem.Text, "",lblprofilename.Text);
				}
			}
			//12/12/2013 NS added
			errorDiv.Style.Value = "display: none;";
			//errorDiv2.Style.Value = "display: none";

			successDiv.Style.Value = "display: none";
			emptyDiv3.Style.Value = "display: none";
			// tblServer.Style.Value = "display: block";
		}
		
		protected void ProfilesGridView_OnPageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ServerSettingsEditor|ProfilesGridView", ProfilesGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		
		protected void CancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Configurator/ManageProfiles.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
		}

		protected void ApplyServersButton_Click(object sender, EventArgs e)
		{
			errorDiv.Style.Value = "display: none;";
			successDiv.Style.Value = "display: none;";
            List<object> fieldValues = ProfilesGridView.GetSelectedFieldValues(new string[] { "RelatedField", "DefaultValue", "RelatedTable", });	
			int profileId = 0;
			bool Update = false;
			bool insert = false;
			string AttributeErrors = "";
			string AppliedAttribute = "";
			if (fieldValues.Count > 0)
			{
				int ProfileMasterID = 0;
				
				DataTable ProfilesDataTable = new DataTable();
				if (Session["Profiles"] != null && Session["Profiles"] != "")
				{
					ProfilesDataTable = (DataTable)Session["Profiles"];
				}
				for (int i = 0; i < ProfilesDataTable.Rows.Count; i++)
				{
					//ProfilesDataTable.Rows[0]["isSelected"] = "True";
					if (ProfilesDataTable.Rows[i]["isSelected"].ToString() == "True")
					{
						ProfileMasterID = Convert.ToInt32(ProfilesDataTable.Rows[i]["ID"]);
						//string profileid = Convert.ToString( LocationComboBox.SelectedItem.Value);
						profileId = Convert.ToInt32(Request.QueryString["id"]);

						if (profileId == 0)
						{
							Update = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateProfiles(CollectDataForProfiles(ProfilesDataTable.Rows[i]), ProfilesDataTable.Rows[i]["ID"].ToString());
						}
						else
						{
							Update = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateeditProfiles(CollectDataForProfiles(ProfilesDataTable.Rows[i]), profileId.ToString(), ProfileMasterID.ToString());
						}

						if (Update == false)
						{
							if (AttributeErrors == "")
							{
								AttributeErrors += ProfilesDataTable.Rows[i]["AttributeName"].ToString();
							}
							else
							{
								AttributeErrors += ", " + ProfilesDataTable.Rows[i]["AttributeName"].ToString();
							}
						}
						else
						{
							if (AppliedAttribute == "")
							{
								AppliedAttribute += ProfilesDataTable.Rows[i]["AttributeName"].ToString();
							}
							else
							{
								AppliedAttribute += ", " + ProfilesDataTable.Rows[i]["AttributeName"].ToString();
							}
						}
					}
					else
					{
						//if (ProfilesDataTable.Rows[i]["isSelected"].ToString() == "false")
						//{
							bool isSelected = false;
							ProfileMasterID = Convert.ToInt32(ProfilesDataTable.Rows[i]["ID"]);
							//string profileid = Convert.ToString( LocationComboBox.SelectedItem.Value);
							profileId = Convert.ToInt32(Request.QueryString["id"]);

							if (profileId == 0)
							{
								Update = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateProfilesunselect(isSelected, ProfilesDataTable.Rows[i]["ID"].ToString());
							}
							else
							{
								Update = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateeditProfilesisselected(isSelected, profileId.ToString(), ProfileMasterID.ToString());
							}


						//}
					}
				}
				if (AttributeErrors != "")
				{
					//2/26/2014 NS modified
					//Error.InnerHtml = "The default values for " + SessionName + " attributes(s): " + AttributeErrors + " were NOT updated.";
					//10/6/2014 NS modified for VSPLUS-990
					errorDiv.InnerHtml = "The following default values for " + AppliedAttribute + " were NOT updated: " + AttributeErrors + " ." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: block";
				}
				if (AppliedAttribute != "")
				{
					//2/26/2014 NS modified
					//Success.InnerHtml = "The default values for " + SessionName + " attributes(s): " + AppliedAttribute + " were updated.";
					//10/6/2014 NS modified for VSPLUS-990
					successDiv.InnerHtml = "The following default values for " + AppliedAttribute + " were successfully updated: " + AppliedAttribute + "." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					successDiv.Style.Value = "display: block";
				}
			}
			else
			{
				//2/26/2014 NS modified
				//Error.InnerHtml = "Kindly select the required attributes to update their default values";
				//10/6/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = "Please select at least one attribute to update its default values." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
			}
		}

		private void FillProfilesGrid(string ServerType,string roletype, string ProfileName)
		{
			object sumObject;
			int idvaue;
			this.ProfilesGridView.DataSource = null;
			try
			{
				ProfilesDataTable = new DataTable();
				DataSet ProfilesDataSet = new DataSet();

				ProfilesDataTable = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.GetAllDataByServerType(ServerType, "", ProfileName);

				DataColumn[] columns = new DataColumn[1];
				columns[0] = ProfilesDataTable.Columns["ID"];
				ProfilesDataTable.PrimaryKey = columns;
				if (ProfilesDataTable.Rows.Count > 0)
				{
					sumObject = ProfilesDataTable.Compute("Max(ID)", "");
					idvaue = Convert.ToInt32(sumObject);
					for (int i = 0; i < ProfilesDataTable.Rows.Count; i++)
					{
						if (ProfilesDataTable.Rows[i]["isSelected"].ToString() != "")
						{

							bool chkvalue = Convert.ToBoolean(ProfilesDataTable.Rows[i]["isSelected"]);
							if (chkvalue == true)
							{

							}
						}
					}
					ProfilesDataTable.PrimaryKey = new DataColumn[] { ProfilesDataTable.Columns["ID"] };
				}
				
				
				
				Session["Profiles"] = ProfilesDataTable;
				ProfilesGridView.DataSource = ProfilesDataTable;
				ProfilesGridView.DataBind();
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		#region UpdateDefaults

		private ProfilesMaster CollectDataForProfiles(DataRow ProfilesRow)
		{
			try
			{
				ProfilesMaster ProfilesObject = new ProfilesMaster();
				ProfileNames Profilesnamesobj = new ProfileNames();
				ProfilesObject.ServerType = ProfilesRow["ServerType"].ToString();
				ProfilesObject.RelatedTable = ProfilesRow["RelatedTable"].ToString();
				ProfilesObject.RelatedField = ProfilesRow["RelatedField"].ToString();
				ProfilesObject.DefaultValue = ProfilesRow["DefaultValue"].ToString();
				ProfilesObject.UnitOfMeasurement = ProfilesRow["UnitofMeasurement"].ToString();
				ProfilesObject.AttributeName = ProfilesRow["AttributeName"].ToString();
				ProfilesObject.ProfileId = Convert.ToInt32(ProfilesRow["ProfileId"].ToString());
				ProfilesObject.isSelected = Convert.ToBoolean(ProfilesRow["isSelected"].ToString());
				//ProfilesObject.ProfileId = Convert.ToInt32(ProfileComboBox.SelectedItem.Value);
				//Profilesnamesobj.ProfileName = ProfileComboBox.SelectedItem.Text;
				return ProfilesObject;
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}


		protected void UpdateAttributeDefaults(string SessionName, ASPxGridView ProfilesGridView, HtmlGenericControl Error, HtmlGenericControl Success)
		{
			Error.Style.Value = "display: none;";
			Success.Style.Value = "display: none";
			List<object> fieldValues = ProfilesGridView.GetSelectedFieldValues(new string[] { "RelatedField", "DefaultValue", "RelatedTable" });
			
			bool Update = false;
			int profileid=0;
			string AttributeErrors = "";
			string AppliedAttribute = "";
			if (fieldValues.Count > 0)
			{
				int ProfileMasterID = 0;
				DataTable ProfilesDataTable = new DataTable();
				if (Session[SessionName] != null && Session[SessionName] != "")
				{
					ProfilesDataTable = (DataTable)Session[SessionName];
				}
				for (int i = 0; i < ProfilesDataTable.Rows.Count; i++)
				{
					if (ProfilesDataTable.Rows[i]["isSelected"].ToString() == "true")
					{
						ProfileMasterID = Convert.ToInt32(ProfilesDataTable.Rows[i]["ID"]);

						Update = VSWebBL.SecurityBL.ProfilesMasterBL.Ins.UpdateProfiles(CollectDataForProfiles(ProfilesDataTable.Rows[i]), ProfilesDataTable.Rows[i]["ID"].ToString());

						


						if (Update == false)
						{
							if (AttributeErrors == "")
							{
								AttributeErrors += ProfilesDataTable.Rows[i]["AttributeName"].ToString();
							}
							else
							{
								AttributeErrors += ", " + ProfilesDataTable.Rows[i]["AttributeName"].ToString();
							}
						}
						else
						{
							if (AppliedAttribute == "")
							{
								AppliedAttribute += ProfilesDataTable.Rows[i]["AttributeName"].ToString();
							}
							else
							{
								AppliedAttribute += ", " + ProfilesDataTable.Rows[i]["AttributeName"].ToString();
							}
						}
					}
				}
				if (AttributeErrors != "")
				{
					//2/26/2014 NS modified
					//Error.InnerHtml = "The default values for " + SessionName + " attributes(s): " + AttributeErrors + " were NOT updated.";
					//10/6/2014 NS modified for VSPLUS-990
					Error.InnerHtml = "The following default values for " + SessionName + " were NOT updated: " + AttributeErrors + " ." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					Error.Style.Value = "display: block";
				}
				if (AppliedAttribute != "")
				{
					//2/26/2014 NS modified
					//Success.InnerHtml = "The default values for " + SessionName + " attributes(s): " + AppliedAttribute + " were updated.";
					//10/6/2014 NS modified for VSPLUS-990
					Success.InnerHtml = "The following default values for " + SessionName + " were successfully updated: " + AppliedAttribute + "." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					Success.Style.Value = "display: block";
				}
			}
			else
			{
				//2/26/2014 NS modified
				//Error.InnerHtml = "Kindly select the required attributes to update their default values";
				//10/6/2014 NS modified for VSPLUS-990
				Error.InnerHtml = "Please select at least one attribute to update its default values." +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				Error.Style.Value = "display: block";
			}
		}
		protected void ProfilesGridView_PreRender(object sender, EventArgs e)
		{
			try
			{

				if (isValid)
				{

					ASPxGridView ProfilesGridView = (ASPxGridView)sender;
					for (int i = 0; i < ProfilesGridView.VisibleRowCount; i++)
					{
						if (ProfilesGridView.GetRowValues(i, "isSelected").ToString() != "")
							ProfilesGridView.Selection.SetSelection(i, (bool)ProfilesGridView.GetRowValues(i, "isSelected") == true);
					}

				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }

		}

		#endregion
		
	}
}
