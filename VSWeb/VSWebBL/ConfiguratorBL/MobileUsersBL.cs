using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDAL;
using VSWebDO;
using System.Data;

namespace VSWebBL.ConfiguratorBL
{
    public class MobileUsersBL
    {

        /// <summary>
        /// Declarations
        /// </summary>
		private static MobileUsersBL _self = new MobileUsersBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
		public static MobileUsersBL Ins
        {
            get { return _self; }
        }
		/// <summary>
		/// Call to Insert Data into DominoServerTasks
		///  </summary>
		/// <param name="DSTObject">DominoCluster object</param>
		/// <returns></returns>

		public object InsertData(string DeviceId,int duration)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.MobileUsersDAL.Ins.InsertData(DeviceId, duration);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
				
		}
		public object UpdateDataforDashboard(string DeviceId, int duration)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.MobileUsersDAL.Ins.UpdateDataforDashboard(DeviceId, duration);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public object DeleteThresholdData(string DeviceId)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.MobileUsersDAL.Ins.DeleteData(DeviceId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable SetGrid()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.MobileUsersDAL.Ins.SetGrid();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable SetThresholdGrid()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.MobileUsersDAL.Ins.SetThresholdGrid();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		
		}
        public object UpdateData(string DeviceId, int syncDuration)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MobileUsersDAL.Ins.UpdateData(DeviceId, syncDuration);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		public DataTable GetDeviceID(string id)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.MobileUsersDAL.Ins.GetDeviceID(id);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
        
    }
}
