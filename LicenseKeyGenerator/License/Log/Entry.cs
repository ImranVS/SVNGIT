using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace Log
{
	public class Entry
	{
		private static Entry _self = new Entry();
		public static Entry Ins
		{
			get
			{
				return _self;
			}

		}
		public void Write(string strpath, string strFileName, string strMsg)
		{
			try
			{
				bool appendMode = true;

				string strLogDestination = strpath + strFileName;
				StreamWriter sw = new StreamWriter(strLogDestination, appendMode, System.Text.Encoding.Unicode);
				sw.WriteLine(strMsg);
				sw.WriteLine();
				sw.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
			GC.Collect();
		}
		//5/15/2014 NS added for VSPLUS-634
		public void WriteHistoryEntry(string strMsg)
		{
			bool appendMode = true;
			//string ServiceLogDestination = VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Log Files Path");
			string ServiceLogDestination = System.Web.HttpContext.Current.Server.MapPath("~") + "\\LogFiles\\VSWebLogs.txt";
			//ServiceLogDestination += "VSWebLogs.txt";
			
			try
			{
				StreamWriter sw = new StreamWriter(ServiceLogDestination, appendMode, System.Text.Encoding.Unicode);
				sw.WriteLine(strMsg);
				sw.Close();
				sw = null;
			}
			catch
			{
				
			}
			finally
			{
				GC.Collect();
			}
		}
	}
}
