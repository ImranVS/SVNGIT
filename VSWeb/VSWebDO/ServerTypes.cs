using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
   public class ServerTypes
    {
       
          /// <summary>
	/// Default Contructor
	/// <summary>
       public ServerTypes()
	{}

        public int ID
        { 
            get { return _ID; }
            set { _ID = value; }  
        }
        private int _ID;
        public string ServerType
        {
            get { return _ServerType; }
            set { _ServerType = value; }
        }
        private string _ServerType;
        /// <summary>
        /// User defined Contructor
        /// <summary>

        public ServerTypes(int ID,
             string ServerType)
        {
            this._ID = ID;
            this._ServerType = ServerType;
        }

   }

}
