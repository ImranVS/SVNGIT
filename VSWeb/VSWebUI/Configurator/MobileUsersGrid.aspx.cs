using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSWebBL;
using System.Data;
using DevExpress.Web;
using System.Globalization;

namespace VSWebUI.Configurator
{
    public partial class MobileUsersGrid : System.Web.UI.Page
    {
		DataRow myRow = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.Title = "Mobile Users";
            if (!IsPostBack)
            {
                FillMobileDevicesGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "MobileUsersGrid|MobileUsersGridView")
                        {
                            NetworkDevicesGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
						if (dr[1].ToString() == "MobileUsersThGrid|MobileUsersThGridView")
						{
							MobUserThGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
                    }
                }
            }
            else
            {

				FillMobileDevicesGridfromSession();

            }
           
        }


		private void FillMobileDevicesGrid()
        {
            try
            {
				DataTable dt = VSWebBL.ConfiguratorBL.MobileUsersBL.Ins.SetGrid();
				NetworkDevicesGridView.DataSource = dt;
				Session["MobileDevicesDT"] = dt;
				NetworkDevicesGridView.DataBind();

				DataTable dt1 = VSWebBL.ConfiguratorBL.MobileUsersBL.Ins.SetThresholdGrid();
				MobUserThGrid.DataSource = dt1;
				Session["MobUserThGrid"] = dt1;
				MobUserThGrid.DataBind();
				
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }

        private void FillMobileDevicesGridfromSession()
        {
            try
            {
				
                DataTable DCTaskSettingsDataTable = new DataTable();
				if ((Session["MobileDevicesDT"] != null) )
                {
					DCTaskSettingsDataTable = (DataTable)Session["MobileDevicesDT"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                    if (DCTaskSettingsDataTable.Rows.Count > 0)
                    {
                        NetworkDevicesGridView.DataSource = DCTaskSettingsDataTable;
                        NetworkDevicesGridView.DataBind();
                    }
                }

				DataTable DTThreshold = new DataTable();
				if ((Session["MobUserThGrid"] != null))
                {
					DTThreshold = (DataTable)Session["MobUserThGrid"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
					if (DTThreshold.Rows.Count > 0)
                    {
						MobUserThGrid.DataSource = DTThreshold;
						MobUserThGrid.DataBind();
                    }
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

        protected void NetworkDevicesGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
			
        }

        protected void NetworkDevicesGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

        }

		public string SetIcon(GridViewDataItemTemplateContainer Container)
		{
			bool imgset = false;
			System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)Container.FindControl("IconImage");
			Label lbl = (Label)Container.FindControl("lblIcon");
			string lblOS = lbl.Text;
			CultureInfo culture = new CultureInfo("");
			//8/16/2013 NS modified
			//if (lblOS.Contains("Android") == true)
			if (culture.CompareInfo.IndexOf(lblOS, "Android", CompareOptions.IgnoreCase) >= 0)
			{
				img.ImageUrl = "~/images/icons/android-icon.png";
				imgset = true;
			}

			//if (lblOS.Contains("Apple") == true)
			//5/6/2014 NS modified for VSPLUS-588
			if (culture.CompareInfo.IndexOf(lblOS, "Apple", CompareOptions.IgnoreCase) >= 0 ||
				culture.CompareInfo.IndexOf(lblOS, "iOS", CompareOptions.IgnoreCase) >= 0)
			{
				img.ImageUrl = "~/images/icons/os_icon_mac.png";
				imgset = true;
			}

			//8/15/2013 NS added
			//if (lblOS.Contains("RIM") == true)
			if (culture.CompareInfo.IndexOf(lblOS, "RIM", CompareOptions.IgnoreCase) >= 0)
			{
				img.ImageUrl = "~/images/icons/rim.png";
				imgset = true;
			}
			//if (lblOS.Contains("Win") == true)
			if (culture.CompareInfo.IndexOf(lblOS, "Win", CompareOptions.IgnoreCase) >= 0)
			{
				img.ImageUrl = "~/images/icons/winphone.png";
				imgset = true;
			}

			if (!imgset)
			{
				img.ImageUrl = "~/images/icons/phone.png";
				imgset = true;
			}
			return "";
		}

        protected void NetworkDevicesGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }

       protected void UsersGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			
		}
		protected void UsersGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{
			
		}
		protected void UsersGrid_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MobileUsersGrid|MobileUsersGridView", NetworkDevicesGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void StatusListMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{

		}

		protected void StatusListPopupMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{

			if (e.Item.Name == "Suspend")
			{
				if (NetworkDevicesGridView.FocusedRowIndex > -1)
				{
					myRow = NetworkDevicesGridView.GetDataRow(NetworkDevicesGridView.FocusedRowIndex);
					Session["MyDeviceId"] = myRow["DeviceID"];
					SuspendPopupControl.HeaderText = "Sync Duration";
					//ASPxLabel9.Text = myRow["Name"].ToString();
					SuspendPopupControl.ShowOnPageLoad = true;
				}
				
			}
		}

		protected void BtnApply_Click(object sender, EventArgs e)
		{
			object ReturnValue = VSWebBL.ConfiguratorBL.MobileUsersBL.Ins.InsertData(Session["MyDeviceId"].ToString(), Convert.ToInt32(TbDuration.Text));
			FillMobileDevicesGrid();
			SuspendPopupControl.ShowOnPageLoad = false;
		}

		protected void MobUserThGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			
		}
		protected void MobUserThGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{
			
		}
		protected void MobUserThGrid_PageSizeChanged(object sender, EventArgs e)
		{
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MobileUsersThGrid|MobileUsersThGridView", MobUserThGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}

		protected void MobUserThGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
			try
			{
				
				object ReturnValue = VSWebBL.ConfiguratorBL.MobileUsersBL.Ins.DeleteThresholdData(e.Keys[0].ToString());

				ASPxGridView gridview = (ASPxGridView)sender;
				gridview.CancelEdit();
				e.Cancel = true;
				FillMobileDevicesGrid();
				//Response.Redirect("MobileUsersGrid.aspx");
			}
			catch (Exception ex)
			{
				//Log.Entry.Ins.Write(Server.MapPath("~/LogFiles/"), "VSPlusLog.txt", DateTime.Now.ToString() + " Error in Page: " +
				//    Request.Url.AbsolutePath + ", Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
				//    ", Error: " + ex.ToString());
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		protected void ExportPdfButton_Click(object sender, EventArgs e)
		{
			UsersGridViewExporter.WritePdfToResponse();
		}

		protected void ExportXlsButton_Click(object sender, EventArgs e)
		{
			UsersGridViewExporter.WriteXlsToResponse();
		}

		protected void ExportXlsxButton_Click(object sender, EventArgs e)
		{
			UsersGridViewExporter.WriteXlsxToResponse();
		}

        protected void MobUserThGrid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

        }

        protected void MobUserThGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
                
            ASPxGridView gridView = (ASPxGridView)sender;
            String syncTime = e.NewValues["SyncTimeThreshold"].ToString();
            String deviceId = e.OldValues["DeviceId"].ToString();

            VSWebBL.ConfiguratorBL.MobileUsersBL.Ins.UpdateData(deviceId, Convert.ToInt32(syncTime));

            MobUserThGrid.CancelEdit();
            e.Cancel = true;

            FillMobileDevicesGrid();
        }

        protected void MobUserThGrid_RowValidation(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if(e.NewValues["SyncTimeThreshold"] == null || (Convert.ToInt32(e.NewValues["SyncTimeThreshold"]) < 20 || Convert.ToInt32(e.NewValues["SyncTimeThreshold"]) > 240) )
                e.RowError="Sync Duration must be between 20 and 240 minutes.";
                

        }
    }
}