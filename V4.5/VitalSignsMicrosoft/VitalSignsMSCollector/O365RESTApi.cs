using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using Microsoft.SharePoint.Client;
using System.Globalization;
using System.Threading;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Text.RegularExpressions;
using System.Configuration;
namespace VitalSignsMicrosoftClasses
{
	public class O365RESTApi
	{
		CultureInfo culture = CultureInfo.CurrentCulture;
		public string nodeName = "";
		public void doADFSCheck(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList)
		{
			string response = submitRequest("https://login.microsoftonline.com/common/userrealm/?user=" + myServer.UserName +"&api-version=2.1&stsRequest=&checkForMicrosoftAccount=true", "GET", "", "", "");
			if (response != "")
			{
				O365ADFS myDevices = new O365ADFS();
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(myDevices.GetType());
				MemoryStream ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(response));
				try
				{
					myDevices = (O365ADFS)serializer.ReadObject(ms);
					if (myDevices != null)
					{
						if (myDevices.NameSpaceType == "Federated")
						{
							myServer.ADFSMode = true;
							response = submitRequest(myDevices.AuthURL, "GET", "", "", "");
							if (response =="-1")
							{
								myServer.ADFSRedirectTest = false;
								Common.makeAlert(false , myServer, commonEnums.AlertType.ADFS, ref AllTestsList, "ADFS Redirect Failed", myServer.Category);
							}
							else
							{
								myServer.ADFSRedirectTest = true;
								Common.makeAlert(true, myServer, commonEnums.AlertType.ADFS, ref AllTestsList, "", myServer.Category);
							}
						}
						else
						{
							myServer.ADFSMode = false;
							//Common.makeAlert(true, myServer, commonEnums.AlertType.ADFS, ref AllTestsList, "", myServer.Category);
						}
					}
						
				}
				catch (Exception ex)
				{
					string s = ex.Message.ToString();
				}
			}
			
		}

