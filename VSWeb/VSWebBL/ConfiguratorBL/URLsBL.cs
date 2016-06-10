using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDAL;
using System.Data;
using VSWebDO;

namespace VSWebBL.ConfiguratorBL
{
   public class URLsBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static URLsBL _self = new URLsBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static URLsBL Ins
        {
            get { return _self; }
        }
        #region "Validations"

        
        /// <param name="URLsObject"></param>
        /// <returns></returns>
        public Object ValidateUpdate(URLs URLsObject)
        {
            Object ReturnValue = "";
            try
            {
                if (URLsObject.Name == null || URLsObject.Name == "")
                {
                    return "ER#Please enter the a name";
                }               
                
                if (URLsObject.Category == null || URLsObject.Category == " ")
                {
                    return "ER#Please enter the category of the URL";
                }
                if (URLsObject.ResponseThreshold == null)
                {
                    return "ER#Please enter a Response Threshold";
                
                }
                if (URLsObject.ScanInterval.ToString() == "")
                {
                    return "ER#Please a scan Interval";
                }
               
                if (URLsObject.OffHoursScanInterval.ToString() == " ")
                {
                    return "ER#Please enter an off-hours Scan Interval";
                }
                if (URLsObject.RetryInterval.ToString() == "")
                {
                    return "ER#Please enter a Retry Interval";
                }
				

				//if ((URLsObject.RetryInterval) > (URLsObject.ScanInterval))
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
        /// Call to Get Data from URLs based on Primary key
        /// </summary>
        /// <param name="URLsObject">URLsObject object</param>
        /// <returns></returns>
        public URLs GetData(URLs URLsObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.URLsDAL.Ins.GetData(URLsObject);
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
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.URLsDAL.Ins.GetAllData();
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
        public bool InsertData(URLs URLsObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.URLsDAL.Ins.InsertData(URLsObject);
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
        public Object UpdateData(URLs URLsObject)
        {
            Object ReturnValue = ValidateUpdate(URLsObject);
			try
			{
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.URLsDAL.Ins.UpdateData(URLsObject);
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
        /// <param name="URLObject"></param>
        /// <returns></returns>
        public Object DeleteData(URLs URLObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.URLsDAL.Ins.DeleteData(URLObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetIPAddress(URLs UrlObj, string mode)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.URLsDAL.Ins.GetIPAddress(UrlObj, mode);
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
				return VSWebDAL.ConfiguratorDAL.URLsDAL.Ins.InsertCustomPageValue(userID, URLval, titleval, isprivate, ID, doinsert);
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
				return VSWebDAL.ConfiguratorDAL.URLsDAL.Ins.GetCustomPageValue(userID);
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
				return VSWebDAL.ConfiguratorDAL.URLsDAL.Ins.GetServerIDbyServerName(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //14/04/2016 Sowmya added for VSPLUS-2725
        public DataTable GetURLDetails(string serverName)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.URLsDAL.Ins.GetURLDetails(serverName);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
