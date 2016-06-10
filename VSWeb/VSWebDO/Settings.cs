public class Settings
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public Settings()
	{}

    public string sname
    {
        get { return _sname; }
        set { _sname = value; }
    }
    private string _sname;
	public string svalue
	{ 
		get { return _svalue; }
		set { _svalue = value; }
	}
	private string _svalue;


	public string stype
	{ 
		get { return _stype; }
		set { _stype = value; }
	}
	private string _stype;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public Settings(string sname,
        string svalue, 
		string stype)
    {
        this._sname = sname; 
		this._svalue = svalue;
		this._stype = stype;
	}
}
