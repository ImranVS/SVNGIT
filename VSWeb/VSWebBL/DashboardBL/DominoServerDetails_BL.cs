using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.ConfiguratorBL
{
    public class DominoServerDetails_BL
    {
        private static DominoServerDetails_BL _self = new DominoServerDetails_BL();

        public static DominoServerDetails_BL Ins
        {
            get
            {
                return _self;
            }
        }

        public DataTable SetGraph(string paramGraph, string DeviceName)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerDetails_DAL.Ins.SetGraph(paramGraph, DeviceName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable SetGraph2(string paramGraph, string DeviceName)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerDetails_DAL.Ins.SetGraph2(paramGraph, DeviceName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable SetGraphForDayCombo(string paramvalue, string servername, string daysvalue)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerDetails_DAL.Ins.SetGraphForDayCombo(paramvalue, servername, daysvalue);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable SetGraphForCPU(string paramGraph, string serverName)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerDetails_DAL.Ins.SetGraphForCPU(paramGraph, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex ;
			}
            
        }

        public DataTable SetGraphForMemory(string paramGraph, string serverName)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerDetails_DAL.Ins.SetGraphForMemory(paramGraph, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable SetGraphForUsers(string paramGraph, string serverName)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerDetails_DAL.Ins.SetGraphForUsers(paramGraph, serverName);
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
				return VSWebDAL.ConfiguratorDAL.DominoServerDetails_DAL.Ins.SetGraphForDiskSpace(serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        public DataTable SetGraphForHrCombo(string paramValue, string servername, string firstvalue, string secondvalue)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerDetails_DAL.Ins.SetGraphForHrCombo(paramValue, servername, firstvalue, secondvalue);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetPerHourDetails()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerDetails_DAL.Ins.GetPerHourDetails();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetPerDayDetails()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.DominoServerDetails_DAL.Ins.GetPerDayDetails();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
    }
}
