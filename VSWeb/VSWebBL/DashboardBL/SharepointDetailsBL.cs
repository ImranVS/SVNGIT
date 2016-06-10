using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
	public  class SharepointDetailsBL
	{

		private static SharepointDetailsBL _self = new SharepointDetailsBL();

		public static SharepointDetailsBL Ins
		{
			get
			{
				return _self;
			}
		}
		public DataTable SetGraphForCPUsharepoint(string paramGraph, string serverName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SharepointdetailsDAL.Ins.SetGraphForCPUsharepoint(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable SetGraphForMemorysharepoint(string paramGraph, string serverName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SharepointdetailsDAL.Ins.SetGraphForMemorysharepoint(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable SetGraphForsharepointEnabledUsers(string paramGraph, string serverName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SharepointdetailsDAL.Ins.SetGraphForsharepointEnabledUsers(paramGraph, serverName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}


		public DataTable GetSharePointServerHealthRoles()
		{
			try
			{
				return VSWebDAL.DashboardDAL.SharepointdetailsDAL.Ins.GetSharePointServerHealthRoles();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetSharePointServerDetails()
		{
			try
			{
				return VSWebDAL.DashboardDAL.SharepointdetailsDAL.Ins.GetSharePointServerDetails();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable SetGraphforperformance(string paramGraph, string DeviceName, int ServerTypeId)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SharepointdetailsDAL.Ins.SetGraphforperformance(paramGraph, DeviceName, ServerTypeId);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable ResponseThresholdForsharepoint(string ServerName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SharepointdetailsDAL.Ins.ResponseThresholdForsharepoint(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable SetGridForsharepointDisk(string ServerName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SharepointdetailsDAL.Ins.SetGridForsharepointDisk(ServerName);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable sharepointtab(string servername)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SharepointdetailsDAL.Ins.sharepointtab(servername);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetDatabaseServerDetails()
		{
			try
			{
				return VSWebDAL.DashboardDAL.SharepointdetailsDAL.Ins.GetDatabaseServerDetails();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetFarmDetails()
		{
			try
			{
				return VSWebDAL.DashboardDAL.SharepointdetailsDAL.Ins.GetFarmDetails();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetFarmNames()
		{
			try
			{
				return VSWebDAL.DashboardDAL.SharepointdetailsDAL.Ins.GetFarmNames();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetSiteCollectionSize(string Farm)
		{
			try
			{
				return VSWebDAL.DashboardDAL.SharepointdetailsDAL.Ins.GetSiteCollectionSize(Farm);
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
        //18-04-2016 Durga Modified for VSPLUS-2851
        public DataTable GetSharePointTimerJobsDetails()
        {
            try
            {
                return VSWebDAL.DashboardDAL.SharepointdetailsDAL.Ins.GetSharePointTimerJobsDetails();
            }
            catch (Exception ex)
            {

                throw ex;
            }

}
	}
}

