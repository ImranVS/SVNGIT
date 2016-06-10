using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Remoting;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Collections;
using System.Data;

namespace VitalSignsMSCollector
{
	class Common
	{
		public string submitRequest(string URL, string requestMethod, string message, string userId, string pwd)
		{
			string responseFromServer = "";
			try
			{


				//SharePointOnlineCredentials creds = new SharePointOnlineCredentials(userId, Common.String2SecureString(pwd));
				System.Net.WebRequest request = System.Net.WebRequest.Create(URL);
				System.Net.CredentialCache c = new System.Net.CredentialCache();
				request.Credentials = BuildCredentials(URL, userId, pwd, "BASIC");
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
	}
}
