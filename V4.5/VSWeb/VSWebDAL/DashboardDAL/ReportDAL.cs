using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebDAL.DashboardDAL
{
   public class ReportDAL
    {
        private Adaptor objAdaptor = new Adaptor();
        private static ReportDAL _self = new ReportDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static ReportDAL Ins
        {
            get { return _self; }
        }

        public DataTable GetAllData(bool isConfig,string URL)
        {
            DataTable ReportDataTable = new DataTable();
            try
            {
                //5/18/2015 NS modified for VSPLUS-1661
                string SqlQuery = "SELECT [ID],[Name],[Category],[Description],[PageURL]+'?M=" + URL +
                    "' AS PageURL,[ImageURL],[ConfiguratorOnly] FROM ReportItems t1,SelectedFeatures sf,FeatureReports fr " +
                    "WHERE fr.FeatureID=sf.FeatureID AND fr.ReportID=t1.ID AND isworking='True' " +
                    " ORDER BY Category,Name";
                
                //2/14/2014 NS modified
                //string SqlQuery = "select [ID],[Name],[Category],[Description],[PageURL]+'?M=" + URL + 
                //    "' as PageURL,[ImageURL],[ConfiguratorOnly] from ReportItems where isworking='True' " + 
                //    (isConfig == false ? " and ConfiguratorOnly='False'" : "") +
                //    " order by Category,Name";
                //string SqlQuery = "select t1.[ID] ReportID,ISNULL(t2.[ID],0) ID,[Name],[Category],[Description],[PageURL]+'?M=" + URL + "' as PageURL," +
                //    "[ImageURL],[ConfiguratorOnly],ISNULL(Frequency,'') Frequency,ISNULL(Days,'') Days,ISNULL(SpecificDay,0) SpecificDay," +
                //    "ISNULL(SendTo,'') SendTo,ISNULL(CopyTo,'') CopyTo,ISNULL(BlindCopyTo,'') BlindCopyTo,ISNULL(Title,'') Title," +
                //    "ISNULL(Body,'') Body,ISNULL(FileFormat,'') FileFormat from ReportItems t1 left outer join ScheduledReports t2 on t1.ID=t2.ReportID " +
                //    "where isworking='True' " + (isConfig == false ? " and ConfiguratorOnly='False'" : "");
                ReportDataTable = objAdaptor.FetchData(SqlQuery);
               
            }
            catch
            {
            }
            finally
            {
            }
            return ReportDataTable;
        }

        public DataTable GetCat()
        {
            DataTable ReportCat = new DataTable();
            try
            {
                string SqlQuery = "Select distinct Category from ReportItems";
                ReportCat = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {
                
                throw;
            }
            return ReportCat;
        }

        public DataTable GetRptsMaySchedule()
        {
            DataTable dt = new DataTable();
            try
            {
                string sqlQuery = "SELECT * FROM ReportItems WHERE MaySchedule=1 ORDER BY Name";
                dt = objAdaptor.FetchData(sqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetScheduledDetails(string rptID)
        {
            DataTable dt = new DataTable();
            try
            {
                string sqlQuery = "select t1.[ID] ReportID,ISNULL(Frequency,'') Frequency,ISNULL(Days,'') Days,ISNULL(SpecificDay,0) SpecificDay," +
                    "ISNULL(SendTo,'') SendTo,ISNULL(CopyTo,'') CopyTo,ISNULL(BlindCopyTo,'') BlindCopyTo,ISNULL(Title,'') Title," +
                    "ISNULL(Body,'') Body,ISNULL(FileFormat,'') FileFormat from ReportItems t1 inner join ScheduledReports t2 on t1.ID=t2.ReportID " +
                    "where isworking='True' and MaySchedule=1 ";
                if (rptID != "")
                {
                    sqlQuery += "and t1.[ID]=" + rptID;
                }
                dt = objAdaptor.FetchData(sqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
    }
}
