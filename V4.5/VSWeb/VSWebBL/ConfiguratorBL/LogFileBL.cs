using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
    public class LogFileBL
    {

        /// <summary>
        /// Declarations
        /// </summary>
        private static LogFileBL _self = new LogFileBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static LogFileBL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.LogFileDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        /// <summary>
        /// Call to Get Data from Locations based on Primary key
        /// </summary>
        /// <param name="LocObject">Locations object</param>
        /// <returns></returns>
        public LogFile GetData(LogFile LocObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.LogFileDAL.Ins.GetData(LocObject);
			}
			catch (Exception ex)
			{

				throw ex;
			}
          
        }
       
        /// <summary>
        /// Call to Insert Data into Locations
        ///  </summary>
        /// <param name="LogObject">Locations object</param>
        /// <returns></returns>
        public object InsertData(LogFile LogObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.LogFileDAL.Ins.InsertData(LogObject);
			}
			catch (Exception ex)
			{

				throw ex;
			}
            

        }
        /// <summary>
        /// Call to Update Data of DominoServers based on Key
        /// </summary>
        /// <param name="LogObject">DominoServers object</param>
        /// <returns>Object</returns>
        public Object UpdateData(LogFile LogObject)
        {
            //Object ReturnValue = ValidateUpdate(LocObject);
			try
			{
				return VSWebDAL.ConfiguratorDAL.LogFileDAL.Ins.UpdateData(LogObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}

            
        }
        /// <summary>
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="LocObject"></param>
        /// <returns></returns>
        public Object DeleteData(LogFile LogObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.LogFileDAL.Ins.DeleteData(LogObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

    }
}
