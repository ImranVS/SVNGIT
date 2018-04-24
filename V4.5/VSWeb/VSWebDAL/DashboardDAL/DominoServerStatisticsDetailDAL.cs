using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
    public class DominoServerStatisticsDetailDAL
    {
        private AdaptorforDsahBoard objAdaptorForDashBoard = new AdaptorforDsahBoard();
        private Adaptor objAdaptor = new Adaptor();
        private static DominoServerStatisticsDetailDAL _self = new DominoServerStatisticsDetailDAL();

        public static DominoServerStatisticsDetailDAL Ins
        {
            get
            {
                return _self;
            }            
        }

        public DataTable SetGraph(string paramGraph, string DeviceName)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    //12/12/2013 NS modified (column name change)
                   //string strQuerry = "SELECT [DeviceName], [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,' " + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";
                    string strQuerry = "SELECT [ServerName] DeviceName, [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,' " + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName]='" + DeviceName + "' order by Date asc";

                   // string strQuerry = "SELECT [DeviceName], [DeviceType],Convert(varchar(5),Date,108) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 , Getdate() ) and [DeviceName]='" + DeviceName + "' order by Date asc";
                    dt = objAdaptorForDashBoard.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (paramGraph == "dd")
            {
                try
                {
                    //string strQuerry = "SELECT [DeviceName], [DeviceType], [Date] , [StatValue] FROM [DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,' " + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";
                    //string strQuerry2 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' )  and [DeviceName] = '"+ DeviceName +"' order by Date asc";
                    //string strQuerry2 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date between '2012-06-22 00:00:00.000' and '2012-06-22 23:59:59.000' and [DeviceName] = '" + DeviceName + "' order by Date asc";
                    //string strQuerry = "SELECT [Date], [StatValue] FROM [DeviceDailyStats] where [StatName]='ResponseTime' and [Date] > DATEADD (" + paramGraph + " , -1 ,' " + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";
                    //12/12/2013 NS modified (column name change)
                    //string strQuerry2 = "SELECT ROW_NUMBER() OVER (ORDER BY [Date]) AS LineNumber, [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date between '" + ConfigurationSettings.AppSettings["Current_Date"].ToString().Substring(0, 10) + " 00:00:00.000' and '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' and [DeviceName] = '" + DeviceName + "' order by Date asc";
                    string strQuerry2 = "SELECT ROW_NUMBER() OVER (ORDER BY [Date]) AS LineNumber, [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date between '" + ConfigurationSettings.AppSettings["Current_Date"].ToString().Substring(0, 10) + " 00:00:00.000' and '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' and [ServerName] = '" + DeviceName + "' order by Date asc";

                   // string strQuerry2 = "SELECT ROW_NUMBER() OVER (ORDER BY [Date]) AS LineNumber, [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date between '"+System.DateTime.Now.ToString().Substring(0, 10) + " 00:00:00.000' and '"+System.DateTime.Now +"' and [DeviceName] = '" + DeviceName + "' order by Date asc";

                    //string strQuerry3 = "SELECT [Date], [StatValue] FROM [DeviceDailyStats] where [StatName]='ResponseTime' and [Date] > DATEADD (" + paramGraph + " , -1 ,' " + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";
                    //string strQuerry4 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= dateadd(dd, -1, CONVERT(VARCHAR(10),'2012-06-22 08:18:24.000',111)) and Date < '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' and [DeviceName]='" + DeviceName + "' order by Date asc";
                    dt = objAdaptorForDashBoard.FetchData(strQuerry2);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    //string strQuerry = "SELECT [DeviceName], [DeviceType], [Date] , [StatValue] FROM [DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,' " + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";
                    //string strQuerry2 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' )  and [DeviceName] = '"+ DeviceName +"' order by Date asc";
                    //string strQuerry2 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date between '2012-06-22 00:00:00.000' and '2012-06-22 23:59:59.000' and [DeviceName] = '" + DeviceName + "' order by Date asc";
                    //string strQuerry = "SELECT [Date], [StatValue] FROM [DeviceDailyStats] where [StatName]='ResponseTime' and [Date] > DATEADD (" + paramGraph + " , -1 ,' " + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";
                    //12/12/2013 NS modified (column name change)
                    //string strQuerry2 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date between DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "'), 0) and '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' and [DeviceName] = '" + DeviceName + "' order by Date asc";
                    string strQuerry2 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date between DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "'), 0) and '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' and [ServerName] = '" + DeviceName + "' order by Date asc";

                   // string strQuerry2 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date between DATEADD(MONTH, DATEDIFF(MONTH, 0, Getdate()), 0) and GetDate() and [DeviceName] = '" + DeviceName + "' order by Date asc";

                    //string strQuerry3 = "SELECT [Date], [StatValue] FROM [DeviceDailyStats] where [StatName]='ResponseTime' and [Date] > DATEADD (" + paramGraph + " , -1 ,' " + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";
                    //string strQuerry4 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= dateadd(dd, -1, CONVERT(VARCHAR(10),'2012-06-22 08:18:24.000',111)) and Date < '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' and [DeviceName]='" + DeviceName + "' order by Date asc";
                    dt = objAdaptorForDashBoard.FetchData(strQuerry2);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }

        public DataTable SetGraphForHrCombo(string paramGraph, string serverName, string firstvalue, string secondvalue,string name)
        {
            DataTable dt = new DataTable();
            string strQuery = "";
          try{
              if (firstvalue == "23")
              {
                  //12/12/2013 NS modified (column name change)
                  //strQuery = "select [DeviceName],convert(varchar(10),[Date],111) as Date,convert(varchar(5),Date,108) as Time, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='" + name + "' and Date >= convert(varchar(10), GetDate() ,111) + ' " + firstvalue + ":00' and Date < dateadd(dd,1,convert(varchar(10), GetDate(),111)) + '00:00' and [DeviceName]='" + serverName + "' order by Date asc";
                  strQuery = "select [ServerName] DeviceName,convert(varchar(10),[Date],111) as Date,convert(varchar(5),Date,108) as Time, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='" + name + "' and Date >= convert(varchar(10), GetDate() ,111) + ' " + firstvalue + ":00' and Date < dateadd(dd,1,convert(varchar(10), GetDate(),111)) + '00:00' and [ServerName]='" + serverName + "' order by Date asc";

              }
              else
              {
                  //12/12/2013 NS modified (column name change)
                  //strQuery = "select [DeviceName],convert(varchar(10),[Date],111) as Date,convert(varchar(5),Date,108) as Time, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='" + name + "' and Date between convert(varchar(10), GetDate() ,111) + ' " + firstvalue + ":00' and convert(varchar(10), GetDate(),111) + ' " + secondvalue + ":00' and [DeviceName]='" + serverName + "' order by Date asc";
                  strQuery = "select [ServerName] DeviceName,convert(varchar(10),[Date],111) as Date,convert(varchar(5),Date,108) as Time, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='" + name + "' and Date between convert(varchar(10), GetDate() ,111) + ' " + firstvalue + ":00' and convert(varchar(10), GetDate(),111) + ' " + secondvalue + ":00' and [ServerName]='" + serverName + "' order by Date asc";
              }
                dt = objAdaptorForDashBoard.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetDominoServerThreshold()
        {
            DataTable dt = new DataTable();
            try
            {
				//1/22/2016 Sowjanya, changed the query for VSPLUS-2535
                string sqlquery = "Select DS.ServerID as ServerID , S.ServerName, DS.PendingThreshold, DS.[Scan Interval], DS.OffHoursScanInterval," +
                "DS.Enabled, DS.DeadThreshold, DS.RetryInterval, " +
                "DS.FailureThreshold, DS.DiskSpaceThreshold, DS.ResponseThreshold, " +
                "DS.BES_Server, DS.BES_Threshold, DS.FailureThreshold, DS.SearchString," +
                 "DS.AdvancedMailScan,DS.DeadMailDeleteThreshold,DS.DiskSpaceThreshold," +
                 "DS.HeldThreshold,DS.ScanDBHealth,DS.Memory_Threshold,DS.CPU_Threshold," +
                 "DS.Cluster_Rep_Delays_Threshold,DS.Modified_By,DS.Modified_On,DS.ServerDaysAlert,DS.MonitoredBy " +
				"FROM DominoServers DS INNER JOIN Servers S ON DS.ServerID = S.ID order by S.ServerName";

                    dt = objAdaptor.FetchData(sqlquery);
                    
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable SetGraphForDayCombo(string paramGraph, string serverName, string daysvalue, string firstvalue, string secondvalue,string name)
        {
            DataTable dt = new DataTable();
             string strQuerry2="";
          
                try
                {
                    if (firstvalue == "23")
                    {
                        //12/12/2013 NS modified (column name change)
                        /*
                         strQuerry2 = " select [DeviceName],convert(varchar(10),[Date],111) as Date," +
                                        "convert(datetime,'2012/01/01 '+convert(varchar(5),Date,108)) as Time, " +
                                                 "[StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='" + name + "' and " +
                               "Date >= convert(varchar(10), GetDate() ,111) + ' " + firstvalue + ":00' and date < dateadd(dd,1,convert(varchar(10), GetDate(),111)) + ' 00:00' and [DeviceName]='" + serverName + "' " +

                   "union " +

                   " SELECT  [DeviceName] ,convert(varchar(10),[Date],111) as Date,convert(datetime,'2012/01/01 '+convert(varchar(5),Date,108)) as Time, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] " +
                           "where [StatName]='" + name + "' and Date >= DATEADD(dd,-" + int.Parse(daysvalue) + ",convert(varchar(10), GetDate() ,111)) + ' " + firstvalue + ":00' and " +
                                     " Date < DATEADD(dd,-" + int.Parse(daysvalue) + ",dateadd(dd,1,convert(varchar(10), GetDate() ,111))) + ' 00:00' and [DeviceName]='" + serverName + "' order by Time asc";
                         */

                        strQuerry2 = " select [ServerName] DeviceName,convert(varchar(10),[Date],111) as Date," +
                                        "convert(datetime,'2012/01/01 '+convert(varchar(5),Date,108)) as Time, " +
                                                 "[StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='" + name + "' and " +
                               "Date >= convert(varchar(10), GetDate() ,111) + ' " + firstvalue + ":00' and date < dateadd(dd,1,convert(varchar(10), GetDate(),111)) + ' 00:00' and [ServerName]='" + serverName + "' " +

                   "union " +

                   " SELECT  [ServerName] DeviceName,convert(varchar(10),[Date],111) as Date,convert(datetime,'2012/01/01 '+convert(varchar(5),Date,108)) as Time, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] " +
                           "where [StatName]='" + name + "' and Date >= DATEADD(dd,-" + int.Parse(daysvalue) + ",convert(varchar(10), GetDate() ,111)) + ' " + firstvalue + ":00' and " +
                                     " Date < DATEADD(dd,-" + int.Parse(daysvalue) + ",dateadd(dd,1,convert(varchar(10), GetDate() ,111))) + ' 00:00' and [ServerName]='" + serverName + "' order by Time asc";
                    }
                    else
                    {
                        //12/12/2013 NS modified (column name change)
                        /*
                         strQuerry2 = " select [DeviceName],convert(varchar(10),[Date],111) as Date," +
                                          "convert(datetime,'2012/01/01 '+convert(varchar(5),Date,108)) as Time, " +
                                                   "[StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='" + name + "' and " +
                                 "Date between convert(varchar(10), GetDate() ,111) + ' " + firstvalue + ":00' and convert(varchar(10), GetDate(),111) + ' " + secondvalue + ":00' and [DeviceName]='" + serverName + "' " +

                     "union " +

                     " SELECT  [DeviceName] ,convert(varchar(10),[Date],111) as Date,convert(datetime,'2012/01/01 '+convert(varchar(5),Date,108)) as Time, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] " +
                             "where [StatName]='" + name + "' and Date between DATEADD(dd,-" + int.Parse(daysvalue) + ",convert(varchar(10), GetDate() ,111)) + ' " + firstvalue + ":00' and " +
                                       " DATEADD(dd,-" + int.Parse(daysvalue) + ",convert(varchar(10), GetDate() ,111)) + ' " + secondvalue + ":00' and [DeviceName]='" + serverName + "' order by Time asc";
                         */
                        strQuerry2 = " select [ServerName] DeviceName,convert(varchar(10),[Date],111) as Date," +
                                          "convert(datetime,'2012/01/01 '+convert(varchar(5),Date,108)) as Time, " +
                                                   "[StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='" + name + "' and " +
                                 "Date between convert(varchar(10), GetDate() ,111) + ' " + firstvalue + ":00' and convert(varchar(10), GetDate(),111) + ' " + secondvalue + ":00' and [ServerName]='" + serverName + "' " +

                     "union " +

                     " SELECT  [ServerName] DeviceName,convert(varchar(10),[Date],111) as Date,convert(datetime,'2012/01/01 '+convert(varchar(5),Date,108)) as Time, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] " +
                             "where [StatName]='" + name + "' and Date between DATEADD(dd,-" + int.Parse(daysvalue) + ",convert(varchar(10), GetDate() ,111)) + ' " + firstvalue + ":00' and " +
                                       " DATEADD(dd,-" + int.Parse(daysvalue) + ",convert(varchar(10), GetDate() ,111)) + ' " + secondvalue + ":00' and [ServerName]='" + serverName + "' order by Time asc";
                    }                    
                   
                    dt = objAdaptorForDashBoard.FetchData(strQuerry2);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
         
            return dt;
        }

        public DataTable SetGraphForMonthCombo(string servername, string paramval, string monthsval)
        {
            DataTable dt = new DataTable();
            if (monthsval == "1")
            {
                try
                {
                    //12/12/2013 NS modified (column name change)
                    //string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') - 1, 0) and Date < DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "'), 0) and [DeviceName] = '" + servername + "' order by Date asc";
                    string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') - 1, 0) and Date < DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "'), 0) and [ServerName] = '" + servername + "' order by Date asc";
                    dt = objAdaptorForDashBoard.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (monthsval == "02")
            {
                try
                {
                    //12/12/2013 NS modified (column name change)
                    //string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + int.Parse(monthsval) + ", 0) and Date < DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + (int.Parse(monthsval) - 1) + ", 0) and [DeviceName] = '" + servername + "' order by Date asc";
                    string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + int.Parse(monthsval) + ", 0) and Date < DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + (int.Parse(monthsval) - 1) + ", 0) and [ServerName] = '" + servername + "' order by Date asc";
                    dt = objAdaptorForDashBoard.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (monthsval == "03")
            {
                try
                {
                    //12/12/2013 NS modified (column name change)
                    //string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + int.Parse(monthsval) + ", 0) and Date < DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + (int.Parse(monthsval) - 1) + ", 0) and [DeviceName] = '" + servername + "' order by Date asc";
                    string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + int.Parse(monthsval) + ", 0) and Date < DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + (int.Parse(monthsval) - 1) + ", 0) and [ServerName] = '" + servername + "' order by Date asc";
                    dt = objAdaptorForDashBoard.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (monthsval == "04")
            {
                try
                {
                    //12/12/2013 NS modified (column name change)
                    //string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + int.Parse(monthsval) + ", 0) and Date < DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + (int.Parse(monthsval) - 1) + ", 0) and [DeviceName] = '" + servername + "' order by Date asc";
                    string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + int.Parse(monthsval) + ", 0) and Date < DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + (int.Parse(monthsval) - 1) + ", 0) and [ServerName] = '" + servername + "' order by Date asc";
                    dt = objAdaptorForDashBoard.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    //12/12/2013 NS modified (column name change)
                    //string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + int.Parse(monthsval) + ", 0) and Date < DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + (int.Parse(monthsval) - 1) + ", 0) and [DeviceName] = '" + servername + "' order by Date asc";
                    string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + int.Parse(monthsval) + ", 0) and Date < DATEADD(MONTH, DATEDIFF(MONTH, 0, '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "') -" + (int.Parse(monthsval) - 1) + ", 0) and [ServerName] = '" + servername + "' order by Date asc";
                    dt = objAdaptorForDashBoard.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }

        public DataTable SetGraph2(string today, string reqDate, string serverName)
        {
            DataTable dt = new DataTable();
            try
            {
                //12/12/2013 NS modified (column name change)
                //string strQuerry = "SELECT CONVERT(VARCHAR(10), Date,111) as DatePart, [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and  [DeviceName]='azphxdom1/RPRWyatt' and date between '2012-06-20 00:00:00.000' and '2012-06-22 08:18:24.000' order by Date asc";
                string strQuerry = "SELECT CONVERT(VARCHAR(10), Date,111) as DatePart, [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and  [ServerName]='azphxdom1/RPRWyatt' and date between '2012-06-20 00:00:00.000' and '2012-06-22 08:18:24.000' order by Date asc";
                dt = objAdaptorForDashBoard.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
    }
}
