using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using DO;
using System.Data;

namespace BL
{
	public class LicenseKeyBL
	{
		/// <summary>
		/// Declarations
		/// </summary>
		private static LicenseKeyBL _self = new LicenseKeyBL();

		/// <summary>
		/// Used to call the functions using class name instead of object
		/// </summary>
		public static LicenseKeyBL Ins
		{
			get { return _self; }
		}

		public bool insert(string encrypt, int Units, string InstallType, string LicenseType, string ExpirationDate, string CreatedBy, string CreatedOn, int CompanyID)
		{
			try
			{
				return DAL.LicenseKeyDAL.Ins.insert(encrypt, Units, InstallType, LicenseType, ExpirationDate, CreatedBy, CreatedOn, CompanyID);

			}
			catch (Exception)
			{
				
				throw;
			}
		}

		public DataTable GetLicenceInformation(int ID)
		{
			try
			{
				return DAL.LicenseKeyDAL.Ins.GetLicenceInformation(ID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetLicenceReportInformations(string param, string qtype)
		{
			try
			{
				return DAL.LicenseKeyDAL.Ins.GetLicenceReportInformations(param, qtype);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetLicenceReportInformations()
		{
			try
			{
				return DAL.LicenseKeyDAL.Ins.GetLicenceReportInformations();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetLicenceReportInformation(string ID)
		{
			try
			{
				return DAL.LicenseKeyDAL.Ins.GetLicenceReportInformation(ID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetAllData(string ID)
		{
			try
			{
				return DAL.LicenseKeyDAL.Ins.GetAllData(ID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetDateRangeData(string fromdate, string todate)
		{
			try
			{
				return DAL.LicenseKeyDAL.Ins.GetDateRangeData(fromdate, todate);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetThirtydaysExpiry(string ID)
		{
			try
			{
				return DAL.LicenseKeyDAL.Ins.GetThirtydaysExpiry(ID);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public DataTable GetExpirysData(int ExpiryValue)
		{
			try
			{
				return DAL.LicenseKeyDAL.Ins.GetExpirysData(ExpiryValue);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetEstimateLicensesData()
		{
			try
			{
				return DAL.LicenseKeyDAL.Ins.GetEstimateLicensesData();
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
	}
}