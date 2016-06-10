using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
	public class FeedbackDAL
    {
        private Adaptor objAdaptor = new Adaptor();
		private static FeedbackDAL _self = new FeedbackDAL();

		public static FeedbackDAL Ins
        {
            get { return _self; }
        }
		public bool InsertData(FeedBack FeedObj)

		{
			bool Insert = false;

			try
			{

				string SqlQuery = "Insert Into Feedback(Subject,Type,Message,Status,Attachments)" +

				" Values('" + FeedObj.Subject + "','" + FeedObj.Type + "','" + FeedObj.Message + "','" + FeedObj.Status + "','" + FeedObj.Attachments + "')";
		
				Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{

				Insert = false;
			}
			return Insert;
		}
		public DataTable GetFeedback()
		{


			// string logopath;
			DataTable Feedbacks = new DataTable();
			try
			{
				string sqlQuery = "Select ID,Subject,Type,Message,Status from Feedback where status='pending'";
				Feedbacks = objAdaptor.FetchData(sqlQuery);

				//5/23/2013 NS modified

			}
			catch (Exception ex)
			{
				throw ex;
			}

			return Feedbacks;
		}
		public bool UpdateFeedback(FeedBack fgd)
		{
			bool Update = false;
			try
			{
				//VSPLUS-613, Mukund 14May14, removed login name as not required in update
				//[LoginName]='" + UserAccObject.LoginName +                   "', 

				string SqlQuery = "update [feedback] set [Status]='Completed' where ID=" + fgd.ID + "";

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
		public DataTable GetCompanyName()
		{

			// string logopath;
			DataTable Name = new DataTable();
			try
			{
				string sqlQuery = "Select CompanyName  from Company";
				Name = objAdaptor.FetchData(sqlQuery);

				//5/23/2013 NS modified

			}
			catch (Exception ex)
			{
				throw ex;
			}

			return Name;
		}
     

    }
}

