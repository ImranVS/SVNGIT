using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.ConfiguratorDAL
{
    public class DominoServerDetails_DAL
    {
        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
        private static DominoServerDetails_DAL _self = new DominoServerDetails_DAL();

        public static DominoServerDetails_DAL Ins
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
                        dt = objAdaptor.FetchData(strQuerry);
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
                        //string strQuerry2 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date between '" + ConfigurationSettings.AppSettings["Current_Date"].ToString().Substring(0, 10) + " 00:00:00.000' and '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' and [DeviceName] = '" + DeviceName + "' order by Date asc";
                        //string strQuerry3 = "SELECT [Date], [StatValue] FROM [DeviceDailyStats] where [StatName]='ResponseTime' and [Date] > DATEADD (" + paramGraph + " , -1 ,' " + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";
                        //string strQuerry4 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= dateadd(dd, -1, CONVERT(VARCHAR(10),'2012-06-22 08:18:24.000',111)) and Date < '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' and [DeviceName]='" + DeviceName + "' order by Date asc";
                        //12/12/2013 NS modified (column name change)
                        //string strQuerry2 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date between '" + ConfigurationSettings.AppSettings["Current_Date"].ToString().Substring(0, 10) + " 00:00:00.000' and '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' and [DeviceName] = '" + DeviceName + "' order by Date asc";
                        string strQuerry2 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date between '" + ConfigurationSettings.AppSettings["Current_Date"].ToString().Substring(0, 10) + " 00:00:00.000' and '" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' and [ServerName] = '" + DeviceName + "' order by Date asc";
                        dt = objAdaptor.FetchData(strQuerry2);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return dt;    
        }

        public DataTable SetGraph2(string paramGraph, string DeviceName)
        {
            DataTable dt = new DataTable();
            try
            {
                //12/12/2013 NS modified (column name change)
                //string strQuerry = "SELECT [DeviceName], [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue], 0 as Date1, 0.0 as Statvalue1 FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,' " + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";
                string strQuerry = "SELECT [ServerName] DeviceName, [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue], 0 as Date1, 0.0 as Statvalue1 FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,' " + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName]='" + DeviceName + "' order by Date asc";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable SetGraphForCPU(string paramGraph, string serverName)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Platform.System.PctCombinedCpuUtil' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='"+ serverName +"' order by Date asc";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;

        }

        public DataTable SetGraphForMemory(string paramGraph, string serverName)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mem.PercentUsed' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable SetGraphForUsers(string paramGraph, string serverName)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Users' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='"+ serverName +"' order by Date asc";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;

        }

        public DataTable SetGraphForDiskSpace(string serverName)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT [DiskName], [DiskFree], [DiskSize]-[DiskFree] As DiskUsed FROM [DominoDiskSpace] where [ServerName]='azphxdom2/RPRWyatt' order by [DiskName]";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable SetGraphForHrCombo(string paramGraph, string serverName, string firstvalue, string secondvalue)
        {
            DataTable dt = new DataTable();
            try
            {
                //12/12/2013 NS modified (column name change)
                //string strQuery = "SELECT [DeviceName] , [DeviceType] , CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue] as StatValue FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date between convert(varchar(10), '" + ConfigurationSettings.AppSettings["Current_date"] + "' ,111) + ' " + firstvalue + ":00' and convert(varchar(10), '" + ConfigurationSettings.AppSettings["Current_date"] + "' ,111) + ' " + secondvalue + ":00' and [DeviceName]='" + serverName + "' order by Date asc";
                string strQuery = "SELECT [ServerName] DeviceName, [DeviceType] , CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue] as StatValue FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date between convert(varchar(10), '" + ConfigurationSettings.AppSettings["Current_date"] + "' ,111) + ' " + firstvalue + ":00' and convert(varchar(10), '" + ConfigurationSettings.AppSettings["Current_date"] + "' ,111) + ' " + secondvalue + ":00' and [ServerName]='" + serverName + "' order by Date asc";
                dt = objAdaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable SetGraphForDayCombo(string paramGraph, string serverName, string daysvalue)
        {
            DataTable dt = new DataTable();
            try
            {
                //string strQuerry = "SELECT [DeviceName] , [DeviceType] , [Date] , [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -" + int.Parse(daysvalue) + " ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [DeviceName]='" + serverName + "' order by Date asc";
                //string strQuerry = "SELECT [Date], [StatValue] FROM [DeviceDailyStats] where [StatName]='ResponseTime' and [Date] > DATEADD (" + paramGraph + " , -1 ,' " + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + serverName + "' order by Date asc";
                //string strQuerry = "SELECT [DeviceName], [DeviceType], [Date], [StatValue] FROM [DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + ", -1, ' " + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + serverName + "' order by Date asc";
                //12/12/2013 NS modified (column name change)
                //string strQuerry2 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= dateadd(dd, -1, CONVERT(VARCHAR(10),'2012-06-22 08:18:24.000',111)) and Date < '" + ConfigurationSettings.AppSettings["Current_Date"].ToString().Substring(0, 10) + " 00:00:00.000' and [DeviceName]='" + serverName + "' order by Date asc";
                string strQuerry2 = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= dateadd(dd, -1, CONVERT(VARCHAR(10),'2012-06-22 08:18:24.000',111)) and Date < '" + ConfigurationSettings.AppSettings["Current_Date"].ToString().Substring(0, 10) + " 00:00:00.000' and [ServerName]='" + serverName + "' order by Date asc";
                dt = objAdaptor.FetchData(strQuerry2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetPerHourDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                //12/12/2013 NS modified (column name change)
                //string strQuerry = "SELECT [DeviceName], [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (hh , -1 ,'2012-06-22 08:18:24.000' ) and [DeviceName]='azphxweb1/RPRWyatt' order by Date asc";
                string strQuerry = "SELECT [ServerName] DeviceName, [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (hh , -1 ,'2012-06-22 08:18:24.000' ) and [ServerName]='azphxweb1/RPRWyatt' order by Date asc";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetPerDayDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                //12/12/2013 NS modified (column name change)
                //string strQuerry = "SELECT [DeviceName], [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (dd , -1 ,'2012-06-22 08:18:24.000' ) order by Date asc";
                string strQuerry = "SELECT [ServerName] DeviceName, [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date,[StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (dd , -1 ,'2012-06-22 08:18:24.000' ) order by Date asc";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
    }
}
