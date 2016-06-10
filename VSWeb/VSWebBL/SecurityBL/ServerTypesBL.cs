using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.SecurityBL
{
   public class ServerTypesBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
       private static ServerTypesBL _self = new ServerTypesBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
       public static ServerTypesBL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		public DataTable GetAllDatawithprofileid( int profilevalue)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.GetAllDatawithprofileid(profilevalue);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetExchangeRoles()
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.GetExchangeRoles();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		public DataTable GetExchangeRoleswithprofile(int servertypeId)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.GetExchangeRoleswithprofile(servertypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public ServerTypes GetDataForServerType(ServerTypes STypeObject)
        {
			try
			{
				return VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.GetDataForServerType(STypeObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        public Object insertforservertypes(ServerTypes StypeObject)
        {
			try
			{
				DataTable value = VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.GetDataForServerTypeByname(StypeObject);
				if (value.Rows.Count == 0)
				{
					return VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.insertServertypes(StypeObject);
				}
				else return "";
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

       public Object updateforservertype(ServerTypes StypeObject)
       {
		   try
		   {
			   DataTable value = VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.GetDataForServerTypeByname(StypeObject);
			   if (value.Rows.Count == 0)
			   {
				   return VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.updateServertype(StypeObject);
			   }
			   else return "";
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
           
       }

       public Object deleteforServertype(ServerTypes StypeObject)
       {
		   try
		   {
			   return VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.deleteservertype(StypeObject);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }

       //MD 06-Jan-14
       public DataTable GetSpecificServerTypes()
       {
		   try
		   {
			   return VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.GetSpecificServerTypes();
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
         
       }
       //2/29/2016 Durga Modified for VSPLUS-2668
       public DataTable GetSpecifiServertypesforSSE(int profilevalue, string Page, string control)
       {
           try
           {
               return VSWebDAL.SecurityDAL.ServerTypesDAL.Ins.GetSpecifiServertypesforSSE(profilevalue,Page,control);
           }
           catch (Exception ex)
           {

               throw ex;
           }

       }
    }
}
