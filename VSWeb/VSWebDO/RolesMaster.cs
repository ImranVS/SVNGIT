using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class RolesMaster
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public RolesMaster()
        { }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private int _Id;

        public string RoleName
        {
            get { return _RoleName; }
            set { _RoleName = value; }
        }
        private string _RoleName;

        public int ServerTypeId
        {
            get { return _ServerTypeId; }
            set { _ServerTypeId = value; }
        }
        private int _ServerTypeId;
        
        /// <summary>
        /// User defined Contructor
        /// <summary>
        public RolesMaster(int Id, string RoleName, int ServerTypeId)
        {
            this._Id = Id;
            this._RoleName = RoleName;
            this._ServerTypeId = ServerTypeId;
        }
    }
}
