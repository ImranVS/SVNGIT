using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.ConfiguratorBL
{
    public class PwrshelBL
    {
        private static PwrshelBL _self = new PwrshelBL();
        public static PwrshelBL Ins
        {
            get
            {
                return _self;
            }

        }

        public Object ValidateUpdate(PowershellScripts pwrObject)
        {
            Object ReturnValue = "";
            try
            {
                if (pwrObject.ScriptName == null || pwrObject.ScriptName == "")
                {
                    return "ER#Please enter the Script name";
                }
                if (pwrObject.ScriptDetails == null || pwrObject.ScriptDetails== "")
                {
                    return "ER#Please enter Script Details";
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { }
            return "";
        }


        public DataTable fillComboBL()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.PwrShelDAL.Ins.fillcombo();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }


           public DataTable GetPwrData()
           {
			   try
			   {
				   return VSWebDAL.ConfiguratorDAL.PwrShelDAL.Ins.GetPwrData();
			   }
			   catch (Exception ex)
			   {
				   
				   throw ex;
			   }
              
           }
           public Object DeleteData(PowershellScripts pwrObject)
           {
			   try
			   {
				   return VSWebDAL.ConfiguratorDAL.PwrShelDAL.Ins.DeleteData(pwrObject);
			   }
			   catch (Exception ex)
			   {
				   
				   throw ex;
			   }
            
           }
           public PowershellScripts GetPwrData(PowershellScripts pwrObject)
           {
			   try
			   {
				   return VSWebDAL.ConfiguratorDAL.PwrShelDAL.Ins.GetPwrData(pwrObject);
			   }
			   catch (Exception ex)
			   {
				   
				   throw ex;
			   }
             
           }
           public Object UpdateScript(PowershellScripts pwrObject)
           { 
               Object ReturnValue = ValidateUpdate(pwrObject);
			   try
			   {
				   if (ReturnValue.ToString() == "")
				   {
					   return VSWebDAL.ConfiguratorDAL.PwrShelDAL.Ins.UpdateData(pwrObject);
				   }
				   else return ReturnValue;
			   }
			   catch (Exception ex)
			   {
				   
				   throw ex;
			   }
              
           }
           public Object InsertScript(PowershellScripts pwrObject)
           {
               Object ReturnValue = ValidateUpdate(pwrObject);
			   try
			   {
				   if (ReturnValue.ToString() == "")
				   {
					   return VSWebDAL.ConfiguratorDAL.PwrShelDAL.Ins.InsertData(pwrObject);
				   }
				   else return ReturnValue;
			   }
			   catch (Exception ex)
			   {
				   
				   throw ex;
			   }
                
           }

           public DataTable GetScriptName(string ScriptName)
           {
			   try
			   {
				   return VSWebDAL.ConfiguratorDAL.PwrShelDAL.Ins.GetScriptName(ScriptName);
			   }
			   catch (Exception)
			   {
				   
				   throw;
			   }
              
           }
    }
}
