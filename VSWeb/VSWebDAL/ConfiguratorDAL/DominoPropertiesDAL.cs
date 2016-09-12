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
    public class DominoPropertiesDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static DominoPropertiesDAL _self = new DominoPropertiesDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static DominoPropertiesDAL Ins
        {
            get { return _self; }
        }
        /// <summary>
        /// Get all Data from DominoServers 
        /// </summary>
        /// <returns></returns>
		public DataTable GetAllDataforuserrestrictions(int Userid)
		{

			DataTable DominoServersDataTable = new DataTable();
			DominoServers ReturnDSObject = new DominoServers();
			try
			{

				//string Query="Select serverid,LocationID from UserLocationRestrictions inner join UserServerRestrictions on UserLocationRestrictions.UserID = UserServerRestrictions.UserID where UserID="++"";
				//12/16/2013 NS added sort by name ASC
				string SqlQuery = "SELECT DiskSpaceThreshold, BES_Server, BES_Threshold, DominoServers.Category, DeadThreshold, " +
					"FailureThreshold,DominoServers.Enabled,MailDirectory,ServerID,OffHoursScanInterval, PendingThreshold, " +
					"ResponseThreshold, DominoServers.RetryInterval, [Scan Interval], SearchString, AdvancedMailScan," +
					"DeadMailDeleteThreshold,HeldThreshold, ScanDBHealth, NotificationGroup, Memory_Threshold, " +
					"DominoServers.CPU_Threshold, Cluster_Rep_Delays_Threshold, Cluster_Rep_Delays_Threshold, description,servername as Name,ipaddress,locationid, " +
					"(select Location from Locations where ID = servers.LocationID) Location,ID from Servers " +
					"left join DominoServers on servers.ID=DominoServers.serverID where " +
					"servers.ServerTypeID=(select ID from ServerTypes where ServerType='Domino') " +
					"and servers.ID not in(select serverID from UserServerRestrictions ur inner join Users U on ur.UserID= U.ID where U.ID=" + Userid + ")" +
					"order by servername ";

				//string SqlQuery = "SELECT DiskSpaceThreshold, BES_Server, BES_Threshold, Category, DeadThreshold, Description, Enabled, FailureThreshold, [Key], Location, MailDirectory, Name, " +
				//                            "OffHoursScanInterval, PendingThreshold, ResponseThreshold, RetryInterval, [Scan Interval], SearchString, AdvancedMailScan, DeadMailDeleteThreshold, " +
				//               " IPAddress, HeldThreshold, ScanDBHealth, NotificationGroup, Memory_Threshold, CPU_Threshold, Cluster_Rep_Delays_Threshold  FROM DominoServers" +
				//               " union " +
				//                "SELECT '', '', '', '', '', Description, '', '', 0, '', '', ServerName as Name, " +
				//                "'', '', '', '', '', '', '', '', " +
				//                "IPAddress, '', '', '', '', '', '' FROM Servers where ServerName not in (SELECT distinct Name FROM DominoServers)";

				DominoServersDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return DominoServersDataTable;
		}
		public DataTable GetAllData()
		{

			DataTable DominoServersDataTable = new DataTable();
			DominoServers ReturnDSObject = new DominoServers();
			try
			{

				//string Query="Select serverid,LocationID from UserLocationRestrictions inner join UserServerRestrictions on UserLocationRestrictions.UserID = UserServerRestrictions.UserID where UserID="++"";
				//12/16/2013 NS added sort by name ASC
				string SqlQuery = "SELECT DiskSpaceThreshold, BES_Server, BES_Threshold, DominoServers.Category, DeadThreshold, " +
					"FailureThreshold,DominoServers.Enabled,MailDirectory,ServerID,OffHoursScanInterval, PendingThreshold, " +
					"ResponseThreshold, DominoServers.RetryInterval, [Scan Interval], SearchString, AdvancedMailScan," +
					"DeadMailDeleteThreshold,HeldThreshold, ScanDBHealth, NotificationGroup, Memory_Threshold, " +
                    "DominoServers.CPU_Threshold, Cluster_Rep_Delays_Threshold, Cluster_Rep_Delays_Threshold, description,servername as Name,ipaddress,locationid, " +
					"(select Location from Locations where ID = servers.LocationID) Location,ID from Servers " +
					"left join DominoServers on servers.ID=DominoServers.serverID where " +
					"servers.ServerTypeID=(select ID from ServerTypes where ServerType='Domino') " +
					
					"order by servername ";

				//string SqlQuery = "SELECT DiskSpaceThreshold, BES_Server, BES_Threshold, Category, DeadThreshold, Description, Enabled, FailureThreshold, [Key], Location, MailDirectory, Name, " +
				//                            "OffHoursScanInterval, PendingThreshold, ResponseThreshold, RetryInterval, [Scan Interval], SearchString, AdvancedMailScan, DeadMailDeleteThreshold, " +
				//               " IPAddress, HeldThreshold, ScanDBHealth, NotificationGroup, Memory_Threshold, CPU_Threshold, Cluster_Rep_Delays_Threshold  FROM DominoServers" +
				//               " union " +
				//                "SELECT '', '', '', '', '', Description, '', '', 0, '', '', ServerName as Name, " +
				//                "'', '', '', '', '', '', '', '', " +
				//                "IPAddress, '', '', '', '', '', '' FROM Servers where ServerName not in (SELECT distinct Name FROM DominoServers)";

				DominoServersDataTable = objAdaptor.FetchData(SqlQuery);

			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return DominoServersDataTable;
		}

        /// <summary>
        /// Get Data from DominoServers based on Key
        /// </summary>
        /// <param name="DSObject">DominoServers object</param>
        /// <returns></returns>
        /// 
        public DataTable DSTaskSettingsUpdateGrid(string ServerKey)
        {
            DataTable DSTaskSettingsServersDataTable = new DataTable();
            try
            {
                string SqlQuery = " Select sts.Enabled, sts.SendLoadCommand,sts.MinNoOfTasks,sts.IsMinTasksEnabled,sts.SendMinTasksLoadCmd, sts.SendRestartCommand, sts.RestartOffHours, " +
                    "sts.SendExitCommand, dst.TaskName, sts.MyID  FROM " +
                    "ServerTaskSettings sts INNER JOIN DominoServerTasks dst ON sts.TaskID = dst.TaskID " +
                    "Where sts.ServerID = " + ServerKey;

                DSTaskSettingsServersDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return DSTaskSettingsServersDataTable;
        }

        public DominoServers GetData(DominoServers DSObject)
        {
            DataTable DominoServersDataTable = new DataTable();
            DominoServers ReturnDSObject = new DominoServers();

            try
            {
                //string SqlQuery = "select *,ISNULL(RequireSSL,0) RS,ServerName as Name,IPAddress,LocationID,Description,servers.ID,Location from Servers left join dominoservers on Servers.ID=DominoServers.ServerID inner join Locations on servers.LocationID=Locations.ID where servers.ID=" + DSObject.Key + "";
                //8/7/2014 NS modified for VSPLUS-853
                //6/19/2015 NS modified for VSPLUS-1802
                //2/23/2016 NS modified for VAPLUA-2641
                string SqlQuery = "select BES_Server,ISNULL(BES_Threshold,0) BES_Threshold,Category,ISNULL(DeadThreshold,0) DeadThreshold, " +
                    "Description,ISNULL(Enabled,0) Enabled,ISNULL(CheckMailThreshold,0) CheckMailThreshold, ExternalAlias,ISNULL(FailureThreshold,2) FailureThreshold, " +
                    "Location,ISNULL(LocationID,0) LocationID,MailDirectory,ISNULL(OffHoursScanInterval,15) OffHoursScanInterval, " +
                    "ISNULL(PendingThreshold,200) PendingThreshold,ISNULL(ResponseThreshold,2500) ResponseThreshold, " +
                    "ISNULL(RetryInterval,2) RetryInterval,ISNULL([Scan Interval],8) [Scan Interval], " +
                    "ISNULL(DeadMailDeleteThreshold,0) DeadMailDeleteThreshold,IPAddress,ISNULL(HeldThreshold,200) HeldThreshold, " +
                    "ISNULL(ScanDBHealth,0) ScanDBHealth,ISNULL(Scanagentlog,0) Scanagentlog,ISNULL(Scanlog,0) Scanlog,  NotificationGroup,ISNULL(Memory_Threshold,0.9) Memory_Threshold, " +
                    "ISNULL(CPU_Threshold,0.9) CPU_Threshold,ISNULL(Cluster_Rep_Delays_Threshold,300) Cluster_Rep_Delays_Threshold, ISNULL(Load_Cluster_Rep_Delays_Threshold,300) Load_Cluster_Rep_Delays_Threshold, " +
                    "ISNULL(SendRouterRestart,0) SendRouterRestart,ServerID,ISNULL(ServerDaysAlert,90) ServerDaysAlert,ISNULL(RequireSSL,0) RS,ISNULL(EnableTravelerBackend,0) ETB, ServerName as Name, " +
					"IPAddress,LocationID,Description,servers.ID,Location,CredentialsID, ISNULL(DominoServers.ScanServlet, 1) ScanServlet,ISNULL(DominoServers.ScanTravelerServer, 1) ScanTravelerServer, " +
                    "EXJEnabled, ISNULL(EXJDuration,0) EXJDuration, ISNULL(EXJLookBackDuration,0) EXJLookBackDuration, ISNULL(EXJStartTime,'') EXJStartTime, " +
                    "ISNULL(AvailabilityIndexThreshold,0) AvailabilityIndexThreshold " + 
                    "from Servers left join dominoservers on " +
                    "Servers.ID=DominoServers.ServerID inner join Locations on servers.LocationID=Locations.ID " +
                    "where servers.ID=" + DSObject.Key + "";

                DominoServersDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object

                if (DominoServersDataTable.Rows.Count > 0)
                {
                    if (DominoServersDataTable.Rows[0]["BES_Server"].ToString() != "" && DominoServersDataTable.Rows[0]["NotificationGroup"] != null && DominoServersDataTable.Rows[0]["OffHoursScanInterval"] != null)
                    {
                        bool value;
                        //Commented by Mukund 06Jun14:Error this parameter is not taken now. Disk space is separately dealt
                        //ReturnDSObject.DiskSpaceThreshold = float.Parse(DominoServersDataTable.Rows[0]["DiskSpaceThreshold"].ToString());
                        ReturnDSObject.BES_Server = bool.Parse(DominoServersDataTable.Rows[0]["BES_Server"].ToString());
                        ReturnDSObject.BES_Threshold = int.Parse(DominoServersDataTable.Rows[0]["BES_Threshold"].ToString());
                        ReturnDSObject.Category = DominoServersDataTable.Rows[0]["Category"].ToString();
                        ReturnDSObject.DeadThreshold = int.Parse(DominoServersDataTable.Rows[0]["DeadThreshold"].ToString());
                        ReturnDSObject.Name = DominoServersDataTable.Rows[0]["Description"].ToString();
                        ReturnDSObject.Enabled = bool.Parse(DominoServersDataTable.Rows[0]["Enabled"].ToString());

                        int msg = Convert.ToInt32(DominoServersDataTable.Rows[0]["CheckMailThreshold"]);
                        if (msg == 1)
                        {
                            ReturnDSObject.CheckMailThreshold = true;
                        }
                        else
                        {
                            ReturnDSObject.CheckMailThreshold = false;
                        }
                        ReturnDSObject.RequireSSL = bool.Parse(DominoServersDataTable.Rows[0]["RS"].ToString());
						ReturnDSObject.EnableTravelerBackend = bool.Parse(DominoServersDataTable.Rows[0]["ETB"].ToString());
                        ReturnDSObject.ExternalAlias = DominoServersDataTable.Rows[0]["ExternalAlias"].ToString();
                        ReturnDSObject.FailureThreshold = int.Parse(DominoServersDataTable.Rows[0]["FailureThreshold"].ToString());
                        ReturnDSObject.Location = DominoServersDataTable.Rows[0]["Location"].ToString();
                        ReturnDSObject.LocationID = int.Parse(DominoServersDataTable.Rows[0]["LocationID"].ToString());
                        ReturnDSObject.MailDirectory = DominoServersDataTable.Rows[0]["MailDirectory"].ToString();
                        ReturnDSObject.Name = DominoServersDataTable.Rows[0]["Name"].ToString();
                        ReturnDSObject.OffHoursScanInterval = int.Parse(DominoServersDataTable.Rows[0]["OffHoursScanInterval"].ToString());
                        ReturnDSObject.PendingThreshold = int.Parse(DominoServersDataTable.Rows[0]["PendingThreshold"].ToString());
                        ReturnDSObject.ResponseThreshold = int.Parse(DominoServersDataTable.Rows[0]["ResponseThreshold"].ToString());
                        ReturnDSObject.RetryInterval = int.Parse(DominoServersDataTable.Rows[0]["RetryInterval"].ToString());
                        ReturnDSObject.ScanInterval = int.Parse(DominoServersDataTable.Rows[0]["Scan Interval"].ToString());
                        ReturnDSObject.SearchString = DominoServersDataTable.Rows[0]["Scan Interval"].ToString();
                        ReturnDSObject.DeadMailDeleteThreshold = int.Parse(DominoServersDataTable.Rows[0]["DeadMailDeleteThreshold"].ToString());
                        ReturnDSObject.IPAddress = DominoServersDataTable.Rows[0]["IPAddress"].ToString();
                        ReturnDSObject.HeldThreshold = int.Parse(DominoServersDataTable.Rows[0]["HeldThreshold"].ToString());
                        ReturnDSObject.ScanDBHealth = bool.Parse(DominoServersDataTable.Rows[0]["ScanDBHealth"].ToString());
                        ReturnDSObject.scanlog = bool.Parse(DominoServersDataTable.Rows[0]["scanlog"].ToString());
                        ReturnDSObject.scanagentlog = bool.Parse(DominoServersDataTable.Rows[0]["scanagentlog"].ToString());
                        ReturnDSObject.NotificationGroup = DominoServersDataTable.Rows[0]["NotificationGroup"].ToString();
                        ReturnDSObject.Memory_Threshold = float.Parse(DominoServersDataTable.Rows[0]["Memory_Threshold"].ToString());
                        ReturnDSObject.CPU_Threshold = float.Parse(DominoServersDataTable.Rows[0]["CPU_Threshold"].ToString());
                        ReturnDSObject.Cluster_Rep_Delays_Threshold = float.Parse(DominoServersDataTable.Rows[0]["Cluster_Rep_Delays_Threshold"].ToString());
                        ReturnDSObject.ServerID = int.Parse(DominoServersDataTable.Rows[0]["ServerID"].ToString());
                        ReturnDSObject.Key = Convert.ToInt16(DominoServersDataTable.Rows[0]["ID"].ToString());
                        ReturnDSObject.SendRouterRestart = bool.Parse(DominoServersDataTable.Rows[0]["SendRouterRestart"].ToString());
                        ReturnDSObject.ServerDaysAlert = int.Parse(DominoServersDataTable.Rows[0]["ServerDaysAlert"].ToString());
                        ReturnDSObject.Load_Cluster_Rep_Delays_Threshold = float.Parse(DominoServersDataTable.Rows[0]["Load_Cluster_Rep_Delays_Threshold"].ToString());
                        //ReturnDSObject.Modified_By = Convert.ToInt16(DominoServersDataTable.Rows[0]["Modified_By"].ToString());
                        //ReturnDSObject.Modified_On = DominoServersDataTable.Rows[0]["Modified_On"].ToString();
                        //8/7/2014 NS added for VSPLUS-853
						if (DominoServersDataTable.Rows[0]["CredentialsID"].ToString() == "")
						{
							ReturnDSObject.CredentialsID = -1;
						}
						else
						{
							ReturnDSObject.CredentialsID = int.Parse(DominoServersDataTable.Rows[0]["CredentialsID"].ToString());
						}
						ReturnDSObject.ScanServlet = bool.Parse(DominoServersDataTable.Rows[0]["ScanServlet"].ToString());
                        //6/19/2015 NS added for VSPLUS-1802
						if (DominoServersDataTable.Rows[0]["EXJEnabled"].ToString() != "")
						{
							ReturnDSObject.EXJEnabled = bool.Parse(DominoServersDataTable.Rows[0]["EXJEnabled"].ToString());
						}
						if (DominoServersDataTable.Rows[0]["EXJDuration"].ToString() != "")
						{
							ReturnDSObject.EXJDuration = int.Parse(DominoServersDataTable.Rows[0]["EXJDuration"].ToString());
						}
						if (DominoServersDataTable.Rows[0]["EXJLookBackDuration"].ToString() != "")
						{
							ReturnDSObject.EXJLookBackDuration = int.Parse(DominoServersDataTable.Rows[0]["EXJLookBackDuration"].ToString());
						}

                        ReturnDSObject.EXJStartTime = DominoServersDataTable.Rows[0]["EXJStartTime"].ToString();
						ReturnDSObject.ScanTravelerServer = bool.Parse(DominoServersDataTable.Rows[0]["ScanTravelerServer"].ToString());
                        ReturnDSObject.AvailabilityIndexThreshold = int.Parse(DominoServersDataTable.Rows[0]["AvailabilityIndexThreshold"].ToString());
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
            }
            //try
            //{
            //    string SqlQuery = "Select * from DominoServers where [Key]=" + DSObject.Key.ToString();
            //    DominoServersDataTable = objAdaptor.FetchData(SqlQuery);
            //    //populate & return data object
            //    ReturnDSObject.DiskSpaceThreshold =float.Parse(DominoServersDataTable.Rows[0]["DiskSpaceThreshold"].ToString());
            //    ReturnDSObject.BES_Server = bool.Parse(DominoServersDataTable.Rows[0]["BES_Server"].ToString());
            //    ReturnDSObject.BES_Threshold = int.Parse(DominoServersDataTable.Rows[0]["BES_Threshold"].ToString());
            //    ReturnDSObject.Category = DominoServersDataTable.Rows[0]["Category"].ToString();
            //    ReturnDSObject.DeadThreshold = int.Parse(DominoServersDataTable.Rows[0]["DeadThreshold"].ToString());
            //    ReturnDSObject.Name = DominoServersDataTable.Rows[0]["Description"].ToString();
            //    ReturnDSObject.Enabled = bool.Parse(DominoServersDataTable.Rows[0]["Enabled"].ToString());
            //    ReturnDSObject.FailureThreshold = int.Parse(DominoServersDataTable.Rows[0]["FailureThreshold"].ToString());
            //    ReturnDSObject.Location = DominoServersDataTable.Rows[0]["Location"].ToString();
            //    ReturnDSObject.MailDirectory = DominoServersDataTable.Rows[0]["MailDirectory"].ToString();
            //    ReturnDSObject.Name = DominoServersDataTable.Rows[0]["Name"].ToString();
            //    ReturnDSObject.OffHoursScanInterval = int.Parse(DominoServersDataTable.Rows[0]["OffHoursScanInterval"].ToString());
            //    ReturnDSObject.PendingThreshold = int.Parse(DominoServersDataTable.Rows[0]["PendingThreshold"].ToString());
            //    ReturnDSObject.ResponseThreshold = int.Parse(DominoServersDataTable.Rows[0]["ResponseThreshold"].ToString());
            //    ReturnDSObject.RetryInterval = int.Parse(DominoServersDataTable.Rows[0]["RetryInterval"].ToString());
            //    ReturnDSObject.ScanInterval = int.Parse(DominoServersDataTable.Rows[0]["Scan Interval"].ToString());
            //    ReturnDSObject.SearchString= DominoServersDataTable.Rows[0]["Scan Interval"].ToString();
            //    ReturnDSObject.DeadMailDeleteThreshold = int.Parse(DominoServersDataTable.Rows[0]["DeadMailDeleteThreshold"].ToString());
            //    ReturnDSObject.IPAddress = DominoServersDataTable.Rows[0]["IPAddress"].ToString();
            //    ReturnDSObject.HeldThreshold = int.Parse(DominoServersDataTable.Rows[0]["HeldThreshold"].ToString());
            //    ReturnDSObject.ScanDBHealth = bool.Parse(DominoServersDataTable.Rows[0]["ScanDBHealth"].ToString());
            //    ReturnDSObject.NotificationGroup = DominoServersDataTable.Rows[0]["NotificationGroup"].ToString();
            //    ReturnDSObject.Memory_Threshold = float.Parse(DominoServersDataTable.Rows[0]["Memory_Threshold"].ToString());
            //    ReturnDSObject.CPU_Threshold=float.Parse(DominoServersDataTable.Rows[0]["CPU_Threshold"].ToString()) ;
            //    ReturnDSObject.Cluster_Rep_Delays_Threshold=float.Parse(DominoServersDataTable.Rows[0]["Cluster_Rep_Delays_Threshold"].ToString());
            //    ReturnDSObject.Modified_By = int.Parse(DominoServersDataTable.Rows[0]["Modified_By"].ToString());
            //    ReturnDSObject.Modified_On = DominoServersDataTable.Rows[0]["Modified_On"].ToString();
            //}
            catch(Exception ex)
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
        public bool InsertData(DominoServers DSObject)
        {

            bool Insert = false;
            try
            {

                //8/7/2014 NS modified for VSPLUS-853
                //6/19/2015 NS modified for VSPLUS-1802
                //2/23/2016 NS modified for VSPLUS-2641
                string SqlQuery = "INSERT INTO DominoServers (DiskSpaceThreshold, BES_Server, BES_Threshold, Category, DeadThreshold, " +
					"Description, Enabled, RequireSSL,EnableTravelerBackend, ExternalAlias, FailureThreshold, Location, MailDirectory, Name, OffHoursScanInterval, PendingThreshold, " +
                    "ResponseThreshold, RetryInterval, [Scan Interval], SearchString, AdvancedMailScan, DeadMailDeleteThreshold, " +
                    "IPAddress, HeldThreshold, ScanDBHealth, NotificationGroup, Memory_Threshold, CPU_Threshold, " +
                    "Cluster_Rep_Delays_Threshold, Load_Cluster_Rep_Delays_Threshold,Modified_By,Modified_On,ServerDaysAlert,CredentialsID, ScanServlet, " +
                    "EXJStartTime,EXJDuration,EXJLookBackDuration,EXJEnabled,ScanTravelerServer,AvailabilityIndexThreshold) " +
                  " VALUES (" + DSObject.DiskSpaceThreshold + ", '" + DSObject.BES_Server +
                  "', " + DSObject.BES_Threshold + ", '" + DSObject.Category +
                  "', " + DSObject.DeadThreshold +
				  "', '" + DSObject.Enabled + "','" + DSObject.RequireSSL + "','" + DSObject.EnableTravelerBackend + "', '" + DSObject.ExternalAlias + "', " + DSObject.FailureThreshold +
                  ", '" + DSObject.LocationID + "', '" + DSObject.MailDirectory +
                  "',  " + DSObject.OffHoursScanInterval +
                  ", " + DSObject.PendingThreshold + ", " + DSObject.ResponseThreshold +
                  ", " + DSObject.RetryInterval + ", " + DSObject.ScanInterval +
                  ", '" + DSObject.SearchString + "', 0, " + DSObject.DeadMailDeleteThreshold +
                  ",  " + DSObject.HeldThreshold +
                  ", '" + DSObject.ScanDBHealth + "', '" + DSObject.NotificationGroup +
                  "', " + DSObject.Memory_Threshold + ", " + DSObject.CPU_Threshold +
                  ", " + DSObject.Cluster_Rep_Delays_Threshold +
                  ", " + DSObject.Load_Cluster_Rep_Delays_Threshold +
                ", " + DSObject.ServerDaysAlert +
                ", " + DSObject.CredentialsID +
                ", " + DSObject.ScanServlet +
                ", '" + DSObject.EXJStartTime + "'" +
                ", " + DSObject.EXJDuration +
                ", " + DSObject.EXJLookBackDuration +
                ", " + Convert.ToInt32(DSObject.EXJEnabled) +
				", '" +DSObject.ScanTravelerServer + "'" +
                ", " + DSObject.AvailabilityIndexThreshold + 
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
        public Object UpdateData(DominoServers DSObject)
        {
            Object Update;
            int val;
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString());
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter("select serverID from Dominoservers", con);
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
                        if (DSObject.CheckMailThreshold == true)
                        {
                            val = 1;
                        }
                        else
                        {
                            val = 0;
                        }
                        //8/7/2014 NS modified for VSPLUS-853
                        //6/19/2015 NS modified for VSPLUS-1802
                        //2/23/2016 NS modified for VSPLUS-2641
                        string SqlQuery = "UPDATE DominoServers SET DiskSpaceThreshold = " + DSObject.DiskSpaceThreshold +
                            ", BES_Server = '" + DSObject.BES_Server + "', BES_Threshold = " + DSObject.BES_Threshold +
                            ", Category = '" + DSObject.Category + "' , DeadThreshold = " + DSObject.DeadThreshold +
                             ", Enabled = '" + DSObject.Enabled +
                               "',CheckMailThreshold = '" + val +
                               "', RequireSSL = '" + DSObject.RequireSSL +
							   "',  EnableTravelerBackend = '" + DSObject.EnableTravelerBackend +
							   "',     ExternalAlias = '" + DSObject.ExternalAlias +
                            "', FailureThreshold = " + DSObject.FailureThreshold +
                            ", MailDirectory = '" + DSObject.MailDirectory + "', OffHoursScanInterval = " + DSObject.OffHoursScanInterval +
                            ", PendingThreshold = " + DSObject.PendingThreshold + ", ResponseThreshold = " + DSObject.ResponseThreshold +
                            ", RetryInterval = " + DSObject.RetryInterval + ", [Scan Interval] = " + DSObject.ScanInterval +
                            ", SearchString = '" + DSObject.SearchString + "', DeadMailDeleteThreshold = " + DSObject.DeadMailDeleteThreshold +
                             ", HeldThreshold = " + DSObject.HeldThreshold +
                            ", ScanDBHealth = '" + DSObject.ScanDBHealth + "', NotificationGroup = '" + DSObject.NotificationGroup +
                            "', Memory_Threshold = " + DSObject.Memory_Threshold + ", CPU_Threshold = " + DSObject.CPU_Threshold +
                            ", Cluster_Rep_Delays_Threshold = " + DSObject.Cluster_Rep_Delays_Threshold + ", Load_Cluster_Rep_Delays_Threshold = " + DSObject.Load_Cluster_Rep_Delays_Threshold +
                            ", ServerDaysAlert = " + DSObject.ServerDaysAlert + ", SendRouterRestart = '" + DSObject.SendRouterRestart + "', scanlog = '" + DSObject.scanlog + "', scanagentlog = '" + DSObject.scanagentlog + "'" +
							(DSObject.CredentialsID != -1 ? ", CredentialsID = " + DSObject.CredentialsID : ", CredentialsID = NULL") +
                           ", ScanServlet = '" + DSObject.ScanServlet + "' " +
                           ", EXJStartTime='" + DSObject.EXJStartTime + "' " +
                           ", EXJDuration=" + DSObject.EXJDuration + 
                           ", EXJLookBackDuration=" + DSObject.EXJLookBackDuration + 
                           ", EXJEnabled=" + Convert.ToInt32(DSObject.EXJEnabled) +
						   ", ScanTravelerServer = '" + DSObject.ScanTravelerServer + "' " +
                           ", AvailabilityIndexThreshold=" + DSObject.AvailabilityIndexThreshold + 
                           " WHERE ServerID =" + DSObject.Key + "";

                        //VSPLUS-896: Mukund 28Aug14,In sqlQuery, Credentials is giving FK constraint err. Adding that if selected in UI.

                        Update = objAdaptor.ExecuteNonQuery(SqlQuery);
                    }
                    else
                    {
                        if (DSObject.CheckMailThreshold == true)
                        {
                            val = 1;
                        }
                        else
                        {
                            val = 0;
                        }
                        string SqlQuery = "INSERT INTO DominoServers (ServerID,DiskSpaceThreshold, BES_Server, BES_Threshold, Category, DeadThreshold, " +
				   "Enabled,CheckMailThreshold, RequireSSL,EnableTravelerBackend, ExternalAlias, FailureThreshold, MailDirectory, OffHoursScanInterval, PendingThreshold, " +
                   "ResponseThreshold, RetryInterval, [Scan Interval], SearchString, AdvancedMailScan, DeadMailDeleteThreshold, " +
                   "HeldThreshold, ScanDBHealth, NotificationGroup, Memory_Threshold, CPU_Threshold, Cluster_Rep_Delays_Threshold, Load_Cluster_Rep_Delays_Threshold, ServerDaysAlert, " +
                   "EXJStartTime,EXJDuration,EXJLookBackDuration,EXJEnabled,ScanTravelerServer,AvailabilityIndexThreshold) " +
                 " VALUES (" + DSObject.Key + "," + DSObject.DiskSpaceThreshold + ", '" + DSObject.BES_Server +
                 "', " + DSObject.BES_Threshold + ", '" + DSObject.Category +
                 "', " + DSObject.DeadThreshold +
				 ", '" + DSObject.Enabled + "','" + val + "','" + DSObject.RequireSSL + "','" + DSObject.EnableTravelerBackend + "',  '" + DSObject.ExternalAlias + "', " + DSObject.FailureThreshold +
                 ", '" + DSObject.MailDirectory +
                 "',  " + DSObject.OffHoursScanInterval +
                 ", " + DSObject.PendingThreshold + ", " + DSObject.ResponseThreshold +
                 ", " + DSObject.RetryInterval + ", " + DSObject.ScanInterval +
                 ", '" + DSObject.SearchString + "', 0, " + DSObject.DeadMailDeleteThreshold +
                 ",  " + DSObject.HeldThreshold +
                 ", '" + DSObject.ScanDBHealth + "', '" + DSObject.NotificationGroup +
                 "', " + DSObject.Memory_Threshold + ", " + DSObject.CPU_Threshold +
                 ", " + DSObject.Cluster_Rep_Delays_Threshold +
                 ", " + DSObject.Load_Cluster_Rep_Delays_Threshold +
                 ", " + DSObject.ServerDaysAlert +
                 ", '" + DSObject.EXJStartTime + "'" +
                ", " + DSObject.EXJDuration +
                ", " + DSObject.EXJLookBackDuration +
                ", " + Convert.ToInt32(DSObject.EXJEnabled) +
				", '" + DSObject.ScanTravelerServer + "'" +
                ", " + DSObject.AvailabilityIndexThreshold + 
                 ")";


                        Update = objAdaptor.ExecuteNonQuery(SqlQuery);
                    }
                }
                else
                {
                    if (DSObject.CheckMailThreshold == true)
                    {
                        val = 1;
                    }
                    else
                    {
                        val = 0;
                    }
                    string SqlQuery = "INSERT INTO DominoServers (ServerID,DiskSpaceThreshold, BES_Server, BES_Threshold, Category, DeadThreshold, " +
				   "Enabled, RequireSSL,EnableTravelerBackend, CheckMailThreshold,ExternalAlias, FailureThreshold, MailDirectory, OffHoursScanInterval, PendingThreshold, " +
                   "ResponseThreshold, RetryInterval, [Scan Interval], SearchString, AdvancedMailScan, DeadMailDeleteThreshold, " +
                   "HeldThreshold, ScanDBHealth, NotificationGroup, Memory_Threshold, CPU_Threshold, Cluster_Rep_Delays_Threshold, Load_Cluster_Rep_Delays_Threshold, " +
                   "EXJStartTime,EXJDuration,EXJLookBackDuration,EXJEnabled,AvailabilityIndexThreshold) " +
                 " VALUES (" + DSObject.Key + "," + DSObject.DiskSpaceThreshold + ", '" + DSObject.BES_Server +
                 "', " + DSObject.BES_Threshold + ", '" + DSObject.Category +
                 "', " + DSObject.DeadThreshold +
				 ", '" + DSObject.Enabled + "','" + val + "','" + DSObject.RequireSSL + "','" + DSObject.EnableTravelerBackend + "', '" + DSObject.ExternalAlias + "', " + DSObject.FailureThreshold +
                 ", '" + DSObject.MailDirectory +
                 "',  " + DSObject.OffHoursScanInterval +
                 ", " + DSObject.PendingThreshold + ", " + DSObject.ResponseThreshold +
                 ", " + DSObject.RetryInterval + ", " + DSObject.ScanInterval +
                 ", '" + DSObject.SearchString + "', 0, " + DSObject.DeadMailDeleteThreshold +
                 ",  " + DSObject.HeldThreshold +
                 ", '" + DSObject.ScanDBHealth + "', '" + DSObject.NotificationGroup +
                 "', " + DSObject.Memory_Threshold + ", " + DSObject.CPU_Threshold +
                 ", " + DSObject.Cluster_Rep_Delays_Threshold +
                 ", " + DSObject.Load_Cluster_Rep_Delays_Threshold +
                 ", '" + DSObject.EXJStartTime + "'" +
                ", " + DSObject.EXJDuration +
                ", " + DSObject.EXJLookBackDuration +
                ", " + Convert.ToInt32(DSObject.EXJEnabled) + 
                ", " + DSObject.AvailabilityIndexThreshold + 
                 ")";


                    Update = objAdaptor.ExecuteNonQuery(SqlQuery);

                }
            }
            catch (Exception ex)
            {
                Update = false;
                //7/29/2014 NS added for VSPLUS-634
                WriteHistoryEntry(DateTime.Now.ToString() + " Exception - " + ex);
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
        //public DataTable DSTaskSettingsUpdateGrid(string ServerKey)
        //{
        //    DataTable DSTaskSettingsServersDataTable = new DataTable();
        //    try
        //    {
        //        string SqlQuery = " Select sts.Enabled, sts.SendLoadCommand, sts.SendRestartCommand, sts.RestartOffHours, " +
        //            "sts.SendExitCommand, dst.TaskName, sts.MyID  FROM " +
        //            "ServerTaskSettings sts INNER JOIN DominoServerTasks dst ON sts.TaskID = dst.TaskID " +
        //            "Where sts.ServerID = " + ServerKey;

        //        DSTaskSettingsServersDataTable = objAdaptor.FetchData(SqlQuery);
        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {
        //    }
        //    return DSTaskSettingsServersDataTable;
        //}

        public DataTable DSTaskSettingsUpdategridFirstTime(string ServerKey)
        {
            DataTable DSTaskSettingsServersDataTable = new DataTable();
            try
            {
                string SqlQuery = "Select *,'' as TaskName,'' as Enabled from ServerTaskSettings where ServerID=" + ServerKey;

                DSTaskSettingsServersDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch(Exception ex)
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
            catch(Exception ex)
            {
				throw ex;
             }
            finally
            {
            }
            return RescServers;
        }

        public DataTable GetDiskSettings(string Server, string strAll)
        {
            DataTable RescServers = new DataTable();
            try
            {
                string Query = "";
                if (strAll == "All")
                {
                    //5/1/2014 NS modified for VSPLUS-602,VSPLUS-616
                    //Query = "SELECT dom.ServerName,dom.DiskName,ds.Threshold,0 as isSelected ,ds.id FROM DominoDiskSpace dom left outer join DominoDiskSettings ds on dom.ServerName=ds.ServerName and dom.DiskName=ds.DiskName where dom.servername='" + Server + "'";
					Query = "SELECT dom.ServerName,ds.DiskInfo,dom.DiskName,ROUND(dom.DiskFree,1) DiskFree,ROUND(dom.DiskSize,1) DiskSize," +
                        "ROUND(dom.PercentFree*100,1) PercentFree,ds.Threshold,ds.ThresholdType,0 as isSelected ,ds.id " +
                        "FROM DominoDiskSpace dom LEFT OUTER JOIN DominoDiskSettings ds ON dom.ServerName=ds.ServerName " +
                        "WHERE dom.servername='" + Server + "'";

                }
                else
                {
                    //5/1/2014 NS modified for VSPLUS-602,VSPLUS-616
                    //Query = "SELECT dom.ServerName,dom.DiskName,ds.Threshold,case when isnull(ds.ServerName,'0')='0' then 0 else 1 end as isSelected ,ds.id FROM DominoDiskSpace dom left outer join DominoDiskSettings ds on dom.ServerName=ds.ServerName and dom.DiskName=ds.DiskName where dom.servername='" + Server + "'";
					Query = "SELECT dom.ServerName,ds.DiskInfo,dom.DiskName,ROUND(dom.DiskFree,1) DiskFree,ROUND(dom.DiskSize,1) DiskSize," +
                        "ROUND(dom.PercentFree*100,1) PercentFree,ds.Threshold,ds.ThresholdType," +
                        "case when isnull(ds.ServerName,'0')='0' then 0 else 1 end as isSelected , ds.id " +
                        "FROM DominoDiskSpace dom LEFT OUTER JOIN DominoDiskSettings ds ON dom.ServerName=ds.ServerName AND " +
                        "dom.DiskName=ds.DiskName WHERE dom.servername='" + Server + "'";

                }
                RescServers = objAdaptor.FetchData(Query);

            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return RescServers;
        }
        public DataTable GetRowsDiskSettings(string Server)
        {
            DataTable RescServers = new DataTable();
            try
            {
                //5/13/2014 NS modified for VSPLUS-616
                //string Query = "SELECT * from DominoDiskSettings where ServerName='" + Server + "'"; ;
                string Query = "SELECT dds.ID ID,dds.ServerName ServerName,dds.DiskName DiskName,dds.Threshold Threshold,dds.ThresholdType ThresholdType," +
                    "dds1.DiskSize DiskSize,dds1.DiskFree DiskFree,ROUND(PercentFree*100,1) PercentFree from DominoDiskSettings dds " +
                    "left outer join dominodiskspace dds1 on dds.ServerName=dds1.ServerName and dds.DiskName=dds1.DiskName " +
                    "where dds.ServerName='" + Server + "'";
                RescServers = objAdaptor.FetchData(Query);

            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return RescServers;
        }

        //Mukund 27Jun14; VSPLUS-724
        public bool InsertDiskSettingsData(DataTable dtDisk, int enabled)
        {
            bool Insert = false;
			DataColumnCollection columns = dtDisk.Columns;
			string SqlQuery2;
            try
            {
                //VE-23 24-Jun-14, Mukund added two parts, one for non Domino servers ie DiskSettings and other for  Domino servers ie DominoDiskSettings  
                string SqlQuery1 = "select sr.id, st.servertype from servers sr, servertypes st where sr.ServerTypeID=st.id and sr.servername='" + dtDisk.Rows[0][0].ToString() + "'";
                DataTable dtservertype = objAdaptor.FetchData(SqlQuery1);
                if (dtservertype.Rows[0]["servertype"].ToString() != "Domino")// Non Domino servers
                {
                    string SqlQuery0 = "delete DiskSettings where ServerID=" + dtservertype.Rows[0]["id"].ToString();
                    Insert = objAdaptor.ExecuteNonQuery(SqlQuery0);
                    for (int i = 0; i < dtDisk.Rows.Count; i++)
                    {

                         SqlQuery2 = "insert into DiskSettings(ServerID,DiskName,Threshold,ThresholdType) " +
                                        "values(" + dtservertype.Rows[0]["id"].ToString() + ",'" + dtDisk.Rows[i][1].ToString() + "'," +
                                        dtDisk.Rows[i][2].ToString() + ",'" + dtDisk.Rows[i][3].ToString() + "')";
                        Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);
                    }
                }
                else // Domino servers
                {
                    string SqlQuery0 = "delete DominoDiskSettings where ServerName='" + dtDisk.Rows[0][0].ToString() + "'";
                    Insert = objAdaptor.ExecuteNonQuery(SqlQuery0);

					//2/11/2016 Durga Added for VSPLUS 2432

                    for (int i = 0; i < dtDisk.Rows.Count; i++)
                    {
						if (columns.Contains("ThresholdType"))
						{
							 SqlQuery2 = "insert into DominoDiskSettings(ServerName,DiskName,Threshold,ThresholdType,DiskInfo) " +
											 "values('" + dtDisk.Rows[i][0].ToString() + "','" + dtDisk.Rows[i][1].ToString() + "'," +
											 dtDisk.Rows[i][2].ToString() + ",'" + dtDisk.Rows[i][3].ToString() + "','" + dtDisk.Rows[i][4].ToString() + "')";
							Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);
						}
						else
						{
							 SqlQuery2 = "insert into DominoDiskSettings(ServerName,DiskName,Threshold) " +
											 "values('" + dtDisk.Rows[i][0].ToString() + "','" + dtDisk.Rows[i][1].ToString() + "'," +
											 dtDisk.Rows[i][2].ToString() + ")";
							Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);
						}
					
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

        //Mukund 27Jun14; VSPLUS-745
        public bool InsertDiskSettingsDataSSE(DataTable dtDisk, int enabled)
        {
            bool Insert = false;
            try
            {
                //VE-23 24-Jun-14, Mukund added two parts, one for non Domino servers ie DiskSettings and other for  Domino servers ie DominoDiskSettings  
                string SqlQuery1 = "select sr.id, st.servertype from servers sr, servertypes st where sr.ServerTypeID=st.id and sr.servername='" + dtDisk.Rows[0][0].ToString() + "'";
                DataTable dtservertype = objAdaptor.FetchData(SqlQuery1);
                if (dtservertype.Rows[0]["servertype"].ToString() != "Domino")// Non Domino servers
                {
                    //VSPLUS-745, 20Jun14 Mukund: Server Settings Editor - Udate Selected Disks should leave other disk settings alone
                    if (dtDisk.Rows[0][1].ToString() == "AllDisks" || dtDisk.Rows[0][1].ToString() == "NoAlerts")
                    {
                        string SqlQuery0 = "delete DiskSettings where ServerID=" + dtservertype.Rows[0]["id"].ToString();
                        Insert = objAdaptor.ExecuteNonQuery(SqlQuery0);
                    }
                    for (int i = 0; i < dtDisk.Rows.Count; i++)
                    {

                        string sqlselect = "select * from DiskSettings where ServerID=" + dtservertype.Rows[0]["id"].ToString() + " and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";
                        DataTable dtdiskcheck = objAdaptor.FetchData(sqlselect);
                        if (dtdiskcheck.Rows.Count == 0)
                        {
                            string SqlQuery2 = "insert into DiskSettings(ServerID,DiskName,Threshold,ThresholdType) " +
                                             "values(" + dtservertype.Rows[0]["id"].ToString() + ",'" + dtDisk.Rows[i][1].ToString() + "'," +
                                             dtDisk.Rows[i][2].ToString() + ",'" + dtDisk.Rows[i][3].ToString() + "')";
                            Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);
                        }
                        else
                        {
                            string SqlUpdatediskdata = "Update DiskSettings set Threshold ='" + dtDisk.Rows[i][2].ToString() + "'," +
                                     "ThresholdType ='" + dtDisk.Rows[i][3].ToString() + "' where ServerID='" + dtservertype.Rows[0]["id"].ToString() +
                                     "' and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";
                            Insert = objAdaptor.ExecuteNonQuery(SqlUpdatediskdata);
                           

                        }
                    }
                }
                else // Domino servers
                {
                    //VSPLUS-745, 20Jun14 Mukund: Server Settings Editor - Udate Selected Disks should leave other disk settings alone
                    if (dtDisk.Rows[0][1].ToString() == "AllDisks" || dtDisk.Rows[0][1].ToString() == "NoAlerts")
                    {
                        string SqlQuery0 = "delete DominoDiskSettings where ServerName='" + dtDisk.Rows[0][0].ToString() + "'";
                        Insert = objAdaptor.ExecuteNonQuery(SqlQuery0);
                    }
                    for (int i = 0; i < dtDisk.Rows.Count; i++)
                    {

                        string sqlselect = "select * from DominoDiskSettings where ServerName='" + dtDisk.Rows[0][0].ToString() + "' and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";
                        DataTable dtdiskcheck = objAdaptor.FetchData(sqlselect);
                        if (dtdiskcheck.Rows.Count == 0)
                        {
                            string SqlQuery2 = "insert into DominoDiskSettings(ServerName,DiskName,Threshold,ThresholdType) " +
                                             "values('" + dtDisk.Rows[i][0].ToString() + "','" + dtDisk.Rows[i][1].ToString() + "','" +
                                             dtDisk.Rows[i][2].ToString() + "','" + dtDisk.Rows[i][3].ToString() + "')";
                            Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);
                        }
                        else
                        {
                            string SqlUpdatediskdata = "Update DominoDiskSettings set Threshold ='" + dtDisk.Rows[i][2].ToString() + "'," +
                                     "ThresholdType ='" + dtDisk.Rows[i][3].ToString() + "' where ServerName='" + dtDisk.Rows[0][0].ToString() + "' and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";
                            Insert = objAdaptor.ExecuteNonQuery(SqlUpdatediskdata);
                            if (enabled == 0)
                            {
                                string sqlquery = "Delete from  DominoDiskSettings where ServerName='" + dtDisk.Rows[0][0].ToString() +
                                     "' and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";
                                Insert = objAdaptor.ExecuteNonQuery(sqlquery);

                            }

                        }
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

        //  Mukund 12Jun14:  VE-4	: Implement Disk Checking - Front End  

		public DataTable GetSrvRowsDiskSettings(string ServerID)
		{
			DataTable RescServers = new DataTable();
			try
			{
				string Query = "SELECT dds.ID ID,dds.DiskName DiskName,dds.Threshold Threshold,dds.ThresholdType ThresholdType," +
					"dds1.DiskSize DiskSize,dds1.DiskFree DiskFree,ROUND(PercentFree*100,1) PercentFree from DiskSettings dds " +
					"left outer join diskspace dds1 on dds1.ServerName=(select ServerName from Servers where ID=" + ServerID + ") and dds.DiskName=dds1.DiskName " +
					"where dds.ServerID=" + ServerID + "";
				RescServers = objAdaptor.FetchData(Query);

			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return RescServers;
		}
        public DataTable GetSrvDiskSettings(string Server, string strAll)
        {
            DataTable RescServers = new DataTable();
            try
            {
                string Query = "";
                if (strAll == "All")
                {
                    //5/1/2014 NS modified for VSPLUS-602,VSPLUS-616

                    Query = "SELECT dom.ServerName,dom.DiskName,ROUND(dom.DiskFree,1) DiskFree,ROUND(dom.DiskSize,1) DiskSize," +
                        "ROUND(dom.PercentFree*100,1) PercentFree,ds.Threshold,ds.ThresholdType,0 as isSelected ,ds.id " +
                        "FROM  dbo.DiskSettings AS ds INNER JOIN " +
                         " dbo.Servers AS sr ON ds.ServerID = sr.ID RIGHT OUTER JOIN" +
                         " dbo.DiskSpace AS dom ON sr.ServerName = dom.ServerName  and ds.DiskName=dom.DiskName " +
                        "WHERE dom.servername='" + Server + "'";

                }
                else
                {
                    //5/1/2014 NS modified for VSPLUS-602,VSPLUS-616
                    Query = "SELECT dom.ServerName,dom.DiskName,ROUND(dom.DiskFree,1) DiskFree,ROUND(dom.DiskSize,1) DiskSize," +
                        "ROUND(dom.PercentFree*100,1) PercentFree,ds.Threshold,ds.ThresholdType," +
                        "case when isnull(ds.ServerID,0)=0 then 0 else 1 end as isSelected , ds.id " +
                       "FROM  dbo.DiskSettings AS ds INNER JOIN " +
                             " dbo.Servers AS sr ON ds.ServerID = sr.ID RIGHT OUTER JOIN" +
                             " dbo.DiskSpace AS dom ON sr.ServerName = dom.ServerName  and ds.DiskName=dom.DiskName " +
                            "WHERE dom.servername='" + Server + "'";

                }
                RescServers = objAdaptor.FetchData(Query);

            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return RescServers;
        }

        public bool InsertSrvDiskSettingsData(DataTable dtDisk, int enabled)
        {
            bool Insert = false;
            bool diskSettings = true;
            try
            {
                string SqlQuery = "delete DiskSettings where ServerID=" + dtDisk.Rows[0][4].ToString() + "";
                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);


                for (int i = 0; i < dtDisk.Rows.Count; i++)
                {

                    string SqlQuery2 = "insert into DiskSettings(ServerID,DiskName,Threshold,ThresholdType) " +
            "values(" + dtDisk.Rows[i][4].ToString() + ",'" + dtDisk.Rows[i][1].ToString() + "'," +
            dtDisk.Rows[i][2].ToString() + ",'" + dtDisk.Rows[i][3].ToString() + "')";
                    Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);

                }



                //string SqlQuery = "delete DiskSettings where ServerID=" + dtDisk.Rows[0][4].ToString() + "";
                //string queryServerDisks = "Select DiskName from DiskSpace where ServerName='" + dtDisk.Rows[0][0].ToString() + "'";
                //if (dtDisk.Rows[0][1].ToString() == "AllDisks" || dtDisk.Rows[0][1].ToString() == "NoAlerts")
                //{
                //    Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
                //    diskSettings = false;
                //}
                //for (int i = 0; i < dtDisk.Rows.Count; i++)
                //{
                //    if (enabled == 0)
                //    {
                //        string sqlDelete = "Delete from DiskSettings where ServerID=" + dtDisk.Rows[0][4].ToString() + " and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";

                //        if (objAdaptor.ExecuteNonQueryRetRows(sqlDelete) >= 0)
                //        {
                //            Insert = true;
                //        }
                //        else
                //        {
                //            Insert = false;
                //        }
                //    }
                //    else
                //    {
                //        // 5/14/2014 - CY modified for VS-545
                //        if (diskSettings)
                //        {
                //            string queryAllNoDisks = "Select DiskName,Threshold,ThresholdType from DiskSettings where ServerID=" + dtDisk.Rows[0][4].ToString() + "";
                //            DataTable dtAllNoDisks = objAdaptor.FetchData(queryAllNoDisks);
                //            if (dtAllNoDisks.Rows.Count == 1 && (dtAllNoDisks.Rows[0]["DiskName"].ToString() == "AllDisks" || dtAllNoDisks.Rows[0]["DiskName"].ToString() == "NoAlerts"))
                //            {
                //                //Delete Alldisks/diskname entry and re-insert data for each disk
                //                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);

                //                DataTable dtServerDisks = objAdaptor.FetchData(queryServerDisks);
                //                foreach (DataRow dr in dtServerDisks.Rows)
                //                {
                //                    string SqlUpdatediskSettings = "insert into DiskSettings(ServerID,DiskName,Threshold,ThresholdType) " +
                //                        "values(" + dtDisk.Rows[0][4].ToString() + ",'" + dr["DiskName"].ToString() + "'," +
                //                            dtAllNoDisks.Rows[0]["Threshold"].ToString() + ",'" + dtAllNoDisks.Rows[0]["ThresholdType"].ToString() + "')";
                //                    Insert = objAdaptor.ExecuteNonQuery(SqlUpdatediskSettings);
                //                }
                //                //Now Update the disk data
                //                string SqlUpdatediskdata = "Update DiskSettings set Threshold ='" + dtDisk.Rows[i][2].ToString() + "'," +
                //                    "ThresholdType ='" + dtDisk.Rows[i][3].ToString() + "' where ServerID=" + dtDisk.Rows[0][4].ToString() + " and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";
                //                Insert = objAdaptor.ExecuteNonQuery(SqlUpdatediskdata);
                //            }
                //            else
                //            {
                //                string SqlQuery1 = "";
                //                DataTable dt;
                //                //SqlQuery1 = "Select DiskName from DiskSpace where ServerName='" + dtDisk.Rows[0][0].ToString() + "'";// and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";
                //                dt = objAdaptor.FetchData(queryServerDisks);
                //                foreach (DataRow dr in dt.Rows)
                //                {
                //                    //Check for the entry of each disk
                //                    if (dr["DiskName"].ToString() == dtDisk.Rows[i][1].ToString())
                //                    {
                //                        //Update when record exists else insert
                //                        string querysingleDisk = "Select DiskName,Threshold,ThresholdType from DiskSettings where ServerID=" + dtDisk.Rows[0][4].ToString() + " and DiskName='" + dtDisk.Rows[i][1].ToString() + "'";
                //                        DataTable dtsingleDisk = objAdaptor.FetchData(querysingleDisk);
                //                        if (dtsingleDisk.Rows.Count == 1)
                //                        {
                //                            string SqlQuery2 = "Update DiskSettings set Threshold ='" + dtDisk.Rows[i][2].ToString() + "'," +
                //                    "ThresholdType ='" + dtDisk.Rows[i][3].ToString() + "' where ServerID=" + dtDisk.Rows[0][4].ToString() + " and DiskName='" + dtDisk.Rows[i][1].ToString() + "'"; ;
                //                            Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);
                //                        }
                //                        else
                //                        {
                //                            string SqlQuery2 = "insert into DiskSettings(ServerID,DiskName,Threshold,ThresholdType) " +
                //                    "values(" + dtDisk.Rows[i][4].ToString() + ",'" + dtDisk.Rows[i][1].ToString() + "'," +
                //                    dtDisk.Rows[i][2].ToString() + ",'" + dtDisk.Rows[i][3].ToString() + "')";
                //                            Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);
                //                        }

                //                    }
                //                }
                //            }

                //        }
                //        else
                //        {
                //            //5/1/2014 NS modified for VSPLUS-602
                //            //SqlQuery = "insert into DiskSettings(ServerID,DiskName,Threshold) values('" + dtDisk.Rows[i][0].ToString() + "','" + dtDisk.Rows[i][1].ToString() + "'," + dtDisk.Rows[i][2].ToString() + ")";
                //            string SqlQuery1 = "insert into DiskSettings(ServerID,DiskName,Threshold,ThresholdType) " +
                //                "values(" + dtDisk.Rows[i][4].ToString() + ",'" + dtDisk.Rows[i][1].ToString() + "'," +
                //                dtDisk.Rows[i][2].ToString() + ",'" + dtDisk.Rows[i][3].ToString() + "')";
                //            Insert = objAdaptor.ExecuteNonQuery(SqlQuery1);
                //        }
                //    }
                //}

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


        //7/29/2014 NS added for VSPLUS-634
        public void WriteHistoryEntry(string strMsg)
        {
            bool appendMode = true;

            string ServiceLogDestination = VSWebDAL.SettingDAL.SettingsDAL.Ins.Getvalue("Log Files Path"); //VSWebBL.SettingBL.SettingsBL.Ins.Getvalue("Log Files Path");
            ServiceLogDestination += "VSWebLogs.txt";
            try
            {
                StreamWriter sw = new StreamWriter(ServiceLogDestination, appendMode, System.Text.Encoding.Unicode);
                sw.WriteLine(strMsg);
                sw.Close();
                sw = null;
            }
            catch
            {
                //7/8/2014 NS added
                ServiceLogDestination = System.Web.HttpContext.Current.Server.MapPath("~") + "\\VSWebLogs.txt";
                try
                {
                    StreamWriter sw = new StreamWriter(ServiceLogDestination, appendMode, System.Text.Encoding.Unicode);
                    sw.WriteLine(strMsg);
                    sw.Close();
                    sw = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    GC.Collect();
                }
            }
            finally
            {
                GC.Collect();
            }
        }



        public bool DeleteAllRecordsfromDiskSettingsDAL(string Servername)
        {
            try
            {

                string SqlQuery = "delete from DominoDiskSettings where Servername='" + Servername + "'";
                return objAdaptor.ExecuteNonQuery(SqlQuery);

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

		public bool ForceDominoTableRefresh()
        {
            try
            {

				string SqlQuery = "insert into DominoServers (Enabled) select 1 where 0=1";
                return objAdaptor.ExecuteNonQuery(SqlQuery);

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

			
    }


}
