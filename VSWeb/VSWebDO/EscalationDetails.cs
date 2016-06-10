using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class EscalationDetails
    {
        public EscalationDetails()
        {
        }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;

        public int AlertKey
        {
            get { return _AlertKey; }
            set { _AlertKey = value; }
        }
        private int _AlertKey;

        public string SMSTo
        {
            get { return _SMSTo; }
            set { _SMSTo = value; }

        }
        private string _SMSTo;
        
        public int ScriptID
        {
            get { return _ScriptID; }
            set { _ScriptID = value; }
        }
        private int _ScriptID;

        //3/31/2015 NS added for VSPLUS-219
        public string EscalateTo
        {
            get { return _EscalateTo; }
            set { _EscalateTo = value; }

        }
        private string _EscalateTo;

        public int EscalationInterval
        {
            get { return _EscalationInterval; }
            set { _EscalationInterval = value; }

        }
        private int _EscalationInterval;

        public EscalationDetails(int ID,
            int AlertKey,
            string EscalateTo,
            string SMSTo,
            int ScriptID,
            int EscalationInterval)
        {
           this._ID = ID;
           this._AlertKey = AlertKey;
           this._EscalateTo = EscalateTo;
           this._SMSTo = SMSTo;
           this._ScriptID = ScriptID;
           this._EscalationInterval = EscalationInterval;
       }
    }
}
