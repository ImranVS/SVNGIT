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
	class ExchangeHubEdge : IServerRole
	{
		VSFramework.VSAdaptor myAdapter = new VSFramework.VSAdaptor();
		VSFramework.XMLOperation myxmlAdapter = new VSFramework.XMLOperation();
		CultureInfo culture = CultureInfo.CurrentCulture;
		public ExchangeHubEdge()
		{
		}
		public void CheckServer(MonitoredItems.ExchangeServer myServer, ReturnPowerShellObjects powerShellObjects, ref TestResults AllTestResults)
		{

			commonEnums.ServerRoles ServerRole;
			string role;

			if (myServer.Role.Contains("HUB", StringComparer.InvariantCultureIgnoreCase))
			{
				ServerRole = commonEnums.ServerRoles.HUB;
				role = "Hub";
			}
			else if (myServer.Role.Contains("EDGE", StringComparer.InvariantCultureIgnoreCase))
			{
				ServerRole = commonEnums.ServerRoles.Edge;
				role = "Edge";
			}
			else
			{
				ServerRole = commonEnums.ServerRoles.CAS;
				role = "CAS";
			}

			Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "In " + role + ".", ServerRole, Common.LogLevel.Normal);
			getQueueCounts(powerShellObjects.PS, ref AllTestResults, myServer, ServerRole, role);
		}

		public void getQueueCounts(PowerShell powershell, ref TestResults AllTestsList, MonitoredItems.ExchangeServer myServer, commonEnums.ServerRoles ServerRole, string role)
		{

            //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "In  getQueueCount", Common.LogLevel.Normal);
			Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "In getQueueCount", ServerRole, Common.LogLevel.Normal);
			try
			{
				bool SubAdded = false;
				bool ShadowAdded = false;
				System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
				//Change the Path to the Script to suit your needs
				System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\ex_QueueCountSum.ps1");
				//String str = "$serverName = '" + myServer.Name + "' \n" + sr.ReadToEnd();
				//String str = sr.ReadToEnd();
				//string str="$server = Get-ExchangeServer " + "\n";
				//str +="$getqueues = foreach($servers in $server){ Get-Queue -Server $servers.Name}" + "\n";
				//str += "$getqueues | ft -AutoSize";
				string str = "Get-Queue -Server " + myServer.Name;
				powershell.AddScript(str);
				results = powershell.Invoke();
				DateTime dtNow = DateTime.Now;
				string sqlQuery = "";
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "getQueueCount: output results: " + results.Count.ToString(), ServerRole, Common.LogLevel.Normal);
				//first get submission and shadow results
				double subCount = 0;
				double shadowCount = 0;
				foreach (PSObject ps in results)
				{
					string strIdentity = ps.Properties["Identity"].Value.ToString();
					string strMessageCount = ps.Properties["MessageCount"].Value == null ? "0" : ps.Properties["MessageCount"].Value.ToString();
					Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Shadow and Submission Counts:", ServerRole, Common.LogLevel.Normal);

					string[] s;
					if (strIdentity != "")
					{
						 s= strIdentity.Split('\\');
						 if (s.Length > 0)
						 {
							 if (s[1].ToString().ToLower() == "submission")
							 {
								 subCount += Convert.ToDouble(strMessageCount.ToString());
							 }
							 
							 if (s[1].ToString().ToLower() == "shadow")
							 {
								 shadowCount += Convert.ToDouble(strMessageCount.ToString());
							 }
						 }
					}
				}


				double unreachableCount = 0;

				foreach (PSObject ps in results)
				{
					string strIdentity = ps.Properties["Identity"].Value.ToString();
					string strMessageCount = ps.Properties["MessageCount"].Value == null ? "0" : ps.Properties["MessageCount"].Value.ToString();
					string strStatus = ps.Properties["Status"].Value.ToString().ToLower();
					if (strStatus =="suspended" || strStatus =="retry")
					{
						unreachableCount += Convert.ToDouble(strMessageCount);
					}
					
				}
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Unreachable Count:" + unreachableCount.ToString(), ServerRole, Common.LogLevel.Normal);
				string unreachableAlertDetail = getAlertDetail("Unreachable",myServer.ThresholdSetting.UnReachableQThreshold,unreachableCount);
				string subAlertDetail = getAlertDetail("Submission", myServer.ThresholdSetting.SubQThreshold, subCount);
				string shadowAlertDetail = getAlertDetail("Shadow", myServer.ThresholdSetting.ShadowQThreshold, shadowCount);
				

				sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName, ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
						+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "','" + dtNow + "','" + role + "@Unreachable#Queues','" + unreachableCount.ToString() +
					"'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
				AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
				Common.makeAlert(unreachableCount, myServer.ThresholdSetting.UnReachableQThreshold, myServer, commonEnums.AlertType.Unreachable, ref AllTestsList, unreachableAlertDetail, role);

				sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName, ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
					+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "','" + dtNow + "','" + role + "@Submission#Queues','" + subCount +
					"'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
				AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Submission Count: " + subCount, ServerRole, Common.LogLevel.Normal);
				Common.makeAlert(subCount, myServer.ThresholdSetting.SubQThreshold, myServer, commonEnums.AlertType.Submission, ref AllTestsList, subAlertDetail,role);

				sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName, ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
					+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "','" + dtNow + "','" + role + "@Shadow#Queues','" + shadowCount +
					"'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
				AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Shadow Count: " + shadowCount, ServerRole, Common.LogLevel.Normal);
				Common.makeAlert(shadowCount, myServer.ThresholdSetting.ShadowQThreshold, myServer, commonEnums.AlertType.Shadow, ref AllTestsList, shadowAlertDetail,role);


                String sql = "Update Status Set PendingMail='" + subCount.ToString() + "', DeadMail='" + unreachableCount.ToString() + "', HeldMail='" + shadowCount.ToString() + "', PendingThreshold='" + myServer.ThresholdSetting.SubQThreshold + "', DeadThreshold='" + myServer.ThresholdSetting.UnReachableQThreshold + "',HeldMailThreshold='" + myServer.ThresholdSetting.ShadowQThreshold + "' WHERE TYPEANDNAME='" + myServer.Name + "-" + myServer.ServerType + "'";
				AllTestsList.SQLStatements.Add(new SQLstatements() { DatabaseName = "VitalSigns", SQL = sql });
                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "Ended for getQueueCount", Common.LogLevel.Normal);

			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "Error in getQueueCounts" + ex.Message, ServerRole, Common.LogLevel.Normal);
			}
			finally
			{
				// dispose the runspace and enable garbage collection
				//runspace.Dispose();
				//runspace = null;
			}
		}

		private string getAlertDetail(string queueName,double th, double actualValue)
		{
			string retVal = "";
			if (th == 0)
				retVal = "The " + queueName +" Queue has a threshold set of 0, so will not send alerts.";
			else if (th < 0)
				retVal = "The " + queueName +" Queue threshold is not set.  Please set a value in the configurator if you would like to receive alerts.";
			else if (th > Convert.ToInt32(actualValue))
				retVal = "The " +queueName +" Queue is below the alert threshold.";
			else
				retVal = "The " + queueName +" Queue has a value of " + actualValue.ToString() + " messages, which is greater than the threshold value of " + th.ToString() + " messages.";
			return retVal;
		}
	}
}
