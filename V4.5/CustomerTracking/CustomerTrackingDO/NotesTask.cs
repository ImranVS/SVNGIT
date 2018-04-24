using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTrackingDO
{
    public class NotesTask
    {
        public NotesTask()
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

        public NotesTask(int ID,
            int CustId,
            string Date,
            string Details)
        {
            this.CustID = CustId;
            this.ID = ID;
            this.Date = Date;
            this.Details = Details;
        }

    }
}
