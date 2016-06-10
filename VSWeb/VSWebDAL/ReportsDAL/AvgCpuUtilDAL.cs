using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;


namespace VSWebDAL.ReportsDAL
{
    public class AvgCpuUtilDAL 
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VSS_StatisticsConnectionString"].ToString());

        private AdaptorforDsahBoard objAdaptor1 = new AdaptorforDsahBoard();
        private Adaptor objAdaptor = new Adaptor();
       
        private static AvgCpuUtilDAL _self = new AvgCpuUtilDAL();
      
        public static AvgCpuUtilDAL Ins
        {
            get
            {
                return _self;
            }
        }


       

        public DataTable getdata(string servername,DateTime starttime,DateTime endtime, string threshold, string servertype)
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                //1/18/2013 NS modified
                /*string str = "SELECT ServerName, ROUND(StatValue,2) StatValue, Date, ID " + 
                    "FROM DominoSummaryStats WHERE (StatName = 'Platform.System.PctCombinedCpuUtil') AND " +
                    "(Date >='" + starttime + "') AND (Date <= '" + endtime + "') ";
                 */

                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");

                string start = starttime.ToString(ci);
                string end = endtime.ToString(ci);

                if (servertype == "")
                {
                    str = "SELECT ServerName, ROUND(StatValue,2) StatValue, Date, ID " +
                                "FROM DominoSummaryStats " +
                                "WHERE " +
                                "(StatName = 'Platform.System.PctCombinedCpuUtil') " +
                                "AND " +
                                "(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) " +
                                "AND " +
                                "(convert(datetime,Date,101) <= convert(datetime,'" + end + "',101)) ";
                }
                else
                {
                    str = "SELECT ServerName, ROUND(StatValue,2) StatValue, Date, ID " +
                                "FROM DominoSummaryStats INNER JOIN vitalsigns.dbo.Status ON ServerName=Name " +
                                "WHERE " +
                                "(StatName = 'Platform.System.PctCombinedCpuUtil') " +
                                "AND " +
                                "(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) " +
                                "AND " +
                                "(convert(datetime,Date,101) <= convert(datetime,'" + end + "',101)) " +
                                "AND SecondaryRole='" + servertype + "' ";   
                }
                if (servername != "")
                {
                    //8/5/2013 NS modified
                    str += "AND ServerName IN (" + servername + ") ";
                }
                //7/14/2014 NS added
                if (threshold != "")
                {
                    str += "AND StatValue >= " + threshold;
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //12/17/2013 NS added
        public DataTable getServersForCPUUtil(string ServerType)
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                if (ServerType == "")
                {
                    //6/8/2015 NS modified for VSPLUS-1619
                    str = "SELECT DISTINCT t1.ServerName " +
                        "FROM DominoSummaryStats t1 " +
                        "INNER JOIN vitalsigns.dbo.Servers t2 ON t1.ServerName=t2.ServerName " +
                        "WHERE (StatName = 'Platform.System.PctCombinedCpuUtil') " +
                        "ORDER BY ServerName ";
                }
                else
                {
                    str = "SELECT DISTINCT ServerName " +
                        "FROM DominoSummaryStats INNER JOIN vitalsigns.dbo.Status ON ServerName=Name " +
                        "WHERE (StatName = 'Platform.System.PctCombinedCpuUtil') " +
                        "AND SecondaryRole='" + ServerType + "' " +
                        "ORDER BY ServerName ";
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }


        public DataTable getdataforAvgDailyResponse(string devicename, DateTime starttime, DateTime endtime, string servertype)
        {
            DataTable dt = new DataTable();
            string str = "";
          //  SELECT        Date, StatValue, DeviceName
             //  FROM            DeviceDailyStats
           //  WHERE        (DeviceName = @MyDevice) AND (StatValue <> 0) AND (StatName = 'ResponseTime') AND (Date >= @StartDate) AND (Date <= @EndDate) OR
                      //   (StatValue <> 0) AND (StatName = 'ResponseTime') AND (Date >= @StartDate) AND (Date <= @EndDate) AND (@MyDevice = ' ')
          // ORDER BY Date

            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");

            string start = starttime.ToString(ci);
            string end = endtime.ToString(ci);

            try
            {
                if (servertype == "" || servertype == "''" || servertype == "'All'")
                {
                    if (devicename == "")
                    {
                        //1/15/2013 NS modified
                        //str = "SELECT   Date, StatValue, DeviceName FROM  DeviceDailyStats WHERE " +
                        //    "(StatValue <> 0) AND (StatName = 'ResponseTime') AND (Date >='" + starttime + "') AND " +
                        //    "(Date <= '" + endtime + "') " +
                        //    "ORDER BY Date";
                        //12/12/2013 NS modified (column name change)
                        //str = "SELECT DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue, DeviceName " +
                        //    "FROM  DeviceDailyStats WHERE (StatValue <> 0) AND (StatName = 'ResponseTime') AND " +
                        //    "(Date >='" + starttime + "') AND (Date <= '" + endtime + "') " +
                        //    "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),DeviceName " +
                        //    "ORDER BY DeviceName,DATEADD(dd,0,DATEDIFF(dd,0,Date)) ";
                        //6/10/2014
                        /*str = "SELECT DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue, ServerName DeviceName " +
                            "FROM  DeviceDailyStats WHERE (StatValue <> 0) AND (StatName = 'ResponseTime') AND " +
                            "(Date >='" + starttime + "') AND (Date <= '" + endtime + "') " +
                            "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),ServerName " +
                            "ORDER BY ServerName,DATEADD(dd,0,DATEDIFF(dd,0,Date)) ";
                         */
                        //8/25/2015 NS modified for VSPLUS-1619
                        /*
                        str = "SELECT DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue, ServerName DeviceName " +
                                "FROM  DeviceDailyStats " +
                                "WHERE " +
                                "(StatValue <> 0) AND (StatName = 'ResponseTime') AND " +
                                "(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) AND " +
                                "(convert(datetime,Date,101) < dateadd(day,1,convert(datetime,'" + end + "',101))) " +
                                "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),ServerName ORDER BY ServerName,DATEADD(dd,0,DATEDIFF(dd,0,Date))";
                         */
                        str = "SELECT DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue, ServerName DeviceName " +
                                "FROM  DeviceDailyStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
								"DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
								"AND t2.ID=t3.DeviceTypeID " +
                                "WHERE " +
                                "(StatValue <> 0) AND (StatName = 'ResponseTime') AND " +
                                "(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) AND " +
                                "(convert(datetime,Date,101) < dateadd(day,1,convert(datetime,'" + end + "',101))) " +
                                "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),ServerName ORDER BY ServerName,DATEADD(dd,0,DATEDIFF(dd,0,Date))";
                    }
                    else
                    {
                        //1/15/2013 NS modified
                        //str = "SELECT   Date, StatValue, DeviceName FROM  DeviceDailyStats WHERE (DeviceName = '" + devicename + "') AND " + 
                        //    "(StatValue <> 0) AND (StatName = 'ResponseTime') AND (Date >='" + starttime + "') AND " + 
                        //    "(Date <= '" + endtime + "') " +
                        //    "ORDER BY Date";

                        //8/5/2013 NS modified
                        //str = "SELECT DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue, DeviceName " +
                        //    "FROM  DeviceDailyStats WHERE (DeviceName = '" + devicename + "') AND (StatValue <> 0) AND (StatName = 'ResponseTime') AND " +
                        //    "(Date >='" + starttime + "') AND (Date <= '" + endtime + "') " +
                        //    "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),DeviceName " +
                        //    "ORDER BY DATEADD(dd,0,DATEDIFF(dd,0,Date)) ";
                        //12/12/2013 NS modified (column name change)
                        //str = "SELECT DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue, DeviceName " +
                        //    "FROM  DeviceDailyStats WHERE (DeviceName IN(" + devicename + ")) AND (StatValue <> 0) AND (StatName = 'ResponseTime') AND " +
                        //    "(Date >='" + starttime + "') AND (Date <= '" + endtime + "') " +
                        //    "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),DeviceName " +
                        //    "ORDER BY DeviceName,DATEADD(dd,0,DATEDIFF(dd,0,Date)) ";
                        //1/13/2014 NS modified the query below to refer to the ServerName column instead of DeviceName
                        //6/10/2014
                        /*str = "SELECT DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue, ServerName DeviceName " +
                            "FROM  DeviceDailyStats WHERE (ServerName IN(" + devicename + ")) AND (StatValue <> 0) AND (StatName = 'ResponseTime') AND " +
                            "(Date >='" + starttime + "') AND (Date <= '" + endtime + "') " +
                            "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),ServerName " +
                            "ORDER BY ServerName,DATEADD(dd,0,DATEDIFF(dd,0,Date)) ";
                         */
                        //8/25/2015 NS modified for VSPLUS-1619
                        /*
                        str = "SELECT DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue, ServerName DeviceName " +
                                "FROM  DeviceDailyStats " +
                                "WHERE " +
                                "(ServerName IN(" + devicename + ")) AND (StatValue <> 0) AND (StatName = 'ResponseTime') AND " +
                                "(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) AND " +
                                "(convert(datetime,Date,101) < dateadd(day,1,convert(datetime,'" + end + "',101))) " +
                                "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),ServerName ORDER BY ServerName,DATEADD(dd,0,DATEDIFF(dd,0,Date))";
                         */
                        str = "SELECT DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue, ServerName DeviceName " +
                                "FROM  DeviceDailyStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                                "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                                "AND t2.ID=t3.DeviceTypeID " +
                                "WHERE " +
                                "(ServerName IN(" + devicename + ")) AND (StatValue <> 0) AND (StatName = 'ResponseTime') AND " +
                                "(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) AND " +
                                "(convert(datetime,Date,101) < dateadd(day,1,convert(datetime,'" + end + "',101))) " +
                                "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),ServerName ORDER BY ServerName,DATEADD(dd,0,DATEDIFF(dd,0,Date))";
                    }
                }
                else
                {
                    if (devicename == "")
                    {
                        //8/25/2015 NS modified for VSPLUS-1619
                        /*
                        str = "SELECT DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue, ServerName DeviceName " +
                                "FROM  DeviceDailyStats " +
                                "WHERE " +
                                "(StatValue <> 0) AND (StatName = 'ResponseTime') AND " +
                                "(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) AND " +
                                "(convert(datetime,Date,101) < dateadd(day,1,convert(datetime,'" + end + "',101))) AND " +
                                "DeviceType IN (" + servertype + ") " +
                                "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),ServerName, DeviceType ORDER BY ServerName,DATEADD(dd,0,DATEDIFF(dd,0,Date))";
                         */
                        str = "SELECT DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue, ServerName DeviceName " +
                                "FROM  DeviceDailyStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                                "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                                "AND t2.ID=t3.DeviceTypeID " +
                                "WHERE " +
                                "(StatValue <> 0) AND (StatName = 'ResponseTime') AND " +
                                "(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) AND " +
                                "(convert(datetime,Date,101) < dateadd(day,1,convert(datetime,'" + end + "',101))) AND " +
                                "DeviceType IN (" + servertype + ") " +
                                "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),ServerName, DeviceType ORDER BY ServerName,DATEADD(dd,0,DATEDIFF(dd,0,Date))";
                    }
                    else
                    {
                        //8/25/2015 NS modified for VSPLUS-1619
                        /*
                        str = "SELECT DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue, ServerName DeviceName " +
                                "FROM  DeviceDailyStats " +
                                "WHERE " +
                                "(ServerName IN(" + devicename + ")) AND (StatValue <> 0) AND (StatName = 'ResponseTime') AND " +
                                "(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) AND " +
                                "(convert(datetime,Date,101) < dateadd(day,1,convert(datetime,'" + end + "',101))) AND " +
                                "DeviceType IN (" + servertype + ") " +
                                "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),ServerName, DeviceType ORDER BY ServerName,DATEADD(dd,0,DATEDIFF(dd,0,Date))";
                         */
                        str = "SELECT DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue, ServerName DeviceName " +
                                "FROM  DeviceDailyStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                                "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                                "AND t2.ID=t3.DeviceTypeID " +
                                "WHERE " +
                                "(ServerName IN(" + devicename + ")) AND (StatValue <> 0) AND (StatName = 'ResponseTime') AND " +
                                "(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) AND " +
                                "(convert(datetime,Date,101) < dateadd(day,1,convert(datetime,'" + end + "',101))) AND " +
                                "DeviceType IN (" + servertype + ") " +
                                "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),ServerName, DeviceType ORDER BY ServerName,DATEADD(dd,0,DATEDIFF(dd,0,Date))";
                    }
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }


        public DataTable getdataBlackBerry(string Mydevice,DateTime starttime,DateTime endtime)
        {
            DataTable dt = new DataTable();
            try
            {



                string str = "SELECT  Date, StatValue, Name  FROM BlackBerryProbeStats  WHERE (Name = '"+Mydevice+"') AND (StatValue <> 0) AND (StatName = 'DeliveryTime.Seconds') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') OR"
                             + " (StatValue <> 0) AND (StatName = 'DeliveryTime.Seconds') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') AND ('"+Mydevice+"' = ' ')"
                             + "  ORDER BY Date";
                dt = objAdaptor1.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;
         }

        public DataTable getClusterSeconQ(string ServerName, string StartDate)
        {
            DataTable dt = new DataTable();

            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
            DateTime dtStart = DateTime.Parse(StartDate);

            StartDate = dtStart.ToString(ci);

            try
            {
                //12/11/2012 NS modified
                string str = "";
                if (ServerName == "")
                {
                    //6/11/2014
                   /* str = "SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, Date)) Date, DATEPART(hh,Date) Hour, " + 
                        "ROUND(AVG(StatValue),0) StatValue, ServerName FROM DominoDailyStats " + 
                        "WHERE StatName='Replica.Cluster.SecondsOnQueue' " +
                        "AND DATEADD(dd, 0, DATEDIFF(dd, 0, Date)) = DATEADD(dd, 0, DATEDIFF(dd, 0, '" + StartDate + "')) " +
                        "GROUP BY DATEADD(dd, 0, DATEDIFF(dd, 0, Date)), DATEPART(hh,Date), ServerName " + 
                        "ORDER BY ServerName, Date";
                    */

                    str = "SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, Date)) Date, DATEPART(hh,Date) Hour, ROUND(AVG(StatValue),0) StatValue, ServerName " +
                            "FROM DominoDailyStats " +
                            "WHERE StatName='Replica.Cluster.SecondsOnQueue' AND " +
                            "DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,Date,101))) = DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,'" + StartDate + "',101)))" +
                            "GROUP BY DATEADD(dd, 0, DATEDIFF(dd, 0, Date)), DATEPART(hh,Date), ServerName " +
                            "ORDER BY ServerName, Date";
                }
                else
                {
                    //8/5/2013 NS modified
                    //6/11/2014
                    /*
                    str = "SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, Date)) Date, DATEPART(hh,Date) Hour, " +
                        "ROUND(AVG(StatValue),0) StatValue, ServerName FROM DominoDailyStats " +
                        "WHERE StatName='Replica.Cluster.SecondsOnQueue' " +
                        "AND ServerName IN(" + ServerName + ") " +
                        "AND DATEADD(dd, 0, DATEDIFF(dd, 0, Date)) >= DATEADD(dd, 0, DATEDIFF(dd, 0, '" + StartDate + "')) " +
                        "GROUP BY DATEADD(dd, 0, DATEDIFF(dd, 0, Date)), DATEPART(hh,Date), ServerName " +
                        "ORDER BY ServerName, Date";
                     */

                    str = "SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, Date)) Date, DATEPART(hh,Date) Hour, ROUND(AVG(StatValue),0) StatValue, ServerName " +
                            "FROM DominoDailyStats " +
                            "WHERE StatName='Replica.Cluster.SecondsOnQueue' AND " +
                            "ServerName IN(" + ServerName + ") AND " +
                            "DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,Date,101))) = DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,'" + StartDate + "',101)))" +
                            "GROUP BY DATEADD(dd, 0, DATEDIFF(dd, 0, Date)), DATEPART(hh,Date), ServerName " +
                            "ORDER BY ServerName, Date";
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;

            }
            return dt;
        }
        //26/4/2016 Durga Modified for VSPLUS-2883

        public DataTable DailyMailVolumeRptDS(string ServerName, string ServerType)
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                //8/5/2013 NS modified
                //10/25/2013 NS modified - the table to get the data from should be DominoSummaryStats, not DominoDailyStats
                if (ServerName == "")
                {
                    if(ServerType=="Domino")

                    str = "SELECT ServerName, StatName, StatValue, ID, DATEADD(dd, 0, DATEDIFF(dd, 0, date)) Date " +
                        "FROM DominoSummaryStats WHERE (StatName = 'Mail.Delivered' OR StatName = 'Mail.Transferred' " +
                        "OR StatName = 'Mail.TotalRouted') AND DATEDIFF(dd,0,date) <= DATEDIFF(dd,0,GETDATE()) AND " +
                        "DATEDIFF(dd,0,date) >= DATEADD(dd,-7,GETDATE()) ";
                    else if(ServerType=="Exchange")
                        str = "SELECT ServerName, StatName, StatValue, ID, DATEADD(dd, 0, DATEDIFF(dd, 0, date)) Date " +
                      "FROM MicrosoftSummaryStats WHERE (StatName = 'Mail_SentCount' OR StatName = 'Mail_ReceivedCount' " +
                      "OR StatName = 'Mail_FailCount' OR StatName ='Mail_DeliverCount') AND DATEDIFF(dd,0,date) <= DATEDIFF(dd,0,GETDATE()) AND " +
                      "DATEDIFF(dd,0,date) >= DATEADD(dd,-7,GETDATE()) AND ServerTypeId=5 ";
                    else

                        str = "SELECT ServerName, StatName, StatValue, ID, DATEADD(dd, 0, DATEDIFF(dd, 0, date)) Date " +
                      "FROM MicrosoftSummaryStats WHERE (StatName = 'Mail_SentCount' OR StatName = 'Mail_ReceivedCount' " +
                      ") AND DATEDIFF(dd,0,date) <= DATEDIFF(dd,0,GETDATE()) AND " +
                      "DATEDIFF(dd,0,date) >= DATEADD(dd,-7,GETDATE()) AND  ServerTypeId=21 ";
                }
                else
                {
                    if (ServerType == "Domino")
                    str = "SELECT ServerName, StatName, StatValue, ID, DATEADD(dd, 0, DATEDIFF(dd, 0, date)) Date " +
                        "FROM DominoSummaryStats WHERE (StatName = 'Mail.Delivered' OR StatName = 'Mail.Transferred' " + 
                        "OR StatName = 'Mail.TotalRouted') AND DATEDIFF(dd,0,date) <= DATEDIFF(dd,0,GETDATE()) AND " + 
                        "DATEDIFF(dd,0,date) >= DATEADD(dd,-7,GETDATE()) " +
                        "AND ServerName IN (" + ServerName + ") ";
                    else if (ServerType == "Exchange")
                        str = "SELECT ServerName, StatName, StatValue, ID, DATEADD(dd, 0, DATEDIFF(dd, 0, date)) Date " +
                       "FROM MicrosoftSummaryStats WHERE (StatName = 'Mail_SentCount' OR StatName = 'Mail_ReceivedCount' " +
                      "OR StatName = 'Mail_FailCount' OR StatName ='Mail_DeliverCount') AND DATEDIFF(dd,0,date) <= DATEDIFF(dd,0,GETDATE()) AND " +
                       "DATEDIFF(dd,0,date) >= DATEADD(dd,-7,GETDATE()) " +
                       "AND ServerName IN (" + ServerName + ") AND ServerTypeId=5";
                    else
                        str = "SELECT ServerName, StatName, StatValue, ID, DATEADD(dd, 0, DATEDIFF(dd, 0, date)) Date " +
                    "FROM MicrosoftSummaryStats WHERE (StatName = 'Mail_SentCount' OR StatName = 'Mail_ReceivedCount' " +
                    ") AND DATEDIFF(dd,0,date) <= DATEDIFF(dd,0,GETDATE()) AND " +
                    "DATEDIFF(dd,0,date) >= DATEADD(dd,-7,GETDATE()) AND ServerName IN (" + ServerName + ") AND  ServerTypeId=21 ";
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;


        }

        public DataTable DailyMemoryUsed(int dateY, int month,string ServerName, string ServerType)
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                //2/2/2015 NS modified for VSPLUS-1370
                if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                {
                    if (ServerName == "" || ServerName == "''")
                    {
                        str = "SELECT ServerName, DATEADD(dd,0,DATEDIFF(dd, 0, Date)) AS date, ROUND(StatValue,1) StatValue, t1.ID, t2.ServerType " +
                            "FROM DominoSummaryStats  t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                            "t2.ServerType='Domino' " +
                            "WHERE (StatName = 'Mem.PercentUsed') and MONTH(Date)=" + month.ToString() + " AND " +
                            "YEAR(Date)=" + dateY.ToString() + " " +
                            "UNION " +
                            "SELECT ServerName, DATEADD(dd,0,DATEDIFF(dd, 0, Date)) AS date, ROUND(100-StatValue,1) StatValue, t1.ID, t2.ServerType " + 
                            "FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                            "t2.ID=t1.ServerTypeId " +
                            "WHERE (StatName = 'Mem.PercentAvailable') and MONTH(Date)=" + month.ToString() + " AND " +
                            "YEAR(Date)=" + dateY.ToString() + " " +
                            "ORDER BY ServerName, Date";
                    }
                    else
                    {
                        //8/5/2013 NS modified
                        //str = "SELECT ServerName, DATEADD(dd,0,DATEDIFF(dd, 0, Date)) AS date, ROUND(StatValue,1) StatValue, ID " + 
                        //    "FROM DominoSummaryStats WHERE (StatName = 'Mem.PercentUsed') and MONTH(Date)=" + month.ToString() + " AND " +
                        //    "YEAR(Date)=" + dateY.ToString() + " AND ServerName='" + ServerName + "' " +
                        //    "ORDER BY ServerName, Date";
                        str = "SELECT ServerName, DATEADD(dd,0,DATEDIFF(dd, 0, Date)) AS date, ROUND(StatValue,1) StatValue, ID " +
                            "FROM DominoSummaryStats WHERE (StatName = 'Mem.PercentUsed') and MONTH(Date)=" + month.ToString() + " AND " +
                            "YEAR(Date)=" + dateY.ToString() + " " +
                            "AND ServerName IN (" + ServerName + ") " +
                            "UNION " +
                            "SELECT ServerName, DATEADD(dd,0,DATEDIFF(dd, 0, Date)) AS date, ROUND(100-StatValue,1) StatValue, ID " +
                            "FROM MicrosoftSummaryStats WHERE (StatName = 'Mem.PercentAvailable') and MONTH(Date)=" + month.ToString() + " AND " +
                            "YEAR(Date)=" + dateY.ToString() + "  " +
                            "AND ServerName IN (" + ServerName + ") " +
                            "ORDER BY ServerName, Date";
                    }
                }
                else
                {
                    if (ServerName == "" || ServerName == "''")
                    {
                        str = "SELECT ServerName, DATEADD(dd,0,DATEDIFF(dd, 0, Date)) AS date, ROUND(StatValue,1) StatValue, t1.ID, t2.ServerType AS ServerType " +
                            "FROM DominoSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                            "t2.ServerType='Domino' " +
                            "WHERE (StatName = 'Mem.PercentUsed') and MONTH(Date)=" + month.ToString() + " AND " + 
                            "YEAR(Date)=" + dateY.ToString() + " AND t2.ServerType IN (" + ServerType + ") " +
                            "UNION " +
                            "SELECT ServerName, DATEADD(dd,0,DATEDIFF(dd, 0, Date)) AS date, ROUND(100-StatValue,1) StatValue, t1.ID, t2.ServerType AS ServerType " +
                            "FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                            "t2.ID=t1.ServerTypeId " +
                            "WHERE (StatName = 'Mem.PercentAvailable') and MONTH(Date)=" + month.ToString() + " AND " +
                            "YEAR(Date)=" + dateY.ToString() + " AND t2.ServerType IN (" + ServerType + ") " +
                            "ORDER BY ServerName, Date";
                    }
                    else
                    {
                        str = "SELECT ServerName, DATEADD(dd,0,DATEDIFF(dd, 0, Date)) AS date, ROUND(StatValue,1) StatValue, t1.ID, t2.ServerType AS ServerType " +
                            "FROM DominoSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                            "t2.ServerType='Domino' " +
                            "WHERE (StatName = 'Mem.PercentUsed') and MONTH(Date)=" + month.ToString() + " AND " +
                            "YEAR(Date)=" + dateY.ToString() + " AND t2.ServerType IN (" + ServerType + ") " +
                            "AND ServerName IN(" + ServerName + ") " +
                            "UNION " +
                            "SELECT ServerName, DATEADD(dd,0,DATEDIFF(dd, 0, Date)) AS date, ROUND(100-StatValue,1) StatValue, t1.ID, t2.ServerType AS ServerType " +
                            "FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                            "t2.ID=t1.ServerTypeId " +
                            "WHERE (StatName = 'Mem.PercentAvailable') and MONTH(Date)=" + month.ToString() + " AND " +
                            "YEAR(Date)=" + dateY.ToString() + " AND t2.ServerType IN (" + ServerType + ") " +
                            "AND ServerName IN(" + ServerName + ") " +
                            "ORDER BY ServerName, Date";
                    }
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable DeviceHourlyonTarget(string Servername,string Servertype)
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                if (Servertype == "" || Servertype == "''" || Servertype == "'All'")
                {
                    str = "SELECT DeviceName, ROUND(AVG(StatValue), 1) AS statvalue, DATEADD(dd, 0, DATEDIFF(dd, 0, Date)) AS date " +
                        "FROM DeviceUpTimeStats WHERE StatName = 'HourlyOnTargetPercent' ";
                    if (Servername != "")
                    {
                        //8/2/2013 NS modified
                        //str += " AND DeviceName='" + Servername + "' ";
                        str += " AND DeviceName IN (" + Servername + ") ";
                    }
                    str += " GROUP BY DeviceName, DATEADD(dd, 0, DATEDIFF(dd, 0, Date)) ORDER BY DeviceName, date";
                }
                else
                {
                    str = "SELECT DeviceName, ROUND(AVG(StatValue), 1) AS statvalue, DATEADD(dd, 0, DATEDIFF(dd, 0, Date)) AS date " +
                        "FROM DeviceUpTimeStats WHERE StatName = 'HourlyOnTargetPercent' AND " +
                        "DeviceType IN(" + Servertype + ") ";
                    if (Servername != "")
                    {
                        //8/2/2013 NS modified
                        //str += " AND DeviceName='" + Servername + "' ";
                        str += " AND DeviceName IN (" + Servername + ") ";
                    }
                    str += " GROUP BY DeviceType,DeviceName, DATEADD(dd, 0, DATEDIFF(dd, 0, Date)) ORDER BY DeviceName, date";
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;
        }


        public DataTable DeviceUptime(DateTime dateval,string ServerName, string ServerType)
        {
            //2/4/2015 NS modified for VSPLUS-1370
            DataTable dt = new DataTable();
            string str;

            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");

            string stringVal = dateval.ToString(ci);

            try
            {
                //8/5/2013 NS modified
                //string str = "SELECT  ID, DeviceType, DeviceName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber, ROUND(StatValue, 1) AS UpPercent, DATEPART(hour, Date) AS Hour, DeviceName + ' - ' + DeviceType AS NameANDType "
                //           + " FROM  dbo.DeviceUpTimeStats WHERE (DATEDIFF(dd, 0, Date) >= DATEDIFF(dd, 0,'"+dateval+"')) AND (StatName = 'HourlyUpPercent') AND (DeviceName + ' - ' + DeviceType = '"+ServerName+"') OR "
                //
                //        + " (DATEDIFF(dd, 0, Date) >= DATEDIFF(dd, 0, '"+dateval+"' )) AND (StatName = 'HourlyUpPercent') AND ('"+ServerName+"' = '')"
                //        + " ORDER BY DeviceName + '-' + DeviceType";
                if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                {
                    if (ServerName == "")
                    {
                        //6/11/2014 Wesley- changed for international and unsure why was >=, so changed to = the same day
                        /*str = "SELECT  ID, DeviceType, DeviceName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, " +
                        "DayNumber, ROUND(StatValue, 1) AS UpPercent, DATEPART(hour, Date) AS Hour, " +
                        "DeviceName + ' - ' + DeviceType AS NameANDType " +
                        "FROM  dbo.DeviceUpTimeStats " +
                        "WHERE (DATEDIFF(dd, 0, Date) >= DATEDIFF(dd, 0,'" + dateval + "')) AND (StatName = 'HourlyUpPercent') " +
                        "ORDER BY DeviceName + '-' + DeviceType";
                         */
                        str = "SELECT  ID, DeviceType, DeviceName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber, " +
                                "ROUND(StatValue, 1) AS UpPercent, DATEPART(hour, Date) AS Hour " +
                                "FROM  dbo.DeviceUpTimeStats " +
                                "WHERE (StatName = 'HourlyUpPercent') AND " +
                                "DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,Date,101))) = DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + stringVal + "',101))) " +
                                "ORDER BY DeviceName, DeviceType, Hour";
                    }
                    else
                    {
                        //6/11/2014 Wesley
                        /*str = "SELECT  ID, DeviceType, DeviceName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, " +
                        "DayNumber, ROUND(StatValue, 1) AS UpPercent, DATEPART(hour, Date) AS Hour, " +
                        "DeviceName + ' - ' + DeviceType AS NameANDType " +
                        "FROM  dbo.DeviceUpTimeStats " +
                        "WHERE (DATEDIFF(dd, 0, Date) >= DATEDIFF(dd, 0,'" + dateval + "')) AND (StatName = 'HourlyUpPercent') " +
                        "AND (DeviceName + ' - ' + DeviceType IN (" + ServerName + ")) ORDER BY DeviceName + '-' + DeviceType";
                         */
                        str = "SELECT  ID, DeviceType, DeviceName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber, " +
                                "ROUND(StatValue, 1) AS UpPercent, DATEPART(hour, Date) AS Hour " +
                                "FROM  dbo.DeviceUpTimeStats " +
                                "WHERE (StatName = 'HourlyUpPercent') AND DeviceName IN (" + ServerName + ") AND " +
                                "DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,Date,101))) = DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + stringVal + "',101))) " +
                                "ORDER BY DeviceName,DeviceType,Hour";
                    }
                }
                else
                {
                    if (ServerName == "")
                    {
                        str = "SELECT  ID, DeviceType, DeviceName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber, " +
                                "ROUND(StatValue, 1) AS UpPercent, DATEPART(hour, Date) AS Hour " +
                                "FROM  dbo.DeviceUpTimeStats " +
                                "WHERE (StatName = 'HourlyUpPercent') AND DeviceType IN (" + ServerType + ") AND " +
                                "DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,Date,101))) = DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + stringVal + "',101))) " +
                                "ORDER BY DeviceName,DeviceType,Hour";
                    }
                    else
                    {
                        str = "SELECT  ID, DeviceType, DeviceName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber, " +
                                "ROUND(StatValue, 1) AS UpPercent, DATEPART(hour, Date) AS Hour " +
                                "FROM  dbo.DeviceUpTimeStats " +
                                "WHERE (StatName = 'HourlyUpPercent') AND DeviceName IN (" + ServerName + ") AND " +
                                "DeviceType IN (" + ServerType + ") AND " +
                                "DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,Date,101))) = DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + stringVal + "',101))) " +
                                "ORDER BY DeviceName,DeviceType,Hour";
                    }
                }
                dt = objAdaptor1.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;
        }

        public DataTable DominoDBDaily()
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT  ID, Title, ScanDate, CASE WHEN Folder = '' THEN '...' ELSE Folder END AS Folder, Server, DesignTemplateName, Status, DocumentCount, PercentUsed, IsMailFile, FileNamePath FROM Daily"
                             +" WHERE (DATEADD(dd, 0, DATEDIFF(dd, 0, ScanDate)) IN"
                            +" (SELECT  MAX(DATEADD(dd, 0, DATEDIFF(dd, 0, ScanDate))) AS Expr1"
                             + " FROM  Daily AS Daily_1))";
                dt = objAdaptor1.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;
        }


        public DataTable DominoDiskSpace(string ServerName)
        {
            DataTable dt = new DataTable();
            try
            {
                //1/16/2013 NS modified
                //string str = "SELECT dds.ServerName, dds.DiskName, ROUND(dds.DiskFree,2) DiskFree, ROUND(dds.DiskSize,2) DiskSize, " +
                //    "ROUND((ISNULL(dds.PercentFree,0) * 100),2) PercentFree, (ISNULL(dds.PercentUtilization,0) * 100) PercentUtilization, " +
                //    "dds.AverageQueueLength, dds.Updated, dds.ID, dds.Threshold, l.Location FROM DominoDiskSpace dds "+
                //    "INNER JOIN Servers s ON dds.ServerName=s.ServerName INNER JOIN Locations l ON s.LocationID=l.ID ";

                //05/02/2014 MD modified, VSPLUS-444
                //6/17/2014 NS modified for VSPLUS-728
                /*
                string str =
                   " SELECT dds.ServerName, dds.DiskName, ROUND(dds.DiskFree,2) DiskFree, ROUND(dds.DiskSize,2) DiskSize,  " +
                    "ROUND((ISNULL(dds.PercentFree,0) * 100),2) PercentFree, (ISNULL(dds.PercentUtilization,0) * 100) PercentUtilization,  " +
                    "dds.AverageQueueLength, dds.Updated, dds.ID, dds.Threshold, l.Location FROM DominoDiskSpace dds  " +
                    "INNER JOIN Servers s ON dds.ServerName=s.ServerName INNER JOIN Locations l ON s.LocationID=l.ID  " +
                    "inner join DominoDiskSettings st on dds.servername=st.servername and st.DiskName<>'NoAlerts' and st.DiskName<>'0' and  " +
                    "dds.DiskName=st.DiskName  and  diskfree is not null and disksize is not null " +
                    "union " +
                    "SELECT dds.ServerName, dds.DiskName, ROUND(dds.DiskFree,2) DiskFree, ROUND(dds.DiskSize,2) DiskSize,  " +
                    "ROUND((ISNULL(dds.PercentFree,0) * 100),2) PercentFree, (ISNULL(dds.PercentUtilization,0) * 100) PercentUtilization,  " +
                    "dds.AverageQueueLength, dds.Updated, dds.ID, dds.Threshold, l.Location FROM DominoDiskSpace dds  " +
                    "INNER JOIN Servers s ON dds.ServerName=s.ServerName INNER JOIN Locations l ON s.LocationID=l.ID  " +
                    "inner join DominoDiskSettings st on dds.servername=st.servername  and st.DiskName='0' and dds.DiskName<>st.DiskName  and  diskfree is not null and disksize is not null ";
                */
                //8/5/2014 NS modified - added Exchange servers to the selection
                //1/27/2016 NS modified for VSPLUS-2555
                string str =
                   "SELECT dds.ServerName, dds.DiskName, ROUND(dds.DiskFree,2) DiskFree, ROUND(dds.DiskSize,2) DiskSize,  " +
                    "ROUND((ISNULL(dds.PercentFree,0) * 100),2) PercentFree, (ISNULL(dds.PercentUtilization,0) * 100) PercentUtilization,  " +
                    "dds.AverageQueueLength, dds.Updated, dds.ID, l.Location, " +
                    "CASE WHEN st.Threshold IS NULL THEN '' ELSE " +
                    "CAST(st.Threshold as varchar(10)) + ' ' + st.ThresholdType END Threshold " +
                    "FROM DominoDiskSpace dds  " +
                    "INNER JOIN Servers s ON dds.ServerName=s.ServerName INNER JOIN Locations l ON s.LocationID=l.ID  " +
                    "left join DominoDiskSettings st on dds.servername=st.servername and  " +
                    "(dds.DiskName=st.DiskName OR st.DiskName='AllDisks') ";
                if (ServerName != "")
                {
                    str += " WHERE (dds.ServerName ='" + ServerName + "') ";
                }
                //1/27/2016 NS modified for VSPLUS-2555
                str += "UNION " +
                    "SELECT dds.ServerName, dds.DiskName, ROUND(dds.DiskFree,2) DiskFree, ROUND(dds.DiskSize,2) DiskSize,  " +
                    "ROUND((ISNULL(dds.PercentFree,0) * 100),2) PercentFree, (ISNULL(dds.PercentUtilization,0) * 100) PercentUtilization,  " +
                    "dds.AverageQueueLength, dds.Updated, dds.ID, l.Location, CASE WHEN st.Threshold IS NULL THEN '' ELSE " +
                    "CAST(st.Threshold as varchar(10)) + ' ' + st.ThresholdType END Threshold FROM DiskSpace dds  " +
                    "INNER JOIN Servers s ON dds.ServerName=s.ServerName INNER JOIN Locations l ON s.LocationID=l.ID  " +
                    "left join DiskSettings st on s.ID=st.ServerID and  " +
                    "(dds.DiskName=st.DiskName OR st.DiskName='AllDisks') ";
                if (ServerName != "")
                {
                    str += " WHERE (dds.ServerName ='" + ServerName + "') ";
                }
                str += "ORDER BY DiskName ";
                dt = objAdaptor.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;
        }

        public DataTable DominoDiskSpaceLoc(string LocationName)
        {
            DataTable dt = new DataTable();
            try
            {
                //1/16/2013 NS modified
                //string str = "SELECT dds.ServerName, dds.DiskName, ROUND(dds.DiskFree,2) DiskFree, ROUND(dds.DiskSize,2) DiskSize, " +
                //    "ROUND((ISNULL(dds.PercentFree,0) * 100),2) PercentFree, (ISNULL(dds.PercentUtilization,0) * 100) PercentUtilization, " +
                //    "dds.AverageQueueLength, dds.Updated, dds.ID, dds.Threshold, l.Location FROM DominoDiskSpace dds "+
                //    "INNER JOIN Servers s ON dds.ServerName=s.ServerName INNER JOIN Locations l ON s.LocationID=l.ID ";

                //05/02/2014 MD modified, VSPLUS-444
                //6/17/2014 NS modified for VSPLUS-728
                /*
                string str =
                   " SELECT dds.ServerName, dds.DiskName, ROUND(dds.DiskFree,2) DiskFree, ROUND(dds.DiskSize,2) DiskSize,  " +
                    "ROUND((ISNULL(dds.PercentFree,0) * 100),2) PercentFree, (ISNULL(dds.PercentUtilization,0) * 100) PercentUtilization,  " +
                    "dds.AverageQueueLength, dds.Updated, dds.ID, dds.Threshold, l.Location FROM DominoDiskSpace dds  " +
                    "INNER JOIN Servers s ON dds.ServerName=s.ServerName INNER JOIN Locations l ON s.LocationID=l.ID  " +
                    "inner join DominoDiskSettings st on dds.servername=st.servername and st.DiskName<>'NoAlerts' and st.DiskName<>'0' and  " +
                    "dds.DiskName=st.DiskName  and  diskfree is not null and disksize is not null " +
                    "union " +
                    "SELECT dds.ServerName, dds.DiskName, ROUND(dds.DiskFree,2) DiskFree, ROUND(dds.DiskSize,2) DiskSize,  " +
                    "ROUND((ISNULL(dds.PercentFree,0) * 100),2) PercentFree, (ISNULL(dds.PercentUtilization,0) * 100) PercentUtilization,  " +
                    "dds.AverageQueueLength, dds.Updated, dds.ID, dds.Threshold, l.Location FROM DominoDiskSpace dds  " +
                    "INNER JOIN Servers s ON dds.ServerName=s.ServerName INNER JOIN Locations l ON s.LocationID=l.ID  " +
                    "inner join DominoDiskSettings st on dds.servername=st.servername  and st.DiskName='0' and dds.DiskName<>st.DiskName  and  diskfree is not null and disksize is not null ";
                */
                //8/5/2014 NS modified - added Exchange servers to the selection
                //1/27/2016 NS modified for VSPLUS-2555
                string str =
                   " SELECT dds.ServerName, dds.DiskName, ROUND(dds.DiskFree,2) DiskFree, ROUND(dds.DiskSize - dds.DiskFree,2) DiskUsed, ROUND(dds.DiskSize,2) DiskSize,  " +
                    "ROUND((ISNULL(dds.PercentFree,0) * 100),2) PercentFree, (ISNULL(dds.PercentUtilization,0) * 100) PercentUtilization,  " +
                    "dds.AverageQueueLength, dds.Updated, dds.ID, CASE WHEN st.Threshold IS NULL THEN '' ELSE " +
                    "CAST(st.Threshold as varchar(10)) + ' ' + st.ThresholdType END Threshold, l.Location FROM DominoDiskSpace dds  " +
                    "INNER JOIN Servers s ON dds.ServerName=s.ServerName INNER JOIN Locations l ON s.LocationID=l.ID  " +
                    "left join DominoDiskSettings st on dds.servername=st.servername and  " +
                    "(dds.DiskName=st.DiskName OR st.DiskName='AllDisks') ";
                if (LocationName != "")
                {
                    str += " WHERE (l.Location ='" + LocationName + "') ";
                }
                //1/27/2016 NS modified for VSPLUS-2555
                str += "UNION " +
                    "SELECT dds.ServerName, dds.DiskName, ROUND(dds.DiskFree,2) DiskFree, ROUND(dds.DiskSize - dds.DiskFree,2) DiskUsed, ROUND(dds.DiskSize,2) DiskSize,  " +
                    "ROUND((ISNULL(dds.PercentFree,0) * 100),2) PercentFree, (ISNULL(dds.PercentUtilization,0) * 100) PercentUtilization,  " +
                    "dds.AverageQueueLength, dds.Updated, dds.ID, CASE WHEN st.Threshold IS NULL THEN '' ELSE " +
                    "CAST(st.Threshold as varchar(10)) + ' ' + st.ThresholdType END Threshold, l.Location FROM DiskSpace dds  " +
                    "INNER JOIN Servers s ON dds.ServerName=s.ServerName INNER JOIN Locations l ON s.LocationID=l.ID  " +
                    "left join DiskSettings st on s.ID=st.ServerID and  " +
                    "(dds.DiskName=st.DiskName OR st.DiskName='AllDisks') ";
                if (LocationName != "")
                {
                    str += " WHERE (l.Location ='" + LocationName + "') ";
                }
                str += "ORDER BY DiskName";
                dt = objAdaptor.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;
        }

        public DataTable DominoDiskSpaceSrc(string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                //8/5/2014 NS modified - added Exchange servers to the list
                string str = "SELECT DiskName, DiskFree * 100 AS DiskFree, (DiskSize - DiskFree) * 100 AS DiskUsed " +
                    "FROM DominoDiskSpace WHERE  (ServerName = '"+servername+"') " +
                    "UNION " +
                    "SELECT DiskName, DiskFree * 100 AS DiskFree, (DiskSize - DiskFree) * 100 AS DiskUsed " +
                    "FROM DiskSpace WHERE  (ServerName = '" + servername + "') " +
                    "ORDER BY DiskName";
                dt = objAdaptor.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;

        }

        public DataTable DominoResponseTimesMonthly(int month,int year,string serverName)
        {
            DataTable dt = new DataTable();
            try
            {
                //1/16/2013 NS modified
                //string str = "SELECT ID, DeviceType, DeviceName, Date, StatName, StatValue, WeekNumber, MonthNumber, YearNumber, DayNumber FROM dbo.DeviceDailyStats"
                //               + " WHERE (DeviceType = 'Domino') AND (MonthNumber = "+month+" OR MonthNumber = MONTH(GETDATE())) AND (YearNumber = "+year+" OR"
                //        + " YearNumber = YEAR(GETDATE())) AND (DeviceName = '"+serverName+"') OR (DeviceType = 'Domino') AND (MonthNumber = "+month+" OR"
                //         + " MonthNumber = MONTH(GETDATE())) AND (YearNumber = "+year+" OR  YearNumber = YEAR(GETDATE())) AND ('"+serverName+"' = '')";
                //10/23/2013 NS modified
                //string str = "SELECT DeviceName, DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue " +
                //    "FROM dbo.DeviceDailyStats WHERE (DeviceType = 'Domino') AND StatName='ResponseTime' AND " +
                //    "(MonthNumber = " + month.ToString() + " OR MonthNumber = MONTH(GETDATE())) AND (YearNumber = " + year.ToString() +
                //    " OR YearNumber = YEAR(GETDATE())) ";
                //12/12/2013 NS modified (column name change)
                //   string str = "SELECT DeviceName, DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue " +
                //        "FROM dbo.DeviceDailyStats WHERE (DeviceType = 'Domino') AND StatName='ResponseTime' AND " +
                //        "(MonthNumber = " + month.ToString() + " AND YearNumber = " + year.ToString() + ") ";
                //8/25/2015 NS modified for VSPLUS-1619
                //string str = "SELECT ServerName DeviceName, DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue " +
                //        "FROM dbo.DeviceDailyStats WHERE (DeviceType = 'Domino') AND StatName='ResponseTime' AND " +
                //        "(MonthNumber = " + month.ToString() + " AND YearNumber = " + year.ToString() + ") ";
                string str = "SELECT ServerName DeviceName, DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date, ROUND(AVG(StatValue),2) StatValue " +
                        "FROM dbo.DeviceDailyStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                        "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                        "AND t2.ID=t3.DeviceTypeID WHERE (DeviceType = 'Domino') AND StatName='ResponseTime' AND " +
                        "(MonthNumber = " + month.ToString() + " AND YearNumber = " + year.ToString() + ") ";
                if (serverName != "")
                {
                    //8/5/2013 NS modified
                    //str += " AND DeviceName='" + serverName + "' ";
                    //12/12/2013 NS modified (column name change)
                    //str += " AND DeviceName IN (" + serverName + ") ";
                    str += " AND ServerName IN (" + serverName + ") ";
                }
                //12/12/2013 NS modified (column name change)
                //str += "GROUP BY DeviceName, DATEADD(dd,0,DATEDIFF(dd,0,Date)) ORDER BY DeviceName, Date";
                str += "GROUP BY ServerName, DATEADD(dd,0,DATEDIFF(dd,0,Date)) ORDER BY ServerName, Date";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;


        }

        public DataTable DominoServerHealth()
        {

            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT  Type, Location, Category, Name, Status, Details, LastUpdate, Description, PendingMail, DeadMail, MailDetails, Upcount, DownCount, UpPercent, ResponseTime,"
                           + " ResponseThreshold, PendingThreshold, DeadThreshold, UserCount, MyPercent, NextScan, DominoServerTasks, TypeANDName, Icon, OperatingSystem,"
                        + " DominoVersion, UpMinutes, DownMinutes, UpPercentMinutes, PercentageChange, CPU, HeldMail, HeldMailThreshold, Severity, Memory, StatusCode,"
                        + " SecondaryRole, CPUThreshold, CASE WHEN StatusCode = 'Not Responding' THEN 0 WHEN StatusCode = 'OK' THEN 2 ELSE 1 END AS SortCol FROM Status WHERE (Type = 'Domino')";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;


        }


        public DataTable DominoSrvCpuutil(string ServerName)
        {

            DataTable dt = new DataTable();
            try
            {
                //8/25/2015 NS modified for VSPLUS-1619
                //string str = "SELECT ServerName, DATEPART(hour, Date) AS Hour, ROUND(AVG(StatValue),2) StatValue " +
                //    "FROM  VSS_Statistics.dbo.DominoDailyStats " +
                //   "WHERE (StatName = 'Platform.System.PctCombinedCpuUtil') AND (DATEADD(dd, 0, DATEDIFF(dd, 0, Date)) IN " +
                //    "(SELECT MAX(DATEADD(dd, 0, DATEDIFF(dd, 0, Date))) AS Expr1 FROM VSS_Statistics.dbo.DominoDailyStats AS DominoDailyStats_1)) ";
                string str = "SELECT ServerName, DATEPART(hour, Date) AS Hour, ROUND(AVG(StatValue),2) StatValue " +
                    "FROM  VSS_Statistics.dbo.DominoDailyStats t1 INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON " +
                    "ServerName=Name AND t3.DeviceTypeID=1 " +
                    "WHERE (StatName = 'Platform.System.PctCombinedCpuUtil') AND (DATEADD(dd, 0, DATEDIFF(dd, 0, Date)) IN " +
                    "(SELECT MAX(DATEADD(dd, 0, DATEDIFF(dd, 0, Date))) AS Expr1 FROM VSS_Statistics.dbo.DominoDailyStats AS DominoDailyStats_1)) ";
                if (ServerName != "")
                {
                    //8/5/2013 NS modified
                    //str += " AND ServerName='" + ServerName + "' ";
                    str += " AND ServerName IN (" + ServerName + ") ";
                }
                str += "GROUP BY ServerName, DATEPART(hour, Date) ORDER BY ServerName, DATEPART(hour, Date) ";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable hr10AvarageDeliveryTimeinSeconds(string MyDevice,DateTime starttime,DateTime endtime)
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT Date, StatValue, Name FROM BlackBerryProbeStats WHERE (Name = '"+MyDevice+"') AND (StatValue <> 0) AND (StatName = 'DeliveryTime.Seconds') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') OR"
                            + " (StatValue <> 0) AND (StatName = 'DeliveryTime.Seconds') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') AND ('"+MyDevice+"' = ' ') ORDER BY Date";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable hr11Monthly(DateTime starttime,DateTime endtime)
        {

            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT  DATENAME(month, DATEADD(month, MonthNumber - 1, CAST('2012-01-01' AS datetime))) AS Month, AVG(StatValue) AS StatValue  FROM DeviceDailyStats"
                             + " WHERE (StatValue <> 0) AND (StatName = 'DailyResponseAverage') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') GROUP BY MonthNumber ORDER BY MonthNumber";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;


        }

        public DataTable hr2Rpt(string MyDevice,DateTime startdate,DateTime endtime)
        {

            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT Date, StatValue, Name FROM NotesMailStats WHERE (Name = '"+MyDevice+"') AND (StatValue <> 0) AND (StatName = 'DeliveryTime.Seconds') AND (Date >= '"+startdate+"') AND (Date <= '"+endtime+"') OR"
                        + " (StatValue <> 0) AND (StatName = 'DeliveryTime.Seconds') AND (Date >= '" + startdate + "') AND (Date <= '" + endtime + "') AND ('"+MyDevice+"' = ' ') ORDER BY Date";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;



        }


        public DataTable hr4Rpt(string mydevice,DateTime starttime,DateTime endtime)
        {
             DataTable dt = new DataTable();
             try
             {


                 string str = "SELECT Date, StatValue, DeviceName FROM DeviceDailyStats WHERE (DeviceName = '"+mydevice+"') AND (StatValue <> 0) AND (StatName = 'ResponseTime') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') OR"
                             + " (StatValue <> 0) AND (StatName = 'ResponseTime') AND (Date >= '" + starttime + "') AND (Date <= '" + endtime + "') AND ('"+mydevice+"' = ' ') ORDER BY Date";
                 dt = objAdaptor1.FetchData(str);
             }
             catch (Exception e)
             {
                 throw e;
             }
             return dt;
        }


        public DataTable hr5Rpt(string mydevice,DateTime starttime,DateTime endtime)
        {
             DataTable dt = new DataTable();
             try
             {


                string str="SELECT  Date, StatValue, Name FROM NotesMailStats WHERE (Name ='"+mydevice+"') AND (StatValue <> 0) AND (StatName = 'DeliveryTime.Seconds') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') OR"
                        + " (StatValue <> 0) AND (StatName = 'DeliveryTime.Seconds') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') AND ('"+mydevice+"' = ' ') ORDER BY Date";

             dt=objAdaptor1.FetchData(str);
             }
             catch (Exception e)
             {
                 throw e;
             }

             return dt;
        }

        public DataTable hr6Rpt(string mydevice,DateTime starttime,DateTime endtime)
        {
             DataTable dt = new DataTable();
             try
             {

                 string str="SELECT  Date, StatValue, DeviceName FROM DeviceDailyStats WHERE (DeviceName = '"+mydevice+"') AND (StatValue <> 0) AND (StatName = 'DailyResponseAverage') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') OR"
                        + " (StatValue <> 0) AND (StatName = 'DailyResponseAverage') AND (Date >= '" + starttime + "') AND (Date <= '"+endtime+"') AND ('"+mydevice+"' = ' ') ORDER BY Date";

                 dt = objAdaptor1.FetchData(str);

             }
            catch(Exception e)
            {
                throw   e;
            }
             return dt;
        }

        public DataTable hr7Rpt(string mydevice,DateTime starttime,DateTime endtime)
        {
            DataTable dt=new DataTable();
            try
            {

                string str = "SELECT  Date, StatValue, DeviceName  FROM  DeviceDailyStats WHERE (DeviceName = '"+mydevice+"') AND (StatValue <> 0) AND (StatName = 'DailyDeliveryTimeAverage') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') OR"
                            + " (StatValue <> 0) AND (StatName = 'DailyDeliveryTimeAverage') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') AND ('"+mydevice+"' = ' ') ORDER BY Date";
               dt=  objAdaptor1.FetchData(str);
            }
            catch(Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable hr8Rpt(string mydevice,DateTime starttime,DateTime endtime)
        {
            DataTable dt=new DataTable();
            try
            {

            string str="SELECT WeekNumber, AVG(StatValue) AS StatValue FROM DeviceDailyStats WHERE (DeviceName = '"+mydevice+"') AND (StatValue <> 0) AND (StatName = 'DailyResponseAverage') AND (Date >= '"+starttime+"') AND (Date <='"+endtime+"') OR"
                        +" (StatValue <> 0) AND (StatName = 'DailyResponseAverage') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') AND ('"+mydevice+"' = ' ')GROUP BY WeekNumber ORDER BY WeekNumber";

            dt = objAdaptor1.FetchData(str);

            }
            catch(Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable hr9Rpt(string mydevice,DateTime starttime,DateTime endtime)
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT WeekNumber, AVG(StatValue) AS StatValue FROM DeviceDailyStats WHERE (DeviceName = '"+mydevice+"') AND (StatValue <> 0) AND (StatName = 'DailyDeliveryTimeAverage') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"')"
                            + " GROUP BY WeekNumber ORDER BY WeekNumber";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }


            return dt;
        }

        public DataTable HRDeviceDailyStatus(string mydevice,DateTime starttime,DateTime endtime)
        {
            DataTable dt = new DataTable();
            try
            {

                string str = "SELECT Date, StatValue, DeviceName FROM DeviceDailyStats WHERE (DeviceName = '"+mydevice+"') AND (StatValue <> 0) AND (StatName = 'ResponseTime') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') OR"
                        + " (StatValue <> 0) AND (StatName = 'ResponseTime') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') AND ('"+mydevice+"' = '') ORDER BY Date";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;

        }

        public DataTable HRMonthlyDS(DateTime starttime,DateTime endtime)
        {
            DataTable dt = new DataTable();
            try
            {

                string str = "SELECT DATENAME(month, DATEADD(month, MonthNumber - 1, CAST('2012-01-01' AS datetime))) AS Month, AVG(StatValue) AS StatValue FROM DeviceDailyStats"
                                + " WHERE (StatValue <> 0) AND (StatName = 'DailyResponseAverage') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') GROUP BY MonthNumber ORDER BY MonthNumber";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;


        }


        public DataTable OverallSrvStatusHealthRptDS()
        {
            DataTable dt = new DataTable();
            try
            {

                string str = "Select Name, Status,cast((CPU*100) as varchar(50))  + '/' + cast((CPUThreshold*100) as varchar(50)) CPU, TypeandName,Details,Description,Type,SecondaryRole,Location,PendingMail,PendingThreshold,DeadMail,DeadThreshold,HeldMail,HeldMailThreshold, case when Status = 'Not Responding' then 0 when status = 'OK' then 2 else 1 end SortCol, cast(PendingMail as varchar(50)) + ' ' + cast(DeadMail as varchar(50)) + ' ' + cast(HeldMail as varchar(50)) AllMail from [VitalSigns].[dbo].[Status]"
                              + " where (Location!='')  and StatusCode is not Null";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;


        }

        public DataTable ResponseTimeXtraRpt()
        {
            DataTable dt = new DataTable();
            try
            {

                string str = "select TypeANDName, Name+'/'+Type as Server,ResponseTime from Status where ResponseTime>0";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;



        }


        public DataTable SrvAvailabilityRpt(int month,int year,string ServerName)
        {
            DataTable dt = new DataTable();
            try
            {

                string str = "SELECT ServerName, DATEADD(dd,0,DATEDIFF(dd, 0, Date)) AS date, ROUND(StatValue,1) StatValue, ID " +
                    "FROM DominoSummaryStats WHERE (StatName = 'Server.AvailabilityIndex') and MONTH(date)="+month+" and " +
                    "YEAR(date)="+year+" and (ServerName='"+ServerName+"' OR '"+ServerName+"'='') " +
                    "ORDER BY ServerName, date ";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;


        }

        public DataTable SrvDiskFreeSpaceTrendRptDS(int month,int year,string serverName, string serverType)
        {

            DataTable dt = new DataTable();
            string str = "";
            try
            {
                //2/4/2015 NS modified for VSPLUS-1370
                if (serverType == "" || serverType == "''" || serverType == "'All'")
                {
                    //8/8/2014 NS modified - added Exchange
                    str = "SELECT ID,ServerName,LEFT(StatName,LEN(StatName)-5) DiskName, ServerName + ' - ' + LEFT(StatName,LEN(StatName)-5) AS ServerDiskName, " +
                        "ROUND(StatValue/1024/1024/1024,1) StatValue, Date FROM DominoSummaryStats " +
                        "WHERE MonthNumber=" + month + " AND YearNumber=" + year + " AND statname LIKE 'Disk%Free' ";
                    if (serverName != "")
                    {
                        //8/5/2013 NS modified
                        //str += " AND ServerName + ' - ' + LEFT(StatName,LEN(StatName)-5)='" + serverName + "' ";
                        str += " AND ServerName + ' - ' + LEFT(StatName,LEN(StatName)-5) IN (" + serverName + ") ";
                    }
                    //11/24/2014 NS modified - Exchange table has been renamed
                    str += "UNION " +
                        "SELECT ID,ServerName,LEFT(StatName,LEN(StatName)-1) DiskName, ServerName + ' - ' + LEFT(StatName,LEN(StatName)-1) AS ServerDiskName, " +
                        "ROUND(StatValue/1024/1024/1024,1) StatValue, Date FROM MicrosoftSummaryStats " +
                        "WHERE MonthNumber=" + month + " AND YearNumber=" + year + " AND statname LIKE 'Disk.%' ";
                    if (serverName != "")
                    {
                        //8/5/2013 NS modified
                        //str += " AND ServerName + ' - ' + LEFT(StatName,LEN(StatName)-5)='" + serverName + "' ";
                        str += " AND ServerName + ' - ' + LEFT(StatName,LEN(StatName)-1) IN (" + serverName + ") ";
                    }
                    str += "ORDER BY ServerDiskName, Date";
                }
                else
                {
                    if (serverType.IndexOf("Domino") != -1)
                    {
                        //8/8/2014 NS modified - added Exchange
                        str = "SELECT ID,ServerName,LEFT(StatName,LEN(StatName)-5) DiskName, ServerName + ' - ' + LEFT(StatName,LEN(StatName)-5) AS ServerDiskName, " +
                            "ROUND(StatValue/1024/1024/1024,1) StatValue, Date, 'Domino' AS ServerType FROM DominoSummaryStats " +
                            "WHERE MonthNumber=" + month + " AND YearNumber=" + year + " AND statname LIKE 'Disk%Free' ";
                        if (serverName != "")
                        {
                            //8/5/2013 NS modified
                            //str += " AND ServerName + ' - ' + LEFT(StatName,LEN(StatName)-5)='" + serverName + "' ";
                            str += " AND ServerName + ' - ' + LEFT(StatName,LEN(StatName)-5) IN (" + serverName + ") ";
                        }
                        //11/24/2014 NS modified - Exchange table has been renamed
                        str += "UNION " +
                            "SELECT t1.ID,ServerName,LEFT(StatName,LEN(StatName)-1) DiskName, ServerName + ' - ' + LEFT(StatName,LEN(StatName)-1) AS ServerDiskName, " +
                            "ROUND(StatValue/1024/1024/1024,1) StatValue, Date, t2.ServerType AS ServerType FROM MicrosoftSummaryStats t1 " +
                            "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                            "WHERE MonthNumber=" + month + " AND YearNumber=" + year + " AND statname LIKE 'Disk.%' AND " +
                            "t2.ServerType IN (" + serverType + ") ";
                        if (serverName != "")
                        {
                            //8/5/2013 NS modified
                            //str += " AND ServerName + ' - ' + LEFT(StatName,LEN(StatName)-5)='" + serverName + "' ";
                            str += " AND ServerName + ' - ' + LEFT(StatName,LEN(StatName)-1) IN (" + serverName + ") ";
                        }
                        str += "ORDER BY ServerDiskName, Date";
                    }
                    else
                    {
                        str = "SELECT t1.ID,ServerName,LEFT(StatName,LEN(StatName)-1) DiskName, ServerName + ' - ' + LEFT(StatName,LEN(StatName)-1) AS ServerDiskName, " +
                            "ROUND(StatValue/1024/1024/1024,1) StatValue, Date, t2.ServerType AS ServerType FROM MicrosoftSummaryStats t1 " +
                            "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                            "WHERE MonthNumber=" + month + " AND YearNumber=" + year + " AND statname LIKE 'Disk.%' AND " +
                            "t2.ServerType IN (" + serverType + ") ";
                        if (serverName != "")
                        {
                            //8/5/2013 NS modified
                            //str += " AND ServerName + ' - ' + LEFT(StatName,LEN(StatName)-5)='" + serverName + "' ";
                            str += " AND ServerName + ' - ' + LEFT(StatName,LEN(StatName)-1) IN (" + serverName + ") ";
                        }
                        str += "ORDER BY ServerDiskName, Date";
                    }
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;


        }

        public DataTable SrvTransPerMinRptDS(int month,int year,string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT ServerName, DATEADD(dd,0,DATEDIFF(dd, 0, Date)) AS date, ROUND(StatValue,1) StatValue, ID " +
                    "FROM DominoSummaryStats WHERE (StatName = 'Server.Trans.PerMinute') and MONTH(date)=" + month + 
                    " AND YEAR(date)=" + year;
                if (servername != "")
                {
                    //8/5/2013 NS modified
                    //str += " AND ServerName='" + servername + "' ";
                    str += " AND ServerName IN(" + servername + ") ";
                }
                str += " ORDER BY ServerName, Date";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;
        }

        public DataTable ConfigUserListRpt()
        {
            DataTable dt = new DataTable();
            try
            {

                string str = "select * from configuserslist order by c2";

                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;


        }
        public DataTable LicenseCountServerTypes()
        {

            DataTable dt = new DataTable();
            try
            {

                string str ="SELECT [ServerType],count([ServerName]) UsedLicenses, (select svalue from Settings WHERE sname='License_Count') TotalLicenses FROM [Servers] t1  INNER JOIN [ServerTypes] t3 ON [ServerTypeID] = t3.[ID] " +
                            "INNER JOIN [DominoServers] t4 ON t1.[ID] = t4.[ServerID]  " +
                            "WHERE [Enabled]='True'  and ServerType='Domino' group by  [ServerType] " +
                            "union " +
                            "SELECT [ServerType],count([ServerName]) UsedLicenses, (select svalue from Settings WHERE sname='License_Count') TotalLicenses  FROM [Servers] t1  INNER JOIN [ServerTypes] t3 ON [ServerTypeID] = t3.[ID]  " +
                            "INNER JOIN [BlackBerryServers] t4 ON t1.[ID] = t4.[ServerID]  " +
                            "WHERE [Enabled]='True' and ServerType='BES' group by  [ServerType] " +
                            "union " +
                            "SELECT [ServerType],count([ServerName]) UsedLicenses, (select svalue from Settings WHERE sname='License_Count') TotalLicenses  FROM [Servers] t1  INNER JOIN [ServerTypes] t3 ON [ServerTypeID] = t3.[ID]  " +
                            "INNER JOIN [SametimeServers] t4 ON t1.[ID] = t4.[ServerID]  " +
                            "WHERE [Enabled]='True' and ServerType='Sametime' group by  [ServerType] " +
                            "union " +
                            "SELECT [ServerType],count(t4.ServerName) UsedLicenses, (select svalue from Settings WHERE sname='License_Count')*4 TotalLicenses FROM [MailServices] t4  INNER JOIN [ServerTypes] t3 ON [ServerTypeID] = t3.[ID] WHERE [Enabled]='True' and ServerType='Mail' group by  [ServerType] " +
                            "union " +
                            "SELECT [ServerType],count(t4.TheURL) UsedLicenses, (select svalue from Settings WHERE sname='License_Count')*2 TotalLicenses  FROM [URLs] t4  INNER JOIN [ServerTypes] t3 ON [ServerTypeID] = t3.[ID] WHERE [Enabled]='True' and ServerType='URL' group by  [ServerType] " +
                            "union " +
                            "SELECT  [ServerType],count(t4.Name) UsedLicenses,(select svalue from Settings WHERE sname='License_Count') TotalLicenses  FROM [Network Devices] t4  INNER JOIN [ServerTypes] t3 ON [ServerTypeID] = t3.[ID] WHERE [Enabled]='True' and ServerType='Network Device' group by  [ServerType] " +
                            "union " +
                            "SELECT 'Notes Database' ServerType,count(t4.Name) UsedLicenses, (select svalue from Settings WHERE sname='License_Count')*5 TotalLicenses  FROM [NotesDatabases] t4 WHERE [Enabled]='True'";

                dt = objAdaptor.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;

        }

        public DataTable LicenseCountServer()
        {

            DataTable dt = new DataTable();
            try
            {

                string str = "SELECT t1.[ID],[ServerName], [ServerType], [Location]  FROM [Servers] t1 INNER JOIN [Locations] t2 ON [LocationID] = t2.[ID]"
                                + " INNER JOIN [ServerTypes] t3 ON [ServerTypeID] = t3.[ID] INNER JOIN [DominoServers] t4 ON t1.[ID] = t4.[ServerID] WHERE [ServerType]='Domino' AND [Enabled]='True'";

                dt = objAdaptor.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;

        }

        public DataTable LicenseCountSetting()
        {

            DataTable dt = new DataTable();
            try
            {

                string str = "SELECT svalue -(SELECT Count(t1.[ID]) FROM [Servers] t1 INNER JOIN [Locations] t2 ON [LocationID] = t2.[ID]  INNER JOIN [ServerTypes] t3 ON [ServerTypeID] = t3.[ID]"
                              + " INNER JOIN [DominoServers] t4 ON t1.[ID] = t4.[ServerID] WHERE [ServerType]='Domino' AND [Enabled]='True') svalue from Settings WHERE sname='License_Count'";

                dt = objAdaptor.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;
        }

        public DataTable LogFileScanRptDS()
        {
            DataTable dt = new DataTable();
            try
            {

                string str = "SELECT svalue -(SELECT Count(t1.[ID]) FROM [Servers] t1 INNER JOIN [Locations] t2 ON [LocationID] = t2.[ID]  INNER JOIN [ServerTypes] t3 ON [ServerTypeID] = t3.[ID]"
                              + " INNER JOIN [DominoServers] t4 ON t1.[ID] = t4.[ServerID] WHERE [ServerType]='Domino' AND [Enabled]='True') svalue from Settings WHERE sname='License_Count'";

                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;



        }

        public DataTable MailFileXtraRpt()
        {
            DataTable dt = new DataTable();
            try
            {

                string str = "SELECT FileName, Title, FileSize, Server, DesignTemplateName, DocumentCount, PercentUsed, FileNamePath, ID FROM Daily WHERE IsMailFile=1 AND (FileSize > @FileSize OR @FileSize = 0)";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;



        }

        public SqlDataSource Hr1Rpt(string mydevice,DateTime starttime,DateTime endtime)
        {
            DataSet ds = new DataSet();
           SqlDataSource datasource = new SqlDataSource();
           // DataTable dt = new DataTable();
            try
            {
               // con.Open();

                string str = "SELECT Date, StatValue, DeviceName FROM DeviceDailyStats WHERE(DeviceName = '"+mydevice+"') AND (StatValue <> 0) AND (StatName = 'ResponseTime') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') OR"
                        +" (StatValue <> 0) AND (StatName = 'ResponseTime') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') AND ('"+mydevice+"' = '') ORDER BY Date";
                 SqlDataAdapter da = new SqlDataAdapter(str, con);
                 
                 datasource.SelectCommand = str;               
                
              
                // datasource.SelectCommand(str);
                  //da.Fill(ds);



              //  ds = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return datasource;
        }

        public DataTable Hr3Rpt(string mydevice,DateTime starttime,DateTime endtime)
        {

            DataTable dt = new DataTable();
            try
            {

                string str = "SELECT  Date, StatValue, Name FROM BlackBerryProbeStats WHERE (Name = '"+mydevice+"') AND (StatValue <> 0) AND (StatName = 'DeliveryTime.Seconds') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') OR"
                        + " (StatValue <> 0) AND (StatName = 'DeliveryTime.Seconds') AND (Date >= '"+starttime+"') AND (Date <= '"+endtime+"') AND ('"+mydevice+"' = ' ') ORDER BY Date";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;

        }

        //9/16/2014 NS added for VSPLUS-456
        public DataTable DeviceUptimePct(string servername, DateTime startdt, DateTime enddt, string servertype)
        {
            DataTable dt = new DataTable();
            string str;
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
            string startdtstr = startdt.ToString(ci);
            string enddtstr = enddt.ToString(ci);
            try
            {
                if (servertype == "" || servertype == "''" || servertype == "'All'")
                {
                    if (servername == "")
                    {
                        //8/25/2015 NS modified for VSPLUS-1619
                        /*
                        str = "SELECT *,ROUND(StatValue,1) StatValueR FROM DeviceUpTimeSummaryStats " +
                            "WHERE StatName='DailyUpTimePercent' AND " +
                            "DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,Date,101))) BETWEEN " +
                            "DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + startdtstr + "',101))) AND " +
                            "DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + enddtstr + "',101))) " +
                            "ORDER BY ServerName";
                         */
                        str = "SELECT *,ROUND(StatValue,1) StatValueR FROM DeviceUpTimeSummaryStats t1 " +
                            "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                            "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                            "AND t2.ID=t3.DeviceTypeID " +
                            "WHERE StatName='DailyUpTimePercent' AND " +
                            "DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,Date,101))) BETWEEN " +
                            "DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + startdtstr + "',101))) AND " +
                            "DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + enddtstr + "',101))) " +
                            "ORDER BY ServerName";
                    }
                    else
                    {
                        //8/25/2015 NS modified for VSPLUS-1619
                        /*
                        str = "SELECT *,ROUND(StatValue,1) StatValueR FROM DeviceUpTimeSummaryStats " +
                            "WHERE StatName='DailyUpTimePercent' AND " +
                            "DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,Date,101))) BETWEEN " +
                            "DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + startdtstr + "',101))) AND " +
                            "DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + enddtstr + "',101))) AND " +
                            "ServerName IN (" + servername + ") " +
                            "ORDER BY ServerName";
                         */
                        str = "SELECT *,ROUND(StatValue,1) StatValueR FROM DeviceUpTimeSummaryStats t1 " +
                            "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                            "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                            "AND t2.ID=t3.DeviceTypeID " +
                            "WHERE StatName='DailyUpTimePercent' AND " +
                            "DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,Date,101))) BETWEEN " +
                            "DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + startdtstr + "',101))) AND " +
                            "DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + enddtstr + "',101))) AND " +
                            "ServerName IN (" + servername + ") " +
                            "ORDER BY ServerName";
                    }
                }
                else
                {
                    if (servername == "")
                    {
                        //8/25/2015 NS modified for VSPLUS-1619
                        str = "SELECT *,ROUND(StatValue,1) StatValueR FROM DeviceUpTimeSummaryStats t1 " +
                            "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                            "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                            "AND t2.ID=t3.DeviceTypeID " +
                            "WHERE StatName='DailyUpTimePercent' AND " +
                            "DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,Date,101))) BETWEEN " +
                            "DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + startdtstr + "',101))) AND " +
                            "DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + enddtstr + "',101))) AND " +
                            "DeviceType IN(" + servertype + ") " +
                            "ORDER BY ServerName";
                    }
                    else
                    {
                        //8/25/2015 NS modified for VSPLUS-1619
                        /*
                        str = "SELECT *,ROUND(StatValue,1) StatValueR FROM DeviceUpTimeSummaryStats " +
                            "WHERE StatName='DailyUpTimePercent' AND " +
                            "DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,Date,101))) BETWEEN " +
                            "DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + startdtstr + "',101))) AND " +
                            "DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + enddtstr + "',101))) AND " +
                            "ServerName IN (" + servername + ") AND " +
                            "DeviceType IN (" + servertype + ") " +
                            "ORDER BY ServerName";
                         */
                        str = "SELECT *,ROUND(StatValue,1) StatValueR FROM DeviceUpTimeSummaryStats t1 " +
                            "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                            "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                            "AND t2.ID=t3.DeviceTypeID " +
                            "WHERE StatName='DailyUpTimePercent' AND " +
                            "DATEADD(dd, 0, DATEDIFF(dd, 0, convert(datetime,Date,101))) BETWEEN " +
                            "DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + startdtstr + "',101))) AND " +
                            "DATEADD(dd,0, DATEDIFF(dd, 0,convert(datetime,'" + enddtstr + "',101))) AND " +
                            "ServerName IN (" + servername + ") AND " +
                            "DeviceType IN (" + servertype + ") " +
                            "ORDER BY ServerName";
                    }
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //12/18/2015 NS added for VSPLUS-2291
        //26/4/2016 Durga Modified for VSPLUS-2883
        public DataTable MonthlyMailVolumeRptDS(string ServerName,string ServerType)
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                if(ServerType=="Domino")
                str = "SELECT ServerName, StatName, SUM(StatValue) StatValue, DATEADD(MONTH, DATEDIFF(MONTH, 0, Date), 0) AS MonthYear " +
                        "FROM DominoSummaryStats WHERE (StatName = 'Mail.Delivered' OR StatName = 'Mail.Transferred' " +
                        "OR StatName = 'Mail.TotalRouted') AND DATEDIFF(dd,0,date) <= DATEDIFF(dd,0,GETDATE()) AND " +
                        "DATEDIFF(mm,0,date) >= DATEDIFF(mm,0,DATEADD(mm,-6,GETDATE())) ";
                else if (ServerType == "Exchange")
                    str = "SELECT ServerName, StatName, SUM(StatValue) StatValue, DATEADD(MONTH, DATEDIFF(MONTH, 0, Date), 0) AS MonthYear " +
                      "FROM MicrosoftSummaryStats WHERE (StatName = 'Mail_SentCount' OR StatName = 'Mail_ReceivedCount' " +
                      "OR StatName = 'Mail_FailCount' OR StatName ='Mail_DeliverCount') AND DATEDIFF(dd,0,date) <= DATEDIFF(dd,0,GETDATE()) AND " +
                      "DATEDIFF(mm,0,date) >= DATEDIFF(mm,0,DATEADD(mm,-6,GETDATE())) AND ServerTypeId=5 ";
                else
                    str = "SELECT ServerName, StatName, SUM(StatValue) StatValue, DATEADD(MONTH, DATEDIFF(MONTH, 0, Date), 0) AS MonthYear " +
                "FROM MicrosoftSummaryStats WHERE (StatName = 'Mail_SentCount' OR StatName = 'Mail_ReceivedCount' " +
                ") AND DATEDIFF(dd,0,date) <= DATEDIFF(dd,0,GETDATE()) AND " +
                "DATEDIFF(mm,0,date) >= DATEDIFF(mm,0,DATEADD(mm,-6,GETDATE()))  AND ServerTypeId=21 ";
                if (ServerName != "")
                {
                    str += "AND ServerName IN (" + ServerName + ") ";
                }
                str += "GROUP BY ServerName,StatName,DATEADD(MONTH, DATEDIFF(MONTH, 0, Date), 0) ";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        //22/4/2016 Durga added for  VSPLUS-2806
        public DataTable GetServersListFromDominoDailyStats(string StatName)
             {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                str = "SELECT DISTINCT DS.ServerName " +
                        "FROM DominoDailyStats DS " +
                        "INNER JOIN vitalsigns.dbo.status St ON DS.ServerName=St.Name " +
                        "WHERE (exjournal != -1 AND exjournal1 != -1 and exjournal2 != -1) and (StatName = '" + StatName + "') " +
                        "ORDER BY ServerName ";
              
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
     //Durga Modified for VSPLUS-2806
        public DataTable GetDominoDailyStatsInfo(string servername, string Threshold,string StatName)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {


                query = "select servername,ROUND(StatValue,0) as StatValue,CASE WHEN [Hour] = 0 THEN '12:00 AM'  WHEN [Hour] < 12 THEN CAST([Hour] AS VarChar) + ':00 AM' " +

     " WHEN [Hour] = 12 THEN '12:00 PM'   ELSE CAST(([Hour]-12) AS VarChar) + ':00 PM' END AS date  from [GetHourlydataforEXJournalDocCountTotal] "+
                "  Gh  INNER JOIN vitalsigns.dbo.status St ON Gh.ServerName=St.Name  where (exjournal != -1 AND exjournal1 != -1 and exjournal2 != -1)";

          

                if (servername != "")
                {

                    query += "AND ServerName IN (" + servername + ") ";
                }

                if (Threshold != "")
                {
                    query += "AND StatValue >= " + Threshold;
                }
                query += " ORDER BY ServerName";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //Durga Added for VSPLUS-2993 
        public DataTable getExchangeMailboxNames(string Type)

        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                if (Type == "Databases")
                    str = "select distinct SUBSTRING(StatName,LEN('ExDatabaseSizeMb.')+1,LEN(StatName)-LEN('ExDatabaseSizeMb.')) as DocumentName,statname from [MicrosoftSummaryStats] Ms INNER JOIN vitalsigns.dbo.Servers Sr ON Ms.ServerName=Sr.ServerName where statname like 'ExDatabaseSizeMb%' and MS.ServerTypeId=5";
                else
                    str = "select distinct  Ms.ServerName from [MicrosoftSummaryStats] Ms INNER JOIN vitalsigns.dbo.Servers Sr ON Ms.ServerName=Sr.ServerName where statname like 'SizeOfMailBoxes' and MS.ServerTypeId=5";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetExchangeMailboxData(string Name, DateTime starttime, DateTime endtime, string threshold)
        {
            DataTable dt = new DataTable();
            string str = "";
            Name = "%" + Name + "%";
            try
            {
              
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");

                string start = starttime.ToString(ci);
                string end = endtime.ToString(ci);


                str = "select distinct(SUBSTRING(StatName,LEN('ExDatabaseSizeMb.')+1,LEN(StatName)-LEN('ExDatabaseSizeMb.'))) as DocumentName,StatName," +
                                "round(statvalue,1) as statvalue,Ms.ServerName,Date from [MicrosoftSummaryStats] Ms INNER JOIN vitalsigns.dbo.Servers Sr  " +
                                "ON Ms.ServerName=Sr.ServerName where statname like 'ExDatabaseSizeMb%' AND " +
                                 "(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) " +
                                "AND " +
                                "(convert(datetime,Date,101) <= convert(datetime,'" + end + "',101))  AND StatName like  '" + Name + "' and MS.ServerTypeId=5 ";
                              

                //    if (Name != "")
                //{

                //    str += "AND StatName IN (" + Name + ") ";
                //}
               
                if (threshold != "")
                {
                    str += "AND StatValue >= " + threshold;
                }
                str += "order by DocumentName";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetServerNames(string Type)
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                if (Type == "Databases")
                    str = "select distinct MS.ServerName from [MicrosoftSummaryStats] Ms INNER JOIN vitalsigns.dbo.Servers Sr ON Ms.ServerName=Sr.ServerName where statname like 'ExDatabaseSizeMb%' order by ms.ServerName";

                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        public DataTable GetO365MailboxesInfo()
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {

                str = "select  distinct statname from [MicrosoftSummaryStats] Ms INNER JOIN vitalsigns.dbo.O365Server  Sr ON Ms.ServerName=Sr.Name where statname like '%TotalItems%' and MS.ServerTypeId=21";

                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        public DataTable GetO365MailboxData(DateTime starttime, DateTime endtime, string threshold)
        {
            DataTable dt = new DataTable();
            string str = "";
           
            try
            {

                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");

                string start = starttime.ToString(ci);
                string end = endtime.ToString(ci);


                str = "select StatName," +
                              "round(statvalue,1) as statvalue,Ms.ServerName,Date from [MicrosoftSummaryStats] Ms INNER JOIN vitalsigns.dbo.O365Server  Sr  " +
                              "ON Ms.ServerName=Sr.Name where " +
                               "(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) " +
                              "AND " +
                              "(convert(datetime,Date,101) <= convert(datetime,'" + end + "',101))  AND StatName like  'SizeOfMailBoxes' and MS.ServerTypeId=21 ";
                if (threshold != "")
                {
                    str += "AND StatValue >= " + threshold;
                }
                str += "order by StatName";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetExchangeServerData(string Name, DateTime starttime, DateTime endtime, string threshold)
        {
            DataTable dt = new DataTable();
            string str = "";
          
            try
            {

                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");

                string start = starttime.ToString(ci);
                string end = endtime.ToString(ci);


                str = "select StatName," +
                                "round(avg(statvalue),1) as statvalue,Ms.ServerName,Date from [MicrosoftSummaryStats] Ms INNER JOIN vitalsigns.dbo.Servers Sr  " +
                                "ON Ms.ServerName=Sr.ServerName where  " +
                                 "(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) " +
                                "AND " +
                                "(convert(datetime,Date,101) <= convert(datetime,'" + end + "',101))  AND StatName like  'SizeOfMailBoxes' and MS.ServerTypeId=5 ";


              
                if (threshold != "")
                {
                    str += "AND StatValue >= " + threshold;
                }
                if (Name != "")
                {
                    str += "AND Ms.ServerName = '"  +Name + "' ";
                }
                str += " group by date,StatName,Ms.ServerName";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }


        //6/3/2016 Sowjanya modified for VSPLUS-2895
        public DataTable ConnectionTags(string ServerName)
        {
            string str = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (ServerName == "")
                {

                    str = "select top (50)  IT.Tag, IO.Name, count(IT.Tag) as Count from dbo.IbmConnectionsTags IT inner join dbo.IbmConnectionsObjectTags IOT on IOT.tagid = IT.id " +
                           "inner join IbmConnectionsObjects IO on IO.ID = IOT.ObjectID where IT.Tag != '' " +
                           "group by  IT.Tag, IO.Name  ";
   
                }
                else
                {
                   
                    str = "select top (50) IT.Tag,IO.Name,count(*) as Count from dbo.IbmConnectionsTags IT inner join dbo.IbmConnectionsObjectTags IOT on IOT.tagid = IT.id " +
                          "inner join IbmConnectionsObjects IO on IO.ID = IOT.ObjectID  where Name = '" + ServerName + "' and IT.Tag != '' " +
                          "group by IT.Tag,IO.Name order by Count desc ";
                       
                }
              
                dt = objAdaptor.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;
        }

        //6/3/2016 Sowjanya modified for VSPLUS-2895
        public DataTable GetTagsCount(string ServerName)
        {
            string query = string.Empty;
            DataTable dt = new DataTable();
            try
            {

                if (ServerName == "")
                {
                    query = "select top (50) tag,count(tagid) as tagcount from dbo.IbmConnectionsTags inner join dbo.IbmConnectionsObjectTags " +
                            " on tagid=id where Tag != ''  group by tag order by tagcount desc";
                }
                else
                {
                    query = "select  top (50) tag,count(*) as tagcount from dbo.IbmConnectionsTags IT inner join dbo.IbmConnectionsObjectTags IOT on IOT.TagId=IT.ID  " +
                             " inner join IbmConnectionsObjects IO on IO.ID = IOT.ObjectID    where IO.Name='" + ServerName + "'  and  IT.Tag != '' " + 
                            " group by tag order by tagcount desc ";
                            
                }

                dt = objAdaptor.FetchData(query);

            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;
        }

    }
}
