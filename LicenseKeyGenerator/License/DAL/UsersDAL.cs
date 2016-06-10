using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DO;
using System.Data;


namespace DAL
{
	public class UsersDAL
	{
		/// <summary>
		/// Declarations
		/// </summary>
		private Adaptor objAdaptor = new Adaptor();
		private static UsersDAL _self = new UsersDAL();

		/// <summary>
		/// Used to call the functions using class name instead of object
		/// </summary>
		public static UsersDAL Ins
		{
			get { return _self; }
		}

		public Users GetData(Users UsersObject)
		{
			Users ReturnUsersObject = new Users();
			DataTable UsersDataTable = new DataTable();
			string SqlQuery = "select * from Users where LoginName='" + UsersObject.LoginName + "'";
			UsersDataTable = objAdaptor.FetchData(SqlQuery);
			if (UsersDataTable.Rows.Count > 0)
			{
				
				ReturnUsersObject.Status = UsersDataTable.Rows[0]["Status"].ToString();
				ReturnUsersObject.LoginName = UsersDataTable.Rows[0]["LoginName"].ToString();
				ReturnUsersObject.Password = UsersDataTable.Rows[0]["Password"].ToString();
				ReturnUsersObject.FullName = UsersDataTable.Rows[0]["FullName"].ToString();
				
				ReturnUsersObject.ID = (int)UsersDataTable.Rows[0]["ID"];
				//5/17/2012 NS added the rest of the return values from the query to be used on the Update My Info tab
				ReturnUsersObject.Email = UsersDataTable.Rows[0]["Email"].ToString();
				ReturnUsersObject.SecurityQuestion1 = UsersDataTable.Rows[0]["SecurityQuestion1"].ToString();
				ReturnUsersObject.SecurityQuestion1Answer = UsersDataTable.Rows[0]["SecurityQuestion1Answer"].ToString();
				ReturnUsersObject.SecurityQuestion2 = UsersDataTable.Rows[0]["SecurityQuestion2"].ToString();
				ReturnUsersObject.SecurityQuestion2Answer = UsersDataTable.Rows[0]["SecurityQuestion2Answer"].ToString();
				

				//01/23/2014 MD modified
				
			}
			return ReturnUsersObject;
		}


