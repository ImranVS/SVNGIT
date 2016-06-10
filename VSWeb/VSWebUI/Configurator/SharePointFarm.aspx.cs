using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebDO;

namespace VSWebUI.Configurator
{
	public partial class SharePointFarm : System.Web.UI.Page
	{
		bool isValid = true;
        protected int ServerId;
        protected int ServerID;
        protected int ServerTypeID;
        protected DataTable ExchangeDataTable = null;
		protected string FarmName;

		protected void Page_Load(object sender, EventArgs e)
		{
			ServerTypeID = 5;
			FarmName = Request.QueryString["Farm"];
			NameTextBox.Text = FarmName;
			if (!IsPostBack)
			{
				FillData();
                lblServer.InnerHtml = "Microsoft SharePoint Farms -" + " " + NameTextBox.Text;
			}
			else
			{
				
			}
		}

		protected void FillData()
		{
			DataTable dt = new DataTable();
			try
			{
				dt = VSWebBL.SharePointSettingsBL.Ins.GetFarmSettings(FarmName);
				if (dt.Rows.Count > 0)
				{
					TestAppTextBox.Text = dt.Rows[0]["TestApplicationURL"].ToString();
					LogonTest.Checked = Convert.ToBoolean(dt.Rows[0]["LogOnTest"]);
					SiteCollectionCreationTest.Checked = Convert.ToBoolean(dt.Rows[0]["SiteCreationTest"]);
					FileUploadTest.Checked = Convert.ToBoolean(dt.Rows[0]["FileUploadTest"]);
				}
			}
			catch (Exception ex)
			{

			}
		}

		protected void FormOkButton_Click(object sender, EventArgs e)
		{

			try
			{
				if (UpdateServersData())
				{
					Response.Redirect("~/Configurator/SharePointFarmGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					Context.ApplicationInstance.CompleteRequest();

				}

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
			
			
		}

		protected bool UpdateServersData()
		{
			SharePointFarmSettings settings = new SharePointFarmSettings();
			settings.FarmName = FarmName;
			settings.FileUploadTest = FileUploadTest.Checked;
			settings.LogOnTest = LogonTest.Checked;
			settings.SiteCreationTest = SiteCollectionCreationTest.Checked;
			settings.TestAppURL = TestAppTextBox.Text;

			return VSWebBL.SharePointSettingsBL.Ins.UpdateFarmSettings(settings);
		}

		protected void FormCancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Configurator/SharePointFarmGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
		}


	}
}