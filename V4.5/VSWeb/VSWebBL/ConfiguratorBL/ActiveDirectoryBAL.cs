using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
  public  class ActiveDirectoryBAL
    {
      private static ActiveDirectoryBAL _self = new ActiveDirectoryBAL();
      public static ActiveDirectoryBAL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.ActiveDirectoryDAL.Ins.GetAllData();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
		public DataTable GetAllDatabyUser( int UserID)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ActiveDirectoryDAL.Ins.GetAllDatabyUser(UserID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
    }
}
