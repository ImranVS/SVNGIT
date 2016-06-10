using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDAL.ConfiguratorDAL
{
    public class DiskSettingsDAL
    {
        private Adaptor adaptor = new Adaptor();
        private static DiskSettingsDAL _self = new DiskSettingsDAL();

        public static DiskSettingsDAL Ins
        {
            get
            {
                return _self;
            }
        }

       
    }
}
