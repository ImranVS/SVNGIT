public class MailServices
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public MailServices()
	{}
    public string Address
    {
        get { return _Address; }
        set { _Address = value; }
    }
    private string _Address;

    //public int ID
    //{
    //    get { return _ID; }
    //    set { _ID = value; }
    //}
    //private int _ID;
	public string Name
	{ 
		get { return _Name; }
		set { _Name = value; }
	}
	private string _Name;

    //public string ServerName
    //{
    //    get { return _ServerName; }
    //    set { _ServerName = value; }
    //}
    //private string _ServerName;


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


	public short Port
	{ 
		get { return _Port; }
		set { _Port = value; }
	}
	private short _Port;


	public short FailureThreshold
	{ 
		get { return _FailureThreshold; }
		set { _FailureThreshold = value; }
	}
	private short _FailureThreshold;

    public string Status
    {
        get { return _status; }
        set { _status = value; }
    }
    private string _status;

    public int LocationId
    {
        get { return _LocationId; }
        set { _LocationId = value; }
    }
    private int _LocationId;

    public string LocationText
    {
        get { return _LocationText; }
        set { _LocationText = value; }

    }
    private string _LocationText;
    public int ServerTypeId
    {
        get { return _ServerTypeId; }
        set { _ServerTypeId = value; }

    }
    private int _ServerTypeId;
	/// <summary>
	/// User defined Contructor
	/// <summary>
	public MailServices(string Address,
        string Name, 
		string Description, 
		string Category, 
		int ScanInterval, 
		int OffHoursScanInterval, 
		System.DateTime NextScan, 
		System.DateTime LastChecked, 
		string LastStatus, 
		bool Enabled, 
		int ResponseThreshold, 
		int RetryInterval, 
		int key, 
		short Port, 
		short FailureThreshold,
        //int ID,
        string status,string ServerName,
        int LocationId, string LocationText,
        int ServerTypeId)
	{
        this._Address = Address;
		this._Name = Name;
		this._Description = Description;
		this._Category = Category;
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
		this._FailureThreshold = FailureThreshold;
        //this._ID = ID;
        this._status = status;
        //this._ServerName = ServerName;
        this._LocationId = LocationId;
        this._LocationText = LocationText;
        this._ServerTypeId = ServerTypeId;
	}
}
