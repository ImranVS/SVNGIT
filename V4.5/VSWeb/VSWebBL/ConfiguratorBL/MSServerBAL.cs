using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;
namespace VSWebBL.ConfiguratorBL
{
    public class MSServerBAL
    {
        private static MSServerBAL _self = new MSServerBAL();
        public static MSServerBAL Ins
        {
            get {return _self;}
        }

        public DataTable GetAllData()
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.GetAllData();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }

        public DataTable FillScriptCombo()
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.FillScriptCombo();
            }
            catch (Exception ex)
            {

                throw ex;
            } 
        }

          public DataTable FillAttributecombo(PowershellScripts obj)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.FillAttributecombo(obj);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       
          public DataTable getthresholdGrid(int ServerID)
          {
              try
              {
                  return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.getthresholdGrid(ServerID);
              }
              catch (Exception ex)
              {

                  throw ex;
              }
          
          }
          public DataTable getdata(int ServerID)
          {
              try
              {
                  return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.getdata(ServerID);
              }
              catch (Exception ex)
              {

                  throw ex;
              }
          }

          public DataTable getpidbypname(string Alias)
          {
              try
              {
                  return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.getpidbypname(Alias);
              }
              catch (Exception ex)
              {

                  throw ex;
              }
          
          }

          public DataTable getpSIDbySname(string Script)
          {
              try
              {
                  return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.getpSIDbySname(Script);
              }
              catch (Exception ex)
              {

                  throw ex;
              }
          
          }

       
        public DataTable GetCredentials()
           {
               try
               {
                   return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.GetCredentials();
               }
               catch (Exception ex)
               {

                   throw ex;
               }
           }

        public bool InsertMSSettings(MSServerSettings msobj)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.InsertMSSettings(msobj);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable FillThresholdcombo(PowershellScripts obj, InputParameters ipobj)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.FillThresholdcombo(obj, ipobj);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool InsertServerThreshold(ServerThresholds srt)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.InsertServerThreshold(srt);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public bool UpdateServerThreshold(ServerThresholds srt)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.UpdateServerThreshold(srt);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public bool DeleteServerThreshold(ServerThresholds srt)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.DeleteServerThreshold(srt);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataTable GetWindowServices(string ServerName)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.GetWindowServices(ServerName);
            }
            catch (Exception ex) 
            {

                throw ex;
            }
        }
        public bool UpdateServicesforMonitored(string ServerName, string Service_Name, string Monitored)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.UpdateServicesforMonitored(ServerName, Service_Name, Monitored);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }


		public Object ValidateDSUpdate(ExchangeServers ExchangeServersObject, string ServerType)
		{
			Object ReturnValue = "";
			try
			{
				if (ExchangeServersObject.Category == null || ExchangeServersObject.Category == "")
					return "ER#Please enter a category";
				if (ExchangeServersObject.ResponseThreshold.ToString() == "")
					return "ER#Please enter a Response Threshold, in milliseconds, over which the device will be considered 'slow'.";
				if (ExchangeServersObject.ScanInterval.ToString() == "")
					return "ER#Please enter a Scan Interval";
				if (ExchangeServersObject.FailureThreshold.ToString() == "")
					return "ER#Please enter the failure threshold.  How many times can the server be down before an alert is sent?";
				if (ExchangeServersObject.OffHoursScanInterval.ToString() == "")
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

		public Object UpdateData(ExchangeServers ExchangeServersObject, string ServerType)
		{
			try
			{
				Object ReturnValue = ValidateDSUpdate(ExchangeServersObject, ServerType);

				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.UpdateData(ExchangeServersObject, ServerType);
					//return VSWebDAL.ConfiguratorDAL.ExchangePropertiesDAL.Ins.UpdateDAGData(ExchangeServersObject);
				}
				else return ReturnValue;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public ExchangeServers GetData(ExchangeServers ExchangeServersObject)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.MSServerDAL.Ins.GetData(ExchangeServersObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public bool InsertDiskSettingsData(DataTable dtDisk, int enabled = 1)
		{
			// 5/14/2014 - CY modified for VS-545
			try
			{
				return VSWebDAL.ConfiguratorDAL.ExchangePropertiesDAL.Ins.InsertDiskSettingsData(dtDisk, enabled);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			

		}
    }
}
