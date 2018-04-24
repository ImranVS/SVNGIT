using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

//using Newtonsoft.Json;

namespace VSWebDAL.SecurityDAL
{
   public class NodesDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
		private static NodesDAL _self = new NodesDAL();

		public static NodesDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from LocationsDAL
        /// </summary>

        public DataTable GetAllData()
        {

            DataTable LocationsDataTable = new DataTable();
            Locations ReturnLOCbject = new Locations();
            try
            {
                //3/19/2014 NS modified for VSPLUS-484
				string SqlQuery = "SELECT * FROM [Nodes] ORDER BY Name";

                LocationsDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return LocationsDataTable;
        }

		//public DataTable AssigntoserverUpdateTree()
		//{
		//    DataTable AssigntoserverDataTable = new DataTable();
		//    try
		//    {
		//        string SqlQuery = "select di.ID,di.Name,ni.Name as NodeName,srt.ServerType,lc.Location from DeviceInventory di,ServerTypes srt,Locations lc ,Nodes ni where  di.LocationID=lc.id and srt.id=di.DeviceTypeID and di.AssignedNodeId=ni.id";

		//        AssigntoserverDataTable = objAdaptor.FetchData(SqlQuery);
		//    }
		//    catch (Exception ex)
		//    {
		//        throw ex;
		//    }
		//    finally
		//    {
		//    }
		//    return AssigntoserverDataTable;
		//}

		//public DataTable GetDataFromNodes()
		//{
		//    try
		//    {
		//        return objAdaptor.GetDataFromProcedure("NodeInsertion");
		//    }
		//    catch (Exception ex)
		//    {

		//        throw ex;
		//    }

		//}

		

		public bool Updatenodes(int AssignedNodeId, List<object> fieldValues)
		{
			bool Update = false;

			try
			{
				for (int i = 0; i < fieldValues.Count; i++)
				{
					string SqlQuery = "Update DeviceInventory set AssignedNodeId = '" + AssignedNodeId + "' where ID=" + fieldValues[i];
					Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return Update;
		}

        public DataTable GetAllDatafromNodes()
		{
			DataTable MSServersDataTable = new DataTable();

			try
			{
				string SqlQuery = "select Nd.ID, Name,HostName,Alive,Version,cd.AliasName as Credential ,NodeType,LoadFactor,IsPrimaryNode, (select Location from Locations where Locations.ID = LocationID) Location from Nodes Nd left outer join Credentials  cd on Nd.CredentialsID = cd.Id";
			MSServersDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return MSServersDataTable;
		}
		public DataTable GetAllDataByNames(Nodes ServerObject)
		{

			DataTable ServersDataTable = new DataTable();
			//Servers ReturnSerevrbject = new Servers();
			try
			{
				if (ServerObject.ID == 0)
				{
					
				}
				else
				{
					string SqlQuery = "select *, (select Location from Locations where Locations.ID = LocationID) Location from Nodes where ID=" + ServerObject.ID + "";
					ServersDataTable = objAdaptor.FetchData(SqlQuery);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return ServersDataTable;
		}


		public bool InsertData(Nodes NodesObject)
		{
			bool Insert = false;
			try
			{
				string SqlQuery = "INSERT INTO Nodes (Name,HostName,Alive,Version,CredentialsID,NodeType," +
					"LoadFactor,IsConfiguredPrimaryNode, LocationID) " +
					   " VALUES('" + NodesObject.Name + "','" + NodesObject.HostName + "'," + NodesObject.Alive + ",'" + NodesObject.Version + "'," + NodesObject.CredentialsID + ",'" +
					   NodesObject.NodeType + "'," + NodesObject.LoadFactor + ",'" + NodesObject.IsConfiguredPrimaryNode + "','" + NodesObject.LocationID + "')";

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

		public Object UpdateDataforservers(Nodes NodesObject)
		{
			Object Update;
			try
			{
				string SqlQuery = "";

				if (NodesObject.IsConfiguredPrimaryNode)
					SqlQuery += "UPDATE Nodes SET IsConfiguredPrimaryNode = 'false' WHERE ID <> '" + NodesObject.ID + "';";
				
				SqlQuery += "UPDATE Nodes SET Name='" + NodesObject.Name +
					 "',HostName='" + NodesObject.HostName + "',Alive= " + NodesObject.Alive +
					 ",Version='" + NodesObject.Version + "',CredentialsID=" + NodesObject.CredentialsID + ",NodeType='" + NodesObject.NodeType +
					 "', LoadFactor=" + NodesObject.LoadFactor + ", IsConfiguredPrimaryNode='" + NodesObject.IsConfiguredPrimaryNode + "', LocationID='" + NodesObject.LocationID + "' where ID=" + NodesObject.ID + "";
				
				Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				if (NodesObject.LocationID > 0)
                    objAdaptor.ExecuteNonQuery(" IF NOT EXISTS (select * from SystemMessagesTemp where Details='Default Location has been asigned to the Primary Node automatically by the System. Please check to ensure this value is correct.' and MessageType=0) insert into dbo.SystemMessagesTemp(Details,MessageType,DateCreated) values('Default Location has been asigned to the Primary Node automatically by the System. Please check to ensure this value is correct.',0,Getdate())");
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

		public DataTable GetforAssignNodes()
		{
			DataTable NodesDataTable = new DataTable();

			try
			{
				//string SqlQuery = "select di.ID as ID,di.Name,ni.ID,ni.Name as NodeName,srt.ServerType,lc.Location from DeviceInventory di,ServerTypes srt,Locations lc ,Nodes ni where  di.LocationID=lc.id and srt.id=di.DeviceTypeID and di.AssignedNodeId=ni.id";
				string SqlQuery = "select di.ID as ID,di.Name,di.AssignedNodeId, di.CurrentNodeId,ni.Name as AssignedNodeName, niCur.Name as CurrentNodeName,srt.ServerType,lc.Location " +
								"from DeviceInventory di " +
								"inner join ServerTypes srt on srt.id=di.DeviceTypeId " +
								"left outer join Locations lc on lc.id=di.LocationID " +
								"left outer join Nodes ni on ni.id=di.AssignedNodeId " +
								"left outer join Nodes niCur on niCur.id=di.CurrentNodeId ";
				NodesDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return NodesDataTable;
		}

		public Object DeleteData(Nodes NodesObject)
		{
			Object Update;
			try
			{

				string SqlQuery = "Delete Nodes Where ID=" + NodesObject.ID;

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
		public DataTable GetCredentialsBynameid(Credentials LOCbject)
		{
			DataTable LocationsDataTable = new DataTable();
			try
			{
				string SqlQuery = "Select * from Credentials where ID='" + LOCbject.ID + "'";
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

		public DataTable GetAllNodeServicesDetails()
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = @"
				
					SELECT 
					Nodes.ID,
					Nodes.IsPrimaryNode,
					Nodes.Name,
					Nodes.HostName,
					Nodes.Version,
					Nodes.NodeType,
					Nodes.LoadFactor,
					(case Nodes.IsConfiguredPrimaryNode when 1 then 'Yes' else 'No' end) IsConfiguredPrimaryNode,
					(select Location from Locations where id=Nodes.LocationID) as Location,
					(CASE Nodes.Alive WHEN '1' THEN 'Yes' WHEN '0' THEN 'No' END) Alive,
					(case Nodes.isDisabled when 1 then 'Yes' else 'No' end) isDisabled,
					[VSService_Alerting] as 'Alerting',
					[VSService_Cluster Health] as 'Cluster Health',
					[VSService_Console Commands] as 'Console Commands',
					[VSService_Core] as 'Core',
					[VSService_Core 64-bit] as 'Core 64-bit',
					[VSService_DB Health] as 'DB Health',
					[VSService_Domino] as 'Domino',
					[VSService_EX Journal] as 'EX Journal',
					[VSService_Master Service] as 'Master Service',
					[VSService_Microsoft] as 'Microsoft',
					Nodes.Pulse
					FROM
					(
					SELECT Value, Name, nd.NodeID from NodeDetails nd where Name like 'VSService_%'
					) x
					pivot
					(
					max(value) for Name in ([VSService_Alerting],[VSService_Cluster Health],[VSService_Console Commands],[VSService_Core],[VSService_Core 64-bit],[VSService_Daily Service],[VSService_DB Health],[VSService_Domino],[VSService_EX Journal],[VSService_Master Service],[VSService_Microsoft])
					) p 
					right outer join Nodes on p.NodeID = Nodes.ID
					order by IsPrimaryNode desc, Name
				";

				dt = objAdaptor.FetchData(SqlQuery);
				//populate & return data object
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return dt;
		}


		public DataTable GetAllNodeStatus()
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = @"
				
					SELECT 
					Nodes.IsPrimaryNode,
					Nodes.Name,
					Nodes.HostName,
					(CASE Nodes.Alive WHEN '1' THEN 'Yes' WHEN '0' THEN 'No' END) Alive,
					Nodes.Pulse
					FROM Nodes on p.NodeID = Nodes.ID
					order by IsPrimaryNode desc, Name
				";

				dt = objAdaptor.FetchData(SqlQuery);
				//populate & return data object
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return dt;
		}

		public DataTable GetNodeServices(string NodeName)
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = @"
				
					SELECT
					Nodes.IsPrimaryNode,
					(CASE Nodes.Alive WHEN '1' THEN 'Yes' WHEN '0' THEN 'No' END) Alive,
					[VSService_Alerting] as 'Alerting',
					[VSService_Cluster Health] as 'Cluster Health',
					[VSService_Console Commands] as 'Console Commands',
					[VSService_Core] as 'Core',
					[VSService_Core 64-bit] as 'Core 64-bit',
					[VSService_DB Health] as 'DB Health',
					[VSService_Domino] as 'Domino',
					[VSService_EX Journal] as 'EX Journal',
					[VSService_Master Service] as 'Master Service',
					[VSService_Microsoft] as 'Microsoft'
					FROM
					(
					SELECT Value, Name, nd.NodeID from NodeDetails nd where Name like 'VSService_%'
					) x
					pivot
					(
					max(value) for Name in ([VSService_Alerting],[VSService_Cluster Health],[VSService_Console Commands],[VSService_Core],[VSService_Core 64-bit],[VSService_Daily Service],[VSService_DB Health],[VSService_Domino],[VSService_EX Journal],[VSService_Master Service],[VSService_Microsoft])
					) p 
					inner join Nodes on p.NodeID = Nodes.ID and Nodes.Name='" + NodeName + @"' 
				";

				dt = objAdaptor.FetchData(SqlQuery);
				//populate & return data object
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return dt;
		}

		public void forceCollectionRefresh()
		{
			
			try
			{
				string SqlQuery = "[dbo].[PR_RefreshServerCollection] @nTriggerRefresh = 1;";

				objAdaptor.ExecuteNonQuery(SqlQuery);

			}
			catch
			{

			}
		}

		public bool SetDisableState(Boolean isDisabled, string NodeID)
		{
			bool Update = false;

			try
			{

				string SqlQuery = "Update Nodes set isDisabled = '" + isDisabled.ToString() + "' where ID='" + NodeID + "'";
				Update = objAdaptor.ExecuteNonQuery(SqlQuery);


			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return Update;
		}

	}
}
