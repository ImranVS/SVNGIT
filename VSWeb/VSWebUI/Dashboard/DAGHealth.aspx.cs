using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;

namespace VSWebUI.Dashboard
{
    public partial class DAGHealth : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
        //int DAGID = -1;
        string DAGName = "";
        static string value = null;
        DataTable dtactive = new DataTable();
        DataTable dthealth = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            int index;
            if (Request.QueryString["Name"] != null)
            {
                //DAGID = int.Parse(Request.QueryString["ID"].ToString());
                DAGName = Request.QueryString["Name"].ToString();
                //9/24/2014 NS modified
                //lblTitle.Text += ":  " + DAGName;
                lblTitle.InnerHtml += ":  " + DAGName;
            }
            if (!IsPostBack)
            {
                Session["DAGStatusList"] = null;
                FillDAGStatusGrid();
                if (Session["UserPreferences"] != null)
                {
                    DataTable UserPreferences = (DataTable)Session["UserPreferences"];
                    foreach (DataRow dr in UserPreferences.Rows)
                    {
                        if (dr[1].ToString() == "DAGHealth|DAGGridView")
                        {
                            DAGGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                        if (dr[1].ToString() == "DAGHealth|ActivPrefGridView")
                        {
                            ActivPrefGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
                        }
                    }
                }
            }
            else
            {
				FillDAGStatusGrid();
            }
            //7/24/2014 NS added
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            Session["BackURL"] = url;
			if (Request.QueryString["Name"] == null)
            {
                index = DAGGridView.FocusedRowIndex;
                if (DAGGridView.VisibleRowCount > 0)
                {
                    value = DAGGridView.GetRowValues(index, "ID").ToString();
                    DAGName = DAGGridView.GetRowValues(index, "DAGName").ToString();
                }
            }
            else
            {
				//value = DAGID.ToString();
				value = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.getDagIdFromName(DAGName);
            }
            DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetDAGActivationPreference(value);
            if (dt.Rows.Count > 0)
            {
                dtactive = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetDAGActivePassive(value);
                dthealth = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetDAGHealth(value);
                //10/28/2014 NS modified for VE-157
                //DAGLabel.Text = DAGName + " Activation Preferences";
                //DAGPageControl.TabPages[0].Text = DAGName + " - Activation Preferences";
                lblSubtitle.InnerHtml = DAGName;
                ActivPrefGridView.Columns.Clear();
                ActivPrefGridView.DataSource = dt;
                ActivPrefGridView.AutoGenerateColumns = true;
                //8/21/2014 NS added for VSPLUS-888
                ActivPrefGridView.Settings.ShowGroupPanel = false;
                ActivPrefGridView.KeyFieldName = String.Empty;
                ActivPrefGridView.DataBind();
                for (int i = 1; i < ActivPrefGridView.Columns.Count; i++)
                {
                    ActivPrefGridView.Columns[i].Caption = "Activation Preference " + dt.Columns[i].Caption;
                }
            }
            else
            {
                //10/28/2014 NS modified for VE-157
                //DAGLabel.Text = DAGName + " Activation Preferences";
                //DAGPageControl.TabPages[0].Text = DAGName + " - Activation Preferences";
                lblSubtitle.InnerHtml = DAGName;
                ActivPrefGridView.Columns.Clear();
                ActivPrefGridView.DataSource = dt;
                ActivPrefGridView.AutoGenerateColumns = true;
                //8/21/2014 NS added for VSPLUS-888
                ActivPrefGridView.Settings.ShowGroupPanel = false;
                ActivPrefGridView.KeyFieldName = String.Empty;
                ActivPrefGridView.DataBind();
            }
            //10/28/2014 NS added for VE-157
            Session["DAGHealth"] = dthealth;
           
            //8/21/2014 NS modified for VSPLUS-888
            if (value != null && value != "")
            {
                //10/28/2014 NS modified for VE-157
                //DAGMembersLabel.Text = DAGName + " - DAG Members";
                //DAGPageControl.TabPages[1].Text = DAGName + " - DAG Members";
                lblSubtitle.InnerHtml = DAGName;
                FillDAGMembersGridView(int.Parse(value));
                //DAGDBsLabel.Text = DAGName + " -  DAG Databases";
                //DAGPageControl.TabPages[2].Text = DAGName + " - DAG Databases";
                lblSubtitle.InnerHtml = DAGName;
                FillDAGDBGridView(int.Parse(value));
				//DAGDBOverviewLabel.Text = DAGName + " -  DAG Databases Status";
                //DAGPageControl.TabPages[3].Text = DAGName + " - DAG Databases Status";
                lblSubtitle.InnerHtml = DAGName;
				FillDAGDBOverviewGridView(int.Parse(value));
            }
            //10/28/2014 NS added for VE-157
            SetTabImage();
        }


