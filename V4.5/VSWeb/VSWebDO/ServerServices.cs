using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class ServerServices
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public ServerServices()
        { }

      
        public int ServerId
        {
            get { return _ServerId; }
            set { _ServerId = value; }
        }
        private int _ServerId;

       
        public int SVRId
        {
            get { return _SVRId; }
            set { _SVRId = value; }
        }
        private int _SVRId;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public ServerServices( int ServerId, int SVRId)
        {
            this._ServerId = ServerId;
            this._SVRId = SVRId;
          
        }

    }
}
