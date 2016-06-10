using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
    public class ExchangeMailProbeBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static ExchangeMailProbeBL _self = new ExchangeMailProbeBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static ExchangeMailProbeBL Ins
        {
            get { return _self; }
        }

        #region "Validations"

        /// <summary>
        /// Validation before submitting data for Server tab
        /// </summary>
        /// <param name="ExchangeMailProbeObject"></param>
        /// <returns></returns>
        public Object ValidateDCUpdate(ExchangeMailProbeClass ExchangeMailProbeObject)
        {
            Object ReturnValue = "";
            try
            {
                if (ExchangeMailProbeObject.Name == null || ExchangeMailProbeObject.Name == "")
                {
                    return "ER#Please enter a short, unique, name for this ExchangeMail probe.";
                }
                if (ExchangeMailProbeObject.ExchangeMailAddress == null || ExchangeMailProbeObject.ExchangeMailAddress == " ")
                {

                    return "ER#Please enter a valid ExchangeMail address to which this test message will be delivered.";
                }
                //12/10/2012 NS commented out the error message below - there is no Filename field on the form. 
                //The error is thrown whenever a new mail probe is created even when all fields are set.
                //if ((ExchangeMailProbeObject.Filename == "") || (ExchangeMailProbeObject.Filename == ""))
                //{
                //    return "ER#Please enter the FILENAME of the Exchange database to which the message will be delivered, if successful.'";
                //}
                if (ExchangeMailProbeObject.SourceServer == " " || ExchangeMailProbeObject.SourceServer == null)
                {
                    return "ER#Please enter the Source SERVER, on which the test message will be deposited for delivery.";
                }
                if (ExchangeMailProbeObject.DeliveryThreshold.ToString() == " " || ExchangeMailProbeObject.DeliveryThreshold == null)
                {
                    return "ER#Please enter a Delivery Threshold, in minutes, over which the delivery to the device will be considered a failure.";
                }
                if (ExchangeMailProbeObject.RetryInterval == null || ExchangeMailProbeObject.RetryInterval.ToString() == " ")
                {
                    return "ER#Please enter a Retry Interval, to be used when the device is down.";
                }
                if (ExchangeMailProbeObject.ScanInterval.ToString() == "")
                {
                    return "ER#Please a scan Interval";
                }

                if (ExchangeMailProbeObject.OffHoursScanInterval.ToString() == " ")
                {
                    return "ER#Please enter an off-hours Scan Interval";
                }

				//if ((ExchangeMailProbeObject.RetryInterval) > (ExchangeMailProbeObject.ScanInterval))
				//{
				//    return "ER#Please enter a Retry Interval that is less than the Scan Interval.";
				//}

                //        If Not (Array.IndexOf(DominoServerlist, Me.cmbSourceServer.Text) >= 0) And Me.cmbSourceServer.Text <> "SMTP Server" Then
                //        MessageBox.Show("Please select a licensed server.", "Validation Failure", MessageBoxButtons.OK, MessageBoxIcon.Error)
                //        Me.cmbSourceServer.Focus()
                //        Exit Sub
                //    End If

                //    If Not (Array.IndexOf(DominoServerlist, Me.cmbTargetServer.Text) >= 0) Then
                //        MessageBox.Show("Please select a licensed server.", "Validation Failure", MessageBoxButtons.OK, MessageBoxIcon.Error)
                //        Me.cmbTargetServer.Focus()
                //        Exit Sub
                //    End If
                //Catch ex As Exception
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { }
            return "";
        }

        #endregion


        /// <summary>
        /// Call to Get Data from ExchangeMailProbe based on Primary key
        /// </summary>
        /// <param name="ExchangeMailProbeObject">ExchangeMailProbeObject object</param>
        /// <returns></returns>
        public ExchangeMailProbeClass GetData(ExchangeMailProbeClass ExchangeMailProbeObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ExchangeMailProbeDAL.Ins.GetData(ExchangeMailProbeObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        /// <summary>
        /// Call to Get Data from ExchangeMailProbe
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ExchangeMailProbeDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }
        public DataTable GetAllHistoryData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ExchangeMailProbeDAL.Ins.GetAllHistoryData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        /// <summary>
        /// Call to Insert Data into ExchangeMailProbe
        ///  </summary>
        /// <param name="ExchangeMailProbeObject">ExchangeMailProbe object</param>
        /// <returns></returns>
        public Object InsertData(ExchangeMailProbeClass ExchangeMailProbeObject)
        {
			try
			{
				Object ReturnValue = ValidateDCUpdate(ExchangeMailProbeObject);
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.ExchangeMailProbeDAL.Ins.InsertData(ExchangeMailProbeObject);
				}
				else return ReturnValue;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        /// <summary>
        /// Call to Update Data of ExchangeMailProbe based on Key
        /// </summary>
        /// <param name="ExchangeMailProbeObject">DominoServers object</param>
        /// <returns>Object</returns>
        public Object UpdateData(ExchangeMailProbeClass ExchangeMailProbeObject, string name)
        {
			try
			{
				Object ReturnValue = ValidateDCUpdate(ExchangeMailProbeObject);
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.ExchangeMailProbeDAL.Ins.UpdateData(ExchangeMailProbeObject, name);
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
        /// <param name="ProbeObject"></param>
        /// <returns></returns>
        public Object DeleteData(ExchangeMailProbeClass ProbeObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ExchangeMailProbeDAL.Ins.DeleteData(ProbeObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        // To Check Unique MailAddress
        public DataTable GetMailAddress(ExchangeMailProbeClass MailObj, string name)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ExchangeMailProbeDAL.Ins.GetIPAddress(MailObj, name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetServername()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ExchangeMailProbeDAL.Ins.GetServername();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        public DataTable GetAllDataByName(ExchangeMailProbeClass ExchangeMailProbeObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ExchangeMailProbeDAL.Ins.GetAllDataByName(ExchangeMailProbeObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

    }
}
