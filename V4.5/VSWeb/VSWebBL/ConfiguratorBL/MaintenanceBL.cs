using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebBL.ConfiguratorBL
{
    public class MaintenanceBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static MaintenanceBL _self = new MaintenanceBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static MaintenanceBL Ins
        {
            get { return _self; }
        }

        //12/7/2015 NS modified for VSPLUS-2227
        public bool UpdateMaintenanceWindows(string ID, string WinName, string StartDate, string StartTime, string Duration,
            string EndDate, string MaintType, string MaintDaysList, string EndDateIndicator, List<string> ServerIDs, string recurinfo,
            string reminderinfo, int label, bool allday, List<string> ServerTypeIDs, string copy, string Durationtype,
            List<string> DeviceIDs = null)
        {
           return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.UpdateMaintenanceWindows(ID,WinName,StartDate,StartTime,Duration,
            EndDate,MaintType,MaintDaysList,EndDateIndicator,ServerIDs,recurinfo,reminderinfo,label,allday,ServerTypeIDs,copy, 
            Durationtype,DeviceIDs);
        }
        public bool delete(Maintenance m)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.delete(m);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetMaintenanceWindow(string WinID, string WinName)
        {

			try
			{
				return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.GetMaintenanceWindow(WinID, WinName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			} 
        }

        public DataTable GetServerMaintenance(string maintID)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.GetServerMaintenance(maintID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetServers(string MaintKey)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.GetServers(MaintKey);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
       //added on june-16
        public DataTable GetAllMaintenData()
        {
			try
			{
				  return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.GetAllMaintenData();
     
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		}
          
        public DataTable GetMaintenDataOnServerID(string ServerId)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.GetMaintenDataOnServerID(ServerId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetMaintenCalDataOnServerID(string ServerId)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.GetMaintenCalanderDataOnServerID(ServerId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}

           
        }

        //June 27
        public DataTable GetMaintenDataOnServerIDs(string ServerId1,string ServerId2,string ServerId3)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.GetMaintenDataOnServerIDs(ServerId1, ServerId2, ServerId3);
			}
			catch (Exception ex)
			{
				
				throw ex; 
			}
           
        }

        //july16

        public string GetName(string ID)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.GetName(ID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

           public DataTable nametable(string Name,string ID)
           {
			   try
			   {
				   return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.nametable(Name, ID);
			   }
			   catch (Exception ex)
			   {
				   
				   throw ex;
			   }
              
           }
           public DataTable servermaintenance(DateTime current,int hrs,int week)
           {
			   try
			   {
				   return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.GettodaysServersfromMaintenance(current, hrs, week);
			   }
			   catch (Exception ex)
			   {
				   
				   throw ex;
			   }
               
           }

           public DataTable Getmaintenanceserversbysearch(string fromdate, string todate, string fromtime, string totime)
           {
			   try
			   {
				   return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.GetmaintenanceServersbysearch(fromdate, todate, fromtime, totime);
			   }
			   catch (Exception ex)
			   {
				   
				   throw ex;
			   }
             
           }


           public DataTable Getmaintenanceserversbyservername(string Servername, string fromdate, string todate, string fromtime, string totime)
           {
			   try
			   {
				   return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.GetmaintenanceserversbyServername(Servername, fromdate, todate, fromtime, totime);
			   }
			   catch (Exception ex)
			   {
				   
				   throw ex;
			   }
              
           }
           public DataTable GetSelectedServers(int MaintID)
           {
			   try
			   {
				   return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.GetSelectedServers(MaintID);
			   }
			   catch (Exception ex)
			   {
				   
				   throw ex;
			   }
              
           }

        //11/30/2015 NS added for VSPLUS-2227
           public DataTable GetKeyMobileUsers(string MaintKey)
           {
               try
               {
                   return VSWebDAL.ConfiguratorDAL.MaintenanceDAL.Ins.GetKeyMobileUsers(MaintKey);
               }
               catch (Exception ex)
               {

                   throw ex;
               }
           }
    }
}