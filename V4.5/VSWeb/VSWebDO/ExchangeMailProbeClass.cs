public class ExchangeMailProbeClass
{
    /// <summary>
    /// Default Contructor
    /// <summary>
    public ExchangeMailProbeClass()
    { }

    public bool Enabled
    {
        get { return _Enabled; }
        set { _Enabled = value; }

    }
    private bool _Enabled;

    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }
    private string _Name;


    public string ExchangeMailAddress
    {
        get { return _ExchangeMailAddress; }
        set { _ExchangeMailAddress = value; }
    }
    private string _ExchangeMailAddress;


    public string Category
    {
        get { return _Category; }
        set { _Category = value; }
    }
    private string _Category;


    public int ScanInterval
    {
        get { return _ScanInterval; }
        set { _ScanInterval = value; }
    }
    private int _ScanInterval;


    public int OffHoursScanInterval
    {
        get { return _OffHoursScanInterval; }
        set { _OffHoursScanInterval = value; }
    }
    private int _OffHoursScanInterval;


    public int DeliveryThreshold
    {
        get { return _DeliveryThreshold; }
        set { _DeliveryThreshold = value; }
    }
    private int _DeliveryThreshold;


    public int RetryInterval
    {
        get { return _RetryInterval; }
        set { _RetryInterval = value; }
    }
    private int _RetryInterval;


	public int SourceServerID
    {
		get { return _SourceServerID; }
		set { _SourceServerID = value; }
    }
	private int _SourceServerID;


    public string SourceServer
    {
        get { return _SourceServer; }
        set { _SourceServer = value; }
    }
    private string _SourceServer;


    public string Filename
    {
        get { return _Filename; }
        set { _Filename = value; }
    }
    private string _Filename;


    /// <summary>
    /// User defined Contructor
    /// <summary>
    public ExchangeMailProbeClass(bool Enabled,
        string Name,
        string ExchangeMailAddress,
        string Category,
        int ScanInterval,
        int OffHoursScanInterval,
        int DeliveryThreshold,
        int RetryInterval,
        string SourceServer,
        string Filename
        )
    {
        this._Enabled = Enabled;
        this._Name = Name;
        this._ExchangeMailAddress = ExchangeMailAddress;
        this._Category = Category;
        this._ScanInterval = ScanInterval;
        this._OffHoursScanInterval = OffHoursScanInterval;
        this._DeliveryThreshold = DeliveryThreshold;
        this._RetryInterval = RetryInterval;
        this._SourceServer = SourceServer;
        this._Filename = Filename;
    }
}
