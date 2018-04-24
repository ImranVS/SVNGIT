using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDAL;
using VSWebDO;

namespace VSWebBL.ConfiguratorBL
{
    public class LyncBL
    {
        private static LyncBL _self = new LyncBL();
        public static LyncBL Ins
        {
            get { return _self; }
        }

        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.LyncDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		public DataTable GetAllDataByUser(int userid)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.LyncDAL.Ins.GetAllDataByUser(userid);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetAllDataByName(Servers ServerObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.LyncDAL.Ins.GetAllDataByName(ServerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public Object UpdateData(LyncServers ServerObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.LyncDAL.Ins.UpdateData(ServerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
    }
}
