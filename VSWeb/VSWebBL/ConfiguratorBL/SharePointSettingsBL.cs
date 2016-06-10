using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL
{
   public class SharePointSettingsBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
       private static SharePointSettingsBL _self = new SharePointSettingsBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
       public static SharePointSettingsBL Ins
        {
            get { return _self; }
        }
       public SharePointSettings GetData(SharePointSettings StObject)
       {
		   try
		   {
			   return VSWebDAL.SharePointSettingsDAL.Ins.GetData(StObject);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
        
       }
       public DataTable GetAllData()
       {
		   try
		   {
			   return VSWebDAL.SharePointSettingsDAL.Ins.GetAllData();
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }
       public string Getvalue(string Sname)
       {
		   try
		   {
			   return VSWebDAL.SharePointSettingsDAL.Ins.Getvalue(Sname);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }

       public bool UpdateSvalue(int ServerId,string Sname, string Svalue)
	   {
		   try
		   {
			   return VSWebDAL.SharePointSettingsDAL.Ins.UpdateSvalue(Sname, Svalue, ServerId);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }

          
       }

       public bool UpdateSharePointSettings(SharePointSettings StObject, string strSname)
       {
		   try
		   {
			   return VSWebDAL.SharePointSettingsDAL.Ins.UpdateSharePointSettings(StObject, strSname);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }

       public object DeleteData(SharePointSettings StObject)
       {
		   try
		   {
			   return VSWebDAL.SharePointSettingsDAL.Ins.DeleteData(StObject);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }

	   public DataTable GetFarmSettings(string farm)
	   {
		   try
		   {
			   return VSWebDAL.SharePointSettingsDAL.Ins.GetFarmSettings(farm);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   } 
	   }

	   public bool UpdateFarmSettings(SharePointFarmSettings settings)
	   {
		   try
		   {
			   return VSWebDAL.SharePointSettingsDAL.Ins.UpdateFarmSettings(settings);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
		  
	   }
    }
}
