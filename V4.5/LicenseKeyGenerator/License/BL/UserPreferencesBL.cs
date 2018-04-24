using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DAL;
using DO;

namespace BL
{
	public class UserPreferencesBL
	{
		private static UserPreferencesBL _self = new UserPreferencesBL();
		public static UserPreferencesBL Ins
		{
			get { return _self; }
		}

		public void UpdateUserPreferences(string PreferenceName, string PreferenceValue, int UserID)
		{
			try
			{
				DAL.UserPreferencesDAL.Ins.UpdateUserPreferences(PreferenceName, PreferenceValue, UserID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetUserRowPrefrenceDetails(int UserID)
		{
			try
			{
				return DAL.UserPreferencesDAL.Ins.GetUserRowPrefrenceDetails(UserID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

	}
}