using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class NetworkDeviceDetailsBL
    {
        private static NetworkDeviceDetailsBL _self = new NetworkDeviceDetailsBL();

        public static NetworkDeviceDetailsBL Ins
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
				return VSWebDAL.DashboardDAL.NetworkDeviceDetailsDAL.Ins.SetGraph(paramGraph, DeviceName);
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
				return VSWebDAL.DashboardDAL.NetworkDeviceDetailsDAL.Ins.ResponseThreshold(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		 public DataTable getStatusAndLastScanDate(string Servername,string type)
		 {
			 try
			 {
				 return VSWebDAL.DashboardDAL.NetworkDeviceDetailsDAL.Ins.getStatusAndLastScanDate(Servername,type);
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
				 return VSWebDAL.DashboardDAL.NetworkDeviceDetailsDAL.Ins.SetGraphForCPU(paramGraph, serverName);
			 }
			 catch (Exception ex)
			 {
				 
				 throw ex;
			 }
			
		 }

		 public DataTable SetGraphForTemperature(string paramGraph, string serverName)
		 {
			 try
			 {
				 return VSWebDAL.DashboardDAL.NetworkDeviceDetailsDAL.Ins.SetGraphForTemperature(paramGraph, serverName);
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
				 return VSWebDAL.DashboardDAL.NetworkDeviceDetailsDAL.Ins.SetGraphForMemory(paramGraph, serverName);
			 }
			 catch (Exception ex)
			 {
				 
				 throw ex; 
			 }
			
		 }

		 public DataTable FillDeviceInfo(string serverName)
		 {
			 try
			 {
				 return VSWebDAL.DashboardDAL.NetworkDeviceDetailsDAL.Ins.FillDeviceInfo(serverName);
			 }
			 catch (Exception ex)
			 {
				 
				 throw ex;
			 }
			
		 }

        //VSPLUS-480;Mukund 07Jul2014
       
        
    }
}
