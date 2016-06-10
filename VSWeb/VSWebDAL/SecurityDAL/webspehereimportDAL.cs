using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VitalSignsWebSphereDLL;
namespace VSWebDAL.SecurityDAL
{
	public class webspehereimportDAL
	{
		private Adaptor objAdaptor = new Adaptor();
		private static webspehereimportDAL _self = new webspehereimportDAL();

		public static webspehereimportDAL Ins
		{
			get { return _self; }
		}
		public DataTable GetServersFromProcedure()
		{
			return objAdaptor.GetDataFromProcedure("WSNodesalldata");
		}
		public DataTable FetserversbycellID(int CellID)
		{
			DataTable statustab = new DataTable();
			try
			{
				statustab = objAdaptor.FetchStatusbyint("WSNodes", CellID);
			}
			catch (Exception)
			{

				throw;
			}

			return statustab;

		}

		public bool updatedata(int cellid, string enable)
		{
			bool update = false;
			string SqlQuery = "";
			try
			{
				// string FeatureID = "";
				//DataTable dt = VSWebDAL.SecurityDAL.MenusDAL.Ins.GetFeatureID(FeatureName);
				//if (dt.Rows.Count > 0)
				//{
				//FeatureID = dt.Rows[0]["ID"].ToString();

				//for (int i = 0; i < MenuDt.Rows.Count; i++)
				//{
				SqlQuery = "UPDATE WebsphereServer set Enabled='" + enable + "' where ServerID=" + cellid + "";

				update = objAdaptor.ExecuteNonQuery(SqlQuery);
				//}
				//}
			}
			catch
			{
				update = false;

            }
            finally
            {
            }
			return update;
        }
		public bool InsertwebsphereData1(Servers ServerObject)
		{
			bool Insert = false;
			bool insertServerDtls = false;
			try
			{
                string SqlQuery = "INSERT INTO [Servers] ([ServerName],[ServerTypeID],[Description],[LocationID],[IPAddress],[ProfileName]) " +
					   "VALUES('" + ServerObject.ServerName + "', " + ServerObject.ServerTypeID + ", '" + ServerObject.Description + "'," + ServerObject.LocationID +
				",'" + ServerObject.IPAddress + "','"+ServerObject.ProfileName+"')";
				//FROM [ServerTypes] t1, [Locations] t2 WHERE "+
				// "[ServerType] ="+STObject.ServerType+" AND [Location] = "+LocObject.Location+")";                

				Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
				Insert = true;

			}
			finally
			{
			}
            return Insert;
		}

