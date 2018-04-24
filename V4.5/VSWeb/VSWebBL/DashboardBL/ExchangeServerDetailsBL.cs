using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
	public class ExchangeServerDetailsBL
	{
		private static ExchangeServerDetailsBL _self = new ExchangeServerDetailsBL();

		public static ExchangeServerDetailsBL Ins
		{
			get
			{
				return _self;
			}
		}

		public DataTable SetGraph(string paramGraph, string DeviceName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraph(paramGraph, DeviceName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable SetGraphforExchange(string paramGraph, string serverName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraphforExchange(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable SetGraphForDiskSpace(string serverName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraphForDiskSpace(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable SetGraphForCPU(string paramGraph, string serverName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraphForCPU(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable SetGraphForMemory(string paramGraph, string serverName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraphForMemory(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable SetGraphForUsers(string paramGraph, string serverName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraphForUsers(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable SetGraphForMailBox(string str, string ColumnName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetGraphForMailBox(str, ColumnName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable SetBarGraphForTopMailBox()
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetBarGraphForTopMail();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable SetBarGraphForTopMailBox(string serverType, string graphType)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetBarGraphForTopMail(serverType, graphType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable SetBarGraphForTopMailDatabase(string databaseName, string graphType)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetBarGraphForTopMailDatabase(databaseName, graphType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetCheckData()
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.CheckedData();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public DataTable SetGraphForTransactionPerMin(string servernamelist)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraphForTransactionPerMin(servernamelist);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable SetGraphForCPU(string servernamelist)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraphForCPU(servernamelist);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable SetGraphForMemory(string servernamelist)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraphForMemory(servernamelist);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetAlertHistry(string Sname)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetAlertHistry(Sname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetOutage(string Sname, string Stype)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetOutage(Sname, Stype);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetMoniteredServices(string ServerName, string Monitored)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetMoniteredServices(ServerName, Monitored);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetExchangeServerHealth(string ServerName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetExchangeServerHealth(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetExchangeMailBoxReport(string ServerName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetExchangeMailBoxReport(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable SetGraphRPCClientAccess(string paramGraph, string DeviceName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraphRPCClientAccess(paramGraph, DeviceName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable SetGraphOutlookWebApp(string paramGraph, string DeviceName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetGraphOutlookWebApp(paramGraph, DeviceName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetExDAGHealthCopyStatus(string ServerName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetExDAGHealthCopyStatus(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetExDAGHealthCopySummary(string ServerName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetExDAGHealthCopySummary(ServerName);
			}
			catch (Exception ex)
			{
				
				throw;
			}
		
		}

		public DataTable GetExDAGHealthMemberReport(string ServerName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetExDAGHealthMemberReport(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}


		public DataTable GetExQueuesStatus(string ServerName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetExQueuesStatus(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			

		}


		//new exchange tables. Mukund 13Mar14        
		public DataTable GetWindowsServices(string servername)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetWindowsServices(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetUnmonitoredWindowsServices(string servername)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetUnmonitoredWindowsServices(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetExMailHealthStatus(string servername)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetExMailHealthStatus(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetExMailHealth(string servername)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetExMailHealth(servername);
			}
			catch (Exception ex	)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetStatusDetails(string servername, string Category)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetStatusDetails(servername, Category);
			}
			catch (Exception ex	)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetHealthAssessmentStatusDetails(string TypeAndName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetHealthAssessmentStatusDetails(TypeAndName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable SetExGraph(string servername, string Statname)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetExGraph(servername, Statname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable SetMailGraph(string servername)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetMailGraph(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

        //2/13/2015 NS added for VSPLUS-1358
        public DataTable SetMailDeliveryRateGraph(string servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetMailDeliveryRateGraph(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        //2/13/2015 NS added for VSPLUS-1358
        public DataTable SetMailSizeGraph(string servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetMailSizeGraph(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        //2/13/2015 NS added for VSPLUS-1358
        public DataTable SetMailCountGraph(string servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetMailCountGraph(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
		public DataTable SetExGraphSingle(string servername, string Statname)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.SetExGraphSingle(servername, Statname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		//7/23/2014 NS modified
		public DataTable GetDAGStatus(string DAGID)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetDAGStatus(DAGID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetDAGMembers(int DAGID)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetDAGMembers(DAGID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetDAGDB(int DAGID)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetDAGDB(DAGID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		
		}
		//Mukund 16Jul14, VSPLUS-824- Threshold in graph is not updating
		public DataTable ResponseThreshold(string ServerName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.ResponseThreshold(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable getMailInformation(string serverName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.getMailInformation(serverName);
			}
			catch (Exception ex) 
			{
				
				throw ex;
			}
			
		}

		//7/24/2014 NS added
		public DataTable GetDAGActivationPreference(string DAGID)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetDAGActivationPreference(DAGID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		//7/25/2014 NS added
		public DataTable GetDAGActivePassive(string DAGID)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetDAGActivePassive(DAGID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		//7/25/2014 NS added
		public DataTable GetDAGHealth(string DAGID)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetDAGHealth(DAGID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public bool isResponding(string TypeANDName)
		{
			try
			{
                return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.isResponding(TypeANDName);
		
			}
			catch (Exception ex)
			{
				
				throw ex;
			}

		}
		//10/8/2014 WS VE-107
		public DataTable getMailDatabaseSize(string ServerName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.getMailDatabaseSettings(ServerName);
			}
			catch (Exception ex)
			{ 
				
				throw ex;
			}
			
		}

		public DataTable getMailDatabaseSizeSettings(string ServerName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.getMailDatabaseSizeSettings(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		//10/27/14 WS VSPLUS-1067
		public DataTable GetDAGDBDetails(int DAGID)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetDAGDBDetails(DAGID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
		
		}

		public string getDagIdFromName(string DagName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.getDagIdFromName(DagName);
			}
			catch (Exception ex)
			{
				
				throw ex; 
			}
		
		}

        //2/9/2015 NS added for VSPLUS-1358
        public DataTable GetExchangeServersList()
        {
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeServerDetailsDAL.Ins.GetExchangeServersList();
			}
			catch (Exception ex )
			{
				
				throw ex;
			}
           
        }
	}
}
