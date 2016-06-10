using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace VSWebDAL.ReportsDAL
{
    public class ReportsDAL
    {

        private AdaptorforDsahBoard objAdaptor1 = new AdaptorforDsahBoard();
        private Adaptor objAdaptor = new Adaptor();
        private static ReportsDAL _self = new ReportsDAL();

        public static ReportsDAL Ins
        {
            get
            {
                return _self;
            }

        }

        public DataTable getDominoSummaryStats(string ServerType)
        {
            //2/6/2015 NS modified for VSPLUS-1370
            DataTable d = new DataTable();
            string str = "";
            try
            {
                //8/8/2014 NS modified
                //11/24/2014 NS modified the name of the Exchange table to Microsoft
                if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                {
                    str = "SELECT DISTINCT [ServerName] FROM DominoSummaryStats " +
                       "WHERE StatName LIKE 'Disk%Free' " +
                       "UNION " +
                       "SELECT DISTINCT [ServerName] FROM MicrosoftSummaryStats " +
                       "WHERE StatName LIKE 'Disk.%' " +
                       "ORDER BY [ServerName]";
                }
                else
                {
                    if (ServerType.IndexOf("Domino") != -1)
                    {
                        str = "SELECT DISTINCT [ServerName] FROM DominoSummaryStats " +
                           "WHERE StatName LIKE 'Disk%Free' " +
                           "UNION " +
                           "SELECT DISTINCT [ServerName] FROM MicrosoftSummaryStats t1 " +
                           "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                           "WHERE StatName LIKE 'Disk.%' AND " +
                           "t2.ServerType IN(" + ServerType + ") " +
                           "ORDER BY [ServerName]";
                    }
                    else
                    {
                        str = "SELECT DISTINCT [ServerName] FROM MicrosoftSummaryStats t1 " +
                           "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                           "WHERE StatName LIKE 'Disk.%' AND " +
                           "t2.ServerType IN(" + ServerType + ") " +
                           "ORDER BY [ServerName]";
                    }
                }
                d = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return d;
        }

        public DataTable getDominoSummaryDiskStats(string ServerType)
        {
            //2/6/2015 NS modified for VSPLUS-1370
            DataTable d = new DataTable();
            string str = "";
            try
            {
                //8/8/2014 NS modified
                //11/24/2014 NS modified the name of the Exchange table to Microsoft
                if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                {
                    str = "SELECT DISTINCT SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) AS DiskName " +
                        "FROM DominoSummaryStats " +
                        "WHERE StatName LIKE 'Disk%Free' " +
                        "UNION " +
                        "SELECT DISTINCT SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName " +
                        "FROM MicrosoftSummaryStats " +
                        "WHERE StatName LIKE 'Disk.%' " +
                        "ORDER BY DiskName ";
                }
                else
                {
                    if (ServerType.IndexOf("Domino") != -1)
                    {
                        str = "SELECT DISTINCT SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) AS DiskName " +
                        "FROM DominoSummaryStats " +
                        "WHERE StatName LIKE 'Disk%Free' " +
                        "UNION " +
                        "SELECT DISTINCT SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName " +
                        "FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                        "WHERE StatName LIKE 'Disk.%' AND " +
                        "t2.ServerType IN(" + ServerType + ") " +
                        "ORDER BY DiskName ";
                    }
                    else
                    {
                        str = "SELECT DISTINCT SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName " +
                        "FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                        "WHERE StatName LIKE 'Disk.%' AND " +
                        "t2.ServerType IN(" + ServerType + ") " +
                        "ORDER BY DiskName ";
                    }
                }
                d = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return d;
        }

        public DataTable getdistinctDominoSummary(string ServerType)
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                //12/12/2013 NS modified (column name change)
                //string str = "SELECT DISTINCT [DeviceName] FROM [DeviceDailyStats] ORDER BY [DeviceName]";
                //2/4/2015 NS modified for VSPLUS-1370
                if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                {
                    //8/25/2015 NS modified for VSPLUS-1619
                    //str = "SELECT DISTINCT [ServerName] DeviceName FROM [DeviceDailyStats] ORDER BY [ServerName]";
                    str = "SELECT DISTINCT [ServerName] DeviceName FROM [DeviceDailyStats] t1 " +
                        "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
						"DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
						"AND t2.ID=t3.DeviceTypeID " +
                        "ORDER BY [ServerName]";
                }
                else
                {
                    //8/25/2015 NS modified for VSPLUS-1619
                    //str = "SELECT DISTINCT [ServerName] DeviceName FROM [DeviceDailyStats] " +
                    //    "WHERE DeviceType IN(" + ServerType + ") " +
                    //    "ORDER BY [ServerName]";
                    str = "SELECT DISTINCT [ServerName] DeviceName FROM [DeviceDailyStats] t1 " +
                        "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
						"DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
						"AND t2.ID=t3.DeviceTypeID " +
                        "WHERE DeviceType IN(" + ServerType + ") " +
                        "ORDER BY [ServerName]";
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch(Exception e)
            {
                throw e;    
            }
            return dt;

        }


        public DataTable getDominoDailyStats()
        {
            DataTable dt = new DataTable();
            try
            {
                //7/10/2015 NS modified for VSPLUS-1887
                string str = "SELECT DISTINCT [ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] " +
                    "WHERE StatName='Replica.Cluster.SecondsOnQueue' " +
                    "ORDER BY ServerName ";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //3-5-2016 Durga Added for VSPLUS-2883
        public DataTable getDailyMailVolume(string ServerType)
        {
            DataTable dt = new DataTable();
            string str;
            try
            {
                //8/25/2015 NS modified for VSPLUS-1619
                //string str = "SELECT DISTINCT ServerName FROM DominoDailyStats  ORDER BY ServerName";
                if(ServerType=="Domino")
                    str = "SELECT DISTINCT ServerName FROM DominoSummaryStats  where  StatName in('Mail.Delivered','Mail.Transferred','Mail.TotalRouted') ORDER BY ServerName";
                else if (ServerType == "Exchange")
                    str = "SELECT DISTINCT ServerName FROM MicrosoftSummaryStats where ServerTypeId=5 and  StatName in( 'Mail_SentCount','Mail_ReceivedCount', 'Mail_FailCount','Mail_DeliverCount') ORDER BY ServerName";
                else
                    str = "SELECT DISTINCT ServerName FROM MicrosoftSummaryStats where ServerTypeId=21 and StatName in('Mail_SentCount','Mail_ReceivedCount')  ORDER BY ServerName";

                dt = objAdaptor1.FetchData(str);


            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }


        public DataTable getDailyMemoryUsed(string ServerType)
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                //2/2/2015 NS modified
                if (ServerType == "" || ServerType == "'All'")
                {
                    str = "SELECT DISTINCT ServerName FROM [DominoSummaryStats] " +
                        "WHERE StatName='Mem.PercentUsed' " +
                        "UNION " +
                        "SELECT DISTINCT ServerName FROM [MicrosoftSummaryStats] " +
                        "WHERE StatName='Mem.PercentAvailable'  " +
                        "ORDER BY ServerName";
                }
                else
                {
                    if (ServerType == "'Domino'")
                    {
                        str = "SELECT DISTINCT ServerName FROM [DominoSummaryStats] " +
                        "WHERE StatName='Mem.PercentUsed' " +
                        "ORDER BY ServerName";
                    }
                    else
                    {
                        str = "SELECT DISTINCT ServerName FROM [MicrosoftSummaryStats] t1 " +
                            "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                            "WHERE StatName='Mem.PercentAvailable' AND ServerType IN (" + ServerType + ") " +
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

        //2/5/2015 NS modified for VSPLUS-1370
        public DataTable getDeviceHourlyOnTargetPctRpt(string ServerType)
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                {
                    str = "SELECT distinct devicename FROM [DeviceUpTimeStats] where statname='HourlyOnTargetPercent' order by devicename";
                }
                else
                {
                    str = "SELECT distinct devicename FROM [DeviceUpTimeStats] where statname='HourlyOnTargetPercent' " +
                        "AND devicetype IN(" + ServerType + ") " +
                        "order by devicename";
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //2/5/2015 NS added for VSPLUS-1370
        public DataTable getDeviceHourlyOnTargetPctServerTypes()
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT distinct devicetype, 1 AS OrderNum FROM [DeviceUpTimeStats] where statname='HourlyOnTargetPercent' " +
                    "UNION " +
                    "SELECT 'All' AS devicetype, 0 AS OrderNum " +
                    "order by OrderNum,devicetype";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable DeviceUptimeRpt(string ServerType)
        {

            DataTable dt = new DataTable();
            string str = "";
            try
            {
                if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                {
                    //7/29/2015 NS modified for VSPLUS-2024
                    str = "SELECT DISTINCT DeviceName " +
                        "FROM DeviceUptimeStats " +
                        "WHERE StatName='HourlyDownTimeMinutes' " +
                        "ORDER BY DeviceName";
                }
                else
                {
                    //7/29/2015 NS modified for VSPLUS-2024
                    str = "SELECT DISTINCT DeviceName " +
                        "FROM DeviceUptimeStats " +
                        "WHERE DeviceType=" + ServerType + " " +
                        "AND StatName='HourlyDownTimeMinutes' " +
                        "ORDER BY DeviceName";
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch(Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable DominoDiskHealthLocRpt()
        {
            DataTable dt = new DataTable();
            try
            {
                //string str = "SELECT DISTINCT [ServerName] FROM DominoDiskSpace ORDER BY ServerName";
                //8/5/2014 NS modified - added Exchange servers to the selection
                //11/4/2015 NS modified for VSPLUS-2023 
                string str = "select distinct lc.Location,lc.Id from Locations lc,Servers srv, DominoDiskSpace dds,ServerTypes st " +
                    "where dds.servername=srv.ServerName and srv.ServerTypeId=st.ID and srv.LocationID=lc.Id and " +
                    "st.ServerType='Domino' " +
                    "union " +
                    "select distinct lc.Location,lc.Id from Locations lc,Servers srv, DiskSpace dds,ServerTypes st " +
                    "where dds.servername=srv.ServerName and srv.ServerTypeId=st.ID and srv.LocationID=lc.Id " +
                    "order by lc.Location";
                dt = objAdaptor.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;    
            }

            return dt;
        }

       
        public DataTable DominoDiskHealthRpt()
        {
            DataTable dt = new DataTable();
            try
            {
                //8/5/2014 NS modified - added Exchange servers to the selection
                string str = "SELECT DISTINCT [ServerName] FROM DominoDiskSpace " +
                    "UNION " +
                    "SELECT DISTINCT [ServerName] FROM DiskSpace " + 
                    "ORDER BY ServerName";
                dt = objAdaptor.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable DominoResponseTimesMonthlyRpt()
        {
            DataTable dt = new DataTable();
            try
            {
                //12/12/2013 NS modified (column name change)
                //string str = "select distinct DeviceName from dbo.DeviceDailyStats where devicetype='Domino' order by DeviceName";
                //8/25/2015 NS modified for VSPLUS-1619
                //string str = "select distinct ServerName DeviceName from dbo.DeviceDailyStats where devicetype='Domino' order by ServerName";
                string str = "SELECT DISTINCT ServerName DeviceName FROM dbo.DeviceDailyStats t1 " +
                    "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON DeviceType=ServerType " +
                    "INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                    "AND t2.ID=t3.DeviceTypeID " +
                    "WHERE devicetype='Domino' ORDER BY ServerName";
                dt = objAdaptor1.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable DominoSrvCPUUtilRpt()
        {
            DataTable dt = new DataTable();
            try
            {
                //8/25/2015 NS modified for VSPLUS-1619
                string str = "SELECT DISTINCT [ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] t1 " +
                    "INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON " +
                    "ServerName=Name AND t3.DeviceTypeID=1 " +
                    "ORDER BY ServerName";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable ServerAvailabilityRpt(int month, int year, string ServerName,bool exactmatch, string downMin, string ServerType, int day)
        {
            DataTable dt = new DataTable();
            string str = "";
            string date = "";
            try
            {
                date = year.ToString() + "-" + month.ToString() + "-" + day.ToString();
                if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                {
                    if (ServerName == "" || ServerName == "''")
                    {
                        str = "SELECT DeviceName,ROUND(SUM(StatValue),0) DownMinutes, " +
                            "CASE WHEN MONTH(CAST('" + date + "' as datetime))=MONTH(GETDATE()) AND YEAR(CAST('" + date + "' as datetime))=YEAR(GETDATE()) THEN " +
                            "ROUND((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60+DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0), GETDATE())-SUM(StatValue)),0) " +
                            "ELSE ROUND((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60-SUM(StatValue)),0) END UpMinutes,MonthNumber,YearNumber, " +
                            "CASE WHEN MONTH(CAST('" + date + "' as datetime))=MONTH(GETDATE()) AND YEAR(CAST('" + date + "' as datetime))=YEAR(GETDATE()) THEN " +
                            "(1-ROUND(SUM(StatValue)/((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60+DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0), GETDATE()))),4))*100 " +
                            "ELSE (1-ROUND(SUM(StatValue)/((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60)),4))*100 END UpPct " +
                            "FROM dbo.DeviceUpTimeStats " +
                            "WHERE StatName='HourlyDownTimeMinutes' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                            "GROUP BY DeviceName,MonthNumber,YearNumber ";
                        //12/24/2013 NS added
                        if (downMin != "")
                        {
                            //11/5/2015 NS modified for VSPLUS-2023
                            str += " HAVING ROUND(SUM(StatValue),0) > " + downMin + " ";
                        }
                        str += "ORDER BY YearNumber DESC, MonthNumber DESC, DeviceName";
                    }
                    else
                    {
                        if (exactmatch)
                        {
                            str = "SELECT DeviceName,ROUND(SUM(StatValue),0) DownMinutes, " +
                                "CASE WHEN MONTH(CAST('" + date + "' as datetime))=MONTH(GETDATE()) AND YEAR(CAST('" + date + "' as datetime))=YEAR(GETDATE()) THEN " +
                                "ROUND((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60+DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0), GETDATE())-SUM(StatValue)),0) " +
                                "ELSE ROUND((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60-SUM(StatValue)),0) END UpMinutes,MonthNumber,YearNumber, " +
                                "CASE WHEN MONTH(CAST('" + date + "' as datetime))=MONTH(GETDATE()) AND YEAR(CAST('" + date + "' as datetime))=YEAR(GETDATE()) THEN " +
                                "(1-ROUND(SUM(StatValue)/((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60+DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0), GETDATE()))),4))*100 " +
                                "ELSE (1-ROUND(SUM(StatValue)/((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60)),4))*100 END UpPct " +
                                "FROM dbo.DeviceUpTimeStats " +
                                "WHERE StatName='HourlyDownTimeMinutes' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                                "AND DeviceName IN(" + ServerName + ") " +
                                "GROUP BY DeviceName,MonthNumber,YearNumber ";
                            //12/24/2013 NS added
                            if (downMin != "")
                            {
                                //11/5/2015 NS modified for VSPLUS-2023
                                str += " HAVING ROUND(SUM(StatValue),0) > " + downMin + " ";
                            }
                            str += "ORDER BY YearNumber DESC, MonthNumber DESC, DeviceName";
                        }
                        else
                        {
                            str = "SELECT DeviceName,ROUND(SUM(StatValue),0) DownMinutes, " +
                                "CASE WHEN MONTH(CAST('" + date + "' as datetime))=MONTH(GETDATE()) AND YEAR(CAST('" + date + "' as datetime))=YEAR(GETDATE()) THEN " +
                                "ROUND((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60+DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0), GETDATE())-SUM(StatValue)),0) " +
                                "ELSE ROUND((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60-SUM(StatValue)),0) END UpMinutes,MonthNumber,YearNumber, " +
                                "CASE WHEN MONTH(CAST('" + date + "' as datetime))=MONTH(GETDATE()) AND YEAR(CAST('" + date + "' as datetime))=YEAR(GETDATE()) THEN " +
                                "(1-ROUND(SUM(StatValue)/((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60+DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0), GETDATE()))),4))*100 " +
                                "ELSE (1-ROUND(SUM(StatValue)/((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60)),4))*100 END UpPct " +
                                "FROM dbo.DeviceUpTimeStats " +
                                "WHERE StatName='HourlyDownTimeMinutes' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                                "AND DeviceName LIKE '%" + ServerName + "%' " +
                                "GROUP BY DeviceName,MonthNumber,YearNumber ";
                            //12/24/2013 NS added
                            if (downMin != "")
                            {
                                //11/5/2015 NS modified for VSPLUS-2023
                                str += " HAVING ROUND(SUM(StatValue),0) > " + downMin + " ";
                            }
                            str += "ORDER BY YearNumber DESC, MonthNumber DESC, DeviceName";
                        }
                    }
                }
                else
                {
                    if (ServerName == "" || ServerName == "''")
                    {
                        str = "SELECT DeviceName,ROUND(SUM(StatValue),0) DownMinutes, " +
                            "CASE WHEN MONTH(CAST('" + date + "' as datetime))=MONTH(GETDATE()) AND YEAR(CAST('" + date + "' as datetime))=YEAR(GETDATE()) THEN " +
                            "ROUND((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60+DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0), GETDATE())-SUM(StatValue)),0) " +
                            "ELSE ROUND((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60-SUM(StatValue)),0) END UpMinutes,MonthNumber,YearNumber, " +
                            "CASE WHEN MONTH(CAST('" + date + "' as datetime))=MONTH(GETDATE()) AND YEAR(CAST('" + date + "' as datetime))=YEAR(GETDATE()) THEN " +
                            "(1-ROUND(SUM(StatValue)/((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60+DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0), GETDATE()))),4))*100 " +
                            "ELSE (1-ROUND(SUM(StatValue)/((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60)),4))*100 END UpPct " +
                            "FROM dbo.DeviceUpTimeStats " +
                            "WHERE StatName='HourlyDownTimeMinutes' AND MonthNumber=" + month.ToString() +
                            " AND YearNumber=" + year.ToString() + " " + " AND DeviceType IN (" + ServerType + ") " +
                            "GROUP BY DeviceName,DeviceType,MonthNumber,YearNumber ";
                        //12/24/2013 NS added
                        if (downMin != "")
                        {
                            //11/5/2015 NS modified for VSPLUS-2023
                            str += " HAVING ROUND(SUM(StatValue),0) > " + downMin + " ";
                        }
                        str += "ORDER BY YearNumber DESC, MonthNumber DESC, DeviceName";
                    }
                    else
                    {
                        if (exactmatch)
                        {
                            str = "SELECT DeviceName,ROUND(SUM(StatValue),0) DownMinutes, " +
                                "CASE WHEN MONTH(CAST('" + date + "' as datetime))=MONTH(GETDATE()) AND YEAR(CAST('" + date + "' as datetime))=YEAR(GETDATE()) THEN " +
                                "ROUND((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60+DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0), GETDATE())-SUM(StatValue)),0) " +
                                "ELSE ROUND((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60-SUM(StatValue)),0) END UpMinutes,MonthNumber,YearNumber, " +
                                "CASE WHEN MONTH(CAST('" + date + "' as datetime))=MONTH(GETDATE()) AND YEAR(CAST('" + date + "' as datetime))=YEAR(GETDATE()) THEN " +
                                "(1-ROUND(SUM(StatValue)/((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60+DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0), GETDATE()))),4))*100 " +
                                "ELSE (1-ROUND(SUM(StatValue)/((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60)),4))*100 END UpPct " +
                                "FROM dbo.DeviceUpTimeStats " +
                                "WHERE StatName='HourlyDownTimeMinutes' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                                "AND DeviceName IN(" + ServerName + ") " + " AND DeviceType IN(" + ServerType + ") " +
                                "GROUP BY DeviceName,DeviceType,MonthNumber,YearNumber ";
                            //12/24/2013 NS added
                            if (downMin != "")
                            {
                                //11/5/2015 NS modified for VSPLUS-2023
                                str += " HAVING ROUND(SUM(StatValue),0) > " + downMin + " ";
                            }
                            str += "ORDER BY YearNumber DESC, MonthNumber DESC, DeviceName";
                        }
                        else
                        {
                            str = "SELECT DeviceName,ROUND(SUM(StatValue),0) DownMinutes, " +
                                "CASE WHEN MONTH(CAST('" + date + "' as datetime))=MONTH(GETDATE()) AND YEAR(CAST('" + date + "' as datetime))=YEAR(GETDATE()) THEN " +
                                "ROUND((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60+DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0), GETDATE())-SUM(StatValue)),0) " +
                                "ELSE ROUND((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60-SUM(StatValue)),0) END UpMinutes,MonthNumber,YearNumber, " +
                                "CASE WHEN MONTH(CAST('" + date + "' as datetime))=MONTH(GETDATE()) AND YEAR(CAST('" + date + "' as datetime))=YEAR(GETDATE()) THEN " +
                                "(1-ROUND(SUM(StatValue)/((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60+DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0), GETDATE()))),4))*100 " +
                                "ELSE (1-ROUND(SUM(StatValue)/((dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime))*24*60)),4))*100 END UpPct " +
                                "FROM dbo.DeviceUpTimeStats " +
                                "WHERE StatName='HourlyDownTimeMinutes' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                                "AND DeviceName LIKE '%" + ServerName + "%' " + " AND DeviceType IN(" + ServerType + ") " +
                                "GROUP BY DeviceName,DeviceType,MonthNumber,YearNumber ";
                            //12/24/2013 NS added
                            if (downMin != "")
                            {
                                //11/5/2015 NS modified for VSPLUS-2023
                                str += " HAVING ROUND(SUM(StatValue),0) > " + downMin + " ";
                            }
                            str += "ORDER BY YearNumber DESC, MonthNumber DESC, DeviceName";
                        }
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

        public DataTable SrvAvailabilityRpt()
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT distinct servername FROM [DominoSummaryStats] where statname='Server.AvailabilityIndex' order by servername";
                dt = objAdaptor1.FetchData(str);


            }
            catch(Exception e)
            {
                throw e;
            }
            return dt;
        }


        public DataTable SrvDiskFreeSpaceTrendRpt(string ServerType)
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                //8/8/2014 NS modified - added Exchange to the mix
                //11/24/2014 NS modified - renamed Exchange table to Microsoft
                //2/4/2015 NS modified for VSPLUS-1370
                if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                {
                    str = "SELECT DISTINCT ServerName + ' - ' + LEFT(StatName,LEN(StatName)-5) AS ServerDiskName " +
                        "FROM DominoSummaryStats WHERE statname LIKE 'Disk%Free' " +
                        "UNION " +
                        "SELECT DISTINCT ServerName + ' - ' + LEFT(StatName,LEN(StatName)-1) AS ServerDiskName " +
                        "FROM MicrosoftSummaryStats WHERE statname LIKE 'Disk.%' " +
                        "ORDER BY ServerDiskName ";
                }
                else
                {
                    if (ServerType.IndexOf("Domino") != -1)
                    {
                        str = "SELECT DISTINCT ServerName + ' - ' + LEFT(StatName,LEN(StatName)-5) AS ServerDiskName " +
                            "FROM DominoSummaryStats WHERE statname LIKE 'Disk%Free' " +
                            "UNION " +
                            "SELECT DISTINCT ServerName + ' - ' + LEFT(StatName,LEN(StatName)-1) AS ServerDiskName " +
                            "FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                            "WHERE statname LIKE 'Disk.%' AND t2.ServerType IN(" + ServerType + ") " +
                            "ORDER BY ServerDiskName ";
                    }
                    else
                    {
                        str = "SELECT DISTINCT ServerName + ' - ' + LEFT(StatName,LEN(StatName)-1) AS ServerDiskName " +
                            "FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                            "WHERE statname LIKE 'Disk.%' AND t2.ServerType IN(" + ServerType + ") " +
                            "ORDER BY ServerDiskName ";
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

        public DataTable SrvDiskFreeSpaceServerTypes()
        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                str = "SELECT DISTINCT t2.ServerType AS ServerType, 1 AS OrderNum " +
                            "FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                            "WHERE statname LIKE 'Disk.%' " +
                            "UNION " +
                            "SELECT 'Domino' AS ServerType, 1 AS OrderNum " + 
                            "UNION " + 
                            "SELECT 'All' AS ServerType, 0 AS OrderNum " +
                            "ORDER BY OrderNum,ServerType ";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable SrvTransPerMinRpt()
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT distinct servername FROM [DominoSummaryStats] where statname='Server.Trans.PerMinute' order by servername";
                dt = objAdaptor1.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }


        public DataTable ServerListLocRpt()
        {
            DataTable dt = new DataTable();
            try
            {
                //2/5/2013 NS modified - select only those locations that have servers assigned to them
                //string str = "SELECT [Location] FROM [Locations] ORDER BY [Location]";
                string str = "SELECT DISTINCT [Location] FROM [Locations] ltbl INNER JOIN [Servers] stbl ON " +
                    "ltbl.ID=stbl.LocationID ORDER BY [Location] ";
                dt = objAdaptor.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //1/06/2016 sowmya added for VSPLUS-2934

        public DataTable GetCommuniyList()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "select distinct [Name] from IbmConnectionsObjects";
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        public DataTable ServerListTypeRpt()
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT [ServerType] FROM [ServerTypes] ORDER BY [ServerType]";
                dt = objAdaptor1.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }


        public DataTable DeviceTypeComboBoxforhistoricalresponse(string DeviceType)
        {
            DataTable dt = new DataTable();
            try
            {
                //12/12/2013 NS modified (column name change)
              //string str="select Distinct DeviceName from DeviceDailyStats where DeviceType='" + DeviceType+"'";
                string str = "select Distinct ServerName DeviceName from DeviceDailyStats where DeviceType='" + DeviceType + "'";
              dt = objAdaptor1.FetchData(str);
  

            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;
        }

        public DataTable DeviceTypeComboBoxforhistoricalresponseelsepart()
        {
            DataTable dt = new DataTable();
            try
            {
                //12/12/2013 NS modified (column name change)
                //string str = "select Distinct DeviceName from DeviceDailyStats";
                string str = "select Distinct ServerName DeviceName from DeviceDailyStats";
                dt = objAdaptor1.FetchData(str);


            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;
        }

        public DataTable FillDeviceTypeComboforhistoricalresponse()
        {
            DataTable dt = new DataTable();
            try
            {
                //2/4/2015 NS modified for VSPLUS-1370
                string str = "select Distinct DeviceType, 1 AS OrderNum from DeviceDailyStats " +
                    "UNION " +
                    "SELECT 'All' AS DeviceType, 0 AS OrderNum " +
                    "ORDER BY OrderNum,DeviceType";
                dt = objAdaptor1.FetchData(str);


            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;

        }


        public DataTable fillBlackBerryProbeStats()
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT DISTINCT [Name] FROM [BlackBerryProbeStats] ORDER BY [Name]";
                dt = objAdaptor1.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;

            }
            return dt;
        }

        public DataTable fillDeviceDailyStats()
        {

            DataTable dt = new DataTable();
            try
            {
                //12/12/2013 NS modified (column name change)
                //string str = "SELECT DISTINCT [DeviceName] FROM [DeviceDailyStats] ORDER BY [DeviceName]";
                string str = "SELECT DISTINCT [ServerName] DeviceName FROM [DeviceDailyStats] ORDER BY [ServerName]";
                dt = objAdaptor1.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;

            }
            return dt;

        }

        public DataTable fillNotesMailStats()
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT DISTINCT [Name] FROM [NotesMailStats] ORDER BY [Name]";
                dt = objAdaptor1.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;

            }
            return dt;


        }


        public DataTable fillServerTypes()
        {
            DataTable dt = new DataTable();
            try
            {
				string str = "SELECT [ServerType] FROM [ServerTypes] order by ServerType";
                dt = objAdaptor.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;

            }
            return dt;
        }


        public DataTable AvgCpuUtil()
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT ServerName, StatValue, Date, ID FROM DominoSummaryStats WHERE (ServerName = @ServerName OR @ServerName = ' ') AND (StatName = 'Platform.System.PctCombinedCpuUtil') AND (Date >= @StartDate) AND (Date <= @EndDate)";

                dt = objAdaptor.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }


        public DataTable fillTravelerInterval()
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT DISTINCT RTRIM(Interval) Interval FROM TravelerStats " +
                    "ORDER BY Interval";
                dt = objAdaptor.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;

            }
            return dt;
        }

        public DataTable fillTravelerServer()
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT DISTINCT TravelerServerName FROM TravelerStats " +
                    "ORDER BY TravelerServerName ";
                dt = objAdaptor.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;

            }
            return dt;
        }

        public DataTable fillMailServer()
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT DISTINCT MailServerName FROM TravelerStats " +
                    "WHERE MailServerName IS NOT NULL AND MailServerName != ''" +
                    "ORDER BY MailServerName ";
                dt = objAdaptor.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;

            }
            return dt;
        }
        public DataTable fillMailbyServerName(string ServerName)
        {
            DataTable dt = new DataTable();
            try
            {
                //4/3/2014 NS modified for VSPLUS-464
                //string str = "SELECT DISTINCT MailServerName FROM TravelerStats " +
                //    "WHERE MailServerName IS NOT NULL AND MailServerName != '' and TravelerServerName = '" + ServerName + "'" +
                //    "ORDER BY MailServerName ";
                //5/7/2014 NS modified - 24 hour selection instead of today's data only
                string str = "SELECT DISTINCT MailServerName FROM TravelerStats " +
                    "WHERE MailServerName IS NOT NULL AND MailServerName != '' and TravelerServerName = '" + ServerName + "' " +
                    "AND DateUpdated >= DATEADD(hh,-24,GETDATE()) " +
                    "ORDER BY MailServerName ";
                dt = objAdaptor.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;

            }
            return dt;
        }

        public DataTable DominoDiskSpaceDAL(string ServerName,bool exactmatch,bool isSummary, string ServerType)
        {
            //8/1/2014 NS modified - added Exchange to the list of Domino servers
            string str = "";
            DataTable dt = new DataTable();
            try
            {
                if (!isSummary)
                {
                    if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                    {
                        if (ServerName == "")
                        {
                            str = "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                            "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                            "ROUND([DiskSize],1) [DiskSize] FROM DominoDiskSpace " +
                            "WHERE [DiskSize] IS NOT NULL " +
                            "UNION " +
                            "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                            "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                            "ROUND([DiskSize],1) [DiskSize] FROM DiskSpace " +
                            "WHERE [DiskSize] IS NOT NULL " +
                            "ORDER BY ServerName, DiskName";
                        }
                        else
                        {
                            if (exactmatch)
                            {
                                str = "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                                "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                                "ROUND([DiskSize],1) [DiskSize] FROM DominoDiskSpace " +
                                "WHERE [ServerName] IN (" + ServerName + ") AND [DiskSize] IS NOT NULL " +
                                "UNION " +
                                "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                                "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                                "ROUND([DiskSize],1) [DiskSize] FROM DiskSpace " +
                                "WHERE [ServerName] IN (" + ServerName + ") AND [DiskSize] IS NOT NULL " +
                                "ORDER BY [ServerName],[DiskName]";
                            }
                            else
                            {
                                str = "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                                "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                                "ROUND([DiskSize],1) [DiskSize] FROM DominoDiskSpace " +
                                "WHERE [ServerName] LIKE '%" + ServerName + "%' AND [DiskSize] IS NOT NULL " +
                                "UNION " +
                                "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                                "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                                "ROUND([DiskSize],1) [DiskSize] FROM DiskSpace " +
                                "WHERE [ServerName] LIKE '%" + ServerName + "%' AND [DiskSize] IS NOT NULL ORDER BY [ServerName],[DiskName] ";
                            }
                        }
                    }
                    else
                    {
                        if (ServerType.IndexOf("Domino") != -1)
                        {
                            if (ServerName == "")
                            {
                                str = "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                                "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                                "ROUND([DiskSize],1) [DiskSize] FROM DominoDiskSpace " +
                                "WHERE [DiskSize] IS NOT NULL " +
                                "UNION " +
                                "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                                "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                                "ROUND([DiskSize],1) [DiskSize] FROM DiskSpace " +
                                "WHERE [DiskSize] IS NOT NULL AND " +
                                "ServerType IN(" + ServerType + ") " + 
                                "ORDER BY ServerName, DiskName";
                            }
                            else
                            {
                                if (exactmatch)
                                {
                                    str = "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                                    "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                                    "ROUND([DiskSize],1) [DiskSize] FROM DominoDiskSpace " +
                                    "WHERE [ServerName] IN (" + ServerName + ") AND [DiskSize] IS NOT NULL " +
                                    "UNION " +
                                    "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                                    "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                                    "ROUND([DiskSize],1) [DiskSize] FROM DiskSpace " +
                                    "WHERE [ServerName] IN (" + ServerName + ") AND [DiskSize] IS NOT NULL AND " +
                                    "ServerType IN(" + ServerType + ") " + 
                                    "ORDER BY [ServerName],[DiskName]";
                                }
                                else
                                {
                                    str = "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                                    "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                                    "ROUND([DiskSize],1) [DiskSize] FROM DominoDiskSpace " +
                                    "WHERE [ServerName] LIKE '%" + ServerName + "%' AND [DiskSize] IS NOT NULL " +
                                    "UNION " +
                                    "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                                    "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                                    "ROUND([DiskSize],1) [DiskSize] FROM DiskSpace " +
                                    "WHERE [ServerName] LIKE '%" + ServerName + "%' AND [DiskSize] IS NOT NULL AND " +
                                    "ServerType IN(" + ServerType + ") " + 
                                    "ORDER BY [ServerName],[DiskName] ";
                                }
                            }
                        }
                        else
                        {
                            if (ServerName == "")
                            {
                                str = "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                                "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                                "ROUND([DiskSize],1) [DiskSize] FROM DiskSpace " +
                                "WHERE [DiskSize] IS NOT NULL AND " +
                                "ServerType IN(" + ServerType + ") " +
                                "ORDER BY ServerName, DiskName";
                            }
                            else
                            {
                                if (exactmatch)
                                {
                                    str = "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                                    "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                                    "ROUND([DiskSize],1) [DiskSize] FROM DiskSpace " +
                                    "WHERE [ServerName] IN (" + ServerName + ") AND [DiskSize] IS NOT NULL AND " +
                                    "ServerType IN(" + ServerType + ") " +
                                    "ORDER BY [ServerName],[DiskName]";
                                }
                                else
                                {
                                    str = "SELECT [ID],[ServerName], SUBSTRING(DiskName,CHARINDEX('.',DiskName)+1,LEN(DiskName)) [DiskName], " +
                                    "ROUND(PercentFree*100,1) [PercentFree], ROUND((1-PercentFree)*100,1) [PercentUsed], ROUND([DiskFree],1) [DiskFree], " +
                                    "ROUND([DiskSize],1) [DiskSize] FROM DiskSpace " +
                                    "WHERE [ServerName] LIKE '%" + ServerName + "%' AND [DiskSize] IS NOT NULL AND " +
                                    "ServerType IN(" + ServerType + ") " +
                                    "ORDER BY [ServerName],[DiskName] ";
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                    {
                        if (ServerName == "")
                        {
                            str = "SELECT ROUND(AVG(PercentFree*100),1) [PercentFree], ROUND(AVG((1-PercentFree)*100),1) [PercentUsed], " +
                                "SUM(ROUND([DiskFree],1)) [DiskFree], SUM(ROUND([DiskSize],1)) [DiskSize] FROM " +
                                "(SELECT PercentFree, DiskFree, DiskSize FROM DominoDiskSpace " +
                                "WHERE [DiskSize] IS NOT NULL " +
                                "UNION " +
                                "SELECT PercentFree, DiskFree, DiskSize FROM DiskSpace " +
                                "WHERE [DiskSize] IS NOT NULL) as RS ";
                        }
                        else
                        {
                            if (exactmatch)
                            {
                                str = "SELECT ROUND(AVG(PercentFree*100),1) [PercentFree], ROUND(AVG((1-PercentFree)*100),1) [PercentUsed], " +
                                    "SUM(ROUND([DiskFree],1)) [DiskFree], SUM(ROUND([DiskSize],1)) [DiskSize] FROM " +
                                    "(SELECT PercentFree, DiskFree, DiskSize FROM DominoDiskSpace " +
                                    "WHERE [ServerName] IN (" + ServerName + ") AND [DiskSize] IS NOT NULL " +
                                    "UNION " +
                                    "SELECT PercentFree, DiskFree, DiskSize FROM DiskSpace " +
                                    "WHERE [ServerName] IN (" + ServerName + ") AND [DiskSize] IS NOT NULL) as RS ";
                            }
                            else
                            {
                                str = "SELECT ROUND(AVG(PercentFree*100),1) [PercentFree], ROUND(AVG((1-PercentFree)*100),1) [PercentUsed], " +
                                    "SUM(ROUND([DiskFree],1)) [DiskFree], SUM(ROUND([DiskSize],1)) [DiskSize] FROM " +
                                    "(SELECT PercentFree, DiskFree, DiskSize FROM DominoDiskSpace " +
                                    "WHERE [ServerName] LIKE '%" + ServerName + "%' AND [DiskSize] IS NOT NULL " +
                                    "UNION " +
                                    "SELECT PercentFree, DiskFree, DiskSize FROM DiskSpace " +
                                    "WHERE [ServerName] LIKE '%" + ServerName + "%' AND [DiskSize] IS NOT NULL) as RS ";
                            }
                        }
                    }
                    else
                    {
                        if (ServerType.IndexOf("Domino") != -1)
                        {
                            if (ServerName == "")
                            {
                                str = "SELECT ROUND(AVG(PercentFree*100),1) [PercentFree], ROUND(AVG((1-PercentFree)*100),1) [PercentUsed], " +
                                    "SUM(ROUND([DiskFree],1)) [DiskFree], SUM(ROUND([DiskSize],1)) [DiskSize] FROM " +
                                    "(SELECT PercentFree, DiskFree, DiskSize FROM DominoDiskSpace " +
                                    "WHERE [DiskSize] IS NOT NULL " +
                                    "UNION " +
                                    "SELECT PercentFree, DiskFree, DiskSize FROM DiskSpace " +
                                    "WHERE [DiskSize] IS NOT NULL AND ServerType IN (" + ServerType + ") ) as RS ";
                            }
                            else
                            {
                                if (exactmatch)
                                {
                                    str = "SELECT ROUND(AVG(PercentFree*100),1) [PercentFree], ROUND(AVG((1-PercentFree)*100),1) [PercentUsed], " +
                                        "SUM(ROUND([DiskFree],1)) [DiskFree], SUM(ROUND([DiskSize],1)) [DiskSize] FROM " +
                                        "(SELECT PercentFree, DiskFree, DiskSize FROM DominoDiskSpace " +
                                        "WHERE [ServerName] IN (" + ServerName + ") AND [DiskSize] IS NOT NULL " +
                                        "UNION " +
                                        "SELECT PercentFree, DiskFree, DiskSize FROM DiskSpace " +
                                        "WHERE [ServerName] IN (" + ServerName + ") AND [DiskSize] IS NOT NULL AND ServerType IN(" + ServerType + ") ) as RS ";
                                }
                                else
                                {
                                    str = "SELECT ROUND(AVG(PercentFree*100),1) [PercentFree], ROUND(AVG((1-PercentFree)*100),1) [PercentUsed], " +
                                        "SUM(ROUND([DiskFree],1)) [DiskFree], SUM(ROUND([DiskSize],1)) [DiskSize] FROM " +
                                        "(SELECT PercentFree, DiskFree, DiskSize FROM DominoDiskSpace " +
                                        "WHERE [ServerName] LIKE '%" + ServerName + "%' AND [DiskSize] IS NOT NULL " +
                                        "UNION " +
                                        "SELECT PercentFree, DiskFree, DiskSize FROM DiskSpace " +
                                        "WHERE [ServerName] LIKE '%" + ServerName + "%' AND [DiskSize] IS NOT NULL AND ServerType IN(" + ServerType + ") ) as RS ";
                                }
                            }
                        }
                        else
                        {
                            if (ServerName == "")
                            {
                                str = "SELECT ROUND(AVG(PercentFree*100),1) [PercentFree], ROUND(AVG((1-PercentFree)*100),1) [PercentUsed], " +
                                    "SUM(ROUND([DiskFree],1)) [DiskFree], SUM(ROUND([DiskSize],1)) [DiskSize] FROM " +
                                    "(SELECT PercentFree, DiskFree, DiskSize FROM DiskSpace " +
                                    "WHERE [DiskSize] IS NOT NULL AND ServerType IN(" + ServerType + ") ) as RS ";
                            }
                            else
                            {
                                if (exactmatch)
                                {
                                    str = "SELECT ROUND(AVG(PercentFree*100),1) [PercentFree], ROUND(AVG((1-PercentFree)*100),1) [PercentUsed], " +
                                        "SUM(ROUND([DiskFree],1)) [DiskFree], SUM(ROUND([DiskSize],1)) [DiskSize] FROM " +
                                        "(SELECT PercentFree, DiskFree, DiskSize FROM DiskSpace " +
                                        "WHERE [ServerName] IN (" + ServerName + ") AND [DiskSize] IS NOT NULL AND ServerType IN(" + ServerType + ") ) as RS ";
                                }
                                else
                                {
                                    str = "SELECT ROUND(AVG(PercentFree*100),1) [PercentFree], ROUND(AVG((1-PercentFree)*100),1) [PercentUsed], " +
                                        "SUM(ROUND([DiskFree],1)) [DiskFree], SUM(ROUND([DiskSize],1)) [DiskSize] FROM " +
                                        "(SELECT PercentFree, DiskFree, DiskSize FROM DiskSpace " +
                                        "WHERE [ServerName] LIKE '%" + ServerName + "%' AND [DiskSize] IS NOT NULL AND ServerType IN(" + ServerType + ") ) as RS ";
                                }
                            }
                        }
                    }
                }
                dt = objAdaptor.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable fillDominoDiskAvgConsumption(string ServerType)
        {
            //8/1/2014 NS modified - added Exchange to the list of Domino servers
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                {
                    str = "SELECT DISTINCT ServerName FROM [DominoDiskSpace] " +
                       "UNION " +
                       "SELECT DISTINCT ServerName FROM [DiskSpace] " +
                       "ORDER BY ServerName";
                }
                else
                {
                    if (ServerType.IndexOf("Domino") != -1)
                    {
                        str = "SELECT DISTINCT ServerName FROM [DominoDiskSpace] " +
                           "UNION " +
                           "SELECT DISTINCT ServerName FROM [DiskSpace] " +
                           "WHERE ServerType IN(" + ServerType + ") " +
                           "ORDER BY ServerName";
                    }
                    else
                    {
                        str = "SELECT DISTINCT ServerName FROM [DiskSpace] " +
                            "WHERE ServerType IN(" + ServerType + ") " +
                           "ORDER BY ServerName";
                    }
                }
                dt = objAdaptor.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;

            }
            return dt;
        }

        //2/5/2015 NS added for VSPLUS-1370
        public DataTable fillDominoDiskAvgConsumptionServerTypes()
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT DISTINCT ServerType, 1 AS OrderNum FROM [DiskSpace] " +
                    "UNION " +
                    "SELECT 'Domino' AS ServerType, 1 AS OrderNum " +
                    "UNION " + 
                    "SELECT 'All' AS ServerType, 0 AS OrderNum " + 
                    "ORDER BY OrderNum,ServerType";
                dt = objAdaptor.FetchData(str);

            }
            catch (Exception e)
            {
                throw e;

            }
            return dt;
        }
        //2/5/2015 NS modified for VSPLUS-1370
        public double GetDominoDiskConsumption(string ServerType, string ServerName, string DiskName, bool exactmatch, bool isSummary)
        {
            double diskc = 0;
            int ismatch = 0;
            if (exactmatch)
            {
                ismatch = 1;
            }
            diskc = objAdaptor1.GetDominoDiskConsumption(ServerType,ServerName, DiskName, ismatch, isSummary);
            return diskc;
        }

        public DataTable GetDominoDiskMonthlyConsumption(string ServerName, string DiskName, string Year, string ServerType)
        {
            //2/6/2015 NS modified for VSPLUS-1370
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                //8/8/2014 NS modified - added Exchange
                if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                {
                    if (ServerName == "" && DiskName == "")
                    {
                        //11/24/2014 NS modified the name of the Exchange table to Microsoft
                        query = "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date), DATENAME(MONTH, Date) MonthName, " +
                            "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) DiskName, " +
                            "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM DominoSummaryStats " +
                            "WHERE StatName LIKE 'Disk%Free' " +
                            "AND YEAR(Date) = " + Year + " " +
                            "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) " +
                            "UNION " +
                            "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date), DATENAME(MONTH, Date) MonthName, " +
                            "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, " +
                            "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM MicrosoftSummaryStats " +
                            "WHERE StatName LIKE 'Disk.%' " +
                            "AND YEAR(Date) = " + Year + " " +
                            "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) " +
                            "ORDER BY YEAR(Date),MONTH(Date) ";
                    }
                    else
                    {
                        if (ServerName == "")
                        {
                            query = "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                            "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) DiskName, " +
                            "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM DominoSummaryStats " +
                            "WHERE StatName LIKE 'Disk%Free' AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) IN(" + DiskName + ") " +
                            "AND YEAR(Date) = " + Year + " " +
                            "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) " +
                            "UNION " +
                            "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                            "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, " +
                            "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM MicrosoftSummaryStats " +
                            "WHERE StatName LIKE 'Disk.%' AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) IN(" + DiskName + ") " +
                            "AND YEAR(Date) = " + Year + " " +
                            "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) " +
                            "ORDER BY YEAR(Date),MONTH(Date) ";
                        }
                        if (DiskName == "")
                        {
                            query = "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                            "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) DiskName, " +
                            "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM DominoSummaryStats " +
                            "WHERE StatName LIKE 'Disk%Free' AND ServerName IN(" + ServerName + ") " +
                            "AND YEAR(Date) = " + Year + " " +
                            "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) " +
                            "UNION " +
                            "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                            "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, " +
                            "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM MicrosoftSummaryStats " +
                            "WHERE StatName LIKE 'Disk.%' AND ServerName IN(" + ServerName + ") " +
                            "AND YEAR(Date) = " + Year + " " +
                            "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) " +
                            "ORDER BY YEAR(Date),MONTH(Date) ";
                        }
                        if (ServerName != "" && DiskName != "")
                        {
                            query = "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                            "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) DiskName, " +
                            "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM DominoSummaryStats " +
                            "WHERE StatName LIKE 'Disk%Free' AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) IN(" + DiskName + ") " +
                            "AND ServerName IN (" + ServerName + ") AND YEAR(Date) = " + Year + " " +
                            "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) " +
                            "UNION " +
                            "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                            "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, " +
                            "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM MicrosoftSummaryStats " +
                            "WHERE StatName LIKE 'Disk.%' AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) IN(" + DiskName + ") " +
                            "AND ServerName IN (" + ServerName + ") AND YEAR(Date) = " + Year + " " +
                            "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) " +
                            "ORDER BY YEAR(Date),MONTH(Date) ";
                        }
                    }
                }
                else
                {
                    if (ServerType.IndexOf("Domino") != -1)
                    {
                        if (ServerName == "" && DiskName == "")
                        {
                            //11/24/2014 NS modified the name of the Exchange table to Microsoft
                            query = "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date), DATENAME(MONTH, Date) MonthName, " +
                                "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) DiskName, " +
                                "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM DominoSummaryStats " +
                                "WHERE StatName LIKE 'Disk%Free' " +
                                "AND YEAR(Date) = " + Year + " " +
                                "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) " +
                                "UNION " +
                                "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date), DATENAME(MONTH, Date) MonthName, " +
                                "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, " +
                                "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM MicrosoftSummaryStats t1 " +
                                "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                "WHERE StatName LIKE 'Disk.%' " +
                                "AND YEAR(Date) = " + Year + " " +
                                "AND t2.ServerType IN(" + ServerType + ") " +
                                "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) " +
                                "ORDER BY YEAR(Date),MONTH(Date) ";
                        }
                        else
                        {
                            if (ServerName == "")
                            {
                                query = "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                                "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) DiskName, " +
                                "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM DominoSummaryStats " +
                                "WHERE StatName LIKE 'Disk%Free' AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) IN(" + DiskName + ") " +
                                "AND YEAR(Date) = " + Year + " " +
                                "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) " +
                                "UNION " +
                                "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                                "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, " +
                                "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM MicrosoftSummaryStats t1 " +
                                "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                "WHERE StatName LIKE 'Disk.%' AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) IN(" + DiskName + ") " +
                                "AND YEAR(Date) = " + Year + " " +
                                "AND t2.ServerType IN(" + ServerType + ") " +
                                "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) " +
                                "ORDER BY YEAR(Date),MONTH(Date) ";
                            }
                            if (DiskName == "")
                            {
                                query = "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                                "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) DiskName, " +
                                "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM DominoSummaryStats " +
                                "WHERE StatName LIKE 'Disk%Free' AND ServerName IN(" + ServerName + ") " +
                                "AND YEAR(Date) = " + Year + " " +
                                "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) " +
                                "UNION " +
                                "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                                "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, " +
                                "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM MicrosoftSummaryStats t1 " +
                                "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                "WHERE StatName LIKE 'Disk.%' AND ServerName IN(" + ServerName + ") " +
                                "AND YEAR(Date) = " + Year + " " +
                                "AND t2.ServerType IN(" + ServerType + ") " +
                                "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) " +
                                "ORDER BY YEAR(Date),MONTH(Date) ";
                            }
                            if (ServerName != "" && DiskName != "")
                            {
                                query = "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                                "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) DiskName, " +
                                "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM DominoSummaryStats " +
                                "WHERE StatName LIKE 'Disk%Free' AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) IN(" + DiskName + ") " +
                                "AND ServerName IN (" + ServerName + ") AND YEAR(Date) = " + Year + " " +
                                "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) " +
                                "UNION " +
                                "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                                "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, " +
                                "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM MicrosoftSummaryStats t1 " +
                                "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                "WHERE StatName LIKE 'Disk.%' AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) IN(" + DiskName + ") " +
                                "AND ServerName IN (" + ServerName + ") AND YEAR(Date) = " + Year + " " +
                                "AND t2.ServerType IN(" + ServerType + ") " +
                                "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) " +
                                "ORDER BY YEAR(Date),MONTH(Date) ";
                            }
                        }
                    }
                    else
                    {
                        if (ServerName == "" && DiskName == "")
                        {
                            //11/24/2014 NS modified the name of the Exchange table to Microsoft
                            query = "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date), DATENAME(MONTH, Date) MonthName, " +
                                "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, " +
                                "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM MicrosoftSummaryStats t1 " +
                                "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                "WHERE StatName LIKE 'Disk.%' " +
                                "AND YEAR(Date) = " + Year + " " +
                                "AND t2.ServerType IN(" + ServerType + ") " +
                                "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) " +
                                "ORDER BY YEAR(Date),MONTH(Date) ";
                        }
                        else
                        {
                            if (ServerName == "")
                            {
                                query = "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                                "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, " +
                                "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM MicrosoftSummaryStats t1 " +
                                "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                "WHERE StatName LIKE 'Disk.%' AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) IN(" + DiskName + ") " +
                                "AND YEAR(Date) = " + Year + " " +
                                "AND t2.ServerType IN(" + ServerType + ") " +
                                "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) " +
                                "ORDER BY YEAR(Date),MONTH(Date) ";
                            }
                            if (DiskName == "")
                            {
                                query = "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                                "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, " +
                                "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM MicrosoftSummaryStats t1 " +
                                "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                "WHERE StatName LIKE 'Disk.%' AND ServerName IN(" + ServerName + ") " +
                                "AND YEAR(Date) = " + Year + " " +
                                "AND t2.ServerType IN(" + ServerType + ") " +
                                "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) " +
                                "ORDER BY YEAR(Date),MONTH(Date) ";
                            }
                            if (ServerName != "" && DiskName != "")
                            {
                                query = "SELECT ServerName,MONTH(Date) MonthYear, YEAR(Date),DATENAME(MONTH, Date) MonthName, " +
                                "SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, " +
                                "ROUND(AVG(StatValue)/1024/1024/1024,1) StatValue FROM MicrosoftSummaryStats t1 " +
                                "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                "WHERE StatName LIKE 'Disk.%' AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) IN(" + DiskName + ") " +
                                "AND ServerName IN (" + ServerName + ") AND YEAR(Date) = " + Year + " " +
                                "AND t2.ServerType IN(" + ServerType + ") " +
                                "GROUP BY ServerName,YEAR(Date),MONTH(Date),DATENAME(MONTH, Date),SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) " +
                                "ORDER BY YEAR(Date),MONTH(Date) ";
                            }
                        }
                    }
                }
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetTravelerStats(string ServerName, string Interval)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                if (ServerName == "")
                {
                    query = "SELECT ID, TravelerServerName , " +
                    "MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
                    "FROM TravelerStats WHERE Interval = '" + Interval + "' " +
                    "AND DATEDIFF(dd,0,DateUpdated)=DATEDIFF(dd,0,GETDATE()) AND MailServerName IS NOT NULL AND MailServerName != ''";
                }
                else
                {
                    query = "SELECT ID, TravelerServerName, " +
                    "MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
                    "FROM TravelerStats WHERE Interval = '" + Interval + "' AND " +
                    "TravelerServerName = '" + ServerName + "' " +
                    "AND DATEDIFF(dd,0,DateUpdated)=DATEDIFF(dd,0,GETDATE()) AND MailServerName IS NOT NULL AND MailServerName != ''";
                }
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetTravelerStatsSrv(string ServerName, string TravelerName)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                if (ServerName == "")
                {
                    query = "SELECT ID, TravelerServerName, " +
                    " MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
                    "FROM TravelerStats WHERE TravelerServerName = '" + TravelerName + "' " +
                    "AND DATEDIFF(dd,0,DateUpdated)=DATEDIFF(dd,0,GETDATE()) AND MailServerName IS NOT NULL AND MailServerName != ''";
                }
                else
                {
                    query = "SELECT ID, TravelerServerName, " +
                    "MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
                    "FROM TravelerStats WHERE TravelerServerName = '" + TravelerName + "' AND " +
                    "MailServerName = '" + ServerName + "' " +
                    "AND DATEDIFF(dd,0,DateUpdated)=DATEDIFF(dd,0,GETDATE()) AND MailServerName IS NOT NULL AND MailServerName != ''";
                }
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetTravelerStatsDelta(string Interval, string ServerName, string StartDate, string EndDate, bool isSummary)
        {
            DataTable dt = new DataTable();
            string query = "";

            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
            DateTime dtStart = DateTime.Parse(StartDate);
            DateTime dtEnd = DateTime.Parse(EndDate);

            StartDate = dtStart.ToString(ci);
            EndDate = dtEnd.ToString(ci);

            try
            {                //4/28/2014 NS modified for VSPLUS-557
                if (isSummary)
                {
                    if (ServerName == "")
                    {
                        //6/11/2014 Wesley
                        /*query = "SELECT TravelerServerName,MailServerName,StatName,DateUpdated," +
                        "Int" + Interval.Replace("-", "") + " Delta " +
                        "FROM TravelerDailySummaryStats " +
                        "WHERE StatName='OpenTimesDelta' AND DateUpdated BETWEEN '" + StartDate + "' " +
                        "AND '" + EndDate + "' ";
                         */
                        //7/9/2014 NS modified for VSPLUS-748
                        /*query = "SELECT TravelerServerName,MailServerName,StatName,DateUpdated, Int" + Interval.Replace("-", "") + " Delta " +
                                "FROM TravelerDailySummaryStats " +
                                "WHERE StatName='OpenTimesDelta' AND " +
                                "convert(datetime,DateUpdated,101) BETWEEN convert(datetime,'" + StartDate + "',101) AND convert(datetime,'" + EndDate + "',101)";
                        */
                        query = "SELECT TravelerServerName,MailServerName,StatName,DateUpdated, Int" + Interval.Replace("-", "") + " Delta " +
                                "FROM TravelerDailySummaryStats " +
                                "WHERE StatName='OpenTimesDelta' AND dateadd(dd,0,datediff(dd,0,convert(datetime,DateUpdated,101))) " +
                                "BETWEEN convert(datetime,'" + StartDate + "',101) AND convert(datetime,'" + EndDate + "',101)";
                    }
                    else
                    {
                       //6/11/2014 Wesley
                        /*
                        query = "SELECT TravelerServerName,MailServerName,StatName,DateUpdated," +
                        "Int" + Interval.Replace("-", "") + " Delta " +
                        "FROM TravelerDailySummaryStats " +
                        "WHERE StatName='OpenTimesDelta' AND DateUpdated BETWEEN '" + StartDate + "' " +
                        "AND '" + EndDate + "' AND " +
                        "TravelerServerName = '" + ServerName + "' ";
                         * */
                        //7/9/2014 NS modified for VSPLUS-748
                        /*
                        query = "SELECT TravelerServerName,MailServerName,StatName,DateUpdated, Int" + Interval.Replace("-", "") + " Delta " +
                                "FROM TravelerDailySummaryStats " +
                                "WHERE StatName='OpenTimesDelta' AND " +
                                "convert(datetime,DateUpdated,101) BETWEEN convert(datetime,'" + StartDate + "',101) AND convert(datetime,'" + EndDate + "',101) AND " +
                                "TravelerServerName = '" + ServerName + "' ";
                        */
                        query = "SELECT TravelerServerName,MailServerName,StatName,DateUpdated, Int" + Interval.Replace("-", "") + " Delta " +
                                "FROM TravelerDailySummaryStats " +
                                "WHERE StatName='OpenTimesDelta' AND " +
                                "dateadd(dd,0,datediff(dd,0,convert(datetime,DateUpdated,101))) " + 
                                "BETWEEN convert(datetime,'" + StartDate + "',101) AND convert(datetime,'" + EndDate + "',101) AND " +
                                "TravelerServerName = '" + ServerName + "' ";
                    }
                    dt = objAdaptor1.FetchData(query);
                }
                else
                {
                    if (ServerName == "")
                    {
                        /*
                        query = "SELECT ID,  TravelerServerName, " +
                        "MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
                        "FROM TravelerStats WHERE Interval = '" + Interval + "' " +
                        "AND DATEDIFF(dd,0,DateUpdated)=DATEDIFF(dd,0,GETDATE()) AND MailServerName IS NOT NULL AND MailServerName != ''";
                         */
                        
                        //6/11/2014 Wesley
                        /*
                        query = "SELECT ID,  TravelerServerName, " +
                        "MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
                        "FROM TravelerStats WHERE Interval = '" + Interval + "' " +
                        "AND DateUpdated BETWEEN '" + StartDate + "' AND '" + EndDate + "' AND " +
                        "MailServerName IS NOT NULL AND MailServerName != ''";
                         */
                        //7/9/2014 NS modified for VSPLUS-748
                        /*query = "SELECT ID, TravelerServerName, MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
                                "FROM TravelerStats  " +
                                "WHERE Interval = '" + Interval + "' AND " +
                                "convert(datetime,DateUpdated,101) BETWEEN convert(datetime,'" + StartDate + "',101) AND convert(datetime,'" + EndDate + "',101) AND  " +
                                "MailServerName IS NOT NULL  " +
                                "AND MailServerName != '' ";
                         */
                        query = "SELECT ID, TravelerServerName, MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
                                "FROM TravelerStats  " +
                                "WHERE Interval = '" + Interval + "' AND " +
                                "dateadd(dd,0,datediff(dd,0,convert(datetime,DateUpdated,101))) " + 
                                "BETWEEN convert(datetime,'" + StartDate + "',101) AND convert(datetime,'" + EndDate + "',101) AND  " +
                                "MailServerName IS NOT NULL  " +
                                "AND MailServerName != '' ";
                    }
                    else
                    {
                        /*
                        query = "SELECT ID, TravelerServerName, " +
                        "MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
                        "FROM TravelerStats WHERE Interval = '" + Interval + "' AND " +
                        "TravelerServerName = '" + ServerName + "' " +
                        "AND DATEDIFF(dd,0,DateUpdated)=DATEDIFF(dd,0,GETDATE()) AND MailServerName IS NOT NULL AND MailServerName != ''";
                         */

                        //6/11/2014 Wesley
                        /*
                        query = "SELECT ID, TravelerServerName, " +
                        "MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
                        "FROM TravelerStats WHERE Interval = '" + Interval + "' AND " +
                        "TravelerServerName = '" + ServerName + "' " +
                        "AND DateUpdated BETWEEN '" + StartDate + "' AND '" + EndDate + "' AND " +
                        "MailServerName IS NOT NULL AND MailServerName != ''";
                        */
                        //7/9/2014 NS modified for VSPLUS-748
                        /*
                        query = "SELECT ID, TravelerServerName, MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
                                "FROM TravelerStats  " +
                                "WHERE Interval = '" + Interval + "' AND TravelerServerName = '" + ServerName + "' AND  " +
                                "convert(datetime,DateUpdated,101) BETWEEN convert(datetime,'" + StartDate + "',101) AND convert(datetime,'" + EndDate + "',101) AND  " +
                                "MailServerName IS NOT NULL  " +
                                "AND MailServerName != '' ";
                         */
                        query = "SELECT ID, TravelerServerName, MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
                                "FROM TravelerStats  " +
                                "WHERE Interval = '" + Interval + "' AND TravelerServerName = '" + ServerName + "' AND  " +
                                "dateadd(dd,0,datediff(dd,0,convert(datetime,DateUpdated,101))) " + 
                                "BETWEEN convert(datetime,'" + StartDate + "',101) AND convert(datetime,'" + EndDate + "',101) AND  " +
                                "MailServerName IS NOT NULL  " +
                                "AND MailServerName != '' ";
                    }
                    dt = objAdaptor.FetchData(query);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //4/22/2014 NS added
        public DataTable GetTravelerHTTPSessions(string TravelerName)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                query = "SELECT Date, dd.[StatValue] StatValue, ts.[HTTP_MaxConfiguredConnections] " +
                    "FROM [VSS_Statistics].[dbo].[DominoDailyStats] dd JOIN [VitalSigns].[dbo].[Status] s ON " +
                    "dd.[ServerName] = s.[Name] JOIN [VitalSigns].[dbo].[Traveler_Status] ts ON " +
                    "s.[Name] = ts.[ServerName] WHERE s.[Name] = '" + TravelerName + "' " +
                    "and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE()) " +
                    "and s.[SecondaryRole] = 'Traveler' and dd.[StatName] = 'Http.CurrentConnections' order by Date asc";
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetUserNameList()
        {
            string query = "";
            DataTable dt = new DataTable();
            try
            {
                //11/4/2015 NS modified for VSPLUS-2023
                query = "SELECT DISTINCT FullName FROM ConfigUsersList ORDER BY FullName";
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetUserAccessList(string UserName)
        {
            string query = "";
            DataTable dt = new DataTable();
            try
            {
                //8/16/2013 NS modified
                //11/4/2015 NS modified for VSPLUS-2023
                if (UserName == "")
                {
                    query = "SELECT * FROM ConfigUsersList ORDER BY FullName,Name";
                }
                else
                {
                    query = "SELECT * FROM ConfigUsersList WHERE FullName='" + UserName + "' ORDER BY FullName,Name";
                }
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetLogFileData()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT Keyword, RepeatOnce, ID FROM LogFile";
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetMailFileStats(string fileSize)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT FileName, Title, FileSize, Server, DesignTemplateName, DocumentCount, PercentUsed, FileNamePath, ID " +
                    "FROM Daily WHERE IsMailFile=1 ";
                if (fileSize != "")
                {
                    query += " AND (FileSize > " + fileSize + ") ";
                }
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetMailThreshold()
        {
            DataTable dt = new DataTable();
            try
            {
                //11/25/2014 NS modified - some of the column names were outdated
				//2/19/2016 Sowjanya modified the query for the ticket VSPLUS-2620
				string query = "select  Servers.ServerName, DominoServers.PendingThreshold,DominoServers.DeadThreshold,"+
				"DominoServers.HeldThreshold as HeldMailThreshold from DominoServers inner join servers on DominoServers.ServerID= servers.ID order by Servers.ServerName";

                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetMaintWin()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT sm.ID, m.Name, s.ServerName, CAST(m.StartDate as Date) StartDate, CAST(m.StartTime as Time) StartTime, m.Duration, CAST(m.EndDate as Date) EndDate, " +
                         "CASE WHEN MaintType = 1 THEN 'One time' WHEN MaintType = 2 THEN 'Daily' WHEN MaintType = 3 THEN 'Weekly' WHEN MaintType = 4 THEN 'Monthly' END AS MaintType, " +
                          "dbo.DecodeMaintSchedule(m.MaintType, m.MaintDaysList) AS MaintDaysList " + 
                          "FROM Maintenance AS m INNER JOIN ServerMaintenance AS sm ON m.ID = sm.MaintID INNER JOIN " +
                          "Servers AS s ON sm.ServerID = s.ID";
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetNotesDBs()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT Name, ScanInterval, OffHoursScanInterval, Enabled, ResponseThreshold, RetryInterval, ServerName, FileName, ID, Category " +
                    "FROM NotesDatabases";
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetServerList(string param, string qtype)
        {
            DataTable dt = new DataTable();
            try
            {
                //7/14/2014 NS modified for VSPLUS-428
                /*
                string query = "SELECT t1.[ID],[ServerName], [ServerType], [Description], [Location], [IPAddress] " +
                    "FROM [Servers] t1 INNER JOIN [Locations] t2 ON [LocationID] = t2.[ID] INNER JOIN [ServerTypes] t3 ON " +
                    "[ServerTypeID] = t3.[ID] ";
                 */
                //9/29/2015 NS modified for VSPLUS-2089
                /*
                string query = "SELECT t1.[ID],[ServerName], [ServerType], t1.[Description], t2.[Location], [IPAddress], " +
                    "s1.OperatingSystem,s1.DominoVersion Release FROM [Servers] t1 INNER JOIN [Status] s1 ON s1.[Name]=t1.[ServerName] " +
                    "INNER JOIN [Locations] t2 ON [LocationID] = t2.[ID] INNER JOIN [ServerTypes] t3 ON " +
                    "[ServerTypeID] = t3.[ID]";
                 */

				//1/22/2016 Sowjanya, changed the query for VSPLUS-2536
                string query = "SELECT t1.[ID],[ServerName], [ServerType], t1.[Description], t2.[Location], [IPAddress], " +
					" case when  s1.OperatingSystem is null or LEN(RTRIM(LTRIM( s1.OperatingSystem))) = 0   then 'Unknown' else s1.OperatingSystem  end as OperatingSystem," +
					"case when s1.DominoVersion is null or LEN(RTRIM(LTRIM(s1.DominoVersion))) = 0  then 'Unknown' else s1.DominoVersion end as Release FROM [Servers] t1 " +
                    "INNER JOIN [Locations] t2 ON [LocationID] = t2.[ID] INNER JOIN [ServerTypes] t3 ON " +
                    "[ServerTypeID] = t3.[ID] INNER JOIN [Status] s1 ON s1.[Name]=t1.[ServerName] " +
                    "AND s1.Type=t3.ServerType ";
                if (param != "")
                {
                    if (qtype == "ServerType")
                    {
                        query += " WHERE t3.[" + qtype + "] = '" + param + "'";
                    }
                    else
                    {
                        query += " WHERE t2.[" + qtype + "] = '" + param + "'";
                    }
                }
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
            
        }

        //1/06/2016 sowmya added for VSPLUS-2934
        public DataTable GetCommList(string param)
        {
            DataTable dt = new DataTable();
            string query = string.Empty;
            try
            {
                if (param != "")
                {
                    query = "Select C.Name,  U.DisplayName as Users, CASE WHEN U.ID = C.OwnerId THEN 'Owner' ELSE '' END AS Owners " +
                    "from dbo.IbmConnectionsObjects C Inner Join Servers S on S.Id=C.ServerId " +
                    "Inner Join ServerTypes ST on ST.Id=S.ServerTypeId AND ST.ServerType='IBM Connections' " +
                    "Inner Join IbmConnectionsObjectUsers CU on C.ID=CU.ObjectId " +
                    "Inner Join IbmConnectionsUsers U on CU.UserId=U.ID where C.Name = '" + param + "' Group By C.Name,C.OwnerId,U.DisplayName,U.ID order by Name,Owners DESC,DisplayName ";
                }
                else
                {
                    query = "Select  C.Name, U.DisplayName as Users, CASE WHEN U.ID = C.OwnerId THEN 'Owner' ELSE '' END AS Owners " +
                          "from dbo.IbmConnectionsObjects C Inner Join Servers S on S.Id=C.ServerId " +
                              " Inner Join ServerTypes ST on ST.Id=S.ServerTypeId AND ST.ServerType='IBM Connections' " +
                                "Inner Join IbmConnectionsObjectUsers CU on C.ID=CU.ObjectId " +
                                 "Inner Join IbmConnectionsUsers U on CU.UserId=U.ID Group By C.Name,C.OwnerId,U.DisplayName,U.ID order by Name,Owners DESC,DisplayName ";

                }
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;

        }

        public DataTable GetServerTasks()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT t2.TaskName, t3.ServerName, t1.Enabled, t1.RestartOffHours, t2.RetryCount, t2.MaxBusyTime, t4.ServerType " +
                    "FROM ServerTaskSettings AS t1 INNER JOIN DominoServerTasks AS t2 ON t1.TaskID = t2.TaskID INNER JOIN " +
                    "Servers AS t3 ON t1.ServerID = t3.ID INNER JOIN ServerTypes AS t4 ON t3.ServerTypeID = t4.ID";
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetDominoDBStats(string servername,string foldername)
        {
            DataTable dt = new DataTable();
            try
            {
                //3/8/2016 NS modified for VSPLUS-2644
                string query = "SELECT ID, Title, ScanDate, CASE WHEN Folder = '' THEN '...' ELSE SUBSTRING(Folder,1,LEN(Folder)-1) END AS Folder, Server, " +
                    "DesignTemplateName, Status, DocumentCount, PercentUsed, IsMailFile, FileNamePath " +
                    "FROM Daily WHERE (DATEADD(dd, 0, DATEDIFF(dd, 0, ScanDate)) IN " +
                         "(SELECT MAX(DATEADD(dd, 0, DATEDIFF(dd, 0, ScanDate))) AS Expr1 " +
                         "FROM Daily AS Daily_1))";
                if (servername != "")
                {
                    query += " AND Server='" + servername + "' ";
                }
                if (foldername != "")
                {
                    if (foldername == "...")
                    {
                        foldername = "";
                    }
                    query += " AND Folder='" + foldername + "\\' ";
                }
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetDominoDBStatsSrv()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT DISTINCT Server FROM Daily WHERE (DATEADD(dd, 0, DATEDIFF(dd, 0, ScanDate)) IN " +
                         "(SELECT MAX(DATEADD(dd, 0, DATEDIFF(dd, 0, ScanDate))) AS Expr1 " +
                         "FROM Daily AS Daily_1)) ";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetDominoDBStatsFolder(string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                //3/8/2016 NS modified for VSPLUS-2644
                string query = "SELECT DISTINCT CASE WHEN Folder = '' THEN '...' ELSE SUBSTRING(Folder,1,LEN(Folder)-1) END AS Folder " +
                    "FROM Daily WHERE (DATEADD(dd, 0, DATEDIFF(dd, 0, ScanDate)) IN " +
                         "(SELECT MAX(DATEADD(dd, 0, DATEDIFF(dd, 0, ScanDate))) AS Expr1 " +
                         "FROM Daily AS Daily_1)) ";
                if (servername != "")
                {
                    query += "AND Server='" + servername + "' ";
                }
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        public DataTable GetDominoServerHealth()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT Type, Location, Category, Name, Status, Details, LastUpdate, Description, PendingMail, DeadMail, MailDetails, Upcount, DownCount, UpPercent, ResponseTime, " +
                         "ResponseThreshold, PendingThreshold, DeadThreshold, UserCount, MyPercent, NextScan, DominoServerTasks, TypeANDName, Icon, OperatingSystem, " +
                         "DominoVersion, UpMinutes, DownMinutes, UpPercentMinutes, PercentageChange, CPU, HeldMail, HeldMailThreshold, Severity, Memory, StatusCode, " +
                         "SecondaryRole, CPUThreshold, CASE WHEN StatusCode = 'Not Responding' THEN 0 WHEN StatusCode = 'OK' THEN 2 ELSE 1 END AS SortCol " +
                         "FROM Status WHERE (Type = 'Domino')";
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetOverallServerHealth()
        {
            DataTable dt = new DataTable();
            try
            {
				//2/12/2016 Sowjanya modified the query for VSPLUS-2521 ticket
				string query = "Select Name, Status,CAST((CPU*100) AS VARCHAR(50))  + '/' + CAST((CPUThreshold*100) AS VARCHAR(50)) CPU, "+
					"TypeandName,Details,Description,Type,SecondaryRole,Location,case when Type in ('Cloud','Network Device','NotesMail Probe','URL') and PendingMail = 0 then null else PendingMail  end as PendingMail," +
				    "PendingThreshold,(case when Type in('Cloud','Network Device','NotesMail Probe','URL') and DeadMail = 0 then null else  DeadMail end )as DeadMail ,"+
				    "DeadThreshold,case when Type in ('Cloud','Network Device','NotesMail Probe','URL') and HeldMail= 0 then null else HeldMail end as HeldMail,HeldMailThreshold,StatusCode," +
				    "CASE WHEN StatusCode = 'Not Responding' THEN 0 WHEN StatusCode = 'OK' " +
				    "THEN 2 ELSE 1 END SortCol, CAST(PendingMail AS VARCHAR(50)) + ' ' + CAST(DeadMail AS VARCHAR(50)) + ' ' + " +
				   "CAST(HeldMail AS VARCHAR(50)) AllMail FROM [Status] " + 
				   "WHERE (Location!='')  AND StatusCode IS NOT NULL ORDER BY Name";

                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
            
        }

        public DataTable GetResponseTimes(string typeval)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                //9/26/2013 NS modified the query to allow for a single type selection
                //10/3/2013 NS modified the query below to sort by sever name in ascending order
                //string query = "SELECT TypeANDName, LEFT(Name,20)+'/'+Type as Server,ResponseTime FROM Status WHERE ResponseTime>0";
                //7/29/2015 NS modified for VSPLUS-2026
                if (typeval == "" || typeval == "All")
                {
                    query = "SELECT TypeANDName, LEFT(Name,25) as Server,ResponseTime FROM Status WHERE ResponseTime>0 ORDER BY Name";
                }
                else
                {
                    query = "SELECT TypeANDName, LEFT(Name,25) as Server,ResponseTime FROM Status WHERE ResponseTime>0 AND " +
                        "Type='" + typeval + "' ORDER BY Name";
                }
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetMAXCpuUtil(string servername,DateTime starttime,DateTime endtime,string threshold)
        {
            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT ServerName, ROUND(StatValue,2) StatValue, Date, ID " +
                    "FROM DominoSummaryStats WHERE (StatName = 'Platform.System.PctCombinedCpuUtil.Max') AND " +
                    "(Date >='" + starttime + "') AND (Date <= '" + endtime + "') ";
                if (servername != "")
                {
                    //8/5/2013 NS modified
                    //str += "AND ServerName='" + servername + "' ";
                    str += "AND ServerName IN(" + servername + ") ";
                }
                //27/05/2016 sowmya added for vaplus-2971
                if (threshold != "")
                {

                    str += "AND StatValue >= " + threshold;
                }
                str += "ORDER BY ServerName";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable GetConsoleCmdList()
        {
            DataTable dt = new DataTable();
            try
            {
                //11/20/2014 NS modified for VSPLUS-1188
                //12/11/2014 NS modified for VSPLUS-1213
                string query = "SELECT [ID],[ServerName],[Command],[Submitter],[DateTimeSubmitted],[DateTimeProcessed],[Result],ISNULL([Comments],'') Comments " +
                    "FROM [DominoConsoleCommands]";
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;

        }

        public DataTable fillDownMinutesServer(string sType)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT DISTINCT DeviceName ServerName " +
                    "FROM DeviceUpTimeStats " +
                    "WHERE StatName='HourlyDownTimeMinutes' ";
                if (sType != "" && sType != "''" && sType != "'All'")
                {
                    query += "AND DeviceType IN(" + sType + ") ";
                }
                query +="ORDER BY DeviceName";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //8/21/2014 NS added for VSPLUS-886
        //9/29/2014 NS modified for VSPLUS-953
        public DataTable GetTravelerDeviceSyncs(string ServerName, string StartDate, string EndDate)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                if (ServerName == "")
                {
                    query = "SELECT ServerName, dateadd(dd,0,datediff(dd,0,Date)) AS Date, StatValue " +
                        "FROM [VSS_Statistics].[dbo].[DominoSummaryStats] " +
                        "WHERE StatName='TotalDeviceSyncs' " +
                        "and dateadd(dd,0,datediff(dd,0,convert(datetime,Date,101))) " +
                        "BETWEEN convert(datetime,'" + StartDate + "',101) AND convert(datetime,'" + EndDate + "',101) " +
                        "order by ServerName, Date asc";
                }
                else
                {
                    query = "SELECT ServerName, dateadd(dd,0,datediff(dd,0,Date)) AS Date, StatValue " +
                        "FROM [VSS_Statistics].[dbo].[DominoSummaryStats] " +
                        "WHERE StatName='TotalDeviceSyncs' " +
                        "AND ServerName IN(" + ServerName + ") " +
                        "and dateadd(dd,0,datediff(dd,0,convert(datetime,Date,101))) " +
                        "BETWEEN convert(datetime,'" + StartDate + "',101) AND convert(datetime,'" + EndDate + "',101) " +
                        "order by ServerName, Date asc";
                }
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //9/30/2014 NS added for VSPLUS-953
        public DataTable GetTravelerDevices()
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                query = "SELECT DISTINCT ServerName " +
                    "FROM [VSS_Statistics].[dbo].[DominoSummaryStats] " +
                    "WHERE StatName='TotalDeviceSyncs' " +
                    "order by ServerName";
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //9/10/2014 NS added for VSPLUS-921
        public DataTable GetDBClusterInfo(string ClusterName)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                //9/11/2015 NS modified for VSPLUS-2126
                //9/30/2015 NS modified for VSPLUS-2150
                query = "SELECT ct.ID ID,[DatabaseName],[DocCountA],[DocCountB],[DocCountC],[DBSizeA]," +
                  "[DBSizeB],[DBSizeC],[DatabaseTitle],st1.[ServerName] ServerNameA," +
                  "ISNULL(st2.[ServerName],'') ServerNameB,ISNULL(st3.[ServerName],'') ServerNameC,'' as color, ReplicaID " +
                  " FROM ClusterDatabaseDetails ct INNER JOIN DominoCluster dt on ct.ClusterID=dt.ID " +
                  "INNER JOIN Servers st1 ON dt.ServerID_A=st1.ID LEFT OUTER JOIN Servers st2 ON dt.ServerID_B=st2.ID " +
                  "LEFT OUTER JOIN Servers st3 ON dt.ServerID_C=st3.ID " +
                  "WHERE Name='" + ClusterName + "' " +
                  " ORDER BY [DatabaseName] ";
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //9/11/2014 NS added for VSPLUS-921
        public DataTable GetDBClusterServers(string ClusterName)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                //9/30/2015 NS modified for VSPLUS-2150
                query = "SELECT DISTINCT st1.[ServerName] ServerNameA,ISNULL(st2.[ServerName],'') ServerNameB, " +
                    "ISNULL(st3.[ServerName],'') ServerNameC,First_Alert_Threshold Threshold, ReplicaID " +
                    "FROM ClusterDatabaseDetails ct INNER JOIN DominoCluster dt on ct.ClusterID=dt.ID " +
                    "INNER JOIN Servers st1 ON dt.ServerID_A=st1.ID LEFT OUTER JOIN Servers st2 ON dt.ServerID_B=st2.ID " +
                    "LEFT OUTER JOIN Servers st3 ON dt.ServerID_C=st3.ID " + 
                    "WHERE Name='" + ClusterName + "' ";
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //9/11/2014 NS added for VSPLUS-921
        public DataTable GetDBClusterNames()
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                query = "SELECT DISTINCT [ID],[Name] ClusterName " +
                    "FROM DominoCluster " +
                    "ORDER BY Name ";
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //9/17/2014 NS added for VSPLUS-456
        public DataTable GetDeviceUptimePctServerNames(string ServerType)
        {

            DataTable dt = new DataTable();
            string str = "";
            try
            {
                if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                {
                    //8/25/2015 NS modified for VSPLUS-1619
                    //10/23/2015 NS modified for VSPLUS-2089
                    str = "SELECT DISTINCT ServerName FROM DeviceUpTimeSummaryStats t1 " +
                        "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                        "DeviceType=ServerType " +
                        "INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=t3.Name " +
                        "AND t2.ID=t3.DeviceTypeID ORDER BY ServerName";
                }
                else
                {
                    //8/25/2015 NS modified for VSPLUS-1619
                    //10/23/2015 NS modified for VSPLUS-2089
                    str = "SELECT DISTINCT ServerName FROM DeviceUpTimeSummaryStats t1 " +
                        "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                        "DeviceType=ServerType " +
                        "INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=t3.Name " +
                        "AND t2.ID=t3.DeviceTypeID " +
                        "WHERE DeviceType IN(" + ServerType + ") " +
                        "ORDER BY ServerName";
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //2/4/2015 NS added for VSPLUS-1370
        public DataTable GetDeviceUptimePctServerTypes()
        {

            DataTable dt = new DataTable();
            try
            {
                string str = "SELECT DISTINCT DeviceType AS ServerType, 1 AS OrderNum FROM DeviceUpTimeSummaryStats " +
                    "UNION " + 
                    "SELECT 'All' AS ServerType, 0 AS OrderNum " +
                    "ORDER BY OrderNum,ServerType";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //11/3/2014 NS added for VSPLUS-648
        public DataTable GetDominoStatList(string stattype,string statname)
        {
            DataTable dt = new DataTable();
            string fromtable = "DominoSummaryStats";
            string joinclause = "";
            try
            {
                //4/18/2016 NS modified for VSPLUS-2827
                if (stattype == "Domino")
                {
                    fromtable = "DominoSummaryStats";
                }
                else if (stattype == "Exchange")
                {
                    fromtable = "MicrosoftSummaryStats";
                    joinclause = " INNER JOIN vitalsigns.dbo.ServerTypes st ON st.ID=ServerTypeId WHERE ServerType='" + stattype + "' ";
                }
                else if (stattype == "Connections")
                {
                    fromtable = "IbmConnectionsSummaryStats";
                }
                string str = "SELECT DISTINCT StatName FROM " + fromtable + joinclause;
                if (statname != "" && stattype == "Domino")
                {
                    str += " WHERE StatName LIKE '%" + statname + "%' ";
                }
                str += " ORDER BY StatName";
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //11/3/2014 NS added for VSPLUS-648
        public DataTable GetDominoStatValues(string statname,string stattype,string startdate,string enddate,string servername)
        {
            string str = "";
            string wheredate = "";
            string fromtable = "DominoSummaryStats";
            DataTable dt = new DataTable();
            try
            {
                if (statname != "")
                {
                    //4/18/2016 NS modified for VSPLUS-2827
                    wheredate = " AND Date BETWEEN '" + startdate + "' AND '" + enddate + "' ";
                    if (stattype == "Domino")
                    {
                        fromtable = "DominoSummaryStats";
                    }
                    else if (stattype == "Exchange")
                    {
                        fromtable = "MicrosoftSummaryStats";
                    }
                    else if (stattype == "Connections")
                    {
                        fromtable = "IbmConnectionsSummaryStats";
                    }
                    if (statname.Substring(0, 5) == "Disk.")
                    {
                        //12/18/2015 NS modified for VSPLUS-2085
                        str = "SELECT ServerName,ROUND(StatValue/1024/1024/1024,1) StatValue," +
                            "CONVERT(date,Date) MonthDay, DATEPART(wk,Date) WeekNumber, Date " +
                            "FROM " + fromtable + " WHERE StatName='" + statname + "' " + wheredate;
                    }
                    else
                    {
                        //12/18/2015 NS modified for VSPLUS-2085
                        str = "SELECT ServerName,ROUND(StatValue,1) StatValue," +
                            "CONVERT(date,Date) MonthDay, DATEPART(wk,Date) WeekNumber, Date " +
                            "FROM " + fromtable + " WHERE StatName='" + statname + "' " + wheredate;
                    }
                    //3/8/2016 NS added for VSPLUS-2642
                    if (servername != "")
                    {
                        str += "AND ServerName IN(" + servername + ") ";
                        str += "ORDER BY ServerName,Date";
                    }
                    else
                    {
                        str += "ORDER BY ServerName,DATEPART(wk,Date)";
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

        //1/27/2015 NS added for VSPLUS-1324
        public DataTable GetSametimeStatNames()
        {
            DataTable dt = new DataTable();
            string sqlquery = "";
            try
            {
                //6/25/2015 NS modified for VSPLUS-1823
                //1/12/2016 NS modified for VSPLUS-1823
                sqlquery = "SELECT DISTINCT (CASE WHEN StatName='TotalnWayChats' THEN 'Total n-Way Chats' WHEN StatName='Total2WayChats' THEN 'Total 2-Way Chats' " +
                    "WHEN StatName='PeakLogins' THEN 'Peak Logins' ELSE StatName END) StatName " +
                    "FROM SametimeSummaryStats ORDER BY StatName ";
                dt = objAdaptor1.FetchData(sqlquery);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //1/30/2015 NS added for VSPLUS-1370
        public DataTable fillServerTypeList()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT DISTINCT DeviceType AS ServerType, 1 AS OrderNum " +
                   "FROM DeviceUpTimeStats " + 
                   "WHERE StatName='HourlyDownTimeMinutes' " +
                   "UNION " +
                   "SELECT 'All' AS ServerType, 0 AS OrderNum " +
                   "ORDER BY OrderNum, ServerType ";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //2/2/2015 NS added for VSPLUS
        //3/13/2015 NS modified for VSPLUS-1534
        public DataTable fillServerTypeList2(string statname)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT DISTINCT ServerType, 1 AS OrderNum FROM vitalsigns.dbo.ServerTypes t2 " +
                    "INNER JOIN [MicrosoftSummaryStats] t1 ON t1.ServerTypeId=t2.ID " +
                    "WHERE StatName='" + statname + "'  " +
                    "UNION " +
                    "SELECT 'All' AS ServerType, 0 AS OrderNum " +
                    "UNION " + 
                    "SELECT 'Domino' AS ServerType, 1 AS OrderNum " +
                    "ORDER BY OrderNum,ServerType ";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //3/12/2015 NS added for VSPLUS-1534
        public DataTable GetExchangeServerList(string statname)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT DISTINCT ServerName FROM MicrosoftDailyStats WHERE StatName='" + statname + "' ";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //3/12/2015 NS added for VSPLUS-1534
        public DataTable GetExchangeMailSentCount(string servername,string statname, string datefrom, string dateto, string rpttype)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                if (rpttype == "0") //Per Hour
                {
                    query = "SELECT ServerName,StatValue,Date,DATEPART(hour, Date) AS Hour " +
                        "FROM MicrosoftDailyStats " +
                        "WHERE StatName='" + statname + "' " + 
                        "AND DATEADD(DAY, DATEDIFF(DAY, 0, Date), 0) = DATEADD(DAY, DATEDIFF(DAY, 0, '" + datefrom + "'), 0) ";
                    if (servername != "")
                    {
                        query += "AND ServerName IN(" + servername + ") ";
                    }
                }
                else //Per Day
                {
                    query = "SELECT ServerName,SUM(StatValue) AS StatValue,DATEADD(DAY, DATEDIFF(DAY, 0, Date), 0) AS Date " +
                        "FROM MicrosoftDailyStats " +
                        "WHERE StatName='" + statname + "' " +
                        "AND Date BETWEEN DATEADD(DAY, DATEDIFF(DAY, 0, '" + datefrom + "'), 0) AND DATEADD(DAY, DATEDIFF(DAY, 0, '" + dateto + "'), 1) ";
                    if (servername != "")
                    {
                        query += "AND ServerName IN(" + servername + ") ";
                    }
                    query += "GROUP BY ServerName,DATEADD(DAY, DATEDIFF(DAY, 0, Date), 0) ";
                }
                query += "ORDER BY ServerName,Date ";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //3/13/2015 NS added for VSPLUS-1534
        public DataTable ResponseTimeTrendRpt(string datefrom, string dateto, string ServerName, bool exactmatch, string ServerType,string rpttype)
        {
            DataTable dt = new DataTable();
            string str = "";
            string date = "";
            /*
            try
            {
                date = year.ToString() + "-" + month.ToString() + "-" + day.ToString();
                if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                {
                    if (ServerName == "" || ServerName == "''")
                    {
                        str = "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                            "FROM dbo.DeviceDailySummaryStats " +
                            "WHERE StatName='DailyResponseAverage' AND DeviceType='Domino' AND MonthNumber=" + month + " AND YearNumber=" + year + " " +
                            "GROUP BY ServerName,MonthNumber,YearNumber " +
                            "UNION " + 
                            "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                            "FROM dbo.MicrosoftSummaryStats " +
                            "WHERE StatName='ResponseTime' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                            "GROUP BY ServerName,MonthNumber,YearNumber ";
                        str += "ORDER BY YearNumber DESC, MonthNumber DESC, ServerName";
                    }
                    else
                    {
                        if (exactmatch)
                        {
                            str = "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                                "FROM dbo.DeviceDailySummaryStats " +
                                "WHERE StatName='DailyResponseAverage' AND DeviceType='Domino' AND MonthNumber=" + month + " AND YearNumber=" + year + " " +
                                "AND ServerName IN(" + ServerName + ") " +
                                "GROUP BY ServerName,MonthNumber,YearNumber " +
                                "UNION " +
                                "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                                "FROM dbo.MicrosoftSummaryStats " +
                                "WHERE StatName='ResponseTime' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                                "AND ServerName IN(" + ServerName + ") " +
                                "GROUP BY ServerName,MonthNumber,YearNumber ";
                            str += "ORDER BY YearNumber DESC, MonthNumber DESC, ServerName";
                        }
                        else
                        {
                            str = "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                                "FROM dbo.DeviceDailySummaryStats " +
                                "WHERE StatName='DailyResponseAverage' AND DeviceType='Domino' AND MonthNumber=" + month + " AND YearNumber=" + year + " " +
                                "AND ServerName LIKE '%" + ServerName + "%' " +
                                "GROUP BY ServerName,MonthNumber,YearNumber " +
                                "UNION " +
                                "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                                "FROM dbo.MicrosoftSummaryStats " +
                                "WHERE StatName='ResponseTime' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                                "AND ServerName LIKE '%" + ServerName + "%' " +
                                "GROUP BY ServerName,MonthNumber,YearNumber ";
                            str += "ORDER BY YearNumber DESC, MonthNumber DESC, ServerName";
                        }
                    }
                }
                else
                {
                    if (ServerName == "" || ServerName == "''")
                    {
                        if (ServerType.IndexOf("Domino") != -1)
                        {
                            str = "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                                "FROM dbo.DeviceDailySummaryStats " +
                                "WHERE StatName='DailyResponseAverage' AND DeviceType='Domino' AND MonthNumber=" + month + " AND YearNumber=" + year + " " +
                                "GROUP BY ServerName,MonthNumber,YearNumber " +
                                "UNION " +
                                "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                                "FROM dbo.MicrosoftSummaryStats t1 " +
                                "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                "WHERE StatName='ResponseTime' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                                "AND t2.ServerType IN(" + ServerType + ") " +
                                "GROUP BY ServerName,MonthNumber,YearNumber ";
                        }
                        else
                        {
                            str = "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                                "FROM dbo.MicrosoftSummaryStats t1 " +
                                "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                "WHERE StatName='ResponseTime' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                                "AND t2.ServerType IN(" + ServerType + ") " +
                                "GROUP BY ServerName,MonthNumber,YearNumber ";
                        }                        
                        str += "ORDER BY YearNumber DESC, MonthNumber DESC, ServerName";
                    }
                    else
                    {
                        if (exactmatch)
                        {
                            if (ServerType.IndexOf("Domino") != -1)
                            {
                                str = "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                                    "FROM dbo.DeviceDailySummaryStats " +
                                    "WHERE StatName='DailyResponseAverage' AND DeviceType='Domino' AND MonthNumber=" + month + " AND YearNumber=" + year + " " +
                                    "AND ServerName IN(" + ServerName + ") " +
                                    "GROUP BY ServerName,MonthNumber,YearNumber " +
                                    "UNION " +
                                    "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                                    "FROM dbo.MicrosoftSummaryStats  t1 " +
                                    "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                    "WHERE StatName='ResponseTime' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                                    "AND ServerName IN(" + ServerName + ") " +
                                    "AND t2.ServerType IN(" + ServerType + ") " +
                                    "GROUP BY ServerName,MonthNumber,YearNumber ";
                            }
                            else
                            {
                                str = "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                                        "FROM dbo.MicrosoftSummaryStats  t1 " +
                                        "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                        "WHERE StatName='ResponseTime' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                                        "AND ServerName IN(" + ServerName + ") " +
                                        "AND t2.ServerType IN(" + ServerType + ") " +
                                        "GROUP BY ServerName,MonthNumber,YearNumber ";
                            }
                            str += "ORDER BY YearNumber DESC, MonthNumber DESC, ServerName";
                        }
                        else
                        {
                            if (ServerType.IndexOf("Domino") != -1)
                            {
                                str = "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                                    "FROM dbo.DeviceDailySummaryStats " +
                                    "WHERE StatName='DailyResponseAverage' AND DeviceType='Domino' AND MonthNumber=" + month + " AND YearNumber=" + year + " " +
                                    "AND ServerName LIKE '%" + ServerName + "%' " +
                                    "GROUP BY ServerName,MonthNumber,YearNumber " +
                                    "UNION " +
                                    "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                                    "FROM dbo.MicrosoftSummaryStats t1 " +
                                    "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                    "WHERE StatName='ResponseTime' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                                    "AND ServerName LIKE '%" + ServerName + "%' " +
                                    "AND t2.ServerType IN(" + ServerType + ") " +
                                    "GROUP BY ServerName,MonthNumber,YearNumber ";
                            }
                            else
                            {
                                str = "SELECT ServerName,ROUND(SUM(StatValue)/dbo.CalcNumDaysinMonth(CAST('" + date + "' as datetime)),1) ResponseTime,MonthNumber,YearNumber " +
                                    "FROM dbo.MicrosoftSummaryStats t1 " +
                                    "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                    "WHERE StatName='ResponseTime' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() + " " +
                                    "AND ServerName LIKE '%" + ServerName + "%' " +
                                    "AND t2.ServerType IN(" + ServerType + ") " +
                                    "GROUP BY ServerName,MonthNumber,YearNumber ";
                            }
                            str += "ORDER BY YearNumber DESC, MonthNumber DESC, ServerName";
                        }
                    }
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
             */
            if (rpttype == "0")
            {
                date = "Date ";
            }
            else
            {
                date = "MonthNumber,YearNumber ";
            }
            try
            {
                //date = year.ToString() + "-" + month.ToString() + "-" + day.ToString();
                if (ServerType == "" || ServerType == "''" || ServerType == "'All'")
                {
                    if (ServerName == "" || ServerName == "''")
                    {
                        //8/25/2015 NS modified for VSPLUS-1619
                        str = "SELECT ServerName,ROUND(SUM(StatValue)/(DATEDIFF(day,'" + datefrom + "','" + dateto + "')),1) ResponseTime," + date + 
                            "FROM( " +
                            "SELECT ServerName,StatValue," + date + 
                            "FROM dbo.DeviceDailySummaryStats t1 " +
                            "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                            "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                            "AND t2.ID=t3.DeviceTypeID " +
                            "WHERE StatName='DailyResponseAverage' AND DeviceType='Domino' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' " +
                            "UNION ALL " +
                            "SELECT ServerName,StatValue," + date + 
                            "FROM dbo.MicrosoftSummaryStats " +
                            "WHERE StatName='ResponseTime' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' " +
                            ") t " +
                            "GROUP BY ServerName," + date;
                        str += "ORDER BY ServerName," + date;
                    }
                    else
                    {
                        if (exactmatch)
                        {
                            //8/25/2015 NS modified for VSPLUS-1619
                            str = "SELECT ServerName,ROUND(SUM(StatValue)/(DATEDIFF(day,'" + datefrom + "','" + dateto + "')),1) ResponseTime," + date + 
                                "FROM( " +
                                "SELECT ServerName,StatValue," + date + 
                                "FROM dbo.DeviceDailySummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                                "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                                "AND t2.ID=t3.DeviceTypeID " +
                                "WHERE StatName='DailyResponseAverage' AND DeviceType='Domino' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' " + 
                                "AND ServerName IN(" + ServerName + ") " +
                                "UNION ALL " +
                                "SELECT ServerName,StatValue," + date + 
                                "FROM dbo.MicrosoftSummaryStats " + 
                                "WHERE StatName='ResponseTime' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' " +
                                "AND ServerName IN(" + ServerName + ") " +
                                ") t " +
                                "GROUP BY ServerName," + date;
                            str += "ORDER BY ServerName," + date;
                        }
                        else
                        {
                            //8/25/2015 NS modified for VSPLUS-1619
                            str = "SELECT ServerName,ROUND(SUM(StatValue)/(DATEDIFF(day,'" + datefrom + "','" + dateto + "')),1) ResponseTime," + date +
                                "FROM( " +
                                "SELECT ServerName,StatValue," + date +
                                "FROM dbo.DeviceDailySummaryStats t1 " +
                                "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                                "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                                "AND t2.ID=t3.DeviceTypeID " +
                                "WHERE StatName='DailyResponseAverage' AND DeviceType='Domino' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' " +
                                "AND ServerName LIKE '%" + ServerName + "%' " +
                                "UNION ALL " +
                                "SELECT ServerName,StatValue," + date + 
                                "FROM dbo.MicrosoftSummaryStats " +
                                "WHERE StatName='ResponseTime' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' "+ 
                                "AND ServerName LIKE '%" + ServerName + "%' " +
                                ") t " +
                                "GROUP BY ServerName," + date;
                            str += "ORDER BY ServerName," + date;
                        }
                    }
                }
                else
                {
                    if (ServerName == "" || ServerName == "''")
                    {
                        if (ServerType.IndexOf("Domino") != -1)
                        {
                            //8/25/2015 NS modified for VSPLUS-1619
                            str = "SELECT ServerName,ROUND(SUM(StatValue)/(DATEDIFF(day,'" + datefrom + "','" + dateto + "')),1) ResponseTime," + date + 
                                "FROM( " +
                                "SELECT ServerName,StatValue," + date + 
                                "FROM dbo.DeviceDailySummaryStats t1 " +
                                "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                                "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                                "AND t2.ID=t3.DeviceTypeID " +
                                "WHERE StatName='DailyResponseAverage' AND DeviceType='Domino' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' " +
                                "UNION ALL " +
                                "SELECT ServerName,StatValue," + date + 
                                "FROM dbo.MicrosoftSummaryStats t1 " +
                                "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                "WHERE StatName='ResponseTime' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' " + 
                                "AND t2.ServerType IN(" + ServerType + ") " +
                                ") t " +
                                "GROUP BY ServerName," + date;
                        }
                        else
                        {
                            str = "SELECT ServerName,ROUND(SUM(StatValue)/(DATEDIFF(day,'" + datefrom + "','" + dateto + "')),1) ResponseTime," + date + 
                                "FROM( " +
                                "SELECT ServerName,StatValue," + date +
                                "FROM dbo.MicrosoftSummaryStats t1 " +
                                "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                                "WHERE StatName='ResponseTime' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' " +
                                "AND t2.ServerType IN(" + ServerType + ") " +
                                ") t " +
                                "GROUP BY ServerName," + date;
                        }
                        str += "ORDER BY ServerName," + date;
                    }
                    else
                    {
                        if (exactmatch)
                        {
                            if (ServerType.IndexOf("Domino") != -1)
                            {
                                //8/25/2015 NS modified for VSPLUS-1619
                                str = "SELECT ServerName,ROUND(SUM(StatValue)/(DATEDIFF(day,'" + datefrom + "','" + dateto + "')),1) ResponseTime," + date + 
                                    "FROM( " +
                                    "SELECT ServerName,StatValue," + date +
                                    "FROM dbo.DeviceDailySummaryStats t1 " +
                                    "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                                    "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                                    "AND t2.ID=t3.DeviceTypeID " +
                                    "WHERE StatName='DailyResponseAverage' AND DeviceType='Domino' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' " +
                                    "AND ServerName IN(" + ServerName + ")  " +
                                    "UNION ALL " +
                                    "SELECT ServerName,StatValue," + date +
                                    "FROM dbo.MicrosoftSummaryStats t1 " +
                                    "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID  " +
                                    "WHERE StatName='ResponseTime' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' " +
                                    "AND ServerName IN(" + ServerName + ")  " +
                                    "AND t2.ServerType IN(" + ServerType + ") " +
                                    ") t " +
                                    "GROUP BY ServerName," + date;
                            }
                            else
                            {
                                str = "SELECT ServerName,ROUND(SUM(StatValue)/(DATEDIFF(day,'" + datefrom + "','" + dateto + "')),1) ResponseTime," + date + 
                                    "FROM( " +
                                    "SELECT ServerName,StatValue," + date + 
                                    "FROM dbo.MicrosoftSummaryStats t1 " + 
                                    "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID  " + 
                                    "WHERE StatName='ResponseTime' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' " + 
                                    "AND ServerName IN(" + ServerName + ")  " + 
                                    "AND t2.ServerType IN(" + ServerType + ") " + 
                                    ") t " +
                                    "GROUP BY ServerName," + date;
                            }
                            str += "ORDER BY ServerName," + date;
                        }
                        else
                        {
                            if (ServerType.IndexOf("Domino") != -1)
                            {
                                //8/25/2015 NS modified for VSPLUS-1619
                                str = "SELECT ServerName,ROUND(SUM(StatValue)/(DATEDIFF(day,'" + datefrom + "','" + dateto + "')),1) ResponseTime," + date + 
                                    "FROM( " +
                                    "SELECT ServerName,StatValue," + date + 
                                    "FROM dbo.DeviceDailySummaryStats t1 " +
                                    "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                                    "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                                    "AND t2.ID=t3.DeviceTypeID " +
                                    "WHERE StatName='DailyResponseAverage' AND DeviceType='Domino' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' " +
                                    "AND ServerName LIKE '%" + ServerName + "%' " +
                                    "UNION ALL " +
                                    "SELECT ServerName,StatValue," + date +
                                    "FROM dbo.MicrosoftSummaryStats t1 " +
                                    "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID  " +
                                    "WHERE StatName='ResponseTime' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' " +
                                    "AND ServerName LIKE '%" + ServerName + "%' " +
                                    "AND t2.ServerType IN(" + ServerType + ") " +
                                    ") t " +
                                    "GROUP BY ServerName," + date;
                            }
                            else
                            {
                                str = "SELECT ServerName,ROUND(SUM(StatValue)/(DATEDIFF(day,'" + datefrom + "','" + dateto + "')),1) ResponseTime," + date +
                                    "FROM( " +
                                    "SELECT ServerName,StatValue," + date +
                                    "FROM dbo.MicrosoftSummaryStats t1 " + 
                                    "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID  " + 
                                    "WHERE StatName='ResponseTime' AND Date BETWEEN '" + datefrom + "' AND '" + dateto + "' " + 
                                    "AND ServerName LIKE '%" + ServerName + "%' " + 
                                    "AND t2.ServerType IN(" + ServerType + ") " +
                                    ") t " +
                                    "GROUP BY ServerName," + date;
                            }
                            str += "ORDER BY ServerName," + date;
                        }
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

        //3/13/2015 NS added for VSPLUS-1534
        public DataTable fillServerListByType(string sType)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                if (sType == "" || sType == "''" || sType == "'All'")
                {
                    //8/25/2015 NS modified for VSPLUS-1619
                    query = "SELECT DISTINCT ServerName " +
                    "FROM DeviceDailySummaryStats t1 " +
                    "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                    "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                    "AND t2.ID=t3.DeviceTypeID " +
                    "WHERE StatName='DailyResponseAverage' " +
                    "AND DeviceType='Domino' " +
                    "UNION " +
                    "SELECT DISTINCT ServerName " +
                    "FROM MicrosoftSummaryStats " +
                    "WHERE StatName='ResponseTime' ";
                }
                else
                {
                    if (sType.IndexOf("Domino") != -1)
                    {
                        //8/25/2015 NS modified for VSPLUS-1619
                        query = "SELECT DISTINCT ServerName " +
                            "FROM DeviceDailySummaryStats t1 " +
                            "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON " +
                            "DeviceType=ServerType INNER JOIN vitalsigns.dbo.DeviceInventory t3 ON ServerName=Name " +
                            "AND t2.ID=t3.DeviceTypeID " +
                            "WHERE StatName='DailyResponseAverage' " +
                            "AND DeviceType='Domino' " +
                            "UNION " +
                            "SELECT DISTINCT ServerName " +
                            "FROM MicrosoftSummaryStats t1 " +
                            "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                            "WHERE StatName='ResponseTime' " +
                            "AND t2.ServerType IN(" + sType + ") ";
                    }
                    else
                    {
                        query = "SELECT DISTINCT ServerName " +
                            "FROM MicrosoftSummaryStats t1 " +
                            "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                            "WHERE StatName='ResponseTime' " +
                            "AND t2.ServerType IN(" + sType + ") ";
                    }
                }
                query += "ORDER BY ServerName";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //4/14/2015 NS added for VSPLUS-1635
        public DataTable GetDominoDBAvgSize(string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT COUNT(*) TotalFiles,SUM(ISNULL(FileSize,0)) TotalLogicalSize, " +
                    "ROUND(SUM(ISNULL(FileSize,0))/COUNT(*),1) AverageSize, " +
                    "Server,CASE WHEN CHARINDEX('\\',FileNamePath) = 0 THEN '' ELSE " +
                    "SUBSTRING(FileNamePath,1,CHARINDEX('\\',FileNamePath)-1) END AS Folder " +
                    "FROM dbo.Daily " +
                    "WHERE CASE WHEN CHARINDEX('\\',FileNamePath) = 0 THEN '' ELSE " +
                    "SUBSTRING(FileNamePath,1,CHARINDEX('\\',FileNamePath)-1) END != '' ";
                if (servername != "")
                {
                    query += " AND Server='" + servername + "' ";
                }
                query += "GROUP BY Server,CASE WHEN CHARINDEX('\\',FileNamePath) = 0 THEN '' ELSE " +
                    "SUBSTRING(FileNamePath,1,CHARINDEX('\\',FileNamePath)-1) END ";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //6/15/2015 NS added for VSPLUS-1841
        //1/5/2016 NS modified for VSPLUS-1534
        public DataTable GetUserCountTrend(string typeval, string startdt, string enddt, string servername, 
            string fromtableParam, string statnameParam, bool exactmatch)
        {
            DataTable dt = new DataTable();
            string query = "";
            string fromtable = "DominoSummaryStats";
            string statname = "Server.Users";
            try
            {
                if (fromtableParam != "")
                {
                    fromtable = fromtableParam;
                }
                if (statnameParam != "")
                {
                    statname = statnameParam;
                }
                //7/29/2015 NS modified for VSPLUS-2023
                if (exactmatch)
                {
                    query = "SELECT ServerName, ROUND(SUM(StatValue),0) UserCount, ROUND(SUM(StatValue),0) StatValue, Date " +
                        "FROM " + fromtable + " " +
                        "WHERE StatName='" + statname + "' AND (Date >= '" + startdt + "') AND (Date <= '" + enddt + "') ";
                }
                else
                {
                    query = "SELECT ServerName, ROUND(SUM(StatValue),0) UserCount, ROUND(SUM(StatValue),0) StatValue, Date " +
                        "FROM " + fromtable + " " +
                        "WHERE StatName LIKE '" + statname + "%' AND (Date >= '" + startdt + "') AND (Date <= '" + enddt + "') ";
                }
                if (servername != "")
                {
                    query += "AND ServerName IN(" + servername + ") ";
                }
                query += "GROUP BY ServerName, Date " +
                         "ORDER BY ServerName, Date ";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //7/29/2015 NS added for VSPLUS-2023
        //1/5/2016 NS modified for VSPLUS-1534
        public DataTable GetUserCountServers(string typeval, string fromtableParam, string statnameParam, bool exactmatch)
        {
            DataTable dt = new DataTable();
            string query = "";
            string fromtable = "DominoSummaryStats";
            string statname = "Server.Users";
            try
            {
                if (fromtableParam != "")
                {
                    fromtable = fromtableParam;
                }
                if (statnameParam != "")
                {
                    statname = statnameParam;
                }
                if (exactmatch)
                {
                    query = "SELECT DISTINCT ServerName FROM " + fromtable + " " +
                            "WHERE StatName='" + statname + "' ";
                }
                else
                {
                    query = "SELECT DISTINCT ServerName FROM " + fromtable + " " +
                            "WHERE StatName LIKE '" + statname + "%' ";
                }
                query += "ORDER BY ServerName ";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //12/1/2015 NS added for VSPLUS-2140
        public DataTable GetMSUserCountTypes(string stype)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                query = "SELECT DISTINCT SUBSTRING(StatName,CHARINDEX('@',StatName)+1,CHARINDEX('#',StatName)-CHARINDEX('@',StatName)-1) StatName " +
                    "FROM MicrosoftSummaryStats " +
                    "WHERE StatName LIKE '" + stype + "CAS%' ";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //12/1/2015 NS added for VSPLUS-2140
        public DataTable GetMSUserCounts(string stype, string utype, string servername, string sdate, string edate)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                query = "SELECT CASE WHEN StatValue > 1000000 THEN 1 ELSE ROUND(StatValue,0) END StatValue, ServerName, Date " +
                    "FROM MicrosoftSummaryStats " +
                    "WHERE StatName='" + stype + "CAS" + "@" + utype + "#User.Count' " +
                    "AND Date BETWEEN CONVERT(DATETIME,'" + sdate + "',101) AND CONVERT(DATETIME,'" + edate + "',101) ";
                if (servername != "")
                {
                    query += "AND ServerName IN(" + servername + ") ";
                }
                query += "ORDER BY ServerName, Date ";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //12/4/2015 NS added for VSPLUS-2140
        public DataTable GetMSServers(string stype, string utype)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                query = "SELECT DISTINCT ServerName " +
                    "FROM MicrosoftSummaryStats " +
                    "WHERE StatName='" + stype + "CAS" + "@" + utype + "#User.Count' ";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //17/3/2016 sowmya added for VSPLUS 2455
        public DataTable Getclusterinfo()
        {
            DataTable dt = new DataTable();
            try
            {
                //3/28/2016 NS modified for VSPLUS-2455
                //5/24/2016 NS modified for VSPLUS-2997
                string str = "select dc.ID,dc.Name ,dc.First_Alert_Threshold,st.LastUpdate, st.NextScan, st.Status from Status st inner join dominocluster dc on dc.Name=st.Name " +
                    "inner join servertypes st1 on st.type=st1.servertype where st.type='Domino Cluster database' ";
                dt = objAdaptor.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        
        
        }

        //3/17/2016 Durga added for VSPLUS-2702
        public DataTable GetStalemailboxesInfo(string server)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                if (server != "")

                    query = "SELECT  [DisplayName],[InActiveDaysCount] FROM [O365AdditionalMailDetails] WHERE [InActiveDaysCount]>30 and Server='" + server + "'ORDER BY InActiveDaysCount DESC";

                else
                    query = "SELECT  [DisplayName],[InActiveDaysCount] FROM [O365AdditionalMailDetails] WHERE [InActiveDaysCount]>30  ORDER BY InActiveDaysCount DESC ";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //3/28/2016 Durga Added for VSPLUS-2698
        public DataTable GetServerUtiliZationOfDomino(string Name)
        {
            //5/5/2016 Sowjanya modified for VSPLUS-2919
            DataTable dt = new DataTable();
            string sqlquery = "";
            try
            {
                if(Name!="")

                    sqlquery = "select avg(StatValue) as StatValue,ds.ServerName,sr.IdealUserCount  from [VSS_Statistics].dbo.DominoSummaryStats Ds inner join  [vitalsigns].dbo.servers sr on Ds.ServerName=sr.ServerName where statname ='Server.Users' and ds.ServerName Like '" + "%" + Name + "%" + "'   group by ds.ServerName,sr.IdealUserCount order by ServerName ";
                else
                    sqlquery = "select avg(StatValue) as StatValue,ds.ServerName,sr.IdealUserCount  from [VSS_Statistics].dbo.DominoSummaryStats Ds inner join  [vitalsigns].dbo.servers sr on Ds.ServerName=sr.ServerName where statname ='Server.Users' group by ds.ServerName,sr.IdealUserCount order by ServerName";
                dt = objAdaptor1.FetchData(sqlquery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }
        public DataTable GetServerUtiliZationOfExchange(string Name,string StatName)

        {
            //5/5/2016 Sowjanya modified for VSPLUS-2919
            DataTable dt = new DataTable();
            string sqlquery = "";
            try
            {
                if (Name != "")

                    sqlquery = "select avg(StatValue) as StatValue,Ms.ServerName,sr.IdealUserCount  from [VSS_Statistics].dbo.MicrosoftSummaryStats Ms inner join  [vitalsigns].dbo.servers sr on Ms.ServerName=sr.ServerName where statname like  '" + "AvgCAS@" + StatName + "#User.Count' and Ms.ServerName like '" + "%" + Name + "%" + "'  group by Ms.ServerName,sr.IdealUserCount order by Ms.ServerName";
                else
                    sqlquery = "select avg(StatValue) as StatValue,Ms.ServerName,sr.IdealUserCount  from [VSS_Statistics].dbo.MicrosoftSummaryStats Ms inner join  [vitalsigns].dbo.servers sr on Ms.ServerName=sr.ServerName where statname like  '" + "AvgCAS@" + StatName + "#User.Count'  group by Ms.ServerName,sr.IdealUserCount order by Ms.ServerName";
                dt = objAdaptor1.FetchData(sqlquery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        //4/12/2016 Sowjanya added for VSPLUS-2831
        public DataTable GetMSForumTypes()
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
               
                query = "SELECT distinct StatName , case when statname='NUM_OF_FORUMS_FORUMS_CREATED_YESTERDAY' then 'Created'"+
                    "when   statname='NUM_OF_FORUMS_TOPICS_CREATED_YESTERDAY' then 'Topics' when   statname='NUM_OF_FORUMS_REPLIES_CREATED_YESTERDAY' then 'Replies'"+
                    "end as Dstatname from IbmConnectionsSummaryStats where StatName in('NUM_OF_FORUMS_FORUMS_CREATED_YESTERDAY','NUM_OF_FORUMS_TOPICS_CREATED_YESTERDAY','NUM_OF_FORUMS_REPLIES_CREATED_YESTERDAY')";
                
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

       
        public DataTable GetIBMConnectionsServerlist(string statname1)
            {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT DISTINCT ServerName FROM [IbmConnectionsSummaryStats] WHERE StatName='" + statname1 + "'";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        //11-05-2016 Durga Modified for VSPLUS-2836
        public DataTable GetonnectionsStatsNames(string StatName1, string StatName2)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                query = "select distinct case when statname='" + StatName1 + "' then 'Created' when   statname='" + StatName2 + "' then 'Edited' end as statname from  [IbmConnectionsSummaryStats] where statname in('" + StatName1 + "','" + StatName2 + "')";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //26/4/2016 Durga Modified for VSPLUS-2878
        public DataTable GetonnectionsStatsNamesofWikis(string StatName1, string StatName2)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                query = "select distinct case when statname='" + StatName1 + "' then 'Wikis Created' when   statname='" + StatName2 + "' then 'Pages Created' end as statname from  [IbmConnectionsSummaryStats] where statname in('" + StatName1 + "','" + StatName2 + "')";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        //11-05-2016 Durga Modified for VSPLUS-2836
        public DataTable GetIBMConnectionstatsInfo(string servername, string statname, string datefrom, string dateto)

        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
               
               
                    query = "SELECT ServerName,StatValue,DATEADD(DAY, DATEDIFF(DAY, 0, Date), 0) AS Date " +
                        "FROM [IbmConnectionsSummaryStats] " +
                        "WHERE StatName='" + statname + "' " +
                        "AND Date BETWEEN DATEADD(DAY, DATEDIFF(DAY, 0, '" + datefrom + "'), 0) AND DATEADD(DAY, DATEDIFF(DAY, 0, '" + dateto + "'), 1) ";
                    if (servername != "")
                    {
                        query += "AND ServerName IN(" + servername + ") ";
                    }
                  
              
                query += " ORDER BY ServerName,Date ";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        //12-05-2016 Sowmya Modified for VSPLUS-2830
        public DataTable Getfiletypes(string statname1, string statname2, string statname3, string statname4)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                query = "select distinct statname, case when statname='" + statname1 + "' then 'Created' when   statname='" + statname2 + "' then 'Updated' when statname='" + statname3 + "' then 'Downloaded' when statname='" + statname4 + "' then 'Revisioned' end as Userstatname from  [IbmConnectionsSummaryStats] where statname in('" + statname1 + "','" + statname2 + "','" + statname3 + "','" + statname4 + "')";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        //13/4/2016 Durga Modified for VSPLUS-2832
        public DataTable GetActivityStatNames()
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                query = "select distinct statname,case when statname='ACTIVITY_LOGINS_LAST_DAY' then 'Logins' when   statname='NUM_OF_ACTIVITIES_ACTIVITIES_CREATED_YESTERDAY' then 'Created' when statname='NUM_OF_ACTIVITIES_ACTIVITIES_FOLLOWED_YESTERDAY' then 'Followed'  end as Userstatname from  [IbmConnectionsSummaryStats] where statname in('ACTIVITY_LOGINS_LAST_DAY','NUM_OF_ACTIVITIES_ACTIVITIES_CREATED_YESTERDAY','NUM_OF_ACTIVITIES_ACTIVITIES_FOLLOWED_YESTERDAY')";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        //5/9/2016 Sowjanya modified for VSPLUS-2931
        public DataTable GetDBClusterC(string ClusterName)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                //9/30/2015 NS modified for VSPLUS-2150
                query = "select ServerCName from DominoCluster " +
                    " WHERE Name='" + ClusterName + "' ";
                dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        //6/3/2016 Sowjanya modified for VSPLUS-2895
        public DataTable CommunityNames()
        {
            DataTable dt = new DataTable();
            try
            {
                //8/5/2014 NS modified - added Exchange servers to the selection
                string str = "SELECT DISTINCT [Name]  FROM IbmConnectionsObjects order by Name ";
                  
                dt = objAdaptor.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public DataTable SetGraphForTags(string serverName)
        {
           
            DataTable dt = new DataTable();
            try
            {
                
                string strQuerry = "";
                
                  if (serverName != "")
                    {


                        strQuerry = "select distinct io.Name,it.tag from IbmConnectionsTags it inner join IbmConnectionsObjectTags iot on it.ID = iot.TagID " +
                       "inner join IbmConnectionsObjects io on io.ID = iot.ObjectID  where Name = '" + serverName + "' "; 
                    }
                
                else
                   {
                        
                        
                            strQuerry = "select distinct io.Name,it.tag from IbmConnectionsTags it inner join IbmConnectionsObjectTags iot on it.ID = iot.TagID inner join " +
                                         "IbmConnectionsObjects io on io.ID = iot.ObjectID";  
                       
                    }

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
