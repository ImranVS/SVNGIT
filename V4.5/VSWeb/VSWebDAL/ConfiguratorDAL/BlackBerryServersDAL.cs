using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using System.Configuration;
using System.Data.SqlClient;

namespace VSWebDAL.ConfiguratorDAL
{
	public class BlackBerryServersDAL
	{

		private Adaptor objAdaptor = new Adaptor();
		private static BlackBerryServersDAL _self = new BlackBerryServersDAL();
		public static BlackBerryServersDAL Ins
		{
			get
			{
				return _self;
			}
		}


		public DataTable getfillgrid()
		{
			DataTable dt = new DataTable();
			try
			{
				string s = "select *,servers.ID as SID,ServerName as Name,(Select Location from Locations where ID = LocationID) as Location,Description,IPAddress as Address from BlackBerryServers right join servers on BlackBerryServers.ServerID=servers.ID where servertypeID=(select ID from ServerTypes where ServerType='BES')";
				//string s = "select * from BlackBerryServers";

				dt = objAdaptor.FetchData(s);
			}
			catch (Exception e)
			{
				throw e;
			}
			return dt;
		}
		public DataTable getfillgridbyUser(int UserID)
		{
			DataTable dt = new DataTable();
			try
			{
				string s = "select *,servers.ID as SID,ServerName as Name,(Select Location from Locations where ID = LocationID) as Location,Description,IPAddress as Address from BlackBerryServers right join servers on BlackBerryServers.ServerID=servers.ID where servertypeID=(select ID from ServerTypes where ServerType='BES') and servers.ID not in(select serverID from UserServerRestrictions ur inner join Users U on ur.UserID= U.ID where U.ID = @UserID) order by Name ";
				//string s = "select * from BlackBerryServers";
				SqlCommand cmd = new SqlCommand(s);
				cmd.Parameters.AddWithValue("@UserID", UserID);
				dt = objAdaptor.FetchDatafromcommand(cmd);
				//dt = objAdaptor.FetchData(s);
			}
			catch (Exception e)
			{
				throw e;
			}
			return dt;
		}

