using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using DevExpress.Web;

using VSFramework;
using VSWebBL;
using VSWebDO;

namespace VSWebUI.Security
{
    public partial class MaintainServers : System.Web.UI.Page
    {
		//protected void Page_PreInit(object sender, EventArgs e)
		//{

		//    this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		//}

		//private void Master_ButtonClick(object sender, EventArgs e)
		//{

		//}
        DataTable ServersDataTable = null;
        public bool isIPVisibleforEdit;
        static int serverid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            //      DataTable dt = new DataTable();
            //ServersGridView.DataSource = dt;
            //ServersGridView.DataBind();
            //}
            //catch (Exception)
            //{
                
            //    throw;
            //}


            pnlAreaDtls.Style.Add("visibility", "hidden");
            if (!IsPostBack)
            {
                FillServersGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "MaintainServers|ServersGridView")
                        {
                            ServersGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillServersGridfromSession();

            }
        }

        private void FillServersGrid()
        {
            try
            {

                ServersDataTable = new DataTable();
                DataSet ServersDataSet = new DataSet();
                ServersDataTable = VSWebBL.SecurityBL.ServersBL.Ins.GetAllData(Convert.ToInt32(Session["UserID"]));
                if (ServersDataTable.Rows.Count > 0)
                {
                    //DataTable dtcopy = ServersDataTable.Copy();
                    //dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    ServersDataTable.PrimaryKey = new DataColumn[] { ServersDataTable.Columns["ID"] };
                }
                    Session["Servers"] = ServersDataTable;
                    ServersGridView.DataSource = ServersDataTable;
                    ServersGridView.DataBind();
               
               
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillServersGridfromSession()
        {
            try
            {

                ServersDataTable = new DataTable();
                if(Session["Servers"]!=null&&Session["Servers"]!="")
                ServersDataTable = (DataTable)Session["Servers"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (ServersDataTable.Rows.Count > 0)
                {
                    ServersGridView.DataSource = ServersDataTable;
                    ServersGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected DataRow GetRow(DataTable ServerObject, IDictionaryEnumerator enumerator,int Keys)
        {
            DataTable dataTable = ServerObject;
            DataRow DRRow = dataTable.NewRow();
            if (Keys == 0)
                DRRow = dataTable.NewRow();
            else
                DRRow = dataTable.Rows.Find(Keys);
            //IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
			while (enumerator.MoveNext())
			{
				if (enumerator.Key.ToString() == "MonthlyOperatingCost" || enumerator.Key.ToString() == "IdealUserCount")
					DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? DBNull.Value  : enumerator.Value);
				else
				DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
			}
            return DRRow;
        }

        protected void ServersGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            if (Session["Servers"] != null && Session["Servers"] != "")
            ServersDataTable = (DataTable)Session["Servers"];
            ASPxGridView gridView = (ASPxGridView)sender;
            DataRow newrow = GetRow(ServersDataTable, e.NewValues.GetEnumerator(), 0);

            DataRow[] matchrow = ServersDataTable.Select("ServerName = '" + newrow.ItemArray[1] + "' AND ServerType='" + newrow.ItemArray[2] + "' ");
            if (matchrow.Length > 0)
            {
                throw new ArgumentException("A server with this name and type already exists.");
            }

           

            //DataTable dataTable = STSettingsDataSet.Tables[0];
            //DataRow STSettingsRow = dataTable.NewRow();
            //STSettingsRow=GetRow(STSettingsDataSet, e.NewValues.GetEnumerator());

            //Insert row in DB
            UpdateServersData("Insert", GetRow(ServersDataTable, e.NewValues.GetEnumerator(),0));

            //Update Grid after inserting new row, refresh grid as in page load
            gridView.CancelEdit();
            e.Cancel = true;
            FillServersGrid();
        }

        private void UpdateServersData(string Mode, DataRow ServersRow)
        {
           
            if (Mode == "Insert")
            {
                Object ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.InsertData(CollectDataForServers(Mode, ServersRow));
                //For default row values for Exchange & all servers :Mukund 12Mar14
				ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.UpdateAttributesData(CollectDataforServerAttributes(ServersRow));
                
            }
            if (Mode == "Update")
            {
                Object ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.UpdateData(CollectDataForServers(Mode, ServersRow));

            }
        }

        private Servers CollectDataForServers(string Mode, DataRow ServersRow)
        {
            if (ServersRow["ServerType"].ToString() != "Database Availability Group" && ServersRow["IPAddress"].ToString() == "False")
            {
                throw new ArgumentException("Enter IPAddress");
            }
            else
            {
                try
                {
                    Servers ServersObject = new Servers();
                    if (Mode == "Update")
                    {
                        ServersObject.ID = int.Parse(ServersRow["ID"].ToString());
                    }
                    ServersObject.ServerName = ServersRow["ServerName"].ToString();
                    ServersObject.IPAddress = ServersRow["IPAddress"].ToString();
					if ( ServersRow["MonthlyOperatingCost"].ToString() != "")
						ServersObject.MonthlyOperatingCost = Convert.ToDouble(ServersRow["MonthlyOperatingCost"]);
					if (ServersRow["IdealUserCount"].ToString() != "")
						ServersObject.IdealUserCount = Convert.ToInt32(ServersRow["IdealUserCount"]);
					ProfileNames profilenames = new ProfileNames();
					profilenames.ProfileName = ServersRow["ProfileName"].ToString();
				//	ServersObject.ProfileName = ServersRow["ProfileName"].ToString();
					ProfileNames ReturnPname = VSWebBL.SecurityBL.ProfilesNamesBL.Ins.GetDataForLocation1(profilenames);
					ServersObject.ProfileName = Convert.ToString(ReturnPname.ID);
                    ServersObject.Description = ServersRow["Description"].ToString();
                    ServerTypes STypeobject = new ServerTypes();
                    STypeobject.ServerType = ServersRow["ServerType"].ToString();
                    ServerTypes ReturnValue = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetDataForServerType(STypeobject);
                    ServersObject.ServerTypeID = ReturnValue.ID;

                    //DataTable ReturnValue = VSWebBL.SecurityBL.AdminTabBL.Ins.GetNavigatorByDisplayText(ServersRow["ServerType"].ToString());
                    //ServersObject.ServerTypeID =int.Parse(ReturnValue.Rows[0]["ID"].ToString());

                    //ServersObject.ServerTypeID = ServerTypeComboBox.Text;
                    // ServersObject.LocationID = int.Parse(ServersRow["LocationID"].ToString());
                    //ServersObject.ServerTypeID = int.Parse(ServersRow["ServerType"].ToString());
                    //ServersObject.LocationID = int.Parse(ServersRow["Location"].ToString());
                    Locations LOCobject = new Locations();
                    LOCobject.Location = ServersRow["Location"].ToString();

                    Locations ReturnLocValue = VSWebBL.SecurityBL.LocationsBL.Ins.GetDataForLocation(LOCobject);
                    ServersObject.LocationID = ReturnLocValue.ID;
					HoursIndicator Busibject = new HoursIndicator();

					Busibject.Type = ServersRow["Type"].ToString();


					HoursIndicator ReturnBusiValue = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.GetDataForBUsinesshrs(Busibject);
					ServersObject.BusinesshoursID = ReturnBusiValue.ID;


                    return ServersObject;
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }
                finally { }
            }
        }

        public ServerAttributes CollectDataforServerAttributes(DataRow ServersRow)
        {
            ServerAttributes Mobj = new ServerAttributes();
            try
            {
                
                int ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(ServersRow["ServerName"].ToString());
                //DEFAULTS TO BE GIVEN

                //Mobj.Enabled = EnabledCheckBox.Checked;
                //Mobj.CPUThreshold = (CPUThresholdTextBox.Text == "" ? 0 : Convert.ToInt32(CPUThresholdTextBox.Text));
                //Mobj.MemThreshold = (MemThresholdTextBox.Text == "" ? 0 : Convert.ToInt32(MemThresholdTextBox.Text));
                //Mobj.Category = CategoryTextBox.Text;
                //Mobj.OffHourInterval = (OffscanTextBox.Text == "" ? 0 : Convert.ToInt32(OffscanTextBox.Text));
                //Mobj.ScanInterval = (ScanIntvlTextBox.Text == "" ? 0 : Convert.ToInt32(ScanIntvlTextBox.Text));
                //Mobj.ResponseTime = (ResponseTextBox.Text == "" ? 0 : Convert.ToInt32(ResponseTextBox.Text));
                //Mobj.RetryInterval = (RetryTextBox.Text == "" ? 0 : Convert.ToInt32(RetryTextBox.Text));
                //Mobj.ConsFailuresBefAlert = (ConsFailuresBefAlertTextBox.Text == "" ? 0 : Convert.ToInt32(ConsFailuresBefAlertTextBox.Text));
                //Mobj.ConsOvrThresholdBefAlert = (ConsOvrThresholdBefAlertTextBox.Text == "" ? 0 : Convert.ToInt32(ConsOvrThresholdBefAlertTextBox.Text));
                Mobj.ServerId = ReturnValue;

            }
            catch (Exception ex)
            {

                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

            return Mobj;

        }

        protected void ServersGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            if (Session["Servers"] != null && Session["Servers"] != "")
            ServersDataTable = (DataTable)Session["Servers"];
            ASPxGridView gridView = (ASPxGridView)sender;

            //DataTable dataTable = STSettingsDataSet.Tables[0];
            //DataRow STSettingsRow = dataTable.NewRow();
            //STSettingsRow=GetRow(STSettingsDataSet, e.NewValues.GetEnumerator());

            //Update row in DB
            UpdateServersData("Update", GetRow(ServersDataTable, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0])));
            //Update Grid after inserting new row, refresh grid as in page load
            gridView.CancelEdit();
            e.Cancel = true;
            FillServersGrid();
        }

        protected void ServersGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            Servers ServerObject = new Servers();
            ServerObject.ID = Convert.ToInt32(e.Keys[0]);
			ServerObject.ServerType = e.Values["ServerType"].ToString();

            //Delete row from DB
            
            
                Object ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.DeleteData(ServerObject);
            
          
           
                //ErrorMessageLabel.Text = "Server Cannot Be Deleted ";
                //ErrorMessagePopupControl.HeaderText = "Alert!!";
                //ErrorMessagePopupControl.ShowCloseButton = false;
                //ValidationUpdatedButton.Visible = true;
                //ValidationOkButton.Visible = true;
                //ErrorMessagePopupControl.ShowOnPageLoad = true;



				if (!(ReturnValue is bool) || Convert.ToBoolean(ReturnValue) == false)
                {
					throw new Exception("Cannot delete this server.  " + ReturnValue.ToString());

                    //ErrorMessageLabel.Text = "Server Cannot Be Deleted ";
                    //ErrorMessagePopupControl.HeaderText = "Alert!!";
                    //ErrorMessagePopupControl.ShowCloseButton = false;
                    //ValidationUpdatedButton.Visible = true;
                    //ValidationOkButton.Visible = true;
                    //ErrorMessagePopupControl.ShowOnPageLoad = true;
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "Not deleted", "alert('This Server Is Using Somewhere, Cannot Delete.');window.open('MaintainServers.aspx');", true);
                }
                else
                {
                    ASPxGridView gridView = (ASPxGridView)sender;
                    gridView.CancelEdit();
                    e.Cancel = true;
                    FillServersGrid();
                }
            //Update Grid after inserting new row, refresh grid as in page load
            
        }

