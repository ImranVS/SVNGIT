using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using System.Data;


namespace VSWebDAL.SecurityDAL
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
				try
				{
					ReturnUsersObject.CloudApplications = Convert.ToBoolean(UsersDataTable.Rows[0]["CloudApplications"]);
				}
				catch
				{

					ReturnUsersObject.CloudApplications = true;
				}
				try
				{
					ReturnUsersObject.OnPremisesApplications = Convert.ToBoolean(UsersDataTable.Rows[0]["OnPremisesApplications"]);
				}
				catch
				{

					ReturnUsersObject.OnPremisesApplications = true;
				}
				try
				{
					ReturnUsersObject.NetworkInfrastucture = Convert.ToBoolean(UsersDataTable.Rows[0]["NetworkInfrastucture"]);
				}
				catch
				{

					ReturnUsersObject.NetworkInfrastucture = true;
				}
				try
				{

					ReturnUsersObject.DominoServerMetrics = Convert.ToBoolean(UsersDataTable.Rows[0]["DominoServerMetrics"]);
				}
				catch
				{

					ReturnUsersObject.DominoServerMetrics = true;
				}
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
				ReturnUsersObject.Refreshtime = Convert.ToInt32(UsersDataTable.Rows[0]["Refreshtime"].ToString());
				ReturnUsersObject.StartupURL = UsersDataTable.Rows[0]["StartupURL"].ToString();
				ReturnUsersObject.CustomBackground = UsersDataTable.Rows[0]["CustomBackground"].ToString();

				//01/23/2014 MD modified
				try
				{
					ReturnUsersObject.IsConfigurator = Convert.ToBoolean(UsersDataTable.Rows[0]["IsConfigurator"].ToString());
				}
				catch
				{

					ReturnUsersObject.IsConfigurator = false;
				}
				try
				{
					ReturnUsersObject.Isdashboard = Convert.ToBoolean(UsersDataTable.Rows[0]["IsDashboard"].ToString());
				}
				catch
				{

					ReturnUsersObject.Isdashboard = false;
				}


				try
				{
					ReturnUsersObject.Isconsolecomm = Convert.ToBoolean(UsersDataTable.Rows[0]["Isconsolecomm"].ToString());
				}
				catch
				{

					ReturnUsersObject.Isconsolecomm = false;
				}

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
		public bool GetIsFirstTimeLogin(int id)
		{
			//bool success = false;
			bool success = false; ;
			DataTable UsersDataTable = new DataTable();
			string SqlQuery = "select IsFirstTimeLogin from Users where ID=" + id + " ";

			UsersDataTable = objAdaptor.FetchData(SqlQuery);
			if (UsersDataTable.Rows.Count > 0)
				success = Convert.ToBoolean(UsersDataTable.Rows[0]["IsFirstTimeLogin"].ToString());
			
			return success;
		}
		public Object UpdateIsFirstTimeLogin(int UserID)
		{
			Object Update;
			try
			{
				string Query = "Update Users set IsFirstTimeLogin='False' WHERE ID =" + UserID;
				Update = objAdaptor.ExecuteNonQuery(Query);
			}
			catch 
			{
				Update = false;
			}
			return Update;
		}
		public DataTable GetAllData()
		{

			DataTable UsersDataTable = new DataTable();
			Users ReturnUserobject = new Users();
			try
			{
                //9/22/2015 NS modified
				string SqlQuery = "SELECT [ID],[LoginName], [Password], [FullName], [Email], [Status], [SuperAdmin],[IsConfigurator],[IsDashboard],[IsConsoleComm],[CloudApplications],[OnPremisesApplications],[NetworkInfrastucture],[DominoServerMetrics] FROM [Users] " +
                    "ORDER BY FullName";
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
				string SqlQuery = "INSERT INTO [Users] (LoginName,Password,FullName,Email,Status,SuperAdmin)" +
				"VALUES('" + UsersObject.LoginName + "','" + UsersObject.Password + "','" + UsersObject.FullName +
				"','" + UsersObject.Email + "','" + UsersObject.Status + "','" + UsersObject.SuperAdmin + "')";


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
				"', [SuperAdmin]='" + UsersObject.SuperAdmin + "' ,[IsConfigurator]='" + UsersObject.IsConfigurator +
				"', [IsDashboard]='" + UsersObject.Isdashboard + "', [IsConsoleComm]='" + UsersObject.Isconsolecomm + "' WHERE ID=" + UsersObject.ID + "";
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
				"',[SecurityQuestion2Answer]=  '" + UserAccObject.SecurityQuestion2Answer + "',Refreshtime=" + UserAccObject.Refreshtime + ",StartupURL='" + UserAccObject.StartupURL + "',CustomBackground='" + UserAccObject.CustomBackground + "',CloudApplications='" + UserAccObject.CloudApplications + "',OnPremisesApplications='" + UserAccObject.OnPremisesApplications + "',NetworkInfrastucture='" + UserAccObject.NetworkInfrastucture + "',DominoServerMetrics='" + UserAccObject.DominoServerMetrics + "' WHERE ID=" + UserAccObject.ID + " ";



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
		public bool UpdateAccount1(Users UserAccObject)
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
                string SqlQuery = "insert into [Users]([LoginName],[FullName],[password],[Email],Status,SuperAdmin,IsConfigurator,IsDashboard,IsConsoleComm,CloudApplications,OnPremisesApplications,NetworkInfrastucture,DominoServerMetrics,IsFirstTimeLogin,[cloudindex],[premisesindex],[networkindex],[dockindex],[cloudZone],[premisesZone],[networkZone] ,[DockZone]) values('" + UserAccObject.LoginName +
				   "','" + UserAccObject.FullName + "','" + UserAccObject.Password + "','" + UserAccObject.Email + "','" + UserAccObject.Status +
               "','" + UserAccObject.SuperAdmin + "','" + UserAccObject.IsConfigurator + "','" + UserAccObject.Isdashboard + "','" + UserAccObject.Isconsolecomm + "','" + UserAccObject.CloudApplications + "','" + UserAccObject.OnPremisesApplications + "','" + UserAccObject.NetworkInfrastucture + "','" + UserAccObject.DominoServerMetrics + "','" + UserAccObject.IsFirstTimeLogin + "','" + UserAccObject.cloudindex + "','" + UserAccObject.premisesindex + "','" + UserAccObject.networkindex + "','" + UserAccObject.dockindex + "','" + UserAccObject.cloudZone + "','" + UserAccObject.premisesZone + "','" + UserAccObject.networkZone + "','" + UserAccObject.DockZone + "')";


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
		public DataTable GetIsAdmin1(string id)
		{
			DataTable dt = new DataTable();
			try
			{
				string q = "Select SuperAdmin from [Users] where ID=" + id + "";
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

		public DataTable GetIsAdmin(string name)
		{
			DataTable dt = new DataTable();
			try
			{
                //6/23/2015 NS modified - the query needs to check the ID, not LoginName
				//string q = "Select SuperAdmin from [Users] where LoginName='" + name + "'";
                string q = "Select SuperAdmin from [Users] where ID=" + name;
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
		public DataTable GetIsnotLoggedIn(string name)
		{
			DataTable dt = new DataTable();
			try
			{

				string q = "Select IsFirstTimeLogin from [Users] where LoginName='" + name + "'";
				
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

		public DataTable GetIsConsoleComm(string userid)
		{
			DataTable dt = new DataTable();
			try
			{
				string q = "Select IsConsoleComm from [Users] where ID=" + userid + "";
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

		//1/2/2014 NS added
		public DataTable GetIsConfig(string userid)
		{
			DataTable dt = new DataTable();
			try
			{
				string q = "Select IsConfigurator from [Users] where ID=" + userid + "";
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

        //9/28/2015 NS added for VSPLUS-2170
        public DataTable GetUserAccessData()
        {

            DataTable UsersDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT t1.[ID],[FullName],[SuperAdmin],dbo.ConcatUserRestrictions(t1.[ID]) Restrictions " +
                    "FROM [Users] t1 " +
                    "LEFT OUTER JOIN  dbo.UserLocationRestrictions t2 ON t1.ID=t2.UserID LEFT OUTER JOIN dbo.Locations t4 ON " +
                    "t2.LocationID=t4.ID " + 
                    "LEFT OUTER JOIN dbo.UserServerRestrictions t3 ON t1.ID=t3.UserID LEFT OUTER JOIN dbo.Servers t5 ON " +
                    "t3.ServerID=t5.ID " +
                    "GROUP BY t1.ID,FullName,SuperAdmin ";
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
	}
}
