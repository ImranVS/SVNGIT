using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomerTrackingDO;
using System.Data;


namespace CustomerTrackingDL
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
        public DataTable GetAllData()
        {

            DataTable UsersDataTable = new DataTable();
            Users ReturnUserobject = new Users();
            try
            {
                string SqlQuery = "SELECT [ID],[LoginName], [Password], [FullName], [Email], [Status], [SuperAdmin],[IsConfigurator],[IsDashboard],[IsConsoleComm] FROM [Users]";
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
                string SqlQuery = "UPDATE [Users] SET [LoginName]='" + UserAccObject.LoginName +
                   "', [FullName]='" + UserAccObject.FullName + "', [Email]='" + UserAccObject.Email + "',[SecurityQuestion1]='" + UserAccObject.SecurityQuestion1 +
               "',[SecurityQuestion1Answer]='" + UserAccObject.SecurityQuestion1Answer + "',[SecurityQuestion2]='" + UserAccObject.SecurityQuestion2.Replace("'", "''") +
                "',[SecurityQuestion2Answer]=  '" + UserAccObject.SecurityQuestion2Answer + "',Refreshtime=" + UserAccObject.Refreshtime + ",StartupURL='" + UserAccObject.StartupURL + "' WHERE ID=" + UserAccObject.ID + " ";



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
                string SqlQuery = "insert into [Users]([LoginName],[FullName],[password],[Email],Status,SuperAdmin,IsConfigurator,IsDashboard,IsConsoleComm) values('" + UserAccObject.LoginName +
                   "','" + UserAccObject.FullName + "','" + UserAccObject.Password + "','" + UserAccObject.Email + "','" + UserAccObject.Status +
               "','" + UserAccObject.SuperAdmin + "','" + UserAccObject.IsConfigurator + "','" + UserAccObject.Isdashboard + "','" + UserAccObject.Isconsolecomm + "')";


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
                string q = "Select SuperAdmin from [Users] where ID=" + userid + "";
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
    }
}
