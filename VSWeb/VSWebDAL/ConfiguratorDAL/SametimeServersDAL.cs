using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using System.Data;
using System.Configuration;

namespace VSWebDAL.ConfiguratorDAL
{
   public class SametimeServersDAL
    {
        private Adaptor objAdaptor = new Adaptor();
        private static SametimeServersDAL _self = new SametimeServersDAL();

        public static SametimeServersDAL Ins
        {
            get 
            {
                return _self; 
            }
        }
		public DataTable getdataforlotussametimegridbyuser(int UserID)//Ms-Raj -changed code by username
		{
			DataTable sametime = new DataTable();
			try
			{
				//1/23/2013 Ns modified reference to the ServerType SameTime to match the correct spelling - Sametime
				//string query = "select *,servers.ID as SID,description,(Select Location from Locations where ID=Servers.LocationID) as Location,IPAddress,ServerName as Name from SametimeServers right join Servers on servers.ID=SametimeServers.serverID where ServerTypeID=(select ID from ServerTypes where ServerType='Sametime')";
				//1/24/2013 NS modified query to get rid of potentially duplicate returned columns
				string query = "select st.ServerID stserverid,st.Category,UserThreshold,ChatThreshold,NChatThreshold,PlacesThreshold,st.Enabled," +
					"st.ScanInterval,OffHoursScanInterval,st.ID ID,st.RetryInterval,ResponseThreshold,nserver,stcommlaunch," +
					"stcommunity,stconfigurationapp,stplaces,stmux,stusers,stonlinedir,stdirectory,stlogger,stlinks," +
					"stprivacy,stsecurity,stpresencemgr,stpresencesubmgr,steventserver,stpolicy,stconfigurationbridge," +
					"stadminsrv,stuserstorage,stchatlogging,stpolling,stpresencecompatmgr,SSL,stservicemanager,stresolve," +
					"stconference,s.ID SID,l.location Location,LocationID,IPAddress,ServerName Name,Description " +
					"from dbo.SametimeServers st right join Servers s on s.ID=st.serverID " +
					"inner join Locations l on l.ID=s.LocationID " +
					"where ServerTypeID in (select ID from ServerTypes where ServerType='Sametime')"+
					"and s.ID not in(select serverID from UserServerRestrictions ur inner join Users U on ur.UserID= U.ID where U.ID='" + UserID + "') order by Name";

				sametime = objAdaptor.FetchData(query);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return sametime;
		}
        public DataTable getdataforlotussametimegrid()
        {
            DataTable sametime = new DataTable();
            try
            {
                //1/23/2013 Ns modified reference to the ServerType SameTime to match the correct spelling - Sametime
                //string query = "select *,servers.ID as SID,description,(Select Location from Locations where ID=Servers.LocationID) as Location,IPAddress,ServerName as Name from SametimeServers right join Servers on servers.ID=SametimeServers.serverID where ServerTypeID=(select ID from ServerTypes where ServerType='Sametime')";
                //1/24/2013 NS modified query to get rid of potentially duplicate returned columns
                string query = "select st.ServerID stserverid,st.Category,UserThreshold,ChatThreshold,NChatThreshold,PlacesThreshold,st.Enabled," +
                    "st.ScanInterval,OffHoursScanInterval,st.ID ID,st.RetryInterval,ResponseThreshold,nserver,stcommlaunch," +
                    "stcommunity,stconfigurationapp,stplaces,stmux,stusers,stonlinedir,stdirectory,stlogger,stlinks," +
                    "stprivacy,stsecurity,stpresencemgr,stpresencesubmgr,steventserver,stpolicy,stconfigurationbridge," +
                    "stadminsrv,stuserstorage,stchatlogging,stpolling,stpresencecompatmgr,SSL,stservicemanager,stresolve," +
                    "stconference,s.ID SID,l.location Location,LocationID,IPAddress,ServerName Name,Description "+
                    "from dbo.SametimeServers st right join Servers s on s.ID=st.serverID " +
                    "inner join Locations l on l.ID=s.LocationID " +
                    "where ServerTypeID in (select ID from ServerTypes where ServerType='Sametime')";

                sametime = objAdaptor.FetchData(query);
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return sametime;
        }
		public DataTable ScangetdataforScanNowItemsgrid()
		{
			DataTable ScanSettings = new DataTable();
			try
			{
				//1/23/2013 Ns modified reference to the ServerType SameTime to match the correct spelling - Sametime
				//string query = "select *,servers.ID as SID,description,(Select Location from Locations where ID=Servers.LocationID) as Location,IPAddress,ServerName as Name from SametimeServers right join Servers on servers.ID=SametimeServers.serverID where ServerTypeID=(select ID from ServerTypes where ServerType='Sametime')";
				//1/24/2013 NS modified query to get rid of potentially duplicate returned columns
				string query = "Select *,Priority as RowOrder from ScanSettings order by priority";

				ScanSettings = objAdaptor.FetchData(query);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return ScanSettings;
		}


        public Object DeleteSametimeServer(SametimeServers SametimeServerObject)
        {
            Object deletesametime;
            try
            {
                //string query = "delete SametimeServers Where ID=" + SametimeServerObject.ID;
                //deletesametime = objAdaptor.ExecuteNonQuery(query);

                string query = "delete SametimeServers Where ID=" + SametimeServerObject.ID;
                string delserver = "delete from servers where ID=" + SametimeServerObject.SID;
                deletesametime = objAdaptor.ExecuteNonQuery(delserver);
                deletesametime = objAdaptor.ExecuteNonQuery(query);
            }
            catch
            {
                deletesametime = false;
            }
            finally
            {
            }
            return deletesametime;
        }

		public Object UpdateSametimeServer(SametimeServers sametimeServer)
		{
			//1/24/2013 NS modified the way Sametime data is populated into the SametimeServers table
			#region comment
			/*
            Object updateSametime;
            try
            {
                //string query = "UPDATE SametimeServers set Name='" + sametimeServer.Name + "',Description='" + sametimeServer.Description + "',Category='" + sametimeServer.Category + "',UserThreshold='" + sametimeServer.UserThreshold + "',ChatThreshold='" + sametimeServer.ChatThreshold + "',NChatThreshold='" + sametimeServer.NChatThreshold + "',PlacesThreshold='" + sametimeServer.PlacesThreshold + "',Enabled='" + sametimeServer.Enabled + "',ScanInterval='" + sametimeServer.ScanInterval + "',OffHoursScanInterval='" + sametimeServer.OffHoursScanInterval + "',Location='" + sametimeServer.Location + "',RetryInterval='" + sametimeServer.RetryInterval + "',ResponseThreshold='" + sametimeServer.ResponseThreshold + "',IPAddress='" + sametimeServer.IPAddress + "',nserver='" + sametimeServer.nserver + "',stcommlaunch='" + sametimeServer.stcommlaunch + "',stcommunity='" + sametimeServer.stcommunity + "',stconfigurationapp='" + sametimeServer.stconfigurationapp + "',stplaces='" + sametimeServer.stplaces + "',stmux='" + sametimeServer.stmux + "',stusers='" + sametimeServer.stusers + "',stonlinedir='" + sametimeServer.stonlinedir + "',stdirectory='" + sametimeServer.stdirectory + "',stlogger='" + sametimeServer.stlogger + "',stlinks='" + sametimeServer.stlinks + "',stprivacy='" + sametimeServer.stprivacy + "',stsecurity='" + sametimeServer.stsecurity + "',stpresencemgr='" + sametimeServer.stpresencemgr + "',stpresencesubmgr='" + sametimeServer.stpresencesubmgr + "',steventserver='" + sametimeServer.steventserver + "',stpolicy='" + sametimeServer.stpolling + "',stconfigurationbridge='" + sametimeServer.stconfigurationbridge + "',stadminsrv='" + sametimeServer.stadminsrv + "',stuserstorage='" + sametimeServer.stuserstorage + "',stchatlogging='" + sametimeServer.stchatlogging + "',stpolling='" + sametimeServer.stpolling + "',stpresencecompatmgr='" + sametimeServer.stpresencecompatmgr + "',SSL='" + sametimeServer.SSL + "',stservicemanager='" + sametimeServer.stservicemanager + "',stresolve='" + sametimeServer.stresolve + "',stconference='" + sametimeServer.stconference + "' where ID='"+sametimeServer.ID+"'";
                //updateSametime = objAdaptor.ExecuteNonQuery(query);
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString());
                System.Data.SqlClient.SqlCommand com = new System.Data.SqlClient.SqlCommand("select max(serverID) from SametimeServers", con);
                con.Open();
                object Maxserverid = com.ExecuteScalar();
                string maxid = Maxserverid.ToString();
                con.Close();
                if (maxid != "")
                {
                    if (int.Parse(maxid) > sametimeServer.SID || int.Parse(maxid) == sametimeServer.SID)
                    {
                        string query = "UPDATE SametimeServers set  Category='" + sametimeServer.Category + "',UserThreshold='" + sametimeServer.UserThreshold + "',ChatThreshold='" + sametimeServer.ChatThreshold + "',NChatThreshold='" + sametimeServer.NChatThreshold + "',PlacesThreshold='" + sametimeServer.PlacesThreshold + "',Enabled='" + sametimeServer.Enabled + "',ScanInterval='" + sametimeServer.ScanInterval + "',OffHoursScanInterval='" + sametimeServer.OffHoursScanInterval + "',RetryInterval='" + sametimeServer.RetryInterval + "',ResponseThreshold='" + sametimeServer.ResponseThreshold + "',nserver='" + sametimeServer.nserver + "',stcommlaunch='" + sametimeServer.stcommlaunch + "',stcommunity='" + sametimeServer.stcommunity + "',stconfigurationapp='" + sametimeServer.stconfigurationapp + "',stplaces='" + sametimeServer.stplaces + "',stmux='" + sametimeServer.stmux + "',stusers='" + sametimeServer.stusers + "',stonlinedir='" + sametimeServer.stonlinedir + "',stdirectory='" + sametimeServer.stdirectory + "',stlogger='" + sametimeServer.stlogger + "',stlinks='" + sametimeServer.stlinks + "',stprivacy='" + sametimeServer.stprivacy + "',stsecurity='" + sametimeServer.stsecurity + "',stpresencemgr='" + sametimeServer.stpresencemgr + "',stpresencesubmgr='" + sametimeServer.stpresencesubmgr + "',steventserver='" + sametimeServer.steventserver + "',stpolicy='" + sametimeServer.stpolling + "',stconfigurationbridge='" + sametimeServer.stconfigurationbridge + "',stadminsrv='" + sametimeServer.stadminsrv + "',stuserstorage='" + sametimeServer.stuserstorage + "',stchatlogging='" + sametimeServer.stchatlogging + "',stpolling='" + sametimeServer.stpolling + "',stpresencecompatmgr='" + sametimeServer.stpresencecompatmgr + "',SSL='" + sametimeServer.SSL + "',stservicemanager='" + sametimeServer.stservicemanager + "',stresolve='" + sametimeServer.stresolve + "',stconference='" + sametimeServer.stconference + "' where ServerID='" + sametimeServer.SID + "'";
                        updateSametime = objAdaptor.ExecuteNonQuery(query);
                        
                       
                    }
                    else
                    {
                        string query = "Insert into SametimeServers(ServerID,Category,UserThreshold,ChatThreshold,NChatThreshold,PlacesThreshold,Enabled,ScanInterval,OffHoursScanInterval" +
                             ",RetryInterval,ResponseThreshold,nserver,stcommlaunch,stcommunity,stconfigurationapp,stplaces,stmux,stusers,stonlinedir,stdirectory,stlogger,stlinks" +
                             ",stprivacy,stsecurity,stpresencemgr,stpresencesubmgr,steventserver,stpolicy,stconfigurationbridge,stadminsrv,stuserstorage,stchatlogging,stpolling,stpresencecompatmgr,SSL,stservicemanager,stresolve,stconference)" +
                  " Values('" + sametimeServer.SID + "','" + sametimeServer.Category + "'," + sametimeServer.UserThreshold + "," + sametimeServer.ChatThreshold + "," + sametimeServer.NChatThreshold +
                  "," + sametimeServer.PlacesThreshold + ",'" + sametimeServer.Enabled + "'," + sametimeServer.ScanInterval + "," + sametimeServer.OffHoursScanInterval + "," + sametimeServer.RetryInterval +
                  "," + sametimeServer.ResponseThreshold + ",'" + sametimeServer.nserver + "','" + sametimeServer.stcommlaunch + "','" + sametimeServer.stcommunity + "','" + sametimeServer.stconfigurationapp +
                  "','" + sametimeServer.stplaces + "','" + sametimeServer.stmux + "','" + sametimeServer.stusers + "','" + sametimeServer.stonlinedir + "','" + sametimeServer.stdirectory + "','" + sametimeServer.stlogger + "','" + sametimeServer.stlinks +
                   "','" + sametimeServer.stprivacy + "','" + sametimeServer.stsecurity + "','" + sametimeServer.stpresencemgr + "','" + sametimeServer.stpresencesubmgr + "','" + sametimeServer.steventserver + "','" + sametimeServer.stpolicy +
                   "','" + sametimeServer.stconfigurationbridge + "','" + sametimeServer.stadminsrv + "','" + sametimeServer.stuserstorage + "','" + sametimeServer.stlogger + "','" + sametimeServer.stpolling + "','" + sametimeServer.stpresencecompatmgr +
                   "','" + sametimeServer.SSL + "','" + sametimeServer.stresolve + "','" + sametimeServer.stservicemanager + "','" + sametimeServer.stconference + "')";
                        updateSametime = objAdaptor.ExecuteNonQuery(query);
                    }

                }
                else
                {
                    string query = "Insert into SametimeServers(ServerID,Category,UserThreshold,ChatThreshold,NChatThreshold,PlacesThreshold,Enabled,ScanInterval,OffHoursScanInterval" +
                              ",RetryInterval,ResponseThreshold,nserver,stcommlaunch,stcommunity,stconfigurationapp,stplaces,stmux,stusers,stonlinedir,stdirectory,stlogger,stlinks" +
                              ",stprivacy,stsecurity,stpresencemgr,stpresencesubmgr,steventserver,stpolicy,stconfigurationbridge,stadminsrv,stuserstorage,stchatlogging,stpolling,stpresencecompatmgr,SSL,stservicemanager,stresolve,stconference)" +
                   " Values('" + sametimeServer.SID + "','" + sametimeServer.Category + "'," + sametimeServer.UserThreshold + "," + sametimeServer.ChatThreshold + "," + sametimeServer.NChatThreshold +
                   "," + sametimeServer.PlacesThreshold + ",'" + sametimeServer.Enabled + "'," + sametimeServer.ScanInterval + "," + sametimeServer.OffHoursScanInterval + "," + sametimeServer.RetryInterval +
                   "," + sametimeServer.ResponseThreshold + ",'" + sametimeServer.nserver + "','" + sametimeServer.stcommlaunch + "','" + sametimeServer.stcommunity + "','" + sametimeServer.stconfigurationapp +
                   "','" + sametimeServer.stplaces + "','" + sametimeServer.stmux + "','" + sametimeServer.stusers + "','" + sametimeServer.stonlinedir + "','" + sametimeServer.stdirectory + "','" + sametimeServer.stlogger + "','" + sametimeServer.stlinks +
                    "','" + sametimeServer.stprivacy + "','" + sametimeServer.stsecurity + "','" + sametimeServer.stpresencemgr + "','" + sametimeServer.stpresencesubmgr + "','" + sametimeServer.steventserver + "','" + sametimeServer.stpolicy +
                    "','" + sametimeServer.stconfigurationbridge + "','" + sametimeServer.stadminsrv + "','" + sametimeServer.stuserstorage + "','" + sametimeServer.stlogger + "','" + sametimeServer.stpolling + "','" + sametimeServer.stpresencecompatmgr +
                    "','" + sametimeServer.SSL + "','" + sametimeServer.stresolve + "','" + sametimeServer.stservicemanager + "','" + sametimeServer.stconference + "')";
                    updateSametime = objAdaptor.ExecuteNonQuery(query);
                }
                



            }
            catch
            {
                updateSametime = false;
            }
             */
			#endregion
			Object updateSametime;
			try
			{
				System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString());
				System.Data.SqlClient.SqlCommand com = new System.Data.SqlClient.SqlCommand("select serverID from SametimeServers where serverID=" + sametimeServer.SID.ToString(), con);
				string maxid =string.Empty;
				try
				{
					con.Open();
					
					object Maxserverid = com.ExecuteScalar();
					
					if (Maxserverid != null)
					{
						maxid = Maxserverid.ToString();
					}
				}
				catch { }
				finally
				{
					con.Close();
				}
				
				if(!string.IsNullOrEmpty(maxid))
				//if (maxid != "")
				{
					string query = "UPDATE SametimeServers set  Category='" + sametimeServer.Category + "',UserThreshold='" +
						sametimeServer.UserThreshold + "',ChatThreshold='" + sametimeServer.ChatThreshold + "',NChatThreshold='" +
						sametimeServer.NChatThreshold + "',PlacesThreshold='" + sametimeServer.PlacesThreshold + "',Enabled='" +
						sametimeServer.Enabled + "',ScanInterval='" + sametimeServer.ScanInterval + "',OffHoursScanInterval='" +
						sametimeServer.OffHoursScanInterval + "',RetryInterval='" + sametimeServer.RetryInterval + "',ResponseThreshold='" +
						sametimeServer.ResponseThreshold + "',nserver='" + sametimeServer.nserver + "',stcommlaunch='" +
						sametimeServer.stcommlaunch + "',stcommunity='" + sametimeServer.stcommunity + "',stconfigurationapp='" +
						sametimeServer.stconfigurationapp + "',stplaces='" + sametimeServer.stplaces + "',stmux='" +
						sametimeServer.stmux + "',stusers='" + sametimeServer.stusers + "',stonlinedir='" +
						sametimeServer.stonlinedir + "',stdirectory='" + sametimeServer.stdirectory + "',stlogger='" +
						sametimeServer.stlogger + "',stlinks='" + sametimeServer.stlinks + "',stprivacy='" +
						sametimeServer.stprivacy + "',stsecurity='" + sametimeServer.stsecurity + "',stpresencemgr='" +
						sametimeServer.stpresencemgr + "',stpresencesubmgr='" + sametimeServer.stpresencesubmgr + "',steventserver='" +
						sametimeServer.steventserver + "',stpolicy='" + sametimeServer.stpolling + "',stconfigurationbridge='" +
						sametimeServer.stconfigurationbridge + "',stadminsrv='" + sametimeServer.stadminsrv + "',stuserstorage='" +
						sametimeServer.stuserstorage + "',stchatlogging='" + sametimeServer.stchatlogging + "',stpolling='" +
						sametimeServer.stpolling + "',stpresencecompatmgr='" + sametimeServer.stpresencecompatmgr + "',SSL='" +
						sametimeServer.SSL + "',stservicemanager='" + sametimeServer.stservicemanager + "',stresolve='" +
						sametimeServer.stresolve + "',stconference='" + sametimeServer.stconference + "',srvawareness='" + sametimeServer.srvawareness + "',srvdirectory='" + sametimeServer.srvdirectory + "',srvstorage='" + sametimeServer.srvstorage + "',srvquery='" + sametimeServer.srvquery + "',srvbuddylist='" + sametimeServer.srvbuddylist + "',srvplace='" + sametimeServer.srvplace + "',srvlookup='" + sametimeServer.srvlookup + "',srvtestchat='" + sametimeServer.srvtestchat + "',srvtestmeeting='" + sametimeServer.srvtestmeeting + "',generalport=" + sametimeServer.generalport + ",proxytype='" + sametimeServer.proxytype + "',proxyprotocol='" + sametimeServer.proxyprotocol + "',db2hostname='" + sametimeServer.db2hostname + "',db2databasename='" + sametimeServer.db2databasename + "',enabledb2port='" + sametimeServer.enabledb2port + "',db2port='" + sametimeServer.db2port + "',FailureThreshold=" + sametimeServer.FailureThreshold + ",Platform='" + sametimeServer.Platform + "',CredentialID='" + sametimeServer.CredentialID + "'," +
						"WsScanMeetingServer='" + sametimeServer.WsScanMeetingServer + "',WsMeetingHost='" + sametimeServer.WsMeetingServerHost + "',WsMeetingRequireSSL='" +sametimeServer.WsMeetingServerRequireSSL + "',WsMeetingPort=" + sametimeServer.WsMeetingPort.ToString() +
						",WsScanMediaServer='" + sametimeServer.WsScanMediaServer + "',WsMediaHost='" + sametimeServer.WsMediaServerHost + "',WsMediaRequireSSL='" + sametimeServer.WsMediaServerRequireSSL + "',WsMediaPort=" + sametimeServer.WsMediaPort.ToString() +
						",STScanExtendedStats='" + sametimeServer.STScanExtendedStats + "',STExtendedStatsPort=" + sametimeServer.STExtendedStatsPort.ToString() + ",TestChatSimulation='" + sametimeServer.TestChatSimulation + "',ChatUser1CredentialsId=" + sametimeServer.ChatUser1Credentials.ToString() + ",ChatUser2CredentialsId=" + sametimeServer.ChatUser2Credentials.ToString() +
						" where ServerID='" +
						sametimeServer.SID + "'";
					updateSametime = objAdaptor.ExecuteNonQuery(query);
				}
				else
				{
					string query = "Insert into SametimeServers(ServerID,Category,UserThreshold,ChatThreshold,NChatThreshold," +
						"PlacesThreshold,Enabled,ScanInterval,OffHoursScanInterval,RetryInterval,ResponseThreshold,nserver," +
						"stcommlaunch,stcommunity,stconfigurationapp,stplaces,stmux,stusers,stonlinedir,stdirectory,stlogger,stlinks," +
						"stprivacy,stsecurity,stpresencemgr,stpresencesubmgr,steventserver,stpolicy,stconfigurationbridge,stadminsrv," +
						"stuserstorage,stchatlogging,stpolling,stpresencecompatmgr,SSL,stservicemanager,stresolve,stconference,FailureThreshold,srvawareness,srvquery,srvdirectory,srvstorage,srvbuddylist,srvplace,srvlookup,srvtestchat,srvtestmeeting,generalport,proxytype,proxyprotocol,db2hostname,db2databasename,enabledb2port,db2port,Platform,CredentialID," +
						"WsScanMeetingServer,WsMeetingPort,WsMeetingHost,WsMeetingRequireSSL,WsScanMediaServer,WsMediaPort,WsMediaHost,WsMediaRequireSSL,STScanExtendedStats,STExtendedStatsPort,ChatUser1CredentialsId,ChatUser2CredentialsId,TestChatSimulation)" +
						" Values(" + sametimeServer.SID + ",'" + sametimeServer.Category + "'," + sametimeServer.UserThreshold + "," +
						sametimeServer.ChatThreshold + "," + sametimeServer.NChatThreshold + "," + sametimeServer.PlacesThreshold + ",'" +
						sametimeServer.Enabled + "'," + sametimeServer.ScanInterval + "," + sametimeServer.OffHoursScanInterval + "," +
						sametimeServer.RetryInterval + "," + sametimeServer.ResponseThreshold + ",'" + sametimeServer.nserver + "','" +
						sametimeServer.stcommlaunch + "','" + sametimeServer.stcommunity + "','" + sametimeServer.stconfigurationapp + "','" +
						sametimeServer.stplaces + "','" + sametimeServer.stmux + "','" + sametimeServer.stusers + "','" +
						sametimeServer.stonlinedir + "','" + sametimeServer.stdirectory + "','" + sametimeServer.stlogger + "','" +
						sametimeServer.stlinks + "','" + sametimeServer.stprivacy + "','" + sametimeServer.stsecurity + "','" +
						sametimeServer.stpresencemgr + "','" + sametimeServer.stpresencesubmgr + "','" + sametimeServer.steventserver + "','" +
						sametimeServer.stpolicy + "','" + sametimeServer.stconfigurationbridge + "','" + sametimeServer.stadminsrv + "','" +
						sametimeServer.stuserstorage + "','" + sametimeServer.stlogger + "','" + sametimeServer.stpolling + "','" +
						sametimeServer.stpresencecompatmgr + "','" + sametimeServer.SSL + "','" + sametimeServer.stservicemanager + "','" +
						sametimeServer.stresolve + "','" + sametimeServer.stconference + "'," + sametimeServer.FailureThreshold + ",'" + sametimeServer.srvawareness + "','" + sametimeServer.srvquery + "', '" +
					   sametimeServer.srvdirectory + "','" + sametimeServer.srvstorage + "','" + sametimeServer.srvbuddylist + "','" + sametimeServer.srvplace + "','" + sametimeServer.srvlookup + "','" + sametimeServer.srvtestchat + "','" + sametimeServer.srvtestmeeting + "'," + sametimeServer.generalport + ",'" + sametimeServer.proxytype + "','" + sametimeServer.proxyprotocol + "','" + sametimeServer.db2hostname + "','" + sametimeServer.db2databasename + "','" + sametimeServer.enabledb2port + "','" + sametimeServer.db2port + "','" + sametimeServer.Platform + "','" + sametimeServer.CredentialID + "'"+
					   ",'" + sametimeServer.WsScanMeetingServer + "'," + sametimeServer.WsMeetingPort + ",'" + sametimeServer.WsMeetingServerHost + "','" + sametimeServer.WsMeetingServerRequireSSL + "','" +
				"','" + sametimeServer.WsScanMediaServer + "'," + sametimeServer.WsMediaPort.ToString() + ",'" + sametimeServer.WsMediaServerHost + "','" + sametimeServer.WsMediaServerRequireSSL + "','" +sametimeServer.STScanExtendedStats + "'," + sametimeServer.STExtendedStatsPort.ToString() +"," + sametimeServer.ChatUser1Credentials.ToString() + "," + sametimeServer.ChatUser2Credentials.ToString() +",'" +sametimeServer.TestChatSimulation + "' )";

					updateSametime = objAdaptor.ExecuteNonQuery(query);
				}
			}
			catch
			{
				updateSametime = false;
			}
			return updateSametime;
		}


		public SametimeServers GetdataSametimeServer(SametimeServers sametimewithid)
		{
			//1/24/2013 NS modified the way Sametime data is populated into the SametimeServers table
			/*
			DataTable getdatasametimedatatable = new DataTable();
			SametimeServers returndata = new SametimeServers();
			try
			{
				 System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString());
				System.Data.SqlClient.SqlCommand da = new System.Data.SqlClient.SqlCommand("select max(serverID) from SametimeServers", con);
				con.Open();
				object maxsid = da.ExecuteScalar();
				con.Close();
				if (maxsid.ToString() != "")
				{
					if (Convert.ToInt32(maxsid.ToString()) == sametimewithid.ID || Convert.ToInt32(maxsid.ToString()) > sametimewithid.ID)
					{

						//string query = "Select * from SametimeServers where [ID]=" + sametimewithid.ID.ToString();
						string query = "Select *,ServerName as Name,Description,(Select Location from Locations where ID = LocationID) as Location,IPAddress from SametimeServers inner join servers on SametimeServers.ServerID=Servers.ID where [ServerID]=" + sametimewithid.ID.ToString();
						getdatasametimedatatable = objAdaptor.FetchData(query);
						returndata.Name = getdatasametimedatatable.Rows[0]["Name"].ToString();
						returndata.Description = getdatasametimedatatable.Rows[0]["Description"].ToString();
						returndata.Category = getdatasametimedatatable.Rows[0]["Category"].ToString();
						returndata.UserThreshold = Convert.ToInt32(getdatasametimedatatable.Rows[0]["UserThreshold"]);
						returndata.ChatThreshold = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ChatThreshold"]);
						returndata.NChatThreshold = Convert.ToInt32(getdatasametimedatatable.Rows[0]["NChatThreshold"]);
						returndata.PlacesThreshold = Convert.ToInt32(getdatasametimedatatable.Rows[0]["PlacesThreshold"]);
						returndata.Enabled = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["Enabled"]);
						returndata.ScanInterval = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ScanInterval"]);
						returndata.OffHoursScanInterval = Convert.ToInt32(getdatasametimedatatable.Rows[0]["OffHoursScanInterval"]);
						returndata.Location = getdatasametimedatatable.Rows[0]["Location"].ToString();
						returndata.RetryInterval = Convert.ToInt32(getdatasametimedatatable.Rows[0]["RetryInterval"]);
						returndata.ResponseThreshold = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ResponseThreshold"]);
						returndata.IPAddress = getdatasametimedatatable.Rows[0]["IPAddress"].ToString();
						returndata.nserver = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["nserver"]);
						returndata.stcommlaunch = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stcommlaunch"]);
						returndata.stcommunity = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stcommunity"]);
						returndata.stconfigurationapp = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stconfigurationapp"]);
						returndata.stplaces = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stplaces"]);
						returndata.stmux = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stmux"]);
						returndata.stusers = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stusers"]);
						returndata.stonlinedir = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stonlinedir"]);
						returndata.stdirectory = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stdirectory"]);
						returndata.stlogger = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stlogger"]);
						returndata.stlinks = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stlinks"]);
						returndata.stprivacy = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stprivacy"]);
						returndata.stsecurity = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stsecurity"]);
						returndata.stpresencemgr = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stpresencemgr"]);
						returndata.stpresencesubmgr = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stpresencesubmgr"]);
						returndata.steventserver = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["steventserver"]);
						returndata.stpolicy = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stpolicy"]);
						returndata.stconfigurationbridge = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stconfigurationbridge"]);
						returndata.stadminsrv = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stadminsrv"]);
						returndata.stuserstorage = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stuserstorage"]);
						returndata.stchatlogging = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stchatlogging"]);
						returndata.stpolling = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stpolling"]);
						returndata.stpresencecompatmgr = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stpresencecompatmgr"]);
						returndata.SSL = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["SSL"]);
						returndata.stservicemanager = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stservicemanager"]);
						returndata.stresolve = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stresolve"]);
						returndata.stconference = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stconference"]);
						returndata.SID = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ServerID"]);

					}
					else
					{
						string q = "select servername as Name,Description,LocationID,IPAddress,ID from Servers where ID=" + sametimewithid.ID;
						getdatasametimedatatable = objAdaptor.FetchData(q);
						returndata.Name = getdatasametimedatatable.Rows[0]["Name"].ToString();
						returndata.Description = getdatasametimedatatable.Rows[0]["Description"].ToString();
						returndata.Location = getdatasametimedatatable.Rows[0]["LocationID"].ToString();
						returndata.IPAddress = getdatasametimedatatable.Rows[0]["IPAddress"].ToString();
						returndata.SID = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ID"]);
						returndata.Category = "";
						//returndata.RetryInterval = 2;
						//returndata.OffHoursScanInterval = 30;
						//returndata.ScanInterval = 8;
						//returndata.ResponseThreshold = 250;
						//returndata.PlacesThreshold = 200;
						//returndata.UserThreshold = 200;
						//returndata.ChatThreshold = 200;
						//returndata.NChatThreshold = 200;
						//returndata.Enabled = true;
                        

					}
				}
				else
				{
					string q = "select servername as Name,Description,LocationID,IPAddress,ID from Servers where ID=" + sametimewithid.ID;
					getdatasametimedatatable = objAdaptor.FetchData(q);
					returndata.Name = getdatasametimedatatable.Rows[0]["Name"].ToString();
					returndata.Description = getdatasametimedatatable.Rows[0]["Description"].ToString();
					returndata.Location = getdatasametimedatatable.Rows[0]["LocationID"].ToString();
					returndata.IPAddress = getdatasametimedatatable.Rows[0]["IPAddress"].ToString();
					returndata.SID = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ID"]);
					returndata.Category = "";
					//returndata.Category = "Production";
					//returndata.RetryInterval = 2;
					//returndata.OffHoursScanInterval = 30;
					//returndata.ScanInterval = 8;
					//returndata.ResponseThreshold = 250;
					//returndata.PlacesThreshold = 200;
					//returndata.UserThreshold = 200;
					//returndata.ChatThreshold = 200;
					//returndata.NChatThreshold = 200;
					//returndata.Enabled = true;
				}
			}
			catch
			{
			}
			finally
			{
			}
			 */
			DataTable getdatasametimedatatable = new DataTable();
			SametimeServers returndata = new SametimeServers();
			try
			{
				System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString());
				System.Data.SqlClient.SqlCommand com = new System.Data.SqlClient.SqlCommand("select serverID from SametimeServers where serverID=" + sametimewithid.ID.ToString(), con);
				string maxid =string.Empty;
				try
				{
					con.Open();
					
					object Maxserverid = com.ExecuteScalar();
					if (Maxserverid != null)
					{
						maxid = Maxserverid.ToString();
					}
				}
				catch { }
				finally
				{
					con.Close();
				}
				if (!string.IsNullOrEmpty(maxid))
				{
					//2/8/2014 NS modified the query to deal with NULL values
					/*
					string query = "select st.ServerID stserverid,Category,UserThreshold,ChatThreshold,NChatThreshold,PlacesThreshold,Enabled," +
					"ScanInterval,OffHoursScanInterval,st.ID ID,RetryInterval,ResponseThreshold,nserver,stcommlaunch," +
					"stcommunity,stconfigurationapp,stplaces,stmux,stusers,stonlinedir,stdirectory,stlogger,stlinks," +
					"stprivacy,stsecurity,stpresencemgr,stpresencesubmgr,steventserver,stpolicy,stconfigurationbridge," +
					"stadminsrv,stuserstorage,stchatlogging,stpolling,stpresencecompatmgr,SSL,stservicemanager,stresolve," +
					"stconference,s.ID SID,l.location Location,LocationID,IPAddress,ServerName Name,Description,st.FailureThreshold " +
					"from dbo.SametimeServers st right join Servers s on s.ID=st.serverID " +
					"inner join Locations l on l.ID=s.LocationID " +
					"where ServerID=" + sametimewithid.ID.ToString();
					 */
					string query = "select st.ServerID stserverid,ISNULL(st.Category,'') Category,ISNULL(st.proxytype,'') proxytype,ISNULL(st.db2port,'') db2port,ISNULL(st.proxyprotocol,'') proxyprotocol,ISNULL(st.db2hostname,'') db2hostname,ISNULL(st.db2databasename,'') db2databasename,ISNULL(UserThreshold,0) UserThreshold, " +
						"ISNULL(ChatThreshold,0) ChatThreshold,ISNULL(generalport,0) generalport,ISNULL(NChatThreshold,0) NChatThreshold,ISNULL(PlacesThreshold,0) PlacesThreshold, " +
						"st.Enabled,st.Platform,st.CredentialID,ISNULL(st.ScanInterval,0) ScanInterval,ISNULL(OffHoursScanInterval,0) OffHoursScanInterval,st.ID ID, " +
						"ISNULL(st.RetryInterval,0) RetryInterval,ISNULL(ResponseThreshold,0) ResponseThreshold, " +
						"ISNULL(nserver,'false') nserver,ISNULL(stcommlaunch,'false') stcommlaunch," +
						"ISNULL(stcommunity,'false') stcommunity,ISNULL(stconfigurationapp,'false') stconfigurationapp, " +
						"ISNULL(stplaces,'false') stplaces,ISNULL(stmux,'false') stmux,ISNULL(stusers,'false') stusers, " +
						"ISNULL(stonlinedir,'false') stonlinedir,ISNULL(stdirectory,'false') stdirectory,ISNULL(stlogger,'false') stlogger, " +
						"ISNULL(stlinks,'false') stlinks,ISNULL(stprivacy,'false') stprivacy,ISNULL(stsecurity,'false') stsecurity, " +
						"ISNULL(stpresencemgr,'false') stpresencemgr,ISNULL(stpresencesubmgr,'false') stpresencesubmgr, " +
						"ISNULL(steventserver,'false') steventserver,ISNULL(stpolicy,'false') stpolicy,ISNULL(stconfigurationbridge,'false') stconfigurationbridge, " +
						"ISNULL(stadminsrv,'false') stadminsrv,ISNULL(stuserstorage,'false') stuserstorage,ISNULL(stchatlogging,'false') stchatlogging, " +
						"ISNULL(stpolling,'false') stpolling,ISNULL(stpresencecompatmgr,'false') stpresencecompatmgr, ISNULL(srvawareness,'false') srvawareness, ISNULL(srvquery,'false') srvquery,ISNULL(srvdirectory,'false') srvdirectory,ISNULL(srvstorage,'false') srvstorage,ISNULL(srvbuddylist,'false') srvbuddylist,ISNULL(srvplace,'false') srvplace,ISNULL(srvlookup,'false') srvlookup,ISNULL(srvtestchat,'false') srvtestchat,ISNULL(srvtestmeeting,'false') srvtestmeeting,ISNULL(enabledb2port,'false') enabledb2port,ISNULL(SSL,'false') SSL, " +
						"ISNULL(stservicemanager,'false') stservicemanager,ISNULL(stresolve,'false') stresolve, " +
						"ISNULL(stconference,'false') stconference,s.ID SID,l.location Location,LocationID,IPAddress,ServerName Name,Description, " +
						"ISNULL(st.FailureThreshold,0) FailureThreshold,ISNULL(st.WsScanMeetingServer,0) WsScanMeetingServer, ISNULL(st.WsScanMediaServer,0) WsScanMediaServer,ISNULL(st.WsMeetingRequireSSL,0) WsMeetingRequireSSL, " +
						"ISNULL(st.WsMediaRequireSSL,0) WsMediaRequireSSL,st.WsMeetingHost,st.WsMeetingPort,st.WsMediaHost,st.WsMediaPort,st.STScanExtendedStats,st.STExtendedStatsPort,st.ChatUser1CredentialsId,st.ChatUser2CredentialsId,ISNULL(st.TestChatSimulation,0) TestChatSimulation " +
						"from dbo.SametimeServers st right join Servers s on s.ID=st.serverID " +
						"inner join Locations l on l.ID=s.LocationID " +
						"left outer join Credentials C1 on C1.ID=st.ChatUser1CredentialsId " +
						"left outer join Credentials C2 on C2.ID=st.ChatUser2CredentialsId " +
						"where ServerID=" + sametimewithid.ID.ToString();

					string serverid = sametimewithid.ID.ToString();


					getdatasametimedatatable = objAdaptor.FetchData(query);
					returndata.Name = getdatasametimedatatable.Rows[0]["Name"].ToString();
					returndata.Description = getdatasametimedatatable.Rows[0]["Description"].ToString();
					returndata.Category = getdatasametimedatatable.Rows[0]["Category"].ToString();
					returndata.UserThreshold = Convert.ToInt32(getdatasametimedatatable.Rows[0]["UserThreshold"]);
					returndata.ChatThreshold = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ChatThreshold"]);
					returndata.NChatThreshold = Convert.ToInt32(getdatasametimedatatable.Rows[0]["NChatThreshold"]);
					returndata.PlacesThreshold = Convert.ToInt32(getdatasametimedatatable.Rows[0]["PlacesThreshold"]);
					returndata.Enabled = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["Enabled"]);
					returndata.ScanInterval = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ScanInterval"]);
					returndata.OffHoursScanInterval = Convert.ToInt32(getdatasametimedatatable.Rows[0]["OffHoursScanInterval"]);
					returndata.Location = getdatasametimedatatable.Rows[0]["Location"].ToString();
					returndata.RetryInterval = Convert.ToInt32(getdatasametimedatatable.Rows[0]["RetryInterval"]);
					returndata.ResponseThreshold = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ResponseThreshold"]);
					returndata.IPAddress = getdatasametimedatatable.Rows[0]["IPAddress"].ToString();
					returndata.nserver = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["nserver"]);
					returndata.stcommlaunch = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stcommlaunch"]);
					returndata.stcommunity = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stcommunity"]);
					returndata.stconfigurationapp = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stconfigurationapp"]);
					returndata.stplaces = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stplaces"]);
					returndata.stmux = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stmux"]);
					returndata.stusers = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stusers"]);
					returndata.stonlinedir = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stonlinedir"]);
					returndata.stdirectory = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stdirectory"]);
					returndata.stlogger = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stlogger"]);
					returndata.stlinks = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stlinks"]);
					returndata.stprivacy = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stprivacy"]);
					returndata.stsecurity = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stsecurity"]);
					returndata.stpresencemgr = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stpresencemgr"]);
					returndata.stpresencesubmgr = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stpresencesubmgr"]);
					returndata.steventserver = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["steventserver"]);
					returndata.stpolicy = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stpolicy"]);
					returndata.stconfigurationbridge = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stconfigurationbridge"]);
					returndata.stadminsrv = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stadminsrv"]);
					returndata.stuserstorage = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stuserstorage"]);
					returndata.stchatlogging = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stchatlogging"]);
					returndata.stpolling = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stpolling"]);
					returndata.stpresencecompatmgr = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stpresencecompatmgr"]);
					returndata.SSL = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["SSL"]);
					returndata.stservicemanager = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stservicemanager"]);
					returndata.stresolve = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stresolve"]);
					returndata.stconference = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["stconference"]);
					returndata.SID = Convert.ToInt32(getdatasametimedatatable.Rows[0]["stserverid"]);
					//................
					returndata.proxytype = getdatasametimedatatable.Rows[0]["proxytype"].ToString();
					returndata.proxyprotocol = getdatasametimedatatable.Rows[0]["proxyprotocol"].ToString();
					returndata.db2hostname = getdatasametimedatatable.Rows[0]["db2hostname"].ToString();
                    if (getdatasametimedatatable.Rows[0]["CredentialID"].ToString() != "")
                    {
                        returndata.CredentialID = Convert.ToInt32(getdatasametimedatatable.Rows[0]["CredentialID"].ToString());
                    }
                        //returndata.db2password = getdatasametimedatatable.Rows[0]["db2password"].ToString();

