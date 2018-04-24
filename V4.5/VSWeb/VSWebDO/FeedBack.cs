public class FeedBack
{
	/// <summary>
	/// Default Contructor
	/// <summary>



	public FeedBack()
	{ }
	public string Subject
	{
		get { return _Subject; }
		set { _Subject = value; }
	}
	private string _Subject;


	public string Type
	{
		get { return _Type; }
		set { _Type = value; }
	}
	private string _Type;


	public string Message
	{
		get { return _Message; }
		set { _Message = value; }
	}
	private string _Message;


	public string Status
	{
		get { return _Status; }
		set { _Status = value; }
	}
	private string _Status;


	public string Attachments
	{
		get { return _Attachments; }
		set { _Attachments = value; }
	}
	private string _Attachments;
	public int ID
	{
		get { return _ID; }
		set { _ID = value; }
	}
	private int _ID;


	public FeedBack(

		string Subject,
		   string Type,

		   string Message,

		   string Status,

		   string Attachments,
	int ID)
	{
		this._Subject = Subject;
		this._Type = Type;
		this._Message = Message;
		this._Status = Status;
		this._Attachments = Attachments;
		this._ID = ID;


	}
}
