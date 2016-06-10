using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSFramework;
using VSWebBL;
using VSWebDO;
using DevExpress.Web;
using System.Collections;
using DevExpress.Web.Data;
using System.Runtime.InteropServices;
using System.Data;

namespace VSWebUI.Configurator
{
    public partial class Applications : System.Web.UI.Page
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
			bool popupopen;
            Control ctrl;
            Page.Title = "WebSphere Cell";
			if (!IsPostBack)
			{
                FillWindowsServicesGrid();
			}
			else
			{
                FillWindowsServicesGridFromSession();
			}
            if (Session["UserPreferences"] != null)
            {
                DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                foreach (DataRow dr in UserPreferences.Rows)
                {
                    if (dr[1].ToString() == "Applications|ServicesGrid")
                    {
                        ServicesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                    }
                }
            }

		}

        private void FillWindowsServicesGrid()
        {
            try
            {
                Session["WindowsServices"] = null;

                DataTable dt = new DataTable();

                dt = VSWebBL.ConfiguratorBL.ServicesBL.Ins.GetWindowsServicesWS();
				if (dt.Rows.Count > 0)
				{
					string servername = dt.Rows[0]["ServerName"].ToString();
			
                Session["servername"] = servername;
				}
                DataColumn[] columns = new DataColumn[1];
                columns[0] = dt.Columns["ID"];
                dt.PrimaryKey = columns;

                if (dt.Rows.Count >= 0)
                {
                    Session["WindowsServices"] = dt;

                }


                ServicesGrid.DataSource = dt;
                ServicesGrid.DataBind();
                (ServicesGrid.Columns["Type"] as GridViewDataColumn).GroupBy();
                //11/18/2014 NS added
                ServicesGrid.SortBy(ServicesGrid.Columns["Type"] as GridViewDataColumn, DevExpress.Data.ColumnSortOrder.Descending);

            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }
        private void FillWindowsServicesGridFromSession()
        {
            try
            {

                DataTable dt = new DataTable();

                if (Session["WindowsServices"] != null && Session["WindowsServices"] != "")
                    dt = (DataTable)Session["WindowsServices"];


                if (dt.Rows.Count > 0)
                {
                    ServicesGrid.DataSource = dt;
                    ServicesGrid.DataBind();
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
        protected void checkToMonitor_Init(object sender, EventArgs e)
        {
            ASPxCheckBox chk = sender as ASPxCheckBox;
            GridViewDataItemTemplateContainer container = chk.NamingContainer as GridViewDataItemTemplateContainer;

            chk.ClientSideEvents.CheckedChanged = String.Format("function (s,e) {{ cb.PerformCallback('{0}|' + s.GetChecked()); }}", container.KeyValue);
        }
        protected void ServicesGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Result" && (e.CellValue.ToString() == "Running"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
            else if (e.DataColumn.FieldName == "Result" && (e.CellValue.ToString() == "Stopped"))
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
            else if (e.DataColumn.FieldName == "Result")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
            }
        }
        private DataTable GetSelectedServices()
        {


            DataTable dtSel = new DataTable();
            try
            {
                dtSel.Columns.Add("id");

                DataTable dt = (DataTable)Session["WindowsServices"];
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["isSelected"].ToString().ToLower() == "true")
                        {
                            DataRow dr = dtSel.NewRow();
                            dr["id"] = row["ID"];
                            dtSel.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }

            return dtSel;

        }

        protected void cb_callback(object sender, DevExpress.Web.CallbackEventArgs e)
        {
            String[] parameters = e.Parameter.Split('|');
            string id = parameters[0];
            bool isChecked = Convert.ToBoolean(parameters[1]);

            DataTable dt = new DataTable();

            if (Session["WindowsServices"] != null && Session["WindowsServices"] != "")
                dt = (DataTable)Session["WindowsServices"];

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows.Find(id);
                (dt.Rows.Find(id))["isSelected"] = isChecked;
                DataRow row2 = dt.Rows.Find(id);
                Session["WindowsServices"] = dt;
            }
        }

        protected void FormOkButton_Click(object sender, EventArgs e)
        {
            string errtext = "";
            bool proceed = true;
            if (proceed)
            {
                try
                {

                    UpdateServersData();
                }
                catch (Exception ex)
                {

                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                    throw ex;
                }
                finally { }
            }
            else
            {
                errorDiv.Style.Value = "display: block;";

                errorDiv.InnerHtml = errtext +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
            }

        }

        private void UpdateServersData()
        {
            try
            {

                //DataTable dt1 = GetWindowsServicesWS();
                //string servername = dt1.Rows[0]["SeverName"].ToString();
                //Session["servername"] = servername;
               

                List<object> fieldValues = new List<object>();

                DataTable dt = GetSelectedServices();
                List<DataRow> servicesSelected = dt.AsEnumerable().ToList();

                if (servicesSelected.Count > 0)
                {
                    foreach (DataRow row in servicesSelected)
                    {
                        if (row["id"] != null || row["id"] != "")
                            fieldValues.Add(row["id"]);
                    }
                }


                Object result2 = VSWebBL.ConfiguratorBL.ServicesBL.Ins.UpdateWindowsServicesWS(Session["servername"].ToString(), fieldValues);

                
                //if (ReturnValue.ToString() == "True")
                //{
                //    Session["WebSphereUpdateStatus"] = NameTextBox.Text;
                //    Response.Redirect("~/Configurator/WebSphereSeverGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                //    Context.ApplicationInstance.CompleteRequest();
                //}



            }
            catch (Exception ex)
            {
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message;
                errorDiv.Style.Value = "display: block";

                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
        }
        protected void ServicesGrid_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("Applications|ServicesGrid", ServicesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        public void FormCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Configurator/WebSphereSeverGrid.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
            Context.ApplicationInstance.CompleteRequest();
        }

	}
}