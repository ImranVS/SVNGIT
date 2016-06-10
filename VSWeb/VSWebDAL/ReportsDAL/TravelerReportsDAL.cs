using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace VSWebDAL.ReportsDAL
{
	public class TravelerReportsDAL
    {
		//2/1/2016 Durga Added for VSPLUS 2174
        private AdaptorforDsahBoard objAdaptor1 = new AdaptorforDsahBoard();
        private Adaptor objAdaptor = new Adaptor();
		private static TravelerReportsDAL _self = new TravelerReportsDAL();

		public static TravelerReportsDAL Ins
        {
            get
            {
                return _self;
            }

        }

		public DataTable GetTravlerData(string servername, DateTime starttime, DateTime endtime, string threshold, string servertype, string StatName)
		{
			DataTable dt = new DataTable();
			string str = "";
			try
			{
				

				System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");

				string start = starttime.ToString(ci);
				string end = endtime.ToString(ci);

				if (servertype == "")
				{
					str = "SELECT ServerName, ROUND(StatValue,2) StatValue, Date, ID " +
								"FROM DominoSummaryStats " +
								"WHERE " +
								"(StatName = '" + StatName + "') " +
								"AND " +
								"(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) " +
								"AND " +
								"(convert(datetime,Date,101) <= convert(datetime,'" + end + "',101)) ";
				}
				else
				{
					str = "SELECT ServerName, ROUND(StatValue,2) StatValue, Date, ID " +
								"FROM DominoSummaryStats INNER JOIN vitalsigns.dbo.Status ON ServerName=Name " +
								"WHERE " +
								"(StatName ='" + StatName + "') " +
								"AND " +
								"(convert(datetime,Date,101) >= convert(datetime,'" + start + "',101)) " +
								"AND " +
								"(convert(datetime,Date,101) <= convert(datetime,'" + end + "',101)) " +
								"AND SecondaryRole='" + servertype + "' ";
				}
				if (servername != "")
				{
					
					str += "AND ServerName IN (" + servername + ") ";
				}
				
				if (threshold != "")
				{
					str += "AND StatValue >= " + threshold;
				}
				dt = objAdaptor1.FetchData(str);
			}
			catch (Exception e)
			{
				throw e;
			}
			return dt;
		}
        //2/19/2016 Durga Added for VSPLUS 2174
        public DataTable getServersForTraveler(string ServerType, string StatName)

        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                if (ServerType == "")
                {
                  
                    str = "SELECT DISTINCT t1.ServerName " +
                        "FROM DominoSummaryStats t1 " +
                        "INNER JOIN vitalsigns.dbo.Servers t2 ON t1.ServerName=t2.ServerName " +
                        "WHERE (StatName ='" + StatName + "') " +
                        "ORDER BY ServerName ";
                }
                else
                {
                    str = "SELECT DISTINCT ServerName " +
                        "FROM DominoSummaryStats INNER JOIN vitalsigns.dbo.Status ON ServerName=Name " +
                        "WHERE (StatName ='" + StatName + "') " +
                        "AND SecondaryRole='" + ServerType + "' " +
                        "ORDER BY ServerName ";
                }
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
        // 3/17/2016 Durga Addded for VSPLUS-2702
        public DataTable GetO365Server()

        {
            DataTable dt = new DataTable();
            string str = "";
            try
            {
                str = " select distinct server from [O365AdditionalMailDetails] ";
              
                dt = objAdaptor1.FetchData(str);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }
    }
}
