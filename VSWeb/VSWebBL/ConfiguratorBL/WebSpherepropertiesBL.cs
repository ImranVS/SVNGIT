using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDAL;
using VSWebDO;

namespace VSWebBL.ConfiguratorBL
{
	public class WebSpherepropertiesBL
    {
		private static WebSpherepropertiesBL _self = new WebSpherepropertiesBL();
		public static WebSpherepropertiesBL Ins
        {
            get { return _self; }
        }

        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.WebSpherepropertiesDAL.Ins.GetAllData();
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
				return VSWebDAL.ConfiguratorDAL.WebSpherepropertiesDAL.Ins.GetServerNameinwebsphereservers(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		
		}
		public DataTable GetAllDataByNames(WebSpherePropertie ServerObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.WebSpherepropertiesDAL.Ins.GetAllDataByNames(ServerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }

		public Object UpdateData(WebSpherePropertie ServerObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.WebSpherepropertiesDAL.Ins.UpdateData(ServerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		

        }

        public Object UpdatesData(WebSpherePropertie ServerObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.WebSpherepropertiesDAL.Ins.UpdatesData(ServerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public WebSpherePropertie GetData(WebSpherePropertie DominoServersObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.WebSpherepropertiesDAL.Ins.GetData(DominoServersObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }


		public Object InsertData(WebSpherePropertie WebSphereobj)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.WebSpherepropertiesDAL.Ins.InsertData(WebSphereobj);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public Object UpdateDataforservers(Servers ServerObject)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.WebSpherepropertiesDAL.Ins.UpdateDataforservers(ServerObject);	
			}
			catch (Exception ex)
			{
				
				throw ex;
			}

				
		
		}
		public Object InsertDataforservers(Servers ServerObject)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.WebSpherepropertiesDAL.Ins.InsertDataforservers(ServerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}

		

		}
		public DataTable GetDataForServerID(string ServerName)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.WebSpherepropertiesDAL.Ins.GetDataForServerID(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetNodeID(string NodeName)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.WebSpherepropertiesDAL.Ins.GetNodeID(NodeName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

    }
}
