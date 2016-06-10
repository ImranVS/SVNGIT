using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
   public class DAGBL
    {
        private static DAGBL _self = new DAGBL();
       public static DAGBL Ins
        {
            get { return _self; }
        }
       public Object DagsettingsData(DagSettings ServerObject)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.DAGDAL.Ins.dagdetailsdata2(ServerObject);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          

       }
       public DataTable GetDagSettings(DagSettings Mobj)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.DAGDAL.Ins.GetDagSettings(Mobj);
		   }
		   catch (Exception ex	)
		   {
			   
			   throw ex;
		   }
         
       }
       public DataTable GetAllData()
       {
           try
           {
               return VSWebDAL.ConfiguratorDAL.DAGDAL.Ins.GetAllData();
           }
           catch (Exception ex)
           {

               throw ex;
           }

       }
	   public DataTable GetAttributes(int ServerId)
	   {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.DAGDAL.Ins.GetAttributes(ServerId);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
		  
	   }


	   public DataTable getMailDatabaseSettings(string ServerName)
	   {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.DAGDAL.Ins.getMailDatabaseSettings(ServerName);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
		  
	   }

	   public DataTable getMailDatabaseQueueSettings(string ServerName)
	   {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.DAGDAL.Ins.getMailDatabaseQueueSettings(ServerName);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
		 
	   }

	   public bool InsertSrvDatabaseSettingsData(DataTable dtDatabase, String ServerName)
	   {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.DAGDAL.Ins.InsertSrvDatabaseSettingsData(dtDatabase, ServerName);
		   }
		   catch (Exception ex) 
		   {
			   
			   throw ex;
		   }
		   
	   }
    }
}
