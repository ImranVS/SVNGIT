public class Status
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public Status()
	{}
    public string Type
    {
        get
        {
            return _Type;
        }
        set { _Type = value; }
    
    }
    private string _Type;

	public string Location
	{ 
		get { return _Location; }
		set { _Location = value; }
	}
	private string _Location;


	public string Category
	{ 
		get { return _Category; }
		set { _Category = value; }
	}
	private string _Category;


	public string Name
	{ 
		get { return _Name; }
		set { _Name = value; }
	}
	private string _Name;


	public string sStatus
	{ 
		get { return _Status; }
		set { _Status = value; }
	}
	private string _Status;


	public string Details
	{ 
		get { return _Details; }
		set { _Details = value; }
	}
	private string _Details;


	public System.DateTime LastUpdate
	{ 
		get { return _LastUpdate; }
		set { _LastUpdate = value; }
	}
	private System.DateTime _LastUpdate;


	public string Description
	{ 
		get { return _Description; }
		set { _Description = value; }
	}
	private string _Description;


	public int PendingMail
	{ 
		get { return _PendingMail; }
		set { _PendingMail = value; }
	}
	private int _PendingMail;


	public int DeadMail
	{ 
		get { return _DeadMail; }
		set { _DeadMail = value; }
	}
	private int _DeadMail;


	public string MailDetails
	{ 
		get { return _MailDetails; }
		set { _MailDetails = value; }
	}
	private string _MailDetails;


	public int Upcount
	{ 
		get { return _Upcount; }
		set { _Upcount = value; }
	}
	private int _Upcount;


	public int DownCount
	{ 
		get { return _DownCount; }
		set { _DownCount = value; }
	}
	private int _DownCount;


	public float UpPercent
	{ 
		get { return _UpPercent; }
		set { _UpPercent = value; }
	}
	private float _UpPercent;


	public int ResponseTime
	{ 
		get { return _ResponseTime; }
		set { _ResponseTime = value; }
	}
	private int _ResponseTime;


	public int ResponseThreshold
	{ 
		get { return _ResponseThreshold; }
		set { _ResponseThreshold = value; }
	}
	private int _ResponseThreshold;


	public int PendingThreshold
	{ 
		get { return _PendingThreshold; }
		set { _PendingThreshold = value; }
	}
	private int _PendingThreshold;


	public int DeadThreshold
	{ 
		get { return _DeadThreshold; }
		set { _DeadThreshold = value; }
	}
	private int _DeadThreshold;


	public int UserCount
	{ 
		get { return _UserCount; }
		set { _UserCount = value; }
	}
	private int _UserCount;


	public float MyPercent
	{ 
		get { return _MyPercent; }
		set { _MyPercent = value; }
	}
	private float _MyPercent;


	public System.DateTime NextScan
	{ 
		get { return _NextScan; }
		set { _NextScan = value; }
	}
	private System.DateTime _NextScan;


	public string DominoServerTasks
	{ 
		get { return _DominoServerTasks; }
		set { _DominoServerTasks = value; }
	}
	private string _DominoServerTasks;


	public string TypeANDName
	{ 
		get { return _TypeANDName; }
		set { _TypeANDName = value; }
	}
	private string _TypeANDName;


	public int Icon
	{ 
		get { return _Icon; }
		set { _Icon = value; }
	}
	private int _Icon;


	public string OperatingSystem
	{ 
		get { return _OperatingSystem; }
		set { _OperatingSystem = value; }
	}
	private string _OperatingSystem;


	public string DominoVersion
	{ 
		get { return _DominoVersion; }
		set { _DominoVersion = value; }
	}
	private string _DominoVersion;


	public float UpMinutes
	{ 
		get { return _UpMinutes; }
		set { _UpMinutes = value; }
	}
	private float _UpMinutes;


	public float DownMinutes
	{ 
		get { return _DownMinutes; }
		set { _DownMinutes = value; }
	}
	private float _DownMinutes;


	public float UpPercentMinutes
	{ 
		get { return _UpPercentMinutes; }
		set { _UpPercentMinutes = value; }
	}
	private float _UpPercentMinutes;


	public float PercentageChange
	{ 
		get { return _PercentageChange; }
		set { _PercentageChange = value; }
	}
	private float _PercentageChange;


	public float CPU
	{ 
		get { return _CPU; }
		set { _CPU = value; }
	}
	private float _CPU;


	public float HeldMail
	{ 
		get { return _HeldMail; }
		set { _HeldMail = value; }
	}
	private float _HeldMail;


	public float HeldMailThreshold
	{ 
		get { return _HeldMailThreshold; }
		set { _HeldMailThreshold = value; }
	}
	private float _HeldMailThreshold;


	public int Severity
	{ 
		get { return _Severity; }
		set { _Severity = value; }
	}
	private int _Severity;


	public float Memory
	{ 
		get { return _Memory; }
		set { _Memory = value; }
	}
	private float _Memory;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public Status(string Type,
        string Location, 
		string Category, 
		string Name, 
		string Status, 
		string Details, 
		System.DateTime LastUpdate, 
		string Description, 
		int PendingMail, 
		int DeadMail, 
		string MailDetails, 
		int Upcount, 
		int DownCount, 
		float UpPercent, 
		int ResponseTime, 
		int ResponseThreshold, 
		int PendingThreshold, 
		int DeadThreshold, 
		int UserCount, 
		float MyPercent, 
		System.DateTime NextScan, 
		string DominoServerTasks, 
		string TypeANDName, 
		int Icon, 
		string OperatingSystem, 
		string DominoVersion, 
		float UpMinutes, 
		float DownMinutes, 
		float UpPercentMinutes, 
		float PercentageChange, 
		float CPU, 
		float HeldMail, 
		float HeldMailThreshold, 
		int Severity, 
		float Memory)
	{
        this._Type = Type;
		this._Location = Location;
		this._Category = Category;
		this._Name = Name;
		this._Status = Status;
		this._Details = Details;
		this._LastUpdate = LastUpdate;
		this._Description = Description;
		this._PendingMail = PendingMail;
		this._DeadMail = DeadMail;
		this._MailDetails = MailDetails;
		this._Upcount = Upcount;
		this._DownCount = DownCount;
		this._UpPercent = UpPercent;
		this._ResponseTime = ResponseTime;
		this._ResponseThreshold = ResponseThreshold;
		this._PendingThreshold = PendingThreshold;
		this._DeadThreshold = DeadThreshold;
		this._UserCount = UserCount;
		this._MyPercent = MyPercent;
		this._NextScan = NextScan;
		this._DominoServerTasks = DominoServerTasks;
		this._TypeANDName = TypeANDName;
		this._Icon = Icon;
		this._OperatingSystem = OperatingSystem;
		this._DominoVersion = DominoVersion;
		this._UpMinutes = UpMinutes;
		this._DownMinutes = DownMinutes;
		this._UpPercentMinutes = UpPercentMinutes;
		this._PercentageChange = PercentageChange;
		this._CPU = CPU;
		this._HeldMail = HeldMail;
		this._HeldMailThreshold = HeldMailThreshold;
		this._Severity = Severity;
		this._Memory = Memory;
	}
}
