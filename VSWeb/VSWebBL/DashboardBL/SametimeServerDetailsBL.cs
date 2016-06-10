using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class SametimeServerDetailsBL
	{
        private static SametimeServerDetailsBL _self = new SametimeServerDetailsBL();

        public static SametimeServerDetailsBL Ins
		{
			get
			{
				return _self;
			}
		}

		public DataTable SetGraph(string paramGraph, string DeviceName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SametimeServerDetailsDAL.Ins.SetGraph(paramGraph, DeviceName, ServerTypeId);
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

		public DataTable SetGraphForCPU(string paramGraph, string serverName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraphForCPU(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable SetGraphForMemory(string paramGraph, string serverName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraphForMemory(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		
		}

		public DataTable SetGraphForUsers(string paramGraph, string serverName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraphForUsers(paramGraph, serverName, ServerTypeId);
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
				return VSWebDAL.DashboardDAL.SametimeServerDetailsDAL.Ins.ResponseThreshold(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable GetResponseTime()
        {
			try
			{
				return VSWebDAL.DashboardDAL.SametimeServerDetailsDAL.Ins.GetResponseTime();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetactivedirectoryMembers()
        {
			try
			{
				return VSWebDAL.DashboardDAL.SametimeServerDetailsDAL.Ins.GetactivedirectoryMembers();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
	}
}
