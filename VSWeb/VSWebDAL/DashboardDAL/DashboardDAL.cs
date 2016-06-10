using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using System.Data.SqlClient;
using System.Configuration;
using VSWebDAL;

namespace VSWebDAL.DashboardDAL
{
    public class DashboardDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        private static DashboardDAL _self = new DashboardDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static DashboardDAL Ins
        {
            get { return _self; }
        }

        public DataTable GetAllData(string ServerLoc,string viewby)
        {
            //VSPLUS-652, 20Jun14 Mukund Added : and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1 union Select Name from MailServices where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1)
            DataTable StatusDataTable = new DataTable();
            try
            {
                if (ServerLoc == "null" && viewby!="Category")
                {
					string SqlQuery = "select StatusCode,count(*) as Total from ( select StatusCode from Status where StatusCode in ('Issue','Maintenance','Not Responding','OK') and Location <>'' and Type<>'' and Status <> 'Disabled' and  StatusCode is not null and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1 union Select Name from MailServices where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1 union Select Name from ExchangeMailProbe where Enabled=1 union Select Name from NotesDatabases where Enabled=1 union select Name from O365Server where Enabled=1 union select Name from CloudDetails where Enabled=1)) as tbl group by StatusCode order by StatusCode";
					//string SqlQuery = "select StatusCode,count(*) as Total from Status where  StatusCode in ('Issue','Maintenance','Not Responding','OK') and Location <>'' and Type<>'' and Status <> 'Disabled' and  StatusCode is not null and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1 union Select Name from MailServices where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1 union Select Name from ExchangeMailProbe where Enabled=1) group by StatusCode order by StatusCode";
                    //string SqlQuery = "SELECT StatusCode ,count(*) AS Total FROM ( ( SELECT Status.* FROM vitalsigns.dbo.Status INNER JOIN vitalsigns.dbo.Servers ON Status.Name = Servers.ServerName AND Status.Type = (Select ServerType FROM vitalsigns.dbo.ServerTypes WHERE Servers.ServerTypeID = ServerTypes.ID) ) UNION ALL(SELECT Status.* FROM vitalsigns.dbo.Status INNER JOIN vitalsigns.dbo.URLs ON Status.Name = URLs.Name AND Status.Type = 'URL' )) AS tbl WHERE  StatusCode IN ('Issue','Maintenance','Not Responding','OK') AND Location <>'' AND Type<>'' AND Status <> 'Disabled' AND  StatusCode IS NOT NULL group by StatusCode order by StatusCode";
                    StatusDataTable = objAdaptor.FetchData(SqlQuery);
                }
                else if (ServerLoc == "null" && viewby == "Category")
                {
					string SqlQuery = "select StatusCode,count(*) as Total from ( select StatusCode from Status where  StatusCode in ('Issue','Maintenance','Not Responding','OK') and Location <>'' and Type<>'' and Category<>'' and Status <> 'Disabled' and StatusCode is not null and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1 union Select Name from MailServices where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1 union Select Name from ExchangeMailProbe where Enabled=1 union Select Name from NotesDatabases where Enabled=1 union select Name from O365Server where Enabled=1 union select Name from CloudDetails where Enabled=1)) as tbl group by StatusCode order by StatusCode";
					//string SqlQuery = "select StatusCode,count(*) as Total from Status where  StatusCode in ('Issue','Maintenance','Not Responding','OK') and Location <>'' and Type<>'' and Category<>'' and Status <> 'Disabled' and StatusCode is not null and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1 union Select Name from MailServices where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1 union Select Name from ExchangeMailProbe where Enabled=1) group by StatusCode order by StatusCode";
                    //string SqlQuery = "select StatusCode,count(*) as Total from  ((SELECT Status.* FROM vitalsigns.dbo.Status INNER JOIN vitalsigns.dbo.Servers ON Status.Name = Servers.ServerName AND Status.Type = (Select ServerType FROM vitalsigns.dbo.ServerTypes WHERE Servers.ServerTypeID = ServerTypes.ID))UNION ALL (SELECT Status.* FROM vitalsigns.dbo.Status INNER JOIN vitalsigns.dbo.URLs ON Status.Name = URLs.Name AND Status.Type = 'URL')) AS tbl where  StatusCode in ('Issue','Maintenance','Not Responding','OK') and Location <>'' and Type<>'' and Category<>'' and Status <> 'Disabled' and StatusCode is not null group by StatusCode order by StatusCode";
                    StatusDataTable = objAdaptor.FetchData(SqlQuery);
                }
                else if (ServerLoc != "null" && viewby == "Category")
                {
					string SqlQuery = "select StatusCode,count(*) as Total from (select StatusCode from Status where (Location in (" + ServerLoc + ") or SecondaryRole in (" + ServerLoc + "))and Location <>'' and Category<>'' and Status <> 'Disabled' and StatusCode in ('Issue','Maintenance','Not Responding','OK') and StatusCode is not null and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1 union Select Name from MailServices where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1 union Select Name from ExchangeMailProbe where Enabled=1 union Select Name from NotesDatabases where Enabled=1 union select Name from O365Server where Enabled=1 union select Name from CloudDetails where Enabled=1) " + 
						")as tbl group by StatusCode order by StatusCode";
                    //string SqlQuery = "select StatusCode,count(*) as Total from ((SELECT Status.* FROM vitalsigns.dbo.Status INNER JOIN vitalsigns.dbo.Servers ON Status.Name = Servers.ServerName AND Status.Type = (Select ServerType FROM vitalsigns.dbo.ServerTypes WHERE Servers.ServerTypeID = ServerTypes.ID))UNION ALL (SELECT Status.* FROM vitalsigns.dbo.Status INNER JOIN vitalsigns.dbo.URLs ON Status.Name = URLs.Name AND Status.Type = 'URL')) AS tbl where (Location in (" + ServerLoc + ") or SecondaryRole in (" + ServerLoc + "))and Location <>'' and Category<>'' and Status <> 'Disabled' and StatusCode in ('Issue','Maintenance','Not Responding','OK') and StatusCode is not null  group by StatusCode order by StatusCode";
                    StatusDataTable = objAdaptor.FetchData(SqlQuery);
                }
                else
                {
					string SqlQuery = "select StatusCode,count(*) as Total from (select StatusCode from Status where (Type in (" + ServerLoc + ") or Location in (" + ServerLoc + ") or SecondaryRole in (" + ServerLoc + "))and Location <>'' and Status <> 'Disabled' and StatusCode in ('Issue','Maintenance','Not Responding','OK') and StatusCode is not null and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1 union Select Name from MailServices where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1 union Select Name from ExchangeMailProbe where Enabled=1 union Select Name from NotesDatabases where Enabled=1 union select Name from O365Server where Enabled=1 union select Name from CloudDetails where Enabled=1) " + 
						")as tbl group by StatusCode order by StatusCode";
                    //string SqlQuery = "select StatusCode,count(*) as Total from ((SELECT Status.* FROM vitalsigns.dbo.Status INNER JOIN vitalsigns.dbo.Servers ON Status.Name = Servers.ServerName AND Status.Type = (Select ServerType FROM vitalsigns.dbo.ServerTypes WHERE Servers.ServerTypeID = ServerTypes.ID))UNION ALL(SELECT Status.* FROM vitalsigns.dbo.Status INNER JOIN vitalsigns.dbo.URLs ON Status.Name = URLs.Name AND Status.Type = 'URL')) AS tbl where (Type in (" + ServerLoc + ") or Location in (" + ServerLoc + ") or SecondaryRole in (" + ServerLoc + ")) and Location <>'' and Status <> 'Disabled' and StatusCode in ('Issue','Maintenance','Not Responding','OK') and StatusCode is not null group by StatusCode order by StatusCode";
                    StatusDataTable = objAdaptor.FetchData(SqlQuery);
                }
            }
            catch
            {
            }
            finally
            {
            }
            return StatusDataTable;
        }
        public DataTable GetIndlData()
        {

            DataTable StatusDataTable = new DataTable();
            try
            {

                //VSPLUS-652, 20Jun14 Mukund Added : and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1 union Select ServerName from MailServices where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1)
                string SqlQuery = "select type, Status, COUNT(*) from Status where status in ('Issue','Maintenance','Not Responding','OK') and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1 union Select ServerName from MailServices where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1) group by type,StatusCode order by type,StatusCode";
                StatusDataTable = objAdaptor.FetchData(SqlQuery);

            }
            catch
            {
            }
            finally
            {
            }
            return StatusDataTable;
        }
	
