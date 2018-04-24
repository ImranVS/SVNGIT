using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebDAL.DashboardDAL
{
  public  class BESDAL
    {
        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
        private Adaptor adaptor = new Adaptor();
        private static BESDAL _self = new BESDAL();

        public static BESDAL Ins
        {
            get
            {
                return _self;
            }
        }
        public DataTable SetGraphforBESmsgsent(string paramGraph, string serverName)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    // string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Users' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    //string strQuerry = "select Servername,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //         "from [VSS_Statistics].[dbo].[DominoDailyStats] " +
                    //         "where Date >='11/10/2012' and Date <'11/11/2012' and StatName='Server.Users' and Servername='" + serverName + "'" +
                    //         "group by Servername,convert(varchar(5),Date,108)";
                    //dt = objAdaptor.FetchData(strQuerry);
                    dt = objAdaptor.FetchBESHourlyVals("BES_Messages_Sent", System.DateTime.Now, serverName);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }
      
        public DataTable SetGraphforBESmsgrecvd(string paramGraph, string serverName)
        {
            DataTable dt = new DataTable();
            if (paramGraph == "hh")
            {
                try
                {
                    // string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Server.Users' and Date > DATEADD (" + paramGraph + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + serverName + "' order by Date asc";
                    //string strQuerry = "select Servername,MAX(statvalue) as MaxVal,convert(varchar(5),Date,108) AS Hour " +
                    //         "from [VSS_Statistics].[dbo].[DominoDailyStats] " +
                    //         "where Date >='11/10/2012' and Date <'11/11/2012' and StatName='Server.Users' and Servername='" + serverName + "'" +
                    //         "group by Servername,convert(varchar(5),Date,108)";
                    //dt = objAdaptor.FetchData(strQuerry);
                    dt = objAdaptor.FetchBESHourlyVals("BES_Messages_Received", System.DateTime.Now, serverName);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }
    }
}
