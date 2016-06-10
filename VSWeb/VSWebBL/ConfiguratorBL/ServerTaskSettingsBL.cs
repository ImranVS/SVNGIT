using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using VSWebDAL;
using System.Data;

namespace VSWebBL.ConfiguratorBL
{
    public class ServerTaskSettingsBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static ServerTaskSettingsBL _self = new ServerTaskSettingsBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static ServerTaskSettingsBL Ins
        {
            get { return _self; }
        }

        /// <summary>
        /// Call DAL Updata Data
        /// </summary>
        /// <param name="STSettingsObject"></param>
        /// <returns></returns>
        public Object UpdateData(ServerTaskSettings STSettingsObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServerTaskSettingsDAL.Ins.UpdateData(STSettingsObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			} 
        }
        //public Object UpdateData1(WebSphereNodes STSettingsObject)
        //{
        //    return VSWebDAL.ConfiguratorDAL.ServerTaskSettingsDAL.Ins.UpdateData1(STSettingsObject);
        //}


        /// <summary>
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="STSettingsObject"></param>
        /// <returns></returns>
        public Object DeleteData(ServerTaskSettings STSettingsObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServerTaskSettingsDAL.Ins.DeleteData(STSettingsObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public Object DeleteData1(WebSphereNodes STSettingsObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServerTaskSettingsDAL.Ins.DeleteData1(STSettingsObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        /// <summary>
        /// Call DAL Insert Data
        /// </summary>
        /// <param name="STSettingsObject"></param>
        /// <returns></returns>
        public Object InsertData(ServerTaskSettings STSettingsObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServerTaskSettingsDAL.Ins.InsertData(STSettingsObject);
			}
			catch (Exception ex)
			{
				
				throw ex; 
			}
            
        }
        //public Object InsertData1(WebSphereNodes STSettingsObject)
        //{
        //    return VSWebDAL.ConfiguratorDAL.ServerTaskSettingsDAL.Ins.InsertData1(STSettingsObject);
        //}

        public DataTable SelectData(string serverid, string taskid)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ServerTaskSettingsDAL.Ins.SelectData(serverid, taskid);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
    }
}
