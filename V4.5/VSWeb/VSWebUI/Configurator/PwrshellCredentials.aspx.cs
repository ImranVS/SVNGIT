using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;
using DevExpress.Web;

using VSFramework;
using VSWebBL;
using VSWebDO;
namespace VSWebUI.Configurator
{
    public partial class PwrshellCredentials : System.Web.UI.Page
    {
        DataTable AliasDataTable = null;
        object msgloc = "";
        static int locID = 0;
       // string rawPass;

        protected void Page_Load(object sender, EventArgs e)
        {

            pnlAreaDtls.Style.Add("visibility", "hidden");
            //3/20/2014 NS added
            errorDiv.Style.Value = "display: none";
            if (!IsPostBack)
            {
                FillAliasGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "PwrshellCredentials|AliasGridView")
                        {
                            AliasGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
                if (cnpsw.Text == "" && cnfrmpassword.Text =="")
                {
                    FillAliasGridfromSession();
                    
                }


            }
			if (Session["ErrorStatus"] != null)
			{
				if (Session["ErrorStatus"].ToString() != "")
				{
					//10/3/2014 NS modified for VSPLUS-990
					errordiv1.InnerHtml = " <b>" + Session["ErrorStatus"].ToString() +
						"</b>"+
						"<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					errordiv1.Style.Value = "display: block";
					Session["ErrorStatus"] = "";
					
				}
			}

            //if (Session["CredentialsUpdateStatus"] != null)
            //{
            //    if (Session["CredentialsUpdateStatus"].ToString() != "")
            //    {

            //        successDiv.InnerHtml = "Credentials  information for <b>" + Session["CredentialsUpdateStatus"].ToString() +
            //            "</b> updated successfully." +
            //            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
            //        successDiv.Style.Value = "display: block";
            //        Session["CredentialsUpdateStatus"] = "";
            //    }
            //}
        }
        private void FillAliasGrid()
        {
            try
            {

                AliasDataTable = new DataTable();
                DataSet CredentialsDataSet = new DataSet();
                AliasDataTable = VSWebBL.ConfiguratorBL.CredentialsBL.Ins.GetAllData();
                //if (AliasDataTable.Rows.Count > 0)
                //{
                DataTable dtcopy = AliasDataTable.Copy();
                dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };

                Session["Alias"] = dtcopy;
                AliasGridView.DataSource = AliasDataTable;
                AliasGridView.DataBind();
                //}
                //else
                //{
                //    AliasGridView.DataSource = AliasDataTable;
                //    AliasGridView.DataBind();
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
        private void FillAliasGridfromSession()
        {
            try
            {

                AliasDataTable = new DataTable();
                if (Session["Alias"] != "" && Session["Alias"] != null)
                    AliasDataTable = (DataTable)Session["Alias"];
                if (AliasDataTable.Rows.Count > 0)
                {
                    AliasGridView.DataSource = AliasDataTable;
                    AliasGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void AliasGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            Credentials LocObject = new Credentials();
            LocObject.ID = Convert.ToInt32(e.Keys[0]);

            //Delete row from DB
            string ReturnValue = VSWebBL.ConfiguratorBL.CredentialsBL.Ins.DeleteData(LocObject);
			if (ReturnValue != "true")
			{
				if (ReturnValue.Contains("FK_DominoServers_Credentials"))
				{
					(Session["ErrorStatus"]) = "This Credentials are used elsewhere. Cannot delete.";

				}
				if (ReturnValue.Contains("FK_O365Server_Credentials"))
				{
					(Session["ErrorStatus"]) = "This Credentials are used elsewhere. Cannot delete.";
					
				}
				if (ReturnValue.Contains("FK_WebsphereCell_Credentials"))
				{
					(Session["ErrorStatus"]) = "This Credentials are used elsewhere. Cannot delete.";
					
				}

				if (ReturnValue.Contains("FK_SametimeServers_Credentials"))
				{
					(Session["ErrorStatus"]) = "This Credentials are used elsewhere. Cannot delete.";
					
				}

              DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/Configurator/PwrshellCredentials.aspx");
			}

            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillAliasGrid();
        }


        /// <summary>
        /// Check data before saving, for fields which cannot be validated through front end.
        /// VSPLUS-1504, Mukund 05/20/15
        /// </summary>
        /// <param name="AliasDataTable"></param>
        protected void ValidateData(DataTable dataTable, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            DataRow DRRow = null;
            DRRow = dataTable.NewRow();
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
            if (DRRow["Password"].ToString().Length < 5)
            {
                throw new Exception("The Password length should be more than 5 characters.");
            }
        }

        protected void AliasGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {

            AliasDataTable = (DataTable)Session["Alias"];
            //VSPLUS-1504, Mukund 05/20/15
            //ValidateData(AliasDataTable,e);

            ASPxGridView gridView = (ASPxGridView)sender;
            UpdateCredentialsData("Insert", GetRow(AliasDataTable, e.NewValues.GetEnumerator(), -1));
            //Update Grid after inserting new row, refresh grid as in page load
            gridView.CancelEdit();
            e.Cancel = true;



            //3/20/2014 NS modified
            //tdmsgforlocation.Text = msgloc.ToString();
            //10/6/2014 NS modified for VSPLUS-990
            errorDiv.InnerHtml = msgloc.ToString() +
                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
            errorDiv.Style.Value = "display: block";
            FillAliasGrid();

        }
        private void UpdateCredentialsData(string Mode, DataRow CredentialsRow)
        {
            if (Mode == "Insert")
            {
                Object ReturnValue = VSWebBL.ConfiguratorBL.CredentialsBL.Ins.InsertData(CollectDataForCredentials(Mode, CredentialsRow));
                if (ReturnValue == "")
                {
                    //3/20/2014 NS modified
                    msgloc = "The credentials you entered already exist.";
                }
            }
            if (Mode == "Update")
            {
                bool ReturnValue = VSWebBL.ConfiguratorBL.CredentialsBL.Ins.UpdateData(CollectDataForCredentials(Mode, CredentialsRow));
                if (ReturnValue == true)
                {
                    successDiv.InnerHtml = "Credentials  information for <b>" + Session["CredentialsUpdateStatus"].ToString() +
                            "</b> updated successfully." +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                    successDiv.Style.Value = "display: block";
                }
            }

        }
        protected DataRow GetRow(DataTable LocObject, IDictionaryEnumerator enumerator, int Keys)
        {
            DataTable dataTable = LocObject;
            DataRow DRRow = null;


            if (Keys == -1)
                DRRow = dataTable.NewRow();
            else
                DRRow = dataTable.Rows.Find(Keys);

            enumerator.Reset();
            while (enumerator.MoveNext())
                DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
            return DRRow;
        }

        private Credentials CollectDataForCredentials(string Mode, DataRow CredentialsRow)
        {
            VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
            try
            {
                Credentials CredentialsObject = new Credentials();
                CredentialsObject.AliasName = CredentialsRow["AliasName"].ToString();
                Session["CredentialsUpdateStatus"] = CredentialsObject.AliasName;
                CredentialsObject.UserID = CredentialsRow["UserID"].ToString();
                ServerTypes STypeobject = new ServerTypes();
                STypeobject.ServerType = CredentialsRow["ServerType"].ToString();
                ServerTypes ReturnValue = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetDataForServerType(STypeobject);
                CredentialsObject.ServerTypeID = ReturnValue.ID;
                string rawPass;
                if (Mode == "Insert")
                {
                    if (Session["password"] != null)
                    {
                        rawPass = Session["password"].ToString();
                    }
                    else
                    {
                        rawPass = "";
                    }
                }

                else
                {
                    rawPass = CredentialsRow["Password"].ToString();
                }
                byte[] encryptedPass;
                string encryptedPassAsString;
                //7/10/2015 NS modified for VSPLUS-1985
                if (rawPass == "test")
                {
                    encryptedPassAsString = rawPass;
                }
                else
                {
                    encryptedPass = tripleDes.Encrypt(rawPass);
                    encryptedPassAsString = string.Join(", ", encryptedPass.Select(s => s.ToString()).ToArray());
                }

                CredentialsObject.Password = encryptedPassAsString;


                //if (Password.Text != "")
                //{
                //    if (PasswordTextBox.Text == "      ")
                //        PasswordTextBox.Text = ViewState["PWD"].ToString();

                //    TripleDES tripleDES = new TripleDES();
                //    byte[] encryptedPass = tripleDES.Encrypt(PasswordTextBox.Text);
                //    string encryptedPassAsString = string.Join(", ", encryptedPass.Select(s => s.ToString()).ToArray());
                //    CredentialsObject.Password = encryptedPassAsString;
                //}
                if (Mode == "Update")
                    CredentialsObject.ID = int.Parse(CredentialsRow["ID"].ToString());

                return CredentialsObject;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void AliasGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

            AliasDataTable = (DataTable)Session["Alias"];
            ASPxGridView gridView = (ASPxGridView)sender;

            UpdateCredentialsData("Update", GetRow(AliasDataTable, e.NewValues.GetEnumerator(), Convert.ToInt32(e.Keys[0])));
            //Update Grid after inserting new row, refresh grid as in page load
            gridView.CancelEdit();
            e.Cancel = true;
            FillAliasGrid();
            //3/20/2014 NS modified
            //tdmsgforlocation.Text = msgloc.ToString();
            //10/6/2014 NS modified for VSPLUS-990
            errorDiv.InnerHtml = msgloc.ToString() +
                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
            errorDiv.Style.Value = "display: block";

        }

        protected void bttnDelete_Click(object sender, EventArgs e)
        {
            ImageButton bttnDel = (ImageButton)sender;
            Credentials locObj = new Credentials();
            locObj.ID = Convert.ToInt32(bttnDel.CommandArgument);
            locID = Convert.ToInt32(bttnDel.CommandArgument);
            string locName = bttnDel.AlternateText;
            pnlAreaDtls.Style.Add("visibility", "visible");
            divmsg.InnerHtml = "Are you sure you want to delete the credential " + locName + "?";
        }
        protected void bttnOK_Click(object sender, EventArgs e)
        {
            Credentials locObj = new Credentials();
            locObj.ID = locID;
            Object returnValue = VSWebBL.ConfiguratorBL.CredentialsBL.Ins.DeleteData(locObj);
            if (Convert.ToBoolean(returnValue) == false)
            {
                //3/20/2014 NS modified
                //NavigatorPopupControl.ShowOnPageLoad = true;
                //MsgLabel.Text = "These credentials may not be deleted, other dependencies exist.";
                //10/6/2014 NS modified for VSPLUS-990
                errorDiv.InnerHtml = "These credentials may not be deleted, other dependencies exist." +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
            }
            else
            {
                FillAliasGrid();
            }
        }

        protected void bttnCancel_Click(object sender, EventArgs e)
        {
            FillAliasGrid();
        }
        protected void subbttnOK_Click(object sender, EventArgs e)
        {
            NavigatorPopupControl.ShowOnPageLoad = false;
        }
        protected void AliasGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("PwrshellCredentials|AliasGridView", AliasGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        private void ServerTypeComboBox_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillServerTypeComboBox(source as ASPxComboBox);
        }
        private void Passwordtext_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillAliasGridfromSession();
        }


        private void FillServerTypeComboBox(ASPxComboBox ServerTypeComboBox)
        {
            DataTable ServerDataTable = VSWebBL.SecurityBL.ServerTypesBL.Ins.GetSpecificServerTypes();
            ServerTypeComboBox.DataSource = ServerDataTable;
            ServerTypeComboBox.TextField = "ServerType";
            ServerTypeComboBox.ValueField = "ServerType";
            ServerTypeComboBox.DataBind();
        }

        protected void CredPassword_PreRender(object sender, EventArgs e)
        {
            ASPxTextBox edit = sender as ASPxTextBox;
            edit.Text = "      ";
            edit.ClientSideEvents.Init = "function(s, e) {s.SetText('" + edit.Text + "');}";
        }
        protected void AliasGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            //e.Editor.SetClientSideEventHandler("KeyPress", "OnTextBoxKeyPress");
            if (e.Column.FieldName == "ServerType")
            {
                ASPxComboBox ServerTypeComboBox = e.Editor as ASPxComboBox;

                FillServerTypeComboBox(ServerTypeComboBox);
                ServerTypeComboBox.Callback += new CallbackEventHandlerBase(ServerTypeComboBox_OnCallback);


            }
            //if (e.Column.FieldName == "Password")
            //{
            //    ASPxTextBox Password = e.Editor as ASPxTextBox;
            //    Password.Attributes["type"] = "password";
            //    Password.Text = "      ";
            //}

            //if ((AliasGridView.IsNewRowEditing == false))
            //{
            //    if ((e.Column.FieldName == "Password"))
            //    {
            //        LinkButton linkbtn = ((LinkButton)(AliasGridView.FindRowCellTemplateControlByKey(e.KeyValue, e.Column, "LinkButton1")));
            //        linkbtn.Visible = true;
            //    }

            //}


            //if ((AliasGridView.IsNewRowEditing == true))
            //{
            //    if ((e.Column.FieldName == "Password"))
            //    {
            //        LinkButton linkaddbtn = ((LinkButton)(AliasGridView.FindRowCellTemplateControlByKey(e.KeyValue, e.Column, "LinkButton2")));
            //        linkaddbtn.Visible = true;
            //    }

            //}




            //    //Password.Text = ""; 
            //    //VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
            //    //Credentials CredentialsObject = new Credentials();
            //    //ASPxTextBox Password = e.Editor as ASPxTextBox;
            //    //string rawPass = CredentialsRow["Password"].ToString();
            //    //byte[] encryptedPass = tripleDes.Encrypt(rawPass);
            //    //string encryptedPassAsString = string.Join(", ", encryptedPass.Select(s => s.ToString()).ToArray());

            //    //CredentialsObject.Password = encryptedPassAsString;
            //}

        }
        protected void AliasGridView_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "ServerType")
            {
                ASPxComboBox ServerTypeComboBox = e.Editor as ASPxComboBox;
                FillServerTypeComboBox(ServerTypeComboBox);
                ServerTypeComboBox.Callback += new CallbackEventHandlerBase(ServerTypeComboBox_OnCallback);
            }
            //if (e.Column.FieldName == "Password")
            //{
            //    ASPxTextBox Password = e.Editor as ASPxTextBox;
            //    //Password.Attributes["type"] = "password";
            //    Password.Text = "      ";
            //}
        }

