using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;


namespace VSWebUI.Dashboard
{
	public partial class MSMap : System.Web.UI.Page
	{
		String statName = "";
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		//SharpMap.Map map;
		//string DATA_NAME = "World Countries";
		//string DATA_PATH = @"world_adm0\world_adm0.shp";
		//System.Windows.Forms.PictureBox pb = new System.Windows.Forms.PictureBox();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["UserLogin"] == null || Session["UserLogin"] == "")
			{
				MenuForKey.Visible = false;
			}
			if (!IsPostBack)
			{
				string APIKey = "";
				try
				{
					APIKey = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("BingApiKey");
				}
				catch (Exception ex)
				{

				}

				FillOffice365ComboBox();
				if (Request.QueryString["StatName"] != null)
				{

					statName = Request.QueryString["StatName"].ToString();
					Office365StatNameComboBox.Text = statName;

				}
				else
				{
					Office365StatNameComboBox.SelectedIndex = Office365StatNameComboBox.Items.Count > 0 ? 0 : -1;
					statName = Office365StatNameComboBox.Text;
				}

				//MapSelectionRBL.SelectedIndex = 1;

				string Locations = getMapShapes("Country", false);
				Locations += getMapShapes("State", false);
				Locations += getMapShapes("County", false);
				Locations += getMapShapes("City", false);

				Locations += getMapShapes("Country", true);
				Locations += getMapShapes("State", true);
				Locations += getMapShapes("County", true);
				Locations += getMapShapes("City", true);

				Locations += getThresholds();

				//string Locations = getMapShapes("Regional");

				Literal1.Text = @"
			<script type='text/javascript' src='http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0'>
			</script>
			<script type='text/javascript'>
				
				var baseUrl = 'http://platform.bing.com/geo/spatial/v1/public/geodata?SpatialFilter=';
				var creds = '" + APIKey + @"';
				var map = new Microsoft.Maps.Map(document.getElementById('mapDiv'), {
					credentials: creds,
					mapTypeId: Microsoft.Maps.MapTypeId.road,
					showMapTypeSelector: false,
					zoom: 2
				});
				Microsoft.Maps.Events.addHandler(map, 'viewchangeend', viewChanged);
				
				dataLayer = new Microsoft.Maps.EntityCollection();
				map.entities.push(dataLayer);

				infoboxLayer = new Microsoft.Maps.EntityCollection();
				map.entities.push(infoboxLayer);

				infobox =new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(0, 0), { visible: false, height: 100, width: 150, showCloseButton: false });
				//Microsoft.Maps.Events.addHandler(infobox, 'click', function () {window.location.href = 'DeviceTypeList.aspx';}); 
				infoboxLayer.push(infobox);				

				lastZoom=map.getZoom();
 
				function GetMap() {
					//map.entities.clear();
					
					" + Locations + @"
				}

