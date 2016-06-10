using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class MailHealthBL
    {

        /// <summary>
        /// Declarations
        /// </summary>
        private static MailHealthBL _self = new MailHealthBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static MailHealthBL Ins
        {
            get { return _self; }
        }


        public DataTable GetMailServiceData()
        {
			try
			{
				return VSWebDAL.DashboardDAL.MailHealthDAL.Ins.GetMailServiceData();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public DataTable GetNotesMailProbeData()
        {
			try
			{
				return VSWebDAL.DashboardDAL.MailHealthDAL.Ins.GetNotesMailProbeData();
			}
			catch (Exception ex)
			{
				
				throw ex; 
			}
           
        }

        public DataTable SetGraphForMailDelivered(string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.MailHealthDAL.Ins.SetGraphForMailDelivered(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public DataTable SetGraphForMailTransffered(string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.MailHealthDAL.Ins.SetGraphForMailTransffered(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public DataTable SetGraphForMailRouted(string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.MailHealthDAL.Ins.SetGraphForMailRouted(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetDominoServers()
        {
			try
			{
				return VSWebDAL.DashboardDAL.MailHealthDAL.Ins.GetDominoServer();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetDominoServersForMailHealth()
        {
			try
			{
				return VSWebDAL.DashboardDAL.MailHealthDAL.Ins.GetDominoServersForMailHealth();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        public DataTable GetServerMailDelivered(string servername, int month, int year, bool exactmatch)
        {
			try
			{
				return VSWebDAL.DashboardDAL.MailHealthDAL.Ins.GetServerMailDelivered(servername, month, year, exactmatch);
			}
			catch (Exception ex)
			{
				
				throw ex;
			} 
          
        }
        public DataTable GetMailStats(string servername, int month, int year, bool exactmatch)
        {
			try
			{
				return VSWebDAL.DashboardDAL.MailHealthDAL.Ins.GetMailStats(servername, month, year, exactmatch);
			}
			catch (Exception ex)
			{
				
				throw ex	;
			}
           
        }
        public DataTable fillStatisticsServer()
        {
			try
			{
				return VSWebDAL.DashboardDAL.MailHealthDAL.Ins.fillStatisticsServer();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
        //2/9/2015 NS modified for VSPLUS-1446
        public DataTable GetMailDelivData(string ServerType)
        {
			try
			{
				return VSWebDAL.DashboardDAL.MailHealthDAL.Ins.GetMailDelivData(ServerType);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }
    }
}
