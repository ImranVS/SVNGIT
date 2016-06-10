using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class SametimeHealthBL
    {
        private static SametimeHealthBL _self = new SametimeHealthBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static SametimeHealthBL Ins
        {
            get { return _self; }
        }

        public DataTable GetDominosametimeData()
        {
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetDominoSametimeData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable GetSametimeData()
        {
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetSametimeData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
        }

        public DataTable GetActivechatGraph(string servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetActivechatGraph(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetActiveMeetingGraph(string servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetActiveMeetingGraph(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetActiveNWayGraph(string servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetActiveNWayGraph(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        //12/12/2013 NS added
        public DataTable GetResponseTimesGraph(string servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetResponseTimesGraph(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable GetCountOfAllActiveUsers(string servername)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetCountOfAllActiveUsers(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetNumberofnwaychats(string servername)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetNumberofnwaychats(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetNumberofchatmessages(string servername)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetNumberofchatmessages(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetNumberofopenchatsessions(string servername)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetNumberofopenchatsessions(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetNumberofactivenwaychats1(string servername)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetNumberofactivenwaychats1(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		
		}
		public DataTable GetTotalcountofall1x1calls(string servername)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetTotalcountofall1x1calls(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetTotalcountofallcalls(string servername)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetTotalcountofallcalls(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetTotalcountofallmultiusercalls(string servername)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetTotalcountofallmultiusercalls(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
        //3/19/2014 NS added
        public DataTable GetSametimeStats(string servername, int month, int year, bool exactmatch)
        {
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetSametimeStats(servername, month, year, exactmatch);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        //1/22/2015 NS added for VSPLUS-1324
        public DataTable GetSametimeSummaryStats(string fromdate, string todate, string statname)
        {
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetSametimeSummaryStats(fromdate, todate, statname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            

        }
		public DataTable SetGraphForChatnWayChatSessionsWebChartControl(string servernamelist)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.SetGraphForChatnWayChatSessionsWebChartControl(servernamelist);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetCountofallcallsandusers(string Name)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetCountofallcallsandusers(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetCountofall1x1callsandusers(string Name)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetCountofall1x1callsandusers(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable Getcountofallmultiusercallsandusers(string Name)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.Getcountofallmultiusercallsandusers(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetNumberofactivemeetingsandusersinsidemeetings(string Name)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SametimeHealthDAL.Ins.GetNumberofactivemeetingsandusersinsidemeetings(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		

    }
}
