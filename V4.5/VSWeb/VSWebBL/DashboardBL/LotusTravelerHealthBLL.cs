using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class LotusTravelerHealthBLL
    {
        private static LotusTravelerHealthBLL _self = new LotusTravelerHealthBLL();

        public static LotusTravelerHealthBLL Ins
        {
            get
            {
                return _self;
            }
        }

        public DataTable SetGrid1()
        {
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGrid1();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		public DataTable SetGrid3()
        {
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGrid3();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable SetGrid1(string servername)
        {
            try
            {
                return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGrid1(servername);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataTable SetGraphForHttpSessions(string paramval, string servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGraphForHttpSessions(paramval, servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable SetGraphForDeviceType(string SortField, string servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGraphForDeviceType(SortField, servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }
        public DataTable SetGraphForDevice_OSType(string SortField,string ServerName,string GraphType)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGraphForDevice_OSType(SortField, ServerName, GraphType);
			}
			catch (Exception ex)
			{
				
				throw ex; 
			}
			
        }
		public DataTable SetGraphForSyncType(string SyncType, string SyncSubType)
		{
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGraphForSyncType(SyncType, SyncSubType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable SetGraphForDeviceCount()
		{
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGraphForDeviceCount();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
        public DataTable SetGrid(int lastmin,int agomin,int moreDevices, int keyUsers)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGrid(lastmin, agomin, moreDevices, keyUsers);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public bool SENDTravelerConsoleCommand(string ServerName, string TellCommand, string user)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SENDTravelerConsoleCommand(ServerName, TellCommand, user);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
         }
        public DataTable SetGraphForMailFileOpens(string servername, string mailservername, string interval)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGraphForMailFileOpens(servername, mailservername, interval);
			}
			catch (Exception ex	)
			{
				
				throw ex;
			}
           
        }

        public DataTable SetGraphForMailFileOpensCumulative(string servername, string mailservername, string interval)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGraphForMailFileOpensCumulative(servername, mailservername, interval);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable SetGridForTravelerInterval(string servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGridForTravelerInterval(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable SelectServerNamesForGrid()
        {
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SelectServerNamesForGrid();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //8/19/2014 NS added for VSPLUS-884
        public DataTable SetGraphForDeviceSyncs(string servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGraphForDeviceSyncs(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        //8/11/2015 NS added for VSPLUS-2029
        public DataTable GetKeyUserDevices(string deviceid)
        {
            return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.GetKeyUserDevices(deviceid);
        }

        //10/8/2015 NS added for VSPLUS-2242
        public DataTable GetMaxLastUpdated()
        {
            return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.GetMaxLastUpdated();
        }

        //10/8/2015 NS added for VSPLUS-2208
        public DataTable SetGraphForJavaMemory(string paramval, string servername)
        {
            return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGraphForJavaMemory(paramval, servername);
        }

        //10/8/2015 NS added for VSPLUS-2208
        public DataTable SetGraphForCMemory(string paramval, string servername)
        {
            return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGraphForCMemory(paramval, servername);
        }

		public DataTable SetGrid()
		{
			try
			{
				return VSWebDAL.DashboardDAL.LotusTravelerHealthDAL.Ins.SetGrid();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
    }
}