        public void FillDAGStatusGrid()
        {
            string DAGList = "";
            DataTable dt = new DataTable();
			if (VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.getDagIdFromName(DAGName) == "-1")
            {
                dt = VSWebBL.ExchangeBAL.Ins.GetDAGStatus("");
            }
            else
            {
                dt = VSWebBL.ExchangeBAL.Ins.GetDAGStatus(DAGName.ToString()); 
            }
            Session["DAGStatusList"] = dt;
            //DAGList.Text = "0";
            DAGList = "0";
            if (dt != null && dt.Rows.Count > 0)
            {
                //DAGList.Text = dt.Rows.Count.ToString();
                DAGList = dt.Rows.Count.ToString();
            }
            DAGGridView.DataSource = dt;
            DAGGridView.DataBind();
        }

        public void FillDAGStatusGridfromSession()
        {
            DataTable DataServers = new DataTable();
            try
            {
                if (Session["DAGStatusList"] != null && Session["DAGStatusList"] != "")
                {
                    DataServers = Session["DAGStatusList"] as DataTable;
                }
                if (DataServers.Rows.Count > 0)
                {
                    DataServers.PrimaryKey = new DataColumn[] { DataServers.Columns["DAGName"] };
                    //DAGList.Text = DataServers.Rows.Count.ToString();
                }
                DAGGridView.DataSource = DataServers;
                DAGGridView.DataBind();
            }
            catch (Exception ex)
            {
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
        }

        protected void DAGGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (DAGGridView.Selection.Count > 0)
            {
                System.Collections.Generic.List<object> DagID = DAGGridView.GetSelectedFieldValues("ID");
                System.Collections.Generic.List<object> DAGName = DAGGridView.GetSelectedFieldValues("DAGName");

                if (DagID.Count > 0)
                {
                    string sID = DagID[0].ToString();
                    DataTable dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetDAGActivationPreference(sID);
                    //10/28/2014 NS added for VE-157
                    string sDAGName = DAGName[0].ToString();
                    lblSubtitle.InnerHtml = sDAGName;
                    dt = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetDAGHealth(sID);
                    Session["DAGHealth"] = dt;
                }
            }
        }

        protected void DAGGridView_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            string status = "";
            status = e.GetValue("Status").ToString();
			string FileWitnessStatus = e.GetValue("FileWitnessServerStatus").ToString();
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

