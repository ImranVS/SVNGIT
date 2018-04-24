using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace VSWebDAL.DashboardDAL
{
	public class MapDAL
	{

		/// <summary>
		/// Declarations
		/// </summary>
		private AdaptorforDsahBoard objAdaptor1 = new AdaptorforDsahBoard();
		private Adaptor objAdaptor = new Adaptor();
		private static MapDAL _self = new MapDAL();

		/// <summary>
		/// Used to call the functions using class name instead of object
		/// </summary>
		public static MapDAL Ins
		{
			get { return _self; }
		}

		public DataTable GetCityLocationInfo()
		{

			DataTable dt = new DataTable();
			try
			{

				//string SqlQuery = "select City + ', ' + Region + ', ' + Country as loc, avg(responseTime) as AvgRT, Count(City) as count," +
				//			"sum(Issues) as Issues, sum(InMaintenance) as InMaintenace, sum(NotResponding) as NotResponding, sum(NoIssues) as NoIssues, RealRegion as Region from mapstuff2 WHERE City IS NOT NULL and City <> '' Group By City, Region, Country, RealRegion";

				string SqlQuery = "select ((case when city is null then '' else (city + ',') end) + " +
					"(case when state is null then '' else (state + ',') end) +  " +
					"(case when country is null then '' else (country ) end) ) as loc, loc.Location, Issue, OK, NotResponding, Maintenance, AvgRT, " +
					"Issue + OK + NotResponding + Maintenance as count, loc.ID as locID " +
					"from Locations loc " +
					"inner join  " +
					"( " +
					"select Location,  " +
					"(Select count(*) from status sts1 where sts1.StatusCode='Issue' and sts2.Location=sts1.Location) as Issue,  " +
					"(Select count(*) from status sts1 where sts1.StatusCode='OK' and sts2.Location=sts1.Location) as OK,  " +
					"(Select count(*) from status sts1 where sts1.StatusCode='Not Responding' and sts2.Location=sts1.Location) as NotResponding, " +
					"(Select count(*) from status sts1 where sts1.StatusCode='Maintenance' and sts2.Location=sts1.Location) as Maintenance, " +
					"(Select avg(ResponseTime) from status sts1 where sts2.Location=sts1.Location) as AvgRT " +
					"from Status sts2 Group By Location  " +
					") as tbl on tbl.Location = loc.Location " +
					"WHERE loc.City IS NOT NULL and loc.City <> ''";
				
				dt = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return dt;
		}
		public DataTable GetStateLocationInfo()
		{

			DataTable dt = new DataTable();
			try
			{

				//string SqlQuery = "select Region + ', ' + Country as loc, avg(responseTime) as AvgRT, Count(Region) as count, " +
				//            "sum(Issues) as Issues, sum(InMaintenance) as InMaintenace, sum(NotResponding) as NotResponding, sum(NoIssues) as NoIssues, RealRegion as Region  from mapstuff2 where Region IS NOT NULL and Region <> '' Group By Region, Country, RealRegion";

				string SqlQuery = "select ((case when city is null then '' else (city + ',') end) + " +
					"(case when state is null then '' else (state + ',') end) +  " +
					"(case when country is null then '' else (country ) end) ) as loc, loc.Location, Issue, OK, NotResponding, Maintenance, AvgRT, " +
					"Issue + OK + NotResponding + Maintenance as count, loc.ID as locID " +
					"from Locations loc " +
					"inner join  " +
					"( " +
					"select Location,  " +
					"(Select count(*) from status sts1 where sts1.StatusCode='Issue' and sts2.Location=sts1.Location) as Issue,  " +
					"(Select count(*) from status sts1 where sts1.StatusCode='OK' and sts2.Location=sts1.Location) as OK,  " +
					"(Select count(*) from status sts1 where sts1.StatusCode='Not Responding' and sts2.Location=sts1.Location) as NotResponding, " +
					"(Select count(*) from status sts1 where sts1.StatusCode='Maintenance' and sts2.Location=sts1.Location) as Maintenance, " +
					"(Select avg(ResponseTime) from status sts1 where sts2.Location=sts1.Location) as AvgRT " +
					"from Status sts2 Group By Location  " +
					") as tbl on tbl.Location = loc.Location " +
					"WHERE loc.State IS NOT NULL and loc.State <> ''";
				
				dt = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return dt;
		}
		public DataTable GetCountryLocationInfo()
		{

			DataTable dt = new DataTable();
			try
			{

				//string SqlQuery = "select Country as loc, avg(responseTime) as AvgRT, Count(Country) as count, " +
				//            "sum(Issues) as Issues, sum(InMaintenance) as InMaintenace, sum(NotResponding) as NotResponding, sum(NoIssues) as NoIssues, RealRegion as Region  from mapstuff2 where Country IS NOT NULL and Country <> '' Group By Country, RealRegion";

				string SqlQuery = "select ((case when city is null then '' else (city + ',') end) + " +
					"(case when state is null then '' else (state + ',') end) +  " +
					"(case when country is null then '' else (country ) end) ) as loc, loc.Location, Issue, OK, NotResponding, Maintenance, AvgRT, " +
					"Issue + OK + NotResponding + Maintenance as count, loc.ID as locID " +
					"from Locations loc " +
					"inner join  " +
					"( " +
					"select Location,  " +
					"(Select count(*) from status sts1 where sts1.StatusCode='Issue' and sts2.Location=sts1.Location) as Issue,  " +
					"(Select count(*) from status sts1 where sts1.StatusCode='OK' and sts2.Location=sts1.Location) as OK,  " +
					"(Select count(*) from status sts1 where sts1.StatusCode='Not Responding' and sts2.Location=sts1.Location) as NotResponding, " +
					"(Select count(*) from status sts1 where sts1.StatusCode='Maintenance' and sts2.Location=sts1.Location) as Maintenance, " +
					"(Select avg(ResponseTime) from status sts1 where sts2.Location=sts1.Location) as AvgRT " +
					"from Status sts2 Group By Location  " +
					") as tbl on tbl.Location = loc.Location " +
					"WHERE loc.Country IS NOT NULL and loc.Country <> ''";
				
				dt = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return dt;
		}

		public DataTable GetLocationInfoO365(String statName)
		{
			DataTable dt = new DataTable();
			try
			{

				string SqlQuery = @"
DECLARE @cols AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX);

SET @cols = STUFF((
select distinct ',' + QUOTENAME(SUBSTRING(mds.StatName, 0, CHARINDEX('@', mds.StatName))) from nodes n
inner join VSS_Statistics.dbo.MicrosoftDailyStats mds on mds.StatName like ('%@' + n.Name) where n.Alive=1
FOR XML PATH(''), TYPE
).value('.', 'NVARCHAR(MAX)') 
,1,1,'')



set @query = '

CREATE TABLE #TempTable ( 
	NodeName varchar(255),
	loc varchar(255),
	LocID int,
	Location varchar(255),
	Alive int,
	' + REPLACE(@cols, ',', ' varchar(255),') + ' varchar(255)
	)

INSERT INTO #TempTable 
select NodeName, 
((case when city is null then '''' else (city + '','') end) +
(case when state is null then '''' else (state + '','') end) + 
(case when country is null then '''' else (country ) end) ) as WrttenLocation, loc.id as LocID, loc.Location, Alive, 
' + @cols + '
 from
(

select tbl2.*, Nodes.LocationID, Nodes.Alive
from (
select mds.StatValue, SUBSTRING(mds.StatName, CHARINDEX(''@'', mds.StatName) + 1, LEN(mds.StatName)) NodeName, 
SUBSTRING(mds.StatName, 0, CHARINDEX(''@'', mds.StatName)) StatName 
from (select distinct StatName, max(date) MostRecent From VSS_Statistics.dbo.MicrosoftDailyStats where ServerTypeId=21 group by StatName) tbl1
inner join VSS_Statistics.dbo.MicrosoftDailyStats mds on mds.StatName=tbl1.StatName and mds.Date = tbl1.MostRecent
) tbl2 
inner join Nodes on Nodes.Name=tbl2.NodeName 
inner join Locations loc on loc.ID=Nodes.LocationID

) x
pivot
(
	max(StatValue) for StatName in (' + @cols + ')

) p
inner join Locations loc on loc.ID=LocationID

select * from #TempTable where Alive=1 and loc <> ''''

'


execute(@query)";
				dt = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return dt;
		}

		public DataTable GetUniqueOffice365Tests()
		{
			DataTable dt = new DataTable();
			try
			{
                //4/4/2016 NS modified for VSPLUS-2666
                string SqlQuery = "select distinct SUBSTRING(mds.StatName, 0, CHARINDEX('@', mds.StatName)) tests from nodes n " +
                    "inner join VSS_Statistics.dbo.MicrosoftDailyStats mds on mds.StatName like ('%@' + n.Name) where n.Alive=1 " +
                    "order by SUBSTRING(mds.StatName, 0, CHARINDEX('@', mds.StatName)) ";
				dt = objAdaptor.FetchData(SqlQuery);

			}
			catch
			{
			}
			finally
			{
			}
			return dt;

		}

		public DataTable GetThresholds()
		{
			DataTable dt = new DataTable();
			try
			{
				string SqlQuery = @"



					DECLARE @cols AS NVARCHAR(MAX);
					DECLARE @query AS NVARCHAR(MAX);

					set @cols = STUFF((SELECT distinct ',' +
											QUOTENAME(tests)
										  FROM Office365Tests
										  FOR XML PATH(''), TYPE
										 ).value('.', 'NVARCHAR(MAX)') 
											, 1, 1, '');

					set @query = 'select * from 
					(select ServerId, ResponseThreshold, Tests from Office365Tests where serverid=(select min(serverid) from office365tests)) t
					 PIVOT 
					 (
					 max(ResponseThreshold) for
					  Tests in (' + @cols + '
					  ) ) as tbl'
  
					  execute(@query)
				";

				dt = objAdaptor.FetchData(SqlQuery);
			}
			catch
			{
			}
			return dt;
		}

	}
}
