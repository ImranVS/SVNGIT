using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class SNMPServerDetailsBL
    {
        private static SNMPServerDetailsBL _self = new SNMPServerDetailsBL();

        public static SNMPServerDetailsBL Ins
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
				return VSWebDAL.DashboardDAL.SNMPServerDetailsDAL.Ins.SetGraph(paramGraph, DeviceName);
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
				return VSWebDAL.DashboardDAL.SNMPServerDetailsDAL.Ins.ResponseThreshold(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
       

       

        //VSPLUS-480;Mukund 07Jul2014
       
        
    }
}
