using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
  public class BlackBerryBL
    {

     private static BlackBerryBL _self = new BlackBerryBL();
     public static BlackBerryBL Ins
     {
         get
         {
             return _self;
         }

     }

     public Object validators(BlackBerry BlackBerryObject)
     {
		 Object returnVal = "";
		 try
		 {
			
			 if (BlackBerryObject.Name == null)
			 {
				 return "ER#Please enter the BlackBerry Name";

			 }
			 if (BlackBerryObject.Category == null)
			 {
				 return "ER#Please Category the BlackBerry";
			 }

			 if (BlackBerryObject.RetryInterval == null)
			 {
				 return "ER#Please Entry RetryInterval BlackBerry";
			 }

			 if (BlackBerryObject.ScanInterval == null)
			 {
				 return "ER#Please Entry ScanInterval BlackBerry";
			 }
			 if (BlackBerryObject.OffHoursScanInterval == null)
			 {
				 return "ER#Please Entry OffHoursScanInterval BlackBerry";

			 }
			 if (BlackBerryObject.RetryInterval > BlackBerryObject.ScanInterval)
			 {
				 return "ER#Please enter a Retry Interval that is less than the Scan Interval.";
			 }
			 if (BlackBerryObject.ConfirmationServerID == 0)
			 {
				 return "ER#Please Select Source Server.";
			 }
			 if (BlackBerryObject.DestinationServerID == 0)
			 {
				 return "ER#Please Select Target Server.";
			 }
		 }
		 catch (Exception ex)
		 {
			 
			 throw ex;
		 }

		 return returnVal;
     }


     public DataTable checkname(string name,string internetmail)
     {
		 try
		 {
			 return VSWebDAL.ConfiguratorDAL.BlackBerryDAL.Ins.checkforname(name, internetmail);
		 }
		 catch (Exception ex)
		 {
			 
			 throw ex;
		 }
       
     }


        public DataTable gettabledetails()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.BlackBerryDAL.Ins.getdataofblackberry();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public Object insertBlackBerryDeviceProdbegrid(BlackBerry BlackBerryObject)
        {
			Object returnval = validators(BlackBerryObject);
			try
			{
			
				
				if (returnval.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.BlackBerryDAL.Ins.InsertBlackBerry(BlackBerryObject);
				}
				
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			return returnval;
        }

        public DataTable gettable(BlackBerry BlackBerryObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.BlackBerryDAL.Ins.getdatawithId(BlackBerryObject);

			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }


        public Object updateBlackBerryDevice(BlackBerry BlackBerryObject,string Hid)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.BlackBerryDAL.Ins.updatedetails(BlackBerryObject, Hid);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable getdatadominoserver()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.BlackBerryDAL.Ins.getdatadominoserver();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public Object delete(BlackBerry blackobject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.BlackBerryDAL.Ins.delete(blackobject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
      
        public DataTable GetDatabyName(BlackBerry BBObj,string mail)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.BlackBerryDAL.Ins.GetDatabyName(BBObj, mail);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }


    }
}
