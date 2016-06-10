using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using VSFramework;
using VSWebBL;
using VSWebDO;


using DevExpress.Web;
using DevExpress.Data;
using DevExpress.Xpo;



namespace VSWebUI.Configurator
{
	public partial class NetworkLatencyTestServers : System.Web.UI.Page
	{
		int ServerKey;
		protected DataTable nwDataTable = null;
		int id;
		int yellowthershold;
		int latency;
		bool checkedvalue;
		bool isValid = true;
		int nwlid;
		string flag;
		bool Errormsg = false;
		protected void Page_Load(object sender, EventArgs e)
		{

			try
			{

				if (!IsPostBack)
				{
					Session["NetworkLatency"] = null;
					//NetworkLatencyTestgrd.ClearSort();
					//(NetworkLatencyTestgrd.Columns["ID"] as GridViewDataColumn).SortAscending();

					if (Session["UserPreferences"] != null)
					{
						DataTable UserPreferences = (DataTable)Session["UserPreferences"];
						foreach (DataRow dr in UserPreferences.Rows)
						{
							if (dr[1].ToString() == "NetworkLatencyTestServers|NetworkLatencyTestgrd")
							{
								NetworkLatencyTestgrd.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
							}
                            
                                if (dr[1].ToString() == "NetworkLatencyTestServers|ThresholdGridView")
							{
								ThresholdGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
							}
						}
					}

				}

				if (Request.QueryString["Key"] != "" && Request.QueryString["Key"] != null)
				{
					flag = "Update";
					ServerKey = int.Parse(Request.QueryString["Key"]);

					if (!IsPostBack)
					{
						FillData(ServerKey);
					}
					ASPxMenu1.Visible = true;
				}
				else
				{
					flag = "Insert";
					ASPxMenu1.Visible = false;
				}

				//ServerKey = int.Parse(Request.QueryString["Key"]);

				//10/13/2014 NS added

				//For Validation Summary
				////ApplyValidationSummarySettings();
				////ApplyEditorsSettings();

				if (!IsPostBack)
				{

					//Fill Server Attributes Tab & Advanced Tab
					//8/7/2014 NS added


					// FillServerTypeComboBox();
					FillNwlatencyServerGrid();
					//FillServerTypeComboBox();
					//11/18/2014 NS modified
					//DominoRoundPanel.HeaderText = "Domino Properties -" + " " + SrvAtrSrvNameComboBox.Text;
					servernamelbldisp.InnerHtml = "Network Latency Test Group -" + " " + (testtxtname.Text == "" ? "New" : testtxtname.Text);
				}
				else
				{
					if (Session["NetworkLatency"] != null)
					{
						bool IsButtonClicked;
						if (IsButtonClicked = true)
						{
							IsButtonClicked = false;
							foreach (string cntrl in Request.Params.AllKeys)
							{
								Control btn = Page.FindControl(cntrl);
								if (btn != null)
									if (btn.ID.ToString() == "btnupdate")
									{
										IsButtonClicked = true;
									}
							}


							if (!IsButtonClicked)
							{
								FillNwLtncyServerGridfromSession();
							}
							//}

						}
						//FillNwLtncyServerGridfromSession();
					}

                    FillThresholdGridfromSession();
				}
				if (Session["NetworkLatency"] == null && Session["NetworkLatency"] == "")
				{
					FillNwLtncyServerGridfromSession();
				}
			}

			catch (Exception ex)
			{
				//11/18/2014 NS modified

				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
			finally { }
		}
		private NetworkLatency CollectDataForProfiles(DataRow ProfilesRow)
		{
			try
			{
				NetworkLatency nl = new NetworkLatency();
				//  ProfileNames Profilesnamesobj = new ProfileNames();
				//ProfilesObject.ServerID=Convert.ToInt32(ProfilesRow["ServerID"].ToString());
				//ProfilesObject.NetworkLatencyId = Convert.ToInt32(ProfilesRow["NetworkLatencyId"].ToString());
				//ProfilesObject.ServerType = ProfilesRow["ServerType"].ToString();
				// ProfilesObject.ServerID = ProfilesRow["ServerID"].ToString();
				nl.NetworkLatencyId = ServerKey;
				if (nl.LatencyYellowThreshold != null)
				{
					
					nl.LatencyYellowThreshold = ProfilesRow["LatencyYellowThreshold"].ToString() == "" ? 0 : Convert.ToInt32(ProfilesRow["LatencyYellowThreshold"].ToString());
				}
				if (nl.LatencyYellowThreshold != null)
				{
					nl.LatencyRedThreshold = ProfilesRow["LatencyRedThreshold"].ToString()==""? 0 : Convert.ToInt32(ProfilesRow["LatencyRedThreshold"].ToString());
				}
				nl.Enabled = Convert.ToBoolean(ProfilesRow["Enabled"].ToString());
				//ProfilesObject.ProfileId = Convert.ToInt32(ProfileComboBox.SelectedItem.Value);
				//Profilesnamesobj.ProfileName = ProfileComboBox.SelectedItem.Text;
				return nl;
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private NetworkLatency CollectDataFornl()
		{
			try
			{
				NetworkLatency nlobj = new NetworkLatency();


				nlobj.NetworkLatencyId = ServerKey;


				nlobj.TestName = testtxtname.Text;


				nlobj.ScanInterval = Convert.ToInt32(txtscaninterval.Text);

				nlobj.TestDuration = Convert.ToInt32(txtduration.Text);

				nlobj.LatencyYellowThreshold = txtyellothld.Text == "" ? 0 : Convert.ToInt32(txtyellothld.Text);
				nlobj.LatencyRedThreshold = txtredthreshold.Text == "" ? 0 : Convert.ToInt32(txtredthreshold.Text);
				// nlobj.ServerType = servertypecombo.Text;
				nlobj.Enable = SrvAtrScanCheckBox.Checked;
				//DominoServersObject.Modified_By = int.Parse(Session["UserID"].ToString());
				//DominoServersObject.Modified_On = DateTime.Now.ToString();
				return nlobj;
			}
			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		protected void FormOkButton_Click(object sender, EventArgs e)
		{
			try
			{

				//5/1/2014 NS modified for VSPLUS-427
				bool proceed = true;
				bool Update = false;
				
				//5/12/2014 NS added for VSPLUS-615
				string errtext = "";
				int gbc = 0;
				if (proceed)
				{
					try
					{
						for (int i = 0; i < NetworkLatencyTestgrd.VisibleRowCount; i++)
						{
							GridViewDataColumn Ischeck = NetworkLatencyTestgrd.Columns[0] as GridViewDataColumn;
							CheckBox chkSelect = NetworkLatencyTestgrd.FindRowCellTemplateControl(i, Ischeck, "chkRow") as CheckBox;
							if (chkSelect != null)
							{
								if (chkSelect.Checked)
								{
									UpdatenlData();
									if (Errormsg == false)
									SaveNwLtncyServerGridnew();
								}
							}
						}
						DataTable ExchangeSettingsDataTable = (DataTable)Session["NetworkLatency"];
						List<object> LatencyYellowThreshold = NetworkLatencyTestgrd.GetSelectedFieldValues(new string[] { "LatencyYellowThreshold" });

						if (LatencyYellowThreshold.Count != 0)
						{
							UpdatenlData();
							if (Errormsg == false)
							{
								SaveNwLtncyServerGrid();

								// FillNwlatencyServerGrid();


								successDiv.Style.Value = "display: block";
								errorDiv.Style.Value = "display: none";
								errtext = "Selected fields were successfully saved.";
								successDiv.InnerHtml = errtext + "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							}
						}
						else
						{
							successDiv.Style.Value = "display: none";
                            errorDiv2.Style.Value = "display: none";
							errorDiv.Style.Value = "display: block";
							errtext = "Please select at least one server.";
                            errorDiv.InnerHtml = errtext + "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						}
						if (Errormsg == false)
						{
							if (LatencyYellowThreshold.Count != 0)
							{

								if (ServerKey == 0)
								{
									Response.Redirect("NetworkLatencyServers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
									Context.ApplicationInstance.CompleteRequest();
								}
								else
								{
									//  FillNwlatencyServerGrid();
									Response.Redirect("NetworkLatencyServers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
									Context.ApplicationInstance.CompleteRequest();
								}
							}

							else
							{
								successDiv.Style.Value = "display: none";
								errorDiv2.Style.Value = "display: none";
								errorDiv.Style.Value = "display: block";
								errtext = "Please select at least one server.";
								errorDiv.InnerHtml = errtext + "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
							}
						}//}

						//if (ServerKey == 0)
						//{
						//    Response.Redirect("NetworkLatencyServers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						//    Context.ApplicationInstance.CompleteRequest();
						//}
						//else
						//{
						//    //FillNwlatencyServerGrid();
						//    Response.Redirect("NetworkLatencyServers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
						//    Context.ApplicationInstance.CompleteRequest();
						//}
					}
					catch (Exception ex)
					{
						//6/27/2014 NS added for VSPLUS-634
						Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
						throw ex;
					}
					finally { }
				}
				else
				{
                    errorDiv2.Style.Value = "display: none";
					errorDiv.Style.Value = "display: block;";
					//10/6/2014 NS modified for VSPLUS-990
					errorDiv.InnerHtml = errtext +
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				}
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void FillData(int Key)
		{
			try
			{

				NetworkLatency nl = new NetworkLatency();
				NetworkLatency ReturnDSObject = new NetworkLatency();
				nl.NetworkLatencyId = Key;
				ReturnDSObject = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.GetData(nl);
				//5/5/2014 NS modified for VSPLUS-589
				//if (ReturnDSObject.NotificationGroup != null)

				testtxtname.Text = ReturnDSObject.TestName.ToString();
				txtscaninterval.Text = ReturnDSObject.ScanInterval.ToString();
				txtduration.Text = ReturnDSObject.TestDuration.ToString();

				// servertypecombo.Text = ReturnDSObject.ServerType.ToString();
				SrvAtrScanCheckBox.Checked = (ReturnDSObject.Enable.ToString() == "True" ? true : false);




			}

			catch (Exception ex)
			{
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void UpdatenlData()
		{
			try
			{
				Object ReturnValue;
				DataTable returntable = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.GetName(CollectDataFornl());
				if (returntable.Rows.Count > 0)
				{
					errorDiv.InnerHtml = "This Test name is already in use.  Please enter a different Test name." +
				   "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errorDiv.Style.Value = "display: block";
					Errormsg = true;
				}
				else
				{

					if (ServerKey > 0)
					{
						ReturnValue = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.UpdateData(CollectDataFornl());

					}
					else
					{
						ReturnValue = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.InsertData(CollectDataFornl());

					}



					SetFocusOnError(ReturnValue);
					if (ReturnValue.ToString() == "True")
					{

						Session["nwlatencyUpdateStatus"] = testtxtname.Text;
					}


				}
			}
			catch (Exception ex)
			{
				//10/3/2014 NS modified for VSPLUS-990
				errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
					"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				errorDiv.Style.Value = "display: block";
				//6/27/2014 NS added for VSPLUS-634
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
		}
		private void SetFocusOnError(Object ReturnValue)
		{
			string ErrorMessage = ReturnValue.ToString();
			if (ErrorMessage.Substring(0, 2) == "ER")
			{
				//Find control & set focus

				//string ControlName = ErrorMessage.Substring(3, ErrorMessage.IndexOf("@") - 3);
				//if (ControlName.EndsWith("ComboBox"))
				//{
				//    DropDownList ddl = (DropDownList)FindControl(ControlName);
				//    ddl.Focus();
				//}
				//if (ControlName.EndsWith("TextBox"))
				//{
				//    TextBox txt = (TextBox)FindControl(ControlName);
				//    txt.Focus(); 
				//}
				//if (ControlName.EndsWith("CheckBox"))
				//{
				//    CheckBox chk = (CheckBox)FindControl(ControlName);
				//    chk.Focus();;
				//}                

				// ErrorMessageLabel.Text = ErrorMessage.Substring(3);
				//ErrorMessagePopupControl.ShowOnPageLoad = true;

			}
		}
		//private void FillServerTypeComboBox()
		//{
		//    DataTable ServerDataTable = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetSpecificServerTypes();
		//    //DataTable ServerDataTable = VSWebBL.SecurityBL.AdminTabBL.Ins.GetNavigatorChildsByRefName("ServersDevices");
		//    servertypecombo.DataSource = ServerDataTable;
		//    servertypecombo.TextField = "ServerType";
		//    servertypecombo.ValueField = "ServerType";
		//    servertypecombo.DataBind();
		//}
		private DataTable FillNwlatencyServerGrid()
		{

			try
			{
                //10/21/2015 NS added for VSPLUS-2223
                DataTable selectedserversDT = new DataTable();
				DataTable networklatencyDataTable = new DataTable();
				NetworkLatency NetworkLatencyObject = new NetworkLatency();
				networklatencyDataTable = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.Getvalue1(ServerKey);
				if (networklatencyDataTable.Rows.Count > 0)
				{

					txtscaninterval.Text = networklatencyDataTable.Rows[0]["ScanInterval"].ToString();


					txtduration.Text = networklatencyDataTable.Rows[0]["TestDuration"].ToString();


					testtxtname.Text = networklatencyDataTable.Rows[0]["TestName"].ToString();
					SrvAtrScanCheckBox.Checked = (networklatencyDataTable.Rows[0]["Enable"].ToString() == "True" ? true : false);


				}

				if ((txtscaninterval.Text == null) || (txtscaninterval.Text == ""))
				{
					txtscaninterval.Text = "8";
				}
				if ((txtduration.Text == null) || (txtduration.Text == ""))
				{
					txtduration.Text = "5";
				}
				string Page = "NetworkLatencyTestServers.aspx";
				string Control = "NetworkLatencyTestgrd";
				nwDataTable = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.GetAllData2(ServerKey, Page, Control);
                //7/16/2015 NS commented out for VSPLUS-1511
				//DataColumn[] columns = new DataColumn[1];
				//columns[0] = nwDataTable.Columns["ID"];
				//nwDataTable.PrimaryKey = columns;

				//nwDataTable.PrimaryKey = new DataColumn[] { nwDataTable.Columns["ID"] };
				nwDataTable.Columns.Add("Isselected", typeof(System.Boolean));
                //10/21/2015 NS added for VSPLUS-2223
                NetworkLatencyTestgrd.DataSource = nwDataTable;
                selectedserversDT = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.GetSelectedServers(ServerKey, Page, Control);
                if (selectedserversDT.Rows.Count > 0)
                {
                    DataRow[] foundRows;
                    for (int i = 0; i < selectedserversDT.Rows.Count; i++)
                    {
                        foundRows = nwDataTable.Select("ID=" + selectedserversDT.Rows[i]["ID"].ToString() + " AND ServerTypeId=" + selectedserversDT.Rows[i]["ServerTypeId"].ToString());
                        if (foundRows.Length > 0)
                        {
                            foundRows[0]["Enabled"] = true;
                            foundRows[0]["LatencyYellowThreshold"] = Convert.ToInt32(selectedserversDT.Rows[i]["LatencyYellowThreshold"].ToString());
                            foundRows[0]["LatencyRedThreshold"] = Convert.ToInt32(selectedserversDT.Rows[i]["LatencyRedThreshold"].ToString());
                        }
                    }
                    nwDataTable.AcceptChanges();
                }
				Session["NetworkLatency"] = nwDataTable;
                NetworkLatencyTestgrd.DataSource = nwDataTable;
				NetworkLatencyTestgrd.DataBind();
				return nwDataTable;

			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}

			finally { }
		}
		private void FillNwLtncyServerGridfromSession()
		{
			try
			{
				DataTable nwlatencyDataTable = new DataTable();
				if (Session["NetworkLatency"] != null && Session["NetworkLatency"] != "")
					nwlatencyDataTable = (DataTable)Session["NetworkLatency"];
				if (nwlatencyDataTable.Rows.Count > 0)
				{
					GridViewDataColumn column3 = NetworkLatencyTestgrd.Columns["LatencyYellowThreshold"] as GridViewDataColumn;
					GridViewDataColumn column4 = NetworkLatencyTestgrd.Columns["LatencyRedThreshold"] as GridViewDataColumn;
					int startIndex = NetworkLatencyTestgrd.PageIndex * NetworkLatencyTestgrd.SettingsPager.PageSize;
					int endIndex = Math.Min(NetworkLatencyTestgrd.VisibleRowCount, startIndex + NetworkLatencyTestgrd.SettingsPager.PageSize);

					for (int i = startIndex; i < endIndex; i++)
					{
						ASPxTextBox txtValue = (ASPxTextBox)NetworkLatencyTestgrd.FindRowCellTemplateControl(i, column3, "txtyellowthreshValue");
						ASPxTextBox txtValue2 = (ASPxTextBox)NetworkLatencyTestgrd.FindRowCellTemplateControl(i, column4, "txtredthreshValue");

						if (txtValue != null)
						{
							nwlatencyDataTable.Rows[i]["LatencyYellowThreshold"] = txtValue.Text;
						}

						if (txtValue2 != null)
						{
							nwlatencyDataTable.Rows[i]["LatencyRedThreshold"] = txtValue2.Text;

						}
                        //7/16/2015 NS added for VSPLUS-1511
                        nwlatencyDataTable.Rows[i]["ServerName"] = NetworkLatencyTestgrd.GetRowValues(i, "ServerName");
                        nwlatencyDataTable.Rows[i]["Location"] = NetworkLatencyTestgrd.GetRowValues(i, "Location");
                        nwlatencyDataTable.Rows[i]["ID"] = NetworkLatencyTestgrd.GetRowValues(i, "ID");
                        nwlatencyDataTable.Rows[i]["ServerType"] = NetworkLatencyTestgrd.GetRowValues(i, "ServerType");
						//if (txtValue != null)
						//{
						//    nwlatencyDataTable.Rows[i]["LatencyYellowThreshold"] = (txtValue.Text == "" ? "0" : txtValue.Text);
						//    if ((txtValue.Text != null) && (txtValue.Text != ""))
						//    {
						//        nwlatencyDataTable.Rows[i]["LatencyYellowThreshold"] = txtValue.Text;
						//    }
						//    else
						//    {

						//        txtValue.Text = "30";
						//        //int Result = 0;
						//        //int.TryParse(txtValue.Text, out Result);
						//        //txtValue.Text = Result.ToString();
						//    }

						//}
						//if (txtValue2 != null)
						//{
						//    nwlatencyDataTable.Rows[i]["LatencyRedThreshold"] = (txtValue2.Text == "" ? "0" : txtValue2.Text);

						//    if ((txtValue2.Text != null) && (txtValue2.Text != ""))
						//    {
						//        nwlatencyDataTable.Rows[i]["LatencyRedThreshold"] = txtValue2.Text;
						//    }
						//    else
						//    {

						//        txtValue2.Text = "40";
						//        //int s = 0;
						//        //int.TryParse(txtValue2.Text, out s);
						//        //txtValue2.Text = s.ToString();
						//    }
						//}
						if (NetworkLatencyTestgrd.Selection.IsRowSelected(i))
						{
							checkedvalue = Convert.ToBoolean(nwlatencyDataTable.Rows[i]["Enabled"] = "true");
						}
						else
						{
							nwlatencyDataTable.Rows[i]["Enabled"] = "false";
							checkedvalue = Convert.ToBoolean(nwlatencyDataTable.Rows[i]["Enabled"] = "false");
						}

					}
				}
				NetworkLatencyTestgrd.DataSource = nwlatencyDataTable;
				NetworkLatencyTestgrd.DataBind();

			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void FillNwLtncyServerGridfromSessionnew()
		{
			try
			{
				DataTable nwlatencyDataTable = new DataTable();
				if (Session["NetworkLatency"] != null && Session["NetworkLatency"] != "")
					nwlatencyDataTable = (DataTable)Session["NetworkLatency"];
				if (nwlatencyDataTable.Rows.Count > 0)
				{
					GridViewDataColumn column3 = NetworkLatencyTestgrd.Columns["LatencyYellowThreshold"] as GridViewDataColumn;
					GridViewDataColumn column4 = NetworkLatencyTestgrd.Columns["LatencyRedThreshold"] as GridViewDataColumn;
					int startIndex = NetworkLatencyTestgrd.PageIndex * NetworkLatencyTestgrd.SettingsPager.PageSize;
					int endIndex = Math.Min(NetworkLatencyTestgrd.VisibleRowCount, startIndex + NetworkLatencyTestgrd.SettingsPager.PageSize);

					for (int i = startIndex; i < endIndex; i++)
					{
						ASPxTextBox txtValue = (ASPxTextBox)NetworkLatencyTestgrd.FindRowCellTemplateControl(i, column3, "txtyellowthreshValue");
						ASPxTextBox txtValue2 = (ASPxTextBox)NetworkLatencyTestgrd.FindRowCellTemplateControl(i, column4, "txtredthreshValue");
						string yellow = txtyellothld.Text;
						string red = txtredthreshold.Text;
						if (txtValue != null)
						{
							nwlatencyDataTable.Rows[i]["LatencyYellowThreshold"] = txtyellothld.Text;
						}

						if (txtValue2 != null)
						{
							nwlatencyDataTable.Rows[i]["LatencyRedThreshold"] = txtredthreshold.Text;

						}
						//if (txtValue != null)
						//{
						//    nwlatencyDataTable.Rows[i]["LatencyYellowThreshold"] = (txtValue.Text == "" ? "0" : txtValue.Text);
						//    if ((txtValue.Text != null) && (txtValue.Text != ""))
						//    {
						//        nwlatencyDataTable.Rows[i]["LatencyYellowThreshold"] = txtValue.Text;
						//    }
						//    else
						//    {

						//        txtValue.Text = "30";
						//        //int Result = 0;
						//        //int.TryParse(txtValue.Text, out Result);
						//        //txtValue.Text = Result.ToString();
						//    }

						//}
						//if (txtValue2 != null)
						//{
						//    nwlatencyDataTable.Rows[i]["LatencyRedThreshold"] = (txtValue2.Text == "" ? "0" : txtValue2.Text);

						//    if ((txtValue2.Text != null) && (txtValue2.Text != ""))
						//    {
						//        nwlatencyDataTable.Rows[i]["LatencyRedThreshold"] = txtValue2.Text;
						//    }
						//    else
						//    {

						//        txtValue2.Text = "40";
						//        //int s = 0;
						//        //int.TryParse(txtValue2.Text, out s);
						//        //txtValue2.Text = s.ToString();
						//    }
						//}
						if (NetworkLatencyTestgrd.Selection.IsRowSelected(i))
						{
							checkedvalue = Convert.ToBoolean(nwlatencyDataTable.Rows[i]["Enabled"] = "true");
						}
						else
						{
							nwlatencyDataTable.Rows[i]["Enabled"] = "false";
							checkedvalue = Convert.ToBoolean(nwlatencyDataTable.Rows[i]["Enabled"] = "false");
						}

					}
				}
				NetworkLatencyTestgrd.DataSource = nwlatencyDataTable;
				NetworkLatencyTestgrd.DataBind();

			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void SaveNwLtncyServerGridnew()
		{
			bool Update = false;
			try
			{
				DataTable nwlatencyDataTable = new DataTable();
				if (Session["NetworkLatency"] != null && Session["NetworkLatency"] != "")
					nwlatencyDataTable = (DataTable)Session["NetworkLatency"];
				List<object> fieldValues = NetworkLatencyTestgrd.GetSelectedFieldValues(new string[] { "LatencyYellowThreshold", "LatencyRedThreshold" });


				//if (fieldValues.Count > 0)
				//{

                //10/21/2015 NS modified for VSPLUS-2223
                VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.DeleteNetworkLatencyServers(ServerKey.ToString());
				for (int i = 0; i < nwlatencyDataTable.Rows.Count; i++)
				{
					//ProfilesDataTable.Rows[0]["isSelected"] = "True";
					id = Convert.ToInt32(nwlatencyDataTable.Rows[i]["ID"]);
                    Update = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.Updatelatency(CollectDataForProfiles(nwlatencyDataTable.Rows[i]), nwlatencyDataTable.Rows[i]["ID"].ToString(), testtxtname.Text);
					
				}
			}
			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		private void SaveNwLtncyServerGrid()
		{
			bool Update = false;
			try
			{
				DataTable nwlatencyDataTable = new DataTable();
				if (Session["NetworkLatency"] != null && Session["NetworkLatency"] != "")
					nwlatencyDataTable = (DataTable)Session["NetworkLatency"];
				List<object> fieldValues = NetworkLatencyTestgrd.GetSelectedFieldValues(new string[] { "LatencyYellowThreshold", "LatencyRedThreshold", "Enabled" });


				if (fieldValues.Count > 0)
				{
                    //10/21/2015 NS modified for VSPLUS-2223
                    VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.DeleteNetworkLatencyServers(ServerKey.ToString());
					for (int i = 0; i < nwlatencyDataTable.Rows.Count; i++)
					{
						//ProfilesDataTable.Rows[0]["isSelected"] = "True";
						id = Convert.ToInt32(nwlatencyDataTable.Rows[i]["ID"]);
						Update = VSWebBL.ConfiguratorBL.NetworkLatencyBL.Ins.Updatelatency(CollectDataForProfiles(nwlatencyDataTable.Rows[i]), nwlatencyDataTable.Rows[i]["ID"].ToString(), testtxtname.Text);
						
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
		//private NetworkLatency CollectDataForlatency(DataRow ProfilesRow)
		//{
		//    try
		//    {
		//        NetworkLatency ProfilesObject = new NetworkLatency();
		//       // ProfileNames Profilesnamesobj = new ProfileNames();
		//        ProfilesObject.LatencyYellowThreshold = Convert.ToInt32(ProfilesRow["LatencyYellowThreshold"].ToString());
		//        ProfilesObject.LatencyRedThreshold = Convert.ToInt32(ProfilesRow["LatencyRedThreshold"].ToString());

		//        ProfilesObject.NetworkLatencyId = Convert.ToInt32(ProfilesRow["NetworkLatencyId"].ToString());
		//        ProfilesObject.Enabled = Convert.ToBoolean(ProfilesRow["Enabled"].ToString());
		//        //ProfilesObject.ProfileId = Convert.ToInt32(ProfileComboBox.SelectedItem.Value);
		//        //Profilesnamesobj.ProfileName = ProfileComboBox.SelectedItem.Text;
		//        return ProfilesObject;
		//    }
		//    catch (Exception ex)
		//    {
		//        //6/27/2014 NS added for VSPLUS-634
		//        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
		//        throw ex;
		//    }
		//    finally { }
		//}
		protected void NetworkLatencyTestgrd_OnPageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("NetworkLatencyTestServers|NetworkLatencyTestgrd", NetworkLatencyTestgrd.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
		protected void NetworkLatencyTestgrd_PreRender(object sender, EventArgs e)
		{
			try
			{

				if (isValid)
				{

					ASPxGridView NetworkLatencyTestgrd = (ASPxGridView)sender;
					int startIndex = NetworkLatencyTestgrd.PageIndex * NetworkLatencyTestgrd.SettingsPager.PageSize;
					int endIndex = Math.Min(NetworkLatencyTestgrd.VisibleRowCount, startIndex + NetworkLatencyTestgrd.SettingsPager.PageSize);
					for (int i = startIndex; i < endIndex; i++)
					{
						DataTable dt2 = (DataTable)Session["NetworkLatency"];
						//for (int i = 0; i < NetworkLatencyTestgrd.VisibleRowCount; i++)
						//{
                        //7/16/2015 NS modified - caused an exception for new records when running in debug mode
                        if (NetworkLatencyTestgrd.GetRowValues(i, "Enabled") != null)
                        {
                            if (NetworkLatencyTestgrd.GetRowValues(i, "Enabled").ToString() != "")
                            {
                                NetworkLatencyTestgrd.Selection.SetSelection(i, (bool)NetworkLatencyTestgrd.GetRowValues(i, "Enabled") == true);
                            }
                        }
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
		protected int GetFilteredRowCountWithoutPage()
		{
			int selectedRowsOnPage = 0;
			foreach (var key in NetworkLatencyTestgrd.GetCurrentPageRowValues("ID"))
			{
				if (NetworkLatencyTestgrd.Selection.IsRowSelectedByKey(key))
					selectedRowsOnPage++;
			}
			return NetworkLatencyTestgrd.Selection.FilteredCount - selectedRowsOnPage;
		}
		protected void FormCancelButton_Click(object sender, EventArgs e)
		{
			try
			{
				Response.Redirect("NetworkLatencyServers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
				Context.ApplicationInstance.CompleteRequest();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}
		protected void NetworkLatencyTestgrd_DataBound(object sender, EventArgs e)
		{

		}
		protected void btnupdate_Click(object sender, EventArgs e)
		{
            DataTable dt = (DataTable)Session["NetworkLatency"];
            ThresholdGridView.DataSource = dt;
            ThresholdGridView.DataBind();
            ThresholdPopupControl.ShowOnPageLoad = true;
            /*
			int ids;
			//string value = "";
			bool flag = false;
			string errtext = "";

			DataTable pagedatatable = new DataTable();

			pagedatatable = (DataTable)Session["NetworkLatency"];
			//pagedatatable.Reset();
			if (pagedatatable.Rows.Count > 0)
			{
				DataRow[] foundRows = pagedatatable.Select("Isselected='true'");
                List<object> LatencyYellowThreshold = NetworkLatencyTestgrd.GetSelectedFieldValues(new string[] { "LatencyYellowThreshold" });
                //if (foundRows.Length > 0)
				if (LatencyYellowThreshold.Count != 0)
				{
                    GridViewDataColumn column1 = NetworkLatencyTestgrd.Columns["LatencyYellowThreshold"] as GridViewDataColumn;
                    GridViewDataColumn column2 = NetworkLatencyTestgrd.Columns["LatencyRedThreshold"] as GridViewDataColumn;
					DataTable dt = new DataTable();
                    for (int i = 0; i < NetworkLatencyTestgrd.VisibleRowCount; i++)
                    {
                        if (NetworkLatencyTestgrd.Selection.IsRowSelected(i))
                        {
                            ASPxTextBox txtThresholdYellow = (ASPxTextBox)NetworkLatencyTestgrd.FindRowCellTemplateControl(i, column1, "txtyellowthreshValue");
                            ASPxTextBox txtThresholdRed = (ASPxTextBox)NetworkLatencyTestgrd.FindRowCellTemplateControl(i, column2, "txtredthreshValue");
							
                            pagedatatable.Rows[i]["LatencyYellowThreshold"] = txtyellothld.Text;
                            pagedatatable.Rows[i]["LatencyRedThreshold"] = txtredthreshold.Text;
                        }
                    }
                    NetworkLatencyTestgrd.DataSource = pagedatatable;
                    NetworkLatencyTestgrd.DataBind();

                    errorDiv2.Style.Value = "display: none";
					successDiv.Style.Value = "display: block";
				}
				else 
				{
					errorDiv2.Style.Value = "display: block";
					successDiv.Style.Value = "display: none";
					errtext = "Please select at least one server.";
					errorDiv2.InnerHtml = errtext + "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
				}
			
			}
            */
		}
		protected void txtyellowthreshValue_Init(object sender, EventArgs e)
		{


		}
		protected void NetworkLatencyTestgrd_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
		{
			if (e.Column.FieldName == "LatencyYellowThreshold")
			{
				Dictionary<object, int> lowerBoundStorage = Session["NetworkLatency"] as Dictionary<object, int>;
				if (lowerBoundStorage == null)
				{
					lowerBoundStorage = new Dictionary<object, int>();
					Session["NetworkLatency"] = lowerBoundStorage;
				}
				object key = e.GetListSourceFieldValue(e.ListSourceRowIndex, "ID");
				if (lowerBoundStorage.ContainsKey(key))
					e.Value = lowerBoundStorage[key];
				else
					e.Value = 0;
			}
		}
		protected void chkRow_CheckedChanged(object sender, EventArgs e)
		{

		}
		protected void chkRow_CheckedChanged1(object sender, EventArgs e)
		{
			DataTable dt = new DataTable();
			DataTable nwlatencyDataTable = new DataTable();
			int ids = 0;
			//string value = "";
			bool flag = false;
			nwlatencyDataTable = (DataTable)Session["NetworkLatency"];
			int startIndex = NetworkLatencyTestgrd.PageIndex * NetworkLatencyTestgrd.SettingsPager.PageSize;
			int endIndex = Math.Min(NetworkLatencyTestgrd.VisibleRowCount, startIndex + NetworkLatencyTestgrd.SettingsPager.PageSize);
			if (nwlatencyDataTable.Rows.Count > 0)
			{
				for (int i = startIndex; i < endIndex; i++)
				{

					GridViewDataColumn Ischeck = NetworkLatencyTestgrd.Columns[0] as GridViewDataColumn;
					CheckBox chkSelect = NetworkLatencyTestgrd.FindRowCellTemplateControl(i, Ischeck, "chkRow") as CheckBox;

					if (chkSelect != null)
					{
						bool Isselect = chkSelect.Checked;

						if (chkSelect.Checked)
						{


							nwlatencyDataTable = (DataTable)Session["NetworkLatency"];

							string yellow = txtyellothld.Text;
							string red = txtredthreshold.Text;

							if (Session["NetworkLatency"] != null && Session["NetworkLatency"] != "")
								nwlatencyDataTable = (DataTable)Session["NetworkLatency"];

							//FillNwLtncyServerGridfromSessionnew();
							ids = Convert.ToInt32(NetworkLatencyTestgrd.GetRowValues(i, "ID"));

							nwlatencyDataTable = (DataTable)Session["NetworkLatency"];
							var rowsToUpdate = nwlatencyDataTable.AsEnumerable().Where(r => r.Field<int>("ID") == ids);

							foreach (var row in rowsToUpdate)
							{
								row.SetField("LatencyYellowThreshold", yellow);
								row.SetField("LatencyRedThreshold", red);

							}

							nwlatencyDataTable.Rows[i]["Isselected"] = Isselect;
						}

						nwlatencyDataTable.Rows[i]["Isselected"] = Isselect;


						nwlatencyDataTable.AcceptChanges();
						Session["NetworkLatency"] = nwlatencyDataTable;
						
					}

				}
			}
			NetworkLatencyTestgrd.DataSource = (DataTable)Session["NetworkLatency"];
			NetworkLatencyTestgrd.DataBind();


		}
		protected void ASPxCheckBox1_CheckedChanged(object sender, EventArgs e)
		{

		}
		protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{
			Response.Redirect("~/Dashboard/LatencyTest.aspx?ID=" + ServerKey, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();

		}

        protected void TOKButton_Click(object sender, EventArgs e)
        {
            bool success = false;
            success = UpdateThresholds();
            if (success)
            {
                ThresholdPopupControl.ShowOnPageLoad = false;
            }
        }

        protected void TCancelButton_Click(object sender, EventArgs e)
        {
            ThresholdPopupControl.ShowOnPageLoad = false;
        }

        private bool UpdateThresholds()
        {
            string errtext = "";
            bool success = true;
            DataTable pagedatatable = new DataTable();

            pagedatatable = (DataTable)Session["NetworkLatency"];
            //pagedatatable.Reset();
            if (pagedatatable.Rows.Count > 0)
            {
                List<object> fieldValues = ThresholdGridView.GetSelectedFieldValues(new string[] { "ID" });
                if (fieldValues.Count != 0)
                {
                    DataTable dt = new DataTable();
                    DataRow[] foundrows;
                    foreach (int item in fieldValues)
                    {
                        foundrows = pagedatatable.Select("ID=" + item.ToString());
                        if (foundrows.Length > 0)
                        {
                            foundrows[0]["LatencyYellowThreshold"] = txtyellothld.Text;
                            foundrows[0]["LatencyRedThreshold"] = txtredthreshold.Text;
                        }
                    }
                    pagedatatable.AcceptChanges();
                    NetworkLatencyTestgrd.DataSource = pagedatatable;
                    NetworkLatencyTestgrd.DataBind();
                    errorDiv2.Style.Value = "display: none";
                    successDiv.Style.Value = "display: block";
                }
                else
                {
                    errorDiv2.Style.Value = "display: block";
                    successDiv.Style.Value = "display: none";
                    errtext = "Please select at least one server.";
                    errorDiv2.InnerHtml = errtext + "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    success = false;
                }
            }
            return success;
        }


        //5/17/2016 Sowjanya modified for VSPLUS-2967

        protected void ThresholdGridView_OnPageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("NetworkLatencyTestServers|ThresholdGridView", ThresholdGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        public void FillThresholdGridfromSession()
        {
            if (Session["NetworkLatency"] != null || Session["NetworkLatency"] != "")
            {
                DataTable dt = (DataTable)Session["NetworkLatency"];
                ThresholdGridView.DataSource = dt;
                ThresholdGridView.DataBind();
            }

        }


	}
}












