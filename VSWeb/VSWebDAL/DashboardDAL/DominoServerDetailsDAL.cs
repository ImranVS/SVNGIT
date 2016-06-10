using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
    public class DominoServerDetailsDAL
    {
        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
        private Adaptor adaptor = new Adaptor();
        private static DominoServerDetailsDAL _self = new DominoServerDetailsDAL();

        public static DominoServerDetailsDAL Ins
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
                    //string strQuerry = "SELECT [DeviceName], [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";                    

                    //string strQuerry = "select devicename,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //            " from [VSS_Statistics].[dbo].[DeviceDailyStats] " +
                    //            " where Date >= '11/10/2012' and StatName='ResponseTime' and [DeviceName]='" + DeviceName + "' " +
                    //            " group by devicename, convert(varchar(5),Date,108) ";

                    //dt = objAdaptor.FetchData(strQuerry);


                    dt = objAdaptor.FetchDeviceHourlyVals("ResponseTime", System.DateTime.Now, DeviceName);



                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            return dt;
        }

        public DataTable SetGraphforNotes(string paramGraph, string serverName)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    //string strQuerry = "SELECT [DeviceName], [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";                    

                    //string strQuerry = "select devicename,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //            " from [VSS_Statistics].[dbo].[DeviceDailyStats] " +
                    //            " where Date >= '11/10/2012' and StatName='ResponseTime' and [DeviceName]='" + DeviceName + "' " +
                    //            " group by devicename, convert(varchar(5),Date,108) ";

                    //dt = objAdaptor.FetchData(strQuerry);


                    dt = objAdaptor.FetchNotesHourlyVals("DeliveryTime.Seconds", System.DateTime.Now, serverName);



                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            return dt;
        }

        public DataTable SetGraphForDiskSpace(string serverName)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT [DiskName], [DiskFree], [DiskSize]-[DiskFree] As DiskUsed FROM [DominoDiskSpace] where [ServerName]='" + serverName + "' order by [DiskName]";
                dt = adaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
		public DataTable SetGraphForDiskSpace(string serverName, string DiskName, string serverType)
		{
            //2/6/2015 NS modified for VSPLUS-1370
			DataTable dt = new DataTable();
			try
			{
                //7/14/2014 NS modified for VSPUS-813
                //string strQuerry = "SELECT [DiskName], [DiskFree], [DiskSize]-[DiskFree] As DiskUsed FROM [DominoDiskSpace] where [ServerName]='" + serverName + "' AND DiskName='" + DiskName + "' order by [DiskName]";
                string strQuerry = "";
                //7/14/2014 NS modified for VSPUS-816
                //8/5/2014 NS modified
                if (serverType == "" || serverType == "''" || serverType == "'All'")
                {
                    if (serverName != "")
                    {
                        if (DiskName != "")
                        {
                            //5/4/2015 NS modified for VSPLUS-1707
                            strQuerry = "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], [DiskFree], " +
                                "[DiskSize]-[DiskFree] As DiskUsed FROM [DominoDiskSpace] where [ServerName] IN(" + serverName + ") " +
                                "AND DiskName='" + DiskName + "' " +
                                "UNION " +
                                "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], [DiskFree], " +
                                "[DiskSize]-[DiskFree] As DiskUsed FROM [DiskSpace] where [ServerName] IN(" + serverName + ") " +
                                "' AND DiskName='" + DiskName + "' " +
                                "order by [DiskName]";
                        }
                        else
                        {
                            //5/4/2015 NS modified for VSPLUS-1707
                            strQuerry = "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], ROUND([DiskFree],2) DiskFree, " +
                                "ROUND([DiskSize]-[DiskFree],2) As DiskUsed FROM [DominoDiskSpace] where [ServerName] IN(" + serverName + ") " +
                                "UNION " +
                                "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], ROUND([DiskFree],2) DiskFree, " +
                                "ROUND([DiskSize]-[DiskFree],2) As DiskUsed FROM [DiskSpace] where [ServerName] IN(" + serverName + ") " +
                                "order by [DiskName]";
                        }
                    }
                    else
                    {
                        strQuerry = "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], ROUND([DiskFree],2) DiskFree, " +
                            "ROUND([DiskSize]-[DiskFree],2) As DiskUsed FROM [DominoDiskSpace] " +
                            "UNION " +
                            "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], ROUND([DiskFree],2) DiskFree, " +
                            "ROUND([DiskSize]-[DiskFree],2) As DiskUsed FROM [DiskSpace] " +
                            "order by [ServerName],[DiskName]";
                    }
                }
                else
                {
                    if (serverType.IndexOf("Domino") != -1)
                    {
                        if (serverName != "")
                        {
                            if (DiskName != "")
                            {
                                //5/4/2015 NS modified for VSPLUS-1707
                                strQuerry = "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], [DiskFree], " +
                                    "[DiskSize]-[DiskFree] As DiskUsed FROM [DominoDiskSpace] where [ServerName] IN(" + serverName + ") " +
                                    "AND DiskName='" + DiskName + "' " +
                                    "UNION " +
                                    "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], [DiskFree], " +
                                    "[DiskSize]-[DiskFree] As DiskUsed FROM [DiskSpace] where [ServerName] IN(" + serverName + ") " +
                                    "' AND DiskName='" + DiskName + "' " +
                                    "AND ServerType IN(" + serverType + ") " +
                                    "order by [DiskName]";
                            }
                            else
                            {
                                //5/4/2015 NS modified for VSPLUS-1707
                                strQuerry = "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], ROUND([DiskFree],2) DiskFree, " +
                                    "ROUND([DiskSize]-[DiskFree],2) As DiskUsed FROM [DominoDiskSpace] where [ServerName] IN(" + serverName + ") " +
                                    "UNION " +
                                    "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], ROUND([DiskFree],2) DiskFree, " +
                                    "ROUND([DiskSize]-[DiskFree],2) As DiskUsed FROM [DiskSpace] where [ServerName] IN(" + serverName + ") " +
                                    "AND ServerType IN(" + serverType + ") " +
                                    "order by [DiskName]";
                            }
                        }
                        else
                        {
                            strQuerry = "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], ROUND([DiskFree],2) DiskFree, " +
                                "ROUND([DiskSize]-[DiskFree],2) As DiskUsed FROM [DominoDiskSpace] " +
                                "UNION " +
                                "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], ROUND([DiskFree],2) DiskFree, " +
                                "ROUND([DiskSize]-[DiskFree],2) As DiskUsed FROM [DiskSpace] " +
                                "WHERE ServerType IN(" + serverType + ") " +
                                "order by [ServerName],[DiskName]";
                        }
                    }
                    else
                    {
                        if (serverName != "")
                        {
                            if (DiskName != "")
                            {
                                //5/4/2015 NS modified for VSPLUS-1707
                                strQuerry = "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], [DiskFree], " +
                                    "[DiskSize]-[DiskFree] As DiskUsed FROM [DiskSpace] where [ServerName] IN(" + serverName + ") " +
                                    "' AND DiskName='" + DiskName + "' " +
                                    "AND ServerType IN(" + serverType + ") " +
                                    "order by [DiskName]";
                            }
                            else
                            {
                                //5/4/2015 NS modified for VSPLUS-1707
                                strQuerry = "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], ROUND([DiskFree],2) DiskFree, " +
                                    "ROUND([DiskSize]-[DiskFree],2) As DiskUsed FROM [DiskSpace] where [ServerName] IN(" + serverName + ") " +
                                    "AND ServerType IN(" + serverType + ") " +
                                    "order by [DiskName]";
                            }
                        }
                        else
                        {
                            strQuerry = "SELECT [ServerName] + ' - ' + [DiskName] ServerDiskName, [ServerName], [DiskName], ROUND([DiskFree],2) DiskFree, " +
                                "ROUND([DiskSize]-[DiskFree],2) As DiskUsed FROM [DiskSpace] " +
                                "WHERE ServerType IN(" + serverType + ") " +
                                "order by [ServerName],[DiskName]";
                        }
                    }
                }
				dt = adaptor.FetchData(strQuerry);
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
            if (paramGraph == "hh")
            {
                try
                {
                    // string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Platform.System.PctCombinedCpuUtil' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    //string strQuerry = "select Servername,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //"from [VSS_Statistics].[dbo].[DominoDailyStats] " +
                    //"where Date >='11/10/2012' and Date <'11/11/2012' and StatName='Platform.System.PctCombinedCpuUtil'  and Servername='" + serverName + "'" +
                    //"group by Servername,convert(varchar(5),Date,108) ";
                    //dt = objAdaptor.FetchData(strQuerry);
                    dt = objAdaptor.FetchDeviceHourlyVals("Platform.System.PctCombinedCpuUtil", System.DateTime.Now, serverName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dt;
        }

        public DataTable SetGraphForMemory(string paramGraph, string serverName)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    //string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mem.PercentUsed' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    //string strQuerry = "select Servername,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //          "from [VSS_Statistics].[dbo].[DominoDailyStats] " +
                    //          "where  Date >='11/10/2012' and Date <'11/11/2012' and StatName='Mem.PercentUsed'  and Servername='" + serverName + "'" +
                    //          "group by Servername,convert(varchar(5),Date,108)";
                    //dt = objAdaptor.FetchData(strQuerry);
                    dt = objAdaptor.FetchDeviceHourlyVals("Mem.PercentUsed", System.DateTime.Now, serverName);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dt;
        }

        public DataTable SetGraphForUsers(string paramGraph, string serverName)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    // string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Users' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    //string strQuerry = "select Servername,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //         "from [VSS_Statistics].[dbo].[DominoDailyStats] " +
                    //         "where Date >='11/10/2012' and Date <'11/11/2012' and StatName='Server.Users' and Servername='" + serverName + "'" +
                    //         "group by Servername,convert(varchar(5),Date,108)";
                    //dt = objAdaptor.FetchData(strQuerry);
                    dt = objAdaptor.FetchDeviceHourlyVals("Server.Users", System.DateTime.Now, serverName);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dt;
        }
        public DataTable GetGraphForMailBox(string str, string ColumnName)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "select server," + ColumnName + " from [VSS_Statistics].[dbo].[dailysummary] where server in (" + str + ")";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable SetBarGraphForTopMail()
        {
            try
            {
                DataTable dt = new DataTable();
                string StrQuery = "select top(20) Title,FileSize from [VSS_Statistics].[dbo].[Daily] where IsMailFile=1";
                dt = objAdaptor.FetchData(StrQuery);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable CheckedData()
        {
            try
            {
                DataTable dt = new DataTable();
                string strQuery = "select distinct S.servername from [VitalSigns].[dbo].[Servers] as S right outer join [VitalSigns].[dbo].[Dominoservers] AS S2 on S2.ServerID = S.ServerTypeID where S.servername !=null or S.servername!=''";
                dt = adaptor.FetchData(strQuery);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public DataTable SetGraphForTransactionPerMin(string servernamelist)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Trans.PerMinute' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName] IN (" + servernamelist + ") order by [ServerName] asc";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable SetGraphForCPU(string servernamelist)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Platform.System.PctCombinedCpuUtil' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName] IN (" + servernamelist + ") order by [ServerName] asc";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable SetGraphForMemory(string servernamelist)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mem.PercentUsed' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName] IN (" + servernamelist + ") order by [ServerName] asc";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //VSPLUS-1029, Durga 17Oct14 :Alerts history tab - grid should be ordered by Date/Time of Alert
        public DataTable GetAlertHistry(string Sname)
        {
            DataTable dt = new DataTable();
            try
            {//21/4/2016 Durga added for  VSPLUS-2872
                string SqlQuery = "Select * from AlertHistory where DeviceName='" + Sname + "' and (DateTimeOfAlert between DATEADD(day, -7, GetDate()) and GetDate() OR DateTimeAlertCleared IS NULL) order by DateTimeOfAlert desc";
                dt = adaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return dt;

        }

        public DataTable GetOutage(string Sname, string Stype)
        {
            DataTable Outage = new DataTable();
            try
            {
                //string sqlQuery = "Select DateTimeDown,DateTimeUp,Description,(select ServerName from Servers where ID in (ServerID)) as ServerName from [VSS_Statistics].[dbo].[Outages] where ServerID in (Select ID from Servers where ServerName='"+Sname+"')";
                //Mukund 22Oct13
                string sqlQuery = "Select o.serverid,o.ServerTypeID, DateTimeDown,DateTimeUp,datediff(mi,DateTimeDown,DateTimeUp) Description," +
                                    "'" + Sname + "' as ServerName from " +
                                    " [VSS_Statistics].[dbo].[Outages] o where o.ServerID in " +
                                    " (select id from Servers s where s.ServerName='" + Sname + "' " +
                                    " union select id from URLs s where Name='" + Sname + "' " +
                                    " union select [key] from MailServices s where Name='" + Sname + "' " +
                                    " union select id from [Network Devices] s where Name='" + Sname + "') " +
                                    " and o.ServerTypeID=(select id from ServerTypes where ServerType='" + Stype + "')";
                Outage = adaptor.FetchData(sqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return Outage;
        }

        //VSPLUS-480;Mukund 07Jul2014
        public DataTable GetBlackberry(string ServerName)
        {
            DataTable dt = new DataTable();

            try
            {
                string query = "select st.OperatingSystem,st.Status,st.PendingMail,st.LicensesUsed,st.SRPConnectionn,bb.BESVersion from Status st,BlackBerryServers bb,Servers sr  where bb.ServerID=sr.id and st.Name=sr.ServerName and sr.ServerName='" + ServerName + "'";
                dt = adaptor.FetchData(query);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Mukund 16Jul14, VSPLUS-824- Threshold in graph is not updating
        public DataTable ResponseThreshold(string ServerName)
        {
            DataTable dt = new DataTable();
            try
            {
                string que = "select ServerID,ResponseThreshold from Servers  s inner join DominoServers ds on ds.ServerID= s.ID where s.ServerName='" + ServerName + "'";
                dt = adaptor.FetchData(que);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //10/6/2015 NS added for VSPLUS-2252
        public DataTable GetSysInfoData(string ServerName, string ServerType)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT * FROM Status WHERE Name='" + ServerName + "' AND Type='" + ServerType + "' ";
                dt = adaptor.FetchData(query);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //10/9/2015 NS added for VSPLUS-2252
        public DataTable GetdataForTraveler(string Name)
        {
            try
            {
                DataTable dt = new DataTable();
                string strQuery = "select *from Status where SecondaryRole='Traveler' and Name='" + Name +"'";
                dt = adaptor.FetchData(strQuery);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetServerDetailsData(string ServerName)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                query = "SELECT t1.ID, t1.ServerID, ElapsedTimeSeconds, VersionArchitecture, CPUCount, ServerName, " +
                    "OperatingSystem, DominoVersion " +
                    "FROM DominoServerDetails t1 INNER JOIN Servers t2 ON " +
                    "t1.ServerID=t2.ID INNER JOIN ServerTypes t3 ON t2.ServerTypeID=t3.ID " +
                    "INNER JOIN Status t4 ON t2.ServerName=t4.Name AND t3.ServerType=t4.Type ";
                if (ServerName != "")
                {
                    query += "WHERE t2.ServerName='" + ServerName + "' ";
                }
                query += "ORDER BY t2.ServerName ";
                dt = adaptor.FetchData(query);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
