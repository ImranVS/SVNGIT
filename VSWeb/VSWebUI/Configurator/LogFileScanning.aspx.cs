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

namespace VSWebUI.Configurator
{
    public partial class LogFileScanning : System.Web.UI.Page
    {
        DataTable LogFileDataTable = null;
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack)
            {
             
                FillLogFileGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "LogFileScanning|LogFileGridView")
                        {
                            LogFileGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {

                FillLogFileGridfromSession();

            }   
        }

        private void FillLogFileGrid()
        {
            try
            {

                LogFileDataTable = new DataTable();
                DataSet LogFileDataSet = new DataSet();
                LogFileDataTable = VSWebBL.ConfiguratorBL.LogFileBL.Ins.GetAllData();
                if (LogFileDataTable.Rows.Count >= 0)
                {
                    DataTable dtcopy = LogFileDataTable.Copy();
                    dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    // LocationDataSet.Tables.Add(dtcopy);

                    //Session["LocationDataSet"] = LocationDataSet;

                    Session["LogFile"] = dtcopy;
                    LogFileGridView.DataSource = LogFileDataTable;
                    LogFileGridView.DataBind();
                    
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


        private void FillLogFileGridfromSession()
        {
            try
            {

                LogFileDataTable = new DataTable();
                if(Session["LogFile"]!=""&&Session["LogFile"]!=null)
                LogFileDataTable = (DataTable)Session["LogFile"];
                if (LogFileDataTable.Rows.Count > 0)
                {
                    LogFileGridView.DataSource = LogFileDataTable;
                    LogFileGridView.DataBind();
                }
                else
                {
                    //VSPLUS-610, 18Jun14, Mukund: commented to eliminate error on clicking Add
                   // LogFileDataTable.Columns.Add("ID");
                    //LogFileDataTable.Columns.Add("Keyword");
                    //LogFileDataTable.Columns.Add("RepeatOnce");
                    //LogFileDataTable.Columns.Add("");
                    LogFileGridView.DataSource = LogFileDataTable;
                    LogFileGridView.DataBind();
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
        //protected DataRow GetRow(DataTable LogObject, IDictionaryEnumerator enumerator, int Keys)
        //{
        //    DataTable dataTable = LogObject;
        //    DataRow DRRow;
        //    if (Keys == 0)
        //        DRRow = dataTable.NewRow();
        //    else
        //        DRRow = dataTable.Rows.Find(Keys);
        //    //IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
        //    enumerator.Reset();
        //    while (enumerator.MoveNext())
        //        DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
        //    return DRRow;
        //}



        protected void LogFileGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            LogFileDataTable = (DataTable)Session["LogFile"];
            ASPxGridView gridView = (ASPxGridView)sender;
            ASPxTextBox LogFileTextBox = (ASPxTextBox)gridView.FindEditFormTemplateControl("LogFileTextBox");
            ASPxTextBox NotLogFileTextBox = (ASPxTextBox)gridView.FindEditFormTemplateControl("NotLogFileTextBox");
            //VSPLUS-610, 18Jun14, Mukund: added condition to eliminate duplicates
            if (LogFileDataTable.Select("keyword='" + LogFileTextBox.Text + "'").Count() == 0)
            {

                //Insert row in DB
                UpdateLogFileData("Insert", GetRowEditTemplate(gridView, "Insert"));

                //Update Grid after inserting new row, refresh grid as in page load
                gridView.CancelEdit();
                e.Cancel = true;
                FillLogFileGrid();
            }
            else
            {
               throw new ArgumentException("Duplicate entry");
            }
          
        }
        private void UpdateLogFileData(string Mode, DataRow LogFileRow)
        {
            if (Mode == "Insert")
            {
                Object ReturnValue = VSWebBL.ConfiguratorBL.LogFileBL.Ins.InsertData(CollectDataForLogFile(Mode, LogFileRow));
            }
            if (Mode == "Update")
            {
                Object ReturnValue = VSWebBL.ConfiguratorBL.LogFileBL.Ins.UpdateData(CollectDataForLogFile(Mode, LogFileRow));

            }
        }

       
        private LogFile CollectDataForLogFile(string Mode, DataRow LogFileRow)
        {
            try
            {
                LogFile LogFileObject = new LogFile();
                ContentPlaceHolder cph = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");

                ASPxGridView gv = (ASPxGridView)cph.FindControl("LogFileGridView");
                    //cph.FindControl("LogFileGridView");
                ASPxTextBox txtKeyword=(ASPxTextBox)gv.FindEditFormTemplateControl("LogFileTextBox");
                ASPxTextBox txtNotRequiredKeyword = (ASPxTextBox)gv.FindEditFormTemplateControl("NotLogFileTextBox");

                ASPxCheckBox LogFileCheckBox = (ASPxCheckBox)gv.FindEditFormTemplateControl("LogFileCheckBox");
				ASPxCheckBox logCheckBox = (ASPxCheckBox)gv.FindEditFormTemplateControl("logCheckBox");
				ASPxCheckBox AgentlogCheckBox = (ASPxCheckBox)gv.FindEditFormTemplateControl("AgentlogCheckBox");
                LogFileObject.Keyword =  txtKeyword.Text;
                LogFileObject.NotRequiredKeyword = txtNotRequiredKeyword.Text;
                    //LogFileRow["Keyword"].ToString();
                if (LogFileCheckBox.Checked == true)
                {
                    LogFileObject.RepeatOnce = true;
                }
                else
                {
                    LogFileObject.RepeatOnce = false;
                }
				if (logCheckBox.Checked == true)
				{
					LogFileObject.Log = true;
				}
				else
				{
					LogFileObject.Log = false;
				}
				if (AgentlogCheckBox.Checked == true)
				{
					LogFileObject.AgentLog = true;
				}
				else
				{
					LogFileObject.AgentLog = false;
				}
                //bool.Parse(LogFileRow["RepeatOnce"].ToString());
                if (Mode == "Update")
                LogFileObject.ID = int.Parse(LogFileRow["ID"].ToString());
                return LogFileObject;
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex; 
            }
            finally { }
        }

       

        protected void LogFileGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            LogFile LogObject = new LogFile();
            LogObject.ID = Convert.ToInt32(e.Keys[0]);

            //Delete row from DB
            Object ReturnValue = VSWebBL.ConfiguratorBL.LogFileBL.Ins.DeleteData(LogObject);

            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillLogFileGrid();
        }

        protected void LogFileGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
           LogFileDataTable = (DataTable)Session["LogFile"];
            ASPxGridView gridView = (ASPxGridView)sender;
            HiddenField LogFileIDHiddenField = (HiddenField)gridView.FindEditFormTemplateControl("LogFileIDHiddenField");
            ASPxTextBox LogFileTextBox = (ASPxTextBox)gridView.FindEditFormTemplateControl("LogFileTextBox");
            ASPxTextBox NotLogFileTextBox = (ASPxTextBox)gridView.FindEditFormTemplateControl("NotLogFileTextBox");
            //VSPLUS-610, 18Jun14, Mukund: added condition to eliminate duplicates
            if (LogFileDataTable.Select("keyword='" + LogFileTextBox.Text + "' and ID<>" + LogFileIDHiddenField.Value).Count() == 0)
            {

                //Update row in DB
                UpdateLogFileData("Update", GetRowEditTemplate(gridView, "Update"));
                //Update Grid after inserting new row, refresh grid as in page load
                gridView.CancelEdit();
                e.Cancel = true;
                FillLogFileGrid();
            }
            else
            { 
               throw new ArgumentException("Duplicate entry");
            
            }
            
           
        }

        //protected void LogFileGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        //{
        //    if (e.RowType == GridViewRowType.EditForm)
        //    {
        //        ASPxLabel LogFileLabel = (sender as ASPxGridView).FindEditFormTemplateControl("LogFileLabel") as ASPxLabel;
        //        LogFileLabel.Text = "VitalSigns will search the log file(log.nsf) for this word or phrase. If you would like to be alerted everytime the word is found. do not check the limitbox.";
        //    }
            
        //}
        
        protected DataRow GetRowEditTemplate(ASPxGridView gridView, string Mode)
        {
            HiddenField LogFileIDHiddenField = (HiddenField)gridView.FindEditFormTemplateControl("LogFileIDHiddenField");
            ASPxTextBox LogFileTextBox = (ASPxTextBox)gridView.FindEditFormTemplateControl("LogFileTextBox");
            ASPxCheckBox LogFileCheckBox = (ASPxCheckBox)gridView.FindEditFormTemplateControl("LogFileCheckBox");
			ASPxCheckBox logCheckBox = (ASPxCheckBox)gridView.FindEditFormTemplateControl("logCheckBox");
			ASPxCheckBox AgentlogCheckBox = (ASPxCheckBox)gridView.FindEditFormTemplateControl("AgentlogCheckBox");
            ASPxTextBox NotLogFileTextBox = (ASPxTextBox)gridView.FindEditFormTemplateControl("NotLogFileTextBox");

          
            DataTable LogFiledatatable = (DataTable)Session["LogFile"];
            DataTable dataTable = LogFiledatatable;

            
            if (dataTable != null)
            {
                DataRow DRRow = dataTable.NewRow();

                if (Mode == "Update") DRRow["ID"] = LogFileIDHiddenField.Value;
                DRRow["Keyword"] = LogFileTextBox.Text;
                DRRow["NotRequiredKeyword"] = NotLogFileTextBox.Text;

               
                DRRow["RepeatOnce"] = LogFileCheckBox.Checked;
				DRRow["Log"] = logCheckBox.Checked;
				DRRow["AgentLog"] = AgentlogCheckBox.Checked;
                return DRRow;
            }
            else
            {
                DataRow dr = null;
                return dr;
            }

          }

        protected void LogFileGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                e.Cell.ToolTip = string.Format("{0}", e.CellValue);
            }
            catch (Exception ex)
            {
                //6/27/2014 NS added for VSPLUS-634
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
          
        }

        protected void LogFileGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("LogFileScanning|LogFileGridView", LogFileGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
    }

}