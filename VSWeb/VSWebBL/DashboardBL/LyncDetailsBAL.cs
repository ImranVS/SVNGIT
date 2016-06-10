using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class LyncDetailsBAL
    {

        private static LyncDetailsBAL _self = new LyncDetailsBAL();

        public static LyncDetailsBAL Ins
        {
            get
            {
                return _self;
            }
        }

		public DataTable SetGraphForLyncEnabledUsers(string paramGraph, string serverName, int ServerTypeId)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LyncDetailsDAL.Ins.SetGraphForLyncEnabledUsers(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }
		public DataTable SetGraphforLyncUsersConnected(string paramGraph, string serverName, int ServerTypeId)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LyncDetailsDAL.Ins.SetGraphforLyncUsersConnected(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable SetGraphforLyncVoiceenabled(string paramGraph, string serverName, int ServerTypeId)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LyncDetailsDAL.Ins.SetGraphforLyncVoiceenabled(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable SetGraphforLyncChatLatency(string paramGraph, string serverName, int ServerTypeId)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LyncDetailsDAL.Ins.SetGraphforLyncChatLatency(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable SetGraphforLyncGroupChatLatency(string paramGraph, string serverName, int ServerTypeId)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LyncDetailsDAL.Ins.SetGraphforLyncGroupChatLatency(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //public DataTable SetGraphForchatstatus(string serverName)
        //{
        //    return VSWebDAL.DashboardDAL.LyncDetailsDAL.Ins.SetGraphForchatstatus(serverName);
        //}
        public DataTable SetGraphForchatstatus(string paramGraph, string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LyncDetailsDAL.Ins.SetGraphForchatstatus(paramGraph, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable SetGridForLyncDisk(string ServerName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LyncDetailsDAL.Ins.SetGridForLyncDisk(ServerName);
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
				return VSWebDAL.DashboardDAL.LyncDetailsDAL.Ins.SetGraphforperformance(paramGraph, DeviceName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable ResponseThresholdForLync(string ServerName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LyncDetailsDAL.Ins.ResponseThresholdForLync(ServerName);
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
				return VSWebDAL.DashboardDAL.LyncDetailsDAL.Ins.SetGraphForMemory(paramGraph, serverName, ServerTypeId);
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
				return VSWebDAL.DashboardDAL.LyncDetailsDAL.Ins.SetGraphForCPU(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }


		public DataTable SetGraphForCPULync(string paramGraph, string serverName, int ServerTypeId)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LyncDetailsDAL.Ins.SetGraphForCPULync(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }
		public DataTable SetGraphForMemoryLync(string paramGraph, string serverName, int ServerTypeId)
        {
			try
			{
				return VSWebDAL.DashboardDAL.LyncDetailsDAL.Ins.SetGraphForMemoryLync(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		
        }
    }
}
