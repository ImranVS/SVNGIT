namespace VSWebDO
{
    public class DominoDiskSpace
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public DominoDiskSpace()
        { }


        public string DiskName
        {
            get { return _DiskName; }
            set { _DiskName = value; }
        }
        private string _DiskName;


        public float DiskFree
        {
            get { return _DiskFree; }
            set { _DiskFree = value; }
        }
        private float _DiskFree;


        public float DiskSize
        {
            get { return _DiskSize; }
            set { _DiskSize = value; }
        }
        private float _DiskSize;


        public float PercentFree
        {
            get { return _PercentFree; }
            set { _PercentFree = value; }
        }
        private float _PercentFree;


        public float PercentUtilization
        {
            get { return _PercentUtilization; }
            set { _PercentUtilization = value; }
        }
        private float _PercentUtilization;


        public int AverageQueueLength
        {
            get { return _AverageQueueLength; }
            set { _AverageQueueLength = value; }
        }
        private int _AverageQueueLength;


        public System.DateTime Updated
        {
            get { return _Updated; }
            set { _Updated = value; }
        }
        private System.DateTime _Updated;


        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;


        public float Threshold
        {
            get { return _Threshold; }
            set { _Threshold = value; }
        }
        private float _Threshold;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public DominoDiskSpace(string DiskName,
            float DiskFree,
            float DiskSize,
            float PercentFree,
            float PercentUtilization,
            int AverageQueueLength,
            System.DateTime Updated,
            int ID,
            float Threshold)
        {
            this._DiskName = DiskName;
            this._DiskFree = DiskFree;
            this._DiskSize = DiskSize;
            this._PercentFree = PercentFree;
            this._PercentUtilization = PercentUtilization;
            this._AverageQueueLength = AverageQueueLength;
            this._Updated = Updated;
            this._ID = ID;
            this._Threshold = Threshold;
        }
    }
}