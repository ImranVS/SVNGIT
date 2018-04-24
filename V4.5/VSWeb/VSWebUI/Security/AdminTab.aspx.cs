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
    public partial class AdminTab : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        object msgloc="";
        object msgservertype = "";
        DataTable LocationsDataTable = null;
        static int locID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
			//ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('InnerHtml');", true);
            pnlAreaDtls.Style.Add("visibility", "hidden");
            if (!IsPostBack)
            {
                FillLocationsGrid();
                fillservertypegridview();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "AdminTab|LocationsGridView")
                        {
                            LocationsGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "AdminTab|ServerTypesGridView")
                        {
                            ServerTypesGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillLocationsGridfromSession();
                fillservertypegridviewbysession();

            }
			if (Session["ErrorStatus"] != null)
			{
				if (Session["ErrorStatus"].ToString() != "")
				{
					//10/3/2014 NS modified for VSPLUS-990
					errordiv.InnerHtml = " <b>" + Session["ErrorStatus"].ToString() +
						"</b>";
					errordiv.Style.Value = "display: block";
					Session["ErrorStatus"] = "";
				}
			}
            //4/5/2016 Sowjanya modified for VSPLUS-2497
            if (Session["LocationStatus"] != null)
            {
                if (Session["LocationStatus"].ToString() != "")
                {

                    successDiv.InnerHtml = "Locations information for <b>" + Session["LocationStatus"].ToString() +
                        "</b> updated successfully." +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                    Session["LocationStatus"] = "";
                }
            }
        }

        //#region "Procedures/Functions"
        
       

        //private string GetRestrictedServersFromGrid()
        //{
        //    string selValues = "";
        //    for (int i = 0; i < SpecificServersNotVisibleGridView.VisibleRowCount; i++)
        //    {
        //        if (selValues == "")
        //        {
        //            selValues = "'" + SpecificServersNotVisibleGridView.GetRowValues(i, SpecificServersNotVisibleGridView.KeyFieldName).ToString() + "'";
        //        }
        //        else
        //        {
        //            selValues += "," + "'" + SpecificServersNotVisibleGridView.GetRowValues(i, SpecificServersNotVisibleGridView.KeyFieldName).ToString() + "'";
        //        }
        //    }
        //    return selValues;
        //}

       

        
        //#endregion

        private void FillLocationsGrid()
        {
            try
            {

                 LocationsDataTable = new DataTable();
                 DataSet LocationDataSet = new DataSet();
                 LocationsDataTable = VSWebBL.SecurityBL.LocationsBL.Ins.GetAllData();
                 if (LocationsDataTable.Rows.Count > 0)
                 {
                     DataTable dtcopy = LocationsDataTable.Copy();
                     dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                     // LocationDataSet.Tables.Add(dtcopy);

                     //Session["LocationDataSet"] = LocationDataSet;

                     Session["Locations"] = dtcopy;
                     LocationsGridView.DataSource = LocationsDataTable;
                     LocationsGridView.DataBind();
                 }
                 else
                 {
                     LocationsGridView.DataSource = LocationsDataTable;
                     LocationsGridView.DataBind();
                     Session["Locations"] = LocationsDataTable;
                 }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillLocationsGridfromSession()
        {
            try
            {

                LocationsDataTable = new DataTable();
                if(Session["Locations"]!=""&&Session["Locations"]!=null)
                LocationsDataTable = (DataTable)Session["Locations"];//VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.GetAllData();
                if (LocationsDataTable.Rows.Count > 0)
                {
                    LocationsGridView.DataSource = LocationsDataTable;
                    LocationsGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        protected DataRow GetRow(DataTable LocObject, IDictionaryEnumerator enumerator,int Keys)
        {
            //4/24/2013 NS added a fix for cases when there are no entries in the Locations table yet
            DataTable dataTable = new DataTable();
            if (LocObject != null)
            {
                dataTable = LocObject;
            }
            else
            {
                dataTable.Columns.Add("Location");
            }
            DataRow DRRow=null;
            if(Keys==0)
               
                    DRRow = dataTable.NewRow();
               
            else
                DRRow = dataTable.Rows.Find(Keys);
            //IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
            return DRRow;
        }

        protected void LocationsGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            //4/24/2013 NS added a fix for an exception when there are no Locations available
            if (Session["Locations"] != null && Session["Locations"] != "")
            {
                LocationsDataTable = (DataTable)Session["Locations"];
                //divmsg.InnerHtml = "Duplicate Rows Are not Allowed";
            }
            else
            {
                LocationsDataTable = null;
               
            }
            ASPxGridView gridView = (ASPxGridView)sender;

            //DataTable dataTable = STSettingsDataSet.Tables[0];
            //DataRow STSettingsRow = dataTable.NewRow();
            //STSettingsRow=GetRow(STSettingsDataSet, e.NewValues.GetEnumerator());

            //Insert row in DB
            UpdateLocationData("Insert", GetRow(LocationsDataTable, e.NewValues.GetEnumerator(), 0));
            //Update Grid after inserting new row, refresh grid as in page load
            gridView.CancelEdit();
            e.Cancel = true;
            FillLocationsGrid();
            
        }

        private void UpdateLocationData(string Mode, DataRow LocationsRow)
        {
            if (Mode == "Insert")
            {
                Object ReturnValue = VSWebBL.SecurityBL.LocationsBL.Ins.InsertData(CollectDataForLocations(Mode, LocationsRow));
                if (ReturnValue == "")
                {
                    msgloc= "Location Already Exists";
                    throw new ArgumentException("Location Already Exists");
                }
            }
            if (Mode == "Update")
            {
                Object ReturnValue = VSWebBL.SecurityBL.LocationsBL.Ins.UpdateData(CollectDataForLocations(Mode, LocationsRow));
            
            }
        }

        private Locations CollectDataForLocations(string Mode, DataRow LocationsRow)
        {
            try
            {
                Locations LocationsObject = new Locations();
                LocationsObject.Location = LocationsRow["Location"].ToString();
               if(Mode=="Update")
                LocationsObject.ID = int.Parse(LocationsRow["ID"].ToString());

                return LocationsObject;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void LocationsGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            LocationsDataTable = (DataTable)Session["Locations"];
            ASPxGridView gridView = (ASPxGridView)sender;

            //DataTable dataTable = STSettingsDataSet.Tables[0];
            //DataRow STSettingsRow = dataTable.NewRow();
            //STSettingsRow=GetRow(STSettingsDataSet, e.NewValues.GetEnumerator());

            //Insert row in DB
            UpdateLocationData("Update", GetRow(LocationsDataTable, e.NewValues.GetEnumerator(),Convert.ToInt32(e.Keys[0])));
            //Update Grid after inserting new row, refresh grid as in page load
            gridView.CancelEdit();
            e.Cancel = true;
            FillLocationsGrid();
            tdmsgforlocation.Text = msgloc.ToString();
        }

		protected void LocationsGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
			Locations LocObject = new Locations();
			LocObject.ID = Convert.ToInt32(e.Keys[0]);

			//Delete row from DB
			String ReturnValue = VSWebBL.SecurityBL.LocationsBL.Ins.DeleteData(LocObject);
			if (ReturnValue != "true")
			{
				string AdditionalMsg = "This location is in use by a <DeviceType>. The <DeviceType> must be deleted or have its location changed first.";
				string deviceType = "";

				if (ReturnValue.Contains("fk_MailServices_LocationID"))
				{
					deviceType = "mail service";
				}
				else if (ReturnValue.Contains("FK_Servers_Location"))
				{
					deviceType = "server";
				}
				if(deviceType == "")
				{
					(Session["ErrorStatus"]) = "This Location is used elsewhere for different servers. Cannot delete.";
				}
				else
				{
					Session["ErrorStatus"] = AdditionalMsg.Replace("<DeviceType>", deviceType);
				}

				DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/Security/AdminTab.aspx");
			}
			
			ASPxGridView gridView = (ASPxGridView)sender;
			gridView.CancelEdit();
			e.Cancel = true;
			FillLocationsGrid();
		}

		protected void LocationsGridView_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
		{

			if (e.RowType == GridViewRowType.EditForm)
			{
				try
				{
					if (e.GetValue("ID") != " " && e.GetValue("ID") != null)
					{
						ASPxWebControl.RedirectOnCallback("LocationsEdit.aspx?ID=" + e.GetValue("ID"));
						Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					}
					else
					{
						ASPxWebControl.RedirectOnCallback("LocationsEdit.aspx");
						Context.ApplicationInstance.CompleteRequest();//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
					}
				}
				catch (Exception ex)
				{
					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				}

			}
		}


        private void fillservertypegridview()
        {
            try
            {
                DataTable servertypedatatable = new DataTable();
                servertypedatatable = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetAllData();
                if (servertypedatatable.Rows.Count > 0)
                {
                    servertypedatatable.PrimaryKey = new DataColumn[] { servertypedatatable.Columns["ID"] };

                }
                    Session["servertypes"] = servertypedatatable;
                    ServerTypesGridView.DataSource = servertypedatatable;
                    ServerTypesGridView.DataBind();
              

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        private void fillservertypegridviewbysession()
        {
            try
            {
                DataTable fillserverstype = new DataTable();
                if(Session["servertypes"]!=""&&Session["servertypes"]!=null)
                fillserverstype = (DataTable)Session["servertypes"];
                if (fillserverstype.Rows.Count > 0)
                {
                    ServerTypesGridView.DataSource = fillserverstype;
                    ServerTypesGridView.DataBind();

                }

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

        }

        protected DataRow getrowservertype(DataTable servertype, IDictionaryEnumerator numerator, int keys)
        {
            DataRow row;
            DataTable dt = servertype;
            if (keys == 0)
            {
                row = dt.NewRow();
            }
            else
            {
                row = dt.Rows.Find(keys);
            }
            numerator.Reset();
            while (numerator.MoveNext())
                //DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
                row[numerator.Key.ToString()] = (numerator.Value == null ? "False" : numerator.Value);
            return row;
        }

        private ServerTypes CollectdataforServertypes(string mode, DataRow servertyperow)
        {
            try
            {
                ServerTypes ServertypeObject = new ServerTypes();
                ServertypeObject.ServerType = servertyperow["ServerType"].ToString();
                if (mode == "Update")
                {
                    ServertypeObject.ID=int.Parse(servertyperow["ID"].ToString());
                }
                return ServertypeObject;
            }
            catch(Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
         }

        private void updateServertype(string mode, DataRow servertyperow)
        {
          //  ServerTypes servertypeObject=new ServerTypes();
            if (mode == "Insert")
            {
                Object returnval = VSWebBL.SecurityBL.ServerTypesBL.Ins.insertforservertypes(CollectdataforServertypes(mode, servertyperow));
                if (returnval == "")
                {
                    ErrorMessageLabel.Text = "Server Type is Already Exists";
                    ErrorMessagePopupControl.ShowOnPageLoad = true;
                }
            }
            if (mode == "Update")
            {
                Object val = VSWebBL.SecurityBL.ServerTypesBL.Ins.updateforservertype(CollectdataforServertypes(mode, servertyperow));
            }
        }



        protected void ServerTypesGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            DataTable servertypes = new DataTable();
            servertypes = (DataTable)Session["servertypes"];
            ASPxGridView gridView = (ASPxGridView)sender;
            updateServertype("Update", getrowservertype(servertypes, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0])));
            gridView.CancelEdit();
            e.Cancel = true;

            fillservertypegridview();


                
        }

        protected void ServerTypesGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            DataTable servertype = new DataTable();
            servertype = (DataTable)Session["servertypes"];
            ASPxGridView gridView = (ASPxGridView)sender;
            updateServertype("Insert",GetRow(servertype,e.NewValues.GetEnumerator(),0));
            gridView.CancelEdit();
            e.Cancel = true;
            fillservertypegridview();
            tdmsgforServertype.InnerHtml = msgservertype.ToString();

          
            //DataView dv = (DataView)AccessDataSource.EqualsS(DataSourceSelectArguments.Empty);

            //if (dv.Table.Select("UnitsOnOrder = " + e.NewValues["UnitsOnOrder"]).Length > 0)
            //    throw new Exception("UnitsOnOrder = " + e.NewValues["UnitsOnOrder"] + " is already exists");


        }

        protected void ServerTypesGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ServerTypes servertypeObject = new ServerTypes();
            servertypeObject.ID = Convert.ToInt32(e.Keys[0]);
            Object returval = VSWebBL.SecurityBL.ServerTypesBL.Ins.deleteforServertype(servertypeObject);
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            fillservertypegridview();
        }

        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminTab.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void bttnDelete_Click(object sender, EventArgs e)
        {
            ImageButton bttnDel = (ImageButton)sender;
            Locations locObj = new Locations();
            locObj.ID = Convert.ToInt32(bttnDel.CommandArgument);
            locID = Convert.ToInt32(bttnDel.CommandArgument);
            string locName = bttnDel.AlternateText;
            pnlAreaDtls.Style.Add("visibility", "visible");
            //5/14/2014 NS added for VSPLUS-627
            DataTable dt = VSWebBL.SecurityBL.AdminTabBL.Ins.Getserversbyloction(locName);
            if (dt.Rows.Count == 0)
            {
                divmsg.InnerHtml = "Are you sure you want to delete the server location " + locName + "?";
                bttnOK.Visible = true;
            }
            else
            {
                bttnOK.Visible = false;
                divmsg.InnerHtml = "You may not delete the location " + locName + " because there are servers assigned to it. " +
                    "Please re-assign servers to other locations or delete all servers for this location before deleting " + locName + ".";
            }
        }        

        protected void bttnOK_Click(object sender, EventArgs e)
        {
            Locations locObj = new Locations();
            locObj.ID = locID;
            Object returnValue = VSWebBL.SecurityBL.LocationsBL.Ins.DeleteData(locObj);
            if (Convert.ToBoolean(returnValue) == false)
            {
                NavigatorPopupControl.ShowOnPageLoad = true;
                MsgLabel.Text = "This location cannot be deleted, other dependencies exist.";
            }
            else
            {
                FillLocationsGrid();
            }
        }
       
        protected void bttnCancel_Click(object sender, EventArgs e)
        {
            FillLocationsGrid();
        }        

        protected void subbttnOK_Click(object sender, EventArgs e)
        {
            NavigatorPopupControl.ShowOnPageLoad = false;
        }

        protected void LocationsGridView_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AdminTab|LocationsGridView", LocationsGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void ServerTypesGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AdminTab|ServerTypesGridView", ServerTypesGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
            //4/16/2015 NS added for VSPLUS-1432
            Response.Redirect("~/Security/LocationsEdit.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}