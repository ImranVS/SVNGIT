using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
    public class ExchangeServerDetailsDAL
    {
        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
        private Adaptor adaptor = new Adaptor();
        private static ExchangeServerDetailsDAL _self = new ExchangeServerDetailsDAL();

        public static ExchangeServerDetailsDAL Ins
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
		public DataTable SetGraphforExchange(string paramGraph, string serverName, int ServerTypeId)
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


                    dt = objAdaptor.FetchMicrosoftHourlyVals("DeliveryTime.Seconds", System.DateTime.Now, serverName, ServerTypeId);



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

		public DataTable SetBarGraphForTopMail(string serverType,string graphType)
		{
			try
			{
				DataTable dt = new DataTable();
				string StrQuery = "";
				if (serverType == "All Servers" || serverType == "")
				{
					if (graphType=="FileSize")
						StrQuery = "select distinct top(20) DisplayName + ' (' + Server + ')' Title, ItemCount from [VSS_Statistics].[dbo].[ExchangeMailfiles] order by ItemCount desc";
					else
						StrQuery = "select distinct top(20) DisplayName + ' (' + Server + ')' Title, TotalItemSizeInMB  from [VSS_Statistics].[dbo].[ExchangeMailfiles] order by TotalItemSizeInMB desc";
				}
				else
				{
					if (graphType == "FileSize")
						StrQuery = "select distinct top(20) DisplayName Title, ItemCount from [VSS_Statistics].[dbo].[ExchangeMailfiles]  Where Server = '" + serverType + "' order by ItemCount desc";
					else
						StrQuery = "select distinct top(20) DisplayName Title, TotalItemSizeInMB from [VSS_Statistics].[dbo].[ExchangeMailfiles]  Where Server = '" + serverType + "' order by TotalItemSizeInMB desc";
				}
				dt = objAdaptor.FetchData(StrQuery);
				return dt;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public DataTable SetBarGraphForTopMailDatabase(string databaseName, string graphType)
		{
			try
			{
				DataTable dt = new DataTable();
				string StrQuery = "";
				if (databaseName == "All Databases" || databaseName == "")
				{
					if (graphType == "FileSize")
						StrQuery = "select distinct top(20) DisplayName + ' (' + [Database] + ')' Title, ItemCount from [VSS_Statistics].[dbo].[ExchangeMailfiles] order by ItemCount desc";
					else
						StrQuery = "select distinct top(20) DisplayName + ' (' + [Database] + ')' Title, TotalItemSizeInMB  from [VSS_Statistics].[dbo].[ExchangeMailfiles] order by TotalItemSizeInMB desc";
				}
				else
				{
					if (graphType == "FileSize")
						StrQuery = "select distinct top(20) DisplayName Title, ItemCount from [VSS_Statistics].[dbo].[ExchangeMailfiles]  Where [Database] = '" + databaseName + "' order by ItemCount desc";
					else
						StrQuery = "select distinct top(20) DisplayName Title, TotalItemSizeInMB from [VSS_Statistics].[dbo].[ExchangeMailfiles]  Where [Database] = '" + databaseName + "' order by TotalItemSizeInMB desc";
				}
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
				string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[MicrosoftDailyStats] where [StatName]='Server.Trans.PerMinute' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName] IN (" + servernamelist + ") order by [ServerName] asc";
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
				string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[MicrosoftDailyStats] where [StatName]='Platform.System.PctCombinedCpuUtil' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName] IN (" + servernamelist + ") order by [ServerName] asc";
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
				string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[MicrosoftDailyStats] where [StatName]='Mem.PercentUsed' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName] IN (" + servernamelist + ") order by [ServerName] asc";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }


        public DataTable GetAlertHistry(string Sname)
        {
            DataTable dt = new DataTable();
            try
            {
				string SqlQuery = "Select * from AlertHistory where DeviceName='" + Sname + "' and (DateTimeOfAlert between DATEADD(day, -7, GetDate()) and GetDate() OR DateTimeAlertCleared IS NULL)";
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

        public DataTable GetMoniteredServices(string ServerName,string Monitored)
        {
            DataTable Charttab = new DataTable();
            try
            {
                string SqlQuery = "Select * from WindowsServices where ServerName='" + ServerName + "' and Monitored=" + Monitored;
                Charttab = adaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return Charttab;
        }
       

        public DataTable GetExchangeServerHealth(string ServerName)
        {
            DataTable Exchangetab = new DataTable();
            try
            {
                string SqlQuery = "Select * from ExchangeServerHealth where ServerName='" + ServerName + "'";
                Exchangetab = adaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return Exchangetab;
        }
        public DataTable GetExchangeMailBoxReport(string ServerName)
        {
            DataTable Exchangetab = new DataTable();
            try
            {
                string SqlQuery = "Select * from ExchangeMailBoxReport where ServerName='" + ServerName + "'";
                Exchangetab = adaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return Exchangetab;
        }

		public DataTable SetGraphOutlookWebApp(string paramGraph, string DeviceName, int ServerTypeId)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    dt = objAdaptor.FetchMicrosoftHourlyVals("OutlookWebApp", System.DateTime.Now, DeviceName, ServerTypeId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            return dt;
        }
		public DataTable SetGraphRPCClientAccess(string paramGraph, string DeviceName, int ServerTypeId)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    dt = objAdaptor.FetchMicrosoftHourlyVals("RPCClientAccess", System.DateTime.Now, DeviceName, ServerTypeId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            return dt;
        }

        public DataTable GetExDAGHealthCopyStatus(string ServerName)
        {
            DataTable Exchangetab = new DataTable();
            try
            {
                string SqlQuery = "Select * from ExchangeDAGHealthCopyStatus where ServerName='" + ServerName + "'";
                Exchangetab = adaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return Exchangetab;
        }

        public DataTable GetExDAGHealthCopySummary(string ServerName)
        {
            DataTable Exchangetab = new DataTable();
            try
            {
                string SqlQuery = "Select * from ExchangeDAGHealthCopySummary where ServerName='" + ServerName + "'";
                Exchangetab = adaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return Exchangetab;
        }

        public DataTable GetExDAGHealthMemberReport(string ServerName)
        {
            DataTable Exchangetab = new DataTable();
            try
            {
                string SqlQuery = "Select * from ExchangeDAGHealthMemberReport where ServerName='" + ServerName + "'";
                Exchangetab = adaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return Exchangetab;
        }

        public DataTable GetExQueuesStatus(string ServerName)
        {
            DataTable Exchangetab = new DataTable();
            try
            {
                string SqlQuery = "Select * from ExchangeQueues where ServerName='" + ServerName + "'";
                Exchangetab = adaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return Exchangetab;
        }



         
        //new exchange tables. Mukund 13Mar14
        public DataTable GetWindowsServices(string servername)
        {
            DataTable ServicesDataTable = new DataTable();

            try
            {
                //string SqlQuery = "SELECT distinct  svr.id, case when ss.ServerId is NULL then 'False' else 'True' end Monitored,ss.ServerId,  rm.ServerTypeId, sm.ServiceId, sm.ServiceName, sm.ServiceShortName, sm.SecurityContext, sm.ServiceDescription, sm.DefaultStartupType, sm.Required,  sm.Custom, rm.RoleName, svr.VersionNo,sd.Result " +
                //                    "FROM vitalsigns.dbo.ServiceMaster AS sm  " +
                //                    "INNER JOIN vitalsigns.dbo.ServiceVersionRole AS svr ON sm.ServiceId = svr.ServiceId  " +
                //                    "INNER JOIN vitalsigns.dbo.RolesMaster AS rm ON svr.RoleId = rm.Id	  " +
                //                    "inner join vitalsigns.dbo.ServerServices ss on svr.Id=ss.SVRId   " +
                //                    "inner join vitalsigns.dbo.Servers ser on ss.ServerId=ser.id and ser.ServerName='" + servername + "' " +
                //                    "inner join vitalsigns.dbo.Status st on st.Name=ser.ServerName  " +
                //                    "inner join vitalsigns.dbo.StatusDetail sd on sd.TypeAndName= st.TypeANDName and sd.Category=rm.RoleName  and sd.TestType='Services'";

                //4/1/2014 NS commented out the original query for the MEC show only!!!!
                /*string SqlQuery = "select sd.ID,rm.RoleName, sm.ServiceName,sd.Result,sm.Required" +
                                " from vitalsigns.dbo.ServiceMaster AS sm, vitalsigns.dbo.StatusDetail sd  ,vitalsigns.dbo.Servers ser, vitalsigns.dbo.ServerServices ss,vitalsigns.dbo.ServiceVersionRole svr, vitalsigns.dbo.RolesMaster AS rm" +
                                " where sm.ServiceName=sd.TestName" +
                                " and sd.Category='Services' and sd.TypeAndName='" + servername + "-Exchange' and ser.ServerName+'-Exchange'=sd.TypeAndName" +
                                " and ss.ServerId=ser.ID and ss.SVRId=svr.Id and rm.Id=svr.RoleId";
                 */
                
                //6/13/2014 Welsey Stanulis
                //string SqlQuery = "SELECT TOP 30 ID,'CAS' RoleName,[DisplayName] ServiceName,[Status] Result,'R' Required FROM [vitalsigns].[dbo].[WindowsServices]";

                //string SqlQuery = "SELECT distinct rm.RoleName, sm.ServiceName, sm.Required, svr.Id as ID, sd.Result " +
                //                    "FROM Servers s " +
                //                    "inner join ServerServices ss on s.ID = ss.ServerId " +
                //                    "inner join ServiceVersionRole svr on ss.SVRId = svr.Id " +
                //                    "inner join ServiceMaster sm on svr.ServiceId = sm.ServiceId " +
                //                    "inner join RolesMaster rm on rm.id=svr.RoleId " +
                //                    "inner join vitalsigns.dbo.Status st on st.Name=s.ServerName " +
                //                    "inner join vitalsigns.dbo.StatusDetail sd on sd.TypeAndName= st.TypeANDName " +
                //                    "where ServerName='" + servername + "' " +
                //                    "ORDER BY rm.RoleName";

                //string SqlQuery = " SELECT sm.ServiceId as ID,'' as role, sm.ServiceName,[Status] as Result ,[StartMode],sm.Required,DateStamp  FROM WindowsServices ws, ServiceMaster sm  where ws.Service_Name=sm.ServiceShortName and ws.ServerName = '" + servername + "'";

				string SqlQuery = "Select ID, DisplayName as ServiceName, [Status] as Result, [StartMode], DateStamp, 'Type' as 'Monitored' FROM WindowsServices WHERE ServerName = '" + servername + "' AND Monitored=1";


				SqlQuery = "Select ID, DisplayName as ServiceName, [Status] as Result, [StartMode], DateStamp, 'Type' = case WHEN ServerRequired='1' THEN 'Required' else 'Monitored' end FROM WindowsServices WHERE ServerName = '" + servername + "' AND Monitored=1";


				ServicesDataTable = adaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return ServicesDataTable;
        }


        public DataTable GetUnmonitoredWindowsServices(string servername)
        {
            DataTable ServicesDataTable = new DataTable();

            try
            {
                //string SqlQuery = "select rm.RoleName, sm.ServiceName, sm.Required, svr.Id as ID,svr.VersionNo from ServiceVersionRole svr, ServiceMaster sm, RolesMaster rm where  rm.id=svr.RoleId and svr.ServiceId = sm.ServiceId "+
                //                   " and sm.ServiceName not in "+
                //                   "(SELECT distinct sm.ServiceName FROM Servers s inner join ServerServices ss on s.ID = ss.ServerId inner join ServiceVersionRole svr on ss.SVRId = svr.Id inner join ServiceMaster sm on svr.ServiceId = sm.ServiceId inner join RolesMaster rm on rm.id=svr.RoleId "+
                //                   " inner join vitalsigns.dbo.Status st on st.Name=s.ServerName inner join vitalsigns.dbo.StatusDetail sd on sd.TypeAndName= st.TypeANDName where "+
                //                   " ServerName='" + servername + "' )";
                //string SqlQuery = " select sm.ServiceId as ID,'' as role, sm.ServiceName,sm.DefaultStartupType,sm.Required from ServiceMaster sm where ServiceId not in " +
                //                    "(SELECT distinct sm.ServiceId FROM WindowsServices ws, ServiceMaster sm  where ws.Service_Name=sm.ServiceShortName and ws.ServerName = '" + servername + "')";
                string SqlQuery = "select ID,DisplayName as ServiceName,[Status] as Result,[StartMode],DateStamp from  WindowsServices where ServerName =  '" + servername + "'";
                ServicesDataTable = adaptor.FetchData(SqlQuery);
                //case [Status] when 'Running' then 'Pass' else 'Fail' end Result
            }
            catch
            {
            }
            finally
            {
            }
            return ServicesDataTable;
        }

        public DataTable GetExMailHealthStatus(string servername)
        {
            DataTable CASStatusDataTable = new DataTable();

            try
            {

                string SqlQuery = "select * FROM [vitalsigns].[dbo].ExgMailHealth  where ServerName= '" + servername + "'";

                CASStatusDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return CASStatusDataTable;
        }
       

        public DataTable GetExMailHealth(string servername)
        {
            DataTable CASStatusDataTable = new DataTable();

            try
            {
                //19/4/2016 sowmya added for VSPLUS-2849
                string SqlQuery = "select distinct eh.*,ed.WhiteSpaceThreshold,ed.DatabaseSizeThreshold from ExgMailHealthDetails eh inner join ExchangeDatabaseSettings ed on eh.DatabaseName=ed.DatabaseName  where eh.ServerName='" + servername + "' ";

                CASStatusDataTable = adaptor.FetchData(SqlQuery);
                 
            }
            catch
            {
            }
            finally
            {
            }
            return CASStatusDataTable;
        }
       
        public DataTable GetStatusDetails(string servername, string Category)
        {
            DataTable CASStatusDataTable = new DataTable();
            try
            {
                //string SqlQuery = "select * FROM [vitalsigns].[dbo].StatusDetail where TestType='" + TestType + "' and TypeAndName= '" + servername + "-Exchange'";
                string SqlQuery = "select * FROM [vitalsigns].[dbo].StatusDetail where Category='" + Category + "' and TypeAndName= '" + servername + "-Exchange'";
                CASStatusDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return CASStatusDataTable;
        }

        public DataTable SetExGraph(string servername, string Statname)
        {
            DataTable dt = new DataTable();
            try
            {
               // string strQuerry = "SELECT Date, [StatValue], " +
               //" SubCategory FROM [VSS_Statistics].[dbo].[ExchangeDailyStats] where [StatName]='" + Statname + "' " +
               //"and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE()) and [ServerName] = '" + servername + "' and  Category='" + Category + "'" +
               //" order by ServerName,Date";
				string strQuerry = "SELECT Date, CASE WHEN IsNumeric([StatValue]) = 1 and [StatValue] > 2147483640 THEN 2147483640 ELSE [StatValue] " +
					"END as 'StatValue' ,SUBSTRING(StatName,CHARINDEX('@',StatName)+1, CHARINDEX('#',StatName)-CHARINDEX('@',StatName)-1) SubCategory FROM [VSS_Statistics].[dbo].[MicrosoftDailyStats] where [StatName] like'" + Statname + "' and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE()) and [ServerName] ='" + servername + "' " +
					" order by ServerName,Date";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

		public DataTable SetMailGraph(string servername)
		{
			DataTable dt = new DataTable();
			try
			{
				// string strQuerry = "SELECT Date, [StatValue], " +
				//" SubCategory FROM [VSS_Statistics].[dbo].[ExchangeDailyStats] where [StatName]='" + Statname + "' " +
				//"and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE()) and [ServerName] = '" + servername + "' and  Category='" + Category + "'" +
				//" order by ServerName,Date";

				string strQuerry = "select DateAdd(hour, Hour, CAST(Date as DateTime)) as Date, tbl.StatValue, tbl.SubCategory from ( " +
									"SELECT Cast(Date as Date) as Date, DATEPART(HOUR, [Date]) as Hour, Round(AVG(StatValue), 0) as StatValue " +
									", SUBSTRING(StatName,CHARINDEX('@',StatName)+1, CHARINDEX('#',StatName)-CHARINDEX('@',StatName)-1) as SubCategory  " +
									"FROM [VSS_Statistics].[dbo].[MicrosoftDailyStats]  " +
									"where [StatName] like '%@%#Queues' and DATEDIFF(dd,0,Date) =  " +
									"DATEDIFF (dd,0,GETDATE()) and " +
									"[ServerName] ='" + servername + "'   " +
									"GROUP BY Cast(Date as Date), DATEPART(HOUR, [Date]), " +
									"SUBSTRING(StatName,CHARINDEX('@',StatName)+1, CHARINDEX('#',StatName)-CHARINDEX('@',StatName)-1) " +
									") as tbl";


				//string strQuerry = "SELECT Date, Round(StatValue,0) as StatValue,SUBSTRING(StatName,CHARINDEX('@',StatName)+1, CHARINDEX('#',StatName)-CHARINDEX('@',StatName)-1) SubCategory FROM [VSS_Statistics].[dbo].[MicrosoftDailyStats] where [StatName] like'" + Statname + "' and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE()) and [ServerName] ='" + servername + "' " +
				//" order by ServerName,Date";
				dt = objAdaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

        //2/13/2015 NS added for VSPLUS-1358
        public DataTable SetMailDeliveryRateGraph(string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT Date,StatValue FROM [MicrosoftDailyStats] " +
                    "WHERE ServerName='" + servername + "' AND StatName='Mail_DeliverySuccessRate'";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //2/13/2015 NS added for VSPLUS-1358
        public DataTable SetMailSizeGraph(string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT * " +
                    "FROM( " +
                    "SELECT DATE,StatValue,StatName FROM MicrosoftDailyStats " +
                    "WHERE StatName IN ('Mail_ReceivedSizeMB','Mail_SentSizeMB') AND ServerName='" + servername + "' " +
                    ") AS t " +
                    "PIVOT " +
                    "(SUM(StatValue) " +
                    "FOR StatName IN ([Mail_ReceivedSizeMB],[Mail_SentSizeMB]) " + 
                    ") AS p";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //2/13/2015 NS added for VSPLUS-1358
        public DataTable SetMailCountGraph(string servername)
        {
            //4/8/2016 Sowjanya modified for VSPLUS-2766
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT * " +
                    "FROM( " +
                    "SELECT DATE,StatValue,StatName FROM MicrosoftDailyStats " +
                    "WHERE StatName IN ('Mail_ReceivedCount','Mail_SentCount') AND ServerName='" + servername + "' " +
                    ") AS t " +
                    "PIVOT " +
                    "(SUM(StatValue) " +
                    "FOR StatName IN ([Mail_ReceivedCount],[Mail_SentCount]) " +
                    ") AS p";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable SetExGraphSingle(string servername, string Statname)
        {
            DataTable dt = new DataTable();
            try
            {
                // string strQuerry = "SELECT Date, [StatValue], " +
                //" SubCategory FROM [VSS_Statistics].[dbo].[ExchangeDailyStats] where [StatName]='" + Statname + "' " +
                //"and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE()) and [ServerName] = '" + servername + "' and  Category='" + Category + "'" +
                //" order by ServerName,Date";
				string strQuerry = "SELECT Date, [StatValue],Statname SubCategory FROM [VSS_Statistics].[dbo].[MicrosoftDailyStats] where [StatName] ='" + Statname + "' and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE()) and [ServerName] ='" + servername + "' " +
              " order by ServerName,Date";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //DAG related
        public DataTable GetDAGStatus(string DAGID)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "select * from [vitalsigns].[dbo].[DAGStatus]";
                //7/23/2014 NS modified
                if (DAGID != "")
                {
                    strQuerry += " WHERE ID=" + DAGID;
                }
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetDAGMembers(int DAGID)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT dm.ID,dm.[ServerName],dm.[DAGID],dm.[ClusterService],dm.[RelayService],dm.[ActiveMgr],dm.[TasksRPCListener],dm.[TPCListner],dm.[DAGMembersUP],dm.[ClusterNetwork],dm.[QuorumGroup],dm.[FileShareQuorum],dm.[DBCopySuspend],dm.[DBDisconnected],dm.[DBLogCopyKeepingUP],dm.[DBLogReplayKeepingUP]  FROM [vitalsigns].[dbo].[DAGMembers] dm where dm.DAGID="+DAGID.ToString();
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetDAGDB(int DAGID)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT db.ID,dm.ServerName,db.[ID],db.[DAGMemberId],db.[DatabaseName],db.[Activation Preference],db.[CopyQueue],db.[ReplayQueue],db.[ReplayLagged],db.[TruncationLagged],db.[ContendIndex]   FROM [vitalsigns].[dbo].[DAGMembers] dm,[vitalsigns].[dbo].[DAGDatabase] db where dm.ID=db.DAGMemberId and dm.DAGID=" + DAGID.ToString();
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        //Mukund 16Jul14, VSPLUS-824- Threshold in graph is not updating
        public DataTable ResponseThreshold(string ServerName)
        {
            DataTable dt = new DataTable();
            try
            {
                string que = "select ServerID,ResponseThreshold from Servers  s inner join MSServerSettings ms on ms.ServerID= s.ID where s.ServerName='" + ServerName + "'";
                dt = adaptor.FetchData(que);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public DataTable getMailInformation(string ServerName)
        {
            DataTable dt = new DataTable();
            try
            {
				string que = "select top 3 SUBSTRING(StatName, CHARINDEX('@', StatName) + 1" +
								", CHARINDEX('#',StatName) - CHARINDEX('@', StatName) - 1) as StatName" +
								", StatValue from [MicrosoftDailyStats] where ServerName='" + ServerName + "' and statname like '%@%#Queues' order by date desc";
				dt = objAdaptor.FetchData(que);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // by Sampath
        public DataTable GetHealthAssessmentStatusDetails(string TypeAndName)
        {
            DataTable HAStatus = new DataTable();
            try
            {
                //string SqlQuery = "select * FROM [vitalsigns].[dbo].StatusDetail where TestType='" + TestType + "' and TypeAndName= '" + servername + "-Exchange'";
                string SqlQuery = "select *,'../images/icons/dominoserver.gif' as imgsource FROM [vitalsigns].[dbo].StatusDetail where TypeAndName= '" + TypeAndName + "' and (testname  Not like '%Log%' or testname  like '%Login%' or testname  like '%blog%' ) ";
                
                HAStatus = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return HAStatus;
        }

        //7/24/2014 NS added
        public DataTable GetDAGActivationPreference(string DAGID)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                //Original query below in case you need to copy/paste into SSMS; DAGID is a variable value
                /*
                    DECLARE @T AS TABLE(y int NOT NULL PRIMARY KEY);

                    DECLARE
                    @cols AS NVARCHAR(MAX),
                    @y    AS int,
                    @sql  AS NVARCHAR(MAX);

                    -- Construct the column list for the IN clause
                    -- e.g., [1],[2],[3]
                    SET @cols = STUFF(
                    (SELECT N',' + QUOTENAME(y) AS [text()]
                    FROM (SELECT DISTINCT [Activation Preference] AS y FROM DAGDatabase  dd
                    INNER JOIN DAGMembers dm on dd.DAGMemberId=dm.ID
                    WHERE dm.DAGID = 14) AS Y
                    ORDER BY y
                    FOR XML PATH('')),
                    1, 1, N'');

                    -- Construct the full T-SQL statement
                    -- and execute dynamically
                    SET @sql = N'SELECT *
                    FROM (select DatabaseName, [Activation Preference], ServerName
                    from DAGDatabase dd inner join DAGMembers dm on dd.DAGMemberId=dm.ID
                    where dm.DAGID = 14
                    ) AS D
                    PIVOT(max(ServerName) FOR [Activation Preference] IN(' + @cols + N')) AS P;';

                    EXEC sp_executesql @sql;
                    GO
                 */
                query = "DECLARE @T AS TABLE(y int NOT NULL PRIMARY KEY); " +
                    "DECLARE " +
                    "@cols AS NVARCHAR(MAX)," +
                    "@y    AS int," +
                    "@sql  AS NVARCHAR(MAX); " +
                    "SET @cols = STUFF(" +
                    "(SELECT N',' + QUOTENAME(y) AS [text()] " +
                    "FROM (SELECT DISTINCT [Activation Preference] AS y FROM DAGDatabase dd " +
                    "INNER JOIN DAGMembers dm on dd.DAGMemberId=dm.ID ";
                if (DAGID != "")
                {
                    query += "WHERE dm.DAGID = " + DAGID;
                }
                query += ") AS Y " +
                    "ORDER BY y " +
                    "FOR XML PATH('')), " +
                    "1, 1, N'');" +
                    "SET @sql = N'SELECT * " +
                    "FROM (select DatabaseName, [Activation Preference], ServerName " +
                    "from DAGDatabase dd inner join DAGMembers dm on dd.DAGMemberId=dm.ID ";
                if (DAGID != "")
                {
                    query += "where dm.DAGID = " + DAGID;
                }
                query += ") AS D " +
                    "PIVOT(max(ServerName) FOR [Activation Preference] IN(' + @cols + N')) AS P;'; " +
                    "EXEC sp_executesql @sql; ";       
                dt = adaptor.FetchData(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //7/25/2014 NS added
        public DataTable GetDAGActivePassive(string DAGID)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                /*
                    DECLARE @T AS TABLE(y int NOT NULL PRIMARY KEY);

                    DECLARE
                    @cols AS NVARCHAR(MAX),
                    @y    AS int,
                    @sql  AS NVARCHAR(MAX);

                    -- Construct the column list for the IN clause
                    -- e.g., [1],[2],[3]
                    SET @cols = STUFF(
                    (SELECT N',' + QUOTENAME(y) AS [text()]
                    FROM (SELECT DISTINCT [Activation Preference] AS y FROM DAGDatabase  dd
                    INNER JOIN DAGMembers dm on dd.DAGMemberId=dm.ID
                    WHERE dm.DAGID = 14) AS Y
                    ORDER BY y
                    FOR XML PATH('')),
                    1, 1, N'');

                    -- Construct the full T-SQL statement
                    -- and execute dynamically
                    SET @sql = N'SELECT *
                    FROM (select DatabaseName, [Activation Preference], IsActive
                    from DAGDatabase dd inner join DAGMembers dm on dd.DAGMemberId=dm.ID
                    where dm.DAGID = 14
                    ) AS D
                    PIVOT(max(IsActive) FOR [Activation Preference] IN(' + @cols + N')) AS P;';

                    EXEC sp_executesql @sql;

                 */
                query = "DECLARE @T AS TABLE(y int NOT NULL PRIMARY KEY); " +
                    "DECLARE " +
                    "@cols AS NVARCHAR(MAX), " +
                    "@y    AS int, " +
                    "@sql  AS NVARCHAR(MAX); " +
                    "SET @cols = STUFF( " +
                    "(SELECT N',' + QUOTENAME(y) AS [text()] " +
                    "FROM (SELECT DISTINCT [Activation Preference] AS y FROM DAGDatabase dd " +
                    "INNER JOIN DAGMembers dm on dd.DAGMemberId=dm.ID ";
                if (DAGID != "")
                {
                    query += "WHERE dm.DAGID = " + DAGID;
                }
                query += ") AS Y " +
                    "ORDER BY y " +
                    "FOR XML PATH('')), " +
                    "1, 1, N''); " +
                    "SET @sql = N'SELECT * " +
                    "FROM (select DatabaseName, [Activation Preference], IsActive " +
                    "from DAGDatabase dd inner join DAGMembers dm on dd.DAGMemberId=dm.ID ";
                if (DAGID != "")
                {
                    query += "where dm.DAGID = " + DAGID;
                }
                query += ") AS D " +
                    "PIVOT(max(IsActive) FOR [Activation Preference] IN(' + @cols + N')) AS P;'; " +
                    "EXEC sp_executesql @sql; ";
                dt = adaptor.FetchData(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //7/25/2014 NS added
        public DataTable GetDAGHealth(string DAGID)
        {
            DataTable dt = new DataTable();
            string query = "";
            try
            {
                /*
                    DECLARE @T AS TABLE(y int NOT NULL PRIMARY KEY);

                    DECLARE
                    @cols AS NVARCHAR(MAX),
                    @y    AS int,
                    @sql  AS NVARCHAR(MAX);

                    -- Construct the column list for the IN clause
                    -- e.g., [1],[2],[3]
                    SET @cols = STUFF(
                    (SELECT N',' + QUOTENAME(y) AS [text()]
                    FROM (SELECT DISTINCT [Activation Preference] AS y FROM DAGDatabase  dd
                    INNER JOIN DAGMembers dm on dd.DAGMemberId=dm.ID
                    WHERE dm.DAGID = 14) AS Y
                    ORDER BY y
                    FOR XML PATH('')),
                    1, 1, N'');

                    -- Construct the full T-SQL statement
                    -- and execute dynamically
                    SET @sql = N'SELECT *
                    FROM (select DatabaseName, [Activation Preference], ContendIndex
                    from DAGDatabase dd inner join DAGMembers dm on dd.DAGMemberId=dm.ID
                    where dm.DAGID = 14
                    ) AS D
                    PIVOT(max(ContendIndex) FOR [Activation Preference] IN(' + @cols + N')) AS P;';

                    EXEC sp_executesql @sql;

                 */
                query = "DECLARE @T AS TABLE(y int NOT NULL PRIMARY KEY); " +
                    "DECLARE " +
                    "@cols AS NVARCHAR(MAX), " +
                    "@y    AS int, " +
                    "@sql  AS NVARCHAR(MAX); " +
                    "SET @cols = STUFF( " +
                    "(SELECT N',' + QUOTENAME(y) AS [text()] " +
                    "FROM (SELECT DISTINCT [Activation Preference] AS y FROM DAGDatabase dd " +
                    "INNER JOIN DAGMembers dm on dd.DAGMemberId=dm.ID ";
                if (DAGID != "")
                {
                    query += "WHERE dm.DAGID = " + DAGID;
                }
                query += ") AS Y " +
                    "ORDER BY y " +
                    "FOR XML PATH('')), " +
                    "1, 1, N''); " +
                    "SET @sql = N'SELECT * " +
                    "FROM (select DatabaseName, [Activation Preference], ContendIndex " +
                    "from DAGDatabase dd inner join DAGMembers dm on dd.DAGMemberId=dm.ID ";
                if (DAGID != "")
                {
                    query += "where dm.DAGID = " + DAGID;
                }
                query += ") AS D " +
                    "PIVOT(max(ContendIndex) FOR [Activation Preference] IN(' + @cols + N')) AS P;'; " +
                    "EXEC sp_executesql @sql; ";
                dt = adaptor.FetchData(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

		public bool isResponding(string TypeANDName)
		{
			DataTable dt = new DataTable();
			try
			{
				//string SqlQuery = "select * FROM [vitalsigns].[dbo].StatusDetail where TestType='" + TestType + "' and TypeAndName= '" + servername + "-Exchange'";
				string SqlQuery = "select Status FROM [vitalsigns].[dbo].Status where TypeAndName= '" + TypeANDName + "'";

				dt = objAdaptor.FetchData(SqlQuery);
			}
			catch
			{
			}
			finally
			{
			}
			if (dt.Rows.Count > 0)
			{
				if (dt.Rows[0]["Status"].ToString() == "Not Responding")
					return false;
				return true;
			}
			return true;
		}

		//10/8/2014 WS VE-107
		public DataTable getMailDatabaseSettings(string ServerName)
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = "SELECT ServerName, DatabaseName, WhiteSpaceThreshold, DatabaseSizeThreshold FROM ExchangeDatabaseSettings where ServerName='" + ServerName + "'";

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

		public DataTable getMailDatabaseSizeSettings(string ServerName)
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = "SELECT emb.ServerName, emb.DatabaseName, eds.WhiteSpaceThreshold, eds.DatabaseSizeThreshold,  " +
									"emb.SizeMB, emb.WhiteSpaceMB, case when (isnull(eds.WhiteSpaceThreshold,0)=0 or isnull(eds.DatabaseSizeThreshold,0)=0)  then 0 else 1 end as isSelected   " +
									"from ExchangeMailboxOverview emb left outer join ExchangeDatabaseSettings eds on " +
									"eds.ServerName=emb.ServerName and eds.DatabaseName=emb.DatabaseName  " +
									"where emb.ServerName = '" + ServerName + "'";

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


		//10/27/14 WS VSPLUS-1067
		public DataTable GetDAGDBDetails(int DAGID)
		{
			DataTable dt = new DataTable();
			try
			{
				string strQuerry = "SELECT tbl.*, " +
									"case  " +
									"WHEN LastBackup = LastFullBackup THEN 'Full Backup' " +
									"WHEN LastBackup = LastIncrementalBackup THEN 'Incremental Backup' " +
									"WHEN LastBackup = LastDifferentialBackup THEN 'Differential Backup' " +
									"WHEN LastBackup = LastCopyBackup THEN 'Copy Backup' " +
									"END as BackupType " +
									"FROM " +
									"(SELECT ID, DatabaseName, StorageGroup, Mounted, BackupInProgress, OnlineMaintInProg, (Select Max(d) FROM (VALUES (LastFullBackup),(LastIncrementalBackup), (LastDifferentialBackup), (LastCopyBackup)) as value(d)) as LastBackup " +
									"from DAGDatabaseDetails WHERE DAGID=" + DAGID + ") as tbl, DAGDatabaseDetails ddd where ddd.ID=tbl.ID and DAGID=" + DAGID + "";
				dt = adaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		public string getDagIdFromName(string DagName)
		{
			DataTable dt = new DataTable();
			try
			{
				string strQuerry = "Select ID from DagStatus Where DagName='" + DagName + "'";
				dt = adaptor.FetchData(strQuerry);
				if (dt.Rows.Count == 1)
					return dt.Rows[0]["ID"].ToString();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return "-1";
		}

        //2/9/2015 NS added for VSPLUS-1358
        public DataTable GetExchangeServersList()
        {
            DataTable dt = new DataTable();
            string SqlQuery = "";
            try
            {
                SqlQuery = "SELECT DISTINCT ServerName FROM MicrosoftSummaryStats t1 " +
                    "INNER JOIN vitalsigns.dbo.ServerTypes t2 ON t1.ServerTypeId=t2.ID " +
                    "WHERE t2.ServerType='Exchange' " +
                    "ORDER BY ServerName";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }
    }
}
