using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
  public  class WebSphereServerGridBAL
    {
      private static WebSphereServerGridBAL _self = new WebSphereServerGridBAL();
      public static WebSphereServerGridBAL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.WebSphereServerGridDAL.Ins.GetAllData();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

		public DataTable GetAllDatabyUserrestrictions(int UserID)
        {
            try
            {
				return VSWebDAL.ConfiguratorDAL.WebSphereServerGridDAL.Ins.GetAllDatabyUserrestrictions(UserID);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
		public DataTable GetAllDataForSametimeServers()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.WebSphereServerGridDAL.Ins.GetAllDataForSametimeServers();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
    }
}
