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


    






    public bool ConflictingContentTypes
    {
        get { return _ConflictingContentTypes; }
        set { _ConflictingContentTypes = value; }
    }
    private bool _ConflictingContentTypes;

    public bool CustomizedFiles
    {
        get { return _CustomizedFiles; }
        set { _CustomizedFiles = value; }
    }
    private bool _CustomizedFiles;

    public bool MissingGalleries
    {
        get { return _MissingGalleries; }
        set { _MissingGalleries = value; }
    }
    private bool _MissingGalleries;

    public bool MissingParentContentTypes
    {
        get { return _MissingParentContentTypes; }
        set { _MissingParentContentTypes = value; }
    }
    private bool _MissingParentContentTypes;

    public bool MissingSiteTemplates
    {
        get { return _MissingSiteTemplates; }
        set { _MissingSiteTemplates = value; }
    }
    private bool _MissingSiteTemplates;

    public bool UnsupportedLanguagePackReferences
    {
        get { return _UnsupportedLanguagePackReferences; }
        set { _UnsupportedLanguagePackReferences = value; }
    }
    private bool _UnsupportedLanguagePackReferences;

    public bool UnsupportedMUIReferences 
    {
        get { return _UnsupportedMUIReferences; }
        set { _UnsupportedMUIReferences = value; }
    }
    private bool _UnsupportedMUIReferences;
}
