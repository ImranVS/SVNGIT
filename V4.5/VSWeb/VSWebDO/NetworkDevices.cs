public class NetworkDevices
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public NetworkDevices()
	{}

    public int ID
    {
        get { return _ID; }
        set { _ID = value; }
    
    }

    private int _ID;
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


	public int Port
	{ 
		get { return _Port; }
		set { _Port = value; }
	}
	private int _Port;


	public string Username
	{ 
		get { return _Username; }
		set { _Username = value; }
	}
	private string _Username;


	public string Password
	{ 
		get { return _Password; }
		set { _Password = value; }
	}
	private string _Password;


	public int ScanningInterval
	{ 
		get { return _ScanningInterval; }
		set { _ScanningInterval = value; }
	}
	private int _ScanningInterval;


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

	public bool IncludeOnDashBoard
	{
		get { return _IncludeOnDashBoard; }
		set { _IncludeOnDashBoard = value; }
	}
	private bool _IncludeOnDashBoard;


	public string Location
	{ 
		get { return _Location; }
		set { _Location = value; }
	}
	private string _Location;


	public string Name
	{ 
		get { return _Name; }
		set { _Name = value; }
	}
	private string _Name;

	public string imagename
	{
		get { return _imagename; }
		set { _imagename = value; }
	}
	private string _imagename;
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
    public string Address
    {
        get { return _Address; }
        set { _Address = value; }
    
    }
    private string _Address;
	public string ImageURL
	{
		get { return _ImageURL; }
		set { _ImageURL = value; }

	}
	private string _ImageURL;


    public int LocationID
    {
        get { return _LocationID; }
        set { _LocationID = value; }

    }
    private int _LocationID;

    public int ServerTypeId
    {
        get { return _ServerTypeId; }
        set { _ServerTypeId = value; }

    }
    private int _ServerTypeId;

	public string NetworkType
	{
		get { return _NetworkType; }
		set { _NetworkType = value; }

	}
	private string _NetworkType;
	/// <summary>
	/// User defined Contructor
	/// <summary>
	public NetworkDevices(int _ID,
        string Description, 
		string Category, 
		int Port, 
		string Username, 
		string Password, 
		int ScanningInterval, 
		int OffHoursScanInterval, 
		System.DateTime NextScan, 
		System.DateTime LastChecked, 
		string LastStatus, 
		bool Enabled,
		bool IncludeOnDashBoard,
		string Location, 
		string Name, 
		int ResponseThreshold, 
		int RetryInterval,
        string Address,
        int LocationID, 
        int ServerTypeId,
		string NetworkType)
    {
        this._ID = ID;
		this._Description = Description;
		this._Category = Category;
		this._Port = Port;
		this._Username = Username;
		this._Password = Password;
		this._ScanningInterval = ScanningInterval;
		this._OffHoursScanInterval = OffHoursScanInterval;
		this._NextScan = NextScan;
		this._LastChecked = LastChecked;
		this._LastStatus = LastStatus;
		this._Enabled = Enabled;
		this._IncludeOnDashBoard = IncludeOnDashBoard;
		this._Location = Location;
		this._Name = Name;
		this._ResponseThreshold = ResponseThreshold;
		this._RetryInterval = RetryInterval;
        this._Address = Address;
        this._LocationID = LocationID;
        this._ServerTypeId = ServerTypeId;
		this._NetworkType = NetworkType;
	}
}
