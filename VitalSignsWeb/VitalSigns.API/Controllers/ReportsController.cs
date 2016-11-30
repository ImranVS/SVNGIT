using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VitalSigns.API.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using VitalSigns.API.Models.Charts;
using VSNext.Mongo.Repository;
using VSNext.Mongo.Entities;
using System.Linq.Expressions;
using MongoDB.Bson;
using System.Globalization;
using System.Dynamic;

namespace VitalSigns.API.Controllers
{
    [Route("[controller]")]
    public class ReportsController : BaseController
    {
        private IRepository<ConsoleCommands> consoleCommandsRepository;
        private IRepository<LogFile> logFileRepository;
        private IRepository<SummaryStatistics> summaryRepository;
        private IRepository<Database> databaseRepository;
        private IRepository<Server> serverRepository;
        private IRepository<Status> statusRepository;
        private IRepository<DailyStatistics> dailyRepository;

        //private string DateFormat = "yyyy-MM-dd";
        private string DateFormat = "yyyy-MM-ddTHH:mm:ss.fffK";
        private IRepository<DominoServerTasks> doimoServerTasksRepository;
        private IRepository<IbmConnectionsObjects> connectionsRepository;
        private IRepository<Location> locationRepository;
        //private string DateFormatMonthYear = "yyyy-MM";


        [HttpGet("disk_availability_trend")]
        public APIResponse GetDiskAvailabilityTrend(string deviceId = "", int year = -1, bool isChart = true)
        {
            FilterDefinition<SummaryStatistics> filterDef = null;
            DateTime dtStart = DateTime.Parse("01/01/" + year.ToString());
            year += 1;
            DateTime dtEnd = DateTime.Parse("01/01/" + year.ToString());

            dtStart = DateTime.SpecifyKind(dtStart, DateTimeKind.Utc);
            dtEnd = DateTime.SpecifyKind(dtEnd, DateTimeKind.Utc);

            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            summaryRepository = new Repository<SummaryStatistics>(ConnectionString);
            Repository<Server> serverRepository = new Repository<Server>(ConnectionString);
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    filterDef = summaryRepository.Filter.And(summaryRepository.Filter.Gte(p => p.StatDate, dtStart),
                    summaryRepository.Filter.Lt(p => p.StatDate, dtEnd),
                    summaryRepository.Filter.Ne(p => p.DeviceName, null),
                    summaryRepository.Filter.Regex(p => p.StatName, new BsonRegularExpression("/Disk.*Free/i")));
                }
                else
                {
                    filterDef = summaryRepository.Filter.And(summaryRepository.Filter.Gte(p => p.StatDate, dtStart),
                    summaryRepository.Filter.Lt(p => p.StatDate, dtEnd),
                    summaryRepository.Filter.Ne(p => p.DeviceName, null),
                    summaryRepository.Filter.Eq(p => p.DeviceId, deviceId),
                    summaryRepository.Filter.Regex(p => p.StatName, new BsonRegularExpression("/Disk.*Free/i")));
                }
                var summarylist = summaryRepository.Find(filterDef).OrderBy(p => p.StatDate).ToList();

                var result = summarylist
                    .GroupBy(row => new
                    {
                        row.StatDate.Value.Year,
                        row.StatDate.Value.Month,
                        row.DeviceId,
                        row.StatName,
                        row.DeviceName
                    })
                    .Select(row => new
                    {
                        Date = mfi.GetMonthName(row.Key.Month).ToString(),
                        Value = Math.Round(row.Average(x => x.StatValue) / 1024 / 1024 / 1024, 2),
                        StatName = row.Key.StatName,
                        DiskName = row.Key.DeviceName + " - " + row.Key.StatName
                    }).ToList();
                List<Serie> series = new List<Serie>();
                foreach (var disk in result.Select(i => i.DiskName).Distinct())
                {
                    Serie serie = new Serie();
                    var output = result.Where(x => x.DiskName == disk).ToList();
                    var segments = new List<Segment>();
                    foreach (var item in output)
                    {
                        segments.Add(new Segment()
                        {
                            Label = item.Date,
                            Value = item.Value
                        });

                        serie.Title = item.DiskName;
                        serie.Segments = segments;
                    }
                    series.Add(serie);
                }
                    

                Chart chart = new Chart();
                chart.Title = "";
                chart.Series = series;
                Response = Common.CreateResponse(chart);

