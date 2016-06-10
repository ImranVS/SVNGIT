using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;


namespace VSWebBL.ConfiguratorBL
{
    public class SametimeServerBL
    {
        private static SametimeServerBL _self = new SametimeServerBL();

        public static SametimeServerBL Ins
        {
            get { return _self; }
        }

        public Object ValidateSametimeServerUpdate( SametimeServers UpdateObject)
        {

            Object ReturnValue = "";
            try
            {
               // if (UpdateObject.Name == null || UpdateObject.Name == "")
                //{
                  //  return "ER#Please enter the  SametimeServers Name";

                //}
                if (UpdateObject.Category == null || UpdateObject.Category == "")
                {
                    return "ER#Please enter the category of the device";
                }
               // if (UpdateObject.Location == null || UpdateObject.Location == "")
               // {
                  //  return "ER#Please enter the location of the device";
               // }
               // if (UpdateObject.Description == null || UpdateObject.Description == "")
               // {
                   // return "ER#Please enter a description of the device";
                //}
                if (UpdateObject.ResponseThreshold==null)
                {
                    return "ER#Please enter a Response Threshold, in milliseconds, over which the device will be considered 'slow'. Validation Failure";

                }

                if (UpdateObject.ScanInterval == null )
                {
                    return "ER#Please enter a Scan Interval";
                }

                if (UpdateObject.RetryInterval == null)
                {
                    return "ER#Please enter a Retry Interval";
                }
               // if (UpdateObject.IPAddress == null)
               // {
                 //   return "ER#Please enter the IP Address device, such as '127.0.0.1'";
                //}
                if (UpdateObject.OffHoursScanInterval== null)
                {
                    return "ER#Please enter an off-hours Scan Interval that is a number, in minutes.";
                }
                //if ((UpdateObject.RetryInterval) > (UpdateObject.ScanInterval))
                //{
                //    return "ER#Please enter a Retry Interval that is less than the Scan Interval";
                  
                //}
               

            }
            catch (Exception t)
            {
                
                throw t;
            }
            return "";




        }



        public DataTable getdata()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.SametimeServersDAL.Ins.getdataforlotussametimegrid();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}

         
        }

		public DataTable getdataforlotussametimegridbyuser(int UserID)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.SametimeServersDAL.Ins.getdataforlotussametimegridbyuser(UserID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}

         
        }
		public DataTable Scangetdata()
		{
			try
			{

				return VSWebDAL.ConfiguratorDAL.SametimeServersDAL.Ins.ScangetdataforScanNowItemsgrid();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}

		}



        public Object DeleteSametime(SametimeServers StSobject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.SametimeServersDAL.Ins.DeleteSametimeServer(StSobject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }


        public Object UpdateSametime(SametimeServers StSObject)
        {
            Object returnval = ValidateSametimeServerUpdate(StSObject);
			try
			{
				if (returnval.ToString() == "")
				{

					return VSWebDAL.ConfiguratorDAL.SametimeServersDAL.Ins.UpdateSametimeServer(StSObject);
				}
				else return returnval;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public SametimeServers getdatawithId(SametimeServers stsObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.SametimeServersDAL.Ins.GetdataSametimeServer(stsObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           

        }

        public Object insertdetail(SametimeServers insertObject)
        {
            //bool insert=false;
            Object returnVal = ValidateSametimeServerUpdate(insertObject);
			try
			{
				if (returnVal.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.SametimeServersDAL.Ins.insertdetails(insertObject);
				}
				else return returnVal;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
            

        }

		public DataTable Getwebspherecell(WebsphereCell Stobj)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.SametimeServersDAL.Ins.Getwebspherecell(Stobj);
		
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		}
			
		public DataTable GetcellID(WebsphereCell Stobj)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.SametimeServersDAL.Ins.GetcellID(Stobj);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
        public DataTable GetIPAddress(SametimeServers StObj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.SametimeServersDAL.Ins.GetIPAddress(StObj);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public bool InsertData(WebsphereCell STSettingsObject, int key)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.SametimeServersDAL.Ins.InsertData1(STSettingsObject, key);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
    }
}
