using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
    public class LyncDetailsDAL
    {


        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
        private Adaptor adaptor = new Adaptor();
        private static LyncDetailsDAL _self = new LyncDetailsDAL();

        public static LyncDetailsDAL Ins
        {
            get
            {
                return _self;
            }
        }
        public DataTable SetGraphForLyncEnabledUsers(string paramGraph, string serverName, int ServerTypeId)
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
					dt = objAdaptor.FetchMicrosoftHourlyVals("Lync@LyncEnabledUsers", System.DateTime.Now, serverName, ServerTypeId);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }

        public DataTable SetGraphforLyncUsersConnected(string paramGraph, string serverName, int ServerTypeId)
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
					dt = objAdaptor.FetchMicrosoftHourlyVals("Lync@UsersConnected", System.DateTime.Now, serverName, ServerTypeId);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }
        public DataTable SetGraphforLyncVoiceenabled(string paramGraph, string serverName, int ServerTypeId)
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
					dt = objAdaptor.FetchMicrosoftHourlyVals("Lync@VoiceEnabledUsers", System.DateTime.Now, serverName, ServerTypeId);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }
        public DataTable SetGraphforLyncChatLatency(string paramGraph, string serverName, int ServerTypeId)
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
					dt = objAdaptor.FetchMicrosoftHourlyVals("Lync@ChatLatency", System.DateTime.Now, serverName, ServerTypeId);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }

        public DataTable SetGraphforLyncGroupChatLatency(string paramGraph, string serverName, int ServerTypeId)
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
					dt = objAdaptor.FetchMicrosoftHourlyVals("Lync@GroupChatLatency", System.DateTime.Now, serverName, ServerTypeId);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }
        //public DataTable SetGraphForchatstatus(string serverName)
        public DataTable SetGraphForchatstatus(string paramGraph, string serverName)

        {
            DataTable dt = new DataTable();
            try
            {
                //7/14/2014 NS modified for VSPUS-813
                //string strQuerry = "SELECT [DiskName], [DiskFree], [DiskSize]-[DiskFree] As DiskUsed FROM [DominoDiskSpace] where [ServerName]='" + serverName + "' AND DiskName='" + DiskName + "' order by [DiskName]";
                string strQuerry = "";
                //7/14/2014 NS modified for VSPUS-816
                //8/5/2014 NS modified


                strQuerry = "SELECT  [StatName],Date,[StatValue]" +
				" FROM [VSS_Statistics].[dbo].[MicrosoftDailyStats] where [StatName]='Lync@UsersConnected' " +
                "Union " + "SELECT  [StatName],Date,[StatValue] " +
				" FROM [VSS_Statistics].[dbo].[MicrosoftDailyStats] where [StatName]='Lync@LyncEnabledUsers' " + "Union " + "SELECT  [StatName],Date,[StatValue] " +
				" FROM [VSS_Statistics].[dbo].[MicrosoftDailyStats] where [StatName]='Lync@VoiceEnabledUsers' " +
                "";

                dt = objAdaptor.FetchchatData(strQuerry);
            }




            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }


        public DataTable SetGridForLyncDisk(string ServerName)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "select DSP.ID,DSP.ServerName,DSP.DiskName,DSP.DiskSize,DSP.DiskFree,DSP.PercentFree,DSP.PercentUtilization,DSP.AverageQueueLength,DSP.Updated,CAST(DDS.Threshold as varchar(50)) + ' ' + CASE WHEN DDS.ThresholdType='Percent' THEN '%' ELSE DDS.ThresholdType END ThresholdDisp,DDS.Threshold,DDS.ThresholdType, DDS.DiskName  from DiskSpace DSP LEFT OUTER JOIN DiskSettings DDS " +
                    "ON (DDS.ServerID =(select ID from Servers where ServerName= DSP.ServerName) and DSP.ServerName='" + ServerName + "') and (DSP.DiskName=DDS.DiskName or DDS.DiskName='AllDisks' ) Where  DSP.ServerName='" + ServerName + "' ORDER BY DSP.serverName,DSP.diskname";
                dt = adaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable SetGraphforperformance(string paramGraph, string DeviceName, int ServerTypeId)
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


					dt = objAdaptor.FetchMicrosoftHourlyVals("ResponseTime", System.DateTime.Now, DeviceName, ServerTypeId);



                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            return dt;
        }
        public DataTable ResponseThresholdForLync(string ServerName)
        {
            DataTable dt = new DataTable();
            try
            {
                string que = "select ServerID,ResponseThreshold from Servers  s inner join LyncServers ds on ds.ServerID= s.ID where s.ServerName='" + ServerName + "'";
                dt = adaptor.FetchData(que);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable SetGraphForMemory(string paramGraph, string serverName, int ServerTypeId)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
					dt = objAdaptor.FetchMicrosoftHourlyVals("Mem.PercentUsed", System.DateTime.Now, serverName, ServerTypeId);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dt;
        }


        public DataTable SetGraphForCPU(string paramGraph, string serverName, int ServerTypeId)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
					dt = objAdaptor.FetchMicrosoftHourlyVals("Platform.System.PctCombinedCpuUtil", System.DateTime.Now, serverName, ServerTypeId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dt;
        }
        public DataTable SetGraphForCPULync(string paramGraph, string serverName, int ServerTypeId)
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
					dt = objAdaptor.FetchMicrosoftHourlyVals("Platform.System.PctCombinedCpuUtil", System.DateTime.Now, serverName, ServerTypeId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dt;
        }
        public DataTable SetGraphForMemoryLync(string paramGraph, string serverName, int ServerTypeId)
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
					dt = objAdaptor.FetchMicrosoftHourlyVals("Mem.PercentUsed", System.DateTime.Now, serverName, ServerTypeId);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dt;
        }


    }
}
