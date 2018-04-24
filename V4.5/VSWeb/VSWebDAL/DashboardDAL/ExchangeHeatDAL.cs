using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.DashboardDAL
{
	public class ExchangeHeatDAL
	{
		private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
		private Adaptor adaptor = new Adaptor();
		private static ExchangeHeatDAL _self = new ExchangeHeatDAL();

		public static ExchangeHeatDAL Ins
		{
			get
			{
				return _self;
			}

			//GetExchangeHeatMapDetails
		}
		public DataTable getexchangeheatmap()
		{
			DataTable dt = new DataTable();

			try
			{
				dt = objAdaptor.GetDataFrompivotProcedure("GetExchangeHeatmap");
			}
			catch
			{
			}
			finally
			{
			}
			return dt;
		}
		public DataTable SetGraphForLatency(DateTime frmdate, string statname, string DeviceName)
		{
			try
			{

				DataTable dt = new DataTable();


				dt = objAdaptor.FetchLatencyHourlyVals(frmdate, statname, DeviceName);
				return dt;
			}

			catch (Exception ex)
			{
				throw ex;
			}

		}



		//public DataTable GetExchangeHeatMapDetails()
		//{
		//    DataTable MSServersDataTable = new DataTable();

		//    try
		//    {
		//        //3/21/2014 NS modified the query - need to add locationid
		//        //string SqlQuery = "select Sr.ID,Sr.ServerName,S.ServerType,L.Location,sa.ScanInterval,sa.Enabled,sr.ipaddress,sa.category from Servers Sr inner join ServerTypes S on Sr.ServerTypeID=S.ID " +
		//        //             " inner join Locations L on Sr.LocationID =L.ID left outer join ServerAttributes sa on sr.ID=sa.serverid  where S.ServerType='Exchange' ";
		//        string SqlQuery = "select sourceserver from MailLatencyStats  pivot ( sum(Latency) for DestinationServer IN([JNITTECH-EXCHG1],[EX13-1],[EX13-2],[JNITTECH-EXCHG2],[sts],[sds]))  pv";
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
	}
}
