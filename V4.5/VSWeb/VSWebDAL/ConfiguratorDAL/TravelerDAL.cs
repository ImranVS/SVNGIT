using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
    public class TravelerDAL
    {
        private AdaptorforDsahBoard objAdaptorDB = new AdaptorforDsahBoard();
        private Adaptor objAdaptorCfg = new Adaptor();
        private static TravelerDAL _self = new TravelerDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static TravelerDAL Ins
        {
            get { return _self; }
        }

        public DataTable GetTravelerHADataStore()
        {
            DataTable dt = new DataTable();
            try
            {
                //7/9/2014 NS modified for VSPLUS-806
                /*string q = "SELECT [ID],[TravelerServicePoolName],[ServerName],[DataStore],[Port],[UserName], " +
                    "[Password],CASE WHEN [IntegratedSecurity]='False' THEN 'No' ELSE 'Yes' END IntegratedSecurity, " +
                    "[TestScanServer],[UsedByServers] FROM Traveler_HA_Datastore ORDER BY TravelerServicePoolName ";
                 */
                string q = "SELECT [ID],[TravelerServicePoolName],[ServerName],[DataStore],[Port],[UserName], " +
                    "[Password],CASE WHEN [IntegratedSecurity]='False' THEN 'SQL Server' ELSE 'Windows' END IntegratedSecurity, " +
                    "[TestScanServer],[UsedByServers],[DatabaseName] FROM Traveler_HA_Datastore ORDER BY TravelerServicePoolName ";
                dt = objAdaptorCfg.FetchData(q);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

        public Object UpdateTravelerDataStoreData(TravelerDS TravelerObject)
        {
            Object Update;
            string port = "";
            try
            {
                port = TravelerObject.Port;
                if (port == "")
                {
                    port = "NULL";
                }
                //7/18/2014 NS modified for VSPLUS-806
                /*
                string SqlQuery = "UPDATE Traveler_HA_Datastore SET TravelerServicePoolName='" + TravelerObject.TravelerPoolName + "', " +
                    "ServerName='" + TravelerObject.ServerName + "', DataStore='" + TravelerObject.DataStore + "', " +
                    "Port=" + port + ", UserName='" + TravelerObject.UserName + "', " +
                    "Password='" + TravelerObject.Password + "', IntegratedSecurity='" + TravelerObject.IntegratedSecurity + "', " +
                    "TestScanServer='" + TravelerObject.TestScan + "', UsedbyServers='" + TravelerObject.UsedByServers + "' " +
                    "WHERE ID=" + TravelerObject.ID.ToString();
                 */
                string SqlQuery = "UPDATE Traveler_HA_Datastore SET TravelerServicePoolName='" + TravelerObject.TravelerPoolName + "', " +
                    "ServerName='" + TravelerObject.ServerName + "', DataStore='" + TravelerObject.DataStore + "', " +
                    "Port=" + port + ", UserName='" + TravelerObject.UserName + "', " +
                    "Password='" + TravelerObject.Password + "', IntegratedSecurity='" + TravelerObject.IntegratedSecurity + "', " +
                    "TestScanServer='" + TravelerObject.TestScan + "', UsedbyServers='" + TravelerObject.UsedByServers + "', " +
                    "DatabaseName='" + TravelerObject.DatabaseName + "' " +
                    "WHERE ID=" + TravelerObject.ID.ToString();
                Update = objAdaptorCfg.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            return Update;
        }

        public Object InsertTravelerDataStoreData(TravelerDS TravelerObject)
        {
            Object Update;
            try
            {
                //7/18/2014 NS modified for VSPLUS-806
                /*
                string SqlQuery = "INSERT INTO Traveler_HA_Datastore VALUES ('" + TravelerObject.TravelerPoolName + "', " +
                    "'" + TravelerObject.ServerName + "', '" + TravelerObject.DataStore + "', " +
                    TravelerObject.Port + ",'" + TravelerObject.UserName + "', " +
                    "'" + TravelerObject.Password + "', '" + TravelerObject.IntegratedSecurity + "', " +
                    "'" + TravelerObject.TestScan + "', '" + TravelerObject.UsedByServers + "')";
                 */
                string SqlQuery = "INSERT INTO Traveler_HA_Datastore VALUES ('" + TravelerObject.TravelerPoolName + "', " +
                    "'" + TravelerObject.ServerName + "', '" + TravelerObject.DataStore + "', " +
                    TravelerObject.Port + ",'" + TravelerObject.UserName + "', " +
                    "'" + TravelerObject.Password + "', '" + TravelerObject.IntegratedSecurity + "', " +
                    "'" + TravelerObject.TestScan + "', '" + TravelerObject.UsedByServers + "', " +
                    "'" + TravelerObject.DatabaseName + "')";
                Update = objAdaptorCfg.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            return Update;
        }

        public Object DeleteTravelerDataStoreData(TravelerDS TravelerObject)
        {
            Object Update;
            try
            {
                string SqlQuery = "DELETE FROM [Traveler_HA_Datastore] WHERE [ID]=" + TravelerObject.ID + "";
                Update = objAdaptorCfg.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            return Update;
        }

        public DataTable GetTravelerTestWhenScan()
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT DISTINCT ServerName TestScanServer FROM [Traveler_Status] ORDER BY ServerName";
                dt = objAdaptorCfg.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }
		public Object UpdateTravelerPassword(TravelerDS TravelerObject)
		{
			Object Update;

			try
			{
				string SqlQuery = "UPDATE Traveler_HA_Datastore SET Password='" + TravelerObject.Password + "' WHERE ID=" + TravelerObject.ID;


				Update = objAdaptorCfg.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Update = false;
			}
			return Update;
		}
		public DataTable GetValuebyID(TravelerDS TravelerObject)
		{
			DataTable dt = new DataTable();
			TravelerDS ReturnTravelersObject = new TravelerDS();
			try
			{
				string q = "Select * from [Traveler_HA_Datastore] where ID=" + TravelerObject.ID + "";
				dt = objAdaptorCfg.FetchData(q);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{

			}
			return dt;
		}

    }
}
