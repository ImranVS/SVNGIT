using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.DashboardDAL
{
    public class ExchangeMailFilesDAL
    {
        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
        private Adaptor adaptor = new Adaptor();
        private static ExchangeMailFilesDAL _self = new ExchangeMailFilesDAL();

        public static ExchangeMailFilesDAL Ins
        {
            get
            {
                return _self;
            }
        }

		public DataTable GetMails()
		{
			DataTable dt = new DataTable();
			string SqlQuery = "";
			try
			{
				//1/29/2016 NS modified for VSPLUS-2564
                //SqlQuery = "Select ID,ScanDate,[Database],DisplayName,CASE when server <> '' THEN server ELSE 'No Server Defined' END as Server,IssueWarningQuota,ProhibitSendQuota,ProhibitSendReceiveQuota," +
				//		"TotalItemSizeInMB,ItemCount,StorageLimitStatus  " +
                //        "from ExchangeMailfiles order by server,DisplayName";
                SqlQuery = "select [sent], [received], " +
                    "ID,ScanDate,[Database],DisplayName,[Server], " +
                    "IssueWarningQuota,ProhibitSendQuota,ProhibitSendReceiveQuota,TotalItemSizeInMB,ItemCount,StorageLimitStatus from " +
                    "(select sum(statvalue) AS StatValue,CASE " +
                    "        WHEN StatName LIKE '%sent%' THEN 'sent' " +
                    "        WHEN StatName LIKE '%received%' THEN 'received' " +
                    "        ELSE NULL " +
                    "    END AS StatName,t1.ID,ScanDate,[Database],DisplayName,CASE when server <> '' THEN server ELSE 'No Server Defined' END [Server], " +
                    "IssueWarningQuota,ProhibitSendQuota,ProhibitSendReceiveQuota,TotalItemSizeInMB,ItemCount,StorageLimitStatus " +
                    "from ExchangeMailfiles t1 left outer join MicrosoftDailyStats t2 on  " +
                    "displayname=SUBSTRING(substring(StatName,CHARINDEX('.',StatName)+1,LEN(StatName)),0,CHARINDEX('.',substring(StatName,CHARINDEX('.',StatName)+1,LEN(StatName))))  " +
                    "where StatName   like   '%count%' and StatName like 'Mailbox%' and  (StatName like '%sent%'  " +
                    "or StatName like '%received%')  " +
                    "and datediff(dd,0,dateadd(dd,0,date)) = datediff(dd,0,dateadd(dd,0,GETDATE())) " +
                    "GROUP BY " +
                    "    CASE " +
                    "        WHEN StatName LIKE '%sent%' THEN 'sent' " +
                    "        WHEN StatName LIKE '%received%' THEN 'received' " +
                    "        ELSE NULL " +
                    "    END,t1.ID,ScanDate,[Database],DisplayName,CASE when server <> '' THEN server ELSE 'No Server Defined' END, " +
                    "IssueWarningQuota,ProhibitSendQuota,ProhibitSendReceiveQuota,TotalItemSizeInMB,ItemCount,StorageLimitStatus " +
                    "    )AS sourcetable " +
                    "		PIVOT " +
                    "		(SUM(StatValue) FOR StatName in ([sent],[received])) as pivottabble ";                        

				dt = objAdaptor.FetchData(SqlQuery);

			}
			catch (Exception)
			{

				throw;
			}
			return dt;
		}

        public DataTable FillProblemsGrid()
        {

            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "Select * from Daily where Status <>'OK' and Temp = 0";
                 dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {
                
                throw;
            }
            return dt;
        }

        public DataTable FillReplicationGrid()
        {

            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "Select * from Daily where EnabledForReplication='False' and Temp = 0";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return dt;
        }

       

		public DataTable FillServerCombobox()
		{

			DataTable dt = new DataTable();
			try
			{
				//12/17/2013 NS modified - added sort
				string SqlQuery = "Select Distinct Server from ExchangeMailfiles ORDER BY Server ";
				dt = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception)
			{

				throw;
			}
			return dt;
		}
		public DataTable FillDBCombobox()
		{

			DataTable dt = new DataTable();
			try
			{
				//12/17/2013 NS modified - added sort
				string SqlQuery = "Select Distinct [Database] from ExchangeMailfiles ORDER BY [Database] ";
				dt = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception)
			{

				throw;
			}
			return dt;
		}

        public DataTable FillDBByTemplateGrid()
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "Select FileName, Title, CASE WHEN ISNULL(DesignTemplateName,'')='' THEN '(No Template Inheritance)' ELSE DesignTemplateName END DesignTemplateName, " + 
                    "Status, Server, Quota, ScanDate, FileSize from Daily where Temp = 0 ";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return dt;
        }
    }
}
