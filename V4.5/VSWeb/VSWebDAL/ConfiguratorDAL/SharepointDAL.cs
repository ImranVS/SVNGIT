using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
    public class SharepointDAL
    {
        private Adaptor objAdaptor = new Adaptor();
        private static SharepointDAL _self = new SharepointDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static SharepointDAL Ins
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
                string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
                                 " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='SharePoint' ";
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

		public DataTable GetAllDataByUserID(int UserID)
        {
            DataTable MSServersDataTable = new DataTable();

            try
            {
                //3/21/2014 NS modified the query - need to add locationid
                //string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
                //             " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='Exchange' ";
                string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
                                 " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='SharePoint' and Sr.ID not in(select serverID from UserServerRestrictions ur inner join Users U on ur.UserID= U.ID where U.ID='" + UserID + "') order by ServerName ";
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
		public DataTable GetAllFarmData()
		{
			DataTable MSServersDataTable = new DataTable();

			try
			{
				//3/21/2014 NS modified the query - need to add locationid
				//string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
				//             " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='Exchange' ";
				string SqlQuery = "select  Row_Number() OVER(ORDER BY Farm)as ID,Farm from (select distinct Farm from sharepointfarms where farm not like '%,%' ) tbl ";
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

    }
}
