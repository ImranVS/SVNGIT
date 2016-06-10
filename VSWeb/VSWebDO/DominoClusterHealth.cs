public class DominoClusterHealth
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public DominoClusterHealth()
	{}


	public string ClusterName
	{ 
		get { return _ClusterName; }
		set { _ClusterName = value; }
	}
	private string _ClusterName;


	public int SecondsOnQueue
	{ 
		get { return _SecondsOnQueue; }
		set { _SecondsOnQueue = value; }
	}
	private int _SecondsOnQueue;


	public int SecondsOnQueueMax
	{ 
		get { return _SecondsOnQueueMax; }
		set { _SecondsOnQueueMax = value; }
	}
	private int _SecondsOnQueueMax;


	public float SecondsOnQueueAvg
	{ 
		get { return _SecondsOnQueueAvg; }
		set { _SecondsOnQueueAvg = value; }
	}
	private float _SecondsOnQueueAvg;


	public System.DateTime LastUpdate
	{ 
		get { return _LastUpdate; }
		set { _LastUpdate = value; }
	}
	private System.DateTime _LastUpdate;


	public int WorkQueueDepth
	{ 
		get { return _WorkQueueDepth; }
		set { _WorkQueueDepth = value; }
	}
	private int _WorkQueueDepth;


	public int WorkQueueDepthMax
	{ 
		get { return _WorkQueueDepthMax; }
		set { _WorkQueueDepthMax = value; }
	}
	private int _WorkQueueDepthMax;


	public float WorkQueueDepthAvg
	{ 
		get { return _WorkQueueDepthAvg; }
		set { _WorkQueueDepthAvg = value; }
	}
	private float _WorkQueueDepthAvg;


	public int Availability
	{ 
		get { return _Availability; }
		set { _Availability = value; }
	}
	private int _Availability;


	public int AvailabilityThreshold
	{ 
		get { return _AvailabilityThreshold; }
		set { _AvailabilityThreshold = value; }
	}
	private int _AvailabilityThreshold;


	public int ID
	{ 
		get { return _ID; }
		set { _ID = value; }
	}
	private int _ID;


	public string Analysis
	{ 
		get { return _Analysis; }
		set { _Analysis = value; }
	}
	private string _Analysis;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public DominoClusterHealth(string ClusterName, 
		int SecondsOnQueue, 
		int SecondsOnQueueMax, 
		float SecondsOnQueueAvg, 
		System.DateTime LastUpdate, 
		int WorkQueueDepth, 
		int WorkQueueDepthMax, 
		float WorkQueueDepthAvg, 
		int Availability, 
		int AvailabilityThreshold, 
		int ID, 
		string Analysis)
	{
		this._ClusterName = ClusterName;
		this._SecondsOnQueue = SecondsOnQueue;
		this._SecondsOnQueueMax = SecondsOnQueueMax;
		this._SecondsOnQueueAvg = SecondsOnQueueAvg;
		this._LastUpdate = LastUpdate;
		this._WorkQueueDepth = WorkQueueDepth;
		this._WorkQueueDepthMax = WorkQueueDepthMax;
		this._WorkQueueDepthAvg = WorkQueueDepthAvg;
		this._Availability = Availability;
		this._AvailabilityThreshold = AvailabilityThreshold;
		this._ID = ID;
		this._Analysis = Analysis;
	}
}
