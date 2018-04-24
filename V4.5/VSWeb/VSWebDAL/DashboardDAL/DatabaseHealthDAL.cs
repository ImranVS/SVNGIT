using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
    public class DatabaseHealthDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private AdaptorforDsahBoard Adaptor1 = new AdaptorforDsahBoard();

        private static DatabaseHealthDAL _self = new DatabaseHealthDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static DatabaseHealthDAL Ins
        {
            get { return _self; }
        }
        public DataTable GetData(string SName)
        {
            DataTable dt = new DataTable();
            try
            {
                if (SName != "" && SName != null)
                {
                    string SqlQuery = "Select Status, DominoServerTasks,Name, DominoVersion as Server,Category,TypeandName from Status where Type='Notes Database'and StatusCode in ('Issue','Maintenance','Not Responding','OK')and StatusCode is not null and [Name]='" + SName + "'";
                    dt = objAdaptor.FetchData(SqlQuery);
                }
                else
                {
                    string SqlQuery = "Select Status, DominoServerTasks,Name, DominoVersion as Server,Category,TypeandName from Status where Type='Notes Database'and StatusCode in ('Issue','Maintenance','Not Responding','OK')and StatusCode is not null";
                    dt = objAdaptor.FetchData(SqlQuery);
                }
            }
            catch (Exception)
            {

                throw;
            }



            return dt;
        }
        public DataTable GetData1(string SName)
        {
            DataTable dt = new DataTable();
            try
            {
                if (SName != "" && SName != null)
                {
                    string SqlQuery = "Select Status, DominoServerTasks,Name, DominoVersion as Server,Category,TypeandName,Type,LastUpdate from Status where Type='Notes Database'and StatusCode in ('Issue','Maintenance','Not Responding','OK')and StatusCode is not null and [Name]='" + SName + "'";
                    dt = objAdaptor.FetchData(SqlQuery);
                }
                else
                {
                    string SqlQuery = "Select Status, DominoServerTasks,Name, DominoVersion as Server,Category,TypeandName,Type,LastUpdate from Status where Type='Notes Database'and StatusCode in ('Issue','Maintenance','Not Responding','OK')and StatusCode is not null";
                    dt = objAdaptor.FetchData(SqlQuery);
                }
            }
            catch (Exception)
            {

                throw;
            }



            return dt;
        }

        public DataTable SetGraph(string paramGraph, string DeviceName)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    //12/12/2013 NS modified (column name change)
                    //string strQuerry = "SELECT [DeviceName], [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";
                    string strQuerry = "SELECT [ServerName] DeviceName, [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName]='" + DeviceName + "' order by Date asc";
                    dt = Adaptor1.FetchData(strQuerry);
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
                    //string strQuerry = "SELECT [DeviceName], [DeviceType], [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";
                    string strQuerry = "SELECT [ServerName] DeviceName, [DeviceType], [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName]='" + DeviceName + "' order by Date asc";
                    dt = Adaptor1.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }

        public DataTable GetAllData(string Server)
        {
            DataTable dt = new DataTable();
            try
            {
                if (Server != ""&& Server!=null)
                {
                    //2/5/2014 NS modified to return date part only
                    string SqlQuery = "Select *,DATEADD(dd, 0, DATEDIFF(dd, 0, ScanDate)) ScanDateOnly " + 
                        "from Daily where Server='" + Server + "' and temp=0 order by Title";
                    dt = Adaptor1.FetchData(SqlQuery);
                }
                else
                {
                    string SqlQuery = "Select *,DATEADD(dd, 0, DATEDIFF(dd, 0, ScanDate)) ScanDateOnly "+
                        "From VSS_Statistics.dbo.Daily a inner join Vitalsigns.dbo.Servers b "+
                        "ON a.Server=b.ServerName inner join Vitalsigns.dbo.DominoServers c "+
                        "on c.ServerID = b.ID where Temp=0 and Enabled=1 and Status is not null order by Title ";
                    dt = Adaptor1.FetchData(SqlQuery);
                }
               
            }
            catch (Exception)
            {

                throw;
            }



            return dt;
        }

        public DataTable GetIPfromServers(string Server)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "Select IPAddress from Servers where ServerName='" + Server+"'";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {
                
                throw;
            }
            return dt;
        }
        public DataTable SetGraph(string DeviceName,System.DateTime starttime ,System.DateTime endtime)
        {
            DataTable dt = new DataTable();
            try
            {
                //12/12/2013 NS modified (column name change)
                //string sqlQuery="SELECT [DeviceName], [DeviceType],[Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and "+
                //"[Date] >= '" + starttime + "' and Date < DATEADD (dd , 1 ,'" + endtime + "') and [DeviceName]='" + DeviceName + "' order by Date asc";
                string sqlQuery = "SELECT [ServerName] DeviceName, [DeviceType],[Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and " +
                "[Date] >= '" + starttime + "' and Date < DATEADD (dd , 1 ,'" + endtime + "') and [ServerName]='" + DeviceName + "' order by Date asc";
                //string sqlQuery= "SELECT [DeviceName], [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date >= DATEADD (hh, -1 ,'" + starttime + "' )  and Date < DATEADD (hh , 1 ,'" + endtime + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";
                dt = Adaptor1.FetchData(sqlQuery);
            }
            catch (Exception)
            {
                
                throw;
            }
        //    if (paramGraph == "Today")
        //    {
        //        try
        //        {
        //            string strQuerry = "SELECT [DeviceName], [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime'  and Date = DATEADD (" + paramGraph + " , -1 ,'" + endtime + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";
        //            dt = Adaptor1.FetchData(strQuerry);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    else if(paramGraph=="Two Days")
        //    {
        //        try
        //        {
        //            string strQuerry = "SELECT [DeviceName], [DeviceType], [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,'" + starttime + "' ) and Date > DATEADD (" + paramGraph + " , -1 ,'" + endtime + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";
        //            dt = Adaptor1.FetchData(strQuerry);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    else if (paramGraph == "weekly")
        //    {
        //    }
        //    else
        //    {
        //    }
           return dt;
        }

       

    }
}
