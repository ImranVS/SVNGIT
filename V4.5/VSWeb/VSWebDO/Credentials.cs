namespace VSWebDO
{
    public class Credentials
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public Credentials()
        { }
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;

        public string AliasName
        {
            get { return _AliasName; }
            set { _AliasName = value; }
        }
        private string _AliasName;


        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        private string _UserID;


        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        private string _Password;
		public int ServerTypeID
		{
			get { return _ServerTypeID; }
			set { _ServerTypeID = value; }
		}
		private int _ServerTypeID;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public Credentials(string AliasName,
            string UserID,
            string Password,
            int ID)
        {
            this._ID = ID;
            this._AliasName = AliasName;
            this._UserID = UserID;
            this._Password = Password;
        }
    }
}
