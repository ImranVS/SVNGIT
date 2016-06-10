using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTrackingDO
{
    public class Company
    {
        public Company()
        { }

        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }

        }
        private string _CompanyName;


        public string LogoPath
        {
            get { return _LogoPath; }
            set { _LogoPath = value; }


        }
        private string _LogoPath;



        public Company(string CompanyName,
            string LogoPath)
        {
            this._CompanyName = CompanyName;
            this._LogoPath = LogoPath;
        }
    }
}
