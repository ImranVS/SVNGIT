public class NotesDatabases
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public NotesDatabases()
	{}

    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }
    private string _Name;

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


	public string FileName
	{ 
		get { return _FileName; }
		set { _FileName = value; }
	}
	private string _FileName;


	public string TriggerType
	{ 
		get { return _TriggerType; }
		set { _TriggerType = value; }
	}
	private string _TriggerType;


	public float TriggerValue
	{ 
		get { return _TriggerValue; }
		set { _TriggerValue = value; }
	}
	private float _TriggerValue;


	public string AboveBelow
	{ 
		get { return _AboveBelow; }
		set { _AboveBelow = value; }
	}
	private string _AboveBelow;


	public string ReplicationDestination
	{ 
		get { return _ReplicationDestination; }
		set { _ReplicationDestination = value; }
	}
	private string _ReplicationDestination;

    public string ServerName
    {
        get { return _ServerName; }
        set { _ServerName = value; }
    }
    private string _ServerName;


	public bool InitiateReplication
	{ 
		get { return _InitiateReplication; }
		set { _InitiateReplication = value; }
	}
	private bool _InitiateReplication;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public NotesDatabases(string Category, 
		int ScanInterval, 
		int OffHoursScanInterval, 
		bool Enabled, 
		int ResponseThreshold, 
		int RetryInterval, 
		int ID,
        int ServerID, 
		string FileName, 
		string TriggerType, 
		float TriggerValue, 
		string AboveBelow, 
		string ReplicationDestination, 
		bool InitiateReplication,
        string ServerName)
	{
		this._Category = Category;
		this._ScanInterval = ScanInterval;
		this._OffHoursScanInterval = OffHoursScanInterval;
		this._Enabled = Enabled;
		this._ResponseThreshold = ResponseThreshold;
		this._RetryInterval = RetryInterval;
		this._ID = ID;
        this._ServerID = ServerID;
		this._FileName = FileName;
		this._TriggerType = TriggerType;
		this._TriggerValue = TriggerValue;
		this._AboveBelow = AboveBelow;
		this._ReplicationDestination = ReplicationDestination;
		this._InitiateReplication = InitiateReplication;
        this._ServerName = ServerName;

	}
}
