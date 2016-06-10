using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class UserProfileMaster
    {
          /// <summary>
	/// Default Contructor
	/// <summary>
        public UserProfileMaster()
	{}

        public int ID
        { 
            get { return _ID; }
            set { _ID = value; }  
        }
        private int _ID;
        public string Name
        {
            get { return _Name; }
            set { _Name= value; }
        }
        private string _Name;
        /// <summary>
        /// User defined Contructor
        /// <summary>
        public UserProfileMaster(
        int ID,
        string Name)
            {
        this._ID = ID;
        this._Name = Name;
        }
           
        
    }
}
