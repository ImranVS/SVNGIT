
namespace VSWebDO
{
    public class MSServerSettings
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public MSServerSettings()
        { }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;
        public int ServerID
        {
            get { return _ServerID; }
            set { _ServerID = value; }
        }
        private int _ServerID;


        public int ScanInterval
        {
            get { return _ScanInterval; }
            set { _ScanInterval = value; }
        }
        private int _ScanInterval;


        public int CredentialsID
        {
            get { return _CredentialsID; }
            set { _CredentialsID = value; }
        }
        private int _CredentialsID;


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


        public int OffscanInterval
        {
            get { return _OffscanInterval; }
            set { _OffscanInterval = value; }
        }
        private int _OffscanInterval;


        public int FailuresbfrAlert
        {
            get { return _FailuresbfrAlert; }
            set { _FailuresbfrAlert = value; }
        }
        private int _FailuresbfrAlert;

        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }
        private bool _Enabled;
        public float DiskSpace
        {
            get { return _DiskSpace; }
            set { _DiskSpace = value; }
        }
        private float _DiskSpace;
        public float CpuUtilization
        {
            get { return _CpuUtilization; }
            set { _CpuUtilization = value; }
        }
        private float _CpuUtilization;
        public bool NetwrkConn
        {
            get { return _NetwrkConn; }
            set { _NetwrkConn = value; }
        }
        private bool _NetwrkConn;
        public float MemoryUsageAlert
        {
            get { return _MemoryUsageAlert; }
            set { _MemoryUsageAlert = value; }
        }
        private float _MemoryUsageAlert;
        public string IpAddresss
        {
            get { return _IpAddresss; }
            set { _IpAddresss = value; }
        }
        private string _IpAddresss;
        

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public MSServerSettings(
            int ID,
            int ServerID,
            int ScanInterval,
            int CredentialsID,
            int RetryInterval,
            int ResponseThreshold,
            int OffscanInterval,
            int FailuresbfrAlert,
            float DiskSpace,
            float CpuUtilization,
            bool NetwrkConn,
            bool Enabled,
            float MemoryUsageAlert,
            string IpAddress
            )
        {
            this._ID = ID;
            this._ServerID = ServerID;
            this._ScanInterval = ScanInterval;
            this._CredentialsID = CredentialsID;
            this._Enabled = Enabled;
            this._CpuUtilization = CpuUtilization;
            this._DiskSpace = DiskSpace;
            this._FailuresbfrAlert = FailuresbfrAlert;
            this._IpAddresss = IpAddress;
            this._ResponseThreshold = ResponseThreshold;
            this._RetryInterval = RetryInterval;
            this._OffscanInterval = OffscanInterval;
            this._NetwrkConn = NetwrkConn;
            this._IpAddresss = IpAddress;
            this._MemoryUsageAlert = MemoryUsageAlert;

        }
    }
}
