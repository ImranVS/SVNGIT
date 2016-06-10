using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace VSWebDAL.ConfiguratorDAL
{
    public class ExchangePropertiesDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
		private static ExchangePropertiesDAL _self = new ExchangePropertiesDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
		public static ExchangePropertiesDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from DominoServers 
        /// </summary>
        /// <returns></returns>
		//public DataTable GetAllData()
		//{

		//    DataTable DominoServersDataTable = new DataTable();
		//    DominoServers ReturnDSObject = new DominoServers();
		//    try
		//    {

		//        //string Query="Select serverid,LocationID from UserLocationRestrictions inner join UserServerRestrictions on UserLocationRestrictions.UserID = UserServerRestrictions.UserID where UserID="++"";
		//        //12/16/2013 NS added sort by name ASC
		//        string SqlQuery = "SELECT DiskSpaceThreshold, BES_Server, BES_Threshold, DominoServers.Category, DeadThreshold, " +
		//            "FailureThreshold,DominoServers.Enabled,MailDirectory,ServerID,OffHoursScanInterval, PendingThreshold, " +
		//            "ResponseThreshold, DominoServers.RetryInterval, [Scan Interval], SearchString, AdvancedMailScan," +
		//            "DeadMailDeleteThreshold,HeldThreshold, ScanDBHealth, NotificationGroup, Memory_Threshold, " +
		//            "DominoServers.CPU_Threshold, Cluster_Rep_Delays_Threshold,description,servername as Name,ipaddress,locationid, " +
		//            "(select Location from Locations where ID = servers.LocationID) Location,ID from Servers " +
		//            "left join DominoServers on servers.ID=DominoServers.serverID where " +
		//            "servers.ServerTypeID=(select ID from ServerTypes where ServerType='Domino') " +
		//            "order by servername ";

		//        //string SqlQuery = "SELECT DiskSpaceThreshold, BES_Server, BES_Threshold, Category, DeadThreshold, Description, Enabled, FailureThreshold, [Key], Location, MailDirectory, Name, " +
		//        //                            "OffHoursScanInterval, PendingThreshold, ResponseThreshold, RetryInterval, [Scan Interval], SearchString, AdvancedMailScan, DeadMailDeleteThreshold, " +
		//        //               " IPAddress, HeldThreshold, ScanDBHealth, NotificationGroup, Memory_Threshold, CPU_Threshold, Cluster_Rep_Delays_Threshold  FROM DominoServers" +
		//        //               " union " +
		//        //                "SELECT '', '', '', '', '', Description, '', '', 0, '', '', ServerName as Name, " +
		//        //                "'', '', '', '', '', '', '', '', " +
		//        //                "IPAddress, '', '', '', '', '', '' FROM Servers where ServerName not in (SELECT distinct Name FROM DominoServers)";

		//        DominoServersDataTable = objAdaptor.FetchData(SqlQuery);

		//    }
		//    catch
		//    {
		//    }
		//    finally
		//    {
		//    }
		//    return DominoServersDataTable;
		//}


        /// <summary>
        /// Get Data from DominoServers based on Key
        /// </summary>
        /// <param name="DSObject">DominoServers object</param>
        /// <returns></returns>
		public ExchangeServers GetData(ExchangeServers DSObject)
        {
            DataTable DominoServersDataTable = new DataTable();
			ExchangeServers ReturnDSObject = new ExchangeServers();

            try
            {
				//string SqlQuery = "select *,ISNULL(RequireSSL,0) RS,ServerName as Name,IPAddress,LocationID,Description,servers.ID,Location from Servers left join dominoservers on Servers.ID=DominoServers.ServerID inner join Locations on servers.LocationID=Locations.ID where servers.ID=" + DSObject.Key + "";
                //8/7/2014 NS modified for VSPLUS-853
				string SqlQuery = "select ServerAttributes.Category, " +
					"Description,ISNULL(ServerAttributes.Enabled,0) Enabled,  " +
					"Location,ISNULL(LocationID,0) LocationID,ISNULL(ServerAttributes.OffHourInterval,15) OffHoursScanInterval, " +
					"ISNULL(ServerAttributes.ResponseTime,2500) ResponseThreshold, " +
					"ISNULL(ServerAttributes.RetryInterval,2) RetryInterval,ISNULL(ServerAttributes.[ScanInterval],8) [Scan Interval], " + 
                    "IPAddress, " +
					"ISNULL(ServerAttributes.MemThreshold,0.9) Memory_Threshold, " +
					"ISNULL(ServerAttributes.CPU_Threshold,0.9) CPU_Threshold, " + 
                    "ServerID,ServerName as Name, " +
                    "servers.ID,Location,ISNULL(CredentialsID,0) CredentialsID from Servers left join ServerAttributes on " +
					"Servers.ID=ServerAttributes.ServerID inner join Locations on servers.LocationID=Locations.ID " + 
                    "where servers.ID=" + DSObject.Key + "";

                DominoServersDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object

                if (DominoServersDataTable.Rows.Count > 0)
                {
					//if (DominoServersDataTable.Rows[0]["BES_Server"].ToString() != "" && DominoServersDataTable.Rows[0]["NotificationGroup"] != null && DominoServersDataTable.Rows[0]["OffHoursScanInterval"] != null)
					//{
                        //Commented by Mukund 06Jun14:Error this parameter is not taken now. Disk space is separately dealt
                        //ReturnDSObject.DiskSpaceThreshold = float.Parse(DominoServersDataTable.Rows[0]["DiskSpaceThreshold"].ToString());
                       
                        ReturnDSObject.Category = DominoServersDataTable.Rows[0]["Category"].ToString();
                       
                        ReturnDSObject.Name = DominoServersDataTable.Rows[0]["Description"].ToString();
                        ReturnDSObject.Enabled = bool.Parse(DominoServersDataTable.Rows[0]["Enabled"].ToString());
						
                        //ReturnDSObject.FailureThreshold = int.Parse(DominoServersDataTable.Rows[0]["FailureThreshold"].ToString());
                        ReturnDSObject.Location = DominoServersDataTable.Rows[0]["Location"].ToString();
                        ReturnDSObject.LocationID = int.Parse(DominoServersDataTable.Rows[0]["LocationID"].ToString());
                      
                        ReturnDSObject.Name = DominoServersDataTable.Rows[0]["Name"].ToString();
                        ReturnDSObject.OffHoursScanInterval = int.Parse(DominoServersDataTable.Rows[0]["OffHoursScanInterval"].ToString());
                        
                        ReturnDSObject.ResponseThreshold = int.Parse(DominoServersDataTable.Rows[0]["ResponseThreshold"].ToString());
                        ReturnDSObject.RetryInterval = int.Parse(DominoServersDataTable.Rows[0]["RetryInterval"].ToString());
                        ReturnDSObject.ScanInterval = int.Parse(DominoServersDataTable.Rows[0]["Scan Interval"].ToString());
                        ReturnDSObject.SearchString = DominoServersDataTable.Rows[0]["Scan Interval"].ToString();
                        
                        ReturnDSObject.IPAddress = DominoServersDataTable.Rows[0]["IPAddress"].ToString();
                        
                        ReturnDSObject.Memory_Threshold = float.Parse(DominoServersDataTable.Rows[0]["Memory_Threshold"].ToString());
                        ReturnDSObject.CPU_Threshold = float.Parse(DominoServersDataTable.Rows[0]["CPU_Threshold"].ToString());
                        
						if (DominoServersDataTable.Rows[0]["ServerID"].ToString() != "")
                        ReturnDSObject.ServerID = int.Parse(DominoServersDataTable.Rows[0]["ServerID"].ToString());
                        ReturnDSObject.Key = Convert.ToInt16(DominoServersDataTable.Rows[0]["ID"].ToString());

                        
                        //ReturnDSObject.Modified_By = Convert.ToInt16(DominoServersDataTable.Rows[0]["Modified_By"].ToString());
                        //ReturnDSObject.Modified_On = DominoServersDataTable.Rows[0]["Modified_On"].ToString();
                        //8/7/2014 NS added for VSPLUS-853
                        ReturnDSObject.CredentialsID = int.Parse(DominoServersDataTable.Rows[0]["CredentialsID"].ToString());
                    }
                    else
                    {
                        ReturnDSObject.Location = DominoServersDataTable.Rows[0]["Location"].ToString();
                        ReturnDSObject.Name = DominoServersDataTable.Rows[0]["Name"].ToString();
                        ReturnDSObject.IPAddress = DominoServersDataTable.Rows[0]["IPAddress"].ToString();
                        ReturnDSObject.Description = DominoServersDataTable.Rows[0]["Description"].ToString();
                        ReturnDSObject.Key = Convert.ToInt16(DominoServersDataTable.Rows[0]["ID"].ToString());
                        ReturnDSObject.LocationID = int.Parse(DominoServersDataTable.Rows[0]["LocationID"].ToString());
                    }

                }
            //}

			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return ReturnDSObject;
        }

        /// <summary>
        /// Insert data into DominoServers table
        /// </summary>
        /// <param name="DSObject">DominoServers object</param>
        /// <returns></returns>
        public bool InsertData(ExchangeServers DSObject)
        {
            bool Insert = false;
            try
            {
                //8/7/2014 NS modified for VSPLUS-853
                string SqlQuery = "INSERT INTO DominoServers (DiskSpaceThreshold, BES_Server, BES_Threshold, Category, DeadThreshold, " +
                    "Description, Enabled, RequireSSL,ExternalAlias, FailureThreshold, Location, MailDirectory, Name, OffHoursScanInterval, PendingThreshold, " +
                    "ResponseThreshold, RetryInterval, [Scan Interval], SearchString, AdvancedMailScan, DeadMailDeleteThreshold, " +
                    "IPAddress, HeldThreshold, ScanDBHealth, NotificationGroup, Memory_Threshold, CPU_Threshold, " + 
                    "Cluster_Rep_Delays_Threshold,Modified_By,Modified_On,ServerDaysAlert,CredentialsID) " +
                  " VALUES (" + DSObject.DiskSpaceThreshold + ", '" + DSObject.BES_Server +
                  "', " + DSObject.BES_Threshold + ", '" + DSObject.Category +
                  "', " + DSObject.DeadThreshold +
                  "', '" + DSObject.Enabled + "','" + DSObject.RequireSSL + "','" + DSObject.ExternalAlias + "', " + DSObject.FailureThreshold +
                  ", '" + DSObject.LocationID + "', '" + DSObject.MailDirectory +
                  "',  " + DSObject.OffHoursScanInterval +
                  ", " + DSObject.PendingThreshold + ", " + DSObject.ResponseThreshold +
                  ", " + DSObject.RetryInterval + ", " + DSObject.ScanInterval +
                  ", '" + DSObject.SearchString + "', 0, " + DSObject.DeadMailDeleteThreshold +
                  ",  " + DSObject.HeldThreshold +
                  ", '" + DSObject.ScanDBHealth + "', '" + DSObject.NotificationGroup +
                  "', " + DSObject.Memory_Threshold + ", " + DSObject.CPU_Threshold +
                  ", " + DSObject.Cluster_Rep_Delays_Threshold +
                ", " + DSObject.ServerDaysAlert + 
                ", " + DSObject.CredentialsID + 
                ")";


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
        /// Insert data into DominoServers table
        /// </summary>
        /// <param name="DSObject">DominoServers object</param>
        /// <returns></returns>
        public Object UpdateData(ExchangeServers DSObject)
        {
            Object Update;

            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString());
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter("select serverID from ServerAttributes", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToInt32(dr["serverID"].ToString()) == DSObject.Key)
                    {
                        i++;
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    if (i > 0)
                    {
                        //8/7/2014 NS modified for VSPLUS-853
                        string SqlQuery = "UPDATE ServerAttributes SET " +
                            ", Category = '" + DSObject.Category + "' " +
                             ", Enabled = '" + DSObject.Enabled +
                            "', RetryInterval = " + DSObject.RetryInterval + ", [ScanInterval] = " + DSObject.ScanInterval +
                            "', MemThreshold = " + DSObject.Memory_Threshold + ", CPU_Threshold = " + DSObject.CPU_Threshold +
                            (DSObject.CredentialsID!=0? ", CredentialsID = " + DSObject.CredentialsID:"") + 
                           " WHERE ServerID =" + DSObject.Key + "";
                        
                        //VSPLUS-896: Mukund 28Aug14,In sqlQuery, Credentials is giving FK constraint err. Adding that if selected in UI.

                        Update = objAdaptor.ExecuteNonQuery(SqlQuery);
                    }
                    else
                    {//11-04-2016 Durga Modified for VSPLUS-2742
                        string SqlQuery = "INSERT INTO ServerAttributes (ServerID, Category, " +
                   "Enabled, OffHourInterval, " +
                   "ResponseTime, RetryInterval, [ScanInterval]" +
				   ", MemThreshold, CPU_Threshold, CredentialsId,ConsFailuresBefAlert,ConsOvrThresholdBefAlert) " +
                 " VALUES (" + DSObject.Key  +
                 ", '" + DSObject.Category +
                 "', '" + DSObject.Enabled + "',"+      DSObject.OffHoursScanInterval + ", " + DSObject.ResponseThreshold +
                 ", " + DSObject.RetryInterval + ", " + DSObject.ScanInterval +
                  ", " + DSObject.Memory_Threshold + ", " + DSObject.CPU_Threshold + "," +DSObject.CredentialsID.ToString() +"," + "0" + "," + "0" +")";
                        Update = objAdaptor.ExecuteNonQuery(SqlQuery);
                        SqlQuery = "INSERT INTO ExchangeSettings (ServerID, CasSmtp, CASpop3,CASimap,CASOWA,CASActiveSync,CASEWS,CASECP,CASAutoDiscovery,CASOAB,SUBQTHRESHOLD,POISONQTHRESHOLD,UNREACHABLEQTHRESHOLD,TOTALQTHRESHOLD,VersionNo,ShadowQThreshold) " +
					" VALUES (" + DSObject.Key + ",0,0,0,0,0,0,0,0,0,100,100,100,2,0,100)";
						Update = objAdaptor.ExecuteNonQuery(SqlQuery);
                    }
                }
                else
                {//11-04-2016 Durga Modified for VSPLUS-2742
					string SqlQuery = "INSERT INTO ServerAttributes (ServerID, Category, " +
				  "Enabled, OffHourInterval, " +
				  "ResponseTime, RetryInterval, [ScanInterval]" +
				  ", MemThreshold, CPU_Threshold,CredentialsId,ConsFailuresBefAlert,ConsOvrThresholdBefAlert) " +
				" VALUES (" + DSObject.Key +
				", '" + DSObject.Category +
				"', '" + DSObject.Enabled + "'," + DSObject.OffHoursScanInterval + ", " + DSObject.ResponseThreshold +
				", " + DSObject.RetryInterval + ", " + DSObject.ScanInterval +
				 ", " + DSObject.Memory_Threshold + ", " + DSObject.CPU_Threshold + "," + DSObject.CredentialsID.ToString() + "," + "0" + "," + "0" + ")";
					Update = objAdaptor.ExecuteNonQuery(SqlQuery);

                    SqlQuery = "INSERT INTO ExchangeSettings (ServerID, CasSmtp, CASpop3,CASimap,CASOWA,CASActiveSync,CASEWS,CASECP,CASAutoDiscovery,CASOAB,SUBQTHRESHOLD,POISONQTHRESHOLD,UNREACHABLEQTHRESHOLD,TOTALQTHRESHOLD,VersionNo,ShadowQThreshold) " +
						" VALUES (" + DSObject.Key + ",0,0,0,0,0,0,0,0,0,100,100,100,2,0,100)";
					Update = objAdaptor.ExecuteNonQuery(SqlQuery);

                }
            }
            catch (Exception ex)
            {
                Update = false;
                //7/29/2014 NS added for VSPLUS-634
                //WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
            }
            finally
            {
            }
            return Update;
        }
		public Object UpdateDAGData(ExchangeServers DSObject)
		{
			Object Update;

			try
			{
				System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString());
				System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter("select serverID from ServerAttributes", con);
				DataTable dt = new DataTable();
				da.Fill(dt);
				int i = 0;
				foreach (DataRow dr in dt.Rows)
				{
					if (Convert.ToInt32(dr["serverID"].ToString()) == DSObject.Key)
					{
						i++;
					}
				}
				if (dt.Rows.Count > 0)
				{
					if (i > 0)
					{
						//8/7/2014 NS modified for VSPLUS-853
						string SqlQuery = "UPDATE ServerAttributes SET " +
							", Category = '" + DSObject.Category + "' " +
							 ", Enabled = '" + DSObject.Enabled +
							"', RetryInterval = " + DSObject.RetryInterval + ", [ScanInterval] = " + DSObject.ScanInterval +
							"', MemThreshold = " + DSObject.Memory_Threshold + ", CPU_Threshold = " + DSObject.CPU_Threshold +
							(DSObject.CredentialsID != 0 ? ", CredentialsID = " + DSObject.CredentialsID : "") +
						   " WHERE ServerID =" + DSObject.Key + "";

						//VSPLUS-896: Mukund 28Aug14,In sqlQuery, Credentials is giving FK constraint err. Adding that if selected in UI.

						Update = objAdaptor.ExecuteNonQuery(SqlQuery);
					}
					else
					{
						string SqlQuery = "INSERT INTO ServerAttributes (ServerID, Category, " +
				   "Enabled, OffHourInterval, " +
				   "ResponseTime, RetryInterval, [ScanInterval]" +
				   ", MemThreshold, CPU_Threshold, CredentialsId,ConsFailuresBefAlert,ConsOvrThresholdBefAlert) " +
				 " VALUES (" + DSObject.Key +
				 ", '" + DSObject.Category +
				 "', '" + DSObject.Enabled + "'," + DSObject.OffHoursScanInterval + ", " + DSObject.ResponseThreshold +
				 ", " + DSObject.RetryInterval + ", " + DSObject.ScanInterval +
				  ", " + DSObject.Memory_Threshold + ", " + DSObject.CPU_Threshold + "," + DSObject.CredentialsID.ToString() + "," + "0" + "," + "0" + ")";
						Update = objAdaptor.ExecuteNonQuery(SqlQuery);
						SqlQuery = "INSERT INTO DAGSettings (ServerID, PrimaryConnection, BackupConnection,ReplyQThreshold,CopyQThreshold) " +
						" VALUES (" + DSObject.Key + "," + DSObject.DAGPrimaryServerId.ToString() + "," + DSObject.DAGBackUpServerID.ToString() + "," + DSObject.DAGResponseQTh.ToString() + "," + DSObject.DAGCopyQTh.ToString() + ")";
						Update = objAdaptor.ExecuteNonQuery(SqlQuery);
					}
				}
				else
				{
					string SqlQuery = "INSERT INTO ServerAttributes (ServerID, Category, " +
				  "Enabled, OffHourInterval, " +
				  "ResponseTime, RetryInterval, [ScanInterval]" +
				  ",ConsFailuresBefAlert,ConsOvrThresholdBefAlert) " +
				" VALUES (" + DSObject.Key +
				", '" + DSObject.Category +
				"', '" + DSObject.Enabled + "'," + DSObject.OffHoursScanInterval + ", " + DSObject.ResponseThreshold +
				", " + DSObject.RetryInterval + ", " + DSObject.ScanInterval +
				 "," + "0" + "," + "0" + ")";
					Update = objAdaptor.ExecuteNonQuery(SqlQuery);

					SqlQuery = "INSERT INTO DAGSettings (ServerID, PrimaryConnection, BackupConnection,ReplyQThreshold,CopyQThreshold) " +
						" VALUES (" + DSObject.Key + "," + DSObject.DAGPrimaryServerId.ToString() + "," + DSObject.DAGBackUpServerID.ToString() +"," + DSObject.DAGResponseQTh.ToString() + "," + DSObject.DAGCopyQTh.ToString() +")";
					Update = objAdaptor.ExecuteNonQuery(SqlQuery);

				}
			}
			catch (Exception ex)
			{
				Update = false;
				//7/29/2014 NS added for VSPLUS-634
				//WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
			}
			finally
			{
			}
			return Update;
		}


        /// <summary>
        /// Get Data from DominoServers & ServerTaskSettings based on ServerID
        /// </summary>
        /// <param name="ServerKey"></param>
        /// <returns></returns>
        public DataTable DSTaskSettingsUpdateGrid(string ServerKey)
        {
            DataTable DSTaskSettingsServersDataTable = new DataTable();
            try
            {
                string SqlQuery = " Select sts.Enabled, sts.SendLoadCommand, sts.SendRestartCommand, sts.RestartOffHours, " +
                    "sts.SendExitCommand, dst.TaskName, sts.MyID  FROM " +
                    "ServerTaskSettings sts INNER JOIN DominoServerTasks dst ON sts.TaskID = dst.TaskID " +
                    "Where sts.ServerID = " + ServerKey;

                DSTaskSettingsServersDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return DSTaskSettingsServersDataTable;
        }

        public DataTable DSTaskSettingsUpdategridFirstTime(string ServerKey)
        {
            DataTable DSTaskSettingsServersDataTable = new DataTable();
            try
            {
                string SqlQuery = "Select *,'' as TaskName,'' as Enabled from ServerTaskSettings where ServerID=" + ServerKey;

                DSTaskSettingsServersDataTable = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return DSTaskSettingsServersDataTable;
        }

        public DataTable GetRestrictedServers(int ID)
        {
            DataTable RescServers = new DataTable();
            try
            {
                string Query = "Select serverid,locationID from UserServerRestrictions left join UserLocationRestrictions on UserServerRestrictions.UserID=UserLocationRestrictions.UserID where UserServerRestrictions.userID=" + ID + "";
                RescServers = objAdaptor.FetchData(Query);
                if (RescServers.Rows.Count == 0)
                {
                    string Query1 = "Select serverid,locationID from UserServerRestrictions right join UserLocationRestrictions on UserServerRestrictions.UserID=UserLocationRestrictions.UserID where UserLocationRestrictions.userID=" + ID + "";
                    RescServers = objAdaptor.FetchData(Query1);

                }


            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return RescServers;
        }

		//public DataTable GetDiskSettings(string Server, string strAll)
		//{
		//    DataTable RescServers = new DataTable();
		//    try
		//    {
		//        string Query = "";
		//        if (strAll == "All")
		//        {
		//            //5/1/2014 NS modified for VSPLUS-602,VSPLUS-616
		//            //Query = "SELECT dom.ServerName,dom.DiskName,ds.Threshold,0 as isSelected ,ds.id FROM DominoDiskSpace dom left outer join DominoDiskSettings ds on dom.ServerName=ds.ServerName and dom.DiskName=ds.DiskName where dom.servername='" + Server + "'";
		//            Query = "SELECT dom.ServerName,dom.DiskName,ROUND(dom.DiskFree,1) DiskFree,ROUND(dom.DiskSize,1) DiskSize," +
		//                "ROUND(dom.PercentFree*100,1) PercentFree,ds.Threshold,ds.ThresholdType,0 as isSelected ,ds.id " +
		//                "FROM DominoDiskSpace dom LEFT OUTER JOIN DominoDiskSettings ds ON dom.ServerName=ds.ServerName " +
		//                "WHERE dom.servername='" + Server + "'";

		//        }
		//        else
		//        {
		//            //5/1/2014 NS modified for VSPLUS-602,VSPLUS-616
		//            //Query = "SELECT dom.ServerName,dom.DiskName,ds.Threshold,case when isnull(ds.ServerName,'0')='0' then 0 else 1 end as isSelected ,ds.id FROM DominoDiskSpace dom left outer join DominoDiskSettings ds on dom.ServerName=ds.ServerName and dom.DiskName=ds.DiskName where dom.servername='" + Server + "'";
		//            Query = "SELECT dom.ServerName,dom.DiskName,ROUND(dom.DiskFree,1) DiskFree,ROUND(dom.DiskSize,1) DiskSize," +
		//                "ROUND(dom.PercentFree*100,1) PercentFree,ds.Threshold,ds.ThresholdType," +
		//                "case when isnull(ds.ServerName,'0')='0' then 0 else 1 end as isSelected , ds.id " +
		//                "FROM DominoDiskSpace dom LEFT OUTER JOIN DominoDiskSettings ds ON dom.ServerName=ds.ServerName AND " +
		//                "dom.DiskName=ds.DiskName WHERE dom.servername='" + Server + "'";

		//        }
		//        RescServers = objAdaptor.FetchData(Query);

		//    }
		//    catch
		//    {

		//    }
		//    finally
		//    {
		//    }
		//    return RescServers;
		//}
		//public DataTable GetRowsDiskSettings(string Server)
		//{
		//    DataTable RescServers = new DataTable();
		//    try
		//    {
		//        //5/13/2014 NS modified for VSPLUS-616
		//        //string Query = "SELECT * from DominoDiskSettings where ServerName='" + Server + "'"; ;
		//        string Query = "SELECT dds.ID ID,dds.ServerName ServerName,dds.DiskName DiskName,dds.Threshold Threshold,dds.ThresholdType ThresholdType," +
		//            "dds1.DiskSize DiskSize,dds1.DiskFree DiskFree,ROUND(PercentFree*100,1) PercentFree from DominoDiskSettings dds " +
		//            "left outer join dominodiskspace dds1 on dds.ServerName=dds1.ServerName and dds.DiskName=dds1.DiskName " +
		//            "where dds.ServerName='" + Server + "'";
		//        RescServers = objAdaptor.FetchData(Query);

		//    }
		//    catch
		//    {

		//    }
		//    finally
		//    {
		//    }
		//    return RescServers;
		//}

        //Mukund 27Jun14; VSPLUS-724
        public bool InsertDiskSettingsData(DataTable dtDisk, int enabled)
        {
            bool Insert = false;
            try
            {
                //VE-23 24-Jun-14, Mukund added two parts, one for non Domino servers ie DiskSettings and other for  Domino servers ie DominoDiskSettings  
                    for (int i = 0; i < dtDisk.Rows.Count; i++)
                    {
						string SqlQuery1 = "select sr.id, st.servertype from servers sr, servertypes st where sr.ServerTypeID=st.id and sr.servername='" + dtDisk.Rows[i][0].ToString() + "'";
						DataTable dtservertype = objAdaptor.FetchData(SqlQuery1);
						string SqlQuery0 = "delete DiskSettings where ServerID=" + dtservertype.Rows[0]["id"].ToString();
						Insert = objAdaptor.ExecuteNonQuery(SqlQuery0);

                        string SqlQuery2 = "insert into DiskSettings(ServerID,DiskName,Threshold,ThresholdType) " +
                                        "values(" + dtservertype.Rows[0]["id"].ToString() + ",'" + dtDisk.Rows[i][1].ToString() + "'," +
                                        dtDisk.Rows[i][2].ToString() + ",'" + dtDisk.Rows[i][3].ToString() + "')";
                        Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);
                    }

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

        //Mukund 27Jun14; VSPLUS-745
	  //  public bool InsertDiskSettingsDataSSE(DataTable dtDisk, int enabled)
	  //  {
	  //      bool Insert = false;
	  //      try
	  //      {
	  //          //VE-23 24-Jun-14, Mukund added two parts, one for non Domino servers ie DiskSettings and other for  Domino servers ie DominoDiskSettings  
	  //          string SqlQuery1 = "select sr.id, st.servertype from servers sr, servertypes st where sr.ServerTypeID=st.id and sr.servername='" + dtDisk.Rows[0][0].ToString() + "'";
	  //          DataTable dtservertype = objAdaptor.FetchData(SqlQuery1);
	  //          if (dtservertype.Rows[0]["servertype"].ToString() != "Domino")// Non Domino servers
	  //          {
	  //              //VSPLUS-745, 20Jun14 Mukund: Server Settings Editor - Udate Selected Disks should leave other disk settings alone
	  //              if (dtDisk.Rows[0][1].ToString() == "AllDisks" || dtDisk.Rows[0][1].ToString() == "NoAlerts")
	  //              {
	  //                  string SqlQuery0 = "delete DiskSettings where ServerID=" + dtservertype.Rows[0]["id"].ToString();
	  //                  Insert = objAdaptor.ExecuteNonQuery(SqlQuery0);
	  //              }
	  //              for (int i = 0; i < dtDisk.Rows.Count; i++)
	  //              {

	  //                  string sqlselect = "select * from DiskSettings where ServerID=" + dtservertype.Rows[0]["id"].ToString() + " and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";
	  //                  DataTable dtdiskcheck = objAdaptor.FetchData(sqlselect);
	  //                  if (dtdiskcheck.Rows.Count == 0)
	  //                  {
	  //                      string SqlQuery2 = "insert into DiskSettings(ServerID,DiskName,Threshold,ThresholdType) " +
	  //                                       "values(" + dtservertype.Rows[0]["id"].ToString() + ",'" + dtDisk.Rows[i][1].ToString() + "'," +
	  //                                       dtDisk.Rows[i][2].ToString() + ",'" + dtDisk.Rows[i][3].ToString() + "')";
	  //                      Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);
	  //                  }
	  //                  else
	  //                  {
	  //                      string SqlUpdatediskdata = "Update DiskSettings set Threshold ='" + dtDisk.Rows[i][2].ToString() + "'," +
	  //                               "ThresholdType ='" + dtDisk.Rows[i][3].ToString() + "' where ServerID='" + dtservertype.Rows[0]["id"].ToString() +
	  //                               "' and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";
	  //                      Insert = objAdaptor.ExecuteNonQuery(SqlUpdatediskdata);

	  //                  }
	  //              }
	  //          }

	  //      }
	  //      catch
	  //      {
	  //          Insert = false;
	  //      }
	  //      finally
	  //      {
	  //      }
	  //      return Insert;
	  //  }

	  ////  Mukund 12Jun14:  VE-4	: Implement Disk Checking - Front End  

	  //  public DataTable GetSrvRowsDiskSettings(string ServerID)
	  //  {
	  //      DataTable RescServers = new DataTable();
	  //      try
	  //      {
	  //          string Query = "SELECT dds.ID ID,dds.DiskName DiskName,dds.Threshold Threshold,dds.ThresholdType ThresholdType," +
	  //              "dds1.DiskSize DiskSize,dds1.DiskFree DiskFree,ROUND(PercentFree*100,1) PercentFree from DiskSettings dds " +
	  //              "left outer join diskspace dds1 on dds1.ServerName=(select ServerName from Servers where ID=" + ServerID + ") and dds.DiskName=dds1.DiskName " +
	  //              "where dds.ServerID=" + ServerID + "";
	  //          RescServers = objAdaptor.FetchData(Query);

	  //      }
	  //      catch
	  //      {

	  //      }
	  //      finally
	  //      {
	  //      }
	  //      return RescServers;
	  //  }
	  //  public DataTable GetSrvDiskSettings(string Server, string strAll)
	  //  {
	  //      DataTable RescServers = new DataTable();
	  //      try
	  //      {
	  //          string Query = "";
	  //          if (strAll == "All")
	  //          {
	  //              //5/1/2014 NS modified for VSPLUS-602,VSPLUS-616

	  //              Query = "SELECT dom.ServerName,dom.DiskName,ROUND(dom.DiskFree,1) DiskFree,ROUND(dom.DiskSize,1) DiskSize," +
	  //                  "ROUND(dom.PercentFree*100,1) PercentFree,ds.Threshold,ds.ThresholdType,0 as isSelected ,ds.id " +
	  //                  "FROM  dbo.DiskSettings AS ds INNER JOIN " +
	  //                   " dbo.Servers AS sr ON ds.ServerID = sr.ID RIGHT OUTER JOIN" +
	  //                   " dbo.DiskSpace AS dom ON sr.ServerName = dom.ServerName  and ds.DiskName=dom.DiskName " +
	  //                  "WHERE dom.servername='" + Server + "'";

	  //          }
	  //          else
	  //          {
	  //              //5/1/2014 NS modified for VSPLUS-602,VSPLUS-616
	  //              Query = "SELECT dom.ServerName,dom.DiskName,ROUND(dom.DiskFree,1) DiskFree,ROUND(dom.DiskSize,1) DiskSize," +
	  //                  "ROUND(dom.PercentFree*100,1) PercentFree,ds.Threshold,ds.ThresholdType," +
	  //                  "case when isnull(ds.ServerID,0)=0 then 0 else 1 end as isSelected , ds.id " +
	  //                 "FROM  dbo.DiskSettings AS ds INNER JOIN " +
	  //                       " dbo.Servers AS sr ON ds.ServerID = sr.ID RIGHT OUTER JOIN" +
	  //                       " dbo.DiskSpace AS dom ON sr.ServerName = dom.ServerName  and ds.DiskName=dom.DiskName " +
	  //                      "WHERE dom.servername='" + Server + "'";

	  //          }
	  //          RescServers = objAdaptor.FetchData(Query);

	  //      }
	  //      catch
	  //      {

	  //      }
	  //      finally
	  //      {
	  //      }
	  //      return RescServers;
	  //  }

	  //  public bool InsertSrvDiskSettingsData(DataTable dtDisk, int enabled)
	  //  {
	  //      bool Insert = false;
	  //      bool diskSettings = true;
	  //      try
	  //      {
	  //          string SqlQuery = "delete DiskSettings where ServerID=" + dtDisk.Rows[0][4].ToString() + "";
	  //          Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
               
           
	  //              for (int i = 0; i < dtDisk.Rows.Count; i++)
	  //              {
                            
	  //                              string SqlQuery2 = "insert into DiskSettings(ServerID,DiskName,Threshold,ThresholdType) " +
	  //                      "values(" + dtDisk.Rows[i][4].ToString() + ",'" + dtDisk.Rows[i][1].ToString() + "'," +
	  //                      dtDisk.Rows[i][2].ToString() + ",'" + dtDisk.Rows[i][3].ToString() + "')";
	  //                              Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);
                               
	  //              }



	  //              //string SqlQuery = "delete DiskSettings where ServerID=" + dtDisk.Rows[0][4].ToString() + "";
	  //              //string queryServerDisks = "Select DiskName from DiskSpace where ServerName='" + dtDisk.Rows[0][0].ToString() + "'";
	  //              //if (dtDisk.Rows[0][1].ToString() == "AllDisks" || dtDisk.Rows[0][1].ToString() == "NoAlerts")
	  //              //{
	  //              //    Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
	  //              //    diskSettings = false;
	  //              //}
	  //              //for (int i = 0; i < dtDisk.Rows.Count; i++)
	  //              //{
	  //              //    if (enabled == 0)
	  //              //    {
	  //              //        string sqlDelete = "Delete from DiskSettings where ServerID=" + dtDisk.Rows[0][4].ToString() + " and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";

	  //              //        if (objAdaptor.ExecuteNonQueryRetRows(sqlDelete) >= 0)
	  //              //        {
	  //              //            Insert = true;
	  //              //        }
	  //              //        else
	  //              //        {
	  //              //            Insert = false;
	  //              //        }
	  //              //    }
	  //              //    else
	  //              //    {
	  //              //        // 5/14/2014 - CY modified for VS-545
	  //              //        if (diskSettings)
	  //              //        {
	  //              //            string queryAllNoDisks = "Select DiskName,Threshold,ThresholdType from DiskSettings where ServerID=" + dtDisk.Rows[0][4].ToString() + "";
	  //              //            DataTable dtAllNoDisks = objAdaptor.FetchData(queryAllNoDisks);
	  //              //            if (dtAllNoDisks.Rows.Count == 1 && (dtAllNoDisks.Rows[0]["DiskName"].ToString() == "AllDisks" || dtAllNoDisks.Rows[0]["DiskName"].ToString() == "NoAlerts"))
	  //              //            {
	  //              //                //Delete Alldisks/diskname entry and re-insert data for each disk
	  //              //                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);

	  //              //                DataTable dtServerDisks = objAdaptor.FetchData(queryServerDisks);
	  //              //                foreach (DataRow dr in dtServerDisks.Rows)
	  //              //                {
	  //              //                    string SqlUpdatediskSettings = "insert into DiskSettings(ServerID,DiskName,Threshold,ThresholdType) " +
	  //              //                        "values(" + dtDisk.Rows[0][4].ToString() + ",'" + dr["DiskName"].ToString() + "'," +
	  //              //                            dtAllNoDisks.Rows[0]["Threshold"].ToString() + ",'" + dtAllNoDisks.Rows[0]["ThresholdType"].ToString() + "')";
	  //              //                    Insert = objAdaptor.ExecuteNonQuery(SqlUpdatediskSettings);
	  //              //                }
	  //              //                //Now Update the disk data
	  //              //                string SqlUpdatediskdata = "Update DiskSettings set Threshold ='" + dtDisk.Rows[i][2].ToString() + "'," +
	  //              //                    "ThresholdType ='" + dtDisk.Rows[i][3].ToString() + "' where ServerID=" + dtDisk.Rows[0][4].ToString() + " and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";
	  //              //                Insert = objAdaptor.ExecuteNonQuery(SqlUpdatediskdata);
	  //              //            }
	  //              //            else
	  //              //            {
	  //              //                string SqlQuery1 = "";
	  //              //                DataTable dt;
	  //              //                //SqlQuery1 = "Select DiskName from DiskSpace where ServerName='" + dtDisk.Rows[0][0].ToString() + "'";// and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";
	  //              //                dt = objAdaptor.FetchData(queryServerDisks);
	  //              //                foreach (DataRow dr in dt.Rows)
	  //              //                {
	  //              //                    //Check for the entry of each disk
	  //              //                    if (dr["DiskName"].ToString() == dtDisk.Rows[i][1].ToString())
	  //              //                    {
	  //              //                        //Update when record exists else insert
	  //              //                        string querysingleDisk = "Select DiskName,Threshold,ThresholdType from DiskSettings where ServerID=" + dtDisk.Rows[0][4].ToString() + " and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";
	  //              //                        DataTable dtsingleDisk = objAdaptor.FetchData(querysingleDisk);
	  //              //                        if (dtsingleDisk.Rows.Count == 1)
	  //              //                        {
	  //              //                            string SqlQuery2 = "Update DiskSettings set Threshold ='" + dtDisk.Rows[i][2].ToString() + "'," +
	  //              //                    "ThresholdType ='" + dtDisk.Rows[i][3].ToString() + "' where ServerID=" + dtDisk.Rows[0][4].ToString() + " and DiskName='" + dtDisk.Rows[i][1].ToString() + "'"; ;
	  //              //                            Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);
	  //              //                        }
	  //              //                        else
	  //              //                        {
	  //              //                            string SqlQuery2 = "insert into DiskSettings(ServerID,DiskName,Threshold,ThresholdType) " +
	  //              //                    "values(" + dtDisk.Rows[i][4].ToString() + ",'" + dtDisk.Rows[i][1].ToString() + "'," +
	  //              //                    dtDisk.Rows[i][2].ToString() + ",'" + dtDisk.Rows[i][3].ToString() + "')";
	  //              //                            Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);
	  //              //                        }

	  //              //                    }
	  //              //                }
	  //              //            }

	  //              //        }
	  //              //        else
	  //              //        {
	  //              //            //5/1/2014 NS modified for VSPLUS-602
	  //              //            //SqlQuery = "insert into DiskSettings(ServerID,DiskName,Threshold) values('" + dtDisk.Rows[i][0].ToString() + "','" + dtDisk.Rows[i][1].ToString() + "'," + dtDisk.Rows[i][2].ToString() + ")";
	  //              //            string SqlQuery1 = "insert into DiskSettings(ServerID,DiskName,Threshold,ThresholdType) " +
	  //              //                "values(" + dtDisk.Rows[i][4].ToString() + ",'" + dtDisk.Rows[i][1].ToString() + "'," +
	  //              //                dtDisk.Rows[i][2].ToString() + ",'" + dtDisk.Rows[i][3].ToString() + "')";
	  //              //            Insert = objAdaptor.ExecuteNonQuery(SqlQuery1);
	  //              //        }
	  //              //    }
	  //          //}

	  //      }
	  //      catch
	  //      {
	  //          Insert = false;
	  //      }
	  //      finally
	  //      {
	  //      }
	  //      return Insert;
	  //  }

       
	  //  //7/29/2014 NS added for VSPLUS-634
	  //  public void WriteHistoryEntry(string strMsg)
	  //  {
	  //      bool appendMode = true;

	  //      string ServiceLogDestination = VSWebDAL.SettingDAL.SettingsDAL.Ins.Getvalue("Log Files Path"); //VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Log Files Path");
	  //      ServiceLogDestination += "VSWebLogs.txt";
	  //      try
	  //      {
	  //          StreamWriter sw = new StreamWriter(ServiceLogDestination, appendMode, System.Text.Encoding.Unicode);
	  //          sw.WriteLine(strMsg);
	  //          sw.Close();
	  //          sw = null;
	  //      }
	  //      catch
	  //      {
	  //          //7/8/2014 NS added
	  //          ServiceLogDestination = System.Web.HttpContext.Current.Server.MapPath("~") + "\\VSWebLogs.txt";
	  //          try
	  //          {
	  //              StreamWriter sw = new StreamWriter(ServiceLogDestination, appendMode, System.Text.Encoding.Unicode);
	  //              sw.WriteLine(strMsg);
	  //              sw.Close();
	  //              sw = null;
	  //          }
	  //          catch (Exception ex)
	  //          {
	  //              throw ex;
	  //          }
	  //          finally
	  //          {
	  //              GC.Collect();
	  //          }
	  //      }
	  //      finally
	  //      {
	  //          GC.Collect();
	  //      }
	  //  }



	  //  public bool DeleteAllRecordsfromDiskSettingsDAL(string Servername)
	  //  {
	  //      try
	  //      {

	  //          string SqlQuery = "delete from DominoDiskSettings where Servername='" + Servername + "'";
	  //          return objAdaptor.ExecuteNonQuery(SqlQuery);

	  //      }
	  //      catch
	  //      {
	  //          throw;
	  //      }
	  //  }

		//10/8/2014 WS VE-107
		public bool InsertSrvDatabaseSettingsData(DataTable dtDisk, String ServerName)
		{
			bool Insert = false;

			try
			{
				string SqlQuery = "delete ExchangeDatabaseSettings where ServerName='" + ServerName + "'";
				Insert = objAdaptor.ExecuteNonQuery(SqlQuery);


				for (int i = 0; i < dtDisk.Rows.Count; i++)
				{

					string SqlQuery2 = "insert into ExchangeDatabaseSettings(ServerName,ServerTypeId,DatabaseName, WhiteSpaceThreshold, DatabaseSizeThreshold) " +
						"values('" + dtDisk.Rows[i]["ServerName"].ToString() + "',(select ID from ServerTypes where ServerType='Exchange'),'" +
						dtDisk.Rows[i]["DatabaseName"].ToString() + "','" + dtDisk.Rows[i]["WhiteSpaceThreshold"].ToString() + "','" + dtDisk.Rows[i]["DatabaseSizeThreshold"].ToString() + "')";

					Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);

				}
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

    }


}
