using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using VSWebDO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace VSWebDAL.SecurityDAL
{
    public class ServersDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        
        private AdaptorforDsahBoard adaptor = new AdaptorforDsahBoard();
        private static ServersDAL _self = new ServersDAL();

        public static ServersDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from ServersDAL
        /// </summary>


        public DataTable GetAllData(int? userId)
		{

			DataTable ServersDataTable = new DataTable();
			Servers ReturnSerevrbject = new Servers();
			try
			{
				string SqlQuery= string.Empty;
                if (userId !=null)
                {
					SqlQuery = "SELECT t1.[ID],[ServerName], [ServerType], [Description],MonthlyOperatingCost,IdealUserCount, [Location], [IPAddress],Type,pm.[ProfileName] " +
                        "FROM [Servers] t1 INNER JOIN [Locations] t2 ON t1.[LocationID] = t2.[ID] INNER JOIN [ServerTypes] t3 ON " +
                        "t1.[ServerTypeID] = t3.[ID] inner join HoursIndicator hr on t1.BusinesshoursID=hr.id left outer join ProfileNames pm on t1.ProfileName= CONVERT(nvarchar(20), pm.ID) " +
						"WHERE t1.[LocationID] Not IN(Select [LocationID] from dbo.UserLocationRestrictions WHERE USERID= @UserID) " +
						"AND t1.[ID] Not IN (Select ServerId from dbo.UserServerRestrictions WHERE USERID = @UserID ) ORDER BY [ServerName] ";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@UserID", userId);
					ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
                }
                else {
                    SqlQuery = "SELECT t1.[ID],[ServerName], [ServerType], [Description], [Location], [IPAddress],Type,pm.[ProfileName] " +
                   "FROM [Servers] t1 INNER JOIN [Locations] t2 ON t1.[LocationID] = t2.[ID] INNER JOIN [ServerTypes] t3 ON " +
                   "t1.[ServerTypeID] = t3.[ID] inner join HoursIndicator hr on t1.BusinesshoursID=hr.id left outer join ProfileNames pm on t1.ProfileName= CONVERT(nvarchar(20), pm.ID) ORDER BY [ServerName] ";
					SqlCommand cmd1 = new SqlCommand(SqlQuery);
					ServersDataTable = objAdaptor.FetchDatafromcommand(cmd1);
				}
				

			}
			catch
			{
			}
			finally
			{
			}
			return ServersDataTable;
		}

        public DataTable GetWebSphereTableData(SametimeServers sametimewithid)
        {

            DataTable WebSphereDataTable = new DataTable();
            Servers ReturnSerevrbject = new Servers();
            try
            {
                //1/29/2013 NS added sort
                //string SqlQuery = "SELECT t1.[ID],[ServerName], [ServerType], [Description], [Location], [IPAddress] FROM [Servers] t1 INNER JOIN [Locations] t2 ON [LocationID] = t2.[ID] INNER JOIN [ServerTypes] t3 ON [ServerTypeID] = t3.[ID]";
				string SqlQuery = "select ID,Enabled,Type,[LoginName],Password,Connector,NodeName,Port from WebSphereNodes where ServerID= @UserID";
                //string SqlQuery = "SELECT t1.[ID],[ServerName], [DisplayText] as ServerType, [Description], [Location], [IPAddress] FROM [Servers] t1 INNER JOIN [Locations] t2 ON [LocationID] = t2.[ID] INNER JOIN [MenuItems] t3 ON [ServerTypeID] = t3.[ID]";
                //string SqlQuery = "SELECT * FROM Servers";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@UserID", sametimewithid.ID.ToString());
				WebSphereDataTable = objAdaptor.FetchDatafromcommand(cmd);
                //WebSphereDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return WebSphereDataTable;
        }

        /// <summary>
        /// Insert data into Locations table
        /// </summary>
        /// <param name="DSObject">Locations object</param>
        /// <returns></returns>

        public bool InsertData(Servers ServerObject)
        {
            bool Insert = false;
            bool insertServerDtls = false;
            try
            {
				string SqlQuery = "INSERT INTO [Servers] ([ServerName],[ServerTypeID],[BusinesshoursID],MonthlyOperatingCost,IdealUserCount,[Description],[LocationID],[IPAddress],[ProfileName]) " +
					   "VALUES(@ServerName ,@ServerTypeID , @BusinesshoursID,@MonthlyOperatingCost,@IdealUserCount,@Description ,@LocationID ,@IPAddress,@ProfileName)";
                //FROM [ServerTypes] t1, [Locations] t2 WHERE "+
                // "[ServerType] ="+STObject.ServerType+" AND [Location] = "+LocObject.Location+")"; 
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ServerName", (object)ServerObject.ServerName ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@ServerTypeID", (object)ServerObject.ServerTypeID ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@BusinesshoursID", (object)ServerObject.BusinesshoursID ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@MonthlyOperatingCost", (object)ServerObject.MonthlyOperatingCost ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@IdealUserCount", (object)ServerObject.IdealUserCount ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Description", (object)ServerObject.Description ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@LocationID", (object)ServerObject.LocationID ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@IPAddress", (object)ServerObject.IPAddress ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@ProfileName", (object)ServerObject.ProfileName ?? DBNull.Value);
				Insert = objAdaptor.ExecuteNonQuerywithcmd(cmd);
               
                if (Insert == true)
                {
                    int serverTypeId;



                    System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString());
                    System.Data.SqlClient.SqlCommand com = new System.Data.SqlClient.SqlCommand("select Max(ID) from Servers", con);
					object Maxserverid= null;
					try
					{
						con.Open();
				        Maxserverid = com.ExecuteScalar();
					}
					catch
					{
					}
					finally
					{
						con.Close();
					}
                    //string strquery = "select Max(ID) from Servers";

                    //int Maxserverid = objAdaptor.ExecuteScalar(strquery);
					if(!string.IsNullOrEmpty(Maxserverid.ToString()))
					//if (Maxserverid != "" ) 
                    {
						//string query = "insert into ServerTaskSettings (ServerID) values(" + Maxserverid + ")";
						//objAdaptor.ExecuteNonQuery(query);

						string qry = "SELECT [ServerTypeID] FROM [Servers] WHERE ID =  @Maxserverid";

                        //MD[01Jan2014]
                       // serverTypeId = objAdaptor.ExecuteScalar(qry);
						SqlCommand cmd1 = new SqlCommand(qry);
						cmd1.Parameters.AddWithValue("@Maxserverid", Maxserverid ?? DBNull.Value);
						serverTypeId = objAdaptor.ExecuteScalarwithcmd(cmd1);
						
                        string sqlServerDtls = "";
                        if (serverTypeId == 1)
                        {
                            //5/1/2014 NS modified for VSPLUS-589
                            /*
                            sqlServerDtls = "INSERT INTO [DominoServers] ([ServerID], [Enabled], [RetryInterval], [Scan Interval], [ResponseThreshold], [OffHoursScanInterval], [FailureThreshold], [Category], [PendingThreshold], [DeadThreshold], [HeldThreshold], [Cluster_Rep_Delays_Threshold]) " +
                                "VALUES(" + Maxserverid + ", " + 1 + ", " + 2 + ", " + 8 +
                                ", " + 50000 + ", " + 15 + ", " + 2 + ", 'Mail', " + 200 + ", " + 500 + ", " + 200 + ", " + 120 + ")";
                            */
                            string attrname = "";
                            string attrval = "";
                            string [] domdefaults = new string[17];
                            DataTable dtattr = new DataTable();
                            dtattr = ProfilesMasterDAL.Ins.GetAllDataByServerType("Domino");
                            if (dtattr.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtattr.Rows.Count; i++)
                                {
                                    attrname = dtattr.Rows[i]["AttributeName"].ToString();
                                    attrval = dtattr.Rows[i]["DefaultValue"].ToString();
                                    switch (attrname)
                                    {
                                        case "Scan Interval":
                                            domdefaults[0] = attrval;
                                            break;
                                        case "Off Hours Scan Interval":
                                            domdefaults[1] = attrval;
                                            break;
                                        case "Retry Interval":
                                            domdefaults[2] = attrval;
                                            break;
                                        case "Response Time Threshold":
                                            domdefaults[3] = attrval;
                                            break;
                                        case "Failure Threshold":
                                            domdefaults[4] = attrval;
                                            break;
                                        case "Cluster Replication Delays Threshold":
                                            domdefaults[5] = attrval;
                                            break;
                                        case "Pending Mail Threshold":
                                            domdefaults[6] = attrval;
                                            break;
                                        case "Dead Mail Threshold":
                                            domdefaults[7] = attrval;
                                            break;
                                        case "Held Mail Threshold":
                                            domdefaults[8] = attrval;
                                            break;
                                        case "Scan DB Health":
                                            domdefaults[9] = attrval;
                                            break;
                                        case "BES Server":
                                            domdefaults[10] = attrval;
                                            break;
                                        //case "Disk Space Threshold":// Commented by Mukund 06Jun14:Error this parameter is not taken now. Disk space is separately dealt
                                        //    domdefaults[11] = attrval;
                                        //    break;
                                        case "Memory Threshold":
                                            if (attrval.ToString() != "")
                                            {
                                                //8/18/2014 NS modified - the default value should be passed on to 
                                                //the DominoServers table as is since it's stored as a decimal in
                                                //ProfilesMaster
                                                //domdefaults[12] = (Convert.ToDecimal(attrval)/100).ToString();
                                                domdefaults[12] = attrval;
                                            }
                                            break;
                                        case "CPU Threshold":
                                            if (attrval.ToString() != "")
                                            {
                                                //8/18/2014 NS modified - the default value should be passed on to 
                                                //the DominoServers table as is since it's stored as a decimal in
                                                //ProfilesMaster
                                                //domdefaults[13] = (Convert.ToDecimal(attrval) / 100).ToString();
                                                domdefaults[13] = attrval;
                                            }
                                            break;
                                        case "BES Threshold":
                                            domdefaults[14] = attrval;
                                            break;
                                        case "Dead Mail Delete Threshold":
                                            domdefaults[15] = attrval;
                                            break;
                                        case "Server Days Alert":
                                            domdefaults[16] = attrval;
                                            break;
                                    }
                                }
                              // Commented by Mukund 06Jun14:Error this parameter is not taken now. Disk space is separately dealt
                                //sqlServerDtls = "INSERT INTO [DominoServers] ([ServerID], [Enabled], [RetryInterval], [Scan Interval], " +
                                //    "[ResponseThreshold], [OffHoursScanInterval], [FailureThreshold], [Category], [PendingThreshold], " +
                                //    "[DeadThreshold], [HeldThreshold], [Cluster_Rep_Delays_Threshold], [ScanDBHealth], [BES_Server], " +
                                //    "[DiskSpaceThreshold], [Memory_Threshold], [CPU_Threshold], [BES_Threshold], " +
                                //    "[DeadMailDeleteThreshold], [ServerDaysAlert]) " + 
                                //    "VALUES(" + Maxserverid + ", " + 1 + ", " + domdefaults[2] + ", " + domdefaults[0] +
                                //    ", " + domdefaults[3] + ", " + domdefaults[1] + ", " + domdefaults[4] + ", 'Mail', " + domdefaults[6] + 
                                //    ", " + domdefaults[7] + ", " + domdefaults[8] + ", " + domdefaults[5] + ", " + domdefaults[9] + 
                                //    ", " + domdefaults[10] + ", " + domdefaults[11] + ", " + domdefaults[12] + ", " + domdefaults[13] + 
                                //    ", " + domdefaults[14] + ", " + domdefaults[15] + ", " + domdefaults[16] + ")";
                                sqlServerDtls = "INSERT INTO [DominoServers] ([ServerID], [Enabled], [RetryInterval], [Scan Interval], " +
                                   "[ResponseThreshold], [OffHoursScanInterval], [FailureThreshold], [Category], [PendingThreshold], " +
                                   "[DeadThreshold], [HeldThreshold], [Cluster_Rep_Delays_Threshold], [ScanDBHealth], [BES_Server], " +
                                   "[Memory_Threshold], [CPU_Threshold], [BES_Threshold], " +
                                   "[DeadMailDeleteThreshold], [ServerDaysAlert]) " +
                                   "VALUES( @ServerID, @Enabled, @RetryInterval, @ScanInterval "+
                                   ", @ResponseThreshold, @OffHoursScanInterval,@FailureThreshold, @Category, @PendingThreshold" +
								   ", @DeadThreshold, @HeldThreshold, @Cluster_Rep_Delays_Threshold, @ScanDBHealth" +
								   ", @BES_Server, @Memory_Threshold, @CPU_Threshold" +
								   ", @BES_Threshold, @DeadMailDeleteThreshold, @ServerDaysAlert)";
								SqlCommand cmd2 = new SqlCommand(sqlServerDtls);
								cmd2.Parameters.AddWithValue("@ServerID", (object) Maxserverid ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@Enabled", "1");
								cmd2.Parameters.AddWithValue("@RetryInterval", (object)domdefaults[2] ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@ScanInterval", (object)domdefaults[0] ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@ResponseThreshold", (object)domdefaults[3] ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@OffHoursScanInterval", (object) domdefaults[1] ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@FailureThreshold", (object)domdefaults[4] ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@Category", (object) "Mail"?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@PendingThreshold", (object) domdefaults[6]?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@DeadThreshold", (object)domdefaults[7]?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@HeldThreshold", (object) domdefaults[8]?? DBNull.Value);
								
								cmd2.Parameters.AddWithValue("@Cluster_Rep_Delays_Threshold", (object)domdefaults[5] ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@ScanDBHealth", (object)domdefaults[9]?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@BES_Server", (object)domdefaults[10] ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@Memory_Threshold", (object)domdefaults[12]?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@CPU_Threshold", (object)domdefaults[13]?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@BES_Threshold", (object)domdefaults[14]?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@DeadMailDeleteThreshold", (object)domdefaults[15]?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@ServerDaysAlert", (object)domdefaults[16] ?? DBNull.Value);
								insertServerDtls = objAdaptor.ExecuteNonQuerywithcmd(cmd2);

                            }
                            else
                            {
                                sqlServerDtls = "INSERT INTO [DominoServers] ([ServerID], [Enabled], [RetryInterval], [Scan Interval], " +
                                    "[ResponseThreshold], [OffHoursScanInterval], [FailureThreshold], [Category], [PendingThreshold], " +
                                    "[DeadThreshold], [HeldThreshold], [Cluster_Rep_Delays_Threshold]) " +
									"VALUES( @ServerID , @Enabled, @RetryInterval, @ScanInterval"+
									", @ResponseThreshold, @OffHoursScanInterval, @FailureThreshold, @Category, @PendingThreshold, @DeadThreshold, @HeldThreshold,@Cluster_Rep_Delays_Threshold)";
								SqlCommand cmd2 = new SqlCommand(sqlServerDtls);
								cmd2.Parameters.AddWithValue("@ServerID", (object)Maxserverid ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@Enabled", "1");
								cmd2.Parameters.AddWithValue("@RetryInterval", (object) 2 ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@ScanInterval", (object) 8 ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@ResponseThreshold", (object)50000 ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@OffHoursScanInterval", (object)15 ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@FailureThreshold", (object)2 ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@Category", (object)"Mail" ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@PendingThreshold", (object)200 ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@DeadThreshold", (object)500 ?? DBNull.Value);
								cmd2.Parameters.AddWithValue("@HeldThreshold", (object)200 ?? DBNull.Value);

								cmd2.Parameters.AddWithValue("@Cluster_Rep_Delays_Threshold", (object)120 ?? DBNull.Value);
								insertServerDtls = objAdaptor.ExecuteNonQuerywithcmd(cmd2);
                            }
                            //insertServerDtls = objAdaptor.ExecuteNonQuery(sqlServerDtls);
                        }
                        else if (Convert.ToInt32(ServerObject.ServerTypeID) == 2)
                        {
                            //6/30/2014 NS modified for VSPLUS-787
                            //sqlServerDtls = "INSERT INTO [BlackBerryServers] ([ServerID], [Enabled], [RetryInterval], [ScanInterval], [OffHoursScanInterval], [Category], [PendingThreshold], [ExpiredThreshold]) " +
                            //    "VALUES(" + Maxserverid + ", " + 1 + ", " + 2 + ", " + 8 +
                            //    ", " + 15 + ", " + "'Mail', " + 200 + ", " + 500 + ")";
                            sqlServerDtls = "INSERT INTO [BlackBerryServers] ([ServerID], [Enabled], [RetryInterval], [ScanInterval], [OffHoursScanInterval], [Category], [PendingThreshold], [ExpiredThreshold]) " +
									"VALUES(@ServerID, @Enabled, @RetryInterval, @ScanInterval" +
									", @OffHoursScanInterval, @Category, @PendingThreshold, @ExpiredThreshold)";
							SqlCommand cmd2 = new SqlCommand(sqlServerDtls);
							cmd2.Parameters.AddWithValue("@ServerID", (object)Maxserverid ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@Enabled", "0");
							cmd2.Parameters.AddWithValue("@RetryInterval", (object)2 ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@ScanInterval", (object)8 ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@OffHoursScanInterval", (object)15 ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@Category", (object)"Mail" ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@PendingThreshold", (object)200 ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@ExpiredThreshold", (object)500 ?? DBNull.Value);
							insertServerDtls = objAdaptor.ExecuteNonQuerywithcmd(cmd2);
                        }
                        else if (Convert.ToInt32(ServerObject.ServerTypeID) == 3)
                        {
                            sqlServerDtls = "INSERT INTO [SametimeServers] ([ServerID], [Enabled], [RetryInterval], [ScanInterval], [ResponseThreshold], [OffHoursScanInterval], [FailureThreshold]) " +
								"VALUES(@ServerID, @Enabled, @RetryInterval,@ScanInterval"+
								", @ResponseThreshold, @OffHoursScanInterval, @FailureThreshold)";
							SqlCommand cmd2 = new SqlCommand(sqlServerDtls);
							cmd2.Parameters.AddWithValue("@ServerID", (object)Maxserverid ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@Enabled", "1");
							cmd2.Parameters.AddWithValue("@RetryInterval", (object)2 ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@ScanInterval", (object)8 ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@ResponseThreshold", (object)50000 ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@OffHoursScanInterval", (object)15 ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@FailureThreshold", (object)2 ?? DBNull.Value);
							insertServerDtls = objAdaptor.ExecuteNonQuerywithcmd(cmd2);
                        }
						else if (Convert.ToInt32(ServerObject.ServerTypeID) == 27)
						{
							sqlServerDtls = "INSERT INTO [IBMConnectionsServers] ([ServerID], [Enabled], [RetryInterval], [ScanInterval], [ResponseThreshold], [OffHoursScanInterval], [FailureThreshold]) " +
								"VALUES(@ServerID, @Enabled, @RetryInterval,@ScanInterval" +
								", @ResponseThreshold, @OffHoursScanInterval, @FailureThreshold)";
							SqlCommand cmd2 = new SqlCommand(sqlServerDtls);
							cmd2.Parameters.AddWithValue("@ServerID", (object)Maxserverid ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@Enabled", "1");
							cmd2.Parameters.AddWithValue("@RetryInterval", (object)2 ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@ScanInterval", (object)8 ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@ResponseThreshold", (object)50000 ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@OffHoursScanInterval", (object)15 ?? DBNull.Value);
							cmd2.Parameters.AddWithValue("@FailureThreshold", (object)2 ?? DBNull.Value);
							insertServerDtls = objAdaptor.ExecuteNonQuerywithcmd(cmd2);
						}
                        else if (Convert.ToInt32(ServerObject.ServerTypeID) == 3)
                        {
                            //sqlServerDtls = "INSERT INTO [ActiveDirectoryServers] ([ServerID], [Enabled], [RetryInterval], [ScanInterval], [ResponseThreshold], [OffHoursScanInterval], [FailureThreshold]) " +
                            //    "VALUES(" + Maxserverid + ", " + 1 + ", " + 2 + ", " + 8 +
                            //    ", " + 50000 + ", " + 15 + ", " + 2 + ")";


                            //insertServerDtls = objAdaptor.ExecuteNonQuery(sqlServerDtls);
                        }
                        else if (Convert.ToInt32(ServerObject.ServerTypeID) == 4)
                        {
                            
                        }    
                        else if (Convert.ToInt32(ServerObject.ServerTypeID) == 5)
                        {
                            //MD[07Mar2014] commented as Exchange data is now maintained in Servers table.
                            //sqlServerDtls = "INSERT INTO [MSServerSettings] ([ServerID], [Enabled], [RetryInterval], [ScanInterval], [ResponseThreshold], [OffscanInterval], [FailuresbfrAlert]) " +
                            //    "VALUES(" + Maxserverid + ", " + 1 + ", " + 2 + ", " + 8 +
                            //    ", " + 50000 + ", " + 15 + ", " + 2 + ")";

                            //insertServerDtls = objAdaptor.ExecuteNonQuery(sqlServerDtls);
                        }
                        else if (Convert.ToInt32(ServerObject.ServerTypeID) == 6)
                        {
                            
                        }
                        else if (Convert.ToInt32(ServerObject.ServerTypeID) == 7)
                        {
                        }
                        else if (Convert.ToInt32(ServerObject.ServerTypeID) == 8)
                        {
                        }
                        else if (Convert.ToInt32(ServerObject.ServerTypeID) == 9)
                        {
                        }
                        //MD[01Jan2014] Ends
                    }
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


       
        /// <summary>
        /// Update data into Locations table
        /// </summary>
        /// <param name="ServerObject">Locations object</param>
        /// <returns></returns>
        public Object UpdateData(Servers ServerObject)
        {
            Object Update;
            try
            {
				string SqlQuery = "UPDATE Servers SET ServerName=@ServerName" +
					",ServerTypeID=@ServerTypeID ,Description=@Description "+

					",LocationID=@LocationID,BusinesshoursID=@BusinesshoursID ,MonthlyOperatingCost=@MonthlyOperatingCost,IdealUserCount=@IdealUserCount,IPAddress=@IPAddress,ProfileName=@ProfileName  where ID=@ID";
                //string SqlQuery = "UPDATE [Servers] SET [ServerName]='"+ServerObject.ServerName+"',[ServerTypeID]=t2.[ID],"+
                //   "[Description]='"+ServerObject.Description+ "',[LocationID]=t3.[ID],[IPAddress]='"+ServerObject.IPAddress+
                //    "' FROM [Servers] t1, [ServerTypes] t2, [Locations] t3 WHERE t1.[ID]="+ServerObject.ID +" AND t2.[ServerType]="+
                //""+STObject.ServerType+" AND t3.[Location]="+LocObject.Location+"";
				SqlCommand cmd2 = new SqlCommand(SqlQuery);
				cmd2.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
				cmd2.Parameters.AddWithValue("@ServerName", (object)ServerObject.ServerName ?? DBNull.Value);
				cmd2.Parameters.AddWithValue("@ServerTypeID", (object)ServerObject.ServerTypeID ?? DBNull.Value);
				cmd2.Parameters.AddWithValue("@Description", (object)ServerObject.Description ?? DBNull.Value);
				cmd2.Parameters.AddWithValue("@LocationID", (object)ServerObject.LocationID ?? DBNull.Value);
				cmd2.Parameters.AddWithValue("@BusinesshoursID", (object)ServerObject.BusinesshoursID ?? DBNull.Value);
				cmd2.Parameters.AddWithValue("@MonthlyOperatingCost", (object)ServerObject.MonthlyOperatingCost ?? DBNull.Value);
				cmd2.Parameters.AddWithValue("@IdealUserCount", (object)ServerObject.IdealUserCount ?? DBNull.Value);
				cmd2.Parameters.AddWithValue("@IPAddress", (object)ServerObject.IPAddress ?? DBNull.Value);
				cmd2.Parameters.AddWithValue("@ProfileName", (object)ServerObject.ProfileName ?? DBNull.Value);
				Update = objAdaptor.ExecuteNonQuerywithcmd(cmd2);
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
        //delete Data from Locations Table
   
        //public Object InsertDatas(WebSphereNodes STSettingsObject,SametimeServers serverid)
        //{
        //    Object Update;
        //    try
        //    {

        //        string SqlQuery = "INSERT INTO WebSphereNodes(ServerID,Port ,[Enabled],LoginName,Password," +
        //            "Connector,NodeName,Type)" +
        //                       " VALUES(" + serverid.ServerID + "," + STSettingsObject.Port + ",'" + STSettingsObject.Enabled +
        //                        "','" + STSettingsObject.LoginName + "','" + STSettingsObject.Password +
        //                         "','" + STSettingsObject.Connector + "','" + STSettingsObject.NodeName +
        //                         "','" + STSettingsObject.Type + "')";
        //        Update = objAdaptor.ExecuteNonQuery(SqlQuery);
        //    }
        //    catch
        //    {
        //        Update = false;
        //    }
        //    finally
        //    {
        //    }
        //    return Update;
        //}

        public Object DeleteData(Servers ServerObject)
        {
            Object Update;
            try
            {
				string SqlQuery = "DELETE FROM [Servers] WHERE [ID]=@ID";
				 SqlCommand cmd2 = new SqlCommand(SqlQuery);
				 cmd2.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
				 Update = objAdaptor.ExecuteNonQuerywithcmd(cmd2);
            }
            catch
            {
                Update = false;
                //Update= "This Server Is Using Somewhere, Cannot Delete. ";
            }
            finally
            {
            }
            return Update;
        }
        public Object DeleteData1(WebSphereNodes ServerObject)
        {
            Object Update;
            try
            {
				string SqlQuery = "DELETE FROM [WebSphereNodes] WHERE [ID]=@ID";
				SqlCommand cmd2 = new SqlCommand(SqlQuery);
				cmd2.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
				Update = objAdaptor.ExecuteNonQuerywithcmd(cmd2);
                
            }
            catch
            {
                Update = false;
                //Update= "This Server Is Using Somewhere, Cannot Delete. ";
            }
            finally
            {
            }
            return Update;
        }

		public Object DeleteServerDependencies(Servers ServerObject)
		{
			Object Update;
			try
			{

				string SqlQuery = "DELETE FROM [ServerRoles] WHERE [ServerId]=@ID";

				//Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				SqlCommand cmd2 = new SqlCommand(SqlQuery);
				cmd2.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
				Update = objAdaptor.ExecuteNonQuerywithcmd(cmd2);


				SqlQuery = "DELETE FROM [ServerAttributes] WHERE [ServerId]=@ID";

				//Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				SqlCommand cmd3 = new SqlCommand(SqlQuery);
				cmd3.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
				Update = objAdaptor.ExecuteNonQuerywithcmd(cmd3);


                SqlQuery = "DELETE DominoDiskSpace FROM DominoDiskSpace DSP WHERE DSP.ServerName = (Select ServerName FROM Servers srv WHERE srv.ID = @ID )";
				SqlCommand cmd4 = new SqlCommand(SqlQuery);
				cmd4.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
				Update = objAdaptor.ExecuteNonQuerywithcmd(cmd4);
                //Update = objAdaptor.ExecuteNonQuery(SqlQuery);

                SqlQuery = "DELETE Daily FROM Daily DS WHERE DS.Server = (SELECT ServerName FROM VitalSigns.dbo.Servers srv WHERE srv.ID = @ID)";
				SqlCommand cmd5 = new SqlCommand(SqlQuery);
				cmd5.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
				Update = objAdaptor.ExecuteNonQuerywithcmd(cmd5);
               // Update = adaptor.ExecuteNonQuery(SqlQuery);

				SqlQuery = "DELETE DominoSummaryStats FROM DominoSummaryStats DSS WHERE DSS.ServerName = (SELECT ServerName FROM VitalSigns.dbo.Servers srv WHERE srv.ID = @ID)";
				SqlCommand cmd6 = new SqlCommand(SqlQuery);
				cmd6.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
				Update = objAdaptor.ExecuteNonQuerywithcmd(cmd6);
                //Update = adaptor.ExecuteNonQuery(SqlQuery);

				SqlQuery = "DELETE DeviceUpTimeStats FROM DeviceUpTimeStats DTS WHERE DTS.DeviceName = (SELECT ServerName FROM VitalSigns.dbo.Servers srv WHERE srv.ID = @ID)";

               // Update = adaptor.ExecuteNonQuery(SqlQuery);
				SqlCommand cmd7 = new SqlCommand(SqlQuery);
				cmd7.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
				Update = objAdaptor.ExecuteNonQuerywithcmd(cmd7);

				SqlQuery = "DELETE DominoClusterHealth FROM DominoClusterHealth DCH WHERE DCH.ServerName = (SELECT ServerName FROM VitalSigns.dbo.Servers srv WHERE srv.ID = @ID and ServerTypeID='1')";
				SqlCommand cmd8 = new SqlCommand(SqlQuery);
				cmd8.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
				Update = objAdaptor.ExecuteNonQuerywithcmd(cmd8);
				//Update = objAdaptor.ExecuteNonQuery(SqlQuery);

                //2/5/2016 NS added for VSPLUS-2584
                SqlQuery = "DELETE FROM DominoServerDetails WHERE ServerID = @ID";
                SqlCommand cmd9 = new SqlCommand(SqlQuery);
                cmd9.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
                Update = objAdaptor.ExecuteNonQuerywithcmd(cmd9);
				
			}
			catch
			{
				Update = false;
				//Update= "This Server Is Using Somewhere, Cannot Delete. ";
			}
			finally
			{
			}
			return Update;
		}
        public Object DeleteServerDependencies1(WebSphereNodes ServerObject)
        {
            Object Update;
            try
            {

				string SqlQuery = "DELETE FROM [WebSphereNodes] WHERE [ServerId]=@ID";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
				Update = objAdaptor.ExecuteNonQuerywithcmd(cmd);
               //Update = objAdaptor.ExecuteNonQuery(SqlQuery);


				SqlQuery = "DELETE FROM [WebSphereNodes] WHERE [ServerId]=@ID";
				SqlCommand cmd1 = new SqlCommand(SqlQuery);
				cmd1.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
				Update = objAdaptor.ExecuteNonQuerywithcmd(cmd1);
                //Update = objAdaptor.ExecuteNonQuery(SqlQuery);


            }
            catch
            {
                Update = false;
                //Update= "This Server Is Using Somewhere, Cannot Delete. ";
            }
            finally
            {
            }
            return Update;
        }

        public DataTable GetDataByName(Servers ServerObject)
        {

            DataTable ServersDataTable = new DataTable();
            Servers ReturnSerevrbject = new Servers();
            try
            {
                if (ServerObject.ID == 0)
                {
                    //5/1/2013 NS updated the query below
                    //string SqlQuery = "SELECT * from Servers where ServerName='" + ServerObject.ServerName + "' ";
                    //10/29/2013 NS update the query to accommodate for the case of the same server working under two different 
                    //server types
                    //string SqlQuery = "SELECT *,ServerType,Location from Servers s1 inner join ServerTypes s2 on s1.ServerTypeID=s2.ID " +
                    //    " inner join Locations l on s1.LocationID=l.ID where ServerName='" + ServerObject.ServerName + "' ";
                    string SqlQuery = "SELECT *,ServerType,Location from Servers s1 inner join ServerTypes s2 on s1.ServerTypeID=s2.ID " +
							" inner join Locations l on s1.LocationID=l.ID where ServerName = @ServerName" +
							" and ServerTypeID = @ServerTypeID ";
                    //string SqlQuery = "SELECT t1.[ID],[ServerName], [DisplayText] as ServerType, [Description], [Location], [IPAddress] FROM [Servers] t1 INNER JOIN [Locations] t2 ON [LocationID] = t2.[ID] INNER JOIN [MenuItems] t3 ON [ServerTypeID] = t3.[ID]";
                    //string SqlQuery = "SELECT * FROM Servers";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@ServerName",(object) ServerObject.ServerName ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ServerTypeID", (object)ServerObject.ServerTypeID ?? DBNull.Value);
					ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
                }
                else
                {
                    //10/29/2013 NS modified - when one of the servers being created/updated has the same name but different type as another server,
                    //the save fails
                    //string SqlQuery = "SELECT * from Servers where ServerName='" + ServerObject.ServerName + "' and ID<>'"+ServerObject.ID+"'";
					string SqlQuery = "SELECT * from Servers where ServerName = @ServerName and ID<>@ID" +
                        " and ServerTypeID=@ServerTypeID";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@ServerName", (object)ServerObject.ServerName ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ServerTypeID", (object)ServerObject.ServerTypeID ?? DBNull.Value);
					ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
                    //ServersDataTable = objAdaptor.FetchData(SqlQuery);
                }

            }
            catch
            {
            }
            finally
            {
            }
            return ServersDataTable;
        }
        public DataTable GetDataByName1(WebSphereNodes ServerObject)
        {

            DataTable WebSphereDataTable = new DataTable();
            Servers ReturnSerevrbject = new Servers();
            try
            {
                if (ServerObject.ID == 0)
                {
                    
                    string SqlQuery = "select * from WebSphereNodes where ServerID=@ID";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
					WebSphereDataTable = objAdaptor.FetchDatafromcommand(cmd);
                }
                else
                {
                    string SqlQuery = "select * from WebSphereNodes where ServerID=@ServerID";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@ServerID", (object)ServerObject.ServerID ?? DBNull.Value);
					WebSphereDataTable = objAdaptor.FetchDatafromcommand(cmd);
                }

            }
            catch
            {
            }
            finally
            {
            }
            return WebSphereDataTable;
        }

        public DataTable GetAllDataByServerType(string ServerType)
        {

            DataTable ServersDataTable = new DataTable();
            try
            {
				string SqlQuery = " select sr.*,st.ServerType,loc.Location from Servers sr,ServerTypes st,Locations loc where sr.ServerTypeID=st.ID and sr.LocationID=loc.ID and st.ServerType=@ServerType ";
			   SqlCommand cmd = new SqlCommand(SqlQuery);
			   cmd.Parameters.AddWithValue("@ServerType",(object) ServerType ?? DBNull.Value);
				ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
            }
            catch
            {
            }
            finally
            {

            }
            return ServersDataTable;
        }

        public Int32 GetServerIDbyServerName(string ServerName)
        {
            DataTable ServersDataTable = new DataTable();
            int ID = 0;
            try
            {
				string SqlQuery = "SELECT ID FROM Servers WHERE ServerName=@ServerName";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ServerName", (object)ServerName ?? DBNull.Value);
				ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
				if (ServersDataTable.Rows.Count > 0)
				{
					ID = Convert.ToInt32(ServersDataTable.Rows[0]["ID"]);
				}
            }
            catch
            {
            }
            finally
            {

            }
            return ID;
        }
		public Int32 GetO365ServerIDbyServerName(string serverName)
        {
            DataTable ServersDataTable = new DataTable();
            int ID = 0;
            try
            {
				string SqlQuery = "SELECT ID FROM O365Server WHERE Name=@ServerName";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ServerName", (object)serverName ?? DBNull.Value);
				ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
				if (ServersDataTable.Rows.Count > 0)
				{
					ID = Convert.ToInt32(ServersDataTable.Rows[0]["ID"]);
				}
            }
            catch
            {
            }
            finally
            {

            }
            return ID;
        }
		
		public Int32 GetCloudIDbyDeviceName(string DeviceName)
        {
            DataTable ServersDataTable = new DataTable();
            int ID = 0;
            try
            {
				string SqlQuery = "SELECT ID FROM CloudDetails WHERE Name = @Name";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@Name", (object)DeviceName ?? DBNull.Value);
				ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
                if (ServersDataTable.Rows.Count > 0)
				{
					ID = Convert.ToInt32(ServersDataTable.Rows[0]["ID"]);
				}
            }
            catch
            {
            }
            finally
            {

            }
            return ID;
        }
        public DataTable GetServerDetailsByName(string serverName)
        {
            DataTable ServersDataTable = new DataTable();
            try
            {
				string SqlQuery = "SELECT * FROM Servers WHERE ServerName=@ServerName";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ServerName", (object)serverName ?? DBNull.Value);
				ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
                
            }
            catch
            {
            }
            finally
            {

            }
            return ServersDataTable;
        }

        public DataTable GetServerDetailsByID(int ID)
        {
            DataTable ServersDataTable = new DataTable();
            try
            {
                //string SqlQuery = "SELECT * FROM Servers WHERE ID=" + ID ;
				string SqlQuery = "select * from Servers ss inner join Status st on ss.ServerName = st.Name where ss.id = @ID";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ID", (object) ID ?? DBNull.Value);
				ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
             }
            catch
            {
            }
            finally
            {

            }
            return ServersDataTable;
        }

		public DataTable GetURLDetailsByID(int ID)
        {
            DataTable ServersDataTable = new DataTable();
            try
            {
                //string SqlQuery = "SELECT * FROM Servers WHERE ID=" + ID ;
				string SqlQuery = "select * from  URLs ss inner join Status st on ss.Name = st.Name where ss.id = @ID";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ID", (object)ID ?? DBNull.Value);
				ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
            }
            catch
            {
            }
            finally
            {

            }
            return ServersDataTable;
        }
		public DataTable GetCloudDetailsByID(int ID)
        {
            DataTable ServersDataTable = new DataTable();
            try
            {
                //string SqlQuery = "SELECT * FROM Servers WHERE ID=" + ID ;
				string SqlQuery = "select * from CloudDetails ss inner join Status st on ss.Name = st.Name where ss.id=@ID";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ID", (object)ID ?? DBNull.Value);
				ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
                //ServersDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {

            }
            return ServersDataTable;
        }
		
		public DataTable GetServerDetailsByName_Mail(string serverName)
		{
			DataTable ServersDataTable = new DataTable();
			try
			{
				//string SqlQuery = "SELECT * FROM Servers WHERE ID=" + ID ;
				string SqlQuery = "select * from status where Name = @Name";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@Name", (object)serverName ?? DBNull.Value);
				ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
				//ServersDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch
			{
			}
			finally
			{

			}
			return ServersDataTable;
		}


        //MD 06Mar14
        /// <summary>
        /// Update data into ServerAttributes table (with new fields based on Exchange discussion)
        /// </summary>
        /// <param name="ServerAttributes">ServerAttributes object</param>
        /// <returns></returns>
        public Object UpdateAttributesData(ServerAttributes ServerObject)
        {
            Object Update;
            try
            {
                DataTable dt = GetAttributes(ServerObject.ServerId);
                if (dt.Rows.Count > 0)
                {
                    string SqlQuery = "UPDATE ServerAttributes SET Enabled=@Enabled" +
                        ",ScanInterval=@ScanInterval,RetryInterval=@RetryInterval" +
                        ",OffHourInterval=@OffHourInterval,Category=@Category" +
                        ",CPU_Threshold=@CPUThreshold,MemThreshold=@MemThreshold,ResponseTime=@ResponseTime" +
                        ",ConsFailuresBefAlert=@ConsFailuresBefAlert,ConsOvrThresholdBefAlert =@ConsOvrThresholdBefAlert" + 
						",CredentialsId =@CredentialsId , ScanDagHealth=@ScanDAGHealth  where ServerId=@ServerId";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@ServerId", (object)ServerObject.ServerId ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@Enabled", (object)ServerObject.Enabled ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ScanInterval", (object)ServerObject.ScanInterval ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@RetryInterval", (object)ServerObject.RetryInterval ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@OffHourInterval", (object)ServerObject.OffHourInterval ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@Category", (object)ServerObject.Category ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@CPUThreshold", (object)ServerObject.CPUThreshold ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@MemThreshold", (object)ServerObject.MemThreshold ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ResponseTime", (object)ServerObject.ResponseTime ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ConsFailuresBefAlert", (object)ServerObject.ConsFailuresBefAlert ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ConsOvrThresholdBefAlert", (object)ServerObject.ConsOvrThresholdBefAlert ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@CredentialsId", (object)ServerObject.CredentialsId ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ScanDagHealth", (object)ServerObject.ScanDAGHealth ?? DBNull.Value);
                    Update = objAdaptor.ExecuteNonQuerywithcmd(cmd);
                }
                else
                {
                    Update = InsertAttributesData(ServerObject);
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

        /// <summary>
        /// Insert data into ServerAttributes table (with new fields based on Exchange discussion)
        /// </summary>
        /// <param name="ServerObject"></param>
        /// <returns></returns>
		public bool InsertAttributesData(ServerAttributes ServerObject)
		{

			//WS modified to fetch realistic data for the exchange servers
			bool Insert = false;
			try
			{
				string sqlStr = "select st.servertype from servertypes st where st.id=(select srv.servertypeid from servers srv where srv.id= @ServerId)";
				SqlCommand cmd = new SqlCommand(sqlStr);
				cmd.Parameters.AddWithValue("@ServerId", (object)ServerObject.ServerId ?? DBNull.Value);
				DataTable dt = objAdaptor.FetchDatafromcommand(cmd);
				string ServerType = dt.Rows.Count == 1 ? dt.Rows[0][0].ToString() : "";
				string[] exchgdefaults = new string[25];
				string attrname = "";
				string attrval = "";
				DataTable dtattr = ProfilesMasterDAL.Ins.GetAllDataByServerType(ServerType);
				if (dtattr.Rows.Count > 0)
				{
					for (int i = 0; i < dtattr.Rows.Count; i++)
					{
						attrname = dtattr.Rows[i]["AttributeName"].ToString();
						attrval = dtattr.Rows[i]["DefaultValue"].ToString();
						switch (attrname)
						{
							case "Scan Interval":
								exchgdefaults[0] = attrval;
								break;
							case "Off Hours Scan Interval":
								exchgdefaults[1] = attrval;
								break;

							case "Retry Interval":
								exchgdefaults[2] = attrval;
								break;
							case "Response Threshold":
								exchgdefaults[3] = attrval;
								break;
							case "Enabled":
								exchgdefaults[12] = attrval;
								break;
							case "Category":
								exchgdefaults[13] = attrval;
								break;
							case "Failure Threshold":
								exchgdefaults[4] = attrval;
								break;
							case "Poison Queue Threshold":
								exchgdefaults[5] = attrval;
								break;
							case "Submission Queue Threshold":
								exchgdefaults[6] = attrval;
								break;
							case "UnReachable Queue Threshold":
								exchgdefaults[7] = attrval;
								break;
							case "Total Queue Threshold":
								exchgdefaults[8] = attrval;
								break;
							case "Memory Threshold":
								if (attrval.ToString() != "")
								{

									double temp = Convert.ToDouble(attrval);
									exchgdefaults[9] = Convert.ToString(temp * 100);
								}
								break;
							case "CPU Threshold":
								if (attrval.ToString() != "")
								{

									double temp = Convert.ToDouble(attrval);
									exchgdefaults[10] = Convert.ToString(temp * 100);
								}
								break;
							case "Server Days Alert":
								exchgdefaults[11] = attrval;
								break;
                            case "Average Thread Poll":
                                exchgdefaults[14] = attrval;
                                break;
                            case "Heap Current":
                                exchgdefaults[15] = attrval;
                                break;
                            case "Up Time":
                                exchgdefaults[16] = attrval;
                                break;
                            case "Dump Generator":
                                exchgdefaults[17] = attrval;
                                break;
                            case "Active Thread Count":
                                exchgdefaults[18] = attrval;
                                break;
                            case "Maximum Heap":
                                exchgdefaults[19] = attrval;
                                break;
                            case "Hung Thread Count":
                                exchgdefaults[20] = attrval;
                                break;
                               // 11-04-2016 Durga Modified for VSPLUS-2742
                            case "Shadow Queue Threshold":
                                exchgdefaults[21] = attrval;
                                break;
							default:
								string s = attrname;
								break;
						}
					}

					string SqlQuery = "INSERT INTO [ServerAttributes] (ServerID,Enabled,ScanInterval,RetryInterval,OffHourInterval,Category,CPU_Threshold,MemThreshold,ResponseTime,ConsFailuresBefAlert,ConsOvrThresholdBefAlert) " +
					   " VALUES(@ServerId,@Enabled,@ScanInterval,@RetryInterval,@OffHourInterval"+
						",@Category,@CPU_Threshold,@MemThreshold,@ResponseTime,@ConsFailuresBefAlert,@ConsOvrThresholdBefAlert)";
					SqlCommand cmd2 = new SqlCommand(SqlQuery);
					cmd2.Parameters.AddWithValue("@ServerId", (object)ServerObject.ServerId ?? DBNull.Value);
					cmd2.Parameters.AddWithValue("@Enabled", (object)exchgdefaults[12] ?? DBNull.Value);
					cmd2.Parameters.AddWithValue("@ScanInterval", (object)exchgdefaults[0] ?? DBNull.Value);
					cmd2.Parameters.AddWithValue("@RetryInterval", (object)exchgdefaults[2] ?? DBNull.Value);
					cmd2.Parameters.AddWithValue("@OffHourInterval", (object)exchgdefaults[1] ?? DBNull.Value);
					cmd2.Parameters.AddWithValue("@Category", (object)exchgdefaults[13] ?? DBNull.Value);
					cmd2.Parameters.AddWithValue("@CPU_Threshold", (object)exchgdefaults[10] ?? DBNull.Value);
					cmd2.Parameters.AddWithValue("@MemThreshold", (object)exchgdefaults[9] ?? DBNull.Value);
					cmd2.Parameters.AddWithValue("@ResponseTime", (object)exchgdefaults[3] ?? DBNull.Value);
					cmd2.Parameters.AddWithValue("@ConsFailuresBefAlert", (object)exchgdefaults[4] ?? DBNull.Value);
					cmd2.Parameters.AddWithValue("@ConsOvrThresholdBefAlert", (object)exchgdefaults[11] ?? DBNull.Value);
					Insert = objAdaptor.ExecuteNonQuerywithcmd(cmd2);
					if (ServerType == "Exchange")
					{//11-04-2016 Durga Modified for VSPLUS-2742
						SqlQuery = "INSERT INTO [ExchangeSettings] (ServerID,CASSmtp,CASPop3,CASImap,CASOARPC,CASOWA,CASActiveSync,CASEWS,CASECP,CASAutoDiscovery,CASOAB, VersionNo, SubQThreshold, " +
                            " PoisonQThreshold, UnreachableQThreshold, TotalQThreshold,ShadowQThreshold) " +
						   " VALUES(@ServerId,@CASSmtp,@CASPop3,@CASImap,@CASOARPC, @CASOWA, @CASActiveSync, @CASEWS,@CASECP,@CASAutoDiscovery, @CASOAB,@VersionNo, " +
                         " @SubQThreshold,@PoisonQThreshold,@UnreachableQThreshold,@TotalQThreshold,@ShadowQThreshold)";
						SqlCommand cmd3 = new SqlCommand(SqlQuery);
						cmd3.Parameters.AddWithValue("@ServerId", (object)ServerObject.ServerId ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@CASSmtp", (object) 0 ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@CASPop3", (object)0 ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@CASImap", (object)0 ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@CASOARPC", (object)0 ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@CASOWA", (object)0 ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@CASActiveSync", (object)0 ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@CASEWS", (object)0 ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@CASECP", (object)0 ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@CASAutoDiscovery", (object)0?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@CASOAB", (object)0 ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@VersionNo", (object)0 ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@SubQThreshold", (object)exchgdefaults[6] ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@PoisonQThreshold", (object)exchgdefaults[5] ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@UnreachableQThreshold", (object)exchgdefaults[7] ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@TotalQThreshold", (object)exchgdefaults[8] ?? DBNull.Value);
                        cmd3.Parameters.AddWithValue("@ShadowQThreshold", (object)exchgdefaults[21] ?? DBNull.Value);
						Insert = objAdaptor.ExecuteNonQuerywithcmd(cmd3);
						
					}
					if (ServerType == "WebSphere")
                    {
						SqlQuery = "UPDATE  [WebsphereServer] set AvgThreadPool=@AvgThreadPool,ActiveThreadCount=@ActiveThreadCount,CurrentHeap=@CurrentHeap,MaxHeap=@MaxHeap,Uptime=@Uptime,HungThreadCount=@HungThreadCount,DumpGenerated=@DumpGenerated where ServerID=@ServerId";
						SqlCommand cmd3 = new SqlCommand(SqlQuery);
						cmd3.Parameters.AddWithValue("@ServerId", (object)ServerObject.ServerId ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@AvgThreadPool", (object)exchgdefaults[14] ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@ActiveThreadCount", (object)exchgdefaults[15] ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@CurrentHeap", (object)exchgdefaults[16] ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@MaxHeap", (object)exchgdefaults[17] ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@Uptime", (object)exchgdefaults[18] ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@HungThreadCount", (object)exchgdefaults[19] ?? DBNull.Value);
						cmd3.Parameters.AddWithValue("@DumpGenerated", (object)exchgdefaults[20] ?? DBNull.Value);
						Insert = objAdaptor.ExecuteNonQuerywithcmd(cmd3);
                    }
				}

				else
				{
					string SqlQuery = "INSERT INTO [ServerAttributes] (ServerID,Enabled,CredentialsId,ScanInterval,RetryInterval,OffHourInterval,Category,CPU_Threshold,MemThreshold,ResponseTime,ConsFailuresBefAlert,ConsOvrThresholdBefAlert) " +
					   " VALUES(@ServerId,@Enabled,@CredentialsId,@ScanInterval,@RetryInterval,@OffHourInterval"+
						",@Category,@CPU_Threshold,@MemThreshold,@ResponseTime,@ConsFailuresBefAlert,@ConsOvrThresholdBefAlert)";
					SqlCommand cmd3 = new SqlCommand(SqlQuery);
					cmd3.Parameters.AddWithValue("@ServerId", (object)ServerObject.ServerId ?? DBNull.Value);
					cmd3.Parameters.AddWithValue("@Enabled", (object)ServerObject.Enabled ?? DBNull.Value);
					cmd3.Parameters.AddWithValue("@CredentialsId", (object)ServerObject.CredentialsId ?? DBNull.Value);
					cmd3.Parameters.AddWithValue("@ScanInterval", (object)ServerObject.ScanInterval ?? DBNull.Value);
					cmd3.Parameters.AddWithValue("@RetryInterval", (object)ServerObject.RetryInterval ?? DBNull.Value);
					cmd3.Parameters.AddWithValue("@OffHourInterval", (object)ServerObject.OffHourInterval ?? DBNull.Value);
					cmd3.Parameters.AddWithValue("@Category", (object)ServerObject.Category ?? DBNull.Value);
					cmd3.Parameters.AddWithValue("@CPU_Threshold", (object)ServerObject.CPUThreshold ?? DBNull.Value);
					cmd3.Parameters.AddWithValue("@MemThreshold", (object)ServerObject.MemThreshold ?? DBNull.Value);
					cmd3.Parameters.AddWithValue("@ResponseTime", (object)ServerObject.ResponseTime ?? DBNull.Value);
					cmd3.Parameters.AddWithValue("@ConsFailuresBefAlert", (object)ServerObject.ConsFailuresBefAlert ?? DBNull.Value);
					cmd3.Parameters.AddWithValue("@ConsOvrThresholdBefAlert", (object)ServerObject.ConsOvrThresholdBefAlert ?? DBNull.Value);
					Insert = objAdaptor.ExecuteNonQuerywithcmd(cmd3);
					//Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
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

		public bool InsertWebspherAttreibuteData(ServerAttributes ServerObject)
		{

			bool Insert = false;
            try
            {
				string SqlQuery = "INSERT INTO [ServerAttributes] (ServerID,Enabled,CredentialsId,ScanInterval,RetryInterval,OffHourInterval,Category,CPU_Threshold,MemThreshold,ResponseTime,ConsFailuresBefAlert,ConsOvrThresholdBefAlert) " +
					   " VALUES(@ServerId,@Enabled,@CredentialsId,@ScanInterval,@RetryInterval,@OffHourInterval" +
						", @Category,@CPU_Threshold,@MemThreshold,@ResponseTime,@ConsFailuresBefAlert,@ConsOvrThresholdBefAlert)";
				SqlCommand cmd3 = new SqlCommand(SqlQuery);
				cmd3.Parameters.AddWithValue("@ServerId", (object)ServerObject.ServerId ?? DBNull.Value);
				cmd3.Parameters.AddWithValue("@Enabled", (object)ServerObject.Enabled ?? DBNull.Value);
				cmd3.Parameters.AddWithValue("@CredentialsId", (object)ServerObject.CredentialsId ?? DBNull.Value);
				cmd3.Parameters.AddWithValue("@ScanInterval", (object)ServerObject.ScanInterval ?? DBNull.Value);
				cmd3.Parameters.AddWithValue("@RetryInterval", (object)ServerObject.RetryInterval ?? DBNull.Value);
				cmd3.Parameters.AddWithValue("@OffHourInterval", (object)ServerObject.OffHourInterval ?? DBNull.Value);
				cmd3.Parameters.AddWithValue("@Category", (object)ServerObject.Category ?? DBNull.Value);
				cmd3.Parameters.AddWithValue("@CPU_Threshold", (object)ServerObject.CPUThreshold ?? DBNull.Value);
				cmd3.Parameters.AddWithValue("@MemThreshold", (object)ServerObject.MemThreshold ?? DBNull.Value);
				cmd3.Parameters.AddWithValue("@ResponseTime", (object)ServerObject.ResponseTime ?? DBNull.Value);
				cmd3.Parameters.AddWithValue("@ConsFailuresBefAlert", (object)ServerObject.ConsFailuresBefAlert ?? DBNull.Value);
				cmd3.Parameters.AddWithValue("@ConsOvrThresholdBefAlert", (object)ServerObject.ConsOvrThresholdBefAlert ?? DBNull.Value);
				Insert = objAdaptor.ExecuteNonQuerywithcmd(cmd3);
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

        public DataTable GetAllDataByName(Servers ServerObject)
        {

            DataTable ServersDataTable = new DataTable();
            Servers ReturnSerevrbject = new Servers();
            try
            {
                if (ServerObject.ID == 0)
                {

					string SqlQuery = "select Sr.ID,Sr.ServerName,sr.Description,S.ServerType,sa.ScanDAGHealth,sa.CredentialsId,L.Location,sr.ipaddress,sa.Enabled,sa.ScanInterval,sa.RetryInterval,sa.OffHourInterval, " +
                    "sa.category,sa.CPU_Threshold,sa.MemThreshold,sa.ResponseTime,sa.ConsFailuresBefAlert,sa.ConsOvrThresholdBefAlert from Servers Sr inner join ServerTypes S "+
					"on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID  left outer join ServerAttributes sa on sr.ID=sa.serverid where sr.ServerName=@ServerName";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@ServerName", (object)ServerObject.ServerName ?? DBNull.Value);
					ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
                    //ServersDataTable = objAdaptor.FetchData(SqlQuery);
                }
                else
                {

                    string SqlQuery = " select Sr.ID,Sr.ServerName,sr.Description,S.ServerType,S.ScanDAGHealth,L.Location,sr.ipaddress,sa.Enabled,sa.ScanInterval,sa.RetryInterval,sa.OffHourInterval, " +
                    "sa.category,sa.CPU_Threshold,sa.MemThreshold,sa.ResponseTime,sa.ConsFailuresBefAlert,sa.ConsOvrThresholdBefAlert from Servers Sr inner join ServerTypes S "+
					"on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID  left outer join ServerAttributes sa on sr.ID=sa.serverid  and ID<>@ID";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
					ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
					//ServersDataTable = objAdaptor.FetchData(SqlQuery);
                }

            }
            catch
            {
            }
            finally
            {
            }
            return ServersDataTable;
        }

        public DataTable GetAttributes(int ServerId)
        {
            DataTable ServersDataTable = new DataTable();
			string SqlQuery = "select * from ServerAttributes where serverid =@ID" ;
			SqlCommand cmd = new SqlCommand(SqlQuery);
			cmd.Parameters.AddWithValue("@ID", (object)ServerId ?? DBNull.Value);
			ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
         
            return ServersDataTable;
        }

        //CY 5/26
        public bool UpdateServerLocation(int serverId, int locationId,string servertype)
        {
            bool update = false;
			string SqlQuery = "";
            try
            {
				if (servertype == "URL")
				{
					 SqlQuery = "Update URLs set LocationId = @LocationId  where ID = @ID";
					 SqlCommand cmd = new SqlCommand(SqlQuery);
					 cmd.Parameters.AddWithValue("@ID", (object)serverId ?? DBNull.Value);
					 cmd.Parameters.AddWithValue("@LocationId", (object)locationId ?? DBNull.Value);
					 update = objAdaptor.ExecuteNonQuerywithcmd(cmd);
				}
				else
				{
					SqlQuery = "Update Servers set LocationId = @LocationId where ID =@ID ";
					 SqlCommand cmd = new SqlCommand(SqlQuery);
					 cmd.Parameters.AddWithValue("@ID", (object)serverId ?? DBNull.Value);
					 cmd.Parameters.AddWithValue("@LocationId", (object)locationId ?? DBNull.Value);
					 update = objAdaptor.ExecuteNonQuerywithcmd(cmd);
				}
               
            }
            catch
            {
                update = false;
            }
            finally
            {
            }
            return update;
        }
		public bool UpdateServerBusinessHours(int serverId, int BusinesshoursID)
		{
			bool update = false;
			try
			{
				string SqlQuery = "Update Servers set BusinesshoursID = @BusinesshoursID  where ID = @ID";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ID", (object)serverId ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@BusinesshoursID", (object)BusinesshoursID ?? DBNull.Value);
				update = objAdaptor.ExecuteNonQuerywithcmd(cmd);
				//update = objAdaptor.ExecuteNonQuery(SqlQuery);
			}
			catch
			{
				update = false;
			}
			finally
			{
			}
			return update;
		}
        
        public Int32 GetServerIDbyServerNameType(string serverName, string serverType)
        {
            DataTable ServersDataTable = new DataTable();
            int ID = 0;
            try
            {
				string SqlQuery = "SELECT sr.id FROM Servers sr,ServerTypes st where sr.ServerTypeID=st.ID and sr.ServerName=@serverName and st.ServerType=@serverType";

				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@serverName", (object)serverName ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@serverType", (object)serverType ?? DBNull.Value);

				ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
                if (ServersDataTable.Rows.Count>0) ID = Convert.ToInt32(ServersDataTable.Rows[0]["ID"]);
            }
            catch
            {
            }
            finally
            {

            }
            return ID;
        }

        public bool UpdateServerCredentials(int serverId, int credentialsID, string serverType)
        {
            bool update = false;
            try
            {
                string SqlQuery = string.Empty;
              
                    if (serverType.ToLower() == "domino")
                        SqlQuery = "Update DominoServers set CredentialsID = @credentialsID where ServerID = @ServerID";
                    else
                        SqlQuery = "Update ServerAttributes set CredentialsID = @credentialsID where ServerID = @ServerID";						
              
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@ServerID", (object)serverId ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@credentialsID", (object)credentialsID ?? DBNull.Value);
					update = objAdaptor.ExecuteNonQuerywithcmd(cmd);
               
            }
            catch
            {
                update = false;
            }
            finally
            {
            }
            return update;
        }

        //Mukund 10Sep14 VE-70
        public DataTable GetServerDetailsByName1(string serverName)
        {
            DataTable ServersDataTable = new DataTable();
            try
            {


             string SqlQuery = "select Sr.ID,Sr.ServerName,sr.Description,S.ServerType,sa.ScanDAGHealth,sa.CredentialsId,L.Location,sr.ipaddress,sa.Enabled,sa.ScanInterval,sa.RetryInterval,sa.OffHourInterval, " +
                        "sa.category,sa.CPU_Threshold,sa.MemThreshold,sa.ResponseTime,sa.ConsFailuresBefAlert,sa.ConsOvrThresholdBefAlert from Servers Sr inner join ServerTypes S " +
                        "on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID  left outer join ServerAttributes sa on sr.ID=sa.serverid where sr.ServerName=@serverName";
                     SqlCommand cmd = new SqlCommand(SqlQuery);
                    cmd.Parameters.AddWithValue("@serverName", (object)serverName ?? DBNull.Value);

                    ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
                }
            
            catch
            {
            }
            finally
            {

            }
            return ServersDataTable;
        }


        public DataTable GetServerDetailsBytype2(string servertype)
        {
            DataTable ServersDataTable = new DataTable();
            try
            {
				string SqlQuery = "select s.ID,ds.PrimaryConnection,ds.BackupConnection,ds.ReplyQThreshold,ds.CopyQThreshold, s.ServerName,(ServerName+'-'+Location)TypeandLocation from servers s Inner Join ServerTypes st on st.ID=s.ServerTypeID  inner join Locations l on l.ID=s.LocationIDinner join DagSettings ds on ds.ServerID=s.ID WHERE st.ServerType=@serverType";
				
				
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@serverType", (object)servertype ?? DBNull.Value);
				ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
            }
            catch
            {
            }
            finally
            {

            }
            return ServersDataTable;
        }
        public DataTable GetServerDetailsBytype(string servertype)
        {
            DataTable ServersDataTable = new DataTable();
            try
            {
				string SqlQuery = "select s.ID, s.ServerName,(ServerName+'-'+Location)TypeandLocation from servers s Inner Join ServerTypes st on st.ID=s.ServerTypeID  inner join Locations l on l.ID=s.LocationID WHERE st.ServerType=@serverType";

				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@serverType", (object)servertype ?? DBNull.Value);
				ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
            }
            catch
            {
            }
            finally
            {

            }
            return ServersDataTable;
        }

		public DataTable GetServerNameinwebsphereservers(string Name)
		{
			//SametimeServers SametimeObj = new SametimeServers();
			DataTable CsTable = new DataTable();
			try
			{

				string sqlQuery = "Select * from WebsphereServer where ServerName=@ServerName";

				SqlCommand cmd = new SqlCommand(sqlQuery);
				cmd.Parameters.AddWithValue("@ServerName", (object)Name ?? DBNull.Value);
				CsTable = objAdaptor.FetchDatafromcommand(cmd);

			}
			catch (Exception)
			{

				throw;
			}
			return CsTable;

		}
		public DataTable GetServerIdinserverattribute(int id)
		{
			//SametimeServers SametimeObj = new SametimeServers();
			DataTable CsTable = new DataTable();
			try
			{

				string sqlQuery = "Select * from ServerAttributes where ServerID=@ServerID";

				SqlCommand cmd = new SqlCommand(sqlQuery);
				cmd.Parameters.AddWithValue("@ServerID", (object)id ?? DBNull.Value);
				CsTable = objAdaptor.FetchDatafromcommand(cmd);

			}
			catch (Exception)
			{

				throw;
			}
			return CsTable;

		}
		public Object UpdateServersProfileName(ProfileNames ProfileNamesObj)
          {
			Object Update;
			try
			{
				string SqlQuery = "UPDATE Servers SET ProfileName=@ProfileName where ProfileName=@ProfileId";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ProfileName", "0");
				cmd.Parameters.AddWithValue("@ProfileId", (object)ProfileNamesObj.ID??DBNull.Value);
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


		public Object SafeToDeleteServer(Servers ServerObject)
		{ 
			try
			{
				switch (ServerObject.ServerType)
				{

					case "Domino":
						if (objAdaptor.FetchData("select * from DominoCluster where " + ServerObject.ID + " in (ServerID_A, ServerID_B, ServerID_C)").Rows.Count > 0)
							return "This server is used in a Domino Cluster.";
						break;

					default:

						break;
				}
			}
			catch
			{
				return "";
				//Update= "This Server Is Using Somewhere, Cannot Delete. ";
			}
			finally
			{
			}
			return true;
		}
        //5/3/2016 Sowjanya added for VSPLUS-2896
        public DataTable GetServerDetailsByWSName(string serverName)
        {
            DataTable ServersDataTable = new DataTable();
            try
            {

                  string SqlQuery = "select NodeName,CellName from WebsphereServer ws  inner join " +
                                   "WebsphereCell wc on wc.CellID=ws.CellID inner join WebsphereNode wn " +
                                   "on wn.NodeID=ws.NodeID where  ServerName =@serverName";
   
                    SqlCommand cmd = new SqlCommand(SqlQuery);
                    cmd.Parameters.AddWithValue("@serverName", (object)serverName ?? DBNull.Value);

                    ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
              
            }
            catch
            {
            }
            finally
            {

            }
            return ServersDataTable;
        }

    }
}
