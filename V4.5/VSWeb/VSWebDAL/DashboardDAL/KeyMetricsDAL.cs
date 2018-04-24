using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VSWebDAL.DashboardDAL
{
    public class KeyMetricsDAL
    {
        /// <summary>
        /// Declarations
        /// </summary>
        private Adaptor objAdaptor = new Adaptor();
        //private AdaptorforDsahBoard Dash new AdaptorforDsahBoard();
        private static KeyMetricsDAL _self = new KeyMetricsDAL();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
        public static KeyMetricsDAL Ins
        {
            get { return _self; }
        }

        public DataTable GetUserCount(string parm,string stype)
        {
            //10/10/2013 NS modified
            //2/4/2015 NS modified for 
            string sortorder = "";
            if (parm == "Server")
            {
                sortorder = "'" + parm + "' DESC";
            }
            else
            {
                sortorder = "'" + parm + "' ASC";
            }
            DataTable statTab = new DataTable();
            string SqlQuery = "";
            try
            {
                //2/13/2015 NS modified for VSPLUS-1346
                if (stype == "" || stype == "All")
                {
                    SqlQuery = "select LEFT(Name,20)+'/'+Type as Server,UserCount from Status where UserCount>0 order by " + sortorder;
                }
                else
                {
                    SqlQuery = "select Name as Server,UserCount from Status " +
                        "where UserCount>0 and Type IN('" + stype + "') " +
                        "order by " + sortorder;
                }
                statTab = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return statTab;
        }


        public DataTable GetResponseTime(string parm,string sname,string sType,string location)
        {
            //10/3/2013 NS modified
            string sortorder = "";
            string SqlQuery = "";
            if (parm == "Server")
            {
                sortorder = "'" + parm + "' DESC";
            }
            else
            {
                sortorder = "'" + parm + "' ASC";
            }
            DataTable statTab = new DataTable();
            try
            {
                //9/26/2013 NS modified the queries below - we'll try to allow a single type selection to limit the number of entries per page
                //2/15/2015 NS modified for VSPLUS-1346
                //Server Type is not All or empty
                if (sType != "All" && sType != "")
                {
                    //Location is not All or empty
                    if (location != "All" && location != "")
                    {
                        //Server Name filter is not empty
                        if (sname != "")
                        {
                            SqlQuery = "select Name as Server,ResponseTime from Status where ResponseTime>0 and Name Like '" +
                                "%" + sname + "%" + "' and Type='" + sType + "' and Location='" + location + "' order by " + sortorder;
                        }
                        //Server Name filter is empty
                        else
                        {
                            SqlQuery = "select Name as Server,ResponseTime from Status where ResponseTime>0 " +
                                " and Type='" + sType + "' and Location='" + location + "' order by " + sortorder;
                        }
                    }
                    //Location is empty or All
                    else
                    {
                        //Server Name filter is not empty
                        if (sname != "")
                        {
                            SqlQuery = "select Name as Server,ResponseTime from Status where ResponseTime>0 and Name Like '" +
                                "%" + sname + "%" + "' and Type='" + sType + "' order by " + sortorder;
                        }
                        //Server Name filter is empty
                        else
                        {
                            SqlQuery = "select Name as Server,ResponseTime from Status where ResponseTime>0 " +
                                " and Type='" + sType + "' order by " + sortorder;
                        }
                    }
                }
                //Server Type is empty or All
                else
                {
                    //Location is not All or empty
                    if (location != "All" && location != "")
                    {
                        //Server Name filter is not empty
                        if (sname != "")
                        {
                            SqlQuery = "select Name as Server,ResponseTime from Status where ResponseTime>0 and Name Like '" +
                                "%" + sname + "%" + "' and Location='" + location + "' order by " + sortorder;
                        }
                        //Server Name filter is empty
                        else
                        {
                            SqlQuery = "select Name as Server,ResponseTime from Status where ResponseTime>0 " +
                                " and Location='" + location + "' order by " + sortorder;
                        }
                    }
                    //Location is empty or All
                    else
                    {
                        //Server Name filter is not empty
                        if (sname != "")
                        {
                            SqlQuery = "select Name as Server,ResponseTime from Status where ResponseTime>0 and Name Like '" +
                                "%" + sname + "%" + "' order by " + sortorder;
                        }
                        //Server Name filter is empty
                        else
                        {
                            SqlQuery = "select Name as Server,ResponseTime from Status where ResponseTime>0 " +
                                " order by " + sortorder;
                        }
                    }
                }

                statTab = objAdaptor.FetchData(SqlQuery);
                /*
                if (sname != "" && sType != "All" && sname != "" && sType !=null && location != null)
                {
                   
                    //string SqlQuery = "select LEFT(Name,20)+'/'+Type as Server,ResponseTime from Status where ResponseTime>0 and Name Like '"+"%"+sname+"%" +"' and Type='"+sType+"' order by '" + parm + "' Asc";
                    //string SqlQuery = "select LEFT(Name,25) as Server,ResponseTime from Status where ResponseTime>0 and Name Like '" + 
                    string SqlQuery = "select Name as Server,ResponseTime from Status where ResponseTime>0 and Name Like '" + 
                        "%" + sname + "%" + "' and Type='" + sType + "' and Location='" + location + "' order by " + sortorder;
                    statTab = objAdaptor.FetchData(SqlQuery);
                }
                else if(sname != ""&&sname != null&&(sType == "All"|| sType ==null)&&(location == "All" || location == null))
                {
                    //string SqlQuery = "select LEFT(Name,20)+'/'+Type as Server,ResponseTime from Status where ResponseTime>0 and Name Like '" + "%" + sname + "%" + "' order by '" + parm + "' Asc";
                    //string SqlQuery = "select LEFT(Name,25) as Server,ResponseTime from Status where ResponseTime>0 and Name Like '" +
                    string SqlQuery = "select Name as Server,ResponseTime from Status where ResponseTime>0 and Name Like '" +
                        "%" + sname + "%" + "' order by " + sortorder;
                    statTab = objAdaptor.FetchData(SqlQuery);
                }
                else if (sType != "All" && sType != null && location != "All" && location != null &&(sname == "" || sname == null))
                {
                    //string SqlQuery = "select LEFT(Name,20)+'/'+Type as Server,ResponseTime from Status where ResponseTime>0 and Type='" + sType + "' order by '" + parm + "' Asc";
                    //string SqlQuery = "select LEFT(Name,25) as Server,ResponseTime from Status where ResponseTime>0 and Type='" +
                    string SqlQuery = "select Name as Server,ResponseTime from Status where ResponseTime>0 and Type='" +
                        sType + "' and Location='" + location + "' order by " + sortorder;
                    statTab = objAdaptor.FetchData(SqlQuery);
                }
                else
                {
                    //string SqlQuery = "select LEFT(Name,20)+'/'+Type as Server,ResponseTime from Status where ResponseTime>0  order by '" + parm + "' Asc";
                    //string SqlQuery = "select LEFT(Name,25) as Server,ResponseTime from Status where ResponseTime>0  order by " + sortorder;
                    string SqlQuery = "select Name as Server,ResponseTime from Status where ResponseTime>0  order by " + sortorder;
                    statTab = objAdaptor.FetchData(SqlQuery);
                }
                 */
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return statTab;
        }
            

        public DataTable GetKeyMetrics(string ServerLoc)
        {
            DataTable dt = new DataTable();
            try
            {
                //9/12/2013 NS modified the queries below - added Type to the Where clause, which may need to be updated
                //or removed when new server types are implemented (i.e., Exchange)
                if (ServerLoc == "null")
                {
                    //string Sqlquery = "select sum(PendingMail) PendingMail,sum(DeadMail) DeadMail,sum(HeldMail) HeldMail,sum(UserCount) as UserCount, Avg(ResponseTime) as Resp,sum(DownMinutes) as DownMinutes  from Status where Name in (select Name from Status where  StatusCode in ('Issue','Maintenance','Not Responding','OK') and Location <>'' and Type<>'' and StatusCode is not null)";
					//string Sqlquery = "SELECT SUM(PendingMail) PendingMail,SUM(DeadMail) DeadMail,SUM(HeldMail) HeldMail, " +
					//    "SUM(UserCount) as UserCount, AVG(ResponseTime) as Resp,SUM(DownMinutes) as DownMinutes " +
					//    "FROM Status WHERE Name IN (SELECT Name FROM Status WHERE  StatusCode in " +
					//    "('Issue','Maintenance','Not Responding','OK') AND Location <>'' AND Type<>'' " +
					//    "AND StatusCode is not null) AND Type='Domino' ";
					string Sqlquery = "SELECT SUM(PendingMail) PendingMail,SUM(DeadMail) DeadMail,SUM(HeldMail) HeldMail, SUM(UserCount) as UserCount, AVG(ResponseTime) as Resp,SUM(DownMinutes) as DownMinutes FROM Status st WHERE Name IN (SELECT Name FROM Status WHERE  StatusCode in ('Issue','Maintenance','Not Responding','OK') AND Location <>'' AND Type<>'' AND StatusCode is not null)  AND Type='Domino'";
                    dt = objAdaptor.FetchData(Sqlquery);
                }
                else
                {
                    //string Sqlquery = "select sum(PendingMail) PendingMail,sum(DeadMail) DeadMail,sum(HeldMail) HeldMail,sum(UserCount) as UserCount, Avg(ResponseTime) as Resp,sum(DownMinutes) as DownMinutes  from Status where Name in (select Name from Status where (Type in (" + ServerLoc + ") or Location in (" + ServerLoc + "))and Location <>'' and StatusCode in ('Issue','Maintenance','Not Responding','OK'))";
                    string Sqlquery = "SELECT SUM(PendingMail) PendingMail,SUM(DeadMail) DeadMail,SUM(HeldMail) HeldMail, " + 
                        "SUM(UserCount) as UserCount, AVG(ResponseTime) as Resp,SUM(DownMinutes) as DownMinutes " + 
                        "FROM Status WHERE Name IN (SELECT Name FROM Status WHERE (Type IN (" + ServerLoc + ") OR " + 
                        "Location IN (" + ServerLoc + ")) AND Location <>'' AND StatusCode IN " +
                        "('Issue','Maintenance','Not Responding','OK')) AND Type='Domino'";
                    dt = objAdaptor.FetchData(Sqlquery);
                }

            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }
		public DataTable GetKeyMetricsvisible(string ServerLoc)
		{
			DataTable dt = new DataTable();
			try
			{
				//9/12/2013 NS modified the queries below - added Type to the Where clause, which may need to be updated
				//or removed when new server types are implemented (i.e., Exchange)
				if (ServerLoc == "null")
				{
					//string Sqlquery = "select sum(PendingMail) PendingMail,sum(DeadMail) DeadMail,sum(HeldMail) HeldMail,sum(UserCount) as UserCount, Avg(ResponseTime) as Resp,sum(DownMinutes) as DownMinutes  from Status where Name in (select Name from Status where  StatusCode in ('Issue','Maintenance','Not Responding','OK') and Location <>'' and Type<>'' and StatusCode is not null)";
					//string Sqlquery = "SELECT SUM(PendingMail) PendingMail,SUM(DeadMail) DeadMail,SUM(HeldMail) HeldMail, " +
					//    "SUM(UserCount) as UserCount, AVG(ResponseTime) as Resp,SUM(DownMinutes) as DownMinutes " +
					//    "FROM Status WHERE Name IN (SELECT Name FROM Status WHERE  StatusCode in " +
					//    "('Issue','Maintenance','Not Responding','OK') AND Location <>'' AND Type<>'' " +
					//    "AND StatusCode is not null) AND Type='Domino' ";
					string Sqlquery = "SELECT SUM(PendingMail) PendingMail,SUM(DeadMail) DeadMail,SUM(HeldMail) HeldMail, SUM(UserCount) as UserCount, AVG(ResponseTime) as Resp,SUM(DownMinutes) as DownMinutes FROM Status st WHERE Name IN (SELECT Name FROM Status WHERE  StatusCode in ('Issue','Maintenance','Not Responding','OK') AND Location <>'' AND Type<>'' AND StatusCode is not null)  AND Type='Domino' ";
					dt = objAdaptor.FetchData(Sqlquery);
				}
				else
				{
					//string Sqlquery = "select sum(PendingMail) PendingMail,sum(DeadMail) DeadMail,sum(HeldMail) HeldMail,sum(UserCount) as UserCount, Avg(ResponseTime) as Resp,sum(DownMinutes) as DownMinutes  from Status where Name in (select Name from Status where (Type in (" + ServerLoc + ") or Location in (" + ServerLoc + "))and Location <>'' and StatusCode in ('Issue','Maintenance','Not Responding','OK'))";
					string Sqlquery = "SELECT SUM(PendingMail) PendingMail,SUM(DeadMail) DeadMail,SUM(HeldMail) HeldMail, " +
						"SUM(UserCount) as UserCount, AVG(ResponseTime) as Resp,SUM(DownMinutes) as DownMinutes " +
						"FROM Status WHERE Name IN (SELECT Name FROM Status WHERE (Type IN (" + ServerLoc + ") OR " +
						"Location IN (" + ServerLoc + ")) AND Location <>'' AND StatusCode IN " +
						"('Issue','Maintenance','Not Responding','OK')) AND Type='Domino'";
					dt = objAdaptor.FetchData(Sqlquery);
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dt;
		}

        //10/20/2015 NS modified for VSPLUS-2072
        public DataTable GetType(string includesrv)
        {
            DataTable typetab = new DataTable();
            try
            {
                //2/13/2015 NS modified for VSPLUS-1346
                //3/22/2016 NS modified for VSPLUS-2736
                string SqlQuery = "Select Distinct Type, 1 AS OrderNum from Status " + 
                    "WHERE ElapsedDays IS NOT NULL ";
                if (includesrv != "")
                {
                    SqlQuery += "AND Type IN(" + includesrv + ") ";
                }
                SqlQuery += "UNION " +
                    "SELECT 'All' AS Type, 0 AS OrderNum ";
                SqlQuery += "ORDER BY OrderNum,Type ";
                typetab = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}

            return typetab;         
        }
		public DataTable GetspecificServerType(string page,string control)
		{
			DataTable typetab = new DataTable();
			try
			{
				//2/13/2015 NS modified for VSPLUS-1346
				//2/16/1016 Durga modified for VSPLUS-2611
                string SqlQuery = "select st.ID,st.ServerType from ServerTypes st inner join SelectedFeatures sf on sf.FeatureID=st.FeatureId  where id in(select ServertypeID from  Servertypeexcludelist where Page='" + page + "' and Control='" + control + "') order by st.[ServerType]";
				typetab = objAdaptor.FetchData(SqlQuery);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return typetab;
		}
        //2/13/2015 NS added for VSPLUS-1346
        public DataTable GetLocation()
        {
            DataTable typetab = new DataTable();
            try
            {
                //2/13/2015 NS modified for VSPLUS-1346
                string SqlQuery = "Select Distinct Location, 1 AS OrderNum from Status " + 
                    "UNION " +
                    "SELECT 'All' AS Location, 0 AS OrderNum " +
                    "ORDER BY OrderNum,Location ";
                typetab = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}

            return typetab;
        }

        //10/30/2013 NS added
        public DataTable GetServerDaysUp(string parm,string stype)
        {
            string SqlQuery;
            string sortorder = "";
            if (parm == "Server")
            {
                sortorder = "'" + parm + "' DESC";
            }
            else
            {
                sortorder = "'" + parm + "' ASC";
            }
            DataTable statTab = new DataTable();
            try
            {
                //2/13/2015 NS modified for VSPLUS-1346
                //3/22/2016 NS modified for VSPLUS-2736
                if (stype != null && stype != "" && stype != "All")
                {
                    SqlQuery = "SELECT Name AS Server, ElapsedDays Duration FROM Status " +
                        "WHERE Type='" + stype + "' AND ElapsedDays IS NOT NULL " + 
                        "ORDER BY " + sortorder;

                    if (stype == "WebSphere")
                    {
                        SqlQuery = "SELECT Name AS Server, ElapsedDays Duration FROM Status " +
                        "WHERE Type='" + stype + "'  and  Details!='" + "WebSphere Cell" + "' and Details!='" + "WebSphere Node" + "' " +
                        "AND ElapsedDays IS NOT NULL " + 
                        "ORDER BY " + sortorder;
                    }
                }
                else
                {
                    SqlQuery = "SELECT LEFT(Name,20)+'/'+Type Server, ElapsedDays Duration FROM Status where Details!='" + "WebSphere Cell" + 
                        "' and Details!='" + "WebSphere Node" +
                        "' AND ElapsedDays IS NOT NULL " +
                        "ORDER BY " + sortorder;
                }
                statTab = objAdaptor.FetchData(SqlQuery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return statTab;
        }

        //2/5/2014 NS added for VSPLUS-211
        public DataTable GetServerDownTime()
        {
            DataTable dt = new DataTable();
            try
            {
                string Sqlquery = "SELECT Name,DownMinutes " +
                        "FROM Status WHERE Name IN (SELECT Name FROM Status WHERE  StatusCode in " +
                        "('Issue','Maintenance','Not Responding','OK') AND Location <>'' AND Type<>'' " +
                        "AND StatusCode is not null) AND Type='Domino' AND DownMinutes > 0 " +
                        "ORDER BY DownMinutes,Name";
                dt = objAdaptor.FetchData(Sqlquery);
            }
			catch (Exception ex)
			{
				throw ex;
			}
            return dt;
        }

        //3/17/2016 Durga Added for VSPLUS-2696
        public DataTable GetCostperuserserveddata(string parm, string sname, string sType, string UserType)

        {
           
            string sortorder = "";
            string SqlQuery = "";
            if (parm == "1")
            {
              //  sortorder = parm + "DESC";
                sortorder = " DESC";
            }
            else
            {
                sortorder = " ASC";
            }
            DataTable statTab = new DataTable();
            try
            {

                if (sType != "All" && sType != "")
                {
                    
                    //UserType is not All or empty
                    if (UserType != "All" && UserType != "")
                    {
                        //Server Name filter is not empty
                        if (sname != "")
                        {

                            SqlQuery = "select avg(StatValue) as StatValue,MonthlyOperatingCost,sr.servername,statname from [VSS_Statistics].dbo.MicrosoftSummaryStats ms inner join  [vitalsigns].dbo.servers sr on ms.ServerName=sr.ServerName inner join [vitalsigns].dbo.ServerTypes st  on st.ID=sr.ServerTypeID" +
                                " where  date>DATEADD(month, -1, GETDATE()) and  sr.servername Like '" + "%" + sname + "%" + "' and statname like  '" + "AvgCAS@" + UserType + "#User.Count' and st.ServerType='" + sType + "'  group by statname,sr.servername,MonthlyOperatingCost  order by StatValue " + sortorder + ",sr.servername";
                        }
                        //Server Name filter is empty
                        else
                        {
                            SqlQuery = "select avg(StatValue) as StatValue,MonthlyOperatingCost,sr.servername,statname from [VSS_Statistics].dbo.MicrosoftSummaryStats ms inner join  [vitalsigns].dbo.servers sr on ms.ServerName=sr.ServerName inner join [vitalsigns].dbo.ServerTypes st  on st.ID=sr.ServerTypeID" +
                                " where  date>DATEADD(month, -1, GETDATE()) and  statname like  '" + "AvgCAS@" + UserType + "#User.Count' and st.ServerType='" + sType + "'  group by statname,sr.servername,MonthlyOperatingCost  order by StatValue " + sortorder + ",sr.servername";
                        }
                    }
                    //UserType is empty or All
                    else
                    {
                        //Server Name filter is not empty
                        if (sname != "")
                        {

                            SqlQuery = "select avg(StatValue) as StatValue,MonthlyOperatingCost,sr.servername,statname from [VSS_Statistics].dbo.MicrosoftSummaryStats ms inner join  [vitalsigns].dbo.servers sr on ms.ServerName=sr.ServerName inner join [vitalsigns].dbo.ServerTypes st  on st.ID=sr.ServerTypeID" +
                                "where  date>DATEADD(month, -1, GETDATE()) and statname like  '" + "AvgCAS" + "%" + "' and sr.servername Like '" + "%" + sname + "%" + "' and st.ServerType='" + sType + "' group by statname,sr.servername,MonthlyOperatingCost  order by StatValue " + sortorder + ",sr.servername";
                        }
                        //Server Name filter is empty
                        else
                        {


                            SqlQuery = "select avg(StatValue) as StatValue,MonthlyOperatingCost,sr.servername,statname from [VSS_Statistics].dbo.MicrosoftSummaryStats ms inner join  [vitalsigns].dbo.servers sr on ms.ServerName=sr.ServerName inner join [vitalsigns].dbo.ServerTypes st  on st.ID=sr.ServerTypeID" +
                               " where  date>DATEADD(month, -1, GETDATE()) and  statname like  '" + "AvgCAS" + "%" + "' and  sr.servername Like '" + "%" + sname + "%" + "' and st.ServerType='" + sType + "' group by statname,sr.servername,MonthlyOperatingCost  order by StatValue " + sortorder + ",sr.servername";
                        }
                    }
                }
                //Server Type is empty or All
                else
                {
                    //UserType is not All or empty
                    if (UserType != "All" && UserType != "")
                    {
                        //Server Name filter is not empty
                        if (sname != "")
                        {
                           // SqlQuery = "select Name as Server,ResponseTime from Status where ResponseTime>0 and Name Like '" +
                                //"%" + sname + "%" + "' and Location='" + UserType + "' order by " + sortorder;
                            SqlQuery = "select avg(StatValue) as StatValue,MonthlyOperatingCost,sr.servername,statname from [VSS_Statistics].dbo.MicrosoftSummaryStats ms inner join  [vitalsigns].dbo.servers sr on ms.ServerName=sr.ServerName inner join [vitalsigns].dbo.ServerTypes st  on st.ID=sr.ServerTypeID" +
                                " where  date>DATEADD(month, -1, GETDATE()) and  sr.servername Like '" + "%" + sname + "%" + "' and statname like  '" + "AvgCAS@" + UserType + "#User.Count'  group by statname,sr.servername,MonthlyOperatingCost  order by StatValue " + sortorder + ",sr.servername";
                        }
                        //Server Name filter is empty
                        else
                        {
                            SqlQuery = "select avg(StatValue) as StatValue,MonthlyOperatingCost,sr.servername,statname from [VSS_Statistics].dbo.MicrosoftSummaryStats ms inner join  [vitalsigns].dbo.servers sr on ms.ServerName=sr.ServerName inner join [vitalsigns].dbo.ServerTypes st  on st.ID=sr.ServerTypeID" +
                                " where  date>DATEADD(month, -1, GETDATE()) and statname like  '" + "AvgCAS@" + UserType + "#User.Count'  group by statname,sr.servername,MonthlyOperatingCost  order by StatValue " + sortorder + ",sr.servername";
                        }
                    }
                    //UserType  is empty or All
                    else
                    {
                        //Server Name filter is not empty
                        if (sname != "")
                        {

                            SqlQuery = "select avg(StatValue) as StatValue,MonthlyOperatingCost,sr.servername,statname from [VSS_Statistics].dbo.MicrosoftSummaryStats ms inner join  [vitalsigns].dbo.servers sr on ms.ServerName=sr.ServerName inner join [vitalsigns].dbo.ServerTypes st  on st.ID=sr.ServerTypeID" +
                                " where  date>DATEADD(month, -1, GETDATE()) and statname like  '" + "AvgCAS" + "%" + "' and  sr.servername Like '" + "%" + sname + "%" + "' group by statname,sr.servername,MonthlyOperatingCost  order by StatValue " + sortorder + ",sr.servername";
                        }
                        //Server Name filter is empty
                        else
                        {


                            SqlQuery = "select avg(StatValue) as StatValue,MonthlyOperatingCost,sr.servername,statname from [VSS_Statistics].dbo.MicrosoftSummaryStats ms inner join  [vitalsigns].dbo.servers sr on ms.ServerName=sr.ServerName inner join [vitalsigns].dbo.ServerTypes st  on st.ID=sr.ServerTypeID" +
                               " where  date>DATEADD(month, -1, GETDATE()) and statname like  '" + "AvgCAS" + "%" + "' and   sr.servername Like '" + "%" + sname + "%" + "' group by statname,sr.servername,MonthlyOperatingCost  order by StatValue " + sortorder + ",sr.servername";
                        }
                    }
                }
                statTab = objAdaptor.FetchData(SqlQuery);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return statTab;
        }
        // 3/18/2016 Durga Addded for VSPLUS-2696
        public DataTable GetServerTypeForCostperUserserved()
        {
            DataTable typetab = new DataTable();
            try
            {
             
                string SqlQuery = "Select * from ServerTypes where ID  in(1,5)" ;
             
                typetab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return typetab;
        }
        public DataTable GetCostperuserserveddataForDomino(string parm, string sname, string sType)
        {

            string sortorder = "";
            string SqlQuery = "";
            if (parm == "1")
            {
                //  sortorder = parm + "DESC";
              //  sortorder = " ASC";
                sortorder = " DESC";
            }
            else
            {
                sortorder = " ASC";
               
            }
            DataTable statTab = new DataTable();
            try
            {

                if (sType != "All" && sType != "")
                {


                    //Server Name filter is not empty
                    if (sname != "")
                    {

                        SqlQuery = "select avg(StatValue) as StatValue,MonthlyOperatingCost,sr.servername,statname from [VSS_Statistics].dbo.DominoSummaryStats Ds inner join  [vitalsigns].dbo.servers sr on Ds.ServerName=sr.ServerName inner join [vitalsigns].dbo.ServerTypes st  on st.ID=sr.ServerTypeID" +
                            " where   date>DATEADD(month, -1, GETDATE()) and sr.servername Like '" + "%" + sname + "%" + "' and statname like  '" + "Server.Users" + "' and st.ServerType='" + sType + "'  group by statname,sr.servername,MonthlyOperatingCost  order by StatValue " + sortorder + ",sr.servername";
                    }
                    //Server Name filter is empty
                    else
                    {
                        SqlQuery = "select avg(StatValue) as StatValue,MonthlyOperatingCost,sr.servername,statname from [VSS_Statistics].dbo.DominoSummaryStats Ds inner join  [vitalsigns].dbo.servers sr on Ds.ServerName=sr.ServerName inner join [vitalsigns].dbo.ServerTypes st  on st.ID=sr.ServerTypeID" +
                            " where  date>DATEADD(month, -1, GETDATE()) and  statname like  '" + "Server.Users" + "' and st.ServerType='" + sType + "'  group by statname,sr.servername,MonthlyOperatingCost  order by StatValue " + sortorder + ",sr.servername";
                    }

                }
                //Server Type is empty or All
                else
                {

                    //Server Name filter is not empty
                    if (sname != "")
                    {
                        // SqlQuery = "select Name as Server,ResponseTime from Status where ResponseTime>0 and Name Like '" +
                        //"%" + sname + "%" + "' and Location='" + UserType + "' order by " + sortorder;
                        SqlQuery = "select avg(StatValue) as StatValue,MonthlyOperatingCost,sr.servername,statname from [VSS_Statistics].dbo.DominoSummaryStats Ds inner join  [vitalsigns].dbo.servers sr on Ds.ServerName=sr.ServerName inner join [vitalsigns].dbo.ServerTypes st  on st.ID=sr.ServerTypeID" +
                            " where   date>DATEADD(month, -1, GETDATE()) and sr.servername Like '" + "%" + sname + "%" + "' and statname like  '" + "Server.Users" + "'  group by statname,sr.servername,MonthlyOperatingCost  order by StatValue " + sortorder + ",sr.servername";
                    }
                    //Server Name filter is empty
                    else
                    {
                        SqlQuery = "select avg(StatValue) as StatValue,MonthlyOperatingCost,sr.servername,statname from [VSS_Statistics].dbo.DominoSummaryStats Ds inner join  [vitalsigns].dbo.servers sr on Ds.ServerName=sr.ServerName inner join [vitalsigns].dbo.ServerTypes st  on st.ID=sr.ServerTypeID" +
                            " where  date>DATEADD(month, -1, GETDATE()) and statname like  '" + "Server.Users" + "'  group by statname,sr.servername,MonthlyOperatingCost  order by StatValue " + sortorder + ",sr.servername";
                    }

                }
                statTab = objAdaptor.FetchData(SqlQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return statTab;
        }
       //22/3/2016 Durga Added for VSPLUS 2695
        public DataTable GetCostPerUserServerGrid(string fromdate, string todate, string statname)

        {
            DataTable dt = new DataTable();
            string sqlQuery = "";
            try
            {



                //sqlQuery = "select  Ds.ServerName,Case when StatName='Server.Users' then 'User Count' else  StatName end as StatName,round(StatValue,0) as StatValue,Date,ISNULL(sr.MonthlyOperatingCost,0) as MonthlyOperatingCost,ISNULL((MonthlyOperatingCost/(SELECT [VSS_Statistics].dbo.[CalcNumDaysinMonth](Date))),0) as costperday,ISNULL((MonthlyOperatingCost/(SELECT [VSS_Statistics].dbo.[CalcNumDaysinMonth](Date))),0)/StatValue as CostPerUser  from [VSS_Statistics].dbo.DominoSummaryStats Ds inner join  [vitalsigns].dbo.servers sr on Ds.ServerName=sr.ServerName" +
                //" where statname ='Server.Users' and date BETWEEN '" + fromdate + "' AND '" + todate + "'";
                //4/14/2016 NS modified for VSPLUS-2864
                //The issue is due to an extremely large number being returned by the Exchange service for the StatValue column.
                //The value is large due to the Exchange server data corruption issue which is known.
                //.Net is unable to convert the value to an Integer value. We are going to change the query to return
                //only the values under a reasonable Integer maximum as a workaround.
                sqlQuery = "select  round(sum(StatValue),0) as StatValue,ServerName,Date " +
                    "from [VSS_Statistics].dbo.MicrosoftSummaryStats " +
                    "where statname IN('AvgCAS@OWAClient#User.Count','AvgCAS@RPCClient#User.Count') and " +
                    "StatValue < 200000 and " +
                    "date BETWEEN '" + fromdate + "' AND '" + todate + "' group by ServerName, Date order by ServerName ";  
                //The code below has been commented out - it will not work with the way the query is built
                //if (statname != "")
                //{
                //    sqlQuery += "AND StatName='" + statname + "' ";
                //}
              
                dt = objAdaptor.FetchData(sqlQuery);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable GetCostPerUserServersGridForDomino(string FromDate,string ToDate)

        {
            DataTable dt = new DataTable();
            string sqlquery = "";
            try
            {
                sqlquery = "select  Ds.ServerName,Case when StatName='Server.Users' then 'User Count' else  StatName end as StatName,round(StatValue,0) as StatValue,Date,ISNULL(sr.MonthlyOperatingCost,0) as MonthlyOperatingCost,ISNULL((MonthlyOperatingCost*12)/365,0) as costperday,ISNULL(((MonthlyOperatingCost*12)/(365*round(StatValue,0))),0) as CostPerUser  from [VSS_Statistics].dbo.DominoSummaryStats Ds inner join  [vitalsigns].dbo.servers sr on Ds.ServerName=sr.ServerName" +
                " where statname ='Server.Users' and date BETWEEN '" + FromDate + "' AND '" + ToDate + "'  order by ServerName";
                dt = objAdaptor.FetchData(sqlquery);
            }
            catch (Exception ex)
            {
                throw  ex;
            }
            return dt;

        }
        public DataTable GetCostPerdayforExchange(string ServerName, string date, int StatValue)
        {
            DataTable dt = new DataTable();
            string sqlQuery = "";
            try
            {

                sqlQuery = "select ISNULL(sr.MonthlyOperatingCost,0) as MonthlyOperatingCost,ISNULL((MonthlyOperatingCost*12)/365,0) as costperday, ISNULL(((MonthlyOperatingCost*12)/(365*" + StatValue + ")),0)  as CostPerUser  from [VSS_Statistics].dbo.MicrosoftSummaryStats MS inner join  [vitalsigns].dbo.servers sr on MS.ServerName=sr.ServerName  where sr.ServerName='" + ServerName + "' and statname='AvgCAS@RPCClient#User.Count'  and date='" + date + "'   order by sr.ServerName";
             

                dt = objAdaptor.FetchData(sqlQuery);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        //18-04-2016 Durga Modified for VSPLUS-2866
        //7-06-2016 Durga Modified for VSPLUS-3002
        public DataTable GetMonthlyExpenditureDetails(string GroupBy)
              {
            DataTable dt = new DataTable();
            string SqlQuery = "";
            try
            {
                if (GroupBy == "ServerType")
                {
                    SqlQuery = "select TOP 5 (Sum(MonthlyOperatingCost)*Count(ServerTypeID)) as MonthlyExpenditure,st.ServerType from servers sr  inner join ServerTypes st on sr.Servertypeid=st.id  group by st.ServerType "+
                    "union select  (Costperuser*ActiveUnits) as MonthlyExpenditure,st.ServerType from  " +

                " O365Server OS  inner join ServerTypes st  on os.ServerTypeId=st.id  inner join Office365AccountStats OA  on OA.ServerID=OS.ID group by st.ServerType,ActiveUnits,Costperuser";



                }
                else if (GroupBy == "Location")
                {
                    SqlQuery = "select TOP 5 (Sum(MonthlyOperatingCost)*Count(ServerTypeID)) as MonthlyExpenditure,Location from servers sr inner join Locations lc on sr.LocationID=lc.ID group by Location";
                }
                else 
                {
                    SqlQuery = "select TOP 5 (Sum(MonthlyOperatingCost)*Count(ServerTypeID)) as MonthlyExpenditure,Description from servers  group by Description";
                }


                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable GetMostUtilizedServers()
        {
            DataTable dt = new DataTable();
            string SqlQuery = "";
            try
            {
               SqlQuery="select avg(StatValue) as StatValue,ds.ServerName,sr.IdealUserCount  from [VSS_Statistics].dbo.DominoSummaryStats Ds inner join  [vitalsigns].dbo.servers sr on Ds.ServerName=sr.ServerName where statname ='Server.Users' and sr.IdealUserCount!=0    group by ds.ServerName,sr.IdealUserCount union "+


"select avg(StatValue) as StatValue,Ms.ServerName,sr.IdealUserCount  from [VSS_Statistics].dbo.MicrosoftSummaryStats Ms inner join  [vitalsigns].dbo.servers sr on Ms.ServerName=sr.ServerName where statname in('AvgCAS@ActiveSync#User.Count','AvgCAS@OWAClient#User.Count','AvgCAS@RPCClient#User.Count') and sr.IdealUserCount!=0    group by Ms.ServerName,sr.IdealUserCount ";

                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable GetCostPerUserServedDetails()
        {
              DataTable dt = new DataTable();
            string SqlQuery = "";
            try
            {
                SqlQuery = "select avg(StatValue) as StatValue,MonthlyOperatingCost,sr.servername,statname from [VSS_Statistics].dbo.DominoSummaryStats Ds inner join  [vitalsigns].dbo.servers sr on Ds.ServerName=sr.ServerName inner join [vitalsigns].dbo.ServerTypes st  on st.ID=sr.ServerTypeID" +
                          "  where  date>DATEADD(month, -1, GETDATE()) and statname like  'Server.Users'  group by statname,sr.servername,MonthlyOperatingCost  union" +
                          "  select avg(StatValue) as StatValue,MonthlyOperatingCost,sr.servername,statname from [VSS_Statistics].dbo.MicrosoftSummaryStats ms inner join  [vitalsigns].dbo.servers sr on ms.ServerName=sr.ServerName inner join [vitalsigns].dbo.ServerTypes st  on st.ID=sr.ServerTypeID" +
                               " where  date>DATEADD(month, -1, GETDATE()) and statname  in('AvgCAS@ActiveSync#User.Count','AvgCAS@OWAClient#User.Count','AvgCAS@RPCClient#User.Count') group by statname,sr.servername,MonthlyOperatingCost  ";

                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        //3-5-2016 Durga Added for VSPLUS-2883
        public DataTable GetServerTypeForDailyMailVolume()
        {
            DataTable typetab = new DataTable();
            try
            {

                string SqlQuery = "Select * from ServerTypes where ID  in(1,5,21)";

                typetab = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return typetab;
        }

        //6/3/2016 Sowjanya modified for VSPLUS-2999
        public DataTable GetCurrencySymbol(string CurrencySymbol)
        {
            DataTable dt = new DataTable();
            string SqlQuery = "";
            try
            {

                SqlQuery = "select svalue from settings where sname ='" + CurrencySymbol + "'";
                

                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }


        //5/26/2016 NS added for VSPLUS-2941
        public DataTable GetCPUMemoryHealth()
        {
            DataTable dt = new DataTable();
            try
            {
                /*
                  select a.ID,a.servername,a.servertype,a.memdisp,a.memory,a.Memory_Threshold,a.avgmemory,a.maxmemory,a.cpudisp,a.cpu,
a.CPU_Threshold,b.avgcpu,b.maxcpu from
(SELECT s.ID,s.ServerName, ServerType,CAST(Memory*100 as varchar(3)) + '/' + CAST(Memory_Threshold*100 as varchar(3)) MemDisp, 
                    Memory*100 Memory, Memory_Threshold*100 Memory_Threshold, ROUND(AVG(StatValue),1) AS AvgMemory,
                    MAX(StatValue) as MaxMemory,
                    CAST(CPU*100 as varchar(3)) + '/' + CAST(CPU_Threshold*100 as varchar(3)) CPUDisp, CPU*100 CPU,CPU_Threshold*100 CPU_Threshold 
                    FROM DominoServers ds 
                    INNER JOIN Servers s ON s.ID=ds.ServerID INNER JOIN ServerTypes st ON st.ID=s.ServerTypeID 
                    INNER JOIN Status st1 ON st1.Name=s.ServerName INNER JOIN VSS_Statistics.dbo.DominoDailyStats ds1 ON 
                    ds1.ServerName=s.ServerName
                    WHERE  dateadd(dd,0,datediff(dd,0,Date))=dateadd(dd,-1,datediff(dd,0,GETDATE()))
                    and (statname='Mem.PercentUsed' ) and st1.Type=st.ServerType
                    group by s.ID,s.ServerName,ServerType,CAST(Memory*100 as varchar(3)) + '/' + CAST(Memory_Threshold*100 as varchar(3)),Memory*100,
                    Memory_Threshold*100,CAST(CPU*100 as varchar(3)) + '/' + CAST(CPU_Threshold*100 as varchar(3)), CPU*100,
                    CPU_Threshold*100) a
                    join
                    (SELECT s.ID,s.ServerName, ROUND(AVG(StatValue),1) AS AvgCpu,MAX(StatValue) AS MaxCpu FROM VSS_Statistics.dbo.DominoDailyStats ds
                    inner join Servers s on s.ServerName=ds.ServerName inner join ServerTypes st on st.ID=s.ServerTypeID
                    WHERE  dateadd(dd,0,datediff(dd,0,Date))=dateadd(dd,-1,datediff(dd,0,GETDATE()))
                    and (statname='Platform.System.PctCombinedCpuUtil' ) 
                    group by s.ID,s.ServerName) b on a.servername=b.servername
union
select a.ID,a.servername,a.servertype,a.memdisp,a.memory,a.Memory_Threshold,a.avgmemory,a.maxmemory,a.cpudisp,a.cpu,a.CPU_Threshold,b.avgcpu,b.maxcpu
from
(SELECT s.ID,s.ServerName,ServerType,CAST(Memory*100 as varchar(3)) + '/' + CAST(MemThreshold as varchar(20)) MemDisp,
                    Memory*100 Memory, MemThreshold Memory_Threshold,ROUND(AVG(StatValue),1) AS AvgMemory,
                    MAX(StatValue) as MaxMemory,
                    CAST(CPU*100 as varchar(3)) + '/' + CAST(CPU_Threshold as varchar(20)) CPUDisp, CPU*100 CPU,CPU_Threshold FROM ServerAttributes sa 
                    INNER JOIN Servers s ON s.ID=sa.ServerID INNER JOIN ServerTypes st ON st.ID=s.ServerTypeID 
                    INNER JOIN Status st1 ON st1.Name=s.ServerName INNER JOIN VSS_Statistics.dbo.MicrosoftDailyStats ds1 ON 
                    ds1.ServerName=s.ServerName
                    WHERE  dateadd(dd,0,datediff(dd,0,Date))=dateadd(dd,-1,datediff(dd,0,GETDATE()))
                    and (statname='Mem.PercentUsed' ) and st1.Type=st.ServerType
                    group by s.ID,s.ServerName,ServerType,CAST(Memory*100 as varchar(3)) + '/' + CAST(MemThreshold as varchar(20)),
                    Memory*100, MemThreshold,CAST(CPU*100 as varchar(3)) + '/' + CAST(CPU_Threshold as varchar(20)), CPU*100,CPU_Threshold
                    ) a
                    join
                    (SELECT s.ID,s.ServerName, ROUND(AVG(StatValue),1) AS AvgCpu,MAX(StatValue) AS MaxCpu FROM VSS_Statistics.dbo.MicrosoftDailyStats ds
                    inner join Servers s on s.ServerName=ds.ServerName inner join ServerTypes st on st.ID=s.ServerTypeID
                    WHERE  dateadd(dd,0,datediff(dd,0,Date))=dateadd(dd,-1,datediff(dd,0,GETDATE()))
                    and (statname='Platform.System.PctCombinedCpuUtil' ) 
                    group by s.ID,s.ServerName) b on a.ServerName=b.ServerName
                 */
                /*
                string SqlQuery = "SELECT ServerName,ServerType,CAST(Memory*100 as varchar(3)) + '/' + CAST(Memory_Threshold*100 as varchar(3)) MemDisp, " +
                    "Memory*100 Memory, Memory_Threshold*100 Memory_Threshold, " +
                    "CAST(CPU*100 as varchar(3)) + '/' + CAST(CPU_Threshold*100 as varchar(3)) CPUDisp, CPU*100 CPU,CPU_Threshold*100 CPU_Threshold FROM DominoServers ds " +
                    "INNER JOIN Servers s ON s.ID=ds.ServerID INNER JOIN ServerTypes st ON st.ID=s.ServerTypeID " +
                    "INNER JOIN Status st1 ON st1.Name=s.ServerName " +
                    "WHERE st1.Type=st.ServerType " +
                    "UNION " + 
                    "SELECT ServerName,ServerType,CAST(Memory*100 as varchar(3)) + '/' + CAST(MemThreshold as varchar(20)) MemDisp, " +
                    "Memory*100 Memory, MemThreshold Memory_Threshold, " +
                    "CAST(CPU*100 as varchar(3)) + '/' + CAST(CPU_Threshold as varchar(20)) CPUDisp, CPU*100 CPU,CPU_Threshold FROM ServerAttributes sa " +
                    "INNER JOIN Servers s ON s.ID=sa.ServerID INNER JOIN ServerTypes st ON st.ID=s.ServerTypeID " +
                    "INNER JOIN Status st1 ON st1.Name=s.ServerName " +
                    "WHERE st1.Type=st.ServerType";
                 */
                string SqlQuery = "select a.ID,a.servername,a.servertype,a.memdisp,a.memory,a.Memory_Threshold,a.avgmemory,a.maxmemory,a.cpudisp,a.cpu, " +
                    "a.CPU_Threshold,b.avgcpu,b.maxcpu from " +
                    "(SELECT s.ID,s.ServerName, ServerType,CAST(Memory*100 as varchar(3)) + '/' + CAST(Memory_Threshold*100 as varchar(3)) MemDisp,  " +
                    "Memory*100 Memory, Memory_Threshold*100 Memory_Threshold, ROUND(AVG(StatValue),1) AS AvgMemory, " +
                    "MAX(StatValue) as MaxMemory, " +
                    "CAST(CPU*100 as varchar(3)) + '/' + CAST(CPU_Threshold*100 as varchar(3)) CPUDisp, CPU*100 CPU,CPU_Threshold*100 CPU_Threshold  " +
                    "FROM DominoServers ds  " +
                    "INNER JOIN Servers s ON s.ID=ds.ServerID INNER JOIN ServerTypes st ON st.ID=s.ServerTypeID  " +
                    "INNER JOIN Status st1 ON st1.Name=s.ServerName INNER JOIN VSS_Statistics.dbo.DominoDailyStats ds1 ON  " +
                    "ds1.ServerName=s.ServerName " +
                    "WHERE  dateadd(dd,0,datediff(dd,0,Date))=dateadd(dd,-1,datediff(dd,0,GETDATE())) " +
                    "and (statname='Mem.PercentUsed' ) and st1.Type=st.ServerType " +
                    "group by s.ID,s.ServerName,ServerType,CAST(Memory*100 as varchar(3)) + '/' + CAST(Memory_Threshold*100 as varchar(3)),Memory*100, " +
                    "Memory_Threshold*100,CAST(CPU*100 as varchar(3)) + '/' + CAST(CPU_Threshold*100 as varchar(3)), CPU*100, " +
                    "CPU_Threshold*100) a " +
                    "join " +
                    "(SELECT s.ID,s.ServerName, ROUND(AVG(StatValue),1) AS AvgCpu,MAX(StatValue) AS MaxCpu FROM VSS_Statistics.dbo.DominoDailyStats ds " +
                    "inner join Servers s on s.ServerName=ds.ServerName inner join ServerTypes st on st.ID=s.ServerTypeID " +
                    "WHERE  dateadd(dd,0,datediff(dd,0,Date))=dateadd(dd,-1,datediff(dd,0,GETDATE())) " +
                    "and (statname='Platform.System.PctCombinedCpuUtil' )  " +
                    "group by s.ID,s.ServerName) b on a.servername=b.servername " +
                    "union " +
                    "select a.ID,a.servername,a.servertype,a.memdisp,a.memory,a.Memory_Threshold,a.avgmemory,a.maxmemory,a.cpudisp,a.cpu,a.CPU_Threshold,b.avgcpu,b.maxcpu " +
                    "from " +
                    "(SELECT s.ID,s.ServerName,ServerType,CAST(Memory*100 as varchar(3)) + '/' + CAST(MemThreshold as varchar(20)) MemDisp, " +
                    "Memory*100 Memory, MemThreshold Memory_Threshold,ROUND(AVG(StatValue),1) AS AvgMemory, " +
                    "MAX(StatValue) as MaxMemory, " +
                    "CAST(CPU*100 as varchar(3)) + '/' + CAST(CPU_Threshold as varchar(20)) CPUDisp, CPU*100 CPU,CPU_Threshold FROM ServerAttributes sa  " +
                    "INNER JOIN Servers s ON s.ID=sa.ServerID INNER JOIN ServerTypes st ON st.ID=s.ServerTypeID  " +
                    "INNER JOIN Status st1 ON st1.Name=s.ServerName INNER JOIN VSS_Statistics.dbo.MicrosoftDailyStats ds1 ON  " +
                    "ds1.ServerName=s.ServerName " +
                    "WHERE  dateadd(dd,0,datediff(dd,0,Date))=dateadd(dd,-1,datediff(dd,0,GETDATE())) " +
                    "and (statname='Mem.PercentUsed' ) and st1.Type=st.ServerType " +
                    "group by s.ID,s.ServerName,ServerType,CAST(Memory*100 as varchar(3)) + '/' + CAST(MemThreshold as varchar(20)), " +
                    "Memory*100, MemThreshold,CAST(CPU*100 as varchar(3)) + '/' + CAST(CPU_Threshold as varchar(20)), CPU*100,CPU_Threshold " +
                    ") a " +
                    "join " +
                    "(SELECT s.ID,s.ServerName, ROUND(AVG(StatValue),1) AS AvgCpu,MAX(StatValue) AS MaxCpu FROM VSS_Statistics.dbo.MicrosoftDailyStats ds " +
                    "inner join Servers s on s.ServerName=ds.ServerName inner join ServerTypes st on st.ID=s.ServerTypeID " +
                    "WHERE  dateadd(dd,0,datediff(dd,0,Date))=dateadd(dd,-1,datediff(dd,0,GETDATE())) " +
                    "and (statname='Platform.System.PctCombinedCpuUtil' )  " +
                    "group by s.ID,s.ServerName) b on a.ServerName=b.ServerName";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

    }

}

