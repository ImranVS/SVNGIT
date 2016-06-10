namespace VSWebDO
{
    public class ServerTaskSettings
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public ServerTaskSettings()
        { }


        public int TaskID
        {
            get { return _TaskID; }
            set { _TaskID = value; }
        }
        private int _TaskID;

        public int ServerID
        {
            get { return _ServerID; }
            set { _ServerID = value; }
        }
        private int _ServerID;


        public int MyID
        {
            get { return _MyID; }
            set { _MyID = value; }
        }
        private int _MyID;

		public int MinNoOfTasks
		{
			get { return _MinNoOfTasks; }
			set { _MinNoOfTasks = value; }
		}
		private int _MinNoOfTasks;
        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }
        private bool _Enabled;
		public bool SendMinTasksLoadCmd
		{
			get { return _SendMinTasksLoadCmd; }
			set { _SendMinTasksLoadCmd = value; }
		}
		private bool _SendMinTasksLoadCmd;
		public bool IsMinTasksEnabled
		{
			get { return _IsMinTasksEnabled; }
			set { _IsMinTasksEnabled = value; }
		}
		private bool _IsMinTasksEnabled;

        public bool SendLoadCommand
        {
            get { return _SendLoadCommand; }
            set { _SendLoadCommand = value; }
        }
        private bool _SendLoadCommand;


        public bool SendRestartCommand
        {
            get { return _SendRestartCommand; }
            set { _SendRestartCommand = value; }
        }
        private bool _SendRestartCommand;


        public bool SendExitCommand
        {
            get { return _SendExitCommand; }
            set { _SendExitCommand = value; }
        }
        private bool _SendExitCommand;


        public bool RestartOffHours
        {
            get { return _RestartOffHours; }
            set { _RestartOffHours = value; }
        }
        private bool _RestartOffHours;

        public int Modified_By
        {
            get { return _Modified_By; }
            set { _Modified_By = value; }
        }
        private int _Modified_By;
        public string Modified_On
        {
            get { return _Modified_On; }
            set { _Modified_On = value; }
        }
        private string _Modified_On;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public ServerTaskSettings(
            int TaskID,
            int ServerID,
            int MyID,
			   int MinNoOfTasks,
			
			 bool SendMinTasksLoadCmd,
			 bool IsMinTasksEnabled,
            bool Enabled,
            bool SendLoadCommand,
            bool SendRestartCommand,
            bool SendExitCommand,
            bool RestartOffHours,
            int Modified_By,
            string Modified_On)
        {
            this._TaskID = TaskID;
            this._ServerID = ServerID;
            this._MyID = MyID;
            this._Enabled = Enabled;
			this._MinNoOfTasks = MinNoOfTasks;
            this._SendLoadCommand = SendLoadCommand;
            this._SendRestartCommand = SendRestartCommand;
            this._SendExitCommand = SendExitCommand;
            this._RestartOffHours = RestartOffHours;
            this._Modified_By = Modified_By;
            this._Modified_On = Modified_On;
			this._IsMinTasksEnabled = IsMinTasksEnabled;
			this._SendMinTasksLoadCmd = SendMinTasksLoadCmd;
        }
    }
}