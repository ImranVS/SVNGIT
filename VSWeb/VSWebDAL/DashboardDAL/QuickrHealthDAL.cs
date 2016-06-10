using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;


namespace VSWebDAL.DashboardDAL
{
    public class QuickrHealthDAL
    {
        private Adaptor objAdaptor = new Adaptor();
        //private AdaptorforDsahBoard objAdaptor1 = new AdaptorforDsahBoard();
        private static QuickrHealthDAL _self = new QuickrHealthDAL();

        public static QuickrHealthDAL Ins
        {
            get
            {
                return _self;
            }
        }

        public DataTable SetQuickrServersGrid(string serverName)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuerry = "SELECT * FROM [VitalSigns].[dbo].[Status] WHERE [Type] = '" + serverName + "' and ([SecondaryRole] LIKE '%Quickr%' or [SecondaryRole] LIKE '%quickr%' )";                
                //string strQuerry = "SELECT [Name], [Location], [Details], [UserCount], [dominoversion], [CPU], [Memory], [TypeANDName] FROM [VitalSigns].[dbo].[Status] WHERE [Type] = '" + serverName + "' and [SecondaryRole] = 'Quickr'";                
                dt = objAdaptor.FetchData(strQuerry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable getLastScanDate(string Servername)
        {
            DataTable dt = new DataTable();
            try 
	        {	        
		      string Sql="Select LastUpdate from Status where Name='"+Servername+"'";
                dt=objAdaptor.FetchData(Sql);
	        }
	        catch (Exception)
	        {
		
		        throw;
	        }
            return dt;
        }

        public DataTable SetGraphForResponseTime(string paramVal, string serverName)
        {
            DataTable dt = new DataTable();
            if (paramVal == "hh")
            {
                try
                {
                   // string strQuerry = "SELECT [DeviceName], [DeviceType], CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramVal + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + serverName + "' order by Date asc";
                    //1/9/2013 NS modified the query below to remove hard coded values
                    //string strQuerry = "select devicename,MAX(statvalue) as statvalue,DATEPART(hour, Date) AS Date " +
                    //           " from [VSS_Statistics].[dbo].[DeviceDailyStats] " +
                    //           " where Date >= '11/10/2012' and StatName='ResponseTime' and [DeviceName]='" + serverName + "'" +
                    //           " group by devicename,DATEPART(hour, Date) ";
                    //12/12/2013 NS modified (column name change)
                    //string strQuerry = "select devicename,statvalue,Date " +
                    //           " from [VSS_Statistics].[dbo].[DeviceDailyStats] " +
                    //           " where DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,GETDATE()) and StatName='ResponseTime' and [DeviceName]='" + serverName + "'" +
                    //           " order by Date ";
                    string strQuerry = "select ServerName devicename,statvalue,Date " +
                               " from [VSS_Statistics].[dbo].[DeviceDailyStats] " +
                               " where DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,GETDATE()) and StatName='ResponseTime' and [ServerName]='" + serverName + "'" +
                               " order by Date ";
                    dt = objAdaptor.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                   // string strQuerry = "SELECT [DeviceName], [DeviceType], [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DeviceDailyStats] where [StatName]='ResponseTime' and Date > DATEADD (" + paramVal + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and [DeviceName]='" + serverName + "' order by Date asc";
                    //12/12/2013 NS modified (column name change)
                    //string strQuerry = "select devicename,MAX(statvalue) as statvalue,DATEPART(hour, Date) AS Date " +
                    //        " from [VSS_Statistics].[dbo].[DeviceDailyStats] " +
                    //        " where Date >= '11/10/2012' and StatName='ResponseTime' and [DeviceName]='" + serverName + "'" +
                    //        " group by devicename,DATEPART(hour, Date) ";
                    string strQuerry = "select ServerName devicename,MAX(statvalue) as statvalue,DATEPART(hour, Date) AS Date " +
                            " from [VSS_Statistics].[dbo].[DeviceDailyStats] " +
                            " where Date >= '11/10/2012' and StatName='ResponseTime' and [ServerName]='" + serverName + "'" +
                            " group by ServerName,DATEPART(hour, Date) ";
                    dt = objAdaptor.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }

        public DataTable SetGraphForHttpSessions(string paramval, string servername)
        {
            DataTable dt = new DataTable();
            if (paramval == "hh")
            {
                try
                {
                    //1/9/2013 NS modified the query
                    //string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':' +CONVERT(varchar, DATEPART ( n, date )) as Date, dd.[StatValue], ts.[HTTP_MaxConfiguredConnections]  FROM [VSS_Statistics].[dbo].[DominoDailyStats] dd JOIN [VitalSigns].[dbo].[Status] s ON dd.[ServerName] = s.[Name] JOIN [VitalSigns].[dbo].[Traveler_Status] ts ON s.[Name] = ts.[ServerName] WHERE s.[Name] = '" + servername + "' and dd.[Date] > DATEADD (" + paramval + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and s.[SecondaryRole] LIKE '%Quickr%' and dd.[StatName] = 'Http.CurrentConnections' order by Date asc";
                    string strQuerry = "SELECT Date, dd.[StatValue]  " +
                        "FROM [VSS_Statistics].[dbo].[DominoDailyStats] dd JOIN [VitalSigns].[dbo].[Status] s ON " +
                        "dd.[ServerName] = s.[Name] WHERE s.[Name] = '" + servername + "' and " +
                        "DATEDIFF(dd,0,Date) = DATEDIFF (dd,0,GETDATE()) and s.[SecondaryRole] LIKE '%Quickr%' and " +
                        "dd.[StatName] = 'Http.CurrentConnections' order by Date asc";
                    dt = objAdaptor.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    //1/9/2013 NS modified the query
                    //string strQuerry = "SELECT dd.[Date], dd.[StatValue], ts.[HTTP_MaxConfiguredConnections] FROM [VSS_Statistics].[dbo].[DominoDailyStats] dd JOIN [VitalSigns].[dbo].[Status] s ON dd.[ServerName] = s.[Name] JOIN [VitalSigns].[dbo].[Traveler_Status] ts ON s.[Name] = ts.[ServerName] WHERE s.[Name] = '" + servername + "' and dd.[Date] > DATEADD (" + paramval + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"].ToString() + "' ) and s.[SecondaryRole] = 'Traveler' and dd.[StatName] = 'Http.CurrentConnections' order by Date asc";
                    string strQuerry = "SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, Date)) Date, MAX(dd.[StatValue]) StatValue " +
                    "FROM [VSS_Statistics].[dbo].[DominoDailyStats] dd JOIN [VitalSigns].[dbo].[Status] s ON " +
                    "dd.[ServerName] = s.[Name] WHERE s.[Name] = '" + servername + "' and " +
                    "DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,GETDATE())) " +
                    "and s.[SecondaryRole] LIKE '%Quickr%' and dd.[StatName] = 'Http.CurrentConnections' " +
                    "group by DATEADD(dd, 0, DATEDIFF(dd, 0, Date)) order by Date asc";
                    dt = objAdaptor.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }

        public DataTable SetGraphForCPU(string paramval, string servername)
        {
            DataTable dt = new DataTable();
            if ( paramval == "hh")
            {
                try
                {
                   // string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Platform.System.PctCombinedCpuUtil' and Date > DATEADD (" + paramval + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + servername + "' order by Date asc";
                    //1/8/2013 NS modified the query below to remove hard coded date values
                    //string strQuerry = "select Servername,MAX(statvalue) as statvalue,DATEPART(hour, Date) AS Date " +
                  //"from [VSS_Statistics].[dbo].[DominoDailyStats] " +
                  //"where Date >='11/10/2012' and Date <'11/11/2012' and StatName='Platform.System.PctCombinedCpuUtil'  and Servername='" + servername + "'" +
                  //"group by Servername,DATEPART(hour, Date) ";
                    string strQuerry = "select Servername,statvalue,Date " +
                    "from [VSS_Statistics].[dbo].[DominoDailyStats] " +
                    "where DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) and StatName='Platform.System.PctCombinedCpuUtil'  and Servername='" + servername + "'" +
                    "order by Date ";
                    dt = objAdaptor.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    //string strQuerry = "SELECT date, StatValue FROM [VSS_Statistics].[dbo].[DominoDailyStats] where StatName='Platform.System.PctCombinedCpuUtil' and Date > DATEADD (" + paramval + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + servername + "' order by Date asc";
                    //1/8/2013 NS modified the query below to remove hard coded date values
                    //string strQuerry = "select Servername,MAX(statvalue) as statvalue,DATEPART(hour, Date) AS Date " +
                   //"from [VSS_Statistics].[dbo].[DominoDailyStats] " +
                   //"where Date >='11/10/2012' and Date <'11/11/2012' and StatName='Platform.System.PctCombinedCpuUtil'  and Servername='" + servername + "'" +
                   //"group by Servername,DATEPART(hour, Date) ";
                    string strQuerry = "select Servername,statvalue,Date " +
                   "from [VSS_Statistics].[dbo].[DominoDailyStats] " +
                   "where DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) and StatName='Platform.System.PctCombinedCpuUtil'  and Servername='" + servername + "'" +
                   "order by Date ";
                    dt = objAdaptor.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;   
        }

        public DataTable SetGraphForMemory(string paramval, string servername)
        {
            DataTable dt = new DataTable();
            if (paramval == "hh")
            {
                try
                {
                  //  string strQuerry = "SELECT CONVERT(varchar, DATEPART ( hh, date )) + ':'+CONVERT(varchar, DATEPART ( n, date )) as Date, [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mem.PercentAvailable' and Date > DATEADD (" + paramval + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + servername + "' order by Date asc";
                    //1/9/2013 NS modified the query below to remove hard coded date values
                    //string strQuerry = "select Servername,MAX(statvalue) as statvalue,DATEPART(hour, Date) AS Date " +
                    //         "from [VSS_Statistics].[dbo].[DominoDailyStats] " +
                    //         "where  Date >='11/10/2012' and Date <'11/11/2012' and StatName='Mem.PercentAvailable'  and Servername='" + servername + "'" +
                    //         "group by Servername,DATEPART(hour, Date) ";
                    string strQuerry = "select Servername,statvalue,Date " +
                             "from [VSS_Statistics].[dbo].[DominoDailyStats] " +
                             "where  DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE())  and StatName='Mem.PercentAvailable'  and Servername='" + servername + "'" +
                             "order by Date ";
                    dt = objAdaptor.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    //string strQuerry = "SELECT [Date], [StatValue] FROM [VSS_Statistics].[dbo].[DominoDailyStats] where [StatName]='Mem.PercentAvailable' and Date > DATEADD (" + paramval + " , -1 ,'" + ConfigurationSettings.AppSettings["Current_Date"] + "' ) and [ServerName]='" + servername + "' order by Date asc";
                    //1/9/2013 NS modified the query below to remove hard coded date values
                    //string strQuerry = "select Servername,MAX(statvalue) as statvalue,DATEPART(hour, Date) AS Date " +
                    //         "from [VSS_Statistics].[dbo].[DominoDailyStats] " +
                    //         "where  Date >='11/10/2012' and Date <'11/11/2012' and StatName='Mem.PercentAvailable'  and Servername='" + servername + "'" +
                    //         "group by Servername,DATEPART(hour, Date) ";
                    string strQuerry = "select Servername,statvalue,Date " +
                             "from [VSS_Statistics].[dbo].[DominoDailyStats] " +
                             "where  DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,GETDATE()) and StatName='Mem.PercentAvailable'  and Servername='" + servername + "'" +
                             "order by Date ";
                    dt = objAdaptor.FetchData(strQuerry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }

        public DataTable SetQuickrPlacesGrid(string serverName)
        {
            DataTable dt = new DataTable();
            try
            {
                if (serverName != "" && serverName != null)
                {
                    string strQuerry = "SELECT * FROM [VSS_Statistics].[dbo].[QuickrPlaces] WHERE ServerName = '" + serverName + "'";
                    dt = objAdaptor.FetchData(strQuerry);
                    return dt;
                }
                else 
                {
                    string strQuerry = "SELECT * FROM [VSS_Statistics].[dbo].[QuickrPlaces]";
                    dt = objAdaptor.FetchData(strQuerry);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

		public DataTable Enabledforscanning(string Servername)
		{
			
			DataTable dt = new DataTable();
			try
			{
				string Sql = "select WsScanMeetingServer,WsScanMediaServer from SametimeServers sa inner join Servers se  on sa.ServeriD=se.Id where  Platform='WebSphere' and  se.ServerName =@Servername";
				//dt = objAdaptor.FetchData(Sql);


				SqlCommand cmd = new SqlCommand(Sql);
				cmd.Parameters.AddWithValue("@Servername", Servername);
				dt = objAdaptor.FetchDatafromcommand(cmd);
				
			}
			catch (Exception)
			{

				throw;
			}
			return dt;
		}
    }    
}
