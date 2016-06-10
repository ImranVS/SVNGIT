using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using VSWebDO;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Data.SqlClient;
using System.Threading;
using System.Collections;
using DevExpress.Web.ASPxTreeList;
using VSWebBL;

namespace VSWebUI.Configurator
{
	public partial class NodeSettingsProperties : System.Web.UI.Page
	{
		
		string Mode;
		
		protected void Page_Load(object sender, EventArgs e)
		{
			lblServerId.Text = Request.QueryString["ID"];
	
			int id=Convert.ToInt32( Request.QueryString["ID"]);
			if (Request.QueryString["ID"] != "" && Request.QueryString["ID"] != null)
			{
				Mode = "Update";
			}
			else 
			{
				Mode = "Insert";
				
			}
			if (Mode == "Insert")
			{
				VersionTextBox.Text = "1.0.0.0";
				AliveTextBox.Text = "0";
				FormDisableButton.Visible = false;
			}

			if (!IsPostBack)
			{

				FillCredentialsComboBox();
				fillLocationsCombobox();

				if (Request.QueryString["ID"] != "" && Request.QueryString["ID"] != null)
				{
					FillNodesData(id);
					
				}
				
				
			}
		
		}
		private void FillCredentialsComboBox()
		{
			DataTable CredentialsDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetCredentials();
			CredentialsComboBox.DataSource = CredentialsDataTable;
			CredentialsComboBox.TextField = "AliasName";
			CredentialsComboBox.ValueField = "ID";
			CredentialsComboBox.DataBind();
		}
		public void FillNodesData(int id)
		{
			DataTable dt = new DataTable();
			try
			{
				Nodes web = new Nodes();
				web.ID =Convert.ToInt32( Request.QueryString["ID"]);
				dt = VSWebBL.SecurityBL.NodesBL.Ins.GetAllDataByNames(web);
				if (dt.Rows.Count > 0)
				{
					NameTextBox.Text = dt.Rows[0]["Name"].ToString();
					HostNameTextBox.Text = dt.Rows[0]["HostName"].ToString();		
					AliveTextBox.Text = dt.Rows[0]["Alive"].ToString();
					VersionTextBox.Text = dt.Rows[0]["Version"].ToString();
					NodeTypeTextBox.Text = dt.Rows[0]["NodeType"].ToString();
					LoadFactorTextBox.Text = dt.Rows[0]["LoadFactor"].ToString();

					string isPriamry = dt.Rows[0]["IsConfiguredPrimaryNode"] != null && dt.Rows[0]["IsConfiguredPrimaryNode"].ToString() != "" ? dt.Rows[0]["IsConfiguredPrimaryNode"].ToString() : "false";
					IsPrimaryNodeCheckBox.Checked = bool.Parse(isPriamry);
					int CredentialsID = 0;
					if (dt.Rows[0]["CredentialsID"].ToString() != null && dt.Rows[0]["CredentialsID"].ToString() != "" && dt.Rows[0]["CredentialsID"].ToString() != "-1")
					{
						Credentials cred = new Credentials();
						cred.ID = Convert.ToInt32(dt.Rows[0]["CredentialsId"].ToString());
						CredentialsID = Convert.ToInt32(dt.Rows[0]["CredentialsId"].ToString());
						DataTable credntials = VSWebBL.SecurityBL.NodesBL.Ins.GetCredentialsBynameid(cred);
						//CredentialsComboBox.Text = Convert.ToString(CredentialsComboBox.Items.FindByValue(CredentialsID));
						CredentialsComboBox.Text = credntials.Rows[0]["AliasName"].ToString();
						for (int i = 0; i < CredentialsComboBox.Items.Count; i++)
						{
							if (CredentialsComboBox.Items[i].Value.ToString() == CredentialsID.ToString())
								CredentialsComboBox.Items[i].Selected = true;
						}
					}

					if (dt.Rows[0]["Location"].ToString() != null && dt.Rows[0]["Location"].ToString() != "")
					{

						LocationsComboBox.Text = dt.Rows[0]["Location"].ToString();
						for (int i = 0; i < LocationsComboBox.Items.Count; i++)
						{
							if (LocationsComboBox.Items[i].Value.ToString() == LocationsComboBox.ToString())
								LocationsComboBox.Items[i].Selected = true;
						}
					}

					if (dt.Rows[0]["isDisabled"].ToString() == "" || !(Convert.ToBoolean(dt.Rows[0]["isDisabled"].ToString())))
						FormDisableButton.Text = "Disable";
					else
						FormDisableButton.Text = "Enable";
				}
			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
		}

		private Nodes CollectDataForNodesPropertie()
		{
			try
			{
				
				Nodes NodesObject = new Nodes();
				if (Mode == "Update")
				  {
				     NodesObject.ID = Convert.ToInt32(Request.QueryString["ID"].ToString());
				  }
				NodesObject.Name = NameTextBox.Text;
				NodesObject.HostName = HostNameTextBox.Text;
				if (AliveTextBox.Text != "" && AliveTextBox.Text != null)
				{
					NodesObject.Alive = Convert.ToInt32(AliveTextBox.Text);
				}
				NodesObject.Version = VersionTextBox.Text;
				
				NodesObject.NodeType = NodeTypeTextBox.Text;
				if (LoadFactorTextBox.Text != "" && LoadFactorTextBox.Text != null)
				{
					NodesObject.LoadFactor = Convert.ToInt32(LoadFactorTextBox.Text);
				}

				NodesObject.IsConfiguredPrimaryNode = IsPrimaryNodeCheckBox.Checked;
				Credentials cred=new Credentials();
				cred.AliasName=CredentialsComboBox.Text;
				//DataTable credid = VSWebBL.ConfiguratorBL.CredentialsBL.Ins.getCredentialsByName(cred);						 ----------COMMENTED DUE TO NOT USING YET
				//NodesObject.CredentialsID = (CredentialsComboBox.Text == "" ? 0 : Convert.ToInt32(CredentialsComboBox.Value));
				NodesObject.CredentialsID = -1;//Convert.ToInt32(credid.Rows[0]["ID"].ToString());								 ----------COMMENTED DUE TO NOT USING YET
				NodesObject.LocationID = Convert.ToInt32(LocationsComboBox.Value == null ? "-1" : LocationsComboBox.Value.ToString());
				return NodesObject;
			}
			catch (Exception ex)
			{
				
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		private void InsertNodesData()
		{
		
		try
		{
						
				object ReturnValue = VSWebBL.SecurityBL.NodesBL.Ins.InsertData(CollectDataForNodesPropertie());	
		}
	     catch (Exception ex)
	    {
		    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
		    throw ex;
		}
		}

	
		private void UpdateNodeSettings()
		{
			try
			{

				object ReturnValue = VSWebBL.SecurityBL.NodesBL.Ins.UpdateDataforservers(CollectDataForNodesPropertie());
			}


			catch (Exception ex)
			{
				errorDiv.InnerHtml = "The following error has occurred: " + ex.Message;
				errorDiv.Style.Value = "display: block";

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}


		protected void FormOkButton_Click(object sender, EventArgs e)
		{
			bool proceed = true;
			string errtext = "";
			if (proceed)
			{
				try
				{
					if (Mode == "Update")
					{
						UpdateNodeSettings();

					}

					if (Mode == "Insert")
					{
						InsertNodesData();
					}

					VSWebBL.SecurityBL.NodesBL.Ins.forceCollectionRefresh();

					Response.Redirect("~/Security/AssignServerToNode.aspx");
				}
				catch (Exception ex)
				{

					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}
				finally { }
			}
			else
			{
				errorDiv.Style.Value = "display: block;";

				errorDiv.InnerHtml = errtext +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			}
		}	


		protected void FormCancelButton_Click(object sender, EventArgs e)
		{
			if (Request.QueryString["serverid"] != null)
			{
				int id = Convert.ToInt32(Request.QueryString["serverid"]);
				Response.Redirect("~/Configurator/SametimeServer.aspx?ID=" + Convert.ToInt32(Request.QueryString["serverid"]) + " ");

			}
			else
			{
				string stype = Request.QueryString["Cat"];
				Response.Redirect("~/Security/AssignServerToNode.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
				Context.ApplicationInstance.CompleteRequest();
			}
		}


    	protected void UpdateTabVisibility()
		{
			/*if (RoleHub.Checked)
			{
				ASPxPageControlWindow.TabPages[3].ClientVisible = true;
			}
			else
			{
				ASPxPageControlWindow.TabPages[3].ClientVisible = false;
			}

			if (RoleEdge.Checked)
			{
				ASPxPageControlWindow.TabPages[4].ClientVisible = true;
			}
			else
			{
				ASPxPageControlWindow.TabPages[4].ClientVisible = false;
			}

			if (RoleCAS.Checked)
			{
				ASPxPageControlWindow.TabPages[5].ClientVisible = true;
			}
			else
			{
				ASPxPageControlWindow.TabPages[5].ClientVisible = false;
			}
			 * */
		}

		protected void UpdatePanel_Unload(object sender, EventArgs e)
		{
			System.Reflection.MethodInfo methodInfo = typeof(ScriptManager).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				.Where(i => i.Name.Equals("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel")).First();
			methodInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { sender as UpdatePanel });
		}

		protected void fillLocationsCombobox()
		{
			DataTable LocationDataTable = VSWebBL.SecurityBL.LocationsBL.Ins.GetAllData();
			LocationsComboBox.DataSource = LocationDataTable;
			LocationsComboBox.TextField = "Location";
			LocationsComboBox.ValueField = "ID";
			LocationsComboBox.DataBind();
		}

		protected void FormDisableButton_Click(object sender, EventArgs e)
		{

			ASPxButton btn = sender as ASPxButton;

			try
			{

				object ReturnValue = VSWebBL.SecurityBL.NodesBL.Ins.SetDisableState(btn.Text == "Disable", Request.QueryString["ID"].ToString());

				VSWebBL.SecurityBL.NodesBL.Ins.forceCollectionRefresh();

				Response.Redirect("~/Security/AssignServerToNode.aspx");
			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
			
		}	
	
	}
}