        // shilpa
        public DataTable GetStatusbyType(string Location)
        {
            DataTable statustab = new DataTable();
            try
            {
                statustab = objAdaptor.FetchStatus("StatusByType", Location);
            }
            catch (Exception)
            {

                throw;
            }

            return statustab;

        }

        public DataTable GetStatusbyLocation(string Type)
        {
            DataTable statustab = new DataTable();
            try
            {

                statustab = objAdaptor.FetchStatus("StatusByLocation", Type);
            }
            catch (Exception)
            {

                throw;
            }

            return statustab;

        }

        public DataTable GetStatusbyCategory(string Location)
        {
            DataTable statustab = new DataTable();
            try
            {
                statustab = objAdaptor.FetchStatus("StatusByCategory", Location);
            }
            catch (Exception)
            {

                throw;
            }

            return statustab;

        }





        //code modified on 07May14 Mukund
        //10/15/2015 NS modified VSPLUS-2247
        public DataTable GetStatusData(string status, string typeloc, string filterbyval, string SessionViewBy, string MapLoc,
            string orderBy = "Name")
        {
			
            Boolean secondaryRole = false;
			

            //9/1/2015 NS modified for VSPLUS-2096
            //10/5/2015 NS modified for VSPLUS-2247
            //4/19/2016 NS modified for VSPLUS-2750
			string SqlQry = "Select Type,"
                        +"isnull((select Location from Locations where ID=LocationID),Location) as Location,Category,case when Type='Office365' then Name else Name end as Name,s.ID,Status," +
                "case when statuscode='Not Responding' then 1 when statuscode='OK' then 3 when statuscode='Maintenance' then 4 else 2 end as ordernum, " +
                "Details,LastUpdate,st.Description,PendingMail,DeadMail,MailDetails,Upcount,DownCount,UpPercent,ResponseTime,ResponseThreshold,PendingThreshold,DeadThreshold,UserCount,MyPercent,NextScan,DominoServerTasks,TypeandName,Icon,OperatingSystem,DominoVersion,UpMinutes,DownMinutes,UpPercentMinutes,PercentageChange,(CPU*100) as CPU,HeldMail,HeldMailThreshold,Severity,Memory,StatusCode,ISNULL(SecondaryRole,'') SecondaryRole," +
				"case when st.type in ('Exchange', 'SharePoint', 'Active Directory', 'WebSphere','Office365', 'Windows') then isnull(( select [CPU_Threshold]*0.01 from [VitalSigns].[dbo].[ServerAttributes]  where serverid=(select srvr.id from [VitalSigns].[dbo].servers srvr,ServerTypes stp where srvr.ServerTypeID=stp.ID and stp.ServerType=st.type and srvr.ServerName=st.name)),0) when st.type='Domino' then isnull(( select [CPU_Threshold] from [VitalSigns].[dbo].[DominoServers] where serverid=(select srvr.id from [VitalSigns].[dbo].servers srvr, ServerTypes stp where srvr.ServerTypeID=stp.ID and stp.ServerType=st.type and srvr.ServerName=st.Name)),0) else CPUThreshold end as CPUThreshold," +
			"case when Type='BES' then '../images/icons/BBDevice.gif' " +
				   "when Type='Domino' then '../images/icons/dominoserver.gif'" +
					"when Type='Exchange' then '../images/icons/Exchange.jpg'" +
					"when Type='Notes Database' then '../images/icons/notesdb.gif'" +
					 "when Type='Sametime' then '../images/icons/sametime.gif'" +
					  "when Type='URL' then '../images/icons/url.gif'" +
					   "when Type='Network Device' then '../images/icons/network.gif'" +
					   "when Type='Active Directory' then '../images/icons/adicon2.png'" +
                       "when Type='WebSphere' then '../images/icons/ibm.png'" +
					   "when Type='Office365' then '../images/icons/O365.png'"+
				   " else '../images/information.png' end as imgsource," +
				   "convert(varchar,case when PendingThreshold>0 then (100*PendingMail)/PendingThreshold else 0 end) as PendingPercent," +
"convert(varchar,case when DeadThreshold>0 then (100*DeadMail)/DeadThreshold else 0 end) as DeadPercent," +
"convert(varchar,case when HeldMailThreshold>0 then (100*HeldMail)/HeldMailThreshold else 0 end) as HeldPercent," +
"'DominoServerDetailsPage2.aspx?Name=' + Name + '&Type=' + Type + '&LastDate='+CONVERT(VARCHAR,LastUpdate,101) + ' ' + substring(convert(varchar(20), LastUpdate, 9), 13, 5) + ' ' + substring(convert(varchar(30), LastUpdate, 9), 25, 2) as redirecto," +
"100*isnull(Memory,0) as MemoryPercent, case when st.type='Domino' then isnull((select memory_threshold from [VitalSigns].[dbo].dominoservers ds, [VitalSigns].[dbo].servers sr where ds.ServerID=sr.ID and ServerName=st.name),0) else case when st.type in  " +
" ('Exchange', 'SharePoint', 'Active Directory', 'WebSphere', 'Windows') then isnull( (select [MemThreshold]*0.01 from [VitalSigns].[dbo].[ServerAttributes] ds, [VitalSigns].[dbo].servers sr where ds.ServerID=sr.ID and ServerName=st.name),0) else 0 end end as MemoryThreshold, " +
"(select min(status) diskstatus from (select  ServerName ,(Case when (ddsdiskname='NoAlerts') then '-1' when (diskfree>YellowThresholdvalue) then '2'  when((RedThresholdValue<diskfree)and(diskfree<YellowThresholdvalue))then '1'when diskfree<=RedThresholdValue then '0' end ) as Status from diskyellowvalue  )a where servername=st.name)Diskstatus" +
" from [VitalSigns].[dbo].[Status] st, (select  srvs.ID,srvs.ServerName,srvs.LocationID,srvs.ServerTypeID FROM Servers srvs,servertypes stp where srvs.servertypeid=stp.id and stp.servertype<>'Office365' union SELECT ID, Name,LocationID,ServerTypeID FROM URLs where Enabled=1 union  " +
" select  0,Name,0,13 FROM NotesMailProbe where Enabled=1 union select [key],Name,LocationID,ServerTypeID FROM MailServices where Enabled=1 union  select ID,Name,LocationID,ServerTypeID FROM [Network Devices] where Enabled=1 union select  0,Name,0,14  " +
" FROM ExchangeMailProbe where Enabled=1 union select  0,Name,0,17   FROM CloudDetails where Enabled=1 union select ndb.ID,ndb.Name,srv.LocationID,9 FROM NotesDatabases ndb, servers srv where ndb.ServerID=srv.ID  " +
 " union SELECT nd.ID, nd.Name, nd.LocationID, osrv.ServerTypeId FROM dbo.Nodes AS nd INNER JOIN dbo.O365Nodes AS ond ON nd.ID = ond.NodeID INNER JOIN dbo.O365Server AS osrv ON ond.O365ServerID = osrv.ID where (nd.LocationID <>'' and LocationID is not null) and  Enabled=1  " +
") s where"+ 
"((st.Name = s.ServerName and s.ServerTypeID in(select id from ServerTypes stp where stp.ServerType=st.Type and servertype<>'Office365') ) or "+

"(st.Category = s.ServerName and s.ServerTypeID in(select id from ServerTypes stp where stp.ServerType=st.Type  and  servertype='Office365')))";
			//"(st.Category = s.ServerName and s.ServerTypeID in(select id from ServerTypes stp where stp.ServerType=st.Type  and (st.Name in(select Name from O365Server)) servertype='Office365')))";//somaraju if not delete O365server in status table
//" from [VitalSigns].[dbo].[Status] st, (select  ID,ServerName,LocationID,ServerTypeID FROM Servers union SELECT ID, Name,LocationID,ServerTypeID FROM URLs where Enabled=1  union select  0,Name,0,13 FROM NotesMailProbe where Enabled=1 union select [key],Name,LocationID,ServerTypeID FROM MailServices where Enabled=1 union select ID,Name,LocationID,ServerTypeID FROM [Network Devices] where Enabled=1) s  where st.Name = s.ServerName";
            
			if (typeloc != "" && typeloc != null)
            {
                int i = typeloc.IndexOf('(');
                if (i > 0)
                {
                    typeloc = typeloc.Substring(0, i - 1);
                    secondaryRole = true;
                }
            }
            DataTable StatusDataTable = new DataTable();
			string SqlQuery;
			string DagWheres;
            try
            {
				if (MapLoc != "")
				{
					SqlQuery = SqlQry + " and locationID in (" + MapLoc + ")";
				}
                else if (status != "" && status != null)
                {
                    if (filterbyval != "null" && typeloc != "" && typeloc != null && status != "" && status != null)
                    {

                        //string SqlQuery = "Select Name, Status,TypeandName,(CPU*100) as CPU,CPUThreshold,Details,Description,Location,SecondaryRole,Type,PendingMail,PendingThreshold,DeadMail,DeadThreshold,HeldMail,HeldMailThreshold,"+
                        //"OperatingSystem,DominoVersion,LastUpdate,NextScan,ResponseTime,UserCount from [VitalSigns].[dbo].[Status] " +
                        SqlQuery = SqlQry + " and StatusCode='" + status + "' and (" +
                            (SessionViewBy == "ServerType" ? (secondaryRole ? " SecondaryRole LIKE '%" + typeloc + "%'" : " Type='" + typeloc + "'" ): "") +
                             (SessionViewBy == "Location" ? " Location='" + typeloc + "'" : "") +
                            (SessionViewBy == "Category" ? " Category='" + typeloc + "'" : "") +
                            ") and (Type in (" + filterbyval + ") or Location in (" + filterbyval + ")  or SecondaryRole in (" + filterbyval + ")) and StatusCode is not Null";

						DagWheres = "Status = '" + status + "' and (" +
							 (SessionViewBy == "Location" ? " loc.Location='" + typeloc + "'" : "") +
							") and (loc.Location in (" + filterbyval + ")) and Status is not Null";
                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);
                    }
                    else if (typeloc != "" && typeloc != null && status != "" && status != null)
                    {

                        //string SqlQuery = "Select Name,Status,(CPU*100) as CPU,CPUThreshold,SecondaryRole,TypeandName,Details,Description,Type,Location,PendingMail,PendingThreshold,DeadMail,DeadThreshold,HeldMail,HeldMailThreshold,"+
                        //     "OperatingSystem,DominoVersion,LastUpdate,NextScan,ResponseTime,UserCount from [VitalSigns].[dbo].[Status] " +
                        SqlQuery = SqlQry + "  and StatusCode='" + status + "' and (" +
                           (SessionViewBy == "ServerType" ? (secondaryRole ? " SecondaryRole LIKE '%" + typeloc + "%'" : " Type='" + typeloc + "'" ): "") +
                             (SessionViewBy == "Location" ? " Location='" + typeloc + "'" : "") +
                            (SessionViewBy == "Category" ? " Category='" + typeloc + "'" : "") +
                            ") and StatusCode is not Null";

						DagWheres = "Status = '" + status + "' and (" +
							(SessionViewBy == "Location" ? " loc.Location='" + typeloc + "'" : "") +
							") and Status is not Null";
                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);
                    }
                    else if (filterbyval != "null" && status != "" && status != null)
                    {
                        //string SqlQuery = "Select Name, Status,(CPU*100) as CPU,CPUThreshold,TypeandName,SecondaryRole,Details,Description,Type,Location,PendingMail,PendingThreshold,DeadMail,DeadThreshold,HeldMail,HeldMailThreshold,"+
                        //     "OperatingSystem,DominoVersion,LastUpdate,NextScan,ResponseTime,UserCount from [VitalSigns].[dbo].[Status] " +
                        SqlQuery = SqlQry + " and StatusCode='" + status + "' and (type in (" + filterbyval + ") or Location in (" + filterbyval + ")) and StatusCode is not Null";

						DagWheres = "Status='" + status + "' and (loc.Location in (" + filterbyval + ")) and Status is not Null";
                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);


                    }
                    else
                    {
                        //string SqlQuery = "Select Name,Status,CPU,CPUThreshold,TypeandName,Details,Description,SecondaryRole,Type,Location,PendingMail,PendingThreshold,DeadMail,DeadThreshold,HeldMail,HeldMailThreshold,"+
                        //     "OperatingSystem,DominoVersion,LastUpdate,NextScan,ResponseTime,UserCount from [VitalSigns].[dbo].[Status] " +
                        SqlQuery = SqlQry + " and StatusCode='" + status + "' and StatusCode is not Null";

						DagWheres = " Status='" + status + "' and Status is not Null";
                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);

                    }
                }
                else
                {
                    // status null
                    if (filterbyval != "null" && typeloc != "" && typeloc != null)
                    {
                        SqlQuery = SqlQry + " and (" +
                            (SessionViewBy == "ServerType" ? (secondaryRole ? " SecondaryRole LIKE '%" + typeloc + "%'" : " Type='" + typeloc + "'" ): "") +
                            (SessionViewBy == "Location" ? " Location='" + typeloc + "'" : "") +
                            (SessionViewBy == "Category" ? " Category='" + typeloc + "'" : "") +
                            ") and (type in (" + filterbyval + ") or Location in (" + filterbyval + ") or SecondaryRole in (" + filterbyval + ")) and StatusCode is not Null";

						DagWheres = " (" + (SessionViewBy == "Location" ? " loc.Location='" + typeloc + "'" : "") +
							" ) and (loc.Location in (" + filterbyval + ")) and Status is not Null";
                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);
                    }
                    else if (filterbyval == "null" && typeloc != "" && typeloc != null)
                    {
                        SqlQuery = SqlQry + " and (" +
                            (SessionViewBy == "ServerType" ? (secondaryRole ? " SecondaryRole LIKE '%" + typeloc + "%'" : " Type='" + typeloc + "'" ): "") + " " +
                            (SessionViewBy == "Location" ? " Location='" + typeloc + "'" : "") +
                            (SessionViewBy == "Category" ? " Category='" + typeloc + "'" : "") +
                            ") and (Location!='') and StatusCode is not Null";

						DagWheres = " (" + (SessionViewBy == "Location" ? " loc.Location='" + typeloc + "'" : "") +
							" ) and (loc.Location !='') and Status is not Null";
                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);
                    }
                    else if (filterbyval != "null" && (typeloc == "" || typeloc == null))
                    {
                        SqlQuery = SqlQry + " and (type in (" + filterbyval + ") or Location in (" + filterbyval + ") or SecondaryRole in (" + filterbyval + ")) and StatusCode is not Null";

						DagWheres = "(loc.Location in (" + filterbyval + ")) and Status is not Null";
                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);
                    }
                    else
                    {

						SqlQuery = SqlQry + " and (Location!='')  and StatusCode is not Null";

						DagWheres = "(loc.Location!='') and Status is not Null";
                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);

                    }
                }
				SqlQuery += " order by " + orderBy;
				StatusDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
            }

            return StatusDataTable;
        }
