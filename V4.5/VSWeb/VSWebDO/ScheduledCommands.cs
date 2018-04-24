public class ScheduledCommands
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public ScheduledCommands()
	{}


	public bool Enabled
	{ 
		get { return _Enabled; }
		set { _Enabled = value; }
	}
	private bool _Enabled;


	public string DominoServerName
	{ 
		get { return _DominoServerName; }
		set { _DominoServerName = value; }
	}
	private string _DominoServerName;


	public string ConsoleCommand
	{ 
		get { return _ConsoleCommand; }
		set { _ConsoleCommand = value; }
	}
	private string _ConsoleCommand;


	public System.DateTime TimeOfDay
	{ 
		get { return _TimeOfDay; }
		set { _TimeOfDay = value; }
	}
	private System.DateTime _TimeOfDay;


	public bool Sunday
	{ 
		get { return _Sunday; }
		set { _Sunday = value; }
	}
	private bool _Sunday;


	public bool Monday
	{ 
		get { return _Monday; }
		set { _Monday = value; }
	}
	private bool _Monday;


	public bool Tuesday
	{ 
		get { return _Tuesday; }
		set { _Tuesday = value; }
	}
	private bool _Tuesday;


	public bool Wednesday
	{ 
		get { return _Wednesday; }
		set { _Wednesday = value; }
	}
	private bool _Wednesday;


	public bool Thursday
	{ 
		get { return _Thursday; }
		set { _Thursday = value; }
	}
	private bool _Thursday;


	public bool Friday
	{ 
		get { return _Friday; }
		set { _Friday = value; }
	}
	private bool _Friday;


	public bool Saturday
	{ 
		get { return _Saturday; }
		set { _Saturday = value; }
	}
	private bool _Saturday;


	public int Key
	{ 
		get { return _Key; }
		set { _Key = value; }
	}
	private int _Key;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public ScheduledCommands(bool Enabled, 
		string DominoServerName, 
		string ConsoleCommand, 
		System.DateTime TimeOfDay, 
		bool Sunday, 
		bool Monday, 
		bool Tuesday, 
		bool Wednesday, 
		bool Thursday, 
		bool Friday, 
		bool Saturday, 
		int Key)
	{
		this._Enabled = Enabled;
		this._DominoServerName = DominoServerName;
		this._ConsoleCommand = ConsoleCommand;
		this._TimeOfDay = TimeOfDay;
		this._Sunday = Sunday;
		this._Monday = Monday;
		this._Tuesday = Tuesday;
		this._Wednesday = Wednesday;
		this._Thursday = Thursday;
		this._Friday = Friday;
		this._Saturday = Saturday;
		this._Key = Key;
	}
}
