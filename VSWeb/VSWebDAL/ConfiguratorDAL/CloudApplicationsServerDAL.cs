using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using System.Data.SqlClient;

namespace VSWebDAL.ConfiguratorDAL
{
    public class CloudApplicationsServerDAL
    {
        ///<summary>
        ///Declarations
        ///</summary>
        private Adaptor objAdaptor = new Adaptor();
        private static CloudApplicationsServerDAL _self = new CloudApplicationsServerDAL();

        public static CloudApplicationsServerDAL Ins
        {

            get { return _self; }
        }

        /// <summary>
        /// Get all Data from URLs
        /// </summary>

        public DataTable GetAllData()
        {

            DataTable CloudApplicationsServerDataTable = new DataTable();
            URLs ReturnDCObject = new URLs();
            try
            {
                //2/5/2014 NS modified the query - adde sort by url
                string SqlQuery = "SELECT * FROM CloudDetails " +
                    "ORDER BY Name";

                CloudApplicationsServerDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return CloudApplicationsServerDataTable;
        }

        /// <summary>
        /// Get Data from CloudApplicationsServer based on Key
        /// </summary>
        public CloudApplicationsServer GetData(CloudApplicationsServer URLObject)
        {
            DataTable CloudApplicationsServerDataTable = new DataTable();
            CloudApplicationsServer ReturnObject = new CloudApplicationsServer();
            try
            {
                string SqlQuery = "Select * from CloudDetails where ID=" + URLObject.ID;
                CloudApplicationsServerDataTable = objAdaptor.FetchData(SqlQuery);

                //populate & return data object
                //11/19/2013 NS added
                ReturnObject.ID = CloudApplicationsServerDataTable.Rows[0]["ID"].ToString();
                ReturnObject.Name = CloudApplicationsServerDataTable.Rows[0]["Name"].ToString();
                if (CloudApplicationsServerDataTable.Rows[0]["ScanInterval"].ToString() != "")
                    ReturnObject.ScanInterval = int.Parse(CloudApplicationsServerDataTable.Rows[0]["ScanInterval"].ToString());
                if (CloudApplicationsServerDataTable.Rows[0]["OffHoursScanInterval"].ToString() != "")
                    ReturnObject.OffHoursScanInterval = int.Parse(CloudApplicationsServerDataTable.Rows[0]["OffHoursScanInterval"].ToString());
                ReturnObject.Category = CloudApplicationsServerDataTable.Rows[0]["Category"].ToString();
                if (CloudApplicationsServerDataTable.Rows[0]["Enabled"].ToString() != "")
                    ReturnObject.Enabled = bool.Parse(CloudApplicationsServerDataTable.Rows[0]["Enabled"].ToString());

                if (CloudApplicationsServerDataTable.Rows[0]["RetryInterval"].ToString() != "")
                    ReturnObject.RetryInterval = int.Parse(CloudApplicationsServerDataTable.Rows[0]["RetryInterval"].ToString());
                if (CloudApplicationsServerDataTable.Rows[0]["ResponseThreshold"].ToString() != "")
                    ReturnObject.ResponseThreshold = int.Parse(CloudApplicationsServerDataTable.Rows[0]["ResponseThreshold"].ToString());
                ReturnObject.URL = CloudApplicationsServerDataTable.Rows[0]["URL"].ToString();
                ReturnObject.SearchStringNotFound = CloudApplicationsServerDataTable.Rows[0]["SearchString"].ToString();
                ReturnObject.UserName = CloudApplicationsServerDataTable.Rows[0]["UserName"].ToString();
                ReturnObject.PW = CloudApplicationsServerDataTable.Rows[0]["PW"].ToString();
                ReturnObject.Location = CloudApplicationsServerDataTable.Rows[0]["Location"].ToString();
                ReturnObject.imageurl = CloudApplicationsServerDataTable.Rows[0]["imageurl"].ToString();
                if (CloudApplicationsServerDataTable.Rows[0]["FailureThreshold"].ToString() != null)
                    ReturnObject.FailureThreshold = Convert.ToInt32(CloudApplicationsServerDataTable.Rows[0]["FailureThreshold"]);
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return ReturnObject;
        }
        /// <summary>
        /// Insert data into URLs table
        /// </summary>
        /// <param name="DSObject">URLs object</param>
        /// <returns></returns>

        public bool InsertData(CloudApplicationsServer CloudApplicationsServerObject)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO CloudDetails (URL,Name,Category,ScanInterval,OffHoursScanInterval,Enabled" +
                    ",ResponseThreshold,RetryInterval,SearchString,AlertStringFound,UserName,PW,Location,ServerTypeId,FailureThreshold,Imageurl)" +
					"VALUES(@URL,@Name,@Category,@ScanInterval" +
                    ",@OffHoursScanInterval,@Enabled,@ResponseThreshold,@RetryInterval"+
					",@SearchString,@AlertStringFound,@UserName,@PW" +
				   ", @Location,@ServerTypeId,@FailureThreshold,@Imageurl)";

				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@URL", (object)CloudApplicationsServerObject.URL ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Name", (object)CloudApplicationsServerObject.Name ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Category", (object)CloudApplicationsServerObject.Category ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@ScanInterval", (object)CloudApplicationsServerObject.ScanInterval ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@OffHoursScanInterval", (object)CloudApplicationsServerObject.OffHoursScanInterval ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Enabled", (object)CloudApplicationsServerObject.Enabled ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@ResponseThreshold", (object)CloudApplicationsServerObject.ResponseThreshold ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@RetryInterval", (object)CloudApplicationsServerObject.RetryInterval ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@SearchString", (object)CloudApplicationsServerObject.SearchStringNotFound ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@AlertStringFound", (object)CloudApplicationsServerObject.SearchStringFound ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@UserName", (object)CloudApplicationsServerObject.UserName ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@PW", (object)CloudApplicationsServerObject.PW ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Location", (object)CloudApplicationsServerObject.LocationId ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@ServerTypeId", (object)CloudApplicationsServerObject.ServerTypeId ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@FailureThreshold", (object)CloudApplicationsServerObject.FailureThreshold ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Imageurl", (object)CloudApplicationsServerObject.imageurl ?? DBNull.Value);
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

        /// <summary>
        /// Update data into URLs table
        /// </summary>
        /// <param name="DSObject">DominoServers object</param>
        /// <returns></returns>
        public Object UpdateData(CloudApplicationsServer CloudApplicationsServerObject)
        {
            Object Update;
            try
            {
                //11/19/2013 NS modified
                //string SqlQuery = "UPDATE URLs SET Name='" + URLObject.Name + "',Category='" + URLObject.Category + "',ScanInterval=" + URLObject.ScanInterval +
                //",ResponseThreshold=" + URLObject.ResponseThreshold + ",Enabled='" + URLObject.Enabled + "',OffHoursScanInterval=" + URLObject.OffHoursScanInterval +
                //",RetryInterval=" + URLObject.RetryInterval +",SearchString='" + URLObject.SearchString +"',UserName='" + URLObject.UserName + "',PW='" + URLObject.PW+
                //"',LocationId=" + URLObject.LocationId + ",ServerTypeId=" + URLObject.ServerTypeId + ",FailureThreshold=" + URLObject.FailureThreshold + " WHERE [TheURL]='" + URLObject.TheURL + "'";
				string SqlQuery = "UPDATE CloudDetails SET Name=@Name,Url=@URL,Category=@Category,ScanInterval=@ScanInterval" +
				",ResponseThreshold=@ResponseThreshold,Enabled=@Enabled,OffHoursScanInterval=@OffHoursScanInterval" +
				",RetryInterval=@RetryInterval,SearchString=@SearchString,AlertStringFound=@AlertStringFound,UserName=@UserName,PW=@PW"+
				"',Location=@Location,ServerTypeId=@ServerTypeId,FailureThreshold=@FailureThreshold" +
                " WHERE [ID]=@ID";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ID", (object)CloudApplicationsServerObject.ID ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@URL", (object)CloudApplicationsServerObject.URL ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Name", (object)CloudApplicationsServerObject.Name ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Category", (object)CloudApplicationsServerObject.Category ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@ScanInterval", (object)CloudApplicationsServerObject.ScanInterval ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@OffHoursScanInterval", (object)CloudApplicationsServerObject.OffHoursScanInterval ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Enabled", (object)CloudApplicationsServerObject.Enabled ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@ResponseThreshold", (object)CloudApplicationsServerObject.ResponseThreshold ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@RetryInterval", (object)CloudApplicationsServerObject.RetryInterval ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@SearchString", (object)CloudApplicationsServerObject.SearchStringNotFound ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@AlertStringFound", (object)CloudApplicationsServerObject.SearchStringFound ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@UserName", (object)CloudApplicationsServerObject.UserName ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@PW", (object)CloudApplicationsServerObject.PW ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Location", (object)CloudApplicationsServerObject.LocationId ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@ServerTypeId", (object)CloudApplicationsServerObject.ServerTypeId ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@FailureThreshold", (object)CloudApplicationsServerObject.FailureThreshold ?? DBNull.Value);
				//cmd.Parameters.AddWithValue("@Imageurl", (object)CloudApplicationsServerObject.imageurl ?? DBNull.Value);
				Update = objAdaptor.ExecuteNonQuerywithcmd(cmd);
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

        //delete Data from URLs Table

        public Object DeleteData(CloudApplicationsServer CloudApplicationsServerObject)
        {
            Object Update;
            try
            {
                //11/19/2013 NS modified
                //string SqlQuery = "Delete URLs Where TheURL='" + URLObject.TheURL+"'";
				string SqlQuery = "Delete CloudDetails Where ID = @ID" ;
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ID", (object)CloudApplicationsServerObject.ID ?? DBNull.Value);
				Update = objAdaptor.ExecuteNonQuerywithcmd(cmd);
               
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


        public DataTable GetIPAddress(CloudApplicationsServer UrlObj, string mode)
        {
            DataTable UrlTable = new DataTable();
            try
            {
                if (mode == "Insert")
                {
					string sqlQuery = "Select * from CloudDetails where URL= @URL";
					SqlCommand cmd = new SqlCommand(sqlQuery);
					cmd.Parameters.AddWithValue("@URL", (object)UrlObj.URL ?? DBNull.Value);
					UrlTable = objAdaptor.FetchDatafromcommand(cmd);
                   // UrlTable = objAdaptor.FetchData(sqlQuery);
                }
                else
                {
					string sqlQuery = "Select * from CloudDetails where  ID<> @ID AND URL= @URL ";
					SqlCommand cmd = new SqlCommand(sqlQuery);
					cmd.Parameters.AddWithValue("@URL", (object)UrlObj.URL ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ID", (object)UrlObj.ID ?? DBNull.Value);
					UrlTable = objAdaptor.FetchDatafromcommand(cmd);
                   // UrlTable = objAdaptor.FetchData(sqlQuery);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return UrlTable;

        }

        public bool InsertCustomPageValue(string userID, string URLval, string titleval, bool isprivate, string ID, bool doinsert)
        {
            bool Insert = false;
            string SqlQuery = "";
            try
            {
                if (doinsert)
                {
					SqlQuery = "INSERT INTO UserCustomPages (UserID,URL,Title,IsPrivate) VALUES(@UserID,@URL,@Title,@IsPrivate)";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@UserID", (object)userID ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@URL", (object)URLval ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@Title", (object)titleval ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@IsPrivate", (object)isprivate ?? DBNull.Value);
					Insert = objAdaptor.ExecuteNonQuerywithcmd(cmd);
                }
                else
                {
					SqlQuery = "UPDATE UserCustomPages SET URL= @URL, Title=@Title, IsPrivate=@IsPrivate " +
						"WHERE UserID = @UserID AND ID=@ID";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@ID", (object)ID ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@UserID", (object)userID ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@URL", (object)URLval ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@Title", (object)titleval ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@IsPrivate", (object)isprivate ?? DBNull.Value);
					Insert = objAdaptor.ExecuteNonQuerywithcmd(cmd);
                }
               // Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Insert;
        }

        public DataTable GetCustomPageValue(string userID)
        {
            DataTable dt = new DataTable();
            try
            {
				string query = "SELECT URL FROM UserCustomPages WHERE UserID= @UserID";
				SqlCommand cmd = new SqlCommand(query);
				cmd.Parameters.AddWithValue("@UserID", (object)userID ?? DBNull.Value);
				dt = objAdaptor.FetchDatafromcommand(cmd);
                //dt = objAdaptor.FetchData(query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        public Int32 GetServerIDbyServerName(string serverName)
        {
            DataTable ServersDataTable = new DataTable();
            int ID = 0;
            try
            {
				string SqlQuery = "SELECT ID FROM CloudDetails WHERE Name= @Name";
				SqlCommand cmd = new SqlCommand(SqlQuery);
                //ServersDataTable = objAdaptor.FetchData(SqlQuery);
				cmd.Parameters.AddWithValue("@Name", (object)serverName ?? DBNull.Value);
				ID = objAdaptor.ExecuteScalarwithcmd(cmd);
                //ID = Convert.ToInt32(ServersDataTable.Rows[0]["ID"]);
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {

            }
            return ID;
        }
        public DataTable GetCloudData()
        {
            DataTable dt = new DataTable();
            try
            {
                //string Query = "select cd.Name, cd.Imageurl, cd.URL,st.StatusCode,st.Lastupdate from CloudDetails cd, Status st where st.TypeANDName= cd.Name+'-Cloud'";
                string Query = "select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate from [Clouddetails] nd, Status st,Users us where nd.UserName=us.LoginName and st.TypeANDName= nd.Name+'-Cloud' and us.CloudApplications='true'";

                dt = objAdaptor.FetchData(Query);
            }
            catch(Exception ex)
            {
				throw ex;
            }
            return dt;
        }
        public DataTable GetCloudDatavisible()
        {
            DataTable dt = new DataTable();
            try
            {
                //string Query = "select cd.Name, cd.Imageurl, cd.URL,st.StatusCode,st.Lastupdate from CloudDetails cd, Status st where st.TypeANDName= cd.Name+'-Cloud'";
                //string Query = "select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate from [Clouddetails] nd, Status st,Users us where nd.UserName=us.LoginName and st.TypeANDName= nd.Name+'-Cloud' ";
				string Query="select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate,'CloudDetails.aspx?Name=' + nd.Name  url  from [Clouddetails] nd, Status st where st.TypeANDName= nd.Name+'-Cloud'";
					//"union select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate,'CloudDetails.aspx?Name=' + nd.Name  url  from [O365Server] nd, Status st where st.TypeANDName= nd.Name+'-Office365'";//soma
               // string Query = "select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate from [O365Server] nd, Status st ,Users us where nd.UserName=us.LoginName and st.TypeANDName= nd.Name+'-Office365' union select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate from [Clouddetails] nd, Status st,Users us where nd.UserName=us.LoginName and st.TypeANDName= nd.Name+'-Cloud'";
                dt = objAdaptor.FetchData(Query);
            }
            catch(Exception ex)
            {
				throw ex;
            }
            return dt;
        }
        public DataTable GetCloudDatavisiblefordashboard()
        {
            DataTable dt = new DataTable();
            try
            {
                //Mukund 18Feb15, added url field 
                string Query = "select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate,'CloudDetails.aspx?Name=' + nd.Name  url  from [Clouddetails] nd, Status st where st.TypeANDName= nd.Name+'-Cloud'";
					//union select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate,'CloudDetails.aspx?Name=' + nd.Name  url  from [O365Server] nd, Status st where st.TypeANDName= nd.Name+'-Office365'";//soma
                //string Query = "select cd.Name, cd.Imageurl, cd.URL,st.StatusCode,st.Lastupdate from CloudDetails cd, Status st where st.TypeANDName= cd.Name+'-Cloud'";
                //string Query = "select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate from [Clouddetails] nd, Status st,Users us where nd.UserName=us.LoginName and st.TypeANDName= nd.Name+'-Cloud' ";

                dt = objAdaptor.FetchData(Query);
            }
            catch(Exception ex)
            {
				throw ex;
            }
            return dt;
        }


        public DataTable GetCloudStatuses()
        {
            DataTable dt = new DataTable();
            try
            {
                //string Query = "select cd.Name, cd.Imageurl, cd.URL,st.StatusCode,st.Lastupdate from CloudDetails cd, Status st where st.TypeANDName= cd.Name+'-Cloud'";
                string Query = "(select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate,'CloudDetails.aspx?Name=' + nd.Name  url from [Clouddetails] nd, Status st where st.TypeANDName= nd.Name+'-Cloud') ";
               // Query += " union ";
               // Query += " (select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate,'office365health.aspx?Name=' + nd.Name  url from [O365Server] nd, Status st where st.TypeANDName= nd.Name+'-Office365')";//soma
                dt = objAdaptor.FetchData(Query);
            }
            catch(Exception ex)
            {
				throw ex;
            }
            return dt;
        }

        public DataTable GetOffice365Statuses()
        {
            DataTable dt = new DataTable();
            try
            {
                //string Query = "select cd.Name, cd.Imageurl, cd.URL,st.StatusCode,st.Lastupdate from CloudDetails cd, Status st where st.TypeANDName= cd.Name+'-Cloud'";
                string Query = "select nd.Name, nd.ImageURL, st.StatusCode,st.Lastupdate from [O365Server] nd, Status st where st.TypeANDName= nd.Name+'-Office365' ";

                dt = objAdaptor.FetchData(Query);
            }
            catch(Exception ex)
            {
				throw ex;
            }
            return dt;
        }

    }
}
