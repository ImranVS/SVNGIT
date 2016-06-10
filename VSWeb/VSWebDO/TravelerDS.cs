using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class TravelerDS
    {
        public TravelerDS()
        {}

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;

        public string TravelerPoolName
        {
            get { return _TravelerPoolName; }
            set { _TravelerPoolName = value; }
        }
        private string _TravelerPoolName;

        public string ServerName
        {
            get { return _ServerName; }
            set { _ServerName = value; }
        }
        private string _ServerName;

        public string DataStore
        {
            get { return _DataStore; }
            set { _DataStore = value; }
        }
        private string _DataStore;

        public string Port
        {
            get { return _Port; }
            set { _Port = value; }
        }
        private string _Port;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        private string _UserName;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        private string _Password;

        public string IntegratedSecurity
        {
            get { return _IntegratedSecurity; }
            set { _IntegratedSecurity = value; }
        }
        private string _IntegratedSecurity;

        public string TestScan
        {
            get { return _TestScan; }
            set { _TestScan = value; }
        }
        private string _TestScan;

        public string UsedByServers
        {
            get { return _UsedByServers; }
            set { _UsedByServers = value; }
        }
        private string _UsedByServers;

        //7/18/2014 NS added for VSPLUS-806
        public string DatabaseName
        {
            get { return _DatabaseName; }
            set { _DatabaseName = value; }
        }
        private string _DatabaseName;
    }
}
