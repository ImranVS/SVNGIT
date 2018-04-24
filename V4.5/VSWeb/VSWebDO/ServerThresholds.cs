using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
  public class ServerThresholds
    {
        /// <summary>
	/// Default Contructor
	/// <summary>
       public ServerThresholds()
	{}

        public int ID
        { 
            get { return _ID; }
            set { _ID = value; }  
        }
        private int _ID;
        public int ServerID
        {
            get { return _ServerID; }
            set { 
                _ServerID = value; }
        }
        private int _ServerID;
      
        public int ParameterID
        { 
            get { return _ParameterID; }
            set {_ParameterID = value; }  
        }
        private int _ParameterID;
        public int Threshold
        {
            get { return _Threshold; }
            set { 
                _Threshold = value; }
        }
        private int _Threshold;

         public string Type
        { 
            get { return _Type; }
            set { _Type = value; }  
        }
        private string _Type;
        public bool ApplyAlert
        {
            get { return _ApplyAlert; }
            set { 
                _ApplyAlert = value; }
        }
        private bool _ApplyAlert;

        public int PSID
        {
            get { return _PSID; }
            set
            {
                _PSID = value;
            }
        }
        private int _PSID;

        public ServerThresholds(int ID,
             int ServerID,
            int ParameterID,
        int Threshold,
            string Type,
            bool ApplyAlert,
            int PSID
            )
        {
            this._ID = ID;
            this._ServerID = ServerID;
            this._ParameterID = ParameterID;
            this._Threshold = Threshold;
            this._Type = Type;
            this._ApplyAlert = ApplyAlert;
            this._PSID = PSID;
        }

   }
    
}
