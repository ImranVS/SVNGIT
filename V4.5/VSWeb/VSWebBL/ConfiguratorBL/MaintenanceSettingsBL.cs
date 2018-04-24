using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDAL;
using VSWebDO;
using System.Data;

namespace VSWebBL.ConfiguratorBL
{
   public class MaintenanceSettingsBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
       private static MaintenanceSettingsBL _self = new MaintenanceSettingsBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
       public static MaintenanceSettingsBL Ins
        {
            get { return _self; }
        }

       /// <summary>
       ///Call DAL GetAllData
       /// </summary>
       /// <returns></returns>
       public DataTable GetAllData()
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.MaintenanceSettingsDAL.Ins.GetAllData();
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
           
       }


       public DataTable MaintenanceWindowsUpdateGrid(string ServerName)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.MaintenanceSettingsDAL.Ins.MaintenanceWindowsUpdateGrid(ServerName);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }
    }
}