        protected void confirmButton_Click(object sender, EventArgs e)
        {
            UpdatePasswordField(cnpsw.Text);

            ASPxPopupControl2.ShowOnPageLoad = false;
            GridViewDataColumn Name = AliasGridView.Columns["Password"] as GridViewDataColumn;
            LinkButton linkbutton = AliasGridView.FindEditRowCellTemplateControl(Name, "LinkButton1") as LinkButton;
            LinkButton lnkadd = AliasGridView.FindEditRowCellTemplateControl(Name, "LinkButton2") as LinkButton;
			//1/25/2016 Durga modified for VSPLUS-2500
			Label msglabel = AliasGridView.FindEditRowCellTemplateControl(Name, "msg") as Label;
            linkbutton.Visible = true;

            lnkadd.Visible = false;
			msglabel.Text = "<b>" +
							  "</b>Password updated successfully." +
							 "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			msglabel.Style.Value = "display: block";

			msglabel.Visible = true;

        }
		
        protected void UpdatePasswordField(string newPassword)
        {
            int index = AliasGridView.EditingRowVisibleIndex;
            DataTable dt = (DataTable)Session["Alias"];
            dt.Rows[index]["Password"] = newPassword;
            Session["Alias"] = dt;


        }





        protected void okbtn_Click(object sender, EventArgs e)
        {
            Session["password"] = cnfrmpassword.Text;

            //UpdatenewPasswordField(cnfrmpassword.Text);
            ASPxPopupControl3.ShowOnPageLoad = false;
            GridViewDataColumn Name = AliasGridView.Columns["Password"] as GridViewDataColumn;
            LinkButton lnkbtn = AliasGridView.FindEditRowCellTemplateControl(Name, "LinkButton2") as LinkButton;
            LinkButton linkedit = AliasGridView.FindEditRowCellTemplateControl(Name, "LinkButton1") as LinkButton;
			//1/27/2016 Durga modified for VSPLUS-2500
			Label msglabel = AliasGridView.FindEditRowCellTemplateControl(Name, "msg") as Label;
            lnkbtn.Visible = true;
            linkedit.Visible = false;
			msglabel.Text = "<b>" +
							  "</b>Password updated successfully." +
							 "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
			msglabel.Style.Value = "display: block";

			msglabel.Visible = true;
          

        }
     

        protected void AliasGridView_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e)
        {
            //7/2/2015 NS added for VSPLUS-1934
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