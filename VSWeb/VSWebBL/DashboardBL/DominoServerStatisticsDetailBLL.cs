using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
    public class DominoServerStatisticsDetailBLL
    {
        private static DominoServerStatisticsDetailBLL _self = new DominoServerStatisticsDetailBLL();

        public static DominoServerStatisticsDetailBLL Ins
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
				return VSWebDAL.DashboardDAL.DominoServerStatisticsDetailDAL.Ins.SetGraph(paramGraph, DeviceName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable GetDominoServerThreshold()
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerStatisticsDetailDAL.Ins.GetDominoServerThreshold();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }


        public DataTable SetGraphForHrCombo(string paramValue, string servername, string firstvalue, string secondvalue, string Statname)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerStatisticsDetailDAL.Ins.SetGraphForHrCombo(paramValue, servername, firstvalue, secondvalue, Statname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable SetGraphForMonthCombo(string servername, string paramval, string monthsval)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerStatisticsDetailDAL.Ins.SetGraphForMonthCombo(servername, paramval, monthsval);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        public DataTable SetGraphForDayCombo(string paramvalue, string servername, string daysvalue, string firstvalue, string secondvalue, string Statname)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerStatisticsDetailDAL.Ins.SetGraphForDayCombo(paramvalue, servername, daysvalue, firstvalue, secondvalue, Statname);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable SetGraph2(string today, string reqDate, string serverName)
        {
			try
			{
				return VSWebDAL.DashboardDAL.DominoServerStatisticsDetailDAL.Ins.SetGraph2(today, reqDate, serverName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        
    }
}
