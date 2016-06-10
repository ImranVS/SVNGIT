using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
     public class Servers
    {
          /// <summary>
	/// Default Contructor
	/// <summary>
         public Servers()
	{}

        public int ID
        { 
            get { return _ID; }
            set { _ID = value; }  
        }
        private int _ID;

        public string ServerName
        {
            get { return _ServerName; }
            set {  _ServerName=value; }
        }
        private string _ServerName;

        public int ServerTypeID
        {
            get { return _ServerTypeID; }
            set { _ServerTypeID = value; }
        
        }
        private int _ServerTypeID;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        private string _Description;

        public int LocationID
        {
            get { return _LocationID; }
            set { _LocationID = value; }
        
        }
        private int _LocationID;

        public string IPAddress
        {

            get { return _IPAddress; }
            set { _IPAddress = value; }
        }
        private string _IPAddress;

		public string ProfileName
		{

			get { return _ProfileName; }
			set { _ProfileName = value; }
		}
		private string _ProfileName;

		public int BusinesshoursID
		{
			get { return _BusinesshoursID; }
			set { _BusinesshoursID = value; }

		}
		private int _BusinesshoursID;

		public string ServerType
		{

			get { return _ServerType; }
			set { _ServerType = value; }
		}
		private string _ServerType;
		public double MonthlyOperatingCost
		{

			get { return _MonthlyOperatingCost; }
			set { _MonthlyOperatingCost = value; }
		}
		private double _MonthlyOperatingCost;
		public int IdealUserCount
		{

			get { return _IdealUserCount; }
			set { _IdealUserCount = value; }
		}
		private int _IdealUserCount;

        /// <summary>
        /// User defined Contructor
        /// <summary>


        public Servers(int ID, string ServerName,

         int ServerTypeID, string Description,int BusinesshoursID,
            int LocationID, string IPAddress)
        {
            this._ID = ID;
            this._ServerName = ServerName;
            this._ServerTypeID = ServerTypeID;
            this._Description = Description;
            this._LocationID = LocationID;
            this._IPAddress = IPAddress;
			this._ProfileName = ProfileName;
			this._BusinesshoursID = BusinesshoursID;
			this._MonthlyOperatingCost = MonthlyOperatingCost;
			this._IdealUserCount = IdealUserCount;

           
        }
     
     }

  }
