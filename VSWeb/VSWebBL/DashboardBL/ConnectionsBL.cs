using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class ConnectionsBL
    {
        private static ConnectionsBL _self = new ConnectionsBL();

        public static ConnectionsBL Ins
        {
            get
            {
                return _self;
            }
        }

        public DataTable GetConnectionsTests()
        {
			try
			{
				return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetConnectionsTests();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable GetNumberVisitors()
        {
			try
			{
				return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetNumberVisitors();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        public DataTable GetNumberVisits()
        {
			try
			{
				return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetNumberVisits();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetMostActiveApps()
        {
			try
			{
				return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetMostActiveApps();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable GetTop10Content()
        {
			try
			{
				return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetTop10Content();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        //3/11/2016 NS added for VSPLUS-2651
        public DataTable GetConnectionsData()
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetConnectionsData();
        }

        public DataTable GetUsersDailyCount(string statname, string servername, string fromdate = "", string todate = "")
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetUsersDailyCount(statname, servername, fromdate, todate);
        }

        public DataTable GetUsersBlogs(string statname, string servername, string fromdate = "", string todate = "")
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetUserBlogs(statname, servername, fromdate, todate);
        }

        public DataTable GetUserStatsCommon(string statname, string statname2, string servername, string fromdate = "", string todate = "")
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetUserStatsCommon(statname, statname2, servername, fromdate, todate);
        }

        //7/1/2016 NS modified for VSPLUS-3100
        public DataTable GetActivities(string servername, string statname1, string statname2 = "")
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetActivities(servername,statname1,statname2);
        }

        //7/1/2016 NS modified for VSPLUS-3100
        public DataTable GetDailyActivities(string servername)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetDailyActivities(servername);
        }

        public DataTable GetProfileStats(string statname, string statname2, string servername, string stattitle, string stattitle2, bool specific)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetProfileStats(statname, statname2, servername,stattitle,stattitle2, specific);
        }

        //7/1/2016 NS modified for VSPLUS-3100
        public DataTable GetTopTags(string servername,string statname)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetTopTags(servername,statname);
        }

        //5/31/2016 NS modified for VSPLUS-3009
        //7/1/2016 NS modified for VSPLUS-3100
        public DataTable GetStatByName(string servername,string statname, bool isExact=true)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetStatByName(servername,statname,isExact);
        }
        //6/5/2016 Durga Added for VSPLUS-2925
        public DataTable GetUserStatsForActivities(string statname, string statname2, string servername, string fromdate = "", string todate = "")
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetUserStatsForActivities(statname, statname2, servername, fromdate, todate);
        }
        //5/31/2016 NS added for VSPLUS-3011
        //7/1/2016 NS modified for VSPLUS-3100
        public DataTable GetMostActiveCommunity(string servername)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetMostActiveCommunity(servername);
        }
        //6/1/2016 NS added for VSPLUS-3015
        //7/1/2016 NS modified for VSPLUS-3100
        public DataTable GetConnectionsUsers(string servername)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetConnectionsUsers(servername);
        }
        //6/1/2016 NS added for VSPLUS-3015
        public DataTable GetCommunitiesForUsers(string user1, string user2)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetCommunitiesForUsers(user1, user2);
        }
        //6/2/2016 NS added for VSPLUS-3021
        public DataTable GetUserAdoption()
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetUserAdoption();
        }
        //6/2/2016 NS added for VSPLUS-3019
        public DataTable GetUserActivity(string uname)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetUserActivity(uname);
        }
        //6/2/2016 NS added for VSPLUS-3016
        //7/1/2016 NS modified for VSPLUS-3100
        public DataTable GetSourceCommunity(string servername,string objtype)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetSourceCommunity(servername,objtype);
        }

        //6/3/2016 NS added for VSPLUS-3025
        public DataTable GetCommunityActivity(string uname)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetCommunityActivity(uname);
        }
        //6/3/2016 NS added for VSPLUS-3012
        //7/1/2016 NS modified for VSPLUS-3100
        public DataTable GetTop5MostActiveCommunities(string servername)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetTop5MostActiveCommunities(servername);
        }
        //6/6/2016 NS added for VSPLUS-3020
        public DataTable GetCommunitiesMonthly(string servername)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetCommunitiesMonthly(servername);
        }
        //6/6/2016 NS added for VSPLUS-3020
        public DataTable GetCommunityItemsMonthly(string servername)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetCommunityItemsMonthly(servername);
        }
        //6/6/2016 NS added for VSPLUS-3020
        public DataTable GetCommunityItemsMonthlyByType(string servername)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetCommunityItemsMonthlyByType(servername);
        }
        public DataTable GetLibraries(string statname, string ServerName)
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetLibraries(statname, ServerName);
        }
        public DataTable GetUserStatsForLibraries(string statname, string statname2, string servername, string fromdate = "", string todate = "")
        {
            return VSWebDAL.DashboardDAL.ConnectionsDAL.Ins.GetUserStatsForLibraries(statname, statname2, servername, fromdate, todate);
        }
    }
}
