using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
    public class NetworkDevicesBL
    {

        /// <summary>
        /// Declarations
        /// </summary>
        private static NetworkDevicesBL _self = new NetworkDevicesBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static NetworkDevicesBL Ins
        {
            get { return _self; }
        }
        #region "Validations"

        /// <summary>
        /// Validation before submitting data for Server tab
        /// </summary>
        /// <param name="NetworkDevicesObject"></param>
        /// <returns></returns>
        public Object ValidateDCUpdate(NetworkDevices NetworkDevicesObject)
        {
            Object ReturnValue = "";
            try
            {
                if (NetworkDevicesObject.Name == null || NetworkDevicesObject.Name == "")
                {
                    return "ER#Please enter a name";
                }
                if (NetworkDevicesObject.Location== null || NetworkDevicesObject.Location== " ")
                {
                    return "ER#Please enter the location of the device, such as '8th floor server room";
                }
                if (NetworkDevicesObject.Description== null || NetworkDevicesObject.Description== " ")
                {

                    return "ER#Please enter a description of the device, such as 'Objectionable Content Filter'";
                }
               
                if (NetworkDevicesObject.Category == null || NetworkDevicesObject.Category == " ")
                {
                    return "ER#Please enter Category";
                }
                if (NetworkDevicesObject.ScanningInterval.ToString() == "")
                {
                    return "ER#Please a scan Interval";
                }
               
                if (NetworkDevicesObject.OffHoursScanInterval.ToString() == " ")
                {
                    return "ER#Please enter an off-hours Scan Interval";
                }

                if (NetworkDevicesObject.ResponseThreshold.ToString()=="")
                {
                    return "ER#Please enter a Response Threshold, in milliseconds, over which the device will be considered 'slow'";
                }
                if (NetworkDevicesObject.RetryInterval == null)
                {
                    return "ER#Please enter a Retry Interval, to be used when the device is down.";
                }
                if (NetworkDevicesObject.Address == null)
                {

                    return "ER#Please enter the IP Address device, such as '127.0.0.1' or host name";
                }
				//if ((NetworkDevicesObject.RetryInterval) > (NetworkDevicesObject.ScanningInterval))
				//{
				//    return "ER#Please enter a Retry Interval that is less than the Scan Interval.";
                
				//}
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { }
            return "";
        }

        #endregion

        /// <summary>
        /// Call to Get Data from NetworkDevices based on Primary key
        /// </summary>
        /// <param name="NetworkDevicesObject">NetworkDevicesObject object</param>
        /// <returns></returns>
        public NetworkDevices GetData(NetworkDevices NetworkDevicesObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkDevicesDAL.Ins.GetData(NetworkDevicesObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        /// <summary>
        /// Call to Get Data from DominoServers
        /// </summary>
        /// <returns></returns>
		/// 
		public DataTable IsNetworkdeviceSelected()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.UserPreferencesDAL.Ins.IsNetworkdeviceSelected();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkDevicesDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        /// <summary>
        /// Call to Insert Data into NetworkDevices
        ///  </summary>
        /// <param name="NetworkDevicesObject">NetworkDevices object</param>
        /// <returns></returns>
        public Object InsertData(NetworkDevices NetworkDevicesObject)
        {
            Object ReturnValue = ValidateDCUpdate(NetworkDevicesObject);
			try
			{
            if (ReturnValue.ToString() == "")
            {
                return VSWebDAL.ConfiguratorDAL.NetworkDevicesDAL.Ins.InsertData(NetworkDevicesObject);

            }
			else return ReturnValue;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
            
        }

        /// <summary>
        /// Call to Update Data of DominoServers based on Key
        /// </summary>
        /// <param name="NetworkDevicesObject">DominoServers object</param>
        /// <returns>Object</returns>
        public Object UpdateData(NetworkDevices NetworkDevicesObject)
        {
			Object ReturnValue = ValidateDCUpdate(NetworkDevicesObject);
			try
			{
				
            if (ReturnValue.ToString() == "")
            {
                return VSWebDAL.ConfiguratorDAL.NetworkDevicesDAL.Ins.UpdateData(NetworkDevicesObject);
            }
			else return ReturnValue;
			}
			catch (Exception)
			{
				
				throw;
			}    
        }
        /// <summary>
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="DCObject"></param>
        /// <returns></returns>
        public Object DeleteData(NetworkDevices DCObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkDevicesDAL.Ins.DeleteData(DCObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		public DataTable GetNetworkdevicedData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkDevicesDAL.Ins.GetNetworkdevicedData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		
            
        }

		public DataTable GetNetworkdevicevisibleData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkDevicesDAL.Ins.GetNetworkdevicevisibleData();
            
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }

		public DataTable GetNetworkDatavisiblefordashboard()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkDevicesDAL.Ins.GetNetworkdevicevisibleDatadashboard();
            
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }
        public DataTable GetIPAddress(NetworkDevices NDObj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkDevicesDAL.Ins.GetIPAddress(NDObj);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }
        public DataTable GetLocation()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkDevicesDAL.Ins.GetLocation();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public Int32 GetServerIDbyServerName(string serverName)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NetworkDevicesDAL.Ins.GetServerIDbyServerName(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        //9/14/2015 NS added for VSPLUS-2148
        public DataTable GetDataByID(string id)
        {
            return VSWebDAL.ConfiguratorDAL.NetworkDevicesDAL.Ins.GetDataByID(id);
        }

        //06/05/2016 sowmya added for VPLUS-2902
        public string GetNetworktype(string serverName)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.NetworkDevicesDAL.Ins.GetNetworktype(serverName);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
