using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDAL;
using VSWebDO;

namespace VSWebBL.SecurityBL
{
   public class ServersBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
       private static ServersBL _self = new ServersBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
       public static ServersBL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData(int? userId=null)
        {
			try
			{
                return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetAllData(userId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public DataTable GetWebSphereTableData(SametimeServers stsObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetWebSphereTableData(stsObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //public DataTable GetAllData()
        //{
        //    return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetAllData();
        //}
        #region "Validations"

        /// <summary>
        /// Validation before submitting data for Server Grid
        /// </summary>
        /// <param name="ServerObject"></param>
        /// <returns></returns>
        public Object ValidateUpdate(Servers ServerObject)
        {
            Object ReturnValue = "";
            try
            {
                if (ServerObject.ServerName== null || ServerObject.ServerName == "")
                {
                    return "ER#Please enter the Server name";
                }
                if (ServerObject.ServerTypeID != 18)
                {
                    if (ServerObject.IPAddress == null || ServerObject.IPAddress == "")
                    {
                        return "#ERPlease enter the IPAddress";
                    }
                }
            }
            catch (Exception ex )
            { throw ex ; }
            finally
            { }
            return "";
        }
        //public Object ValidateUpdate1(Servers ServerObject)
        //{
        //    Object ReturnValue = "";
        //    try
        //    {
        //        if (ServerObject.ServerName == null || ServerObject.ServerName == "")
        //        {
        //            return "ER#Please enter the Server name";
        //        }
        //        if (ServerObject.ServerTypeID != 18)
        //        {
        //            if (ServerObject.IPAddress == null || ServerObject.IPAddress == "")
        //            {
        //                return "#ERPlease enter the IPAddress";
        //            }
        //        }
        //    }
        //    catch (Exception ex ex)
        //    { throw ex ex; }
        //    finally
        //    { }
        //    return "";
        //}
        #endregion

        /// <summary>
        /// Call to Get Data from Servers based on Primary key
        /// </summary>
        /// <param name="ServerObject">Servers object</param>
        /// <returns></returns>
        //public Servers GetData(Servers ServerObject)
        //{
        //    return VSWebDAL.ConfiguratorDAL.ServersDAL.Ins.GetData(ServerObject);
        //}

        /// <summary>
        /// Call to Insert Data into Locations
        ///  </summary>
        /// <param name="ServerObject">Locations object</param>
        /// <returns></returns>
        public object InsertData(Servers ServerObject)
        {
			try
			{
				Object ReturnValue = ValidateUpdate(ServerObject);
				DataTable dt = VSWebDAL.SecurityDAL.ServersDAL.Ins.GetDataByName(ServerObject);
				if (dt.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return VSWebDAL.SecurityDAL.ServersDAL.Ins.InsertData(ServerObject);
					}
					else return ReturnValue;
				}
				else return "Servername Already Exists.";
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            

        }
        


        /// Call to Update Data of DominoServers based on Key
        /// </summary>
        /// <param name="LocObject">DominoServers object</param>
        /// <returns>Object</returns>
        public Object UpdateData(Servers ServerObject)
        {
			try
			{
				Object ReturnValue = ValidateUpdate(ServerObject);
				DataTable dt = VSWebDAL.SecurityDAL.ServersDAL.Ins.GetDataByName(ServerObject);
				if (dt.Rows.Count == 0)
				{

					if (ReturnValue.ToString() == "")
					{
						return VSWebDAL.SecurityDAL.ServersDAL.Ins.UpdateData(ServerObject);
					}

					else return ReturnValue;
				}
				else return "Servername Already Exists.";
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            

        }
        //public Object UpdateData(Servers ServerObject)
        //{
        //    Object ReturnValue = ValidateUpdate(ServerObject);
        //    DataTable dt = VSWebDAL.SecurityDAL.ServersDAL.Ins.GetDataByName(ServerObject);
        //    if (dt.Rows.Count == 0)
        //    {

        //        if (ReturnValue.ToString() == "")
        //        {
        //            return VSWebDAL.SecurityDAL.ServersDAL.Ins.UpdateData(ServerObject);
        //        }

        //        else return ReturnValue;
        //    }
        //    else return "Servername Already Exists.";

        //}
        /// <summary>
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="ServersObject"></param>
        /// <returns></returns>
        public Object DeleteData(Servers ServersObject)
        {
			try
			{
				VSWebDAL.SecurityDAL.ServersDAL.Ins.DeleteServerDependencies(ServersObject);
				
				//WS Added to check if there are any issues with deleting the server
				Object obj = VSWebDAL.SecurityDAL.ServersDAL.Ins.SafeToDeleteServer(ServersObject);
				if (obj is String)
					return obj;

				return VSWebDAL.SecurityDAL.ServersDAL.Ins.DeleteData(ServersObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }
        //public Object DeleteData1(WebSphereNodes ServersObject)
        //{
        //    VSWebDAL.SecurityDAL.ServersDAL.Ins.DeleteServerDependencies1(ServersObject);
        //    return VSWebDAL.SecurityDAL.ServersDAL.Ins.DeleteData1(ServersObject);
        //}

        public DataTable GetDataByName(Servers ServersObject)
		{
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetDataByName(ServersObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable GetAllDataByServerType(string ServerType)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetAllDataByServerType(ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        
        }
        public Int32 GetServerIDbyServerName(string serverName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetServerIDbyServerName(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
		public Int32 GetO365ServerIDbyServerName(string serverName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetO365ServerIDbyServerName(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
	   

		public Int32 GetCloudIDbyDeviceName(string DeviceName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetCloudIDbyDeviceName(DeviceName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable GetServerDetailsByName(string serverName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetServerDetailsByName(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable GetServerDetailsByID(int ID)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetServerDetailsByID(ID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		public DataTable GetURLDetailsByID(int ID)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetURLDetailsByID(ID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable GetCloudDetailsByID(int ID)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetCloudDetailsByID(ID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
	   
		public DataTable GetServerDetailsByName_Mail(string serverName)
		{
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetServerDetailsByName_Mail(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		
		}

       /// <summary>
        ///  Call to Update Server Data 
       /// </summary>
       /// <param name="ServerObject"></param>
       /// <returns></returns>
        public Object UpdateAttributesData(ServerAttributes ServerObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.UpdateAttributesData(ServerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          

        }
        //public Object UpdateAttributesData(ServerAttributes ServerObject)
        //{
        //    return VSWebDAL.SecurityDAL.ServersDAL.Ins.UpdateAttributesData(ServerObject);

        //}

		public Object InsertWebspherAttreibuteData(ServerAttributes ServerObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.InsertWebspherAttreibuteData(ServerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		

        }
        public DataTable GetAllDataByName(Servers ServersObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetAllDataByName(ServersObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public bool UpdateServerLocation(int serverId, int locationId,string servertype)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.UpdateServerLocation(serverId, locationId,servertype);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
		public bool UpdateServerBusinessHours(int serverId, int BusinesshoursID)
		{
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.UpdateServerBusinessHours(serverId, BusinesshoursID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
        public Int32 GetServerIDbyServerNameType(string serverName, string serverType)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetServerIDbyServerNameType(serverName, serverType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public bool UpdateServerCredentials(int serverId, int credentialsID, string ServerType)
        {
            try
            {
                return VSWebDAL.SecurityDAL.ServersDAL.Ins.UpdateServerCredentials(serverId, credentialsID, ServerType);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //Mukund 10Sep14 VE-70
        public DataTable GetServerDetailsByName1(string serverName)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetServerDetailsByName1(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetAllDataByType(string servertype)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetServerDetailsBytype(servertype);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
        }

		public DataTable GetServerNameinwebsphereservers(string Name)
		{
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetServerNameinwebsphereservers(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		
		}
		public DataTable GetServerIdinserverattribute(int id)
		{
			try
			{
				return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetServerIdinserverattribute(id);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public Object UpdateServersProfileName(ProfileNames ProfileNamesObj)
		{
			try
			{

				return VSWebDAL.SecurityDAL.ServersDAL.Ins.UpdateServersProfileName(ProfileNamesObj);
				
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}

        //5/3/2016 Sowjanya added for VSPLUS-2896
        public DataTable GetServerDetailsByWSName(string serverName)
        {
            try
            {
                return VSWebDAL.SecurityDAL.ServersDAL.Ins.GetServerDetailsByWSName(serverName);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
