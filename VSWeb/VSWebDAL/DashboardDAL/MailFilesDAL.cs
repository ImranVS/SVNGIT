using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.DashboardDAL
{
	public class MailFilesDAL
	{
		private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
		private Adaptor adaptor = new Adaptor();
		private static MailFilesDAL _self = new MailFilesDAL();

		public static MailFilesDAL Ins
		{
			get
			{
				return _self;
			}
		}

		public DataTable GetMails(string tab)
		{
			DataTable dt = new DataTable();
			string SqlQuery = "";
			try
			{
				//12/16/2013 NS modified the queries below - there should be a % of quota value
				if (tab == "1")
				{
					SqlQuery = "Select ID,ScanDate,FileName,Title,FileSize,Server,DesignTemplateName,Quota,FTIndexed," +
"EnabledForClusterReplication,ReplicaID,ODS,Status,DocumentCount,Categories," +
					"CASE Created WHEN '1900-01-01 00:00:00.000' THEN NULL ELSE Created END as Created," +
"CurrentAccessLevel,FTIndexFrequency,IsInService,Folder,IsPrivateAddressBook,IsPublicAddressBook," +
					"Case LastFixup when '1900-01-01 00:00:00.000'then null else LastFixup END as LastFixup ," +
"Case LastFTIndexed when '1900-01-01 00:00:00.000'then null else LastFTIndexed END as LastFTIndexed ," +
"PercentUsed,Details,Case LastModified when '1900-01-01 00:00:00.000'then null else LastModified END as LastModified," +
"EnabledForReplication,IsMailFile,InboxDocCount,Q_PlaceBotCount," +
"Q_CustomFormCount,PersonDocID,FileNamePath,CASE WHEN ISNULL(Quota,0) = 0 THEN 0 ELSE ROUND(ISNULL(FileSize,0)/Quota*100,1) END PercentQuota " +
"from Daily where IsMailFile ='True' and Temp = 0 order by FileName";
					//SqlQuery = "Select ID,ScanDate,FileName,Title,FileSize,Server,DesignTemplateName,Quota,FTIndexed," + 
					//    "EnabledForClusterReplication,ReplicaID,ODS,Status,DocumentCount,Categories,Created," + 
					//    "CurrentAccessLevel,FTIndexFrequency,IsInService,Folder,IsPrivateAddressBook," +
					//    "IsPublicAddressBook,LastFixup,LastFTIndexed,PercentUsed,Details,LastModified," + 
					//    "EnabledForReplication,IsMailFile,InboxDocCount,Q_PlaceBotCount,Q_CustomFormCount," +
					//    "PersonDocID,FileNamePath,CASE WHEN ISNULL(Quota,0) = 0 THEN 0 ELSE " +
					//    "ROUND(ISNULL(FileSize,0)/Quota*100,1) END PercentQuota  " + 
					//    "from Daily where IsMailFile ='True' and Temp = 0 order by FileName";
				}

				if (tab == "2")
				{

					SqlQuery = "Select *, CASE WHEN ISNULL(Quota,0) = 0 THEN 0 ELSE " +
						"ROUND(ISNULL(FileSize,0)/Quota*100,1) END PercentQuota from Daily where IsMailFile ='True' " +
						"order by InboxDocCount";

				}
				if (tab == "3")
				{
					SqlQuery = "Select *, CASE WHEN ISNULL(Quota,0) = 0 THEN 0 ELSE " +
						"ROUND(ISNULL(FileSize,0)/Quota*100,1) END PercentQuota from Daily where IsMailFile ='True' " +
						"order by PercentQuota DESC";
				}


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
				string SqlQuery = "Select * from Daily d inner join [vitalsigns].[dbo].[Servers] svrs on svrs.ServerName = d.Server AND svrs.ServerTypeID = 1 inner join [vitalsigns].[dbo].[DominoServers] ds on ds.ServerID = svrs.ID where d.Status <>'OK' and Temp = 0 and ds.enabled = 1";
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
				string SqlQuery = "Select Distinct Server from Daily where Temp = 0 ORDER BY Server ";
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
		public DataTable GetServerNames()
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = "select Distinct Servername from MicrosoftDailyStats where  statname like 'Mailbox%'";
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
