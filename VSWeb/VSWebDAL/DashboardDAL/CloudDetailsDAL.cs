using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
    public class CloudDetailsDAL
    {
        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();

        private Adaptor adaptor = new Adaptor();
        private static CloudDetailsDAL _self = new CloudDetailsDAL();

        public static CloudDetailsDAL Ins
        {
            get
            {
                return _self;
            }
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


                    dt = objAdaptor.FetchSNMPHourlyVals("ResponseTime", System.DateTime.Now, DeviceName);



                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            return dt;
        }
		        public DataTable SetGraphCloud(string paramGraph, string DeviceName)
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


					dt = objAdaptor.FetchCloudHourlyVals("ResponseTime", System.DateTime.Now, DeviceName);



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



        public DataTable GetcloudServerDetails()
        {
            DataTable dt = new DataTable();

            try
            {

                string SqlQuery = "select  distinct st.Status,st.ResponseTime,sr.Name ServerName from CloudDetails sr inner join status st on st.name=sr.Name where st.Type='Cloud' ";


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

        public DataTable GetIssuesTasks()
        {
            DataTable HAStatus = new DataTable();
            try
            {

                string SqlQuery = "select sd.TestName,sd.Result,sd.LastUpdate,sd.Details,st.Name ServerName from StatusDetail sd inner join status st on sd.TypeANDName=st.TypeANDName where Type='Cloud' and Result='Fail'";

                HAStatus = adaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return HAStatus;
        }
    }
}
