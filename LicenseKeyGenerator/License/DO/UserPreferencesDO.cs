using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DO
{
	public class UserPreferencesDO
	{
		
		public UserPreferencesDO()
		{}
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;


		public string PreferenceName
        {
			get { return _PreferenceName; }
			set { _PreferenceName = value; }
        }

		private string _PreferenceName;

		public string PreferenceValue
		{
			get { return _PreferenceValue; }
			set { _PreferenceValue = value; }
		}

		private string _PreferenceValue;

		public int UserID
		{
			get { return _UserID; }
			set { _UserID = value; }
		}
		private int _UserID;


	}
}