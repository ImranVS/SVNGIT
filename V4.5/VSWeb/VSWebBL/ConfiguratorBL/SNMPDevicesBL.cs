using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
    public class SNMPDevicesBL
    {

        /// <summary>
        /// Declarations
        /// </summary>
        private static SNMPDevicesBL _self = new SNMPDevicesBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static SNMPDevicesBL Ins
        {
            get { return _self; }
        }
        #region "Validations"

        /// <summary>
        /// Validation before submitting data for Server tab
        /// </summary>
        /// <param name="SNMPDevicesObject"></param>
        /// <returns></returns>
        public Object ValidateDCUpdate(SNMPDevices SNMPDevicesObject)
        {
            Object ReturnValue = "";
            try
            {
                if (SNMPDevicesObject.Name == null || SNMPDevicesObject.Name == "")
                {
                    return "ER#Please enter a name";
                }
                if (SNMPDevicesObject.Location == null || SNMPDevicesObject.Location == " ")
                {
                    return "ER#Please enter the location of the device, such as '8th floor server room";
                }
                if (SNMPDevicesObject.Description == null || SNMPDevicesObject.Description == " ")
                {

                    return "ER#Please enter a description of the device, such as 'Objectionable Content Filter'";
                }

                if (SNMPDevicesObject.Category == null || SNMPDevicesObject.Category == " ")
                {
                    return "ER#Please enter Category";
                }
                if (SNMPDevicesObject.ScanningInterval.ToString() == "")
                {
                    return "ER#Please a scan Interval";
                }

                if (SNMPDevicesObject.OffHoursScanInterval.ToString() == " ")
                {
                    return "ER#Please enter an off-hours Scan Interval";
                }

                if (SNMPDevicesObject.ResponseThreshold.ToString() == "")
                {
                    return "ER#Please enter a Response Threshold, in milliseconds, over which the device will be considered 'slow'";
                }
                if (SNMPDevicesObject.RetryInterval == null)
                {
                    return "ER#Please enter a Retry Interval, to be used when the device is down.";
                }
                if (SNMPDevicesObject.Address == null)
                {

                    return "ER#Please enter the IP Address device, such as '127.0.0.1' or host name";
                }
                if ((SNMPDevicesObject.RetryInterval) > (SNMPDevicesObject.ScanningInterval))
                {
                    return "ER#Please enter a Retry Interval that is less than the Scan Interval.";
                
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
        /// Call to Get Data from SNMPDevices based on Primary key
        /// </summary>
        /// <param name="SNMPDevicesObject">SNMPDevicesObject object</param>
        /// <returns></returns>
        public SNMPDevices GetData(SNMPDevices SNMPDevicesObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.SNMPDevicesDAL.Ins.GetData(SNMPDevicesObject);
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
		public DataTable GetSNMPdevicedData()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.SNMPDevicesDAL.Ins.GetSNMPdevicedData();
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
				return VSWebDAL.ConfiguratorDAL.SNMPDevicesDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        /// <summary>
        /// Call to Insert Data into SNMPDevices
        ///  </summary>
        /// <param name="SNMPDevicesObject">SNMPDevices object</param>
        /// <returns></returns>
        public Object InsertData(SNMPDevices SNMPDevicesObject)
        {
            Object ReturnValue = ValidateDCUpdate(SNMPDevicesObject);
			try
			{
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.SNMPDevicesDAL.Ins.InsertData(SNMPDevicesObject);

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
        /// <param name="SNMPDevicesObject">DominoServers object</param>
        /// <returns>Object</returns>
        public Object UpdateData(SNMPDevices SNMPDevicesObject)
        {
            Object ReturnValue = ValidateDCUpdate(SNMPDevicesObject);
			try
			{
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.SNMPDevicesDAL.Ins.UpdateData(SNMPDevicesObject);
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
        /// <param name="DCObject"></param>
        /// <returns></returns>
        public Object DeleteData(SNMPDevices DCObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.SNMPDevicesDAL.Ins.DeleteData(DCObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }


        public DataTable GetIPAddress(SNMPDevices SNMPObj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.SNMPDevicesDAL.Ins.GetIPAddress(SNMPObj);
			}
			catch (Exception ex	)
			{
				
				throw ex;
			}
          
        }
        public DataTable GetLocation()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.SNMPDevicesDAL.Ins.GetLocation();
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
				return VSWebDAL.ConfiguratorDAL.SNMPDevicesDAL.Ins.GetServerIDbyServerName(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
    }
}
