using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
	public class ExchangeheatmapBL
	{
		private static ExchangeheatmapBL _self = new ExchangeheatmapBL();

		public static ExchangeheatmapBL Ins
		{
			get
			{
				return _self;
			}
		}

		public DataTable SetGraph(string sourceserver, string DestinationServer)
		{
			return VSWebDAL.DashboardDAL.ExchangeheatmapDAL.Ins.SetGraph(sourceserver, DestinationServer);
		}
	
	
	}
}
