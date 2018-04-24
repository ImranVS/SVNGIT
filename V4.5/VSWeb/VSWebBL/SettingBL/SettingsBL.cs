using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.SettingBL
{
   public class SettingsBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
       private static SettingsBL _self = new SettingsBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
       public static SettingsBL Ins
        {
            get { return _self; }
        }
       public Settings GetData(Settings StObject)
       {
		   try
		   {
			   return VSWebDAL.SettingDAL.SettingsDAL.Ins.GetData(StObject);
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
			   return VSWebDAL.SettingDAL.SettingsDAL.Ins.GetAllData();
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
			   return VSWebDAL.SettingDAL.SettingsDAL.Ins.Getvalue(Sname);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }
	   public bool UpdateScanvalue(string sname, string svalue, string stype)
	   {
		   try
		   {
			   return VSWebDAL.SettingDAL.SettingsDAL.Ins.UpdateScanvalue(sname, svalue, stype);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
		   
	   }
	   public bool UpdateScanFirstvalue(string sname, string svalue, string stype)
	   {
		   try
		   {
			   return VSWebDAL.SettingDAL.SettingsDAL.Ins.UpdateScanFirstvalue(sname, svalue, stype);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
		   
	   }
	   public bool Getcheckvalue(string Sname)
	   {
		   try
		   {
			   return VSWebDAL.SettingDAL.SettingsDAL.Ins.Getcheckvalue(Sname);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
		   
	   }
       public bool UpdateSvalue(string sname, string svalue, string stype)
       {
		   try
		   {
			   return VSWebDAL.SettingDAL.SettingsDAL.Ins.UpdateSvalue(sname, svalue, stype);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
           
       }

       public bool UpdateSettings(Settings StObject, string strsname)
       {
		   try
		   {
			   return VSWebDAL.SettingDAL.SettingsDAL.Ins.UpdateSettings(StObject, strsname);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
           
       }

       public object DeleteData(Settings StObject)
       {
		   try
		   {
			   return VSWebDAL.SettingDAL.SettingsDAL.Ins.DeleteData(StObject);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }
    }
}
