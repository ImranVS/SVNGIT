using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTrackingDO
{
    public class VersionInfoTask
    {
        public VersionInfoTask()
        { }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;

        public string InstallDate
        {
            get { return _InstallDate; }
            set { _InstallDate = value; }
        }
        private string _InstallDate;

        public string VersionNumber
        {
            get { return _VersionNumber; }
            set { _VersionNumber = value; }
        }
        private string _VersionNumber;

        public string Details
        {
            get { return _Details; }
            set { _Details = value; }
        }
        private string _Details;


        public int CustID
        {
            get { return _CustID; }
            set { _CustID = value; }
        }
        private int _CustID;

        public VersionInfoTask(int ID,
            int CustID,
            string InstallDate,
            string VersionNumber,
            string Details)
        {
            this.ID = ID;
            this.CustID = CustID;
            this.InstallDate = InstallDate;
            this.VersionNumber = VersionNumber;
            this.Details = Details;
        }

    }
}
