public class BlackBerryServers
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public BlackBerryServers()
	{}


	public string Name
	{ 
		get { return _Name; }
		set { _Name = value; }
	}
	private string _Name;

    public int ServerID
    {
        get { return _ServerID; }
        set { _ServerID = value; }
    }
    private int _ServerID;
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


	public bool Enabled
	{ 
		get { return _Enabled; }
		set { _Enabled = value; }
	}
	private bool _Enabled;


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


	public string SNMPCommunity
	{ 
		get { return _SNMPCommunity; }
		set { _SNMPCommunity = value; }
	}
	private string _SNMPCommunity;


	public bool Messaging
	{ 
		get { return _Messaging; }
		set { _Messaging = value; }
	}
	private bool _Messaging;


	public bool Controller
	{ 
		get { return _Controller; }
		set { _Controller = value; }
	}
	private bool _Controller;


	public bool Dispatcher
	{ 
		get { return _Dispatcher; }
		set { _Dispatcher = value; }
	}
	private bool _Dispatcher;


	public bool Synchronization
	{ 
		get { return _Synchronization; }
		set { _Synchronization = value; }
	}
	private bool _Synchronization;


	public bool Policy
	{ 
		get { return _Policy; }
		set { _Policy = value; }
	}
	private bool _Policy;


	public bool MDS
	{ 
		get { return _MDS; }
		set { _MDS = value; }
	}
	private bool _MDS;


	public bool Attachment
	{ 
		get { return _Attachment; }
		set { _Attachment = value; }
	}
	private bool _Attachment;


	public bool Alert
	{ 
		get { return _Alert; }
		set { _Alert = value; }
	}
	private bool _Alert;


	public bool Router
	{ 
		get { return _Router; }
		set { _Router = value; }
	}
	private bool _Router;


	public string AlertIP
	{ 
		get { return _AlertIP; }
		set { _AlertIP = value; }
	}
	private string _AlertIP;


	public string RouterIP
	{ 
		get { return _RouterIP; }
		set { _RouterIP = value; }
	}
	private string _RouterIP;


	public string AttachmentIP
	{ 
		get { return _AttachmentIP; }
		set { _AttachmentIP = value; }
	}
	private string _AttachmentIP;


	public string OtherServices
	{ 
		get { return _OtherServices; }
		set { _OtherServices = value; }
	}
	private string _OtherServices;


	public bool MDSConnection
	{ 
		get { return _MDSConnection; }
		set { _MDSConnection = value; }
	}
	private bool _MDSConnection;


	public string BESVersion
	{ 
		get { return _BESVersion; }
		set { _BESVersion = value; }
	}
	private string _BESVersion;


	public bool MDSServices
	{ 
		get { return _MDSServices; }
		set { _MDSServices = value; }
	}
	private bool _MDSServices;


	public int TimeZoneAdjustment
	{ 
		get { return _TimeZoneAdjustment; }
		set { _TimeZoneAdjustment = value; }
	}
	private int _TimeZoneAdjustment;


	public bool USDateFormat
	{ 
		get { return _USDateFormat; }
		set { _USDateFormat = value; }
	}
	private bool _USDateFormat;

    public string Address
    {
        get { return _Address; }
        set { _Address = value; }
    }
    private string _Address;

	public int PendingThreshold
	{ 
		get { return _PendingThreshold; }
		set { _PendingThreshold = value; }
	}
	private int _PendingThreshold;


	public int ExpiredThreshold
	{ 
		get { return _ExpiredThreshold; }
		set { _ExpiredThreshold = value; }
	}
	private int _ExpiredThreshold;


	public string NotificationGroup
	{ 
		get { return _NotificationGroup; }
		set { _NotificationGroup = value; }
	}
    private string _NotificationGroup;

    public string HAOption
    {
        get { return _HAOption; }
        set { _HAOption = value; }
    }


    private string _HAOption;

    public string HAPartner
    {
        get { return _HAPartner; }
        set { _HAPartner = value; }
    }


    private string _HAPartner;



	/// <summary>
	/// User defined Contructor
	/// <summary>
	public BlackBerryServers(string Name, 
        int ServerID,
		string Description, 
		string Category, 
		int ScanInterval, 
		int OffHoursScanInterval, 
		bool Enabled, 
		int RetryInterval, 
		int key, 
		string SNMPCommunity, 
		bool Messaging, 
		bool Controller, 
		bool Dispatcher, 
		bool Synchronization, 
		bool Policy, 
		bool MDS, 
		bool Attachment, 
		bool Alert, 
		bool Router, 
		string AlertIP, 
		string RouterIP, 
		string AttachmentIP, 
		string OtherServices, 
		bool MDSConnection, 
		string BESVersion, 
		bool MDSServices, 
		int TimeZoneAdjustment, 
		bool USDateFormat, 
		int PendingThreshold, 
		int ExpiredThreshold, 
		string NotificationGroup,string Address,
        string HAOption, string HAPartner)
	{
        this._ServerID = ServerID;
		this._Name = Name;
		this._Description = Description;
		this._Category = Category;
		this._ScanInterval = ScanInterval;
		this._OffHoursScanInterval = OffHoursScanInterval;
		this._Enabled = Enabled;
		this._RetryInterval = RetryInterval;
		this._key = key;
		this._SNMPCommunity = SNMPCommunity;
		this._Messaging = Messaging;
		this._Controller = Controller;
		this._Dispatcher = Dispatcher;
		this._Synchronization = Synchronization;
		this._Policy = Policy;
		this._MDS = MDS;
		this._Attachment = Attachment;
		this._Alert = Alert;
		this._Router = Router;
		this._AlertIP = AlertIP;
		this._RouterIP = RouterIP;
		this._AttachmentIP = AttachmentIP;
		this._OtherServices = OtherServices;
		this._MDSConnection = MDSConnection;
		this._BESVersion = BESVersion;
		this._MDSServices = MDSServices;
		this._TimeZoneAdjustment = TimeZoneAdjustment;
		this._USDateFormat = USDateFormat;
		this._PendingThreshold = PendingThreshold;
		this._ExpiredThreshold = ExpiredThreshold;
		this._NotificationGroup = NotificationGroup;
        this._Address = Address;
        this.HAOption = HAOption;
        this.HAPartner = HAPartner;

	}
}
