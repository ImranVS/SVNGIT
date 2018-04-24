using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebDAL.DashboardDAL
{
    public class SametimeHealthDAL
    {
        private AdaptorforDsahBoard objAdaptor1 = new AdaptorforDsahBoard();
        private Adaptor objAdaptor = new Adaptor();
        private static SametimeHealthDAL _self = new SametimeHealthDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static SametimeHealthDAL Ins
        {
            get { return _self; }
        }
        public DataTable GetDominoSametimeData()
        {
            DataTable dt = new DataTable();
            try
            {
                //1/29/2013 NS modified - the SecondaryRole column shows a value of 'Sametime Server' instead of 'Sametime'
                //string q = "Select * From Status where Type='Domino' and SecondaryRole='Sametime'";
                //10/11/2013 NS modified
                //string q = "Select * From Status where Type='Domino' and SecondaryRole='Sametime Server'";
                string q = "Select * From Status where Type='Domino' and SecondaryRole LIKE '%Sametime%'";
                dt = objAdaptor.FetchData(q);
            }
            catch {
            }
            return dt;
        }

        public DataTable GetSametimeData()
        {
            DataTable dt = new DataTable();
            try
            {
                //10/11/2013 NS modified to include Domino servers with Sametime as secondary role (single grid)
                //string q = "Select * From Status where Type='Sametime Server'";
                //11/20/2013 NS modified the query to exclude disabled servers
                //string q = "SELECT *, CASE WHEN Type='Domino' THEN 'Domino-Sametime' ELSE 'Sametime' END SType " +
                //    "FROM Status WHERE Type='Sametime' OR Type='Domino' and SecondaryRole LIKE '%Sametime%' " + 
                //    "ORDER BY Name";
                string q = "SELECT *, CASE WHEN Type='Domino' THEN 'Domino-Sametime' ELSE 'Sametime' END SType " +
                    " FROM Status WHERE Type='Sametime'" +
                    " AND Status != 'Disabled' " + 
                    " ORDER BY Name";
                dt = objAdaptor.FetchData(q);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetActivechatGraph(string name)
        {
            DataTable dt = new DataTable();
            try
            {
                //1/29/2013 NS modified
                //string query = "Select Date,StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='Users'";
                string query = "Select Date,StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='Users' " +
                    "and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
                    "order by date ";
                dt = objAdaptor1.FetchData(query);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetActiveMeetingGraph(string name)
        {
            DataTable dt = new DataTable();
            try
            {
                //1/29/2013 NS modified
                //string query = "Select Date,StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='Chat Sessions'";
				string query = "Select Date,StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='Chat Sessions' or StatName='n-Way Chat Sessions'" +
                    "and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
                    "order by date ";
                dt = objAdaptor1.FetchData(query);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetActiveNWayGraph(string name)
        {
            DataTable dt = new DataTable();
            try
            {
                //1/29/2013 NS modified
                //string query = "Select Date,StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='n-Way Chat Sessions'";
                string query = "Select Date,StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='n-Way Chat Sessions'" +
                    "and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
                    "order by date ";
                dt = objAdaptor1.FetchData(query);
            }
            catch
            {
            }
            return dt;
        }

        //12/12/2013 NS added
        public DataTable GetResponseTimesGraph(string name)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "Select Date,ROUND(StatValue,0) StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='ResponseTime'" +
                    "and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
                    "order by date ";
                dt = objAdaptor1.FetchData(query);
            }
            catch
            {
            }
            return dt;
        }
		public DataTable GetCountOfAllActiveUsers(string name)
		{
			DataTable dt = new DataTable();
			try
			{
				string query = "Select Date,StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='CountOfAllActiveUsers'" +
					"and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
					"order by date ";
				dt = objAdaptor1.FetchData(query);
			}
			catch
			{
			}
			return dt;
		}
		public DataTable GetNumberofnwaychats(string name)
		{
			DataTable dt = new DataTable();
			try
			{
				string query = "Select Date,StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='Numberofnwaychats'" +
					"and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
					"order by date ";
				dt = objAdaptor1.FetchData(query);
			}
			catch
			{
			}
			return dt;
		}
		public DataTable GetNumberofchatmessages(string name)
		{
			DataTable dt = new DataTable();
			try
			{
				string query = "Select Date,StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='Numberofchatmessages'" +
					"and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
					"order by date ";
				dt = objAdaptor1.FetchData(query);
			}
			catch
			{
			}
			return dt;
		}

		public DataTable GetNumberofopenchatsessions(string name)
		{
			DataTable dt = new DataTable();
			try
			{
				string query = "Select Date,StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='Numberofopenchatsessions'" +
					"and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
					"order by date ";
				dt = objAdaptor1.FetchData(query);
			}
			catch
			{
			}
			return dt;
		}
		public DataTable GetNumberofactivenwaychats1(string name)
		{
			DataTable dt = new DataTable();
			try
			{
				string query = "Select Date,StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='Numberofactivenwaychats'" +
					"and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
					"order by date ";
				dt = objAdaptor1.FetchData(query);
			}
			catch
			{
			}
			return dt;
		}
		public DataTable GetTotalcountofall1x1calls(string name)
		{
			DataTable dt = new DataTable();
			try
			{
				string query = "Select Date,StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='Totalcountofall1x1calls'" +
					"and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
					"order by date ";
				dt = objAdaptor1.FetchData(query);
			}
			catch
			{
			}
			return dt;
		}
		public DataTable GetTotalcountofallcalls(string name)
		{
			DataTable dt = new DataTable();
			try
			{
				string query = "Select Date,StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='Totalcountofallcalls'" +
					"and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
					"order by date ";
				dt = objAdaptor1.FetchData(query);
			}
			catch
			{
			}
			return dt;
		}
		public DataTable GetTotalcountofallmultiusercalls(string name)
		{
			DataTable dt = new DataTable();
			try
			{
				string query = "Select Date,StatValue from SametimeDailyStats where ServerName='" + name + "' and StatName='Totalcountofallmultiusercalls'" +
					"and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
					"order by date ";
				dt = objAdaptor1.FetchData(query);
			}
			catch
			{
			}
			return dt;
		}
		public DataTable SetGraphForChatnWayChatSessionsWebChartControl(string Name)
		{
			DataTable dt = new DataTable();
			try
			{
				//string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Trans.PerMinute' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName] IN (" + servernamelist + ") order by [ServerName] asc";

				//1/7/2013 NS modified the query below
				//string strQuerry = " SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, Max(StatValue) as StatValue," +
				//    "[ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Trans.PerMinute' and Date > DATEADD (hh , -1 ,'2012-11-10 00:00:00.000' )" +
				//    " and [ServerName] IN ( "+servernamelist+") group by [ServerName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) order by [ServerName] asc";
				//1/22/2014 NS added sort for VSPLUS-265
				string strQuerry = "SELECT Date, [StatValue], " +
				"[StatName] FROM [VSS_Statistics].[dbo].[SametimeDailyStats] where [ServerName] ='" + Name + "' and [StatName] IN ('ChatSessions','n-WayChatSessions') " +
				"and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE())" +
				"order by Date";
				dt = objAdaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
		public DataTable GetCountofallcallsandusers(string Name)
		{
			DataTable dt = new DataTable();
			try
			{
				//string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Trans.PerMinute' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName] IN (" + servernamelist + ") order by [ServerName] asc";

				//1/7/2013 NS modified the query below
				//string strQuerry = " SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, Max(StatValue) as StatValue," +
				//    "[ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Trans.PerMinute' and Date > DATEADD (hh , -1 ,'2012-11-10 00:00:00.000' )" +
				//    " and [ServerName] IN ( "+servernamelist+") group by [ServerName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) order by [ServerName] asc";
				//1/22/2014 NS added sort for VSPLUS-265
				string strQuerry = "SELECT Date, [StatValue], " +
				"[StatName] FROM [VSS_Statistics].[dbo].[SametimeDailyStats] where [ServerName] ='" + Name + "' and [StatName] IN ('Countofallcalls','Countofallusers') " +
				"and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE())" +
				"order by Date";
				dt = objAdaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
		public DataTable GetCountofall1x1callsandusers(string Name)
		{
			DataTable dt = new DataTable();
			try
			{
				//string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Trans.PerMinute' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName] IN (" + servernamelist + ") order by [ServerName] asc";

				//1/7/2013 NS modified the query below
				//string strQuerry = " SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, Max(StatValue) as StatValue," +
				//    "[ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Trans.PerMinute' and Date > DATEADD (hh , -1 ,'2012-11-10 00:00:00.000' )" +
				//    " and [ServerName] IN ( "+servernamelist+") group by [ServerName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) order by [ServerName] asc";
				//1/22/2014 NS added sort for VSPLUS-265
				string strQuerry = "SELECT Date, [StatValue], " +
				"[StatName] FROM [VSS_Statistics].[dbo].[SametimeDailyStats] where [ServerName] ='" + Name + "' and [StatName] IN ('Countofall1x1calls','Countofall1x1users') " +
				"and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE())" +
				"order by Date";
				dt = objAdaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
		public DataTable Getcountofallmultiusercallsandusers(string Name)
		{
			DataTable dt = new DataTable();
			try
			{
				//string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Trans.PerMinute' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName] IN (" + servernamelist + ") order by [ServerName] asc";

				//1/7/2013 NS modified the query below
				//string strQuerry = " SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, Max(StatValue) as StatValue," +
				//    "[ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Trans.PerMinute' and Date > DATEADD (hh , -1 ,'2012-11-10 00:00:00.000' )" +
				//    " and [ServerName] IN ( "+servernamelist+") group by [ServerName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) order by [ServerName] asc";
				//1/22/2014 NS added sort for VSPLUS-265
				string strQuerry = "SELECT Date, [StatValue], " +
				"[StatName] FROM [VSS_Statistics].[dbo].[SametimeDailyStats] where [ServerName] ='" + Name + "' and [StatName] IN ('Countofallmultiusercalls','Countofallmultiuserusers') " +
				"and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE())" +
				"order by Date";
				dt = objAdaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
		public DataTable GetNumberofactivemeetingsandusersinsidemeetings(string Name)
		{
			DataTable dt = new DataTable();
			try
			{
				//string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Trans.PerMinute' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName] IN (" + servernamelist + ") order by [ServerName] asc";

				//1/7/2013 NS modified the query below
				//string strQuerry = " SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, Max(StatValue) as StatValue," +
				//    "[ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Trans.PerMinute' and Date > DATEADD (hh , -1 ,'2012-11-10 00:00:00.000' )" +
				//    " and [ServerName] IN ( "+servernamelist+") group by [ServerName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) order by [ServerName] asc";
				//1/22/2014 NS added sort for VSPLUS-265
				string strQuerry = "SELECT Date, [StatValue], " +
				"[StatName] FROM [VSS_Statistics].[dbo].[SametimeDailyStats] where [ServerName] ='" + Name + "' and [StatName] IN ('Numberofactivemeetings','Currentnumberofusersinsidemeetings') " +
				"and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE())" +
				"order by Date";
				dt = objAdaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
        //3/19/2014 NS added
        public DataTable GetSametimeStats(string servername, int month, int year, bool exactmatch)
        {
            DataTable dt = new DataTable();
            string sqlQuery = "";
            string ANDservername = "";
            try
            {
                if (servername == "")
                {
                    ANDservername = "";
                }
                else
                {
                    if (exactmatch)
                    {
                        ANDservername = "  AND ServerName IN(" + servername + ") ";
                    }
                    else
                    {
                        ANDservername = "  AND ServerName LIKE '%" + servername + "%' ";
                    }
                }
                //2/3/2015 NS commented out the query below, changed to the new style query
                /*
                sqlQuery = "SELECT ServerName, ISNULL([n-Way Chat Sessions],0) nWayChat, ISNULL([Chat Sessions],0) ChatSessions, " +
                        "ISNULL([Users],0) Users " +
                        "FROM " +
                        " (SELECT ServerName,StatName,ROUND(SUM(StatValue),0) StatValue FROM dbo.SametimeSummaryStats " +
                        "  WHERE StatName='n-Way Chat Sessions' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(SUM(ISNULL(StatValue,0)),0) StatValue FROM dbo.SametimeSummaryStats " +
                        "  WHERE StatName='Chat Sessions' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(SUM(ISNULL(StatValue,0)),0) StatValue FROM dbo.SametimeSummaryStats " +
                        "  WHERE StatName='Users' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +

                        " ) AS SourceTable " +
                        "PIVOT " +
                        "( " +
                        "SUM(StatValue) " +
                        "FOR StatName IN ([n-Way Chat Sessions],[Chat Sessions],[Users]) " +
                        ") AS PivotTable ";
                 */
                //6/26/2015 NS modified for VSPLUS-1823
                //3/7/2016 NS modified
                sqlQuery = "SELECT ServerName,CASE WHEN StatName='TotalNWChats' OR StatName='TotalnWayChats' THEN 'Total n-Way Chats' WHEN StatName='TotalTwoWayChats' OR StatName='Total2WayChats' THEN 'Total 2-Way Chats' " +
                    "WHEN StatName='MaxConcurrentLogins' OR StatName='PeakLogins' THEN 'Peak Logins' ELSE StatName END StatName,ROUND(SUM(StatValue),0) StatValue " +
                    "FROM dbo.SametimeSummaryStats " +
                    "WHERE MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                    ANDservername +
                    "GROUP BY ServerName,StatName ";
                dt = objAdaptor1.FetchData(sqlQuery);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        //1/22/2015 NS added for VSPLUS-1324
        public DataTable GetSametimeSummaryStats(string fromdate, string todate, string statname)
        {
            DataTable dt = new DataTable();
            string sqlQuery = "";
            try
            {
                //6/26/2015 NS modified for VSPLUS-1823
                //1/12/2016 NS modified for VSPLUS-1823
                switch (statname)
                {
                    case "Total n-Way Chats":
                        statname = "TotalnWayChats";
                        break;
                    case "Total 2-Way Chats":
                        statname = "Total2WayChats";
                        break;
                    case "Peak Logins":
                        statname = "PeakLogins";
                        break;
                }
                sqlQuery = "SELECT ServerName,Date,CASE WHEN StatName='TotalnWayChats' THEN 'Total n-Way Chats' WHEN StatName='Total2WayChats' THEN 'Total 2-Way Chats' " +
                    "WHEN StatName='PeakLogins' THEN 'Peak Logins' ELSE StatName END StatName,ROUND(StatValue,0) StatValue FROM SametimeSummaryStats " +
                    "WHERE Date BETWEEN '" + fromdate + "' AND '" + todate + "' ";
                if (statname != "")
                {
                    sqlQuery += "AND StatName='" + statname + "' ";
                }
                //"WHERE StatName IN ('Users','Chat Sessions','n-Way Chat Sessions','MaxConcurrentLoggedInUsers') " +
                dt = objAdaptor1.FetchData(sqlQuery);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
    }
}
