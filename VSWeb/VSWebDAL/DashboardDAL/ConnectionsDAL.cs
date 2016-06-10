using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebDAL.DashboardDAL
{
    public class ConnectionsDAL
    {
        private AdaptorforDsahBoard objAdaptor = new AdaptorforDsahBoard();
        private Adaptor adaptor = new Adaptor();
        private static ConnectionsDAL _self = new ConnectionsDAL();

        public static ConnectionsDAL Ins
        {
            get
            {
                return _self;
            }
        }

        public DataTable GetConnectionsTests()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "SELECT ID,[HTTPServerTest],[WebSphereAppServerTest],[RDBMSTest],[TivoliDirTest],[LDAPDirTest], " +
                    "[SMTPServerTest] FROM ConnectionsComponentsTest";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetNumberVisitors()
        {
            DataTable dt = new DataTable();
            try
            {
                //Last 30 days
                string strQuery = "SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, UpdatedDate)) Date, COUNT(DISTINCT ParticipantID) UserCount " + 
                    "FROM ConnectionsParticipantActivity " +
                    "WHERE DATEADD(dd, 0, DATEDIFF(dd, 0, UpdatedDate)) BETWEEN DATEADD(dd, 0, DATEDIFF(dd, 30, GETDATE())) AND " +
                    "DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) " + 
                    "GROUP BY DATEADD(dd, 0, DATEDIFF(dd, 0, UpdatedDate))";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetNumberVisits()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, UpdatedDate)) Date, COUNT(ParticipantID) VisitCount " +
                    "FROM ConnectionsParticipantActivity " +
                    "WHERE DATEADD(dd, 0, DATEDIFF(dd, 0, UpdatedDate)) BETWEEN DATEADD(dd, 0, DATEDIFF(dd, 30, GETDATE())) AND " +
                    "DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) " +
                    "GROUP BY DATEADD(dd, 0, DATEDIFF(dd, 0, UpdatedDate))";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetMostActiveApps()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "SELECT TOP 10 COUNT(*) TotalCount, ContentType FROM ConnectionsParticipantActivity t1 " +
                    "INNER JOIN ConnectionsContent t2 on t1.ContentID = t2.ID " +
                    "INNER JOIN ConnectionsContentTypes t3 on t2.ContentTypeID = t3.ID " +
                    "GROUP BY ContentTypeID, ContentType " + 
                    "ORDER BY COUNT(*) DESC";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetTop10Content()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "SELECT TOP 10 COUNT(*) TotalCount, ContentName FROM ConnectionsParticipantActivity t1 " +
                    "INNER JOIN ConnectionsContent t2 on t1.ContentID = t2.ID " +
                    "GROUP BY ContentID, ContentName " +
                    "ORDER BY COUNT(*) DESC";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //3/11/2016 NS added for VSPLUS-2651
        public DataTable GetConnectionsData()
        {
            DataTable dt = new DataTable();
            try
            {
                /*
                 DECLARE @T AS TABLE(y int NOT NULL PRIMARY KEY);
                    DECLARE
                    @cols AS NVARCHAR(MAX),
                    @y    AS int,
                    @sql  AS NVARCHAR(MAX);
                    -- Construct the column list for the IN clause
                    -- e.g., [1],[2],[3]
                    SET @cols = STUFF(
                    (SELECT N',' + QUOTENAME(y) AS [text()]
                    FROM (SELECT DISTINCT TestName AS y FROM StatusDetail  dd
                    INNER JOIN Status dm on dd.TypeAndName=dm.TypeAndName
                    WHERE dm.Type = 'IBM Connections') AS Y
                    ORDER BY y
                    FOR XML PATH('')),
                    1, 1, N'');
                    -- Construct the full T-SQL statement
                    -- and execute dynamically
                    SET @sql = N'SELECT *
                    FROM (SELECT sr.ServerName ServerName, sr.Id,ln.Location,s.Status,s.LastUpdate,TestName,Result
                    FROM  Status s INNER JOIN Servers sr ON s.Name=sr.ServerName
                    INNER JOIN ServerTypes st ON sr.ServerTypeID=st.ID AND s.Type=st.ServerType
                    INNER JOIN Locations ln ON sr.LocationID=ln.ID 
                    LEFT OUTER JOIN StatusDetail sd ON s.TypeAndName=sd.TypeAndName
                    WHERE st.ID=27
                    ) AS D
                    PIVOT(max(Result) FOR TestName IN(' + @cols + N')) AS P;';
                    EXEC sp_executesql @sql;
                    
                */
                string strQuery = "DECLARE @T AS TABLE(y int NOT NULL PRIMARY KEY); " +
                    "DECLARE " +
                    "@cols AS NVARCHAR(MAX), " +
                    "@y    AS int, " +
                    "@sql  AS NVARCHAR(MAX); " +
                    "SET @cols = STUFF( " +
                    "(SELECT N',' + QUOTENAME(y) AS [text()] " +
                    "FROM (SELECT DISTINCT TestName AS y FROM StatusDetail  dd " +
                    "INNER JOIN Status dm on dd.TypeAndName=dm.TypeAndName " +
                    "WHERE dm.Type = 'IBM Connections') AS Y " +
                    "ORDER BY y " +
                    "FOR XML PATH('')), " +
                    "1, 1, N''); " +
                    "SET @sql = N'SELECT * " +
                    "FROM (SELECT sr.ServerName ServerName, sr.Id,ln.Location,s.Status,s.LastUpdate,TestName,Result " +
                    "FROM  Status s INNER JOIN Servers sr ON s.Name=sr.ServerName " +
                    "INNER JOIN ServerTypes st ON sr.ServerTypeID=st.ID AND s.Type=st.ServerType " +
                    "INNER JOIN Locations ln ON sr.LocationID=ln.ID  " +
                    "LEFT OUTER JOIN StatusDetail sd ON s.TypeAndName=sd.TypeAndName " +
                    "WHERE st.ID=27 " +
                    " ) AS D " +
                    "PIVOT(max(Result) FOR TestName IN(' + @cols + N')) AS P;'; " +
                    "EXEC sp_executesql @sql; ";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetUsersDailyCount(string statname, string servername, string fromdate, string todate)
        {
            DataTable dt = new DataTable();
            try
            {
                if (fromdate == "" && todate == "")
                {
                    fromdate = "DATEADD(dd,-7,DATEDIFF(dd,0,GETDATE()))";
                    todate = "DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))";
                }
                else
                {
                    fromdate = "'" + fromdate + "'";
                    todate = "'" + todate + "'";
                }
                string SqlQuery = "SELECT SUBSTRING(StatName,1,CHARINDEX('_created_last_day',StatName)-1) StatName, " +
                    "DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date,MAX(StatValue) StatValue " +
                    "FROM IbmConnectionsSummaryStats INNER JOIN vitalsigns.dbo.Status ON " +
                    "ServerName=Name " +
                    "WHERE StatName LIKE '" + statname + "' AND ServerName='" + servername + "' " +
                    "AND DATEADD(dd,0,DATEDIFF(dd,0,Date)) BETWEEN " + fromdate + " AND " + todate + " " +
                    "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),StatName ";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetUserBlogs(string statname, string servername, string fromdate, string todate)
        {
            DataTable dt = new DataTable();
            try
            {
                if (fromdate == "" && todate == "")
                {
                    fromdate = "DATEADD(dd,-7,DATEDIFF(dd,0,GETDATE()))";
                    todate = "DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))";
                }
                else
                {
                    fromdate = "'" + fromdate + "'";
                    todate = "'" + todate + "'";
                }
                string SqlQuery = "SELECT  CASE WHEN StatName='Num_Of_Active_Blogs' THEN 'Active Blogs' " +
                    "WHEN StatName='Num_Of_Blogs_More_Then_One_Author' THEN 'More Than One Author' " +
                    "WHEN StatName='Num_Of_Published_Blogs' THEN 'Published Blogs' END StatName, " +
                    "DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date,MAX(StatValue) StatValue " +
                    "FROM IbmConnectionsSummaryStats INNER JOIN vitalsigns.dbo.Status ON " +
                    "ServerName=Name " +
                    "WHERE StatName LIKE '" + statname + "' AND ServerName='" + servername + "' " +
                    "AND DATEADD(dd,0,DATEDIFF(dd,0,Date)) BETWEEN " + fromdate + " AND " + todate + " " +
                    "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),StatName ";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetUserStatsCommon(string statname, string statname2, string servername, string fromdate, string todate)
        {
            DataTable dt = new DataTable();
            try
            {
                if (fromdate == "" && todate == "")
                {
                    fromdate = "DATEADD(dd,-7,DATEDIFF(dd,0,GETDATE()))";
                    todate = "DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))";
                }
                else
                {
                    fromdate = "'" + fromdate + "'";
                    todate = "'" + todate + "'";
                }
                string SqlQuery = "SELECT SUBSTRING(StatName,CHARINDEX('" + statname + "',StatName)+" + statname.Length.ToString() + 
                    ",CHARINDEX('_YESTERDAY',StatName)-1-" + statname.Length.ToString() + ") StatName, " +
                    "DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date,MAX(StatValue) StatValue " +
                    "FROM IbmConnectionsSummaryStats INNER JOIN vitalsigns.dbo.Status ON " +
                    "ServerName=Name " +
                    "WHERE StatName LIKE '%" + statname + "%" + statname2 + "%' AND ServerName='" + servername + "' " +
                    "AND DATEADD(dd,0,DATEDIFF(dd,0,Date)) BETWEEN DATEADD(dd,-7,DATEDIFF(dd,0,GETDATE())) AND DATEADD(dd,0,DATEDIFF(dd,0,GETDATE())) " +
                    "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),StatName ";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetActivities(string statname1, string statname2)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "SELECT ID,SUBSTRING(StatName,CHARINDEX(StatName,'" + statname1 + "') + " +
                    "LEN('" + statname1 + "')+2,LEN(StatName)-(LEN('" + statname1 + "')+1)) StatName,StatValue " +
                    "FROM [VSS_Statistics].[dbo].[IbmConnectionsSummaryStats] ";
                if (statname2 != "")
                {
                    strQuery += "WHERE (StatName LIKE '" + statname1 + "%' OR StatName LIKE '" + statname2 + "') ";
                }
                else
                {
                    strQuery += "WHERE StatName LIKE '" + statname1 + "%' ";
                }
                //29/4/2016 Durga Modified for VSPLUS-2909

                strQuery += "AND DATEDIFF(dd,0,Date)=DATEDIFF(dd,0,DATEADD(dd,0,GETDATE())) order by StatName ";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetDailyActivities()
        {
            DataTable dt = new DataTable();
            try
            {
                //4/21/2016 NS modified for VSPLUS-2824
                //29/4/2016 Durga Modified for VSPLUS-2909
                string strQuery = "SELECT ID,SUBSTRING(StatName,CHARINDEX(StatName,'NUM_OF_')+LEN('NUM_OF_')+1,LEN(StatName)-LEN('NUM_OF_')) StatName, StatValue " +
                    "FROM [VSS_Statistics].[dbo].[IbmConnectionsSummaryStats] " +
                    "WHERE (StatName LIKE 'NUM_OF%' " +
                    "AND StatName NOT LIKE '%_LAST_%' " +
                    "AND StatName NOT LIKE '%_YESTERDAY') " +
                    "AND DATEDIFF(dd,0,Date) = DATEDIFF(dd,0,DATEADD(dd,0,GETDATE())) order by StatName";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetProfileStats(string statname, string statname2, string servername, string stattitle, string stattitle2, bool specific)
        {
            DataTable dt = new DataTable();
            string SqlQuery = "";
            try
            {
                /*
                   SELECT StatName, StatValue FROM
                    (
                   SELECT [NoManager],[All]-[NoManager] AS [Manager] FROM
                    (SELECT SUM(StatValue) AS StatValue,CASE 
	                    WHEN StatName = 'NUM_OF_PROFILES_WITH_NO_MANAGER' THEN 'NoManager' 
                        WHEN StatName = 'NUM_OF_PROFILES_PROFILES' THEN 'All' 
                        END AS StatName 
                        FROM dbo.IbmConnectionsSummaryStats 
                        WHERE (StatName='NUM_OF_PROFILES_WITH_NO_MANAGER' OR StatName='NUM_OF_PROFILES_PROFILES')
                        AND DATEADD(dd,0,DATEDIFF(dd,0,Date)) = DATEADD(dd,0,DATEDIFF(dd,0,GETDATE()))
                        AND ServerName='IBM Connections'
                        GROUP BY StatName
                    )
                    AS sourcetable 
                    PIVOT (SUM(StatValue) FOR StatName in ([NoManager],[All])) as pivottabble
                    ) P
                    UNPIVOT
                    (StatValue FOR StatName IN (NoManager,Manager)) AS U
                */
                if (specific)
                {
                    SqlQuery = "SELECT StatName, StatValue FROM " +
                        "(" +
                        "SELECT [" + stattitle + "],[" + stattitle2 + "] FROM " +
                        "(SELECT SUM(StatValue) AS StatValue,CASE " +
                            "WHEN StatName = '" + statname + "' THEN '" + stattitle + "' " +
                            "WHEN StatName = '" + statname2 + "' THEN '" + stattitle2 + "' " +
                            "END AS StatName " +
                            "FROM dbo.IbmConnectionsSummaryStats " +
                            "WHERE (StatName='" + statname + "' OR StatName='" + statname2 + "') " +
                            "AND DATEADD(dd,0,DATEDIFF(dd,0,Date)) = DATEADD(dd,0,DATEDIFF(dd,0,GETDATE())) " +
                            "AND ServerName='" + servername + "' " +
                            "GROUP BY StatName " +
                        ") " +
                        "AS sourcetable " +
                        "PIVOT (SUM(StatValue) FOR StatName in ([" + stattitle + "],[" + stattitle2 + "])) as pivottabble " +
                        ") P " +
                        "UNPIVOT " +
                        "(StatValue FOR StatName IN ([" + stattitle + "],[" + stattitle2 + "])) AS U ";
                }
                else
                {
                    SqlQuery = "SELECT StatName, StatValue FROM " +
                        "(" +
                        "SELECT [" + stattitle + "],[All]-[" + stattitle + "] AS [" + stattitle2 + "] FROM " +
                        "(SELECT SUM(StatValue) AS StatValue,CASE " +
                            "WHEN StatName = '" + statname + "' THEN '" + stattitle + "' " +
                            "WHEN StatName = '" + statname2 + "' THEN 'All' " +
                            "END AS StatName " +
                            "FROM dbo.IbmConnectionsSummaryStats " +
                            "WHERE (StatName='" + statname + "' OR StatName='" + statname2 + "') " +
                            "AND DATEADD(dd,0,DATEDIFF(dd,0,Date)) = DATEADD(dd,0,DATEDIFF(dd,0,GETDATE())) " +
                            "AND ServerName='" + servername + "' " +
                            "GROUP BY StatName " +
                        ") " +
                        "AS sourcetable " +
                        "PIVOT (SUM(StatValue) FOR StatName in ([" + stattitle + "],[All])) as pivottabble " +
                        ") P " +
                        "UNPIVOT " +
                        "(StatValue FOR StatName IN ([" + stattitle + "],[" + stattitle2 + "])) AS U ";
                }
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }

        public DataTable GetTopTags(string statname)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "SELECT TOP 5 SUM(UsageCount) StatValue,Name StatName " +
                  "FROM [VSS_Statistics].[dbo].[IbmConnectionsTopStats] " +
                  "WHERE DATEDIFF(dd,0,DateTime)=DATEDIFF(dd,0,DATEADD(dd,0,GETDATE())) ";
                if (statname != "")
                {
                    strQuery += "AND Type='" + statname + "' ";
                }
                strQuery += "GROUP BY Name " +
                    "ORDER BY SUM(UsageCount) DESC ";
                dt = objAdaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetStatByName(string statname, bool isExact)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "SELECT StatName,StatValue " +
                  "FROM [VSS_Statistics].[dbo].[IbmConnectionsSummaryStats] " +
                  "WHERE DATEDIFF(dd,0,Date)=DATEDIFF(dd,0,DATEADD(dd,0,GETDATE())) ";
                if (statname != "")
                {
                    //5/31/2016 NS modified for VSPLUS-3009
                    if (isExact)
                    {
                        strQuery += "AND StatName='" + statname + "' ";
                    }
                    else
                    {
                        strQuery = "SELECT SUBSTRING(StatName,CHARINDEX('COMMUNITY_TYPE_',StatName)+LEN('COMMUNITY_TYPE_'),LEN(StatName)-LEN('COMMUNITY_TYPE_')) StatName,StatValue " +
                          "FROM [VSS_Statistics].[dbo].[IbmConnectionsSummaryStats] " +
                          "WHERE DATEDIFF(dd,0,Date)=DATEDIFF(dd,0,DATEADD(dd,0,GETDATE())) AND StatName LIKE '%" + statname + "%' ";
                    }
                }
                dt = objAdaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        //6/5/2016 Durga Added for VSPLUS-2925
        public DataTable GetUserStatsForActivities(string statname, string statname2, string servername, string fromdate, string todate)
        {
            DataTable dt = new DataTable();
            try
            {
                if (fromdate == "" && todate == "")
                {
                    fromdate = "DATEADD(dd,-7,DATEDIFF(dd,0,GETDATE()))";
                    todate = "DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))";
                }
                else
                {
                    fromdate = "'" + fromdate + "'";
                    todate = "'" + todate + "'";
                }

                string SqlQuery = "SELECT Case when StatName='NUM_OF_ACTIVITIES_ACTIVITIES_CREATED_YESTERDAY' then 'ACTIVITIES_CREATED' when StatName='NUM_OF_ACTIVITIES_ACTIVITIES_FOLLOWED_YESTERDAY' then 'ACTIVITIES_FOLLOWED' when StatName='ACTIVITY_LOGINS_LAST_DAY' then 'ACTIVITY_LOGINS' else StatName end StatName ,DATEADD(dd,0,DATEDIFF(dd,0,Date)) Date " +
                    "  ,MAX(StatValue) StatValue FROM IbmConnectionsSummaryStats INNER JOIN vitalsigns.dbo.Status ON ServerName=Name " +
                        "WHERE StatName LIKE '%" + statname + "%" + statname2 + "%' OR StatName LIKE 'ACTIVITY_LOGINS_LAST_DAY' AND ServerName='" + servername + "' " +
                        "AND DATEADD(dd,0,DATEDIFF(dd,0,Date)) BETWEEN DATEADD(dd,-7,DATEDIFF(dd,0,GETDATE())) AND DATEADD(dd,0,DATEDIFF(dd,0,GETDATE())) " +
                        "GROUP BY DATEADD(dd,0,DATEDIFF(dd,0,Date)),StatName ";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            return dt;
        }
        
        //6/1/2016 NS added for VSPLUS-3015
        public DataTable GetConnectionsUsers()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "SELECT DISTINCT DisplayName FROM IbmConnectionsUsers " +
                    "ORDER BY DisplayName ";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //6/1/2016 NS added for VSPLUS-3015
        public DataTable GetCommunitiesForUsers(string user1, string user2)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "(SELECT 'Communities in Common' Category,1 ParentID,t1.ID,Name FROM IbmConnectionsObjects t1 " +
                    "INNER JOIN IbmConnectionsObjectUsers t2 ON t1.ID=t2.ObjectId  " +
                    "INNER JOIN IbmConnectionsUsers t3 ON t2.UserId=t3.ID " +
                    "WHERE DisplayName='" + user1 + "' " +
                    "INTERSECT " +
                    "SELECT 'Communities in Common' Category,1 ParentID,t1.ID,Name FROM IbmConnectionsObjects t1  " +
                    "INNER JOIN IbmConnectionsObjectUsers t2 ON t1.ID=t2.ObjectId  " +
                    "INNER JOIN IbmConnectionsUsers t3 ON t2.UserId=t3.ID " +
                    "WHERE DisplayName='" + user2 + "' " +
                    ") " +
                    "UNION " +
                    "( " +
                    "SELECT 'Communities only " + user1 + " is a member of' Category,2 ParentID,t1.ID,Name FROM IbmConnectionsObjects t1  " +
                    "INNER JOIN IbmConnectionsObjectUsers t2 ON t1.ID=t2.ObjectId  " +
                    "INNER JOIN IbmConnectionsUsers t3 ON t2.UserId=t3.ID " +
                    "WHERE DisplayName='" + user1 + "' OR DisplayName='" + user2 + "' " +
                    "EXCEPT " +
                    "SELECT 'Communities only " + user1 + " is a member of' Category,2 ParentID,t1.ID,Name FROM IbmConnectionsObjects t1  " +
                    "INNER JOIN IbmConnectionsObjectUsers t2 ON t1.ID=t2.ObjectId  " +
                    "INNER JOIN IbmConnectionsUsers t3 ON t2.UserId=t3.ID " +
                    "WHERE DisplayName='" + user2 + "' " +
                    ") " +
                    "UNION " +
                    "( " +
                    "SELECT 'Communities only " + user2 + " is a member of' Category,3 ParentID,t1.ID,Name FROM IbmConnectionsObjects t1  " +
                    "INNER JOIN IbmConnectionsObjectUsers t2 ON t1.ID=t2.ObjectId  " +
                    "INNER JOIN IbmConnectionsUsers t3 ON t2.UserId=t3.ID " +
                    "WHERE DisplayName='" + user1 + "' OR DisplayName='" + user2 + "' " +
                    "EXCEPT " +
                    "SELECT 'Communities only " + user2 + " is a member of' Category,3 ParentID,t1.ID,Name FROM IbmConnectionsObjects t1  " +
                    "INNER JOIN IbmConnectionsObjectUsers t2 ON t1.ID=t2.ObjectId  " +
                    "INNER JOIN IbmConnectionsUsers t3 ON t2.UserId=t3.ID " +
                    "WHERE DisplayName='" + user1 + "' " +
                    ")";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //6/2/2016 NS added for VSPLUS-3011
        public DataTable GetMostActiveCommunity()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "SELECT COUNT(*) Total, a.Type, b.Name " +
                    "FROM IbmConnectionsObjects AS a LEFT JOIN IbmConnectionsObjects AS b ON a.ParentObjectID = b.ID " +
                    "WHERE a.ParentObjectID IS NOT NULL AND a.ParentObjectID!='' " +
                    "AND DATEDIFF(dd,0,DATEADD(dd,0,a.DateLastModified))>=DATEDIFF(dd,0,DATEADD(dd,-7,GETDATE())) " +
                    "AND b.Name=(SELECT TOP 1 b.Name " +
                    "FROM IbmConnectionsObjects AS a LEFT JOIN IbmConnectionsObjects AS b ON a.ParentObjectID = b.ID " +
                    "WHERE a.ParentObjectID IS NOT NULL AND a.ParentObjectID!='' " +
                    "AND DATEDIFF(dd,0,DATEADD(dd,0,a.DateLastModified))>=DATEDIFF(dd,0,DATEADD(dd,-7,GETDATE())) " +
                    "GROUP BY b.Name " +
                    "ORDER BY COUNT(*) desc " +
                    ") " +
                    "GROUP BY b.Name,a.Type";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //6/2/2016 NS added for VSPLUS-3021
        public DataTable GetUserAdoption()
        {
            DataTable dt = new DataTable();
            try
            {
                /* Below is an advanced report query
                 * SELECT COUNT(*) Total,CASE WHEN (Type='Blog' OR Type='Blog Entry') THEN 'Blog' 
WHEN (Type='Wiki' OR Type='Wiki Entry') THEN 'Wiki' ELSE Type END Type ,DisplayName,'Last 180 Days' LastDate
FROM dbo.IbmConnectionsObjects t1
INNER JOIN dbo.IbmConnectionsUsers t2 ON t2.ID=t1.OwnerId
WHERE Type!='Community' AND DATEDIFF(dd,0,DATEADD(dd,0,DateCreated))>=DATEDIFF(dd,0,DATEADD(dd,-180,GETDATE()))
GROUP BY CASE WHEN (Type='Blog' OR Type='Blog Entry') THEN 'Blog' 
WHEN (Type='Wiki' OR Type='Wiki Entry') THEN 'Wiki' ELSE Type END,DisplayName
UNION
SELECT COUNT(*) Total,CASE WHEN (Type='Blog' OR Type='Blog Entry') THEN 'Blog' 
WHEN (Type='Wiki' OR Type='Wiki Entry') THEN 'Wiki' ELSE Type END Type ,DisplayName,'Last 90 Days' LastDate
FROM dbo.IbmConnectionsObjects t1
INNER JOIN dbo.IbmConnectionsUsers t2 ON t2.ID=t1.OwnerId
WHERE Type!='Community' AND DATEDIFF(dd,0,DATEADD(dd,0,DateCreated))>=DATEDIFF(dd,0,DATEADD(dd,-90,GETDATE()))
GROUP BY CASE WHEN (Type='Blog' OR Type='Blog Entry') THEN 'Blog' 
WHEN (Type='Wiki' OR Type='Wiki Entry') THEN 'Wiki' ELSE Type END,DisplayName
UNION
SELECT COUNT(*) Total,CASE WHEN (Type='Blog' OR Type='Blog Entry') THEN 'Blog' 
WHEN (Type='Wiki' OR Type='Wiki Entry') THEN 'Wiki' ELSE Type END Type ,DisplayName,'Last 30 Days' LastDate
FROM dbo.IbmConnectionsObjects t1
INNER JOIN dbo.IbmConnectionsUsers t2 ON t2.ID=t1.OwnerId
WHERE Type!='Community' AND DATEDIFF(dd,0,DATEADD(dd,0,DateCreated))>=DATEDIFF(dd,0,DATEADD(dd,-30,GETDATE()))
GROUP BY CASE WHEN (Type='Blog' OR Type='Blog Entry') THEN 'Blog' 
WHEN (Type='Wiki' OR Type='Wiki Entry') THEN 'Wiki' ELSE Type END,DisplayName
                 */
                string strQuery = "SELECT COUNT(*) Total,CASE WHEN (Type='Blog' OR Type='Blog Entry') THEN 'Blog' " +
                    "WHEN (Type='Wiki' OR Type='Wiki Entry') THEN 'Wiki' ELSE Type END Type ,DisplayName  " +
                    "FROM dbo.IbmConnectionsObjects t1 " +
                    "INNER JOIN dbo.IbmConnectionsUsers t2 ON t2.ID=t1.OwnerId " +
                    "WHERE Type!='Community' AND DATEDIFF(dd,0,DATEADD(dd,0,DateCreated))>=DATEDIFF(dd,0,DATEADD(dd,-90,GETDATE())) " +
                    "GROUP BY CASE WHEN (Type='Blog' OR Type='Blog Entry') THEN 'Blog'  " +
                    "WHEN (Type='Wiki' OR Type='Wiki Entry') THEN 'Wiki' ELSE Type END,DisplayName";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //6/2/2016 NS added for VSPLUS-3019
        public DataTable GetUserActivity(string uname)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "SELECT COUNT(*) Total,CASE WHEN (Type='Blog' OR Type='Blog Entry') THEN 'Blog' " +
                    "WHEN (Type='Wiki' OR Type='Wiki Entry') THEN 'Wiki' ELSE Type END Type ,DisplayName,'Last 7 days' LastDate, 0 OrdNum   " +
                    "FROM dbo.IbmConnectionsObjects t1  " +
                    "INNER JOIN dbo.IbmConnectionsUsers t2 ON t2.ID=t1.OwnerId  " +
                    "WHERE Type!='Community' AND DATEDIFF(dd,0,DATEADD(dd,0,DateCreated))>=DATEDIFF(dd,0,DATEADD(dd,-7,GETDATE()))  " +
                    "GROUP BY CASE WHEN (Type='Blog' OR Type='Blog Entry') THEN 'Blog'   " +
                    "WHEN (Type='Wiki' OR Type='Wiki Entry') THEN 'Wiki' ELSE Type END,DisplayName " +
                    "UNION " +
                    "SELECT COUNT(*) Total,CASE WHEN (Type='Blog' OR Type='Blog Entry') THEN 'Blog'  " +
                    "WHEN (Type='Wiki' OR Type='Wiki Entry') THEN 'Wiki' ELSE Type END Type ,DisplayName,'Last 30 days' LastDate, 1 OrdNum   " +
                    "FROM dbo.IbmConnectionsObjects t1  " +
                    "INNER JOIN dbo.IbmConnectionsUsers t2 ON t2.ID=t1.OwnerId  " +
                    "WHERE Type!='Community' AND DATEDIFF(dd,0,DATEADD(dd,0,DateCreated))>=DATEDIFF(dd,0,DATEADD(dd,-30,GETDATE()))  " +
                    "GROUP BY CASE WHEN (Type='Blog' OR Type='Blog Entry') THEN 'Blog'   " +
                    "WHEN (Type='Wiki' OR Type='Wiki Entry') THEN 'Wiki' ELSE Type END,DisplayName " +
                    "UNION " +
                    "SELECT COUNT(*) Total,CASE WHEN (Type='Blog' OR Type='Blog Entry') THEN 'Blog'  " +
                    "WHEN (Type='Wiki' OR Type='Wiki Entry') THEN 'Wiki' ELSE Type END Type ,DisplayName,'Last 90 days' LastDate, 2 OrdNum   " +
                    "FROM dbo.IbmConnectionsObjects t1  " +
                    "INNER JOIN dbo.IbmConnectionsUsers t2 ON t2.ID=t1.OwnerId  " +
                    "WHERE Type!='Community' AND DATEDIFF(dd,0,DATEADD(dd,0,DateCreated))>=DATEDIFF(dd,0,DATEADD(dd,-90,GETDATE()))  " +
                    "GROUP BY CASE WHEN (Type='Blog' OR Type='Blog Entry') THEN 'Blog'   " +
                    "WHEN (Type='Wiki' OR Type='Wiki Entry') THEN 'Wiki' ELSE Type END,DisplayName";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //6/2/2016 NS added for VSPLUS-3016
        public DataTable GetSourceCommunity(string objtype)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "SELECT TOP 5 COUNT(*) AS Total,b.Name FROM IbmConnectionsObjects a " +
                    "INNER JOIN IbmConnectionsObjects b ON a.ParentObjectId=b.ID " +
                    "WHERE a.Type='" + objtype + "' " +
                    "GROUP BY b.Name " +
                    "ORDER BY Total DESC";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //6/3/2016 NS added for VSPLUS-3025
        public DataTable GetCommunityActivity(string uname)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "SELECT COUNT(*) Total,CASE WHEN (t1.Type='Blog' OR t1.Type='Blog Entry') THEN 'Blog' " +
                    "WHEN (t1.Type='Wiki' OR t1.Type='Wiki Entry') THEN 'Wiki' ELSE t1.Type END Type ,t2.Name,'Last 7 days' LastDate, 0 OrdNum   " +
                    "FROM dbo.IbmConnectionsObjects t1  " +
                    "INNER JOIN dbo.IbmConnectionsObjects t2 ON t2.ID=t1.ParentObjectID  " +
                    "WHERE t1.Type!='Community' AND DATEDIFF(dd,0,DATEADD(dd,0,t1.DateCreated))>=DATEDIFF(dd,0,DATEADD(dd,-7,GETDATE()))  " +
                    "GROUP BY CASE WHEN (t1.Type='Blog' OR t1.Type='Blog Entry') THEN 'Blog'   " +
                    "WHEN (t1.Type='Wiki' OR t1.Type='Wiki Entry') THEN 'Wiki' ELSE t1.Type END,t2.Name " +
                    "UNION " +
                    "SELECT COUNT(*) Total,CASE WHEN (t1.Type='Blog' OR t1.Type='Blog Entry') THEN 'Blog'  " +
                    "WHEN (t1.Type='Wiki' OR t1.Type='Wiki Entry') THEN 'Wiki' ELSE t1.Type END Type ,t2.Name,'Last 30 days' LastDate, 1 OrdNum   " +
                    "FROM dbo.IbmConnectionsObjects t1  " +
                    "INNER JOIN dbo.IbmConnectionsObjects t2 ON t2.ID=t1.ParentObjectID  " +
                    "WHERE t1.Type!='Community' AND DATEDIFF(dd,0,DATEADD(dd,0,t1.DateCreated))>=DATEDIFF(dd,0,DATEADD(dd,-30,GETDATE()))  " +
                    "GROUP BY CASE WHEN (t1.Type='Blog' OR t1.Type='Blog Entry') THEN 'Blog'   " +
                    "WHEN (t1.Type='Wiki' OR t1.Type='Wiki Entry') THEN 'Wiki' ELSE t1.Type END,t2.Name " +
                    "UNION " +
                    "SELECT COUNT(*) Total,CASE WHEN (t1.Type='Blog' OR t1.Type='Blog Entry') THEN 'Blog'  " +
                    "WHEN (t1.Type='Wiki' OR t1.Type='Wiki Entry') THEN 'Wiki' ELSE t1.Type END Type ,t2.Name,'Last 90 days' LastDate, 2 OrdNum   " +
                    "FROM dbo.IbmConnectionsObjects t1  " +
                    "INNER JOIN dbo.IbmConnectionsObjects t2 ON t2.ID=t1.ParentObjectID  " +
                    "WHERE t1.Type!='Community' AND DATEDIFF(dd,0,DATEADD(dd,0,t1.DateCreated))>=DATEDIFF(dd,0,DATEADD(dd,-90,GETDATE()))  " +
                    "GROUP BY CASE WHEN (t1.Type='Blog' OR t1.Type='Blog Entry') THEN 'Blog'   " +
                    "WHEN (t1.Type='Wiki' OR t1.Type='Wiki Entry') THEN 'Wiki' ELSE t1.Type END,t2.Name";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        //6/3/2016 NS added for VSPLUS-3012
        public DataTable GetTop5MostActiveCommunities()
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "SELECT COUNT(*) Total, a.Type, b.Name " +
                    "FROM IbmConnectionsObjects AS a LEFT JOIN IbmConnectionsObjects AS b ON a.ParentObjectID = b.ID  " +
                    "WHERE a.ParentObjectID IS NOT NULL AND a.ParentObjectID!=''  " +
                    "AND DATEDIFF(dd,0,DATEADD(dd,0,a.DateLastModified))>=DATEDIFF(dd,0,DATEADD(dd,-7,GETDATE()))  " +
                    "AND b.Name IN (SELECT TOP 5 b.Name  " +
                    "FROM IbmConnectionsObjects AS a LEFT JOIN IbmConnectionsObjects AS b ON a.ParentObjectID = b.ID  " +
                    "WHERE a.ParentObjectID IS NOT NULL AND a.ParentObjectID!=''  " +
                    "AND DATEDIFF(dd,0,DATEADD(dd,0,a.DateLastModified))>=DATEDIFF(dd,0,DATEADD(dd,-7,GETDATE()))  " +
                    "GROUP BY b.Name  " +
                    "ORDER BY COUNT(*) desc  " +
                    ")  " +
                    "GROUP BY b.Name,a.Type " +
                    "ORDER BY a.Type,b.Name";
                dt = adaptor.FetchData(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
    }
}
