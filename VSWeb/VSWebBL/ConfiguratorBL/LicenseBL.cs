using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDAL;
using VSWebDO;

namespace VSWebBL.ConfiguratorBL
{
	public class LicenseBL
    {
		private static LicenseBL _self = new LicenseBL();
		public static LicenseBL Ins
        {
            get { return _self; }
        }

		public bool InsertLicense(License keyvalue)
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.LicenseDAL.Ins.InsertLicense(keyvalue);
					
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable GetLicensedetails()

		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.LicenseDAL.Ins.GetLicensedetails();

			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable Getlicenseusage()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.LicenseDAL.Ins.Getlicenseusage();

			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable Getlicenseunitsinfo()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.LicenseDAL.Ins.Getlicenseunitsinfo();

			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public DataTable Gettotalunitsused()
		{
			try
			{
				return VSWebDAL.ConfiguratorDAL.LicenseDAL.Ins.Gettotalunitsused();

			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		
    }
}
