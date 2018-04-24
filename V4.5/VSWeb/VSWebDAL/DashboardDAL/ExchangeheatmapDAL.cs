using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
    public class ExchangeheatmapDAL
    {
        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
        private Adaptor adaptor = new Adaptor();
        private static ExchangeheatmapDAL _self = new ExchangeheatmapDAL();

        public static ExchangeheatmapDAL Ins
        {
            get
            {
                return _self;
            }
        }

		public DataTable SetGraph(string sourceserver, string DestinationServer)
        {
            DataTable dt = new DataTable();
            try
                {
					dt = objAdaptor.Fetchexchangedailyststs( sourceserver, System.DateTime.Now, DestinationServer);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
           


            return dt;
        }

     
     
    }
}