//        public DataTable GetStatusData(string status, string typeloc, string filterbyval, string SessionViewBy, string MapLoc,
//            string orderBy = "Name")
//        {

//            Boolean secondaryRole = false;
//            //9/1/2015 NS modified for VSPLUS-2096
//            //10/5/2015 NS modified for VSPLUS-2247
//            string SqlQry = "Select Type,isnull((select Location from Locations where ID=LocationID),Location) as Location,Category,case when Type='Office365' then category else Name end as Name,s.ID,Status," +
//                "case when statuscode='Not Responding' then 1 when statuscode='OK' then 3 when statuscode='Maintenance' then 4 else 2 end as ordernum, " +
//                "Details,LastUpdate,st.Description,PendingMail,DeadMail,MailDetails,Upcount,DownCount,UpPercent,ResponseTime,ResponseThreshold,PendingThreshold,DeadThreshold,UserCount,MyPercent,NextScan,DominoServerTasks,TypeandName,Icon,OperatingSystem,DominoVersion,UpMinutes,DownMinutes,UpPercentMinutes,PercentageChange,(CPU*100) as CPU,HeldMail,HeldMailThreshold,Severity,Memory,StatusCode,ISNULL(SecondaryRole,'') SecondaryRole," +
//                "case when st.type in ('Exchange', 'SharePoint', 'Active Directory', 'WebSphere') then isnull(( select [CPU_Threshold]*0.01 from [VitalSigns].[dbo].[ServerAttributes]  where serverid=(select srvr.id from [VitalSigns].[dbo].servers srvr,ServerTypes stp where srvr.ServerTypeID=stp.ID and stp.ServerType=st.type and srvr.ServerName=st.name)),0) when st.type='Domino' then isnull(( select [CPU_Threshold] from [VitalSigns].[dbo].[DominoServers] where serverid=(select srvr.id from [VitalSigns].[dbo].servers srvr, ServerTypes stp where srvr.ServerTypeID=stp.ID and stp.ServerType=st.type and srvr.ServerName=st.Name)),0) else CPUThreshold end as CPUThreshold," +
//            "case when Type='BES' then '../images/icons/BBDevice.gif' " +
//                   "when Type='Domino' then '../images/icons/dominoserver.gif'" +
//                    "when Type='Exchange' then '../images/icons/Exchange.jpg'" +
//                    "when Type='Notes Database' then '../images/icons/notesdb.gif'" +
//                     "when Type='Sametime' then '../images/icons/sametime.gif'" +
//                      "when Type='URL' then '../images/icons/url.gif'" +
//                       "when Type='Network Device' then '../images/icons/network.gif'" +
//                       "when Type='Active Directory' then '../images/icons/adicon2.png'" +
//                       "when Type='WebSphere' then '../images/icons/ibm.png'" +
//                   " else '../images/information.png' end as imgsource," +
//                   "convert(varchar,case when PendingThreshold>0 then (100*PendingMail)/PendingThreshold else 0 end) as PendingPercent," +
//"convert(varchar,case when DeadThreshold>0 then (100*DeadMail)/DeadThreshold else 0 end) as DeadPercent," +
//"convert(varchar,case when HeldMailThreshold>0 then (100*HeldMail)/HeldMailThreshold else 0 end) as HeldPercent," +
//"'DominoServerDetailsPage2.aspx?Name=' + Name + '&Type=' + Type + '&LastDate='+CONVERT(VARCHAR,LastUpdate,101) + ' ' + substring(convert(varchar(20), LastUpdate, 9), 13, 5) + ' ' + substring(convert(varchar(30), LastUpdate, 9), 25, 2) as redirecto," +
//"100*isnull(Memory,0) as MemoryPercent, case when st.type='Domino' then isnull((select memory_threshold from [VitalSigns].[dbo].dominoservers ds, [VitalSigns].[dbo].servers sr where ds.ServerID=sr.ID and ServerName=st.name),0) else case when st.type in  " +
//" ('Exchange', 'SharePoint', 'Active Directory', 'WebSphere') then isnull( (select [MemThreshold]*0.01 from [VitalSigns].[dbo].[ServerAttributes] ds, [VitalSigns].[dbo].servers sr where ds.ServerID=sr.ID and ServerName=st.name),0) else 0 end end as MemoryThreshold " +
//" from [VitalSigns].[dbo].[Status] st, (select  srvs.ID,srvs.ServerName,srvs.LocationID,srvs.ServerTypeID FROM Servers srvs,servertypes stp where srvs.servertypeid=stp.id and stp.servertype<>'Office365' union SELECT ID, Name,LocationID,ServerTypeID FROM URLs where Enabled=1 union  " +
//" select  0,Name,0,13 FROM NotesMailProbe where Enabled=1 union select [key],Name,LocationID,ServerTypeID FROM MailServices where Enabled=1 union select ID,Name,LocationID,ServerTypeID FROM [Network Devices] where Enabled=1 union select  0,Name,0,14  " +
//" FROM ExchangeMailProbe where Enabled=1 union select ndb.ID,ndb.Name,srv.LocationID,9 FROM NotesDatabases ndb, servers srv where ndb.ServerID=srv.ID  " +
// " union SELECT nd.ID, nd.Name, nd.LocationID, osrv.ServerTypeId FROM dbo.Nodes AS nd INNER JOIN dbo.O365Nodes AS ond ON nd.ID = ond.NodeID INNER JOIN dbo.O365Server AS osrv ON ond.O365ServerID = osrv.ID where (nd.LocationID <>'' and LocationID is not null) and  Enabled=1  " +
//") s where" +
//"((st.Name = s.ServerName and s.ServerTypeID in(select id from ServerTypes stp where stp.ServerType=st.Type and servertype<>'Office365') ) or " +

