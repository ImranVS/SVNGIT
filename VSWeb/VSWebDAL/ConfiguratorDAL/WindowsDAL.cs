using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
    public class WindowsDAL
    {
        private Adaptor objAdaptor = new Adaptor();
        private static WindowsDAL _self = new WindowsDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static WindowsDAL Ins
        {
            get { return _self; }
        }

        public DataTable GetAllData()
        {
             DataTable MSServersDataTable = new DataTable();

            try
            {
                
                string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
                                 " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='Windows' ";
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
		public DataTable GetAllDataByUser(int UserID)
        {
             DataTable MSServersDataTable = new DataTable();

            try
            {
                
                string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
                                 " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='Windows'  and Sr.ID not in(select serverID from UserServerRestrictions ur inner join Users U on ur.UserID= U.ID where U.ID='" + UserID + "') order by ServerName ";
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


