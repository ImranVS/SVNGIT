namespace VSWebDO
{
    public class MaintenanceSettings
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public MaintenanceSettings()
        { }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Name;

        public bool Monday
        {
            get { return _Monday; }
            set { _Monday = value; }
        }
        private bool _Monday;


        public bool Tuesday
        {
            get { return _Tuesday; }
            set { _Tuesday = value; }
        }
        private bool _Tuesday;


        public bool Wednesday
        {
            get { return _Wednesday; }
            set { _Wednesday = value; }
        }
        private bool _Wednesday;


        public bool Thursday
        {
            get { return _Thursday; }
            set { _Thursday = value; }
        }
        private bool _Thursday;


        public bool Friday
        {
            get { return _Friday; }
            set { _Friday = value; }
        }
        private bool _Friday;


        public bool Saturday
        {
            get { return _Saturday; }
            set { _Saturday = value; }
        }
        private bool _Saturday;


        public bool Sunday
        {
            get { return _Sunday; }
            set { _Sunday = value; }
        }
        private bool _Sunday;


        public string StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }
        private string _StartTime;


        public string EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }
        private string _EndTime;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public MaintenanceSettings(bool Monday,
            bool Tuesday,
            bool Wednesday,
            bool Thursday,
            bool Friday,
            bool Saturday,
            bool Sunday,
            string StartTime,
            string EndTime)
        {
            this._Monday = Monday;
            this._Tuesday = Tuesday;
            this._Wednesday = Wednesday;
            this._Thursday = Thursday;
            this._Friday = Friday;
            this._Saturday = Saturday;
            this._Sunday = Sunday;
            this._StartTime = StartTime;
            this._EndTime = EndTime;
        }
    }
}