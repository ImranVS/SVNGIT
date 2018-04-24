using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DO
{
    public class LicenseCompanys
    {
		public LicenseCompanys()
        {}

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _ID;


		public string CompanyName
        {
			get { return _CompanyName; }
			set { _CompanyName = value; }
        }

		private string _CompanyName;
    }
}
