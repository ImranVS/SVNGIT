using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class ServerAttributes
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public ServerAttributes()
        { }

        public int ServerId
        {
            get { return _ServerId; }
            set { _ServerId = value; }
        }
        private int _ServerId;

        

        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }

        }
        private bool _Enabled;

        public bool ScanDAGHealth
        {
            get { return _ScanDAGHealth; }
            set { _ScanDAGHealth = value; }

        }
        private bool _ScanDAGHealth;

        public int ScanInterval
        {
            get { return _ScanInterval; }
            set { _ScanInterval = value; }

        }
        private int _ScanInterval;

        public int RetryInterval
        {
            get { return _RetryInterval; }
            set { _RetryInterval = value; }

        }
        private int _RetryInterval;

        public int OffHourInterval
        {
            get { return _OffHourInterval; }
            set { _OffHourInterval = value; }

        }
        private int _OffHourInterval;

        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        private string _Category;


        public int CPUThreshold
        {
            get { return _CPUThreshold; }
            set { _CPUThreshold = value; }

        }
        private int _CPUThreshold;


        public int MemThreshold
        {
            get { return _MemThreshold; }
            set { _MemThreshold = value; }

        }
        private int _MemThreshold;

        public int ResponseTime
        {
            get { return _ResponseTime; }
            set { _ResponseTime = value; }

        }
        private int _ResponseTime;

        public int ConsFailuresBefAlert
        {
            get { return _ConsFailuresBefAlert; }
            set { _ConsFailuresBefAlert = value; }

        }
        private int _ConsFailuresBefAlert;

        public int ConsOvrThresholdBefAlert
        {
            get { return _ConsOvrThresholdBefAlert; }
            set { _ConsOvrThresholdBefAlert = value; }

        }
        private int _ConsOvrThresholdBefAlert;

		public int CredentialsId
		{
			get { return _CredentialsId; }
			set { _CredentialsId = value; }

		}
		private int _CredentialsId;
        /// <summary>
        /// User defined Contructor
        /// <summary>


        public ServerAttributes(int ServerID,bool Enabled,bool ScanDAGHealth,int ScanInterval,int RetryInterval,int OffHourInterval,string Category,int CPUThreshold,
			int MemThreshold, int ResponseTime, int ConsFailuresBefAlert, int ConsOvrThresholdBefAlert, int CredentialsId)
        {
            this._ServerId = ServerId;
           

            this._Enabled = Enabled;
            this._ScanDAGHealth = ScanDAGHealth;
            this._ScanInterval = ScanInterval;
            this._RetryInterval = RetryInterval;
            this._OffHourInterval = OffHourInterval;
            this._Category = Category;
            this._CPUThreshold = CPUThreshold;
            this._MemThreshold = MemThreshold;
            this._ResponseTime = ResponseTime;
            this._ConsFailuresBefAlert = ConsFailuresBefAlert;
            this._ConsOvrThresholdBefAlert = ConsOvrThresholdBefAlert;
			this._CredentialsId = CredentialsId;

        }


     
    }

}
