using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.SecurityDAL
{
	public class ProfilesNamesDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
		private static ProfilesNamesDAL _self = new ProfilesNamesDAL();

		public static ProfilesNamesDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from LocationsDAL
        /// </summary>

        public DataTable GetAllData()
        {

			DataTable ProfileNamesDataTable = new DataTable();
			ProfileNames ReturnLOCbject = new ProfileNames();
            try
            {
                //3/19/2014 NS modified for VSPLUS-484
				string SqlQuery = "SELECT * FROM ProfileNames ORDER BY ProfileName";

				ProfileNamesDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
			return ProfileNamesDataTable;
        }
        /// <summary>
        /// Get Data from DominoServers based on Key
        /// </summary>
		public ProfileNames GetData1(ProfileNames LOCbject)
        {
			DataTable ProfileNamesDataTable = new DataTable();
			ProfileNames ReturnLOCbject = new ProfileNames();
            try
            {
				string SqlQuery = "Select * from ProfileNames where [ID]=" + LOCbject.ID.ToString();
				ProfileNamesDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
				ReturnLOCbject.ProfileName = ProfileNamesDataTable.Rows[0]["ProfileName"].ToString();
              
            }
            catch
            {
            }
            finally
            {
            }
            return ReturnLOCbject;
        }
		public ProfileNames GetDataForLocation1(ProfileNames LOCbject)
        {
			DataTable ProfilesNamesDataTable = new DataTable();
			ProfileNames ReturnLOCbject = new ProfileNames();
            object response = "";
            try
            {
				string SqlQuery = "Select * from ProfileNames where ProfileName='" + LOCbject.ProfileName + "'";
				ProfilesNamesDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
				if (ProfilesNamesDataTable.Rows.Count > 0)
                {
					if (ProfilesNamesDataTable.Rows[0]["ID"].ToString() != "")
                    {
						ReturnLOCbject.ID = int.Parse(ProfilesNamesDataTable.Rows[0]["ID"].ToString());
                        
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

		public bool InsertData1(ProfileNames LOCbject)

        {

            bool Insert = false;
            try
            {
				string SqlQuery = "INSERT INTO ProfileNames (ProfileName) VALUES('" + LOCbject.ProfileName + "')";
                   

                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
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
		public Object UpdateData1(ProfileNames LOCbject)
        {
            Object Update;
            try
            {
				string SqlQuery = "UPDATE ProfileNames SET ProfileName='" + LOCbject.ProfileName + "'WHERE ID = " + LOCbject.ID + "";
                    
                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
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

		public Object DeleteData1(ProfileNames LOCbject)
        {
            Object Update;
            try
            {

				string SqlQuery = "Delete ProfileNames Where ID=" + LOCbject.ID;

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
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

		public DataTable GetDataForProfilesByname1(ProfileNames LOCbject)
        {

			DataTable ProfileNamesDataTable = new DataTable();
            try
            {
				string SqlQuery = "Select * from ProfileNames where ProfileName=('" + LOCbject.ProfileName + "')";
				ProfileNamesDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
            }
            catch
            {
            }
            finally
            {
            }
			return ProfileNamesDataTable;
        }

		public bool GetDataCopy(string profilename)
		{
			bool Insert = false;
            try
            {
			
			string SqlQuery = "INSERT INTO ProfileNames (ProfileName) VALUES('" + profilename + "')";
			Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Insert = false;
			}
			finally
			{
			}
			return true;
		}
		public DataTable GetServersByLocation(ProfileNames LocName)
		{
		    DataTable ServersByLocation = new DataTable();
		    try
		    {
		        //2/5/2014 NS modified the query - added sort by server name
		        string Query = "Select ID,ServerName,(Select ProfileName from ProfileNames where ID=Servers.ProfileNameID) " +
		            "as ProfileName,(select ServerType from ServerTypes where ID=Servers.ServerTypeID) as ServerType " +
		            "from Servers where ProfileNameID in(Select ID from ProfileNames where ProfileName='" + LocName.ProfileName  + "') " +
		            "order by ServerName";
		
		        ServersByLocation = objAdaptor.FetchData(Query);

		    }
		    catch
		    {
		    }
		    finally
		    {
		    }
		    return ServersByLocation;
		}

		public DataTable GetServersByLocation1(string LocName)
		{
		    DataTable ServersByLocation = new DataTable();
		    try
		    {
		        //2/5/2014 NS modified the query - added sort by server name
		        string Query = "Select ID,ServerName,(Select Location from Locations where ID=Servers.LocationID) " +
		            "as Location,(select ServerType from ServerTypes where ID=Servers.ServerTypeID) as ServerType " +
		            "from Servers where LocationID in(Select ID from Locations where Location='" + LocName + "') " +
		            "order by ServerName";
				ServersByLocation = objAdaptor.FetchData(Query);

		    }
		    catch
		    {
		    }
		    finally
		    {
		    }
			return ServersByLocation;
		}
		public DataTable GetValuebyID(ProfileNames ProfileObject)
		{
			DataTable dtv = new DataTable();
			ProfileNames ReturnTravelersObject = new ProfileNames();
			try
			{
				string q = "Select * from ProfileNames where ID=" + ProfileObject.ID + "";
				dtv = objAdaptor.FetchData(q);
			}
			catch
			{
			}
			finally
			{

			}
			return dtv;
		}


    }
}
