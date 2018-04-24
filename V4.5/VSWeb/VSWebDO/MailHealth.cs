public class MailHealth
{
	/// <summary>
	/// Default Contructor
	/// <summary>
	public MailHealth()
	{}


	public string Domino_Domain
	{ 
		get { return _Domino_Domain; }
		set { _Domino_Domain = value; }
	}
	private string _Domino_Domain;


	public int Mailbox_Count
	{ 
		get { return _Mailbox_Count; }
		set { _Mailbox_Count = value; }
	}
	private int _Mailbox_Count;


	public float Mailbox_PerformanceIndex
	{ 
		get { return _Mailbox_PerformanceIndex; }
		set { _Mailbox_PerformanceIndex = value; }
	}
	private float _Mailbox_PerformanceIndex;


	public int Mail_Pending
	{ 
		get { return _Mail_Pending; }
		set { _Mail_Pending = value; }
	}
	private int _Mail_Pending;


	public int Mail_Dead
	{ 
		get { return _Mail_Dead; }
		set { _Mail_Dead = value; }
	}
	private int _Mail_Dead;


	public int Mail_Waiting
	{ 
		get { return _Mail_Waiting; }
		set { _Mail_Waiting = value; }
	}
	private int _Mail_Waiting;


	public int Mail_Held
	{ 
		get { return _Mail_Held; }
		set { _Mail_Held = value; }
	}
	private int _Mail_Held;


	public int Mail_MaximiumSizeDelivered
	{ 
		get { return _Mail_MaximiumSizeDelivered; }
		set { _Mail_MaximiumSizeDelivered = value; }
	}
	private int _Mail_MaximiumSizeDelivered;


	public string Mail_PeakMessageDeliveryTime
	{ 
		get { return _Mail_PeakMessageDeliveryTime; }
		set { _Mail_PeakMessageDeliveryTime = value; }
	}
	private string _Mail_PeakMessageDeliveryTime;


	public int DeadThreshold
	{ 
		get { return _DeadThreshold; }
		set { _DeadThreshold = value; }
	}
	private int _DeadThreshold;


	public int PendingThreshold
	{ 
		get { return _PendingThreshold; }
		set { _PendingThreshold = value; }
	}
	private int _PendingThreshold;


	public int HeldMailThreshold
	{ 
		get { return _HeldMailThreshold; }
		set { _HeldMailThreshold = value; }
	}
	private int _HeldMailThreshold;


	public int Mail_AverageDeliveryTime
	{ 
		get { return _Mail_AverageDeliveryTime; }
		set { _Mail_AverageDeliveryTime = value; }
	}
	private int _Mail_AverageDeliveryTime;


	public int Mail_AverageSizeDelivered
	{ 
		get { return _Mail_AverageSizeDelivered; }
		set { _Mail_AverageSizeDelivered = value; }
	}
	private int _Mail_AverageSizeDelivered;


	public int Mail_AverageServerHops
	{ 
		get { return _Mail_AverageServerHops; }
		set { _Mail_AverageServerHops = value; }
	}
	private int _Mail_AverageServerHops;


	public int Mail_Transferred
	{ 
		get { return _Mail_Transferred; }
		set { _Mail_Transferred = value; }
	}
	private int _Mail_Transferred;


	public int Mail_Delivered
	{ 
		get { return _Mail_Delivered; }
		set { _Mail_Delivered = value; }
	}
	private int _Mail_Delivered;


	public int Mail_Transferred_NRPC
	{ 
		get { return _Mail_Transferred_NRPC; }
		set { _Mail_Transferred_NRPC = value; }
	}
	private int _Mail_Transferred_NRPC;


	public int Mail_Transferred_SMTP
	{ 
		get { return _Mail_Transferred_SMTP; }
		set { _Mail_Transferred_SMTP = value; }
	}
	private int _Mail_Transferred_SMTP;


	public int Mail_TransferThreads_Active
	{ 
		get { return _Mail_TransferThreads_Active; }
		set { _Mail_TransferThreads_Active = value; }
	}
	private int _Mail_TransferThreads_Active;


	public int Mail_WaitingForDeliveryRetry
	{ 
		get { return _Mail_WaitingForDeliveryRetry; }
		set { _Mail_WaitingForDeliveryRetry = value; }
	}
	private int _Mail_WaitingForDeliveryRetry;


	public int Mail_WaitingForDIR
	{ 
		get { return _Mail_WaitingForDIR; }
		set { _Mail_WaitingForDIR = value; }
	}
	private int _Mail_WaitingForDIR;


	public int Mail_WaitingForDNS
	{ 
		get { return _Mail_WaitingForDNS; }
		set { _Mail_WaitingForDNS = value; }
	}
	private int _Mail_WaitingForDNS;


	public int Mail_DeliveredSize_100KB_to_1MB
	{ 
		get { return _Mail_DeliveredSize_100KB_to_1MB; }
		set { _Mail_DeliveredSize_100KB_to_1MB = value; }
	}
	private int _Mail_DeliveredSize_100KB_to_1MB;


	public int Mail_DeliveredSize_10KB_to_100KB
	{ 
		get { return _Mail_DeliveredSize_10KB_to_100KB; }
		set { _Mail_DeliveredSize_10KB_to_100KB = value; }
	}
	private int _Mail_DeliveredSize_10KB_to_100KB;


	public int Mail_DeliveredSize_10MB_to_100MB
	{ 
		get { return _Mail_DeliveredSize_10MB_to_100MB; }
		set { _Mail_DeliveredSize_10MB_to_100MB = value; }
	}
	private int _Mail_DeliveredSize_10MB_to_100MB;


	public int Mail_DeliveredSize_1KB_to_10KB
	{ 
		get { return _Mail_DeliveredSize_1KB_to_10KB; }
		set { _Mail_DeliveredSize_1KB_to_10KB = value; }
	}
	private int _Mail_DeliveredSize_1KB_to_10KB;


	public int Mail_DeliveredSize_1MB_to_10MB
	{ 
		get { return _Mail_DeliveredSize_1MB_to_10MB; }
		set { _Mail_DeliveredSize_1MB_to_10MB = value; }
	}
	private int _Mail_DeliveredSize_1MB_to_10MB;


	public int Mail_DeliveredSize_Under_1KB
	{ 
		get { return _Mail_DeliveredSize_Under_1KB; }
		set { _Mail_DeliveredSize_Under_1KB = value; }
	}
	private int _Mail_DeliveredSize_Under_1KB;


	public int Mail_Routed
	{ 
		get { return _Mail_Routed; }
		set { _Mail_Routed = value; }
	}
	private int _Mail_Routed;


	public int Mail_PeakMessagesDelivered
	{ 
		get { return _Mail_PeakMessagesDelivered; }
		set { _Mail_PeakMessagesDelivered = value; }
	}
	private int _Mail_PeakMessagesDelivered;


	public int Mail_PeakMessagesTransferred
	{ 
		get { return _Mail_PeakMessagesTransferred; }
		set { _Mail_PeakMessagesTransferred = value; }
	}
	private int _Mail_PeakMessagesTransferred;


	public string Mail_PeakMessageTransferredTime
	{ 
		get { return _Mail_PeakMessageTransferredTime; }
		set { _Mail_PeakMessageTransferredTime = value; }
	}
	private string _Mail_PeakMessageTransferredTime;


	public int Mail_RecallFailures
	{ 
		get { return _Mail_RecallFailures; }
		set { _Mail_RecallFailures = value; }
	}
	private int _Mail_RecallFailures;


	public int Mail_WaitingRecipients
	{ 
		get { return _Mail_WaitingRecipients; }
		set { _Mail_WaitingRecipients = value; }
	}
	private int _Mail_WaitingRecipients;


	public int ID
	{ 
		get { return _ID; }
		set { _ID = value; }
	}
	private int _ID;

	/// <summary>
	/// User defined Contructor
	/// <summary>
	public MailHealth(string Domino_Domain, 
		int Mailbox_Count, 
		float Mailbox_PerformanceIndex, 
		int Mail_Pending, 
		int Mail_Dead, 
		int Mail_Waiting, 
		int Mail_Held, 
		int Mail_MaximiumSizeDelivered, 
		string Mail_PeakMessageDeliveryTime, 
		int DeadThreshold, 
		int PendingThreshold, 
		int HeldMailThreshold, 
		int Mail_AverageDeliveryTime, 
		int Mail_AverageSizeDelivered, 
		int Mail_AverageServerHops, 
		int Mail_Transferred, 
		int Mail_Delivered, 
		int Mail_Transferred_NRPC, 
		int Mail_Transferred_SMTP, 
		int Mail_TransferThreads_Active, 
		int Mail_WaitingForDeliveryRetry, 
		int Mail_WaitingForDIR, 
		int Mail_WaitingForDNS, 
		int Mail_DeliveredSize_100KB_to_1MB, 
		int Mail_DeliveredSize_10KB_to_100KB, 
		int Mail_DeliveredSize_10MB_to_100MB, 
		int Mail_DeliveredSize_1KB_to_10KB, 
		int Mail_DeliveredSize_1MB_to_10MB, 
		int Mail_DeliveredSize_Under_1KB, 
		int Mail_Routed, 
		int Mail_PeakMessagesDelivered, 
		int Mail_PeakMessagesTransferred, 
		string Mail_PeakMessageTransferredTime, 
		int Mail_RecallFailures, 
		int Mail_WaitingRecipients, 
		int ID)
	{
		this._Domino_Domain = Domino_Domain;
		this._Mailbox_Count = Mailbox_Count;
		this._Mailbox_PerformanceIndex = Mailbox_PerformanceIndex;
		this._Mail_Pending = Mail_Pending;
		this._Mail_Dead = Mail_Dead;
		this._Mail_Waiting = Mail_Waiting;
		this._Mail_Held = Mail_Held;
		this._Mail_MaximiumSizeDelivered = Mail_MaximiumSizeDelivered;
		this._Mail_PeakMessageDeliveryTime = Mail_PeakMessageDeliveryTime;
		this._DeadThreshold = DeadThreshold;
		this._PendingThreshold = PendingThreshold;
		this._HeldMailThreshold = HeldMailThreshold;
		this._Mail_AverageDeliveryTime = Mail_AverageDeliveryTime;
		this._Mail_AverageSizeDelivered = Mail_AverageSizeDelivered;
		this._Mail_AverageServerHops = Mail_AverageServerHops;
		this._Mail_Transferred = Mail_Transferred;
		this._Mail_Delivered = Mail_Delivered;
		this._Mail_Transferred_NRPC = Mail_Transferred_NRPC;
		this._Mail_Transferred_SMTP = Mail_Transferred_SMTP;
		this._Mail_TransferThreads_Active = Mail_TransferThreads_Active;
		this._Mail_WaitingForDeliveryRetry = Mail_WaitingForDeliveryRetry;
		this._Mail_WaitingForDIR = Mail_WaitingForDIR;
		this._Mail_WaitingForDNS = Mail_WaitingForDNS;
		this._Mail_DeliveredSize_100KB_to_1MB = Mail_DeliveredSize_100KB_to_1MB;
		this._Mail_DeliveredSize_10KB_to_100KB = Mail_DeliveredSize_10KB_to_100KB;
		this._Mail_DeliveredSize_10MB_to_100MB = Mail_DeliveredSize_10MB_to_100MB;
		this._Mail_DeliveredSize_1KB_to_10KB = Mail_DeliveredSize_1KB_to_10KB;
		this._Mail_DeliveredSize_1MB_to_10MB = Mail_DeliveredSize_1MB_to_10MB;
		this._Mail_DeliveredSize_Under_1KB = Mail_DeliveredSize_Under_1KB;
		this._Mail_Routed = Mail_Routed;
		this._Mail_PeakMessagesDelivered = Mail_PeakMessagesDelivered;
		this._Mail_PeakMessagesTransferred = Mail_PeakMessagesTransferred;
		this._Mail_PeakMessageTransferredTime = Mail_PeakMessageTransferredTime;
		this._Mail_RecallFailures = Mail_RecallFailures;
		this._Mail_WaitingRecipients = Mail_WaitingRecipients;
		this._ID = ID;
	}
}