					returndata.db2databasename = getdatasametimedatatable.Rows[0]["db2databasename"].ToString();
					returndata.db2port = getdatasametimedatatable.Rows[0]["db2port"].ToString();
					// returndata.db2downalert = getdatasametimedatatable.Rows[0]["db2downalert"].ToString();
					//returndata.db2backonlinealert = getdatasametimedatatable.Rows[0]["db2backonlinealert"].ToString();
					returndata.srvawareness = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["srvawareness"]);
					returndata.srvdirectory = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["srvdirectory"]);
					returndata.srvstorage = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["srvstorage"]);

					returndata.srvbuddylist = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["srvbuddylist"]);

					returndata.srvplace = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["srvplace"]);

					returndata.srvlookup = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["srvlookup"]);

					returndata.srvtestchat = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["srvtestchat"]);

					returndata.srvtestmeeting = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["srvtestmeeting"]);
					returndata.srvquery = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["srvquery"]);

					returndata.enabledb2port = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["enabledb2port"]);

					returndata.generalport = Convert.ToInt32(getdatasametimedatatable.Rows[0]["generalport"]);
					returndata.Platform = getdatasametimedatatable.Rows[0]["Platform"].ToString();


					//CredentialsComboBox.DataSource = CredentialsDataTable;
					//CredentialsComboBox.TextField = "AliasName";
					//CredentialsComboBox.ValueField = "ID";
					//CredentialsComboBox.DataBind();

					//''''''''''''''
					if (getdatasametimedatatable.Rows[0]["FailureThreshold"].ToString() != null)
						returndata.FailureThreshold = Convert.ToInt32(getdatasametimedatatable.Rows[0]["FailureThreshold"]);

					if (getdatasametimedatatable.Rows[0]["WsScanMeetingServer"].ToString() != null)
						returndata.WsScanMeetingServer = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["WsScanMeetingServer"].ToString());
					else
						returndata.WsScanMeetingServer = false;

					if (getdatasametimedatatable.Rows[0]["WsMeetingRequireSSL"].ToString() != null)
						returndata.WsMeetingServerRequireSSL = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["WsMeetingRequireSSL"].ToString());
					else
						returndata.WsMeetingServerRequireSSL = false;

					if (getdatasametimedatatable.Rows[0]["WsMeetingPort"].ToString() != "")
						returndata.WsMeetingPort = Convert.ToInt32(getdatasametimedatatable.Rows[0]["WsMeetingPort"].ToString());
					if (getdatasametimedatatable.Rows[0]["WsMeetingHost"].ToString() != null)
						returndata.WsMeetingServerHost = getdatasametimedatatable.Rows[0]["WsMeetingHost"].ToString();


					if (getdatasametimedatatable.Rows[0]["WsScanMediaServer"].ToString() != null)
						returndata.WsScanMediaServer = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["WsScanMediaServer"].ToString());
					else
						returndata.WsScanMediaServer = false;

					if (getdatasametimedatatable.Rows[0]["WsMediaRequireSSL"].ToString() != null)
						returndata.WsMediaServerRequireSSL = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["WsMediaRequireSSL"].ToString());
					else
						returndata.WsMediaServerRequireSSL = false;

					if (getdatasametimedatatable.Rows[0]["WsMediaPort"].ToString() != "")
						returndata.WsMediaPort = Convert.ToInt32(getdatasametimedatatable.Rows[0]["WsMediaPort"].ToString());
					if (getdatasametimedatatable.Rows[0]["WsMediaHost"].ToString() != null)
						returndata.WsMediaServerHost = getdatasametimedatatable.Rows[0]["WsMediaHost"].ToString();
					if (getdatasametimedatatable.Rows[0]["WsMediaHost"].ToString() != null)
						returndata.WsMediaServerHost = getdatasametimedatatable.Rows[0]["WsMediaHost"].ToString();

					if (getdatasametimedatatable.Rows[0]["STScanExtendedStats"].ToString() != "")
						returndata.STScanExtendedStats = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["STScanExtendedStats"].ToString());
					else
						returndata.STScanExtendedStats = false;

					if (getdatasametimedatatable.Rows[0]["STExtendedStatsPort"].ToString() != "")
						returndata.STExtendedStatsPort = Convert.ToInt32(getdatasametimedatatable.Rows[0]["STExtendedStatsPort"].ToString());

					if (getdatasametimedatatable.Rows[0]["ChatUser1CredentialsId"].ToString() != "")
						returndata.ChatUser1Credentials = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ChatUser1CredentialsId"].ToString());

					if (getdatasametimedatatable.Rows[0]["ChatUser2CredentialsId"].ToString() != "")
						returndata.ChatUser2Credentials = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ChatUser2CredentialsId"].ToString());

					if (getdatasametimedatatable.Rows[0]["TestChatSimulation"].ToString() != "")
						returndata.TestChatSimulation = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["TestChatSimulation"].ToString());
					else
						returndata.TestChatSimulation = false;
				}
				else
				{
					string q = "select servername as Name,Description,Location,IPAddress,s.ID ID from Servers s " +
						"inner join Locations l on l.ID=s.LocationID where s.ID=" +
						sametimewithid.ID;
					getdatasametimedatatable = objAdaptor.FetchData(q);
					returndata.Name = getdatasametimedatatable.Rows[0]["Name"].ToString();
					returndata.Description = getdatasametimedatatable.Rows[0]["Description"].ToString();
					returndata.Location = getdatasametimedatatable.Rows[0]["Location"].ToString();
					returndata.IPAddress = getdatasametimedatatable.Rows[0]["IPAddress"].ToString();
					returndata.SID = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ID"]);
					//1/24/2013 NS added
					returndata.Category = "";
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return returndata;
		}

        public bool insertdetails(SametimeServers SametimeServerObject)
        {
            bool insert = false;
            try
            {
                //string query = "Insert into SametimeServers(Name,Description,Category,UserThreshold,ChatThreshold,NChatThreshold,PlacesThreshold,Enabled,ScanInterval,OffHoursScanInterval" +
                //           ",Location,RetryInterval,ResponseThreshold,IPAddress,nserver,stcommlaunch,stcommunity,stconfigurationapp,stplaces,stmux,stusers,stonlinedir,stdirectory,stlogger,stlinks" +
                //           ",stprivacy,stsecurity,stpresencemgr,stpresencesubmgr,steventserver,stpolicy,stconfigurationbridge,stadminsrv,stuserstorage,stchatlogging,stpolling,stpresencecompatmgr,SSL,stservicemanager,stresolve,stconference)"+
                //" Values('" + SametimeServerObject.Name + "','" + SametimeServerObject.Description + "','" + SametimeServerObject.Category + "'," + SametimeServerObject.UserThreshold + "," + SametimeServerObject.ChatThreshold + "," + SametimeServerObject.NChatThreshold +
                //"," + SametimeServerObject.PlacesThreshold + ",'" + SametimeServerObject.Enabled + "'," + SametimeServerObject.ScanInterval + "," + SametimeServerObject.OffHoursScanInterval + ",'" + SametimeServerObject.Location + "'," + SametimeServerObject.RetryInterval +
                //"," + SametimeServerObject.ResponseThreshold + ",'" + SametimeServerObject.IPAddress + "','" + SametimeServerObject.nserver + "','" + SametimeServerObject.stcommlaunch + "','" + SametimeServerObject.stcommunity + "','" + SametimeServerObject.stconfigurationapp +
                //"','" + SametimeServerObject.stplaces + "','" + SametimeServerObject.stmux + "','" + SametimeServerObject.stusers + "','" + SametimeServerObject.stonlinedir + "','" + SametimeServerObject.stdirectory + "','" + SametimeServerObject.stlogger + "','" + SametimeServerObject.stlinks +
                // "','" + SametimeServerObject.stprivacy + "','" + SametimeServerObject.stsecurity + "','" + SametimeServerObject.stpresencemgr + "','" + SametimeServerObject.stpresencesubmgr + "','" + SametimeServerObject.steventserver + "','" + SametimeServerObject.stpolicy +
                // "','" + SametimeServerObject.stconfigurationbridge + "','" + SametimeServerObject.stadminsrv + "','" + SametimeServerObject.stuserstorage + "','" + SametimeServerObject.stlogger + "','" + SametimeServerObject.stpolling + "','" + SametimeServerObject.stpresencecompatmgr +
                // "','" + SametimeServerObject.SSL + "','"+SametimeServerObject.stresolve+"','"+ SametimeServerObject.stservicemanager + "','" + SametimeServerObject.stconference +"')";
				string query = "Insert into SametimeServers(Category,UserThreshold,ChatThreshold,NChatThreshold,PlacesThreshold,Enabled,ScanInterval,OffHoursScanInterval" +
						",RetryInterval,ResponseThreshold,nserver,stcommlaunch,stcommunity,stconfigurationapp,stplaces,stmux,stusers,stonlinedir,stdirectory,stlogger,stlinks" +
						",stprivacy,stsecurity,stpresencemgr,stpresencesubmgr,steventserver,stpolicy,stconfigurationbridge,stadminsrv,stuserstorage,stchatlogging,stpolling,stpresencecompatmgr,SSL,stservicemanager,stresolve,stconference,FailureThreshold,srvawareness,srvdirectory,srvstorage,srvbuddylist,srvplace,srvlookup,srvtestchat,srvtestmeeting,generalport,proxytype,proxyprotocol,db2hostname,db2login,db2password,db2databasename,enabledb2port,db2port,db2downalert,db2backonlinealert,srvquery," +
						"WsScanMeetingServer,WsMeetingPort,WsMeetingHost,WsMeetingRequireSSL,WsScanMediaServer,WsMediaPort,WsMediaHost,WsMediaRequireSSL)" +
			 " Values('" + SametimeServerObject.Category + "'," + SametimeServerObject.UserThreshold + "," + SametimeServerObject.ChatThreshold + "," + SametimeServerObject.NChatThreshold +
			 "," + SametimeServerObject.PlacesThreshold + ",'" + SametimeServerObject.Enabled + "'," + SametimeServerObject.ScanInterval + "," + SametimeServerObject.OffHoursScanInterval + "," + SametimeServerObject.RetryInterval +
			 "," + SametimeServerObject.ResponseThreshold + ",'" + SametimeServerObject.nserver + "','" + SametimeServerObject.stcommlaunch + "','" + SametimeServerObject.stcommunity + "','" + SametimeServerObject.stconfigurationapp +
			 "','" + SametimeServerObject.stplaces + "','" + SametimeServerObject.stmux + "','" + SametimeServerObject.stusers + "','" + SametimeServerObject.stonlinedir + "','" + SametimeServerObject.stdirectory + "','" + SametimeServerObject.stlogger + "','" + SametimeServerObject.stlinks +
			  "','" + SametimeServerObject.stprivacy + "','" + SametimeServerObject.stsecurity + "','" + SametimeServerObject.stpresencemgr + "','" + SametimeServerObject.stpresencesubmgr + "','" + SametimeServerObject.steventserver + "','" + SametimeServerObject.stpolicy +
			  "','" + SametimeServerObject.stconfigurationbridge + "','" + SametimeServerObject.stadminsrv + "','" + SametimeServerObject.stuserstorage + "','" + SametimeServerObject.stlogger + "','" + SametimeServerObject.stpolling + "','" + SametimeServerObject.stpresencecompatmgr +
			  "','" + SametimeServerObject.SSL + "','" + SametimeServerObject.stresolve + "','" + SametimeServerObject.stservicemanager + "','" + SametimeServerObject.stconference + "'," + SametimeServerObject.FailureThreshold + ",'" + SametimeServerObject.srvawareness + "','" + SametimeServerObject.srvdirectory + "','" + SametimeServerObject.srvstorage + "','" + SametimeServerObject.srvbuddylist + "','" + SametimeServerObject.srvplace + "','" + SametimeServerObject.srvlookup + "','" + SametimeServerObject.srvtestchat + "','" + SametimeServerObject.srvtestmeeting + "'," + SametimeServerObject.generalport + ",'" + SametimeServerObject.proxytype + "','" + SametimeServerObject.proxyprotocol + "','" + SametimeServerObject.db2hostname + "','" + SametimeServerObject.CredentialID + "','" + SametimeServerObject.db2databasename + "','" + SametimeServerObject.enabledb2port + "','" + SametimeServerObject.db2port + "','" + SametimeServerObject.db2downalert + "','" + SametimeServerObject.db2backonlinealert + "','" + SametimeServerObject.srvquery + "'" +
				",'" + SametimeServerObject.WsScanMeetingServer + "'," + SametimeServerObject.WsMeetingPort + ",'" + SametimeServerObject.WsMeetingServerHost + "','" + SametimeServerObject.WsMeetingServerRequireSSL + "','" +
				"','" + SametimeServerObject.WsScanMediaServer + "'," + SametimeServerObject.WsMediaPort + ",'" + SametimeServerObject.WsMediaServerHost + "','" + SametimeServerObject.WsMediaServerRequireSSL + "')";
                 

                insert = objAdaptor.ExecuteNonQuery(query);


            }
            catch (Exception e)
            {
                insert = false;
                throw e;
            }
            finally
            {
            }
            return insert;

        }

        public DataTable GetIPAddress(SametimeServers Stobj)
        {
            //SametimeServers SametimeObj = new SametimeServers();
            DataTable sametimeTable = new DataTable();
            try
            {
                string sqlQuery = "Select * from SametimeServers where IPAddress='" + Stobj.IPAddress + "'";
                sametimeTable = objAdaptor.FetchData(sqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            return sametimeTable;
        
        }
		public DataTable Getwebspherecell(WebsphereCell Stobj)

		{
			//SametimeServers SametimeObj = new SametimeServers();
			DataTable sametimeTable = new DataTable();
			try
			{
				string sqlQuery = "Select * from WebsphereCell where SametimeId=" + Stobj.SametimeId + "";
				sametimeTable = objAdaptor.FetchData(sqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			return sametimeTable;

		}

		public DataTable GetcellID(WebsphereCell Stobj)
		{
			//SametimeServers SametimeObj = new SametimeServers();
			DataTable sametimeTable = new DataTable();
			try
			{
				string sqlQuery = "Select CellID from WebsphereCell where SametimeId='" + Stobj.SametimeId + "'";
				sametimeTable = objAdaptor.FetchData(sqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			return sametimeTable;

		}


		public bool InsertData1(WebsphereCell STSettingsObject, int key)
		{
			int cellid;
			int Insert = 0;
			bool retInsert = false;
			string Cellname = STSettingsObject.CellName;
			//DataTable dt = new DataTable();
			//string sqlqury = "select * from WebsphereCell where CellName= '" + STSettingsObject.CellName + "'";
			//dt = objAdaptor.FetchData(sqlqury);
			try
			{
				//    if (dt.Rows.Count > 0)
				//    {
				//cellid = Convert.ToInt32(dt.Rows[0]["CellID"]);
				if (STSettingsObject.CellID != null &&STSettingsObject.CellID !=0)
				{
					string SqlQuery2 = "UPDATE WebsphereCell set Name='" + STSettingsObject.Name + "',HostName='" + STSettingsObject.HostName + "',ConnectionType='" + STSettingsObject.ConnectionType +
						 "',PortNo='" + STSettingsObject.PortNo + "',GlobalSecurity='" + STSettingsObject.GlobalSecurity + "',CredentialsID=" + STSettingsObject.CredentialsID + ",Realm='" + STSettingsObject.Realm.ToString() + "' where CellID=" + Convert.ToInt32(STSettingsObject.CellID.ToString())+ "";
						
					//"' where CellID=" + dt.Rows[0]["cellid"] + "";
					Insert = objAdaptor.ExecuteNonQueryRetRows(SqlQuery2);
				}
				//    }
			}
			catch
			{
				Insert = 0;
			}
			if (Insert == 0)
			{
				try
				{
					
					if (STSettingsObject.CellID ==0)
				{
					string SqlQuery;

					SqlQuery = "INSERT INTO WebsphereCell(Name,HostName,ConnectionType,PortNo,GlobalSecurity,CredentialsID,Realm,SametimeId) VALUES('"
					  + STSettingsObject.Name + "','" + STSettingsObject.HostName + "','" + STSettingsObject.ConnectionType + "'," + STSettingsObject.PortNo +
					 ",'" + STSettingsObject.GlobalSecurity + "'," + STSettingsObject.CredentialsID + ",'" + STSettingsObject.Realm + "','" + key + "')";

					Insert = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
				}
				}
				catch
				{
					Insert = 0;
				}
			}
			if (Insert == 1)
			{
				retInsert = true;
			}
			return retInsert;
		}
		//public bool updateswebservers(WebsphereCell STSettingsObject, int key)
		//{
		//    int cellid;
		//    int update = 0;
		//    bool retInsert = false;
		//    string Cellname = STSettingsObject.CellName;
			
		//    try
		//    {
				
		//        if (STSettingsObject.CellID != null && STSettingsObject.CellID != 0)
		//        {
		//            string SqlQuery2 = "UPDATE WebsphereServer set GlobalSecurity='" + STSettingsObject.GlobalSecurity + "' where CellID=" + Convert.ToInt32(STSettingsObject.CellID.ToString()) + "";

					
				
		//            update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery2);
		//        }
			
		//    }
		//    catch
		//    {
		//        update = 0;
		//    }
			
			
		//    return retInsert;
		//}

    }
}
