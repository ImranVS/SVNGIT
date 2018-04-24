using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
   public class MaintenanceWindowsDAL
    {

        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static MaintenanceWindowsDAL _self = new MaintenanceWindowsDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static MaintenanceWindowsDAL Ins
        {
            get { return _self; }
        }

        /// <summary>
        /// Get Data from MaintenanceWindows based on ServerName
        /// </summary>
        /// <param name="ServerName"></param>
        /// <returns></returns>
        public DataTable MaintenanceWindowsUpdateGrid(string ServerName)
        {
            DataTable MaintenanceWindowsDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT MaintWindow, ID FROM MaintenanceWindows " +
                  "WHERE Name='" + ServerName + "' AND DeviceType='Domino' ORDER BY Name";

                MaintenanceWindowsDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return MaintenanceWindowsDataTable;
        }
        /// <summary>
        /// Insert data into MaintenanceWindows table
        /// </summary>
        /// <param name="DSObject">MaintenanceWindows object</param>
        /// <returns></returns>
        public bool InsertData(MaintenanceWindows MWObject)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO MaintenanceWindows( DeviceType, Name, MaintWindow ) " +
            "VALUES ('" + MWObject.DeviceType + "', '" + MWObject.Name + "', '" + MWObject.MaintWindow + "')";

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
        /// Delete data of MaintenanceWindows  table
        /// </summary>
        /// <param name="objDominoServers">MaintenanceWindows  object</param>
        /// <returns></returns>
        public Object DeleteData(MaintenanceWindows MWObject)
        {
            Object Update;
            try
            {

                string SqlQuery = "Delete MaintenanceWindows Where MaintWindow='" + MWObject.MaintWindow +"'";

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

        //9/16/2014 NS added for VSPLUS-921
        public void DeletePastMaintWin()
        {
            bool Update = false;
            try
            {
                string SqlQuery = "DELETE FROM Maintenance WHERE EndDate <= DATEADD(dd,-1,GETDATE()) ";
                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
