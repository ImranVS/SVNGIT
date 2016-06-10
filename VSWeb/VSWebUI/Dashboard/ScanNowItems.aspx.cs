using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebBL;
using VSWebDO;
using DevExpress.Web;
using DevExpress.XtraGauges.Core.Drawing;
using DevExpress.Web.ASPxGauges.Gauges.Linear;
using DevExpress.Web.ASPxGauges.Base;
using DevExpress.Web.ASPxGauges;
using DevExpress.Web.ASPxGauges.Gauges;
using DevExpress.XtraGauges.Base;
using System.Drawing;
using DevExpress.XtraGauges.Core.Base;
using System.Text;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Xml;


namespace VSWebUI.Configurator
{
	public partial class ScanNowItems : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		DataRow myRow = null;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (!IsPostBack)
				{
					filllotusScanNowItems();
					Session["myRow"] = null;
				}
				
				

				StatusListPopupMenu.Items.Clear();
				if (Session["Isconfigurator"] != null)
				{
					if (Session["Isconfigurator"].ToString() == "True")
					{
						DevExpress.Web.MenuItem item = new DevExpress.Web.MenuItem();
						item.Text = "Scan First";
						item.Name = "ScanNow";
						StatusListPopupMenu.Items.Add(item);
						//DevExpress.Web.MenuItem item1 = new DevExpress.Web.MenuItem();
						//item1.Text = "Edit in Configurator";
						//item1.Name = "EditConfigurator";
						//StatusListPopupMenu.Items.Add(item1);
						//DevExpress.Web.MenuItem item2 = new DevExpress.Web.MenuItem();
						//item2.Text = "Suspend Temporarily";
						//item2.Name = "Suspend";
						//StatusListPopupMenu.Items.Add(item2);
					}
					else
					{
						DevExpress.Web.MenuItem item = new DevExpress.Web.MenuItem();
						item.Text = "Scan First";
						item.Name = "ScanNow";
						StatusListPopupMenu.Items.Add(item);
					}
				}
				else
				{
					DevExpress.Web.MenuItem item = new DevExpress.Web.MenuItem();
					item.Text = "Scan First";
					item.Name = "ScanNow";
					StatusListPopupMenu.Items.Add(item);
					filllotusScanNowItems();
				}
			}
			else
			{
				
				FilllotusScanNowItemsfromSession();
			}
				

		}



       

		public void filllotusScanNowItems()
        {
            try
            {
                DataTable ScanSettings = new DataTable();

				ScanSettings = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.Scangetdata();

				if (ScanSettings.Rows.Count > 0)
                {
					Session["ScanSettings"] = ScanSettings;
					GvScanNowItems.DataSource = ScanSettings;
					GvScanNowItems.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally
            {
            }
        }

		public void FilllotusScanNowItemsfromSession()
		{
			try
			{
				DataTable ScanSettings = new DataTable();

				//ScanSettings = VSWebBL.ConfiguratorBL.SametimeServerBL.Ins.Scangetdata();
				ScanSettings = Session["ScanSettings"] as DataTable;

				if (ScanSettings.Rows.Count > 0)
				{
					Session["ScanSettings"] = ScanSettings;
					GvScanNowItems.DataSource = ScanSettings;
					GvScanNowItems.DataBind();
				}
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally
			{
			}
		}


		protected void StatusListMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{

		}
		protected void StatusListPopupMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{
			//VSPLUS-766, Mukund, 15Sep14 
			ErrorMsg.InnerHtml = "";
			ErrorMsg.Style.Value = "display: none";

			string name = "";
			//if (Request.QueryString["server"] != "" && Request.QueryString["server"] != null)
			//{
			int focusrow = GvScanNowItems.FocusedRowIndex;

			if (e.Item.Name == "ScanNow")
			{
				if (focusrow > -1)
				{
					myRow = GvScanNowItems.GetDataRow(focusrow);
					Session["myRow"] = myRow;
					try
					{
						Status StatusObj = new Status();
						StatusObj.Name = myRow["sname"].ToString();
						StatusObj.Type = myRow["svalue"].ToString();

						bool bl = false;
						
							bl = VSWebBL.SettingBL.SettingsBL.Ins.UpdateScanFirstvalue(StatusObj.Name, StatusObj.Type, VSWeb.Constants.Constants.SysString);
							filllotusScanNowItems();
					}
					catch (Exception ex)
					{
						//myUserName = "";
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						throw ex;
					}
				}
				else
				{
					msgPopupControl.HeaderText = "Scan Now";
					msgLabel.Text = "Please select a device in the grid.";
					msgPopupControl.ShowOnPageLoad = true;
					YesButton.Visible = false;
					NOButton.Visible = false;
					CancelButton.Visible = true;
					CancelButton.Text = "OK";
				}
			}
			
		}

		
    }
}