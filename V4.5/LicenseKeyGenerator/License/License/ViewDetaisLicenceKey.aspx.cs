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
	public partial class ViewDetaisLicenceKey : System.Web.UI.Page
	{
		int ID;
		protected void Page_Load(object sender, EventArgs e)
		{

			if (!IsPostBack)
			{
				Session["Licence"] = null;
				if (Session["li"] != null && Session["li"] != "")
				{
					FillUsersGrid(Convert.ToInt32(Session["li"].ToString()));
				
				}
				FillUsersComboBox();
			}

			else
			{

				FillUsersGridfromSession();
			}
		}
		private void FillUsersGrid(int ID)
		{
			try
			{
				DataTable Licenceinformation = new DataTable();
				Licenceinformation = BL.LicenseKeyBL.Ins.GetLicenceInformation(ID);
				Session["Licence"] = Licenceinformation;
				LicenceUsersGridView.DataSource = Licenceinformation;
				LicenceUsersGridView.DataBind();
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

				DataTable Licenceinformation = new DataTable();
				if (Session["Licence"] != null && Session["Licence"] != "")
					Licenceinformation = (DataTable)Session["Licence"];//VSWebBL.ConfiguratorBL.WindowsPropertiesBL.Ins.GetAllData();
				if (Licenceinformation.Rows.Count > 0)
				{
					LicenceUsersGridView.DataSource = Licenceinformation;
					LicenceUsersGridView.DataBind();
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
		private void FillUsersComboBox()
		{
			DataTable UsersDataTable = BL.UsersBL.Ins.Getusers();
			UsersComboBox.DataSource = UsersDataTable;
			UsersComboBox.TextField = "LoginName";
			
			UsersComboBox.ValueField = "ID";
			UsersComboBox.DataBind();
			if(Session["li"]!=null && Session["li"]!="")

			UsersComboBox.SelectedIndex = Convert.ToInt32(Session["li"].ToString());
		}
		protected void UsersComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (UsersComboBox.SelectedIndex != -1)
			{
				if (UsersComboBox.SelectedItem.Text != "")
				{
					
					ID = Convert.ToInt32(UsersComboBox.SelectedItem.Value);
					//ViewState["ID"] = ID;
					Session["li"] = ID;
					FillUsersGrid(Convert.ToInt32(Session["li"].ToString()));
				
				}
			}

		}
	}
	}