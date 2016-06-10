namespace VSWebDO
{
    public class InputParameters
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public InputParameters()
        { }
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;

        public int PSID
        {
            get { return _PSID; }
            set { _PSID = value; }
        }
        private int _PSID;


        public string Parameter
        {
            get { return _Parameter; }
            set { _Parameter = value; }
        }
        private string _Parameter;


        public string Threshold
        {
            get { return _Threshold; }
            set { _Threshold = value; }
        }
        private string _Threshold;


        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        private string _Type;


        public string Alias
        {
            get { return _Alias; }
            set { _Alias = value; }
        }
        private string _Alias;


        public bool ApplyThreshold
        {
            get { return _ApplyThreshold; }
            set { _ApplyThreshold = value; }
        }
        private bool _ApplyThreshold;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public InputParameters(
            int ID,
            int PSID,
            string Parameter,
            string Threshold,
            string Type,
            string Alias,
            bool ApplyThreshold)
        {
            this._ID = ID;
            this._PSID = PSID;
            this._Parameter = Parameter;
            this._Threshold = Threshold;
            this._Type = Type;
            this._Alias = Alias;
            this._ApplyThreshold = ApplyThreshold;
        }
    }
}
