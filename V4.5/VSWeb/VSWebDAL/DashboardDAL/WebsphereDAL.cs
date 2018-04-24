using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.DashboardDAL
{
	public class WebsphereDAL
	{
		
		private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
		private Adaptor adaptor = new Adaptor();
		private static WebsphereDAL _self = new WebsphereDAL();

		public static WebsphereDAL Ins
		{
			get
			{
				return _self;
			}

			//GetExchangeHeatMapDetails
		}




		public DataTable GetWebsphereCellStatus()
		{
			DataTable dt = new DataTable();
			string SqlQuery = "";
			try
			{

				SqlQuery = "select * from WebsphereCellStats";

				dt = adaptor.FetchData(SqlQuery);
			}
			catch
			{
			}
			finally
			{
			}
			return dt;
		}


		public DataTable GetWebsphereNodeStatus(int value)


		{
			DataTable dt = new DataTable();
			string SqlQuery = "";
			try
			{

				SqlQuery = "select * from WebsphereNode where CellID=" + value + "";

				dt = adaptor.FetchData(SqlQuery);
			}
			catch
			{
			}
			finally
			{
			}
			return dt;
		}
		public DataTable GetWebsphereseversStatus(int cellvalue)
		{
			DataTable dt = new DataTable();
			string SqlQuery = "";
			try
			{

                SqlQuery = "select ws.*,NodeName,'WebSphereServerDetailsPage.aspx?Name=' + st.Name + '&Type=' + st.Type + '&Status=' + st.Status + '&LastDate='+CONVERT(VARCHAR,st.LastUpdate,101) + ' ' + substring(convert(varchar(20), LastUpdate, 9), 13, 5) + ' ' + substring(convert(varchar(30), st.LastUpdate, 9), 25, 2)  as redirectto " +
                    "from status st, WebsphereServer ws inner join WebsphereNode wn on ws.NodeID=wn.NodeID and ws.CellID=wn.CellID " +
                    "where st.name=ws.ServerName and ws.CellID='" + cellvalue + "'";
                    //"select * from WebsphereServer where NodeID=" + nodevalue + "";





				SqlQuery =
@"SELECT pTbl.*, NodeName, st.Status, ws.ServerID ID, 
'WebSphereServerDetailsPage.aspx?Name=' + st.Name + '&Type=' + st.Type + '&Status=' + st.Status + '&LastDate='+CONVERT(VARCHAR,st.LastUpdate,101) + ' ' + substring(convert(varchar(20), LastUpdate, 9), 13, 5) + ' ' + substring(convert(varchar(30), st.LastUpdate, 9), 25, 2)  as redirectto 
from (
	select ServerName, 
	[ActiveThreadCount],[ClearedHungThreadCount],[CurrentHungThreadCount],[DeclaredHungThreadCount],[Memory],[CurrentHeapSize],[AveragePoolSize],[UpTime],[ProcessCPUUsage]
	from (
		SELECT tbl.StatName, StatValue, mds.ServerName
		FROM(
			SELECT ServerName, StatName, MAX(Date) As Date
			FROM VSS_Statistics.dbo.WebSphereDailyStats
			WHERE ServerName in (select ServerName from WebSphereServer where CellId='" + cellvalue + @"')
			GROUP BY StatName, ServerName
		) tbl
		inner JOIN
		VSS_Statistics.dbo.WebSphereDailyStats mds ON tbl.Statname = mds.Statname AND tbl.Date = mds.Date and mds.ServerName=tbl.ServerName
	) t 
	PIVOT(
	MAX(StatValue) for StatName in ([ActiveThreadCount],[ClearedHungThreadCount],[CurrentHungThreadCount],[DeclaredHungThreadCount],[Memory],[CurrentHeapSize],[AveragePoolSize],[UpTime],[ProcessCPUUsage])
	) p
) pTbl
INNER JOIN WebSphereServer ws on ws.ServerName=pTbl.ServerName
INNER JOIN WebSphereNode wn on ws.NodeID=wn.NodeID
left outer JOIN Status st on st.TypeANDName = ws.ServerName + '-WebSphere'";


				dt = adaptor.FetchData(SqlQuery);
			}
			catch
			{
			}
			finally
			{
			}
			return dt;
		}

        public DataTable GetWebsphereStatusAll()
        {
            DataTable dt = new DataTable();
            string SqlQuery = "";
            try
            {

                SqlQuery = "SELECT SUM(StatusCount),Status FROM " +
                 "( " +
                 "SELECT COUNT(Status) StatusCount,Status FROM Status " +
				 "WHERE Type='WebSphere' " +
                 "GROUP BY Status " +
                 "UNION " +
                 "SELECT COUNT(Status) StatusCount,Status FROM WebsphereCellStats " +
                 "GROUP BY Status " +
                 "UNION " +
                 "SELECT COUNT(Status) StatusCount,Status FROM WebsphereNode " +
                 "GROUP BY Status " +
                 ") t " +
                 "GROUP BY t.Status ";
                dt = adaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }
	}
}	
				
				
		
