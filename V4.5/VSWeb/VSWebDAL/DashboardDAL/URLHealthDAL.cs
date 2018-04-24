using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebDAL.DashboardDAL
{
   public class URLHealthDAL
    {
        private AdaptorforDsahBoard objAdaptor1 = new AdaptorforDsahBoard();
        private Adaptor objAdaptor = new Adaptor();
        private static URLHealthDAL _self = new URLHealthDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static URLHealthDAL Ins
        {
            get { return _self; }
        }

        public DataTable GetData()
        {
            DataTable dt = new DataTable();
            try
            {
                string q = "Select * from Status where Type='URL' ";
                dt = objAdaptor.FetchData(q);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetresponsetimeGraphData(string name)
        {
            DataTable dt = new DataTable();
            try
            {
                //1/11/2013 NS modified
                //string q = "Select Date,Max(StatValue) as StatValue from DeviceDailyStats where DeviceType='URL' and DeviceName='" + name + "' and StatName='ResponseTime' Group by Date order by Date ";
                //12/12/2013 NS modified (column name change)
                //string q = "Select Date,StatValue from DeviceDailyStats " + 
                //    "where DeviceType='URL' and DeviceName='" + name + "' and StatName='ResponseTime' " +
                //    "and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
                //    "order by Date ";
                string q = "Select Date,StatValue from DeviceDailyStats " +
                    "where DeviceType='URL' and ServerName='" + name + "' and StatName='ResponseTime' " +
                    "and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
                    "order by Date ";
                dt = objAdaptor1.FetchData(q);
            }
            catch
            {
            }
            return dt;
        }
        public DataTable GetAvailabilityGraph(string name)
        {
            DataTable dt = new DataTable();
            try
            {
                //2/6/2014 NS modified - statname is HourlyUpTimePercent
                //3/22/2014 NS modified - the graph should display today's data only
                string q = "Select Date,Max(StatValue) as StatValue from DeviceUpTimeStats where DeviceType='URL' and " +
                    "DeviceName='" + name + "' and StatName='HourlyUpTimePercent' " +
                    "and DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) " +
                    "group by Date order by Date ";
                dt = objAdaptor1.FetchData(q);
            }
            catch
            {
            }
            return dt;
        }
    }
}