//"(st.Category = s.ServerName and s.ServerTypeID in(select id from ServerTypes stp where stp.ServerType=st.Type and servertype='Office365')))";

//            //" from [VitalSigns].[dbo].[Status] st, (select  ID,ServerName,LocationID,ServerTypeID FROM Servers union SELECT ID, Name,LocationID,ServerTypeID FROM URLs where Enabled=1  union select  0,Name,0,13 FROM NotesMailProbe where Enabled=1 union select [key],Name,LocationID,ServerTypeID FROM MailServices where Enabled=1 union select ID,Name,LocationID,ServerTypeID FROM [Network Devices] where Enabled=1) s  where st.Name = s.ServerName";

//            if (typeloc != "" && typeloc != null)
//            {
//                int i = typeloc.IndexOf('(');
//                if (i > 0)
//                {
//                    typeloc = typeloc.Substring(0, i - 1);
//                    secondaryRole = true;
//                }
//            }
//            DataTable StatusDataTable = new DataTable();
//            string SqlQuery;
//            string DagWheres;
//            try
//            {
//                if (MapLoc != "")
//                {
//                    SqlQuery = SqlQry + " and locationID in (" + MapLoc + ")";
//                }
//                else if (status != "" && status != null)
//                {
//                    if (filterbyval != "null" && typeloc != "" && typeloc != null && status != "" && status != null)
//                    {

