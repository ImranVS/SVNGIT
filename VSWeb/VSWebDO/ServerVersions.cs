using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class ServerVersions
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public ServerVersions()
        { }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private int _Id;

        public string VersionNo
        {
            get { return _VersionNo; }
            set { _VersionNo= value; }
        }
        private string _VersionNo;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public ServerVersions(int Id, string VersionNo)
        {
            this._Id = Id;
             this._VersionNo= VersionNo;
        }

    }
}
