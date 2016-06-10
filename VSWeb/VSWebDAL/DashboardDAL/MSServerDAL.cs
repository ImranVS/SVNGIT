using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using VSWebDO;
namespace VSWebDAL.ConfiguratorDAL
{
    public class MSServerDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static MSServerDAL _self = new MSServerDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static MSServerDAL Ins
        {
            get { return _self; }
        }

        public DataTable GetAllData()
        {
            DataTable MSServersDataTable = new DataTable();

            try
            {
                string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,MS.ScanInterval,ms.Enabled,sr.ipaddress from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
                             " inner join Locations L on Sr.LocationID =L.ID left join MSServerSettings MS on Sr.ID=MS.ServerID where S.ServerType='Exchange' ";
                MSServersDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return MSServersDataTable;
        }

        public DataTable FillScriptCombo()
        {

            DataTable dt = new DataTable();

            try
            {
                string SqlQuery = "select ScriptName from PowershellScripts ";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return dt;
        }

        public DataTable FillAttributecombo(PowershellScripts obj)
        {
            DataTable dt = new DataTable();

            try
            {
                string SqlQuery = "select Alias from InputParameters where PSID=(select ID from PowershellScripts where ScriptName= '" + obj.ScriptName + "' )";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return dt;
        }

        public DataTable FillThresholdcombo(PowershellScripts obj, InputParameters ipobj)
        {
            DataTable dt = new DataTable();

            try
            {
                string SqlQuery = "select Threshold,Type,ApplyThreshold from InputParameters where PSID=(select ID from PowershellScripts where ScriptName= '" + obj.ScriptName + "') and Alias='" + ipobj.Alias + "'";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return dt;
        }

        public DataTable getthresholdGrid(int ServerID)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "select ID,ServerID,ParameterID,Type,ApplyAlert,PSID,Threshold,(select ScriptName from PowershellScripts where ID= PSID) as ScriptName," +
                   " (select Alias from InputParameters where ID=ParameterID) as Alias from ServerThresholds  where ServerID=" + ServerID + "";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return dt;

        }
        public DataTable getdata(int ServerID)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "select  ms.ScanInterval,(select AliasName from Credentials where ID=CredentialsID)as Credential,  Enabled,RetryInterval,ResponseThreshold,OffscanInterval,FailuresbfrAlert,DiskSpace,CpuUtilization,  NetwrkConn,MemoryUsageAlert,ms.IpAddress,srv.ServerName   from MSServerSettings ms, Servers srv where  ms.ServerID=srv.ID and ms.ServerID=" + ServerID + " ";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return dt;

        }
        public DataTable getpidbypname(string Alias)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "select ID from InputParameters where Alias='" + Alias + "'";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return dt;
        }
        public DataTable getpSIDbySname(string Script)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "select ID from PowershellScripts where ScriptName='" + Script + "'";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return dt;
        }