		public void doURLTest(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList)
		{
			if (ConfigurationManager.AppSettings["VSNodeName"] != null)
				nodeName = ConfigurationManager.AppSettings["VSNodeName"].ToString();
			try
			{
				long done;
				long start;
				TimeSpan elapsed = new TimeSpan(0);
				start = DateTime.Now.Ticks;
				string response = submitRequest(myServer.IPAddress, "GET", "", "", "");

				done = DateTime.Now.Ticks;
				elapsed = new TimeSpan(done - start);

				if (response != "")
				{
				
					if (response == "-1")
					{
						myServer.URLTest = false;
						Common.makeAlert(false, myServer, commonEnums.AlertType.URL, ref AllTestsList, "O365 URL Test Failed", myServer.Category);
					}
					else
					{
						myServer.URLTest = true;
						Common.makeAlert(true, myServer, commonEnums.AlertType.URL, ref AllTestsList, "", myServer.Category);
						DateTime dtNow = DateTime.Now;
						int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
						string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
								+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'ResponseTime" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
							   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
						AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });

					}
				}
				else
				{
					myServer.URLTest = false;
					Common.makeAlert(false, myServer, commonEnums.AlertType.URL, ref AllTestsList, "O365 URL Test Failed", myServer.Category);
				}
			}
			catch (Exception ex)
			{
				string s = ex.Message.ToString();
			}

		}


		public void doO365RESTApiTests(MonitoredItems.Office365Server Server, ref TestResults AllTestsList)
		{
			if (ConfigurationManager.AppSettings["VSNodeName"] != null)
				nodeName = ConfigurationManager.AppSettings["VSNodeName"].ToString();
			//testSameTimeService();
			doADFSCheck(Server, ref AllTestsList);
			try
			{
				if (Server.EnableCreateCalEntryTest)
					try
					{ doNewTaskTest(Server, ref AllTestsList); }
					catch (Exception ex)
					{ Common.WriteDeviceHistoryEntry(Server.ServerType,Server.Name, "Error with doNewTaskTest: " + ex.Message.ToString(), Common.LogLevel.Normal); }
				if (Server.EnableComposeEmailTest)
					try
					{ doComposeMailTest(Server, ref AllTestsList); }
					catch (Exception ex)
					{ Common.WriteDeviceHistoryEntry(Server.ServerType,Server.Name, "Error with doComposeMailTest: " + ex.Message.ToString(), Common.LogLevel.Normal); }
				if (Server.EnableMailFlow)
					try
					{ doMailFlowTest(Server, ref AllTestsList);}
					catch (Exception ex)
					{ Common.WriteDeviceHistoryEntry(Server.ServerType,Server.Name, "Error with doMailFlowTest: " + ex.Message.ToString(), Common.LogLevel.Normal); }
				if (Server.EnableInboxTest)
					try
					{ doInboxTest(Server, ref AllTestsList); }
					catch (Exception ex)
					{ Common.WriteDeviceHistoryEntry(Server.ServerType,Server.Name, "Error with doInboxTest: " + ex.Message.ToString(), Common.LogLevel.Normal); }
				if (Server.EnableOneDriveUploadTest)
					try
					{ uploadFile(Server, ref AllTestsList); }
					catch (Exception ex)
					{ Common.WriteDeviceHistoryEntry(Server.ServerType,Server.Name, "Error with uploadFile: " + ex.Message.ToString(), Common.LogLevel.Normal); }
				if (Server.EnableOneDriveDownloadTest)
					try
					{ downloadSPFile(Server, ref AllTestsList); }
					catch (Exception ex)
					{ Common.WriteDeviceHistoryEntry(Server.ServerType,Server.Name, "Error with downloadSPFile: " + ex.Message.ToString(), Common.LogLevel.Normal); }
				if (Server.EnableCreateFolderTest)
					try
					{ createFolderTest(Server, ref AllTestsList); }
					catch (Exception ex)
					{ Common.WriteDeviceHistoryEntry(Server.ServerType,Server.Name, "Error with createFolderTest: " + ex.Message.ToString(), Common.LogLevel.Normal); }
				//createCalendar("RPR");
				//getSharePointFolderInfo("RPR");

			}
			catch (Exception ex)
			{
				string s = ex.Message.ToString();
			}
		}
		//private void testSameTimeService()
		//{
		//    Chilkat.Http WebPage = new Chilkat.Http();
		//    bool success = false;
		//    string strResponse = "";
		//    string strURL = "";
		//    try
		//    {
		//        success = WebPage.UnlockComponent("MZLDADHttp_efwTynJYYR3X");
		//    }
		//    catch (Exception ex)
		//    {
		//    }

		//    try
		//    {

		//        if ((success != true))
		//        {
		//        }
		//        else
		//        {
		//            try
		//            {
		//                strURL = "https://47.22.0.132:9446/ConferenceFocus/monitoring/MonitoringRestServlet?method=AcrossConferenceData";

		//                strResponse = WebPage.QuickGetStr(strURL);
		//                AcrossConferenceData myDevices = new AcrossConferenceData();
		//                DataContractJsonSerializer serializer = new DataContractJsonSerializer(myDevices.GetType());
		//                MemoryStream ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(strResponse));
		//                try
		//                {
		//                    myDevices = (AcrossConferenceData)serializer.ReadObject(ms);
		//                }
		//                catch (Exception ex)
		//                {
		//                    string s = ex.Message.ToString();
		//                }
		//            }
		//            catch (Exception ex)
		//            {
		//            }
		//        }

		//    }
		//    catch (Exception ex)
		//    {
		//    }
		//    finally
		//    {
		//        WebPage.Dispose();

		//    }

		//    string response = submitRequest("https://47.22.0.132:9446/ConferenceFocus/monitoring/MonitoringRestServlet?method=AcrossConferenceData", "GET", "", "", "");
		//    if (response != "")
		//    {
		//        AcrossConferenceData myDevices = new AcrossConferenceData();
		//        DataContractJsonSerializer serializer = new DataContractJsonSerializer(myDevices.GetType());
		//        MemoryStream ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(response));
		//        try
		//        {
		//            myDevices = (AcrossConferenceData)serializer.ReadObject(ms);
		//            //if (myDevices.value[0].Subject == randomNumber)
		//            //    messageId = myDevices.value[0].Id;
		//        }
		//        catch (Exception ex)
		//        {
		//            //messageId = "";
		//            string s = ex.Message.ToString();
		//        }
		//    }

		//}
		#region utilities
		public string submitRequest(string URL, string requestMethod, string message, string userId, string pwd)
		{
			string responseFromServer = "";
			try
			{


				//SharePointOnlineCredentials creds = new SharePointOnlineCredentials(userId, Common.String2SecureString(pwd));
				System.Net.WebRequest request = System.Net.WebRequest.Create(URL);
				System.Net.CredentialCache c = new System.Net.CredentialCache();
				request.Credentials = BuildCredentials("https://outlook.office365.com", userId, pwd, "BASIC");
				//request.Credentials = creds;
				//System.Net.WebClient wc = new System.Net.WebClient();

				request.Method = requestMethod;
				//request.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED: f");

				if (requestMethod == "POST")
				{
					request.ContentType = "application/json";
					string s = "" + (char)34;
					message = message.Replace("'", s);
					byte[] bytes = System.Text.Encoding.ASCII.GetBytes(message);
					request.ContentLength = bytes.Length;
					System.IO.Stream os = request.GetRequestStream();
					os.Write(bytes, 0, bytes.Length); //Push it out there
					os.Close();
				}
				//SharePointOnlineCredentials 
				//Microsoft.SharePoint.Client.ClientRuntimeContext.SetupRequestCredential(

				System.Net.WebResponse ws = request.GetResponse();
				Stream dataStream = ws.GetResponseStream();
				// Open the stream using a StreamReader for easy access.
				StreamReader reader = new StreamReader(dataStream);
				responseFromServer = reader.ReadToEnd();
			}
			catch (Exception ex)
			{
				responseFromServer = "-1";
				string s = ex.Message.ToString();
			}
			return responseFromServer;
		}
		public string submitSharePointRequest(string URL, string requestMethod, string message, string userId, string pwd)
		{
			string responseFromServer = "";
			try
			{


				SharePointOnlineCredentials creds = new SharePointOnlineCredentials(userId, Common.String2SecureString(pwd));

				System.Net.WebRequest request = System.Net.WebRequest.Create(URL);
				System.Net.CredentialCache c = new System.Net.CredentialCache();
				//request.Credentials = BuildCredentials("https://outlook.office365.com", userId, pwd, "BASIC");
				request.Credentials = creds;

				request.Method = requestMethod;
				request.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED: f");

				if (requestMethod == "POST")
				{
					request.ContentType = "application/json";
					string s = "" + (char)34;
					message = message.Replace("'", s);
					byte[] bytes = System.Text.Encoding.ASCII.GetBytes(message);
					request.ContentLength = bytes.Length;
					System.IO.Stream os = request.GetRequestStream();
					os.Write(bytes, 0, bytes.Length); //Push it out there
					os.Close();
				}
				//SharePointOnlineCredentials 
				//Microsoft.SharePoint.Client.ClientRuntimeContext.SetupRequestCredential(

				System.Net.WebResponse ws = request.GetResponse();
				Stream dataStream = ws.GetResponseStream();
				// Open the stream using a StreamReader for easy access.
				StreamReader reader = new StreamReader(dataStream);
				responseFromServer = reader.ReadToEnd();
			}
			catch (Exception ex)
			{
				responseFromServer = "-1";
				string s = ex.Message.ToString();
			}
			return responseFromServer;
		}
		//    public void uploadSharePointfile(string URL, string URL1, MonitoredItems.Office365Server myServer, ref TestResults AllTestsList)
		//    {
		//                    SharePointOnlineCredentials creds = new SharePointOnlineCredentials(myServer.UserName, Common.String2SecureString(myServer.Password));
		//                    UploadFile(URL1, creds, "Documents", "c:\\myfolder\\jnit\\o365.png");
		////        bool bFileUploaded = false;
		////        try
		////        {
		////            SharePointOnlineCredentials creds = new SharePointOnlineCredentials(myServer.UserName, Common.String2SecureString(myServer.Password));
		////            System.Net.WebClient wc = new System.Net.WebClient();
		////            using (var client = new CookieAwareWebClient())
		////            {
		////                var values = new System.Collections.Specialized.NameValueCollection
		////{
		////    { "username", myServer.UserName},
		////    { "password", myServer.Password  },
		////};
		////                client.Credentials = creds;
		////                client.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED: f");
		////                client.UploadValues("https://" + myServer.tenantName + ".sharepoint.com", values);

		////                //client.Headers.Add(System.Net.HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
		////                byte[] responseArray = client.UploadFile(URL1, "PUT", "test.txt");
		////                if (responseArray.Length > 0)
		////                    bFileUploaded = true;
		////            }
		////            //wc.Credentials = creds;
		////            //wc.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED: f");
		////            //wc.Headers.Add(System.Net.HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
		////            //byte[] responseArray = wc.UploadFile(URL1, "PUT", "o365.txt");
		////        }
		////        catch (Exception ex)
		////        {
		////            bFileUploaded = false;
		////            string s = ex.Message.ToString();
		////        }
		////        Common.makeAlert(bFileUploaded, myServer, commonEnums.AlertType.OneDrive_Upload_Document, ref AllTestsList, "", myServer.ServerType);

		//    }
		public string downloadSharePointfile(string URL, string URL1, string requestMethod, string message, string userId, string pwd)
		{
			string responseFromServer = "";
			try
			{
				SharePointOnlineCredentials creds = new SharePointOnlineCredentials(userId, Common.String2SecureString(pwd));
				System.Net.WebClient wc = new System.Net.WebClient();

				System.Net.CredentialCache c = new System.Net.CredentialCache();
				wc.Credentials = creds;
				wc.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED: f");
				wc.DownloadFile(URL1, "c:\\myfolder\\jnit\\o3652.png");
			}
			catch (Exception ex)
			{
				responseFromServer = "-1";
				string s = ex.Message.ToString();
			}
			return responseFromServer;
		}
		private System.Net.ICredentials BuildCredentials(string siteurl, string username, string password, string authtype)
		{
			System.Net.NetworkCredential cred;
			if (username.Contains(@"\"))
			{
				string domain = username.Substring(0, username.IndexOf(@"\"));
				username = username.Substring(username.IndexOf(@"\") + 1);
				cred = new System.Net.NetworkCredential(username, password, domain);
			}
			else
			{
				cred = new System.Net.NetworkCredential(username, password);
			}
			System.Net.CredentialCache cache = new System.Net.CredentialCache();
			if (authtype.Contains(":"))
			{
				authtype = authtype.Substring(authtype.IndexOf(":") + 1); //remove the TMG: prefix
			}
			cache.Add(new Uri(siteurl), authtype, cred);
			return cache;
		}

		#endregion
		#region composeMail
		private void doComposeMailTest(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList)
		{
			long done;
			long start;
			TimeSpan elapsed = new TimeSpan(0);
			start = DateTime.Now.Ticks;
			string response = submitRequest("https://outlook.office365.com/owa", "GET", "", myServer.UserName, myServer.Password);
			done = DateTime.Now.Ticks;
			elapsed = new TimeSpan(done - start);
			if (response != "" && response != "-1" && response.ToLower().Contains("we don't recognize this user id or password") == false)
				Common.makeAlert(false, myServer, commonEnums.AlertType.Compose_Email, ref AllTestsList, "", myServer.Category);
			else
			{
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
						+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'ComposeEMail" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
					   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
				AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
				Common.makeAlert(true, myServer, commonEnums.AlertType.Compose_Email, ref AllTestsList, "", myServer.Category);
			}
		}
		#endregion
		#region calendarTask
		private string doNewTaskTest(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList)
		{
			long done;
			long start;
			TimeSpan elapsed = new TimeSpan(0);

			string fromDate = DateTime.Now.Year.ToString() + "-" + "12" + "-" + DateTime.Now.Day.ToString() + "T" + "22" + ":00:00Z";
			string toTime = DateTime.Now.Year.ToString() + "-" + "12" + "-" + DateTime.Now.Day.ToString() + "T" + "23" + ":00:00Z";

			string message = "{  'Subject': 'VS Task',  'Body': {    'ContentType': 'HTML',    'Content': 'New event'  },  'Start': '" + fromDate + "',  'End': '" + toTime + "',  'Attendees': [    {      'EmailAddress': {        'Address': '" + myServer.UserName + "'      },      'Type': 'Required'    }  ]}";
			start = DateTime.Now.Ticks;
			string response = submitRequest("https://outlook.office365.com/api/v1.0/me/events", "POST", message, myServer.UserName, myServer.Password);
			done = DateTime.Now.Ticks;
			elapsed = new TimeSpan(done - start);
			string calendarId = "";
			if (response != "")
			{
				calendarEvent myDevices = new calendarEvent();
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(myDevices.GetType());
				MemoryStream ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(response));
				try
				{
					myDevices = (calendarEvent)serializer.ReadObject(ms);
					calendarId = myDevices.Id;

				}
				catch (Exception ex)
				{
					calendarId = "";
					string s = ex.Message.ToString();
				}
			}
			if (calendarId != "")
			{
				Common.makeAlert(true, myServer, commonEnums.AlertType.Create_Calendar_Entry, ref AllTestsList, "", myServer.Category);
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
							+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'CreateTask" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
						   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
				AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
				if (deleteTask(calendarId, myServer))
					Common.makeAlert(true, myServer, commonEnums.AlertType.Delete_Calendar_Entry, ref AllTestsList, "", myServer.Category);
				else
					Common.makeAlert(false, myServer, commonEnums.AlertType.Delete_Calendar_Entry, ref AllTestsList, "", myServer.Category);
			}
			else
				Common.makeAlert(false, myServer, commonEnums.AlertType.Create_Calendar_Entry, ref AllTestsList, "", myServer.Category);

			return calendarId;
			//create a new event
			//delete the event
		}
		public bool deleteTask(string calendarId, MonitoredItems.Office365Server myServer)
		{
			bool isCalDeleted = false;
			string response = submitRequest("https://outlook.office365.com/api/v1.0/me/events/" + calendarId, "DELETE", "", myServer.UserName, myServer.Password);
			if (response == "")
				isCalDeleted = true;

			return isCalDeleted;
		}
		#endregion
		#region mailFlow
		private void doMailFlowTest(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList)
		{
			Common.WriteDeviceHistoryEntry(myServer.ServerType, myServer.Name, "in doMailFlowTest ");
			//first send email, check for that email then Delete the email
			Random r = new Random();
			string randomNo = r.NextDouble().ToString();
			long done;
			long start;
			TimeSpan elapsed = new TimeSpan(0);

			bool mailSent = sendMessage(randomNo, myServer);
			start = DateTime.Now.Ticks;

			System.Threading.Thread.Sleep(myServer.MailFlowThreshold);
			string messageId = getMessages(randomNo, myServer);
			//if the message has not been delivered yet, well try to check for another 30 secs
			if (messageId == "")
			{
				DateTime dtElapsed = DateTime.Now.AddSeconds(30);
				//do few more times
				while (DateTime.Now < dtElapsed)
				{
					messageId = getMessages(randomNo, myServer);
					if (messageId != "")
					{
						done = DateTime.Now.Ticks;
						elapsed = new TimeSpan(done - start);
						break;
					}
				}
			}
			//if the message has been delivered
			if (messageId != "")
			{
				done = DateTime.Now.Ticks;
				elapsed = new TimeSpan(done - start);
				deleteMessage(messageId, myServer);
			}
			else
			{
				done = DateTime.Now.Ticks;
				elapsed = new TimeSpan(done - start);
			}
			if (messageId == "")
				Common.makeAlert(elapsed.TotalMilliseconds, myServer.MailFlowThreshold, myServer, commonEnums.AlertType.Mail_flow, ref AllTestsList, "Mail was not delivered in the specified threshold time plus additional 30 secs.", myServer.Category);
			else
			{
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
						+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'MailLatency" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
					   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
				AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
				Common.makeAlert(elapsed.TotalMilliseconds, myServer.MailFlowThreshold, myServer, commonEnums.AlertType.Mail_flow, ref AllTestsList, "Mail was delivered", myServer.Category);
			}

		}
		public bool sendMessage(string randomNumber, MonitoredItems.Office365Server myServer)
		{
			bool mailSent = false;
			string message = "{  'Message': {    'Subject': '" + randomNumber + "',    'Body': {      'ContentType': 'Text',      'Content': '" + randomNumber + "'    },    'ToRecipients': [      {        'EmailAddress': {          'Address': '" + myServer.UserName + "'        }      }    ] },  'SaveToSentItems': 'false'}";
			string response = submitRequest("https://outlook.office365.com/api/v1.0/me/sendmail", "POST", message, myServer.UserName, myServer.Password);
			if (response == "" && response != "-1")
				mailSent = true;
			return mailSent;
		}
		public string getMessages(string randomNumber, MonitoredItems.Office365Server myServer)
		{
			string messageId = "";
			//string response = submitRequest("https://outlook.office365.com/api/v1.0/me/messages", "GET", "", "info@RPRVitalSigns.com", "V1talS1gns");
			string response = submitRequest("https://outlook.office365.com/api/v1.0/me/messages", "GET", "", myServer.UserName, myServer.Password);
			if (response != "")
			{
				RootObject myDevices = new RootObject();
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(myDevices.GetType());
				MemoryStream ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(response));
				try
				{
					myDevices = (RootObject)serializer.ReadObject(ms);
					if (myDevices.value[0].Subject == randomNumber)
						messageId = myDevices.value[0].Id;
				}
				catch (Exception ex)
				{
					messageId = "";
					string s = ex.Message.ToString();
				}
			}
			return messageId;
		}
		public void doInboxTest(MonitoredItems.Office365Server myServer, ref TestResults AllTestsList)
		{
			long done;
			long start;
			TimeSpan elapsed = new TimeSpan(0);
			bool bInboxTest = false;
			start = DateTime.Now.Ticks;
			//string response = submitRequest("https://outlook.office365.com/api/v1.0/me/messages", "GET", "", "info@RPRVitalSigns.com", "V1talS1gns");
			string response = submitRequest("https://outlook.office365.com/api/v1.0/me/messages", "GET", "", myServer.UserName, myServer.Password);
			if (response != "")
			{
				done = DateTime.Now.Ticks;
				elapsed = new TimeSpan(done - start);

				RootObject myDevices = new RootObject();
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(myDevices.GetType());
				MemoryStream ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(response));
				try
				{
					myDevices = (RootObject)serializer.ReadObject(ms);
					if (myDevices.value.Count > 0)
						bInboxTest = true;
				}
				catch (Exception ex)
				{
					bInboxTest = false;
					string s = ex.Message.ToString();
				}
			}
			if (bInboxTest)
			{
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
						+ " values('" + myServer.Name + "','" + myServer.ServerTypeId + "',GetDate(),'Inbox" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
					   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
				AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
			}
			Common.makeAlert(bInboxTest, myServer, commonEnums.AlertType.Inbox, ref AllTestsList, "", myServer.Category);

		}
		public bool deleteMessage(string messageId, MonitoredItems.Office365Server myServer)
		{
			bool isMessageDeleted = false;
			string response = submitRequest("https://outlook.office365.com/api/v1.0/me/messages/" + messageId, "DELETE", "", myServer.UserName, myServer.Password);
			return isMessageDeleted;
		}
		#endregion
		#region calendar
		public bool createCalendar(string calendarName, MonitoredItems.Office365Server myServer)
		{
			bool bPass = false;

			string message = "{ 'Name': '" + calendarName + "'}";
			string response = submitRequest("https://outlook.office365.com/api/v1.0/me/calendars", "POST", message, myServer.UserName, myServer.Password);
			if (response != "-1")
				bPass = true;
			return bPass;
		}
		public bool deleteCalendar(string calendarName, MonitoredItems.Office365Server myServer)
		{
			bool bPass = false;
			string message = "{ 'Name': '" + calendarName + "'}";
			string response = submitRequest("https://outlook.office365.com/api/v1.0/me/calendars", "POST", message, myServer.UserName, myServer.Password);
			if (response != "-1")
				bPass = true;
			return bPass;
		}
		private void doNewCalendarTest()
		{
			//create a new calendar
			//delete the calendar
		}
		#endregion
		#region sharepointFiles
		public void uploadFile(MonitoredItems.Office365Server Server, ref TestResults AllTestsList)
		{
			//first get the folder. or just upload to root?
			//string URL1 = "https://rprvitalsigns-my.sharepoint.com/_api/v1.0/me/files/01KJ5FF2WLRQDGXS25KVGIDBIHBGT2I5DY/children/0365.png/content";
			//string URL2 = "https://" + Server.tenantName + "-my.sharepoint.com/_api/v1.0/me/files/root/children/test.txt/content";
			string URL = "https://" + Server.tenantName + ".sharepoint.com";
			//string URL = "https://" + Server.tenantName + "-my.sharepoint.com/_api/v1.0/me/files/root";
			//uploadSharePointfile(URL, URL2, Server, ref AllTestsList);
			SharePointOnlineCredentials creds = new SharePointOnlineCredentials(Server.UserName, Common.String2SecureString(Server.Password));
			string fileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Scripts\\TestFiles\\VSTestText.txt";
			bool fileUploadStatus = UploadFile(URL, creds, "Documents", fileName, Server, ref AllTestsList);
			Common.makeAlert(fileUploadStatus, Server, commonEnums.AlertType.OneDrive_Upload_Document, ref AllTestsList, "", Server.Category);
			//bool fileDownloadStatus = DownloadDocument(URL, "VSTestText.txt", creds, "Documents");
			//Common.makeAlert(fileDownloadStatus, Server, commonEnums.AlertType.OneDrive_Download_Document, ref AllTestsList, "", Server.ServerType);
		}
		private bool UploadFile(string url, System.Net.ICredentials creds, string listTitle, string fileName, MonitoredItems.Office365Server Server, ref TestResults AllTestsList)
		{
			bool fileUploadSuccess = false;
			long done;
			long start;
			TimeSpan elapsed = new TimeSpan(0);
			Common.WriteDeviceHistoryEntry(Server.ServerType,Server.Name, "Upload file location: " + fileName, Common.LogLevel.Normal);
			//string file = System.Reflection.Assembly.GetEntryAssembly().Location + fileName;
			try
			{
				using (var clientContext = new ClientContext(url))
				{
					clientContext.Credentials = creds;

					using (var fs = new FileStream(fileName, FileMode.Open))
					{
						var fi = new FileInfo(fileName);
						var list = clientContext.Web.Lists.GetByTitle(listTitle);
						clientContext.Load(list.RootFolder);
						start = DateTime.Now.Ticks;
						clientContext.ExecuteQuery();
						var fileUrl = String.Format("{0}/{1}", list.RootFolder.ServerRelativeUrl, fi.Name);

						Microsoft.SharePoint.Client.File.SaveBinaryDirect(clientContext, fileUrl, fs, true);
						done = DateTime.Now.Ticks;
						elapsed = new TimeSpan(done - start);
						fileUploadSuccess = true;
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteDeviceHistoryEntry(Server.ServerType,Server.Name, "Error in function UploadFile: " + ex.Message.ToString(), Common.LogLevel.Normal);
				fileUploadSuccess = false;

			}
			if (fileUploadSuccess)
			{
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
						+ " values('" + Server.Name + "','" + Server.ServerTypeId + "',GetDate(),'OneDriveUpload" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
					   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
				AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
			}

			return fileUploadSuccess;
		}
		public void downloadSPFile(MonitoredItems.Office365Server Server, ref TestResults AllTestsList)
		{
			string URL = "https://" + Server.tenantName + ".sharepoint.com";
			SharePointOnlineCredentials creds = new SharePointOnlineCredentials(Server.UserName, Common.String2SecureString(Server.Password));
			bool fileDownloadStatus = DownloadDocument(URL, "VSTestText.txt", creds, "Documents", Server, ref AllTestsList);
			Common.makeAlert(fileDownloadStatus, Server, commonEnums.AlertType.OneDrive_Download_Document, ref AllTestsList, "", Server.Category);
		}
		public bool DownloadDocument(string siteURL, string documentName, System.Net.ICredentials creds, string documentListName, MonitoredItems.Office365Server Server, ref TestResults AllTestsList)
		{
			long done;
			long start;
			TimeSpan elapsed = new TimeSpan(0);
			bool downloadStatus = false;
			try
			{
				ListItem item = GetDocumentFromSP(documentName, creds, siteURL, documentListName);
				if (item != null)
				{
					using (ClientContext clientContext = new ClientContext(siteURL))
					{
						clientContext.Credentials = creds;
						start = DateTime.Now.Ticks;
						FileInformation fInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(clientContext,
							item["FileRef"].ToString());
						//SaveStreamToFile("VSTestDL.txt", fInfo.Stream);
						//testdownload(fInfo.Stream);
						done = DateTime.Now.Ticks;
						elapsed = new TimeSpan(done - start);
						if (fInfo.Stream != null)
							downloadStatus = true;
					}

				}
			}
			catch (Exception ex)
			{
				downloadStatus = false;
			}

			if (downloadStatus)
			{
				DateTime dtNow = DateTime.Now;
				int weekNumber = culture.Calendar.GetWeekOfYear(dtNow, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
				string sqlQuery = "Insert into VSS_Statistics.dbo.MicrosoftDailyStats(ServerName,ServerTypeId,Date,StatName,StatValue,WeekNumber,MonthNumber,YearNumber,DayNumber, HourNumber, Details) "
						+ " values('" + Server.Name + "','" + Server.ServerTypeId + "',GetDate(),'OneDriveDownload" + "@" + nodeName + "'" + " ," + elapsed.TotalMilliseconds.ToString() +
					   "," + weekNumber + ", " + dtNow.Month.ToString() + ", " + dtNow.Year.ToString() + ", " + dtNow.Day.ToString() + ", " + dtNow.Hour.ToString() + ", '')";
				AllTestsList.SQLStatements.Add(new SQLstatements() { SQL = sqlQuery, DatabaseName = "VSS_Statistics" });
			}

			return downloadStatus;

		}

		public void SaveStreamToFile(string fileFullPath, Stream stream)
		{
			if (stream.Length == 0) return;

			// Create a FileStream object to write a stream to a file
			using (FileStream fileStream = System.IO.File.Create(fileFullPath, (int)stream.Length))
			{
				// Fill the bytes[] array with the stream data
				byte[] bytesInStream = new byte[stream.Length];
				stream.Read(bytesInStream, 0, (int)bytesInStream.Length);

				// Use FileStream object to write to the specified file
				fileStream.Write(bytesInStream, 0, bytesInStream.Length);
			}
		}
		public void testdownload(Stream input)
		{
			byte[] buffer = new byte[16345];
			string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string file = currentDirectory + "\\VsTestDL.txt";
			using (FileStream fs = new FileStream(file,
								FileMode.Create, FileAccess.Write, FileShare.None))
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					fs.Write(buffer, 0, read);
				}
			}
		}
		private static ListItem GetDocumentFromSP(string documentName, System.Net.ICredentials creds, string URL, string documentList)
		{

			//This method is discussed above i.e. Get List Item Collection from SharePoint
			//Document List
			ListItemCollection listItems = GetListItemCollectionFromSP("FileLeafRef",
				documentName, "Text", 1, creds, URL, documentList);


			return (listItems != null && listItems.Count == 1) ? listItems[0] : null;

		}
		private static ListItemCollection GetListItemCollectionFromSP(string name,
   string value, string type, int rowLimit, System.Net.ICredentials creds, string URL, string documentList)
		{
			string siteURL = URL;
			string documentListName = documentList;
			ListItemCollection listItems = null;
			using (ClientContext clientContext = new ClientContext(siteURL))
			{
				clientContext.Credentials = creds;
				List documentsList = clientContext.Web.Lists.GetByTitle(documentListName);


				CamlQuery camlQuery = new CamlQuery(); ;

				camlQuery.ViewXml =
				@"<View>

<Query>
<Where>

<Eq>
<FieldRef Name='" + name + @"'/>

<Value Type='" + type + "'>" + value + @"</Value>
</Eq>

</Where>                    
<RowLimit>" + rowLimit.ToString() + @"</RowLimit>

</Query>
</View>";


				listItems = documentsList.GetItems(camlQuery);

				clientContext.Load(documentsList);
				clientContext.Load(listItems);

				clientContext.ExecuteQuery();
			}


			return listItems;

		}
		//public void downloadFile(MonitoredItems.Office365Server Server, ref TestResults AllTestsList)
		//{
		//    string URL1 = "https://rprvitalsigns-my.sharepoint.com/_api/v1.0/me/files/01KJ5FF2WLRQDGXS25KVGIDBIHBGT2I5DY/children/0365.png/content";
		//    string URL = "https://" + Server.tenantName + "-my.sharepoint.com/_api/v1.0/me/files/root";
		//    string URL2 = "https://" + Server.tenantName + "-my.sharepoint.com/_api/v1.0/me/files/01KJ5FF2T6SQPKD4SSEJBI3BBVQLTPCNF4/content";
		//    downloadSharePointfile(URL, URL2, "POST", "", Server.UserName, Server.Password);


		//}
		public void getSharePointFolderInfo(string folderName, MonitoredItems.Office365Server myServer)
		{
			//string response = submitRequest("https://rprvitalsigns-my.sharepoint.com/_api/v1.0/me/files/getByPath('/RPR')", "GET", "", "info@RPRVitalSigns.com", "V1talS1gns");
			string response = submitSharePointRequest("https://" + myServer.tenantName + "-my.sharepoint.com/_api/v1.0/me/files/root", "GET", "", myServer.UserName, myServer.Password);
			string test = "";
			if (response != "")
			{
				sharepointFiles myDevices = new sharepointFiles();
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(myDevices.GetType());
				MemoryStream ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(response));
				try
				{
					myDevices = (sharepointFiles)serializer.ReadObject(ms);

				}
				catch (Exception ex)
				{
					string s = ex.Message.ToString();
				}
			}

		}

		public void createFolderTest(MonitoredItems.Office365Server Server, ref TestResults AllTestsList)
		{
			
			long done;
			long start;
			TimeSpan elapsed = new TimeSpan(0);
			start = DateTime.Now.Ticks;

			//Makes the folder
			string message = "{'DisplayName': 'VSInbox'}";
			string response = submitRequest("https://outlook.office365.com/api/v1.0/me/folders/inbox/childfolders", "POST", message, Server.UserName, Server.Password);
			

			if (response != "" && response != "-1")
			{
				done = DateTime.Now.Ticks;
				elapsed = new TimeSpan(done-start);

				//Gets id and deletes the folder
				string Id = response.Substring(response.IndexOf("\"Id") + "\"Id\":".Length);
				Id = Id.Substring(0, Id.IndexOf('"', 2)).Replace("\"", "").Trim();

				response = submitRequest("https://outlook.office365.com/api/v1.0/me/folders/" + Id, "DELETE", "", Server.UserName, Server.Password);


				//Alerting and SQLs
				string sql = Common.GetInsertIntoDailyStats(Server.Name, Server.ServerTypeId.ToString(), "CreateFolder"+ "@" + nodeName  , elapsed.Milliseconds.ToString());
				AllTestsList.SQLStatements.Add(new SQLstatements() { DatabaseName = "VSS_Statistics", SQL = sql });
				Common.makeAlert(elapsed.Milliseconds, Server.CreateFolderThreshold, Server, commonEnums.AlertType.Create_Mail_Folder, ref AllTestsList, Server.Category);

			}
		}
		#endregion

		#region sampleTestCode
		//private void firstCalltoExchange()
		//{
		//    string URL1 = "https://ex13-1.jnittech.com/owa/auth/logon.aspx?url=https://ex13-1.jnittech.com/owa/&reason=0";
		//    string URL2 = "https://ex13-1.jnittech.com/owa/auth.owa";
		//    string URL3 = "https://ex13-1.jnittech.com/owa/#authRedirect=true";
		//    string URL4="https://ex13-1.jnittech.com/owa/userspecificresourceinjector.ashx?ver=15.0.1044.25&appcacheclient=1&layout=mouse";

		//    string USERNAME = "jnittech\\nitin.davis";
		//    string PASSWORD = "ASdfg!@345";
		//    string ck1 = "logondata=acc=1&lgn=" + HttpUtility.UrlEncode(USERNAME);
		//    string ck2 = "";
		//    CookieContainer cookies = new CookieContainer();

		//    //---------------- FIRST CALL GET-----------------------
		//    HttpWebRequest webRequest = WebRequest.Create(URL1) as HttpWebRequest;
		//    webRequest.Timeout = 60000; //set to 1 min
		//    webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
		//    webRequest.ContentType = "application/x-www-form-urlencoded";
		//    webRequest.Accept = "text/html, application/xhtml+xml, */*";
		//    webRequest.Headers.Add("DNT", "1");

		//    //webRequest.Headers.Add("Cookie", ck1);
		//    webRequest.KeepAlive = true;
		//    System.Net.ServicePointManager.ServerCertificateValidationCallback +=
		//    delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
		//                            System.Security.Cryptography.X509Certificates.X509Chain chain,
		//                            System.Net.Security.SslPolicyErrors sslPolicyErrors)
		//    {
		//        return true; // **** Always accept
		//    };
		//    webRequest.CookieContainer = cookies;
		//    WebResponse webresp = webRequest.GetResponse();
		//    //ck1 = webresp.Headers.Get(7).ToString();
		//    WebHeaderCollection webh = webresp.Headers;
		//    ck1 = webh["Set-Cookie"].ToString();

		//    //ck1 = cookies.GetCookieHeader(new Uri("https://ex13-1.jnittech.com")).ToString();

		//    //StreamReader responseReader = webRequest.GetResponse().GetResponseStream();
		//    StreamReader responseReader = new StreamReader(
		//          webresp.GetResponseStream()
		//       );
		//    string responseData = responseReader.ReadToEnd();
		//    responseReader.Close();

		//    //-----------SECOND CALL-POST-------------------------------
		//    string postData =
		//          String.Format(
		//             "destination={0}&flags=4&forcedownlevel=0&username={1}&password={2}&isUtf8=1",
		//            HttpUtility.UrlEncode(URL3), HttpUtility.UrlEncode(USERNAME), HttpUtility.UrlEncode(PASSWORD)
		//          );

		//    //postData = HttpUtility.HtmlEncode(postData);
		//    webRequest = WebRequest.Create(URL2) as HttpWebRequest;
		//    webRequest.Timeout = 60000; //set to 1 min
		//    webRequest.Method = "POST";
		//    webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
		//    webRequest.Accept = "text/html, application/xhtml+xml, */*";
		//    webRequest.Headers.Add("DNT", "1");
		//    webRequest.KeepAlive = true;
		//    webRequest.Referer = "https://ex13-1.jnittech.com/owa/auth/logon.aspx?url=https://ex13-1.jnittech.com/owa/&reason=0";
		//    ck1 = ck1 + "; PBack=0";
		//    //webRequest.AllowAutoRedirect = true;
		//    //CookieContainer cookies2 = new CookieContainer();
		//    webRequest.CookieContainer = cookies;
		//    //webRequest.Headers.Add("Cookie", ck1);
		//    webRequest.Headers.Add("Accept-Language", "en-US");
		//    //webRequest.Connection.a
		//    webRequest.Headers.Add("Cache-Control", "no-cache");
		//    webRequest.ContentType = "application/x-www-form-urlencoded";
		//    //webRequest.Connection = "Keep-Alive";
		//    //webRequest.CookieContainer = cookies;
		//    webRequest.AllowAutoRedirect = false;
		//    //cookies.Add(new Cookie("PBack", "0","","https://ex13-1.jnittech.com"));
		//    // write the form values into the request message
		//    StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
		//    requestWriter.Write(postData);
		//    requestWriter.Close();
		//    webresp = webRequest.GetResponse();
		//    webh = webresp.Headers;
		//    string headerCookie = webh["Set-Cookie"].ToString();

		//    //ck2 = webresp.Headers.Get(3).ToString();
		//    //ck2 = webresp.Headers.ToString();
		//    //string strCADataCookie = Regex.Replace(headerCookie, "(.*)cadata=\"(.*)\"(.*)", "$2");
		//    //string strSessionIDCookie = Regex.Replace(headerCookie, "(.*)sessionid=(.*)(,|;)(.*)", "$2");
		//    //ck2 = ck1 + "; " + "cadata=" + strCADataCookie + "; sessionid=" + strSessionIDCookie;
		//    ck2 = ck1 + "; " + headerCookie;
		//    // we don't need the contents of the response, just the cookie it issues
		//    webRequest.GetResponse().Close();

		//    //------------------- LAST CALL GET---------------------------
		//    webRequest = WebRequest.Create(URL4) as HttpWebRequest;
		//    webRequest.Timeout = 60000; //set to 1 min
		//    webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
		//    //webRequest.ContentType = "application/x-www-form-urlencoded";
		//    webRequest.Accept = "text/html, application/xhtml+xml, */*";
		//    webRequest.Headers.Add("DNT", "1");
		//    webRequest.Headers.Add("Accept-Language", "en-US");
		//    //webRequest.Connection.a
		//    webRequest.Headers.Add("Cache-Control", "no-cache");
		//    webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
		//    //webRequest.Headers.Add("Cookie", ck2);
		//    webRequest.CookieContainer = cookies;
		//    webRequest.Referer = "https://ex13-1.jnittech.com/owa/auth/logon.aspx?url=https://ex13-1.jnittech.com/owa/&reason=0";
		//    webRequest.KeepAlive = true;
		//    //webRequest.AllowAutoRedirect = false;
		//    System.Net.ServicePointManager.ServerCertificateValidationCallback +=
		//    delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
		//                            System.Security.Cryptography.X509Certificates.X509Chain chain,
		//                            System.Net.Security.SslPolicyErrors sslPolicyErrors)
		//    {
		//        return true; // **** Always accept
		//    };

		//    //webRequest.CookieContainer = cookies;
		//    webresp = webRequest.GetResponse();
		//    //ck1 = webresp.Headers.Get(7).ToString();
		//    webh = webresp.Headers;
		//    headerCookie = webh["Set-Cookie"].ToString();
		//    string userContextCookie = headerCookie.Substring(headerCookie.IndexOf("UserContext=") + 12);
		//    userContextCookie = userContextCookie.Substring(0, userContextCookie.IndexOf(";"));
		//    //string[] userContextCookie=headerCookie.Split(';');// headerCookie.IndexOf("UserContext=")
		//    //(.*?)=(.*?)($|;|,(?! ))
		//    //string he = Regex.Replace(headerCookie, "(.*)sessionid=(.*)(,|;)(.*)", "$2");
		//    //string userContext = Regex.Replace(headerCookie, "(.*)UserContext=\"(.*)\"(.*)", "$2");
		//    //StreamReader responseReader = webRequest.GetResponse().GetResponseStream();
		//    responseReader = new StreamReader(
		//          webresp.GetResponseStream()
		//       );
		//    responseData = responseReader.ReadToEnd();
		//    responseReader.Close();
		//    bool owaTest = false;
		//    if (headerCookie.ToLower().Contains("usercontext"))
		//        owaTest = true;
		//    string result = "";

		//}

//        private void TestAutoDiscovery()
//        {
//            formAuthExchg();

//            testBasicAuthO365OWA();
//            formAuth0365();
//            formAuthJira();
//            //GetPerson();
//            //test1();
//            //RedirectHTTPAuthExample("alan forbes", "lotusnotes");
//            //string strResponse = "";
//            ////strResponse = submitRequest2("https://ex13-1.jnittech.com/owa/login.aspx?replaceCurrent=1&url=https://ex13-1.jnittech.com/owa", "POST", "username=jnittech\\administrator&Password=Pa$$w0rd&trusted=4", "jnittech\\administrator", "Pa$$w0rd");
//            //strResponse = submitRequest2("http://azphxdom1.rprwyatt.com//api/traveler/devices", "GET", "Username=alan+forbes&Password=lotusnotes&RedirectTo=/api/traveler/devices", "alan forbes", "lotusnotes");

//            ////string strURL = "https://ex13-1.jnittech.com/owa/login.aspx?replaceCurrent=1&url=https://ex13-1.jnittech.com/owa";
//            //string strURL = "https://mail01.gbc.edu/exchweb/bin/auth/owalogon.asp?url=https://mail01.gbc.edu/exchange&reason=0";
//            ////string strURL = "http://azphxdom1.rprwyatt.com/api/traveler/devices";

//            //Chilkat.Http WebPage = new Chilkat.Http();
//            //bool success = false;
//            //try
//            //{


//            //    //WebPage.MimicIE = true;
//            //    //WebPage.S3Ssl = true;
//            //    success = WebPage.UnlockComponent("MZLDADHttp_efwTynJYYR3X");


//            //}
//            //catch (Exception ex)
//            //{
//            //}



//            //try
//            //{

//            //    if ((success != true))
//            //    {
//            //        strResponse = WebPage.LastErrorText;


//            //    }
//            //    else
//            //    {


//            //        try
//            //        {
//            //            WebPage.Password = "1234!@#$";
//            //            WebPage.Login = "Administrator";
//            //            WebPage.MimicFireFox = true;
//            //            //WebPage.Login = "alan forbes";
//            //            //WebPage.Password = "lotusnotes";
//            //            //WebPage.MimicIE = true;
//            //            strURL = "http://craterlake.qsius.com/winlims7.JBS742/Default.aspx";
//            //            //WebPage.FollowRedirects = true;
//            //            //Chilkat.HttpRequest req = new Chilkat.HttpRequest();
//            //            //req.AddParam("username", "jnittech\administrator");
//            //            //req.AddParam("trusted", "4");
//            //            //req.AddParam("password", "Pa$$w0rd");
//            //            ////req.AddHeader("Content-Type", "application/x-www-form-urlencoded");
//            //            //req.ContentType = "application/x-www-form-urlencoded";

//            //            //byte[] bytes = System.Text.Encoding.ASCII.GetBytes("username=jnittech\administrator&password=Pa$$w0rd&trusted=4");
//            //            //req.AddHeader("Content-Length", bytes.Length.ToString());
//            //            //req.
//            //            //System.IO.Stream os = req.GetRequestStream();
//            //            //os.Write(bytes, 0, bytes.Length); //Push it out there
//            //            //os.Close();
//            //            //Chilkat.HttpResponse resp = WebPage.PostUrlEncoded(strURL, req);
//            //            //strResponse = resp.Body;

//            //            strResponse = WebPage.QuickGetStr(strURL);


//            //        }
//            //        catch (Exception ex)
//            //        {
//            //        }



//            //    }

//            //}
//            //catch (Exception ex)
//            //{
//            //}
//            //finally
//            //{
//            //    WebPage.Dispose();

//            //}



//        }
//        private void firstCalltoExchange()
//        {
//            string URL1 = "https://jnittech-exchg1.jnittech.com/owa/auth/logon.aspx?url=https://jnittech-exchg1.jnittech.com/owa/&reason=0";
//            string URL2 = "https://jnittech-exchg1.jnittech.com/owa/auth.owa";
//            string URL3 = "https://jnittech-exchg1.jnittech.com/owa";
//            string USERNAME = "jnittech\\administrator";
//            string PASSWORD = "Pa$$w0rd";
//            string ck1 = "logondata=acc=1&lgn=" + HttpUtility.UrlEncode(USERNAME);
//            string ck2 = "";
//            CookieContainer cookies = new CookieContainer();

//            //---------------- FIRST CALL GET-----------------------
//            HttpWebRequest webRequest = WebRequest.Create(URL1) as HttpWebRequest;
//            webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            webRequest.ContentType = "application/x-www-form-urlencoded";
//            webRequest.Accept = "text/html, application/xhtml+xml, */*";
//            webRequest.Headers.Add("DNT", "1");

//            webRequest.Headers.Add("Cookie", ck1);
//            webRequest.KeepAlive = true;
//            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
//            delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
//                                    System.Security.Cryptography.X509Certificates.X509Chain chain,
//                                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
//            {
//                return true; // **** Always accept
//            };
//            webRequest.CookieContainer = cookies;
//            WebResponse webresp = webRequest.GetResponse();
//            //ck1 = webresp.Headers.Get(7).ToString();
//            ck1 = cookies.GetCookieHeader(new Uri("https://jnittech-exchg1.jnittech.com")).ToString();

//            //StreamReader responseReader = webRequest.GetResponse().GetResponseStream();
//            StreamReader responseReader = new StreamReader(
//                  webresp.GetResponseStream()
//               );
//            string responseData = responseReader.ReadToEnd();
//            responseReader.Close();

//            //-----------SECOND CALL-POST-------------------------------
//            string postData =
//                  String.Format(
//                     "destination={0}&flags=4&forcedownlevel=0&trusted=4&username={1}&password={2}&isUtf8=1",
//                    HttpUtility.UrlEncode(URL3), HttpUtility.UrlEncode(USERNAME), HttpUtility.UrlEncode(PASSWORD)
//                  );

//            //postData = HttpUtility.HtmlEncode(postData);
//            webRequest = WebRequest.Create(URL2) as HttpWebRequest;
//            webRequest.Method = "POST";
//            webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            webRequest.Accept = "text/html, application/xhtml+xml, */*";
//            webRequest.Headers.Add("DNT", "1");
//            webRequest.KeepAlive = true;
//            webRequest.Referer = "https://jnittech-exchg1.jnittech.com/owa/auth/logon.aspx?url=https://jnittech-exchg1.jnittech.com/owa/&reason=0";
//            ck1 = ck1 + "; PBack=0";
//            //webRequest.AllowAutoRedirect = true;
//            //CookieContainer cookies2 = new CookieContainer();
//            //webRequest.CookieContainer = cookies2;
//            webRequest.Headers.Add("Cookie", ck1);
//            webRequest.Headers.Add("Accept-Language", "en-US");
//            //webRequest.Connection.a
//            webRequest.Headers.Add("Cache-Control", "no-cache");
//            webRequest.ContentType = "application/x-www-form-urlencoded";
//            //webRequest.Connection = "Keep-Alive";
//            //webRequest.CookieContainer = cookies;
//            webRequest.AllowAutoRedirect = false;
//            //cookies.Add(new Cookie("PBack", "0","","https://jnittech-exchg1.jnittech.com"));
//            // write the form values into the request message
//            StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
//            requestWriter.Write(postData);
//            requestWriter.Close();
//            webresp = webRequest.GetResponse();
//            WebHeaderCollection webh = webresp.Headers;
//            string headerCookie = webh["Set-Cookie"].ToString();

//            //ck2 = webresp.Headers.Get(3).ToString();
//            //ck2 = webresp.Headers.ToString();
//            string strCADataCookie = Regex.Replace(headerCookie, "(.*)cadata=\"(.*)\"(.*)", "$2");
//            string strSessionIDCookie = Regex.Replace(headerCookie, "(.*)sessionid=(.*)(,|;)(.*)", "$2");
//            ck2 = ck1 + "; " + "cadata=" + strCADataCookie + "; sessionid=" + strSessionIDCookie;
//            // we don't need the contents of the response, just the cookie it issues
//            webRequest.GetResponse().Close();

//            //------------------- LAST CALL GET---------------------------
//            webRequest = WebRequest.Create(URL3) as HttpWebRequest;
//            webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            //webRequest.ContentType = "application/x-www-form-urlencoded";
//            webRequest.Accept = "text/html, application/xhtml+xml, */*";
//            webRequest.Headers.Add("DNT", "1");
//            webRequest.Headers.Add("Accept-Language", "en-US");
//            //webRequest.Connection.a
//            webRequest.Headers.Add("Cache-Control", "no-cache");
//            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
//            webRequest.Headers.Add("Cookie", ck2);
//            webRequest.Referer = "https://jnittech-exchg1.jnittech.com/owa/auth/logon.aspx?url=https://jnittech-exchg1.jnittech.com/owa/&reason=0";
//            webRequest.KeepAlive = true;
//            //webRequest.AllowAutoRedirect = false;
//            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
//            delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
//                                    System.Security.Cryptography.X509Certificates.X509Chain chain,
//                                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
//            {
//                return true; // **** Always accept
//            };

//            //webRequest.CookieContainer = cookies;
//            webresp = webRequest.GetResponse();
//            ck1 = webresp.Headers.Get(7).ToString();
//            //StreamReader responseReader = webRequest.GetResponse().GetResponseStream();
//            responseReader = new StreamReader(
//                  webresp.GetResponseStream()
//               );
//            responseData = responseReader.ReadToEnd();
//            responseReader.Close();
//            bool owaTest = false;
//            if (ck1.ToLower().Contains("usercontext"))
//                owaTest = true;
//            string result = "";

//        }
//        private void formAuthExchg()
//        {
//            firstCalltoExchange();
//            string LOGIN_URL = "https://jnittech-exchg1.jnittech.com/owa";
//            string SECRET_PAGE_URL = "https://jnittech-exchg1.jnittech.com/owa";
//            string USERNAME = "jnittech%5Cadministrator";
//            string PASSWORD = "Pa%24%24w0rd";

//            CookieContainer cookies = new CookieContainer();
//            // first, request the login form to get the viewstate value
//            HttpWebRequest webRequest = WebRequest.Create(LOGIN_URL) as HttpWebRequest;
//            webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            webRequest.ContentType = "application/x-www-form-urlencoded";
//            webRequest.Accept = "text/html, application/xhtml+xml, */*";
//            webRequest.Headers.Add("DNT", "1");
//            webRequest.KeepAlive = true;
//            webRequest.Referer = "https://jnittech-exchg1.jnittech.com/owa/auth/logon.aspx?url=https://jnittech-exchg1.jnittech.com/owa/&reason=0";
//            //webRequest.AllowAutoRedirect = true;
//            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
//delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
//                        System.Security.Cryptography.X509Certificates.X509Chain chain,
//                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
//{
//    return true; // **** Always accept
//};

//            //webRequest.ContentType = "application/x-www-form-urlencoded";
//            webRequest.CookieContainer = cookies;
//            WebResponse webresp = webRequest.GetResponse();
//            string ck = webresp.Headers.Get(7).ToString();
//            //StreamReader responseReader = webRequest.GetResponse().GetResponseStream();
//            StreamReader responseReader = new StreamReader(
//                  webresp.GetResponseStream()
//               );
//            string responseData = responseReader.ReadToEnd();
//            responseReader.Close();

//            // extract the viewstate value and build out POST data
//            //string viewState = ExtractViewState(responseData);
//            //string eventValidation = ExtractEventValidation(responseData);
//            //string viewstategenerator = ExtractViewStateGenerator(responseData);
//            //string wpps = ExtractWPPS(responseData);
//            //string viewState = ExtractViewState(responseData);  
//            string postData =
//                  String.Format(
//                     "destination=https%3A%2F%2Fjnittech-exchg1.jnittech.com%2Fowa%2F&flags=4&forcedownlevel=0&trusted=4&username=jnittech%5Cadministrator&password=Pa%24%24w0rd&isUtf8=1",
//                    USERNAME, PASSWORD
//                  );

//            // have a cookie container ready to receive the forms auth cookie
//            LOGIN_URL = "https://jnittech-exchg1.jnittech.com/owa/auth.owa";
//            // now post to the login form
//            webRequest = WebRequest.Create(LOGIN_URL) as HttpWebRequest;
//            webRequest.Method = "POST";
//            webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            webRequest.Accept = "text/html, application/xhtml+xml, */*";
//            webRequest.Headers.Add("DNT", "1");
//            webRequest.KeepAlive = true;
//            webRequest.Referer = "https://jnittech-exchg1.jnittech.com/owa/auth/logon.aspx?url=https://jnittech-exchg1.jnittech.com/owa/&reason=0";
//            //ck = ck + "; PBack=0";
//            //webRequest.AllowAutoRedirect = true;
//            webRequest.Headers.Add("Cookie", ck);
//            webRequest.Headers.Add("Accept-Language", "en-US");
//            //webRequest.Connection.a
//            webRequest.Headers.Add("Cache-Control", "no-cache");
//            webRequest.ContentType = "application/x-www-form-urlencoded";
//            webRequest.Connection = "Keep-Alive";
//            webRequest.CookieContainer = cookies;
//            //cookies.Add(new Cookie("PBack", "0","","https://jnittech-exchg1.jnittech.com"));
//            // write the form values into the request message
//            StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
//            requestWriter.Write(postData);
//            requestWriter.Close();

//            // we don't need the contents of the response, just the cookie it issues
//            webRequest.GetResponse().Close();

//            // now we can send out cookie along with a request for the protected page
//            webRequest = WebRequest.Create(SECRET_PAGE_URL) as HttpWebRequest;
//            webRequest.CookieContainer = cookies;
//            webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            webRequest.Accept = "text/html, application/xhtml+xml, */*";
//            webRequest.ContentType = "application/x-www-form-urlencoded";
//            webRequest.Headers.Add("DNT", "1");
//            //webRequest.Headers.Add("Connection", "Keep-Alive");
//            webRequest.Connection = "Keep-Alive";
//            webRequest.Headers.Add("Cache-Control", "no-cache");
//            webRequest.KeepAlive = true;
//            webRequest.Referer = "https://jnittech-exchg1.jnittech.com/owa/auth/logon.aspx?url=https://jnittech-exchg1.jnittech.com/owa/&reason=0";
//            //webRequest.AllowAutoRedirect = true;
//            responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());

//            // and read the response
//            responseData = responseReader.ReadToEnd();
//            responseReader.Close();

//            //Response.Write(responseData);         
//        }
//        private void formAuth0365()
//        {
//            string responseFromServer = "";
//            CookieContainer cookies = new CookieContainer();

//            try
//            {

//                string requestMethod = "GET";
//                string message = "";
//                string userId = "info@RPRVitalSigns.com";
//                string pwd = "V1talS1gns";
//                string URL = "https://login.microsoftonline.com/getuserrealm.srf";
//                //SharePointOnlineCredentials creds = new SharePointOnlineCredentials(userId, Common.String2SecureString(pwd));
//                System.Net.HttpWebRequest request = WebRequest.Create(URL) as HttpWebRequest;
//                System.Net.CredentialCache c = new System.Net.CredentialCache();
//                request.Credentials = BuildCredentials("https://outlook.office365.com", userId, pwd, "BASIC");
//                //request.Credentials = creds;
//                //System.Net.WebClient wc = new System.Net.WebClient();
//                request.CookieContainer = cookies;
//                request.Method = requestMethod;
//                //request.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED: f");

//                if (requestMethod == "POST")
//                {
//                    request.ContentType = "application/json";
//                    string s = "" + (char)34;
//                    message = message.Replace("'", s);
//                    byte[] bytes = System.Text.Encoding.ASCII.GetBytes(message);
//                    request.ContentLength = bytes.Length;
//                    System.IO.Stream os = request.GetRequestStream();
//                    os.Write(bytes, 0, bytes.Length); //Push it out there
//                    os.Close();
//                }
//                //SharePointOnlineCredentials 
//                //Microsoft.SharePoint.Client.ClientRuntimeContext.SetupRequestCredential(

//                System.Net.WebResponse ws = request.GetResponse();
//                Stream dataStream = ws.GetResponseStream();
//                // Open the stream using a StreamReader for easy access.
//                StreamReader reader = new StreamReader(dataStream);
//                responseFromServer = reader.ReadToEnd();
//                string ck = ws.Headers.Get(11).ToString();
//                URL = "https://outlook.office365.com/owa";

//                request = WebRequest.Create(URL) as HttpWebRequest;
//                request.Headers.Add("Cookie", ck);
//                //System.Net.CredentialCache c = new System.Net.CredentialCache();
//                request.Credentials = BuildCredentials("https://outlook.office365.com", userId, pwd, "BASIC");
//                request.CookieContainer = cookies;
//                request.Method = requestMethod;
//                ws = request.GetResponse();
//                dataStream = ws.GetResponseStream();
//                // Open the stream using a StreamReader for easy access.
//                reader = new StreamReader(dataStream);
//                responseFromServer = reader.ReadToEnd();
//            }
//            catch (Exception ex)
//            {
//                responseFromServer = "-1";
//                string s = ex.Message.ToString();
//            }
//            //return responseFromServer;

//            string LOGIN_URL = "https://login.microsoftonline.com/login.srf";
//            string SECRET_PAGE_URL = "https://outlook.office365.com/owa";
//            string USERNAME = "info@RPRVitalSigns.com";
//            string PASSWORD = "V1talS1gns";

//            // first, request the login form to get the viewstate value
//            HttpWebRequest webRequest = WebRequest.Create(LOGIN_URL) as HttpWebRequest;
//            webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            webRequest.ContentType = "application/x-www-form-urlencoded";
//            webRequest.Accept = "text/html, application/xhtml+xml, */*";
//            webRequest.Headers.Add("DNT", "1");
//            webRequest.KeepAlive = true;
//            //webRequest.Referer = "https://jnittech-exchg1.jnittech.com/owa/auth/logon.aspx?url=https://jnittech-exchg1.jnittech.com/owa/&reason=0";
//            //webRequest.AllowAutoRedirect = true;
//            //            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
//            //delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
//            //                        System.Security.Cryptography.X509Certificates.X509Chain chain,
//            //                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
//            //{
//            //    return true; // **** Always accept
//            //};

//            //webRequest.ContentType = "application/x-www-form-urlencoded";
//            webRequest.CookieContainer = cookies;
//            WebResponse webresp = webRequest.GetResponse();
//            //string ck = webresp.Headers.Get(7).ToString();
//            //StreamReader responseReader = webRequest.GetResponse().GetResponseStream();
//            StreamReader responseReader = new StreamReader(
//                  webresp.GetResponseStream()
//               );
//            string responseData = responseReader.ReadToEnd();
//            responseReader.Close();

//            // extract the viewstate value and build out POST data
//            //string viewState = ExtractViewState(responseData);
//            //string eventValidation = ExtractEventValidation(responseData);
//            //string viewstategenerator = ExtractViewStateGenerator(responseData);
//            //string wpps = ExtractWPPS(responseData);
//            //string viewState = ExtractViewState(responseData);  
//            string postData =
//                  String.Format(
//                     "login=info@RPRVitalSigns.com&passwd=V1talS1gns&PPSX=Passpo&PPFT=A0lTWhxQNuzJNlDID181OiP5*9bvYC%210D%21qWjPEVqfNtWrqc0XBfRU6N5mbZVVcqC4r07sioa9JqBT%21WW3VqNiO7ncJGp3LC0IMCJ89pi9NOFqXRI1S1H9a08YRwT2trDpm8019iiqmvS0Q79Zbp5zu0IxR8WZoXHD%21oV4wAFa4pxsGKYDxLMdPIkI1hjnTPQv3rW8Pd2RhUKmbwoA%24%24&n1=65358&n2=-1423543347000&n3=-1423543347000&n4=65388&n5=65388&n6=65388&n7=65388&n8=NaN&n9=65388&n10=65612&n11=65612&n12=65612&n13=65612&n14=66542&n15=48&n16=66590&n17=66591&n18=66595&n19=48.48498020291299&n20=1&n21=0&n22=0&n23=1&n24=27.38237432841288&n25=1&n26=0&n27=0&n28=0&n29=-1423543412834&n30=-1423543412834&n31=0&n32=0&n33=0&n34=0&n35=0&n36=0&n37=0&n38=0&n39=0&n40=1238.8938141382932&n41=1240.921064904323&n42=1206.2036409013237&n43=1237.2976107358563&type=11&LoginOptions=2&NewUser=1&idsbho=1&PwdPad=&sso=&vv=&uiver=1&i12=1&i13=Firefox&i14=11.0&i15=1920&i16=963",
//                    USERNAME, PASSWORD
//                  );

//            // have a cookie container ready to receive the forms auth cookie
//            LOGIN_URL = "https://login.microsoftonline.com/ppsecure/post.srf";
//            // now post to the login form
//            webRequest = WebRequest.Create(LOGIN_URL) as HttpWebRequest;
//            webRequest.Method = "POST";
//            webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            webRequest.Accept = "text/html, application/xhtml+xml, */*";
//            webRequest.Headers.Add("DNT", "1");
//            webRequest.KeepAlive = true;
//            //webRequest.Referer = "https://jnittech-exchg1.jnittech.com/owa/auth/logon.aspx?url=https://jnittech-exchg1.jnittech.com/owa/&reason=0";
//            //ck = ck + "; PBack=0";
//            //webRequest.AllowAutoRedirect = true;
//            //webRequest.Headers.Add("Cookie", ck);
//            webRequest.Headers.Add("Accept-Language", "en-US");
//            //webRequest.Connection.a
//            webRequest.Headers.Add("Cache-Control", "no-cache");
//            webRequest.ContentType = "application/x-www-form-urlencoded";
//            webRequest.Connection = "Keep-Alive";
//            webRequest.CookieContainer = cookies;
//            //cookies.Add(new Cookie("PBack", "0","","https://jnittech-exchg1.jnittech.com"));
//            // write the form values into the request message
//            StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
//            requestWriter.Write(postData);
//            requestWriter.Close();

//            // we don't need the contents of the response, just the cookie it issues
//            webRequest.GetResponse().Close();

//            // now we can send out cookie along with a request for the protected page
//            webRequest = WebRequest.Create(SECRET_PAGE_URL) as HttpWebRequest;
//            webRequest.CookieContainer = cookies;
//            webRequest.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            webRequest.Accept = "text/html, application/xhtml+xml, */*";
//            webRequest.ContentType = "application/x-www-form-urlencoded";
//            webRequest.Headers.Add("DNT", "1");
//            //webRequest.Headers.Add("Connection", "Keep-Alive");
//            webRequest.Connection = "Keep-Alive";
//            webRequest.Headers.Add("Cache-Control", "no-cache");
//            webRequest.KeepAlive = true;
//            //webRequest.Referer = "https://jnittech-exchg1.jnittech.com/owa/auth/logon.aspx?url=https://jnittech-exchg1.jnittech.com/owa/&reason=0";
//            //webRequest.AllowAutoRedirect = true;
//            responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());

//            // and read the response
//            responseData = responseReader.ReadToEnd();
//            responseReader.Close();

//            //Response.Write(responseData);         
//        }
//        //private static void GetPerson()
//        //{
//        //    //Console.WriteLine("Username:");
//        //    string username = "jnittech\administrator";

//        //    //Console.WriteLine("Password:");
//        //    string password = "Pa$$w0rd";
//        //    //jnittech\\administrator&Password=Pa$$w0rd
//        //    HttpClient httpClient = new HttpClient();

//        //    HttpRequestMessage authRequest = new HttpRequestMessage();
//        //    authRequest.Method = HttpMethod.Post;
//        //    authRequest.RequestUri = new Uri(@"https://jnittech-exchg1.jnittech.com/owa/auth/logon.aspx");
//        //    authRequest.Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
//        //    {
//        //        new KeyValuePair<string, string>("UserName", username),
//        //        new KeyValuePair<string, string>("Password", password)
//        //    });

//        //    HttpResponseMessage authResponse = httpClient.SendAsync(authRequest).Result;

//        //    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, @"https://jnittech-exchg1.jnittech.com/owa");
//        //    request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

//        //    HttpResponseMessage response = httpClient.SendAsync(request).Result;

//        //    if (!response.IsSuccessStatusCode)
//        //    {
//        //        Console.WriteLine("Username or password is incorrect");
//        //        return;
//        //    }

//        //    string s1 = response.Content.ReadAsStringAsync().Result;
//        //    // Open the stream using a StreamReader for easy access.
//        //    //StreamReader reader = new StreamReader(dataStream);
//        //    //string s = reader.ReadToEnd();

//        //    //response.Content.ReadAsAsync<Person>().ContinueWith((x) =>
//        //    //{
//        //    //    Person person = x.Result;

//        //    //    Console.WriteLine("First name: {0}", person.FirstName);
//        //    //    Console.WriteLine("Last name: {0}", person.LastName);
//        //    //});
//        //}
//        private void formAuthGood()
//        {
//            string LOGIN_URL = "http://hyderabad/winlims7.jbs2/PagesAnon/userLogin.aspx";
//            string SECRET_PAGE_URL = "http://hyderabad/winlims7.jbs2/default.aspx";
//            string USERNAME = "Administrator";
//            string PASSWORD = "1234%21@%23%24";
//            CookieContainer cookies = new CookieContainer();
//            // first, request the login form to get the viewstate value
//            HttpWebRequest webRequest = WebRequest.Create(LOGIN_URL) as HttpWebRequest;
//            webRequest.ContentType = "application/x-www-form-urlencoded";
//            webRequest.CookieContainer = cookies;
//            WebResponse webresp = webRequest.GetResponse();
//            string ck = webresp.Headers.Get(4).ToString();
//            //StreamReader responseReader = webRequest.GetResponse().GetResponseStream();
//            StreamReader responseReader = new StreamReader(
//                  webresp.GetResponseStream()
//               );
//            string responseData = responseReader.ReadToEnd();
//            responseReader.Close();

//            // extract the viewstate value and build out POST data
//            string viewState = ExtractViewState(responseData);
//            string eventValidation = ExtractEventValidation(responseData);
//            string viewstategenerator = ExtractViewStateGenerator(responseData);
//            string wpps = ExtractWPPS(responseData);
//            //string viewState = ExtractViewState(responseData);  
//            string postData =
//                  String.Format(
//                     "__EVENTTRAGET=&__EVENTARGUMENT&__LASTFOCUS=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={3}&__WPPS={4}&__EVENTVALIDATION={5}&ctl00$MiddleContent$LoginView1$Login1$UserName={1}&ctl00$MiddleContent$LoginView1$Login1$Password={2}&ctl00$MiddleContent$LoginView1$Login1$RememberMe=on&ctl00$MiddleContent$LoginView1$Login1$LoginButton=Log+In",
//                     viewState, USERNAME, PASSWORD, viewstategenerator, wpps, eventValidation
//                  );

//            // have a cookie container ready to receive the forms auth cookie

//            // now post to the login form
//            webRequest = WebRequest.Create(LOGIN_URL) as HttpWebRequest;
//            webRequest.Method = "POST";
//            webRequest.Headers.Add("Cookie", ck);
//            webRequest.ContentType = "application/x-www-form-urlencoded";
//            webRequest.CookieContainer = cookies;

//            // write the form values into the request message
//            StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
//            requestWriter.Write(postData);
//            requestWriter.Close();

//            // we don't need the contents of the response, just the cookie it issues
//            webRequest.GetResponse().Close();

//            // now we can send out cookie along with a request for the protected page
//            webRequest = WebRequest.Create(SECRET_PAGE_URL) as HttpWebRequest;
//            webRequest.CookieContainer = cookies;
//            responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());

//            // and read the response
//            responseData = responseReader.ReadToEnd();
//            responseReader.Close();

//            //Response.Write(responseData);         
//        }
//        private void formAuthJira()
//        {
//            string LOGIN_URL = "http://jira.jnitinc.com/secure/Dashboard.jspa";
//            string SECRET_PAGE_URL = "http://jira.jnitinc.com/secure/Dashboard.jspa";
//            string USERNAME = "dhanraj";
//            string PASSWORD = "1234%40%24!%23";
//            CookieContainer cookies = new CookieContainer();
//            // first, request the login form to get the viewstate value
//            HttpWebRequest webRequest = WebRequest.Create(LOGIN_URL) as HttpWebRequest;
//            webRequest.ContentType = "application/x-www-form-urlencoded";
//            webRequest.CookieContainer = cookies;
//            WebResponse webresp = webRequest.GetResponse();
//            string ck = webresp.Headers.Get(6).ToString();
//            //StreamReader responseReader = webRequest.GetResponse().GetResponseStream();
//            StreamReader responseReader = new StreamReader(
//                  webresp.GetResponseStream()
//               );
//            string responseData = responseReader.ReadToEnd();
//            responseReader.Close();

//            // extract the viewstate value and build out POST data
//            //string viewState = ExtractViewState(responseData);
//            //string eventValidation = ExtractEventValidation(responseData);
//            //string viewstategenerator = ExtractViewStateGenerator(responseData);
//            //string wpps = ExtractWPPS(responseData);
//            //string viewState = ExtractViewState(responseData);  
//            string postData =
//                  String.Format(
//                     "os_username={0}&os_password={1}&os_cookie=true",
//                      USERNAME, PASSWORD
//                  );

//            // have a cookie container ready to receive the forms auth cookie

//            // now post to the login form
//            webRequest = WebRequest.Create(LOGIN_URL) as HttpWebRequest;
//            webRequest.Method = "POST";
//            webRequest.Headers.Add("Cookie", ck);
//            webRequest.ContentType = "application/x-www-form-urlencoded";
//            webRequest.CookieContainer = cookies;

//            // write the form values into the request message
//            StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
//            requestWriter.Write(postData);
//            requestWriter.Close();

//            // we don't need the contents of the response, just the cookie it issues
//            webRequest.GetResponse().Close();

//            // now we can send out cookie along with a request for the protected page
//            webRequest = WebRequest.Create(SECRET_PAGE_URL) as HttpWebRequest;
//            webRequest.CookieContainer = cookies;
//            responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());

//            // and read the response
//            responseData = responseReader.ReadToEnd();
//            responseReader.Close();

//            //Response.Write(responseData);         
//        }
//        private string ExtractViewState(string s)
//        {
//            string viewStateNameDelimiter = "__VIEWSTATE";
//            string valueDelimiter = "value=\"";

//            int viewStateNamePosition = s.IndexOf(viewStateNameDelimiter);
//            int viewStateValuePosition = s.IndexOf(
//                  valueDelimiter, viewStateNamePosition
//               );

//            int viewStateStartPosition = viewStateValuePosition +
//                                         valueDelimiter.Length;
//            int viewStateEndPosition = s.IndexOf("\"", viewStateStartPosition);

//            return HttpUtility.UrlEncodeUnicode(
//                     s.Substring(
//                        viewStateStartPosition,
//                        viewStateEndPosition - viewStateStartPosition
//                     )
//                  );
//        }
//        private string ExtractEventValidation(string s)
//        {
//            string viewStateNameDelimiter = "__EVENTVALIDATION";
//            string valueDelimiter = "value=\"";

//            int viewStateNamePosition = s.IndexOf(viewStateNameDelimiter);
//            int viewStateValuePosition = s.IndexOf(
//                  valueDelimiter, viewStateNamePosition
//               );

//            int viewStateStartPosition = viewStateValuePosition +
//                                         valueDelimiter.Length;
//            int viewStateEndPosition = s.IndexOf("\"", viewStateStartPosition);

//            return HttpUtility.UrlEncodeUnicode(
//                     s.Substring(
//                        viewStateStartPosition,
//                        viewStateEndPosition - viewStateStartPosition
//                     )
//                  );
//        }
//        private string ExtractViewStateGenerator(string s)
//        {
//            string viewStateNameDelimiter = "__VIEWSTATEGENERATOR";
//            string valueDelimiter = "value=\"";

//            int viewStateNamePosition = s.IndexOf(viewStateNameDelimiter);
//            int viewStateValuePosition = s.IndexOf(
//                  valueDelimiter, viewStateNamePosition
//               );

//            int viewStateStartPosition = viewStateValuePosition +
//                                         valueDelimiter.Length;
//            int viewStateEndPosition = s.IndexOf("\"", viewStateStartPosition);

//            return HttpUtility.UrlEncodeUnicode(
//                     s.Substring(
//                        viewStateStartPosition,
//                        viewStateEndPosition - viewStateStartPosition
//                     )
//                  );
//        }
//        private string ExtractWPPS(string s)
//        {
//            string viewStateNameDelimiter = "__WPPS";
//            string valueDelimiter = "value=\"";

//            int viewStateNamePosition = s.IndexOf(viewStateNameDelimiter);
//            int viewStateValuePosition = s.IndexOf(
//                  valueDelimiter, viewStateNamePosition
//               );

//            int viewStateStartPosition = viewStateValuePosition +
//                                         valueDelimiter.Length;
//            int viewStateEndPosition = s.IndexOf("\"", viewStateStartPosition);

//            return HttpUtility.UrlEncodeUnicode(
//                     s.Substring(
//                        viewStateStartPosition,
//                        viewStateEndPosition - viewStateStartPosition
//                     )
//                  );
//        }
//        private void test1()
//        {
//            string url = "http://azphxdom1.rprwyatt.com/api/traveler/devices";
//            //string url = "https://jnittech-exchg1.jnittech.com/owa";
//            //string url = "http://craterlake.qsius.com/winlims7.jbs742/PagesAnon/userLogin.aspx?ReturnUrl=/winlims7.jbs2/default.aspx";
//            HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;
//            //req.CookieContainer = new CookieContainer();

//            System.Net.CredentialCache c = new System.Net.CredentialCache();
//            //string user = "administrator";
//            //string pwd = "1234!@#$";
//            string user = "alan forbes";
//            string pwd = "lotusnotes";
//            //string user = "jnittech\administrator";
//            //string pwd = "Pa$$w0rd";
//            //string domain = "http://craterlake.qsius.com";
//            //string domain = "https://jnittech-exchg1.jnittech.com";
//            string domain = "http://azphxdom1.rprwyatt.com";

//            req.Credentials = BuildCredentials(domain, user, pwd, "BASIC");
//            //req.Method = "POST";
//            //req.Connection = "Keep-Alive";
//            //req.Headers.Add("Connection", "Keep-Alive");
//            req.Headers.Add("DNT", "1");
//            string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(user + ":" + pwd));
//            req.PreAuthenticate = true;
//            req.Headers.Add("Authorization", auth);
//            req.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            req.AllowAutoRedirect = true;
//            req.ContentType = "application/x-www-form-urlencoded";
//            //string message = "ctl00$MiddleContent$LoginView1$Login1$UserName=Administrator&ctl00$MiddleContent$LoginView1$Login1$Password=1234!@#$&ctl00$MiddleContent$LoginView1$Login1$RememberMe=on&ctl00$MiddleContent$LoginView1$Login1$LoginButton=Log+In";
//            string message = "ctl00$MiddleContent$LoginView1$Login1$UserName=Administrator&ctl00$MiddleContent$LoginView1$Login1$Password=1234!@#$&ctl00$MiddleContent$LoginView1$Login1$RememberMe=on&ctl00$MiddleContent$LoginView1$Login1$LoginButton=Log+In";
//            int len = message.Length;

//            byte[] bytes = System.Text.Encoding.ASCII.GetBytes("ctl00$MiddleContent$LoginView1$Login1$UserName=Administrator&ctl00$MiddleContent$LoginView1$Login1$Password=1234!@#$&ctl00$MiddleContent$LoginView1$Login1$RememberMe=on&ctl00$MiddleContent$LoginView1$Login1$LoginButton=Log+In");
//            //req.ContentLength = message.Length;
//            //System.IO.Stream os = req.GetRequestStream();
//            //os.Write(bytes, 0, bytes.Length); //Push it out there
//            //os.Close();

//            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
//delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
//                        System.Security.Cryptography.X509Certificates.X509Chain chain,
//                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
//{
//    return true; // **** Always accept
//};

//            WebResponse resp = req.GetResponse();
//            Stream dataStream = resp.GetResponseStream();
//            // Open the stream using a StreamReader for easy access.
//            StreamReader reader = new StreamReader(dataStream);
//            string s = reader.ReadToEnd();
//            string ck = resp.Headers.Get(3).ToString();

//            resp.Close();


//            req = HttpWebRequest.Create(url) as HttpWebRequest;
//            req.PreAuthenticate = true;
//            req.Headers.Add("Cookie", ck);
//            //req.Credentials = new NetworkCredential(user, pwd, domain);
//            req.UserAgent = ": Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.3) Gecko/20090824 Firefox/3.5.3 (.NET CLR 4.0.20506)";
//            req.Method = "POST";
//            //req.Connection = "Keep-Alive";
//            //req.Headers.Add("Connection", "Keep-Alive");
//            req.Headers.Add("DNT", "1");
//            //string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(user + ":" + pwd));
//            //req.PreAuthenticate = true;
//            //req.Headers.Add("Authorization", auth);
//            req.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            req.AllowAutoRedirect = true;
//            req.ContentType = "application/x-www-form-urlencoded";
//            req.ContentLength = message.Length;
//            System.IO.Stream os1 = req.GetRequestStream();
//            os1.Write(bytes, 0, bytes.Length); //Push it out there
//            os1.Close();
//            resp = req.GetResponse();
//            resp.Close();

//            req = HttpWebRequest.Create(url) as HttpWebRequest;
//            req.PreAuthenticate = true;
//            req.Credentials = new NetworkCredential(user, pwd, domain);
//            req.UserAgent = ": Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.3) Gecko/20090824 Firefox/3.5.3 (.NET CLR 4.0.20506)";
//            resp = req.GetResponse();

//        }
//        private void testBasicAuthO365OWA()
//        {
//            string url = "https://outlook.office365.com/owa";
//            //string url = "https://jnittech-exchg1.jnittech.com/owa";
//            //string url = "http://craterlake.qsius.com/winlims7.jbs742/PagesAnon/userLogin.aspx?ReturnUrl=/winlims7.jbs2/default.aspx";
//            HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;
//            //req.CookieContainer = new CookieContainer();

//            System.Net.CredentialCache c = new System.Net.CredentialCache();
//            //string user = "administrator";
//            //string pwd = "1234!@#$";
//            string user = "info@RPRVitalSigns.com";
//            string pwd = "V1talS1gns";
//            //string user = "jnittech\administrator";
//            //string pwd = "Pa$$w0rd";
//            //string domain = "http://craterlake.qsius.com";
//            //string domain = "https://jnittech-exchg1.jnittech.com";
//            string domain = "https://outlook.office365.com";

//            req.Credentials = BuildCredentials(domain, user, pwd, "BASIC");
//            //req.Method = "POST";
//            //req.Connection = "Keep-Alive";
//            //req.Headers.Add("Connection", "Keep-Alive");
//            req.Headers.Add("DNT", "1");
//            string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(user + ":" + pwd));
//            req.PreAuthenticate = true;
//            req.Headers.Add("Authorization", auth);
//            req.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            req.AllowAutoRedirect = true;
//            req.ContentType = "application/x-www-form-urlencoded";
//            //string message = "ctl00$MiddleContent$LoginView1$Login1$UserName=Administrator&ctl00$MiddleContent$LoginView1$Login1$Password=1234!@#$&ctl00$MiddleContent$LoginView1$Login1$RememberMe=on&ctl00$MiddleContent$LoginView1$Login1$LoginButton=Log+In";
//            string message = "ctl00$MiddleContent$LoginView1$Login1$UserName=Administrator&ctl00$MiddleContent$LoginView1$Login1$Password=1234!@#$&ctl00$MiddleContent$LoginView1$Login1$RememberMe=on&ctl00$MiddleContent$LoginView1$Login1$LoginButton=Log+In";
//            int len = message.Length;

//            byte[] bytes = System.Text.Encoding.ASCII.GetBytes("ctl00$MiddleContent$LoginView1$Login1$UserName=Administrator&ctl00$MiddleContent$LoginView1$Login1$Password=1234!@#$&ctl00$MiddleContent$LoginView1$Login1$RememberMe=on&ctl00$MiddleContent$LoginView1$Login1$LoginButton=Log+In");
//            //req.ContentLength = message.Length;
//            //System.IO.Stream os = req.GetRequestStream();
//            //os.Write(bytes, 0, bytes.Length); //Push it out there
//            //os.Close();

//            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
//delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
//                        System.Security.Cryptography.X509Certificates.X509Chain chain,
//                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
//{
//    return true; // **** Always accept
//};

//            WebResponse resp = req.GetResponse();
//            Stream dataStream = resp.GetResponseStream();
//            // Open the stream using a StreamReader for easy access.
//            StreamReader reader = new StreamReader(dataStream);
//            string s = reader.ReadToEnd();
//            string ck = resp.Headers.Get(3).ToString();

//            resp.Close();


//            req = HttpWebRequest.Create(url) as HttpWebRequest;
//            req.PreAuthenticate = true;
//            req.Headers.Add("Cookie", ck);
//            //req.Credentials = new NetworkCredential(user, pwd, domain);
//            req.UserAgent = ": Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.3) Gecko/20090824 Firefox/3.5.3 (.NET CLR 4.0.20506)";
//            req.Method = "POST";
//            //req.Connection = "Keep-Alive";
//            //req.Headers.Add("Connection", "Keep-Alive");
//            req.Headers.Add("DNT", "1");
//            //string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(user + ":" + pwd));
//            //req.PreAuthenticate = true;
//            //req.Headers.Add("Authorization", auth);
//            req.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            req.AllowAutoRedirect = true;
//            req.ContentType = "application/x-www-form-urlencoded";
//            req.ContentLength = message.Length;
//            System.IO.Stream os1 = req.GetRequestStream();
//            os1.Write(bytes, 0, bytes.Length); //Push it out there
//            os1.Close();
//            resp = req.GetResponse();
//            resp.Close();

//            req = HttpWebRequest.Create(url) as HttpWebRequest;
//            req.PreAuthenticate = true;
//            req.Credentials = new NetworkCredential(user, pwd, domain);
//            req.UserAgent = ": Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.3) Gecko/20090824 Firefox/3.5.3 (.NET CLR 4.0.20506)";
//            resp = req.GetResponse();

//        }
//        private void testBasicAuthWorking()
//        {
//            string url = "http://azphxdom1.rprwyatt.com/api/traveler/devices";
//            //string url = "https://jnittech-exchg1.jnittech.com/owa";
//            //string url = "http://craterlake.qsius.com/winlims7.jbs742/PagesAnon/userLogin.aspx?ReturnUrl=/winlims7.jbs2/default.aspx";
//            HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;
//            //req.CookieContainer = new CookieContainer();

//            System.Net.CredentialCache c = new System.Net.CredentialCache();
//            //string user = "administrator";
//            //string pwd = "1234!@#$";
//            string user = "alan forbes";
//            string pwd = "lotusnotes";
//            //string user = "jnittech\administrator";
//            //string pwd = "Pa$$w0rd";
//            //string domain = "http://craterlake.qsius.com";
//            //string domain = "https://jnittech-exchg1.jnittech.com";
//            string domain = "http://azphxdom1.rprwyatt.com";

//            req.Credentials = BuildCredentials(domain, user, pwd, "BASIC");
//            //req.Method = "POST";
//            //req.Connection = "Keep-Alive";
//            //req.Headers.Add("Connection", "Keep-Alive");
//            req.Headers.Add("DNT", "1");
//            string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(user + ":" + pwd));
//            req.PreAuthenticate = true;
//            req.Headers.Add("Authorization", auth);
//            req.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            req.AllowAutoRedirect = true;
//            req.ContentType = "application/x-www-form-urlencoded";
//            //string message = "ctl00$MiddleContent$LoginView1$Login1$UserName=Administrator&ctl00$MiddleContent$LoginView1$Login1$Password=1234!@#$&ctl00$MiddleContent$LoginView1$Login1$RememberMe=on&ctl00$MiddleContent$LoginView1$Login1$LoginButton=Log+In";
//            string message = "ctl00$MiddleContent$LoginView1$Login1$UserName=Administrator&ctl00$MiddleContent$LoginView1$Login1$Password=1234!@#$&ctl00$MiddleContent$LoginView1$Login1$RememberMe=on&ctl00$MiddleContent$LoginView1$Login1$LoginButton=Log+In";
//            int len = message.Length;

//            byte[] bytes = System.Text.Encoding.ASCII.GetBytes("ctl00$MiddleContent$LoginView1$Login1$UserName=Administrator&ctl00$MiddleContent$LoginView1$Login1$Password=1234!@#$&ctl00$MiddleContent$LoginView1$Login1$RememberMe=on&ctl00$MiddleContent$LoginView1$Login1$LoginButton=Log+In");
//            //req.ContentLength = message.Length;
//            //System.IO.Stream os = req.GetRequestStream();
//            //os.Write(bytes, 0, bytes.Length); //Push it out there
//            //os.Close();

//            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
//delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
//                        System.Security.Cryptography.X509Certificates.X509Chain chain,
//                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
//{
//    return true; // **** Always accept
//};

//            WebResponse resp = req.GetResponse();
//            Stream dataStream = resp.GetResponseStream();
//            // Open the stream using a StreamReader for easy access.
//            StreamReader reader = new StreamReader(dataStream);
//            string s = reader.ReadToEnd();
//            string ck = resp.Headers.Get(3).ToString();

//            resp.Close();


//            req = HttpWebRequest.Create(url) as HttpWebRequest;
//            req.PreAuthenticate = true;
//            req.Headers.Add("Cookie", ck);
//            //req.Credentials = new NetworkCredential(user, pwd, domain);
//            req.UserAgent = ": Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.3) Gecko/20090824 Firefox/3.5.3 (.NET CLR 4.0.20506)";
//            req.Method = "POST";
//            //req.Connection = "Keep-Alive";
//            //req.Headers.Add("Connection", "Keep-Alive");
//            req.Headers.Add("DNT", "1");
//            //string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(user + ":" + pwd));
//            //req.PreAuthenticate = true;
//            //req.Headers.Add("Authorization", auth);
//            req.UserAgent = ": Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
//            req.AllowAutoRedirect = true;
//            req.ContentType = "application/x-www-form-urlencoded";
//            req.ContentLength = message.Length;
//            System.IO.Stream os1 = req.GetRequestStream();
//            os1.Write(bytes, 0, bytes.Length); //Push it out there
//            os1.Close();
//            resp = req.GetResponse();
//            resp.Close();

//            req = HttpWebRequest.Create(url) as HttpWebRequest;
//            req.PreAuthenticate = true;
//            req.Credentials = new NetworkCredential(user, pwd, domain);
//            req.UserAgent = ": Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.3) Gecko/20090824 Firefox/3.5.3 (.NET CLR 4.0.20506)";
//            resp = req.GetResponse();

//        }
//        public string submitRequest2(string URL, string requestMethod, string message, string userId, string pwd)
//        {

//            //     using (System.Net.WebClient client = new System.Net.WebClient())
//            //     {

//            //         byte[] response =
//            //         client.UploadValues("https://jnittech-exchg1.jnittech.com/owa", new System.Collections.Specialized.NameValueCollection()
//            //{
//            //    { "username", "jnittech\administrator" },
//            //    { "password", "Pa$$w0rd" },
//            //    { "trusted", "4" }
//            //});

//            //         string result = System.Text.Encoding.UTF8.GetString(response);
//            //     }

//            using (System.Net.WebClient client = new System.Net.WebClient())
//            {

//                byte[] response =
//                client.UploadValues("http://azphxdom1.rprwyatt.com/names.nsf?Login", new System.Collections.Specialized.NameValueCollection()
//       {
//           { "username", "alan+forbes" },
//           { "password", "lotusnotes" },
//           { "RedirerctTo", "api/traveler/devices" }
//       });

//                string result = System.Text.Encoding.UTF8.GetString(response);
//            }

//            string responseFromServer = "";
//            try
//            {


//                //SharePointOnlineCredentials creds = new SharePointOnlineCredentials(userId, Common.String2SecureString(pwd));
//                System.Net.WebRequest request = System.Net.WebRequest.Create(URL);

//                System.Net.CredentialCache c = new System.Net.CredentialCache();
//                request.Credentials = BuildCredentials(URL, userId, pwd, "BASIC");
//                //request.Credentials = creds;
//                //System.Net.WebClient wc = new System.Net.WebClient();
//                //wc.Credentials = request.Credentials;
//                request.Method = requestMethod;
//                //request.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED: f");

//                if (requestMethod == "POST")
//                {
//                    //                    Accept: text/html, application/xhtml+xml, */*
//                    //Referer: http://azphxdom1.rprwyatt.com/api/traveler
//                    //Accept-Language: en-US
//                    //Content-Type: application/x-www-form-urlencoded
//                    //User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
//                    //Accept-Encoding: gzip, deflate
//                    //Connection: Keep-Alive
//                    //Content-Length: 100
//                    //DNT: 1
//                    //Host: azphxdom1.rprwyatt.com
//                    //Pragma: no-cache
//                    //request.Headers.Add("Accept", "text/html, application/xhtml+xml, */*");
//                    //request.Headers.Add("Referer", "http://azphxdom1.rprwyatt.com/api/traveler");
//                    request.Headers.Add("Accept-Language", "en-US");
//                    //request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko");
//                    //request.Headers.Add("Accept-Encoding", "gzip, deflate");
//                    //request.Headers.Add("Connection", "Keep-Alive");
//                    //request.Headers.Add("DNT", "1");
//                    //request.Headers.Add("Host", "azphxdom1.rprwyatt.com");

//                    request.ContentType = "application/x-www-form-urlencoded";
//                    string s = "" + (char)34;
//                    message = message.Replace("'", s);
//                    byte[] bytes = System.Text.Encoding.ASCII.GetBytes(message);
//                    request.ContentLength = bytes.Length;
//                    System.IO.Stream os = request.GetRequestStream();
//                    os.Write(bytes, 0, bytes.Length); //Push it out there
//                    os.Close();
//                }
//                //SharePointOnlineCredentials 
//                //Microsoft.SharePoint.Client.ClientRuntimeContext.SetupRequestCredential(
//                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
//delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
//                        System.Security.Cryptography.X509Certificates.X509Chain chain,
//                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
//{
//    return true; // **** Always accept
//};
//                System.Net.WebResponse ws = request.GetResponse();
//                Stream dataStream = ws.GetResponseStream();
//                // Open the stream using a StreamReader for easy access.
//                StreamReader reader = new StreamReader(dataStream);
//                responseFromServer = reader.ReadToEnd();
//            }
//            catch (Exception ex)
//            {
//                responseFromServer = "-1";
//                string s = ex.Message.ToString();
//            }
//            return responseFromServer;
//        }

//        public static void RedirectHTTPAuthExample(String username, String password)
//        {

//            NetworkCredential creds = new NetworkCredential(username, password);

//            CredentialCache credCache = new CredentialCache();

//            // Cached credentials can only be used when requested by

//            // specific URLs and authorization schemes

//            credCache.Add(new Uri("http://azphxdom1.rprwyatt.com/"), "Basic", creds);



//            WebRequest request = WebRequest.Create("http://azphxdom1.rprwyatt.com/api/traveler/devices/");

//            // Must be a cache, basic credentials are cleared on redirect

//            request.Credentials = credCache;

//            request.PreAuthenticate = true; // More efficient, but optional



//            WebResponse response = request.GetResponse();

//            StreamReader responseStreamReader =

//                new StreamReader(response.GetResponseStream());

//            String result = responseStreamReader.ReadToEnd();

//            responseStreamReader.Close();

//            Console.WriteLine(result);

//        }
		#endregion
	}
	#region supportingClasses

	public class CookieAwareWebClient : System.Net.WebClient
	{
		public CookieAwareWebClient()
		{
			CookieContainer = new System.Net.CookieContainer();
		}
		public System.Net.CookieContainer CookieContainer { get; private set; }

		protected override System.Net.WebRequest GetWebRequest(Uri address)
		{
			var request = (System.Net.HttpWebRequest)base.GetWebRequest(address);
			request.CookieContainer = CookieContainer;

			return request;
		}
	}


	class Product
	{
		public string Name { get; set; }
		public double Price { get; set; }
		public string Category { get; set; }
	}

	public class DataObject
	{
		public string Name { get; set; }
	}
	public class Body
	{
		public string ContentType { get; set; }
		public string Content { get; set; }
	}

	public class EmailAddress
	{
		public string Address { get; set; }
		public string Name { get; set; }
	}

	public class From
	{
		public EmailAddress EmailAddress { get; set; }
	}

	public class EmailAddress2
	{
		public string Address { get; set; }
		public string Name { get; set; }
	}

	public class Sender
	{
		public EmailAddress2 EmailAddress { get; set; }
	}

	public class EmailAddress3
	{
		public string Address { get; set; }
		public string Name { get; set; }
	}

	public class ToRecipient
	{
		public EmailAddress3 EmailAddress { get; set; }
	}

	public class Value
	{
		// public string __invalid_name__@odata.id { get; set; }
		// public string __invalid_name__@odata.etag { get; set; }
		public string Id { get; set; }
		public string ChangeKey { get; set; }
		public List<object> Categories { get; set; }
		public string DateTimeCreated { get; set; }
		public string DateTimeLastModified { get; set; }
		public string Subject { get; set; }
		public string BodyPreview { get; set; }
		public Body Body { get; set; }
		public string Importance { get; set; }
		public bool HasAttachments { get; set; }
		public string ParentFolderId { get; set; }
		public From From { get; set; }
		public Sender Sender { get; set; }
		public List<ToRecipient> ToRecipients { get; set; }
		public List<object> CcRecipients { get; set; }
		public List<object> BccRecipients { get; set; }
		public List<object> ReplyTo { get; set; }
		public string ConversationId { get; set; }
		public string DateTimeReceived { get; set; }
		public string DateTimeSent { get; set; }
		public object IsDeliveryReceiptRequested { get; set; }
		public bool IsReadReceiptRequested { get; set; }
		public bool IsDraft { get; set; }
		public bool IsRead { get; set; }
	}

	public class RootObject
	{
		//public string __invalid_name__@odata.context { get; set; }
		public List<Value> value { get; set; }
	}

	public class User
	{
		public string id { get; set; }
		public string displayName { get; set; }
	}

	public class CreatedBy
	{
		public object application { get; set; }
		public User user { get; set; }
	}

	public class User2
	{
		public string id { get; set; }
		public string displayName { get; set; }
	}

	public class LastModifiedBy
	{
		public object application { get; set; }
		public User2 user { get; set; }
	}

	public class ParentReference
	{
		public string driveId { get; set; }
		public string id { get; set; }
		public string path { get; set; }
	}

	public class sharepointFiles
	{

		public CreatedBy createdBy { get; set; }
		public string eTag { get; set; }
		public string id { get; set; }
		public LastModifiedBy lastModifiedBy { get; set; }
		public string name { get; set; }
		public ParentReference parentReference { get; set; }
		public int size { get; set; }
		public string dateTimeCreated { get; set; }
		public string dateTimeLastModified { get; set; }
		public string type { get; set; }
		public string webUrl { get; set; }
		public int childCount { get; set; }
	}
	public class calendarEvent
	{
		public string Id { get; set; }
		public string DateTimeCreated { get; set; }
		public string DateTimeLastModified { get; set; }
		public string Subject { get; set; }

	}
	public class AcrossConferenceData
	{
		public List<string> P2PActiveCalls { get; set; }
		public List<string> AllActiveUsers { get; set; }
		public List<string> MCUActiveUsers { get; set; }
		public List<string> ExternallyInvokedConferences { get; set; }
		public List<string> TelephonyActivities { get; set; }
		public List<string> MCUCallsStarted { get; set; }
		public List<string> PSActiveCalls { get; set; }
		public List<string> AllCallsStarted { get; set; }
		public List<string> AllActiveCalls { get; set; }
		public List<string> PSCallsStarted { get; set; }
		public List<string> MCUActiveCalls { get; set; }
		public List<string> P2PActiveUsers { get; set; }
		public List<string> P2PCallsStarted { get; set; }
		public List<string> PSActiveUsers { get; set; }
	}

	public class O365ADFS
	{
		public string MicrosoftAccount;
		public string NameSpaceType;
		public string AuthURL;
		public string Login;
		public string DomainName;
	}

	#endregion
}
