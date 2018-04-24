using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebBL;
using DevExpress.Web;
using VSWebDO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Threading;
using System.Collections;
namespace VSWebUI.Configurator
{
	public partial class MessageLatencyTest : System.Web.UI.Page
	{
		bool isValid = true;
		int id;
		int yellowthershold;
		int latency;
		bool checkedvalue;
		protected DataTable ExchangeSettingsDataTable = null;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{

				Session["Submenu"] = "";
				FillExchangeServerGrid();
				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "MSServersGrid|MSServerGridView")
						{
							MessageLatencyTestgrd.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
					}
				}
			}
			else
			{

				FillExchangeServerGridfromSession();

			}
			if (Session["ExchangeUpdateStatus"] != null)
			{
				if (Session["ExchangeUpdateStatus"].ToString() != "")
				{
					successDiv.InnerHtml = "Exchange information for <b>" + Session["ExchangeUpdateStatus"].ToString() +
						"</b> updated successfully.";
					successDiv.Style.Value = "display: block";
					Session["ExchangeUpdateStatus"] = "";
				}
			}
		}
		private void FillExchangeServerGrid()
		{
			object sumObject;
			int idvaue;
			string name = "EnableLatencyReport";
			string interval = "LatencyScanInterval";
			try
			{
				DataTable ExchangeSettingsDataTable = new DataTable();
				string svalue = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue(name);
				if (svalue == "True")
				{
					Enreportcheckbox.Checked = true;
				}
				string scanvalue = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue(interval);
				if ((scanvalue == null) || (scanvalue == ""))
				{
					scantext.Text = "8";
				}
				if ((scanvalue != null) && (scanvalue != ""))
				{
					scantext.Text = scanvalue;
				}
				ExchangeSettingsDataTable = VSWebBL.ExchangeBAL.Ins.GetAllData1();
				DataColumn[] columns = new DataColumn[1];
				columns[0] = ExchangeSettingsDataTable.Columns["ServerId"];
				ExchangeSettingsDataTable.PrimaryKey = columns;
				if (ExchangeSettingsDataTable.Rows.Count > 0)
				{
					sumObject = ExchangeSettingsDataTable.Compute("Max(ServerId)", "");
					idvaue = Convert.ToInt32(sumObject);
					for (int i = 0; i < ExchangeSettingsDataTable.Rows.Count; i++)
					{
						if (ExchangeSettingsDataTable.Rows[i]["EnableLatencyTest"].ToString() != "")
						{

							bool chkvalue = Convert.ToBoolean(ExchangeSettingsDataTable.Rows[i]["EnableLatencyTest"]);
							if (chkvalue == true)
							{

							}
						}
					}
					ExchangeSettingsDataTable.PrimaryKey = new DataColumn[] { ExchangeSettingsDataTable.Columns["ServerId"] };
				}
				Session["ExchangeServer"] = ExchangeSettingsDataTable;
				MessageLatencyTestgrd.DataSource = ExchangeSettingsDataTable;
				MessageLatencyTestgrd.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void FillExchangeServerGridfromSession()
		{

	       
			try
			{
				DataTable ExchangeSettingsDataTable = new DataTable();
				if (Session["ExchangeServer"] != null && Session["ExchangeServer"] != "")
					ExchangeSettingsDataTable = (DataTable)Session["ExchangeServer"];//VSWebBL.ConfiguratorBL.ExchangePropertiesBL.Ins.GetAllData();
				if (ExchangeSettingsDataTable.Rows.Count > 0)
				{
					GridViewDataColumn column3 = MessageLatencyTestgrd.Columns["LatencyYellowThreshold"] as GridViewDataColumn;
					GridViewDataColumn column4 = MessageLatencyTestgrd.Columns["LatencyRedThreshold"] as GridViewDataColumn;
					int startIndex = MessageLatencyTestgrd.PageIndex * MessageLatencyTestgrd.SettingsPager.PageSize;
					int endIndex = Math.Min(MessageLatencyTestgrd.VisibleRowCount, startIndex + MessageLatencyTestgrd.SettingsPager.PageSize);
					for (int i = startIndex; i < endIndex; i++)
					{
						ASPxTextBox txtValue = (ASPxTextBox)MessageLatencyTestgrd.FindRowCellTemplateControl(i, column3, "txtyellowthreshValue");
						ASPxTextBox txtValue2 = (ASPxTextBox)MessageLatencyTestgrd.FindRowCellTemplateControl(i, column4, "txtredthreshValue");
						
						ExchangeSettingsDataTable.Rows[i]["LatencyYellowThreshold"] = (string.IsNullOrEmpty( txtValue.Text.Trim())? "0" :txtValue.Text.Trim()) ;
						
						ExchangeSettingsDataTable.Rows[i]["LatencyRedThreshold"] = (string.IsNullOrEmpty(txtValue2.Text.Trim())? "0" :txtValue2.Text.Trim());

						
						if (MessageLatencyTestgrd.Selection.IsRowSelected(i))
						{
							checkedvalue = Convert.ToBoolean(ExchangeSettingsDataTable.Rows[i]["EnableLatencyTest"] = "true");

                           	}
						else
						{
							ExchangeSettingsDataTable.Rows[i]["EnableLatencyTest"] = "false";
							checkedvalue = Convert.ToBoolean(ExchangeSettingsDataTable.Rows[i]["EnableLatencyTest"] = "false");
						}
						id = Convert.ToInt32(MessageLatencyTestgrd.GetRowValues(i, "ServerId"));

						yellowthershold = Convert.ToInt32(string.IsNullOrEmpty(txtValue.Text.Trim()) ? "0" : txtValue.Text.Trim());
						latency = Convert.ToInt32(string.IsNullOrEmpty(txtValue2.Text.Trim())? "0" : txtValue2.Text.Trim());
						object dt = VSWebBL.ExchangeBAL.Ins.updateEnableLatencyTest(id, yellowthershold, latency, checkedvalue);
					}
				}
				MessageLatencyTestgrd.DataSource = ExchangeSettingsDataTable;
				MessageLatencyTestgrd.DataBind();

			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		protected void FormSaveButton_Click(object sender, EventArgs e)
		{

			Settings st = new Settings();
			st.svalue = Enreportcheckbox.Checked.ToString();
			bool blret = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSettings(st, "EnableLatencyReport");
			st.svalue = scantext.Text;
			blret = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSettings(st, "LatencyScanInterval");
			DataTable ExchangeSettingsDataTable = (DataTable)Session["ExchangeServer"];
			FillExchangeServerGrid();
			List<object> LatencyYellowThreshold = MessageLatencyTestgrd.GetSelectedFieldValues(new string[] { "LatencyYellowThreshold" });
			if (LatencyYellowThreshold.Count != 0)
			{
				successDiv.Style.Value = "display: block";
				errorDiv.Style.Value = "display: none";
			}
			else
			{
				errorDiv.Style.Value = "display: block";
				successDiv.Style.Value = "display: none";
			}
			//if (blret == true)
			//{
			//    successDiv.Style.Value = "display: block";
			//}
			//else
			//{
			//    errorDiv.Style.Value = "display: none";
			//}

		}
		protected void MessageLatencyTestgrd_PreRender(object sender, EventArgs e)
		{
			try
			{

				if (isValid)
				{

					ASPxGridView MessageLatencyTestgrd = (ASPxGridView)sender;
					for (int i = 0; i < MessageLatencyTestgrd.VisibleRowCount; i++)
					{
						if (MessageLatencyTestgrd.GetRowValues(i, "EnableLatencyTest").ToString() != "")
							MessageLatencyTestgrd.Selection.SetSelection(i, (bool)MessageLatencyTestgrd.GetRowValues(i, "EnableLatencyTest") == true);
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
		protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{



			if (e.Item.Name == "ExchangeHeatMap")
			{
				Response.Redirect("~/Dashboard/ExchangeHeatMap.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
				Context.ApplicationInstance.CompleteRequest();
			}
			else
			{
			}


		}



	}
}