        public bool InsertServerThreshold(ServerThresholds srt)
        {
            bool result = false;
            try
            {
                string SqlQuery = "insert into ServerThresholds(ServerID,ParameterID,Threshold,Type,ApplyAlert,PSID) " +
                                   " values(" + srt.ServerID + "," + srt.ParameterID + "," + srt.Threshold + ",'" + srt.Type + "','" + srt.ApplyAlert + "'," + srt.PSID + ")";
                result = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return result;

        }

        public bool UpdateServerThreshold(ServerThresholds srt)
        {
            bool result = false;
            try
            {
                string SqlQuery = "Update ServerThresholds set ServerID=" + srt.ServerID + ",ParameterID=" + srt.ParameterID + ",Threshold=" + srt.Threshold +
                    ",Type='" + srt.Type + "',ApplyAlert='" + srt.ApplyAlert + "',PSID=" + srt.PSID + " where ID=" + srt.ID + "";

                result = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return result;
        }
        public bool DeleteServerThreshold(ServerThresholds srt)
        {
            bool result = false;
            try
            {
                string SqlQuery = "Delete from ServerThresholds where ID=" + srt.ID + "";

                result = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return result;
        }

        public DataTable GetCredentials()
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "select * from Credentials";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return dt;
        }

        public bool InsertMSSettings(MSServerSettings msobj)
        {
            bool result = false; string sql = "";
            try
            {
                DataTable dt = getdata(msobj.ServerID);
                if (dt.Rows.Count > 0)
                {
                    sql = "Update MSServerSettings set ScanInterval=" + msobj.ScanInterval + ",CredentialsID=" + msobj.CredentialsID + ",Enabled='" + msobj.Enabled + "',RetryInterval=" + msobj.RetryInterval +
                        ",ResponseThreshold=" + msobj.ResponseThreshold + ",OffscanInterval=" + msobj.OffscanInterval + ",FailuresbfrAlert=" + msobj.FailuresbfrAlert + ",DiskSpace=" + msobj.DiskSpace +
                        " , CpuUtilization=" + msobj.CpuUtilization + ",NetwrkConn='" + msobj.NetwrkConn + "',MemoryUsageAlert=" + msobj.MemoryUsageAlert + ",IpAddress='" + msobj.IpAddresss +
                        "' where ServerID=" + msobj.ServerID + "";

                }
                else
                {
                    sql = "Insert into MSServerSettings (ServerID,ScanInterval,CredentialsID,Enabled,RetryInterval,ResponseThreshold,OffscanInterval,FailuresbfrAlert,DiskSpace," +
                        "CpuUtilization,NetwrkConn,MemoryUsageAlert,IpAddress) values(" + msobj.ServerID + "," + msobj.ScanInterval + "," + msobj.CredentialsID + ",'" + msobj.Enabled + "'," + msobj.RetryInterval +
                        "," + msobj.ResponseThreshold + "," + msobj.OffscanInterval + "," + msobj.FailuresbfrAlert + "," + msobj.DiskSpace +
                        " , " + msobj.CpuUtilization + ",'" + msobj.NetwrkConn + "'," + msobj.MemoryUsageAlert + ",'" + msobj.IpAddresss + "')";
                }
                result = objAdaptor.ExecuteNonQuery(sql);

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        public DataTable GetWindowServices(string ServerName)
        {
            DataTable Charttab = new DataTable();
            try
            {
                string SqlQuery = "Select * from WindowsServices where ServerName='" + ServerName + "'";
                Charttab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return Charttab;
        }
        public bool UpdateServicesforMonitored(string ServerName, string Service_Name, string Monitored)
        {
            bool result = false; string sql = "";
            try
            {
                sql = "Update WindowsServices set Monitored='" + Monitored + "' where ServerName='" + ServerName + "' and Service_Name='" + Service_Name + "'";
                result = objAdaptor.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

		public Object UpdateData(ExchangeServers DSObject, string ServerType)
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
							(DSObject.CredentialsID != 0 ? ", CredentialsID = " + DSObject.CredentialsID : "") + ", ConsFailuresBefAlert=" + DSObject.FailureThreshold +
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
				  ", " + DSObject.Memory_Threshold + ", " + DSObject.CPU_Threshold + "," + DSObject.CredentialsID.ToString() + "," + DSObject.FailureThreshold + "," + "0" + ")";
						Update = objAdaptor.ExecuteNonQuery(SqlQuery);
					}
				}
				else
				{
					string SqlQuery = "INSERT INTO ServerAttributes (ServerID, Category, " +
				  "Enabled, OffHourInterval, " +
				  "ResponseTime, RetryInterval, [ScanInterval]" +
				  ", MemThreshold, CPU_Threshold,CredentialsId,ConsFailuresBefAlert,ConsOvrThresholdBefAlert) " +
				" VALUES (" + DSObject.Key +
				", '" + DSObject.Category +
				"', '" + DSObject.Enabled + "'," + DSObject.OffHoursScanInterval + ", " + DSObject.ResponseThreshold +
				", " + DSObject.RetryInterval + ", " + DSObject.ScanInterval +
				 ", " + DSObject.Memory_Threshold + ", " + DSObject.CPU_Threshold + "," + DSObject.CredentialsID.ToString() + "," + DSObject.FailureThreshold  + "," + "0" + ")";
					Update = objAdaptor.ExecuteNonQuery(SqlQuery);

					

				}

				if (i == 0 || dt.Rows.Count == 0)
				{
					//insert into their respective tables
					string SqlQuery;
					switch (ServerType)
					{

						case "Exchange":

                            SqlQuery = "INSERT INTO ExchangeSettings (ServerID, CasSmtp, CASpop3,CASimap,CASOWA,CASActiveSync,CASEWS,CASECP,CASAutoDiscovery,CASOAB,SUBQTHRESHOLD,POISONQTHRESHOLD,UNREACHABLEQTHRESHOLD,TOTALQTHRESHOLD,VersionNo,AuthenticationType,ShadowQThreshold) " +
										" VALUES (" + DSObject.Key + ",0,0,0,0,0,0,0,0,0,100,100,100,2,0,'" + DSObject.AuthenticationType.ToString() + "',100)";
							Update = objAdaptor.ExecuteNonQuery(SqlQuery);

							break;

						case "Database Availability Group":

							SqlQuery = "INSERT INTO DAGSettings (ServerID, PrimaryConnection, BackupConnection,ReplyQThreshold,CopyQThreshold) " +
										" VALUES (" + DSObject.Key + "," + DSObject.DAGPrimaryServerId.ToString() + "," + DSObject.DAGBackUpServerID.ToString() + "," + DSObject.DAGResponseQTh.ToString() + "," + DSObject.DAGCopyQTh.ToString() + ")";
							Update = objAdaptor.ExecuteNonQuery(SqlQuery);
							
							break;

						case "Active Directory":
							//nothing as of now
							break;

						case "SharePoint":
							//nothing as of now
							break;

						case "Windows":
							//nothing as of now and probably forever
							break;

						case "Skype for Business":
							//nothing as of now
							break;
					}


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

			catch
			{
			}
			finally
			{
			}
			return ReturnDSObject;
		}

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

    }
}
