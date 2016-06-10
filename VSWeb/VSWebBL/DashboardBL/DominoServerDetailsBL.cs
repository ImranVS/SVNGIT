using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class DominoServerDetailsBL
    {
        private static DominoServerDetailsBL _self = new DominoServerDetailsBL();

        public static DominoServerDetailsBL Ins
        {
            get
            {
                return _self;
            }
        }

        public DataTable SetGraph(string paramGraph, string DeviceName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.SetGraph(paramGraph, DeviceName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetdataForTraveler(string Name)
        {
            try
            {
                return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.GetdataForTraveler(Name);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable SetGraphforNotes(string paramGraph, string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.SetGraphforNotes(paramGraph, serverName);
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
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.SetGraphForDiskSpace(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

		public DataTable SetGraphForDiskSpace(string serverName,string DiskName,string serverType)
		{
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.SetGraphForDiskSpace(serverName, DiskName, serverType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

        public DataTable SetGraphForCPU(string paramGraph, string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.SetGraphForCPU(paramGraph, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable SetGraphForMemory(string paramGraph, string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.SetGraphForMemory(paramGraph, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable SetGraphForUsers(string paramGraph, string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.SetGraphForUsers(paramGraph, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable SetGraphForMailBox(string str,string ColumnName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.GetGraphForMailBox(str, ColumnName);
			}
			catch (Exception ex)
			{

				throw ex;
			}
            
        }

        public DataTable SetBarGraphForTopMailBox()
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.SetBarGraphForTopMail();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetCheckData()
        {
            try
            {
                return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.CheckedData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SetGraphForTransactionPerMin(string servernamelist)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.SetGraphForTransactionPerMin(servernamelist);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable SetGraphForCPU(string servernamelist)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.SetGraphForCPU(servernamelist);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable SetGraphForMemory(string servernamelist)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.SetGraphForMemory(servernamelist);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable GetAlertHistry(string Sname)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.GetAlertHistry(Sname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable GetOutage(string Sname,string Stype)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.GetOutage(Sname, Stype);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        //VSPLUS-480;Mukund 07Jul2014
        public DataTable getBlackberry(string ServerName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.GetBlackberry(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        //Mukund 16Jul14, VSPLUS-824- Threshold in graph is not updating
        public DataTable ResponseThreshold(string ServerName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.ResponseThreshold(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        //10/6/2015 NS added for VSPLUS-2252
        public DataTable GetSysInfoData(string ServerName, string ServerType)
        {
            return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.GetSysInfoData(ServerName, ServerType);
        }
        //10/9/2015 NS added for VSPLUS-2252
        public DataTable GetServerDetailsData(string ServerName)
        {
            return VSWebDAL.DashboardDAL.DominoServerDetailsDAL.Ins.GetServerDetailsData(ServerName);
        }
    }
}
