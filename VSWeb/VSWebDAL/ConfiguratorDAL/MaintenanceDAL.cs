using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;

namespace VSWebDAL.ConfiguratorDAL
{
    public class MaintenanceDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static MaintenanceDAL _self = new MaintenanceDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static MaintenanceDAL Ins
        {
            get { return _self; }
        }
        //added on june 16
        public DataTable GetAllMaintenData()
        {

            DataTable MaintDataTable = new DataTable();
            // Maintenance ReturnObj= new Maintenance();
            try
            {
                string SqlQuery = "SELECT ID, Name, StartDate, StartTime, Duration, EndDate, MaintType = CASE MaintType WHEN '1' THEN 'One time' WHEN '2' THEN 'Daily' WHEN '3' THEN 'Weekly' WHEN '4' THEN 'Monthly' END FROM Maintenance";
                // string SqlQuery = "SELECT t1.[ID],[ServerName], [ServerType], [Description], [Location], [IPAddress] FROM [Servers] t1 INNER JOIN [Locations] t2 ON [LocationID] = t2.[ID] INNER JOIN [ServerTypes] t3 ON [ServerTypeID] = t3.[ID]";
                //string SqlQuery = "SELECT * FROM Servers";
                MaintDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return MaintDataTable;
        }
        public DataTable GetMaintenDataOnServerID(string ServerId)
        {

            DataTable MaintDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT t3.ID, Name, StartDate, StartTime, Duration, EndDate, MaintType =" +
    "CASE MaintType WHEN '1' THEN 'One time' WHEN '2' THEN 'Daily' WHEN '3' THEN 'Weekly' " +
    "WHEN '4' THEN 'Monthly' END ,t1.[ID] ServerID,ServerName FROM (select  ID,ServerName,LocationID,ServerTypeID FROM Servers union SELECT ID, Name,LocationID,ServerTypeID FROM URLs) t1 INNER JOIN  " +
	" ServerMaintenance t2 ON t1.[ID]=t2.[ServerID] and t1.ServerTypeID = t2.ServerTypeID INNER JOIN Maintenance t3 on t2.[MaintID]=t3.[ID] INNER JOIN " +
    " ServerTypes t4 on t1.[ServerTypeID]=t4.[ID] INNER JOIN Locations t5 on t1.[LocationID]=t5.[ID] where t1.[ServerName]='" + ServerId + "'";

                
                MaintDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return MaintDataTable;
        }

        public DataTable GetMaintenCalanderDataOnServerID(string ServerId)
        {

            DataTable MaintDataTable = new DataTable();
            try
            {
                string SqlQuery = "SELECT t3.ID, Name, StartDate, StartTime, Duration, EndDate, MaintType,RecurrenceInfo,ReminderInfo,Label,AllDay" +
   
    ",t1.[ID] ServerID,ServerName FROM Servers t1 INNER JOIN  " +
    " ServerMaintenance t2 ON t1.[ID]=t2.[ServerID] INNER JOIN Maintenance t3 on t2.[MaintID]=t3.[ID] INNER JOIN " +
    " ServerTypes t4 on t1.[ServerTypeID]=t4.[ID] INNER JOIN Locations t5 on t1.[LocationID]=t5.[ID] where t1.[ServerName]='" + ServerId + "'";


                MaintDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return MaintDataTable;
        } 
        
        public DataTable GetMaintenDataOnServerIDs(string ServerId1, string ServerId2, string ServerId3)
        {

            DataTable MaintDataTable = new DataTable();
            try
            {
    //            string SqlQuery = "SELECT distinct t3.ID, Name, StartDate, StartTime, Duration, EndDate, MaintType =" +
    //"CASE MaintType WHEN '1' THEN 'One time' WHEN '2' THEN 'Daily' WHEN '3' THEN 'Weekly' " +
    //"WHEN '4' THEN 'Monthly' END , t1.[ID] ServerID,ServerName FROM Servers t1 INNER JOIN  " +
    //" ServerMaintenance t2 ON t1.[ID]=t2.[ServerID] INNER JOIN Maintenance t3 on t2.[MaintID]=t3.[ID] INNER JOIN " +
    //" ServerTypes t4 on t1.[ServerTypeID]=t4.[ID] INNER JOIN Locations t5 on t1.[LocationID]=t5.[ID] where t1.[ServerName] in ('" + ServerId1 + "','" + ServerId2 + "','" + ServerId3 + "')";

                //Mukund 11Apr14
                string SqlQuery = "SELECT distinct t3.ID, Name, StartDate, StartTime, Duration, EndDate, MaintType =" +
    "CASE MaintType WHEN '1' THEN 'One time' WHEN '2' THEN 'Daily' WHEN '3' THEN 'Weekly' " +
    "WHEN '4' THEN 'Monthly' END FROM Servers t1 INNER JOIN  " +
    " ServerMaintenance t2 ON t1.[ID]=t2.[ServerID] INNER JOIN Maintenance t3 on t2.[MaintID]=t3.[ID] INNER JOIN " +
    " ServerTypes t4 on t1.[ServerTypeID]=t4.[ID] INNER JOIN Locations t5 on t1.[LocationID]=t5.[ID] where t1.[ServerName] in ('" + ServerId1 + "','" + ServerId2 + "','" + ServerId3 + "')";

                MaintDataTable = objAdaptor.FetchData(SqlQuery);

            }
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
            }
            return MaintDataTable;
        }

        //12/7/2015 NS modified for VSPLUS-2227
        public bool UpdateMaintenanceWindows(string ID, string WinName, string StartDate, string StartTime, string Duration,
            string EndDate, string MaintType, string MaintDaysList, string EndDateIndicator, List<string> ServerIDs,string recurinfo,string reminderinfo,int label,bool allday,
            List<string> ServerTypeIDs, string copy, string Durationtype, List<string> DeviceIDs = null)
        {
            bool update = false;
            string SqlQuery = "";
            DataTable MaintenanceDataTable = new DataTable();
            DataTable NewMaintenanceDataTable = new DataTable();
            try
            {
                if (copy.ToLower() != "true")
                {
                    SqlQuery = "SELECT * FROM Maintenance WHERE [ID]=" + Convert.ToInt32(ID);
                    MaintenanceDataTable = objAdaptor.FetchData(SqlQuery);
                    if (MaintenanceDataTable.Rows.Count > 0)
                    {
                        string maintID = MaintenanceDataTable.Rows[0].ItemArray[0].ToString();
                        //Delete records from ServerMaintenance table first
                        SqlQuery = "DELETE FROM ServerMaintenance WHERE [MaintID]='" + maintID + "'";
                        objAdaptor.ExecuteNonQuery(SqlQuery);
                        //Delete records from Maintenance table next
                        SqlQuery = "DELETE FROM Maintenance WHERE [ID]=" + Convert.ToInt32(ID);
                        objAdaptor.ExecuteNonQuery(SqlQuery);
                    }
                }
                //Insert new record into Maintenance table first
                SqlQuery = "INSERT INTO Maintenance (Name,StartDate,StartTime,Duration,Type,EndDate,MaintType,MaintDaysList,EndDateIndicator,RecurrenceInfo,ReminderInfo,Label,AllDay) " +
                     "VALUES('" + WinName + "','" + StartDate + "','" + StartTime + "'," + Duration + "," + Durationtype + ",'" + EndDate + "','" + 
                     MaintType + "','" + MaintDaysList + "','" + EndDateIndicator + "','" + recurinfo + "','" + reminderinfo + "'," + label + ",'" + allday + "')";
                objAdaptor.ExecuteNonQuery(SqlQuery);
                //Select the new Maintenance ID value from the Maintenance table
                SqlQuery = "SELECT ID FROM Maintenance WHERE [Name]='" + WinName + "'";
                NewMaintenanceDataTable = objAdaptor.FetchData(SqlQuery);
                if (NewMaintenanceDataTable.Rows.Count > 0)
                {
                    string newMaintID = NewMaintenanceDataTable.Rows[0].ItemArray[0].ToString();
                    //Insert new record(s) into ServerMaintenance table using the new Maintenance ID
                    for (int i = 0; i < ServerIDs.Count; i++)
                    {
                        //9/17/2013 NS modified the query below - added ServerTypeID since now URLs are also 
                        //going to be put under maintenance
                        //12/7/2015 NS modified for VSPLUS-2227
                        if (DeviceIDs[i] == "NULL")
                        {
                            SqlQuery = "INSERT INTO ServerMaintenance (MaintID,ServerID,ServerTypeID) " +
                            "VALUES('" + newMaintID + "','" + ServerIDs[i] + "','" + ServerTypeIDs[i] + "')";
                        }
                        else
                        {
                            SqlQuery = "INSERT INTO ServerMaintenance (MaintID,ServerID,ServerTypeID,DeviceID) " +
                            "VALUES('" + newMaintID + "','" + ServerIDs[i] + "','" + ServerTypeIDs[i] + "','" + DeviceIDs[i] + "')";
                        }
                        objAdaptor.ExecuteNonQuery(SqlQuery);
                    }
                }

                update = true;
            }
            catch
            {
                update = false;
            }
            finally
            {
            }

            return update;
        }

        public DataTable GetMaintenanceWindow(string WinID, string WinName)
        {
            string SqlQuery = "";
            DataTable MaintenanceDataTable = new DataTable();
            try
            {
                if (WinID != "")
                {
                    SqlQuery = "SELECT ID,[Name],StartDate,StartTime,Duration,EndDate,MaintType,MaintDaysList,Type FROM Maintenance WHERE [ID]='" + WinID + "'";
                }
                else
                {
                    if (WinName != "")
                    {
                        SqlQuery = "SELECT ID,StartDate,StartTime,Duration,EndDate,MaintType,MaintDaysList,Type FROM Maintenance WHERE [Name]='" + WinName + "'";
                    }
                    else
                    {
                        SqlQuery = "SELECT [Name] FROM Maintenance";
                    }
                }
                MaintenanceDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return MaintenanceDataTable;
        }

        public string GetName(string ID)
        {
            string Name = "";
            try
            {
                DataTable Nametable = new DataTable();
                string SqlQuery = "Select [Name] from Maintenance where ID=" + ID;
                Nametable = objAdaptor.FetchData(SqlQuery);
                Name = Nametable.Rows[0]["Name"].ToString();
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return Name;
        }

        public DataTable GetServerMaintenance(string maintID)
        {
            string SqlQuery = "";
            DataTable SrvMaintenanceDataTable = new DataTable();
            try
            {
                // SqlQuery = "SELECT t1.[ID] ServerID,ServerName, ServerType,Description,Location FROM Servers t1 INNER JOIN " +
                //"ServerMaintenance t2 ON t1.[ID]=t2.[ServerID] INNER JOIN Maintenance t3 on t2.[MaintID]=t3.[ID] INNER JOIN " +
                //"ServerTypes t4 on t1.[ServerTypeID]=t4.[ID] INNER JOIN Locations t5 on t1.[LocationID]=t5.[ID] " +
                //"WHERE t2.[MaintID]='" + maintID + "' ";

                //9/17/2013 NS modified the query to account for URLs 
                //SqlQuery = "SELECT t1.[ID] ServerID,ServerName,ServerType,Description,Location FROM Servers t1 INNER JOIN " +
               //"ServerMaintenance t2 ON t1.[ID]=t2.[ServerID] INNER JOIN Maintenance t3 on t2.[MaintID]=t3.[ID] INNER JOIN " +
               //"ServerTypes  t4 on t1.[ServerTypeID]=t4.[ID] INNER JOIN Locations t5 on t1.[LocationID]=t5.[ID] " +
               //"WHERE t2.[MaintID]='" + maintID + "' ";

                SqlQuery = "SELECT t1.[ID] ServerID,ServerName,ServerType,Description,Location,t1.ServerTypeID ServerTypeID " +
                    "FROM Servers t1 INNER JOIN ServerMaintenance t2 ON t1.[ID]=t2.[ServerID] INNER JOIN Maintenance t3 " +
                    "ON t2.[MaintID]=t3.[ID] INNER JOIN ServerTypes t4 ON t1.[ServerTypeID]=t4.[ID] INNER JOIN Locations t5 " +
                    "ON t1.[LocationID]=t5.[ID] " +
                    "WHERE t2.[MaintID]='" + maintID + "' AND t1.ServerTypeID=t2.ServerTypeID " +
                    "UNION " +
                    "SELECT t1.[ID] ServerID,TheURL ServerName,'URL',t1.Name,'',t4.ID ServerTypeID " + 
                    "FROM URLs t1 INNER JOIN ServerMaintenance t2 " +
                    "ON t1.[ID]=t2.[ServerID] INNER JOIN Maintenance t3 ON t2.[MaintID]=t3.[ID] INNER JOIN ServerTypes t4 " +
                    "ON t4.[ServerType]='URL' " +
                    "WHERE t2.[MaintID]='" + maintID + "' AND t4.ID=t2.ServerTypeID " +
                    "ORDER BY t1.ServerTypeID ";

                SrvMaintenanceDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return SrvMaintenanceDataTable;
        }

        public DataTable GetServers(string MaintKey)
        {
            string SqlQuery = "";
            DataTable ServerDataTable = new DataTable();
            try
            {
                // SqlQuery = "SELECT t1.[ID],[ServerName], [ServerType], [Description], [Location], [IPAddress] FROM [Servers] t1 " +
                //"INNER JOIN [Locations] t2 ON [LocationID] = t2.[ID] INNER JOIN [ServerTypes] t3 ON [ServerTypeID] = t3.[ID]";

                //7/30/2013 NS added a query below to get the servers that are in maintenance listed at the top
                if (MaintKey == "")
                {
                    SqlQuery = "SELECT t1.[ID],[ServerName], [DisplayText] as ServerType , [Description], [Location], [IPAddress] " +
                        "FROM [Servers] t1 INNER JOIN [Locations] t2 ON [LocationID] = t2.[ID] INNER JOIN [MenuItems] t3 ON [ServerTypeID] = t3.[ID]";
                }
                else
                {
                    SqlQuery = "SELECT t1.[ID] ID,ServerName,ServerType,Description,Location, t2.[MaintID], t1.ServerTypeID, " +
                        "CAST(t1.[ID] as varchar(10)) + '_' + CAST(t1.ServerTypeID as varchar(10)) __Key, t1.[LocationID], t1.[ID] as ServerID " + 
                        "FROM Servers t1 INNER JOIN " +
                        "ServerMaintenance t2 ON t1.[ID]=t2.[ServerID] INNER JOIN Maintenance t3 on t2.[MaintID]=t3.[ID] INNER JOIN " +
                        "ServerTypes  t4 on t1.[ServerTypeID]=t4.[ID] INNER JOIN Locations t5 on t1.[LocationID]=t5.[ID] " +
                        "WHERE t2.[MaintID]='" + MaintKey + "' AND t1.ServerTypeID=t2.ServerTypeID " +
                        "UNION " +
                        "SELECT t1.[ID] ID,ServerName,ServerType,Description,Location,NULL,t1.ServerTypeID, " +
                        "CAST(t1.[ID] as varchar(10)) + '_' + CAST(t1.ServerTypeID as varchar(10)) __Key, t1.[LocationID], t1.[ID] as ServerID " + 
                        "FROM Servers t1 INNER JOIN " + 
                        "ServerTypes  t4 on t1.[ServerTypeID]=t4.[ID] INNER JOIN Locations t5 on t1.[LocationID]=t5.[ID] " + 
                        "WHERE t1.[ID] NOT IN " +
                            "(SELECT t1.[ID] ID FROM Servers t1 INNER JOIN " + 
                            "ServerMaintenance t2 ON t1.[ID]=t2.[ServerID] INNER JOIN Maintenance t3 on t2.[MaintID]=t3.[ID] INNER JOIN " + 
                            "ServerTypes  t4 on t1.[ServerTypeID]=t4.[ID] INNER JOIN Locations t5 on t1.[LocationID]=t5.[ID] " +
                            "WHERE t2.[MaintID]='" + MaintKey + "') " +
                        "UNION " +
                        "SELECT t1.[ID] ID,t1.Name ServerName,'URL',TheURL,'', t2.[MaintID], t2.ServerTypeID, " + 
                        "CAST(t1.[ID] as varchar(10)) + '_' + CAST(t2.ServerTypeID as varchar(10)) __Key, t1.[LocationID], t1.[ID] as ServerID " + 
                        "FROM URLs t1 INNER JOIN " + 
                        "ServerMaintenance t2 ON t1.[ID]=t2.[ServerID] INNER JOIN Maintenance t3 on t2.[MaintID]=t3.[ID] INNER JOIN " +  
                        "ServerTypes  t4 on t2.[ServerTypeID]=t4.[ID] " +
                        "WHERE t2.[MaintID]='" + MaintKey + "' AND t4.ID=t2.ServerTypeID AND t4.ServerType='URL' " + 
                        "UNION " +
                        "SELECT t1.[ID] ID,t1.Name ServerName,'URL',TheURL,'',NULL, t4.ID, " + 
                        "CAST(t1.[ID] as varchar(10)) + '_' + CAST(t4.ID as varchar(10)) __Key, t1.[LocationID], t1.[ID] as ServerID " + 
                        "FROM URLs t1 INNER JOIN " + 
                        "ServerTypes  t4 on t4.[ServerType]='URL' " + 
                        "WHERE t1.[ID]  NOT IN " + 
                            "(SELECT t1.[ID] ID FROM URLs t1 INNER JOIN " + 
                            "ServerMaintenance t2 ON t1.[ID]=t2.[ServerID] INNER JOIN Maintenance t3 on t2.[MaintID]=t3.[ID] INNER JOIN " + 
                            "ServerTypes  t4 on t2.[ServerTypeID]=t4.[ID] " +
                            "WHERE t2.[MaintID]='" + MaintKey + "') " + 
                        "ORDER BY t2.[MaintID] desc,ServerName ASC";
                }
                ServerDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return ServerDataTable;
        }

        public bool delete(Maintenance mobj)
        {
            string SqlQuery = "";
            bool delete = false;
            DataTable MaintenanceDataTable = new DataTable();
            try
            {
                SqlQuery = "SELECT * FROM Maintenance WHERE [ID]=" + mobj.ID;
                MaintenanceDataTable = objAdaptor.FetchData(SqlQuery);
                if (MaintenanceDataTable.Rows.Count > 0)
                {
                    string maintID = MaintenanceDataTable.Rows[0].ItemArray[0].ToString();
                    //Delete records from ServerMaintenance table first
                    SqlQuery = "DELETE FROM ServerMaintenance WHERE [MaintID]='" + maintID + "'";
                    objAdaptor.ExecuteNonQuery(SqlQuery);
                    //Delete records from Maintenance table next
                    SqlQuery = "DELETE FROM Maintenance WHERE [ID]=" + mobj.ID;
                    objAdaptor.ExecuteNonQuery(SqlQuery);
                }
            }
            catch
            {
                delete = false;

            }
            return delete;
        }

        public DataTable nametable(string Name,string ID)
        {
            DataTable dt = new DataTable();
            try
            {
                if (ID == "" || ID == null)
                {
                    string S = "Select * from Maintenance where Name='" + Name + "'";
                    dt = objAdaptor.FetchData(S);
                }
                else
                {
                    string S = "Select * from Maintenance where Name='" + Name + "' and ID<>'"+ID+"'";
                    dt = objAdaptor.FetchData(S);
                }

            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }


        public DataTable GettodaysServersfromMaintenance(DateTime current, int hrs, int week)
        {
            DataTable dt = new DataTable();
            try
            {
                if (current != null && hrs == 0 && week == 0)
                {
                    string q = " select" +
     " srv.servername, srvt.ServerType,convert(varchar(10),mnt.StartDate,101) as StartDate , Convert(varchar(8),mnt.StartTime,108) StartTime, mnt.Duration,convert(varchar(10),mnt.EndDate,101) as EndDate,CASE WHEN MaintType=1 THEN 'One time' WHEN MaintType=2 THEN 'Daily' WHEN MaintType=3 THEN 'Weekly' WHEN MaintType=4 THEN 'Monthly' END MaintType,dbo.DecodeMaintSchedule(MaintType,MaintDaysList) MaintDaysList" +
     " from " +
      " ServerMaintenance srvm, Servers srv,Maintenance mnt,ServerTypes srvt" +
      " where " +
      " srvm.ServerID=srv.ID and srvm.MaintID=mnt.ID and srv.ServerTypeID=srvt.ID ";
                    //and mnt.StartDate='" + current + "' ";

                    dt = objAdaptor.FetchData(q);
                }
                if (current != null && hrs == 24)
                {
                    string q = " select" +
    " srv.servername, srvt.ServerType,mnt.StartDate, mnt.StartTime, mnt.Duration" +
    " from " +
     " ServerMaintenance srvm, Servers srv,Maintenance mnt,ServerTypes srvt" +
     " where " +
     " srvm.ServerID=srv.ID and srvm.MaintID=mnt.ID and srv.ServerTypeID=srvt.ID and convert(varchar(20),mnt.StartDate,108)>'" + current + "' and convert(varchar(20),mnt.StartDate,108)<'" + current.AddHours(24) + "'";

                    dt = objAdaptor.FetchData(q);

                }
                if (current != null && week == 7)
                {
                    string q = " select" +
    " srv.servername, srvt.ServerType,mnt.StartDate, mnt.StartTime, mnt.Duration" +
    " from " +
     " ServerMaintenance srvm, Servers srv,Maintenance mnt,ServerTypes srvt" +
     " where " +
     " srvm.ServerID=srv.ID and srvm.MaintID=mnt.ID and srv.ServerTypeID=srvt.ID and mnt.StartDate> '" + current + "' and mnt.StartDate< '" + current.AddDays(7) + "'";

                    dt = objAdaptor.FetchData(q);
                }
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }


        //10/16/2013 Mukund rewrote the original query
        public DataTable GetmaintenanceServersbysearch(string fromdate, string todate, string fromtime, string totime)
        {
            DataTable dt = new DataTable();
            try
            {
                if (fromdate == null || todate == "")
                    fromdate = DateTime.Now.ToString("M/d/yyyy");
                if (todate == "" || todate == null)
                    todate = DateTime.Now.ToString("M/d/yyyy");

                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
                DateTime dtStart = DateTime.Parse(fromdate);
                DateTime dtEnd = DateTime.Parse(todate);

                fromdate = dtStart.ToString("d",ci) + " " + fromtime;
                todate = dtEnd.ToString("d",ci) + " " + totime;

                /*string q = " select m1.ID,ServerName servername, ServerType ,Name,CONVERT(varchar(10),m1.StartDate,101)  StartDate," +
                "CONVERT(varchar(5),m1.StartTime,108) StartTime,m1.Duration Duration,CONVERT(varchar(10),m1.EndDate,101) EndDate," +
                    "CASE WHEN MaintType=1 THEN 'One time' WHEN MaintType=2 THEN 'Daily' WHEN MaintType=3 THEN 'Weekly' " +
                    "WHEN MaintType=4 THEN 'Monthly' END MaintType,dbo.DecodeMaintSchedule(MaintType,MaintDaysList) MaintDaysList " +
                           " from Maintenance m1 " +
                           "     inner Join " +
                           " ServerMaintenance m2 ON m1.ID=m2.MaintID " +
                           "   INNER Join " +
                           " Servers s1 ON s1.ID=m2.ServerID " +
                           "  INNER Join " +
                           " ServerTypes s2 ON s2.ID=s1.ServerTypeID " +
                           " where  " +
                           " (StartDate + starttime between '" + fromdate + " " + fromtime + "'  and '" + todate + " " + totime + "' )" +
                           " or" +
                           " (enddate between '" + fromdate + " " + fromtime + "'  and '" + todate + " " + totime + "' )" +

                           " or" +
                           " (StartDate + starttime <= '" + fromdate + " " + fromtime + "'  and enddate>= '" + todate + " " + totime + "' ) " +
                           " ORDER BY StartDate,m1.StartTime";
                 * */
                string q = "select m1.ID,ServerName servername, ServerType ,Name, " +
                            "CONVERT(varchar(10),m1.StartDate,101)  StartDate,CONVERT(varchar(5),m1.StartTime,108) StartTime, " +
                            "m1.Duration Duration,CONVERT(varchar(10),m1.EndDate,101) EndDate," +
                            "CASE WHEN MaintType=1 THEN 'One time' " +
                            "WHEN MaintType=2 THEN 'Daily' WHEN MaintType=3 THEN 'Weekly' WHEN MaintType=4 THEN 'Monthly' END " +
                            "MaintType,dbo.DecodeMaintSchedule(MaintType,MaintDaysList) MaintDaysList " +
                            "from Maintenance m1 inner Join  ServerMaintenance m2 ON m1.ID=m2.MaintID INNER Join  Servers s1 ON s1.ID=m2.ServerID INNER Join  ServerTypes s2 ON s2.ID=s1.ServerTypeID " +
                            "where " +
                            "(" +
                            "(convert(datetime,StartDate,101) + CONVERT(datetime,m1.StartTime,101)) >= CONVERT(datetime,'" + fromdate  + "',101) and " +
                            "(convert(datetime,StartDate,101) + CONVERT(datetime,m1.StartTime,101)) <= CONVERT(datetime,'" + todate +  "',101)" +
                            ")"+
                            "or"+
                            "("+
                            "convert(datetime,EndDate,101) + dateadd(minute,Duration, convert(datetime,StartTime,101)) >= CONVERT(datetime,'" + fromdate + "',101) and " +
                            "convert(datetime,EndDate,101) + dateadd(minute,Duration, convert(datetime,StartTime,101)) <= CONVERT(datetime,'" + todate + "',101)" +
                            ")"+
                            "or"+
                            "("+
                            "(convert(datetime,StartDate,101) + CONVERT(datetime,StartTime,101)) <= convert(datetime,'" + fromdate  +  "',101) and " +
                            "convert(datetime,EndDate,101) + dateadd(minute,Duration, convert(datetime,StartTime,101)) >= convert(datetime,'" + todate + "',101)" +
                            ")"+
                            "ORDER BY StartDate,m1.StartTime";

                dt = objAdaptor.FetchData(q);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

        //8/9/2013 NS rewrote the original query
        public DataTable GetmaintenanceServersbysearch_NS(string fromdate, string todate, string fromtime, string totime)
        {
            DataTable dt = new DataTable();
            try
            {
                if (fromdate == null || todate == "")
                    fromdate = DateTime.Now.ToString("M/d/yyyy");
                if (todate == "" || todate == null)
                    todate = DateTime.Now.ToString("M/d/yyyy");
                string q = "SELECT srv.ServerName servername, srvt.ServerType ServerType,CONVERT(varchar(10),mnt.StartDate,101) StartDate, " +
                    "CONVERT(varchar(5),mnt.StartTime,108) StartTime,mnt.Duration Duration,CONVERT(varchar(10),mnt.EndDate,101) EndDate," + 
                    "CASE WHEN MaintType=1 THEN 'One time' WHEN MaintType=2 THEN 'Daily' WHEN MaintType=3 THEN 'Weekly' " +
                    "WHEN MaintType=4 THEN 'Monthly' END MaintType,dbo.DecodeMaintSchedule(MaintType,MaintDaysList) MaintDaysList " +
                    "FROM ServerMaintenance srvm, Servers srv, Maintenance mnt, ServerTypes srvt " +
                    "WHERE srvm.ServerID=srv.ID AND srvm.MaintID=mnt.ID AND srv.ServerTypeID=srvt.ID AND " +
                    "CAST(mnt.StartDate AS DATETIME) + CAST(mnt.StartTime AS DATETIME)>='" + fromdate + " " + fromtime + "' " +
                    "AND CAST(mnt.StartDate AS DATETIME) + CAST(mnt.StartTime AS DATETIME)<='" + todate + " " + totime + "' " +
                    "ORDER BY StartDate,StartTime";

                dt = objAdaptor.FetchData(q);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }


        //8/9/2013 NS rewrote the original query
        public DataTable GetmaintenanceserversbyServername(string servername, string fromdate, string todate, string fromtime, string totime)
        {
            DataTable dt = new DataTable();
            try
            {
                if (fromdate != "" && todate != "" && fromtime != "" && totime != "" && servername != "")
                {
                    string q = "SELECT srv.ServerName servername,srvt.ServerType ServerType,CONVERT(varchar(10),mnt.StartDate,101) StartDate," + 
                        "CONVERT(varchar(5),mnt.StartTime,108) StartTime,mnt.Duration Duration,CONVERT(varchar(10),mnt.EndDate,101) EndDate," +
                        "CASE WHEN MaintType=1 THEN 'One time' WHEN MaintType=2 THEN 'Daily' WHEN MaintType=3 THEN 'Weekly' " +
                        "WHEN MaintType=4 THEN 'Monthly' END MaintType,dbo.DecodeMaintSchedule(MaintType,MaintDaysList) MaintDaysList " +
                        "FROM ServerMaintenance srvm,Servers srv,Maintenance mnt,ServerTypes srvt " +
                        "WHERE srvm.ServerID=srv.ID AND srvm.MaintID=mnt.ID AND srv.ServerTypeID=srvt.ID AND " +
						"(('" + fromdate + " " + fromtime +"'  between CAST(StartDate AS DATETIME) + CAST(StartTime AS DATETIME) and CAST(EndDate  AS DATETIME) ) " +
                    "OR ('" + todate + " " + totime + "'  BETWEEN CAST(StartDate AS DATETIME) + CAST(StartTime AS DATETIME) and CAST(EndDate  AS DATETIME) )) " +
						//"OR (CAST(StartTime AS DATETIME) between '"+ fromdate + " " + fromtime + "' and '" + todate + " " + totime + "' ) "+
						//"OR (CAST(EndDate  AS DATETIME) between '"+ fromdate + " " + fromtime +"' and '" + todate + " " + totime + "') " +

                        //"(CAST(mnt.StartDate AS DATETIME) + CAST(mnt.StartTime AS DATETIME)<='" + fromdate + " " + fromtime + "' OR " +
                        //"CAST(mnt.EndDate AS DATETIME) >='" + todate + " " + totime + "') AND " + 
                        " AND srv.servername='" + servername + "' " +
                        "ORDER BY StartDate,StartTime";
                    dt = objAdaptor.FetchData(q);
                }
                //1/4/2013 NS - because time is a secondary parameter and if dates are empty should not be used in a query,
                //it may be removed as a condition
                //else if (fromdate == "" && todate == "" && fromtime == "" && totime == "" && servername != "")
                else if (fromdate == "" && todate == "" && servername != "")
                {
                    string q = "SELECT srv.ServerName servername,srvt.ServerType ServerType,CONVERT(varchar(10),mnt.StartDate,101) StartDate," + 
                        "CONVERT(varchar(5),mnt.StartTime,108) StartTime,mnt.Duration,CONVERT(varchar(10),mnt.EndDate,101) EndDate," + 
                        "CASE WHEN MaintType=1 THEN 'One time' WHEN MaintType=2 THEN 'Daily' WHEN MaintType=3 THEN 'Weekly' " + 
                        "WHEN MaintType=4 THEN 'Monthly' END MaintType,dbo.DecodeMaintSchedule(MaintType,MaintDaysList) MaintDaysList " +
                        "FROM ServerMaintenance srvm,Servers srv,Maintenance mnt,ServerTypes srvt " +
                        "WHERE srvm.ServerID=srv.ID AND srvm.MaintID=mnt.ID AND srv.ServerTypeID=srvt.ID AND " + 
                        "srv.servername='" + servername + "' " +                      
                        "ORDER BY StartDate,StartTime";
                    dt = objAdaptor.FetchData(q);
                }
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

        public DataTable GetmaintenanceServersbysearchOLD(string fromdate, string todate, string fromtime,string totime )
        {
            DataTable dt = new DataTable();
            try
            {
                string fromtime1 = fromtime.Replace("AM","");
                fromtime1 = fromtime1.Replace("PM", "");
                string totime1 = totime.Replace("AM", "");
                totime1 = totime1.Replace("PM", "");
                if (fromdate == null || todate=="")
                    fromdate = DateTime.Now.ToString("M/d/yyyy");
                if(todate=="" || todate==null)
                    todate = DateTime.Now.ToString("M/d/yyyy");
                    string q = " select" +
     " srv.servername, srvt.ServerType,convert(varchar(10),mnt.StartDate,101) as StartDate , Convert(varchar(5),mnt.StartTime,108) StartTime, mnt.Duration,convert(varchar(10),mnt.EndDate,101) as EndDate,CASE WHEN MaintType=1 THEN 'One time' WHEN MaintType=2 THEN 'Daily' WHEN MaintType=3 THEN 'Weekly' WHEN MaintType=4 THEN 'Monthly' END MaintType,dbo.DecodeMaintSchedule(MaintType,MaintDaysList) MaintDaysList" +
     " from " +
      " ServerMaintenance srvm, Servers srv,Maintenance mnt,ServerTypes srvt" +
      " where " +
      " srvm.ServerID=srv.ID and srvm.MaintID=mnt.ID and srv.ServerTypeID=srvt.ID and mnt.StartDate>='" + fromdate + "' and "+
                " mnt.StartDate<='" + todate + "' and  mnt.StartTime>='" + fromtime1 + "' and  mnt.StartTime<='" + totime1 + "' order by StartDate,StartTime";
                    //and mnt.StartDate='" + current + "' ";

                    dt = objAdaptor.FetchData(q);


                
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;


            

        }
        public DataTable GetmaintenanceserversbyServernameOLD(string servername, string fromdate, string todate, string fromtime, string totime)
        {
            DataTable dt = new DataTable();
            try
            {
                if (fromdate != "" && todate != "" && fromtime != "" && totime != "" && servername!="")
                {
                    string q = " select" +
     " srv.servername, srvt.ServerType,convert(varchar(10),mnt.StartDate,101) as StartDate , Convert(varchar(5),mnt.StartTime,108) StartTime, mnt.Duration,convert(varchar(10),mnt.EndDate,101) as EndDate,CASE WHEN MaintType=1 THEN 'One time' WHEN MaintType=2 THEN 'Daily' WHEN MaintType=3 THEN 'Weekly' WHEN MaintType=4 THEN 'Monthly' END MaintType,dbo.DecodeMaintSchedule(MaintType,MaintDaysList) MaintDaysList" +
     " from " +
      " ServerMaintenance srvm, Servers srv,Maintenance mnt,ServerTypes srvt" +
      " where " +
      " srvm.ServerID=srv.ID and srvm.MaintID=mnt.ID and srv.ServerTypeID=srvt.ID and mnt.StartDate>='" + fromdate + "' and " +
                " mnt.StartDate<='" + todate + "' and  mnt.StartTime>='" + fromtime + "' and  mnt.StartTime<='" + totime + "' and srv.servername='"+servername+"' order by StartDate,StartTime";
                    //and mnt.StartDate='" + current + "' ";

                    dt = objAdaptor.FetchData(q);

                }
                //1/4/2013 NS - because time is a secondary parameter and if dates are empty should not be used in a query,
                //it may be removed as a condition
                //else if (fromdate == "" && todate == "" && fromtime == "" && totime == "" && servername != "")
                else if (fromdate == "" && todate == "" && servername != "")
                {
                    string q = " select" +
    " srv.servername, srvt.ServerType,convert(varchar(10),mnt.StartDate,101) as StartDate , Convert(varchar(5),mnt.StartTime,108) StartTime, mnt.Duration,convert(varchar(10),mnt.EndDate,101) as EndDate,CASE WHEN MaintType=1 THEN 'One time' WHEN MaintType=2 THEN 'Daily' WHEN MaintType=3 THEN 'Weekly' WHEN MaintType=4 THEN 'Monthly' END MaintType,dbo.DecodeMaintSchedule(MaintType,MaintDaysList) MaintDaysList" +
    " from " +
     " ServerMaintenance srvm, Servers srv,Maintenance mnt,ServerTypes srvt" +
     " where " +
     " srvm.ServerID=srv.ID and srvm.MaintID=mnt.ID and srv.ServerTypeID=srvt.ID and srv.servername='" + servername + "'" +
               //     mnt.StartDate>='" + fromdate + "' and " +
               //" mnt.StartDate<='" + todate + "' and  mnt.StartTime>='" + fromtime + "' and  mnt.StartTime<='" + totime + "' 
                    
                   " order by StartDate,StartTime";
                    //and mnt.StartDate='" + current + "' ";

                    dt = objAdaptor.FetchData(q);
                }
                
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

        public DataTable GetSelectedServers(int MaintID)
        {

            DataTable EventsTab = new DataTable();
            try
            {
                string SqlQuery = "select * from ServerMaintenance where MaintID=" + MaintID;
                EventsTab = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return EventsTab;
        }
        //11/30/2015 NS added for VSPLUS-2227
        public DataTable GetKeyMobileUsers(string MaintKey)
        {
            string SqlQuery = "";
            DataTable ServerDataTable = new DataTable();
            try
            {
                SqlQuery = "SELECT t1.[DeviceID] ID,t1.UserName ServerName,'Mobile Users',DeviceName,'', t2.[MaintID], t2.ServerTypeID, " +
                        "CAST(t1.[ID] as varchar(10)) + '_' + CAST(t2.ServerTypeID as varchar(10)) __Key, 0, t1.[ID] as ServerID " + 
                        "FROM Traveler_Devices t1 INNER JOIN " + 
                        "ServerMaintenance t2 ON t1.UserName + '-' + t1.[DeviceID]=t2.[DeviceID] INNER JOIN Maintenance t3 on t2.[MaintID]=t3.[ID] INNER JOIN " + 
                        "ServerTypes  t4 on t2.[ServerTypeID]=t4.[ID] " + 
                        "WHERE t2.[MaintID]='" + MaintKey + "' AND t4.ID=t2.ServerTypeID AND t2.ServerTypeID=11 " + 
                        "UNION " + 
                        "SELECT t1.[DeviceID] ID,t1.UserName ServerName,'Mobile Users',DeviceName,'',NULL, t4.ID,  " +
                        "CAST(t1.[ID] as varchar(10)) + '_' + CAST(t4.ID as varchar(10)) __Key, 0, t1.[ID] as ServerID " + 
                        "FROM Traveler_Devices t1 INNER JOIN " + 
                        "ServerTypes  t4 on t4.ID=11 " + 
                        "WHERE t1.UserName + '-' + t1.[DeviceID]  NOT IN " + 
                        "    (SELECT t1.UserName + '-' + t1.[DeviceID] FROM Traveler_Devices t1 INNER JOIN " + 
                        "    ServerMaintenance t2 ON t1.UserName + '-' + t1.[DeviceID]=t2.[DeviceID] " + 
                        "    INNER JOIN Maintenance t3 on t2.[MaintID]=t3.[ID] " + 
                        "    INNER JOIN ServerTypes  t4 on t2.[ServerTypeID]=t4.[ID]  " +
                        "    WHERE t2.[MaintID]='" + MaintKey + "') " + 
                        "ORDER BY t2.[MaintID] desc,ServerName ASC";
                ServerDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ServerDataTable;
        }
    }
}
