public class ScheduledReports
{
    public ScheduledReports()
    {

    }

    public int ID
    {
        get { return _ID; }
        set { _ID = value; }
    }
    private int _ID;

    public int ReportID
    {
        get { return _ReportID; }
        set { _ReportID = value; }
    }
    private int _ReportID;

    public string Frequency
    {
        get { return _Frequency; }
        set { _Frequency = value; }
    }
    private string _Frequency;

    public string Days
    {
        get { return _Days; }
        set { _Days = value; }
    }
    private string _Days;

    public int SpecificDay
    {
        get { return _SpecificDay; }
        set { _SpecificDay = value; }
    }
    private int _SpecificDay;

    public string SendTo
    {
        get { return _SendTo; }
        set { _SendTo = value; }
    }
    private string _SendTo;

    public string CopyTo
    {
        get { return _CopyTo; }
        set { _CopyTo = value; }
    }
    private string _CopyTo;

    public string BlindCopyTo
    {
        get { return _BlindCopyTo; }
        set { _BlindCopyTo = value; }
    }
    private string _BlindCopyTo;

    public string Title
    {
        get { return _Title; }
        set { _Title = value; }
    }
    private string _Title;

    public string Body
    {
        get { return _Body; }
        set { _Body = value; }
    }
    private string _Body;

    public string FileFormat
    {
        get { return _FileFormat; }
        set { _FileFormat = value; }
    }
    private string _FileFormat;

    public ScheduledReports(int ID,
        int ReportID,
        string Frequency,
        string Days,
        int SpecificDay,
        string SendTo,
        string CopyTo,
        string BlindCopyTo,
        string Title,
        string Body,
        string FileFormat)
    {
        this._ID = ID;
        this._ReportID = ReportID;
        this._Frequency = Frequency;
        this._Days = Days;
        this._SpecificDay = SpecificDay;
        this._SendTo = SendTo;
        this._CopyTo = CopyTo;
        this._BlindCopyTo = BlindCopyTo;
        this._Title = Title;
        this._Body = Body;
        this._FileFormat = FileFormat;
    }
}
    