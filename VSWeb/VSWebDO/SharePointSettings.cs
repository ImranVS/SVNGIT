public class SharePointSettings
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public SharePointSettings()
	{}

    public int ServerId
    {
        get { return _ServerId; }
        set { _ServerId = value; }
    }
    private int _ServerId;
	public string Sname
	{ 
		get { return _Sname; }
		set { _Sname = value; }
	}
	private string _Sname;


	public string Svalue
	{ 
		get { return _Svalue; }
		set { _Svalue = value; }
	}
	private string _Svalue;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public SharePointSettings(int ServerId,
        string Sname, 
		string Svalue)
    {
        this._ServerId = ServerId; 
		this._Sname = Sname;
		this._Svalue = Svalue;
	}
}