        //protected void ServersGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        //{
        //    if (e.Column.FieldName == "ServerType")
        //    {
        //        ASPxComboBox ServerTypeComboBox = e.Editor as ASPxComboBox;
        //        //6/30/2014 NS added for VSPLUS-781
        //        if (!ServersGridView.IsNewRowEditing)
        //        {
        //            e.Editor.ReadOnly = true;
        //        }
        //        else
        //        {
        //            FillServerTypeComboBox(ServerTypeComboBox);
        //        }
        //        ServerTypeComboBox.Callback += new CallbackEventHandlerBase(ServerTypeComboBox_OnCallback);
        //    }
        //    if (e.Column.FieldName == "Location")
        //    {
        //        ASPxComboBox LocationComboBox = e.Editor as ASPxComboBox;
        //        FillLocationComboBox(LocationComboBox);
        //        LocationComboBox.Callback += new CallbackEventHandlerBase(LocationComboBox_OnCallback);

        //    }
        //}


        protected void ServersGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
			
		  //e.Editor.SetClientSideEventHandler("KeyPress", "OnTextBoxKeyPress");
       
            //VE-126	Maintain servers - Don't require IP Address when server type is Database Availability Group
            if (e.Column.FieldName == "ServerType")
            {
                //var combo = (ASPxComboBox)e.Editor;
                //combo.Callback += new CallbackEventHandlerBase(combo_Callback);

				isIPVisibleforEdit = true;
                if (e.Value != null)
                {
                    if (e.Value.ToString() == "Database Availability Group")
                    {
						isIPVisibleforEdit = false;

                    }
                }
                
                ASPxComboBox ServerTypeComboBox = e.Editor as ASPxComboBox;
                //6/30/2014 NS added for VSPLUS-781
                if (!ServersGridView.IsNewRowEditing)
                {
					e.Editor.Enabled= false;
                    
                }
                else
                {
                    FillServerTypeComboBox(ServerTypeComboBox);
                }
                ServerTypeComboBox.Callback += new CallbackEventHandlerBase(ServerTypeComboBox_OnCallback);
            }
            if (e.Column.FieldName == "Location")
            {
                ASPxComboBox LocationComboBox = e.Editor as ASPxComboBox;
                FillLocationComboBox(LocationComboBox);
                LocationComboBox.Callback += new CallbackEventHandlerBase(LocationComboBox_OnCallback);
				

            }
			if (e.Column.FieldName == "ProfileName")
			{
				
					//var combo = (ASPxComboBox)e.Editor;
					//combo.Callback += new CallbackEventHandlerBase(combo_Callback);

				    isIPVisibleforEdit = true;
					ASPxComboBox profileComboBox = e.Editor as ASPxComboBox;
					if (!ServersGridView.IsNewRowEditing)
					{
						e.Editor.Enabled = false;
					}
					else
					{
						FillprofileComboBox(profileComboBox);
					}
						
					profileComboBox.Callback += new CallbackEventHandlerBase(profileComboBox_OnCallback);
                    profileComboBox.ValidationSettings.RequiredField.IsRequired = false;
					//profileComboBox.Enabled = false;

				}
			if (e.Column.FieldName == "Type")
			{
				ASPxComboBox BusinesshoursComboBox = e.Editor as ASPxComboBox;
				FillBusinesshoursComboBox(BusinesshoursComboBox);
				BusinesshoursComboBox.Callback += new CallbackEventHandlerBase(BusinesshoursComboBox_OnCallback);

			}
            if (e.Column.FieldName == "IPAddress")
            {
                if (!isIPVisibleforEdit)
                {

                    e.Editor.Parent.Parent.Visible = false;

                }
                else
                {
                    e.Editor.Parent.Parent.Visible = true;
                }
            }
        }
        private void combo_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "Database Availability Group")
            {
                
                //object s = "IPAddress";
                //ASPxTextBox txt = (ASPxTextBox) s ;
                string txtSelected="IPAddress";
                ASPxTextBox txt;
                txt = (ASPxTextBox)this.FindControl(txtSelected);
                txt.ValidationSettings.RequiredField.IsRequired = false;
            }
        
        }
        private void ServerTypeComboBox_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillServerTypeComboBox(source as ASPxComboBox);
        }

        private void FillServerTypeComboBox(ASPxComboBox ServerTypeComboBox)
        {
            DataTable ServerDataTable = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetSpecificServerTypes();
            //DataTable ServerDataTable = VSWebBL.SecurityBL.AdminTabBL.Ins.GetNavigatorChildsByRefName("ServersDevices");
            ServerTypeComboBox.DataSource = ServerDataTable;
            ServerTypeComboBox.TextField = "ServerType";
            ServerTypeComboBox.ValueField = "ServerType";
            ServerTypeComboBox.DataBind();
        }

        private void LocationComboBox_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillLocationComboBox(source as ASPxComboBox);
        }
	
        private void FillLocationComboBox(ASPxComboBox LocationComboBox)
        {
            DataTable LocationDataTable = VSWebBL.SecurityBL.LocationsBL.Ins.GetAllData();
            LocationComboBox.DataSource = LocationDataTable;
            LocationComboBox.TextField = "Location";
            LocationComboBox.ValueField = "Location";
            LocationComboBox.DataBind();
        }
		private void profileComboBox_OnCallback(object source, CallbackEventArgsBase e)
		{
			FillprofileComboBox(source as ASPxComboBox);
		}
		private void FillprofileComboBox(ASPxComboBox profileComboBox)
		{
			DataTable ProfileNamesDataTable = VSWebBL.SecurityBL.ProfilesNamesBL.Ins.GetAllData();
			profileComboBox.DataSource = ProfileNamesDataTable;
			profileComboBox.TextField = "ProfileName";
			profileComboBox.ValueField = "ProfileName";
			profileComboBox.DataBind();
		}
		
        protected void ValidationOkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("MaintainServers.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            // btn.Attributes["onClick"]=
            Servers ServerObject = new Servers();
            ServerObject.ID = Convert.ToInt32(btn.CommandArgument);
            serverid = Convert.ToInt32(btn.CommandArgument);
            string name = btn.AlternateText;
            pnlAreaDtls.Style.Add("visibility", "visible");
            divmsg.InnerHtml = "Are you sure you want to delete the server " + name + "?";
            //string name = btn.AlternateText;
            //string title = "Confirm";
            //string text = @"Are You Sure to Delete : " + name + " ";
            ////"Hello everyone, I am an Asp.net MessageBox. You can set MessageBox.SuccessEvent and MessageBox.FailedEvent and Click Yes(OK) or No(Cancel) buttons for calling them. The Methods must be a WebMethod because client-side application will call web services.";
            //MessageBox messageBox = new MessageBox(text, title, MessageBox.MessageBoxIcons.Question, MessageBox.MessageBoxButtons.OKCancel, MessageBox.MessageBoxStyle.StyleB);
            //messageBox.SuccessEvent.Add("OkClick");
            //messageBox.SuccessEvent.Add("OkClick");
            //messageBox.FailedEvent.Add("CancalClick");
            //Literal1.Text = messageBox.Show(this);
            //Object ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.DeleteData(ServerObject);



            ////ErrorMessageLabel.Text = "Server Cannot Be Deleted ";
            ////ErrorMessagePopupControl.HeaderText = "Alert!!";
            ////ErrorMessagePopupControl.ShowCloseButton = false;
            ////ValidationUpdatedButton.Visible = true;
            ////ValidationOkButton.Visible = true;
            ////ErrorMessagePopupControl.ShowOnPageLoad = true;



            //if (Convert.ToBoolean(ReturnValue) == false)
            //{
            //    //ServersGridView.JSProperties["cpCancelEdit"] = e.Cancel;
            //    //ServersGridView.JSProperties["cpMessage"] = string.Format("Server Cant Be Deleted");

            //    // throw new Exception("Cant Be Deleted");
            //    //ASPxGridView gridView = (ASPxGridView)sender;
            //    // ErrorMessageLabel.Text = "Server Cannot Be Deleted ";
            //    // ErrorMessagePopupControl.HeaderText = "Alert!!";
            //    // ErrorMessagePopupControl.ShowCloseButton = false;
            //    // ValidationUpdatedButton.Visible = true;
            //    // ValidationOkButton.Visible = true;
            //    // //gridView.CancelEdit();
            //    // //e.Cancel = true;
            //    //ErrorMessagePopupControl.ShowOnPageLoad = true;
            //    // ErrorMessagePopupControl.ClientSideEvents.Shown="OnShown";

            //    // Response.Write("<script>alert('Cant')</script>");

            //    NavigatorPopupControl.ShowOnPageLoad = true;
            //    MsgLabel.Text = "Server Cannot Be Deleted ";
            //    //ErrorMessageLabel.Text = "Server Cannot Be Deleted ";
            //    //ErrorMessagePopupControl.HeaderText = "Alert!!";
            //    //ErrorMessagePopupControl.ShowCloseButton = false;
            //    //ValidationUpdatedButton.Visible = true;
            //    //ValidationOkButton.Visible = true;
            //    //ErrorMessagePopupControl.IsVisible();
            //    // ScriptManager.RegisterStartupScript(this, this.GetType(), "Not deleted", "alert('This Server Is Using Somewhere, Cannot Delete.');", true);
            //}
            //else
            //{
            //    // ASPxGridView gridView = (ASPxGridView)sender;
            //    //gridView.CancelEdit();
            //    //e.Cancel = true;
            //    FillServersGrid();
            //}
        }
        protected void btn_OkClick(object sender, EventArgs e)
        {
            Servers ServerObject = new Servers();
            ServerObject.ID = serverid;
            Object ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.DeleteData(ServerObject);
            if (Convert.ToBoolean(ReturnValue) == false)
            {
                NavigatorPopupControl.ShowOnPageLoad = true;
                MsgLabel.Text = "This server cannot be deleted, other dependencies exist.";
            }
            else
            {
                //ASPxGridView gridView = (ASPxGridView)sender;
                //gridView.CancelEdit();
                //e.Cancel = true;
                FillServersGrid();
            }
            //pnlAreaDtls.Style.Add("visibility", "hidden");

            //Updates top boxes on delete
            Site1 currMaster = (Site1)this.Master;
            currMaster.refreshStatusBoxes();

        }
        protected void btn_CancelClick(object sender, EventArgs e)
        {
            // ASPxGridView gridView = (ASPxGridView)sender;
            //gridView.CancelEdit();
            //e.Cancel = true;
            FillServersGrid();
            // pnlAreaDtls.Style.Add("visibility", "hidden");
        }
        protected void OKButton_Click(object sender, EventArgs e)
        {
            NavigatorPopupControl.ShowOnPageLoad = false;
        }

        protected void ServersGridView_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "ServerType")
            {
                ASPxComboBox ServerTypeComboBox = e.Editor as ASPxComboBox;
                FillServerTypeComboBox(ServerTypeComboBox);
                ServerTypeComboBox.Callback += new CallbackEventHandlerBase(ServerTypeComboBox_OnCallback);
            }
            if (e.Column.FieldName == "Location")
            {
                ASPxComboBox LocationComboBox = e.Editor as ASPxComboBox;
                FillLocationComboBox(LocationComboBox);
                LocationComboBox.Callback += new CallbackEventHandlerBase(LocationComboBox_OnCallback);
            }
			if (e.Column.FieldName == "ProfileName")
			{
				ASPxComboBox profileComboBox = e.Editor as ASPxComboBox;
				FillprofileComboBox(profileComboBox);
				profileComboBox.Callback += new CallbackEventHandlerBase(profileComboBox_OnCallback);
			}
			if (e.Column.FieldName == "Type")
			{
				ASPxComboBox BusinesshoursComboBox = e.Editor as ASPxComboBox;
				FillBusinesshoursComboBox(BusinesshoursComboBox);
				BusinesshoursComboBox.Callback += new CallbackEventHandlerBase(BusinesshoursComboBox_OnCallback);
			}
        }
		private void BusinesshoursComboBox_OnCallback(object source, CallbackEventArgsBase e)
		{
			FillBusinesshoursComboBox(source as ASPxComboBox);
		}

        protected void ServersGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("MaintainServers|ServersGridView", ServersGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
		private void FillBusinesshoursComboBox(ASPxComboBox BusinesshoursComboBox)
		{
			//DataTable ProfileNamesDataTable = VSWebBL.SecurityBL.ProfilesNamesBL.Ins.GetAllData();
			DataTable Businesshoursname = VSWebBL.ConfiguratorBL.Businesshoursbl.Ins.GetBusinesshoursNames();

			BusinesshoursComboBox.DataSource = Businesshoursname;
			BusinesshoursComboBox.TextField = "Type";
			BusinesshoursComboBox.ValueField = "Type";
			BusinesshoursComboBox.DataBind();
		}

        //8/21/2015 NS added for VSPLUS-2087
        protected void ServersGridView_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e)
        {
            Exception ex = e.Exception;
            if (ex != null)
            {

                if (ex.Message.IndexOf("ErrCode!-1") != -1)
                {
                    e.ErrorText = ex.Message;
                }
                else if (ex.Message.IndexOf("ErrCode=-1") > 0)
                {
                    e.ErrorText = "some error";
                }
                else
                {
                    e.ErrorText = ex.Message;
                }
            }
        }
    }
}
