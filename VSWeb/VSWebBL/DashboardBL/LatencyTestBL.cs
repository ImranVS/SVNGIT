using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;
namespace VSWebBL.DashboardBL
{
	public class LatencyTestBL
	{
		private static LatencyTestBL _self = new LatencyTestBL();
		public static LatencyTestBL Ins
		{
			get { return _self; }
		}

		public DataTable getNetworkLatencyHeatMap(string NetworkLatencyID)
		{
			try
			{
				return VSWebDAL.DashboardDAL.LatencyTestDAL.Ins.getNetworkLatencyHeatMap(NetworkLatencyID);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable getNetworkLatencyTestNames()
		{
			try
			{
				return VSWebDAL.DashboardDAL.LatencyTestDAL.Ins.getNetworkLatencyTestNames();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}


	}
}
