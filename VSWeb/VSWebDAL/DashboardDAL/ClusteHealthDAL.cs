using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebDAL.DashboardDAL
{
   public class ClusteHealthDAL
    {
        private AdaptorforDsahBoard objAdaptor1 = new AdaptorforDsahBoard();
        private Adaptor objAdaptor = new Adaptor();
        private static ClusteHealthDAL _self = new ClusteHealthDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static ClusteHealthDAL Ins
        {
            get { return _self; }
        }

        public DataTable Getdata()
        {
            DataTable dt=new DataTable();
            try
            {
                string q = "Select * from DominoClusterHealth";
                dt = objAdaptor.FetchData(q);
            }
            catch
            {

            }
            return dt;
        }

        public DataTable GetGraphValues(string ServerName, string StartDate, string EndDate)
        {
            DataTable dt = new DataTable();
            ServerName = ServerName.Replace("CN=","");
            ServerName = ServerName.Replace("O=","");

            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
            string s = System.Globalization.CultureInfo.CurrentCulture.ToString();
            DateTime dtStart = DateTime.Parse(StartDate);
            DateTime dtEnd = DateTime.Parse(EndDate);

            StartDate = dtStart.ToString(ci);
            EndDate = dtEnd.ToString(ci);

            try
            {
                //1/11/2013 NS modified
                //string q = "Select convert(varchar(5),Date,108) as Date, Max(StatValue) as StatValue FROM DominoDailyStats Where ServerName= '" + ServerName + "' AND StatName= 'Replica.Cluster.SecondsOnQueue' AND Date >= '" + StartDate + "' AND Date <= '" + EndDate + "' group by Date Order By Date ASC";
                //10/9/2013 NS added ServerName
                //string q = "Select ServerName, Date, StatValue FROM DominoDailyStats Where ServerName= '" + ServerName + "' AND StatName= 'Replica.Cluster.SecondsOnQueue' AND Date >= '" + StartDate + "' AND Date <= '" + EndDate + "' Order By Date ASC";
                string q = "Select ServerName, Date, StatValue "+
                            "FROM DominoDailyStats "+
                            "Where ServerName= '" + ServerName + "' AND StatName= 'Replica.Cluster.SecondsOnQueue' AND "+
                            "convert(datetime,Date,101) > '"+StartDate+"' AND "+
                            "convert(datetime,Date,101) <='"+EndDate+"'Order By Date ASC";
                //convert(varchar(5),Date,108)
                dt = objAdaptor1.FetchData(q);
            }
            catch
            {

            }
            return dt;
        }
    }
}