		public Object deletefromgrid(BlackBerryServers BlackBerryServerObject)
		{
			Object BlackBerryServer;
			try
			{
				string st = "delete from BlackBerryServers where [key]=" + BlackBerryServerObject.key + "";
				string delServer = "delete from servers where ID=" + BlackBerryServerObject.key;
				BlackBerryServer = objAdaptor.ExecuteNonQuery(delServer);
				BlackBerryServer = objAdaptor.ExecuteNonQuery(st);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return BlackBerryServer;
		}

		public Object insertBlackBerryService(BlackBerryServers BlackBerryServerObject)
		{
			Object insertBlackBerryServer;
			try
			{
				//string str = "insert into BlackBerryServers(Address,Name,Description,Category,ScanInterval,OffHoursScanInterval,Enabled,RetryInterval,Alert,Policy,Synchronization,Controller,Messaging,MDSServices,MDSConnection,OtherServices,BESVersion,Attachment,Router,AlertIP,RouterIP,AttachmentIP,PendingThreshold,TimeZoneAdjustment,USDateFormat,NotificationGroup) values('" + BlackBerryServerObject.Address + "','" + BlackBerryServerObject.Name + "','" + BlackBerryServerObject.Description +
				//   "','" + BlackBerryServerObject.Category + "'," + BlackBerryServerObject.ScanInterval + "," + BlackBerryServerObject.OffHoursScanInterval + ",'" + BlackBerryServerObject.Enabled + "'," + BlackBerryServerObject.RetryInterval + ",'" + BlackBerryServerObject.Alert + "','"+BlackBerryServerObject.Policy+"','"+BlackBerryServerObject.Synchronization+"','"+BlackBerryServerObject.Controller+"','"+BlackBerryServerObject.Messaging+"','"+BlackBerryServerObject.MDSServices+"','"+BlackBerryServerObject.MDSConnection+"','"+BlackBerryServerObject.OtherServices+"','"+BlackBerryServerObject.BESVersion+"','"+BlackBerryServerObject.Attachment+"','"+BlackBerryServerObject.Router+"','"+BlackBerryServerObject.AlertIP+"','"+BlackBerryServerObject.RouterIP+"','"+BlackBerryServerObject.AttachmentIP+"',"+BlackBerryServerObject.PendingThreshold+","+BlackBerryServerObject.TimeZoneAdjustment+",'"+BlackBerryServerObject.USDateFormat+"','"+BlackBerryServerObject.NotificationGroup+"')";
				string str = "insert into BlackBerryServers(ServerID,Category,ScanInterval,OffHoursScanInterval,Enabled,RetryInterval,Alert,Policy,Synchronization,Controller,Messaging,MDSServices,MDSConnection,OtherServices,BESVersion,Attachment,Router,AlertIP,RouterIP,AttachmentIP,PendingThreshold,TimeZoneAdjustment,USDateFormat,NotificationGroup,SNMPCommunity,HAOption,HAPartner) values(@ServerID , @Category ,@ScanInterval ,@OffHoursScanInterval,@Enabled ,@RetryInterval ,@Alert ,@Policy ,@Synchronization ,@Controller,@Messaging,@MDSServices ,@MDSConnection ,@OtherServices ,@BESVersion ,@Attachment ,@Router ,@AlertIP ,@RouterIP ,@AttachmentIP ,@PendingThreshold ,@TimeZoneAdjustment ,@USDateFormat ,@NotificationGroup ,@SNMPCommunity ,@HAOption,@HAPartner)";
				SqlCommand cmd = new SqlCommand(str);
				cmd.Parameters.AddWithValue("@ServerID", BlackBerryServerObject.ServerID);
				cmd.Parameters.AddWithValue("@Category", BlackBerryServerObject.Category);
				cmd.Parameters.AddWithValue("@ScanInterval", BlackBerryServerObject.ScanInterval);
				cmd.Parameters.AddWithValue("@OffHoursScanInterval", BlackBerryServerObject.OffHoursScanInterval);
				cmd.Parameters.AddWithValue("@Enabled", BlackBerryServerObject.Enabled);
				cmd.Parameters.AddWithValue("@RetryInterval", BlackBerryServerObject.RetryInterval);
				cmd.Parameters.AddWithValue("@Alert", BlackBerryServerObject.Alert);
				cmd.Parameters.AddWithValue("@Policy", BlackBerryServerObject.Policy);
				cmd.Parameters.AddWithValue("@Synchronization", BlackBerryServerObject.Synchronization);
				cmd.Parameters.AddWithValue("@Controller", BlackBerryServerObject.Controller);
				cmd.Parameters.AddWithValue("@Messaging", BlackBerryServerObject.Messaging);
				cmd.Parameters.AddWithValue("@MDSServices", BlackBerryServerObject.MDSServices);
				cmd.Parameters.AddWithValue("@MDSConnection", BlackBerryServerObject.MDSConnection);
				cmd.Parameters.AddWithValue("@OtherServices", BlackBerryServerObject.OtherServices);
				cmd.Parameters.AddWithValue("@BESVersion", BlackBerryServerObject.BESVersion);
				cmd.Parameters.AddWithValue("@Attachment", BlackBerryServerObject.Attachment);
				cmd.Parameters.AddWithValue("@Router", BlackBerryServerObject.Router);
				cmd.Parameters.AddWithValue("@AlertIP", BlackBerryServerObject.AlertIP);
				cmd.Parameters.AddWithValue("@RouterIP", BlackBerryServerObject.RouterIP);
				cmd.Parameters.AddWithValue("@AttachmentIP", BlackBerryServerObject.AttachmentIP);
				cmd.Parameters.AddWithValue("@PendingThreshold", BlackBerryServerObject.PendingThreshold);
				cmd.Parameters.AddWithValue("@TimeZoneAdjustment", BlackBerryServerObject.TimeZoneAdjustment);
				cmd.Parameters.AddWithValue("@USDateFormat", BlackBerryServerObject.USDateFormat);
				cmd.Parameters.AddWithValue("@NotificationGroup", BlackBerryServerObject.NotificationGroup);
				cmd.Parameters.AddWithValue("@SNMPCommunity", BlackBerryServerObject.SNMPCommunity);
				cmd.Parameters.AddWithValue("@HAOption", BlackBerryServerObject.HAOption);
				cmd.Parameters.AddWithValue("@HAPartner", BlackBerryServerObject.HAPartner);
				insertBlackBerryServer = objAdaptor.ExecuteNonQuerywithcmd(cmd);
				//insertBlackBerryServer = objAdaptor.ExecuteNonQuery(str);
				//insertBlackBerryServer = objAdaptor.ExecuteNonQuery(str);
			}
			catch (Exception e)
			{
				throw e;
			}
			return insertBlackBerryServer;
		}

		public DataTable filldatabyid(BlackBerryServers BlackBerryserversObject)
		{
			DataTable dt = new DataTable();
			try
			{
				//string s = "Select * from BlackBerryServers where [key]="+BlackBerryserversObject.key;
				//dt=objAdaptor.FetchData(s);
				System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString());
				System.Data.SqlClient.SqlCommand da = new System.Data.SqlClient.SqlCommand("select max(serverID) from BlackBerryServers", con);
				string maxid = string.Empty;
				try
				{
					con.Open();
					object maxsid = da.ExecuteScalar();
					maxid = maxsid.ToString();
				}
				catch { }
				// int mi = Convert.ToInt16(maxid);
				finally
				{
					con.Close();
				}
				if (!string.IsNullOrEmpty(maxid))
				{
					if (int.Parse(maxid) > BlackBerryserversObject.key || int.Parse(maxid) == BlackBerryserversObject.key)
					{
						string q = "select *,ServerName as Name,ID as SID,LocationID,Description,IPAddress from servers left join BlackBerryServers on servers.ID=BlackBerryServers.ServerID where BlackBerryServers.ServerID=@Key";
						SqlCommand cmd = new SqlCommand(q);
						cmd.Parameters.AddWithValue("@Key", BlackBerryserversObject.key);
						dt = objAdaptor.FetchDatafromcommand(cmd);
						//dt = objAdaptor.FetchData(q);
					}
					else
					{
						string s = "select *,ServerName as Name,ID as SID,LocationID,Description,IPAddress from servers left join BlackBerryServers on servers.ID=BlackBerryServers.ServerID where Servers.ID=@Key";
						SqlCommand cmd = new SqlCommand(s);
						cmd.Parameters.AddWithValue("@Key", BlackBerryserversObject.key);
						dt = objAdaptor.FetchDatafromcommand(cmd);
						// dt = objAdaptor.FetchData(s);
						BlackBerryserversObject.Category = "";
					}
				}
				else
				{
					string s = "select *,ServerName as Name,ID as SID,LocationID,Description,IPAddress from servers left join BlackBerryServers on servers.ID=BlackBerryServers.ServerID where Servers.ID=@Key";
					SqlCommand cmd = new SqlCommand(s);
					cmd.Parameters.AddWithValue("@Key", BlackBerryserversObject.key);
					dt = objAdaptor.FetchDatafromcommand(cmd);
					// dt = objAdaptor.FetchData(s);
					BlackBerryserversObject.Category = "";
				}



			}
			catch (Exception e)
			{
				throw e;
			}
			return dt;

		}

