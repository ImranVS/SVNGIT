public class Traveler_Devices
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public Traveler_Devices()
	{}


	public string DeviceName
	{ 
		get { return _DeviceName; }
		set { _DeviceName = value; }
	}
	private string _DeviceName;


	public string ConnectionState
	{ 
		get { return _ConnectionState; }
		set { _ConnectionState = value; }
	}
	private string _ConnectionState;


	public string LastSyncTime
	{ 
		get { return _LastSyncTime; }
		set { _LastSyncTime = value; }
	}
	private string _LastSyncTime;


	public string OS_Type
	{ 
		get { return _OS_Type; }
		set { _OS_Type = value; }
	}
	private string _OS_Type;


	public string Client_Build
	{ 
		get { return _Client_Build; }
		set { _Client_Build = value; }
	}
	private string _Client_Build;


	public string NotificationType
	{ 
		get { return _NotificationType; }
		set { _NotificationType = value; }
	}
	private string _NotificationType;


	public int ID
	{ 
		get { return _ID; }
		set { _ID = value; }
	}
	private int _ID;


	public string DocID
	{ 
		get { return _DocID; }
		set { _DocID = value; }
	}
	private string _DocID;


	public string device_type
	{ 
		get { return _device_type; }
		set { _device_type = value; }
	}
	private string _device_type;


	public string Access
	{ 
		get { return _Access; }
		set { _Access = value; }
	}
	private string _Access;


	public string Security_Policy
	{ 
		get { return _Security_Policy; }
		set { _Security_Policy = value; }
	}
	private string _Security_Policy;


	public string wipeRequested
	{ 
		get { return _wipeRequested; }
		set { _wipeRequested = value; }
	}
	private string _wipeRequested;


	public string wipeOptions
	{ 
		get { return _wipeOptions; }
		set { _wipeOptions = value; }
	}
	private string _wipeOptions;


	public string wipeStatus
	{ 
		get { return _wipeStatus; }
		set { _wipeStatus = value; }
	}
	private string _wipeStatus;


	public string SyncType
	{ 
		get { return _SyncType; }
		set { _SyncType = value; }
	}
	private string _SyncType;


	public string wipeSupported
	{ 
		get { return _wipeSupported; }
		set { _wipeSupported = value; }
	}
	private string _wipeSupported;


	public string ServerName
	{ 
		get { return _ServerName; }
		set { _ServerName = value; }
	}
	private string _ServerName;


	public string Approval
	{ 
		get { return _Approval; }
		set { _Approval = value; }
	}
	private string _Approval;


	public string DeviceID
	{ 
		get { return _DeviceID; }
		set { _DeviceID = value; }
	}
	private string _DeviceID;

    public string LastUpdated
    {
        get { return _LastUpdated; }
        set {_LastUpdated = value;}
    }
    private string _LastUpdated;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public Traveler_Devices(string DeviceName, 
		string ConnectionState, 
		string LastSyncTime, 
		string OS_Type, 
		string Client_Build, 
		string NotificationType, 
		int ID, 
		string DocID, 
		string device_type, 
		string Access, 
		string Security_Policy, 
		string wipeRequested, 
		string wipeOptions, 
		string wipeStatus, 
		string SyncType, 
		string wipeSupported, 
		string ServerName, 
		string Approval, 
		string DeviceID,
        string LastUpdated)
	{
		this._DeviceName = DeviceName;
		this._ConnectionState = ConnectionState;
		this._LastSyncTime = LastSyncTime;
		this._OS_Type = OS_Type;
		this._Client_Build = Client_Build;
		this._NotificationType = NotificationType;
		this._ID = ID;
		this._DocID = DocID;
		this._device_type = device_type;
		this._Access = Access;
		this._Security_Policy = Security_Policy;
		this._wipeRequested = wipeRequested;
		this._wipeOptions = wipeOptions;
		this._wipeStatus = wipeStatus;
		this._SyncType = SyncType;
		this._wipeSupported = wipeSupported;
		this._ServerName = ServerName;
		this._Approval = Approval;
		this._DeviceID = DeviceID;
        this._LastUpdated = LastUpdated;
	}
}
