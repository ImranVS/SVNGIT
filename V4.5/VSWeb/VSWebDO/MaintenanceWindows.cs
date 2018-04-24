namespace VSWebDO
{
    public class MaintenanceWindows
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public MaintenanceWindows()
        { }


        public string DeviceType
        {
            get { return _DeviceType; }
            set { _DeviceType = value; }
        }
        private string _DeviceType;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Name;


        public string MaintWindow
        {
            get { return _MaintWindow; }
            set { _MaintWindow = value; }
        }
        private string _MaintWindow;


        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public MaintenanceWindows(string Name,
            string MaintWindow,
            int ID)
        {
            this._Name = Name;
            this._MaintWindow = MaintWindow;
            this._ID = ID;
        }
    }
}