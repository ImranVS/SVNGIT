using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.ConfiguratorBL
{
    public class ReportsBL
    {
        private static ReportsBL _self = new ReportsBL();
        public static ReportsBL Ins
        {
            get
            {
                return _self;
            }
        }

        public DataTable GetReports(int ReportID)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ReportsDAL.Ins.GetReports(ReportID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public bool InsertData(ScheduledReports SRObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ReportsDAL.Ins.InsertData(SRObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public bool UpdateData(ScheduledReports SRObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ReportsDAL.Ins.UpdateData(SRObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetReportNames()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ReportsDAL.Ins.GetReportNames();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        public bool DeleteData(ScheduledReports SRObject)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ReportsDAL.Ins.DeleteData(SRObject);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetReportFavorites(string userid)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ReportsDAL.Ins.GetReportFavorites(userid);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }

        public DataTable GetReportPopularity()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ReportsDAL.Ins.GetReportPopularity();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable GetReportTopRated()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ReportsDAL.Ins.GetReportTopRated();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
         
        }

        public void UpdateUserReportFavorites(string userid, string reportid, string isfavorite)
        {
			try
			{
				VSWebDAL.ConfiguratorDAL.ReportsDAL.Ins.UpdateUserReportFavorites(userid, reportid, isfavorite);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
        }

        public DataTable GetReportID(string reporturl)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ReportsDAL.Ins.GetReportID(reporturl);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
          
        }

        public DataTable GetUserReportFavorites(string userid, string reportid)
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.ReportsDAL.Ins.GetUserReportFavorites(userid, reportid);
			}
			catch (Exception)
			{
				
				throw;
			}
            
        }
    }
}
