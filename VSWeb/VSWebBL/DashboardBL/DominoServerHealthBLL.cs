using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class DominoServerHealthBLL
    {
        private static DominoServerHealthBLL _self = new DominoServerHealthBLL();

        public static DominoServerHealthBLL Ins
        {
            get
            {
                return _self;
            }
        }

        //public DataTable GetCheckData()
        //{
        //    try
        //    {
        //        return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.CheckedData();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable SetGrid(string serverType)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.SetGrid(serverType);
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
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.GetGraphForMailBox(str, ColumnName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable SetBarGraphForTopMailBox(string serverType)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.SetBarGraphForTopMail(serverType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
		public DataTable SetGraphForTopMailUsers(string server, string radiovalue1, string radiovalue2)
		{
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.SetGraphForTopMailUsers(server, radiovalue1, radiovalue2);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

        public DataTable GetMailStatus(string servernamelist)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.GetMailStatus(servernamelist);
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
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.SetGraphForTransactionPerMin(servernamelist);
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
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.SetGraphForCPU(servernamelist);
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
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.SetGraphForMemory(servernamelist);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //1/2/2014 NS added
        public DataTable SetBarGraphForTopQuota(string serverType)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.SetBarGraphForTopQuota(serverType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        //1/15/2014 NS added
        public DataTable SetPieChartForMailTemplates(string servername)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.SetPieChartForMailTemplates(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
		public DataTable GetDominoServerDetails()
		{
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.GetDominoServerDetails();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetTravelerServerDetails()
		{
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.GetTravelerServerDetails();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetSametimeServerDetails()
		{
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.GetSametimeServerDetails();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetServerTasks(string Name)
		{
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.GetServerTasks(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			} 
			
		}
		public DataTable GetMoniteredTasks(string Name)
		{
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.GetMoniteredTasks(Name);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetIssuesTasks()
		{
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.GetIssuesTasks();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetMailChart(string Type)
		{
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerHealthDAL.Ins.GetMailChart(Type);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

    }
}