//                        //string SqlQuery = "Select Name, Status,TypeandName,(CPU*100) as CPU,CPUThreshold,Details,Description,Location,SecondaryRole,Type,PendingMail,PendingThreshold,DeadMail,DeadThreshold,HeldMail,HeldMailThreshold,"+
//                        //"OperatingSystem,DominoVersion,LastUpdate,NextScan,ResponseTime,UserCount from [VitalSigns].[dbo].[Status] " +
//                        SqlQuery = SqlQry + " and StatusCode='" + status + "' and (" +
//                            (SessionViewBy == "ServerType" ? (secondaryRole ? " SecondaryRole LIKE '%" + typeloc + "%'" : " Type='" + typeloc + "'") : "") +
//                             (SessionViewBy == "Location" ? " Location='" + typeloc + "'" : "") +
//                            (SessionViewBy == "Category" ? " Category='" + typeloc + "'" : "") +
//                            ") and (Type in (" + filterbyval + ") or Location in (" + filterbyval + ")  or SecondaryRole in (" + filterbyval + ")) and StatusCode is not Null";

//                        DagWheres = "Status = '" + status + "' and (" +
//                             (SessionViewBy == "Location" ? " loc.Location='" + typeloc + "'" : "") +
//                            ") and (loc.Location in (" + filterbyval + ")) and Status is not Null";
//                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);
//                    }
//                    else if (typeloc != "" && typeloc != null && status != "" && status != null)
//                    {

//                        //string SqlQuery = "Select Name,Status,(CPU*100) as CPU,CPUThreshold,SecondaryRole,TypeandName,Details,Description,Type,Location,PendingMail,PendingThreshold,DeadMail,DeadThreshold,HeldMail,HeldMailThreshold,"+
//                        //     "OperatingSystem,DominoVersion,LastUpdate,NextScan,ResponseTime,UserCount from [VitalSigns].[dbo].[Status] " +
//                        SqlQuery = SqlQry + "  and StatusCode='" + status + "' and (" +
//                           (SessionViewBy == "ServerType" ? (secondaryRole ? " SecondaryRole LIKE '%" + typeloc + "%'" : " Type='" + typeloc + "'") : "") +
//                             (SessionViewBy == "Location" ? " Location='" + typeloc + "'" : "") +
//                            (SessionViewBy == "Category" ? " Category='" + typeloc + "'" : "") +
//                            ") and StatusCode is not Null";

