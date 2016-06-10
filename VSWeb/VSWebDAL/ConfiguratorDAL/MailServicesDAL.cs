using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using System.Configuration;

namespace VSWebDAL.ConfiguratorDAL
{
    public class MailServicesDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static MailServicesDAL _self = new MailServicesDAL();

        public static MailServicesDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from MailServices
        /// </summary>

        public DataTable GetAllData()
        {

            DataTable MailServicesDataTable = new DataTable();
            MailServices ReturnMailObject = new MailServices();
            try
            {
                string SqlQuery = "select * from MailServices";
                //string SqlQuery = "SELECT ID,Server_A_Name,Server_A_Directory,Server_A_ExcludeList,Server_B_Name,Server_B_Directory,Server_B_ExcludeList" +
                //    ", Server_C_Name,Server_C_Directory,Server_C_ExcludeList,Missing_Replica_Alert,First_Alert_Threshold" +
                //    ",Enabled,Name,ScanInterval,OffHoursScanInterval,RetryInterval,Category FROM MailServices";

                MailServicesDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return MailServicesDataTable;
        }
        /// <summary>
        /// Get Data from MailServices based on Key
        /// </summary>
        public MailServices GetData(MailServices MSObject)
        {
            DataTable MailServicesDataTable = new DataTable();
            MailServices ReturnObject = new MailServices();
            try
            {
                //string SqlQuery = "Select * from MailServices where [key]=" + MSObject.key;
                string SqlQuery = "Select ms.*,t2.Location as LocationText from MailServices ms  INNER JOIN [Locations] t2 ON ms.[LocationId] = t2.[ID] where ms.[key]=" + MSObject.key.ToString();
              
                MailServicesDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                if (MailServicesDataTable.Rows.Count > 0)
                {
                    //ReturnObject.ID = int.Parse(MailServicesDataTable.Rows[0]["ServerID"].ToString());
                    //if(MailServicesDataTable.Rows[0]["ServerName"].ToString()!="")
                    //ReturnObject.ServerName = MailServicesDataTable.Rows[0]["ServerName"].ToString();
                    ReturnObject.Address = MailServicesDataTable.Rows[0]["Address"].ToString();
                    ReturnObject.Name = MailServicesDataTable.Rows[0]["Name"].ToString();
                    ReturnObject.Description = MailServicesDataTable.Rows[0]["Description"].ToString();
                    ReturnObject.Category = MailServicesDataTable.Rows[0]["Category"].ToString();
                    if (MailServicesDataTable.Rows[0]["Key"].ToString() != "")
                        ReturnObject.key = int.Parse(MailServicesDataTable.Rows[0]["Key"].ToString());
                    if (MailServicesDataTable.Rows[0]["ScanInterval"].ToString() != "")
                        ReturnObject.ScanInterval = int.Parse(MailServicesDataTable.Rows[0]["ScanInterval"].ToString());
                    if (MailServicesDataTable.Rows[0]["OffHoursScanInterval"].ToString() != "")
                        ReturnObject.OffHoursScanInterval = int.Parse(MailServicesDataTable.Rows[0]["OffHoursScanInterval"].ToString());
                    if (MailServicesDataTable.Rows[0]["NextScan"].ToString() != "")
                        ReturnObject.NextScan = DateTime.Parse(MailServicesDataTable.Rows[0]["NextScan"].ToString());
                    if (MailServicesDataTable.Rows[0]["LastChecked"].ToString() != "")
                        ReturnObject.LastChecked = DateTime.Parse(MailServicesDataTable.Rows[0]["LastChecked"].ToString());
                    ReturnObject.LastStatus = MailServicesDataTable.Rows[0]["LastStatus"].ToString();
                    if (MailServicesDataTable.Rows[0]["Enabled"].ToString() != "")
                        ReturnObject.Enabled = bool.Parse(MailServicesDataTable.Rows[0]["Enabled"].ToString());
                    if (MailServicesDataTable.Rows[0]["ResponseThreshold"].ToString() != "")
                        ReturnObject.ResponseThreshold = int.Parse(MailServicesDataTable.Rows[0]["ResponseThreshold"].ToString());
                    if (MailServicesDataTable.Rows[0]["RetryInterval"].ToString() != "")
                        ReturnObject.RetryInterval = int.Parse(MailServicesDataTable.Rows[0]["RetryInterval"].ToString());

                    ReturnObject.key = int.Parse(MailServicesDataTable.Rows[0]["key"].ToString());
                    if (MailServicesDataTable.Rows[0]["Port"].ToString() != "")
                        ReturnObject.Port = short.Parse(MailServicesDataTable.Rows[0]["Port"].ToString());
                    if (MailServicesDataTable.Rows[0]["FailureThreshold"].ToString() != "")
                        ReturnObject.FailureThreshold = short.Parse(MailServicesDataTable.Rows[0]["FailureThreshold"].ToString());

                    ReturnObject.LocationText = MailServicesDataTable.Rows[0]["LocationText"].ToString();

                }
                else
                {
                    ReturnObject.Status = "";
                }
             
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

        public bool InsertData(MailServices MSObject)
        {
            bool Insert = false;
            try
            {
                DataTable MailServicesDataTable = new DataTable();
                string SqlQuery = "select ID from servertypes where servertype='Mail'";
                MailServicesDataTable = objAdaptor.FetchData(SqlQuery);
                if (MailServicesDataTable.Rows.Count > 0)
                    MSObject.ServerTypeId = int.Parse(MailServicesDataTable.Rows[0][0].ToString());

                SqlQuery = "INSERT INTO MailServices (Address,Name,Description,Category,ScanInterval,OffHoursScanInterval" +
                   ",Enabled,ResponseThreshold,RetryInterval,Port,FailureThreshold,LocationId,ServerTypeId)" +
                   "VALUES('" + MSObject.Address + "','" + MSObject.Name +
                   "','" + MSObject.Description + "','" + MSObject.Category +
                   "'," + MSObject.ScanInterval + "," + MSObject.OffHoursScanInterval +
                   ",'" + MSObject.Enabled + "'," + MSObject.ResponseThreshold + "," + MSObject.RetryInterval +
                   "," + MSObject.Port + "," + MSObject.FailureThreshold + "," + MSObject.LocationId + "," + MSObject.ServerTypeId + ")";


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
        /// Update data into MailServices table
        /// </summary>
        /// <param name="DSObject">MailServices object</param>
        /// <returns></returns>
        public Object UpdateData(MailServices MSObject)
        {
            Object Update;
            try
            {
                //System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString());
                //System.Data.SqlClient.SqlCommand com = new System.Data.SqlClient.SqlCommand("select max(serverID) from MailServices", con);
                //con.Open();
                //object Maxserverid = com.ExecuteScalar();
                //string maxid=Maxserverid.ToString();
                //con.Close();
                //if (maxid != "")
                //{
                //    if (int.Parse(maxid) > MSObject.ID)
                //    {
                      string SqlQuery = "UPDATE MailServices SET Address='" + MSObject.Address + "',Name='" + MSObject.Name +
                            "',Description='" + MSObject.Description + "',Category='" + MSObject.Category + "',ScanInterval=" + MSObject.ScanInterval +
                            ",OffHoursScanInterval=" + MSObject.OffHoursScanInterval +
                            ",Enabled='" + MSObject.Enabled + "',ResponseThreshold=" + MSObject.ResponseThreshold +
                            ", RetryInterval=" + MSObject.RetryInterval + ",Port=" + MSObject.Port +",FailureThreshold=" + MSObject.FailureThreshold + ",LocationId=" + MSObject.LocationId + "  WHERE [Key] = " + MSObject.key + "";

                        Update = objAdaptor.ExecuteNonQuery(SqlQuery);

                    //}
                    //else
                    //{
                    //    string SqlQuery = "INSERT INTO MailServices (ServerID,Address,Name,Description,Category,ScanInterval,OffHoursScanInterval" +
                    //                           ",Enabled,ResponseThreshold,RetryInterval,Port,FailureThreshold)" +
                    //                           "VALUES(" + MSObject.ID + ",'" + MSObject.Address + "','" + MSObject.Name +
                    //                           "','" + MSObject.Description + "','" + MSObject.Category +
                    //                           "'," + MSObject.ScanInterval + "," + MSObject.OffHoursScanInterval +
                    //                           ",'" + MSObject.Enabled + "'," + MSObject.ResponseThreshold + "," + MSObject.RetryInterval +
                    //                           "," + MSObject.Port + "," + MSObject.FailureThreshold + ")";
                    //    Update = objAdaptor.ExecuteNonQuery(SqlQuery);
                    //}
                //}
                //else
                //{
                //    string SqlQuery = "INSERT INTO MailServices (ServerID,Address,Name,Description,Category,ScanInterval,OffHoursScanInterval" +
                //       ",Enabled,ResponseThreshold,RetryInterval,Port,FailureThreshold)" +
                //       "VALUES(" + MSObject.ID + ",'" + MSObject.Address + "','" + MSObject.Name +
                //       "','" + MSObject.Description + "','" + MSObject.Category +
                //       "'," + MSObject.ScanInterval + "," + MSObject.OffHoursScanInterval +
                //       ",'" + MSObject.Enabled + "'," + MSObject.ResponseThreshold + "," + MSObject.RetryInterval +
                //       "," + MSObject.Port + "," + MSObject.FailureThreshold + ")";
                //    Update = objAdaptor.ExecuteNonQuery(SqlQuery);
                //}
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

        //delete Data from MailServices Table

        public Object DeleteData(MailServices MSObject)
        {
            Object Update;
            try
            {
                if (MSObject.key.ToString() != "")
                {
                    string SqlQuery = "Delete MailServices Where [key]=" + MSObject.key;
                    
                    


                    Update = objAdaptor.ExecuteNonQuery(SqlQuery);
                }
                else
                {
                    Update = "";
                }
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

        public DataTable GetIPAddress(MailServices MailObj)
        {
            //SametimeServers SametimeObj = new SametimeServers();
            DataTable MailServiceTable = new DataTable();
            try
            {
                if (MailObj.key == 0)
                {
                    string sqlQuery = "Select * from MailServices where Name='" + MailObj.Name + "'";
                    MailServiceTable = objAdaptor.FetchData(sqlQuery);
                }
                else
                {
                    string sqlQuery = "Select * from MailServices where Name='" + MailObj.Name + "' and [key]<>"+MailObj.key+"";
                    MailServiceTable = objAdaptor.FetchData(sqlQuery);
                }

            }

            catch (Exception ex)
            { 

                throw ex;
            }
            return MailServiceTable;

        }

        public DataTable GetServer()
        {
            DataTable DTservers = new DataTable();
            try
            {
                string sqlQuery = "select servername,ID,LocationID from servers";
                DTservers = objAdaptor.FetchData(sqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return DTservers;

        }
        public int UpdateEXGServiceSettings(int Enabled, string DisplayName, string ServiceName, string ServerType, int SVRId)
        {
            int Update = 0;
            try
            {
                
                    string SQLQuery = "Update WindowsServices Set Monitored = '" + Enabled + "' Where DisplayName = '" + DisplayName + "' and Service_Name = '" + ServiceName + "' and ServerTypeID = (Select ID from ServerTypes where ServerType = '" + ServerType + "') and ServerName = (Select ServerName from Servers where ID = '"+SVRId+"') ";
                    Update = objAdaptor.ExecuteNonQueryRetRows(SQLQuery);
                    //string SqlQuery = "SELECT * FROM ServerServices where ServerId = " + serverID + " AND SVRId = " + SVRId;
                    //if (objAdaptor.FetchData(SqlQuery).Rows.Count > 0)
                    //{
                    //    string SqlQuery1 = "Delete from ServerServices WHERE ServerId = " + serverID + " AND SVRId = " + SVRId;
                    //    Update = objAdaptor.ExecuteNonQueryRetRows(SqlQuery1);
                    //}
                
            }
            catch
            {
                Update = 0;
            }
            finally
            {
            }
            return Update;
        }

        public Int32 GetServerIDbyServerName(string Name)
        {
            DataTable ServersDataTable = new DataTable();
            int ID = 0;
            try
            {
                string SqlQuery = "SELECT [key] FROM MailServices WHERE Name='" + Name + "'";
                ServersDataTable = objAdaptor.FetchData(SqlQuery);
                ID = Convert.ToInt32(ServersDataTable.Rows[0]["key"]);
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
    }
}
