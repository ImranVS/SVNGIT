using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
  public  class WindowsBAL
    {
      private static WindowsBAL _self = new WindowsBAL();
      public static WindowsBAL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.WindowsDAL.Ins.GetAllData();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
		public DataTable GetAllDataByUser(int UserID)
        {
            try
            {
				return VSWebDAL.ConfiguratorDAL.WindowsDAL.Ins.GetAllDataByUser(UserID);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
	  
    }
}
