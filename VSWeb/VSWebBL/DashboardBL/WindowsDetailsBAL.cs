using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDAL;

namespace VSWebBL.DashboardBL
{
    public class WindowsDetailsBAL
    {

        private static WindowsDetailsBAL _self = new WindowsDetailsBAL();

        public static WindowsDetailsBAL Ins
        {
            get
            {
                return _self;
            }
        }

		public DataTable SetGraphForCPUGeneric(string paramGraph, string serverName, int ServerTypeId)
        {
			try
			{
				return VSWebDAL.DashboardDAL.WindowsDetailsDAL.Ins.SetGraphForCPUGeneric(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }
		public DataTable SetGraphForMemoryGeneric(string paramGraph, string serverName, int ServerTypeId)
        {
			try
			{
				return VSWebDAL.DashboardDAL.WindowsDetailsDAL.Ins.SetGraphForMemoryGeneric(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }

		public DataTable SetGraphForGenericEnabledUsers(string paramGraph, string serverName, int ServerTypeId)
        {
			try
			{
				return VSWebDAL.DashboardDAL.WindowsDetailsDAL.Ins.SetGraphForGenericEnabledUsers(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }
        
        public DataTable SetGraphforperformance(string paramGraph, string DeviceName, int ServerTypeId)
        {
			try
			{
				return VSWebDAL.DashboardDAL.WindowsDetailsDAL.Ins.SetGraphforperformance(paramGraph, DeviceName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }
        public DataTable ResponseThresholdForGeneric(string ServerName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.WindowsDetailsDAL.Ins.ResponseThresholdForGeneric(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable SetGridForGenericDisk(string ServerName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.WindowsDetailsDAL.Ins.SetGridForGenericDisk(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }
    }
}
