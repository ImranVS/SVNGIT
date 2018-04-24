using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data;
using System.Runtime.Serialization.Json;
using VSWebDO;

//using Newtonsoft.Json;

namespace VSWebUI.Security
{
	public partial class LocationsEdit : System.Web.UI.Page
	{
		protected void Page_PreInit(object sender, EventArgs e)
		{

			this.Master.contentCallEvent += new EventHandler(Master_ButtonClick);
		}

		private void Master_ButtonClick(object sender, EventArgs e)
		{

		}
		string flag;
		int ID;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				FillCountryCombobox();
			}
			if (Request.QueryString["ID"] != "" && Request.QueryString["ID"] != null)
			{
				flag = "Update";
				ID = int.Parse(Request.QueryString["ID"]);

				if (!IsPostBack)
				{
					fillData(ID);
				}
			}
			else
			{
				flag = "Insert";
			}
			
		}

		protected void fillData(int currID)
		{
			Locations loc = new Locations();
			loc.ID = currID;


			Locations returnLoc = VSWebBL.SecurityBL.LocationsBL.Ins.GetData(loc);
			LocationTextBox.Text = returnLoc.Location;
			CityCombobox.Text = returnLoc.City;
			StateCombobox.Text = returnLoc.State;
			CountryCombobox.Text = returnLoc.Country;

			CountryCombobox.SelectedItem = CountryCombobox.Items.FindByText(CountryCombobox.Text);
			CountryCombobox_SelectedIndexChanged(null, null);
			StateCombobox.SelectedItem = StateCombobox.Items.FindByText(StateCombobox.Text);
			StateCombobox_SelectedIndexChanged(null, null);
			//Citytextbox.SelectedItem = CityCombobox.Items.FindByText(CityCombobox.Text);
			CityCombobox.Text = returnLoc.City.ToString();
		}

		protected void FillCountryCombobox()
		{
			try
			{
				DataTable dt = new DataTable();
				dt = VSWebBL.SecurityBL.LocationsBL.Ins.GetAllCountries();
				CountryCombobox.DataSource = dt;
				CountryCombobox.TextField = "Country";
				CountryCombobox.ValueField = "Country";
				CountryCombobox.DataBind();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }


		}

		protected void CountryCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{

				DataTable dt = new DataTable();
				dt = VSWebBL.SecurityBL.LocationsBL.Ins.GetStatesFromCountry(CountryCombobox.Text);
				StateCombobox.DataSource = dt;
				StateCombobox.TextField = "State";
				StateCombobox.ValueField = "State";
				StateCombobox.DataBind();
				StateCombobox.Focus();
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }


		}

		protected void StateCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{

			try
			{

				String State = StateCombobox.Text;
				String Country = CountryCombobox.Text;

				System.Net.WebClient web = new System.Net.WebClient();
				web.QueryString.Add("Country", Country);
				web.QueryString.Add("State", State);
				string response = web.DownloadString("http://jnitinc.com/WebService/GetCity.php?Country=" + Country + "&State=" + State + "");

				//List<City> ls = deserializeJson<List<LocationValues>>(response);
				List<CityNames> lst = new List<CityNames>();

				DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(lst.GetType());
				System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.Unicode.GetBytes(response));
				lst = (List<CityNames>)jsonSer.ReadObject(ms);

				DataTable dt = new DataTable();
				dt.Columns.Add("City");

				foreach (CityNames city in lst)
				{
					DataRow row = dt.NewRow();
					row["City"] = city.City;
					dt.Rows.Add(row);
				}


				CityCombobox.DataSource = dt;
				CityCombobox.TextField = "City";
				CityCombobox.ValueField = "City";
				CityCombobox.DataBind();
				CityCombobox.Focus();

			
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }










/*

			try
			{

				DataTable dt = new DataTable();
				dt = VSWebBL.SecurityBL.LocationsBL.Ins.GetCitiesFromStateAndCountry(StateCombobox.Text, CountryCombobox.Text);
				
			}
			catch (Exception ex)
			{
				Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
				throw ex;
			}
			finally { }
			*/

		}

		protected void ValidationUpdatedButton_Click(object sender, EventArgs e)
		{
            try
            {
                Locations loc = new Locations();
                if (Request.QueryString["ID"] != "" && Request.QueryString["ID"] != null)
                    loc.ID = Convert.ToInt32(Request.QueryString["ID"].ToString());

                loc.Location = LocationTextBox.Text;
                loc.State = StateCombobox.Text == "" ? null : StateCombobox.Text;
                loc.City = CityCombobox.Text == "" ? null : CityCombobox.Text;
                loc.Country = CountryCombobox.Text == "" ? null : CountryCombobox.Text;

                Object ReturnValue;
                if (Request.QueryString["ID"] == "" || Request.QueryString["ID"] == null)
                    ReturnValue = VSWebBL.SecurityBL.LocationsBL.Ins.InsertData(loc);
                else
                    ReturnValue = VSWebBL.SecurityBL.LocationsBL.Ins.UpdateData(loc);

                if (ReturnValue != "")
                {
                    Session["LocationStatus"] = LocationTextBox.Text;
                    Response.Redirect("AdminTab.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    errorDiv.Style.Value = "display: block";
                    errorDiv.InnerHtml = "Please select a unique Location name." +
                         "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                }
                //UpdateLocationData(loc);
            }
            catch(Exception ex)
            {
              
                errorDiv.InnerHtml = "The following error has occurred: " + ex.Message +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";
                errorDiv.Style.Value = "display: block";
                
                Log.Entry.Ins.WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
		}

		protected void CancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("AdminTab.aspx", false);//Mukund, 05Aug14, VSPLUS-844:Page redirect on callback
			Context.ApplicationInstance.CompleteRequest();
		}
	}

	public class CityNames
	{
		public string City { get; set; }
	}
}