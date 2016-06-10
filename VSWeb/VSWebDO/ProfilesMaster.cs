public class ProfilesMaster
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public ProfilesMaster()
	{}
    public int Id
    {
        get { return _Id; }
        set { _Id = value; }
    }
    private int _Id;

    public int ServerTypeId
    {
        get { return _ServerTypeId; }
        set { _ServerTypeId = value; }
    }
    private int  _ServerTypeId;

    public string ServerType
    {
        get { return _ServerType; }
        set { _ServerType = value; }
    }
    private string _ServerType;

    public string AttributeName
    {
        get { return _AttributeName; }
        set { _AttributeName = value; }
    }
    private string _AttributeName;

    public string UnitOfMeasurement
    {
        get { return _UnitOfMeasurement; }
        set { _UnitOfMeasurement = value; }
    }
    private string _UnitOfMeasurement;

    

    public string DefaultValue
    {
        get { return _DefaultValue; }
        set { _DefaultValue = value; }
    }
    private string _DefaultValue;

    public string RelatedTable
    {
        get { return _RelatedTable; }
        set { _RelatedTable = value; }
    }
    private string _RelatedTable;

    public string RelatedField
    {
        get { return _RelatedField; }
        set { _RelatedField = value; }
    }
    private string _RelatedField;

    private string _RoleType;

    public string RoleType
    {
        get { return _RoleType; }
        set { _RoleType = value; }
    }
	public int ProfileId
	{
		get { return _ProfileId; }
		set { _ProfileId = value; }
	}
	private int _ProfileId;

	public bool isSelected
	{
		get { return _isSelected; }
		set { _isSelected = value; }
	}
	private bool _isSelected;
	/// <summary>
	/// User defined Contructor
	/// <summary>
	public ProfilesMaster(
        int Id,
        int ServerTypeId,
        string ServerType,
        string AttributeName,
        string AttributeMeasurement,
        string DefaultValue,
        string RelatedTable,
        string RelatedField,
        string RoleType,
	    int ProfileId
        )
    {
        this._Id = Id;
        this._ServerTypeId = ServerTypeId;
        this._ServerType = ServerType;
        this._AttributeName = AttributeName;
        this._UnitOfMeasurement = AttributeMeasurement;
        this._DefaultValue = DefaultValue;
        this._RelatedTable = RelatedTable;
        this._RelatedField = RelatedField;
        this._RoleType = RoleType;
		this._ProfileId = ProfileId;
	}
}
