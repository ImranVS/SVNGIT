using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class ActiveDirectory
    {
        public ActiveDirectory()
        { }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;

        public int ServerID
        {
            get { return _ServerID; }
            set { _ServerID = value; }
        }
        private int _ServerID;

        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        private string _Category;

        public int ScanInterval
        {
            get { return _ScanInterval; }
            set { _ScanInterval = value; }
        }
        private int _ScanInterval;


        public int CredentialsID
        {
            get { return _CredentialsID; }
            set { _CredentialsID = value; }
        }
        private int _CredentialsID;

        public int RetryInterval
        {
            get { return _RetryInterval; }
            set { _RetryInterval = value; }
        }
        private int _RetryInterval;

        public int ResponseThreshold
        {
            get { return _ResponseThreshold; }
            set { _ResponseThreshold = value; }
        }
        private int _ResponseThreshold;


        public int OffHoursScanInterval
        {
            get { return _OffHoursScanInterval; }
            set { _OffHoursScanInterval = value; }
        }
        private int _OffHoursScanInterval;


        public int FailureThreshold
        {
            get { return _FailureThreshold; }
            set { _FailureThreshold = value; }
        }
        private int _FailureThreshold;

        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }
        private bool _Enabled;

        public float MemoryThreshold
        {
            get { return _MemoryThreshold; }
            set { _MemoryThreshold = value; }
        }
        private float _MemoryThreshold;

        public float CPUThreshold
        {
            get { return _CPUThreshold; }
            set { _CPUThreshold = value; }
        }
        private float _CPUThreshold;

        public ActiveDirectory(
            int ID,
            int ServerID,
            string Category,
            int ScanInterval,
            int RetryInterval,
            int OffHoursScanInterval,
            int ResponseThreshold,
            int FailureThreshold,
            float MemoryThreshold,
            float CPUThreshold,
            int CredentialsID,
            bool Enabled
            )
        {
            this._ID = ID;
            this._ServerID = ServerID;
            this._Category = Category;
            this._ScanInterval = ScanInterval;
            this._RetryInterval = RetryInterval;
            this._OffHoursScanInterval = OffHoursScanInterval;
            this._ResponseThreshold = ResponseThreshold;
            this._FailureThreshold = FailureThreshold;
            this._MemoryThreshold = MemoryThreshold;
            this._CPUThreshold = CPUThreshold;
            this._CredentialsID = CredentialsID;
            this._Enabled = Enabled;
        }
    }
}