		//5/17/2012 NS added new function to verify user account info
		public bool VerifyAccount(ref Users UsersObject)
		{
			bool success = false;
			DataTable UsersDataTable = new DataTable();
			string SqlQuery = "select * from Users where LoginName='" + UsersObject.LoginName + "' " +
				"AND Email='" + UsersObject.Email + "'";
			UsersDataTable = objAdaptor.FetchData(SqlQuery);
			if (UsersDataTable.Rows.Count > 0)
			{
				success = true;
			}
			return success;
		}
		public DataTable GetAllData()
		{

			DataTable UsersDataTable = new DataTable();
			Users ReturnUserobject = new Users();
			try
			{
				string SqlQuery = "SELECT [ID],[LoginName], [Password], [FullName], [Email], [Status], [UserType] FROM [Users] order by LoginName";
				//string SqlQuery = "SELECT * FROM Servers";
				UsersDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return UsersDataTable;
		}
		/// <summary>
		/// Insert data into Locations table
		/// </summary>
		/// <param name="DSObject">Locations object</param>
		/// <returns></returns>

		public bool InsertData(Users UsersObject)
		{
			bool Insert = false;
			try
			{
				string SqlQuery = "INSERT INTO [Users] (LoginName,Password,FullName,Email,Status,UserType)" +
				"VALUES('" + UsersObject.LoginName + "','" + UsersObject.Password + "','" + UsersObject.FullName +
				"','" + UsersObject.Email + "','" + UsersObject.Status + "','" + UsersObject.UserType + "')";


				Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Insert = false;
			}
			finally
			{
			}
			return Insert;
		}

		/// <summary>
		/// Update data into Users table
		/// </summary>
		/// <param name="UsersObject">Locations object</param>
		/// <returns></returns>
		public Object UpdateData(Users UsersObject)
		{
			Object Update;
			try
			{
				string SqlQuery = "UPDATE [Users] SET [LoginName]='" + UsersObject.LoginName + "', [Password]='" + UsersObject.Password +
					"', [FullName]='" + UsersObject.FullName + "', [Email]='" + UsersObject.Email + "', [Status]='" + UsersObject.Status +
				"', [UserType]='" + UsersObject.UserType + "'  WHERE ID=" + UsersObject.ID + "";
				Update = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Update = false;
			}
			finally
			{
			}
			return Update;
		}
		//delete Data from Users Table

		public Object DeleteData(Users UsersObject)
		{
			Object Update;
			try
			{

				string SqlQuery = "DELETE FROM [Users] WHERE [ID]=" + UsersObject.ID + "";

				Update = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Update = false;
			}
			finally
			{
			}
			return Update;
		}

		public bool UpdateAccount(Users UserAccObject)
		{
			bool Update = false;
			try
			{
				//VSPLUS-613, Mukund 14May14, removed login name as not required in update
				//[LoginName]='" + UserAccObject.LoginName +                   "', 

				string SqlQuery = "UPDATE [Users] SET [FullName]='" + UserAccObject.FullName + "', [Email]='" + UserAccObject.Email + "',[SecurityQuestion1]='" + UserAccObject.SecurityQuestion1 +
			   "',[SecurityQuestion1Answer]='" + UserAccObject.SecurityQuestion1Answer + "',[SecurityQuestion2]='" + UserAccObject.SecurityQuestion2 +
                "',[SecurityQuestion2Answer]=  '" + UserAccObject.SecurityQuestion2Answer + "' WHERE ID=" + UserAccObject.ID + " ";



				Update = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Update = false;
			}
			finally
			{
			}
			return Update;
		}

		public bool CreateAccount(Users UserAccObject)
		{
			bool Update = false;
			try
			{
				// string SqlQuery = "insert into [Users] values([LoginName],[FullName],[Email],[SecurityQuestion1],[SecurityQuestion1Answer],[SecurityQuestion2],[SecurityQuestion2Answer]) ('" + UserAccObject.LoginName +
				//    "','" + UserAccObject.FullName + "','" + UserAccObject.Email + "','" + UserAccObject.SecurityQuestion1 +
				//"','" + UserAccObject.SecurityQuestion1Answer + "','" + UserAccObject.SecurityQuestion2.Replace("'", "''") +
				// "','" + UserAccObject.SecurityQuestion2Answer + "')";
				//2/7/2013 NS modified 
				string SqlQuery = "insert into [Users]([LoginName],[FullName],[password],[Email],Status,UserType) values('" + UserAccObject.LoginName +
				   "','" + UserAccObject.FullName + "','" + UserAccObject.Password + "','" + UserAccObject.Email + "','" + UserAccObject.Status +
			   "','" + UserAccObject.UserType + "')";


				Update = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Update = false;
			}
			finally
			{
			}
			return Update;
		}
		public bool UpdateAccntPassword(Users UserPasswordObject)
		{

			bool Update = false;
			try
			{
				string SqlQuery = "UPDATE [Users] SET Password='" + UserPasswordObject.Password + "'  WHERE ID=" + UserPasswordObject.ID + "";



				Update = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Update = false;
			}
			finally
			{
			}
			return Update;

		}

		public DataTable GetDataByLoginNmae(Users UsersObject)
		{

			DataTable UsersDataTable = new DataTable();
			//Users ReturnUserobject = new Users();
			try
			{
				if (UsersObject.ID == 0)
				{
					string SqlQuery = "SELECT * FROM [Users] where LoginName='" + UsersObject.LoginName + "'";
					//string SqlQuery = "SELECT * FROM Servers";
					UsersDataTable = objAdaptor.FetchData(SqlQuery);
				}
				else
				{
					string SqlQuery = "SELECT * FROM [Users] where LoginName='" + UsersObject.LoginName + "' and ID<>'" + UsersObject.ID + "'";
					UsersDataTable = objAdaptor.FetchData(SqlQuery);
				}
			}
			catch
			{
			}
			finally
			{
			}
			return UsersDataTable;
		}

		public DataTable Getdatabyid(Users UsersObject)
		{
			DataTable dt = new DataTable();
			Users ReturnUserobject = new Users();
			try
			{
				string q = "Select * from [Users] where ID=" + UsersObject.ID + "";
				dt = objAdaptor.FetchData(q);
			}
			catch
			{
			}
			finally
			{

			}
			return dt;
		}

		public DataTable GetIsAdmin(string userid)
		{
			DataTable dt = new DataTable();
			try
			{
				string q = "Select UserType  from [Users] where ID=" + userid + "";
				dt = objAdaptor.FetchData(q);
			}
			catch
			{
			}
			finally
			{

			}
			return dt;
		}
		public DataTable GetIsAdmin1(string userid)
		{
			DataTable dt = new DataTable();
			try
			{
				string q = "Select *  from [Users] where ID=" + userid + "";
				dt = objAdaptor.FetchData(q);
			}
			catch
			{
			}
			finally
			{

			}
			return dt;
		}


        //2/27/2015 NS added for VSPLUS-1414
        public DataTable GetUserSysMessages(string userid)
        {
            DataTable dt = new DataTable();
            try
            {
                string q = "SELECT TOP 1 t2.SysMsgID,Details FROM SystemMessages t1 " +
                    "INNER JOIN UserSystemMessages t2 ON t1.ID=t2.SysMsgID " + 
                    "WHERE t2.UserID=" + userid + " AND DateDismissed IS NULL " +
                    "ORDER BY DateDisplayed";
                dt = objAdaptor.FetchData(q);
            }
            catch
            {
            }
            finally
            {

            }
            return dt;
        }

        //2/27/2015 NS added for VSPLUS-1414
        public bool UpdateUserSysMessageDate(string userid,string msgid, string dateupd)
        {
            bool Update = false;
            try
            {
                string SqlQuery = "UPDATE UserSystemMessages SET " + dateupd + "='" + DateTime.Now.ToString() + "' " + 
                    "WHERE UserID=" + userid + " AND SysMsgID=" + msgid;
                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }

		public DataTable Getusers()
		{
			DataTable UsersDataTable = new DataTable();

			try
			{
				string SqlQuery = "SELECT * from Users order by LoginName";
				UsersDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return UsersDataTable;
		}
		public DataTable GetCompanyNames()
		{
			DataTable CompanysDataTable = new DataTable();

			try
			{
				string SqlQuery = "SELECT * from LicenseCompanys order by CompanyName";
				CompanysDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return CompanysDataTable;
		}

		public DataTable GetCompanyID(string name)
		{
			DataTable CompanysDataTable = new DataTable();

			try
			{
				string SqlQuery = "SELECT ID from LicenseCompanys where CompanyName='"+name+"' order by CompanyName";
				CompanysDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return CompanysDataTable;
		}
		public bool InsertCompanyName(LicenseCompanys UsersObject)
		{
			bool Insert = false;
			try
			{
				string SqlQuery = "INSERT INTO [LicenseCompanys] (CompanyName)" +
				"VALUES('" + UsersObject.CompanyName + "')";
				Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Insert = false;
			}
			finally
			{
			}
			return Insert;
		}
		public Object UpdateCompanyData(LicenseCompanys UsersObject)
		{
			Object Update;
			try
			{
				string SqlQuery = "UPDATE [LicenseCompanys] SET [CompanyName]='" + UsersObject.CompanyName + "'  WHERE ID=" + UsersObject.ID + "";
				Update = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Update = false;
			}
			finally
			{
			}
			return Update;
		}
		public DataTable GetDataByCompanyName(LicenseCompanys UsersObject)
		{

			DataTable UsersDataTable = new DataTable();
			//Users ReturnUserobject = new Users();
			try
			{
				if (UsersObject.ID == 0)
				{
					string SqlQuery = "SELECT * FROM [LicenseCompanys] where CompanyName='" + UsersObject.CompanyName + "'";
					//string SqlQuery = "SELECT * FROM Servers";
					UsersDataTable = objAdaptor.FetchData(SqlQuery);
				}
				else
				{
					string SqlQuery = "SELECT * FROM [LicenseCompanys] where CompanyName='" + UsersObject.CompanyName + "' and ID<>'" + UsersObject.ID + "'";
					UsersDataTable = objAdaptor.FetchData(SqlQuery);
				}
			}
			catch
			{
			}
			finally
			{
			}
			return UsersDataTable;
		}
		public Object DeleteCompanyData(Users UsersObject)
		{
			Object Update;
			try
			{

				string SqlQuery = "DELETE FROM [LicenseCompanys] WHERE [ID]=" + UsersObject.ID + "";

				Update = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				Update = false;
			}
			finally
			{
			}
			return Update;
		}
		public DataTable GetCompanyData()
		{

			DataTable UsersDataTable = new DataTable();
			Users ReturnUserobject = new Users();
			try
			{
				string SqlQuery = "SELECT [ID],[CompanyName] FROM [LicenseCompanys] order by CompanyName";
				//string SqlQuery = "SELECT * FROM Servers";
				UsersDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return UsersDataTable;
		}

		public LicenseKey GetData(LicenseKey Licensekeyobject, int id)
		{
			
		
			DataTable LicenseKeyDataTable = new DataTable();
			LicenseKey ReturnObject = new LicenseKey();
			try
			{
				//string SqlQuery = "select li.*,lc.CompanyName from  License li inner join LicenseCompanys  lc on li.CompanyID=lc.ID  where li.CreateBy='" +id+ "' and li.CreatedOn in (select max (Createdon) from License group by companyid)  and li.ID= '"+ Licensekeyobject.ID.ToString()+"' order by CompanyName";

				string SqlQuery = "select li.*,lc.CompanyName from  License li inner join LicenseCompanys  lc on li.CompanyID=lc.ID  where  li.ID=  '" + Licensekeyobject.ID.ToString() + "'";
				LicenseKeyDataTable = objAdaptor.FetchData(SqlQuery);
				//populate & return data object
				ReturnObject.InstallType = LicenseKeyDataTable.Rows[0]["InstallType"].ToString();
				ReturnObject.Key = LicenseKeyDataTable.Rows[0]["LicenseKey"].ToString();
				ReturnObject.LicenseType = LicenseKeyDataTable.Rows[0]["LicenseType"].ToString();
				ReturnObject.CompanyName = LicenseKeyDataTable.Rows[0]["CompanyName"].ToString();
                ReturnObject.EncUnits = LicenseKeyDataTable.Rows[0]["EncUnits"].ToString();
				ReturnObject.Units = int.Parse(LicenseKeyDataTable.Rows[0]["Units"].ToString());
				ReturnObject.CreateBy = int.Parse(LicenseKeyDataTable.Rows[0]["CreateBy"].ToString());
				ReturnObject.CreatedOn =  Convert.ToDateTime(LicenseKeyDataTable.Rows[0]["CreatedOn"].ToString());
				ReturnObject.ID = int.Parse(LicenseKeyDataTable.Rows[0]["ID"].ToString());
				ReturnObject.CompanyID = int.Parse(LicenseKeyDataTable.Rows[0]["CompanyID"].ToString());
				ReturnObject.ExpirationDate = (LicenseKeyDataTable.Rows[0]["ExpirationDate"].ToString());
				

			}
			catch
			{

			}
			finally
			{
			}
			return ReturnObject;

		}


	}
}
