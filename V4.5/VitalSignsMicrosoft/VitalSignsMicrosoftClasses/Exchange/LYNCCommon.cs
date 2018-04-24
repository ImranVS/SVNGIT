using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using System.Data.SqlClient;
using System.Security;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Remoting;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Threading;

using VSFramework;
using System;

namespace VitalSignsMicrosoftClasses
{
	class LYNCCommon
	{

	   //  string sqlcon = "";
	   //SqlConnection con;
       VSFramework.VSAdaptor myAdapter = new VSFramework.VSAdaptor();
       VSFramework.XMLOperation myxmlAdapter = new VSFramework.XMLOperation();
       CultureInfo culture = CultureInfo.CurrentCulture;
	   public LYNCCommon()
        {
            //InitializeComponent();
			//sqlcon = myxmlAdapter.GetDBConnectionString("VitalSigns");
			//con = new SqlConnection(sqlcon);'
        }
       public void CheckServer(MonitoredItems.ExchangeServer myServer, ReturnPowerShellObjects powerShellObjects, ref TestResults AllTestResults)
       {
		   getLYNCStats(powerShellObjects.PS, myServer, ref AllTestResults);
		   getChatStats(powerShellObjects.PS, myServer, ref AllTestResults);
		   getGroupChatStats(powerShellObjects.PS, myServer, ref AllTestResults);
	   }
	   public void getGroupChatStats(PowerShell powershell, MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList)
	   {
		   string servername = myServer.Name;
		   string IPAddress = myServer.IPAddress;
		   Common.WriteDeviceHistoryEntry("Skype for Business", servername, "In getMailFlow.", commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);

		   try
		   {

			   System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
			   String str = "Test-CSim -TargetFqdn " + servername;
			   powershell.Streams.Error.Clear();
			   powershell.Commands.Clear();
			   powershell.AddScript(str);

			   results = powershell.Invoke();
			   Common.WriteDeviceHistoryEntry("Skype for Business", servername, "getChatStats result count:" + results.Count.ToString(), commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);
			   DateTime dtNow = DateTime.Now;
			   int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
			   if (results.Count > 0)
			   {
				   foreach (PSObject ps in results)
				   {
					   string status = ps.Properties["Result"].Value == null ? "Fail" : ps.Properties["Result"].Value.ToString();
					   string sTime = ps.Properties["Latency"].Value == null ? "-1" : ps.Properties["Latency"].Value.ToString();
					   if (sTime != "")
						   sTime = sTime.Split(':')[2].ToString();
					   Common.WriteDeviceHistoryEntry("Skype for Business", servername, "getChatStats: Result status:" + status, commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);
					   if (status == "Success")
						   AllTestsList.StatusDetails.Add(new TestList() { Details = "Skype for Business Group Chat Succeeded with latency time:" + sTime, TestName = "Skype for Business Group Chat", Category = commonEnums.ServerRoles.Lync, Result = commonEnums.ServerResult.Pass });
					   else
						   AllTestsList.StatusDetails.Add(new TestList() { Details = "Skype for Business Group Chat Failed with latency time:" + sTime, TestName = "Skype for Business Group Chat", Category = commonEnums.ServerRoles.Lync, Result = commonEnums.ServerResult.Fail });

					   //string strDetails = " status:" + status + " Latency:" + sTime;
					   //AllTestsList.StatusDetails.Add(new TestList() { Details = strDetails, TestName = "ClientVersion", Category = commonEnums.ServerRoles.Lync, Result = commonEnums.ServerResult.Pass });

					   //string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
					   //  + " values('" + servername + "','" + dtNow + "','Lync@GroupChatStatus'" + " ,'" + status.ToString() +
					   // "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
					   //AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

					   string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName, ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
						 + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','Lync@GroupChatLatency'" + " ," + sTime.ToString() +
						"," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
					   AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

					   //string strSQL = "INSERT INTO dbo.LYNCChat(SERVERNAME,STATUS,DETAILS) VALUES('" + servername + "','" + status + "','" + "LYNC Chat Responded with latency:" + sTime + "')";
					   //Common.WriteDeviceHistoryEntry("LYNC", servername, DateTime.Now.ToString() + " getLyncStats :SQL:" + strSQL, Common.LogLevel.Normal);
					   //AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = strSQL, DatabaseName = "vitalsigns" });
				   }
			   }
		   }

		   catch (Exception ex)
		   {

			   Common.WriteDeviceHistoryEntry("Skype for Business", servername, "Error in getChatStats: " + ex.Message, commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);

		   }
	   }
	   public void getLYNCStats(PowerShell powershell, MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList)
	   {
		   string servername = myServer.Name;
		   string IPAddress = myServer.IPAddress;
		   Common.WriteDeviceHistoryEntry("Skype for Business", servername, "In get Skype for Business Stats.", commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);
		   try
		   {
			   System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
			    //String str = "Test-CsGroupIM -TargetFqdn Lync.jnittech.com";
			   String str = "Set-Location '" + AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts' " + System.Environment.NewLine;
			   str += "./LYNC_GETCONNECTIONSVersion1.ps1 –PoolFQDN " + servername + " –Includeusers –ShowTotal | Foreach-Object{New-Object PSObject -Property @{ "+ System.Environment.NewLine +
								"ClientVersion=$_.ClientVersion "+ System.Environment.NewLine +
								"Agent=$_.Agent " + System.Environment.NewLine +
								"Connections=$_.Connections " + System.Environment.NewLine +
								"FrontEndServers=$_.FrontEndServers " + System.Environment.NewLine +
								"ConnectionsPercent=$_.ConnectionsPercent " + System.Environment.NewLine +
								"UsersConnected=$_.UsersConnected " + System.Environment.NewLine +
								"LyncEnabledUsers=$_.LyncEnabledUsers " + System.Environment.NewLine +
								"VoiceEnabledUsers=$_.VoiceEnabledUsers " + System.Environment.NewLine +
								"PercentOfEnabledUsersConnected=$_.PercentOfEnabledUsersConnected " + System.Environment.NewLine +
								"ClientVersionsConnected=$_.ClientVersionsConnected " + System.Environment.NewLine +
								"ConnectedUsers=$_.ConnectedUsers " + System.Environment.NewLine +
								"Section=$_.Section " + System.Environment.NewLine +
								"TotalConnections=$_.TotalConnections " + System.Environment.NewLine +
								"}}";
			   Common.WriteDeviceHistoryEntry("Skype for Business", servername, "get Skype for BusinessStats Script:" + str, commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);
				powershell.Streams.Error.Clear();
				powershell.Commands.Clear();
				powershell.AddScript(str);

				results = powershell.Invoke();
				Common.WriteDeviceHistoryEntry("Skype for Business", servername, "get Skype for Business Stats result count:" + results.Count.ToString(), commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				if (results.Count > 0)
				{
					foreach (PSObject ps in results)
					{

						string section = ps.Properties["Section"].Value == null ? "-1" : ps.Properties["Section"].Value.ToString();
						Common.WriteDeviceHistoryEntry("Skype for Business", servername, "Section " + section, commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);

						if (section == "1")
						{
							string ClientVersion = ps.Properties["ClientVersion"].Value == null ? "Fail" : ps.Properties["ClientVersion"].Value.ToString();
							string Agent = ps.Properties["Agent"].Value == null ? "-1" : ps.Properties["Agent"].Value.ToString();
							string Connections = ps.Properties["Connections"].Value == null ? "-1" : ps.Properties["Connections"].Value.ToString();
							Common.WriteDeviceHistoryEntry("Skype for Business", servername, "Sectio n 1: \n" + ClientVersion + " " + Agent + " " + Connections, commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);
							string strDetails = " ClientVerions:" + ClientVersion + " Agent:" + Agent + " Connections:" + Connections;
							AllTestsList.StatusDetails.Add(new TestList() { Details = strDetails, TestName = "ClientVersion", Category = commonEnums.ServerRoles.Lync, Result = commonEnums.ServerResult.Pass });
							AllTestsList.StatusDetails.Add(new TestList() { Details = strDetails, TestName = "Agent", Category = commonEnums.ServerRoles.Lync, Result = commonEnums.ServerResult.Pass });
							AllTestsList.StatusDetails.Add(new TestList() { Details = strDetails, TestName = "Connections", Category = commonEnums.ServerRoles.Lync, Result = commonEnums.ServerResult.Pass });

							string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
							  + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','Lync@ClientVersion'" + " ,'" + ClientVersion.ToString() +
							 "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });


						}
						if (section == "2")
						{
							//checks to see if the TotalConnections output is null or not...if it is, then it is not the last entry (the one with the total output)
							//if it is not null, then it is the last entry
							string TotalConnections = ps.Properties["TotalConnections"].Value == null ? "Fail" : ps.Properties["TotalConnections"].Value.ToString();
							if (TotalConnections == "Fail")
							{
								string FrontEndServers = ps.Properties["FrontEndServers"].Value == null ? "Fail" : ps.Properties["FrontEndServers"].Value.ToString();
								string Connections = ps.Properties["Connections"].Value == null ? "-1" : ps.Properties["Connections"].Value.ToString();
								string ConnectionsPercent = ps.Properties["ConnectionsPercent"].Value == null ? "-1" : ps.Properties["ConnectionsPercent"].Value.ToString();
								Common.WriteDeviceHistoryEntry("Skype for Business", servername, "Section 2: \n" + FrontEndServers + " " + Connections + " " + ConnectionsPercent, commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);

								string strDetails=" FrontEndServers:" + FrontEndServers + " Connections:" + Connections + " ConnectionsPercent:" + ConnectionsPercent ;
							AllTestsList.StatusDetails.Add(new TestList() { Details = strDetails, TestName = "ClientVersion", Category = commonEnums.ServerRoles.Lync, Result = commonEnums.ServerResult.Pass });

							string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
							  + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','Lync@FrontEndServers'" + " ,'" + FrontEndServers.ToString() +
							 "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

							sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
							  + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','Lync@FEConnections'" + " ,'" + Connections.ToString() +
							 "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

							sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
							  + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','Lync@ConnectionsPercent'" + " ,'" + ConnectionsPercent.ToString() +
							 "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });


							}
							else
							{
								Common.WriteDeviceHistoryEntry("Skype for Business", servername, "Section 2: \n" + TotalConnections, commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);
							}
						}
						if (section == "3")
						{
							string UsersConnected = ps.Properties["UsersConnected"].Value == null ? "Fail" : ps.Properties["UsersConnected"].Value.ToString();
							string LyncEnabledUsers = ps.Properties["LyncEnabledUsers"].Value == null ? "-1" : ps.Properties["LyncEnabledUsers"].Value.ToString();
							string VoiceEnabledUsers = ps.Properties["VoiceEnabledUsers"].Value == null ? "-1" : ps.Properties["VoiceEnabledUsers"].Value.ToString();
							string PercentOfEnabledUsersConnected = ps.Properties["PercentOfEnabledUsersConnected"].Value == null ? "-1" : ps.Properties["PercentOfEnabledUsersConnected"].Value.ToString();
							string ClientVersionsConnected = ps.Properties["ClientVersionsConnected"].Value == null ? "-1" : ps.Properties["ClientVersionsConnected"].Value.ToString();
							Common.WriteDeviceHistoryEntry("Skype for Business", servername, "Section 3: \n" + UsersConnected + " " + LyncEnabledUsers + " " + VoiceEnabledUsers + " " + PercentOfEnabledUsersConnected + " " + ClientVersionsConnected, commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);

							string strDetails = " UsersConnected:" + UsersConnected + " LyncEnabledUsers:" + LyncEnabledUsers + " VoiceEnabledUsers:" + VoiceEnabledUsers + " PercentOfEnabledUsersConnected:" + PercentOfEnabledUsersConnected + " ClientVersionsConnected:" + ClientVersionsConnected;
							AllTestsList.StatusDetails.Add(new TestList() { Details = strDetails, TestName = "ClientVersion", Category = commonEnums.ServerRoles.Lync, Result = commonEnums.ServerResult.Pass });

							string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
							  + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','Lync@UsersConnected'" + " ,'" + UsersConnected.ToString() +
							 "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

							sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
							  + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','Lync@LyncEnabledUsers'" + " ,'" + LyncEnabledUsers.ToString() +
							 "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

							sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
							  + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','Lync@VoiceEnabledUsers'" + " ,'" + VoiceEnabledUsers.ToString() +
							 "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

							sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
							  + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','Lync@PercentOfEnabledUsersConnected'" + " ,'" + PercentOfEnabledUsersConnected.ToString() +
							 "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

							sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
							  + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','Lync@ClientVersionsConnected'" + " ,'" + ClientVersionsConnected.ToString() +
							 "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

							

						}
						if (section == "4")
						{
							string ConnectedUsers = ps.Properties["ConnectedUsers"].Value == null ? "Fail" : ps.Properties["ConnectedUsers"].Value.ToString();
							string LyncEnabledUsers = ps.Properties["Connections"].Value == null ? "-1" : ps.Properties["Connections"].Value.ToString();
							Common.WriteDeviceHistoryEntry("Skype for Business", servername, "Section 4: \n" + ConnectedUsers + " " + LyncEnabledUsers, commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);

							string strDetails = " ConnectedUsers:" + ConnectedUsers + " LyncEnabledUsers:" + LyncEnabledUsers;
							AllTestsList.StatusDetails.Add(new TestList() { Details = strDetails, TestName = "ClientVersion", Category = commonEnums.ServerRoles.Lync, Result = commonEnums.ServerResult.Pass });

							string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
							  + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','Lync@ConnectedUsers'" + " ,'" + ConnectedUsers.ToString() +
							 "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

							sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
							  + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','Lync@LyncEnabledUsers'" + " ,'" + LyncEnabledUsers.ToString() +
							 "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
							AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						}


						
						/*
						string status = ps.Properties["Total Unique Users/Clients"].Value == null ? "Fail" : ps.Properties["Total Unique Users/Clients"].Value.ToString();
						string sTime = ps.Properties["Client Versions Connected"].Value == null ? "-1" : ps.Properties["Client Versions Connected"].Value.ToString();
						string sTime2 = ps.Properties["Client Versions Connected"].Value == null ? "-1" : ps.Properties["Client Versions Connected"].Value.ToString();

						Common.WriteDeviceHistoryEntry("LYNC", servername, DateTime.Now.ToString() + " : getChatStats: Result status:" + status, Common.LogLevel.Normal);
						Common.WriteDeviceHistoryEntry("LYNC", servername, DateTime.Now.ToString() + " : getChatStats: Result time:" + sTime, Common.LogLevel.Normal);
						if (status == "Success")
							AllTestsList.StatusDetails.Add(new TestList() { Details = "Lync Chat Succeeded with latency time:" + sTime, TestName = "LYNC Chat", Category = commonEnums.ServerRoles.MailFlow, Result = commonEnums.ServerResult.Pass });
						else
							AllTestsList.StatusDetails.Add(new TestList() { Details = "Lync Chat Failed with latency time:" + sTime, TestName = "LYNC Chat", Category = commonEnums.ServerRoles.MailFlow, Result = commonEnums.ServerResult.Fail });

						//string strSQL = "INSERT INTO dbo.LYNCChat(SERVERNAME,STATUS,DETAILS) VALUES('" + servername + "','" + status + "','" + "LYNC Chat Responded with latency:" + sTime + "')";
						//Common.WriteDeviceHistoryEntry("LYNC", servername, DateTime.Now.ToString() + " getLyncStats :SQL:" + strSQL, Common.LogLevel.Normal);
						//AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = strSQL, DatabaseName = "vitalsigns" });
					  * */
					}
				}
			}
		   catch (Exception ex)
		   {
			   Common.WriteDeviceHistoryEntry("Skype for Business", servername, "Error in getLYNCStats: " + ex.Message, commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);

		   }
		}

	   public void getChatStats(PowerShell powershell, MonitoredItems.ExchangeServer myServer, ref TestResults AllTestsList)
	   {
		   string servername = myServer.Name;
		   string IPAddress = myServer.IPAddress;
		   Common.WriteDeviceHistoryEntry("Skype for Business", servername, "In getMailFlow.", commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);

		   try
		   {

			   System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
			   String str = "Test-CsGroupIM -TargetFqdn " + servername;
			   powershell.Streams.Error.Clear();
			   powershell.Commands.Clear();
			   powershell.AddScript(str);

			   results = powershell.Invoke();
			   Common.WriteDeviceHistoryEntry("Skype for Business", servername, "getChatStats result count:" + results.Count.ToString(), commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);
			   DateTime dtNow = DateTime.Now;
			   int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
			   if (results.Count > 0)
			   {
				   foreach (PSObject ps in results)
				   {
					   string status = ps.Properties["Result"].Value == null ? "Fail" : ps.Properties["Result"].Value.ToString();
					   string sTime = ps.Properties["Latency"].Value == null ? "-1" : ps.Properties["Latency"].Value.ToString();
					   if (sTime != "")
						   sTime = sTime.Split(':')[2].ToString();
					   Common.WriteDeviceHistoryEntry("Skype for Business", servername, "getChatStats: Result status:" + status, commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);
					   if (status == "Success")
						   AllTestsList.StatusDetails.Add(new TestList() { Details = "Skype for Business Chat Succeeded with latency time:" + sTime, TestName = "Skype for Business Chat", Category = commonEnums.ServerRoles.Lync, Result = commonEnums.ServerResult.Pass });
					   else
						   AllTestsList.StatusDetails.Add(new TestList() { Details = "Skype for Business Chat Failed with latency time:" + sTime, TestName = "Skype for Business Chat", Category = commonEnums.ServerRoles.Lync, Result = commonEnums.ServerResult.Fail });

					   //string strDetails = " status:" + status + " Latency:" + sTime;
					   //AllTestsList.StatusDetails.Add(new TestList() { Details = strDetails, TestName = "ClientVersion", Category = commonEnums.ServerRoles.Lync, Result = commonEnums.ServerResult.Pass });

					   //string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
					   //  + " values('" + servername + "','" + dtNow + "','Lync@ChatStatus'" + " ,'" + status.ToString() +
					   // "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
					   //AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

					   string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
						 + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','Lync@ChatLatency'" + " ," + sTime.ToString() +
						"," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
					   AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

					   //string strSQL = "INSERT INTO dbo.LYNCChat(SERVERNAME,STATUS,DETAILS) VALUES('" + servername + "','" + status + "','" + "LYNC Chat Responded with latency:" + sTime + "')";
					   //Common.WriteDeviceHistoryEntry("LYNC", servername, DateTime.Now.ToString() + " getLyncStats :SQL:" + strSQL, Common.LogLevel.Normal);
					   //AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = strSQL, DatabaseName = "vitalsigns" });
				   }
			   }
		   }

		   catch (Exception ex)
		   {

			   Common.WriteDeviceHistoryEntry("Skype for Business", servername, "Error in getChatStats: " + ex.Message, commonEnums.ServerRoles.Lync, Common.LogLevel.Normal);

		   }
	   }

	   }
	}
