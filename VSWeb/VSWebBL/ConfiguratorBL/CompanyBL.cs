using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDAL;
using VSWebDO;

namespace VSWebBL.ConfiguratorBL
{
   public class CompanyBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
       private static CompanyBL _self = new CompanyBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
       public static CompanyBL Ins
        {
            get { return _self; }
        }
       public Object UpdateLogo(Company CObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.CompanyDAL.Ins.UpdateLogo(CObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}  
        }

       public Company GetLogo()
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.CompanyDAL.Ins.GetLogo();
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }

         

       }
    }
}
