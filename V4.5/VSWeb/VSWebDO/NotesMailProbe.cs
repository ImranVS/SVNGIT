public class NotesMailProbe
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public NotesMailProbe()
	{}

    public bool Enabled
    {
        get { return _Enabled; }
        set { _Enabled = value; }
            
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


	public string SourceServer
	{ 
		get { return _SourceServer; }
		set { _SourceServer = value; }
	}
	private string _SourceServer;


	public bool EchoService
	{ 
		get { return _EchoService; }
		set { _EchoService = value; }
	}
	private bool _EchoService;


	public string ReplyTo
	{ 
		get { return _ReplyTo; }
		set { _ReplyTo = value; }
	}
	private string _ReplyTo;


	public string Filename
	{ 
		get { return _Filename; }
		set { _Filename = value; }
	}
	private string _Filename;

    //12/7/2012 NS added ServerName variable to store Target Server info
    public string ServerName
    {
        get { return _ServerName; }
        set { _ServerName = value; }
    }
    private string _ServerName;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public NotesMailProbe(bool Enabled,
        string Name, 
		string NotesMailAddress, 
		string Category, 
		int ScanInterval, 
		int OffHoursScanInterval, 
		int DeliveryThreshold, 
		int RetryInterval, 
		int DestinationServerID, 
		string DestinationDatabase, 
		string SourceServer, 
		bool EchoService, 
		string ReplyTo, 
		string Filename,
        string ServerName)
	{
        this._Enabled = Enabled;
		this._Name = Name;
		this._NotesMailAddress = NotesMailAddress;
		this._Category = Category;
		this._ScanInterval = ScanInterval;
		this._OffHoursScanInterval = OffHoursScanInterval;
		this._DeliveryThreshold = DeliveryThreshold;
		this._RetryInterval = RetryInterval;
		this._DestinationServerID = DestinationServerID;
		this._DestinationDatabase = DestinationDatabase;
		this._SourceServer = SourceServer;
		this._EchoService = EchoService;
		this._ReplyTo = ReplyTo;
		this._Filename = Filename;
        this.ServerName = ServerName;
	}
}
