using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using VSWebDAL;
using System.Data;

namespace VSWebBL.SecurityBL
{
     public class UserStartupURLBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
		 private static UserStartupURLBL _self = new UserStartupURLBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
		 public static UserStartupURLBL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData(Users u)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UserStartupURLDAL.Ins.GetAllData(u);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
    }
}
