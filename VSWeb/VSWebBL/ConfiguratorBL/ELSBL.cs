using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
   public class ELSBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static ELSBL _self = new ELSBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static ELSBL Ins
        {
            get { return _self; }
        }
        public DataTable GetAllData()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.GetAllData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable GetServersFromProcedure()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.GetServersFromProcedure();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetSelectedEventsForKey(int EventId)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.GetEventsForKey(EventId);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public string GetEventNamebyKey(int id)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.EventnamebyKey(id);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

        public Object ValidateUpdate(ELSMaster LocObject)
        {
            Object ReturnValue = "";
            try
            {
				if (LocObject.EventLevel == null || LocObject.EventLevel == "")
                {
                    return "ER#Please enter the Event Level";
                }

            }
            catch (Exception ex)
            { throw ex; }
            finally
            { }
            return "";
        }

        /// <summary>
        /// Call to Insert Data into Alias
        ///  </summary>
        /// <param name="LocObject">Alias object</param>
        /// <returns></returns>
        public object InsertData(ELSMaster LocObject)
        {
			try
			{
				//Object ReturnValue = ValidateUpdate(LocObject);
				DataTable value = VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.GetDataForELSByname(LocObject);
				if (value.Rows.Count == 0)
				{
					//if (ReturnValue.ToString() == "")
					//{
						return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.InsertData(LocObject);
					//}
					//else return ReturnValue;
				}
				else return "";
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
               
        }
        /// <summary>
        /// Call to Update Data of DominoServers based on Key
        /// </summary>
        /// <param name="LocObject">DominoServers object</param>
        /// <returns>Object</returns>
        public bool UpdateData(ELSMaster LocObject)
		{
			bool update = false;
			try
			{
			
				Object ReturnValue = ValidateUpdate(LocObject);
				DataTable value = VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.GetDataForELSByname(LocObject);
				
				if (value.Rows.Count > 0)
				{
					string name = value.Rows[0]["AliasName"].ToString();
					if (ReturnValue.ToString() == "")
					{
						if (name != LocObject.AliasName)
						{
							update = VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.UpdateData(LocObject);

						}
						else
						{
							update = false;
						}
					}
					else
						return update;
				}
				else return update;
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			return update;
        }
        /// <summary>
        /// Call DAL Delete Data
        /// </summary>
        /// <param name="LocObject"></param>
        /// <returns></returns>
		public Object DeleteData(ELSMaster LocObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.DeleteData(LocObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

		public Object DeleteELSDef(int id)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.DeleteELSDef(id);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
       
		public DataTable getCredentialsById(ELSMaster LocObject)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.GetDataForCredentialsById(LocObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetELSNames()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.GetAllELSNames();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetEventHistory()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.GetAllEventHistory();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetEventHistorydetails(string name,string type)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.GetEventHistorydetails(name,type);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetEventHistory(string startdate, string enddate)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.GetAllEventHistory(startdate, enddate);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetEventHistorydetails(string startdate, string enddate,string name)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.GetAllEventHistorydetails(startdate, enddate,name);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetEventHistoryByLog(string logName)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.GetAllEventHistoryByLogName(logName);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetEventHistoryByLogdetails(string logName,string name,string type)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.GetAllEventHistoryByLogNamedetails(logName,name,type);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
	   
		public bool insertEventServers(string eventDef, DataTable dtEvents, DataTable dtEventServers)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.insertEventServers(eventDef, dtEvents, dtEventServers);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public bool insertEventName(string eventDef)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.insertEventDef(eventDef);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public bool updateEventName(int id,string eventDef,string sessionname)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.updateEventDef(id, eventDef, sessionname);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetSelectedServers(int eventKey)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.ELSDAL.Ins.GetSelectedServers(eventKey);
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}


    }
}
