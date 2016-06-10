using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using System.Data;

namespace VSWebDAL.ConfiguratorDAL
{
    public class MobileUsersDAL
    {

        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
		private static MobileUsersDAL _self = new MobileUsersDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
		public static MobileUsersDAL Ins
        {
            get { return _self; }
        }

       
        public bool InsertData(string DeviceId, int duration)
        {
			
            bool Insert = false;
			try
			{

                   string SqlQuery = "INSERT INTO MobileUserThreshold (DeviceId,SyncTimeThreshold)" +
			   " VALUES('" + DeviceId + "'," + duration.ToString() + ")";


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
		public bool UpdateDataforDashboard(string DeviceId, int duration)
		{
			bool Update = false;
		
			try
			{
				//Sowjanya VSPLUS-2064
				string SqlQuery = "if exists(Select DeviceId from MobileUserThreshold where DeviceId='" + DeviceId + "')"+
					              "Begin" +
			                      " UPDATE MobileUserThreshold SET SyncTimeThreshold = '" + duration + "' WHERE DeviceId = '" + DeviceId + "'" +
			                      " end " +
			                      " else " +
			                      " begin " +
			                      " INSERT INTO MobileUserThreshold (DeviceId,SyncTimeThreshold) VALUES('" + DeviceId + "'," + duration.ToString() + ")"+
			                      " end";
				//string SqlQuery = "UPDATE MobileUserThreshold SET SyncTimeThreshold = '" + duration + "' WHERE DeviceId = '" + DeviceId + "'";


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

        public bool UpdateData(string DeviceId, int syncDuration)
        {
            bool Update = false;
            try
            {
                string SqlQuery = "UPDATE MobileUserThreshold SET SyncTimeThreshold = '"+syncDuration+"' WHERE DeviceId = '"+DeviceId+"'";
                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update=false;
            }
            return Update;
        }

		public Object DeleteData(string DeviceId)
		{
			Object Update;
			try
			{

				string SqlQuery = "Delete MobileUserThreshold Where DeviceId='" + DeviceId + "'";

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

		public DataTable SetGrid()
		{
			DataTable dt = new DataTable();
			try
			{
				string strQuerry = "SELECT distinct UserName,DeviceName,device_type, DeviceID,OS_Type FROM [Traveler_Devices] where deviceid not in(select deviceid from MobileUserThreshold)";
				dt = objAdaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{	
				throw ex;
			}
			return dt;
		}
		public DataTable SetThresholdGrid()
		{
			DataTable dt = new DataTable();
			try
			{
				string strQuerry = "SELECT distinct UserName,DeviceName,device_type, MUT.DeviceId DeviceId,SyncTimeThreshold,OS_Type FROM MobileUserThreshold MUT,Traveler_Devices TD where MUT.DeviceId=TD.DeviceId ";
				dt = objAdaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{	
				throw ex;
			}
			
			return dt;
		}

		public DataTable GetDeviceID(string id)
		{
			
			DataTable dt = new DataTable();
			try
			{

				
             string strQuerry = "Select DeviceId from MobileUserThreshold where DeviceId='" + id + "'";

				dt = objAdaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

    }
}