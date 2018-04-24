using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebDAL.DashboardDAL
{
    public class DiskHealthDAL
    {
        //private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
        private Adaptor adaptor = new Adaptor();
        private static DiskHealthDAL _self = new DiskHealthDAL();

        public static DiskHealthDAL Ins
        {
            get
            {
                return _self;
            }
        }


		public DataTable SetGrid()
		{
			DataTable dt = new DataTable();
			try
			{
				//string strQuerry = "SELECT * FROM [dbo].[DominoDiskSpace]  where diskfree is not null and disksize is not null ORDER BY [serverName],diskname";

				//05/02/2014 MD modified, VSPLUS-444
				//string strQuerry = "SELECT sp.ServerName,sp.DiskName,sp.DiskFree,sp.PercentFree,sp.PercentUtilization,sp.AverageQueueLength,sp.ID,sp.DiskSize,sp.Updated,st.Threshold, st.ThresholdType FROM [dbo].[DominoDiskSpace] sp, DominoDiskSettings st " +
				//                "where sp.servername=st.servername and " +
				//                "(st.DiskName='AllDisks' and sp.DiskName<>st.DiskName)  and  diskfree is not null and disksize is not null " +
				//                "union " +
				//                "SELECT sp.ServerName,sp.DiskName,sp.DiskFree,sp.PercentFree,sp.PercentUtilization,sp.AverageQueueLength,sp.ID,sp.DiskSize,sp.Updated,st.Threshold, st.ThresholdType FROM [dbo].[DominoDiskSpace] sp, DominoDiskSettings st " +
				//                "where sp.servername=st.servername and " +
				//                "(st.DiskName<>'NoAlerts' and st.DiskName<>'AllDisks' and sp.DiskName=st.DiskName)  and  diskfree is not null and disksize is not null " +
				//                "ORDER BY sp.serverName,sp.diskname";
				//string strQuerry = "select DSP.ID,DSP.ServerName,DSP.DiskName,DSP.DiskSize,DSP.DiskFree,DSP.PercentFree,DSP.PercentUtilization,DSP.AverageQueueLength,DSP.Updated,DDS.Threshold,DDS.ThresholdType,DDS.DiskName  from V_DominoDiskSpace DSP LEFT OUTER JOIN DominoDiskSettings DDS " +
				//        "ON (DDS.ServerName =DSP.ServerName) and (DSP.DiskName=DDS.DiskName or DDS.DiskName='AllDisks' ) ORDER BY DSP.serverName,DSP.diskname";

				string strQuerry = "select DSP.ID,DSP.ServerName,DDS.DiskInfo,DSP.DiskName,DSP.DiskSize,DSP.DiskFree,DSP.PercentFree,DSP.PercentUtilization,DSP.AverageQueueLength,DSP.Updated,DDS.Threshold,DDS.ThresholdType,DDS.DiskName  from V_DominoDiskSpace DSP LEFT OUTER JOIN DominoDiskSettings DDS  " +
								  "ON (DDS.ServerName =DSP.ServerName) and (DSP.DiskName=DDS.DiskName or DDS.DiskName='AllDisks' ) " +
								  "union " +
								  "SELECT DSP.ID,DSP.ServerName,NULL as DiskInfo,DSP.DiskName,DSP.DiskSize,DSP.DiskFree,DSP.PercentFree,DSP.PercentUtilization,DSP.AverageQueueLength,DSP.Updated,DDS.Threshold,DDS.ThresholdType,DDS.DiskName     " +
								  "FROM dbo.Servers srv INNER JOIN " +
								  "dbo.DiskSettings DDS ON srv.ID = DDS.ServerID RIGHT OUTER JOIN " +
								  "dbo.V_DiskSpace DSP ON srv.ServerName = DSP.ServerName and (DSP.DiskName=DDS.DiskName or DDS.DiskName='AllDisks' ) " +
								  "ORDER BY DSP.serverName,DSP.PercentFree";
				
		dt = adaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
		public DataTable SetGrid(string ServerName)
		{
			DataTable dt = new DataTable();
			try
			{
				//string strQuerry = "SELECT * FROM [dbo].[DominoDiskSpace]  where diskfree is not null and disksize is not null ORDER BY [serverName],diskname";

				//05/02/2014 MD modified, VSPLUS-444
				//string strQuerry = "SELECT sp.ServerName,sp.DiskName,sp.DiskFree,sp.PercentFree,sp.PercentUtilization,sp.AverageQueueLength,sp.ID,sp.DiskSize,sp.Updated,st.Threshold, st.ThresholdType FROM [dbo].[DominoDiskSpace] sp, DominoDiskSettings st " +
				//                "where sp.servername=st.servername and " +
				//                "(st.DiskName='AllDisks' and sp.DiskName<>st.DiskName)  and  diskfree is not null and disksize is not null " +
				//                "union " +
				//                "SELECT sp.ServerName,sp.DiskName,sp.DiskFree,sp.PercentFree,sp.PercentUtilization,sp.AverageQueueLength,sp.ID,sp.DiskSize,sp.Updated,st.Threshold, st.ThresholdType FROM [dbo].[DominoDiskSpace] sp, DominoDiskSettings st " +
				//                "where sp.servername=st.servername and " +
				//                "(st.DiskName<>'NoAlerts' and st.DiskName<>'AllDisks' and sp.DiskName=st.DiskName)  and  diskfree is not null and disksize is not null " +
				//                "ORDER BY sp.serverName,sp.diskname";
                //7/11/2014 NS modified for VSPLUS-813
				/* 
                string strQuerry = "select DSP.ID,DSP.ServerName,DSP.DiskName,DSP.DiskSize,DSP.DiskFree,DSP.PercentFree,DSP.PercentUtilization,DSP.AverageQueueLength,DSP.Updated,DDS.Threshold,DDS.ThresholdType,DDS.DiskName  from V_DominoDiskSpace DSP LEFT OUTER JOIN DominoDiskSettings DDS " +
					"ON (DDS.ServerName =DSP.ServerName and DSP.ServerName='" + ServerName + "') and (DSP.DiskName=DDS.DiskName or DDS.DiskName='AllDisks' ) Where  DSP.ServerName='" + ServerName + "' ORDER BY DSP.serverName,DSP.diskname";
                 */
				string strQuerry = "select DSP.ID,DSP.ServerName,DSP.DiskName,DSP.DiskSize,ROUND(DSP.DiskFree,0) AS DiskFree,DSP.PercentFree,DSP.PercentUtilization,DSP.AverageQueueLength,DSP.Updated,CAST(DDS.Threshold as varchar(50)) + ' ' + CASE WHEN DDS.ThresholdType='Percent' THEN '%' ELSE DDS.ThresholdType END ThresholdDisp,DDS.Threshold,DDS.ThresholdType, DDS.DiskName  from V_DominoDiskSpace DSP LEFT OUTER JOIN DominoDiskSettings DDS " +
                    "ON (DDS.ServerName =DSP.ServerName and DSP.ServerName='" + ServerName + "') and (DSP.DiskName=DDS.DiskName or DDS.DiskName='AllDisks' ) Where  DSP.ServerName='" + ServerName + "' ORDER BY DSP.serverName,DSP.diskname";
				dt = adaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}
        public DataTable SetGridforNotes(string Devicename)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT * FROM [vitalsigns].[dbo].[NotesMailProbeHistory]  where sentdatetime >= DATEADD(d,-1,  getdate()) and Devicename='" + Devicename + "' order by sentdatetime desc";
                dt = adaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable SetGridforExchange(string Devicename)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT * FROM [vitalsigns].[dbo].[ExchangeMailProbeHistory]  where sentdatetime >= DATEADD(d,-1,  getdate()) and Devicename='" + Devicename + "' order by sentdatetime desc";
                dt = adaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable SetGraph(string serverName, string diskname)
        {
            DataTable dt = new DataTable();
            string strQuerry = "";
            try
            {
                //8/21/2014 NS modified
                //string strQuerry = "SELECT [DiskName], [DiskFree], [DiskSize]-[DiskFree] As DiskUsed FROM [DominoDiskSpace] where [ServerName]='" + serverName + "' order by [DiskName]";
               //string strQuerry = " SELECT [DiskName], [DiskFree], [DiskSize]-[DiskFree] As DiskUsed FROM [DominoDiskSpace] where [ServerName]='" + serverName + "' " +
               //                     "union " +
               //                     "SELECT [DiskName], [DiskFree], [DiskSize]-[DiskFree] As DiskUsed FROM [DiskSpace] where [ServerName]='" + serverName + "' order by [DiskName] " ;
                //3/2/2015 NS modified
                if (diskname == "")
                {
                    strQuerry = "SELECT [DiskName], ROUND([DiskFree],2) DiskFree, ROUND([DiskSize]-[DiskFree],2) As DiskUsed " +
                        "FROM [DominoDiskSpace] where [ServerName]='" + serverName + "' " +
                        "union " +
                        "SELECT [DiskName], ROUND([DiskFree],2) DiskFree, ROUND([DiskSize]-[DiskFree],2) As DiskUsed " +
                        "FROM [DiskSpace] where [ServerName]='" + serverName + "' order by [DiskName] ";
                }
                else
                {
                    strQuerry = "SELECT [DiskName], ROUND([DiskFree],2) DiskFree, ROUND([DiskSize]-[DiskFree],2) As DiskUsed " +
                        "FROM [DominoDiskSpace] where [ServerName]='" + serverName + "' AND [DiskName]='" + diskname + "' " +
                        "union " +
                        "SELECT [DiskName], ROUND([DiskFree],2) DiskFree, ROUND([DiskSize]-[DiskFree],2) As DiskUsed " +
                        "FROM [DiskSpace] where [ServerName]='" + serverName + "' AND [DiskName]='" + diskname + "' " + 
                        "ORDER BY [DiskName] ";
                }
                dt = adaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable getFixedDskSize()
        {
            DataTable dt = new DataTable();
            try
            {
                //string strQuerry = "SELECT [DiskName], [DiskFree], [DiskSize]-[DiskFree] As DiskUsed FROM [DominoDiskSpace] where [ServerName]='" + serverName + "' order by [DiskName]";
                string strQuerry = "Select * from Settings where sname = 'DiskYellowThreshold'";
                dt = adaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }


        public DataTable SetGridFromQString(string serverName)
        {
            DataTable dt = new DataTable();
            try
            {

                //string strQuerry = "SELECT * FROM [dbo].[DominoDiskSpace] WHERE [ServerName] = '" + serverName + "'";

                string strQuerry = "SELECT [ServerName],[DiskName],[ServerType],[DiskFree],[DiskSize],[PercentFree],[PercentUtilization],[AverageQueueLength],[Updated],[ID],[Threshold] FROM [DiskSpace] WHERE [ServerName] = '" + serverName + "'" +
                  " union " +
                  " SELECT [ServerName],[DiskName],'Domino' as [ServerType],[DiskFree],[DiskSize],[PercentFree],[PercentUtilization],[AverageQueueLength],[Updated],[ID],[Threshold] FROM [DominoDiskSpace] WHERE [ServerName] = '" + serverName + "'";
  
                dt = adaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable SetExchangeGridFromQString(string serverName)
        {
            DataTable dt = new DataTable();
            try
            {

                //string strQuerry = "SELECT * FROM [dbo].[DominoDiskSpace] WHERE [ServerName] = '" + serverName + "'";

                string strQuerry = "SELECT [ServerName],[DiskName],[ServerType],[DiskFree],[DiskSize],[PercentFree],[PercentUtilization],[AverageQueueLength],[Updated],[ID],[Threshold] FROM [DiskSpace] WHERE [ServerName] = '" + serverName + "'" +
                  " union " +
                  " SELECT [ServerName],[DiskName],'Exchange' as [ServerType],[DiskFree],[DiskSize],[PercentFree],[PercentUtilization],[AverageQueueLength],[Updated],[ID],[Threshold] FROM [DominoDiskSpace] WHERE [ServerName] = '" + serverName + "'";

                dt = adaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

		public DataTable SetGridForExchange(string ServerName)
		{

			DataTable dt = new DataTable();
			try
			{

				string strQuerry = "" +
					"select ds.ID, ds.ServerName, ds.DiskName, ds.DiskSize, ds.DiskFree, ds.PercentFree, ds.PercentUtilization, " +
					"ds.AverageQueueLength, ds.Updated, " +
					"CAST(dset.Threshold as varchar(50)) + ' ' + CASE WHEN dset.ThresholdType='Percent' THEN '%' " +
					"ELSE dset.ThresholdType END ThresholdDisp, dset.Threshold, dset.ThresholdType, ds.DiskName " +
					"from diskspace ds " +
					"inner join servers srv on srv.ServerName = ds.ServerName AND srv.ServerName='" + ServerName + "' " +
					"left outer join disksettings dset on (srv.id=dset.ServerID) and (dset.DiskName='AllDisks' or " +
					"ds.DiskName=dset.DiskName) " +
					"order by ds.ServerName,ds.DiskName ";
				dt = adaptor.FetchData(strQuerry);
				dt = adaptor.FetchData(strQuerry);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;

		}
    }
}
