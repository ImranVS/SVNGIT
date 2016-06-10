using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class CloudDetailsBL
    {
        private static CloudDetailsBL _self = new CloudDetailsBL();

        public static CloudDetailsBL Ins
        {
            get
            {
                return _self;
            }
        }

		public DataTable SetGraphCloud(string paramGraph, string DeviceName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.CloudDetailsDAL.Ins.SetGraph(paramGraph, DeviceName);
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
				return VSWebDAL.DashboardDAL.CloudDetailsDAL.Ins.ResponseThreshold(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }


        public DataTable GetcloudServerDetails()
        {
            try
            {
                return VSWebDAL.DashboardDAL.CloudDetailsDAL.Ins.GetcloudServerDetails();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetIssuesTasks()
        {
            try
            {
                return VSWebDAL.DashboardDAL.CloudDetailsDAL.Ins.GetIssuesTasks();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //VSPLUS-480;Mukund 07Jul2014
       
        
    }
}
