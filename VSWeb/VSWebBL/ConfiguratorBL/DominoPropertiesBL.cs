using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDAL;
using VSWebDO;

namespace VSWebBL.ConfiguratorBL
{
    public class DominoPropertiesBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
         private static DominoPropertiesBL _self = new DominoPropertiesBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static DominoPropertiesBL Ins
        {
            get { return _self; }
        }

        #region "Validations"

        /// <summary>
        /// Validation before submitting data for Server Attributes tab
        /// </summary>
        /// <param name="DominoServersObject"></param>
        /// <returns></returns>
        public Object ValidateDSUpdate(DominoServers DominoServersObject)
        {
            Object ReturnValue="";
            try
            {
               // if (DominoServersObject.Name == null || DominoServersObject.Name == "")
                    //return "ER#Please enter the server name, in hierarchical format, such as 'NYMail01/US/IBM'";
                if ( DominoServersObject.DeadThreshold.ToString() == "")
                    return "ER#Please enter the number 'Dead' mail messages, over which you would like to be alerted.'";
                if ( DominoServersObject.PendingThreshold.ToString() == "")
                    return "ER#Please enter the number 'Pending' mail messages, over which you would like to be alerted.'";
                if (DominoServersObject.HeldThreshold.ToString() == "")
                    return "ER#Please enter the number 'Held' mail messages, over which you would like to be alerted.'";
               // if (DominoServersObject.Location == null || DominoServersObject.Location == "")
                   // return "ER#Please enter the location of the device, such as '8th floor server room'";
               // if (DominoServersObject.Description == null || DominoServersObject.Description == "")
                  //  return "ER#Please enter a description of the server, such as 'New York Executives'";
                if (DominoServersObject.Category == null || DominoServersObject.Category == "")
                    return "ER#Please enter a category";
                if ( DominoServersObject.ResponseThreshold.ToString() == "")
                    return "ER#Please enter a Response Threshold, in milliseconds, over which the device will be considered 'slow'.";
                if (DominoServersObject.ScanInterval.ToString() == "")
                    return "ER#Please enter a Scan Interval";
                if (DominoServersObject.FailureThreshold.ToString() == "")
                    return "ER#Please enter the failure threshold.  How many times can the server be down before an alert is sent?";
                if ( DominoServersObject.OffHoursScanInterval.ToString() == "")
                    return "ER#Please enter an off-hours Scan Interval";
                if (DominoServersObject.RetryInterval.ToString() == "")
                    return "ER#Please enter a Retry Interval, to be used when the device is down.";
                //6/18/2015 NS added for VSPLUS-1802
                if (DominoServersObject.EXJEnabled)
                {
                    if (DominoServersObject.EXJLookBackEnabled)
                    {
                        if (DominoServersObject.EXJStartTime.ToString() == "")
                            return "ER#Please enter a Start Time to be used when scanning EXJournals (Server Attributes tab).";
                        if (DominoServersObject.EXJDuration.ToString() == "" || DominoServersObject.EXJDuration == 0)
                            return "ER#Please enter a non-zero Duration to be used when scanning EXJournals (Server Attributes tab).";
                        if (DominoServersObject.EXJLookBackDuration.ToString() == "" || DominoServersObject.EXJLookBackDuration == 0)
                            return "ER#Please enter a non-zero Look Back Period to be used when scanning EXJournals (Server Attributes tab).";
                    }
                }
                //2/23/2016 NS added for VSPLUS-2641
                if (DominoServersObject.AvailabilityIndexThreshold.ToString() == "")
                    return "ER#Please enter the availability index threshold.";
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
        /// <param name="DominoServersObject">DominoServers object</param>
        /// <returns></returns>
        public DominoServers GetData(DominoServers DominoServersObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.GetData(DominoServersObject);
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
		/// GetAllDataforuserrestrictions
		///  
		public DataTable GetAllDataforuserrestrictions(int userid)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.GetAllDataforuserrestrictions(userid);
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
				return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        /// <summary>
        /// Call to Insert Data into DominoServers
        /// </summary>
        /// <param name="DominoServersObject">DominoServers object</param>
        /// <returns></returns>
        public Object InsertData(DominoServers DominoServersObject)
        {
			try
			{
				Object ReturnValue = ValidateDSUpdate(DominoServersObject);
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.InsertData(DominoServersObject);
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
        public Object UpdateData(DominoServers DominoServersObject)
        {
			try
			{
				Object ReturnValue = ValidateDSUpdate(DominoServersObject);
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.UpdateData(DominoServersObject);
				}
				else return ReturnValue;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        /// <summary>
        /// Call to Get Data from DominoServers & ServerTaskSettings based on ServerID
        /// </summary>
        /// <param name="ServerKey"></param>
        /// <returns></returns>
        public DataTable DSTaskSettingsUpdateGrid(string ServerKey)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.DSTaskSettingsUpdateGrid(ServerKey);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        
        }
        public DataTable DSTaskSettingsUpdategridFirstTime(string ServerKey)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.DSTaskSettingsUpdategridFirstTime(ServerKey);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable Getrestrictedservers(int ID)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.GetRestrictedServers(ID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetDiskSettings(string Server,string strAll)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.GetDiskSettings(Server, strAll);
			}
			catch (Exception ex)
			{
				
				throw ex ;
			}
           

        }
        public DataTable GetRowsDiskSettings(string Server)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.GetRowsDiskSettings(Server);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         

        }
        public bool InsertDiskSettingsData(DataTable dtDisk,int enabled = 1)
        {
            // 5/14/2014 - CY modified for VS-545
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.InsertDiskSettingsData(dtDisk, enabled);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         

        }
        public bool InsertDiskSettingsDataSSE(DataTable dtDisk, int enabled = 1)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.InsertDiskSettingsDataSSE(dtDisk, enabled);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        //Mukund 12Jun14:  VE-4	: Implement Disk Checking - Front End
        public DataTable GetSrvRowsDiskSettings(string ServerID)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.GetSrvRowsDiskSettings(ServerID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          

        }
        public DataTable GetSrvDiskSettings(string Server, string strAll)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.GetSrvDiskSettings(Server, strAll);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           

        }
        public bool InsertSrvDiskSettingsData(DataTable dtDisk, int enabled = 1)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.InsertSrvDiskSettingsData(dtDisk, enabled);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            

        }
        public bool DeleteAllRecordsfromDiskSettingsBL(string Servername)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.DeleteAllRecordsfromDiskSettingsDAL(Servername);

            }
			catch (Exception ex)
            {
                throw ex;
            }
        }

		public bool ForceDominoTableRefresh()
		{
			try
            {
				return VSWebDAL.ConfiguratorDAL.DominoPropertiesDAL.Ins.ForceDominoTableRefresh();

            }
			catch (Exception ex)
            {
                throw ex;
            }
		}
    }
}
