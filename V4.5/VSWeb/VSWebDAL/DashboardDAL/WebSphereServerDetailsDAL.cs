using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
    public class WebSphereServerDetailsDAL
    {
        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
        private Adaptor adaptor = new Adaptor();
        private static WebSphereServerDetailsDAL _self = new WebSphereServerDetailsDAL();

        public static WebSphereServerDetailsDAL Ins
        {
            get
            {
                return _self;
            }
        }

		public DataTable SetGraphheapsize(string paramGraph, string DeviceName)
		{
			DataTable dt = new DataTable();
			if (paramGraph == "hh")
			{
				try
				{
                    dt = objAdaptor.FetchWebSphereValsPerScan("CurrentHeapSize", System.DateTime.Now, DeviceName);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}


			return dt;
		}
		//public DataTable SetGraph(string paramGraph, string DeviceName)
		//{
		//    DataTable dt = new DataTable();
		//    if (paramGraph == "hh")
		//    {
		//        try
		//        {
		//            string strQuerry = "SELECT [DeviceName], [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";

		//            string strQuerry = "select devicename,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
		//                        " from [VSS_Statistics].[dbo].[DeviceDailyStats] " +
		//                        " where Date >= '11/10/2012' and StatName='ResponseTime' and [DeviceName]='" + DeviceName + "' " +
		//                        " group by devicename, convert(varchar(5),Date,108) ";

		//            dt = objAdaptor.FetchData(strQuerry);


		//            dt = objAdaptor.FetchSametimeHourlyVals("ResponseTime", System.DateTime.Now, DeviceName);



		//        }
		//        catch (Exception ex)
		//        {
		//            throw ex;
		//        }
		//    }


		//    return dt;
		//}

        public DataTable SetGraphForDiskSpace(string serverName)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT [DiskName], [DiskFree], [DiskSize]-[DiskFree] As DiskUsed FROM [DiskSpace] where [ServerName]='" + serverName + "' order by [DiskName]";
                dt = adaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable SetGraphForusedmemory(string paramGraph, string serverName)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    // string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[ExchangeDailyStats] where [StatName]='Platform.System.PctCombinedCpuUtil' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    //string strQuerry = "select Servername,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //"from [VSS_Statistics].[dbo].[ExchangeDailyStats] " +
                    //"where Date >='11/10/2012' and Date <'11/11/2012' and StatName='Platform.System.PctCombinedCpuUtil'  and Servername='" + serverName + "'" +
                    //"group by Servername,convert(varchar(5),Date,108) ";
                    //dt = objAdaptor.FetchData(strQuerry);
                    dt = objAdaptor.FetchWebSphereHourlyVals("Memory", System.DateTime.Now, serverName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dt;
        }

		public DataTable SetGraphForuptime(string paramGraph, string serverName)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    //string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[ExchangeDailyStats] where [StatName]='Mem.PercentUsed' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    //string strQuerry = "select Servername,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //          "from [VSS_Statistics].[dbo].[ExchangeDailyStats] " +
                    //          "where  Date >='11/10/2012' and Date <'11/11/2012' and StatName='Mem.PercentUsed'  and Servername='" + serverName + "'" +
                    //          "group by Servername,convert(varchar(5),Date,108)";
                    //dt = objAdaptor.FetchData(strQuerry);
                    dt = objAdaptor.FetchWebSphereHourlyVals("UpTime", System.DateTime.Now, serverName);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dt;
        }
        public DataTable SetGraphForProcessCpuUsage(string paramGraph, string serverName)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    //string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[ExchangeDailyStats] where [StatName]='Mem.PercentUsed' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    //string strQuerry = "select Servername,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //          "from [VSS_Statistics].[dbo].[ExchangeDailyStats] " +
                    //          "where  Date >='11/10/2012' and Date <'11/11/2012' and StatName='Mem.PercentUsed'  and Servername='" + serverName + "'" +
                    //          "group by Servername,convert(varchar(5),Date,108)";
                    //dt = objAdaptor.FetchData(strQuerry);
					dt = objAdaptor.FetchWebSphereValsPerScan("ProcessCpuUsage", System.DateTime.Now, serverName);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dt;
        }
        public DataTable SetGraphForActiveCount(string paramGraph, string serverName)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    //string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[ExchangeDailyStats] where [StatName]='Mem.PercentUsed' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    //string strQuerry = "select Servername,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //          "from [VSS_Statistics].[dbo].[ExchangeDailyStats] " +
                    //          "where  Date >='11/10/2012' and Date <'11/11/2012' and StatName='Mem.PercentUsed'  and Servername='" + serverName + "'" +
                    //          "group by Servername,convert(varchar(5),Date,108)";
                    //dt = objAdaptor.FetchData(strQuerry);
					dt = objAdaptor.FetchWebSphereValsPerScan("ActiveThreadCount", System.DateTime.Now, serverName);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dt;
        }
        public DataTable SetGraphForPoolSize(string paramGraph, string serverName)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    //string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[ExchangeDailyStats] where [StatName]='Mem.PercentUsed' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    //string strQuerry = "select Servername,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //          "from [VSS_Statistics].[dbo].[ExchangeDailyStats] " +
                    //          "where  Date >='11/10/2012' and Date <'11/11/2012' and StatName='Mem.PercentUsed'  and Servername='" + serverName + "'" +
                    //          "group by Servername,convert(varchar(5),Date,108)";
                    //dt = objAdaptor.FetchData(strQuerry);
                    // dt = objAdaptor.FetchDeviceHourlyVals("PoolSize", System.DateTime.Now, serverName);
					dt = objAdaptor.FetchWebSphereValsPerScan("AveragePoolSize", System.DateTime.Now, serverName);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dt;
        }

	
        public DataTable ResponseThreshold(string ServerName)
        {
            DataTable dt = new DataTable();
            try
            {
                string que = "select ServerID,ResponseTime as ResponseThreshold from Servers  s inner join ServerAttributes ds on ds.ServerID= s.ID where s.ServerName='" + ServerName + "'";
                dt = adaptor.FetchData(que);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


		public DataTable SetGraphfreeheapsize(string paramGraph, string DeviceName)
		{
			DataTable dt = new DataTable();
			if (paramGraph == "hh")
			{
				try
				{
					dt = objAdaptor.FetchWebSphereValsPerScan("MemoryFree", System.DateTime.Now, DeviceName);
					
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}


			return dt;

		}

		public DataTable SetGraphusedheapsize(string paramGraph, string DeviceName)
		{
			DataTable dt = new DataTable();
			if (paramGraph == "hh")
			{
				try
				{
					dt = objAdaptor.FetchWebSphereValsPerScan("MemoryUsed", System.DateTime.Now, DeviceName);
					
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}


			return dt;

		}

		public DataTable GetWebSphereServerDetails(string serverName)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = @"
select  wsn.NodeName, wsc.CellName, wsn.HostName, 
isnull(wssd.ProcessID, -1) as ProcessID,
isnull(wssd.UpTimeSeconds, 0) as UpTimeSeconds
from WebSphereServer wss 
left outer join WebSphereServerDetails wssd on wss.ServerID=wssd.ServerID
left outer join WebSphereNode wsn on wsn.NodeID=wss.NodeID
left outer join WebSphereCell wsc on wsc.CellID=wss.CellID where wss.ServerName='" + serverName + "'";
                dt = adaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

		public DataTable SetGraphForResponseTime(string serverName)
        {
			DataTable dt = new DataTable();

			try
			{
				dt = objAdaptor.FetchWebSphereValsPerScan("ResponseTime", System.DateTime.Now, serverName);

			}
			catch (Exception ex)
			{
				throw ex;
			}

			return dt;

        }
		
      
    }
}