		public DataTable getServerID(string servername)
		{
			DataTable dt = new DataTable();
			try
			{
				BlackBerryServers BlackBerryObject = new BlackBerryServers();
				string serversqery = "select * from Servers where ServerName=@ServerName";
				SqlCommand cmd = new SqlCommand(serversqery);
				cmd.Parameters.AddWithValue("@ServerName", servername);
				dt = objAdaptor.FetchDatafromcommand(cmd);
				//dt = objAdaptor.FetchData(serversqery);

				//string finddatawithname = "select * from BlackBerryServers where Name='" +name+ "' and [key]<>"+BlackBerryserverObject.key+"";

				//dt = objAdaptor.FetchData(finddatawithname);
			}
			catch (Exception e)
			{
				throw e;
			}
			return dt;

		}
		private SqlParameter SetDBNullIfEmpty(string parmName, string parmValue)
		{
			return new SqlParameter(parmName, String.IsNullOrEmpty(parmValue) ? DBNull.Value : (object)parmValue);
		}
		public Object updateBlackBerrySever(BlackBerryServers BlackBerryServerObject)
		{
			Object updateBlackBerryServer;
			try
			{
				// string st = "update BlackBerryServers set Address='" + BlackBerryServerObject.Address + "',Name='" + BlackBerryServerObject.Name + "',Description='" + BlackBerryServerObject.Description + "',Category='" + BlackBerryServerObject.Category + "',ScanInterval=" + BlackBerryServerObject.ScanInterval + ",OffHoursScanInterval=" + BlackBerryServerObject.OffHoursScanInterval + ",Enabled='" + BlackBerryServerObject.Enabled + "',RetryInterval=" + BlackBerryServerObject.RetryInterval + ",PendingThreshold='" + BlackBerryServerObject.PendingThreshold+ "',Policy='" + BlackBerryServerObject.Policy + "',Synchronization='" + BlackBerryServerObject.Synchronization + "',Controller='" + BlackBerryServerObject.Controller + "',Messaging='" + BlackBerryServerObject.Messaging + "',MDSServices='" + BlackBerryServerObject.MDSServices + "',MDSConnection='" + BlackBerryServerObject.MDSConnection + "',OtherServices='" + BlackBerryServerObject.OtherServices + "',BESVersion='" + BlackBerryServerObject.BESVersion + "',Attachment='" + BlackBerryServerObject.Attachment + "',Alert='" + BlackBerryServerObject.Alert + "',Router='" + BlackBerryServerObject.Router + "',AlertIP='" + BlackBerryServerObject.AlertIP + "',RouterIP='" + BlackBerryServerObject.RouterIP + "',AttachmentIP='" + BlackBerryServerObject.AttachmentIP + "',USDateFormat='" + BlackBerryServerObject.USDateFormat + "',NotificationGroup='" + BlackBerryServerObject.NotificationGroup + "' where  [key]=" + BlackBerryServerObject.key + "";
				// updateBlackBerryServer=objAdaptor.ExecuteNonQuery(st);

				System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["VitalSignsConnectionString"].ToString());
				System.Data.SqlClient.SqlCommand com = new System.Data.SqlClient.SqlCommand("select max(serverID) from BlackBerryServers", con);
				string maxid = string.Empty;
				try
				{
					con.Open();
					object Maxserverid = com.ExecuteScalar();
					maxid = Maxserverid.ToString();
				}
				catch { }
				finally
				{
					con.Close();
				}
				if (!string.IsNullOrEmpty(maxid))
				{
					if (int.Parse(maxid) > BlackBerryServerObject.ServerID || int.Parse(maxid) == BlackBerryServerObject.ServerID)
					{
						string st = "update BlackBerryServers set Category = @Category, ScanInterval = @ScanInterval ,OffHoursScanInterval= @OffHoursScanInterval,Enabled = @Enabled ,RetryInterval = @RetryInterval,PendingThreshold = @PendingThreshold  ,Policy= @Policy,Synchronization = @Synchronization,Controller = @Controller,Messaging = @Messaging,MDSServices = @MDSServices ,MDSConnection = @MDSConnection,OtherServices = @OtherServices,BESVersion = @BESVersion,Attachment = @Attachment ,Alert = @Alert ,Router = @Router,AlertIP = @AlertIP,RouterIP = @RouterIP,AttachmentIP = @AttachmentIP ,USDateFormat = @USDateFormat,NotificationGroup = @NotificationGroup , SNMPCommunity = @SNMPCommunity, Dispatcher = @Dispatcher, MDS = @MDS , TimeZoneAdjustment = @TimeZoneAdjustment ,HAOption = @HAOption,HAPartner = @HAPartner, ExpiredThreshold = @ExpiredThreshold where  [ServerID] = @key";
						// updateBlackBerryServer = objAdaptor.ExecuteNonQuery(st);
						SqlCommand cmd = new SqlCommand(st);
						cmd.Parameters.AddWithValue("@Key", (object)BlackBerryServerObject.key?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Category", (object)BlackBerryServerObject.Category ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@ScanInterval", (object)BlackBerryServerObject.ScanInterval ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@OffHoursScanInterval", (object)BlackBerryServerObject.OffHoursScanInterval ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Enabled", (object)BlackBerryServerObject.Enabled ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@RetryInterval", (object)BlackBerryServerObject.RetryInterval ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@PendingThreshold", (object)BlackBerryServerObject.PendingThreshold ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Policy", (object)BlackBerryServerObject.Policy ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Synchronization", (object)BlackBerryServerObject.Synchronization ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Controller", (object)BlackBerryServerObject.Controller ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Messaging", (object)BlackBerryServerObject.Messaging ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@MDSServices", (object)BlackBerryServerObject.MDSServices ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@MDSConnection", (object)BlackBerryServerObject.MDSConnection ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@OtherServices", (object)BlackBerryServerObject.OtherServices ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@BESVersion", (object)BlackBerryServerObject.BESVersion ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Attachment", (object)BlackBerryServerObject.Attachment ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Alert", (object)BlackBerryServerObject.Alert ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Router", (object)BlackBerryServerObject.Router ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@AlertIP", (object)BlackBerryServerObject.AlertIP ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@RouterIP", (object)BlackBerryServerObject.RouterIP ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@AttachmentIP", (object)BlackBerryServerObject.AttachmentIP ?? DBNull.Value);
						//cmd.Parameters.AddWithValue("@USDateFormat", BlackBerryServerObject.USDateFormat);
						cmd.Parameters.AddWithValue("@USDateFormat", (object)BlackBerryServerObject.USDateFormat ??DBNull.Value);
						cmd.Parameters.AddWithValue("@NotificationGroup", (object)BlackBerryServerObject.NotificationGroup ?? DBNull.Value);
						//cmd.Parameters.AddWithValue("@NotificationGroup", !String.IsNullOrEmpty(BlackBerryServerObject.NotificationGroup) ? BlackBerryServerObject.NotificationGroup : (object)DBNull.Value);
						//if( string.IsNullOrEmpty(BlackBerryServerObject.NotificationGroup))
						//    cmd.Parameters.AddWithValue("@NotificationGroup", DBNull.Value );
						//else
						//    cmd.Parameters.AddWithValue("@NotificationGroup", BlackBerryServerObject.NotificationGroup);

						cmd.Parameters.AddWithValue("@SNMPCommunity", (object)BlackBerryServerObject.SNMPCommunity ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@Dispatcher", (object)BlackBerryServerObject.Dispatcher ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@MDS", (object)BlackBerryServerObject.MDS);
						cmd.Parameters.AddWithValue("@TimeZoneAdjustment", (object)BlackBerryServerObject.TimeZoneAdjustment ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@HAOption", (object)BlackBerryServerObject.HAOption ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@HAPartner", (object)BlackBerryServerObject.HAPartner ?? DBNull.Value);
						cmd.Parameters.AddWithValue("@ExpiredThreshold", (object)BlackBerryServerObject.ExpiredThreshold ?? DBNull.Value);
						updateBlackBerryServer = objAdaptor.ExecuteNonQuerywithcmd(cmd);
					}
					else
					{
						//    string str = "insert into BlackBerryServers(ServerID,Category,ScanInterval,OffHoursScanInterval,Enabled,RetryInterval,Alert,Policy,Synchronization,Controller,Messaging,MDSServices,MDSConnection,OtherServices,BESVersion,Attachment,Router,AlertIP,RouterIP,AttachmentIP,PendingThreshold,TimeZoneAdjustment,USDateFormat,NotificationGroup,HAOption,HAPartner) values('" + BlackBerryServerObject.ServerID +
						//"','" + BlackBerryServerObject.Category + "'," + BlackBerryServerObject.ScanInterval + "," + BlackBerryServerObject.OffHoursScanInterval + ",'" + BlackBerryServerObject.Enabled + "'," + BlackBerryServerObject.RetryInterval + ",'" + BlackBerryServerObject.Alert + "','" + BlackBerryServerObject.Policy + "','" + BlackBerryServerObject.Synchronization + "','" + BlackBerryServerObject.Controller + "','" + BlackBerryServerObject.Messaging + "','" + BlackBerryServerObject.MDSServices + "','" + BlackBerryServerObject.MDSConnection + "','" + BlackBerryServerObject.OtherServices + "','" + BlackBerryServerObject.BESVersion + "','" + BlackBerryServerObject.Attachment + "','" + BlackBerryServerObject.Router + "','" + BlackBerryServerObject.AlertIP + "','" + BlackBerryServerObject.RouterIP + "','" + BlackBerryServerObject.AttachmentIP + "'," + BlackBerryServerObject.PendingThreshold + "," + BlackBerryServerObject.TimeZoneAdjustment + ",'" + BlackBerryServerObject.USDateFormat + "','" + BlackBerryServerObject.NotificationGroup + "','" +BlackBerryServerObject.HAOption+"','" +BlackBerryServerObject.HAPartner+"')";

						//    updateBlackBerryServer = objAdaptor.ExecuteNonQuery(str);
						string str = "insert into BlackBerryServers(ServerID,Category,ScanInterval,OffHoursScanInterval,Enabled,RetryInterval,Alert,Policy,Synchronization,Controller,Messaging,MDSServices,MDSConnection,OtherServices,BESVersion,Attachment,Router,AlertIP,RouterIP,AttachmentIP,PendingThreshold,TimeZoneAdjustment,USDateFormat,NotificationGroup,HAOption,HAPartner) values(@ServerID , @Category ,@ScanInterval ,@OffHoursScanInterval,@Enabled ,@RetryInterval ,@Alert ,@Policy ,@Synchronization ,@Controller,@Messaging,@MDSServices ,@MDSConnection ,@OtherServices ,@BESVersion ,@Attachment ,@Router ,@AlertIP ,@RouterIP ,@AttachmentIP ,@PendingThreshold ,@TimeZoneAdjustment ,@USDateFormat ,@NotificationGroup ,@HAOption,@HAPartner)";
						SqlCommand cmd = new SqlCommand(str);
						cmd.Parameters.AddWithValue("@ServerID", BlackBerryServerObject.ServerID);
						cmd.Parameters.AddWithValue("@Category", BlackBerryServerObject.Category);
						cmd.Parameters.AddWithValue("@ScanInterval", BlackBerryServerObject.ScanInterval);
						cmd.Parameters.AddWithValue("@OffHoursScanInterval", BlackBerryServerObject.OffHoursScanInterval);
						cmd.Parameters.AddWithValue("@Enabled", BlackBerryServerObject.Enabled);
						cmd.Parameters.AddWithValue("@RetryInterval", BlackBerryServerObject.RetryInterval);
						cmd.Parameters.AddWithValue("@Alert", BlackBerryServerObject.Alert);
						cmd.Parameters.AddWithValue("@Policy", BlackBerryServerObject.Policy);
						cmd.Parameters.AddWithValue("@Synchronization", BlackBerryServerObject.Synchronization);
						cmd.Parameters.AddWithValue("@Controller", BlackBerryServerObject.Controller);
						cmd.Parameters.AddWithValue("@Messaging", BlackBerryServerObject.Messaging);
						cmd.Parameters.AddWithValue("@MDSServices", BlackBerryServerObject.MDSServices);
						cmd.Parameters.AddWithValue("@MDSConnection", BlackBerryServerObject.MDSConnection);
						cmd.Parameters.AddWithValue("@OtherServices", BlackBerryServerObject.OtherServices);
						cmd.Parameters.AddWithValue("@BESVersion", BlackBerryServerObject.BESVersion);
						cmd.Parameters.AddWithValue("@Attachment", BlackBerryServerObject.Attachment);
						cmd.Parameters.AddWithValue("@Router", BlackBerryServerObject.Router);
						cmd.Parameters.AddWithValue("@AlertIP", BlackBerryServerObject.AlertIP);
						cmd.Parameters.AddWithValue("@RouterIP", BlackBerryServerObject.RouterIP);
						cmd.Parameters.AddWithValue("@AttachmentIP", BlackBerryServerObject.AttachmentIP);
						cmd.Parameters.AddWithValue("@PendingThreshold", BlackBerryServerObject.PendingThreshold);
						cmd.Parameters.AddWithValue("@TimeZoneAdjustment", BlackBerryServerObject.TimeZoneAdjustment);
						cmd.Parameters.AddWithValue("@USDateFormat", BlackBerryServerObject.USDateFormat);
						cmd.Parameters.AddWithValue("@NotificationGroup", BlackBerryServerObject.NotificationGroup);
						//cmd.Parameters.AddWithValue("@SNMPCommunity", BlackBerryServerObject.SNMPCommunity);
						cmd.Parameters.AddWithValue("@HAOption", BlackBerryServerObject.HAOption);
						cmd.Parameters.AddWithValue("@HAPartner", BlackBerryServerObject.HAPartner);
						updateBlackBerryServer = objAdaptor.ExecuteNonQuerywithcmd(cmd);
					}
				}
				else
				{
					//string str = "insert into BlackBerryServers(ServerID,Category,ScanInterval,OffHoursScanInterval,Enabled,RetryInterval,Alert,Policy,Synchronization,Controller,Messaging,MDSServices,MDSConnection,OtherServices,BESVersion,Attachment,Router,AlertIP,RouterIP,AttachmentIP,PendingThreshold,TimeZoneAdjustment,USDateFormat,NotificationGroup) values('" + BlackBerryServerObject.ServerID +
					//"','" + BlackBerryServerObject.Category + "'," + BlackBerryServerObject.ScanInterval + "," + BlackBerryServerObject.OffHoursScanInterval + ",'" + BlackBerryServerObject.Enabled + "'," + BlackBerryServerObject.RetryInterval + ",'" + BlackBerryServerObject.Alert + "','" + BlackBerryServerObject.Policy + "','" + BlackBerryServerObject.Synchronization + "','" + BlackBerryServerObject.Controller + "','" + BlackBerryServerObject.Messaging + "','" + BlackBerryServerObject.MDSServices + "','" + BlackBerryServerObject.MDSConnection + "','" + BlackBerryServerObject.OtherServices + "','" + BlackBerryServerObject.BESVersion + "','" + BlackBerryServerObject.Attachment + "','" + BlackBerryServerObject.Router + "','" + BlackBerryServerObject.AlertIP + "','" + BlackBerryServerObject.RouterIP + "','" + BlackBerryServerObject.AttachmentIP + "'," + BlackBerryServerObject.PendingThreshold + "," + BlackBerryServerObject.TimeZoneAdjustment + ",'" + BlackBerryServerObject.USDateFormat + "','" + BlackBerryServerObject.NotificationGroup + "','" +BlackBerryServerObject.HAOption+"','" +BlackBerryServerObject.HAPartner+ "')";

					//updateBlackBerryServer = objAdaptor.ExecuteNonQuery(str);
					string str = "insert into BlackBerryServers(ServerID,Category,ScanInterval,OffHoursScanInterval,Enabled,RetryInterval,Alert,Policy,Synchronization,Controller,Messaging,MDSServices,MDSConnection,OtherServices,BESVersion,Attachment,Router,AlertIP,RouterIP,AttachmentIP,PendingThreshold,TimeZoneAdjustment,USDateFormat,NotificationGroup) values(@ServerID , @Category ,@ScanInterval ,@OffHoursScanInterval,@Enabled ,@RetryInterval ,@Alert ,@Policy ,@Synchronization ,@Controller,@Messaging,@MDSServices ,@MDSConnection ,@OtherServices ,@BESVersion ,@Attachment ,@Router ,@AlertIP ,@RouterIP ,@AttachmentIP ,@PendingThreshold ,@TimeZoneAdjustment ,@USDateFormat ,@NotificationGroup)";
					SqlCommand cmd = new SqlCommand(str);
					cmd.Parameters.AddWithValue("@ServerID", BlackBerryServerObject.ServerID);
					cmd.Parameters.AddWithValue("@Category", BlackBerryServerObject.Category);
					cmd.Parameters.AddWithValue("@ScanInterval", BlackBerryServerObject.ScanInterval);
					cmd.Parameters.AddWithValue("@OffHoursScanInterval", BlackBerryServerObject.OffHoursScanInterval);
					cmd.Parameters.AddWithValue("@Enabled", BlackBerryServerObject.Enabled);
					cmd.Parameters.AddWithValue("@RetryInterval", BlackBerryServerObject.RetryInterval);
					cmd.Parameters.AddWithValue("@Alert", BlackBerryServerObject.Alert);
					cmd.Parameters.AddWithValue("@Policy", BlackBerryServerObject.Policy);
					cmd.Parameters.AddWithValue("@Synchronization", BlackBerryServerObject.Synchronization);
					cmd.Parameters.AddWithValue("@Controller", BlackBerryServerObject.Controller);
					cmd.Parameters.AddWithValue("@Messaging", BlackBerryServerObject.Messaging);
					cmd.Parameters.AddWithValue("@MDSServices", BlackBerryServerObject.MDSServices);
					cmd.Parameters.AddWithValue("@MDSConnection", BlackBerryServerObject.MDSConnection);
					cmd.Parameters.AddWithValue("@OtherServices", BlackBerryServerObject.OtherServices);
					cmd.Parameters.AddWithValue("@BESVersion", BlackBerryServerObject.BESVersion);
					cmd.Parameters.AddWithValue("@Attachment", BlackBerryServerObject.Attachment);
					cmd.Parameters.AddWithValue("@Router", BlackBerryServerObject.Router);
					cmd.Parameters.AddWithValue("@AlertIP", BlackBerryServerObject.AlertIP);
					cmd.Parameters.AddWithValue("@RouterIP", BlackBerryServerObject.RouterIP);
					cmd.Parameters.AddWithValue("@AttachmentIP", BlackBerryServerObject.AttachmentIP);
					cmd.Parameters.AddWithValue("@PendingThreshold", BlackBerryServerObject.PendingThreshold);
					cmd.Parameters.AddWithValue("@TimeZoneAdjustment", BlackBerryServerObject.TimeZoneAdjustment);
					cmd.Parameters.AddWithValue("@USDateFormat", BlackBerryServerObject.USDateFormat);
					cmd.Parameters.AddWithValue("@NotificationGroup", BlackBerryServerObject.NotificationGroup);
					//cmd.Parameters.AddWithValue("@SNMPCommunity", BlackBerryServerObject.SNMPCommunity);
					//cmd.Parameters.AddWithValue("@HAOption", BlackBerryServerObject.HAOption);
					//cmd.Parameters.AddWithValue("@HAPartner", BlackBerryServerObject.HAPartner);
					updateBlackBerryServer = objAdaptor.ExecuteNonQuerywithcmd(cmd);

				}



			}
			catch (Exception e)
			{
				throw e;
			}
			return updateBlackBerryServer;
		}

