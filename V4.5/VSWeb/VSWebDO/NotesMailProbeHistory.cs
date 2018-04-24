public class NotesMailProbeHistory
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public NotesMailProbeHistory()
	{}

    public int ProbeID
    {
        get { return _ProbeID; }
        set { _ProbeID = value; }
    
    }
    private int _ProbeID;
	public System.DateTime SentDateTime
	{ 
		get { return _SentDateTime; }
		set { _SentDateTime = value; }
	}
	private System.DateTime _SentDateTime;


	public string SentTo
	{ 
		get { return _SentTo; }
		set { _SentTo = value; }
	}
	private string _SentTo;


	public int DeliveryThresholdInMinutes
	{ 
		get { return _DeliveryThresholdInMinutes; }
		set { _DeliveryThresholdInMinutes = value; }
	}
	private int _DeliveryThresholdInMinutes;


	public int DeliveryTimeInMinutes
	{ 
		get { return _DeliveryTimeInMinutes; }
		set { _DeliveryTimeInMinutes = value; }
	}
	private int _DeliveryTimeInMinutes;


	public string SubjectKey
	{ 
		get { return _SubjectKey; }
		set { _SubjectKey = value; }
	}
	private string _SubjectKey;


	public System.DateTime ArrivalAtMailBox
	{ 
		get { return _ArrivalAtMailBox; }
		set { _ArrivalAtMailBox = value; }
	}
	private System.DateTime _ArrivalAtMailBox;


	public string Status
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


	public string DeviceName
	{ 
		get { return _DeviceName; }
		set { _DeviceName = value; }
	}
	private string _DeviceName;


	public string TargetServer
	{ 
		get { return _TargetServer; }
		set { _TargetServer = value; }
	}
	private string _TargetServer;


	public string TargetDatabase
	{ 
		get { return _TargetDatabase; }
		set { _TargetDatabase = value; }
	}
	private string _TargetDatabase;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public NotesMailProbeHistory(int ProbeID,
        System.DateTime SentDateTime, 
		string SentTo, 
		int DeliveryThresholdInMinutes, 
		int DeliveryTimeInMinutes, 
		string SubjectKey, 
		System.DateTime ArrivalAtMailBox, 
		string Status, 
		string Details, 
		string DeviceName, 
		string TargetServer, 
		string TargetDatabase)
	{
        this._ProbeID = ProbeID;
		this._SentDateTime = SentDateTime;
		this._SentTo = SentTo;
		this._DeliveryThresholdInMinutes = DeliveryThresholdInMinutes;
		this._DeliveryTimeInMinutes = DeliveryTimeInMinutes;
		this._SubjectKey = SubjectKey;
		this._ArrivalAtMailBox = ArrivalAtMailBox;
		this._Status = Status;
		this._Details = Details;
		this._DeviceName = DeviceName;
		this._TargetServer = TargetServer;
		this._TargetDatabase = TargetDatabase;
	}
}
