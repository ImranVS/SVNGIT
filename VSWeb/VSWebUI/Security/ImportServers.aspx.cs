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

namespace VSWebUI.Security
{
    public partial class ImportServers : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        string filename = null;
        OleDbConnection oledbcon = new OleDbConnection();

        protected void Page_Load(object sender, EventArgs e)
        {

            string ServerName;

            if (!IsPostBack)
            {
				FillprofileComboBox();

                ServerName = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Primary Server");
                if (ServerName != "")
                {
                    DomServerTextBox.Text = ServerName;
                }
            }

        }


        public void LoadDominoServers()
        {
            Domino.NotesDbDirectory dir;
            Domino.NotesDatabase db;
            Domino.NotesView view;
            Domino.NotesDocument doc;
            Domino.NotesName sname;
            Domino.NotesItem item;
            Domino.NotesItem item2;
            byte[] MyPass;
            string MyDominoPassword; //should be string
            string MyObjPwd;
            string[] MyObjPwdArr;
            //1/8/2014 NS added
            DataTable importedDT;
            //12/24/2013 NS added - store the value of the Domino Server field in the Settings table Primary Server column
            bool updatedsrv = false;
            updatedsrv = VSWebBL.SettingBL.SettingsBL.Ins.UpdateSvalue("Primary Server", DomServerTextBox.Text, VSWeb.Constants.Constants.SysString);
            //object MyObjPwd;
            //12/20/2013 NS added
            errorDiv.Style.Value = "display: none;";
            //8/12/2014 NS added for VSPLUS-861
            errorinfoDiv.Style.Value = "display: none";
            DataTable LocationsDataTable = new DataTable();
            LocationsDataTable = VSWebBL.SecurityBL.LocationsBL.Ins.GetAllData();
            if (LocationsDataTable.Rows.Count > 0)
            {
                LocComboBox.DataSource = LocationsDataTable;
                LocComboBox.TextField = "Location";
                LocComboBox.ValueField = "Location";
                LocComboBox.DataBind();
                LocIDComboBox.DataSource = LocationsDataTable;
                LocIDComboBox.TextField = "ID";
                LocIDComboBox.ValueField = "ID";
                LocIDComboBox.DataBind();
                VSFramework.TripleDES mySecrets = new VSFramework.TripleDES();
                try
                {
                    MyObjPwd = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Password");
                    //3/25/2014 NS modified for VSPLUS-494
                    if (MyObjPwd != "")
                    {
                        MyObjPwdArr = MyObjPwd.Split(',');
                        MyPass = new byte[MyObjPwdArr.Length];
                        for (int i = 0; i < MyObjPwdArr.Length; i++)
                        {
                            MyPass[i] = Byte.Parse(MyObjPwdArr[i]);
                        }
                    }
                    else
                    {
                        //10/6/2014 NS modified for VSPLUS-990
                        errorDiv.InnerHtml = "The following error has occurred: Notes password may not be empty. Please update the password under Stored Passwords & Options\\IBM Domino Settings." +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorDiv.Style.Value = "display: block";
                        MyPass = null;
                    }
                }
                catch (Exception ex)
                {
                    //12/20/2013 NS added
                    //10/6/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                    //8/12/2014 NS added for VSPLUS-861
                    errorinfoDiv.Style.Value = "display: block";
                    MyPass = null;
                    //5/15/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }

                try
                {
                    if (MyPass != null)
                    {
                        MyDominoPassword = mySecrets.Decrypt(MyPass);
                    }
                    else
                    {
                        MyDominoPassword = null;
                    }
                }
                catch (Exception ex)
                {
                    //12/20/2013 NS added
                    //10/6/2014 NS modified for VSPLUS-990
                    errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                        "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    errorDiv.Style.Value = "display: block";
                    //8/12/2014 NS added for VSPLUS-861
                    errorinfoDiv.Style.Value = "display: block";
                    MyDominoPassword = "";
                    //5/15/2014 NS added for VSPLUS-634
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }
                //3/25/2014 NS modified for VSPLUS-494
                if (MyDominoPassword != null)
                {
                    try
                    {
                        Domino.NotesSession NotesSessionObject = new Domino.NotesSession();
                        NotesSessionObject.Initialize(MyDominoPassword);
                        dir = NotesSessionObject.GetDbDirectory(DomServerTextBox.Text);
                        //db = dir.GetFirstDatabase(Domino.DB_TYPES.NOTES_DATABASE);
                        db = dir.OpenDatabase("names.nsf");
                        view = db.GetView("($Servers)");
                        doc = view.GetFirstDocument();
                        DataTable dt = new DataTable();
                        DataRow dr = dt.NewRow();
                        //1/8/2014 NS added
                        DataRow[] foundRows;
                        dt.Columns.Add("ServerName", typeof(string));
                        dt.Columns.Add("IPAddress", typeof(string));
                        //1/8/2014 NS added - get a list of all servers already imported. Display only the ones that have not yet been imported.
                        importedDT = VSWebBL.ConfiguratorBL.AlertsBL.Ins.GetServer();
                        while (doc != null)
                        {
                            item = doc.GetFirstItem("ServerName");
                            sname = NotesSessionObject.CreateName(item.Text);
                            dr["ServerName"] = sname.Abbreviated;
                            //1/8/2014 NS added
                            foundRows = importedDT.Select("ServerName = '" + sname.Abbreviated + "'");
                            if (foundRows.Length == 0)
                            {
                                //5/16/2013 NS modified
                                //item = doc.GetFirstItem("NetAddresses");
                                item = doc.GetFirstItem("SMTPFullHostDomain");
                                dr["IPAddress"] = item.Text;
                                //2/5/2016 NS modified for VSPLUS-2068
                                if (item == null || item.Text == null || item.Text == "")
                                {
                                    //1/26/2016 NS modified for VSPLUS-2068
                                    item2 = doc.GetFirstItem("NetAddresses");
                                    dr["IPAddress"] = item2.Text;
                                    if (item2 == null || item2.Text == null || item2.Text == "")
                                    {
                                        dr["IPAddress"] = "dummyaddress.yourdomain.com";
                                    }
                                }
                                dt.Rows.Add(dr);
                                dr = dt.NewRow();
                            }
                            doc = view.GetNextDocument(doc);
                        }
                        //1/8/2014 NS modified
                        if (dt.Rows.Count > 0)
                        {
                            //1/8/2014 NS added
                            infoDiv.Style.Value = "display: block";
                            SrvCheckBoxList.DataSource = dt;
                            SrvCheckBoxList.TextField = "ServerName";
                            SrvCheckBoxList.ValueField = "ServerName";
                            SrvCheckBoxList.DataBind();
                            IPCheckBoxList.DataSource = dt;
                            IPCheckBoxList.TextField = "IPAddress";
                            IPCheckBoxList.ValueField = "IPAddress";
                            IPCheckBoxList.DataBind();
                            //12/20/2013 NS added
                            ASPxRoundPanel1.Visible = true;
                        }
                        else
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "There are no new servers in the address book that have not already been imported into VitalSigns." +
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            errorDiv.Style.Value = "display: block";
                        }
                    }
                    catch (Exception ex)
                    {
                        //12/20/2013 NS added
                        //10/6/2014 NS modified for VSPLUS-990
                        errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                        errorDiv.Style.Value = "display: block";
                        //8/12/2014 NS added for VSPLUS-861
                        errorinfoDiv.Style.Value = "display: block";
                        db = null;
                        dir = null;
                        view = null;
                        doc = null;
                        //5/15/2014 NS added for VSPLUS-634
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    }
                }
            }
            //5/13/2014 NS added for VSPLUS-183
            else
            {
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "All imported servers must be assigned to a location. There were no locations found. Please create at least one location entry using the 'Setup & Security - Maintain Server Locations' menu option." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }
        }

