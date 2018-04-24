using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;
namespace VSWebBL.DashboardBL
{
	public class ExchangeHeatBL
	{
		private static ExchangeHeatBL _self = new ExchangeHeatBL();
		public static ExchangeHeatBL Ins
		{
			get { return _self; }
		}

		public DataTable getexchangeheatmap()
		{
			try
			{
				return VSWebDAL.DashboardDAL.ExchangeHeatDAL.Ins.getexchangeheatmap();
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
				return VSWebDAL.DashboardDAL.ExchangeHeatDAL.Ins.SetGraphForLatency(frmdate, statname, DeviceName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
	}
}
