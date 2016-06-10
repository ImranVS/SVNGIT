using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
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


		public bool CloudApplications
		{
			get { return _CloudApplications; }
			set { _CloudApplications = value; }
		}

		private bool _CloudApplications;

		public bool OnPremisesApplications
		{
			get { return _OnPremisesApplications; }
			set { _OnPremisesApplications = value; }
		}

		private bool _OnPremisesApplications;


		public bool NetworkInfrastucture
		{
			get { return _NetworkInfrastucture; }
			set { _NetworkInfrastucture = value; }
		}

		private bool _NetworkInfrastucture;

		public bool DominoServerMetrics
		{
			get { return _DominoServerMetrics; }
			set { _DominoServerMetrics = value; }
		}

		private bool _DominoServerMetrics;
		public bool IsFirstTimeLogin
		{
			get { return _IsFirstTimeLogin; }
			set { _IsFirstTimeLogin = value; }
		}

		private bool _IsFirstTimeLogin;



        public string CustomBackground
        {
            get {return _CustomBackground; }
            set {_CustomBackground = value; }
        }
        private string _CustomBackground;
        public int cloudindex
        {
            get { return _cloudindex; }
            set { _cloudindex = value; }
        }
        private int _cloudindex;
        public int premisesindex
        {
            get { return _premisesindex; }
            set { _premisesindex = value; }
        }
        private int _premisesindex;
        public int networkindex
        {
            get { return _networkindex; }
            set { _networkindex = value; }
        }
        private int _networkindex;
        public int dockindex
        {
            get { return _dockindex; }
            set { _dockindex = value; }
        }
        private int _dockindex;
        public string cloudZone
        {
			get { return _cloudZone; }
			set { _cloudZone = value; }
        }
		private string _cloudZone;
        public string premisesZone
        {
            get { return _premisesZone; }
            set { _premisesZone = value; }
        }
        private string _premisesZone;
        public string networkZone
        {
            get { return _networkZone; }
            set { _networkZone = value; }
        }
        private string _networkZone;
        public string DockZone
        {
            get { return _DockZone; }
            set { _DockZone = value; }
        }
        private string _DockZone;
		//2/11/2016 Durga Added for VSPLUS 2595
		public string StatusZone
		{
			get { return _StatusZone; }
			set { _StatusZone = value; }
		}
		private string _StatusZone;
		public string KeyUserDevicesZone
		{
			get { return _KeyUserDevicesZone; }
			set { _KeyUserDevicesZone = value; }
		}
		private string _KeyUserDevicesZone;
		public string TravelerZone
		{
			get { return _TravelerZone; }
			set { _TravelerZone = value; }
		}
		private string _TravelerZone;
		public int TravelerIndex
		{
			get { return _TravelerIndex; }
			set { _TravelerIndex = value; }
		}
		private int _TravelerIndex;
		public int KeyUserDevicesIndex
		{
			get { return _KeyUserDevicesIndex; }
			set { _KeyUserDevicesIndex = value; }
		}
		private int _KeyUserDevicesIndex;
		public int StatusIndex
		{
			get { return _StatusIndex; }
			set { _StatusIndex = value; }
		}
		private int _StatusIndex;

	}
}
