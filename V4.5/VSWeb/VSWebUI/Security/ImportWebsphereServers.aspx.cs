using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebDO;
using DevExpress.Web;
using System.Data.OleDb;
using System.IO;
using VSWebBL;
using DevExpress.Web.ASPxTreeList;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
namespace VSWebUI.Security
{
	public partial class ImportWebsphereServers : System.Web.UI.Page
	{
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		string fileName1;
		private static readonly Regex validHostnameRegex = new Regex(@"^(([a-z]|[a-z][a-z0-9\-]*[a-z0-9])\.)*([a-z]|[a-z][a-z0-9\-]*[a-z0-9])$", RegexOptions.IgnoreCase);
		int AlertKey;
		bool cellnameinsert = true;
		IPHostEntry hostEntry;
		IPAddress[] ipaddress;
		string UIip, dbipaddress, port, convertdbip,prevHostIp;
		bool validhostname = true;
		//bool ccredentilasvisiblity;

	
		VSFramework.TripleDES mytestenkey = new VSFramework.TripleDES();
		protected void Page_Load(object sender, EventArgs e)
		{
		

			errormsg.Style.Value = "display: none";
			
			FillCredentialsComboBox();
			Session["edit"] = "";
			Session["cellid"] = null;
			//Session["chbx"] = false;
			
			if (Request.QueryString["CellID"] != "" && Request.QueryString["CellID"] != null)
			{
				Session["cellid"] = Convert.ToInt32(Request.QueryString["CellID"]);
				Session["edit"] = "true";
				
			}
			if (Request.QueryString["webcellid"] != "" && Request.QueryString["webcellid"] != null)
			{
			Session["cellid"]=	Convert.ToInt32(Request.QueryString["webcellid"]);
			}
			if (!IsPostBack)
			{
				Session["DataEvents"] = null;
				//fillNodesTreeList();
				if (Request.QueryString["CellID"] != "" && Request.QueryString["CellID"] != null)
				{
					int cellid = Convert.ToInt32(Request.QueryString["CellID"]);
					Session["cellid"] = cellid;

					fillCelldetails();
					//if (Session["cellname"] != null)
					//{
					//    CellnameTextBox.Text = Session["cellname"].ToString();
					//    HostName.Text = Session["HostName"].ToString();
					//    ConnectionComboBox.Text = Session["ConnectionComboBox"].ToString();
					//    porttextbox.Text = Session["txtport"].ToString();
					//    chbx.Checked = Convert.ToBoolean(Session["chbx"].ToString());
					//    Session["edit"] = "true";
					//}
				}
				if (Request.QueryString["add"] == "2")
				{

					CellnameTextBox.Text = Session["cellname"].ToString();
					HostName.Text = Session["HostName"].ToString();
					ConnectionComboBox.Text = Session["ConnectionComboBox"].ToString();
					porttextbox.Text = Session["txtport"].ToString();
					chbx.Checked = Convert.ToBoolean(Session["chbx"].ToString());
					Session["edit"] = "true";
					if(chbx.Checked==true)
					{
					CredentialsLabel.Visible = true;
					CredentialsComboBox.Visible = true;
					ASPxButton1.Visible = true;
					reallbl.Visible = true;
					realmtxtbx.Visible = true;
					realmtxtbx.Text = Session["realmtxtbx"].ToString();
					CredentialsComboBox.Text = Session["CredentialsComboBox"].ToString();
					}
				}



			}
			else
			{

				fillCelldetailsfromSession();
				fillNodesTreefromSession();
			}
			if (Request.QueryString["cred"] != null)
			{
				CredentialsLabel.Visible = true;
				CredentialsComboBox.Visible = true;
				reallbl.Visible = true;
				realmtxtbx.Visible = true;
				ASPxButton1.Visible = true;
				
			}
		}
		public void fillCelldetailsfromSession()
		{

			DataTable CellDetails = new DataTable();
			try
			{
				if (Session["cell"] != "" && Session["cell"] != null)
					CellDetails = Session["cell"] as DataTable;
				NodesTreeList.DataSource = CellDetails;
				NodesTreeList.DataBind();

			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		public void fillNodesTreefromSession()
		{

			DataTable DataEvents = new DataTable();
			try
			{
				if (Session["DataEvents"] != "" && Session["DataEvents"] != null)
					DataEvents = Session["DataEvents"] as DataTable;
				NodesTreeList.DataSource = DataEvents;
				NodesTreeList.DataBind();

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
		//protected void fileupld_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
		//{

		//    try
		//    {
		//        //string fileExtension = Path.GetExtension(uploadedFile.FileName);
		//        e.CallbackData = SavePostedFile(e.UploadedFile);



		//    }
		//    catch (Exception ex)
		//    {
		//        e.IsValid = false;
		//        e.ErrorText = ex.Message;
		//    }
		//    e.CallbackData = SavePostedFile(e.UploadedFile);


		//}



		//string SavePostedFile(UploadedFile uploadedFile)
		//{
		//    string logPath = "";

		//    //fileupld.SaveAs(Server.MapPath("~/LogFiles/" + uploadedFile.FileName));
		//    // fileupld.SaveAs(logPath + uploadedFile.FileName);
		//    //string fileName1 = Path.Combine(MapPath("~/log_files/") + uploadedFile.FileName);
		//    fileName1 = "../LogFiles/" + uploadedFile.FileName;// Path.Combine(logPath + uploadedFile.FileName);

		//    string fileExtension = Path.GetExtension(uploadedFile.FileName);
		//    string path = Path.GetFullPath(uploadedFile.FileName);
		//    Session["path"] = path;

		//    return fileName1;
		//}
		public void Insertdata()
		{
			bool ReturnValue;
			WebsphereCell webspehereObject = new WebsphereCell();
			if (Session["cellid"]!= null)
			{
				//webspehereObject.CellID = int.Parse(Request.QueryString["CellID"].ToString());
				webspehereObject.CellID = int.Parse(Session["cellid"].ToString());
				Session["CellID"] = Session["cellid"];
				//webspehereObject.CellID = Convert.ToInt32(Session["CellID"].ToString());
			}

			webspehereObject.Name = CellnameTextBox.Text;
			webspehereObject.ConnectionType = ConnectionComboBox.Text;

			webspehereObject.GlobalSecurity = chbx.Checked;
			webspehereObject.HostName = HostName.Text;
			webspehereObject.PortNo = Convert.ToInt32(porttextbox.Text);
			//webspehereObject.Certification = fileName1;
			if (chbx.Checked == true)
			{
				DataTable dt = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetCredentialValue(CredentialsComboBox.Text);
				webspehereObject.CredentialsID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
				//if (fileName1 == null)
				//{
				//    webspehereObject.Certification = "";
				//}


				webspehereObject.Realm = realmtxtbx.Text;
			}
			
			ReturnValue = VSWebBL.SecurityBL.webspehereImportBL.Ins.InsertData(webspehereObject);
			if (ReturnValue == true)
			{
				//successDiv.Style.Value = "display: block";
				Response.Redirect("WebsphereCellGrid.aspx", false);
				Context.ApplicationInstance.CompleteRequest();
			}
		}

		protected void CollapseAllButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (CollapseAllButton.Text == "Collapse All")
				{
					NodesTreeList.CollapseAll();
					CollapseAllButton.Image.Url = "~/images/icons/add.png";
					CollapseAllButton.Text = "Expand All";
				}
				else
				{
					NodesTreeList.ExpandAll();
					CollapseAllButton.Image.Url = "~/images/icons/forbidden.png";
					CollapseAllButton.Text = "Collapse All";
				}
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

		public void fillNodesTreeList()
		{
			try
			{
				int cellid = Convert.ToInt32(Request.QueryString["CellID"]);
				//string id = Convert.ToString(cellid);
				NodesTreeList.CollapseAll();
				CollapseAllButton.Image.Url = "~/images/icons/add.png";
				CollapseAllButton.Text = "Expand All";
				if (Session["DataEvents"] == null)
				{
					DataTable DataEventsTree = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetnodesserversFromProcedure();
					//DataTable bycelldata = VSWebBL.SecurityBL.webspehereImportBL.Ins.FetserversbycellID(cellid);
					//DataTable DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
					Session["DataEvents"] = DataEventsTree;
				}
				NodesTreeList.DataSource = (DataTable)Session["DataEvents"];
				NodesTreeList.DataBind();

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
		public void fillCelldetails()
		{
			DataTable cell = VSWebBL.SecurityBL.webspehereImportBL.Ins.Getcelldata(Convert.ToInt32(Session["cellid"].ToString()));
			CellnameTextBox.Text = cell.Rows[0]["Name"].ToString();
			Session["cellinfo"] = cell.Rows[0]["Name"].ToString();
			HostName.Text = cell.Rows[0]["HostName"].ToString();
			Session["HostName"] = cell.Rows[0]["HostName"].ToString();
			porttextbox.Text = cell.Rows[0]["PortNo"].ToString();
			Session["PortNo"] = cell.Rows[0]["PortNo"].ToString();
			ConnectionComboBox.Text = cell.Rows[0]["ConnectionType"].ToString();

			//string path = cell.Rows[0]["Certification"].ToString();
			
			//lblfilename.Text = cell.Rows[0]["Certification"].ToString().Substring(cell.Rows[0]["Certification"].ToString().LastIndexOf("\\") + 1);
			//if( cell.Rows[0]["Certification"].ToString()!="" &&  cell.Rows[0]["Certification"].ToString()!=null)
			//{
			//    lnkchange.Visible = true;
			//}
			//anchorFilename.HRef = cell.Rows[0]["Certification"].ToString();
			chbx.Checked = Convert.ToBoolean(cell.Rows[0]["GlobalSecurity"].ToString());
			if (chbx.Checked)
			{
				//lnkResumeRemove.Visible = true;
				ASPxButton1.Visible = true;
				//fileupld.Visible = true;
				CredentialsComboBox.Visible = true;
				CredentialsLabel.Visible = true;
				
				//certifaicatelbl.Visible = true;
				//if (lblfilename.Text != null && lblfilename.Text != "")
				//{
				//    lnkchange.Visible = true;
				//}
				//lnkchange.Visible = true;
				//tdload.Visible = true;
				int crid = Convert.ToInt32(cell.Rows[0]["CredentialsID"].ToString());
				DataTable cell1 = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetCrentials(crid);
				CredentialsComboBox.Text = cell1.Rows[0]["AliasName"].ToString();
				if (ConnectionComboBox.Text == "SOAP")
				{
					reallbl.Visible = false;
					realmtxtbx.Visible = false;
				}
				else
				{
					reallbl.Visible = true;
					realmtxtbx.Visible = true;
				}
				realmtxtbx.Text = cell.Rows[0]["Realm"].ToString();

				//certificateTextBox2.Visible = true;
			}

		}
		public void fillNodesTreeListbycellID()
		{
			try
			{
				int cellid = Convert.ToInt32(Request.QueryString["CellID"]);
				//string id = Convert.ToString(cellid);
				NodesTreeList.CollapseAll();
				CollapseAllButton.Image.Url = "~/images/icons/add.png";
				CollapseAllButton.Text = "Expand All";
				if (Session["DataEvents"] == null)
				{
					//DataTable DataEventsTree = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetnodesserversFromProcedure();
					DataTable bycelldata = VSWebBL.SecurityBL.webspehereImportBL.Ins.FetserversbycellID(cellid);
					//DataTable DataServersTree = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServersFromProcedure();
					Session["DataEvents"] = bycelldata;
				}
				NodesTreeList.DataSource = (DataTable)Session["DataEvents"];
				NodesTreeList.DataBind();

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

		//protected void LoadServersButton_Click(object sender, EventArgs e)
		//{
		////	Insertdata();
		//    if (Request.QueryString["CellID"] != "" && Request.QueryString["CellName"] != null)
		//    {
		//        fillNodesTreeListbycellID();
		//        ASPxRoundPanel1.Visible = true;
		//    }
		//    else
		//    {
		//        fillNodesTreeList();
		//    }
		//}

		protected void chbx_CheckedChanged(object sender, EventArgs e)
		{
			
			if (chbx.Checked)
			{
				//hypResume.Visible = true;
				ASPxButton1.Visible = true;
				CredentialsLabel.Visible = true;
				CredentialsComboBox.Visible = true;
				//if (ConnectionComboBox.SelectedItem.Text != "SOAP")
				//{
					reallbl.Visible = true;
					realmtxtbx.Visible = true;
				//}
				//certifaicatelbl.Visible = true;
				//fileupld.Visible = true;
				//tdload.Visible = true;

			}
			else
			{
				ASPxButton1.Visible = false;
				//hypResume.Visible = false;
				CredentialsLabel.Visible = false;
				CredentialsComboBox.Visible = false;
				reallbl.Visible = false;
				realmtxtbx.Visible = false;
				//certifaicatelbl.Visible = false;
				//fileupld.Visible = false;
				//tdload.Visible = false;

			}
			Session["chbx"] = chbx.Checked;
		}

		//protected void OKButton_Click(object sender, EventArgs e)
		//{
		//    Insertdata();
		//}



		protected void ConnectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ConnectionComboBox.SelectedItem.Text == "SOAP")
			{
				porttextbox.Text = "8879";
				//realmtxtbx.Visible = false;
				//reallbl.Visible = false;
				realmtxtbx.Text = ""; 
			}
			else
			{
				porttextbox.Text = "9809";
				//if (chbx.Checked)
				//{
					//realmtxtbx.Visible = true;
				//	reallbl.Visible = true;
				//}
			}
		}
		protected void savebtn_Click(object sender, EventArgs e)
		{
			if (validhostname == true)
			{
				ipaddress = Dns.GetHostAddresses(HostName.Text);
				foreach (IPAddress var in ipaddress)
				{
					if (var.AddressFamily == AddressFamily.InterNetwork)
					{
						UIip = var.ToString();

						break;
					}
				}
                if (Session["HostName"] != null)
                {
                    ipaddress = Dns.GetHostAddresses(Session["HostName"].ToString());
                    foreach (IPAddress var in ipaddress)
                    {
                        if (var.AddressFamily == AddressFamily.InterNetwork)
                        {
                            prevHostIp = var.ToString();

                            break;
                        }
                    }
                }
				DataTable dt = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetAllHostNmaes();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dbipaddress = dt.Rows[i]["HostName"].ToString();
					port = dt.Rows[i]["PortNo"].ToString();
					ipaddress = Dns.GetHostAddresses(dbipaddress);

					foreach (IPAddress var in ipaddress)
					{
						if (var.AddressFamily == AddressFamily.InterNetwork)
						{
							convertdbip = var.ToString();
							break;
						}
					}

					if (convertdbip == UIip && port == porttextbox.Text)
					{
						if (Session["cellid"] == null)
						{
							cellnameinsert = false;
						}
                        else if (prevHostIp == UIip && Session["PortNo"].ToString() == porttextbox.Text)
						{
							cellnameinsert = true;
						}
						else
						{
							cellnameinsert = false;
						}
					}
				}

				if (cellnameinsert == true)
				{

					Insertdata();


				}

				else
				{
					errormsg.InnerHtml = "Cell Name already exists with this WebSphere configuration.Please enter another WebSphere configuration." +
							 "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errormsg.Style.Value = "display: block";
				}
			}
			else
			{
				errormsg.InnerHtml = "Please enter valid Host Name." +
								   "<button type=\"button\" onclick='javascript:var SearchInput = $(\"#ContentPlaceHolder1_ASPxRoundPanel2_ASPxCallbackPanel2_HostName_I\");var strLength= SearchInput.val().length * 2;SearchInput.focus();SearchInput[0].setSelectionRange(strLength, strLength);' class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";


				errormsg.Style.Value = "display: block";
			}
		}

		protected void cancelbtn_Click(object sender, EventArgs e)
		{
		    Response.Redirect("WebsphereCellGrid.aspx", false);
		    Context.ApplicationInstance.CompleteRequest();
		}

		private void FillCredentialsComboBox()
		{
			DataTable CredentialsDataTable = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetWebsphereserverCredentials(22);
			CredentialsComboBox.DataSource = CredentialsDataTable;
			CredentialsComboBox.TextField = "AliasName";
			CredentialsComboBox.ValueField = "ID";
			CredentialsComboBox.DataBind();


		}
		protected void OKCopy_Click(object sender, EventArgs e)
		{
			bool check = false;
			Credentials Csibject = new Credentials();

			Csibject.AliasName = AliasName.Text;
			string rawPass = Password.Text;
			byte[] encryptedPass = mytestenkey.Encrypt(rawPass);
			string encryptedPassAsString = string.Join(", ", encryptedPass.Select(s => s.ToString()).ToArray());

			//DataTable returntable = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.GetName(Csibject);
			DataTable returntable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetAliasName(Csibject);
			if (Session["edit"].ToString() == "true")
			{
				if (returntable.Rows.Count > 0)
				{

					Div3.InnerHtml = "This Alias already exists. Enter another one." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					Div3.Style.Value = "display: block";

				}
				else
				{
					check = VSWebBL.SecurityBL.webspehereImportBL.Ins.Insertpwd(AliasName.Text, UserID.Text, encryptedPassAsString, 22);
					Response.Redirect("~/Security/ImportWebsphereServers.aspx?cred=2&CellID=" + Session["CellID"]);
				}
			}
			else
			{
				if (returntable.Rows.Count > 0)
				{

					Div3.InnerHtml = "This Alias already exists. Enter another one." +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					Div3.Style.Value = "display: block";

				}
				else
				{
					check = VSWebBL.SecurityBL.webspehereImportBL.Ins.Insertpwd(AliasName.Text, UserID.Text, encryptedPassAsString, 22);
					Response.Redirect("~/Security/ImportWebsphereServers.aspx?add=2&webcellid="+ Session["CellID"]);
				}
			}
		}
		protected void Cancel_Click(object sender, EventArgs e)
		{
			
			if (Session["edit"].ToString() == "true")
			{

				Response.Redirect("~/Security/ImportWebsphereServers.aspx?cred=2&CellID=" + Session["CellID"]);
				//ccredentilasvisiblity = true;
			}
			else
			{
				Response.Redirect("~/Security/ImportWebsphereServers.aspx?add=2");
			}
		}
		protected void btn_clickcopyprofile(object sender, EventArgs e)
		{

			CopyProfilePopupControl.ShowOnPageLoad = true;
			UserID.Visible = true;
			OKCopy.Visible = true;
			Cancel.Visible = true;
			Password.Visible = true;
			Session["cellname"] = CellnameTextBox.Text;
			Session["HostName"] = HostName.Text;

			Session["ConnectionComboBox"] = ConnectionComboBox.Text;
			Session["txtport"] = porttextbox.Text;
			//Session["chbx"] = chbx.Checked;
			Session["realmtxtbx"] = realmtxtbx.Text;
			Session["CredentialsComboBox"] = CredentialsComboBox.Text;



		}//popup
		//protected void lnkResumeRemove_Click(object sender, EventArgs e)
		//{

		//    lnkResumeRemove.Visible = false;
		//    hypResume.Visible = false;

		//    hypResume.Visible = false;
		//    lnkResumeRemove.Visible = false;

		//    string path = hypResume.NavigateUrl;
		//    System.IO.File.Delete(Server.MapPath(Session["path"].ToString()));

		//}



		//protected void lnkChange_Click(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        fileupld.Visible = true;
		//        lblfilename.Visible = false;
		//        //  mainAnchor.Visible = false;


		//    }
		//    catch
		//    {
		//    }
		//}	




		//public static string HostName2IP(string	 HostName)
		//{
		//    IPHostEntry iphost = System.Net.Dns.Resolve(HostName);

		//    IPAddress[] addresses = iphost.AddressList;

		//    StringBuilder addressList = new StringBuilder();
		//    // get each ip address
		//    foreach (IPAddress address in addresses)
		//    {
		//        // append it to the list
		//        addressList.Append("IP Address: ");
		//        addressList.Append(address.ToString());
		//        addressList.Append(";");
		//    }
		//    return addressList.ToString();
		//}			

		protected void CellnameTextBox_Validation(object sender, ValidationEventArgs e)
		{
			DataTable returntable = VSWebBL.SecurityBL.webspehereImportBL.Ins.GetNmae(CellnameTextBox.Text);

			if (returntable.Rows.Count > 0)
			{
				if (Request.QueryString["CellID"] != null)
				{
					if (Session["cellinfo"].ToString() != CellnameTextBox.Text)
					{

						errormsg.InnerHtml = "Name already exists.Please enter another Name." +
									 //  "<button type=\"button\" onclick='javascript:document.getElementById(\"ContentPlaceHolder1_ASPxRoundPanel2_ASPxCallbackPanel1_CellnameTextBox_I\").focus();' class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
									  "<button type=\"button\" onclick='javascript:var SearchInput = $(\"#ContentPlaceHolder1_ASPxRoundPanel2_ASPxCallbackPanel1_CellnameTextBox_I\");var strLength= SearchInput.val().length * 2;SearchInput.focus();SearchInput[0].setSelectionRange(strLength, strLength);' class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";

						errormsg.Style.Value = "display: block";
						Session["Namevalidation"] = "true";

					}

				}
				else
				{

					errormsg.InnerHtml = "Name already exists.Please enter another Name." +
									  // "<button type=\"button\" onclick='javascript:document.getElementById(\"ContentPlaceHolder1_ASPxRoundPanel2_ASPxCallbackPanel1_CellnameTextBox_I\").focus();' class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
									 "<button type=\"button\" onclick='javascript:var SearchInput = $(\"#ContentPlaceHolder1_ASPxRoundPanel2_ASPxCallbackPanel1_CellnameTextBox_I\");var strLength= SearchInput.val().length * 2;SearchInput.focus();SearchInput[0].setSelectionRange(strLength, strLength);' class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errormsg.Style.Value = "display: block";
					Session["Namevalidation"] = "true";

				}
			}
			else
			{
				errormsg.Style.Value = "display: none";


			}

		}

		protected void HostName_Validation(object sender, ValidationEventArgs e)
		{
			try
			{

				//hostEntry = Dns.GetHostEntry(HostName.Text);
				 ipaddress = Dns.GetHostAddresses(HostName.Text);
				if (ipaddress!= null)
				{
					errormsg.Style.Value = "display: none";
				}
			}
			catch (Exception ex)
			{
				validhostname = false;
			
				errormsg.InnerHtml = "Please enter valid Host Name." +
								   "<button type=\"button\" onclick='javascript:var SearchInput = $(\"#ContentPlaceHolder1_ASPxRoundPanel2_ASPxCallbackPanel2_HostName_I\");var strLength= SearchInput.val().length * 2;SearchInput.focus();SearchInput[0].setSelectionRange(strLength, strLength);' class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";

				
				errormsg.Style.Value = "display: block";

			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			ASPxWebControl[] objs = {reallbl, realmtxtbx} ;
			foreach (ASPxWebControl obj in objs)
			{
				if (ConnectionComboBox.SelectedItem != null && ConnectionComboBox.SelectedItem.Text == "SOAP")
				{
					obj.Visible = false;
				}
				else
				{
					if (chbx.Checked)
						obj.Visible = true;
				}
			}
		}

	}
}