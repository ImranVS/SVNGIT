using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;


namespace VSWebDAL.ConfiguratorDAL
{
    public class AlertsDAL
    {
        private Adaptor objAdaptor = new Adaptor();
        private static AlertsDAL _self = new AlertsDAL();

        public static AlertsDAL Ins
        {
            get { return _self; }
        }

        public DataTable GetAllAlertsNames()
        {
            DataTable AlertNamestab = new DataTable();
            try
            {

                string SqlQuery = "Select * from AlertNames";
                AlertNamestab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return AlertNamestab;


        }

        public DataTable GetType()
        {
            DataTable AlertNamestab = new DataTable();
            try
            {
                //4/23/2015 NS modified
                string SqlQuery = "Select * from HoursIndicator WHERE UseType!=1";
                AlertNamestab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return AlertNamestab;


        }

		public AlertNames AlertnamebyKey(AlertNames Alert)
		{
			DataTable Alerttab = new DataTable();
			AlertNames ReturnAlert = new AlertNames();
			try
			{
				string SqlQuery = "Select AlertName,Templateid from AlertNames where AlertKey=" + Alert.AlertKey;
				Alerttab = objAdaptor.FetchData(SqlQuery);
				ReturnAlert.AlertName = Alerttab.Rows[0]["AlertName"].ToString();
				//ReturnAlert.Templateid = Convert.ToInt32(Alerttab.Rows[0]["Templateid"]);
				if (!DBNull.Value.Equals(Alerttab.Rows[0]["Templateid"]))
				{
					ReturnAlert.Templateid = Convert.ToInt32(Alerttab.Rows[0]["Templateid"]);
				}
				else
				{
					ReturnAlert.Templateid = 0;
				}
				//int? stockvalue = (int?)(!Convert.IsDBNull(result) ? result : null);

			}
			catch (Exception ex)
			{

				throw ex;
			}
			finally
			{
			}

			return ReturnAlert;

		}

        public Object DeleteAlert(AlertNames Alert)
        {
            Object Delete;
            try
            {
                string S = "Delete from AlertDetails where AlertKey=" + Alert.AlertKey;
                Delete = objAdaptor.ExecuteNonQuery(S);
                string s1 = "Delete from AlertDeviceTypes where AlertKey=" + Alert.AlertKey;
                Delete = objAdaptor.ExecuteNonQuery(s1);
                string s2 = "Delete from AlertEvents where AlertKey=" + Alert.AlertKey;
                Delete = objAdaptor.ExecuteNonQuery(s2);
                string s3 = "Delete from AlertLocations where AlertKey=" + Alert.AlertKey;
                Delete = objAdaptor.ExecuteNonQuery(s3);
                string s4 = "Delete from AlertServers where AlertKey=" + Alert.AlertKey;
                Delete = objAdaptor.ExecuteNonQuery(s4);
                //5/13/2015 NS added for VSPLUS-1763 - need to delete relevant alert records from the EscalationDetails table due to a FK constraint
                string s5 = "Delete from EscalationDetails where AlertKey=" + Alert.AlertKey;
                Delete = objAdaptor.ExecuteNonQuery(s5);
                string SqlQuery = "Delete from AlertNames where AlertKey=" + Alert.AlertKey;
                Delete = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Delete = false;
				
            }
            finally
            {
            }

            return Delete;

        }
        public DataTable GetHoursData(AlertNames Alertobj)
        {
            DataTable Alerttab = new DataTable();
            try
            {
                //4/4/2014 NS modified for VSPLUS-519
                //string SqlQuery = "Select ID,AlertKey,(select Type from HoursIndicator where ID=HoursIndicator) as HoursIndicator ,SendTo,CopyTo,BlindCopyTo, StartTime,Duration, Day, SendSNMPTrap from AlertDetails where AlertKey=(select AlertKey from AlertNames where AlertName='" + Alertobj.AlertName + "')"; //Brijesh
                //12/1/2014 NS modified for VSPLUS-946
                //12/4/2014 NS modified for VSPLUS-1229
                //6/16/2015 NS modified for VSPLUS-1868
                string SqlQuery = "Select t1.ID,AlertKey,(select Type from HoursIndicator where ID=HoursIndicator) as HoursIndicator, " +
                    "SendTo,CopyTo,BlindCopyTo, StartTime,Duration, Day, SendSNMPTrap, EnablePersistentAlert, ISNULL(SMSTo,'') SMSTo, ISNULL(ScriptName,'') ScriptName, " +
                    "ISNULL(t2.ID,'') ScriptID " +
                    "from AlertDetails t1 left outer join CustomScripts t2 on t1.ScriptID=t2.ID where AlertKey=(select AlertKey from AlertNames where AlertName='" + Alertobj.AlertName + "')"; //Brijesh
                Alerttab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Alerttab;


        }

        public Object DeleteAlertDetails(AlertDetails Alert)
        {
            Object Delete;
			string SqlQuery;
            try
            {
				//DataTable dtt = new DataTable();
				//SqlQuery = "Select * from AlertDetails where AlertKey=" + Alert.AlertKey; 
				//dtt = objAdaptor.FetchData(SqlQuery);
				//if (dtt.Rows.Count > 1)
				//{
					SqlQuery = "Delete from AlertDetails where ID=" + Alert.ID;
					Delete = objAdaptor.ExecuteNonQuery(SqlQuery);
				//}
				//else
				//{
				//    Delete = false;
				//}
            }
            catch
            {

                Delete = false;
            }
            finally
            {
            }

            return Delete;

        }

        public DataTable GetAllEvents(string ServertypeIDs)
        {
            string SqlQuery = "";
            DataTable EventsTable = new DataTable();
            try
            {
                //1/9/2013 NS modified the query
                //string SqlQuery = "Select * from EventsMaster where ServerTypeID in (select ID from ServerTypes where ServerType in (" + ServertypeIDs + "))";
                //8/6/2013 NS modified
                if (ServertypeIDs == "")
                {
                    //10/17/2014 NS modified for VSPLUS-730
                    SqlQuery = "Select e.ID ID, ServerType + ' - ' + e.EventName EventName, e.EventName Name, " +
                        "ISNULL(AlertOnRepeat,0) ConsecutiveFailures, ServerType, " +
                        "e.ServerTypeID ServerTypeID from EventsMaster e " +
                        "INNER JOIN ServerTypes s ON ServerTypeID=s.ID ";
                }
                else
                {
                    SqlQuery = "Select e.ID ID, ServerType + ' - ' + e.EventName EventName, e.ServerTypeID ServerTypeID from EventsMaster e " +
                    "INNER JOIN ServerTypes s ON ServerTypeID=s.ID where ServerTypeID in " +
                    "(select ID from ServerTypes where ServerType in (" + ServertypeIDs + "))";
                }
                EventsTable = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return EventsTable;

        }
                
        public DataTable GetrestrictedEvents(int AlertKey)
        {

            DataTable EventsTab = new DataTable();
            try
            {
                //1/9/2013 NS modified
                //string SqlQuery = "select EventName from EventsMaster where ID in (select EventID from AlertEvents where AlertKey=" + AlertKey + ")";
                string SqlQuery = "select ServerType + ' - ' + EventName EventName from EventsMaster e INNER JOIN " +
                    "ServerTypes s ON e.ServerTypeID=s.ID where e.ID in (select EventID from AlertEvents where AlertKey=" + AlertKey + ")";
                EventsTab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return EventsTab;
        }

        public DataTable GetSelectedEvents(int AlertKey)
        {

            DataTable EventsTab = new DataTable();
            try
            {
                string SqlQuery = "select * from AlertEvents where AlertKey=" + AlertKey;
                EventsTab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return EventsTab;
        }

        public DataTable GetSelectedServers(int AlertKey)
        {

            DataTable EventsTab = new DataTable();
            try
            {
                string SqlQuery = "select * from AlertServers where AlertKey=" + AlertKey ;
                EventsTab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return EventsTab;
        }
        public DataTable GetServers(int AlertKey)
        {

            DataTable ServerTab = new DataTable();
            try
            {
                string SqlQuery = "select ServerName from Servers where ID in (Select ServerID from AlertServers where AlertKey=" + AlertKey + ")";
                ServerTab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ServerTab;
        }      

        public bool InsertName(AlertNames Obj)
        {
            bool Insert = false;            
            try
            {
                string SqlQuery = "Insert into AlertNames(AlertName,Templateid) values('" + Obj.AlertName + "',"+Obj.Templateid+")";
                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }            
            return Insert;

        }

		public bool UpdateName(AlertNames obj)
		{
			bool Update = false;
			try
			{
				string SqlQuery = "Update AlertNames set AlertName='" + obj.AlertName + "',Templateid=" + obj.Templateid + " where AlertKey=" + obj.AlertKey + "";
				Update = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch (Exception)
			{

				Update = false;
			}
			return Update;
		}

        public bool InsertRestrictedServers(string AlertName, string Server)
        {

            bool Insert = false;
            string[] RServers = Server.Split(',');

            try
            {
                if (Server != "")
                {
                    string SqlQuery = "Delete from AlertServers where AlertKey=(select AlertKey from AlertNames where AlertName='" + AlertName + "')";
                    Insert = objAdaptor.ExecuteNonQuery(SqlQuery);

                    for (int i = 0; i < RServers.Length; i++)
                    {

                        SqlQuery = "Insert into AlertServers(AlertKey,ServerID) values((select AlertKey from AlertNames where AlertName='" + AlertName + "')" +
                            ",(select ID from Servers where ServerName=" + RServers[i] + "))";
                        Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
                    }

                }
                else
                {
                    Insert = true;
                }
            }
            catch (Exception)
            {

                Insert = false;
            }

            return Insert;
        }

        public bool InsertRestrictedEvents(string AlertName, string Events)
        {
            //1/9/2013 NS modified
            int index = -1;
            string eventR = "";
            string eventL = "";
            bool Insert = false;

            string[] REvents = Events.Split(',');

            try
            {
                if (Events != "")
                {
                    string SqlQuery = "Delete from AlertEvents where AlertKey=(select AlertKey from AlertNames where AlertName='" + AlertName + "')";
                    Insert = objAdaptor.ExecuteNonQuery(SqlQuery);

                    for (int i = 0; i < REvents.Length; i++)
                    {
                        //1/9/2013 NS modified
                        index = REvents[i].LastIndexOf("-");
                        eventR = REvents[i].Substring(index + 2, REvents[i].Length-index-3);
                        eventL = REvents[i].Substring(1, index - 2);
                        //SqlQuery = "Insert into AlertEvents(AlertKey,EventID) values((select AlertKey from AlertNames where AlertName='" + AlertName + "')" +
                        //    ",(select ID from EventsMaster where EventName=" + REvents[i] + "))";
                        SqlQuery = "Insert into AlertEvents(AlertKey,EventID) values((select AlertKey from AlertNames where AlertName='" + AlertName + "')" +
                                ",(select e.ID from EventsMaster e INNER JOIN ServerTypes s ON e.ServerTypeID = s.ID " + 
                                "where EventName='" + eventR + "' AND s.ServerType='" + eventL + "'))";
                        Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
                    }

                }
                else
                {
                    Insert = true;
                }
            }
            catch (Exception)
            {

                Insert = false;
            }

            return Insert;
        }

        public bool InsertSelectedEvents(int AlertKey, DataTable dtSel)
        {
            bool Insert = false;
            try
            {
                //string SqlQuery = "Select count(*) as totcount from EventsMaster";
                //DataTable ServerTab = objAdaptor.FetchData(SqlQuery);


                Insert = objAdaptor.ExecuteNonQuery("delete from AlertEvents where AlertKey=" + AlertKey);
                if (dtSel.Rows.Count> 0)
                {
                    
                    //if (Convert.ToInt32(ServerTab.Rows[0]["totcount"]) == dtSel.Rows.Count)
                    //{
                    //    SqlQuery = "Insert into AlertEvents(AlertKey,EventID,ServerTypeID) values(" + dtSel.Rows[0]["AlertKey"] + ",0,0)";
                    //    Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
                    //}
                    //else
                    //{
                        for (int i = 0; i < dtSel.Rows.Count; i++)
                        {
                           string SqlQuery = "Insert into AlertEvents(AlertKey,EventID,ServerTypeID) values(" +
                               dtSel.Rows[i]["AlertKey"] + "," + dtSel.Rows[i]["EventID"] + "," + dtSel.Rows[i]["ServerTypeID"] + ")";
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

        public bool InsertSelectedServers(int AlertKey, DataTable dtSel)
        {
            bool Insert = false;
            try
            {
               // string SqlQuery = "select (SELECT count(*) FROM servers) + (Select count(*) from URLs)";
               //DataTable ServerTab = objAdaptor.FetchData(SqlQuery);

                    Insert = objAdaptor.ExecuteNonQuery("delete from AlertServers where AlertKey=" + AlertKey);

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
                                string SqlQuery = "Insert into AlertServers(AlertKey,ServerID,LocationID,ServerTypeID) values(" +
                                     dtSel.Rows[i]["AlertKey"] + "," + dtSel.Rows[i]["ServerID"] + "," + dtSel.Rows[i]["LocationID"] + "," +
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


        public DataTable GetDataByAlertName(AlertNames AlertsObject)
        {

            DataTable AlertDataTable = new DataTable();
            AlertNames Returnobject = new AlertNames();
            
            try
            {
                if (AlertsObject.AlertKey == 0)
                {
                    string SqlQuery = "SELECT * FROM [AlertNames] where AlertName='" + AlertsObject.AlertName + "'";
                    //string SqlQuery = "SELECT * FROM Servers";
                    AlertDataTable = objAdaptor.FetchData(SqlQuery);
                }
                else
                {
                    //7/3/2013 NS modified
                    //string SqlQuery = "SELECT * FROM [Users] where LoginName='" + AlertsObject.AlertName + "' and AlertKey<>'" + AlertsObject.AlertKey + "'";
                    string SqlQuery = "SELECT * FROM [AlertNames] where AlertName='" + AlertsObject.AlertName + "' and AlertKey<>" + AlertsObject.AlertKey;
                    AlertDataTable = objAdaptor.FetchData(SqlQuery);
                }
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return AlertDataTable;


        }

        public DataTable GetServer()
        {
            DataTable ServerTable = new DataTable();
            try
            {
                string sqlQuery = "Select ServerName,ID,LocationID from Servers ";
                ServerTable = objAdaptor.FetchData(sqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ServerTable;

        }

        //public DataTable GetAlertTab(string ServerName)
        //{
        //    DataTable AlertDetails = new DataTable();
        //    try
        //    {
                //Mukund modified on 27Sep13
                 //string sqlQuery = "SELECT t2.ID,t1.AlertKey,t1.AlertName Name,HoursIndicator, SendTo,CopyTo,BlindCopyTo, StartTime,Duration,Day " +
                 //               ", ISNULL(SUBSTRING((SELECT ',' +   " +
                 //               "ServerType from ServerTypes where id  " +
                 //               "not in (select ServerTypeID from AlertDeviceTypes where  AlertKey =t1.AlertKey) " +
                 //               "FOR XML PATH('')), 2, 1000000),'All') as ServerType " +
                 //               ", ISNULL(SUBSTRING((SELECT ',' +   " +
                 //               "EventName from EventsMaster where id  " +
                 //               "not in (select EventID from AlertEvents where  AlertKey =t1.AlertKey) " +
                 //               "FOR XML PATH('')), 2, 1000000),'All') as EventName " +
                 //               " FROM AlertNames t1 INNER JOIN AlertDetails t2 ON t1.AlertKey=t2.AlertKey " +
                 //               "WHERE t1.AlertKey  IN  " +
                 //               "(SELECT distinct t1.AlertKey " +
                 //               " FROM AlertNames t1 INNER JOIN AlertDetails t2 ON t1.AlertKey=t2.AlertKey " +
                 //               "WHERE t1.AlertKey not IN  " +
                 //               "( SELECT t2.AlertKey FROM Servers t1 INNER JOIN AlertServers t2 ON t1.ID=t2.ServerID WHERE  " +
                 //               "ServerName='" + ServerName + "')) " +
                 //               "or  t1.AlertKey  IN  " +
                 //               "(select alertkey from " +
                 //               "(SELECT     AlertKey, (select COUNT(*) from Servers)-COUNT(*) AS totalservers " +
                 //               "FROM AlertServers " +
                 //               "GROUP BY AlertKey) as alertserver where totalservers=0) " +
                 //               "ORDER BY AlertKey ";

//Mukund modified on 02Oct13
                //string sqlQuery = "SELECT t2.ID,t1.AlertKey,t1.AlertName Name,HoursIndicator, SendTo,CopyTo,BlindCopyTo, StartTime, " +
                //"Duration,Day, " +
                //"SUBSTRING((SELECT distinct ',' + " +
                //"ServerType from ServerTypes where ServerType " +
                //" in (select distinct ServerType from ServerTypes st, AlertEvents ae where st.ID=ae.servertypeid " +
                //"and  ae.AlertKey=t1.AlertKey " +
                //"union " +
                //"select distinct ServerType from ServerTypes  " +
                //"where 1 in (select  case ServerTypeId when 0 then 1 else 0 end from AlertEvents where AlertKey=t1.AlertKey) " +
                //") " +
                //"FOR XML PATH('')), 2, 1000000) as ServerType " +
                //",SUBSTRING((SELECT distinct ',' +   " +
                //"EventName from EventsMaster where EventName  " +
                //" in (select distinct st.EventName from EventsMaster st, AlertEvents ae where st.ID=ae.eventid " +
                //"and  ae.AlertKey=t1.AlertKey " +
                //"union " +
                //"select distinct EventName from EventsMaster  " +
                //"where 1 in (select  case eventid when 0 then 1 else 0 end from AlertEvents where AlertKey=t1.AlertKey)) " +
                //"FOR XML PATH('')), 2, 1000000) as EventName " +
                //"FROM AlertNames t1 INNER JOIN AlertDetails t2 ON t1.AlertKey=t2.AlertKey " +
                //"where t1.AlertKey in " +
                //"(SELECT t2.AlertKey FROM Servers t1 INNER JOIN AlertServers t2 ON t1.ID=t2.ServerID WHERE  " +
                //"ServerName='" + ServerName + "'" +
                //" union " +
                //" SELECT t2.AlertKey FROM URLs t1 INNER JOIN AlertServers t2 ON t1.ID=t2.ServerID WHERE " +
                //" Name='" + ServerName + "'" +
                //" ) " +
                //"or " +
                //"t1.AlertKey  IN  " +
                //"(SELECT distinct AlertKey FROM Servers t1,AlertServers t2 where t2.serverid=0 and (ServerName='" + ServerName + "'" +
                //"and (t1.LocationID = t2.LocationID or t2.LocationID = 0))" +
                //"union " + 
                //"SELECT distinct AlertKey FROM URLs t1,AlertServers t2 where t2.serverid=0 and (Name='" + ServerName + "'" +
                //"and (t1.LocationID = t2.LocationID or t2.LocationID = 0)))";               
        public DataTable GetAlertTab(string ServerName, string servertypeid)
        {
            DataTable AlertDetails = new DataTable();
            try
            {
                string sqlQuery = "SELECT t2.ID,t1.AlertKey,t1.AlertName Name,HoursIndicator, SendTo,CopyTo,BlindCopyTo, StartTime, " +
                         "Duration,Day, " +
                         "SUBSTRING((SELECT distinct ',' + " +
                         "ServerType from ServerTypes where ServerType " +
                         " in (select distinct ServerType from ServerTypes st, AlertEvents ae where st.ID=ae.servertypeid " +
                         "and  ae.AlertKey=t1.AlertKey " +
                         "union " +
                         "select distinct ServerType from ServerTypes  " +
                         "where 1 in (select  case ServerTypeId when 0 then 1 else 0 end from AlertEvents where AlertKey=t1.AlertKey) " +
                         ") " +
                         "FOR XML PATH('')), 2, 1000000) as ServerType " +
                         ",SUBSTRING((SELECT distinct ',' +   " +
                         "EventName from EventsMaster where EventName  " +
                         " in (select distinct st.EventName from EventsMaster st, AlertEvents ae where st.ID=ae.eventid " +
                         "and  ae.AlertKey=t1.AlertKey and ae.EventID<>0 and ae.ServerTypeID=st.ServerTypeID  and st.servertypeid=(select id from ServerTypes where servertype='" + servertypeid + "')" +
                         "union " +
                         "select distinct EventName from EventsMaster st, AlertEvents ae where  ae.AlertKey=t1.AlertKey and ae.EventID=0 and ae.ServerTypeID=st.ServerTypeID and st.servertypeid=(select id from ServerTypes where servertype='" + servertypeid + "')" +
                         "union " +
                         "select distinct EventName from EventsMaster st where 1 in (select isnull((select 1  from AlertEvents ae where AlertKey=t1.AlertKey  and ae.EventID=0 and ae.ServerTypeID=0),0)) and ServerTypeID =(select id from ServerTypes where servertype='" + servertypeid + "')) " +
                         "FOR XML PATH('')), 2, 1000000) as EventName " +
                         "FROM AlertNames t1 INNER JOIN AlertDetails t2 ON t1.AlertKey=t2.AlertKey " +
                         "where t1.AlertKey in " +
                         "(SELECT t2.AlertKey FROM Servers t1 INNER JOIN AlertServers t2 ON t1.ID=t2.ServerID WHERE  " +
                         " ServerName in ('" + ServerName + "')" +
                         " union " +
                         " SELECT t2.AlertKey FROM URLs t1 INNER JOIN AlertServers t2 ON t1.ID=t2.ServerID WHERE " +
                         " Name in ('" + ServerName + "')" +
                         " ) " +
                         "or " +
                         "t1.AlertKey  IN  " +
                         "(SELECT distinct AlertKey FROM Servers t1,AlertServers t2 where t2.serverid=0 and ( ServerName in ('" + ServerName + "')" +
                         "and (t1.LocationID = t2.LocationID or t2.LocationID = 0))" +
                         "union " +
                         "SELECT distinct AlertKey FROM URLs t1,AlertServers t2 where t2.serverid=0 and (Name in ('" + ServerName + "')" +
                         "and (t1.LocationID = t2.LocationID or t2.LocationID = 0)))";     
                AlertDetails = objAdaptor.FetchData(sqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return AlertDetails;
        }

 //       public DataTable GetAlertTab(string ServerName)
 //       {
 //           DataTable AlertDetails = new DataTable();
 //           try
 //           {
 //               //9/11/2013 NS modified the query below
 //               //string sqlQuery = "select ID, AlertName as Name ,HoursIndicator,SendTo,CopyTo,BlindCopyTo,StartTime,Duration, Day,t1.AlertKey,tab.EventName from  AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey inner join" +
 ////"(select t3.AlertKey ,t4.EventName from  AlertDeviceTypes t3 inner join EventsMaster t4 on t3.ServerTypeID=t4.ServerTypeID where t3.ServerTypeID in " +
 ////"(select ID from Servers where ServerName in ('" + ServerName + "'))) as tab on tab.AlertKey=t1.AlertKey";//Updations by Brijesh
 //               string sqlQuery = "SELECT t2.ID,t1.AlertKey,t1.AlertName Name,HoursIndicator,SendTo,CopyTo,BlindCopyTo, " +
 //                   "StartTime,Duration,Day,EventName,t3.ServerTypeID,t4.ServerType FROM AlertNames t1 " +
 //                   "INNER JOIN AlertDetails t2 ON t1.AlertKey=t2.AlertKey,EventsMaster t3 " + 
 //                   "INNER JOIN ServerTypes t4 ON t3.ServerTypeID=t4.ID " +
 //                   "WHERE t1.AlertKey NOT IN " +
 //                   "( " +
 //                   "SELECT t2.AlertKey FROM Servers t1 " + 
 //                   "INNER JOIN AlertServers t2 ON t1.ID=t2.ServerID " +
 //                   "WHERE ServerName='" + ServerName + "' " + 
 //                   ") " +
 //                   "EXCEPT " + 
 //                   "SELECT t3.ID,t1.AlertKey,t2.AlertName Name,HoursIndicator,SendTo,CopyTo,BlindCopyTo, " + 
 //                   "StartTime,Duration,Day,EventName,t1.ServerTypeID,t6.ServerType " + 
 //                   "FROM EventsMaster t4,ServerTypes t6,AlertDeviceTypes t1 " + 
 //                   "INNER JOIN AlertNames t2 ON t1.AlertKey=t2.AlertKey " + 
 //                   "INNER JOIN AlertDetails t3 ON t2.AlertKey=t3.AlertKey " +
 //                   "WHERE t4.ServerTypeID=t1.ServerTypeID AND t1.AlertKey NOT IN " + 
 //                   "( " + 
 //                   "SELECT t1.AlertKey FROM AlertNames t1 " + 
 //                   "INNER JOIN AlertServers t2 ON t1.AlertKey=t2.AlertKey " + 
 //                   "INNER JOIN Servers t3 ON t2.ServerID=t3.id " + 
 //                   "WHERE ServerName='" + ServerName + "' " +
 //                   ") " +  
 //                   "EXCEPT " + 
 //                   "SELECT t4.ID,t1.AlertKey,t3.AlertName Name,HoursIndicator,SendTo,CopyTo,BlindCopyTo, " + 
 //                   "StartTime,Duration,Day,EventName,ServerTypeID,t6.ServerType FROM AlertEvents t1 " + 
 //                   "INNER JOIN EventsMaster t2 ON t1.EventID=t2.ID,AlertNames t3 " + 
 //                   "INNER JOIN AlertDetails t4 ON t3.AlertKey=t4.AlertKey,ServerTypes t6 " + 
 //                   "WHERE t1.AlertKey NOT IN " + 
 //                   "( " + 
 //                   "SELECT t1.AlertKey FROM AlertNames t1 " + 
 //                   "INNER JOIN AlertServers t2 ON t1.AlertKey=t2.AlertKey " + 
 //                   "INNER JOIN Servers t3 ON t2.ServerID=t3.ID " + 
 //                   "WHERE ServerName='" + ServerName + "' " + 
 //                   ") " + 
 //                   "ORDER BY AlertKey,ServerTypeID ";
 //               AlertDetails = objAdaptor.FetchData(sqlQuery);
 //           }
 //           catch (Exception)
 //           {
                
 //               throw;
 //           }
 //           return AlertDetails;
 //       }

        public DataTable GetAlertTabbyDiffServers0(string ServerId1, string ServerId2, string ServerId3)
        {
            DataTable AlertDetails = new DataTable();
            try
            {
               // string sqlQuery = "select ID,(select AlertName from AlertNames t2 where t2.AlertKey = t1.AlertKey) Name,HoursIndicator,SendTo,CopyTo,BlindCopyTo,EscalateTo,StartTime,EndTime from  AlertDetails t1 where AlertKey in (select AlertKey from AlertServers t2 where ServerID in (select ID from Servers where ServerName in ('" + ServerId1 + "','" + ServerId2 + "','" + ServerId3 + "'))";
                //string sqlQuery = "select AlertName as Name ,HoursIndicator,SendTo,CopyTo,BlindCopyTo,StartTime,Duration,Day,t1.AlertKey,tab.EventName from  AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey inner join" +
                //                     "(select t3.AlertKey ,t4.EventName from  AlertDeviceTypes t3 inner join EventsMaster t4 on t3.ServerTypeID=t4.ServerTypeID where t3.ServerTypeID in " +
                //                     "(select ID from Servers where ServerName in ('" + ServerId1 + "','" + ServerId2 + "','" + ServerId3 + "'))) as tab on tab.AlertKey=t1.AlertKey";//Brijesh Updated

                string sqlQuery = "SELECT t2.ID,t1.AlertKey,t1.AlertName Name,HoursIndicator, SendTo,CopyTo,BlindCopyTo, StartTime, " +
                "Duration,Day, " +
                "SUBSTRING((SELECT distinct ',' + " +
                "ServerType from ServerTypes where ServerType " +
                " in (select distinct ServerType from ServerTypes st, AlertEvents ae where st.ID=ae.servertypeid " +
                "and  ae.AlertKey=t1.AlertKey " +
                "union " +
                "select distinct ServerType from ServerTypes  " +
                "where 1 in (select  case ServerTypeId when 0 then 1 else 0 end from AlertEvents where AlertKey=t1.AlertKey) " +
                ") " +
                "FOR XML PATH('')), 2, 1000000) as ServerType " +
                ",SUBSTRING((SELECT distinct ',' +   " +
                "EventName from EventsMaster where EventName  " +
                " in (select distinct st.EventName from EventsMaster st, AlertEvents ae where st.ID=ae.eventid " +
                "and  ae.AlertKey=t1.AlertKey and ae.EventID<>0 and ae.ServerTypeID=st.ServerTypeID " +
                "union " +
                "select distinct EventName from EventsMaster st, AlertEvents ae where  ae.AlertKey=t1.AlertKey and ae.EventID=0 and ae.ServerTypeID=st.ServerTypeID) " +
                "FOR XML PATH('')), 2, 1000000) as EventName " +
                "FROM AlertNames t1 INNER JOIN AlertDetails t2 ON t1.AlertKey=t2.AlertKey " +
                "where t1.AlertKey in " +
                "(SELECT t2.AlertKey FROM Servers t1 INNER JOIN AlertServers t2 ON t1.ID=t2.ServerID WHERE  " +
                " ServerName in ('" + ServerId1 + "','" + ServerId2 + "','" + ServerId3 + "')" +
                " union " +
                " SELECT t2.AlertKey FROM URLs t1 INNER JOIN AlertServers t2 ON t1.ID=t2.ServerID WHERE " +
                " Name in ('" + ServerId1 + "','" + ServerId2 + "','" + ServerId3 + "')" +
                " ) " +
                "or " +
                "t1.AlertKey  IN  " +
                "(SELECT distinct AlertKey FROM Servers t1,AlertServers t2 where t2.serverid=0 and ( ServerName in ('" + ServerId1 + "','" + ServerId2 + "','" + ServerId3 + "')" +
                "and (t1.LocationID = t2.LocationID or t2.LocationID = 0))" +
                "union " +
                "SELECT distinct AlertKey FROM URLs t1,AlertServers t2 where t2.serverid=0 and (Name in ('" + ServerId1 + "','" + ServerId2 + "','" + ServerId3 + "')" +
                "and (t1.LocationID = t2.LocationID or t2.LocationID = 0)))";   


                AlertDetails = objAdaptor.FetchData(sqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return AlertDetails;
        }

    public DataTable GetAlertTabbyDiffServers(string ServerId1, string ServerId2, string ServerId3, string servertypeid)
        {
            DataTable AlertDetails = new DataTable();
            try
            {
               // string sqlQuery = "select ID,(select AlertName from AlertNames t2 where t2.AlertKey = t1.AlertKey) Name,HoursIndicator,SendTo,CopyTo,BlindCopyTo,EscalateTo,StartTime,EndTime from  AlertDetails t1 where AlertKey in (select AlertKey from AlertServers t2 where ServerID in (select ID from Servers where ServerName in ('" + ServerId1 + "','" + ServerId2 + "','" + ServerId3 + "'))";
                //string sqlQuery = "select AlertName as Name ,HoursIndicator,SendTo,CopyTo,BlindCopyTo,StartTime,Duration,Day,t1.AlertKey,tab.EventName from  AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey inner join" +
                //                     "(select t3.AlertKey ,t4.EventName from  AlertDeviceTypes t3 inner join EventsMaster t4 on t3.ServerTypeID=t4.ServerTypeID where t3.ServerTypeID in " +
                //                     "(select ID from Servers where ServerName in ('" + ServerId1 + "','" + ServerId2 + "','" + ServerId3 + "'))) as tab on tab.AlertKey=t1.AlertKey";//Brijesh Updated

                string sqlQuery = "SELECT t2.ID,t1.AlertKey,t1.AlertName Name,HoursIndicator, SendTo,CopyTo,BlindCopyTo, StartTime, " +
                "Duration,Day, " +
                "SUBSTRING((SELECT distinct ',' + " +
                "ServerType from ServerTypes where ServerType " +
                " in (select distinct ServerType from ServerTypes st, AlertEvents ae where st.ID=ae.servertypeid " +
                "and  ae.AlertKey=t1.AlertKey " +
                "union " +
                "select distinct ServerType from ServerTypes  " +
                "where 1 in (select  case ServerTypeId when 0 then 1 else 0 end from AlertEvents where AlertKey=t1.AlertKey) " +
                ") " +
                "FOR XML PATH('')), 2, 1000000) as ServerType " +
                ",SUBSTRING((SELECT distinct ',' +   " +
                "EventName from EventsMaster where EventName  " +
                " in (select distinct st.EventName from EventsMaster st, AlertEvents ae where st.ID=ae.eventid " +
                "and  ae.AlertKey=t1.AlertKey and ae.EventID<>0 and ae.ServerTypeID=st.ServerTypeID  and st.servertypeid=(select id from ServerTypes where servertype='" + servertypeid + "')" +
                "union " +
                "select distinct EventName from EventsMaster st, AlertEvents ae where  ae.AlertKey=t1.AlertKey and ae.EventID=0 and ae.ServerTypeID=st.ServerTypeID and st.servertypeid=(select id from ServerTypes where servertype='" + servertypeid + "')" +
                "union " +
                "select distinct EventName from EventsMaster st where 1 in (select isnull((select 1  from AlertEvents ae where AlertKey=t1.AlertKey  and ae.EventID=0 and ae.ServerTypeID=0),0)) and ServerTypeID =(select id from ServerTypes where servertype='" + servertypeid + "')) " +
                "FOR XML PATH('')), 2, 1000000) as EventName " +
                "FROM AlertNames t1 INNER JOIN AlertDetails t2 ON t1.AlertKey=t2.AlertKey " +
                "where t1.AlertKey in " +
                "(SELECT t2.AlertKey FROM Servers t1 INNER JOIN AlertServers t2 ON t1.ID=t2.ServerID WHERE  " +
                " ServerName in ('" + ServerId1 + "','" + ServerId2 + "','" + ServerId3 + "')" +
                " union " +
                " SELECT t2.AlertKey FROM URLs t1 INNER JOIN AlertServers t2 ON t1.ID=t2.ServerID WHERE " +
                " Name in ('" + ServerId1 + "','" + ServerId2 + "','" + ServerId3 + "')" +
                " ) " +
                "or " +
                "t1.AlertKey  IN  " +
                "(SELECT distinct AlertKey FROM Servers t1,AlertServers t2 where t2.serverid=0 and ( ServerName in ('" + ServerId1 + "','" + ServerId2 + "','" + ServerId3 + "')" +
                "and (t1.LocationID = t2.LocationID or t2.LocationID = 0))" +
                "union " +
                "SELECT distinct AlertKey FROM URLs t1,AlertServers t2 where t2.serverid=0 and (Name in ('" + ServerId1 + "','" + ServerId2 + "','" + ServerId3 + "')" +
                "and (t1.LocationID = t2.LocationID or t2.LocationID = 0)))";   


                AlertDetails = objAdaptor.FetchData(sqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return AlertDetails;
        }

        //Brijesh
        public bool InsertAlertDetails(AlertDetails alertObj)
        {
            bool i = false;
            //4/4/2014 NS added for VSPLUS-519
            int snmpTrap = 0;
            int persistentAlert = 0;
            string scriptid = "";
            try
            {
                //4/4/2014 NS modified for VSPLUS-519
                if (alertObj.SendSNMPTrap)
                {
                    snmpTrap = 1;
                }
                if (alertObj.EnablePersistentAlert)
                {
                    persistentAlert = 1;
                }
                //12/4/2014 NS added for VSPLUS-1229
                if (alertObj.ScriptID == 0 || alertObj.ScriptID.ToString() == "")
                {
                    scriptid = "NULL";
                }
                else
                {
                    scriptid = alertObj.ScriptID.ToString();
                }
                //12/1/2014 NS modified for VSPLUS-946
                //12/4/2014 NS modified for VSPLUS-1229
                //3/31/2015 NS modified for VSPLUS-219
                string sqlQuerry = "INSERT INTO AlertDetails(AlertKey, HoursIndicator, SendTo, CopyTo, BlindCopyTo, StartTime, Duration, " +
                    "Day, SendSNMPTrap,EnablePersistentAlert,SMSTo,ScriptID) " +
                    "VALUES(" + int.Parse(alertObj.AlertKey.ToString()) + ", " + int.Parse(alertObj.HoursIndicator.ToString()) + ", '" + 
                    alertObj.SendTo.ToString() + "', '" + alertObj.CopyTo.ToString() + "', '" + alertObj.BlindCopyTo.ToString() + "', '" + 
                    alertObj.StartTime.ToString() + "', " + Convert.ToInt32(alertObj.Duration.ToString()) + ", '" + 
                    alertObj.Day.ToString() + "', " + snmpTrap + "," + persistentAlert + ",'" + alertObj.SMSTo + "'," + scriptid + ")";
                i = objAdaptor.ExecuteNonQuery(sqlQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }

        public int GetHrsIndicator(string hrsIndicator)
        {
            int i;
            try
            {
                string sqlQuerry = "SELECT ID FROM HoursIndicator WHERE Type = '" + hrsIndicator + "'";
                i = objAdaptor.ExecuteScalar(sqlQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }

        public bool UpdateAlertDetails(AlertDetails alertObj)
        {
            bool i = false;
            //4/4/2014 NS added for VSPLUS-519
            int snmpTrap = 0;
            int persistentAlert = 0;
            string scriptid = "";
            try
            {
                //4/4/2014 NS modified for VSPLUS-519
                if (alertObj.SendSNMPTrap)
                {
                    snmpTrap = 1;
                }
                if (alertObj.EnablePersistentAlert)
                {
                    persistentAlert = 1;
                }
                //12/4/2014 NS added for VSPLUS-1229
                if (alertObj.ScriptID == 0 || alertObj.ScriptID.ToString() == "")
                {
                    scriptid = "NULL";
                }
                else
                {
                    scriptid = alertObj.ScriptID.ToString();
                }
                //12/4/2014 NS modified for VSPLUS-1229
                //3/31/2015 NS modified for VSPLUS-219
                string sqlQuerry = "UPDATE AlertDetails SET HoursIndicator = '" + alertObj.HoursIndicator.ToString() + "',SendTo = '" + 
                    alertObj.SendTo.ToString() + "',CopyTo = '" + alertObj.CopyTo.ToString() + "',BlindCopyTo = '" + 
                    alertObj.BlindCopyTo + "', StartTime = '" + alertObj.StartTime.ToString() + "', Day = '" + 
                    alertObj.Day.ToString() + "', SendSNMPTrap = " + snmpTrap + ", Duration = " + 
                    Convert.ToInt32(alertObj.Duration.ToString()) + ", EnablePersistentAlert = " + 
                    persistentAlert + ", SMSTo='" + alertObj.SMSTo + "', ScriptID=" + scriptid +
                    " WHERE AlertKey = " + Convert.ToInt32(alertObj.AlertKey.ToString()) + " AND ID = " + 
                    Convert.ToInt32(alertObj.ID.ToString()) + "";
                i = objAdaptor.ExecuteNonQuery(sqlQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }

        public DataTable GetAlertDetailsData(int alertKey)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "Select * from AlertDetails where AlertKey = " + alertKey + "";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return dt;
        }

        public bool DeleteFromAlertDetails(int id)
        {
            bool del = false;
            try
            {
                string SqlQuery = "DELETE FROM AlertDetails WHERE ID = " + id + "";
                del = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return del;
        }        
        //Brijesh Ends

        public DataTable GetServerIP(string servername)
        {
            DataTable ServerTable = new DataTable();
            try
            {
                string sqlQuery = "Select ServerName,ID,LocationID,IPAddress from Servers where ServerName='" + servername + "' ";
                ServerTable = objAdaptor.FetchData(sqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ServerTable;

        }

        public DataTable GetEventsFromProcedure()
        {
			try
			{
				return objAdaptor.GetDataFromProcedure("ServerTypeEvents");
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetSpecificServersFromProcedure(string Page,string Control)

        {
			try
			{
				return objAdaptor.FetchSpecificservers("SpecificServerLocations", Page, Control);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		public DataTable GetServersCredentialsFromProcedure(string ServerTypeFilter)
		{
			try
			{
                return objAdaptor.GetServersCredentialsFromProcedure("ServerCredentials",ServerTypeFilter);
			}
			catch (Exception ex)
			{
				
				throw;
			}
			
		}

        public DataTable GetExchangeServersFromProcedure()
        {
            DataTable ExchangeDataTable = objAdaptor.GetDataFromProcedure("ExchangeServerLocations");

			try
			{
				if (ExchangeDataTable.Rows.Count > 0)
				{
					for (int i = 0; i < ExchangeDataTable.Rows.Count - 1; i++)
					{
						DataRow currRow = ExchangeDataTable.Rows[i];
						DataRow nextRow = ExchangeDataTable.Rows[i + 1];

						if (currRow["Name"] != null && currRow["Name"].ToString() == nextRow["Name"].ToString())
						{
							currRow["RoleType"] = currRow["RoleType"].ToString() + "," + nextRow["RoleType"].ToString();

							ExchangeDataTable.Rows[i + 1].Delete();
							ExchangeDataTable.AcceptChanges();
							i--;
						}

					}
				}
			}
			
			
			catch (Exception ex)
			{
				
				throw ex;
			}
			return ExchangeDataTable;
        }

        //11/25/2014 NS modified
        public DataTable GetAlertHistory(string startdate, string enddate)
        {
            DataTable AlertTable = new DataTable();
            try
            {
                //12/4/2013 NS modified
                string sqlQuery = "SELECT * FROM AlertHistory WHERE MONTH(DateTimeOfAlert)="+startdate+
                    " AND YEAR(DateTimeOfAlert)="+ enddate + " ORDER BY DateTimeOfAlert DESC";
                /*
                string sqlQuery = "SELECT * FROM AlertHistory WHERE DateTimeOfAlert BETWEEN '" + startdate + "' " +
                    " AND '" + enddate + "' ORDER BY DateTimeOfAlert DESC";
                 */
                AlertTable = objAdaptor.FetchData(sqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return AlertTable;
        }

        //1/10/2014 NS added
        public string ClearAlertHistory()
        {
            bool updated = false;
            string errorstr = "";
            try
            {
                string sqlquery = "UPDATE AlertHistory SET DateTimeAlertCleared=GETDATE() WHERE DateTimeAlertCleared IS NULL OR " +
                    "DateTimeAlertCleared = ''";
                updated = objAdaptor.ExecuteNonQuery(sqlquery);
            }
            catch (Exception ex)
            {
                errorstr = ex.Message;
            }
            return errorstr;
        }

        //1/10/2014 NS added
        public string DeleteAlertHistory(DateTime daysPriorTo)
        {
            bool updated = false;
            string errorstr = "";
            try
            {
                //8/7/2015 NS modified for VSPlUS-2059
                string sqlquery = "";
                if (daysPriorTo == DateTime.MinValue)
                {
                    sqlquery = "DELETE FROM AlertHistory";
                }
                else
                {
                    sqlquery = "DELETE FROM AlertHistory WHERE [DateTimeOfAlert] < '" + daysPriorTo + "'";
                }
                updated = objAdaptor.ExecuteNonQuery(sqlquery);
            }
            catch (Exception ex)
            {
                errorstr = ex.Message;
            }
            return errorstr;
        }

        //10/20/2014 NS added for VSPLUS-730
        public bool UpdateEventsMaster(string ids)
        {
            bool UpdateRet1 = false;
            bool UpdateRet2 = false;
            int mode = 0;
            string SqlQuery = "";
            try
            {
                SqlQuery = "UPDATE EventsMaster SET [AlertOnRepeat]=1 WHERE ID IN(" + ids + ")";
                mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                if (mode >= 1)
                {
                    UpdateRet1 = true;
                }
                SqlQuery = "UPDATE EventsMaster SET [AlertOnRepeat]=0 WHERE ID NOT IN(" + ids + ")";
                mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                if (mode >= 1)
                {
                    UpdateRet2 = true;
                }
            }
            catch(Exception ex)
            {
				throw ex;
            }
            return UpdateRet1 & UpdateRet2;       
        }

        public DataTable GetSharePointServersFromProcedure()
        {
            DataTable ExchangeDataTable = objAdaptor.GetDataFromProcedure("SharePointServerLocations");

			try
			{
				if (ExchangeDataTable.Rows.Count > 0)
				{
					for (int i = 0; i < ExchangeDataTable.Rows.Count - 1; i++)
					{
						DataRow currRow = ExchangeDataTable.Rows[i];
						DataRow nextRow = ExchangeDataTable.Rows[i + 1];

						if (currRow["Name"] != null && currRow["Name"].ToString() == nextRow["Name"].ToString())
						{
							currRow["RoleType"] = currRow["RoleType"].ToString() + "," + nextRow["RoleType"].ToString();

							ExchangeDataTable.Rows[i + 1].Delete();
							ExchangeDataTable.AcceptChanges();
							i--;
						}

					}
				}
			}
			catch (Exception)
			{
				
				throw;
			}
            
            return ExchangeDataTable;
        }

        public DataTable GetActiveDirectoryServersFromProcedure()
        {
            DataTable ExchangeDataTable = objAdaptor.GetDataFromProcedure("ActiveDirectoryServerLocations");

			try
			{
				if (ExchangeDataTable.Rows.Count > 0)
				{
					for (int i = 0; i < ExchangeDataTable.Rows.Count - 1; i++)
					{
						DataRow currRow = ExchangeDataTable.Rows[i];
						DataRow nextRow = ExchangeDataTable.Rows[i + 1];

						if (currRow["Name"] != null && currRow["Name"].ToString() == nextRow["Name"].ToString())
						{
							currRow["RoleType"] = currRow["RoleType"].ToString() + "," + nextRow["RoleType"].ToString();

							ExchangeDataTable.Rows[i + 1].Delete();
							ExchangeDataTable.AcceptChanges();
							i--;
						}

					}
				}
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
			return ExchangeDataTable;
        }

        //12/4/2014 NS added for VSPLUS-1229
        public DataTable GetCustomScripts()
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT * FROM CustomScripts";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //12/4/2014 NS added for VSPLUS-1229
        public CustomScript GetCustomScriptsByKey(CustomScript CScript)
        {
            DataTable dt = new DataTable();
            CustomScript ReturnScript = new CustomScript();
            try
            {
                string SqlQuery = "SELECT * FROM CustomScripts WHERE ID=" + CScript.ID;
                dt = objAdaptor.FetchData(SqlQuery);
                ReturnScript.ScriptName = dt.Rows[0]["ScriptName"].ToString();
                ReturnScript.ScriptLocation = dt.Rows[0]["ScriptLocation"].ToString();
                ReturnScript.ScriptCommand = dt.Rows[0]["ScriptCommand"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ReturnScript;
        }

        //12/4/2014 NS added for VSPLUS-1229
        public bool InsertScriptDetails(CustomScript CScript)
        {
            bool i = false;
            try
            {
                string sqlQuery = "INSERT INTO CustomScripts(ScriptName,ScriptCommand,ScriptLocation) " +
                    "VALUES('" + CScript.ScriptName + "','" + CScript.ScriptCommand + "','" + CScript.ScriptLocation + "')";
                i = objAdaptor.ExecuteNonQuery(sqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }

        //12/4/2014 NS added for VSPLUS-1229
        public bool UpdateScriptDetails(CustomScript CScript)
        {
            bool i = false;
            try
            {
                string sqlQuery = "UPDATE CustomScripts SET ScriptName='" + CScript.ScriptName + "', " +
                    "ScriptCommand='" + CScript.ScriptCommand + "',ScriptLocation='" + CScript.ScriptLocation + "' " +
                    "WHERE ID=" + Convert.ToInt32(CScript.ID.ToString());
                i = objAdaptor.ExecuteNonQuery(sqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }

        //12/4/2014 NS added for VSPLUS-1229
        public DataTable GetScripts()
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT * FROM CustomScripts ORDER BY ScriptName";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //12/5/2014 NS added for VSPLUS-1229
        //1/8/2015 NS modified for VSPLUS-1315
        public Object DeleteScript(string id)
        {
            Object Delete;
            try
            {
                string SqlQuery = "DELETE FROM CustomScripts WHERE ID=" + id;
                Delete = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Delete = false;
            }
            finally
            {
            }
            return Delete;
        }

        //12/5/2014 NS added for VSPLUS-1229
        public DataTable GetAlertsByScriptID(string id)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT * FROM AlertDetails WHERE ScriptID=" + id;
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

		public DataTable GetServersFromProcedure()
		{
			try
			{
				return objAdaptor.GetDataFromProcedure("ServerLocations");
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetBUsinessHoursFromProcedure(string Page, string Control)

		{
			try
			{
				return objAdaptor.FetchSpecificservers("BusinessHours", Page, Control);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
        //4/2/2015 NS added for VSPLUS-219
        public DataTable GetEscalationData(AlertNames Alertobj)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "Select t1.ID,AlertKey,EscalateTo,EscalationInterval,SMSTo,ISNULL(ScriptName,'') ScriptName, " +
                    "ISNULL(t2.ID,'') ScriptID " +
                    "from EscalationDetails t1 left outer join CustomScripts t2 on t1.ScriptID=t2.ID where AlertKey=(select AlertKey from AlertNames where AlertName='" + Alertobj.AlertName + "')"; //Brijesh
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public bool InsertEscalationDetails(EscalationDetails escalationObj)
        {
            bool i = false;
            string scriptid = "";
            try
            {
                if (escalationObj.ScriptID == 0 || escalationObj.ScriptID.ToString() == "")
                {
                    scriptid = "NULL";
                }
                else
                {
                    scriptid = escalationObj.ScriptID.ToString();
                }
                string sqlQuerry = "INSERT INTO EscalationDetails(AlertKey,EscalateTo,EscalationInterval,SMSTo,ScriptID) " +
                    "VALUES(" + int.Parse(escalationObj.AlertKey.ToString()) + ",'" + escalationObj.EscalateTo + "'," +
                    escalationObj.EscalationInterval.ToString() + ",'" + escalationObj.SMSTo + "'," + scriptid + ")";
                i = objAdaptor.ExecuteNonQuery(sqlQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }
        public DataTable GetEscalationDetailsData(int alertKey)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "Select * from EscalationDetails where AlertKey = " + alertKey + "";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return dt;
        }
        public bool UpdateEscalationDetails(EscalationDetails escalationObj)
        {
            bool i = false;
            string scriptid = "";
            try
            {
                if (escalationObj.ScriptID == 0 || escalationObj.ScriptID.ToString() == "")
                {
                    scriptid = "NULL";
                }
                else
                {
                    scriptid = escalationObj.ScriptID.ToString();
                }
                string sqlQuerry = "UPDATE EscalationDetails SET EscalateTo='" + escalationObj.EscalateTo + "', EscalationInterval=" + 
                    escalationObj.EscalationInterval.ToString() + ",SMSTo='" + escalationObj.SMSTo + "',ScriptID=" + scriptid +
                    " WHERE AlertKey = " + Convert.ToInt32(escalationObj.AlertKey.ToString()) + " AND ID = " +
                    Convert.ToInt32(escalationObj.ID.ToString()) + "";
                i = objAdaptor.ExecuteNonQuery(sqlQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }
        public Object DeleteEscalationDetails(EscalationDetails escalationObj)
        {
            Object Delete;
            try
            {
                string SqlQuery = "Delete from EscalationDetails where ID=" + escalationObj.ID;
                Delete = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Delete = false;
            }
            finally
            {
            }
            return Delete;
        }

        //5/6/2015 NS added for VSPLUS-1622
        public DataTable GetAlertHistoryByAlertID(string AlertKey)
        {
            DataTable AlertHist = new DataTable();
            try
            {
                AlertHist = objAdaptor.GetDataFromProcedure("GetAlertHistoryByAlertKey","AlertKey",AlertKey);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AlertHist;
        }

        //7/17/2015 NS added for VSPLUS-1562
        public DataTable GetAlertEmergencyContacts()
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT * FROM AlertEmergencyContacts";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //7/17/2015 NS added for VSPLUS-1562
        public DataTable GetEmergencyDataByEmail(string id, string email)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT * FROM AlertEmergencyContacts WHERE Email='" + email + "' AND ID <>" + id;
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        //7/17/2015 NS added for VSPLUS-1562
        public object InsertEmergencyAlertData(string id, string email)
        {
            try
            {
                DataTable dt = VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetEmergencyDataByEmail(id,email);
                if (dt.Rows.Count == 0)
                {
                    return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.InsertData(email);
                }
                else return "Email address already exists.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //7/17/2015 NS added for VSPLUS-1562
        public bool InsertData(string email)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO [AlertEmergencyContacts] VALUES('" + email + "')";
                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Insert = false;
            }
            return Insert;
        }
        
        //7/17/2015 NS added for VSPLUS-1562
        public object UpdateEmergencyAlertData(string id, string email)
        {
            try
            {
                DataTable dt = VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetEmergencyDataByEmail(id, email);
                if (dt.Rows.Count == 0)
                {
                    return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.UpdateData(id, email);
                }
                else return "Email address already exists.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //7/17/2015 NS added for VSPLUS-1562
        public bool UpdateData(string id, string email)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "UPDATE [AlertEmergencyContacts] SET Email='" + email + "' WHERE ID=" + id;
                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Insert = false;
            }
            return Insert;
        }

        //7/17/2015 NS added for VSPLUS-1562
        public Object DeleteData(string id)
        {
            Object Update;
            try
            {
                string SqlQuery = "DELETE FROM [AlertEmergencyContacts] WHERE [ID]=" + id + "";
                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            return Update;
        }
		public DataTable GetAlerteventnames()
		{
			DataTable ServerTable = new DataTable();
			try
			{
				string sqlQuery = "Select ID,Name from EventsTemplate";
				ServerTable = objAdaptor.FetchData(sqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}
			return ServerTable;

		}
		public Object Deletetemplatedata(EventsTemplate tempobj)
		{
			Object Delete;
			try
			{
				string SqlQuery = "Delete from EventsTemplate where ID=" + tempobj.ID;
				Delete = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{

				Delete = false;
			}
			finally
			{
			}

			return Delete;

		}

		public DataTable GetSelectedEventsfortemplate2(int ID)
		{

			DataTable EventsTab = new DataTable();
			try
			{
				//string SqlQuery = "select* from EventsMaster where ID in (SELECT Split.a.value('.', 'VARCHAR(100)') AS String  FROM  (SELECT [id],   CAST ('<M>' + REPLACE([EventID], ',', '</M><M>') + '</M>' AS XML) AS String  FROM  EventsTemplate  where ID='ID') AS A CROSS APPLY String.nodes ('/M') AS Split(a))";
				EventsTab = objAdaptor.FetchEventsbyint("Getselectedeventbytemplate", ID);
				//EventsTab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}
			return EventsTab;
		}
		public DataTable GetSelectedEventsfortemplate(int ID)
		{

			DataTable EventsTab = new DataTable();
			try
			{
				//string SqlQuery = "select* from EventsMaster where ID in (SELECT Split.a.value('.', 'VARCHAR(100)') AS String  FROM  (SELECT [id],   CAST ('<M>' + REPLACE([EventID], ',', '</M><M>') + '</M>' AS XML) AS String  FROM  EventsTemplate  where ID='ID') AS A CROSS APPLY String.nodes ('/M') AS Split(a))";
				EventsTab = objAdaptor.FetchEventsbyint("GetselectedeventsbyID", ID);
				//EventsTab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}
			return EventsTab;
		}
		public bool InserttemplateSelectedEvents(string Nmae, string eventids)
		{
			bool Insert = false;
			DataTable eventdt= new DataTable();
			string SqlQuery;

			try
			{
				SqlQuery = "select Name from EventsTemplate where Name= '" + Nmae + "' ";
				eventdt = objAdaptor.FetchData(SqlQuery);
				if (eventdt.Rows.Count > 0)
				{
					Insert = false;
					
				}
				else 
				{
					SqlQuery = "Insert into EventsTemplate(Name,EventID) values('" + Nmae + "','" + eventids + "') ";

					Insert = objAdaptor.ExecuteNonQuery(SqlQuery);	
					
				}


			}
			catch (Exception ex)
			{

				throw ex;
			}
			return Insert;
		}
		public bool UpdatetemplateSelectedEvents(string Nmae, string eventids, int ID, string sessionname)
		{
			bool Insert = false;
			string SqlQuery;
			DataTable dt = new DataTable();
			try
			{

				if (Nmae == sessionname)
				{
					SqlQuery = "UPDATE EventsTemplate SET Name='" + Nmae + "',EventID='" + eventids + "' WHERE ID=" + ID;
					Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
				}
				else 
				{
					SqlQuery = "select Name from EventsTemplate where Name= '" + Nmae + "' ";
					dt = objAdaptor.FetchData(SqlQuery);
					if (dt.Rows.Count > 0)
					{
						Insert = false;
					}
					else
					{
						SqlQuery = "UPDATE EventsTemplate SET Name='" + Nmae + "',EventID='" + eventids + "' WHERE ID=" + ID;
						Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
					}
				}


			}
			catch (Exception ex)
			{

				throw ex;
			}
			return Insert;
		}

		public DataTable GetEventsTemplateidbyNames(string templatename)
		{
			DataTable AlertNamestab = new DataTable();
			try
			{

				string SqlQuery = "Select ID from EventsTemplate where Name='" + templatename + "'";
				AlertNamestab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return AlertNamestab;


		}
		public DataTable GetEventsTemplateNmaes(int ID)
		{
			DataTable AlertNamestab = new DataTable();
			try
			{

				string SqlQuery = "Select * from EventsTemplate where ID=" + ID;
				AlertNamestab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return AlertNamestab;


		}
		public DataTable GetEventsTemplateNames(int ID)
		{
			DataTable AlertNamestab = new DataTable();
			try
			{

				string SqlQuery = "Select Name from EventsTemplate where ID=" + ID;
				AlertNamestab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}

			return AlertNamestab;


		}
		public DataTable GetAllDataForalerteventNames()
		{

			DataTable ProfileNamesDataTable = new DataTable();

			try
			{

				string SqlQuery = "SELECT * FROM EventsTemplate ORDER BY Name";

				ProfileNamesDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return ProfileNamesDataTable;
		}
        // 3/14/2016 Durga Addded for VSPLUS-2704
        public DataTable GetAllOpenAlers(string startdate, string enddate)

        {

            DataTable AlertTable = new DataTable();
            try
            {
                
                string sqlQuery = "SELECT * FROM AlertHistory WHERE MONTH(DateTimeOfAlert)=" + startdate +
                    " AND YEAR(DateTimeOfAlert)=" + enddate + " AND DateTimeAlertCleared IS NULL ORDER BY DateTimeOfAlert DESC";
              
                AlertTable = objAdaptor.FetchData(sqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return AlertTable;
        }

        public bool UpdateEventsMasterforUncheckedCondition(string ids)
        {
            bool UpdateRet1 = false;
           
            int mode = 0;
            string SqlQuery = "";
            try
            {
                SqlQuery = "UPDATE EventsMaster SET [AlertOnRepeat]=0 WHERE ID IN(" + ids + ")";
                mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                if (mode >= 1)
                {
                    UpdateRet1 = true;
                }
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return UpdateRet1 ;
        }

    }    
}