		public DataTable getthevaluewithname(BlackBerryServers BlackBerryserverObject, string name)
		{
			DataTable dt = new DataTable();
			try
			{
				BlackBerryServers BlackBerryObject = new BlackBerryServers();
				string finddatawithname = "select * from BlackBerryServers where ServerID = @ServerID";
				SqlCommand cmd = new SqlCommand(finddatawithname);
				cmd.Parameters.AddWithValue("@ServerID", BlackBerryObject.ServerID);
				dt = objAdaptor.FetchDatafromcommand(cmd);
				// dt = objAdaptor.FetchData(finddatawithname);
				//string finddatawithname = "select * from BlackBerryServers where Name='" +name+ "' and [key]<>"+BlackBerryserverObject.key+"";

				//dt = objAdaptor.FetchData(finddatawithname);
			}
			catch (Exception e)
			{
				throw e;
			}
			return dt;
		}

		//public DataTable getkey(string name)
		//{
		//    DataTable dt = new DataTable();
		//    try
		//    {
		//        string s = "select * from BlackBerryServers where ServerID='" + name + "'";
		//        dt = objAdaptor.FetchData(s);
		//    }
		//    catch (Exception e)
		//    {
		//        throw e;
		//    }
		//    return dt;
		//}





		//public Object Insert(BlackBerryServers BlackBerryServerObject)
		//{

