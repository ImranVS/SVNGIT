namespace VSWebDO
{
    public class DiskSettings
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public DiskSettings()
        { }

        public string ServerName
        {
            get { return _ServerName; }
            set { _ServerName = value; }
        }
        private string _ServerName;

        public string DiskName
        {
            get { return _DiskName; }
            set { _DiskName = value; }
        }
        private string _DiskName;

        public float Threshold
        {
            get { return _Threshold; }
            set { _Threshold = value; }
        }
        private float _Threshold;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public DiskSettings(string ServerName,
            string DiskName,
            float Threshold)
        {
            this._DiskName = DiskName;
            this._ServerName = ServerName;
            this._Threshold = Threshold;
        }
    }
}