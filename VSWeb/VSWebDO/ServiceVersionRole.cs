using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class ServiceVersionRole
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public ServiceVersionRole()
        { }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private int _Id;
        
        public int ServiceId
        {
            get { return _ServiceId; }
            set { _ServiceId = value; }
        }
        private int _ServiceId;

        public string VersionNo
        {
            get { return _VersionNo; }
            set { _VersionNo= value; }
        }
        private string _VersionNo;

        public int RoleId
        {
            get { return _RoleId; }
            set { _RoleId = value; }
        }
        private int _RoleId;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public ServiceVersionRole(int Id, int ServiceId,string VersionNo,int RoleId)
        {
            this._Id = Id;
            this._ServiceId = ServiceId;
            this._VersionNo = VersionNo;
            this._RoleId = RoleId;
        }

    }
}
