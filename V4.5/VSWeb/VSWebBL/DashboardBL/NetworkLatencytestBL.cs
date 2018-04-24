using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDAL;
namespace VSWebBL.DashboardBL
{
	public class NetworkLatencytestBL
	{
		private static NetworkLatencytestBL _self = new NetworkLatencytestBL();

		public static NetworkLatencytestBL Ins
		{
			get { return _self; }
		}

		public DataTable GetNetworkLatencyHeatmap()
		{
			try
			{
				return VSWebDAL.DashboardDAL.NetworkLatencytestDAL.Ins.GetNetworkLatencyHeatmap();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable SetGraphForLatency(DateTime frmdate, string statname, string DeviceName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.NetworkLatencytestDAL.Ins.SetGraphForLatency(frmdate, statname, DeviceName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
	}
}
