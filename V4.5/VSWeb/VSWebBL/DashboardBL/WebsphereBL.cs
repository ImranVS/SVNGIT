using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;
namespace VSWebBL.DashboardBL
{
	public class WebsphereBL
	{
		private static WebsphereBL _self = new WebsphereBL();
		public static WebsphereBL Ins
		{
			get { return _self; }
		}

		public DataTable GetWebsphereCellStatus()

		{
			try
			{
				return VSWebDAL.DashboardDAL.WebsphereDAL.Ins.GetWebsphereCellStatus();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetWebsphereNodeStatus(int value)

		{
			try
			{
				return VSWebDAL.DashboardDAL.WebsphereDAL.Ins.GetWebsphereNodeStatus(value);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetWebsphereseversStatus(int cellvalue)
		{
			try
			{
				return VSWebDAL.DashboardDAL.WebsphereDAL.Ins.GetWebsphereseversStatus(cellvalue);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
        public DataTable GetWebsphereStatusAll()
        {
			try
			{
				return VSWebDAL.DashboardDAL.WebsphereDAL.Ins.GetWebsphereStatusAll();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
            
        }
	}
}
