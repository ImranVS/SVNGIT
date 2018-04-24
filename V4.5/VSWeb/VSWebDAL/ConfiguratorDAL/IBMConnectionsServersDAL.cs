using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using System.Configuration;

namespace VSWebDAL.ConfiguratorDAL
{
	public class IBMConnectionsServersDAL
	{
		private Adaptor objAdaptor = new Adaptor();
		private static IBMConnectionsServersDAL _self = new IBMConnectionsServersDAL();

		public static IBMConnectionsServersDAL Ins
		{
			get
			{
				return _self;
			}
		}


		public DataTable GetdataforIBMConnectionsServersGridbyUser(int UserID)
		{
			DataTable sametime = new DataTable();

			try
			{

				
				string query = "select st.ServerID,st.Category,st.Enabled,st.ScanInterval,SSL,s.ID SID,l.location Location,LocationID,"+
                               "IPAddress,ServerName Name,Description from dbo.IBMConnectionsServers st right join Servers s on s.ID=st.serverID inner join Locations l on l.ID=s.LocationID "+
							   " where ServerTypeID in (select ID from ServerTypes where ServerType='IBM Connections')" +
                               "and s.ID not in(select serverID from UserServerRestrictions ur inner join Users U on ur.UserID= U.ID where U.ID='1') order by Name";

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

		public Object DeleteIBMConnectionsServers(IBMConnectionsServers IBMConnectionsObject)
		{
			Object deletesametime;
			try
			{

				string query = "delete IBMConnectionsServers Where ID=" + IBMConnectionsObject.ID;
				string delserver = "delete from servers where ID=" + IBMConnectionsObject.SID;
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

		public  IBMConnectionsServers GetdatawithId(IBMConnectionsServers IBMConnectionswithid)
		{
			
			DataTable getdatasametimedatatable = new DataTable();
			IBMConnectionsServers returndata = new IBMConnectionsServers();
			try
			{
				System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString());
				System.Data.SqlClient.SqlCommand com = new System.Data.SqlClient.SqlCommand("select serverID from IBMConnectionsServers where serverID=" + IBMConnectionswithid.ID.ToString(), con);
				string maxid = string.Empty;
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
                    //3/14/2016 NS modified for VSPLUS-2650
					string query = "select st.ServerID stserverid,ISNULL(st.Category,'') Category,ISNULL(st.ProxyServerType,'') ProxyServerType,ISNULL(st.DBPort,'') DBPort,ISNULL(st.ProxyServerProtocall,'') ProxyServerProtocall," +
					"ISNULL(st.DBHostName ,'') DBHostName ,ISNULL(st.DBName,'') DBName,ISNULL(EnableDB2port,'false') EnableDB2port, " +
								   "ISNULL(st.Enabled,0) Enabled, st.CredentialID, st.DBCredentialsID,ISNULL(st.ScanInterval,0) ScanInterval,ISNULL(OffHoursScanInterval,0) OffHoursScanInterval,st.ServerID ID," + 
                                   "ISNULL(st.RetryInterval,0) RetryInterval,ISNULL(ResponseThreshold,0) ResponseThreshold,"+ 
                                   "ISNULL(SSL,'false') SSL, s.ID SID,l.location Location,LocationID,IPAddress,ServerName Name,Description,"+
								   "ISNULL(st.FailureThreshold,0) FailureThreshold, st.ChatUser1CredentialsId,st.ChatUser2CredentialsId,ISNULL(st.TestChatSimulation,0) TestChatSimulation,ISNULL(st.PortNumber,0) PortNumber,ISNULL(st.URL,'') URL from " +
								   "dbo.IBMConnectionsServers st right join Servers s on s.ID=st.serverID inner join Locations l on l.ID=s.LocationID "+   
                                    " left outer join Credentials C1 on C1.ID=st.ChatUser1CredentialsId left outer join "+
                                    "Credentials C2 on C2.ID = st.ChatUser2CredentialsId "+
						             "where ServerID=" + IBMConnectionswithid.ID.ToString();

					string serverid = IBMConnectionswithid.ID.ToString();


					getdatasametimedatatable = objAdaptor.FetchData(query);
					if (getdatasametimedatatable.Rows.Count > 0)
					{
					returndata.Name = getdatasametimedatatable.Rows[0]["Name"].ToString();
					returndata.Description = getdatasametimedatatable.Rows[0]["Description"].ToString();
					returndata.Category = getdatasametimedatatable.Rows[0]["Category"].ToString();
					returndata.Enabled = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["Enabled"]);
					returndata.ScanInterval = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ScanInterval"]);
					returndata.OffHoursScanInterval = Convert.ToInt32(getdatasametimedatatable.Rows[0]["OffHoursScanInterval"]);
					returndata.Location = getdatasametimedatatable.Rows[0]["Location"].ToString();
					returndata.RetryInterval = Convert.ToInt32(getdatasametimedatatable.Rows[0]["RetryInterval"]);
					returndata.ResponseThreshold = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ResponseThreshold"]);
					returndata.IPAddress = getdatasametimedatatable.Rows[0]["IPAddress"].ToString();
					
					returndata.SSL = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["SSL"]);
					returndata.SID = Convert.ToInt32(getdatasametimedatatable.Rows[0]["stserverid"]);

					returndata.ProxyServerType = getdatasametimedatatable.Rows[0]["ProxyServerType"].ToString();
					returndata.ProxyServerProtocall = getdatasametimedatatable.Rows[0]["ProxyServerProtocall"].ToString();
					returndata.DBHostName = getdatasametimedatatable.Rows[0]["DBHostName"].ToString();


					returndata.DBName = getdatasametimedatatable.Rows[0]["DBName"].ToString();
					returndata.DBPort = getdatasametimedatatable.Rows[0]["DBPort"].ToString();
					returndata.EnableDB2port = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["EnableDB2port"]);
					returndata.PortNumber = Convert.ToInt32(getdatasametimedatatable.Rows[0]["PortNumber"]);
					returndata.URL = getdatasametimedatatable.Rows[0]["URL"].ToString();
						if (getdatasametimedatatable.Rows[0]["CredentialID"].ToString() != "")
						{
							returndata.CredentialID = Convert.ToInt32(getdatasametimedatatable.Rows[0]["CredentialID"].ToString());
						}
						if (getdatasametimedatatable.Rows[0]["DBCredentialsID"].ToString() != "")
						{
							returndata.DBCredentialsID = Convert.ToInt32(getdatasametimedatatable.Rows[0]["DBCredentialsID"].ToString());
						}

						if (getdatasametimedatatable.Rows[0]["FailureThreshold"].ToString() != null)
							returndata.FailureThreshold = Convert.ToInt32(getdatasametimedatatable.Rows[0]["FailureThreshold"]);





						if (getdatasametimedatatable.Rows[0]["ChatUser1CredentialsId"].ToString() != "")
							returndata.ChatUser1Credentials = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ChatUser1CredentialsId"].ToString());

						if (getdatasametimedatatable.Rows[0]["ChatUser2CredentialsId"].ToString() != "")
							returndata.ChatUser2Credentials = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ChatUser2CredentialsId"].ToString());

						if (getdatasametimedatatable.Rows[0]["TestChatSimulation"].ToString() != "")
							returndata.TestChatSimulation = Convert.ToBoolean(getdatasametimedatatable.Rows[0]["TestChatSimulation"].ToString());
						else
							returndata.TestChatSimulation = false;
					}
				}
				else
				{
					string q = "select servername as Name,Description,Location,IPAddress,s.ID ID from Servers s " +
						"inner join Locations l on l.ID=s.LocationID where s.ID=" +
						IBMConnectionswithid.ID;
					getdatasametimedatatable = objAdaptor.FetchData(q);
					returndata.Name = getdatasametimedatatable.Rows[0]["Name"].ToString();
					returndata.Description = getdatasametimedatatable.Rows[0]["Description"].ToString();
					returndata.Location = getdatasametimedatatable.Rows[0]["Location"].ToString();
					returndata.IPAddress = getdatasametimedatatable.Rows[0]["IPAddress"].ToString();
					returndata.SID = Convert.ToInt32(getdatasametimedatatable.Rows[0]["ID"]);

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


		public DataTable GetServerType(string ServerName)
		{
			DataTable ServerTypeDetails = new DataTable();
			try
			{
				string sqlQuery = "Select st.ServerType,st.ID  from  Servers se  inner join ServerTypes st  on se.ServertypeID = st.ID where ServerName = '" + ServerName + "'";
				ServerTypeDetails = objAdaptor.FetchData(sqlQuery);
			}
			catch (Exception ex)
			{

				throw ex;
			}
			return ServerTypeDetails;
		}

		public Object UpdateIBMConnectionsServers(IBMConnectionsServers IBMConnectionsServer)
		{
			
			#region comment
		
			#endregion
			Object updateIBMConnection;
			try
			{
				System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString());
				System.Data.SqlClient.SqlCommand com = new System.Data.SqlClient.SqlCommand("select serverID from IBMConnectionsServers where serverID=" + IBMConnectionsServer.SID.ToString(), con);
				string maxid = string.Empty;
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
				//if (maxid != "")
				{
					string query = "UPDATE IBMConnectionsServers set  Category='" + IBMConnectionsServer.Category + "'," +
						"ProxyServerType='" + IBMConnectionsServer.ProxyServerType + "', ProxyServerProtocall = '" + IBMConnectionsServer.ProxyServerProtocall + "', DBHostName = '" + IBMConnectionsServer.DBHostName + "', " +
						"DBName = '" + IBMConnectionsServer.DBName + "',DBPort = '" + IBMConnectionsServer.DBPort + "',EnableDB2port='" + IBMConnectionsServer.EnableDB2port + "'," +
					               "Enabled = '"+IBMConnectionsServer.Enabled +"', ScanInterval='" + IBMConnectionsServer.ScanInterval + "',OffHoursScanInterval='" +IBMConnectionsServer.OffHoursScanInterval + "', "+
						"RetryInterval='" + IBMConnectionsServer.RetryInterval + "', FailureThreshold ='" + IBMConnectionsServer .FailureThreshold+ "', " +
						"ResponseThreshold='" +IBMConnectionsServer.ResponseThreshold + "', "+
						"SSL='" +IBMConnectionsServer.SSL + "', "+
						"CredentialID='" + IBMConnectionsServer.CredentialID + "'," +
                         "TestChatSimulation='" + IBMConnectionsServer.TestChatSimulation + "',ChatUser1CredentialsId=" + IBMConnectionsServer.ChatUser1Credentials.ToString() + ",ChatUser2CredentialsId=" + IBMConnectionsServer.ChatUser2Credentials.ToString() +", "+
						 "URL = '" + IBMConnectionsServer.URL + "',DBCredentialsID = " + IBMConnectionsServer .DBCredentialsID+ " ,PortNumber = " + IBMConnectionsServer.PortNumber + "" +
						" where ServerID ='" + IBMConnectionsServer.SID + "'";
					
					updateIBMConnection = objAdaptor.ExecuteNonQuery(query);
				}
				else
				{
					string query = "Insert into IBMConnectionsServers(ServerID,Category," +
						"Enabled,ScanInterval,OffHoursScanInterval,RetryInterval,ResponseThreshold," +
						"SSL,FailureThreshold,CredentialID,ProxyServerType,ProxyServerProtocall,DBHostName" +
						"ChatUser1CredentialsId,ChatUser2CredentialsId,TestChatSimulation,DBCredentialsID,DBName,DBPort)" +
						" Values(" + IBMConnectionsServer.SID + ",'" + IBMConnectionsServer.Category + "','" + IBMConnectionsServer.Enabled + "',"+
						"'" + IBMConnectionsServer.ScanInterval + "','" + IBMConnectionsServer.OffHoursScanInterval + "','" + IBMConnectionsServer.RetryInterval + "',"+
						"'" + IBMConnectionsServer.ResponseThreshold + "','" + IBMConnectionsServer.SSL + "','" + IBMConnectionsServer.FailureThreshold + "', '" + IBMConnectionsServer.CredentialID + "'," +
						"'" + IBMConnectionsServer.ProxyServerType + "','" + IBMConnectionsServer.ProxyServerProtocall + "','" + IBMConnectionsServer.DBHostName + "',  " +
						"'" + IBMConnectionsServer.DBName + "','" + IBMConnectionsServer.DBPort + "','" + IBMConnectionsServer.EnableDB2port + "', " +
						"'" + IBMConnectionsServer.URL + "', " + IBMConnectionsServer.PortNumber + ", " +
					"" + IBMConnectionsServer.ChatUser1Credentials.ToString() + "," + IBMConnectionsServer.ChatUser2Credentials.ToString() + ",'" + IBMConnectionsServer.TestChatSimulation + "'," + IBMConnectionsServer.DBCredentialsID + ")";

					updateIBMConnection = objAdaptor.ExecuteNonQuery(query);
				}
			}
			catch
			{
				updateIBMConnection = false;
			}
			return updateIBMConnection;
		}

		public DataTable GetIPAddress(IBMConnectionsServers Stobj)
		{
			//SametimeServers SametimeObj = new SametimeServers();
			DataTable sametimeTable = new DataTable();
			try
			{
				string sqlQuery = "Select * from IBMConnectionsServers where IPAddress='" + Stobj.IPAddress + "'";
				sametimeTable = objAdaptor.FetchData(sqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			return sametimeTable;

		}
		public bool insertdetails(IBMConnectionsServers IBMConnectionsServer)
		{
			bool insert = false;
			try
			{

				string query = "Insert into IBMConnectionsServer(ServerID,Category," +
						"Enabled,ScanInterval,OffHoursScanInterval,RetryInterval,ResponseThreshold," +
						"SSL,FailureThreshold,CredentialID," +
						"ChatUser1CredentialsId,ChatUser2CredentialsId,TestChatSimulation)" +
						" Values(" + IBMConnectionsServer.SID + ",'" + IBMConnectionsServer.Category + "','" + IBMConnectionsServer.Enabled + "'," +
						"'" + IBMConnectionsServer.ScanInterval + "','" + IBMConnectionsServer.OffHoursScanInterval + "','" + IBMConnectionsServer.RetryInterval + "'," +
						"'" + IBMConnectionsServer.ResponseThreshold + "','" + IBMConnectionsServer.SSL + "','" + IBMConnectionsServer.CredentialID + "'," +
					"" + IBMConnectionsServer.ChatUser1Credentials.ToString() + ",ChatUser2CredentialsId=" + IBMConnectionsServer.ChatUser2Credentials.ToString() + ",'" + IBMConnectionsServer.TestChatSimulation + "')";


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

		public DataTable GetIBMConnectionsCredentials(int ServerTypeID)
		{
			DataTable CredentialsDataTable = new DataTable();

			try
			{
				string SqlQuery = "SELECT * from Credentials where ServerTypeId=" + ServerTypeID + "";
				CredentialsDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return CredentialsDataTable;
		}

		public bool UpdateDatafortestsnew(IBMConnectionTests IBMConnectiondata)
		{
			string sqlQuery;
			bool Update = false;
			DataTable testdatatable = new DataTable();
			try
			{
				sqlQuery = "select *  from IBMConnectionsTests where ServerId='" + IBMConnectiondata.ServerId + "' and Id= '" + IBMConnectiondata.Id + "' ";
				testdatatable = objAdaptor.FetchData(sqlQuery);
				if (testdatatable.Rows.Count > 0)
				{
					sqlQuery = "UPDATE IBMConnectionsTests set  ServerId='" + IBMConnectiondata.ServerId + "', EnableSimulationTests= '" + IBMConnectiondata.EnableSimulationTests + "',ResponseThreshold=" + IBMConnectiondata.ResponseThreshold + "  WHERE ServerId='" + IBMConnectiondata.ServerId + "' and Id='" + IBMConnectiondata.Id + "'";

					Update = objAdaptor.ExecuteNonQuery(sqlQuery);
				}
				else
				{
					string SqlQuery = "INSERT into IBMConnectionsTests (ServerId,EnableSimulationTests,ResponseThreshold,Id)" +//somaraj
							   "VALUES(" + IBMConnectiondata.ServerId + ", '" + IBMConnectiondata.EnableSimulationTests + "','" + IBMConnectiondata.ResponseThreshold + "','" + IBMConnectiondata.Id + "')";
					Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				}
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

		public int GetIBMConnectionTestsId(string tests)
		{
			int i;
			try
			{
				string sqlQuerry = "SELECT Id  from TestsMaster WHERE Tests = '" + tests + "'";
				i = objAdaptor.ExecuteScalar(sqlQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return i;
		}

		public DataTable GetIBMConnectionTestsData(int ID)
		{
			DataTable Office365TestsDataTable = new DataTable();
			Office365Tests ReturnObject = new Office365Tests();
			try
			{
				string SqlQuery = "Select TM.Tests,IT.EnableSimulationTests,IT.ResponseThreshold  from IBMConnectionsTests IT inner join TestsMaster TM  on IT.Id=TM.Id  where ServerId=" + ID;
				Office365TestsDataTable = objAdaptor.FetchData(SqlQuery);


			}
			catch
			{
			}
			finally
			{
			}
			return Office365TestsDataTable;
		}

		public DataTable GetwebspherecellforIBMC(IBMConnectionsServers Stobj)
		{
			//SametimeServers SametimeObj = new SametimeServers();
			DataTable sametimeTable = new DataTable();
			try
			{
				//string sqlQuery = "Select * from WebsphereCell where SametimeId=" + Stobj.SametimeId + "";
				string sqlQuery = "Select * from WebsphereCell wc inner join IBMConnectionsServers  ic on wc.cellid = ic.WSCellID   where ServerId = " + Stobj.SID + "";
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
				if (STSettingsObject.CellID != null && STSettingsObject.CellID != 0)
				{
					string SqlQuery2 = "UPDATE WebsphereCell set Name='" + STSettingsObject.Name + "',HostName='" + STSettingsObject.HostName + "',ConnectionType='" + STSettingsObject.ConnectionType +
						 "',PortNo='" + STSettingsObject.PortNo + "',GlobalSecurity='" + STSettingsObject.GlobalSecurity + "',CredentialsID=" + STSettingsObject.CredentialsID + ",Realm='" + STSettingsObject.Realm.ToString() + "' where CellID=" + Convert.ToInt32(STSettingsObject.CellID.ToString()) + "";

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

					if (STSettingsObject.CellID == 0)
					{
						string SqlQuery;

						SqlQuery = "INSERT INTO WebsphereCell(Name,HostName,ConnectionType,PortNo,GlobalSecurity,CredentialsID,Realm,IBMConnectionSID) VALUES('"
						  + STSettingsObject.Name + "','" + STSettingsObject.HostName + "','" + STSettingsObject.ConnectionType + "'," + STSettingsObject.PortNo +
						 ",'" + STSettingsObject.GlobalSecurity + "'," + STSettingsObject.CredentialsID + ",'" + STSettingsObject.Realm + "',"+ key +")";

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
		public DataTable GetwebspherecellforIBMCS(IBMConnectionsServers Stobj, int key)
		{
			//SametimeServers SametimeObj = new SametimeServers();
			DataTable sametimeTable = new DataTable();
			try
			{
				//string sqlQuery = "Select * from WebsphereCell where SametimeId=" + Stobj.SametimeId + "";
				string sqlQuery = "Select * from WebsphereCell    where IBMConnectionSID = " +key+ "";
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
				string sqlQuery = "Select * from WebsphereCell where IBMConnectionSID='" + Stobj.IBMConnectionSID+ "'";
				sametimeTable = objAdaptor.FetchData(sqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			return sametimeTable;

		}

		public bool InsertwebsphereSametimenodesandservers(VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Cell cell, int id, int Sid)
		{
			bool returnval = false;
			bool insertServerDtls = false;
			string sql;

			DataTable dt = new DataTable();
			try
			{




				sql = "select * from WebsphereCell where CellName='" + cell.Name.ToString() + "' and  IBMConnectionSID='" + Sid + "'";
				dt = objAdaptor.FetchData(sql);
				if (dt.Rows.Count == 0)
				{
					sql = "Update  WebsphereCell set CellName ='" + cell.Name.ToString() + "' where CellID='" + id + "'  ";
				
					returnval = objAdaptor.ExecuteNonQuery(sql);
				}
				sql = "select CellID from WebsphereCell where CellName='" + cell.Name.ToString() + "'";
				dt = objAdaptor.FetchData(sql);
				int cellid = Convert.ToInt32(dt.Rows[0]["CellID"].ToString());
				foreach (VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Node node in cell.Nodes.Node)
				{

					sql = "select * from WebsphereNode where CellID='" + cellid + "' and NodeName='" + node.Name.ToString() + "'";
					dt = objAdaptor.FetchData(sql);

					if (dt.Rows.Count == 0)
					{
						sql = "INSERT INTO WebsphereNode (NodeName, CellId, HostName) VALUES ('" + node.Name.ToString() + "',(SELECT MAX(CellID) FROM WebsphereCell where CellName='" + cell.Name.ToString() + "'),'" + node.HostName.ToString() + "');\n";
						//INSERT
						returnval = objAdaptor.ExecuteNonQuery(sql);
					}
					sql = "select NodeID from WebsphereNode where NodeName='" + node.Name.ToString() + "'";
					dt = objAdaptor.FetchData(sql);
					int nodeid = Convert.ToInt32(dt.Rows[0]["NodeID"].ToString());

					foreach (string serverName in node.Servers.Server)
					{
						string servername1 = serverName;
						string nodename1 = node.Name.ToString();
						string cellname = cell.Name.ToString();
						string inserservername = servername1 + " [" + cellname + '~' + nodename1 + "]";
						sql = "select * from WebsphereServer where CellID='" + cellid + "' and NodeID='" + nodeid + "' and ServerName='" + inserservername + "'";
						dt = objAdaptor.FetchData(sql);
						if (dt.Rows.Count == 0)
						{
							sql = "INSERT INTO Servers (ServerName, ServerTypeId, Description, LocationId, IPAddress) VALUES ('" + inserservername + "', '22', 'WebSphere', (SELECT MIN(ID) From Locations),'');\n";
							returnval = objAdaptor.ExecuteNonQuery(sql);
							sql = "INSERT INTO WebsphereServer (ServerName, CellId, NodeId, ServerId) VALUES ('" + inserservername + "',(SELECT MAX(CellID) FROM WebsphereCell where CellName='" + cell.Name.ToString() + "')," +
								"(SELECT MAX(NodeId) FROM WebsphereNode where NodeName='" + node.Name.ToString() + "'), (SELECT MAX(ID) FROM Servers WHERE ServerName='" + inserservername + "'));\n";
							returnval = objAdaptor.ExecuteNonQuery(sql);
						}
					}

				}
			}


			finally
			{
			}
			return returnval;
		}

		public DataTable FetsametimeserversbycellID(int CellID)
		{
			DataTable statustab = new DataTable();
			try
			{
				statustab = objAdaptor.FetchSametimeStatusbyint("WSIBMConnectionsNodes", CellID);
			}
			catch (Exception)
			{

				throw;
			}

			return statustab;

		}

		public DataTable GetCredentialID(string name)
		{
			DataTable CompanysDataTable = new DataTable();

			try
			{
				string SqlQuery = "SELECT ID from Credentials  where AliasName='" + name + "'";
				CompanysDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return CompanysDataTable;
		}

	}
}
