using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class ProfileNames
    {
          /// <summary>
	/// Default Contructor
	/// <summary>
        public ProfileNames()
	{}

        public int ID
        { 
            get { return _ID; }
            set { _ID = value; }  
        }
        private int _ID;
        public string ProfileName
        {
            get { return _ProfileName; }
            set { _ProfileName = value; }
        }
        private string _ProfileName;
        /// <summary>
        /// User defined Contructor
        /// <summary>
        public ProfileNames(
        int ID,
        string ProfileName)
            {
        this._ID = ID;
        this.ProfileName = ProfileName;
        }
           
        
    }
}

        
    
