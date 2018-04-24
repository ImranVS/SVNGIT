public class BlackBerryUsers
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public BlackBerryUsers()
	{}


	public string Username
	{ 
		get { return _Username; }
		set { _Username = value; }
	}
	private string _Username;


	public string Server
	{ 
		get { return _Server; }
		set { _Server = value; }
	}
	private string _Server;


	public int BESInteraction
	{ 
		get { return _BESInteraction; }
		set { _BESInteraction = value; }
	}
	private int _BESInteraction;


	public int PendingMessages
	{ 
		get { return _PendingMessages; }
		set { _PendingMessages = value; }
	}
	private int _PendingMessages;


	public string Category
	{ 
		get { return _Category; }
		set { _Category = value; }
	}
	private string _Category;


	public bool Enabled
	{ 
		get { return _Enabled; }
		set { _Enabled = value; }
	}
	private bool _Enabled;


	public int ExpiredMessages
	{ 
		get { return _ExpiredMessages; }
		set { _ExpiredMessages = value; }
	}
	private int _ExpiredMessages;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public BlackBerryUsers(string Username, 
		string Server, 
		int BESInteraction, 
		int PendingMessages, 
		string Category, 
		bool Enabled, 
		int ExpiredMessages)
	{
		this._Username = Username;
		this._Server = Server;
		this._BESInteraction = BESInteraction;
		this._PendingMessages = PendingMessages;
		this._Category = Category;
		this._Enabled = Enabled;
		this._ExpiredMessages = ExpiredMessages;
	}
}
