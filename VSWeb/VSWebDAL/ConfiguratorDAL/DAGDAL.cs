using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using System.Data.SqlClient;

namespace VSWebDAL.ConfiguratorDAL
{
   public class DAGDAL
    {
        private Adaptor objAdaptor = new Adaptor();
        private static DAGDAL _self = new DAGDAL();
        public static DAGDAL Ins
        {
            get { return _self; }
        }
        public Object dagdetailsdata2(DagSettings ServerObject)
        {
            
            Object Update;
            try
            {
                DataTable dt = GetAttributes(ServerObject.ServerID);
                if (dt.Rows.Count > 0)
                {
					string SqlQuery = "UPDATE DagSettings SET ServerID = @ServerID,PrimaryConnection = @PrimaryConnection,BackupConnection = @BackupConnection where ServerID = @ServerID";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@ServerID", (object)ServerObject.ServerID ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@PrimaryConnection", (object)ServerObject.PrimaryConnection ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@BackupConnection", (object)ServerObject.BackupConnection ?? DBNull.Value);
					Update = objAdaptor.ExecuteNonQuerywithcmd(cmd);
                    //Update = objAdaptor.ExecuteNonQuery(SqlQuery);
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
        public DataTable GetDagSettings(DagSettings objdag)
        {
            DataTable MSServersDataTable = new DataTable();

            try
            {
                string SqlQuery = "select dg.ServerID,dg.ReplyQThreshold,dg.CopyQThreshold,(select sr.ServerName +'-'+ l.Location  from servers sr  inner join Locations l on l.ID=sr.LocationID where sr.ID=dg.PrimaryConnection) as PrimaryConnection,(select sr.ServerName +'-'+ l.Location  from servers sr  inner join Locations l on l.ID=sr.LocationID where sr.ID=dg.BackupConnection) as BackupConnection from DagSettings dg where ServerID=" + objdag.ServerID + "";
               // select (ServerName +'-'+ Location) as Primaryname,dg.PrimaryConnection from servers sr inner join DagSettings dg on dg.PrimaryConnection=sr.ID inner join Locations l on l.ID=sr.LocationID where ServerID=34
//select (ServerName +'-'+ Location) as Backupname,dg.BackupConnection from servers sr inner join DagSettings dg on dg.BackupConnection=sr.ID inner join Locations l on l.ID=sr.LocationID where ServerID=34
                MSServersDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return MSServersDataTable;
        }
        public bool InsertAttributesData(DagSettings ServerObject)
        {

            //WS modified to fetch realistic data for the exchange servers
            bool Insert = false;
            try
            {
                string sqlStr = "select st.servertype from servertypes st where st.id=(select srv.servertypeid from servers srv where srv.id=" + ServerObject.ServerID + ")";
                DataTable dt = objAdaptor.FetchData(sqlStr);
                string ServerType = dt.Rows.Count == 1 ? dt.Rows[0][0].ToString() : "";

                if (ServerType == "Exchange")
                {
                    string[] exchgdefaults = new string[17];
                    string attrname = "";
                    string attrval = "";
                    DataTable dtattr = VSWebDAL.SecurityDAL.ServersDAL .Ins. GetServerDetailsBytype2("Exchange");
                    if (dtattr.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtattr.Rows.Count; i++)
                        {
                            attrname = dtattr.Rows[i]["PrimaryConnection"].ToString();
                            attrval = dtattr.Rows[i]["DefaultValue"].ToString();
                            switch (attrname)
                            {
                                case "Scan Interval":
                                    exchgdefaults[0] = attrval;
                                    break;
                                case "Off Hours Scan Interval":
                                    exchgdefaults[1] = attrval;
                                    break;
                                case "PrimaryConnection":
                                    exchgdefaults[2] = attrval;
                                    break;
                                case "BackupConnection":
                                    exchgdefaults[3] = attrval;
                                    break;
                 
                               
                                default:
                                    string s = attrname;
                                    break;
                            }
                        }

                        string SqlQuery = "INSERT INTO [DagSettings] (ServerID,PrimaryConnection,BackupConnection) " +
						   " VALUES(@ServerID,@PrimaryConnection,@BackupConnection)";

						SqlCommand cmd = new SqlCommand(SqlQuery);
						cmd.Parameters.AddWithValue("@ServerID", (object)ServerObject.ServerID ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@PrimaryConnection", (object)ServerObject.PrimaryConnection ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@BackupConnection", (object)ServerObject.BackupConnection ?? DBNull.Value);
						Insert = objAdaptor.ExecuteNonQuerywithcmd(cmd);
                        //Insert = objAdaptor.ExecuteNonQuery(SqlQuery);

                    }
                }
                else
                {
                    string SqlQuery = "INSERT INTO [DagSettings] (ServerID,PrimaryConnection,BackupConnection)" +
					   "VALUES(@ServerID,@PrimaryConnection,@BackupConnection)";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@ServerID", (object)ServerObject.ServerID ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@PrimaryConnection", (object)ServerObject.PrimaryConnection ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@BackupConnection", (object)ServerObject.BackupConnection ?? DBNull.Value);
					Insert = objAdaptor.ExecuteNonQuerywithcmd(cmd);
					  
                   // Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
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

        public DataTable GetAllDataByName(Servers ServerObject)
        {

            DataTable ServersDataTable = new DataTable();
            Servers ReturnSerevrbject = new Servers();
            try
            {
                if (ServerObject.ID == 0)
                {

                    string SqlQuery = "select Sr.ID,Sr.ServerName,sr.Description,S.ServerType,sa.ScanDAGHealth,sa.CredentialsId,L.Location,sr.ipaddress,sa.Enabled,sa.ScanInterval,sa.RetryInterval,sa.OffHourInterval, " +
                    "sa.category,sa.CPU_Threshold,sa.MemThreshold,sa.ResponseTime,sa.ConsFailuresBefAlert,sa.ConsOvrThresholdBefAlert from Servers Sr inner join ServerTypes S " +
					"on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID  left outer join ServerAttributes sa on sr.ID=sa.serverid where sr.ServerName = @ServerName";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@ServerName", (object)ServerObject.ServerName ?? DBNull.Value);
					ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
                   // ServersDataTable = objAdaptor.FetchData(SqlQuery);
                }
                else
                {

                    string SqlQuery = " select Sr.ID,Sr.ServerName,sr.Description,S.ServerType,S.ScanDAGHealth,L.Location,sr.ipaddress,sa.Enabled,sa.ScanInterval,sa.RetryInterval,sa.OffHourInterval, " +
                    "sa.category,sa.CPU_Threshold,sa.MemThreshold,sa.ResponseTime,sa.ConsFailuresBefAlert,sa.ConsOvrThresholdBefAlert from Servers Sr inner join ServerTypes S " +
					"on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID  left outer join ServerAttributes sa on sr.ID=sa.serverid  and ID<> @ID";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@ID", (object)ServerObject.ID ?? DBNull.Value);
					ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
                    //ServersDataTable = objAdaptor.FetchData(SqlQuery);
                }

            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return ServersDataTable;
        }
        public DataTable GetAttributes(int ServerId)
        {
			DataTable ServersDataTable = new DataTable();
			try
			{
				string SqlQuery = "select * from DagSettings where ServerID = @ID";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ID", (object) ServerId ?? DBNull.Value);
				ServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
				//ServersDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{	
				throw ex;
			}
           
            return ServersDataTable;
        }
        public DataTable GetAllData()
        {
            DataTable MSServersDataTable = new DataTable();

            try
            {
                //3/21/2014 NS modified the query - need to add locationid
                //string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
                //             " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='Exchange' ";
                string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category,ISNULL(sa.ScanDagHealth,'false') as ScanDagHealth from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
								 " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='Database Availability Group' ";
                MSServersDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch(Exception ex)
            {
				throw ex;
            }
            finally
            {
            }
            return MSServersDataTable;
        }

		public DataTable getMailDatabaseSettings(string ServerName)
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = "SELECT ServerName, DatabaseName, ReplayQueueThreshold, CopyQueueThreshold FROM DAGQueueThresholds where DAGName = @ServerName";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ServerName", (object)ServerName ?? DBNull.Value);
				dt = objAdaptor.FetchDatafromcommand(cmd);
				//dt = objAdaptor.FetchData(SqlQuery);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
			}

			return dt;
		}

		public DataTable getMailDatabaseQueueSettings(string ServerName)
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = "select dm.ServerName, dd.DatabaseName, dqt.ReplayQueueThreshold, dqt.CopyQueueThreshold, dd.CopyQueue, dd.ReplayQueue, " +
									"case when (isnull(dqt.ReplayQueueThreshold,0)=0 or isnull(dqt.CopyQueueThreshold,0)=0) then 0 else 1 end as isSelected  " +
									"from DAGDatabase dd  " +
									"left outer join DAGMembers dm on dm.ID=dd.DAGMemberId " +
									"left outer join DAGQueueThresholds dqt on dqt.ServerName = dm.ServerName AND dqt.DatabaseName=dd.DatabaseName " +
									"left outer join DAGStatus ds on dm.DAGID=ds.ID WHERE ds.DAGName = @ServerName ";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ServerName", (object)ServerName ?? DBNull.Value);
				dt = objAdaptor.FetchDatafromcommand(cmd);
				//dt = objAdaptor.FetchData(SqlQuery);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
			}

			return dt;
		}

		public bool InsertSrvDatabaseSettingsData(DataTable dtDatabase, String ServerName)
		{
			bool Insert = false;

			try
			{
				string SqlQuery = "delete DAGQueueThresholds where DAGName='" + ServerName + "'";

				Insert = objAdaptor.ExecuteNonQuery(SqlQuery);


				for (int i = 0; i < dtDatabase.Rows.Count; i++)
				{

					string SqlQuery2 = "insert into DAGQueueThresholds(DAGName,ServerName,ServerTypeId,DatabaseName, ReplayQueueThreshold, CopyQueueThreshold) " +
						"values(@DAGName,@ServerName,(select ID from ServerTypes where ServerType='Database Availability Group'),@DatabaseName,@ReplayQueueThreshold,@CopyQueueThreshold)";
					SqlCommand cmd = new SqlCommand(SqlQuery2);
					cmd.Parameters.AddWithValue("@DAGName", (object)ServerName ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ServerName", (object)dtDatabase.Rows[i]["ServerName"].ToString() ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@DatabaseName", (object)dtDatabase.Rows[i]["DatabaseName"].ToString() ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@ReplayQueueThreshold", (object)dtDatabase.Rows[i]["ReplayQueueThreshold"].ToString() ?? DBNull.Value);
					cmd.Parameters.AddWithValue("@CopyQueueThreshold", (object)dtDatabase.Rows[i]["CopyQueueThreshold"].ToString() ?? DBNull.Value);
					Insert = objAdaptor.ExecuteNonQuerywithcmd(cmd);
					//Insert = objAdaptor.ExecuteNonQuery(SqlQuery2);

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
