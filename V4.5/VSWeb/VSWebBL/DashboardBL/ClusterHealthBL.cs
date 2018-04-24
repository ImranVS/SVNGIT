using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
   public class ClusterHealthBL
    {
       private static ClusterHealthBL _self = new ClusterHealthBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
       public static ClusterHealthBL Ins
        {
            get { return _self; }
        }



       public DataTable GetData()
       {
		   try
		   {
			   return VSWebDAL.DashboardDAL.ClusteHealthDAL.Ins.Getdata();
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
           
       }
       public DataTable Getgraphdata(string name, string from,string to)
       {
		   try
		   {
			   return VSWebDAL.DashboardDAL.ClusteHealthDAL.Ins.GetGraphValues(name, from, to);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
           
       }
    }
}
