using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using System.Data.SqlClient;


namespace VSWebDAL.ConfiguratorDAL
{
    public class CredentialsDAL
    {
        private Adaptor objAdaptor = new Adaptor();
        private static CredentialsDAL _self = new CredentialsDAL();

        public static CredentialsDAL Ins
        {
            get { return _self; }
        }

        public DataTable GetAllData()
        {
            DataTable Credentialstab = new DataTable();
            try
            {

               // string SqlQuery = "Select * from Credentials";
				
				string SqlQuery = "SELECT cd.AliasName, cd.ID,cd.UserID,'test' Password,st.ServerType FROM Credentials cd left outer JOIN ServerTypes st on cd.ServerTypeID=st.id";
				Credentialstab = objAdaptor.FetchData(SqlQuery); 
			}
            catch (Exception ex)
            {

                throw ex;
            }

            return Credentialstab;


        }

        public string DeleteData(Credentials LOCbject)
        {
            string Update;
            try
            {

				string SqlQuery = "Delete from Credentials Where ID=" + LOCbject .ID+ "";
				Update = objAdaptor.ExecuteNonQuerynotreturn(SqlQuery);
            }
            catch(Exception)
            {
                Update = "false";
            }
            finally
            {
            }
            return Update;
        }
        public bool UpdateData(Credentials LOCbject)
		{
			string SqlQuery="";
            bool Update;
			
            try
            {
                //7/10/2015 NS modified for VSPLUS-1985
				//if (LOCbject.Password == "      ")
                if (LOCbject.Password == "test")
				{
					SqlQuery = "UPDATE Credentials SET AliasName = @AliasName,UserID = @UserID, ServerTypeID= @ServerTypeID WHERE ID = @ID";

					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@AliasName", (object)LOCbject.AliasName ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@UserID", (object)LOCbject.UserID ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ServerTypeID", (object)LOCbject.ServerTypeID ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ID", (object)LOCbject.ID ?? DBNull.Value);
					Update = objAdaptor.ExecuteNonQuerywithcmd(cmd);
				}
				else
				{
					SqlQuery = "UPDATE Credentials SET AliasName= @AliasName,UserID= @UserID, Password= @Password, ServerTypeID= @ServerTypeID WHERE ID = @ID";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@AliasName", (object)LOCbject.AliasName ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@UserID", (object)LOCbject.UserID ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@Password", (object)LOCbject.Password ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ServerTypeID", (object)LOCbject.ServerTypeID ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ID", (object)LOCbject.ID ?? DBNull.Value);
					Update = objAdaptor.ExecuteNonQuerywithcmd(cmd);
				}
					//Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				
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
        public bool InsertData(Credentials LOCbject)
        {

            bool Insert = false;
            try
            {
				string SqlQuery = "INSERT INTO Credentials (AliasName,UserID,Password,ServerTypeID) VALUES(@AliasName,@UserID,@Password,@ServerTypeID)";
                //Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@AliasName", (object)LOCbject.AliasName ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@UserID", (object)LOCbject.UserID ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Password", (object)LOCbject.Password ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@ServerTypeID", (object)LOCbject.ServerTypeID ?? DBNull.Value);
				Insert = objAdaptor.ExecuteNonQuerywithcmd(cmd);
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
        public DataTable GetDataForCredentialsByname(Credentials LOCbject)
        {
            DataTable LocationsDataTable = new DataTable();
            try
            {
				string SqlQuery = "Select * from Credentials where AliasName = @AliasName";
                
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@AliasName", (object)LOCbject.AliasName ?? DBNull.Value);
				LocationsDataTable=objAdaptor.FetchDatafromcommand(cmd);
                //populate & return data object
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return LocationsDataTable;
        }
        public DataTable GetDataForCredentialsById(Credentials LOCbject)
        {
            DataTable LocationsDataTable = new DataTable();
            try
            {
                //string SqlQuery = "Select * from Credentials where AliasName='" + LOCbject.AliasName + "'";
				string SqlQuery = "Select * from Credentials where ID= @ID";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ID", (object)LOCbject.ID ?? DBNull.Value);
				LocationsDataTable = objAdaptor.FetchDatafromcommand(cmd);
                //populate & return data object
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return LocationsDataTable;
        }

        public DataTable GetpwrshlCredentials(string cred,int serverid)
        {
            bool result = false;
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
             string SqlQuery="";
            try
            {

				SqlQuery = "select * from MSServerSettings where ServerID=@ServerID";
             
			 SqlCommand cmd = new SqlCommand(SqlQuery);
			 cmd.Parameters.AddWithValue("@ServerID", (object)serverid ?? DBNull.Value);
			 dt1 = objAdaptor.FetchDatafromcommand(cmd);
             //result = objAdaptor.ExecuteNonQuery(SqlQuery);

             //if (result == true)
             if (dt1.Rows.Count > 0)
             {
				 SqlQuery = "select * from Credentials where AliasName = @AliasName";
				 SqlCommand cmd1 = new SqlCommand(SqlQuery);
				 cmd.Parameters.AddWithValue("@AliasName", (object)cred ?? DBNull.Value);
				 dt = objAdaptor.FetchDatafromcommand(cmd1);
                
             }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dt;
        }
    }
}
