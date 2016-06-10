using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTrackingDO
{
    public class Users
    {
        public Users()
        {}

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;


        public string LoginName
        {
            get { return _LoginName; }
            set { _LoginName = value; }
        }

        private string _LoginName;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private string _Password;

        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }

        private string _FullName;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _Email;

        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private string _Status;

        public string SuperAdmin
        {
            get { return _SuperAdmin; }
            set { _SuperAdmin = value; }
        }

        private string _SuperAdmin;

        //5/17/2012 NS added new columns from the Users table
        public string SecurityQuestion1
        {
            get { return _SecurityQuestion1; }
            set { _SecurityQuestion1 = value; }
        }

        private string _SecurityQuestion1;

        public string SecurityQuestion1Answer
        {
            get { return _SecurityQuestion1Answer; }
            set { _SecurityQuestion1Answer = value; }
        }

        private string _SecurityQuestion1Answer;

        public string SecurityQuestion2
        {
            get { return _SecurityQuestion2; }
            set { _SecurityQuestion2 = value; }
        }

        private string _SecurityQuestion2;

        public string SecurityQuestion2Answer
        {
            get { return _SecurityQuestion2Answer; }
            set { _SecurityQuestion2Answer = value; }
        }

        private string _SecurityQuestion2Answer;

        public bool IsConfigurator
        {
            get { return _IsConfigurator; }
            set { _IsConfigurator = value; }
        }

        private bool _IsConfigurator;

        public bool Isdashboard
        {
            get { return _Isdashboard; }
            set { _Isdashboard = value; }
        }

        private bool _Isdashboard;

        public int Refreshtime
        {
            get { return _Refreshtime; }
            set { _Refreshtime = value; }
        }

        private int _Refreshtime;

        public bool Isconsolecomm
        {
            get { return _Isconsolecomm; }
            set { _Isconsolecomm = value; }
        }

        private bool _Isconsolecomm;

		public string StartupURL
		{
			get { return _StartupURL; }
			set { _StartupURL = value; }
		}

		private string _StartupURL;

        
    }
}
