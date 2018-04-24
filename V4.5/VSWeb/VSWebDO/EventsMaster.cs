using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class EventsMaster
    {
        public EventsMaster()
        {
        }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;
        public string EventName
        {
            get { return _EventName; }
            set { _EventName = value; }
        
        }
        private string _EventName;

        public int ServerTypeID
        {
            get { return _ServerTypeID; }
            set { _ServerTypeID = value; }
          
        }
        private int _ServerTypeID;

        public EventsMaster(int ID,
            string EventName,
            int ServerTypeID)
        {
            this._ID = ID;
            this._ServerTypeID = ServerTypeID;
            this._EventName = EventName;
        
        }

        
    }
}
