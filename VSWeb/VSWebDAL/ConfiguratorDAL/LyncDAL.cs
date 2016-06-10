using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
    public class LyncDAL
    {
        private Adaptor objAdaptor = new Adaptor();
        private static LyncDAL _self = new LyncDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static LyncDAL Ins
        {
            get { return _self; }
        }

        public DataTable GetAllData()
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,ls.ScanInterval," +
                    "ls.Enabled,ls.Category FROM Servers Sr INNER JOIN ServerTypes S on Sr.ServerTypeID=S.ID " +
                    "INNER JOIN Locations L ON Sr.LocationID=L.ID LEFT OUTER JOIN LyncServers ls ON sr.ID=ls.serverid " +
					"WHERE S.ServerType='Skype for Business' ";
                dt = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

		public DataTable GetAllDataByUser(int UserID)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,ls.ScanInterval," +
                    "ls.Enabled,ls.Category FROM Servers Sr INNER JOIN ServerTypes S on Sr.ServerTypeID=S.ID " +
                    "INNER JOIN Locations L ON Sr.LocationID=L.ID LEFT OUTER JOIN LyncServers ls ON sr.ID=ls.serverid " +
					"WHERE S.ServerType='Skype for Business'and Sr.ID not in(select serverID from UserServerRestrictions ur inner join Users U on ur.UserID= U.ID where U.ID='" + UserID + "') order by ServerName "; 
                dt = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

        public DataTable GetAllDataByName(Servers ServerObject)
        {

            DataTable ServersDataTable = new DataTable();
            Servers ReturnSerevrbject = new Servers();
            try
            {
                if (ServerObject.ID == 0)
                {
                    string SqlQuery = "select Sr.ID,Sr.ServerName,sr.Description,S.ServerType,sa.CredentialsId,"+
                        "L.Location,sa.Enabled,sa.ScanInterval,sa.RetryInterval,sa.OffHoursScanInterval, "+
                        "sa.category,sa.CPUThreshold,sa.MemoryThreshold,sa.ResponseThreshold,sa.FailureThreshold "+
                        "from Servers Sr inner join ServerTypes S "+
                        "on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID  "+
                        "left outer join LyncServers sa on sr.ID=sa.serverid "+
                        "where sr.ServerName='" + ServerObject.ServerName + "'";
                    ServersDataTable = objAdaptor.FetchData(SqlQuery);
                }
                else
                {
                    string SqlQuery = "select Sr.ID,Sr.ServerName,sr.Description,S.ServerType,L.Location,"+
                        "sa.Enabled,sa.ScanInterval,sa.RetryInterval,sa.OffHoursScanInterval, "+
                        "sa.category,sa.CPUThreshold,sa.MemoryThreshold,sa.ResponseThreshold,sa.FailureThreshold "+
                        "from Servers Sr inner join ServerTypes S "+
                        "on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID  "+
                        "left outer join LyncServers sa on sr.ID=sa.serverid  and Sr.ID<>'" + ServerObject.ID + "'";
                    ServersDataTable = objAdaptor.FetchData(SqlQuery);
                }
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

        public Object UpdateData(LyncServers ServerObject)
        {
            Object Update;
            try
            {
                DataTable dt = GetData(ServerObject.ServerID);
                if (dt.Rows.Count > 0)
                {
                    string SqlQuery = "UPDATE LyncServers SET Enabled='" + ServerObject.Enabled.ToString() +
                        "',ScanInterval=" + ServerObject.ScanInterval + ",RetryInterval=" + ServerObject.RetryInterval +
                        ",OffHoursScanInterval=" + ServerObject.OffHoursScanInterval + ",Category='" + ServerObject.Category +
                        "',CPUThreshold=" + ServerObject.CPUThreshold + ",MemoryThreshold=" + ServerObject.MemoryThreshold + 
                        ",ResponseThreshold=" + ServerObject.ResponseThreshold +
                        ",FailureThreshold=" + ServerObject.FailureThreshold + ",CredentialsId =" + ServerObject.CredentialsID + 
                        " where ServerId=" + ServerObject.ServerID + "";

                    Update = objAdaptor.ExecuteNonQuery(SqlQuery);
                }
                else
                {
                    Update = InsertData(ServerObject);
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

        public bool InsertData(LyncServers ServerObject)
        {
            bool Insert = false;
            try
            {
                string SqlQuery = "INSERT INTO [LyncServers] (ServerID,Category,ScanInterval,RetryInterval,"+
                    "OffHoursScanInterval,ResponseThreshold,FailureThreshold,MemoryThreshold,CPUThreshold,CredentialsID,Enabled) " +
                       " VALUES(" + ServerObject.ServerID + ",'" + ServerObject.Category + "'," + 
                       ServerObject.ScanInterval + "," + ServerObject.RetryInterval + "," + ServerObject.OffHoursScanInterval + "," + 
                       ServerObject.ResponseThreshold + "," + ServerObject.FailureThreshold + "," + ServerObject.MemoryThreshold + "," + 
                       ServerObject.CPUThreshold + "," + ServerObject.CredentialsID + ",'" + ServerObject.Enabled + "')";

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

        public DataTable GetData(int ServerId)
        {
            DataTable ServersDataTable = new DataTable();
            string SqlQuery = "select * from LyncServers where serverid =" + ServerId;
            ServersDataTable = objAdaptor.FetchData(SqlQuery);
            return ServersDataTable;
        }
    }
}
