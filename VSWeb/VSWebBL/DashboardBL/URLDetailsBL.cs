using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class URLDetailsBL
    {
        private static URLDetailsBL _self = new URLDetailsBL();

        public static URLDetailsBL Ins
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
				return VSWebDAL.DashboardDAL.URLDetailsDAL.Ins.SetGraph(paramGraph, DeviceName);
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
				return VSWebDAL.DashboardDAL.URLDetailsDAL.Ins.ResponseThreshold(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
       
        //VSPLUS-480;Mukund 07Jul2014
      
    }
}
