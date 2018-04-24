using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class DominoServersMD
    {

        #region "Private Variables Declaration"

        private string _Name;
        private string _Description;
        private string _Category;
        private int _PendingThreshold;
        private int _DeadThreshold;
        private bool _Enabled;
        private int _ScanInterval;
        private int _OffHoursScanInterval;
        private string _Location;
        private int _Key;
        private string _MailDirectory;
        private int _RetryInterval;
        private int _ResponseThreshold;
        private bool _BES_Server;
        private int _BES_Threshold;
        private int _FailureThreshold;
        private string _SearchString;
        private bool _AdvancedMailScan;
        private int _DeadMailDeleteThreshold;
        private float _DiskSpaceThreshold;
        private string _IPAddress;
        private float _HeldThreshold;
        private bool _ScanDBHealth;
        private string _NotificationGroup;
        private float _Memory_Threshold;
        private float _CPU_Threshold;
        private float _Cluster_Rep_Delays_Threshold;

        #endregion

        #region "Constructor"

        public DominoServersMD()
        {
            _Name = "";
            _Description = "";
            _Category = "";
            _PendingThreshold = 0;
            _DeadThreshold = 0;
            _Enabled = false;
            _ScanInterval = 0;
            _OffHoursScanInterval = 0;
            _Location = "";
            _Key = 0;
            _MailDirectory = "";
            _RetryInterval = 0;
            _ResponseThreshold =0;
            _BES_Server =false;
            _BES_Threshold = 0;
            _FailureThreshold = 0;
            _SearchString = "";
            _AdvancedMailScan = false;
            _DeadMailDeleteThreshold =0;
            _DiskSpaceThreshold = 0;
            _IPAddress = "";
            _HeldThreshold = 0;
            _ScanDBHealth = false;
            _NotificationGroup = "";
            _Memory_Threshold =0;
            _CPU_Threshold = 0;
            _Cluster_Rep_Delays_Threshold = 0;
        }
        #endregion

        #region "Public variables Declaration equal to table, get set"

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        public int PendingThreshold
        {
            get { return _PendingThreshold; }
            set { _PendingThreshold = value; }
        }
        public int DeadThreshold
        {
            get { return _DeadThreshold; }
            set { _DeadThreshold = value; }
        }
        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }
        public int ScanInterval
        {
            get { return _ScanInterval; }
            set { _ScanInterval = value; }
        }
        public int OffHoursScanInterval
        {
            get { return _OffHoursScanInterval; }
            set { _OffHoursScanInterval = value; }
        }
        public string Location
        {
            get { return _Location; }
            set { _Location = value; }
        }
        public int Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        public string MailDirectory
        {
            get { return _MailDirectory; }
            set { _MailDirectory = value; }
        }
        public int RetryInterval
        {
            get { return _RetryInterval; }
            set { _RetryInterval = value; }
        }
        public int ResponseThreshold
        {
            get { return _ResponseThreshold; }
            set { _ResponseThreshold = value; }
        }
        public bool BES_Server
        {
            get { return _BES_Server; }
            set { _BES_Server = value; }
        }
        public int BES_Threshold
        {
            get { return _BES_Threshold; }
            set { _BES_Threshold = value; }
        }
        public int FailureThreshold
        {
            get { return _FailureThreshold; }
            set { _FailureThreshold = value; }
        }
        public string SearchString
        {
            get { return _SearchString; }
            set { _SearchString = value; }
        }
        public bool AdvancedMailScan
        {
            get { return _AdvancedMailScan; }
            set { _AdvancedMailScan = value; }
        }
        public int DeadMailDeleteThreshold
        {
            get { return _DeadMailDeleteThreshold; }
            set { _DeadMailDeleteThreshold = value; }
        }
        public float DiskSpaceThreshold
        {
            get { return _DiskSpaceThreshold; }
            set { _DiskSpaceThreshold = value; }
        }
        public string IPAddress
        {
            get { return _IPAddress; }
            set { _IPAddress = value; }
        }
        public float HeldThreshold
        {
            get { return _HeldThreshold; }
            set { _HeldThreshold = value; }
        }
        public bool ScanDBHealth
        {
            get { return _ScanDBHealth; }
            set { _ScanDBHealth = value; }
        }
        public string NotificationGroup
        {
            get { return _NotificationGroup; }
            set { _NotificationGroup = value; }
        }
        public float Memory_Threshold
        {
            get { return _Memory_Threshold; }
            set { _Memory_Threshold = value; }
        }
        public float CPU_Threshold
        {
            get { return _CPU_Threshold; }
            set { _CPU_Threshold = value; }
        }
        public float Cluster_Rep_Delays_Threshold
        {
            get { return _Cluster_Rep_Delays_Threshold; }
            set { _Cluster_Rep_Delays_Threshold = value; }
        }
        #endregion
 
    }
}
