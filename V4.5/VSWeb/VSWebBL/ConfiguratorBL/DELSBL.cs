using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;


namespace VSWebBL.ConfiguratorBL
{
	public class DELSBL
	{
		private static DELSBL _self = new DELSBL();

		/// <summary>
		/// Used to call the functions using class name instead of object
		/// </summary>
		public static DELSBL Ins
		{
			get { return _self; }
		}
		public DataTable GetDELSData(DominoEventLog nameObj)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.GetDELSData(nameObj);
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
				return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.insertEventDef(eventDef);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public bool updateEventName(int id, string eventDef)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.updateEventDef(id, eventDef);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetDELSNames()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.GetAllDELSNames();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public Object DeleteDELSDef(int id)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.DeleteDELSDef(id);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public bool InsertData(LogFile logObj)
		{
			try
			{
				//Object ReturnValue = ValidateUpdate(LocObject);
				//DataTable value = VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.GetDataForELSByname(LocObject);
				//if (value.Rows.Count == 0)
				//{
				try
				{
					return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.InsertData(logObj);
				}
				catch (Exception ex)
				{
					throw ex;
				}
					
				//}
				//else return "";
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}

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
		public bool UpdateData(LogFile LocObject)
		{
			
			
				try
				{
					return  VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.UpdateData(LocObject);
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
				return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.EventnamebyKey(id);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public int GetDominoEventLogIdbyName(string  name)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.Eventidbyname(name);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetLogFilesData(int alertKey)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.GetLogFilesData(alertKey);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public bool UpdateLogfileDetails(LogFile logObj)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.UpdateLogfileDetails(logObj);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public Object DeleteData(LogFile LocObject)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.DeleteData(LocObject);
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
				return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.GetServersFromProcedure();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetSelectedServers(int EventKey)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.GetSelectedServers(EventKey);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public bool InsertSelectedServers(int EventKey, DataTable dtSel)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.InsertSelectedServers(EventKey, dtSel);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetDataByAlertName(DominoEventLog NameObj)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.DELSDAL.Ins.GetDataByAlertName(NameObj);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}





	}
	}

