using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebBL.DashboardBL
{
	public class MapBL
	{

		/// <summary>
		/// Declarations
		/// </summary>
		private static MapBL _self = new MapBL();

		/// <summary>
		/// Used to call the functions using class name instead of object
		/// </summary>
		public static MapBL Ins
		{
			get { return _self; }
		}

		public DataTable GetCityLocationInfo()
		{
			try
			{
				return VSWebDAL.DashboardDAL.MapDAL.Ins.GetCityLocationInfo();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetCountryLocationInfo()
		{
			try
			{
				return VSWebDAL.DashboardDAL.MapDAL.Ins.GetCountryLocationInfo();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}
		public DataTable GetStateLocationInfo()
		{
			try
			{
				return VSWebDAL.DashboardDAL.MapDAL.Ins.GetStateLocationInfo();
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
		}

		public DataTable GetLocationInfoO365(String statName)
		{
			try
			{
				return VSWebDAL.DashboardDAL.MapDAL.Ins.GetLocationInfoO365(statName);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetUniqueOffice365Tests()
		{
			try
			{
				return VSWebDAL.DashboardDAL.MapDAL.Ins.GetUniqueOffice365Tests();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetThresholds()
		{
			try
			{
				return VSWebDAL.DashboardDAL.MapDAL.Ins.GetThresholds();
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
	}
}