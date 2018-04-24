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
	public partial class EstimateLicenses : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				FillEstimateLicensesGrid();
				if (Session["UserPreferences"] != null)
				{
					DataTable UserPreferences = (DataTable)Session["UserPreferences"];
					foreach (DataRow dr in UserPreferences.Rows)
					{
						if (dr[1].ToString() == "EstimateLicenses|EstimateLicensesGrid")
						{
							EstimateLicensesGrid.SettingsPager.PageSize = Convert.ToInt32(dr[2]);
						}
					}
				}

			}
			else
			{

				FillEstimateLicensesGridfromSession();

			}
		}
		private void FillEstimateLicensesGrid()
		{
			try
			{

				DataTable LicenseDataTable = new DataTable();

				LicenseDataTable = BL.LicenseKeyBL.Ins.GetEstimateLicensesData();
				//string testing = LicenseDataTable.Rows[0][0].ToString();

				Session["EstimateLicenses"] = LicenseDataTable;
				EstimateLicensesGrid.DataSource = LicenseDataTable;
				EstimateLicensesGrid.DataBind();
			}


			catch (Exception ex)
			{

				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
		}

		private void FillEstimateLicensesGridfromSession()
		{
			try
			{

				DataTable LicenseDataTable = new DataTable();
				if ((Session["EstimateLicenses"] != null) && (Session["EstimateLicenses"] != ""))
				{
					LicenseDataTable = (DataTable)Session["EstimateLicenses"];
					if (LicenseDataTable.Rows.Count > 0)
					{


						//GridViewDataColumn column3 = EstimateLicensesGrid.Columns["UnitCost"] as GridViewDataColumn;
						//GridViewDataColumn column4 = EstimateLicensesGrid.Columns["noofservers"] as GridViewDataColumn;
						//GridViewDataColumn column5 = EstimateLicensesGrid.Columns["totalunits"] as GridViewDataColumn;
						//int startIndex = EstimateLicensesGrid.PageIndex * EstimateLicensesGrid.SettingsPager.PageSize;
						//int endIndex = Math.Min(EstimateLicensesGrid.VisibleRowCount, startIndex + EstimateLicensesGrid.SettingsPager.PageSize);
						//for (int i = startIndex; i < endIndex; i++)
						//{
						//    //ASPxTextBox unitcost = (ASPxTextBox)EstimateLicensesGrid.( "UnitCost" ,i);
						//    int unitcost = Convert.ToInt32(LicenseDataTable.Rows[i]["UnitCost"].ToString());
						//    //ASPxTextBox test = new ASPxTextBox();
						//    //unitcost = int.Parse(test.Text);

						//    ASPxTextBox noofservers = (ASPxTextBox)EstimateLicensesGrid.FindRowCellTemplateControl(i, column4, "noofservers");
						//    ASPxTextBox totalunits = (ASPxTextBox)EstimateLicensesGrid.FindRowCellTemplateControl(i, column5, "totalunits");
						//    int noofsrvrs= int.Parse(noofservers.Text);
						//    if (unitcost != null && noofservers != null)
						//    {
						//        totalunits.Text = (unitcost * noofsrvrs).ToString();
						//        //unitcost.Attributes["onKeyup"] = "javascript: return multiplication('" + Convert.ToInt32(unitcost.Text) + "','" + Convert.ToInt32(noofservers.Text) + "','" + Convert.ToInt32(totalunits.Text) + "')";
						//    }


						//}
						EstimateLicensesGrid.DataSource = LicenseDataTable;
						EstimateLicensesGrid.DataBind();
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
		
		protected void EstimateLicensesGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{



			DataTable LicenseDataTable = new DataTable();
			if ((Session["EstimateLicenses"] != null) && (Session["EstimateLicenses"] != ""))
			{
				LicenseDataTable = (DataTable)Session["EstimateLicenses"];
				if (LicenseDataTable.Rows.Count > 0)
				{

					ASPxGridView EstimateLicensesGrid = (ASPxGridView)sender;
					GridViewDataColumn column3 = EstimateLicensesGrid.Columns["UnitCost"] as GridViewDataColumn;
					GridViewDataColumn column4 = EstimateLicensesGrid.Columns["noofservers"] as GridViewDataColumn;
					GridViewDataColumn column5 = EstimateLicensesGrid.Columns["totalunits"] as GridViewDataColumn;
					int startIndex = EstimateLicensesGrid.PageIndex * EstimateLicensesGrid.SettingsPager.PageSize;
					int endIndex = Math.Min(EstimateLicensesGrid.VisibleRowCount, startIndex + EstimateLicensesGrid.SettingsPager.PageSize);
					for (int i = startIndex; i < endIndex; i++)
					{

						int unitcost = Convert.ToInt32(LicenseDataTable.Rows[i]["UnitCost"].ToString());


						ASPxTextBox noofservers = (ASPxTextBox)EstimateLicensesGrid.FindRowCellTemplateControl(i, column4, "noofservers");
						ASPxTextBox totalunits = (ASPxTextBox)EstimateLicensesGrid.FindRowCellTemplateControl(i, column5, "totalunits");

						if (noofservers != null)
						{
							noofservers.Text = (noofservers.Text == "" ? "0" : noofservers.Text);
							int noofsrvrs = int.Parse(noofservers.Text);

							if (unitcost != null)
							{

								//totalunits.Text = (unitcost * noofsrvrs).ToString();
								//noofservers.Attributes["onChange"] = "javascript: return multiplication('" + Convert.ToInt32(unitcost) + "','" + Convert.ToInt32(noofservers.Text) + "','" + Convert.ToInt32(totalunits.Text) + "')";
								noofservers.Attributes["onChange"] = "javascript: return multiplication('" + Convert.ToInt32(unitcost) + "',this,this,'"+endIndex+"')";
							}

						}
					}

				
				}
			}
		}

		protected void EstimateLicensesGrid_PageSizeChanged(object sender, EventArgs e)
		{
			//ProfilesGridView.PageIndex;
			BL.UserPreferencesBL.Ins.UpdateUserPreferences("EstimateLicenses|EstimateLicensesGrid", EstimateLicensesGrid.SettingsPager.PageSize.ToString(), Convert.ToInt32(Session["UserID"]));
			Session["UserPreferences"] = BL.UserPreferencesBL.Ins.GetUserRowPrefrenceDetails(Convert.ToInt32(Session["UserID"]));
		}
	}
}
