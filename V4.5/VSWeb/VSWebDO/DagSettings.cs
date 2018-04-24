using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class DagSettings
    {
        public DagSettings()
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



        public string PrimaryConnection
        {
            get { return _PrimaryConnection; }
            set { _PrimaryConnection = value; }

        }
        private string _PrimaryConnection;

        public string BackupConnection
        {
            get { return _BackupConnection; }
            set { _BackupConnection = value; }

        }
        private string _BackupConnection;

        public int ReplyQThreshold
        {
            get { return _ReplyQThreshold; }
            set { _ReplyQThreshold = value; }

        }
        private int _ReplyQThreshold;

        public int CopyQThreshold
        {
            get { return _CopyQThreshold; }
            set { _CopyQThreshold = value; }

        }
        private int _CopyQThreshold;
        public DagSettings(int ID, int ServerID, string PrimaryConnection, string BackupConnection, int ReplyQThreshold, int CopyQThreshold)
        {
            this._ID = ID;
            this._ServerID = ServerID;
            this._PrimaryConnection = PrimaryConnection;
            this._BackupConnection = BackupConnection;
            this._ReplyQThreshold = ReplyQThreshold;
            this._CopyQThreshold = CopyQThreshold;
        }
    }
}

