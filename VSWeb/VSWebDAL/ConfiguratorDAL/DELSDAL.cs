using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;


namespace VSWebDAL.ConfiguratorDAL
{
	public class DELSDAL
	{
		private Adaptor objAdaptor = new Adaptor();
		private static DELSDAL _self = new DELSDAL();

		public static DELSDAL Ins
		{
			get { return _self; }
		}

		public DataTable GetDELSData(DominoEventLog nameObj)
		{
			DataTable Credentialstab = new DataTable();
			try
			{

				// string SqlQuery = "Select * from Credentials";
				string SqlQuery = "Select ID,DominoEventLogId, Keyword,RepeatOnce,NotRequiredKeyword,[Log],AgentLog from [LogFile] where DominoEventLogId=(select ID from DominoEventLog where Name='"+nameObj.Name+"')";
				Credentialstab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return Credentialstab;


		}

		public bool insertEventDef(string eventDef)
		{
			bool success = false;

			try
			{
				string sql = "INSERT INTO DominoEventLog(NAME) VALUES('" + eventDef + "')";

				success = objAdaptor.ExecuteNonQuery(sql);
			}
			catch (Exception ex)
			{
				success = false;
				throw ex;
			}
			return success;

		}
		public bool updateEventDef(int id, string name)
		{
			bool success = false;

			try
			{
				string sql = "update DominoEventLog set name='" + name + "' where id=" + id;

				success = objAdaptor.ExecuteNonQuery(sql);
			}
			catch (Exception ex)
			{
				success = false;
				throw ex;
			}
			return success;

		}

		public DataTable GetAllDELSNames()
		{
			DataTable AlertNamestab = new DataTable();
			try
			{

				string SqlQuery = "Select * from DominoEventLog";
				AlertNamestab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return AlertNamestab;


		}
		public Object DeleteDELSDef(int id)
		{
			Object Update;
			try
			{

				string SqlQuery = "Delete from DominoEventLog Where ID=" + id.ToString();

				Update = objAdaptor.ExecuteNonQuery(SqlQuery);

				SqlQuery = "Delete from Logfile Where DominoEventLogId=" + id.ToString();

				Update = objAdaptor.ExecuteNonQuery(SqlQuery);

			}
			catch
			{
				Update = false;
			}
			finally
			{
			}
			return Update;
		}
		//public DataTable GetDataForELSByname(LogFile LOCbject)
		//{
		//    DataTable LocationsDataTable = new DataTable();
		//    try
		//    {
		//        string SqlQuery = "Select * from LogFile where AliasName='" + LOCbject.AliasName + "'";
		//        LocationsDataTable = objAdaptor.FetchData(SqlQuery);
		//        //populate & return data object
		//    }
		//    catch (Exception ex)
		//    {
		//        throw ex;
		//    }
		//    finally
		//    {
		//    }
		//    return LocationsDataTable;
		//}
		public bool InsertData(LogFile LOgbject)
		{

			bool Insert = false;
			try
			{
				string SqlQuery = "INSERT INTO [LogFile] (Keyword,RepeatOnce,Log,AgentLog,NotRequiredKeyword,DominoEventLogId) VALUES('" + LOgbject.Keyword + "','" + LOgbject.RepeatOnce + "','" + LOgbject.Log + "','" + LOgbject.AgentLog + "','" + LOgbject.NotRequiredKeyword + "'," + LOgbject.DominoEventLogId + ")";
				Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Insert = false;
			}
			finally
			{
			}
			return Insert;
		}
		//public DataTable GetDataForELSByname(LogFile LOCbject)
		//{
		//    DataTable LocationsDataTable = new DataTable();
		//    try
		//    {
		//        string SqlQuery = "Select * from LogFile where AliasName='" + LOCbject.AliasName + "'";
		//        LocationsDataTable = objAdaptor.FetchData(SqlQuery);
		//        //populate & return data object
		//    }
		//    catch (Exception ex)
		//    {
		//        throw ex;
		//    }
		//    finally
		//    {
		//    }
		//    return LocationsDataTable;
		//}
		public bool UpdateData(LogFile LOCbject)
		{
			string SqlQuery = "";
			bool Update;

			try
			{
				//7/10/2015 NS modified for VSPLUS-1985
				//if (LOCbject.Password == "      ")

				SqlQuery = "UPDATE LogFile SET Keyword='" + LOCbject.Keyword + "',NotRequiredKeyword='" + LOCbject.NotRequiredKeyword + "',RepeatOnce='" + LOCbject.RepeatOnce + "',Log='" + LOCbject.Log + "',AgentLog='" + LOCbject.AgentLog + "' WHERE ID = " + LOCbject.ID + "";
				Update = objAdaptor.ExecuteNonQuery(SqlQuery);

			}
			catch
			{
				Update = false;
			}
			finally
			{
			}
			return Update;
		}

		public string EventnamebyKey(int id)
		{
			string eventName = "";
			DataTable Alerttab = new DataTable();
			//AlertNames ReturnAlert = new AlertNames();
			try
			{
				string SqlQuery = "Select name from DominoEventLog where ID=" + id.ToString();
				Alerttab = objAdaptor.FetchData(SqlQuery);

				if (!DBNull.Value.Equals(Alerttab.Rows[0]["name"]))
				{
					eventName = Alerttab.Rows[0]["name"].ToString();
				}

			}
			catch (Exception ex)
			{

				throw ex;
			}
			finally
			{
			}

			return eventName;

		}

		public int Eventidbyname(string  name)
		{
			int DominoEventLogId;
			DataTable Alerttab = new DataTable();
			//AlertNames ReturnAlert = new AlertNames();
			try
			{
				string SqlQuery = "Select ID from DominoEventLog where Name='" + name + "'";
				Alerttab = objAdaptor.FetchData(SqlQuery);

				//if (!DBNull.Value.Equals(Alerttab.Rows[0]["ID"]))
				//{
					 DominoEventLogId = Convert.ToInt32(Alerttab.Rows[0]["ID"]);
				//}

			}
			catch (Exception ex)
			{

				throw ex;
			}
			finally
			{
			}

			return  DominoEventLogId;

		}

		public DataTable GetLogFilesData(int alertKey)
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = "Select * from LogFile where DominoEventLogId = " + alertKey + "";
				dt = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception)
			{

				throw;
			}
			return dt;
		}

