using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
	public class ActiveDirectoryServerDetailsBL
	{
        private static ActiveDirectoryServerDetailsBL _self = new ActiveDirectoryServerDetailsBL();

        public static ActiveDirectoryServerDetailsBL Ins
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
				return VSWebDAL.DashboardDAL.ActiveDirectoryServerDetailsDAL.Ins.SetGraph(paramGraph, DeviceName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
	
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
				return VSWebDAL.DashboardDAL.ActiveDirectoryServerDetailsDAL.Ins.ResponseThreshold(ServerName);
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
				return VSWebDAL.DashboardDAL.ActiveDirectoryServerDetailsDAL.Ins.GetResponseTime();
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
				return VSWebDAL.DashboardDAL.ActiveDirectoryServerDetailsDAL.Ins.GetactivedirectoryMembers();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		public DataTable GetReplicationSumamryData(string serverName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ActiveDirectoryServerDetailsDAL.Ins.GetReplicationSumamryData(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
	}
}
