using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomerTrackingDL;
using System.Data;
using CustomerTrackingDO;

namespace CustomerServiceBL
{
    public class UserPreferencesBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static UserPreferencesBL _self = new UserPreferencesBL();
        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static UserPreferencesBL Ins
        {
            get { return _self; }
        }

        public void UpdateUserPreferences(string PreferenceName, string PreferenceValue, int UserID)
        {
            CustomerTrackingDL.UserPreferencesDL.Ins.UpdateUserPreferences(PreferenceName, PreferenceValue, UserID);
        }

        public DataTable GetUserRowPrefrenceDetails(int UserID)
        {
            return CustomerTrackingDL.UserPreferencesDL.Ins.GetUserRowPrefrenceDetails(UserID);
        }
    }
}
