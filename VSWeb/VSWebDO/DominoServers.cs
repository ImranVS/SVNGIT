namespace VSWebDO
{
    public class DominoServers
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public DominoServers()
        { }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Name;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        private string _Description;


        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        private string _Category;


        public int PendingThreshold
        {
            get { return _PendingThreshold; }
            set { _PendingThreshold = value; }
        }
        private int _PendingThreshold;


        public int DeadThreshold
        {
            get { return _DeadThreshold; }
            set { _DeadThreshold = value; }
        }
        private int _DeadThreshold;


        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }
        private bool _Enabled;


        public int ScanInterval
        {
            get { return _ScanInterval; }
            set { _ScanInterval = value; }
        }
        private int _ScanInterval;


        public int OffHoursScanInterval
        {
            get { return _OffHoursScanInterval; }
            set { _OffHoursScanInterval = value; }
        }
        private int _OffHoursScanInterval;


        public string Location
        {
            get { return _Location; }
            set { _Location = value; }
        }
        private string _Location;


        public int LocationID
        {
            get { return _LocationID; }
            set { _LocationID = value; }
        }

        private int _LocationID;

        public int Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        private int _Key;


        public string MailDirectory
        {
            get { return _MailDirectory; }
            set { _MailDirectory = value; }
        }
        private string _MailDirectory;


        public int RetryInterval
        {
            get { return _RetryInterval; }
            set { _RetryInterval = value; }
        }
        private int _RetryInterval;


        public int ResponseThreshold
        {
            get { return _ResponseThreshold; }
            set { _ResponseThreshold = value; }
        }
        private int _ResponseThreshold;


        public bool BES_Server
        {
            get { return _BES_Server; }
            set { _BES_Server = value; }
        }
        private bool _BES_Server;


        public int BES_Threshold
        {
            get { return _BES_Threshold; }
            set { _BES_Threshold = value; }
        }
        private int _BES_Threshold;


        public int FailureThreshold
        {
            get { return _FailureThreshold; }
            set { _FailureThreshold = value; }
        }
        private int _FailureThreshold;


        public string SearchString
        {
            get { return _SearchString; }
            set { _SearchString = value; }
        }
        private string _SearchString;


        public bool AdvancedMailScan
        {
            get { return _AdvancedMailScan; }
            set { _AdvancedMailScan = value; }
        }
        private bool _AdvancedMailScan;


        public int DeadMailDeleteThreshold
        {
            get { return _DeadMailDeleteThreshold; }
            set { _DeadMailDeleteThreshold = value; }
        }
        private int _DeadMailDeleteThreshold;


        public float DiskSpaceThreshold
        {
            get { return _DiskSpaceThreshold; }
            set { _DiskSpaceThreshold = value; }
        }
        private float _DiskSpaceThreshold;


        public string IPAddress
        {
            get { return _IPAddress; }
            set { _IPAddress = value; }
        }
        private string _IPAddress;


        public float HeldThreshold
        {
            get { return _HeldThreshold; }
            set { _HeldThreshold = value; }
        }
        private float _HeldThreshold;


        public bool ScanDBHealth
        {
            get { return _ScanDBHealth; }
            set { _ScanDBHealth = value; }
        }
        private bool _ScanDBHealth;
        public bool scanlog
        {
            get { return _scanlog; }
            set { _scanlog = value; }
        }
        private bool _scanlog;

        public bool scanagentlog
        {
            get { return _scanagentlog; }
            set { _scanagentlog = value; }
        }
        private bool _scanagentlog;


        public string NotificationGroup
        {
            get { return _NotificationGroup; }
            set { _NotificationGroup = value; }
        }
        private string _NotificationGroup;


        public float Memory_Threshold
        {
            get { return _Memory_Threshold; }
            set { _Memory_Threshold = value; }
        }
        private float _Memory_Threshold;


        public float CPU_Threshold
        {
            get { return _CPU_Threshold; }
            set { _CPU_Threshold = value; }
        }
        private float _CPU_Threshold;


        public float Cluster_Rep_Delays_Threshold
        {
            get { return _Cluster_Rep_Delays_Threshold; }
            set { _Cluster_Rep_Delays_Threshold = value; }
        }
                   
        private float _Cluster_Rep_Delays_Threshold;
        public int ServerID
        {
            get { return _ServerId; }
            set { _ServerId = value; }
        }
        private int _ServerId;

        public int ServerDaysAlert
        {
            get { return _ServerDaysAlert; }
            set { _ServerDaysAlert = value; }
        }
        private int _ServerDaysAlert;

		public bool RequireSSL
		{
			get { return _RequireSSL; }
			set { _RequireSSL = value; }
		}
		private bool _RequireSSL;

		public bool ScanTravelerServer
		{
			get { return _ScanTravelerServer; }
			set { _ScanTravelerServer = value; }
		}
		private bool _ScanTravelerServer;

		public bool EnableTravelerBackend
		{
			get { return _EnableTravelerBackend; }
			set { _EnableTravelerBackend = value; }
		}
		private bool _EnableTravelerBackend;

		public string ExternalAlias
		{
			get { return _ExternalAlias; }
			set { _ExternalAlias = value; }
		}
		private string _ExternalAlias;

        //8/7/2014 NS added for VSPLUS-853
        public int CredentialsID
        {
            get { return _CredentialsID; }
            set { _CredentialsID = value; }
        }
        private int _CredentialsID;

        public bool CheckMailThreshold
        {
            get { return _CheckMailThreshold; }
            set { _CheckMailThreshold = value; }
        }
        private bool _CheckMailThreshold;

        public bool SendRouterRestart
        {
            get { return _SendRouterRestart; }
            set { _SendRouterRestart = value; }
        }
        private bool _SendRouterRestart;

		public bool ScanServlet
		{
			get { return _ScanServlet; }
			set { _ScanServlet = value; }
		}
		private bool _ScanServlet;

		
		public string DiskInfo
		{
			get { return _DiskInfo; }
			set { _DiskInfo = value; }
		}
		private string _DiskInfo;

        public float Load_Cluster_Rep_Delays_Threshold
        {
            get { return _Load_Cluster_Rep_Delays_Threshold; }
            set { _Load_Cluster_Rep_Delays_Threshold = value; }
        }

        private float _Load_Cluster_Rep_Delays_Threshold;

        //6/18/2015 NS added for VSPLUS-1802
        public string EXJStartTime
        {
            get { return _EXJStartTime; }
            set { _EXJStartTime = value; }
        }
        private string _EXJStartTime;

        public int EXJDuration
        {
            get { return _EXJDuration; }
            set { _EXJDuration = value; }
        }
        private int _EXJDuration;

        public int EXJLookBackDuration
        {
            get { return _EXJLookBackDuration; }
            set { _EXJLookBackDuration = value; }
        }
        private int _EXJLookBackDuration;

        public bool EXJEnabled
        {
            get { return _EXJEnabled; }
            set { _EXJEnabled = value; }
        }
        private bool _EXJEnabled;

        public bool EXJLookBackEnabled
        {
            get { return _EXJLookBackEnabled; }
            set { _EXJLookBackEnabled = value; }
        }
        private bool _EXJLookBackEnabled;

        //2/23/2016 NS added for VSPLUS-2641
        public int AvailabilityIndexThreshold
        {
            get { return _AvailabilityIndexThreshold; }
            set { _AvailabilityIndexThreshold = value; }
        }
        private int _AvailabilityIndexThreshold;

        //public int Modified_By
        //{
        //    get { return _Modified_By; }
        //    set { _Modified_By= value; }
                   
        //}
        //private int _Modified_By;
        //public string Modified_On
        //{
        //    get { return _Modified_On; }
        //    set { _Modified_On = value; }
        
        //}
        //private string _Modified_On;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public DominoServers(string Description,
            string Category,
            int PendingThreshold,
            int DeadThreshold,
            bool Enabled,
			int ScanInterval,
            int OffHoursScanInterval,
            string Location,
            int Key,
            string MailDirectory,
            int RetryInterval,
            int ResponseThreshold,
            bool BES_Server,
            int BES_Threshold,
            int FailureThreshold,
            string SearchString,
            bool AdvancedMailScan,
            int DeadMailDeleteThreshold,
            float DiskSpaceThreshold,
            string IPAddress,
            float HeldThreshold,
            bool ScanDBHealth,
              bool scanlog,
              bool scanagentlog,
            string NotificationGroup,
            float Memory_Threshold,
            float CPU_Threshold,
            float Cluster_Rep_Delays_Threshold,
            //int Modified_By,
            //string Modified_On,
             int ServerID, int LocationID,
            int ServerDaysAlert,
			bool RequireSSL,
			bool EnableTravelerBackend,
			string ExternalAlias,
            int CredentialsID,
            bool CheckMailThreshold,
			bool SendRouterRestart, bool ScanTravelerServer,
            float Load_Cluster_Rep_Delays_Threshold,
            string EXJStartTime,
            int EXJDuration,
            int EXJLookBackDuration,
            bool EXJEnabled,
            bool EXJLookBackEnabled,
            int AvailabilityIndexThreshold)
        {
            this._Description = Description;
            this._Category = Category;
            this._PendingThreshold = PendingThreshold;
            this._DeadThreshold = DeadThreshold;
            this._Enabled = Enabled;
			this._RequireSSL = RequireSSL;
			this._EnableTravelerBackend = EnableTravelerBackend;
			this._ExternalAlias = ExternalAlias;
            this._ScanInterval = ScanInterval;
            this._OffHoursScanInterval = OffHoursScanInterval;
            this._Location = Location;
            this._Key = Key;
            this._MailDirectory = MailDirectory;
            this._RetryInterval = RetryInterval;
            this._ResponseThreshold = ResponseThreshold;
            this._BES_Server = BES_Server;
            this._BES_Threshold = BES_Threshold;
            this._FailureThreshold = FailureThreshold;
            this._SearchString = SearchString;
            this._AdvancedMailScan = AdvancedMailScan;
            this._DeadMailDeleteThreshold = DeadMailDeleteThreshold;
            this._DiskSpaceThreshold = DiskSpaceThreshold;
            this._IPAddress = IPAddress;
            this._HeldThreshold = HeldThreshold;
            this._ScanDBHealth = ScanDBHealth;
            this._scanlog = scanlog;
            this._scanagentlog = scanagentlog;
            this._NotificationGroup = NotificationGroup;
            this._Memory_Threshold = Memory_Threshold;
            this._CPU_Threshold = CPU_Threshold;
            this._Cluster_Rep_Delays_Threshold = Cluster_Rep_Delays_Threshold;
            //this._Modified_By = Modified_By;
            //this.Modified_On = Modified_On;
            this._ServerId = ServerID;
            this._LocationID = LocationID;
            this._ServerDaysAlert = ServerDaysAlert;
            this._CredentialsID = CredentialsID;
            this._CheckMailThreshold = CheckMailThreshold;
            this._SendRouterRestart = SendRouterRestart;
			this._ScanTravelerServer = ScanTravelerServer;
            this._Load_Cluster_Rep_Delays_Threshold = Load_Cluster_Rep_Delays_Threshold;
            //6/18/2015 NS added for VSPLUS-1802
            this._EXJStartTime = EXJStartTime;
            this._EXJDuration = EXJDuration;
            this.EXJLookBackDuration = EXJLookBackDuration;
            this._EXJEnabled = EXJEnabled;
            this._EXJLookBackEnabled = EXJLookBackEnabled;
            //2/23/2016 NS added for VSPLUS-2641
            this._AvailabilityIndexThreshold = AvailabilityIndexThreshold;
        }
    }
}