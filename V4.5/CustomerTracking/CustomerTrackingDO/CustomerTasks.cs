using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTrackingDO
{
    public class CustomerTasks
    {
        public CustomerTasks()
        { }
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Name;

        public string Status_Type
        {
            get { return _Status_Type; }
            set { _Status_Type = value; }
        }
        private string _Status_Type;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        private string _Address;

        public string ServerCount
        {
            get { return _ServerCount; }
            set { _ServerCount = value; }
        }
        private string _ServerCount;

        public string CompReplacement
        {
            get { return _CompReplacement; }
            set { _CompReplacement = value; }
        }
        private string _CompReplacement;

        public string OverallStatus
        {
            get { return _OverallStatus; }
            set { _OverallStatus = value; }
        }
        private string _OverallStatus;

        public string NextFollowUpDate
        {
            get { return _NextFollowUpDate; }
            set { _NextFollowUpDate = value; }
        }
        private string _NextFollowUpDate;

        public string LicExpDate
        {
            get { return _LicExpDate; }
            set { _LicExpDate = value; }
        }
        private string _LicExpDate;

        public string BudInfo
        {
            get { return _BudInfo; }
            set { _BudInfo = value; }
        }
        private string _BudInfo;

        
        public CustomerTasks(int ID,
            string Name,
            string Status_Type,
            string Address,
            string ServerCount,
            string CompReplacement,
            string OverallStatus,
            string NextFollowUpDate,
            string LicExpDate,
            string BudInfo
           )
        {
            this.ID = ID;
            this.Name = Name;
            this.Status_Type = Status_Type;
            this.Address = Address;
            this.ServerCount = ServerCount;
            this.CompReplacement = CompReplacement;
            this.OverallStatus = OverallStatus;
            this.NextFollowUpDate = NextFollowUpDate;
            this.LicExpDate = LicExpDate;
            this.BudInfo = BudInfo;
            
        }

    }
}
