using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VitalSigns.API.Models;
using VitalSigns.API.Models.Charts;
using VSNext.Mongo.Repository;
using VSNext.Mongo.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Globalization;
using MongoDB.Bson;

namespace VitalSigns.API.Controllers
{

    [Route("[controller]")]
    public class ServicesController : BaseController
    {

        private IRepository<Status> statusRepository;
        private IRepository<DailyStatistics> dailyRepository;
        private IRepository<SummaryStatistics> summaryRepository;
        private IRepository<Drive> diskRepository;


        private IRepository<IbmConnectionsTopStats> ibmRepository;




        [HttpGet("dashboard_summary")]
        public APIResponse ServersStatusSummary()
        {
            statusRepository = new Repository<Status>(ConnectionString);
            var result = statusRepository.Collection.Aggregate()
                                               .Group(x => x.StatusCode, g => new { label = g.Key, value = g.Count() })
                                               .Project(x => new
                                               {
                                                   Label = x.label,
                                                   Value = x.value
                                               }).ToList();
            var issue = result.Where(item => item.Label == "Issue").FirstOrDefault().Value;
            var ok = result.Where(item => item.Label == "OK").FirstOrDefault().Value; ;
            var notResponding = result.Where(item => item.Label == "Not Responding").FirstOrDefault().Value;
            var maintenance = result.Where(item => item.Label == "Maintenance").FirstOrDefault().Value;
            return Common.CreateResponse(new { issue = issue, ok = ok, notResponding = notResponding, maintenance = maintenance });
        }

