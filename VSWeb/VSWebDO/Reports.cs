public class Reports
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public Reports()
	{}


	public string ReportName
	{ 
		get { return _ReportName; }
		set { _ReportName = value; }
	}
	private string _ReportName;


	public string Frequency
	{ 
		get { return _Frequency; }
		set { _Frequency = value; }
	}
	private string _Frequency;


	public int ID
	{ 
		get { return _ID; }
		set { _ID = value; }
	}
	private int _ID;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public Reports(string ReportName, 
		string Frequency, 
		int ID)
	{
		this._ReportName = ReportName;
		this._Frequency = Frequency;
		this._ID = ID;
	}
}
