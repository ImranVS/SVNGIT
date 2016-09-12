using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
   public class MailHealthDAL
    {

        /// <summary>
        /// Declarations
        /// </summary>
       private AdaptorforDsahBoard objAdaptor1 = new AdaptorforDsahBoard();
        private Adaptor objAdaptor = new Adaptor();
        private static MailHealthDAL _self = new MailHealthDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static MailHealthDAL Ins
        {
            get { return _self; }
        }

        public DataTable GetMailServiceData()
        {

            DataTable StatusDataTable = new DataTable();
            try
            {

                string SqlQuery = "select Status,Name,TypeANDName,Location,Category,Details from Status where  StatusCode in ('Issue','Maintenance','Not Responding','OK') and Type='Mail'";
                    StatusDataTable = objAdaptor.FetchData(SqlQuery);
              
            }
            catch
            {
            }
            finally
            {
            }
            return StatusDataTable;
        }

        public DataTable GetNotesMailProbeData()
        {

            DataTable StatusDataTable = new DataTable();
            try
            {

				//string SqlQuery = "select Status,Name,TypeANDName,LastUpdate,Details from Status where  StatusCode in ('Issue','Maintenance','Not Responding','OK') and Type='NotesMail Probe'";
				string SqlQuery = "select st.Status,st.Name,st.TypeANDName,st.LastUpdate,st.Details from Status st inner join NotesMailProbe nm  on St.Name= nm.Name where  StatusCode in ('Issue','Maintenance','Not Responding','OK')  and Type='NotesMail Probe'";
                StatusDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return StatusDataTable;
        }
        public DataTable SetGraphForMailDelivered(string serverName)
        {
            DataTable dt = new DataTable();
            //if (paramGraph == "hh")
            //{
               try
                {
                    //string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mail.Delivered' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    string strQuerry = "select Servername,StatValue, Date " +
                             "from [VSS_Statistics].[dbo].[DominoDailyStats] " +
                             "where DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) and StatName='Mail.Delivered'  and Servername='" + serverName + "' " +
                             "order by Date ";
                   dt = objAdaptor1.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            //}
            //else
            //{
            //    try
            //    {
            //        string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mail.Delivered' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
            //        dt = objAdaptor1.FetchData(strQuerry);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            return dt;
        }


        public DataTable SetGraphForMailTransffered(string serverName)
        {
            DataTable dt = new DataTable();
            //if (paramGraph == "hh")
            //{
                try
                {
                    //1/10/2013 NS modified
                    //string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mail.Transferred' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    string strQuerry = "SELECT Date,[StatValue] " +
                        "FROM [VSS_Statistics].[dbo].[DominoDailyStats] " +
                        "where [StatName]='Mail.Transferred' and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
                        "and [ServerName]='" + serverName + "' order by Date asc";
                    dt = objAdaptor1.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            //}
            //else
            //{
            //    try
            //    {
            //        string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mail.Transferred' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
            //        dt = objAdaptor1.FetchData(strQuerry);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            return dt;
        }

        public DataTable SetGraphForMailRouted(string serverName)
        {
            DataTable dt = new DataTable();
            //if (paramGraph == "hh")
            //{
               try
                {
                   //1/10/2013 NS modified
                    //string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mail.TotalRouted' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    string strQuerry = "SELECT Date,[StatValue] " + 
                        "FROM [VSS_Statistics].[dbo].[DominoDailyStats] " +
                        "where [StatName]='Mail.TotalRouted' and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
                        "and [ServerName]='" + serverName + "'  order by Date asc";
                    dt = objAdaptor1.FetchData(strQuerry);
               }
                catch (Exception ex)
                {
                    throw ex;
                }
             return dt;
            }
            //else
            //{
            //    try
            //    {
            //        string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mail.TotalRouted' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
            //        dt = objAdaptor1.FetchData(strQuerry);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
           
      

        public DataTable SetGraphForMailTraffic(string paramGraph, string serverName)
        {
            DataTable dt = new DataTable();
            //if (paramGraph == "hh")
            //{
                try
                {
                    string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mail.TotalRouted' and Date > DATEADD (hh , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    dt = objAdaptor1.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            //}
            //else
            //{
            //    try
            //    {
            //        string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mail.TotalRouted' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
            //        dt = objAdaptor1.FetchData(strQuerry);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            return dt;
        }

        public DataTable GetDominoServer()
        {
            DataTable DominoTab = new DataTable();
            try
            {
                string SqlQuery = "select distinct ServerName from DominoServers,Servers where DominoServers.ServerID=Servers.ID";
                DominoTab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {
                
                throw;
            }
            return DominoTab;

        }

        public DataTable GetDominoServersForMailHealth()
        {
            DataTable DominoTab = new DataTable();
            try
            {
                string SqlQuery = "SELECT DISTINCT Name FROM dbo.Status WHERE Type='Domino' ORDER BY Name";
                DominoTab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return DominoTab;
        }

        public DataTable GetServerMailDelivered(string servername, int month, int year, bool exactmatch)
        {
            DataTable dt = new DataTable();
            string sqlQuery = "";
            string ANDservername = "";
            string ANDdevicename = "";
            try
            {
                if (servername == "")
                {
                    ANDservername = "";
                    ANDdevicename = "";
                }
                else
                {
                    if (exactmatch)
                    {
                        ANDservername = "  AND ServerName IN(" + servername + ") ";
                        ANDdevicename = "  AND DeviceName IN(" + servername + ") ";
                    }
                    else
                    {
                        ANDservername = "  AND ServerName LIKE '%" + servername + "%' ";
                        ANDdevicename = "  AND DeviceName LIKE '%" + servername + "%' ";
                    }
                }
                //6/1/2015 NS modified - added DeviceType='Domino' to the HourlyDownTimeMinutes query
                //8/1/2016 NS modified for VSPLUS-3170
                sqlQuery = "SELECT ServerName, [Mail.Delivered] MailDeliv, [Mail.AverageDeliverTime] MailAvgDeliv, " +
                        "[Server.AvailabilityIndex] SrvAvailInd, [HourlyDownTimeMinutes] DownTime, [Mem.PercentAvailable] MemAvgAvail, " +
                        "[Domino.Command.OpenDocument] WebOpenDoc, [Domino.Command.CreateDocument] WebCreateDoc, " +
                        "[Domino.Command.OpenDatabase] WebOpenDb, [Domino.Command.OpenView] WebOpenView, " +
                        "[Domino.Command.Total] WebComTotal, [HTTP sessions] HttpSessions " +
                        "FROM " +
                        " (SELECT ServerName,StatName,ROUND(SUM(StatValue),0) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Mail.Delivered' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(AVG(StatValue),1) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Mail.AverageDeliverTime' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(AVG(StatValue),1) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Server.AvailabilityIndex' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT DeviceName,StatName,ROUND(SUM(ISNULL(StatValue,0)),0) StatValue FROM dbo.DeviceUpTimeStats " +
                        "  WHERE StatName='HourlyDownTimeMinutes' AND DeviceType='Domino' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDdevicename +
                        "  GROUP BY DeviceName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(AVG(StatValue),1) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Mem.PercentAvailable' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(SUM(StatValue),0) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Domino.Command.OpenDocument' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(SUM(StatValue),0) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Domino.Command.CreateDocument' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(SUM(StatValue),0) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Domino.Command.OpenDatabase' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(SUM(StatValue),0) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Domino.Command.OpenView' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(SUM(StatValue),0) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Domino.Command.Total' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(SUM(StatValue),0) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='HTTP sessions' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        " ) AS SourceTable " +
                        "PIVOT " +
                        "( " +
                        "SUM(StatValue) " +
                        "FOR StatName IN ([Mail.Delivered], [Mail.AverageDeliverTime], [Server.AvailabilityIndex], " +
                        "[HourlyDownTimeMinutes],[Mem.PercentAvailable],[Domino.Command.OpenDocument], " +
                        "[Domino.Command.CreateDocument],[Domino.Command.OpenDatabase],[Domino.Command.OpenView], " +
                        "[Domino.Command.Total],[HTTP sessions]) " +
                        ") AS PivotTable ";
                dt = objAdaptor1.FetchData(sqlQuery);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable GetMailStats(string servername, int month, int year, bool exactmatch)
        {
            DataTable dt = new DataTable();
            string sqlQuery = "";
            string ANDservername = "";
            try
            {
                if (servername == "")
                {
                    ANDservername = "";
                }
                else
                {
                    if (exactmatch)
                    {
                        ANDservername = "  AND ServerName IN(" + servername + ") ";
                    }
                    else
                    {
                        ANDservername = "  AND ServerName LIKE '%" + servername + "%' ";
                    }
                }
                sqlQuery = "SELECT ServerName, ISNULL([Mail.TotalRouted],0) TotalRouted, ISNULL([Mail.Delivered],0) Delivered, " +
                    " ISNULL([Mail.TransferFailures],0) TransferFailures, ROUND(ISNULL([Mail.TotalPending],0),0) TotalPending, " +
                    " ISNULL([Mail.AverageDeliverTime],0) AvgDelivTime, ISNULL([Mail.AverageServerHops],0) AvgSrvHops, " +
                    " ISNULL([Mail.AverageSizeDelivered],0) AvgSizeDeliv, ISNULL([SMTP.MessagesProcessed],0) SMTPMsgProc " +
                        "FROM " +
                        " (SELECT ServerName,StatName,ROUND(SUM(StatValue),0) StatValue FROM dbo.DominoSummaryStats " + 
                        "  WHERE StatName='Mail.TotalRouted' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(SUM(ISNULL(StatValue,0)),0) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Mail.Delivered' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(SUM(ISNULL(StatValue,0)),0) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Mail.TransferFailures' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(AVG(ISNULL(StatValue,0)),1) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Mail.TotalPending' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(AVG(ISNULL(StatValue,0)),1) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Mail.AverageDeliverTime' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(AVG(ISNULL(StatValue,0)),1) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Mail.AverageServerHops' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(AVG(ISNULL(StatValue,0)),1) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='Mail.AverageSizeDelivered' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        "  UNION " +
                        "  SELECT ServerName,StatName,ROUND(SUM(ISNULL(StatValue,0)),0) StatValue FROM dbo.DominoSummaryStats " +
                        "  WHERE StatName='SMTP.MessagesProcessed' AND MonthNumber=" + month.ToString() + " AND YearNumber=" + year.ToString() +
                        ANDservername +
                        "  GROUP BY ServerName,StatName " +
                        
                        " ) AS SourceTable " +
                        "PIVOT " +
                        "( " +
                        "SUM(StatValue) " +
                        "FOR StatName IN ([Mail.TotalRouted],[Mail.Delivered], [Mail.TransferFailures], [Mail.TotalPending], " +
                        "[Mail.AverageDeliverTime],[Mail.AverageServerHops],[Mail.AverageSizeDelivered],[SMTP.MessagesProcessed]) " +
                        ") AS PivotTable ";
                dt = objAdaptor1.FetchData(sqlQuery);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable fillStatisticsServer()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT DISTINCT ServerName " +
                    "FROM DominoSummaryStats " +
                    "WHERE StatName='Mail.Delivered' " +
                    "ORDER BY DeviceName";
                dt = objAdaptor1.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

       //11/20/2013 NS added
        //2/9/2015 NS modified for VSPLUS-1446
        public DataTable GetMailDelivData(string ServerType)
        {
            try
            {
                DataTable dt = new DataTable();
                //1/6/2014 NS modified - added columns to the query
                //2/9/2015 NS modified for VSPLUS-1446
                string sql = "SELECT Name, Status, PendingMail, HeldMail, DeadMail, Location, Category,LastUpdate " +
                    "FROM Status WHERE Type='" + ServerType + "' ";
                sql += "ORDER BY PendingMail DESC,Name";
                dt = objAdaptor.FetchData(sql);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
