using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDAL;

namespace VSWebBL.DashboardBL
{
    public class ReportBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static ReportBL _self = new ReportBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static ReportBL Ins
        {
            get { return _self; }
        }

        public DataTable GetAllData(bool isConfig, string URL)
        {
			try
			{
				return VSWebDAL.DashboardDAL.ReportDAL.Ins.GetAllData(isConfig, URL);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }
        public DataTable GetCat()
        {
			try
			{
				return VSWebDAL.DashboardDAL.ReportDAL.Ins.GetCat();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable GetRptsMaySchedule()
        {
			try
			{
				return VSWebDAL.DashboardDAL.ReportDAL.Ins.GetRptsMaySchedule();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
        public DataTable GetScheduledDetails(string rptID)
        {
			try
			{
				return VSWebDAL.DashboardDAL.ReportDAL.Ins.GetScheduledDetails(rptID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }
    }
}
