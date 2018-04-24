using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class ServiceMaster
    {
        /// <summary>
        /// Default Contructor
        /// <summary>
        public ServiceMaster()
        { }

      
        public int ServerId
        {
            get { return _ServerId; }
            set { _ServerId = value; }
        }
        private int _ServerId;


        public string ServiceName
        {
            get { return _ServiceName; }
            set { _ServiceName = value; }
        }
        private string _ServiceName;

        public string ServiceShortName
        {
            get { return _ServiceShortName; }
            set { _ServiceShortName = value; }
        }
        private string _ServiceShortName;

        public string SecurityContext
        {
            get { return _SecurityContext; }
            set { _SecurityContext = value; }
        }
        private string _SecurityContext;

        public string ServiceDescription
        {
            get { return _ServiceDescription; }
            set { _ServiceDescription = value; }
        }
        private string _ServiceDescription;

        public string DefaultStartupType
        {
            get { return _DefaultStartupType; }
            set { _DefaultStartupType = value; }
        }
        private string _DefaultStartupType;

        public string Required
        {
            get { return _Required; }
            set { _Required = value; }
        }
        private string _Required;

        public bool Custom
        {
            get { return _Custom; }
            set { _Custom = value; }
        }
        private bool _Custom;

        /// <summary>
        /// User defined Contructor
        /// <summary>
        public ServiceMaster(int ServerId, string ServiceName, string ServiceShortName, string SecurityContext, string ServiceDescription, 
            string DefaultStartupType,string Required,bool Custom)
        {
            this._ServerId = ServerId;
            this._ServiceName =  ServiceName;
            this._ServiceShortName = ServiceShortName;
            this._SecurityContext = SecurityContext;
            this._ServiceDescription= ServiceDescription;
            this._DefaultStartupType = DefaultStartupType;
            this._Required= Required;
            this._Custom = Custom;
        }

    }
}