                return Response;

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("server_utilization")]
        public APIResponse GetServerUtilization(string deviceId = "", string statName = "")
        {
            FilterDefinition<SummaryStatistics> filterDef = null;
           // string startDate = DateTime.UtcNow.AddDays(-30).ToString(DateFormat);

            DateTime dtStart = DateTime.UtcNow.AddDays(-30).ToUniversalTime();

            //dtStart = DateTime.SpecifyKind(dtStart, DateTimeKind.Utc);

            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            summaryRepository = new Repository<SummaryStatistics>(ConnectionString);
            Repository<Server> serverRepository = new Repository<Server>(ConnectionString);
            List<Server> serverlist = null;
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    filterDef = summaryRepository.Filter.And(summaryRepository.Filter.Gte(p => p.StatDate, dtStart),
                    summaryRepository.Filter.Ne(p => p.DeviceName, null),
                    summaryRepository.Filter.Eq(p => p.StatName, statName));
                }
                else
                {
                    filterDef = summaryRepository.Filter.And(summaryRepository.Filter.Gte(p => p.StatDate, dtStart),
                    summaryRepository.Filter.Ne(p => p.DeviceName, null),
                    summaryRepository.Filter.Eq(p => p.DeviceId, deviceId),
                    summaryRepository.Filter.Eq(p => p.StatName, statName));
                }
                var summarylist = summaryRepository.Find(filterDef).OrderBy(p => p.StatDate).ToList();
                serverlist = serverRepository.Collection.Aggregate().Match(_ => true).ToList();
                var result = summarylist
                    .GroupBy(row => new
                    {
                        row.DeviceId,
                        row.StatName,
                        row.DeviceName
                    })
                    .Select(row => new
                    {
                        DeviceId = row.Key.DeviceId,
                        Value = Math.Round(row.Average(x => x.StatValue), 2),
                        StatName = row.Key.StatName,
                        DeviceName = row.Key.DeviceName
                    }).ToList();
                List<Serie> series = new List<Serie>();
                var segments = new List<Segment>();
                Serie serie = new Serie();

                foreach (var deviceid in result.Select(i => i.DeviceId).Distinct())
                {
                    var output = result.Where(x => x.DeviceId == deviceid).ToList();
                    var server = serverlist.Where(x => x.Id == deviceid).ToList();
                    foreach (var item in output)
                    {
                        if (item.DeviceId == deviceid)
                        {
                            if (server[0].IdealUserCount == null || server[0].IdealUserCount == 0)
                            {
                                segments.Add(new Segment()
                                {
                                    Label = item.DeviceName,
                                    Value = 0
                                });
                            }
                            else
                            {
                                var usercount = Convert.ToInt32(server[0].IdealUserCount);
                                segments.Add(new Segment()
                                {
                                    Label = item.DeviceName,
                                    Value = Math.Round(((item.Value / usercount) * 100), 2)
                                });
                            }
                        }
                        
                    }
                    
                }
                serie.Title = "";
                serie.Segments = segments;
                series.Add(serie);