//                        DagWheres = "Status = '" + status + "' and (" +
//                            (SessionViewBy == "Location" ? " loc.Location='" + typeloc + "'" : "") +
//                            ") and Status is not Null";
//                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);
//                    }
//                    else if (filterbyval != "null" && status != "" && status != null)
//                    {
//                        //string SqlQuery = "Select Name, Status,(CPU*100) as CPU,CPUThreshold,TypeandName,SecondaryRole,Details,Description,Type,Location,PendingMail,PendingThreshold,DeadMail,DeadThreshold,HeldMail,HeldMailThreshold,"+
//                        //     "OperatingSystem,DominoVersion,LastUpdate,NextScan,ResponseTime,UserCount from [VitalSigns].[dbo].[Status] " +
//                        SqlQuery = SqlQry + " and StatusCode='" + status + "' and (type in (" + filterbyval + ") or Location in (" + filterbyval + ")) and StatusCode is not Null";

//                        DagWheres = "Status='" + status + "' and (loc.Location in (" + filterbyval + ")) and Status is not Null";
//                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);


//                    }
//                    else
//                    {
//                        //string SqlQuery = "Select Name,Status,CPU,CPUThreshold,TypeandName,Details,Description,SecondaryRole,Type,Location,PendingMail,PendingThreshold,DeadMail,DeadThreshold,HeldMail,HeldMailThreshold,"+
//                        //     "OperatingSystem,DominoVersion,LastUpdate,NextScan,ResponseTime,UserCount from [VitalSigns].[dbo].[Status] " +
//                        SqlQuery = SqlQry + " and StatusCode='" + status + "' and StatusCode is not Null";

//                        DagWheres = " Status='" + status + "' and Status is not Null";
//                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);

//                    }
//                }
//                else
//                {
//                    // status null
//                    if (filterbyval != "null" && typeloc != "" && typeloc != null)
//                    {
//                        SqlQuery = SqlQry + " and (" +
//                            (SessionViewBy == "ServerType" ? (secondaryRole ? " SecondaryRole LIKE '%" + typeloc + "%'" : " Type='" + typeloc + "'") : "") +
//                            (SessionViewBy == "Location" ? " Location='" + typeloc + "'" : "") +
//                            (SessionViewBy == "Category" ? " Category='" + typeloc + "'" : "") +
//                            ") and (type in (" + filterbyval + ") or Location in (" + filterbyval + ") or SecondaryRole in (" + filterbyval + ")) and StatusCode is not Null";

//                        DagWheres = " (" + (SessionViewBy == "Location" ? " loc.Location='" + typeloc + "'" : "") +
//                            " ) and (loc.Location in (" + filterbyval + ")) and Status is not Null";
//                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);
//                    }
//                    else if (filterbyval == "null" && typeloc != "" && typeloc != null)
//                    {
//                        SqlQuery = SqlQry + " and (" +
//                            (SessionViewBy == "ServerType" ? (secondaryRole ? " SecondaryRole LIKE '%" + typeloc + "%'" : " Type='" + typeloc + "'") : "") + " " +
//                            (SessionViewBy == "Location" ? " Location='" + typeloc + "'" : "") +
//                            (SessionViewBy == "Category" ? " Category='" + typeloc + "'" : "") +
//                            ") and (Location!='') and StatusCode is not Null";

//                        DagWheres = " (" + (SessionViewBy == "Location" ? " loc.Location='" + typeloc + "'" : "") +
//                            " ) and (loc.Location !='') and Status is not Null";
//                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);
//                    }
//                    else if (filterbyval != "null" && (typeloc == "" || typeloc == null))
//                    {
//                        SqlQuery = SqlQry + " and (type in (" + filterbyval + ") or Location in (" + filterbyval + ") or SecondaryRole in (" + filterbyval + ")) and StatusCode is not Null";

//                        DagWheres = "(loc.Location in (" + filterbyval + ")) and Status is not Null";
//                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);
//                    }
//                    else
//                    {

//                        SqlQuery = SqlQry + " and (Location!='')  and StatusCode is not Null";

//                        DagWheres = "(loc.Location!='') and Status is not Null";
//                        //StatusDataTable = objAdaptor.FetchData(SqlQuery);

//                    }
//                }
//                SqlQuery += " order by " + orderBy;
//                StatusDataTable = objAdaptor.FetchData(SqlQuery);
//            }
//            catch (Exception)
//            {

//                throw;
//            }
//            finally
//            {
//            }

