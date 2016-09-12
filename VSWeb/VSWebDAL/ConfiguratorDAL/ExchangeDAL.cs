using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using System.Data.SqlClient;

namespace VSWebDAL.ConfiguratorDAL
{
    public class ExchangeDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static ExchangeDAL _self = new ExchangeDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static ExchangeDAL Ins
        {
            get { return _self; }
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
                                 " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='Exchange' ";
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
		

		public DataTable GetAllDatabyUser(int UserID)
        {
            DataTable MSServersDataTable = new DataTable();

            try
            {
				//SqlParameter[] p = new SqlParameter[1];
				// p[0] = new SqlParameter("@UserID", UserID);
                //3/21/2014 NS modified the query - need to add locationid
                //string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
                //             " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='Exchange' ";
				string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category,ISNULL(sa.ScanDagHealth,'false') as ScanDagHealth from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
								 " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='Exchange'  and Sr.ID not in(select serverID from UserServerRestrictions ur inner join Users U on ur.UserID= U.ID where U.ID = @UserID) order by ServerName ";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@UserID", UserID);
				MSServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
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
        public DataTable GetExchangeSettings(ExchangeSettings objExchange)
        {
            DataTable MSServersDataTable = new DataTable();

            try
            {
                string SqlQuery = "select * from   ExchangeSettings where ServerId= @ServerID";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@ServerID", objExchange.ServerID);
				MSServersDataTable = objAdaptor.FetchDatafromcommand(cmd);
			
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return MSServersDataTable;
        }

        public Object InsertExchangeSettings(ExchangeSettings objExchange)
        {

            //4/25/2016 Sowjanya modified for VSPLUS-2850

            Object returnval;
            try
            {

                string st = "Insert into ExchangeSettings(ServerID, "+
                    "CASSmtp , CASPop3 , CASImap , CASOARPC , CASOWA, CASActiveSync ,"+
                    "CASEWS , CASAutoDiscovery , "+
                    "SubQThreshold , PoisonQThreshold , UnReachableQThreshold , TotalQThreshold , "+
					"VersionNo,ActiveSyncCredentialsId, ShadowQThreshold, AuthenticationType) values" +
					//"ActiveSyncCredentialsId) values" +
				   "(@ServerID, @CASSmtp ,@CASPop3,@CASImap,@CASOARPC,@CASOWA,@CASActiveSync,@CASEWS,@CASAutoDiscovery,@SubQThreshold ,@PoisonQThreshold ,@UnReachableQThreshold ,@TotalQThreshold ,@VersionNo,@ACCredentialsId , @ShadowQThreshold ,@AuthenticationType )";
				   //objExchange.ACCredentialsId.ToString() + ")";
				SqlCommand cmd = new SqlCommand(st);
				cmd.Parameters.AddWithValue("@ServerID", objExchange.ServerID);
				cmd.Parameters.AddWithValue("@CASSmtp", objExchange.CASSmtp);
				cmd.Parameters.AddWithValue("@CASPop3", objExchange.CASPop3);
				cmd.Parameters.AddWithValue("@CASImap", objExchange.CASImap);
				cmd.Parameters.AddWithValue("@CASOARPC", objExchange.CASOARPC);
				cmd.Parameters.AddWithValue("@CASOWA", objExchange.CASOWA);
				cmd.Parameters.AddWithValue("@CASActiveSync", objExchange.CASActiveSync);
				cmd.Parameters.AddWithValue("@CASEWS", objExchange.CASEWS);
                //cmd.Parameters.AddWithValue("@CASECP", objExchange.CASECP);
				cmd.Parameters.AddWithValue("@CASAutoDiscovery", objExchange.CASAutoDiscovery);
                //cmd.Parameters.AddWithValue("@CASOAB", objExchange.CASOAB);
				cmd.Parameters.AddWithValue("@SubQThreshold", objExchange.SubQThreshold);
				cmd.Parameters.AddWithValue("@PoisonQThreshold", objExchange.PoisonQThreshold);
				cmd.Parameters.AddWithValue("@UnReachableQThreshold", objExchange.UnReachableQThreshold);
				cmd.Parameters.AddWithValue("@TotalQThreshold", objExchange.TotalQThreshold);
				cmd.Parameters.AddWithValue("@VersionNo", 0);
				cmd.Parameters.AddWithValue("@ACCredentialsId", objExchange.ACCredentialsId.ToString());
				cmd.Parameters.AddWithValue("@ShadowQThreshold", objExchange.ShadowQThreshold);
				cmd.Parameters.AddWithValue("@AuthenticationType", objExchange.AuthenticationType);

				returnval = objAdaptor.ExecuteNonQuerywithcmd(cmd);
            }
            catch (Exception e)
            {
                throw e;
            }
            return returnval;

        }

        public Object UpdateExchangeSettings(ExchangeSettings objExchange)
        {
            //4/25/2016 Sowjanya modified for VSPLUS-2850
            Object returnval;
            try
            {

				string st = "Update ExchangeSettings Set CASSmtp=@CASSmtp ,CASPop3=@CASPop3 ,CASImap=@CASImap,CASOARPC=@CASOARPC,CASOWA=@CASOWA,CASActiveSync=@CASActiveSync ,CASEWS=@CASEWS,CASAutoDiscovery=@CASAutoDiscovery ,SubQThreshold=@SubQThreshold,PoisonQThreshold=@PoisonQThreshold ,UnReachableQThreshold=@UnReachableQThreshold ,TotalQThreshold=@TotalQThreshold ,LatencyYellowThreshold=@LatencyYellowThreshold ,LatencyRedThreshold=@LatencyRedThreshold ,EnableLatencyTest=@EnableLatencyTest ,   ActiveSyncCredentialsId=@ACCredentialsId, ShadowQThreshold=@ShadowQThreshold , AuthenticationType=@AuthenticationType where ServerId=@ServerID"; 
                  //",VersionNo='" + objExchange.VersionNo + "',ActiveSyncCredentialsId=" +  objExchange.ACCredentialsId.ToString()  +" where ServerId=" + objExchange.ServerID;
				 
				SqlCommand cmd = new SqlCommand(st);
				cmd.Parameters.AddWithValue("@ServerID", (object)objExchange.ServerID ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@CASSmtp", (object)objExchange.CASSmtp ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@CASPop3", (object)objExchange.CASPop3 ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@CASImap", (object)objExchange.CASImap ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@CASOARPC", (object)objExchange.CASOARPC ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@CASOWA", (object)objExchange.CASOWA ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@CASActiveSync", (object)objExchange.CASActiveSync ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@CASEWS", (object)objExchange.CASEWS ?? DBNull.Value);
                //cmd.Parameters.AddWithValue("@CASECP", (object)objExchange.CASECP ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@CASAutoDiscovery", (object)objExchange.CASAutoDiscovery ?? DBNull.Value);
                //cmd.Parameters.AddWithValue("@CASOAB", (object)objExchange.CASOAB ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@SubQThreshold", (object)objExchange.SubQThreshold ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@PoisonQThreshold", (object)objExchange.PoisonQThreshold ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@UnReachableQThreshold", (object)objExchange.UnReachableQThreshold ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@TotalQThreshold", (object)objExchange.TotalQThreshold ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@LatencyYellowThreshold", (object)objExchange.LatencyYellowThreshold ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@LatencyRedThreshold", (object)objExchange.LatencyRedThreshold ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@EnableLatencyTest", (object)objExchange.EnableLatencyTest ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@ACCredentialsId", (object)objExchange.ACCredentialsId.ToString() ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@ShadowQThreshold", (object)objExchange.ShadowQThreshold ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@AuthenticationType", (object)objExchange.AuthenticationType ?? DBNull.Value);
				returnval = objAdaptor.ExecuteNonQuerywithcmd(cmd);
            }
            catch (Exception e)
            {
                throw e;
            }
            return returnval;

        }


        public Object UpdateExchangeSettingsData(ExchangeSettings Mobj)
        {
            Object returnval;
			DataTable dt = GetExchangeSettings(Mobj);
			try
			{
				if (dt.Rows.Count == 0)
				{
					returnval = InsertExchangeSettings(Mobj);
				}
				else
				{
					returnval = UpdateExchangeSettings(Mobj);

				}
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
           
            
            return returnval;
        }


        public DataTable GetExchangeServerDetails()
        {
            DataTable dt = new DataTable();

            try
            {
                //7/22/2014 NS added OperatingSystem and DominoVersion to the list
                //10/9/2014 NS modified the query by adding Status to the QueryString
                //1/20/2015 NS modified the query by addint Status sort
                     string SqlQuery = "select * from "+
                                    "(Select Type,Name,Status,CASE WHEN Status='Not Responding' THEN 0 WHEN Status='Issue' THEN 1 ELSE 2 END StatusSort,"+
                                    "ResponseTime,OperatingSystem,DominoVersion, " +
                                    "(CPU*100) as CPU, "+
                                    "'''../images/icons/exchange.jpg''' as imgsource, "+
                                    "'ExchangeServerDetailsPage3.aspx?Name=' + Name + '&Type=' + Type + '&Status=' + Status + '&LastDate='+CONVERT(VARCHAR,LastUpdate,101) + ' ' + substring(convert(varchar(20), LastUpdate, 9), 13, 5) + ' ' + substring(convert(varchar(30), LastUpdate, 9), 25, 2)  as redirectto, "+
									"isnull(Memory,0)*100 as MemoryPercent  " +
                                    "from [VitalSigns].[dbo].[Status] st) as sts   "+
                                    "inner join  "+
                                    "(select * from (select * from ( "+
                                    "select s.ID,s.ServerName,roles.rolename,sa.CPU_Threshold as CPUThreshold,sa.MemThreshold,sa.ResponseTime as ResponseThreshold FROM Servers AS s  "+
                                    "LEFT OUTER JOIN  ServerAttributes sa on sa.ServerID=s.ID   "+
                                    "INNER JOIN  "+
                                    "ServerTypes AS st ON st.ID = s.ServerTypeID  "+
                                    "LEFT OUTER JOIN  "+
                                    "(select ss.serverId, rm.id as roleId, rm.rolename from ServerRoles ss, RolesMaster rm  "+
                                    " where rm.id = ss.RoleId) as roles on s.ID = roles.serverId where st.ServerType='Exchange') "+
                                    " as exchangeservers  "+
                                    ") as pr  "+
                                    " pivot  "+
                                    "( count(rolename) for rolename IN (Mailbox,CAS,Edge,Hub) )as pv) as exg   "+
                                     "on exg.ServerName = sts.name and sts.Type = 'Exchange' " +
                                     "order by StatusSort";

                dt = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return dt;
        }

        public DataTable GetDAGStatus(string DagName)
        {
            DataTable dt = new DataTable();
            string SqlQuery = "";
            try
            {
                //need to get the other column named service. Its not clear as of what to retrieve now
                if (DagName == "")
                {
					SqlQuery = "select ID, DAGName, TotalDatabases, FileWitnessSereverName, Status, FileWitnessServerStatus, (Select COUNT(*) from DAGMembers dm where dm.DAGID =ds.ID)as Members, " +
						"(select LastUpdate from status s where s.TypeANDName = DAGName + '-Database Availability Group') as  " +
						"LastUpdate, (select Sum(mh.ConnectedMailboxes) from [vitalsigns].[dbo].[ExchangeMailboxOverview]  mh, [vitalsigns].[dbo].[DAGMembers] dm, " +
						"[vitalsigns].[dbo].[DAGStatus] ds1 where mh.ServerName=dm.ServerName and dm.DAGID=ds.ID and  " +
						"ds1.DAGName=ds.DagName) as ConnectedMailboxes, (select Sum(mh.DisconnectMailboxes) from [vitalsigns].[dbo].[ExchangeMailboxOverview]  mh, [vitalsigns].[dbo].[DAGMembers] dm, " +
						"[vitalsigns].[dbo].[DAGStatus] ds1 where mh.ServerName=dm.ServerName and dm.DAGID=ds.ID and  " +
						"ds1.DAGName=ds.DagName) as DisconnectedMailboxes, TotalMailBoxes " +
						"from DAGStatus ds order by DAGName";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					dt = objAdaptor.FetchDatafromcommand(cmd);
                }
                else
                {
					SqlQuery = "select ID, DAGName, TotalDatabases, FileWitnessSereverName, Status, FileWitnessServerStatus, (Select COUNT(*) from DAGMembers dm where dm.DAGID =ds.ID)as Members, " +
						"(select LastUpdate from status s where s.TypeANDName = DAGName + '-Database Availability Group') as  " +
						"LastUpdate, (select Sum(mh.ConnectedMailboxes) from [vitalsigns].[dbo].[ExchangeMailboxOverview]  mh, [vitalsigns].[dbo].[DAGMembers] dm, " +
						"[vitalsigns].[dbo].[DAGStatus] ds1 where mh.ServerName=dm.ServerName and dm.DAGID=ds.ID and  " +
						"ds1.DAGName=ds.DagName) as ConnectedMailboxes, (select Sum(mh.DisconnectMailboxes) from [vitalsigns].[dbo].[ExchangeMailboxOverview]  mh, [vitalsigns].[dbo].[DAGMembers] dm, " +
						"[vitalsigns].[dbo].[DAGStatus] ds1 where mh.ServerName=dm.ServerName and dm.DAGID=ds.ID and  " +
						"ds1.DAGName=ds.DagName) as DisconnectedMailboxes, TotalMailBoxes " +
						"from DAGStatus ds " +
						"WHERE DagName=@Dagname order by DAGName";
					SqlCommand cmd = new SqlCommand(SqlQuery);
					cmd.Parameters.AddWithValue("@Dagname", DagName);
					dt = objAdaptor.FetchDatafromcommand(cmd);
					
                }
				
               // dt = objAdaptor.FetchData(SqlQuery);
				//SqlCommand cmd = new SqlCommand(SqlQuery);
				//cmd.Parameters.AddWithValue("@Dagname", DagName);
				//dt = objAdaptor.FetchDatafromcommand(cmd);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return dt;
        }

        public DataTable GetHubEdgeStatus()
        {
            DataTable dt = new DataTable();

            try
            {

                //need to get the other column named service. Its not clear as of what to retrieve now
                //string SqlQuery = "select srvs.ID,srvs.ServerName,srvs.rolename, isnull(st.PendingMail,0) as SubmissionQ, isnull(st.DeadMail,0) as UnreachableQ, isnull(st.HeldMail,0) as PoisonQ "+
                //    "from Status st inner join  (select s.ID,s.ServerName,roles.rolename FROM Servers AS s INNER JOIN ServerTypes AS st ON st.ID = s.ServerTypeID Inner JOIN "+
                //                   "(select ss.serverId, rm.id as roleId, rm.rolename from ServiceVersionRole svr, ServerServices ss, RolesMaster rm "+
                //                   "where rm.id = svr.RoleId and svr.id = ss.svrid and rm.rolename in ('Edge','Hub')) as roles on s.ID = roles.serverId where st.ServerType='Exchange') as srvs on srvs.ServerName = st.Name";
                //7/21/2014 NS modified the query
                /*
                string SqlQuery = "select servs.ID,servs.ServerName,redirectto,servs.rolename,servs.PoisonQ,servs.SubmissionQ,servs.UnreachableQ," +
                                "CASE when servs.rolename <> 'edge' THEN es.HubSubQThreshold ELSE es.EdgeSubQThreshold END as SubQThreshold," +
                                "CASE when servs.rolename <> 'edge' THEN es.HubPoisonQThreshold ELSE es.EdgePosQThreshold END as PosQThreshold," +
                                "CASE when servs.rolename <> 'edge' THEN es.EdgeUnReachableQThreshold ELSE es.EdgeUnReachableQThreshold END as URQThreshold," +
                                "CASE when servs.rolename <> 'edge' THEN es.HubTotalQThreshold ELSE es.EdgeTotalQThreshold END as TotalQThreshold" +
                                 " from (select srvs.ID,srvs.ServerName,srvs.rolename, isnull(sts.PendingMail,0) as SubmissionQ, isnull(sts.DeadMail,0) as UnreachableQ," +
                                 "isnull(sts.HeldMail,0) as PoisonQ,'ExchangeServerDetailsPage3.aspx?Name=' + sts.Name + '&Type=' + sts.Type + '&LastDate='+CONVERT(VARCHAR,LastUpdate,101) + ' ' + substring(convert(varchar(20), LastUpdate, 9), 13, 5) + ' ' + substring(convert(varchar(30), LastUpdate, 9), 25, 2) as redirectto" +
                                 " from Status sts inner join  (select s.ID,s.ServerName,roles.rolename FROM Servers AS s INNER JOIN ServerTypes AS st ON st.ID = s.ServerTypeID Inner JOIN " +
                                  " (select ss.serverId, rm.id as roleId, rm.rolename from ServiceVersionRole svr, ServerServices ss, RolesMaster rm " +
                                   "where rm.id = svr.RoleId and svr.id = ss.svrid and rm.rolename in ('Edge','Hub') group by ss.serverId,rm.id,rm.rolename) as roles on s.ID = roles.serverId where st.ServerType='Exchange') as srvs on srvs.ServerName = sts.Name) as servs left outer join exchangesettings es on servs.id = es.ServerId";
                */
                /*
				 * 1/29/15 WS Modified to add queues correctly
				 * string SqlQuery = "select RANK() over (order by id,rolename) as CID, servs.ID,servs.ServerName,redirectto,servs.rolename,servs.PoisonQ,servs.SubmissionQ,servs.UnreachableQ, " +
								" SubQThreshold, PoisonQThreshold, UnReachableQThreshold as URQThreshold, TotalQThreshold,LatencyYellowThreshold,LatencyRedThreshold " +
                                " from (select srvs.ID,srvs.ServerName,srvs.rolename, isnull(sts.PendingMail,0) as SubmissionQ, isnull(sts.DeadMail,0) as UnreachableQ," +
                                "isnull(sts.HeldMail,0) as PoisonQ,'ExchangeServerDetailsPage3.aspx?Name=' + sts.Name + '&Type=' + sts.Type + '&LastDate='+CONVERT(VARCHAR,LastUpdate,101) + ' ' + substring(convert(varchar(20), LastUpdate, 9), 13, 5) + ' ' + substring(convert(varchar(30), LastUpdate, 9), 25, 2) as redirectto" +
                                " from Status sts inner join  (select s.ID,s.ServerName,roles.rolename FROM Servers AS s INNER JOIN ServerTypes AS st ON st.ID = s.ServerTypeID Inner JOIN " +
                                " (select sr.serverId, rm.id as roleId, rm.rolename from ServerRoles sr, RolesMaster rm where sr.RoleId = rm.id and rm.rolename in ('Edge','Hub') group by sr.serverId,rm.id,rm.rolename) " +
								"as roles on s.ID = roles.serverId where st.ServerType='Exchange') as srvs on srvs.ServerName = sts.Name) as servs left outer join exchangesettings es on servs.id = es.ServerId";
				*/

				string SqlQuery = "select RANK() over (order by id,rolename) as CID, servs.ID,servs.ServerName,redirectto,servs.rolename,servs.ShadowQ,servs.SubmissionQ,servs.UnreachableQ, " +
								"SubQThreshold, ShadowQThreshold, UnReachableQThreshold as URQThreshold, TotalQThreshold,LatencyYellowThreshold,LatencyRedThreshold " +
								"from  ( select srvs.ID,srvs.ServerName,srvs.rolename, isnull(srvs.Submission,0) as SubmissionQ, isnull(srvs.Unreachable,0) as UnreachableQ, " +
								"isnull(srvs.Shadow,0) as ShadowQ,'ExchangeServerDetailsPage3.aspx?Name=' + sts.Name + '&Type=' + sts.Type + '&LastDate='+CONVERT(VARCHAR,LastUpdate,101) + ' ' + substring(convert(varchar(20), LastUpdate, 9), 13, 5) + ' ' + substring(convert(varchar(30), LastUpdate, 9), 25, 2) as redirectto " +
								"from Status sts inner join  (select s.ID,s.ServerName,roles.rolename,(select top 1 StatValue from [VSS_Statistics].[dbo].[MicrosoftDailyStats] mds where mds.servertypeid=5 and mds.servername=s.ServerName and mds.statname like '%@Unreachable#Queues' order by date desc) as Unreachable, " +
								"(select top 1 StatValue from [VSS_Statistics].[dbo].[MicrosoftDailyStats] mds where mds.servertypeid=5 and mds.servername=s.ServerName and mds.statname like '%@Submission#Queues' order by date desc) as Submission, " +
								"(select top 1 StatValue from [VSS_Statistics].[dbo].[MicrosoftDailyStats] mds where mds.servertypeid=5 and mds.servername=s.ServerName and mds.statname like '%@Shadow#Queues' order by date desc) as Shadow " +
								"FROM Servers AS s INNER JOIN ServerTypes AS st ON st.ID = s.ServerTypeID Inner JOIN (select sr.serverId, rm.id as roleId, rm.rolename 	from ServerRoles sr, RolesMaster rm " +
								"where sr.RoleId = rm.id and (((select VersionNo from ExchangeSettings es where es.ServerID=sr.serverId) = '2013' and rm.RoleName in ('Edge', 'Hub', 'CAS')) or " +
								"((select VersionNo from ExchangeSettings es where es.ServerID=sr.serverId) = '2010' and rm.RoleName in ('Edge', 'Hub'))) group by sr.serverId,rm.id,rm.rolename ) 	as roles on s.ID = roles.serverId " +
								"where st.ServerType='Exchange') as srvs on srvs.ServerName = sts.Name) as servs left outer join exchangesettings es on servs.id = es.ServerId";
                dt = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return dt;
        }

        public DataTable GetCASStatus()
        {
            DataTable dt = new DataTable();

            try
            {
                dt = objAdaptor.GetDataFromProcedure("CASResults");
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return dt;
        }

        public DataTable GetMailBoxStatus()
        {

            DataTable dt = new DataTable();

            try
            {
                //need to get the other column named service. Its not clear as of what to retrieve now
                string SqlQuery = "select mail.ServerName,mail.DatabaseCount,mail.MailBoxCount,mail.services,TotalSize ,'ExchangeServerDetailsPage3.aspx?Name=' + st.Name + '&Type=' + st.Type + '&LastDate='+CONVERT(VARCHAR,st.LastUpdate,101) + ' ' + substring(convert(varchar(20), st.LastUpdate, 9), 13, 5) + ' ' + substring(convert(varchar(30), st.LastUpdate, 9), 25, 2) as redirectto" +
                        " from (select mhd.ServerName,count(mhd.DatabaseName) as [DatabaseCount],SUM(mhd.MailBoxes) as [MailBoxCount],SUM(mhd.Size) as TotalSize ,null as services from ExgMailHealth mh inner join ExgMailHealthDetails mhd" +
                        " on mh.ServerName = mhd.ServerName group by mhd.ServerName)as mail inner join status st on st.Name= mail.ServerName and st.type='Exchange'";
                dt = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return dt;

        }

        public string getServerVersion(string serverID)
        {
            DataTable dt = new DataTable();

            try
            {
				string SqlQuery = "select VersionNo from ExchangeSettings where ServerId = @serverID ";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@serverID", serverID);
				dt = objAdaptor.FetchDatafromcommand(cmd);
                
            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            if (dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();
            return "";
        }


        public object UpdateServerRolesData(string ServerId,bool RoleHub, bool RoleMailBox, bool RoleCAS, bool RoleEdge, bool RoleUnified)
        {
            Object returnval;
			try
			{
				string st = "delete from ServerRoles where ServerId = @ServerId";
				SqlCommand cmd = new SqlCommand(st);
				cmd.Parameters.AddWithValue("@ServerId", ServerId);
				returnval = objAdaptor.ExecuteNonQuerywithcmd(cmd);
				//returnval = objAdaptor.ExecuteNonQuery(st);

				if (RoleHub)
				{
					st = "Insert into ServerRoles(ServerID,RoleId)  values ( @ServerId, (select id from RolesMaster where RoleName='Hub'))";
					SqlCommand cmd1 = new SqlCommand(st);
					cmd1.Parameters.AddWithValue("@ServerId", ServerId);
					returnval = objAdaptor.ExecuteNonQuerywithcmd(cmd);
					//returnval = objAdaptor.ExecuteNonQuery(st);
				}
				if (RoleMailBox)
				{
					st = "Insert into ServerRoles(ServerID,RoleId)  values ( @ServerId,(select id from RolesMaster where RoleName='MailBox'))";
					SqlCommand cmd1 = new SqlCommand(st);
					cmd1.Parameters.AddWithValue("@ServerId", ServerId);
					returnval = objAdaptor.ExecuteNonQuerywithcmd(cmd);
					//returnval = objAdaptor.ExecuteNonQuery(st);
				}
				if (RoleCAS)
				{
					st = "Insert into ServerRoles(ServerID,RoleId)  values ( @ServerId,(select id from RolesMaster where RoleName='CAS'))";
					SqlCommand cmd1 = new SqlCommand(st);
					cmd1.Parameters.AddWithValue("@ServerId", ServerId);
					returnval = objAdaptor.ExecuteNonQuerywithcmd(cmd);
					//returnval = objAdaptor.ExecuteNonQuery(st);
				}
				if (RoleEdge)
				{
					st = "Insert into ServerRoles(ServerID,RoleId)  values (@ServerId,(select id from RolesMaster where RoleName='Edge'))";
					SqlCommand cmd1 = new SqlCommand(st);
					cmd1.Parameters.AddWithValue("@ServerId", ServerId);
					returnval = objAdaptor.ExecuteNonQuerywithcmd(cmd);
					//returnval = objAdaptor.ExecuteNonQuery(st);
				}
				if (RoleUnified)
				{
					st = "Insert into ServerRoles(ServerID,RoleId) values (@ServerId,(select id from RolesMaster where RoleName='Unified Messaging'))";
					SqlCommand cmd1 = new SqlCommand(st);
					cmd1.Parameters.AddWithValue("@ServerId", ServerId);
					returnval = objAdaptor.ExecuteNonQuerywithcmd(cmd);
					//returnval = objAdaptor.ExecuteNonQuery(st);
				}
				return returnval;
			}
			catch (Exception ex)
			{
				throw ex;
			}
        }

		public int GetMailboxRoleCount()
		{

			DataTable dt = new DataTable();

			try
			{
				//need to get the other column named service. Its not clear as of what to retrieve now
				string SqlQuery = "select count(*) from serverroles sr inner join rolesmaster rm on sr.roleid=rm.id where rm.rolename='Mailbox'";
				dt = objAdaptor.FetchData(SqlQuery);
				if (dt.Rows[0][0] != null)
					return Convert.ToInt32(dt.Rows[0][0].ToString());
				
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return 0;
		}
        //14/07/2016 sowmya added for VSPLUS-3097
        public bool InsertData(ExchangeSettings LOCbject)
        {

            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO CASServerTests (ServerId,TestId,URLs,CredentialsId) VALUES(@ServerId,@TestId,@URLs,@CredentialsId)";
                SqlCommand cmd = new SqlCommand(SqlQuery);
                cmd.Parameters.AddWithValue("@ServerId", (object)LOCbject.ServerId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TestId", (object)LOCbject.TestId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@URLs", (object)LOCbject.URLs ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CredentialsId", (object)LOCbject.CredentialsId ?? DBNull.Value);
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

         public ExchangeSettings GetDataForServerType(ExchangeSettings STypebject)
        {
            DataTable ServerTypesDataTable = new DataTable();
            ExchangeSettings ReturnSTypeobject = new ExchangeSettings();
            try
            {
                string SqlQuery = "Select TestId from ExchangeTestNames where TestName='" + STypebject.TestName + "'";
                ServerTypesDataTable = objAdaptor.FetchData(SqlQuery);
                //populate & return data object
                if (ServerTypesDataTable.Rows.Count > 0)
                {
                    if (ServerTypesDataTable.Rows[0]["TestId"].ToString() != "")
                        ReturnSTypeobject.id = int.Parse(ServerTypesDataTable.Rows[0]["TestId"].ToString());
                }
                else
                {
                    ReturnSTypeobject.TestId = 0;
                }
            }
            catch
            {
            }
            finally
            {
            }
            return ReturnSTypeobject;
        }

        public bool UpdateData(ExchangeSettings LOCbject)
        {
            string SqlQuery = "";
            bool Update;

            try
            {
                //7/10/2015 NS modified for VSPLUS-1985
                //if (LOCbject.Password == "      ")


                SqlQuery = "UPDATE CASServerTests SET ServerId= @ServerId,TestId= @TestId, URLs= @URLs, CredentialsId= @CredentialsId WHERE id = @id";
                    SqlCommand cmd = new SqlCommand(SqlQuery);
                    cmd.Parameters.AddWithValue("@ServerId", (object)LOCbject.ServerId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TestId", (object)LOCbject.TestId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@URLs", (object)LOCbject.URLs ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CredentialsId", (object)LOCbject.CredentialsId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", (object)LOCbject.id ?? DBNull.Value);
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

        public DataTable GetDataForCredentialsById(ExchangeSettings LOCbject)
        {
            DataTable LocationsDataTable = new DataTable();
            try
            {
                string SqlQuery = "Select * from CASServerTests where id= @id";
                SqlCommand cmd = new SqlCommand(SqlQuery);
                cmd.Parameters.AddWithValue("@id", (object)LOCbject.id ?? DBNull.Value);
                LocationsDataTable = objAdaptor.FetchDatafromcommand(cmd);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return LocationsDataTable;
        }


        public DataTable GetDataForCredentialsByname(ExchangeSettings LOCbject)
        {
            DataTable LocationsDataTable = new DataTable();
            try
            {
                string SqlQuery = "Select * from CASServerTests where ServerId = @ServerId";

                SqlCommand cmd = new SqlCommand(SqlQuery);
                cmd.Parameters.AddWithValue("@ServerId", (object)LOCbject.ServerId ?? DBNull.Value);
                LocationsDataTable = objAdaptor.FetchDatafromcommand(cmd);
                //populate & return data object
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return LocationsDataTable;
        }

        public Credentials getdataforcred(Credentials LOCbject)
        {
            DataTable LocationsDataTable = new DataTable();
            Credentials ReturnSTypeobject = new Credentials();
            try
            {
                string SqlQuery = "Select * from Credentials where AliasName = @AliasName";

                SqlCommand cmd = new SqlCommand(SqlQuery);
                cmd.Parameters.AddWithValue("@AliasName", (object)LOCbject.AliasName ?? DBNull.Value);
                LocationsDataTable = objAdaptor.FetchDatafromcommand(cmd);
                if (LocationsDataTable.Rows.Count > 0)
                {
                    if (LocationsDataTable.Rows[0]["ID"].ToString() != "")
                        ReturnSTypeobject.ID = int.Parse(LocationsDataTable.Rows[0]["ID"].ToString());
                }
                else
                {
                    ReturnSTypeobject.ID = 0;
                }
                //populate & return data object
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return ReturnSTypeobject;
        }
		//public DataTable GetAllData1()
		//{
		//    DataTable MSServersDataTable = new DataTable();

		//    try
		//    {
		//        //3/21/2014 NS modified the query - need to add locationid
		//        //string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
		//        //             " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='Exchange' ";
		//        string SqlQuery = "select isnull(es.EnableLatencyTest,'False') EnableLatencyTest,LatencyYellowThreshold,LatencyRedThreshold,Sr.ServerName,L.Location from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
		//                         " inner join Locations L on Sr.LocationID =L.ID left outer join ExchangeSettings es  on s.ID=es.ServerId   where S.ServerType='Exchange' ";
		//        MSServersDataTable = objAdaptor.FetchData(SqlQuery);
		//    }
		//    catch
		//    {
		//    }
		//    finally
		//    {
		//    }
		//    return MSServersDataTable;
		//}
		public DataTable GetAllData1()
		{
			DataTable MSServersDataTable = new DataTable();

			try
			{
				//3/21/2014 NS modified the query - need to add locationid
				//string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
				//             " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='Exchange' ";
				string SqlQuery = "select  es.ServerId,es.EnableLatencyTest,es.LatencyYellowThreshold,es.LatencyRedThreshold,Sr.ServerName,L.Location from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
								 " inner join Locations L on Sr.LocationID =L.ID inner join ExchangeSettings es  on sr.ID=es.ServerId   where S.ServerType='Exchange' ";
				MSServersDataTable = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}
			return MSServersDataTable;
		}
		//somaraju

		public object updateEnableLatencyTest(int id, int yellowthershold, int latencyred, bool checkedvalue)
		{
			//ExchangeServers - ServerId, LatencyEnable, Red,Yellow
			//In DAL,update each item to db
			string SqlQuery = "";
			string parameters = "";
			string tableName = "";
			object Update;
			int count = 0;

			try
			{
				//foreach (ExchangeSettings fieldValue in fieldValues)
				//{
				SqlQuery = "update ExchangeSettings set LatencyYellowThreshold = @yellowthershold, LatencyRedThreshold = @latencyred,EnableLatencyTest = @checkedvalue  where ServerID= @Id";
				SqlCommand cmd = new SqlCommand(SqlQuery);
				cmd.Parameters.AddWithValue("@Id", (object)id ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@yellowthershold", (object)yellowthershold ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@latencyred", (object)latencyred ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@checkedvalue", (object)checkedvalue ?? DBNull.Value);
				Update = objAdaptor.ExecuteNonQuerywithcmd(cmd);
				//Update = objAdaptor.ExecuteNonQuery(SqlQuery);
				//}
			}
			catch
			{
				Update = 0;
			}
			return Update;


		}

        //14/07/2016 sowmya added for VSPLUS-3097
        public DataTable GetCASData(int ServerId)
        {
            DataTable MSServersDataTable = new DataTable();

            try
            {
                string SqlQuery = "select ns.id,ns.ServerId,ns.TestId ,et.[TestName] ,URLs,cd.AliasName  " + 
                    "from CASServerTests ns left outer join [ExchangeTestNames] et on ns.testid = et.testid left outer join Credentials cd on  cd.ID=ns.CredentialsId where ServerId="+ServerId;
  
  

                MSServersDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return MSServersDataTable;
        }

        //7/19/2016 NS added for VSPLUS-3097
        public Object DeleteCASData(string id, string serverid)
        {
            Object Update;
            try
            {
                string SqlQuery = "DELETE FROM [CASServerTests] WHERE [TestId]=" + id + " AND [ServerId]=" + serverid + " ";
                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            return Update;
        }

        public object UpdateCASTestData(string id, string serverid, string url, string credid)
        {
            try
            {
                return VSWebDAL.ConfiguratorDAL.ExchangeDAL.Ins.UpdateCASData(id, serverid, url, credid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateCASData(string id, string serverid, string url, string credid)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "UPDATE [CASServerTests] SET URLs='" + url + "', CredentialsId=" + credid + " " +
                    "WHERE TestId=" + id + " AND ServerId=" + serverid;
                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Insert = false;
            }
            return Insert;
        }

        public bool InsertCASData(string id, string serverid, string url, string credid)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO [CASServerTests] VALUES(" + serverid + "," + id + ",'" + url + "'," + credid + ")";
                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Insert = false;
            }
            return Insert;
        }
    }
}
