using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
    public class URLsDAL
    {
        ///<summary>
        ///Declarations
        ///</summary>
        private Adaptor objAdaptor = new Adaptor();
        private static URLsDAL _self = new URLsDAL();

        public static URLsDAL Ins
        {

            get { return _self; }
        }

        /// <summary>j
        /// Get all Data from URLs
        /// </summary>

        public DataTable GetAllData()
        {

            DataTable URLsDataTable = new DataTable();
            URLs ReturnDCObject = new URLs();
            try
            {
                //2/5/2014 NS modified the query - adde sort by url
                string SqlQuery = "SELECT * FROM URLs " +
                    "ORDER BY Name";

                URLsDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return URLsDataTable;
        }

        /// <summary>
        /// Get Data from URLs based on Key
        /// </summary>
        public URLs GetData(URLs URLObject)
        {
            DataTable URLsDataTable = new DataTable();
            URLs ReturnObject = new URLs();
            try
            {
                //11/19/2013 NS modified
                //string SqlQuery = "Select urls.*,t2.Location from URLs  INNER JOIN [Locations] t2 ON urls.[LocationID] = t2.[ID]  where [TheURL]='" + URLObject.TheURL + "'";
				string SqlQuery = "Select urls.*,t2.Location from URLs  INNER JOIN [Locations] t2 ON urls.[LocationID] = t2.[ID]  " +
				    "where urls.[ID]=" + URLObject.ID;
				//string SqlQuery="select * from urls where URLs where ID="URLObject.ID
                URLsDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
               //11/19/2013 NS added
                ReturnObject.ID = URLsDataTable.Rows[0]["ID"].ToString();
                ReturnObject.Name = URLsDataTable.Rows[0]["Name"].ToString();
                if (URLsDataTable.Rows[0]["ScanInterval"].ToString() != "")
                    ReturnObject.ScanInterval = int.Parse(URLsDataTable.Rows[0]["ScanInterval"].ToString());
                if (URLsDataTable.Rows[0]["OffHoursScanInterval"].ToString() != "")
                    ReturnObject.OffHoursScanInterval = int.Parse(URLsDataTable.Rows[0]["OffHoursScanInterval"].ToString());
                ReturnObject.Category = URLsDataTable.Rows[0]["Category"].ToString();
				if (URLsDataTable.Rows[0]["Enabled"].ToString() != "")
					ReturnObject.Enabled = bool.Parse(URLsDataTable.Rows[0]["Enabled"].ToString());

                if (URLsDataTable.Rows[0]["RetryInterval"].ToString() != "")
                    ReturnObject.RetryInterval = int.Parse(URLsDataTable.Rows[0]["RetryInterval"].ToString());
                if(URLsDataTable.Rows[0]["ResponseThreshold"].ToString()!="")
                    ReturnObject.ResponseThreshold=int.Parse(URLsDataTable.Rows[0]["ResponseThreshold"].ToString());
                ReturnObject.TheURL = URLsDataTable.Rows[0]["TheURL"].ToString();
                ReturnObject.SearchStringNotFound = URLsDataTable.Rows[0]["SearchString"].ToString();
				ReturnObject.SearchStringFound = URLsDataTable.Rows[0]["AlertStringFound"].ToString();
                ReturnObject.UserName = URLsDataTable.Rows[0]["UserName"].ToString();
                ReturnObject.PW = URLsDataTable.Rows[0]["PW"].ToString();
                ReturnObject.Location = URLsDataTable.Rows[0]["Location"].ToString();
                if ( URLsDataTable.Rows[0]["FailureThreshold"].ToString() != null)
                    ReturnObject.FailureThreshold = Convert.ToInt32(URLsDataTable.Rows[0]["FailureThreshold"]);
            }
			catch (Exception ex)
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

        public bool InsertData(URLs URLObject)
        {
            bool Insert = false;
            try
            {
				string SqlQuery = "INSERT INTO URLs (TheURL,Name,Category,ScanInterval,OffHoursScanInterval,Enabled" +
                    ",ResponseThreshold,RetryInterval,SearchString,AlertStringFound,UserName,PW,LocationId,ServerTypeId,FailureThreshold)" +
                    "VALUES('" + URLObject.TheURL + "','" + URLObject.Name + "','" + URLObject.Category + "'," + URLObject.ScanInterval +
					"," + URLObject.OffHoursScanInterval + ",'" + URLObject.Enabled + "'," + URLObject.ResponseThreshold + "," + URLObject.RetryInterval +
                    ",'" + URLObject.SearchStringNotFound + "','" + URLObject.SearchStringFound + "','" + URLObject.UserName + "','" + URLObject.PW + "'," + URLObject.LocationId + "," + URLObject.ServerTypeId + "," + URLObject.FailureThreshold + ")";
                    


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
        /// Update data into URLs table
        /// </summary>
        /// <param name="DSObject">DominoServers object</param>
        /// <returns></returns>
        public Object UpdateData(URLs URLObject)
        {
            Object Update;
            try
            {
                //11/19/2013 NS modified
                //string SqlQuery = "UPDATE URLs SET Name='" + URLObject.Name + "',Category='" + URLObject.Category + "',ScanInterval=" + URLObject.ScanInterval +
                //",ResponseThreshold=" + URLObject.ResponseThreshold + ",Enabled='" + URLObject.Enabled + "',OffHoursScanInterval=" + URLObject.OffHoursScanInterval +
                //",RetryInterval=" + URLObject.RetryInterval +",SearchString='" + URLObject.SearchString +"',UserName='" + URLObject.UserName + "',PW='" + URLObject.PW+
                //"',LocationId=" + URLObject.LocationId + ",ServerTypeId=" + URLObject.ServerTypeId + ",FailureThreshold=" + URLObject.FailureThreshold + " WHERE [TheURL]='" + URLObject.TheURL + "'";
				string SqlQuery = "UPDATE URLs SET Name='" + URLObject.Name + "', TheURL='" + URLObject.TheURL + "',Category='" + URLObject.Category + "',ScanInterval=" + URLObject.ScanInterval +
				",ResponseThreshold=" + URLObject.ResponseThreshold + ",Enabled='" + URLObject.Enabled + "',OffHoursScanInterval=" + URLObject.OffHoursScanInterval +
                ",RetryInterval=" + URLObject.RetryInterval + ",SearchString='" + URLObject.SearchStringNotFound + "',AlertStringFound='" + URLObject.SearchStringFound + "',UserName='" + URLObject.UserName + "',PW='" + URLObject.PW +
                "',LocationId=" + URLObject.LocationId + ",ServerTypeId=" + URLObject.ServerTypeId + ",FailureThreshold=" + URLObject.FailureThreshold + 
                " WHERE [ID]=" + URLObject.ID;
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

        //delete Data from URLs Table

        public Object DeleteData(URLs URLObject)
        {
            Object Update;
            try
            {
                //11/19/2013 NS modified
                //string SqlQuery = "Delete URLs Where TheURL='" + URLObject.TheURL+"'";
                string SqlQuery = "Delete URLs Where ID=" + URLObject.ID;

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


        public DataTable GetIPAddress(URLs UrlObj, string mode)
        {
            //SametimeServers SametimeObj = new SametimeServers();
            DataTable UrlTable = new DataTable();
            try
            {
                if (mode=="Insert")//UrlObj.TheURL == "" && UrlObj.TheURL == null)
                {
                    //11/19/2013 NS modified
                    string sqlQuery = "Select * from URLs where TheURL='" + UrlObj.TheURL + "' or Name='" + UrlObj.Name + "' ";
                    //string sqlQuery = "Select * from URLs where ID=" + UrlObj.ID;
                    UrlTable = objAdaptor.FetchData(sqlQuery);
                }
                else {
                    string sqlQuery = "Select * from URLs where  ID<>"+UrlObj.ID + " AND TheURL='" + UrlObj.TheURL + "' ";
                    UrlTable = objAdaptor.FetchData(sqlQuery);
                
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
                    SqlQuery = "INSERT INTO UserCustomPages (UserID,URL,Title,IsPrivate) VALUES('" + userID + "','" + URLval + "','" + titleval + "','" + isprivate + "')";
                }
                else
                {
                    SqlQuery = "UPDATE UserCustomPages SET URL='" + URLval + "', Title='" + titleval + "', IsPrivate='" + isprivate + "' " +
                        "WHERE UserID=" + userID + " AND ID=" + ID;
                }
                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
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
                string query = "SELECT URL FROM UserCustomPages WHERE UserID=" + userID;
                dt = objAdaptor.FetchData(query);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }
        public Int32 GetServerIDbyServerName(string serverName)
        {
            DataTable ServersDataTable = new DataTable();
            int ID = 0;
            try
            {
                string SqlQuery = "SELECT ID FROM URLs WHERE Name='" + serverName + "'";
                ServersDataTable = objAdaptor.FetchData(SqlQuery);
				if (ServersDataTable.Rows.Count > 0)
				{
					ID = Convert.ToInt32(ServersDataTable.Rows[0]["ID"]);
				}
            }
            catch (Exception ex)
			{
				throw ex;
			}
			finally
            {
            }
            return ID;
        }
        //14/04/2016 Sowmya added for VSPLUS-2725
        public DataTable GetURLDetails(string serverName)
        {
            DataTable ServersDataTable = new DataTable();
            
            try
            {
                string SqlQuery = "SELECT * FROM URLs WHERE Name='" + serverName + "'";
                ServersDataTable = objAdaptor.FetchData(SqlQuery);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return ServersDataTable;
        }
    }
}