				GetMap();
			</script>";
			}


		}


		public string getMapShapes(string typeOfSearch, bool isOffice365Test)
		{
			string BingIdentifyer = "";
			string Locations = "";

			DataTable dt;

			if (isOffice365Test)
			{
				if (typeOfSearch == "City")
				{
					BingIdentifyer = "PopulatedPlace";
					dt = VSWebBL.DashboardBL.MapBL.Ins.GetLocationInfoO365(statName);
					//dt = VSWebBL.DashboardBL.MapBL.Ins.GetCityLocationInfoO365(statName);
				}
				else if (typeOfSearch == "State")
				{
					BingIdentifyer = "AdminDivision1";
					dt = VSWebBL.DashboardBL.MapBL.Ins.GetLocationInfoO365(statName);
					//dt = VSWebBL.DashboardBL.MapBL.Ins.GetStateLocationInfoO365(statName);
				}
				else if (typeOfSearch == "County")
				{
					BingIdentifyer = "AdminDivision2";
					dt = VSWebBL.DashboardBL.MapBL.Ins.GetLocationInfoO365(statName);
					//dt = VSWebBL.DashboardBL.MapBL.Ins.GetCityLocationInfoO365(statName);
				}
				else if (typeOfSearch == "Regional")
				{
					BingIdentifyer = "AdminDivision1";
					dt = VSWebBL.DashboardBL.MapBL.Ins.GetLocationInfoO365(statName);
				}
				else
				{
					BingIdentifyer = "CountryRegion";
					dt = VSWebBL.DashboardBL.MapBL.Ins.GetLocationInfoO365(statName);
					Locations += "\n numOfCountries = " + dt.Rows.Count;
				}
			}
			else
			{
				if (typeOfSearch == "City")
				{
					BingIdentifyer = "PopulatedPlace";
					dt = VSWebBL.DashboardBL.MapBL.Ins.GetCityLocationInfo();
				}
				else if (typeOfSearch == "State")
				{
					BingIdentifyer = "AdminDivision1";
					dt = VSWebBL.DashboardBL.MapBL.Ins.GetStateLocationInfo();
				}
				else if (typeOfSearch == "County")
				{
					BingIdentifyer = "AdminDivision2";
					dt = VSWebBL.DashboardBL.MapBL.Ins.GetCityLocationInfo();
				}
				else if (typeOfSearch == "Regional")
				{
					BingIdentifyer = "AdminDivision1";
					dt = VSWebBL.DashboardBL.MapBL.Ins.GetCountryLocationInfo();
				}
				else
				{
					BingIdentifyer = "CountryRegion";
					dt = VSWebBL.DashboardBL.MapBL.Ins.GetCountryLocationInfo();
					Locations += "\n numOfCountries = " + dt.Rows.Count;
				}
			}

			foreach (DataRow row in dt.Rows)
			{
				string avg = "";
				string location = "";
				string count = "";
				string Issues = "";
				string NoIssues = "";
				string InMaintenance = "";
				string NotResponding = "";
				string VSLocation = "";
				string LocID = "";

				
				
				location = row["loc"].ToString();
				//count = row["Count"].ToString();
				VSLocation = row["Location"].ToString();
				LocID = row["LocID"].ToString();


				if (!isOffice365Test)
				{
					Issues = row["Issue"].ToString();
					NoIssues = row["OK"].ToString();
					InMaintenance = row["Maintenance"].ToString();
					NotResponding = row["NotResponding"].ToString();
				}
				else
				{


					//avg = row["AvgRT"].ToString();
				}

				Locations = Locations + @"
				var boundaryUrl = baseUrl + " + "\"GetBoundary('" + location + "',1,'" + BingIdentifyer + "',0,0,'en','US')&$format=json&key=\" + creds + \"&jsonp=?\";" + @"
				$.getJSON(boundaryUrl, function(results){
				var arrVals = [];
				var arrCounts = [];
				";

				if (isOffice365Test)
				{
					foreach (DataColumn tCol in row.Table.Columns)
					{
						string colName = tCol.ColumnName;
						if (colName != "loc" && colName != "LocID" && colName != "Location" && colName != "LocationID" && !colName.Contains(" Count"))
						{
							Locations += "arrVals['" + colName + "'] = '" + row[tCol].ToString() + "';\n";
							Locations += "arrCounts['" + colName + "'] = '1';\n";
						}
					}

				}

				Locations += @"
				boundaryCallback(results,'" + location + "','" + BingIdentifyer + @"','" + avg + "','" + count + "','" + Issues + "','" + NoIssues + "','" + InMaintenance + "','" + NotResponding + "','" + VSLocation + "','" + LocID + @"',arrVals, arrCounts);
});
				
				numOfCalls++;
				";
			}

			return Locations;
		}

		public string getThresholds()
		{
			string str = "\n";
			DataTable dt = VSWebBL.DashboardBL.MapBL.Ins.GetThresholds();
			string colName = "";
			if (dt.Rows.Count > 0)
			{
				foreach (DataColumn col in dt.Columns)
				{
					colName = col.ColumnName.ToString();
					if (colName == "ServerId")
						continue;
					str += "Office365TestsThresholds['" + colName + "'] = " + 
						( (dt.Rows[0][colName] == null || dt.Rows[0][colName].ToString() == "") ? "0" : dt.Rows[0][col.ColumnName.ToString()].ToString()) + 
						"; \n";
				}
			}

			return str;
		}

		protected void MenuForKey_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
		{
			if (e.Item.Name == "ChangeKey")
			{
				Response.Redirect("~/Configurator/UserPreferences.aspx");
				Context.ApplicationInstance.CompleteRequest();
			}
		}

		private void FillOffice365ComboBox()
		{
			DataTable CredentialsDataTable = VSWebBL.DashboardBL.MapBL.Ins.GetUniqueOffice365Tests();
			Office365StatNameComboBox.DataSource = CredentialsDataTable;
			Office365StatNameComboBox.TextField = "tests";
			Office365StatNameComboBox.ValueField = "tests";
			Office365StatNameComboBox.DataBind();

		}

		protected void Office365StatNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				Response.Redirect("~/Dashboard/MSMap.aspx?StatName=" + Office365StatNameComboBox.Text);
				Context.ApplicationInstance.CompleteRequest();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }


		}

	}
}