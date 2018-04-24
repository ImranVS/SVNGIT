using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DO;
using System.Data;

namespace DAL
{
	public class UserPreferencesDAL
	{
		private Adaptor objAdaptor = new Adaptor();

		private static UserPreferencesDAL _self = new UserPreferencesDAL();

		public static UserPreferencesDAL Ins
		{
			get { return _self; }
		}

		public void UpdateUserPreferences(string PreferenceName, string PreferenceValue, int UserID)
		{
			
			int mode = 0;
			try
			{
				int count = 0;
				string Query = "SELECT Count(*) FROM UserPreferences WHERE PreferenceName = '" + PreferenceName + "' and UserID =" + UserID;
				count = objAdaptor.ExecuteScalar(Query);
				string SqlQuery = "";
				if (count == 0)
				{
					SqlQuery = "INSERT INTO UserPreferences VALUES('" + PreferenceName + "','" + PreferenceValue + "'," + UserID + ")";
				}
				else
				{
					SqlQuery = "UPDATE UserPreferences SET PreferenceValue='" + PreferenceValue + "' WHERE PreferenceName = '" + PreferenceName + "' and UserID =" + UserID;
				}

				mode = objAdaptor.ExecuteNonQueryRetRows(SqlQuery);
				
			}



			catch (Exception ex)
			{
				throw ex;
			}
		}

		public DataTable GetUserRowPrefrenceDetails(int UserID)
		{
			DataTable dt = new DataTable();
			try
			{
				string Query = "SELECT * FROM UserPreferences WHERE UserID =" + UserID;
				dt = objAdaptor.FetchData(Query);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
	}
}