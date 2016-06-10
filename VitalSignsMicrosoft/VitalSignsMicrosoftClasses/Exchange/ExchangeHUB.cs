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
    class ExchangeHUB : IServerRole
    {
        string sqlcon = "";
       VSFramework.VSAdaptor myAdapter = new VSFramework.VSAdaptor();
       VSFramework.XMLOperation myxmlAdapter = new VSFramework.XMLOperation();
       CultureInfo culture = CultureInfo.CurrentCulture;
       public ExchangeHUB()
        {
        }
       public void CheckServer(MonitoredItems.ExchangeServer myServer, ReturnPowerShellObjects powerShellObjects, ref TestResults AllTestResults)
        {
            if (powerShellObjects != null)
            {
				Common.WriteDeviceHistoryEntry("Exchange", myServer.Name, "In HUB.", commonEnums.ServerRoles.HUB, Common.LogLevel.Normal);
				
                QueuesBasedOnIdentity(powerShellObjects.PS, myServer.Name, myServer.IPAddress, myServer.Name, "Submission", ref AllTestResults, myServer.ThresholdSetting.SubQThreshold, myServer);
				QueuesBasedOnIdentity(powerShellObjects.PS, myServer.Name, myServer.IPAddress, myServer.Name, "Poison", ref AllTestResults, myServer.ThresholdSetting.PoisonQThreshold, myServer);
				QueuesBasedOnIdentity(powerShellObjects.PS, myServer.Name, myServer.IPAddress, myServer.Name, "Unreachable", ref AllTestResults, myServer.ThresholdSetting.UnReachableQThreshold, myServer);
            }

        }
       public void QueuesBasedOnIdentity(PowerShell powershell, string servername, string IPAddress, string oExchange, string strAction, ref TestResults AllTestsList, int threshold, MonitoredItems.ExchangeServer myServer)
        {
			Common.WriteDeviceHistoryEntry("Exchange", servername, "In QueuesBasedOnIdentity: " + strAction , commonEnums.ServerRoles.HUB, Common.LogLevel.Normal);
			string strAlertDetail = "";
            //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "In Hub QueuesBasedOnIdentity:", Common.LogLevel.Normal);
            try
            {
                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();
                //Change the Path to the Script to suit your needs
                System.IO.StreamReader sr = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\ex_QueuesBasedOnIdentity.ps1");
                String str = sr.ReadToEnd();

				string strIdentity = servername.Replace("https://", "") + "\\" + strAction;// "\\Submission";
                powershell.AddScript(str );
                powershell.AddParameter("identity", strIdentity);
                results = powershell.Invoke();

                if (powershell.Streams.Error.Count > 51)
                {
                    foreach (ErrorRecord er in powershell.Streams.Error)
                        Console.WriteLine(er.ErrorDetails);
					Common.makeAlert(false, myServer, (commonEnums.AlertType)System.Enum.Parse(typeof(commonEnums.AlertType), strAction), ref AllTestsList, "Errors: Instance:" + oExchange + " Error count>51 at " + System.DateTime.Now.ToShortTimeString(), "Hub");
                }
                else
                {
					Common.WriteDeviceHistoryEntry("Exchange", servername, "ex_QueuesBasedOnIdentity: "+ strAction + " output results: " + results.Count.ToString(), commonEnums.ServerRoles.HUB, Common.LogLevel.Normal);

                    foreach (PSObject ps in results)
                    {
                        string strDeliveryType = ps.Properties["DeliveryType"].Value == null ? "" : ps.Properties["DeliveryType"].Value.ToString();
                        string strStatus = ps.Properties["Status"].Value == null ? "" : ps.Properties["Status"].Value.ToString();
                        string strMessageCount = ps.Properties["MessageCount"].Value == null ? "" : ps.Properties["MessageCount"].Value.ToString();
                        string strNextHopDomain = ps.Properties["NextHopDomain"].Value == null ? "" : ps.Properties["NextHopDomain"].Value.ToString();
						
                        DateTime dtNow = DateTime.Now;
                        int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
						string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName, ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
						  + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','HUB@" + strAction + "#Queues','" + strMessageCount.ToString() +
                         "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
                        AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
						
				if (threshold == 0)
				{
					strAlertDetail = "The " + strAction + " Queue has a threshold set of 0, so will not send alerts.";
				}
				else if (threshold < 0)
				{
					strAlertDetail = "The " +strAction + " Queue threshold is not set.  Please set a value in the configurator if you would like to receive alerts.";
				}
				else if (threshold > Convert.ToInt32(strMessageCount))
				{
					strAlertDetail = "The " + strAction + " Queue is below the alert threshold.";
				}
				else
				{
					strAlertDetail = "The " + strAction + " Queue has a value of " + strMessageCount.ToString() + " messages, which is greater than the threshold value of " + threshold.ToString() + " messages.";
				}

						
						Common.makeAlert(double.Parse(strMessageCount), threshold, myServer, (commonEnums.AlertType)System.Enum.Parse(typeof(commonEnums.AlertType), strAction), ref AllTestsList, strAlertDetail,"Hub");
                    }
					if (results.Count == 0)
					{
						DateTime dtNow = DateTime.Now;
						int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
						string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
						  + " values('" + servername + "','" + myServer.ServerTypeId + "','" + dtNow + "','HUB@" + strAction + "#Queues','0" + 
						 "'," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

						Common.makeAlert(true, myServer, (commonEnums.AlertType)System.Enum.Parse(typeof(commonEnums.AlertType), strAction), ref AllTestsList, strAlertDetail, "Hub");
					}
                }
                //Common.WriteDeviceHistoryEntry("All", "Microsoft_Performance", "In  HUb QueuesBasedOnIdentity:", Common.LogLevel.Normal);
            }
            catch (Exception ex)
            {
				Common.WriteDeviceHistoryEntry("Exchange", servername, "Error in QueuesBasedOnIdentity: " +strAction + " " + ex.Message, commonEnums.ServerRoles.HUB, Common.LogLevel.Normal);
            }
                
            finally
            {
                // dispose the runspace and enable garbage collection
                //runspace.Dispose();
                //runspace = null;
            }
        }
    }
}