		//    Object insertBlackBerryServer;
		//    try
		//    {
		//        //string str = "insert into BlackBerryServers(Address,Name,Description,Category,ScanInterval,OffHoursScanInterval,Enabled,RetryInterval,Alert,Policy,Synchronization,Controller,Messaging,MDSServices,MDSConnection,OtherServices,BESVersion,Attachment,Router,AlertIP,RouterIP,AttachmentIP,PendingThreshold,TimeZoneAdjustment,USDateFormat,NotificationGroup) values('" + BlackBerryServerObject.Address + "','" + BlackBerryServerObject.Name + "','" + BlackBerryServerObject.Description +
		//        //   "','" + BlackBerryServerObject.Category + "'," + BlackBerryServerObject.ScanInterval + "," + BlackBerryServerObject.OffHoursScanInterval + ",'" + BlackBerryServerObject.Enabled + "'," + BlackBerryServerObject.RetryInterval + ",'" + BlackBerryServerObject.Alert + "','"+BlackBerryServerObject.Policy+"','"+BlackBerryServerObject.Synchronization+"','"+BlackBerryServerObject.Controller+"','"+BlackBerryServerObject.Messaging+"','"+BlackBerryServerObject.MDSServices+"','"+BlackBerryServerObject.MDSConnection+"','"+BlackBerryServerObject.OtherServices+"','"+BlackBerryServerObject.BESVersion+"','"+BlackBerryServerObject.Attachment+"','"+BlackBerryServerObject.Router+"','"+BlackBerryServerObject.AlertIP+"','"+BlackBerryServerObject.RouterIP+"','"+BlackBerryServerObject.AttachmentIP+"',"+BlackBerryServerObject.PendingThreshold+","+BlackBerryServerObject.TimeZoneAdjustment+",'"+BlackBerryServerObject.USDateFormat+"','"+BlackBerryServerObject.NotificationGroup+"')";
		//        string str = "insert into BlackBerryServers(HAOption,HAPartner) values('" + BlackBerryServerObject.HAOption + "','" + BlackBerryServerObject.HAPartner +  "')";
		//        insertBlackBerryServer = objAdaptor.ExecuteNonQuery(str);
		//        //insertBlackBerryServer = objAdaptor.ExecuteNonQuery(str);
		//    }
		//    catch (Exception e)
		//    {
		//        throw e;
		//    }
		//    return insertBlackBerryServer;
		//}


