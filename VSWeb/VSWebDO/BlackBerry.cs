public class BlackBerry
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public BlackBerry()
	{}

    public bool Enabled
    {
        get
        {
            return _Enabled;
        }
        set
        {
            _Enabled = value;

        }
    }
    private bool _Enabled;
	public string Name
	{ 
		get { return _Name; }
		set { _Name = value; }
	}
	private string _Name;


	public string NotesMailAddress
	{ 
		get { return _NotesMailAddress; }
		set { _NotesMailAddress = value; }
	}
	private string _NotesMailAddress;


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


	public int DeliveryThreshold
	{ 
		get { return _DeliveryThreshold; }
		set { _DeliveryThreshold = value; }
	}
	private int _DeliveryThreshold;


	public int RetryInterval
	{ 
		get { return _RetryInterval; }
		set { _RetryInterval = value; }
	}
	private int _RetryInterval;


	public int DestinationServerID
	{ 
		get { return _DestinationServerID; }
		set { _DestinationServerID = value; }
	}
	private int _DestinationServerID;


	public string DestinationDatabase
	{ 
		get { return _DestinationDatabase; }
		set { _DestinationDatabase = value; }
	}
	private string _DestinationDatabase;


	public string InternetMailAddress
	{ 
		get { return _InternetMailAddress; }
		set { _InternetMailAddress = value; }
	}
	private string _InternetMailAddress;


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


	public string SourceServer
	{ 
		get { return _SourceServer; }
		set { _SourceServer = value; }
	}
	private string _SourceServer;


	public int ConfirmationServerID
	{ 
		get { return _ConfirmationServerID; }
		set { _ConfirmationServerID = value; }
	}
	private int _ConfirmationServerID;


	public string ConfirmationDatabase
	{ 
		get { return _ConfirmationDatabase; }
		set { _ConfirmationDatabase = value; }
	}
	private string _ConfirmationDatabase;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public BlackBerry(string Name, 
		string NotesMailAddress, 
		string Category, 
		int ScanInterval, 
		int OffHoursScanInterval, 
		int DeliveryThreshold, 
		int RetryInterval, 
		int DestinationServerID, 
		string DestinationDatabase, 
		string InternetMailAddress, 
		System.DateTime NextScan, 
		System.DateTime LastChecked, 
		string LastStatus, 
		string SourceServer, 
		int ConfirmationServerID, 
		string ConfirmationDatabase)
	{
		this._Name = Name;
		this._NotesMailAddress = NotesMailAddress;
		this._Category = Category;
		this._ScanInterval = ScanInterval;
		this._OffHoursScanInterval = OffHoursScanInterval;
		this._DeliveryThreshold = DeliveryThreshold;
		this._RetryInterval = RetryInterval;
		this._DestinationServerID = DestinationServerID;
		this._DestinationDatabase = DestinationDatabase;
		this._InternetMailAddress = InternetMailAddress;
		this._NextScan = NextScan;
		this._LastChecked = LastChecked;
		this._LastStatus = LastStatus;
		this._SourceServer = SourceServer;
		this._ConfirmationServerID = ConfirmationServerID;
		this._ConfirmationDatabase = ConfirmationDatabase;
	}
}
