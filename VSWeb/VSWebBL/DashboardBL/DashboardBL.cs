using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDAL;
using VSWebDO;
using System.Data;

namespace VSWebBL.DashboardBL
{
  public  class DashboardBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static DashboardBL _self = new DashboardBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static DashboardBL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData(string ServerLoc,string viewby )
        {
			try
			{
				return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetAllData(ServerLoc, viewby);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

		public DataTable GetDeviceStatusdiskdetails(string Servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetDeviceStatusdiskdetails(Servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
		public DataTable GetDeviceStatusdiskdetails2(string Servername,string Diskname)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetDeviceStatusdiskdetails2(Servername, Diskname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
	  
        public DataTable GetIndlData()
        {
			try
			{
				 return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetIndlData();
       
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		}
           


      // shilpa

        public DataTable GetStatusbyLocation(string Type)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetStatusbyLocation(Type);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetStatusbyType(string Location)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetStatusbyType(Location);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable GetStatusbyCategory(string Category)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetStatusbyCategory(Category);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        //10/15/2015 NS modified VSPLUS-2247
        public DataTable GetDeviceStatus(string status, string typeloc, string filterbyval, string SessionViewBy, string MapLoc, string orderBy = "Name")
        {
			try
			{
				return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetStatusData(status, typeloc, filterbyval, SessionViewBy, MapLoc, orderBy);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}

			
        }
		public DataTable GetDeviceStatus2()
		{
			try
			{
				return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetStatusData2();
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}
        public DataTable GetStatusChart(string Name, string Type)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetStatusChart(Name, Type);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
     
        public DataTable GetMoniteredServerTasks(string Name)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetMoniteredServerTasks(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public DataTable GetNonMoniteredServerTasks(string Name)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetNonMoniteredServerTasks(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public DataTable GetCombobox()
        {
			try
			{
				return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetCombobox();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        
        }
        public DataTable GetMailStatus(string ServerName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetMailStatus(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //2/25/2016 Durga Modified for VSPLUS-2634
        public List<string> GetProcessStatus()
        {
			try
			{
				return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetProcessStatus();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
          }

      public DataTable GetMyCustomPages(string userID, bool isprivate, bool isadmin)
      {
		  try
		  {
			  return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetMyCustomPages(userID, isprivate, isadmin);
		  }
		  catch (Exception ex)
		  {
			  
			  throw ex;
		  }
        
      }

      public void DeleteMyCustomPage(string ID)
      {
		  try
		  {
			  VSWebDAL.DashboardDAL.DashboardDAL.Ins.DeleteMyCustomPage(ID);
		  }
		  catch (Exception ex)
		  {
			  
			  throw ex;
		  }
          
      }
      public DataTable GetStatusGrid(string Type)
      {
		  try
		  {
			  return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetStatusGrid(Type);
		  }
		  catch (Exception ex)
		  {
			  
			  throw ex;
		  }
          
      }

      public DataTable GetServerType(string notinType)
      {
		  try
		  {
			  return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetServerType(notinType);
		  }
		  catch (Exception ex)
		  {
			  
			  throw ex;
		  }
        
      }

      public DataTable GetSpecificServerType(string inType)
      {
		  try
		  {
			  return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetSpecificServerType(inType);
		  }
		  catch (Exception ex)
		  {
			  
			  throw ex;
		  }
        
      }

      public DataTable GetEXJournalData()
      {
		  try
		  {
			  return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetEXJournalData();
		  }
		  catch (Exception ex)
		  {
			  
			  throw ex;
		  }
       
      }
      public DataTable GetPageSessions()
      {
		  try
		  {
			  return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetPageSessions();
		  }
		  catch (Exception ex)
		  {
			  
			  throw ex;
		  }
         
      }

	  public DataTable GetDeviceStatusHistory(int time)
	  {
		  try
		  {
			  return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetDeviceStatusHistory(time);
		  }
		  catch (Exception ex)
		  {
			  
			  throw ex;
		  }
		 
	  }

      //4/10/2015 NS added
      public DataTable GetStatusCountByType()
      {
          return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetStatusCountByType();
      }
      public DataTable GetStatusCountTotalByType()
      {
          return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetStatusCountTotalByType();
      }
	  //2/9/16 Sowjanya added for VSPLUS-2527
	  public DataTable GetDevicecount(string Name)
	  {
		  try
		  {
			  return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetDevicecount(Name);
		  }
		  catch (Exception ex)
		  {

			  throw ex;
		  }


	  }
      //4/19/16 Sowjanya added for VSPLUS-2863
      public DataTable GetMonitoredURL(string Name)
      {
          try
          {
              return VSWebDAL.DashboardDAL.DashboardDAL.Ins.GetMonitoredURL(Name);
          }
          catch (Exception ex)
          {

              throw ex;
          }


      }

    }


}
