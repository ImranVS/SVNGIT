using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VSWebBL;
using VSWebDO;
using DevExpress.Web;
using DevExpress.Web.Data;

namespace VSWeb.Configurator
{
  
    public partial class GridTest : System.Web.UI.Page
    {
      

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               

                
                DataTable DSTaskSettingsDataTable = new DataTable();
              
                DSTaskSettingsDataTable = VSWebBL.ConfiguratorBL.DominoPropertiesBL.Ins.DSTaskSettingsUpdateGrid("1");
                if (DSTaskSettingsDataTable.Rows.Count > 0)
                {
                    //DataTable dtcopy = DSTaskSettingsDataTable.Copy();
                    //dtcopy.PrimaryKey = new DataColumn[] { dtcopy.Columns["MyID"] };
                    //STSettingsDataSet.Tables.Add(dtcopy);
                    //Session["STSettingsDataSet"] = STSettingsDataSet;

                    grid.DataSource = DSTaskSettingsDataTable;//STSettingsDataSet.Tables[0];//
                    grid.DataBind();
                }

               // grid.StartEdit(2);
            }
            grid.SettingsEditing.Mode = chkPopup.Checked
                ? GridViewEditingMode.PopupEditForm
                : GridViewEditingMode.EditFormAndDisplayRow;
        }
        //protected void grid_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        //{
        //    e.NewValues["SendLoadCommand"] = false;
        //}

        protected void grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.NewValues["Notes"] = GetMemoText();
        }
        protected void grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.NewValues["Notes"] = GetMemoText();
        }
        protected string GetMemoText()
        {
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("pageControl") as ASPxPageControl;
            ASPxMemo memo = pageControl.FindControl("notesEditor") as ASPxMemo;
            return memo.Text;
        }
        protected void AccessDataSource1_Modifying(object sender, SqlDataSourceCommandEventArgs e)
        {
            //DevExpress.Web.Design.Utils.AssertNotReadOnly();
        }
    }
}