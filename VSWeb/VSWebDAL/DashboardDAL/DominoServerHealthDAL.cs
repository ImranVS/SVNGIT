using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
    public class DominoServerHealthDAL
    {
        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
        private Adaptor adaptor = new Adaptor();
        private static DominoServerHealthDAL _self = new DominoServerHealthDAL();

        public static DominoServerHealthDAL Ins
        {
            get
            {
                return _self;
            }
        }

        //public DataTable CheckedData()
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        string strQuery = "select distinct S.servername from [VitalSigns].[dbo].[Servers] as S right outer join [VitalSigns].[dbo].[Dominoservers] AS S2 on S2.ServerID = S.ServerTypeID where S.servername !=null or S.servername!=''";
        //        dt = adaptor.FetchData(strQuery);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable SetGrid(string serverType)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT TypeANDName,Name,Status,Location,'false' as isSelected,Type  FROM [VitalSigns].[dbo].[Status] WHERE Type = '" + serverType + "'";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetGraphForMailBox(string str, string ColumnName)
        {
            DataTable dt = new DataTable();
            try
            {
                //2/27/2014 NS modified for VSPLUS-414
                //string strQuerry = "select server," + ColumnName + " from [VSS_Statistics].[dbo].[dailysummary] where server in (" + str + ")";
                //
                string strQuerry = "";
                if (ColumnName == "Count")
                {
                    strQuerry = "select server," + ColumnName + "(*) Count from Daily " +
                    "WHERE Temp!=1 and server in (" + str + ") " +
                    "GROUP BY server";
                }
                else
                {
                    strQuerry = "select server," + ColumnName + "(FileSize) Sum from Daily " +
                    "WHERE Temp!=1 and server in (" + str + ") " +
                    "GROUP BY server";
                }
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable SetBarGraphForTopMail(string serverType)
        {
            try
            {
                DataTable dt = new DataTable();
                string StrQuery="";
                if (serverType == "All Servers" || serverType == "")
                {
                    //1/11/2013 NS modified
                    //StrQuery = "select top(20) Title, FileSize from [VSS_Statistics].[dbo].[Daily] where IsMailFile=1 order by FileSize desc";
                    StrQuery = "select top(20) Title + ' (' + Server + ')' Title, FileSize from [VSS_Statistics].[dbo].[Daily] where IsMailFile=1 and Temp = 0 order by FileSize desc";
                    // string StrQuery = "select top(20) Title,FileSize from [VSS_Statistics].[dbo].[Daily] where IsMailFile=1 and Title in (select srv.ServerName from VitalSigns.dbo.DominoServers ds, VitalSigns.dbo.Servers srv where ds.ServerID=srv.ID) order by FileSize desc";
                }
                else
                {
                    StrQuery = "select top(20) Title, FileSize from [VSS_Statistics].[dbo].[Daily] where IsMailFile=1 and Temp = 0 and Server = '" + serverType + "' order by FileSize desc";
                }
                    dt = objAdaptor.FetchData(StrQuery);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
		public DataTable SetGraphForTopMailUsers(string server,string radiovalue1,string radiovalue2)


		{
			try
			{
				DataTable dt = new DataTable();
				string StrQuery = "";
                //1/19/2016 NS modified for VSPLUS-2488
				if (server == "All Servers" || server == "")
				{
					//1/11/2013 NS modified
					//StrQuery = "select top(20) Title, FileSize from [VSS_Statistics].[dbo].[Daily] where IsMailFile=1 order by FileSize desc";
					//StrQuery = "select top (10) StatValue,StatName from [VSS_Statistics].[dbo].[MicrosoftDailyStats] where StatName   like   '" + radiovalue1 + "' and StatName like 'Mailbox%' and  StatName like '" + radiovalue2 + "'";
					// string StrQuery = "select top(20) Title,FileSize from [VSS_Statistics].[dbo].[Daily] where IsMailFile=1 and Title in (select srv.ServerName from VitalSigns.dbo.DominoServers ds, VitalSigns.dbo.Servers srv where ds.ServerID=srv.ID) order by FileSize desc";
					StrQuery = "select top 10 sum(StatValue) as StatValue,MAX(StatValue) as MAXStatvalue,StatName from [VSS_Statistics].[dbo].[MicrosoftDailyStats] " +
                        "where StatName   like   '" + radiovalue1 + "' and StatName like 'Mailbox%' and  StatName like '" + radiovalue2 + "' " +
                        "and date>=dateadd(hh,-24,GETDATE()) and date <= GETDATE() " +
                        "group by statname ";
				}
				else
				{
					//StrQuery = "select top (10) StatValue,StatName from [VSS_Statistics].[dbo].[MicrosoftDailyStats] where StatName   like   '" + radiovalue1 + "' and  StatName like 'Mailbox%' and  StatName like '" + radiovalue2 + "' and  ServerName= '" + server + "' ";
					StrQuery = "select top 10 sum(StatValue) as StatValue,MAX(StatValue) as MAXStatvalue,StatName from [VSS_Statistics].[dbo].[MicrosoftDailyStats] " +
                        "where StatName   like   '" + radiovalue1 + "' and StatName like 'Mailbox%' and  StatName like '" + radiovalue2 + "' " +
                        "and date>=dateadd(hh,-24,GETDATE()) and date <= GETDATE() " +
                        "and  ServerName= '" + server + "' " +
                        "group by statname ";
				}
                StrQuery += "order by StatValue desc ";
				dt = objAdaptor.FetchData(StrQuery);
				return dt;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public DataTable GetMailStatus(string servernamelist)
        {
            DataTable mailstatus = new DataTable();
            try
            {
                //string[] NameandType = ServerName.Split('-');
                string SqlQuery = "select Name, Mail, Value " + "from (select Name,isnull(DeadMail,0) DeadMail,isnull(PendingMail,0) PendingMail,isnull(cast(HeldMail as int),0) HeldMail from Status where Name IN (" + servernamelist + "))P  " + "UNPIVOT " + "(Value FOR Mail IN  " + " (DeadMail,PendingMail,HeldMail))AS unpvt";
                mailstatus = adaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return mailstatus;
        }

        public DataTable SetGraphForTransactionPerMin(string servernamelist)
        {
            DataTable dt = new DataTable();
            try
            {
                //string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Trans.PerMinute' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName] IN (" + servernamelist + ") order by [ServerName] asc";
                
                //1/7/2013 NS modified the query below
                //string strQuerry = " SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, Max(StatValue) as StatValue," +
                //    "[ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Trans.PerMinute' and Date > DATEADD (hh , -1 ,'2012-11-10 00:00:00.000' )" +
                //    " and [ServerName] IN ( "+servernamelist+") group by [ServerName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) order by [ServerName] asc";
                //1/22/2014 NS added sort for VSPLUS-265
                string strQuerry = "SELECT Date, [StatValue], " +
                "[ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Trans.PerMinute' " +
                "and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE()) and [ServerName] IN (" + servernamelist + ") " +
                "order by ServerName,Date";
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
              //  string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Platform.System.PctCombinedCpuUtil' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName] IN (" + servernamelist + ") order by [ServerName] asc";
                //1/7/2013 NS modified the query below
                //string strQuerry = " SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, Max(StatValue) as StatValue," +
                //    "[ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Platform.System.PctCombinedCpuUtil' and Date > DATEADD (hh , -1 ,'2012-11-10 00:00:00.000' )" +
                //    " and [ServerName] IN ( " + servernamelist + ") group by [ServerName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) order by [ServerName] asc";
                //1/22/2014 NS added sort by server for VSPLUS-265
                string strQuerry = "SELECT Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] " + 
                    "where [StatName]='Platform.System.PctCombinedCpuUtil' and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE()) " +
                    "and [ServerName] IN (" + servernamelist + ") order by ServerName,Date";
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
                //  string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue], [ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mem.PercentUsed' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [ServerName] IN (" + servernamelist + ") order by [ServerName] asc";
                //1/7/2013 NS modified the query below
                //string strQuerry = " SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, Max(StatValue) as StatValue," +
                //     "[ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mem.PercentAvailable' and Date > DATEADD (hh , -1 ,'2012-11-10 00:00:00.000' )" +
                //     " and [ServerName] IN ( " + servernamelist + ") group by [ServerName],CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) order by [ServerName] asc";
                //1/22/2014 NS added sort by server for VSPLUS-265
                string strQuerry = "SELECT Date, [StatValue], " +
                "[ServerName] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mem.PercentUsed' and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE()) " +
                "and [ServerName] IN ( " + servernamelist + ") " +
                "order by ServerName,Date";
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //1/2/2014 NS added
        public DataTable SetBarGraphForTopQuota(string serverType)
        {
            try
            {
                DataTable dt = new DataTable();
                string StrQuery = "";
                //4/8/2014 NS modified for VSPLUS-485
                /*
                if (serverType == "All Servers" || serverType == "")
                {
                    StrQuery = "select distinct top(20) Title + ' (' + Server + ')' Title, " +
                        "CASE WHEN ISNULL(Quota,0) = 0 THEN 0 ELSE ROUND(ISNULL(FileSize,0)/Quota*100,1) END AS PercentQuota "+
                        "from [VSS_Statistics].[dbo].[Daily] where IsMailFile=1 and Temp = 0 order by PercentQuota desc";
                }
                else
                {
                    StrQuery = "select distinct top(20) Title + ' (' + Server + ')' Title, " +
                        "CASE WHEN ISNULL(Quota,0) = 0 THEN 0 ELSE ROUND(ISNULL(FileSize,0)/Quota*100,1) END AS PercentQuota "+
                        "from [VSS_Statistics].[dbo].[Daily] where IsMailFile=1 and Temp = 0 and Server = '" + serverType + "' " +
                        "order by PercentQuota desc";
                }
                 */
                if (serverType == "All Servers" || serverType == "")
                {
                    StrQuery = "select top 20 Title + ' (' + ReplicaID + ')' Title, CASE WHEN ISNULL(Quota,0) = 0 THEN 0 ELSE ROUND(ISNULL(FileSize,0)/Quota*100,1) END AS PercentQuota " +
                        "from [VSS_Statistics].[dbo].[Daily] where IsMailFile=1 and Temp = 0 " +
                        "group by ReplicaID,Title,CASE WHEN ISNULL(Quota,0) = 0 THEN 0 ELSE ROUND(ISNULL(FileSize,0)/Quota*100,1) END " +
                        "order by PercentQuota desc ";
                }
                else
                {
                    StrQuery = "select top(20) Title, CASE WHEN ISNULL(Quota,0) = 0 THEN 0 ELSE ROUND(ISNULL(FileSize,0)/Quota*100,1) END AS PercentQuota " +
                        "from [VSS_Statistics].[dbo].[Daily] where IsMailFile=1 and Temp = 0 and Server = '" + serverType + "' " +
                        "order by PercentQuota desc";
                }
                dt = objAdaptor.FetchData(StrQuery);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //1/15/2014 NS added
        public DataTable SetPieChartForMailTemplates(string servername)
        {
            try
            {
                DataTable dt = new DataTable();
                string StrQuery = "";
                if (servername == "All Servers" || servername == "")
                {
                    StrQuery = "select count(*) MailDBCount, CASE WHEN DesignTemplateName = '' OR " +
                        "DesignTemplateName IS NULL THEN 'No Template' ELSE DesignTemplateName END DesignTemplateName " + 
                        "from [VSS_Statistics].[dbo].[Daily] where IsMailFile=1 and Temp = 0 group by DesignTemplateName";
                }
                else
                {
                    StrQuery = "select count(*) MailDBCount, CASE WHEN DesignTemplateName = '' OR " +
                        "DesignTemplateName IS NULL THEN 'No Template' ELSE DesignTemplateName END DesignTemplateName " + 
                        "from [VSS_Statistics].[dbo].[Daily] where IsMailFile=1 and Temp = 0 AND Server = '" + servername + "' " +
                        "group by DesignTemplateName ";
                }
                dt = objAdaptor.FetchData(StrQuery);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
		public DataTable GetDominoServerDetails()
		{
			DataTable dt = new DataTable();

			try
			{
                //10/21/2015 NS modified for VSPLUS-2253
				string SqlQuery = "SELECT DISTINCT st.Status,(CONVERT(VARCHAR(20),(st.CPU * 100)) + '%') CPU," +
                    "(CONVERT(VARCHAR(20),(st.Memory * 100)) + '%') Memory,st.ResponseTime,sr.ServerName,st.OperatingSystem,st.DominoVersion " +
                    "FROM Servers sr INNER JOIN Status st ON st.name=sr.servername WHERE st.Type='Domino' AND " +
                    "(st.SecondaryRole IS NULL OR st.SecondaryRole = '')";
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

		public DataTable GetTravelerServerDetails()
		{
			DataTable dt = new DataTable();

			try
			{

				string SqlQuery = "select st.Status,(CONVERT(VARCHAR(20),(st.CPU * 100)) + '%')CPU,(CONVERT(VARCHAR(20),(st.Memory * 100)) + '%')Memory,st.ResponseTime,sr.ServerName from servers sr inner join status st on st.name=sr.servername where st.Type='Domino' and st.SecondaryRole='Traveler'";

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

		public DataTable GetSametimeServerDetails()
		{
			DataTable dt = new DataTable();

			try
			{

				string SqlQuery = "select  distinct st.Status,(CONVERT(VARCHAR(20),(st.CPU * 100)) + '%')CPU,(CONVERT(VARCHAR(20),(st.Memory * 100)) + '%')Memory,st.ResponseTime,sr.ServerName from servers sr inner join status st on st.name=sr.servername where st.Type='Domino' and st.SecondaryRole='Sametime'";

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
		public DataTable GetServerTasks(string ServerName)
		{
			DataTable Charttab = new DataTable();
			try
			{
				string SqlQuery = "Select * from DominoServerTaskStatus where Monitored=1 and ServerName='" + ServerName + "'";
				Charttab = adaptor.FetchData(SqlQuery);
			}
			catch (Exception)
			{

				throw;
			}
			return Charttab;
		}
		public DataTable GetMoniteredTasks(string ServerName)
		{
			DataTable Charttab = new DataTable();
			try
			{
				//string SqlQuery = "Select ds.ServerName,ds.TaskName,ds.Monitored,ds.StatusSummary,ds.PrimaryStatus,ds.SecondaryStatus,ds.LastUpdate from DominoServerTaskStatus ds inner join Status st on st.Name=ds.ServerName where ds.Monitored=1 and ds.ServerName='Label' and st.Status!= 'OK'";
				string SqlQuery = "Select ds.ServerName,ds.TaskName,ds.StatusSummary,ds.Monitored,ds.PrimaryStatus,ds.SecondaryStatus,ds.LastUpdate from DominoServerTaskStatus  ds inner join servers sr on sr.servername=ds.servername where  sr.servertypeid=1 and ds.StatusSummary!='OK' and ds.Monitored=1";
				Charttab = adaptor.FetchData(SqlQuery);
			}
			catch (Exception)
			{

				throw;
			}
			return Charttab;
		}
		public DataTable GetIssuesTasks()
		{
			DataTable HAStatus = new DataTable();
			try
			{
                //10/22/2015 NS modified for VSPLUS-2253
				string SqlQuery = "select sd.TestName,sd.Result,sd.LastUpdate,sd.Details,st.Name ServerName from StatusDetail sd inner join status st " +
                    "on sd.TypeANDName=st.TypeANDName where Type='Domino' and Result='Fail' " +
                    "order by servername ";
				HAStatus = adaptor.FetchData(SqlQuery);
			}
			catch
			{
			}
			finally
			{
			}
			return HAStatus;
		}

		public DataTable GetMailChart(string Type)
		{
			DataTable mailstatus = new DataTable();
			try
			{

				string SqlQuery = "select Type,Mail,sum(Value) as Value from " +
                                  "(select Type,isnull(DeadMail,0) DeadMail,isnull(PendingMail,0) PendingMail,isnull(cast(HeldMail as int),0) HeldMail from " +
                                  " Status where Type= 'Domino' )P  UNPIVOT (Value FOR Mail IN   (DeadMail,PendingMail,HeldMail))AS unpvt group by  Type,Mail";

				mailstatus = adaptor.FetchData(SqlQuery);
			}
			catch (Exception)
			{

				throw;
			}
			return mailstatus;

		}
    }
    }

