using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
	public class WebSphereServerGridDAL
	{
		private Adaptor objAdaptor = new Adaptor();
		private static WebSphereServerGridDAL _self = new WebSphereServerGridDAL();

		/// <summary>
		/// Used to call the functions using class name instead of object
		/// </summary>
		public static WebSphereServerGridDAL Ins
		{
			get { return _self; }
		}

		public DataTable GetAllData()
		{
			DataTable MSServersDataTable = new DataTable();

			try
			{
				string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category,wc.CellName, wn.NodeName,ws.CellID,ws.NodeID from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID inner Join WebsphereServer ws on ws.ServerName=Sr.ServerName Inner join WebsphereCell wc on wc.CellID=ws.CellID inner join WebsphereNode wn on wn.NodeID=ws.NodeID inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='WebSphere'";
				//string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category,wn.NodeName,wc.CellName from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
				              //  " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid inner join WebsphereServer ws on sr.ID=ws.serverid inner join WebsphereNode wn on wn.NodeID=ws.NodeID inner join WebsphereCell wc on wc.CellID=ws.CellID  where S.ServerType='WebSphere' ";
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
		public DataTable GetAllDatabyUserrestrictions(int UserID)//M.somaraj
		{
			DataTable MSServersDataTable = new DataTable();

			try
			{
				string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category,wc.CellName, wn.NodeName,ws.CellID,ws.NodeID from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID inner Join WebsphereServer ws on ws.ServerName=Sr.ServerName Inner join WebsphereCell wc on wc.CellID=ws.CellID inner join WebsphereNode wn on wn.NodeID=ws.NodeID inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='WebSphere' and Sr.ID not in(select serverID from UserServerRestrictions ur inner join Users U on ur.UserID= U.ID where U.ID='" + UserID + "') order by ServerName";
				//string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category,wn.NodeName,wc.CellName from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
				//  " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid inner join WebsphereServer ws on sr.ID=ws.serverid inner join WebsphereNode wn on wn.NodeID=ws.NodeID inner join WebsphereCell wc on wc.CellID=ws.CellID  where S.ServerType='WebSphere' ";
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

		public DataTable GetAllDataForSametimeServers()


		{
			DataTable MSServersDataTable = new DataTable();

			try
			{

				//string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category,wn.NodeName,wc.CellName from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
								// " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid inner join WebsphereServer ws on sr.ID=ws.serverid inner join WebsphereNode wn on wn.NodeID=ws.NodeID inner join WebsphereCell wc on wc.CellID=ws.CellID  where S.ServerType='Sametime' ";
				//string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category,wn.NodeName,wc.CellName from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid inner join WebsphereServer ws on sr.ID=ws.serverid inner join WebsphereNode wn on wn.NodeID=ws.NodeID inner join WebsphereCell wc on wc.CellID=ws.CellID inner join SametimeServers stime on stime.ServerID=wc.SametimeId where  S.ServerType='Sametime'";
				string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,Sr.LocationID,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category,wn.NodeName,wc.CellName from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID  inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid inner join WebsphereServer ws on sr.ID=ws.serverid inner join WebsphereNode wn on wn.NodeID=ws.NodeID inner join WebsphereCell wc on wc.CellID=ws.CellID  where  ws.Enabled='true' and s.ServerType='WebSphere'";

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


