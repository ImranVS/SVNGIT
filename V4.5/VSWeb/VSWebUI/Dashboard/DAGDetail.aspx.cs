using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraCharts;
using System.Drawing;
using VSWebBL;
using DevExpress.Web.Data;

namespace VSWebUI.Dashboard
{
    public partial class DAGDetail : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        //7/23/2014 NS modified
        int DAGID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !IsCallback)
            {

                if (Request.QueryString["Type"] != "" && Request.QueryString["Type"] != null)
                {
                    lblServerType.Text = Request.QueryString["Type"].ToString() + ":  ";
                }
                if (Request.QueryString["Typ"] != "" && Request.QueryString["Typ"] != null)
                {
                    lblServerType.Text = Request.QueryString["Type"].ToString();
                }



                if (Request.QueryString["Name"] != "" && Request.QueryString["Name"] != null)
                {
                    servernamelbl.Text = Request.QueryString["Name"];
                }
                if (Request.QueryString["LastDate"] != "" && Request.QueryString["LastDate"] != null)
                {
                    Lastscanned.Text = Request.QueryString["LastDate"].ToString();
                }
                else
                {
                    DataTable dt = VSWebBL.DashboardBL.QuickrHealthBLL.Ins.getLastScanDate(servernamelbl.Text);
                    if (dt.Rows.Count > 0)
                    {
                        Lastscanned.Text = dt.Rows[0]["LastUpdate"].ToString();
                    }
                    else
                    {
                        lbltext.Visible = false;
                        Lastscanned.Visible = false;
                    }
                }
                //7/23/2014 NS modified
			
				//DAGID = int.Parse(Request.QueryString["ID"].ToString());
                FillDAGStatusGridView();
                FillDAGMembersGridView(DAGID);
                FillDAGDBGridView(DAGID);
            }
            Session["FocusedRow"] = DAGStatusGridView.GetRowValues(DAGStatusGridView.FocusedRowIndex, new string[] { DAGStatusGridView.KeyFieldName });
        }

        public virtual string GetFieldValue(object item)
        {
            WebDescriptorRowBase row = item as WebDescriptorRowBase;
            return DAGStatusGridView.GetRowValues(row.VisibleIndex, DAGStatusGridView.KeyFieldName).ToString();
        }
        public virtual string GetFieldChecked(object item)
        {
            WebDescriptorRowBase row = item as WebDescriptorRowBase;
            object o = DAGStatusGridView.GetRowValues(row.VisibleIndex, DAGStatusGridView.KeyFieldName);
            return Session["FocusedRow"] == o ? "CHECKED" : "";
        }

        private void FillDAGStatusGridView()
        {
            try
            {
                DataTable DSTaskSettingsDataTable = new DataTable();
                //7/23/2014 NS modified
                DSTaskSettingsDataTable = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetDAGStatus(DAGID.ToString());
                Session["DAGStatus"] = DSTaskSettingsDataTable;
                DAGStatusGridView.DataSource = DSTaskSettingsDataTable;
                DAGStatusGridView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void DAGStatusGridView_SelectionChanged(object sender, EventArgs e)
        {
            
            System.Collections.Generic.List<object> Type = DAGStatusGridView.GetSelectedFieldValues("ID");
            System.Collections.Generic.List<object> DAGName = DAGStatusGridView.GetSelectedFieldValues("DAGName");
            if (Type.Count > 0)
            {
                int DAGID = int.Parse(Type[0].ToString());
                servernamelbl.Text = DAGName[0].ToString();
                FillDAGMembersGridView(DAGID);
                FillDAGDBGridView(DAGID);

            }
        }
        private void FillDAGMembersGridView(int DAGID)
        {
            try
            {
                DataTable DSTaskSettingsDataTable = new DataTable();
                DSTaskSettingsDataTable = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetDAGMembers( DAGID);
                Session["DAGMembers"] = DSTaskSettingsDataTable;
                DAGMembersGridView.DataSource = DSTaskSettingsDataTable;
                DAGMembersGridView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
        private void FillDAGDBGridView(int DAGID)
        {
            try
            {
                DataTable DSTaskSettingsDataTable = new DataTable();
                DSTaskSettingsDataTable = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetDAGDB(DAGID);
                Session["DAGDB"] = DSTaskSettingsDataTable;
                DAGDBGridView.DataSource = DSTaskSettingsDataTable;
                DAGDBGridView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }

        protected void DAGStatusGridView_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void DAGStatusGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            string status = "";
            status = e.GetValue("Status").ToString();
            switch (e.DataColumn.FieldName)
            {
                case "Status":
                    if ((status.ToUpper()).Contains("FAIL"))
                    {
                        e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (status.ToUpper() == "ISSUE")
                    {
                        e.Cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    else if (status.ToUpper() == "OK" || status.ToUpper() == "PASS" || status.ToUpper() == "PASSED")
                    {
                        e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    }
                    break;
            }
        }

        protected void DAGMembersGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName != "ServerName" && (e.CellValue.ToString() == "Pass" || e.CellValue.ToString() == "Passed"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
            else if (e.DataColumn.FieldName != "ServerName" && (e.CellValue.ToString() == "Fail" || e.CellValue.ToString() == "Failed"))
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
        }

        protected void DAGDBGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "ContendIndex" && e.CellValue.ToString() == "Healthy")
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
            else if (e.DataColumn.FieldName == "ContendIndex")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
            }
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            if (Session["BackURL"] != "" && Session["BackURL"] != null)
            {
                Response.Redirect(Session["BackURL"].ToString());
                Session["BackURL"] = "";

            }
        }
    }
}