                Chart chart = new Chart();
                chart.Title = "";
                chart.Series = series;
                Response = Common.CreateResponse(chart);

                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }


        [HttpGet("summarystats_aggregation")]
        public APIResponse GetSummaryStatAggregation(string type, string statName, string aggregationType, string startDate = "", string endDate = "")
        {

            try
            {

                if (startDate == "")
                    startDate = DateTime.UtcNow.AddDays(-7).ToString(DateFormat);

                if (endDate == "")
                    endDate = DateTime.UtcNow.ToString(DateFormat);
                
                DateTime dtStart = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture).ToUniversalTime();
                DateTime dtEnd = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture).AddDays(1).ToUniversalTime();

                summaryRepository = new Repository<SummaryStatistics>(ConnectionString);

                var filterDef = summaryRepository.Filter.Eq(x => x.DeviceType, type) &
                    summaryRepository.Filter.Eq(x => x.StatName, statName) &
                    summaryRepository.Filter.Gte(p => p.StatDate, dtStart) &
                    summaryRepository.Filter.Lte(p => p.StatDate, dtEnd);

                var results = summaryRepository.Find(filterDef).ToList();

                double aggregatedValue = 0;
                string aggregationDisplay = "";

                switch (aggregationType.ToLower())
                {
                    case "sum":
                        aggregatedValue = results.Sum(x => x.StatValue);
                        aggregationDisplay = "Total";
                        break;
                    case "avg":
                        aggregatedValue = results.Average(x => x.StatValue);
                        aggregationDisplay = "Average";
                        break;
                    case "max":
                        aggregatedValue = results.Max(x => x.StatValue);
                        aggregationDisplay = "Max";
                        break;
                    case "min":
                        aggregatedValue = results.Min(x => x.StatValue);
                        aggregationDisplay = "Min";
                        break;
                    default:
                        throw new Exception("No matching aggregation type.");
                }

                
                var expandoObj = new ExpandoObject() as IDictionary<string, Object>;
                if (results.Count > 0)
                {
                    expandoObj.Add("Device Name", results[0].DeviceName);
                    foreach (var entity in results)
                    {
                        //expandoObj.Add(entity.CreatedOn.ToString("MM/dd/yyyy"), entity.StatValue);
                        expandoObj.Add(entity.StatDate.Value.ToString(DateFormat), entity.StatValue);
                    }
                    expandoObj.Add(aggregationDisplay, aggregatedValue);
                }

                var list = new List<IDictionary<string,object>>();
                list.Add(expandoObj);

                Response = Common.CreateResponse(list);
                return Response;

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }


        }

        [HttpGet("summarystats_chart")]
        public APIResponse GetSumamryStatsChart(string statName, string deviceId = "", string startDate = "", string endDate = "", string type = "")
        {
            FilterDefinition<SummaryStatistics> filterDef = null;
            if (startDate == "")
                startDate = DateTime.UtcNow.AddDays(-7).ToString(DateFormat);

            if (endDate == "")
                endDate = DateTime.UtcNow.ToString(DateFormat);

            //1 day is added to the end so we include that days data
            DateTime dtStart = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture).ToUniversalTime();
            DateTime dtEnd = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture).AddDays(1).ToUniversalTime();

            summaryRepository = new Repository<SummaryStatistics>(ConnectionString);

            List<String> listOfDevices;
            List<String> listOfTypes;

            if (string.IsNullOrWhiteSpace(deviceId))
            {
                listOfDevices = new List<string>();
            }
            else
            {
                listOfDevices = deviceId.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',').ToList();
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                listOfTypes = new List<string>();
            }
            else
            {
                listOfTypes = type.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',').ToList();
            }

            try
            {
                filterDef = summaryRepository.Filter.Gte(p => p.StatDate, dtStart) &
                    summaryRepository.Filter.Lte(p => p.StatDate, dtEnd) &
                    summaryRepository.Filter.Eq(p => p.StatName, statName) &
                    summaryRepository.Filter.Ne(p => p.DeviceName, null);

                if (listOfDevices.Count > 0)
                {
                    filterDef = filterDef & summaryRepository.Filter.In(p => p.DeviceId, listOfDevices);
                }
                else if (listOfTypes.Count > 0)
                {
                    filterDef = filterDef & summaryRepository.Filter.In(p => p.DeviceType, listOfTypes);
                }
                var result = summaryRepository.Find(filterDef).OrderBy(p => p.StatDate).ToList();

                List<Serie> series = new List<Serie>();

                foreach (var currDeviceId in result.Select(x => x.DeviceId).Distinct())
                {
                    var currList = result.Where(x => x.DeviceId == currDeviceId).ToList();
                    Serie serie = new Serie();
                    serie.Segments = new List<Segment>();
                    serie.Title = currList[0].DeviceName;

                    for (DateTime date = dtStart; date < dtEnd; date = date.AddDays(1))
                    {
                        var item = currList.Where(x => x.StatDate.Value.Date == date.Date).ToList();

                        serie.Segments.Add(new Segment() { Label = date.ToString(DateFormat), Value = item.Count > 0 ? (double?)item[0].StatValue : null });
                    }
                    series.Add(serie);
                }
               
                Chart chart = new Chart();
                chart.Title = statName;
                chart.Series = series;
                Response = Common.CreateResponse(chart);

                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("dailystats_hourly_chart")]
        public APIResponse GetDailyStatsChart(string statName, string date, string deviceId = "", string type = "")
        {
            FilterDefinition<DailyStatistics> filterDef = null;

            //1 day is added to the end so we include that days data
            DateTime dtDate = DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture).ToUniversalTime(); 

            //dtDate = DateTime.SpecifyKind(dtDate, DateTimeKind.Utc);

            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            dailyRepository = new Repository<DailyStatistics>(ConnectionString);

            List<String> listOfDevices;
            List<String> listOfTypes;

            if (string.IsNullOrWhiteSpace(deviceId))
            {
                listOfDevices = new List<string>();
            }
            else
            {
                listOfDevices = deviceId.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',').ToList();
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                listOfTypes = new List<string>();
            }
            else
            {
                listOfTypes = type.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',').ToList();
            }

            try
            {
                filterDef = dailyRepository.Filter.Gte(p => p.CreatedOn, dtDate) &
                    dailyRepository.Filter.Lt(p => p.CreatedOn, dtDate.AddDays(1)) &
                    dailyRepository.Filter.Eq(p => p.StatName, statName);

                if (listOfDevices.Count > 0)
                {
                    filterDef = filterDef & dailyRepository.Filter.In(p => p.DeviceId, listOfDevices);
                }
                else if (listOfTypes.Count > 0)
                {
                    filterDef = filterDef & dailyRepository.Filter.In(p => p.DeviceType, listOfTypes);
                }
                //var result = dailyRepository.Find(filterDef).OrderBy(p => p.CreatedOn).ToList();
                var result = dailyRepository.Collection.Aggregate().Match(filterDef)
                    .Group(x => new
                    {
                        Hour = x.CreatedOn.Hour,
                        //Date = x.CreatedOn.Date,
                        DeviceId = x.DeviceId,
                        DeviceName = x.DeviceName
                    }, y => new
                    {
                        Key = y.Key,
                        Average = y.Average(z => z.StatValue)
                    })
                    .Project(y => new
                    {
                        Hour = y.Key.Hour,

                        //DateHour = 
                        StatValue = y.Average,
                        DeviceName = y.Key.DeviceName,
                        DeviceId = y.Key.DeviceId
                    }).ToList();

                List<Serie> series = new List<Serie>();

                foreach (var currDeviceId in result.Select(x => x.DeviceId).Distinct())
                {
                    var currList = result.Where(x => x.DeviceId == currDeviceId).ToList();
                    Serie serie = new Serie();
                    serie.Segments = new List<Segment>();
                    serie.Title = currList[0].DeviceName;

                    for(var tempDate = dtDate; tempDate < dtDate.AddDays(1); tempDate = tempDate.AddHours(1))
                    {
                        var item = currList.Where(x => x.Hour == tempDate.Hour).ToList();

                        serie.Segments.Add(new Segment() { Label = tempDate.ToString(DateFormat), Value = item.Count > 0 ? (double?)item[0].StatValue : null });

                    }

                    //for (var hour = 0; hour < 24; hour++)
                    //{
                    //    var item = currList.Where(x => x.Hour == hour).ToList();

                    //    serie.Segments.Add(new Segment() { Label = dtDate.AddHours( (hour - dtDate.Hour) > 0 ? (hour - dtDate.Hour) : (hour - dtDate.Hour + 24) ).ToString(DateFormat), Value = item.Count > 0 ? (double?)item[0].StatValue : null });
                    //}


                    series.Add(serie);
                }

                Chart chart = new Chart();
                chart.Title = statName;
                chart.Series = series;
                Response = Common.CreateResponse(chart);

                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("server_availability")]
        public APIResponse GetMonthlyServerDownTime(string statName, string deviceId = "", string month = "", string type = "", string minValue = "0", string reportType = "minutes")
         {
            try
            {
                //string statName = "HourlyDownTimeMinutes";
                //string statName = "DeviceUpTimeStats";
                if (month == "")
                    month = DateTime.UtcNow.Date.ToString(DateFormat);

                DateTime dtStart = DateTime.ParseExact(month, DateFormat, CultureInfo.InvariantCulture).ToUniversalTime();
                DateTime dtEnd = dtStart.AddMonths(1).AddDays(-1).ToUniversalTime();

                Chart chart = ((Chart)(GetSumamryStatsChart(statName, deviceId: deviceId, startDate: dtStart.ToString(DateFormat), endDate: dtEnd.ToString(DateFormat), type: type).Data));
                List<Serie> series = chart.Series.ToList();
                List<Segment> segments = new List<Segment>();
                foreach (Serie currSerie in series)
                {
                    if (reportType == "minutes")
                    {
                        segments.Add(new Segment() { Label = currSerie.Title, Value = currSerie.Segments.Where(x => x.Value >= Convert.ToInt32(minValue)).Sum(x => x.Value) });
                    }
                    else if (reportType == "percent")
                    {
                        int minsInMonth;
                        if (dtStart.Year == DateTime.Now.Year && dtStart.Month == DateTime.Now.Month)
                        {
                            minsInMonth = (int)((DateTime.UtcNow - new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1)).TotalMinutes);
                        }
                        else
                        {
                            minsInMonth = (int)((dtStart - dtStart.AddMonths(1)).TotalMinutes);
                        }
                        segments.Add(new Segment() { Label = currSerie.Title, Value = (int)(currSerie.Segments.Where(x => x.Value >= Convert.ToInt32(minValue)).Sum(x => x.Value)/minsInMonth) });
                    }

                }

                Serie serie = new Serie();
                serie.Title = "";
                serie.Segments = segments;
                chart.Series = new List<Serie>() { serie };

                return Common.CreateResponse(chart);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
            
        }

        [HttpGet("cost_per_user")]
        public APIResponse GetCostPerUser(string deviceId = "", string statName = "", bool isChart = true)
        {
            List<Server> serverlist = null;
            List<dynamic> result = new List<dynamic>();
            FilterDefinition<SummaryStatistics> filterDef = null;

            //string startDate = DateTime.Now.AddDays(-30).ToString(DateFormat);
            DateTime dtStart = DateTime.UtcNow.AddDays(-30).ToUniversalTime();
            //dtStart = DateTime.SpecifyKind(dtStart, DateTimeKind.Utc);

            summaryRepository = new Repository<SummaryStatistics>(ConnectionString);
            Repository<Server> serverRepository = new Repository<Server>(ConnectionString);

            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    filterDef = summaryRepository.Filter.And(summaryRepository.Filter.Gte(p => p.StatDate, dtStart),
                    summaryRepository.Filter.Ne(p => p.DeviceName, null),
                    summaryRepository.Filter.Eq(p => p.StatName, statName));
                }
                else
                {
                    filterDef = summaryRepository.Filter.And(summaryRepository.Filter.Gte(p => p.StatDate, dtStart),
                    summaryRepository.Filter.Ne(p => p.DeviceName, null),
                    summaryRepository.Filter.Eq(p => p.DeviceId, deviceId),
                    summaryRepository.Filter.Eq(p => p.StatName, statName));
                }

                if (isChart)
                {
                    var summarylist = summaryRepository.Find(filterDef).OrderBy(p => p.StatDate).ToList();
                    serverlist = serverRepository.Collection.Aggregate().Match(_ => true).ToList();
                    var result1 = summarylist
                        .GroupBy(row => new
                        {
                            row.DeviceId,
                            row.StatName,
                            row.DeviceName
                        })
                        .Select(row => new
                        {
                            DeviceId = row.Key.DeviceId,
                            Value = Math.Round(row.Average(x => x.StatValue), 2),
                            StatName = row.Key.StatName,
                            DeviceName = row.Key.DeviceName
                        }).ToList();
                    List<Serie> series = new List<Serie>();
                    var segments = new List<Segment>();
                    Serie serie = new Serie();

                    foreach (var deviceid in result1.Select(i => i.DeviceId).Distinct())
                    {
                        var output = result1.Where(x => x.DeviceId == deviceid).ToList();
                        var server = serverlist.Where(x => x.Id == deviceid).ToList();
                        foreach (var item in output)
                        {
                            if (item.DeviceId == deviceid)
                            {
                                if (server[0].MonthlyOperatingCost == null || server[0].MonthlyOperatingCost == 0 || item.Value == 0)
                                {
                                    segments.Add(new Segment()
                                    {
                                        Label = item.DeviceName,
                                        Value = 0
                                    });
                                }
                                else
                                {
                                    var monthlycost = Convert.ToInt32(server[0].MonthlyOperatingCost);
                                    segments.Add(new Segment()
                                    {
                                        Label = item.DeviceName,
                                        Value = Math.Round(((monthlycost / item.Value) * 100), 2)
                                    });
                                }
                            }
                        }

                    }
                    serie.Title = "";
                    serie.Segments = segments;
                    series.Add(serie);

                    Chart chart = new Chart();
                    chart.Title = "";
                    chart.Series = series;
                    Response = Common.CreateResponse(chart);
                }
                else
                {
                    var summarytemp = summaryRepository.Find(filterDef).OrderBy(p => p.DeviceName).ToList();
                    var summarylist = summarytemp.GroupBy(row => new
                    {
                        row.DeviceId,
                        row.DeviceName,
                        row.StatDate.Value.Date
                    })
                    .Select(grp => new
                    {
                        DeviceId = grp.Key.DeviceId,
                        DeviceName = grp.Key.DeviceName,
                        Label = grp.Key.DeviceName,
                        StatValue = grp.Average(x => x.StatValue),
                        StatDate = grp.Key.Date
                    }).ToList(); ;
                    serverlist = serverRepository.Collection.Aggregate().Match(_ => true).ToList();
                    foreach (var stats in summarylist)
                    {
                        var x = new ExpandoObject() as IDictionary<string, Object>;
                        x.Add("device_name", stats.DeviceName);
                        x.Add("user_count", stats.StatValue);
                        x.Add("date", stats.StatDate);
                        var server = serverlist.Where(p => p.Id == stats.DeviceId).ToList();
                        var bson2 = server[0].ToBsonDocument();
                        var fieldvalue = bson2["monthly_operating_cost"].ToDouble();
                        if (stats.StatValue != 0)
                        {
                            x.Add("cost_per_user", Math.Round((fieldvalue * 12) / (365 * Math.Round(stats.StatValue, 0)), 2));
                        }
                        else
                        {
                            x.Add("cost_per_user", 0);
                        }
                        x.Add("cost_per_day", Math.Round((fieldvalue * 12) / 365, 2));
                        x.Add("monthly_operating_cost", fieldvalue);
                        result.Add(x);
                    }
                    Response = Common.CreateResponse(result);
                }
                return Response;
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("traveler_stats")]
        public APIResponse GetTravelerStats(string deviceId, string paramtype, string paramvalue)
        {
            FilterDefinition<Server> filterDef = null;
            Repository<TravelerStats> travelerRepository = new Repository<TravelerStats>(ConnectionString);
            List<Serie> series = new List<Serie>();
            string travelername = "";
            bool foundInt = false;

            try
            {
                serverRepository = new Repository<Server>(ConnectionString);
                filterDef = serverRepository.Filter.Eq(x => x.Id, deviceId);
                var serverlist = serverRepository.Find(filterDef).ToList();
                if (serverlist.Count > 0)
                {
                    travelername = serverlist[0].DeviceName;
                }
                var builder = Builders<TravelerStats>.Filter;
                var result = travelerRepository.Collection.Aggregate()
                               .Match(builder.And(builder.Eq(paramtype, paramvalue), builder.Eq(x => x.TravelerServerName, travelername))).ToList();
                
                if (paramtype == "interval")
                {
                    foreach (var mailserver in result.Select(x => x.MailServerName).Distinct()) 
                    {
                        foundInt = false;
                        List<Segment> segments = new List<Segment>();
                        Serie serie = new Serie();
                        foreach (var record in result)
                        {
                            if (record.MailServerName == mailserver)
                            {
                                foundInt = true;
                                segments.Add(new Segment { Label = record.CreatedOn.ToString(DateFormat), Value = Convert.ToDouble(record.OpenTimes) });
                                serie.Title = mailserver;
                            }
                        }
                        if (!foundInt)
                        {
                            for (int i = 0; i < segments.Count; i++)
                            {
                                segments.Add(new Segment { Label = segments[i].Label, Value = 0 });
                                serie.Title = mailserver;
                            }
                        }
                        serie.Segments = segments;
                        series.Add(serie);
                    }
                }
                else
                {
                    foreach (var interval in result.Select(x => x.Interval).Distinct())
                    {
                        foundInt = false;
                        List<Segment> segments = new List<Segment>();
                        Serie serie = new Serie();
                        foreach (var record in result)
                        {
                            if (record.Interval == interval)
                            {
                                foundInt = true;
                                segments.Add(new Segment { Label = record.CreatedOn.TimeOfDay.ToString(), Value = Convert.ToDouble(record.OpenTimes) });
                                serie.Title = interval;
                            }
                        }
                        if (!foundInt)
                        {
                            for (int i=0; i < segments.Count; i++)
                            {
                                segments.Add(new Segment { Label = segments[i].Label, Value = 0 });
                                serie.Title = interval;
                            }
                        }
                        serie.Segments = segments;
                        series.Add(serie);
                    }
                }
                
                Chart chart = new Chart();
                chart.Title = "";
                chart.Series = series;
                Response = Common.CreateResponse(chart);
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("domino_summarystats_chart")]
        public APIResponse GetDominoSumamryStatsChart(string statName, string deviceId = "", string startDate = "", string endDate = "")
        {
            try
            {

                if (deviceId == "")
                {
                    VSNext.Mongo.Repository.Repository<Server> repo = new VSNext.Mongo.Repository.Repository<Server>(ConnectionString);
                    FilterDefinition<VSNext.Mongo.Entities.Server> filterdef = repo.Filter.Where(i => i.DeviceType == "Domino");
                    ProjectionDefinition<VSNext.Mongo.Entities.Server> projectDef = repo.Project.Include(i => i.Id);
                    List<Server> serverList = repo.Find(filterdef, projectDef).ToList();
                    foreach (Server s in serverList)
                    {
                        if (deviceId == "")
                            deviceId = s.Id;
                        else
                            deviceId += "," + s.Id;
                    }
                }
                return GetSumamryStatsChart(statName, deviceId, startDate, endDate);


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("console_command_list")]
        public APIResponse GetAllConsoleCommands()
        {
            consoleCommandsRepository = new Repository<ConsoleCommands>(ConnectionString);
            List<ConsoleCommandList> result = null;

            result = consoleCommandsRepository.Collection
                             .AsQueryable()
                             .Select(x => new ConsoleCommandList
                             {
                                 ServerName = x.DeviceName,
                                 Command = x.Command,
                                 Submitter = x.Submitter,
                                 Result = x.Result,
                                 Comment = x.Comments
                             }).ToList();

            Response = Common.CreateResponse(result.OrderBy(x => x.ServerName));
            return Response;
        }

        [HttpGet("database_inventory")]
        public APIResponse getDatabaseInventory(string deviceId)
        {
            databaseRepository = new Repository<Database>(ConnectionString);
            List<DatabaseInventoryList> result = null;

            result = databaseRepository.Collection
                             .AsQueryable()
                             .Select(x => new DatabaseInventoryList
                             {
                                 Folder = x.Folder,
                                 FileNamePath = x.FileNamePath,
                                 Server = x.DeviceName,
                                 Title = x.Title,
                                 Status = x.Status,
                                 DesignTemplateName = x.DesignTemplateName,
                                 IsMailFile = x.IsMailFile
                             }).ToList();

            Response = Common.CreateResponse(result.OrderBy(x => x.Server));
            return Response;
        }

        [HttpGet("log_file")]
        public APIResponse GetLogFileData()
        {
            logFileRepository = new Repository<LogFile>(ConnectionString);
            List<LogFileList> result = null;

            result = logFileRepository.Collection
                             .AsQueryable()
                             .Select(x => new LogFileList
                             {
                                 Keyword = x.KeyWord,
                                 RepeatOnce = x.RepeateOnce
                             }).ToList();

            Response = Common.CreateResponse(result.OrderBy(x => x.Keyword));
            return Response;
        }

        [HttpGet("domino_mail_threshold")]
        public APIResponse GetMailThreshold()
        {
            serverRepository = new Repository<Server>(ConnectionString);
            List<MailThresholdList> result = null;
            FilterDefinition<Server> filterDef = serverRepository.Filter.Eq(x => x.DeviceType, "Domino");
            result = serverRepository.Find(filterDef)
                             .AsQueryable()
                             .Select(x => new MailThresholdList
                             {
                                 ServerName = x.DeviceName,
                                 DeadMailThreshold = x.DeadMailThreshold,
                                 HeldMailThreshold=x.HeldMailThreshold,
                                 PendingMailThreshold=x.PendingMailThreshold
                             }).ToList();

            Response = Common.CreateResponse(result.OrderBy(x => x.ServerName));
            return Response;
        }

        [HttpGet("notes_database")]
        public APIResponse GetNotesDatabase()
        {
            serverRepository = new Repository<Server>(ConnectionString);
            List<NotesDatabaseList> result = null;
            FilterDefinition<Server> filterDef = serverRepository.Filter.Eq(x => x.DeviceType, "Notes Database");
            result = serverRepository.Find(filterDef)
                             .AsQueryable()
                             .Select(x => new NotesDatabaseList
                             {
                                 ServerName = x.DeviceName,
                                 DatabaseFileName = x.DatabaseFileName,
                                 Category = x.Category,
                                 ScanInterval = x.ScanInterval,
                                 OffHoursScanInterval = x.OffHoursScanInterval,
                                 ResponseTime = x.ResponseTime,
                                 RetryInterval = x.RetryInterval,
                             }).ToList();

            Response = Common.CreateResponse(result.OrderBy(x => x.ServerName));
            return Response;
        }

        [HttpGet("domino_server_tasks")]
        public APIResponse GetDominoServerTasks()
        {
            doimoServerTasksRepository = new Repository<DominoServerTasks>(ConnectionString);
            List<DominoServerTasksList> result = null;
            result = doimoServerTasksRepository.Collection
                             .AsQueryable()
                             .Select(x => new DominoServerTasksList
                             {
                                 TaskName  = x.TaskName,
                                 FreezeDetect = x.FreezeDetect,
                                 IdleString = x.IdleString,
                                 RetryCount = x.RetryCount,
                                 LoadString = x.LoadString,
                                 MaxBusyTime = x.MaxBusyTime,
                                 SendExitCmd = x.ConsoleString,
                             }).ToList();

            Response = Common.CreateResponse(result.OrderBy(x => x.TaskName));
            return Response;
        }

        [HttpGet("community_users")]
        public APIResponse GetCommunityUsers()
        {
            connectionsRepository = new Repository<IbmConnectionsObjects>(ConnectionString);
            List<IBMConnCommunityUsersList> result = null;
            FilterDefinition<IbmConnectionsObjects> filterDef = connectionsRepository.Filter.Eq(x => x.Type, "Community");
            result = connectionsRepository.Find(filterDef)
                             .AsQueryable()
                             .Select(x => new IBMConnCommunityUsersList
                             {
                                 ServerName = x.DeviceName,
                                 Name = x.Name,
                                 users = x.users,
                             }).ToList();

            List<IBMConnCommunityUsersList> result2 = new List<IBMConnCommunityUsersList>(); ;
            foreach (IBMConnCommunityUsersList l in result)
            {
                foreach (string s in l.users)
                {
                    IBMConnCommunityUsersList ibm2 = new IBMConnCommunityUsersList();
                    ibm2.ServerName = l.ServerName;
                    ibm2.Name = l.Name;
                    FilterDefinition<IbmConnectionsObjects> filterDef2 = connectionsRepository.Filter.Eq(x => x.Type, "Users") & connectionsRepository.Filter.Eq(x => x.Id, s);
                    List<string> us = connectionsRepository.Find(filterDef2)
                                .AsQueryable()
                                .Select(x => x.Name).ToList();
                    foreach (string s1 in us)
                        ibm2.user = s1;
                    result2.Add(ibm2);
                }


            }
            Response = Common.CreateResponse(result2.OrderBy(x => x.Name));
            return Response;
        }


        [HttpGet("sametime_stats_grid")]
        public APIResponse GetSametimeStatisticsGrid(string startDate = "", string endDate = "")
        {

            try
            {

                List<String> StatNames = new List<string>() { "TotalNWayChats", "Total2WayChats", "PeakLogins" };
                //StatNames = new List<string>() { "Platform.System.PctCombinedCpuUtil", "ResponseTime", "Mem.PercentUsed" };
                if (startDate == "")
                    startDate = DateTime.UtcNow.AddDays(-7).ToUniversalTime().ToString(DateFormat);

                if (endDate == "")
                    endDate = DateTime.UtcNow.ToUniversalTime().ToString(DateFormat);

                //1 day is added to the end so we include that days data
                DateTime dtStart = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture);
                DateTime dtEnd = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture).AddDays(1);

                dtStart = DateTime.SpecifyKind(dtStart, DateTimeKind.Utc);
                dtEnd = DateTime.SpecifyKind(dtEnd, DateTimeKind.Utc);

                summaryRepository = new Repository<SummaryStatistics>(ConnectionString);

                var filterDef = summaryRepository.Filter.In(x => x.StatName, StatNames) &
                    summaryRepository.Filter.Gte(p => p.StatDate, dtStart) &
                    summaryRepository.Filter.Lte(p => p.StatDate, dtEnd);

                var results = summaryRepository.Find(filterDef).ToList();

                var listOfObjs = new List<IDictionary<string, object>>();
                
                foreach(var deviceName in results.Select(x => x.DeviceName).Distinct().ToList())
                {
                    var currList = results.Where(x => x.DeviceName == deviceName).ToList();
                    foreach (var statName in currList.Select(x => x.StatName).Distinct())
                    {
                        var workingList = currList.Where(x => x.StatName == statName).ToList();
                        var expandoObj = new ExpandoObject() as IDictionary<string, Object>;

                        expandoObj.Add("stat_name", workingList[0].StatName);
                        expandoObj.Add("device_name", workingList[0].DeviceName);

                        foreach (var entity in workingList)
                        {
                            if(expandoObj.Where(x => x.Key == entity.StatDate.Value.ToString(DateFormat)).Count() == 0)
                                expandoObj.Add(entity.StatDate.Value.ToString(DateFormat), entity.StatValue);
                        }
                        
                        listOfObjs.Add(expandoObj);
                    }
                    
                }

                Response = Common.CreateResponse(listOfObjs);
                return Response;

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }


        }


        [HttpGet("server_configuration")]
        public APIResponse GetDominoServerConfiguration(string type)
        {

            try
            {
                
                serverRepository = new Repository<Server>(ConnectionString);

                var filterDef = serverRepository.Filter.Eq(x => x.DeviceType, type);

                var results = serverRepository.Find(filterDef).ToList();
                
                Response = Common.CreateResponse(results);
                return Response;

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }


        }

        [HttpGet("server_list")]
        public APIResponse GetStatusList(string location_id = "", string device_type = "")
        {

            try
            {
                //DeviceName, IP, Type, Location, OS, Software
                serverRepository = new Repository<Server>(ConnectionString);
                statusRepository = new Repository<Status>(ConnectionString);

                var filterDefServer = serverRepository.Filter.Where(x => true);
                var filterDefStatus = statusRepository.Filter.Where(x => true);

                if(location_id != "")
                {
                    filterDefServer = filterDefServer & serverRepository.Filter.In(x => x.LocationId, location_id.Replace("[", "").Replace("]", "").Split(',').ToList());
                }

                if (device_type != "")
                {
                    filterDefServer = filterDefServer & serverRepository.Filter.In(x => x.DeviceType, device_type.Replace("[", "").Replace("]", "").Split(',').ToList());
                }

                var resultsServer = serverRepository.Find(filterDefServer).ToList();
                var resultsStatus = statusRepository.Find(filterDefStatus).ToList();

                var results = new List<Object>();

                foreach(var server in resultsServer)
                {
                    var status = resultsStatus.Where(i => i.DeviceId == server.Id).DefaultIfEmpty(new Status() {  }).First();
                    results.Add(new
                    {
                        DeviceName = server.DeviceName,
                        IPAddress = server.IPAddress,
                        DeviceType = server.DeviceType,
                        Location = status.Location,
                        OperatingSystem = status.OperatingSystem,
                        SoftwareVersion = status.SoftwareVersion
                    });
                }

                Response = Common.CreateResponse(results);
                return Response;

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }


        }

        [HttpGet("server_configuration_dropdown")]
        public APIResponse GetServerConfigurationDropdown(string docfield, string type = "")
        {

            try
            { 

                serverRepository = new Repository<Server>(ConnectionString);
                List<Server> results;
                if (type != "")
                    results = serverRepository.Find(x => x.DeviceType == type).ToList();
                else
                    results = serverRepository.Find(x => true).ToList();

                List<Location> locList = null;
                if (docfield == "location_id")
                {
                    locationRepository = new Repository<Location>(ConnectionString);
                    locList = locationRepository.Find(x => true).ToList();
                }
                List<NameValueModel> docList = new List<NameValueModel>();

                foreach (Server server in results)
                {
                    var x = new ExpandoObject() as IDictionary<string, Object>;
                    var bson = server.ToBsonDocument();

                    if (bson.Contains(docfield))
                    {
                        var statvalue = bson[docfield].ToString();

                        if(docfield == "location_id")
                            docList.Add(new NameValueModel() { Name = locList.Where(y => y.Id == statvalue).Select(y => y.LocationName).First(), Id = statvalue });
                        else
                            docList.Add(new NameValueModel() { Name = statvalue, Id = statvalue });

                    }
                }

                var list = docList.Select(x => new { Name = x.Name, Id = x.Id}).Distinct().Select(x => new NameValueModel() { Name = x.Name, Id = x.Id }).ToList();

                Response = Common.CreateResponse(list);
            
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }


    }
}
