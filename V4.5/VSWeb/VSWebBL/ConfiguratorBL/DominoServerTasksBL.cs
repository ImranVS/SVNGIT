using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDAL;
using VSWebDO;
using System.Data;

namespace VSWebBL.ConfiguratorBL
{
    public class DominoServerTasksBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static DominoServerTasksBL _self = new DominoServerTasksBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static DominoServerTasksBL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// GET ExchnageServicesData
        /// </summary>
        /// <returns></returns>
        public DataTable GetExchangeServiceData(string roleType)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerTasksDAL.Ins.GetExchangeServiceData(roleType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        /// <summary>
        ///Call DAL GetAllData
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerTasksDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        #region Validations
        public Object ValidateDCUpdate(DominoServerTasks DSTObject)
        {
            Object ReturnValue = "";
            try
            {
                if (DSTObject.TaskName == null || DSTObject.TaskName == "")
                {
                    return "ER#Please enter a short, unique, name for this Server Task.";
                }
                if ((DSTObject.MaxBusyTime)<1 && (DSTObject.FreezeDetect)==true)
                {
                    return "ER#Please enter the maximum time to wait for the server task to update, in minutes.";
                }
                if (DSTObject.RetryCount<1)
                {

                    return "ER#Please enter a Retry Interval that is a positive number.";
                }
               
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { }
            return "";
        }

        #endregion


        /// <summary>
        /// Call DAL GetDataForID
        /// </summary>
        /// <param name="DSTasksObject"></param>
        /// <returns></returns>
        public DominoServerTasks GetDataForID(DominoServerTasks DSTasksObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerTasksDAL.Ins.GetDataForID(DSTasksObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        /// <summary>
        /// Call DAL GetDataForTaskName
        /// </summary>
        /// <param name="DSTasksObject"></param>
        /// <returns></returns>
        public DominoServerTasks GetDataForTaskName(DominoServerTasks DSTasksObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerTasksDAL.Ins.GetDataForTaskName(DSTasksObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        /// <summary>
        /// Call to Insert Data into DominoServerTasks
        ///  </summary>
        /// <param name="DSTObject">DominoCluster object</param>
        /// <returns></returns>


        public object InsertData(DominoServerTasks DSTObject)
        {
			try
			{
				Object ReturnValue = ValidateDCUpdate(DSTObject);
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.DominoServerTasksDAL.Ins.InsertData(DSTObject);
				}
				else return ReturnValue;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}     
        }

        /// <summary>
        /// Call to Update Data of DominoServerTasks based on Key
        /// </summary>
        /// <param name="DominoServerTasks">DominoServerTasks object</param>
        /// <returns>Object</returns>
        public Object UpdateData(DominoServerTasks DSTObject)
        {
			try
			{
				Object ReturnValue = ValidateDCUpdate(DSTObject);
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.DominoServerTasksDAL.Ins.UpdateData(DSTObject);
				}
				else return ReturnValue;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        /// <summary>
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="DSTObject"></param>
        /// <returns></returns>
        public Object DeleteData(DominoServerTasks DSTObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerTasksDAL.Ins.DeleteData(DSTObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        //public DataTable TaskName()
        //{

        //    return VSWebDAL.ConfiguratorDAL.DominoServerTasksDAL.Ins.GetTaskName();
        //}
        public DataTable GetName(DominoServerTasks NDObj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerTasksDAL.Ins.GetName(NDObj);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public int UpdateDominoServerTasks(int TaskID, int ServerID, int Enabled,int SendLoadCommand, int SendRestartCommand, int RestartOffHours, int SendExitCommand)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerTasksDAL.Ins.UpdateDominoServerTasks(TaskID, ServerID, Enabled, SendLoadCommand, SendRestartCommand, RestartOffHours, SendExitCommand);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            // return
            //int ID = VSWebBL.SecurityBL.ServersBL.Ins.GetServerIDbyServerName(serverId);
            
        }

        public DataTable GetDisksData()
        {
			try
			{
				  return VSWebDAL.ConfiguratorDAL.DominoServerTasksDAL.Ins.GetDisksData();
        
			}
			catch (Exception)
			{
				
				throw;
			}
		}  
    }
}
