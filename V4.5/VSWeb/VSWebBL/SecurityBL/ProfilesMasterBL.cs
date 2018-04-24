using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.SecurityBL
{
    public class ProfilesMasterBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static ProfilesMasterBL _self = new ProfilesMasterBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static ProfilesMasterBL Ins
        {
            get { return _self; }
        }


        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesMasterDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable GetAllDataByServerType(string ServerType, string RoleType, string ProfileName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesMasterDAL.Ins.GetAllDataByServerType(ServerType, RoleType, ProfileName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }
        public string Getvalue(string AttributeName, string ServerType)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesMasterDAL.Ins.Getvalue(AttributeName, ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

		public bool UpdateProfilesunselect(bool isselected, string strsname)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesMasterDAL.Ins.UpdateProfilesunselect(isselected, strsname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public bool UpdateProfiles(ProfilesMaster StObject, string strsname)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesMasterDAL.Ins.UpdateProfiles(StObject, strsname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		public bool UpdateeditProfilesisselected(bool isselected, string strsname, string profilemasterid)
		{
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesMasterDAL.Ins.UpdateeditProfilesisselected(isselected, strsname, profilemasterid);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public bool UpdateeditProfiles(ProfilesMaster StObject, string strsname,string profilemasterid)
		{
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesMasterDAL.Ins.UpdateeditProfiles(StObject, strsname, profilemasterid);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
        public object DeleteData(ProfilesMaster StObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesMasterDAL.Ins.DeleteData(StObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

		//public DataTable GetAllDataByName(string profilename)
		//{
		//    return VSWebDAL.SecurityDAL.ProfilesMasterDAL.Ins.GetAllDatabyname(profilename);
		//}

        public DataTable GetColumns(string TableName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesMasterDAL.Ins.GetColumns(TableName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public int UpdateRunMode(string runMode)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesMasterDAL.Ins.UpdateRunMode(runMode);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public int UpdateServerNodes(int NodeID, string NodeHostName, string NodeIPAddress, string Description)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesMasterDAL.Ins.UpdateServerNodes(NodeID, NodeHostName, NodeIPAddress, Description);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }
        public int UpdateServerMonitoringNodes(int serverID, string serverType, int nodeID)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesMasterDAL.Ins.UpdateServerMonitoringNodes(serverID, serverType, nodeID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable GetServerNodes()
		{
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesMasterDAL.Ins.GetServerNodes();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
    }
}
