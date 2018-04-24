using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

//using Newtonsoft.Json;

namespace VSWebDAL.SecurityDAL
{
   public class LocationsDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static LocationsDAL _self = new LocationsDAL();

        public static LocationsDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from LocationsDAL
        /// </summary>

        public DataTable GetAllData()
        {

            DataTable LocationsDataTable = new DataTable();
            Locations ReturnLOCbject = new Locations();
            try
            {
                //3/19/2014 NS modified for VSPLUS-484
                string SqlQuery = "SELECT * FROM [Locations] ORDER BY Location";

                LocationsDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return LocationsDataTable;
        }
        /// <summary>
        /// Get Data from DominoServers based on Key
        /// </summary>
        public Locations GetData(Locations LOCbject)
        {
            DataTable LocationsDataTable = new DataTable();
            Locations ReturnLOCbject = new Locations();
            try
            {
                string SqlQuery = "Select * from Locations where [ID]=" + LOCbject.ID.ToString();
                LocationsDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                ReturnLOCbject.Location = LocationsDataTable.Rows[0]["Location"].ToString();
				ReturnLOCbject.City = LocationsDataTable.Rows[0]["City"].ToString();
				ReturnLOCbject.Country = LocationsDataTable.Rows[0]["Country"].ToString();
				ReturnLOCbject.State = LocationsDataTable.Rows[0]["State"].ToString();
            }
            catch
            {
            }
            finally
            {
            }
            return ReturnLOCbject;
        }
        public Locations GetDataForLocation(Locations LOCbject)
        {
            DataTable LocationsDataTable = new DataTable();
            Locations ReturnLOCbject = new Locations();
            object response = "";
            try
            {
                string SqlQuery = "Select * from Locations where Location='" + LOCbject.Location + "'";
                LocationsDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                if (LocationsDataTable.Rows.Count > 0)
                {
                    if (LocationsDataTable.Rows[0]["ID"].ToString() != "")
                    {
                        ReturnLOCbject.ID = int.Parse(LocationsDataTable.Rows[0]["ID"].ToString());
                        
                    }


                }
                else
                {
                    response = 0;
                }
            }
            catch
            {
            }
            finally
            {
            }
            return ReturnLOCbject;
        }

        /// <summary>
        /// Insert data into Locations table
        /// </summary>
        /// <param name="DSObject">Locations object</param>
        /// <returns></returns>

        public bool InsertData(Locations LOCbject)
        {
            //4/5/2016 Sowjanya modified for VSPLUS-2497
            bool Insert = false;
          
            int paramnum = 0;
            string[] paramnames = new string[4];
            string[] paramvalues = new string[4];
            try
            {
                paramnum = 4;
                paramnames[0] = "@Location";
                paramnames[1] = "@Country";
                paramnames[2] = "@State";
                paramnames[3] = "@City";


                paramvalues[0] = LOCbject.Location;
                paramvalues[1] = LOCbject.Country;
                paramvalues[2] = LOCbject.State;
                paramvalues[3] = LOCbject.City;

                string SqlQuery = "INSERT INTO Locations (Location,Country,State,City)" +

                    "VALUES(" + paramnames[0] + "," + paramnames[1] + "," + paramnames[2] + "," + paramnames[3] + ")";
                  


                Insert = objAdaptor.ExecuteQueryWithParams(SqlQuery, paramnum, paramnames, paramvalues);
            }
            catch
            {
                Insert = false;
            }
            finally
            {
            }
            return Insert;
        }
        /// <summary>
        /// Update data into Locations table
        /// </summary>
        /// <param name="LOCbject">Locations object</param>
        /// <returns></returns>
        public Object UpdateData(Locations LOCbject)
        {
            //4/5/2016 Sowjanya modified for VSPLUS-2497
            Object Update;
           
            int paramnum = 0;
            string[] paramnames = new string[5];
            string[] paramvalues = new string[5];
            try
            {
                paramnum = 5;
                paramnames[0] = "@Location";
                paramnames[1] = "@Country";
                paramnames[2] = "@State";
                paramnames[3] = "@City";
                paramnames[4] = "@ID";


                paramvalues[0] = LOCbject.Location;
                paramvalues[1] = LOCbject.Country;
                paramvalues[2] = LOCbject.State;
                paramvalues[3] = LOCbject.City;
                paramvalues[4] = Convert.ToString( LOCbject.ID);

                string SqlQuery = "UPDATE Locations SET Location=" + paramnames[0] + ",Country=" + paramnames[1] +
                  ",State=" + paramnames[2] + ",City=" + paramnames[3] + " WHERE ID = " + paramnames[4];
                  
                Update = objAdaptor.ExecuteQueryWithParams(SqlQuery, paramnum, paramnames, paramvalues);

            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }
        //delete Data from Locations Table

        public string DeleteData(Locations LOCbject)
        {
            string Update;
            try
            {

                string SqlQuery = "Delete Locations Where ID=" + LOCbject.ID;
				Update = objAdaptor.ExecuteNonQuerynotreturn(SqlQuery);

            }
			catch (Exception)
            {
                Update = "false";

            }
            finally
            {
            }
            return Update;
        }

        public DataTable GetDataForLocationByname(Locations LOCbject)
        {
            DataTable LocationsDataTable = new DataTable();
            
            
            try
            {
				string SqlQuery;
				if(LOCbject.ID == null)
					SqlQuery = "Select * from Locations where Location='" + LOCbject.Location + "'";
				else
					SqlQuery = "Select * from Locations where Location='" + LOCbject.Location + "' and ID <> " + LOCbject.ID;
                LocationsDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
               
               
            }
            catch
            {
            }
            finally
            {
            }
            return LocationsDataTable;
        }

		public DataTable GetAllCountries()
		{
			DataTable LocationsDataTable = new DataTable();
			Locations ReturnLOCbject = new Locations();
			try
			{
				//3/19/2014 NS modified for VSPLUS-484
				string SqlQuery = "SELECT distinct Country, (Case Country WHEN 'United States' then 1 else 0 end) weight from ValidLocations where Country <> '' ORDER BY weight desc,Country";

				LocationsDataTable = objAdaptor.FetchData(SqlQuery);
				
				//System.Net.WebClient web = new System.Net.WebClient();
				//string response = web.DownloadString("http://localhost:53156/LocationService/LocationService.svc/getCountries");
				//string testStr = "[{\"Location\":\"United States\"},{\"Location\":\"United States 1\"},{\"Location\":\"United States 2\"},{\"Location\":\"United States 3\"}]";
				//LocationsDataTable = (DataTable)JsonConvert.DeserializeObject(response, typeof(DataTable));


			}
			catch
			{
			}
			finally
			{
			}
			return LocationsDataTable;
		}

		public DataTable GetStatesFromCountry(string Country)
		{
			DataTable LocationsDataTable = new DataTable();
			Locations ReturnLOCbject = new Locations();
			try
			{
				//3/19/2014 NS modified for VSPLUS-484
				string SqlQuery = "SELECT distinct State FROM ValidLocations where State <> '' AND Country = '" + Country + "' ORDER BY State";

				LocationsDataTable = objAdaptor.FetchData(SqlQuery);

				//System.Net.WebClient web = new System.Net.WebClient();
				//web.QueryString.Add("Country", Country);
				//string response = web.DownloadString("http://localhost:53156/LocationService/LocationService.svc/getStates");
				//LocationsDataTable = (DataTable)JsonConvert.DeserializeObject(response, typeof(DataTable));

			}
			catch
			{
			}
			finally
			{
			}
			return LocationsDataTable;
		}

		/*public DataTable GetCitiesFromStateAndCountry(string State, string Country)
		{
			DataTable LocationsDataTable = new DataTable();
			Locations ReturnLOCbject = new Locations();
			try
			{
				//3/19/2014 NS modified for VSPLUS-484
				//string SqlQuery = "SELECT distinct City FROM ValidLocations where City <> '' AND State = '" + State + "' AND Country='" + Country + "' ORDER BY City";

				//LocationsDataTable = objAdaptor.FetchData(SqlQuery);

				System.Net.WebClient web = new System.Net.WebClient();
				web.QueryString.Add("Country", Country);
				web.QueryString.Add("State", State);
				string response = web.DownloadString("http://web.njit.edu/~wgs4/workTestPHP.php?Country=" + Country + "&State=" + State + "");

				//List<City> ls = deserializeJson<List<LocationValues>>(response);


				DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(T));
				using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result)))
				{
					ms.Position = 0;
					return (T)jsonSer.ReadObject(ms);
				}


				LocationsDataTable = (DataTable)JsonConvert.DeserializeObject(response, typeof(DataTable));

			}
			catch
			{
			}
			finally
			{
			}
			return LocationsDataTable;
		}
	   */
    }

  
}
