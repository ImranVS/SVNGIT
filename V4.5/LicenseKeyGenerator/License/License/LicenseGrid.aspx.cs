using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL;
using System.Data;
using DevExpress.Web;



namespace License
{
	public partial class LicenseGrid : System.Web.UI.Page
    {
		string fromdate;
		string todate;
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack)
            {

				FillLicenseGrid();
				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "LicenseGrid|LicenseGridView")
						{
							LicenseGridView.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}

					}
				}
					
            }
            else
            {

				FillLicenseGridfromSession();

            }

			
			if (Session["LicenseUpdateStatus"] != null)
			{
				if (Session["LicenseUpdateStatus"].ToString() != "")
				{
					
					successDiv.InnerHtml = "License Key Generator information for <b>" + Session["LicenseUpdateStatus"].ToString() +
						"</b> updated Successfully.<br></br>" +
						"License Key is <b>" + Session["Key"].ToString() +
						 "</b> <button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
					successDiv.Style.Value = "display: block";
					Session["LicenseUpdateStatus"] = "";
				}
			}
        }


		private void FillLicenseGrid()

            {
            try
            {
				
				DataTable LicenseDataTable = new DataTable();

				LicenseDataTable = BL.LicenseKeyBL.Ins.GetAllData(Convert.ToString(Session["UserID"]));
				
				Session["License"] = LicenseDataTable;
				LicenseGridView.DataSource = LicenseDataTable;
                LicenseGridView.DataBind();
                }
            
	
            catch (Exception ex)
            {
                
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                throw ex;
            }
            finally { }
        }
		//protected void LicenseGridView_DataBinding(object sender, EventArgs e)
		//{
		//    if (Session["License"] != null)
		//    {
		//        // Assign the data source in grid_DataBinding
		//        LicenseGridView.DataSource = Session["License"];
		//    }
		//}

		private void FillLicenseGridfromSession()
        {
            try
            {

				DataTable LicenseDataTable = new DataTable();
				if ((Session["License"] != null) && (Session["License"] != ""))
                {
					LicenseDataTable = (DataTable)Session["License"];
					if (LicenseDataTable.Rows.Count > 0)
                    {
						LicenseGridView.DataSource = LicenseDataTable;
                        LicenseGridView.DataBind();
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

		protected void LicenseGridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.EditForm)
            
			
			
			{
                
                try
                {
                    if (e.GetValue("ID") != " " && e.GetValue("ID") != null )
                    {
						ASPxWebControl.RedirectOnCallback("Licensekey.aspx?ID=" + e.GetValue("ID"));
                        Context.ApplicationInstance.CompleteRequest();

                    }
                    else
                    {
						ASPxWebControl.RedirectOnCallback("Licensekey.aspx");
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
                catch (Exception ex)
                {
                    Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
                   
                }
                
            }


			
        }

		protected void LicenseGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
		

        }

        protected void NewButton_Click(object sender, EventArgs e)
        {
          
			Response.Redirect("~/Licensekey.aspx",false );
            Context.ApplicationInstance.CompleteRequest();
        }






	
		protected void LicenseReport_Click(object sender, EventArgs e)
		{
			try
			{
				string value = "";
				DataTable dt = new DataTable();
				DataTable dtSession = (DataTable)Session["License"];

				//dt.Columns.Add("");
				dt.Columns.Add("LoginName");
				dt.Columns.Add("CompanyName");
				dt.Columns.Add("Units");
				dt.Columns.Add("InstallType");
				dt.Columns.Add("LicenseType");
				dt.Columns.Add("CreatedOn");
				dt.Columns.Add("ExpirationDate");
				dt.Columns.Add("LicenseKey");

				ASPxGridView GridView = (ASPxGridView)LicenseGridView;

				for (int i = 0; i < GridView.VisibleRowCount; i++)
				{
					if (GridView.GetRowLevel(i) == GridView.GroupCount)
					{
						object keyValue = GridView.GetRowValues(i, new string[] { GridView.KeyFieldName });
						if (keyValue != null)
						{
							value = keyValue.ToString();
							DataRow[] row = dtSession.Select("ID=" + value);
							foreach (DataRow dr in row)
							{
								dt.ImportRow(dr);
							}
						}
					}
				}

				Session["report"] = dt;
				Response.Redirect("~/LicenseReports.aspx",false);
				Context.ApplicationInstance.CompleteRequest();
			}
			catch
			{
			}
		}
		protected void Audit_Click(object sender, EventArgs e)
		{

			Response.Redirect("~/AuditLicense.aspx",false);
			Context.ApplicationInstance.CompleteRequest();
		}
		//protected void GoButton_Click(object sender, EventArgs e)
		//{
		//    fromdate = dtPick.FromDate;
		//    todate = dtPick.ToDate;
		//    FillLicenseGridforDateRange(fromdate, todate);
			
		//}

			//protected void FillLicenseGridforDateRange(string fromdate, string todate)
			//{
			//    try
			//    {

			//        DataTable LicenseDataTable = new DataTable();

			//        LicenseDataTable = BL.LicenseKeyBL.Ins.GetDateRangeData(fromdate, todate);

			//        Session["License"] = LicenseDataTable;
			//        LicenseGridView.DataSource = LicenseDataTable;
			//        LicenseGridView.DataBind();
			//    }


			//    catch (Exception ex)
			//    {

			//        Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			//        throw ex;
			//    }
			//    finally { }

			//}
			protected void ExpiryButton_Click(object sender, EventArgs e)
			{
				
				FillLicenseGridforThirtydaysExpiry();

			}
			
			private void FillLicenseGridforThirtydaysExpiry()
			{
				try
				{

					DataTable LicenseDataTable = new DataTable();

					LicenseDataTable = BL.LicenseKeyBL.Ins.GetThirtydaysExpiry(Convert.ToString(Session["UserID"]));

					Session["License"] = LicenseDataTable;
					LicenseGridView.DataSource = LicenseDataTable;
					LicenseGridView.DataBind();
				}


				catch (Exception ex)
				{

					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}
				finally { }
			}

			protected void Submit_Click(object sender, EventArgs e)
			{

				int Expiryvalue = Convert.ToInt32(ExpiryTextBox.Text);
				FillLicenseGridforExpirys(Expiryvalue);

			}
			private void FillLicenseGridforExpirys(int Expiryvalue)
			{
				try
				{

					DataTable LicenseDataTable = new DataTable();

					LicenseDataTable = BL.LicenseKeyBL.Ins.GetExpirysData(Expiryvalue);

					Session["License"] = LicenseDataTable;
					LicenseGridView.DataSource = LicenseDataTable;
					LicenseGridView.DataBind();
				}


				catch (Exception ex)
				{

					Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
					throw ex;
				}
				finally { }
			}
			protected void LicenseGridView_PageSizeChanged(object sender, EventArgs e)
			{
				//ProfilesGridView.PageIndex;
				BL.UserPreferencesBL.Ins.UpdateUserPreferences("LicenseGrid|LicenseGridView", LicenseGridView.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
				Session["UserPreferences"] = BL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
			}

    }
}