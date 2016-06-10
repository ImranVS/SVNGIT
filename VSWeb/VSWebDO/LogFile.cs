public class LogFile
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public LogFile()
	{}

    public string Keyword
    {
        get { return _Keyword; }
        set { _Keyword = value; }

    }
    private string _Keyword;

    public string NotRequiredKeyword
    {
        get {return _NotRequiredKeyword; }
        set {_NotRequiredKeyword=value; }
    }
    private string _NotRequiredKeyword;

	public bool RepeatOnce
	{ 
		get { return _RepeatOnce; }
		set { _RepeatOnce = value; }
	}
	private bool _RepeatOnce;
	public bool AgentLog
	{
		get { return _AgentLog; }
		set { _AgentLog = value; }
	}
	private bool _AgentLog;
	public bool Log
	{
		get { return _Log; }
		set { _Log = value; }
	}
	private bool _Log;


	public int DominoEventLogId
	{
		get { return _DominoEventLogId; }
		set { _DominoEventLogId = value; }
	}
	private int _DominoEventLogId;

	public int ID
	{
		get { return _ID; }
		set { _ID = value; }
	}
	private int _ID;


	/// <summary>
	/// User defined Contructor
	/// <summary>
	public LogFile(string Keyword,
        bool RepeatOnce,
		 bool Log,
		bool AgentLog,
        int ID,int _DominoEventLogId,
        string NotRequiredKeyword)
	{
        this._Keyword = Keyword;
		this._RepeatOnce = RepeatOnce;
		this._Log = Log;
		this._AgentLog = AgentLog;
		this._DominoEventLogId = DominoEventLogId;
		this._ID = ID;
        this._NotRequiredKeyword = NotRequiredKeyword;
    }
}
