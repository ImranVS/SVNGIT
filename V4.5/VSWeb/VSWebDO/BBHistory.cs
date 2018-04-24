public class BBHistory
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public BBHistory()
	{}


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


	public System.DateTime ReceiptFromRIM
	{ 
		get { return _ReceiptFromRIM; }
		set { _ReceiptFromRIM = value; }
	}
	private System.DateTime _ReceiptFromRIM;


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


	public string ConfirmationServer
	{ 
		get { return _ConfirmationServer; }
		set { _ConfirmationServer = value; }
	}
	private string _ConfirmationServer;


	public string ConfirmationDatabase
	{ 
		get { return _ConfirmationDatabase; }
		set { _ConfirmationDatabase = value; }
	}
	private string _ConfirmationDatabase;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public BBHistory(System.DateTime SentDateTime, 
		string SentTo, 
		int DeliveryThresholdInMinutes, 
		int DeliveryTimeInMinutes, 
		string SubjectKey, 
		System.DateTime ArrivalAtMailBox, 
		System.DateTime ReceiptFromRIM, 
		string Status, 
		string Details, 
		string DeviceName, 
		string TargetServer, 
		string TargetDatabase, 
		string ConfirmationServer, 
		string ConfirmationDatabase)
	{
		this._SentDateTime = SentDateTime;
		this._SentTo = SentTo;
		this._DeliveryThresholdInMinutes = DeliveryThresholdInMinutes;
		this._DeliveryTimeInMinutes = DeliveryTimeInMinutes;
		this._SubjectKey = SubjectKey;
		this._ArrivalAtMailBox = ArrivalAtMailBox;
		this._ReceiptFromRIM = ReceiptFromRIM;
		this._Status = Status;
		this._Details = Details;
		this._DeviceName = DeviceName;
		this._TargetServer = TargetServer;
		this._TargetDatabase = TargetDatabase;
		this._ConfirmationServer = ConfirmationServer;
		this._ConfirmationDatabase = ConfirmationDatabase;
	}
}
