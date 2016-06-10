using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;

namespace VSWebUI.Security
{
    public partial class AssignServerAccess : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {

            this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
        }

        private void Master_ButtonClick(object sender, EventArgs e)
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
				
                if (!IsPostBack)
                {
                    //9/22/2015 NS added
                    FillGrid();
                    Session["visible"] = ""; Session["NotVisible"] = "";
                    FillUserNameComboBox();
                    //LocationsRoundPanel.Enabled = false;
                    LocToggleEnabled(false);
                    //SpecificServersRoundPanel.Enabled = true;
                    if (Session["UserPreferences"] != null)
                    {
                        DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                        foreach (DataRow dr in UserPreferences.Rows)
                        {
                            if (dr[1].ToString() == "ServerAccessGridView|ServerAccessGridView")
                            {
                                ServerAccessGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                            }
                        }
                    }
                }
                else
                {
                    //9/22/2015 NS added
                    FillGridFromSession();
                    if (Session["visible"] != "" && Session["visible"] != null)
                    {
                        SpecificServersVisibleGridView.DataSource = (DataTable)Session["visible"];
                        SpecificServersVisibleGridView.KeyFieldName = "ID";
                        SpecificServersVisibleGridView.DataBind();
                    }
                    if (Session["NotVisible"] != "" && Session["NotVisible"] != null)
                    {
						SpecificServersNotVisibleGridView.PageIndex = 0;
                        SpecificServersNotVisibleGridView.DataSource = (DataTable)Session["NotVisible"];
                        SpecificServersNotVisibleGridView.DataBind();
                    }
                }
				//1/22/2016 Durga modified for VSPLUS--2516
				if (Session["AssignSeverAcessUpdateStatus"] != null)
				{
					if (Session["AssignSeverAcessUpdateStatus"].ToString() != "")
					{
						
						Sucessdiv.InnerHtml = "Server Access information for <b>" + Session["AssignSeverAcessUpdateStatus"].ToString() +
							"</b> updated successfully." +
							"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
						Sucessdiv.Style.Value = "display: block";
						Session["AssignSeverAcessUpdateStatus"] = "";
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

        //9/22/2015 NS added for VSPLUS-2170
        private void FillGrid()
        {
            DataTable UsersDataTable = VSWebBL.SecurityBL.UsersBL.Ins.GetUserAccessData();
            ServerAccessGridView.DataSource = UsersDataTable;
            ServerAccessGridView.DataBind();
            Session["ServerAccessGrid"] = UsersDataTable;
        }

        private void FillGridFromSession()
        {
            if (Session["ServerAccessGrid"] != null & Session["ServerAccessGrid"] != "")
            {
                ServerAccessGridView.DataSource = (DataTable)Session["ServerAccessGrid"];
                ServerAccessGridView.DataBind();
            }
        }

        private void FillUserNameComboBox()
        {
            DataTable UsersDataTable = VSWebBL.SecurityBL.UsersBL.Ins.GetAllData();
            UserNameComboBox.DataSource = UsersDataTable;
            UserNameComboBox.TextField = "FullName";
            UserNameComboBox.ValueField = "FullName";
            UserNameComboBox.DataBind();
        }

        private void LocToggleEnabled(bool IsEnabled)
        {
            LocVisibleLabel.Enabled = IsEnabled;
            LocNotVisibleLabel.Enabled = IsEnabled;
            LocVisibleListBox.Enabled = IsEnabled;
            LocNotVisibleListBox.Enabled = IsEnabled;
            LocMoveVisibleButton.Enabled = IsEnabled;
            LocMoveNotVisibleButton.Enabled = IsEnabled;
        }

        protected void LocRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selItemValue = LocRadioButtonList.SelectedItem.Value.ToString();
            if (selItemValue == "0")
            {
                LocToggleEnabled(false);
                //SpecificServersRoundPanel.Enabled = true;
            }
            else
            {
                LocToggleEnabled(true);
                //SpecificServersRoundPanel.Enabled = true;
            }
            //9/21/2015 NS modified
            FillServerLocation();
            //FillServersVisibleGrid();
        }

        protected void LocMoveVisibleButton_Click(object sender, EventArgs e)
        {
            try
            {
                ListBoxRebuild(true, LocNotVisibleListBox, LocVisibleListBox, SpecificServersNotVisibleGridView, SpecificServersVisibleGridView);
                //SpecificServersRoundPanel.Enabled = true;
                FillServersVisibleGrid();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void LocMoveNotVisibleButton_Click(object sender, EventArgs e)
        {
            try
            {

                ListBoxRebuild(true, LocVisibleListBox, LocNotVisibleListBox, SpecificServersVisibleGridView, SpecificServersNotVisibleGridView);
                //SpecificServersRoundPanel.Enabled = true;
                FillServersVisibleGrid();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(typeof(Page), "Select Location", "<script>alert('Select Location')<script>");
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;

            }

        }

        protected void FillServersVisibleGrid()
        {
            string lname = "";
            DataTable dtSelLocFillGrid = new DataTable();
            for (int i = 0; i < LocVisibleListBox.Items.Count; i++)// string lname in LocVisibleListBox.Items) //.SelectedValues)
            {
                lname += "'" + LocVisibleListBox.Items[i].Value.ToString() + "',";
            }
            if (lname != "")
            {
                string LocName = lname.Substring(0, lname.Length - 1);
                dtSelLocFillGrid = VSWebBL.SecurityBL.AdminTabBL.Ins.GetServersByLocations(LocName);
            }
            Session["locationnotdt"] = (DataTable)dtSelLocFillGrid;
            Session["visible"] = dtSelLocFillGrid;
            //Delete Visible repeating rows of NotVisible list
            if (Session["NotVisible"] != null && Session["NotVisible"] != "")
            {
                DataTable dtNotVisible = (DataTable)Session["NotVisible"];
                if (dtNotVisible.Rows.Count > 0)
                {
                    foreach (DataRow r1 in dtSelLocFillGrid.Rows.Cast<DataRow>().ToArray()) // save rows to array
                    {
                        foreach (DataRow r2 in dtNotVisible.Rows)
                        {
                            try
                            {
                                if (r1.Field<String>("ServerName") == r2.Field<String>("ServerName"))
                                {
                                    r1.Delete();
                                    break; // break inner loop
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            Session["visible"] = dtSelLocFillGrid;
			SpecificServersVisibleGridView.PageIndex = 0;
            SpecificServersVisibleGridView.DataSource = dtSelLocFillGrid;
            SpecificServersVisibleGridView.DataBind();

            if (Session["NotVisible"] != null && Session["NotVisible"] != "")
            {
                DataTable dtNotVisible = (DataTable)Session["NotVisible"];
                if (dtNotVisible.Rows.Count > 0)
                {
                    for (int i = 0; i < LocNotVisibleListBox.Items.Count; i++)
                    {
                        foreach (DataRow r2 in dtNotVisible.Rows)
                        {
                            try
                            {
                                if (LocNotVisibleListBox.Items[i].Value.ToString() == r2.Field<String>("Location"))
                                {
                                    r2.Delete();
                                    // break; // break inner loop
                                }
                            }
                            catch { }
                        }
                    }
                }

                Session["NotVisible"] = dtNotVisible;
				SpecificServersNotVisibleGridView.PageIndex = 0;
                SpecificServersNotVisibleGridView.DataSource = dtNotVisible;
                SpecificServersNotVisibleGridView.DataBind();
            }
        }

        private void ListBoxRebuild(bool isLocation, object selListBoxFrom, object selListBoxTo, object fromGridView, object toGridView)
        {
            ASPxListBox listboxFrom = (ASPxListBox)selListBoxFrom;
            ASPxListBox listboxTo = (ASPxListBox)selListBoxTo;
            ASPxGridView fromGrid = (ASPxGridView)fromGridView;
            ASPxGridView toGrid = (ASPxGridView)toGridView;
            List<string> fieldValuesSel = new List<string>();
            listboxTo.Items.AddRange(listboxFrom.SelectedItems);
            for (int i = 0; i < listboxFrom.SelectedItems.Count; i++)
            {
                if (listboxFrom.SelectedItems[i] != null)
                {
                    fieldValuesSel.Add(listboxFrom.SelectedItems[i].ToString());
                }
            }
            while (listboxFrom.SelectedItems.Count > 0)
            {
                listboxFrom.Items.Remove(listboxFrom.SelectedItems[0]);
            }
            //SpecificServersUpdateGrids(fieldValuesSel, isLocation, fromGrid, toGrid);
        }

        protected void LocVisibleListBox_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string selValues;
                selValues = "";
                try
                {
                    DataTable LocationsDataTable = LocationsUpdateListBox(selValues);
                    if (LocationsDataTable.Rows.Count > 0)
                    {
                        LocVisibleListBox.DataSource = LocationsDataTable;
                        LocVisibleListBox.DataBind();
                    }
                    else
                    {
                        LocVisibleListBox.DataSource = null;
                        LocVisibleListBox.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }
                finally { }
            }
        }

        private void LoadUserRestrictions(string UserName)
        {
            try
            {
                //Load locations users are allowed to access
                DataTable LocationVisibleDataTable = new DataTable();
                LocationVisibleDataTable = VSWebBL.SecurityBL.AdminTabBL.Ins.UserLocationVisibleUpdateListBox(UserName);
                if (LocationVisibleDataTable.Rows.Count >= 0)
                {
                    LocVisibleListBox.DataSource = LocationVisibleDataTable;
                    LocVisibleListBox.DataBind();
                }
                //Load locations users are NOT allowed to access
                DataTable LocationNotVisibleDataTable = new DataTable();
                LocationNotVisibleDataTable = VSWebBL.SecurityBL.AdminTabBL.Ins.UserLocationNotVisibleUpdateListBox(UserName);
                if (LocationNotVisibleDataTable.Rows.Count > 0)
                {
                    //SpecificServersRoundPanel.Enabled = true;
                    //LocRadioButtonList.SelectedIndex = 1;
                    LocToggleEnabled(true);
                    LocNotVisibleListBox.DataSource = LocationNotVisibleDataTable;
                    LocNotVisibleListBox.DataBind();
                }
                else
                {
                    //Load all allowed locations
                    LocationVisibleDataTable = VSWebBL.SecurityBL.AdminTabBL.Ins.LocVisibleUpdateListBox();
                    if (LocationVisibleDataTable.Rows.Count >= 0)
                    {
                        //SpecificServersRoundPanel.Enabled = true;
                        //LocRadioButtonList.SelectedIndex = 0;
                        LocToggleEnabled(false);
                        LocVisibleListBox.DataSource = LocationVisibleDataTable;
                        LocVisibleListBox.DataBind();
                    }
                }


                DataTable ServerVisibleDataTable = new DataTable();
                ServerVisibleDataTable = VSWebBL.SecurityBL.AdminTabBL.Ins.ServersVisibleUpdateGrid(UserName);
                if (ServerVisibleDataTable.Rows.Count >= 0)
                {
                    SpecificServersVisibleGridView.DataSource = ServerVisibleDataTable;
                    SpecificServersVisibleGridView.DataBind();
                }
                DataTable ServerNotVisibleDataTable = new DataTable();
                ServerNotVisibleDataTable = VSWebBL.SecurityBL.AdminTabBL.Ins.ServersNotVisibleUpdateGrid(UserName);
                if (ServerNotVisibleDataTable.Rows.Count > 0 || LocationNotVisibleDataTable.Rows.Count > 0)
                {
                    LocRadioButtonList.SelectedIndex = 1;
                }
                else
                {
                    LocRadioButtonList.SelectedIndex = 0;
                }
                SpecificServersNotVisibleGridView.DataSource = ServerNotVisibleDataTable;
                SpecificServersNotVisibleGridView.DataBind();
                //9/21/2015 NS added
                Session["NotVisible"] = ServerNotVisibleDataTable;
                FillServersVisibleGrid();
            }
            catch (Exception ex)
            { throw ex; }
            finally { }
        }


        private DataTable LocationsUpdateListBox(string ServerType)
        {
            DataTable LocationsDataTable = new DataTable();
            try
            {
                LocationsDataTable = VSWebBL.SecurityBL.AdminTabBL.Ins.LocVisibleUpdateListBox();
            }
            catch (Exception ex)
            { throw ex; }
            finally { }
            return LocationsDataTable;
        }

        protected void SpecificServersMoveVisibleButton_Click(object sender, EventArgs e)
        {
            SpecificServersRebuildGrids(SpecificServersNotVisibleGridView, SpecificServersVisibleGridView, "NotVisible", "visible");

        }

        protected void SpecificServersMoveNotVisibleButton_Click(object sender, EventArgs e)
        {
            SpecificServersRebuildGrids(SpecificServersVisibleGridView, SpecificServersNotVisibleGridView, "visible", "NotVisible");

        }
        //private void SpecificServersRebuildGrids(object fromGridVal, object toGridVal)
        //{
        //    ASPxGridView fromGrid = (ASPxGridView)fromGridVal;
        //    ASPxGridView toGrid = (ASPxGridView)toGridVal;
        //    DataTable dataFrom = new DataTable();
        //    DataTable dataTo = new DataTable();
        //    dataTo.Columns.Add("ID");
        //    dataTo.Columns.Add("ServerName");
        //    dataTo.Columns.Add("Location");
        //    dataTo.Columns.Add("ServerType");
        //    if (Session["visible"] != null && Session["visible"] != "")
        //    {
        //        dataFrom = (DataTable)Session["visible"];
        //    }
        //    //else
        //    //{
        //    //    dataFrom = (DataTable)Session["ServerVisibleDataGrid"];
        //    //}
        //    List<int> Id = new List<int>();
        //    List<Object> SelectItemServerID = fromGrid.GetSelectedFieldValues("ID");
        //    foreach (object SelectItemID in SelectItemServerID)
        //    {
        //        foreach (DataRow Dr in dataFrom.Rows)
        //        {
        //            if (Dr["ID"].ToString() == SelectItemID.ToString())
        //            {

        //                Id.Add(dataFrom.Rows.IndexOf(Dr));
        //                dataTo.ImportRow(Dr);
        //                //dataFrom.Rows.Remove(Dr);

        //            }
        //            dataTo.AcceptChanges();

        //        }

        //    }
        //    foreach (int DrID in Id)
        //    {
        //        dataFrom.Rows[DrID].Delete();
        //    }

        //    dataFrom.AcceptChanges();

        //    DataTable NotVisible = new DataTable();
        //    NotVisible.Columns.Add("ID");
        //    NotVisible.Columns.Add("ServerName");
        //    NotVisible.Columns.Add("Location");
        //    NotVisible.Columns.Add("ServerType");
        //    if (Session["NotVisible"] != null && Session["NotVisible"] != "")
        //    {
        //        NotVisible = (DataTable)Session["NotVisible"];
        //        foreach (DataRow dr in dataTo.Rows)
        //        {
        //            NotVisible.ImportRow(dr);
        //        }

        //    }
        //    else
        //    {
        //        Session["NotVisible"] = dataTo;
        //    }
        //    fromGrid.DataSource = dataFrom;
        //    fromGrid.KeyFieldName = "ID";
        //    fromGrid.DataBind();
        //    fromGrid.Selection.UnselectAll();
        //    Session["visible"] = dataFrom;
        //    if (NotVisible.Rows.Count == 0)
        //    {
        //        toGrid.DataSource = (DataTable)Session["NotVisible"];
        //    }
        //    else
        //    {
        //        toGrid.DataSource = NotVisible;
        //    }
        //    toGrid.KeyFieldName = "ID";
        //    toGrid.DataBind();
        //}

        private void SpecificServersRebuildGrids(object fromGridVal, object toGridVal, string fromSession, string toSession)
        {
            ASPxGridView fromGrid = (ASPxGridView)fromGridVal;
            ASPxGridView toGrid = (ASPxGridView)toGridVal;
            DataTable dataFrom = new DataTable();
            DataTable dataTo = new DataTable();
            dataTo.Columns.Add("ID");
            dataTo.Columns.Add("ServerName");
            dataTo.Columns.Add("Location");
            dataTo.Columns.Add("ServerType");
            if (Session[fromSession] != null && Session[fromSession] != "")
            {
                dataFrom = (DataTable)Session[fromSession];
            }

            //else
            //{
            //    dataFrom = (DataTable)Session["ServerVisibleDataGrid"];
            //}
            List<int> Id = new List<int>();
            List<Object> SelectItemServerID = fromGrid.GetSelectedFieldValues("ID");
            foreach (object SelectItemID in SelectItemServerID)
            {
                foreach (DataRow Dr in dataFrom.Rows)
                {
                    try
                    {
                        if (Dr["ID"].ToString() == SelectItemID.ToString())
                        {

                            Id.Add(dataFrom.Rows.IndexOf(Dr));
                            dataTo.ImportRow(Dr);
                            //dataFrom.Rows.Remove(Dr);

                        }
                        dataTo.AcceptChanges();
                    }
                    catch { }
                }

            }
            foreach (int DrID in Id)
            {
                dataFrom.Rows[DrID].Delete();
            }

            dataFrom.AcceptChanges();

            DataTable NotVisible = new DataTable();
            NotVisible.Columns.Add("ID");
            NotVisible.Columns.Add("ServerName");
            NotVisible.Columns.Add("Location");
            NotVisible.Columns.Add("ServerType");
            if (Session[toSession] != null && Session[toSession] != "")
            {
                NotVisible = (DataTable)Session[toSession];
                foreach (DataRow dr in dataTo.Rows)
                {
                    NotVisible.ImportRow(dr);
                }

            }
            else
            {
                Session[toSession] = dataTo;
            }
            fromGrid.DataSource = dataFrom;
            fromGrid.KeyFieldName = "ID";
            fromGrid.DataBind();
            fromGrid.Selection.UnselectAll();
            Session[fromSession] = dataFrom;
            if (NotVisible.Rows.Count == 0)
            {
                toGrid.DataSource = (DataTable)Session[toSession];
            }
            else
            {
                toGrid.DataSource = NotVisible;
            }
            toGrid.KeyFieldName = "ID";
            toGrid.DataBind();
            //9/21/2015 NS added
            Session[toSession] = NotVisible;
        }


        protected void UserNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selValue;
            selValue = "";
            LocVisibleListBox.Items.Clear();
            LocNotVisibleListBox.Items.Clear();

            for (int i = 0; i < UserNameComboBox.Items.Count; i++)
            {
                if (UserNameComboBox.Items[i].Selected)
                {
                    Session["visible"] = "";
                    selValue = UserNameComboBox.Items[i].Value.ToString();
                    //LocationsRoundPanel.Enabled = true;
                    //SpecificServersRoundPanel.Enabled = true;
                    LoadUserRestrictions(selValue);

                }
            }
        }
        protected void SpecificServersVisibleGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AssignServerAccess|SpecificServersVisibleGridView", SpecificServersVisibleGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void SpecificServersNotVisibleGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AssignServerAccess|SpecificServersNotVisibleGridView", SpecificServersNotVisibleGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        protected void OKButton_Click(object sender, EventArgs e)
        {
            ServerAccessPopupControl.ShowOnPageLoad = false;
        }
        protected void ResetServerAccessButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    LocToggleEnabled(false);
                    //Server Type panel fields and buttons
                    //ServerTypesRoundPanel.Enabled = false;
                    //ServerTypeToggleEnabled(false);
                    LocRadioButtonList.SelectedIndex = 0;
                    //ServerTypeRadioButtonList.SelectedIndex = 0;
                    LocNotVisibleListBox.Items.Clear();
                    // LocVisibleListBox.Items.Clear();
                    //ServerTypeNotVisibleListBox.Items.Clear();
                    SpecificServersNotVisibleGridView.DataSource = null;
                    SpecificServersNotVisibleGridView.DataBind();
                    //Specific Servers
                    //SpecificServersRoundPanel.Enabled = false;
                    DataTable ServerAccessGrid = VSWebBL.SecurityBL.AdminTabBL.Ins.ServerAllowUpdateGrid("");
                    if (ServerAccessGrid.Rows.Count > 0)
                    {
                        SpecificServersVisibleGridView.DataSource = ServerAccessGrid;
                        SpecificServersVisibleGridView.KeyFieldName = "ID";
                        SpecificServersVisibleGridView.DataBind();
                    }
                    else
                    {
                        SpecificServersVisibleGridView.DataSource = null;
                        SpecificServersVisibleGridView.KeyFieldName = "ID";
                        SpecificServersVisibleGridView.DataBind();
                    }
                    DataTable LocationVisibleDataTable = VSWebBL.SecurityBL.AdminTabBL.Ins.LocVisibleUpdateListBox();
                    if (LocationVisibleDataTable.Rows.Count > 0)
                    {
                        //SpecificServersRoundPanel.Enabled = true;
                        LocRadioButtonList.SelectedIndex = 0;
                        LocToggleEnabled(false);
                        LocVisibleListBox.DataSource = LocationVisibleDataTable;
                        LocVisibleListBox.DataBind();
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
        protected void AssignServerAccessButton_Click(object sender, EventArgs e)
        {
            int flag = 0;
            try
            {
                string Uname = UserNameComboBox.Text;
                bool returnloc, returnserver;

                if (SpecificServersNotVisibleGridView.VisibleRowCount == 0)
                {
                    returnloc = VSWebBL.SecurityBL.AdminTabBL.Ins.InsertRestricted_Locations(Uname, GetRestricted_Locations());
                    returnserver = VSWebBL.SecurityBL.AdminTabBL.Ins.InsertRestricted_Servers(Uname, GetRestrictedServersFromGrid());
                }
                else
                {
                    List<string> loc = new List<string>();
                    List<string> locName = new List<string>();
                    for (int i = 0; i < LocNotVisibleListBox.Items.Count; i++)
                    {
                        loc.Add(LocNotVisibleListBox.Items[i].Text);
                    }

                    DataTable dt = new DataTable();
                    if (Session["NotVisible"] != "")
                        dt = (DataTable)Session["NotVisible"];

                    foreach (string locnotvis in loc)
                    {
                        int cnt = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["location"].ToString() != locnotvis)
                            {
                                cnt++;
                            }
                        }
                        if (cnt == dt.Rows.Count)
                        {
                            locName.Add(locnotvis);
                        }
                        //foreach (DataRow dr in dt.Rows)
                        //{

                        //    if (dr["Location"].ToString() != locnotvis)
                        //    {
                        //        locName.Add(locnotvis);
                        //        //returnserver = VSWebBL.SecurityBL.AdminTabBL.Ins.InsertRestricted_Servers(Uname, GetRestrictedServersFromGrid());
                        //    }

                        //}

                    }
                    returnserver = VSWebBL.SecurityBL.AdminTabBL.Ins.InsertRestricted_Servers(Uname, GetRestrictedServersFromGrid());
                    if (locName.Count > 0)
                    {
                        string name = "";
                        foreach (string locname in locName)
                        {

                            if (name == "")
                            {
                                int i = 0;
                                returnloc = VSWebBL.SecurityBL.AdminTabBL.Ins.InsertRestricted_Location(Uname, locname, i);
                                name = locname;
                            }
                            else
                            {
                                if (name != locname)
                                {
                                    int i = 1;
                                    returnloc = VSWebBL.SecurityBL.AdminTabBL.Ins.InsertRestricted_Location(Uname, locname, i);
                                    name = locname;
                                }
                            }


                        }
                        //returnloc = VSWebBL.SecurityBL.AdminTabBL.Ins.InsertRestricted_Locations(Uname);
                    }
                    else
                    {
                        returnloc = VSWebBL.SecurityBL.AdminTabBL.Ins.InsertRestricted_Location(Uname, "", 0);
                    }


                }
                flag++;

            }
            catch (Exception ex)
            {
                //9/17/2015 NS added
                successDiv.Style.Value = "display: none";
                errorDiv.InnerHtml = "There was an issue updating user access: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

            if (flag > 0)
            {
                //9/17/2015 NS modified
                //ServerAccessPopupControl.ShowOnPageLoad = true;
                //MsgLabel.Text = "Server access assigned successfully.";
                successDiv.InnerHtml = "User access has been updated successfully." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                successDiv.Style.Value = "display: block";
                errorDiv.Style.Value = "display: none";
            }
            else
            {
                //9/17/2015 NS modified
                //ServerAccessPopupControl.ShowOnPageLoad = true;
                //MsgLabel.Text = "Server access not assigned.";
                successDiv.Style.Value = "display: none";
                errorDiv.InnerHtml = "There was an issue updating user access." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }

            // AssignServerTypeRestrictions(SpecificServersNotVisibleGridView, SpecificServersVisibleGridView, ServerTypeNotVisibleListBox,UserNameComboBox);
        }
        protected string GetRestricted_Locations()
        {

            string Locations = "";

            for (int i = 0; i < LocNotVisibleListBox.Items.Count; i++)
            {
                if (Locations == "")
                {
                    Locations = "'" + LocNotVisibleListBox.Items[i].Text + "'";


                }
                else
                {
                    Locations = Locations + ",'" + LocNotVisibleListBox.Items[i].Text + "'";
                }

            }
            return Locations;

        }
        private Dictionary<string, string> GetRestrictedServersFromGrid()
        {
            string separator = "###$$$###";
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i < SpecificServersNotVisibleGridView.VisibleRowCount; i++)
            {
                if (SpecificServersNotVisibleGridView.GetRowValues(i, "ServerName") != null)
                {
                    dictionary.Add("'" + SpecificServersNotVisibleGridView.GetRowValues(i, "ServerType").ToString() + "'" +
                    separator + "'" + SpecificServersNotVisibleGridView.GetRowValues(i, "ServerName").ToString() + "'", 
                    "'" + SpecificServersNotVisibleGridView.GetRowValues(i, "ServerName").ToString() + "'");
                }
            }
            /*
            string selValues = "";
            for (int i = 0; i < SpecificServersNotVisibleGridView.VisibleRowCount; i++)
            {
                if (SpecificServersNotVisibleGridView.GetRowValues(i, "ServerName") != null)
                {
                    if (selValues == "")
                    {
                        selValues = "'" + SpecificServersNotVisibleGridView.GetRowValues(i, "ServerName").ToString() + "'";
                    }
                    else
                    {
                        selValues += "," + "'" + SpecificServersNotVisibleGridView.GetRowValues(i, "ServerName").ToString() + "'";
                    }
                }
            }
            */
            return dictionary;
        }

        //9/17/2015 NS added
        protected void UpdatePanel_Unload(object sender, EventArgs e)
        {
            System.Reflection.MethodInfo methodInfo = typeof(ScriptManager).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Where(i => i.Name.Equals("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel")).First();
            methodInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { sender as UpdatePanel });
        }

        protected void FillServerLocation()
        {
            if (LocRadioButtonList.SelectedIndex == 0)
            {
                LocNotVisibleListBox.DataSource = "";
                LocNotVisibleListBox.DataBind();
                
            }
            DataTable dt = VSWebBL.SecurityBL.AdminTabBL.Ins.LocVisibleUpdateListBox();
            LocVisibleListBox.DataSource = dt;
            LocVisibleListBox.DataBind();
            SpecificServersNotVisibleGridView.DataSource = "";
            SpecificServersNotVisibleGridView.DataBind();
            dt = VSWebBL.SecurityBL.AdminTabBL.Ins.ServerAllowUpdateGrid("");
            SpecificServersVisibleGridView.DataSource = dt;
            SpecificServersVisibleGridView.DataBind();
            Session["visible"] = dt;
            Session["NotVisible"] = "";
        }

        protected void ServerAccessGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            {
                try
                {
                    if (e.GetValue("ID") != " " && e.GetValue("ID") != null)
                    {
                        ASPxWebControl.RedirectOnCallback("AssignServerAccess_Edit.aspx?UserID=" + e.GetValue("ID") + "&FullName=" + e.GetValue("FullName"));
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                }
            }
        }

		protected void SpecificServersVisibleGridView_PageIndexChanged(object sender, EventArgs e)
		{
			if (Session["visible"] != "" && Session["visible"] != null)
			{
				int pageIndex = (sender as ASPxGridView).PageIndex;

				SpecificServersVisibleGridView.PageIndex = pageIndex;
				//this.SpecificServersVisibleGridView.PageIndex = SpecificServersVisibleGridView.PageIndex;
				SpecificServersVisibleGridView.DataSource = (DataTable)Session["visible"];
				SpecificServersVisibleGridView.DataBind();
			}
		}

		protected void SpecificServersNotVisibleGridView_PageIndexChanged(object sender, EventArgs e)
		{
			
				if (Session["NotVisible"] != "" && Session["NotVisible"] != null)
			{
				int pageIndex = (sender as ASPxGridView).PageIndex;

				SpecificServersNotVisibleGridView.PageIndex = pageIndex;
				//this.SpecificServersVisibleGridView.PageIndex = SpecificServersVisibleGridView.PageIndex;
				SpecificServersNotVisibleGridView.DataSource = (DataTable)Session["NotVisible"];
				SpecificServersNotVisibleGridView.DataBind();
			}
		}

        protected void ServerAccessGridView_PageSizeChanged(object sender, EventArgs e)
        {
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("ServerAccessGridView|ServerAccessGridView", ServerAccessGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }

}
