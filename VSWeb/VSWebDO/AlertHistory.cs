public class AlertHistory
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public AlertHistory()
	{}


	public string DeviceName
	{ 
		get { return _DeviceName; }
		set { _DeviceName = value; }
	}
	private string _DeviceName;


	public string DeviceType
	{ 
		get { return _DeviceType; }
		set { _DeviceType = value; }
	}
	private string _DeviceType;


	public string AlertType
	{ 
		get { return _AlertType; }
		set { _AlertType = value; }
	}
	private string _AlertType;


	public System.DateTime DateTimeOfAlert
	{ 
		get { return _DateTimeOfAlert; }
		set { _DateTimeOfAlert = value; }
	}
	private System.DateTime _DateTimeOfAlert;



	public System.DateTime DateTimeAlertCleared
	{ 
		get { return _DateTimeAlertCleared; }
		set { _DateTimeAlertCleared = value; }
	}
	private System.DateTime _DateTimeAlertCleared;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public AlertHistory(string DeviceName, 
		string DeviceType, 
		string AlertType, 
		System.DateTime DateTimeOfAlert, 
		System.DateTime DateTimeAlertCleared)
	{
		this._DeviceName = DeviceName;
		this._DeviceType = DeviceType;
		this._AlertType = AlertType;
		this._DateTimeOfAlert = DateTimeOfAlert;
		this._DateTimeAlertCleared = DateTimeAlertCleared;
	}
}
