using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDAL;
using VSWebDO;

namespace VSWebBL.ConfiguratorBL
{
    public class ExchangePropertiesBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
		private static ExchangePropertiesBL _self = new ExchangePropertiesBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
		public static ExchangePropertiesBL Ins
        {
            get { return _self; }
        }

        #region "Validations"

        /// <summary>
        /// Validation before submitting data for Server Attributes tab
        /// </summary>
        /// <param name="ExchangeServersObject"></param>
        /// <returns></returns>
        public Object ValidateDSUpdate(ExchangeServers  ExchangeServersObject)
        {
            Object ReturnValue="";
            try
            {
               // if (DominoServersObject.Name == null || DominoServersObject.Name == "")
                    //return "ER#Please enter the server name, in hierarchical format, such as 'NYMail01/US/IBM'";
                if ( ExchangeServersObject.DeadThreshold.ToString() == "")
                    return "ER#Please enter the number 'Dead' mail messages, over which you would like to be alerted.'";
                if ( ExchangeServersObject.PendingThreshold.ToString() == "")
                    return "ER#Please enter the number 'Pending' mail messages, over which you would like to be alerted.'";
                if (ExchangeServersObject.HeldThreshold.ToString() == "")
                    return "ER#Please enter the number 'Held' mail messages, over which you would like to be alerted.'";
               // if (DominoServersObject.Location == null || DominoServersObject.Location == "")
                   // return "ER#Please enter the location of the device, such as '8th floor server room'";
               // if (DominoServersObject.Description == null || DominoServersObject.Description == "")
                  //  return "ER#Please enter a description of the server, such as 'New York Executives'";
                if (ExchangeServersObject.Category == null || ExchangeServersObject.Category == "")
                    return "ER#Please enter a category";
                if ( ExchangeServersObject.ResponseThreshold.ToString() == "")
                    return "ER#Please enter a Response Threshold, in milliseconds, over which the device will be considered 'slow'.";
                if (ExchangeServersObject.ScanInterval.ToString() == "")
                    return "ER#Please enter a Scan Interval";
                if (ExchangeServersObject.FailureThreshold.ToString() == "")
                    return "ER#Please enter the failure threshold.  How many times can the server be down before an alert is sent?";
                if ( ExchangeServersObject.OffHoursScanInterval.ToString() == "")
                    return "ER#Please enter an off-hours Scan Interval";
                if (ExchangeServersObject.RetryInterval.ToString() == "")
                    return "ER#Please enter a Retry Interval, to be used when the device is down.";


            }
            catch (Exception ex)
            { throw ex; }
            finally
            { }
            return "";
        }

        #endregion

        /// <summary>
        /// Call to Get Data from DominoServers based on Primary key
        /// </summary>
        /// <param name="ExchangeServersObject">DominoServers object</param>
        /// <returns></returns>
		public ExchangeServers GetData(ExchangeServers ExchangeServersObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ExchangePropertiesDAL.Ins.GetData(ExchangeServersObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

		///// <summary>
		///// Call to Get Data from DominoServers
		///// </summary>
		///// <returns></returns>
		//public DataTable GetAllData()
		//{
		//    return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.GetAllData();
		//}

        /// <summary>
        /// Call to Insert Data into DominoServers
        /// </summary>
        /// <param name="ExchangeServersObject">DominoServers object</param>
        /// <returns></returns>
        public Object InsertData(ExchangeServers ExchangeServersObject)
        {
			try
			{
				Object ReturnValue = ValidateDSUpdate(ExchangeServersObject);
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.ExchangePropertiesDAL.Ins.InsertData(ExchangeServersObject);
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
        /// <param name="DominoServersObject">DominoServers object</param>
        /// <returns>Object</returns>
        public Object UpdateData(ExchangeServers ExchangeServersObject)
        {
			try
			{
				Object ReturnValue = ValidateDSUpdate(ExchangeServersObject);
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.ExchangePropertiesDAL.Ins.UpdateData(ExchangeServersObject);
				}
				else return ReturnValue;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }

		public Object UpdateDAGData(ExchangeServers ExchangeServersObject)
		{
			try
			{
				Object ReturnValue = ValidateDSUpdate(ExchangeServersObject);
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.ExchangePropertiesDAL.Ins.UpdateDAGData(ExchangeServersObject);
				}
				else return ReturnValue;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		///// <summary>
		///// Call to Get Data from DominoServers & ServerTaskSettings based on ServerID
		///// </summary>
		///// <param name="ServerKey"></param>
		///// <returns></returns>
		//public DataTable DSTaskSettingsUpdateGrid(string ServerKey)
		//{
		//    return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.DSTaskSettingsUpdateGrid(ServerKey);
        
		//}
		//public DataTable DSTaskSettingsUpdategridFirstTime(string ServerKey)
		//{
		//    return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.DSTaskSettingsUpdategridFirstTime(ServerKey);
		//}

		//public DataTable Getrestrictedservers(int ID)
		//{
		//    return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.GetRestrictedServers(ID);
		//}

		//public DataTable GetDiskSettings(string Server,string strAll)
		//{
		//    return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.GetDiskSettings(Server, strAll);

		//}
		//public DataTable GetRowsDiskSettings(string Server)
		//{
		//    return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.GetRowsDiskSettings(Server);

		//}
		public bool InsertDiskSettingsData(DataTable dtDisk, int enabled = 1)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ExchangePropertiesDAL.Ins.InsertDiskSettingsData(dtDisk, enabled);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			// 5/14/2014 - CY modified for VS-545
		

		}
		//public bool InsertDiskSettingsDataSSE(DataTable dtDisk, int enabled = 1)
		//{
		//    return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.InsertDiskSettingsDataSSE(dtDisk, enabled);
		//}

		////Mukund 12Jun14:  VE-4	: Implement Disk Checking - Front End
		//public DataTable GetSrvRowsDiskSettings(string ServerID)
		//{
		//    return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.GetSrvRowsDiskSettings(ServerID);

		//}
		//public DataTable GetSrvDiskSettings(string Server, string strAll)
		//{
		//    return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.GetSrvDiskSettings(Server, strAll);

		//}
		//public bool InsertSrvDiskSettingsData(DataTable dtDisk, int enabled = 1)
		//{
		//    return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.InsertSrvDiskSettingsData(dtDisk, enabled);

		//}
		//public bool DeleteAllRecordsfromDiskSettingsBL(string Servername)
		//{
		//    try
		//    {
		//        return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.DeleteAllRecordsfromDiskSettingsDAL(Servername);

		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}

		//10/8/2014 WS VE-107
		public bool InsertSrvDatabaseSettingsData(DataTable dtDisk, String ServerName)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ExchangePropertiesDAL.Ins.InsertSrvDatabaseSettingsData(dtDisk, ServerName);
			}
			catch (Exception ex)
			{

				throw ex;
			}
		
		}
    }
}
