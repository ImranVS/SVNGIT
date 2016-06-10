using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebBL;
using VSWebDO;
using System.Data;

namespace VSWebBL.ConfiguratorBL
{
   public class NotesDatabaseBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
       private static NotesDatabaseBL _self = new NotesDatabaseBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
       public static NotesDatabaseBL Ins
        {
            get { return _self; }
        }
       #region "Validations"

       /// <summary>
       /// Validation before submitting data for Server tab
       /// </summary>
       /// <param name="NDObject"></param>
       /// <returns></returns>
       public Object ValidateDCUpdate(NotesDatabases NDObject)
       {
           Object ReturnValue = "";
           try
           {
               if (NDObject.Name == null || NDObject.Name == "")
               {
                   return "ER#Please enter a name to identify this Notes database.";
               }
               if (NDObject.ServerName == null || NDObject.ServerName == " ")
               {
                   return "ER#Please select a licensed server.";
               }
               if(NDObject.TriggerValue==null||NDObject.TriggerValue.ToString()=="")
               {
                  switch(NDObject.Category.ToString())
                  {
                      case "Document Count":

                       return "ER#Please enter a number of documents for this Notes database, over which will be considered 'too many'.";

                          break;

                      case "Database Size":
                         
                          return "ER#Please enter a size, over which the Notes database will be considered 'too big'.";
                          
                          break;

                      case "Database Response Time":
                         
                          return "ER#Please enter a Response Threshold, in milliseconds, over which the device will be considered 'slow'.";
                         
                          break;
                      
                        
                  }
                   
               }
               
               if (NDObject.FileName == null || NDObject.FileName== " ")
               {

                   return "ER#Please enter a database file name";
               }
               if (NDObject.ScanInterval == null) 
               {
                   return "ER#Please enter a Scan Interval";
               }
               if (NDObject.OffHoursScanInterval==null)
               {
                   return "ER#Please enter an off-hours Scan Interval";
               }
               if (NDObject.RetryInterval== null)
               {
                   return "ER#Please enter a Retry Interval, to be used when the device is down.";
               }
			   //if ((NDObject.RetryInterval)>(NDObject.ScanInterval))
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
       /// Call to Get Data from NotesDatabases
       /// </summary>
       /// <returns></returns>
       public DataTable GetAllData()
	   {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.NotesDatabaseDAL.Ins.GetAllData();
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }

       public NotesDatabases GetDataOnID(NotesDatabases NDObject)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.NotesDatabaseDAL.Ins.GetDataOnID(NDObject);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
         
       }

       /// <summary>
       /// Call to Insert Data into NotesDatabases
       ///  </summary>
       /// <param name="NDObject">NotesDatabases object</param>
       /// <returns></returns>
       public Object InsertData(NotesDatabases NDObject)
       {
            Object ReturnValue = ValidateDCUpdate(NDObject);
			try
			{
				if (ReturnValue.ToString() == "")
				{
					return VSWebDAL.ConfiguratorDAL.NotesDatabaseDAL.Ins.InsertData(NDObject);
				}
				else return ReturnValue;
			}
			catch (Exception ex)
			{
				
				throw ex; 
			}
          
       }

       /// <summary>
       /// Call to Update Data of NotesDatabases based on Key
       /// </summary>
       /// <param name="NDObject">NotesDatabases object</param>
       /// <returns>Object</returns>
       public Object UpdateData(NotesDatabases NDObject)
       {
           Object ReturnValue = ValidateDCUpdate(NDObject);
		   try
		   {
			   if (ReturnValue.ToString() == "")
			   {
				   return VSWebDAL.ConfiguratorDAL.NotesDatabaseDAL.Ins.UpdateData(NDObject);
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
       /// <param name="NDObject"></param>
       /// <returns></returns>
       public Object DeleteData(NotesDatabases NDObject)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.NotesDatabaseDAL.Ins.DeleteData(NDObject);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
          
       }
       public DataTable GetName(NotesDatabases NDObj)
       {
		   try
		   {
			   return VSWebDAL.ConfiguratorDAL.NotesDatabaseDAL.Ins.GetName(NDObj);
		   }
		   catch (Exception ex)
		   {
			   
			   throw ex;
		   }
         
       }


    }
}
