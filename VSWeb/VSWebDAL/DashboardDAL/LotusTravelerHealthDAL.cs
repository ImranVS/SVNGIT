using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
	public class LotusTravelerHealthDAL
	{
		private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
		private Adaptor objAdaptor1 = new Adaptor();
		private static LotusTravelerHealthDAL _self = new LotusTravelerHealthDAL();

		public static LotusTravelerHealthDAL Ins
		{
			get
			{
				return _self;
			}
		}

		public DataTable SetGrid1()
		{
			DataTable dt = new DataTable();
			try
			{
				//1/21/2014 NS modified the query to add Datastore Status
				//1/14/2014 NS modified for VSPLUS-285
				//string strQuerry = "SELECT ServerName As Name, Status,  Users, Details, IncrementalSyncs, ID, Devices, HTTP_Status, HTTP_Details, HTTP_PeakConnections, HTTP_MaxConfiguredConnections, TravelerVersion FROM [VitalSigns].[dbo].[Traveler_Status]";
				//2/5/2014 NS added TravelerServlet column for VSPLUS-328
				//string strQuerry = "SELECT ServerName As Name, Status,  Users, Details, IncrementalSyncs, ID, " +
				//    "Devices, HTTP_Status, HTTP_Details, HTTP_PeakConnections, HTTP_MaxConfiguredConnections, " +
				//    "TravelerVersion, HA, CASE WHEN HA=1 THEN '../images/imagesIcons/HA.gif' ELSE '' END HAImage, " +
				//    "ISNULL(HA_Datastore_Status,'') AS HA_Datastore_Status,TravelerServlet,HeartBeat,'Domino' as Type,DevicesAPIStatus " + 
				//    "FROM [VitalSigns].[dbo].[Traveler_Status]";

				//5/15/2015 NS modified for VSPLUS-1754
                //3/10/2016 Durga Modified for VSPLUS-2691
				string strQuerry = "  SELECT TS.ServerName As Name, TS.Status,  TS.Users, TS.Details, TS.IncrementalSyncs, TS.ID," +
					"TS.Devices, TS.HTTP_Status, TS.HTTP_Details, TS.HTTP_PeakConnections, TS.HTTP_MaxConfiguredConnections," +
					"TS.TravelerVersion, TS.HA, CASE WHEN HA=1 THEN '../images/imagesIcons/HA.gif' ELSE '' END HAImage, " +
					"ISNULL(TS.HA_Datastore_Status,'') AS HA_Datastore_Status,TS.TravelerServlet,TS.HeartBeat,'Domino' as Type,TS.DevicesAPIStatus," +
					 "TSR.Details as Reasons, ISNULL(TS.ResourceConstraint,'') AS ResourceConstraint " +
                    "FROM [VitalSigns].[dbo].[Traveler_Status] as TS left outer join TravelerStatusReasons as TSR on TS.ServerName=TSR.ServerName inner join servers sr on TS.ServerName=sr.ServerName inner join DominoServers ds on  sr.ID=ds.ServerID where ds.Enabled='true'  ORDER BY TS.ServerName";


				dt = objAdaptor1.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		public DataTable SetGrid3()
		{
			DataTable dt = new DataTable();
			try
			{
				//=================soma 03/02/2016 modified for VSPLUS-2509
				string strQuerry = "  select distinct TS.ServerName Name from [Traveler_Status] as TS left outer join TravelerStatusReasons as TSR on TS.ServerName=TSR.ServerName " +
									" union  " +
									 " select distinct TD.ServerName Name from Traveler_Devices AS TD LEFT OUTER JOIN [vitalsigns].[dbo].[MobileUserThreshold] As two ON TD.DeviceID = two.DeviceID where IsActive=1 ";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
		public DataTable SetGrid1(string servername)
		{
			DataTable dt = new DataTable();
			try
			{
				//1/21/2014 NS modified the query to add Datastore Status
				//1/14/2014 NS modified for VSPLUS-285
				//string strQuerry = "SELECT ServerName As Name, Status,  Users, Details, IncrementalSyncs, ID, Devices, HTTP_Status, HTTP_Details, HTTP_PeakConnections, HTTP_MaxConfiguredConnections, TravelerVersion FROM [VitalSigns].[dbo].[Traveler_Status]";
				//2/5/2014 NS added TravelerServlet column for VSPLUS-328
				//string strQuerry = "SELECT ServerName As Name, Status,  Users, Details, IncrementalSyncs, ID, " +
				//    "Devices, HTTP_Status, HTTP_Details, HTTP_PeakConnections, HTTP_MaxConfiguredConnections, " +
				//    "TravelerVersion, HA, CASE WHEN HA=1 THEN '../images/imagesIcons/HA.gif' ELSE '' END HAImage, " +
				//    "ISNULL(HA_Datastore_Status,'') AS HA_Datastore_Status,TravelerServlet,HeartBeat,'Domino' as Type,DevicesAPIStatus " + 
				//    "FROM [VitalSigns].[dbo].[Traveler_Status]";
				//5/15/2015 NS modified for VSPLUS-1754
				string strQuerry = "  SELECT TS.ServerName As Name, TS.Status,  TS.Users, TS.Details, TS.IncrementalSyncs, TS.ID," +
					"TS.Devices, TS.HTTP_Status, TS.HTTP_Details, TS.HTTP_PeakConnections, TS.HTTP_MaxConfiguredConnections," +
					"TS.TravelerVersion, TS.HA, CASE WHEN HA=1 THEN '../images/imagesIcons/HA.gif' ELSE '' END HAImage, " +
					"ISNULL(TS.HA_Datastore_Status,'') AS HA_Datastore_Status,TS.TravelerServlet,TS.HeartBeat,'Domino' as Type,TS.DevicesAPIStatus," +
					 "TSR.Details as Reasons, ISNULL(TS.ResourceConstraint,'') AS ResourceConstraint " +
					"FROM [VitalSigns].[dbo].[Traveler_Status] as TS left outer join TravelerStatusReasons as TSR on TS.ServerName=TSR.ServerName where ts.ServerName='" + servername + "' ORDER BY TS.ServerName";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
        public DataTable SetGraphForHttpSessions(string paramval, string servername)
        {
            DataTable dt = new DataTable();
            if (paramval == "hh")
            {
                try
                {
                    //1/7/2013 NS modified the query below
                    //string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':' +CONVERT(varchar, DATEPART ( n, date )) as Date, dd.[StatValue], ts.[HTTP_MaxConfiguredConnections]  FROM [VSS_Statistics].[dbo].[DominoDailyStats] dd JOIN [VitalSigns].[dbo].[Status] s ON dd.[ServerName] = s.[Name] JOIN [VitalSigns].[dbo].[Traveler_Status] ts ON s.[Name] = ts.[ServerName] WHERE s.[Name] = '" + servername + "' and dd.[Date] > DATEADD (" + paramval + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and s.[SecondaryRole] = 'Traveler' and dd.[StatName] = 'Http.CurrentConnections' order by Date asc";
                    //2/2/2015 NS modified for VSPLUS-1302
                    string strQuerry = "SELECT Date, dd.[StatValue], ts.[HTTP_MaxConfiguredConnections] " +
                    "FROM [VSS_Statistics].[dbo].[DominoDailyStats] dd JOIN [VitalSigns].[dbo].[Status] s ON " +
                    "dd.[ServerName] = s.[Name] JOIN [VitalSigns].[dbo].[Traveler_Status] ts ON " +
                    "s.[Name] = ts.[ServerName] WHERE s.[Name] = '" + servername + "' " +
                    //"and DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE()) " +
                    "and Date>=DATEADD(hh,-24,GETDATE()) " +
                    "and s.[SecondaryRole] = 'Traveler' and dd.[StatName] = 'Http.CurrentConnections' order by Date asc";
                    dt = objAdaptor.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    //1/7/2013 NS modified the query below
                    //string strQuerry = "SELECT dd.[Date], dd.[StatValue], ts.[HTTP_MaxConfiguredConnections] FROM [VSS_Statistics].[dbo].[DominoDailyStats] dd JOIN [VitalSigns].[dbo].[Status] s ON dd.[ServerName] = s.[Name] JOIN [VitalSigns].[dbo].[Traveler_Status] ts ON s.[Name] = ts.[ServerName] WHERE s.[Name] = '" + servername + "' and dd.[Date] > DATEADD (" + paramval + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and s.[SecondaryRole] = 'Traveler' and dd.[StatName] = 'Http.CurrentConnections' order by Date asc";
                    string strQuerry = "SELECT Date,dd.[StatValue], ts.[HTTP_MaxConfiguredConnections] HTTP_MaxConfiguredConnections " +
                    "FROM [VSS_Statistics].[dbo].[DominoDailyStats] dd JOIN [VitalSigns].[dbo].[Status] s ON " +
                    "dd.[ServerName] = s.[Name] JOIN [VitalSigns].[dbo].[Traveler_Status] ts ON " +
                    "s.[Name] = ts.[ServerName] WHERE s.[Name] = '" + servername + "' and " +
                    "dd.[Date] >= DATEADD (dd , -30 ,GETDATE()) and " +
                    "s.[SecondaryRole] = 'Traveler' and dd.[StatName] = 'Http.CurrentConnections' " +
                    "order by dd.[Date] asc ";
                    dt = objAdaptor.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }


		public DataTable SetGraphForDeviceType(string SortField, string ServerName)
		{
			DataTable dt = new DataTable();
			string SortDirection = " DESC";
			if (SortField == "No_of_Users")
				SortDirection = " ASC ";
			if (ServerName == "All")
			{

				string strQuerry = "SELECT COUNT(*) as No_of_Users, DeviceName FROM Traveler_Devices Where IsActive=1 GROUP BY DeviceName ORDER BY " + SortField + SortDirection;
				dt = objAdaptor1.FetchData(strQuerry);
			}
			else
			{
				try
				{
					string strQuerry = "SELECT COUNT(*) as No_of_Users, DeviceName FROM Traveler_Devices WHERE IsActive=1 and ServerName = '" + ServerName + "' GROUP BY DeviceName ORDER BY " + SortField + SortDirection;
					dt = objAdaptor1.FetchData(strQuerry);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			return dt;
		}
		public DataTable SetGraphForDevice_OSType(string SortField, string ServerName, string GraphType)
		{
			DataTable dt = new DataTable();
			string SortDirection = " DESC";
			if (SortField == "No_of_Users")
				SortDirection = " ASC ";
			string Top20 = "";
			if (GraphType == "0")
			{
				SortField = "No_of_Users";
				Top20 = " TOP 20 ";
				SortDirection = " DESC ";
			}
			if (ServerName == "All")
			{

				//string strQuerry = "select Count(*) as No_of_Users, Cast(Count(*) as varchar)+'-'+ OS_Type as OS_Type from (SELECT COUNT(*) as No_of_Users,'Apple ('+Substring(OS_Type,CHARINDEX('(',OS_Type)+1,(len(OS_Type)-(CHARINDEX('(',OS_Type)+1)))+')' as OS_Type FROM Traveler_Devices where OS_Type Like '%Apple%'  GROUP BY OS_Type )as Tab  GROUP BY OS_Type" +
				//" union " +
				//" SELECT COUNT(*) as No_of_Users, Cast(Count(*) as varchar)+'-'+ OS_Type FROM Traveler_Devices where OS_Type like '%Android%' GROUP BY OS_Type ORDER BY OS_Type";
				string strQuerry = "select " + Top20 + " Count(*) as No_of_Users, OS_Type_Min  FROM Traveler_Devices where IsActive=1 and OS_Type_min is not null group by OS_Type_Min ORDER BY " + SortField + SortDirection;
				dt = objAdaptor1.FetchData(strQuerry);
			}
			else
			{
				try
				{
					//    string strQuerry = "select Count(*) as No_of_Users, Cast(Count(*) as varchar)+'-'+ OS_Type as OS_Type from (SELECT COUNT(*) as No_of_Users,'Apple ('+Substring(OS_Type,CHARINDEX('(',OS_Type)+1,(len(OS_Type)-(CHARINDEX('(',OS_Type)+1)))+')' as OS_Type FROM Traveler_Devices where OS_Type Like '%Apple%' and ServerName='"+ServerName+"' GROUP BY OS_Type ) as Tab  GROUP BY OS_Type" +
					//" union " +
					//" SELECT COUNT(*) as No_of_Users, Cast(Count(*) as varchar)+'-'+ OS_Type FROM Traveler_Devices where OS_Type like '%Android%' and ServerName='" + ServerName + "' GROUP BY OS_Type ORDER BY OS_Type";
					string strQuerry = "select " + Top20 + " Count(*) as No_of_Users, OS_Type_Min FROM Traveler_Devices where IsActive=1 and OS_Type_min is not null and ServerName='" + ServerName + "' group by OS_Type_Min ORDER BY " + SortField + SortDirection;
					dt = objAdaptor1.FetchData(strQuerry);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			return dt;
		}
		public DataTable SetGraphForSyncType(string SyncType, string SyncSubType)
		{
			DataTable dt = new DataTable();
			if (SyncType == "All Devices")
			{

				string strQuerry = "SELECT COUNT(*) As No_of_Users,' Within 15 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and LastSyncTime between  dateadd(mi,-15 ,GETDATE()) and GETDATE() having count(*) >0 ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*)  As No_of_Users,' Between 15-30 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and LastSyncTime between  ";
				strQuerry += " dateadd(mi,-30 ,GETDATE()) and dateadd(mi,-15 ,GETDATE()) having count(*) >0 ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*)  As No_of_Users,' Between 30-60 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-60 ,GETDATE()) and dateadd(mi,-30 ,GETDATE())  having count(*) >0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*)  As No_of_Users,' Between 60-120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-120 ,GETDATE()) and dateadd(mi,-60 ,GETDATE()) having count(*) >0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*)  As No_of_Users,' Greater than 120 mins.' As Duration FROM [Traveler_Devices] where LastSyncTime   ";
				strQuerry += " < DATEADD(MINUTE, -120, GETDATE()) having count(*) >0";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			else if (SyncType == "By OS Type" && SyncSubType == "All OS")
			{
				string subType = " not like'%android%' and os_type_min not like '%ios%' and os_type_min not like '%win%' ";
				string strQuerry = "SELECT COUNT(*) As No_of_Users,os_type_min + ': Within 15 mins.' As Duration  FROM [Traveler_Devices] where   IsActive=1 and os_type_min " + subType + " and LastSyncTime between  dateadd(mi,-15 ,GETDATE()) and GETDATE() group by os_type_min  ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,os_type_min + ': Between 15-30 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and  os_type_min " + subType + " and LastSyncTime between  ";
				strQuerry += " dateadd(mi,-30 ,GETDATE()) and dateadd(mi,-15 ,GETDATE())  group by os_type_min ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,os_type_min + ': Between 30-60 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and  os_type_min " + subType + " and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-60 ,GETDATE()) and dateadd(mi,-30 ,GETDATE())  group by os_type_min ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,os_type_min + ': Between 60-120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and  os_type_min " + subType + " and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-120 ,GETDATE()) and dateadd(mi,-60 ,GETDATE()) group by os_type_min ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users, os_type_min + ': Greater than 120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and  os_type_min " + subType + " and LastSyncTime   ";
				strQuerry += " < DATEADD(MINUTE, -120, GETDATE()) group by os_type_min ";
				strQuerry += " union ";
				strQuerry += "SELECT COUNT(*) As No_of_Users,'" + "iOS" + ": Within 15 mins.' As Duration  FROM [Traveler_Devices] where  IsActive=1 and  os_type_min like '%" + "iOS" + "%' and LastSyncTime between  dateadd(mi,-15 ,GETDATE()) and GETDATE() having COUNT(*) > 0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'" + "iOS" + ": Between 15-30 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + "iOS" + "%' and LastSyncTime between  ";
				strQuerry += " dateadd(mi,-30 ,GETDATE()) and dateadd(mi,-15 ,GETDATE())  having COUNT(*) > 0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'" + "iOS" + ": Between 30-60 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + "iOS" + "%' and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-60 ,GETDATE()) and dateadd(mi,-30 ,GETDATE()) having COUNT(*) > 0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'" + "iOS" + ": Between 60-120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + "iOS" + "%' and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-120 ,GETDATE()) and dateadd(mi,-60 ,GETDATE()) having COUNT(*) > 0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'" + "iOS" + ": Greater than 120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + "iOS" + "%' and LastSyncTime   ";
				strQuerry += " < DATEADD(MINUTE, -120, GETDATE()) having COUNT(*) > 0";
				strQuerry += " union ";

				strQuerry += "SELECT COUNT(*) As No_of_Users,'" + "Android" + ": Within 15 mins.' As Duration  FROM [Traveler_Devices] where  IsActive=1 and  os_type_min like '%" + "Android" + "%' and LastSyncTime between  dateadd(mi,-15 ,GETDATE()) and GETDATE() having COUNT(*) > 0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'" + "Android" + ": Between 15-30 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + "Android" + "%' and LastSyncTime between  ";
				strQuerry += " dateadd(mi,-30 ,GETDATE()) and dateadd(mi,-15 ,GETDATE())  having COUNT(*) > 0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'" + "Android" + ": Between 30-60 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + "Android" + "%' and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-60 ,GETDATE()) and dateadd(mi,-30 ,GETDATE()) having COUNT(*) > 0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'" + SyncSubType + ": Between 60-120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + "Android" + "%' and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-120 ,GETDATE()) and dateadd(mi,-60 ,GETDATE()) having COUNT(*) > 0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'" + "Android" + ": Greater than 120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + "Android" + "%' and LastSyncTime   ";
				strQuerry += " < DATEADD(MINUTE, -120, GETDATE()) having COUNT(*) > 0";
				strQuerry += " union ";
				strQuerry += "SELECT COUNT(*) As No_of_Users,'" + "Win" + ": Within 15 mins.' As Duration  FROM [Traveler_Devices] where  IsActive=1 and  os_type_min like '%" + "Win" + "%' and LastSyncTime between  dateadd(mi,-15 ,GETDATE()) and GETDATE() having COUNT(*) > 0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'" + "Win" + ": Between 15-30 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + "Win" + "%' and LastSyncTime between  ";
				strQuerry += " dateadd(mi,-30 ,GETDATE()) and dateadd(mi,-15 ,GETDATE())  having COUNT(*) > 0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'" + "Win" + ": Between 30-60 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + "Win" + "%' and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-60 ,GETDATE()) and dateadd(mi,-30 ,GETDATE()) having COUNT(*) > 0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'" + "Win" + ": Between 60-120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + "Win" + "%' and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-120 ,GETDATE()) and dateadd(mi,-60 ,GETDATE()) having COUNT(*) > 0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'" + "Win" + ": Greater than 120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + "Win" + "%' and LastSyncTime   ";
				strQuerry += " < DATEADD(MINUTE, -120, GETDATE()) having COUNT(*) > 0";


				dt = objAdaptor1.FetchData(strQuerry);
			}
			else if (SyncType == "By OS Type" && (SyncSubType != "All OS" && SyncSubType != "All Other OS"))
			{

				string strQuerry = "SELECT COUNT(*) As No_of_Users,'Within 15 mins.' As Duration  FROM [Traveler_Devices] where  IsActive=1 and  os_type_min like '%" + SyncSubType + "%' and LastSyncTime between  dateadd(mi,-15 ,GETDATE()) and GETDATE()  having count(*) >0 ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'Between 15-30 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + SyncSubType + "%' and LastSyncTime between  ";
				strQuerry += " dateadd(mi,-30 ,GETDATE()) and dateadd(mi,-15 ,GETDATE())  having count(*) >0 ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'Between 30-60 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + SyncSubType + "%' and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-60 ,GETDATE()) and dateadd(mi,-30 ,GETDATE()) having count(*) >0 ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'Between 60-120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + SyncSubType + "%' and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-120 ,GETDATE()) and dateadd(mi,-60 ,GETDATE()) having count(*) >0 ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'Greater than 120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min like '%" + SyncSubType + "%' and LastSyncTime   ";
				strQuerry += " < DATEADD(MINUTE, -120, GETDATE()) having count(*) >0 ";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			else if (SyncType == "By OS Type" && (SyncSubType == "All Other OS"))
			{
				string subType = " not like'%android%' and os_type_min not like '%ios%' and os_type_min not like '%win%' ";
				string strQuerry = "SELECT COUNT(*) As No_of_Users,os_type_min + ': Within 15 mins.' As Duration  FROM [Traveler_Devices] where  IsActive=1 and  os_type_min " + subType + " and LastSyncTime between  dateadd(mi,-15 ,GETDATE()) and GETDATE() group by os_type_min ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,os_type_min + ': Between 15-30 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min " + subType + "  and LastSyncTime between  ";
				strQuerry += " dateadd(mi,-30 ,GETDATE()) and dateadd(mi,-15 ,GETDATE())  group by os_type_min ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,os_type_min + ': Between 30-60 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min " + subType + "  and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-60 ,GETDATE()) and dateadd(mi,-30 ,GETDATE())  group by os_type_min ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,os_type_min + ': Between 60-120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min " + subType + "  and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-120 ,GETDATE()) and dateadd(mi,-60 ,GETDATE()) group by os_type_min ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users, os_type_min + ': Greater than 120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and os_type_min " + subType + "  and LastSyncTime   ";
				strQuerry += " < DATEADD(MINUTE, -120, GETDATE()) group by os_type_min ";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			else if (SyncType == "By Device Type" && (SyncSubType == "All Device Types"))
			{
				string subType = " not like'%android%' and not like '%ios%' and not like '%win%' ";
				string strQuerry = "SELECT COUNT(*) As No_of_Users,devicename + ': Within 15 mins.' As Duration  FROM [Traveler_Devices] where  IsActive=1 and   LastSyncTime between  dateadd(mi,-15 ,GETDATE()) and GETDATE() group by devicename ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,devicename + ': Between 15-30 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and  LastSyncTime between  ";
				strQuerry += " dateadd(mi,-30 ,GETDATE()) and dateadd(mi,-15 ,GETDATE())  group by devicename ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,devicename + ': Between 30-60 mins.' As Duration FROM [Traveler_Devices] where   IsActive=1 and  LastSyncTime between   ";
				strQuerry += " dateadd(mi,-60 ,GETDATE()) and dateadd(mi,-30 ,GETDATE())  group by devicename ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,devicename + ': Between 60-120 mins.' As Duration FROM [Traveler_Devices] where   IsActive=1 and  LastSyncTime between   ";
				strQuerry += " dateadd(mi,-120 ,GETDATE()) and dateadd(mi,-60 ,GETDATE()) group by devicename ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users, devicename + ': Greater than 120 mins.' As Duration FROM [Traveler_Devices] where   IsActive=1 and   LastSyncTime   ";
				strQuerry += " < DATEADD(MINUTE, -120, GETDATE()) group by devicename ";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			else if (SyncType == "By Device Type" && (SyncSubType == "Apple Tablets" || SyncSubType == "Apple Phones" || SyncSubType == "Samsung" | SyncSubType == "All Other Devices"))
			{
				string subType = "";
				if (SyncSubType == "Apple Tablets")
					subType = " DeviceName like '%ipad%' ";
				if (SyncSubType == "Apple Phones")
					subType = " DeviceName like '%iphone%' ";
				if (SyncSubType == "Samsung")
					subType = " DeviceName like '%samsung%' ";
				if (SyncSubType == "All Other Devices")
					subType = " DeviceName not like '%samsung%' and DeviceName not like '%iphone%' and DeviceName not like '%ipad%'";

				string strQuerry = "SELECT COUNT(*) As No_of_Users,devicename + ': Within 15 mins.' As Duration  FROM [Traveler_Devices] where   IsActive=1 and  " + subType + " and LastSyncTime between  dateadd(mi,-15 ,GETDATE()) and GETDATE() group by devicename ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,devicename + ': Between 15-30 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and  " + subType + "  and LastSyncTime between  ";
				strQuerry += " dateadd(mi,-30 ,GETDATE()) and dateadd(mi,-15 ,GETDATE())  group by devicename ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,devicename + ': Between 30-60 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and  " + subType + "  and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-60 ,GETDATE()) and dateadd(mi,-30 ,GETDATE())  group by devicename ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,devicename + ': Between 60-120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and  " + subType + "  and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-120 ,GETDATE()) and dateadd(mi,-60 ,GETDATE()) group by devicename ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users, devicename + ': Greater than 120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and  " + subType + "  and LastSyncTime   ";
				strQuerry += " < DATEADD(MINUTE, -120, GETDATE()) group by devicename ";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			if (SyncType == "Key Users")
			{

				string strQuerry = "SELECT COUNT(*) As No_of_Users,' Within 15 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and DeviceID in(select deviceid from MobileUserThreshold) and  LastSyncTime between  dateadd(mi,-15 ,GETDATE()) and GETDATE() having count(*) >0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*)  As No_of_Users,' Between 15-30 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and DeviceID in(select deviceid from MobileUserThreshold) and LastSyncTime between  ";
				strQuerry += " dateadd(mi,-30 ,GETDATE()) and dateadd(mi,-15 ,GETDATE())  having count(*) >0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*)  As No_of_Users,' Between 30-60 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and DeviceID in(select deviceid from MobileUserThreshold) and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-60 ,GETDATE()) and dateadd(mi,-30 ,GETDATE())  having count(*) >0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*)  As No_of_Users,' Between 60-120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and DeviceID in(select deviceid from MobileUserThreshold) and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-120 ,GETDATE()) and dateadd(mi,-60 ,GETDATE()) having count(*) >0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*)  As No_of_Users,' Greater than 120 mins.' As Duration FROM [Traveler_Devices] where  IsActive=1 and DeviceID in(select deviceid from MobileUserThreshold) and LastSyncTime   ";
				strQuerry += " < DATEADD(MINUTE, -120, GETDATE()) having count(*) >0";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			return dt;
		}

		public DataTable SetGraphForDeviceCount()
		{
			DataTable dt = new DataTable();


			string strQuerry = "SELECT COUNT(*) Users,username   FROM [Traveler_Devices] WHERE  IsActive=1 group by UserName having COUNT(*) = 1";
			strQuerry += " union ";
			strQuerry += " SELECT COUNT(*) Users,UserName    FROM [Traveler_Devices] WHERE  IsActive=1 group by UserName having COUNT(*) = 2   ";
			strQuerry += " union ";
			strQuerry += " SELECT COUNT(*) Users,UserName    FROM [Traveler_Devices] WHERE  IsActive=1 group by UserName having COUNT(*) = 3   ";
			strQuerry += " union ";
			strQuerry += " SELECT COUNT(*) Users,UserName    FROM [Traveler_Devices] WHERE  IsActive=1 group by UserName having COUNT(*) = 4  ";
			strQuerry += " union ";
			strQuerry += " SELECT COUNT(*) Users,UserName    FROM [Traveler_Devices] WHERE  IsActive=1 group by UserName having COUNT(*) > 5  ";

			dt = objAdaptor1.FetchData(strQuerry);
			DataTable dtNew = new DataTable();

			dtNew.Columns.Add("UserCount");
			dtNew.Columns.Add("Description");

			dt.DefaultView.RowFilter = "Users=1";
			if (dt.DefaultView.Count > 0)
			{
				DataRow dr = dtNew.NewRow();
				dr["UserCount"] = dt.DefaultView.Count;
				dr["Description"] = "Users with only 1 Device";
				dtNew.Rows.Add(dr);
			}
			dt.DefaultView.RowFilter = "Users=2";
			if (dt.DefaultView.Count > 0)
			{
				DataRow dr = dtNew.NewRow();
				dr = dtNew.NewRow();
				dr["UserCount"] = dt.DefaultView.Count;
				dr["Description"] = "Users with 2 Devices";
				dtNew.Rows.Add(dr);
			}
			dt.DefaultView.RowFilter = "Users=3";
			if (dt.DefaultView.Count > 0)
			{
				DataRow dr = dtNew.NewRow();
				dr = dtNew.NewRow();
				dr["UserCount"] = dt.DefaultView.Count;
				dr["Description"] = "Users with 3 Devices";
				dtNew.Rows.Add(dr);
			}
			dt.DefaultView.RowFilter = "Users=4";
			if (dt.DefaultView.Count > 0)
			{
				DataRow dr = dtNew.NewRow();
				dr = dtNew.NewRow();
				dr["UserCount"] = dt.DefaultView.Count;
				dr["Description"] = "Users with 4 Devices";
				dtNew.Rows.Add(dr);
			}
			dt.DefaultView.RowFilter = "Users>4";
			if (dt.DefaultView.Count > 0)
			{
				DataRow dr = dtNew.NewRow();
				dr = dtNew.NewRow();
				dr["UserCount"] = dt.DefaultView.Count;
				dr["Description"] = "Users with more than 5 Devices";
				dtNew.Rows.Add(dr);
			}
			return dtNew;
		}

		public DataTable SetGrid(int lastmin, int agomin, int moreDevices, int keyUsers)
		{

			DataTable dt = new DataTable();
			if (lastmin == 0 && agomin == 0 && moreDevices == 0 && keyUsers == 0)
			{
				string strQuerry = "SELECT Traveler_Devices.DeviceID, SyncTimeThreshold, OS_Type,OS_Type_Min, LOWER(UserName) UserName, DeviceName, ConnectionState, LastSyncTime, OS_Type, case when isnull(HAPoolName,'')='' then 'NONE' else HAPoolName end as HAPoolName, case when isnull(Client_Build,'')='' then 'NONE' else Client_Build end as Client_Build, NotificationType, ID, DocID, device_type, Access, Security_Policy, wipeRequested, wipeOptions, wipeStatus, case when isnull(SyncType,'')='' then 'NONE' else SyncType end as SyncType, wipeSupported, ServerName, LastUpdated, isnull(MobileUserThreshold.DeviceId,'') Monitoring from Traveler_Devices left outer join MobileUserThreshold on Traveler_Devices.DeviceId = MobileUserThreshold.DeviceId	 order by UserName ";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			else if (lastmin != 0 && agomin == 0 && moreDevices == 0 && keyUsers == 0)
			{
				try
				{
					string strQuerry = "SELECT Traveler_Devices.DeviceID, SyncTimeThreshold, OS_Type,OS_Type_Min, LOWER(UserName) UserName, DeviceName, ConnectionState, LastSyncTime, OS_Type, case when isnull(HAPoolName,'')='' then 'NONE' else HAPoolName end as HAPoolName, case when isnull(Client_Build,'')='' then 'NONE' else Client_Build end as Client_Build, NotificationType, ID, DocID, device_type, Access, Security_Policy, wipeRequested, wipeOptions, wipeStatus, case when isnull(SyncType,'')='' then 'NONE' else SyncType end as SyncType, wipeSupported, ServerName, LastUpdated, isnull(MobileUserThreshold.DeviceId,'') Monitoring FROM [Traveler_Devices] left outer join MobileUserThreshold on Traveler_Devices.DeviceId = MobileUserThreshold.DeviceId where LastSyncTime between  dateadd(mi," + "-" + lastmin + ",GETDATE()) and GETDATE() and IsActive=1  order by UserName";
					//                    string strQuerry = "SELECT distinct *, isnull(MobileUserThreshold.DeviceId,'') Monitoring FROM [Traveler_Devices] ";
					//strQuerry+=" left outer join MobileUserThreshold on Traveler_Devices.DeviceId = MobileUserThreshold.DeviceId where LastSyncTime between  dateadd(mi,-90,GETDATE()) and GETDATE() ";
					//strQuerry += "and ([Traveler_Devices].DeviceID + '-' + convert(varchar,[Traveler_Devices].LastSyncTime)) in (select deviceid + '-' +convert(varchar,max(LastSyncTime)) from Traveler_Devices group by DeviceID)";
					dt = objAdaptor1.FetchData(strQuerry);
				}
				catch (Exception ex)
				{

					throw ex;
				}
			}
			else if (moreDevices > 0)
			{
				string strQuerry = "select Traveler_Devices.DeviceID, SyncTimeThreshold, OS_Type,OS_Type_Min, LOWER(UserName) UserName, DeviceName, ConnectionState, LastSyncTime, OS_Type, case when isnull(HAPoolName,'')='' then 'NONE' else HAPoolName end as HAPoolName, case when isnull(Client_Build,'')='' then 'NONE' else Client_Build end as Client_Build, NotificationType, ID, DocID, device_type, Access, Security_Policy, wipeRequested, wipeOptions, wipeStatus, case when isnull(SyncType,'')='' then 'NONE' else SyncType end as SyncType, wipeSupported, ServerName, LastUpdated, isnull(MobileUserThreshold.DeviceId,'') Monitoring from Traveler_Devices left outer join MobileUserThreshold on Traveler_Devices.DeviceId = MobileUserThreshold.DeviceId where UserName in( select UserName  from Traveler_Devices where IsActive=1 group by UserName having COUNT(UserName)>" + moreDevices.ToString() + ") and IsActive=1   order by UserName";
				dt = objAdaptor1.FetchData(strQuerry);

			}
			else if (keyUsers == 1)
			{

				string strQuerry = "SELECT one.DeviceID, SyncTimeThreshold, OS_Type,OS_Type_Min, LOWER(UserName) UserName, DeviceName, ConnectionState, LastSyncTime, OS_Type, case when isnull(HAPoolName,'')='' then 'NONE' else HAPoolName end as HAPoolName, case when isnull(Client_Build,'')='' then 'NONE' else Client_Build end as Client_Build, NotificationType, ID, DocID, device_type, Access, Security_Policy, wipeRequested, wipeOptions, wipeStatus, case when isnull(SyncType,'')='' then 'NONE' else SyncType end as SyncType, wipeSupported, ServerName, LastUpdated, isnull(two.DeviceId,'') Monitoring FROM  [vitalsigns].[dbo].[Traveler_Devices] AS one INNER JOIN [vitalsigns].[dbo].[MobileUserThreshold] As two ON one.DeviceID = two.DeviceID where IsActive=1 order by UserName";

				dt = objAdaptor1.FetchData(strQuerry);
			}
			else if (keyUsers == 2)
			{
				string strQuerry = "SELECT distinct LOWER(UserName) UserName,DeviceName,Security_Policy,OS_Type,OS_Type_Min,two.SyncTimeThreshold,Os_Type_Min,Device_type,ID,Access,Security_Policy,wipeRequested,wipeOptions,wipeStatus,case when isnull(SyncType,'')='' then 'NONE' else SyncType end as SyncType,one.DeviceId as DeviceID,LastUpdated,case when isnull(HAPoolName,'')='' then 'NONE' else HAPoolName end as HAPoolName,LastSyncTime,ConnectionState,'' As ServerName,case when isnull(Client_Build,'')='' then 'NONE' else Client_Build end as Client_Build, isnull(two.DeviceId,'') Monitoring FROM  [vitalsigns].[dbo].[Traveler_Devices] AS one LEFT OUTER JOIN [vitalsigns].[dbo].[MobileUserThreshold] As two ON one.DeviceID = two.DeviceID where IsActive=1  order by UserName";
				//string strQuerry = "select distinct UserName,DeviceName,Security_Policy,OS_Type,two.SyncTimeThreshold,Os_Type_Min,Device_type,Access,Security_Policy,wipeRequested,wipeOptions,wipeStatus,case when isnull(SyncType,'')='' then 'NONE' else SyncType end as SyncType,";
				//strQuerry += " one.DeviceId as DeviceID,'' as LastUpdated,'' as case when isnull(HAPoolName,'')='' then 'NONE' else HAPoolName end as HAPoolName,ConnectionState,'' As ServerName,case when isnull(Client_Build,'')='' then 'NONE' else Client_Build end as Client_Build, isnull(two.DeviceId,'') Monitoring , ";
				//strQuerry += " (Select MAX(LastSyncTime) from [vitalsigns].[dbo].[Traveler_Devices]  where DeviceID=one.DeviceID group by DeviceID)  as LastSyncTime  FROM  [vitalsigns].[dbo].[Traveler_Devices] AS one LEFT OUTER JOIN [vitalsigns].[dbo].[MobileUserThreshold] As two ON one.DeviceID = two.DeviceID";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			else
			{
				//1/8/2013 NS modified the query below - the argument that should be used in the query is agomin, not lastmin
				//string strQuerry = "SELECT *, isnull(MobileUserThreshold.DeviceId,'') Monitoring FROM [Traveler_Devices] left outer join MobileUserThreshold on Traveler_Devices.DeviceId = MobileUserThreshold.DeviceId where LastSyncTime not between  dateadd(mi," + "-" + agomin + ",GETDATE()) and GETDATE() and IsActive=1";
				string strQuerry = "SELECT Traveler_Devices.DeviceID, SyncTimeThreshold, OS_Type,OS_Type_Min, LOWER(UserName) UserName, DeviceName, ConnectionState, LastSyncTime, OS_Type, case when isnull(HAPoolName,'')='' then 'NONE' else HAPoolName end as HAPoolName, case when isnull(Client_Build,'')='' then 'NONE' else Client_Build end as Client_Build, NotificationType, ID, DocID, device_type, Access, Security_Policy, wipeRequested, wipeOptions, wipeStatus, case when isnull(SyncType,'')='' then 'NONE' else SyncType end as SyncType, wipeSupported, ServerName, LastUpdated, isnull(MobileUserThreshold.DeviceId,'') Monitoring FROM [Traveler_Devices] left outer join MobileUserThreshold on Traveler_Devices.DeviceId = MobileUserThreshold.DeviceId where LastSyncTime <  dateadd(mi," + "-" + agomin + ",GETDATE())  order by UserName ";
				//string strQuerry = "SELECT *, isnull(MobileUserThreshold.DeviceId,'') Monitoring FROM [Traveler_Devices] left outer join MobileUserThreshold on Traveler_Devices.DeviceId = MobileUserThreshold.DeviceId where LastSyncTime not between dateadd(mi," + "-" + agomin + ",GETDATE()) and GETDATE() ";
				//strQuerry += "and ([Traveler_Devices].DeviceID + '-' + convert(varchar,[Traveler_Devices].LastSyncTime)) in (select deviceid + '-' +convert(varchar,max(LastSyncTime)) from Traveler_Devices group by DeviceID)";
				dt = objAdaptor1.FetchData(strQuerry);

			}
			return dt;
		}

		public bool SENDTravelerConsoleCommand(string ServerName, string TellCommand, string user)
		{
			bool insert = false;
			try
			{
				string SqlQuery = " Insert into DominoConsoleCommands(ServerName,Command,Submitter,DateTimeSubmitted) values('" + ServerName + "','" + TellCommand + "','" + user + "','" + DateTime.Now + "')";
				insert = objAdaptor1.ExecuteNonQuery(SqlQuery);
			}
			catch (Exception)
			{

				insert = false;
			}

			return insert;
		}
		public DataTable SetGraphForMailFileOpens(string servername, string mailservername, string interval)
		{
			DataTable dt = new DataTable();
			try
			{
				//4/30/2014 NS modified - 24 hour selection instead of today's data only
				/*
				string strQuerry = "SELECT ID, TravelerServerName, MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
					"FROM TravelerStats " +
					"WHERE TravelerServerName = '" + servername + "' " +
					"AND MailServerName = '" + mailservername + "' " +
					"AND Interval='" + interval + "' " +
					"AND DATEDIFF(dd,0,DateUpdated)=DATEDIFF(dd,0,GETDATE()) " +
					"AND MailServerName IS NOT NULL AND MailServerName != ''";
				 */
				string strQuerry = "SELECT ID, TravelerServerName, MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
					"FROM TravelerStats " +
					"WHERE TravelerServerName = '" + servername + "' " +
					"AND MailServerName = '" + mailservername + "' " +
					//12/31/2014 NS commented out for VSPLUS-1283
					//"AND Interval='" + interval + "' " +
					"AND DateUpdated>=DATEADD(hh,-24,GETDATE()) " +
					"AND MailServerName IS NOT NULL AND MailServerName != ''";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		public DataTable SetGraphForMailFileOpensCumulative(string servername, string mailservername, string interval)
		{
			DataTable dt = new DataTable();
			try
			{
				//4/30/2014 NS modified - 24 hour selection instead of today's data only
				/*
				string strQuerry = "SELECT ID, TravelerServerName, " +
					"MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
					"FROM TravelerStats " +
					"WHERE TravelerServerName  = '" + servername + "' " +
					"AND MailServerName = '" + mailservername + "' " +
					"AND Interval='" + interval + "' " +
					"AND DATEDIFF(dd,0,DateUpdated)=DATEDIFF(dd,0,GETDATE()) AND MailServerName IS NOT NULL AND " +
					"MailServerName != '' ";
				 */
				string strQuerry = "SELECT ID, TravelerServerName, " +
					"MailServerName, Interval, Delta, OpenTimes, DateUpdated " +
					"FROM TravelerStats " +
					"WHERE TravelerServerName  = '" + servername + "' " +
					"AND MailServerName = '" + mailservername + "' " +
					//12/31/2014 NS commented out for VSPLUS-1283
					//"AND Interval='" + interval + "' " +
					"AND DateUpdated>=DATEADD(hh,-24,GETDATE()) " +
					"AND MailServerName IS NOT NULL AND MailServerName != '' ";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		public DataTable SetGridForTravelerInterval(string servername)
		{
			DataTable dt = new DataTable();
			try
			{
				//This query may need to be updated with a function call to abbreviate Traveler server name
				//3/1/2013 NS modified the query below to include traveler server name check into the max date selection
				//5/6/2014 NS modified the query for VSPLUS-465
				/*
				string strQuerry = "SELECT MailServerName, ISNULL([000-001],0) as [000-001], ISNULL([001-002],0) as [001-002], ISNULL([002-005],0) as [002-005], ISNULL([005-010],0) as [005-010], ISNULL([010-030],0) as [010-030], ISNULL([030-060],0) as [030-060], ISNULL([060-120],0) as [060-120], ISNULL([120-INF],0) as [120-INF], DateUpdated , Location " +
					"FROM (SELECT MailServerName, Interval, Delta, DateUpdated, Location " +
					"  FROM TravelerStats INNER JOIN Servers s1 ON MailServerName =ServerName " +
					"  INNER JOIN DominoServers ON ServerID = s1.ID INNER JOIN Locations l1 ON s1.LocationID=l1.ID " +

					"  WHERE MailServerName != '' AND TravelerServerName = '" + servername + "' " +
					"  AND DateUpdated = (SELECT MAX(DateUpdated) FROM TravelerStats " +
					"  WHERE TravelerServerName = '" + servername + "')) " +

					"  AS SourceTable " +
					"PIVOT " +
					"(AVG(Delta) " +
					"FOR Interval IN ([000-001], [001-002], [002-005], [005-010], [010-030], [030-060], " +
					"[060-120], [120-INF]) " +
					") AS PivotTable";
				 */
				//10/14/2014 NS modified for VSPLUS-1013
				string strQuerry = "SELECT MailServerName, CASE WHEN [000-001] < 0 THEN 0 ELSE ISNULL([000-001],0) END as [000-001], " +
					"CASE WHEN [001-002] < 0 THEN 0 ELSE ISNULL([001-002],0) END as [001-002], " +
					"CASE WHEN [002-005] < 0 THEN 0 ELSE ISNULL([002-005],0) END as [002-005], " +
					"CASE WHEN [005-010] < 0 THEN 0 ELSE ISNULL([005-010],0) END as [005-010], " +
					"CASE WHEN [010-030] < 0 THEN 0 ELSE ISNULL([010-030],0) END as [010-030], " +
					"CASE WHEN [030-060] < 0 THEN 0 ELSE ISNULL([030-060],0) END as [030-060], " +
					"CASE WHEN [060-120] < 0 THEN 0 ELSE ISNULL([060-120],0) END as [060-120], " +
					"CASE WHEN [120-INF] < 0 THEN 0 ELSE ISNULL([120-INF],0) END as [120-INF], " +
					"DateUpdated , Location " +
					"FROM (SELECT MailServerName, Interval, Delta, DateUpdated, ISNULL(Location,'Unavailable') Location " +
					"  FROM TravelerStats LEFT OUTER JOIN Servers s1 ON MailServerName = ServerName " +
					"  LEFT OUTER JOIN Locations l1 ON s1.LocationID=l1.ID " +
					"  WHERE MailServerName != '' AND TravelerServerName = '" + servername + "' " +
					"  AND DateUpdated = (SELECT MAX(DateUpdated) FROM TravelerStats " +
					"  WHERE TravelerServerName = '" + servername + "')) " +
					"  AS SourceTable " +
					"PIVOT " +
					"(AVG(Delta) " +
					"FOR Interval IN ([000-001], [001-002], [002-005], [005-010], [010-030], [030-060], " +
					"[060-120], [120-INF]) " +
					") AS PivotTable";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		//4/15/2014 NS added
		public DataTable SelectServerNamesForGrid()
		{
			DataTable dt = new DataTable();
			try
			{
				string strQuerry = "SELECT DISTINCT ServerName FROM [Traveler_Devices] ORDER BY ServerName ";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		//8/19/2014 NS added for VSPLUS-884
		public DataTable SetGraphForDeviceSyncs(string servername)
		{
			DataTable dt = new DataTable();
			try
			{
				string strQuerry = "SELECT ID, StatValue, Date FROM DominoDailyStats " +
					"WHERE StatName='Traveler.IncrementalDeviceSyncs' AND ServerName = '" + servername + "' " +
					"AND Date>=DATEADD(hh,-24,GETDATE())";
				dt = objAdaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		//8/11/2015 NS added for VSPLUS-2029
		public DataTable GetKeyUserDevices(string deviceid)
		{
			DataTable dt = new DataTable();
			try
			{
				//8/28/2015 NS modified for VSPLUS-2110
				//11/30/2015 NS modified for VSPLUS-2227
				string strQuerry = "SELECT Traveler_Devices.ID,Traveler_Devices.DeviceID, SyncTimeThreshold, UserName, DeviceName, LastSyncTime, " +
					"DATEDIFF (mi,LastSyncTime,GETDATE()) AS LastSyncMinActual," +
					"CASE WHEN DATEDIFF(mi,LastSyncTime,GETDATE()) > 240 THEN 240 ELSE DATEDIFF (mi,LastSyncTime,GETDATE()) END AS LastSyncMin," +
					"CASE WHEN DATEDIFF (mi,LastSyncTime,GETDATE()) > SyncTimeThreshold THEN 'Overdue' ELSE 'OK' END AS Status " +
					"FROM Traveler_Devices LEFT OUTER JOIN MobileUserThreshold ON Traveler_Devices.DeviceId = MobileUserThreshold.DeviceId " +
					"WHERE ISNULL(MobileUserThreshold.DeviceId,'')!= '' AND IsActive=1 ";
				if (deviceid != "")
				{
					strQuerry += "AND Traveler_Devices.DeviceID='" + deviceid + "' ";
				}
				strQuerry += "ORDER BY LastSyncMin DESC";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		//10/8/2015 NS added for VSPLUS-2242
		public DataTable GetMaxLastUpdated()
		{
			DataTable dt = new DataTable();
			try
			{
				string strQuerry = "SELECT CASE WHEN DATEADD(dd, 0, DATEDIFF(dd, 0, MAX(LastUpdated))) >= " +
					"DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) THEN 'true' ELSE 'false' END IsCurrent " +
					"FROM Traveler_Devices LEFT OUTER JOIN MobileUserThreshold " +
					"ON Traveler_Devices.DeviceId = MobileUserThreshold.DeviceId ";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		//10/8/2015 NS added for VSPLUS-2208
		public DataTable SetGraphForJavaMemory(string paramval, string servername)
		{
			DataTable dt = new DataTable();
			if (paramval == "hh")
			{
				try
				{
					string strQuerry = "SELECT Date, dd.[StatValue] " +
					"FROM [VSS_Statistics].[dbo].[DominoDailyStats] dd JOIN [VitalSigns].[dbo].[Status] s ON " +
					"dd.[ServerName] = s.[Name] JOIN [VitalSigns].[dbo].[Traveler_Status] ts ON " +
					"s.[Name] = ts.[ServerName] WHERE s.[Name] = '" + servername + "' " +
					"and Date>=DATEADD(hh,-24,GETDATE()) " +
					"and s.[SecondaryRole] = 'Traveler' and dd.[StatName] = 'Traveler.Memory.Java.Current' order by Date asc";
					dt = objAdaptor.FetchData(strQuerry);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			else
			{
				try
				{
					string strQuerry = "SELECT Date,dd.[StatValue] " +
					"FROM [VSS_Statistics].[dbo].[DominoDailyStats] dd JOIN [VitalSigns].[dbo].[Status] s ON " +
					"dd.[ServerName] = s.[Name] JOIN [VitalSigns].[dbo].[Traveler_Status] ts ON " +
					"s.[Name] = ts.[ServerName] WHERE s.[Name] = '" + servername + "' and " +
					"dd.[Date] >= DATEADD (dd , -30 ,GETDATE()) and " +
					"s.[SecondaryRole] = 'Traveler' and dd.[StatName] = 'Traveler.Memory.Java.Current' " +
					"order by dd.[Date] asc ";
					dt = objAdaptor.FetchData(strQuerry);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			return dt;
		}

		//10/8/2015 NS added for VSPLUS-2208
		public DataTable SetGraphForCMemory(string paramval, string servername)
		{
			DataTable dt = new DataTable();
			if (paramval == "hh")
			{
				try
				{
					string strQuerry = "SELECT Date, dd.[StatValue] " +
					"FROM [VSS_Statistics].[dbo].[DominoDailyStats] dd JOIN [VitalSigns].[dbo].[Status] s ON " +
					"dd.[ServerName] = s.[Name] JOIN [VitalSigns].[dbo].[Traveler_Status] ts ON " +
					"s.[Name] = ts.[ServerName] WHERE s.[Name] = '" + servername + "' " +
					"and Date>=DATEADD(hh,-24,GETDATE()) " +
					"and s.[SecondaryRole] = 'Traveler' and dd.[StatName] = 'Traveler.Memory.C.Current' order by Date asc";
					dt = objAdaptor.FetchData(strQuerry);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			else
			{
				try
				{
					string strQuerry = "SELECT Date,dd.[StatValue] " +
					"FROM [VSS_Statistics].[dbo].[DominoDailyStats] dd JOIN [VitalSigns].[dbo].[Status] s ON " +
					"dd.[ServerName] = s.[Name] JOIN [VitalSigns].[dbo].[Traveler_Status] ts ON " +
					"s.[Name] = ts.[ServerName] WHERE s.[Name] = '" + servername + "' and " +
					"dd.[Date] >= DATEADD (dd , -30 ,GETDATE()) and " +
					"s.[SecondaryRole] = 'Traveler' and dd.[StatName] = 'Traveler.Memory.C.Current' " +
					"order by dd.[Date] asc ";
					dt = objAdaptor.FetchData(strQuerry);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			return dt;
		}

		public DataTable SetGrid()
		{
			DataTable dt = new DataTable();
			try
			{
				//1/27/2016 NS modified for VSPLUS-2531
				string strQuerry = "  SELECT TS.ID,TS.ServerName As Name, TS.Status, TS.HeartBeat, " +
				 " TS.HTTP_Status,TS.Details,TSR.Details as Reasons, TS.TravelerServlet" +
						   " FROM [VitalSigns].[dbo].[Traveler_Status] as TS left outer join TravelerStatusReasons as TSR on TS.ServerName=TSR.ServerName ORDER BY TS.ServerName";
				dt = objAdaptor1.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
	}
}