        protected void ImportButton_Click(object sender, EventArgs e)
        {
            //1. Check whether selected servers have already been imported
            //2. If the servers are not in the table, import server and location info
            string profilename = "";
            Object ReturnValue;
            Servers ServersObject;
            DataTable dtsrv = new DataTable();
            if (LocComboBox.SelectedIndex != -1)
            {
                if (LocComboBox.SelectedItem.Value.ToString() != "")
                {
                    if (SrvCheckBoxList.SelectedItems.Count > 0)
                    {
                        dtsrv.Columns.Add("ID");
                        dtsrv.Columns.Add("ServerName");
                        dtsrv.Columns.Add("IPAddress");
                        dtsrv.Columns.Add("Description");
                        dtsrv.Columns.Add("ServerType");
                        dtsrv.Columns.Add("Location");
                        dtsrv.Columns.Add("LocationID");
						dtsrv.Columns.Add("ProfileName");
                        //5/16/2013 NS modified
                        //for (int i = 0; i < SrvCheckBoxList.SelectedItems.Count; i++)
                        for (int i = 0; i < SrvCheckBoxList.Items.Count; i++)
                        {
                            if (SrvCheckBoxList.Items[i].Selected)
                            {
                                DataRow dr = dtsrv.NewRow();
                                dr["ID"] = "";
                                dr["ServerName"] = SrvCheckBoxList.Items[i].ToString();
                                dr["IPAddress"] = IPCheckBoxList.Items[i].Text;
                                if (IPCheckBoxList.Items[i].Text == "")
                                {
                                    dr["IPAddress"] = "dummyaddress.yourdomain.com";
                                }
                                dr["Description"] = "Production";
                                dr["ServerType"] = "Domino";
                                dr["Location"] = LocComboBox.SelectedItem.Value.ToString();
                                dr["LocationID"] = LocIDComboBox.Items[LocComboBox.SelectedIndex].Text;
								dr["ProfileName"] = ProfileComboBox.SelectedItem.Value.ToString();
                                profilename = ProfileComboBox.Items[ProfileComboBox.SelectedIndex].Text;
                                dtsrv.Rows.Add(dr);
                                ServersObject = CollectDataForServers("Insert", dr);
                                DataTable dt = VSWebBL.SecurityBL.ServersBL.Ins.GetDataByName(ServersObject);
                                if (dt.Rows.Count == 0)
                                {
                                    ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.InsertData(ServersObject);
                                }
                                else
                                {
                                    dt.Rows[0]["LocationID"] = LocIDComboBox.Items[LocComboBox.SelectedIndex].Text;
                                    dt.Rows[0]["Location"] = LocComboBox.SelectedItem.Value.ToString();
                                    ServersObject = CollectDataForServers("Update", dt.Rows[0]);
                                    ReturnValue = VSWebBL.SecurityBL.ServersBL.Ins.UpdateData(ServersObject);
                                }
                            }
                        }
                        Session["ImportedServers"] = dtsrv;
                        Response.Redirect("~/Security/ImportServers2.aspx?Profilename="+profilename, false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
            }
        }

        private Servers CollectDataForServers(string Mode, DataRow ServersRow)
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
                ServersObject.Description = ServersRow["Description"].ToString();
                ServerTypes STypeobject = new ServerTypes();
                STypeobject.ServerType = ServersRow["ServerType"].ToString();
                ServersObject.LocationID = int.Parse(ServersRow["LocationID"].ToString());
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
				ProfileNames proobject = new ProfileNames();
				proobject.ProfileName = ServersRow["ProfileName"].ToString();
				ServersObject.ProfileName = proobject.ProfileName;

                return ServersObject;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void DoneButton_Click(object sender, EventArgs e)
        {
			string ProfileName = "";
			if(ProfileComboBox.SelectedIndex < 0)
			{
				ProfileName = "Default";
			}
			else
			{
				ProfileComboBox.SelectedItem.Text = ProfileComboBox.SelectedItem.Text;
			}
			Response.Redirect("~/Security/ImportServers2.aspx?ProfileNames=" + ProfileName, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void SelectAllButton_Click(object sender, EventArgs e)
        {
            SrvCheckBoxList.SelectAll();
        }

        protected void DeselectAllButton_Click(object sender, EventArgs e)
        {
            SrvCheckBoxList.UnselectAll();
        }

        protected void LoadServersButton_Click(object sender, EventArgs e)
        {
            LoadDominoServers();
            //12/20/2013 NS modified - make the panel visible if the server list is loaded
            //ASPxRoundPanel1.Visible = true;
        }

        //protected void ASPxButton1_Click(object sender, EventArgs e)
        //{
        //    GetCSVValue();

        //}
        protected void fileupld_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {

            try
            {
                e.CallbackData = SavePostedFile(e.UploadedFile);
                //10/10/2015 NS modified for VSPLUS-2002
                /*
                ASPxLabel7.ForeColor = System.Drawing.Color.Green;

                ASPxLabel7.Text = "File uploaded successfully.";
                successDiv.InnerHtml = "File uploaded successfully." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>"; ;
                successDiv.Style.Value = "display: block";
                errorDiv2.Style.Value = "display: none";
                 */
            }
            catch (Exception ex)
            {
                e.IsValid = false;
                e.ErrorText = ex.Message;
                //10/10/2015 NS added for VSPLUS-2002
                successDiv.Style.Value = "display: none";
                errorDiv2.InnerHtml = "The following error has occurred while loading the file: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv2.Style.Value = "display: block";
            } 
            //e.CallbackData = SavePostedFile(e.UploadedFile);
        

        }
        string SavePostedFile(UploadedFile uploadedFile)
        {
            string logPath = "";
            //Mukund VSPLUS-1035, Changed path to save CSV from Settings default to VSWeb- LogFiles
            //if (!uploadedFile.IsValid)
             //   logPath = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Log Files Path");
            //if (logPath == "")
            //{
            //    logPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
            //}
            fileupld.SaveAs(Server.MapPath("~/LogFiles/" + uploadedFile.FileName));
           // fileupld.SaveAs(logPath + uploadedFile.FileName);
            //string fileName1 = Path.Combine(MapPath("~/log_files/") + uploadedFile.FileName);
            string fileName1 = Server.MapPath("~/LogFiles/" + uploadedFile.FileName);// Path.Combine(logPath + uploadedFile.FileName);
            //string filename2 = Path.GetFileName(uploadedFile.FileName);
            string fileExtension = Path.GetExtension(uploadedFile.FileName);
            //uploadedFile.SaveAs(Server.MapPath("~/log_files/" + filename2));
            //using (Image original = Image.FromStream(uploadedFile.FileContent))
            //using (Image thumbnail = PhotoUtils.Inscribe(original, 100))
            //{
            //    PhotoUtils.SaveToJpeg(thumbnail, fileName);
            //}
            if (fileExtension == ".csv")
            {
                // oledbcon.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                oledbcon.ConnectionString = ("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + System.IO.Path.GetDirectoryName(filename) + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\"");


                string fileName = fileName1;
                DataTable dt = new DataTable();
                DataRow dr;
                DataColumn column;
                string s;
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "ServerName";
                dt.Columns.Add(column);
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "IPAddress";
                dt.Columns.Add(column);
                logPath = fileName;
                //ASPxLabel6.Text = "File uploaded successfully.";
                try
                {
                    DataTable LocationsDataTable = new DataTable();
                    LocationsDataTable = VSWebBL.SecurityBL.LocationsBL.Ins.GetAllData();
                    if (LocationsDataTable.Rows.Count > 0)
                    {
                        LocComboBox.DataSource = LocationsDataTable;
                        LocComboBox.TextField = "Location";
                        LocComboBox.ValueField = "Location";
                        LocComboBox.DataBind();
                        LocIDComboBox.DataSource = LocationsDataTable;
                        LocIDComboBox.TextField = "ID";
                        LocIDComboBox.ValueField = "ID";
                        LocIDComboBox.DataBind();
                    }
                    using (StreamReader sr = new StreamReader(logPath))
                    {
                        while (!sr.EndOfStream)
                        {
                            s = sr.ReadLine();
                            dr = dt.NewRow();
							//2/11/2016 Durga Added for VSPLUS 2432
							DataTable serversdt = VSWebBL.SecurityBL.ServersBL.Ins.GetServerDetailsByName(s.ToString());
							if (serversdt.Rows.Count == 0)
							{
								dr["IPAddress"] = "dummyaddress.yourdomain.com";
								dr["ServerName"] = s.ToString();
                            dt.Rows.Add(dr);
							}
                        }
						
                        if (dt.Rows.Count > 0)
                        {
                            infoDiv.Style.Value = "display: block";
                            SrvCheckBoxList.DataSource = dt;
                            SrvCheckBoxList.TextField = "ServerName";
                            SrvCheckBoxList.ValueField = "ServerName";
                            SrvCheckBoxList.DataBind();
                            IPCheckBoxList.DataSource = dt;
                            IPCheckBoxList.TextField = "IPAddress";
                            IPCheckBoxList.ValueField = "IPAddress";
                            IPCheckBoxList.DataBind();
                            //12/20/2013 NS added
                            ASPxRoundPanel1.Visible = true;
                        }
                        else
                        {
                            //10/6/2014 NS modified for VSPLUS-990
                            errorDiv.InnerHtml = "There are no new servers in the address book that have not already been imported into VitalSigns." +
                                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                            errorDiv.Style.Value = "display: block";
                        }
                        sr.Close();
                    }
                }
                catch (Exception ex)
                {
                    logPath = System.Web.HttpContext.Current.Server.MapPath("~");
                    logPath += "\\" + fileName;
                    try
                    {
                        using (StreamReader sr = new StreamReader(logPath))
                        {
                            while (!sr.EndOfStream)
                            {
                                s = sr.ReadLine();
                                dr = dt.NewRow();
                                dr["ServerName"] = s.ToString();
                                dr["IPAddress"] = "dummyaddress.yourdomain.com";
                                dt.Rows.Add(dr);
                            }
                            if (dt.Rows.Count > 0)
                            {
                                infoDiv.Style.Value = "display: block";
                                SrvCheckBoxList.DataSource = dt;
                                SrvCheckBoxList.TextField = "ServerName";
                                SrvCheckBoxList.ValueField = "ServerName";
                                SrvCheckBoxList.DataBind();
                                IPCheckBoxList.DataSource = dt;
                                IPCheckBoxList.TextField = "IPAddress";
                                IPCheckBoxList.ValueField = "IPAddress";
                                IPCheckBoxList.DataBind();
                                //12/20/2013 NS added
                                ASPxRoundPanel1.Visible = true;
                            }
                            else
                            {
                                //10/6/2014 NS modified for VSPLUS-990
                                errorDiv.InnerHtml = "There are no new servers in the address book that have not already been imported into VitalSigns." +
                                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                                errorDiv.Style.Value = "display: block";
                            }
                            sr.Close();
                        }
                    }
                    catch
                    {
                        //6/27/2014 NS added for VSPLUS-634
                        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    }
                }
               
            }

            return fileName1;
        }
        //protected void GetCSVValue()
        //{


        //    if (fileupld.HasFile)
        //    {
                
        //        DataTable EmpData = new DataTable();
        //        fileupld.SaveAs(Server.MapPath("~/LogFiles/" + fileupld.FileName));
        //        filename = Server.MapPath("LogFiles") + "\\" + fileupld.FileName;
        //        string filename2 = Path.GetFileName(fileupld.FileName);
        //        string fileExtension = Path.GetExtension(fileupld.FileName);
        
        //        else
        //        {
        //            //select file 
        //            ASPxLabel6.ForeColor = System.Drawing.Color.Green;

        //            ASPxLabel6.Text = "Please Select File.";
        //        }
        //    }

        //}
		private void FillprofileComboBox()
		{
			DataTable ProfileNamesDataTable = VSWebBL.SecurityBL.ProfilesNamesBL.Ins.GetAllData();
			ProfileComboBox.DataSource = ProfileNamesDataTable;
			ProfileComboBox.TextField = "ProfileName";
			ProfileComboBox.ValueField = "ID";
			ProfileComboBox.DataBind();
		}

    }
    }
