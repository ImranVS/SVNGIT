using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDAL;
using System.Data;
using VSWebDO;

namespace VSWebBL.ConfiguratorBL
{
    public class CloudApplicationsServerBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static CloudApplicationsServerBL _self = new CloudApplicationsServerBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static CloudApplicationsServerBL Ins
        {
            get { return _self; }
        }
        #region "Validations"

        
        /// <param name="URLsObject"></param>
        /// <returns></returns>
        public Object ValidateUpdate(CloudApplicationsServer CloudApplicationsServerObject)
        {
            Object ReturnValue = "";
            try
            {
                if (CloudApplicationsServerObject.Name == null || CloudApplicationsServerObject.Name == "")
                {
                    return "ER#Please enter a value in the Name field.";
                }

                if (CloudApplicationsServerObject.Category == null || CloudApplicationsServerObject.Category == " ")
                {
                    return "ER#Please enter a value in the Category field.";
                }

                if (CloudApplicationsServerObject.ResponseThreshold == null)
                {
                    return "ER#Please enter a value in the Response Threshold field.";
                
                }
                if (CloudApplicationsServerObject.ScanInterval.ToString() == "")
                {
                    return "ER#Please enter a value in the Scan Interval field.";
                }

                if (CloudApplicationsServerObject.OffHoursScanInterval.ToString() == " ")
                {
                    return "ER#Please enter a value in the Off-Hours Scan Interval field.";
                }
                if (CloudApplicationsServerObject.RetryInterval.ToString() == "")
                {
                    return "ER#Please enter a value in the Retry Interval field.";
                }
                if (CloudApplicationsServerObject.URL == "" || CloudApplicationsServerObject.URL == null)
                {
                    return "ER#Please enter a value in the Address field, such as 'http://www.IBM.com'.";
                }

				//if ((CloudApplicationsServerObject.RetryInterval) > (CloudApplicationsServerObject.ScanInterval))
				//{

				//    return "ER#Please enter a value in the Retry Interval field that is less than the Scan Interval value.";
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
        /// Call to Get Data from URLs based on Primary key
        /// </summary>
        /// <param name="URLsObject">URLsObject object</param>
        /// <returns></returns>
        public CloudApplicationsServer GetData(CloudApplicationsServer CloudApplicationsServerObject)
        {

            return VSWebDAL.ConfiguratorDAL.CloudApplicationsServerDAL.Ins.GetData(CloudApplicationsServerObject);
        }

        /// <summary>
        /// Call to Get Data from DominoServers
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.CloudApplicationsServerDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        /// <summary>
        /// Call to Insert Data into URLs
        ///  </summary>
        /// <param name="URLsObject">URLs object</param>
        /// <returns></returns>
        public object InsertData(CloudApplicationsServer CloudApplicationsServerObject)
        {
			Object ReturnValue = ValidateUpdate(CloudApplicationsServerObject);

			try
			{
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.CloudApplicationsServerDAL.Ins.InsertData(CloudApplicationsServerObject);
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
        /// <param name="URLsObject">DominoServers object</param>
        /// <returns>Object</returns>
        public Object UpdateData(CloudApplicationsServer CloudApplicationsServerObject)
        {
            Object ReturnValue = ValidateUpdate(CloudApplicationsServerObject);
			try
			{
				if (ReturnValue.ToString() == "")
            {
                return VSWebDAL.ConfiguratorDAL.CloudApplicationsServerDAL.Ins.UpdateData(CloudApplicationsServerObject);
            }
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
             return ReturnValue;
        }
        /// <summary>
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="URLObject"></param>
        /// <returns></returns>
        public Object DeleteData(CloudApplicationsServer CloudApplicationsServerObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.CloudApplicationsServerDAL.Ins.DeleteData(CloudApplicationsServerObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
        
        }

        public DataTable GetIPAddress(CloudApplicationsServer UrlObj, string mode)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.CloudApplicationsServerDAL.Ins.GetIPAddress(UrlObj, mode);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public bool InsertCustomPageValue(string userID, string URLval, string titleval, bool isprivate, string ID, bool doinsert)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.CloudApplicationsServerDAL.Ins.InsertCustomPageValue(userID, URLval, titleval, isprivate, ID, doinsert);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetCustomPageValue(string userID)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.CloudApplicationsServerDAL.Ins.GetCustomPageValue(userID);
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
				return VSWebDAL.ConfiguratorDAL.CloudApplicationsServerDAL.Ins.GetServerIDbyServerName(serverName);
			}
			catch (Exception ex	)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetCloudData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.CloudApplicationsServerDAL.Ins.GetCloudData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
            
        }
		      public DataTable GetCloudDatavisible()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.CloudApplicationsServerDAL.Ins.GetCloudDatavisible();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
            
        }
			  public DataTable GetCloudDatavisiblefordashboard()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.CloudApplicationsServerDAL.Ins.GetCloudDatavisiblefordashboard();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
            
        }
		

		public DataTable GetCloudStatuses()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.CloudApplicationsServerDAL.Ins.GetCloudStatuses();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			

		}
		public DataTable GetO365Statuses()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.CloudApplicationsServerDAL.Ins.GetOffice365Statuses();

			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		
    }
}
