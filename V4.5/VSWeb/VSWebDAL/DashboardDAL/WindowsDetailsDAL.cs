using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using VSWebDO;

namespace VSWebDAL.DashboardDAL
{
    public class WindowsDetailsDAL
    {


        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
        private Adaptor adaptor = new Adaptor();
        private static WindowsDetailsDAL _self = new WindowsDetailsDAL();

        public static WindowsDetailsDAL Ins
        {
            get
            {
                return _self;
            }
        }
        public DataTable SetGraphForCPUGeneric(string paramGraph, string serverName, int ServerTypeId)
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
        public DataTable SetGraphForMemoryGeneric(string paramGraph, string serverName, int ServerTypeId)
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
        public DataTable SetGraphForGenericEnabledUsers(string paramGraph, string serverName, int ServerTypeId)
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
					dt = objAdaptor.FetchMicrosoftHourlyVals("Server.Users", System.DateTime.Now, serverName, ServerTypeId);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
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
        public DataTable ResponseThresholdForGeneric(string ServerName)
        {
            DataTable dt = new DataTable();
            try
            {
                string que = "select ServerID,ResponseThreshold from Servers  s inner join WindowsServers ds on ds.ServerID= s.ID where s.ServerName='" + ServerName + "'";
                dt = adaptor.FetchData(que);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      

        public DataTable SetGridForGenericDisk(string ServerName)
        {
            DataTable dt = new DataTable();
            try
            {
                //string strQuerry = "SELECT * FROM [dbo].[DominoDiskSpace]  where diskfree is not null and disksize is not null ORDER BY [serverName],diskname";

                //05/02/2014 MD modified, VSPLUS-444
                //string strQuerry = "SELECT sp.ServerName,sp.DiskName,sp.DiskFree,sp.PercentFree,sp.PercentUtilization,sp.AverageQueueLength,sp.ID,sp.DiskSize,sp.Updated,st.Threshold, st.ThresholdType FROM [dbo].[DominoDiskSpace] sp, DominoDiskSettings st " +
                //                "where sp.servername=st.servername and " +
                //                "(st.DiskName='AllDisks' and sp.DiskName<>st.DiskName)  and  diskfree is not null and disksize is not null " +
                //                "union " +
                //                "SELECT sp.ServerName,sp.DiskName,sp.DiskFree,sp.PercentFree,sp.PercentUtilization,sp.AverageQueueLength,sp.ID,sp.DiskSize,sp.Updated,st.Threshold, st.ThresholdType FROM [dbo].[DominoDiskSpace] sp, DominoDiskSettings st " +
                //                "where sp.servername=st.servername and " +
                //                "(st.DiskName<>'NoAlerts' and st.DiskName<>'AllDisks' and sp.DiskName=st.DiskName)  and  diskfree is not null and disksize is not null " +
                //                "ORDER BY sp.serverName,sp.diskname";
                //7/11/2014 NS modified for VSPLUS-813
                /* 
                string strQuerry = "select DSP.ID,DSP.ServerName,DSP.DiskName,DSP.DiskSize,DSP.DiskFree,DSP.PercentFree,DSP.PercentUtilization,DSP.AverageQueueLength,DSP.Updated,DDS.Threshold,DDS.ThresholdType,DDS.DiskName  from V_DominoDiskSpace DSP LEFT OUTER JOIN DominoDiskSettings DDS " +
                    "ON (DDS.ServerName =DSP.ServerName and DSP.ServerName='" + ServerName + "') and (DSP.DiskName=DDS.DiskName or DDS.DiskName='AllDisks' ) Where  DSP.ServerName='" + ServerName + "' ORDER BY DSP.serverName,DSP.diskname";
                 */
                string strQuerry = "select DSP.ID,DSP.ServerName,DSP.DiskName,DSP.DiskSize,DSP.DiskFree,DSP.PercentFree,DSP.PercentUtilization,DSP.AverageQueueLength,DSP.Updated,CAST(DDS.Threshold as varchar(50)) + ' ' + CASE WHEN DDS.ThresholdType='Percent' THEN '%' ELSE DDS.ThresholdType END ThresholdDisp,DDS.Threshold,DDS.ThresholdType, DDS.DiskName  from DiskSpace DSP LEFT OUTER JOIN DiskSettings DDS " +
					"ON (DDS.ServerID =(select ID from Servers where ServerName='" + ServerName + "' and ServerTypeID=16) and DSP.ServerName='" + ServerName + "') and (DSP.DiskName=DDS.DiskName or DDS.DiskName='AllDisks' ) Where  DSP.ServerName='" + ServerName + "' ORDER BY DSP.serverName,DSP.diskname";
                dt = adaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
    }
}



