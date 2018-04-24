using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CustomerTrackingDL;
using CustomerTrackingDO;

namespace CustomerServiceBL
{
    public class CompanyBL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private static CompanyBL _self = new CompanyBL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static CompanyBL Ins
        {
            get { return _self; }
        }
        public Object UpdateLogo(Company cobj)
        {


            return CustomerTrackingDL.CompanyDAL.Ins.UpdateLogo(cobj);

        }

        public Company GetLogo()
        {


            return CustomerTrackingDL.CompanyDAL.Ins.GetLogo();

        }
    }
}
