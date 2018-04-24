using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDAL;
using VSWebDO;
using System.Data;

namespace VSWebBL.ConfiguratorBL
{
  public  class MaintenanceWindowsBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static MaintenanceWindowsBL _self = new MaintenanceWindowsBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static MaintenanceWindowsBL Ins
        {
            get { return _self; }
        }

        /// <summary>
        ///  Call to Get Data from MaintenanceWindows based on ServerName
        /// </summary>
        /// <param name="ServerName"></param>
        /// <returns></returns>
        public DataTable MaintenanceWindowsUpdateGrid(string ServerName)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MaintenanceWindowsDAL.Ins.MaintenanceWindowsUpdateGrid(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         

        }
        /// <summary>
        /// Call to Insert Data into DominoServers
        /// </summary>
        /// <param name="DominoServersObject">DominoServers object</param>
        /// <returns></returns>
        public bool InsertData(MaintenanceWindows MWObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MaintenanceWindowsDAL.Ins.InsertData(MWObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        /// <summary>
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="STSettingsObject"></param>
        /// <returns></returns>
        public Object DeleteData(MaintenanceWindows MWObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MaintenanceWindowsDAL.Ins.DeleteData(MWObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        //9/16/2014 NS added for VSPLUS-921
        public void DeletePastMaintWin()
        {
			try
			{
				VSWebDAL.ConfiguratorDAL.MaintenanceWindowsDAL.Ins.DeletePastMaintWin();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

    }
}
