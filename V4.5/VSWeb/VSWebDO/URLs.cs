public class URLs
{
    /// <summary>
    /// Default Contructor
    /// <summary>
    public URLs()
    { }

    //11/19/2013 NS added unique identifier for URLs
    public string ID
    {
        get { return _ID; }
        set { _ID = value; }
    }
    private string _ID;

    public string TheURL
    {
        get { return _TheURL; }
        set { _TheURL = value; }
    
    }
    private string _TheURL;
    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }
    private string _Name;


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


    public System.DateTime NextScan
    {
        get { return _NextScan; }
        set { _NextScan = value; }
    }
    private System.DateTime _NextScan;


    public System.DateTime LastChecked
    {
        get { return _LastChecked; }
        set { _LastChecked = value; }
    }
    private System.DateTime _LastChecked;


    public string LastStatus
    {
        get { return _LastStatus; }
        set { _LastStatus = value; }
    }
    private string _LastStatus;
    
    public bool Enabled
    {
        get { return _Enabled; }
        set { _Enabled = value; }
    }
    private bool _Enabled;

    public int ResponseThreshold
    {

        get { return _ResponseThreshold; }
        set { _ResponseThreshold = value; }
    }
    private int _ResponseThreshold;
    public int RetryInterval
    {

        get { return _RetryInterval; }
        set { _RetryInterval = value; }
    
       
    }
    private int _RetryInterval;
    public string SearchStringNotFound
    {

        get { return _SearchStringNotFound; }
        set { _SearchStringNotFound = value; }
    
    }
    private string _SearchStringNotFound;

    public string SearchStringFound
    {

        get { return _SearchStringFound; }
        set { _SearchStringFound = value; }

    }
    private string _SearchStringFound;
    public string UserName
    {

        get { return _UserName; }
        set { _UserName = value; }
    }
    private string _UserName;

    public string PW
    {
        get { return _PW; }
        set { _PW = value; }
       
    }
    private string _PW;

    public int LocationId
    {
        get { return _LocationId; }
        set { _LocationId = value; }
    }
    private int _LocationId;

    public int ServerTypeId
    {
        get { return _ServerTypeId; }
        set { _ServerTypeId = value; }
    }
    private int _ServerTypeId;

    public string Location
    {
        get { return _Location; }
        set { _Location = value; }

    }
    private string _Location;

    public int FailureThreshold
    {
        get { return _FailureThreshold; }
        set { _FailureThreshold = value; }
    }
    private int _FailureThreshold;

}