using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class Features
    {
          /// <summary>
	/// Default Contructor
	/// <summary>
        public Features()
	{}

        public int ID
        { 
            get { return _ID; }
            set { _ID = value; }  
        }
        private int _ID;
        public string Name
        {
            get { return _Feature; }
            set { _Feature = value; }
        }
        private string _Feature;
        /// <summary>
        /// User defined Contructor
        /// <summary>
        public Features(
        int ID,
        string Name)
            {
        this._ID = ID;
        this._Feature = Name;
        }
           
        
    }
}
