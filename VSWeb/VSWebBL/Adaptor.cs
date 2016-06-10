using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace VSWebBL
{
    public class Adaptor
    {
        private static Adaptor _self = new Adaptor();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static Adaptor Ins
        {
            get { return _self; }
        }

        //5/20/2015 NS added for VSPLUS-1753
        public string TestConnection()
        {
            VSWebDAL.Adaptor objAdaptor = new VSWebDAL.Adaptor();
            try
            {
                return objAdaptor.TestConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
