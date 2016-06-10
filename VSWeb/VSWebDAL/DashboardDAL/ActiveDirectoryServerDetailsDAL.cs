using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
    public class ActiveDirectoryServerDetailsDAL
    {
        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
        private Adaptor adaptor = new Adaptor();
        private static ActiveDirectoryServerDetailsDAL _self = new ActiveDirectoryServerDetailsDAL();

        public static ActiveDirectoryServerDetailsDAL Ins
        {
            get
            {
                return _self;
            }
        }

        public DataTable SetGraph(string paramGraph, string DeviceName, int ServerTypeId)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    dt = objAdaptor.FetchMicrosoftHourlyVals("ResponseTime", System.DateTime.Now, DeviceName, ServerTypeId);
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
                string strQuerry = "SELECT [DiskName], [DiskFree], [DiskSize]-[DiskFree] As DiskUsed FROM [DiskSpace] where [ServerName]='" + serverName + "' order by [DiskName]";
                dt = adaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
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
                    // string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[ExchangeDailyStats] where [StatName]='Platform.System.PctCombinedCpuUtil' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    //string strQuerry = "select Servername,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //"from [VSS_Statistics].[dbo].[ExchangeDailyStats] " +
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

		public DataTable SetGraphForMemory(string paramGraph, string serverName, int ServerTypeId)
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
                    dt = objAdaptor.FetchMicrosoftHourlyVals("Mem.PercentUsed", System.DateTime.Now, serverName, ServerTypeId);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dt;
        }

		public DataTable SetGraphForUsers(string paramGraph, string serverName, int ServerTypeId)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    // string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[ExchangeDailyStats] where [StatName]='Server.Users' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    //string strQuerry = "select Servername,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //         "from [VSS_Statistics].[dbo].[ExchangeDailyStats] " +
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

        public DataTable GetactivedirectoryMembers()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT sr.ID,sr.[ServerName],st.Status,AD.[ServerID],AD.[LogOnTest],AD.[QueryTest],AD.[LDAPPortTest],AD.[DNS],AD.[DomainController],AD.[ADMembersUP],AD.[ClusterNetwork],AD.[Advertising], AD.[FrsSysVol], AD.[Replications], AD.[Services], AD.[FsmoCheck],AD.LastScanDate  FROM [vitalsigns].[dbo].[ActiveDirectoryTest] AD , Servers sr ,status st where AD.ServerID=sr.ID and sr.[ServerName] = st.[Name]";
                dt = adaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetResponseTime()
        {
            //10/3/2013 NS modified


            DataTable statTab = new DataTable();
            try
            {
                //9/26/2013 NS modified the queries below - we'll try to allow a single type selection to limit the number of entries per page



                //string SqlQuery = "select LEFT(Name,20)+'/'+Type as Server,ResponseTime from Status where ResponseTime>0 and Type='" + sType + "' order by '" + parm + "' Asc";
                string SqlQuery = "select LEFT(Name,25) as Server,ResponseTime from Status where ResponseTime>=0 and Type='Active Directory'";
                statTab = adaptor.FetchData(SqlQuery);

            }
            catch (Exception)
            {
                throw;
            }
            return statTab;
        }

		public DataTable GetReplicationSumamryData(string serverName)
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = "select SourceServer, LargestDelta, Fails, DirectoryPartitions, LastScanTime from ActiveDirectoryReplicationSummary where ServerName = '" + serverName + "'";
				dt = adaptor.FetchData(SqlQuery);

			}
			catch (Exception)
			{
				throw;
			}
			return dt;
		}
    }
}
