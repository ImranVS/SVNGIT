using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Xml.Linq;

namespace VSWebDAL.DashboardDAL
{
	public class Office365DAL
	{
		private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
		private Adaptor adaptor = new Adaptor();
		private static Office365DAL _self = new Office365DAL();
		int id;
		public static Office365DAL Ins
		{
			get
			{
				return _self;
			}
		}



        public DataTable get0635grid(string Name)
        {
            DataTable HAStatus = new DataTable();
			string SqlQuery;
            try
            {
				if (Name != "All")
				{
					SqlQuery = "select sr.name AccountName, n.Id,sr.Mode,n.Name NodeName,ln.Location,s.Status,s.LastUpdate FROM   [vitalsigns].[dbo].Status s,[vitalsigns].[dbo].Nodes n,[vitalsigns].[dbo].O365Nodes o365,[vitalsigns].[dbo].Locations ln,[vitalsigns].[dbo].O365Server sr where s.Name=sr.Name and sr.ID=o365.O365ServerID and o365.NodeID=n.ID and n.LocationID=ln.ID and s.Location=ln.Location and sr.Name='" + Name + "'";
					 HAStatus = adaptor.FetchData(SqlQuery);
				}
				else
				{
					SqlQuery = "select sr.name AccountName, n.Id,sr.Mode,n.Name NodeName,ln.Location,s.Status,s.LastUpdate FROM   [vitalsigns].[dbo].Status s,[vitalsigns].[dbo].Nodes n,[vitalsigns].[dbo].O365Nodes o365,[vitalsigns].[dbo].Locations ln,[vitalsigns].[dbo].O365Server sr where s.Name=sr.Name and sr.ID=o365.O365ServerID and o365.NodeID=n.ID and n.LocationID=ln.ID and s.Location=ln.Location and sr.Name in (select name from O365Server)";
					HAStatus = adaptor.FetchData(SqlQuery);
				}
            }
            catch
            {
            }
            return HAStatus;
        }

