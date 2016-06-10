using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class UserProfileDetailed
    {
          /// <summary>
	/// Default Contructor
	/// <summary>
        public UserProfileDetailed()
	{}

        public int ID
        { 
            get { return _ID; }
            set { _ID = value; }  
        }
        private int _ID;

        public int UserProfileMasterID
        {
            get { return _UserProfileMasterID; }
            set { _UserProfileMasterID = value; }
        }
        private int _UserProfileMasterID;

        public int ProfilesMasterID
        {
            get { return _ProfilesMasterID; }
            set { _ProfilesMasterID = value; }
        }
        private int _ProfilesMasterID;


        public string Value
        {
            get { return _Value; }
            set { _Value= value; }
        }
        private string _Value;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public UserProfileDetailed(
            int ID,
            int UserProfileMasterID,
            int ProfilesMasterID,
            string Value)
            {
                this._ID = ID;
                this._UserProfileMasterID = UserProfileMasterID;
                this._ProfilesMasterID = ProfilesMasterID;
                this._Value = Value;
        }
           
        
    }
}
