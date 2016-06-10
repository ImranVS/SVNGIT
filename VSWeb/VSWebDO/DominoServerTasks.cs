namespace VSWebDO
{
    public class DominoServerTasks
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public DominoServerTasks()
        { }
        public int TaskID
        {
            get { return _TaskID; }
            set { _TaskID = value; }
        }
        private int _TaskID;

        public string TaskName
        {
            get { return _TaskName; }
            set { _TaskName = value; }
        }
        private string _TaskName;


        public string ConsoleString
        {
            get { return _ConsoleString; }
            set { _ConsoleString = value; }
        }
        private string _ConsoleString;


        public int RetryCount
        {
            get { return _RetryCount; }
            set { _RetryCount = value; }
        }
        private int _RetryCount;


        public bool FreezeDetect
        {
            get { return _FreezeDetect; }
            set { _FreezeDetect = value; }
        }
        private bool _FreezeDetect;


        public int MaxBusyTime
        {
            get { return _MaxBusyTime; }
            set { _MaxBusyTime = value; }
        }
        private int _MaxBusyTime;


        public string IdleString
        {
            get { return _IdleString; }
            set { _IdleString = value; }
        }
        private string _IdleString;


        public string LoadString
        {
            get { return _LoadString; }
            set { _LoadString = value; }
        }
        private string _LoadString;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public DominoServerTasks(int TaskID,
            string TaskName,
            string ConsoleString,
            int RetryCount,
            bool FreezeDetect,
            int MaxBusyTime,
            string IdleString,
            string LoadString)
        {

            this._TaskID = TaskID;
            this._TaskName = TaskName;
            this._ConsoleString = ConsoleString;
            this._RetryCount = RetryCount;
            this._FreezeDetect = FreezeDetect;
            this._MaxBusyTime = MaxBusyTime;
            this._IdleString = IdleString;
            this._LoadString = LoadString;
        }
    }
}