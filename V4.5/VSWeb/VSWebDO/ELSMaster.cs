namespace VSWebDO
{
    public class ELSMaster
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public ELSMaster()
        { }
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;

        public string EventId
        {
            get { return _eventId; }
            set { _eventId = value; }
        }
        private string _eventId;

		public string EventName
		{
			get { return _eventName; }
			set { _eventName = value; }
		}
		private string _eventName;

		public string AliasName
		{
			get { return _aliasName; }
			set { _aliasName = value; }
		}
		private string _aliasName;

        public string EventKey
        {
            get { return _eventKey; }
            set { _eventKey = value; }
        }
        private string _eventKey;

		public string EventLevel
		{
			get { return _eventLevel; }
			set { _eventLevel = value; }
		}
		private string _eventLevel;

        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }
        private string _source;
		public string TaskCategory
		{
			get { return _taskCategory; }
			set { _taskCategory = value; }
		}
		private string _taskCategory;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public ELSMaster(string EventId,
            string EventKey,
            string Source,
			string TaskCategory,
			string EventLevel,
            int ID)
        {
            this._ID = ID;
            this._eventId = EventId ;
            this._eventKey = EventKey;
            this._source = Source;
			this._taskCategory = TaskCategory;
			this._eventLevel = EventLevel;
        }
    }
}
