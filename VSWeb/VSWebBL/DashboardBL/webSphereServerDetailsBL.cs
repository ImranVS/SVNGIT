using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class webSphereServerDetailsBL
	{
        private static webSphereServerDetailsBL _self = new webSphereServerDetailsBL();

        public static webSphereServerDetailsBL Ins
		{
			get
			{
				return _self;
			}
		}

        public DataTable SetGraphheapsize(string paramGraph, string DeviceName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.WebSphereServerDetailsDAL.Ins.SetGraphheapsize(paramGraph, DeviceName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
		}
		//public DataTable SetGraph(string paramGraph, string DeviceName)
		//{
		//    return VSWebDAL.DashboardDAL.SametimeServerDetailsDAL.Ins.SetGraph(paramGraph, DeviceName);
		//}
		public DataTable SetGraphForDiskSpace(string serverName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraphForDiskSpace(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

        public DataTable SetGraphForusedmemory(string paramGraph, string serverName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.WebSphereServerDetailsDAL.Ins.SetGraphForusedmemory(paramGraph, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
		}

        public DataTable SetGraphForuptime(string paramGraph, string serverName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.WebSphereServerDetailsDAL.Ins.SetGraphForuptime(paramGraph, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
		}
        public DataTable SetGraphForProcessCpuUsage(string paramGraph, string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.WebSphereServerDetailsDAL.Ins.SetGraphForProcessCpuUsage(paramGraph, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable SetGraphForActiveCount(string paramGraph, string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.WebSphereServerDetailsDAL.Ins.SetGraphForActiveCount(paramGraph, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable SetGraphForPoolSize(string paramGraph, string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.WebSphereServerDetailsDAL.Ins.SetGraphForPoolSize(paramGraph, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable ResponseThreshold(string ServerName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.WebSphereServerDetailsDAL.Ins.ResponseThreshold(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

		public DataTable SetGraphfreeheapsize(string paramGraph, string DeviceName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.WebSphereServerDetailsDAL.Ins.SetGraphfreeheapsize(paramGraph, DeviceName);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable SetGraphusedheapsize(string paramGraph, string DeviceName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.WebSphereServerDetailsDAL.Ins.SetGraphusedheapsize(paramGraph, DeviceName);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetWebSphereServerDetails(string serverID)
		{
			try
			{
				return VSWebDAL.DashboardDAL.WebSphereServerDetailsDAL.Ins.GetWebSphereServerDetails(serverID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable SetGraphForResponseTime(string serverName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.WebSphereServerDetailsDAL.Ins.SetGraphForResponseTime(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		
      
	}
}
