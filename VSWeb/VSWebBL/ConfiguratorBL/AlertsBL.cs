using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
    public class AlertsBL
    {
        private static AlertsBL _self = new AlertsBL();
        public static AlertsBL Ins
        {
            get
            {
                return _self;
            }

        }

        public DataTable GetAlertNames()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetAllAlertsNames();  
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
             
        }
        public DataTable GetTypes()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetType();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public Object DeleteAlert(AlertNames Alert)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.DeleteAlert(Alert);
			}
			catch (Exception ex)
			{
				
				throw ex;
			} 

        }
        public DataTable GetAlertDetails(AlertNames Alertobj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetHoursData(Alertobj);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public Object DeleteHoursData(AlertDetails AlertObj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.DeleteAlertDetails(AlertObj);
			}
			catch (Exception)
			{
				
				throw;
			}
          
        }
		public Object Deletetemplatedata(EventsTemplate TempObj)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.Deletetemplatedata(TempObj);
			}
			catch (Exception)
			{

				throw;
			}

		}
        public AlertNames GetAlertNamebyKey(AlertNames Alertobj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.AlertnamebyKey(Alertobj);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetAllEvents(string ServerIDs)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetAllEvents(ServerIDs);
        
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetrestrictedEvents(int AlertKey)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetrestrictedEvents(AlertKey);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetSelectedEvents(int AlertKey)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetSelectedEvents(AlertKey);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable GetSelectedServers(int AlertKey)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetSelectedServers(AlertKey);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetServers(int AlertKey)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetServers(AlertKey);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable GetEventsTemplateidbyNames(string templatename)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetEventsTemplateidbyNames(templatename);

			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
        public bool InsertAlertName(AlertNames Aobj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.InsertName(Aobj);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
           
        }

        public bool InsertRestrictedServers(string AlertName, string Server)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.InsertRestrictedServers(AlertName, Server);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public bool InsertRestrictedEvents(string AlertName, string Events)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.InsertRestrictedEvents(AlertName, Events);

			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public bool InsertSelectedEvents(int AlertKey,DataTable dtSel)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.InsertSelectedEvents(AlertKey, dtSel);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public bool InsertSelectedServers(int AlertKey, DataTable dtSel)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.InsertSelectedServers(AlertKey, dtSel);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public bool UpdateName(AlertNames obj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.UpdateName(obj);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable GetDataByAlertName(AlertNames AlertsObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetDataByAlertName(AlertsObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetServer()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetServer();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetAlertTab(string ServerName, string servertypeid)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetAlertTab(ServerName, servertypeid);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetAlertTabbyDiffServers0(string ServerId1, string ServerId2, string ServerId3)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetAlertTabbyDiffServers0(ServerId1, ServerId2, ServerId3);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable GetAlertTabbyDiffServers(string ServerId1, string ServerId2, string ServerId3, string servertypeid)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetAlertTabbyDiffServers(ServerId1, ServerId2, ServerId3, servertypeid);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        //Brijesh
        public bool InsertAlertDetails(AlertDetails alertObj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.InsertAlertDetails(alertObj);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public int GetHrsIndicator(string hrsIndicator)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetHrsIndicator(hrsIndicator);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public bool UpdateAlertDetails(AlertDetails alertObj)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.UpdateAlertDetails(alertObj);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetAlertDetailsData(int alertKey)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetAlertDetailsData(alertKey);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public bool DeleteFromAlertDetails(int id)
        {
			try
			{
				 return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.DeleteFromAlertDetails(id);
        
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
}
           

        public DataTable GetServerIP(string servername)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetServerIP(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

		public DataTable GetEventsFromProcedure()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetEventsFromProcedure();

			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		public DataTable GetSpecificServersFromProcedure(string Page, string Control)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetSpecificServersFromProcedure(Page, Control);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable GetServersCredentialsFromProcedure(string ServerTypeFilter)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetServersCredentialsFromProcedure(ServerTypeFilter);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
        public DataTable GetExchangeServersFromProcedure()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetExchangeServersFromProcedure();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //11/25/2014 NS modified
        public DataTable GetAlertHistory(string startdate, string enddate)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetAlertHistory(startdate, enddate);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        //1/10/2014 NS added
        public string ClearAlertHistory()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.ClearAlertHistory();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //1/10/2014 NS added
        public string DeleteAlertHistory(DateTime daysPriorTo)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.DeleteAlertHistory(daysPriorTo);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //10/20/2014 NS added for VSPLUS-730
        public bool UpdateEventsMaster(string ids)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.UpdateEventsMaster(ids);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable GetActiveDirectoryServersFromProcedure()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetActiveDirectoryServersFromProcedure();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetSharePointServersFromProcedure()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetSharePointServersFromProcedure();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        //12/4/2014 NS added for VSPLUS-1229
        public DataTable GetCustomScripts()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetCustomScripts();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        //12/4/2014 NS added for VSPLUS-1229
        public CustomScript GetScriptByKey(CustomScript CScript)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetCustomScriptsByKey(CScript);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //12/4/2014 NS added for VSPLUS-1229
        public bool InsertScriptDetails(CustomScript CScript)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.InsertScriptDetails(CScript);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //12/4/2014 NS added for VSPLUS-1229
        public bool UpdateScriptDetails(CustomScript CScript)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.UpdateScriptDetails(CScript);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        //12/4/2014 NS added for VSPLUS-1229
        public DataTable GetScripts()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetScripts();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //12/5/2014 NS added for VSPLUS-1229
        public Object DeleteScript(string id)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.DeleteScript(id);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //12/5/2014 NS added for VSPLUS-1229
        public DataTable GetAlertsByScriptID(string id)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetAlertsByScriptID(id);
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
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetServersFromProcedure();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetBUsinessHoursFromProcedure(string Page, string Control)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetBUsinessHoursFromProcedure(Page,Control);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
        //4/2/2015 NS added for VSPLUS-219
        public DataTable GetEscalationDetails(AlertNames Alertobj)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetEscalationData(Alertobj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool InsertEscalationDetails(EscalationDetails escalationObj)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.InsertEscalationDetails(escalationObj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetEscalationDetailsData(int alertKey)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetEscalationDetailsData(alertKey);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool UpdateEscalationDetails(EscalationDetails escalationObj)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.UpdateEscalationDetails(escalationObj);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Object DeleteEscalationData(EscalationDetails escalationObj)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.DeleteEscalationDetails(escalationObj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //5/6/2015 NS added for VSPLUS-1622
        public DataTable GetAlertHistoryByAlertID(string AlertKey)
        {
            return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetAlertHistoryByAlertID(AlertKey);
        }

        //7/17/2015 NS added for VSPLUS-1562
        public DataTable GetAlertEmergencyContacts()
        {
            return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetAlertEmergencyContacts();
        }

        //7/17/2015 NS added for VSPLUS-1562
        public object InsertEmergencyAlertData(string id, string email)
        {
            return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.InsertEmergencyAlertData(id, email);
        }

        //7/17/2015 NS added for VSPLUS-1562
        public object UpdateEmergencyAlertData(string id, string email)
        {
            return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.UpdateEmergencyAlertData(id, email);
        }

        //7/17/2015 NS added for VSPLUS-1562
        public object DeleteData(string id)
        {
            return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.DeleteData(id);
        }
		public DataTable GetAlerteventnames()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetAlerteventnames();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetSelectedEventsfortemplate(int ID)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetSelectedEventsfortemplate(ID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetSelectedEventsfortemplate2(int ID)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetSelectedEventsfortemplate(ID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public bool InserttemplateSelectedEvents(string Nmae, string eventids)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.InserttemplateSelectedEvents(Nmae, eventids);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public bool UpdatetemplateSelectedEvents(string Nmae, string eventids, int ID,string sessionname)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.UpdatetemplateSelectedEvents(Nmae, eventids, ID, sessionname);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetEventsTemplateNmaes(int ID)
		{
			return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetEventsTemplateNmaes(ID);
		}
		public DataTable GetEventsTemplateNames(int ID)
		{
			return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetEventsTemplateNmaes(ID);
		}

		public DataTable GetAllDataForalerteventNames()
		{
			return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetAllDataForalerteventNames();

		}
        // 3/14/2016 Durga Addded for VSPLUS-2704
        public DataTable GetAllOpenAlers(string startdate, string enddate)

        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.GetAllOpenAlers(startdate, enddate);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public bool UpdateEventsMasterforUncheckedCondition(string ids)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.AlertsDAL.Ins.UpdateEventsMasterforUncheckedCondition(ids);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
