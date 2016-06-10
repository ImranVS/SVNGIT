using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DO;
using System.Data;
using System.Configuration;

namespace DAL
{
	public class LicenseKeyDAL
	{
		private Adaptor objAdaptor = new Adaptor();
		
		private static LicenseKeyDAL _self = new LicenseKeyDAL();

		/// <summary>
		/// Used to call the functions using class name instead of object
		/// </summary>
		public static LicenseKeyDAL Ins
		{
			get { return _self; }
		}
		public bool insert(string encrypt, int Units, string InstallType, string LicenseType, string ExpirationDate, string CreatedBy, string CreatedOn, int CompanyID)
		{
			bool insert = false;
			try
			{

				string sqlquery = "INSERT INTO License ([LicenseKey],[Units],[InstallType],[LicenseType],[ExpirationDate],[CreateBy],[CreatedOn],[CompanyID]) VALUES('" + encrypt + "'," + Units + ",'" + InstallType + "','" + LicenseType + "','" + ExpirationDate + "','" + CreatedBy + "','" + CreatedOn + "'," + CompanyID + ")";
				insert = objAdaptor.ExecuteNonQuery(sqlquery);
			}
			catch{
				insert = false;
			}
			return true;

		}
		public DataTable GetLicenceInformation(int ID)
		{
			DataTable LicenceDataTable = new DataTable();

			try
			{
				//string SqlQuery = "select ID, LicenseKey,Units,InstallType,CompanyName,LicenseType,ExpirationDate,CreatedOn from  License Where CreateBy=" + ID;
				string SqlQuery = "select li.*,lc.CompanyName from  License li inner join LicenseCompanys  lc on li.CompanyID=lc.ID  where li.CreateBy='" + ID + "' order by CompanyName ";

				LicenceDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return LicenceDataTable;
		}
		public DataTable GetLicenceReportInformations(string param, string qtype)
		{
			DataTable dt = new DataTable();
			try
			{
				

				string sqlQuery = "select li.LicenseKey,li.Units,li.InstallType,li.LicenseType,li.ExpirationDate,u.LoginName,li.CreatedOn,lc.CompanyName from  License li inner join LicenseCompanys  lc on li.CompanyID=lc.ID inner join Users u on li.CreateBy=u.ID ";

				if (param != "")
				{
					if (qtype == "LoginName")
					{
						sqlQuery += " WHERE u.[" + qtype + "] = '" + param + "'";
					}
					else
					{
						sqlQuery += " WHERE li.[" + qtype + "] = '" + param + "'";
					}
				}
				dt = objAdaptor.FetchData(sqlQuery);
			}
			catch (Exception e)
			{
				throw e;
			}
			return dt;

		}
		public DataTable GetLicenceReportInformations()
		{
			DataTable dt = new DataTable();
			try
			{
				

				string sqlQuery = "select li.LicenseKey,li.Units,li.InstallType,li.LicenseType,li.ExpirationDate,u.LoginName,li.CreatedOn,lc.CompanyName from  License li inner join LicenseCompanys  lc on li.CompanyID=lc.ID inner join Users u on li.CreateBy=u.ID ";

				
				dt = objAdaptor.FetchData(sqlQuery);
			}
			catch (Exception e)
			{
				throw e;
			}
			return dt;

		}


		public DataTable GetLicenceReportInformation(string ID)
		{
			DataTable LicenceDataTable = new DataTable();

			try
			{
				//string SqlQuery = "select ID, LicenseKey,Units,InstallType,CompanyName,LicenseType,ExpirationDate,CreatedOn from  License Where CreateBy=" + ID;
				
			//string sqlQuery = "select li.*,lc.CompanyName from  License li inner join LicenseCompanys  lc on li.CompanyID=lc.ID where ExpirationDate  BETWEEN CONVERT(date, getdate())  and (select dateadd(m, 1, getdate()))";
				string sqlQuery = "select li.* ,lc.CompanyName from  License li inner join LicenseCompanys  lc on li.CompanyID=lc.ID where ExpirationDate  BETWEEN CONVERT(date, getdate())  and (select dateadd(m, 1, getdate())) and "+
                                   "li.CreatedOn in (select max (Createdon) from License group by companyid)  order by ExpirationDate Asc ";
    
     


				LicenceDataTable = objAdaptor.FetchData(sqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return LicenceDataTable;
		}
		public DataTable GetAllData(string ID)
		{

			DataTable LicenseDataTable = new DataTable();
			LicenseKey ReturnObject = new LicenseKey();
			try
			{
				//string SqlQuery = "Select * from License a where a.CreateBy = '" + ID + "'   and a.CreatedOn  = (select max (Createdon) from License b where b.CreateBy = '" + ID + "' and  a.companyName = b.companyName)";
				//string SqlQuery = "select li.*,lc.CompanyName from  License li inner join LicenseCompanys  lc on li.CompanyID=lc.ID  where li.CreateBy='" + ID + "' and li.CreatedOn in (select max (Createdon) from License group by companyid) order by CompanyName";
             //string SqlQuery = "select li.*,lc.CompanyName,u.LoginName from  License li inner join Users u on li.CreateBy=u.ID inner join LicenseCompanys  lc on li.CompanyID=lc.ID   where li.CreateBy='" + ID + "' and li.CreatedOn in (select max (Createdon) from License group by companyid) order by CompanyName";
				string SqlQuery = "select li.*,lc.CompanyName,u.LoginName from  License li inner join Users u on li.CreateBy=u.ID inner join LicenseCompanys  lc on li.CompanyID=lc.ID   where  li.CreatedOn in (select Createdon from License) order by CompanyName, Createdon desc";



				LicenseDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return LicenseDataTable;
		}

		public DataTable GetDateRangeData(string fromdate, string todate)
		{

			DataTable LicenseDataTable = new DataTable();
            LicenseKey ReturnObject = new LicenseKey();
			System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
			DateTime dtStart = DateTime.Parse(fromdate);
			DateTime dtEnd = DateTime.Parse(todate);

			fromdate = dtStart.ToString(ci);
			todate = dtEnd.ToString(ci);
			try
			{
				
				string SqlQuery = "select li.*,lc.CompanyName,u.LoginName from  License li inner join Users u on li.CreateBy=u.ID inner join LicenseCompanys  lc on li.CompanyID=lc.ID"+
				" where convert(datetime,ExpirationDate,101) > '" + fromdate + "' AND  convert(datetime,ExpirationDate,101) <='" + todate + "'Order By ExpirationDate ASC"; 
                           
              LicenseDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return LicenseDataTable;
		}

		public DataTable GetThirtydaysExpiry(string ID)
		{

			DataTable LicenseDataTable = new DataTable();
			LicenseKey ReturnObject = new LicenseKey();
			try
			{


				string sqlQuery = "select li.*,lc.CompanyName,u.LoginName from  License li inner join Users u on li.CreateBy=u.ID inner join LicenseCompanys  lc on li.CompanyID=lc.ID where ExpirationDate  BETWEEN CONVERT(date, getdate())  and (select dateadd(m, 1, getdate())) and " +
								   "li.CreatedOn in (select max (Createdon) from License group by companyid)  order by ExpirationDate Asc ";


				LicenseDataTable = objAdaptor.FetchData(sqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return LicenseDataTable;
		}

		public DataTable GetExpirysData(int ExpiryValue)
		{

			DataTable LicenseDataTable = new DataTable();
			LicenseKey ReturnObject = new LicenseKey();
			try
			{

				string SqlQuery = "select DATEDIFF(DAY, GETDATE(),ExpirationDate),GETDATE(),ExpirationDate, li.*,lc.CompanyName,u.LoginName from  License li inner join Users u on li.CreateBy=u.ID inner join LicenseCompanys  lc on li.CompanyID=lc.ID  where  DATEDIFF(DAY, GETDATE(),ExpirationDate) <= '" + ExpiryValue + "' and ExpirationDate>=GETDATE()";
				
               LicenseDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return LicenseDataTable;
		}
		public DataTable GetEstimateLicensesData()
		{

			DataTable EstimateLicensesDataTable = new DataTable();
			LicenseKey ReturnObject = new LicenseKey();
			try
			{

			//	string SqlQuery = "select * from ServerTypes";

				string SqlQuery = "select ServerType,UnitCost,'' as noofservers,'' as totalunits from ServerTypes";
				EstimateLicensesDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return EstimateLicensesDataTable;
		}
	}
}