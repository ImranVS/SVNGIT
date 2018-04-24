public class IBMConnectionsServers
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public IBMConnectionsServers()
	{ }
	public string Name
	{
		get
		{
			return _Name;
		}
		set
		{
			_Name = value;
		}
	}
	private string _Name;

	public int SID
	{
		get { return _SID; }
		set { _SID = value; }
	}
	private int _SID;

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


	public int ID
	{
		get { return _ID; }
		set { _ID = value; }
	}
	private int _ID;


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


	public string IPAddress
	{
		get { return _IPAddress; }
		set { _IPAddress = value; }
	}
	private string _IPAddress;


	public bool nserver
	{
		get { return _nserver; }
		set { _nserver = value; }
	}
	private bool _nserver;
   public bool SSL
	{
		get { return _SSL; }
		set { _SSL = value; }
	}
	private bool _SSL;

 public bool stconference
	{
		get { return _stconference; }
		set { _stconference = value; }
	}
	private bool _stconference;

	public int FailureThreshold
	{
		get { return _FailureThreshold; }
		set { _FailureThreshold = value; }
	}
	private int _FailureThreshold;

	public int CredentialID
	{
		get { return _CredentialID; }
		set { _CredentialID = value; }
	}
	private int _CredentialID;

	public int ChatUser1Credentials
	{
		get { return _chatUser1Credentials; }
		set { _chatUser1Credentials = value; }
	}
	private int _chatUser1Credentials;

	public int ChatUser2Credentials
	{
		get { return _chatUser2Credentials; }
		set { _chatUser2Credentials = value; }
	}
	private int _chatUser2Credentials;
	public bool TestChatSimulation
	{
		get { return _TestChatSimulation; }
		set { _TestChatSimulation = value; }
	}
	private bool _TestChatSimulation;
	public string ProxyServerType
	{
		get { return _ProxyServerType; }
		set { _ProxyServerType = value; }
	}
	private string _ProxyServerType;

	public string ProxyServerProtocall
	{
		get { return _ProxyServerProtocall; }
		set { _ProxyServerProtocall = value; }
	}
	private string _ProxyServerProtocall;

	public string DBHostName
	{
		get { return _DBHostName; }
		set { _DBHostName = value; }
	}
	private string _DBHostName;


	public string DBName
	{
		get { return _DBName; }
		set { _DBName = value; }
	}
	private string _DBName;

	public bool EnableDB2port
	{
		get { return _EnableDB2port; }
		set { _EnableDB2port = value; }
	}
	private bool _EnableDB2port;
	public string DBPort
	{
		get { return _DBPort; }
		set { _DBPort = value; }
	}
	private string _DBPort;

	public string URL
	{
		get { return _URL; }
		set { _URL = value; }
	}
	private string _URL;
	public int PortNumber
	{
		get { return _PortNumber; }
		set { _PortNumber = value; }
	}
	private int _PortNumber;

	public int WSCellID
	{
		get { return _WSCellID; }
		set { _WSCellID = value; }
	}
	private int _WSCellID;

	public int DBCredentialsID
	{
		get { return _DBCredentialsID; }
		set { _DBCredentialsID = value; }
	}
	private int _DBCredentialsID;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public IBMConnectionsServers(string Name,
		string Description,
		string Category,
		
		bool Enabled,
		int ScanInterval,
		int OffHoursScanInterval,
		string Location,
		int ID,
		int RetryInterval,
		int ResponseThreshold,
		string IPAddress,
	    bool SSL,
		int SID,
		string Credential,
	   int CredentialID,
       int FailureThreshold,
       int chatUser1Credentials,
       int chatUser2Credentials,
      bool testChatSimulation,
		string ProxyServerType,
		string ProxyServerProtocall,
		string DBHostName,

		string DBName,
		bool EnableDB2port,
	  string DBPort,
		int PortNumber,
		string URL,
		int WSCellID,
		int DBCredentialsID
	)
	{
		this._SID = SID;
		this._Name = Name;
		this._Description = Description;
		this._Category = Category;
		this._Enabled = Enabled;
		this._ScanInterval = ScanInterval;
		this._OffHoursScanInterval = OffHoursScanInterval;
		this._Location = Location;
		this._ID = ID;
		this._RetryInterval = RetryInterval;
		this._ResponseThreshold = ResponseThreshold;
		this._IPAddress = IPAddress;
		this._nserver = nserver;
	    this._SSL = SSL;
		this._chatUser1Credentials = chatUser1Credentials;
		this._chatUser2Credentials = chatUser2Credentials;
		this._TestChatSimulation = TestChatSimulation;
		this._ProxyServerProtocall = ProxyServerProtocall;
		this._ProxyServerType = ProxyServerType;
		this._DBHostName = DBHostName;
		this._DBPort = DBPort;
		this._DBName = DBName;
		this._URL = URL;
		this._PortNumber = PortNumber;
		this._WSCellID = WSCellID;
		this.DBCredentialsID = DBCredentialsID;



	}
}
