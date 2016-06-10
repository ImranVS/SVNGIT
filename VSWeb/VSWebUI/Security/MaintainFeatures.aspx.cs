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
    public partial class MaintainFeatures : System.Web.UI.Page
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
        DataTable FeaturesDataTable = null;
        static int locID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {            
            pnlAreaDtls.Style.Add("visibility", "hidden");
            if (!IsPostBack)
            {
                FillFeaturesGrid();
                 if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "AdminTab|FeaturesGridView")
                        {
                            FeaturesGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }                        
                    }
                }
            }
            else
            {
                FillFeaturesGridfromSession();             
            }   
        }

   
        private void FillFeaturesGrid()
        {
            try
            {

                 FeaturesDataTable = new DataTable();
                 DataSet FeatureDataSet = new DataSet();
                 FeaturesDataTable = VSWebBL.SecurityBL.FeaturesBL.Ins.GetAllData();
                 if (FeaturesDataTable.Rows.Count > 0)
                 {
                     DataTable dtcopy = FeaturesDataTable.Copy();
                     dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["ID"] };
                    

                     Session["Features"] = dtcopy;
                     FeaturesGridView.DataSource = FeaturesDataTable;
                     FeaturesGridView.DataBind();
                 }
                 else
                 {
                     FeaturesGridView.DataSource = FeaturesDataTable;
                     FeaturesGridView.DataBind();
                     Session["Features"] = FeaturesDataTable;
                 }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        private void FillFeaturesGridfromSession()
        {
            try
            {

                FeaturesDataTable = new DataTable();
                if(Session["Features"]!=""&&Session["Features"]!=null)
                FeaturesDataTable = (DataTable)Session["Features"];
                if (FeaturesDataTable.Rows.Count > 0)
                {
                    FeaturesGridView.DataSource = FeaturesDataTable;
                    FeaturesGridView.DataBind();
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
            DataTable dataTable = new DataTable();
            if (LocObject != null)
            {
                dataTable = LocObject;
            }
            else
            {
                dataTable.Columns.Add("Name");
            }
            DataRow DRRow=null;
            if(Keys==0)
               
                    DRRow = dataTable.NewRow();
               
            else
                DRRow = dataTable.Rows.Find(Keys);
 
            enumerator.Reset();
            while (enumerator.MoveNext())
                DRRow[enumerator.Key.ToString()] = (enumerator.Value == null ? "False" : enumerator.Value);
            return DRRow;
        }

        protected void FeaturesGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            if (Session["Features"] != null && Session["Features"] != "")
            {
                FeaturesDataTable = (DataTable)Session["Features"];
            }
            else
            {
                FeaturesDataTable = null;
            }
            ASPxGridView gridView = (ASPxGridView)sender;

           UpdateFeatureData("Insert", GetRow(FeaturesDataTable, e.NewValues.GetEnumerator(), 0));
            gridView.CancelEdit();
            e.Cancel = true;
            FillFeaturesGrid();
            
        }

        private void UpdateFeatureData(string Mode, DataRow FeaturesRow)
        {
            if (Mode == "Insert")
            {
                Object ReturnValue = VSWebBL.SecurityBL.FeaturesBL.Ins.InsertData(CollectDataForFeatures(Mode, FeaturesRow));
                if (ReturnValue == "")
                {
                    msgloc= "Feature Already Exists";
                }
            }
            if (Mode == "Update")
            {
                Object ReturnValue = VSWebBL.SecurityBL.FeaturesBL.Ins.UpdateData(CollectDataForFeatures(Mode, FeaturesRow));
            
            }
        }

        private Features CollectDataForFeatures(string Mode, DataRow FeaturesRow)
        {
            try
            {
                Features FeaturesObject = new Features();
                FeaturesObject.Name = FeaturesRow["Name"].ToString();
               if(Mode=="Update")
                FeaturesObject.ID = int.Parse(FeaturesRow["ID"].ToString());

                return FeaturesObject;
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void FeaturesGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            FeaturesDataTable = (DataTable)Session["Features"];
            ASPxGridView gridView = (ASPxGridView)sender;

            //Insert row in DB
            UpdateFeatureData("Update", GetRow(FeaturesDataTable, e.NewValues.GetEnumerator(),Convert.ToInt32(e.Keys[0])));
            //Update Grid after inserting new row, refresh grid as in page load
            gridView.CancelEdit();
            e.Cancel = true;
            FillFeaturesGrid();
        }

        protected void FeaturesGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            Features LocObject = new Features();
            LocObject.ID = Convert.ToInt32(e.Keys[0]);

            //Delete row from DB
            Object ReturnValue = VSWebBL.SecurityBL.FeaturesBL.Ins.DeleteData(LocObject);

            //Update Grid after inserting new row, refresh grid as in page load
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.CancelEdit();
            e.Cancel = true;
            FillFeaturesGrid();
        }

      
 
        protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("MaintainFeatures.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void bttnDelete_Click(object sender, EventArgs e)
        {
            ImageButton bttnDel = (ImageButton)sender;
            Features locObj = new Features();
            locObj.ID = Convert.ToInt32(bttnDel.CommandArgument);
            locID = Convert.ToInt32(bttnDel.CommandArgument);
            string locName = bttnDel.AlternateText;
            pnlAreaDtls.Style.Add("visibility", "visible");
            divmsg.InnerHtml = "Are you sure you want to delete the Feature " + locName + "?";
        }        

        protected void bttnOK_Click(object sender, EventArgs e)
        {
            Features locObj = new Features();
            locObj.ID = locID;
            Object returnValue = VSWebBL.SecurityBL.FeaturesBL.Ins.DeleteData(locObj);
            if (Convert.ToBoolean(returnValue) == false)
            {
                NavigatorPopupControl.ShowOnPageLoad = true;
                MsgLabel.Text = "This Feature cannot be deleted, other dependencies exist.";
            }
            else
            {
                FillFeaturesGrid();
            }
        }
       
        protected void bttnCancel_Click(object sender, EventArgs e)
        {
            FillFeaturesGrid();
        }        

        protected void subbttnOK_Click(object sender, EventArgs e)
        {
            NavigatorPopupControl.ShowOnPageLoad = false;
        }

        protected void FeaturesGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("AdminTab|FeaturesGridView", FeaturesGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        
    }
}