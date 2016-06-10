using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTrackingDO
{
    public class ContactsTask
    {
        public ContactsTask()
        { }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;


        public int CustID
        {
            get { return _CustID; }
            set { _CustID = value; }
        }
        private int _CustID;

        public string ContactName
        {
            get { return _ContactName; }
            set { _ContactName = value; }
        }
        private string _ContactName;

        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { _PhoneNumber = value; }
        }
        private string _PhoneNumber;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        private string _Title;

        public string Details
        {
            get { return _Details; }
            set { _Details = value; }
        }
        private string _Details;

        public ContactsTask(int ID, 
            int CustId,
            string PhoneNumber,
            string Title,
            string Details,
            string ContactName)
        {
            this.ID = ID;
            this.CustID = CustId;
            this.ContactName = ContactName;
            this.PhoneNumber = PhoneNumber;
            this.Title = Title;
            this.Details = Details;
        }

    }
}
