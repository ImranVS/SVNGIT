using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;


namespace VSWebDAL.ConfiguratorDAL
{
	public class ELSDAL
	{
		private Adaptor objAdaptor = new Adaptor();
		private static ELSDAL _self = new ELSDAL();

		public static ELSDAL Ins
		{
			get { return _self; }
		}

		public DataTable GetAllData()
		{
			DataTable Credentialstab = new DataTable();
			try
			{

				// string SqlQuery = "Select * from Credentials";
				string SqlQuery = "SELECT AliasName,ID,EventName,EventId,EventKey,EventLevel,Source,TaskCategory,'false' as isSelected from dbo.ELSMaster";
				Credentialstab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return Credentialstab;


		}
		public DataTable GetServersFromProcedure()
		{
			try
			{
				return objAdaptor.GetDataFromProcedure("ServerLocationsMS");
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetEventsForKey(int EventId)
		{
			DataTable Credentialstab = new DataTable();
			try
			{

				// string SqlQuery = "Select * from Credentials";
				string SqlQuery = "SELECT distinct ELSId from dbo.ELSDetail where EventId=" + EventId.ToString();
				Credentialstab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return Credentialstab;


		}
		public string EventnamebyKey(int id)
		{
			string eventName = "";
			DataTable Alerttab = new DataTable();
			AlertNames ReturnAlert = new AlertNames();
			try
			{
				string SqlQuery = "Select name from ELS where ID=" + id.ToString();
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

		public Object DeleteELSDef(int id)
		{
			Object Update;
			try
			{

				string SqlQuery = "Delete from ELS Where ID=" + id.ToString();

				Update = objAdaptor.ExecuteNonQuery(SqlQuery);

				SqlQuery = "Delete from ELSDetail Where Event=" + id.ToString();

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

		public Object DeleteData(ELSMaster LOCbject)
		{
			Object Update;
			try
			{

				string SqlQuery = "Delete from ELSMaster Where ID=" + LOCbject.ID;

				Update = objAdaptor.ExecuteNonQuery(SqlQuery);

				SqlQuery = "Delete from ELSDetail Where ELSId=" + LOCbject.ID;

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
		public bool UpdateData(ELSMaster LOCbject)
		{
			string SqlQuery = "";
			bool Update;

			try
			{
				//7/10/2015 NS modified for VSPLUS-1985
				//if (LOCbject.Password == "      ")

				SqlQuery = "UPDATE ELSMaster SET AliasName='" + LOCbject.AliasName + "', EventId='" + LOCbject.EventId + "',EventKey='" + LOCbject.EventKey + "', Source='" + LOCbject.Source + "', TaskCategory='" + LOCbject.TaskCategory + "',EventName='" + LOCbject.EventName + "',EventLevel='" + LOCbject.EventLevel + "' WHERE ID = " + LOCbject.ID + "";
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
		public bool InsertData(ELSMaster LOCbject)
		{

			bool Insert = false;
			try
			{
				DataTable eventsdt = new DataTable();
				string sqlquery = "select * from ELSMaster where AliasName = '" + LOCbject.AliasName + "'";
				eventsdt = objAdaptor.FetchData(sqlquery);
				if (eventsdt.Rows.Count > 0)
				{

					Insert = false;
				}
				else
				{
					string SqlQuery = "INSERT INTO ELSMaster (AliasName,EventName,EventId,EventKey,Source,TaskCategory,EventLevel) VALUES('" + LOCbject.AliasName + "','" + LOCbject.EventName + "','" + LOCbject.EventId + "','" + LOCbject.EventKey + "','" + LOCbject.Source + "','" + LOCbject.TaskCategory + "','" + LOCbject.EventLevel + "')";
					Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
				}
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

		public DataTable GetDataForELSByname(ELSMaster LOCbject)
		{
			DataTable LocationsDataTable = new DataTable();
			try
			{
				string SqlQuery = "Select * from ELSMaster where ID ='" + LOCbject.ID + "'";
				LocationsDataTable = objAdaptor.FetchData(SqlQuery);
				//populate & return data object
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return LocationsDataTable;
		}

		public DataTable GetAllELSNames()
		{
			DataTable AlertNamestab = new DataTable();
			try
			{

				string SqlQuery = "Select * from ELS";
				AlertNamestab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return AlertNamestab;


		}

		public DataTable GetAllEventHistory()
		{
			DataTable EventHistorytab = new DataTable();
			try
			{

				string SqlQuery = "Select * from EventHistory order by eventtime desc";
				EventHistorytab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return EventHistorytab;


		}

		public DataTable GetEventHistorydetails(string name, string type)
		{
			DataTable EventHistorytab = new DataTable();
			DataTable serveriddt = new DataTable();
			try
			{
				string SqlQuery1 = "select distinct ID from servers where ServerName = '" + name + "' and ServerTypeID in (select ID from ServerTypes where serverType = '" + type + "' ) ";
				serveriddt = objAdaptor.FetchData(SqlQuery1);
				if (serveriddt.Rows.Count > 0)
				{
					int serverId = Convert.ToInt32(serveriddt.Rows[0]["ID"]);
					if (serverId != null)
					{
						string SqlQuery = "Select * from EventHistory where DeviceName In  (select distinct servername from servers srv inner join ELSDetail ed  on srv.ID= ed.ServerId inner join  ELSMaster em on ed.ELSId = em.ID where ed.serverId= " + serverId + ") and DeviceType = '" + type + "' order by eventtime desc";
						EventHistorytab = objAdaptor.FetchData(SqlQuery);
					}
				}
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return EventHistorytab;


		}

		public DataTable GetAllEventHistory(string startdate, string enddate)
		{
			DataTable EventHistorytab = new DataTable();
			try
			{

				string SqlQuery = "Select * from EventHistory WHERE MONTH(EventTime)=" + startdate + " AND YEAR(EventTime)=" + enddate + " ORDER BY eventtime DESC";
				EventHistorytab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return EventHistorytab;


		}


		public DataTable GetAllEventHistorydetails(string startdate, string enddate, string name)
		{
			DataTable EventHistorytab = new DataTable();
			try
			{

				string SqlQuery = "Select * from EventHistory WHERE MONTH(EventTime)=" + startdate + " AND YEAR(EventTime)=" + enddate + " and DeviceName = '" + name + "' ORDER BY eventtime DESC";
				EventHistorytab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return EventHistorytab;


		}
		public DataTable GetAllEventHistoryByLogName(string logName)
		{
			DataTable AlertNamestab = new DataTable();
			try
			{

				string SqlQuery = "Select * from EventHistory where LogName='" + logName + "' order by eventtime desc";
				AlertNamestab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return AlertNamestab;


		}

		public DataTable GetAllEventHistoryByLogNamedetails(string logName, string name, string type)
		{
			DataTable AlertNamestab = new DataTable();
			DataTable serveriddt = new DataTable();
			try
			{
				string SqlQuery1 = "select distinct ID from servers where ServerName = '" + name + "' and ServerTypeID in (select ID from ServerTypes where serverType = '" + type + "' ) ";
				serveriddt = objAdaptor.FetchData(SqlQuery1);
				if (serveriddt.Rows.Count > 0)
				{
					int serverId = Convert.ToInt32(serveriddt.Rows[0]["ID"]);
					if (serverId != null)
					{

						string SqlQuery = "Select * from EventHistory where LogName='" + logName + "' and DeviceName In  (select distinct servername from servers srv inner join ELSDetail ed  on srv.ID= ed.ServerId inner join  ELSMaster em on ed.ELSId = em.ID where ed.serverId= " + serverId + ") and DeviceType = '" + type + "' order by eventtime desc";
						AlertNamestab = objAdaptor.FetchData(SqlQuery);
					}
				}
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return AlertNamestab;


		}
		public bool insertEventServers(string eventDef, DataTable dtEvents, DataTable dtEventServers)
		{
			bool success = false;

			try
			{

				string sql = "delete from dbo.ELSDetail where EventId=(select id from els where name='" + eventDef + "')";
				success = objAdaptor.ExecuteNonQuery(sql);
				foreach (DataRow r in dtEvents.Rows)
				{
					if (r["isSelected"].ToString() == "true")
					{
						foreach (DataRow e in dtEventServers.Rows)
						{
							string sqlInsert = "insert into dbo.ELSDetail(EventId,ElsId,ServerId,LocationId) values((select id from els where name='" + eventDef + "')," + r[1].ToString() + "," + e[1].ToString() + "," + e[2].ToString() + ")";
							success = objAdaptor.ExecuteNonQuery(sqlInsert);
						}
					}
				}
				success = true;
			}
			catch (Exception ex)
			{
				success = false;
				throw ex;
			}
			return success;

		}
		public bool insertEventDef(string eventDef)
		{
			bool success = false;

			try
			{
				DataTable eventsdt = new DataTable();
				string sqlquery = "select * from ELS where Name = '" + eventDef + "'";
				eventsdt = objAdaptor.FetchData(sqlquery);
				if (eventsdt.Rows.Count > 0)
				{

					success = false;
				}
				else
				{
					string sql = "INSERT INTO ELS(NAME) VALUES('" + eventDef + "')";

					success = objAdaptor.ExecuteNonQuery(sql);
				}
			}
			catch (Exception ex)
			{
				success = false;
				throw ex;
			}
			return success;

		}
		public DataTable GetSelectedServers(int eventKey)
		{

			DataTable EventsTab = new DataTable();
			try
			{
				string SqlQuery = "select * from ELSDetail where EventId=" + eventKey;
				EventsTab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}
			return EventsTab;
		}

		public bool updateEventDef(int id, string name, string sessionname)
		{
			bool success = false;
			string sql;
			DataTable eventsdt = new DataTable();
			try
			{
				if (name == sessionname)
				{
					sql = "update ELS set name='" + name + "' where id=" + id;

					success = objAdaptor.ExecuteNonQuery(sql);
				}

				else
				{
					sql = "select Name from ELS where Name = '" + name + "' ";
					eventsdt = objAdaptor.FetchData(sql);
					if (eventsdt.Rows.Count > 0)
					{
						success = false;
					}
					else
					{
						sql = "update ELS set name='" + name + "' where id=" + id;
						success = objAdaptor.ExecuteNonQuery(sql);
					}
				}
				//if (eventsdt.Rows.Count > 0)
				//{

				//    success = false;
				//}
				//else
				//{
				//    sql = "update ELS set name='" + name + "' where id=" + id;

				//    success = objAdaptor.ExecuteNonQuery(sql);
				//}
			}
			catch (Exception ex)
			{
				success = false;
				throw ex;
			}
			return success;

		}
		public DataTable GetDataForCredentialsById(ELSMaster LOCbject)
		{
			DataTable LocationsDataTable = new DataTable();
			try
			{
				//string SqlQuery = "Select * from Credentials where AliasName='" + LOCbject.AliasName + "'";
				string SqlQuery = "Select * from ELSMaster where ID=" + LOCbject.ID;
				LocationsDataTable = objAdaptor.FetchData(SqlQuery);
				//populate & return data object
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return LocationsDataTable;
		}

	}
}
