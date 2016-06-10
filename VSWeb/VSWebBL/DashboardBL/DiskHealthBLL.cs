using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class DiskHealthBLL
    {
        private static DiskHealthBLL _self = new DiskHealthBLL();

        public static DiskHealthBLL Ins
        {
            get
            {
                return _self;
            }
        }

        public DataTable SetGrid()
        {
			try
			{
				return VSWebDAL.DashboardDAL.DiskHealthDAL.Ins.SetGrid();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		public DataTable SetGrid(string ServerName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.DiskHealthDAL.Ins.SetGrid(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

        public DataTable SetGridforNotes(string Devicename)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DiskHealthDAL.Ins.SetGridforNotes(Devicename);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable SetGridforExchange(string Devicename)
		{
			try
			{
				return VSWebDAL.DashboardDAL.DiskHealthDAL.Ins.SetGridforExchange(Devicename);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable SetGridFromQString(string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DiskHealthDAL.Ins.SetExchangeGridFromQString(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable SetExchangeGridFromQString(string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DiskHealthDAL.Ins.SetExchangeGridFromQString(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable SetGraph(string serverName,string diskName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DiskHealthDAL.Ins.SetGraph(serverName,diskName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable getFixedDskSize()
        {
			try
			{
				return VSWebDAL.DashboardDAL.DiskHealthDAL.Ins.getFixedDskSize();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		public DataTable SetGridForExchange(string serverName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.DiskHealthDAL.Ins.SetGridForExchange(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

        
    }
}