		// 26/06/2014

		public DataTable fillcombo1(BlackBerryServers BlackBerryserverObject)
		{
			DataTable dt = new DataTable();
			try
			{
				// BlackBerryServers BlackBerryObject = new BlackBerryServers();
				//string query = "select ServerName,ServerID  from Servers s inner join BlackBerryServers b on s.ID= b.ServerID where ServerID = '"+BlackBerryserverObject.ServerID+"'";
				string query = "select * from Servers where id <>" + BlackBerryserverObject.ServerID.ToString() + " and servertypeid=2 order by ServerName";
				dt = objAdaptor.FetchData(query);
				//string finddatawithname = "select * from BlackBerryServers where Name='" +name+ "' and [key]<>"+BlackBerryserverObject.key+"";

				//dt = objAdaptor.FetchData(finddatawithname);
			}
			catch (Exception e)
			{
				throw e;
			}
			return dt;

		}


		public DataTable getHAName(string Id)
		{
			DataTable dt = new DataTable();
			try
			{
				//  BlackBerryServers BlackBerryObject = new BlackBerryServers();
				string query = "select ServerName  from Servers s where s.ID=@ID";
				SqlCommand cmd = new SqlCommand(query);
				cmd.Parameters.AddWithValue("@ID", Id);
				dt = objAdaptor.FetchDatafromcommand(cmd);
				// dt = objAdaptor.FetchData(query);

			}
			catch (Exception e)
			{
				throw e;
			}
			return dt;

		}

		public DataTable GetBESName(string Name)
		{
			//SametimeServers SametimeObj = new SametimeServers();
			DataTable dt = new DataTable();
			try
			{

				string sqlQuery = "Select * from Status where Name= @Name ";
				SqlCommand cmd = new SqlCommand(sqlQuery);
				cmd.Parameters.AddWithValue("@Name", Name);
				dt = objAdaptor.FetchDatafromcommand(cmd);
				//dt = objAdaptor.FetchData(sqlQuery);

			}
			catch (Exception)
			{

				throw;
			}
			return dt;

		}
	}
}

