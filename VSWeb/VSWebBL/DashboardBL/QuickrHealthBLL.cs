using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class QuickrHealthBLL
    {
        private static QuickrHealthBLL _self = new QuickrHealthBLL();

        public static QuickrHealthBLL Ins
        {
            get
            {
                return _self;
            }
        }

        public DataTable SetQuickrServersGrid(string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.QuickrHealthDAL.Ins.SetQuickrServersGrid(serverName);   
			}
			catch (Exception ex)
			{

				throw ex;
			}
          
        }

        public DataTable SetGraphForResponseTime(string paramVal, string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.QuickrHealthDAL.Ins.SetGraphForResponseTime(paramVal, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        public DataTable SetGraphForHttpSessions(string paramValue, string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.QuickrHealthDAL.Ins.SetGraphForHttpSessions(paramValue, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable SetGraphForCPU(string paramValue, string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.QuickrHealthDAL.Ins.SetGraphForCPU(paramValue, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable SetGraphForMemory(string paramValue, string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.QuickrHealthDAL.Ins.SetGraphForMemory(paramValue, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable SetQuickrPlacesGrid(string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.QuickrHealthDAL.Ins.SetQuickrPlacesGrid(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }
        public DataTable getLastScanDate(string Servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.QuickrHealthDAL.Ins.getLastScanDate(Servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable Enabledforscanning(string Servername)

		{
			try
			{
				return VSWebDAL.DashboardDAL.QuickrHealthDAL.Ins.Enabledforscanning(Servername);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

    }
}