        [HttpGet("status_summary_by_type")]
        public IEnumerable<StatusSummary> GetStatusSummaryByType()
        {
            statusRepository = new Repository<Status>(ConnectionString);
            var result = statusRepository.All()
                                        .Select(x => new
                                        {
                                            DeviceType = x.DeviceType,
                                            StatusCode = x.StatusCode
                                        }).ToList();
            List<string> typeList = result.Select(x => x.DeviceType).Distinct().ToList();
            List<StatusSummary> summaryList = new List<StatusSummary>();
            foreach (string type in typeList)
            {
                summaryList.Add(new StatusSummary
                {
                    Type = type,
                    Ok = result.Where(x => x.DeviceType == type && x.StatusCode == "OK").Count(),
                    NotResponding = result.Where(x => x.DeviceType == type && x.StatusCode == "Not Responding").Count(),
                    Issue = result.Where(x => x.DeviceType == type && x.StatusCode == "Issue").Count(),
                    Maintenance = result.Where(x => x.DeviceType == type && x.StatusCode == "Maintenance").Count()
                });
            }
            return summaryList.Where(x => x.Type != null && x.Type != "Domino Cluster").ToList();
        }
        [HttpGet("dashboard_stats")]
        public APIResponse GetDashboardStats()
        {
            statusRepository = new Repository<Status>(ConnectionString);
            var result = statusRepository.All()
                                        .Select(x => new
                                        {
                                            UserCount = x.UserCount,
                                            ResponseTime = x.ResponseTime,
                                            DownMinutes = x.DownMinutes,
                                            PendingMail = x.PendingMail,
                                            DeadMail = x.DeadMail,
                                            HeldMail = x.HeldMail

                                        }).ToList();
            int? userCount = result.Sum(x => x.UserCount);
            double? responseTime = result.Average(x => x.ResponseTime);
            double? downMinutes = result.Sum(x => x.DownMinutes);
            int? pendingMail = result.Sum(x => x.PendingMail);
            int? deadMail = result.Sum(x => x.DeadMail);
            int? heldMail = result.Sum(x => x.HeldMail);




            return Common.CreateResponse(new { user_count = userCount, response_time = responseTime, downMinutes = downMinutes, pendingMail = pendingMail, deadMail = deadMail, heldMail = heldMail });
        }
        /// <summary>
        /// Returns all servers details
        /// </summary>
        /// <author>Kiran Dadireddy</author>
        /// <returns>List of servers details</returns>
        [HttpGet("device_list")]
        public APIResponse GetAllServerServices()
        {
            statusRepository = new Repository<Status>(ConnectionString);
            try
            {
                var serviceIcons = Common.GetServerTypeIcons();
                var result = statusRepository.All().AsQueryable()
                                     .Select(x => new ServerStatus
                                     {
                                         Id = x.Id,
                                         Type = x.DeviceType,
                                         Country = x.Location,
                                         Name = x.DeviceName,
                                         Version = x.SoftwareVersion,
                                         LastUpdated = x.LastUpdated,
                                         Description = x.Description,
                                         Status = x.StatusCode,
                                         DeviceId = x.DeviceId

                                     }).ToList();
                foreach (ServerStatus item in result)
                {

                    if (item.LastUpdated.HasValue)
                        item.Description = "Last Updated: " + item.LastUpdated.Value.ToShortDateString();
                    if (item.Type != null)
                    {
                        if (serviceIcons.ContainsKey(item.Type))
                            item.Icon = serviceIcons[item.Type];
                        else
                            item.Icon = @"/img/servers/Paintbrush.svg";
                    }
                    if (!string.IsNullOrEmpty(item.Status))
                        item.Status = item.Status.ToLower().Replace(" ", "");
                }
                Response = Common.CreateResponse(result.OrderBy(x => x.Name));
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }
        /// <summary>
        /// Returns server details by id
        /// </summary>
        /// <author>Kiran Dadireddy</author>
        /// <param name="id"></param>
        /// <returns>Server details</returns>
        [HttpGet("device_details")]
        public APIResponse GetServerDetails(string device_id, string destination, string secondaryRole)
        {

            statusRepository = new Repository<Status>(ConnectionString);


            try
            {
                Expression<Func<Status, bool>> expression = (p => p.DeviceId == device_id);
                var result = (statusRepository.Find(expression)
                                     .Select(x => new ServerStatus
                                     {
                                         Id = x.Id,
                                         Type = x.DeviceType,
                                         Country = x.Location,
                                         Name = x.DeviceName,
                                         Version = x.SoftwareVersion,
                                         LastUpdated = x.LastUpdated,
                                         Description = x.Description,
                                         Status = x.StatusCode,
                                         DeviceId = x.DeviceId,
                                         SecondaryRole = x.SecondaryRole


                                     })).FirstOrDefault();
                var serviceIcons = Common.GetServerTypeIcons();
                Models.ServerType serverType = Common.GetServerTypeTabs(result.Type);

                if (string.IsNullOrEmpty(result.SecondaryRole))
                    result.Tabs = serverType.Tabs.Where(x => x.Type.ToUpper() == destination.ToUpper() && x.SecondaryRole == null).ToList();
                else
                {
                    var secondaryRoles = result.SecondaryRole.Split(';').Select(x=>x.Trim());
                    result.Tabs = serverType.Tabs.Where(x => x.Type.ToUpper() == destination.ToUpper() && (x.SecondaryRole == null || secondaryRoles.Contains(x.SecondaryRole))).ToList();
                }


                result.Description = "Last Updated: " + result.LastUpdated.Value.ToShortDateString();
                result.Icon = serverType.Icon;
                if (!string.IsNullOrEmpty(result.Status))
                    result.Status = result.Status.ToLower().Replace(" ", "");
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }


        /// <summary>
        /// Returns daily stats data by deviceid
        /// </summary>
        /// <author>Swathi Dongari</author>
        /// <param name="id"></param>
        /// <returns> daily stats data </returns>
        [HttpGet("statistics")]
        public APIResponse GetDailyStat(string deviceId, string statName, string operation)
        {

            dailyRepository = new Repository<DailyStatistics>(ConnectionString);

            try
            {
                var statNames = statName.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');
                if (string.IsNullOrEmpty(deviceId) && !string.IsNullOrEmpty(statName))
                {


                    Expression<Func<DailyStatistics, bool>> expression = (p => statNames.Contains(p.StatName));
                    var result = dailyRepository.Find(expression).Select(x => new StatsData
                    {
                        DeviceId = x.DeviceId,
                        StatName = x.StatName,
                        StatValue = x.StatValue

                    }).OrderBy(x => x.StatName).ToList();


                    Response = Common.CreateResponse(result);

                }
                else
                {
                    Expression<Func<DailyStatistics, bool>> expression = (p => statNames.Contains(p.StatName) && p.DeviceId == deviceId);

                    if (string.IsNullOrEmpty(operation) && !string.IsNullOrEmpty(statName))
                    {



                        var result = dailyRepository.Find(expression).Select(x => new StatsData
                        {
                            DeviceId = x.DeviceId,
                            StatName = x.StatName,
                            StatValue = x.StatValue

                        }).OrderBy(x => x.StatName).ToList();

                        Response = Common.CreateResponse(result);

                        return Response;
                    }
                    else if (!string.IsNullOrEmpty(operation) && !string.IsNullOrEmpty(statName))
                    {

                        // StatsData statsData;


                        switch (operation.ToUpper())
                        {
                            case "SUM":

                                var statsSum = dailyRepository.Find(expression);

                                var statsSumData = statsSum
                                              .GroupBy(row => row.StatName)
                                              .Select(grp => new
                                              {
                                                  StatName = grp.Key,
                                                  Value = grp.Sum(x => x.StatValue),
                                                  DeviceId = deviceId
                                              }).ToList();

                                // statsData = new StatsData { StatName = statName, StatValue = statsSum, DeviceId = deviceId };
                                Response = Common.CreateResponse(statsSumData);

                                break;

                            case "AVG":

                                var statsAvg = dailyRepository.Find(expression);

                                var statsAvgData = statsAvg
                                          .GroupBy(row => row.StatName)
                                          .Select(grp => new
                                          {
                                              StatName = grp.Key,
                                              Value = grp.Average(x => x.StatValue),
                                              DeviceId = deviceId
                                          }).ToList();
                                Response = Common.CreateResponse(statsAvgData);
                                break;
                            case "COUNT":
                                var statsCount = dailyRepository.Find(expression);

                                var statsCountData = statsCount
                                       .GroupBy(row => row.StatName)
                                       .Select(grp => new
                                       {
                                           StatName = grp.Key,
                                           Value = grp.Count(),
                                           DeviceId = deviceId
                                       }).ToList();
                                Response = Common.CreateResponse(statsCountData);
                                break;
                            case "HOURLY":
                                var statsHourly = dailyRepository.Find(expression);


                                var result = statsHourly
                                       .GroupBy(row => new
                                       {
                                           row.CreatedOn.Hour,
                                           row.StatName

                                       })
                                       .Select(row => new
                                       {
                                           Hour = row.Key.Hour,
                                           Value = Math.Round(row.Average(x => x.StatValue), 2),
                                           StatName = row.Key.StatName

                                       }).ToList();






                                // DateTime moment = DateTime.Now.Hour;
                                // int onhour = moment.Hour;
                                List<Serie> series = new List<Serie>();

                                foreach (var name in statNames)
                                {

                                    List<Segment> segments = new List<Segment>();
                                    Serie serie = new Serie();
                                    for (int hour = 1; hour <= 24; hour++)
                                    {
                                        // To do
                                        // string hourString =hour<12?hour.ToString()+ " A.M " 

                                        var item = result.Where(x => x.Hour == hour).FirstOrDefault();
                                        var output = result.Where(x => x.Hour == hour && x.StatName == name).ToList();
                                        DateTime time = new DateTime();
                                        time = DateTime.Now.AddHours(-hour);
                                        time = time.AddMinutes(-1 * time.Minute);
                                        string displayTime = "";
                                        displayTime = time.ToString("hh:mm tt");


                                        if (item != null && statNames.Length == 1)
                                        {

                                            segments.Add(new Segment { Label = displayTime.ToString(), Value = item.Value });
                                            serie.Title = name;
                                            serie.Segments = segments;
                                        }
                                        else if (item != null && output != null && statNames.Length > 1)
                                        {
                                            foreach (var statvalue in output)
                                            {
                                                segments.Add(new Segment { Label = displayTime, Value = statvalue.Value });
                                                serie.Title = name;
                                                serie.Segments = segments;
                                            }

                                        }
                                        else
                                        {

                                            // TimeSpan timespan = new TimeSpan(hour);

                                            segments.Add(new Segment { Label = displayTime.ToString(), Value = 0 });
                                            serie.Title = name;
                                            serie.Segments = segments;

                                        }





                                    }
                                    series.Add(serie);
                                }

                                Chart chart = new Chart();
                                chart.Title = statName;
                                chart.Series = series;



                                Response = Common.CreateResponse(chart);
                                break;
                        }


                    }
                }
                return Response;



            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        /// <summary>
        /// Returns summary stats data by deviceid
        /// </summary>
        /// <author>Swathi Dongari</author>
        /// <param name="id"></param>
        /// <returns> summary stats data </returns>
        [HttpGet("summarystats")]
        public APIResponse GetSummaryStat(string deviceId, string statName)
        {
            summaryRepository = new Repository<SummaryStatistics>(ConnectionString);
            var statNames = statName.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');
            try
            {
                if (string.IsNullOrEmpty(deviceId) && !string.IsNullOrEmpty(statName))
                {
                    Expression<Func<SummaryStatistics, bool>> expression = (p => statNames.Contains(p.StatName));
                    var result = summaryRepository.Find(expression).Select(x => new StatsData
                    {
                        //DeviceId = x.DeviceId,
                        StatName = x.StatName,
                        StatValue = x.StatValue

                    }).Take(500).ToList();
                    Response = Common.CreateResponse(result);

                }
                else if (!string.IsNullOrEmpty(deviceId) && !string.IsNullOrEmpty(statName))
                {

                    Expression<Func<SummaryStatistics, bool>> expression = (p => statNames.Contains(p.StatName) && p.DeviceId == deviceId);
                    var statsHourly = summaryRepository.Find(expression);
                    var result = statsHourly
                                       .GroupBy(row => new
                                       {
                                           row.CreatedOn.Date,
                                           row.StatName

                                       })
                                       .Select(row => new
                                       {
                                           Date = row.Key.Date,
                                           Value = Math.Round(row.Average(x => x.StatValue), 2),
                                           StatName = row.Key.StatName

                                       }).ToList();



                    DateTime now = DateTime.Now;
                    var startDate = new DateTime(now.Year, now.Month, 1);
                    var endDate = now;
                    List<Serie> series = new List<Serie>();
                    foreach (var name in statNames)
                    {

                        List<Segment> segments = new List<Segment>();
                        Serie serie = new Serie();



                        for (DateTime date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1))
                        {
                            var item = result.Where(x => x.Date == date).FirstOrDefault();
                            var output = result.Where(x => x.Date == date && x.StatName == name.ToString()).ToList();

                            string statdate = date.ToString("d-MMMM-yyyy", CultureInfo.InvariantCulture);
                            if (item != null && statNames.Length == 1)
                            {
                                segments.Add(new Segment { Label = statdate.ToString(), Value = item.Value });
                                serie.Title = name.ToString();
                                serie.Segments = segments;
                            }
                            else if (item != null && output != null && statNames.Length > 1)
                            {
                                foreach (var statvalue in output)
                                {
                                    segments.Add(new Segment { Label = statdate, Value = statvalue.Value });
                                    serie.Title = name.ToString();
                                    serie.Segments = segments;
                                }
                            }
                            else
                            {
                                segments.Add(new Segment { Label = statdate.ToString(), Value = 0 });
                                serie.Title = name.ToString();
                                serie.Segments = segments;

                            }



                        }
                        series.Add(serie);

                    }


                    Chart chart = new Chart();
                    chart.Title = statName;
                    chart.Series = series;
                    Response = Common.CreateResponse(chart);

                }
                return Response;

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("top_tags")]
        public APIResponse GetIbmConnection(string deviceId)
        {
            ibmRepository = new Repository<IbmConnectionsTopStats>(ConnectionString);
            try
            {
                if (!string.IsNullOrEmpty(deviceId))
                {

                    Expression<Func<IbmConnectionsTopStats, bool>> expression = (p => p.DeviceId == deviceId);
                    var topTags = ibmRepository.Find(expression);
                    var result = topTags
                                      .GroupBy(row => row.Name)
                                      .Select(grp => new Segment
                                      {
                                          Label = grp.Key,
                                          Value = grp.Count(),

                                      }).ToList();
                    List<Segment> segments = new List<Segment>();
                    Serie serie = new Serie();
                    //serie.Title = name;
                    serie.Segments = result.OrderBy(x => x.Value).Take(5).ToList();

                    List<Serie> series = new List<Serie>();
                    series.Add(serie);

                    Chart chart = new Chart();
                    // chart.Title = name;
                    chart.Series = series;
                    Response = Common.CreateResponse(chart);

                }
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }


        [HttpGet("status_list")]
        public APIResponse GetStatusList(string type)
        {
            statusRepository = new Repository<Status>(ConnectionString);

            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    var result = statusRepository.Collection.AsQueryable()
                                     .Select(x => new ServerStatus
                                     {

                                         DeviceId = x.DeviceId,
                                         Name = x.DeviceName,
                                         Status = x.StatusCode,
                                         Country = x.Location,
                                         Details = x.Details,
                                         UserCount = x.UserCount,
                                         CPU = x.CPU,
                                         Type = x.DeviceType
                                         // LastUpdated = x.LastUpdated,
                                         // Description = x.Description,


                                     });
                    Response = Common.CreateResponse(result);

                }
                else if (!string.IsNullOrEmpty(type))
                {

                    Expression<Func<Status, bool>> expression = (p => p.DeviceType == type);
                    var result = statusRepository.Find(expression).AsQueryable()
                                                                    .Select(x => new ServerStatus
                                                                    {

                                                                        // Id = x.Id,
                                                                        DeviceId = x.DeviceId,
                                                                        Name = x.DeviceName,
                                                                        Status = x.StatusCode,
                                                                        Country = x.Location,
                                                                        Details = x.Details,
                                                                        UserCount = x.UserCount,
                                                                        CPU = x.CPU,
                                                                        Type = x.DeviceType
                                                                        // LastUpdated = x.LastUpdated,
                                                                        // Description = x.Description,


                                                                    });

                    Response = Common.CreateResponse(result);







                    // Response = Common.CreateResponse(chart);
                    //Response = Common.CreateResponse(result);

                }
                return Response;

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("status_count")]
        public APIResponse GetStatusCount(string type, string docfield)
        {

            statusRepository = new Repository<Status>(ConnectionString);

            try
            {

                Expression<Func<Status, bool>> expression = (p => p.DeviceType == type);

                // var output = statusRepository.All().Where(x => x.DeviceType == type);
                if (type == "Domino")
                {
                    var result = statusRepository.Collection.Aggregate()
                                                       .Group(x => docfield, g => new { label = g.Key, value = g.Count() })
                                                       .Project(x => new Segment
                                                       {
                                                           Label = x.label,
                                                           Value = x.value
                                                       }).ToList();

                    // List<Status> result = List<Status>();

                    if (docfield == "operating_system")
                    {
                        result = statusRepository.Collection.Aggregate()
                                                       .Group(x => x.OperatingSystem, g => new { label = g.Key, value = g.Count() })
                                                       .Project(x => new Segment
                                                       {
                                                           Label = x.label,
                                                           Value = x.value
                                                       }).ToList();

                    }


                    else if (docfield == "software_version")
                    {
                        result = statusRepository.Collection.Aggregate()
                                                       .Group(x => x.SoftwareVersion, g => new { label = g.Key, value = g.Count() })
                                                       .Project(x => new Segment
                                                       {
                                                           Label = x.label,
                                                           Value = x.value
                                                       }).ToList();
                    }


                    else if (docfield == "status_code")
                    {
                        result = statusRepository.Collection.Aggregate()
                                                      .Group(x => x.StatusCode, g => new { label = g.Key, value = g.Count() })
                                                      .Project(x => new Segment
                                                      {
                                                          Label = x.label,
                                                          Value = x.value
                                                      }).ToList();
                    }
                    else if (docfield == "secondary_role")
                    {
                        result = statusRepository.Collection.Aggregate()
                                                      .Group(x => x.SecondaryRole, g => new { label = g.Key, value = g.Count() })
                                                      .Project(x => new Segment
                                                      {
                                                          Label = x.label,
                                                          Value = x.value
                                                      }).ToList();


                    }

                    result.RemoveAll(item => item.Label == null);
                    result.RemoveAll(item => item.Label == "");
                    Serie serie = new Serie();
                    serie.Title = docfield;
                    serie.Segments = result;


                    List<Serie> series = new List<Serie>();
                    series.Add(serie);

                    Chart chart = new Chart();
                    chart.Title = docfield;
                    chart.Series = series;

                    Response = Common.CreateResponse(chart);


                }

                return Response;
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }

        }

        [HttpGet("disk_space")]
        public APIResponse GetDiskSpace(string type, string deviceid)
        {
            try
            {
                statusRepository = new Repository<Status>(ConnectionString);
                Expression<Func<Status, bool>> diskexpression = (p => p.DeviceId == deviceid);
                var result = statusRepository.Find(diskexpression).FirstOrDefault();

                List<DiskSerie> diskserie = new List<DiskSerie>();
                List<string> name = new List<string>();
                List<double> diskfree = new List<double>();
                List<double> disksize = new List<double>();
                ServerDiskStatus serverDiskStatus = new ServerDiskStatus();
                serverDiskStatus.Id = result.DeviceId;
                foreach (Disk drive in result.Disks)
                {
                    serverDiskStatus.Drives.Add(new DiskDriveStatus
                    {
                        DiskFree = drive.DiskFree,
                        DiskSize = drive.DiskSize,
                        DiskName = drive.DiskName,
                        DiskUsed = drive.DiskSize - drive.DiskFree,

                        PercentFree = drive.PercentFree,
                        Threshold = drive.Threshold,

                    });

                    name.Add(drive.DiskName);
                    diskfree.Add(drive.DiskFree.HasValue ? (double)drive.DiskFree : 0);
                    disksize.Add(drive.DiskSize.HasValue ? (double)drive.DiskSize : 0);
 
                }
                diskserie.Add(new DiskSerie { Label = "Available", Value = diskfree });
                diskserie.Add(new DiskSerie { Label = "Used", Value = disksize });
                DiskChart chart = new DiskChart();
                chart.Title = "Disk Space";
                chart.Series = diskserie;
                chart.Categories = name;


                Response = Common.CreateResponse(chart);

                return Response;
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }

        }

        //  var diskresult = statusRepository.Find(typeexpression);

    }
}

