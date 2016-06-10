public class PowershellScripts
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public PowershellScripts()
	{}

    public string ScriptName
    {
        get { return _ScriptName; }
        set { _ScriptName = value; }
    }
    private string _ScriptName;

	public string ScriptDetails
	{ 
		get { return _ScriptDetails; }
		set { _ScriptDetails = value; }
	}
	private string _ScriptDetails;

	/// <summary>
	/// User defined Contructor
	/// <summary>
    /// 
    private string _Category;

    public string Category
    {
        get { return _Category; }
        set { _Category = value; }
        
    }

    private string _Description;
    public string Description
    {
        get { return _Description; }
        set { _Description = value; }
    }
    
	public PowershellScripts(string ScriptDetails, string ScriptName)
	{
        this._ScriptName = ScriptName;
		this._ScriptDetails = ScriptDetails;

	}

    public PowershellScripts(string Scriptdetails, string ScriptName, string Category, string Description)
    {
        this._ScriptDetails = Scriptdetails;
        this._ScriptName = ScriptName;
        this._Category = Category;
        this._Description = Description;

    }

}
