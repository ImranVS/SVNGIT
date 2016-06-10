public class DNS_Servers
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public DNS_Servers()
	{}


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


	public int QueryType
	{ 
		get { return _QueryType; }
		set { _QueryType = value; }
	}
	private int _QueryType;


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


	public System.DateTime NextScan
	{ 
		get { return _NextScan; }
		set { _NextScan = value; }
	}
	private System.DateTime _NextScan;


	public System.DateTime LastChecked
	{ 
		get { return _LastChecked; }
		set { _LastChecked = value; }
	}
	private System.DateTime _LastChecked;


	public string LastStatus
	{ 
		get { return _LastStatus; }
		set { _LastStatus = value; }
	}
	private string _LastStatus;


	public bool Enabled
	{ 
		get { return _Enabled; }
		set { _Enabled = value; }
	}
	private bool _Enabled;


	public int ResponseThreshold
	{ 
		get { return _ResponseThreshold; }
		set { _ResponseThreshold = value; }
	}
	private int _ResponseThreshold;


	public int RetryInterval
	{ 
		get { return _RetryInterval; }
		set { _RetryInterval = value; }
	}
	private int _RetryInterval;


	public int key
	{ 
		get { return _key; }
		set { _key = value; }
	}
	private int _key;


	public string Port
	{ 
		get { return _Port; }
		set { _Port = value; }
	}
	private string _Port;


	public string AcceptableReponses
	{ 
		get { return _AcceptableReponses; }
		set { _AcceptableReponses = value; }
	}
	private string _AcceptableReponses;


	public string HostToQuery
	{ 
		get { return _HostToQuery; }
		set { _HostToQuery = value; }
	}
	private string _HostToQuery;


	public string Category
	{ 
		get { return _Category; }
		set { _Category = value; }
	}
	private string _Category;


	public string A_Record_Responses
	{ 
		get { return _A_Record_Responses; }
		set { _A_Record_Responses = value; }
	}
	private string _A_Record_Responses;


	public string NS_Record_Responses
	{ 
		get { return _NS_Record_Responses; }
		set { _NS_Record_Responses = value; }
	}
	private string _NS_Record_Responses;


	public string Location
	{ 
		get { return _Location; }
		set { _Location = value; }
	}
	private string _Location;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public DNS_Servers(string Name, 
		string Description, 
		int QueryType, 
		int ScanInterval, 
		int OffHoursScanInterval, 
		System.DateTime NextScan, 
		System.DateTime LastChecked, 
		string LastStatus, 
		bool Enabled, 
		int ResponseThreshold, 
		int RetryInterval, 
		int key, 
		string Port, 
		string AcceptableReponses, 
		string HostToQuery, 
		string Category, 
		string A_Record_Responses, 
		string NS_Record_Responses, 
		string Location)
	{
		this._Name = Name;
		this._Description = Description;
		this._QueryType = QueryType;
		this._ScanInterval = ScanInterval;
		this._OffHoursScanInterval = OffHoursScanInterval;
		this._NextScan = NextScan;
		this._LastChecked = LastChecked;
		this._LastStatus = LastStatus;
		this._Enabled = Enabled;
		this._ResponseThreshold = ResponseThreshold;
		this._RetryInterval = RetryInterval;
		this._key = key;
		this._Port = Port;
		this._AcceptableReponses = AcceptableReponses;
		this._HostToQuery = HostToQuery;
		this._Category = Category;
		this._A_Record_Responses = A_Record_Responses;
		this._NS_Record_Responses = NS_Record_Responses;
		this._Location = Location;
	}
}