		public DataTable FillMailboxChart()
		{
			DataTable dt = new DataTable();
			try
			{
				dt.Columns.Add("Time", typeof(String));
				dt.Columns.Add("Count",typeof(Int32));

				DataRow row = dt.NewRow();
				row["Time"] = "0-1 week";
				row["Count"] = 30;
				dt.Rows.Add(row);

				row = dt.NewRow();
				row["Time"] = "1-2 weeks";
				row["Count"] = 60;
				dt.Rows.Add(row);

				row = dt.NewRow();
				row["Time"] = "2-4 weeks";
				row["Count"] = 10;
				dt.Rows.Add(row);

				row = dt.NewRow();
				row["Time"] = "4+ weeks";
				row["Count"] = 2;
				dt.Rows.Add(row);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		public DataTable FillDevicesChart()
		{
			DataTable dt = new DataTable();
			try
			{
				dt.Columns.Add("Time", typeof(String));
				dt.Columns.Add("Count", typeof(Int32));

				DataRow row = dt.NewRow();
				row["Time"] = "0-1 week";
				row["Count"] = 30;
				dt.Rows.Add(row);

				row = dt.NewRow();
				row["Time"] = "1-2 weeks";
				row["Count"] = 60;
				dt.Rows.Add(row);

				row = dt.NewRow();
				row["Time"] = "2-4 weeks";
				row["Count"] = 100;
				dt.Rows.Add(row);

				row = dt.NewRow();
				row["Time"] = "4+ weeks";
				row["Count"] = 9;
				dt.Rows.Add(row);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
		public DataTable Groupgrid()
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = "Select G.GroupId,G.GroupName,G.GroupType, MU.DisplayName,UG.UserPrincipalName,o365.Name AccountName from O365Groups G INNER JOIN O365UserGroups UG on G.GroupId=UG.GroupId INNER JOIN O365MSOLUsers MU on UG.UserPrincipalName=MU.UserPrincipalName INNER JOIN O365Server o365 on o365.ID=MU.ServerId";

				dt = adaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		public DataTable GetOffice365Userlicensesstatus()
		{
			DataTable dt = new DataTable();
			DataTable resultTable = new DataTable();
			try
			{
				string SqlQuery = "select DisplayName, XMLConfiguration,ServerId from O365UserswithLicensesandServices O365UL inner join  O365Server o365s on O365UL.ServerId = o365s.ID where o365s.Enabled=1";

				dt = adaptor.FetchData(SqlQuery);
				resultTable.Columns.Add("DisplayName", typeof(string));
				resultTable.Columns.Add("ServerId", typeof(int));
				foreach (DataRow row in dt.Rows)
				{
					string xmlConfiguration =Convert.ToString( row["XMLConfiguration"]);
					if (!string.IsNullOrEmpty(xmlConfiguration))
					{
						XDocument xdoc = XDocument.Parse(xmlConfiguration);
						var result = from x in xdoc.Descendants("Row")
									 select new
									 {
										 Name = x.Attribute("Key").Value,
										 Value = x.Attribute("Value").Value
									 };

						foreach (var item in result)
						{
							if (!resultTable.Columns.Contains(item.Name))
								resultTable.Columns.Add(item.Name, typeof(String));
						}
						DataRow newRow = resultTable.NewRow();
						newRow["DisplayName"] = Convert.ToString(row["DisplayName"]);
						newRow["ServerId"] = Convert.ToInt32(row["ServerId"]);
						foreach (var item in result)
						{
							newRow[item.Name] = item.Value;
						}
						resultTable.Rows.Add(newRow);

					}
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
			return resultTable;
		}
		public DataTable Usersettingsgrid()
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = "Select ServerId,DisplayName,UserPrincipalName,StrongPasswordRequired,PasswordNeverExpires from O365MSOLUsers";
				////string SqlQuery = " select distinct serverId, (select count(*) from O365MSOLUsers where StrongPasswordRequired=1)as Activestrongpasswords ,(select count(*) from O365MSOLUsers where StrongPasswordRequired=0)as InactiveStrongpasswords from O365MSOLUsers Group by ServerId";
				//string SqlQuery = "SELECT serverId, count( * ) as NumberofInActivePasswords FROM O365MSOLUsers  WHERE StrongPasswordRequired=0 Group by serverId Union all  SELECT serverId, count( * ) as NumberofActivePasswords FROM O365MSOLUsers  WHERE StrongPasswordRequired=1 Group by serverId";
				dt = adaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
		public DataTable PasswordsChart()
		{
			DataTable dt = new DataTable();
			try
			{
				//string SqlQuery = "Select ServerId,DisplayName,UserPrincipalName,StrongPasswordRequired,PasswordNeverExpires from O365MSOLUsers";
				//string SqlQuery = " select distinct serverId, (select count(*) from O365MSOLUsers where StrongPasswordRequired=1)as Activestrongpasswords ,(select count(*) from O365MSOLUsers where StrongPasswordRequired=0)as InactiveStrongpasswords from O365MSOLUsers Group by ServerId";
				//string SqlQuery = "SELECT serverId, count( * ) as NumberofInActivePasswords FROM O365MSOLUsers  WHERE StrongPasswordRequired=0 Group by serverId Union all  SELECT serverId, count( * ) as NumberofActivePasswords FROM O365MSOLUsers  WHERE StrongPasswordRequired=1 Group by serverId";
				string SqlQuery = "SELECT StrongPasswordRequired, count( * ) as NumberOfPasswords FROM O365MSOLUsers  WHERE StrongPasswordRequired=0 Group by StrongPasswordRequired Union all SELECT StrongPasswordRequired , count( * ) as NumberOfPasswords FROM O365MSOLUsers  WHERE StrongPasswordRequired=1 Group by StrongPasswordRequired";
				dt = adaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
		public DataTable PasswordsNeverexpiresChart()
		{
			DataTable dt = new DataTable();
			try
			{
				//string SqlQuery = "Select ServerId,DisplayName,UserPrincipalName,StrongPasswordRequired,PasswordNeverExpires from O365MSOLUsers";
				//string SqlQuery = " select distinct serverId, (select count(*) from O365MSOLUsers where StrongPasswordRequired=1)as Activestrongpasswords ,(select count(*) from O365MSOLUsers where StrongPasswordRequired=0)as InactiveStrongpasswords from O365MSOLUsers Group by ServerId";
				//string SqlQuery = "SELECT serverId, count( * ) as NumberofInActivePasswords FROM O365MSOLUsers  WHERE StrongPasswordRequired=0 Group by serverId Union all  SELECT serverId, count( * ) as NumberofActivePasswords FROM O365MSOLUsers  WHERE StrongPasswordRequired=1 Group by serverId";
				string SqlQuery = " SELECT PasswordNeverExpires, count( * ) as NumberOfPasswords FROM O365MSOLUsers  WHERE PasswordNeverExpires=0 Group by PasswordNeverExpires Union all SELECT PasswordNeverExpires , count( * ) as NumberOfPasswords FROM O365MSOLUsers  WHERE PasswordNeverExpires=1 Group by PasswordNeverExpires";
				dt = adaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
		public DataTable FillStatListView(string Type,string serverName)
		{
			DataTable dt = new DataTable();
			try
			{
				dt.Columns.Add("Stat", typeof(String));
				dt.Columns.Add("Value", typeof(String));


				if (Type == "Mailboxes")
				{
					//DataRow row = dt.NewRow();
					//row["Stat"] = "Number of Mailboxes";
					//row["Value"] = "155";
					//dt.Rows.Add(row);

					////row = dt.NewRow();
					////row["Stat"] = "Number of Licenses";
					////row["Value"] = "2000";
					////dt.Rows.Add(row);

					////row = dt.NewRow();
					////row["Stat"] = "Number of Available Licenses";
					////row["Value"] = "100";
					////dt.Rows.Add(row);

					//row = dt.NewRow();
					//row["Stat"] = "Size of Mailboxes";
					//row["Value"] = "150 TB";
					//dt.Rows.Add(row);

					//row = dt.NewRow();
					//row["Stat"] = "Total Number of Items";
					//row["Value"] = "150000";
					//dt.Rows.Add(row);

					//row = dt.NewRow();
					//row["Stat"] = "Average Size of a Mailbox";
					//row["Value"] = "1.5 GB";
					//dt.Rows.Add(row);

					//row = dt.NewRow();
					//row["Stat"] = "Average Size of a Item";
					//row["Value"] = ".1 GB";
					//dt.Rows.Add(row);
					string SqlQuery = "select COUNT(DisplayName) NoOfMailBoxes,SUM(itemcount) TotalItemCount,SUM(totalitemsizeinmb) TotalItemSizeInMB,round(AVG(totalitemsizeinmb),2) AvgSizeOfMailBox,round(AVG(itemcount),2) AvgCountOfItems from ExchangeMailFiles Where Server='" + serverName +"'";
					   
					dt = objAdaptor.FetchData(SqlQuery);
				}
				else if (Type == "General")
				{
					string SqlQuery = "select SUM(activeunits),SUM(consumedunits),SUM(warningunits) from Office365AccountStats  Where ServerId =(Select id from O365Server where Name='" + serverName +"')";

					dt = adaptor.FetchData(SqlQuery);
				}
				else if (Type == "Devices")
				{
					//DataRow row = dt.NewRow();
					//row["Stat"] = "Number of Devices";
					//row["Value"] = "55";
					//dt.Rows.Add(row);

					//row = dt.NewRow();
					//row["Stat"] = "Number of Devices not synced in the past 14 days";
					//row["Value"] = "15";
					//dt.Rows.Add(row);

					//row = dt.NewRow();
					//row["Stat"] = "Number of Monitored Devices";
					//row["Value"] = "27";
					//dt.Rows.Add(row);

					//row = dt.NewRow();
					//row["Stat"] = "Number of Monitored Devices not synced in the past 14 days";
					//row["Value"] = "6";
					//dt.Rows.Add(row);

					string SqlQuery = "select COUNT(Username) DeviceCount,(select count(DATEDIFF(d, lastsynctime,getdate())) as 'da' from Traveler_Devices where ServerName='" + serverName + "' and  DATEDIFF(d, lastsynctime,getdate()) >9) as DeviceCountNotSynced,";
SqlQuery  +="(select count(*) from MobileUserThreshold) as MonitoredDevices,(select count(DATEDIFF(d, lastsynctime,getdate())) as 'da' from Traveler_Devices TD,MobileUserThreshold MBU where TD.ServerName='" + serverName +"' and MBU.DeviceId=TD.DeviceID and  DATEDIFF(d, lastsynctime,getdate()) >9) as MonitoredDevicesNotSynced";
SqlQuery  +=" from Traveler_Devices";

					dt = adaptor.FetchData(SqlQuery);


				
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

		public DataTable FillMailboxUsage()
		{
			DataTable dt = new DataTable();

			try
			{
				dt.Columns.Add("MailType", typeof(String));
				dt.Columns.Add("Count", typeof(Int32));

				DataRow row = dt.NewRow();
				row["MailType"] = "Sent Mail";
				row["Count"] = 410;
				dt.Rows.Add(row);

				row = dt.NewRow();
				row["MailType"] = "Sent Spam";
				row["Count"] = 25;
				dt.Rows.Add(row);

				row = dt.NewRow();
				row["MailType"] = "Received Spam";
				row["Count"] = 10;
				dt.Rows.Add(row);

				row = dt.NewRow();
				row["MailType"] = "Received Mail";
				row["Count"] = 900;
				dt.Rows.Add(row);
			}
			catch (Exception)
			{ }

			return dt;
		}

		public DataTable SetGraphForMailFiles( string ServerName)
		{
			DataTable dt = new DataTable();
			string StrQuery = "select distinct top(5) DisplayName  Title, TotalItemSizeInMB,ROUND((TotalItemSizeInMB/1000),4) as TotalItemSizeInGB from [VSS_Statistics].[dbo].[ExchangeMailfiles] Where Server='" + ServerName + "' order by TotalItemSizeInMB desc";
			dt = objAdaptor.FetchData(StrQuery);
			
			return dt;
		}

		public DataTable SetGraphLastLogonUsers(string Name)
		{
			DataTable dt = new DataTable();


			string strQuerry = "SELECT COUNT(*) As No_of_Users,'1 day ago' As Duration FROM O365AdditionalMailDetails where Server='" + Name +"' and lastlogontime between  dateadd(dd,-1 ,GETDATE()) and GETDATE() having count(*) >0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'2 days ago' As Duration FROM O365AdditionalMailDetails where Server='" + Name + "' and lastlogontime between  dateadd(dd,-2 ,GETDATE()) and dateadd(dd,-1 ,GETDATE()) having count(*) >0";
				
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'7 days ago' As Duration FROM O365AdditionalMailDetails where Server='" + Name + "' and lastlogontime between  dateadd(dd,-7 ,GETDATE()) and dateadd(dd,-2 ,GETDATE()) having count(*) >0";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*) As No_of_Users,'more than 7 days ago' As Duration FROM O365AdditionalMailDetails where Server='" + Name + "' and lastlogontime <  dateadd(dd,-7 ,GETDATE()) having count(*) >0";
				dt = objAdaptor.FetchData(strQuerry);
			return dt;
		}
		public DataTable SetGraphForSyncType(string Name)
		{
			DataTable dt = new DataTable();

			string strQuerry = "SELECT COUNT(*) As No_of_Users,' Within 15 mins.' As Duration FROM [Traveler_Devices] where ServerName='" + Name + "' and LastSyncTime between  dateadd(mi,-15 ,GETDATE()) and GETDATE() having count(*) >0 ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*)  As No_of_Users,' Between 15-30 mins.' As Duration FROM [Traveler_Devices] where ServerName='" + Name + "' and LastSyncTime between  ";
				strQuerry += " dateadd(mi,-30 ,GETDATE()) and dateadd(mi,-15 ,GETDATE()) having count(*) >0 ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*)  As No_of_Users,' Between 30-60 mins.' As Duration FROM [Traveler_Devices] where ServerName='" + Name + "' and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-60 ,GETDATE()) and dateadd(mi,-30 ,GETDATE())  having count(*) >0 ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*)  As No_of_Users,' Between 60-120 mins.' As Duration FROM [Traveler_Devices] where ServerName='" + Name + "' and LastSyncTime between   ";
				strQuerry += " dateadd(mi,-120 ,GETDATE()) and dateadd(mi,-60 ,GETDATE()) having count(*) >0 ";
				strQuerry += " union ";
				strQuerry += " SELECT COUNT(*)  As No_of_Users,' Greater than 120 mins.' As Duration FROM [Traveler_Devices] where ServerName='" + Name + "' and LastSyncTime   ";
				strQuerry += " < DATEADD(MINUTE, -120, GETDATE()) having count(*) >0 ";
				dt = adaptor.FetchData(strQuerry);
				return dt;
		}
		public DataTable SetGraphDeviceTypes(string Name)
		{
			DataTable dt = new DataTable();


			string strQuerry = "SELECT WindowsUsers as No_of_Users,'Windows PC' as DeviceType from O365LYNCDevices where ServerId=(Select ID from O365Server where Name='" + Name + "')";
			strQuerry += " union ";
			strQuerry += " SELECT WindowsPhoneUsers as No_of_Users,'Windows Phone' as DeviceType from O365LYNCDevices where ServerId=(Select ID from O365Server where Name='" + Name + "')";
			strQuerry += " union ";
			strQuerry += " SELECT AndroidUsers as No_of_Users,'Android' as DeviceType from O365LYNCDevices where ServerId=(Select ID from O365Server where Name='" + Name + "')";
			strQuerry += " union ";
			strQuerry += " SELECT iPhoneUsers as No_of_Users,'iPhone' as DeviceType from O365LYNCDevices where ServerId=(Select ID from O365Server where Name='" + Name + "')";
			strQuerry += " union ";
			strQuerry += " SELECT iPadUsers as No_of_Users,'iPad' as DeviceType from O365LYNCDevices where ServerId=(Select ID from O365Server where Name='" + Name + "')";

			dt = adaptor.FetchData(strQuerry);
			return dt;
		}

		public DataTable SetGraphP2PSessions(string Name)
		{
			DataTable dt = new DataTable();


			string strQuerry = "SELECT P2PIMSessions as No_of_Users,'IM' as DeviceType from O365LYNCP2PSessionReport Where ServerId=(Select ID from O365Server where Name='" + Name +"')";
			strQuerry += " union ";
			strQuerry += " SELECT P2PAudioSessions as No_of_Users,'Audio' as DeviceType from O365LYNCP2PSessionReport where ServerId=(Select ID from O365Server where Name='" + Name + "')";
			strQuerry += " union ";
			strQuerry += " SELECT P2PVideoSessions as No_of_Users,'Video' as DeviceType from O365LYNCP2PSessionReport where ServerId=(Select ID from O365Server where Name='" + Name + "')";
			strQuerry += " union ";
			strQuerry += " SELECT P2PApplicationSharingSessions as No_of_Users,'Application Sharing' as DeviceType from O365LYNCP2PSessionReport where ServerId=(Select ID from O365Server where Name='" + Name + "')";
			strQuerry += " union ";
			strQuerry += " SELECT P2PFileTransferSessions as No_of_Users,'File Transfer' as DeviceType from O365LYNCP2PSessionReport where ServerId=(Select ID from O365Server where Name='" + Name + "')";

			dt = adaptor.FetchData(strQuerry);
			return dt;
		}

		public DataTable SetGraphAVSessions(string Name)
		{
			DataTable dt = new DataTable();


			string strQuerry = "SELECT TotalAudioMinutes as No_of_Users,'Audio' as DeviceType from O365LYNCPAVTimeReport Where ServerId=(Select ID from O365Server where Name='" + Name + "')";
			strQuerry += " union ";
			strQuerry += " SELECT TotalVideoMinutes as No_of_Users,'Video' as DeviceType from O365LYNCPAVTimeReport Where ServerId=(Select ID from O365Server where Name='" + Name + "')";
			
			dt = adaptor.FetchData(strQuerry);
			return dt;
		}

		public DataTable SetGraphConfReport(string Name)
		{
			DataTable dt = new DataTable();


			string strQuerry = "SELECT AVConferences as No_of_Users,'AV' as DeviceType from O365LYNCConferenceReport Where ServerId=(Select ID from O365Server where Name='" + Name + "')";
			strQuerry += " union ";
			strQuerry += " SELECT IMConferences as No_of_Users,'IM' as DeviceType from O365LYNCConferenceReport Where ServerId=(Select ID from O365Server where Name='" + Name + "')";
			strQuerry += " union ";
			strQuerry += " SELECT ApplicationSharingConferences as No_of_Users,'Application Sharing' as DeviceType from O365LYNCConferenceReport Where ServerId=(Select ID from O365Server where Name='" + Name + "')";
			strQuerry += " union ";
			strQuerry += " SELECT WebConferences as No_of_Users,'Web' as DeviceType from O365LYNCConferenceReport Where ServerId=(Select ID from O365Server where Name='" + Name + "')";
			strQuerry += " union ";
			strQuerry += " SELECT TelephonyConferences as No_of_Users,'Telephony' as DeviceType from O365LYNCConferenceReport Where ServerId=(Select ID from O365Server where Name='" + Name + "')";

			dt = adaptor.FetchData(strQuerry);
			return dt;
		}

        //1/15/2015 NS added for VSPLUS-1316
        public DataTable GetHealthAssessmentStatusDetails(string TypeAndName,string servername)
        {
            DataTable HAStatus = new DataTable();
			string SqlQuery;
            try
            {
				if (servername != "All")
				{
					 SqlQuery = "select * FROM [vitalsigns].[dbo].StatusDetail where TypeAndName= '" + TypeAndName + "'";
				}
				else
				{
					 SqlQuery = "select * FROM [vitalsigns].[dbo].StatusDetail";
				}
                HAStatus = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return HAStatus;
        }

        //1/16/2015 NS added for VSPLUS-1316
        public DataTable GetMailTestsResponseTimes(string  nodename,string servername)
        {
            
            
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "select StatName,Date,ROUND(StatValue,1) StatValue FROM MicrosoftDailyStats where ServerTypeId=21 and " +
                    "StatName IN ('POP@" + nodename + "  ','IMAP@" + nodename + "','SMTP@" + nodename + "') and " +
					"ServerName='"+servername+"' and "+
                    "DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE())";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

		public DataTable GetDirSyncStats(string nodename,string accountName)
		{


			DataTable dt = new DataTable();
			try
			{
				//if (accountName != "All")
				//{
				//    string SqlQuery = "select StatName,Date,ROUND(StatValue,1) StatValue FROM MicrosoftDailyStats where ServerTypeId=21 and " +
				//        "StatName IN ('DirSyncEstimated@" + nodename + "  ','DirSyncActual@" + nodename + "') and ServerName='" + accountName + "' and " +
				//        "DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE())";
				//    dt = objAdaptor.FetchData(SqlQuery);
				//}
				//else
				//{
					//string SqlQuery = "select StatName,isnull(Max(statvalue),0) as maxval,,CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00') as dtfrom,Date,ROUND(StatValue,1) StatValue FROM MicrosoftDailyStats where ServerTypeId=21 and " +
					//    "StatName IN ('DirSyncEstimated@" + nodename + "  ','DirSyncActual@" + nodename + "') and ServerName in (select name from [vitalsigns].[dbo].O365Server) and Date>=dtfrom and " +
					//    "DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE())";
					//dt = objAdaptor.FetchData(SqlQuery);
					//==================soma=============================
					//string SqlQuery = "select StatName,Date,ROUND(StatValue,1) maxval,CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00') as dtfrom  FROM MicrosoftDailyStats where ServerTypeId=21 and " +
					//"StatName IN ('DirSyncEstimated@" + nodename + "  ','DirSyncActual@" + nodename + "') and ServerName in (select name from [vitalsigns].[dbo].O365Server where Mode='Dir Sync'  ) and  " +
					//"DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE())";
					//dt = objAdaptor.FetchData(SqlQuery);
					///=============soma===================================
					//string SqlQuery = "Select  isnull(Max(statvalue),0) as maxval,CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00') as dtfrom from MicrosoftDailyStats where ServerName in (select name from [vitalsigns].[dbo].O365Server) and statname in  ('DirSyncEstimated@" + nodename + "  ','DirSyncActual@" + nodename + "')  group by CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00')";
					//                dt = objAdaptor.FetchData(SqlQuery);
				//--and [Date] >= @dtfrom
				//--and [Date] < @dtto
					dt = objAdaptor.FetchOffice365HourlyVals(nodename, System.DateTime.Now, accountName);
					
				//}
			}
			catch
			{
			}
			return dt;
		}
        //1/16/2015 NS added for VSPLUS-1316
        public DataTable GetMailScenarioTestsResponseTimes(string nodename,string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "select StatName,Date,ROUND(StatValue,1) StatValue FROM MicrosoftDailyStats where ServerTypeId=21 and " +
                    "StatName IN ('MailLatency@" + nodename + "','MailFlow@" + nodename + "','Inbox@" + nodename + "','ComposeEmail@" + nodename + "') and " +
					"ServerName = '"+servername+"' and " +
                    "DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE())";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetOneDriveStats(string nodename,string servername)
        {

            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "select StatName,Date,ROUND(StatValue,1) StatValue FROM MicrosoftDailyStats where ServerTypeId=21 and " +
                    "StatName IN ('OneDriveUpload@" + nodename + "','OneDriveDownload@" + nodename + "') and " +
					"ServerName = '"+servername+"' and " +
                    "DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE())";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        //1/20/2015 NS added for VSPLUS-1316
        public DataTable GetSiteTestsResponseTimes(string nodename,string servername)
        {

            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "select StatName,Date,ROUND(StatValue,1) StatValue FROM MicrosoftDailyStats where ServerTypeId=21 and " +
                    "StatName IN ('CreateSite@" + nodename + "') and " +
					"ServerName = '"+servername+"' and  " +
                    "DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE())";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        //1/20/2015 NS added for VSPLUS-1316
        public DataTable GetTaskFolderTestsResponseTimes(string nodename ,string servername)
        {
            
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "select StatName,Date,ROUND(StatValue,1) StatValue FROM MicrosoftDailyStats where ServerTypeId=21 and " +
                    "StatName IN ('CreateTask@" + nodename + "','CreateFolder@" + nodename + "') and " +
					"ServerName = '"+servername+"' and  " +
                    "DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE())";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

		public DataTable ResponseThreshold(string ServerName, string NodeName)
		{
		
			DataTable dt = new DataTable();
			try
			{
				if (NodeName != null)
				{
					string q = "select ID from Nodes where Name='" + NodeName + "'";
					dt = adaptor.FetchData(q);
					id = Convert.ToInt32(dt.Rows[0]["ID"]);
				}
				if (ServerName != "All")
				{
					string que = "select ID,ResponseThreshold AS Averagethrshold from O365Server where Name='" + ServerName + "'";
					dt = adaptor.FetchData(que);
				}
				else
				{
					//string que = "select Id, name, ResponseThreshold ,  Avg(ResponseThreshold) OVER () AS Averagethrshold  from O365Server where Name in (select ServerName from [VSS_Statistics].[dbo].MicrosoftDailyStats where serverTypeid=21)";
					string que = "select O365serverID,Name,Responsethreshold,avg (Responsethreshold)OVER () AS Averagethrshold from O365server OS inner join O365nodes Ons on OS.ID=Ons.O365serverID where Name in (select ServerName from [VSS_Statistics].[dbo].MicrosoftDailyStats where serverTypeid =21) and NodeID =" + id + "";

					dt = adaptor.FetchData(que);
				}
				return dt;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public DataTable GetNodeName(int ID, string Accountname2)
        {
           
            DataTable dt = new DataTable();
            try
            {
               // string query = "Select n.Name,ln.Location from [vitalsigns].[dbo].Nodes n  , [vitalsigns].[dbo].Locations ln where n.LocationID=ln.ID and n.Id='" + ID + "'";
				string query = "Select n.Name,ln.Location,os.Name AccountName,os.Mode from [vitalsigns].[dbo].Nodes n  , [vitalsigns].[dbo].Locations ln,[vitalsigns].[dbo].O365Server os,[vitalsigns].[dbo].O365nodes ond where n.LocationID=ln.ID and ond.O365ServerID=os.ID and n.Id='" + ID + "' and os.Name='" + Accountname2 + "'"; 
                 dt = adaptor.FetchData(query);
                
            }
            catch
            {
            }
            return dt;
        }

        //2/25/2016 Durga Modified for  VSPLUS-2611

		public DataTable GetOffice365Mails()

		{
			DataTable dt = new DataTable();
			string SqlQuery = "";
			try
			{


                SqlQuery = "select ex.* from ExchangeMailfiles ex   inner join  vitalsigns.dbo.O365Server o365 on ex.server=o365.Name order by DisplayName";
				dt = objAdaptor.FetchData(SqlQuery);

			}
			catch (Exception)
			{

				throw;
			}
			return dt;
		}


        //2/26/2016 NS added for VSPLUS-2648
        public DataTable GetOffice365Tests(string aname)
        {
            DataTable dt = new DataTable();
            try
            {
                /* Full query below in case you need to run a test
                    DECLARE @T AS TABLE(y int NOT NULL PRIMARY KEY);
                    DECLARE
                    @cols AS NVARCHAR(MAX),
                    @y    AS int,
                    @sql  AS NVARCHAR(MAX);
                    -- Construct the column list for the IN clause
                    -- e.g., [1],[2],[3]
                    SET @cols = STUFF(
                    (SELECT N',' + QUOTENAME(y) AS [text()]
                    FROM (SELECT DISTINCT TestName AS y FROM StatusDetail  dd
                    INNER JOIN Status dm on dd.TypeAndName=dm.TypeAndName
                    WHERE dm.Type = 'Office365') AS Y
                    ORDER BY y
                    FOR XML PATH('')),
                    1, 1, N'');
                    -- Construct the full T-SQL statement
                    -- and execute dynamically
                    SET @sql = N'SELECT *
                    FROM (select sr.name AccountName, n.Id,sr.Mode,n.Name NodeName,ln.Location,s.Status,s.LastUpdate,sd.TestName,sd.Result 
                    FROM   
                    [vitalsigns].[dbo].Status s,[vitalsigns].[dbo].Nodes n,[vitalsigns].[dbo].O365Nodes o365,
                    [vitalsigns].[dbo].Locations ln,[vitalsigns].[dbo].O365Server sr , StatusDetail sd
                    where s.Name=sr.Name and sr.ID=o365.O365ServerID and o365.NodeID=n.ID and n.LocationID=ln.ID and s.Location=ln.Location
                    and s.TypeAndName=sd.TypeAndName and s.Type = ''Office365''
                    ) AS D
                    PIVOT(max(Result) FOR TestName IN(' + @cols + N')) AS P;';
                    EXEC sp_executesql @sql;
                    GO
                 */
                string query = "DECLARE @T AS TABLE(y int NOT NULL PRIMARY KEY); " +
                        "DECLARE " +
                        "@cols AS NVARCHAR(MAX), " +
                        "@y    AS int, " +
                        "@sql  AS NVARCHAR(MAX); " +
                        "SET @cols = STUFF( " +
                        "(SELECT N',' + QUOTENAME(y) AS [text()] " +
                        "FROM (SELECT DISTINCT TestName AS y FROM [vitalsigns].[dbo].StatusDetail  dd " +
                        "INNER JOIN [vitalsigns].[dbo].Status dm on dd.TypeAndName=dm.TypeAndName " +
                        "WHERE dm.Type = 'Office365') AS Y " +
                        "ORDER BY y " +
                        "FOR XML PATH('')), " +
                        "1, 1, N''); " +
                        "SET @sql = N'SELECT * " +
                        "FROM (select sr.name AccountName, n.Id,sr.Mode,n.Name NodeName,ln.Location,s.Status,s.LastUpdate,sd.TestName,sd.Result  " +
                        "FROM    " +
                        "[vitalsigns].[dbo].Status s,[vitalsigns].[dbo].Nodes n,[vitalsigns].[dbo].O365Nodes o365, " +
                        "[vitalsigns].[dbo].Locations ln,[vitalsigns].[dbo].O365Server sr , [vitalsigns].[dbo].StatusDetail sd " +
                        "where s.Name=sr.Name and sr.ID=o365.O365ServerID and o365.NodeID=n.ID and n.LocationID=ln.ID and s.Location=ln.Location " +
                        "and s.TypeAndName=sd.TypeAndName and s.Type = ''Office365'' ";
                if (aname != "All")
                {
                    query += "and sr.Name=''" + aname + "'' ";
                }
                query += " ) AS D " +
                    "PIVOT(max(Result) FOR TestName IN(' + @cols + N')) AS P;'; " +
                    "EXEC sp_executesql @sql; ";
                dt = adaptor.FetchData(query);
            }
            catch
            {
            }
            return dt;
        }
        //3/9/2016 NS added for VSPLUS2648
        public DataTable GetMailBoxTypes(string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT COUNT(*) StatValue, mailboxtype StatName FROM O365AdditionalMailDetails " +
                    "WHERE Server='" + servername + "' " +
                    "GROUP BY MailBoxType";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetUsersDailyCount(string statname, string servername, string fromdate, string todate)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT '" + statname + "' AS StatName, DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date,MAX(StatValue) StatValue " +
                    "FROM MicrosoftSummaryStats INNER JOIN vitalsigns.dbo.Status ON " +
                    "ServerName=Name " +
                    "WHERE StatName='" + statname + "' AND ServerName='" + servername + "' ";
                if (fromdate != "" && todate != "")
                {
                    SqlQuery += "AND DATEADD(dd,0,DATEDIFF(dd,0,Date)) BETWEEN '" + fromdate + "' AND '" + todate + "' ";
                }
                SqlQuery += "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),StatName ";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetInactiveUsers(string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT COUNT(*) StatValue, CASE WHEN InActiveDaysCount > 0 AND InActiveDaysCount <= 30 THEN '< 30 days' " +
                    "WHEN InActiveDaysCount > 30 AND InActiveDaysCount <= 60 THEN '30-60 days' " +
                    "WHEN InActiveDaysCount > 60 THEN '> 60 days' END AS StatName FROM O365AdditionalMailDetails " +
                    "WHERE InActiveDaysCount > 0 AND Server='" + servername + "' " +
                    "GROUP BY " +
                    "CASE WHEN InActiveDaysCount > 0 AND InActiveDaysCount <= 30 THEN '< 30 days' " +
                    "WHEN InActiveDaysCount > 30 AND InActiveDaysCount <= 60 THEN '30-60 days' " +
                    "WHEN InActiveDaysCount > 60 THEN '> 60 days' END ";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetActiveInactiveUsers(string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT COUNT(*) StatValue, CASE WHEN InActiveDaysCount > 7 THEN 'Inactive' " +
                    "WHEN InActiveDaysCount <= 7 THEN 'Active' END StatName FROM O365AdditionalMailDetails " +
                    "WHERE Server='" + servername + "' " +
                    "GROUP BY " +
                    "CASE WHEN InActiveDaysCount > 7 THEN 'Inactive' " +
                    "WHEN InActiveDaysCount <= 7 THEN 'Active' END";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetInactiveMailboxes(string ServerName)
        {
            DataTable dt = new DataTable();
            string StrQuery = "SELECT DISTINCT TOP(5) DisplayName  Title, InActiveDaysCount TotalDaysInactive " +
                "FROM O365AdditionalMailDetails WHERE InActiveDaysCount > 7 AND Server='" + ServerName + "' " +
                "ORDER BY InActiveDaysCount DESC";
            dt = objAdaptor.FetchData(StrQuery);

            return dt;
        }
        //3/21/2016 NS added for VSPLUS-2652
        public DataTable GetOffice365Servers()
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT DISTINCT t1.Name ServerName FROM O365Server t1 INNER JOIN Status t2 ON " +
                    "t1.Name=t2.Name INNER JOIN ServerTypes t3 ON t3.ServerType=t2.Type " +
                    "WHERE t1.Enabled=1 " +
                    "ORDER BY t1.Name";
                dt = adaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetOffice365PwdExpSettings(string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT CASE WHEN PasswordNeverExpires=0 THEN 'Never Expires' WHEN PasswordNeverExpires=1 THEN 'Expires' END PasswordNeverExpires, " +
                    "COUNT(PasswordNeverExpires) CountEach " +
                    "FROM O365MSOLUsers INNER JOIN O365Server ON ServerId=ID " +
                    "WHERE Name='" + servername + "' " + 
                    "GROUP BY PasswordNeverExpires ";
                dt = adaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetOffice365PwdStrongSettings(string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT CASE WHEN StrongPasswordRequired=0 THEN 'NOT Required' WHEN StrongPasswordRequired=1 THEN 'Required' END StrongPasswordRequired, " +
                    "COUNT(StrongPasswordRequired) CountEach " +
                    "FROM O365MSOLUsers INNER JOIN O365Server ON ServerId=ID " +
                    "WHERE Name='" + servername + "' " +
                    "GROUP BY StrongPasswordRequired ";
                dt = adaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetOffice365UserSettings(string servername)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT ServerId,DisplayName,UserPrincipalName,UserType,StrongPasswordRequired,PasswordNeverExpires " +
                    "FROM O365MSOLUsers INNER JOIN O365Server ON ServerId=ID " +
                    "WHERE Name='" + servername + "' ";
                dt = adaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

		public DataTable GetOffice365UserServicedetails(string servername)
        {
            DataTable dt = new DataTable();
			DataTable dt2 = new DataTable();
            try
            {
				string SqlQuery = "SELECT ID from o365server " +
					"WHERE Name='" + servername + "' ";
				dt2 = adaptor.FetchData(SqlQuery);
				if (dt2.Rows.Count > 0)
				{
					int serverId = Convert.ToInt32(dt2.Rows[0]["ID"]);
					string SqlQuery2 = "SELECT ServerId,ServiceName,ServiceID,StartTime,Status,Message " +
						" FROM O365ServiceDetails " +
						" WHERE ServerId='" + serverId + "' ";
					dt = adaptor.FetchData(SqlQuery2);
				}
            }
            catch
            {
            }
            return dt;
        }
		

        public DataTable GetOffice365GroupCount()
        {
            DataTable dt = new DataTable();
            try
            {
                string sqlQuery = "Select G.GroupName,G.GroupType,COUNT(GroupName) GroupCount " +
                    "from O365Groups G INNER JOIN O365UserGroups UG on G.GroupId=UG.GroupId " +
                    "INNER JOIN O365MSOLUsers MU on UG.UserPrincipalName=MU.UserPrincipalName " +
                    "INNER JOIN O365Server o365 on o365.ID=MU.ServerId " +
                    "GROUP BY G.GroupName,G.GroupType ";
                dt = adaptor.FetchData(sqlQuery);
            }
            catch
            {
            }
            return dt;
        }
        // 6/6/2016 Durga Addded for VSPLUS-3001

        public DataTable GetLicensesInfo(string ServerName)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "Select ActiveUnits,ConsumedUnits,ISNULL(Costperuser,0) as Costperuser from Office365AccountStats OA inner join O365Server OS on OA.ServerID=OS.ID where OS.Name='" + ServerName + "'";
                dt = adaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }
        public DataTable GetInactiveUsersCount(string ServerName)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT count(*) as count FROM O365AdditionalMailDetails WHERE InActiveDaysCount > 7 and Server='" + ServerName + "'";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }
	}
}
