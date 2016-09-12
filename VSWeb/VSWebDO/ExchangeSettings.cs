
namespace VSWebDO
{
    public class ExchangeSettings
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public ExchangeSettings()
        { }


        public int ServerID
        {
            get { return _ServerID; }
            set { _ServerID = value; }
        }
        private int _ServerID;

       
        public bool CASSmtp
        {
            get { return _CASSmtp; }
            set { _CASSmtp = value; }
        }
        private bool _CASSmtp;


        public bool CASPop3
        {
            get { return _CASPop3; }
            set { _CASPop3 = value; }
        }
        private bool _CASPop3;


        public bool CASImap
        {
            get { return _CASImap; }
            set { _CASImap = value; }
        }
        private bool _CASImap;

        public bool CASOARPC
        {
            get { return _CASOARPC; }
            set { _CASOARPC = value; }
        }
        private bool _CASOARPC;
        public bool CASOWA
        {
            get { return _CASOWA; }
            set { _CASOWA = value; }
        }
        private bool _CASOWA;

        public bool CASActiveSync
        {
            get { return _CASActiveSync; }
            set { _CASActiveSync = value; }
        }
        private bool _CASActiveSync;

        public bool CASEWS
        {
            get { return _CASEWS; }
            set { _CASEWS = value; }
        }
        private bool _CASEWS;

        public bool CASECP
        {
            get { return _CASECP; }
            set { _CASECP = value; }
        }
        private bool _CASECP;

        public bool CASAutoDiscovery
        {
            get { return _CASAutoDiscovery; }
            set { _CASAutoDiscovery = value; }
        }
        private bool _CASAutoDiscovery;


        public bool CASOAB
        {
            get { return _CASOAB; }
            set { _CASOAB = value; }
        }
        private bool _CASOAB;

        public int SubQThreshold
        {
            get { return _SubQThreshold; }
            set { _SubQThreshold = value; }
        }
        private int _SubQThreshold;

        public int PoisonQThreshold
        {
            get { return _PoisonQThreshold; }
            set { _PoisonQThreshold = value; }
        }
        private int _PoisonQThreshold;

        public int UnReachableQThreshold
        {
            get { return _UnReachableQThreshold; }
            set { _UnReachableQThreshold = value; }
        }
        private int _UnReachableQThreshold;

		public int ShadowQThreshold
		{
			get { return _ShadowQThreshold; }
			set { _ShadowQThreshold = value; }
		}
		private int _ShadowQThreshold;

        public int TotalQThreshold
        {
            get { return _TotalQThreshold; }
            set { _TotalQThreshold = value; }
        }
        private int _TotalQThreshold;
		public int LatencyYellowThreshold
		{
			get { return _LatencyYellowThreshold; }
			set { _LatencyYellowThreshold = value; }
		}
		private int _LatencyYellowThreshold;
		public int LatencyRedThreshold
		{
			get { return _LatencyRedThreshold; }
			set { _LatencyRedThreshold = value; }
		}
		private int _LatencyRedThreshold;

        public bool EnableLatencyTest
        {
            get { return _EnableLatencyTest; }
            set { _EnableLatencyTest = value; }
        }
        private bool _EnableLatencyTest;


		//public int EdgeSubQThreshold
		//{
		//    get { return _EdgeSubQThreshold; }
		//    set { _EdgeSubQThreshold = value; }
		//}
		//private int _EdgeSubQThreshold;

		//public int EdgePosQThreshold
		//{
		//    get { return _EdgePosQThreshold; }
		//    set { _EdgePosQThreshold = value; }
		//}
		//private int _EdgePosQThreshold;

		//public int EdgeUnReachableQThreshold
		//{
		//    get { return _EdgeUnReachableQThreshold; }
		//    set { _EdgeUnReachableQThreshold = value; }
		//}
		//private int _EdgeUnReachableQThreshold;

		//public int EdgeTotalQThreshold
		//{
		//    get { return _EdgeTotalQThreshold; }
		//    set { _EdgeTotalQThreshold = value; }
		//}
		//private int _EdgeTotalQThreshold;

        public string VersionNo
        {
            get { return _VersionNo; }
            set { _VersionNo = value; }
        }
        private string _VersionNo;

		public int ACCredentialsId
		{
			get { return _ACCredentialsId; }
			set { _ACCredentialsId = value; }
		}
		private int _ACCredentialsId;

		public string AuthenticationType
		{
			get { return _AuthenticationType; }
			set { _AuthenticationType = value; }
		}
		private string _AuthenticationType;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public ExchangeSettings(
         bool CASSmtp ,
	bool CASPop3 ,
	bool CASImap ,
	bool CASOARPC ,
	bool CASOWA,
	bool CASActiveSync ,
	bool CASEWS ,
	bool CASECP ,
	bool CASAutoDiscovery ,
	bool CASOAB ,
	int SubQThreshold ,
	int PoisonQThreshold ,
	int UnReachableQThreshold ,
	int TotalQThreshold ,
	int LatencyYellowThreshold,
	int LatencyRedThreshold,
	string VersionNo,
	int ACCredentialsId
            )
        {
            this._ServerID = ServerID;
            this._CASSmtp  = CASSmtp ;
            this._CASPop3  = CASPop3 ;
            this._CASImap  = CASImap ;
            this._CASOARPC  = CASOARPC ;
            this._CASOWA = CASOWA;
            this._CASActiveSync  = CASActiveSync ;
            this._CASEWS  = CASEWS ;
            this._CASECP  = CASECP ;
            this._CASAutoDiscovery  = CASAutoDiscovery ;
            this._CASOAB  = CASOAB ;
            this._SubQThreshold  = SubQThreshold ;
            this._PoisonQThreshold  = PoisonQThreshold ;
            this._UnReachableQThreshold  = UnReachableQThreshold ;
            this._TotalQThreshold  = TotalQThreshold ;
			this._LatencyYellowThreshold = LatencyYellowThreshold;
			this._LatencyRedThreshold = LatencyRedThreshold;
            this._VersionNo = VersionNo;
			this._ACCredentialsId = ACCredentialsId;

        }

        //14/07/2016 sowmya added for VSPLUS-3097
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _id;

        public int ServerId
        {
            get { return _ServerId; }
            set { _ServerId = value; }
        }
        private int _ServerId;

        public int TestId
        {
            get { return _TestId; }
            set { _TestId = value; }
        }
        private int _TestId;

        public string URLs
        {
            get { return _URLs; }
            set { _URLs = value; }
        }
        private string _URLs;

        public int CredentialsId
        {
            get { return _CredentialsId; }
            set { _CredentialsId = value; }
        }
        private int _CredentialsId;
        public string TestName
        {
            get { return _TestName; }
            set { _TestName = value; }
        }
        private string _TestName; 
    }
}
