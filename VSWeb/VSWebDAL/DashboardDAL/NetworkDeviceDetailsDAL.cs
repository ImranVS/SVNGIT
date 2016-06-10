using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
    public class NetworkDeviceDetailsDAL
    {
        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
     
        private Adaptor adaptor = new Adaptor();
        private static NetworkDeviceDetailsDAL _self = new NetworkDeviceDetailsDAL();

        public static NetworkDeviceDetailsDAL Ins
        {
            get
            {
                return _self;
            }
        }
		public DataTable getStatusAndLastScanDate(string Servername,string type)
		{
			DataTable dt = new DataTable();
			try
			{
				string Sql = "Select Status, LastUpdate from Status where Type='" + type +"' and Name='" + Servername + "'";
				dt = adaptor.FetchData(Sql);
			}
			catch (Exception)
			{

				throw;
			}
			return dt;
		}
        public DataTable SetGraph(string paramGraph, string DeviceName)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    //string strQuerry = "SELECT [DeviceName], [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + DeviceName + "' order by Date asc";                    

                    //string strQuerry = "select devicename,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //            " from [VSS_Statistics].[dbo].[DeviceDailyStats] " +
                    //            " where Date >= '11/10/2012' and StatName='ResponseTime' and [DeviceName]='" + DeviceName + "' " +
                    //            " group by devicename, convert(varchar(5),Date,108) ";

                    //dt = objAdaptor.FetchData(strQuerry);


					dt = objAdaptor.FetchNetworkDeviceHourlyVals("ResponseTime", System.DateTime.Now, DeviceName);



                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            return dt;
        }


        public DataTable ResponseThreshold(string ServerName)
        {
            DataTable dt = new DataTable();
            try
            {
                string que = "select ServerID,ResponseThreshold from Servers  s inner join DominoServers ds on ds.ServerID= s.ID where s.ServerName='" + ServerName + "'";
                dt = adaptor.FetchData(que);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public DataTable SetGraphForCPU(string paramGraph, string serverName)
		{
			DataTable dt = new DataTable();
			if (paramGraph == "hh")
			{
				try
				{
					dt = objAdaptor.FetchNetworkDeviceHourlyVals("Platform.System.PctCombinedCpuUtil", System.DateTime.Now, serverName);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}

			return dt;
		}

		public DataTable SetGraphForTemperature(string paramGraph, string serverName)
		{
			DataTable dt = new DataTable();
			if (paramGraph == "hh")
			{
				try
				{
					dt = objAdaptor.FetchNetworkDeviceHourlyVals("TemeratureFahrenheit", System.DateTime.Now, serverName);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}

			return dt;
		}

		public DataTable SetGraphForMemory(string paramGraph, string serverName)
		{
			DataTable dt = new DataTable();
			if (paramGraph == "hh")
			{
				try
				{
					dt = objAdaptor.FetchNetworkDeviceHourlyVals("Memory", System.DateTime.Now, serverName);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}

			return dt;
		}

		public DataTable FillDeviceInfo(string serverName)
		{
			DataTable dt = new DataTable();
			try
			{
				string que = "select StatName, StatValue from NetworkDevicesDetails ndd where ndd.NetworkId=(select ID from [Network Devices] where Name='" + serverName + "')";
				dt = adaptor.FetchData(que);
				return dt;
			}
			catch (Exception ex)
			{
				throw ex;
			}


			return dt;
		}

    }
}
