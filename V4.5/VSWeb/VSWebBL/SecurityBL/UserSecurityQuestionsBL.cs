using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using VSWebDAL;
using System.Data;

namespace VSWebBL.SecurityBL
{
     public class UserSecurityQuestionsBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
         private static UserSecurityQuestionsBL _self = new UserSecurityQuestionsBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
         public static UserSecurityQuestionsBL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.SecurityDAL.UserSecurityQuestionDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetFilepathData(string loginname)
        {
			try
			{
				return VSWebDAL.SecurityDAL.UserSecurityQuestionDAL.Ins.Getimagepath(loginname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
         
    }
}
