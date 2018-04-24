using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
    public class SharepointdetailsDAL
    {
        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
        private Adaptor adaptor = new Adaptor();
        private static SharepointdetailsDAL _self = new SharepointdetailsDAL();

        public static SharepointdetailsDAL Ins
        {
            get
            {
                return _self;
            }
        }
        public DataTable SetGraphForCPUsharepoint(string paramGraph, string serverName, int ServerTypeId)
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

        public DataTable GetSharePointServerDetails()
        {
            DataTable dt = new DataTable();

            try
            {
                //string SqlQuery = "select * from SharePointServerHealth";
                //7/22/2014 NS added OperatingSystem and DominoVersion to the list
                //10/9/2014 NS modified the query by adding Status to the QueryString
                //1/20/2015 NS modified the query by adding Status sort for VSPLUS-1377
                string SqlQuery = "select * from " +
                               "(Select Type,Name,Status,CASE WHEN Status='Not Responding' THEN 0 WHEN Status='Issue' THEN 1 ELSE 2 END StatusSort, " +
                               "ResponseTime,ContentMemory,OperatingSystem,DominoVersion, " +
                               "(CPU*100) as CPU, " +
                               "'''../images/icons/SharePoint_2013.jpg''' as imgsource, " +
                               "'Sharepointdetailspage.aspx?Name=' + Name + '&Type=' + Type + '&Status=' + Status + '&LastDate='+CONVERT(VARCHAR,LastUpdate,101) + ' ' + substring(convert(varchar(20), LastUpdate, 9), 13, 5) + ' ' + substring(convert(varchar(30), LastUpdate, 9), 25, 2)  as redirectto, " +
                               "isnull(Memory,0)*100 as MemoryPercent  " +
                               "from [VitalSigns].[dbo].[Status] st) as sts   " +
                               "inner join  " +
                               "(select * from (select * from ( " +
                               "select s.ID,s.ServerName,roles.rolename,sa.CPU_Threshold as CPUThreshold,sa.MemThreshold,sa.ResponseTime as ResponseThreshold, spf.Farm FROM Servers AS s  " +
                               "LEFT OUTER JOIN  ServerAttributes sa on sa.ServerID=s.ID   " +
                               "INNER JOIN  " +
                               "ServerTypes AS st ON st.ID = s.ServerTypeID  " +
                               "LEFT OUTER JOIN  " +
                               "(select ss.serverId, rm.id as roleId, rm.rolename from ServerRoles ss, RolesMaster rm  " +
                               " where rm.id = ss.RoleId) as roles on s.ID = roles.serverId " +
								"inner join (Select Main.ServerId, Left(Main.Students,Len(Main.Students)-1) As Farm From " +
								"(Select distinct ST2.ServerId, (Select ST1.Farm + ', ' AS [text()] " +
								"From dbo.SharePointFarms ST1 " +
								"Where ST1.ServerId = ST2.ServerId " +
								"ORDER BY ST1.ServerId " +
 								"For XML PATH ('') ) [Students] " +
								"From dbo.SharePointFarms ST2) [Main]) as spf on spf.ServerId=s.ID where st.ServerType='SharePoint') " +
                               " as exchangeservers  " +
                               ") as pr  " +
                               ") as exg   " +
                                "on exg.ServerName = sts.name and sts.Type = 'SharePoint' " +
                                "order by StatusSort";

                dt = adaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return dt;
        }
        public DataTable GetSharePointServerHealthRoles()
        {
            DataTable dt = new DataTable();

            try
            {
                //string SqlQuery = "select * from SharePointServerHealth";
                //7/22/2014 NS added OperatingSystem and DominoVersion to the list
                //10/9/2014 NS modified the query by adding Status to the QueryString
                string SqlQuery = "select s.ID,rm.RoleName,rm.ServerTypeId ,st.ServerType from ServerRoles sr inner join RolesMaster rm on rm.Id=sr.RoleId inner join Servers s on s.ServerTypeID=rm.ServerTypeId inner join ServerTypes st on st.ID=rm.ServerTypeId inner join Status sts on sts.Name=s.ServerName and sr.ServerId=s.ID  where  st.ServerType='SharePoint'  ";

                dt = adaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return dt;
        }
        public DataTable SetGraphForMemorysharepoint(string paramGraph, string serverName, int ServerTypeId)
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
        public DataTable SetGraphForsharepointEnabledUsers(string paramGraph, string serverName, int ServerTypeId)
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
        public DataTable ResponseThresholdForsharepoint(string ServerName)
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

        public DataTable SetGridForsharepointDisk(string ServerName)
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
                    "ON (DDS.ServerID =DSP.ServerName and DSP.ServerName='" + ServerName + "') and (DSP.DiskName=DDS.DiskName or DDS.DiskName='AllDisks' ) Where  DSP.ServerName='" + ServerName + "' ORDER BY DSP.serverName,DSP.diskname";
                dt = adaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
		public DataTable sharepointtab(string servername)
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
				string strQuerry = "select ws.ServerID,ws.Sname,ws.Svalue from servers s inner join WindowsStatus ws on s.Id=ws.ServerID and s.ServerName='" + servername + "'";
				dt = adaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		public DataTable GetDatabaseServerDetails()
		{
			DataTable dt = new DataTable();
			try
			{
				
				string strQuerry = "select ID,WebAppName, DatabaseName, DatabaseSiteCount, MaxSiteCountThreshold, WarningSiteCountThreshold, DatabaseServer, " +
									"(Select 'Sharepointdetailspage.aspx?Name=' + Name + '&Type=' + Type + '&Status=' + Status + '&LastDate='+CONVERT(VARCHAR,LastUpdate,101) + ' ' + substring(convert(varchar(20), LastUpdate, 9), 13, 5) + ' ' + substring(convert(varchar(30), LastUpdate, 9), 25, 2)  as redirectto " +
									"from [VitalSigns].[dbo].[Status] st where DatabaseServer = st.Name and st.Type = 'SharePoint') as redirectto " +
									"from SharePointDatabaseDetails";
				dt = adaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		public DataTable GetFarmDetails()
		{
			DataTable dt = new DataTable();
			try
			{

				string strQuerry = "select spf.Farm, case spfh.LogOnTest WHEN 'True' THEN 'Pass' WHEN 'False' THEN 'Fail' else '' end as LogOnTest, " + 
									"case spfh.UploadTest WHEN 'True' THEN 'Pass' WHEN 'False' THEN 'Fail' else '' end as UploadTest, " +
									"case spfh.SiteCollectionTest WHEN 'True' THEN 'Pass' WHEN 'False' THEN 'Fail' else '' end as SiteCollectionTest " +
									"from (select distinct Farm from sharepointfarms where Farm not like '%,%') as spf " +
									"left outer join SharePointFarmHealth spfh on spfh.FarmName=spf.Farm";
				dt = adaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		public DataTable GetFarmNames()
		{
			DataTable dt = new DataTable();
			try
			{

				string strQuerry = "select distinct Farm from SharePointFarms order by Farm";
				dt = adaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		public DataTable GetSiteCollectionSize(string Farm)
		{

			DataTable dt = new DataTable();
			try
			{
                //4/14/2016 Sowjanya modified for VSPLUS-2853
                string strQuerry = "select ID, SiteCollection, SizeMB,Owner,SiteCount from SharePointSiteCollections where FarmName='" + Farm + "' order by SizeMB desc";
				dt = adaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
        //18-04-2016 Durga Modified for VSPLUS-2851
        public DataTable GetSharePointTimerJobsDetails()
        {
            DataTable dt = new DataTable();
            try
            {

                string strQuerry = "select * from SharePointTimerJobs";
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

