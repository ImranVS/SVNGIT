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
        private IRepository<SummaryStatistics> summaryRepository;
        private string DateFormat = "yyyy-MM-dd";

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
                    filterDef = summaryRepository.Filter.And(summaryRepository.Filter.Gte(p => p.CreatedOn, dtStart),
                    summaryRepository.Filter.Lt(p => p.CreatedOn, dtEnd),
                    summaryRepository.Filter.Ne(p => p.DeviceName, null),
                    summaryRepository.Filter.Regex(p => p.StatName, new BsonRegularExpression("/Disk.*Free/i")));
                }
                else
                {
                    filterDef = summaryRepository.Filter.And(summaryRepository.Filter.Gte(p => p.CreatedOn, dtStart),
                    summaryRepository.Filter.Lt(p => p.CreatedOn, dtEnd),
                    summaryRepository.Filter.Ne(p => p.DeviceName, null),
                    summaryRepository.Filter.Eq(p => p.DeviceId, deviceId),
                    summaryRepository.Filter.Regex(p => p.StatName, new BsonRegularExpression("/Disk.*Free/i")));
                }
                var summarylist = summaryRepository.Find(filterDef).OrderBy(p => p.CreatedOn).ToList();

                var result = summarylist
                    .GroupBy(row => new
                    {
                        row.CreatedOn.Year,
                        row.CreatedOn.Month,
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
            string startDate = DateTime.Now.AddDays(-30).ToString(DateFormat);

            DateTime dtStart = DateTime.Parse(startDate);

            dtStart = DateTime.SpecifyKind(dtStart, DateTimeKind.Utc);

            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            summaryRepository = new Repository<SummaryStatistics>(ConnectionString);
            Repository<Server> serverRepository = new Repository<Server>(ConnectionString);
            List<Server> serverlist = null;
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    filterDef = summaryRepository.Filter.And(summaryRepository.Filter.Gte(p => p.CreatedOn, dtStart),
                    summaryRepository.Filter.Ne(p => p.DeviceName, null),
                    summaryRepository.Filter.Eq(p => p.StatName, statName));
                }
                else
                {
                    filterDef = summaryRepository.Filter.And(summaryRepository.Filter.Gte(p => p.CreatedOn, dtStart),
                    summaryRepository.Filter.Ne(p => p.DeviceName, null),
                    summaryRepository.Filter.Eq(p => p.DeviceId, deviceId),
                    summaryRepository.Filter.Eq(p => p.StatName, statName));
                }
                var summarylist = summaryRepository.Find(filterDef).OrderBy(p => p.CreatedOn).ToList();
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
                    startDate = DateTime.Now.AddDays(-7).ToString(DateFormat);

                if (endDate == "")
                    endDate = DateTime.Today.ToString(DateFormat);

                //1 day is added to the end so we include that days data
                DateTime dtStart = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture);
                DateTime dtEnd = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture).AddDays(1);

                dtStart = DateTime.SpecifyKind(dtStart, DateTimeKind.Utc);
                dtEnd = DateTime.SpecifyKind(dtEnd, DateTimeKind.Utc);

                summaryRepository = new Repository<SummaryStatistics>(ConnectionString);

                var filterDef = summaryRepository.Filter.Eq(x => x.DeviceType, type) &
                    summaryRepository.Filter.Eq(x => x.StatName, statName) &
                    summaryRepository.Filter.Gte(p => p.CreatedOn, dtStart) &
                    summaryRepository.Filter.Lte(p => p.CreatedOn, dtEnd);

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
                        expandoObj.Add(entity.CreatedOn.ToString("MM/dd/yyyy"), entity.StatValue);
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
        public APIResponse GetSumamryStatsChart(string statName, string deviceId = "", string startDate = "", string endDate = "")
        {
            FilterDefinition<SummaryStatistics> filterDef = null;
            if (startDate == "")
                startDate = DateTime.Now.AddDays(-28).ToString(DateFormat);

            if (endDate == "")
                endDate = DateTime.Today.ToString(DateFormat);

            //1 day is added to the end so we include that days data
            DateTime dtStart = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture);
            DateTime dtEnd = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture).AddDays(1);

            dtStart = DateTime.SpecifyKind(dtStart, DateTimeKind.Utc);
            dtEnd = DateTime.SpecifyKind(dtEnd, DateTimeKind.Utc);

            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            summaryRepository = new Repository<SummaryStatistics>(ConnectionString);

            List<String> listOfDevices;

            if (string.IsNullOrWhiteSpace(deviceId))
            {
                listOfDevices = new List<string>();
            }
            else
            {
                listOfDevices = deviceId.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',').ToList();
            }

            try
            {
                filterDef = summaryRepository.Filter.Gte(p => p.CreatedOn, dtStart) &
                    summaryRepository.Filter.Lte(p => p.CreatedOn, dtEnd) &
                    summaryRepository.Filter.Eq(p => p.StatName, statName);

                if (listOfDevices.Count > 0)
                {
                    filterDef = filterDef & summaryRepository.Filter.In(p => p.DeviceId, listOfDevices);
                }
                var result = summaryRepository.Find(filterDef).OrderBy(p => p.CreatedOn).ToList();

                List<Serie> series = new List<Serie>();

                foreach (var currDeviceId in result.Select(x => x.DeviceId).Distinct())
                {
                    var currList = result.Where(x => x.DeviceId == currDeviceId).ToList();
                    Serie serie = new Serie();
                    serie.Segments = new List<Segment>();
                    serie.Title = currList[0].DeviceName;

                    for (DateTime date = dtStart.Date; date.Date < dtEnd.Date; date = date.AddDays(1))
                    {
                        var item = currList.Where(x => x.CreatedOn.Date == date.Date).ToList();

                        serie.Segments.Add(new Segment() { Label = date.Date.ToString(DateFormat), Value = item.Count > 0 ? (double?) item[0].StatValue : null });
                        
                        series.Add(serie);
                    }
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

    }
}
