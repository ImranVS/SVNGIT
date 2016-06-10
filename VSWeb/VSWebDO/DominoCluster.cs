public class DominoCluster
{
    /// <summary>
    /// Default Contructor
    /// <summary>
    public DominoCluster()
    { }
    public int ID
    {
        get { return _ID; }
        set { _ID = value; }
    }
    private int _ID;


    public int ServerID_A
    {
        get { return _ServerID_A; }
        set { _ServerID_A = value; }
    }
    private int _ServerID_A;

    public string ServerName
    {
        get { return _ServerName; }
        set { _ServerName = value; }
    }
    private string _ServerName;


    public string Server_A_Directory
    {
        get { return _Server_A_Directory; }
        set { _Server_A_Directory = value; }
    }
    private string _Server_A_Directory;


    public string Server_A_ExcludeList
    {
        get { return _Server_A_ExcludeList; }
        set { _Server_A_ExcludeList = value; }
    }
    private string _Server_A_ExcludeList;


    public int ServerID_B
    {
        get { return _ServerID_B; }
        set { _ServerID_B = value; }
    }
    private int _ServerID_B;




    public string Server_B_Directory
    {
        get { return _Server_B_Directory; }
        set { _Server_B_Directory = value; }
    }
    private string _Server_B_Directory;


    public string Server_B_ExcludeList
    {
        get { return _Server_B_ExcludeList; }
        set { _Server_B_ExcludeList = value; }
    }
    private string _Server_B_ExcludeList;


    public int ServerID_C
    {
        get { return _ServerID_C; }
        set { _ServerID_C = value; }
    }
    private int _ServerID_C;




    public string Server_C_Directory
    {
        get { return _Server_C_Directory; }
        set { _Server_C_Directory = value; }
    }
    private string _Server_C_Directory;


    public string Server_C_ExcludeList
    {
        get { return _Server_C_ExcludeList; }
        set { _Server_C_ExcludeList = value; }
    }
    private string _Server_C_ExcludeList;


    public bool Missing_Replica_Alert
    {
        get { return _Missing_Replica_Alert; }
        set { _Missing_Replica_Alert = value; }
    }
    private bool _Missing_Replica_Alert;


    public float First_Alert_Threshold
    {
        get { return _First_Alert_Threshold; }
        set { _First_Alert_Threshold = value; }
    }
    private float _First_Alert_Threshold;


    public float Second_Alert_Threshold
    {
        get { return _Second_Alert_Threshold; }
        set { _Second_Alert_Threshold = value; }
    }
    private float _Second_Alert_Threshold;


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


    public int RetryInterval
    {
        get { return _RetryInterval; }
        set { _RetryInterval = value; }
    }
    private int _RetryInterval;


    public string Category
    {
        get { return _Category; }
        set { _Category = value; }
    }
    private string _Category;

    //12/13/2012 NS added
    public string ServerAName
    {
        get { return _ServerAName; }
        set { _ServerAName = value; }
    }
    private string _ServerAName;

    public string ServerBName
    {
        get { return _ServerBName; }
        set { _ServerBName = value; }
    }
    private string _ServerBName;

    public string ServerCName
    {
        get { return _ServerCName; }
        set { _ServerCName = value; }
    }
    private string _ServerCName;
    public string ClusterScan
    {
        get { return _ClusterScan; }
        set { _ClusterScan = value; }
    }
    private string _ClusterScan;
    /// <summary>
    /// User defined Contructor
    /// <summary>
    public DominoCluster(
        int ID,
        int ServerID_A,
       string ServerName,
        string Server_A_Directory,
        string Server_A_ExcludeList,
        int ServerID_B,

        string Server_B_Directory,
        string Server_B_ExcludeList,
        int ServerID_C,

        string Server_C_Directory,
        string Server_C_ExcludeList,
        bool Missing_Replica_Alert,
        float First_Alert_Threshold,
        float Second_Alert_Threshold,
        bool Enabled,
        string Name,
        int ScanInterval,
        int OffHoursScanInterval,
        int RetryInterval,
        string Category,
        string ServerAName,
        string ServerBName,
        string ServerCName,string ClusterScan)
    {
        this._ID = ID;
        this._ServerID_A = ServerID_A;
        this._ServerName = ServerName;
        this._Server_A_Directory = Server_A_Directory;
        this._Server_A_ExcludeList = Server_A_ExcludeList;
        this._ServerID_B = ServerID_B;

        this._Server_B_Directory = Server_B_Directory;
        this._Server_B_ExcludeList = Server_B_ExcludeList;
        this._ServerID_C = ServerID_C;

        this._Server_C_Directory = Server_C_Directory;
        this._Server_C_ExcludeList = Server_C_ExcludeList;
        this._Missing_Replica_Alert = Missing_Replica_Alert;
        this._First_Alert_Threshold = First_Alert_Threshold;
        this._Second_Alert_Threshold = Second_Alert_Threshold;
        this._Enabled = Enabled;
        this._Name = Name;
        this._ScanInterval = ScanInterval;
        this._OffHoursScanInterval = OffHoursScanInterval;
        this._RetryInterval = RetryInterval;
        this._Category = Category;
        this._ServerAName = ServerAName;
        this._ServerBName = ServerBName;
        this._ServerCName = ServerCName;
        this._ClusterScan = ClusterScan;
    }
}
