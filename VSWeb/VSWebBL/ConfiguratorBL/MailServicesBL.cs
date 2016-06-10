using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
   public class MailServicesBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
       private static MailServicesBL _self = new MailServicesBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
       public static MailServicesBL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MailServicesDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        #region "Validations"

        /// <summary>
        /// Validation before submitting data for Server tab
        /// </summary>
        /// <param name="MailServicesObject"></param>
        /// <returns></returns>
        public Object ValidateUpdate(MailServices MailServicesObject)
        {
            Object ReturnValue = "";
            try
            {
                if (MailServicesObject.Name == null || MailServicesObject.Name == "")
                {
                    return "ER#Please enter a Name";
                }
                //if (MailServicesObject.Port== null )
                //{
                //    return "ER#Please enter the protocol of the Mail Service, such as POP3, SMTP, or IMAP";
                //}
                if (MailServicesObject.Description == null || MailServicesObject.Description == " ")
                {

                    return "ER#Please enter a description of the Mail Service, such as 'Objectionable Content Filter'";
                }
                if (MailServicesObject.ResponseThreshold== null)
                {
                    return "ER#Please enter a Response Threshold, in milliseconds, over which the device will be considered 'slow'.";
                }
                if (MailServicesObject.ScanInterval == null)
                {
                    return "ER#Please enter a Scan Interval";
                }
                if (MailServicesObject.OffHoursScanInterval==null)
                {
                    return "ER#Please enter an off-hours Scan Interval";
                }
                if (MailServicesObject.RetryInterval ==  null)
                {
                    return "ER#Please enter a Retry Interval, to be used when the device is down.";
                }
                if (MailServicesObject.Category == null || MailServicesObject.Category == " ")
                {
                    return "ER#Please enter the protocol of the Mail Service, such as POP3, SMTP, or IMAP";
                }
                if (MailServicesObject.ScanInterval.ToString() == "")
                {
                    return "ER#Please a scan Interval";
                }
               
                if (MailServicesObject.OffHoursScanInterval.ToString() == " ")
                {
                    return "ER#Please enter an off-hours Scan Interval";
                }
                if (MailServicesObject.Address == "" && MailServicesObject.Address == null)
                {

                    return "ER#Please enter the Network Address of the Mail Service.";
                }
				//if ((MailServicesObject.RetryInterval) > (MailServicesObject.ScanInterval))
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
        /// Call to Get Data from MailServices based on Primary key
        /// </summary>
        /// <param name="MailServicesObject">MailServicesObject object</param>
        /// <returns></returns>
        public MailServices GetData(MailServices MailServicesObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MailServicesDAL.Ins.GetData(MailServicesObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

       
       
        /// <summary>
        /// Call to Insert Data into MailServices
        ///  </summary>
        /// <param name="MailServicesObject">MailServices object</param>
        /// <returns></returns>
        public bool InsertData(MailServices MailServicesObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MailServicesDAL.Ins.InsertData(MailServicesObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        /// <summary>
        /// Call to Update Data of DominoServers based on Key
        /// </summary>
        /// <param name="MailServicesObject">DominoServers object</param>
        /// <returns>Object</returns>
        public Object UpdateData(MailServices MailServicesObject)
        {
			try
			{
				Object ReturnValue = ValidateUpdate(MailServicesObject);
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.MailServicesDAL.Ins.UpdateData(MailServicesObject);
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
        /// <param name="MSObject"></param>
        /// <returns></returns>
        public Object DeleteData(MailServices MSObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MailServicesDAL.Ins.DeleteData(MSObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

       // To Check Unique IPAddress
        public DataTable GetIPAddress(MailServices MailObj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MailServicesDAL.Ins.GetIPAddress(MailObj);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetServers()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MailServicesDAL.Ins.GetServer();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public int UpdateEXGServiceSettings(int Enabled, string DisplayName, string ServiceName, string ServerType, int SVRId)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MailServicesDAL.Ins.UpdateEXGServiceSettings(Enabled, DisplayName, ServiceName, ServerType, SVRId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public Int32 GetServerIDbyServerName(string Name)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MailServicesDAL.Ins.GetServerIDbyServerName(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
    }
}
