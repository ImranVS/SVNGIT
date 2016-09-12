using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebBL.ConfiguratorBL
{
    public class ServicesBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static ServicesBL _self = new ServicesBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static ServicesBL Ins
        {
            get { return _self; }
        }

        public DataTable GetAllServices()
        {
			try
			{

			}
			catch (Exception)
			{
				
				throw;
			}
            return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetAllServices();
        }

        /// <summary>
        /// GetAllServicesByServerType, left outer join on services and servers
        /// </summary>
        /// <param name="ServerTypeId"></param>
        /// <returns></returns>
        public DataTable GetAllServicesByServerType(int ServerTypeId)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetAllServicesByServerType(ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           

        }

        /// <summary>
        /// GetAllServicesByServerIdType;left outer join on services and servers
        /// </summary>
        /// <param name="ServerTypeId"></param>
        /// <param name="ServerId"></param>
        /// <returns></returns>
        public DataTable GetAllServicesByServerIdType(int ServerTypeId, int ServerId, string VersionNo)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetAllServicesByServerIdType(ServerTypeId, ServerId, VersionNo);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           

        }
        public DataTable GetVersions()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetVersions();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         

        }
		public DataTable GetCredentials()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetCredentials();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}		

		}
        // 8/7/2016 Durga Addded for VSPLUS-2877
        public DataTable GetCredentialsForSSE()
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetCredentialsForSSE();
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
		public DataTable GetSametimeCredentials()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetSametimeCredentials();
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}
		public DataTable GetExchangeServers()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetExchangeServers();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			

		}
        public DataTable GetSelectedRoles(int ServerTypeId, int ServerId, string VersionNo)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetSelectedRoles(ServerTypeId, ServerId, VersionNo);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           

        }
        public DataTable GetRolesbyName(string ServerName)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetRolesbyName(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            

        }
        public DataTable GetRoles(int ServerId)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetRoles(ServerId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           

        }
        public bool InsertSVRData(List<object> fieldValues, string ServerId)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.InsertSVRData(fieldValues, ServerId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetSelectedVersions(int ServerTypeId, int ServerId)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetSelectedVersions(ServerTypeId, ServerId);
			}
			catch (Exception ex	)
			{
				
				throw ex;
			}
           

        }

        public DataTable GetAllServicesByServerIdTypeAndRoles(int ServerTypeId, int ServerId, string VersionNo, string Roles)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetAllServicesByServerIdTypeAndRoles(ServerTypeId, ServerId, VersionNo, Roles);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          

        }
		public DataTable GetWebsphereserverCredentials(int ServerType)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetWebsphereserverCredentials(ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex	;
			}
			

		}

		public DataTable GetWindowsServices(string servername)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetWindowsServices(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
        public DataTable GetWindowsServices1(string servername)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetWindowsServices1(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		public Boolean UpdateWindowsServices(string servername, List<object> fieldValues)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.UpdateWindowsServices(servername, fieldValues);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetWindowsServicesForSP(string servername)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetWindowsServicesForSP(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetWindowsServicesWS(string servername)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetWindowsServicesWS(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetWindowsServicesWS()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetWindowsServicesWS();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public Boolean UpdateWindowsServicesWS(string servername, List<object> fieldValues)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.UpdateWindowsServicesWS(servername, fieldValues);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

        //14/07/2016 sowmya added for VSPLUS-3097
        public DataTable GetTestNames()
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.ServicesDAL.Ins.GetTestNames();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
