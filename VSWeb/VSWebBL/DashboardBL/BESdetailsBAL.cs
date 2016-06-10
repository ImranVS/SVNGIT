using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
  public  class BESdetailsBAL
    {
      private static BESdetailsBAL _self = new BESdetailsBAL();

      public static BESdetailsBAL Ins
        {
            get
            {
                return _self;
            }
        }
      public DataTable SetGraphforBESmsgsent(string paramGraph, string serverName)
      {
		  try
		  {
			  return VSWebDAL.DashboardDAL.BESDAL.Ins.SetGraphforBESmsgsent(paramGraph, serverName);
		  }
		  catch (Exception ex)
		  {
			  
			  throw ex;
		  }
        
      }
    
      public DataTable SetGraphforBESmsgrecvd(string paramGraph, string serverName)
      {
		  try
		  {
			  return VSWebDAL.DashboardDAL.BESDAL.Ins.SetGraphforBESmsgrecvd(paramGraph, serverName);
		  }
		  catch (Exception ex)
		  {
			  
			  throw ex;
		  }
        
      }
    }
}