		public bool UpdateLogfileDetails(LogFile logObj)
		{
			bool i = false;
			//4/4/2014 NS added for VSPLUS-519
			
			try
			{
				string sqlQuerry = "UPDATE LogFile SET Keyword='" + logObj.Keyword + "',NotRequiredKeyword='" + logObj.NotRequiredKeyword + "',RepeatOnce='" + logObj.RepeatOnce + "',Log='" + logObj.Log + "',AgentLog='" + logObj.AgentLog + "'"
				+ " WHERE ID = " + logObj.ID + " and DominoEventLogId = " + logObj.DominoEventLogId + " ";
				i = objAdaptor.ExecuteNonQuery(sqlQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return i;
		}

		public Object DeleteData(LogFile LOgobject)
		{
			Object Update;
			try
			{

				string SqlQuery = "Delete LogFile Where ID=" + LOgobject.ID;

				Update = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Update = false;
			}
			finally
			{
			}
			return Update;
		}

		public DataTable GetServersFromProcedure()
		{
			try
			{
				return objAdaptor.GetDataFromProcedure("ServerLocationsDS");
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetSelectedServers(int EventKey)
		{

			DataTable EventsTab = new DataTable();
			try
			{
				string SqlQuery = "select * from [DominoEventLogServers] where DominoEventLogId=" + EventKey;
				EventsTab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}
			return EventsTab;
		}

		public bool InsertSelectedServers(int EventKey, DataTable dtSel)
		{
			bool Insert = false;
			try
			{
				// string SqlQuery = "select (SELECT count(*) FROM servers) + (Select count(*) from URLs)";
				//DataTable ServerTab = objAdaptor.FetchData(SqlQuery);

				Insert = objAdaptor.ExecuteNonQuery("delete from [DominoEventLogServers] where DominoEventLogId=" + EventKey);

				if (dtSel.Rows.Count > 0)
				{

					//if (Convert.ToInt32(ServerTab.Rows[0][0]) == dtSel.Rows.Count)
					//{
					//    SqlQuery = "Insert into AlertServers(AlertKey,ServerID,LocationID) values(" + dtSel.Rows[0]["AlertKey"] + ",0,0)";
					//    Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
					//}
					//else
					//{
					for (int i = 0; i < dtSel.Rows.Count; i++)
					{
						//11/26/2013 NS modified the query - added a new column for ServerTypeID, can't rely on LocationID 
						//and ServerID combination in case of URLs
						//string SqlQuery = "Insert into AlertServers(AlertKey,ServerID,LocationID) values(" +
						//   dtSel.Rows[i]["AlertKey"] + "," + dtSel.Rows[i]["ServerID"] + "," + dtSel.Rows[i]["LocationID"] + ")";
						string SqlQuery = "Insert into DominoEventLogServers(DominoEventLogId,ServerID,LocationID,ServerTypeID) values(" +
							 dtSel.Rows[i]["DominoEventLogId"] + "," + dtSel.Rows[i]["ServerID"] + "," + dtSel.Rows[i]["LocationID"] + "," +
							 dtSel.Rows[i]["ServerTypeID"] + ")";
						Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
					}
					//}
				}
			}
			catch (Exception ex)
			{

				throw ex;
			}
			return Insert;
		}

		public DataTable GetDataByAlertName(DominoEventLog NameObj)
		{

			DataTable AlertDataTable = new DataTable();
			AlertNames Returnobject = new AlertNames();

			try
			{
				if (NameObj.ID == 0)
				{
				string SqlQuery = "SELECT * FROM [DominoEventLog] where Name='" + NameObj.Name + "'";
					//string SqlQuery = "SELECT * FROM Servers";
					AlertDataTable = objAdaptor.FetchData(SqlQuery);
				}
				else
				{
					//7/3/2013 NS modified
					//string SqlQuery = "SELECT * FROM [Users] where LoginName='" + AlertsObject.AlertName + "' and AlertKey<>'" + AlertsObject.AlertKey + "'";
					string SqlQuery = "SELECT * FROM [DominoEventLog] where Name='" + NameObj.Name + "' and ID<>" + NameObj.ID;
					AlertDataTable = objAdaptor.FetchData(SqlQuery);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return AlertDataTable;


		}


	}
}
