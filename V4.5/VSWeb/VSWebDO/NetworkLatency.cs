public class NetworkLatency
{
    /// <summary>
    /// Default Contructor
    /// <summary>
    public NetworkLatency()
    { }

    public int ServerID
    {
        get { return _ServerID; }
        set { _ServerID = value; }
    }
    private int _ServerID;



    public int NetworkLatencyId
    {
        get { return _NetworkLatencyId; }
        set { _NetworkLatencyId = value; }
    }
    private int _NetworkLatencyId;

    public bool Enable
    {
        get { return _Enable; }
        set { _Enable = value; }
    }
    private bool _Enable;

  
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

    public string TestName
    {
        get { return _TestName; }
        set { _TestName = value; }
    }
    private string _TestName;

  

    public int ScanInterval
    {
        get { return _ScanInterval; }
        set { _ScanInterval = value; }
    }
    private int _ScanInterval;


    public int TestDuration
    {
        get { return _TestDuration; }
        set { _TestDuration = value; }
    }
    private int _TestDuration;


    public int LatencyYellowThreshold
    {
        get { return _LatencyYellowThreshold; }
        set { _LatencyYellowThreshold = value; }
    }
    private int _LatencyYellowThreshold;

    public int LatencyRedThreshold
    {
        get { return _LatencyRedThreshold; }
        set { _LatencyRedThreshold = value; }
    }
    private int _LatencyRedThreshold;



    /// <summary>
    /// User defined Contructor
    /// <summary>
    public NetworkLatency(
        int ServerID,
        int NetworkLatencyId,
       
        bool Enabled,
        string Name,
        string TestName,
        int ScanInterval,
        int TestDuration
       
        )
    {
        this._ServerID = ServerID;
        this._NetworkLatencyId = NetworkLatencyId;
      
        this._Enabled = Enabled;
        this._Name = Name;
        this._TestName = TestName;
        this._ScanInterval = ScanInterval;
        this._TestDuration = TestDuration;
       

       
    }
}
