using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using System.Data;

namespace VSWebDAL.ConfiguratorDAL
{
   public class MaintenanceSettingsDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static MaintenanceSettingsDAL _self = new MaintenanceSettingsDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static MaintenanceSettingsDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get total Data
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllData()
        {
            DataTable DSTasksDataTable = new DataTable();

            try
            {
                string SqlQuery = "Select * from MaintenanceSettings";
                DSTasksDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return DSTasksDataTable;
        }


        public DataTable MaintenanceWindowsUpdateGrid(string ServerName)
        {
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception ex)
			{
				throw ex;
			} 
        }
    }
}
