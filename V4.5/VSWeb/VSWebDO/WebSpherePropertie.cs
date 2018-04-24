namespace VSWebDO
{
 
public class WebSpherePropertie
{
	/// <summary>
	/// Default Contructor
	/// <summary>



	public WebSpherePropertie()
	{ }
	public int ServerID
        {
            get { return _ServerID; }
            set { _ServerID = value; }
        }
        private int _ServerID;

		 public int NodeID
        {
            get { return _NodeID; }
            set { _NodeID = value; }
        }
        private int _NodeID;

		public int LocationId
		{
			get { return _LocationId; }
			set { _LocationId = value; }
		}
		private int _LocationId;

		 public int CellID
        {
            get { return _CellID; }
            set { _CellID = value; }
        }
        private int _CellID;

        public string ServerName
        {
            get { return _ServerName; }
            set { _ServerName = value; }
        }

        private string _ServerName;
        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        private string _Category;
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
        public int FailureThreshold
        {
            get { return _FailureThreshold; }
            set { _FailureThreshold = value; }
        }
        private int _FailureThreshold;
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

        public float AvgThreadPool
        {
            get { return _AvgThreadPool; }
            set { _AvgThreadPool = value; }
        }
        private float _AvgThreadPool;


		public float ActiveThreadCount
        {
            get { return _ActiveThreadCount; }
            set { _ActiveThreadCount = value; }
        }
		private float _ActiveThreadCount;

		 public string CurrentHeap
        {
            get { return _CurrentHeap; }
            set { _CurrentHeap = value; }
        }
        private string _CurrentHeap;
       
		 public string MaxHeap
        {
            get { return _MaxHeap; }
            set { _MaxHeap = value; }
        }
        private string _MaxHeap;
       
		 public string FreeMemory
        {
            get { return _FreeMemory; }
            set { _FreeMemory = value; }
        }
        private string _FreeMemory;

		public float CPUUtilization
        {
            get { return _CPUUtilization; }
            set { _CPUUtilization = value; }
        }
        private float _CPUUtilization;

		
		public float Uptime
        {
            get { return _Uptime; }
            set { _Uptime = value; }
        }
        private float _Uptime;

		public float HungThreadCount
        {
            get { return _HungThreadCount; }
            set { _HungThreadCount = value; }
        }
        private float _HungThreadCount;

       public string DumpGenerated
        {
            get { return _DumpGenerated; }
            set { _DumpGenerated = value; }
        }
        private string _DumpGenerated;

		public string Status
		{
			get { return _Status; }
			set { _Status = value; }
		}
		private string _Status;
		

		public WebSpherePropertie(
             string Category,
             bool Enabled,
            int ScanInterval,
            int OffHoursScanInterval,
             int RetryInterval,
            int ResponseThreshold,

               int FailureThreshold,
            int ServerID,
		    int NodeID,
            int CellID,
			int LocationId,
		    string ServerName,
		    float AvgThreadPool,
		    float	ActiveThreadCount,
		    string CurrentHeap,
		    string MaxHeap,
		    string FreeMemory,
             float Memory_Threshold,
            float CPU_Threshold,
			float CPUUtilization,
			float Uptime,
		    float	HungThreadCount,
		    string DumpGenerated,


			

			string Status
			

            )
        {
			this._ServerID = ServerID;
			this._NodeID = NodeID;
			this._LocationId = LocationId;
			this._CellID = CellID;
			this._ServerName = ServerName;
            this._Enabled = Enabled;
            this._ScanInterval = ScanInterval;
            this._OffHoursScanInterval = OffHoursScanInterval;
            this._RetryInterval = RetryInterval;
            this._ResponseThreshold = ResponseThreshold;
            this._FailureThreshold = FailureThreshold;
			this._AvgThreadPool = AvgThreadPool;
			this._ActiveThreadCount =ActiveThreadCount; 
			this._CurrentHeap = CurrentHeap;
			this._MaxHeap = MaxHeap;
            this._Memory_Threshold = Memory_Threshold;
            this._CPU_Threshold = CPU_Threshold;
			this._FreeMemory = FreeMemory;
			this._CPUUtilization = CPUUtilization;
			this._Uptime = Uptime;
			this._HungThreadCount = HungThreadCount;
			this._DumpGenerated = DumpGenerated;
			this._Status = Status;

        }
    }
}