		public bool Insertpwd(string AliasName, string UserID, string Password)
		{
			bool Insert = false;
			try
			{

				string SqlQuery = "INSERT INTO Credentials (AliasName,UserID,Password) VALUES('" + AliasName + "','" + UserID + "','" + Password + "')";
				Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Insert = false;
			}
			finally
			{
			}
			return true;
		}
		public DataTable Getcelldata(int id)
		{

			DataTable cells = new DataTable();
			try
			{
				string SqlQuery = "select wc.CellID,wc.CredentialsID,cd.Aliasname as Credentials,wc.CellName,wc.Name,wc.HostName,wc.ConnectionType,wc.PortNo,wc.GlobalSecurity,wc.Realm,cd.AliasName as AliasName from WebsphereCell wc left outer join Credentials cd  on wc.CredentialsID=cd.ID where CellID=" + id + "";
				//string SqlQuery = "select wc.CellName,wc.HostName,wc.ConnectionType,wc.PortNo,wc.GlobalSecurity,wc.Realm,wc.Certification,cd. from WebsphereCell wc inner join Credentials cd.AliasName as AliasName  on wc.CredentialsID=cd.ID where CellID=" + id + "";
				cells = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception)
			{

				throw;
			}
			return cells;
		}
		public DataTable GetCrentials(int id)
		{

			DataTable cells = new DataTable();
			try
			{
				string SqlQuery = "select * from Credentials where ID=" + id + "";

				cells = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception)
			{

				throw;
			}
			return cells;
		}
		public DataTable GetAliasName(Credentials Busibject)
		{
			//SametimeServers SametimeObj = new SametimeServers();
			DataTable CsTable = new DataTable();
			try
			{
				if (Busibject.ID == 0)
				{
					string sqlQuery = "Select * from Credentials where AliasName='" + Busibject.AliasName + "' ";
					CsTable = objAdaptor.FetchData(sqlQuery);
				}
				else
				{
					string sqlQuery = "Select * from Credentials where (AliasName='" + Busibject.AliasName + "')and ID<>" + Busibject.ID + " ";
					CsTable = objAdaptor.FetchData(sqlQuery);
				}
			}
			catch (Exception)
			{

				throw;
			}
			return CsTable;

		}
		public bool Insertpwd(string AliasName, string UserID, string Password, int ServerType)
		{
			bool Insert = false;
			try
			{

				string SqlQuery = "INSERT INTO Credentials (AliasName,UserID,Password,ServerTypeID) VALUES('" + AliasName + "','" + UserID + "','" + Password + "'," + ServerType + ")";
				Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Insert = false;
			}
			finally
			{
			}
			return true;
		}
		public bool InsertData1(WebsphereCell STSettingsObject)
		{
			int cellid;
			int Insert = 0;
			bool retInsert = false;
			string Cellname = STSettingsObject.CellName;

			try
			{
				if (STSettingsObject.CellID != null)
				{
					string SqlUpdate = "";
					string SqlInsert = "";

					if (STSettingsObject.CredentialsID != 0)
					{
						SqlUpdate = "UPDATE WebsphereCell set Name='" + STSettingsObject.Name + "',HostName='" + STSettingsObject.HostName + "',ConnectionType='" + STSettingsObject.ConnectionType +
							"',PortNo='" + STSettingsObject.PortNo + "',Credentialsid='" + STSettingsObject.CredentialsID + "',GlobalSecurity='" + STSettingsObject.GlobalSecurity + "',Realm='" + STSettingsObject.Realm + "'  where CellID=" + STSettingsObject.CellID.ToString() + "";

						SqlInsert = "INSERT INTO WebsphereCell(Name,HostName,ConnectionType,PortNo,GlobalSecurity,Credentialsid,Realm) VALUES('"
						  + STSettingsObject.Name + "','" + STSettingsObject.HostName + "','" + STSettingsObject.ConnectionType + "'," + STSettingsObject.PortNo +
						 ",'" + STSettingsObject.GlobalSecurity + "','" + STSettingsObject.CredentialsID + "','" + STSettingsObject.Realm + "')";

					}
					else
					{
						SqlUpdate = "UPDATE WebsphereCell set Name='" + STSettingsObject.Name + "',HostName='" + STSettingsObject.HostName + "',ConnectionType='" + STSettingsObject.ConnectionType +
							"',PortNo='" + STSettingsObject.PortNo + "',GlobalSecurity='" + STSettingsObject.GlobalSecurity + "',Realm='" + STSettingsObject.Realm + "'  where CellID=" + STSettingsObject.CellID.ToString() + "";

						SqlInsert = "INSERT INTO WebsphereCell(Name,HostName,ConnectionType,PortNo,GlobalSecurity,Realm) VALUES('"
						  + STSettingsObject.Name + "','" + STSettingsObject.HostName + "','" + STSettingsObject.ConnectionType + "'," + STSettingsObject.PortNo +
						 ",'" + STSettingsObject.GlobalSecurity + "','" + STSettingsObject.Realm + "')";

					}

					string sql = "IF EXISTS(SELECT * FROM WebsphereCell WHERE CellID=" + STSettingsObject.CellID.ToString() + ") BEGIN " +
						SqlUpdate + "; END ELSE BEGIN " + SqlInsert + "; END";

					if (objAdaptor.ExecuteNonQueryRetRows(sql) > 0)
						retInsert = true;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
			return retInsert;
		}
		public bool updatedataweb(int cellID, string sname, string enable)
		{
			bool update = false;
			string SqlQuery = "";
			try
			{
				
				SqlQuery = "UPDATE WebsphereServer set Enabled='" + enable + "',  ServerID=" + cellID + " where ServerName='" + sname + "'";

				update = objAdaptor.ExecuteNonQuery(SqlQuery);
				
			}
			catch
			{
				update = false;
			}
			finally
			{
			}
			return update;
		}
		public bool updatewebservers(int cellID)
           {
			bool update = false;
			string SqlQuery = "";
			try
			{

				SqlQuery = "UPDATE WebsphereServer set Enabled='False'  where CellID=" + cellID + "";

				update = objAdaptor.ExecuteNonQuery(SqlQuery);

			}
			catch
			{
				update = false;
			}
			finally
			{
			}
			return update;
		}
		public DataTable GetServerName(Servers Busibject)
		{
			//SametimeServers SametimeObj = new SametimeServers();
			DataTable CsTable = new DataTable();
			try
			{

				string sqlQuery = "Select * from Servers where (ServerName='" + Busibject.ServerName + "')and ID<>" + Busibject.ID + " ";
				CsTable = objAdaptor.FetchData(sqlQuery);

			}
			catch (Exception)
			{

				throw;
			}
			return CsTable;

		}
		public DataTable FetsametimeserversbycellID(int CellID)
		{
			DataTable statustab = new DataTable();
			try
			{
				statustab = objAdaptor.FetchSametimeStatusbyint("WSsametimeNodes", CellID);
			}
			catch (Exception)
			{

				throw;
			}

			return statustab;

		}
		public DataTable Fetsametimeservers(int CellID)

		{
			DataTable statustab = new DataTable();
			try
			{
				statustab = objAdaptor.FetchSametimeStatusbyint("WSsametimeservers", CellID);
			}
			catch (Exception)
			{

				throw;
			}

			return statustab;

		}

		public DataTable GetCredentialValue(string Name)

		{
			DataTable CredentialsDataTable = new DataTable();

			try
			{
				string SqlQuery = "SELECT * from Credentials where AliasName='" + Name + "'";
				CredentialsDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch
			{
			}
			finally
			{
			}
			return CredentialsDataTable;

		}

		public DataTable GetSpecificCellTypes()
		{

			DataTable WebsphereCellDataTable = new DataTable();
			WebsphereCell ReturnLOCbject = new WebsphereCell();
			try
			{
				//Mukund removed Sharepoint from 'not in'
				string SqlQuery = "SELECT * FROM WebsphereCell ";

				WebsphereCellDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return WebsphereCellDataTable;
		}
		public DataTable GetSpecificCellData(int id)
		{

			DataTable WebsphereCellDataTable = new DataTable();
			WebsphereCell ReturnLOCbject = new WebsphereCell();
			try
			{
				//Mukund removed Sharepoint from 'not in'
				string SqlQuery = "SELECT * FROM WebsphereCell where CellID='"+id+"'";

				WebsphereCellDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return WebsphereCellDataTable;
		}

		public DataTable GetNodeName(int CellID)
		{

			DataTable WebsphereNodeDataTable = new DataTable();
			WebsphereCell ReturnLOCbject = new WebsphereCell();
			try
			{
				//Mukund removed Sharepoint from 'not in'
				//string SqlQuery = "SELECT NodeName,NodeID from WebsphereNode Where CellID = " + CellID + " ";
				string SqlQuery = "SELECT * from WebsphereNode Where CellID = " + CellID + " ";
				WebsphereNodeDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return WebsphereNodeDataTable;
		}

		public bool InsertwebsphereData(Servers ServerObject)
		{
			bool Insert = false;
			bool insertServerDtls = false;
			try
			{
				string SqlQuery = "INSERT INTO [Servers] ([ServerName],[ServerTypeID],[Description],[LocationID],[IPAddress]) " +
					   "VALUES('" + ServerObject.ServerName + "', " + ServerObject.ServerTypeID + ", '" + ServerObject.Description + "'," + ServerObject.LocationID +
				",'" + ServerObject.IPAddress + "')";
				//FROM [ServerTypes] t1, [Locations] t2 WHERE "+
				// "[ServerType] ="+STObject.ServerType+" AND [Location] = "+LocObject.Location+")";                

				Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
				Insert = true;
			}

			catch
			{
				Insert = false;
			}
			return Insert;
		}
		public bool Insertwebspherenodesandservers(VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Cell cell, int id)
		{
			bool returnval = false;
			bool insertServerDtls = false;
			string sql;

			DataTable dt = new DataTable();
			

			try

			{

				sql = "select * from WebsphereCell where CellName='" + cell.Name.ToString() + "'";
				dt = objAdaptor.FetchData(sql);
				if (dt.Rows.Count == 0)
				{
					sql = "Update  WebsphereCell set CellName ='" + cell.Name.ToString() + "' where CellID='" + id + "'  ";
					//sql += "INSERT INTO WebsphereCell (CellName) VALUES ('" + cell.Name.ToString() + "');\n";
					returnval = objAdaptor.ExecuteNonQuery(sql);
				}
				sql = "select CellID from WebsphereCell where CellName='" + cell.Name.ToString() + "'";
				dt = objAdaptor.FetchData(sql);
				int cellid = id;
				foreach (VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Node node in cell.Nodes.Node)
				{

					sql = "select * from WebsphereNode where CellID='" + cellid + "' and NodeName='" + node.Name.ToString() + "'";
					dt = objAdaptor.FetchData(sql);

					if (dt.Rows.Count == 0)
					{
						//sql = "INSERT INTO WebsphereNode (NodeName, CellId, HostName) VALUES ('" + node.Name.ToString() + "',(SELECT MAX(CellID) FROM WebsphereCell where CellName='" + cell.Name.ToString() + "'),'" + node.HostName.ToString() + "');\n";
						//INSERT
						sql = "INSERT INTO WebsphereNode (NodeName, CellId, HostName) VALUES ('" + node.Name.ToString() + "','" + cellid + "','" + node.HostName.ToString() + "');\n";
						returnval = objAdaptor.ExecuteNonQuery(sql);
					}
					sql = "select NodeID from WebsphereNode where NodeName='" + node.Name.ToString() + "' and CellID='" + cellid + "'";
					dt = objAdaptor.FetchData(sql);
					int nodeid = Convert.ToInt32(dt.Rows[0]["NodeID"].ToString());

					foreach (string serverName in node.Servers.Server)
					{
						string servername1 = serverName;
						string nodename1 = node.Name.ToString();
						string cellname = cell.Name.ToString();
						string inserservername = servername1 + " [" + cellname + '~' + nodename1 + "]";
						//1/20/16 WS Commented out due to duplicates being able to be made using RMI and SOAP. It will now jsut compare ServerName to stop this issue.  
						//The server name has cell, node and server name in it so it should prevent all duplications
						//sql = "select * from WebsphereServer where CellID='" + cellid + "' and NodeID='" + nodeid + "' and ServerName='" + inserservername + "'";
						sql = "select * from WebsphereServer where ServerName='" + inserservername + "'";
						dt = objAdaptor.FetchData(sql);
						if (dt.Rows.Count == 0)
						{
							
							sql = "INSERT INTO Servers (ServerName, ServerTypeId, Description, LocationId, IPAddress, ProfileName, BusinessHoursID) VALUES ('" + inserservername + "', '22', 'WebSphere', (SELECT MIN(ID) From Locations),'', '0', '0');\n";
							returnval = objAdaptor.ExecuteNonQuery(sql);
							//sql = "INSERT INTO WebsphereServer (ServerName, CellId, NodeId, ServerId) VALUES ('" + inserservername + "',(SELECT MAX(CellID) FROM WebsphereCell where CellName='" + cell.Name.ToString() + "')," +
							//"(SELECT MAX(NodeId) FROM WebsphereNode where NodeName='" + node.Name.ToString() + "'), (SELECT MAX(ID) FROM Servers WHERE ServerName='" + inserservername + "'));\n";
							sql = "INSERT INTO WebsphereServer (ServerName, CellId, NodeId, ServerId) VALUES ('" + inserservername + "','" + cellid + "'," +
							 "'" + nodeid + "', (SELECT MAX(ID) FROM Servers WHERE ServerName='" + inserservername + "'));\n";
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

		public bool InsertwebsphereSametimenodesandservers(VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.Cell cell, int id,int Sid)
		{
			bool returnval = false;
			bool insertServerDtls = false;
			string sql;

			DataTable dt = new DataTable();
			try
			{




				sql = "select * from WebsphereCell where CellName='" + cell.Name.ToString() + "' and  SametimeId='"+Sid+"'";
				dt = objAdaptor.FetchData(sql);
				if (dt.Rows.Count == 0)
				{
					sql = "Update  WebsphereCell set CellName ='" + cell.Name.ToString() + "' where CellID='" + id + "'  ";
					//sql += "INSERT INTO WebsphereCell (CellName) VALUES ('" + cell.Name.ToString() + "');\n";
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

		public DataTable GetCellIdBynmae(string Name)
		{

			DataTable WebsphereCellDataTable = new DataTable();
			WebsphereCell ReturnLOCbject = new WebsphereCell();
			try
			{
				//Mukund removed Sharepoint from 'not in'
				string SqlQuery = "SELECT * FROM WebsphereCell where CellName='" + Name + "'";

				WebsphereCellDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return WebsphereCellDataTable;
		}
		public DataTable GetNmae(string Name)


		{

			DataTable WebsphereCellDataTable = new DataTable();
			WebsphereCell ReturnLOCbject = new WebsphereCell();
			try
			{
				//Mukund removed Sharepoint from 'not in'
				string SqlQuery = "SELECT * FROM WebsphereCell where Name='" + Name + "'";

				WebsphereCellDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return WebsphereCellDataTable;
		}
		public DataTable GetcellinfobyIDandport(string IP,int  portno)
          {

			DataTable WebsphereCellDataTable = new DataTable();
			WebsphereCell ReturnLOCbject = new WebsphereCell();
			try
			{

				string SqlQuery = "SELECT * FROM WebsphereCell where HostName='" + IP + "' and PortNo=" + portno;

				WebsphereCellDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return WebsphereCellDataTable;
		}
		public DataTable GetAllHostNmaes()
	{

			DataTable WebsphereCellDataTable = new DataTable();
			
			try
			{

				string SqlQuery = "SELECT * FROM WebsphereCell";

				WebsphereCellDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return WebsphereCellDataTable;
		}
	}
	}




