public class DominoCustomStatValues
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public DominoCustomStatValues()
	{}
    public int ID
    {
        get { return _ID; }
        set { _ID = value; }
    }
    private int _ID;

	public string ServerName
	{ 
		get { return _ServerName; }
		set { _ServerName = value; }
	}
	private string _ServerName;


	public string StatName
	{ 
		get { return _StatName; }
		set { _StatName = value; }
	}
	private string _StatName;


	public float ThresholdValue
	{ 
		get { return _ThresholdValue; }
		set { _ThresholdValue = value; }
	}
	private float _ThresholdValue;


	public string GreaterThanORLessThan
	{ 
		get { return _GreaterThanORLessThan; }
		set { _GreaterThanORLessThan = value; }
	}
	private string _GreaterThanORLessThan;


	public int TimesInARow
	{ 
		get { return _TimesInARow; }
		set { _TimesInARow = value; }
	}
	private int _TimesInARow;


	public string ConsoleCommand
	{ 
		get { return _ConsoleCommand; }
		set { _ConsoleCommand = value; }
	}
	private string _ConsoleCommand;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public DominoCustomStatValues(string ServerName, 
		string StatName, 
		float ThresholdValue, 
		string GreaterThanORLessThan, 
		int TimesInARow, 
		string ConsoleCommand,
        int ID)
	{
		this._ServerName = ServerName;
		this._StatName = StatName;
		this._ThresholdValue = ThresholdValue;
		this._GreaterThanORLessThan = GreaterThanORLessThan;
		this._TimesInARow = TimesInARow;
		this._ConsoleCommand = ConsoleCommand;
        this._ID = ID;
	}
}
