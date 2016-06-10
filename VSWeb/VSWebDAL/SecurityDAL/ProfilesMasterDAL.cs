using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.SecurityDAL
{
   public class ProfilesMasterDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static ProfilesMasterDAL _self = new ProfilesMasterDAL();

        public static ProfilesMasterDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from ProfilesMaster
        /// </summary>



        public DataTable GetAllData()
        {
            DataTable ProfilesMasterTable = new DataTable();
            ProfilesMaster ReturnObj = new ProfilesMaster();
            try
            {
				string SqlQuery = "SELECT pm.ID,pm.ServerTypeId,st.ServerType,pm.AttributeName,pm.DefaultValue,pm.UnitOfMeasurement,pm.RelatedTable,pm.RelatedField,st.ServerType,'false' as isSelected" +
							  " FROM ProfilesMaster pm,ServerTypes st where pm.ServerTypeId=st.ID and ServerType='st.ServerType' and pm.RoleType=''"; 
				//and pm.ProfileId =(select id from ProfileNames where ProfileName='Default' )";
					ProfilesMasterTable = objAdaptor.FetchData(SqlQuery);
				
	                //"SELECT pm.ID,pm.ServerTypeId,pm.AttributeName,pm.AttributeMeasurement,pm.RelatedTable,pm.RelatedField,st.ServerType " +
                    //" FROM ProfilesMaster pm,ServerTypes st where pm.ServerTypeId=st.ID";              
            }
            catch
            {
            }
            finally
            {
            }
            return ProfilesMasterTable;
        }
		public DataTable GetAllDataByServerType(string ServerType, string RoleType, string ProfileName)
		{
			DataTable ProfilesMasterTable = new DataTable();
			ProfilesMaster ReturnObj = new ProfilesMaster();
			string SqlQuery = "SELECT pm.ID,pm.ServerTypeId,pm.AttributeName,pm.DefaultValue,pm.UnitOfMeasurement,pm.RelatedTable," +
						"pm.RelatedField, pm.ProfileId,st.ServerType,pm.isSelected  FROM ProfilesMaster pm inner join  ServerTypes  st on pm.ServerTypeId=st.ID  inner join ProfileNames pn on pn.ID=pm.profileId" +
						" where st.ServerType='" + ServerType + "' and pm.RoleType='" + RoleType + "'and pn.ProfileName='" + ProfileName + "' union SELECT pm.ID,pm.ServerTypeId,pm.AttributeName,pm.DefaultValue,pm.UnitOfMeasurement,pm.RelatedTable,pm.RelatedField, pm.ProfileId,st.ServerType,pm.isSelected  FROM ProfilesMaster pm inner join  ServerTypes  st on pm.ServerTypeId=st.ID  inner join ProfileNames pn on pn.ID=pm.profileId where st.ServerType='" + ServerType + "' and pm.RoleType='" + RoleType + "'and  pn.ProfileName='Default' and AttributeName not in (SELECT pm.AttributeName FROM ProfilesMaster pm inner join  ServerTypes  st on pm.ServerTypeId=st.ID  inner join ProfileNames pn on pn.ID=pm.profileId where st.ServerType='" + ServerType + "' and pm.RoleType='" + RoleType + "'and pn.ProfileName='" + ProfileName + "')  order by pm.AttributeName";
			ProfilesMasterTable = objAdaptor.FetchData(SqlQuery);
			try
			{
				if (ProfilesMasterTable.Rows.Count == 0)
				{
					 SqlQuery = "SELECT pm.ID,pm.ServerTypeId,pm.AttributeName,pm.DefaultValue,pm.UnitOfMeasurement,pm.RelatedTable," +
					"pm.RelatedField,isnull(pm.ProfileId,'0') as ProfileId ,st.ServerType,pm.isSelected FROM ProfilesMaster pm inner join  ServerTypes  st on pm.ServerTypeId=st.ID  inner join ProfileNames pn on pn.ID=pm.profileId" +
					" where st.ServerType='" + ServerType + "' and (pm.RoleType='" + RoleType + "' or pm.RoleType is NULL) and pn.ProfileName='Default'  order by pm.AttributeName";
					 ProfilesMasterTable = objAdaptor.FetchData(SqlQuery);
				}			
			
			}
			catch
			{
			}
			finally
			{
			}
			return ProfilesMasterTable;
		}
        public string Getvalue(string AttributeName, string ServerType)
        {
            string svalue;
            DataTable ProfilesMasterdatatable = new DataTable();
            try
            {
                string SqlQuery = "SELECT pm.ID FROM ProfilesMaster pm,ServerTypes st where pm.ServerTypeId=st.ID and pm.AttributeName='" + AttributeName + "' and st.ServerType='" + ServerType + "'";
                ProfilesMasterdatatable = objAdaptor.FetchData(SqlQuery);
                if (ProfilesMasterdatatable.Rows.Count > 0)
                {
                    svalue = ProfilesMasterdatatable.Rows[0]["Id"].ToString();

                }
                else
                {
                    svalue = "";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return svalue;
        }

		public bool UpdateProfilesunselect(bool isSelected, string strsname)
        {
            bool UpdateRet = false;
            int Update = 0;
            try
            {
                ServerTypes STypebject = new ServerTypes();
                ServerTypes RetnSTypebject = new ServerTypes();
            
                       string  SqlQuery = "UPDATE ProfilesMaster set isSelected='" + isSelected + "' where ID='" + strsname + "'";
                    Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                }
            
            catch
            {
                
            }

            return UpdateRet;
        }
        public bool UpdateProfiles(ProfilesMaster StObject, string strsname)
        {
            bool UpdateRet = false;
            int Update = 0;
            try
            {
                ServerTypes STypebject = new ServerTypes();
                ServerTypes RetnSTypebject = new ServerTypes();
                STypebject.ServerType = StObject.ServerType;
                RetnSTypebject = VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.GetDataForServerType(STypebject);
                StObject.ServerTypeId = RetnSTypebject.ID;
                if (strsname != "")
                {
                    string SqlQuery = "";
                    if (StObject.RoleType == null)
                    {
                        StObject.RoleType = "";
                    }
                    SqlQuery = "UPDATE ProfilesMaster set AttributeName='" + StObject.AttributeName + "',UnitOfMeasurement='" + StObject.UnitOfMeasurement +
						"',isSelected='" + StObject.isSelected + "',RelatedTable='" + StObject.RelatedTable + "',RelatedField='" + StObject.RelatedField + "',DefaultValue='" + StObject.DefaultValue + "',ServerTypeId='" + StObject.ServerTypeId + "',RoleType='" + StObject.RoleType.ToString() + "'where ID='" + strsname + "'";
                    Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                }
            }
            catch
            {
                Update = 0;
            }
            if (Update == 0)
            {
                try
                {
                    string SqlQuery = "";

					SqlQuery = "INSERT INTO ProfilesMaster(ServerTypeId,AttributeName,DefaultValue,UnitOfMeasurement,RelatedTable,RelatedField,RoleType,ProfileId,isSelected) VALUES('"
                            + StObject.ServerTypeId + "','" + StObject.AttributeName + "','" + StObject.DefaultValue + "','" + StObject.UnitOfMeasurement +
						"','" + StObject.RelatedTable + "','" + StObject.RelatedField + "','" + StObject.RoleType.ToString() + "','" + StObject.ProfileId + "','" + StObject.isSelected + "')";
                    
                    Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                }
                catch
                {
                    Update = 0;
                }
            }
            if (Update >= 1)
            {
                UpdateRet = true;
            }
            return UpdateRet;
        }

		//public bool UpdateProfiles(ProfilesMaster StObject, string strsname, string profileid)
		//{
		//    bool UpdateRet = false;
		//    int Update = 0;
		//    try
		//    {
		//        ServerTypes STypebject = new ServerTypes();
		//        ServerTypes RetnSTypebject = new ServerTypes();
		//        STypebject.ServerType = StObject.ServerType;
		//        RetnSTypebject = VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.GetDataForServerType(STypebject);
		//        StObject.ServerTypeId = RetnSTypebject.ID;
		//        if (strsname != "")
		//        {
		//            string SqlQuery = "";
		//            if (StObject.RoleType == null)
		//            {
		//                StObject.RoleType = "";
		//            }
		//            SqlQuery = "UPDATE ProfilesMaster set AttributeName='" + StObject.AttributeName + "',UnitOfMeasurement='" + StObject.UnitOfMeasurement +
		//                "',RelatedTable='" + StObject.RelatedTable + "',RelatedField='" + StObject.RelatedField + "',DefaultValue='" + StObject.DefaultValue + "',ServerTypeId='" + StObject.ServerTypeId + "',RoleType='" + StObject.RoleType.ToString() + "',ProfileId='" + profileid + "'where ID='" + strsname + "'";
		//            Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
		//        }
		//    }
		//    catch
		//    {
		//        Update = 0;
		//    }
		//    if (Update == 0)
		//    {
		//        try
		//        {
		//            string SqlQuery = "";

		//            SqlQuery = "INSERT INTO ProfilesMaster(ServerTypeId,AttributeName,DefaultValue,UnitOfMeasurement,RelatedTable,RelatedField,RoleType,ProfileId) VALUES('"
		//                    + StObject.ServerTypeId + "','" + StObject.AttributeName + "','" + StObject.DefaultValue + "','" + StObject.UnitOfMeasurement +
		//                "','" + StObject.RelatedTable + "','" + StObject.RelatedField + "','" + StObject.RoleType.ToString() + "','" + StObject.ProfileId + "')";

		//            Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
		//        }
		//        catch
		//        {
		//            Update = 0;
		//        }
		//    }
		//    if (Update == 1)
		//    {
		//        UpdateRet = true;
		//    }
		//    return UpdateRet;
		//}

		public bool UpdateeditProfilesisselected(bool isselected, string strsname, string profilemasterid)
		{
			bool UpdateRet = false;
			int Update = 0;
			try
			{
				DataTable dt = new DataTable();
				ServerTypes STypebject = new ServerTypes();
				ServerTypes RetnSTypebject = new ServerTypes();

				string SqlQuery = "UPDATE ProfilesMaster set isSelected='" + isselected + "'where ProfileId='" + strsname + "' and ID='" + profilemasterid + "'";
						Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
					}
				
			
			catch
			{
				
			}
			return UpdateRet;
			
		}
		public bool UpdateeditProfiles(ProfilesMaster StObject, string strsname, string profilemasterid)
		{
			bool UpdateRet = false;
			int Update = 0;
			try
			{
				DataTable dt = new DataTable();
				ServerTypes STypebject = new ServerTypes();
				ServerTypes RetnSTypebject = new ServerTypes();
				STypebject.ServerType = StObject.ServerType;
				RetnSTypebject = VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.GetDataForServerType(STypebject);
				StObject.ServerTypeId = RetnSTypebject.ID;
				if (strsname != "")
				{
					string SqlQuery = "";
					if (StObject.RoleType == null)
					{
						StObject.RoleType = "";
					}
					SqlQuery = "select * from ProfilesMaster where ProfileId='" + strsname + "' and ServerTypeId ='" + StObject.ServerTypeId + "'and AttributeName='"+StObject.AttributeName+"'";
					dt = objAdaptor.FetchData(SqlQuery);
					if (dt.Rows.Count > 0)
					{
						SqlQuery = "UPDATE ProfilesMaster set AttributeName='" + StObject.AttributeName + "',isSelected='" + StObject.isSelected + "',UnitOfMeasurement='" + StObject.UnitOfMeasurement +
							"',RelatedTable='" + StObject.RelatedTable + "',RelatedField='" + StObject.RelatedField + "',DefaultValue='" + StObject.DefaultValue + "',ServerTypeId='" + StObject.ServerTypeId + "',RoleType='" + StObject.RoleType.ToString() + "'where ProfileId='" + strsname + "' and ID='" + profilemasterid + "'";
						Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
					}
				}
			}
			catch
			{
				Update = 0;
			}
			if (Update == 0)
			{
				try
				{
					string SqlQuery = "";

					SqlQuery = "INSERT INTO ProfilesMaster(ServerTypeId,AttributeName,DefaultValue,UnitOfMeasurement,RelatedTable,RelatedField,RoleType,ProfileId,isSelected) VALUES('"
							+ StObject.ServerTypeId + "','" + StObject.AttributeName + "','" + StObject.DefaultValue + "','" + StObject.UnitOfMeasurement +
						"','" + StObject.RelatedTable + "','" + StObject.RelatedField + "','" + StObject.RoleType.ToString() + "','" + strsname + "','" + StObject.isSelected + "')";

					Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
				}
				catch
				{
					Update = 0;
				}
			}
			if (Update >= 1)
			{
				UpdateRet = true;
			}
			return UpdateRet;
		}
        public object DeleteData(ProfilesMaster StObject)
        {
            bool UpdateRet = false;
            int Update = 0;
            try
            {
                string SqlQuery = "delete ProfilesMaster where id=" + StObject.Id ;
                Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
            }
            catch
            {
                Update = 0;
            }

            if (Update == 1)
            {
                UpdateRet = true;
            }
            return UpdateRet;
        }

        public DataTable GetColumns(string TableName)
        {
            DataTable ColumnDT = new DataTable();
            try
            {
                string SqlQuery = "SELECT COLUMN_NAME FROM vitalsigns.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + TableName + "'";
                ColumnDT = objAdaptor.FetchData(SqlQuery);
                
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return ColumnDT;
        }

        public int UpdateRunMode(string runMode)
        {
            int mode = 0;
            try
            {
                int count = 0;
                string Query = "SELECT Count(*) FROM Settings WHERE sname = 'runmode'";
                count = objAdaptor.ExecuteScalar(Query);
                string SqlQuery = "";
                if (count == 0)
                {
                    SqlQuery = "INSERT INTO Settings VALUES('RunMode','" + runMode + "','System.String')";
                }
                else
                {
                    SqlQuery = "UPDATE Settings SET svalue='" + runMode + "' WHERE sname = 'RunMode'";
                }
                
                mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
            }
            catch
            {
            }
            return mode;
        }

        public int UpdateServerNodes(int NodeID, string NodeHostName, string NodeIPAddress, string Description)
        {
            int mode = 0;
            string SqlQuery = "";
            try
            {
                int count = 0;
                string Query = "SELECT * FROM ServerNodes WHERE NodeID =" + NodeID;
                count = objAdaptor.ExecuteScalar(Query);
                int DataUpdate = 0;
                if ( count != NodeID)
                {
                    SqlQuery = "INSERT INTO ServerNodes VALUES (" + NodeID + ",'" + NodeHostName + "','" + NodeIPAddress + "','" + Description + "')";                    
                }
                else
                {
                    SqlQuery = "UPDATE ServerNodes SET NodeHostName='" + NodeHostName + "',NodeIPAddress='" + NodeIPAddress + "',NodeDescription='" + Description + "' WHERE NodeID=" + NodeID;
                }
                mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
            }
            catch
            {
                
            }
            return mode;
        }

        public int UpdateServerMonitoringNodes(int serverID, string serverType, int nodeID)
        {
            int update = 0;
            int update1 = 0;
            int update2 = 0;
            string SqlQuery = "";
            string SqlQuery1 = "";
            string tableName = "";
            switch (serverType.ToLower())
            {
                case "domino":
                    tableName = "DominoServers";
                    break;
                case "bes":
                    tableName = "BlackBerryServers";
                    break;
                case "sametime":
                    tableName = "SametimeServers";
                    break;
                //case "sharepoint":
                //    tableName = "DominoServers";
                //case "exchange":
                //    tableName = "DominoServers";
                //case "mail":
                //    tableName = "DominoServers";
                //case "url":
                //    tableName = "DominoServers";
                //case "network device":
                //    tableName = "DominoServers";
                //case "notes database":
                //    tableName = "DominoServers";
                default:
                    tableName = "DominoServers";
                    break;
            }
            try
            {

                SqlQuery = "UPDATE " + tableName + " SET MonitoredBy=" + nodeID + "WHERE ServerID=" + serverID;
                
                update1 = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
                if(update1 == 1)
                {
                    SqlQuery1 = "UPDATE Servers SET MonitoredBy=" + nodeID + "WHERE ID=" + serverID;
                    update2 = objAdaptor.ExecuteNonQueryRetRows(SqlQuery1);
                }
                if (update1 == 1 && update2 == 1)
                {
                    update = 1;
                }
            }
            catch
            {

            }
            return update;
        }
        public DataTable GetServerNodes()
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT * FROM ServerNodes";
                dt = objAdaptor.FetchData(SqlQuery);
                
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return dt;
        }

		public DataTable GetAllDataByServerType(string ServerType)
		{
			DataTable ProfilesMasterTable = new DataTable();
			ProfilesMaster ReturnObj = new ProfilesMaster();
			try
			{
				string SqlQuery = "SELECT pm.ID,pm.ServerTypeId,pm.AttributeName,pm.DefaultValue,pm.UnitOfMeasurement,pm.RelatedTable," +
					"pm.RelatedField,pm.ProfileId,st.ServerType,'false' as isSelected FROM ProfilesMaster pm,ServerTypes st " +
					"where pm.ServerTypeId=st.ID and ServerType='" + ServerType + "'";
				//,  s.LocationID
				//,Servers s 
				//pm.ID = s.ID and 
				ProfilesMasterTable = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return ProfilesMasterTable;
		}



    }
}
