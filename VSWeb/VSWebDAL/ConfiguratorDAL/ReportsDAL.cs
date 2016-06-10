using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebDAL.ConfiguratorDAL
{
    public class ReportsDAL
    {
        private Adaptor objAdaptor = new Adaptor();
        private static ReportsDAL _self = new ReportsDAL();

        public static ReportsDAL Ins
        {
            get { return _self; }
        }

        public DataTable GetReports(int ReportID)
        {
            DataTable dt = new DataTable();
            string SqlQuery = "";
            try
            {
                //2/12/2016 NS modified for VSPLUS-2588
                if (ReportID != 0)
                {
                    SqlQuery = "SELECT sr.ID,ReportID,CASE WHEN Frequency = 'Daily' THEN Frequency WHEN Frequency = 'Weekly' THEN " +
                    "Frequency + ' on ' + [Days] ELSE Frequency + ' on the ' + CAST(SpecificDay as varchar(2)) + ' day' END FrequencyDisp, " +
                    "Frequency,Days,SpecificDay,SendTo,CopyTo,BlindCopyTo,Title,Body,FileFormat,Name " +
                    "FROM [ScheduledReports] sr INNER JOIN [ReportItems] ri ON sr.ReportID=ri.ID " +
                    "WHERE sr.ID=" + ReportID.ToString();
                    dt = objAdaptor.FetchData(SqlQuery);
                }
                else if (ReportID != -1)
                {
                    SqlQuery = "SELECT sr.ID,ReportID,CASE WHEN Frequency = 'Daily' THEN Frequency WHEN Frequency = 'Weekly' THEN " +
                    "Frequency + ' on ' + [Days] ELSE Frequency + ' on the ' + CAST(SpecificDay as varchar(2)) + ' day' END FrequencyDisp, " +
                    "Frequency,Days,SpecificDay,SendTo,CopyTo,BlindCopyTo,Title,Body,FileFormat,Name " +
                    "FROM [ScheduledReports] sr INNER JOIN [ReportItems] ri ON sr.ReportID=ri.ID ";
                    dt = objAdaptor.FetchData(SqlQuery);
                }
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

        public bool InsertData(ScheduledReports SRObject)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO [ScheduledReports] (ReportID,Frequency,Days,SpecificDay,SendTo,CopyTo,BlindCopyTo,Title,Body,FileFormat) " +
                    "VALUES(" + SRObject.ReportID + ",'" + SRObject.Frequency + "','" + SRObject.Days + "'," + SRObject.SpecificDay + "," +
                    "'" + SRObject.SendTo + "','" + SRObject.CopyTo + "','" + SRObject.BlindCopyTo + "','" + SRObject.Title +
                    "','" + SRObject.Body + "','" + SRObject.FileFormat + "')";
                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Insert = false;
            }
            return Insert;
        }

        public bool UpdateData(ScheduledReports SRObject)
        {
            bool Update;
            try
            {
                string SqlQuery = "UPDATE [ScheduledReports] SET ReportID=" + SRObject.ReportID + ",Frequency='" + SRObject.Frequency +
                    "',Days='" + SRObject.Days + "',SpecificDay=" + SRObject.SpecificDay + ",SendTo='" + SRObject.SendTo +
                    "',CopyTo='" + SRObject.CopyTo + "',BlindCopyTo='" + SRObject.BlindCopyTo + "',Title='" + SRObject.Title +
                    "',Body='" + SRObject.Body + "',FileFormat='" + SRObject.FileFormat + "' WHERE ID=" + SRObject.ID + "";
                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }

        public bool DeleteData(ScheduledReports SRObject)
        {
            bool Delete;
            try
            {
                string SqlQuery = "DELETE FROM [ScheduledReports] WHERE ID=" + SRObject.ID + "";
                Delete = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Delete = false;
            }
            finally
            {
            }
            return Delete;
        }

        public DataTable GetReportNames()
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT ID, Name FROM [ReportItems] WHERE MaySchedule='True' ORDER BY Name";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetReportFavorites(string userid)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT t3.ID, Name, PageURL FROM Users t1 INNER JOIN UserReportFavorites t2 ON " + 
                    "t1.ID=t2.UserID INNER JOIN ReportItems t3 ON t2.ReportID=t3.ID " +
                    "WHERE t1.ID=" + userid + " ORDER BY IsFavorite DESC, Name ASC";
                dt = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

        public DataTable GetReportPopularity()
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT t1.ID, Name, PageURL, ISNULL(SUM(NumOfClicks),0) FROM ReportItems t1 INNER JOIN " + 
                    "UserReportFavorites t2 ON t1.ID=t2.ReportID GROUP BY t1.ID, Name, PageURL " +
                    "ORDER BY ISNULL(SUM(NumOfClicks),0) DESC, Name ASC";
                dt = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

        public DataTable GetReportTopRated()
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT t1.ID, Name, PageURL, ISNULL(SUM(CAST(IsFavorite as int)),0) FROM ReportItems t1 " +
                    "INNER JOIN UserReportFavorites t2 ON t1.ID=t2.ReportID " +
                    "GROUP BY t1.ID, Name, PageURL HAVING ISNULL(SUM(CAST(IsFavorite as int)),0) > 0 " +
                    "ORDER BY ISNULL(SUM(CAST(IsFavorite as int)),0) DESC, Name ASC";
                dt = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

        public void UpdateUserReportFavorites(string userid,string reportid,string isfavorite)
        {
            bool Update = false;
            try
            {
                string SqlQuery = "UPDATE [UserReportFavorites] SET IsFavorite='" + isfavorite + "',NumOfClicks=NumOfClicks+1 " +
                    "WHERE UserID=" + userid + " AND ReportID=" + reportid;
                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
        }

        public DataTable GetReportID(string reporturl)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT ID FROM ReportItems WHERE PageURL LIKE '%" + reporturl + "' ";
                dt = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

        public DataTable GetUserReportFavorites(string userid, string reportid)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT ID, IsFavorite FROM UserReportFavorites " +
                    "WHERE UserID=" + userid + " AND ReportID=" + reportid;
                dt = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }
    }
}
