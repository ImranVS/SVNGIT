public class Traveler_Status
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public Traveler_Status()
	{}


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


	public int Users
	{ 
		get { return _Users; }
		set { _Users = value; }
	}
	private int _Users;


	public int IncrementalSyncs
	{ 
		get { return _IncrementalSyncs; }
		set { _IncrementalSyncs = value; }
	}
	private int _IncrementalSyncs;


	public int ID
	{ 
		get { return _ID; }
		set { _ID = value; }
	}
	private int _ID;


	public int Devices
	{ 
		get { return _Devices; }
		set { _Devices = value; }
	}
	private int _Devices;


	public string HTTP_Status
	{ 
		get { return _HTTP_Status; }
		set { _HTTP_Status = value; }
	}
	private string _HTTP_Status;


	public string HTTP_Details
	{ 
		get { return _HTTP_Details; }
		set { _HTTP_Details = value; }
	}
	private string _HTTP_Details;


	public int HTTP_PeakConnections
	{ 
		get { return _HTTP_PeakConnections; }
		set { _HTTP_PeakConnections = value; }
	}
	private int _HTTP_PeakConnections;


	public int HTTP_MaxConfiguredConnections
	{ 
		get { return _HTTP_MaxConfiguredConnections; }
		set { _HTTP_MaxConfiguredConnections = value; }
	}
	private int _HTTP_MaxConfiguredConnections;


	public string TravelerVersion
	{ 
		get { return _TravelerVersion; }
		set { _TravelerVersion = value; }
	}
	private string _TravelerVersion;

	public string Heartbeat
	{
		get { return _heartBeat; }
		set { _heartBeat = value; }
	}
	private string _heartBeat;
	/// <summary>
	/// User defined Contructor
	/// <summary>
	public Traveler_Status(string Status, 
		string Details, 
		int Users, 
		int IncrementalSyncs, 
		int ID, 
		int Devices, 
		string HTTP_Status, 
		string HTTP_Details, 
		int HTTP_PeakConnections, 
		int HTTP_MaxConfiguredConnections, 
		string TravelerVersion,
		string HeartBeat)
	{
		this._Status = Status;
		this._Details = Details;
		this._Users = Users;
		this._IncrementalSyncs = IncrementalSyncs;
		this._ID = ID;
		this._Devices = Devices;
		this._HTTP_Status = HTTP_Status;
		this._HTTP_Details = HTTP_Details;
		this._HTTP_PeakConnections = HTTP_PeakConnections;
		this._HTTP_MaxConfiguredConnections = HTTP_MaxConfiguredConnections;
		this._TravelerVersion = TravelerVersion;
		this._heartBeat = HeartBeat;
	}
}
