﻿using System;
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
using Microsoft.AspNet.Authorization;

namespace VitalSigns.API.Controllers
{
    [Authorize("Bearer")]
    [Route("[controller]")]
    public class ReportsController : BaseController
    {
        private IRepository<ConsoleCommands> consoleCommandsRepository;
        private IRepository<LogFile> logFileRepository;
        private IRepository<SummaryStatistics> summaryRepository;
        private IRepository<Database> databaseRepository;
        private IRepository<Server> serverRepository;
        private IRepository<Credentials> credentialsRepository;
        private IRepository<Status> statusRepository;
        private IRepository<DailyStatistics> dailyRepository;
        private IRepository<IbmConnectionsObjects> connectionsObjectsRepository;

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
            if (year == -1)
            {
                year = DateTime.Today.Year;
            }
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
                        if (item.DeviceId == deviceid && server.Count > 0)
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

                var statNames = statName.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',').ToList();

                summaryRepository = new Repository<SummaryStatistics>(ConnectionString);

                var filterDef = summaryRepository.Filter.Eq(x => x.DeviceType, type) &
                    summaryRepository.Filter.In(x => x.StatName, statNames) &
                    summaryRepository.Filter.Gte(p => p.StatDate, dtStart) &
                    summaryRepository.Filter.Lte(p => p.StatDate, dtEnd);

                var results = summaryRepository.Find(filterDef).OrderBy(p => p.DeviceName).OrderBy(p => p.StatDate).ToList();