//            return StatusDataTable;
//        }


        public DataTable GetStatusChart(string Name, string Type)
        {
            DataTable Charttab = new DataTable();
            try
            {
                //12/12/2013 NS modified (column name change)
                //string SqlQuery = "Select [Date],[StatValue] from [VSS_Statistics].[dbo].[DeviceDailyStats] where devicetype = '" + Type + "' and DeviceName='" + Name + "' and StatName='ResponseTime' and Date > DATEADD (dd , -1 ,'2012-06-22 08:17:44.000')";
                string SqlQuery = "Select [Date],[StatValue] from [VSS_Statistics].[dbo].[DeviceDailyStats] where devicetype = '" + Type + "' and ServerName='" + Name + "' and StatName='ResponseTime' and Date > DATEADD (dd , -1 ,'2012-06-22 08:17:44.000')";
                Charttab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return Charttab;
        }

        public DataTable GetMoniteredServerTasks(string ServerName)
        {
            DataTable Charttab = new DataTable();
            try
            {
                string SqlQuery = "Select * from DominoServerTaskStatus where Monitored=1 and ServerName='"+ServerName+"'";
                Charttab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return Charttab;
        }

        public DataTable GetNonMoniteredServerTasks(string ServerName)
        {
            DataTable Charttab = new DataTable();
            try
            {
                string SqlQuery = "Select * from DominoServerTaskStatus where Monitored=0 and ServerName='"+ServerName+"'";
                Charttab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return Charttab;
        }
		public DataTable GetStatusData2()
		{
			DataTable diskdata = new DataTable();

			try
			{
				string SqlQuery = "select * from devicetypegridview dg left join diskyellowvalue dy on dg.name=dy.servername";
				diskdata = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception)
			{

				throw;
			}
			return diskdata;

		}


        public DataTable GetCombobox()
        {
            DataTable Statustab = new DataTable();

            try
            {
                string SqlQuery = "Select [Name]+'-'+Type NameandType from Status";
                Statustab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {
                
                throw;
            }
            return Statustab;
        
        }

        public DataTable GetMailStatus(string ServerName)
        {
            DataTable mailstatus = new DataTable();
            try
            {
                // string[] NameandType = ServerName.Split('-');                
                string SqlQuery = "select TypeANDName,Mail,Value " +
                               "from (select TypeANDName,isnull(DeadMail,0) DeadMail,isnull(PendingMail,0) PendingMail,isnull(cast(HeldMail as int),0) HeldMail from Status where Name='" + ServerName + "')P  " +
                                     "UNPIVOT " +
                                     "(Value FOR Mail IN  " +
                                         " (DeadMail,PendingMail,HeldMail))AS unpvt";
                //and Type='" + NameandType[1] + "'
                mailstatus = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return mailstatus;

        }
        //2/25/2016 Durga Modified for VSPLUS-2634
        public List<string> GetProcessStatus()
        {
            List<string> list = new List<string>();
            string ProcessStatus = "Stopped";
            //12/12/2013 NS added
            string DiffInMin = "0";
            try
            {
                DataTable statustab = new DataTable();

                //12/12/2013 NS modified - include difference in minutes between the current time and last run time
                //in the return string
                //10/29/2014 NS modified for VSPLUS-1129
                //string SqlQuery = "SELECT CONVERT(VARCHAR(10), max(LastUpdate), 101) + ' ' + CONVERT(VARCHAR(8), " +
                // "max(LastUpdate), 108) AS LastUpdate, ISNULL(DATEDIFF(MI,MAX(LastUpdate),GETDATE()),0) DiffInMin " +
                //    "FROM [VitalSigns].[dbo].[Status]";


				//string SqlQuery = "SELECT CONVERT(VARCHAR(10), max(LastUpdate), 101) + ' ' + " +
				//    "SUBSTRING(CONVERT(varchar(20),max(LastUpdate),22), 10, 11) AS LastUpdate, " +
				//    "ISNULL(DATEDIFF(MI,MAX(LastUpdate),GETDATE()),0) DiffInMin " +
				//    "FROM [VitalSigns].[dbo].[Status] ";

				string SqlQuery = " SELECT right(convert(varchar(32),max(LastUpdate),100),8)" +
                                    "+ ' ('+ CONVERT(VARCHAR(11),datediff(hh,getutcdate(), getdate())) + ' GMT' +') on ' +"+
                                      "REPLACE(CONVERT(VARCHAR(11), max(LastUpdate), 106), ' ', '-')  AS LastUpdate,"+
									  "ISNULL(DATEDIFF(MI,MAX(LastUpdate),GETDATE()),0) DiffInMin FROM [VitalSigns].[dbo].[Status]";

 
                statustab = objAdaptor.FetchData(SqlQuery);
                if (statustab.Rows.Count > 0)
                {
                    ProcessStatus = statustab.Rows[0][0].ToString();
                    //12/12/2013 NS added
                    DiffInMin = statustab.Rows[0][1].ToString();
                    //2/25/2016 Durga Modified for  VSPLUS-2634
                    list.Add(ProcessStatus);
                    list.Add(DiffInMin);
                    //ProcessStatus += " (-7 GMT)";
                }

            }
            catch (Exception)
            {

            }

            return list;
           
        }

        public DataTable GetMyCustomPages(string userID, bool isprivate, bool isadmin)
        {
            DataTable dt = new DataTable();
            try
            {
                string SqlQuery = "SELECT ID, URL, Title, IsPrivate FROM UserCustomPages WHERE UserID=" + userID + " ";
                if (!isprivate || isadmin)
                {
                    SqlQuery += "OR IsPrivate='False'";
                }
                else
                {
                    SqlQuery += "AND IsPrivate='True'";
                }
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public void DeleteMyCustomPage(string ID)
        {
            try
            {
                string SqlQuery = "DELETE FROM [UserCustomPages] WHERE [ID]=" + ID;
                objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }
        public DataTable GetStatusGrid(string Type)
        {
            try
            {
                DataTable dt = new DataTable();
                //3/4/2013 NS modified the query to exclude disabled servers per Alan's request
                //1/10/2014 NS modified the query to sort by status, then name
                //3/6/2014 NS modified the query to include Mail Services for VSPLUS-450
                //7/29/2014 NS modified the query to include Exchange servers
                //1/30/2015 NS modified the query to include LastUpdate for VSPLUS-1367
				string sql = "select Name,Type,Category,LastUpdate, case when Type='BES' then '../images/icons/BBDevice.gif' " +
                       "when Type='Domino' then '../images/icons/dominoserver.gif' " +
                        "when Type='Notes Database' then '../images/icons/notesdb.gif' " +
                         "when Type='Sametime' then '../images/icons/sametime.gif' " +
                          "when Type='URL' then '../images/icons/url.gif' " +
                           "when Type='Network Device' then '../images/icons/network.gif' " +
                           "when Type='Exchange' then '../images/icons/exchange.jpg' " +
                           "when Type='SharePoint' then '../images/icons/SP16.png' " +
                           "when Type='Mail' then '../images/icons/email.png' " +
                           "when Type='Active Directory' then '../images/icons/windows.gif' " +
                           "when Type='Database Availability Group' then '../images/icons/DAG-Health-Icon.png' " +
                           "when Type='Office365' then '../images/icons/O365.png' " +
						   "when Type='Office365' then '../images/icons/O365.png'"+
                       " else '../images/information.png' end as imgsource," +
                       "Location,Status, StatusCode," +
                       "case when statuscode='Not Responding' then 1 when statuscode='OK' then 3 when statuscode='Maintenance' then 4 else 2 end as ordernum, srv.IPAddress " +
					   "from status  st left outer join Servers srv on st.Name=srv.ServerName  and srv.ServerTypeID=(select ID from ServerTypes where ServerType=st.Type)" +
                       //1/30/2015 NS modified for VSPLUS-1367
                       //"where Type in (" + Type + ") AND Status !='Disabled' " + 
                       "where Status !='Disabled' " + 
                       "order by ordernum,Name";
                dt = objAdaptor.FetchData(sql);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetServerType(string notinType)
        {
            try
            {
                DataTable dt = new DataTable();
                string sql = "select distinct [Type] from status where type<>'" + notinType + "'";
                dt = objAdaptor.FetchData(sql);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetSpecificServerType(string inType)
        {
            try
            {
                DataTable dt = new DataTable();
                string sql = "select distinct [Type] from status where type='" + inType + "'";
                dt = objAdaptor.FetchData(sql);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetEXJournalData()
        {
            try
            {
                DataTable dt = new DataTable();
                //10/3/2013 NS modified the query below - added a total sort and excluded the servers that don't have the exjournals
                //10/9/2013 NS added ExJournalDate
                string sql = "SELECT Name, Status, (exjournal+exjournal1+exjournal2) as EXTotal, EXJournal, EXJournal1, EXJournal2, " +
                    "ExJournalDate,PendingMail, HeldMail, DeadMail FROM Status " +
                    "WHERE Type='Domino' AND (exjournal != -1 AND exjournal1 != -1 and exjournal2 != -1) " + 
                    "ORDER BY EXTotal DESC,Name";
                dt = objAdaptor.FetchData(sql);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        //Mukund 16Jul14, VSPLUS-741, VSPLUS-785 Disable/Enable Timer to update count in Header boxes
        //Same function is called for killing PageSessions & the data will be from a common table MenuItems
        public DataTable GetPageSessions()
        {
            try
            {
                
                DataTable dt = new DataTable();

                string sql = "SELECT * from MenuItems";
                dt = objAdaptor.FetchData(sql);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }

		public DataTable GetDeviceStatusHistory(int time)
		{
			try
			{
				DataTable dt = new DataTable();

				dt = objAdaptor.GetDataFromProcedure("StatusChanges","time",time.ToString());
				return dt;
			}
			catch (Exception)
			{
				throw;
			}
		}

        //4/10/2015 NS added
        public DataTable GetStatusCountByType()
        {
            DataTable dt = new DataTable();
            try
            {
                //2/8/2016 NS modified for VSPLUS-2531
                //2/23/2016 NS modified for VSPLUS-2531
                //20/05/2016 sowmya added for VSPLUS-2989
                string SqlQuery = "SELECT COUNT(Type) StatusCount,Type,ISNULL(StatusCode,'Not Scanned') StatusCode, " +
                    "CASE WHEN StatusCode='OK' THEN 1 WHEN StatusCode='Issue' THEN 2 WHEN StatusCode='Not Responding' " +
                    "THEN 3 ELSE 4 END AS OrderNum FROM Status  st INNER JOIN ServerTypes st1 " +
                    "ON st1.ServerType=st.Type INNER JOIN Servers srv " +
                    "ON st.Name=srv.ServerName AND st1.ID=srv.ServerTypeID WHERE Status !='Disabled' " +
                    "GROUP BY Type,StatusCode,CASE WHEN StatusCode='OK' THEN 1 WHEN StatusCode='Issue' THEN 2 " +
                    "WHEN StatusCode='Not Responding' THEN 3 ELSE 4 END " +
                    "UNION " +
                    "SELECT COUNT(Type) StatusCount,Type,ISNULL(StatusCode,'Not Scanned') StatusCode, " +
                    "CASE WHEN StatusCode='OK' THEN 1 WHEN StatusCode='Issue' THEN 2 WHEN StatusCode='Not Responding' " +
                    "THEN 3 ELSE 4 END AS OrderNum FROM Status  st INNER JOIN ServerTypes st1 " +
                    "ON st1.ServerType=st.Type WHERE st1.ID NOT IN (SELECT DISTINCT ServerTypeID FROM Servers) and Type not in('Domino Cluster database','Domino Cluster') " + 
                    "GROUP BY Type,StatusCode,CASE WHEN StatusCode='OK' THEN 1 WHEN StatusCode='Issue' THEN 2 " +
                    "WHEN StatusCode='Not Responding' THEN 3 ELSE 4 END " +
                    "ORDER BY Type,CASE WHEN StatusCode='OK' THEN 1 " +
                    "WHEN StatusCode='Issue' THEN 2 WHEN StatusCode='Not Responding' THEN 3 ELSE 4 END ";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable GetStatusCountTotalByType()
        {
            DataTable dt = new DataTable();
            try
            {
                //2/8/2016 NS modified for VSPLUS-2531
                //2/23/2016 NS modified for VSPLUS-2531
                string SqlQuery = "SELECT COUNT(Type) TotalStatusCount,Type FROM Status st INNER JOIN ServerTypes st1 " +
                    "ON st1.ServerType=st.Type INNER JOIN Servers srv ON st.Name=srv.ServerName AND " +
                    "st1.ID=srv.ServerTypeID WHERE Status !='Disabled' GROUP BY Type " +
                    "UNION " +
                    "SELECT COUNT(Type) TotalStatusCount,Type FROM Status st INNER JOIN ServerTypes st1 " +
                    "ON st1.ServerType=st.Type WHERE st1.ID NOT IN (SELECT DISTINCT ServerTypeID FROM Servers) " +
                    "GROUP BY Type " +
                    "ORDER BY Type ";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
		public DataTable GetDeviceStatusdiskdetails(string servername)
		{
			DataTable dt = new DataTable();
			try
			{
                string SqlQuery = "select DSP.ServerName,DSP.DiskName,DSP.DiskFree  from V_DominoDiskSpace DSP LEFT OUTER JOIN DominoDiskSettings DDS "+
                                  "ON (DDS.ServerName =DSP.ServerName) and (DSP.DiskName=DDS.DiskName or DDS.DiskName='AllDisks' )  where DSP.servername='" + servername + "'" +
                                  " UNION "+
                                  "SELECT DSP.ServerName,DSP.DiskName,DSP.DiskFree "+
                                  "FROM dbo.Servers srv INNER JOIN "+
                                  "dbo.DiskSettings DDS ON srv.ID = DDS.ServerID RIGHT OUTER JOIN "+ 
                                  "dbo.V_DiskSpace DSP ON srv.ServerName = DSP.ServerName and (DSP.DiskName=DDS.DiskName or DDS.DiskName='AllDisks' ) "+
                                  " where DSP.servername='" + servername + "'";
				dt = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception)
			{
				throw;
			}
			return dt;
		}
		public DataTable GetDeviceStatusdiskdetails2(string servername,string Diskname)
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = "select diskname,ROUND(ISNULL(diskfree,'0.0'),2) diskfree,ROUND(ISNULL(RedThresholdValue,'0.0'),2) RedThresholdValue from diskheastatusnew  where servername='" + servername + "' and diskname = ('" + Diskname + "' )";
				dt = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception)
			{
				throw;
			}
			return dt;
		}
		//2/9/16 Sowjanya added for VSPLUS-2527
		public DataTable GetDevicecount(string Name)
		{
			DataTable issuecount = new DataTable();

			try
            {//21/4/2016 Durga added for  VSPLUS-2872
				//string SqlQuery = " select  count(st.Name) as IssueCount   from status st  inner join AlertHistory Ah  on ah.DeviceName = st.Name  where ah.DeviceName='" + Name + "'and DateTimeAlertCleared IS NULL  group by st.Name";
                string SqlQuery = "Select count(DeviceName) as IssueCount from AlertHistory where DeviceName='" + Name + "' and (DateTimeOfAlert between DATEADD(day, -7, GetDate()) and GetDate() OR DateTimeAlertCleared IS NULL)  and DateTimeAlertCleared IS NULL group by DeviceName";
                issuecount = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception)
			{

				throw;
			}
			return issuecount;

		}
        //4/19/16 Sowjanya added for VSPLUS-2863
        public DataTable GetMonitoredURL(string Name)
        {
            DataTable issuecount = new DataTable();

            try
            {
                
                string SqlQuery = "Select URL  as MonitoredURL from CloudDetails where Name='" + Name + "'";
                issuecount = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {

                throw;
            }
            return issuecount;

        }
    }

}

