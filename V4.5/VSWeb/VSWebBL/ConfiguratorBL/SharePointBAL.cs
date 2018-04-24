using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
  public  class SharePointBAL
    {
      private static SharePointBAL _self = new SharePointBAL();
      public static SharePointBAL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.SharepointDAL.Ins.GetAllData();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

		public DataTable GetAllDataByUserID(int userid)
        {
            try
            {
				return VSWebDAL.ConfiguratorDAL.SharepointDAL.Ins.GetAllDataByUserID(userid);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
		public DataTable GetAllFarmData()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.SharepointDAL.Ins.GetAllFarmData();
			}
			catch (Exception)
			{

				throw;
			}

		}

    }
}
