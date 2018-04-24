using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTrackingDO
{
    public class TicketsTask
    {
        public TicketsTask()
        { }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;

        public string Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        private string _Date;

        public string TicketNumber
        {
            get { return _TicketNumber; }
            set { _TicketNumber = value; }
        }
        private string _TicketNumber;

        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private string _Status;

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

        public TicketsTask(int ID,
            int CustID,
            string Date,
            string TicketNumber,
            string Status,
            string Details)
        {
            this.ID = ID;
            this.CustID = CustID;
            this.Date = Date;
            this.TicketNumber = TicketNumber;
            this.Status = Status;
            this.Details = Details;
        }

    }
}
