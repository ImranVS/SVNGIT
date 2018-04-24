using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
   public class NotesMailProbeBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static NotesMailProbeBL _self = new NotesMailProbeBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static NotesMailProbeBL Ins
        {
            get { return _self; }
        }

        #region "Validations"

        /// <summary>
        /// Validation before submitting data for Server tab
        /// </summary>
        /// <param name="NotesMailProbeObject"></param>
        /// <returns></returns>
        public Object ValidateDCUpdate(NotesMailProbe NotesMailProbeObject)
        {
            Object ReturnValue = "";
            try
            {
                if (NotesMailProbeObject.Name == null || NotesMailProbeObject.Name == "")
                {
                    return "ER#Please enter a short, unique, name for this NotesMail probe.";
                }
                if (NotesMailProbeObject.EchoService == true)
                {
                    if (NotesMailProbeObject.ReplyTo == "")
                    {
                        return "ER#Please enter a valid ReplyTo address to which the Echo Server will send a reply message.  This address should resolve to the target server and database specified.";

                    }
                }
                if (NotesMailProbeObject.NotesMailAddress == null || NotesMailProbeObject.NotesMailAddress == " ")
                {

                    return "ER#Please enter a valid NotesMail address to which this test message will be delivered.";
                }
                //12/10/2012 NS commented out the error message below - there is no Filename field on the form. 
                //The error is thrown whenever a new mail probe is created even when all fields are set.
                //if ((NotesMailProbeObject.Filename == "") || (NotesMailProbeObject.Filename == ""))
                //{
                //    return "ER#Please enter the FILENAME of the Notes database to which the message will be delivered, if successful.'";
                //}
                if (NotesMailProbeObject.DestinationServerID== 0||NotesMailProbeObject.DestinationServerID.ToString()=="")
                {
                    return "ER#Please enter the Domino SERVER which hosts the Notes database to which the message will be delivered, if successful.";
                }
                if (NotesMailProbeObject.SourceServer == " " || NotesMailProbeObject.SourceServer == null)
                {
                    return "ER#Please enter the Source SERVER, on which the test message will be deposited for delivery.";
                }
                if (NotesMailProbeObject.DeliveryThreshold.ToString() == " " || NotesMailProbeObject.DeliveryThreshold == null)
                {
                    return "ER#Please enter a Delivery Threshold, in minutes, over which the delivery to the device will be considered a failure.";
                }
                if (NotesMailProbeObject.RetryInterval == null || NotesMailProbeObject.RetryInterval.ToString()== " ")
                {
                    return "ER#Please enter a Retry Interval, to be used when the device is down.";
                }
                if (NotesMailProbeObject.ScanInterval.ToString() == "")
                {
                    return "ER#Please a scan Interval";
                }
               
                if (NotesMailProbeObject.OffHoursScanInterval.ToString() == " ")
                {
                    return "ER#Please enter an off-hours Scan Interval";
                }

				//if ((NotesMailProbeObject.RetryInterval) > (NotesMailProbeObject.ScanInterval))
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
        /// Call to Get Data from NotesMailProbe based on Primary key
        /// </summary>
        /// <param name="NotesMailProbeObject">NotesMailProbeObject object</param>
        /// <returns></returns>
        public NotesMailProbe GetData(NotesMailProbe NotesMailProbeObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NotesMailProbeDAL.Ins.GetData(NotesMailProbeObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        /// <summary>
        /// Call to Get Data from NotesMailProbe
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NotesMailProbeDAL.Ins.GetAllData();
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
				return VSWebDAL.ConfiguratorDAL.NotesMailProbeDAL.Ins.GetAllHistoryData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        /// <summary>
        /// Call to Insert Data into NotesMailProbe
        ///  </summary>
        /// <param name="NotesMailProbeObject">NotesMailProbe object</param>
        /// <returns></returns>
        public Object InsertData(NotesMailProbe NotesMailProbeObject)
        {     Object ReturnValue = ValidateDCUpdate(NotesMailProbeObject);
		try
		{
			if (ReturnValue.ToString() == "")
			{
				return VSWebDAL.ConfiguratorDAL.NotesMailProbeDAL.Ins.InsertData(NotesMailProbeObject);
			}
			else return ReturnValue;
		}
		catch (Exception ex)
		{
			
			throw ex;
		}
            
        }

        /// <summary>
        /// Call to Update Data of NotesMailProbe based on Key
        /// </summary>
        /// <param name="NotesMailProbeObject">DominoServers object</param>
        /// <returns>Object</returns>
        public Object UpdateData(NotesMailProbe NotesMailProbeObject, string name)
        {
            Object ReturnValue = ValidateDCUpdate(NotesMailProbeObject);
			try
			{
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.NotesMailProbeDAL.Ins.UpdateData(NotesMailProbeObject, name);
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
        public Object DeleteData(NotesMailProbe ProbeObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NotesMailProbeDAL.Ins.DeleteData(ProbeObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        // To Check Unique MailAddress
        public DataTable GetMailAddress(NotesMailProbe MailObj, string name)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NotesMailProbeDAL.Ins.GetIPAddress(MailObj, name);
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
				return VSWebDAL.ConfiguratorDAL.NotesMailProbeDAL.Ins.GetServername();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetAllDataByName(NotesMailProbe NotesMailProbeObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.NotesMailProbeDAL.Ins.GetAllDataByName(NotesMailProbeObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}

            
        }

		public DataTable GetDominoServerNames()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.NotesMailProbeDAL.Ins.GetDominoServerNames();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
    }
}
