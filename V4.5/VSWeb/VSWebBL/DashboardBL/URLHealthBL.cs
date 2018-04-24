using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace VSWebBL.DashboardBL
{
   public class URLHealthBL
    {
       private static URLHealthBL _self = new URLHealthBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
       public static URLHealthBL Ins
        {
            get { return _self; }
        }

       public DataTable Getdata()
       {
		   try
		   {
			   return VSWebDAL.DashboardDAL.URLHealthDAL.Ins.GetData();
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }

       public DataTable GetResponseTimeGraphdata(string name)
       {
		   try
		   {
			   return VSWebDAL.DashboardDAL.URLHealthDAL.Ins.GetresponsetimeGraphData(name);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }

       public DataTable GetAvailabilityGraphdata(string name)
       {
		   try
		   {
			   return VSWebDAL.DashboardDAL.URLHealthDAL.Ins.GetAvailabilityGraph(name);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
           
       }

    }
}