                var list = new List<IDictionary<string, object>>();
                foreach (var deviceId in results.Select(x => x.DeviceId).Distinct())
                {
                    double aggregatedValue = 0;
                    string aggregationDisplay = "";
                    
                    foreach (var stat in statNames)
                    {
                        var res = results.Where(x => x.DeviceId == deviceId && x.StatName == stat).ToList();
                        if (res.Count > 0)
                        {
                            switch (aggregationType.ToLower())
                            {
                                case "sum":
                                    aggregatedValue = results.Where(x => x.DeviceId == deviceId && x.StatName == stat).Sum(x => x.StatValue);
                                    aggregationDisplay = "Total";
                                    break;
                                case "avg":
                                    aggregatedValue = results.Where(x => x.DeviceId == deviceId && x.StatName == stat).Average(x => x.StatValue);
                                    aggregationDisplay = "Average";
                                    break;
                                case "max":
                                    aggregatedValue = results.Where(x => x.DeviceId == deviceId && x.StatName == stat).Max(x => x.StatValue);
                                    aggregationDisplay = "Max";
                                    break;
                                case "min":
                                    aggregatedValue = results.Where(x => x.DeviceId == deviceId && x.StatName == stat).Min(x => x.StatValue);
                                    aggregationDisplay = "Min";
                                    break;
                                default:
                                    throw new Exception("No matching aggregation type.");
                            }

                            var expandoObj = new ExpandoObject() as IDictionary<string, Object>;

                            
                            foreach (var entity in results.Where(x => x.DeviceId == deviceId && x.StatName == stat))
                            {
                                //expandoObj.Add(entity.CreatedOn.ToString("MM/dd/yyyy"), entity.StatValue);
                                expandoObj.Add(entity.StatDate.Value.ToString(DateFormat), entity.StatValue);
                            }
                            expandoObj.Add("Device Name", results.Where(x => x.DeviceId == deviceId && x.StatName == stat).ToList()[0].DeviceName);
                            
                            expandoObj.Add(aggregationDisplay, aggregatedValue);
                            if (statNames.Count > 1)
                            {
                                expandoObj.Add("Statistic", stat);
                            }
                            list.Add(expandoObj);
                        }
                    }
                }
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
        public APIResponse GetSumamryStatsChart(string statName, string deviceId = "", string startDate = "", string endDate = "", string type = "", string aggregation = "")
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
                if (!string.IsNullOrWhiteSpace(aggregation))
                {
                    filterDef = filterDef & summaryRepository.Filter.Eq(p => p.AggregationType, aggregation);
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

                        serie.Segments.Add(new Segment() { Label = date.ToString(DateFormat), Value = item.Count > 0 ? (double?)Math.Round(item[0].StatValue, 2) : null });
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

                    for (var tempDate = dtDate; tempDate < dtDate.AddDays(1); tempDate = tempDate.AddHours(1))
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
                        segments.Add(new Segment() { Label = currSerie.Title, Value = (int)(currSerie.Segments.Where(x => x.Value >= Convert.ToInt32(minValue)).Sum(x => x.Value) / minsInMonth) });
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
                            if (item.DeviceId == deviceid && server.Count > 0)
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
        public APIResponse GetTravelerStats(string deviceId = "", string paramtype = "", string paramvalue = "")
        {
            List<TravelerStats> result = new List<TravelerStats>();
            FilterDefinition<Server> filterDef = null;
            Repository<TravelerStats> travelerRepository = new Repository<TravelerStats>(ConnectionString);
            List<Serie> series = new List<Serie>();
            string travelername = "";
            bool foundInt = false;

            try
            {
                if (!string.IsNullOrEmpty(deviceId))
                {
                    serverRepository = new Repository<Server>(ConnectionString);
                    filterDef = serverRepository.Filter.Eq(x => x.Id, deviceId);
                    var serverlist = serverRepository.Find(filterDef).ToList();
                    if (serverlist.Count > 0)
                    {
                        travelername = serverlist[0].DeviceName;
                    }
                    var builder = Builders<TravelerStats>.Filter;
                    result = travelerRepository.Collection.Aggregate()
                               .Match(builder.And(builder.Eq(paramtype, paramvalue), builder.Eq(x => x.TravelerServerName, travelername))).ToList();
                }
                else
                {
                    result = travelerRepository.Collection.Aggregate().ToList();
                }

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
                                segments.Add(new Segment { Label = record.CreatedOn.ToString(DateFormat), Value = Convert.ToDouble(record.OpenTimes) });
                                serie.Title = interval;
                            }
                        }
                        if (!foundInt)
                        {
                            for (int i = 0; i < segments.Count; i++)
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
            int charLimit = 40;
            databaseRepository = new Repository<Database>(ConnectionString);
            List<DatabaseInventoryList> result = null;
            var res = databaseRepository.Collection.Aggregate()
                                    .Match(_ => true).ToList();
            result = res.Select(x => new DatabaseInventoryList
            {
                Folder = x.Folder,
                FileNamePath = x.FileNamePath != null ? (x.FileNamePath.Length > charLimit ? x.FileNamePath.Substring(0, charLimit) + "..." : x.FileNamePath) : "",
                Server = x.DeviceName,
                Title = x.Title.Length > charLimit ? x.Title.Substring(0, charLimit) + "..." : x.Title,
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
                                 HeldMailThreshold = x.HeldMailThreshold,
                                 PendingMailThreshold = x.PendingMailThreshold
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
                                 TaskName = x.TaskName,
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
        public APIResponse GetCommunityUsers(string deviceId = "")
        {
            FilterDefinition<IbmConnectionsObjects> filterDef;
            List<String> listOfDevices;
            try
            {
                connectionsRepository = new Repository<IbmConnectionsObjects>(ConnectionString);
                List<IBMConnCommunityUsersList> result = null;
                if (!string.IsNullOrEmpty(deviceId))
                {
                    listOfDevices = deviceId.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',').ToList();
                    filterDef = connectionsRepository.Filter.And(connectionsRepository.Filter.Eq(x => x.Type, "Community"),
                        connectionsRepository.Filter.In(x => x.DeviceId, listOfDevices));
                }
                else
                {
                    filterDef = connectionsRepository.Filter.Eq(x => x.Type, "Community");
                }
                result = connectionsRepository.Find(filterDef)
                                 .AsQueryable()
                                 .Select(x => new IBMConnCommunityUsersList
                                 {
                                     ServerName = x.DeviceName,
                                     Name = x.Name,
                                     users = x.users,
                                     CommunityType = x.CommunityType,
                                     NumOfOwners = x.NumOfOwners,
                                     NumOfMembers = x.NumOfMembers.Value,
                                     NumOfFollowers = x.NumOfFollowers.Value
                                 }).ToList();

                List<IBMConnCommunityUsersList> result2 = new List<IBMConnCommunityUsersList>();
                foreach (IBMConnCommunityUsersList l in result)
                {
                    if (l.users != null)
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
                            ibm2.users = us;
                            ibm2.CommunityType = l.CommunityType;
                            ibm2.NumOfOwners = l.NumOfOwners;
                            ibm2.NumOfMembers = l.NumOfMembers;
                            ibm2.NumOfFollowers = l.NumOfFollowers;
                            result2.Add(ibm2);
                        }
                    }
                }
                Response = Common.CreateResponse(result2.OrderBy(x => x.Name));
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }


        [HttpGet("sametime_stats_grid")]
        public APIResponse GetSametimeStatisticsGrid(string startDate = "", string endDate = "")
        {

            try
            {

                List<String> StatNames = new List<string>() { "TotalnWayChats", "Total2WayChats", "PeakLogins" };
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

                foreach (var deviceName in results.Select(x => x.DeviceName).Distinct().ToList())
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
                            if (expandoObj.Where(x => x.Key == entity.StatDate.Value.ToString(DateFormat)).Count() == 0)
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

                if (location_id != "")
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

                foreach (var server in resultsServer)
                {
                    var status = resultsStatus.Where(i => i.DeviceId == server.Id).DefaultIfEmpty(new Status() { }).First();
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

                        if (docfield == "location_id")
                            docList.Add(new NameValueModel() { Name = locList.Where(y => y.Id == statvalue).Select(y => y.LocationName).First(), Id = statvalue });
                        else
                            docList.Add(new NameValueModel() { Name = statvalue, Id = statvalue });

                    }
                }

                var list = docList.Select(x => new { Name = x.Name, Id = x.Id }).Distinct().Select(x => new NameValueModel() { Name = x.Name, Id = x.Id }).ToList();

                Response = Common.CreateResponse(list);

                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("server_types_summary")]
        public APIResponse GetServerTypesFromSumamry()
        {

            try
            {

                summaryRepository = new Repository<SummaryStatistics>(ConnectionString);


                var list = summaryRepository.Collection.Distinct(x => x.DeviceType, x => true).ToList().Select(x => new NameValueModel()
                {
                    Name = x,
                    Value = x
                }).ToList().OrderBy(x => x.Name).ToList();

                Response = Common.CreateResponse(list);

                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("statistics_types_summary")]
        public APIResponse GetStatisticsTypesFromSumamry(string type)
        {

            try
            {

                summaryRepository = new Repository<SummaryStatistics>(ConnectionString);


                var list = summaryRepository.Collection.Distinct(x => x.StatName, x => x.DeviceType == type).ToList().Select(x => new NameValueModel()
                {
                    Name = x,
                    Value = x
                }).ToList().OrderBy(x => x.Name).ToList();

                Response = Common.CreateResponse(list);

                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("connections/community_activity")]
        public APIResponse ConnectionsCommunityActivity()
        {
            List<dynamic> result = new List<dynamic>();
            FilterDefinition<IbmConnectionsObjects> filterDef;
            DateTime lastXDays = new DateTime();
            try
            {
                lastXDays = DateTime.Now.AddDays(-7);
                UtilsController uc = new UtilsController();
                if (uc.isRPRWyattMachine())
                    lastXDays = DateTime.Now.AddYears(-5);
                connectionsObjectsRepository = new Repository<IbmConnectionsObjects>(ConnectionString);
                var listOfCommunity = connectionsObjectsRepository.Find(i => i.Type == "Community")
                    .OrderBy(i => i.Name).OrderBy(i => i.DeviceName).ToList();



                filterDef = connectionsObjectsRepository.Filter.And(connectionsObjectsRepository.Filter.In(i => i.ParentGUID, listOfCommunity.Select(x => x.Id)),
                       connectionsObjectsRepository.Filter.Gte(i => i.ObjectCreatedDate, lastXDays));

                var res = connectionsObjectsRepository.Collection
                    .Aggregate()
                    .Match(filterDef)
                    .Group(
                        x => new { DeviceName = x.DeviceName, Type = x.Type, ParentGUID = x.ParentGUID }, 
                        g => new { Key = g.Key, Count = g.Count()})
                    .ToList()
                    .Select(x => new CommunityActivity
                    {
                        ServerName = x.Key.DeviceName,
                        Community = listOfCommunity.Find(y => y.Id == x.Key.ParentGUID).Name,
                        ObjectName = x.Key.Type,
                        ObjectValue = x.Count,
                        DateRange = "Last 7 Days"
                    })
                    .OrderBy(x => x.ServerName)
                    .ThenBy(x => x.Community)
                    .ThenBy(x => x.ObjectName)
                    .ToList();
                                    
                Response = Common.CreateResponse(res);
                return Response;
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }

        }

        [HttpGet("connections/user_activity")]
        public APIResponse ConnectionsUserActivity(string deviceId = "", bool isChart = false, int topX = 0)
        {
            List<UserAdoptionPivot> result = new List<UserAdoptionPivot>();
            List<UserActivityBubble> resultchart = new List<UserActivityBubble>();
            UserActivityBubble uab = new UserActivityBubble();
            List<dynamic> resultFinal = new List<dynamic>();
            List<string> objectTypes = new List<string>();
            List<string> userList = new List<string>();
            List<string> listOfDevices = new List<string>();
            List<UserAdoptionPivot> sortedList;
            List<IbmConnectionsObjects> listOfCommunity = new List<IbmConnectionsObjects>();
            UserAdoptionPivot ua = new UserAdoptionPivot();
            List<int> uaVal = new List<int>();
            int total = 0;
            FilterDefinition<IbmConnectionsObjects> filterDef;
            DateTime lastXDays = new DateTime();
            int xcoord = 0;
            int ycoord = 0;
            try
            {
                lastXDays = DateTime.Now.AddDays(-90);
                UtilsController uc = new UtilsController();
                if (uc.isRPRWyattMachine())
                    lastXDays = DateTime.Now.AddYears(-5);
                //Find all communities
                connectionsObjectsRepository = new Repository<IbmConnectionsObjects>(ConnectionString);
                if (!string.IsNullOrEmpty(deviceId))
                {
                    listOfDevices = deviceId.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',').ToList();
                    listOfCommunity = connectionsObjectsRepository.Find(i => i.Type == "Community" && listOfDevices.Contains(i.DeviceId)).ToList();
                }
                else
                {
                    listOfCommunity = connectionsObjectsRepository.Find(i => i.Type == "Community").ToList();
                }

                //Iterate through the list of communities and build a list of distinct object types, i.e., blogs, bookmarks, etc.

                var listOfStr = listOfCommunity.Select(x => x.Id).ToList();

                filterDef = connectionsObjectsRepository.Filter.And(connectionsObjectsRepository.Filter.In(i => i.ParentGUID, listOfCommunity.Select(x => x.Id)),
                        connectionsObjectsRepository.Filter.Gte(i => i.ObjectCreatedDate, lastXDays));
                objectTypes = connectionsObjectsRepository.Collection.Distinct(i => i.Type, filterDef).ToList();

                //Iterate through the list of users and collect information about each user's activity level for each of the object types above
                if (!string.IsNullOrEmpty(deviceId))
                {
                    filterDef = connectionsObjectsRepository.Filter.Eq(i => i.Type, "Users") & connectionsObjectsRepository.Filter.In(x => x.DeviceId, listOfDevices);
                }
                else
                {
                    filterDef = connectionsObjectsRepository.Filter.Eq(i => i.Type, "Users");
                }
                
                var listOfUsers = connectionsObjectsRepository.Find(filterDef).ToList();
                //var listOfUserNames = connectionsObjectsRepository.Collection.Distinct(i => i.Name, filterDef).ToList();

                filterDef = connectionsObjectsRepository.Filter.And(connectionsObjectsRepository.Filter.In(i => i.OwnerId, listOfUsers.Select(x => x.Id)),
                        connectionsObjectsRepository.Filter.Gte(i => i.ObjectCreatedDate, lastXDays),
                        connectionsObjectsRepository.Filter.In(i => i.Type, objectTypes));
                if (!string.IsNullOrEmpty(deviceId))
                {
                    filterDef = filterDef & connectionsObjectsRepository.Filter.In(x => x.DeviceId, listOfDevices);
                }

                var res = connectionsObjectsRepository.Collection.Aggregate()
                    .Match(filterDef)
                    .Group(
                    x => new { DeviceName = x.DeviceName, OwnerId = x.OwnerId, Type = x.Type},
                    g => new {Key = g.Key , Count = g.Count()})
                    .ToList()
                    .Select(x => new UserAdoption
                    {
                        ServerName = x.Key.DeviceName,
                        ObjectName = x.Key.Type,
                        ObjectValue = x.Count,
                        UserName = listOfUsers.Find(y => y.Id == x.Key.OwnerId).Name
                    }).ToList();

                if (!isChart)
                {
                    foreach (var curr in res)
                    {

                        var currResults = result.Where(x => x.UserName == curr.UserName && x.ServerName == curr.ServerName).ToList();
                        UserAdoptionPivot currResult;
                        if (currResults.Count() == 0)
                        {
                            currResult = new UserAdoptionPivot()
                            {
                                ServerName = curr.ServerName,
                                UserName = curr.UserName,
                                ObjectValues = Enumerable.Repeat(0, objectTypes.Count()).ToList()
                            };
                            result.Add(currResult);
                        }
                        else
                        {
                            currResult = currResults[0];
                        }

                        int currIndex = objectTypes.IndexOf(curr.ObjectName);
                        currResult.ObjectValues[currIndex] = curr.ObjectValue;
                    }
                }
                else
                {
                    if(topX != 0)
                    {
                        var grouped = res.GroupBy(x => x.UserName).Select(g => new { UserName = g.Key, Sum = g.Sum(x => x.ObjectValue) }).OrderByDescending(x => x.Sum).Take(5);
                        res = res.Where(x => grouped.Select(y => y.UserName).Contains(x.UserName)).ToList();
                    }

                    userList = res.GroupBy(x => x.UserName).Select(x => new { UserName = x.Key, Count = x.Sum(y => y.ObjectValue) }).OrderByDescending(x => x.Count).Select(x => x.UserName).Distinct().ToList();

                    foreach(var user in userList)
                    {
                        var currList = res.Where(x => x.UserName == user).GroupBy(x => x.ObjectName).Select(x => new { ObjectName = x.Key, Count = x.Sum(y => y.ObjectValue)}).ToList();
                        foreach(var curr in currList)
                        {
                            var currResult = new UserActivityBubble()
                            {
                                Y = ycoord,
                                X = xcoord,
                                Z = curr.Count,
                                Name = user,
                                Activity = curr.ObjectName
                            };
                            resultchart.Add(currResult);

                            ycoord += 1;
                        }
                        ycoord = 0;
                        xcoord += 1;
                    }

                    
                }
                


                if (topX != 0)
                {
                    sortedList = result.OrderByDescending(i => i.Total).Take(topX).ToList();
                }
                else
                {
                    sortedList = result.OrderByDescending(i => i.Total).ToList();
                }


                if (!isChart)
                {
                    resultFinal.Add(sortedList);
                    resultFinal.Add(objectTypes);
                    resultFinal.Add(listOfUsers.Select(x => x.Name).Distinct().ToList());
                    Response = Common.CreateResponse(resultFinal);
                }
                else
                {
                    List<Serie> series = new List<Serie>();
                    Serie serie = new Serie();
                    serie.Title = "User Activity";
                    serie.Segments = new List<Segment>();
                    foreach (var val in resultchart)
                    {
                        serie.Segments.Add(new Segment() { Label = val.Name, Label2 = val.Activity, Value = val.Y, Value1 = val.X, Value2 = val.Z });
                    }
                    series.Add(serie);
                    Chart chart = new Chart();
                    chart.Title = "";
                    chart.Series = series;
                    resultFinal.Add(chart);
                    resultFinal.Add(objectTypes);
                    resultFinal.Add(userList);
                    Response = Common.CreateResponse(resultFinal);

                }

                return Response;

                //}
                //    foreach (var user in sortedList)
                //    {
                //        var userId = listOfUsers.Find(i => i.Name == user.UserName);
                //        if (!string.IsNullOrEmpty(deviceId))
                //        {
                //            listOfDevices = deviceId.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',').ToList();
                //            filterDef = connectionsObjectsRepository.Filter.And(connectionsObjectsRepository.Filter.Eq(i => i.OwnerId, userId.Id),
                //            connectionsObjectsRepository.Filter.Gte(i => i.ObjectCreatedDate, lastXDays),
                //            connectionsObjectsRepository.Filter.In(i => i.Type, objectTypes),
                //            connectionsObjectsRepository.Filter.In(i => i.DeviceId, listOfDevices));
                //        }
                //        else
                //        {
                //            filterDef = connectionsObjectsRepository.Filter.And(connectionsObjectsRepository.Filter.Eq(i => i.OwnerId, userId.Id),
                //            connectionsObjectsRepository.Filter.Gte(i => i.ObjectCreatedDate, lastXDays),
                //            connectionsObjectsRepository.Filter.In(i => i.Type, objectTypes));
                //        }
                //        var res = connectionsObjectsRepository.Find(filterDef)
                //        .GroupBy(row => new
                //        {
                //            row.DeviceName,
                //            row.OwnerId,
                //            row.Type
                //        })
                //        .Select(x => new UserAdoption
                //        {
                //            ServerName = x.Key.DeviceName,
                //            ObjectName = x.Key.Type,
                //            ObjectValue = x.Count(),
                //            UserName = user.UserName
                //        }).ToList();
                //        ycoord = 0;
                //        foreach (var obj in objectTypes)
                //        {
                //            var record = res.Find(x => x.ObjectName == obj);
                //            if (record != null)
                //            {
                //                uab = new UserActivityBubble
                //                {
                //                    X = xcoord,
                //                    Y = ycoord,
                //                    Z = record.ObjectValue,
                //                    Name = user.UserName,
                //                    Activity = obj
                //                };
                //                resultchart.Add(uab);
                //                if (!userList.Contains(user.UserName))
                //                {
                //                    userList.Add(user.UserName);
                //                }
                //            }
                //            ycoord += 1;
                //        }
                //        xcoord += 1;
                //    }
                //    List<Serie> series = new List<Serie>();
                //    Serie serie = new Serie();
                //    serie.Title = "User Activity";
                //    serie.Segments = new List<Segment>();
                //    foreach (var val in resultchart)
                //    {
                //        serie.Segments.Add(new Segment() { Label = val.Name, Label2 = val.Activity, Value = val.Y, Value1 = val.X, Value2 = val.Z });
                //    }
                //    series.Add(serie);
                //    Chart chart = new Chart();
                //    chart.Title = "";
                //    chart.Series = series;
                //    resultFinal.Add(chart);
                //    resultFinal.Add(objectTypes);
                //    resultFinal.Add(userList);
                //    Response = Common.CreateResponse(resultFinal);

                return Response;
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }

        }

        [HttpGet("connections/user_activity_monthly")]
        public APIResponse ConnectionsUserActivityMonthly(string userNames = "")
        {
            List<UserAdoptionPivot> result = new List<UserAdoptionPivot>();
            List<UserActivityBubble> resultchart = new List<UserActivityBubble>();
            List<dynamic> resultFinal = new List<dynamic>();
            FilterDefinition<IbmConnectionsObjects> filterDef;
            List<string> objectTypes = new List<string>();
            Dictionary<string, int> objectsAdded = new Dictionary<string, int>();
            List<string> userList = new List<string>();
            List<string> objectList = new List<string>();
            List<string> users = new List<string>();
            List<string> listOfUserNames = new List<string>();
            List<IbmConnectionsObjects> listOfUsers = new List<IbmConnectionsObjects>();
            DateTime lastXDays = new DateTime();
            UserAdoptionPivot ua = new UserAdoptionPivot();
            UserActivityBubble uab = new UserActivityBubble();
            int xcoord = 0;
            int ycoord = 0;

            try
            {
                lastXDays = DateTime.Now.AddMonths(-6);
                UtilsController uc = new UtilsController();
                if (uc.isRPRWyattMachine())
                    lastXDays = DateTime.Now.AddYears(-5);
                //Find all communities
                connectionsObjectsRepository = new Repository<IbmConnectionsObjects>(ConnectionString);
                var listOfCommunity = connectionsObjectsRepository.Find(i => i.Type == "Community").ToList();

                //Iterate through the list of communities and build a list of distinct object types, i.e., blogs, bookmarks, etc.
                filterDef = connectionsObjectsRepository.Filter.And(connectionsObjectsRepository.Filter.In(i => i.ParentGUID, listOfCommunity.Select(x => x.Id)),
                        connectionsObjectsRepository.Filter.Gte(i => i.ObjectCreatedDate, lastXDays));
                objectTypes = connectionsObjectsRepository.Collection.Distinct(i => i.Type, filterDef).ToList();

                //Iterate through the list of users and collect information about each user's activity level for each of the object types above
                if (!string.IsNullOrWhiteSpace(userNames))
                {
                    users = userNames.Replace("[", "").Replace("]", "").Split(',').ToList();
                    filterDef = connectionsObjectsRepository.Filter.And(connectionsObjectsRepository.Filter.Eq(i => i.Type, "Users"),
                        connectionsObjectsRepository.Filter.In(i => i.Name, users));
                    listOfUsers = connectionsObjectsRepository.Find(filterDef).ToList();
                    listOfUserNames = users;
                }
                else
                {
                    filterDef = connectionsObjectsRepository.Filter.Eq(i => i.Type, "Users");
                    listOfUsers = connectionsObjectsRepository.Find(filterDef).ToList();
                    listOfUserNames = connectionsObjectsRepository.Collection.Distinct(i => i.Name, filterDef).ToList();
                }

                filterDef = connectionsObjectsRepository.Filter.And(connectionsObjectsRepository.Filter.In(i => i.OwnerId, listOfUsers.Select(x => x.Id)),
                        connectionsObjectsRepository.Filter.Gte(i => i.ObjectCreatedDate, lastXDays),
                        connectionsObjectsRepository.Filter.In(i => i.Type, objectTypes));

                var res = connectionsObjectsRepository.Find(filterDef)
                        .GroupBy(row => new
                        {
                            row.OwnerId,
                            row.Type,
                            row.ObjectCreatedDate.Value.Month,
                            row.ObjectCreatedDate.Value.Year
                        })
                        .ToList()
                        .Select(x => new UserAdoption
                        {
                            ObjectName = x.Key.Type,
                            ObjectValue = x.Count(),
                            ObjectCreatedDate = new DateTime(x.Key.Year, x.Key.Month, 1),
                            UserName = listOfUsers.Find(y => y.Id == x.Key.OwnerId).Name
                        }).ToList();

                foreach(var curr in res)
                {

                    ua = new UserAdoptionPivot
                    {
                        ServerName = curr.ServerName,
                        UserName = curr.UserName,
                        ObjectValue = curr.ObjectValue,
                        ObjectType = curr.ObjectName,
                        ObjectCreatedDate = curr.ObjectCreatedDate
                    };
                    result.Add(ua);
                    
                }

                result = result.OrderBy(i => i.ObjectCreatedDate).ToList();
                foreach (var record in result)
                {
                    if (!userList.Contains(record.UserName))
                    {
                        userList.Add(record.UserName);
                    }
                    var dt = record.ObjectCreatedDate.ToString("MMM yyyy");
                    if (!objectList.Contains(dt))
                    {
                        objectList.Add(dt);
                    }
                }
                List<Serie> series = new List<Serie>();
                foreach (var val in result)
                {
                    Serie serie = new Serie();
                    Serie seriefound = series.Find(i => i.Title == val.ObjectType);
                    ycoord = userList.FindIndex(i => i == val.UserName);
                    if (ycoord == -1)
                    {
                        ycoord = 0;
                    }

                    var allObjects = result.FindAll(i => i.ObjectCreatedDate == val.ObjectCreatedDate);
                    var coords = CalculateCoords(allObjects.Count);
                    var dti = val.ObjectCreatedDate.ToString("MMM yyyy");
                    xcoord = objectList.FindIndex(i => i == dti);
                    if (xcoord == -1)
                    {
                        xcoord = 0;
                    }
                    if (!objectsAdded.ContainsKey(val.ObjectCreatedDate.ToShortDateString()))
                    {
                        objectsAdded.Add(val.ObjectCreatedDate.ToShortDateString(), 0);
                    }
                    else
                    {
                        objectsAdded[val.ObjectCreatedDate.ToShortDateString()] += 1;
                    }
                    if (seriefound == null)
                    {
                        serie.Title = val.ObjectType;
                        serie.Segments = new List<Segment>();
                        Segment segment = new Segment
                        {
                            Label = dti,
                            Label2 = val.UserName,
                            Value = ycoord,
                            Value1 = xcoord + coords[objectsAdded[val.ObjectCreatedDate.ToShortDateString()]],
                            Value2 = val.ObjectValue
                        };
                        serie.Segments.Add(segment);
                        series.Add(serie);
                    }
                    else
                    {
                        Segment segment = new Segment
                        {
                            Label = dti,
                            Label2 = val.UserName,
                            Value = ycoord,
                            Value1 = xcoord + coords[objectsAdded[val.ObjectCreatedDate.ToShortDateString()]],
                            Value2 = val.ObjectValue
                        };
                        seriefound.Segments.Add(segment);
                    }
                }
                Chart chart = new Chart();
                chart.Title = "";
                chart.Series = series;
                resultFinal.Add(chart);
                resultFinal.Add(userList);
                resultFinal.Add(objectList);
                Response = Common.CreateResponse(resultFinal);
                return Response;


                /*
                foreach (var user in listOfUserNames)
                {
                    var userId = listOfUsers.Find(i => i.Name == user);
                    filterDef = connectionsObjectsRepository.Filter.And(connectionsObjectsRepository.Filter.Eq(i => i.OwnerId, userId.Id),
                        connectionsObjectsRepository.Filter.Gte(i => i.ObjectCreatedDate, lastXDays),
                        connectionsObjectsRepository.Filter.In(i => i.Type, objectTypes));
                    var res = connectionsObjectsRepository.Find(filterDef)
                        .GroupBy(row => new
                        {
                            row.DeviceName,
                            row.OwnerId,
                            row.Type,
                            row.ObjectCreatedDate.Value.Month,
                            row.ObjectCreatedDate.Value.Year
                        })
                        .Select(x => new UserAdoption
                        {
                            ServerName = x.Key.DeviceName,
                            ObjectName = x.Key.Type,
                            ObjectValue = x.Count(),
                            ObjectCreatedDate = new DateTime(x.Key.Year, x.Key.Month, 1),
                            UserName = user
                        }).ToList();
                    if (res.Count > 0)
                    {
                        foreach (var obj in objectTypes)
                        {
                            var records = res.FindAll(x => x.ObjectName == obj);
                            if (records.Count > 0)
                            {
                                foreach (var record in records)
                                {
                                    if (record != null)
                                    {
                                        ua = new UserAdoptionPivot
                                        {
                                            ServerName = record.ServerName,
                                            UserName = record.UserName,
                                            ObjectValue = record.ObjectValue,
                                            ObjectType = record.ObjectName,
                                            ObjectCreatedDate = record.ObjectCreatedDate
                                        };
                                        result.Add(ua);
                                    }
                                }
                            }
                        }
                    }
                }
                result = result.OrderBy(i => i.ObjectCreatedDate).ToList();
                foreach (var record in result)
                {
                    if (!userList.Contains(record.UserName))
                    {
                        userList.Add(record.UserName);
                    }
                    var dt = record.ObjectCreatedDate.ToString("MMM yyyy");
                    if (!objectList.Contains(dt))
                    {
                        objectList.Add(dt);
                    }
                }
                List<Serie> series = new List<Serie>();
                foreach (var val in result)
                {
                    Serie serie = new Serie();
                    Serie seriefound = series.Find(i => i.Title == val.ObjectType);
                    ycoord = userList.FindIndex(i => i == val.UserName);
                    if (ycoord == -1)
                    {
                        ycoord = 0;
                    }

                    var allObjects = result.FindAll(i => i.ObjectCreatedDate == val.ObjectCreatedDate);
                    var coords = CalculateCoords(allObjects.Count);
                    var dti = val.ObjectCreatedDate.ToString("MMM yyyy");
                    xcoord = objectList.FindIndex(i => i == dti);
                    if (xcoord == -1)
                    {
                        xcoord = 0;
                    }
                    if (!objectsAdded.ContainsKey(val.ObjectCreatedDate.ToShortDateString()))
                    {
                        objectsAdded.Add(val.ObjectCreatedDate.ToShortDateString(), 0);
                    }
                    else
                    {
                        objectsAdded[val.ObjectCreatedDate.ToShortDateString()] += 1;
                    }
                    if (seriefound == null)
                    {
                        serie.Title = val.ObjectType;
                        serie.Segments = new List<Segment>();
                        Segment segment = new Segment
                        {
                            Label = dti,
                            Label2 = val.UserName,
                            Value = ycoord,
                            Value1 = xcoord + coords[objectsAdded[val.ObjectCreatedDate.ToShortDateString()]],
                            Value2 = val.ObjectValue
                        };
                        serie.Segments.Add(segment);
                        series.Add(serie);
                    }
                    else
                    {
                        Segment segment = new Segment
                        {
                            Label = dti,
                            Label2 = val.UserName,
                            Value = ycoord,
                            Value1 = xcoord + coords[objectsAdded[val.ObjectCreatedDate.ToShortDateString()]],
                            Value2 = val.ObjectValue
                        };
                        seriefound.Segments.Add(segment);
                    }
                }
                Chart chart = new Chart();
                chart.Title = "";
                chart.Series = series;
                resultFinal.Add(chart);
                resultFinal.Add(userList);
                resultFinal.Add(objectList);
                Response = Common.CreateResponse(resultFinal);
                return Response;

    */
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        private List<double> CalculateCoords(int listsize)
        {
            List<double> coords = new List<double>();
            double inc = 0.0;
            //inc = 1.0 / (double)(objList.Count + 1);
            inc = 1.0 / 20.0;
            for (int i = 0; i < listsize; i++)
            {
                coords.Add(inc * (i + 1) * (-1));
            }
            return coords;
        }

        [HttpGet("connections/most_popular_communities")]
        public APIResponse ConnectionsPopularCommunitiesMonthly(string userNames = "")
        {
            try
            {
                connectionsObjectsRepository = new Repository<IbmConnectionsObjects>(ConnectionString);
                var listOfObjects = connectionsObjectsRepository.Collection.Aggregate()
                    .Match(x => (x.Type == "Community") && x.Children != null)
                    .Unwind(x => x.Children)
                    .Sort(new BsonDocument("children.count", -1)).ToList();
                List<Segment> segmentList = new List<Segment>();

                foreach (var doc in listOfObjects)
                {
                    var children = doc["children"];
                    Segment segment = new Segment()
                    {
                        Label = doc["name"].AsString,
                        Value = children[1].AsInt32
                    };
                    segmentList.Add(segment);
                }
                Serie serie = new Serie();
                serie.Title = "total";
                serie.Segments = segmentList;

                List<Serie> series = new List<Serie>();
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

        [HttpGet("connections/executive_overview")]
        public APIResponse ConnectionsExecutiveOverview(string date = "")
        {
            try
            {
                connectionsObjectsRepository = new Repository<IbmConnectionsObjects>(ConnectionString);
                credentialsRepository = new Repository<Credentials>(ConnectionString);
                serverRepository = new Repository<Server>(ConnectionString);

                if (date == "")
                    date = DateTime.UtcNow.AddDays(-7).ToString(DateFormat);

                DateTime dtStart = DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture).ToUniversalTime();
                DateTime dtEnd = dtStart.AddDays(7);

                //group aggregation string since cannot use expressions
                //groups on type nad device_name, aggregates the total count, if it has a parent (in a community), new objects in a community and new objects not in a community
                var bsonStr = BsonDocument.Parse(@"{ 
        ""_id"" : {
                    ""type"" : ""$type"", 
            ""device_name"" : ""$device_name""
        },
        ""count"" : { ""$sum"" : 1 }, 
        ""has_parent"" : {
                    ""$sum"" : {
                        ""$cond"" : [{ 
                    ""$ifNull"" : [""$parent_guid"", false]
    }, 1, 0] } 
         }, 
         ""new_objects_in_community"" : { 
             ""$sum"" : { 
                 ""$cond"" : [{ 
                     ""$and"" : [
                        {""$gte"" : [ ""$object_created_date"", ISODate(""" + new BsonDateTime(dtStart).ToString() + @""") ]},
                        {""$lte"" : [ ""$object_created_date"", ISODate(""" + new BsonDateTime(dtEnd).ToString() + @""") ]},
                        {""$ifNull"" : [ ""$parent_guid"", false ]}
                     ] 
                 }, 1, 0] 
             },
         } 
        ""new_objects_not_in_community"" : { 
             ""$sum"" : { 
                 ""$cond"" : [{ 
                     ""$and"" : [
                        {""$gte"" : [ ""$object_created_date"", ISODate(""" + new BsonDateTime(dtStart).ToString() + @""") ]},
                        {""$lte"" : [ ""$object_created_date"", ISODate(""" + new BsonDateTime(dtEnd).ToString() + @""") ]},
                        {""$ifNull"" : [ ""$parent_guid"", true ]}
                     ] 
                 }, 1, 0] 
             } } }");

                //creates a filter def
                var types = new List<string>() { "Community", "Blog", "Wiki", "Forum", "Activity" };
                var filterDef = connectionsObjectsRepository.Filter.In(x => x.Type, types) & connectionsObjectsRepository.Filter.Lte(x => x.ObjectCreatedDate, dtEnd);
                
                //excludes the user ID which VS uses to test simulation tests
                try
                {
                    var credIds = serverRepository.Find(serverRepository.Filter.Eq(x => x.DeviceType, Enums.ServerType.IBMConnections.ToDescription())).Select(x => new { x.CredentialsId, x.Id }).ToList();
                    var creds = credentialsRepository.Find(credentialsRepository.Filter.In(x => x.Id, credIds.Select(y => y.CredentialsId))).ToList();
                    foreach (var curr in credIds)
                    {
                        try
                        {
                            var userId = creds.Where(x => x.Id == curr.CredentialsId).FirstOrDefault().UserId.ToLower();
                            var connectionsObjectUserId = connectionsObjectsRepository.Find(
                                    connectionsObjectsRepository.Filter.Eq(x => x.DeviceId, curr.Id) &
                                    connectionsObjectsRepository.Filter.Eq(x => x.LogonName, userId)
                                ).ToList();
                            if (connectionsObjectUserId.Count() > 0)
                                filterDef = filterDef & !(connectionsObjectsRepository.Filter.Eq(x => x.DeviceId, curr.Id) & connectionsObjectsRepository.Filter.Eq(x => x.OwnerId, connectionsObjectUserId.First().Id));
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception) { }

                var ffff = filterDef.ToString();

                //does the call to mongo and outputs the data into usable objects from BSONDocuments
                var resultsFromMongo = connectionsObjectsRepository.Collection.Aggregate()
                    .Match(filterDef)
                    .Group(bsonStr)
                    .ToList()
                    .Select(x => new
                    {
                        Type = x["_id"]["type"].AsString,
                        DeviceName = x["_id"]["device_name"].AsString,
                        Count = x["count"].AsInt32,
                        CountInCommunity = x["has_parent"].AsInt32,
                        CountNewInCommunity = x["new_objects_in_community"].AsInt32,
                        CountNewNotInCommunity = x["new_objects_not_in_community"].AsInt32
                    }).ToList();

                //loops through the results and creates a return reponse
                List<ConnectionsBreakdown> results = new List<ConnectionsBreakdown>();
                foreach(string deviceName in resultsFromMongo.Select(x => x.DeviceName).Distinct())
                {
                    ConnectionsBreakdown result;
                   
                    result = new ConnectionsBreakdown();
                    result.DeviceName = deviceName;
                    result.StartDate = dtStart;
                    result.EndDate = dtEnd;
                    result.Types = new List<ConnectionsBreakdownType>();
                    foreach(string type in types)
                    {
                        var currTypeFromMongoList = resultsFromMongo.Where(x => x.DeviceName == deviceName && x.Type == type).ToList();
                        if (currTypeFromMongoList.Count() == 0)
                        {
                            currTypeFromMongoList.Add(new
                            {
                                Type = type,
                                DeviceName = deviceName,
                                Count = 0,
                                CountInCommunity = 0,
                                CountNewInCommunity = 0,
                                CountNewNotInCommunity = 0
                            });
                        }
                        ConnectionsBreakdownType currType = new ConnectionsBreakdownType();
                        currType.IsInCommunity = false;
                        currType.NewCount = currTypeFromMongoList.First().CountNewNotInCommunity;
                        currType.Total = currTypeFromMongoList.First().Count - currTypeFromMongoList.First().CountInCommunity;
                        currType.Type = type;
                        result.Types.Add(currType);

                        if (type != "Community")
                        {
                            currType = new ConnectionsBreakdownType();
                            currType.IsInCommunity = true;
                            currType.NewCount = currTypeFromMongoList.First().CountNewInCommunity;
                            currType.Total = currTypeFromMongoList.First().CountInCommunity;
                            currType.Type = "Community " + type;
                            result.Types.Add(currType);
                        }
                        

                    }
                    results.Add(result);
                }

                if (results.Count() > 1)
                {
                    var totalBreakdown = new ConnectionsBreakdown();
                    totalBreakdown.DeviceName = "All Environments";
                    totalBreakdown.StartDate = dtStart;
                    totalBreakdown.EndDate = dtEnd;
                    totalBreakdown.Types = new List<ConnectionsBreakdownType>();
                    foreach (string type in results[0].Types.Select(y => y.Type))
                    {
                        ConnectionsBreakdownType currType = new ConnectionsBreakdownType();
                        currType.IsInCommunity = type.StartsWith("Community ");
                        currType.NewCount = results.Sum(x => x.Types.Where(y => y.IsInCommunity == currType.IsInCommunity && y.Type == type).First().NewCount);
                        currType.Total = results.Sum(x => x.Types.Where(y => y.IsInCommunity == currType.IsInCommunity && y.Type == type).First().Total);
                        currType.Type = type;
                        totalBreakdown.Types.Add(currType);
                        
                    }

                    results.Insert(0, totalBreakdown);
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
    }
    
}