				case "FileWitnessServerStatus":
					if ((FileWitnessStatus.ToUpper()).Contains("NOT RESPONDING"))
					{
						e.Cell.BackColor = System.Drawing.Color.Red;
						e.Cell.ForeColor = System.Drawing.Color.White;
					}
					else if (FileWitnessStatus.ToUpper() == "OK")
					{
						e.Cell.BackColor = System.Drawing.Color.LightGreen;
					}
					break;
            }
        }

        protected void ActivPrefGridView_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string DAGID = "";
            DAGID = e.Parameters.ToString();
            ActivPrefGridView.DataSource = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetDAGActivationPreference(DAGID);
            ActivPrefGridView.Columns.Clear();
            ActivPrefGridView.AutoGenerateColumns = true;
            ActivPrefGridView.KeyFieldName = String.Empty;
            ActivPrefGridView.DataBind(); 
        }

        protected void ActivPrefGridView_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetDAGActivationPreference("");
        }

        protected void ActivPrefGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            int n = 0;
            bool isNumeric = false;
            string dbname = "";
            dbname = e.GetValue("DatabaseName").ToString();
            isNumeric = int.TryParse(e.DataColumn.FieldName, out n);
            DataRow[] dr = dtactive.Select("DatabaseName like '" + dbname + "'");
            DataRow[] drh = dthealth.Select("DatabaseName like '" + dbname + "'");
            if (e.DataColumn.FieldName != "DatabaseName" && dr[0][e.DataColumn.FieldName].ToString() == "Active")
            {
                if (drh[0][e.DataColumn.FieldName].ToString() == "Healthy")
                {
                    e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#2F9C02");
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
            }
            else if (e.DataColumn.FieldName != "DatabaseName" && dr[0][e.DataColumn.FieldName].ToString() == "Passive")
            {
                if (drh[0][e.DataColumn.FieldName].ToString() == "Healthy")
                {
                    if (e.DataColumn.FieldName == "1")
                    {
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2f200");
                        e.Cell.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#90D175");
                        e.Cell.ForeColor = System.Drawing.Color.Black;
                    }
                }
                else
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        protected void DAGMembersGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName != "ServerName" && (e.CellValue.ToString() == "Pass" || e.CellValue.ToString() == "Passed"))
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
            //7/29/2014 NS modified
            //else if (e.DataColumn.FieldName != "ServerName" && (e.CellValue.ToString() == "Fail" || e.CellValue.ToString() == "Failed"))
            else if (e.DataColumn.FieldName != "ServerName" && ((e.CellValue.ToString()).ToUpper()).Contains("FAIL"))
            {
                e.Cell.BackColor = System.Drawing.Color.Red;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
        }

        protected void DAGDBGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
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

        private void FillDAGMembersGridView(int DAGID)
        {
            try
            {
                DataTable DSTaskSettingsDataTable = new DataTable();
                DSTaskSettingsDataTable = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetDAGMembers(DAGID);
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

		private void FillDAGDBOverviewGridView(int DAGID)
		{
			try
			{
				DataTable DSTaskSettingsDataTable = new DataTable();
				DSTaskSettingsDataTable = VSWebBL.DashboardBL.ExchangeServerDetailsBL.Ins.GetDAGDBDetails(DAGID);
				Session["DAGDBDetails"] = DSTaskSettingsDataTable;
				DAGDBOverviewGridView.DataSource = DSTaskSettingsDataTable;
				DAGDBOverviewGridView.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

        //10/28/2014 NS added for VE-157

        protected void DAGGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DAGHealth|DAGGridView", DAGGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }

        protected void ActivPrefGridView_PageSizeChanged(object sender, EventArgs e)
        {
            //ProfilesGridView.PageIndex;
            VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.UpdateUserPreferences("DAGHealth|ActivPrefGridView", ActivPrefGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
            Session["UserPreferences"] = VSWebBL.ConfiguratorBL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
        }
        private void SetTabImage()
        {
            bool found = false;
            DataTable dt = (DataTable)Session["DAGMembers"];
            DAGPageControl.TabPages[1].TabImage.Url = "";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (((dt.Rows[i][j].ToString()).ToUpper()).Contains("FAIL"))
                            {
                                DAGPageControl.TabPages[1].TabImage.Url = "~/images/icons/exclamation.png";
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            break;
                        }
                    }
                }
            }
            found = false;
            dt = (DataTable)Session["DAGDB"];
            DAGPageControl.TabPages[2].TabImage.Url = "";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Rows[i][j].ToString() != "Healthy" && dt.Columns[j].ColumnName == "ContendIndex")
                            {
                                DAGPageControl.TabPages[2].TabImage.Url = "~/images/icons/exclamation.png";
                                found = true;
                                break;

                            }
                        }
                        if (found)
                        {
                            break;
                        }
                    }
                }
            }
            dt = (DataTable)Session["DAGHealth"];
            DAGPageControl.TabPages[0].TabImage.Url = "";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Rows[i][j].ToString() != "Healthy" && dt.Columns[j].ColumnName != "DatabaseName")
                            {
                                DAGPageControl.TabPages[0].TabImage.Url = "~/images/icons/exclamation.png";
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}