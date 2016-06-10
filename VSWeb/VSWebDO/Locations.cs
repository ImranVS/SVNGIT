using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
    public class Locations
    {
          /// <summary>
	/// Default Contructor
	/// <summary>
        public Locations()
	{}

        public int ID
        { 
            get { return _ID; }
            set { _ID = value; }  
        }
        private int _ID;
        public string Location
        {
            get { return _Location; }
            set { _Location = value; }
        }
        private string _Location;

		public string City
		{
			get { return _City; }
			set { _City = value; }
		}
		private string _City;

		public string State
		{
			get { return _State; }
			set { _State = value; }
		}
		private string _State;

		public string Country
		{
			get { return _Country; }
			set { _Country = value; }
		}
		private string _Country;
        /// <summary>
        /// User defined Contructor
        /// <summary>
        public Locations(
        int ID,
        string Location)
            {
        this._ID = ID;
        this._Location = Location;
        }
           
        
    }
}
