using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
  public class DatabaseHealthBL
  {
      
      /// <summary>
      /// Declarations
      /// </summary>
      private static DatabaseHealthBL _self = new DatabaseHealthBL();

      /// <summary>
      /// Used to call the functions using class name instead of object
      /// </summary>
      public static DatabaseHealthBL Ins
      {
          get { return _self; }
      }

      public DataTable GetData(string Name)
      {
		  try
		  {
			  return VSWebDAL.DashboardDAL.DatabaseHealthDAL.Ins.GetData(Name);       
		  }
		  catch (Exception ex)
		  {
			  
			  throw ex;
		  }
             
      }
      public DataTable GetData1(string Name)
      {
          try
          {
              return VSWebDAL.DashboardDAL.DatabaseHealthDAL.Ins.GetData1(Name);
          }
          catch (Exception ex)
          {

              throw ex;
          }

      }
      public DataTable GetAllData(string Server)
      {
		  try
		  {
			  return VSWebDAL.DashboardDAL.DatabaseHealthDAL.Ins.GetAllData(Server);
		  }
		  catch (Exception ex)
		  {
			  
			  throw ex;
		  }
          
      }
      public DataTable SetGraph( string DeviceName, System.DateTime starttime, System.DateTime endtime)
      {
		  try
		  {
			  return VSWebDAL.DashboardDAL.DatabaseHealthDAL.Ins.SetGraph(DeviceName, starttime, endtime);
		  }
		  catch (Exception ex)
		  {
			  
			  throw ex;
		  }
         
      }
      public DataTable GetIPfromServers(string Server)
      {
		  try
		  {
			  return VSWebDAL.DashboardDAL.DatabaseHealthDAL.Ins.GetIPfromServers(Server);
		  }
		  catch (Exception ex)
		  {
			  
			  throw ex;
		  }
        
      }
     

    }
}
