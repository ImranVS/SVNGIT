namespace VSWebDO
{
    public class ExchangeServers
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public ExchangeServers()
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

		public int DAGPrimaryServerId
		{
			get { return _DAGPrimaryServerID; }
			set { _DAGPrimaryServerID = value; }
		}
		private int _DAGPrimaryServerID;

		public int DAGBackUpServerID
		{
			get { return _DAGBackUpServerID; }
			set { _DAGBackUpServerID = value; }
		}
		private int _DAGBackUpServerID;

		public int DAGResponseQTh
		{
			get { return _DAGResponseQTh; }
			set { _DAGResponseQTh = value; }
		}
		private int _DAGResponseQTh;

		public int DAGCopyQTh
		{
			get { return _DAGCopyQTh; }
			set { _DAGCopyQTh = value; }
		}
		private int _DAGCopyQTh;

		public string AuthenticationType
		{
			get { return _AuthenticationType; }
			set { _AuthenticationType = value; }
		}
		private string _AuthenticationType;

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
		public ExchangeServers(string Description,
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
            string NotificationGroup,
            float Memory_Threshold,
            float CPU_Threshold,
            float Cluster_Rep_Delays_Threshold,
            //int Modified_By,
            //string Modified_On,
             int ServerID, int LocationID,
            int ServerDaysAlert,
			bool RequireSSL,
			string ExternalAlias,
            int CredentialsID)
        {
            this._Description = Description;
            this._Category = Category;
            this._PendingThreshold = PendingThreshold;
            this._DeadThreshold = DeadThreshold;
            this._Enabled = Enabled;
			this._RequireSSL = RequireSSL;
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
        }
    }
}