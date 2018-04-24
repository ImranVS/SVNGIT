using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSFramework;

namespace ServicesFlags
{

	public enum ServerTypes
	{
		Exchange,
		Active_Directory,
		SharePoint,
		Lync,
		Database_Availability_Group,
		Windows,
		Domino,
		URL,
		Sametime,
		Domino_Cluster,
		Notes_Database,
		Notes_Mail_Probe,
		Traverler,
		Key_Words,
		Network_Devices,
		Black_Berry_Servers,
		Mail,
		Network_Latency,
		Office_365
	}
	public class ServicesFlags
	{

		public bool UpdateServiceCollection(ServerTypes ServerType, string NodeName)
		{

			try
			{
				VSFramework.VSAdaptor adapter = new VSFramework.VSAdaptor();

				string sql = "Select * from NodeDetails where NodeId=(Select ID from Nodes where Name='" + NodeName + "') and Name='" + ServerType.ToString().Replace("_", " ") + " - UpdateCollection' and Value=1";
				DataSet ds = new DataSet();
				DataTable dt = new DataTable();
				adapter.FillDatasetAny("VitalSigns", "VitalSigns", sql, ref ds, "NodeDetails");
				dt = ds.Tables["NodeDetails"];

				if (dt.Rows.Count > 0)
				{
					string Names = String.Join(",", dt.AsEnumerable().Select(r => r.Field<string>("Name").ToString()));
					string IDs = String.Join(",", dt.AsEnumerable().Select(r => r.Field<Int32>("ID").ToString()));
					WriteHistoryEntry("ID " + IDs + " AND Names " + Names + " have been marked for resetCollection.  Resetting value to False now...");
					sql = "UPDATE NodeDetails set Value=0 where ID in (" + IDs + ")";
					adapter.ExecuteNonQueryAny("VitalSigns", "VitalSigns", sql);
					WriteHistoryEntry("ID " + IDs + " have been reset.  Leaving function");
					return true;

				}
				return false;

			}
			catch (Exception ex)
			{
				WriteHistoryEntry("Error in UpdateServiceCollection. Error: " + ex.Message);
			}
			return true;
		}

		

		private static void WriteHistoryEntry(string strMsg)
		{
			try
			{
				string path;
				bool appendMode = true;

				try
				{
					path = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Log_Files\\ServicesFlag.txt";
					
				}
				catch (Exception)
				{
					throw;
				}

				try
				{
					System.IO.StreamWriter sw = new System.IO.StreamWriter(path, appendMode, System.Text.Encoding.Unicode);
					sw.WriteLine(DateTime.Now.ToString() + "  " + strMsg);
					sw.Close();
					sw = null;
				}
				catch (Exception ex)
				{
				}
				GC.Collect();
			}
			catch
			{
			}
			//}
		}

		
	}
}
